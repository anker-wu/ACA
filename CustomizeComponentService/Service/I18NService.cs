#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: I18NService.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: I18NService.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.Common.Util;
using Accela.ACA.CustomizeAPI;

namespace Accela.ACA.ComponentService.Service
{
    /// <summary>
    /// This class provide the ability to operate I18N configuration
    /// </summary>
    public class I18NService : BaseService
    {        
        /// <summary>
        /// Initializes a new instance of the I18NService class.
        /// </summary>
        /// <param name="context">User Context</param>
        public I18NService(UserContext context) : base(context)
        {
        }

        /// <summary>
        /// Get user preferred culture
        /// </summary>
        /// <returns>User Preferred Culture</returns>
        public string GetUserPreferredCulture()
        {
            return I18nCultureUtil.UserPreferredCulture;
        }
    }
}
