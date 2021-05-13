using System;
using System.Collections.Generic;
using System.Text;
using AcgParkour.Models;

using AyaGameEngine2D;

using GS = AcgParkour.Game.GameStatus;

namespace AcgParkour.GameLogic
{
    /// <summary>
    /// 类      名：LogicBlock
    /// 功      能：图块逻辑静态类
    /// 日      期：2015-03-23
    /// 修      改：2015-03-23
    /// 作      者：ls9512
    /// </summary>
    public static class LogicBlock
    {
        /// <summary>
        /// 图块数量统计
        /// </summary>
        public static int BlockCount;
        /// <summary>
        /// 图块创建高度
        /// </summary>
        public static int[] BlockCreateHeight = { 680, 620, 560, 500, 440, 320 };
        /// <summary>
        /// 创建高度索引
        /// </summary>
        public static int BlockHeightIndex = 0;
        /// <summary>
        /// 同一高度已创建数量
        /// </summary>
        private static int sameHeightNum = 0;

        /// <summary>
        /// 创建全新块列表
        /// </summary>
        public static void CreateNewBlockList()
        {
            BlockCount = 0;
            BlockHeightIndex = 0;
            AddBlockList(General.Draw_Rect.Width / GS.BlockWidth + 10, false, false);
        }

        /// <summary>
        /// 添加图块
        /// </summary>
        /// <param name="addNum">添加数量</param>
        /// <param name="isChangeHeight">允许改变高度</param>
        /// <param name="isBlank">允许留空</param>
        public static void AddBlockList(int addNum, bool isChangeHeight,bool isBlank)
        {
            float loc_x = 0, loc_y = 0;
            int width = GS.BlockWidth;
            if (GS.BlockList.Count == 0)
            {
                loc_x = 0;
            }
            else
            {
                loc_x = GS.BlockList[GS.BlockList.Count - 1].X + width;
            }
            loc_y = BlockCreateHeight[BlockHeightIndex];

            int createCount = 0;

            for (int i = 0; i < addNum; i++)
            {
                Block block = new Block(BlockType.Normal);
                block.X = loc_x;
                block.Y = loc_y;
                block.Width = width;
                block.Height = width;
                block.Value = BlockCount++;
                GS.BlockList.Add(block);
                createCount++;
                sameHeightNum++;
                int t;

                // 随机留空
                if (isBlank)
                {
                    t = RandomHelper.RandInt(0, 100);
                    if (t > 85 && createCount > 3)
                    {
                        loc_x += width * 4;
                        createCount = 0;
                    }
                }

                // 移动
                loc_x += width;

                // 随机改变高度
                t = RandomHelper.RandInt(0, 100);
                if (t > 20 && createCount > 3 && isChangeHeight && sameHeightNum > 3)
                {
                    sameHeightNum = 0;
                    int temp = 0;
                    while (temp == 0)
                    {
                        temp = RandomHelper.RandInt(-1, 2);
                    }
                    BlockHeightIndex += temp;
                    // 防溢出
                    if (BlockHeightIndex < 0) BlockHeightIndex = 0;
                    if (BlockHeightIndex > BlockCreateHeight.Length - 1) BlockHeightIndex = BlockCreateHeight.Length - 1;
                }
            }
            // 设置图块类型
            SetBlockType();
        }

        /// <summary>
        /// 移动图块列表
        /// 剔除旧的图块
        /// 添加新的图块
        /// </summary>
        public static void MoveBlockList()
        {
            // 移动
            for (int i = 0; i < GS.BlockList.Count; i++)
            {
                GS.BlockList[i].X -= (GS.MoveSpeed + GS.PlayerFlySpeed) * Time.DeltaTime;
            }

            // 删除旧的图块
            for (int i = GS.BlockList.Count - 1; i >= 0; i--)
            {
                // 如果在屏幕左边，则废弃
                if (GS.BlockList[i].X + GS.BlockList[i].Width * 2 < General.Draw_Rect.Left)
                {
                    GS.BlockList.Remove(GS.BlockList[i]);
                }
            }

            // 添加新图块
            if (GS.BlockList.Count < General.Draw_Rect.Width / GS.BlockWidth + 9)
            {
                LogicBlock.AddBlockList(9, true, true);
            }
        }

        /// <summary>
        /// 设置图块类型
        /// </summary>
        public static void SetBlockType()
        {
            if (GS.BlockList == null || GS.BlockList.Count == 0) return;
            bool hasLeft;
            bool hasRight;
            // 设置 左 中 右 类型
            for (int i = 0; i < GS.BlockList.Count; i++)
            {
                if (GS.BlockList[i].Type == BlockType.Pillar) continue;
                hasLeft = isHasLeft(i);
                hasRight = isHasRight(i);
                if (!hasLeft && hasRight) GS.BlockList[i].Type = BlockType.Left;
                if (hasLeft && !hasRight) GS.BlockList[i].Type = BlockType.Right;
                if (!hasLeft && !hasRight) GS.BlockList[i].Type = BlockType.Single;
                if (hasLeft && hasRight) GS.BlockList[i].Type = BlockType.Normal;
            }
            // 设置柱子
            int index = 0,temp = 0;
            // 找到最后一个柱子
            for (int i = GS.BlockList.Count - 1; i >= 0; i--)
            {
                if (GS.BlockList[i].Type == BlockType.Pillar)
                {
                    index = i;
                    break;
                }
            }
            // 按一定间隔设置柱子
            for (int i = index + 1; i < GS.BlockList.Count; i++)
            {
                if(GS.BlockList[i].Type == BlockType.Normal)
                {
                    temp++;
                    if(temp%6==0)
                    {
                        GS.BlockList[i].Type = BlockType.Pillar;
                    }
                }
            }
        }

        /// <summary>
        /// 左边是否有图块
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool isHasLeft(int index)
        {
            for (int i = index - 1; i >= 0; i--)
            {
                if (Math.Abs(GS.BlockList[i].X + GS.BlockList[i].Width - GS.BlockList[index].X) < 2 &&  GS.BlockList[i].Y == GS.BlockList[index].Y)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 右边是否有图块
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool isHasRight(int index)
        {
            for (int i = index + 1; i < GS.BlockList.Count; i++)
            {
                if (Math.Abs(GS.BlockList[i].X - GS.BlockList[index].X - GS.BlockList[i].Width) < 2 && GS.BlockList[i].Y == GS.BlockList[index].Y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
