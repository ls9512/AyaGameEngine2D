 using System.Text;

#region 使用说明
/* ★ 使用说明
path为ini文件的物理路径
IniFile ini = new IniFile(path);
//读取ini文件的所有段落名
byte[] allSection = ini.IniReadValues(null, null);
通过如下方式转换byte[]类型为string[]数组类型
string[] sectionList;
ASCIIEncoding ascii = new ASCIIEncoding();
//获取自定义设置section中的所有key，byte[]类型
sectionByte = ini.IniReadValues("personal", null);
//编码所有key的string类型
sections = ascii.GetString(sectionByte);
//获取key的数组
sectionList = sections.Split(new char[1]{'\0'});

//读取ini文件personal段落的所有键名，返回byte[]类型
byte[] sectionByte = ini.IniReadValues("personal", null);

//读取ini文件evideo段落的MODEL键值
model = ini.IniReadValue("evideo", "MODEL");

//将值eth0写入ini文件evideo段落的DEVICE键
ini.IniWriteValue("evideo", "DEVICE", "eth0");
即：
[evideo]
DEVICE = eth0

//删除ini文件下personal段落下的所有键
ini.IniWriteValue("personal", null, null);

//删除ini文件下所有段落
ini.IniWriteValue(null, null, null);
*/
#endregion

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：IniHelper
    /// 功      能：INI文件操作类，提供对INI配置文件的操作功能
    /// 日      期：2015-03-21
    /// 修      改：2015-03-21
    /// 作      者：ls9512
    /// </summary>
    public class IniHelper
    {
        #region 公有成员
        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path { get; set; }
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        public IniHelper(string path)
        {
            Path = path;
        }
        #endregion

        #region INI读写
        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="section">段落</param>
        /// <param name="key">键</param>
        /// <param name="iValue">值</param>
        public void IniWriteValue(string section, string key, string iValue)
        {
            Win32.WritePrivateProfileString(section, key, iValue, Path);
        }

        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">段落</param>
        /// <param name="key">键</param>
        /// <returns>返回的键值</returns>
        public string IniReadValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            Win32.GetPrivateProfileString(section, key, "", temp, 255, Path);
            return temp.ToString();
        }

        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">段，格式[]</param>
        /// <param name="key">键</param>
        /// <returns>返回String类型的section组或键值组</returns>
        /// 不查询的参数填NULL
        public string[] IniReadAllValue(string section, string key)
        {
            byte[] temp = new byte[255];
            Win32.GetPrivateProfileString(section, key, "", temp, 255, Path);
            ASCIIEncoding ascii = new ASCIIEncoding();
            //获取自定义设置section中的所有key，byte[]类型
            string sections = ascii.GetString(temp);
            //获取key的数组
            string[] sectionList = sections.Split('\0');
            return sectionList;
        } 
        #endregion
    }
}
