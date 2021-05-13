using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：Security
    /// 功      能：加密解密类，提供MD5计算和对数据的AES 256级别加密和解密，可以用于游戏数据加密
    /// 日      期：2015-03-20
    /// 修      改：2015-11-22
    /// 作      者：ls9512
    /// </summary>
    public class Security
    {
        #region MD5
        /// <summary>
        /// 获取字符串的MD5
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>MD5</returns>
        public string GetMd5FromStr(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytValue = Encoding.UTF8.GetBytes(str);
            byte[] bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
            return sTemp.ToUpper();
        }

        /// <summary>
        /// 获取文件的MD5
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>MD5</returns>
        public string GetMd5FormFile(string filePath)
        {
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString().ToUpper();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        } 
        #endregion

        #region AES 256 byte[] <-> string key:string
        /// <summary>
        /// AES 256 解密
        /// </summary>
        /// <param name="str">待解密数据</param>
        /// <param name="password">密码</param>
        /// <returns>字符串</returns>
        public string AesDecryptor(byte[] str, string password)
        {
            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.Key = Encoding.UTF8.GetBytes(GetMd5FromStr(password));
                aes.IV = Encoding.UTF8.GetBytes(GetMd5FromStr(password).Substring(8, 16));
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = aes.CreateDecryptor();
                byte[] result = transform.TransformFinalBlock(str, 0, str.Length);
                return Encoding.UTF8.GetString(result);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// AES 256 加密
        /// </summary>
        /// <param name="str">待加密数据</param>
        /// <param name="password">密码</param>
        /// <returns>加密结果</returns>
        public byte[] AesEncryptor(string str, string password)
        {
            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.Key = Encoding.UTF8.GetBytes(GetMd5FromStr(password));
                aes.IV = Encoding.UTF8.GetBytes(GetMd5FromStr(password).Substring(8, 16));
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = aes.CreateEncryptor();
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                return transform.TransformFinalBlock(bytes, 0, bytes.Length);
            }
            catch
            {
                return null;
            }
        } 
        #endregion
    }
}
