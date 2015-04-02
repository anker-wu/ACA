#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GeneralSearchModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012
 *
 *  Description:
 *  This is the general search model.
 *  Notes:
 *      $Id: GeneralSearchModel.cs 185614 2012-06-14 06:22:24Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Provide a class to manage general search model
    /// </summary>
    public class GeneralSearchModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the CapModel object.
        /// </summary>
        public CapModel4WS CapModel
        { 
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the module name list.
        /// </summary>
        public List<string> ModuleNameList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the hidden view element names
        /// </summary>
        public string[] HiddenViewEltNames
        {
            get;
            set;
        }

        #endregion Properties
    }
}
