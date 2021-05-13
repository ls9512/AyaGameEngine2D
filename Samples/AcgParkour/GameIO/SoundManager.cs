using System;
using System.Collections.Generic;
using System.Text;

using AyaGameEngine2D;
using SM = AyaGameEngine2D.SoundManager;

namespace AcgParkour.GameIO
{
    /// <summary>
    /// 类      名：SoundManager
    /// 功      能：音频管理
    /// 日      期：2015-03-27
    /// 修      改：2015-03-30
    /// 作      者：ls9512
    /// </summary>
    public static class SoundManager
    {
        public static int SoundAddScore = 0;
        public static int SoundGameBGM = 0;
        public static int SoundGameTitleBGM = 0;
        public static int SoundJump = 0;
        public static int SoundRoadBlockAttack = 0;
        public static int SoundRocketAttack = 0;
        public static int SoundButton = 0;

        public static List<int> SoundList;

        /// <summary>
        /// 初始化音频资源
        /// </summary>
        public static void SoundInit()
        {
            try
            {
                SoundList = new List<int>();
                // 加载加分音效
                SoundAddScore = SM.Instance.CreateSoundStream(General.Data_Path + @"\Sound\SE\AddScore.ogg");
                // 加载BGM        循环/初始0音量，1000ms淡入
                SoundGameBGM = SM.Instance.CreateSoundStream(General.Data_Path + @"\Sound\BGM\BGM.ogg");
                SM.Instance.SetLoop(SoundGameBGM, true);
                // 加载标题画面BGM
                SoundGameTitleBGM = SM.Instance.CreateSoundStream(General.Data_Path + @"\Sound\BGM\Title.mp3");
                SM.Instance.SetLoop(SoundGameTitleBGM, true);
                // 加载跳跃音效
                SoundJump = SM.Instance.CreateSoundStream(General.Data_Path + @"\Sound\SE\Jump.ogg");
                SM.Instance.SetPlayVolume(SoundJump, 0.3f);
                // 加载人物撞击路障音效
                SoundRoadBlockAttack = SM.Instance.CreateSoundStream(General.Data_Path + @"\Sound\SE\RoadBlockAttack.ogg");
                SM.Instance.SetPlayVolume(SoundRoadBlockAttack, 0.7f);
                // 加载火箭爆炸音效
                SoundRocketAttack = SM.Instance.CreateSoundStream(General.Data_Path + @"\Sound\SE\Boom.ogg");
                SM.Instance.SetPlayVolume(SoundRocketAttack, 0.6f);
                // 加载按钮音效
                SoundButton = SM.Instance.CreateSoundStream(General.Data_Path + @"\Sound\SE\Button.mp3");

                // 添加到列表中
                SoundList.Add(SoundAddScore);
                SoundList.Add(SoundGameBGM);
                SoundList.Add(SoundGameTitleBGM);
                SoundList.Add(SoundJump);
                SoundList.Add(SoundRoadBlockAttack);
                SoundList.Add(SoundRocketAttack);
                SoundList.Add(SoundButton);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// 设置系统音量
        /// </summary>
        /// <param name="volume"></param>
        public static void SetSystemVolume(float volume)
        {
            SM.Instance.SetSystemVolume(volume);
        }

        /// <summary>
        /// 播放加分音效
        /// </summary>
        public static void PlayAddScore()
        {
            if (General.Game_SE)
            {
                SM.Instance.Play(SoundAddScore);
            }
        }

        /// <summary>
        /// 播放跳跃音效
        /// </summary>
        public static void PlayJump()
        {
            if (General.Game_SE)
            {
                SM.Instance.Play(SoundJump);
            }
        }

        /// <summary>
        /// 播放撞击路障音效
        /// </summary>
        public static void PlayRoadBlockAttack()
        {
            if (General.Game_SE)
            {
                SM.Instance.Play(SoundRoadBlockAttack);
            }
        }

        /// <summary>
        /// 播放火箭爆炸音效
        /// </summary>
        public static void PlayRocketAttack()
        {
            if (General.Game_SE)
            {
                SM.Instance.Play(SoundRocketAttack);
            }
        }

        /// <summary>
        /// 播放按钮音效
        /// </summary>
        public static void PlayButton()
        {
            if (General.Game_SE)
            {
                SM.Instance.Play(SoundButton);
            }
        }

        /// <summary>
        /// 播放游戏背景音乐
        /// </summary>
        public static void PlayTitleBGM()
        {
            if (General.Game_BGM)
            {
                SM.Instance.SetPlayVolume(SoundGameTitleBGM, 0.0f);
                SM.Instance.Play(SoundGameTitleBGM);
                SM.Instance.SetPlayVolume(SoundGameTitleBGM, 0.8f, 3000);
            }
        }

        /// <summary>
        /// 停止播放背景音乐
        /// </summary>
        public static void StopTitleBGM()
        {
            if (General.Game_BGM)
            {
                SM.Instance.SetPlayVolume(SoundGameTitleBGM, 0.0f, 3000);
                //SM.Instance.Stop(SoundGameBGM);
            }
        }

        /// <summary>
        /// 播放游戏背景音乐
        /// </summary>
        public static void PlayGameBGM()
        {
            if (General.Game_BGM)
            {
                SM.Instance.SetPlayVolume(SoundGameBGM, 0.0f);
                SM.Instance.Play(SoundGameBGM);
                SM.Instance.SetPlayVolume(SoundGameBGM, 0.9f, 3000);
            }
        }

        /// <summary>
        /// 停止播放背景音乐
        /// </summary>
        public static void StopGameBGM()
        {
            if (General.Game_BGM)
            {
                SM.Instance.SetPlayVolume(SoundGameBGM, 0.0f, 1000);
                //SM.Instance.Stop(SoundGameBGM);
            }
        }

        /// <summary>
        /// 销毁 释放资源
        /// </summary>
        public static void Dispose()
        {
            try
            {
                // 释放音频资源
                foreach (int n in SoundList)
                {
                    SM.Instance.Release(n);
                }
                // 销毁音频管理
                SM.Instance.Dispose();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
                Environment.Exit(0);
            }
        }
    }
}
