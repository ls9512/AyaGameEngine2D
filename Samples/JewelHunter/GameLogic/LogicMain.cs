using System;
using System.Collections.Generic;
using System.Drawing;
using AyaGameEngine2D;
using JewelHunter.Models;
using GS = JewelHunter.Game.GameStatus;
using TM = JewelHunter.GameGraphic.TextureManager;
using UI = JewelHunter.GameGraphic.UiManager;
using SM = JewelHunter.GameIO.SoundManager;

namespace JewelHunter.GameLogic
{
    /// <summary>
    /// 类      名：LogicMain
    /// 功      能：游戏主逻辑类
    /// 日      期：2015-11-22
    /// 修      改：2016-04-14
    /// 作      者：ls9512
    /// </summary>
    public static class LogicMain
    {
        public static void Logic()
        {
            switch (GS.GamePhase)
            {
                case GamePhase.Menu:
                    UI.BtnStart.UILogic();
                    UI.BtnHelp.UILogic();
                    UI.BtnExit.UILogic();
                    TitleEffect();
                    break;
                case GamePhase.Help:
                    UI.BtnBack.UILogic();
                    TitleEffect();
                    break;
                case GamePhase.Gaming:
                    if (!GS.IsPause)
                    {
                        // 执行宝石逻辑
                        for (int i = GS.JewelList.Count - 1; i >= 0; i--)
                        {
                            GS.JewelList[i].Logic();
                        }
                        // 循环结束检测
                        LoopEndCheck();
                    }
                    else
                    {
                        UI.BtnResume.UILogic();
                    }
                    break;
                case GamePhase.GameOver:
                    UI.BtnBack.UILogic();
                    TitleEffect();
                    break;
            }
        }

        #region 标题界面粒子效果
        /// <summary>
        /// 上次创建时间
        /// </summary>
        private static int _lastEffectCreateTime;

        /// <summary>
        /// 标题画面效果逻辑
        /// </summary>
        public static void TitleEffect()
        {
            // 不存在则新建
            if (GS.EffectItemList == null)
            {
                GS.EffectItemList = new List<EffectItem>();
            }
            // 判断是否生成新的效果物件
            int time = Environment.TickCount;
            if (GS.EffectItemList.Count < 150 && time - _lastEffectCreateTime > 15)
            {
                _lastEffectCreateTime = time;
                EffectItem item = new EffectItem(TM.TextureStarEffect.TextureID, TM.TextureStarEffect.Width,
                    TM.TextureStarEffect.Height)
                {
                    X = RandomHelper.RandFloat(0, General.DrawRect.Width),
                    Y = -50
                };
                ;
                item.MoveX = RandomHelper.RandFloat(-20, 20) / 20;
                item.MoveY = RandomHelper.RandFloat(10, 30) / 10;
                item.AngleSpeed = RandomHelper.RandFloat(-20, 20) / 10;
                GS.EffectItemList.Add(item);
            }
            // 删除旧的效果物件
            for (int i = GS.EffectItemList.Count - 1; i >= 0; i--)
            {
                // 不在屏幕内或者透明的
                if ((GS.EffectItemList[i].X < -50 || GS.EffectItemList[i].Y > General.DrawRect.Height) || GS.EffectItemList[i].Pellucidity < 1)
                {
                    GS.EffectItemList.Remove(GS.EffectItemList[i]);
                }
            }
        } 
        #endregion

        #region 新游戏
        /// <summary>
        /// 新游戏
        /// </summary>
        public static void NewGame()
        {
            // 初始化标志
            GS.IsJewelInit = false;

            // 初始化数据
            GS.IsPause = false;
            GS.Score = 0;
            GS.Level = 1;
            GS.TimeMax = 600;
            GS.TimeNow = GS.TimeMax;
            GS.TimeDraw = GS.TimeMax;

            // 播放BGM
            SM.StopTitleBgm();
            SM.PlayGameBgm();

            // 初次随机生成
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    GS.JewelArray[i, j] = RandomHelper.RandInt(0, 7);
                }
            }
            // 检查是否存在可消除的,存在则替换其中一个直至整个棋盘无法消除
            List<Point> listCheck;
            do
            {
                listCheck = CheckClear();
                // 随机替换
                if (listCheck.Count > 0)
                {
                    int index = RandomHelper.RandInt(0, listCheck.Count);
                    GS.JewelArray[listCheck[index].X, listCheck[index].Y] = RandomHelper.RandInt(0, 7);
                }
            } while (listCheck.Count > 0);

            // 添加Jewel实例
            GS.JewelList.Clear();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Jewel jewel = new Jewel(i, j, GS.JewelArray[i, j], true);
                    GS.JewelList.Add(jewel);
                }
            }
            // 下一个用于交换的宝石
            GS.JewelNext = RandomHelper.RandInt(0, 7);
            // 初始化标志
            GS.IsJewelInit = true;
        } 
        #endregion

        #region 宝石数组和屏幕坐标转换
        /// <summary>
        /// 获屏幕坐标所在宝石的坐标
        /// </summary>
        /// <param name="screenPoint">屏幕坐标</param>
        /// <returns>在范围内则返回坐标，否则返回(-1,-1)</returns>
        public static Point GetJewelPoint(Point screenPoint)
        {
            float startX = GS.JewelStartPoint.X;
            float startY = GS.JewelStartPoint.Y;
            int mouseX = (int)((Input.MousePosition.X - startX) / 80);
            int mouseY = (int)((Input.MousePosition.Y - startY) / 80);
            if (Input.MousePosition.X >= startX && Input.MousePosition.Y >= mouseY && mouseX >= 0 && mouseY >= 0 && mouseX <= 7 && mouseY <= 7)
            {
                return new Point(mouseX, mouseY);
            }
            else
            {
                return new Point(-1, -1);
            }
        }

        /// <summary>
        /// 获取宝石坐标所在的屏幕坐标
        /// </summary>
        /// <param name="jewelPoint">宝石坐标</param>
        /// <returns>在范围内则返回坐标，否则返回(-1,-1)</returns>
        public static Point GetScreenPoint(Point jewelPoint)
        {
            float startX = GS.JewelStartPoint.X;
            float startY = GS.JewelStartPoint.Y;
            if (jewelPoint.X < 0 || jewelPoint.Y < 0 || jewelPoint.X > 7 || jewelPoint.Y > 7)
            {
                return new Point(-1, -1);
            }
            else
            {
                int x = (int)(jewelPoint.X * 80 + startX);
                int y = (int)(jewelPoint.Y * 80 + startY);
                return new Point(x, y);
            }
        } 
        #endregion

        #region 宝石状态检测和处理
        /// <summary>
        /// 循环结束检测
        /// </summary>
        public static void LoopEndCheck()
        {
            // 时间处理
            if (!CheckJewelMoving())
            {
                GS.TimeNow -= 2f * Time.DeltaTime;
            }
            if (GS.TimeDraw < GS.TimeNow - 1) GS.TimeDraw += 3f * Time.DeltaTime;
            if (GS.TimeDraw > GS.TimeNow + 1) GS.TimeDraw -= 2f * Time.DeltaTime;
            // 检测游戏结束
            if (GS.TimeNow <= 0)
            {
                GS.TimeNow = 0;
                GS.GamePhase = GamePhase.GameOver;
            }

            // 检查是否有空位并处理下落
            CheckBlank();
            // 检查下落后的消除状态
            DoClear();
            // 检查下一个宝石的可用性,不可用则重新生成直至可用
            bool changeNext = false;
            while (!CheckNextAvilable())
            {
                GS.JewelNext = RandomHelper.RandInt(0, 7);
                changeNext = true;
            }
            if (changeNext)
            {
                TM.AnimationJewelChage.Reset();
                GameGraphic.GraphicMain.AnimationList.Add(TM.AnimationJewelChage);
                // 播放音效
                SM.PlayChange();
            }
        }

        /// <summary>
        /// 检查下一个宝石是否可用
        /// </summary>
        public static bool CheckNextAvilable()
        {
            // 移动中则不检测
            if (CheckJewelMoving()) return true;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    // 暂时替换用于检测
                    int temp = GS.JewelArray[i, j];
                    GS.JewelArray[i, j] = GS.JewelNext;
                    int count = CheckClear().Count;
                    GS.JewelArray[i, j] = temp;
                    // 存在可替换的
                    if (count > 0) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查棋盘上是否有空位,如果有则处理下落
        /// </summary>
        /// <returns>检查结果</returns>
        public static void CheckBlank()
        {
            // 移动中则不检测
            if (CheckJewelMoving()) return;

            // 检查是否有空位
            bool result = false;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (GS.JewelArray[i, j] == -1)
                    {
                        result = true;
                    }
                }
            }

            // 有空档
            if (result)
            {
                // 记录每一列空格数
                int[] blankNum = new int[8];

                // 已有宝石下落处理
                for (int i = 0; i < 8; i++)
                {
                    int blank = 0;
                    for (int j = 7; j >= 0; j--)
                    {
                        if (GS.JewelArray[i, j] == -1)
                        {
                            blank++;
                        }
                        else
                        {
                            SetJewelDrop(new Point(i, j), new Point(i, j + blank));
                        }
                    }
                    blankNum[i] = blank;
                }

                // 生成新宝石填补上部空档
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < blankNum[i]; j++)
                    {
                        GS.JewelArray[i, j] = RandomHelper.RandInt(0, 7);
                        Jewel jewel = new Jewel(i, j, GS.JewelArray[i, j], true);
                        GS.JewelList.Add(jewel);
                    }
                }
            }
        }

        /// <summary>
        /// 设置某个宝石下落
        /// </summary>
        /// <param name="jewelPoint">宝石坐标</param>
        /// <param name="aimPoint">下落目的坐标</param>
        public static void SetJewelDrop(Point jewelPoint, Point aimPoint)
        {
            // jewelPoint位置的宝石下落到aimPoint
            foreach (Jewel jewel in GS.JewelList)
            {
                if (jewel.JewelPoint == jewelPoint)
                {
                    jewel.SetDropAimPoint(aimPoint);
                    break;
                }
            }
        }

        /// <summary>
        /// 进行消除
        /// </summary>
        public static void DoClear()
        {
            // 移动中则不检测
            if (CheckJewelMoving()) return;

            List<Point> list = CheckClear();

            if (list.Count == 0) return;

            // 消除加分
            int add = 100;
            int score = 0;
            for (int i = 0; i < list.Count; i++)
            {
                score += add;
                if (i >= 3) add += 50;
            }
            GS.Score += score;
            // 时间恢复
            GS.TimeNow += list.Count * 30f;
            if (GS.TimeNow > GS.TimeMax) GS.TimeNow = GS.TimeMax;

            // 设置宝石进入消除状态
            int index = 0;
            foreach (Point p in list)
            {
                foreach (Jewel jewel in GS.JewelList)
                {
                    if (jewel.JewelPoint == p)
                    {
                        Jewel temp = jewel;
                        Timer.Delay(() => { temp.JewelStatus = JewelStatus.Boom; }, index++ * 0.05f);
                        break;
                    }
                }
            }

            // 播放音效
            SM.PlayClear();
        }

        /// <summary>
        /// 将指定的宝石与下一个宝石进行交换
        /// </summary>
        /// <param name="point">宝石坐标</param>
        /// <returns>是否可以消除</returns>
        public static bool ChangeJewel(Point point)
        {
            int nowColor = GS.JewelArray[point.X, point.Y];
            GS.JewelArray[point.X, point.Y] = GS.JewelNext;
            // 无法完成交换
            if (CheckClear().Count == 0)
            {
                // 恢复
                GS.JewelArray[point.X, point.Y] = nowColor;
                return false;
            }

            // 找到列表中的的对应坐标的宝石
            for (int i = GS.JewelList.Count - 1; i >= 0; i--)
            {
                if (GS.JewelList[i].JewelPoint == point)
                {
                    // 找到
                    GS.JewelList[i].SetMoveAimLoaction(GS.JewelNextLoaction);
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// 检查宝石是否在移动中，用于判断鼠标是否允许操作
        /// </summary>
        /// <returns>判断结果</returns>
        public static bool CheckJewelMoving()
        {
            bool result = false;
            foreach (Jewel jewel in GS.JewelList)
            {
                if (jewel.JewelStatus != JewelStatus.Noraml)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 检查是否存在可以消除的，存在则返回可消除列表
        /// </summary>
        /// <returns>消除列表，如果为空则说明当前状态无法消除</returns>
        public static List<Point> CheckClear()
        {
            List<Point> list = new List<Point>();

            // 连续相同检测长度
            int length = 3;

            // 纵向扫描
            for (int i = 0; i < 8; i++)
            {
                // 扫描第i列
                for (int j = 0; j < 9 - length; )
                {
                    int start = j, end = j;
                    int color = GS.JewelArray[i, start];
                    for (int x = start + 1; x < 8; x++)
                    {
                        if (GS.JewelArray[i, x] == color)
                        {
                            end = x;
                        }
                        else
                        {
                            break;
                        }
                    }
                    // 找到
                    if (end - start >= length - 1)
                    {
                        j += end - start;
                        // 加入结果列表
                        for (int x = start; x <= end; x++)
                        {
                            Point p = new Point(i, x);
                            if (!list.Contains(p)) list.Add(p);
                        }
                    }
                    else
                    {
                        j++;
                    }
                }
            }

            // 横向扫描
            for (int j = 0; j < 8; j++)
            {
                // 扫描第j行
                for (int i = 0; i < 9 - length; )
                {
                    int start = i, end = i;
                    int color = GS.JewelArray[start, j];
                    for (int x = start + 1; x < 8; x++)
                    {
                        if (GS.JewelArray[x, j] == color)
                        {
                            end = x;
                        }
                        else
                        {
                            break;
                        }
                    }
                    // 找到
                    if (end - start >= length - 1)
                    {
                        i += end - start;
                        // 加入结果列表
                        for (int x = start; x <= end; x++)
                        {
                            Point p = new Point(x, j);
                            if (!list.Contains(p)) list.Add(p);
                        }
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            return list;
        } 
        #endregion
    }
}
