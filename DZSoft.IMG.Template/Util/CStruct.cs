using DZSoft.IMG.Template.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DZSoft.IMG.Template.Util
{
    public class StructUtil
    {
        #region 内部方法
        /// <summary>
        /// 释放bitmap指针
        /// </summary>
        /// <param name="ptr">图像指针资源</param>
        public static void dzFreePtr(IntPtr ptr)
        {
            DzBitmap mybmp = (DzBitmap)Marshal.PtrToStructure(ptr, typeof(DzBitmap));
            Marshal.FreeHGlobal(mybmp.imgData);
            Marshal.FreeHGlobal(ptr);
        }

        /// <summary>
        /// 从Bitmap变换为IntPtr
        /// 注意：该函数使用后需显示调用FreeMyBitmapPtr
        /// </summary>
        /// <param name="src">BITMAP原图</param>
        /// <returns>图像指针</returns>
        public static IntPtr GetPtrByBitmap(Bitmap src)
        {
            if (src == null)
            {
                return IntPtr.Zero;
            }
            Bitmap original = null;
            if (src.PixelFormat != PixelFormat.Format24bppRgb
                && src.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                original = new Bitmap(src.Width, src.Height, PixelFormat.Format24bppRgb);
                using (Graphics g = Graphics.FromImage(original))
                {
                    g.PageUnit = GraphicsUnit.Pixel;
                    g.DrawImageUnscaled(src, 0, 0);
                }
            }
            else
            {
                original = src;
            }
            // 将源图像内存区域锁定  
            Rectangle rect = new Rectangle(0, 0, original.Width, original.Height);
            BitmapData bmpData = original.LockBits(rect, ImageLockMode.ReadOnly, original.PixelFormat);
            DzBitmap bmp = new DzBitmap();
            bmp.width = bmpData.Width;
            bmp.height = bmpData.Height;

            switch (original.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    bmp.type = 3;
                    break;
                case PixelFormat.Format32bppArgb:
                    bmp.type = 3;
                    break;
                default:
                    bmp.type = 1;
                    break;
            }

            bmp.stride = bmpData.Stride;
            int length = bmpData.Stride * bmp.height;
            byte[] data = new byte[length];
            bmp.imgData = Marshal.AllocHGlobal(length);
            Marshal.Copy(bmpData.Scan0, data, 0, length);
            Marshal.Copy(data, 0, bmp.imgData, length);
            int sz = Marshal.SizeOf(bmp);
            IntPtr ptr = Marshal.AllocHGlobal(sz);
            Marshal.StructureToPtr(bmp, ptr, false);
            original.UnlockBits(bmpData);  // 解锁内存区域  
            if (src.PixelFormat == PixelFormat.Format32bppArgb)
            {
                original.Dispose();
            }
            return ptr;
        }

        /// <summary>
        /// 从MyBitmap转换为Bitmap，函数内会将MyBitmap的imgData释放
        /// </summary>
        /// <param name="mybmp">MyBitmap</param>
        /// <returns>Bitmap</returns>
        public static Bitmap GetBitmapByMyBitmap(DzBitmap mybmp)
        {
            PixelFormat format = PixelFormat.Format8bppIndexed;
            if (mybmp.type == 1)
            {
                int count = mybmp.width * mybmp.height;
                byte[] data = new byte[count];
                Marshal.Copy(mybmp.imgData, data, 0, count);
                return BuiltGrayBitmap(data, mybmp.width, mybmp.height);
                //format = PixelFormat.Format8bppIndexed;
            }
            if (mybmp.type == 3)
            {
                format = PixelFormat.Format24bppRgb;
            }
            else if (mybmp.type == 4)
            {
                format = PixelFormat.Format32bppRgb;
            }
            //return new Bitmap(mybmp.width, mybmp.height, mybmp.stride, format, mybmp.imgData);
            Bitmap bmp = new Bitmap(mybmp.width, mybmp.height, format);
            Rectangle rect = new Rectangle(0, 0, mybmp.width, mybmp.height);
            unsafe
            {
                BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly,
                  format);

                if (mybmp.stride >= bmpData.Stride)
                {
                    int length = bmpData.Stride * bmp.Height;
                    byte[] temp = new byte[length];
                    Marshal.Copy(mybmp.imgData, temp, 0, length);
                    Marshal.Copy(temp, 0, bmpData.Scan0, length);
                }
                else if (mybmp.stride < bmpData.Stride)
                {
                    for (int i = 0; i < bmp.Height; i++)
                    {
                        byte[] temp = new byte[mybmp.stride];
                        Marshal.Copy(mybmp.imgData + mybmp.stride * i, temp, 0, mybmp.stride);
                        Marshal.Copy(temp, 0, bmpData.Scan0 + i * bmpData.Stride, mybmp.stride);
                    }
                }
                bmp.UnlockBits(bmpData);
            }
            dzFreePtr(mybmp.imgData);
            return bmp;
        }

        /// <summary>  
        /// 用灰度数组新建一个8位灰度图像。  
        /// </summary>  
        /// <param name="rawValues"> 灰度数组(length = width * height)。 </param>  
        /// <param name="width"> 图像宽度。 </param>  
        /// <param name="height"> 图像高度。 </param>  
        /// <returns> 新建的8位灰度位图。 </returns>  
        private static Bitmap BuiltGrayBitmap(byte[] rawValues, int width, int height)
        {
            // 新建一个8位灰度位图，并锁定内存区域操作  
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height),
                 ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            // 计算图像参数  
            int offset = bmpData.Stride - bmpData.Width;        // 计算每行未用空间字节数  
            IntPtr ptr = bmpData.Scan0;                         // 获取首地址  
            int scanBytes = bmpData.Stride * bmpData.Height;    // 图像字节数 = 扫描字节数 * 高度  
            byte[] grayValues = new byte[scanBytes];            // 为图像数据分配内存  

            // 为图像数据赋值  
            int posSrc = 0, posScan = 0;                        // rawValues和grayValues的索引  
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    grayValues[posScan++] = rawValues[posSrc++];
                }
                // 跳过图像数据每行未用空间的字节，length = stride - width * bytePerPixel  
                posScan += offset;
            }
            // 内存解锁  
            Marshal.Copy(grayValues, 0, ptr, scanBytes);
            bitmap.UnlockBits(bmpData);  // 解锁内存区域  
            // 修改生成位图的索引表，从伪彩修改为灰度  
            ColorPalette palette;
            // 获取一个Format8bppIndexed格式图像的Palette对象  
            using (Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
            {
                palette = bmp.Palette;
            }
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }
            // 修改生成位图的索引表  
            bitmap.Palette = palette;
            return bitmap;
        }



        #endregion
    }
}
