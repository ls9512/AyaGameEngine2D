using System.Drawing;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：DebugDraw
    /// 功      能：调试绘图静态类，用于辅助调试，此处的函数只在调试模式会被执行。
    /// 说      明：该模块仅提供简单的点/线/矩形等图形绘制，且线宽等参数受GraphicHelper中的设置影响
    /// 日      期：2016-01-02
    /// 修      改：2016-01-02
    /// 作      者：ls9512
    /// </summary>
    public static class DebugDraw
    {
        #region 调试绘制
        /// <summary>
        /// 绘制点
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="p">起点</param>
        public static void DrawPoint(Color color, PointF p)
        {
            if (General.Engine_Debug) GraphicHelper.DrawPoint(color, p);
        }

        /// <summary>
        /// 绘制线段
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="p1">起点</param>
        /// <param name="p2">终点</param>
        public static void DrawLine(Color color, PointF p1, PointF p2)
        {
            if (General.Engine_Debug) GraphicHelper.DrawLine(color, p1, p2);
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="rect">矩形</param>
        public static void DrawRectangle(Color color, RectangleF rect)
        {
            if (General.Engine_Debug) GraphicHelper.DrawRectangle(color, rect);
        } 
        #endregion
    }
}
