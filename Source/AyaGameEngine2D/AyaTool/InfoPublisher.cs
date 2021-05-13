using System;
using System.Collections.Generic;
using System.Drawing;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：InfoPublisher
    /// 功      能：提供运行在引擎主循环级，与游戏流程无关的文本消息提示
    /// 日      期：2015-11-20
    /// 修      改：2015-11-20
    /// 作      者：ls9512
    /// </summary>
    internal static class InfoPublisher
    {
        #region 私有成员
        /// <summary>
        /// 发现错误标识
        /// </summary>
        private static bool _runException; 
        #endregion

        #region 公有成员
        /// <summary>
        /// 消息列表
        /// </summary>
        public static List<EngineInfo> InfoList = new List<EngineInfo>();
        #endregion

        #region 消息操作
        /// <summary>
        /// 插入引擎消息
        /// </summary>
        /// <param name="info"></param>
        public static void PushEngineInfo(EngineInfo info)
        {
            // 插入消息队列
            InfoList.Add(info);
        }

        /// <summary>
        /// 移除消息
        /// </summary>
        /// <param name="e">消息</param>
        public static void RemoveEngineInfo(EngineInfo e)
        {
            InfoList.Remove(e);
        }
        #endregion

        #region 显示(每帧调用)
        /// <summary>
        /// 显示引擎消息列表 - 每一帧调用
        /// </summary>
        public static void ShowEngineInfo()
        {
            for (int i = InfoList.Count - 1; i >= 0; i--)
            {
                InfoList[i].DrawInfo();
            }
        }
        #endregion

        #region 异常信息
        /// <summary>
        /// 抛出异常提示
        /// </summary>
        /// <param name="titile">标题</param>
        /// <param name="exception">异常信息</param>
        public static void ThrowException(string titile, Exception exception)
        {
            if (General.Engine_IsInit)
            {
                if (!_runException)
                {
                    _runException = true;
                    EngineInfo info = new EngineInfo(titile + " - 请结束程序并修改以下错误！", exception.ToString(), DockLocType.Center);
                    info.SetTextShowTime(-1);
                    info.SetWrapWidth(150);
                    PushEngineInfo(info);
                    // 写入日志
                    LogHelper.AddLog(info.Title, info.Text);
                }
            }
            else
            {
                // 写入日志
                LogHelper.AddLog("捕获错误", exception.ToString());
            }

        }
        #endregion

        #region 调试信息
        /// <summary>
        /// 输出调试信息
        /// </summary>
        public static void ShowDebugInfo()
        {
            // 暂停性能计数
            PerformanceAnalyzer.StopPerformanceCount();
            // 每秒帧数
            string str = "FPS：" + PerformanceAnalyzer.Gaming_Fps.ToString("F2");
            float fontsize = 12f;
            float yy = 10;
            FontHelper.SetFont("Arial", fontsize);
            //FontHelper.DrawString(str, Color.Black, 11, 11);
            FontHelper.DrawString(str, Color.Yellow, 10, yy);
            // 总帧数
            str = "Fps Count：" + PerformanceAnalyzer.Gaming_FpsCount.ToString();
            FontHelper.DrawString(str, Color.Yellow, 10, yy += fontsize * 1.8f);

            //// 渲染时间
            //str = "Cpu Time：" + Time.CpuTime.ToString("F2");
            //FontHelper.DrawString(str, Color.Yellow, 10, yy += fontsize * 1.8f);
            //str = "Render Time：" + Time.RenderTime.ToString("F2");
            //FontHelper.DrawString(str, Color.Yellow, 10, yy += fontsize * 1.8f);

            // 每秒元素绘制数
            str = "Object：" + PerformanceAnalyzer.Gaming_ObjectPerSec.ToString("F2");
            //FontHelper.DrawString(str, Color.Black, 11, 61);
            FontHelper.DrawString(str, Color.Yellow, 10, yy += fontsize * 1.8f);
            // 跳帧次数
            if (PerformanceAnalyzer.Gaming_FpsOutTime > 0)
            {
                str = "Timeout：" + PerformanceAnalyzer.Gaming_FpsOutTime.ToString();
                //FontHelper.DrawString(str, Color.Black, 11, 86);
                FontHelper.DrawString(str, Color.Red, 10, yy += fontsize * 1.8f);
            }
            str = "Mouse：" + Input.MousePosition.X + "," + Input.MousePosition.Y;
            FontHelper.DrawString(str, Color.Yellow, 10, yy += fontsize * 1.8f);
            // 恢复性能计数
            PerformanceAnalyzer.StartPerformanceCount();
        } 
        #endregion
    }
}
