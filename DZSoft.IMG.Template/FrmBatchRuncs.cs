using DZSoft.IMG.Template.BLL;
using DZSoft.IMG.Template.Model;
using DZSoft.IMG.Template.Util;
using DZSOFT.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DZSoft.IMG.Template
{
    public partial class FrmBatchRuncs : Form
    {
        private VisionChecker VisionChecker { get; set; }
        public List<string> Files { get; set; }
        public FrmBatchRuncs()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.txtImgPath.Text = fbd.SelectedPath;
                Files = Directory.GetFiles(this.txtImgPath.Text, "*.jpg").ToList();
            }
        }

        private void btnGetTemp_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.txtTemp.Text = ofd.FileName;
            }

        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            Thread th = new Thread(new ThreadStart((Action)delegate ()
            {
                if (Files.Count == 0) return;

                foreach (var file in Files)
                {
                    string dirName = Path.GetDirectoryName(file);
                    string fileName = Path.GetFileName(file);
                    string resultPath = Path.Combine(dirName, "CheckResult");
                    string fullFileName = Path.Combine(resultPath, fileName);


                    Bitmap map = (Bitmap)ImageUtil.ReadImage(file);

                    #region 实际
                    //DzBitmap dzBitmap = new DzBitmap()
                    //{
                    //    stride = 0,
                    //    type = 3,
                    //    width = map.Width,
                    //    height = map.Height,
                    //    imgData = StructUtil.GetPtrByBitmap(map),
                    //};
                    //List<INSPECT_RESULT_INFO> results = VisionChecker.Check(dzBitmap);
                    List<INSPECT_RESULT_INFO> results = new List<INSPECT_RESULT_INFO>() { new INSPECT_RESULT_INFO() { rcOrigin = new Rectangle(30, 30, 200, 200) } };
                    #endregion
                    if (results.Count > 0)
                    {
                        List<Rectangle> retcs = results.Select(o => { return o.rcOrigin; }).ToList();
                        using (Graphics g = Graphics.FromImage(map))
                        {
                            foreach (var result in results)
                            {
                                g.DrawRectangles(new Pen(Brushes.Red, 3), retcs.ToArray());
                            }
                        }

                        if (!Directory.Exists(resultPath))
                        {
                            Directory.CreateDirectory(resultPath);
                        }

                        map.Save(fullFileName);
                    }
                    else
                    {
                        fullFileName = file;

                    }

                    this.Invoke((Action)delegate ()
                    {
                        if (picContent.Image != null)
                        {
                            picContent.Image.Dispose();
                        }
                        picContent.Image = (Bitmap)map.Clone();

                        ListViewItem item = new ListViewItem(Path.GetFileNameWithoutExtension(file));
                        item.SubItems.Add(results.Count().ToString());
                        item.Tag = fullFileName;
                        listView1.Items.Add(item);
                        item.EnsureVisible();

                        map.Dispose();
                    });
                    Thread.Sleep(1000);
                }
            }));
            th.Start();



        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            string file = listView1.SelectedItems[0].Tag.ToString();

            picContent.Image = ImageUtil.ReadImage(file);
        }
    }
}
