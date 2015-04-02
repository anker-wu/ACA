#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ASITColumnWrapperModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ASITColumnWrapperModel.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.ComponentService.Model
{
    /// <summary>
    /// The ASIT Column definition wrapper model.
    /// </summary>
    public class ASITColumnWrapperModel
    {
        /// <summary>
        /// WSDL model AppSpecificTableColumnModel4WS of AA service
        /// </summary>
        private AppSpecificTableColumnModel4WS asitTableCol;

        /// <summary>
        /// Initializes a new instance of the ASITColumnWrapperModel class.
        /// </summary>
        /// <param name="tableCol">WSDL model AppSpecificTableColumnModel4WS of AA service</param>
        internal ASITColumnWrapperModel(AppSpecificTableColumnModel4WS tableCol)
        {
            asitTableCol = tableCol;
        }

        /// <summary>
        /// Gets ASIT column name
        /// </summary>
        public string ColName
        {
            get
            {
                return I18nStringUtil.GetString(asitTableCol.resColumnName, asitTableCol.columnName);
            }
        }

        /// <summary>
        /// Gets ASIT column type
        /// </summary>
        public string ColType
        {
            get
            {
                return asitTableCol.columnType;
            }
        }

        /// <summary>
        /// Gets ASIT column index
        /// </summary>
        public int ColIndex
        {
            get
            {
                return int.Parse(asitTableCol.displayOrder);
            }
        }
    }
}
