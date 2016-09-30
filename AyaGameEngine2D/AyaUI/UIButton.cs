using System;
using System.Drawing;

using GH = AyaGameEngine2D.GraphicHelper;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：UIButton
    /// 功      能：游戏UI按钮类
    /// 说      明：按钮选中状态下会放大
    ///             按钮逻辑和绘图修饰由用户自行完成
    /// 日      期：2015-11-12
    /// 修      改：2015-06-25
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public abstract class UIButton : BaseUI
    {
        #region 私有成员
        /// <summary>
        /// 按钮浮动参数
        /// </summary>
        private float _zoom = 0;
        private float _zoomMax = 7;
        private float _zoomSpeed = 0.5f;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x">坐标x</param>
        /// <param name="y">坐标y</param>
        /// <param name="texture">纹理</param>
        public UIButton(int x, int y, Texture texture)
        {
            X = x;
            Y = y;
            Texture = texture;
        }
        #endregion

        #region 重写逻辑
        /// <summary>
        /// 重写UI逻辑
        /// </summary>
        public override void UILogic()
        {

        }
        #endregion

        #region 重写绘图
        /// <summary>
        /// 重写UI绘图
        /// </summary>
        public override void UIGraphic()
        {
            if (Enable)
            {
                switch (UIStatus)
                {
                    case UIStatus.Normal:
                        if (_zoom > 0) _zoom -= _zoomSpeed * Time.DeltaTimeUnScale;
                        GH.DrawImage(Texture.TextureID, X - _zoom, Y - _zoom, Width + _zoom * 2, Height + _zoom * 2);
                        break;
                    case UIStatus.MouseOn:
                        if (_zoom < _zoomMax) _zoom += _zoomSpeed * Time.DeltaTimeUnScale;
                        GH.DrawImage(Texture.TextureID, X - _zoom, Y - _zoom, Width + _zoom * 2, Height + _zoom * 2);
                        break;
                    case UIStatus.MouseDown:
                        if (_zoom < _zoomMax) _zoom += _zoomSpeed * Time.DeltaTimeUnScale;
                        GH.DrawImage(Texture.TextureID, X - _zoom, Y - _zoom, Width + _zoom * 2, Height + _zoom * 2, Color.Gray);
                        break;
                    case UIStatus.MouseClick:
                        if (_zoom < _zoomMax) _zoom += _zoomSpeed * Time.DeltaTimeUnScale;
                        GH.DrawImage(Texture.TextureID, X - _zoom, Y - _zoom, Width + _zoom * 2, Height + _zoom * 2, Color.DimGray);
                        break;
                    case UIStatus.MouseUp:
                        if (_zoom < _zoomMax) _zoom += _zoomSpeed * Time.DeltaTimeUnScale;
                        GH.DrawImage(Texture.TextureID, X - _zoom, Y - _zoom, Width + _zoom * 2, Height + _zoom * 2);
                        break;
                }
            }
            else
            {
                GH.DrawImage(Texture.TextureID, X - _zoom, Y - _zoom, Width + _zoom * 2, Height + _zoom * 2, Color.DimGray);
            }
        } 
        #endregion
    }
}
