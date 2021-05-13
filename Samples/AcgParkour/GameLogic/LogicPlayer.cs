using System;
using System.Collections.Generic;
using System.Text;

using AyaGameEngine2D;

using AcgParkour.Models;
using GS = AcgParkour.Game.GameStatus;

namespace AcgParkour.GameLogic
{
    /// <summary>
    /// 类      名：LogicPlayer
    /// 功      能：玩家逻辑静态类
    /// 日      期：2015-03-23
    /// 修      改：2015-03-23
    /// 作      者：ls9512
    /// </summary>
    public static class LogicPlayer
    {
        /// <summary>
        /// 玩家起始坐标
        /// </summary>
        private static int playerStartX = 200; 

        /// <summary>
        /// 创建玩家
        /// </summary>
        public static void CreatePlayer()
        {
            // 创建玩家
            GS.GamePlayer = new Player();
            GS.GamePlayer.Jump = 2;
            GS.GamePlayer.MaxJump = 2;
            GS.GamePlayer.RealWidth = 60;
            GS.GamePlayer.Width = 261;
            GS.GamePlayer.Height = 200;
            GS.GamePlayer.X = playerStartX;
            GS.GamePlayer.Y = -GS.GamePlayer.Height;
            GS.GamePlayer.AcceleratedSpeed = -10f;
            GS._gameGraphic.CreatePlayerTexture();
        }

        /// <summary>
        /// 玩家碰撞检测
        /// </summary>
        public static void MainPlayerLogic()
        {
            // 玩家逻辑
            GS.GamePlayer.PlayerLogic();

            bool isUpHitBlock = false;
            int hitY = 0;
            bool isLeftHitBlock = false;
            bool isDownHitBlock = false;
            int hitX = 0;
            int hitHeight = 0;
            // 判断是否上方碰撞
            for (int i = 0; i < GS.BlockList.Count; i++)
            {
                if (GameSupport.RectHitCheckByDirection(GS.GamePlayer.ObjectRect, GS.BlockList[i].ObjectRect, AyaGameEngine2D.Direction.Up))
                {
                    isUpHitBlock = true;
                    hitY = (int)GS.BlockList[i].Y + GS.GroundWalkHeightOffest;
                    break;
                }
            }
            // 判断是否左方碰撞
            for (int i = 0; i < GS.BlockList.Count; i++)
            {
                if (GameSupport.RectHitCheckByDirection(GS.GamePlayer.ObjectRect, GS.BlockList[i].ObjectRect, AyaGameEngine2D.Direction.Left))
                {
                    //isLeftHitBlock = true;
                    hitX = (int)GS.BlockList[i].X;
                    break;
                }
            }
            // 判断是否下方碰撞
            for (int i = 0; i < GS.BlockList.Count; i++)
            {
                if (GameSupport.RectHitCheckByDirection(GS.GamePlayer.ObjectRect, GS.BlockList[i].ObjectRect, AyaGameEngine2D.Direction.Down))
                {
                    isDownHitBlock = true;
                    hitHeight = (int)GS.BlockList[i].ObjectRect.Bottom;
                    break;
                }
            }
            // 左侧碰撞
            if (isLeftHitBlock)
            {
                // 人物被推向地图左侧
                GS.GamePlayer.X = hitX - GS.GamePlayer.ObjectRect.Width;
                Debug.Log("Hitx " + hitX);
                // 添加表情
                if (GS.GamePlayer.Expression == null)
                {
                    GS.GamePlayer.Expression = new Expression(ExpressionType.Surprised);
                }
            }
            // 上方碰撞
            if (isUpHitBlock)
            {
                // 落地，设置玩家相关参数
                if ((GS.GamePlayer.PlayerStatus == PlayerStatus.Jump || GS.GamePlayer.PlayerStatus == PlayerStatus.Glide) && GS.GamePlayer.AcceleratedSpeed >= 0)
                {
                    GS.GamePlayer.PlayerStatus = PlayerStatus.Run;
                    GS.GamePlayer.AcceleratedSpeed = 0;
                    GS.GamePlayer.Jump = 2;
                    GS.GamePlayer.Y = hitY - GS.GamePlayer.Height;
                    // 恢复飞行速度
                    GS.PlayerFlySpeed = 0;
                }
            }
            else
            {
                // 如果浮空且没有跳跃滑翔，则变为跳跃状态
                if (GS.GamePlayer.PlayerStatus != PlayerStatus.Glide && GS.GamePlayer.PlayerStatus != PlayerStatus.Jump)
                {
                    GS.GamePlayer.PlayerStatus = PlayerStatus.Jump;
                    // 跌落则只可以一段跳
                    GS.GamePlayer.Jump = 1;
                }

                // 浮空
                if (GS.GamePlayer.PlayerStatus == PlayerStatus.Jump)
                {
                    GS.GamePlayer.AcceleratedSpeed += GS.GravitySpeed * Time.DeltaTime;
                }
                // 滑翔
                if (GS.GamePlayer.PlayerStatus == PlayerStatus.Glide)
                {
                    // 加速度
                    GS.GamePlayer.AcceleratedSpeed = GS.GravitySpeed * 2f;
                    // 飞行槽
                    GS.GamePlayer.FlyFrame += 1 * Time.DeltaTime;
                    // 滑翔结束
                    if (GS.GamePlayer.FlyFrame > GS.GamePlayer.MaxFlyFrame)
                    {
                        GS.GamePlayer.PlayerStatus = PlayerStatus.Jump;
                    }
                }
            }
            // 下方碰撞
            if (isDownHitBlock)
            {
                GS.GamePlayer.Y = hitHeight + 1;
                GS.GamePlayer.AcceleratedSpeed = 0;
            }

            // 加速度累加
            GS.GamePlayer.Y += GS.GamePlayer.AcceleratedSpeed * Time.DeltaTime;

            // 跑步时飞行槽恢复
            if (GS.GamePlayer.FlyFrame > 0 && GS.GamePlayer.PlayerStatus == PlayerStatus.Run)
            {
                GS.GamePlayer.FlyFrame -= 0.5f * Time.DeltaTime;
                if (GS.GamePlayer.FlyFrame < 0) GS.GamePlayer.FlyFrame = 0;
            }

            // 防止过度下落导致的落碰撞地误判
            if (GS.GamePlayer.AcceleratedSpeed != 0)
            {
                for (int i = 0; i < GS.BlockList.Count; i++)
                {
                    // 存在上方碰撞，则浮空
                    if (GameSupport.RectHitCheckByDirection(GS.GamePlayer.ObjectRect, GS.BlockList[i].ObjectRect, AyaGameEngine2D.Direction.Up))
                    {
                        isUpHitBlock = true;
                        GS.GamePlayer.Y = GS.BlockList[i].Y - GS.GamePlayer.Height + GS.GroundWalkHeightOffest;
                        break;
                    }
                }
            }

            // 如果玩家被推离起始X点则加速复位
            if (GS.GamePlayer.X < playerStartX)
            {
                GS.GamePlayer.X += 2 * Time.DeltaTime;
            }
        }

        /// <summary>
        /// 随即表情
        /// </summary>
        public static void RandExpression()
        {
            int value = RandomHelper.RandInt(0, 100000);
            int value2 = ((int)GS.ScoreDistance / 100);
            if (value2 % 1000 == 0  && value2 > 100)
            {
                // 困倦
                if (GS.GamePlayer.Expression == null)
                {
                    GS.GamePlayer.Expression = new Expression(ExpressionType.Sleepy);
                }
            }
            if (value > 99080 & value2 % 100 == 0)
            {
                // 无聊
                if (GS.GamePlayer.Expression == null)
                {
                    GS.GamePlayer.Expression = new Expression(ExpressionType.Speechless);
                }
            } 
        }
    }
}
