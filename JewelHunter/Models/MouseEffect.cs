using System;
using System.Drawing;

namespace JewelHunter.Models
{
    /// <summary>
    /// 类      名：MouseEffect
    /// 功      能：鼠标效果类
    /// 日      期：2015-11-19
    /// 修      改：2016-04-14
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class EffectMouse
    {
        /// <summary>
        /// 坐标
        /// </summary>
        public Point Point { get; set; }
        /// <summary>
        /// 透明度
        /// </summary>
        public float Pellucidity 
        {
            get { return _pellucidity; }
            set
            {
                _pellucidity = value;
                if (_pellucidity > 255) _pellucidity = 255;
                if (_pellucidity < 0) _pellucidity = 0;
            }
        }
        private float _pellucidity;

        /// <summary>
        /// 构造方法
        /// </summary>
        public EffectMouse(Point p)
        {
            Point = p;
            _pellucidity = 125f;
        }
    }
}
