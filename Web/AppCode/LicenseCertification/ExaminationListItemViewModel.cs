#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExaminationListItemViewModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Examination
{
    /// <summary>
    /// examination list item view model
    /// </summary>
    [Serializable]
    public class ExaminationListItemViewModel
    {
        /// <summary>
        /// Gets or sets examination view model.
        /// </summary>
        /// <value>the examination view model.</value>
        public ExaminationModel ExaminationViewModel
        {
            get;
            set;
        }
         
        /// <summary>
        /// Gets or sets the combined info.
        /// </summary>
        /// <value>The combined info.</value>
        public string CombinedInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the available actions.
        /// </summary>
        /// <value>The available actions.</value>
        public ExaminationActionViewModel[] AvailableActions
        {
            get;
            set;
        }
    }
}
