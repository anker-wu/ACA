#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: I18nNumberUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  I18nNumberUtil for getting I18n number information,such as currency, decimal separators, and other numeric symbols.
 *
 *  Notes:
 * $Id: I18nNumberUtil.cs 279268 2014-10-16 07:47:19Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Globalization;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// provides I18n number to serve the framework. 
    /// </summary>
    public static class I18nNumberUtil
    {
        #region Fields

        /// <summary>
        /// default money format
        /// </summary>
        public const string DEFAULT_MONEY_FORMAT = "#,0.00";

        /// <summary>
        /// service provider region info.
        /// </summary>
        private static RegionInfo _currencyRegionInfo = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the negative sign.
        /// </summary>
        public static string NegativeSign
        {
            get
            {
                return I18nCultureUtil.UserPreferredCultureInfo.NumberFormat.NegativeSign;
            }
        }

        /// <summary>
        /// Gets the number decimal separator.
        /// </summary>
        public static string NumberDecimalSeparator
        {
            get
            {
                return I18nCultureUtil.UserPreferredCultureInfo.NumberFormat.NumberDecimalSeparator;
            }
        }

        /// <summary>
        /// Gets global currency decimal separator
        /// </summary>
        /// <value>The currency decimal separator.</value>
        public static string CurrencyDecimalSeparator
        {
            get
            {
                return I18nCultureUtil.UserPreferredCultureInfo.NumberFormat.CurrencyDecimalSeparator;
            }
        }

        /// <summary>
        /// Gets Global Currency Group Separator
        /// </summary>
        /// <value>The currency group separator.</value>
        public static string CurrencyGroupSeparator
        {
            get
            {
                return I18nCultureUtil.UserPreferredCultureInfo.NumberFormat.CurrencyGroupSeparator;
            }
        }

        /// <summary>
        /// Gets Global Currency Symbol
        /// </summary>
        /// <value>The currency symbol.</value>
        public static string CurrencySymbol
        {
            get
            {
                string result = I18nSettingsProvider.GetI18nLocaleRelevantSettings().currencySymbol;

                if (!I18nCultureUtil.IsMultiLanguageEnabled && string.IsNullOrEmpty(result))
                {
                    result = CurrencyRegionInfo.CurrencySymbol;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the currency pattern.
        /// </summary>
        public static string CurrencyPattern
        {
            get
            {
                string result = string.Empty;
                double number = 1.01;
                string currencyString = FormatMoneyForUI(number);
                string numberString = currencyString.Replace(CurrencySymbol, string.Empty);
                result = currencyString.Replace(numberString.Trim(), "{0}");

                return result;
            }
        }

        /// <summary>
        /// Gets number regular expression for validation
        /// </summary>
        /// <value>The number validation expression.</value>
        public static string NumberValidationExpression
        {
            get
            {
                return @"^-?(([0-9]{{1,{0}}}\.[0-9]{{{1}}})|(0\.[0-9]{{{2}}}|0?\.0+)|(0))$";
            }
        }

        /// <summary>
        /// Gets currency region information
        /// </summary>
        /// <value>The service provider region info.</value>
        public static RegionInfo CurrencyRegionInfo
        {
            get
            {
                if (_currencyRegionInfo == null)
                {
                    _currencyRegionInfo = new RegionInfo(I18nCultureUtil.UserPreferredCulture);
                }

                return _currencyRegionInfo;
            }
        }

        /// <summary>
        /// Gets the I18n settings provider.
        /// </summary>
        /// <value>The I18n settings provider.</value>
        private static II18nSettingsProvider I18nSettingsProvider
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
        /// Formats the money for UI.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns>
        /// the formatted money
        /// </returns>
        public static string FormatMoneyForUI(decimal money)
        {
            string result = string.Empty;

            result = money.ToString("C", I18nCultureUtil.UserPreferredCultureInfo);
            result = result.Replace(I18nCultureUtil.UserPreferredCultureInfo.NumberFormat.CurrencySymbol, CurrencySymbol);

            return result;
        }

        /// <summary>
        /// Formats the money for UI.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns>
        /// the formatted money
        /// </returns>
        public static string FormatMoneyForUI(double money)
        {
            return FormatMoneyForUI((decimal)money);
        }

        /// <summary>
        /// Formats the money for UI.
        /// </summary>
        /// <param name="moneyObject">The money object, the value usually is of value Type, if the value is of string type, it should not include any culture-specific info.</param>
        /// <returns>the formatted money</returns>
        public static string FormatMoneyForUI(object moneyObject)
        {
            string result = string.Empty;
            bool isInvalid = moneyObject == null || (moneyObject is string && string.IsNullOrWhiteSpace(Convert.ToString(moneyObject)));

            if (!isInvalid)
            {
                double money = 0;

                if (double.TryParse(Convert.ToString(moneyObject, CultureInfo.InvariantCulture), NumberStyles.Currency, CultureInfo.InvariantCulture, out money))
                {
                    result = FormatMoneyForUI(money);
                }
            }

            return result;
        }

        /// <summary>
        /// Parses the number from web service.
        /// </summary>
        /// <param name="numberString">The number string.</param>
        /// <returns>the parsed number from web service.</returns>
        public static double ParseNumberFromWebService(string numberString)
        {
            double result = 0;
            double parsedResult = 0;

            if (TryParseNumberFromWebService(numberString, out parsedResult))
            {
                result = parsedResult;
            }

            return result;
        }

        /// <summary>
        /// Parses the money from web service.
        /// </summary>
        /// <param name="moneyString">The money string.</param>
        /// <returns>the parsed money from web service.</returns>
        public static double ParseMoneyFromWebService(string moneyString)
        {
            double result = 0;
            double parsedResult = 0;

            if (TryParseMoneyFromWebService(moneyString, out parsedResult))
            {
                result = parsedResult;
            }

            return result;
        }

        /// <summary>
        /// Tries the parse money from web service.
        /// </summary>
        /// <param name="moneyString">The money string.</param>
        /// <param name="money">The money.</param>
        /// <returns>true: parse successfully. false:parse failed.</returns>
        public static bool TryParseMoneyFromWebService(string moneyString, out double money)
        {
            bool result = false;
            double parsedMoney = 0;
            money = 0;

            moneyString = !string.IsNullOrWhiteSpace(moneyString) ? moneyString.Trim() : string.Empty;
            result = double.TryParse(moneyString, NumberStyles.Currency, CultureInfo.InvariantCulture, out parsedMoney);

            if (result)
            {
                money = parsedMoney;
            }

            return result;
        }

        /// <summary>
        /// Tries parsing number from input.
        /// </summary>
        /// <param name="numberString">The number string.</param>
        /// <param name="number">The number.</param>
        /// <returns>true: parse successfully. false:parse failed.</returns>
        public static bool TryParseNumberFromInput(string numberString, out double number)
        {
            bool result = false;
            number = default(double);
            double parsedResult = default(double);

            numberString = !string.IsNullOrWhiteSpace(numberString) ? numberString.Trim() : string.Empty;
            result = double.TryParse(numberString, out parsedResult);

            if (result)
            {
                number = parsedResult;
            }

            return result;
        }

        /// <summary>
        /// Tries the parse number from web service.
        /// </summary>
        /// <param name="numberString">The number string.</param>
        /// <param name="number">The number.</param>
        /// <returns>true: parse successfully. false:parse failed.</returns>
        public static bool TryParseNumberFromWebService(string numberString, out double number)
        {
            bool result = false;
            number = default(double);
            double parsedResult = default(double);

            numberString = !string.IsNullOrWhiteSpace(numberString) ? numberString.Trim() : string.Empty;
            result = double.TryParse(numberString, NumberStyles.Number, CultureInfo.InvariantCulture, out parsedResult);

            if (result)
            {
                number = parsedResult;
            }

            return result;
        }

        /// <summary>
        /// Convert the decimal from web service to input
        /// </summary>
        /// <param name="decimalString">decimal value</param>
        /// <returns>return convert result</returns>
        public static string ConvertDecimalFromWebServiceToInput(string decimalString)
        {
            if (string.IsNullOrWhiteSpace(decimalString))
            {
                return string.Empty;
            }

            string result = string.Empty;
            decimal parseResult;

            if (decimal.TryParse(decimalString, NumberStyles.Any, CultureInfo.InvariantCulture, out parseResult))
            {
                result = parseResult.ToString(CultureInfo.CurrentUICulture);
            }
           
            return result;
        }

        /// <summary>
        /// Convert the decimal for UI input.
        /// </summary>
        /// <param name="decimalString">decimal value</param>
        /// <returns>return convert result</returns>
        public static string ConvertDecimalForUI(string decimalString)
        {
            if (string.IsNullOrWhiteSpace(decimalString))
            {
                return string.Empty;
            }

            string result = string.Empty;
            decimal parseResult;

            if (decimal.TryParse(decimalString, NumberStyles.Any, CultureInfo.CurrentUICulture, out parseResult))
            {
                result = parseResult.ToString(CultureInfo.CurrentUICulture);
            }

            return result;
        }

        /// <summary>
        /// Converts the number from web service to input.
        /// </summary>
        /// <param name="numberString">The number string.</param>
        /// <returns>the number from web service to input.</returns>
        public static string ConvertNumberFromWebServiceToInput(string numberString)
        {
            string result = string.Empty;
            double parsedResult = 0;

            if (!string.IsNullOrWhiteSpace(numberString))
            {
                if (TryParseNumberFromWebService(numberString, out parsedResult))
                {
                    result = FormatNumberForInput(parsedResult);
                }
                else
                {
                    throw new Exception("Invalid number string.");
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the money from web service to input.
        /// </summary>
        /// <param name="moneyString">The money string.</param>
        /// <returns>the money from web service to input.</returns>
        public static string ConvertMoneyFromWebServiceToInput(string moneyString)
        {
            string result = string.Empty;
            double parsedResult = 0;

            if (!string.IsNullOrWhiteSpace(moneyString))
            {
                if (TryParseMoneyFromWebService(moneyString, out parsedResult))
                {
                    result = FormatMoneyForInput(parsedResult);
                }
                else
                {
                    throw new Exception("Invalid number string.");
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the money to invariant string.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns>the invariant money string</returns>
        public static string ConvertMoneyToInvariantString(double money)
        {
            return money.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the money to invariant string.
        /// </summary>
        /// <param name="moneyObject">The money object, the value usually is of value Type, if the value is of string type, it should not include any culture-specific info.</param>
        /// <returns>the invariant money string</returns>
        public static string ConvertMoneyToInvariantString(object moneyObject)
        {
            string result = string.Empty;
            bool isInvalid = moneyObject == null || (moneyObject is string && string.IsNullOrWhiteSpace(Convert.ToString(moneyObject)));

            if (!isInvalid)
            {
                double money = Convert.ToDouble(moneyObject, CultureInfo.InvariantCulture);
                result = ConvertMoneyToInvariantString(money);
            }

            return result;
        }

        /// <summary>
        /// Converts the number to invariant string.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>the number invariant string.</returns>
        public static string ConvertNumberToInvariantString(double number)
        {
            return number.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the number to invariant string.
        /// </summary>
        /// <param name="numberObject">The number object.</param>
        /// <returns>the number invariant string.</returns>
        public static string ConvertNumberToInvariantString(object numberObject)
        {
            string result = string.Empty;
            bool isInvalid = numberObject == null || (numberObject is string && string.IsNullOrWhiteSpace(Convert.ToString(numberObject)));

            if (!isInvalid)
            {
                double number = Convert.ToDouble(numberObject, CultureInfo.InvariantCulture);
                result = ConvertNumberToInvariantString(number);
            }

            return result;
        }

        /// <summary>
        /// Formats the number for web service.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>the number for web service.</returns>
        public static string FormatNumberForWebService(double number)
        {
            return number.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Formats the number for UI.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>the number for UI.</returns>
        public static string FormatNumberForUI(double number)
        {
            return number.ToString();
        }

        /// <summary>
        /// Formats the number for UI.
        /// </summary>
        /// <param name="numberObject">The number object.</param>
        /// <returns>the number for UI.</returns>
        public static string FormatNumberForUI(object numberObject)
        {
            string result = string.Empty;
            bool isInvalid = numberObject == null || (numberObject is string && string.IsNullOrWhiteSpace(Convert.ToString(numberObject)));

            if (!isInvalid)
            {
                double number = Convert.ToDouble(numberObject, CultureInfo.InvariantCulture);
                result = FormatNumberForUI(number);
            }

            return result;
        }

        /// <summary>
        /// Converts the number to input string.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>the number for input string.</returns>
        public static string FormatNumberForInput(double number)
        {
            return number.ToString();
        }

        /// <summary>
        /// Formats the money for input.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns>the money for input string</returns>
        public static string FormatMoneyForInput(double money)
        {
            return money.ToString();
        }

        /// <summary>
        /// Formats the number for input.
        /// </summary>
        /// <param name="numberObject">The number object.</param>
        /// <returns>the number for input.</returns>
        public static string FormatNumberForInput(object numberObject)
        {
            string result = string.Empty;
            bool isInvalid = numberObject == null || (numberObject is string && string.IsNullOrWhiteSpace(Convert.ToString(numberObject)));

            if (!isInvalid)
            {
                double number = Convert.ToDouble(numberObject, CultureInfo.InvariantCulture);
                result = FormatNumberForInput(number);
            }

            return result;
        }

        /// <summary>
        /// Converts the money string from web service to UI.
        /// </summary>
        /// <param name="moneyString">The money string.</param>
        /// <returns>the money string from web service to UI</returns>
        public static string ConvertMoneyStringFromWebServiceToUI(string moneyString)
        {
            string result = string.Empty;

            if (!string.IsNullOrWhiteSpace(moneyString))
            {
                double tempNumber = 0;
                bool isParsedOK = double.TryParse(moneyString, NumberStyles.Currency, CultureInfo.InvariantCulture, out tempNumber);

                if (isParsedOK)
                {
                    result = FormatMoneyForUI(tempNumber);
                }
                else
                {
                    throw new Exception("Invalid number string.");
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the number string from web service to UI.
        /// </summary>
        /// <param name="numberString">The number string.</param>
        /// <returns>the number string for UI</returns>
        public static string ConvertNumberStringFromWebServiceToUI(string numberString)
        {
            string result = string.Empty;

            if (!string.IsNullOrWhiteSpace(numberString))
            {
                double tempNumber = 0;
                bool isParsedOK = double.TryParse(numberString, NumberStyles.Number, CultureInfo.InvariantCulture, out tempNumber);

                if (isParsedOK)
                {
                    result = tempNumber.ToString();
                }
                else
                {
                    throw new Exception("Invalid number string.");
                }
            }

            return result;
        }

        /// <summary>
        /// Convert Number
        /// </summary>
        /// <param name="number">arab number.</param>
        /// <returns>return the culture number.</returns>
        public static string GetI18Number(int number)
        {
            if (!I18nCultureUtil.IsChineseCultureEnabled)
            {
                return number.ToString();
            }

            string result = string.Empty;

            switch (number)
            {
                case 0:
                    result = "\u96F6";
                    break;
                case 1:
                    result = "\u4E00";
                    break;
                case 2:
                    result = "\u4E8C";
                    break;
                case 3:
                    result = "\u4E09";
                    break;
                case 4:
                    result = "\u56DB";
                    break;
                case 5:
                    result = "\u4E94";
                    break;
                case 6:
                    result = "\u516D";
                    break;
                case 7:
                    result = "\u4E03";
                    break;
                case 8:
                    result = "\u516B";
                    break;
                case 9:
                    result = "\u4E5D";
                    break;
                case 10:
                    result = "\u5341";
                    break;
                case 11:
                    result = "\u5341\u4E00";
                    break;
                case 12:
                    result = "\u5341\u4E8C";
                    break;
                case 13:
                    result = "\u5341\u4E09";
                    break;
                case 14:
                    result = "\u5341\u56DB";
                    break;
                case 15:
                    result = "\u5341\u4E94";
                    break;
                case 16:
                    result = "\u5341\u516D";
                    break;
                case 17:
                    result = "\u5341\u4E03";
                    break;
                case 18:
                    result = "\u5341\u516B";
                    break;
                case 19:
                    result = "\u5341\u4E5D";
                    break;
                default:
                    result = number.ToString();
                    break;
            }

            return result;
        }

        #endregion Methods
    }
}