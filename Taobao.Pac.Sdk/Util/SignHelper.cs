using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Taobao.Pac.Sdk.Core
{
    public class SignHelper
    {
        /// <summary>
        /// 官方签名辅助类
        /// http://open.taobao.com/doc.htm?docId=107148&docType=1
        /// </summary>
        /// <param name="content">报文内容</param>
        /// <param name="appSecret">appSecret</param>
        /// <param name="charset">编码方式，编码方式目前支持GBK与UTF-8两种</param>
        /// <returns></returns>
        public static string MakeSign(string content,  string appSecret,string charset= "UTF-8")
        {

            string toSignContent = content + appSecret;

            MD5 md5 = MD5.Create();

            byte[] inputBytes =Encoding.GetEncoding(charset).GetBytes(toSignContent);
            byte[] hash = md5.ComputeHash(inputBytes);

            return Convert.ToBase64String(hash);
        }
    }
}
