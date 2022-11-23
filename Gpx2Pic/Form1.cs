using Gpx;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ExifLib;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using FastFitParser.Core;
using System.Reflection;

namespace Gpx2Pic
{
    public partial class Form1 : Form
    {
        private GpxPointCollection<GpxPoint> gpxPoints;
        private bool Resizing = false;
        private Process myProcess = null;
        private readonly Regex reg = new Regex(@"{ready(\d+)}", RegexOptions.IgnoreCase);

        // https://stackoverflow.com/questions/1406887/only-change-a-listviewitems-checked-state-if-the-checkbox-is-clicked/1407230#1407230
        private bool inhibitAutoCheck;

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadFile(string fileName)
        {
            if (string.IsNullOrEmpty(textBoxGpxFile.Text) || !File.Exists(textBoxGpxFile.Text))
            {
                return;
            }

            try
            {
                if (Path.GetExtension(fileName).ToLower() == ".fit")
                {
                    LoadFit(fileName);
                }
                else
                {
                    LoadGpx(fileName);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFit(string fileName)
        {
            double SEMICIRCLES_TO_DEGREES = (180 / Math.Pow(2, 31));
            gpxPoints = gpxPoints == null ? new GpxPointCollection<GpxPoint>() : gpxPoints;
            gpxPoints.Clear();

            using (var stream = File.OpenRead(fileName))
            {
                var fastParser = new FastParser(stream);

                Debug.WriteLine("Fit file is valid: " + fastParser.IsFileValid());

                foreach (var dataRecord in fastParser.GetDataRecords())
                {
                    if (dataRecord.GlobalMessageNumber == GlobalMessageDecls.Record)
                    {
                        GpxPoint p = new GpxPoint();

                        if (dataRecord.TryGetField(RecordDef.TimeStamp, out DateTime timeStamp))
                        {
                            p.Time = timeStamp;
                        }
                        if (dataRecord.TryGetField(RecordDef.PositionLat, out double latitude))
                        {
                            p.Latitude = latitude * SEMICIRCLES_TO_DEGREES;
                        }
                        if (dataRecord.TryGetField(RecordDef.PositionLong, out double longitude))
                        {
                            p.Longitude = longitude * SEMICIRCLES_TO_DEGREES;
                        }

                        gpxPoints.Add(p);
                    }
                }

                Debug.WriteLine("Read {0} fit records", gpxPoints.Count);
            }
        }

        private void LoadGpx(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                using (GpxReader gpxReader = new GpxReader(fs))
                {
                    while (gpxReader.Read())
                    {
                        switch (gpxReader.ObjectType)
                        {
                            case GpxObjectType.Track:
                                gpxPoints = gpxReader.Track.ToGpxPoints();
                                break;
                        }
                    }
                }

                Debug.WriteLine("Read {0} gpx records", gpxPoints.Count);
            }
        }

        private void FillListView(string picturesFolder)
        {
            try
            {
                if (string.IsNullOrEmpty(textBoxPicturesFolder.Text) || !Directory.Exists(textBoxPicturesFolder.Text))
                {
                    listView1.Items.Clear();
                    return;
                }

                listView1.BeginUpdate();
                listView1.Items.Clear();

                string[] filter = new string[] { ".jpg", ".jpeg" };
                foreach (var f in Directory.EnumerateFiles(picturesFolder, "*.*", SearchOption.TopDirectoryOnly).Where(f => filter.Any(x => f.EndsWith(x, StringComparison.OrdinalIgnoreCase))))
                {
                    FileInfo fi = new FileInfo(f);
                    ListViewItem item = new ListViewItem(fi.Name);

                    item.SubItems[0].Name = "FileName";
                    item.SubItems[0].Tag = fi.FullName;

                    item.SubItems.Add((fi.Length / 1024.0 / 1024.0).ToString("N") + " MB").Name = "FileSize";
                    item.SubItems.Add("").Name = "Model";
                    item.SubItems.Add("").Name = "Taken";
                    item.SubItems.Add("").Name = "ImageSize";
                    item.SubItems.Add("").Name = "LatLong";

                    try
                    {
                        using (ExifReader reader = new ExifReader(f))
                        {
                            if (reader.GetTagValue(ExifTags.Model, out string model))
                            {
                                item.SubItems["Model"].Text = model;
                            }

                            ExifTags[] tags = new ExifTags[] { ExifTags.DateTimeOriginal, ExifTags.DateTimeDigitized, ExifTags.DateTime };
                            bool datePictureTakenAssigned = false;

                            foreach (ExifTags tag in tags)
                            {
                                if (reader.GetTagValue(tag, out DateTime datePictureTaken))
                                {
                                    DateTime newDatePictureTaken = datePictureTaken.AddSeconds((double)numericUpDownTimeOffset.Value);
                                    string text = datePictureTaken.ToString();
                                    if (numericUpDownTimeOffset.Value != 0)
                                    {
                                        text += " -> " + newDatePictureTaken.ToLongTimeString();
                                    }
                                    item.SubItems["Taken"].Text = text;
                                    item.Tag = FindNearestTrackPoint(newDatePictureTaken);

                                    datePictureTakenAssigned = true;
                                    break;
                                }
                            }
                            
                            if (datePictureTakenAssigned == false)
                            {
                                DateTime? datePictureTaken2 = ExtractTimeFromFileName(fi.Name);
                                if (datePictureTaken2.HasValue)
                                {
                                    DateTime newDatePictureTaken = datePictureTaken2.Value.AddSeconds((double)numericUpDownTimeOffset.Value);
                                    string text = datePictureTaken2.ToString();
                                    if (numericUpDownTimeOffset.Value != 0)
                                    {
                                        text += " -> " + newDatePictureTaken.ToLongTimeString();
                                    }
                                    item.SubItems["Taken"].Text = text;
                                    item.SubItems["Taken"].Tag = newDatePictureTaken;
                                    item.Tag = FindNearestTrackPoint(newDatePictureTaken);
                                }
                            }

                            if (reader.GetTagValue(ExifTags.PixelXDimension, out uint width) && reader.GetTagValue(ExifTags.PixelYDimension, out uint height))
                            {
                                item.SubItems["ImageSize"].Text = string.Format("{0} x {1}", width, height);
                            }
                            
                            if (item.Tag is GpxPoint point)
                            {
                                item.SubItems["LatLong"].Text = string.Format(CultureInfo.InvariantCulture, "{0:F6}, {1:F6}", point.Latitude, point.Longitude);
                            }
                        }    
                    }
                    catch
                    {
                    }

                    item.Checked = item.Tag != null;
                    listView1.Items.Add(item);
                }

                listView1.EndUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1_Resize(listView1, EventArgs.Empty);

            var descriptionAttribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyDescriptionAttribute>();
            Text = string.Format("{0} - {1} (v{2})", Application.ProductName, descriptionAttribute.Description, Application.ProductVersion.Substring(0, Application.ProductVersion.Length - 2));
        }

        private void MyProcess_Exited(object sender, EventArgs e)
        {
            Debug.WriteLine("Process exited");
        }

        private GpxPoint FindNearestTrackPoint(DateTime localDateTime)
        {
            try
            {
                //var watch = Stopwatch.StartNew();
                DateTime utcDateTime = localDateTime.ToUniversalTime();

                //GpxPoint closestPoint = gpxPoints.OrderBy(x => Math.Abs((x.Time - utcDateTime).Value.TotalSeconds)).FirstOrDefault();

                GpxPoint closestPoint = null;
                long min = long.MaxValue;

                foreach(GpxPoint p in gpxPoints)
                {
                    long diff = (long)Math.Abs((p.Time - utcDateTime).Value.TotalSeconds);
                    if (min > diff)
                    {
                        min = diff;
                        closestPoint = p;
                    }
                }

                //watch.Stop();
                //Debug.WriteLine("Sort Time taken: {0} ms", watch.Elapsed.TotalMilliseconds);
                
                if (Math.Abs((closestPoint.Time - utcDateTime).Value.TotalSeconds) > (int)Properties.Settings.Default.ErrorMargin)
                {
                    return null;
                }
                else
                {
                    return closestPoint;
                }
            }
            catch
            {
                return null;
            }
        }
        
        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            buttonSave.Enabled = listView1.CheckedItems.Count > 0;
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            listView1.Items.Cast<ListViewItem>().ToList().ForEach(x => x.Checked = true);
        }

        private void buttonDeselectAll_Click(object sender, EventArgs e)
        {
            listView1.Items.Cast<ListViewItem>().ToList().ForEach(x => x.Checked = false);
        }

        private void buttonInverseSelection_Click(object sender, EventArgs e)
        {
            listView1.Items.Cast<ListViewItem>().ToList().ForEach(x => x.Checked = !x.Checked);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy == false)
            {
                listView1.Items.Cast<ListViewItem>().ToList().ForEach(x => { x.BackColor = Color.White; x.ForeColor = Color.Black; });
                buttonSave.Text = "Cancel";
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                if (MessageBox.Show("Operation in progress. Cancel?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    backgroundWorker1.CancelAsync();
                    //StopProcess();
                }
            }
        }

        private void listView1_Resize(object sender, EventArgs e)
        {
            // Don't allow overlapping of SizeChanged calls
            if (!Resizing)
            {
                // Set the resizing flag
                Resizing = true;

                if (sender is ListView listView)
                {
                    float totalColumnWidth = 0;

                    // Get the sum of all column tags
                    for (int i = 0; i < listView.Columns.Count; i++)
                        totalColumnWidth += Convert.ToInt32(listView.Columns[i].Tag);

                    // Calculate the percentage of space each column should 
                    // occupy in reference to the other columns and then set the 
                    // width of the column to that percentage of the visible space.
                    for (int i = 0; i < listView.Columns.Count; i++)
                    {
                        float colPercentage = (Convert.ToInt32(listView.Columns[i].Tag) / totalColumnWidth);
                        listView.Columns[i].Width = (int)(colPercentage * listView.ClientRectangle.Width);
                    }
                }
            }

            // Clear the resizing flag
            Resizing = false;
        }

        private void textBoxGpxFile_TextChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = !(string.IsNullOrEmpty(textBoxGpxFile.Text) || string.IsNullOrEmpty(textBoxPicturesFolder.Text));
            textBoxPicturesFolder.Text = Path.GetDirectoryName(textBoxGpxFile.Text);
        }

        private void textBoxPicturesFolder_TextChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = !(string.IsNullOrEmpty(textBoxGpxFile.Text) || string.IsNullOrEmpty(textBoxPicturesFolder.Text));
            Analyze();
        }

        private void textBoxGpxFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.GpxFileName = openFileDialog1.FileName;
                LoadFile(openFileDialog1.FileName);
            }
        }

        private void textBoxPicturesFolder_Click(object sender, EventArgs e)
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            })
            {
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    Properties.Settings.Default.PicturesFolder = dialog.FileName;
                }
            }
        }

        private void buttonAnalyze_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxGpxFile.Text) || !File.Exists(textBoxGpxFile.Text))
            {
                MessageBox.Show("Select an existing GPX file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrEmpty(textBoxPicturesFolder.Text) || !Directory.Exists(textBoxPicturesFolder.Text))
            {
                MessageBox.Show("Select an existing pictures folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Analyze();
            }
        }

        private void Analyze()
        {
            LoadFile(textBoxGpxFile.Text);
            FillListView(textBoxPicturesFolder.Text);
        }

        private void buttonShowUpInGoogleMaps_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                if (listView1.SelectedItems[0].Tag is GpxPoint p)
                {
                    Process.Start(string.Format(CultureInfo.InvariantCulture, "https://www.google.com/maps/search/?api=1&query={0},{1}", p.Latitude, p.Longitude));
                }
                else
                {
                    MessageBox.Show("No latitude or longitude.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Select item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Gpx2Pic by Mariusz Kacki.");
            sb.AppendLine();
            sb.AppendLine("We using resources listed below:");
            sb.AppendLine();

            sb.AppendLine("  - ExifTool by Phil Harvey,");
            sb.AppendLine("  - ExifLib by Simon McKenzie,");
            sb.AppendLine("  - Windows API Code Pack by rpastric,");
            sb.AppendLine("  - GPX by dlg.krakow.pl,");
            sb.AppendLine("  - FastFitParser John Lam,");
            sb.AppendLine("  - Dynamically Sizing Columns by Nick Olsen.");

            MessageBox.Show(sb.ToString(), "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var watch = Stopwatch.StartNew();

            try
            {
                Debug.WriteLine("Start process");

                int affected = 0;

                if (myProcess != null && myProcess.HasExited == false)
                {
                    myProcess.Kill();
                }

                List<ListViewItem> items = (List<ListViewItem>)listView1.Invoke(new Func<List<ListViewItem>>(() =>
                {
                    return listView1.CheckedItems.Cast<ListViewItem>().ToList();
                }));

                myProcess = new Process();
                myProcess.Exited += MyProcess_Exited;
                myProcess.OutputDataReceived += (o, ee) =>
                {
                    Debug.WriteLine("Output: " + ee.Data);

                    if (string.IsNullOrEmpty(ee.Data) == false)
                    {
                        var match = reg.Match(ee.Data);
                        if (match.Success)
                        {
                            affected++;
                            int index = int.Parse(match.Groups[1].Value);
                            backgroundWorker1.ReportProgress(0, index);
                        }
                    }
                };

                myProcess.ErrorDataReceived += (o, ee) => Debug.WriteLine("Error: " + ee.Data);

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "exiftool.exe",
                    Arguments = "-stay_open True -@ -",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                myProcess.StartInfo = startInfo;
                myProcess.Start();

                myProcess.BeginOutputReadLine();
                myProcess.BeginErrorReadLine();

                foreach (var item in items)
                {
                    if (backgroundWorker1.CancellationPending)
                    {
                        e.Cancel = true;
                        Debug.WriteLine("Cancel task");
                        break;
                    }

                    if (item.Tag is GpxPoint point)
                    {
                        string fileName = item.SubItems["FileName"].Tag as string;
                        string command = string.Format(CultureInfo.InvariantCulture, "{0}\r\n-GPSLatitude={1}\r\n-GPSLongitude={2}\r\n-GPSLatitudeRef={3}\r\n-GPSLongitudeRef={4}" + (Properties.Settings.Default.BackupOriginal ? "" : "\r\n-overwrite_original"), fileName, point.Latitude, point.Longitude, point.Latitude >= 0 ? "North" : "South", point.Longitude >= 0 ? "East" : "West");

                        int index = (int)listView1.Invoke(new Func<int>(() =>
                        {
                            return item.Index;
                        }));

                        myProcess.StandardInput.WriteLine(command);
                        myProcess.StandardInput.WriteLine("-execute" + index);
                    }
                }

                myProcess.StandardInput.WriteLine("-stay_open");
                myProcess.StandardInput.WriteLine("False");
                myProcess.StandardInput.Close();

                myProcess.WaitForExit();
                e.Result = affected;

                Debug.WriteLine("End process");
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }

            watch.Stop();
            Debug.WriteLine("Time taken: {0:00}:{1:00}:{2:00}", watch.Elapsed.Hours, watch.Elapsed.Minutes, watch.Elapsed.Seconds);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("The operation has been cancelled.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (e.Error != null)
            {
                MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Result is Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (e.Result == null)
                {
                    MessageBox.Show("The files has been modified.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    int affectedFiles = (int)e.Result;
                    int totalFiles = listView1.CheckedItems.Count;
                    MessageBox.Show(string.Format("The files has been modified ({0}/{1}).", affectedFiles, totalFiles), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            buttonSave.Text = "Save GPS";
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.UserState is int index)
            {
                ListViewItem item = listView1.Items[index];
                item.BackColor = Color.LightGreen;
                item.EnsureVisible();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
            StopTask();
        }

        private DateTime? ExtractTimeFromFileName(string fileName)
        {
            string[] tokens = Path.GetFileNameWithoutExtension(fileName).Split('_');
            string timeString = "";

            if (tokens.Count() >= 3)
            {
                timeString = tokens[1] + tokens[2];
            }
            else if (tokens.Count() >= 2)
            {
                timeString = tokens[0] + tokens[1];
            }

            if (tokens.Count() >= 3)
            {
                try
                {
                    if (DateTime.TryParseExact(timeString, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datePictureTaken))
                    {
                        return datePictureTaken;
                    }
                }
                catch { }
            }
            return null;
        }

        private void StopTask()
        {
            try
            {
                if (backgroundWorker1.IsBusy)
                {
                    if (MessageBox.Show("Operation in progress. Cancel?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        backgroundWorker1.CancelAsync();
                    }
                }
            }
            catch { }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = listView1.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                string fileName = item.SubItems["FileName"].Tag as string;
                if (File.Exists(fileName))
                {
                    Process.Start(fileName);
                }
            }
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            inhibitAutoCheck = true;
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            inhibitAutoCheck = false;
        }

        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (inhibitAutoCheck)
            {
                e.NewValue = e.CurrentValue;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Analyze();
        }
    }
}
