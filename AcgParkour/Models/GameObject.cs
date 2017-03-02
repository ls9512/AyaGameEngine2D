using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AcgParkour.Models
{
    /// <summary>
    /// 类      名：GameObject
    /// 功      能：基本模型类，提供常规模型通用的封装
    /// 日      期：2015-03-21
    /// 修      改：2015-12-16
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class GameObject
    {
        /// <summary>
        /// 模型自引用
        /// </summary>
        public static GameObject gameObject;

        /// <summary>
        /// 坐标X
        /// </summary>
        public virtual float X
        {
            get { return this._x; }
            set { this._x = value; }
        }
        private float _x;

        /// <summary>
        /// 坐标Y
        /// </summary>
        public virtual float Y
        {
            get { return this._y; }
            set { this._y = value; }
        }
        private float _y;

        /// <summary>
        /// 横向移动速度
        /// </summary>
        public float MoveX
        {
            get { return this._moveX; }
            set { this._moveX = value; }
        }
        private float _moveX;

        /// <summary>
        /// 纵向移动速度
        /// </summary>
        public float MoveY
        {
            get { return this._moveY; }
            set { this._moveY = value; }
        }
        private float _moveY;

        /// <summary>
        /// 宽度
        /// </summary>
        public virtual int Width
        {
            get { return this._width; }
            set { this._width = value; }
        }
        private int _width;

        /// <summary>
        /// 高度
        /// </summary>
        public virtual int Height
        {
            get { return this._height; }
            set { this._height = value; }
        }
        private int _height;

        /// <summary>
        /// 价值
        /// </summary>
        public int Value
        {
            get { return this._value; }
            set { this._value = value; }
        }
        private int _value;

        /// <summary>
        /// 构造函数
        /// </summary>
        public GameObject()
        {
            gameObject = this;
        }

        /// <summary>
        /// 所在位置
        /// 左上角起点坐标
        /// </summary>
        public virtual PointF Location
        {
            get { return new PointF(this._x, this._y); }
        }

        /// <summary>
        /// 中心点
        /// </summary>
        public virtual PointF CenterPoint
        {
            get { return new PointF(this._x + this._width / 2, this._y + this._height / 2); }
        }

        /// <summary>
        /// 所在矩形
        /// </summary>
        public virtual RectangleF ObjectRect
        {
            get { return new RectangleF(this._x, this._y, this._width, this._height); }
        }
    }
}
