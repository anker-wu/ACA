#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ProviderDetail.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ProviderDetail.aspx.cs 277932 2014-08-22 10:29:52Z ACHIEVO\james.shi $.
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
    /// Page for provider detail.
    /// </summary>
    public partial class ProviderDetail : BasePage
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
                    refEducationList.DataSource = new DataTable();
                    refEducationList.BindEducationList(0, string.Empty);
                    refContinuingEducationList.DataSource = new DataTable();
                    refContinuingEducationList.BindContEduList(0, string.Empty);
                    refExaminationList.DataSource = new DataTable();
                    refExaminationList.BindExaminationList(0, string.Empty);
                }
                else
                {
                    IProviderBll providerBll = (IProviderBll)ObjectFactory.GetObject(typeof(IProviderBll));
                    ProviderModel4WS providerModel = providerBll.GetProviderByPK(GetProviderPK());

                    if (providerModel == null)
                    {
                        return;
                    }

                    //1. display provider detail.
                    refProviderDetail.DisplayProviderDetail(providerModel);

                    //2. display education list.
                    DataTable dtRefEducation = refEducationList.ConvertEducationListToDataTable(providerModel.refEduModel);
                    refEducationList.DataSource = dtRefEducation;
                    refEducationList.BindEducationList(0, string.Empty);

                    //3. display continuing education list.
                    DataTable dtRefContinuingEducation = refContinuingEducationList.ConvertListToDataTable(providerModel.refContEducations);
                    refContinuingEducationList.DataSource = dtRefContinuingEducation;
                    refContinuingEducationList.BindContEduList(0, string.Empty);

                    //4. display examination list.
                    DataTable dtRefExamination = refExaminationList.ConvertListToDataTable(providerModel.refExaminations);
                    refExaminationList.DataSource = dtRefExamination;
                    refExaminationList.BindExaminationList(0, string.Empty);

                    //5. Display Title info
                    lblPropertyInfo.Text = providerModel.providerName;
                }
            }
        }

        /// <summary>
        /// Get providerPK by URL parameter.
        /// </summary>
        /// <returns>a providerPK</returns>
        private ProviderPKModel4WS GetProviderPK()
        {
            ProviderPKModel4WS providerPKModel4WS = new ProviderPKModel4WS();

            if (ValidationUtil.IsInt(Request["providerPKNbr"]))
            {
                providerPKModel4WS.providerNbr = long.Parse(Request["providerPKNbr"]);
            }

            providerPKModel4WS.serviceProviderCode = ConfigManager.AgencyCode;

            return providerPKModel4WS;
        }

        #endregion Methods
    }
}