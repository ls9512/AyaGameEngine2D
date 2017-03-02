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
    /// 类      名：ItemGold
    /// 功      能：金钱物件类
    /// 日      期：2015-06-23
    /// 修      改：2015-06-26
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class ItemGold : BaseItem
    {
        /// <summary>
        /// 吸收距离
        /// 用于吸收时的缩放动画效果
        /// </summary>
        public float GetLength
        {
            get { return this._getLength; }
            set { this._getLength = value; }
        }
        private float _getLength;

        /// <summary>
        /// 动画帧 物件动画状态
        /// </summary>
        public int Frame
        {
            get
            {
                this._frame += 0.05f * Time.DeltaTime;
                if (this._frame >= 4) this._frame = 0;
                return (int)this._frame;
            }
            set { this._frame = value; }
        }
        private float _frame = 0;

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
                return new RectangleF(this.X, this.Y + this.FlowFrame, this.Width, this.Height);
            }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        public ItemGold()
        {
            this.GetLength = 0;
            this.Type = ItemType.Normal;
            this.ItemStatus = ItemStatus.Normal;
        }

        /// <summary>
        /// 重写物件逻辑
        /// </summary>
        public override void ItemLogic()
        {
            // 跟随地图移动
            this.X -= (GS.MoveSpeed + GS.PlayerFlySpeed) * Time.DeltaTime;

            // 被玩家吸引·飞行状态
            if (this.ItemStatus == ItemStatus.Fly)
            {
                this.X += this.MoveX * Time.DeltaTime;
                this.Y += this.MoveY * Time.DeltaTime;
            }
            // 物件浮动
            this._floaFrame += this._flag * Time.DeltaTime;
            if (this._floaFrame < -10 || this._floaFrame > 0) this._flag *= -1;

            // 物品被吸收距离
            float flyLength = GS.GamePlayer.GetItemLength;
            // 滑翔中吸收速度加成300%
            if (GS.GamePlayer.PlayerStatus == PlayerStatus.Glide)
            {
                flyLength += GS.GamePlayer.GetItemLength * 3;
            }
            // 物品
            float getLnegth = 20f;
            // 向玩家飞行
            float length = (float)GameSupport.LengthFromPointToPoint(GS.GamePlayer.CenterPoint, this.CenterPoint);
            if (this.ItemStatus == ItemStatus.Normal)
            {
                if (length < flyLength && length > getLnegth)
                {
                    this.ItemStatus = ItemStatus.Fly;
                    // 随机初始速度方向
                    //this.MoveX = (float)(Math.Cos((float)30 * GS.RandCreater.Next(0, 100)));
                    //this.MoveY = (float)(Math.Sin((float)30 * GS.RandCreater.Next(0, 100)));
                }
            }
            // 飞行中则实时计算飞行速度
            if (this.ItemStatus == ItemStatus.Fly)
            {
                // 计算实时速度
                this.MoveX = -(this.CenterPoint.X - GS.GamePlayer.CenterPoint.X) / (length / 8f);
                this.MoveY = -(this.CenterPoint.Y - GS.GamePlayer.CenterPoint.Y) / (length / 8f);
                // 加速收回
                if (this.CenterPoint.X < GS.GamePlayer.CenterPoint.X)
                {
                    this.MoveX += GS.MoveSpeed;
                }
                // 计算缩放
                if (length < flyLength)
                {
                    this.GetLength = (flyLength - length + getLnegth) / flyLength * this.Width / 3;
                }
            }
            // 吸收加分物件
            if (length < getLnegth && this.Type == ItemType.Normal)
            {
                // 加分
                GS.Score += this.Value;

                // 播放音效
                SM.PlayAddScore();
                if (this.Type == ItemType.Normal)
                {
                    // 连击统计 
                    GS.Combo++;
                    if (GS.Combo > GS.MaxCombo) GS.MaxCombo = GS.Combo;
                    GS.ItemGet++;
                    // 连击表情
                    if (GS.Combo % 100 == 0)
                    {
                        if (GS.GamePlayer.Expression == null)
                        {
                            GS.GamePlayer.Expression = new Expression(ExpressionType.Love);
                        }
                    }
                }
                // 移除
                GS.ItemList.Remove(this);
            }              
        }

        /// <summary>
        /// 重写物件绘图
        /// </summary>
        public override void ItemGraphic()
        {
            /*
            // 不绘制在屏幕外的
            if (!GameSupport.RectHitCheck(this.ObjectRect, General.Draw_Rect))
            {
                return;
            }
             */
            int itemWidth = 48;
            // 绘制物件
            if (this.ItemStatus == ItemStatus.Fly)
            {
                float length = this.GetLength / 2;
                GH.DrawImage(TM.TextureItem.TextureID[this.Frame, this.Index], this.X + length, this.Y + length + GS.GamePlayer.OffestY, itemWidth - length * 2, itemWidth - length * 2);
            }
            else
            {
                GH.DrawImage(TM.TextureItem.TextureID[this.Frame, this.Index], this.X, this.Y + this.FlowFrame + GS.GamePlayer.OffestY, itemWidth, itemWidth);
            }
        }
    }
}
