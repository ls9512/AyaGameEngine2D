using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using AcgParkour.GameUI;

using AyaGameEngine2D;

using GS = AcgParkour.Game.GameStatus;
using TM = AcgParkour.GameGraphic.TextureManager;
using GH = AyaGameEngine2D.GraphicHelper;

namespace AcgParkour.GameGraphic
{
    /// <summary>
    /// 类      名：GraphicGameUI
    /// 功      能：游戏UI绘图静态类
    /// 日      期：2015-03-26
    /// 修      改：2015-03-26
    /// 作      者：ls9512
    /// </summary>
    public static class GraphicGameUI
    {
        /// <summary>
        /// 绘制游戏UI
        /// </summary>
        public static void DrawGameUI()
        {
            DrawScore();
            DrawFlyProcessbar();
            DrawLife();
            DrawJumpStatus();
        }

        private static float _lifeIconZoom = 3f;
        private static float _lifeIconFlag = 0.05f;

        private static float _flyBarPellucidity = 0f;
        private static float _flyBarPpelluciditySpeed = 10f;

        /// <summary>
        /// 绘制生命槽
        /// </summary>
        public static void DrawLife()
        {
            float x = 10, y = 10, blank = 6, width = TM.TextureLifeIcon.Width  / 1.5f;
            float zoom = _lifeIconZoom;
            float flag = _lifeIconFlag;
            for (int i = 0; i < GS.GamePlayer.Life; i++)
            {
                GH.DrawImage(TM.TextureLifeIcon.TextureID, x + (width + blank) * i + zoom, y + zoom, width - zoom * 2, width - zoom * 2);
                // blank -= 5;
                zoom *= 1.2f;
            }
            _lifeIconZoom += _lifeIconFlag * Time.DeltaTime;
            if (_lifeIconZoom > 3 || _lifeIconZoom < 0) _lifeIconFlag *= -1;
        }

        /// <summary>
        /// 绘制跳跃状态
        /// </summary>
        public static void DrawJumpStatus()
        {
            float x = 10, y = 55, blank = 2, width = TM.TextureJumpIcon.Width / 2.5f;
            for (int i = 0; i < GS.GamePlayer.Jump; i++)
            {
                GH.DrawImage(TM.TextureJumpIcon.TextureID, x + (width + blank) * i,y, width, width, 220);
            }
        }

        /// <summary>
        /// 绘制分数
        /// </summary>
        public static void DrawScore()
        {
            // 绘制距离分数 计算绘制参数
            int score = GS.ScoreDistance / General.Game_DistancePixel;
            int width = TM.TextureScoreDistance.Width, height = TM.TextureScoreDistance.Height;
            int blank = 2;
            int blank_Top = 10, blank_Right = 10;
            // 绘制距离积分
            int x = General.Draw_Rect.Width - (width + blank) * score.ToString().Length - blank_Right;
            GH.DrawNumberByTextureMatrix(TM.TextureScoreDistance, x, blank_Top, width, height, score, blank);
            // 绘制积分 计算绘制参数
            score = GS.Score;
            width = (int)(TM.TextureScore.Width * 1.3);
            height = (int)(TM.TextureScore.Height * 1.3);
            blank = 2;
            blank_Top = 60;
            blank_Right = 10;
            x = General.Draw_Rect.Width - (width + blank) * score.ToString().Length - blank_Right;
            // 绘制积分
            GH.DrawNumberByTextureMatrix(TM.TextureScore, x, blank_Top, width, height, score, blank);
            // 绘制收获率
            if (GS.Accuracy > 0.001f)
            {
                score = (int)(GS.Accuracy * 100);
                width = (int)(TM.TextureComboNumber.Width * 1.3);
                height = (int)(TM.TextureComboNumber.Height * 1.3);
                blank = 2;
                blank_Top = 100;
                blank_Right = 10;
                x = General.Draw_Rect.Width - (width + blank) * score.ToString().Length - blank_Right - 2;
                // 绘制收获率
                GH.DrawImage(TM.TextureAccuracy.TextureID, General.Draw_Rect.Width - 180, blank_Top - 6, TM.TextureAccuracy.Width, TM.TextureAccuracy.Height);
                GH.DrawNumberByTextureMatrix(TM.TextureComboNumber, x - 20, blank_Top, width, height, score, blank);
            }
            // 绘制连击
            if (GS.Combo > 5)
            {
                score = GS.Combo;
                width = (int)(TM.TextureComboNumber.Width * 1.3);
                height = (int)(TM.TextureComboNumber.Height * 1.3);
                blank = 2;
                blank_Top = 135;
                blank_Right = 10;
                x = General.Draw_Rect.Width - (width + blank) * score.ToString().Length - blank_Right;
                // 绘制连接
                GH.DrawImage(TM.TextureCombo.TextureID, x - 100, blank_Top - 4, TM.TextureCombo.Width, TM.TextureCombo.Height);
                GH.DrawNumberByTextureMatrix(TM.TextureComboNumber, x, blank_Top, width, height, score, blank);
            }
            
        }

        /// <summary>
        /// 绘制飞行进度条
        /// </summary>
        public static void DrawFlyProcessbar()
        {
            if (GS.GamePlayer.PlayerStatus == PlayerStatus.Glide || GS.GamePlayer.FlyFrame > 0)
            {
                if (_flyBarPellucidity < 255) _flyBarPellucidity += _flyBarPpelluciditySpeed * Time.DeltaTime;
            }
            else
            {
                if (_flyBarPellucidity > 0) _flyBarPellucidity -= _flyBarPpelluciditySpeed * Time.DeltaTime;
            }
            if (_flyBarPellucidity > 0)
            {
                int x = General.Draw_Rect.Width / 2 - TM.Texture_UI_ProcessBar.Width / 2;
                int y = 600;
                GH.DrawImage(TM.Texture_UI_ProcessBar.TextureID, x, y, TM.Texture_UI_ProcessBar.Width, TM.Texture_UI_ProcessBar.Height, (int)_flyBarPellucidity);
                int width = (int)(TM.Texture_UI_Process.Width * ((GS.GamePlayer.MaxFlyFrame - GS.GamePlayer.FlyFrame) / GS.GamePlayer.MaxFlyFrame));
                GH.DrawImage(TM.Texture_UI_Process.TextureID, new Size(449, 32), new Rectangle(0, 0, width, 32), new Rectangle(x + 62, y + 9, width, TM.Texture_UI_Process.Height), (int)_flyBarPellucidity);
            }
        }
    }
}
