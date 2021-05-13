using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using AyaGameEngine2D.Models;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：GameObject
    /// 功      能：基本游戏对象模型
    /// 说      明：GameObject是一个特殊的组件，
    /// 日      期：2016-01-02
    /// 修      改：2016-01-02
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class GameObject : Component
    {
        #region 公有字段
        /// <summary>
        /// GUID
        /// </summary>
        public string Guid
        {
            get { return _guid; }
        }
        private string _guid;

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        private string _tag;

        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _name;

        /// <summary>
        /// 层蒙板
        /// </summary>
        public LayerMask Layer
        {
            get { return _layer; }
            set { _layer = value; }
        }
        private LayerMask _layer;

        /// <summary>
        /// 所在场景
        /// </summary>
        public Scene Scene
        {
            get { return _scene; }
            set { _scene = value; }
        }
        private Scene _scene;
        #endregion

        #region 组件存取
        /// <summary>
        /// 组件字典
        /// </summary>
        private readonly Dictionary<Type,Component> _components = new Dictionary<Type, Component>();

        /// <summary>
        /// 获取特定类型的组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns>获取结果</returns>
        public override T GetComponent<T>() // where T : Component, new()
        {
            return (T)_components[typeof(T)];
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns>添加结果</returns>
        public override sealed T AddComponent<T>() // where T : Component, new()
        {
            // 创建组件
            T component = new T();
            // 添加组件引用
            component.gameObject = this;
            component.SetParentComponent();
            foreach (var c in _components)
            {
                c.Value.transform = transform;
                c.Value.renderer = renderer;
                c.Value.movement = movement;
                c.Value.script = script;
            }
            // 添加组件到组件字典
            _components.Add(typeof(T), component);
            return component;
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public GameObject()
        {
            // 初始化ID
            _guid = RandomHelper.RandGuid();
            // 初始化引用
            gameObject = this;
            // 添加必要的组件
            AddComponent<Transform>();
            AddComponent<Renderer>();
        } 
        #endregion
    }
}
