#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ProductLicenseMessageInfo.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ProductLicenseMessageInfo.cs 158459 2009-11-25 09:56:33Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.Common
{
    /// <summary>
    /// Product License Message
    /// </summary>
    public class ProductLicenseMessageInfo
    {
        #region Properties

        /// <summary>
        /// Gets or sets the error message shown in admin site
        /// </summary>
        public string AdminMessage
        { 
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the error message shown in daily site
        /// </summary>
        public string DailyMessage
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether redirect to error page when it the expire or missing license.
        /// </summary>
        public bool IsRedirect
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the license days remains
        /// </summary>
        public int RemainDays
        {
            get;

            set;
        }

        #endregion Properties
    }
}
