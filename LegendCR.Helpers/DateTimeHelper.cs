using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LegendCR.Helpers
{
   public class DateTimeHelper
    {
        public const string ServerDateFormat = "dd/MM/yyyy";

        public const string ServerDateTimeFormat = "dd/MM/yyyy HH:mm";
        public static string ToArabicDate(DateTime dateStr)
        {
            return dateStr.ToString("dd, MMMM, yyyy", new CultureInfo("ar-AE"));
        }
        public static string ToDate(DateTime dateStr)
        {
            return dateStr.ToString("dd, MMMM, yyyy", new CultureInfo("ar-AE"));
        }

        public static DateTime ToDateDD_MM_YYYY(string dateStr)
        {
            DateTime dt;
            TryParseToDateDD_MM_YYYY(dateStr, out dt);
            return dt;
        }
        public static bool TryParseToDateDD_MM_YYYY(string dd_MM_yyyy, out DateTime dt)
        {
            var vals = dd_MM_yyyy.Trim().Split(new char[] { '-', '/', '_', '\\' });
            try
            {
                if (!string.IsNullOrWhiteSpace(dd_MM_yyyy))
                    dt = new DateTime(int.Parse(vals[2].Trim()), int.Parse(vals[0].Trim()), int.Parse(vals[1].Trim()));
                else
                    dt = DateTime.Today;

            }
            catch
            {
                dt = new DateTime(int.Parse(vals[2].Trim()), int.Parse(vals[0].Trim()), int.Parse(vals[1].Trim()));
            }
            return true;
        }
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            return GetFirstDayOfWeek(dayInWeek, defaultCultureInfo);
        }
        public static int DifferenceDate(DateTime dateTime)
        {
            TimeSpan difference = DateTime.Now - dateTime;
            return difference.Days;
        }
        /// <summary>
        /// Generate date format for validation
        /// </summary>
        public static string MinDate()
        {
            var Date = DateTime.Now.ToShortDateString().Split('/');
            var MinDate = Date[2] + "-";
            MinDate += int.Parse(Date[0]) > 9 ? Date[0] : "0" + Date[0] + "-";
            MinDate += int.Parse(Date[1]) > 9 ? Date[1] : "0" + Date[1];
            return MinDate;
        }
        /// <summary>
        /// Generate date format for validation
        /// </summary>
        /// /// <param name="Today">If you don't want to show today too</param>
        public static string MinDate(bool Today)
        {
            if (Today)
            {
                var Date = DateTime.Now.AddDays(1).ToShortDateString().Split('/');
                var MinDate = Date[2] + "-";
                MinDate += int.Parse(Date[0]) > 9 ? Date[0] + "-" : "0" + Date[0] + "-";
                MinDate += int.Parse(Date[1]) > 9 ? Date[1] : "0" + Date[1];
                return MinDate;
            }
            else
                return MinDate();
        }
        public static string InputDateFormat(string date)
        {
            var Date = date.Split('/');
            var MinDate = Date[2] + "-";
            MinDate += int.Parse(Date[0]) > 9 ? Date[0] : "0" + Date[0] + "-";
            MinDate += int.Parse(Date[1]) > 9 ? Date[1] : "0" + Date[1];
            return MinDate;
        }
        public static string InputTimeFormat(string time)
        {
            var Time = time.Split(':');
            var InputTimeFormat = int.Parse(Time[0]) > 9 ? Time[0] + ":" : "0" + Time[0] + ":";
            //  InputTimeFormat += int.Parse(Time[1].Remove(2)) > 9 ? Time[1].Remove(2) : "0" + Time[1].Remove(2);
            InputTimeFormat += Time[1].Remove(2);
            return InputTimeFormat;
        }
    }
}
