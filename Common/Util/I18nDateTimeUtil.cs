#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: I18nDateTimeUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  I18nDateTimeUtil for getting I18n datetime information.
 *
 *  Notes:
 * $Id: I18nDateTimeUtil.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Globalization;
using Accela.ACA.Inspection;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// provides I18n utility to serve the framework. 
    /// </summary>
    public static class I18nDateTimeUtil
    {
        #region Fields

        /// <summary>
        /// the morning
        /// </summary>
        private const string AM = "AM";

        /// <summary>
        /// the afternoon
        /// </summary>
        private const string PM = "PM";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the customized date time format info.
        /// </summary>
        /// <value>The customized date time format info.</value>
        public static DateTimeFormatInfo CustomizedDateTimeFormatInfo
        {
            get
            {
                string dateFormat = I18nSettingsProvider.GetI18nLocaleRelevantSettings().dateFormat;
                DateTimeFormatInfo result = new DateTimeFormatInfo();
                result.ShortDatePattern = dateFormat;
                result.AMDesignator = I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.AMDesignator;
                result.PMDesignator = I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.PMDesignator;

                return result;
            }
        }

        /// <summary>
        /// Gets the web service date time format info.
        /// </summary>
        /// <value>The web service date time format info.</value>
        public static DateTimeFormatInfo WebServiceDateTimeFormatInfo
        {
            get
            {
                DateTimeFormatInfo result = new DateTimeFormatInfo();
                result.ShortDatePattern = ACAConstant.DATE_FORMAT;

                return result;
            }
        }

        /// <summary>
        /// Gets Long date pattern, such as <c>dddd, MMMM dd, yyyy</c>, usually used for UI language switch
        /// </summary>
        public static string LongDatePattern
        {
            get
            {
                string result = I18nSettingsProvider.GetI18nLocaleRelevantSettings().longDateFormat;

                if (!I18nCultureUtil.IsMultiLanguageEnabled && string.IsNullOrEmpty(result))
                {
                    result = ACAConstant.LONG_DATE_FORMAT;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets Long Time Mask, such as 99:99:99 ,usually used for UI language switch
        /// </summary>
        public static string LongTimeMask
        {
            get
            {
                string separator = I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.TimeSeparator;
                return LongTimePattern.ToLowerInvariant().Replace(separator, ":").Replace("h", "9").Replace("m", "9").Replace("s", "9");
            }
        } 

        /// <summary>
        /// Gets Long time pattern, such as <c>hh:mm:ss</c>, usually used for UI language switch
        /// </summary>
        public static string LongTimePattern
        {
            get
            {
                return "HH:mm:ss";
            }
        }

        /// <summary>
        /// Gets Short Time Mask, such as 99:99 ,usually used for UI language switch
        /// </summary>
        public static string ShortTimeMask
        {
            get
            {
                string result = string.Empty;

                if (!string.IsNullOrEmpty(ShortTimePattern))
                {
                    string datePattern = ShortTimePattern.Replace(" ", string.Empty).ToLowerInvariant();
                    result = datePattern.Replace("h", "9").Replace("m", "9").Replace("s", "9").Replace("t", string.Empty);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets Short Date Time Mask, such as 99/99/9999 ,usually used for UI language switch
        /// </summary>
        public static string ShortDateMask
        {
            get
            {
                string result = string.Empty;

                if (!string.IsNullOrEmpty(ShortDatePattern))
                {
                    string datePattern = ShortDatePattern.Replace(" ", string.Empty).ToLowerInvariant();
                    result = datePattern.Replace("y", "9").Replace("m", "9").Replace("d", "9");
                }

                return result;
            }
        }

        /// <summary>
        /// Gets date time pattern ,such as <c>MM/dd/yyyy HH:mm:ss</c>
        /// </summary>
        /// <value>The date time pattern.</value>
        public static string DateTimePattern
        {
            get
            {
                string result = I18nSettingsProvider.GetI18nLocaleRelevantSettings().dateTimeFormat;

                if (!I18nCultureUtil.IsMultiLanguageEnabled && string.IsNullOrEmpty(result))
                {
                    result = string.Concat(I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.ShortDatePattern, " ", ShortTimePattern);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the inspection date time pattern.
        /// </summary>
        /// <value>The inspection date time pattern.</value>
        public static string InspectionDateTimePattern
        {
            get
            {
                string result = I18nSettingsProvider.GetI18nLocaleRelevantSettings().inspectionDateTimeFormat;

                if (!I18nCultureUtil.IsMultiLanguageEnabled && string.IsNullOrEmpty(result))
                {
                    result = string.Concat(I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.ShortDatePattern, " ", InspectionShortTimePattern);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets Short date pattern ,such as <c>MM/dd/yyyy</c>, usually used for UI language switch
        /// </summary>
        /// <value>The short date pattern.</value>
        public static string ShortDatePattern
        {
            get
            {
                string result = I18nSettingsProvider.GetI18nLocaleRelevantSettings().dateFormat;

                if (!I18nCultureUtil.IsMultiLanguageEnabled && string.IsNullOrEmpty(result))
                {
                    string shortDatePattern = I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.ShortDatePattern;

                    if (shortDatePattern.IndexOf("MM", StringComparison.InvariantCulture) == -1)
                    {
                        shortDatePattern = shortDatePattern.Replace("M", "MM");
                    }

                    if (shortDatePattern.IndexOf("dd", StringComparison.InvariantCulture) == -1)
                    {
                        shortDatePattern = shortDatePattern.Replace("d", "dd");
                    }

                    result = shortDatePattern; //same to "d" pattern
                }

                return result;
            }
        }

        /// <summary>
        /// Gets Short time pattern, such as <c>hh:mm</c>, usually used for UI language switch
        /// </summary>
        public static string ShortTimePattern
        {
            get
            {
                string result = I18nSettingsProvider.GetI18nLocaleRelevantSettings().timeFormat;

                if (!I18nCultureUtil.IsMultiLanguageEnabled && string.IsNullOrEmpty(result))
                {
                    result = I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.ShortTimePattern;
                }

                if (!string.IsNullOrEmpty(result) && result.IndexOf("hh", StringComparison.InvariantCultureIgnoreCase) == -1)
                {
                    result = result.Replace("h", "hh");
                    result = result.Replace("H", "HH");
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the short inspection time pattern.
        /// </summary>
        /// <value>The short inspection time pattern.</value>
        public static string InspectionShortTimePattern
        {
            get
            {
                string result = I18nSettingsProvider.GetI18nLocaleRelevantSettings().inspectionTimeFormat;

                if (!I18nCultureUtil.IsMultiLanguageEnabled && string.IsNullOrEmpty(result))
                {
                    result = I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.ShortTimePattern;
                }

                if (!string.IsNullOrEmpty(result) && result.IndexOf("hh", StringComparison.InvariantCultureIgnoreCase) == -1)
                {
                    result = result.Replace("h", "hh");
                    result = result.Replace("H", "HH");
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the I18n settings provider.
        /// </summary>
        /// <value>The I18n settings provider.</value>
        public static II18nSettingsProvider I18nSettingsProvider
        {
            get
            {
                II18nSettingsProvider result = ObjectFactory.GetObject(typeof(II18nSettingsProvider)) as II18nSettingsProvider;

                return result;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Convert a date string from UI to web service
        /// </summary>
        /// <param name="dateString">data string</param>
        /// <returns>format data string.</returns>
        public static string ConvertDateStringFromUIToWebService(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return string.Empty;
            }

            return FormatToDateStringForWebService(ParseFromUI(dateString));
        }

        /// <summary>
        /// Convert a date string from web service to UI
        /// </summary>
        /// <param name="dateString">data string</param>
        /// <returns>format data string.</returns>
        public static string ConvertDateStringFromWebServiceToUI(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return string.Empty;
            }

            string formatDate = dateString;
            DateTime tryResult = new DateTime();

            bool isDate = TryParseFromWebService(dateString, out tryResult);

            if (isDate)
            {
                formatDate = FormatToDateStringForUI(tryResult);    
            }
            
            return formatDate;
        }

        /// <summary>
        /// Convert a datetime string from UI to web service
        /// </summary>
        /// <param name="dateTimeString">data string</param>
        /// <returns>format data string.</returns>
        public static string ConvertDateTimeStringFromUIToWebService(string dateTimeString)
        {
            if (string.IsNullOrEmpty(dateTimeString))
            {
                return string.Empty;
            }

            return FormatToDateTimeStringForWebService(ParseFromUI(dateTimeString));
        }

        /// <summary>
        /// Convert a datetime string from UI to web service
        /// </summary>
        /// <param name="dateTime">date time string</param>
        /// <returns>format data string.</returns>
        public static string ConvertDateTimeStringFromUIToWebService(object dateTime)
        {
            string result = string.Empty;

            if (dateTime != null && dateTime is DateTime)
            {
                result = FormatToDateTimeStringForWebService((DateTime)dateTime);
            }

            return result;
        }

        /// <summary>
        /// Convert a datetime string from web service to UI
        /// </summary>
        /// <param name="dateTimeString">data string</param>
        /// <returns>format data string.</returns>
        public static string ConvertDateTimeStringFromWebServiceToUI(string dateTimeString)
        {
            if (string.IsNullOrEmpty(dateTimeString))
            {
                return string.Empty;
            }

            return FormatToDateTimeStringForUI(ParseFromWebService(dateTimeString));
        }

        /// <summary>
        /// Convert time string from web service to UI.
        /// </summary>
        /// <param name="timeString">time string.</param>
        /// <param name="isInspection">if set to <c>true</c> [is inspection].</param>
        /// <returns>format time string.</returns>
        public static string ConvertTimeStringFromWebServiceToUI(string timeString, bool isInspection)
        {
            if (string.IsNullOrEmpty(timeString))
            {
                return string.Empty;
            }

            bool parseResult = false;
            DateTime resultDateTime;
            parseResult = DateTime.TryParse(timeString, WebServiceDateTimeFormatInfo, DateTimeStyles.None, out resultDateTime);

            if (parseResult)
            {
                string timePattern = isInspection ? InspectionShortTimePattern : ShortTimePattern;
                return resultDateTime.ToString(timePattern, CustomizedDateTimeFormatInfo);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Formats the time string for web service.
        /// e.g. format string "am" to "12:00 AM"
        /// </summary>
        /// <param name="timeString">The time string with web service format</param>
        /// <param name="isInspection">if set to <c>true</c> [is inspection].</param>
        /// <returns>
        /// the formatted time string with web service format
        /// </returns>
        public static string FormatTimeStringForWebService(string timeString, bool isInspection)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(timeString))
            {
                bool parseResult = false;
                DateTime resultDateTime;
                parseResult = DateTime.TryParse(timeString, WebServiceDateTimeFormatInfo, DateTimeStyles.None, out resultDateTime);

                if (parseResult)
                {
                    string timePattern = isInspection ? InspectionShortTimePattern : ShortTimePattern;
                    result = resultDateTime.ToString(timePattern, WebServiceDateTimeFormatInfo);
                }
            }

            return result;
        }

        /// <summary>
        /// Format a datetime instance or a string with en-US pattern or DBNull.Value to date string for UI.
        /// </summary>
        /// <param name="dateTime">date time info.</param>
        /// <returns>format date time.</returns>
        public static string FormatToDateStringForUI(object dateTime)
        {
            string result = string.Empty;

            if (dateTime is DateTime)
            {
                result = FormatToDateStringForUI((DateTime)dateTime);
            }
            else if (dateTime is string)
            {
                result = ConvertDateStringFromWebServiceToUI(dateTime as string);
            }

            return result;
        }

        /// <summary>
        /// Format a datetime instance to date string for UI.
        /// </summary>
        /// <param name="dateTime">date time info.</param>
        /// <returns>format date time.</returns>
        public static string FormatToDateStringForUI(DateTime dateTime)
        {
            return dateTime.ToString(ShortDatePattern, CustomizedDateTimeFormatInfo);
        }

        /// <summary>
        /// Format a datetime instance to long date string for UI.
        /// </summary>
        /// <param name="dateTime">date time value</param>
        /// <returns>formatted date time string</returns>
        public static string FormatToLongDateStringForUI(DateTime dateTime)
        {
            return dateTime.ToString(LongDatePattern);
        }

        /// <summary>
        /// Format a datetime instance or DBNull.Value to date string for web service.
        /// </summary>
        /// <param name="dateTime">date time info.</param>
        /// <returns>format date time</returns>
        public static string FormatToDateStringForWebService(object dateTime)
        {
            string result = string.Empty;

            if (dateTime is DateTime)
            {
                result = FormatToDateStringForWebService((DateTime)dateTime);
            }
            else if (dateTime is string)
            {
                result = ConvertDateStringFromUIToWebService(dateTime as string);
            }

            return result;
        }

        /// <summary>
        /// Format a datetime instance to date string for web service.
        /// </summary>
        /// <param name="dateTime">date time info.</param>
        /// <returns>format date time</returns>
        public static string FormatToDateStringForWebService(DateTime dateTime)
        {
            return dateTime.ToString(ACAConstant.DATE_FORMAT, WebServiceDateTimeFormatInfo);
        }

        /// <summary>
        /// Format a datetime instance to datetime string for UI.
        /// </summary>
        /// <param name="dateTime">date time info.</param>
        /// <returns>format date time.</returns>
        public static string FormatToDateTimeStringForUI(DateTime dateTime)
        {
            return dateTime.ToString(DateTimePattern, CustomizedDateTimeFormatInfo);
        }

        /// <summary>
        /// Formats to inspection date time string for UI.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>inspection date time string for UI.</returns>
        public static string FormatToInspectionDateTimeStringForUI(DateTime dateTime)
        {
            return dateTime.ToString(InspectionDateTimePattern, CustomizedDateTimeFormatInfo);
        }

        /// <summary>
        /// Format a datetime instance to datetime string for web service.
        /// </summary>
        /// <param name="dateTime">date time info.</param>
        /// <returns>format date time.</returns>
        public static string FormatToDateTimeStringForWebService(DateTime dateTime)
        {
            return dateTime.ToString(ACAConstant.DATATIME_FORMAT, WebServiceDateTimeFormatInfo);
        }
        
        /// <summary>
        /// Format a datetime instance to time string for web service.
        /// </summary>
        /// <param name="dateTime">date time info.</param>
        /// <returns>format date time.</returns>
        public static string FormatToTimeStringForWebService(DateTime dateTime)
        {
            return dateTime.ToString(ACAConstant.TIME_FORMAT, WebServiceDateTimeFormatInfo);
        }

        /// <summary>
        /// Format a datetime instance to time string for UI.
        /// </summary>
        /// <param name="dateTime">date time info.</param>
        /// <param name="isInspection">if set to <c>true</c> [is inspection].</param>
        /// <returns>format date time.</returns>
        public static string FormatToTimeStringForUI(DateTime dateTime, bool isInspection)
        {
            string timePattern = isInspection ? InspectionShortTimePattern : ShortTimePattern;
            return dateTime.ToString(timePattern, CustomizedDateTimeFormatInfo);
        }

        /// <summary>
        /// Parse datetime string From UI according to current culture.
        /// </summary>
        /// <param name="s">date time string.</param>
        /// <returns>format date time info.</returns>
        public static DateTime ParseFromUI(string s)
        {
            return DateTime.Parse(s, CustomizedDateTimeFormatInfo);
        }

        /// <summary>
        /// Parses from UI.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The datetime parse from UI.</returns>
        public static object ParseFromUI(object s)
        {
            DateTime dateTime;
            if (s is DateTime)
            {
                if ((DateTime)s != DateTime.MinValue)
                {
                    return s;
                }
            }
            else if (DateTime.TryParse(s.ToString(), CustomizedDateTimeFormatInfo, DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }

            return string.Empty;
        }

        /// <summary>
        /// Parse datetime string according to the convention from web service.
        /// the datetime parameters/values send to or received from web service always use en-US culture information.
        /// </summary>
        /// <param name="s">date time string.</param>
        /// <returns>format date time info.</returns>
        public static DateTime ParseFromWebService(string s)
        {
            return DateTime.Parse(s, I18nCultureUtil.WebServiceCultureInfo);
        }

        /// <summary>
        /// Parse datetime string for data table.
        /// </summary>
        /// <param name="s">date time string</param>
        /// <returns>if s is null or empty, then return DBNull.Value, otherwise return parsed date time value.</returns>
        public static object ParseFromWebService4DataTable(string s)
        {
            object result = string.IsNullOrEmpty(s) ? DBNull.Value : (object)ParseFromWebService(s);
            return result;
        }

        /// <summary>
        /// Try to parse datetime string from UI according to current culture.
        /// </summary>
        /// <param name="s">date time string.</param>
        /// <param name="result">parse result date time.</param>
        /// <returns>format date time info.</returns>
        public static bool TryParseFromUI(string s, out DateTime result)
        {
            return DateTime.TryParse(s, CustomizedDateTimeFormatInfo, DateTimeStyles.None, out result);
        }

        /// <summary>
        /// Try to parse datetime string according to the convention from web service.
        /// the datetime parameters/values send to or received from web service always use en-US culture information.
        /// </summary>
        /// <param name="s">date time string.</param>
        /// <param name="result">parse result date time.</param>
        /// <returns>format date time info.</returns>
        public static bool TryParseFromWebService(string s, out DateTime result)
        {
            return DateTime.TryParse(s, I18nCultureUtil.WebServiceCultureInfo, DateTimeStyles.None, out result);
        }

        /// <summary>
        /// Gets now's millisecond with <c>yyyyMMddHHmmss</c>.
        /// </summary>
        /// <returns><c>yyyyMMddHHmmss</c> + millisecond</returns>
        public static string GetMilliSecond()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
        }

        /// <summary>
        /// Gets a value indicating whether current time is using 12-hour format.
        /// </summary>
        /// <param name="isInspection">indicate whether it used to inspection.</param>
        /// <returns><c>true</c> if current time is using 12-hour format; otherwise, <c>false</c>.</returns>
        public static bool IsUsing12HourFormat(bool isInspection)
        {
            string timePattern = isInspection ? InspectionShortTimePattern : ShortTimePattern;
            bool result = timePattern.IndexOf("h", 0, StringComparison.Ordinal) != -1;

            return result;
        }

        /// <summary>
        /// Get the inspection date.
        /// </summary>
        /// <param name="date">the Date.</param>
        /// <param name="time">the time string.</param>
        /// <param name="ampm">the am/pm string</param>
        /// <param name="timeOption">the time option.</param>
        /// <returns>Return Date + time + am/pm</returns>
        public static DateTime? GetInspectionDate(DateTime? date, string time, string ampm, out InspectionTimeOption timeOption)
        {
            DateTime? result = null;
            timeOption = InspectionTimeOption.Unknow;

            if (date.HasValue)
            {
                string theDateString = FormatToDateStringForWebService(date.Value);
                string theTimeString = string.IsNullOrWhiteSpace(time) ? string.Empty : time;
                string theAmPmString = string.IsNullOrWhiteSpace(ampm) ? string.Empty : ampm;
                string parsingString = string.Empty;
                DateTime parsingResult = DateTime.Now;
                bool parsingSucceeded = false;

                //try to parse "date" + "time" + "am/pm"
                if (!parsingSucceeded && !string.IsNullOrWhiteSpace(theDateString) && !string.IsNullOrWhiteSpace(theTimeString) && !string.IsNullOrWhiteSpace(theAmPmString))
                {
                    parsingString = string.Format("{0} {1} {2}", theDateString, theTimeString, theAmPmString);
                    parsingSucceeded = TryParseFromWebService(parsingString, out parsingResult);
                    timeOption = parsingSucceeded ? InspectionTimeOption.SpecificTime : timeOption;
                }

                //try to parse "date" + "am/pm"
                if (!parsingSucceeded && !string.IsNullOrWhiteSpace(theDateString) && !string.IsNullOrWhiteSpace(theAmPmString))
                {
                    parsingString = string.Format("{0} {1}", theDateString, theAmPmString);
                    parsingSucceeded = TryParseFromWebService(parsingString, out parsingResult);

                    if (parsingSucceeded && AM.Equals(theAmPmString, StringComparison.OrdinalIgnoreCase))
                    {
                        timeOption = InspectionTimeOption.AM;
                    }
                    else if (parsingSucceeded && PM.Equals(theAmPmString, StringComparison.OrdinalIgnoreCase))
                    {
                        timeOption = InspectionTimeOption.PM;
                    }
                }

                //try to parse "date"
                if (!parsingSucceeded && !string.IsNullOrWhiteSpace(theDateString))
                {
                    parsingSucceeded = TryParseFromWebService(theDateString, out parsingResult);
                    timeOption = parsingSucceeded ? InspectionTimeOption.AllDay : timeOption;
                }

                result = parsingSucceeded ? parsingResult : result;
            }

            return result;
        }

        #endregion Methods
    }
}