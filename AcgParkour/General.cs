using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AcgParkour
{
    /// <summary>
    /// 全局参数
    /// </summary>
    public static class General
    {
        public static Rectangle Draw_Rect = new Rectangle(0, 0, 1280, 720);

        public static bool Debug_ShowHitCheckRect = false;
        public static bool Debug_ShowRect = false;
        public static Color Debug_ColorHitCheckRect = Color.GreenYellow;
        
        public static string Pro_Title = "Miku! Miku! Run!";

        public static string Data_Path = @"C:\Users\Administrator\Desktop\AGE2D\DataAcg";

        #region Config配置参数
        public static bool Game_BGM = false;
        public static bool Game_SE = false;
        public static int Game_Fps = 60;
        public static bool Game_MouseEffect = true; 
        #endregion

        public static ViewMode Game_ViewMode = ViewMode.Center;

        // 初始移动速度
        public static float Game_DefMoveSpeed = 9f;
        // 最大移动速度
        public static float Game_MaxMoveSpeed = 20f;
        // 移动提速
        public static float Game_AddMoveSpeed = 0.0005f;
        // 默认重力加速度
        public static float Game_DefGravitySpeed = 0.8f;
        // 距离换算系数(x像素=1m)
        public static int Game_DistancePixel = 100;

        
        // 以下是物件创建距离

        // 开始距离
        public static int Game_Distance_Start = Game_DistancePixel * 0;
        // 加分物件创建初始距离
        public static int Game_Distance_Normal = Game_DistancePixel * 15;
        // 路障提示距离
        public static int Game_Distance_Tip_RoadBlock = Game_DistancePixel * 130;
        // 路障创建初始距离
        public static int Game_Distance_RoadBlock = Game_DistancePixel * 170;
        // 火箭提示距离
        public static int Game_Distance_Tip_Rocket = Game_DistancePixel * 350;
        // 火箭创建初始距离
        public static int Game_Distance_Rocket = Game_DistancePixel * 390;   
       
    }

    /// <summary>
    /// 显示模式
    /// </summary>
    public enum ViewMode
    {
        Normal,             // 普通，地图固定不动，人物移动
        Center,             // 人物自动保持在画面中心
    }

    /// <summary>
    /// 游戏阶段枚举
    /// </summary>
    public enum GamePhase
    {
        Logo,               // Logo
        Title,              // 标题1
        Loading,            // 加载资源
        MainMenu,           // 主菜单
        Gaming,             // 游戏中,按GameMode细分
        Help,               // 帮助
        About,              // 关于
        GameOver,           // 游戏结束
    }

    /// <summary>
    /// 游戏模式枚举
    /// </summary>
    public enum GameMode
    {
        Normal,
    }

    /// <summary>
    /// 地图块类型枚举
    /// </summary>
    public enum BlockType
    {
        Left,           // 左边
        Normal,         // 中间，正常连续
        Right,          // 右边
        Single,         // 单独
        Pillar,         // 柱子
    }

    /// <summary>
    /// 玩家状态枚举
    /// </summary>
    public enum PlayerStatus
    { 
        Stand,          // 站立
        Run,            // 奔跑
        Jump,           // 跳跃
        Glide,          // 滑翔
    }

    /// <summary>
    /// 物件类型枚举
    /// </summary>
    public enum ItemType
    { 
        Normal,         // 正常
        RoadBlock,      // 路障
        Rocket,         // 火箭
        Life,           // 生命                                                                                                                                                           
    }

    /// <summary>
    /// 物件状态枚举
    /// </summary>
    public enum ItemStatus
    { 
        Normal,         // 正常
        Fly,            // 飞行(被吸收)
        Attack,         // 已攻击
    }

    /// <summary>
    /// 表情类型枚举
    /// </summary>
    public enum ExpressionType
    {
        Surprised = 0,          // 惊讶
        Question = 1,           // 疑问
        Happy = 2,              // 喜悦
        Love = 3,               // 喜爱
        Embarrassed = 4,        // 尴尬
        Shame = 5,              // 汗颜
        Anxiety = 6,            // 烦躁
        Speechless  = 7,        // 无语
        Inspiration = 8,        // 灵感
        Sleepy = 9,             // 困倦
    } 
}
