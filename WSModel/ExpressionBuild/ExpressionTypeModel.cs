#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExpressionTypeModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ExpressionTypeModel.cs 194095 2011-03-29 12:17:11Z ACHIEVO\hans.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.ExpressionBuild
{
    /// <summary>
    /// Expression type model
    /// </summary>
    [Serializable]
    public class ExpressionTypeModel
    {
        /// <summary>
        /// Gets or sets the PortLet id.
        /// </summary>
        public ExpressionType PortletID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the template view id
        /// </summary>
        public ExpressionType? TemplateViewID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the generic template view id.
        /// </summary>
        public ExpressionType? TPL_FormViewID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the generic template table view id.
        /// </summary>
        public ExpressionType? TPL_TableViewID
        {
            get;
            set;
        }
    }
}
