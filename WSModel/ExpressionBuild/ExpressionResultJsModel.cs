#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExpressionResultJsModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ExpressionResultJsModel.cs 194095 2011-03-29 12:17:11Z ACHIEVO\hans.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.ExpressionBuild
{
    /// <summary>
    /// the expression JavaScript model 
    /// </summary>
    [Serializable]
    public class ExpressionResultJsModel
    {
        /// <summary>
        /// Gets or sets the expression result control clientID.
        /// </summary>
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the expression result control required.
        /// </summary>
        public string required
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the expression result control readonly.
        /// </summary>
        public string readOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the expression result control message.
        /// </summary>
        public string message
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the expression result control type.
        /// </summary>
        public string type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the expression result control hidden.
        /// </summary>
        public string hidden
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the expression result control checked.
        /// </summary>
        public string IsChecked
        {
            get;
            set;
        }
    }
}
