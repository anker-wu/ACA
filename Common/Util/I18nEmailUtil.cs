/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: I18nEmailUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  I18nEmailUtil for getting I18n Email information.
 *
 *  Notes:
 * $Id: I18nEmailUtil.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System.Text.RegularExpressions;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// provides I18n email to serve the framework. 
    /// </summary>
    public static class I18nEmailUtil
    {
        #region Properties

        /// <summary>
        /// Gets email regular expression for validation
        /// </summary>
        public static string EmailValidationExpression
        {
            get
            {
                return @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            }
        }

        /// <summary>
        /// Gets emails regular expression for validation
        /// </summary>
        public static string EmailsValidationExpression
        {
            get
            {
                return @"^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})(;[\w-\.]+@([\w-]+\.)+[\w-]{2,4})*$";
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Check Email Format
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns>true if it the email address format.</returns>
        public static bool CheckEmailFormat(string emailAddress)
        {
            bool isEmail = false;
            Regex reg = new Regex(EmailValidationExpression);

            if (emailAddress != null)
            {
                isEmail = reg.IsMatch(emailAddress);
            }

            return isEmail;
        }

        #endregion Methods
    }
}