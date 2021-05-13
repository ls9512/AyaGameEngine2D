using System.Drawing;

using AcgParkour.Models;
using AyaGameEngine2D;

using GS = AcgParkour.Game.GameStatus;
using TM = AcgParkour.GameGraphic.TextureManager;
using GH = AyaGameEngine2D.GraphicHelper;

namespace AcgParkour.GameUI
{
    /// <summary>
    /// 类      名：Btn_Title
    /// 功      能：返回标题按钮类
    /// 日      期：2015-06-25
    /// 修      改：2015-06-25
    /// 作      者：ls9512
    /// </summary>
    public class Btn_Title : UIButton
    {
        /// <summary>
        /// 重写是否显示UI
        /// </summary>
        public override bool IsShowUI
        {
            get
            {
                if (GS.GamePhase == GamePhase.Help || GS.GamePhase == GamePhase.Gaming) return true;
                else return false;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x">坐标x</param>
        /// <param name="y">坐标y</param>
        /// <param name="texture">纹理</param>
        public Btn_Title(int x, int y, Texture texture)
            : base(x, y, texture)
        {
        }

        /// <summary>
        /// 重写按钮逻辑
        /// </summary>
        public override void UILogic()
        {
            base.UILogic();
            if (this.UIStatus == UIStatus.MouseDown && this.Enable)
            {
                AcgParkour.GameIO.SoundManager.PlayButton();
            }
            if (this.UIStatus == UIStatus.MouseClick)
            {
                if (TM.AnimationTransition == null)
                {
                    // 开启渐变
                    TM.AnimationTransition = new AnimationTrans(new Texture(General.Data_Path + @"\Graphic\Transitions\Transitions_Black.png"), "BackTitle");
                    this.SetClickOver();
                }
            }
        }

        /// <summary>
        /// 重写按钮绘图
        /// </summary>
        public override void UIGraphic()
        {
            base.UIGraphic();
            // 碰撞矩形
            if (General.Debug_ShowRect)
            {
                GH.DrawRectangle(Color.White, this.UIDrawRect);
            }
        }
    }
}
