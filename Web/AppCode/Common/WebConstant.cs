#region Header

/**
 *  Accela Citizen Access
 *  File: WebConstant.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *  $Id: WebConstant.cs 132442 2009-05-26 05:27:19Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Provide a class to defined web constant.
    /// </summary>
    public static class WebConstant
    {
        /// <summary>
        /// Gets --select-- message.
        /// </summary>
        public static string DropDownDefaultText
        {
            get
            {
                return LabelUtil.GetGlobalTextByKey("aca_common_select");
            }
        }

        /// <summary>
        /// Gets common exception message.
        /// </summary>
        public static string ExceptionUtilDefaultValue
        {
            get
            {
                return LabelUtil.GetGlobalTextByKey("aca_common_technical_difficulty");
            }
        }
    }
}
