using System;
using GS = JewelHunter.Game.GameStatus;
using AyaGameEngine2D;
using GH = AyaGameEngine2D.GraphicHelper;

namespace JewelHunter.Models
{
    /// <summary>
    /// 类      名：EffectItem
    /// 功      能：效果物件
    /// 日      期：2015-03-26
    /// 修      改：2015-03-26
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public sealed class EffectItem : GameObject
    {
        /// <summary>
        /// 标志
        /// </summary>
        public bool Flag
        {
            get { return _flag; }
            set
            {
                _flag = value;
            }
        }
        private bool _flag;

        /// <summary>
        /// 旋转角度
        /// </summary>
        public float Angle
        {
            get { return _angle; }
            set 
            { 
                _angle = value;
            }
        }
        private float _angle;

        /// <summary>
        /// 旋转角速度
        /// </summary>
        public float AngleSpeed
        { 
             get { return _angleSpeed; }
            set { _angleSpeed = value; }
        }
        private float _angleSpeed;

        /// <summary>
        /// 效果纹理ID
        /// </summary>
        public uint[] TextureId
        {
            get { return _textureId; }
            set { _textureId = value; }
        }
        private uint[] _textureId;

        /// <summary>
        /// 透明度
        /// </summary>
        public float Pellucidity
        {
            get { return _pellucidity; }
            set { _pellucidity = value; }
        }
        private float _pellucidity;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EffectItem(uint[] textureId,int widht,int height)
        {
            _textureId = textureId;
            Angle = 0;
            AngleSpeed = RandomHelper.RandFloat(-5, 5);
            MoveX = RandomHelper.RandFloat(-5, 5);
            MoveY = RandomHelper.RandFloat(-5, 5);
            Width = widht;
            Height = height;
            _pellucidity = 250;
        }

        /// <summary>
        /// 绘制效果物件
        /// </summary>
        public void DrawEffectItem()
        {
            GH.DrawImage(_textureId, ObjectRect, Angle, (int)_pellucidity);

            X += MoveX * Time.DeltaTime;
            Y += MoveY * Time.DeltaTime;
            Angle += AngleSpeed;
            if (_pellucidity > 0)
            {
                // 在屏幕内才变淡
                if (GameSupport.RectHitCheck(ObjectRect, General.DrawRect))
                {
                    if (GS.GamePhase == GamePhase.Gaming)
                    {
                        _pellucidity -= 0.8f * Time.DeltaTime;
                    }
                    else
                    {
                        _pellucidity -= 1.2f * Time.DeltaTime;
                    }
                }
            }
        }
    }
}
