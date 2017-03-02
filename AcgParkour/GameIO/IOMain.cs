using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using AcgParkour.Models;

using AyaGameEngine2D;

using GS = AcgParkour.Game.GameStatus;
using TM = AcgParkour.GameGraphic.TextureManager;
using SM = AcgParkour.GameIO.SoundManager;
using KM = AyaGameEngine2D.KeyManager;
using MM = AyaGameEngine2D.MouseManager;

namespace AcgParkour.GameIO
{
    /// <summary>
    /// 类      名：GameIO
    /// 功      能：游戏输入输出控制类,对游戏的输入输出进行统一管理
    /// 日      期：2015-03-21
    /// 修      改：2015-03-21
    /// 作      者：ls9512
    /// </summary>
    public class IOMain
    {
        public bool isTitleBGM = false;

        /// <summary>
        /// 游戏IO处理
        /// </summary>
        public void IO()
        {
            // 调试 - 显示碰撞监测区域
            if (Input.IsKeyPressed(Keys.F6))
            {
                General.Debug_ShowHitCheckRect = !General.Debug_ShowHitCheckRect;
            }
            // 调试 - 显示元素区域
            if (Input.IsKeyPressed(Keys.F7))
            {
                General.Debug_ShowRect = !General.Debug_ShowRect;
                General.Debug_ShowHitCheckRect = General.Debug_ShowRect;
            }

            switch (GS.GamePhase)
            {
                case GamePhase.Title:
                    if (!isTitleBGM)
                    {
                        SM.PlayTitleBGM();
                        isTitleBGM = true;
                    }
                    if ((Input.IsAnyKey || Input.IsMouseLeft) && TM.AnimationTransition == null)
                    {
                        // 开启渐变
                        TM.AnimationTransition = new AnimationTrans(new Texture(General.Data_Path + @"\Graphic\Transitions\Transitions_Black.png"), "Title");
                    }
                    break;
                case GamePhase.MainMenu:
                    if (Input.IsKeyPressed(Keys.Space))
                    {
                        if (TM.AnimationTransition == null)
                        {
                            // 开启渐变
                            TM.AnimationTransition = new AnimationTrans(new Texture(General.Data_Path + @"\Graphic\Transitions\Transitions_" + RandomHelper.RandInt(1, 14) + ".png"), "Gaming");
                        }
                    }
                    break;
                case GamePhase.Gaming:
                    // 按下ESC或者P键暂停
                    if (Input.IsKeyPressed(Keys.Escape) || Input.IsKeyPressed(Keys.P))
                    {
                        if (GS.IsPause)
                        {
                            GS.IsPauseCountDown = true;
                        }
                        else
                        {
                            GS.IsPause = true;
                            GS.IsPauseCountDown = false;
                        }
                    }
                    if (!GS.IsPause)
                    {
                        // 按下空格键
                        if (Input.IsKeyPressed(Keys.Space))
                        {
                            // 跳跃
                            if (GS.GamePlayer.Jump > 0 && GS.GamePlayer.Jump <= GS.GamePlayer.MaxJump)
                            {
                                // 跳的加速度，随段数提高而减弱
                                GS.GamePlayer.AcceleratedSpeed = -(17f - (GS.GamePlayer.MaxJump - GS.GamePlayer.Jump) * 1.5f);
                                GS.GamePlayer.Jump--;
                                GS.GamePlayer.PlayerStatus = PlayerStatus.Jump;
                                // 稍微提高高度防止落地碰撞误判
                                GS.GamePlayer.Y--;
                                // 释放按键
                                Input.ReleaseKey(Keys.Space);
                                // 播放跳跃音效
                                SM.PlayJump();
                            }
                        }
                        else if (Input.IsKeyHeld(Keys.Space))
                        {
                            if (GS.GamePlayer.Jump == 0)
                            {
                                // 滑翔开始
                                if (GS.GamePlayer.PlayerStatus != PlayerStatus.Glide && GS.GamePlayer.FlyFrame < GS.GamePlayer.MaxFlyFrame)
                                {
                                    GS.GamePlayer.PlayerStatus = PlayerStatus.Glide;
                                    GS.GamePlayer.AcceleratedSpeed = 0f;
                                    // 飞行提速
                                    GS.PlayerFlySpeed = GS.GamePlayer.FlySpeed;
                                    // 稍微提高高度防止落地碰撞误判
                                    GS.GamePlayer.Y--;
                                }
                            }
                        }
                        else if(Input.IsKeyUp(Keys.Space))
                        {
                            // 空格键释放，滑翔结束
                            if (GS.GamePlayer.Jump == 0 && GS.GamePlayer.PlayerStatus == PlayerStatus.Glide)
                            {
                                GS.GamePlayer.PlayerStatus = PlayerStatus.Jump;
                            }
                        }
                        // ★ 调试功能：重置玩家
                        if (Input.IsKeyPressed(Keys.R) && AyaGameEngine2D.General.Engine_Debug)
                        {
                            // 释放按键
                            AcgParkour.GameLogic.LogicPlayer.CreatePlayer();
                        }
                    }
                    break;
                case GamePhase.GameOver:
                    if ((Input.IsKeyPressed(Keys.Space) || Input.IsMouseLeft) && TM.AnimationTransition == null)
                    {
                        if (TM.AnimationTransition == null)
                        {
                            // 开启渐变
                            TM.AnimationTransition = new AnimationTrans(new Texture(General.Data_Path + @"\Graphic\Transitions\Transitions_" + RandomHelper.RandInt(1, 14) + ".png"), "BackTitle");
                        }
                    }
                    break;
            }
        }
    }
}
