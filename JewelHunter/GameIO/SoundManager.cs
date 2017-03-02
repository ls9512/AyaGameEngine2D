using System;
using System.Collections.Generic;

using AyaGameEngine2D;
using SM = AyaGameEngine2D.SoundManager;

namespace JewelHunter.GameIO
{
    /// <summary>
    /// 类      名：SoundManager
    /// 功      能：游戏声音管理类
    /// 日      期：2015-11-22
    /// 修      改：2016-04-14
    /// 作      者：ls9512
    /// </summary>
    public static class SoundManager
    {
        // 声音列表
        public static List<int> SoundList;

        public static int SoundGameBGM;
        public static int SoundTitleBGM;

        public static int SoundMouseClick;
        public static int SoundMouseClickNo;
        public static int SoundClear;
        public static int SouneChange;
        public static int SoundButtonClick;

        /// <summary>
        /// 初始化音频资源
        /// </summary>
        public static void SoundInit()
        {
            try
            {
                SoundList = new List<int>();

                // 加载BGM
                SoundTitleBGM = SM.Instance.CreateSoundStream(General.DataPath + @"\Sound\BGM\Title.ogg");
                SM.Instance.SetLoop(SoundTitleBGM, true);
                SoundGameBGM = SM.Instance.CreateSoundStream(General.DataPath + @"\Sound\BGM\Game.ogg");
                SM.Instance.SetLoop(SoundGameBGM, true);

                // 加载鼠标单击声音
                SoundMouseClick = SM.Instance.CreateSoundStream(General.DataPath + @"\Sound\SE\Cursor.ogg");
                SM.Instance.SetPlayVolume(SoundMouseClick , 0.9f);
                // 加载按钮单击声音
                SoundButtonClick = SM.Instance.CreateSoundStream(General.DataPath + @"\Sound\SE\Click.ogg");
                SM.Instance.SetPlayVolume(SoundButtonClick, 0.9f);
                // 加载鼠标单击声音
                SoundMouseClickNo = SM.Instance.CreateSoundStream(General.DataPath + @"\Sound\SE\CursorNo.ogg");
                SM.Instance.SetPlayVolume(SoundMouseClickNo, 0.9f);
                // 加载消除声音
                SoundClear = SM.Instance.CreateSoundStream(General.DataPath + @"\Sound\SE\Clear.ogg");
                SM.Instance.SetPlayVolume(SoundClear, 0.65f);
                // 加载改变声音
                SouneChange = SM.Instance.CreateSoundStream(General.DataPath + @"\Sound\SE\Change.ogg");
                SM.Instance.SetPlayVolume(SouneChange, 7f);


                // 添加到列表中
                SoundList.Add(SoundTitleBGM);
                SoundList.Add(SoundGameBGM);
                SoundList.Add(SoundMouseClick);
                SoundList.Add(SoundMouseClickNo);
                SoundList.Add(SoundClear);
                SoundList.Add(SouneChange);
                SoundList.Add(SoundButtonClick);

            }
            catch (Exception e)
            {
                Debug.ThrowException("声音加载错误", e);
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
        /// 播放游戏背景音乐
        /// </summary>
        public static void PlayTitleBgm()
        {
            if (General.GameBgm)
            {
                SM.Instance.SetPlayVolume(SoundTitleBGM, 0.0f);
                SM.Instance.Play(SoundTitleBGM);
                SM.Instance.SetPlayVolume(SoundTitleBGM, 0.5f, 5000);
            }
        }

        /// <summary>
        /// 停止播放背景音乐
        /// </summary>
        public static void StopTitleBgm()
        {
            if (General.GameBgm)
            {
                SM.Instance.SetPlayVolume(SoundTitleBGM, 0.0f, 3000);
            }
        }

        /// <summary>
        /// 播放游戏背景音乐
        /// </summary>
        public static void PlayGameBgm()
        {
            if (General.GameBgm)
            {
                SM.Instance.SetPlayVolume(SoundGameBGM, 0.0f);
                SM.Instance.Play(SoundGameBGM);
                SM.Instance.SetPlayVolume(SoundGameBGM, 0.3f, 5000);
            }
        }

        /// <summary>
        /// 停止播放背景音乐
        /// </summary>
        public static void StopGameBgm()
        {
            if (General.GameBgm)
            {
                SM.Instance.SetPlayVolume(SoundGameBGM, 0.0f, 3000);
            }
        }

        /// <summary>
        /// 播放鼠标音效
        /// </summary>
        public static void PlayMouseClick()
        {
            if (General.GameSe)
            {
                SM.Instance.Play(SoundMouseClick);
            }
        }

        /// <summary>
        /// 播放按钮音效
        /// </summary>
        public static void PlayButtonClick()
        {
            if (General.GameSe)
            {
                SM.Instance.Play(SoundButtonClick);
            }
        }

        /// <summary>
        /// 播放鼠标音效
        /// </summary>
        public static void PlayMouseClickNo()
        {
            if (General.GameSe)
            {
                SM.Instance.Play(SoundMouseClickNo);
            }
        }

        /// <summary>
        /// 播放消除音效
        /// </summary>
        public static void PlayClear()
        {
            if (General.GameSe)
            {
                SM.Instance.Play(SoundClear);
            }
        }

        /// <summary>
        /// 播放改变音效
        /// </summary>
        public static void PlayChange()
        {
            if (General.GameSe)
            {
                SM.Instance.Play(SouneChange);
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
