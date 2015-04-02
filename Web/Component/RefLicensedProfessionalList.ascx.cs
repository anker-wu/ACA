#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefLicensedProfessionalList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefLicensedProfessionalList.ascx.cs 245602 2013-03-07 10:17:15Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// UserControl for display RefLicenseProfessionalList list.
    /// </summary>
    public partial class RefLicensedProfessionalList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Command constant "selected license".
        /// </summary>
        private const string COMMAND_SELECTED_LICENSEE = "selectedLicensee";

        /// <summary>
        /// export file name for licensee
        /// </summary>
        private const string EXPORT_FILENAME_LICENSEE = "LicenseeList";

        /// <summary>
        /// export file name for food facility
        /// </summary>
        private const string EXPORT_FILENAME_FOODFACILITY = "FoodFacilityList";

        /// <summary>
        /// grid view page index changing event.
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        /// <summary>
        /// grid view sorted event.
        /// </summary>
        public event GridViewSortedEventHandler GridViewSort;

        /// <summary>
        /// grid view download event.
        /// </summary>
        public event GridViewDownloadEventHandler GridViewDownloadAll;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets license list data source.
        /// </summary>
        public IList<LicenseModel4WS> DataSource
        {
            get
            {
                if (ViewState["DataSource"] == null)
                {
                    ViewState["DataSource"] = new List<LicenseModel4WS>();
                }

                return (IList<LicenseModel4WS>)ViewState["DataSource"];
            }

            set
            {
                ViewState["DataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the search type with license or food facility
        /// </summary>
        public GeneralInformationSearchType SearchType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the page size.
        /// </summary>
        public int PageSize
        {
            get
            {
                return gdvRefLicenseeList.PageSize;
            }
        }

        /// <summary>
        /// Gets the count for display in search result label.
        /// </summary>
        public string CountSummary
        {
            get
            {
                return gdvRefLicenseeList.CountSummary;
            }
        }

        /// <summary>
        /// Gets or sets the view id for licensee list
        /// </summary>
        public string GViewID
        {
            get
            {
                return gdvRefLicenseeList.GridViewNumber;
            }

            set
            {
                gdvRefLicenseeList.GridViewNumber = value;
            }
        }

        #endregion Properties

        #region Method

        /// <summary>
        /// Bind licensee list.
        /// </summary>
        /// <param name="pageIndex">page index.</param>
        /// <param name="sort">string for sort.</param>
        public void BindLicenseeList(int pageIndex, string sort)
        {
            gdvRefLicenseeList.DataSource = DataSource;

            if (pageIndex >= 0)
            {
                gdvRefLicenseeList.PageIndex = pageIndex;
            }

            if (!string.IsNullOrEmpty(sort))
            {
                string[] s = sort.Trim().Split(' ');

                if (s.Length == 2)
                {
                    gdvRefLicenseeList.GridViewSortExpression = s[0];
                    gdvRefLicenseeList.GridViewSortDirection = s[1];
                    gdvRefLicenseeList.DataSource = gdvRefLicenseeList.SortList(DataSource, true);
                }
            }

            gdvRefLicenseeList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvRefLicenseeList.DataBind();
        }

        /// <summary>
        /// Convert licensee list to data table.
        /// </summary>
        /// <param name="licenseeModelist">LicenseModel array</param>
        /// <returns>dataTable for licensee list</returns>
        public DataTable ConvertLicenseeListToDataTable(IList<LicenseModel4WS> licenseeModelist)
        {
            DataTable dtLicensee = LicenseUtil.CreateDataTable4License();

            if (licenseeModelist == null || licenseeModelist.Count == 0)
            {
                return dtLicensee;
            }

            foreach (LicenseModel4WS licenseeModel in licenseeModelist)
            {
                if (licenseeModel == null)
                {
                    continue;
                }

                DataRow dr = dtLicensee.NewRow();
                dtLicensee.Rows.Add(dr);

                dr[ColumnConstant.RefLicenseProfessional.StateLicense.ToString()] = licenseeModel.stateLicense;
                dr[ColumnConstant.RefLicenseProfessional.LicenseType.ToString()] = licenseeModel.licenseType;
                dr[ColumnConstant.RefLicenseProfessional.TypeFlag.ToString()] = licenseeModel.typeFlag;
                dr[ColumnConstant.RefLicenseProfessional.MaskedSSN.ToString()] = licenseeModel.maskedSsn;
                dr[ColumnConstant.RefLicenseProfessional.Fein.ToString()] = licenseeModel.fein;
                dr[ColumnConstant.RefLicenseProfessional.BusinessName.ToString()] = licenseeModel.businessName;
                dr[ColumnConstant.RefLicenseProfessional.BusinessLicense.ToString()] = licenseeModel.businessLicense;
                dr[ColumnConstant.RefLicenseProfessional.ContactFirstName.ToString()] = licenseeModel.contactFirstName;
                dr[ColumnConstant.RefLicenseProfessional.ContactMiddleName.ToString()] = licenseeModel.contactMiddleName;
                dr[ColumnConstant.RefLicenseProfessional.ContactLastName.ToString()] = licenseeModel.contactLastName;
                dr[ColumnConstant.RefLicenseProfessional.FullAddress.ToString()] = licenseeModel;
                dr[ColumnConstant.RefLicenseProfessional.LicenseExpirationDate.ToString()] = licenseeModel.licenseExpirationDate;
                dr[ColumnConstant.RefLicenseProfessional.InsuranceExpDate.ToString()] = licenseeModel.insuranceExpDate;
                dr[ColumnConstant.RefLicenseProfessional.LicenseBoard.ToString()] = licenseeModel.licenseBoard;
                dr[ColumnConstant.RefLicenseProfessional.LicenseIssueDate.ToString()] = licenseeModel.licenseIssueDate;
                dr[ColumnConstant.RefLicenseProfessional.LicenseLastRenewalDate.ToString()] = licenseeModel.licenseLastRenewalDate;
                dr[ColumnConstant.RefLicenseProfessional.City.ToString()] = licenseeModel.city;
                dr[ColumnConstant.RefLicenseProfessional.State.ToString()] = licenseeModel.state;
                dr[ColumnConstant.RefLicenseProfessional.Zip.ToString()] = licenseeModel.zip;
                dr[ColumnConstant.RefLicenseProfessional.Address1.ToString()] = licenseeModel.address1;
                dr[ColumnConstant.RefLicenseProfessional.Address2.ToString()] = licenseeModel.address2;
                dr[ColumnConstant.RefLicenseProfessional.Address3.ToString()] = licenseeModel.address3;
                dr[ColumnConstant.RefLicenseProfessional.BusName2.ToString()] = licenseeModel.busName2;
                dr[ColumnConstant.RefLicenseProfessional.Title.ToString()] = licenseeModel.title;
                dr[ColumnConstant.RefLicenseProfessional.Policy.ToString()] = licenseeModel.policy;
                dr[ColumnConstant.RefLicenseProfessional.InsuranceCo.ToString()] = licenseeModel.insuranceCo;
                dr[ColumnConstant.RefLicenseProfessional.ResState.ToString()] = licenseeModel.resState;
                dr[ColumnConstant.RefLicenseProfessional.CountryCode.ToString()] = licenseeModel.countryCode;
            }

            return dtLicensee;
        }

        /// <summary>
        /// fire sorted event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LicenseeList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            if (GridViewSort != null)
            {
                GridViewSort(sender, e);
            }
        }

        /// <summary>
        /// Page index changing event
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LicenseeList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// GridView LicenseeList row command event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="args">A System.EventArgs object containing the event data.</param>
        protected void LicenseeList_RowCommand(object sender, GridViewCommandEventArgs args)
        {
            //if e is null, not process
            if (args == null || args.CommandArgument == null)
            {
                return;
            }

            if (args.CommandName == COMMAND_SELECTED_LICENSEE)
            {
                int dataItemIndex = Convert.ToInt32(args.CommandArgument);
                LicenseModel4WS license = null;

                // use the grid view's data source not the Component's data source,
                // because the grid view can be sorted cause the data source items' index changed.
                IList gvDataSoure = gdvRefLicenseeList.DataSource as IList;

                if (gvDataSoure != null && gvDataSoure.Count > dataItemIndex)
                {
                    license = gvDataSoure[dataItemIndex] as LicenseModel4WS;
                }

                if (license != null)
                {
                    string licenseNumber = Convert.ToString(license.stateLicense);
                    string licenseType = Convert.ToString(license.licenseType);

                    string url = "LicenseeDetail.aspx";
                    if (SearchType == GeneralInformationSearchType.Search4FoodFacilityInspection)
                    {
                        url = "FoodFacilityInspectionDetail.aspx";
                    }

                    Response.Redirect(string.Format("{0}?LicenseeNumber={1}&LicenseeType={2}", url, licenseNumber, licenseType));
                }
            }
        }

        /// <summary>
        /// GridView RefLicenseeList download event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A GridViewDownloadEventArgs object containing the event data.</param>
        protected void RefLicenseeList_GridViewDownload(object sender, GridViewDownloadEventArgs e)
        {
            // execute the download event that setting the exported content.
            if (GridViewDownloadAll != null)
            {
                GridViewDownloadAll(sender, e);
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the GridView LicenseeList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void LicenseeList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            AccelaLabel lblAddress = e.Row.FindControl("lblAddress") as AccelaLabel;
            AccelaLabel lblZip = e.Row.FindControl("lblZip") as AccelaLabel;

            if (e.Row.RowType == DataControlRowType.DataRow && lblAddress != null)
            {
                LicenseModel4WS licensee = e.Row.DataItem as LicenseModel4WS;

                if (licensee != null)
                {
                    lblAddress.Text = LicenseUtil.GetAddressDetail4License(licensee);
                    lblZip.Text = ModelUIFormat.FormatZipShow(licensee.zip, licensee.countryCode);
                }
            }
        }
        
        /// <summary>
        /// <c>OnInit</c> event method.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            gdvRefLicenseeList.GridViewNumber = GviewID.LicenseeList;

            string exportFileName = EXPORT_FILENAME_LICENSEE;

            if (SearchType == GeneralInformationSearchType.Search4FoodFacilityInspection)
            {
                gdvRefLicenseeList.GridViewNumber = GviewID.FoodFacilityList;
                exportFileName = EXPORT_FILENAME_FOODFACILITY;
            }

            // set the simple view element before the GridViewNumber is setted.
            GridViewBuildHelper.SetSimpleViewElements(gdvRefLicenseeList, ModuleName, AppSession.IsAdmin);

            if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
            {
                gdvRefLicenseeList.ShowExportLink = true;
                gdvRefLicenseeList.ExportFileName = exportFileName;
            }
            else
            {
                gdvRefLicenseeList.ShowExportLink = false;
            }

            base.OnInit(e);
        }

        /// <summary>
        /// OnPreRender event method.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);            

            ChangeGridViewHeaderLabelKey();
        }

        /// <summary>
        /// Change the GridView header's label key.
        /// </summary>
        private void ChangeGridViewHeaderLabelKey()
        {
            Control headerRow = null;

            if (gdvRefLicenseeList.HasControls() && gdvRefLicenseeList.Controls[0].HasControls())
            {
                headerRow = gdvRefLicenseeList.Controls[0].Controls[0];
            }

            if (headerRow != null)
            {
                GridViewHeaderLabel lnkRefLicenseNbrHeader = headerRow.FindControl("lnkRefLicenseNbrHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkLicenseTypeHeader = headerRow.FindControl("lnkLicenseTypeHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkBusinessNameHeader = headerRow.FindControl("lnkBusinessNameHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkLicenseFirstNameHeader = headerRow.FindControl("lnkLicenseFirstNameHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkLicenseMiddleNameHeader = headerRow.FindControl("lnkLicenseMiddleNameHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkLicenseLastNameHeader = headerRow.FindControl("lnkLicenseLastNameHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkAddressHeader = headerRow.FindControl("lnkAddressHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkLicenseExpirationDateHeader = headerRow.FindControl("lnkLicenseExpirationDateHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkInsuranceExpirationDateHeader = headerRow.FindControl("lnkInsuranceExpirationDateHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkLicensingBoardHeader = headerRow.FindControl("lnkLicensingBoardHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkLicenseIssueDateHeader = headerRow.FindControl("lnkLicenseIssueDateHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkLicenseLastRenewalDateHeader = headerRow.FindControl("lnkLicenseLastRenewalDateHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkContactTypeHeader = headerRow.FindControl("lnkContactTypeHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkSSNHeader = headerRow.FindControl("lnkSSNHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkFEINHeader = headerRow.FindControl("lnkFEINHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkBusinessLicenseHeader = headerRow.FindControl("lnkBusinessLicenseHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkCityHeader = headerRow.FindControl("lnkCityHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkStateHeader = headerRow.FindControl("lnkStateHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkZipHeader = headerRow.FindControl("lnkZipHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkAddress1Header = headerRow.FindControl("lnkAddress1Header") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkAddress2Header = headerRow.FindControl("lnkAddress2Header") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkAddress3Header = headerRow.FindControl("lnkAddress3Header") as GridViewHeaderLabel;

                GridViewHeaderLabel lnkBusiName2Header = headerRow.FindControl("lnkBusiName2Header") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkTitleHeader = headerRow.FindControl("lnkTitleHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkInsurancePolicyHeader = headerRow.FindControl("lnkInsurancePolicyHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkInsuranceCompanyHeader = headerRow.FindControl("lnkInsuranceCompanyHeader") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkCountryHeader = headerRow.FindControl("lnkCountryHeader") as GridViewHeaderLabel;

                switch (SearchType)
                {
                    case GeneralInformationSearchType.Search4Licensee:
                        SetGridViewHeaderLabelKey(lnkRefLicenseNbrHeader, "aca_licenseelist_license_number");
                        SetGridViewHeaderLabelKey(lnkLicenseTypeHeader, "aca_licenseelist_license_type");
                        SetGridViewHeaderLabelKey(lnkBusinessNameHeader, "aca_licenseelist_license_businessname");
                        SetGridViewHeaderLabelKey(lnkLicenseFirstNameHeader, "aca_licenseelist_license_firstame");
                        SetGridViewHeaderLabelKey(lnkLicenseMiddleNameHeader, "aca_licenseelist_license_middlename");
                        SetGridViewHeaderLabelKey(lnkLicenseLastNameHeader, "aca_licenseelist_license_lastname");
                        SetGridViewHeaderLabelKey(lnkAddressHeader, "aca_licenseelist_license_address");
                        SetGridViewHeaderLabelKey(lnkLicenseExpirationDateHeader, "aca_licenseelist_license_expirationdate");
                        SetGridViewHeaderLabelKey(lnkInsuranceExpirationDateHeader, "aca_licenseelist_license_insuranceexpirationdate");
                        SetGridViewHeaderLabelKey(lnkLicensingBoardHeader, "aca_licenseelist_license_licensingboard");
                        SetGridViewHeaderLabelKey(lnkLicenseIssueDateHeader, "aca_licenseelist_license_issuedate");
                        SetGridViewHeaderLabelKey(lnkLicenseLastRenewalDateHeader, "aca_licenseelist_license_renewaldate");
                        SetGridViewHeaderLabelKey(lnkContactTypeHeader, "aca_licenseelist_license_contacttype");
                        SetGridViewHeaderLabelKey(lnkSSNHeader, "aca_licenseelist_license_ssn");
                        SetGridViewHeaderLabelKey(lnkFEINHeader, "aca_licenseelist_license_fein");
                        SetGridViewHeaderLabelKey(lnkBusinessLicenseHeader, "aca_licenseelist_license_businesslicense");
                        SetGridViewHeaderLabelKey(lnkCityHeader, "aca_licenseelist_label_city");
                        SetGridViewHeaderLabelKey(lnkStateHeader, "aca_licenseelist_label_state");
                        SetGridViewHeaderLabelKey(lnkZipHeader, "aca_licenseelist_label_zip");
                        SetGridViewHeaderLabelKey(lnkAddress1Header, "aca_licenseelist_label_address1");
                        SetGridViewHeaderLabelKey(lnkAddress2Header, "aca_licenseelist_label_address2");
                        SetGridViewHeaderLabelKey(lnkAddress3Header, "aca_licenseelist_label_address3");                        

                        SetGridViewHeaderLabelKey(lnkBusiName2Header, "aca_licenseelist_label_businame2");
                        SetGridViewHeaderLabelKey(lnkTitleHeader, "aca_licenseelist_label_title");
                        SetGridViewHeaderLabelKey(lnkInsurancePolicyHeader, "aca_licenseelist_label_insurancepolicy");
                        SetGridViewHeaderLabelKey(lnkInsuranceCompanyHeader, "aca_licenseelist_label_insurancecompany");
                        SetGridViewHeaderLabelKey(lnkCountryHeader, "aca_licenseelist_label_country");
                        break;
                    case GeneralInformationSearchType.Search4FoodFacilityInspection:
                        SetGridViewHeaderLabelKey(lnkRefLicenseNbrHeader, "aca_foodfacilitylist_label_number");
                        SetGridViewHeaderLabelKey(lnkLicenseTypeHeader, "aca_foodfacilitylist_label_type");
                        SetGridViewHeaderLabelKey(lnkBusinessNameHeader, "aca_foodfacilitylist_label_businessname");
                        SetGridViewHeaderLabelKey(lnkLicenseFirstNameHeader, "aca_foodfacilitylist_label_firstame");
                        SetGridViewHeaderLabelKey(lnkLicenseMiddleNameHeader, "aca_foodfacilitylist_label_middlename");
                        SetGridViewHeaderLabelKey(lnkLicenseLastNameHeader, "aca_foodfacilitylist_label_lastname");
                        SetGridViewHeaderLabelKey(lnkAddressHeader, "aca_foodfacilitylist_label_address");
                        SetGridViewHeaderLabelKey(lnkLicenseExpirationDateHeader, "aca_foodfacilitylist_label_expirationdate");
                        SetGridViewHeaderLabelKey(lnkInsuranceExpirationDateHeader, "aca_foodfacilitylist_label_insuranceexpirationdate");
                        SetGridViewHeaderLabelKey(lnkLicensingBoardHeader, "aca_foodfacilitylist_label_licensingboard");
                        SetGridViewHeaderLabelKey(lnkLicenseIssueDateHeader, "aca_foodfacilitylist_label_issuedate");
                        SetGridViewHeaderLabelKey(lnkLicenseLastRenewalDateHeader, "aca_foodfacilitylist_label_renewaldate");
                        SetGridViewHeaderLabelKey(lnkContactTypeHeader, "aca_foodfacilitylist_label_contacttype");
                        SetGridViewHeaderLabelKey(lnkSSNHeader, "aca_foodfacilitylist_label_ssn");
                        SetGridViewHeaderLabelKey(lnkFEINHeader, "aca_foodfacilitylist_label_fein");
                        SetGridViewHeaderLabelKey(lnkBusinessLicenseHeader, "aca_foodfacilitylist_label_businesslicense");
                        SetGridViewHeaderLabelKey(lnkCityHeader, "aca_foodfacilitylist_label_city");
                        SetGridViewHeaderLabelKey(lnkStateHeader, "aca_foodfacilitylist_label_state");
                        SetGridViewHeaderLabelKey(lnkZipHeader, "aca_foodfacilitylist_label_zip");
                        SetGridViewHeaderLabelKey(lnkAddress1Header, "aca_foodfacilitylist_label_address1");
                        SetGridViewHeaderLabelKey(lnkAddress2Header, "aca_foodfacilitylist_label_address2");
                        SetGridViewHeaderLabelKey(lnkAddress3Header, "aca_foodfacilitylist_label_address3");    

                        SetGridViewHeaderLabelKey(lnkBusiName2Header, "aca_foodfacilitylist_label_businame2");
                        SetGridViewHeaderLabelKey(lnkTitleHeader, "aca_foodfacilitylist_label_title");
                        SetGridViewHeaderLabelKey(lnkInsurancePolicyHeader, "aca_foodfacilitylist_label_insurancepolicy");
                        SetGridViewHeaderLabelKey(lnkInsuranceCompanyHeader, "aca_foodfacilitylist_label_insurancecompany");
                        SetGridViewHeaderLabelKey(lnkCountryHeader, "aca_foodfacilitylist_label_country");
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Set the GridViewHeaderLabel's label key
        /// </summary>
        /// <param name="gridViewHeaderLabel">The GridView header label</param>
        /// <param name="labelKey">The label key</param>
        private void SetGridViewHeaderLabelKey(GridViewHeaderLabel gridViewHeaderLabel, string labelKey)
        {
            if (gridViewHeaderLabel != null)
            {
                gridViewHeaderLabel.LabelKey = labelKey;
            }
        }

        #endregion Method        
    }
}
