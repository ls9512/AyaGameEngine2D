using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AyaGameEngine2D.Controls;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：KeyManager
    /// 功      能：键盘管理类，对按键状态统一管理
    /// 日      期：2015-11-21
    /// 修      改：2015-11-21
    /// 作      者：ls9512
    /// </summary>
    public class KeyManager
    {
        #region 静态实例
        /// <summary>
        /// KeyManager静态实例
        /// </summary>
        public static KeyManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new KeyManager();
                return _instance;
            }
        }
        private static KeyManager _instance;
        #endregion

        #region 公有成员
        /// <summary>
        /// 按键按下事件
        /// </summary>
        internal KeyPressEventHandler KeyPressEvent = null; 
        #endregion

        #region 私有成员
        /// <summary>
        /// 按键状态字典
        /// </summary>
        private readonly Dictionary<Keys, KeyState> _keyStatesList = new Dictionary<Keys, KeyState>(); 
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        protected KeyManager()
        {
            // 绑定控件的按键事件
            OpenGLPanel.Instance.KeyDown += OnKeyDown;
            OpenGLPanel.Instance.KeyUp += OnKeyUp;
            OpenGLPanel.Instance.KeyPress += OnKeyPress;
        } 
        #endregion

        #region 按键处理
        /// <summary>
        /// 按键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressEvent != null)
            {
                KeyPressEvent(sender, e);
            }
        }

        /// <summary>
        /// 按键抬起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            EnsureKeyStateExists(e.KeyCode);
            _keyStatesList[e.KeyCode].OnUp();
        }

        /// <summary>
        /// 按键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            EnsureKeyStateExists(e.KeyCode);
            _keyStatesList[e.KeyCode].OnDown();
        }

        /// <summary>
        /// 确认按键状态
        /// </summary>
        /// <param name="key"></param>
        private void EnsureKeyStateExists(Keys key)
        {
            if (!_keyStatesList.ContainsKey(key))
            {
                // 不存在则存入新按键和对应的状态
                _keyStatesList.Add(key, new KeyState());
            }
        }
        #endregion

        #region 按键查询
        /// <summary>
        /// 查询某键是否按下
        /// </summary>
        /// <param name="key"></param>
        /// <returns>查询结果</returns>
        public bool IsKeyPressed(Keys key)
        {
            EnsureKeyStateExists(key);
            return _keyStatesList[key].Pressed;
        }

        /// <summary>
        /// 查询按键是否按住
        /// </summary>
        /// <param name="key"></param>
        /// <returns>查询结果</returns>
        public bool IsKeyHeld(Keys key)
        {
            EnsureKeyStateExists(key);
            return _keyStatesList[key].Held;
        }

        /// <summary>
        /// 查询某键是否未按下
        /// </summary>
        /// <param name="key"></param>
        /// <returns>查询结果</returns>
        public bool IsKeyUp(Keys key)
        {
            EnsureKeyStateExists(key);
            // 未按下且未按住则为Up
            return !_keyStatesList[key].Pressed && !_keyStatesList[key].Held;
        }

        /// <summary>
        /// 查询一组按键是否同时按下
        /// </summary>
        /// <param name="key1">按键1</param>
        /// <param name="key2">按键2</param>
        /// <returns>查询结果</returns>
        public bool IsKeysPressed(Keys key1, Keys key2)
        {
            EnsureKeyStateExists(key1);
            EnsureKeyStateExists(key2);
            return _keyStatesList[key1].Pressed && _keyStatesList[key2].Pressed;
        }

        /// <summary>
        /// 查询一组按键是否按下
        /// </summary>
        /// <param name="keys">按键列表</param>
        /// <returns>查询结果</returns>
        public bool IsKeysPressed(List<Keys> keys)
        {
            bool result = true;
            foreach (Keys key in keys)
            {
                EnsureKeyStateExists(key);
                if (!_keyStatesList[key].Pressed)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 查询一组按键是否同时按住
        /// </summary>
        /// <param name="key1">按键1</param>
        /// <param name="key2">按键2</param>
        /// <returns>查询结果</returns>
        public bool IsKeysHeld(Keys key1, Keys key2)
        {
            EnsureKeyStateExists(key1);
            EnsureKeyStateExists(key2);
            return _keyStatesList[key1].Held && _keyStatesList[key2].Held;
        }

        /// <summary>
        /// 查询一组按键是否按住
        /// </summary>
        /// <param name="keys">按键列表</param>
        /// <returns>查询结果</returns>
        public bool IsKeysHeld(List<Keys> keys)
        {
            bool result = true;
            foreach (Keys key in keys)
            {
                EnsureKeyStateExists(key);
                if (!_keyStatesList[key].Held)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 查询是否按下任意键
        /// </summary>
        /// <returns>查询结果</returns>
        public bool IsAnyKey()
        {
            bool result = false;
            foreach (KeyState state in _keyStatesList.Values)
            {
                if (state.Pressed)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 强制释放按键，消除某些连续按键识别错误的问题
        /// </summary>
        /// <param name="key">键值</param>
        public void Release(Keys key)
        {
            EnsureKeyStateExists(key);
            _keyStatesList[key].Pressed = false;
            _keyStatesList[key].Held = false;
        }
        #endregion

        #region 每一帧对按键的处理和更新
        /// <summary>
        /// 更新键盘状态 - 每一帧调用
        /// </summary>
        /// <param name="deltaTime"></param>
        internal void Update(float deltaTime)
        {
            ProcessControlKeys();
            foreach (KeyState state in _keyStatesList.Values)
            {
                state.Pressed = false;
                state.Process();
            }
        }

        /// <summary>
        /// 查询按键是否按下
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool PollKeyPress(Keys key)
        {
            return Win32.GetAsyncKeyState((int)key) != 0;
        }

        /// <summary>
        /// 处理控制按键
        /// </summary>
        private void ProcessControlKeys()
        {
            UpdateControlKey(Keys.Left);
            UpdateControlKey(Keys.Right);
            UpdateControlKey(Keys.Up);
            UpdateControlKey(Keys.Down);
            UpdateControlKey(Keys.LMenu); // 左Alt
            UpdateControlKey(Keys.RMenu); // 右Alt
        }

        /// <summary>
        /// 更新控件按键状态
        /// </summary>
        /// <param name="key"></param>
        private void UpdateControlKey(Keys key)
        {
            if (PollKeyPress(key))
            {
                OnKeyDown(OpenGLPanel.Instance, new KeyEventArgs(key));
            }
            else
            {
                OnKeyUp(OpenGLPanel.Instance, new KeyEventArgs(key));
            }
        } 
        #endregion
    }

    #region 按键状态
    /// <summary>
    /// 类      名：KeyState
    /// 功      能：按键状态类，用于存储按和更新键的各种状态
    /// 日      期：2015-11-21
    /// 修      改：2015-11-21
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class KeyState
    {
        /// <summary>
        /// 按下状态检查
        /// </summary>
        bool _keyPressDetected;

        /// <summary>
        /// 是否按住
        /// </summary>
        public bool Held
        {
            get { return _held; }
            set { _held = value; }
        }
        private bool _held;

        /// <summary>
        /// 是否按下
        /// </summary>
        public bool Pressed
        {
            get { return _pressed; }
            set { _pressed = value; }
        }
        private bool _pressed;

        /// <summary>
        /// 构造方法
        /// </summary>
        public KeyState()
        {
            Held = false;
            Pressed = false;
        }

        /// <summary>
        /// 按下
        /// </summary>
        internal void OnDown()
        {
            if (Held == false)
            {
                _keyPressDetected = true;
            }
            _held = true;
        }

        /// <summary>
        /// 抬起
        /// </summary>
        internal void OnUp()
        {
            _held = false;
        }

        /// <summary>
        /// 按键处理
        /// </summary>
        internal void Process()
        {
            _pressed = false;
            if (_keyPressDetected)
            {
                _pressed = true;
                _keyPressDetected = false;
            }
        }
    } 
    #endregion
}

// ★ 以下是原版KeyManager 代码的备份参考

/*
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：KeyEventType
    /// 功      能：键盘事件类型枚举
    /// 日      期：2015-03-23
    /// 修      改：2015-03-23
    /// 作      者：ls9512
    /// </summary>
    public enum KeyEventType
    {
        Down,
        Up,
        Press,
    }

    /// <summary>
    /// 类      名：KeyEvent
    /// 功      能：键盘事件类，对键盘事件的各项属性封装
    /// 日      期：2015-03-23
    /// 修      改：2015-03-23
    /// 作      者：ls9512
    /// </summary>
    public class KeyEvent
    {
        /// <summary>
        /// 键盘事件类型
        /// </summary>
        public KeyEventType KeyEventType
        {
            get { return this._keyEventType; }
            set { this._keyEventType = value; }
        }
        private KeyEventType _keyEventType;

        /// <summary>
        /// 按键值
        /// </summary>
        public Keys KeyCode
        {
            get { return this._keyCode; }
            set { this._keyCode = value; }
        }
        private Keys _keyCode;

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
    /// 类      名：KeyManager
    /// 功      能：键盘管理类，对按键状态统一管理
    /// 日      期：2015-03-21
    /// 修      改：2015-03-21
    /// 作      者：ls9512
    /// </summary>
    public class KeyManager
    {
        /// <summary>
        /// 按键集合
        /// </summary>
        private List<Keys> _keyEventList = new List<Keys>();

        /// <summary>
        /// 构造方法
        /// </summary>
        public KeyBoard()
        {
            this._keyEventList.Clear();
        }

        /// <summary>
        /// 键盘管理静态实例
        /// </summary>
        public static KeyManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new KeyManager();
                return _instance;
            }
        }
        private static KeyManager _instance;

        /// <summary>
        /// 是否按下，检测后立刻释放
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>查询结果</returns>
        public bool IsKeyDown(Keys key)
        {
            bool isKeyDown = false;
            int time = Environment.TickCount;
            foreach (Keys k in this._keyEventList)
            {
                if (k == key)
                {
                    isKeyDown = true;
                    break;
                }
            }
            // 检测后立刻释放该按键
            ReleaseKey(key);
            return isKeyDown;
        }

        /// <summary>
        /// 是否持续按下，需要手动释放
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyDownRepeat(Keys key)
        {
            bool isKeyDown = false;
            int time = Environment.TickCount;
            foreach (Keys k in this._keyEventList)
            {
                if (k == key)
                {
                    isKeyDown = true;
                    break;
                }
            }
            return isKeyDown;
        }

        /// <summary>
        /// 按键是否未按下
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>查询结果</returns>
        public bool IsKeyUp(Keys key)
        {
            bool isKeyUp = true;
            int time = Environment.TickCount;
            foreach (Keys k in this._keyEventList)
            {
                if (k == key)
                {
                    isKeyUp = false;
                    break;
                }
            }
            return isKeyUp;
        }

        /// <summary>
        /// 释放按键
        /// </summary>
        /// <param name="key"></param>
        public void ReleaseKey(Keys key)
        {
            this._keyEventList.Remove(key);
        }

        /// <summary>
        /// 按压键盘的键
        /// </summary>
        /// <param name="key"></param>
        public void KeyDown(Keys key)
        {
            if (!this._keyEventList.Contains(key))
            {
                this._keyEventList.Add(key);
                //RemoveOldEvent();
            }
        }

        /// <summary>
        /// 释放按压的键
        /// </summary>
        /// <param name="key"></param>
        public void KeyUp(Keys key)
        {
            this._keyEventList.Remove(key);
        }

        /// <summary>
        /// 统计
        /// </summary>
        /// <returns></returns>
        public int CountEventNum()
        {
            return this._keyEventList.Count;
        }

    }
}

*/
