#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ProviderNameList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ProviderNameList.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Provider name list, It used for populate some provider information to education detail form.
    /// </summary>
    public partial class ProviderNameList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Method name for fill provider information.
        /// </summary>
        private string _fillMethodName = "FillProviderInfo";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating fill provider information method.
        /// </summary>
        public string FillMethodName
        {
            get
            {
                return _fillMethodName;
            }

            set
            {
                _fillMethodName = value;
            }
        }

        #endregion Properties

        /// <summary>
        /// Display provider names in search Form.
        /// </summary>
        /// <param name="providers">provider model list</param>
        /// <param name="provierName">provider name</param>
        public void DisplayProviderNames(ProviderModel4WS[] providers, string provierName)
        {
            txtProviderName.Text = provierName;

            // get all provider name, it split by ACAConstant.SPLIT_CHAR. 
            string providerNames = GetProviderNames(providers);

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "DisplayProviderName" + ClientID, string.Format("DisplayProviderNames('{0}')", providerNames), true);
        } 
      
        /// <summary>
        /// Page load event.
        /// </summary>
        /// <param name="sender">Event object.</param>
        /// <param name="e">Event argument.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // onpropertychange is input event in IE.
                txtProviderName.Attributes.Add("onpropertychange", "FilterProviders()");

                // oninput is input event in FireFox.
                txtProviderName.Attributes.Add("oninput", "FilterProviders()");
            }
        }

        /// <summary>
        /// Get all provider name.
        /// </summary>
        /// <param name="providers">provider model list</param>
        /// <returns>provider names</returns>
        private string GetProviderNames(ProviderModel4WS[] providers)
        {
            StringBuilder sb = new StringBuilder();

            if (providers != null && providers.Length > 0)
            {
                foreach (ProviderModel4WS provider in providers)
                {
                    sb.Append(provider.providerName);
                    sb.Append(ACAConstant.SPLIT_CHAR);
                }
            }

            string providerNames = string.Empty;

            if (sb.Length > 0)
            {
                sb.Length -= 1; // remove the last delimiter-ACAConstant.SPLIT_CHAR.
                providerNames = sb.ToString();
            }

            return providerNames;
        }
    }
}
