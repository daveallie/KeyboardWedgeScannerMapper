namespace KeyboardWedgeScannerMapper
{
    partial class FrmWindowPicker
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmWindowPicker));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.StartTracking = new System.Windows.Forms.ToolStripButton();
            this.StopTracking = new System.Windows.Forms.ToolStripButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copySelectedRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyAllRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dgwWindowInfo = new System.Windows.Forms.DataGridView();
            this.Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mouseTrackerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwWindowInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mouseTrackerBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StartTracking,
            this.StopTracking});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(284, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // StartTracking
            // 
            this.StartTracking.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StartTracking.Image = ((System.Drawing.Image)(resources.GetObject("StartTracking.Image")));
            this.StartTracking.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StartTracking.Name = "StartTracking";
            this.StartTracking.Size = new System.Drawing.Size(23, 22);
            this.StartTracking.Text = "Start Tracking";
            this.StartTracking.ToolTipText = "Start tracking";
            this.StartTracking.Click += new System.EventHandler(this.StartTracking_Click);
            // 
            // StopTracking
            // 
            this.StopTracking.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StopTracking.Enabled = false;
            this.StopTracking.Image = ((System.Drawing.Image)(resources.GetObject("StopTracking.Image")));
            this.StopTracking.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StopTracking.Name = "StopTracking";
            this.StopTracking.Size = new System.Drawing.Size(23, 22);
            this.StopTracking.Text = "Stop Tracking";
            this.StopTracking.ToolTipText = "Stop tracking";
            this.StopTracking.Click += new System.EventHandler(this.StopTracking_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.copySelectedRowsToolStripMenuItem,
            this.copyAllRowsToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(185, 70);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // copySelectedRowsToolStripMenuItem
            // 
            this.copySelectedRowsToolStripMenuItem.Name = "copySelectedRowsToolStripMenuItem";
            this.copySelectedRowsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.copySelectedRowsToolStripMenuItem.Text = "Copy selected row(s)";
            this.copySelectedRowsToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // copyAllRowsToolStripMenuItem
            // 
            this.copyAllRowsToolStripMenuItem.Name = "copyAllRowsToolStripMenuItem";
            this.copyAllRowsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.copyAllRowsToolStripMenuItem.Text = "Copy all rows";
            this.copyAllRowsToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // dgwWindowInfo
            // 
            this.dgwWindowInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwWindowInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Key,
            this.Value});
            this.dgwWindowInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgwWindowInfo.Location = new System.Drawing.Point(0, 25);
            this.dgwWindowInfo.Margin = new System.Windows.Forms.Padding(2);
            this.dgwWindowInfo.Name = "dgwWindowInfo";
            this.dgwWindowInfo.RowHeadersVisible = false;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgwWindowInfo.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgwWindowInfo.Size = new System.Drawing.Size(284, 236);
            this.dgwWindowInfo.TabIndex = 1;
            // 
            // Key
            // 
            this.Key.HeaderText = "Name";
            this.Key.Name = "Key";
            this.Key.ReadOnly = true;
            this.Key.Width = 90;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            this.Value.Width = 190;
            // 
            // FrmWindowPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.dgwWindowInfo);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FrmWindowPicker";
            this.ShowIcon = false;
            this.Text = "Window Picker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmWindowPicker_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgwWindowInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mouseTrackerBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton StartTracking;
        private System.Windows.Forms.ToolStripButton StopTracking;
        private System.Windows.Forms.BindingSource mouseTrackerBindingSource;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copySelectedRowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyAllRowsToolStripMenuItem;
        private System.Windows.Forms.DataGridView dgwWindowInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Key;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
    }
}

