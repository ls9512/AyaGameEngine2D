using System;
using System.Windows.Forms;

namespace AyaGameEngine2D.Core
{
    /// <summary>
    /// 类      名：GameLoop
    /// 功      能：游戏主循环
    ///             每一帧进行指定函数回调，并传递参数：每一帧间隔 deltaTime
    /// 日      期：2015-11-17
    /// 修      改：2016-01-03
    /// 作      者：ls9512
    /// </summary>
    internal class GameLoop
    {
        #region 静态实例
        /// <summary>
        /// 静态实例
        /// </summary>
        internal static GameLoop Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameLoop();
                return _instance;
            }
        }
        private static GameLoop _instance;
        #endregion

        #region 回调委托
        /// <summary>
        /// 主循环回调事件委托 
        /// </summary>
        /// <param name="deltaTime">间隔时间</param>
        public delegate void LoopCallBackEventHandler(float deltaTime);

        /// <summary>
        /// 主循环初始化委托
        /// </summary>
        public delegate void LoopInitEventHandler();

        /// <summary>
        /// 主循环启动委托
        /// </summary>
        public delegate void LoopStartEventHandler();
        #endregion

        #region 公有字段
        /// <summary>
        /// 主循环初始化
        /// </summary>
        internal static event LoopInitEventHandler OnLoopInit;
        /// <summary>
        /// 主循环启动
        /// </summary>
        internal static event LoopStartEventHandler OnLoopStart; 
        #endregion

        #region 私有字段
        /// <summary>
        /// 主循环回调
        /// </summary>
        private LoopCallBackEventHandler _callBack;
        
        /// <summary>
        /// 循环标识
        /// </summary>
        private bool _isRunning; 
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        protected GameLoop()
        {
            // 初始化
            if (OnLoopInit != null) OnLoopInit();
            // 注册程序空闲回调
            Application.Idle += OnApplicationEnterIdle;
        } 
        #endregion

        #region 循环启动和中断
        /// <summary>
        /// 开始循环
        /// </summary>
        public void StartLoop()
        {
            if (OnLoopStart != null) OnLoopStart();
            _isRunning = true;
        }

        /// <summary>
        /// 暂停循环
        /// </summary>
        public void PauseLoop()
        {
            _isRunning = false;
        } 
        #endregion

        #region 公有方法
        /// <summary>
        /// 设置回调函数
        /// </summary>
        /// <param name="callBack">回调函数</param>
        public void SetCallBack(LoopCallBackEventHandler callBack)
        {
            _callBack = callBack;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 当进入空闲时发生
        /// </summary>
        /// <param name="sender">参数1</param>
        /// <param name="e">参数2</param>
        private void OnApplicationEnterIdle(object sender, EventArgs e)
        {
            while (IsAppStillIdle() && _isRunning && _callBack != null)
            {
                _callBack(PreciseTimer.GetElapsedTime());
            }
        }

        /// <summary>
        /// 判断程序是否处于空闲状态
        /// </summary>
        /// <returns></returns>
        private bool IsAppStillIdle()
        {
            WinMessage msg;
            return !Win32.PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
        } 
        #endregion
    }
}
