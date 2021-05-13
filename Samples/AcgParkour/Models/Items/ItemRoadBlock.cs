using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using AyaGameEngine2D;

using GS = AcgParkour.Game.GameStatus;
using TM = AcgParkour.GameGraphic.TextureManager;
using SM = AcgParkour.GameIO.SoundManager;
using GH = AyaGameEngine2D.GraphicHelper;

namespace AcgParkour.Models
{
    /// <summary>
    /// 类      名：ItemRoadBlock
    /// 功      能：路障物件类
    /// 日      期：2015-06-23
    /// 修      改：2015-06-23
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class ItemRoadBlock : BaseItem
    {
        /// <summary>
        /// 帧浮动变化标识
        /// </summary>
        private float _flag = 0.5f;

        /// <summary>
        /// 浮动帧 漂浮距离
        /// </summary>
        public float FlowFrame
        {
            get { return this._floaFrame; }
            set { this._floaFrame = value; }
        }
        private float _floaFrame = 0;

        /// <summary>
        /// 重写所在矩形
        /// </summary>
        public override RectangleF ObjectRect
        {
            get
            {
                return new RectangleF(this.X + 4, this.Y + this.FlowFrame, this.Width - 8, this.Height - this.FlowFrame);
            }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        public ItemRoadBlock()
        {
            this.Type = ItemType.RoadBlock;
            this.ItemStatus = ItemStatus.Normal;
        }

        /// <summary>
        /// 重写物件逻辑
        /// </summary>
        public override void ItemLogic()
        {
            // 跟随地图移动
            this.X -= (GS.MoveSpeed + GS.PlayerFlySpeed) * Time.DeltaTime;
            // 浮动
            if (this.ItemStatus != ItemStatus.Attack)
            {
                this._floaFrame += this._flag * Time.DeltaTime;
            }
            if (this._floaFrame < 0 || this._floaFrame > this.Height) this._flag *= -1;
            // 如果与物件发生碰撞
            if (GameSupport.RectHitCheck(this.ObjectRect, GS.GamePlayer.ObjectRect))
            {
                // 路障与玩家碰撞
                if (this.ItemStatus != ItemStatus.Attack && this.ObjectRect.Height > 25)
                {
                    // 更改物件状态，不会再次发生碰撞
                    this.ItemStatus = ItemStatus.Attack;
                    this.FlowFrame = this.Height * 0.7f;
                    // 添加碰撞动画                       
                    TM.AnimationRoadBlockAttack.X = GS.GamePlayer.X + 10;
                    TM.AnimationRoadBlockAttack.Y = GS.GamePlayer.Y + 10;
                    TM.AnimationRoadBlockAttack.NowFrame = 0;
                    TM.AnimationRoadBlockAttack.Reset();
                    GS.AnimationList.Add(TM.AnimationRoadBlockAttack);
                    // 玩家处理
                    GS.GamePlayer.Life--;
                    GS.GamePlayer.AcceleratedSpeed = -10f;
                    // 游戏数据处理
                    GS.Score = (int)(0.95f * GS.Score);
                    GS.Combo = 0;
                    // 添加表情
                    if (GS.GamePlayer.Expression == null)
                    {
                        GS.GamePlayer.Expression = new Expression(ExpressionType.Anxiety);
                    }
                    // 播放音效
                    SM.PlayRoadBlockAttack();
                }
            }
        }

        /// <summary>
        /// 重写物件绘图
        /// </summary>
        public override void ItemGraphic()
        {
            // 不绘制在屏幕外的
            if (!GameSupport.RectHitCheck(this.ObjectRect, General.Draw_Rect))
            {
                return;
            }
            // 路障
            GH.DrawImage(TM.TextureRoadBlockBottom.TextureID, this.X - 7, this.Y + 79 + GS.GamePlayer.OffestY, this.Width + 14, 16);
            GH.DrawImage(TM.TextureRoadBlock.TextureID, this.X, this.Y + this.FlowFrame + GS.GamePlayer.OffestY, this.Width, this.Height - this.FlowFrame);
        }
    }
}
