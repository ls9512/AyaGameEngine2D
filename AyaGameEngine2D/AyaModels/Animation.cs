using System;
using System.Drawing;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：Animation
    /// 功      能：动画类，提供将TextureMatrix纹理处理成逐帧动画的方法
    /// 日      期：2015-03-20
    /// 修      改：2015-03-30
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class Animation : IDisposable
    {
        #region 公有字段
        /// <summary>
        /// 坐标X
        /// </summary>
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }
        private float _x;

        /// <summary>
        /// 坐标Y
        /// </summary>
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }
        private float _y;

        /// <summary>
        /// 帧纹理
        /// </summary>
        public TextureMatrix TextureFrame
        {
            get { return _textureFrame; }
        }
        private readonly TextureMatrix _textureFrame;

        /// <summary>
        /// 纹理宽度
        /// </summary>
        public int Width
        {
            get { return _width; }
        }
        private readonly int _width;

        /// <summary>
        /// 纹理高度
        /// </summary>
        public int Height
        {
            get { return _height; }
        }
        private readonly int _height;

        /// <summary>
        /// 所在矩形
        /// </summary>
        public virtual RectangleF ObjectRect
        {
            get { return new RectangleF(_x, _y, _width, _height); }
        }

        /// <summary>
        /// 当前帧数
        /// </summary>
        public int NowFrame
        {
            get { return _nowFrame; }
            set
            {
                _nowFrame = value;
                _nowFrameTemp = _nowFrame;
            }
        }
        private int _nowFrame;

        /// <summary>
        /// 当前帧坐标
        /// </summary>
        public Point NowFrameLoc
        {
            get { return GetFrameLoc(_nowFrame); }
        }

        /// <summary>
        /// 当前帧数·调速
        /// </summary>
        private float _nowFrameTemp;

        /// <summary>
        /// 开始帧数
        /// </summary>
        public int StartFrame
        {
            get { return _startFrame; }
            set
            {
                _startFrame = value;
                if (_startFrame < 0) _startFrame = 0;
                if (_startFrame > _endFrame) _startFrame = _endFrame;
                if (_nowFrame < _startFrame) _nowFrame = _startFrame;
                _nowFrameTemp = _nowFrame;
            }
        }
        private int _startFrame;

        /// <summary>
        /// 结束帧数
        /// </summary>
        public int EndFrame
        {
            get { return _endFrame; }
            set { _endFrame = value; }
        }
        private int _endFrame;

        /// <summary>
        /// 动画速度
        /// ======================================================
        /// 默认速度为Norml
        /// 当速度小于Normal时,会等待一定帧数绘制下一帧
        /// 当速度大于Normal时,会有一定程度的跳帧
        /// </summary>
        public FrameSpeed Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        private FrameSpeed _speed;

        /// <summary>
        /// 循环标志
        /// ======================================================
        /// 如果为真则动画一直循环播放
        /// 如果为假则播放到最后一个有效帧停止
        /// </summary>
        public bool IsLoop
        {
            get { return _isLoop; }
            set { _isLoop = value; }
        }
        private bool _isLoop;

        /// <summary>
        /// 结束标志
        /// ======================================================
        /// 当IsLoop为假时，如果播放到最后一帧，则该值为真
        /// 否则该值为假
        /// 如果需要重新播放，请调用Reset方法重置，否则将停留在最后一帧
        /// </summary>
        public bool IsEnd
        {
            get { return _isEnd; }
        }
        private bool _isEnd; 
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="textureMatrix">矩阵纹理</param>
        public Animation(TextureMatrix textureMatrix)
        {
            _textureFrame = textureMatrix;
            _nowFrame = 0;
            _nowFrameTemp = 0;
            _startFrame = 0;
            _endFrame = _textureFrame.ValidFrame;
            _width = textureMatrix.Width;
            _height = textureMatrix.Height;
            _isLoop = true;
            _isEnd = false;
            _speed = FrameSpeed.Normal;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="textureMatrixFilePath">矩阵纹理文件路径</param>
        /// <param name="numX">纹理横向个数</param>
        /// <param name="numY">纹理纵向个数</param>
        public Animation(string textureMatrixFilePath, int numX, int numY)
        {
            _textureFrame = new TextureMatrix(textureMatrixFilePath, numX, numY);
            _nowFrame = 0;
            _nowFrameTemp = 0;
            _startFrame = 0;
            _endFrame = _textureFrame.ValidFrame;
            _width = _textureFrame.Width;
            _height = _textureFrame.Height;
            _isLoop = true;
            _isEnd = false;
            _speed = FrameSpeed.Normal;
        } 
        #endregion

        #region 公有方法
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            _isEnd = false;
            _nowFrame = _startFrame;
            _nowFrameTemp = _nowFrame;
        }

        /// <summary>
        /// 获取当前帧纹理ID用于绘制
        /// </summary>
        /// <returns>纹理ID</returns>
        public uint[] GetTextureFrameID()
        {
            // 计算当前帧纹理矩阵坐标
            Point p = GetFrameLoc(_nowFrame);
            // 跳到下一帧
            NextFrame();
            // 返回纹理ID
            return _textureFrame.TextureID[p.X, p.Y];
        }

        /// <summary>
        /// 根据索引获取帧纹理ID
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>纹理ID</returns>
        public uint[] GetTextureIDbyIndex(int index)
        {
            Point p = GetFrameLoc(index);
            return _textureFrame.TextureID[p.X, p.Y];
        }

        /// <summary>
        /// 跳到下一帧
        /// </summary>
        public void NextFrame()
        {
            // 结束时判断是否需要循环
            if (_nowFrame + 1 > _endFrame)
            {
                if (_isLoop)
                {
                    _nowFrame = _startFrame;
                    _nowFrameTemp = _startFrame;
                }
                else
                {
                    // 结束
                    _isEnd = true;
                    return;
                }
            }
            // 下一帧
            if (_nowFrame < _endFrame)
            {
                // 速度控制
                float speed = 1.0f;
                switch (_speed)
                {
                    case FrameSpeed.Slowest: speed = 0.0625f; break;
                    case FrameSpeed.Slower: speed = 0.125f; break;
                    case FrameSpeed.Slow: speed = 0.25f; break;
                    case FrameSpeed.Normal: speed = 0.50f; break;
                    case FrameSpeed.Fast: speed = 0.75f; break;
                    case FrameSpeed.Faster: speed = 1.0f; break;
                    case FrameSpeed.Fastest: speed = 1.5f; break;
                }
                _nowFrameTemp += speed * Time.DeltaTime;
                _nowFrame = (int)_nowFrameTemp;
                // 防溢出
                if (_nowFrame > _endFrame) _nowFrame = _endFrame;
            }
        }

        /// <summary>
        /// 获取某一帧在纹理矩阵中的坐标
        /// </summary>
        /// <param name="frameIndex">帧索引</param>
        /// <returns>坐标点</returns>
        public Point GetFrameLoc(int frameIndex)
        {
            int y = frameIndex / _textureFrame.NumX;
            int x = frameIndex - y * _textureFrame.NumX;
            return new Point(x, y);
        } 
        #endregion

        #region 销毁
        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            if (_textureFrame != null)
            {
                _textureFrame.Dispose();
            }
        } 
        #endregion
    }
}
