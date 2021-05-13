using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AcgParkour
{
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
            //GameForm gameForm = new GameForm();
            Application.Run(MainForm.Instance);
            // Application.Run();
        }
    }
}
