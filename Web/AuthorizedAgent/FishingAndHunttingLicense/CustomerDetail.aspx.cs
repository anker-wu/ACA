#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CustomerDetail.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: CustomerDetail.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.Web.People;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.AuthorizedAgent.FishingAndHunttingLicense
{
    /// <summary>
    /// This class provides the customer detail page.
    /// </summary>
    public partial class CustomerDetail : BasePage
    {
        #region Fields

        /// <summary>
        /// The authorized service setting model
        /// </summary>
        private AuthorizedServiceSettingModel _authServiceSetting = null;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the contact sequence number.
        /// </summary>
        protected string ContactSeqNbr
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether Customer detail form is editable.
        /// </summary>
        protected bool IsEditable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsEditable"]);
            }

            set
            {
                ViewState["IsEditable"] = value;
            }
        }

        /// <summary>
        /// Gets the authorized service setting model.
        /// </summary>
        private AuthorizedServiceSettingModel AuthServiceSetting 
        {
            get
            {
                if (_authServiceSetting == null)
                {
                    _authServiceSetting = AuthorizedAgentServiceUtil.GetAuthorizedServiceSetting();
                }

                return _authServiceSetting;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the contact parameters session.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="contactAddressIndex">Index of the contact address.</param>
        /// <param name="processType">Type of the process.</param>
        /// <param name="callbackName">Name of the callback.</param>
        /// <param name="parameterString">The parameter string.</param>
        [WebMethod(Description = "Operation Contact Address Session", EnableSession = true)]
        public static void OperationContactAddressSession(string moduleName, string contactAddressIndex, string processType, string callbackName, string parameterString)
        {
            ContactSessionParameter sessionParameter = AppSession.GetContactSessionParameter();

            if (!string.IsNullOrEmpty(parameterString))
            {
                ContactSessionParameter newSessionParameter = new JavaScriptSerializer().Deserialize<ContactSessionParameter>(parameterString);
                sessionParameter.ContactType = newSessionParameter.ContactType;
            }

            if (!string.IsNullOrEmpty(processType))
            {
                sessionParameter.Process.ContactAddressProcessType = EnumUtil<ContactAddressProcessType>.Parse(processType);
            }

            if (!string.IsNullOrEmpty(contactAddressIndex))
            {
                sessionParameter.Data.ContactAddressRowIndex = int.Parse(contactAddressIndex);
            }
            else
            {
                sessionParameter.Data.ContactAddressRowIndex = null;
            }

            sessionParameter.Process.CACallbackFunctionName = callbackName;
            AppSession.SetContactSessionParameter(sessionParameter);
        }

        /// <summary>
        /// Creates the contact parameters session.
        /// </summary>
        /// <param name="people">people model</param>
        public void CreateContactParametersSession(PeopleModel4WS people)
        {
            ContactSessionParameter sessionParameter = new ContactSessionParameter();
            sessionParameter.ContactExpressionType = ExpressionType.AuthAgent_Customer_Detail;
            sessionParameter.ContactSectionPosition = ACAConstant.ContactSectionPosition.AuthAgentCustomerDetail;

            if (people != null)
            {
                sessionParameter.ContactType = people.contactType;
                sessionParameter.Process.ContactProcessType = ContactProcessType.Edit;

                if (people.contactAddressList != null)
                {
                    int addressIndex = 0;

                    foreach (ContactAddressModel address in people.contactAddressList)
                    {
                        address.RowIndex = addressIndex;
                        addressIndex++;
                    }
                }
            }
            else
            {
                people = new PeopleModel4WS();
                sessionParameter.Process.ContactProcessType = ContactProcessType.Add;
            }

            sessionParameter.Process.CallbackFunctionName = customerForm.ClientID;
            sessionParameter.Data.DataObject = people;
            AppSession.SetContactSessionParameter(sessionParameter);
        }

        #endregion Public Methods

        #region protected Methods

        /// <summary>
        /// <c>OnPreInit</c> event method.
        /// </summary>
        /// <param name="e">The Event Arguments</param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            /*
             * If only one contact type available in AuthorizedAgentCustomerEdit control,
             *  the Session of generic template table will be created in Init event in AuthorizedAgentCustomerEdit control.
             * So move the ClearUIData mehtod to PreInit event to prevent the needed data been clear.
             */
            if (!IsPostBack)
            {
                UIModelUtil.ClearUIData();
            }
        }

        /// <summary>
        /// On Initial event method
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            IsEditable = StandardChoiceUtil.IsCustomerDetailEditable();

            if (!AppSession.IsAdmin)
            {
                chkVerified.Attributes.Add("onclick", "SetWizardButtonDisable('" + btnNextStep.ClientID + "', !this.checked);");
                InitialExport();
            }
        }

        /// <summary>
        /// Page load event method
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The EventArgs object.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
                string bizDomainValue = bizBll.GetBizDomainItemDesc(ConfigManager.AgencyCode, BizDomainConstant.STD_AUTHORIZED_SERVICE, BizDomainConstant.STD_ITEM_AUTHORIZED_SERVICE_FISHING_AND_HUNTING_LICENSE_SALES);

                if (!string.IsNullOrEmpty(bizDomainValue))
                {
                    lblPageHeader.InnerText = bizDomainValue;
                }
                else
                {
                    lblPageHeader.InnerText = LabelUtil.GetTextByKey("aca_authagent_customer_label_pageheader", ModuleName);
                }
            }

            if (AppSession.IsAdmin)
            {
                // the section id need split into 3 or 4 parts which support form design.
                lblSectionPerson.SectionID = string.Format(
                    "{0}{1}{0}{2}{0}{3}",
                    ACAConstant.SPLIT_CHAR,
                    GviewID.AuthAgentCustomerDetail,
                    customerForm.ClientID + "_",
                    IsEditable.ToString().ToLower());

                LicenseList.BindCapList(null, 0, null);
            }

            LicenseList.GridViewSort += LicenseList_GridViewSort;
            LicenseList.PageIndexChanging += LicenseList_GridViewIndexChanging;

            ContactSeqNbr = Request["id"];

            if (!AppSession.IsAdmin && !IsPostBack)
            {
                PeopleModel4WS peopleModel4WS = null;

                // bind contact form
                if (!string.IsNullOrEmpty(ContactSeqNbr))
                {
                    // it is edit page
                    PeopleModel people = AppSession.GetPeopleModelFromSession(ContactSeqNbr);

                    if (people != null)
                    {
                        // fill the customer form
                        CapContactModel capContactModel = new CapContactModel();
                        capContactModel.people = people;
                        peopleModel4WS = TempModelConvert.ConvertToPeopleModel4WS(people);
                        customerForm.FillCustomerForm(capContactModel);

                        BindLicensesByCustomer(0, null);
                    }

                    if (!IsEditable)
                    {
                        chkVerified.Visible = false;
                    }
                }
                else
                {
                    // it is new page
                    divLicenses.Visible = false;
                }

                CreateContactParametersSession(peopleModel4WS);
            }
        }

        /// <summary>
        /// Save the customer information and go to next step
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event arguments</param>
        protected void NextStepButton_Click(object sender, EventArgs e)
        {
            try
            {
                PeopleModel peopleSavedModel = null;

                if (IsEditable || string.IsNullOrEmpty(ContactSeqNbr))
                {
                    PeopleModel peopleModel = customerForm.GetPeopleModel();
                    if (!Validate4NextStep(AuthServiceSetting, peopleModel))
                    {
                        return;
                    }

                    IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
                    peopleSavedModel = peopleBll.CreateOrEditCustomers(peopleModel);
                }
                else
                {
                    peopleSavedModel = AppSession.GetPeopleModelFromSession(ContactSeqNbr);
                }

                // set the saved people model to session
                AppSession.SetPeopleModelToSession(peopleSavedModel.contactSeqNumber, peopleSavedModel);

                string redirectUrl = string.Format(
                    "{0}?{1}={2}&{3}={4}&{5}={6}&{7}={8}",
                    "~/Cap/CapType.aspx",
                    UrlConstant.AgencyCode,
                    ConfigManager.AgencyCode,
                    ACAConstant.MODULE_NAME,
                    AuthServiceSetting.ModuleName,
                    UrlConstant.FILTER_NAME,
                    HttpUtility.UrlEncode(AuthServiceSetting.CapTypeFilterName),
                    UrlConstant.CONTACT_SEQ_NUMBER,
                    peopleSavedModel.contactSeqNumber);

                Response.Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// Relocate the Customer to search page.
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event arguments</param>
        protected void RelocateButton_Click(object sender, EventArgs e)
        {
            string url = string.Format(
                "SearchCustomer.aspx?relocate={0}&{1}={2}&soundex={3}", 
                ACAConstant.COMMON_Y, 
                UrlConstant.CAPTYPE, 
                Request.QueryString[UrlConstant.CAPTYPE],
                ValidationUtil.IsYes(Request.QueryString["soundex"]) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N);

            Response.Redirect(url);
        }

        /// <summary>
        /// License list grid view index change event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void LicenseList_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(LicenseList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                BindLicensesByCustomer(e.NewPageIndex, pageInfo.SortExpression);
            }
        }

        /// <summary>
        /// LicenseList GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void LicenseList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            //when user trigger this Gridview pageIndexChanged event, permitList.pageIndex will be set to zero
            int currentPageIndex = 0;
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(LicenseList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;

            //when sorting gridview, we use GridViewDataSource as the data, so ,needn't pass the first parameters
            LicenseList.BindCapList(null, currentPageIndex, e.GridViewSortExpression);
        }

        /// <summary>
        /// OnPreRender event method.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //Don't run expression in administration model.
            if (!AppSession.IsAdmin)
            {
                ExpressionUtil.RegisterScriptLibToCurrentPage(this);

                if (!Page.IsPostBack)
                {
                    // Clear some temporary data stored in seestion for expression 
                    ExpressionUtil.ClearExpressionVariables();
                    RegisterExpressionOnLoad();
                }

                RegisterExpressionOnSubmit();
                ExpressionUtil.ResetJsExpression(this);
            }
        }

        /// <summary>
        /// Handle the LoadComplete event.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);

            if (ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
            {
                //Handle ASIT expression behaviors.
                ExpressionUtil.HandleASITPostbackBehavior(Page);
            }
        }

        #endregion protected Methods
        
        #region Private Methods

        /// <summary>
        /// register JS function.
        /// register "javascript" code into page to running expression for "on submit".
        /// </summary>
        private void RegisterExpressionOnSubmit()
        {
            var callJsFunction = ExpressionUtil.GetExpressionScript(true, customerForm);
            var strSubmitFuction = ExpressionUtil.GetExpressionScriptOnSubmit(callJsFunction);

            if (!Page.ClientScript.IsStartupScriptRegistered("OnSubmitExpression"))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "OnSubmitExpression", strSubmitFuction, true);
            }
        }

        /// <summary>
        /// register "javascript" code into page to running expression for "on submit".
        /// </summary>
        private void RegisterExpressionOnLoad()
        {
            var callJsFunction = ExpressionUtil.GetExpressionScript(false, customerForm);

            if (!Page.ClientScript.IsStartupScriptRegistered("OnLoadExpression"))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "OnLoadExpression", callJsFunction, true);
            }
        }

        /// <summary>
        /// Bind license by customer
        /// </summary>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        private void BindLicensesByCustomer(int currentPageIndex, string sortExpression)
        {
            if (AppSession.User.IsAnonymous)
            {
                return;
            }

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(LicenseList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = LicenseList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
            SimpleCapModel[] simpleCap = capBll.GetLicensesByCustomer(ContactSeqNbr, AuthServiceSetting.ModuleName, AuthServiceSetting.CapTypeFilterName, queryFormat, AppSession.User.UserSeqNum);
            DataTable licenseList = PaginationUtil.MergeDataSource(LicenseList.GridViewDataSource, LicenseList.CreateDataSource(simpleCap), pageInfo);

            LicenseList.BindCapList(licenseList);
        }

        /// <summary>
        /// Do validation before goto next step (SPEAR form page flow).
        /// </summary>
        /// <param name="authSettingModel">The authorized agent setting.</param>
        /// <param name="peopleModel">The people model.</param>
        /// <returns>Return whether it is validate or not.</returns>
        private bool Validate4NextStep(AuthorizedServiceSettingModel authSettingModel, PeopleModel peopleModel)
        {
            // validate the authorized agent's setting for filter name/record type/module name
            CapTypeModel[] capTypes = null;

            if (authSettingModel != null)
            {
                ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();
                capTypes = capTypeBll.GetGeneralCapTypeList(authSettingModel.ModuleName, authSettingModel.CapTypeFilterName, ACAConstant.VCH_TYPE_VHAPP, AppSession.User.PublicUserId);
            }

            if (capTypes == null || capTypes.Length == 0)
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, GetTextByKey("aca_authagent_customerdetail_msg_nobasetypeconfig"));
                return false;
            }

            // validate the duplicate contact identity
            PeopleModel4WS searchModel = TempModelConvert.ConvertToPeopleModel4WS(peopleModel);
            string message = PeopleUtil.GetIdentityKeyMessage(searchModel, customerForm.IdentityFieldLabels, "aca_authagent_customerdetail_msg_identityduplicate");

            if (!string.IsNullOrEmpty(message))
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, message);
                return false;
            }

            string errorMsg = customerForm.ValidateContactAddress();

            if (!string.IsNullOrEmpty(errorMsg))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Initial export by setting.
        /// </summary>
        private void InitialExport()
        {
            if (StandardChoiceUtil.IsEnableExport2CSV())
            {
                LicenseList.InitialExport(true);
                LicenseList.ExportFileName = "AssociatedLicenseList";
            }
            else
            {
                LicenseList.InitialExport(false);
            }
        }
        #endregion
    }
}