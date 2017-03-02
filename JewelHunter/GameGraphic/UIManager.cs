using JewelHunter.GameUI;
using TM = JewelHunter.GameGraphic.TextureManager;

namespace JewelHunter.GameGraphic
{
    /// <summary>
    /// 类      名：UiManager
    /// 功      能：UI管理类
    /// 日      期：2016-05-07
    /// 修      改：2016-05-07
    /// 作      者：ls9512
    /// </summary>
    public static class UiManager
    {
        public static BtnStart BtnStart;
        public static BtnHelp BtnHelp;
        public static BtnExit BtnExit;
        public static BtnResume BtnResume;
        public static BtnBack BtnBack;

        /// <summary>
        /// 初始化UI
        /// </summary>
        public static void UiInit()
        {
            BtnStart = new BtnStart(670, 350, TM.TextureUiBtnStart);
            BtnHelp = new BtnHelp(670, 430, TM.TextureUiBtnHelp);
            BtnExit = new BtnExit(670, 510, TM.TextureUiBtnExit);
            BtnBack = new BtnBack(830, 610, TM.TextureUiBtnBack);
            BtnResume = new BtnResume(450, 510, TM.TextureUiBtnResume);
        }
    }
}
