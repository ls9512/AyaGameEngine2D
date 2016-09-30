using System;

using System.Drawing;

using AyaGameEngine2D.Core;

namespace AyaGameEngine2D
{
    #region UI触发状态枚举
    /// <summary>
    /// 类      名：UIStatus
    /// 功      能：UI状态枚举
    /// 日      期：2015-03-21
    /// 修      改：2015-12-27
    /// 作      者：ls9512
    /// </summary>
    public enum UIStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 鼠标焦点
        /// </summary>
        MouseOn,
        /// <summary>
        /// 鼠标按下
        /// </summary>
        MouseDown,
        /// <summary>
        /// 鼠标单击
        /// </summary>
        MouseClick,
        /// <summary>
        /// 鼠标抬起
        /// </summary>
        MouseUp
    } 
    #endregion

    #region UI加载模式枚举
    /// <summary>
    /// 类      名：UILoadMode
    /// 功      能：UI加载模式枚举
    /// 日      期：2015-03-21
    /// 修      改：2015-12-27
    /// 作      者：ls9512
    /// </summary>
    public enum UILoadMode
    {
        /// <summary>
        /// 正常,直接出现
        /// </summary>
        Normal,
        /// <summary>
        /// 淡入淡出
        /// </summary>
        Slide,
    } 
    #endregion

    /// <summary>
    /// 类      名：BaseUI
    /// 功      能：游戏UI基类
    /// 日      期：2015-11-12
    /// 修      改：2016-01-19
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public abstract class BaseUI
    {
        #region 公有成员
        /// <summary>
        /// 坐标X
        /// </summary>
        public virtual float X
        {
            get { return _x; }
            set { _x = value; }
        }
        private float _x;

        /// <summary>
        /// 坐标Y
        /// </summary>
        public virtual float Y
        {
            get { return _y; }
            set { _y = value; }
        }
        private float _y;

        /// <summary>
        /// 宽度
        /// </summary>
        public virtual int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        private int _width;

        /// <summary>
        /// 高度
        /// </summary>
        public virtual int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        private int _height;

        /// <summary>
        /// 可用标识
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }
        private bool _enable;

        /// <summary>
        /// 上次鼠标按下时间
        /// </summary>
        private long _lastMouseDonwTime = 0;
        /// <summary>
        /// 上次鼠标抬起时间
        /// </summary>
        private long _lastMouseUpTime = 0;

        /// <summary>
        /// UI状态
        /// </summary>
        public UIStatus UIStatus
        {
            get
            {
                if (IsMouseDown)
                {
                    _uiStatus = UIStatus.MouseDown;
                    _lastMouseDonwTime = GameTimer.DurationMillisecond;
                }
                else if (_lastMouseUpTime - _lastMouseDonwTime < 500 && GameTimer.DurationMillisecond - _lastMouseUpTime < 500)
                {
                    _uiStatus = UIStatus.MouseClick;
                }
                else if (MouseManager.Instance.LeftUp && IsMouseOn)
                {
                    _uiStatus = UIStatus.MouseUp;
                    _lastMouseUpTime = GameTimer.DurationMillisecond;
                }
                else if (IsMouseOn)
                {
                    _uiStatus = UIStatus.MouseOn;
                }
                else
                {
                    _uiStatus = UIStatus.Normal;
                }
                return _uiStatus;
            }
            set
            {
                _uiStatus = value;
            }
        }

        /// <summary>
        /// 设置点击事件结束
        /// </summary>
        public void SetClickOver()
        {
            _lastMouseUpTime = 0;
            _lastMouseDonwTime = 0;
        }
        private UIStatus _uiStatus;

        /// <summary>
        /// UI加载模式
        /// </summary>
        public UILoadMode UILoadMode
        {
            get { return _uiLoadMode; }
            set { _uiLoadMode = value; }
        }
        private UILoadMode _uiLoadMode;

        /// <summary>
        /// 贴图纹理
        /// </summary>
        public Texture Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                Width = _texture.Width;
                Height = _texture.Height;
            }
        }
        private Texture _texture;

        /// <summary>
        /// UI显示所在区域(计算窗口缩放)
        /// </summary>
        public RectangleF UIRect
        {
            get
            {
                float xx = General.Win_Rect.Width * 1f / General.Draw_Rect.Width;
                float yy = General.Win_Rect.Height * 1f / General.Draw_Rect.Height;
                return new RectangleF(_x * xx, _y * yy, _width * xx, _height * yy);
            }
        }

        /// <summary>
        /// UI绘制区域
        /// </summary>
        public RectangleF UIDrawRect
        {
            get { return new RectangleF(_x, _y, _width, _height); }
        }

        /// <summary>
        /// 鼠标是否在区域内
        /// </summary>
        public bool IsMouseOn
        {
            get { return Input.IsMouseOn(UIRect); }
        }

        /// <summary>
        /// 鼠标是否按下(UI区域内)
        /// </summary>
        public bool IsMouseDown
        {
            get { return Input.IsMouseLeftHeld && IsMouseOn; }
        }

        /// <summary>
        /// 是否显示UI
        /// </summary>
        public virtual bool IsShowUI
        {
            get { return true; }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        protected BaseUI()
        {
            _uiLoadMode = UILoadMode.Normal;
            _enable = true;
        }
        #endregion

        #region 逻辑
        /// <summary>
        /// 物件逻辑，必须被重写
        /// </summary>
        public abstract void UILogic();
        #endregion

        #region 绘图
        /// <summary>
        /// 物件绘图，必须被重写
        /// </summary>
        public abstract void UIGraphic(); 
        #endregion
    }
}
