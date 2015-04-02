#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: HijriDateUtil.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: 
*
* </pre>
*/

#endregion

using System;
using System.Globalization;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// The class for Hijri date utility.
    /// </summary>
    public static class HijriDateUtil
    {
        /// <summary>
        ///  Culture info of Arabic.
        /// </summary>
        private static readonly CultureInfo ArCultureInfo = new CultureInfo("ar-AE");

        /// <summary>
        ///  Calendar of hijri
        /// </summary>
        private static readonly HijriCalendar HijriCalendar = new HijriCalendar();

        /// <summary>
        /// Initializes static members of the <see cref="HijriDateUtil"/> class.
        /// </summary>
        static HijriDateUtil()
        {
            ArCultureInfo.DateTimeFormat.Calendar = HijriCalendar;
        }

        /// <summary>
        /// Make normal date as hijri date
        /// </summary>
        /// <param name="dateStr">The normal of date</param>
        /// <returns>Islamic date</returns>
        public static string ToHijriDate(string dateStr)
        {
            if (string.IsNullOrEmpty(dateStr))
            {
                return dateStr;
            }

            try
            {
                var dateTime = I18nDateTimeUtil.ParseFromUI(dateStr);
                var jd = GetGregorianJD(dateTime);
                var hijriDate = FromJDtohijri(jd);

                return hijriDate.ToString(I18nDateTimeUtil.ShortDatePattern, ArCultureInfo.DateTimeFormat);
            }
            catch (FormatException)
            {
                return dateStr;
            }
        }

        /// <summary>
        /// Make hijri date as normal date
        /// </summary>
        /// <param name="dateStr">The Islamic of date</param>
        /// <returns>Gregorian date</returns>
        public static string ToGregorianDate(string dateStr)
        {
            if (string.IsNullOrEmpty(dateStr))
            {
                return dateStr;
            }

            try
            {
                var dateFormat = ArCultureInfo.DateTimeFormat;
                dateFormat.ShortDatePattern = I18nDateTimeUtil.I18nSettingsProvider.GetI18nLocaleRelevantSettings().dateFormat;
                dateFormat.AMDesignator = I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.AMDesignator;
                dateFormat.PMDesignator = I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.PMDesignator;
                
                var hijriDate = DateTime.Parse(dateStr, dateFormat);
                var jd = GetHijriJD(HijriCalendar.GetYear(hijriDate), HijriCalendar.GetMonth(hijriDate), HijriCalendar.GetDayOfMonth(hijriDate));
                var date = FromJDtogregorian(jd);
                return date.ToString(I18nDateTimeUtil.ShortDatePattern);
            }
            catch (FormatException)
            {
                return dateStr;
            }
        }

        /// <summary>
        /// Get JD of gregorian
        /// </summary>
        /// <param name="dt">The date of gregorian</param>
        /// <returns>JD of gregorian</returns>
        private static double GetGregorianJD(DateTime dt)
        {
            double year = dt.Year;
            double month = dt.Month;
            double day = dt.Day;

            if (year < 0)
            {
                year++;
            }

            if (month < 3)
            {
                month += 12;
                year--;
            }

            var a = Math.Floor(year / 100);
            var b = 2 - a + Math.Floor(a / 4);

            return Math.Floor(365.25 * (year + 4716.00)) + Math.Floor(30.6001 * (month + 1)) + day + b - 1524.5;
        }

        /// <summary>
        /// Calculates the islamic date, month and year from JD
        /// </summary>
        /// <param name="jd">The JD</param>
        /// <returns>Islamic date</returns>
        private static DateTime FromJDtohijri(double jd)
        {
            jd = Math.Floor(jd) + 0.5;
            var year = Math.Floor(((30 * (jd - 1948439.5)) + 10646) / 10631);
            year = year <= 0 ? year - 1 : year;
            var month = Math.Min(12, Math.Ceiling((jd - 29 - GetHijriJD(year, 1, 1)) / 29.5) + 1);
            var day = jd - GetHijriJD(year, month, 1) + 1;

            return new DateTime((int)year, (int)month, (int)day, HijriCalendar);
        }

        /// <summary>
        ///  Gets JD of islamic date
        /// </summary>
        /// <param name="year">year of islamic date</param>
        /// <param name="month">month of islamic date</param>
        /// <param name="day">day of islamic date</param>
        /// <returns>JD of islamic date</returns>
        private static double GetHijriJD(double year, double month, double day)
        {
            year = year <= 0 ? year + 1 : year;
            return day + Math.Ceiling(29.5 * (month - 1)) + ((year - 1) * 354) + Math.Floor((3 + (11 * year)) / 30) + 1948438.5;
        }

        /// <summary>
        /// Calculates the gregorian date, month and year from JD
        /// </summary>
        /// <param name="jd">The JD</param>
        /// <returns>Gregorian date</returns>
        private static DateTime FromJDtogregorian(double jd)
        {
            var z = Math.Floor(jd + 0.5);
            var a = Math.Floor((z - 1867216.25) / 36524.25);
            a = z + 1 + a - Math.Floor(a / 4);
            var b = a + 1524;
            var c = Math.Floor((b - 122.1) / 365.25);
            var d = Math.Floor(365.25 * c);
            var e = Math.Floor((b - d) / 30.6001);
            var day = b - d - Math.Floor(e * 30.6001);
            var month = e - (e > 13.5 ? 13 : 1);
            var year = c - (month > 2.5 ? 4716 : 4715);

            if (year <= 0)
            {
                year--;
            }

            return new DateTime((int)year, (int)month, (int)day);
        }
    }
}