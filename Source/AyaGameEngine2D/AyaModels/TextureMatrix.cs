using System;
using System.Drawing;
using System.Drawing.Imaging;

using CsGL.OpenGL;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：TextureMatrix
    /// 功      能：矩阵贴图纹理类，提供 X * Y 的纹理素材和相关参数封装
    /// 日      期：2015-03-20
    /// 修      改：2015-03-20
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class TextureMatrix : IDisposable
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

        /// <summary>
        /// 横向纹理个数
        /// </summary>
        public int NumX
        {
            get { return _numX; }
        }
        private int _numX;

        /// <summary>
        /// 纵向纹理个数
        /// </summary>
        public int NumY
        {
            get { return _numY; }
        }
        private int _numY;

        /// <summary>
        /// 有效帧
        /// ======================================================
        /// X * Y 的矩阵中，有效帧数量可能不足 X * Y 有空白帧，可以设置此值来防止动画空帧
        /// 该类实例化后有效帧默认值为 X * Y - 1
        /// 注：帧数从0下标开始
        /// </summary>
        public int ValidFrame
        {
            get { return _validFrame; }
            set
            {
                _validFrame = value;
                // 防止溢出
                if (_validFrame < 0) _validFrame = 0;
                if (_validFrame > _width * _height - 1) _validFrame = _width * _height - 1;
            }
        }
        private int _validFrame; 
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="numX">横向个数</param>
        /// <param name="numY">纵向个数</param>
        public TextureMatrix(string fileName, int numX, int numY)
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
            CreateTextureGroup(bmp_new, numX, numY);
            // 释放内存
            g.Dispose();
            bmp_new.Dispose();
        }

        /// <summary>
        /// 构造函数组
        /// </summary>
        /// <param name="bitmap">位图</param>
        /// <param name="numX">横向个数</param>
        /// <param name="numY">纵向个数</param>
        public TextureMatrix(Bitmap bitmap, int numX, int numY)
        {
            // 创建纹理组
            CreateTextureGroup(bitmap, numX, numY);
        } 
        #endregion

        #region 私有方法
        /// <summary>
        /// 创建纹理数组
        /// </summary>
        /// <param name="bitmap">图片</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        private void CreateTextureGroup(Bitmap bitmap, int width, int height)
        {
            // 创建纹理和图像数组
            _textureID = new uint[width, height][];
            _bitmap = new Bitmap[width, height];
            // 切割参数
            int cutWidth = bitmap.Width / width;
            int cutHeight = bitmap.Height / height;
            // 循环生成纹理和图像
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    // 切割图像
                    Bitmap bitmap_temp = BitmapHelper.BitmapCut(bitmap, i * cutWidth, j * cutHeight, cutWidth, cutHeight);
                    // 保存切割后的图像
                    _bitmap[i, j] = bitmap_temp;
                    // 创建切割后图像的纹理
                    _textureID[i, j] = GraphicHelper.CreateTexture2D(bitmap_temp);
                }
            }
            // 其他参数生成
            _numX = width;
            _numY = height;
            _width = _bitmap[0, 0].Width;
            _height = _bitmap[0, 0].Height;
            _validFrame = _numX * _numY - 1;
        } 
        #endregion

        #region 销毁
        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
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
