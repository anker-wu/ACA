#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TimeZoneBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  This class deal with time zone conversion
 *
 *  Notes:
 * $Id: BllUtil.cs 277237 2014-08-13 01:17:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Text;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// This class provide the ability to operation business utility.
    /// </summary>
    internal static class BllUtil
    {
        #region Fields

        /// <summary>
        /// Query Format.
        /// </summary>
        private static readonly QueryFormat4WS QueryFormat = new QueryFormat4WS();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an initial QueryFormat4WS instance.
        /// This is a single instance for BLL to use when invoking web service to avoid creating a QueryFormat4WS instance each time.
        /// </summary>
        public static QueryFormat4WS EmptyQueryFormat
        {
            get
            {
                return QueryFormat;
            }
        }

        /// <summary>
        /// Get Audit model.
        /// </summary>
        /// <param name="publicUserId">public user id.</param>
        /// <returns>Audit model.</returns>
        public static AuditModel4WS GetAuditModel(string publicUserId)
        {
            AuditModel4WS auditModel = new AuditModel4WS();
            auditModel.auditID = publicUserId;
            auditModel.auditDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(DateTime.Now);
            auditModel.auditStatus = ACAConstant.VALID_STATUS;

            return auditModel;
        }

        /// <summary>
        /// add json key-value
        /// </summary>
        /// <param name="sb">string builder</param>
        /// <param name="key">string key.</param>
        /// <param name="value">string value.</param>
        public static void AddKeyValue(StringBuilder sb, string key, string value)
        {
            sb.AppendFormat("\"{0}\":\"{1}\",", key, value == null ? string.Empty : value.Replace("'", "\'").Replace("\"", "\\\"").Replace(@"\", @"\\"));
        }

        #endregion Properties
    }
}
