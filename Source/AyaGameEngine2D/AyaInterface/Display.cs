using System.Windows.Forms;
using System.Drawing;

using AyaGameEngine2D.Controls;
using AyaGameEngine2D.Core;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：Display
    /// 功      能：显示静态类
    ///             用于提供一些常用的显示方法以供快速调用
    /// 日      期：2015-12-30
    /// 修      改：2015-12-30
    /// 作      者：ls9512
    /// </summary>
    public static class Display
    {
        #region 内部委托和事件
        /// <summary>
        /// 设置鼠标指针样式委托
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="hotPoint"></param>
        internal delegate void SetCursorStyleEventHandler(Bitmap cursor, Point hotPoint);

        /// <summary>
        /// 设置鼠标指针样式事件
        /// </summary>
        internal static event SetCursorStyleEventHandler OnSetCursorStyle;
        #endregion

        #region LOGO
        /// <summary>
        /// 添加LOGO
        /// </summary>
        /// <param name="bmp">图像</param>
        public static void AddEngineLogo(Bitmap bmp)
        {
            EngineLogo.AddLogo(bmp);
        } 
        #endregion

        #region 截图
        /// <summary>
        /// 屏幕截图
        /// ★ 文件读写会阻塞引擎运行，仅供调试用
        /// </summary>
        public static void ScreenShot()
        {
            OpenGLPanel.Instance.ScreenShot();
        }
        #endregion

        #region 鼠标指针
        /// <summary>
        /// 设置鼠标指针显示
        /// </summary>
        /// <param name="isShow">是否显示</param>
        public static void SetCursorShow(bool isShow)
        {
            if (isShow)
            {
                Cursor.Show();
            }
            else
            {
                Cursor.Hide();
            }
        }

        /// <summary>
        /// 设置鼠标指针样式
        /// </summary>
        /// <param name="cursor">鼠标图片</param>
        /// <param name="hotPoint">热点</param>
        public static void SetCursorStyle(Bitmap cursor, Point hotPoint)
        {
            if (OnSetCursorStyle != null)
            {
                OnSetCursorStyle(cursor, hotPoint);
            }
        } 
        #endregion
    }
}
