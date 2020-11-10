using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Public.CSUtil.Log;

namespace AlertTime
{
    /// <summary>
    /// Xml�ַ�����Xml�ļ������ת�����������кͷ����л���
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
        /// ��һ���������л�ΪXML�ַ���
        /// </summary>
        /// <param name="o">Ҫ���л��Ķ���</param>
        /// <param name="encoding">���뷽ʽ</param>
        /// <returns>���л�������XML�ַ���</returns>
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
        /// ��һ������XML���л��ķ�ʽд�뵽һ���ļ�
        /// </summary>
        /// <param name="o">Ҫ���л��Ķ���</param>
        /// <param name="path">�����ļ�·��</param>
        /// <param name="encoding">���뷽ʽ</param>
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
        /// ��XML�ַ����з����л�����
        /// </summary>
        /// <typeparam name="T">�����������</typeparam>
        /// <param name="s">���������XML�ַ���</param>
        /// <param name="encoding">���뷽ʽ</param>
        /// <returns>�����л��õ��Ķ���</returns>
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
        /// ����һ���ļ�������XML�ķ�ʽ�����л�����
        /// </summary>
        /// <typeparam name="T">�����������</typeparam>
        /// <param name="path">�ļ�·��</param>
        /// <param name="encoding">���뷽ʽ</param>
        /// <returns>�����л��õ��Ķ���</returns>
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
        /// ��ȡ�����ļ��е���Ϣ
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
                LogUtil.Write(String.Format("XmlHelper.GetXmlInfo��ȡXML�ļ�����ʧ�ܣ�path: {0}, type: {1}, msg: {2}"
                    , path, typeof(T).Name, e.ToString()), LogType.Error);
            }

            return default;
        }
    }
}
