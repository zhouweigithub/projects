using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Public.CSUtil.Log;

namespace AlertTime
{
    /// <summary>
    /// Xml字符串、Xml文件与对象转换（采用序列和反序列化）
    /// </summary>
    public static class XmlHelper
    {
        private static void XmlSerializeInternal(Stream stream, Object o, Encoding encoding)
        {
            if (o == null)
            {
                throw new ArgumentNullException("o");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            XmlSerializer serializer = new XmlSerializer(o.GetType());

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                NewLineChars = "\r\n",
                Encoding = encoding,
                IndentChars = "    "
            };

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, o);
            }
        }

        /// <summary>
        /// 将一个对象序列化为XML字符串
        /// </summary>
        /// <param name="o">要序列化的对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>序列化产生的XML字符串</returns>
        public static String XmlSerialize(Object o, Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializeInternal(stream, o, encoding);

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 将一个对象按XML序列化的方式写入到一个文件
        /// </summary>
        /// <param name="o">要序列化的对象</param>
        /// <param name="path">保存文件路径</param>
        /// <param name="encoding">编码方式</param>
        public static void XmlSerializeToFile(Object o, String path, Encoding encoding)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                XmlSerializeInternal(file, o, encoding);
            }
        }

        /// <summary>
        /// 从XML字符串中反序列化对象
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="s">包含对象的XML字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserialize<T>(String s, Encoding encoding)
        {
            if (String.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("s");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            XmlSerializer mySerializer = new XmlSerializer(typeof(T));

            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(s)))
            {
                using (StreamReader sr = new StreamReader(ms, encoding))
                {
                    return (T)mySerializer.Deserialize(sr);
                }
            }
        }

        /// <summary>
        /// 读入一个文件，并按XML的方式反序列化对象。
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserializeFromFile<T>(String path, Encoding encoding)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            String xml = File.ReadAllText(path, encoding);

            return XmlDeserialize<T>(xml, encoding);
        }

        /// <summary>
        /// 获取配置文件中的信息
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T GetXmlInfo<T>(String path, Encoding encoding) where T : new()
        {
            try
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(T));

                String xml = File.ReadAllText(path, encoding);

                using (MemoryStream ms = new MemoryStream(encoding.GetBytes(xml)))
                {
                    using (StreamReader sr = new StreamReader(ms, encoding))
                    {
                        return (T)mySerializer.Deserialize(sr);
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.Write(String.Format("XmlHelper.GetXmlInfo读取XML文件内容失败！path: {0}, type: {1}, msg: {2}"
                    , path, typeof(T).Name, e.ToString()), LogType.Error);
            }

            return default;
        }
    }
}
