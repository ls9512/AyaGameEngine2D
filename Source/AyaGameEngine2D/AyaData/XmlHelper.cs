using System;
using System.Xml;
using System.Data;
using System.IO;
using System.Xml.Serialization;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：XmlHelper
    /// 功      能：XML文件操作类，提供对XML文件和节点读写操作的基本方法
    /// 日      期：2015-03-27
    /// 修      改：2015-12-17
    /// 作      者：ls9512
    /// </summary>
    public class XmlHelper
    {
        #region 私有成员
        /// <summary>
        /// XML文件的物理路径
        /// </summary>
        private readonly string _filePath = string.Empty;

        /// <summary>
        /// Xml文档
        /// </summary>
        private XmlDocument _xml;

        /// <summary>
        /// XML的根节点
        /// </summary>
        private XmlElement _element;
        #endregion

        #region 构造
        /// <summary>
        /// 实例化XmlHelper对象
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的绝对路径</param>
        public XmlHelper(string xmlFilePath)
        {
            //获取XML文件的绝对路径
            _filePath = xmlFilePath;
        }
        #endregion

        #region 创建
        /// <summary>
        /// 创建XML的根节点
        /// </summary>
        private void CreateXmlElement()
        {
            //创建一个XML对象
            _xml = new XmlDocument();
            if (Directory.Exists(_filePath))
            {
                //加载XML文件
                _xml.Load(_filePath);
            }
            //为XML的根节点赋值
            _element = _xml.DocumentElement;
        } 
        #endregion

        #region 序列化 反序列化
        /// <summary>
        /// 对象序列化为XML文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sourceObj"></param>
        /// <param name="type"></param>
        /// <param name="xmlRootName"></param>
        public static void SaveToXml(string filePath, object sourceObj, Type type, string xmlRootName)
        {
            if (!string.IsNullOrEmpty(filePath) && sourceObj != null)
            {
                type = type != null ? type : sourceObj.GetType();

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    XmlSerializer xmlSerializer = string.IsNullOrEmpty(xmlRootName) ?
                        new XmlSerializer(type) :
                        new XmlSerializer(type, new XmlRootAttribute(xmlRootName));
                    xmlSerializer.Serialize(writer, sourceObj);
                }
            }
        }

        /// <summary>
        /// XML文件反序列化为对象
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object LoadFromXml(string filePath, Type type)
        {
            object result = null;

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(type);
                    result = xmlSerializer.Deserialize(reader);
                }
            }

            return result;
        }
        #endregion

        #region XML操作
        /// <summary>
        /// 获取指定XPath表达式的节点对象
        /// </summary>        
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public XmlNode GetNode(string xPath)
        {
            //创建XML的根节点
            CreateXmlElement();
            //返回XPath节点
            return _element.SelectSingleNode(xPath);
        }

        /// <summary>
        /// 获取指定XPath表达式节点的值
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public string GetValue(string xPath)
        {
            //创建XML的根节点
            CreateXmlElement();
            //返回XPath节点的值
            return _element.SelectSingleNode(xPath).InnerText;
        }

        /// <summary>
        /// 获取指定XPath表达式节点的属性值
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        /// <param name="attributeName">属性名</param>
        public string GetAttributeValue(string xPath, string attributeName)
        {
            //创建XML的根节点
            CreateXmlElement();
            //返回XPath节点的属性值
            return _element.SelectSingleNode(xPath).Attributes[attributeName].Value;
        }

        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将任意节点插入到当前Xml文件中。
        /// </summary>        
        /// <param name="xmlNode">要插入的Xml节点</param>
        public void AppendNode(XmlNode xmlNode)
        {
            //创建XML的根节点
            CreateXmlElement();
            //导入节点
            XmlNode node = _xml.ImportNode(xmlNode, true);
            //将节点插入到根节点下
            _element.AppendChild(node);
        }

        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将DataSet中的第一条记录插入Xml文件中。
        /// </summary>        
        /// <param name="ds">DataSet的实例，该DataSet中应该只有一条记录</param>
        public void AppendNode(DataSet ds)
        {
            //创建XmlDataDocument对象
            XmlDataDocument xmlDataDocument = new XmlDataDocument(ds);
            //导入节点
            XmlNode node = xmlDataDocument.DocumentElement.FirstChild;
            //将节点插入到根节点下
            AppendNode(node);
        }

        /// <summary>
        /// 删除指定XPath表达式的节点
        /// </summary>        
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public void RemoveNode(string xPath)
        {
            //创建XML的根节点
            CreateXmlElement();
            //获取要删除的节点
            XmlNode node = _xml.SelectSingleNode(xPath);
            //删除节点
            _element.RemoveChild(node);
        }

        /// <summary>
        /// 保存XML文件
        /// </summary>        
        public void Save()
        {
            //创建XML的根节点
            CreateXmlElement();
            //保存XML文件
            _xml.Save(_filePath);
        }

        /// <summary>
        /// 创建根节点对象
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的绝对路径</param>        
        private static XmlElement CreateRootElement(string xmlFilePath)
        {
            //定义变量，表示XML文件的绝对路径
            string filePath = xmlFilePath;
            //创建XmlDocument对象
            XmlDocument xmlDocument = new XmlDocument();
            //加载XML文件
            xmlDocument.Load(filePath);

            //返回根节点
            return xmlDocument.DocumentElement;
        }

        /// <summary>
        /// 获取指定XPath表达式节点的值
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public static string GetValue(string xmlFilePath, string xPath)
        {
            //创建根对象
            XmlElement rootElement = CreateRootElement(xmlFilePath);
            //返回XPath节点的值
            return rootElement.SelectSingleNode(xPath).InnerText;
        }

        /// <summary>
        /// 获取指定XPath表达式节点的属性值
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        /// <param name="attributeName">属性名</param>
        public static string GetAttributeValue(string xmlFilePath, string xPath, string attributeName)
        {
            //创建根对象
            XmlElement rootElement = CreateRootElement(xmlFilePath);
            //返回XPath节点的属性值
            return rootElement.SelectSingleNode(xPath).Attributes[attributeName].Value;
        } 
        #endregion
    }
}
