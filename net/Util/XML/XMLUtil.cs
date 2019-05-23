// ****************************************
// FileName:XMLUtil.cs
// Description: XML助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
// ****************************************

using System;
using System.Xml;

namespace Util.XML
{
    /// <summary>
    /// XML助手类
    /// </summary>
    public static class XMLUtil
    {
        /// <summary>
        /// 返回 XML字符串 节点value
        /// </summary>
        /// <param name="xmlDoc">XML格式 数据</param>
        /// <param name="xmlNode">节点</param>
        /// <exception cref="System.Xml.XmlException"></exception>
        /// <returns>节点value</returns>
        public static String GetXmlNode(String xmlDoc, String xmlNode)
        {
            XmlDocument xml = new XmlDocument();

            //加载xml内容
            xml.LoadXml(xmlDoc);

            //获取xml节点，并返回节点内容
            XmlNode xn = xml.SelectSingleNode(xmlNode);

            return xn.InnerText;
        }
    }
}