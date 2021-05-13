using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AyaGameEngine2D.Core
{
    /// <summary>
    /// 类      名：EngineLogo
    /// 功      能：引擎LOGO，会在游戏启动前显示引擎和游戏LOGO
    /// 日      期：2016-01-30
    /// 修      改：2016-01-30
    /// 作      者：ls9512
    /// </summary>
    internal static class EngineLogo
    {
        #region 私有成员
        /// <summary>
        /// LOGO纹理列表
        /// </summary>
        private static List<Texture> LogoTexture = new List<Texture>();

        /// <summary>
        /// 当前显示LOGO下标
        /// </summary>
        private static int logoIndex = 0;

        /// <summary>
        /// LOGO显示时间
        /// </summary>
        private static float time = 0;

        /// <summary>
        /// 透明度
        /// </summary>
        private static float opacity = 0;

        /// <summary>
        /// 透明度变化速度
        /// </summary>
        private static float fadeTime = 0.5f;

        /// <summary>
        /// 停留时间
        /// </summary>
        private static float stayTime = 1f;
        #endregion

        #region 公有方法
        /// <summary>
        /// 添加LOGO
        /// </summary>
        /// <param name="bmp">LOGO图片</param>
        public static void AddLogo(Bitmap bmp)
        {
            Texture texture = new Texture(bmp);
            LogoTexture.Add(texture);
        }

        /// <summary>
        /// LOGO输入输出
        /// </summary>
        public static void IO()
        {
            // 加速
            if (Input.IsKeyHeld(Keys.Space))
            {
                time += Time.DeltaTimeFrame * 2;
            }
        }

        /// <summary>
        /// LOGO逻辑
        /// </summary>
        public static void Logic()
        {
            // 无LOGO，结束
            if (LogoTexture.Count == 0)
            {
                End();
                return;
            }
            // 时间累加
            time += Time.DeltaTimeFrame;
            if (time < fadeTime)
            {
                // 淡入
                opacity = time / fadeTime * 255f;
            }
            else if (time < fadeTime + stayTime)
            {
                // 持续
                opacity = 255;
            }
            else if (time < fadeTime * 2 + stayTime)
            {
                // 淡出
                opacity = (fadeTime * 2 - time + stayTime) / fadeTime * 255;
            }
            else
            {
                // 下一个
                logoIndex++;
                // 结束
                if (logoIndex >= LogoTexture.Count)
                {
                    End();
                }
                time = 0;
            }
        }

        /// <summary>
        /// LOGO渲染
        /// </summary>
        public static void Render()
        {
            // 绘图
            if( logoIndex < LogoTexture.Count)
                GraphicHelper.DrawImage(LogoTexture[logoIndex].TextureID, General.Draw_Rect, (int)opacity);
        }

        /// <summary>
        /// 结束
        /// </summary>
        public static void End()
        {
            General.Engine_ShowLogo = false;
            for (int i = LogoTexture.Count - 1; i >= 0; i--)
            {
                LogoTexture[i].Dispose();
                LogoTexture.Remove(LogoTexture[i]);
            }
            LogoTexture = null;
        }
        #endregion
    }
}
