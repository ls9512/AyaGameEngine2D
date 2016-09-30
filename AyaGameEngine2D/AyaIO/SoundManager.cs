using System;
using System.Windows.Forms;

using Un4seen.Bass;

namespace AyaGameEngine2D
{
    #region 音频播放状态枚举
    /// <summary>
    /// 类      名：SoundPlayStatus
    /// 功      能：音频播放状态枚举
    /// 日      期：2015-11-19
    /// 修      改：2015-12-28
    /// 作      者：ls9512
    /// </summary>
    public enum SoundPlayStatus
    {
        /// <summary>
        /// 停止
        /// </summary>
        Stopped = 0,
        /// <summary>
        /// 播放
        /// </summary>
        Playing = 1,
        /// <summary>
        /// 停滞
        /// </summary>
        Stalled = 2,
        /// <summary>
        /// 暂停
        /// </summary>
        Paused = 3,
        /// <summary>
        /// 未知
        /// </summary>
        Unkonwn = 4,
    } 
    #endregion

    /// <summary>
    /// 类      名：SoundManager
    /// 功      能：音频事件管理类，提供基于Bss.Net的常见格式音频播放
    ///             基于Bass.dll (版本24) Bass.Net (版本2.4.10.00
    /// 日      期：2015-03-27
    /// 修      改：2015-03-30
    /// 作      者：ls9512
    /// </summary>
    public class SoundManager : IDisposable
    {
        #region 静态实例
        /// <summary>
        /// 音频事件管理类静态实例
        /// </summary>
        public static SoundManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SoundManager(General.Pro_WinHandle);
                return _instance;
            }
        }
        private static SoundManager _instance; 
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        protected SoundManager(IntPtr hdc)
        {
            if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_CPSPEAKERS, hdc))
            {
                MessageBox.Show("Bass.Net初始化错误！" + Bass.BASS_ErrorGetCode().ToString());
            }
        }
        #endregion

        #region 创建
        /// <summary>
        /// 创建音频流
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns>流ID</returns>
        public int CreateSoundStream(string fileName)
        {
            int streamID;
            streamID = Bass.BASS_StreamCreateFile(fileName, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT);
            return streamID;
        }
        #endregion

        #region 播放 / 暂停 / 停止
        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        public void Play(int soundStreamID)
        {
            Bass.BASS_ChannelPlay(soundStreamID, true);
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        public void Pause(int soundStreamID)
        {
            Bass.BASS_ChannelPause(soundStreamID);
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        public void Stop(int soundStreamID)
        {
            Bass.BASS_ChannelStop(soundStreamID);
        }
        #endregion

        #region 状态
        /// <summary>
        /// 获取音频播放状态
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        public SoundPlayStatus Active(int soundStreamID)
        {
            BASSActive bassActive = Bass.BASS_ChannelIsActive(soundStreamID);
            SoundPlayStatus status = SoundPlayStatus.Unkonwn;
            switch (bassActive)
            {
                case BASSActive.BASS_ACTIVE_PAUSED: status = SoundPlayStatus.Paused; break;
                case BASSActive.BASS_ACTIVE_PLAYING: status = SoundPlayStatus.Playing; break;
                case BASSActive.BASS_ACTIVE_STALLED: status = SoundPlayStatus.Stalled; break;
                case BASSActive.BASS_ACTIVE_STOPPED: status = SoundPlayStatus.Stopped; break;
            }
            return status;
        }
        #endregion

        #region 音量
        /// <summary>
        /// 设置一定时间达到目标播放音量（淡入淡出）
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        /// <param name="aimVolume">目标音量(0-1)</param>
        /// <param name="time">达到该音量时间(ms)</param>
        public void SetPlayVolume(int soundStreamID, float aimVolume, int time)
        {
            if (aimVolume > 1) aimVolume = 1;
            if (aimVolume < 0) aimVolume = 0;
            if (time < 0) time = 0;
            Bass.BASS_ChannelSlideAttribute(soundStreamID, BASSAttribute.BASS_ATTRIB_VOL, aimVolume, time);
        }

        /// <summary>
        /// 设置播放音量
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        /// <param name="volume">播放音量(0-1)</param>
        public void SetPlayVolume(int soundStreamID, float volume)
        {
            if (volume > 1) volume = 1;
            if (volume < 0) volume = 0;
            Bass.BASS_ChannelSetAttribute(soundStreamID, BASSAttribute.BASS_ATTRIB_VOL, volume);
        }

        /// <summary>
        /// 获取播放音量
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        /// <returns>音量</returns>
        public float GetPlayVolume(int soundStreamID)
        {
            float volume = 0;
            Bass.BASS_ChannelGetAttribute(soundStreamID, BASSAttribute.BASS_ATTRIB_VOL, ref volume);
            return volume;
        }

        /// <summary>
        /// 设置系统音量
        /// </summary>
        /// <param name="volume">音量(0-1)</param>
        public void SetSystemVolume(float volume)
        {
            if (volume > 1) volume = 1;
            if (volume < 0) volume = 0;
            Bass.BASS_SetVolume(volume);
        }

        /// <summary>
        /// 获取系统音量
        /// </summary>
        /// <returns></returns>
        public float GetSystemVolume()
        {
            return Bass.BASS_GetVolume();
        }
        #endregion

        #region 时间 / 位置
        /// <summary>
        /// 设置循环播放
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        /// <param name="isLoop">是否循环</param>
        public void SetLoop(int soundStreamID, bool isLoop)
        {
            if (isLoop)
            {
                Bass.BASS_ChannelFlags(soundStreamID, BASSFlag.BASS_SAMPLE_LOOP, BASSFlag.BASS_SAMPLE_LOOP);
            }
            else
            {
                Bass.BASS_ChannelFlags(soundStreamID, 0, BASSFlag.BASS_SAMPLE_LOOP);
            }
        }

        /// <summary>
        /// 获取当前播放时间
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        /// <returns>播放时间(秒)</returns>
        public double GetPosition(int soundStreamID)
        {
            double second = Bass.BASS_ChannelBytes2Seconds(soundStreamID, Bass.BASS_ChannelGetPosition(soundStreamID));
            return second;
        }

        /// <summary>
        /// 设置播放位置
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        /// <param name="time">播放位置(秒)</param>
        public void SetPosition(int soundStreamID, double time)
        {
            if (time < 0) time = 0;
            Bass.BASS_ChannelSetPosition(soundStreamID, time);
        }

        /// <summary>
        /// 获取音频持续时间
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        /// <returns>长度(秒)</returns>
        public double GetLength(int soundStreamID)
        {
            double length = Bass.BASS_ChannelBytes2Seconds(soundStreamID, Bass.BASS_ChannelGetLength(soundStreamID));
            return length;
        }
        #endregion

        #region FFT采样
        /// <summary>
        /// 获取FFT采样数据，返回512个浮点采样数据
        /// </summary>
        /// <returns>频谱采样数组</returns>
        public float[] GetFFTData(int soundStreamID)
        {
            float[] fft = new float[512];
            Bass.BASS_ChannelGetData(soundStreamID, fft, (int)BASSData.BASS_DATA_FFT512);
            return fft;
        }
        #endregion

        #region 释放
        /// <summary>
        /// 释放音频
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        public void Release(int soundStreamID)
        {
            Stop(soundStreamID);
            Bass.BASS_StreamFree(soundStreamID);
        }
        #endregion

        #region 销毁
        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            Bass.BASS_Stop();
            Bass.BASS_Free();
        } 
        #endregion
    }
}
