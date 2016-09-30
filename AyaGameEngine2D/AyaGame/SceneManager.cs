namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：SceneManager
    /// 功      能：场景管理器，用于实现游戏场景调度。
    /// 日      期：2016-01-01
    /// 修      改：2016-01-01
    /// 作      者：ls9512
    /// </summary>
    public class SceneManager
    {
        #region 静态实例
        /// <summary>
        /// KeyManager静态实例
        /// </summary>
        public static SceneManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SceneManager();
                return _instance;
            }
        }
        private static SceneManager _instance;
        #endregion

        /// <summary>
        /// 当前场景
        /// </summary>
        public Scene NowScene = null;

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="scene">场景</param>
        public void LoadScene(Scene scene)
        { 
        }
    }
}
