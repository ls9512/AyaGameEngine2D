using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using AcgParkour.Models;

using AyaGameEngine2D;

using GS = AcgParkour.Game.GameStatus;
using TM = AcgParkour.GameGraphic.TextureManager;
using SM = AcgParkour.GameIO.SoundManager;
using GH = AyaGameEngine2D.GraphicHelper;

namespace AcgParkour.GameGraphic
{
    /// <summary>
    /// 类      名：GraphicGameOver
    /// 功      能：游戏结束绘图静态类
    /// 日      期：2015-03-30
    /// 修      改：2015-03-30
    /// 作      者：ls9512
    /// </summary>
    public static class GraphicGameOver
    {
        /// <summary>
        /// 游戏结束绘图
        /// </summary>
        public static void DrawGameOver()
        {
            DrawResult();
            DrawTrans();
        }

        /// <summary>
        /// 绘制结果
        /// </summary>
        private static void DrawResult()
        {
            GH.DrawImage(TM.TextureGameOver.TextureID, 0, 0, General.Draw_Rect.Width, General.Draw_Rect.Height);
        }

        /// <summary>
        /// 绘制渐变
        /// </summary>
        private static void DrawTrans()
        {
            // 绘制渐变
            if (TM.AnimationTransition != null)
            {
                TM.AnimationTransition.DrawTransAnimation();
                if (TM.AnimationTransition.IsNewSence && TM.AnimationTransition.Flag == "BackTitle")
                {
                    // 切换到标题画面
                    GS.GamePhase = GamePhase.MainMenu;
                    // 切换BGM
                    SM.StopGameBGM();
                    SM.PlayTitleBGM();
                    // 清空效果物件
                    GS.EffectItemList.Clear();
                    // 重设Y轴浮动
                    GS.GamePlayer.ResetOffestY();
                }
                if (TM.AnimationTransition.IsEnd && TM.AnimationTransition.Flag == "GameOver")
                {
                    TM.AnimationTransition.Dispose();
                    TM.AnimationTransition = null;
                    SM.PlayTitleBGM();
                }
            }
        }
    }
}
