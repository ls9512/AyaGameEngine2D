using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using AyaGameEngine2D;
using GS = AcgParkour.Game.GameStatus;

namespace AcgParkour.Models
{
    /// <summary>
    /// 类      名：Block
    /// 功      能：游戏图块类
    /// 日      期：2015-03-21
    /// 修      改：2015-03-21
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class Block : GameObject
    {
        /// <summary>
        /// 块类型
        /// </summary>
        public BlockType Type
        {
            get { return this._type; }
            set { this._type = value; }
        }
        private BlockType _type;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Block(BlockType type)
        {
            this._type = type;
        }

        /// <summary>
        /// ★ 重写 所在矩形 
        /// 玩家的实际体积只是图片中身体和头的部分，头发不参与碰撞检测
        /// </summary>
        public override RectangleF ObjectRect
        {
            get { return new RectangleF(this.X, this.Y + GS.GroundWalkHeightOffest, this.Width, this.Height - GS.GroundWalkHeightOffest); }
        }
    }
}
