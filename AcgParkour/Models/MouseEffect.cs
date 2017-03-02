using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AcgParkour.Models
{
    /// <summary>
    /// 类      名：MouseEffect
    /// 功      能：鼠标效果类
    /// 日      期：2015-11-19
    /// 修      改：2015-11-19
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
            get { return this._pellucidity; }
            set
            {
                this._pellucidity = value;
                if (this._pellucidity > 255) this._pellucidity = 255;
                if (this._pellucidity < 0) this._pellucidity = 0;
            }
        }
        private float _pellucidity;

        /// <summary>
        /// 构造方法
        /// </summary>
        public EffectMouse(Point p)
        {
            Point = p;
            this._pellucidity = 125F;
        }
    }
}
