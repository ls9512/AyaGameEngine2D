using System;
using System.Collections.Generic;
using System.Text;

using AyaGameEngine2D;

namespace AyaGameEngine2D.Models
{
    /// <summary>
    /// 类      名：Component
    /// 功      能：组件基类，所有游戏对象组件通过继承该类实现共有特性
    /// 日      期：2016-01-01
    /// 修      改：2016-01-03
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class Component
    {
        #region 组建引用
        /// <summary>
        /// 自引用
        /// </summary>
        public Component self
        {
            get { return _self; }
            set { _self = value; }
        }
        private Component _self;
        
        /// <summary>
        /// 对象引用
        /// </summary>
        public GameObject gameObject
        {
            get { return _gameObject; }
            set { _gameObject = value; }
        }
        private GameObject _gameObject;

        /// <summary>
        /// 变换引用
        /// </summary>
        public Transform transform
        {
            get { return _transform; }
            set { _transform = value; }
        }
        private Transform _transform;

        /// <summary>
        /// 渲染引用
        /// </summary>
        public Renderer renderer
        {
            get { return _renderer; }
            set { _renderer = value; }
        }
        private Renderer _renderer;

        /// <summary>
        /// 移动引用
        /// </summary>
        public Movement movement
        {
            get { return _movement; }
            set { _movement = value; }
        }
        private Movement _movement;

        /// <summary>
        /// 脚本引用
        /// </summary>
        public Script script
        {
            get { return _script; }
            set { _script = value; }
        }
        private Script _script;
        #endregion

        #region 组件存取
        /// <summary>
        /// 获取特定类型的组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns>获取结果</returns>
        public virtual T GetComponent<T>() where T : Component, new()
        {
            if (gameObject != null) return gameObject.GetComponent<T>();
            else return null;
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns>添加结果</returns>
        public virtual T AddComponent<T>() where T : Component, new()
        {
            if (gameObject != null) return gameObject.AddComponent<T>();
            return null;
        }

        /// <summary>
        /// 设置父对象组件引用
        /// </summary>
        public virtual void SetParentComponent()
        {
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        public Component()
        {
            self = this;
        } 
        #endregion

        #region 组件生命周期 (需要被重写以实现具体功能)
        /// <summary>
        /// 唤醒
        /// </summary>
        public virtual void Awake()
        {
        }

        /// <summary>
        /// 生效
        /// </summary>
        public virtual void OnEnbale()
        {
        }

        /// <summary>
        /// 开始
        /// </summary>
        public virtual void Start()
        {
        }

        /// <summary>
        /// 物理更新
        /// </summary>
        public virtual void FixedUpdate()
        {
        }

        /// <summary>
        /// 帧更新
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// 延时更新
        /// </summary>
        public virtual void LateUpdate()
        {
        }

        /// <summary>
        /// 失效
        /// </summary>
        public virtual void OnDisable()
        {
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void OnDestory()
        {
        } 
        #endregion
    }
}
