using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：BitmapHelper
    /// 功      能：提供对图像的基本处理方法，并可以返回Bitmap格式处理结果
    ///             该模块的处理方法用GDI实现，系统开销较高，不建议频繁调用
    /// 日      期：2015-03-20
    /// 修      改：2015-11-17
    /// 作      者：ls9512
    /// </summary>
    public static class BitmapHelper
    {
        #region 错误位图
        /// <summary>
        /// 错误位图
        /// </summary>
        public static Bitmap ErrorBitmap
        {
            get
            {
                if (_errorBitmap == null)
                {
                    _errorBitmap = new Bitmap(100, 100);
                    Graphics g = Graphics.FromImage(_errorBitmap);
                    g.FillRectangle(Brushes.Yellow, new Rectangle(0, 0, 100, 100));
                    g.DrawString("Error", new Font("The New Roman", 16f), Brushes.Red, 25, 33);
                    g.Dispose();
                }
                return _errorBitmap;
            }
        }
        private static Bitmap _errorBitmap; 
        #endregion

        #region 切割
        /// <summary>
        /// 切割图像的指定区域
        /// </summary>
        /// <param name="bitmap">源图像</param>
        /// <param name="startX">起点x</param>
        /// <param name="startY">起点y</param>
        /// <param name="iWidth">宽度</param>
        /// <param name="iHeight">高度</param>
        /// <returns>切割结果</returns>
        public static Bitmap BitmapCut(Bitmap bitmap, int startX, int startY, int iWidth, int iHeight)
        {
            if (bitmap == null)
            {
                return null;
            }
            int w = bitmap.Width;
            int h = bitmap.Height;
            if (startX >= w || startY >= h)
            {
                return null;
            }
            if (startX + iWidth > w)
            {
                iWidth = w - startX;
            }
            if (startY + iHeight > h)
            {
                iHeight = h - startY;
            }
            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(bitmap, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(startX, startY, iWidth, iHeight), GraphicsUnit.Pixel);
                //元件边框
                //g.DrawRectangle(Pens.Black, new Rectangle(new Point(0, 0), new Size(iWidth - 3, iHeight - 3)));
                g.Dispose();
                return bmpOut;
            }
            catch
            {
                return null;
            }
        } 
        #endregion

        #region 透明度
        /// <summary>
        /// 图像透明度
        /// </summary>
        /// <param name="srcImage">源图像</param>
        /// <param name="opacity">透明度</param>
        /// <returns>返回图像</returns>
        public static Bitmap BitmapCtrlOpacity(Bitmap srcImage, float opacity)
        {
            float[][] nArray ={ new float[] {1, 0, 0, 0, 0}, 
                new float[] {0, 1, 0, 0, 0}, 
                new float[] {0, 0, 1, 0, 0}, 
                new float[] {0, 0, 0, opacity, 0}, 
                new float[] {0, 0, 0, 0, 1}
                              };
            ColorMatrix matrix = new ColorMatrix(nArray);
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            Bitmap resultImage = new Bitmap(srcImage.Width, srcImage.Height);
            Graphics g = Graphics.FromImage(resultImage);
            g.DrawImage(srcImage, new Rectangle(0, 0, srcImage.Width, srcImage.Height), 0, 0, srcImage.Width, srcImage.Height, GraphicsUnit.Pixel, attributes);
            g.Dispose();
            return resultImage;
        } 
        #endregion

        #region 旋转
        /// <summary>          
        /// 以逆时针为方向对图像进行旋转          
        /// </summary>          
        /// <param name="b">位图流</param>          
        /// <param name="angle">旋转角度[0,360]</param>  
        /// /// <returns>返回图像</returns>          
        public static Bitmap RotateImg(Bitmap b, int angle)
        {
            angle = angle % 360;//弧度转换  
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);//原图的宽和高  
            int w = b.Width;
            int h = b.Height;
            int width = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int height = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));//目标位图  
            Bitmap dsImage = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(dsImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//计算偏移量  
            Point Offset = new Point((width - w) / 2, (height - h) / 2);//构造图像显示区域：让图像的中心与窗口的中心点一致  
            Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(360 - angle);//恢复图像在水平和垂直方向的平移  
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(b, rect);//重至绘图的所有变换  
            g.ResetTransform(); g.Save();
            g.Dispose();
            //保存旋转后的图片  
            b.Dispose();
            return dsImage;
        } 
        #endregion

        #region 灰度/黑白
        /// <summary>  
        /// 图像灰度化  
        /// </summary>  
        /// <param name="orBitmap">原始图像</param>  
        /// <param name="Step">灰阶</param>  
        /// <returns>返回图像</returns>  
        public static Bitmap BitmapToGrapBitMap(Bitmap orBitmap, int Step)
        {
            //返回图像  
            Bitmap reBitmap = new Bitmap(orBitmap.Width, orBitmap.Height);
            //准备每一阶的灰度  
            int[] stepx = new int[Step];
            for (int i = 0; i < Step; i++)
            {
                stepx[i] = (int)(255.0 / (Step - 1) * i);
            }
            for (int i = 0; i < orBitmap.Height; i++)
            {
                for (int j = 0; j < orBitmap.Width; j++)
                {
                    //取像素点颜色  
                    Color co = orBitmap.GetPixel(j, i);
                    //计算明度  
                    int cx = (int)((Convert.ToInt32(co.R) + Convert.ToInt32(co.G) + Convert.ToInt32(co.B)) * 1.0 / 3);
                    //对应到阶  
                    cx = (int)(cx * Step * 1.0 / 255);
                    if (cx < 0) cx = 0;
                    if (cx > Step - 1) cx = Step - 1;
                    cx = stepx[cx];
                    co = Color.FromArgb(cx, cx, cx);
                    //设置像素点颜色  
                    reBitmap.SetPixel(j, i, co);
                }
            }
            return reBitmap;
        }

        /// <summary>  
        /// 图像黑白化1(最值法)  
        /// </summary>  
        /// <param name="orBitmap">原始图像</param>  
        /// <returns>返回图像</returns>  
        public static Bitmap BitmapToBW1(Bitmap orBitmap)
        {
            //返回图像  
            Bitmap reBitmap = new Bitmap(orBitmap.Width, orBitmap.Height);
            for (int i = 0; i < orBitmap.Height; i++)
            {
                for (int j = 0; j < orBitmap.Width; j++)
                {
                    int x = 0;
                    //最大值法置黑白图像  
                    Color c = orBitmap.GetPixel(j, i);
                    if (c.R > c.B) x = c.R; else x = c.B;
                    if (c.G > x) x = c.G;
                    reBitmap.SetPixel(j, i, Color.FromArgb(x, x, x));
                }
            }
            return reBitmap;
        }

        /// <summary>  
        /// 图像黑白化2(加权平均值法)  
        /// </summary>  
        /// <param name="orBitmap">原始图像</param>  
        /// <returns>返回图像</returns>  
        public static Bitmap BitmapToBW2(Bitmap orBitmap)
        {
            //返回图像  
            Bitmap reBitmap = new Bitmap(orBitmap.Width, orBitmap.Height);
            for (int i = 0; i < orBitmap.Height; i++)
            {
                for (int j = 0; j < orBitmap.Width; j++)
                {
                    int x = 0;
                    //加权平均求黑白色  
                    Color c = orBitmap.GetPixel(j, i);
                    x = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                    reBitmap.SetPixel(j, i, Color.FromArgb(x, x, x));
                }
            }
            return reBitmap;
        }

        /// <summary>  
        /// 图像黑白2值化  
        /// </summary>  
        /// <param name="orBitmap">原始图像</param>  
        /// <returns>返回图像</returns>  
        public static Bitmap BitmapToBW2Value(Bitmap orBitmap)
        {
            //返回图像  
            Bitmap reBitmap = new Bitmap(orBitmap.Width, orBitmap.Height);
            //计算平均颜色  
            int average = 0;
            for (int i = 0; i < orBitmap.Width; i++)
            {
                for (int j = 0; j < orBitmap.Height; j++)
                {
                    Color color = orBitmap.GetPixel(i, j);
                    average += color.B;
                }
            }
            average = (int)average / (orBitmap.Width * orBitmap.Height);
            for (int i = 0; i < orBitmap.Height; i++)
            {
                for (int j = 0; j < orBitmap.Width; j++)
                {
                    //设置像素点颜色  
                    if (orBitmap.GetPixel(j, i).B < average)
                    {
                        reBitmap.SetPixel(j, i, Color.Black);
                    }
                    else
                    {
                        reBitmap.SetPixel(j, i, Color.White);
                    }
                }
            }
            return reBitmap;
        }
        #endregion  

        #region 色彩
        /// <summary>  
        /// 图像反色/底片效果
        /// </summary>  
        /// <param name="orBitmap">原始图像</param>  
        /// <returns>返回图像</returns>  
        public static Bitmap BitmapToAntiColor(Bitmap orBitmap)
        {
            //返回图像  
            Bitmap reBitmap = new Bitmap(orBitmap.Width, orBitmap.Height);
            for (int i = 0; i < orBitmap.Height; i++)
            {
                for (int j = 0; j < orBitmap.Width; j++)
                {
                    //反色  
                    Color c = orBitmap.GetPixel(j, i);
                    reBitmap.SetPixel(j, i, Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B));
                }
            }
            return reBitmap;
        }
        #endregion

        #region 高斯模糊 高效
        /// <summary>
        /// 高斯模糊(unsafe)
        /// </summary>
        /// <param name="bmpSrc">原图像</param>
        /// <returns>模糊结果</returns>
        public static Bitmap BitmapToGaussianBlur(Bitmap bmpSrc)
        {
            float[,] kernel =
            {
                //	{0.33f, 0, 0},
                //	{0, 0.33f, 0},
                //	{0, 0, 0.33f}
                {0.045f, 0.122f, 0.045f},
                {0.122f, 0.332f, 0.122f},
                {0.045f, 0.122f, 0.045f}
                //{0.111f, 0.111f, 0.111f},
                //{0.111f, 0.111f, 0.111f},
                //{0.111f, 0.111f, 0.111f}
                //{1}
            };
            return Convolve(bmpSrc, kernel);
        }

        /// <summary>
        /// 高斯模糊 辅助方法
        /// </summary>
        /// <param name="bmpSrc"></param>
        /// <param name="kernel"></param>
        /// <returns>返回图像</returns>
        private static Bitmap Convolve(Bitmap bmpSrc, float[,] kernel)
        {
            Bitmap bmpDst = new Bitmap(bmpSrc.Width, bmpSrc.Height, bmpSrc.PixelFormat);
            BitmapData bmpDstData = bmpDst.LockBits(
                new Rectangle(0, 0, bmpDst.Width, bmpDst.Height),
                ImageLockMode.ReadWrite,
                bmpDst.PixelFormat);

            BitmapData bmpSrcData = bmpSrc.LockBits(
                new Rectangle(0, 0, bmpSrc.Width, bmpSrc.Height),
                ImageLockMode.ReadWrite,
                bmpSrc.PixelFormat);

            EPoint pntKernelSize = new EPoint(kernel.GetLength(0), kernel.GetLength(1));
            int nBpp = 3;

            unsafe
            {
                for (int y = pntKernelSize.Y / 2; y < bmpSrc.Height - pntKernelSize.Y / 2; y++)
                {
                    byte* ptrSrc = (byte*)bmpSrcData.Scan0;
                    byte* ptrDst = (byte*)bmpDstData.Scan0;
                    ptrDst += bmpDstData.Stride * y;

                    for (int x = pntKernelSize.X / 2; x < bmpSrc.Width - pntKernelSize.X / 2; x++)
                    {
                        for (int bit = 0; bit < nBpp; bit++)
                        {
                            float fVal = 0;
                            for (int yKernel = 0; yKernel < pntKernelSize.Y; yKernel++)
                            {
                                for (int xKernel = 0; xKernel < pntKernelSize.X; xKernel++)
                                {
                                    fVal += kernel[xKernel, yKernel] *
                                        (float)*(ptrSrc +
                                        bit + (x + xKernel - pntKernelSize.X / 2) * nBpp +
                                        (y + yKernel - pntKernelSize.Y / 2) * bmpSrcData.Stride);
                                }
                            }
                            *ptrDst = (byte)fVal;
                            ptrDst++;
                        }
                    }
                }
            }

            bmpSrc.UnlockBits(bmpSrcData);
            bmpDst.UnlockBits(bmpDstData);

            return bmpDst;
        }

        /// <summary>
        /// 辅助点结构
        /// </summary>
        private class EPoint
        {
            private int x = 0;
            private int y = 0;

            public EPoint(float X, float Y)
            {
                x = (int)X;
                y = (int)Y;
            }
            public EPoint(int X, int Y)
            {
                x = X;
                y = Y;
            }
            public EPoint(PointF pnt)
            {
                x = (int)pnt.X;
                y = (int)pnt.Y;
            }
            public EPoint(Point pnt)
            {
                x = pnt.X;
                y = pnt.Y;
            }
            public EPoint Copy()
            {
                return new EPoint(x, y);
            }

            public PointF ToPointF()
            {
                return new PointF(x, y);
            }
            public Point ToPoint()
            {
                return new Point(x, y);
            }
            public SizeF ToSizeF()
            {
                return new SizeF(x, y);
            }
            public Size ToSize()
            {
                return new Size(x, y);
            }


            public static EPoint operator -(EPoint p1, EPoint p2)
            {
                return new EPoint(p1.X - p2.X, p1.Y - p2.Y);
            }
            public static EPoint operator +(EPoint p1, EPoint p2)
            {
                return new EPoint(p1.X + p2.X, p1.Y + p2.Y);
            }
            public static EPoint operator *(EPoint p1, EPoint p2)
            {
                return new EPoint(p1.X * p2.X, p1.Y * p2.Y);
            }
            public static EPoint operator *(EPoint p1, int i)
            {
                return new EPoint(p1.X * i, p1.Y * i);
            }
            public static EPoint operator /(EPoint p1, EPoint p2)
            {
                return new EPoint(p1.X / p2.X, p1.Y / p2.Y);
            }
            public static EPoint operator /(EPoint p1, int i)
            {
                return new EPoint(p1.X / i, p1.Y / i);
            }

            public int X
            {
                get { return x; }
                set { x = value; }
            }
            public int Y
            {
                get { return y; }
                set { y = value; }
            }


            public override bool Equals(object obj)
            {
                EPoint pnt = (EPoint)obj;
                return (pnt.X == x && pnt.Y == y);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                return "x=" + x.ToString() + ";y=" + y.ToString();
            }
        }
        #endregion

        #region 模糊/锐化
        /// <summary>  
        /// 图像高斯模糊 GDI 
        /// </summary>  
        /// <param name="orBitmap">原始图像</param>  
        /// <returns>返回图像</returns>  
        public static Bitmap BitmapToGaussianBlurByGdi(Bitmap orBitmap)
        {
            //返回图像  
            Bitmap reBitmap = new Bitmap(orBitmap.Width, orBitmap.Height);
            //高斯模板   
            int[] gauss = { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
            for (int i = 1; i < orBitmap.Height - 1; i++)
            {
                for (int j = 1; j < orBitmap.Width - 1; j++)
                {
                    int r = 0, g = 0, b = 0;
                    int index = 0;
                    for (int col = -1; col <= 1; col++)
                    {
                        for (int row = -1; row <= 1; row++)
                        {
                            Color pixel = orBitmap.GetPixel(j + row, i + col);
                            r += pixel.R * gauss[index];
                            g += pixel.G * gauss[index];
                            b += pixel.B * gauss[index];
                            index++;
                        }
                    }
                    r /= 16;
                    g /= 16;
                    b /= 16;
                    //处理颜色值溢出  
                    ColorRgbOverflow(ref r, ref g, ref b);
                    reBitmap.SetPixel(j, i, Color.FromArgb(r, g, b));
                }
            }
            return reBitmap;
        }

        /// <summary>  
        /// 模糊效果  
        /// </summary>  
        /// <param name="orBitmap">原始图像</param>  
        /// <returns>返回图像</returns>  
        public static Bitmap BitmapToBlur(Bitmap orBitmap)
        {
            //返回图像  
            Bitmap reBitmap = new Bitmap(orBitmap.Width, orBitmap.Height);
            int Strength = 1;
            for (int i = 0; i < orBitmap.Height; i++)
            {
                for (int j = 0; j < orBitmap.Width; j++)
                {
                    int r = 0, g = 0, b = 0;
                    int x = i, y = j;
                    if (x == 0) x = 1;
                    if (y == 0) y = 1;
                    if (x == orBitmap.Height - 1) x = orBitmap.Height - 2;
                    if (y == orBitmap.Width - 1) y = orBitmap.Width - 2;
                    Color pixel1 = orBitmap.GetPixel(y - Strength, x - Strength);
                    Color pixel2 = orBitmap.GetPixel(y + Strength, x + Strength);
                    Color pixel3 = orBitmap.GetPixel(y - Strength, x + Strength);
                    Color pixel4 = orBitmap.GetPixel(y + Strength, x - Strength);
                    pixel2 = Color.FromArgb((pixel2.R + pixel1.R + pixel3.R + pixel4.R) / 4, (pixel2.G + pixel1.G + pixel3.G + pixel4.G) / 4, (pixel2.B + pixel1.B + pixel3.B + pixel4.B) / 4);
                    r = pixel2.R;
                    g = pixel2.G;
                    b = pixel2.B;
                    //防止溢出  
                    ColorRgbOverflow(ref r, ref g, ref b);
                    reBitmap.SetPixel(j, i, Color.FromArgb(r, g, b));
                }
            }
            return reBitmap;
        }

        /// <summary>  
        /// 图像锐化  
        /// </summary>  
        /// <param name="orBitmap">原始图像</param>  
        /// <returns>返回图像</returns>  
        public static Bitmap BitmapToSharpen(Bitmap orBitmap)
        {
            //返回图像  
            Bitmap reBitmap = new Bitmap(orBitmap.Width, orBitmap.Height);
            //拉普拉斯模板  
            int[] Laplacian = { -1, -1, -1, -1, 9, -1, -1, -1, -1 };
            Color pixel;
            for (int i = 1; i < orBitmap.Height - 1; i++)
            {
                for (int j = 1; j < orBitmap.Width - 1; j++)
                {
                    int r = 0, g = 0, b = 0;
                    int Index = 0;
                    for (int col = -1; col <= 1; col++)
                    {
                        for (int row = -1; row <= 1; row++)
                        {
                            pixel = orBitmap.GetPixel(j + row, i + col);
                            r += pixel.R * Laplacian[Index];
                            g += pixel.G * Laplacian[Index];
                            b += pixel.B * Laplacian[Index];
                            Index++;
                        }
                    }
                    //处理颜色值溢出   
                    ColorRgbOverflow(ref r, ref g, ref b);
                    reBitmap.SetPixel(j - 1, i - 1, Color.FromArgb(r, g, b));
                }
            }
            return reBitmap;
        }

        /// <summary>  
        /// 图像雾化  
        /// </summary>  
        /// <param name="orBitmap">原始图像</param>  
        /// <param name="Strength">雾化强度(0-10)</param>  
        /// <returns>返回图像</returns>  
        public static Bitmap BitmapToAtomization(Bitmap orBitmap, int Strength)
        {
            //返回图像  
            Bitmap reBitmap = new Bitmap(orBitmap.Width, orBitmap.Height);
            Color pixel;
            Random randx = new Random();
            for (int i = 1; i < orBitmap.Height - 1; i++)
            {
                for (int j = 1; j < orBitmap.Width - 1; j++)
                {
                    //随机雾化强度(可自己调整)  
                    int k = randx.Next(Strength + 1, (Strength + 1) * 10);
                    //像素块大小  
                    int dx = j + k % 19;
                    int dy = i + k % 19;
                    if (dx >= orBitmap.Width) dx = orBitmap.Width - 1;
                    if (dy >= orBitmap.Height) dy = orBitmap.Height - 1;
                    pixel = orBitmap.GetPixel(dx, dy);
                    int r = pixel.R, g = pixel.G, b = pixel.B;
                    Color c1, c2, c3, c4;
                    c1 = orBitmap.GetPixel(j - 1, i - 1);
                    c2 = orBitmap.GetPixel(j - 1, i + 1);
                    c3 = orBitmap.GetPixel(j + 1, i - 1);
                    c4 = orBitmap.GetPixel(j + 1, i + 1);
                    r = (r + c1.R + c2.R + c3.R + c4.R) / 5;
                    g = (g + c1.G + c2.G + c3.G + c4.G) / 5;
                    b = (b + c1.B + c2.B + c3.B + c4.B) / 5;
                    reBitmap.SetPixel(j, i, Color.FromArgb(r, g, b));
                }
            }
            return reBitmap;
        }
        #endregion

        #region 艺术/绘画
        /// <summary>  
        /// 图像浮雕效果  
        /// </summary>  
        /// <param name="orBitmap">原始图像</param>  
        /// <returns>返回图像</returns>  
        public static Bitmap BitmapToRelief(Bitmap orBitmap)
        {
            //返回图像  
            Bitmap reBitmap = new Bitmap(orBitmap.Width, orBitmap.Height);
            for (int i = 0; i < orBitmap.Height - 1; i++)
            {
                for (int j = 0; j < orBitmap.Width - 1; j++)
                {
                    Color pixel1 = orBitmap.GetPixel(j, i);
                    Color pixel2 = orBitmap.GetPixel(j + 1, i + 1);
                    int r = Math.Abs(pixel1.R - pixel2.R + 128);
                    int g = Math.Abs(pixel1.G - pixel2.G + 128);
                    int b = Math.Abs(pixel1.B - pixel2.B + 128);
                    ColorRgbOverflow(ref r, ref g, ref b);
                    reBitmap.SetPixel(j, i, Color.FromArgb(r, g, b));
                }
            }
            return reBitmap;
        }

        /// <summary>  
        /// 油画效果  
        /// </summary>  
        /// <param name="orBitmap">原始图像</param>  
        /// <param name="Strength">强度(0-10)</param>  
        /// <returns>返回图像</returns>  
        public static Bitmap BitmapToPainting(Bitmap orBitmap, int Strength)
        {
            //返回图像  
            Bitmap reBitmap = new Bitmap(orBitmap.Width, orBitmap.Height);
            //产生随机数序列  
            Random randx = new Random();
            //强度  
            Strength++;
            Strength *= 2;
            int i = 0;
            while (i < orBitmap.Width)
            {
                int j = 0;
                while (j < orBitmap.Height)
                {
                    int iPosx = randx.Next(100000) % Strength;
                    int iPosy = randx.Next(100000) % Strength;
                    if (i + iPosx >= orBitmap.Width) iPosx = -1 * randx.Next(0, orBitmap.Width - i);
                    if (j + iPosy >= orBitmap.Height) iPosy = -1 * randx.Next(0, orBitmap.Height - j);
                    //将该点的RGB值设置成附近iModel点之内的任一点  
                    Color color = orBitmap.GetPixel(i + iPosx, j + iPosy);
                    reBitmap.SetPixel(i, j, color);
                    j++;
                }
                i++;
            }
            return reBitmap;
        }

        /// <summary>  
        /// 积木效果  
        /// </summary>  
        /// <param name="orBitmap">原始图像</param>  
        /// <returns>返回图像</returns>  
        public static Bitmap BitmapToBlock(Bitmap orBitmap)
        {
            //返回图像  
            Bitmap reBitmap = new Bitmap(orBitmap.Width, orBitmap.Height);
            for (int i = 1; i < orBitmap.Height - 1; i++)
            {
                for (int j = 1; j < orBitmap.Width - 1; j++)
                {
                    Color color = orBitmap.GetPixel(j, i);
                    int r, g, b;
                    if (color.R > 128) r = 255; else r = 0;
                    if (color.G > 128) g = 255; else g = 0;
                    if (color.B > 128) b = 255; else b = 0;
                    Color nColor = Color.FromArgb(255, r, g, b);
                    reBitmap.SetPixel(j, i, nColor);
                }
            }
            return reBitmap;
        }

        /// <summary>  
        /// 彩色线稿效果  
        /// </summary>  
        /// <param name="orBitmap">原始图像</param>  
        /// <returns>返回图像</returns>  
        public static Bitmap BitmapToColorLineArt(Bitmap orBitmap)
        {
            //返回图像  
            Bitmap reBitmap = new Bitmap(orBitmap.Width, orBitmap.Height);
            for (int i = 1; i < orBitmap.Height - 1; i++)
            {
                for (int j = 1; j < orBitmap.Width - 1; j++)
                {
                    int r = 0, g = 0, b = 0;
                    Color pixel1 = orBitmap.GetPixel(j, i);
                    Color pixel2 = orBitmap.GetPixel(j - 1, i - 1);
                    //pixel3 = orBitmap.GetPixel(j + 1, i + 1);  
                    //pixel2 = Color.FromArgb((pixel2.R + pixel3.R) / 2, (pixel2.G + pixel3.G) / 2, (pixel2.B + pixel3.B) / 2);  
                    r = 255 - Math.Abs(pixel1.R - pixel2.R);
                    g = 255 - Math.Abs(pixel1.G - pixel2.G);
                    b = 255 - Math.Abs(pixel1.B - pixel2.B);
                    //防止溢出  
                    ColorRgbOverflow(ref r, ref g, ref b);
                    reBitmap.SetPixel(j, i, Color.FromArgb(r, g, b));
                }
            }
            return reBitmap;
        }

        /// <summary>  
        /// 炭笔效果  
        /// </summary>  
        /// <param name="orBitmap">原始图像</param>  
        /// <returns>返回图像</returns>  
        public static Bitmap BitmapToCharcoal(Bitmap orBitmap)
        {
            //返回图像  
            Bitmap reBitmap = new Bitmap(orBitmap.Width, orBitmap.Height);
            //计算平均颜色  
            int average = 0;
            for (int i = 1; i < orBitmap.Height - 1; i++)
            {
                for (int j = 1; j < orBitmap.Width - 1; j++)
                {
                    int r = 0, g = 0, b = 0;
                    Color pixel1 = orBitmap.GetPixel(j, i);
                    Color pixel2 = orBitmap.GetPixel(j - 1, i - 1);
                    Color pixel3 = orBitmap.GetPixel(j + 1, i + 1);
                    //pixel2 = Color.FromArgb((pixel2.R + pixel3.R) / 2, (pixel2.G + pixel3.G) / 2, (pixel2.B + pixel3.B) / 2);  
                    r = 255 - Math.Abs(pixel1.R - pixel2.R);
                    g = 255 - Math.Abs(pixel1.G - pixel2.G);
                    b = 255 - Math.Abs(pixel1.B - pixel2.B);
                    ColorRgbOverflow(ref r, ref g, ref b);
                    Color c = Color.FromArgb(r, g, b);
                    reBitmap.SetPixel(j, i, c);
                    average += c.B;
                }
            }
            average /= orBitmap.Width * orBitmap.Height;
            for (int i = 0; i < orBitmap.Width; i++)
            {
                for (int j = 0; j < orBitmap.Height; j++)
                {
                    //设置像素点颜色  
                    if (reBitmap.GetPixel(i, j).B < average)
                    {
                        reBitmap.SetPixel(i, j, Color.Black);
                    }
                    else
                    {
                        reBitmap.SetPixel(i, j, Color.White);
                    }
                }
            }
            return reBitmap;
        }
        #endregion

        #region 光照
        /// <summary>  
        /// 图像中心光照  
        /// </summary>  
        /// <param name="orBitmap">原始图像</param>  
        /// <param name="strength">光照强度(0-10)</param>  
        /// <param name="centerPoint">光照中心</param>  
        /// <returns>返回图像</returns>  
        public static Bitmap BitmapToIllumination(Bitmap orBitmap, int strength, Point centerPoint)
        {
            //返回图像  
            Bitmap reBitmap = new Bitmap(orBitmap.Width, orBitmap.Height);
            int A = orBitmap.Width / 2; int B = orBitmap.Height / 2;
            //Center图片中心点，发亮此值会让强光中心发生偏移  
            Point center = centerPoint;
            //R强光照射面的半径，即”光晕”  
            int length = 0;
            if (orBitmap.Width > orBitmap.Height) length = orBitmap.Width; else length = orBitmap.Height;
            int R = Math.Min(length, length);
            //计算强度  
            R = (int)(strength * 1.0 / 10 * R);
            for (int i = 1; i < orBitmap.Height - 1; i++)
            {
                for (int j = 1; j < orBitmap.Width - 1; j++)
                {
                    float rLength = (float)Math.Sqrt(Math.Pow((j - center.X), 2) + Math.Pow((i - center.Y), 2));
                    //如果像素位于”光晕”之内  
                    if (rLength < R)
                    {
                        Color Color = orBitmap.GetPixel(j, i);
                        int r, g, b;
                        //220亮度增加常量，该值越大，光亮度越强  
                        float Pixel = 220.0f * (1.0f - rLength / R); r = Color.R + (int)Pixel;
                        r = Math.Max(0, Math.Min(r, 255));
                        g = Color.G + (int)Pixel; g = Math.Max(0, Math.Min(g, 255));
                        b = Color.B + (int)Pixel; b = Math.Max(0, Math.Min(b, 255));
                        //将增亮后的像素值回写到位图  
                        Color nColor = Color.FromArgb(255, r, g, b);
                        reBitmap.SetPixel(j, i, nColor);
                    }
                    else
                    {
                        reBitmap.SetPixel(j, i, orBitmap.GetPixel(j, i));
                    }
                }
            }
            return reBitmap;
        }

        /// <summary>  
        /// 补光  
        /// </summary>  
        /// <param name="orBitmap">原始图像</param>  
        /// <param name="strength">强度(0-10)</param>  
        /// <returns>返回图像</returns>  
        public static Bitmap BitmapToUpLight(Bitmap orBitmap, int strength)
        {
            //返回图像  
            Bitmap reBitmap = new Bitmap(orBitmap.Width, orBitmap.Height);
            strength++;
            strength *= 10;
            for (int i = 1; i < orBitmap.Height - 1; i++)
            {
                for (int j = 1; j < orBitmap.Width - 1; j++)
                {
                    Color Color = orBitmap.GetPixel(j, i);
                    int r, g, b;
                    r = Color.R + strength;
                    g = Color.G + strength;
                    b = Color.B + strength;
                    ColorRgbOverflow(ref r, ref g, ref b);
                    Color nColor = Color.FromArgb(255, r, g, b);
                    reBitmap.SetPixel(j, i, nColor);
                }
            }
            return reBitmap;
        }

        /// <summary>  
        /// 减光  
        /// </summary>  
        /// <param name="orBitmap">原始图像</param>  
        /// <param name="strength">强度(0-10)</param>  
        /// <returns>返回图像</returns>  
        public static Bitmap BitmapToDownLight(Bitmap orBitmap, int strength)
        {
            //返回图像  
            Bitmap reBitmap = new Bitmap(orBitmap.Width, orBitmap.Height);
            strength++;
            strength *= 10;
            for (int i = 1; i < orBitmap.Height - 1; i++)
            {
                for (int j = 1; j < orBitmap.Width - 1; j++)
                {
                    Color Color = orBitmap.GetPixel(j, i);
                    int r, g, b;
                    r = Color.R - strength;
                    g = Color.G - strength;
                    b = Color.B - strength;
                    ColorRgbOverflow(ref r, ref g, ref b);
                    Color nColor = Color.FromArgb(255, r, g, b);
                    reBitmap.SetPixel(j, i, nColor);
                }
            }
            return reBitmap;
        }
        #endregion  

        #region 防溢出
        /// <summary>
        /// RGB颜色防溢出
        /// </summary>
        /// <param name="r">R</param>
        /// <param name="g">G</param>
        /// <param name="b">B</param>
        private static void ColorRgbOverflow(ref int r, ref int g, ref int b)
        {
            r = r > 255 ? 255 : r;
            r = r < 0 ? 0 : r;
            g = g > 255 ? 255 : g;
            g = g < 0 ? 0 : g;
            b = b > 255 ? 255 : b;
            b = b < 0 ? 0 : b;
        } 
        #endregion
    }
}