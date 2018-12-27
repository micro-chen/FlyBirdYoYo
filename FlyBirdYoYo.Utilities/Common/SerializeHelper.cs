using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace FlyBirdYoYo.Utilities.Common
{
   public  class SerializeHelper
    {
        #region XML Serialize
        /// <summary>
        /// 将对象序列化为XML
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <returns>序列化的Base64字符串。</returns>
        public static string SerializeXML(object obj)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                MemoryStream stream = new MemoryStream();
                serializer.Serialize(stream, obj);
                byte[] buffer = stream.ToArray();
                string result = null;
                result = System.Text.Encoding.UTF8.GetString(buffer);
                stream.Close();
                stream.Dispose();
                return result;
            }
            catch
            {
                return null;
            }
        }
        /// 反序列化XML为对象
        /// </summary>
        /// <typeparam name="T">所生成对象的类型。</typeparam>
        /// <param name="xmlString">要进行反序列化的 xmlString 字符串。</param>
        /// <returns>反序列化的对象。</returns>
        public static T DeserializeXml<T>(string xmlString)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(xmlString);
                MemoryStream stream = new MemoryStream(buffer);

                return (T)serializer.Deserialize(stream);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 反序列化XML为List对象
        /// </summary>
        /// <typeparam name="T">所生成对象的类型。</typeparam>
        /// <param name="xmlString">要进行反序列化的 xmlString 字符串。</param>
        /// <returns>反序列化的对象。</returns>
        public static List<T> DeserializeXmlToList<T>(string xmlString)
        {
            return DeserializeXml<List<T>>(xmlString);
        }
        #endregion
    }
}
