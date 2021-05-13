using System;
using System.Collections.Generic;
using AcgParkour.Models;

using AyaGameEngine2D;

using GS = AcgParkour.Game.GameStatus;
using TM = AcgParkour.GameGraphic.TextureManager;
using UM = AcgParkour.GameGraphic.UIManager;

namespace AcgParkour.GameLogic
{
    /// <summary>
    /// 类      名：LogicTitle
    /// 功      能：标题画面逻辑静态类
    /// 日      期：2015-03-26
    /// 修      改：2015-06-26
    /// 作      者：ls9512
    /// </summary>
    public static class LogicTitle
    {
        /// <summary>
        /// 上次创建时间
        /// </summary>
        private static int lastEffectCreateTime = 0;

        /// <summary>
        /// 标题画面效果逻辑
        /// </summary>
        public static void TitleEffect()
        {
            // 不存在则新建
            if (GS.EffectItemList == null)
            {
                GS.EffectItemList = new List<EffectItem>();
            }
            // 判断是否生成新的效果物件
            int time = Environment.TickCount;
            if (GS.EffectItemList.Count < 150 && time - lastEffectCreateTime > 15)
            {
                lastEffectCreateTime = time;
                EffectItem item = new EffectItem(TM.TextureStarEffect.TextureID, TM.TextureStarEffect.Width, TM.TextureStarEffect.Height);
                item.X = RandomHelper.RandFloat(0, General.Draw_Rect.Width);
                item.Y = -50; ;
                item.MoveX = RandomHelper.RandFloat(-20, 20) / 20;
                item.MoveY = RandomHelper.RandFloat(10, 30) / 10;
                item.AngleSpeed = RandomHelper.RandFloat(-20, 20) / 10;
                GS.EffectItemList.Add(item);
            }
            // 删除旧的效果物件
            for (int i = GS.EffectItemList.Count - 1; i >= 0; i--)
            {
                // 不在屏幕内或者透明的
                if ((GS.EffectItemList[i].X < -50 || GS.EffectItemList[i].Y > General.Draw_Rect.Height) || GS.EffectItemList[i].Pellucidity < 1)
                {
                    GS.EffectItemList.Remove(GS.EffectItemList[i]);
                }
            }
        }

        /// <summary>
        /// UI逻辑
        /// </summary>
        public static void UILogic()
        {
            UM.Btn_Start.UILogic();
            UM.Btn_Load.UILogic();
            UM.Btn_Rank.UILogic();
            UM.Btn_Help.UILogic();
            UM.Btn_About.UILogic();
            UM.Btn_Exit.UILogic();
        }
    }
}
