using System;
using System.Collections.Generic;
using System.Drawing;
using AyaGameEngine2D;
using JewelHunter.GameGraphic;
using JewelHunter.GameIO;
using JewelHunter.GameLogic;
using JewelHunter.Models;
using SoundManager = JewelHunter.GameIO.SoundManager;

namespace JewelHunter.Game
{
    /// <summary>
    /// 类      名：GameStatus
    /// 功      能：游戏状态类，对游戏进行中的各种数据进行封装
    /// 日      期：2015-11-22
    /// 修      改：2016-04-14
    /// 作      者：ls9512
    /// </summary>
    public static class GameStatus
    {
        #region 游戏绘图、逻辑、IO实例调用
        /// <summary>
        /// 游戏绘图
        /// </summary>
        public static void GameDraw()
        {
            GraphicMain.Draw();
        }

        /// <summary>
        /// 游戏逻辑
        /// </summary>
        public static void GameLogic()
        {
            LogicMain.Logic();
        }

        /// <summary>
        /// 游戏输入输出
        /// </summary>
        public static void GameIo()
        {
            IoMain.Io();
        }
        #endregion


        /// <summary>
        /// 效果物件列表
        /// </summary>
        public static List<EffectItem> EffectItemList;

        /// <summary>
        /// 游戏阶段
        /// </summary>
        public static GamePhase GamePhase = GamePhase.Title;

        /// <summary>
        /// 宝石起点位置
        /// </summary>
        public static Point JewelStartPoint = new Point(40, 40);

        /// <summary>
        /// 下一个交换宝石的坐标
        /// </summary>
        public static Point JewelNextLoaction = new Point(JewelStartPoint.X + 640 + 130, 540);

        public static int[,] JewelArray = new int[8, 8];
        public static List<Jewel> JewelList = new List<Jewel>();
        public static int JewelNext;
        public static bool IsJewelInit = false;
        public static bool IsPause = false;

        public static float TimeMax;
        public static float TimeNow;
        public static float TimeDraw;
        public static int Level;
        public static int Score;

        /// <summary>
        /// 游戏初始化
        /// </summary>
        public static void GameInit()
        { 
            // 初始化游戏阶段
            GamePhase = GamePhase.Menu;

            // 加载纹理资源
            TextureManager.InitTextureAll();
            // 加载声音资源
            SoundManager.SoundInit();
            // 加载UI
            UiManager.UiInit();
        }

        /// <summary>
        /// 新游戏
        /// </summary>
        public static void NewGame()
        {
            GamePhase = GamePhase.Gaming;
            LogicMain.NewGame();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public static void Dispose()
        {
            TextureManager.Dispose();
        }
    }
}
