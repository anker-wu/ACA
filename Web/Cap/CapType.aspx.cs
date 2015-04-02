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
 *      $Id: CapType.aspx.cs 278183 2014-08-28 10:13:42Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// This class provide the ability to operation CAP type. 
    /// </summary>
    public partial class CapType : BasePage
    {
        #region Fields

        /// <summary>
        /// is for fee estimator
        /// </summary>
        private const string IS4FEE_ESTIMATOR = "IS4FEE_ESTIMATOR";

        /// <summary>
        /// selected license model
        /// </summary>
        private const string SELECTED_LICENSE_MODEL = "SELECTED_LICENSE_MODEL";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether fee estimator.
        /// </summary>
        private bool Is4FeeEstimator
        {
            get
            {
                if (ViewState[IS4FEE_ESTIMATOR] == null)
                {
                    ViewState[IS4FEE_ESTIMATOR] = false;
                }

                return (bool)ViewState[IS4FEE_ESTIMATOR];
            }

            set
            {
                ViewState[IS4FEE_ESTIMATOR] = value;
            }
        }

        /// <summary>
        /// Gets or sets selected license model
        /// </summary>
        private LicenseModel4WS SelectedLicenseModel
        {
            get
            {
                if (ViewState[SELECTED_LICENSE_MODEL] == null)
                {
                    return null;
                }

                return (LicenseModel4WS)ViewState[SELECTED_LICENSE_MODEL];
            }

            set
            {
                ViewState[SELECTED_LICENSE_MODEL] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // All public users are not allowed to access this page directly, but access from deep link.
            if (Request.UrlReferrer == null && Session[SessionConstant.FROM_DEEP_LINK] == null)
            {
                Response.Redirect(ACAConstant.URL_DEFAULT);
            }

            string from = Request.QueryString["isFeeEstimator"];
            string filterName = Request.QueryString["FilterName"];

            if (!IsPostBack)
            {
                btnContinue.AccessKey = AccessibilityUtil.GetAccessKey(AccessKeyType.SubmitForm);

                if (!AppSession.IsAdmin)
                {
                    BreadCrumpToolBar.PageFlow = BreadCrumpUtil.GetPageFlowConfig(this.ModuleName, false);
                }

                if (StandardChoiceUtil.IsSuperAgency() && !CloneRecordUtil.IsCloneRecord(Request))
                {
                    // Forbiden user to come back from cap edit page under super agency
                    Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                    Response.Cache.SetNoStore();

                    bool isForLocation = !ValidationUtil.IsYes(Request.QueryString["createRecordByService"]);
                    string url = string.Format("{0}{1}{2}{3}{4}", "WorkLocation.aspx?Module=", ScriptFilter.EncodeUrlEx(ModuleName), ACAConstant.AMPERSAND, "FilterName=", ScriptFilter.EncodeUrlEx(filterName));

                    // When no Selected Address or Services, return back to WorkLocation Lookup Page
                    if (AppSession.GetSelectedServicesFromSession() == null || ((AppSession.SelectedParcelInfo == null) && isForLocation))
                    {
                        if (!isForLocation)
                        {
                            url += "&createRecordByService=" + ACAConstant.COMMON_YES;
                        }

                        Response.Redirect(url);

                        return;
                    }
                }

                if (ACAConstant.COMMON_Y.Equals(from, StringComparison.InvariantCulture))
                {
                    Is4FeeEstimator = true;
                }
                else
                {
                    Is4FeeEstimator = false;
                }

                UserLicenseList prePage = PreviousPage as UserLicenseList;

                if (prePage != null)
                {
                    SelectedLicenseModel = prePage.SelectedLicenseModel;
                }

                // get selected trade name when creating a trade license
                if (filterName == ACAConstant.REQUEST_PARMETER_TRADE_LICENSE)
                {
                    ILicenseBLL licenseBll = ObjectFactory.GetObject<ILicenseBLL>();
                    LicenseModel4WS licenseModule = new LicenseModel4WS();
                    licenseModule.serviceProviderCode = ConfigManager.AgencyCode;
                    licenseModule.stateLicense = Request.QueryString["licenseNumber"];
                    licenseModule.licenseType = Request.QueryString["licenseType"];
                    SelectedLicenseModel = licenseBll.GetLicenseByStateLicNbr(licenseModule);
                }

                //User source record's record type when user is cloning a record. so redirect to capedit page.
                if (!CloneRecordUtil.IsCloneRecord(Request))
                {
                    // Cap home don't need cap model session, so here need to clear session
                    AppSession.SetCapModelToSession(ModuleName, null);
                    AppSession.SetPageflowGroupToSession(null);
                }

                //Clear the parent info to session for the Associated Forms.
                CapUtil.ClearAssoFormParentFromAppSession();

                //Clear the Parcel PK Model in Session.
                Session[SessionConstant.APO_SESSION_PARCELMODEL] = null;

                //Clear from deep link access session.
                Session[SessionConstant.FROM_DEEP_LINK] = null;

                if (AppSession.IsAdmin)
                {
                    PermitTypeSelect.Visible = false;
                    this.ddlBoardTypeSelection.Visible = false;
                }
                else
                {
                    InitPermitType();
                }
            }

            btnContinue.LabelKey = CapUtil.GetContinueButtonLabelKey(ModuleName, from);
            DisplayAmendmentUIMsg();

            ddlBoardTypeSelection.ToolTip = GetTextByKey("aca_cap_type_board_type_title");
        }

        /// <summary>
        /// Continue Button click event method
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PermitTypeSelect.SelectedValue))
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("per_applyPermit_error_selectType"));
                return;
            }

            SkipToCapEditPage();
        }

        /// <summary>
        /// The event handler for current Board Type change
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BoardTypeSelectionCheckBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vchType = GetVCHType();
            string filterName = GetFilterName();
            ShowCapTypes(vchType, filterName, this.ddlBoardTypeSelection.SelectedValue);
        }

        /// <summary>
        /// Skip to cap type page when select one cap type or only one cap type.
        /// </summary>
        private void SkipToCapEditPage()
        {
            CapTypeModel capType = CapUtil.CreateNewCapType(PermitTypeSelect.SelectedValue, PermitTypeSelect.SelectedItem.ToString(), ModuleName);
            PageFlowGroupModel pageflowGroup = CreatePageflowToSession(capType);

            if (pageflowGroup == null || pageflowGroup.stepList == null || pageflowGroup.stepList.Length < 1)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("per_applyPermit_error_noRelatedPageflowGroup"));
            }
            else
            {
                SetupCapType();
            }
        }

        /// <summary>
        /// Rest Cloned cap model to session.
        /// </summary>
        /// <returns>true or false.</returns>
        private bool ResetCloneCapModelToSession()
        {
            CapModel4WS cap = AppSession.GetCapModelFromSession(ModuleName);
            CreatePageflowToSession(cap.capType);
            cap = ResetCloneLP(cap);

            cap.auditID = AppSession.User.PublicUserId;
            cap.createdBy = string.IsNullOrEmpty(Request.QueryString["createdBy"]) ? AppSession.User.PublicUserId : Request.QueryString["createdBy"];

            AppSession.SetCapModelToSession(ModuleName, cap);
            
            return true;
        }

        /// <summary>
        /// Create cap mode to session.
        /// </summary>
        /// <returns>true or false.</returns>
        private bool CreateCapModeToSession()
        {
            CapTypeModel capType = CapUtil.CreateNewCapType(
                PermitTypeSelect.SelectedValue,
                PermitTypeSelect.SelectedItem.ToString(),
                ModuleName);

            if (!LicenseUtil.IsAvailableLicense(SelectedLicenseModel, capType))
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("per_applypermit_selecttype_error_unavailablelicense"));
                return false;
            }

            PageFlowGroupModel pageflowGroup = CreatePageflowToSession(capType);
            if (pageflowGroup == null || pageflowGroup.stepList == null || pageflowGroup.stepList.Length < 1)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("per_applyPermit_error_noRelatedPageflowGroup"));
                return false;
            }

            if (SelectedLicenseModel != null)
            {
                bool hasLPComonent = PageFlowUtil.IsComponentExist(GViewConstant.SECTION_LICENSE) || PageFlowUtil.IsComponentExist(GViewConstant.SECTION_MULTIPLE_LICENSES);

                // if has no LP component in page flow,clear the selected license.
                if (!hasLPComonent)
                {
                    SelectedLicenseModel = null;
                }
            }

            CapModel4WS capModel4WS = new CapModel4WS();
            capModel4WS.capType = capType;

            if (Request.QueryString["parentCapModelID"] != null && Request.QueryString["parentCapModelID"].ToString() != string.Empty)
            {
                CapIDModel4WS parentCapIdModel = new CapIDModel4WS();
                string customID = Request.QueryString["parentCapModelID"].ToString();
                parentCapIdModel.customID = customID;
                string[] idParameters = customID.Split('-');
                parentCapIdModel.id1 = idParameters[0];
                parentCapIdModel.id2 = idParameters[1];
                parentCapIdModel.id3 = idParameters[2];
                parentCapIdModel.serviceProviderCode = ConfigManager.AgencyCode;

                if (Request.QueryString["trackingID"] != null && Request.QueryString["trackingID"].ToString() != string.Empty)
                {
                    parentCapIdModel.trackingID = long.Parse(Request.QueryString["trackingID"]);
                }

                capModel4WS.parentCapID = parentCapIdModel;
                AppSession.SetParentCapIDModelToSession(ACAConstant.CAP_RELATIONSHIP_AMENDMENT, parentCapIdModel);
            }

            capModel4WS.licSeqNbr = SelectedLicenseModel == null ? null : SelectedLicenseModel.licSeqNbr;
            capModel4WS.licenseProfessionalModel = TempModelConvert.ConvertToLicenseProfessionalModel4WS(CapUtil.CreateLicenseProfessionalModel(SelectedLicenseModel));

            capModel4WS.auditID = AppSession.User.PublicUserId;

            //This partial cap will don't display in ACA, unless public user click save and resume button to save.
            capModel4WS.accessByACA = ACAConstant.COMMON_N;
            capModel4WS.capClass = ACAConstant.INCOMPLETE_TEMP_CAP; //This is temporary cap flag. This cap be saved, but it can't by view in ACA.

            capModel4WS.createdBy = string.IsNullOrEmpty(Request.QueryString["createdBy"]) ? AppSession.User.PublicUserId : Request.QueryString["createdBy"];
          
            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
            
            ServiceModel[] services = AppSession.GetSelectedServicesFromSession();

            // if current agency is super agency,create parent cap and children caps.
            if (StandardChoiceUtil.IsSuperAgency())
            {
                SetAPOFromWorkLocation(capModel4WS);
                capModel4WS = capBll.CreatePartialCaps(capModel4WS, services, Is4FeeEstimator);
            }
            else
            {
                //Super agency site select one service, which have page flow.
                if (services != null && services.Length > 0 && ValidationUtil.IsYes(Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]))
                {
                    SetAPOFromWorkLocation(capModel4WS);
                    capModel4WS.service = services[0];

                    // Because no license select page in super agency environment, so if the page flow contains License section, auto copy associated license to License section.
                    if (PageFlowUtil.IsComponentExist(GViewConstant.SECTION_LICENSE) || PageFlowUtil.IsComponentExist(GViewConstant.SECTION_MULTIPLE_LICENSES))
                    {
                        capModel4WS.licenseProfessionalList = GetUserLicenseList(capModel4WS);
                    }
                }

                capModel4WS = capBll.CreateWrapperForPartialCap(ConfigManager.AgencyCode, capModel4WS, AppSession.User.PublicUserId, string.Empty, Is4FeeEstimator);
            }

            CapWithConditionModel4WS capWithConditionModel4WS = capBll.GetCapViewBySingle(capModel4WS.capID, AppSession.User.UserSeqNum, ACAConstant.COMMON_N, StandardChoiceUtil.IsSuperAgency());
            capModel4WS = capWithConditionModel4WS.capModel;

            if (capModel4WS.licenseProfessionalList != null && capModel4WS.licenseProfessionalList.Length > 0)
            {
                foreach (LicenseProfessionalModel4WS item in capModel4WS.licenseProfessionalList)
                {
                    if (string.IsNullOrEmpty(item.agencyCode))
                    {
                        item.agencyCode = item.capID.serviceProviderCode;
                    }
                }

                capModel4WS.licenseProfessionalModel = capModel4WS.licenseProfessionalList[0];
            }

            capModel4WS.applicantModel = null;

            PeopleModel4WS peopleModel = GetPeopleModel();

            if (peopleModel != null)
            {
                peopleModel.contactType = string.Empty;
                SetReferenceContactToFirstContactComponent(pageflowGroup, peopleModel, capModel4WS);
            }

            if (capModel4WS.licenseProfessionalList != null && capModel4WS.licenseProfessionalList.Length != 0)
            {
                foreach (var licenseProfessionalModel in capModel4WS.licenseProfessionalList)
                {
                    licenseProfessionalModel.TemporaryID = CommonUtil.GetRandomUniqueID();
                }
            }

            AppSession.SetCapModelToSession(ModuleName, capModel4WS);

            pageflowGroup = CapUtil.GetPageFlowWithoutBlankPage(capModel4WS, pageflowGroup);
            AppSession.SetPageflowGroupToSession(pageflowGroup);

            return true;
        }

        /// <summary>
        /// Get people model.
        /// </summary>
        /// <returns>The people model.</returns>
        private PeopleModel4WS GetPeopleModel()
        {
            string contactSeqNbr = Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER];
            PeopleModel4WS people = null;

            if (!string.IsNullOrEmpty(contactSeqNbr))
            {
                // authorized agent, NOT use the current user's people model.
                if (AppSession.User.IsAuthorizedAgent || AppSession.User.IsAgentClerk)
                {
                    PeopleModel temp = AppSession.GetPeopleModelFromSession(contactSeqNbr);
                    people = TempModelConvert.ConvertToPeopleModel4WS(temp);
                }
                else
                {
                    PublicUserModel4WS model = AppSession.User.UserModel4WS;

                    if (model.peopleModel != null && model.peopleModel.Count(p => contactSeqNbr.Equals(p.contactSeqNumber)) > 0)
                    {
                        people = model.peopleModel.Where(p => contactSeqNbr.Equals(p.contactSeqNumber)).Single();
                    }
                }
            }

            return ObjectCloneUtil.DeepCopy(people);
        }

        /// <summary>
        /// Set APO From work location page.
        /// </summary>
        /// <param name="capModel4WS">The cap model.</param>
        private void SetAPOFromWorkLocation(CapModel4WS capModel4WS)
        {
            ParcelInfoModel parcelInfo = AppSession.SelectedParcelInfo;

            // Save Selected Address into Parent Cap Model on Super Agency.
            if (parcelInfo != null)
            {
                RefAddressModel refAddressModel = parcelInfo.RAddressModel;

                if (ValidationUtil.IsYes(Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]) && refAddressModel != null
                    && refAddressModel.duplicatedAPOKeys != null && refAddressModel.duplicatedAPOKeys.Length > 0)
                {
                    //select one service with page flow to apply a record, get subagency address information.
                    IAPOBll apoBll = (IAPOBll)ObjectFactory.GetObject(typeof(IAPOBll));
                    refAddressModel.duplicatedAPOKeys = null;
                    refAddressModel.sourceNumber = null;
                    refAddressModel.refAddressId = null;
                    SearchResultModel result = apoBll.GetParcelInfoByAddress(ConfigManager.AgencyCode, refAddressModel, null, true);
                    ParcelInfoModel[] parcelInfos = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);

                    if (parcelInfos != null && parcelInfos.Length > 0)
                    {
                        parcelInfo = parcelInfos[0];
                    }
                }

                capModel4WS.addressModel = CapUtil.ConvertRefAddressModel2AddressModel(parcelInfo.RAddressModel);
                capModel4WS.addressModel.serviceProviderCode = ConfigManager.AgencyCode;
                capModel4WS.parcelModel = CapUtil.ConvertParcelModel2CapParcelModel(parcelInfo.parcelModel);
                capModel4WS.ownerModel = CapUtil.ConvertOwnerModel2RefOwnerModel(parcelInfo.ownerModel);
            }
        }

        /// <summary>
        /// Get User license professional list.
        /// </summary>
        /// <param name="capModel4WS">the cap model.</param>
        /// <returns>the license professional list.</returns>
        private LicenseProfessionalModel4WS[] GetUserLicenseList(CapModel4WS capModel4WS)
        {
            PublicUserModel4WS user = new PublicUserModel4WS();

            if (capModel4WS.createdBy != AppSession.User.PublicUserId)
            {
                user = AppSession.User.UserModel4WS.initialUsers.Where(p => p.userSeqNum == capModel4WS.createdBy.Replace(ACAConstant.PUBLIC_USER_NAME, string.Empty)).Single();
            }
            else
            {
                user = AppSession.User.UserModel4WS;
            }

            List<LicenseProfessionalModel4WS> userLPs = new List<LicenseProfessionalModel4WS>();

            if (user.licenseModel != null && user.licenseModel.Count() > 0)
            {
                foreach (LicenseModel4WS license in user.licenseModel)
                {
                    userLPs.Add(TempModelConvert.ConvertToLicenseProfessionalModel4WS(CapUtil.CreateLicenseProfessionalModel(license)));
                }
            }

            return userLPs == null ? null : userLPs.ToArray();
        }

        /// <summary>
        /// create page flow to session.
        /// </summary>
        /// <param name="capType">CapTypeModel object.</param>
        /// <returns>page flow group model</returns>
        private PageFlowGroupModel CreatePageflowToSession(CapTypeModel capType)
        {
            string isCloningRecord = Request.QueryString[ACAConstant.IS_CLONE_RECORD] == null ? string.Empty : Request.QueryString[ACAConstant.IS_CLONE_RECORD].ToString();

            if (string.IsNullOrEmpty(PermitTypeSelect.SelectedValue) && !ACAConstant.COMMON_TRUE.Equals(isCloningRecord))
            {
                return null;
            }

            IPageflowBll pageflowBll = ObjectFactory.GetObject<IPageflowBll>();
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            PageFlowGroupModel pageflowGroup = pageflowBll.GetPageflowGroupByCapType(capType);

            //Get parent page flow for clone.
            if (capModel != null && capModel.capID != null && pageflowGroup == null)
            {
                if (Request.QueryString[UrlConstant.PAGEFLOW_GROUP_CODE] == null)
                {
                    return null;
                }

                string pageFlowGroupCode = Request.QueryString[UrlConstant.PAGEFLOW_GROUP_CODE];
                pageflowGroup = pageflowBll.GetPageFlowGroup(ModuleName, pageFlowGroupCode);
            }

            pageflowGroup = CapUtil.GetPageFlowWithoutBlankPage(capModel, pageflowGroup);

            AppSession.SetPageflowGroupToSession(pageflowGroup);

            return pageflowGroup;
        }

        /// <summary>
        /// Bing the permit type to radio button list.
        /// </summary>
        /// <param name="capTypeModels">cap type model</param>
        private void Display(CapTypeModel[] capTypeModels)
        {
            // clear the item list
            PermitTypeSelect.Items.Clear();
            PermitTypeSelect.RepeatDirection = RepeatDirection.Vertical;

            string selectedType = GetSelectedType();

            //SortedList sortedList = new SortedList();
            IList<ListItem> items = new List<ListItem>();

            foreach (CapTypeModel typemodel in capTypeModels)
            {
                ListItem item = new ListItem();

                //item.Text = ScriptFilter.FilterScript(typemodel.alias);//.Replace("<", "&lt").Replace(">", "&gt");
                //item.Text = I18nStringUtil.GetString(typemodel.resAlias, CAPHelper.GetCapTypeLabel(typemodel));
                item.Text = CAPHelper.GetAliasOrCapTypeLabel(typemodel);

                //item.Value = ModelUIFormat.FormatCapType4Display(typemodel);
                item.Value = CAPHelper.GetCapTypeValue(typemodel);

                if (item.Value == selectedType)
                {
                    item.Selected = true;
                }

                //sortedList[item.Text] = item;
                items.Add(item);
            }

            ((List<ListItem>)items).Sort(ListItemComparer.Instance);

            //foreach (DictionaryEntry de in sortedList)
            foreach (ListItem item in items)
            {
                //PermitTypeSelect.Items.Add(de.Value as ListItem);
                PermitTypeSelect.Items.Add(item);
            }
        }

        /// <summary>
        /// Apply Amendment permit process UI
        /// </summary>
        private void DisplayAmendmentUIMsg()
        {
            if (AppSession.IsAdmin)
            {
                per_applyPermit_label_selectAmendmentType.Visible = true;
                per_applyPermit_text_selectAmendmentType.Visible = true;
                per_applyPermit_label_selectType.Visible = true;
                per_applyPermit_text_selectType.Visible = true;
                divBlankLineTitle.Visible = true;
                divBlankLineBody.Visible = true;
            }
            else if (Request.QueryString["parentCapModelID"] != null && Request.QueryString["parentCapModelID"].ToString() != string.Empty)
            {
                per_applyPermit_label_selectAmendmentType.Visible = true;
                per_applyPermit_text_selectAmendmentType.Visible = true;
                per_applyPermit_label_selectType.Visible = false;
                per_applyPermit_text_selectType.Visible = false;
            }
            else
            {
                per_applyPermit_label_selectAmendmentType.Visible = false;
                per_applyPermit_text_selectAmendmentType.Visible = false;
                per_applyPermit_label_selectType.Visible = true;
                per_applyPermit_text_selectType.Visible = true;
            }

            h1SelectType.Visible = per_applyPermit_label_selectType.Visible;
            h1SelectAmendmentType.Visible = per_applyPermit_label_selectAmendmentType.Visible;
        }

        /// <summary>
        /// Gets the current filter name
        /// </summary>
        /// <returns>the current filter name</returns>
        private string GetFilterName()
        {
            string filterName = string.Empty; //get filter name from cache.

            if (!string.IsNullOrEmpty(Request.QueryString["FilterName"]))
            {
                filterName = Request.QueryString["FilterName"].ToString();
            }

            return filterName;
        }

        /// <summary>
        /// when back to this page, get the selected type from session 
        /// </summary>
        /// <returns>string for selected cap type</returns>
        private string GetSelectedType()
        {
            //If obtain a fee estimate, we don't get the cap type from the cap model in the session.
            if (Is4FeeEstimator)
            {
                return null;
            }

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            if (capModel != null && capModel.capType != null)
            {
                //return ModelUIFormat.FormatCapType4Display(capModel.capType);
                return CAPHelper.GetCapTypeValue(capModel.capType);
            }

            return null;
        }

        /// <summary>
        /// Gets the type of the VCH.
        /// </summary>
        /// <returns>The VCH type</returns>
        private string GetVCHType()
        {
            string vchType = ACAConstant.VCH_TYPE_VHAPP;

            if (Is4FeeEstimator)
            {
                vchType = ACAConstant.VCH_TYPE_EST;
            }

            return vchType;
        }

        /// <summary>
        /// Get cap type and display them on the screen
        /// </summary>
        private void InitPermitType()
        {
            if (AppSession.User == null)
            {
                return;
            }

            string vchType = GetVCHType();
            string filterName = GetFilterName();

            string isCloningRecord = Request.QueryString[ACAConstant.IS_CLONE_RECORD] == null ? string.Empty : Request.QueryString[ACAConstant.IS_CLONE_RECORD].ToString();
            
            //User source record's record type when user is cloning a record. so redirect to capedit page.
            if (ACAConstant.COMMON_TRUE.Equals(isCloningRecord))
            {
                SetupCapType();
            }
            else
            {
                // determind whether Board Type DropDownList need to display
                bool isEnabledBoardTypeSelection = StandardChoiceUtil.IsEnableBoardTypeSelection(this.ModuleName);
                bool isJumpDirectly = !string.IsNullOrEmpty(Request.QueryString[UrlConstant.CAPTYPE]) && !string.IsNullOrEmpty(Request.QueryString["Module"]);

                if (isEnabledBoardTypeSelection && !isJumpDirectly)
                {
                    this.ddlBoardTypeSelection.Visible = true;
                    string userID = string.IsNullOrEmpty(Request.QueryString["createdBy"]) ? AppSession.User.PublicUserId : Request.QueryString["createdBy"];
                    DropDownListBindUtil.BindBoardTypes(this.ddlBoardTypeSelection, this.ModuleName, filterName, vchType, userID, IsContainAsChildOnly());

                    string boardType = ddlBoardTypeSelection.SelectedValue;
                    
                    if (!string.IsNullOrEmpty(boardType))
                    {
                        ShowCapTypes(vchType, filterName, ddlBoardTypeSelection.SelectedValue);
                    }
                }
                else
                {
                    this.ddlBoardTypeSelection.Visible = false;
                    ShowCapTypes(vchType, filterName, null);
                    if (PermitTypeSelect.Items.Count == 1)
                    {
                        SetupCapType();
                    }
                }
            }
        }

        /// <summary>
        /// Set reference contact to first contact component.
        /// </summary>
        /// <param name="pfGroup">The page flow group model.</param>
        /// <param name="peopleModel">The people model.</param>
        /// <param name="capModel">The cap model.</param>
        private void SetReferenceContactToFirstContactComponent(PageFlowGroupModel pfGroup, PeopleModel4WS peopleModel, CapModel4WS capModel)
        {
            if (peopleModel != null)
            {
                List<long> contactSectionNames = new List<long> { (long)PageFlowComponent.CONTACT_1, (long)PageFlowComponent.CONTACT_2, (long)PageFlowComponent.CONTACT_3, (long)PageFlowComponent.APPLICANT, (long)PageFlowComponent.CONTACT_LIST };
                var contactComponents = from step in pfGroup.stepList
                                        where step != null
                                        from page in step.pageList
                                        where page != null
                                        from cpt in page.componentList
                                        where cpt != null && !string.IsNullOrEmpty(cpt.componentName)
                                        where contactSectionNames.Contains(cpt.componentID)
                                        select cpt;

                if (!contactComponents.Any())
                {
                    return;
                }

                string contactType = string.Empty;

                ComponentModel component = contactComponents.ToArray()[0];

                switch (component.componentID)
                {
                    case (long)PageFlowComponent.CONTACT_1:
                        contactType = component.customHeading;
                        break;
                    case (long)PageFlowComponent.CONTACT_2:
                        contactType = component.customHeading;
                        break;
                    case (long)PageFlowComponent.CONTACT_3:
                        contactType = component.customHeading;
                        break;
                    case (long)PageFlowComponent.APPLICANT:
                        contactType = component.customHeading;
                        break;
                }

                var contactModel = new CapContactModel4WS();
                contactModel.people = peopleModel;
                contactModel.refContactNumber = peopleModel.contactSeqNumber;
                contactModel.componentName = PageFlowConstant.SECTION_NAME_MULTI_CONTACT_PREFIX;

                capModel.contactsGroup = new CapContactModel4WS[] { contactModel };

                //Should clear the contact seq number, otherwish will update contact when save partial record.
                peopleModel.contactSeqNumber = string.Empty;
                PeopleUtil.FilterInactivateContactAddress(peopleModel);

                /*
                 * For single contact section, the contact type of reference contact may not same with the target contact section.
                 * So the associated generic template also not same with the target contact type associated, we just need to fill
                 *   the template data to the target contact section.
                 */
                if (peopleModel.template != null && !string.IsNullOrEmpty(contactType))
                {
                    ContactUtil.MergeTemplateFieldByContactType(capModel, contactType, ModuleName);
                }
            }
        }

        /// <summary>
        /// set up cap type
        /// </summary>
        private void SetupCapType()
        {
            //this step should always be 1.
            int nextStep = 2;

            string isCloningRecord = Request.QueryString[ACAConstant.IS_CLONE_RECORD] == null ? string.Empty : Request.QueryString[ACAConstant.IS_CLONE_RECORD].ToString();

            if (ACAConstant.COMMON_TRUE.Equals(isCloningRecord))
            {
                if (!ResetCloneCapModelToSession())
                {
                    return;
                }
            }
            else
            {
                if (!CreateCapModeToSession())
                {
                    return;
                }
            }

            string url = "CapEdit.aspx?Module=" + ModuleName + "&stepNumber=" + nextStep + "&pageNumber=1&isFeeEstimator=" + Request.QueryString["isFeeEstimator"];

            // if only one license and one cap type, the tab name should pass to the next page.
            url = url + "&TabName=" + Request.QueryString["TabName"];

            if (!string.IsNullOrEmpty(Request.QueryString["FilterName"]))
            {
                url = url + "&FilterName=" + Request.QueryString["FilterName"];
            }

            if (ACAConstant.COMMON_TRUE.Equals(isCloningRecord))
            {
                url += ACAConstant.AMPERSAND + ACAConstant.IS_CLONE_RECORD + ACAConstant.EQUAL_MARK + ACAConstant.COMMON_TRUE;
            }

            if (ValidationUtil.IsYes(Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]))
            {
                url += ACAConstant.AMPERSAND + ACAConstant.IS_SUBAGENCY_CAP + ACAConstant.EQUAL_MARK + ACAConstant.COMMON_Y;
            }

            if (!string.IsNullOrEmpty(Request.QueryString[UrlConstant.AgencyCode]))
            {
                url += "&" + UrlConstant.AgencyCode + "=" + Request.QueryString[UrlConstant.AgencyCode];
            }

            if (Session[SessionConstant.SESSION_CREATE_RECORD_BY_MAP] != null)
            {
                url += "&IsFromMap=" + ACAConstant.COMMON_TRUE;
            }

            if (!string.IsNullOrEmpty(Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER]))
            {
                url += "&" + UrlConstant.CONTACT_SEQ_NUMBER + "=" + Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER];
            }

            if (!string.IsNullOrEmpty(Request.QueryString["isAmendment"]))
            {
                url += "&isAmendment=" + Request.QueryString["isAmendment"];
            }

            //Set the parent info to session for the Associated Forms.
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            CapUtil.SetAssoFormParentToAppSession(capModel, capModel.capType, AppSession.GetPageflowGroupFromSession());

            Response.Redirect(url);
        }

        /// <summary>
        /// Show all available CAP types in the dropdownlist.
        /// By default, the CAP type will be automatically selected if only one CAP type.
        /// </summary>
        /// <param name="vchType">the current VCHType used to get CAP types</param>
        /// <param name="filterName">the selected filter name used to get CAP types</param>
        /// <param name="boardType">The board type used to filter CAP type</param>
        private void ShowCapTypes(string vchType, string filterName, string boardType)
        {
            string reqCAPType = Request.QueryString[UrlConstant.CAPTYPE] == null ? string.Empty : Request.QueryString[UrlConstant.CAPTYPE].ToString();
            string reqModuleName = Request.QueryString["Module"] == null ? string.Empty : Request.QueryString["Module"].ToString();

            CapTypeModel[] permitTypelist = null;

            if (!string.IsNullOrEmpty(reqCAPType) && !string.IsNullOrEmpty(reqModuleName))
            {
                CapTypeModel capType = CapUtil.ConstructCAPTypeModel(reqModuleName, reqCAPType, ConfigManager.AgencyCode);

                /*
                 * CapType may be passed by 3rd part deep link, so need to check if the passed cap type is valid.
                 * If passed cap type is invalid, will show all cap type list.
                 */
                if (capType != null && !string.IsNullOrEmpty(capType.group))
                {
                    permitTypelist = new CapTypeModel[1];
                    permitTypelist[0] = capType;
                }
            }

            if (permitTypelist == null)
            {
                ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
                string userID = string.IsNullOrEmpty(Request.QueryString["createdBy"]) ? AppSession.User.PublicUserId : Request.QueryString["createdBy"];

                CapTypeModel[] capTypeModels = capTypeBll.GetGeneralCapTypeList(ModuleName, filterName, vchType, boardType, userID, IsContainAsChildOnly());

                // if cap type anonymousCreateAllowed field is N then remove the cap type from the cap type list
                permitTypelist = AppSession.User.IsAnonymous && capTypeModels != null
                                     ? capTypeModels.Where(w => !ValidationUtil.IsNo(w.anonymousCreateAllowed)).ToArray()
                                     : capTypeModels;
            }

            if (permitTypelist != null)
            {
                Display(permitTypelist);

                if (permitTypelist.Length == 1)
                {
                    PermitTypeSelect.Items[0].Selected = true;
                    SkipToCapEditPage();
                }
            }
        }

        /// <summary>
        /// Is Contain As Child Only
        /// </summary>
        /// <returns>Contain As Child Only Flag</returns>
        private bool IsContainAsChildOnly()
        {
            string isAmendment = Request.QueryString["isAmendment"];
            string isFromAccount = Request.QueryString[UrlConstant.IS_FROM_ACCOUNT_MANANGEMENT];
            return ValidationUtil.IsYes(isAmendment) && !ValidationUtil.IsYes(isFromAccount);
        }

        /// <summary>
        /// Reset LP into current cap which is in current page flow. reorder LP list with primary field.
        /// </summary>
        /// <param name="cap">current cap</param>
        /// <returns>current cap with reset LP</returns>
        private CapModel4WS ResetCloneLP(CapModel4WS cap)
        {
            bool hasLPComonent = PageFlowUtil.IsComponentExist(GViewConstant.SECTION_LICENSE) || PageFlowUtil.IsComponentExist(GViewConstant.SECTION_MULTIPLE_LICENSES);

            if (hasLPComonent)
            {
                /*
                If user selected "None Applicable", don't copy the license info.(SelectedLicenseModel is empty object.)
                If the SelectedLicenseModel is null, the user have not related license, needs copy license info.
                */
                if (SelectedLicenseModel != null)
                {
                    cap.licenseProfessionalList = null;
                    cap.licenseProfessionalModel = null;
                    cap.licSeqNbr = null;

                    if (!string.IsNullOrEmpty(SelectedLicenseModel.licSeqNbr))
                    {
                        LicenseProfessionalModel licensee = CapUtil.CreateLicenseProfessionalModel(SelectedLicenseModel);

                        //In license picker page select a license, should only get reference LP template. 
                        ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
                        TemplateAttributeModel[] lpTemplates = templateBll.GetPeopleTemplateAttributes(SelectedLicenseModel.licenseType, ConfigManager.AgencyCode, AppSession.User.PublicUserId);
                        
                        // Fill reference attribute values.
                        licensee.templateAttributes = LicenseUtil.FillRefLicenseTemplate(licensee.agencyCode, licensee.licenseType, licensee.licSeqNbr, lpTemplates);

                        LicenseProfessionalModel4WS lp4WS = TempModelConvert.ConvertToLicenseProfessionalModel4WS(licensee);
                        cap.licSeqNbr = SelectedLicenseModel.licSeqNbr;
                        cap.licenseProfessionalList = new LicenseProfessionalModel4WS[1];
                        cap.licenseProfessionalList[0] = lp4WS;
                    }
                }
                else if (cap.licenseProfessionalList != null && cap.licenseProfessionalList.Length > 1)
                {
                    var lps = from lp in cap.licenseProfessionalList orderby lp.printFlag descending select lp;
                    cap.licenseProfessionalList = lps.ToArray();
                    cap.licSeqNbr = cap.licenseProfessionalList[0].licSeqNbr;
                }
            }
            else
            {
                cap.licenseProfessionalList = null;
                cap.licenseProfessionalModel = null;
                cap.licSeqNbr = null;
                SelectedLicenseModel = null;
            }

            return cap;
        }

        #endregion Methods
    }
}