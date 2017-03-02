using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using AyaGameEngine2D;

namespace AcgParkour.Models
{
    /// <summary>
    /// 类      名：BaseItem
    /// 功      能：游戏基础物件类
    /// 日      期：2015-03-21
    /// 修      改：2015-06-23
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public abstract class BaseItem : GameObject
    {
        /// <summary>
        /// 物件状态
        /// </summary>
        public ItemStatus ItemStatus
        {
            get { return this._itemStatus; }
            set { this._itemStatus = value; }
        }
        private ItemStatus _itemStatus;

        /// <summary>
        /// 帧索引 物件类型
        /// </summary>
        public int Index
        {
            get { return this._index; }
            set { this._index = value; }
        }
        private int _index;

        /// <summary>
        /// 物件类型
        /// </summary>
        public ItemType Type
        {
            get { return this._type; }
            set { this._type = value; }
        }
        private ItemType _type;

        /// <summary>
        /// 构造方法
        /// </summary>
        public BaseItem()
        { 
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="type">物件类型</param>
        public BaseItem(ItemType type)
        {
            this._type = type;
            this._itemStatus = ItemStatus.Normal;
        }

        /// <summary>
        /// 物件逻辑，必须被重写
        /// </summary>
        public abstract void ItemLogic();

        /// <summary>
        /// 物件绘图，必须被重写
        /// </summary>
        public abstract void ItemGraphic();
    }
}
