#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionActionViewModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionActionViewModel.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accela.ACA.Inspection;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Inspection
{
    #region enum

    /// <summary>
    /// Inspection wizard page name
    /// </summary>
    public enum InspectionWizardPageName
    {
        /// <summary>
        /// Unknown page
        /// </summary>
        Unknown,

        /// <summary>
        /// input type page
        /// </summary>
        InputType,

        /// <summary>
        /// input date time page
        /// </summary>
        InputDateTime,

        /// <summary>
        /// input location page
        /// </summary>
        InputLocation,

        /// <summary>
        /// input comments page
        /// </summary>
        InputComments,

        /// <summary>
        /// view details page
        /// </summary>
        ViewDetails,

        /// <summary>
        /// cancel page
        /// </summary>
        Cancel,

        /// <summary>
        /// print page
        /// </summary>
        Print
    }

    #endregion enum

    /// <summary>
    /// Inspection action view model
    /// </summary>
    [Serializable]
    public class InspectionActionViewModel
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public InspectionAction Action
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="InspectionActionViewModel"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="InspectionActionViewModel"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the action label.
        /// </summary>
        /// <value>The action label.</value>
        public string ActionLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the target URL.
        /// </summary>
        /// <value>The target URL.</value>
        public string TargetURL
        {
            get;
            set;
        }
    }
}
