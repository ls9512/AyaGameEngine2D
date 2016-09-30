using System;
using System.Text;
using System.IO;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：LogHelper
    /// 功      能：日志管理器，以天为单位记录引擎运行情况
    /// 日      期：2015-11-20
    /// 修      改：2015-12-29
    /// 作      者：ls9512
    /// </summary>
    public static class LogHelper
    {
        #region 私有成员
        /// <summary>
        /// 日志文件路径
        /// </summary>
        private static string LogFilePath
        {
            get
            {
                // 目录不存在则创建
                if (!FileSystem.IsExistDirectory(General.Engine_Log_Path))
                {
                    FileSystem.CreateDir(General.Engine_Log_Path);
                }
                // 以时间命名保存
                string path = General.Engine_Log_Path + Time.TimeStringToFormart("yyyy-MM-dd") + "  Age2D Log.txt";
                // 文件不存在则创建
                if (!FileSystem.IsExistFile(path))
                {
                    FileSystem.CreateFile(path);
                }
                return path;
            }
        }
        #endregion

        #region 日志操作
        /// <summary>
        /// 增加日志
        /// </summary>
        /// <param name="logText">日志内容</param>
        public static void AddLog(string logText)
        {
            AddLog(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]"), logText);
        }

        /// <summary>
        /// 增加日志
        /// </summary>
        /// <param name="logTitle">日志标题</param>
        /// <param name="logText">日志内容</param>
        public static void AddLog(string logTitle, string logText)
        {
            if (!General.Engine_Debug) return;
            logText = logText.Replace("\n", "\r\n");
            logText += "\r\n\r\n";
            using (StreamWriter sw = new StreamWriter(LogFilePath, true, Encoding.Unicode))
            {
                sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] - ") + logTitle + "\r\n" + logText);
            }
        } 
        #endregion
    }
}
