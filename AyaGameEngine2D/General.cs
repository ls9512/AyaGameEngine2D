using System;
using System.Drawing;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：General
    /// 功      能：引擎全局参数静态类
    /// 日      期：2015-03-18
    /// 修      改：2015-03-30
    /// 作      者：ls9512
    /// </summary>
    public static class General
    {
        #region 程序参数
        /// <summary>
        /// 程序名称
        /// </summary>
        public static string Pro_Title = "Aya Game Engine 2D";
        /// <summary>
        /// 程序版本
        /// </summary>
        public static string Pro_Ver = "0.9";
        /// <summary>
        /// 程序发布日期
        /// </summary>
        public static string Pro_Date = "2015.11.20";
        /// <summary>
        /// 游戏窗体句柄
        /// </summary>
        public static IntPtr Pro_WinHandle = IntPtr.Zero;
        /// <summary>
        /// 游戏GL控件句柄
        /// </summary>
        public static IntPtr Pro_GLHandle = IntPtr.Zero; 
        #endregion

        #region 调试参数
        /// <summary>
        /// 调试参数 - 是否逐帧绘制
        /// </summary>
        public static bool Debug_FrameStop = false;
        /// <summary>
        /// 调试参数 - 是否显示FPS
        /// </summary>
        public static bool Debug_ShowFPS = false;
        /// <summary>
        /// 调试参数 - 捕获错误
        /// </summary>
        public static bool Debug_RunException = false;
        /// <summary>
        /// 调试参数 - 上一次异常时间
        /// </summary>
        public static long Debug_LastExceptionTime = 0;
        #endregion

        #region 引擎参数
        /// <summary>
        /// 引擎调试模式
        /// </summary>
        public static bool Engine_Debug
        {
            get 
            {
                if (Engine_RunMode == EngineRunMode.Debug) return true;
                return false;
            }
        }
        /// <summary>
        /// 显示引擎消息
        /// </summary>
        public static bool Engine_ShowLogo = true;
        /// <summary>
        /// 引擎运行模式
        /// </summary>
        public static EngineRunMode Engine_RunMode = EngineRunMode.Debug;
        /// <summary>
        /// 引擎计时器模式
        /// </summary>
        internal static EngineTimeMode Engine_TimeMode = EngineTimeMode.GameTimer;
        /// <summary>
        /// 引擎帧时间缓存(参与预测计算的帧数)
        /// 说明：参与预算的帧数越多，应对帧率尖峰的稳定性越强，帧率动态调节能力越差。
        /// </summary>
        internal static int Engine_FpsTimeCache= 5;
        /// <summary>
        /// 引擎是否完成初始化
        /// </summary>
        internal static bool Engine_IsInit = false;
        /// <summary>
        /// 当前帧数
        /// </summary>
        internal static float Engine_Fps_Now = 60;
        /// <summary>
        /// 默认帧数
        /// </summary>
        internal static float Engine_Fps_Def = 60;
        /// <summary>
        /// 最大帧数
        /// </summary>
        internal static float Engine_Fps_Max = 120;
        /// <summary>
        /// 帧数限制
        /// </summary>
        internal static bool Engine_Fps_Limit = true;
        /// <summary>
        /// 截图保存路径
        /// </summary>
        internal static string Engine_ScreenShot_Path = FileSystem.GetProgramPath() + @"\ScreenShot\";
        /// <summary>
        /// 日志文件保存路径
        /// </summary>
        internal static string Engine_Log_Path = FileSystem.GetProgramPath() + @"\Log\";
        /// <summary>
        /// 窗体矩形
        /// </summary>
        internal static Rectangle Win_Rect = new Rectangle(0, 0, 1280, 720);
        /// <summary>
        /// 绘制矩形 - 缩放显示到窗体矩形
        /// </summary>
        internal static Rectangle Draw_Rect = new Rectangle(0, 0, 1280, 720);   
        #endregion     
    }

    #region 引擎计时模式枚举
    /// <summary>
    /// 引擎计时模式枚举
    /// </summary>
    public enum EngineTimeMode
    {
        /// <summary>
        /// 普通计时器
        /// </summary>
        GameTimer,
        /// <summary>
        /// 高性能计时器
        /// </summary>
        PreciseTimer,
    } 
    #endregion

    #region 引擎运行模式枚举
    /// <summary>
    /// 类      名：EngineRunMode
    /// 功      能：引擎运行模式
    /// 日      期：2015-12-29
    /// 修      改：2015-12-29
    /// 作      者：ls9512
    /// </summary>
    public enum EngineRunMode
    {
        /// <summary>
        /// 发布模式
        /// </summary>
        Release,
        /// <summary>
        /// 调试模式
        /// </summary>
        Debug,
        /// <summary>
        /// 开发模式
        /// </summary>
        Develop,
    } 
    #endregion

    #region 方向枚举
    /// <summary>
    /// 类      名：Direction
    /// 功      能：方向枚举
    /// 日      期：2015-03-21
    /// 修      改：2015-12-27
    /// 作      者：ls9512
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// 下
        /// </summary>
        Down = 0,
        /// <summary>
        /// 左
        /// </summary>
        Left = 1,
        /// <summary>
        /// 右
        /// </summary>
        Right = 2,
        /// <summary>
        /// 上
        /// </summary>
        Up = 3,
        /// <summary>
        /// 无 (用于碰撞检测，不可作为矩阵纹理索引)
        /// </summary>
        None = -1
    } 
    #endregion

    #region 动画帧速枚举
    /// <summary>
    /// 类      名：FrameSpeed
    /// 功      能：帧速度枚举
    /// 日      期：2015-03-21
    /// 修      改：2015-12-27
    /// 作      者：ls9512
    /// </summary>
    public enum FrameSpeed
    {
        /// <summary>
        /// 最慢
        /// </summary>
        Slowest,
        /// <summary>
        /// 比较慢
        /// </summary>
        Slower,
        /// <summary>
        /// 慢
        /// </summary>
        Slow,
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 快
        /// </summary>
        Fast,
        /// <summary>
        /// 比较快
        /// </summary>
        Faster,
        /// <summary>
        /// 最快
        /// </summary>
        Fastest,
    } 
    #endregion

    #region 停靠位置枚举
    /// <summary>
    /// 类      名：DockLocType
    /// 功      能：停靠位置枚举
    /// 日      期：2015-11-21
    /// 修      改：2015-12-27
    /// 作      者：ls9512
    /// </summary>
    public enum DockLocType
    {
        /// <summary>
        /// 用户自定义
        /// </summary>
        User,
        /// <summary>
        /// 中心
        /// </summary>
        Center,
        /// <summary>
        /// 右下角
        /// </summary>
        LowerRightCorner,
        /// <summary>
        /// 左下角
        /// </summary>
        LowerLeftCorner,
        /// <summary>
        /// 左上角
        /// </summary>
        TopLeftCorner,
        /// <summary>
        /// 右上角
        /// </summary>
        TopRightCorner,
    }
    #endregion
}
