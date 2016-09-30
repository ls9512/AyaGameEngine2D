namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：LayerManager
    /// 功      能：层管理器，用于管理游戏中的层机制，可以通过该类以层的形式搜索游戏对象。
    /// 日      期：2016-01-01
    /// 修      改：2016-01-01
    /// 作      者：ls9512
    /// </summary>
    public class LayerManager
    {
        #region 静态实例
        /// <summary>
        /// KeyManager静态实例
        /// </summary>
        public static LayerManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LayerManager();
                return _instance;
            }
        }
        private static LayerManager _instance;
        #endregion

        #region 公有成员
        /// <summary>
        /// 最大层数
        /// </summary>
        public static int MaxLayerNum
        {
            get { return 32; }
        } 
        #endregion

        #region 私有成员
        /// <summary>
        /// 层号数组
        /// </summary>
        private static readonly int[] LayerValue = new int[MaxLayerNum];

        /// <summary>
        /// 层名称数组
        /// </summary>
        private static readonly string[] LayerName = new string[MaxLayerNum];
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        public LayerManager()
        {
            int value = 1;
            // 初始化层号
            for (int i = 0; i < 32; i++)
            {
                LayerValue[i] = value;
                value *= 2;
            }
        } 
        #endregion

        #region 层号 - 索引 转换
        /// <summary>
        /// 层号转换为层索引
        /// </summary>
        /// <param name="value">号</param>
        /// <returns>层索引</returns>
        public static int LayerValueToIndex(int value)
        {
            int index = 0;
            while ((value = value >> 1) != 0)
            {
                index++;
            }
            return index;
        }

        /// <summary>
        /// 层索引转换为层号
        /// </summary>
        /// <param name="index">层索引</param>
        /// <returns>层号(失败返回-1)</returns>
        public static int LayerIndexToValue(int index)
        {
            index = index < 0 ? 0 : index;
            index = index > MaxLayerNum - 1 ? MaxLayerNum - 1 : index;
            return LayerValue[index];
        }
        #endregion

        #region 层号 - 名称 转换
        /// <summary>
        /// 层号转换为层名称
        /// </summary>
        /// <param name="value">层号</param>
        /// <returns>层名称</returns>
        public static string LayerValueToName(int value)
        {
            int index = LayerValueToIndex(value);
            return LayerName[index];
        }

        /// <summary>
        /// 层名称转换为层号
        /// </summary>
        /// <param name="name">层名称</param>
        /// <returns>层号</returns>
        public static int LayerNameToValue(string name)
        {
            int value = -1;
            for (int i = 0; i < MaxLayerNum; i++)
            {
                if (LayerName[i] == name)
                {
                    value = LayerIndexToValue(i);
                    return value;
                }
            }
            return value;
        }
        #endregion

        #region 索引 - 名称 转换
        /// <summary>
        /// 层索引转换为层名称
        /// </summary>
        /// <param name="index">层索引</param>
        /// <returns>层名称</returns>
        public static string LayerIndexToName(int index)
        {
            index = index < 0 ? 0 : index;
            index = index > MaxLayerNum - 1 ? MaxLayerNum - 1 : index;
            return LayerName[index];
        }

        /// <summary>
        /// 层名称转换为层索引
        /// </summary>
        /// <param name="name">层名称</param>
        /// <returns>层索引</returns>
        public static int LayerNameToIndex(string name)
        {
            for (int i = 0; i < MaxLayerNum; i++)
            {
                if (LayerName[i] == name)
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion

        #region 参数设置
        /// <summary>
        /// 设置层名称
        /// </summary>
        /// <param name="index">层索引(0-32)</param>
        /// <param name="name">层名称</param>
        /// <returns>设置结果</returns>
        public static bool SetLayerNameByIndex(int index, string name)
        {
            if (index < 0 || index > MaxLayerNum - 1) return false;
            LayerName[index] = name;
            return false;
        }

        /// <summary>
        /// 设置层名称
        /// </summary>
        /// <param name="value">层号</param>
        /// <param name="name">层名称</param>
        /// <returns>设置结果</returns>
        public static bool SetLayerNameByValue(int value, string name)
        {
            int index = LayerValueToIndex(value);
            LayerName[index] = name;
            return true;
        } 
        #endregion
    }
}
