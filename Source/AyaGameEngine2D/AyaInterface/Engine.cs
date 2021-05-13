using AyaGameEngine2D.Core;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：Engine
    /// 功      能：引擎静态类
    ///             用于提供一些常用的引擎方法以供快速调用
    /// 日      期：2015-12-19
    /// 修      改：2015-12-30
    /// 作      者：ls9512
    /// </summary>
    public static class Engine
    {
        #region 内部委托和事件
        /// <summary>
        /// 引擎初始化委托
        /// </summary>
        internal delegate void EngineInitEventHandler();

        /// <summary>
        /// 引擎启动委托
        /// </summary>
        internal delegate void EngineStartEventHandler();

        /// <summary>
        /// 引擎关闭委托
        /// </summary>
        internal delegate void EngineCloseEventHandler();

        /// <summary>
        /// 设置游戏窗口尺寸委托
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// </summary>
        internal delegate void EngineSetGameWinRectEventHandler(int width,int height);

        /// <summary>
        /// 设置游戏绘制尺寸委托
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// </summary>
        internal delegate void EngineSetGameDrawRectEventHandler(int width,int height);

        /// <summary>
        /// 设置引擎渲染帧数委托
        /// </summary>
        /// <param name="fps">帧数</param>
        internal delegate void EngineSetRenderFpsEventHandler(float fps);


        /// <summary>
        /// 引擎初始化事件
        /// </summary>
        internal static event EngineInitEventHandler OnEngineInit;

        /// <summary>
        /// 引擎启动事件
        /// </summary>
        internal static event EngineStartEventHandler OnEngineStart;

        /// <summary>
        /// 引擎关闭事件
        /// </summary>
        internal static event EngineCloseEventHandler OnEngineClose;

        /// <summary>
        /// 引擎渲染帧数事件
        /// </summary>
        internal static event EngineSetRenderFpsEventHandler OnSetRenderFps;

        /// <summary>
        /// 设置游戏窗口尺寸事件
        /// </summary>
        internal static event EngineSetGameWinRectEventHandler OnSetGameWinRect;

        /// <summary>
        /// 设置游戏绘制尺寸事件
        /// </summary>
        internal static event EngineSetGameDrawRectEventHandler OnSetGameDrawRect;
        #endregion

        #region 引擎初始化/启动/关闭
        /// <summary>
        /// 引擎初始化
        /// </summary>
        public static void EngineInit()
        {
            if (OnEngineInit != null)
            {
                OnEngineInit();
            }
        }

        /// <summary>
        /// 引擎启动
        /// (确保数据初始化已经完成)
        /// </summary>
        public static void EngineStart()
        {
            if (OnEngineStart != null)
            {
                OnEngineStart();
            }
        }

        /// <summary>
        /// 引擎关闭
        /// (窗体关闭时会自动调用)
        /// </summary>
        public static void EngineClose()
        {
            if (OnEngineClose != null)
            {
                OnEngineClose();
            }
        } 
        #endregion

        #region 引擎参数设置
        /// <summary>
        /// 设置游戏窗体尺寸
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <remarks>在引擎初始化前设置</remarks>
        public static void SetGameWinRect(int width, int height)
        {
            if (General.Engine_IsInit) return;
            if (OnSetGameWinRect != null)
            {
                OnSetGameWinRect(width, height);
            }
        }

        /// <summary>
        /// 设置游戏绘制尺寸
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <remarks>在引擎初始化前设置</remarks>
        public static void SetGameDrawRect(int width, int height)
        {
            if (General.Engine_IsInit) return;
            if (OnSetGameDrawRect != null)
            {
                OnSetGameDrawRect(width, height);
            }
        }

        /// <summary>
        /// 设置游戏窗体和绘制尺寸
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <remarks>在引擎初始化前设置</remarks>
        public static void SetGameRect(int width, int height)
        {
            if (General.Engine_IsInit) return;
            if (OnSetGameDrawRect != null && OnSetGameWinRect != null)
            {
                OnSetGameWinRect(width, height);
                OnSetGameDrawRect(width, height);
            }
        }

        /// <summary>
        /// 设置渲染帧速率
        /// </summary>
        /// <param name="fps">帧数(1-120)</param>
        public static void SetRenderFps(float fps)
        {
            if (OnSetRenderFps != null)
            {
                OnSetRenderFps(fps);
            }
        } 
        #endregion

        #region 引擎断点
        /// <summary>
        /// 开启引擎(用于暂停恢复)
        /// </summary>
        public static void StartEngine()
        {
            GameLoop.Instance.StartLoop();
        }

        /// <summary>
        /// 暂停引擎
        /// </summary>
        public static void PauseEngine()
        {
            GameLoop.Instance.PauseLoop();
        } 
        #endregion

        #region 游戏暂停/恢复/调速
        /// <summary>
        /// 开始(或恢复)游戏
        /// 说明：通过timeScale缩放时间，会恢复与时间有关的运动
        /// </summary>
        public static void StartGame()
        {
            Time.TimeScale = 1;
        }

        /// <summary>
        /// 暂停游戏
        /// 说明：通过timeScale缩放时间，会暂停与时间有关的运动
        /// </summary>
        public static void PauseGame()
        {
            Time.TimeScale = 0;
        }

        /// <summary>
        /// 设置游戏速度
        /// 说明：通过timeScale缩放时间，可以改变所有与时间有关的运动速度
        /// </summary>
        /// <param name="timeScale">时间缩放</param>
        public static void SetGameSpeed(float timeScale)
        {
            Time.TimeScale = timeScale;
        } 
        #endregion

       

        ///// <summary>
        ///// 引擎携程委托
        ///// </summary>
        //public delegate void EngineCoroutine();

        ///// <summary>
        ///// 延时运行函数
        ///// </summary>
        ///// <param name="func">延时协程函数</param>
        ///// <param name="delayTimeSec">延时时间(秒)</param>
        //public static void StartFuncDelay(Action<int> func,float delayTimeSec)
        //{
            
        //}
    }
}
