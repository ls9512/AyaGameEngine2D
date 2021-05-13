using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using GH = AyaGameEngine2D.GraphicHelper;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：EngineInfo
    /// 功      能：引擎级消息类
    /// 日      期：2015-11-21
    /// 修      改：2015-11-21
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class EngineInfo :IDisposable
    {
        #region 公有字段
        /// <summary>
        /// 消息文本
        /// </summary>
        public string Text
        {
            get { return _text; }
        }
        private readonly string _text;

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title
        {
            get { return _title; }
        }
        private readonly string _title;

        /// <summary>
        /// 位置
        /// </summary>
        public PointF Loaction
        {
            get { return _location; }
        }
        private PointF _location;

        /// <summary>
        /// 宽度
        /// </summary>
        public float Width
        {
            get { return _width; }
        }
        private float _width;

        /// <summary>
        /// 高度
        /// </summary>
        public float Height
        {
            get { return _height; }
        }
        private float _height;

        /// <summary>
        /// 绘制矩形
        /// </summary>
        public RectangleF DrawRect
        {
            get { return new RectangleF(_location.X, _location.Y, _width, _height); }
        } 
        #endregion

        #region 私有字段
        /// <summary>
        /// 默认字体
        /// </summary>
        private Font _infoTitleFont = new Font("宋体", 12f, FontStyle.Bold);
        /// <summary>
        /// 消息文本字体
        /// </summary>
        private Font _infoTextFont = new Font("宋体", 9f);
        /// <summary>
        /// 消息文本数组
        /// </summary>
        private readonly List<string> _textList = new List<string>();
        /// <summary>
        /// 消息纹理数组
        /// </summary>
        private readonly List<Texture> _texture = new List<Texture>();
        /// <summary>
        /// 消息标题颜色
        /// </summary>
        private Color _titleColor = Color.FromArgb(235, 221, 36);
        /// <summary>
        /// 消息文本颜色
        /// </summary>
        private Color _textColor = Color.FromArgb(240, 240, 240);
        /// <summary>
        /// 透明度
        /// </summary>
        private float _pellucidity;
        /// <summary>
        /// 显示时间
        /// </summary>
        private float _showTime;
        /// <summary>
        /// Y轴偏移量
        /// </summary>
        private float _offset_y = 100f;
        /// <summary>
        /// 换行宽度
        /// </summary>
        private int _wrapWith = 50;
        /// <summary>
        /// 位置模式
        /// </summary>
        private readonly DockLocType _infoLocationType = DockLocType.User;
        /// <summary>
        /// 显示时间长度(秒)
        /// </summary>
        private float _showTimeLength = 2.5f;
        /// <summary>
        /// 纹理是否已经创建
        /// </summary>
        private bool _isTextureCreate;
        #endregion

        #region 消息构造
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="title">消息标题</param>
        /// <param name="text">消息内容</param>
        /// <param name="x">坐标x</param>
        /// <param name="y">坐标y</param>
        public EngineInfo(string title, string text, float x, float y)
        {
            _title = title;
            _text = text;
            _location = new PointF(x, y);
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="title">消息标题</param>
        /// <param name="text">消息内容</param>
        /// <param name="infoLoc">停靠位置</param>
        public EngineInfo(string title, string text, DockLocType infoLoc)
        {
            _title = title;
            _text = text;
            _infoLocationType = infoLoc;
        }

        /// <summary>
        /// 设置换行宽度
        /// </summary>
        /// <param name="width">宽度</param>
        public void SetWrapWidth(int width)
        {
            // 设置换行宽度
            if (width < 10) width = 10;
            if (width > 150) width = 150;
            _wrapWith = width;
            //// 释放资源并重新生成
            //foreach (Texture tex in this._texture)
            //{
            //    tex.Dispose();
            //}
            //this._textList = new List<string>();
            //this._texture = new List<Texture>();
            //TextSplit();
            //SetLocation();
        }

        /// <summary>
        /// 设置标题颜色
        /// </summary>
        /// <param name="color">标题颜色</param>
        public void SetTitleColor(Color color)
        {
            _titleColor = color;
        }

        /// <summary>
        /// 设置文本颜色
        /// </summary>
        /// <param name="color">颜色</param>
        public void SetTextColor(Color color)
        {
            _textColor = color;
        }

        /// <summary>
        /// 设置标题字体
        /// </summary>
        /// <param name="font">字体</param>
        public void SetTitleFont(Font font)
        {
            _infoTitleFont = font;
        }

        /// <summary>
        /// 设置文本字体
        /// </summary>
        /// <param name="font">字体</param>
        public void SetTextFont(Font font)
        {
            _infoTextFont = font;
        }

        /// <summary>
        /// 设置消息显示时间
        /// </summary>
        /// <param name="timeLength">时长(单位:秒  小于0时为一直显示)</param>
        public void SetTextShowTime(float timeLength)
        {
            _showTimeLength = timeLength;
        }

        /// <summary>
        /// 分割消息文本，并计算消息宽高度
        /// </summary>
        private void TextSplit()
        {
            int length = 0;
            while (length < _text.Length)
            {
                int lengthTemp = 0;
                int lengthIndex = 0;
                string str_temp = "";
                while (lengthTemp < _wrapWith - 2 && length + lengthIndex < _text.Length)
                {
                    // 逐字拼装直至长度满一行
                    string strSub = _text.Substring(length + lengthIndex, 1);
                    str_temp += strSub;
                    lengthTemp += Encoding.Default.GetBytes(strSub).Length;
                    lengthIndex++;
                    if (strSub == "\n") break;
                }
                length += lengthIndex;
                _textList.Add(str_temp);
            }
            // 创建纹理
            Texture textureTitle = GH.CreateStringTexture2DByGdi(_title, _titleColor, _infoTitleFont);
            _texture.Add(textureTitle);
            for (int i = 0; i < _textList.Count; i++)
            {
                Texture texture = GH.CreateStringTexture2DByGdi(_textList[i], _textColor, _infoTextFont);
                _texture.Add(texture);
            }
            // 计算宽高
            int blank_border = 10;
            int blank_line = 5;
            _width = 0;
            for (int i = 0; i < _texture.Count; i++)
            {
                if (_texture[i].Width > _width) _width = _texture[i].Width + blank_border * 2;
            }
            _height = (_texture.Count + 1) * (blank_line + _texture[1].Height) + blank_border;
        }

        /// <summary>
        /// 设置消息位置
        /// </summary>
        private void SetLocation()
        {
            int blankX = 20;
            int blankY = 20;
            switch (_infoLocationType)
            {
                case DockLocType.User:
                    break;
                case DockLocType.Center:
                    _location.X = (General.Draw_Rect.Width * 1f - _width) / 2;
                    _location.Y = (General.Draw_Rect.Height * 1f - _height) / 2;
                    break;
                case DockLocType.TopLeftCorner:
                    _location.X = blankX;
                    _location.Y = blankY;
                    break;
                case DockLocType.TopRightCorner:
                    _location.X = General.Draw_Rect.Width - _width - blankX;
                    _location.Y = blankY;
                    break;
                case DockLocType.LowerLeftCorner:
                    _location.X = blankX;
                    _location.Y = General.Draw_Rect.Height - Height - blankY;
                    break;
                case DockLocType.LowerRightCorner:
                    _location.X = General.Draw_Rect.Width - _width - blankX;
                    _location.Y = General.Draw_Rect.Height - Height - blankY;
                    break;
            }
        } 
        #endregion

        #region 消息绘制
        /// <summary>
        /// 绘制纹理
        /// </summary>
        public void DrawInfo()
        {
            // 如过没有创建纹理则创建
            if (!_isTextureCreate)
            {
                TextSplit();
                SetLocation();
                _isTextureCreate = true;
            }

            // 暂停性能计数器
            PerformanceAnalyzer.StopPerformanceCount();

            // 动画效果
            float deltaTime = Time.DeltaTimeUnScale / General.Engine_Fps_Def;
            _showTime += deltaTime;
            // 淡入
            if (_showTime < 0.6f)
            {
                _pellucidity += 255f * deltaTime / 6f * 10f;
                _offset_y -= 100f * deltaTime / 6f * 10f;
                if (_pellucidity > 255f) _pellucidity = 255;
            }
            // 淡出
            if (_showTimeLength > 0)
            {
                if (_showTime > 0.6f + _showTimeLength)
                {
                    _pellucidity -= 255f * deltaTime * 2;
                    _offset_y -= 100f * deltaTime * 2;
                    // 销毁
                    if (_pellucidity < 0)
                    {
                        InfoPublisher.RemoveEngineInfo(this);
                        Dispose();
                        return;
                    }
                }
            }

            // 矩形偏移
            RectangleF rect = new RectangleF(DrawRect.X, DrawRect.Y + _offset_y, DrawRect.Width, DrawRect.Height);
            // 透明度渐变
            int pTL = (int)(_pellucidity / 255f * 140f);
            int pTR = (int)(_pellucidity / 255f * 160f);
            int pLL = (int)(_pellucidity / 255f * 200f);
            int pLR = (int)(_pellucidity / 255f * 200f);
            int pBox = (int)(_pellucidity / 255f * 230f);
            // 绘制消息框
            GH.FillRectangleGradient(
                Color.FromArgb(pTL, Color.FromArgb(100, 100, 100)),
                Color.FromArgb(pTR, Color.FromArgb(100, 100, 100)),
                Color.FromArgb(pLL, Color.Black),
                Color.FromArgb(pLR, Color.Black),
                rect);
            GH.SetLineWitth(1f);
            GH.DrawRectangle(Color.FromArgb(pBox, Color.Black), rect);
            // 绘制文本
            float x = _location.X + 10, y = _location.Y + 15;
            for (int i = 0; i < _texture.Count; i++)
            {
                GH.DrawImage(_texture[i].TextureID, (int)x, (int)(y + _offset_y), _texture[i].Width, _texture[i].Height, (int)_pellucidity);
                if (i == 0) y += _texture[i].Height + 10;
                else y += _texture[i].Height + 5;
            }

            // 恢复性能计数器
            PerformanceAnalyzer.StartPerformanceCount();
        } 
        #endregion

        #region 销毁
        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            foreach (Texture tex in _texture)
            {
                tex.Dispose();
            }
        } 
        #endregion
    }
}
