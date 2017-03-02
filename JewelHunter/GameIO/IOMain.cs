using System.Windows.Forms;
using System.Drawing;
using AyaGameEngine2D;
using GS = JewelHunter.Game.GameStatus;
using SM = JewelHunter.GameIO.SoundManager;

namespace JewelHunter.GameIO
{
    /// <summary>
    /// 类      名：IOMain
    /// 功      能：游戏主输入输出处理类
    /// 日      期：2015-11-22
    /// 修      改：2016-04-14
    /// 作      者：ls9512
    /// </summary>
    public static class IoMain
    {
        public static void Io()
        {
            switch (GS.GamePhase)
            {
                case GamePhase.Menu:
                    // 按空格开始游戏
                    if (Input.IsKeyPressed(Keys.Space))
                    {
                        GS.GamePhase = GamePhase.Gaming;
                        GS.NewGame();
                    }
                    break;
                case GamePhase.Help:
                    // 按空格开始游戏
                    if (Input.IsKeyPressed(Keys.Escape))
                    {
                        GS.GamePhase = GamePhase.Menu;
                    }
                    break;
                case GamePhase.Gaming:
                    // 调试用重新生成
                    if (Input.IsKeyPressed(Keys.R))
                    {
                        GameLogic.LogicMain.NewGame();
                    }
                    if (Input.IsKeyPressed(Keys.Y))
                    {
                        GS.JewelList.Clear();
                        for (int i = 0; i < 8; i++)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                GS.JewelArray[i, j] = RandomHelper.RandInt(0, 7);
                                Models.Jewel jewel = new Models.Jewel(i, j, GS.JewelArray[i, j], true);
                                GS.JewelList.Add(jewel);
                            }
                        }
                    }
                    // 鼠标单击宝石
                    if (Input.IsMouseLeft)
                    {
                        // 如果宝石在移动中
                        if (GameLogic.LogicMain.CheckJewelMoving())
                        {
                            // 播放声音
                            SM.PlayMouseClickNo();
                            return;
                        }
                        Point point = GameLogic.LogicMain.GetJewelPoint(Input.MousePosition);
                        if (point.X != -1 && point.Y != -1)
                        {
                            // 点击宝石进行交换
                            if (GameLogic.LogicMain.ChangeJewel(point))
                            {
                                // 播放声音
                                SM.PlayMouseClick();
                            }
                            else
                            {
                                // 播放声音
                                SM.PlayMouseClickNo();
                            }
                        }
                    }
                    // 按ESC返回标题
                    if (Input.IsKeyPressed(Keys.Escape))
                    {
                        GS.GamePhase = GamePhase.Menu;
                    }
                    break;
                case GamePhase.GameOver:

                    break;
            }
        }
    }
}


