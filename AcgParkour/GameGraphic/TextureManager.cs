using System;
using System.Collections.Generic;
using System.Text;

using AcgParkour.Models;

using AyaGameEngine2D;

namespace AcgParkour.GameGraphic
{
    /// <summary>
    /// 类      名：TextureManager
    /// 功      能：纹理管理静态类，对游戏纹理素材进行统一加载和管理
    /// 日      期：2015-03-26
    /// 修      改：2015-03-26
    /// 作      者：ls9512
    /// </summary>
    public static class TextureManager
    {
        // 以下是UI纹理
        public static Texture TextureMouse;
        public static TextureMatrix TextureScore;
        public static TextureMatrix TextureScoreDistance;
        public static Texture TextureCombo;
        public static TextureMatrix TextureComboNumber;
        public static Texture TextureGameOver;
        public static Texture TextureJumpIcon;
        public static Texture TextureAccuracy;

        public static Texture Texture_UI_ProcessBar;
        public static Texture Texture_UI_Process;
        public static Texture Texture_UI_Title_01;
        public static Texture Texture_UI_Title_02;
        public static Texture Texture_UI_Btn_PressSpace;
        public static Texture Texture_UI_Btn_Start;
        public static Texture Texture_UI_Btn_Help;
        public static Texture Texture_UI_Btn_Exit;
        public static Texture Texture_UI_Btn_About;
        public static Texture Texture_UI_Btn_Rank;
        public static Texture Texture_UI_Btn_Load;
        public static Texture Texture_UI_Win_Pause;
        public static Texture Texture_UI_Btn_Title;
        public static Texture Texture_UI_Btn_Resume;
        public static Texture Texture_UI_Btn_Next;
        public static Texture Texture_UI_CountDown_1;
        public static Texture Texture_UI_CountDown_2;
        public static Texture Texture_UI_CountDown_3;
        public static Texture Texture_UI_Win_Help;
        public static Texture Texture_UI_Win_About;

        // 帮助图片
        public static Texture[] Texture_UI_Help = new Texture[2];

        // 以下是游戏素材纹理
        public static TextureMatrix TextureBlock;
        public static Texture TextureBackGround;
        public static Texture TextureBackGroundBlur;
        public static TextureMatrix TextureItem;
        public static Animation AnimationPlayerFlyEffect;
        public static Texture TextureBackMoveEffect;
        public static Texture TextureLifeIcon;
        public static Animation AnimationSakura;
        public static AnimationTrans AnimationTransition;
        public static TextureMatrix TextureExpression;
        public static Texture TextureRoadBlock;
        public static Texture TextureRoadBlockBottom;
        public static Animation AnimationRoadBlockAttack;
        public static Texture TextureStarEffect;
        public static Texture TextureRocket;
        public static Animation AnimationRocketAttack;
        public static Texture TextureRocketWarning;
        public static Texture TextureRocketWarningLine;

        // 以下是物件饰品纹理
        public static Texture TextureOrnament_Billboard_Start;
        public static Texture TextureOrnament_Billboard_RoadBlock;
        public static Texture TextureOrnament_Billboard_Rocket;
        

        /// <summary>
        /// 加载UI纹理素材
        /// </summary>
        public static void InitTextureUI()
        {
            try
            {
                // 加载鼠标
                TextureMouse = new Texture(General.Data_Path + @"\Graphic\Icon\MouseCursor.png");
                // 加载标题界面
                Texture_UI_Title_01 = new Texture(General.Data_Path + @"\Graphic\UI\Title_01.jpg");
                Texture_UI_Title_02 = new Texture(General.Data_Path + @"\Graphic\UI\Title_02.jpg");
                Texture_UI_Btn_PressSpace = new Texture(General.Data_Path + @"\Graphic\UI\UI_Btn_PressSpace.png");
                // 加载进度条纹理
                Texture_UI_Process = new Texture(General.Data_Path + @"\Graphic\UI\UI_Process.png");
                Texture_UI_ProcessBar = new Texture(General.Data_Path + @"\Graphic\UI\UI_ProcessBar.png");
                // 加载星星特效
                TextureStarEffect = new Texture(General.Data_Path + @"\Graphic\Animation\FloatStar.png");
                // 加载开始界面按钮纹理
                Texture_UI_Btn_Start = new Texture(General.Data_Path + @"\Graphic\UI\Btn_Start.png");
                Texture_UI_Btn_Help = new Texture(General.Data_Path + @"\Graphic\UI\Btn_Help.png");
                Texture_UI_Btn_Exit = new Texture(General.Data_Path + @"\Graphic\UI\Btn_Exit.png");
                Texture_UI_Btn_About = new Texture(General.Data_Path + @"\Graphic\UI\Btn_About.png");
                Texture_UI_Btn_Rank = new Texture(General.Data_Path + @"\Graphic\UI\Btn_Rank.png");
                Texture_UI_Btn_Load = new Texture(General.Data_Path + @"\Graphic\UI\Btn_Load.png");
                Texture_UI_Btn_Next = new Texture(General.Data_Path + @"\Graphic\UI\Btn_Next.png");
                // 加载暂停界面素材
                Texture_UI_Win_Pause = new Texture(General.Data_Path + @"\Graphic\UI\UI_Win_Pause.png");
                Texture_UI_Btn_Title = new Texture(General.Data_Path + @"\Graphic\UI\Btn_Title.png");
                Texture_UI_Btn_Resume = new Texture(General.Data_Path + @"\Graphic\UI\Btn_Resume.png");
                // 加载暂停倒计时数字
                Texture_UI_CountDown_1 = new Texture(General.Data_Path + @"\Graphic\UI\UI_CountDown_1.png");
                Texture_UI_CountDown_2 = new Texture(General.Data_Path + @"\Graphic\UI\UI_CountDown_2.png");
                Texture_UI_CountDown_3 = new Texture(General.Data_Path + @"\Graphic\UI\UI_CountDown_3.png");
                // 加载帮助界面
                Texture_UI_Win_Help = new Texture(General.Data_Path + @"\Graphic\UI\UI_Win_Help.jpg");
                // 加载关于界面
                Texture_UI_Win_About = new Texture(General.Data_Path + @"\Graphic\UI\UI_Win_About.jpg");
                // 加载帮助文件
                Texture_UI_Help[0] = new Texture(General.Data_Path + @"\Graphic\UI\UI_Help_1.png");
                Texture_UI_Help[1] = new Texture(General.Data_Path + @"\Graphic\UI\UI_Help_2.png");
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
                Environment.Exit(0);
            }
        }

        public static bool IsTexturoLoaded = false;

        /// <summary>
        /// 加载所有纹理素材
        /// </summary>
        public static void InitTextureAll()
        {
            try
            {
                // 加载地面
                TextureBlock = new TextureMatrix(General.Data_Path + @"\Graphic\Ground\Ground.png", 6, 1);
                // 加载背景
                TextureBackGround = new Texture(General.Data_Path + @"\Graphic\Background\Back_06.jpg");
                // 加载背景模糊图
                TextureBackGroundBlur = new Texture(General.Data_Path + @"\Graphic\Background\Back_Blur.jpg");
                // 地图移动效果
                TextureBackMoveEffect = new Texture(General.Data_Path + @"\Graphic\Animation\BackMoveEffect.png");
                // 加载物件素材
                TextureItem = new TextureMatrix(General.Data_Path + @"\Graphic\Item\TestItem.png", 4, 8);
                // 加载分数纹理
                TextureScore = new TextureMatrix(General.Data_Path + @"\Graphic\Number\Score.png", 10, 1);
                TextureScoreDistance = new TextureMatrix(General.Data_Path + @"\Graphic\Number\ScoreDistance.png", 10, 1);
                // 加载连击纹理
                TextureCombo = new Texture(General.Data_Path + @"\Graphic\Number\Combo.png");
                TextureComboNumber = new TextureMatrix(General.Data_Path + @"\Graphic\Number\ComboNumber.png", 10, 1);
                // 加载玩家飞行效果动画
                AnimationPlayerFlyEffect = new Animation(General.Data_Path + @"\Graphic\Animation\PlayerFlyEffect.png", 5, 4);
                AnimationPlayerFlyEffect.EndFrame = 14;
                AnimationPlayerFlyEffect.Speed = AyaGameEngine2D.FrameSpeed.Slowest;
                // 加载生命图标
                TextureLifeIcon = new Texture(General.Data_Path + @"\Graphic\Icon\Life.png");
                // 加载樱花花瓣动画纹理
                AnimationSakura = new Animation(General.Data_Path + @"\Graphic\Animation\Sakura.png", 5, 3);
                // 加载人物表情
                TextureExpression = new TextureMatrix(General.Data_Path + @"\Graphic\Animation\Expression.png", 8, 10);
                // 加载路障纹理
                TextureRoadBlock = new Texture(General.Data_Path + @"\Graphic\Item\RoadBlock.png");
                TextureRoadBlockBottom = new Texture(General.Data_Path + @"\Graphic\Item\RoadBlockBottom.png");
                // 加载火箭纹理
                TextureRocket = new Texture(General.Data_Path + @"\Graphic\Item\Rocket.png");
                AnimationRocketAttack = new Animation(General.Data_Path + @"\Graphic\Animation\RocketAttack.png", 5, 1);
                AnimationRocketAttack.Speed = AyaGameEngine2D.FrameSpeed.Slower;
                AnimationRocketAttack.IsLoop = false;
                // 加载火箭警示纹理
                TextureRocketWarning = new Texture(General.Data_Path + @"\Graphic\Item\RocketWarning.png");
                TextureRocketWarningLine = new Texture(General.Data_Path + @"\Graphic\Item\RocketWarningLine.png");
                // 加载障碍碰撞动画
                AnimationRoadBlockAttack = new Animation(General.Data_Path + @"\Graphic\Animation\RoadBlockAttack.png", 5, 1);
                AnimationRoadBlockAttack.Speed = AyaGameEngine2D.FrameSpeed.Slower;
                AnimationRoadBlockAttack.IsLoop = false;
                // 加载游戏结束背景图
                TextureGameOver = new Texture(General.Data_Path + @"\Graphic\UI\Gameover.jpg");
                // 加载跳跃图标
                TextureJumpIcon = new Texture(General.Data_Path + @"\Graphic\Icon\JumpIcon.png");
                // 加载收获率纹理
                TextureAccuracy = new Texture(General.Data_Path + @"\Graphic\Number\Accuracy.png");
                // 加载游戏开始提示牌纹理
                TextureOrnament_Billboard_Start = new Texture(General.Data_Path + @"\Graphic\Item\Billboard_Start.png");
                // 加载游戏路障提示牌纹理
                TextureOrnament_Billboard_RoadBlock = new Texture(General.Data_Path + @"\Graphic\Item\Billboard_RoadBlock.png");
                // 加载游戏火箭提示牌纹理
                TextureOrnament_Billboard_Rocket = new Texture(General.Data_Path + @"\Graphic\Item\Billboard_Rocket.png");
                // 加载完成
                IsTexturoLoaded = true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// 销毁 释放资源
        /// </summary>
        public static void Dispose()
        {
            try
            {
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
                Environment.Exit(0);
            }
        }
    }
}
