namespace DZSoft.IMG.Template
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbRectangle = new System.Windows.Forms.ToolStripButton();
            this.tsbPolygon = new System.Windows.Forms.ToolStripButton();
            this.tsbCircle = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmExit = new System.Windows.Forms.ToolStripMenuItem();
            this.dzPanel1 = new DZSoft.IMG.Template.UserControls.DZPanel();
            this.picContent = new System.Windows.Forms.PictureBox();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.dzPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picContent)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRectangle,
            this.tsbPolygon,
            this.tsbCircle});
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(923, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbRectangle
            // 
            this.tsbRectangle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRectangle.Image = global::DZSoft.IMG.Template.Properties.Resources.Rectangle;
            this.tsbRectangle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRectangle.Name = "tsbRectangle";
            this.tsbRectangle.Size = new System.Drawing.Size(23, 22);
            this.tsbRectangle.Text = "toolStripButton1";
            this.tsbRectangle.Click += new System.EventHandler(this.tsbRectangle_Click);
            // 
            // tsbPolygon
            // 
            this.tsbPolygon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPolygon.Image = global::DZSoft.IMG.Template.Properties.Resources.polygon;
            this.tsbPolygon.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPolygon.Name = "tsbPolygon";
            this.tsbPolygon.Size = new System.Drawing.Size(23, 22);
            this.tsbPolygon.Text = "toolStripButton2";
            this.tsbPolygon.Click += new System.EventHandler(this.tsbPolygon_Click);
            // 
            // tsbCircle
            // 
            this.tsbCircle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCircle.Image = global::DZSoft.IMG.Template.Properties.Resources.circle;
            this.tsbCircle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCircle.Name = "tsbCircle";
            this.tsbCircle.Size = new System.Drawing.Size(23, 22);
            this.tsbCircle.Text = "toolStripButton3";
            this.tsbCircle.Click += new System.EventHandler(this.tsbCircle_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(723, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 432);
            this.panel1.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(923, 25);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmOpen,
            this.tsmExit});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(58, 21);
            this.文件ToolStripMenuItem.Text = "文件(&F)";
            // 
            // tsmOpen
            // 
            this.tsmOpen.Name = "tsmOpen";
            this.tsmOpen.Size = new System.Drawing.Size(118, 22);
            this.tsmOpen.Text = "打开(&O)";
            this.tsmOpen.Click += new System.EventHandler(this.tsmOpen_Click);
            // 
            // tsmExit
            // 
            this.tsmExit.Name = "tsmExit";
            this.tsmExit.Size = new System.Drawing.Size(118, 22);
            this.tsmExit.Text = "推出(&X)";
            // 
            // dzPanel1
            // 
            this.dzPanel1.Controls.Add(this.picContent);
            this.dzPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dzPanel1.Location = new System.Drawing.Point(0, 50);
            this.dzPanel1.Name = "dzPanel1";
            this.dzPanel1.Size = new System.Drawing.Size(723, 432);
            this.dzPanel1.TabIndex = 2;
            // 
            // picContent
            // 
            this.picContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picContent.Location = new System.Drawing.Point(0, 0);
            this.picContent.Name = "picContent";
            this.picContent.Size = new System.Drawing.Size(723, 432);
            this.picContent.TabIndex = 0;
            this.picContent.TabStop = false;
            this.picContent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picContent_MouseDown);
            this.picContent.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picContent_MouseMove);
            this.picContent.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picContent_MouseUp);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 482);
            this.Controls.Add(this.dzPanel1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.Text = "模板制作";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.dzPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picContent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbRectangle;
        private System.Windows.Forms.ToolStripButton tsbPolygon;
        private System.Windows.Forms.Panel panel1;
        private UserControls.DZPanel dzPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmOpen;
        private System.Windows.Forms.ToolStripMenuItem tsmExit;
        private System.Windows.Forms.PictureBox picContent;
        private System.Windows.Forms.ToolStripButton tsbCircle;
    }
}

