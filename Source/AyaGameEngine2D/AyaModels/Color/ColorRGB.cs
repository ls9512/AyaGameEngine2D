using System;
using System.Drawing;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：ColorRGB
    /// 功      能：R 红色 \ G 绿色 \ B 蓝色 颜色模型
    ///             所有颜色模型的基类，RGB是用于输出到屏幕的颜色模式，所以所有模型都将转换成RGB输出
    /// 日      期：2015-01-22
    /// 修      改：2016-01-03
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public struct ColorRGB
    {
        #region 公有成员
        /// <summary>
        /// 红色
        /// </summary>
        public int R
        {
            get { return _r; }
            set
            {
                _r = value;
                _r = _r > 255 ? 255 : _r;
                _r = _r < 0 ? 0 : _r;
            }
        }
        private int _r;

        /// <summary>
        /// 绿色
        /// </summary>
        public int G
        {
            get { return _g; }
            set
            {
                _g = value;
                _g = _g > 255 ? 255 : _g;
                _g = _g < 0 ? 0 : _g;
            }
        }
        private int _g;

        /// <summary>
        /// 蓝色
        /// </summary>
        public int B
        {
            get { return _b; }
            set
            {
                _b = value;
                _b = _b > 255 ? 255 : _b;
                _b = _b < 0 ? 0 : _b;
            }
        }
        private int _b; 
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="r">红</param>
        /// <param name="g">绿</param>
        /// <param name="b">蓝</param>
        public ColorRGB(int r, int g, int b)
        {
            r = r > 255 ? 255 : r;
            r = r < 0 ? 0 : r;
            g = g > 255 ? 255 : g;
            g = g < 0 ? 0 : g;
            b = b > 255 ? 255 : b;
            b = b < 0 ? 0 : b;
            _r = r;
            _g = g;
            _b = b;
        }
        #endregion

        #region 运算符重载
        /// <summary>
        /// ColorRGB隐式转换为Color
        /// </summary>
        /// <param name="color">RGB颜色</param>
        /// <returns>颜色</returns>
        public static implicit operator Color(ColorRGB color)
        {
            return Color.FromArgb(color.R, color.G, color.B);
        }
        /// <summary>
        /// Color隐式转换为ColorRGB
        /// </summary>
        /// <param name="color">颜色</param>
        /// <returns>RGB颜色</returns>
        public static implicit operator ColorRGB(Color color)
        {
            return new ColorRGB(color.R, color.G, color.B);
        }
        #endregion

        #region 其他方法
        /// <summary>
        /// 获取实际颜色
        /// </summary>
        /// <returns></returns>
        public Color GetColor()
        {
            return Color.FromArgb(_r, _g, _b);
        } 
        #endregion
    } 
}
