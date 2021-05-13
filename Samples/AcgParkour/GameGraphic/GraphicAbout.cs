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
    /// 类      名：GraphicAbout
    /// 功      能：关于界面绘图静态类
    /// 日      期：2015-07-26
    /// 修      改：2015-06-26
    /// 作      者：ls9512
    /// </summary>
    public static class GraphicAbout
    {
        /// <summary>
        /// 绘制主菜单
        /// </summary>
        public static void DrawAbout()
        {
            // 绘制背景图
            GH.DrawImage(TM.Texture_UI_Win_About.TextureID, 0, 0, General.Draw_Rect.Width, General.Draw_Rect.Height);

            // 绘制效果动画
            if (GS.EffectItemList != null)
            {
                foreach (EffectItem item in GS.EffectItemList)
                {
                    item.DrawEffectItem();
                }
            }

            // 绘制返回按钮
            UM.Btn_AboutBackTitle.UIGraphic();

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
                if (TM.AnimationTransition.IsNewSence && TM.AnimationTransition.Flag == "BackTitle")
                {
                    // 切换到标题画面
                    GS.GamePhase = GamePhase.MainMenu;
                }
            }
        }
    }
}
