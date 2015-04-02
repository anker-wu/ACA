#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AdminConfigurationSave.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2010
 *
 *  Description:
 *
 *
 *  Notes:
 * $Id: AdminConfigurationSave.cs 178385 2010-08-06 07:47:06Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.BLL.Admin;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// This class is save configuration info in ACA admin 
    /// </summary>
    public class AdminConfigurationSave : IAdminConfigurationSave
    {
        #region Properties

        /// <summary>
        /// Gets an instance of GFilterView.
        /// </summary>
        private GFilterViewWebServiceService GFilterView
        {
            get
            {
                return WSFactory.Instance.GetWebService<GFilterViewWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Save admin configuration data
        /// </summary>
        /// <param name="configurationModel">configuration model</param>
        /// <returns>save result</returns>
        public bool SaveAdminConfigurationData(AdminConfigurationModel4WS configurationModel)
        {
            bool result = false;
            
            try
            {
                GFilterView.saveAdminConfigurationData(configurationModel);
                result = true;
            }
            catch
            {
            }

            return result;
        }

        #endregion Methods
    }
}