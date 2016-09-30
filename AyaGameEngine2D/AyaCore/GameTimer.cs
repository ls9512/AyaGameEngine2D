using System.Diagnostics;

namespace AyaGameEngine2D.Core
{
    /// <summary>
    /// 类      名：GameTimer
    /// 功      能：游戏计时器静态类,提供毫秒级的精确运行时间统计
    ///             可以获取游戏已经运行的时间
    /// 日      期：2015-03-20
    /// 修      改：2015-12-31
    /// 作      者：ls9512
    /// </summary>
    internal static class GameTimer
    {
        #region 私有成员
        /// <summary>
        /// 开始时间
        /// </summary>
        private static long _startTime;

        /// <summary>
        /// 结束时间
        /// </summary>
        private static long _endTime;

        /// <summary>
        /// 计时器
        /// </summary>
        private static readonly Stopwatch Stopwatch = new Stopwatch();
        #endregion

        #region 公有成员
        /// <summary>
        /// 开始计时
        /// ★ 必须在使用其他功能函数前调用一次
        /// </summary>
        public static void StartTimer()
        {
            Stopwatch.Start();
            _startTime = Stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// 计时器时间(秒)
        /// </summary>
        public static float DurationSecond
        {
            get
            {
                _endTime = Stopwatch.ElapsedMilliseconds;
                return (_endTime - _startTime) * 1f / 1000;
            }
        }

        /// <summary>
        /// 计时器时间(毫秒)
        /// </summary>
        public static long DurationMillisecond
        {
            get
            {
                _endTime = Stopwatch.ElapsedMilliseconds;
                return _endTime - _startTime;
            }
        } 
        #endregion
    }
}
