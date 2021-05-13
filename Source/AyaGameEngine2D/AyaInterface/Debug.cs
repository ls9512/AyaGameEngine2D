using System;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：Debug
    /// 功      能：游戏调试静态类
    /// 日      期：2015-12-29
    /// 修      改：2015-12-29
    /// 作      者：ls9512
    /// </summary>
    public static class Debug
    {
        #region 引擎消息
        /// <summary>
        /// 推送引擎消息
        /// </summary>
        /// <param name="info"></param>
        public static void PushEngineInfo(EngineInfo info)
        {
            InfoPublisher.PushEngineInfo(info);
        } 
        #endregion

        #region 日志
        /// <summary>
        /// 打印一条调试日志
        /// </summary>
        /// <param name="msg">打印内容</param>
        public static void Log(object msg)
        {
            Console.WriteLine(msg.ToString());
        }

        /// <summary>
        /// 输出并保存一条日志
        /// </summary>
        /// <param name="logText">日志内容</param>
        public static void AddLog(string logText)
        {
            Log(logText);
            LogHelper.AddLog(logText);
        }

        /// <summary>
        /// 打印并保存一条日志
        /// </summary>
        /// <param name="logTitle">日志标题</param>
        /// <param name="logText">日志内容</param>
        public static void AddLog(string logTitle, string logText)
        {
            Log(logTitle + " " + logText);
            LogHelper.AddLog(logTitle, logText);
        } 
        #endregion

        #region 异常
        /// <summary>
        /// 抛出异常 - 发布引擎消息
        /// </summary>
        /// <param name="e">异常</param>
        public static void ThrowException(Exception e)
        {
            ThrowException("AGE2D Exception", e);
        }

        /// <summary>
        /// 抛出异常 - 发布引擎消息
        /// </summary>
        /// <param name="title">异常标题</param>
        /// <param name="e">异常</param>
        public static void ThrowException(string title, Exception e)
        {
            InfoPublisher.ThrowException("初始化错误", e);
        } 
        #endregion
    }
}
