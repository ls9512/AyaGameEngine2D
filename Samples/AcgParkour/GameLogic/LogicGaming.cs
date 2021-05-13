using System;
using System.Collections.Generic;
using System.Text;
using AcgParkour.Models;

using AyaGameEngine2D;

using GS = AcgParkour.Game.GameStatus;
using TM = AcgParkour.GameGraphic.TextureManager;
using UM = AcgParkour.GameGraphic.UIManager;

namespace AcgParkour.GameLogic
{
    /// <summary>
    /// 类      名：LogicGaming
    /// 功      能：游戏逻辑静态类
    /// 日      期：2015-03-26
    /// 修      改：2015-03-26
    /// 作      者：ls9512
    /// </summary>
    public static class LogicGaming
    {
        /// <summary>
        /// 游戏运行逻辑
        /// </summary>
        public static void Gaming()
        {
            LogicMap.MapAnimaEffect();

            if (GS.IsPause)
            {
                if (!GS.IsPauseCountDown)
                {
                    UM.Btn_PauseBackTitle.UILogic();
                    UM.Btn_Resume.UILogic();
                }
                return;
            }

            if (GS.IsGameInit)
            {
                LogicPlayer.MainPlayerLogic();
                LogicItem.MoveItemList();
                LogicBlock.MoveBlockList();
                LogicItem.AddRoadBlock();
                LogicItem.AddRocket();
                LogicMap.MoveOrnamentList();
                LogicMap.MoveBackground();
                LogicMap.SpeedUp();
                LogicPlayer.RandExpression();
                // 检测游戏结束
                LogicMap.GameOverCheck();
            }
        }
    }
}
