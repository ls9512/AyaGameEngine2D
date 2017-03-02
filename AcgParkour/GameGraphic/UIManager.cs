using System;
using System.Collections.Generic;
using System.Text;

using AcgParkour.GameUI;
using GS = AcgParkour.Game.GameStatus;
using TM = AcgParkour.GameGraphic.TextureManager;

namespace AcgParkour.GameGraphic
{
    /// <summary>
    /// 类      名：UIManager
    /// 功      能：游戏UI管理器，用于加载游戏中的UI控件
    /// 日      期：2015-06-25
    /// 修      改：2015-06-25
    /// 作      者：ls9512
    /// </summary>
    public static class UIManager
    {
        public static Btn_Start Btn_Start;
        public static Btn_Help Btn_Help;
        public static Btn_Exit Btn_Exit;
        public static Btn_About Btn_About;
        public static Btn_Load Btn_Load;
        public static Btn_Rank Btn_Rank;
        public static Btn_Title Btn_PauseBackTitle;
        public static Btn_Title Btn_HelpBackTitle;
        public static Btn_Title Btn_AboutBackTitle;
        public static Btn_Resume Btn_Resume;
        public static Btn_Next Btn_Next;

        /// <summary>
        /// 加载UI控件
        /// </summary>
        public static void LoadUI()
        {
            // 加载主菜单
            Btn_Start = new Btn_Start(920, 270, TM.Texture_UI_Btn_Start);   
            Btn_Load = new Btn_Load(920, 332, TM.Texture_UI_Btn_Load);
            Btn_Load.Enable = false;
            Btn_Rank = new Btn_Rank(920, 394, TM.Texture_UI_Btn_Rank);
            Btn_Rank.Enable = false;
            Btn_Help = new Btn_Help(920, 456, TM.Texture_UI_Btn_Help);
            Btn_About = new Btn_About(920, 518, TM.Texture_UI_Btn_About);
            Btn_Exit = new Btn_Exit(920, 580, TM.Texture_UI_Btn_Exit);
            // 暂停界面
            Btn_Resume = new Btn_Resume(426, 510, TM.Texture_UI_Btn_Resume);
            Btn_PauseBackTitle = new Btn_Title(726, 510, TM.Texture_UI_Btn_Title);
            // 帮助界面
            Btn_HelpBackTitle = new Btn_Title(1120, 620, TM.Texture_UI_Btn_Title);
            Btn_Next = new Btn_Next(1120, 530, TM.Texture_UI_Btn_Next);
            // 关于界面
            Btn_AboutBackTitle = new Btn_Title(1120, 620, TM.Texture_UI_Btn_Title);
        }
    }
}
