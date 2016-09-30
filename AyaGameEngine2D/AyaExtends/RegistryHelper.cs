using Microsoft.Win32;

#region 使用说明
/*
读注册表：
            RegistryHelper rh = new RegistryHelper();
            string portName = rh.GetRegistryData(Registry.LocalMachine, "SOFTWARE\\TagReceiver\\Params\\SerialPort", "PortName");
写注册表：
            RegistryHelper rh = new RegistryHelper();
            rh.SetRegistryData(Registry.LocalMachine, "SOFTWARE\\TagReceiver\\Params\\SerialPort", "PortName", portName);
*/

#endregion

namespace AyaGameEngine2D.Extends
{
    /// <summary>
    /// 类      名：RegistryHelper
    /// 功      能：注册表操作类，提供对注册表的读取、写入，删除和检查。
    /// 日      期：2016-01-19
    /// 修      改：2016-01-19
    /// 作      者：ls9512
    /// </summary>
    public class RegistryHelper
    {
        #region 读取
        /// <summary>
        /// 读取指定名称的注册表的值
        /// </summary>
        /// <param name="root">根</param>
        /// <param name="subkey">子健</param>
        /// <param name="name">值</param>
        /// <returns>结果</returns>
        public string GetRegistryData(RegistryKey root, string subkey, string name)
        {
            string registData = "";
            RegistryKey myKey = root.OpenSubKey(subkey, true);
            if (myKey != null)
            {
                registData = myKey.GetValue(name).ToString();
            }

            return registData;
        }  
        #endregion

        #region 写入
        /// <summary>
        /// 向注册表中写数据
        /// </summary>
        /// <param name="root">根</param>
        /// <param name="subkey">子健</param>
        /// <param name="name">值</param>
        public void SetRegistryData(RegistryKey root, string subkey, string name, string value)
        {
            RegistryKey aimdir = root.CreateSubKey(subkey);
            aimdir.SetValue(name, value);
        }  
        #endregion

        #region 删除
        /// <summary>
        /// 删除注册表中指定的注册表项
        /// </summary>
        /// <param name="root">根</param>
        /// <param name="subkey">子健</param>
        /// <param name="name">值</param>
        public void DeleteRegistry(RegistryKey root, string subkey, string name)
        {
            RegistryKey myKey = root.OpenSubKey(subkey, true);
            var subkeyNames = myKey.GetSubKeyNames();
            foreach (string aimKey in subkeyNames)
            {
                if (aimKey == name)
                    myKey.DeleteSubKeyTree(name);
            }
        }  
        #endregion

        #region 判断存在
        /// <summary>
        /// 判断指定注册表项是否存在
        /// </summary>
        /// <param name="root">根</param>
        /// <param name="subkey">子健</param>
        /// <param name="name">值</param>
        /// <returns>结果</returns>
        public bool IsRegistryExist(RegistryKey root, string subkey, string name)
        {
            bool exist = false;
            RegistryKey myKey = root.OpenSubKey(subkey, true);
            var subkeyNames = myKey.GetSubKeyNames();
            foreach (string keyName in subkeyNames)
            {
                if (keyName == name)
                {
                    exist = true;
                    return exist;
                }
            }
            return exist;
        }  
        #endregion
    }
}
