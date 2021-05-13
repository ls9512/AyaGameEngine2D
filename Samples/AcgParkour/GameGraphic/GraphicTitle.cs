using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using AcgParkour.Models;

using AyaGameEngine2D;

using GS = AcgParkour.Game.GameStatus;
using TM = AcgParkour.GameGraphic.TextureManager;
using SM = AcgParkour.GameIO.SoundManager;
using UM = AcgParkour.GameGraphic.UIManager;
using GH = AyaGameEngine2D.GraphicHelper;


namespace AcgParkour.GameGraphic
{
    /// <summary>
    /// 类      名：GraphicTitle
    /// 功      能：标题绘图静态类
    /// 日      期：2015-03-21
    /// 修      改：2015-06-26
    /// 作      者：ls9512
    /// </summary>
    public static class GraphicTitle
    {
        private static float zoomSize = 0;
        private static float zoomFlag = 0.1f;

        /// <summary>
        /// 绘制标题
        /// </summary>
        public static void DrawTitle()
        {
            // 绘制背景图
            GH.DrawImage(TM.Texture_UI_Title_01.TextureID, 0, 0, General.Draw_Rect.Width, General.Draw_Rect.Height);

            // 绘制效果动画
            if (GS.EffectItemList != null)
            {
                foreach (EffectItem item in GS.EffectItemList)
                {
                    item.DrawEffectItem();
                }
            }

            // 计算按键提示位置
            int x = General.Draw_Rect.Width / 2 - TM.Texture_UI_Btn_PressSpace.Width / 2;
            int y = 600;
            GH.DrawImage(TM.Texture_UI_Btn_PressSpace.TextureID, x - zoomSize, y - zoomSize / 2, TM.Texture_UI_Btn_PressSpace.Width + zoomSize * 2, TM.Texture_UI_Btn_PressSpace.Height + zoomSize,225 - (int)((6f - zoomSize) * 30));
            zoomSize += zoomFlag * Time.DeltaTime;
            if (zoomSize > 5 || zoomSize < 0) zoomFlag *= -1;

            // 绘制渐变
            if (TM.AnimationTransition != null)
            {
                TM.AnimationTransition.DrawTransAnimation();
                if (TM.AnimationTransition.IsNewSence)
                {
                    GS.GamePhase = GamePhase.MainMenu;
                }
                if (TM.AnimationTransition.IsEnd)
                {
                    TM.AnimationTransition.Dispose();
                    TM.AnimationTransition = null;
                }
            }
        }


        /// <summary>
        /// 绘制主菜单
        /// </summary>
        public static void DrawMainMenu()
        {

            // 绘制背景图
            GH.DrawImage(TM.Texture_UI_Title_02.TextureID, 0, 0, General.Draw_Rect.Width, General.Draw_Rect.Height);

            // 绘制按钮
            UM.Btn_Start.UIGraphic();
            UM.Btn_Load.UIGraphic();
            UM.Btn_Rank.UIGraphic();
            UM.Btn_Help.UIGraphic();
            UM.Btn_About.UIGraphic();
            UM.Btn_Exit.UIGraphic();

            // 绘制效果动画
            if (GS.EffectItemList != null)
            {
                foreach (EffectItem item in GS.EffectItemList)
                {
                    item.DrawEffectItem();
                }
            }

            // 绘制渐变
            if (TM.AnimationTransition != null)
            {
                TM.AnimationTransition.DrawTransAnimation();
                if (TM.AnimationTransition.IsEnd)
                {
                    if (TM.AnimationTransition.IsEnd)
                    {
                        TM.AnimationTransition.Dispose();
                        TM.AnimationTransition = null;
                        return;
                    }
                }
                if (TM.AnimationTransition.IsNewSence && TM.AnimationTransition.Flag == "Gaming")
                {
                    // 创建新游戏
                    GS._gameLogic.NewGame();
                    // 播放BGM
                    SM.PlayGameBGM();
                    SM.StopTitleBGM();
                }
                if (TM.AnimationTransition.IsNewSence && TM.AnimationTransition.Flag == "Help")
                {
                    GS.GamePhase = GamePhase.Help;
                    GraphicHelp.HelpIndex = 0;
                }
                if (TM.AnimationTransition.IsNewSence && TM.AnimationTransition.Flag == "About")
                {
                    GS.GamePhase = GamePhase.About;
                }
            }
        }
    }
}
