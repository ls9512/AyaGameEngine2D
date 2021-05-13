using System;
using System.Drawing;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：BaseObject
    /// 功      能：基本模型类，提供常规模型通用的封装
    /// 日      期：2015-03-21
    /// 修      改：2015-12-16
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class BaseObject
    {
        #region 公有成员
        /// <summary>
        /// 坐标X
        /// </summary>
        public virtual float X
        {
            get { return _x; }
            set { _x = value; }
        }
        private float _x;

        /// <summary>
        /// 坐标Y
        /// </summary>
        public virtual float Y
        {
            get { return _y; }
            set { _y = value; }
        }
        private float _y;

        /// <summary>
        /// 横向移动速度
        /// </summary>
        public float MoveX
        {
            get { return _moveX; }
            set { _moveX = value; }
        }
        private float _moveX;

        /// <summary>
        /// 纵向移动速度
        /// </summary>
        public float MoveY
        {
            get { return _moveY; }
            set { _moveY = value; }
        }
        private float _moveY;

        /// <summary>
        /// 宽度
        /// </summary>
        public virtual int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        private int _width;

        /// <summary>
        /// 高度
        /// </summary>
        public virtual int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        private int _height;

        /// <summary>
        /// 价值
        /// </summary>
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
        private int _value;

        /// <summary>
        /// 所在位置
        /// 左上角起点坐标
        /// </summary>
        public virtual PointF Location
        {
            get { return new PointF(_x, _y); }
        }

        /// <summary>
        /// 中心点
        /// </summary>
        public virtual PointF CenterPoint
        {
            get { return new PointF(_x + _width / 2, _y + _height / 2); }
        }

        /// <summary>
        /// 所在矩形
        /// </summary>
        public virtual RectangleF ObjectRect
        {
            get { return new RectangleF(_x, _y, _width, _height); }
        } 
        #endregion
    }
}
