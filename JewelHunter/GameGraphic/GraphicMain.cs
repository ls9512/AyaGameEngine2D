using System;
using System.Collections.Generic;
using System.Drawing;

using AyaGameEngine2D;
using JewelHunter.Models;
using TM = JewelHunter.GameGraphic.TextureManager;
using UI = JewelHunter.GameGraphic.UiManager;
using GS = JewelHunter.Game.GameStatus;
using GH = AyaGameEngine2D.GraphicHelper;

namespace JewelHunter.GameGraphic
{
    /// <summary>
    /// 类      名：GraphicMain
    /// 功      能：游戏主绘图类
    /// 日      期：2015-11-22
    /// 修      改：2016-04-14
    /// 作      者：ls9512
    /// </summary>
    public static class GraphicMain
    {
        /// <summary>
        /// 主游戏绘图函数
        /// </summary>
        public static void Draw()
        {
            // 根据不同游戏阶段绘图
            switch (GS.GamePhase)
            {
                case GamePhase.Menu:
                    DrawMenu();
                    DrawStarEffect();
                    break;
                case GamePhase.Help:
                    DrawHelp();
                    DrawStarEffect();
                    break;
                case GamePhase.Gaming:
                    DrawJewelBoard();
                    DrawGameUi();
                    break;
                case GamePhase.GameOver:
                    DrawGameOver();
                    DrawStarEffect();
                    break;
            }

            DrawMouse();
        }

        #region 绘制 粒子效果
        /// <summary>
        /// 绘制粒子效果
        /// </summary>
        public static void DrawStarEffect()
        {
            // 绘制效果动画
            if (GS.EffectItemList != null)
            {
                foreach (EffectItem item in GS.EffectItemList)
                {
                    item.DrawEffectItem();
                }
            }
        } 
        #endregion

        #region 绘制 主菜单界面
        /// <summary>
        /// 绘制 主菜单界面
        /// </summary>
        public static void DrawMenu()
        {
            GH.DrawImage(TM.TextureUiMeneBackground.TextureID, General.DrawRect);
            UI.BtnStart.UIGraphic();
            UI.BtnHelp.UIGraphic();
            UI.BtnExit.UIGraphic();
        }
        #endregion

        #region 绘制 帮助界面
        /// <summary>
        /// 绘制 主菜单界面
        /// </summary>
        public static void DrawHelp()
        {
            GH.DrawImage(TM.TextureUiHelp.TextureID, General.DrawRect);
            UI.BtnBack.UIGraphic();
        }
        #endregion

        #region 绘制 游戏界面
        /// <summary>
        /// 下一个宝石偏移坐标
        /// </summary>
        public static PointF JewelNextOffsetPoint = new PointF(0, 0);

        /// <summary>
        /// 动画列表
        /// </summary>
        public static List<Animation> AnimationList = new List<Animation>();

        /// <summary>
        /// 绘制 - 宝石棋盘
        /// </summary>
        public static void DrawJewelBoard()
        {
            // 绘制游戏背景
            // GH.DrawImage(TM.Texture_UI_GameBack.TextureID, General.Draw_Rect, 200);
            GH.DrawImage(TM.TextureUiGameBack.TextureID, General.DrawRect, Color.White, Color.White, Color.FromArgb(255, 0, 0, 0), Color.FromArgb(255, 0, 0, 0));

            // 绘制棋盘
            GH.DrawImage(TM.TextureUiJewelBoard.TextureID, GS.JewelStartPoint.X, GS.JewelStartPoint.Y, 640, 640, 150);

            if (!GS.IsPause)
            {
                // 绘制宝石
                foreach (Jewel jewel in GS.JewelList)
                {
                    jewel.Graphic();
                }

                // 绘制下一个宝石
                float x = GS.JewelNextLoaction.X;
                float y = GS.JewelNextLoaction.Y;
                float width = 15;
                GH.DrawImage(TM.TextureJewel[GS.JewelNext].TextureID, x + JewelNextOffsetPoint.X,
                    y + JewelNextOffsetPoint.Y, 80, 80);
                GH.DrawImage(TM.TextureUiJewelNextRect.TextureID, x - width, y - width, 80 + width*2, 80 + width*2, 210);

                // 绘制鼠标选中状态
                Point mouse = GameLogic.LogicMain.GetJewelPoint(Input.MousePosition);
                if (mouse.X != -1 && mouse.Y != -1)
                {
                    // 可操作和禁用状态按不同亮度绘制
                    if (GameLogic.LogicMain.CheckJewelMoving())
                    {
                        GH.DrawImage(TM.TextureUiMouseMoveRect.TextureID, mouse.X*80 + GS.JewelStartPoint.X,
                            mouse.Y*80 + GS.JewelStartPoint.Y, 80, 80, Color.Gray);
                    }
                    else
                    {
                        GH.DrawImage(TM.TextureUiMouseMoveRect.TextureID, mouse.X*80 + GS.JewelStartPoint.X,
                            mouse.Y*80 + GS.JewelStartPoint.Y, 80, 80);
                    }
                }
            }
            else
            {
                // 暂停状态
                GH.FillRectangle(Color.FromArgb(100, Color.Black), General.DrawRect);
                GH.DrawImage(TM.TextureUiPause.TextureID, 150, 100, General.DrawRect.Width - 300, General.DrawRect.Height - 200);
                UI.BtnResume.UIGraphic();
            }
        }

        /// <summary>
        /// 绘制 - 游戏UI
        /// </summary>
        public static void DrawGameUi()
        {
            // 绘制分数
            GH.DrawNumberByTextureMatrix(TM.TextureUiScore, 700, 40, 30, 45, GS.Score, 1);

            // 绘制时间进度条
            int time_width = 300, xx = 700, yy = 80;
            GH.DrawImage(TM.TextureUiTimeBarBack.TextureID, xx, yy, time_width, 70);
            GH.DrawImage(TM.TextureUiTimeBar.TextureID, new SizeF(391, 33), new RectangleF(0, 0, 391f * GS.TimeDraw / GS.TimeMax, 33), new RectangleF(xx + 11, yy + 24, (time_width - 27) * GS.TimeDraw / GS.TimeMax, 24), 240);
        }
        #endregion

        #region 绘制 游戏结束界面
        /// <summary>
        /// 绘制游戏结束画面
        /// </summary>
        public static void DrawGameOver()
        {
            GH.DrawImage(TM.TextureUiGameOver.TextureID, General.DrawRect);
            // 绘制分数
            GH.DrawNumberByTextureMatrix(TM.TextureUiScore, 520, 312, 35, 60, GS.Score, 1);
            UI.BtnBack.UIGraphic();
        } 
        #endregion

        #region 绘制 鼠标
        /// <summary>
        /// 绘制鼠标
        /// </summary>
        public static void DrawMouse()
        {
            // 绘制鼠标
            DrawMouseEffect();

            // 绘制动画
            for (int i = AnimationList.Count - 1; i >= 0; i--)
            {
                GH.DrawImage(AnimationList[i].GetTextureFrameID(), AnimationList[i].X, AnimationList[i].Y, AnimationList[i].Width, AnimationList[i].Height);
                if (AnimationList[i].IsEnd)
                {
                    AnimationList.Remove(AnimationList[i]);
                }
            }
        } 
        #endregion

        #region 鼠标效果绘制
        /// <summary>
        /// 鼠标轨迹列表
        /// </summary>
        private static readonly List<EffectMouse> MouseLoc = new List<EffectMouse>();
        /// <summary>
        /// 鼠标HSV颜色
        /// </summary>
        private static ColorHSV _mouseColor = new ColorHSV(0, 200, 255);

        /// <summary>
        /// 绘制鼠标效果
        /// </summary>
        public static void DrawMouseEffect()
        {
            // 是否显示特效
            if (General.GameMouseEffect)
            {

                // 检测是否添加移动轨迹
                if (MouseLoc.Count > 0)
                {
                    // 距离大于一定数值才加入该序列
                    if (GameSupport.LengthFromPointToPoint(Input.MousePosition, MouseLoc[MouseLoc.Count - 1].Point) > 5)
                    {
                        MouseLoc.Add(new EffectMouse(Input.MousePosition));
                    }
                }
                else
                {
                    MouseLoc.Add(new EffectMouse(Input.MousePosition));
                }

                if (MouseLoc.Count > 0)
                {
                    // 透明
                    for (int i = MouseLoc.Count - 1; i >= 0; i--)
                    {
                        MouseLoc[i].Pellucidity -= 5f * Time.DeltaTime;
                    }

                    // 删除
                    for (int i = MouseLoc.Count - 1; i >= 0; i--)
                    {
                        if (Math.Abs(MouseLoc[i].Pellucidity) < 1e-6)
                        {
                            MouseLoc.Remove(MouseLoc[i]);
                        }
                    }
                }

                int h = _mouseColor.H;
                h++;
                if (h >= 360) h = 0;
                _mouseColor.H = h;
                Color c = _mouseColor.GetColor();
                Color c2 = Color.FromArgb(250, Color.White);

                // 绘制
                for (int i = 0; i < MouseLoc.Count; i++)
                {
                    if (i == MouseLoc.Count - 1)
                    {
                        GH.SetLineWitth(8f);
                        GH.DrawLine(Color.FromArgb((int)MouseLoc[i].Pellucidity, c), MouseLoc[i].Point, Input.MousePosition);
                        GH.SetLineWitth(4f);
                        GH.DrawLine(Color.FromArgb((int)(MouseLoc[i].Pellucidity / 2), c2), MouseLoc[i].Point, Input.MousePosition);
                    }
                    else
                    {
                        GH.SetLineWitth(8f);
                        GH.DrawLine(Color.FromArgb((int)MouseLoc[i].Pellucidity, c), MouseLoc[i].Point, MouseLoc[i + 1].Point);
                        GH.SetLineWitth(4f);
                        GH.DrawLine(Color.FromArgb((int)(MouseLoc[i].Pellucidity / 2), c2), MouseLoc[i].Point, MouseLoc[i + 1].Point);
                    }
                }
            }

            // 鼠标测试
            GH.DrawImage(TM.TextureUiMouse.TextureID, Input.MousePosition.X - 7, Input.MousePosition.Y - 5, TM.TextureUiMouse.Width, TM.TextureUiMouse.Height);
        }
        #endregion
    }
}
