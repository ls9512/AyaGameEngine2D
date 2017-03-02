using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using AyaGameEngine2D;

using GS = AcgParkour.Game.GameStatus;
using GH = AyaGameEngine2D.GraphicHelper;

namespace AcgParkour.Models
{
    /// <summary>
    /// 类      名：EffectItem
    /// 功      能：效果物件
    /// 日      期：2015-03-26
    /// 修      改：2015-03-26
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class EffectItem : GameObject
    {
        /// <summary>
        /// 标志
        /// </summary>
        public bool Flag
        {
            get { return this._flag; }
            set
            {
                this._flag = value;

            }
        }
        private bool _flag;

        /// <summary>
        /// 旋转角度
        /// </summary>
        public float Angle
        {
            get { return this._angle; }
            set 
            { 
                this._angle = value;
                
            }
        }
        private float _angle;

        /// <summary>
        /// 旋转角速度
        /// </summary>
        public float AngleSpeed
        { 
             get { return this._angleSpeed; }
            set { this._angleSpeed = value; }
        }
        private float _angleSpeed;

        /// <summary>
        /// 效果纹理ID
        /// </summary>
        public uint[] TextureID
        {
            get { return this._textureID; }
            set { this._textureID = value; }
        }
        private uint[] _textureID;

        /// <summary>
        /// 透明度
        /// </summary>
        public float Pellucidity
        {
            get { return this._pellucidity; }
            set { this._pellucidity = value; }
        }
        private float _pellucidity;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EffectItem(uint[] textureID,int widht,int height)
        {
            this._textureID = textureID;
            this.Angle = 0;
            this.AngleSpeed = RandomHelper.RandFloat(-5, 5);
            this.MoveX = RandomHelper.RandFloat(-5, 5);
            this.MoveY = RandomHelper.RandFloat(-5, 5);
            this.Width = widht;
            this.Height = height;
            this._pellucidity = 250;
        }

        /// <summary>
        /// 绘制效果物件
        /// </summary>
        public void DrawEffectItem()
        {
            RectangleF draw_rect;
            if (GS.GamePlayer == null)
            {
                draw_rect = new RectangleF(this.X, this.Y, this.Width, this.Height);
            }
            else
            {
                draw_rect = new RectangleF(this.X, this.Y + GS.GamePlayer.OffestY / 2, this.Width, this.Height);
            }

            GH.DrawImage(this._textureID, draw_rect, this.Angle, (int)this._pellucidity);
            // 显示所在矩形
            if (General.Debug_ShowRect)
            {
                if (GS.GamePlayer == null)
                {
                    GH.DrawRectangle(Color.Gray, this.ObjectRect);
                }
                else
                {
                    GH.DrawRectangle(Color.Gray, this.ObjectRect, 0, GS.GamePlayer.OffestY / 2);
                }
            }

            this.X += this.MoveX * Time.DeltaTime;
            this.Y += this.MoveY * Time.DeltaTime;
            this.Angle += this.AngleSpeed;
            if (this._pellucidity > 0)
            {
                // 在屏幕内才变淡
                if (GameSupport.RectHitCheck(draw_rect, General.Draw_Rect))
                {
                    if (GS.GamePhase == GamePhase.Gaming)
                    {
                        this._pellucidity -= 0.8f * Time.DeltaTime;
                    }
                    else
                    {
                        this._pellucidity -= 1.2f * Time.DeltaTime;
                    }
                }
            }
        }
    }
}
