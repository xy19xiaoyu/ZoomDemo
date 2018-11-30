using DZSoft.IMG.Template.BLL;
using DZSoft.IMG.Template.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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

        private List<MODEL_REGION> MODEL_REGIONs { get; set; }

        private string FileName { get; set; }

        private Bitmap BaseMap { get; set; }


        private MODEL_REGION CurrentRegion { get; set; } = new MODEL_REGION();

        private RegionType RegionType { get; set; }

        private Point StartPoint { get; set; }

        private Point EndPoint { get; set; }

        public List<Point> Points { get; set; } = new List<Point>();
        public List<Point> Model_Points { get; set; } = new List<Point>();

        public bool IsStart { get; set; } = false;

        private Pen MyPen { get; set; } = new Pen(Brushes.Red, 2);

        private void tsmOpen_Click(object sender, EventArgs e)
        {

        }



        private void tsbRectangle_Click(object sender, EventArgs e)
        {
            IsStart = true;
            RegionType = RegionType.Rectangl;
            StartPoint = new Point();
            EndPoint = new Point();
            DrawShape();
        }
        private void tsbCircle_Click(object sender, EventArgs e)
        {
            IsStart = true;
            RegionType = RegionType.Circle;
            StartPoint = new Point();
            EndPoint = new Point();
            DrawShape();
        }

        private void tsbPolygon_Click(object sender, EventArgs e)
        {
            IsStart = true;
            RegionType = RegionType.Rolygon;
            Points = new List<Point>();
            DrawShape();
        }
        private void picContent_MouseDown(object sender, MouseEventArgs e)
        {
            if (BaseMap == null) return;
            var location = TransPoint(e.Location);
            if (this.Cursor == Cursors.SizeAll && e.Button == MouseButtons.Left)
            {
                IsMove = true;
                MouseDownPoint = Cursor.Position;
                picContent.Focus();
            }
            else
            {
                if (RegionType == RegionType.NONE) return;

                if (e.Button == MouseButtons.Left)
                {
                    if (RegionType == RegionType.Rolygon && IsStart)
                    {
                        Points.Add(location);
                    }
                    else
                    {
                        StartPoint = location;

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
                        //RegionType = RegionType.NONE;
                        this.Cursor = Cursors.Default;
                    }
                }
                else if (e.Button == MouseButtons.Right && (RegionType == RegionType.Rectangl || RegionType == RegionType.Circle))
                {
                    this.Cursor = Cursors.Default;
                }
            }

        }

        private void picContent_MouseMove(object sender, MouseEventArgs e)
        {
            picContent.Focus();
            if (IsMove)
            {
                int x, y;
                int moveX, moveY;
                moveX = Cursor.Position.X - MouseDownPoint.X;
                moveY = Cursor.Position.Y - MouseDownPoint.Y;
                x = picContent.Location.X + moveX;
                y = picContent.Location.Y + moveY;
                picContent.Location = new Point(x, y);
                MouseDownPoint.X = Cursor.Position.X;
                MouseDownPoint.Y = Cursor.Position.Y;
            }
            else
            {
                var location = TransPoint(e.Location);
                if (!IsStart) return;
                if (e.Button == MouseButtons.Left || RegionType == RegionType.Rolygon)
                {
                    EndPoint = location;
                    DrawShape();
                }
            }

        }

        private void DrawShape()
        {
            DrawTemp();
            Model_Points = new List<Point>();
            var minx = Math.Min(StartPoint.X, EndPoint.X);
            var miny = Math.Min(StartPoint.Y, EndPoint.Y);
            var maxx = Math.Max(StartPoint.X, EndPoint.X);
            var maxy = Math.Max(StartPoint.Y, EndPoint.Y);
            var width = maxx - minx;
            var height = maxy - miny;
            var tmpMap = (Bitmap)midMap.Clone();

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
                            tmp.X = width / 2;
                            tmp.Y = height / 2;
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

            if (this.picContent.Image != null) this.picContent.Image.Dispose();
            this.picContent.Image = tmpMap;
        }

        private void picContent_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsMove)
            {
                IsMove = false;
            }
            else
            {
                if (!IsStart) return;
                if (e.Button == MouseButtons.Left)
                {
                    EndPoint = TransPoint(e.Location);
                    DrawShape();
                }
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
            //    Selected_MARK_REGION Selected_MARK_REGION = new Selected_MARK_REGION() { sMarkProp = new MARK_PROP(), sMarkRegion = new MODEL_REGION() };
            //    SelectedMarkRegion = Selected_MARK_REGION;
            //    TEMPLATE_INFO.vMainMarkRegion.Add(Selected_MARK_REGION);
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
            CreateNewTemplateInfo();
            tpMark.Parent = tabMain;
            tpCheck.Parent = null;
            tpCL.Parent = null;
            this.picContent.MouseWheel += PicContent_MouseWheel;
            this.picContent.Focus();
        }

        private void PicContent_MouseWheel(object sender, MouseEventArgs e)
        {
            if (BaseMap == null) return;
            if (e.Delta > 0)
            {
                Zoom(e.Location, true);
            }
            else
            {
                Zoom(e.Location, false);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="isZoomIn"></param>
        public void Zoom(Point point, bool isZoomIn)
        {
            int x = point.X;
            int y = point.Y;
            int ow = picContent.Width;
            int oh = picContent.Height;
            int VX, VY;
            if (isZoomIn)
            {
                picContent.Width += ZoomStep;
                picContent.Height += ZoomStep;
                PropertyInfo pInfo = picContent.GetType().GetProperty("ImageRectangle", BindingFlags.Instance |
                 BindingFlags.NonPublic);
                Rectangle rect = (Rectangle)pInfo.GetValue(picContent, null);
                picContent.Width = rect.Width;
                picContent.Height = rect.Height;
            }
            else
            {
                if (picContent.Width < BaseMap.Width / 10)
                    return;
                picContent.Width -= ZoomStep;
                picContent.Height -= ZoomStep;
                PropertyInfo pInfo = picContent.GetType().GetProperty("ImageRectangle", BindingFlags.Instance |
                 BindingFlags.NonPublic);
                Rectangle rect = (Rectangle)pInfo.GetValue(picContent, null);
                picContent.Width = rect.Width;
                picContent.Height = rect.Height;
            }
            VX = (int)((double)x * (ow - picContent.Width) / ow);
            VY = (int)((double)y * (oh - picContent.Height) / oh);
            picContent.Location = new Point(picContent.Location.X + VX, picContent.Location.Y + VY);
            if (picContent.Width > BaseMap.Width)
            {
                this.Cursor = Cursors.SizeAll;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }

        }
        public void ReSetPictureSize()
        {
            if (BaseMap == null) return;
            picContent.Location = new Point(0, 0);
            picContent.Size = BaseMap.Size;
        }

        private void tpMark_Click(object sender, EventArgs e)
        {

        }
        private bool IsControlDown { get; set; }
        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            IsControlDown = e.Control;

            if (e.Alt) { this.Cursor = Cursors.SizeAll; }
        }

        private void picContent_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            IsControlDown = e.Control;
            if (e.Alt) { this.Cursor = Cursors.SizeAll; }
        }



        private void button5_Click(object sender, EventArgs e)
        {
            if (Model_Points.Count == 0) { MessageBox.Show("请选绘制区域"); return; }
            MARK_REGION tmp_MARK_REGION = new MARK_REGION();
            tmp_MARK_REGION.sMarkProp = new MARK_PROP();
            if (rbMarkModel0.Checked)
            {
                tmp_MARK_REGION.sMarkProp.byMode = 0;
            }
            else
            {
                tmp_MARK_REGION.sMarkProp.byMode = 1;
            }
            tmp_MARK_REGION.sMarkProp.fRatio = (float)this.numMarkRatio.Value;
            tmp_MARK_REGION.sMarkProp.nSearchX = (int)this.numMarkSearchX.Value;
            tmp_MARK_REGION.sMarkProp.nSearchY = (int)this.numMarkSearchY.Value;

            tmp_MARK_REGION.sMarkRegion = GetMODEL_REGION();

            TEMPLATE_INFO.vMainMarkRegion.Add(tmp_MARK_REGION);
            tmp_MARK_REGION.sMarkProp.nRegionNum = tvTemp.Nodes[0].Nodes[0].Nodes.Count + 1;
            TreeNode node = new TreeNode(tmp_MARK_REGION.sMarkProp.nRegionNum.ToString());
            node.Tag = tmp_MARK_REGION;
            tvTemp.Nodes[0].Nodes[0].Nodes.Add(node);
            tvTemp.ExpandAll();
        }

        private void ShowTablePage(TabPage tabPage = null)
        {
            tpMark.Parent = null;
            tpCheck.Parent = null;
            tpCL.Parent = null;
            tabUnCheck.Parent = null;
            if (tabPage != null)
            {
                tabPage.Parent = tabMain;
            }

        }

        private void tvTemp_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MODEL_REGION hiLight = null;
            TabPage show = null;
            switch (e.Node.Level)
            {
                case 0:
                    break;
                case 1:
                    switch (e.Node.Text)
                    {
                        case "定位":
                            show = tpMark;
                            //设置定位默认值
                            SetDefaultMarkParp();
                            break;
                        default:
                            break;
                    }
                    break;
                case 2:
                    switch (e.Node.Parent.Text)
                    {
                        case "定位":
                            show = tpMark;
                            Selected_MARK_REGION = e.Node.Tag as MARK_REGION;
                            hiLight = Selected_MARK_REGION.sMarkRegion;
                            ShowMarkRegion();
                            break;
                        default:
                            break;
                    }
                    break;
                case 3:
                    switch (e.Node.Text)
                    {
                        case "检测":
                            show = tpCheck;
                            // 设置检测项默认值 
                            SetDefaultCheckParp();
                            break;
                        case "测量":
                            show = tpCL;
                            //设置测量相默认值
                            SetDefaultCLParp();
                            break;
                        default:
                            break;
                    }
                    break;
                case 4:
                    switch (e.Node.Parent.Text)
                    {
                        case "检测":
                            {
                                Selected_INSPECT_REGION = e.Node.Tag as INSPECT_REGION;
                                hiLight = Selected_INSPECT_REGION.sInspectRegion;
                                ShowCheckInspectRegion();
                            }
                            show = tpCheck;
                            break;
                        case "测量":
                            {
                                Selected_INSPECT_REGION = e.Node.Tag as INSPECT_REGION;
                                hiLight = Selected_INSPECT_REGION.sInspectRegion;
                                ShowCheckInspectRegion();
                            }
                            show = tpCL;
                            break;
                        default:
                            break;
                    }
                    break;
                case 5:
                    switch (e.Node.Text)
                    {
                        case "不检测":
                            show = tabUnCheck;
                            btnAddUnCheck.Visible = true;
                            btnEditUnCheck.Visible = false;
                            btnDelUnCheck.Visible = false;
                            break;
                    }
                    break;
                case 6:
                    {
                        Selected_UNINSPECT_REGION = e.Node.Tag as UNINSPECT_REGION;
                        hiLight = Selected_UNINSPECT_REGION.rgnUnInspectRegion;
                        show = tabUnCheck;
                        btnAddUnCheck.Visible = false;
                        btnEditUnCheck.Visible = true;
                        btnDelUnCheck.Visible = true;
                    }
                    break;

            }
            ShowTablePage(show);
            ClearDrawTemp();
            DrawTemp(hiLight);

        }

        private void SetDefaultCLParp()
        {
            cmbCL.SelectedIndex = 0;
            numCL1.Value = 1;
            numCL2.Value = 1;
            numCL3.Value = 1;
            numCL4.Value = 1;

            btnAddCL.Visible = true;
            btnDelCL.Visible = false;
            btnEditCL.Visible = false;
        }

        private void SetDefaultCheckParp()
        {
            //普通检测
            chkGeneral.Checked = false;

            numWhite1.Value = 1;
            numWhite2.Value = 1;
            numBlack1.Value = 1;
            numBlack2.Value = 1;

            //精细检测
            chkGrade.Checked = false;
            numGradeWhite1.Value = 1;
            numGradeWhite2.Value = 1;
            numGradeBlack1.Value = 1;
            numGradeBlack2.Value = 1;

            //面积偏差
            chkArea.Checked = false;
            numArea1.Value = 1;
            numArea2.Value = 1;
            //数字偏差
            chkNum.Checked = false;
            numNum1.Value = 1;
            numNum2.Value = 1;
            //位置偏差
            chkLocation.Checked = false;
            numLocalH.Value = 1;
            numLocalV.Value = 1;

            //颜色偏差
            chkColor.Checked = false;
            numColor1.Value = 1;
            //统计
            chkStat.Checked = false;
            numStatArea.Value = 1;
            numStatNum.Value = 1;


            //文图提取
            chkWT.Checked = false;
            numWTPZ.Value = 1;
            numWTHight.Value = 1;
            numWTLow.Value = 1;
            cmbWT1.SelectedIndex = 1;

            btnAddCheck.Visible = true;
            btnEditCheck.Visible = false;
            btnDelCheck.Visible = false;
        }

        /// <summary>
        /// 检测区域默认值
        /// </summary>
        private void SetDefaultMarkParp()
        {
            rbMarkModel0.Checked = true;
            this.numMarkRatio.Value = 0.1M;
            this.numMarkSearchX.Value = 10;
            this.numMarkSearchY.Value = 10;
        }

        private void ShowCheckInspectRegion()
        {

            #region 检测属性
            if (Selected_INSPECT_REGION.sInspectProp.byRegionMode == 0)
            {
                //普通检测
                chkGeneral.Checked = Selected_INSPECT_REGION.sInspectProp.sRegionPara.bAllShape == true;
                if (chkGeneral.Checked)
                {
                    numWhite1.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sSpotPara.sSpotUp.nGray;
                    numWhite2.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sSpotPara.sSpotUp.fMaxArea;
                    numBlack1.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sSpotPara.sSpotDown.nGray;
                    numBlack2.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sSpotPara.sSpotDown.fMaxArea;
                }
                //精细检测
                chkGrade.Checked = Selected_INSPECT_REGION.sInspectProp.sRegionPara.bMircoAllShape == true;
                if (chkGrade.Checked)
                {
                    numGradeWhite1.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMircoAllPara.sSpotUp.nGray;
                    numGradeWhite2.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMircoAllPara.sSpotUp.fMaxArea;
                    numGradeBlack1.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMircoAllPara.sSpotDown.nGray;
                    numGradeBlack2.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMircoAllPara.sSpotDown.fMaxArea;
                }
                //面积偏差
                chkArea.Checked = Selected_INSPECT_REGION.sInspectProp.sRegionPara.bMeasureFrontArea == true;
                if (chkArea.Checked)
                {
                    numArea1.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMeasureFrontAreaPara.fDistUp;
                    numArea2.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMeasureFrontAreaPara.fDistDown;
                }
                //数字偏差
                chkNum.Checked = Selected_INSPECT_REGION.sInspectProp.sRegionPara.bMeasureFrontNum == true;
                if (chkNum.Checked)
                {
                    numNum1.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMeasureFrontNumPara.fDistUp;
                    numNum2.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMeasureFrontNumPara.fDistDown;
                }
                //位置偏差
                chkLocation.Checked = Selected_INSPECT_REGION.sInspectProp.sRegionPara.bOverPrint == true;
                if (chkLocation.Checked)
                {
                    numLocalH.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sOverprintPara.fDistHor;
                    numLocalV.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sOverprintPara.fDistHor;
                }

                //颜色偏差
                chkColor.Checked = Selected_INSPECT_REGION.sInspectProp.sRegionPara.bCheckColor == true;
                if (chkColor.Checked)
                {
                    numColor1.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sColorPara.fDeltaE;
                }
                //统计
                chkStat.Checked = Selected_INSPECT_REGION.sInspectProp.sRegionPara.bStatisticPara == true;
                if (chkStat.Checked)
                {
                    numStatArea.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sStatisticInfo.fValue;
                    numStatNum.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sStatisticInfo.nCount;
                }


                //文图提取
                //chkWT.Checked = Selected_INSPECT_REGION.sInspectProp.sRegionPara.bStatisticPara == true;
                //if (chkWT.Checked)
                //{
                numWTPZ.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionProp.sFrontProp.nDialtion;
                numWTHight.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionProp.sFrontProp.byThredHigh;
                numWTLow.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionProp.sFrontProp.byThredLow;
                cmbWT1.SelectedIndex = Selected_INSPECT_REGION.sInspectProp.sRegionProp.sFrontProp.byThredType;
                // }
                btnAddCheck.Visible = false;
                btnEditCheck.Visible = true;
                btnDelCheck.Visible = true;
            }
            else
            {
                //测量属性
                cmbCL.SelectedIndex = Selected_INSPECT_REGION.sInspectProp.byRegionModeSecond;
                numCL1.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMeasurePara.fDistUp;
                numCL2.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMeasurePara.fDistDown;
                numCL3.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionProp.sMeasureProp.sEdgeLeftUp.byGray;
                numCL4.Value = (decimal)Selected_INSPECT_REGION.sInspectProp.sRegionProp.sMeasureProp.sEdgeRightBottom.byGray;

                btnAddCL.Visible = false;
                btnDelCL.Visible = true;
                btnEditCL.Visible = true;

            }
            #endregion
        }

        private MARK_REGION Selected_MARK_REGION { get; set; }
        private INSPECT_REGION Selected_INSPECT_REGION { get; set; }
        private UNINSPECT_REGION Selected_UNINSPECT_REGION { get; set; }
        private void ShowMarkRegion()
        {
            rbMarkModel0.Checked = Selected_MARK_REGION.sMarkProp.byMode == 0 ? true : false;
            rbMarkModel1.Checked = Selected_MARK_REGION.sMarkProp.byMode == 1 ? true : false;
            this.numMarkRatio.Value = (decimal)Selected_MARK_REGION.sMarkProp.fRatio;
            this.numMarkSearchX.Value = Selected_MARK_REGION.sMarkProp.nSearchX;
            this.numMarkSearchY.Value = Selected_MARK_REGION.sMarkProp.nSearchY;
            btnAddMark.Visible = false;
            btnEditMark.Visible = true;
            btnDelMark.Visible = true;

            tpMark.Parent = tabMain;
            tpCheck.Parent = null;
            tpCL.Parent = null;
            tabUnCheck.Parent = null;
        }

        private TreeNode CellNo1Node { get; set; }
        private void CreateNewTemplateInfo()
        {
            TEMPLATE_INFO = new TEMPLATE_INFO();
            TEMPLATE_INFO.sTemplateInfoProp.byCreateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            TEMPLATE_INFO.sTemplateInfoProp.byModelName = "test";
            flowLayoutPanel1.Enabled = true;
            CELL_INFO cELL_INFO = new CELL_INFO();
            TEMPLATE_INFO.vCellInfo.Add(cELL_INFO);
            CellNo1Node = tvTemp.Nodes[0].Nodes[1].Nodes[0];
        }

        private void FrmMain_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            IsControlDown = e.Control;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TEMPLATE_INFO.sTemplateInfoProp.nWidth = BaseMap.Width;
            TEMPLATE_INFO.sTemplateInfoProp.nHeight = BaseMap.Height;
            TEMPLATE_INFO.sTemplateInfoProp.nTemplateW = BaseMap.Width;
            TEMPLATE_INFO.sTemplateInfoProp.nTemplateH = BaseMap.Height;

            TEMPLATE_INFO.sTemplateInfoProp.rtModelPostion = new Rectangle(new Point(0, 0), BaseMap.Size);
            TEMPLATE_INFO.sTemplateInfoProp.rtCellMarks.Add(new Rectangle(new Point(0, 0), BaseMap.Size));

            TEMPLATE_INFO.vCellInfo[0].sCellRegion = new MODEL_REGION();
            TEMPLATE_INFO.vCellInfo[0].sCellRegion.nRegionType = 1;
            TEMPLATE_INFO.vCellInfo[0].sCellRegion.vModelPoints.Add(new Point(0, 0));
            TEMPLATE_INFO.vCellInfo[0].sCellRegion.vModelPoints.Add(new Point(BaseMap.Width, 0));
            TEMPLATE_INFO.vCellInfo[0].sCellRegion.vModelPoints.Add(new Point(BaseMap.Width, BaseMap.Height));
            TEMPLATE_INFO.vCellInfo[0].sCellRegion.vModelPoints.Add(new Point(0, BaseMap.Height));

            var content = JsonConvert.SerializeObject(TEMPLATE_INFO);
            File.WriteAllText("TEMPLATE_INF.JSON", content);
            try
            {
                VisionChecker vision = new VisionChecker(BaseMap.Width, BaseMap.Height);
                vision.ModelOperate(e_ModelFLAG.FLAG_INIT, content, BaseMap);
                MessageBox.Show("OK");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }

        private void btnAddCheck_Click(object sender, EventArgs e)
        {
            if (Model_Points.Count == 0) { MessageBox.Show("请选绘制区域"); return; }

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

            iNSPECT_REGION.sInspectRegion = GetMODEL_REGION();
            iNSPECT_REGION.sInspectProp.nRegionNum = CellNo1Node.Nodes[0].Nodes.Count + 1;
            TEMPLATE_INFO.vCellInfo[0].vInspectRegion.Add(iNSPECT_REGION);
            TreeNode node = new TreeNode(iNSPECT_REGION.sInspectProp.nRegionNum.ToString());
            node.Tag = iNSPECT_REGION;
            TreeNode uncheck = new TreeNode("不检测");
            node.Nodes.Add(uncheck);
            CellNo1Node.Nodes[0].Nodes.Add(node);
            tvTemp.ExpandAll();
        }

        private void btnAddCL_Click(object sender, EventArgs e)
        {
            if (Model_Points.Count == 0) { MessageBox.Show("请选绘制区域"); return; }

            INSPECT_REGION iNSPECT_REGION = new INSPECT_REGION();

            #region 测量属性

            iNSPECT_REGION.sInspectProp = new INSPECT_PROP();
            iNSPECT_REGION.sInspectProp.byRegionMode = 1;

            iNSPECT_REGION.sInspectProp.byRegionModeSecond = (byte)cmbCL.SelectedIndex;

            iNSPECT_REGION.sInspectProp.sRegionPara.sMeasurePara.fDistUp = (float)numCL1.Value;
            iNSPECT_REGION.sInspectProp.sRegionPara.sMeasurePara.fDistDown = (float)numCL2.Value;

            iNSPECT_REGION.sInspectProp.sRegionProp.sMeasureProp.sEdgeLeftUp.byGray = (byte)numCL3.Value;
            iNSPECT_REGION.sInspectProp.sRegionProp.sMeasureProp.sEdgeRightBottom.byGray = (byte)numCL3.Value;

            #endregion


            #region 形状

            iNSPECT_REGION.sInspectRegion = GetMODEL_REGION();

            #endregion

            iNSPECT_REGION.sInspectProp.nRegionNum = CellNo1Node.Nodes[1].Nodes.Count + 1;
            TEMPLATE_INFO.vCellInfo[0].vInspectRegion.Add(iNSPECT_REGION);
            TreeNode node = new TreeNode(iNSPECT_REGION.sInspectProp.nRegionNum.ToString());
            node.Tag = iNSPECT_REGION;
            TreeNode uncheck = new TreeNode("不检测");
            node.Nodes.Add(uncheck);
            CellNo1Node.Nodes[1].Nodes.Add(node);
            tvTemp.ExpandAll();
        }


        private void btnAddUnCheck_Click(object sender, EventArgs e)
        {
            if (Model_Points.Count == 0) { MessageBox.Show("请选绘制区域"); return; }

            INSPECT_REGION iNSPECT_REGION = tvTemp.SelectedNode.Parent.Tag as INSPECT_REGION;
            if (iNSPECT_REGION == null) return;
            if (Model_Points.Count == 0) return;
            UNINSPECT_REGION uNINSPECT_REGION = new UNINSPECT_REGION();
            uNINSPECT_REGION.sUnInspectRegionProp.nRegionNum = tvTemp.SelectedNode.Nodes.Count + 1;
            uNINSPECT_REGION.rgnUnInspectRegion = GetMODEL_REGION();

            iNSPECT_REGION.vUnInspectRegion.Add(uNINSPECT_REGION);

            TreeNode node = new TreeNode(uNINSPECT_REGION.sUnInspectRegionProp.nRegionNum.ToString());
            node.Tag = uNINSPECT_REGION;
            tvTemp.SelectedNode.Nodes.Add(node);
            tvTemp.ExpandAll();
        }

        private MODEL_REGION GetMODEL_REGION()
        {

            MODEL_REGION tmp = new MODEL_REGION();
            switch (RegionType)
            {
                case RegionType.Rectangl:
                    tmp.nRegionType = 1;
                    break;
                case RegionType.Circle:
                    tmp.nRegionType = 3;
                    break;
                case RegionType.Rolygon:
                    tmp.nRegionType = 2;
                    break;
            }
            tmp.vModelPoints = Model_Points.ToList();

            return tmp;
        }

        private void FrmMain_KeyUp(object sender, KeyEventArgs e)
        {
            IsControlDown = e.Control;
            if (!e.Alt) { this.Cursor = Cursors.Default; }
        }

        private void ClearDrawTemp()
        {
            IsStart = false;
            StartPoint = new Point();
            EndPoint = new Point();
            Points.Clear();
            Model_Points.Clear();
        }

        private Bitmap midMap { get; set; }

        private void DrawTemp(MODEL_REGION hilight = null)
        {


            if (BaseMap == null) return;
            if (midMap != null) midMap.Dispose();
            midMap = (Bitmap)BaseMap.Clone();
            using (Graphics g = Graphics.FromImage(midMap))
            {
                foreach (var tmp in TEMPLATE_INFO.vMainMarkRegion)
                {
                    DrawMarkRegion(g, tmp.sMarkRegion);
                }
                foreach (var cell in TEMPLATE_INFO.vCellInfo)
                {
                    foreach (var inspectRegion in cell.vInspectRegion)
                    {
                        DrawMarkRegion(g, inspectRegion.sInspectRegion);
                        foreach (var unCheckRegion in inspectRegion.vUnInspectRegion)
                        {
                            DrawMarkRegion(g, unCheckRegion.rgnUnInspectRegion);
                        }
                    }
                }

                if (hilight != null)
                {
                    DrawMarkRegion(g, hilight, new Pen(Brushes.Red, 3));
                }
            }
            if (picContent.Image != null) picContent.Image.Dispose();
            this.picContent.Image = (Bitmap)midMap.Clone();
        }
        private void DrawMarkRegion(Graphics g, MODEL_REGION model_region, Pen pen = null)
        {
            var tmpPen = MyPen;
            if (pen != null) tmpPen = pen;
            switch (model_region.nRegionType)
            {
                case 1:
                    {
                        var x = model_region.vModelPoints[0].X;
                        var y = model_region.vModelPoints[0].Y;
                        var w = model_region.vModelPoints[2].X - model_region.vModelPoints[0].X;
                        var h = model_region.vModelPoints[2].Y - model_region.vModelPoints[0].Y;
                        g.DrawRectangle(tmpPen, x, y, w, h);
                    }
                    break;
                case 2:
                    g.DrawLines(tmpPen, model_region.vModelPoints.ToArray());
                    break;
                case 3:
                    {
                        var x = model_region.vModelPoints[0].X;
                        var y = model_region.vModelPoints[0].Y;
                        var w = model_region.vModelPoints[1].X;
                        var h = model_region.vModelPoints[1].Y;
                        g.DrawEllipse(tmpPen, x - w, y - h, w * 2, h * 2);
                    }
                    break;
            }
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnEditMark_Click(object sender, EventArgs e)
        {
            if (rbMarkModel0.Checked)
            {
                Selected_MARK_REGION.sMarkProp.byMode = 0;
            }
            else
            {
                Selected_MARK_REGION.sMarkProp.byMode = 1;
            }
            Selected_MARK_REGION.sMarkProp.fRatio = (float)this.numMarkRatio.Value;
            Selected_MARK_REGION.sMarkProp.nSearchX = (int)this.numMarkSearchX.Value;
            Selected_MARK_REGION.sMarkProp.nSearchY = (int)this.numMarkSearchY.Value;

            if (Model_Points.Count > 0)
            {
                Selected_MARK_REGION.sMarkRegion = GetMODEL_REGION();

            }

            tvTemp.SelectedNode.Tag = Selected_MARK_REGION;
            tvTemp.ExpandAll();
            DrawTemp(Selected_MARK_REGION.sMarkRegion);
        }

        private void btnEditCheck_Click(object sender, EventArgs e)
        {
            #region 检测属性
            if (chkGeneral.Checked)
            {
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.bAllShape = true;
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sSpotPara.sSpotUp.nGray = (int)numWhite1.Value;
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sSpotPara.sSpotUp.fMaxArea = (int)numWhite2.Value;

                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sSpotPara.sSpotDown.nGray = (int)numBlack1.Value;
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sSpotPara.sSpotDown.fMaxArea = (int)numBlack2.Value;
            }

            if (chkGrade.Checked)
            {
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.bMircoAllShape = true;
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMircoAllPara.sSpotUp.nGray = (int)numGradeWhite1.Value;
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMircoAllPara.sSpotUp.fMaxArea = (int)numGradeWhite2.Value;
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMircoAllPara.sSpotDown.nGray = (int)numGradeBlack1.Value;
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMircoAllPara.sSpotDown.fMaxArea = (int)numGradeBlack2.Value;
            }

            if (chkArea.Checked)
            {
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.bMeasureFrontArea = true;
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMeasureFrontAreaPara.fDistUp = (int)numArea1.Value;
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMeasureFrontAreaPara.fDistDown = (int)numArea2.Value;
            }


            if (chkNum.Checked)
            {
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.bMeasureFrontNum = true;
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMeasureFrontNumPara.fDistUp = (int)numNum1.Value;
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMeasureFrontNumPara.fDistDown = (int)numNum2.Value;
            }


            if (chkLocation.Checked)
            {
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.bOverPrint = true;
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sOverprintPara.fDistHor = (int)numLocalH.Value;
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sOverprintPara.fDistVer = (int)numLocalV.Value;
            }

            if (chkColor.Checked)
            {
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.bCheckColor = true;
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sColorPara.byEnable = 1;
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sColorPara.fDeltaE = (int)numColor1.Value;
            }

            if (chkStat.Checked)
            {
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.bStatisticPara = true;
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sStatisticInfo.fValue = (int)numStatArea.Value;
                Selected_INSPECT_REGION.sInspectProp.sRegionPara.sStatisticInfo.nCount = (int)numStatNum.Value;
            }

            if (chkWT.Checked)
            {
                Selected_INSPECT_REGION.sInspectProp.sRegionProp.sFrontProp.nDialtion = (int)numWTPZ.Value;
                Selected_INSPECT_REGION.sInspectProp.sRegionProp.sFrontProp.byThredHigh = (byte)numWTHight.Value;
                Selected_INSPECT_REGION.sInspectProp.sRegionProp.sFrontProp.byThredLow = (byte)numWTLow.Value;
                Selected_INSPECT_REGION.sInspectProp.sRegionProp.sFrontProp.byThredType = (byte)cmbWT1.SelectedIndex;
            }
            #endregion
            if (Model_Points.Count > 0)
            {
                Selected_INSPECT_REGION.sInspectRegion = GetMODEL_REGION();
            }
            tvTemp.SelectedNode.Tag = Selected_MARK_REGION;
            tvTemp.ExpandAll();
            DrawTemp(Selected_MARK_REGION.sMarkRegion);
        }

        private void btnEditCL_Click(object sender, EventArgs e)
        {

            #region 测量属性

            Selected_INSPECT_REGION.sInspectProp.byRegionModeSecond = (byte)cmbCL.SelectedIndex;

            Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMeasurePara.fDistUp = (float)numCL1.Value;
            Selected_INSPECT_REGION.sInspectProp.sRegionPara.sMeasurePara.fDistDown = (float)numCL2.Value;

            Selected_INSPECT_REGION.sInspectProp.sRegionProp.sMeasureProp.sEdgeLeftUp.byGray = (byte)numCL3.Value;
            Selected_INSPECT_REGION.sInspectProp.sRegionProp.sMeasureProp.sEdgeRightBottom.byGray = (byte)numCL3.Value;

            #endregion


            #region 形状
            if (Model_Points.Count > 0)
            {
                Selected_INSPECT_REGION.sInspectRegion = GetMODEL_REGION();
            }

            #endregion

            tvTemp.SelectedNode.Tag = Selected_MARK_REGION;
            tvTemp.ExpandAll();
            DrawTemp(Selected_MARK_REGION.sMarkRegion);
        }

        private void btnEditUnCheck_Click(object sender, EventArgs e)
        {
            if (Model_Points.Count > 0)
            {
                Selected_UNINSPECT_REGION.rgnUnInspectRegion = GetMODEL_REGION();
            }
            tvTemp.SelectedNode.Tag = Selected_UNINSPECT_REGION;
            tvTemp.ExpandAll();
            DrawTemp(Selected_UNINSPECT_REGION.rgnUnInspectRegion);
        }

        private void btnDelMark_Click(object sender, EventArgs e)
        {
            TEMPLATE_INFO.vMainMarkRegion.Remove(Selected_MARK_REGION);
            DrawTemp();
        }

        private void btnDelCheck_Click(object sender, EventArgs e)
        {
            TEMPLATE_INFO.vCellInfo[0].vInspectRegion.Remove(Selected_INSPECT_REGION);
            DrawTemp();
            tvTemp.SelectedNode.Parent.Nodes.Remove(tvTemp.SelectedNode);
        }

        private void btnDelCL_Click(object sender, EventArgs e)
        {
            TEMPLATE_INFO.vCellInfo[0].vInspectRegion.Remove(Selected_INSPECT_REGION);
            DrawTemp();
            tvTemp.SelectedNode.Parent.Nodes.Remove(tvTemp.SelectedNode);
        }

        private void btnDelUnCheck_Click(object sender, EventArgs e)
        {
            INSPECT_REGION iNSPECT_REGION = tvTemp.SelectedNode.Parent.Parent.Tag as INSPECT_REGION;
            iNSPECT_REGION.vUnInspectRegion.Remove(Selected_UNINSPECT_REGION);
            DrawTemp();
            tvTemp.SelectedNode.Parent.Parent.Nodes.Remove(tvTemp.SelectedNode);
        }

        private void bsmBatch_Click(object sender, EventArgs e)
        {
            FrmBatchRuncs frm = new FrmBatchRuncs();
            frm.ShowDialog();

        }

        #region 移动
        private bool IsMove { get; set; }
        private Point MouseDownPoint = new Point(); //记录拖拽过程鼠标位置
        private int ZoomStep { get; set; } = 60;   //缩放步长
        #endregion
        private void tsbMove_Click(object sender, EventArgs e)
        {
            if (this.Cursor == Cursors.SizeAll)
            {
                this.Cursor = Cursors.Default;
            }
            else
            {
                this.Cursor = Cursors.SizeAll;
            }

        }

        private void pnlContent_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.Cursor == Cursors.SizeAll && e.Button == MouseButtons.Left)
            {
                IsMove = true;
                MouseDownPoint = Cursor.Position;
                picContent.Focus();
            }
        }

        private void pnlContent_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMove)
            {
                int x, y;
                int moveX, moveY;
                moveX = Cursor.Position.X - MouseDownPoint.X;
                moveY = Cursor.Position.Y - MouseDownPoint.Y;
                x = picContent.Location.X + moveX;
                y = picContent.Location.Y + moveY;
                picContent.Location = new Point(x, y);
                MouseDownPoint.X = Cursor.Position.X;
                MouseDownPoint.Y = Cursor.Position.Y;
            }
        }

        private void pnlContent_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsMove)
            {
                IsMove = false;
            }
        }

        private void tsbZoomIn_Click(object sender, EventArgs e)
        {
            Zoom(new Point(0, 0), true);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Zoom(new Point(0, 0), false);
        }

        private void tsbReset_Click(object sender, EventArgs e)
        {
            ReSetPictureSize();
        }

        private Point TransPoint(Point point)
        {
            if (BaseMap == null) return point;
            var wv = (double)picContent.Width / BaseMap.Width;
            var hv = (double)picContent.Height / BaseMap.Height;

            Point newPoint = new Point();
            newPoint.X = (int)(point.X / wv);
            newPoint.Y = (int)(point.Y / hv);

            return newPoint;
        }

        private void tsbFull_Click(object sender, EventArgs e)
        {

            picContent.Width = pnlContent.Width;
            picContent.Height = pnlContent.Height;
            PropertyInfo pInfo = picContent.GetType().GetProperty("ImageRectangle", BindingFlags.Instance |
                 BindingFlags.NonPublic);
            Rectangle rect = (Rectangle)pInfo.GetValue(picContent, null);
            picContent.Width = rect.Width;
            picContent.Height = rect.Height;
            picContent.Location = new Point(rect.X, rect.Y);
            return;
            //var width = BaseMap.Width;
            //var height = BaseMap.Height;


            //var diffWidth = Math.Abs(BaseMap.Width - pnlContent.Width);
            //var diffHeight = Math.Abs(BaseMap.Height - pnlContent.Height);


            //var deff = Math.Max(diffWidth, diffHeight);
            //// deff = (int)(deff / ZoomStep) * ZoomStep;

            //if (BaseMap.Width > pnlContent.Width || BaseMap.Height > pnlContent.Height)
            //{
            //    picContent.Width = BaseMap.Width - deff;
            //    picContent.Height = BaseMap.Height - deff;
            //    PropertyInfo pInfo = picContent.GetType().GetProperty("ImageRectangle", BindingFlags.Instance |
            //     BindingFlags.NonPublic);
            //    Rectangle rect = (Rectangle)pInfo.GetValue(picContent, null);
            //    //picContent.Width = rect.Width;
            //    //picContent.Height = rect.Height;
            //}
            //else
            //{
            //    picContent.Width = BaseMap.Width + deff;
            //    picContent.Height = BaseMap.Height + deff;
            //    //PropertyInfo pInfo = picContent.GetType().GetProperty("ImageRectangle", BindingFlags.Instance |
            //    // BindingFlags.NonPublic);
            //    //Rectangle rect = (Rectangle)pInfo.GetValue(picContent, null);
            //    //picContent.Width = rect.Width;
            //    //picContent.Height = rect.Height;
            //}

            // this.picContent.Location = new Point(0, 0);
        }

        private void tsbOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.jpg|*.jpg|*.bmp|*.bmp";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileName = ofd.FileName;
                BaseMap = (Bitmap)DZSOFT.Common.ImageUtil.ReadImage(FileName);
                this.picContent.Image = (Bitmap)BaseMap.Clone();
                this.picContent.Width = BaseMap.Width;
                this.picContent.Height = BaseMap.Height;
                this.picContent.Location = new Point(0, 0);

                this.tsbCircle.Enabled = true;
                this.tsbRectangle.Enabled = true;
                this.tsbPolygon.Enabled = true;
                this.tsbRectangle.Enabled = true;
                this.tsbZoomIn.Enabled = true;
                this.tsbZoonOut.Enabled = true;
                this.tsbMove.Enabled = true;
                this.tsbFull.Enabled = true;

            }
        }
    }
}

