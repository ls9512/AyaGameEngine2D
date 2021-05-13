using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using LuaInterface;

namespace AyaGameEngine2D.AyaExtends
{
    /// <summary>
    /// 类      名：LuaScript
    /// 功      能：提供对Lua脚本的基础支持
    /// 日      期：2015-12-27
    /// 修      改：2015-12-27
    /// 作      者：ls9512
    /// </summary>
    public static class LuaScript
    {
        #region 私有成员
        /// <summary>
        /// Lua脚本操作实例
        /// </summary>
        private static Lua _lua = new Lua();
        #endregion

        /// <summary>
        /// 注册LUA方法
        /// </summary>
        /// <param name="luaFuncName">lua方法名</param>
        /// <param name="targetClass">类</param>
        /// <param name="classFunc">类中的方法名</param>
        /// <remarks>使用："Function", this, this.GetType().GetMethod("Function")</remarks>
        public static void RegisterFunction(string luaFuncName, object targetClass, MethodBase classFunc)
        {
            _lua.RegisterFunction(luaFuncName, targetClass, classFunc);
        }

        /// <summary>
        /// 执行LUA脚本文件
        /// </summary>
        /// <param name="scriptFilePath">LUA脚本文件路径</param>
        public static void ExecuteScriptFile(string scriptFilePath)
        {
            _lua.DoFile(scriptFilePath);
        }

        /// <summary>
        /// 执行LUA脚本
        /// </summary>
        /// <param name="scriptText">LUA脚本</param>
        public static void ExcuteScript(string scriptText)
        {
            _lua.DoString(scriptText);
        }

        /// <summary>
        /// 执行LUA函数
        /// </summary>
        /// <param name="functionName">函数名</param>
        public static void GetFunction(string functionName)
        {
            _lua.GetFunction(functionName);
        }
    }
}
