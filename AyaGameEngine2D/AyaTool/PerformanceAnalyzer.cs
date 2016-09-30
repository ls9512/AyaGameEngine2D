using System.Diagnostics;
using System.Drawing;

using AyaGameEngine2D.Core;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：PerformanceAnalyzer
    /// 功      能：性能分析器，统计CPU、内存、实时帧率等信息用于优化参考
    /// 日      期：2015-11-20
    /// 修      改：2015-11-20
    /// 作      者：ls9512
    /// </summary>
    public static class PerformanceAnalyzer
    {
        #region 公有字段
        /// <summary>
        /// 实时帧数
        /// </summary>
        public static float Gaming_Fps = 0f;
        /// <summary>
        /// 实时帧数峰值
        /// </summary>
        public static float Gaming_FpsMax = 0f;
        /// <summary>
        /// 帧统计
        /// </summary>
        public static long Gaming_FpsCount = 0;
        /// <summary>
        /// 逻辑超时帧数
        /// </summary>
        public static long Gaming_LogicFpsOutTime = 0;
        /// <summary>
        /// 绘图超时帧数
        /// </summary>
        public static long Gaming_GraphicFpsOutTime = 0;
        /// <summary>
        /// 帧超时总数
        /// </summary>
        public static long Gaming_FpsOutTime
        {
            get { return Gaming_LogicFpsOutTime + Gaming_GraphicFpsOutTime; }
        }
        /// <summary>
        /// 总游戏时间(秒)
        /// </summary>
        public static float Gaming_TimeCount
        {
            get { return PreciseTimer.DurationSecond; }
        }
        /// <summary>
        /// 纹理绘制数
        /// </summary>
        public static long Gaming_TextureCount
        {
            set
            {
                if (_isCount)
                {
                    _gaming_TextureCount++;
                    _gaming_TexturePerSecCount++;
                }
            }
            get 
            {
                return _gaming_TextureCount;
            }
        }
        /// <summary>
        /// 非纹理元素绘制数
        /// </summary>
        public static long Gaming_ElementCount
        {
            set
            {
                if (_isCount)
                {
                    _gaming_ElementCount++; ;
                    _gaming_ElementPerSecCount++;
                }
            }
            get
            {
                return _gaming_ElementCount;
            }
        }
        /// <summary>
        /// 元素绘制数
        /// </summary>
        public static long Gaming_ObjectCount
        {
            get { return _gaming_TextureCount + _gaming_ElementCount; }
        }

        // 以下是实时监控数据

        /// <summary>
        /// 每秒纹理绘制数
        /// </summary>
        public static float Gaming_TexturePerSec = 0;
        /// <summary>
        /// 每秒非纹理元素绘制数
        /// </summary>
        public static float Gaming_ElementPerSec = 0;
        /// <summary>
        /// 每秒元素绘制数
        /// </summary>
        public static float Gaming_ObjectPerSec
        {
            get { return Gaming_TexturePerSec + Gaming_ElementPerSec; }
        }
        #endregion

        #region 私有字段
        /// <summary>
        /// 计数标识 - 用于跳过一些引擎本身的资源消耗计算
        /// </summary>
        private static bool _isCount = true;
        /// <summary>
        /// 每秒帧数
        /// </summary>
        private static int _gameing_FpsPerSec = 0;
        /// <summary>
        /// 每秒时间统计
        /// </summary>
        private static float _gaming_TimePerSec = 0;
        /// <summary>
        /// 纹理绘制数
        /// </summary>
        private static long _gaming_TextureCount = 0;
        /// <summary>
        /// 非纹理元素绘制数
        /// </summary>
        private static long _gaming_ElementCount = 0;
        /// <summary>
        /// 每秒纹理绘制数
        /// </summary>
        private static long _gaming_TexturePerSecCount = 0;
        /// <summary>
        /// 每秒非纹理元素绘制数
        /// </summary>
        private static long _gaming_ElementPerSecCount = 0;
        /// <summary>
        /// 绘图超时警告
        /// </summary>
        private static bool _isGraphicFrameOutTimeWarning = false;
        #endregion

        #region 性能计数 / 启动和停止
        /// <summary>
        /// 开启性能计数
        /// </summary>
        internal static void StartPerformanceCount()
        {
            _isCount = true;
        }

        /// <summary>
        /// 停止性能计数
        /// </summary>
        internal static void StopPerformanceCount()
        {
            _isCount = false;
        }

        /// <summary>
        /// 性能统计
        /// 每一帧调用一次，每满一秒更新一次数据
        /// </summary>
        /// <param name="deltaTimeUnScale">间隔时间(无缩放)</param>
        internal static void PerformanceCount(float deltaTimeUnScale)
        {
            if (!_isCount) return;
            // 累积间隔时间
            _gaming_TimePerSec += deltaTimeUnScale;
            // 累积时间段内的帧数
            _gameing_FpsPerSec++;
            // 总帧数累加
            Gaming_FpsCount++;
            // 累积满1秒更新相关数据
            if (_gaming_TimePerSec >= 1f)
            {
                // 1秒内的平均帧数
                Gaming_Fps = _gameing_FpsPerSec * 1f / _gaming_TimePerSec;
                // 1秒内的平均纹理数
                Gaming_TexturePerSec = _gaming_TexturePerSecCount * 1f / _gaming_TimePerSec;
                // 1秒内的平均非纹理元素数
                Gaming_ElementPerSec = _gaming_ElementPerSecCount * 1f / _gaming_TimePerSec;
                // 清零重新统计
                _gameing_FpsPerSec = 0;
                _gaming_TimePerSec = 0;
                _gaming_TexturePerSecCount = 0;
                _gaming_ElementPerSecCount = 0;
            }

            // 性能警告
            if (!_isGraphicFrameOutTimeWarning)
            {
                // 启动1分钟后丢帧率大于10%
                if (Gaming_GraphicFpsOutTime * 100f / Gaming_FpsCount >= 10f && Time.RunTimeSEC > 60)
                {
                    _isGraphicFrameOutTimeWarning = true;
                    string str = "";
                    str += GetPerformanceReport();
                    str += "\n";
                    str += "  帧消耗时间过高，游戏运行稳定性较差，请改善资源调度和代码结构，进行合理的性能优化。";
                    EngineInfo info = new EngineInfo("Age2D - 帧性能不足警告", str, DockLocType.LowerRightCorner);
                    info.SetTitleColor(Color.OrangeRed);
                    info.SetTextShowTime(10f);
                    InfoPublisher.PushEngineInfo(info);
                }
            }
        } 
        #endregion

        #region 性能监视报告
        /// <summary>
        /// 获取引擎运行到目前为止的性能监视报告
        /// </summary>
        /// <returns>监视结果</returns>
        public static string GetPerformanceReport()
        {
            string str = "[性能监视结果参考]\n";
            str += " 游戏运行时间：\t" + Time.TiemStringToHHMMSS((int)Time.RunTimeSEC) + "\n";
            str += " 逻辑执行超时：\t" + Gaming_LogicFpsOutTime + "  \t" + (Gaming_LogicFpsOutTime * 100f / Gaming_FpsCount).ToString("F2") + "％\n";
            str += " 绘图执行超时：\t" + Gaming_GraphicFpsOutTime + "  \t" + (Gaming_GraphicFpsOutTime * 100f / Gaming_FpsCount).ToString("F2") + "％\n";
            str += " 帧超时统计：\t" + Gaming_FpsOutTime + "  \t" + (Gaming_FpsOutTime * 100f / Gaming_FpsCount).ToString("F2") + "％\n";
            str += " 平均帧数：\t" + (Gaming_FpsCount * 1f / Time.RunTimeSEC).ToString("F2") + "f/s\n";
            str += " 总帧数：\t" + Gaming_FpsCount + "\n";
            str += " 平均每帧绘图：\t" + (int)(Gaming_ObjectCount * 1f / Gaming_FpsCount) + "\n";
            str += " 平均每秒绘图：\t" + (int)(Gaming_ObjectCount * 1f / Time.RunTimeSEC) + "\n";
            str += " 总绘图：\t" + Gaming_ObjectCount;
            return str;
        } 
        #endregion

        #region 内存 / CPU   测试
        /// <summary>
        /// 获取当前进程占用的物理内存
        /// </summary>
        /// <returns></returns>
        internal static double GetProcessUsedMemory()
        {
            double usedMemory = 0;
            usedMemory = Process.GetCurrentProcess().WorkingSet64 / 1024.0 / 1024.0;
            return usedMemory;
        }

        /// <summary>
        /// 获取当前进程占用的虚拟内存
        /// </summary>
        /// <returns></returns>
        internal static double GetProcessUsedVirtualMemory()
        {
            double usedMemory = 0;
            usedMemory = Process.GetCurrentProcess().VirtualMemorySize64 / 1024.0 / 1024.0;
            return usedMemory;
        }         
        #endregion
    }
}
