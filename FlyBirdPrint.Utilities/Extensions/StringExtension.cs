using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using System.Globalization;

namespace System
{
    public static class StringExtension
    {

        /// <summary>
        /// 默认DES 加密/解密 密钥
        /// </summary>
        public const string DEFAULT_ENCRYPT_KEY = "bhkf8+jSJXb507GpE+I1nDQAw";


        /// <summary>
        /// 截取字符串 ，从开始到结束索引。类似python中的字符串截取
        /// Returns characters slices from string between two indexes.
        /// If start or end are negative, their indexes will be calculated counting 
        /// back from the end of the source string. 
        /// If the end param is less than the start param, the Slice will return a 
        /// substring in reverse order.
        /// 
        /// <param name="source">String the extension method will operate upon.</param>
        /// <param name="startIndex">Starting index, may be negative.</param>
        /// <param name="endIndex">Ending index, may be negative).</param>
        /// </summary>
        public static string Slice(this string source, int startIndex, int endIndex = int.MaxValue)
        {
            // If startIndex or endIndex exceeds the length of the string they will be set 
            // to zero if negative, or source.Length if positive.
            if (source.ExceedsLength(startIndex)) startIndex = startIndex < 0 ? 0 : source.Length;
            if (source.ExceedsLength(endIndex)) endIndex = endIndex < 0 ? 0 : source.Length;

            // Negative values count back from the end of the source string.
            if (startIndex < 0) startIndex = source.Length + startIndex;
            if (endIndex < 0) endIndex = source.Length + endIndex;

            // Calculate length of characters to slice from string.
            int length = Math.Abs(endIndex - startIndex);
            // If the endIndex is less than the startIndex, return a reversed substring.
            if (endIndex < startIndex) return source.Substring(endIndex, length).Reverse();

            return source.Substring(startIndex, length);
        }

        /// <summary>
        /// Reverses character order in a string.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>string</returns>
        public static string Reverse(this string source)
        {
            char[] charArray = source.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// Verifies that the index is within the range of the string source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns>bool</returns>
        public static bool ExceedsLength(this string source, int index)
        {
            return Math.Abs(index) > source.Length ? true : false;
        }

        /// <summary>
        /// 获取url 的协议+域名
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetUrlStringDomainWithScheme(this string source)
        {
            var uri = new Uri(source);
            string result = string.Empty;
            try
            {
                result = string.Format("{0}://{1}/", uri.Scheme, uri.Host);
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 获取url 的 cookie域；
        /// 比如：www.taobao.com ,返回.taobao.com
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetUrlCookieDomain(this string source)
        {
            var uri = new Uri(source);
            string result = string.Empty;
            try
            {
                var hostName = uri.Host;
                result = hostName.Substring(hostName.IndexOf('.'));
            }
            catch { }
            return result;
        }

        /// <summary>
        /// 判断路径是否存在 不存在则创建 返回路径
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string PathExists(this string str)
        {
            if (!Directory.Exists(str))
            {
                Directory.CreateDirectory(str);
            }
            return str;
        }
        /// <summary>
        /// 获取截取后的字符串,英文占1个字节，中文占两个字节
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length">最多中文数</param>
        /// <returns></returns>
        public static string GetByteString(this string str, int length)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            string newStr = str;
            if (b.Length > length * 2)
            {
                byte[] b1 = new byte[length * 2];
                for (int i = 0; i < length * 2; i++)
                {
                    b1[i] = b[i];
                }
                newStr = Encoding.Default.GetString(b1) + "...";
                if (newStr.Contains('?'))
                {
                    byte[] b2 = new byte[length * 2 - 1];
                    for (int i = 0; i < b1.Length - 1; i++)
                    {
                        b2[i] = b1[i];
                    }
                    newStr = Encoding.Default.GetString(b2) + "...";
                }
            }
            return newStr;
        }

        /// <summary>
        /// 判断字符串是否为null或者空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        /// <summary>
        /// 使用string.Format方式将字符串连接起来
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string With(this string str, params object[] args)
        {
            return string.Format(str, args);
        }
        /// <summary>
        /// 拼接URL字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlSupply(this string str)
        {
            if (str.IndexOf("?") == -1) return str + "?";
            return str + "&";
        }
        /// <summary>
        /// 将字符串以MD5方式加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5(this string str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(str);
            bs = md5.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToUpper());
            }
            string password = s.ToString();
            return password;
        }







        /// <summary>
        /// Create salt key
        /// </summary>
        /// <param name="size">Key size</param>
        /// <returns>Salt key</returns>
        public static string CreateSaltKey(int size)
        {
            // Generate a cryptographic random number
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }

        /// <summary>
        /// Create a password hash
        /// </summary>
        /// <param name="password">{assword</param>
        /// <param name="saltkey">Salk key</param>
        /// <param name="passwordFormat">Password format (hash algorithm)</param>
        /// <returns>Password hash</returns>
        public static string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1")
        {
            if (String.IsNullOrEmpty(passwordFormat))
                passwordFormat = "SHA1";
            string saltAndPassword = String.Concat(password, saltkey);

            //return FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPassword, passwordFormat);
            var algorithm = HashAlgorithm.Create(passwordFormat);
            if (algorithm == null)
                throw new ArgumentException("Unrecognized hash name", "hashName");

            var hashByteArray = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword));
            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }

        /// <summary>
        /// 将字符串以SHA1方式加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetSHA1(this string str)
        {
            System.Security.Cryptography.SHA1CryptoServiceProvider SHA1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(str);
            bs = SHA1.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToUpper());
            }
            string password = s.ToString();
            return password;
        }

        /// <summary>
        /// 以Byte方式截取字符串长度
        /// 默认从0开始截取
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ByteSubstring(this string s, int length, string info)
        {
            if (length < ByteLength(s))
            {
                return s.ByteSubstring(0, length) + info;
            }
            return s.ByteSubstring(0, length);
        }
        /// <summary>
        /// 以Byte方式获取字符串长度
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ByteLength(this string s)
        {
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(s);
            int n = 0, i = 0;
            for (; i < bytes.GetLength(0); i++)
            {
                if (i % 2 == 0) n++;
                else
                    if (bytes[i] > 0) n++;
            }
            return n;
        }
        /// <summary>
        /// 以Byte方式截取字符串长度
        /// 默认从0开始截取
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ByteSubstring(this string s, int length)
        {
            return s.ByteSubstring(0, length);
        }
        /// <summary>
        /// 以Byte方式截取字符串长度
        /// </summary>
        /// <param name="s"></param>
        /// <param name="startIndex">开始位置（从0开始）</param>
        /// <param name="length">要截取的长度</param>
        /// <returns></returns>
        public static string ByteSubstring(this string s, int startIndex, int length)
        {
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(s);
            int n = 0, i = 0;
            for (; i < bytes.GetLength(0) && n < length; i++)
            {
                if (i % 2 == 0) n++;
                else
                    if (bytes[i] > 0) n++;
            }
            if (i % 2 == 1)
            {
                if (bytes[i] > 0) i = i - 1;
                else i = i + 1;
            }
            return System.Text.Encoding.Unicode.GetString(bytes, startIndex, length == 0 ? bytes.GetLength(0) : i);
        }
        /// <summary>
        /// 将一个字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string s)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(s, new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.IsoDateFormat });
        }
        /// <summary>
        /// 将字符串序列化为json格式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToJson(this string s)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(s, new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.IsoDateFormat });
        }
        public static string GetSafeString(this string s)
        {
            Regex r = new Regex(@"[^\u4e00-\u9fa5A-Za-z0-9]", RegexOptions.IgnoreCase);
            return r.Replace(s, "").Replace("/", "_").Replace(@"\", "_");
        }

        /// <summary>
        /// 首字母 转为大写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToTitledBigString(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            string titledConfig = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(input);
            return titledConfig;
        }

        /// <summary>
        /// 将 int? 转换为int类型，如果转换失败则返回0
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt(this int? s)
        {
            int c = 0;
            if (s==null)
            {
                return c; 
            }
            return s.Value;
        }
        /// <summary>
        /// 将字符串转换为int类型，如果转换失败则返回0
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt(this string s)
        {
            int c = 0;
            if (!int.TryParse(s, out c))
            {
                c = 0;
            }
            return c;
        }
        /// <summary>
        /// 将字符串转换为int类型，如果转换失败则返回默认数字dNum
        /// </summary>
        /// <param name="s"></param>
        /// <param name="dNum">默认数字</param>
        /// <returns></returns>
        public static int ToInt(this string s, int dNum)
        {
            int c = 0;
            if (!int.TryParse(s, out c))
            {
                c = dNum;
            }
            return c;
        }
        /// <summary>
        /// 将字符串转换为long类型，如果转换失败则返回0
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static long ToLong(this string s)
        {
            long c = 0;
            if (!long.TryParse(s, out c))
            {
                c = 0;
            }
            return c;
        }
        /// <summary>
        /// 将字符串转换为long类型，如果转换失败则返回默认数字dNum
        /// </summary>
        /// <param name="s"></param>
        /// <param name="dNum">默认数字</param>
        /// <returns></returns>
        public static long ToLong(this string s, int dNum)
        {
            long c = 0;
            if (!long.TryParse(s, out c))
            {
                c = dNum;
            }
            return c;
        }
        /// <summary>
        /// 将字符串转换为Datetime类型，如果转换失败则返回1970年1月1日
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ToDatetime(this string s)
        {
            DateTime d;
            if (!DateTime.TryParse(s, out d))
            {
                d = new DateTime(1970, 1, 1);
            }
            return d;
        }
        /// <summary>
        /// 将字符串转换为decimal类型，如果转换失败则返回0
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string s)
        {
            decimal c = 0;
            if (!decimal.TryParse(s, out c))
            {
                c = 0;
            }
            return c;
        }
        public static bool ToBool(this string s)
        {
            bool b = false;
            Boolean.TryParse(s, out b);
            return b;
        }

        /// <summary>
        /// 参数URL编码，同JS的encodeURIComponent
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string EscapeDataString(this string t)
        {
            return Uri.EscapeDataString(t);
        }

        /// <summary>
        /// 参数URL解码，同JS的decodeURIComponent
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string UnescapeDataString(this string t)
        {
            if (t.IsNullOrEmpty())
            {
                return "";
            }
            return Uri.UnescapeDataString(t);
        }

        /// <summary>
        /// URL Encode 编码
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string URLEncode(this string t)
        {
            return HttpUtility.UrlEncode(t, Encoding.UTF8);
        }

        /// <summary>
        /// URL Decode 解码
        /// </summary>
        /// <param name="pStr"></param>
        /// <returns></returns>
        public static string URLDecode(this string t)
        {
            return HttpUtility.UrlDecode(t, Encoding.UTF8);
        }
        public static string ReplaceRegex(this string t, string Regex)
        {
            // Regex r = new Regex("<.*?>");
            Regex r = new Regex(Regex);
            return r.Replace(t, "");
        }
       /// <summary>
       /// 跨站加密字符串签名
       /// 指定字符串的md5+时间戳-的MD5
       /// </summary>
       /// <param name="toEnCodeString"></param>
       /// <returns></returns>
        public static string GetCrossSiteSign(this string toEnCodeString,long timeStamp)
        {
            return (toEnCodeString + timeStamp).GetMD5();
        }

        /// <summary>
        /// id加密算法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ERd(this string str)
        {
            var rd = new Random(Convert.ToInt32(str));
            var nrd = rd.Next(1000001, 9999999).ToString();
            var bstr = nrd.Substring(0, 3);
            var estr = nrd.Substring(3, 4);
            return bstr + str.ToString() + estr;
        }
        /// <summary>
        /// id解密算法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string URd(this string str)
        {
            //461 34567 8709
            if (str.Length < 7)
            {
                return str;
            }
            var rd = str.Substring(3, str.Length - 7);
            return rd;
        }
        public static bool CheckIsChinessReg(this string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fbb]+$");
        }



        /// <summary>
        /// 静态构造函数
        /// </summary>
         static StringExtension()
        {
            var _timer_manage_business_number = OrderNumberManagedTimer;
            if (null == _timer_manage_business_number)
            {
                throw new Exception("StringUtil-OrderNumberManagedTimer初始化失败！");
            }
        }



        /// <summary>
        /// 得到对象的 DateTime 类型的值，默认值为DateTime.MinValue
        /// </summary>
        /// <param name="Value">要转换的值</param>
        /// <param name="defaultValue">如果转换失败，返回默认值为DateTime.MinValue</param>
        /// <returns>如果对象的值可正确返回， 返回对象转换的值 ，否则， 返回的默认值为DateTime.MinValue</returns>
        public static DateTime GetDateTime(this object Value, DateTime defaultValue)
        {
            if (Value == null) return defaultValue;

            if (Value is DBNull) return defaultValue;

            string strValue = Value as string;
            if (strValue == null && (Value is IConvertible))
            {
                return (Value as IConvertible).ToDateTime(CultureInfo.CurrentCulture);
            }
            if (strValue != null)
            {
                strValue = strValue
                    .Replace("年", "-")
                    .Replace("月", "-")
                    .Replace("日", "-")
                    .Replace("点", ":")
                    .Replace("时", ":")
                    .Replace("分", ":")
                    .Replace("秒", ":")
                      ;
            }
            DateTime dt = defaultValue;
            if (DateTime.TryParse(Value.ToString(), out dt))
            {
                return dt;
            }

            return defaultValue;
        }

    




        /// <summary>
        /// 生成随机码（包含字母）
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomString(int length)
        {
            string chars = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";

            return GetRandomCode(chars, length);
        }

        /// <summary>
        /// 生成随机码（不包含字母）
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomNumberString(int length)
        {
            string chars = "1,2,3,4,5,6,7,8,9";

            return GetRandomCode(chars, length);
        }




        #region  在单位时间内产生的随机订单流水号容器管理
        //此类的静态成员单例容器，定时进行流水号的情况（秒），防止重复的订单号

        private static System.Threading.Timer _OrderNumberManagedTimer;
        private static List<string> _OnePeriodGenerateOrderNumberRecords;


        /// <summary>
        /// 计时器，管理一段时间内的订单号
        /// </summary>
        private static System.Threading.Timer OrderNumberManagedTimer
        {
            get
            {
                if (_OrderNumberManagedTimer == null)
                {
                    _OrderNumberManagedTimer = new System.Threading.Timer(new System.Threading.TimerCallback((state) =>
                    {
                        //一旦初始化化完毕定时器，立即启动，每间隔一秒钟，对流水号的容器，进行一次清理
                        OnePeriodGenerateOrderNumberRecords.Clear();
                    }), null, 0, 1000);
                }
                return _OrderNumberManagedTimer;
            }


        }

        /// <summary>
        ///一定时间内产生的订单号集合- 
        /// </summary>

        private static List<string> OnePeriodGenerateOrderNumberRecords
        {
            get
            {

                if (null == _OnePeriodGenerateOrderNumberRecords)
                {
                    _OnePeriodGenerateOrderNumberRecords = new List<string>();
                }
                return _OnePeriodGenerateOrderNumberRecords;
            }


        }

        /// <summary>
        /// 生成业务流水号;
        /// 算法：
        /// 0215162722+000000
        ///年月日时分秒+6位随机数=每秒10的6次方=100W
        /// </summary>
        /// <returns></returns>
        public static string GenerateBusinessOrderNumber()
        {
            var orderNumber = string.Empty;
            while (string.IsNullOrEmpty(orderNumber))
            {
                //为了保证订单流水号的不重复性，只要在一定的时间段产生了重复的订单号，那么从新生成
                string firstPartOrderNumber = DateTime.Now.ToString("yyyyMMddHHmmss");//第一部分：年月日时分秒
                string secondPartOrderNumber = GenerateRandomSequence(6);//第一部分：6位的随机数

                string tmpOrderNumber = string.Concat(firstPartOrderNumber, secondPartOrderNumber);
                //从当前内存订单集合管理队列中检索是否出现重复
                if (OnePeriodGenerateOrderNumberRecords.Contains(tmpOrderNumber))
                {
                    //一旦出现重复 ，那么从新生成
                    continue;
                }

                //确定在此时间段是唯一的流水号，记录到队列中
                OnePeriodGenerateOrderNumberRecords.Add(tmpOrderNumber);
                orderNumber = tmpOrderNumber;
            }

            return orderNumber;
        }

        /// <summary>
        /// 生成0-9的随机数字组合
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string GenerateRandomSequence(int length)
        {
            string result = string.Empty;

            //源数据长度
            int total = 10;


            int[] sequence = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            int[] output = new int[length];



            Random random = new Random();

            int end = total - 1;
            //随机数生成
            for (int i = 0; i < length; i++)
            {
                int num = random.Next(0, end + 1);
                output[i] = sequence[num];
                sequence[num] = sequence[end];//对源中的序列进行翻转倒置，保证随机性

                end--;

                if (end < 0)
                {
                    end = total - 1;
                }
            }

            result = string.Concat(output);


            return result;
        }


        #endregion




        /// <summary>
        /// 快速判断字符串起始部分
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool StartsWith(this string target, string lookfor)
        {
            if (string.IsNullOrEmpty(target) || string.IsNullOrEmpty(lookfor))
            {
                return false;
            }

            if (lookfor.Length > target.Length)
            {
                return false;
            }

            return (0 == string.Compare(target, 0, lookfor, 0, lookfor.Length));
        }

        /// <summary>
        /// 快速判断字符串起始部分
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool StartsWithIgnoreCase(this string target, string lookfor)
        {
            if (string.IsNullOrEmpty(target) || string.IsNullOrEmpty(lookfor))
            {
                return false;
            }

            if (lookfor.Length > target.Length)
            {
                return false;
            }
            return (0 == string.Compare(target, 0, lookfor, 0, lookfor.Length, StringComparison.CurrentCultureIgnoreCase));
        }


        public static bool EndsWith(this string target, string lookfor)
        {
            int indexA = target.Length - lookfor.Length;

            if (indexA < 0)
            {
                return false;
            }

            return (0 == string.Compare(target, indexA, lookfor, 0, lookfor.Length));
        }

        /// <summary>
        /// 快递判断字符串结束部分
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool EndsWithIgnoreCase(this string target, string lookfor)
        {
            int indexA = target.Length - lookfor.Length;

            if (indexA < 0)
            {
                return false;
            }

            return (0 == string.Compare(target, indexA, lookfor, 0, lookfor.Length, StringComparison.CurrentCultureIgnoreCase));
        }



        /// <summary>
        /// 截取指定长度字符串.后面补...
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CutString(this string text, int length)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (length < 1)
                return text;

            byte[] buf = Encoding.Default.GetBytes(text);

            if (buf.Length <= length)
            {
                return text;
            }

            int newLength = length;
            int[] numArray1 = new int[length];
            byte[] newBuf = null;
            int counter = 0;
            for (int i = 0; i < length; i++)
            {
                if (buf[i] > 0x7f)
                {
                    counter++;
                    if (counter == 3)
                    {
                        counter = 1;
                    }
                }
                else
                {
                    counter = 0;
                }
                numArray1[i] = counter;
            }

            if ((buf[length - 1] > 0x7f) && (numArray1[length - 1] == 1))
            {
                newLength = length + 1;
            }
            newBuf = new byte[newLength];
            Array.Copy(buf, newBuf, newLength);
            return Encoding.Default.GetString(newBuf) + "...";

        }



        public static string GetSafeFormText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            StringBuilder result = new StringBuilder(text);
            result.Replace("\"", "&quot;");
            result.Replace("<", "&lt;");
            result.Replace(">", "&gt;");

            return result.ToString();
        }


    


        /// <summary>
        /// 对字符串进行Html解码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string content)
        {
            return HttpUtility.HtmlDecode(content);
        }

        /// <summary>
        /// 对字符串进行Html编码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string content)
        {
            return HttpUtility.HtmlEncode(content);
        }


        /// <summary>
        /// <函数：Decode>
        ///作用：将16进制数据编码转化为字符串，是Encode的逆过程
        /// </summary>
        /// <param name="strDecode"></param>
        /// <returns></returns>
        public static string HexDecode(string strDecode)
        {
            if (strDecode.IndexOf(@"\u") == -1)
                return strDecode;

            int startIndex = 0;
            if (strDecode.StartsWith(@"\u") == false)
            {
                startIndex = 1;
            }

            string[] codes = Regex.Split(strDecode, @"\\u");

            StringBuilder result = new StringBuilder();
            if (startIndex == 1)
                result.Append(codes[0]);
            for (int i = startIndex; i < codes.Length; i++)
            {
                try
                {
                    if (codes[i].Length > 4)
                    {
                        result.Append((char)short.Parse(codes[i].Substring(0, 4), global::System.Globalization.NumberStyles.HexNumber));
                        result.Append(codes[i].Substring(4));
                    }
                    else
                    {
                        result.Append((char)short.Parse(codes[i].Substring(0, 4), global::System.Globalization.NumberStyles.HexNumber));
                    }
                }
                catch
                {
                    result.Append(codes[i]);
                }
            }

            return result.ToString();
        }




        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(this string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }






        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(this string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        ///  转半角的函数(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }





        /// <summary>
        /// 从字符串里随机得到，规定个数的字符串.
        /// </summary>
        /// <param name="allChar"></param>
        /// <param name="CodeCount"></param>
        /// <returns></returns>
        public static string GetRandomCode(this string allChar, int CodeCount)
        {
            //string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z"; 
            if (allChar.IndexOf(',') < 0)
            {
                throw new Exception("指定的字符串需要包含逗号分隔！");
            }
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(allCharArray.Length - 1);

                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }

                temp = t;
                RandomCode += allCharArray[t];
            }
            return RandomCode;
        }

        /// <summary>
        /// 将长整数转换成IP地址
        /// </summary>
        /// <param name="ipInt"></param>
        /// <returns></returns>
        public static string ConvertIntToIpString(this long ipInt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((ipInt >> 24) & 0xFF).Append(".");
            sb.Append((ipInt >> 16) & 0xFF).Append(".");
            sb.Append((ipInt >> 8) & 0xFF).Append(".");
            sb.Append(ipInt & 0xFF);
            return sb.ToString();
        }

        /// <summary>
        /// IP地址转换成长整数
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long ConvertIpStringToInt(this string ip)
        {


            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return long.Parse(items[0]) << 24
                    | long.Parse(items[1]) << 16
                    | long.Parse(items[2]) << 8
                    | long.Parse(items[3]);
        }


    }
}
