using System;
using System.ComponentModel;
using System.Drawing;

using AyaGameEngine2D;
using GS = JewelHunter.Game.GameStatus;
using Timer = System.Windows.Forms.Timer;

namespace JewelHunter
{
    /// <summary>
    /// 类      名：GameForm
    /// 功      能：游戏主窗体类，通过继承GameForm并重写必要的方法来调用引擎功能
    /// 日      期：2015-11-23
    /// 修      改：2016-05-08
    /// 作      者：ls9512
    /// </summary>
    public partial class MainForm : GameForm
    {
        #region 静态实例
        /// <summary>
        /// 窗体类静态实例
        /// </summary>
        public static MainForm Instance => _instance ?? (_instance = new MainForm());

        private static MainForm _instance;
        #endregion

        #region 窗体开启和关闭动画
        // 渐变效果参数
        private int _flagWindowOpen;
        private double _formOpacity = 1.0;
        private readonly double _speedWindowOpen = 2.0;
        private Timer _timerWindowOpen;

        /// <summary>
        /// 开启窗体动画
        /// </summary>
        private void OpenWindow()
        {
            _timerWindowOpen = new Timer();
            _timerWindowOpen.Tick += Timer_WindowOpen_Tick;
            _timerWindowOpen.Interval = 1;
            _timerWindowOpen.Enabled = true;
        }

        /// <summary>
        /// 关闭窗体动画
        /// </summary>
        public void CloseWindow()
        {
            _flagWindowOpen = 1;
            _formOpacity = Convert.ToInt32(Opacity * 100) - 1;
            _timerWindowOpen.Enabled = true;
        }

        /// <summary>
        /// 窗口启动效果时钟
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_WindowOpen_Tick(object sender, EventArgs e)
        {
            if (_flagWindowOpen == 0)
            {
                // 渐变启动窗口
                _formOpacity += _speedWindowOpen;
                // 用正弦曲线使得渐变平滑
                Opacity = Math.Sin(1.0 * _formOpacity / 100 * Math.PI / 2);
                if (_formOpacity > 100)
                {
                    // this.Opacity = (sf.Form_EndOpacity) * 1.0 / 100;
                    Opacity = 1;
                    _timerWindowOpen.Enabled = false;
                }
            }
            else
            {
                // 渐变关闭窗口
                _formOpacity -= _speedWindowOpen;
                Opacity = Math.Sin(1.0 * _formOpacity / 100 * Math.PI / 2);
                if (_formOpacity < 1)
                {
                    CloseGameForm();
                }
            }
        }
        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        protected MainForm()
        {
            // 绑定事件
            Load += GameForm_Load;
            Deactivate += GameForm_Deactivate;
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_Load(object sender, EventArgs e)
        {
            Visible = false;
            // 载入配置
            LoadConfig();
            // 设置标题
            Text = General.ProTitle;
            // 设置图标
            Icon = new Icon(General.DataPath + @"\Graphic\UI\Icon.ico");
            // 设置为非调试模式
            AyaGameEngine2D.General.Engine_RunMode = EngineRunMode.Release;
            // 设置窗体尺寸
            Engine.SetGameRect(General.DrawRect.Width, General.DrawRect.Height);
            // 初始化引擎
            Engine.EngineInit();
            // 加载LOGO
            Display.AddEngineLogo(new Bitmap(General.DataPath + @"\Graphic\UI\Logo_Studio.png"));
            // Display.AddEngineLogo(new Bitmap(General.DataPath + @"\Graphic\UI\Logo_Engine.png"));
            Display.AddEngineLogo(new Bitmap(General.DataPath + @"\Graphic\UI\Logo_School.png"));
            // 设置绘图帧数
            Engine.SetRenderFps(General.GameFps);
            // 游戏初始化
            try
            {
                GS.GameInit();
            }
            catch (Exception ex)
            {
                Debug.ThrowException("初始化错误", ex);
            }

            CenterToScreen();

            Display.SetCursorShow(false);

            Opacity = 0;
            Visible = true;
            OpenWindow();

            // 启动引擎,所有必须资源要在引擎启动前完成初始化
            Engine.EngineStart();

            // 延时播放BGM
            AyaGameEngine2D.Timer.Delay(JewelHunter.GameIO.SoundManager.PlayTitleBgm, 0);
        }

        #region 读取配置文件
        /// <summary>
        /// 读取配置文件
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                // 加载配置文件
                IniHelper ini = new IniHelper(FileSystem.GetProgramPath() + @"\Config.ini");
                // 读取数据文件路径
                string value = FileSystem.GetProgramPath() + ini.IniReadValue("JewelHunter", "DataPath");
                // 如果路径存在
                if (System.IO.Directory.Exists(value))
                {
                    General.DataPath = value;
                }
            }
            catch (Exception e)
            {
                Debug.ThrowException("载入配置错误", e);
            }
        } 
        #endregion

        #region 重写游戏主循环体
        /// <summary>
        /// 重写游戏绘图函数
        /// ======================================================
        /// 在此处根据需要调用用户自定义的子函数绘制游戏画面
        /// </summary>
        protected override void GameRender()
        {
            try
            {
                GS.GameDraw();
            }
            catch (Exception e)
            {
                Debug.ThrowException("Graphic Error", e);
            }
        }

        /// <summary>
        /// 重写游戏逻辑函数
        /// ======================================================
        /// 在此处根据需要执行游戏的逻辑处理
        /// </summary>
        protected override void GameLogic()
        {
            try
            {
                GS.GameLogic();
            }
            catch (Exception e)
            {
                Debug.ThrowException("Loggic Error", e);
            }
        }

        /// <summary>
        /// 重写游戏IO函数
        /// ======================================================
        /// 在此处根据需要执行游戏的输入输出处理
        /// </summary>
        protected override void GameIO()
        {
            try
            {
                GS.GameIo();
            }
            catch (Exception e)
            {
                Debug.ThrowException("Input Error", e);
            }
        }
        #endregion

        /// <summary>
        /// 失去焦点时关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_Deactivate(object sender, EventArgs e)
        {
            if (GS.GamePhase == GamePhase.Gaming)
            {
                GS.IsPause = true;
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            // 取消关闭并调用关闭动画
            e.Cancel = true;
            CloseWindow();
        }

        /// <summary>
        /// 关闭游戏窗体
        /// </summary>
        private void CloseGameForm()
        {
            // 释放资源
            GS.Dispose();
            // 关闭窗体
            Dispose();
        }
    }
}
