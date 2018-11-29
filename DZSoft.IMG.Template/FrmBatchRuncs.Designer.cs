namespace DZSoft.IMG.Template
{
    partial class FrmBatchRuncs
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
            this.btnGetTemp = new System.Windows.Forms.Button();
            this.txtTemp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtImgPath = new System.Windows.Forms.TextBox();
            this.btnGetImages = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRun = new System.Windows.Forms.Button();
            this.picContent = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picContent)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGetTemp
            // 
            this.btnGetTemp.Location = new System.Drawing.Point(631, 16);
            this.btnGetTemp.Name = "btnGetTemp";
            this.btnGetTemp.Size = new System.Drawing.Size(75, 23);
            this.btnGetTemp.TabIndex = 0;
            this.btnGetTemp.Text = "浏览";
            this.btnGetTemp.UseVisualStyleBackColor = true;
            this.btnGetTemp.Click += new System.EventHandler(this.btnGetTemp_Click);
            // 
            // txtTemp
            // 
            this.txtTemp.Location = new System.Drawing.Point(93, 17);
            this.txtTemp.Name = "txtTemp";
            this.txtTemp.Size = new System.Drawing.Size(532, 21);
            this.txtTemp.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "模板路径：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "图片路径：";
            // 
            // txtImgPath
            // 
            this.txtImgPath.Location = new System.Drawing.Point(93, 47);
            this.txtImgPath.Name = "txtImgPath";
            this.txtImgPath.Size = new System.Drawing.Size(532, 21);
            this.txtImgPath.TabIndex = 4;
            // 
            // btnGetImages
            // 
            this.btnGetImages.Location = new System.Drawing.Point(631, 45);
            this.btnGetImages.Name = "btnGetImages";
            this.btnGetImages.Size = new System.Drawing.Size(75, 23);
            this.btnGetImages.TabIndex = 5;
            this.btnGetImages.Text = "浏览";
            this.btnGetImages.UseVisualStyleBackColor = true;
            this.btnGetImages.Click += new System.EventHandler(this.button2_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(24, 74);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(754, 506);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "图片";
            this.columnHeader1.Width = 625;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "结果";
            this.columnHeader2.Width = 124;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(712, 15);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(66, 53);
            this.btnRun.TabIndex = 7;
            this.btnRun.Text = "执行";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // picContent
            // 
            this.picContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picContent.Location = new System.Drawing.Point(798, 16);
            this.picContent.Name = "picContent";
            this.picContent.Size = new System.Drawing.Size(641, 564);
            this.picContent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picContent.TabIndex = 8;
            this.picContent.TabStop = false;
            // 
            // FrmBatchRuncs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1451, 592);
            this.Controls.Add(this.picContent);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btnGetImages);
            this.Controls.Add(this.txtImgPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTemp);
            this.Controls.Add(this.btnGetTemp);
            this.Name = "FrmBatchRuncs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批量检测";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.picContent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetTemp;
        private System.Windows.Forms.TextBox txtTemp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtImgPath;
        private System.Windows.Forms.Button btnGetImages;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.PictureBox picContent;
    }
}