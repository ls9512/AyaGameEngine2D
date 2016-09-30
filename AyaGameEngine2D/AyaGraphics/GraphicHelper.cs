using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

using CsGL.OpenGL;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：GraphicHelper
    /// 功      能：提供基于CSGL的纹理、颜色、文字和图形按指定格式绘制
    /// 说      明：基于OpenGL的引擎底层绘图接口
    /// 日      期：2015-03-20
    /// 修      改：2015-11-22
    /// 作      者：ls9512
    /// </summary>
    public static class GraphicHelper
    {
        #region 错误纹理
        /// <summary>
        /// 错误纹理
        /// </summary>
        public static uint[] ErrorTexture2D
        {
            get
            {
                if (_errorTexture2D == null)
                {
                    _errorTexture2D = CreateTexture2D(BitmapHelper.ErrorBitmap, false);
                }
                return _errorTexture2D;
            }
        }
        private static uint[] _errorTexture2D; 
        #endregion

        #region 创建纹理
        /// <summary>
        /// 从位图创建2D纹理
        /// </summary>
        /// <param name="bitmap">位图</param>
        /// <param name="isMipmaped">是否压缩</param>
        /// 
        /// 说明 ： 对于绘制文本等需要精确显示的元素，可以关闭压缩
        /// 
        /// <returns>纹理ID</returns>
        public static uint[] CreateTexture2D(Bitmap bitmap, bool isMipmaped)
        {
            try
            {
                // 转换成32位位图后再处理
                Bitmap bmpNew = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(bmpNew);
                g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                // 锁定内存
                BitmapData tex = bmpNew.LockBits(new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                // 像素处理
                //IntPtr ptr = tex.Scan0;
                //int bytesLength = tex.Stride * tex.Height;
                //byte[] rgbValues = new byte[bytesLength];
                //Marshal.Copy(ptr, rgbValues, 0, bytesLength);
                //for (int i = 0; i < rgbValues.Length; i += 4)
                //{
                //    //rgbValues[i] = 255;               // B
                //    //rgbValues[i + 1] = 255;           // G
                //    //rgbValues[i + 2] = 255;           // R
                //    //rgbValues[i + 3] = 255;           // A
                //    //if (rgbValues[i + 3] == 0) rgbValues[i + 3] = 220;
                //}
                //Marshal.Copy(rgbValues, 0, ptr, bytesLength);

                // 创建纹理
                uint[] texture = new uint[1];
                GL.glGenTextures(texture.Length, texture);
                OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, texture[0]);
                // 设置像素对齐
                //GL.glPixelStoref(GL.GL_PACK_ALIGNMENT, 4);
                // 设置环境融合方式和线性过滤参数
                // GL.glTexEnvi(GL.GL_TEXTURE_ENV, GL.GL_TEXTURE_ENV_MODE, (int)GL.GL_MODULATE);
                GL.glTexParameteri(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR);
                GL.glTexParameteri(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR);
                // GL_CLAMP 参数会导致部分机器纹理贴图出现黑边
                GL.glTexParameteri(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL_Extension.GL_CLAMP_TO_EDGE);
                GL.glTexParameteri(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL_Extension.GL_CLAMP_TO_EDGE);
                // 创建纹理
                bool IsBorder = false;
                // GL_RGBA 不压缩 GL_COMPRESSED_RGBA 压缩
                if (isMipmaped)
                {
                    GL.gluBuild2DMipmaps(OpenGL.GL_TEXTURE_2D, (int)OpenGL.GL_RGBA, bmpNew.Width, bmpNew.Height, OpenGL_Extension.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, tex.Scan0);
                }
                else
                {
                    GL.glTexImage2D(OpenGL.GL_TEXTURE_2D, 0, (int)OpenGL.GL_RGBA, bmpNew.Width, bmpNew.Height, IsBorder ? 1 : 0, OpenGL_Extension.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, tex.Scan0);
                }
                // 解锁并释放内存
                bmpNew.UnlockBits(tex);
                bmpNew.Dispose();
                g.Dispose();

                // 返回纹理ID
                return texture;
            }
            catch
            {
                return ErrorTexture2D;
            }
        }

        /// <summary>
        /// 从位图创建2D纹理
        /// </summary>
        /// <param name="bitmap">位图</param>
        /// <returns>纹理ID</returns>
        public static uint[] CreateTexture2D(Bitmap bitmap)
        {
            return CreateTexture2D(bitmap, true);
        }

        /// <summary>
        /// 从文件创建2D纹理
        /// </summary>
        /// <param name="fileName">文件名(路径)</param>
        /// <returns>纹理ID</returns>
        public static uint[] CreateTexture2D(string fileName)
        {
            // 用GDI将原始图像转换为32位png格式图像
            Bitmap bmpOrign = new Bitmap(fileName);
            uint[] textureID = CreateTexture2D(bmpOrign);
            bmpOrign.Dispose();
            return textureID;
        }
        #endregion

        #region 屏幕
        /// <summary>
        /// 以指定颜色清除屏幕
        /// </summary>
        /// <param name="color">颜色（不支持透明）</param>
        public static void ClearScreen(Color color)
        {
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            int x = 0, y = 0;
            int width = General.Draw_Rect.Width;
            int height = General.Draw_Rect.Height;
            // 坐标转换
            y = General.Draw_Rect.Height - y - height;
            OpenGL.glBegin(OpenGL.GL_QUADS);
            {
                // 设置颜色
                OpenGL.glColor3f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255);
                OpenGL.glVertex2f(x, y);
                OpenGL.glVertex2f(x + width, y);
                OpenGL.glVertex2f(x + width, y + height);
                OpenGL.glVertex2f(x, y + height);
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        } 
        #endregion

        #region 绘制 矩阵纹理数字
        /// <summary>
        /// 通过纹理绘制数字
        /// </summary>
        /// <param name="textureMatrix">数字矩阵纹理</param>
        /// <param name="x">起点X</param>
        /// <param name="y">起点Y</param>
        /// <param name="width">纹理宽度</param>
        /// <param name="height">纹理高度</param>
        /// <param name="value">数值（大于等于0）</param>
        /// <param name="blank">数字间隔</param>
        public static void DrawNumberByTextureMatrix(TextureMatrix textureMatrix, float x, float y, float width, float height, int value, float blank)
        {
            if (value < 0) value = 0;
            int index = 0;
            float xx = x + (value.ToString().Length - 1) * (width + blank);
            // 0单独处理
            if (value == 0)
            {
                DrawImage(textureMatrix.TextureID[0, 0], xx, y, width, height);
            }
            // 循环绘制各位
            while (value != 0)
            {
                int num = value % 10;
                float xTemp = xx - (width + blank) * index;
                DrawImage(textureMatrix.TextureID[num, 0], xTemp, y, width, height);
                index++;
                value /= 10;
            }
        }
        #endregion

        #region 绘制 GDI字符串
        /// <summary>
        /// 基于GDI创建字符串纹理
        /// ★ 消耗系统资源，不建议频繁调用
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="color">颜色</param>
        /// <param name="font">字体</param>
        /// <returns>字符串纹理</returns>
        public static Texture CreateStringTexture2DByGdi(string text, Color color, Font font)
        {
            // 设置字体
            if (font == null)
                font = new Font("宋体", 9F);
            // 计算字符串尺寸
            Bitmap bmp = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bmp);
            Size bmpSize = g.MeasureString(text, font).ToSize() + new Size(2, 2);
            g.Dispose();
            bmp.Dispose();
            // 创建字符串图像
            Bitmap bitmap = new Bitmap(bmpSize.Width, bmpSize.Height);
            g = Graphics.FromImage(bitmap);
            Brush brush = new SolidBrush(color);
            // 绘制字符串
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g.DrawString(text, font, brush, new Rectangle(1, 1, bmpSize.Width, bmpSize.Height));
            // 释放内存
            brush.Dispose();
            g.Dispose();
            // 创建纹理
            uint[] textureID = CreateTexture2D(bitmap, false);
            Texture texture = new Texture(textureID, bitmap.Width, bitmap.Height);
            bitmap.Dispose();
            return texture;
        }

        /// <summary>
        /// 基于GDI创建字符串纹理
        /// ★ 消耗系统资源，不建议频繁调用
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="color">颜色</param>
        /// <param name="font">字体</param>
        /// <param name="size">绘制矩形,尺寸有用户保证</param>
        /// <returns>字符串纹理</returns>
        public static Texture CreateStringTexture2DByGdi(string text, Color color, Font font, Size size)
        {
            // 设置字体
            if (font == null)
                font = new Font("宋体", 9F);
            // 创建字符串图像
            Bitmap bitmap = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(bitmap);
            Brush brush = new SolidBrush(color);
            // 绘制字符串
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g.DrawString(text, font, brush, new Rectangle(1, 1, size.Width, size.Height));
            // 释放内存
            brush.Dispose();
            g.Dispose();
            // 创建纹理
            uint[] textureID = CreateTexture2D(bitmap, false);
            Texture texture = new Texture(textureID, size.Width, size.Height);
            bitmap.Dispose();
            return texture;
        }

        /// <summary>
        /// 基于GDI的绘制字符串 - 立即释放
        /// ★ 消耗系统资源，不建议频繁调用
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="color">颜色</param>
        /// <param name="font">字体</param>
        /// <param name="x">起点x</param>
        /// <param name="y">起点y</param>
        public static void DrawStringGdi(string text, Color color, Font font, float x, float y)
        {
            // 计算字符串尺寸
            Bitmap bmp = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bmp);
            Size bmpSize = g.MeasureString(text, font).ToSize() + new Size(1, 0);
            g.Dispose();
            bmp.Dispose();
            // 创建纹理
            uint[] textureString = CreateStringTexture2DByGdi(text, color, font).TextureID;
            // 绘制纹理
            DrawImage(textureString, x, y, bmpSize.Width, bmpSize.Height);
            // 释放纹理
            GL.glDeleteTextures(textureString.Length, textureString);
        } 
        #endregion 

        #region 绘制 纹理
        /// <summary>
        /// 绘制纹理
        /// </summary>
        /// <param name="texture">纹理ID</param>
        /// <param name="x">起点x</param>
        /// <param name="y">起点y</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public static void DrawImage(uint[] texture, float x, float y, float width, float height)
        {
            
            // 绑定纹理
            OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, texture[0]);
            // 坐标转换
            OpenGL.glBegin(OpenGL.GL_QUADS);
            {
                // 纹理坐标映射
                OpenGL.glTexCoord2f(0.0f, 0.0f); OpenGL.glVertex2f(x, y);
                OpenGL.glTexCoord2f(0.0f, 1.0f); OpenGL.glVertex2f(x, y + height);
                OpenGL.glTexCoord2f(1.0f, 1.0f); OpenGL.glVertex2f(x + width, y + height);
                OpenGL.glTexCoord2f(1.0f, 0.0f); OpenGL.glVertex2f(x + width, y);
            }
            OpenGL.glEnd();
            // 性能计数
            PerformanceAnalyzer.Gaming_TextureCount++;
        }

        /// <summary>
        /// 绘制纹理
        /// </summary>
        /// <param name="texture">纹理ID</param>
        /// <param name="rect">矩形区域</param>
        public static void DrawImage(uint[] texture, RectangleF rect)
        {
            DrawImage(texture, rect.X, rect.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// 绘制纹理
        /// </summary>
        /// <param name="texture">纹理ID</param>
        /// <param name="x">起点x</param>
        /// <param name="y">起点y</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="pellucidity">透明度(0-255)</param>
        public static void DrawImage(uint[] texture, float x, float y, float width, float height, int pellucidity)
        {
            // 防溢出
            if (pellucidity < 0) pellucidity = 0;
            if (pellucidity > 255) pellucidity = 255;
            // 绑定纹理
            OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, texture[0]);
            OpenGL.glBegin(OpenGL.GL_QUADS);
            {
                // 设置透明度
                OpenGL.glColor4f(1f, 1f, 1f, pellucidity * 1f / 255);
                // 纹理坐标映射
                OpenGL.glTexCoord2f(0.0f, 0.0f); OpenGL.glVertex2f(x, y);
                OpenGL.glTexCoord2f(0.0f, 1.0f); OpenGL.glVertex2f(x, y + height);
                OpenGL.glTexCoord2f(1.0f, 1.0f); OpenGL.glVertex2f(x + width, y + height);
                OpenGL.glTexCoord2f(1.0f, 0.0f); OpenGL.glVertex2f(x + width, y);
                // 恢复透明度
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 性能计数
            PerformanceAnalyzer.Gaming_TextureCount++;
        }

        /// <summary>
        /// 绘制纹理
        /// </summary>
        /// <param name="texture">纹理ID</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="pellucidity">透明度(0-255)</param>
        public static void DrawImage(uint[] texture, RectangleF rect, int pellucidity)
        {
            DrawImage(texture, rect.X, rect.Y, rect.Width, rect.Height, pellucidity);
        }

        /// <summary>
        /// 绘制纹理
        /// </summary>
        /// <param name="texture">纹理ID</param>
        /// <param name="x">起点x</param>
        /// <param name="y">起点y</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="color">叠加颜色</param>
        public static void DrawImage(uint[] texture, float x, float y, float width, float height, Color color)
        {
            // 绑定纹理
            OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, texture[0]);
            OpenGL.glBegin(OpenGL.GL_QUADS);
            {
                // 设置颜色
                OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
                // 纹理坐标映射
                OpenGL.glTexCoord2f(0.0f, 0.0f); OpenGL.glVertex2f(x, y);
                OpenGL.glTexCoord2f(0.0f, 1.0f); OpenGL.glVertex2f(x, y + height);
                OpenGL.glTexCoord2f(1.0f, 1.0f); OpenGL.glVertex2f(x + width, y + height);
                OpenGL.glTexCoord2f(1.0f, 0.0f); OpenGL.glVertex2f(x + width, y);
                // 恢复透明度
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 性能计数
            PerformanceAnalyzer.Gaming_TextureCount++;
        }

        /// <summary>
        /// 绘制纹理 - 渐变
        /// </summary>
        /// <param name="texture">纹理ID</param>
        /// <param name="rect">矩形</param>
        /// <param name="colorTl">左上角颜色</param>
        /// <param name="colorTr">右上角颜色</param>
        /// <param name="colorLl">左下角颜色</param>
        /// <param name="colorLr">右下角颜色</param>
        public static void DrawImage(uint[] texture, Rectangle rect, Color colorTl, Color colorTr, Color colorLl, Color colorLr)
        {
            DrawImage(texture, rect.X, rect.Y, rect.Width, rect.Height, colorTl, colorTr, colorLl, colorLr);
        }

        /// <summary>
        /// 绘制纹理 - 渐变
        /// </summary>
        /// <param name="texture">纹理ID</param>
        /// <param name="x">起点x</param>
        /// <param name="y">起点y</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="colorTl">左上角颜色</param>
        /// <param name="colorTr">右上角颜色</param>
        /// <param name="colorLl">左下角颜色</param>
        /// <param name="colorLr">右下角颜色</param>
        public static void DrawImage(uint[] texture, float x, float y, float width, float height, Color colorTl, Color colorTr, Color colorLl, Color colorLr)
        {
            // 绑定纹理
            OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, texture[0]);
            OpenGL.glBegin(OpenGL.GL_QUADS);
            {
                // 设置左上角颜色
                OpenGL.glColor4f(colorTl.R * 1f / 255, colorTl.G * 1f / 255, colorTl.B * 1f / 255, colorTl.A * 1f / 255);
                // 纹理坐标映射
                OpenGL.glTexCoord2f(0.0f, 0.0f); OpenGL.glVertex2f(x, y);
                // 设置左下角颜色
                OpenGL.glColor4f(colorLl.R * 1f / 255, colorLl.G * 1f / 255, colorLl.B * 1f / 255, colorLl.A * 1f / 255);
                OpenGL.glTexCoord2f(0.0f, 1.0f); OpenGL.glVertex2f(x, y + height);
                // 设置右上角颜色
                OpenGL.glColor4f(colorLr.R * 1f / 255, colorLr.G * 1f / 255, colorLr.B * 1f / 255, colorLr.A * 1f / 255);
                OpenGL.glTexCoord2f(1.0f, 1.0f); OpenGL.glVertex2f(x + width, y + height);
                // 设置右下角颜色
                OpenGL.glColor4f(colorTr.R * 1f / 255, colorTr.G * 1f / 255, colorTr.B * 1f / 255, colorTr.A * 1f / 255);
                OpenGL.glTexCoord2f(1.0f, 0.0f); OpenGL.glVertex2f(x + width, y);
                // 恢复透明度
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 性能计数
            PerformanceAnalyzer.Gaming_TextureCount++;
        }

        /// <summary>
        /// 绘制纹理
        /// </summary>
        /// <param name="texture">纹理ID</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="color">叠加颜色</param>
        public static void DrawImage(uint[] texture, RectangleF rect, Color color)
        {
            DrawImage(texture, rect.X, rect.Y, rect.Width, rect.Height, color);
        }

        /// <summary>
        /// 绘制纹理
        /// </summary>
        /// <param name="texture">纹理ID</param>
        /// <param name="x">起点x</param>
        /// <param name="y">起点y</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="angle">旋转角度</param>
        public static void DrawImage(uint[] texture, float x, float y, float width, float height, float angle)
        {
            // 绑定纹理
            OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, texture[0]);
            // 旋转设置
            OpenGL.glRotatef(angle, 0, 0, 1);
            // 计算旋转后的中心点，纠正GL坐标系绕0,0,0旋转的偏移
            PointF p1 = new PointF(x + width / 2, y + height / 2);
            PointF p2 = GameSupport.RotatePoint(p1, new PointF(0, 0), angle);
            float xx = p2.X - p1.X;
            float yy = p2.Y - p1.Y;
            x += xx;
            y += yy;
            OpenGL.glBegin(OpenGL.GL_QUADS);
            {
                // 纹理坐标映射
                OpenGL.glTexCoord2f(0.0f, 0.0f); OpenGL.glVertex2f(x, y);
                OpenGL.glTexCoord2f(0.0f, 1.0f); OpenGL.glVertex2f(x, y + height);
                OpenGL.glTexCoord2f(1.0f, 1.0f); OpenGL.glVertex2f(x + width, y + height);
                OpenGL.glTexCoord2f(1.0f, 0.0f); OpenGL.glVertex2f(x + width, y);
            }
            OpenGL.glEnd();
            // 恢复旋转
            OpenGL.glRotatef(-angle, 0, 0, 1);
            // 性能计数
            PerformanceAnalyzer.Gaming_TextureCount++;
        }

        /// <summary>
        /// 绘制纹理
        /// </summary>
        /// <param name="texture">纹理ID</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="angle">旋转角度</param>
        public static void DrawImage(uint[] texture, RectangleF rect, float angle)
        {
            DrawImage(texture, rect.X, rect.Y, rect.Width, rect.Height, angle);
        }

        /// <summary>
        /// 绘制纹理
        /// </summary>
        /// <param name="texture">纹理ID</param>
        /// <param name="x">起点x</param>
        /// <param name="y">起点y</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="angle">旋转角度</param>
        /// <param name="pellucidity">透明度</param>
        public static void DrawImage(uint[] texture, float x, float y, float width, float height, float angle, int pellucidity)
        {
            // 绑定纹理
            OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, texture[0]);
            // 旋转设置
            OpenGL.glRotatef(angle, 0, 0, 1);
            // 计算旋转后的中心点
            PointF p1 = new PointF(x + width / 2, y + height / 2);
            PointF p2 = GameSupport.RotatePoint(p1, new PointF(0, 0), angle);
            float xx = p2.X - p1.X;
            float yy = p2.Y - p1.Y;
            x += xx;
            y += yy;
            // 防溢出
            if (pellucidity < 0) pellucidity = 0;
            if (pellucidity > 255) pellucidity = 255;
            OpenGL.glBegin(OpenGL.GL_QUADS);
            {
                // 设置透明度
                OpenGL.glColor4f(1f, 1f, 1f, pellucidity * 1f / 255);
                // 纹理坐标映射
                OpenGL.glTexCoord2f(0.0f, 0.0f); OpenGL.glVertex2f(x, y);
                OpenGL.glTexCoord2f(0.0f, 1.0f); OpenGL.glVertex2f(x, y + height);
                OpenGL.glTexCoord2f(1.0f, 1.0f); OpenGL.glVertex2f(x + width, y + height);
                OpenGL.glTexCoord2f(1.0f, 0.0f); OpenGL.glVertex2f(x + width, y);
                // 恢复透明度
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复旋转
            OpenGL.glRotatef(-angle, 0, 0, 1);
            // 性能计数
            PerformanceAnalyzer.Gaming_TextureCount++;
        }

        /// <summary>
        /// 绘制纹理
        /// </summary>
        /// <param name="texture">纹理ID</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="angle">旋转角度</param>
        /// <param name="pellucidity">透明度(0-255)</param>
        public static void DrawImage(uint[] texture, RectangleF rect, float angle, int pellucidity)
        {
            DrawImage(texture, rect.X, rect.Y, rect.Width, rect.Height, angle, pellucidity);
        }

        /// <summary>
        /// 绘制纹理
        /// </summary>
        /// <param name="texture">纹理ID</param>
        /// <param name="x">起点x</param>
        /// <param name="y">起点y</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="angle">旋转角度</param>
        /// <param name="centerPoint">旋转中心点</param>
        public static void DrawImage(uint[] texture, float x, float y, float width, float height, float angle, PointF centerPoint)
        {
            // 绑定纹理
            OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, texture[0]);
            // 旋转设置
            OpenGL.glRotatef(angle, 0, 0, 1);
            // 计算旋转后的中心点，纠正GL坐标系绕0,0,0旋转的偏移
            PointF p1 = new PointF(x + width / 2, y + height / 2);
            PointF p2 = GameSupport.RotatePoint(p1, new PointF(0, 0), angle);
            float xx = p2.X - p1.X;
            float yy = p2.Y - p1.Y;
            x += xx;
            y += yy;
            PointF p3 = GameSupport.RotatePoint(p1, centerPoint, angle);
            xx = p3.X - p1.X;
            yy = p3.Y - p1.Y;
            x -= xx;
            y -= yy;
            OpenGL.glBegin(OpenGL.GL_QUADS);
            {
                // 纹理坐标映射
                OpenGL.glTexCoord2f(0.0f, 0.0f); OpenGL.glVertex2f(x, y);
                OpenGL.glTexCoord2f(0.0f, 1.0f); OpenGL.glVertex2f(x, y + height);
                OpenGL.glTexCoord2f(1.0f, 1.0f); OpenGL.glVertex2f(x + width, y + height);
                OpenGL.glTexCoord2f(1.0f, 0.0f); OpenGL.glVertex2f(x + width, y);
            }
            OpenGL.glEnd();
            // 恢复旋转
            OpenGL.glRotatef(-angle, 0, 0, 1);
            // 性能计数
            PerformanceAnalyzer.Gaming_TextureCount++;
        }
        #endregion

        #region 绘制 纹理-顶点逆时针映射
        /// <summary>
        /// 绘制纹理 - 逆时针坐标映射
        /// </summary>
        /// <param name="texture">纹理ID</param>
        /// <param name="p1">点1</param>
        /// <param name="p2">点2</param>
        /// <param name="p3">点3</param>
        /// <param name="p4">点4</param>
        public static void DrawImage(uint[] texture, PointF p1, PointF p2, PointF p3, PointF p4)
        {
            // 绑定纹理
            OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, texture[0]);
            // 坐标转换
            OpenGL.glBegin(OpenGL.GL_QUADS);
            {
                // 纹理坐标映射
                OpenGL.glTexCoord2f(0.0f, 0.0f); OpenGL.glVertex2f(p1.X, p1.Y);
                OpenGL.glTexCoord2f(0.0f, 1.0f); OpenGL.glVertex2f(p2.X, p2.Y);
                OpenGL.glTexCoord2f(1.0f, 1.0f); OpenGL.glVertex2f(p3.X, p3.Y);
                OpenGL.glTexCoord2f(1.0f, 0.0f); OpenGL.glVertex2f(p4.X, p4.Y);
            }
            OpenGL.glEnd();
            // 性能计数
            PerformanceAnalyzer.Gaming_TextureCount++;
        }

        /// <summary>
        /// 绘制纹理 - 逆时针坐标映射
        /// </summary>
        /// <param name="texture">纹理ID</param>
        /// <param name="p1">点1</param>
        /// <param name="p2">点2</param>
        /// <param name="p3">点3</param>
        /// <param name="p4">点4</param>
        /// <param name="pellucidity">透明度</param>
        public static void DrawImage(uint[] texture, PointF p1, PointF p2, PointF p3, PointF p4, int pellucidity)
        {
            // 防溢出
            if (pellucidity < 0) pellucidity = 0;
            if (pellucidity > 255) pellucidity = 255;
            // 绑定纹理
            OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, texture[0]);
            // 坐标转换
            OpenGL.glBegin(OpenGL.GL_QUADS);
            {
                // 设置透明度
                OpenGL.glColor4f(1f, 1f, 1f, pellucidity * 1f / 255);
                // 纹理坐标映射
                OpenGL.glTexCoord2f(0.0f, 0.0f); OpenGL.glVertex2f(p1.X, p1.Y);
                OpenGL.glTexCoord2f(0.0f, 1.0f); OpenGL.glVertex2f(p2.X, p2.Y);
                OpenGL.glTexCoord2f(1.0f, 1.0f); OpenGL.glVertex2f(p3.X, p3.Y);
                OpenGL.glTexCoord2f(1.0f, 0.0f); OpenGL.glVertex2f(p4.X, p4.Y);
                // 恢复透明度
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 性能计数
            PerformanceAnalyzer.Gaming_TextureCount++;
        } 
        #endregion

        #region 绘制 部分纹理
        /// <summary>
        /// 绘制部分纹理
        /// </summary>
        /// <param name="texture">纹理</param>
        /// <param name="src">纹理原尺寸</param>
        /// <param name="rectSrc">需要绘制的区域</param>
        /// <param name="rectDst">绘制目标区域</param>
        public static void DrawImage(uint[] texture, SizeF src, RectangleF rectSrc, RectangleF rectDst)
        {
            // 绑定纹理
            OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, texture[0]);
            OpenGL.glBegin(OpenGL.GL_QUADS);
            {
                // 纹理坐标映射
                OpenGL.glTexCoord2f(rectSrc.X * 1f / src.Width, rectSrc.Y * 1f / src.Height);
                OpenGL.glVertex2f(rectDst.X, rectDst.Y);
                OpenGL.glTexCoord2f(rectSrc.X * 1f / src.Width, (rectSrc.Y + rectSrc.Height) * 1f / src.Height);
                OpenGL.glVertex2f(rectDst.X, rectDst.Y + rectDst.Height);
                OpenGL.glTexCoord2f((rectSrc.X + rectSrc.Width) * 1f / src.Width, (rectSrc.Y + rectSrc.Height) * 1f / src.Height);
                OpenGL.glVertex2f(rectDst.X + rectDst.Width, rectDst.Y + rectDst.Height);
                OpenGL.glTexCoord2f((rectSrc.X + rectSrc.Width) * 1f / src.Width, rectSrc.Y * 1f / src.Height);
                OpenGL.glVertex2f(rectDst.X + rectDst.Width, rectDst.Y);

            }
            OpenGL.glEnd();
            // 性能计数
            PerformanceAnalyzer.Gaming_TextureCount++;
        }

        /// <summary>
        /// 绘制部分纹理
        /// </summary>
        /// <param name="texture">纹理</param>
        /// <param name="src">纹理原尺寸</param>
        /// <param name="rectSrc">需要绘制的区域</param>
        /// <param name="rectDst">绘制目标区域</param>
        /// <param name="pellucidity">透明度(0-255)</param>
        public static void DrawImage(uint[] texture, SizeF src, RectangleF rectSrc, RectangleF rectDst, int pellucidity)
        {
            // 防溢出
            if (pellucidity < 0) pellucidity = 0;
            if (pellucidity > 255) pellucidity = 255;
            // 绑定纹理
            OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, texture[0]);
            // 坐标转换
            float y = General.Draw_Rect.Height - rectDst.Y - rectDst.Height;
            OpenGL.glBegin(OpenGL.GL_QUADS);
            {
                // 设置透明度
                OpenGL.glColor4f(1f, 1f, 1f, pellucidity * 1f / 255);
                // 纹理坐标映射
                OpenGL.glTexCoord2f(rectSrc.X * 1f / src.Width, rectSrc.Y * 1f / src.Height);
                OpenGL.glVertex2f(rectDst.X, rectDst.Y);
                OpenGL.glTexCoord2f(rectSrc.X * 1f / src.Width, (rectSrc.Y + rectSrc.Height) * 1f / src.Height);
                OpenGL.glVertex2f(rectDst.X, rectDst.Y + rectDst.Height);
                OpenGL.glTexCoord2f((rectSrc.X + rectSrc.Width) * 1f / src.Width, (rectSrc.Y + rectSrc.Height) * 1f / src.Height);
                OpenGL.glVertex2f(rectDst.X + rectDst.Width, rectDst.Y + rectDst.Height);
                OpenGL.glTexCoord2f((rectSrc.X + rectSrc.Width) * 1f / src.Width, rectSrc.Y * 1f / src.Height);
                OpenGL.glVertex2f(rectDst.X + rectDst.Width, rectDst.Y);
                // 恢复透明度
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 性能计数
            PerformanceAnalyzer.Gaming_TextureCount++;
        }

        /// <summary>
        /// 绘制部分纹理
        /// </summary>
        /// <param name="texture">纹理</param>
        /// <param name="src">纹理原尺寸</param>
        /// <param name="rectSrc">需要绘制的区域</param>
        /// <param name="rectDst">绘制目标区域</param>
        /// <param name="color">叠加颜色</param>
        public static void DrawImage(uint[] texture, SizeF src, RectangleF rectSrc, RectangleF rectDst, Color color)
        {
            // 绑定纹理
            OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, texture[0]);
            // 坐标转换
            float y = General.Draw_Rect.Height - rectDst.Y - rectDst.Height;
            OpenGL.glBegin(OpenGL.GL_QUADS);
            {
                // 设置颜色
                OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
                // 纹理坐标映射
                OpenGL.glTexCoord2f(rectSrc.X * 1f / src.Width, rectSrc.Y * 1f / src.Height);
                OpenGL.glVertex2f(rectDst.X, rectDst.Y);
                OpenGL.glTexCoord2f(rectSrc.X * 1f / src.Width, (rectSrc.Y + rectSrc.Height) * 1f / src.Height);
                OpenGL.glVertex2f(rectDst.X, rectDst.Y + rectDst.Height);
                OpenGL.glTexCoord2f((rectSrc.X + rectSrc.Width) * 1f / src.Width, (rectSrc.Y + rectSrc.Height) * 1f / src.Height);
                OpenGL.glVertex2f(rectDst.X + rectDst.Width, rectDst.Y + rectDst.Height);
                OpenGL.glTexCoord2f((rectSrc.X + rectSrc.Width) * 1f / src.Width, rectSrc.Y * 1f / src.Height);
                OpenGL.glVertex2f(rectDst.X + rectDst.Width, rectDst.Y);
                // 恢复透明度
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 性能计数
            PerformanceAnalyzer.Gaming_TextureCount++;
        } 
        #endregion

        #region 绘制 渐变遮罩纹理
        /// <summary>
        /// 绘制渐变纹理
        /// </summary>
        /// <param name="texture">纹理ID</param>
        /// <param name="x">起点x</param>
        /// <param name="y">起点y</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="pellucidity">透渐变程度</param>
        /// <param name="flag">透明方式标记</param>
        public static void DrawImageTrans(uint[] texture, float x, float y, float width, float height, int pellucidity, bool flag)
        {
            // 设置融合方式
            OpenGL.glBlendFunc(OpenGL.GL_ONE_MINUS_SRC_COLOR, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            // 防溢出
            if (pellucidity < 0) pellucidity = 0;
            if (pellucidity > 255) pellucidity = 255;
            // 绑定纹理
            OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, texture[0]);
            OpenGL.glBegin(OpenGL.GL_QUADS);
            {
                // 设置透明度
                if (flag)
                {
                    OpenGL.glColor4f(pellucidity * 1f / 255, pellucidity * 1f / 255, pellucidity * 1f / 255, pellucidity * 1f / 255);
                }
                else
                {
                    OpenGL.glColor4f(1 - pellucidity * 1f / 255, 1 - pellucidity * 1f / 255, 1 - pellucidity * 1f / 255, pellucidity * 1f / 255);
                }
                // 纹理坐标映射
                OpenGL.glTexCoord2f(0.0f, 0.0f); OpenGL.glVertex2f(x, y);
                OpenGL.glTexCoord2f(0.0f, 1.0f); OpenGL.glVertex2f(x, y + height);
                OpenGL.glTexCoord2f(1.0f, 1.0f); OpenGL.glVertex2f(x + width, y + height);
                OpenGL.glTexCoord2f(1.0f, 0.0f); OpenGL.glVertex2f(x + width, y);
                // 恢复透明度
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复融合
            OpenGL.glBlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            // 性能计数
            PerformanceAnalyzer.Gaming_TextureCount++;
        } 
        #endregion

        #region 绘制 点
        /// <summary>
        /// 设置点尺寸
        /// </summary>
        /// <param name="width">宽度</param>
        public static void SetPointWitth(float width)
        {
            // 设定线宽
            if (width < 1f) width = 1f;
            OpenGL.glPointSize(width);
        }

        /// <summary>
        /// 绘制点
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="p">起点</param>
        public static void DrawPoint(Color color, PointF p)
        {
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            OpenGL.glBegin(OpenGL.GL_POINTS);
            {
                // 设置颜色
                OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
                OpenGL.glVertex2f(p.X, p.Y);
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        } 
        #endregion

        #region 绘制 直线
        /// <summary>
        /// 设置线宽度 - 对所有非填充图形有效
        /// </summary>
        /// <param name="width">宽度</param>
        public static void SetLineWitth(float width)
        {
            // 设定线宽
            if (width < 1f) width = 1f;
            OpenGL.glLineWidth(width);
        }

        /// <summary>
        /// 绘制线段
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="p1">起点</param>
        /// <param name="p2">终点</param>
        public static void DrawLine(Color color, PointF p1, PointF p2)
        {
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            OpenGL.glBegin(OpenGL.GL_LINE_LOOP);
            {
                // 设置颜色
                OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
                OpenGL.glVertex2f(p1.X, p1.Y);
                OpenGL.glVertex2f(p2.X, p2.Y);
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        } 
        #endregion

        #region 绘制 弧线 / 扇形 / 圆形
        /// <summary>
        /// 绘制弧线
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="pointCenter">圆心坐标</param>
        /// <param name="radius">半径</param>
        /// <param name="startAngle">开始角度</param>
        /// <param name="endAngle">结束角度</param>
        /// <param name="deltaAngle">角度跨度(绘制精度)</param>
        public static void DrawArc(Color color, PointF pointCenter, float radius, float startAngle, float endAngle, float deltaAngle)
        {
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            OpenGL.glBegin(OpenGL.GL_LINE_STRIP);
            {
                // 设置颜色
                OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
                for (float i = startAngle; i <= endAngle; i += deltaAngle)
                {
                    double vx = pointCenter.X + radius * Math.Cos(i / 180f * Math.PI);
                    double vy = pointCenter.Y + radius * Math.Sin(i / 180f * Math.PI);
                    OpenGL.glVertex2d(vx, vy);
                }
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        }

        /// <summary>
        /// 绘制饼图
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="pointCenter">圆心坐标</param>
        /// <param name="radius">半径</param>
        /// <param name="startAngle">开始角度</param>
        /// <param name="endAngle">结束角度</param>
        /// <param name="deltaAngle">角度跨度(绘制精度)</param>
        public static void DrawPie(Color color, PointF pointCenter, float radius, float startAngle, float endAngle, float deltaAngle)
        {
            DrawArc(color, pointCenter, radius, startAngle, endAngle, deltaAngle);
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            OpenGL.glBegin(OpenGL.GL_LINES);
            {
                // 设置颜色
                OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
                OpenGL.glVertex2d(pointCenter.X, pointCenter.Y);
                OpenGL.glVertex2d(pointCenter.X + radius * Math.Cos(startAngle / 180f * Math.PI), pointCenter.Y + radius * Math.Sin(startAngle / 180f * Math.PI));
                OpenGL.glVertex2d(pointCenter.X, pointCenter.Y);
                OpenGL.glVertex2d(pointCenter.X + radius * Math.Cos(endAngle / 180f * Math.PI), pointCenter.Y + radius * Math.Sin(endAngle / 180f * Math.PI));
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        }

        /// <summary>
        /// 填充饼图
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="pointCenter">圆心坐标</param>
        /// <param name="radius">半径</param>
        /// <param name="startAngle">开始角度</param>
        /// <param name="endAngle">结束角度</param>
        /// <param name="deltaAngle">角度跨度(绘制精度)</param>
        public static void FillPie(Color color, PointF pointCenter, float radius, float startAngle, float endAngle, float deltaAngle)
        {
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            OpenGL.glBegin(OpenGL.GL_TRIANGLE_STRIP);
            {
                // 设置颜色
                OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
                for (float i = startAngle + deltaAngle; i <=endAngle; i += deltaAngle)
                {
                    OpenGL.glVertex2d(pointCenter.X, pointCenter.Y);
                    double vx = pointCenter.X + radius * Math.Cos(i / 180f * Math.PI);
                    double vy = pointCenter.Y + radius * Math.Sin(i / 180f * Math.PI);
                    OpenGL.glVertex2d(vx, vy);
                    vx = pointCenter.X + radius * Math.Cos((i + deltaAngle) / 180f * Math.PI);
                    vy = pointCenter.Y + radius * Math.Sin((i + deltaAngle) / 180f * Math.PI);
                    OpenGL.glVertex2d(vx, vy);
                }
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        }

        /// <summary>
        /// 绘制圆形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="pointCenter">圆心坐标</param>
        /// <param name="radius">半径</param>
        /// <param name="deltaAngle">角度跨度(绘制精度)</param>
        public static void DrawCircle(Color color, PointF pointCenter, float radius, float deltaAngle)
        {
            DrawArc(color, pointCenter, radius, 0, 360, deltaAngle);
        }

        /// <summary>
        /// 填充圆形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="pointCenter">圆心坐标</param>
        /// <param name="radius">半径</param>
        /// <param name="deltaAngle">角度跨度(绘制精度)</param>
        public static void FillCircle(Color color, PointF pointCenter, float radius, float deltaAngle)
        {
            FillPie(color, pointCenter, radius, 0, 360, deltaAngle);
        } 
        #endregion

        #region 绘制 三角形
        /// <summary>
        /// 绘制三角形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="p1">顶点1</param>
        /// <param name="p2">顶点2</param>
        /// <param name="p3">顶点3</param>
        public static void DrawTriangles(Color color, PointF p1, PointF p2, PointF p3)
        {
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            // 坐标转换
            OpenGL.glBegin(OpenGL.GL_LINE_LOOP);
            {
                // 设置颜色
                OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
                OpenGL.glVertex2f(p1.X, p1.Y);
                OpenGL.glVertex2f(p2.X, p2.Y);
                OpenGL.glVertex2f(p3.X, p3.Y);
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        } 

        /// <summary>
        /// 填充三角形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="p1">顶点1</param>
        /// <param name="p2">顶点2</param>
        /// <param name="p3">顶点3</param>
        public static void FillTriangles(Color color, PointF p1, PointF p2, PointF p3)
        {
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            // 坐标转换
            OpenGL.glBegin(OpenGL.GL_TRIANGLE_STRIP);
            {
                // 设置颜色
                OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
                OpenGL.glVertex2f(p1.X, p1.Y);
                OpenGL.glVertex2f(p2.X, p2.Y);
                OpenGL.glVertex2f(p3.X, p3.Y);
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        }
        #endregion

        #region 绘制 矩形
        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="rect">矩形</param>
        public static void DrawRectangle(Color color, RectangleF rect)
        {
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            OpenGL.glBegin(OpenGL.GL_LINE_LOOP);
            {
                // 设置颜色
                OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
                OpenGL.glVertex2f(rect.X, rect.Y);
                OpenGL.glVertex2f(rect.X, rect.Y + rect.Height);
                OpenGL.glVertex2f(rect.X + rect.Width, rect.Y + rect.Height);
                OpenGL.glVertex2f(rect.X + rect.Width, rect.Y);
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        }

        /// <summary>
        /// 绘制矩形 - xy偏移
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="rect">矩形</param>
        /// <param name="offsetX">偏移X</param>
        /// <param name="offsetY">偏移Y</param>
        public static void DrawRectangle(Color color, RectangleF rect, float offsetX, float offsetY)
        {
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            OpenGL.glBegin(OpenGL.GL_LINE_LOOP);
            {
                // 设置颜色
                OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
                OpenGL.glVertex2f(rect.X + offsetX, rect.Y + offsetY);
                OpenGL.glVertex2f(rect.X + offsetX, rect.Y + rect.Height + offsetY);
                OpenGL.glVertex2f(rect.X + rect.Width + offsetX, rect.Y + rect.Height + offsetY);
                OpenGL.glVertex2f(rect.X + rect.Width + offsetX, rect.Y + offsetY);
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="x">起点x</param>
        /// <param name="y">起点y</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public static void DrawRectangle(Color color, float x, float y, float width, float height)
        {
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            // 坐标转换
            OpenGL.glBegin(OpenGL.GL_LINE_LOOP);
            {
                // 设置颜色
                OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
                OpenGL.glVertex2f(x, y);
                OpenGL.glVertex2f(x, y + height);
                OpenGL.glVertex2f(x + width, y + height);
                OpenGL.glVertex2f(x + width, y);
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        }

        /// <summary>
        /// 填充矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="rect">矩形</param>
        public static void FillRectangle(Color color, RectangleF rect)
        {
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            OpenGL.glBegin(OpenGL.GL_QUADS);
            {
                // 设置颜色
                OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
                OpenGL.glVertex2f(rect.X, rect.Y);
                OpenGL.glVertex2f(rect.X, rect.Y + rect.Height);
                OpenGL.glVertex2f(rect.X + rect.Width, rect.Y + rect.Height);
                OpenGL.glVertex2f(rect.X + rect.Width, rect.Y);
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        }

        /// <summary>
        /// 填充矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="x">起点x</param>
        /// <param name="y">起点y</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public static void FillRectangle(Color color, float x, float y, float width, float height)
        {
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            OpenGL.glBegin(OpenGL.GL_QUADS);
            {
                // 设置颜色
                OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
                OpenGL.glVertex2f(x, y);
                OpenGL.glVertex2f(x, y + height);
                OpenGL.glVertex2f(x + width, y + height);
                OpenGL.glVertex2f(x + width, y);
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        }

        /// <summary>
        /// 绘制渐变矩形
        /// </summary>
        /// <param name="colorTL">左上角颜色</param>
        /// <param name="colorTr">右上角颜色</param>
        /// <param name="colorLl">左下角颜色</param>
        /// <param name="colorLr">右下角颜色</param>
        /// <param name="rect">矩形</param>
        public static void FillRectangleGradient(Color colorTL, Color colorTr, Color colorLl, Color colorLr, RectangleF rect)
        {
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            OpenGL.glBegin(OpenGL.GL_QUADS);
            {
                OpenGL.glColor4f(colorTL.R * 1f / 255, colorTL.G * 1f / 255, colorTL.B * 1f / 255, colorTL.A * 1f / 255);
                OpenGL.glVertex2f(rect.X, rect.Y);
                OpenGL.glColor4f(colorLl.R * 1f / 255, colorLl.G * 1f / 255, colorLl.B * 1f / 255, colorLl.A * 1f / 255);
                OpenGL.glVertex2f(rect.X, rect.Y + rect.Height);
                OpenGL.glColor4f(colorLr.R * 1f / 255, colorLr.G * 1f / 255, colorLr.B * 1f / 255, colorLr.A * 1f / 255);
                OpenGL.glVertex2f(rect.X + rect.Width, rect.Y + rect.Height);
                OpenGL.glColor4f(colorTr.R * 1f / 255, colorTr.G * 1f / 255, colorTr.B * 1f / 255, colorTr.A * 1f / 255);
                OpenGL.glVertex2f(rect.X + rect.Width, rect.Y);
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        }
        #endregion

        #region 绘制 多边形
        /// <summary>
        /// 绘制多边形(线循环，顺序无关)
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="pointList">顶点列表</param>
        public static void DrawPolygon(Color color, List<PointF> pointList)
        {
            if (pointList.Count < 4) return;
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            OpenGL.glBegin(OpenGL.GL_LINE_LOOP);
            {
                // 设置颜色
                OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
                for (int i = 0; i < pointList.Count; i++)
                {
                    OpenGL.glVertex2f(pointList[i].X, pointList[i].Y);
                }
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        }

        /// <summary>
        /// 填充多边形(逆时针序列)
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="pointList">顶点列表</param>
        public static void FillPolygon(Color color, List<PointF> pointList)
        {
            if (pointList.Count < 4) return;
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            OpenGL.glPolygonMode(OpenGL.GL_FRONT, OpenGL.GL_FILL);
            OpenGL.glBegin(OpenGL.GL_POLYGON);
            {
                // 设置颜色
                OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
                for (int i = 0; i < pointList.Count; i++)
                {
                    OpenGL.glVertex2f(pointList[i].X, pointList[i].Y);
                }
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        }

        /// <summary>
        /// 填充多边形(逆时针序列)
        /// </summary>
        /// <param name="colorList">颜色列表</param>
        /// <param name="pointList">顶点列表</param>
        public static void FillPolygon(List<Color> colorList, List<PointF> pointList)
        {
            if (pointList.Count < 4 || pointList.Count != colorList.Count) return;
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            OpenGL.glPolygonMode(OpenGL.GL_FRONT, OpenGL.GL_FILL);
            OpenGL.glBegin(OpenGL.GL_POLYGON);
            {
                for (int i = 0; i < pointList.Count; i++)
                {
                    // 设置颜色
                    OpenGL.glColor4f(colorList[i].R * 1f / 255, colorList[i].G * 1f / 255, colorList[i].B * 1f / 255, colorList[i].A * 1f / 255);
                    OpenGL.glVertex2f(pointList[i].X, pointList[i].Y);
                }
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        } 
        #endregion

        #region 绘制 贝塞尔曲线
        /// <summary>
        /// 绘制贝塞尔曲线(3次，4控制点)
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="p1">控制点1</param>
        /// <param name="p2">控制点2</param>
        /// <param name="p3">控制点3</param>
        /// <param name="p4">控制点4</param>
        public static void DrawBezier(Color color, PointF p1, PointF p2, PointF p3, PointF p4)
        {
            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            OpenGL.glBegin(OpenGL.GL_LINES);
            {
                // 设置颜色
                OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
                PointF tempPoint = p1;
                PointF endPoint = new PointF();
                // 绘制精度
                float delta = 0.01f;
                for (double t = 0.0f; t < 1.0f; t += delta)
                {
                    endPoint.X = (float)(Math.Pow((1 - t), 3) * p1.X + 3 * t * Math.Pow((1 - t), 2) * p2.X + 3 * (1 - t) * Math.Pow(t, 2) * p3.X + Math.Pow(t, 3) * p4.X);
                    endPoint.Y = (float)(Math.Pow((1 - t), 3) * p1.Y + 3 * t * Math.Pow((1 - t), 2) * p2.Y + 3 * (1 - t) * Math.Pow(t, 2) * p3.Y + Math.Pow(t, 3) * p4.Y);
                    OpenGL.glVertex2f(tempPoint.X, tempPoint.Y);
                    OpenGL.glVertex2f(endPoint.X, endPoint.Y);
                    tempPoint = endPoint;
                }
                // 恢复颜色
                OpenGL.glColor4f(1f, 1f, 1f, 1f);
            }
            OpenGL.glEnd();
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 性能计数
            PerformanceAnalyzer.Gaming_ElementCount++;
        }   
        #endregion
    }
}
