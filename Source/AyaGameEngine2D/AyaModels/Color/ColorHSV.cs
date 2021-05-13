using System;
using System.Drawing;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：ColorHSV
    /// 功      能：H 色相 \ S 饱和度(纯度) \ V 明度 颜色模型 
    /// 日      期：2015-01-22
    /// 修      改：2016-01-03
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public struct ColorHSV
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
        /// 明度
        /// </summary>
        public int V
        {
            get { return _v; }
            set
            {
                _v = value;
                _v = _v > 255 ? 255 : _v;
                _v = _v < 0 ? 0 : _v;
            }
        }
        private int _v; 
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="h">色相</param>
        /// <param name="s">饱和度(纯度)</param>
        /// <param name="v">明度</param>
        public ColorHSV(int h, int s, int v)
        {
            h = h > 360 ? 360 : h;
            h = h < 0 ? 0 : h;
            s = s > 255 ? 255 : s;
            s = s < 0 ? 0 : s;
            v = v > 255 ? 255 : v;
            v = v < 0 ? 0 : v;
            _h = h;
            _s = s;
            _v = v;
        }
        #endregion

        #region 其他方法
        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <returns></returns>
        public Color GetColor()
        {
            ColorRGB color = ColorHelper.HsvToRgb(this);
            return Color.FromArgb(color.R, color.G, color.B);
        } 
        #endregion
    }
}
