using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：Vector4
    /// 功      能：四维向量
    /// 说      明：在大多数情况下使用二维、三维向量即可，在非必要情况下不建议使用该类。
    /// 日      期：2015-12-30
    /// 修      改：2015-12-30
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class Vector4 : CollectionBase
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

        /// <summary>
        /// Z
        /// </summary>
        public float Z
        {
            get { return _z; }
            set { _z = value; }
        }
        private float _z;

        /// <summary>
        /// W
        /// </summary>
        public float W
        {
            get { return _w; }
            set { _w = value; }
        }
        private float _w;
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法(0,0,0,0)
        /// </summary>
        public Vector4()
        {
            _x = 0;
            _y = 0;
            _z = 0;
            _w = 0;
        }

        /// <summary>
        /// 构造方法(x,y,z,w)
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        /// <param name="w">W</param>
        public Vector4(float x, float y, float z,float w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }
        #endregion

        #region 静态类变量
        /// <summary>
        /// 获取一个四维向量(0,0,0)
        /// </summary>
        public static Vector4 Zero
        {
            get { return new Vector4(0, 0, 0, 0); }
        }

        /// <summary>
        /// 获取一个四维向量(1,1,1)
        /// </summary>
        public static Vector4 One
        {
            get { return new Vector4(1, 1, 1, 1); }
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
                    case 2: return _z;
                    case 3: return _w;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: _x = value; break;
                    case 1: _y = value; break;
                    case 2: _z = value; break;
                    case 4: _w = value; break;
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
            return "(" + _x + "," + _y + "," + _z + "," + _w + ")";
        }

        /// <summary>
        /// 取反
        /// </summary>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static Vector4 operator -(Vector4 rhs)
        {
            return new Vector4(-rhs.X, -rhs.Y, -rhs.Z, -rhs.W);
        }

        /// <summary>
        /// 加法
        /// </summary>
        /// <param name="lhs">左运算量</param>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static Vector4 operator +(Vector4 lhs, Vector4 rhs)
        {
            return new Vector4(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z, lhs.W + rhs.W);
        }

        /// <summary>
        /// 减法
        /// </summary>
        /// <param name="lhs">左运算量</param>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static Vector4 operator -(Vector4 lhs, Vector4 rhs)
        {
            return new Vector4(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z, lhs.W - rhs.W);
        }

        /// <summary>
        /// 乘法
        /// </summary>
        /// <param name="lhs">左运算量</param>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static float operator *(Vector4 lhs, Vector4 rhs)
        {
            return lhs.X * rhs.X + lhs.Y * rhs.Y + lhs.Z * rhs.Z + lhs.W * rhs.W;
        }

        /// <summary>
        /// 乘法
        /// </summary>
        /// <param name="lhs">左运算量</param>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static Vector4 operator *(float lhs, Vector4 rhs)
        {
            return new Vector4(lhs * rhs.X, lhs * rhs.Y, lhs * rhs.Z, lhs * rhs.W);
        }

        /// <summary>
        /// 乘法
        /// </summary>
        /// <param name="lhs">左运算量</param>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static Vector4 operator *(Vector4 lhs, float rhs)
        {
            return new Vector4(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs, lhs.W * rhs);
        }

        /// <summary>
        /// 除法
        /// </summary>
        /// <param name="lhs">左运算量</param>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static Vector4 operator /(Vector4 lhs, float rhs)
        {
            return new Vector4(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs, lhs.W / rhs);
        }

        /// <summary>
        /// 相等
        /// </summary>
        /// <param name="lhs">左运算量</param>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static bool operator ==(Vector4 lhs, Vector4 rhs)
        {
            return lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z && lhs.W == rhs.W;
        }

        /// <summary>
        /// 不等
        /// </summary>
        /// <param name="lhs">左运算量</param>
        /// <param name="rhs">右运算量</param>
        /// <returns>结果</returns>
        public static bool operator !=(Vector4 lhs, Vector4 rhs)
        {
            return lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z || lhs.W != rhs.W;
        }

        /// <summary>
        /// Vector4隐式转换为Vector3
        /// </summary>
        /// <param name="vec">四维向量</param>
        /// <returns>结果</returns>
        public static implicit operator Vector3(Vector4 vec)
        {
            return new Vector3(vec.X, vec.Y, vec.Z);
        }

        /// <summary>
        /// Vector3隐式转换为Vector4
        /// </summary>
        /// <param name="vec">三维向量</param>
        /// <returns>结果</returns>
        public static implicit operator Vector4(Vector3 vec)
        {
            return new Vector4(vec.X, vec.Y, vec.Z, 0);
        }
        #endregion

        #region 其他计算
        /// <summary>
        /// 四维向量长度的平方
        /// </summary>
        public float SqrMagnitude
        {
            get { return _x * _x + _y * _y + _z * _z + _w * _w; }
        }

        /// <summary>
        /// 四维向量的长度
        /// </summary>
        public float Magnitude
        {
            get { return (float)Math.Sqrt((double)SqrMagnitude); }
        }

        /// <summary>
        /// 返回四维向量规范化后的新向量(长度变为1,方向不变)
        /// </summary>
        public Vector4 Normalized
        {
            get
            {
                float length = Magnitude;
                if (length != 0)
                {
                    float x = _x / length;
                    float y = _y / length;
                    float z = _z / length;
                    float w = _w / length;
                    return new Vector4(x, y, z, w);
                }
                else return new Vector4(0, 0, 0, 0);
            }
        }
        #endregion

        #region 功能函数
        /// <summary>
        /// 使向量本身规范化(长度变为1,方向不变)
        /// </summary>
        public void Normalize()
        {
            Vector4 vec = Normalized;
            _x = vec.X;
            _y = vec.Y;
            _z = vec.Z;
            _w = vec.W;
        }
        #endregion

        #region 静态方法
        /// <summary>
        /// 返回向量的长度，最大不超过maxLength所指示的长度
        /// </summary>
        /// <param name="vector">向量</param>
        /// <param name="maxLength">长度</param>
        /// <returns>结果</returns>
        public static float ClampMagnitude(Vector4 vector, float maxLength)
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
        public static float Distance(Vector4 vec1, Vector4 vec2)
        {
            return (vec1 - vec2).Magnitude;
        }

        /// <summary>
        /// 两个向量的点乘积
        /// </summary>
        /// <param name="vec1">向量1</param>
        /// <param name="vec2">向量2</param>
        /// <returns>结果</returns>
        public static float Dot(Vector4 vec1, Vector4 vec2)
        {
            return vec1.X * vec2.X + vec1.Y * vec2.Y + vec1.Z * vec2.Z + vec1.W * vec2.W;
        }

        /// <summary>
        /// 返回两个向量中较大的一个
        /// </summary>
        /// <param name="vec1">向量1</param>
        /// <param name="vec2">向量2</param>
        /// <returns>结果</returns>
        public static Vector4 Max(Vector4 vec1, Vector4 vec2)
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
        public static Vector4 Min(Vector4 vec1, Vector4 vec2)
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
        public static Vector4 Lerp(Vector4 from, Vector4 to, float t)
        {
            t = t < 0 ? 0 : t;
            t = t > 1 ? 1 : t;
            return new Vector4(from.X * (1 - t) + to.X * t, from.Y * (1 - t) + to.Y * t, from.Z * (1 - t) + to.Z * t, from.W * (1 - t) + to.W * t);
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
