#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AutoFillCityAndState.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  All of global session objects should be definted in this class.
 *  Notes:
 *      $Id: AutoFillCityAndStateUtil.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// The utility is from auto Fill City and State.
    /// </summary>
    public static class AutoFillCityAndStateUtil
    {
        /// <summary>
        /// Set current City
        /// </summary>
        /// <param name="txtCity">the city control.</param>
        /// <param name="moduleName">the module name.</param>
        public static void SetCurrentCity(AccelaTextBox txtCity, string moduleName)
        {
            bool isAutoFillCity = txtCity.Visible && StandardChoiceUtil.IsEnableForData4AsKey(ACAConstant.STD_AUTOFILL_CITY_ENABLED, moduleName, ((int)txtCity.PositionID).ToString());

            if (isAutoFillCity)
            {
                IServiceProviderBll serProviderBll = (IServiceProviderBll)ObjectFactory.GetObject(typeof(IServiceProviderBll));
                ServiceProviderModel model = serProviderBll.GetServiceProviderByPK(ConfigManager.AgencyCode, null);

                txtCity.Text = model.city;
            }
        }

        /// <summary>
        /// Set current State
        /// </summary>
        /// <param name="ddlState">the state control.</param>
        /// <param name="moduleName">the module name.</param>
        public static void SetCurrentState(AccelaStateControl ddlState, string moduleName)
        {
            bool isAutoFillState = ddlState.Visible && StandardChoiceUtil.IsEnableForData4AsKey(ACAConstant.STD_AUTOFILL_STATE_ENABLED, moduleName, ((int)ddlState.PositionID).ToString());

            if (isAutoFillState)
            {
                IServiceProviderBll serProviderBll = (IServiceProviderBll)ObjectFactory.GetObject(typeof(IServiceProviderBll));
                ServiceProviderModel model = serProviderBll.GetServiceProviderByPK(ConfigManager.AgencyCode, null);

                if (isAutoFillState)
                {
                    ddlState.Text = model.state;
                }
            }
        }
    }
}