﻿namespace SimplexIde
{
    partial class ToolWindow
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolWindow));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.darkTreeView1 = new DarkUI.Controls.DarkTreeView();
            this.darkToolStrip1 = new DarkUI.Controls.DarkToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox2 = new System.Windows.Forms.ToolStripTextBox();
            this.darkContextMenu1 = new DarkUI.Controls.DarkContextMenu();
            this.insertFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertNewObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeTagColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkContextMenu2 = new DarkUI.Controls.DarkContextMenu();
            this.renameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.editPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkToolStrip1.SuspendLayout();
            this.darkContextMenu1.SuspendLayout();
            this.darkContextMenu2.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // darkTreeView1
            // 
            this.darkTreeView1.AllowMoveNodes = true;
            this.darkTreeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkTreeView1.Location = new System.Drawing.Point(0, 33);
            this.darkTreeView1.Margin = new System.Windows.Forms.Padding(4);
            this.darkTreeView1.MaxDragChange = 20;
            this.darkTreeView1.Name = "darkTreeView1";
            this.darkTreeView1.ShowIcons = true;
            this.darkTreeView1.Size = new System.Drawing.Size(268, 575);
            this.darkTreeView1.TabIndex = 0;
            this.darkTreeView1.Text = "darkTreeView1";
            this.darkTreeView1.SelectedNodesChanged += new System.EventHandler(this.darkTreeView1_SelectedNodesChanged);
            this.darkTreeView1.AfterNodeDragged += new System.EventHandler(this.DarkTreeView1_AfterNodeDragged);
            this.darkTreeView1.Click += new System.EventHandler(this.darkTreeView1_Click);
            this.darkTreeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.DarkTreeView1_DragDrop);
            this.darkTreeView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.darkTreeView1_MouseClick);
            this.darkTreeView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DarkTreeView1_MouseDoubleClick);
            // 
            // darkToolStrip1
            // 
            this.darkToolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkToolStrip1.AutoSize = false;
            this.darkToolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.darkToolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.darkToolStrip1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkToolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.darkToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripTextBox1,
            this.toolStripTextBox2});
            this.darkToolStrip1.Location = new System.Drawing.Point(0, 612);
            this.darkToolStrip1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 123);
            this.darkToolStrip1.Name = "darkToolStrip1";
            this.darkToolStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.darkToolStrip1.Size = new System.Drawing.Size(268, 38);
            this.darkToolStrip1.TabIndex = 1;
            this.darkToolStrip1.Text = "darkToolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(24, 35);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(148, 35);
            this.toolStripTextBox1.Text = "Search for a resource";
            // 
            // toolStripTextBox2
            // 
            this.toolStripTextBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripTextBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripTextBox2.Name = "toolStripTextBox2";
            this.toolStripTextBox2.Size = new System.Drawing.Size(180, 27);
            // 
            // darkContextMenu1
            // 
            this.darkContextMenu1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.darkContextMenu1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkContextMenu1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.darkContextMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertFolderToolStripMenuItem,
            this.insertNewObjectToolStripMenuItem,
            this.editPropertiesToolStripMenuItem,
            this.editCodeToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.changeColorToolStripMenuItem,
            this.setTagToolStripMenuItem,
            this.changeTagColorToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.darkContextMenu1.Name = "darkContextMenu1";
            this.darkContextMenu1.Size = new System.Drawing.Size(193, 220);
            this.darkContextMenu1.Opening += new System.ComponentModel.CancelEventHandler(this.darkContextMenu1_Opening);
            // 
            // insertFolderToolStripMenuItem
            // 
            this.insertFolderToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.insertFolderToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.insertFolderToolStripMenuItem.Name = "insertFolderToolStripMenuItem";
            this.insertFolderToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.insertFolderToolStripMenuItem.Text = "Insert folder";
            this.insertFolderToolStripMenuItem.Click += new System.EventHandler(this.InsertFolderToolStripMenuItem_Click);
            // 
            // insertNewObjectToolStripMenuItem
            // 
            this.insertNewObjectToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.insertNewObjectToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.insertNewObjectToolStripMenuItem.Name = "insertNewObjectToolStripMenuItem";
            this.insertNewObjectToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.insertNewObjectToolStripMenuItem.Text = "Insert object";
            this.insertNewObjectToolStripMenuItem.Click += new System.EventHandler(this.insertNewObjectToolStripMenuItem_Click);
            // 
            // editCodeToolStripMenuItem
            // 
            this.editCodeToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.editCodeToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.editCodeToolStripMenuItem.Name = "editCodeToolStripMenuItem";
            this.editCodeToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.editCodeToolStripMenuItem.Text = "Edit code";
            this.editCodeToolStripMenuItem.Click += new System.EventHandler(this.editCodeToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.renameToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // changeColorToolStripMenuItem
            // 
            this.changeColorToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.changeColorToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.changeColorToolStripMenuItem.Name = "changeColorToolStripMenuItem";
            this.changeColorToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.changeColorToolStripMenuItem.Text = "Change color";
            this.changeColorToolStripMenuItem.Click += new System.EventHandler(this.ChangeColorToolStripMenuItem_Click);
            // 
            // setTagToolStripMenuItem
            // 
            this.setTagToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.setTagToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.setTagToolStripMenuItem.Name = "setTagToolStripMenuItem";
            this.setTagToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.setTagToolStripMenuItem.Text = "Set tag";
            this.setTagToolStripMenuItem.Click += new System.EventHandler(this.SetTagToolStripMenuItem_Click);
            // 
            // changeTagColorToolStripMenuItem
            // 
            this.changeTagColorToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.changeTagColorToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.changeTagColorToolStripMenuItem.Name = "changeTagColorToolStripMenuItem";
            this.changeTagColorToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.changeTagColorToolStripMenuItem.Text = "Change tag color";
            this.changeTagColorToolStripMenuItem.Click += new System.EventHandler(this.ChangeTagColorToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.deleteToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // darkContextMenu2
            // 
            this.darkContextMenu2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.darkContextMenu2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkContextMenu2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.darkContextMenu2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem1,
            this.deleteToolStripMenuItem1});
            this.darkContextMenu2.Name = "darkContextMenu2";
            this.darkContextMenu2.Size = new System.Drawing.Size(133, 52);
            // 
            // renameToolStripMenuItem1
            // 
            this.renameToolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.renameToolStripMenuItem1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.renameToolStripMenuItem1.Name = "renameToolStripMenuItem1";
            this.renameToolStripMenuItem1.Size = new System.Drawing.Size(132, 24);
            this.renameToolStripMenuItem1.Text = "Rename";
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.deleteToolStripMenuItem1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(132, 24);
            this.deleteToolStripMenuItem1.Text = "Delete";
            // 
            // editPropertiesToolStripMenuItem
            // 
            this.editPropertiesToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.editPropertiesToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.editPropertiesToolStripMenuItem.Name = "editPropertiesToolStripMenuItem";
            this.editPropertiesToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.editPropertiesToolStripMenuItem.Text = "Properties Editor";
            this.editPropertiesToolStripMenuItem.Click += new System.EventHandler(this.EditPropertiesToolStripMenuItem_Click);
            // 
            // ToolWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.Controls.Add(this.darkToolStrip1);
            this.Controls.Add(this.darkTreeView1);
            this.DefaultDockArea = DarkUI.Docking.DarkDockArea.Document;
            this.DockText = "Project Explorer";
            this.Icon = global::SimplexIde.Properties.Resources.Noname;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ToolWindow";
            this.Size = new System.Drawing.Size(272, 650);
            this.Load += new System.EventHandler(this.ToolWindow_Load);
            this.darkToolStrip1.ResumeLayout(false);
            this.darkToolStrip1.PerformLayout();
            this.darkContextMenu1.ResumeLayout(false);
            this.darkContextMenu2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private DarkUI.Controls.DarkTreeView darkTreeView1;
        private DarkUI.Controls.DarkToolStrip darkToolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripTextBox1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private DarkUI.Controls.DarkContextMenu darkContextMenu1;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertNewObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertFolderToolStripMenuItem;
        private DarkUI.Controls.DarkContextMenu darkContextMenu2;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem changeColorToolStripMenuItem;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ToolStripMenuItem setTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeTagColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editPropertiesToolStripMenuItem;
    }
}
