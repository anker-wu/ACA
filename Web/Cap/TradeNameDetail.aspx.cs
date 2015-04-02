#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TradeNameDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: TradeNameDetail.aspx.cs 277932 2014-08-22 10:29:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  2008.08.15          Kale.huang
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// the class for trade name detail.
    /// </summary>
    public partial class TradeNameDetail : BasePage
    {
        #region Methods

        /// <summary>
        /// On Initial event method.
        /// </summary>
        /// <param name="e">EventArgs object</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvPermitList, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string tradeNameNumber = Request.QueryString["lpNum"].ToString();
                string tradeNameSeqNbr = Request.QueryString["LpSeqNbr"].ToString();
                string tradeNameType = Request.QueryString["lpType"].ToString();
                string agencyCode = ConfigManager.AgencyCode;

                CapIDModel4WS capIDModelCon = new CapIDModel4WS();
                capIDModelCon.customID = tradeNameNumber;
                capIDModelCon.serviceProviderCode = agencyCode;

                ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
                CapModel4WS capModel = capBll.GetTradeNameCapModel(capIDModelCon);

                // display cap detail info
                DisplayTradeNameDetail(capModel, tradeNameNumber, tradeNameType, agencyCode);

                //display associated license
                DisplayTradeLicenseDetail(tradeNameNumber, tradeNameType, tradeNameSeqNbr, agencyCode);
            }
        }

        /// <summary>
        /// PermitList RowDataBound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void PermitList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                HyperLink hlTradeLicenseDetail = (HyperLink)e.Row.FindControl("hlTradeLicenseDetail");

                string capID1 = rowView["capID1"].ToString();
                string capID2 = rowView["capID2"].ToString();
                string capID3 = rowView["capID3"].ToString();
                string agencyCode = rowView["AgencyCode"].ToString();

                hlTradeLicenseDetail.NavigateUrl = string.Format("../Cap/CapDetail.aspx?Module={0}&capID1={1}&capID2={2}&capID3={3}&{4}={5}", ModuleName, capID1, capID2, capID3, UrlConstant.AgencyCode, agencyCode);
            }
        }

        /// <summary>
        /// Display Trade License Detail
        /// </summary>
        /// <param name="tradeNameNumber">trade Name Number</param>
        /// <param name="tradeNameType">trade Name Type</param>
        /// <param name="tradeNameSeqNbr">trade Name Sequence Number</param>
        /// <param name="agencyCode">the agency code.</param>
        private void DisplayTradeLicenseDetail(string tradeNameNumber, string tradeNameType, string tradeNameSeqNbr, string agencyCode)
        {
            LicenseModel4WS lpModel = new LicenseModel4WS();
            lpModel.serviceProviderCode = agencyCode;
            lpModel.licSeqNbr = tradeNameSeqNbr;
            lpModel.licenseType = tradeNameType;
            lpModel.stateLicense = tradeNameNumber;

            ILicenseProfessionalBll lpBll = ObjectFactory.GetObject<ILicenseProfessionalBll>();
            System.Data.DataTable associatedLicense = new DataTable();

            if (!AppSession.IsAdmin)
            {
                associatedLicense = lpBll.GetAssociatedLicense(lpModel, null);
            }

            if (AppSession.IsAdmin || associatedLicense.Rows.Count > 0)
            {
                divRelatedTradeLicense.Visible = true;
                gdvPermitList.DataSource = associatedLicense;
                gdvPermitList.DataBind();
            }
            else
            {
                divRelatedTradeLicense.Visible = false;
            }
        }

        /// <summary>
        /// Display Trade License Detail
        /// </summary>
        /// <param name="capModel">cap model for ACA.</param>
        /// <param name="tradeNameNumber">trade Name Number</param>
        /// <param name="tradeNameType">trade Name Type</param>
        /// <param name="agencyCode">the agency code.</param>
        private void DisplayTradeNameDetail(CapModel4WS capModel, string tradeNameNumber, string tradeNameType, string agencyCode)
        {
            int lines = 0;
            LicenseModel4WS licenseModelCon = new LicenseModel4WS();
            licenseModelCon.stateLicense = tradeNameNumber;
            licenseModelCon.licenseType = tradeNameType;
            licenseModelCon.serviceProviderCode = agencyCode;

            LicenseModel4WS licenseModel = null;
            if (!AppSession.IsAdmin)
            {
                ILicenseBLL licenseBll = ObjectFactory.GetObject<ILicenseBLL>();
                licenseModel = licenseBll.GetLicenseByStateLicNbr(licenseModelCon);
            }

            //Show the Trade Name Info
            if (licenseModel != null)
            {
                //Show Trade Name Num
                lblNumber.Text = licenseModel.stateLicense;

                //Show English/Arabic Name of Trade Name
                System.Text.StringBuilder sb = new StringBuilder();
                sb.Append(string.IsNullOrEmpty(licenseModel.businessName) ? string.Empty : licenseModel.businessName.Trim());
                if (sb.ToString().Length > 0)
                {
                    if (!string.IsNullOrEmpty(licenseModel.busName2))
                    {
                        sb.Append("/").Append(licenseModel.busName2.Trim());
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(licenseModel.busName2))
                    {
                        sb.Append(licenseModel.busName2.Trim());
                    }
                }

                lblTradeName.Text = sb.ToString();

                //Show Trade Name
                string tradeName = string.Empty;
                if (licenseModel != null)
                {
                    tradeName = ModelUIFormat.FormatTradeName4Basic(licenseModel, capModel);
                }

                lblLicenseProfessionalBasic.Text = tradeName;
            }

            //Show the Applicant info.
            if (capModel.applicantModel != null && capModel.applicantModel.people != null)
            {
                lblApplicantBasic.Text = ModelUIFormat.FormatApplicant4Display(capModel.applicantModel, ModuleName, out lines);
            }

            // Show ASTTable
            ASITable asit = new ASITable(capModel, ModuleName, IsPostBack);
            pnlASITable.Controls.Add(asit.DisplayInTradeNameDetailView());

            if (AppSession.IsAdmin || lines != 0)
            {
                dvApplicant.Visible = true;
            }
            else
            {
                dvApplicant.Visible = false;
            }

            dvLicenseProfessional.Visible = AppSession.IsAdmin || !string.IsNullOrEmpty(lblLicenseProfessionalBasic.Text);

            tbMoreDetail.Visible = asit.StrASIT == string.Empty ? AppSession.IsAdmin : true;
        }

        #endregion Methods
    }
}