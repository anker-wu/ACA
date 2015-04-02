#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ParcelEdit.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.Web.Component
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
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
    using Newtonsoft.Json;

    /// <summary>
    /// ParcelEdit user control, includes search feature. 
    /// User need to select one parcel information from search result,then fill the selected to edit area.
    /// this control should aggregate TemplateEdit control.
    /// </summary>
    public partial class ParcelEdit : FormDesignerBaseControl
    {
        #region Fields

        /// <summary>
        /// control value validate function.
        /// </summary>
        protected const string CONTROL_VALUE_VALIDATE_FUNCTION = "ParcelEdit_CheckControlValueValidate";

        /// <summary>
        /// template control value validate function.
        /// </summary>
        protected const string TEMPLETE_CONTROL_VALUE_VALIDATE_FUNCTION = "Templete_ParcelEdit_CheckControlValueValidate";

        /// <summary>
        /// indicate the the parcel form is editable or not.
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
        /// Initializes a new instance of the ParcelEdit class.
        /// </summary>
        public ParcelEdit()
            : base(GviewID.ParcelEdit)
        {
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the parcel edit is completed.
        /// </summary>
        public event CommonEventHandler ParceEditCompleted;

        #endregion Events

        #region Properties

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
        /// Sets a value indicating whetherTrue must fill all required field, False skip validate required field
        /// </summary>
        public bool IsSectionRequired
        {
            set
            {
                if (!value)
                {
                    //txtParcelNo.CheckControlValueValidateFunction = CONTROL_VALUE_VALIDATE_FUNCTION;
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
        /// Gets or sets a value indicating whether the SmartChoice validate flag for parcel
        /// </summary>
        public bool IsValidate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the data come from
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
        /// Gets or sets Permission
        /// </summary>
        protected override GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                base.Permission = new GFilterScreenPermissionModel4WS();
                base.Permission.permissionLevel = "APO";
                base.Permission.permissionValue = "PARCEL";
                return base.Permission;
            }

            set
            {
                base.Permission = value;
            }
        }

        /// <summary>
        /// Gets function name to update address and associates.
        /// </summary>
        protected string UpdateParcelAndAssociatesFunctionName
        {
            get
            {
                return ClientID + "_UpdateParcelAndAssociates";
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether address list is from map.
        /// </summary>
        private bool IsFromMap
        {
            get
            {
                if (ViewState["IsFromMap"] != null)
                {
                    return (bool)ViewState["IsFromMap"];
                }

                return false;
            }

            set
            {
                ViewState["IsFromMap"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether create cap from map or not.
        /// </summary>
        private bool IsCreateCapFromGIS
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsCreateCapFromGIS"]);
            }

            set
            {
                ViewState["IsCreateCapFromGIS"] = value;
            }
        }

        /// <summary>
        /// Gets all control ids that needn't to be controlled
        /// </summary>
        private string[] FilteredControlIDs
        {
            get
            {
                return new string[] { "mapParcel" };
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Display Parcel
        /// </summary>
        /// <param name="parcel">a CapParcelModel</param>
        /// <param name="isInConfirmPage">is InConfirmPage</param>
        public void DisplayParcel(CapParcelModel parcel, bool isInConfirmPage)
        {
            DisplayParcel(false, parcel, isInConfirmPage);
        }

        /// <summary>
        /// Presents the CapParcelModel information to control.
        /// </summary>
        /// <param name="isRefrenceParcel">Is ReferenceParcel</param>
        /// <param name="parcel">A CapParcelModel data to be presented.</param>
        /// <param name="isInConfirmPage">Is InConfirmPage Flag.</param>
        public void DisplayParcel(bool isRefrenceParcel, CapParcelModel parcel, bool isInConfirmPage)
        {
            imgErrorIcon.Visible = false;
            ucConditon.HideCondition();

            if (isInConfirmPage)
            {
                chkAutoFillParcelInfo.Visible = false;
                ddlAutoFillParcelInfo.Visible = false;
            }

            if (parcel == null)
            {
                DisplayParcel(isRefrenceParcel, null, null);
            }
            else
            {
                DisplayParcel(isRefrenceParcel, parcel.parcelModel, parcel.l1ParcelNo);

                if (parcel.parcelModel != null
                    && !string.IsNullOrEmpty(parcel.parcelModel.parcelNumber)
                    && !string.IsNullOrEmpty(parcel.l1ParcelNo))
                {
                    //Cache the reference data;
                    RefEntityCache = parcel.parcelModel;
                }
            }

            // disabled parcel section in when set the property in aca admin.
            if (!IsEditable && !AppSession.IsAdmin)
            {
                if (parcel != null && string.IsNullOrEmpty(parcel.l1ParcelNo))
                {
                    CreateOriginalParcelTemplate();
                }

                DisableParcelForm();
                btnSearch.Enabled = false;
                btnClear.Enabled = false;
            }

            if (AppSession.IsAdmin)
            {
                divAutoFill.Visible = true;
            }
        }

        /// <summary>
        /// Gets the parcel information from user entered. 
        /// </summary>
        /// <param name="capParcel">cap parcel model.</param>
        /// <returns>An CapParcelModel data.</returns>
        public CapParcelModel GetParcel(CapParcelModel capParcel)
        {
            if (capParcel == null)
            {
                capParcel = new CapParcelModel();
            }

            ParcelModel parcel = capParcel.parcelModel;
            this.GetParcelFormControl(ref parcel); 

            if (!string.IsNullOrEmpty(capParcel.l1ParcelNo))
            {
                //Merge the reference data and UI data to prevent lose the data which fields hidden by form designer.
                CapUtil.MergeRefDataToUIData<ParcelModel, ParcelModel>(
                    ref parcel,
                    "parcelList",
                    "parcelModel*",
                    string.Format("{0}{1}{2}{1}{3}", "sourceSeqNumber", ACAConstant.SPLIT_CHAR, "parcelNumber", "UID"),
                    string.Format("{0}{1}{2}{1}{3}", txtSourceSeq.Value, ACAConstant.SPLIT_CHAR, capParcel.l1ParcelNo, capParcel.UID),
                    this.ModuleName,
                    this.RefEntityCache,
                    this.Permission,
                    this.ViewId);
            }

            capParcel.parcelModel = parcel;
            capParcel.parcelNo = txtParcelNo.Text;
            capParcel.l1ParcelNo = txtRefParcelNo.Value;
            capParcel.UID = txtParcelUID.Value;
            capParcel.parcelModel.templates = templateEdit.GetAttributeModels();
            capParcel.parcelModel.duplicatedAPOKeys = JsonConvert.DeserializeObject<DuplicatedAPOKeyModel[]>(hfDuplicateParcelKeys.Value);

            this.ClearRefEntityCache();

            return capParcel;
        }

        /// <summary>
        /// validate parcel, if validate flag in smart choice is true, then 
        /// </summary>
        /// <returns>True if the parcel is valid</returns>
        public bool ValidateParcel()
        {
            bool isNotValidate = false;

            if (!IsEditable
                || !IsValidate
                || APOUtil.IsEmpty(GetParcelCriterias())
                || (!string.IsNullOrEmpty(txtRefParcelNo.Value) || !string.IsNullOrEmpty(txtParcelUID.Value)))
            {
                isNotValidate = true;
            }
            else
            {
                imgErrorIcon.Visible = true;
            }

            return isNotValidate;
        }

        /// <summary>
        /// Display Condition
        /// </summary>
        /// <param name="noticeConditions">notice condition</param>
        /// <param name="hightestCondition">highest Condition</param>
        /// <param name="conditionType">condition type</param>
        public void DisplayCondition(NoticeConditionModel[] noticeConditions, NoticeConditionModel hightestCondition, ConditionType conditionType)
        {
            ucConditon.IsShowCondition(noticeConditions, hightestCondition, ConditionType.Parcel);
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// <c>OnInit</c> event method.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!IsPostBack)
            {
                BindAutoFillItems();
                DropDownListBindUtil.BindSubdivision(ddlSubdivision);
            }

            ControlBuildHelper.AddValidationForStandardFields(GviewID.ParcelEdit, ModuleName, Permission, Controls);
            mapParcel.Visible = StandardChoiceUtil.IsShowMap4SelectObject(this.ModuleName);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["__EVENTTARGET"] != null && Request["__EVENTTARGET"].IndexOf("lnkParcelNumber") > -1)
            {
                ucConditon.HideCondition();
                ScriptManager.RegisterStartupScript(Page, GetType(), "InitialHideLinkCondtion", "initialParcelConditionStatus='0';", true);
            }

            ddlAutoFillParcelInfo.Enabled = false;

            if (!AppSession.IsAdmin)
            {
                ddlAutoFillParcelInfo.Attributes.Add("onchange", "ddlAutoFillParcelChanged();");
                chkAutoFillParcelInfo.Attributes.Add("onclick", "chkAutoFillParcelChanged();");
            }

            chkAutoFillParcelInfo.Attributes.Add("title", GetTextByKey(chkAutoFillParcelInfo.LabelKey));

            hlEnd.NextControlClientID = SkippingToParentClickID;
            //// Initial the PlaceHolder's properties
            phContent.TemplateControlIDPrefix = ACAConstant.CAP_PARCEL_TEMPLATE_FIELD_PREFIX;
            InitFormDesignerPlaceHolder(phContent);

            if (!IsPostBack)
            {
                InitializeFromSession();
            }

            InitButton();
        }

        /// <summary>
        /// Clear Button Click
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ClearButton_Click(object sender, EventArgs e)
        {
            ClearParcelForm();
            this.ClearRefEntityCache();
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            if (capModel.parcelModel != null)
            {
                capModel.parcelModel = null;
                AppSession.SetCapModelToSession(ModuleName, capModel);
            }

            ddlAutoFillParcelInfo.Enabled = chkAutoFillParcelInfo.Checked;
            hfDuplicateParcelKeys.Value = string.Empty;

            Page.FocusElement(btnSearch.ClientID);
        }

        /// <summary>
        /// Raise post back button click event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void PostbackButton_Click(object sender, EventArgs e)
        {
            if (Request.Form["__EVENTTARGET"].Replace("$", "_") == btnPostback.ClientID)
            {
                string args = Request.Form["__EVENTARGUMENT"];
                ACAGISModel model = SerializationUtil.XmlDeserialize(args, typeof(ACAGISModel)) as ACAGISModel;
                ParcelModel parcelmodel = new ParcelModel();
                if (model.GisObjects != null && model.GisObjects.Length == 1)
                {
                    parcelmodel.gisObjectList = model.GisObjects;
                }
                else if (model.ParcelModels != null && model.ParcelModels.Length == 1)
                {
                    parcelmodel = model.ParcelModels[0];
                }
                else
                {
                    return;
                }

                parcelmodel.auditStatus = ACAConstant.VALID_STATUS;
                ucConditon.HideCondition();
                IsCreateCapFromGIS = false;
                IsFromMap = true;

                // Save APO session parameter
                APOSessionParameterModel sessionParameter = GetAPOSessionParameter();
                sessionParameter.SearchCriterias = parcelmodel;
                AppSession.SetAPOSessionParameter(sessionParameter);

                // Open the search list page
                OpenSearchResultPage();

                mapParcel.CloseMap();
                mapPanel.Update();
            }
        }

        /// <summary>
        /// Search Button Click
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            IsFromMap = false;
            imgErrorIcon.Visible = false;
            ucConditon.HideCondition();
            IsCreateCapFromGIS = false;
            ParcelModel searchCriterias = GetParcelCriterias();

            // Check empty input on external source.
            if (StandardChoiceUtil.IsExternalParcelSource() && APOUtil.IsEmpty(searchCriterias))
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, GetTextByKey("aca_apo_msg_searchcriteria_required"));
                return;
            }

            // Save APO session parameter
            APOSessionParameterModel sessionParameter = GetAPOSessionParameter();
            sessionParameter.SearchCriterias = searchCriterias;
            AppSession.SetAPOSessionParameter(sessionParameter);

            // Open the search list page
            OpenSearchResultPage();
        }

        /// <summary>
        /// Update parcel and associated address/owner after selecting them.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void UpdateParcelAndAssociatesButton_Click(object sender, EventArgs e)
        {
            Page.FocusElement(btnSearch.ClientID);
            imgErrorIcon.Visible = false;

            APOSessionParameterModel sessionParameter = AppSession.GetAPOSessionParameter();

            if (!string.IsNullOrEmpty(sessionParameter.ErrorMessage))
            {
                // Fail to load data
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, sessionParameter.ErrorMessage);
                return;
            }

            ParcelModel parcel = sessionParameter.SelectedParcel;
            OwnerModel owner = sessionParameter.SelectedOwner;
            AddressModel address = sessionParameter.SelectedAddress;

            // Display condition
            if (sessionParameter.ConditionInfo != null)
            {
                ShowCondition((ParcelModel)sessionParameter.ConditionInfo);
            }

            // Display parcel information
            DisplayParcel(true, parcel, parcel.parcelNumber);

            // Cache the reference data.
            ParcelModel cacheParcel = new ParcelModel();
            GetParcelFormControl(ref cacheParcel);
            RefEntityCache = cacheParcel;

            // Display associated address and owner information
            if (ParceEditCompleted != null)
            {
                string duplicateAddressKey = address == null || address.duplicatedAPOKeys == null || address.duplicatedAPOKeys.Length == 0
                        ? string.Empty
                        : JsonConvert.SerializeObject(address.duplicatedAPOKeys);
                string duplicateOwnerKey = owner == null || owner.duplicatedAPOKeys == null || owner.duplicatedAPOKeys.Length == 0
                        ? string.Empty
                        : JsonConvert.SerializeObject(owner.duplicatedAPOKeys);

                Hashtable htArgs = new Hashtable(4);
                htArgs.Add("SequenceNumber", Convert.ToString(parcel.sourceSeqNumber));
                htArgs.Add("AddressID", address == null ? string.Empty : Convert.ToString(address.refAddressId));
                htArgs.Add("ParcelNumber", parcel.parcelNumber);
                htArgs.Add("OwnerNumber", owner == null ? string.Empty : Convert.ToString(owner.ownerNumber));
                htArgs.Add("AddressUID", address == null ? string.Empty : address.UID);
                htArgs.Add("ParcelUID", parcel.UID);
                htArgs.Add("OwnerUID", owner == null ? string.Empty : owner.UID);
                htArgs.Add("duplicateAddressKey", duplicateAddressKey);
                htArgs.Add("duplicateOwnerKey", duplicateOwnerKey);
                htArgs.Add("IsFromMap", IsFromMap);

                ParceEditCompleted(this, new CommonEventArgs(htArgs));
            }

            IsFromMap = false;
        }

        /// <summary>
        /// ShowOnMap event handler
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void MapParcel_ShowOnMap(object sender, EventArgs e)
        {
            ACAGISModel gisModel = GISUtil.CreateACAGISModel();
            gisModel.Context = mapParcel.AGISContext;
            if (AppSession.User.IsAnonymous)
            {
                gisModel.UserGroups.Add(GISUserGroup.Anonymous.ToString());
            }
            else
            {
                gisModel.UserGroups.Add(GISUserGroup.Register.ToString());
            }

            gisModel.IsHideSendFeatures = !IsEditable;

            GISUtil.SetPostUrl(this.Page, gisModel);
            gisModel.ModuleName = ModuleName;

            ParcelModel parcel = GetParcelCriterias();

            if (string.IsNullOrEmpty(parcel.parcelNumber)
                && string.IsNullOrEmpty(parcel.lot)
                && string.IsNullOrEmpty(parcel.block)
                && string.IsNullOrEmpty(parcel.subdivision)
                && string.IsNullOrEmpty(parcel.book)
                && string.IsNullOrEmpty(parcel.page)
                && string.IsNullOrEmpty(parcel.tract)
                && string.IsNullOrEmpty(parcel.legalDesc)
                && string.IsNullOrEmpty(txtParcelArea.Text)
                && string.IsNullOrEmpty(txtLandValue.Text)
                && string.IsNullOrEmpty(txtExceptionValue.Text)
                && string.IsNullOrEmpty(txtExceptionValue.Text)
                && templateEdit.IsControlsValueEmpty())
            {
                mapParcel.ACAGISModel = gisModel;
                return;
            }

            IAPOBll apoBll = (IAPOBll)ObjectFactory.GetObject(typeof(IAPOBll));
            SearchResultModel result = apoBll.GetParcelInfoByParcel(ConfigManager.AgencyCode, parcel, null, true);
            ParcelInfoModel[] parcelInfos = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);

            if (parcelInfos != null && parcelInfos.Length > 0)
            {
                List<ParcelModel> list = new List<ParcelModel>();
                foreach (ParcelInfoModel item in parcelInfos)
                {
                    if (item.parcelModel != null)
                    {
                        ParcelModel parcelmodel = new ParcelModel();
                        parcelmodel.parcelNumber = item.parcelModel.parcelNumber;
                        parcelmodel.unmaskedParcelNumber = item.parcelModel.unmaskedParcelNumber;
                        list.Add(parcelmodel);
                    }
                }

                gisModel.ParcelModels = list.ToArray();
            }

            mapParcel.ACAGISModel = gisModel;
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Bind Parcel item into dropdownlist.
        /// </summary>
        private void BindAutoFillItems()
        {
            IList<ListItem> items = new List<ListItem>();
            ListItem item = new ListItem();

            var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
            PublicUserModel4WS user = proxyUserRoleBll.GetCurrentUser(AppSession.GetCapModelFromSession(ModuleName));

            if (user.parcelList != null && user.parcelList.Length > 0)
            {
                foreach (ParcelModel parcel in user.parcelList)
                {
                    // Check the audit status.
                    if (parcel == null || !ACAConstant.VALID_STATUS.Equals(parcel.auditStatus))
                    {
                        continue;
                    }

                    item = new ListItem();
                    item.Text = parcel.parcelNumber;

                    AutoFillParameter parameter = new AutoFillParameter()
                    {
                        AutoFillType = ACAConstant.AutoFillType4SpearForm.Parcel,
                        SectionId = this.ID,
                        EntityId = parcel.sourceSeqNumber.HasValue ? parcel.sourceSeqNumber.ToString() : string.Empty,
                        EntityType = parcel.parcelNumber,
                        EntityRefId = parcel.UID
                    };

                    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                    item.Value = javaScriptSerializer.Serialize(parameter);
                    items.Add(item);
                }

                DropDownListBindUtil.BindDDL(items, this.ddlAutoFillParcelInfo, false);
            }

            if (items.Count <= 0 && !AppSession.IsAdmin)
            {
                chkAutoFillParcelInfo.Visible = false;
                divAutoFill.Visible = false;
            }
        }

        /// <summary>
        /// clear parcel data
        /// </summary>
        private void ClearParcelForm()
        {
            ControlUtil.ClearValue(this, FilteredControlIDs);

            hfDisableFormFlag.Value = string.Empty;
            txtRefParcelNo.Value = string.Empty;
            txtParcelUID.Value = string.Empty;
            ucConditon.HideCondition();

            EnableParcelForm();
        }

        /// <summary>
        /// Creates an original parcel template.
        /// </summary>
        private void CreateOriginalParcelTemplate()
        {
            ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
            TemplateAttributeModel[] attributes;

            attributes = templateBll.GetAPOTemplateAttributes(TemplateType.CAP_PARCEL, ConfigManager.AgencyCode, AppSession.User.PublicUserId);

            templateEdit.DisplayAttributes(attributes, ACAConstant.CAP_PARCEL_TEMPLATE_FIELD_PREFIX);

            //SimpleViewModel4WS simpleViewModel = GetSimpleViewModel();
            //templateEdit.LoadFields(simpleViewModel);
        }

        /// <summary>
        /// disable the parcel form.
        /// </summary>
        private void DisableParcelForm()
        {
            DisableEdit(this, FilteredControlIDs);
        }

        /// <summary>
        /// Enable Parcel Form
        /// </summary>
        private void EnableParcelForm()
        {
            EnableEdit(this, null);
        }

        /// <summary>
        /// Fill Template
        /// </summary>
        /// <param name="parcelNumber">Parcel number</param>
        /// <param name="attributes">Template attributes</param>
        private void FillTemplate(string parcelNumber, TemplateAttributeModel[] attributes)
        {
            ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();

            if (attributes == null)
            {
                attributes = templateBll.GetRefAPOTemplateAttributes(
                    TemplateType.CAP_PARCEL,
                    parcelNumber,
                    ConfigManager.AgencyCode,
                    AppSession.User.PublicUserId);
            }
            else
            {
                attributes = templateBll.FillTemplateDropDownList(TemplateType.CAP_PARCEL, attributes);
            }

            templateEdit.FillAttributeValues(attributes);
        }

        /// <summary>
        /// Get input parcel criteria.
        /// </summary>
        /// <returns>Parcel model</returns>
        private ParcelModel GetParcelCriterias()
        {
            // construct a parcel model to search APO.
            ParcelModel parcel = new ParcelModel();
            parcel.parcelNumber = txtParcelNo.Text;
            parcel.lot = txtLot.Text;
            parcel.block = txtBlock.Text;
            parcel.subdivision = ddlSubdivision.SelectedValue;
            parcel.book = txtBook.Text;
            parcel.page = txtPage.Text;
            parcel.tract = txtTract.Text;
            parcel.legalDesc = txtLegalDescription.Text;
            parcel.parcelArea = txtParcelArea.DoubleValue;
            parcel.landValue = txtLandValue.DoubleValue;
            parcel.improvedValue = txtImprovedValue.DoubleValue;
            parcel.exemptValue = txtExceptionValue.DoubleValue;
            parcel.parcelStatus = ACAConstant.VALID_STATUS;
            parcel.auditStatus = ACAConstant.VALID_STATUS;
            parcel.templates = templateEdit.GetAttributeModels(true);

            return parcel;
        }

        /// <summary>
        /// Initializes Parcel section by session.
        /// </summary>
        private void InitializeFromSession()
        {
            if (Session[SessionConstant.SESSION_CREATE_RECORD_BY_MAP] == null)
            {
                return;
            }

            ACAGISModel model = Session[SessionConstant.SESSION_CREATE_RECORD_BY_MAP] as ACAGISModel;

            if (model == null || model.ModuleName != this.ModuleName)
            {
                return;
            }

            if (model.ParcelInfoModels != null && model.ParcelInfoModels.Length > 1
                && ((model.GisObjects != null && model.GisObjects.Length == 1)
                    || (model.ParcelModels != null && model.ParcelModels.Length == 1)))
            {
                /*
                 * create cap from map,search apolist logic in GISPostBack.aspx page,data bind in parcel edit page
                 * so,need research data base on parcel list to support get more than 100 records. 
                 */
                IsFromMap = true;
                IsCreateCapFromGIS = true;
                GISUtil.RemoveACAGISModelFromSession(ModuleName);

                // Save search conditions
                APOSessionParameterModel sessionParameter = GetAPOSessionParameter();
                sessionParameter.SearchCriterias = model;
                AppSession.SetAPOSessionParameter(sessionParameter);

                // Open the search list page
                OpenSearchResultPage();
            }
        }

        /// <summary>
        /// Show condition
        /// </summary>
        /// <param name="parcel">a <see cref="ParcelModel"/></param>
        private void ShowCondition(ParcelModel parcel)
        {
            ConditionsUtil.ShowCondition(ucConditon, parcel);

            hfDuplicateParcelKeys.Value = (parcel.duplicatedAPOKeys == null || parcel.duplicatedAPOKeys.Length == 0) ? string.Empty : JsonConvert.SerializeObject(parcel.duplicatedAPOKeys);
        }

        /// <summary>
        /// Show parcel template fields
        /// </summary>
        /// <param name="isReferenceParcel">true is reference parcel info; false isn't reference parcel info</param>
        /// <param name="parcelModel">ParcelModel object</param>
        private void ShowTemplateFields(bool isReferenceParcel, ParcelModel parcelModel)
        {
            if (parcelModel == null)
            {
                return;
            }

            if (isReferenceParcel)
            {
                FillTemplate(parcelModel.parcelNumber, parcelModel.templates);
            }
            else
            {
                ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));

                if (AppSession.IsAdmin)
                {
                    TemplateAttributeModel[] attributes = templateBll.GetAPOTemplateAttributes(TemplateType.CAP_PARCEL, ConfigManager.AgencyCode, ACAConstant.ADMIN_CALLER_ID);
                    templateEdit.DisplayAttributes(attributes, ACAConstant.CAP_PARCEL_TEMPLATE_FIELD_PREFIX);
                }
                else
                {
                     //when resume an application or click back button or in confirm page(edit mode), get it from address model.
                    if (parcelModel.templates != null)
                    {
                        parcelModel.templates = templateBll.FillTemplateDropDownList(TemplateType.CAP_PARCEL, parcelModel.templates);
                        templateEdit.DisplayAttributes(parcelModel.templates, ACAConstant.CAP_PARCEL_TEMPLATE_FIELD_PREFIX);
                    }
                    else
                    {
                        //at first time in cap edit page, parcelModel.parcelAttribute is null for super agency.
                        TemplateAttributeModel[] attributes = templateBll.GetRefAPOTemplateAttributes(TemplateType.CAP_PARCEL, parcelModel.parcelNumber, ConfigManager.AgencyCode, AppSession.User.PublicUserId);

                        templateEdit.DisplayAttributes(attributes, ACAConstant.CAP_PARCEL_TEMPLATE_FIELD_PREFIX);
                    }
                }
            }
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
        /// Gets new APO session parameters.
        /// </summary>
        /// <returns>An APOSessionParameterModel</returns>
        private APOSessionParameterModel GetAPOSessionParameter()
        {
            APOSessionParameterModel sessionParameter = new APOSessionParameterModel();
            sessionParameter.IsCreateCapFromGIS = IsCreateCapFromGIS;
            sessionParameter.IsFromMap = IsFromMap;
            sessionParameter.IsValidate = IsValidate;
            sessionParameter.CallbackFunctionName = UpdateParcelAndAssociatesFunctionName;

            return sessionParameter;
        }

        /// <summary>
        /// Open the search result page
        /// </summary>
        private void OpenSearchResultPage()
        {
            string script = string.Format("{0}_OpenParcelSearchResult();", ClientID);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "OpenParcelSearchResult" + CommonUtil.GetRandomUniqueID(), script, true);
        }

        /// <summary>
        /// Display Parcel
        /// </summary>
        /// <param name="isRefrenceParcel">Indicate whether parcel is reference or not.</param>
        /// <param name="parcel">A ParcelModel</param>
        /// <param name="refParcelNo">Reference parcel No</param>
        private void DisplayParcel(bool isRefrenceParcel, ParcelModel parcel, string refParcelNo)
        {
            // template scene
            // scene 1: create a cap, first enter this page, need to get standard template to be displayed
            // scene 2: create a cap, when address search is clicked then fill parcel information, in this case, also need to
            //          get standard template but not retrive it from db.
            // scene 3: after saving, then enter this page again.
            // if parcel model is null,then init the control's initial value to resolve the page postback problem.
            if (parcel == null || parcel.parcelNumber == null)
            {
                ClearParcelForm();

                CreateOriginalParcelTemplate();
            }
            else
            {
                txtParcelNo.Text = parcel.parcelNumber;
                txtLot.Text = parcel.lot;
                txtBlock.Text = parcel.block;
                DropDownListBindUtil.SetSelectedValue(ddlSubdivision, parcel.subdivision);
                txtBook.Text = parcel.book;
                txtPage.Text = parcel.page;
                txtTract.Text = parcel.tract;
                txtLegalDescription.Text = parcel.legalDesc;
                txtParcelArea.DoubleValue = parcel.parcelArea;
                txtLandValue.DoubleValue = parcel.landValue;
                txtImprovedValue.DoubleValue = parcel.improvedValue;
                txtExceptionValue.DoubleValue = parcel.exemptValue;

                txtRefParcelNo.Value = refParcelNo;
                txtParcelUID.Value = parcel.UID;
                hfDuplicateParcelKeys.Value = (parcel.duplicatedAPOKeys == null || parcel.duplicatedAPOKeys.Length == 0) ? string.Empty : JsonConvert.SerializeObject(parcel.duplicatedAPOKeys);

                ShowTemplateFields(isRefrenceParcel, parcel);

                if (!string.IsNullOrEmpty(refParcelNo) || !string.IsNullOrEmpty(parcel.UID))
                {
                    if (IsValidate)
                    {
                        DisableParcelForm();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the parcel form control.
        /// </summary>
        /// <param name="parcel">The parcel.</param>
        private void GetParcelFormControl(ref ParcelModel parcel)
        {
            if (parcel == null)
            {
                parcel = new ParcelModel();
            }

            parcel.primaryParcelFlag = ACAConstant.COMMON_Y;
            parcel.parcelNumber = this.txtParcelNo.Text;
            parcel.UID = this.txtParcelUID.Value;
            parcel.lot = this.txtLot.Text;
            parcel.block = this.txtBlock.Text;
            parcel.subdivision = this.ddlSubdivision.SelectedValue;
            parcel.resSubdivision = this.GetDLLSelectedText(this.ddlSubdivision);
            parcel.book = this.txtBook.Text;
            parcel.page = this.txtPage.Text;
            parcel.tract = this.txtTract.Text;
            parcel.legalDesc = this.txtLegalDescription.Text;
            parcel.parcelArea = this.txtParcelArea.DoubleValue;
            parcel.landValue = this.txtLandValue.DoubleValue;
            parcel.improvedValue = this.txtImprovedValue.DoubleValue;
            parcel.exemptValue = this.txtExceptionValue.DoubleValue;
            parcel.auditID = AppSession.User.PublicUserId;
            parcel.auditStatus = ACAConstant.VALID_STATUS;
        }

        #endregion Private Methods
    }
}