#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EducationDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EducationDetail.aspx.cs 277932 2014-08-22 10:29:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;

using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.GeneralProperty
{
    /// <summary>
    /// Page for education detail.
    /// </summary>
    public partial class EducationDetail : BasePage
    {
        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    refProviderList.DataSource = new DataTable();
                    refProviderList.BindProviderList(0, string.Empty);
                }
                else
                {
                    IRefEducationBll refEducationBll = (IRefEducationBll)ObjectFactory.GetObject(typeof(IRefEducationBll));
                    RefEducationModel4WS refEducationModel = refEducationBll.GetRefEducationByPK(GetEducationPK());

                    if (refEducationModel == null)
                    {
                        return;
                    }

                    //1. display education detail.
                    refEducationDetail.DisplayEducationDetail(refEducationModel);

                    //2. display provider list.
                    DataTable dtProviderInfo = refProviderList.ConvertProviderListToDataTable(refEducationModel.providerModels);
                    refProviderList.DataSource = dtProviderInfo;
                    refProviderList.BindProviderList(0, string.Empty);

                    //3. Display Title info
                    lblPropertyInfo.Text = refEducationModel.refEducationName;
                }
            }
        }

        /// <summary>
        /// Get educationPK by URL parameter.
        /// </summary>
        /// <returns>a educationPK</returns>
        private RefEducationPKModel4WS GetEducationPK()
        {
            RefEducationPKModel4WS providerPK = new RefEducationPKModel4WS();

            if (ValidationUtil.IsInt(Request["refEducationNbr"]))
            {
                providerPK.refEducationNbr = long.Parse(Request["refEducationNbr"]);
            }
            
            providerPK.serviceProviderCode = ConfigManager.AgencyCode;

            return providerPK;
        }

        #endregion Methods
    }
}