using System;
using System.Globalization;

namespace AyaGameEngine2D
{
    #region 表达式操作符枚举
    /// <summary>
    /// 类      名：FormulaType
    /// 功      能：表达式操作符枚举
    /// 日      期：2016-01-19
    /// 修      改：2016-01-19
    /// 作      者：ls9512
    /// </summary>
    public enum FormulaType
    {
        /// <summary>
        /// 加号
        /// </summary>
        Add,

        /// <summary>
        /// 减号
        /// </summary>
        Dec,

        /// <summary>
        /// 乘号
        /// </summary>
        Mul,

        /// <summary>
        /// 除号
        /// </summary>
        Div,

        /// <summary>
        /// 正弦
        /// </summary>
        Sin,

        /// <summary>
        /// 余弦
        /// </summary>
        Cos,

        /// <summary>
        /// 正切
        /// </summary>
        Tan,

        /// <summary>
        /// 余切
        /// </summary>
        ATan,

        /// <summary>
        /// 开跟
        /// </summary>
        Sqrt,

        /// <summary>
        /// 求幂
        /// </summary>
        Pow,

        /// <summary>
        /// 无
        /// </summary>
        None,
    }
    #endregion

    /// <summary>
    /// 类      名：Formula
    /// 功      能：表达式计算类，支持加减乘除/括号/三角函数/开根/次方等。
    /// 日      期：2016-01-19
    /// 修      改：2016-01-19
    /// 作      者：ls9512
    /// </summary>
    public static class Formula
    {
        #region 私有方法
        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="strExpression">表达式</param>
        /// <returns>结果</returns>
        private static double CalcExpress(string strExpression)
        {
            while (strExpression.IndexOf("+", StringComparison.Ordinal) != -1 ||
                   strExpression.IndexOf("-", StringComparison.Ordinal) != -1
                   || strExpression.IndexOf("*", StringComparison.Ordinal) != -1 ||
                   strExpression.IndexOf("/", StringComparison.Ordinal) != -1)
            {
                var strTemp = "";
                var strTempB = "";
                var strOne = "";
                var strTwo = "";
                double replaceValue = 0;
                if (strExpression.IndexOf("*", StringComparison.Ordinal) != -1)
                {
                    strTemp = strExpression.Substring(strExpression.IndexOf("*", StringComparison.Ordinal) + 1,
                        strExpression.Length - strExpression.IndexOf("*", StringComparison.Ordinal) - 1);
                    strTempB = strExpression.Substring(0, strExpression.IndexOf("*", StringComparison.Ordinal));
                    strOne = strTempB.Substring(GetPrivorPos(strTempB) + 1, strTempB.Length - GetPrivorPos(strTempB) - 1);

                    strTwo = strTemp.Substring(0, GetNextPos(strTemp));
                    replaceValue = Convert.ToDouble(GetExpType(strOne)) * Convert.ToDouble(GetExpType(strTwo));
                    strExpression = strExpression.Replace(strOne + "*" + strTwo,
                        replaceValue.ToString(CultureInfo.InvariantCulture));
                }
                else if (strExpression.IndexOf("/", StringComparison.Ordinal) != -1)
                {
                    strTemp = strExpression.Substring(strExpression.IndexOf("/", StringComparison.Ordinal) + 1,
                        strExpression.Length - strExpression.IndexOf("/", StringComparison.Ordinal) - 1);
                    strTempB = strExpression.Substring(0, strExpression.IndexOf("/", StringComparison.Ordinal));
                    strOne = strTempB.Substring(GetPrivorPos(strTempB) + 1, strTempB.Length - GetPrivorPos(strTempB) - 1);


                    strTwo = strTemp.Substring(0, GetNextPos(strTemp));
                    replaceValue = Convert.ToDouble(GetExpType(strOne)) / Convert.ToDouble(GetExpType(strTwo));
                    strExpression = strExpression.Replace(strOne + "/" + strTwo,
                        replaceValue.ToString(CultureInfo.InvariantCulture));
                }
                else if (strExpression.IndexOf("+", StringComparison.Ordinal) != -1)
                {
                    strTemp = strExpression.Substring(strExpression.IndexOf("+", StringComparison.Ordinal) + 1,
                        strExpression.Length - strExpression.IndexOf("+", StringComparison.Ordinal) - 1);
                    strTempB = strExpression.Substring(0, strExpression.IndexOf("+", StringComparison.Ordinal));
                    strOne = strTempB.Substring(GetPrivorPos(strTempB) + 1, strTempB.Length - GetPrivorPos(strTempB) - 1);

                    strTwo = strTemp.Substring(0, GetNextPos(strTemp));
                    replaceValue = Convert.ToDouble(GetExpType(strOne)) + Convert.ToDouble(GetExpType(strTwo));
                    strExpression = strExpression.Replace(strOne + "+" + strTwo,
                        replaceValue.ToString(CultureInfo.InvariantCulture));
                }
                else if (strExpression.IndexOf("-") != -1)
                {
                    strTemp = strExpression.Substring(strExpression.IndexOf("-", StringComparison.Ordinal) + 1,
                        strExpression.Length - strExpression.IndexOf("-", StringComparison.Ordinal) - 1);
                    strTempB = strExpression.Substring(0, strExpression.IndexOf("-", StringComparison.Ordinal));
                    strOne = strTempB.Substring(GetPrivorPos(strTempB) + 1, strTempB.Length - GetPrivorPos(strTempB) - 1);


                    strTwo = strTemp.Substring(0, GetNextPos(strTemp));
                    replaceValue = Convert.ToDouble(GetExpType(strOne)) - Convert.ToDouble(GetExpType(strTwo));
                    strExpression = strExpression.Replace(strOne + "-" + strTwo,
                        replaceValue.ToString(CultureInfo.InvariantCulture));
                }
            }
            return Convert.ToDouble(strExpression);
        }

        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="strExpression">表达式</param>
        /// <param name="expressType">类型</param>
        /// <returns>结果</returns>
        private static double CalcExExpress(string strExpression, FormulaType expressType)
        {
            double retValue = 0;
            switch (expressType)
            {
                case FormulaType.Sin:
                    retValue = Math.Sin(Convert.ToDouble(strExpression));
                    break;
                case FormulaType.Cos:
                    retValue = Math.Cos(Convert.ToDouble(strExpression));
                    break;
                case FormulaType.Tan:
                    retValue = Math.Tan(Convert.ToDouble(strExpression));
                    break;
                case FormulaType.ATan:
                    retValue = Math.Atan(Convert.ToDouble(strExpression));
                    break;
                case FormulaType.Sqrt:
                    retValue = Math.Sqrt(Convert.ToDouble(strExpression));
                    break;
                case FormulaType.Pow:
                    retValue = Math.Pow(Convert.ToDouble(strExpression), 2);
                    break;
            }
            if (retValue == 0) return Convert.ToDouble(strExpression);
            return retValue;
        }

        /// <summary>
        /// 获取下一个位置
        /// </summary>
        /// <param name="strExpression">表达式</param>
        /// <returns>位置</returns>
        private static int GetNextPos(string strExpression)
        {
            int[] ExpPos = new int[4];
            ExpPos[0] = strExpression.IndexOf("+", StringComparison.Ordinal);
            ExpPos[1] = strExpression.IndexOf("-", StringComparison.Ordinal);
            ExpPos[2] = strExpression.IndexOf("*", StringComparison.Ordinal);
            ExpPos[3] = strExpression.IndexOf("/", StringComparison.Ordinal);
            int tmpMin = strExpression.Length;
            for (int count = 1; count <= ExpPos.Length; count++)
            {
                if (tmpMin > ExpPos[count - 1] && ExpPos[count - 1] != -1)
                {
                    tmpMin = ExpPos[count - 1];
                }
            }
            return tmpMin;
        }

        /// <summary>
        /// 获取当前位置
        /// </summary>
        /// <param name="strExpression">表达式</param>
        /// <returns>位置</returns>
        private static int GetPrivorPos(string strExpression)
        {
            int[] ExpPos = new int[4];
            ExpPos[0] = strExpression.LastIndexOf("+", StringComparison.Ordinal);
            ExpPos[1] = strExpression.LastIndexOf("-", StringComparison.Ordinal);
            ExpPos[2] = strExpression.LastIndexOf("*", StringComparison.Ordinal);
            ExpPos[3] = strExpression.LastIndexOf("/", StringComparison.Ordinal);
            int tmpMax = -1;
            for (int count = 1; count <= ExpPos.Length; count++)
            {
                if (tmpMax < ExpPos[count - 1] && ExpPos[count - 1] != -1)
                {
                    tmpMax = ExpPos[count - 1];
                }
            }
            return tmpMax;

        }

        /// <summary>
        /// 获取表达式类型
        /// </summary>
        /// <param name="strExpression">表达式</param>
        /// <returns>类型</returns>
        private static string GetExpType(string strExpression)
        {
            strExpression = strExpression.ToUpper();
            if (strExpression.IndexOf("SIN", StringComparison.Ordinal) != -1)
            {
                return
                    CalcExExpress(
                        strExpression.Substring(strExpression.IndexOf("N", StringComparison.Ordinal) + 1,
                            strExpression.Length - 1 - strExpression.IndexOf("N", StringComparison.Ordinal)),
                        FormulaType.Sin).ToString(CultureInfo.InvariantCulture);
            }
            if (strExpression.IndexOf("COS", StringComparison.Ordinal) != -1)
            {
                return
                    CalcExExpress(
                        strExpression.Substring(strExpression.IndexOf("S", StringComparison.Ordinal) + 1,
                            strExpression.Length - 1 - strExpression.IndexOf("S", StringComparison.Ordinal)),
                        FormulaType.Cos).ToString(CultureInfo.InvariantCulture);
            }
            if (strExpression.IndexOf("TAN", StringComparison.Ordinal) != -1)
            {
                return
                    CalcExExpress(
                        strExpression.Substring(strExpression.IndexOf("N", StringComparison.Ordinal) + 1,
                            strExpression.Length - 1 - strExpression.IndexOf("N", StringComparison.Ordinal)),
                        FormulaType.Tan).ToString(CultureInfo.InvariantCulture);
            }
            if (strExpression.IndexOf("ATAN", StringComparison.Ordinal) != -1)
            {
                return
                    CalcExExpress(
                        strExpression.Substring(strExpression.IndexOf("N", StringComparison.Ordinal) + 1,
                            strExpression.Length - 1 - strExpression.IndexOf("N", StringComparison.Ordinal)),
                        FormulaType.ATan).ToString(CultureInfo.InvariantCulture);
            }
            if (strExpression.IndexOf("SQRT", StringComparison.Ordinal) != -1)
            {
                return
                    CalcExExpress(
                        strExpression.Substring(strExpression.IndexOf("T", StringComparison.Ordinal) + 1,
                            strExpression.Length - 1 - strExpression.IndexOf("T", StringComparison.Ordinal)),
                        FormulaType.Sqrt).ToString(CultureInfo.InvariantCulture);
            }
            if (strExpression.IndexOf("POW", StringComparison.Ordinal) != -1)
            {
                return
                    CalcExExpress(
                        strExpression.Substring(strExpression.IndexOf("W", StringComparison.Ordinal) + 1,
                            strExpression.Length - 1 - strExpression.IndexOf("W", StringComparison.Ordinal)),
                        FormulaType.Pow).ToString(CultureInfo.InvariantCulture);
            }
            return strExpression;
        } 
        #endregion

        #region 公有方法
        /// <summary>
        /// 计算表达式的值
        /// </summary>
        /// <param name="strExpression">表达式</param>
        /// <returns>结果</returns>
        public static string CalculateExpress(string strExpression)
        {
            while (strExpression.IndexOf("(", StringComparison.Ordinal) != -1)
            {
                var strTemp = strExpression.Substring(strExpression.LastIndexOf("(", StringComparison.Ordinal) + 1,
                    strExpression.Length - strExpression.LastIndexOf("(", StringComparison.Ordinal) - 1);
                var strExp = strTemp.Substring(0, strTemp.IndexOf(")", StringComparison.Ordinal));
                strExpression = strExpression.Replace("(" + strExp + ")", CalcExpress(strExp).ToString(CultureInfo.InvariantCulture));
            }
            if (strExpression.IndexOf("+", StringComparison.Ordinal) != -1 ||
                strExpression.IndexOf("-", StringComparison.Ordinal) != -1
                || strExpression.IndexOf("*", StringComparison.Ordinal) != -1 ||
                strExpression.IndexOf("/", StringComparison.Ordinal) != -1)
            {
                strExpression = CalcExpress(strExpression).ToString(CultureInfo.InvariantCulture);
            }
            return strExpression;
        } 
        #endregion
    }
}
