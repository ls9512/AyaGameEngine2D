using AyaGameEngine2D;
using GS = JewelHunter.Game.GameStatus;
using SM = JewelHunter.GameIO.SoundManager;

namespace JewelHunter.GameUI
{
    /// <summary>
    /// 类      名：Btn_Start
    /// 功      能：开始按钮类
    /// 日      期：2015-06-25
    /// 修      改：2015-06-25
    /// 作      者：ls9512
    /// </summary>
    public class BtnExit : UIButton
    {
        /// <summary>
        /// 重写是否显示UI
        /// </summary>
        public override bool IsShowUI
        {
            get
            {
                if (GS.GamePhase == GamePhase.Menu) return true;
                return false;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x">坐标x</param>
        /// <param name="y">坐标y</param>
        /// <param name="texture">纹理</param>
        public BtnExit(int x, int y, Texture texture)
            : base(x, y, texture)
        {
        }

        /// <summary>
        /// 重写按钮逻辑
        /// </summary>
        public override void UILogic()
        {
            base.UILogic();
            if (UIStatus == UIStatus.MouseDown && Enable)
            {
                // JewelHunter.GameIO.SoundManager.PlayButton();
            }
            if (UIStatus == UIStatus.MouseClick)
            {
                GameForm.Instance.Close();
                SM.PlayButtonClick();
                SetClickOver();
            }
        }
    }
}
