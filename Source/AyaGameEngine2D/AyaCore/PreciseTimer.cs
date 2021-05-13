namespace AyaGameEngine2D.Core
{
    /// <summary>
    /// 类      名：PreciseTimer
    /// 功      能：高性能计数器
    ///             用于精确计算每一帧消耗的时间，保证游戏的运行速度在不同速度的CPU上一致。
    ///             计数精确到纳秒，但是每次查询有较大的开销，所以不建议频繁调用。
    /// 日      期：2015-11-16
    /// 修      改：2016-01-01
    /// 作      者：ls9512
    /// </summary>
    internal static class PreciseTimer
    {
        #region 公有成员
        /// <summary>
        /// 每秒CPU计数
        /// </summary>
        public static long TicksPerSecond
        {
            get { return _ticksPerSecond; }
        }
        /// <summary>
        /// 每毫秒CPU计数
        /// </summary>
        public static long TicksPerMillisecond
        {
            get { return _ticksPerMillisecond; }
        }
        #endregion

        #region 私有成员
        /// <summary>
        /// 每秒CPU计数
        /// </summary>
        private static long _ticksPerSecond;

        /// <summary>
        /// 每毫秒CPU计数
        /// </summary>
        private static long _ticksPerMillisecond;

        /// <summary>
        /// 上一次时间
        /// </summary>
        private static long _previousElapsedTime;

        /// <summary>
        /// 开始时间
        /// </summary>
        private static long _startTime;

        /// <summary>
        /// 结束时间
        /// </summary>
        private static long _endTime;
        #endregion

        #region 公有方法
        /// <summary>
        /// 高性能计数器初始化
        /// ★ 必须在调用其他功能函数之前调用一次
        /// </summary>
        public static void StartTimer()
        {
            // 获取每秒晶振次数
            Win32.QueryPerformanceFrequency(ref _ticksPerSecond);
            _ticksPerMillisecond = (long)(_ticksPerSecond * 1f / 1000);
            // 获取启动时间
            Win32.QueryPerformanceCounter(ref _startTime);
            // 第一次获取间隔时间
            GetElapsedTime();
        }

        /// <summary>
        /// 获取间隔时间
        /// 警告：该时间间隔可以用于短时间内的累加和时间统计,但不宜超过几分钟,否则会导致极大的精度问题!
        /// </summary>
        /// <returns>与上一次调用的间隔时间</returns>
        public static float GetElapsedTime()
        {
            Win32.QueryPerformanceCounter(ref _endTime);
            float elapsedTime = (_endTime - _previousElapsedTime) * 1f / _ticksPerSecond;
            _previousElapsedTime = _endTime;
            return elapsedTime;
        }

        /// <summary>
        /// 获取运行时间(秒)
        /// </summary>
        /// <returns>运行时间</returns>
        public static float DurationSecond
        {
            get
            {
                Win32.QueryPerformanceCounter(ref _endTime);
                return (_endTime - _startTime) * 1f / _ticksPerSecond;
            }
        }

        /// <summary>
        /// 获取运行时间(毫秒)
        /// </summary>
        /// <returns>运行时间</returns>
        public static float DurationMillisecond
        {
            get
            {
                Win32.QueryPerformanceCounter(ref _endTime);
                return (_endTime - _startTime) * 1000f / _ticksPerSecond;
            }
        }

        /// <summary>
        /// 获取运行时间(CPU计数)
        /// </summary>
        /// <returns>运行时间</returns>
        public static long DurationPerformanceCounter
        {
            get
            {
                Win32.QueryPerformanceCounter(ref _endTime);
                return _endTime;
            }
        }

        /// <summary>
        /// 重设上一帧时间
        /// </summary>
        public static void ResetPreviousTime()
        {
            Win32.QueryPerformanceCounter(ref _previousElapsedTime);
        } 
        #endregion
    }
}
