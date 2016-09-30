using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AyaGameEngine2D 
{
    /// <summary>
    /// 类      名：Vector2
    /// 功      能：二维向量
    /// 日      期：2015-12-28
    /// 修      改：2015-12-28
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class Vector2 : CollectionBase
    {
        #region 共有成员
        /// <summary>
        /// X
        /// </summary>
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }
        private float _x;

        /// <summary>
        /// Y
        /// </summary>
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }
        private float _y;
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法(0,0)
        /// </summary>
        public Vector2()
        {
            _x = 0;
            _y = 0;
        }

        /// <summary>
        /// 构造方法(x,y)
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public Vector2(float x, float y)
        {
            _x = x;
            _y = y;
        }
        #endregion

        #region 静态类变量
        /// <summary>
        /// 获取一个二维向量(0,0)
        /// </summary>
        public static Vector2 Zero
        {
            get { return new Vector2(0, 0); }
        }

        /// <summary>
        /// 获取一个二维向量(1,1)
        /// </summary>
        public static Vector2 One
        {
            get { return new Vector2(1, 1); }
        }

        /// <summary>
        /// 获取一个二维向量(0,1)
        /// </summary>
        public static Vector2 Up
        {
            get { return new Vector2(0, 1); }
        }

        /// <summary>
        /// 获取一个二维向量(1,0)
        /// </summary>
        public static Vector2 Right
        {
            get { return new Vector2(1, 0); }
        }
        #endregion

        #region 索引器
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index">下标</param>
        /// <returns>第index个组件的值</returns>
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return _x;
                    case 1: return _y;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: _x = value; break;
                    case 1: _y = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }
        #endregion

        #region 运算符重载
        /// <summary>
        /// 获取格式化字符串
        /// </summary>
        /// <returns>结果</returns>
        public override string ToString()
        {
            return "(" + _x + "," + _y + ")";
        }

        /// <summary>
        /// 取反
        /// </summary>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static Vector2 operator -(Vector2 rhs)
        {
            return new Vector2(-rhs.X, -rhs.Y);
        }

        /// <summary>
        /// 加法
        /// </summary>
        /// <param name="lhs">左运算量</param>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        /// <summary>
        /// 减法
        /// </summary>
        /// <param name="lhs">左运算量</param>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }

        /// <summary>
        /// 乘法
        /// </summary>
        /// <param name="lhs">左运算量</param>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static float operator *(Vector2 lhs, Vector2 rhs)
        {
            return lhs.X * rhs.X + lhs.Y * rhs.Y;
        }

        /// <summary>
        /// 乘法
        /// </summary>
        /// <param name="lhs">左运算量</param>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static Vector2 operator *(float lhs, Vector2 rhs)
        {
            return new Vector2(lhs * rhs.X, lhs * rhs.Y);
        }

        /// <summary>
        /// 乘法
        /// </summary>
        /// <param name="lhs">左运算量</param>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static Vector2 operator *(Vector2 lhs, float rhs)
        {
            return new Vector2(lhs.X * rhs, lhs.Y * rhs);
        }

        /// <summary>
        /// 除法
        /// </summary>
        /// <param name="lhs">左运算量</param>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static Vector2 operator /(Vector2 lhs, float rhs)
        {
            return new Vector2(lhs.X / rhs, lhs.Y / rhs);
        }

        /// <summary>
        /// 相等
        /// </summary>
        /// <param name="lhs">左运算量</param>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static bool operator ==(Vector2 lhs, Vector2 rhs)
        {
            return lhs.X == rhs.X && lhs.Y == rhs.Y;
        }

        /// <summary>
        /// 不等
        /// </summary>
        /// <param name="lhs">左运算量</param>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return lhs.X != rhs.X || lhs.Y != rhs.Y;
        }

        /// <summary>
        /// Vector2隐式转换为PointF
        /// </summary>
        /// <param name="vec">二维向量</param>
        /// <returns>结果</returns>
        public static implicit operator PointF(Vector2 vec)
        {
            return new PointF(vec.X,vec.Y);
        }

        /// <summary>
        /// PointF隐式转换为Vector2
        /// </summary>
        /// <param name="p">点</param>
        /// <returns>结果</returns>
        public static implicit operator Vector2(PointF p)
        {
            return new Vector2(p.X, p.Y);
        }

        /// <summary>
        /// Vector2隐式转换为SizeF
        /// </summary>
        /// <param name="vec">二维向量</param>
        /// <returns>结果</returns>
        public static implicit operator SizeF(Vector2 vec)
        {
            return new SizeF(vec.X, vec.Y);
        }

        /// <summary>
        /// SizeF隐式转换为Vector2
        /// </summary>
        /// <param name="size">尺寸</param>
        /// <returns>结果</returns>
        public static implicit operator Vector2(SizeF size)
        {
            return new Vector2(size.Width, size.Height);
        }

        /// <summary>
        /// Vector3隐式转换为Vector2
        /// </summary>
        /// <param name="vec">三维向量</param>
        /// <returns>结果</returns>
        public static implicit operator Vector2(Vector3 vec)
        {
            return new Vector2(vec.X, vec.Y);
        }

        /// <summary>
        /// Vector2隐式转换为Vector3
        /// </summary>
        /// <param name="vec">二维向量</param>
        /// <returns>结果</returns>
        public static implicit operator Vector3(Vector2 vec)
        {
            return new Vector3(vec.X, vec.Y, 0);
        }
        #endregion

        #region 其他计算
        /// <summary>
        /// 二维向量长度的平方
        /// </summary>
        public float SqrMagnitude
        {
            get { return _x * _x + _y * _y; }
        }

        /// <summary>
        /// 二维向量的长度
        /// </summary>
        public float Magnitude
        {
            get { return (float)Math.Sqrt((double)SqrMagnitude); }
        }

        /// <summary>
        /// 返回二维向量规范化后的新向量(长度变为1,方向不变)
        /// </summary>
        public Vector2 Normalized
        {
            get
            {
                float length = Magnitude;
                if (length != 0)
                {
                    float x = _x / length;
                    float y = _y / length;
                    return new Vector2(x, y);
                }
                else return new Vector2(0, 0);
            }
        } 
        #endregion

        #region 功能函数
        /// <summary>
        /// 使向量本身规范化(长度变为1,方向不变)
        /// </summary>
        public void Normalize()
        {
            Vector2 vec = Normalized;
            _x = vec.X;
            _y = vec.Y;
        }
        #endregion

        #region 静态方法
        /// <summary>
        /// 返回向量的长度，最大不超过maxLength所指示的长度
        /// </summary>
        /// <param name="vector">向量</param>
        /// <param name="maxLength">长度</param>
        /// <returns>结果</returns>
        public static float ClampMagnitude(Vector2 vector, float maxLength)
        {
            float length = vector.Magnitude;
            return length > maxLength ? maxLength : length;
        }

        /// <summary>
        /// 返回两个向量之间的距离。
        /// </summary>
        /// <param name="vec1">向量1</param>
        /// <param name="vec2">向量2</param>
        /// <returns>长度</returns>
        public static float Distance(Vector2 vec1, Vector2 vec2)
        {
            return (vec1 - vec2).Magnitude;
        }

        /// <summary>
        /// 两个向量的点乘积
        /// </summary>
        /// <param name="vec1">向量1</param>
        /// <param name="vec2">向量2</param>
        /// <returns>结果</returns>
        public static float Dot(Vector2 vec1, Vector2 vec2)
        {
            return vec1.X * vec2.X + vec1.Y * vec2.Y;
        } 

        /// <summary>
        /// 返回两个向量中较大的一个
        /// </summary>
        /// <param name="vec1">向量1</param>
        /// <param name="vec2">向量2</param>
        /// <returns>结果</returns>
        public static Vector2 Max(Vector2 vec1, Vector2 vec2)
        {
            float len1 = vec1.Magnitude;
            float len2 = vec2.Magnitude;
            return len1 > len2 ? vec1 : vec2;
        }

        /// <summary>
        /// 返回两个向量中较小的一个
        /// </summary>
        /// <param name="vec1">向量1</param>
        /// <param name="vec2">向量2</param>
        /// <returns>结果</returns>
        public static Vector2 Min(Vector2 vec1, Vector2 vec2)
        {
            float len1 = vec1.Magnitude;
            float len2 = vec2.Magnitude;
            return len1 < len2 ? vec1 : vec2;
        }

        /// <summary>
        /// 返回两个向量中的一个线性插值中间向量
        /// </summary>
        /// <param name="from">起点向量</param>
        /// <param name="to">终点向量</param>
        /// <param name="t">0-1</param>
        /// <returns>结果</returns>
        public static Vector2 Lerp(Vector2 from, Vector2 to,float t)
        {
            t = t < 0 ? 0 : t;
            t = t > 1 ? 1 : t;
            return new Vector2(from.X * (1 - t) + to.X * t, from.Y * (1 - t) + to.Y * t);
        }
        #endregion

        #region 必要的重写
        /// <summary>
        /// 哈希函数
        /// </summary>
        /// <returns>哈希值</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>结果</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        #endregion
    }
}
