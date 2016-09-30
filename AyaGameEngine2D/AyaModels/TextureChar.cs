using System;
using System.Drawing;
using System.Drawing.Imaging;

using CsGL.OpenGL;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：TextureChar (TextureCharacter)
    /// 功      能：角色贴图纹理类，提供 4x4 的带方向纹理素材和相关参数封装
    /// 日      期：2015-03-20
    /// 修      改：2015-03-20
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class TextureChar : IDisposable
    {
        #region 公有字段
        /// <summary>
        /// 纹理ID数组
        /// </summary>
        public uint[,][] TextureID
        {
            get { return _textureID; }
        }
        private uint[,][] _textureID;

        /// <summary>
        /// 纹理源图像
        /// </summary>
        public Bitmap[,] Bitmap
        {
            get { return _bitmap; }
        }
        private Bitmap[,] _bitmap;

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
        public TextureChar(string fileName)
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
            // 创建纹理组
            CreateTextureGroup(bmp_new);
            // 释放内存
            g.Dispose();
            bmp_new.Dispose();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bitmap">位图</param>
        public TextureChar(Bitmap bitmap)
        {
            // 创建纹理组
            CreateTextureGroup(bitmap);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 创建纹理组
        /// </summary>
        /// <param name="bitmap"></param>
        private void CreateTextureGroup(Bitmap bitmap)
        {
            // 创建纹理和图像数组
            _textureID = new uint[4, 4][];
            _bitmap = new Bitmap[4, 4];
            // 切割参数
            int width = bitmap.Width / 4;
            int height = bitmap.Height / 4;
            // 循环生成纹理和图像
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    // 切割图像
                    Bitmap bitmap_temp = BitmapHelper.BitmapCut(bitmap, i * width, j * height, width, height);
                    // 保存切割后的图像
                    _bitmap[i, j] = bitmap_temp;
                    // 创建切割后图像的纹理
                    _textureID[i, j] = GraphicHelper.CreateTexture2D(bitmap_temp);
                }
            }
            // 其他参数生成
            _width = _bitmap[0, 0].Width;
            _height = _bitmap[0, 0].Height;
        }
        #endregion

        #region 销毁
        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    uint[] tex = _textureID[i, j];
                    if (tex != null)
                    {
                        GL.glDeleteTextures(tex.Length, tex);
                    }
                    if (_bitmap[i, j] != null)
                    {
                        _bitmap[i, j].Dispose();
                    }
                }
            }
        } 
        #endregion
    }
}
