using System;
using System.Collections.Generic;
using System.Text;

using AyaGameEngine2D.Models;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：Script
    /// 功      能：脚本组件，用于实现游戏对象功能的程序外部扩展。
    /// 说      明：具体功能暂未实现
    /// 日      期：2016-01-03
    /// 修      改：2016-01-03
    /// 作      者：ls9512
    /// </summary>
    [Serializable]
    public class Script : Component
    {
        #region 公有字段
        public string ScriptText
        {
            get { return _scriptText; }
            set { _scriptText = value; }
        }
        private string _scriptText; 
        #endregion


    }
}
