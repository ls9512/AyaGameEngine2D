using System;
using System.Drawing;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：ColorHSL
    /// 功      能：H 色相 \ S 饱和度(纯度) \ L 亮度 颜色模型 
    /// 日      期：2015-02-08
    /// 修      改：2016-01-03
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public struct ColorHSL
    {
        #region 公有成员
        /// <summary>
        /// 色相
        /// </summary>
        public int H
        {
            get { return _h; }
            set
            {
                _h = value;
                _h = _h > 360 ? 360 : _h;
                _h = _h < 0 ? 0 : _h;
            }
        }
        private int _h;

        /// <summary>
        /// 饱和度(纯度)
        /// </summary>
        public int S
        {
            get { return _s; }
            set
            {
                _s = value;
                _s = _s > 255 ? 255 : _s;
                _s = _s < 0 ? 0 : _s;
            }
        }
        private int _s;

        /// <summary>
        /// 亮度
        /// </summary>
        public int L
        {
            get { return _l; }
            set
            {
                _l = value;
                _l = _l > 255 ? 255 : _l;
                _l = _l < 0 ? 0 : _l;
            }
        }
        private int _l; 
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="h">色相</param>
        /// <param name="s">饱和度(纯度)</param>
        /// <param name="l">亮度</param>
        public ColorHSL(int h, int s, int l)
        {
            _h = h;
            _s = s;
            _l = l;
        }
        #endregion

        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <returns></returns>
        public Color GetColor()
        {
            ColorRGB color = ColorHelper.HslToRgb(this);
            return Color.FromArgb(color.R, color.G, color.B);
        }
    }
}
