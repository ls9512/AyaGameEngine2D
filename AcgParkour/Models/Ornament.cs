using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using AyaGameEngine2D;

namespace AcgParkour.Models
{
    /// <summary>
    /// 类      名：Ornament
    /// 功      能：游戏饰品类，游戏中达到一定条件时出现作为提示或装饰的物件，不影响游戏进程。
    /// 日      期：2015-06-22
    /// 修      改：2015-06-22
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class Ornament : GameObject
    {
        /// <summary>
        /// 纹理
        /// </summary>
        public Texture Texture
        {
            get { return this._texture; }
            set { this._texture = value; }
        }
        private Texture _texture;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="x">坐标X</param>
        /// <param name="y">坐标Y</param>
        /// <param name="texture">纹理</param>
        public Ornament(int x, int y,Texture texture)
        {
            this.X = x;
            this.Y = y;
            this.Width = texture.Width;
            this.Height = texture.Height;
            this._texture = texture;
        }
    }
}
