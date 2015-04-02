#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContinuingEducationDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ContinuingEducationDetail.aspx.cs 140040 2009-07-21 06:06:55Z ACHIEVO\jackie.yu $.
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
    /// Page for ContinuingEducationDetail detail.
    /// </summary>
    public partial class ContinuingEducationDetail : BasePage
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
                    IRefContinuingEducationBll refContinuingEducationBll = (IRefContinuingEducationBll)ObjectFactory.GetObject(typeof(IRefContinuingEducationBll));

                    RefContinuingEducationModel4WS refContinuingEducationModel = refContinuingEducationBll.GetRefContinuingEducationByPK(GetContinuingEducationPK());

                    if (refContinuingEducationModel == null)
                    {
                        return;
                    }

                    //1. display education detail.
                    refContinuingEducationDetail.Display(refContinuingEducationModel);

                    //2. display provider list.
                    DataTable dtProviderInfo = refProviderList.ConvertProviderListToDataTable(refContinuingEducationModel.providerModels);
                    refProviderList.DataSource = dtProviderInfo;
                    refProviderList.BindProviderList(0, string.Empty);

                    //3. Display Title info
                    lblPropertyInfo.Text = refContinuingEducationModel.contEduName;
                }
            }
        }

        /// <summary>
        /// Get continuing educationPK by URL parameter.
        /// </summary>
        /// <returns>a education PK Model</returns>
        private RefContinuingEducationPKModel4WS GetContinuingEducationPK()
        {
            RefContinuingEducationPKModel4WS refContinuingEducationPKModel  = new RefContinuingEducationPKModel4WS();

            if (ValidationUtil.IsInt(Request["refContEduNbr"]))
            {
                refContinuingEducationPKModel.refContEduNbr = long.Parse(Request["refContEduNbr"]);
            }

            refContinuingEducationPKModel.serviceProviderCode = ConfigManager.AgencyCode;

            return refContinuingEducationPKModel;
        }

        #endregion Methods
    }
}
