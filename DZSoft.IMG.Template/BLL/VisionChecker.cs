using DZSoft.IMG.Template.Model;
using DZSoft.IMG.Template.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DZSoft.IMG.Template.BLL
{
    public class VisionChecker : IDisposable
    {
        #region C++ DLL 调用
        [DllImport("VisionCheckD.dll", EntryPoint = "vision_test_interface", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vision_test_interface(int nTest);

        [DllImport("VisionCheckD.dll", EntryPoint = "vision_init", CallingConvention = CallingConvention.Cdecl)]
        private static extern int vision_init(ref int nHandle, int width, int height, int channel, int nIndependentImg);

        [DllImport("VisionCheckD.dll", EntryPoint = "vision_free", CallingConvention = CallingConvention.Cdecl)]
        private static extern void vision_free(ref int nHandle);

        [DllImport("VisionCheckD.dll", EntryPoint = "vision_model_operate", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool vision_model_operate(int nHandle, e_ModelFLAG eModelFlag, IntPtr chTemplate, int nLengthTemplate, int nIndependentImgNo, DzBitmap modelImage);

        [DllImport("VisionCheckD.dll", EntryPoint = "vision_read_model", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool vision_read_model(int nHandle, IntPtr chTemplate, int nLengthTemplate, out string folderName);

        [DllImport("VisionCheckD.dll", EntryPoint = "vision_check", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool vision_check(int nHandle, DzBitmap checkImg, ref IntPtr result, ref int errorCount);
        #endregion


        public int Handle { get; set; } = 0;

        public VisionChecker(int width, int height, int nHandle = 0, int channel = 3, int nIndependentImg = 0)
        {
            Handle = nHandle;
            Init(width, height, nHandle, channel, nIndependentImg);
        }
        public void Dispose()
        {
            Free(Handle);
        }


        public void Init(int width, int height, int nHandle = 0, int channel = 3, int nIndependentImg = 0)
        {
            vision_init(ref nHandle, width, height, channel, nIndependentImg);
        }

        public void Free(int nHandle = 0)
        {
            vision_free(ref nHandle);
        }


        public bool ModelOperate(e_ModelFLAG eModelFlag, string Template, Bitmap modelImage, int nIndependentImgNo = 0)
        {
            DzBitmap dzBitmap = new DzBitmap()
            {
                stride = 0,
                type = 3,
                width = modelImage.Width,
                height = modelImage.Height,
                imgData = StructUtil.GetPtrByBitmap(modelImage),
            };

            var templateLength = 0;

            IntPtr intPtr = mallocIntptr(Template, out templateLength);
            return vision_model_operate(Handle, eModelFlag, intPtr, templateLength, nIndependentImgNo, dzBitmap);
        }


        public bool ReadModel(string Template, out string folderName)
        {
            var templateLength = 0;
            IntPtr intPtr = mallocIntptr(Template, out templateLength);
            return vision_read_model(Handle, intPtr, templateLength, out folderName);
        }


        public List<INSPECT_RESULT_INFO> Check(DzBitmap dzBitmap)
        {
            IntPtr intPtr = new IntPtr();
            int length = 0;
            //调用函数
            vision_check(Handle, dzBitmap, ref intPtr, ref length);

            //判断结果
            if (length == 0) return new List<INSPECT_RESULT_INFO>();


            //读取结果
            byte[] aryResult = new byte[length];
            Marshal.Copy(intPtr, aryResult, 0, length);

            //反序列化结果
            string strResult = Encoding.Default.GetString(aryResult);
            List<INSPECT_RESULT_INFO> result = JsonConvert.DeserializeObject<List<INSPECT_RESULT_INFO>>(strResult);
            return result;
        }

        #region " IntPtr mallocIntptr( string strData ) 根据数据的长度申请非托管空间"
        /// <summary>
        /// 根据数据的长度申请非托管空间
        /// </summary>
        /// <param name="strData">要申请非托管空间的数据</param>
        /// <returns>指向非拖管空间的指针</returns>
        private static IntPtr mallocIntptr(string strData, out int length)
        {
            //先将字符串转化成字节方式
            Byte[] btData = System.Text.Encoding.Default.GetBytes(strData);
            length = btData.Length;
            //申请非拖管空间
            IntPtr m_ptr = Marshal.AllocHGlobal(btData.Length);

            //给非拖管空间清０ 
            Byte[] btZero = new Byte[btData.Length + 1]; //一定要加1,否则后面是乱码，原因未找到
            Marshal.Copy(btZero, 0, m_ptr, btZero.Length);

            //给指针指向的空间赋值
            Marshal.Copy(btData, 0, m_ptr, btData.Length);

            return m_ptr;
        }

        /// <summary>
        /// 根据长度申请非托管空间
        /// </summary>
        /// <param name="strData">要申请非托管空间的大小</param>
        /// <returns>指向非拖管空间的指针</returns>
        private static IntPtr mallocIntptr(int length)
        {
            //申请非拖管空间
            IntPtr m_ptr = Marshal.AllocHGlobal(length);

            //给非拖管空间清０ 
            Byte[] btZero = new Byte[length + 1]; //一定要加1,否则后面是乱码，原因未找到
            Marshal.Copy(btZero, 0, m_ptr, btZero.Length);

            //给指针指向的空间赋值
            Marshal.Copy(btZero, 0, m_ptr, length);

            return m_ptr;
        }


        #endregion
    }


    public enum e_ModelFLAG
    {
        FLAG_INIT = 0,      // 初始化状态
        FLAG_DEL,           // 删除标志
        FLAG_ADD,           // 新增标志
        FLAG_MODIFY,            // 修改标志
        FLAG_BACKUP,        // 备份标志
        FLAG_MULTI_MODEL_ADD,   // 增加多模板
        FLAG_MULTI_MODEL_DEL,   // 删除多模板
        FLAG_STUDY,         //学习
        FLAG_STUDY_ONLINE,  //检测中学习
        FLAG_RECREATE,      //重新创建(包含：删除多模板、修改其它区域)
        FLAG_REPLACE,       // 替换
        FLAG_UNDO_STUDY,    // 撤销学习
    };
}
