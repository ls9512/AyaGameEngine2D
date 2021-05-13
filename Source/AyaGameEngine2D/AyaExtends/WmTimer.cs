using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 精确计时结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TimerCaps
    {
        /// <summary>
        /// 最小间隔
        /// </summary>
        public int periodMin;

        /// <summary>
        /// 最大间隔
        /// </summary>
        public int periodMax;
    }
}

namespace AyaGameEngine2D.Extends
{
    #region 依赖的计时器枚举
    /// <summary>
    /// 类      名：TimerMode
    /// 功      能：计时器类型枚举
    /// 日      期：2015-11-12
    /// 修      改：2015-06-22
    /// 作      者：ls9512
    /// </summary>
    [Flags]
    public enum TimerMode : uint
    {
        /// <summary>
        /// 单词触发
        /// </summary>
        TIME_ONESHOT = 0,
        /// <summary>
        /// 周期触发
        /// </summary>
        TIME_PERIODIC = 1,
        /// <summary>
        /// 函数回调
        /// </summary>
        TIME_CALLBACK_FUNCTION = 0x0000,
        /// <summary>
        /// 事件回调(SetEvent)
        /// </summary>
        TIME_CALLBACK_EVENT_SET = 0x0010,
        /// <summary>
        /// 事件脉冲回调
        /// </summary>
        TIME_CALLBACK_EVENT_PULSE = 0x0020
    }
    #endregion

    /// <summary>
    /// 类      名：WmTimer
    /// 功      能：多媒体定时触发器,提供1ms精度的高频率事件发生
    ///             (★在本程序中暂时无效原因不明)
    /// 日      期：2015-93-23
    /// 修      改：2015-11-17
    /// 作      者：ls9512
    /// 说      明：将定时发生的事件绑定到Tick
    /// </summary>
    public class WmTimer : IComponent
    {
        #region 私有成员
        /// <summary>
        /// 计时结构
        /// </summary>
        private static TimerCaps _caps;

        /// <summary>
        /// 间隔
        /// </summary>
        private int _interval;

        /// <summary>
        /// 精度
        /// </summary>
        private int _resolution;

        /// <summary>
        /// 是否运行
        /// </summary>
        private bool _isRunning;

        /// <summary>
        /// 计时模式
        /// </summary>
        private TimerMode _mode;

        /// <summary>
        /// 一次触发回调
        /// </summary>
        private readonly TimeProc _timeProcOneShot;

        /// <summary>
        /// 间隔触发回调
        /// </summary>
        private readonly TimeProc _timeProcPeriodic;

        /// <summary>
        /// 计时器ID
        /// </summary>
        private int _timerId;

        /// <summary>
        /// 回调事件委托
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="msg">消息</param>
        /// <param name="user">用户</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        private delegate void TimeProc(int id, int msg, int user, int param1, int param2);
        #endregion

        #region 公有成员
        /// <summary>
        /// 销毁事件句柄
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// 触发事件句柄
        /// </summary>
        public event EventHandler Tick;

        /// <summary>
        /// 运行标识
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
        }

        /// <summary>
        /// 计时器模式
        /// </summary>
        public TimerMode Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
            }
        }

        /// <summary>
        /// 位置
        /// </summary>
        public ISite Site { get; set; }
        #endregion

        #region 实例
        /// <summary>
        /// 静态实例
        /// </summary>
        static WmTimer()
        {
            Win32.timeGetDevCaps(ref _caps, Marshal.SizeOf(_caps));
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造函数
        /// </summary>
        public WmTimer()
        {
            _interval = _caps.periodMin;
            _resolution = _caps.periodMin;
            _mode = TimerMode.TIME_PERIODIC;
            _isRunning = false;
            _timeProcPeriodic = TimerPeriodicEventCallback;
            _timeProcOneShot = TimerOneShotEventCallback;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container"></param>
        public WmTimer(IContainer container)
            : this()
        {
            container.Add(this);
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 间隔精度
        /// </summary>
        public int Interval
        {
            get
            {
                return _interval;
            }
            set
            {
                if ((value < _caps.periodMin) || (value > _caps.periodMax))
                {
                    throw new Exception("invalid period");
                }
                _interval = value;
            }
        }



        /// <summary>
        /// Tick
        /// </summary>
        private void OnTick()
        {
            if (Tick != null)
            {
                Tick(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 开始计时
        /// </summary>
        public void Start()
        {
            if (!_isRunning)
            {
                lock (this)
                {
                    if (Mode == TimerMode.TIME_PERIODIC)
                    {
                        _timerId = Win32.timeSetEvent(_interval, 1, _timeProcPeriodic, UIntPtr.Zero, (int)Mode);
                    }
                    else
                    {
                        _timerId = Win32.timeSetEvent(_interval, 1, _timeProcOneShot, UIntPtr.Zero, (int)Mode);
                    }
                }
                // 创建失败
                if (_timerId == 0)
                {
                    throw new Exception("Unable to start WmTimer");
                }
                else _isRunning = true;
            }
        }

        /// <summary>
        /// 停止计时
        /// </summary>
        public void Stop()
        {
            if (_isRunning)
            {
                Win32.timeKillEvent(_timerId);
                _isRunning = false;
            }
        }

        /// <summary>
        /// 一次中断回调
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <param name="user"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        private void TimerOneShotEventCallback(int id, int msg, int user, int param1, int param2)
        {
            OnTick();
            Stop();
        }

        /// <summary>
        /// 周期事件回调
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <param name="user"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        private void TimerPeriodicEventCallback(int id, int msg, int user, int param1, int param2)
        {
            OnTick();
        } 
        #endregion

        #region 销毁
        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            Win32.timeKillEvent(_timerId);
            GC.SuppressFinalize(this);
            EventHandler disposed = Disposed;
            if (disposed != null)
            {
                disposed(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~WmTimer()
        {
            Win32.timeKillEvent(_timerId);
        } 
        #endregion
    }
}
