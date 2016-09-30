using System;
using System.Drawing;
using CsGL.OpenGL;

using AyaGameEngine2D.Core;

namespace AyaGameEngine2D.Controls
{
    /// <summary>
    /// 类      名：OpenGLPanel
    /// 功      能：游戏面板控件，用于承载游戏画面，接收鼠标事件
    /// 日      期：2015-03-20
    /// 修      改：2015-11-21
    /// 作      者：ls9512
    /// </summary>
    internal class OpenGLPanel : OpenGLControl
    {
        #region 静态实例
        /// <summary>
        /// OpenGL控件静态实例
        /// </summary>
        public static OpenGLPanel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new OpenGLPanel();
                return _instance;
            }
        }
        private static OpenGLPanel _instance; 
        #endregion

        #region 绘图委托
        /// <summary>
        /// 绘图委托声明
        /// </summary>
        internal delegate void GameDrawEventHandler();
        /// <summary>
        /// 游戏绘图委托
        /// </summary>
        internal event GameDrawEventHandler OnGameRender;
        #endregion

        #region OpenGL 环境调用和初始化
        /// <summary>
        /// 创建绘制环境
        /// </summary>
        /// <returns></returns>
        protected override OpenGLContext CreateContext()
        {
            // return base.CreateContext();
            ControlGLContext c = new ControlGLContext(this);
            // 双缓冲
            c.Create(new DisplayType(DisplayFlags.DOUBLEBUFFER, true), null);
            return c;
        }

        /// <summary>
        /// 执行OpenGL初始化
        /// </summary>
        protected override void InitGLContext()
        {
            base.InitGLContext();
            // 平滑模式
            OpenGL.glShadeModel(OpenGL.GL_SMOOTH);
            // 清除画面
            OpenGL.glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            // 重置投影矩阵
            OpenGL.glMatrixMode(OpenGL.GL_PROJECTION);
            OpenGL.glPushMatrix();
            OpenGL.glLoadIdentity();
            // 启用纹理
            OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
            // 设置混合
            OpenGL.glEnable(OpenGL.GL_BLEND);
            OpenGL.glBlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            // 设置深度缓存
            // GL.glClearDepth(1.0f);
            // 启用深度测试
            //GL.glDisable(GL.GL_DEPTH_TEST);
            // 所作深度测试的类型
            //GL.glDepthFunc(GL.GL_LEQUAL);
            // 最精细的透视计算
            //GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_HINT, GL.GL_NICEST);
            // 最快速压缩
            OpenGL.glHint(OpenGL_Extension.GL_TEXTURE_COMPRESSION_HINT, OpenGL.GL_FASTEST);
            // 去掉背面
            OpenGL.glEnable(OpenGL.GL_CULL_FACE);
            OpenGL.glCullFace(OpenGL.GL_BACK);
            // 关闭光照
            OpenGL.glDisable(OpenGL.GL_LIGHTING);
            // 设置视口
            GLU.gluOrtho2D(0.0, General.Draw_Rect.Width, General.Draw_Rect.Height, 0.0);
        }
        
        #endregion

        #region 参数设置
        /// <summary>
        /// 设置游戏绘制宽度 ★ 有问题，慎用
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        internal void SetDrawRect(int width, int height)
        {
            General.Draw_Rect = new Rectangle(0, 0, width, height);
            // 设置视口
            GLU.gluOrtho2D(0.0, width, height, 0);
        } 
        #endregion

        #region GL绘图
        /// <summary>
        /// 重写绘制函数
        /// </summary>
        public override void glDraw()
        {
            // 清除颜色和深度缓冲
            OpenGL.glClear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            if (General.Engine_IsInit)
            {
                if (General.Engine_ShowLogo)
                {
                    EngineLogo.Render();
                }
                else
                {
                    // 执行游戏绘图委托
                    OnGameRender();
                    // 引擎级绘图
                    EngineLoop.Render();
                }
            }
            OpenGL.glFinish();
        } 
        #endregion

        #region 控件尺寸
        /// <summary>
        /// 控件大小改变时
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        } 
        #endregion

        #region 屏幕截图
        /// <summary>
        /// 屏幕截图
        /// ★ 文件读写会阻塞引擎运行，仅供调试用
        /// </summary>
        public void ScreenShot()
        {
            IntPtr handle = General.Pro_GLHandle;
            Rectangle rect = Rectangle.Empty;
            // 获取指定句柄的窗口 Rectangle
            IntPtr result = Win32.GetWindowRect(handle, ref rect);
            if (result != IntPtr.Zero) // 如果成功获取
            {
                Bitmap bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(rect.Location,
                    Point.Empty, ClientSize, CopyPixelOperation.SourceCopy);
                    g.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);
                    // 如果不存在，则创建
                    if (!FileSystem.IsExistDirectory(General.Engine_ScreenShot_Path))
                    {
                        FileSystem.CreateDir(General.Engine_ScreenShot_Path);
                    }
                    // 以时间命名保存
                    string path = General.Engine_ScreenShot_Path + Time.TimeStringToFormart("yyyy-MM-dd HH-mm-ss-fff") + ".png";
                    bmp.Save(path);
                    // 消息提示
                    InfoPublisher.PushEngineInfo(new EngineInfo("截图成功", "截图保存于：" + path, DockLocType.LowerRightCorner));
                }
            }
        }
        #endregion
    }
}
