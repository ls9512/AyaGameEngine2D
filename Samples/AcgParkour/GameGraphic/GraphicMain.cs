using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using AcgParkour.Models;
using AyaGameEngine2D;

using GS = AcgParkour.Game.GameStatus;
using TM = AcgParkour.GameGraphic.TextureManager;
using SM = AcgParkour.GameIO.SoundManager;
using UM = AcgParkour.GameGraphic.UIManager;
using GH = AyaGameEngine2D.GraphicHelper;

namespace AcgParkour.GameGraphic
{
    /// <summary>
    /// 类      名：GraphicMain
    /// 功      能：游戏主绘图类，游戏中所有视觉效果在此实现
    /// 日      期：2015-03-21
    /// 修      改：2015-06-26
    /// 作      者：ls9512
    /// </summary>
    public class GraphicMain
    {
        /// <summary>
        /// 背景效果坐标
        /// </summary>
        private float _backMoveEffectLoc = 0;
        
        /// <summary>
        /// 创建玩家纹理
        /// </summary>
        public void CreatePlayerTexture()
        {
            GS.GamePlayer.PlayerTexture = new uint[16][];
            TextureMatrix texture = new TextureMatrix(General.Data_Path + @"\Graphic\Player\Player_1.png", 16 ,1);
            for (int i = 0; i < texture.NumX; i++)
            {
                GS.GamePlayer.PlayerTexture[i] = texture.TextureID[i, 0];
            }
        }

        /// <summary>
        /// 游戏核心绘图
        /// </summary>
        public void Draw()
        {
            switch (GS.GamePhase)
            {
                case GamePhase.Title:
                    GraphicTitle.DrawTitle();
                    break;
                case GamePhase.MainMenu:
                    GraphicTitle.DrawMainMenu();
                    break;
                case GamePhase.Gaming:
                    // 背景
                    DrawBackground();
                    // 后景效果
                    DrawGamingEffect(true);
                    if (!GS.IsPause || GS.IsPauseCountDown)
                    {
                        // 绘制饰品物件列表
                        DrawOrnamentList();
                        DrawBlockList();
                        // 元件列表和玩家
                        DrawPlayer();
                        DrawItemList();
                        // 动画列表
                        DrawAnimationList();
                    }
                    // 前景效果
                    DrawGamingEffect(false);
                    // UI界面
                    if (!GS.IsPause)
                    {
                        GraphicGameUI.DrawGameUI();
                    }
                    if (GS.IsPause)
                    {
                        if (GS.IsPauseCountDown) GraphicGameUI.DrawGameUI();
                        // 暂停画面
                        DrawPause(); 
                    }
                    // 渐变效果
                    DrawTransition();
                    break;
                case GamePhase.GameOver:
                    GraphicGameOver.DrawGameOver();
                    break;
                case GamePhase.Help:
                    GraphicHelp.DrawHelp();
                    break;
                case GamePhase.About:
                    GraphicAbout.DrawAbout();
                    break;
            }

            // 绘制鼠标
            DrawMouseEffect();
        }

		#region 频谱测试
		float[] ft = new float[512];
		float[] fd = new float[512];

		/// <summary>
		/// 频谱绘制测试
		/// </summary>
		public void TestDrawFFT() {
			int id = AcgParkour.GameIO.SoundManager.SoundGameTitleBGM;
			//if (id <= 0) return;
			float[] f = SoundManager.Instance.GetFFTData(id);

			for (int i = 0; i < f.Length; i++)
			{
				f[i] = (float)Math.Sqrt(Math.Sqrt(f[i])) * General.Draw_Rect.Height;
				if (f[i] < fd[i])
				{
					fd[i] -= 1f * Time.DeltaTime;
				} else
				{
					fd[i] = f[i];
				}
				if (f[i] < ft[i])
				{
					ft[i] -= 10f * Time.DeltaTime;
				} else
				{
					ft[i] += 5f * Time.DeltaTime;
				}
			}
			for (int i = 0; i < ft.Length / 2; i += 2)
			{
				int width = 3;
				float x = i * (width / 2 + 4f);
				GH.FillRectangle(Color.FromArgb(150, Color.White), new RectangleF(x, General.Draw_Rect.Height - fd[i], width, 3));
				GH.FillRectangle(Color.FromArgb(130, Color.White), new RectangleF(x, General.Draw_Rect.Height - ft[i], width, ft[i]));
			}
		}
		#endregion

		#region 鼠标效果绘制
		/// <summary>
		/// 鼠标轨迹列表
		/// </summary>
		private static List<EffectMouse> _mouseLoc = new List<EffectMouse>();
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
            if (General.Game_MouseEffect)
            {

                // 检测是否添加移动轨迹
                if (_mouseLoc.Count > 0)
                {
                    // 距离大于一定数值才加入该序列
                    if (GameSupport.LengthFromPointToPoint(Input.MousePosition, _mouseLoc[_mouseLoc.Count - 1].Point) > 5)
                    {
                        _mouseLoc.Add(new EffectMouse(Input.MousePosition));
                    }
                }
                else
                {
                    _mouseLoc.Add(new EffectMouse(Input.MousePosition));
                }

                if (_mouseLoc.Count > 0)
                {
                    // 透明
                    for (int i = _mouseLoc.Count - 1; i >= 0; i--)
                    {
                        _mouseLoc[i].Pellucidity -= 5f * Time.DeltaTime;
                    }

                    // 删除
                    for (int i = _mouseLoc.Count - 1; i >= 0; i--)
                    {
                        if (_mouseLoc[i].Pellucidity == 0)
                        {
                            _mouseLoc.Remove(_mouseLoc[i]);
                        }
                    }
                }

                int h = _mouseColor.H;
                h++;
                if (h >= 360) h = 0;
                _mouseColor.H = h;
                ColorRGB color = ColorHelper.HsvToRgb(_mouseColor);

                Color c = Color.FromArgb(color.R, color.G, color.B);
                Color c2 = Color.FromArgb(220, Color.White);

                // 绘制
                for (int i = 0; i < _mouseLoc.Count; i++)
                {
                    if (i == _mouseLoc.Count - 1)
                    {
                        GH.SetLineWitth(8f);
                        GH.DrawLine(Color.FromArgb((int)_mouseLoc[i].Pellucidity, c), _mouseLoc[i].Point, Input.MousePosition);
                        GH.SetLineWitth(4f);
                        GH.DrawLine(Color.FromArgb((int)(_mouseLoc[i].Pellucidity / 2), c2), _mouseLoc[i].Point, Input.MousePosition);
                    }
                    else
                    {
                        GH.SetLineWitth(8f);
                        GH.DrawLine(Color.FromArgb((int)_mouseLoc[i].Pellucidity, c), _mouseLoc[i].Point, _mouseLoc[i + 1].Point);
                        GH.SetLineWitth(4f);
                        GH.DrawLine(Color.FromArgb((int)(_mouseLoc[i].Pellucidity / 2), c2), _mouseLoc[i].Point, _mouseLoc[i + 1].Point);
                    }
                }
            }

            // 鼠标测试
            GH.DrawImage(TM.TextureMouse.TextureID, Input.MousePosition.X - 7, Input.MousePosition.Y - 5, TM.TextureMouse.Width, TM.TextureMouse.Height);
        }
        #endregion

        /// <summary>
        /// 绘制渐变
        /// </summary>
        public void DrawTransition()
        {
            // 绘制渐变
            if (TM.AnimationTransition != null)
            {
                TM.AnimationTransition.DrawTransAnimation();
                if (TM.AnimationTransition.IsEnd && TM.AnimationTransition.Flag == "Gaming")
                {
                    TM.AnimationTransition.Dispose();
                    TM.AnimationTransition = null;
                    return;
                }
                if (TM.AnimationTransition.IsNewSence && TM.AnimationTransition.Flag == "GameOver")
                {
                    // 切换到游戏结束画面
                    GS.GamePhase = GamePhase.GameOver;
                    // 停止BGM
                    SM.StopGameBGM();
                }
                if (TM.AnimationTransition.IsNewSence && TM.AnimationTransition.Flag == "BackTitle")
                {
                    // 切换到标题画面
                    GS.GamePhase = GamePhase.MainMenu ;
                    // 停止BGM
                    SM.StopGameBGM();
                    SM.PlayTitleBGM();
                }
            }
        }

        /// <summary>
        /// 绘制游戏效果
        /// 飘落樱花效果
        /// </summary>
        /// <param name="backOrFront">前景/后景标志</param>
        public void DrawGamingEffect(bool backOrFront)
        {
            // 绘制效果动画
            if (GS.EffectItemList != null)
            {
                for (int i = 0; i < GS.EffectItemList.Count;i++ )
                {
                    if (backOrFront == GS.EffectItemList[i].Flag) GS.EffectItemList[i].DrawEffectItem();
                }
            }
        }

        /// <summary>
        /// 绘制饰品物件列表
        /// </summary>
        public void DrawOrnamentList()
        {
            for (int i = 0; i < GS.OrnamentList.Count; i++)
            {
                GH.DrawImage(GS.OrnamentList[i].Texture.TextureID, GS.OrnamentList[i].X, GS.OrnamentList[i].Y + GS.GamePlayer.OffestY, GS.OrnamentList[i].Texture.Width, GS.OrnamentList[i].Texture.Height);
                // 碰撞矩形
                if (General.Debug_ShowRect)
                {
                    GH.DrawRectangle(Color.LightGray, GS.OrnamentList[i].ObjectRect, 0, GS.GamePlayer.OffestY);
                }
            }
        }

        /// <summary>
        /// 绘制背景
        /// </summary>
        public void DrawBackground()
        {
            /*
            // 远景
            GH.DrawImage(TM.TextureBackGround.TextureID, GS.BackgroundLoc, 0, General.Draw_Rect.Width, General.Draw_Rect.Height);
            GH.DrawImage(TM.TextureBackGround.TextureID, General.Draw_Rect.Width + GS.BackgroundLoc, 0, General.Draw_Rect.Width, General.Draw_Rect.Height);
            // 移动效果
            GH.DrawImage(TM.TextureBackMoveEffect.TextureID, backMoveEffectLoc, 0, General.Draw_Rect.Width, General.Draw_Rect.Height);
            GH.DrawImage(TM.TextureBackMoveEffect.TextureID, General.Draw_Rect.Width + backMoveEffectLoc, 0, General.Draw_Rect.Width, General.Draw_Rect.Height);
             */

            // 远景
            Size size = new Size(General.Draw_Rect.Width, General.Draw_Rect.Height);
            int yy = 100;
            RectangleF rect = new RectangleF(0, yy - GS.GamePlayer.OffestY / 6, General.Draw_Rect.Width, General.Draw_Rect.Height - yy * 2 - GS.GamePlayer.OffestY / 6);
            GH.DrawImage(TM.TextureBackGround.TextureID,size,rect,  new RectangleF(GS.BackgroundLoc, 0, General.Draw_Rect.Width, General.Draw_Rect.Height));
            GH.DrawImage(TM.TextureBackGround.TextureID, size, rect, new RectangleF(General.Draw_Rect.Width + GS.BackgroundLoc, 0, General.Draw_Rect.Width, General.Draw_Rect.Height));
            // 移动效果
            GH.DrawImage(TM.TextureBackMoveEffect.TextureID, size, rect, new RectangleF(_backMoveEffectLoc, 0, General.Draw_Rect.Width, General.Draw_Rect.Height));
            GH.DrawImage(TM.TextureBackMoveEffect.TextureID, size, rect, new RectangleF(General.Draw_Rect.Width + _backMoveEffectLoc, 0, General.Draw_Rect.Width, General.Draw_Rect.Height));
            float speed = GS.MoveSpeed;
            if (GS.GamePlayer.PlayerStatus == PlayerStatus.Glide) speed += GS.GamePlayer.FlySpeed;
            _backMoveEffectLoc -= ((speed) / 1.5f) * Time.DeltaTime;
            if (_backMoveEffectLoc < -General.Draw_Rect.Width) _backMoveEffectLoc = 0;
        }     

        /// <summary>
        /// 绘制块列表
        /// </summary>
        public void DrawBlockList()
        {
            for (int i = 0; i < GS.BlockList.Count; i++)
            {
                // 不绘制在屏幕外的
                if (GameSupport.RectHitCheck(GS.BlockList[i].ObjectRect, General.Draw_Rect))
                {
                    // 判断图块类型
                    int index = 0;
                    switch (GS.BlockList[i].Type)
                    {
                        case BlockType.Left: index = 0; break;
                        case BlockType.Pillar: index = 1; break;
                        case BlockType.Normal: index = 2; break;
                        case BlockType.Right: index = 3; break;
                        case BlockType.Single: index = 4; break;
                    }
                    GH.DrawImage(TM.TextureBlock.TextureID[index, 0], GS.BlockList[i].X, GS.BlockList[i].Y + GS.GamePlayer.OffestY, GS.BlockWidth, GS.BlockWidth);
                    // 柱子底部
                    if (GS.BlockList[i].Type == BlockType.Pillar)
                    {
                        GH.DrawImage(TM.TextureBlock.TextureID[5, 0], GS.BlockList[i].X, GS.BlockList[i].Y + GS.BlockWidth + GS.GamePlayer.OffestY, GS.BlockWidth, General.Draw_Rect.Height - GS.BlockList[i].Y - GS.BlockWidth - GS.GamePlayer.OffestY);//
                    }
                    // 碰撞矩形
                    if (General.Debug_ShowHitCheckRect)
                    {
                        GH.DrawRectangle(General.Debug_ColorHitCheckRect, GS.BlockList[i].ObjectRect, 0, GS.GamePlayer.OffestY);
                    }
                }
            }
        }

        /// <summary>
        /// 绘制物件列表
        /// </summary>
        public void DrawItemList()
        {
            for (int i = 0; i < GS.ItemList.Count; i++)
            {
                GS.ItemList[i].ItemGraphic();
                // 碰撞矩形
                if (General.Debug_ShowHitCheckRect)
                {
                    GH.DrawRectangle(General.Debug_ColorHitCheckRect, GS.ItemList[i].ObjectRect, 0, GS.GamePlayer.OffestY);
                }
            }
        }
        
        /// <summary>
        /// 滑翔动画帧参数
        /// </summary>
        private int animationFlyEffectFrame = 0;
        private int animationFlyEffectFlag = 5;

        /// <summary>
        /// 绘制玩家
        /// </summary>
        public void DrawPlayer()
        {
            if (GS.GamePlayer != null)
            {
                // 滑翔中
                if (GS.GamePlayer.PlayerStatus == PlayerStatus.Glide)
                {
                    if (animationFlyEffectFrame < 1)
                    {
                        animationFlyEffectFrame = 1;
                        animationFlyEffectFlag = 5;
                    }
                }
                else
                {
                    if (animationFlyEffectFrame > 0) animationFlyEffectFlag = -5;
                }
                if (animationFlyEffectFrame > 0)
                {
                    // 飞行效果
                    GH.DrawImage(TM.AnimationPlayerFlyEffect.GetTextureFrameID(), GS.GamePlayer.X + 92, GS.GamePlayer.Y + 70 + GS.GamePlayer.OffestY, TM.AnimationPlayerFlyEffect.Width, TM.AnimationPlayerFlyEffect.Height, animationFlyEffectFrame += animationFlyEffectFlag);
                }
                // 玩家本体
                if (GS.GamePlayer.PlayerStatus == PlayerStatus.Run)
                {
                    GH.DrawImage(GS.GamePlayer.PlayerTexture[GS.GamePlayer.FrameIndex], GS.GamePlayer.X, GS.GamePlayer.Y + 2 + GS.GamePlayer.OffestY, GS.GamePlayer.Width, GS.GamePlayer.Height, -5f);
                }
                else
                {
                    GH.DrawImage(GS.GamePlayer.PlayerTexture[GS.GamePlayer.FrameIndex], GS.GamePlayer.X, GS.GamePlayer.Y + 2 + GS.GamePlayer.OffestY, GS.GamePlayer.Width, GS.GamePlayer.Height);
                }
                // 绘制表情
                if (GS.GamePlayer.Expression != null)
                {
                    GS.GamePlayer.Expression.DrawExpression();
                    // 显示完释放
                    if (GS.GamePlayer.Expression.NowFrame >= 8)
                    {
                        GS.GamePlayer.Expression = null;
                    }
                }
                // 碰撞矩形
                if (General.Debug_ShowHitCheckRect)
                {
                    GH.DrawRectangle(Color.Yellow, GS.GamePlayer.ObjectRect, 0, GS.GamePlayer.OffestY);
                }
            }
        }

        /// <summary>
        /// 绘制动画列表
        /// </summary>
        public void DrawAnimationList()
        {
            if (GS.AnimationList == null) return;
            for(int i =GS.AnimationList.Count -1;i>=0;i--)
            {
                // 移除播放结束的
                if (GS.AnimationList[i].IsEnd == true)
                {
                    GS.AnimationList.Remove(GS.AnimationList[i]);
                    break;
                }
                else
                {
                    GH.DrawImage(GS.AnimationList[i].GetTextureFrameID(), GS.AnimationList[i].X, GS.AnimationList[i].Y + GS.GamePlayer.OffestY, GS.AnimationList[i].Width, GS.AnimationList[i].Height);
                    // 碰撞矩形
                    if (General.Debug_ShowRect)
                    {
                        GH.DrawRectangle(Color.Red, GS.AnimationList[i].ObjectRect, 0, GS.GamePlayer.OffestY);
                    }
                }
            }
        }

        /// <summary>
        /// 暂停倒计时
        /// </summary>
        private int GamePause_CountDown = 180;
        private int GamePause_CountDownMax = 180;

        /// <summary>
        /// 绘制暂停界面
        /// </summary>
        public void DrawPause()
        {
            if (GS.IsPause)
            {
                if (!GS.IsPauseCountDown)
                {
                    GH.DrawImage(TM.TextureBackGroundBlur.TextureID, 0, 0, General.Draw_Rect.Width, General.Draw_Rect.Height, 150);
                    this.GamePause_CountDown = this.GamePause_CountDownMax;
                    float x = (General.Draw_Rect.Width - TM.Texture_UI_Win_Pause.Width) / 2;
                    float y = (General.Draw_Rect.Height - TM.Texture_UI_Win_Pause.Height) / 2;
                    GH.DrawImage(TM.Texture_UI_Win_Pause.TextureID, x, y, TM.Texture_UI_Win_Pause.Width, TM.Texture_UI_Win_Pause.Height);
                    UM.Btn_PauseBackTitle.UIGraphic();
                    UM.Btn_Resume.UIGraphic();
                }
                else
                {
                    // 绘制倒计时数字
                    float x = (General.Draw_Rect.Width - TM.Texture_UI_CountDown_1.Width) / 2;
                    float y = (General.Draw_Rect.Height - TM.Texture_UI_CountDown_1.Height) / 2;
                    int index = GamePause_CountDown / (GamePause_CountDownMax / 3) + 1;
                    // 计算缩放参数
                    int w = (GamePause_CountDownMax / 3) - (GamePause_CountDown - index * (GamePause_CountDownMax / 3));
                    // 计算透明度参数
                    int h = 0;
                    if (w < 40) h = 255;
                    else h = 255 - (w - 40) * 3;
                    GH.DrawImage(TM.TextureBackGroundBlur.TextureID, 0, 0, General.Draw_Rect.Width, General.Draw_Rect.Height, this.GamePause_CountDown);
                    if (index == 3)
                    {
                        GH.DrawImage(TM.Texture_UI_CountDown_3.TextureID, x - w, y - w, TM.Texture_UI_CountDown_1.Width + w * 2, TM.Texture_UI_CountDown_1.Height + w * 2, h);
                    }
                    else if (index == 2)
                    {
                        GH.DrawImage(TM.Texture_UI_CountDown_2.TextureID, x - w, y - w, TM.Texture_UI_CountDown_1.Width + w * 2, TM.Texture_UI_CountDown_1.Height + w * 2, h);
                    }
                    else if (index == 1)
                    {
                        GH.DrawImage(TM.Texture_UI_CountDown_1.TextureID, x - w, y - w, TM.Texture_UI_CountDown_1.Width + w * 2, TM.Texture_UI_CountDown_1.Height + w * 2, h);
                    }
                    if (GamePause_CountDown > 0) GamePause_CountDown--;
                    if (GamePause_CountDown == 0)
                    {
                        GS.IsPause = false;
                        GS.IsPauseCountDown = false;
                    }
                }
            }
        }
    }
}
