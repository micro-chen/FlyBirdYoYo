using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace FlyBirdYoYo.Utilities.DEncrypt
{
	/// <summary>
	/// DES加密/解密类。
	/// </summary>
	public class DESEncrypt
	{

        private const string DEFAULT_ENCRYPT_KEY = StringExtension.DEFAULT_ENCRYPT_KEY;



        #region ========加密======== 


        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey = DEFAULT_ENCRYPT_KEY)
        {
            using (DESCryptoServiceProvider provider = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(Text);
                provider.Key = Encoding.UTF8.GetBytes(sKey.Substring(0, 8));
                provider.IV = provider.Key;
                
                MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, provider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string s = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return s;
            }

        }

        #endregion

        #region ========解密======== 



        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey = DEFAULT_ENCRYPT_KEY)
        {
            byte[] inputByteArray = Convert.FromBase64String(Text);
            using (DESCryptoServiceProvider provider = new DESCryptoServiceProvider())
            {
                provider.Key = Encoding.UTF8.GetBytes(sKey.Substring(0, 8));
                provider.IV = provider.Key;

                MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, provider.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string s = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return s;
            }
        }

        #endregion



	}
}
