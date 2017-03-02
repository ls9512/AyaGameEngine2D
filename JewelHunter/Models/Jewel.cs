using System;
using System.Drawing;
using AyaGameEngine2D;
using TM = JewelHunter.GameGraphic.TextureManager;
using GS = JewelHunter.Game.GameStatus;
using GH = AyaGameEngine2D.GraphicHelper;

namespace JewelHunter
{
    /// <summary>
    /// 宝石状态枚举
    /// </summary>
    public enum JewelStatus
    { 
        Noraml,     // 正常
        Drop,       // 下落
        Move,       // 移动
        Boom,       // 消除
    }
}

namespace JewelHunter.Models
{
    /// <summary>
    /// 类      名：Jewel
    /// 功      能：宝石类
    /// 日      期：2015-11-23
    /// 修      改：2016-04-14
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public sealed class Jewel : GameObject
    {
        /// <summary>
        /// 旋转角度
        /// </summary>
        private float _angle;
        /// <summary>
        /// 动画帧
        /// </summary>
        private float _animationFrame;
        /// <summary>
        /// 动画帧速度
        /// </summary>
        private float _animationSpeed = 0.3f;

        /// <summary>
        /// 宝石索引
        /// </summary>
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        private int _index;

        /// <summary>
        /// 当前宝石坐标
        /// </summary>
        public Point JewelPoint => _jewelPoint;

        private Point _jewelPoint;

        /// <summary>
        /// 目标宝石坐标
        /// </summary>
        public Point AimPoint
        {
            get { return _aimPoint; }
            set { _aimPoint = value; }
        }
        private Point _aimPoint;

        /// <summary>
        /// 目标屏幕坐标
        /// </summary>
        public Point AimLocation
        {
            get { return _aimLocation; }
            set { _aimLocation = value; }
        }
        private Point _aimLocation;

        /// <summary>
        /// 宝石状态
        /// </summary>
        public JewelStatus JewelStatus
        {
            get { return _jewelStatus; }
            set { _jewelStatus = value; }
        }
        private JewelStatus _jewelStatus;

        /// <summary>
        /// 构造方法
        /// </summary>
        public Jewel(int x, int y, int index,bool randCreateLoc)
        {
            _index = index;
            _jewelPoint = new Point(x, y);
            _aimPoint = new Point(x, y);
            _aimLocation = GameLogic.LogicMain.GetScreenPoint(_aimPoint);
            if (randCreateLoc)
            {
                if (!GS.IsJewelInit)
                {
                    Y = -(7 - y) * 100 - 80;
                }
                else
                {
                    Y = -(7 - y) * 10 - 70;
                }
                X = _aimLocation.X;
                _jewelStatus = JewelStatus.Drop;
                MoveX = 0;
                MoveY = RandomHelper.RandFloat(5, 10) * (y + 1) * 1f / 5;
            }
            else
            {
                X = _jewelPoint.X * 80 + GS.JewelStartPoint.X;
                Y = _jewelPoint.Y * 80 + GS.JewelStartPoint.Y;
                _jewelStatus = JewelStatus.Noraml;
                MoveX = 0;
                MoveY = 0;
            }       
            _angle = RandomHelper.RandFloat(0, 360);
        }

        /// <summary>
        /// 设置目标下落坐标
        /// </summary>
        /// <param name="aimPoint"></param>
        public void SetDropAimPoint(Point aimPoint)
        {
            _aimPoint = aimPoint;
            _aimLocation = GameLogic.LogicMain.GetScreenPoint(aimPoint);
            // 30帧内移动到目标位置
            MoveX = (_aimLocation.X - X) * 1f / 30f;
            MoveY = (_aimLocation.Y - Y) * 1f / 30f;
            _jewelStatus = JewelStatus.Drop;
        }

        /// <summary>
        /// 设置目标移动位置
        /// </summary>
        /// <param name="location">目标位置</param>
        public void SetMoveAimLoaction(Point location)
        {
            _aimLocation = location;
            // 30帧内移动到目标位置
            MoveX = (_aimLocation.X - X) * 1f / 30f;
            MoveY = (_aimLocation.Y - Y) * 1f / 30f;
            _jewelStatus = JewelStatus.Move;
        }

        /// <summary>
        /// 绘图方法 - 每一帧调用
        /// </summary>
        public void Graphic()
        {
            // 绘制宝石
            GH.DrawImage(TM.TextureJewel[_index].TextureID, X, Y, 80, 80, _angle);

            // 消除
            if (_jewelStatus == JewelStatus.Boom)
            {
                int width = 20;
                GH.DrawImage(TM.TexutreBoom[(int)_animationFrame].TextureID, X - width, Y - width, 80 + width * 2, 80 + width * 2);
            }
        }

        /// <summary>
        /// 逻辑方法 - 每一帧调用
        /// </summary>
        public void Logic()
        {
            // 旋转
            _angle += 0.5f * Time.DeltaTime;

            // 下落状态
            if (_jewelStatus == JewelStatus.Drop)
            {
                X += MoveX * Time.DeltaTime;
                Y += MoveY * Time.DeltaTime;
                MoveY += 0.1f * Time.DeltaTime;

                // 距目标位置小于一定距离后
                if (GameSupport.LengthFromPointToPoint(Location, _aimLocation) < 5 || Y > _aimLocation.Y)
                {
                    X = _aimLocation.X;
                    Y = _aimLocation.Y;
                    _jewelStatus = JewelStatus.Noraml;
                    // 设置目标位置状态
                    _jewelPoint = _aimPoint;
                    GS.JewelArray[_jewelPoint.X, _jewelPoint.Y] = _index;
                }
            }

            // 移动状态
            if (_jewelStatus == JewelStatus.Move)
            {
                X += MoveX * Time.DeltaTime;
                Y += MoveY * Time.DeltaTime;
                PointF p = GameGraphic.GraphicMain.JewelNextOffsetPoint;
                GameGraphic.GraphicMain.JewelNextOffsetPoint = new PointF(p.X -= MoveX * Time.DeltaTime, p.Y -= MoveY * Time.DeltaTime);
                _angle += 5f * Time.DeltaTime;

                // 距目标位置小于一定距离后
                if (GameSupport.LengthFromPointToPoint(Location, _aimLocation) < 10 || X > _aimLocation.X)
                {
                    X = _aimLocation.X;
                    Y = _aimLocation.Y;
                    _jewelStatus = JewelStatus.Noraml;

                    // 到达目的地,进行交换
                    Jewel jewel = new Jewel(_jewelPoint.X, _jewelPoint.Y, GS.JewelNext, false);
                    GS.JewelList.Add(jewel);
                    GS.JewelNext = _index;
                    GS.JewelList.Remove(this);
                    GameGraphic.GraphicMain.JewelNextOffsetPoint = new PointF(0, 0);

                    // 进行消除
                    GameLogic.LogicMain.DoClear();
                }
            }

            // 消除
            if (_jewelStatus == JewelStatus.Boom)
            {
                _animationFrame += _animationSpeed * Time.DeltaTime;

                // 动画结束，消除完成
                if (_animationFrame >= 7)
                {
                    // 消除并加分
                    GS.JewelArray[_jewelPoint.X, _jewelPoint.Y] = -1;
                    GS.JewelList.Remove(this);
                }
            }
        }
    }
}
