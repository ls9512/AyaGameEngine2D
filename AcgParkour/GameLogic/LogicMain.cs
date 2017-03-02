using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using AcgParkour.Models;
using AcgParkour.GameUI;

using AyaGameEngine2D;

using GS = AcgParkour.Game.GameStatus;
using TM = AcgParkour.GameGraphic.TextureManager;
using SM = AcgParkour.GameIO.SoundManager;
using UM = AcgParkour.GameGraphic.UIManager;

namespace AcgParkour.GameLogic
{
    /// <summary>
    /// 类      名：LogicMain
    /// 功      能：游戏主逻辑类，游戏中所有逻辑方法在此实现
    /// 日      期：2015-03-21
    /// 修      改：2015-03-23
    /// 作      者：ls9512
    /// </summary>
    public class LogicMain
    {
        /// <summary>
        /// 游戏核心逻辑
        /// </summary>
        public void Logic()
        {
            switch (GS.GamePhase)
            {
                case GamePhase.Title:
                    LogicTitle.TitleEffect();
                    break;
                case GamePhase.MainMenu:
                    LogicTitle.TitleEffect();
                    LogicTitle.UILogic();
                    break;
                case GamePhase.Gaming:
                    LogicGaming.Gaming();
                    break;
                case GamePhase.About:
                    UM.Btn_AboutBackTitle.UILogic();
                    break;
                case GamePhase.Help:
                    UM.Btn_HelpBackTitle.UILogic();
                    UM.Btn_Next.UILogic();
                    break;
            }
        }

        /// <summary>
        /// 游戏标题
        /// </summary>
        public void GameTitle()
        {
            GS.GamePhase = GamePhase.Title;
        }

        /// <summary>
        /// 新游戏
        /// </summary>
        public void NewGame()
        {
            // 设置状态
            GS.GamePhase = GamePhase.Gaming;
            GS.GameMode = GameMode.Normal;
            GS.IsPause = false;
            GS.Score = 0;
            GS.ScoreDistance = 0;
            GS.Combo = 0;
            GS.Accuracy = 0;
            GS.ItemGet = 0;
            GS.ItemLost = 0;
            GS.ItemCount = 0;
            GS.MaxCombo = 0;
            GS.MoveSpeed = General.Game_DefMoveSpeed;
            GS.BackMoveSpeed = GS.MoveSpeed / 2f;
            GS.GravitySpeed = General.Game_DefGravitySpeed;
            // 清空粒子效果
            GS.EffectItemList.Clear();
            // 创建玩家
            LogicPlayer.CreatePlayer();
            // 创建列表
            GS.BlockList = new List<Block>();
            GS.ItemList = new List<BaseItem>();
            GS.OrnamentList = new List<Ornament>();
            GS.AnimationList = new List<Animation>();
            LogicBlock.CreateNewBlockList();
            // 重设告示牌
            LogicMap.ResetBillboard();
            // 初始化完成
            GS.IsGameInit = true;
        }
    }
}
