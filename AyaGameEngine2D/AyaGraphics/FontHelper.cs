using System;
using System.Drawing;
using System.Drawing.Text;

using CsGL.OpenGL;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：FontHelper
    /// 功      能：OpenGL字体绘制类
    /// 日      期：2015-11-19
    /// 修      改：2015-11-22
    /// 作      者：ls9512
    /// </summary>
    public static class FontHelper
    {
        #region 私有字段
        /// <summary>
        /// 暂存交换字体句柄
        /// </summary>
        private static IntPtr _hDC = Win32.GetDC(IntPtr.Zero);
        /// <summary>
        /// 新字体句柄
        /// </summary>
        private static IntPtr _hFont;
        /// <summary>
        /// List长度
        /// </summary>
        private static uint _MAX_CHAR = 255;
        /// <summary>
        /// List编号
        /// </summary>
        private static uint _lists = 1000;
        /// <summary>
        /// 测量字体用的位图
        /// </summary>
        private static Bitmap _bmp = new Bitmap(256, 256);
        /// <summary>
        /// 位图的绘图调用
        /// </summary>
        private static Graphics _graphics = Graphics.FromImage(_bmp);
        /// <summary>
        /// 字体创建标识
        /// </summary>
        private static bool _isFontCreate = false;
        /// <summary>
        /// 字体高度
        /// </summary>
        private static SizeF _fontSzie;
        /// <summary>
        /// 当前字体
        /// </summary>
        private static Font _nowFont = new Font("The New Roman", 24f);
        #endregion

        #region 字体文件
        /// <summary>
        /// 加载字体文件
        /// </summary>
        /// <param name="fontFile">字体文件路径</param>
        /// <param name="fontSize">字体尺寸</param>
        /// <returns>字体</returns>
        public static Font LoadFontFile(string fontFile, float fontSize)
        {
            try
            {
                PrivateFontCollection userFont = new PrivateFontCollection();
                userFont.AddFontFile(fontFile);
                Font font = new Font(userFont.Families[0], fontSize, FontStyle.Regular);
                userFont.Dispose();
                return font;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 设置字体
        /// <summary>
        /// 设置新的绘制字体
        /// ★ 需要消耗系统资源，不宜频繁调用
        /// </summary>
        /// <param name="fontName">字体名称</param>
        /// <param name="fontSize">字号</param>
        /// <returns></returns>
        public static bool SetFont(string fontName, float fontSize)
        {
            try
            {
                // 与当前字体相同则无需创建
                if (_nowFont.Name == fontName && Math.Abs(_nowFont.Size - fontSize) < 1f)
                {
                    return false;
                }
                _nowFont = new Font(fontName, fontSize);
                SizeF s = _graphics.MeasureString("测", _nowFont);
                _fontSzie = s;
                _hFont = Win32.CreateFont((int)Math.Ceiling(_fontSzie.Height),
                                  0,
                                  0,
                                  0,
                                  Win32.FW_DONTCARE,
                                  0,
                                  0,
                                  0,
                                  Win32.DEFAULT_CHARSET,
                                  Win32.OUT_OUTLINE_PRECIS,
                                  Win32.CLIP_DEFAULT_PRECIS,
                                  Win32.CLEARTYPE_QUALITY,
                                  Win32.VARIABLE_PITCH,
                                  _nowFont.Name);
                // 选择字体，得到老字体
                IntPtr hOldFont = Win32.SelectObject(_hDC, _hFont);
                // 删除老字体(用新字体替换老字体)
                bool b = Win32.DeleteObject(hOldFont);
                // 创建标识
                _isFontCreate = true;
                return b;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 绘制字体
        /// <summary>
        /// 输出文字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="color">颜色</param>
        /// <param name="x">坐标x</param>
        /// <param name="y">坐标y</param>
        public static void DrawString(string str, Color color, float x, float y)
        {
            // 未创建字体列表则返回
            if (!_isFontCreate)
            {
                // 创建默认字体
                SetFont("The New Roman", 24f);
            }

            // 禁用2D纹理 ，否则几何绘图将失效
            OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
            // 设置颜色
            OpenGL.glColor4f(color.R * 1f / 255, color.G * 1f / 255, color.B * 1f / 255, color.A * 1f / 255);
            // 坐标处理
            y += _fontSzie.Height - _nowFont.Size + 3;
            // 设置显示位置
            OpenGL.glRasterPos2f(x, y);
            // 获取显示列表
            _lists = OpenGL.glGenLists(1);
            // 绘制显示列表
            for (int i = 0; i < str.Length; i++)
            {
                // 一定要注意这里调用的不一样 
                Win32.wglUseFontBitmapsW(_hDC, (uint)(str[i]), 1, _lists);
                OpenGL.glCallList(_lists);
                // 性能计数
                PerformanceAnalyzer.Gaming_TextureCount++;
            }
            // 恢复颜色
            OpenGL.glColor4f(1f, 1f, 1f, 1f);
            // 恢复2D纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
        } 
        #endregion

        #region 试验方法

        /// <summary>
        /// 只是输出文字，没有字体，不支持中文
        /// </summary>
        /// <param name="str">内容(英文)</param>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        private static void Print(string str, float x, float y)
        {
            // 申请MAX_CHAR个连续的显示列表编号
            _lists = OpenGL.glGenLists((int)_MAX_CHAR);
            // 把每个字符的绘制命令都装到对应的显示列表中
            y = General.Draw_Rect.Height - y;
            bool b = Win32.wglUseFontBitmaps(_hDC, 0, _MAX_CHAR, _lists); 

            OpenGL.glRasterPos2f(x, y);
            for (int i = 0; i < str.Length; i++)
            {
                OpenGL.glCallList((_lists + str[i]));
            }
        }

        /// <summary>
        /// 带字体样式输出，仅英文
        /// </summary>
        /// <param name="str">内容</param>
        /// <param name="font">字体</param>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        private static void StylePrint(string str, Font font, float x, float y)
        {
            Print(str, x, y);
        }

        #endregion
    }
}
