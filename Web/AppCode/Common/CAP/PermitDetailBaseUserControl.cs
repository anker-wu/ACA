#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PermitDetailBaseUserControl.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 * define common function on permit detail page
 *
 *  Notes:
 *      $Id: PermitDetailBaseUserControl.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web
{
    /// <summary>
    /// Provide permit detail user control
    /// </summary>
    public class PermitDetailBaseUserControl : BaseUserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the PermitDetailBaseUserControl class.
        /// </summary>
        public PermitDetailBaseUserControl()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets CapModel object
        /// </summary>
        protected CapModel4WS CapModel
        {
            get
            {
                return AppSession.GetCapModelFromSession(ModuleName);
            }
        }

        #endregion Properties
    }
}