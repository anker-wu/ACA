#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: LicenseList.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: LicenseList.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;  &lt;Who&gt;  &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// UC for display contact list.
    /// </summary>
    public partial class LicenseList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Command name.
        /// </summary>
        private const string COMMAND_SELECT_LICENSE = "SelectLicense";

        /// <summary>
        /// Command name for deleting a contact
        /// </summary>
        private const string COMMAND_DELETE_LICENSE = "DeleteLicense";

        /// <summary>
        /// the validate flag.
        /// </summary>
        private bool _validate = true;

        /// <summary>
        /// Indicate whether enable delete action or not.
        /// Disable in confirm page.
        /// </summary>
        private bool _enableDeleteAction = true;

        /// <summary>
        /// grid view row command event
        /// </summary>
        public event CommonEventHandler LicenseDeleted;

        /// <summary>
        /// grid view page index changing event.
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        /// <summary>
        /// grid view sorted event.
        /// </summary>
        public event GridViewSortedEventHandler GridViewSort;

        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets or sets cap contact data table.
        /// </summary>
        public DataTable DataSource
        {
            get
            {
                if (ViewState["LicenseListDataSource"] == null)
                {
                    ViewState["LicenseListDataSource"] = new DataTable();
                }

                return (DataTable)ViewState["LicenseListDataSource"];
            }

            set
            {
                ViewState["LicenseListDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets license list page index
        /// </summary>
        public int PageIndex
        {
            get
            {
                return gdvLicenseList.PageIndex;
            }

            set
            {
                gdvLicenseList.PageIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected license record index
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return ViewState["SelectedIndex"] == null ? -1 : Convert.ToInt32(ViewState["SelectedIndex"]);
            }

            set
            {
                ViewState["SelectedIndex"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the smart choice validate flag for license
        /// </summary>
        public bool IsValidate
        {
            get
            {
                return _validate;
            }

            set
            {
                _validate = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether of multiple LP form editable property.
        /// </summary>
        public bool IsEditable
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether enable delete action or not.
        /// </summary>
        public bool EnableDeleteAction
        {
            get
            {
                return _enableDeleteAction;
            }

            set
            {
                _enableDeleteAction = value;
            }
        }

        /// <summary>
        /// Gets or sets component name.
        /// </summary>
        public string ComponentName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets parent control id.
        /// </summary>
        public string ParentControlID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether LP list position is on Record Confirm page or not.
        /// </summary>
        public bool IsInCapConfirm
        {
            get;
            set;
        }

        /// <summary>
        /// Gets edit license function name in the list
        /// </summary>
        public string EditLicenseFunction
        {
            get
            {
                return ClientID + "_EditLicense";
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether all required field, False skip validate required field or not.
        /// </summary>
        protected bool IsSectionRequired
        {
            get;
            set;
        }

        /// <summary>
        /// Gets current SubAgencyCap type
        /// </summary>
        protected string IsSubAgencyCap
        {
            get { return Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// bind license list by license list data source.
        /// </summary>
        public void BindLicenseList()
        {
            DataTable dtMerged = GridViewBuildHelper.MergeTemplateAttributes2DataTable(gdvLicenseList, DataSource);

            gdvLicenseList.DataSource = dtMerged;
            gdvLicenseList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvLicenseList.DataBind();
            LicenseListPanel.Update();
        }

        /// <summary>
        /// Set the GridView required property
        /// </summary>
        /// <param name="isRequired">Indicate if the GridView is required</param>
        public void SetGridViewRequired(bool isRequired)
        {
            gdvLicenseList.IsRequired = isRequired;
            IsSectionRequired = isRequired;
        }

        /// <summary>
        /// <c>OnInit</c> event method
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.InitializeGridWithTemplate(gdvLicenseList, ModuleName, BizDomainConstant.STD_CAT_LICENSE_TYPE);

            base.OnInit(e);
        }

        /// <summary>
        /// GridView LicenseList row data bound event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void LicenseList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //alert a confirm message when delete a record.
                DataRowView dv = (DataRowView)e.Row.DataItem;
                AccelaDiv divImg = (AccelaDiv)e.Row.FindControl("divImg");
                AccelaLinkButton lnkDelete = (AccelaLinkButton)e.Row.FindControl("btnDelete");
                string deleteMessage = GetTextByKey("aca_common_delete_alert_message").Replace("'", "\\'");
                lnkDelete.Attributes.Add("onclick", string.Format("javascript:return confirm('{0}')", deleteMessage));
                LicenseProfessionalModel licenseProfessional = dv["LicenseProfessionalModel"] as LicenseProfessionalModel;
                
                if (!EnableDeleteAction || !IsEditable)
                {
                    // Hide delecte link button when in Confirm page or disable editable each record.
                    lnkDelete.CssClass = "ACA_Hide";
                }
                
                if ((IsValidate
                    && licenseProfessional != null
                    && (string.IsNullOrEmpty(licenseProfessional.licSeqNbr)
                        || ACAConstant.DAILY_LICENSE_NUMBER.Equals(licenseProfessional.licSeqNbr, StringComparison.InvariantCultureIgnoreCase)))
                    || !LicenseUtil.ValidateRequiredField4SingleLicense(licenseProfessional, ModuleName, IsEditable, IsValidate))
                {
                    divImg.Visible = true;
                }
                else
                {
                    divImg.Visible = false;
                }

                //display phone number like (+086)13299876658
                FormatPhoneShow((HiddenField)e.Row.FindControl("hdnFaxIDD"), (AccelaLabel)e.Row.FindControl("lblFax"), licenseProfessional.countryCode);
                FormatPhoneShow((HiddenField)e.Row.FindControl("hdnPhoneIDD"), (AccelaLabel)e.Row.FindControl("lblPhone"), licenseProfessional.countryCode);
                FormatPhoneShow((HiddenField)e.Row.FindControl("hdnMobilePhoneIDD"), (AccelaLabel)e.Row.FindControl("lblMobilePhone"), licenseProfessional.countryCode);
            }
        }

        /// <summary>
        /// GridView LicenseList row command event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void LicenseList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == COMMAND_SELECT_LICENSE)
            {
                //select a license to edit
                SelectLicense(sender, e);
            }
            else if (e.CommandName == COMMAND_DELETE_LICENSE)
            {
                //delete a contact
                int dataItemIndex = Convert.ToInt32(e.CommandArgument);
                DeleteLicense(sender, dataItemIndex);
            }
            else
            {
                BindLicenseList();
            }
        }

        /// <summary>
        /// fire sorted event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LicenseList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            if (GridViewSort != null)
            {
                DataSource = GridViewBuildHelper.MergeTemplateAttributes2DataTable(gdvLicenseList, DataSource);
                DataSource.DefaultView.Sort = e.GridViewSortExpression;
                DataSource = DataSource.DefaultView.ToTable();
                GridViewSort(sender, e);
            }
        }

        /// <summary>
        /// page index changing event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LicenseList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// Select a contact and raise contactSelected event
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        private void SelectLicense(object sender, GridViewCommandEventArgs e)
        {
            //Raise ContactSelected event
            int dataItemIndex = Convert.ToInt32(e.CommandArgument);
            DataRow drLicense = DataSource.Rows[dataItemIndex];
            SelectedIndex = Convert.ToInt32(drLicense["RowIndex"]);
            LicenseProfessionalModel licenseProfessionalModel = (LicenseProfessionalModel)drLicense["LicenseProfessionalModel"];

            if (IsInCapConfirm)
            {
                ComponentModel searchComponentModel = new ComponentModel();
                searchComponentModel.componentID = (long)PageFlowComponent.LICENSED_PROFESSIONAL_LIST;
                string sectionName = licenseProfessionalModel != null ? licenseProfessionalModel.componentName : string.Empty;
                PageFlowGroupModel pageflowGroup = AppSession.GetPageflowGroupFromSession();

                CapUtil.BackToPageContainSection(searchComponentModel, sectionName, pageflowGroup, SelectedIndex.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                AccelaLinkButton lnkEdit = e.CommandSource as AccelaLinkButton;
                string jsFunction = string.Format(
                                                  "SetLastFocus('{5}');{0}('{1}','{2}','{3}','{4}');",
                                                  EditLicenseFunction,
                                                  licenseProfessionalModel.licSeqNbr,
                                                  ScriptFilter.AntiXssJavaScriptEncode(licenseProfessionalModel.licenseNbr),
                                                  ScriptFilter.AntiXssJavaScriptEncode(licenseProfessionalModel.licenseType),
                                                  ScriptFilter.AntiXssJavaScriptEncode(licenseProfessionalModel.TemporaryID),
                                                  lnkEdit.ClientID);

                ScriptManager.RegisterStartupScript(Page, GetType(), ComponentName, jsFunction, true);
            }
        }

        /// <summary>
        /// Remove a license from contact list and raise the contacts changed event
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="dataItemIndex">the index a contact record in contact list</param>
        private void DeleteLicense(object sender, int dataItemIndex)
        {
            if (DataSource != null)
            {
                DataRow drLicense = DataSource.Rows[dataItemIndex];
                LicenseProfessionalModel licenseProfessionalModel = (LicenseProfessionalModel)drLicense["LicenseProfessionalModel"];

                DataTable dtLicenses = DataSource as DataTable;
                dtLicenses.Rows.RemoveAt(dataItemIndex);
                ListUtil.UpdateRowIndex(dtLicenses, "RowIndex");
                dtLicenses.AcceptChanges();
                DataSource = dtLicenses;
                BindLicenseList();

                if (LicenseDeleted != null)
                {
                    LicenseDeleted(sender, new CommonEventArgs(licenseProfessionalModel));
                }
            }
        }

        /// <summary>
        /// Format phone show
        /// </summary>
        /// <param name="hfnPhoneCode">control for phone code.</param>
        /// <param name="lblPhone">control for phone.</param>
        /// <param name="countryCode">The country code.</param>
        private void FormatPhoneShow(HiddenField hfnPhoneCode, AccelaLabel lblPhone, string countryCode)
        {
            string homePhoneCode = hfnPhoneCode != null ? hfnPhoneCode.Value : string.Empty;
            string homePhone = lblPhone != null ? lblPhone.Text : string.Empty;

            if (lblPhone != null)
            {
                lblPhone.Text = ModelUIFormat.FormatPhoneShow(homePhoneCode, homePhone, countryCode);
            }
        }
        #endregion Methods
    }
}
