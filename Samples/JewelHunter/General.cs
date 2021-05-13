using System.Drawing;

namespace JewelHunter
{
    /// <summary>
    /// 类      名：General
    /// 功      能：全局静态参数类
    /// 日      期：2015-11-24
    /// 修      改：2016-04-14
    /// 作      者：ls9512
    /// </summary>
    public static class General
    {
        public static Rectangle DrawRect = new Rectangle(0, 0, 1020, 720);

        public static bool DebugShowHitCheckRect = false;
        public static bool DebugShowRect = false;
        public static Color DebugColorHitCheckRect = Color.GreenYellow;

        public static string ProTitle = "Jewel Hunter";

        public static string DataPath = @"..\..\Samples\JewelHunter_Data";

        #region Config配置参数
        public static bool GameBgm = true;
        public static bool GameSe = true;
        public static int GameFps = 120;
        public static bool GameMouseEffect = true;
        #endregion
    }

    /// <summary>
    /// 游戏阶段
    /// </summary>
    public enum GamePhase
    {
        /// <summary>
        /// 标题界面
        /// </summary>
        Title = 1,
        /// <summary>
        /// 菜单界面
        /// </summary>
        Menu = 2,
        /// <summary>
        /// 游戏界面
        /// </summary>
        Gaming = 3,
        /// <summary>
        /// 游戏结束界面
        /// </summary>
        GameOver = 4,
        /// <summary>
        /// 帮助界面
        /// </summary>
        Help = 5
    }
}
