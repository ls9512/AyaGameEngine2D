using System.Collections.Generic;

namespace AyaGameEngine2D
{
    #region 使用说明
	/*
        int key = Timer.Delay(() =>
        {
            // 延时执行的内容
            // 
        }, time);

        int key = Timer.Interval(() =>
        {
            // 间隔一定时间执行的内容
            // 
        }, time);

        // 删除某个已经在执行的事件
        Timer.Remove(key);
    */ 
	#endregion

    /// <summary>
    /// 类      名：Timer
    /// 功      能：计时器类，提供延时一定时间执行某个方法的功能。
    /// 说      明：与多媒体定时器依赖winAPI实现不同，该类通过游戏主循环的时间计算来实现。
    ///             所有事件在每一帧逻辑执行完后处理。
    /// 日      期：2016-01-29
    /// 修      改：2016-01-29
    /// 作      者：ls9512
    /// </summary>
    public static class Timer
    {
        #region 公有成员
        /// <summary>
        /// 计时器事件
        /// </summary>
        public delegate void TimerEvent(); 
        #endregion

        #region 私有成员
        /// <summary>
        /// 事件序号
        /// </summary>
        private static int _eventKey = 0;

        /// <summary>
        /// 延时事件列表
        /// </summary>
        private static readonly Dictionary<int, TimerAction> TimerDelayEvent = new Dictionary<int, TimerAction>();

        /// <summary>
        /// 定时事件列表
        /// </summary>
        private static readonly Dictionary<int, TimerAction> TimerIntervalEvent = new Dictionary<int, TimerAction>();

        /// <summary>
        /// 移除键值列表
        /// </summary>
        private static readonly List<int> RemoveKeyList = new List<int>(); 
        #endregion

        #region 内部方法
        /// <summary>
        /// 处理计时器事件，由引擎每一帧调用
        /// </summary>
        internal static void DoTimerEvent()
        {
            foreach (TimerAction e in TimerDelayEvent.Values)
            {
                e.Update();
            }
            foreach (TimerAction e in TimerIntervalEvent.Values)
            {
                e.Update();
            }
            for (int i = RemoveKeyList.Count - 1; i >= 0; i--)
            {
                int key = RemoveKeyList[i];
                TimerDelayEvent.Remove(key);
                TimerIntervalEvent.Remove(key);
                RemoveKeyList.Remove(key);
            }
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 创建延时触发事件
        /// </summary>
        /// <param name="e">事件</param>
        /// <param name="time">延迟时间</param>
        /// <returns></returns>
        public static int Delay(TimerEvent e, float time)
        {
            int key = ++_eventKey;
            TimerDelayEvent.Add(key, new TimerAction(key, e, time, TimerEventType.Delay));
            return key;
        }

        /// <summary>
        /// 创建间隔定时触发事件
        /// </summary>
        /// <param name="e">事件</param>
        /// <param name="time">间隔时间</param>
        /// <returns>事件ID</returns>
        public static int Interval(TimerEvent e, float time)
        {
            int key = ++_eventKey;
            TimerIntervalEvent.Add(key, new TimerAction(key, e, time, TimerEventType.Interval));
            return key;
        }

        /// <summary>
        /// 移除指定的事件
        /// </summary>
        /// <param name="eventKey">事件ID</param>
        public static void Remove(int key)
        {
            RemoveKeyList.Add(key);
        } 
        #endregion
    }

    #region 计时器事件类型枚举
    /// <summary>
    /// 类      名：TimerEventType
    /// 功      能：计时器事件类型枚举
    /// 日      期：2016-01-29
    /// 修      改：2016-01-29
    /// 作      者：ls9512
    /// </summary>
    internal enum TimerEventType
    {
        /// <summary>
        /// 延时触发
        /// </summary>
        Delay,
        /// <summary>
        /// 间隔定时触发
        /// </summary>
        Interval
    } 
    #endregion

    /// <summary>
    /// 类      名：TimerAction
    /// 功      能：计时器事件类，供Timer调用
    /// 日      期：2016-01-29
    /// 修      改：2016-01-29
    /// 作      者：ls9512
    /// </summary>
    internal class TimerAction
    {
        #region 公有成员
        /// <summary>
        /// 事件ID
        /// </summary>
        public int Key
        {
            set { _key = value; }
            get { return _key; }
        }
        private int _key; 
        #endregion

        #region 私有成员
        /// <summary>
        /// 事件
        /// </summary>
        private readonly Timer.TimerEvent _event;

        /// <summary>
        /// 时间
        /// </summary>
        private readonly float _time;

        /// <summary>
        /// 时间统计
        /// </summary>
        private float _timeCount;

        /// <summary>
        /// 事件类型
        /// </summary>
        private readonly TimerEventType _type;

        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="key">事件ID</param>
        /// <param name="e">事件</param>
        /// <param name="time">事件</param>
        /// <param name="type">类型</param>
        public TimerAction(int key,Timer.TimerEvent e, float time, TimerEventType type)
        {
            _key = key;
            _timeCount = 0;
            _event = e;
            _time = time;
            _type = type;
        }

        #endregion

        #region 私有成员
        /// <summary>
        /// 执行
        /// </summary>
        private void doEvent()
        {
            if (_event != null) _event();
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 更新，每一帧执行
        /// </summary>
        public void Update()
        {
            switch (_type)
            {
                case TimerEventType.Delay:
                    // 延迟执行
                    if (_timeCount >= _time)
                    {
                        doEvent();
                        Timer.Remove(_key);
                    }
                    break;
                case TimerEventType.Interval:
                    // 间隔触发
                    if (_timeCount >= _time)
                    {
                        doEvent();
                        _timeCount -= _time;
                    }
                    break;
            }
            _timeCount += Time.DeltaTimeFrame;
        } 
        #endregion
    }
}
