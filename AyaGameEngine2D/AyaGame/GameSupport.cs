using System;
using System.Collections.Generic;
using System.Drawing;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：GameSupport
    /// 功      能：游戏辅助类，提供区域包含，碰撞检测等常用算法
    /// 日      期：2015-03-21
    /// 修      改：2015-12-20
    /// 作      者：ls9512
    /// </summary>
    public static class GameSupport
    {
        #region 点
        /// <summary>
        /// 计算点到点的距离
        /// </summary>
        /// <param name="point1">点1</param>
        /// <param name="point2">点2</param>
        /// <returns>距离</returns>
        public static float LengthFromPointToPoint(PointF point1, PointF point2)
        {
            return (float)Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        /// <summary>
        /// 点绕指定坐标旋转指定角度
        /// </summary>
        /// <param name="orginalPoint">原始点</param>
        /// <param name="centerPoint">中心坐标</param>
        /// <param name="angle">旋转角度</param>
        /// <returns>旋转后的点</returns>
        public static PointF RotatePoint(PointF orginalPoint, PointF centerPoint, float angle)
        {
            try
            {
                // 与中心点相同则不处理
                if (orginalPoint == centerPoint) return orginalPoint;
                // 旋转后的点
                float rx = 0f, ry = 0f;
                // 中心点
                double xx = centerPoint.X, yy = centerPoint.Y;
                double L = Math.Sqrt(Math.Pow(orginalPoint.X - xx, 2) + Math.Pow(orginalPoint.Y - yy, 2));
                double tanSita = (orginalPoint.Y - yy) * 1.0 / (orginalPoint.X - xx);
                double sita = Math.Atan(tanSita);

                if (orginalPoint.Y > yy && sita < 0) sita += Math.PI;
                if (orginalPoint.Y < yy && sita > 0) sita += Math.PI;
                if (orginalPoint.Y < yy && sita < 0) sita += 2 * Math.PI;
                if (orginalPoint.Y == yy && orginalPoint.X > xx) sita = 0;
                if (orginalPoint.Y == yy && orginalPoint.X < xx) sita = Math.PI;

                rx = (float)(xx + L * Math.Cos(sita - angle * 2 * Math.PI / 360));
                ry = (float)(yy + L * Math.Sin(sita - angle * 2 * Math.PI / 360));

                return new PointF(rx, ry);
            }
            catch
            {
                return new PointF(0, 0);
            }
        }
        #endregion

        #region 线
        /// <summary>
        /// 求点到线的距离
        /// </summary>
        /// <param name="point">点</param>
        /// <param name="lineA">线的端点A</param>
        /// <param name="lineB">线的端点B</param>
        /// <returns>计算结果</returns>
        public static float LengthFromPointToLine(PointF point, PointF lineA, PointF lineB)
        {
            float space = 0;
            float a, b, c;
            // 线段的长度  
            a = LengthFromPointToPoint(lineA, lineB);
            // lineA到点的距离
            b = LengthFromPointToPoint(lineA, point);
            // lineB到点的距离
            c = LengthFromPointToPoint(lineB, point);
            if (c <= 0.000001 || b <= 0.000001)
            {
                space = 0;
                return space;
            }
            if (a <= 0.000001)
            {
                space = b;
                return space;
            }
            if (c * c >= a * a + b * b)
            {
                space = b;
                return space;
            }
            if (b * b >= a * a + c * c)
            {
                space = c;
                return space;
            }
            // 半周长 
            double p = (a + b + c) / 2;
            // 海伦公式求面积  
            double s = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
            // 返回点到线的距离（利用三角形面积公式求高）
            space = (float)(2 * s / a);
            return space;
        }

        /// <summary>
        /// 计算两条直线的交点
        /// </summary>
        /// <param name="p1">L1的点1坐标</param>
        /// <param name="p2">L1的点2坐标</param>
        /// <param name="p3">L2的点1坐标</param>
        /// <param name="p4">L2的点2坐标</param>
        /// <returns></returns>
        static PointF LineaIntersection(PointF p1, PointF p2, PointF p3, PointF p4)
        {
            /*
             * L1，L2都存在斜率的情况：
             * 直线方程L1: ( y - y1 ) / ( y2 - y1 ) = ( x - x1 ) / ( x2 - x1 ) 
             * => y = [ ( y2 - y1 ) / ( x2 - x1 ) ]( x - x1 ) + y1
             * 令 a = ( y2 - y1 ) / ( x2 - x1 )
             * 有 y = a * x - a * x1 + y1   .........1
             * 直线方程L2: ( y - y3 ) / ( y4 - y3 ) = ( x - x3 ) / ( x4 - x3 )
             * 令 b = ( y4 - y3 ) / ( x4 - x3 )
             * 有 y = b * x - b * x3 + y3 ..........2
             * 
             * 如果 a = b，则两直线平等，否则， 联解方程 1,2，得:
             * x = ( a * x1 - b * x3 - y1 + y3 ) / ( a - b )
             * y = a * x - a * x1 + y1
             * 
             * L1存在斜率, L2平行Y轴的情况：
             * x = x3
             * y = a * x3 - a * x1 + y1
             * 
             * L1 平行Y轴，L2存在斜率的情况：
             * x = x1
             * y = b * x - b * x3 + y3
             * 
             * L1与L2都平行Y轴的情况：
             * 如果 x1 = x3，那么L1与L2重合，否则平等
             * 
            */

            float a = 0, b = 0;
            int state = 0;

            if (p1.X != p2.X)
            {
                a = (p2.Y - p1.Y) / (p2.X - p1.X);
                state |= 1;
            }
            if (p3.X != p4.X)
            {
                b = (p4.Y - p3.Y) / (p4.X - p3.X);
                state |= 2;
            }
            switch (state)
            {
                case 0: //L1与L2都平行Y轴
                    {
                        if (p1.X == p3.X)
                        {
                            throw new Exception("两条直线互相重合，且平行于Y轴，无法计算交点。");
                        }
                        else
                        {
                            throw new Exception("两条直线互相平行，且平行于Y轴，无法计算交点。");
                        }
                    }
                case 1: //L1存在斜率, L2平行Y轴
                    {
                        float x = p3.X;
                        float y = a * x - a * p1.X + p1.Y;
                        return new PointF(x, y);
                    }
                case 2: //L1 平行Y轴，L2存在斜率
                    {
                        float x = p1.X;
                        float y = b * x + b * p3.X + p3.Y;
                        return new PointF(x, y);
                    }
                case 3: //L1，L2都存在斜率
                    {
                        if (a == b)
                        {
                            throw new Exception("两条直线平行或重合，无法计算交点。");
                        }
                        float x = (a * p1.X - b * p3.X - p1.Y + p3.Y) / (a - b);
                        float y = a * x - a * p1.X + p1.Y;
                        return new PointF(x, y);
                    }
            }
            throw new Exception("不可能发生的情况");
        }
        #endregion

        #region 矩形
        /// <summary>
        /// 点是否在矩形内
        /// </summary>
        /// <param name="point">点</param>
        /// <param name="rect">矩形</param>
        /// <returns>检测结果</returns>
        public static bool IsPointInRect(PointF point, RectangleF rect)
        {
            bool result = true;
            if (point.X <         rect.Left || point.Y < rect.Top || point.X > rect.Right || point.Y > rect.Bottom) result = false;
            return result;
        }

        /// <summary>
        /// 矩形1是否在矩形2内
        /// </summary>
        /// <param name="rect1">待检测矩形</param>
        /// <param name="rect2">包含矩形</param>
        /// <returns>检测结果</returns>
        public static bool IsRectInRect(RectangleF rect1, RectangleF rect2)
        {
            bool result = false;
            PointF point1 = new PointF(rect1.Left, rect1.Top);
            PointF point2 = new PointF(rect1.Right, rect1.Bottom);
            // 当矩形1对角点都在矩形2内则矩形1在矩形2内
            if (IsPointInRect(point1, rect2) && IsPointInRect(point2, rect2)) result = true;
            return result;
        }

        /// <summary>
        /// 矩形碰撞检测，矩形1和矩形2是否有重叠
        /// </summary>
        /// <param name="rect1">矩形1</param>
        /// <param name="rect2">矩形2</param>
        /// <returns>检测结果</returns>
        public static bool RectHitCheck(RectangleF rect1, RectangleF rect2)
        {
            bool result = false;
            PointF point1 = new PointF(rect1.Left, rect1.Top);
            PointF point2 = new PointF(rect1.Left, rect1.Bottom);
            PointF point3 = new PointF(rect1.Right, rect1.Top);
            PointF point4 = new PointF(rect1.Right, rect1.Bottom);
            // 矩形1的顶点有一个在矩形2内则发生碰撞
            if (IsPointInRect(point1, rect2) || IsPointInRect(point2, rect2) || IsPointInRect(point3, rect2) || IsPointInRect(point4, rect2)) result = true;
            return result;
        }

        /// <summary>
        /// 矩形组合与矩形组合之间是否发生重叠
        /// </summary>
        /// <param name="rect1">矩形数组1</param>
        /// <param name="rect2">矩形数组2</param>
        /// <returns>检测结果</returns>
        public static bool RectHitCheck(RectangleF[] rect1, RectangleF[] rect2)
        {
            bool result = false;
            for (int i = 0; i < rect1.Length; i++)
            {
                for (int j = 0; j < rect2.Length; j++)
                {
                    if (RectHitCheck(rect1[i], rect2[j]))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 矩形组合与矩形组合之间是否发生重叠
        /// </summary>
        /// <param name="rect1">矩形列表1</param>
        /// <param name="rect2">矩形列表2</param>
        /// <returns>检测结果</returns>
        public static bool RectHitCheck(List<RectangleF> rect1, List<RectangleF> rect2)
        {
            bool result = false;
            for (int i = 0; i < rect1.Count; i++)
            {
                for (int j = 0; j < rect2.Count; j++)
                {
                    if (RectHitCheck(rect1[i], rect2[j]))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 矩形1相对于矩形2在某方向上是否发生碰撞
        /// </summary>
        /// <param name="rect1">矩形1</param>
        /// <param name="rect2">矩形2</param>
        /// <param name="direct">方向</param>
        /// <returns>是否碰撞</returns>
        public static bool RectHitCheckByDirection(RectangleF rect1, RectangleF rect2, Direction direct)
        {
            bool isHit = false;
            switch (direct)
            {
                case Direction.Left:
                    if (rect1.Right >= rect2.Left && rect1.Right <= rect2.Right && !(rect1.Bottom <= rect2.Top) && !(rect1.Top >= rect2.Bottom)) isHit = true;
                    break;
                case Direction.Right:
                    if (rect1.Left <= rect2.Right && rect1.Left >= rect2.Left && !(rect1.Bottom <= rect2.Top) && !(rect1.Top >= rect2.Bottom)) isHit = true;
                    break;
                case Direction.Up:
                    if (rect1.Bottom >= rect2.Top && rect1.Bottom <= rect2.Bottom && !(rect1.Left >= rect2.Right) && !(rect1.Right <= rect2.Left)) isHit = true;
                    break;
                case Direction.Down:
                    if (rect1.Top <= rect2.Bottom && rect1.Top >= rect2.Top && !(rect1.Left >= rect2.Right) && !(rect1.Right <= rect2.Left)) isHit = true;
                    break;
            }
            return isHit;
        }

        /// <summary>
        /// 矩形1和矩形2的碰撞方向
        /// </summary>
        /// <param name="rect1">矩形1</param>
        /// <param name="rect2">矩形2</param>
        /// <returns>碰撞方向  非接触碰撞的其他情况返回Direction.None</returns>
        public static Direction RectHitCheckDirection(RectangleF rect1, RectangleF rect2)
        {
            Direction direct = Direction.None;
            if (rect1.Right >= rect2.Left && rect1.Right <= rect2.Right && !(rect1.Bottom <= rect2.Top) && !(rect1.Top >= rect2.Bottom)) direct = Direction.Left;
            if (rect1.Left <= rect2.Right && rect1.Left >= rect2.Left && !(rect1.Bottom <= rect2.Top) && !(rect1.Top >= rect2.Bottom)) direct = Direction.Right;
            if (rect1.Bottom >= rect2.Top && rect1.Bottom <= rect2.Bottom && !(rect1.Left >= rect2.Right) && !(rect1.Right <= rect2.Left)) direct = Direction.Up;
            if (rect1.Top <= rect2.Bottom && rect1.Top >= rect2.Top && !(rect1.Left >= rect2.Right) && !(rect1.Right <= rect2.Left)) direct = Direction.Down;
            return direct;
        } 
        #endregion

        #region 圆形
        /// <summary>
        /// 点是否在圆内
        /// </summary>
        /// <param name="pointCircle">圆心坐标</param>
        /// <param name="r">圆形半径</param>
        /// <param name="point">点T</param>
        /// <returns></returns>
        public static bool IsInCircle(PointF pointCircle, float r, PointF point)
        {
            float length = (float)LengthFromPointToPoint(pointCircle, point);
            if (length <= r)
                return true;
            else
                return false;
        } 
        #endregion

        #region 三角形
        /// <summary>
        /// 点T是否在三角形内
        /// </summary>
        /// <param name="t">点T</param>
        /// <param name="a">三角形A点坐标</param>
        /// <param name="b">三角形B点坐标</param>
        /// <param name="c">三角形C点坐标</param>
        /// <returns></returns>
        public static bool IsInTriangle(PointF t, PointF a, PointF b, PointF c)
        {
            bool temp1 = IsInIntercept(a, b, c, t);
            bool temp2 = IsInIntercept(a, c, b, t);
            bool temp3 = IsInIntercept(b, c, a, t);
            if (temp1 && temp2 && temp3) return true;
            else return false;
        }

        /// <summary>
        /// 点T是否在线AB和C的截距范围内 (三角形碰撞检测辅助方法)
        /// </summary>
        /// <param name="t">点T</param>
        /// <param name="a">线端点A</param>
        /// <param name="b">线端点B</param>
        /// <param name="c">三角形的另一个点C</param>
        /// <returns></returns>
        private static bool IsInIntercept(PointF t, PointF a, PointF b, PointF c)
        {
            // AB延长线在Y轴上的截点，AB过C和T平行线在坐标轴上的截点
            float p1, p2, pt;
            // 斜率不存在时
            if (a.X == b.X)
            {
                p1 = a.X;
                p2 = c.X;
                pt = t.X;
            }
            else
            {
                // 斜率为0时
                if (a.Y == b.Y)
                {
                    p1 = a.Y;
                    p2 = c.Y;
                    pt = t.Y;
                }
                // 斜率不为0时
                else
                {
                    // 斜率
                    float k = (a.Y - b.Y) / (a.X - b.X);
                    float bb = a.Y - k * a.X;
                    // Y轴的截距
                    p1 = bb;
                    p2 = c.Y - k * c.X;
                    pt = t.Y - k * t.X;
                }
            }
            if ((pt <= p2 && pt >= p1) || (pt <= p1 && pt >= p2)) return true;
            else return false;
        } 
        #endregion
    }
}
