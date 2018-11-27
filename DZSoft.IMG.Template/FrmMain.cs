using DZSoft.IMG.Template.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            tvTemp.ExpandAll();
        }


        private TEMPLATE_INFO TEMPLATE_INFO { get; set; }
        private MARK_REGION SelectedMarkRegion { get; set; }
        private CELL_INFO SelectedCellInfo { get; set; }


        private List<MODEL_REGION> MODEL_REGIONs { get; set; }

        private string FileName { get; set; }

        private Bitmap BaseMap { get; set; }

        private MODEL_REGION CurrentRegion { get; set; } = new MODEL_REGION();

        private RegionType RegionType { get; set; }

        private Point StartPoint { get; set; }

        private Point EndPoint { get; set; }

        public List<Point> Points { get; set; }
        public List<Point> Model_Points { get; set; }

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

        private void InitCellInfo()
        {
            SelectedCellInfo = new CELL_INFO();
        }

        private void tsbRectangle_Click(object sender, EventArgs e)
        {
            RegionType = RegionType.Rectangl;
            StartPoint = new Point();
            EndPoint = new Point();
            DrawShape();
        }
        private void tsbCircle_Click(object sender, EventArgs e)
        {
            RegionType = RegionType.Circle;
            StartPoint = new Point();
            EndPoint = new Point();
            DrawShape();
        }

        private void tsbPolygon_Click(object sender, EventArgs e)
        {
            RegionType = RegionType.Rolygon;
            Points = new List<Point>();
            DrawShape();
        }
        private void picContent_MouseDown(object sender, MouseEventArgs e)
        {
            if (BaseMap == null) return;
            if (RegionType == RegionType.NONE) return;
            if (e.Button == MouseButtons.Left)
            {
                IsStart = true;
                if (RegionType == RegionType.Rolygon)
                {
                    Points.Add(e.Location);
                }
                else
                {
                    StartPoint = e.Location;

                }
            }
            else if (e.Button == MouseButtons.Right && RegionType == RegionType.Rolygon)
            {
                if (Points.Count >= 3)
                {
                    IsStart = false;
                    Points.Add(Points.First());
                    EndPoint = new Point();
                    DrawShape();
                    RegionType = RegionType.NONE;
                    this.Cursor = Cursors.Default;
                }
            }
            else if (e.Button == MouseButtons.Right && (RegionType == RegionType.Rectangl || RegionType == RegionType.Circle))
            {
                this.Cursor = Cursors.Default;
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
            if (RegionType == RegionType.Rolygon)
            {
                EndPoint = e.Location;
                DrawShape();
            }
        }

        private void DrawShape()
        {
            Model_Points = new List<Point>();
            var minx = Math.Min(StartPoint.X, EndPoint.X);
            var miny = Math.Min(StartPoint.Y, EndPoint.Y);
            var maxx = Math.Max(StartPoint.X, EndPoint.X);
            var maxy = Math.Max(StartPoint.Y, EndPoint.Y);
            var width = maxx - minx;
            var height = maxy - miny;
            var tmpMap = (Bitmap)BaseMap.Clone();
            switch (RegionType)
            {
                case RegionType.Rectangl:
                    using (Graphics g = Graphics.FromImage(tmpMap))
                    {
                        g.DrawRectangle(MyPen, minx, miny, width, height);
                    }
                    Model_Points.Add(new Point(minx, miny));
                    Model_Points.Add(new Point(maxx, miny));
                    Model_Points.Add(new Point(maxx, maxy));
                    Model_Points.Add(new Point(minx, maxy));
                    this.Cursor = Cursors.Default;
                    break;
                case RegionType.Circle:
                    using (Graphics g = Graphics.FromImage(tmpMap))
                    {
                        if (IsControlDown)
                        {
                            var max = Math.Max(width, height);
                            g.DrawEllipse(MyPen, minx, miny, max, max);
                            Model_Points.Add(new Point(minx + max / 2, miny + max / 2));
                            Model_Points.Add(new Point(max, max));
                        }
                        else
                        {
                            g.DrawEllipse(MyPen, minx, miny, width, height);
                            Model_Points.Add(new Point(minx + width / 2, miny + height / 2));
                            var tmp = new Point();
                            tmp.Y = Math.Max(width, height) / 2;
                            tmp.X = Math.Min(width, height) / 2;
                            Model_Points.Add(tmp);
                        }
                    }
                    this.Cursor = Cursors.Default;
                    break;
                case RegionType.Rolygon:
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
                        Model_Points = Points.ToList();
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

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tsmExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        /// <summary>
        /// 新建模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmNew_Click(object sender, EventArgs e)
        {

        }



        /// <summary>
        /// 定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void rbLocation_CheckedChanged(object sender, EventArgs e)
        {



            //if (TEMPLATE_INFO.vMainMarkRegion.Count() == 0)
            //{
            //    MARK_REGION mARK_REGION = new MARK_REGION() { sMarkProp = new MARK_PROP(), sMarkRegion = new MODEL_REGION() };
            //    SelectedMarkRegion = mARK_REGION;
            //    TEMPLATE_INFO.vMainMarkRegion.Add(mARK_REGION);
            //}
            //else
            //{
            //    SelectedMarkRegion = TEMPLATE_INFO.vMainMarkRegion.First();
            //}
            //this.rbMarkModel0.Checked = SelectedMarkRegion.sMarkProp.byMode == 0 ? true : false;
            //this.rbMarkModel1.Checked = SelectedMarkRegion.sMarkProp.byMode == 1 ? true : false;
            //this.numMarkRatio.Value = (decimal)SelectedMarkRegion.sMarkProp.fRatio;
            //this.numMarkSearchX.Value = SelectedMarkRegion.sMarkProp.nSearchX;
            //this.numMarkSearchY.Value = SelectedMarkRegion.sMarkProp.nSearchY;
        }

        /// <summary>
        /// 检测区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void rbCheck_CheckedChanged(object sender, EventArgs e)
        {




        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            tpMark.Parent = tabMain;
            tpCheck.Parent = null;
            tpCL.Parent = null;
        }



        private void tpMark_Click(object sender, EventArgs e)
        {

        }
        private bool IsControlDown { get; set; }
        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            IsControlDown = e.Control;
        }

        private void picContent_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            IsControlDown = e.Control;
        }

        private void FrmMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            //  IsControlDown = e.KeyChar  == ;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Model_Points.Count == 0) return;
            MARK_REGION mARK_REGION = new MARK_REGION();
            mARK_REGION.sMarkProp = new MARK_PROP();
            if (rbMarkModel0.Checked)
            {
                mARK_REGION.sMarkProp.byMode = 0;
            }
            else
            {
                mARK_REGION.sMarkProp.byMode = 1;
            }
            mARK_REGION.sMarkProp.fRatio = (float)this.numMarkRatio.Value;
            mARK_REGION.sMarkProp.nSearchX = (int)this.numMarkSearchX.Value;
            mARK_REGION.sMarkProp.nSearchY = (int)this.numMarkSearchY.Value;

            mARK_REGION.sMarkRegion = new MODEL_REGION();
            switch (RegionType)
            {
                case RegionType.Rectangl:
                    mARK_REGION.sMarkRegion.nRegionType = 1;
                    break;
                case RegionType.Circle:
                    mARK_REGION.sMarkRegion.nRegionType = 2;
                    break;
                case RegionType.Rolygon:
                    mARK_REGION.sMarkRegion.nRegionType = 3;
                    break;
            }
            mARK_REGION.sMarkRegion.vModelPoints = Model_Points;

            TEMPLATE_INFO.vMainMarkRegion.Add(mARK_REGION);
            mARK_REGION.sMarkProp.nRegionNum = tvTemp.Nodes[0].Nodes[0].Nodes.Count + 1;
            TreeNode node = new TreeNode(mARK_REGION.sMarkProp.nRegionNum.ToString());
            node.Tag = mARK_REGION;
            tvTemp.Nodes[0].Nodes[0].Nodes.Add(node);
            tvTemp.ExpandAll();
        }

        private void tvTemp_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 1)
            {
                switch (e.Node.Text)
                {
                    case "定位":
                        tpMark.Parent = tabMain;
                        tpCheck.Parent = null;
                        tpCL.Parent = null;
                        break;
                    case "检测":
                        tpMark.Parent = null;
                        tpCheck.Parent = tabMain;
                        tpCL.Parent = null;
                        break;
                    case "测量":
                        tpMark.Parent = null;
                        tpCheck.Parent = null;
                        tpCL.Parent = tabMain;
                        break;
                    default:
                        tpMark.Parent = tabMain;
                        tpCheck.Parent = null;
                        tpCL.Parent = null;
                        break;
                }
            }

        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TEMPLATE_INFO = new TEMPLATE_INFO();
            TEMPLATE_INFO.sTemplateInfoProp.byCreateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            TEMPLATE_INFO.sTemplateInfoProp.byModelName = "test";
            flowLayoutPanel1.Enabled = true;
        }

        private void FrmMain_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            IsControlDown = e.Control;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var content = JsonConvert.SerializeObject(TEMPLATE_INFO);
            File.WriteAllText("D:\\TEMPLATE_INFO.txt", content);
            MessageBox.Show("OK");
        }

        private void btnAddCheck_Click(object sender, EventArgs e)
        {
            INSPECT_REGION iNSPECT_REGION = new INSPECT_REGION();

            #region 检测属性
            iNSPECT_REGION.sInspectProp = new INSPECT_PROP();
            iNSPECT_REGION.sInspectProp.byRegionMode = 0;

            if (chkGeneral.Checked)
            {
                iNSPECT_REGION.sInspectProp.sRegionPara.bAllShape = true;
                iNSPECT_REGION.sInspectProp.sRegionPara.sSpotPara.sSpotUp.nGray = (int)numWhite1.Value;
                iNSPECT_REGION.sInspectProp.sRegionPara.sSpotPara.sSpotUp.fMaxArea = (int)numWhite2.Value;

                iNSPECT_REGION.sInspectProp.sRegionPara.sSpotPara.sSpotDown.nGray = (int)numBlack1.Value;
                iNSPECT_REGION.sInspectProp.sRegionPara.sSpotPara.sSpotDown.fMaxArea = (int)numBlack2.Value;
            }

            if (chkGrade.Checked)
            {
                iNSPECT_REGION.sInspectProp.sRegionPara.bMircoAllShape = true;
                iNSPECT_REGION.sInspectProp.sRegionPara.sMircoAllPara.sSpotUp.nGray = (int)numGradeWhite1.Value;
                iNSPECT_REGION.sInspectProp.sRegionPara.sMircoAllPara.sSpotUp.fMaxArea = (int)numGradeWhite2.Value;
                iNSPECT_REGION.sInspectProp.sRegionPara.sMircoAllPara.sSpotDown.nGray = (int)numGradeBlack1.Value;
                iNSPECT_REGION.sInspectProp.sRegionPara.sMircoAllPara.sSpotDown.fMaxArea = (int)numGradeBlack2.Value;
            }

            if (chkArea.Checked)
            {
                iNSPECT_REGION.sInspectProp.sRegionPara.bMeasureFrontArea = true;
                iNSPECT_REGION.sInspectProp.sRegionPara.sMeasureFrontAreaPara.fDistUp = (int)numArea1.Value;
                iNSPECT_REGION.sInspectProp.sRegionPara.sMeasureFrontAreaPara.fDistDown = (int)numArea2.Value;
            }


            if (chkNum.Checked)
            {
                iNSPECT_REGION.sInspectProp.sRegionPara.bMeasureFrontNum = true;
                iNSPECT_REGION.sInspectProp.sRegionPara.sMeasureFrontNumPara.fDistUp = (int)numNum1.Value;
                iNSPECT_REGION.sInspectProp.sRegionPara.sMeasureFrontNumPara.fDistDown = (int)numNum2.Value;
            }


            if (chkLocation.Checked)
            {
                iNSPECT_REGION.sInspectProp.sRegionPara.bOverPrint = true;
                iNSPECT_REGION.sInspectProp.sRegionPara.sOverprintPara.fDistHor = (int)numLocalH.Value;
                iNSPECT_REGION.sInspectProp.sRegionPara.sOverprintPara.fDistVer = (int)numLocalV.Value;
            }

            if (chkColor.Checked)
            {
                iNSPECT_REGION.sInspectProp.sRegionPara.bCheckColor = true;
                iNSPECT_REGION.sInspectProp.sRegionPara.sColorPara.byEnable = 1;
                iNSPECT_REGION.sInspectProp.sRegionPara.sColorPara.fDeltaE = (int)numColor1.Value;
            }

            if (chkStat.Checked)
            {
                iNSPECT_REGION.sInspectProp.sRegionPara.bStatisticPara = true;
                iNSPECT_REGION.sInspectProp.sRegionPara.sStatisticInfo.fValue = (int)numStatArea.Value;
                iNSPECT_REGION.sInspectProp.sRegionPara.sStatisticInfo.nCount = (int)numStatNum.Value;
            }

            if (chkWT.Checked)
            {
                iNSPECT_REGION.sInspectProp.sRegionProp.sFrontProp.nDialtion = (int)numWTPZ.Value;
                iNSPECT_REGION.sInspectProp.sRegionProp.sFrontProp.byThredHigh = (byte)numWTHight.Value;
                iNSPECT_REGION.sInspectProp.sRegionProp.sFrontProp.byThredLow = (byte)numWTLow.Value;
                iNSPECT_REGION.sInspectProp.sRegionProp.sFrontProp.byThredType = (byte)cmbWT1.SelectedIndex;
            }
            #endregion

            iNSPECT_REGION.sInspectRegion = new MODEL_REGION();
            switch (RegionType)
            {
                case RegionType.Rectangl:
                    iNSPECT_REGION.sInspectRegion.nRegionType = 1;
                    break;
                case RegionType.Circle:
                    iNSPECT_REGION.sInspectRegion.nRegionType = 2;
                    break;
                case RegionType.Rolygon:
                    iNSPECT_REGION.sInspectRegion.nRegionType = 3;
                    break;
            }
            iNSPECT_REGION.sInspectRegion.vModelPoints = Model_Points;

            iNSPECT_REGION.sInspectProp.nRegionNum = tvTemp.Nodes[0].Nodes[1].Nodes.Count + 1;
            TEMPLATE_INFO.vCellInfo[0].vInspectRegion.Add(iNSPECT_REGION);
            TreeNode node = new TreeNode(iNSPECT_REGION.sInspectProp.nRegionNum.ToString());
            node.Tag = iNSPECT_REGION.sInspectProp;
            tvTemp.Nodes[0].Nodes[1].Nodes.Add(node);
            tvTemp.ExpandAll();
        }

        private void btnAddCL_Click(object sender, EventArgs e)
        {
            INSPECT_REGION iNSPECT_REGION = new INSPECT_REGION();

            #region 检测属性
            iNSPECT_REGION.sInspectProp = new INSPECT_PROP();
            iNSPECT_REGION.sInspectProp.byRegionMode = 1;
            iNSPECT_REGION.sInspectProp.byRegionModeSecond = (byte)cmbCL.SelectedIndex;
            iNSPECT_REGION.sInspectProp.sRegionPara.sMeasurePara.fDistUp = 0;
            iNSPECT_REGION.sInspectProp.sRegionPara.sMeasurePara.fDistDown = 0;

            #endregion


            #region 形状

            iNSPECT_REGION.sInspectRegion = new MODEL_REGION();
            switch (RegionType)
            {
                case RegionType.Rectangl:
                    iNSPECT_REGION.sInspectRegion.nRegionType = 1;
                    break;
                case RegionType.Circle:
                    iNSPECT_REGION.sInspectRegion.nRegionType = 2;
                    break;
                case RegionType.Rolygon:
                    iNSPECT_REGION.sInspectRegion.nRegionType = 3;
                    break;
            }
            iNSPECT_REGION.sInspectRegion.vModelPoints = Model_Points;
            #endregion

            iNSPECT_REGION.sInspectProp.nRegionNum = tvTemp.Nodes[0].Nodes[1].Nodes.Count + 1;
            TEMPLATE_INFO.vCellInfo[0].vInspectRegion.Add(iNSPECT_REGION);
            TreeNode node = new TreeNode(iNSPECT_REGION.sInspectProp.nRegionNum.ToString());
            node.Tag = iNSPECT_REGION.sInspectProp;
            tvTemp.Nodes[0].Nodes[1].Nodes.Add(node);
            tvTemp.ExpandAll();
        }
    }
}
