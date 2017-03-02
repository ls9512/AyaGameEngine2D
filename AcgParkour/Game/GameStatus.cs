using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using AcgParkour.Models;
using AcgParkour.GameGraphic;
using AcgParkour.GameLogic;
using AcgParkour.GameIO;
using AcgParkour.GameUI;

using AyaGameEngine2D;

using GS = AcgParkour.Game.GameStatus;
using TM = AcgParkour.GameGraphic.TextureManager;
using SM = AcgParkour.GameIO.SoundManager;
using UM = AcgParkour.GameGraphic.UIManager;

using KM = AyaGameEngine2D.KeyManager;

namespace AcgParkour.Game
{
    /// <summary>
    /// 类      名：GameStatus
    /// 功      能：游戏状态静态类，游戏中所有相关数据在此静态存储方便调用
    /// 日      期：2015-11-12
    /// 修      改：2015-03-21
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public static class GameStatus
    {
        #region 游戏绘图、逻辑、IO实例和调用
        /// <summary>
        /// 游戏绘图类实例
        /// </summary>
        [NonSerialized]
        public static GraphicMain _gameGraphic;

        /// <summary>
        /// 游戏逻辑类实例
        /// </summary>
        [NonSerialized]
        public static LogicMain _gameLogic;

        /// <summary>
        /// 游戏IO处理类实例
        /// </summary>
        [NonSerialized]
        public static IOMain _gameIO;
        /// <summary>
        /// 游戏绘图
        /// </summary>
        public static void GameDraw()
        {
            _gameGraphic.Draw();
        }

        /// <summary>
        /// 游戏逻辑
        /// </summary>
        public static void GameLogic()
        {
            _gameLogic.Logic();
        }

        /// <summary>
        /// 游戏输入输出
        /// </summary>
        public static void GameIO()
        {
            _gameIO.IO();
        }
        #endregion

        #region 游戏状态数据
        /// <summary>
        /// 游戏阶段
        /// </summary>
        public static GamePhase GamePhase;
        /// <summary>
        /// 游戏是否完成初始化
        /// </summary>
        public static bool IsGameInit;
        /// <summary>
        /// 是否暂停游戏
        /// </summary>
        public static bool IsPause;
        /// <summary>
        /// 是否暂停倒计时
        /// </summary>
        public static bool IsPauseCountDown;
        /// <summary>
        /// 游戏模式
        /// </summary>
        public static GameMode GameMode;
        /// <summary>
        /// 游戏玩家对象
        /// </summary>
        public static Player GamePlayer;
        /// <summary>
        /// 图块列表
        /// </summary>
        public static List<Block> BlockList;
        /// <summary>
        /// 物件列表
        /// </summary>
        public static List<BaseItem> ItemList;
        /// <summary>
        /// 饰品物件列表
        /// </summary>
        public static List<Ornament> OrnamentList;
        /// <summary>
        /// 一次性动画列表
        /// </summary>
        public static List<Animation> AnimationList;
        /// <summary>
        /// 效果物件列表
        /// </summary>
        public static List<EffectItem> EffectItemList;

        /// <summary>
        /// 分数
        /// </summary>
        public static int Score = 0;
        /// <summary>
        /// 距离积分
        /// </summary>
        public static int ScoreDistance = 0;
        /// <summary>
        /// 连击  
        /// </summary>
        public static int Combo = 0;
        /// <summary>
        /// 收获率
        /// </summary>
        public static float Accuracy = 0;
        /// <summary>
        /// 收获物件数量
        /// </summary>
        public static int ItemGet = 0;
        /// <summary>
        /// 总物件数量
        /// </summary>
        public static int ItemCount = 0;
        /// <summary>
        /// 丢失物件数
        /// </summary>
        public static int ItemLost = 0;
        /// <summary>
        /// 最大连击
        /// </summary>
        public static int MaxCombo = 0;
        /// <summary>
        /// 地图移动速度
        /// </summary>
        public static float MoveSpeed = 10f;
        
        /// <summary>
        /// 背景移动速度
        /// </summary>
        public static float BackMoveSpeed = MoveSpeed / 2f;
        /// <summary>
        /// 背景图绘制坐标
        /// </summary>
        public static float BackgroundLoc = 0;
        /// <summary>
        /// 飞行效果绘制坐标
        /// </summary>
        public static float FlyEffectLoc = 0;
        /// <summary>
        /// 玩家飞行速度
        /// </summary>
        public static float PlayerFlySpeed = 0f;
        /// <summary>
        /// 地图行走高度容差
        /// </summary>
        public static int GroundWalkHeightOffest = 15;
        /// <summary>
        /// 图块宽度
        /// </summary>
        public static int BlockWidth = 72;
        /// <summary>
        /// 重力加速度
        /// </summary>
        public static float GravitySpeed = 0.8f; 
        #endregion

        /// <summary>
        /// 游戏初始化
        /// </summary>
        public static void GameInit()
        {
            // 未初始化
            GS.IsGameInit = false;

            // 实例化
            _gameGraphic = new GraphicMain();
            _gameLogic = new LogicMain();
            _gameIO = new IOMain();
            // 加载纹理数据
            TM.InitTextureUI();
            TM.InitTextureAll();
            // 加载音频数据
            SM.SoundInit();
            // 加载UI控件
            UM.LoadUI();

            // 进入标题界面
            _gameLogic.GameTitle();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public static void Dispose()
        {
            // 释放纹理资源
            TM.Dispose();
            // 释放音频资源
            SM.Dispose();
        }
    }
}
