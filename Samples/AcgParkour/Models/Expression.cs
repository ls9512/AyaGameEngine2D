using System;
using System.Collections.Generic;
using System.Text;

using AyaGameEngine2D;

using GS = AcgParkour.Game.GameStatus;
using TM = AcgParkour.GameGraphic.TextureManager;
using GH = AyaGameEngine2D.GraphicHelper;

namespace AcgParkour.Models
{
    /// <summary>
    /// 类      名：Expression
    /// 功      能：表情动画类
    /// 日      期：2015-03-27
    /// 修      改：2015-03-27
    /// 作      者：ls9512
    /// </summary>
    public class Expression
    {
        /// <summary>
        /// 表情类型
        /// </summary>
        public ExpressionType Type
        {
            get { return this._type; }
            set { this._type = value; }
        }
        private ExpressionType _type;

        /// <summary>
        /// 当前帧索引
        /// </summary>
        public float NowFrame
        {
            get { return this._nowFrame; }
            set { this._nowFrame = value; }
        }
        private float _nowFrame = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">表情类型</param>
        public Expression(ExpressionType type)
        {
            this._type = type;
            this._nowFrame = 0;
        }

        /// <summary>
        /// 绘制表情
        /// </summary>
        public void DrawExpression()
        {
            if (this._nowFrame < 9)
            {
                float x = GS.GamePlayer.X + GS.GamePlayer.Width / 2;
                float y = GS.GamePlayer.Y - 60;
                float size = 55;
                GH.DrawImage(TM.TextureExpression.TextureID[(int)this._nowFrame, (int)this._type], x, y + GS.GamePlayer.OffestY, size + this._nowFrame * 2, size + this._nowFrame * 2, 225);
                this._nowFrame += 0.1f * Time.DeltaTime;
            }
        }
    }
}
