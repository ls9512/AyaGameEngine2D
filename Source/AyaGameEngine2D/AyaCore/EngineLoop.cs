namespace AyaGameEngine2D.Core
{
    /// <summary>
    /// 类      名：EngineLoop
    /// 功      能：引擎级主循环体
    /// 说      明：在主循环体内容执行完后执行
    /// 日      期：2016-01-29
    /// 修      改：2016-01-29
    /// 作      者：ls9512
    /// </summary>
    internal static class EngineLoop
    {
        /// <summary>
        /// 引擎IO
        /// </summary>
        public static void IO()
        {
            
        }

        /// <summary>
        /// 引擎逻辑
        /// </summary>
        public static void Logic()
        {
            // 处理计时器事件
            Timer.DoTimerEvent();
        }

        /// <summary>
        /// 引擎渲染
        /// </summary>
        public static void Render()
        {
            // 输出调试信息
            if (General.Engine_Debug && General.Debug_ShowFPS)
            {
                InfoPublisher.ShowDebugInfo();
            }
            // 显示引擎消息列表
            InfoPublisher.ShowEngineInfo();
        }
    }
}
