using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Utilities
{
    internal static class TimeHelper
    {
        /// <summary>
        /// 获取自定义时间显示样式的文本。
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string GetCustomTimeSpanString(this TimeSpan span)
        {
            string timeStr = span switch
            {
                { TotalMinutes: var m } when m <= 1 => span.ToString(@"ss\秒"),
                { TotalHours: var h } when h <= 1 => span.ToString(@"mm\分ss\秒"),
                { TotalDays: var d } when d <= 1 => span.ToString(@"hh\小\时mm\分"),
                { TotalDays: var d } when d>1 && d<2 => span.ToString(@"dd\天hh\小\时"),
                { TotalDays: var d } when d >= 2 => $"{d:F0}天",
                _ => throw new NotImplementedException()
            };
            return timeStr;
        }

        public static string GetClockStyleTimeSpanString(this TimeSpan span)
        {
            return span.ToString(@"hh\:mm\:ss");
        }

        /// <summary>
        /// 获取一个DateTimeOffset对象，其代表了<paramref name="dateTime"/>当天的起始时间（即当天0点）。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTimeOffset GetParitalDateTimeOffset(this DateTime dateTime) 
        {
            //使用TimeZoneInfo.Local.GetUtcOffset来获取本地时间和UTC的偏移
            return new DateTimeOffset(dateTime.Year,dateTime.Month,dateTime.Day,0,0,0,TimeZoneInfo.Local.GetUtcOffset(DateTime.Now));
        }

        /// <summary>
        /// 获取一个TimeSpan对象，其代表了<paramref name="dateTime"/>具体当天起始时间的距离。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static TimeSpan GetPartialTimeSpan(this DateTime dateTime)
        {
            return new TimeSpan(dateTime.Hour,dateTime.Minute,dateTime.Second);
        }

        /// <summary>
        /// 将<paramref name="offset"/>和<paramref name="partialTimeSpan"/>所代表的时间结合。
        /// 注意该方法与<see cref="GetParitalDateTimeOffset(DateTime)"/>和<see cref="GetPartialTimeSpan(DateTime)"/>对应。
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="partialTimeSpan"></param>
        /// <returns></returns>
        public static DateTime Combine(DateTimeOffset offset, TimeSpan partialTimeSpan)
        {
            DateTimeOffset dateTimeOffset = offset + partialTimeSpan;
            return new DateTime(dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, dateTimeOffset.Hour, dateTimeOffset.Month, dateTimeOffset.Second);
        }

        /// <summary>
        /// 获取<paramref name="dateTime"/>所代表时间的下一天的起始时间（即0点）。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetStartOfNextDay(this DateTime dateTime)
        {
            // 获取明天的日期
            DateTime tomorrow = dateTime.AddDays(1);
            // 获取明天的0点的对象
            DateTime endOfDay = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, 0, 0);
            return endOfDay;
        }
    }
}
