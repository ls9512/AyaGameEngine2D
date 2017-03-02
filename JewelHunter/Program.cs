using System;
using System.Windows.Forms;

namespace JewelHunter
{
    /// <summary>
    /// 类      名：Program
    /// 功      能：程序类，游戏启动入口点
    /// 日      期：2015-11-21
    /// 修      改：2016-04-14
    /// 作      者：ls9512
    /// </summary>
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(GameForm.Instance);
        }
    }
}
