#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: PeopleList.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: PeopleList.ascx.cs 178055 2010-07-30 07:37:23Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;  &lt;Who&gt;  &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Data;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// UC for display people list.
    /// </summary>
    public partial class PeopleList : BaseUserControl
    {
        #region Properties
        /// <summary>
        /// Gets or sets people list data table.
        /// </summary>
        private DataTable DataSource
        {
            get
            {
                if (ViewState["DataSource"] == null)
                {
                    ViewState["DataSource"] = new DataTable();
                }

                return ViewState["DataSource"] as DataTable;
            }

            set
            {
                ViewState["DataSource"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Bind data table by people list.
        /// </summary>
        /// <param name="trustAccountPeople">TrustAccount PeopleModels</param>
        public void BindList(TrustAccountPeopleModel[] trustAccountPeople)
        {
            DataSource = ConvertLicenseProfessionalDataTable(trustAccountPeople);
            Bind();
        }

        /// <summary>
        /// GridView People row command event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void PeopleList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Bind();
        }

        /// <summary>
        /// <c>OnInit</c> event method.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
                {
                    dgvPeopleList.ShowExportLink = true;
                    dgvPeopleList.ExportFileName = "AssociatedPeopleList";
                }
                else
                {
                    dgvPeopleList.ShowExportLink = false;
                }
            }

            GridViewBuildHelper.SetSimpleViewElements(dgvPeopleList, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// GridView ContactList row data bound event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void PeopleList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AccelaLabel lblCountryOrRegion = (AccelaLabel)e.Row.FindControl("lblCountryOrRegion");
                AccelaLabel lblPhone1 = (AccelaLabel)e.Row.FindControl("lblPhone1");
                AccelaLabel lblPhone2 = (AccelaLabel)e.Row.FindControl("lblPhone2");
                AccelaLabel lblFax = (AccelaLabel)e.Row.FindControl("lblFax");

                if (lblCountryOrRegion != null)
                {
                    lblCountryOrRegion.Text = StandardChoiceUtil.GetCountryByKey(lblCountryOrRegion.Text);
                }

                DataRow drPeople = (e.Row.DataItem as DataRowView).Row;
                lblPhone1.Text = ModelUIFormat.FormatPhoneShow(
                        drPeople[ColumnConstant.PeopleList.Phone1Code.ToString()].ToString(),
                        drPeople[ColumnConstant.PeopleList.Phone1.ToString()].ToString(),
                        drPeople[ColumnConstant.PeopleList.CountryOrRegion.ToString()].ToString());
                lblPhone2.Text = ModelUIFormat.FormatPhoneShow(
                        drPeople[ColumnConstant.PeopleList.Phone2Code.ToString()].ToString(),
                        drPeople[ColumnConstant.PeopleList.Phone2.ToString()].ToString(),
                        drPeople[ColumnConstant.PeopleList.CountryOrRegion.ToString()].ToString());
                lblFax.Text = ModelUIFormat.FormatPhoneShow(
                        drPeople[ColumnConstant.PeopleList.FaxCode.ToString()].ToString(),
                        drPeople[ColumnConstant.PeopleList.Fax.ToString()].ToString(),
                        drPeople[ColumnConstant.PeopleList.CountryOrRegion.ToString()].ToString());
            }
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //when export CSV form request, it need Re-bind GV.
            if (IsPostBack && Request.Form["__EVENTTARGET"] != null && Request.Form["__EVENTTARGET"].IndexOf("btnExport") > -1)
            {
                Bind();
            }
        }

        /// <summary>
        /// bind people list data source.
        /// </summary>
        private void Bind()
        {
            DataView dv = new DataView(DataSource);

            if (!string.IsNullOrEmpty(dgvPeopleList.GridViewSortExpression))
            {
                dv.Sort = dgvPeopleList.GridViewSortExpression + " " + dgvPeopleList.GridViewSortDirection;
            }

            dgvPeopleList.DataSource = dv.ToTable();
            dgvPeopleList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            dgvPeopleList.DataBind();
        }

        /// <summary>
        /// Construct People DataTable
        /// </summary>
        /// <returns>data table for People</returns>
        private DataTable ConstructLicenseDataTable()
        {
            DataTable licenseTable = new DataTable();

            licenseTable.Columns.Add(ColumnConstant.PeopleList.Type.ToString());
            licenseTable.Columns.Add(ColumnConstant.PeopleList.FirstName.ToString());
            licenseTable.Columns.Add(ColumnConstant.PeopleList.MiddleName.ToString());
            licenseTable.Columns.Add(ColumnConstant.PeopleList.LastName.ToString());
            licenseTable.Columns.Add(ColumnConstant.PeopleList.Address1.ToString());
            licenseTable.Columns.Add(ColumnConstant.PeopleList.Address2.ToString());
            licenseTable.Columns.Add(ColumnConstant.PeopleList.Address3.ToString());
            licenseTable.Columns.Add(ColumnConstant.PeopleList.Phone1.ToString());
            licenseTable.Columns.Add(ColumnConstant.PeopleList.Phone1Code.ToString());
            licenseTable.Columns.Add(ColumnConstant.PeopleList.Phone2.ToString());
            licenseTable.Columns.Add(ColumnConstant.PeopleList.Phone2Code.ToString());
            licenseTable.Columns.Add(ColumnConstant.PeopleList.Fax.ToString());
            licenseTable.Columns.Add(ColumnConstant.PeopleList.FaxCode.ToString());
            licenseTable.Columns.Add(ColumnConstant.PeopleList.Email.ToString());
            licenseTable.Columns.Add(ColumnConstant.PeopleList.LicenseNumber.ToString());
            licenseTable.Columns.Add(ColumnConstant.PeopleList.LicenseExpirationDate.ToString(), typeof(DateTime));
            licenseTable.Columns.Add(ColumnConstant.PeopleList.CountryOrRegion.ToString());
            licenseTable.Columns.Add(ColumnConstant.PeopleList.FullName.ToString());
            
            return licenseTable;
        }

        /// <summary>
        /// build data table for people list.
        /// </summary>
        /// <param name="trustAccountPeopleModels">Trust Account People Model array</param>
        /// <returns>dataTable for array</returns>
        private DataTable ConvertLicenseProfessionalDataTable(TrustAccountPeopleModel[] trustAccountPeopleModels)
        {
            DataTable dtLicenseProfessional = ConstructLicenseDataTable();

            if (trustAccountPeopleModels != null && trustAccountPeopleModels.Length > 0)
            {
                foreach (TrustAccountPeopleModel trustAccountPeople in trustAccountPeopleModels)
                {
                    if (trustAccountPeople == null)
                    {
                        continue;
                    }

                    DataRow drLicenseProfessional = dtLicenseProfessional.NewRow();

                    drLicenseProfessional[ColumnConstant.PeopleList.Type.ToString()] = trustAccountPeople.peopleType;
                    drLicenseProfessional[ColumnConstant.PeopleList.FirstName.ToString()] = trustAccountPeople.firstName;
                    drLicenseProfessional[ColumnConstant.PeopleList.MiddleName.ToString()] = trustAccountPeople.middleName;
                    drLicenseProfessional[ColumnConstant.PeopleList.LastName.ToString()] = trustAccountPeople.lastName;
                    drLicenseProfessional[ColumnConstant.PeopleList.Address1.ToString()] = trustAccountPeople.address1;
                    drLicenseProfessional[ColumnConstant.PeopleList.Address2.ToString()] = trustAccountPeople.address2;
                    drLicenseProfessional[ColumnConstant.PeopleList.Address3.ToString()] = trustAccountPeople.address3;
                    drLicenseProfessional[ColumnConstant.PeopleList.Phone1.ToString()] = trustAccountPeople.phone1;
                    drLicenseProfessional[ColumnConstant.PeopleList.Phone1Code.ToString()] = trustAccountPeople.phone1CountryCode;
                    drLicenseProfessional[ColumnConstant.PeopleList.Phone2.ToString()] = trustAccountPeople.phone2;
                    drLicenseProfessional[ColumnConstant.PeopleList.Phone2Code.ToString()] = trustAccountPeople.phone2CountryCode;
                    drLicenseProfessional[ColumnConstant.PeopleList.Fax.ToString()] = trustAccountPeople.fax;
                    drLicenseProfessional[ColumnConstant.PeopleList.FaxCode.ToString()] = trustAccountPeople.faxCountryCode;
                    drLicenseProfessional[ColumnConstant.PeopleList.Email.ToString()] = trustAccountPeople.email;
                    drLicenseProfessional[ColumnConstant.PeopleList.LicenseNumber.ToString()] = trustAccountPeople.licNbr;
                    drLicenseProfessional[ColumnConstant.PeopleList.LicenseExpirationDate.ToString()] = trustAccountPeople.expirationDate == null ? DBNull.Value : (object)trustAccountPeople.expirationDate;
                    drLicenseProfessional[ColumnConstant.PeopleList.CountryOrRegion.ToString()] = trustAccountPeople.countryCode;
                    string[] fullName = { trustAccountPeople.firstName, trustAccountPeople.middleName, trustAccountPeople.lastName };
                    drLicenseProfessional[ColumnConstant.PeopleList.FullName.ToString()] = string.IsNullOrEmpty(trustAccountPeople.fullName) ? DataUtil.ConcatStringWithSplitChar(fullName, ACAConstant.BLANK) : trustAccountPeople.fullName;

                    dtLicenseProfessional.Rows.Add(drLicenseProfessional);
                }
            }

            return dtLicenseProfessional;
        }

        #endregion Methods
    }
}
