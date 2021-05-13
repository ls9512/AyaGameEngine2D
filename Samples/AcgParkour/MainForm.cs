using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using AyaGameEngine2D;

using GS = AcgParkour.Game.GameStatus;
using Timer = System.Windows.Forms.Timer;

namespace AcgParkour
{
    /// <summary>
    /// 类      名：MainForm
    /// 功      能：游戏主窗体类，通过继承Age2DForm并重写必要的方法来调用引擎功能
    /// 日      期：2015-03-17
    /// 修      改：2015-06-21
    /// 作      者：ls9512
    /// </summary>
    public class MainForm : GameForm
    {
        #region 静态实例
        /// <summary>
        /// 鼠标事件管理类静态实例
        /// </summary>
        public static MainForm Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MainForm();
                return _instance;
            }
        }
        private static MainForm _instance; 
        #endregion

        #region 窗体开启和关闭动画
        // 渐变效果参数
        private int _flagWindowOpen = 0;
        private double _formOpacity = 1.0;
        private const double SpeedWindowOpen = 2.0;
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
            _formOpacity = Convert.ToInt32(this.Opacity * 100) - 1;
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
                //渐变启动窗口
                _formOpacity += SpeedWindowOpen;
                this.Opacity = Math.Sin(1.0 * _formOpacity / 100 * Math.PI / 2);//用正弦曲线使得渐变平滑
                if (_formOpacity > 100)
                {
                    // this.Opacity = (sf.Form_EndOpacity) * 1.0 / 100;
                    this.Opacity = 1;
                    _timerWindowOpen.Enabled = false;
                }
            }
            else
            {
                //渐变关闭窗口
                _formOpacity -= SpeedWindowOpen;
                this.Opacity = Math.Sin(1.0 * _formOpacity / 100 * Math.PI / 2);
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
        public MainForm()
        { 
            // 绑定事件
            this.Load += GameForm_Load;
            this.Deactivate += GameForm_Deactivate;
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_Load(object sender, EventArgs e)
        {
            try
            {
                Visible = false;
                // 载入配置
                LoadConfig();
                // 设置标题
                Text = General.Pro_Title;
                // 设置图标
                Icon = new Icon(General.Data_Path + @"\Graphic\Icon\Icon.ico");
                // 设置窗体尺寸
                Engine.SetGameRect(General.Draw_Rect.Width, General.Draw_Rect.Height);
                // 初始化引擎
                Engine.EngineInit();

                // 加载LOGO
                Display.AddEngineLogo(new Bitmap(General.Data_Path + @"\Graphic\UI\Logo_Age2D.jpg"));
                Display.AddEngineLogo(new Bitmap(General.Data_Path + @"\Graphic\UI\Logo_ls9512.jpg"));
                Display.AddEngineLogo(new Bitmap(General.Data_Path + @"\Graphic\UI\Logo_GM.jpg"));
                // 设置绘图帧数
                Engine.SetRenderFps(General.Game_Fps);
                // 隐藏鼠标
                Cursor.Hide();
                // 设置鼠标样式
                // this.SetCursorStyle(new Bitmap(General.Data_Path + @"\Graphic\Icon\MouseCursor.png"), new Point(0, 0));
                // 游戏初始化
                GS.GameInit();

                //适配显示器分辨率
                // int width = (int)(Screen.PrimaryScreen.WorkingArea.Width * 0.48); int height = (int)(Screen.PrimaryScreen.WorkingArea.Height * 0.48);
                //int width = 1280; int height = 720;
                //this.SetGameWinRect(width, height);

                AyaGameEngine2D.General.Engine_ShowLogo = false;

                Opacity = 0;
                Visible = true;
                OpenWindow();

                // 启动引擎,所有必须资源要在引擎启动前完成初始化
                Engine.EngineStart();
            }
            catch (Exception exception)
            {
                Debug.ThrowException("加载错误", exception);
            }
        }

        /// <summary>
        /// 载入配置
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                // 加载配置文件
                IniHelper ini = new IniHelper(FileSystem.GetProgramPath() + @"\Config.ini");
                // 读取数据文件路径
                string value = FileSystem.GetProgramPath() + ini.IniReadValue("AgeEngine2d", "DataPath");
                // 如果路径存在
                if (System.IO.Directory.Exists(value))
                {
                    General.Data_Path = value;
                }
                // 读取调试
                value = ini.IniReadValue("AgeEngine2d", "DEBUG");
                if (Convert.ToInt32(value) == 1)
                {
                    AyaGameEngine2D.General.Engine_RunMode = EngineRunMode.Debug;
                }
                else
                {
                    AyaGameEngine2D.General.Engine_RunMode = EngineRunMode.Release;
                }
                // 读取BGM
                value = ini.IniReadValue("AgeEngine2d", "BGM");
                if (Convert.ToInt32(value) == 1)
                {
                    General.Game_BGM = true;
                }
                else
                {
                    General.Game_BGM = false;
                }
                // 读取SE
                value = ini.IniReadValue("AgeEngine2d", "SE");
                if (Convert.ToInt32(value) == 1)
                {
                    General.Game_SE = true;
                }
                else
                {
                    General.Game_SE = false;
                }
                // 读取帧数
                value = ini.IniReadValue("AgeEngine2d", "FPS");
                General.Game_Fps = Convert.ToInt32(value);
                if (General.Game_Fps < 30) General.Game_Fps = 30;
                // 读取鼠标效果
                value = ini.IniReadValue("AgeEngine2d", "MOUSE");
                if (Convert.ToInt32(value) == 1)
                {
                    General.Game_MouseEffect = true;
                }
                else
                {
                    General.Game_MouseEffect = false;
                }
            }
            catch (Exception e)
            {
                Debug.ThrowException("载入配置错误", e);
            }
        }

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
                Debug.ThrowException("绘图错误", e);
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
                Debug.ThrowException("逻辑错误", e);
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
                GS.GameIO();
            }
            catch (Exception e)
            {
                Debug.ThrowException("输入错误", e);
            }
        }

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
                GS.IsPauseCountDown = false;
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
            // base.OnClosing(new CancelEventArgs(false));
        }
    }
}
