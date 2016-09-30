using System;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：LayerMask
    /// 功      能：层蒙板类，可以标志游戏对象所处的层，用于快速搜索对象。
    /// 日      期：2016-01-01
    /// 修      改：2016-01-01
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public struct LayerMask
    {
        #region 公有成员
        /// <summary>
        /// 层号
        /// </summary>
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
        private int _value;

        /// <summary>
        /// 层索引
        /// </summary>
        public int Index
        {
            get { return LayerManager.LayerValueToIndex(_value); }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="index">层索引</param>
        public LayerMask(int index)
        {
            index = index < 0 ? 0 : index;
            index = index > LayerManager.MaxLayerNum ? LayerManager.MaxLayerNum : index;
            _value = LayerManager.LayerIndexToValue(index);
        } 
        #endregion

        #region 运算符重载
        /// <summary>
        /// int隐式转换为LayerMask
        /// </summary>
        /// <param name="index">值(0-31)</param>
        /// <returns>层蒙板</returns>
        public static implicit operator LayerMask(int index)
        {
            if (index < 0) return new LayerMask(0);
            return index > LayerManager.MaxLayerNum - 1 ? new LayerMask(LayerManager.MaxLayerNum - 1) : new LayerMask(index);
        } 
        #endregion
    }
}
