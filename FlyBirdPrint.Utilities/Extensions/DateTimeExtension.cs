using System;

namespace System
{
    public static class DateTimeExtension
    {

        /// <summary>
        /// 将时间转换成 yyyy-MM-dd HH:mm:ss 格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToOfenTimeString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 转化成时间戳-精确到毫秒
        /// </summary>
        /// <returns></returns>
        public static long ToTimeStampMilliseconds(this DateTime dt)
        {
            var dateTime = dt;// DateTime.Now;
            var dateTimeOffset = new DateTimeOffset(dateTime);
            var unixDateTime = dateTimeOffset.ToUnixTimeMilliseconds();
            return unixDateTime;
        }
        /// <summary>
        /// 转化成时间戳-精确到秒
        /// </summary>
        /// <returns></returns>
        public static long ToTimeStampSeconds(this DateTime dt)
        {
            var dateTime = dt;// DateTime.Now;
            var dateTimeOffset = new DateTimeOffset(dateTime);
            var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
            return unixDateTime;
        }
        /// <summary>
        /// 将精确到毫秒级别时间戳转化成时间
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static DateTime ConvertUnixTimeTokenToDateTime(this long timeStamp)
        {
            var time = DateTimeOffset.FromUnixTimeMilliseconds(timeStamp).DateTime.ToLocalTime();
            return time;
        }



        /// <summary>
        /// 把秒转换成分钟
        /// </summary>
        /// <returns></returns>
        public static int SecondToMinute(int Second)
        {
            decimal mm = (decimal)((decimal)Second / (decimal)60);
            return Convert.ToInt32(Math.Ceiling(mm));
        }

        #region 返回某年某月最后一天
        /// <summary>
        /// 返回某年某月最后一天
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>日</returns>
        public static int GetMonthLastDate(int year, int month)
        {
            DateTime lastDay = new DateTime(year, month, new System.Globalization.GregorianCalendar().GetDaysInMonth(year, month));
            int Day = lastDay.Day;
            return Day;
        }
        #endregion

        #region 返回时间差
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            try
            {
                //TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                //TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                //TimeSpan ts = ts1.Subtract(ts2).Duration();
                TimeSpan ts = DateTime2 - DateTime1;
                if (ts.Days >= 1)
                {
                    dateDiff = DateTime1.Month.ToString() + "月" + DateTime1.Day.ToString() + "日";
                }
                else
                {
                    if (ts.Hours > 1)
                    {
                        dateDiff = ts.Hours.ToString() + "小时前";
                    }
                    else
                    {
                        dateDiff = ts.Minutes.ToString() + "分钟前";
                    }
                }
            }
            catch
            { }
            return dateDiff;
        }
        #endregion
    }
}
