using System.Drawing;
using System.Windows.Forms;

using AyaGameEngine2D.Controls;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：MouseManager
    /// 功      能：鼠标事件管理类，对鼠标事件进行统一的获取、处理和反馈
    /// 日      期：2015-11-22
    /// 修      改：2015-11-23
    /// 作      者：ls9512
    /// </summary>
    public class MouseManager
    {
        #region 静态实例
        /// <summary>
        /// 鼠标事件管理类静态实例
        /// </summary>
        public static MouseManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MouseManager();
                return _instance;
            }
        }
        private static MouseManager _instance;
        #endregion

        #region 按键状态和位置
        /// <summary>
        /// 鼠标位置
        /// </summary>
        public Point Position
        {
            get { return _position; }
        }
        private Point _position;

        /// <summary>
        /// 当前鼠标绘制位置(计算窗口缩放)
        /// </summary>
        public Point MouseDrawPosition
        {
            get
            {
                float xx = General.Draw_Rect.Width * 1f / General.Win_Rect.Width;
                float yy = General.Draw_Rect.Height * 1f / General.Win_Rect.Height;
                return new Point((int)(_position.X * xx), (int)(_position.Y * yy));
            }
        }

        /// <summary>
        /// 鼠标左键状态检查
        /// </summary>
        private bool _leftClickDetect;

        /// <summary>
        /// 鼠标右键状态检查
        /// </summary>
        private bool _rightClickDetect;

        /// <summary>
        /// 鼠标中键状态检查
        /// </summary>
        private bool _middleClickDetect;

        /// <summary>
        /// 鼠标左键按下
        /// </summary>
        public bool LeftPressed
        {
            get { return _leftPressed; }
        }
        private bool _leftPressed;

        /// <summary>
        /// 鼠标右键按下
        /// </summary>
        public bool RightPressed
        {
            get { return _rightPressed; }
        }
        private bool _rightPressed;

        /// <summary>
        /// 鼠标中键按下
        /// </summary>
        public bool MiddlePressed
        {
            get { return _middlePressed; }
        }
        private bool _middlePressed;

        /// <summary>
        /// 鼠标左键按住
        /// </summary>
        public bool LeftHeld
        {
            get { return _leftHeld; }
        }
        private bool _leftHeld;

        /// <summary>
        /// 鼠标右键按住
        /// </summary>
        public bool RightHeld
        {
            get { return _rightHeld; }
        }
        private bool _rightHeld;

        /// <summary>
        /// 鼠标中键按住
        /// </summary>
        public bool MiddleHeld
        {
            get { return _middleHeld; }
        }
        private bool _middleHeld;

        /// <summary>
        /// 鼠标左键抬起
        /// </summary>
        public bool LeftUp
        {
            get { return !_leftHeld && !_leftPressed; }
        }

        /// <summary>
        /// 鼠标右键抬起
        /// </summary>
        public bool RightUp
        {
            get { return !_rightHeld && !_rightPressed; }
        }

        /// <summary>
        /// 鼠标中键抬起
        /// </summary>
        public bool MiddleUp
        {
            get { return !_middleHeld && !_middlePressed; }
        }
        #endregion

        #region 构造和事件绑定
        /// <summary>
        /// 构造方法
        /// </summary>
        protected MouseManager()
        {
            // 添加鼠标移动委托
            OpenGLPanel.Instance.MouseMove += delegate(object ebj, MouseEventArgs e)
            {
                _position.X = e.X;
                _position.Y = e.Y;
            };

            // 添加鼠标单击委托
            OpenGLPanel.Instance.MouseClick += delegate(object ebj, MouseEventArgs e)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        _leftClickDetect = true;
                        break;
                    case MouseButtons.Middle:
                        _middleClickDetect = true;
                        break;
                    case MouseButtons.Right:
                        _rightClickDetect = true;
                        break;
                }
            };

            // 添加鼠标按下委托
            OpenGLPanel.Instance.MouseDown += delegate(object ebj, MouseEventArgs e)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        _leftHeld = true;
                        break;
                    case MouseButtons.Middle:
                        _middleHeld = true;
                        break;
                    case MouseButtons.Right:
                        _rightHeld = true;
                        break;
                }
            };

            // 添加鼠标抬起委托
            OpenGLPanel.Instance.MouseUp += delegate(object ebj, MouseEventArgs e)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        this._leftHeld = false;
                        break;
                    case MouseButtons.Middle:
                        this._middleHeld = false;
                        break;
                    case MouseButtons.Right:
                        this._rightHeld = false;
                        break;
                }
            };

            // 添加鼠标离开委托
            OpenGLPanel.Instance.MouseLeave += delegate
            {
                _leftHeld = false;
                _middleHeld = false;
                _rightHeld = false;
            };
        }
        #endregion

        #region 鼠标状态更新
        /// <summary>
        /// 更新状态 - 每一帧调用
        /// </summary>
        /// <param name="deltaTime">间隔时间</param>
        internal void Update(float deltaTime)
        {
            UpdateMouseButtons();
        }

        /// <summary>
        /// 更新鼠标按键
        /// </summary>
        private void UpdateMouseButtons()
        {
            _leftPressed = false;
            _rightPressed = false;
            _middlePressed = false;

            if (_leftClickDetect)
            {
                _leftPressed = true;
                _leftClickDetect = false;
            }

            if (_rightClickDetect)
            {
                _rightPressed = true;
                _rightClickDetect = false;
            }

            if (_middleClickDetect)
            {
                _middlePressed = true;
                _middleClickDetect = false;
            }
        }
        #endregion
    }
}

// ★ 以下是原版MouseManager 代码的备份参考

/*
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：MouseEventType
    /// 功      能：鼠标事件类型枚举
    /// 日      期：2015-03-21
    /// 修      改：2015-03-21
    /// 作      者：ls9512
    /// </summary>
    public enum MouseEventType
    {
        Down,           //按下
        UP,             //抬起
        Hover,          //停留
        Move            //移动
    }

    /// <summary>
    /// 类      名：MouseEvent
    /// 功      能：鼠标事件类，对鼠标事件的各项属性封装
    /// 日      期：2015-03-21
    /// 修      改：2015-06-25
    /// 作      者：ls9512
    /// </summary>
    public class MouseEvent
    {
        /// <summary>
        /// 鼠标事件类型
        /// </summary>
        public MouseEventType MouseEventType
        {
            get { return this._mouseEventType; }
            set { this._mouseEventType = value; }
        }
        private MouseEventType _mouseEventType;

        /// <summary>
        /// 鼠标按键
        /// </summary>
        public MouseButtons MouseButtons
        {
            get { return this._mouseButtons; }
            set { this._mouseButtons = value; }
        }
        private MouseButtons _mouseButtons;

        /// <summary>
        /// 鼠标坐标X
        /// </summary>
        public int MouseX
        {
            get { return this._mouseX; }
            set { this._mouseX = value; }
        }
        private int _mouseX;

        /// <summary>
        /// 鼠标坐标Y
        /// </summary>
        public int MouseY
        {
            get { return this._mouseY; }
            set { this._mouseY = value; }
        }
        private int _mouseY;

        /// <summary>
        /// 发生时间
        /// </summary>
        public int Time
        {
            get { return this._time; }
            set { this._time = value; }
        }
        private int _time;
    }

    /// <summary>
    /// 类      名：MouseManager
    /// 功      能：鼠标事件管理类，对鼠标事件进行统一的获取、处理和反馈
    /// 日      期：2015-03-21
    /// 修      改：2015-03-21
    /// 作      者：ls9512
    /// </summary>
    public class MouseManager
    {
        /// <summary>
        /// 最后一次鼠标事件发生时间
        /// ==========
        /// 用于判断鼠标悬停
        /// </summary>
        public long LastMouseEventTime { get; private set; }

        /// <summary>
        /// 上一次鼠标事件
        /// </summary>
        public MouseEvent LastMouseEvent;

        /// <summary>
        /// 鼠标当前位置
        /// </summary>
        public Point NowMouseLocation;

        /// <summary>
        /// 当前鼠标绘制位置(计算窗口缩放)
        /// </summary>
        public Point NowMouseDrawLocation
        {
            get
            {
                float xx = General.Draw_Rect.Width * 1f / General.Win_Rect.Width;
                float yy = General.Draw_Rect.Height * 1f / General.Win_Rect.Height;
                return new Point((int)(NowMouseLocation.X * xx), (int)(NowMouseLocation.Y * yy));
            }
        }

        /// <summary>
        /// 鼠标悬停
        /// </summary>
        public bool IsMouseHover
        {
            get
            {
                if (GameTimer.DurationMillisecond - this.LastMouseEventTime > 1000) return true;
                else return false;
            }
        }

        /// <summary>
        /// 鼠标事件集合
        /// </summary>
        private List<MouseEvent> _mouseEventList = new List<MouseEvent>();

        /// <summary>
        /// 构造方法
        /// </summary>
        public MouseManager()
        {
            _mouseEventList.Clear();
            LastMouseEvent = new MouseEvent();
            LastMouseEvent.MouseEventType = MouseEventType.Move;
            NowMouseLocation = new Point();
        }

        /// <summary>
        /// 鼠标事件管理类静态实例
        /// </summary>
        public static MouseManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MouseManager();
                return _instance;
            }
        }
        private static MouseManager _instance;

        /// <summary>
        /// 判断按键是否按下
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsMouseDown(MouseButtons button)
        {
            if (LastMouseEvent.MouseButtons == button && LastMouseEvent.MouseEventType == MouseEventType.Down)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 压入鼠标事件
        /// </summary>
        /// <param name="key"></param>
        public void AddMouseEvent(MouseEvent e)
        {
            if (!_mouseEventList.Contains(e))
            {
                //记录最近鼠标操作时间
                LastMouseEventTime = Environment.TickCount;
                LastMouseEvent = e;
                NowMouseLocation.X = e.MouseX;
                NowMouseLocation.Y = e.MouseY;
                this._mouseEventList.Add(e);
            }
            RemoveOldEvent();
        }

        /// <summary>
        /// 释放按压的键
        /// </summary>
        /// <param name="key"></param>
        public MouseEvent RemoveOldEvent()
        {
            // 清除过旧的鼠标事件,保留最新的1次
            while (_mouseEventList.Count > 1)
            {
                this._mouseEventList.Remove(this._mouseEventList[0]);
            }
            return null;
        }

        /// <summary>
        /// 清空事件列表
        /// </summary>
        public void ClearEvent()
        {
            this._mouseEventList.Clear();
        }

        /// <summary>
        /// 统计
        /// </summary>
        /// <returns>鼠标事件数量</returns>
        public int CountEventNum()
        {
            return this._mouseEventList.Count;
        }
    }
}
*/
