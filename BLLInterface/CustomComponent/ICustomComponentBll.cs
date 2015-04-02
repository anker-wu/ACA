#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ICustomComponentBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ICustomComponentBll.cs 200815 2011-08-03 07:47:44Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  08/22/2011    daly.zeng    Initial version.
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.CustomComponent
{
    /// <summary>
    /// Defines methods for operation customize component configuration.
    /// </summary>
    public interface ICustomComponentBll
    {
        #region Methods

        /// <summary>
        /// Gets the component configuration.
        /// </summary>
        /// <param name="searchModel">The CustomComponentModel for search.</param>
        /// <returns>Return the component configuration.</returns>
        CustomComponentModel[] GetComponentConfig(CustomComponentModel searchModel);

        /// <summary>
        /// Saves the component configuration.
        /// </summary>
        /// <param name="customComponentList">The custom component list.</param>
        void SaveComponentConfig(CustomComponentModel[] customComponentList);

        #endregion Methods
    }
}
