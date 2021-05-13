using System;
using System.Collections.Generic;
using System.Text;

using AcgParkour.Models;

using AyaGameEngine2D;

using GS = AcgParkour.Game.GameStatus;
using SM = AcgParkour.GameIO.SoundManager;
using TM = AcgParkour.GameGraphic.TextureManager;

namespace AcgParkour.GameLogic
{
    /// <summary>
    /// 类      名：LogicItem
    /// 功      能：物件逻辑静态类
    /// 日      期：2015-03-23
    /// 修      改：2015-03-23
    /// 作      者：ls9512
    /// </summary>
    public static class LogicItem
    {
        /// <summary>
        /// 最后一次创建位置
        /// </summary>
        private static int lastItemLocX = 0;

        /// <summary>
        /// 添加新物件
        /// </summary>
        public static void AddItemList()
        {
            if (GS.ScoreDistance < General.Game_Distance_Normal) return;
            // 样式总数
            int count = Convert.ToInt32(ItemStyle.StyleGrid.GetLongLength(0));
            int index = RandomHelper.RandInt(0, count);

            // 计算添加起始位置
            float loc_x = 0, loc_y = 0;
            int width = 64, height = 64, blank_x = 5, blank_y = -10; ;
            if (GS.ItemList.Count == 0)
            {
                loc_x = 200;
            }
            else
            {
                loc_x = 200;
                for (int i = GS.ItemList.Count - 1; i >= 0; i--)
                {
                    if (GS.ItemList[i].Type == ItemType.Normal)
                    {
                        loc_x = GS.ItemList[i].X + width;
                        return;
                    }
                }
            }

            // 距离过近则不创建
            if (GS.ItemList.Count != 0 && loc_x > 200)
            {
                return;
            }

            loc_x += General.Draw_Rect.Width;
            loc_y = LogicBlock.BlockCreateHeight[LogicBlock.BlockHeightIndex] - 20;

            int classIndex = RandomHelper.RandInt(0, 8);
            int frameIndex = 0;

            // 根据样式循环添加
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (ItemStyle.StyleGrid[index, i, j] != 0)
                    {
                        // 左下 9,0  右上 0,9
                        ItemGold item = new ItemGold();
                        item.X = loc_x + j * (width + blank_x);
                        item.Y = loc_y - (9 - i) * (height + blank_y);
                        item.Width = 48;
                        item.Height = 48;
                        // 分值
                        item.Value = (classIndex + 1) * 50;
                        item.FlowFrame = RandomHelper.RandFloat(-10, 0);
                        item.Frame = frameIndex++;
                        if (frameIndex > 3) frameIndex = 0;
                        item.Index = classIndex;
                        GS.ItemList.Add(item);
                        GS.ItemCount++;
                    }
                }
            }

            // 记录最后一次创建的位置，防止重叠创建
            lastItemLocX = (int)loc_x + 9 * (width + blank_x);
        }

        /// <summary>
        /// 移动物件列表
        /// </summary>
        public static void MoveItemList()
        {
            // 物件逻辑
            for (int i = GS.ItemList.Count - 1; i >= 0; i--)
            {
                GS.ItemList[i].ItemLogic();
            }

            // 删除旧的物件
            for (int i = GS.ItemList.Count - 1; i >= 0; i--)
            {
                // 在屏幕左边的未吸引物件，则废弃
                if (GS.ItemList[i].X + GS.ItemList[i].Width * 4 < General.Draw_Rect.Left)
                {
                    if (GS.ItemList[i].Type == ItemType.Normal && GS.ItemList[i].ItemStatus == ItemStatus.Fly)
                    {
                        return;
                    }
                    // 断连表情
                    if (GS.Combo > 0 && GS.ItemList[i].Type == ItemType.Normal)
                    {
                        if (GS.GamePlayer.Expression == null)
                        {
                            GS.GamePlayer.Expression = new Expression(ExpressionType.Embarrassed);
                        }
                        // 连击清零
                        GS.Combo = 0;
                    }
                    if (GS.ItemList[i].Type == ItemType.Normal)
                    {
                        // 丢失计数
                        GS.ItemLost++;
                    }
                    GS.ItemList.Remove(GS.ItemList[i]);
                }
            }

            // 添加新物件
            if (GS.ItemList.Count < 10)
            {
                AddItemList();
                // AddBlockList(9, false);
            }

            // 更新收获率
            GS.Accuracy = GS.ItemGet * 1f / (GS.ItemGet + GS.ItemLost);
        }

        /// <summary>
        /// 添加火箭
        /// </summary>
        public static void AddRocket()
        {
            if (GS.ScoreDistance < General.Game_Distance_Rocket) return;
            // 概率随机
            int value = RandomHelper.RandInt(0, 1000);
            if (value < 995) return;
            int count = 0;
            foreach (BaseItem itemt in GS.ItemList)
            {
                if (itemt.Type == ItemType.Rocket) count++;
            }
            if (count > 0) return;
            // 出现位置，预留提示时间
            float x = General.Draw_Rect.Width * 3f;
            float y = GS.GamePlayer.Y + 20 ;
            if (y < 10) y = 10;
            ItemRocket item = new ItemRocket();
            item.X = x;
            item.Y = y;
            item.Width = 151;
            item.Height = 92;
            item.MoveX = GS.MoveSpeed * 1.6f;
            GS.ItemList.Add(item);
        }

        /// <summary>
        /// 添加路障
        /// </summary>
        public static void AddRoadBlock()
        {
            if (GS.ScoreDistance < General.Game_Distance_RoadBlock) return;
            if (GS.BlockList == null || GS.BlockList.Count < 2) return;    
            if (GS.BlockList[GS.BlockList.Count - 2].Type != BlockType.Normal && GS.BlockList[GS.BlockList.Count - 2].Type != BlockType.Pillar) return;
            // 概率随机
            int value = RandomHelper.RandInt(0, 1000);
            if (value < 995) return;          
            // 添加路障
            float x = GS.BlockList[GS.BlockList.Count - 2].X;
            if (IsHasRoadBlock(x)) return;
            float y = GS.BlockList[GS.BlockList.Count - 2].Y;
            ItemRoadBlock item = new ItemRoadBlock();
            item.X = x;
            item.Y = y - 75;
            item.Width = 90;
            item.Height = 90;
            item.FlowFrame = RandomHelper.RandFloat(0, 90);
            GS.ItemList.Add(item);
        }

        /// <summary>
        /// 检测该位置是否有障碍
        /// </summary>
        /// <param name="x">位置x</param>
        /// <returns>检测结果</returns>
        public static bool IsHasRoadBlock(float x)
        {
            bool result = false;
            foreach (BaseItem item in GS.ItemList)
            {
                if (Math.Abs(item.X - x) < 2)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
