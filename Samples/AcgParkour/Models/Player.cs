using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using AyaGameEngine2D;

namespace AcgParkour.Models
{
    /// <summary>
    /// 类      名：Player
    /// 功      能：玩家类
    /// 日      期：2015-03-22
    /// 修      改：2015-03-22
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class Player : GameObject
    {
        /// <summary>
        /// 已飞行距离
        /// </summary>
        public float FlyFrame
        {
            get { return this._flyFrame; }
            set { this._flyFrame = value; }
        }
        private float _flyFrame;

        /// <summary>
        /// 最大飞行距离
        /// </summary>
        public float MaxFlyFrame
        {
            get { return this._maxFlyFrame; }
            set { this._maxFlyFrame = value; }
        }
        private float _maxFlyFrame;

        /// <summary>
        /// 飞行速度
        /// </summary>
        public float FlySpeed
        {
            get { return this._flySpeed; }
            set { this._flySpeed = value; }
        }
        private float _flySpeed;

        /// <summary>
        /// 玩家状态
        /// </summary>
        public PlayerStatus PlayerStatus
        {
            get { return this._playerStatus; }
            set { this._playerStatus = value; }
        }
        private PlayerStatus _playerStatus;

        /// <summary>
        /// 吸收物品距离
        /// </summary>
        public int GetItemLength
        {
            get { return this._getItemLength; }
            set { this._getItemLength = value; }
        }
        private int _getItemLength;

        /// <summary>
        /// 玩家纹理ID
        /// </summary>
        public uint[][] PlayerTexture
        {
            get { return this._playerTexture; }
            set { this._playerTexture = value; }
        }
        private uint[][] _playerTexture;

        /// <summary>
        /// 中心点
        /// </summary>
        public override PointF CenterPoint
        {
            get
            {
                float x = this.ObjectRect.X + this.ObjectRect.Width / 2;
                float y = this.ObjectRect.Y + this.ObjectRect.Height / 2;
                return new PointF(x, y);
            }
        }

        /// <summary>
        /// ★ 重写 所在矩形 
        /// 玩家的实际体积只是图片中身体和头的部分，头发不参与碰撞检测
        /// </summary>
        public override RectangleF ObjectRect
        {
            get
            {
                //if (this._playerStatus == PlayerStatus.Jump || this._playerStatus == PlayerStatus.Glide)
                {
                    //return new Rectangle((int)this.X + 70, (int)this.Y + 15, this.Width - 110, this.Height - 35);
                }
                //else
                {
                    return new RectangleF(this.X + 175, this.Y + 32, this._realWidth, this.Height - 15);
                }
            }
        }

        /// <summary>
        /// 吸收物品距离
        /// </summary>
        public int RealWidth
        {
            get { return this._realWidth; }
            set { this._realWidth = value; }
        }
        private int _realWidth;

        /// <summary>
        /// 多段跳计数
        /// </summary>
        public int Jump
        {
            get { return this._jump; }
            set { this._jump = value; }
        }
        private int _jump;

        /// <summary>
        /// 最大跳段数
        /// </summary>
        public int MaxJump
        {
            get { return this._maxJump; }
            set { this._maxJump = value; }
        }
        private int _maxJump;

        /// <summary>
        /// 下落加速度
        /// </summary>
        public float AcceleratedSpeed
        {
            get { return this._acceleratedSpeed; }
            set { this._acceleratedSpeed = value; }
        }
        private float _acceleratedSpeed;

        /// <summary>
        /// 生命值
        /// </summary>
        public int Life
        {
            get { return this._life; }
            set { this._life = value; }
        }
        private int _life;

        /// <summary>
        /// 生命值上限
        /// </summary>
        public int MaxLife
        {
            get { return this._maxLife; }
            set { this._maxLife = value; }
        }
        private int _maxLife;

        /// <summary>
        /// 表情
        /// </summary>
        public Expression Expression
        {
            get { return this._expression; }
            set { this._expression = value; }
        }
        private Expression _expression;

        /// <summary>
        /// 玩家绘制纵向偏移
        /// ★保持人物在画面中心，地图随玩家位置上下移动
        /// </summary>
        public float OffestY
        {
            get 
            {
                switch (General.Game_ViewMode)
                {
                    case ViewMode.Normal:
                        return 0;
                    case ViewMode.Center:
                        return this._offestY_Temp;
                    default:
                        return 0;
                }
            }
        }
        private float _offestY_Temp = 0;

        /// <summary>
        /// 帧索引
        /// </summary>
        public int FrameIndex
        {
            get { return (int)this._frameIndex; }
        }
        private float _frameIndex;

        /// <summary>
        /// 帧速度
        /// </summary>
        private float _frameBuffer = 0.35f;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Player()
        {
            this._playerStatus = PlayerStatus.Jump;
            this._frameIndex = 0;
            this._acceleratedSpeed = 0;
            this._life = 5;
            this._maxLife = 5;
            this._jump = 2;
            this._maxJump = 2;
            this._flyFrame = 0;
            this._maxFlyFrame = 100;
            this._flySpeed = 4f;
            this._getItemLength = 100;
        }

        /// <summary>
        /// 重设Y轴浮动
        /// </summary>
        public void ResetOffestY()
        {
            this._offestY_Temp = 0f;
        }

        /// <summary>
        /// 玩家逻辑
        /// </summary>
        /// <returns>纹理ID</returns>
        public void PlayerLogic()
        {
            if (General.Game_ViewMode == ViewMode.Center)
            {
                // 玩家位置保持中心所需要的偏移量
                float centerY_Min = General.Draw_Rect.Height / 2 - this.Y - 320;
                float centerY_Max = General.Draw_Rect.Height / 2 - this.Y - 80;
                float speed = 3f;
                if (this._offestY_Temp > centerY_Max + speed)
                {
                    this._offestY_Temp -= speed * Time.DeltaTime;
                }
                if (this._offestY_Temp < centerY_Min - speed)
                {
                    this._offestY_Temp += speed * Time.DeltaTime;
                }
            }

            // 下一帧
            int length = this._playerTexture.Length;
            switch (this._playerStatus)
            {
                case PlayerStatus.Stand:
                    this._frameIndex = 0;
                    break;
                case PlayerStatus.Run:
                    this._frameIndex += this._frameBuffer * Time.DeltaTime;
                    if (this._frameIndex >= length - 1)
                    {
                        this._frameIndex = 1;
                    }
                    break;
                case PlayerStatus.Jump:
                    this._frameIndex = length - 1;
                    break;
            }
        }
    }
}
