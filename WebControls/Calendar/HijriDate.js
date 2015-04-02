/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: HijriDate.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 *
 * </pre>
 */

HijriDate.prototype = {
    year: null,
    month: null,
    day: null,
    hour: null,
    minute: null,
    second: null,
    gregorianDate: null,
    /* Create islamic date from gregorian date
     * @param gDate (Date) date for convert
     */
    create_date_from_gregorian_date: function (gDate) {
        this.gregorianDate = gDate;
        var gY = this.gregorianDate.getFullYear();
        var gM = this.gregorianDate.getMonth() + 1;
        var gD = this.gregorianDate.getDate();
        var gregorianJd = this.get_gregorian_JD(gY, gM, gD);
        var hijriJson = this.from_JD_to_hijri(gregorianJd);
        this.year = hijriJson.year;
        this.month = hijriJson.month;
        this.day = hijriJson.day;
        this.hour = this.gregorianDate.getHours();
        this.minute = this.gregorianDate.getMinutes();
        this.second = this.gregorianDate.getSeconds();
    },
    //Refresh gregorian date of islamic date
    refresh_gregorian_date: function () {
        this.gregorianDate = null;
        this.get_gregorian_date();
    },
    /* Get gregorian date of islamic date
     * @return (Date) Gregorain of hijridate
     */
    get_gregorian_date: function () {
        if (this.gregorianDate != null) {
            return this.gregorianDate;
        }

        var hijriJd = this.get_hijri_JD(this.year, this.month, this.day);
        var gregorianJson = this.from_JD_to_gregorian(hijriJd);
        this.gregorianDate = new Date(gregorianJson.year, gregorianJson.month - 1, gregorianJson.day);
        return this.gregorianDate;
    },
    //Calculates the islamic day, month and year from JD
    from_JD_to_hijri: function (jd) {
        jd = Math.floor(jd) + 0.5;
        var year = Math.floor((30 * (jd - 1948439.5) + 10646) / 10631);
        year = (year <= 0 ? year - 1 : year);
        var month = Math.min(12, Math.ceil((jd - 29 - this.get_hijri_JD(year, 1, 1)) / 29.5) + 1);
        var day = jd - this.get_hijri_JD(year, month, 1) + 1;
        return {
            year: year,
            month: month,
            day: day
        };
    },
    //Calculates the gregorian day, month and year from JD
    from_JD_to_gregorian: function (jd) {
        var z = Math.floor(jd + 0.5);
        var a = Math.floor((z - 1867216.25) / 36524.25);
        a = z + 1 + a - Math.floor(a / 4);
        var b = a + 1524;
        var c = Math.floor((b - 122.1) / 365.25);
        var d = Math.floor(365.25 * c);
        var e = Math.floor((b - d) / 30.6001);
        var day = b - d - Math.floor(e * 30.6001);
        var month = e - (e > 13.5 ? 13 : 1);
        var year = c - (month > 2.5 ? 4716 : 4715);

        if (year <= 0) {
            year--;
        }

        return {
            year: year,
            month: month,
            day: day
        };
    },
    // Gets JD of islamic date
    get_hijri_JD: function (year, month, day) {
        year = (year <= 0 ? year + 1 : year);
        return day + Math.ceil(29.5 * (month - 1)) + (year - 1) * 354 +
               Math.floor((3 + (11 * year)) / 30) + 1948438.5;
    },
    //Gets JD of gregorian date
    get_gregorian_JD: function (year, month, day) {
        if (year < 0) {
            year++;
        }

        if (month < 3) {
            month += 12;
            year--;
        }

        var a = Math.floor(year / 100);
        var b = 2 - a + Math.floor(a / 4);
        return Math.floor(365.25 * (year + 4716)) +
               Math.floor(30.6001 * (month + 1)) + day + b - 1524.5;
    },
    /* Format current date 
     * @param format (String) format of date
     * @return format text of date.
     */
    localeFormat: function (format) {
        if (this.day != 29 || this.month != 2 || this.year % 4 == 0) {
            var date1 = new Date(this.year, this.month - 1, this.day, this.hour, this.minute, this.second);
            return date1._toFormattedString(format, GetCalendarCultureInfo(true));
        }

        var date2 = new Date(2072, this.month - 1, this.day, this.hour, this.minute, this.second);
        var formatDateStr;

        if (format.indexOf("y") > 0) {
            format = format.replace("yyyy", "{1}");
            format = format.replace("yyy", "{2}");
            format = format.replace("yy", "{3}");
            format = format.replace("y", "{4}");
            formatDateStr = date2._toFormattedString(format, GetCalendarCultureInfo(true));
            formatDateStr = formatDateStr.replace("{1}", this.year);
            formatDateStr = formatDateStr.replace("{2}", this.year.toString().substr(1, 3));
            formatDateStr = formatDateStr.replace("{3}", this.year.toString().substr(2, 2));
            formatDateStr = formatDateStr.replace("{4}", this.year.toString().substr(3, 1));
        } else {
            formatDateStr = date2._toFormattedString(format, GetCalendarCultureInfo(true));
            formatDateStr = formatDateStr.replace(2072, this.year);
        }

        return formatDateStr;
    },
    /*Sets day value of islamic date
    * @param value (Number) day for setting
    */
    set_day: function (value) {
        this.day = value;
        this.refresh_gregorian_date();
    },
    /*Creates islamic date by year, month , day 
    * @param year (Number) year of date
    * @param month (Number) month of date
    * @param day (Number) day of date
    * @return (HijriDate) The hijridate created by year, month ,day
    */
    create_date: function (year, month, day) {
        var date = new HijriDate();
        date.day = day;
        var newMonth = month;

        if (newMonth >= 1 && newMonth <= 12) {
            date.year = year;
            date.month = newMonth;
        } else {
            var numberYears = Math.floor(newMonth / 12);
            var numberMonths = newMonth % 12;
            date.year = year + numberYears;
            if (newMonth > 0) {
                date.month = numberMonths;
            } else {
                date.month = 12 + numberMonths;
            }
        }

        date.refresh_gregorian_date();
        return date;
    }
};

AdjustDate.prototype = {
    //Is hijri date
    isHijriDate: false,
    //Cur date object
    curDate: null,
    /*Get day in week
    * @return current day in week.
    */
    getDay: function () {
        if (this.isHijriDate) {
            return this.curDate.gregorianDate.getDay();
        } else {
            return this.curDate.getDay();
        }
    },
    /*Get day of current date.
    * @param getGregorian (Boolean) whether get day of gregorian
    * @return day of current date.
    */
    getDate: function (getGregorian) {
        if (this.isHijriDate) {
            if (getGregorian) {
                return this.curDate.gregorianDate.getDate();
            } else {
                return this.curDate.day;
            }
        } else {
            return this.curDate.getDate();
        }
    },
    /*Get month of current date.
    * @param getGregorian (Boolean) whether get month of gregorian
    * @return month of current date.
    */
    getMonth: function (getGregorian) {
        if (this.isHijriDate) {
            if (getGregorian) {
                return this.curDate.gregorianDate.getMonth();
            } else {
                return this.curDate.month - 1;
            }
        } else {
            return this.curDate.getMonth();
        }
    },
    /*Get year of current date.
    * @param getGregorian (Boolean) whether get year of gregorian
    * @return year of current date.
    */
    getFullYear: function (getGregorian) {
        if (this.isHijriDate) {
            if (getGregorian) {
                return this.curDate.gregorianDate.getFullYear();
            } else {
                return this.curDate.year;
            }
        } else {
            return this.curDate.getFullYear();
        }
    },
    /* Format current date 
     * @param format (String) format of date
     * @return format text of date.
     */
    localeFormat: function (format) {
        return this.curDate.localeFormat(format);
    },
    /* Get format text of  gregorian date
     * @param format (String) format of date
     * @return format text of date.
     */
    getGregorianDateText: function (format) {
        if (this.isHijriDate) {
            return this.curDate.gregorianDate.localeFormat(format);
        } else {
            return this.localeFormat(format);
        }
    },
    /* Compare adjustdate
     * @param adjustDate (AdjustDate) adjustdate for compare
     * @return (Boolean) If current greater than adjustDate return true, else return false
     */
    compareDate: function (adjustDate) {
        var gregorianDate;

        if (adjustDate) {

            if (adjustDate.isHijriDate) {
                gregorianDate = adjustDate.curDate.gregorianDate;
            } else {
                gregorianDate = adjustDate.curDate;
            }
        } else {
            return false;
        }

        if (this.isHijriDate) {
            return this.curDate.gregorianDate > gregorianDate;
        } else {
            return this.curDate > gregorianDate;
        }
    }
};

/* New a islamic date with a gregorian date
 * @param gDate (Date) the gregorian date
 */
function HijriDate(gDate) {
    if (gDate)
        this.create_date_from_gregorian_date(gDate);
}

/* Creats a adjust date with special parmas.
 * @param isHijriDate (Boolean) is hijri date
 * @param gDate (Date) the gregorian date
 * @param year (Number) the year of date
 * @param month (Number) the month of date
 * @param day (Number) the day of date
 */
function AdjustDate(params) {
    this.isHijriDate = params.isHijriDate;

    if (params.gDate && params.gDate != null) {
        if (this.isHijriDate) {
            this.curDate = new HijriDate(params.gDate);
        } else {
            this.curDate = params.gDate;
        }
    } else {
        if (this.isHijriDate) {
            this.curDate = new HijriDate().create_date(params.year, params.month + 1, params.day);
        } else {
            this.curDate = new Date(params.year, params.month, params.day);
        }
    }
}

/* Gets culture info of calendar.
 * @param isHijriDate (Boolean) Current calendar is or not use hijri date.
 * @return (Sys.CultureInfo.CurrentCulture) if current calendar isn't hijri date, or current culture is "ar-AE" then
 * return Sys.CultureInfo.CurrentCulture, else reutrn "ar-AE" culture after builded.
 */
function GetCalendarCultureInfo(isHijriDate) {
    var currentCulture = Sys.CultureInfo.CurrentCulture;

    if (!isHijriDate || currentCulture.name == "ar-AE") {
        return currentCulture;
    } else {
        var cultureInfo = {
            "name": "ar-AE",
            "numberFormat": {
                "CurrencyDecimalDigits": 2,
                "CurrencyDecimalSeparator": ".",
                "IsReadOnly": false,
                "CurrencyGroupSizes": [3],
                "NumberGroupSizes": [3],
                "PercentGroupSizes": [3],
                "CurrencyGroupSeparator": ",",
                "CurrencySymbol": "د.إ.?",
                "NaNSymbol": "ليس برقم",
                "CurrencyNegativePattern": 3,
                "NumberNegativePattern": 3,
                "PercentPositivePattern": 0,
                "PercentNegativePattern": 0,
                "NegativeInfinitySymbol": "-لا نهاية",
                "NegativeSign": "-",
                "NumberDecimalDigits": 2,
                "NumberDecimalSeparator": ".",
                "NumberGroupSeparator": ",",
                "CurrencyPositivePattern": 2,
                "PositiveInfinitySymbol": "+لا نهاية",
                "PositiveSign": "+",
                "PercentDecimalDigits": 2,
                "PercentDecimalSeparator": ".",
                "PercentGroupSeparator": ",",
                "PercentSymbol": "%",
                "PerMilleSymbol": "‰",
                "NativeDigits": ["٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩"],
                "DigitSubstitution": 0
            },
            "dateTimeFormat": {
                "AMDesignator": "ص",
                "Calendar": {
                    "MinSupportedDateTime": "\/Date(-62135568000000)\/",
                    "MaxSupportedDateTime": "\/Date(253402300799999)\/",
                    "AlgorithmType": 1,
                    "CalendarType": 1,
                    "Eras": [1],
                    "TwoDigitYearMax": 2029,
                    "IsReadOnly": false
                },
                "DateSeparator": "/",
                "FirstDayOfWeek": 6,
                "CalendarWeekRule": 0,
                "FullDateTimePattern": "dd MMMM, yyyy hh:mm:ss tt",
                "LongDatePattern": "dd MMMM, yyyy",
                "LongTimePattern": "hh:mm:ss tt",
                "MonthDayPattern": "dd MMMM",
                "PMDesignator": "م",
                "RFC1123Pattern": "ddd, dd MMM yyyy HH\u0027:\u0027mm\u0027:\u0027ss \u0027GMT\u0027",
                "ShortDatePattern": "dd/MM/yyyy",
                "ShortTimePattern": "hh:mm tt",
                "SortableDateTimePattern": "yyyy\u0027-\u0027MM\u0027-\u0027dd\u0027T\u0027HH\u0027:\u0027mm\u0027:\u0027ss",
                "TimeSeparator": ":",
                "UniversalSortableDateTimePattern": "yyyy\u0027-\u0027MM\u0027-\u0027dd HH\u0027:\u0027mm\u0027:\u0027ss\u0027Z\u0027",
                "YearMonthPattern": "MMMM, yyyy",
                "AbbreviatedDayNames": ["الأحد", "الإثنين", "الثلاثاء", "الأربعاء", "الخميس", "الجمعة", "السبت"],
                "ShortestDayNames": ["ح", "ن", "ث", "ر", "خ", "ج", "س"],
                "DayNames": ["الأحد", "الإثنين", "الثلاثاء", "الأربعاء", "الخميس", "الجمعة", "السبت"],
                "AbbreviatedMonthNames": ["يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو", "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر", ""],
                "MonthNames": ["يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو", "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر", ""],
                "IsReadOnly": false,
                "NativeCalendarName": "التقويم الميلادي (تسمية إنجليزية)?",
                "AbbreviatedMonthGenitiveNames": ["يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو", "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر", ""],
                "MonthGenitiveNames": ["يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو", "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر", ""]
            },
            "eras": [1, "A.D.", null, 0]
        };

        return Sys.CultureInfo._parse(cultureInfo);
    }
}
