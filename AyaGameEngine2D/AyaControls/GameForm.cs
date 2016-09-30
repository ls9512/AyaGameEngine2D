using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

using AyaGameEngine2D.Controls;
using AyaGameEngine2D.Core;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：GameForm
    /// 功      能：提供对OpenGL承载控件的容器以及程序的启动器，程序主窗口通过继承此类来调用引擎
    /// 说      明：引擎核心实现
    /// 日      期：2015-03-20
    /// 修      改：2016-01-02
    /// 作      者：ls9512
    /// </summary>
    public abstract class GameForm : Form
    {
        #region 私有成员
        /// <summary>
        /// 游戏主循环
        /// </summary>
        private static GameLoop _gameLoop;
        /// <summary>
        /// 游戏面板控件
        /// </summary>
        private readonly OpenGLPanel _gamePanel;
        /// <summary>
        /// 帧时间
        /// </summary>
        private float _graphicFrameTime = 1000f / General.Engine_Fps_Def;
        /// <summary>
        /// 安全帧时间
        /// </summary>
        private readonly float _safeGraphicFrameTime = 1000f / General.Engine_Fps_Def + 1;
        /// <summary>
        /// 帧开始时间
        /// </summary>
        private long _graphicFrameStartTime = 0;
        /// <summary>
        /// 帧结束时间
        /// </summary>
        private long _graphicFrameEndTime = 0;
        /// <summary>
        /// 帧开始时间
        /// </summary>
        private long _graphicFrameStartCpuCount = 0;
        /// <summary>
        /// 帧结束时间
        /// </summary>
        private long _graphicFrameEndCpuCount = 0;
        /// <summary>
        /// 上一帧间隔时间
        /// </summary>
        private float _lastDeltaTime = 0;
        /// <summary>
        /// 上一帧时间间隔(无缩放)
        /// </summary>
        private float _lastDeltaTimeUnScale = 0;
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造函数
        /// </summary>
        protected GameForm()
        {
            // 设置创体句柄
            General.Pro_WinHandle = Handle;
            // 窗体参数设置
            Text = General.Pro_Title;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            ImeMode = ImeMode.Disable;
            DoubleBuffered = true;
            // 允许跨线程操作
            CheckForIllegalCrossThreadCalls = false;
            // 注册引擎方法回调
            Engine.OnEngineInit += GameEngineInit;
            Engine.OnEngineStart += GameEngineStart;
            Engine.OnEngineClose += GameEngineClose;
            Engine.OnSetGameWinRect += SetGameWinRect;
            Engine.OnSetGameDrawRect += SetGameDrawRect;
            Engine.OnSetRenderFps += SetRenderFps;
            Display.OnSetCursorStyle += SetCursorStyle;
            GameLoop.OnLoopInit += PreciseTimer.StartTimer;
            GameLoop.OnLoopInit += GameTimer.StartTimer;
            GameLoop.OnLoopStart += PreciseTimer.ResetPreviousTime;
            // 启动子模块
            object get = null;
            get = KeyManager.Instance;
            get = MouseManager.Instance;
            get = SoundManager.Instance;
            get = LayerManager.Instance;
            get = SceneManager.Instance;
            get = GameLoop.Instance;
            get = OpenGLPanel.Instance;
            get = null;
            // 加载OpenGL控件
            _gamePanel = OpenGLPanel.Instance;
            _gamePanel.Dock = DockStyle.Fill;
            _gamePanel.OnGameRender += GameRender;
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        #endregion

        #region 引擎初始化
        /// <summary>
        /// 游戏初始化
        /// </summary>
        internal void GameEngineInit()
        {
            // 添加控件
            Controls.Add(_gamePanel);
            // 记录句柄
            General.Pro_WinHandle = Handle;
            General.Pro_GLHandle = _gamePanel.Handle;
            // 接收按键
            KeyPreview = true;
            // 设置绘图主循环
            _gameLoop = GameLoop.Instance;
            switch (General.Engine_TimeMode)
            {
                case EngineTimeMode.GameTimer: _gameLoop.SetCallBack(doNextFrame_GameTimer); break;
                case EngineTimeMode.PreciseTimer: _gameLoop.SetCallBack(doNextFrame_PreciseTimer); break;
            }
            // 调试模式提示
            if (General.Engine_Debug)
            {
                EngineInfo info = new EngineInfo(
                    "Age2D - Debug模式",
                    "   当前处于Debug模式，可以使用调试快捷键。\n    F1：显示调试信息\n    F5：逐帧模式\n    F11：绘制下一帧\n    F12：截图",
                    DockLocType.LowerRightCorner);
                // info.SetTitleColor(Color.RoyalBlue);
                info.SetTextShowTime(4f);
                Debug.PushEngineInfo(info);
                Debug.AddLog("Age2D", "以Debug模式启动引擎。");
            }
            // 引擎初始化完成
            General.Engine_IsInit = true;
        }
        #endregion

        #region 引擎启动
        /// <summary>
        /// 启动引擎,开始游戏
        /// </summary>
        internal void GameEngineStart()
        {
            // 开启游戏主循环线程
            _gameLoop.StartLoop();
        }
        #endregion

        #region 引擎参数设置
        /// <summary>
        /// 设置游戏窗体尺寸
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        internal void SetGameWinRect(int width, int height)
        {
            General.Win_Rect = new Rectangle(0, 0, width, height);
            ClientSize = General.Win_Rect.Size;
            CenterToScreen();
        }

        /// <summary>
        /// 设置游戏绘制尺寸
        /// ★ 会导致黑屏
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        internal void SetGameDrawRect(int width, int height)
        {
            General.Draw_Rect = new Rectangle(0, 0, width, height);
            _gamePanel.SetDrawRect(width, height);
        }

        /// <summary>
        /// 设置画面渲染帧数
        /// ======================================================
        /// 决定游戏画面的刷新速度，需要与逻辑帧数相匹配
        /// </summary>
        /// <param name="fps">帧数</param>
        internal void SetRenderFps(float fps)
        {
            fps = fps < 1 ? 1 : fps;
            fps = fps > General.Engine_Fps_Max ? General.Engine_Fps_Max : fps;
            General.Engine_Fps_Now = fps;
            _graphicFrameTime = 1000f / fps;
            // if (1000f / this._graphicFrameTime < fps) this._graphicFrameTime--;
        }  
        #endregion

        #region 主循环体 GmaeTimer
        /// <summary>
        /// 绘制一帧
        /// </summary>
        /// <param name="deltaTime">帧消耗时间</param>
        private void doNextFrame_GameTimer(float deltaTime)
        {
            try
            {
                #region 帧开始
                // 将帧消耗时间赋值给时间静态类
                _lastDeltaTime = deltaTime;
                _lastDeltaTimeUnScale = deltaTime;
                Time.DeltaTime = deltaTime * General.Engine_Fps_Def;
                Time.DeltaTimeUnScale = deltaTime * General.Engine_Fps_Def;
                // 时间缩放
                Time.DeltaTime *= Time.TimeScale;

                // 帧开始
                _graphicFrameStartTime = GameTimer.DurationMillisecond;
                #endregion

                #region 逻辑
                // 输入更新
                KeyManager.Instance.Update(_lastDeltaTime);
                MouseManager.Instance.Update(_lastDeltaTime);
                if (General.Engine_ShowLogo)
                {
                    EngineLogo.IO();
                }
                else
                {
                    // 输入处理
                    GameIO();
                    EngineLoop.IO();
                }

                // 逻辑
                if (General.Engine_ShowLogo)
                {
                    EngineLogo.Logic();
                }
                else
                {
                    GameLogic();
                    EngineLoop.Logic();
                }

                // 逻辑执行超时,跳帧
                if (GameTimer.DurationMillisecond - _graphicFrameStartTime >= _graphicFrameTime)
                {
                    // 帧结束
                    _graphicFrameEndTime = GameTimer.DurationMillisecond;
                    // 性能计数
                    PerformanceAnalyzer.Gaming_LogicFpsOutTime++;
                    return;
                }
                #endregion

                #region 渲染
                // 刷新绘图
                _gamePanel.Refresh();
                #endregion

                #region 帧性能评估
                // 耗时高于安全帧时间阈值,发出帧性能不足警告
                if (GameTimer.DurationMillisecond - _graphicFrameStartTime > _safeGraphicFrameTime)
                {
                    // 帧结束
                    _graphicFrameEndTime = GameTimer.DurationMillisecond;
                    // 性能计数
                    PerformanceAnalyzer.Gaming_GraphicFpsOutTime++;
                    // 帧数统计
                    PerformanceAnalyzer.PerformanceCount(_lastDeltaTimeUnScale);
                    return;
                } 
                #endregion

                #region 帧延时
                // 有帧数限制时则开始进行延迟
                if (General.Engine_Fps_Limit)
                {
                    // Sleep填充剩余不足一帧的时间
                    while (GameTimer.DurationMillisecond - _graphicFrameStartTime < _graphicFrameTime - 1)
                    {
                        Thread.Sleep(1);
                    }

                    // 空循环精确填充剩余不足一帧的时间
                    while (PreciseTimer.DurationMillisecond - _graphicFrameStartTime < _graphicFrameTime)
                    {
                    }
                } 
                #endregion

                #region 帧结束
                // 帧结束
                _graphicFrameEndTime = GameTimer.DurationMillisecond;

                // 帧数统计
                PerformanceAnalyzer.PerformanceCount(_lastDeltaTimeUnScale); 
                #endregion
            }
            catch (Exception e)
            {
                Debug.ThrowException("主循环错误", e);
            }
        }
        #endregion

        #region 主循环体 PreciseTimer
        /// <summary>
        /// 绘制一帧
        /// </summary>
        /// <param name="deltaTime">帧消耗时间</param>
        private void doNextFrame_PreciseTimer(float deltaTime)
        {
            try
            {
                #region 帧开始
                // 将帧消耗时间赋值给时间静态类
                _lastDeltaTime = deltaTime;
                _lastDeltaTimeUnScale = deltaTime;
                Time.DeltaTimeUnScale = deltaTime * General.Engine_Fps_Def;
                Time.DeltaTime = Time.DeltaTimeUnScale * Time.TimeScale;

                // 帧开始
                _graphicFrameStartCpuCount = PreciseTimer.DurationPerformanceCounter;
                #endregion

                #region 逻辑
                float timeStart;
                float timeEnd;

                timeStart = PreciseTimer.DurationPerformanceCounter;

                // 输入更新
                KeyManager.Instance.Update(_lastDeltaTime);
                MouseManager.Instance.Update(_lastDeltaTime);
                if (General.Engine_ShowLogo)
                {
                    EngineLogo.IO();
                }
                else
                {
                    // 输入处理
                    GameIO();
                    EngineLoop.IO();
                }

                // 逻辑
                if (General.Engine_ShowLogo)
                {
                    EngineLogo.Logic();
                }
                else
                {
                    GameLogic();
                    EngineLoop.Logic();
                }

                timeEnd = PreciseTimer.DurationPerformanceCounter;
                Time.CpuTime = (timeEnd - timeStart) * 1f / PreciseTimer.TicksPerMillisecond;

                // 逻辑执行超时,跳帧
                if (PreciseTimer.DurationPerformanceCounter - _graphicFrameStartCpuCount > _graphicFrameTime * PreciseTimer.TicksPerMillisecond)
                {
                    // 帧结束
                    _graphicFrameEndCpuCount = PreciseTimer.DurationPerformanceCounter;
                    // 性能计数
                    PerformanceAnalyzer.Gaming_LogicFpsOutTime++;
                    return;
                }
                #endregion

                #region 渲染
                // 刷新绘图 
                _gamePanel.Refresh();
                #endregion

                #region 帧性能评估
                // 耗时高于安全帧时间阈值,发出帧性能不足警告
                if (PreciseTimer.DurationPerformanceCounter - _graphicFrameStartCpuCount > _safeGraphicFrameTime * PreciseTimer.TicksPerMillisecond)
                {
                    // 帧结束
                    _graphicFrameEndCpuCount = PreciseTimer.DurationPerformanceCounter;
                    // 性能计数
                    PerformanceAnalyzer.Gaming_GraphicFpsOutTime++;
                    // 帧数统计
                    PerformanceAnalyzer.PerformanceCount(_lastDeltaTimeUnScale);
                    return;
                }
                #endregion

                #region 帧延时
                // 有帧数限制时则开始进行延迟
                if (General.Engine_Fps_Limit)
                {
                    // Sleep填充剩余不足一帧的时间
                    while (PreciseTimer.DurationPerformanceCounter - _graphicFrameStartCpuCount < (_graphicFrameTime - 1) * PreciseTimer.TicksPerMillisecond)
                    {
                        Thread.Sleep(1);
                    }

                    // 空循环精确填充剩余不足一帧的时间
                    while (PreciseTimer.DurationPerformanceCounter - _graphicFrameStartCpuCount < _graphicFrameTime * PreciseTimer.TicksPerMillisecond)
                    {
                    }
                }
                #endregion

                #region 帧结束
                // 帧结束
                _graphicFrameEndCpuCount = PreciseTimer.DurationPerformanceCounter;

                // long fpsTime = this._graphicFrameEndCpuCount - this._graphicFrameStartCpuCount;

                // 帧数统计
                PerformanceAnalyzer.PerformanceCount(_lastDeltaTimeUnScale);
                #endregion
            }
            catch (Exception e)
            {
                Debug.ThrowException("主循环错误", e);
            }
        }
        #endregion

        #region 需要被继承重写的循环体
        /// <summary>
        /// 游戏绘制函数
        /// ======================================================
        /// 游戏的核心绘图，在游戏初始化完成后每一帧都执行一次
        /// ★ 必须被重写，由用户实现
        /// </summary>
        protected abstract void GameRender();

        /// <summary>
        /// 游戏逻辑函数
        /// ======================================================
        /// 游戏的核心逻辑，在每一帧绘制结束后执行
        /// ★ 必须被重写，由用户实现
        /// </summary>
        protected abstract void GameLogic();

        /// <summary>
        /// 游戏IO处理函数
        /// ======================================================
        /// 游戏的核心IO处理，在游戏初始化完成后每一帧都执行一次
        /// ★ 必须被重写，由用户实现
        /// </summary>
        protected abstract void GameIO(); 
        #endregion

        #region 按键处理
        /// <summary>
        /// 按键处理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            // 调试控制
            if (General.Engine_Debug)
            {
                switch (e.KeyCode)
                {
                    case Keys.F5:
                        // 调试暂停
                        General.Debug_FrameStop = !General.Debug_FrameStop;
                        if (General.Debug_FrameStop)
                        {
                            _gameLoop.PauseLoop();
                        }
                        else
                        {
                            _gameLoop.StartLoop();
                        }
                        break;
                    case Keys.F11:
                        // 绘制下一帧
                        switch (General.Engine_TimeMode)
                        {
                            case EngineTimeMode.GameTimer: doNextFrame_GameTimer(_lastDeltaTime); break;
                            case EngineTimeMode.PreciseTimer: doNextFrame_PreciseTimer(_lastDeltaTime); break;
                        } 
                        
                        break;
                    case Keys.F12:
                        // 截图
                        Display.ScreenShot();
                        break;
                    case Keys.F1:
                        // 调试信息开关
                        General.Debug_ShowFPS = !General.Debug_ShowFPS;
                        break;
                }
            }
        }
        #endregion

        #region 鼠标指针样式
        /// <summary>
        /// 设置鼠标指针样式
        /// </summary>
        /// <param name="cursor">鼠标图片</param>
        /// <param name="hotPoint">热点</param>
        internal void SetCursorStyle(Bitmap cursor, Point hotPoint)
        {
            int hotX = hotPoint.X;
            int hotY = hotPoint.Y;
            Bitmap myNewCursor = new Bitmap(cursor.Width * 2 - hotX, cursor.Height * 2 - hotY);
            Graphics g = Graphics.FromImage(myNewCursor);
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g.DrawImage(cursor, cursor.Width - hotX, cursor.Height - hotY, cursor.Width,
            cursor.Height);
            Cursor = new Cursor(myNewCursor.GetHicon());
            g.Dispose();
            myNewCursor.Dispose();
        } 
        #endregion

        #region 引擎关闭
        /// <summary>
        /// 关闭引擎
        /// </summary>
        internal void GameEngineClose()
        {
            // 结束循环
            _gameLoop.PauseLoop();
            _gamePanel.Dispose();
            // 写入日志
            if (General.Engine_Debug)
            {
                LogHelper.AddLog("Age 2D", PerformanceAnalyzer.GetPerformanceReport());
            }
            Dispose();
        }

        /// <summary>
        /// 重写窗体关闭
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            GameEngineClose();
        } 
        #endregion
    }
}
