#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LicenseInput.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: LicenseInput.aspx.cs 257891 2013-09-28 09:56:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Component;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using log4net;

namespace Accela.ACA.Web.People
{
    /// <summary>
    /// Class LicenseInput.
    /// </summary>
    public partial class LicenseInput : PopupDialogBasePage
    {
        #region Fileds

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(LicenseInput));

        /// <summary>
        /// Check the license records select from user's account or search reference LP record 
        /// whether they are skipped the license edit page and added directly to license list or not.
        /// </summary>
        private bool _isSkipLicenseEdit = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance is multiple LP list.
        /// </summary>
        /// <value><c>true</c> if this instance is multiple LP list; otherwise, <c>false</c>.</value>
        protected bool IsMultipleLPList
        {
            get
            {
                return ValidationUtil.IsTrue(Request.QueryString["isLPList"]);
            }
        }

        /// <summary>
        /// Gets parent control id.
        /// </summary>
        protected string ParentControlID
        {
            get
            {
                return Request.QueryString["parentControlId"];
            }
        }

        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        /// <value>The name of the component.</value>
        protected string ComponentName
        {
            get
            {
                return Request.QueryString["componentName"];
            }
        }

        /// <summary>
        /// Gets current action type
        /// </summary>
        private string Action
        {
            get
            {
                return Request.QueryString["action"];
            }
        }

        /// <summary>
        /// Gets the license sequence NBR.
        /// </summary>
        /// <value>The license sequence NBR.</value>
        private string LicenseSeqNbr
        {
            get
            {
                return Request.QueryString["licenseSeqNbr"];
            }
        }

        /// <summary>
        /// Gets the license temporary unique identifier.
        /// </summary>
        /// <value>The license temporary unique identifier.</value>
        private string LicenseTempID
        {
            get
            {
                return Request.QueryString["licenseTempID"];
            }
        }

        /// <summary>
        /// Gets the license NBR.
        /// </summary>
        /// <value>The license NBR.</value>
        private string LicenseNbr
        {
            get
            {
                return Request.QueryString[UrlConstant.LICENSE_NBR];
            }
        }

        /// <summary>
        /// Gets the type of the license.
        /// </summary>
        /// <value>The type of the license.</value>
        private string LicenseType
        {
            get
            {
                return Request.QueryString[UrlConstant.LICENSE_TYPE];
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is editable.
        /// </summary>
        /// <value><c>true</c> if this instance is editable; otherwise, <c>false</c>.</value>
        private bool IsEditable
        {
            get
            {
                return ValidationUtil.IsTrue(Request.QueryString["isEditable"]);
            }
        }

        /// <summary>
        ///  Gets a value indicating whether section validate property
        /// </summary>
        private bool IsValidate
        {
            get
            {
                return ValidationUtil.IsTrue(Request.QueryString["isValidate"]);
            }
        }

        /// <summary>
        ///  Gets a value indicating whether section required property
        /// </summary>
        private bool IsSectionRequired
        {
            get
            {
                return ValidationUtil.IsTrue(Request.QueryString["isSectionRequired"]);
            }
        }

        /// <summary>
        /// Gets or sets value of current step of the page
        /// </summary>
        private string CurrentStep
        {
            get
            {
                return ViewState["CurrentStep"] as string;
            }

            set 
            { 
                ViewState["CurrentStep"] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// On Initial event
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnInit(EventArgs e)
        {
            string viewId = string.Empty;

            switch (Action.ToUpperInvariant())
            {
                case AddDataWay.Edit:
                case AddDataWay.AddNew:
                    viewId = GviewID.LicenseEdit;
                    break;

                case AddDataWay.LookUp:
                   viewId = GviewID.LicenseLookUp;
                    break;

                default:
                    viewId = string.Empty;
                    break;
            }

            licenseInput.SectionViewId = AddDataWay.LookUp.Equals(Action, StringComparison.InvariantCultureIgnoreCase) ? GviewID.LicenseLookUp : GviewID.LicenseEdit;
            lblNewLicenseForm.SectionID = string.Format("{1}{0}{2}{0}{3}_", ACAConstant.SPLIT_CHAR, ModuleName, viewId, licenseInput.ClientID);

            base.OnInit(e);
        }

        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            DialogUtil.RegisterScriptForDialog(Page);
            SetDialogMaxHeight("550");

            licenseInput.IsValidate = IsValidate;
            licenseInput.IsEditable = IsEditable;
            licenseInput.IsSectionRequired = IsSectionRequired;
            licenseInput.ComponentName = ComponentName;
            licenseInput.IsMultipleLicensedProfessional = IsMultipleLPList;
            refLicenseList.IsMultipleLicensedProfessional = IsMultipleLPList;
            refLicenseList.GridViewSort += new GridViewSortedEventHandler(LicenseList_GridViewSort);
            refLicenseList.PageIndexChanging += new GridViewPageEventHandler(LicenseList_PageIndexChanging);

            if (!IsPostBack)
            {
                switch (Action.ToUpperInvariant())
                {
                    case AddDataWay.SelectFromAccount:
                        BindLPListBySelectFromAccount();
                        break;

                    case AddDataWay.Edit:
                    case AddDataWay.AddNew:
                        LicenseProfessionalModel license = GetLPModelFromSession();

                        if (!AppSession.IsAdmin
                            && license != null
                            && !string.IsNullOrEmpty(license.licSeqNbr))
                        {
                            LicenseUtil.IsLockedCondition(ModuleName, long.Parse(license.licSeqNbr), licenseCondition);

                            if (licenseCondition.ConditionResult != ConditionResult.None)
                            {
                                licenseCondition.Visible = true;
                            }
                        }

                        ShowAddNewLicenseForm(true);

                        /* 
                         * DisplayLicense() must be run after ShowAddNewLicenseForm(true).
                         * Otherwise, some values such as State maybe no way to set.
                         */
                        DisplayLicense(license);
                        break;

                    case AddDataWay.LookUp:
                        ShowLookUpLicenseForm(true);
                        break;
                }
            }

            if (AppSession.IsAdmin)
            {
                lblNewLicenseForm.Visible = true;
            }
        }

        /// <summary>
        /// OnPreRender event
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (string.Equals(AddDataWay.Edit, CurrentStep))
            {
                SetLicenseInputPageTitle(AddDataWay.Edit);
            }
            else if (string.Equals(AddDataWay.AddNew, CurrentStep))
            {
                SetLicenseInputPageTitle(AddDataWay.AddNew);
            }
            else
            {
                SetLicenseInputPageTitle(Action);
            }

            // Don't run expression in Admin side.
            if (!AppSession.IsAdmin)
            {
                // In LookUp License Form, it shouldn't support expression builder, set the value before base.OnPreRender.
                if (GviewID.LicenseLookUp.Equals(licenseInput.SectionViewId, StringComparison.InvariantCultureIgnoreCase))
                {
                    licenseInput.NeedRunExpression = false;
                }

                ExpressionUtil.RegisterScriptLibToCurrentPage(this);

                if (!Page.IsPostBack)
                {
                    RegisterExpressionOnLoad();
                }

                RegisterExpressionOnSubmit();
                ExpressionUtil.ResetJsExpression(this);
            }
        }

        /// <summary>
        /// Save license professional information into licensed professional list.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void BtnContinueClick(object sender, EventArgs e)
        {
            licenseNoticeBar.Hide();
            ShowLookUpLicenseForm(false);

            List<LicenseModel4WS> licenseList = new List<LicenseModel4WS>();

            if (IsMultipleLPList)
            {
                if (refLicenseList.SelectedItems != null && refLicenseList.SelectedItems.Length > 0)
                {
                     licenseList.AddRange(refLicenseList.SelectedItems);
                }
            }
            else
            {
                if (refLicenseList.SelectedItem != null)
                {
                    licenseList.Add(refLicenseList.SelectedItem);
                }
            }

            if (licenseList.Count() == 1)
            {
                CheckOutOneResult(licenseList.First());
            }
            else if (licenseList.Count() > 1)
            {
                CheckOutMultipleResult(licenseList.ToArray());
            }
        }

        /// <summary>
        /// Handle search button click.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void BtnSearchClick(object sender, EventArgs e)
        {
            if (!licenseInput.CheckInputConditionForLicense())
            {
                string message = GetTextByKey("per_permitList_SearchCriteria_Required");
                MessageUtil.ShowMessageInPopup(Page, MessageType.Error, message);
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "GotoMessageBar", "scrollIntoView('messageSpan');", true);
                return;
            }

            licenseInput.ResetTemplate();
            refLicenseList.ResetGridView();
            SearchRefLicense(0, null);
        }

        /// <summary>
        /// Handle clear button click.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void BtnClearClick(object sender, EventArgs e)
        {
            licenseNoticeBar.Hide();
            licenseCondition.HideCondition();
            licenseInput.ClearLicenseInputForm(true);
        }

        /// <summary>
        /// Handle search button click.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LnkBackSearchForm(object sender, EventArgs e)
        {
            ShowLookUpLicenseForm(true);
            divRefLicenseList.Visible = false;
            divNewLicenseButton.Visible = false;
            liContinue.Visible = false;
            licenseNoticeBar.Hide();
            licenseCondition.HideCondition();
        }

        /// <summary>
        /// Save license professional information into licensed professional list.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void BtnSaveAndCloseClick(object sender, EventArgs e)
        {
            try
            {
                string errorMsg = licenseInput.SaveCurrentLicenseProfessional();

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    // 1. Show Error Message
                    MessageUtil.ShowMessageInPopupScrollTop(Page, MessageType.Error, errorMsg);
                }
                else
                {
                    ActionType successType = AddDataWay.Edit.Equals(Action, StringComparison.InvariantCultureIgnoreCase) ? ActionType.UpdateSuccess : ActionType.AddSuccess;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), ParentControlID, "ClosePopup('" + successType.ToString("D") + "');", true);
                    CurrentStep = AddDataWay.AddNew;
                    liClear.Visible = true;
                }
            }
            catch (ACAException ex)
            {
                Logger.Error(ex);
                ActionType failedType = AddDataWay.Edit.Equals(Action, StringComparison.InvariantCultureIgnoreCase) ? ActionType.UpdateFailed : ActionType.AddFailed;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), ParentControlID, "ClosePopup('" + failedType.ToString("D") + "');", true);
            }
        }

        /// <summary>
        /// Set the license input page title
        /// </summary>
        /// <param name="action">The action</param>
        private void SetLicenseInputPageTitle(string action)
        {
            if (string.IsNullOrEmpty(action))
            {
                return;
            }

            string viewID = string.Empty;
            switch (action.ToUpperInvariant())
            {
                case AddDataWay.SelectFromAccount:
                    lblNewLicenseForm.LabelKey = "aca_licenseinput_label_selectlicensefromaccount";
                    break;

                case AddDataWay.Edit:
                case AddDataWay.AddNew:
                    lblNewLicenseForm.LabelKey = "aca_licenseinput_label_editlicensedprofessional";
                    viewID = GviewID.LicenseEdit;
                    break;

                case AddDataWay.LookUp:
                    lblNewLicenseForm.LabelKey = "aca_licenseinput_label_lookuplicense";
                    viewID = GviewID.LicenseLookUp;
                    break;
            }

            licenseInput.SectionViewId = AddDataWay.LookUp.Equals(action, StringComparison.InvariantCultureIgnoreCase) ? GviewID.LicenseLookUp : GviewID.LicenseEdit;
            lblNewLicenseForm.SectionID = string.Format("{1}{0}{2}{0}{3}_", ACAConstant.SPLIT_CHAR, ModuleName, viewID, licenseInput.ClientID);

            if (!AppSession.IsAdmin)
            {
                SetPageTitleKey(lblNewLicenseForm.LabelKey);
            }
        }

        /// <summary>
        /// Bind license professional list by public user associated licenses.
        /// </summary>
        private void BindLPListBySelectFromAccount()
        {
            ILicenseBLL licenseBll = ObjectFactory.GetObject<ILicenseBLL>();
            LicenseModel4WS[] licenses = LicenseUtil.GetPublicUserLicenses(ModuleName);
            refLicenseList.GridViewDataSource = licenseBll.BuildLicenseDataTable(licenses);

            if (licenses != null && licenses.Length > 1)
            {
                ShowSelectFromAccountForm(true);
            }
            else if (licenses != null && licenses.Length == 1)
            {
                CheckOutOneResult(licenses.First());
            }
            else
            {
                ShowSelectFromAccountForm(true);
            }
        }

        /// <summary>
        /// check out one result.
        /// </summary>
        /// <param name="license">license model</param>
        private void CheckOutOneResult(LicenseModel4WS license)
        {
            LicenseModel4WS[] licenses = new[] { license };
            string errorMsg = licenseInput.IsAvailableLicense(licenses);
            List<LicenseProfessionalModel4WS> lpList = LicenseUtil.ConvertLicenseModel2LicenseProfessionalModel4WS(licenses, ComponentName);
            LicenseProfessionalModel licenseProfessional = TempModelConvert.ConvertToLicenseProfessionalModel(lpList.First());
            bool isPassValidate = LicenseUtil.ValidateRequiredField4SingleLicense(licenseProfessional, ModuleName, IsEditable, IsValidate);
            bool isPassed2NextStep = true;

            if (LicenseUtil.IsLockedCondition(ModuleName, long.Parse(license.licSeqNbr), licenseCondition))
            {
                licenseCondition.Visible = true;
                ShowSelectFromAccountForm(true);
                isPassed2NextStep = false;
            }
            else if (!string.IsNullOrEmpty(errorMsg))
            {
                ShowSelectFromAccountForm(true);
                licenseNoticeBar.ShowWithText(MessageType.Error, errorMsg, MessageSeperationType.Top);
                isPassed2NextStep = false;
            }
            else if (licenseCondition.ConditionResult != ConditionResult.None || !isPassValidate)
            {
                licenseCondition.Visible = licenseCondition.ConditionResult != ConditionResult.None;
                ShowSelectFromAccountForm(false);
                ShowAddNewLicenseForm(true);
                DisplayLicense(licenseProfessional);

                //Execute onload expression when from lookup form turn to Edit form .
                RegisterExpressionOnLoad();
            }
            else
            {
                _isSkipLicenseEdit = true;
                SaveLP2CapModel(licenses);
            }

            if (isPassed2NextStep)
            {
                CurrentStep = AddDataWay.Edit;
            }
        }

        /// <summary>
        /// check out multiple result
        /// </summary>
        /// <param name="licenseList">license model list</param>
        private void CheckOutMultipleResult(LicenseModel4WS[] licenseList)
        {
            bool hasLockCondition = false;

            foreach (LicenseModel4WS license in licenseList)
            {
                if (LicenseUtil.IsLockedCondition(ModuleName, long.Parse(license.licSeqNbr), licenseCondition))
                {
                    hasLockCondition = true;
                    break;
                }
            }

            string errorMsg = licenseInput.IsAvailableLicense(licenseList);

            if (!string.IsNullOrEmpty(errorMsg))
            {
                ShowSelectFromAccountForm(true);
                licenseNoticeBar.ShowWithText(MessageType.Error, errorMsg, MessageSeperationType.Top);
            }
            else if (hasLockCondition)
            {
                licenseCondition.Visible = true;
                ShowSelectFromAccountForm(true);
            }
            else
            {
                _isSkipLicenseEdit = true;
                SaveLP2CapModel(licenseList);
            }
        }

        /// <summary>
        /// Save license professional to cap model session.
        /// </summary>
        /// <param name="licenseList">license list</param>
        private void SaveLP2CapModel(IEnumerable<LicenseModel4WS> licenseList)
        {
            try
            {
                List<LicenseProfessionalModel> lpList = LicenseUtil.ConvertLicenseModel2LicenseProfessionalModel(licenseList, ComponentName);

                foreach (var licenseProfessionalModel in lpList)
                {
                    licenseInput.SaveLicenseProfessionalModel(licenseProfessionalModel.licSeqNbr, licenseProfessionalModel, _isSkipLicenseEdit);
                }

                ActionType successType = AddDataWay.Edit.Equals(Action, StringComparison.InvariantCultureIgnoreCase) ? ActionType.UpdateSuccess : ActionType.AddSuccess;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), ParentControlID, "ClosePopup('" + successType.ToString("D") + "');", true);
            }
            catch (ACAException ex)
            {
                Logger.Error(ex);
                ActionType failedType = AddDataWay.Edit.Equals(Action, StringComparison.InvariantCultureIgnoreCase) ? ActionType.UpdateFailed : ActionType.AddFailed;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), ParentControlID, "ClosePopup('" + failedType.ToString("D") + "');", true);
            }
        }

        /// <summary>
        /// Show select from account page
        /// </summary>
        /// <param name="isShow">is show</param>
        private void ShowSelectFromAccountForm(bool isShow)
        {
            divNewLicenseButton.Visible = isShow;
            liCancel.Visible = isShow;
            liContinue.Visible = isShow;
            divRefLicenseList.Visible = isShow;
            lnkReviseSearch.Visible = AddDataWay.LookUp.Equals(Action, StringComparison.InvariantCultureIgnoreCase);
            lblSearchLicenseList.Visible = AddDataWay.LookUp.Equals(Action, StringComparison.InvariantCultureIgnoreCase);

            if (isShow)
            {
                licenseInput.SectionViewId = GviewID.LicenseLookUp;
                refLicenseList.BindLicenseList();
            }
        }

        /// <summary>
        /// Show add new license page
        /// </summary>
        /// <param name="isShow">is show</param>
        private void ShowAddNewLicenseForm(bool isShow)
        {
            divNewLicenseButton.Visible = isShow;
            liCancel.Visible = isShow;
            liClear.Visible = AppSession.IsAdmin || (isShow && Action == AddDataWay.AddNew);
            liSaveAndClose.Visible = isShow;
            divNewLicenseEditForm.Visible = isShow;

            if (isShow)
            {
                licenseInput.SectionViewId = GviewID.LicenseEdit;
            }
        }

        /// <summary>
        /// Get license professional from app session.
        /// </summary>
        /// <returns>license professional model</returns>
        private LicenseProfessionalModel GetLPModelFromSession()
        {
            LicenseProfessionalModel license = null;
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            if (!string.IsNullOrEmpty(LicenseSeqNbr))
            {
                if (LicenseUtil.IsLockedCondition(ModuleName, long.Parse(LicenseSeqNbr), licenseCondition))
                {
                    return null;
                }

                license = TempModelConvert.ConvertToLicenseProfessionalModel(capModel.licenseProfessionalList.FirstOrDefault(p => p.TemporaryID.Equals(LicenseTempID, StringComparison.InvariantCultureIgnoreCase)));
            }
            else if (capModel.licenseProfessionalList != null)
            {
                LicenseProfessionalModel4WS lpModel =
                    capModel.licenseProfessionalList.FirstOrDefault(
                        p =>
                            p.licenseType.Equals(LicenseType, StringComparison.InvariantCultureIgnoreCase)
                            && p.licenseNbr.Equals(LicenseNbr, StringComparison.InvariantCultureIgnoreCase));

                license = TempModelConvert.ConvertToLicenseProfessionalModel(lpModel);
            }

            return license;
        }

        /// <summary>
        /// Display license in popup window.
        /// </summary>
        /// <param name="license">The license.</param>
        private void DisplayLicense(LicenseProfessionalModel license)
        {
            bool isDisable = licenseInput.DisplayLicense(license);

            if (isDisable)
            {
                btnClear.Enabled = false;
            }

            if (!AppSession.IsAdmin && !IsEditable)
            {
                btnClear.Enabled = false;

                //always edit fields can be edit and save when isEditable = false,
                btnSaveAndClose.Enabled = TemplateUtil.IsExistsAlwaysEditableRequiredTemplateField(license.templateAttributes);
            }
        }

        /// <summary>
        /// Show look up license page
        /// </summary>
        /// <param name="isShow">is show</param>
        private void ShowLookUpLicenseForm(bool isShow)
        {
            divNewLicenseEditForm.Visible = isShow;
            divSearchButton.Visible = isShow;
            
            // In LookUp License Form, it shouldn't support expression builder.
            licenseInput.NeedRunExpression = !isShow;
            liSearch.Visible = isShow;
            liSearchClear.Visible = isShow;
            liSearchDisCard.Visible = isShow;
            licenseInput.SetLicenseNumAutoPostBack(false);
            licenseInput.ResetSupervisorTemplate();

            // In admin Look Up Page, it should display the search result list.
            if (AppSession.IsAdmin)
            {
                ShowSelectFromAccountForm(true);
            }
        }

        /// <summary>
        /// in page index event, keep control display status
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">EventArgs e</param>
        private void LicenseList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (Action.ToUpperInvariant() == AddDataWay.LookUp)
            {
                PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(refLicenseList.ClientID);

                if (e.NewPageIndex > pageInfo.EndPage)
                {
                    SearchRefLicense(e.NewPageIndex, pageInfo.SortExpression);
                }
            }
        }

        /// <summary>
        /// Handles the GridViewSort event of the contactList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A CommonEventArgs object.</param>
        private void LicenseList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(refLicenseList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// search reference licenses.
        /// </summary>
        /// <param name="currentPageIndex">current page index</param>
        /// <param name="sortExpression">sort expression</param>
        private void SearchRefLicense(int currentPageIndex, string sortExpression)
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            bool onlySearchMyLicense = xPolicyBll.GetValueByKey(ACAConstant.ACA_ENABLE_ONLY_SEARCH_MY_LICENSE) == ACAConstant.COMMON_Y;
            ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
            DataTable dt;

            if (AppSession.User.IsAnonymous && onlySearchMyLicense)
            {
                dt = licenseBll.ConstructLicenseDataTable();
            }
            else
            {
                LicenseModel4WS licenseModel = licenseInput.GetLicenseProfessionals();
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(refLicenseList.ClientID);
                pageInfo.SortExpression = sortExpression;
                pageInfo.CurrentPageIndex = currentPageIndex;
                pageInfo.CustomPageSize = refLicenseList.PageSize;
                QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
                SearchResultModel searchResult = licenseBll.GetLicenseValidList(ConfigManager.AgencyCode, onlySearchMyLicense ? long.Parse(AppSession.User.UserSeqNum) : 0, licenseModel, true, capModel.capType, queryFormat);
                pageInfo.StartDBRow = searchResult.startRow;
                dt = licenseBll.BuildLicenseDataTable(ObjectConvertUtil.ConvertObjectArray2EntityArray<LicenseModel4WS>(searchResult.resultList));
                dt = PaginationUtil.MergeDataSource<DataTable>(refLicenseList.GridViewDataSource, dt, pageInfo);

                if (dt.Columns.Contains("LicenseNumber"))
                {
                    DataView dv = dt.DefaultView;
                    dv.Sort = "LicenseNumber ASC";
                    dt = dv.ToTable();
                }
            }

            refLicenseList.GridViewDataSource = dt;
            ShowLookUpLicenseForm(false);

            if (refLicenseList.GridViewDataSource.Rows.Count > 1)
            {
                //if more than one result is returned display them in the list
                ShowSelectFromAccountForm(true);
            }
            else if (refLicenseList.GridViewDataSource.Rows.Count == 1)
            {
                DataRow dr = refLicenseList.GridViewDataSource.Rows[0];
                CheckOutOneResult((LicenseModel4WS)dr["LicenseModel"]);
            }
            else
            {
                ShowSelectFromAccountForm(true);
                refLicenseList.GenerateNoSearchResultMessage();
            }
        }

        /// <summary>
        /// register "java script" code into page to running expression for "on submit".
        /// </summary>
        private void RegisterExpressionOnLoad()
        {
            string callJsFunction = ExpressionUtil.GetExpressionScript(false, licenseInput);

            if (!Page.ClientScript.IsStartupScriptRegistered("OnLoadExpression"))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "OnLoadExpression", callJsFunction, true);
            }
        }

        /// <summary>
        /// register JS function.
        /// register "javascript" code into page to running expression for "on submit".
        /// </summary>
        private void RegisterExpressionOnSubmit()
        {
            string callJsFunction = ExpressionUtil.GetExpressionScript(true, licenseInput);
            string strSubmitFunction = ExpressionUtil.GetExpressionScriptOnSubmit(callJsFunction);

            if (!Page.ClientScript.IsStartupScriptRegistered("OnSubmitExpression"))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "OnSubmitExpression", strSubmitFunction, true);
            }
        }

        #endregion
    }
}