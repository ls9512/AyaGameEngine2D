using System;
using System.Collections.Generic;
using System.Text;

using AyaGameEngine2D;

namespace AcgParkour.Models
{
    /// <summary>
    /// 类      名：AnimationTrans
    /// 功      能：渐变动画类，提供将Texture纹理处理成减半动画的方法，用于场景切换
    /// 日      期：2015-03-24
    /// 修      改：2015-03-24
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class AnimationTrans : IDisposable
    {
        /// <summary>
        /// 渐变纹理
        /// </summary>
        public Texture Texture
        {
            get { return this._texture; }
        }
        private Texture _texture;

        /// <summary>
        /// 纹理宽度
        /// </summary>
        public int Width
        {
            get { return this._width; }
        }
        private int _width;

        /// <summary>
        /// 纹理高度
        /// </summary>
        public int Height
        {
            get { return this._height; }
        }
        private int _height;

        /// <summary>
        /// 标志
        /// ======================================================
        /// 同一场景用到多次渐变效果时，用此标志区分
        /// </summary>
        public string Flag
        {
            get { return this._flag; }
        }
        private string _flag;

        /// <summary>
        /// 透明度
        /// ======================================================
        /// 0 - 255 ,第一次渐变隐去原场景
        /// 255 - 0，第二次渐变显现新场景
        /// 说明：一次绘制结束后再次调用则返回-1，需要Reset()重置后方可再次调用
        /// </summary>
        public float Pellucidity
        {
            get 
            {
                // 结束判断
                if (this._isEnd)
                {
                    return -1;
                }
                // 动画开始条件判断
                if (this._isAnimating == false && this._isNewSence == false && !this._isEnd)
                {
                    this._isAnimating = true;
                    this._pellucidity = 0;
                }
                // 动画中
                if (this._isAnimating == true)
                {
                    if (!this._isNewSence)
                    {
                        this._pellucidity += this._pelluciditySpeed * Time.DeltaTime;
                        // 旧场景结束
                        if (this._pellucidity > 255)
                        {
                            this._pellucidity = 255;
                            this._isNewSence = true;
                            this._pelluciditySpeed *= -1;
                        }
                    }
                    else
                    {
                        this._pellucidity += this._pelluciditySpeed * Time.DeltaTime;
                        // 新场景结束，动画结束
                        if (this._pellucidity < 0)
                        {
                            this._pellucidity = 0;
                            this._isNewSence = true;
                            this._isAnimating = false;
                            this._isEnd = true;
                        }
                    }
                }
                return this._pellucidity; 
            }
            set { this._pellucidity = value; }
        }
        private float _pellucidity;

        /// <summary>
        /// 是否在动画中
        /// ======================================================
        /// 在绘制一帧前调用，以判断是否需要继续绘制
        /// </summary>
        public bool IsAnimating
        {
            get { return this._isAnimating; }
        }
        private bool _isAnimating;

        /// <summary>
        /// 是否显示新场景
        /// ======================================================
        /// 在绘制一帧后调用，以便判断是否需要切换场景
        /// 表示旧场景是否被完全遮盖
        /// </summary>
        public bool IsNewSence
        {
            get { return this._isNewSence; }
        }
        private bool _isNewSence;

        /// <summary>
        /// 结束标志
        /// ======================================================
        /// 相当于IsAnimating == false && IsNewSence == true
        /// 用于判断是否动画结束
        /// 结束后可以销毁该对象
        /// </summary>
        public bool IsEnd
        {
            get { return this._isEnd; }
        }
        private bool _isEnd;

        /// <summary>
        /// 渐变速度
        /// </summary>
        private int _pelluciditySpeed = 5;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="textureMatrix">纹理</param>
        /// <param name="flag">标志</param>
        public AnimationTrans(Texture texture,string flag)
        {
            this._flag = flag;
            this._texture = texture;
            this._width = texture.Width;
            this._height = texture.Height;
            Reset();
        }

        /// <summary>
        /// 重置渐变动画状态
        /// </summary>
        public void Reset()
        {
            this._pellucidity = 0;
            this._isAnimating = false;
            this._isNewSence = false;
            this._isEnd = false;
            this._pelluciditySpeed = 5;
        }

        /// <summary>
        /// 绘制渐变动画帧
        /// ======================================================
        /// 每次在场景绘制之后调用绘制渐变动画遮罩
        /// 注意：当场景切换渐变结束后，调用该函数将直接返回
        ///       如需要重复渐变动画，请调用Reset()重置
        /// </summary>
        public void DrawTransAnimation()
        {
            int pellucidity = (int)this.Pellucidity;
            if (pellucidity < 0) return;
            GraphicHelper.DrawImageTrans(this._texture.TextureID, General.Draw_Rect.Left, General.Draw_Rect.Top, General.Draw_Rect.Width, General.Draw_Rect.Height, pellucidity,this._isNewSence);
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            if (this._texture != null)
            {
                this._texture.Dispose();
            }
        }
    }
}
