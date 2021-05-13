using System;
using System.Collections.Generic;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：Scene
    /// 功      能：游戏场景类
    /// 日      期：2016-01-01
    /// 修      改：2016-01-03
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class Scene
    {
        /// <summary>
        /// 游戏对象列表
        /// </summary>
        private readonly List<GameObject> _gameObjectList;

        /// <summary>
        /// 构造方法
        /// </summary>
        public Scene()
        {
            _gameObjectList = new List<GameObject>();
        }

        /// <summary>
        /// 添加游戏对象到场景
        /// </summary>
        /// <param name="gameObject">游戏对象</param>
        /// <returns>添加对象</returns>
        public GameObject AddGameObject(GameObject gameObject)
        {
            gameObject.Scene = this;
            _gameObjectList.Add(gameObject);
            return gameObject;
        }
    }
}
