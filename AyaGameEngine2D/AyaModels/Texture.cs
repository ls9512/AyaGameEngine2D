using System;
using System.Drawing;
using System.Drawing.Imaging;

using CsGL.OpenGL;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：Texture
    /// 功      能：贴图纹理类，提供对纹理对象和相关参数的封装
    /// 日      期：2015-03-20
    /// 修      改：2015-12-29
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class Texture : IDisposable
    {
        #region 公有字段
        /// <summary>
        /// 纹理ID
        /// </summary>
        public uint[] TextureID
        {
            get { return _textureID; }
        }
        private uint[] _textureID;

        /// <summary>
        /// 纹理源图像
        /// </summary>
        public Bitmap Bitmap
        {
            get { return _bitmap; }
        }
        private Bitmap _bitmap;

        /// <summary>
        /// 纹理宽度
        /// </summary>
        public int Width
        {
            get { return _width; }
        }
        private int _width;

        /// <summary>
        /// 纹理高度
        /// </summary>
        public int Height
        {
            get { return _height; }
        }
        private int _height; 
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName">文件名</param>
        public Texture(string fileName)
        {
            // 加载文件
            Bitmap bitmap;
            try
            {
                bitmap = new Bitmap(fileName);
            }
            catch
            {
                bitmap = BitmapHelper.ErrorBitmap;
            }
            // 转换为32位位图
            Bitmap bmp_new = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp_new);
            g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
            // 创建纹理
            CreateTexture(bmp_new);
            // 释放内存
            g.Dispose();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bitmap">位图</param>
        public Texture(Bitmap bitmap)
        {
            // 创建纹理
            CreateTexture(bitmap);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="textureID">纹理ID</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public Texture(uint[] textureID, int width, int height)
        {
            _textureID = textureID;
            _width = width;
            _height = height;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 创建纹理
        /// </summary>
        /// <param name="bitmap">图像</param>
        private void CreateTexture(Bitmap bitmap)
        {
            _bitmap = bitmap;
            _textureID = GraphicHelper.CreateTexture2D(bitmap);
            _width = _bitmap.Width;
            _height = _bitmap.Height;
        } 
        #endregion

        #region 销毁
        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            uint[] tex = _textureID;
            if (tex == null)
            {
                return;
            }
            GL.glDeleteTextures(tex.Length, tex);
            if (_bitmap != null)
            {
                _bitmap.Dispose();
            }
        } 
        #endregion
    }
}
