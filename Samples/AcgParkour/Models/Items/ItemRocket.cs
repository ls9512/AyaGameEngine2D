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
    /// 类      名：ItemRocket
    /// 功      能：火箭物件类
    /// 日      期：2015-06-23
    /// 修      改：2015-06-23
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class ItemRocket : BaseItem
    {
        /// <summary>
        /// 重写所在矩形
        /// </summary>
        public override RectangleF ObjectRect
        {
            get
            {
                return new RectangleF(this.X + 22, this.Y + 22, 80, 54);
            }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        public ItemRocket()
        {
            this.Type = ItemType.Rocket;
            this.ItemStatus = ItemStatus.Normal;
        }

        /// <summary>
        /// 重写物件逻辑
        /// </summary>
        public override void ItemLogic()
        {
            // 跟随地图移动
            this.X -= this.MoveX * Time.DeltaTime;
            // 如果与物件发生碰撞
            if (GameSupport.RectHitCheck(this.ObjectRect, GS.GamePlayer.ObjectRect))
            {
                // 路障与玩家碰撞
                if (this.ItemStatus != ItemStatus.Attack)
                {
                    this.ItemStatus = ItemStatus.Attack;
                    // 添加碰撞动画                       
                    TM.AnimationRocketAttack.X = this.X - 80;
                    TM.AnimationRocketAttack.Y = this.Y - 65;
                    TM.AnimationRocketAttack.NowFrame = 0;
                    TM.AnimationRocketAttack.Reset();
                    GS.AnimationList.Add(TM.AnimationRocketAttack);
                    // 玩家处理
                    GS.GamePlayer.Life--;
                    GS.GamePlayer.AcceleratedSpeed = -15f;
                    // 游戏数据处理
                    GS.Score = (int)(0.9f * GS.Score);
                    GS.Combo = 0;
                    // 添加表情
                    if (GS.GamePlayer.Expression == null)
                    {
                        GS.GamePlayer.Expression = new Expression(ExpressionType.Anxiety);
                    }
                    // 播放音效
                    SM.PlayRocketAttack();
                    // 移除
                    GS.ItemList.Remove(this);
                }
            }
        }

        // 警示动画参数
        private float frame_zoom = 0;
        private float frame_speed = 0.5f;
        private float frame_move = 0;

        /// <summary>
        /// 重写物件绘图
        /// </summary>
        public override void ItemGraphic()
        {
            if (!GameSupport.RectHitCheck(this.ObjectRect, General.Draw_Rect))
            {
                // 躲过的不需绘制警示
                if (this.X < 0) return;
                // 不在屏幕中时绘制警示动画
                // 警示线条
                GH.DrawImage(TM.TextureRocketWarningLine.TextureID, frame_move, this.Y + GS.GamePlayer.OffestY + 40, General.Draw_Rect.Width + 1, 20);
                GH.DrawImage(TM.TextureRocketWarningLine.TextureID, General.Draw_Rect.Width + frame_move, this.Y + GS.GamePlayer.OffestY + 40, General.Draw_Rect.Width + 1, 20);
                frame_move -= 40f * Time.DeltaTime;
                if (frame_move < -General.Draw_Rect.Width) frame_move = 0;
                // 警示标志
                GH.DrawImage(TM.TextureRocketWarning.TextureID, General.Draw_Rect.Width - 120 - frame_zoom, this.Y + GS.GamePlayer.OffestY - frame_zoom, 96 + frame_zoom * 2, 96 + frame_zoom * 2);
                frame_zoom += frame_speed * Time.DeltaTime;
                if (frame_zoom < 0 || frame_zoom > 20) frame_speed *= -1;
            }
            else
            {
                // 火箭
                GH.DrawImage(TM.TextureRocket.TextureID, this.X, this.Y + GS.GamePlayer.OffestY, this.Width, this.Height);
            }
        }
    }
}
