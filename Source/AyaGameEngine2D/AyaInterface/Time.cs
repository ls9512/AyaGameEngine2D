using System;

using AyaGameEngine2D.Core;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：Time
    /// 功      能：游戏时间静态参数类
    /// 日      期：2015-11-17
    /// 修      改：2016-01-02
    /// 作      者：ls9512
    /// </summary>
    public static class Time
    {
        #region 私有成员
        /// <summary>
        /// 参与帧时间预测计算的帧时间缓存
        /// </summary>
        private static readonly float[] DeltaTimeCache = new float[General.Engine_FpsTimeCache];

        /// <summary>
        /// 上一次更新时间
        /// </summary>
        private static float _lastTimeUpdate = 0;
        #endregion

        #region 公有成员
        /// <summary>
        /// 每60帧消耗的时间，用于帧速控制，受TimeScale影响
        /// </summary>
        public static float DeltaTime
        {
            get { return _deltaTime; }
            set
            {
                // 一帧时不需要计算
                if (DeltaTimeCache.Length == 1)
                {
                    _deltaTime = value;
                    return;
                }
                // 前移
                for (int i = 1; i < DeltaTimeCache.Length; i++)
                {
                    DeltaTimeCache[i - 1] = DeltaTimeCache[i];
                }
                // 存储当前帧时间
                DeltaTimeCache[DeltaTimeCache.Length - 1] = value;
                // 计算当前帧预测时间 (缓存帧时间平均值)
                float sum = 0;
                for (int i = 0; i < DeltaTimeCache.Length; i++)
                {
                    sum += DeltaTimeCache[i];
                }
                // 结果赋值
                _deltaTime = sum / DeltaTimeCache.Length;
            }
        }
        private static float _deltaTime;

        /// <summary>
        /// 每一帧的时间
        /// </summary>
        public static float DeltaTimeFrame
        {
            get { return _deltaTime / General.Engine_Fps_Now; }
        }

        /// <summary>
        /// 每60帧消耗的时间，用于帧速控制，不受TimeScale影响
        /// </summary>
        public static float DeltaTimeUnScale = 0f;

        /// <summary>
        /// 每60帧的时间缩放速度，用于调节游戏速度
        /// 为0时可暂停游戏中所有依赖时间的运动
        /// 为n时则使游戏以N倍速度运行
        /// </summary>
        public static float TimeScale
        {
            get { return _timeScale; }
            set
            {
                _timeScale = _timeScale < 0 ? 0 : value;
            }
        }
        private static float _timeScale = 1f;

        /// <summary>
        /// 当前帧CPU时间
        /// </summary>
        public static float CpuTime
        {
            set
            {
                if (GameTimer.DurationSecond > _lastTimeUpdate)
                {
                    _lastTimeUpdate = GameTimer.DurationSecond;
                    _cpuTime = value;
                }
            }
            get { return _cpuTime; }
        }
        private static float _cpuTime;

        /// <summary>
        /// 当前帧渲染时间
        /// </summary>
        public static float RenderTime
        {
            set
            {
                if (GameTimer.DurationSecond > _lastTimeUpdate)
                {
                    _lastTimeUpdate = GameTimer.DurationSecond;
                    _renderTime = value;
                }
            }
            get { return _renderTime; }
        }
        private static float _renderTime;

        /// <summary>
        /// 游戏运行时间(毫秒)
        /// </summary>
        public static float RunTimeMSEC
        {
            get { return GameTimer.DurationMillisecond; }
        }

        /// <summary>
        /// 游戏运行时间(秒)
        /// </summary>
        public static float RunTimeSEC
        {
            get { return GameTimer.DurationSecond; }
        }

        #endregion

        #region 公有方法
        // <summary>
        /// 获取HH MM SS格式的日期字符串
        /// </summary>
        /// <param name="sec">秒数</param>
        /// <returns></returns>
        public static string TiemStringToHHMMSS(int sec)
        {
            TimeSpan ts = new TimeSpan(0, 0, sec);
            string str;
            if (ts.Hours == 0 && ts.Minutes != 0)
            {
                str = ts.Minutes + "分 " + ts.Seconds + "秒";
            }
            else if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = ts.Seconds + "秒";
            }
            else
            {
                str = ts.Hours + "时 " + ts.Minutes + "分 " + ts.Seconds + "秒";
            }
            return str;
        }

        /// <summary>
        /// 获取当前时间 (格式: yyyy-MM-dd HH-mm-ss-fff)
        /// </summary>
        public static string TimeStringToFormart(string formart)
        {
            // "yyyy-MM-dd HH-mm-ss-fff"
            return DateTime.Now.ToString(formart);
        }
        #endregion
    }
}
