using System;
using System.Collections.Generic;
using System.Text;
using AcgParkour.Models;

using AyaGameEngine2D;

using GS = AcgParkour.Game.GameStatus;
using TM = AcgParkour.GameGraphic.TextureManager;

namespace AcgParkour.GameLogic
{
    /// <summary>
    /// 类      名：LogicMap
    /// 功      能：地图逻辑静态类
    /// 日      期：2015-03-23
    /// 修      改：2015-06-23
    /// 作      者：ls9512
    /// </summary>
    public static class LogicMap
    {
        // 告示牌是否出现
        public static bool Billboard_Start = false;
        public static bool Billboard_RoadBlock = false;
        public static bool Billboard_Rocket = false;

        /// <summary>
        /// 重置告示牌
        /// </summary>
        public static void ResetBillboard()
        {
            Billboard_Start = false;
            Billboard_RoadBlock = false;
            Billboard_Rocket = false;
        }

        /// <summary>
        /// 背景移动
        /// </summary>
        public static void MoveBackground()
        {
            // 地图卷动速度
            GS.BackgroundLoc -= GS.BackMoveSpeed * Time.DeltaTime;
            // 循环贴图坐标计算
            if (GS.BackgroundLoc < -General.Draw_Rect.Width) GS.BackgroundLoc = 0;
        }

        /// <summary>
        /// 移动提速
        /// </summary>
        public static void SpeedUp()
        {
            // 提速
            if (GS.MoveSpeed < General.Game_MaxMoveSpeed)
            {
                GS.MoveSpeed += General.Game_AddMoveSpeed * Time.DeltaTime;
                GS.BackMoveSpeed = GS.MoveSpeed / 3;
            }

            // 移动距离积分累加
            // 统计的距离为移动的总像素数，按一定比例换算成其他单位
            GS.ScoreDistance += (int)(GS.MoveSpeed + GS.PlayerFlySpeed);
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        public static void GameOverCheck()
        {
            if (isGameOver())
            {
                if (TM.AnimationTransition == null)
                {
                    // 开启渐变
                    TM.AnimationTransition = new AnimationTrans(new Texture(General.Data_Path + @"\Graphic\Transitions\Transitions_" +  RandomHelper.RandInt(1, 14) + ".png"), "GameOver");
                }
            }
        }

        /// <summary>
        /// 检测游戏是否结束
        /// </summary>
        /// <returns>结束标志</returns>
        private static bool isGameOver()
        {
            // 生命为0
            if (GS.GamePlayer.Life == 0) return true;
            if (GS.GamePlayer.X + GS.GamePlayer.Width < General.Draw_Rect.Left - 90 || GS.GamePlayer.Y > General.Draw_Rect.Height + 180) return true;
            return false;
        }

        /// <summary>
        /// 上次创建时间
        /// </summary>
        private static int lastEffectCreateTime = 0;

        /// <summary>
        /// 地图画面特效逻辑
        /// </summary>
        public static void MapAnimaEffect()
        {
            // 不存在则新建
            if (GS.EffectItemList == null)
            {
                GS.EffectItemList = new List<EffectItem>();
            }
            // 判断是否生成新的效果物件
            int time = Environment.TickCount;
            if (GS.EffectItemList.Count < 300 && time - lastEffectCreateTime > 15)
            {
                lastEffectCreateTime = time;
                EffectItem item = new EffectItem(TM.AnimationSakura.GetTextureIDbyIndex(RandomHelper.RandInt(0, 14)), 42, 42);
                // 随机从屏幕顶部或者右边产生
                if (RandomHelper.RandFloat(0, 100) > 40)
                {
                    item.X = RandomHelper.RandFloat(500, General.Draw_Rect.Width + 500);
                    item.Y = -600; ;
                }
                else
                {
                    item.X = General.Draw_Rect.Width + 100;
                    item.Y = RandomHelper.RandFloat(-200, General.Draw_Rect.Height / 3 * 2);
                }
                // 随机产生前后景
                if (RandomHelper.RandFloat(0, 100) > 50)
                {
                    item.Flag = true;
                }
                else
                {
                    item.Flag = false;
                }
                item.MoveX = RandomHelper.RandFloat(-8, -1);
                item.MoveY = RandomHelper.RandFloat(1, 3);
                do
                {
                    item.AngleSpeed = RandomHelper.RandFloat(-2, 3);
                } while (item.AngleSpeed == 0);
                item.Pellucidity = 255;
                GS.EffectItemList.Add(item);
            }
            // 删除旧的效果物件
            for (int i = GS.EffectItemList.Count - 1; i >= 0; i--)
            {
                // 不在屏幕内或者透明的
                if ((GS.EffectItemList[i].X < -50 || GS.EffectItemList[i].Y > General.Draw_Rect.Height + 200) || GS.EffectItemList[i].Pellucidity < 1)
                {
                    GS.EffectItemList.Remove(GS.EffectItemList[i]);
                }
            }
        }

        /// <summary>
        /// 处理饰品物件列表
        /// </summary>
        public static void MoveOrnamentList()
        {
            // 添加新饰品
            // 游戏开始提示牌
            if (GS.ScoreDistance > General.Game_Distance_Start && !Billboard_Start)
            {
                Billboard_Start = true;
                Ornament ornament = new Ornament((int)GS.BlockList[GS.BlockList.Count - 6].X, (int)GS.BlockList[GS.BlockList.Count - 6].Y - TM.TextureOrnament_Billboard_Start.Height + 30, TM.TextureOrnament_Billboard_Start);
                GS.OrnamentList.Add(ornament);
            }
            // 添加路障提示
            if (GS.ScoreDistance > General.Game_Distance_Tip_RoadBlock && !Billboard_RoadBlock)
            {
                LogicBlock.AddBlockList(15, false, false);
                Billboard_RoadBlock = true;
                Ornament ornament = new Ornament((int)GS.BlockList[GS.BlockList.Count - 6].X, (int)GS.BlockList[GS.BlockList.Count - 6].Y - TM.TextureOrnament_Billboard_RoadBlock.Height + 30, TM.TextureOrnament_Billboard_RoadBlock);
                GS.OrnamentList.Add(ornament);
            }
            // 添加火箭提示
            if (GS.ScoreDistance > General.Game_Distance_Tip_Rocket && !Billboard_Rocket)
            {
                LogicBlock.AddBlockList(15, false, false);
                Billboard_Rocket = true;
                Ornament ornament = new Ornament((int)GS.BlockList[GS.BlockList.Count - 6].X, (int)GS.BlockList[GS.BlockList.Count - 6].Y - TM.TextureOrnament_Billboard_Rocket.Height + 30, TM.TextureOrnament_Billboard_Rocket);
                GS.OrnamentList.Add(ornament);
            }
            // 饰品移动
            for (int i = GS.OrnamentList.Count - 1; i >= 0; i--)
            {
                // 跟随地图移动
                GS.OrnamentList[i].X -= (GS.MoveSpeed + GS.PlayerFlySpeed) * Time.DeltaTime;
            }
            // 删除旧的饰品
            for (int i = GS.OrnamentList.Count - 1; i >= 0; i--)
            {
                // 在屏幕左边的未吸引物件，则废弃
                if (GS.OrnamentList[i].X + GS.OrnamentList[i].Texture.Width * 2 < General.Draw_Rect.Left)
                {
                    GS.OrnamentList.Remove(GS.OrnamentList[i]);
                }
            }
        }
    }
}
