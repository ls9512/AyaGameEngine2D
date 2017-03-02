namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：Mathf
    /// 功      能：浮点数学类，重新实现.Net Math类的方法，更适合于浮点运算。
    /// 日      期：2016-01-03
    /// 修      改：2016-01-03
    /// 作      者：ls9512
    /// </summary>
    public class Mathf
    {
        #region 构造方法
        /// <summary>
        /// 构造函数
        /// </summary>
        private Mathf()
        {
        }
        #endregion

        /// <summary>
        /// 自然对数
        /// </summary>
        public static float E = 2.7182818f;

        /// <summary>
        /// 圆周率
        /// </summary>
        public static float PI = 3.1415926f;

		/// <summary>
		/// 限定范围(整形)
		/// </summary>
		/// <param name="value">值</param>
		/// <param name="min">最小</param>
		/// <param name="max">最大</param>
		/// <returns>结果</returns>
		public static int Clamp(int value, int min, int max) {
			value = value < min ? min : value;
			value = value > max ? max : value;
			return value;
		}

		/// <summary>
		/// 限定范围(浮点)
		/// </summary>
		/// <param name="value">值</param>
		/// <param name="min">最小</param>
		/// <param name="max">最大</param>
		/// <returns>结果</returns>
		public static float Clamp(float value, float min, float max) {
			value = value < min ? min : value;
			value = value > max ? max : value;
			return value;
		}
	}

}
