using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：Input
    /// 功      能：输入静态类，用于快速调用输入状态
    ///             用户可以自行调用KeyManager和MouseManager查询也可由此接口查询
    /// 日      期：2015-11-18
    /// 修      改：2015-11-20
    /// 作      者：ls9512
    /// </summary>
    public static class Input
    {
        #region 输入管理实例调用
        /// <summary>
        /// 键盘管理实例调用
        /// </summary>
        private static readonly KeyManager Km = KeyManager.Instance;

        /// <summary>
        /// 鼠标管理实例调用
        /// </summary>
        private static readonly MouseManager Mm = MouseManager.Instance; 
        #endregion

        #region 鼠标
        /// <summary>
        /// 鼠标左键是否按下
        /// </summary>
        public static bool IsMouseLeft
        {
            get { return Mm.LeftPressed; }
        }

        /// <summary>
        /// 鼠标右键是否按下
        /// </summary>
        public static bool IsMouseRight
        {
            get { return Mm.RightPressed; }
        }

        /// <summary>
        /// 鼠标滚轮是否按下
        /// </summary>
        public static bool IsMouseMiddle
        {
            get { return Mm.MiddlePressed; }
        }

        /// <summary>
        /// 鼠标左键是否按住
        /// </summary>
        public static bool IsMouseLeftHeld
        {
            get { return Mm.LeftHeld; }
        }

        /// <summary>
        /// 鼠标右键是否按住
        /// </summary>
        public static bool IsMouseRightHeld
        {
            get { return Mm.RightHeld; }
        }

        /// <summary>
        /// 鼠标滚轮是否按住
        /// </summary>
        public static bool IsMouseMiddleHeld
        {
            get { return Mm.MiddleHeld; }
        }

        /// <summary>
        /// 鼠标左键是否抬起
        /// </summary>
        public static bool IsMouseLeftUp
        {
            get { return Mm.LeftUp; }
        }

        /// <summary>
        /// 鼠标右键是否抬起
        /// </summary>
        public static bool IsMouseRightUp
        {
            get { return Mm.RightUp; }
        }

        /// <summary>
        /// 鼠标滚轮是否抬起
        /// </summary>
        public static bool IsMouseMiddleUp
        {
            get { return Mm.MiddleUp; }
        }

        /// <summary>
        /// 鼠标是否在某个区域内
        /// </summary>
        /// <param name="rect">区域s</param>
        /// <returns>结果</returns>
        public static bool IsMouseOn(RectangleF rect)
        {
            return GameSupport.IsPointInRect(MousePosition, rect);
        }

        /// <summary>
        /// 当前鼠标位置
        /// </summary>
        public static Point MousePosition
        {
            get { return Mm.Position; }
        }

        /// <summary>
        /// 当前鼠标绘制位置(计算窗口缩放)
        /// </summary>
        public static Point MouseDrawPosition
        {
            get { return Mm.MouseDrawPosition; }
        } 
        #endregion

        #region 键盘
        /// <summary>
        /// 按键是否按下
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>查询结果</returns>
        public static bool IsKeyPressed(Keys key)
        {
            return Km.IsKeyPressed(key);
        }

        /// <summary>
        /// 按键是否持续按下
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>查询结果</returns>
        public static bool IsKeyHeld(Keys key)
        {
            return Km.IsKeyHeld(key);
        }

        /// <summary>
        /// 查询一组按键是否同时按下
        /// </summary>
        /// <param name="key1">按键1</param>
        /// <param name="key2">按键2</param>
        /// <returns>查询结果</returns>
        public static bool IsKeysPressed(Keys key1, Keys key2)
        {
            return Km.IsKeysPressed(key1, key2);
        }

        /// <summary>
        /// 查询一组按键是否按下
        /// </summary>
        /// <param name="keys">按键列表</param>
        /// <returns>查询结果</returns>
        public static bool IsKeysPressed(List<Keys> keys)
        {
            return Km.IsKeysPressed(keys);
        }

        /// <summary>
        /// 查询一组按键是否同时按住
        /// </summary>
        /// <param name="key1">按键1</param>
        /// <param name="key2">按键2</param>
        /// <returns>查询结果</returns>
        public static bool IsKeysHeld(Keys key1, Keys key2)
        {
            return Km.IsKeysHeld(key1, key2);
        }

        /// <summary>
        /// 查询一组按键是否按住
        /// </summary>
        /// <param name="keys">按键列表</param>
        /// <returns>查询结果</returns>
        public static bool IsKeysHeld(List<Keys> keys)
        {
            return Km.IsKeysHeld(keys);
        }

        /// <summary>
        /// 强制释放按键
        /// </summary>
        /// <param name="key">键值</param>
        public static void ReleaseKey(Keys key)
        {
            Km.Release(key);
        }
        
        /// <summary>
        /// 是否按下任意键
        /// </summary>
        public static bool IsAnyKey
        {
            get
            {
                return Km.IsAnyKey();
            }
        }

        /// <summary>
        /// 按键是否未按下
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>查询结果</returns>
        public static bool IsKeyUp(Keys key)
        {
            return Km.IsKeyUp(key);
        } 
        #endregion
    }
}
