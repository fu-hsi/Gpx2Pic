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

namespace Gpx2Pic
{
    public partial class Form1 : Form
    {
        GpxPointCollection<GpxPoint> gpxPoints;
        private bool Resizing = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadGpx(string fileName)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        private void FillListView(string picturesFolder)
        {
            try
            {
                listView1.BeginUpdate();
                listView1.Items.Clear();

                string[] filter = new string[] { ".jpg", ".jpeg" };
                foreach (var f in Directory.EnumerateFiles(picturesFolder, "*.*", SearchOption.TopDirectoryOnly).Where(f => filter.Any(x => f.EndsWith(x, StringComparison.OrdinalIgnoreCase))))
                {
                    ListViewItem item = new ListViewItem(Path.GetFileName(f));
                    FileInfo fi = new FileInfo(f);
                    item.SubItems.Add((fi.Length / 1024.0 / 1024.0).ToString("N") + " MB").Name = "FileSize";

                    using (ExifReader reader = new ExifReader(f))
                    {
                        bool isValid = true;

                        if (reader.GetTagValue(ExifTags.Model, out string model))
                        {
                            item.SubItems.Add(model.ToString()).Name = "Model";
                        }
                        else
                        {
                            item.SubItems.Add("").Name = "Model";
                        }

                        if (reader.GetTagValue(ExifTags.DateTimeOriginal, out DateTime datePictureTaken))
                        {
                            item.SubItems.Add(datePictureTaken.ToString()).Name = "Taken";
                            item.Tag = FindNearestTrackPoint(datePictureTaken);
                        }
                        else
                        {
                            item.SubItems.Add("").Name = "Taken";
                            isValid = false;
                        }

                        if (reader.GetTagValue(ExifTags.PixelXDimension, out uint width) && reader.GetTagValue(ExifTags.PixelYDimension, out uint height))
                        {
                            item.SubItems.Add(string.Format("{0} x {1}", width, height)).Name = "ImageSize";
                        }
                        else
                        {
                            item.SubItems.Add("").Name = "ImageSize";
                        }

                        if (item.Tag is GpxPoint point)
                        {
                            item.SubItems.Add(string.Format(CultureInfo.InvariantCulture, "{0}, {1}", point.Latitude, point.Longitude)).Name = "LatLong";
                        }
                        else
                        {
                            item.SubItems.Add("").Name = "LatLong";
                            isValid = false;
                        }

                        item.Checked = isValid;
                    }

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
            Text = string.Format("{0} - Automatically geotag your photos (v{1})", Application.ProductName, Application.ProductVersion.Substring(0, Application.ProductVersion.Length - 2));

            if (!(string.IsNullOrEmpty(Properties.Settings.Default.GpxFileName) || string.IsNullOrEmpty(Properties.Settings.Default.PicturesFolder)))
            {
                Analyze(Properties.Settings.Default.GpxFileName, Properties.Settings.Default.PicturesFolder);
            }
        }

        private bool GeoTagPhoto(string fileName, double latitude, double longitude)
        {
            try
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "exiftool.exe",
                    Arguments = String.Format(CultureInfo.InvariantCulture, "\"{0}\" -GPSLatitude=\"{1}\" -GPSLongitude=\"{2}\" -GPSLatitudeRef=\"{3}\" -GPSLongitudeRef=\"{4}\"", fileName, latitude, longitude, latitude >= 0 ? "North" : "South", longitude >= 0 ? "East" : "West"),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit(5000);

                if (process.HasExited)
                {
                    return process.ExitCode == 0;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private GpxPoint FindNearestTrackPoint(DateTime localDateTime)
        {
            try
            {
                int errorMargin = 60 * 5;
                DateTime utcDateTime = localDateTime.ToUniversalTime();

                GpxPoint closestGpx = gpxPoints.OrderBy(x => Math.Abs((x.Time - utcDateTime).Value.TotalSeconds)).FirstOrDefault();

                if (Math.Abs((closestGpx.Time - utcDateTime).Value.TotalSeconds) > errorMargin)
                {
                    return null;
                }
                else
                {
                    return closestGpx;
                }
            }
            catch (Exception)
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
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                if (MessageBox.Show("Operation in progress. Cancel?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    backgroundWorker1.CancelAsync();
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
            Properties.Settings.Default.Save();
        }

        private void textBoxGpxFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.GpxFileName = openFileDialog1.FileName;
                LoadGpx(openFileDialog1.FileName);
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
            if (string.IsNullOrEmpty(textBoxGpxFile.Text))
            {
                MessageBox.Show("Select GPX file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrEmpty(textBoxPicturesFolder.Text))
            {
                MessageBox.Show("Select pictures folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Analyze(textBoxGpxFile.Text, textBoxPicturesFolder.Text);
            }
        }

        private void Analyze(string gpxFileName, string picturesFolder)
        {
            LoadGpx(textBoxGpxFile.Text);
            FillListView(textBoxPicturesFolder.Text);
        }

        private void buttonShowUpInGoogleMaps_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string latLong = listView1.SelectedItems[0].SubItems["LatLong"].Text;
                if (!string.IsNullOrEmpty(latLong))
                {
                    Process.Start(string.Format("https://www.google.com/maps/search/?api=1&query={0}", latLong.Replace(" ", "")));
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
            sb.AppendLine("  - Dynamically Sizing Columns by Nick Olsen.");

            MessageBox.Show(sb.ToString(), "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                int affected = 0;

                List<ListViewItem> items = (List<ListViewItem>)listView1.Invoke(new Func<List<ListViewItem>>(() =>
                {
                    return listView1.CheckedItems.Cast<ListViewItem>().ToList();
                }));

                foreach (var item in items)
                {
                    if (backgroundWorker1.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        if (item.Tag is GpxPoint point)
                        {
                            string fileName = Path.Combine(textBoxPicturesFolder.Text, item.Text);
                            bool result = GeoTagPhoto(Path.Combine(textBoxPicturesFolder.Text, fileName), point.Latitude, point.Longitude);

                            if (result)
                            {
                                affected++;
                                e.Result = affected;
                                item.BackColor = Color.LightGreen;
                            }
                            else
                            {
                                item.BackColor = Color.LightPink;
                            }
                        }

                        backgroundWorker1.ReportProgress(0, item);
                    }
                }
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
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
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.UserState is ListViewItem item)
            {
                item.EnsureVisible();
            }
        }
    }
}
