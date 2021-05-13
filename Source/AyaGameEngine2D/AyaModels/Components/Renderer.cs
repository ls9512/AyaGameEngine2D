using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using AyaGameEngine2D.Models;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：Renderer
    /// 功      能：渲染组件，提供对纹理的渲染支持。
    /// 说      明：缺少该组件的游戏对象将不会被显示。
    /// 日      期：2016-01-02
    /// 修      改：2016-01-02
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class Renderer : Component
    {
        #region 公有字段
        /// <summary>
        /// 渲染纹理
        /// </summary>
        public Texture Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }
        private Texture _texture;

        /// <summary>
        /// 渲染颜色
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        private Color _color;
        #endregion

        #region 构造方法
        public Renderer()
        {     
        } 
        #endregion

        #region 组建引用
        /// <summary>
        /// 设置父对象组件引用
        /// </summary>
        public override void SetParentComponent()
        {
            base.SetParentComponent();
            gameObject.renderer = this;
        }
        #endregion

        #region 重写更新 - 渲染
        /// <summary>
        /// 重写更新，实现渲染游戏对象
        /// </summary>
        public override void Update()
        {
            base.Update();
            // Vector2 pos = GetComponent<Transform>().Position;
            RectangleF rect = GetComponent<Transform>().Rect;
            float rotate = GetComponent<Transform>().Rotation;
            GraphicHelper.DrawImage(_texture.TextureID, rect, rotate, _color.A);
        } 
        #endregion
    }
}
