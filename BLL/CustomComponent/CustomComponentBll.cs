#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CustomComponentBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: CustomComponentBll.cs 200815 2011-08-03 07:47:44Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  08/22/2011    daly.zeng    Initial version.
 * </pre>
 */

#endregion Header

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.CustomComponent
{
    /// <summary>
    /// This class provide the ability to operation customize component configuration.
    /// </summary>
    internal class CustomComponentBll : BaseBll, ICustomComponentBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of CustomComponentWebService.
        /// </summary>
        private CustomComponentWebServiceService CustomComponentService
        {
            get
            {
                return WSFactory.Instance.GetWebService<CustomComponentWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the component configuration.
        /// </summary>
        /// <param name="searchModel">The CustomComponentModel for search.</param>
        /// <returns>Return the component configuration.</returns>
        public CustomComponentModel[] GetComponentConfig(CustomComponentModel searchModel)
        {
            return CustomComponentService.getComponentConfig(searchModel);
        }

        /// <summary>
        /// Saves the component configuration.
        /// </summary>
        /// <param name="customComponentList">The CustomComponentModel list.</param>
        public void SaveComponentConfig(CustomComponentModel[] customComponentList)
        {
            CustomComponentService.saveComponentConfig(customComponentList);
        }

        #endregion Methods
    }
}
