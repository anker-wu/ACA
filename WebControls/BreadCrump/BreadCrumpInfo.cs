#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: BreadCrumpInfo.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: BreadCrumpInfo.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Provide bread crump info
    /// </summary>
    [Serializable]
    public class BreadCrumpInfo
    {
        #region Fields

        /// <summary>
        /// Implement default value for Enable property.
        /// </summary>
        private bool _enable = true;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets cap name
        /// </summary>
        public string CapName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets page title
        /// </summary>
        public string Pagetitle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets step index
        /// </summary>
        public int StepIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets step index title
        /// </summary>
        public string StepIndexTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets bread crump title
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets url
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the page instructions.
        /// </summary>
        /// <value>The page instructions.</value>
        public List<string> PageInstructions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the bread crumb is enable. Default value is true.
        /// </summary>
        public bool Enable
        {
            get
            {
                return _enable;
            }

            set
            {
                _enable = value;
            }
        }

        #endregion Properties
    }
}