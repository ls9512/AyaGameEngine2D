using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using AyaGameEngine2D.Models;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：Transform
    /// 功      能：变换组件，提供游戏对象的位置、旋转和缩放信息，并提供相关操作方法。
    /// 说      明：所有游戏对象都必须拥有该组件。
    /// 日      期：2016-01-01
    /// 修      改：2016-01-01
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class Transform : Component
    {
        #region 公有字段
        /// <summary>
        /// 位置
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private Vector2 _position;

        /// <summary>
        /// 旋转
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }
        private float _rotation;

        /// <summary>
        /// 缩放(0 - n)
        /// </summary>
        public Vector2 Scale 
        {
            get { return _scale; }
            set 
            {
                _scale.X = value.X < 0f ? 0f : value.X;
                _scale.Y = value.Y < 0f ? 0f : value.Y;
            }
        }
        private readonly Vector2 _scale;

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
        /// 中心点
        /// </summary>
        public virtual PointF CenterPoint
        {
            get { return new PointF(_position.X + _width / 2, _position.Y + _height / 2); }
        }

        /// <summary>
        /// 所在矩形
        /// </summary>
        public virtual RectangleF Rect
        {
            get { return new RectangleF(_position.X , _position.Y, _width, _height); }
        }

        /// <summary>
        /// 是否绑定父物体
        /// </summary>
        public bool IsBindParent
        {
            get { return _isBindParent; }
            set { _isBindParent = value; }
        }
        private bool _isBindParent;

        /// <summary>
        /// 父物体变换
        /// </summary>
        public Transform Parent = null;

        /// <summary>
        /// 子物体变换
        /// </summary>
        public List<Transform> Child = new List<Transform>();
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        public Transform()
        {
            _position = new Vector2(0, 0);
            _rotation = 0f;
            _scale = new Vector2(1, 1);
        }
        #endregion

        #region 组建引用
        /// <summary>
        /// 设置父对象组件引用
        /// </summary>
        public override void SetParentComponent()
        {
            base.SetParentComponent();
            gameObject.transform = this;
        }
        #endregion

        #region 位移操作
        /// <summary>
        /// 位移
        /// </summary>
        /// <param name="pos">二维向量</param>
        public void Translate(Vector2 pos)
        {
            Translate(pos.X, pos.Y);
        }

        /// <summary>
        /// 位移
        /// </summary>
        /// <param name="x">坐标X</param>
        /// <param name="y">坐标Y</param>
        public void Translate(float x, float y)
        {
            _position.X += x;
            _position.Y += y;
        } 
        #endregion

        #region 旋转操作
        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="angle">角度</param>
        public void Rotate(float angle)
        {
            _rotation += angle;
        } 
        #endregion

        #region 缩放操作
        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="scale">二维向量</param>
        public void Zoon(Vector2 scale)
        {
            Zoon(scale.X, scale.Y);
        }

        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="x">坐标X</param>
        /// <param name="y">坐标Y</param>
        public void Zoon(float x, float y)
        {
            _scale.X += x;
            _scale.Y += y;
        } 
        #endregion
    }
}
