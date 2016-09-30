using System;
using System.Collections.Generic;
using System.Text;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：RandomHelper
    /// 功      能：随机数类，提供随机数生成，随机序列生成，数组乱序功能
    /// 日      期：2015-11-18
    /// 修      改：2015-11-18
    /// 作      者：ls9512
    /// </summary>
    public static class RandomHelper
    {
        #region 私有字段
        /// <summary>
        /// 随机数发生器静态实例
        /// </summary>
        private static readonly Random Rand = new Random();

        /// <summary>
        /// 数字字母集合
        /// </summary>
        private static readonly string[] Char = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" }; 
        #endregion

        #region 随机数生成
        /// <summary>
        /// 生成一个指定范围的随机整数，该随机数范围包括最小值(不包含最大值)
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static int RandInt(int min, int max)
        {
            return Rand.Next(min, max);
        }

        /// <summary>
        /// 生成一个指定范围的随机浮点数，该随机数范围包括最小值
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static float RandFloat(float min, float max)
        {
            return (float)(min + Rand.NextDouble() * (max - min));
        }

        /// <summary>
        /// 生成一个0.0到1.0的随机小数
        /// </summary>
        public static float RandNumZeroToOne()
        {
            return (float)Rand.NextDouble();
        } 
        #endregion

        #region 随机数字集合生成
        /// <summary>
        /// 生成随机数字集合
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="length">长度</param>
        /// <returns>结果</returns>
        public static List<int> RandIntList(int min, int max, int length)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < length; i++)
            {
                list.Add(RandInt(min, max + 1));
            }
            return list;
        }

        /// <summary>
        /// 生成随机数组
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="length">长度</param>
        /// <returns>结果</returns>
        public static int[] RandIntArray(int min, int max, int length)
        {
            int[] list = new int[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = RandInt(min, max + 1);
            }
            return list;
        } 
        #endregion

        #region 随即字符串生成
        /// <summary>
        /// 生成随机数字字符串
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns>结果</returns>
        public static string RandNumString(int length)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                str.Append(Char[RandInt(0, 10)]);
            }
            return str.ToString();
        }

        /// <summary>
        /// 生成随机英文数字混合字符串
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="randUpOrLow">是否随机大小写(false则全部小写)</param>
        /// <returns>结果</returns>
        public static string RandNumAndCharString(int length, bool randUpOrLow)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int index = RandInt(0, 36);
                string str_temp = Char[index];
                // 1/2概率转换为大写
                if (index > 9 && randUpOrLow == true && RandInt(0, 10) > 4)
                {
                    str_temp = str_temp.ToUpper();
                }
                str.Append(str_temp);
            }
            return str.ToString();
        }

        /// <summary>
        /// 生成随机GUID
        /// </summary>
        /// <returns>结果</returns>
        public static string RandGuid()
        {
            return Guid.NewGuid().ToString();
        } 
        #endregion

        #region 随机排序
        /// <summary>
        /// 随机打乱字符串内容(不支持中文)
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>结果</returns>
        public static string StringToRand(string str)
        {
            char[] c = str.ToCharArray();
            // 交换的次数,这里使用数组的长度作为交换次数
            int count = str.Length * 2;
            // 循环交换
            for (int i = 0; i < count; i++)
            {
                //生成两个随机数位置
                int randomNum1 = RandInt(0, str.Length);
                int randomNum2 = RandInt(0, str.Length);
                //交换两个随机数位置的值
                var temp = c[randomNum1];
                c[randomNum1] = c[randomNum2];
                c[randomNum2] = temp;
            }
            return new string(c);
        }

        /// <summary>
        /// 随机打乱数组顺序
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="array">数组</param>
        public static void ArrayToRand<T>(T[] array)
        {
            // 交换的次数,这里使用数组的长度作为交换次数
            int count = array.Length * 2;
            // 开始交换
            for (int i = 0; i < count; i++)
            {
                // 生成两个随机数位置
                int randomNum1 = RandInt(0, array.Length);
                int randomNum2 = RandInt(0, array.Length);
                // 交换两个随机数位置的值
                var temp = array[randomNum1];
                array[randomNum1] = array[randomNum2];
                array[randomNum2] = temp;
            }
        }

        /// <summary>
        /// 随机打乱集合顺序
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="list">数组</param>
        public static void ListToRand<T>(List<T> list)
        {
            // 交换的次数,这里使用数组的长度作为交换次数
            int count = list.Count * 2;
            // 开始交换
            for (int i = 0; i < count; i++)
            {
                // 生成随机数位置
                int randomNum = RandInt(0, list.Count);
                // 取出某个元素并放置到末尾
                list.Remove(list[randomNum]);
                list.Add(list[randomNum]);
            }
        } 
        #endregion
    }
}
