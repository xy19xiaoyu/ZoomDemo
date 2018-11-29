using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DZSoft.IMG.Template.Model
{
    /// <summary>
    /// 注意使用完后要调用dzFreePtr释放imgData
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DzBitmap
    {
        [MarshalAs(UnmanagedType.I4)]
        public Int32 width;
        [MarshalAs(UnmanagedType.I4)]
        public Int32 height;
        [MarshalAs(UnmanagedType.I4)]
        public Int32 stride;

        [MarshalAs(UnmanagedType.I4)]
        public Int32 type;

        public IntPtr imgData;
    }



    /// <summary>
    /// 非检测区域属性
    /// </summary>
    public class UNINSPECT_REGION_PROP
    {
        public int nRegionNum { get; set; }
    }

    /// <summary>
    /// 非检测区域
    /// </summary>
    public class UNINSPECT_REGION
    {
        public UNINSPECT_REGION()
        {
            sUnInspectRegionProp = new UNINSPECT_REGION_PROP();
            rgnUnInspectRegion = new MODEL_REGION();
        }
        public UNINSPECT_REGION_PROP sUnInspectRegionProp { get; set; }
        public MODEL_REGION rgnUnInspectRegion { get; set; }
    }

    /// <summary>
    ///多模板参数
    /// </summary>
    public class MULTIINSPECT_PROP
    {
        /// <summary>
        /// 多模板区域的ID号，不能重
        /// </summary>
        public int nMultiID { get; set; }
        /// <summary>
        /// 多模板对应的模板图像索引号
        /// </summary>
        public int nMultiImageIndex { get; set; }
        public bool bNew { get; set; }     // FALSE时：如果不是新建就不存图
        public INSPECT_PROP sInspectProp { get; set; }
        public int nMarkCount { get; set; }
        /// <summary>
        /// MARK区域链表
        /// </summary>
        public List<MARK_REGION> vMarkInfo { get; set; }
    }

    /// <summary>
    /// MARK区域结构
    /// </summary>
    public class MARK_REGION
    {
        public MARK_REGION()
        {
            sMarkProp = new MARK_PROP();
            sMarkRegion = new MODEL_REGION();
        }
        /// <summary>
        /// mark属性
        /// </summary>
        public MARK_PROP sMarkProp { get; set; }
        /// <summary>
        /// MARK区域在原始图像中的坐标
        /// </summary>
        public MODEL_REGION sMarkRegion { get; set; }
    }
    public class MEASURE_EDGE
    {
        /// <summary>
        /// 灰度阈值
        /// </summary>
        public byte byGray { get; set; }
        /// <summary>
        /// 0:从左上  1：从右下
        /// </summary>
        public byte byOrientation { get; set; }
        /// <summary>
        /// 测量通道，自动获取并保存
        /// </summary>
        public byte byChannel { get; set; }
    }

    public class MEASURE_PROP
    {
        public MEASURE_PROP()
        {
            byOritation = 0;
            sEdgeLeftUp = new MEASURE_EDGE();
            sEdgeRightBottom = new MEASURE_EDGE();
        }
        /// <summary>
        /// 0：水平方向，1：垂直方向
        /// </summary>
        public byte byOritation { get; set; }
        public MEASURE_EDGE sEdgeLeftUp;      //对应界面：上/左边缘对比度
        public MEASURE_EDGE sEdgeRightBottom; //对应界面：下/右边缘对比度
    }
    public class FRONT_REGION
    {
        public FRONT_REGION()
        {
            byThredType = 1;
            nDialtion = 1;
            byThredLow = 1;
            byThredHigh = 1;
        }
        /// <summary>
        /// 分割类型：,0：固定阈值 1：自动提取
        /// </summary>
        public byte byThredType { get; set; }
        /// <summary>
        /// 0膨胀尺寸
        /// </summary>
        public int nDialtion { get; set; }
        /// <summary>
        /// 低阈值
        /// </summary>
        public byte byThredLow { get; set; }
        /// <summary>
        /// 高阈值
        /// </summary>
        public byte byThredHigh { get; set; }
    }
    /// <summary>
    /// 检测区域的属性
    /// </summary>
    public class CHECK_PROP
    {
        public CHECK_PROP()
        {
            sFrontProp = new FRONT_REGION();
            sMeasureProp = new MEASURE_PROP();

        }
        public FRONT_REGION sFrontProp { get; set; }

        //边缘测量的属性
        public MEASURE_PROP sMeasureProp { get; set; }
    }

    /// <summary>
    /// 位置偏差
    /// </summary>
    public class OVERPRINT_CHECK_PARA
    {
        public float fDistHor;      //对应界面  水平
        public float fDistVer;      //对应界面  垂直
    }
    /// <summary>
    /// 偏差
    /// </summary>
    public class MEASURE_CHECK_PARA
    {
        //检测参数，测量面积、个数时使用	
        public float fDistUp { get; set; }        //对应界面  偏大
        public float fDistDown { get; set; }     //对应界面  偏小
    }
    /// <summary>
    /// 统计
    /// </summary>

    public class STATIC_INFO
    {
        /// <summary>
        /// 个数
        /// </summary>
        public int nCount { get; set; }         //缺陷个数   对应界面：
        /// <summary>
        /// 面积
        /// </summary>
        public float fValue { get; set; }           //统计的值   对应界面：
    }

    /// <summary>
    /// 普通检测
    /// </summary>
    public class CHECKPARA_GENERAL
    {
        public int nGray;               //对比度;    //对应界面：对比度
        public float fMaxArea;          //最大面积(考虑到以后可能改为mm，使用float类型)  对应界面：最大面积
    }
    /// <summary>
    /// 标准检测
    /// </summary>
    public class GENERAL_CHECK_PARA
    {
        public GENERAL_CHECK_PARA()
        {
            sSpotUp = new CHECKPARA_GENERAL();
            sSpotDown = new CHECKPARA_GENERAL();
        }
        /// <summary>
        /// “白”缺陷参数
        /// </summary>
        public CHECKPARA_GENERAL sSpotUp { get; set; }
        /// <summary>
        /// “黑”缺陷参数
        /// </summary>
        public CHECKPARA_GENERAL sSpotDown { get; set; }
    }
    public class CHECKPARA_COLOR
    {
        public byte byEnable { get; set; } = 0;
        public float fDeltaE { get; set; } = 0;
    }

    public class MARK_PROP
    {
        /// <summary>
        /// 递增 0 开始
        /// </summary>
        public int nRegionNum { get; set; }
        /// <summary>
        /// nIndependentImg:如果有4个相机拍摄，每2个相机拼接后一同处理，那么nIndependentImg有2个，同一个模板中有分在2个位置存储
        /// </summary>
        public int nIndependentImg { get; set; } = 0;
        /// <summary>
        /// 小开号
        /// </summary>
        public int nCellID { get; set; } = 0;
        /// <summary>
        /// 0：主定位核，1：次定位核 
        /// </summary>
        public byte byMarkType { get; set; } = 0;
        /// <summary>
        /// 采用何种配备方式0：图像预处理；1：原始图像; 
        /// </summary>
        public byte byMode { get; set; } = 0;
        /// <summary>
        /// 匹配率
        /// </summary>
        public float fRatio { get; set; } = 0.1F;
        /// <summary>
        /// 左右搜索范围
        /// </summary>
        public int nSearchX { get; set; } = 10;
        /// <summary>
        /// 垂直搜索范围
        /// </summary>
        public int nSearchY { get; set; } = 10;
    }
    /// <summary>
    ///  检测参数
    /// </summary>
    public class CHECK_PARA
    {
        public CHECK_PARA()
        {
            bAllShape = false;

            sSpotPara = new GENERAL_CHECK_PARA();

            bMircoAllShape = false;
            sMircoAllPara = new GENERAL_CHECK_PARA();


            bStatisticPara = false;
            sStatisticInfo = new STATIC_INFO();

            bMeasureFrontArea = false;
            sMeasureFrontAreaPara = new MEASURE_CHECK_PARA();

            bMeasureFrontNum = false;
            sMeasureFrontNumPara = new MEASURE_CHECK_PARA();

            bOverPrint = false;
            sOverprintPara = new OVERPRINT_CHECK_PARA();

            bCheckColor = false;
            sColorPara = new CHECKPARA_COLOR();

            sMeasurePara = new MEASURE_CHECK_PARA();
        }
        /// <summary>
        /// 普通检测
        /// </summary>
        public bool bAllShape { get; set; }
        /// <summary>
        /// 普通检测
        /// </summary>
        public GENERAL_CHECK_PARA sSpotPara { get; set; }

        /// <summary>
        /// 精细检测
        /// </summary>
        public bool bMircoAllShape { get; set; }
        /// <summary>
        /// 精细检测
        /// </summary>
        public GENERAL_CHECK_PARA sMircoAllPara { get; set; }

        /// <summary>
        /// 统计
        /// </summary>
        public bool bStatisticPara { get; set; }
        /// <summary>
        /// 统计
        /// </summary>
        public STATIC_INFO sStatisticInfo { get; set; }



        /// <summary>
        /// 面积偏差
        /// </summary>
        public bool bMeasureFrontArea { get; set; }
        /// <summary>
        /// 面积偏差
        /// </summary>
        public MEASURE_CHECK_PARA sMeasureFrontAreaPara { get; set; }
        /// <summary>
        /// 个数偏差
        /// </summary>
        public bool bMeasureFrontNum { get; set; }
        /// <summary>
        /// 个数偏差
        /// </summary>
        public MEASURE_CHECK_PARA sMeasureFrontNumPara { get; set; }
        /// <summary>
        /// 位置偏差
        /// </summary>
        public bool bOverPrint { get; set; }
        /// <summary>
        /// 位置偏差
        /// </summary>
        public OVERPRINT_CHECK_PARA sOverprintPara { get; set; }

        /// <summary>
        /// 色差复选框
        /// </summary>
        public bool bCheckColor { get; set; }
        /// <summary>
        /// 色差
        /// </summary>
        public CHECKPARA_COLOR sColorPara { get; set; }
        /// <summary>
        /// 测量参数：偏大、偏小
        /// </summary>

        public MEASURE_CHECK_PARA sMeasurePara { get; set; }
    }

    public class INSPECT_PROP
    {
        public INSPECT_PROP()
        {
            sRegionPara = new CHECK_PARA();
            sRegionProp = new CHECK_PROP();
        }
        /// <summary>
        /// 检测区域号
        /// </summary>
        public int nRegionNum;
        /// <summary>
        /// 是否学习标志，：不学习，：学习
        /// </summary>
        public byte byStudy { get; set; } = 0;
        /// <summary>
        /// 是否学习、添加多模板标志。选中标志
        /// </summary>
        public byte bySelect { get; set; } = 0;
        /// <summary>
        ///  检测0，测量1
        /// </summary>
        public byte byRegionMode;

        public byte byRegionModeSecond { get; set; } = 0;

        /// <summary>
        /// 小开号
        /// </summary>
        public int nCellID { get; set; } = 0;
        /// <summary>
        /// 当前多模板序号
        /// </summary>
        public int nCurMultiIndex { get; set; } = 0;
        /// <summary>
        /// 检测参数
        /// </summary>
        public CHECK_PARA sRegionPara;
        /// <summary>
        /// ???
        /// </summary>
        public CHECK_PROP sRegionProp;
        public float fEdgeCtrl { get; set; } = 1;
    }

    /// <summary>
    /// 区域信息
    /// </summary>
    public class INSPECT_REGION
    {
        public INSPECT_REGION()
        {
            sInspectProp = new INSPECT_PROP();
            vMarkRegion = new List<MARK_REGION>();
            vMultiInspectProp = new List<MULTIINSPECT_PROP>();
            vUnInspectRegion = new List<UNINSPECT_REGION>();
        }
        /// <summary>
        /// 区域属性
        /// </summary>
        public INSPECT_PROP sInspectProp { get; set; }
        public int nMarkRegCount
        {
            get { return vMarkRegion.Count; }
        }
        /// <summary>
        /// 子定位信息
        /// </summary>
        public List<MARK_REGION> vMarkRegion;

        //0不使用多模板 0增加的多模板个数
        public int nMultiModel { get; set; } = 0;
        /// <summary>
        /// 多模板信息
        /// </summary>
        public List<MULTIINSPECT_PROP> vMultiInspectProp { get; set; }
        /// <summary>
        /// 在图像中的坐标
        /// </summary>
        public MODEL_REGION sInspectRegion { get; set; }

        public int nUnInspectRegCount
        {
            get { return vUnInspectRegion.Count; }
        }
        public List<UNINSPECT_REGION> vUnInspectRegion { get; set; }
    }

    public class MODEL_REGION
    {
        public MODEL_REGION()
        {
            vModelPoints = new List<Point>();
        }
        public int nModelCount
        {
            get
            {
                return vModelPoints.Count;
            }
        }
        /// <summary>
        /// 原始图像中的坐标
        /// </summary>
        public List<Point> vModelPoints { get; set; }
        /// <summary>
        /// 1-矩形；2-多边形；3-椭圆
        /// </summary>
        public int nRegionType { get; set; }
    }

    /// <summary>
    /// 小开 信息
    /// </summary>
    public class CELL_INFO_PROP
    {
        public int nCellId { get; set; } = 0;
        public byte byCellRow { get; set; } = 0;
        public byte byCellCol { get; set; } = 0;
    }


    /// <summary>
    /// 小开
    /// </summary>
    public class CELL_INFO
    {
        public CELL_INFO()
        {
            sCellInfoProp = new CELL_INFO_PROP();
            vInspectRegion = new List<INSPECT_REGION>();
        }
        /// <summary>
        /// 小开信息
        /// </summary>
        public CELL_INFO_PROP sCellInfoProp { get; set; }

        /// <summary>
        /// 图像上所有的框选图形
        /// </summary>
        public MODEL_REGION sCellRegion { get; set; }
        public int nInspectRegCount
        {
            get { return vInspectRegion.Count; }
        }
        public List<INSPECT_REGION> vInspectRegion { get; set; }
    }

    public class TEMPLATE_INFO_PROP
    {
        public TEMPLATE_INFO_PROP()
        {
            nIndependentImgNo = 0;
            nStudyNum = 0;
            nChannel = 3;
            rtCellMarks = new List<Rectangle>();
        }
        /// <summary>
        /// 拼接后的图像号
        /// </summary>
        public int nIndependentImgNo { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string byModelName { get; set; }
        /// <summary>
        /// 学习张数
        /// </summary>
        public int nStudyNum { get; set; } = 0;
        /// <summary>
        /// YYYYMMDDHHNNSS
        /// </summary>
        public string byCreateTime { get; set; }
        /// <summary>
        /// 采集图像宽
        /// </summary>
        public int nWidth { get; set; }
        /// <summary>
        /// 采集图像高
        /// </summary>
        public int nHeight { get; set; }
        /// <summary>
        /// 模板图像宽
        /// </summary>
        public int nTemplateW { get; set; }
        /// <summary>
        /// 模板图像高
        /// </summary>
        public int nTemplateH { get; set; }
        /// <summary>
        /// 模板图像通道数（3和1）
        /// </summary>
        public int nChannel { get; set; } = 3;
        /// <summary>
        /// 模板图像在采集图上的位置
        /// </summary>
        public Rectangle rtModelPostion { get; set; }
        /// <summary>
        /// 小开区域Mark位置，最多支持5个Mark
        /// </summary>
        public List<Rectangle> rtCellMarks { get; set; }
        /// <summary>
        /// 第一小开在大图中的区域
        /// </summary>
        public Rectangle rtFirstCell
        {
            get
            {
                if (rtCellMarks.Count == 0) return new Rectangle();
                return rtCellMarks.First();
            }
        }
        /// <summary>
        /// 小开的行数
        /// </summary>
        public byte byRowNum { get; set; } = 1;
        /// <summary>
        /// 小开的列数
        /// </summary>
        public byte byColNum { get; set; } = 1;
        /// <summary>
        /// 使用的图集名称 20byte
        /// </summary>
        public string byImgSetName;   //默认				
        public byte byModelVersion { get; set; } = 3;
        /// <summary>
        /// 多模板图像的个数，不包含原始模板图
        /// </summary>
        public int nMultiImageCount { get; set; } = 0;
        /// <summary>
        /// 当前显示多模板的索引号，从0开始。0表示原始模板图像
        /// </summary>
        public int nCurImageIndex { get; set; } = 0;

    }


    public class TEMPLATE_INFO
    {
        public TEMPLATE_INFO()
        {
            vCellInfo = new List<CELL_INFO>();
            vMainMarkRegion = new List<MARK_REGION>();
            sTemplateInfoProp = new TEMPLATE_INFO_PROP();
        }
        public TEMPLATE_INFO_PROP sTemplateInfoProp { get; set; }
        public int nCellCount
        {
            get
            {
                return vCellInfo.Count;
            }
        }
        /// <summary>
        /// 小开区域信息
        /// </summary>
        public List<CELL_INFO> vCellInfo { get; set; }
        public int nMainMarkCount
        {
            get
            {
                return vMainMarkRegion.Count;
            }
        }
        /// <summary>
        /// MARK区域链表
        /// </summary>
        public List<MARK_REGION> vMainMarkRegion { get; set; }

    }
    /// <summary>
    /// 检测结果
    /// </summary>
    public class INSPECT_RESULT_INFO
    {
        /// <summary>
        /// 小开号码
        /// </summary>
        public int nCellId { get; set; }
        /// <summary>
        /// 开对应的行号
        /// </summary>
        public byte byCellRow { get; set; }
        /// <summary>
        /// 开对应的列号
        /// </summary>
        public byte byCellCol { get; set; }

        public int nRegionId { get; set; }
        /// <summary>
        /// 算法类型,功能：使用该变量返回错误所属于的检测区域类型
        /// </summary>
        public int nAlgType { get; set; }
        public int nAlgTypeSecond { get; set; }
        /// <summary>
        /// 缺陷类别（配置文件指定）按位设置缺陷类型
        /// </summary>
        public byte byErrorType { get; set; }
        /// <summary>
        /// 缺陷面积或测量值
        /// </summary>
        public float fErrorValue { get; set; }

        /// <summary>
        /// 多模板ID//nModelIndex
        /// </summary>
        public int nMultiID { get; set; }
        /// <summary>
        /// 多模板图像的索引
        /// </summary>
        public int nModelIndex { get; set; }
        /// <summary>
        /// 区域相对于采集图像中的位置
        /// </summary>
        public Rectangle rcOrigin { get; set; }
        /// <summary>
        /// 区域相对于模板图像的区域
        /// </summary>
        public Rectangle rcModel { get; set; }
    }
}
