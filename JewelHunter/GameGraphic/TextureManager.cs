using System.Drawing;

using AyaGameEngine2D;

using GS = JewelHunter.Game.GameStatus;

namespace JewelHunter.GameGraphic
{
    /// <summary>
    /// 类      名：TextureManager
    /// 功      能：纹理管理静态类，对游戏纹理素材进行统一加载和管理
    /// 日      期：2015-11-23
    /// 修      改：2016-04-14
    /// 作      者：ls9512
    /// </summary>
    public static class TextureManager
    {
        // 宝石纹理数组
        public static Texture[] TextureJewel = new Texture[7];

        // 主菜单背景纹理
        public static Texture TextureUiMeneBackground;
        // 帮助界面纹理
        public static Texture TextureUiHelp;
        // 游戏结束界面纹理
        public static Texture TextureUiGameOver;

        // 菜单界面粒子纹理
        public static Texture TextureStarEffect;

        // 暂停界面窗口纹理
        public static Texture TextureUiPause;

        // 按钮UI
        public static Texture TextureUiBtnStart;
        public static Texture TextureUiBtnHelp;
        public static Texture TextureUiBtnExit;
        public static Texture TextureUiBtnResume;
        public static Texture TextureUiBtnBack;

        // 游戏背景纹理
        public static Texture TextureUiGameBack;
        // 鼠标纹理
        public static Texture TextureUiMouse;
        // 鼠标移动矩形纹理
        public static Texture TextureUiMouseMoveRect;
        // 下一个宝石纹理
        public static Texture TextureUiJewelNextRect;
        // 棋盘纹理
        public static Texture TextureUiJewelBoard;
        // 分数纹理
        public static TextureMatrix TextureUiScore;
        // 时间进度条纹理
        public static Texture TextureUiTimeBar;
        // 时间进度条结果纹理
        public static Texture TextureUiTimeBarBack;

        // 消除动画纹理
        public static Texture[] TexutreBoom = new Texture[8];

        // 宝石交换动画
        public static Animation AnimationJewelChage;

        /// <summary>
        /// 加载完成标识
        /// </summary>
        public static bool IsTexturoLoaded;

        /// <summary>
        /// 加载所有纹理素材
        /// </summary>
        public static void InitTextureAll()
        {
            // 加载标题界面背景图纹理
            TextureUiMeneBackground = new Texture(General.DataPath + @"\Graphic\UI\UI_Title.png");
            // 加载帮助界面背景图纹理
            TextureUiHelp = new Texture(General.DataPath + @"\Graphic\UI\UI_Help.png");
            // 加载游戏结束界面纹理
            TextureUiGameOver = new Texture(General.DataPath + @"\Graphic\UI\UI_GameOver.png");
            // 加载暂停界面窗口纹理
            TextureUiPause = new Texture(General.DataPath + @"\Graphic\UI\UI_Pause.png");

            // 加载按钮UI
            TextureUiBtnStart = new Texture(General.DataPath + @"\Graphic\UI\UI_Button_Start.png");
            TextureUiBtnHelp = new Texture(General.DataPath + @"\Graphic\UI\UI_Button_Help.png");
            TextureUiBtnExit = new Texture(General.DataPath + @"\Graphic\UI\UI_Button_Exit.png");
            TextureUiBtnResume = new Texture(General.DataPath + @"\Graphic\UI\UI_Button_Resume.png");
            TextureUiBtnBack = new Texture(General.DataPath + @"\Graphic\UI\UI_Button_Back.png");

            // 粒子纹理
            TextureStarEffect = new Texture(General.DataPath + @"\Graphic\Item\FloatStar.png");

            // 加载宝石纹理
            Bitmap bitmap = new Bitmap(General.DataPath + @"\Graphic\Item\Jewel.png");
            int width  = bitmap.Width / 7;
            for (int i = 0; i < 7; i++)
            {
                TextureJewel[i] = new Texture(BitmapHelper.BitmapCut(bitmap, i * width, 0, width, width));
            }

            // 加载游戏背景图纹理d
            TextureUiGameBack = new Texture(General.DataPath + @"\Graphic\UI\Game_Back.jpg");
            // 加载鼠标纹理
            TextureUiMouse = new Texture(General.DataPath + @"\Graphic\UI\Mouse.png");
            // 加载鼠标移动矩形纹理
            TextureUiMouseMoveRect = new Texture(General.DataPath + @"\Graphic\UI\Mouse_MoveRect.png");
            // 加载下一个宝石矩形纹理
            TextureUiJewelNextRect = new Texture(General.DataPath + @"\Graphic\UI\Jewel_NextRect.png");
            // 加载棋盘纹理
            TextureUiJewelBoard = new Texture(General.DataPath + @"\Graphic\UI\Jewel_Board.png"); 
            // 加载分数纹理
            TextureUiScore = new TextureMatrix(General.DataPath + @"\Graphic\Text\Score.png", 10, 1);
            // 加载时间进度条纹理
            TextureUiTimeBar = new Texture(General.DataPath + @"\Graphic\UI\Time_Bar.png");
            // 加载时间进度条背景纹理
            TextureUiTimeBarBack = new Texture(General.DataPath + @"\Graphic\UI\Time_Bar_Back.png"); 

            // 加载消除动画纹理
            bitmap = new Bitmap(General.DataPath + @"\Graphic\Animation\Clear.png");
            for (int i = 0; i < 5; i++)
            {
                TexutreBoom[i] = new Texture(BitmapHelper.BitmapCut(bitmap, 192 * i, 0, 192, 192));
            }
            for (int i = 0; i < 3; i++)
            {
                TexutreBoom[i + 5] = new Texture(BitmapHelper.BitmapCut(bitmap, 192 * i, 192, 192, 192));
            }

            // 加载宝石交换动画
            AnimationJewelChage = new Animation(General.DataPath + @"\Graphic\Animation\Change.png", 8, 1)
            {
                Speed = FrameSpeed.Slower,
                X = GS.JewelNextLoaction.X + 40 - 96,
                Y = GS.JewelNextLoaction.Y + 40 - 96,
                IsLoop = false
            };

            // 加载完成
            IsTexturoLoaded = true;
        }

        /// <summary>
        /// 销毁 释放资源
        /// </summary>
        public static void Dispose()
        {

        }
    }
}
