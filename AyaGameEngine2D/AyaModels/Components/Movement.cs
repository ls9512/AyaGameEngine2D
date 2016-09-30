using System;
using System.Collections.Generic;
using System.Text;

using AyaGameEngine2D.Models;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：Movement
    /// 功      能：移动组件，提供游戏对象的规则移动和旋转等操作。
    /// 日      期：2016-01-02
    /// 修      改：2016-01-02
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class Movement : Component
    {
        #region 速度
        /// <summary>
        /// X移动速度
        /// </summary>
        public float MoveX
        {
            get { return _moveX; }
            set { _moveX = value; }
        }
        private float _moveX;

        /// <summary>
        /// Y移动速度
        /// </summary>
        public float MoveY
        {
            get { return _moveY; }
            set { _moveY = value; }
        }
        private float _moveY;

        /// <summary>
        /// 角速度
        /// </summary>
        public float AngleSpeed
        {
            get { return _angleSpeed; }
            set { _angleSpeed = value; }
        }
        private float _angleSpeed; 
        #endregion

        #region 加速度
        /// <summary>
        /// X移动速度
        /// </summary>
        public float AccelerationX
        {
            get { return _accelerationX; }
            set { _accelerationX = value; }
        }
        private float _accelerationX;

        /// <summary>
        /// Y移动速度
        /// </summary>
        public float AccelerationY
        {
            get { return _accelerationY; }
            set { _accelerationY = value; }
        }
        private float _accelerationY;

        /// <summary>
        /// 角速度
        /// </summary>
        public float AccelerationAngle
        {
            get { return _accelerationAngle; }
            set { _accelerationAngle = value; }
        }
        private float _accelerationAngle; 
        #endregion

        #region 构造方法
        public Movement()
        {
            
        } 
        #endregion

        #region 组建引用
        /// <summary>
        /// 设置父对象组件引用
        /// </summary>
        public override void SetParentComponent()
        {
            base.SetParentComponent();
            gameObject.movement = this;
        }
        #endregion

        #region 重写更新 - 运动
        /// <summary>
        /// 重写更新，实现自动运动
        /// </summary>
        public override void Update()
        {
            base.Update();
            // 运动
            GetComponent<Transform>().Translate(_moveX, _moveY);
            GetComponent<Transform>().Rotate(_angleSpeed);
            // 提速
            _moveX += _accelerationX;
            _moveY += _accelerationY;
            _angleSpeed += _accelerationAngle;
        } 
        #endregion
    }
}
