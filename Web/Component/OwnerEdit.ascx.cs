#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: OwnerEdit.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: OwnerEdit.ascx.cs 77905 2007-10-15 12:49:28Z ACHIEVO\lytton.cheng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.FormDesigner;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using Newtonsoft.Json;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// OwnerEdit user control, includes search feature. 
    /// User need to select one owner information from search result,then fill the selected to edit area.
    /// this control should aggregate TemplateEdit control.
    /// </summary>
    public partial class OwnerEdit : FormDesignerBaseControl
    {
        #region Fields

        /// <summary>
        /// control value validate function.
        /// </summary>
        protected const string CONTROL_VALUE_VALIDATE_FUNCTION = "OwnerEdit_CheckControlValueValidate";

        /// <summary>
        /// template control value validate function.
        /// </summary>
        protected const string TEMPLETE_CONTROL_VALUE_VALIDATE_FUNCTION = "Templete_OwnerEdit_CheckControlValueValidate";

        /// <summary>
        /// indicate the the owner form is editable or not.
        /// </summary>
        private bool _isEditable = true;

        /// <summary>
        /// R-Reference,need reference data
        /// T-Transactional, user input data.
        /// N-No Limitation,both reference and daily
        /// </summary>
        private string _validateFlag = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the OwnerEdit class.
        /// </summary>
        public OwnerEdit()
            : base(GviewID.OwnerEdit)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        public DataTable GridViewDataSource
        {
            get
            {
                return (DataTable)ViewState["OwnerModels"];
            }

            set
            {
                ViewState["OwnerModels"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        public bool IsEditable
        {
            get
            {
                return _isEditable;
            }

            set
            {
                _isEditable = value;
            }
        }

        /// <summary>
        /// Sets a value indicating whether fill all required field, False skip validate required field
        /// </summary>
        public bool IsSectionRequired
        {
            set
            {
                if (!value)
                {
                    ControlBuildHelper.AddValidationFuctionForRequiredFields(Controls, CONTROL_VALUE_VALIDATE_FUNCTION);
                    templateEdit.ScriptName = TEMPLETE_CONTROL_VALUE_VALIDATE_FUNCTION;
                }
            }
        }

        /// <summary>
        /// Gets or sets focus click id.
        /// </summary>
        public string SkippingToParentClickID
        {
            get
            {
                return ViewState["SkippingToParentClickID"] as string;
            }

            set
            {
                ViewState["SkippingToParentClickID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the smart choice validate flag for owner
        /// </summary>
        public bool IsValidate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets data comes from
        /// </summary>
        public string ValidateFlag
        {
            get
            {
                return _validateFlag;
            }

            set
            {
                _validateFlag = value;

                if (ComponentDataSource.Reference.Equals(value))
                {
                    IsValidate = true;
                }
            }
        }

        /// <summary>
        /// Gets or Sets Permission
        /// </summary>
        protected override GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                base.Permission = new GFilterScreenPermissionModel4WS();
                base.Permission.permissionLevel = "APO";
                base.Permission.permissionValue = "OWNER";
                return base.Permission;
            }

            set
            {
                base.Permission = value;
            }
        }

        /// <summary>
        /// Gets or sets the Template JS function.
        /// </summary>
        protected string SetTemplateJsFunction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets all phone control id.
        /// </summary>
        private string RelevantControlIDs
        {
            get
            {
                StringBuilder sbControls = new StringBuilder();
                sbControls.Append(txtPhone.ClientID);
                sbControls.Append(",").Append(txtFax.ClientID);

                return sbControls.ToString();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Display Owner
        /// </summary>
        /// <param name="owner">a RefOwnerModel</param>
        /// <param name="isInConfirmPage">Is InConfirmPage</param>
        public void DisplayOwner(RefOwnerModel owner, bool isInConfirmPage)
        {
            DisplayOwner(false, owner, isInConfirmPage);
        }

        /// <summary>
        /// Presents the OwnerModel information to control.
        /// </summary>
        /// <param name="isReferenceOwner">is ReferenceOwner</param>
        /// <param name="owner">An OwnerModel data to be presented.</param>
        /// <param name="isInConfirmPage">Is InConfirmPage</param>
        public void DisplayOwner(bool isReferenceOwner, RefOwnerModel owner, bool isInConfirmPage)
        {
            imgErrorIcon.Visible = false;
            ucOwnerList.Visible = false;
            divResultNotice.Visible = false;
            ucConditon.HideCondition();

            DisplayOwner(isReferenceOwner, owner);

            IsAppliedRegional = true;

            if (isInConfirmPage)
            {
                chkAutoFillOwnerInfo.Visible = false;
                ddlAutoFillOwnerInfo.Visible = false;
            }

            if (!IsEditable && !AppSession.IsAdmin)
            {
                DisableOwnerForm(true);
                btnSearch.Enabled = false;
                btnClearAddress.Enabled = false;
            }

            if (ddlAutoFillOwnerInfo.Visible && !ddlAutoFillOwnerInfo.Enabled)
            {
                chkAutoFillOwnerInfo.Checked = false;
            }

            if (AppSession.IsAdmin)
            {
                ucOwnerList.Visible = true;
                ucOwnerList.BindDataSource(APOUtil.BuildOwnerDataTable(null));

                divAutoFill.Visible = true;
                chkAutoFillOwnerInfo.Visible = true;
                ddlAutoFillOwnerInfo.Visible = true;
            }

            //if daily site and standard choice display owner section is "N" search owner button is invisible.
            if (!AppSession.IsAdmin && !StandardChoiceUtil.IsDisplayOwnerSection())
            {
                btnSearch.Visible = false;
            }
        }

        /// <summary>
        /// Gets the owner information from user entered.
        /// </summary>
        /// <param name="owner">a RefOwnerModel</param>
        /// <returns>A daily OwnerModel data.</returns>
        public RefOwnerModel GetOwner(RefOwnerModel owner)
        {
            GetOwnerModelFromControl(ref owner);
            owner.templates = templateEdit.GetAttributeModels();

            if (!string.IsNullOrEmpty(hfAPOKeys.Value))
            {
                owner.duplicatedAPOKeys = JsonConvert.DeserializeObject<DuplicatedAPOKeyModel[]>(hfAPOKeys.Value);
            }

            if (!string.IsNullOrEmpty(hfRefOwnerId.Value))
            {
                //Merge the reference data and UI data to prevent lose the data which fields hidden by form designer.
                CapUtil.MergeRefDataToUIData<RefOwnerModel, OwnerModel>(
                    ref owner,
                    "ownerModel",
                    string.Empty,
                    string.Format("{0}{1}{2}{1}{3}", "sourceSeqNumber", ACAConstant.SPLIT_CHAR, "ownerNumber", "UID"),
                    string.Format("{0}{1}{2}{1}{3}", hfSourceSeq.Value, ACAConstant.SPLIT_CHAR, hfRefOwnerId.Value, owner.UID),
                    ModuleName,
                    RefEntityCache,
                    Permission,
                    ViewId);
            }

            ClearRefEntityCache();

            return owner;
        }

        /// <summary>
        /// validate owner, if validate flag in smart choice is true, then need to check the data if reference from AA
        /// </summary>
        /// <returns>True if the owner is valid,otherwise return false</returns>
        public bool ValidateOwner()
        {
            bool isNotValidate = false;

            //if not need validate or the data is refrence from AA or owner information configurate to hidden, return true
            if (!IsEditable
                || !IsValidate
                || GetOwnerValidateCondition()
                || (!string.IsNullOrEmpty(hfRefOwnerId.Value) || !string.IsNullOrEmpty(hfRefOwnerUID.Value))
                || !StandardChoiceUtil.IsDisplayOwnerSection())
            {
                isNotValidate = true;
            }
            else
            {
                imgErrorIcon.Visible = true;
                ucOwnerList.Visible = false;
                divResultNotice.Visible = false;
            }

            return isNotValidate;
        }

        /// <summary>
        /// <c>OnInit</c> event method.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ddlCountry.BindItems();

            // Bind state list
            if (!IsPostBack)
            {
                //Bind owner item into dropdownlist.
                BindAutoFillItems();

                // set default selected to first item.
                DropDownListBindUtil.SetSelectedToFirstItem(ddlAutoFillOwnerInfo);

                //convert ID to ClientID for finding the right control to populate the IDD code.
                ddlCountry.RelevantControlIDs = RelevantControlIDs;
            }

            ddlCountry.RegisterScripts();
            ddlCountry.SetCountryControls(txtZip, ddlAppState, txtPhone, txtFax);

            ControlBuildHelper.AddValidationForStandardFields(GviewID.OwnerEdit, ModuleName, Permission, Controls);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAppliedRegional)
            {
                ControlUtil.ApplyRegionalSetting(IsPostBack, false, true, !IsPostBack, ddlCountry);
            }

            ddlAutoFillOwnerInfo.Enabled = chkAutoFillOwnerInfo.Checked;

            if (!AppSession.IsAdmin)
            {
                ddlAutoFillOwnerInfo.Attributes.Add("onchange", "ddlAutoFillOwnerChanged();");
                chkAutoFillOwnerInfo.Attributes.Add("onclick", "chkAutoFillOwnerChanged();");
            }

            chkAutoFillOwnerInfo.Attributes.Add("title", GetTextByKey(chkAutoFillOwnerInfo.LabelKey));

            divLockIcon.Attributes.Add("title", LabelUtil.GetTextByKey("aca_condition_notice_locked", string.Empty));
            divHoldIcon.Attributes.Add("title", LabelUtil.GetTextByKey("aca_condition_notice_hold", string.Empty));
            divNoteIcon.Attributes.Add("title", LabelUtil.GetTextByKey("aca_condition_notice_note", string.Empty));
            hlEnd.NextControlClientID = SkippingToParentClickID;

            // Initial the PlaceHolder's properties
            phContent.TemplateControlIDPrefix = ACAConstant.CAP_OWNER_TEMPLATE_FIELD_PREFIX;
            InitFormDesignerPlaceHolder(phContent);

            InitButton();
        }

        /// <summary>
        /// ClearAddress Button Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ClearAddressButton_Click(object sender, EventArgs e)
        {
            ClearOwnerForm();
            ClearRefEntityCache();
            ControlUtil.ClearRegionalSetting(ddlCountry, false, ModuleName, Permission, ViewId);
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            if (capModel.ownerModel != null)
            {
                capModel.ownerModel = null;
                AppSession.SetCapModelToSession(ModuleName, capModel);
            }

            ddlAutoFillOwnerInfo.Enabled = chkAutoFillOwnerInfo.Checked;
            hfAPOKeys.Value = string.Empty;
            Page.FocusElement(btnSearch.ClientID);
        }

        /// <summary>
        /// Search Button Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            Page.FocusElement(btnSearch.ClientID);
            imgErrorIcon.Visible = false;
            ucConditon.HideCondition();
            ucOwnerList.OwnerDataSource = null;
            ucOwnerList.PageIndex = 0;
            SearchAPOList(0, null);
        }

        /// <summary>
        /// Occur after selecting a owner.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">A OwnerListSelectEventArgs</param>
        protected void Owner_Selected(object sender, OwnerListSelectEventArgs e)
        {
            // Hid conditions
            ucConditon.HideCondition();
            ScriptManager.RegisterStartupScript(Page, GetType(), "InitialHideLinkCondtion", "initialOwnerConditionStatus='0';", true);

            OwnerModel owner = e.SelectedOwner;
            Page.FocusElement(btnSearch.ClientID);
            imgErrorIcon.Visible = false;

            if (ShowCondition(Convert.ToString(owner.sourceSeqNumber), Convert.ToString(owner.ownerNumber), owner.UID, owner.duplicatedAPOKeys))
            {
                DisplayOwner(true, owner.ToRefOwnerModel());

                ucOwnerList.Visible = false;
                divResultNotice.Visible = false;

                // Cache the reference data.
                RefOwnerModel refOwner = new RefOwnerModel();
                GetOwnerModelFromControl(ref refOwner);
                RefEntityCache = refOwner;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "SetValue", "SetOwnerListVisible();", true);
            }

            ddlAutoFillOwnerInfo.Enabled = chkAutoFillOwnerInfo.Checked;
        }

        /// <summary>
        /// Owner List GridViewIndexChanging
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">A OwnerListSelectEventArgs</param>
        protected void Owner_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ucOwnerList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                SearchAPOList(e.NewPageIndex, pageInfo.SortExpression);
            }
        }

        /// <summary>
        /// PermitList GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Owner_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ucOwnerList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// Get owner name.
        /// </summary>
        /// <param name="ownerModel">OwnerModel object</param>
        /// <returns>owner name</returns>
        private static string GetOwnerName(OwnerModel ownerModel)
        {
            if (ownerModel == null)
            {
                return null;
            }

            string ownerName = string.Empty;

            if (string.IsNullOrEmpty(ownerModel.ownerFirstName)
                || string.IsNullOrEmpty(ownerModel.ownerLastName)
                || ownerModel.ownerFirstName.Trim().Length == 0
                || ownerModel.ownerLastName.Trim().Length == 0)
            {
                ownerName = ownerModel.ownerFullName;
            }
            else
            {
                ownerName = ownerModel.ownerFirstName + " " + ownerModel.ownerLastName;
            }

            return ownerName;
        }

        /// <summary>
        /// Bind owner item into dropdownlist.
        /// </summary>
        private void BindAutoFillItems()
        {
            IList<ListItem> items = new List<ListItem>();
            ListItem item = new ListItem();

            var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
            PublicUserModel4WS user = proxyUserRoleBll.GetCurrentUser(AppSession.GetCapModelFromSession(ModuleName));

            if (user.ownerModel != null && user.ownerModel.Length > 0)
            {
                foreach (OwnerModel ownerModel in user.ownerModel)
                {
                    // Check the audit status.
                    if (ownerModel == null || !ACAConstant.VALID_STATUS.Equals(ownerModel.auditStatus))
                    {
                        continue;
                    }

                    string ownerName = GetOwnerName(ownerModel); //Owner information.

                    if (string.IsNullOrEmpty(ownerName) || ownerName.Trim().Length == 0)
                    {
                        continue;
                    }

                    item = new ListItem();
                    item.Text = ownerName;

                    AutoFillParameter parameter = new AutoFillParameter()
                    {
                        AutoFillType = ACAConstant.AutoFillType4SpearForm.Owner,
                        SectionId = ID,
                        EntityId = ownerModel.sourceSeqNumber.HasValue ? ownerModel.sourceSeqNumber.ToString() : string.Empty,
                        EntityType = ownerModel.ownerNumber.HasValue ? ownerModel.ownerNumber.ToString() : string.Empty,
                        EntityRefId = ownerModel.UID
                    };

                    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                    item.Value = javaScriptSerializer.Serialize(parameter);

                    if (!items.Contains(item))
                    {
                        items.Add(item);
                    }
                }
                //// false indicates need not to add the default item '--select--'.
                DropDownListBindUtil.BindDDL(items, ddlAutoFillOwnerInfo, false);
            }

            if (items.Count <= 0)
            {
                chkAutoFillOwnerInfo.Visible = false;
                divAutoFill.Visible = false;
            }
        }

        /// <summary>
        /// clear address data
        /// </summary>
        private void ClearOwnerForm()
        {
            ControlUtil.ClearValue(this, null);
            hfRefOwnerId.Value = string.Empty;
            hfRefOwnerUID.Value = string.Empty;
            hfDisableFormFlag.Value = string.Empty;
            ucConditon.HideCondition();

            DisableOwnerForm(false);
        }

        /// <summary>
        /// Gets Owner search criteria
        /// </summary>
        /// <returns>An OwnerCompModel</returns>
        private OwnerCompModel GetOwnerCriterias()
        {
            OwnerCompModel ownerModel = new OwnerCompModel();
            ownerModel.ownerTitle = txtTitle.Text.Trim();
            ownerModel.ownerFullName = txtName.Text.Trim();
            ownerModel.mailAddress1 = txtAddress1.Text.Trim();
            ownerModel.mailAddress2 = txtAddress2.Text.Trim();
            ownerModel.mailAddress3 = txtAddress3.Text.Trim();
            ownerModel.mailCity = ControlUtil.GetControlValue(txtCity);
            ownerModel.mailZip = txtZip.GetZip(ddlCountry.SelectedValue.Trim());
            ownerModel.mailState = ControlUtil.GetControlValue(ddlAppState);
            ownerModel.fax = txtFax.GetPhone(ddlCountry.SelectedValue.Trim());
            ownerModel.faxCountryCode = txtFax.Visible ? txtFax.CountryCodeText.Trim() : string.Empty;
            ownerModel.phone = txtPhone.GetPhone(ddlCountry.SelectedValue.Trim());
            ownerModel.phoneCountryCode = txtPhone.Visible ? txtPhone.CountryCodeText.Trim() : string.Empty;
            ownerModel.email = txtEmail.Text.Trim();
            ownerModel.mailCountry = ControlUtil.GetControlValue(ddlCountry);
            ownerModel.templates = templateEdit.GetAttributeModels(true);
            ownerModel.ownerStatus = ACAConstant.VALID_STATUS;
            ownerModel.auditStatus = ACAConstant.VALID_STATUS;

            return ownerModel;
        }

        /// <summary>
        /// Create the data source for address list
        /// </summary>
        /// <param name="currentPageIndex">Current page index</param>
        /// <param name="sortExpression">Sort expression</param>
        /// <param name="searchCriterias">Owner search criteria</param>
        /// <returns>data source for address list</returns>
        private DataTable CreateDataSource(int currentPageIndex, string sortExpression, OwnerCompModel searchCriterias)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ucOwnerList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = ucOwnerList.PageSize;

            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
            SearchResultModel result = apoBll.GetRefOwnerList(ConfigManager.AgencyCode, searchCriterias, queryFormat);
            pageInfo.StartDBRow = result.startRow;

            DataTable dt = APOUtil.BuildOwnerDataTable(result.resultList);
            dt = PaginationUtil.MergeDataSource<DataTable>(ucOwnerList.OwnerDataSource, dt, pageInfo);

            if (dt.Columns.Contains("OwnerFullName"))
            {
                DataView dv = dt.DefaultView;
                dv.Sort = "OwnerFullName ASC";
                dt = dv.ToTable();
            }

            return dt;
        }

        /// <summary>
        /// Creates an original owner template.
        /// </summary>
        private void CreateOriginalOwnerTemplate()
        {
            ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
            TemplateAttributeModel[] attributes;

            attributes = templateBll.GetAPOTemplateAttributes(TemplateType.CAP_OWNER, ConfigManager.AgencyCode, AppSession.User.PublicUserId);

            templateEdit.DisplayAttributes(attributes, ACAConstant.CAP_OWNER_TEMPLATE_FIELD_PREFIX);            
        }

        /// <summary>
        /// when set the validate property true and editable property false in aca admin disable the owner form.
        /// </summary>
        /// <param name="disabled">Is disabled flag</param>
        private void DisableOwnerForm(bool disabled)
        {
            if (disabled)
            {
                DisableEdit(this, null);
            }
            else
            {
                EnableEdit(this, null);
            }
        }

        /// <summary>
        /// Fill Template
        /// </summary>
        /// <param name="ownerNumber">owner number</param>
        /// <param name="attributes">A TemplateAttributeModel array</param>
        private void FillTemplate(string ownerNumber, TemplateAttributeModel[] attributes)
        {
            ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();

            if (attributes == null)
            {
                attributes = templateBll.GetRefAPOTemplateAttributes(
                                                                     TemplateType.CAP_OWNER, 
                                                                     ownerNumber,
                                                                     ConfigManager.AgencyCode,
                                                                     AppSession.User.PublicUserId);
            }
            else
            {
                attributes = templateBll.FillTemplateDropDownList(TemplateType.CAP_OWNER, attributes);
            }

            templateEdit.FillAttributeValues(attributes);
        }

        /// <summary>
        /// Get record not found message
        /// </summary>
        /// <returns>message Information.</returns>
        private string GenerateNoSearchResultMessage()
        {
            if (IsValidate)
            {
                return GetTextByKey("per_ownerInfo_label_NoOwnerFound");
            }
            else
            {
                return GetTextByKey("per_ownerInfo_label_ManuallyEnterOwner");
            }
        }

        /// <summary>
        /// get owner condition.
        /// </summary>
        /// <param name="sourceSeqNumber">source sequence Number</param>
        /// <param name="ownerNumber">owner Number.</param>
        /// <param name="ownerUID">owner UID.</param>
        /// <param name="duplicateAPOkeys">Duplicated APO keys.</param>
        /// <returns>The owner model.</returns>
        private OwnerModel GetOwerCondition(string sourceSeqNumber, string ownerNumber, string ownerUID, DuplicatedAPOKeyModel[] duplicateAPOkeys)
        {
            IOwnerBll ownerBll = (IOwnerBll)ObjectFactory.GetObject(typeof(IOwnerBll));
            OwnerModel ownerModel;

            if (!CapUtil.IsSuperCAP(ModuleName))
            {
                ownerModel = ownerBll.GetOwnerCondition(ConfigManager.AgencyCode, sourceSeqNumber, ownerNumber, ownerUID);
                return ownerModel;
            }

            ownerModel = ownerBll.GetOwnerCondition(CapUtil.GetAgencyCodeList(ModuleName), sourceSeqNumber, ownerNumber, ownerUID, duplicateAPOkeys);
            return ownerModel;
        }

        /// <summary>
        /// If all owner's fields is empty then return true.
        /// </summary>
        /// <returns>isAllEmpty true or false</returns>
        private bool GetOwnerValidateCondition()
        {
            bool isAllEmpty = false;

            if (txtName.Text.Trim() == string.Empty && txtAddress1.Text.Trim() == string.Empty && txtAddress2.Text.Trim() == string.Empty && txtAddress3.Text.Trim() == string.Empty && txtZip.GetZip(ControlUtil.GetControlValue(ddlCountry)) == string.Empty
                && txtCity.Text.Trim() == string.Empty && ControlUtil.GetControlValue(ddlCountry) == string.Empty && templateEdit.IsControlsValueEmpty() && txtTitle.Text.Trim() == string.Empty && ddlAppState.Text.Trim() == string.Empty
                && txtFax.Text.Trim() == string.Empty && txtPhone.Text.Trim() == string.Empty && txtEmail.Text.Trim() == string.Empty)
            {
                isAllEmpty = true;
            }

            return isAllEmpty;
        }

        /// <summary>
        /// Set current City and State
        /// </summary>
        private void SetCurrentCityAndState()
        {
            AutoFillCityAndStateUtil.SetCurrentCity(txtCity, ModuleName);
            AutoFillCityAndStateUtil.SetCurrentState(ddlAppState, ModuleName);
        }

        /// <summary>
        /// if conditionModel is not null, show locked, hold or notice message.
        /// </summary>
        /// <param name="sourceSeqNumber">Source sequence number of APO</param>
        /// <param name="ownerNumber">the reference address id number</param>
        /// <param name="ownerUID">Owner unique ID for supporting XAPO</param>
        /// <param name="duplicateAPOkeys">Duplicated APO Keys</param>
        /// <returns>true or false.</returns>
        private bool ShowCondition(string sourceSeqNumber, string ownerNumber, string ownerUID, DuplicatedAPOKeyModel[] duplicateAPOkeys)
        {
            if (AppSession.IsAdmin || (string.IsNullOrEmpty(ownerNumber) && string.IsNullOrEmpty(ownerUID)))
            {
                return true;
            }

            OwnerModel refOwnerModel = GetOwerCondition(sourceSeqNumber, ownerNumber, ownerUID, duplicateAPOkeys);

            bool isLocked = false;

            if (refOwnerModel == null || refOwnerModel.noticeConditions == null || refOwnerModel.noticeConditions.Length == 0)
            {
                return true;
            }

            if (refOwnerModel.hightestCondition == null || string.IsNullOrEmpty(refOwnerModel.hightestCondition.impactCode))
            {
                return true;
            }

            isLocked = ucConditon.IsShowCondition(refOwnerModel.noticeConditions, refOwnerModel.hightestCondition, ConditionType.Owner);

            return !isLocked;
        }

        /// <summary>
        /// Show owner template fields
        /// </summary>
        /// <param name="isReferenceOwner">is reference owner</param>
        /// <param name="owner">a RefOwnerModel</param>
        private void ShowTemplateFields(bool isReferenceOwner, RefOwnerModel owner)
        {
            if (owner == null)
            {
                return;
            }

            // when select one record from reference owner list(search result)
            if (isReferenceOwner)
            {
                FillTemplate(Convert.ToString(owner.ownerNumber), owner.templates);
            }
            else
            {
                ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();

                if (AppSession.IsAdmin)
                {
                    TemplateAttributeModel[] attributes = templateBll.GetAPOTemplateAttributes(TemplateType.CAP_OWNER, ConfigManager.AgencyCode, ACAConstant.ADMIN_CALLER_ID);
                    templateEdit.DisplayAttributes(attributes, ACAConstant.CAP_OWNER_TEMPLATE_FIELD_PREFIX);
                }
                else
                {
                    if (owner.templates == null)
                    {
                        // null indicates that the cap is creating and enter cap edit page first time.
                        CreateOriginalOwnerTemplate();

                        // Fill the reference owner template data.
                        FillTemplate(Convert.ToString(owner.l1OwnerNumber), owner.templates);
                    }
                    else
                    {
                        owner.templates = templateBll.FillTemplateDropDownList(TemplateType.CAP_OWNER, owner.templates);

                        // when resume an application or click back button or in confirm page(edit mode), get it from owner model.
                        templateEdit.DisplayAttributes(owner.templates, ACAConstant.CAP_OWNER_TEMPLATE_FIELD_PREFIX);
                        SetTemplateJsFunction = templateEdit.TemplateScript;
                    }
                }
            }
        }

        /// <summary>
        /// Search APO List
        /// </summary>
        /// <param name="currentPageIndex">Current PageIndex</param>
        /// <param name="sortExpression">Sort Expression</param>
        private void SearchAPOList(int currentPageIndex, string sortExpression)
        {
            DataTable dataSource = null;
            OwnerCompModel searchCriterias = GetOwnerCriterias();

            // Check empty input on external source.
            if (StandardChoiceUtil.IsExternalOwnerSource() && APOUtil.IsEmpty(searchCriterias))
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, GetTextByKey("aca_apo_msg_searchcriteria_required"));
                return;
            }

            try
            {
                dataSource = CreateDataSource(currentPageIndex, sortExpression, searchCriterias);
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, ex.Message);
                ucOwnerList.Visible = false;
                divResultNotice.Visible = false;
                return;
            }

            if (dataSource.Rows.Count > 1)
            {
                //if more than one result is returned display them in the list
                ucOwnerList.Visible = true;
                ucOwnerList.BindDataSource(dataSource);
                lblResultNotice.Text = string.Format(GetTextByKey("per_owner_Result_label_Notice"), ucOwnerList.CountSummary);
                divResultNotice.Visible = true;

                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "GotoAPOResult", "scrollIntoView('lnkOwnerResult');", true);
            }
            else if (dataSource.Rows.Count == 1)
            {
                DataRow newRow = dataSource.Rows[0];
                OwnerModel owner = APOUtil.GetOwnerModel(newRow);

                //if only one result is returned just populate it into the work location section
                ucOwnerList.Visible = false;
                divResultNotice.Visible = false;

                if (ShowCondition(Convert.ToString(owner.sourceSeqNumber), Convert.ToString(owner.ownerNumber), owner.UID, owner.duplicatedAPOKeys))
                {
                    DisplayOwner(true, owner.ToRefOwnerModel());

                    // Cache the reference data.
                    RefOwnerModel refOwner = new RefOwnerModel();
                    GetOwnerModelFromControl(ref refOwner);
                    RefEntityCache = refOwner;

                    if (!string.IsNullOrEmpty(hfRefOwnerId.Value) || !string.IsNullOrEmpty(hfRefOwnerUID.Value))
                    {
                        if (IsValidate)
                        {
                            chkAutoFillOwnerInfo.Checked = false;
                        }
                    }
                }
            }
            else
            {
                //if no results is returned show error message according to the validate flag
                ucOwnerList.Visible = true;
                ucOwnerList.BindDataSource(dataSource);
                ucOwnerList.EmptyDataText = GenerateNoSearchResultMessage();
                lblResultNotice.Text = string.Format(GetTextByKey("per_owner_Result_label_Notice"), dataSource.Rows.Count);

                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "GotoAPOResult", "scrollIntoView('lnkOwnerResult');", true);
            }

            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "SetValue", "SetOwnerListVisible();", true);

            ddlAutoFillOwnerInfo.Enabled = chkAutoFillOwnerInfo.Checked;
        }

        /// <summary>
        /// when set component property data source 'Transactional',
        /// should hidden search button and clean button.
        /// </summary>
        private void InitButton()
        {
            if (ComponentDataSource.Transactional.Equals(ValidateFlag))
            {
                liSearchButton.Visible = false;
            }
        }

        /// <summary>
        /// Display an owner to edit form
        /// </summary>
        /// <param name="isReferenceOwner">A value indicating whether it is Reference owner</param>
        /// <param name="owner">A RefOwnerModel</param>
        private void DisplayOwner(bool isReferenceOwner, RefOwnerModel owner)
        {
            // if owner model is null,then init the control's initial value to resolve the page postback problem.
            if (owner == null)
            {
                ClearOwnerForm();

                CreateOriginalOwnerTemplate();
                DropDownListBindUtil.SetCountrySelectedValue(ddlCountry, string.Empty, true, true, false);

                if (!IsPostBack && !AppSession.IsAdmin)
                {
                    SetCurrentCityAndState();
                }
            }
            else
            {
                txtTitle.Text = owner.ownerTitle;
                txtName.Text = owner.ownerFullName;
                txtAddress1.Text = owner.mailAddress1;
                txtAddress2.Text = owner.mailAddress2;
                txtAddress3.Text = owner.mailAddress3;
                txtCity.Text = owner.mailCity;

                DropDownListBindUtil.SetCountrySelectedValue(ddlCountry, owner.mailCountry, false, true, false);
                txtZip.Text = ModelUIFormat.FormatZipShow(owner.mailZip, owner.mailCountry, false);
                ddlAppState.Text = owner.mailState;

                txtPhone.Text = ModelUIFormat.FormatPhone4EditPage(owner.phone, owner.mailCountry);
                txtPhone.CountryCodeText = owner.phoneCountryCode;
                txtFax.Text = ModelUIFormat.FormatPhone4EditPage(owner.fax, owner.mailCountry);
                txtFax.CountryCodeText = owner.faxCountryCode;
                txtEmail.Text = owner.email;
                hfAPOKeys.Value = (owner.duplicatedAPOKeys == null || owner.duplicatedAPOKeys.Length == 0) ? string.Empty : JsonConvert.SerializeObject(owner.duplicatedAPOKeys);

                ShowTemplateFields(isReferenceOwner, owner);

                if (owner.l1OwnerNumber != null || !string.IsNullOrEmpty(owner.UID))
                {
                    hfRefOwnerId.Value = Convert.ToString(owner.l1OwnerNumber);
                    hfRefOwnerUID.Value = owner.UID;

                    // Cache the reference data.
                    RefEntityCache = owner;

                    if (IsValidate)
                    {
                        txtZip.SetZipFromAA(owner.mailZip);
                        DisableOwnerForm(true);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the owner model from control.
        /// </summary>
        /// <param name="owner">The owner.</param>
        private void GetOwnerModelFromControl(ref RefOwnerModel owner)
        {
            if (owner == null)
            {
                owner = new RefOwnerModel();
            }

            //Implements the feature 09ACC-05987_APO_Primary_Flag.
            //Set the primary flag to 'Y' when applying for a permit in ACA
            owner.primaryOwner = ACAConstant.COMMON_Y;
            owner.ownerTitle = txtTitle.Text.Trim();
            owner.ownerFullName = txtName.Text.Trim();
            owner.mailAddress1 = txtAddress1.Text.Trim();
            owner.mailAddress2 = txtAddress2.Text.Trim();
            owner.mailAddress3 = txtAddress3.Text.Trim();
            owner.mailCity = txtCity.Text.Trim();
            owner.mailZip = txtZip.GetZip(ddlCountry.SelectedValue.Trim());
            owner.mailState = ddlAppState.Text;
            owner.resState = ddlAppState.ResText;
            owner.mailCountry = ddlCountry.SelectedValue;
            owner.phone = txtPhone.GetPhone(ddlCountry.SelectedValue.Trim());
            owner.phoneCountryCode = txtPhone.CountryCodeText.Trim();
            owner.fax = txtFax.GetPhone(ddlCountry.SelectedValue.Trim());
            owner.faxCountryCode = txtFax.CountryCodeText.Trim();
            owner.email = txtEmail.Text.Trim();
            owner.l1OwnerNumber = StringUtil.ToDouble(hfRefOwnerId.Value);
            owner.UID = hfRefOwnerUID.Value;
            owner.auditID = AppSession.User.PublicUserId;
            owner.auditStatus = ACAConstant.VALID_STATUS;
        }

        #endregion Methods
    }
}