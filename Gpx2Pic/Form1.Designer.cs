namespace Gpx2Pic
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonSave = new System.Windows.Forms.Button();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.checkBoxBackupOriginal = new System.Windows.Forms.CheckBox();
            this.buttonAbout = new System.Windows.Forms.Button();
            this.buttonShowUpInGoogleMaps = new System.Windows.Forms.Button();
            this.buttonInverseSelection = new System.Windows.Forms.Button();
            this.buttonDeselectAll = new System.Windows.Forms.Button();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.buttonAnalyze = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.numericUpDownErrorMargin = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownTimeOffset = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxGpxFile = new System.Windows.Forms.TextBox();
            this.textBoxPicturesFolder = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panelButtons.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownErrorMargin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(10, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(764, 249);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listView1_ItemCheck);
            this.listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            this.listView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDown);
            this.listView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseUp);
            this.listView1.Resize += new System.EventHandler(this.listView1_Resize);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Tag = "2";
            this.columnHeader1.Text = "File Name";
            this.columnHeader1.Width = 177;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Tag = "1";
            this.columnHeader2.Text = "File Size";
            this.columnHeader2.Width = 102;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Tag = "1";
            this.columnHeader3.Text = "Model Name";
            this.columnHeader3.Width = 113;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Tag = "2";
            this.columnHeader4.Text = "Taken";
            this.columnHeader4.Width = 111;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Tag = "1";
            this.columnHeader5.Text = "Image Size";
            this.columnHeader5.Width = 126;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Tag = "2";
            this.columnHeader6.Text = "Location";
            this.columnHeader6.Width = 127;
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.AutoSize = true;
            this.buttonSave.Enabled = false;
            this.buttonSave.ForeColor = System.Drawing.Color.Red;
            this.buttonSave.Location = new System.Drawing.Point(694, 11);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(80, 31);
            this.buttonSave.TabIndex = 10;
            this.buttonSave.Text = "Save EXIF";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // panelButtons
            // 
            this.panelButtons.AutoSize = true;
            this.panelButtons.Controls.Add(this.checkBoxBackupOriginal);
            this.panelButtons.Controls.Add(this.buttonAbout);
            this.panelButtons.Controls.Add(this.buttonShowUpInGoogleMaps);
            this.panelButtons.Controls.Add(this.buttonInverseSelection);
            this.panelButtons.Controls.Add(this.buttonDeselectAll);
            this.panelButtons.Controls.Add(this.buttonSelectAll);
            this.panelButtons.Controls.Add(this.buttonSave);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 386);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new System.Windows.Forms.Padding(10);
            this.panelButtons.Size = new System.Drawing.Size(784, 55);
            this.panelButtons.TabIndex = 11;
            // 
            // checkBoxBackupOriginal
            // 
            this.checkBoxBackupOriginal.AutoSize = true;
            this.checkBoxBackupOriginal.Checked = global::Gpx2Pic.Properties.Settings.Default.BackupOriginal;
            this.checkBoxBackupOriginal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxBackupOriginal.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Gpx2Pic.Properties.Settings.Default, "BackupOriginal", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxBackupOriginal.Location = new System.Drawing.Point(408, 19);
            this.checkBoxBackupOriginal.Name = "checkBoxBackupOriginal";
            this.checkBoxBackupOriginal.Size = new System.Drawing.Size(63, 17);
            this.checkBoxBackupOriginal.TabIndex = 16;
            this.checkBoxBackupOriginal.Text = "Backup";
            this.checkBoxBackupOriginal.UseVisualStyleBackColor = true;
            // 
            // buttonAbout
            // 
            this.buttonAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbout.AutoSize = true;
            this.buttonAbout.ForeColor = System.Drawing.Color.Black;
            this.buttonAbout.Location = new System.Drawing.Point(608, 11);
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(80, 31);
            this.buttonAbout.TabIndex = 15;
            this.buttonAbout.Text = "About";
            this.buttonAbout.UseVisualStyleBackColor = true;
            this.buttonAbout.Click += new System.EventHandler(this.buttonAbout_Click);
            // 
            // buttonShowUpInGoogleMaps
            // 
            this.buttonShowUpInGoogleMaps.AutoSize = true;
            this.buttonShowUpInGoogleMaps.Location = new System.Drawing.Point(285, 11);
            this.buttonShowUpInGoogleMaps.Name = "buttonShowUpInGoogleMaps";
            this.buttonShowUpInGoogleMaps.Size = new System.Drawing.Size(117, 31);
            this.buttonShowUpInGoogleMaps.TabIndex = 14;
            this.buttonShowUpInGoogleMaps.Text = "View in Google Maps";
            this.buttonShowUpInGoogleMaps.UseVisualStyleBackColor = true;
            this.buttonShowUpInGoogleMaps.Click += new System.EventHandler(this.buttonShowUpInGoogleMaps_Click);
            // 
            // buttonInverseSelection
            // 
            this.buttonInverseSelection.AutoSize = true;
            this.buttonInverseSelection.Location = new System.Drawing.Point(182, 11);
            this.buttonInverseSelection.Name = "buttonInverseSelection";
            this.buttonInverseSelection.Size = new System.Drawing.Size(97, 31);
            this.buttonInverseSelection.TabIndex = 13;
            this.buttonInverseSelection.Text = "Inverse selection";
            this.buttonInverseSelection.UseVisualStyleBackColor = true;
            this.buttonInverseSelection.Click += new System.EventHandler(this.buttonInverseSelection_Click);
            // 
            // buttonDeselectAll
            // 
            this.buttonDeselectAll.AutoSize = true;
            this.buttonDeselectAll.Location = new System.Drawing.Point(96, 11);
            this.buttonDeselectAll.Name = "buttonDeselectAll";
            this.buttonDeselectAll.Size = new System.Drawing.Size(80, 31);
            this.buttonDeselectAll.TabIndex = 12;
            this.buttonDeselectAll.Text = "Deselect all";
            this.buttonDeselectAll.UseVisualStyleBackColor = true;
            this.buttonDeselectAll.Click += new System.EventHandler(this.buttonDeselectAll_Click);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.AutoSize = true;
            this.buttonSelectAll.Location = new System.Drawing.Point(9, 11);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(80, 31);
            this.buttonSelectAll.TabIndex = 11;
            this.buttonSelectAll.Text = "Select all";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 137);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.panel1.Size = new System.Drawing.Size(784, 249);
            this.panel1.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "GPX file:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Pictures folder:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "gpx";
            this.openFileDialog1.Filter = "GPX and FIT files|*.gpx;*.fit";
            // 
            // buttonAnalyze
            // 
            this.buttonAnalyze.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAnalyze.AutoSize = true;
            this.buttonAnalyze.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonAnalyze.ForeColor = System.Drawing.Color.Green;
            this.buttonAnalyze.Location = new System.Drawing.Point(694, 26);
            this.buttonAnalyze.Name = "buttonAnalyze";
            this.buttonAnalyze.Size = new System.Drawing.Size(80, 59);
            this.buttonAnalyze.TabIndex = 14;
            this.buttonAnalyze.Text = "Refresh";
            this.buttonAnalyze.UseVisualStyleBackColor = true;
            this.buttonAnalyze.Click += new System.EventHandler(this.buttonAnalyze_Click);
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.numericUpDownErrorMargin);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.numericUpDownTimeOffset);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.buttonAnalyze);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.textBoxGpxFile);
            this.panel2.Controls.Add(this.textBoxPicturesFolder);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(784, 137);
            this.panel2.TabIndex = 13;
            // 
            // numericUpDownErrorMargin
            // 
            this.numericUpDownErrorMargin.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::Gpx2Pic.Properties.Settings.Default, "ErrorMargin", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDownErrorMargin.Location = new System.Drawing.Point(9, 104);
            this.numericUpDownErrorMargin.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownErrorMargin.Name = "numericUpDownErrorMargin";
            this.numericUpDownErrorMargin.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownErrorMargin.TabIndex = 18;
            this.numericUpDownErrorMargin.Value = global::Gpx2Pic.Properties.Settings.Default.ErrorMargin;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Error margin [s]:";
            // 
            // numericUpDownTimeOffset
            // 
            this.numericUpDownTimeOffset.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::Gpx2Pic.Properties.Settings.Default, "TimeOffset", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDownTimeOffset.Location = new System.Drawing.Point(135, 104);
            this.numericUpDownTimeOffset.Maximum = new decimal(new int[] {
            50400,
            0,
            0,
            0});
            this.numericUpDownTimeOffset.Minimum = new decimal(new int[] {
            50400,
            0,
            0,
            -2147483648});
            this.numericUpDownTimeOffset.Name = "numericUpDownTimeOffset";
            this.numericUpDownTimeOffset.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownTimeOffset.TabIndex = 16;
            this.numericUpDownTimeOffset.Value = global::Gpx2Pic.Properties.Settings.Default.TimeOffset;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(132, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Time offset [s]:";
            // 
            // textBoxGpxFile
            // 
            this.textBoxGpxFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxGpxFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.textBoxGpxFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Gpx2Pic.Properties.Settings.Default, "GpxFileName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxGpxFile.Location = new System.Drawing.Point(9, 26);
            this.textBoxGpxFile.Name = "textBoxGpxFile";
            this.textBoxGpxFile.Size = new System.Drawing.Size(679, 20);
            this.textBoxGpxFile.TabIndex = 9;
            this.textBoxGpxFile.Text = global::Gpx2Pic.Properties.Settings.Default.GpxFileName;
            this.textBoxGpxFile.Click += new System.EventHandler(this.textBoxGpxFile_Click);
            this.textBoxGpxFile.TextChanged += new System.EventHandler(this.textBoxGpxFile_TextChanged);
            // 
            // textBoxPicturesFolder
            // 
            this.textBoxPicturesFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPicturesFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.textBoxPicturesFolder.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Gpx2Pic.Properties.Settings.Default, "PicturesFolder", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxPicturesFolder.Location = new System.Drawing.Point(9, 65);
            this.textBoxPicturesFolder.Name = "textBoxPicturesFolder";
            this.textBoxPicturesFolder.Size = new System.Drawing.Size(679, 20);
            this.textBoxPicturesFolder.TabIndex = 11;
            this.textBoxPicturesFolder.Text = global::Gpx2Pic.Properties.Settings.Default.PicturesFolder;
            this.textBoxPicturesFolder.Click += new System.EventHandler(this.textBoxPicturesFolder_Click);
            this.textBoxPicturesFolder.TextChanged += new System.EventHandler(this.textBoxPicturesFolder_TextChanged);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 441);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelButtons);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gpx2Pic - Automatically geotag your photos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownErrorMargin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeOffset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonDeselectAll;
        private System.Windows.Forms.Button buttonInverseSelection;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPicturesFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxGpxFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonAnalyze;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonShowUpInGoogleMaps;
        private System.Windows.Forms.Button buttonAbout;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.NumericUpDown numericUpDownTimeOffset;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownErrorMargin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxBackupOriginal;
    }
}

