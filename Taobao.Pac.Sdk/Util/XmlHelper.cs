using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace Taobao.Pac.Sdk.Core
{

    /// <summary>  
    /// <remarks>Xml序列化与反序列化</remarks>  
    /// <creator>zhangdapeng</creator>  
    /// </summary>  
    internal class XmlHelper
    {
        #region 反序列化  

        /// <summary>  
        /// 反序列化  
        /// </summary>  
        /// <param name="xml">XML字符串</param>  
        /// <returns></returns>  
        public static T Deserialize<T>(string xml)
        {
            Type type = typeof(T);

            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmldes = new XmlSerializer(type,"");
                return (T)xmldes.Deserialize(sr);
            }
        }

        /// <summary>  
        /// 反序列化  
        /// </summary>  
        /// <param name="type">类型</param>  
        /// <param name="xml">XML字符串</param>  
        /// <returns></returns>  
        public static object Deserialize(Type type, string xml)
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmldes = new XmlSerializer(type,"");
                return xmldes.Deserialize(sr);
            }
        }
        /// <summary>  
        /// 反序列化  
        /// </summary>  
        /// <param name="type"></param>  
        /// <param name="xml"></param>  
        /// <returns></returns>  
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type,"");
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region 序列化  
        /// <summary>  
        /// 序列化  
        /// </summary>  
        /// <param name="type">类型</param>  
        /// <param name="obj">对象</param>  
        /// <returns></returns>  
        public static string SerializeObject(Type type, object obj)
        {
            string strXml = string.Empty;

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            //settings.IndentChars = "    ";
            settings.NewLineChars = "\r\n";
            settings.Encoding = Encoding.UTF8;
            settings.OmitXmlDeclaration = true;  // 不生成声明头
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", null);//把命名空间设置为空，这样就没有命名空间了

            using (MemoryStream sm = new MemoryStream())
            {

                using (XmlWriter writer = XmlWriter.Create(sm, settings))
                {
                    XmlSerializer formatter = new XmlSerializer(type);
                    formatter.Serialize(writer, obj, ns);

                }


                strXml = Encoding.UTF8.GetString(sm.ToArray());
                sm.Position = 0;
            }
            if (strXml[0]!='<')
            {
                strXml = strXml.Substring(strXml.IndexOf('<'));
            }
            return strXml;
        }


        /// <summary>  
        /// 序列化  
        /// </summary>  
        /// <param name="obj">对象</param>  
        /// <returns></returns>  
        public static string SerializeObject<T>(object obj)
        {
            string strXml = string.Empty;
            Type type = typeof(T);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            //settings.IndentChars = "    ";
            settings.NewLineChars = "\r\n";
            settings.Encoding = Encoding.UTF8;
            settings.OmitXmlDeclaration = true;  // 不生成声明头
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", null);//把命名空间设置为空，这样就没有命名空间了

            using (MemoryStream sm = new MemoryStream())
            {

                using (XmlWriter writer = XmlWriter.Create(sm, settings))
                {
                    XmlSerializer formatter = new XmlSerializer(type);
                    formatter.Serialize(writer, obj, ns);
                }

                strXml = Encoding.UTF8.GetString(sm.ToArray());
                sm.Position = 0;
            }
            if (strXml[0] != '<')
            {
                strXml = strXml.Substring(strXml.IndexOf('<'));
            }
            return strXml;

        }


        #endregion
    }

}

