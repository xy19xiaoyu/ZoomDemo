using DZSoft.IMG.Template.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DZSoft.IMG.Template
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private string FileName { get; set; }

        private Bitmap BaseMap { get; set; }

        private DrawType DrawType { get; set; }

        private Point StartPoint { get; set; }

        private Point EndPoint { get; set; }

        public List<Point> Points { get; set; }

        public bool IsStart { get; set; } = false;

        private Pen MyPen { get; set; } = new Pen(Brushes.Red, 3);

        private void tsmOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.jpg|*.jpg|*.bmp|*.bmp";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileName = ofd.FileName;
                BaseMap = (Bitmap)DZSOFT.Common.ImageUtil.ReadImage(FileName);
                this.picContent.Image = BaseMap;
            }
        }

        private void tsbRectangle_Click(object sender, EventArgs e)
        {
            if (this.Cursor == Cursors.Cross && DrawType == DrawType.Rectangl)
            {
                this.Cursor = Cursors.Default;
            }
            else
            {
                DrawType = DrawType.Rectangl;
                this.Cursor = Cursors.Cross;
            }

        }
        private void tsbCircle_Click(object sender, EventArgs e)
        {
            if (this.Cursor == Cursors.Cross && DrawType == DrawType.Circle)
            {
                this.Cursor = Cursors.Default;
            }
            else
            {
                DrawType = DrawType.Circle;
                this.Cursor = Cursors.Cross;
            }

        }

        private void tsbPolygon_Click(object sender, EventArgs e)
        {
            if (this.Cursor == Cursors.Cross && DrawType == DrawType.Rolygon)
            {
                this.Cursor = Cursors.Default;
            }
            else
            {
                DrawType = DrawType.Rolygon;
                this.Cursor = Cursors.Cross;
                Points = new List<Point>();
            }
        }
        private void picContent_MouseDown(object sender, MouseEventArgs e)
        {
            if (BaseMap == null) return;
            if (DrawType == DrawType.NONE) return;
            if (e.Button == MouseButtons.Left)
            {
                IsStart = true;
                if (DrawType == DrawType.Rolygon)
                {
                    Points.Add(e.Location);
                }
                else
                {
                    StartPoint = e.Location;

                }
            }
            else if (e.Button == MouseButtons.Right && DrawType == DrawType.Rolygon)
            {
                if (Points.Count >= 3)
                {
                    IsStart = false;
                    Points.Add(Points.First());
                    EndPoint = new Point();
                    DrawShape();
                    DrawType = DrawType.NONE;
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void picContent_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsStart) return;
            if (e.Button == MouseButtons.Left)
            {
                EndPoint = e.Location;
                DrawShape();
            }
            if (DrawType == DrawType.Rolygon)
            {
                EndPoint = e.Location;
                DrawShape();
            }
        }

        private void DrawShape()
        {
            var minx = Math.Min(StartPoint.X, EndPoint.X);
            var miny = Math.Min(StartPoint.Y, EndPoint.Y);
            var maxx = Math.Max(StartPoint.X, EndPoint.X);
            var maxy = Math.Max(StartPoint.Y, EndPoint.Y);
            var width = maxx - minx;
            var height = maxy - miny;
            var tmpMap = (Bitmap)BaseMap.Clone();
            switch (DrawType)
            {
                case DrawType.Rectangl:
                    using (Graphics g = Graphics.FromImage(tmpMap))
                    {
                        g.DrawRectangle(MyPen, minx, miny, width, height);
                    }
                    break;
                case DrawType.Circle:
                    using (Graphics g = Graphics.FromImage(tmpMap))
                    {
                        g.DrawEllipse(MyPen, minx, miny, width, height);
                    }
                    break;
                case DrawType.Rolygon:
                    using (Graphics g = Graphics.FromImage(tmpMap))
                    {
                        if (Points.Count >= 2)
                        {
                            g.DrawLines(MyPen, Points.ToArray());
                        }
                        if (!EndPoint.IsEmpty && Points.Count > 0)
                        {
                            g.DrawLine(MyPen, Points.Last(), EndPoint);
                        }

                    }
                    break;
            }


            this.picContent.Image = tmpMap;
        }

        private void picContent_MouseUp(object sender, MouseEventArgs e)
        {
            if (!IsStart) return;
            if (e.Button == MouseButtons.Left)
            {
                EndPoint = e.Location;
                DrawShape();
            }
        }


    }
}
