namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：Sound
    /// 功      能：声音快速调用静态接口
    ///             用户可以自行调用SoundManager使用也可由此接口使用声音功能
    /// 日      期：2015-12-21
    /// 修      改：2015-12-21
    /// 作      者：ls9512
    /// </summary>
    public static class Sound
    {
        #region 创建
        /// <summary>
        /// 创建音频流
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns>流ID</returns>
        public static int Create(string fileName)
        {
            return SoundManager.Instance.CreateSoundStream(fileName);
        }
        #endregion

        #region 播放 / 暂停 / 停止
        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        public static void Play(int soundStreamID)
        {
            SoundManager.Instance.Play(soundStreamID);
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        public static void Pause(int soundStreamID)
        {
            SoundManager.Instance.Pause(soundStreamID);
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        public static void Stop(int soundStreamID)
        {
            SoundManager.Instance.Stop(soundStreamID);
        }
        #endregion

        #region 状态
        /// <summary>
        /// 获取音频播放状态
        /// </summary>
        /// <param name="soundStreamID"></param>
        public static SoundPlayStatus Active(int soundStreamID)
        {
            return SoundManager.Instance.Active(soundStreamID);
        }
        #endregion

        #region 音量
        /// <summary>
        /// 设置一定时间达到目标播放音量（淡入淡出）
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        /// <param name="aimVolume">目标音量(0-1)</param>
        /// <param name="time">达到该音量时间(ms)</param>
        public static void SetPlayVolume(int soundStreamID, float aimVolume, int time)
        {
            SoundManager.Instance.SetPlayVolume(soundStreamID, aimVolume, time);
        }

        /// <summary>
        /// 设置播放音量
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        /// <param name="volume">播放音量(0-1)</param>
        public static void SetPlayVolume(int soundStreamID, float volume)
        {
            SoundManager.Instance.SetPlayVolume(soundStreamID, volume);
        }

        /// <summary>
        /// 获取播放音量
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        /// <returns>音量</returns>
        public static float GetPlayVolume(int soundStreamID)
        {
            return SoundManager.Instance.GetPlayVolume(soundStreamID);
        }

        /// <summary>
        /// 设置系统音量
        /// </summary>
        /// <param name="volume">音量(0-1)</param>
        public static void SetSystemVolume(float volume)
        {
            SoundManager.Instance.SetSystemVolume(volume);
        }

        /// <summary>
        /// 获取系统音量
        /// </summary>
        /// <returns></returns>
        public static float GetSystemVolume()
        {
            return SoundManager.Instance.GetSystemVolume();
        }
        #endregion

        #region 时间 / 位置
        /// <summary>
        /// 设置循环播放
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        /// <param name="isLoop">是否循环</param>
        public static void SetLoop(int soundStreamID, bool isLoop)
        {
            SoundManager.Instance.SetLoop(soundStreamID, isLoop);
        }

        /// <summary>
        /// 获取当前播放时间
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        /// <returns>播放时间(秒)</returns>
        public static double GetPosition(int soundStreamID)
        {
            return SoundManager.Instance.GetPosition(soundStreamID);
        }

        /// <summary>
        /// 设置播放位置
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        /// <param name="time">播放位置(秒)</param>
        public static void SetPosition(int soundStreamID, double time)
        {
            SoundManager.Instance.SetPosition(soundStreamID, time);
        }

        /// <summary>
        /// 获取音频持续时间
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        /// <returns>长度(秒)</returns>
        public static double GetLength(int soundStreamID)
        {
            return SoundManager.Instance.GetLength(soundStreamID);
        }
        #endregion

        #region FFT采样
        /// <summary>
        /// 获取FFT采样数据，返回512个浮点采样数据
        /// </summary>
        /// <returns>频谱采样数组</returns>
        public static float[] GetFFTData(int soundStreamID)
        {
            return SoundManager.Instance.GetFFTData(soundStreamID);
        }
        #endregion

        #region 释放
        /// <summary>
        /// 释放音频
        /// </summary>
        /// <param name="soundStreamID">流ID</param>
        public static void Release(int soundStreamID)
        {
            SoundManager.Instance.Release(soundStreamID);
        } 
        #endregion
    }
}
