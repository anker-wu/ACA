#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExpressionSessionModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ExpressionSessionModel.cs 194095 2011-03-29 12:17:11Z ACHIEVO\hans.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;

using Accela.ACA.WSProxy;

namespace Accela.ACA.ExpressionBuild
{
    /// <summary>
    /// The model of the expression Session.
    /// </summary>
    [Serializable]
    public class ExpressionSessionModel
    {
        /// <summary>
        /// Gets or sets the expression factory type
        /// </summary>
        public ExpressionType ExpressionFactoryType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the expression runtime argument model
        /// </summary>
        public List<ExpressionRuntimeArgumentPKModel> ExpressionArgumentPKModels
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the expression controls
        /// Format:(ExpressionFieldName,ControlClientID)
        /// </summary>
        public Dictionary<string, string> ExpressionBaseControls
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the asit UI data key.
        /// </summary>
        public string AsitUiDataKey
        {
            get;
            set;
        }
    }
}
