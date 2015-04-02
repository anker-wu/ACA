#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapCompletions.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapCompletions.aspx.cs 77905 2008-10-15 11:11:28Z ACHIEVO\Kale.Huang
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Admin;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.BLL.Report;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.CAP;
using Accela.ACA.Web.Component;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using log4net;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// receipt page class after payment.
    /// </summary>
    public partial class CapCompletions : CapCompletionBasePage
    {
        #region Fields

        /// <summary>
        /// Renew License.
        /// </summary>
        private const string ACTIONSOURCE_RENEWLICENSE = "RenewLicense";

        /// <summary>
        /// all successful.
        /// </summary>
        private const int PAYMENT_ALLSECCESSFULL = 0;

        /// <summary>
        /// all failed
        /// </summary>
        private const int PAYMENT_FAILED = 2;

        /// <summary>
        /// other case.
        /// </summary>
        private const int PAYMENT_OTHERS = 3;

        /// <summary>
        /// partly successful.
        /// </summary>
        private const int PAYMENT_PARTSECCESSFULL = 1;

        /// <summary>
        /// use for log.
        /// </summary>
        private static readonly ILog Loggers = LogFactory.Instance.GetLogger(typeof(CapCompletions));

        /// <summary>
        /// weather is super agency.
        /// </summary>
        private bool _isSuperAgency;

        /// <summary>
        /// payment result.
        /// </summary>
        private CapPaymentResultWithAddressModel[] capPaymentResultWithAddressModels = null;

        /// <summary>
        /// SimpleViewElementModel4WS model list.
        /// </summary>
        private Hashtable simpleViewElements = new Hashtable();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance has ready2 schedule examination.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has ready2 schedule examination; otherwise, <c>false</c>.
        /// </value>
        protected bool HasReady2ScheduleExamination
        {
            get
            {
                bool result = false;

                CapPaymentResultModel[] capPaymentResultModels = GetCapPaymentResultModelFromSession();

                // Defer payment function will made the payment result is null.
                if (capPaymentResultModels != null)
                {
                    CapIDModel[] capIdModels = capPaymentResultModels.Where(o => o.paymentStatus).Select(o => o.capID).ToArray();

                    if (capIdModels.Length != 0)
                    {
                        IExaminationBll examinationBll = ObjectFactory.GetObject<IExaminationBll>();
                        result = examinationBll.HasReady2ScheduleExamination(capIdModels);
                    }
                }

                return result;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Schedules the examinations.
        /// </summary>
        /// <returns>Schedule result</returns>
        [WebMethod(EnableSession = true, Description = "Schedule Examinations")]
        public static string ScheduleExaminations()
        {
            string result = string.Empty;
            CapPaymentResultModel[] capPaymentResultModels = GetCapPaymentResultModelFromSession();

            // Defer payment function will made the payment result is null.
            if (capPaymentResultModels != null)
            {
                CapIDModel[] capIdModels = capPaymentResultModels.Where(o => o.paymentStatus).Select(o => o.capID).ToArray();

                if (capIdModels != null && capIdModels.Length != 0)
                {
                    IExaminationBll examinationBll = ObjectFactory.GetObject<IExaminationBll>();
                    result = examinationBll.ScheduleAllPaidExam(capIdModels, AppSession.User.PublicUserId);
                }
            }

            return result;
        }

        /// <summary>
        /// unload event.
        /// </summary>
        /// <param name="e">event handle.</param>
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            AppSession.ShoppingCartBreadcrumbParams = null;
        }

        /// <summary>
        /// when page load, raise this event.
        /// </summary>
        /// <param name="sender">system sender.</param>
        /// <param name="e">event args..</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";

            if (!IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    divDependentCapTypeSetting.Visible = true;
                    divConditionsSetting.Visible = true;
                }

                try
                {
                    ShoppingCartUtil.SetCartItemNumber();

                    if (!AppSession.IsAdmin)
                    {
                        if (AppSession.User.IsAnonymous || !StandardChoiceUtil.IsEnableShoppingCart())
                        {
                            BreadCrumpShoppingCart.Visible = false;
                            BreadCrumbBar.PageFlow = BreadCrumpUtil.GetPageFlowConfig(this.ModuleName, false);
                        }
                        else
                        {
                            BreadCrumbBar.Visible = false;
                            BreadCrumpShoppingCart.PageFlow = BreadCrumpUtil.GetShoppingCartFlowConfig();
                        }
                    }

                    AppSession.SetPageflowGroupToSession(null);

                    DisplayUIMsgAndBtns();
                    GetGviewElementByModules();
                    BindAddress();

                    if (!AppSession.IsAdmin)
                    {
                        CapUtil.ShowResultMessage(authMessage, this.ModuleName);
                        sepForHeader.Visible = true;
                        ReportBtn.BatchTransactionNbr = GetBatchTransactionNbr();
                        ReportBtnTop.BatchTransactionNbr = ReportBtn.BatchTransactionNbr;
                        ReportBtn.ModuleList = GetModuleList();
                        ReportBtnTop.ModuleList = ReportBtn.ModuleList;
                        ReportBtn.CapIDArray = GetCapIDArray();
                        ReportBtnTop.CapIDArray = ReportBtn.CapIDArray;
                    }

                    // Only 1 web service call for two report button.
                    IReportBll reportBll = ObjectFactory.GetObject<IReportBll>();
                    ReportButtonPropertyModel4WS[] reportButtonProperties = reportBll.GetReportButtonProperty(ReportBtn.CapID, AppSession.User.PublicUserId, ReportBtn.ModuleList);
                    ReportBtn.ButtonProperties = reportButtonProperties;
                    ReportBtnTop.ButtonProperties = reportButtonProperties;
                }
                catch (ACAException ex)
                {
                    MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
                }
            }
        }

        /// <summary>
        /// bind address to UI.
        /// </summary>
        /// <param name="sender">system sender.</param>
        /// <param name="e">data list event args.</param>
        protected void AddressList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DataList agenceList = (DataList)e.Item.FindControl("agenciesList");
            CapPaymentResultWithAddressModel capPaymentResultWithAddressModel = (CapPaymentResultWithAddressModel)e.Item.DataItem;
            string moduleName = capPaymentResultWithAddressModel.moduleName;
            string addressName = ShoppingCartUtil.FormatAddress(capPaymentResultWithAddressModel.address, (SimpleViewElementModel4WS[])simpleViewElements[moduleName]);

            AccelaLabel lblAddress = (AccelaLabel)e.Item.FindControl("lblAddress");

            if (string.IsNullOrEmpty(addressName))
            {
                addressName = string.Empty;
                lblAddress.LabelKey = "per_shoppingcart_label_noaddressselected";
            }
            else
            {
                lblAddress.Text = ShoppingCartUtil.FormatAddress(capPaymentResultWithAddressModel.address, (SimpleViewElementModel4WS[])simpleViewElements[moduleName]);
            }

            ArrayList agences = new ArrayList();

            agences = ConstrustAgencyData(addressName);

            agenceList.DataSource = agences;
            agenceList.DataBind();
        }

        /// <summary>
        /// bind agency to UI.
        /// </summary>
        /// <param name="sender">system sender.</param>
        /// <param name="e">data list event args.</param>
        protected void AgenciesList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (!_isSuperAgency)
            {
                HtmlTable tbAgency = (HtmlTable)e.Item.FindControl("tbAgency");
                tbAgency.Visible = false;
            }

            DataList dataList = (DataList)e.Item.FindControl("capsList");

            HtmlTableCell tdLogo = (HtmlTableCell)e.Item.FindControl("tdLogo");
            HtmlImage imgAgencyLogo = (HtmlImage)e.Item.FindControl("imgAgencyLogo");

            CapPaymentResultWithAddressModel capPaymentResultWithAddressModel = (CapPaymentResultWithAddressModel)e.Item.DataItem;
            string currentAgenceCode = capPaymentResultWithAddressModel.capID.serviceProviderCode;
            string addressName = ShoppingCartUtil.FormatAddress(capPaymentResultWithAddressModel.address, (SimpleViewElementModel4WS[])simpleViewElements[capPaymentResultWithAddressModel.moduleName]);

            ArrayList caps = ConstrustCapData(currentAgenceCode, addressName);

            dataList.DataSource = caps;
            dataList.DataBind();

            ILogoBll logo = (ILogoBll)ObjectFactory.GetObject(typeof(ILogoBll));

            LogoModel logoModel = logo.GetAgencyLogo(currentAgenceCode);

            if (logoModel != null)
            {
                tdLogo.Visible = true;
                imgAgencyLogo.Src = FileUtil.AppendApplicationRoot("Cap/ImageHandler.aspx?") + UrlConstant.AgencyCode + "=" + logoModel.serviceProviderCode;
                imgAgencyLogo.Alt = GetTextByKey("aca_common_msg_imgalt_agencylogo");

                if (!string.IsNullOrEmpty(logoModel.docDesc))
                {
                    imgAgencyLogo.Alt = logoModel.docDesc;
                }
            }
            else
            {
                tdLogo.Visible = false;
            }
        }

        /// <summary>
        /// bind cap list to UI
        /// </summary>
        /// <param name="sender">system sender.</param>
        /// <param name="e">data list event args.</param>
        protected void CapsList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            CapPaymentResultWithAddressModel capPaymentResultWithAddressModel = (CapPaymentResultWithAddressModel)e.Item.DataItem;
            CapIDModel4WS capIDModel = capPaymentResultWithAddressModel.capID;
            AccelaLinkButton hlCAPDetail = (AccelaLinkButton)e.Item.FindControl("hlCAPDetail");

            if (_isSuperAgency)
            {
                Label lblCapType = (Label)e.Item.FindControl("lblCapType");
                lblCapType.Text = capPaymentResultWithAddressModel.capType;
            }

            AccelaLinkButton hlCAPPermit = (AccelaLinkButton)e.Item.FindControl("hlCAPPermit");
            AccelaLinkButton hlCAPReceipt = (AccelaLinkButton)e.Item.FindControl("hlCAPReceipt");
            AccelaLinkButton hlCAPSummary = (AccelaLinkButton)e.Item.FindControl("hlCAPSummary");
            AccelaLinkButton hlCloneRecord = (AccelaLinkButton)e.Item.FindControl("hlCloneRecord");
            AccelaLinkButton lnkLabelPrint = (AccelaLinkButton)e.Item.FindControl("lnkLabelPrint");
            DependentCapTypeList dependentCapTypeList = (DependentCapTypeList)e.Item.FindControl("dependentCapTypeList");

            CapIDModel4WS capID = capPaymentResultWithAddressModel.capID;
            string subModuleName = capPaymentResultWithAddressModel.moduleName;

            string capDetailUrl = string.Format("CapDetail.aspx?Module={0}&TabName={0}&capID1={1}&capID2={2}&capID3={3}&{4}={5}", ScriptFilter.AntiXssUrlEncode(subModuleName), Server.UrlEncode(capID.id1), Server.UrlEncode(capID.id2), Server.UrlEncode(capID.id3), UrlConstant.AgencyCode, capID.serviceProviderCode);
            hlCAPDetail.PostBackUrl = capDetailUrl;

            DisplayReportLink(capPaymentResultWithAddressModel, hlCAPPermit, hlCAPReceipt, hlCAPSummary, lnkLabelPrint);
            DisplayCloneLink(capPaymentResultWithAddressModel.capID, hlCloneRecord, capPaymentResultWithAddressModel.moduleName);
            dependentCapTypeList.DisplayDependentCapTypeLink(capIDModel, capPaymentResultWithAddressModel.moduleName);

            // display conditions
            CapConditions capConditions = (CapConditions)e.Item.FindControl("capConditions");
            CapWithConditionModel4WS capWithConditionModel4WS = CapUtil.GetCapWithConditionModel4WS(capID, AppSession.User.UserSeqNum, true, null);

            capConditions.Display(capWithConditionModel4WS);
        }

        /// <summary>
        /// Get Payment Result model from session.
        /// </summary>
        /// <returns>Cap payment Result model list.</returns>
        private static CapPaymentResultModel[] GetCapPaymentResultModelFromSession()
        {
            OnlinePaymentResultModel onlinePaymentResultModel = AppSession.GetOnlinePaymentResultModelFromSession();

            if (onlinePaymentResultModel == null)
            {
                return null;
            }

            CapPaymentResultModel[] capPaymentResultModels = onlinePaymentResultModel.capPaymentResultModels;

            return capPaymentResultModels;
        }

        /// <summary>
        /// Gets clone url used by UI page.
        /// </summary>
        /// <param name="subCapID">The sub cap ID.</param>
        /// <param name="hlCloneRecord">The clone record.</param>
        /// <param name="subModuleName">The sub module name.</param>
        private void DisplayCloneLink(CapIDModel4WS subCapID, AccelaLinkButton hlCloneRecord, string subModuleName)
        {
            if (subCapID != null && !string.IsNullOrEmpty(subCapID.serviceProviderCode) && !string.IsNullOrEmpty(subCapID.id1))
            {
                ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
                CapTypeModel capType = capTypeBll.GetCapTypeByCapID(subCapID);

                if (CloneRecordUtil.IsDisplayCloneButton(capType, TempModelConvert.Trim4WSOfCapIDModel(subCapID), subModuleName, true) && !this.IsForceLoginToApplyPermit(subModuleName))
                {
                    string cloneUrl = CloneRecordUtil.BuildClonePageUrl(TempModelConvert.Trim4WSOfCapIDModel(subCapID), subModuleName);
                    hlCloneRecord.Attributes.Add("onclick", "ShowCloneDialog('" + cloneUrl + "','" + hlCloneRecord.ClientID + "');return false;");
                    hlCloneRecord.Visible = true;
                }
            }
        }

        /// <summary>
        /// bind Address list data
        /// </summary>
        private void BindAddress()
        {
            CapPaymentResultModel[] capPaymentResultModels = GetCapPaymentResultModelFromSession();
            capPaymentResultModels = GetSuccessFullCapPaymentResult(capPaymentResultModels);

            if (capPaymentResultModels == null || capPaymentResultModels.Length == 0)
            {
                return;
            }

            capPaymentResultWithAddressModels = ConstructCapPaymentReslutWithAddressModel(capPaymentResultModels);

            ArrayList addressArray = new ArrayList();
            addressArray = GetAddress(capPaymentResultWithAddressModels);
            addressList.DataSource = addressArray;
            addressList.DataBind();
        }

        /// <summary>
        /// bind the permit link url to button
        /// </summary>
        /// <param name="hlCAPPermit">button which need the link</param>
        /// <param name="reportID">report id.</param>
        /// <param name="subCapID">sub cap id</param>
        /// <param name="subModuleName">sub module name</param>
        private void BindPermitLinkUrl(AccelaLinkButton hlCAPPermit, string reportID, CapIDModel4WS subCapID, string subModuleName)
        {
            //Summary Report Link
            string permitReportUrl = string.Format(
                                                    ACAConstant.REPORT_PARAMETER_PAGE + "?Module={0}&reportType={1}&reportID={2}&subID1={3}&subID2={4}&subID3={5}&{6}={7}&subCustomerID={8}&SubModule={9}",
                                                    ModuleName,
                                                    ACAConstant.PRINT_PERMIT_REPORT,
                                                    reportID,
                                                    Server.UrlEncode(subCapID.id1),
                                                    Server.UrlEncode(subCapID.id2),
                                                    Server.UrlEncode(subCapID.id3),
                                                    UrlConstant.AgencyCode,
                                                    subCapID.serviceProviderCode,
                                                    Server.UrlEncode(subCapID.customID),
                                                    subModuleName);
            hlCAPPermit.Attributes.Add("onclick", "print_onclick('" + permitReportUrl + "');return false;");
        }

        /// <summary>
        /// bind the receipt link url to button.
        /// </summary>
        /// <param name="hlCAPReceipt">button which need the link</param>
        /// <param name="receiptNbr">the receipt number.</param>
        /// <param name="reportID">the report id.</param>
        /// <param name="subCapID">sub cap id</param>
        /// <param name="subModuleName">sub module name</param>
        private void BindReceiptLinkUrl(AccelaLinkButton hlCAPReceipt, string receiptNbr, string reportID, CapIDModel4WS subCapID, string subModuleName)
        {
            //Receipt Report Link
            string receiptReportUrl = string.Format(
                                                    ACAConstant.REPORT_PARAMETER_PAGE + "?Module={0}&reportType={1}&RecepitNbr={2}&reportID={3}&subID1={4}&subID2={5}&subID3={6}&{7}={8}&subCustomerID={9}&SubModule={10}",
                                                    ModuleName,
                                                    ACAConstant.PRINT_PAYMENT_RECEIPT_REPORT,
                                                    receiptNbr,
                                                    reportID,
                                                    Server.UrlEncode(subCapID.id1),
                                                    Server.UrlEncode(subCapID.id2),
                                                    Server.UrlEncode(subCapID.id3),
                                                    UrlConstant.AgencyCode,
                                                    subCapID.serviceProviderCode,
                                                    Server.UrlEncode(subCapID.customID),
                                                    subModuleName);
            hlCAPReceipt.Attributes.Add("onclick", "print_onclick('" + receiptReportUrl + "');return false;");
        }

        /// <summary>
        /// Bind the SummaryLink Url to button.
        /// </summary>
        /// <param name="hlCAPSummary">button which need the link</param>
        /// <param name="reportID">the report id.</param>
        /// <param name="subCapID">the sub cap id</param>
        /// <param name="subModuleName">sub module name.</param>
        private void BindSummaryLinkUrl(AccelaLinkButton hlCAPSummary, string reportID, CapIDModel4WS subCapID, string subModuleName)
        {
            //Summary Report Link
            string summaryReportUrl = string.Format(
                                                    ACAConstant.REPORT_PARAMETER_PAGE + "?Module={0}&reportType={1}&reportID={2}&subID1={3}&subID2={4}&subID3={5}&{6}={7}&subCustomerID={8}&SubModule={9}",
                                                    ModuleName,
                                                    ACAConstant.PRINT_PERMIT_SUMMARY_REPORT,
                                                    reportID,
                                                    Server.UrlEncode(subCapID.id1),
                                                    Server.UrlEncode(subCapID.id2),
                                                    Server.UrlEncode(subCapID.id3),
                                                    UrlConstant.AgencyCode,
                                                    subCapID.serviceProviderCode,
                                                    Server.UrlEncode(subCapID.customID),
                                                    subModuleName);
            hlCAPSummary.Attributes.Add("onclick", "print_onclick('" + summaryReportUrl + "');return false;");
        }

        /// <summary>
        /// get Cap PaymentResultWithAddress model list with CapPaymentResultModel list
        /// </summary>
        /// <param name="capPaymentResultModels">payment result model.</param> 
        /// <returns>Cap PaymentResultWithAddress model list </returns>
        private CapPaymentResultWithAddressModel[] ConstructCapPaymentReslutWithAddressModel(CapPaymentResultModel[] capPaymentResultModels)
        {
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
            CapPaymentResultWithAddressModel[] capPaymentResultWithAddressModels = capBll.ConstructCapPaymentResultWithAddressModel(capPaymentResultModels);

            /* The 'hasFee' attribute to be used to control the Receipt button display or not.
             * For the Associated Forms Parent and all Child Records, only display the Receipt button on Parent Record.
             */
            if (capPaymentResultWithAddressModels != null && capPaymentResultWithAddressModels.Length > 0)
            {
                foreach (CapPaymentResultWithAddressModel paymentResult in capPaymentResultWithAddressModels)
                {
                    CapIDModel4WS capID = paymentResult.capID;
                    CapIDModel4WS parentCapID = CapUtil.GetParentAssoFormCapID(capID);
                    if (!parentCapID.Equals(capID))
                    {
                        CapPaymentResultWithAddressModel parentPaymentResult = capPaymentResultWithAddressModels.SingleOrDefault(p => p.capID.Equals(parentCapID));

                        if (parentPaymentResult != null)
                        {
                            if (!parentPaymentResult.hasFee && paymentResult.hasFee)
                            {
                                parentPaymentResult.hasFee = true;
                            }

                            paymentResult.hasFee = false;
                        }
                    }
                }
            }

            return capPaymentResultWithAddressModels;
        }

        /// <summary>
        /// construct agency data.
        /// </summary>
        /// <param name="addressName">the address name.</param>
        /// <returns>payment result model list.</returns>
        private ArrayList ConstrustAgencyData(string addressName)
        {
            ArrayList agencys = new ArrayList();

            if (capPaymentResultWithAddressModels == null || capPaymentResultWithAddressModels.Length <= 0)
            {
                return null;
            }

            SortedList list = new SortedList();

            for (int i = 0; i < capPaymentResultWithAddressModels.Length; i++)
            {
                string displayAddress = ShoppingCartUtil.FormatAddress(capPaymentResultWithAddressModels[i].address, (SimpleViewElementModel4WS[])simpleViewElements[capPaymentResultWithAddressModels[i].moduleName]);
                if (displayAddress == addressName)
                {
                    string tempAgencyCode = capPaymentResultWithAddressModels[i].capID.serviceProviderCode;
                    if (!list.ContainsKey(tempAgencyCode))
                    {
                        list.Add(tempAgencyCode, capPaymentResultWithAddressModels[i]);
                    }
                }
            }

            foreach (DictionaryEntry de in list)
            {
                agencys.Add(de.Value);
            }

            return agencys;
        }

        /// <summary>
        /// construct cap data.
        /// </summary>
        /// <param name="agencyCode">the agency code.</param>
        /// <param name="addressName">the address name.</param>
        /// <returns>payment result module list.</returns>
        private ArrayList ConstrustCapData(string agencyCode, string addressName)
        {
            ArrayList caps = new ArrayList();

            if (capPaymentResultWithAddressModels == null || capPaymentResultWithAddressModels.Length <= 0)
            {
                return null;
            }

            SortedList list = new SortedList();

            int index = 0;
            foreach (CapPaymentResultWithAddressModel capPaymentResultWithAddressModel in capPaymentResultWithAddressModels)
            {
                string displayAddress = ShoppingCartUtil.FormatAddress(capPaymentResultWithAddressModel.address, (SimpleViewElementModel4WS[])simpleViewElements[capPaymentResultWithAddressModel.moduleName]);
                if (capPaymentResultWithAddressModel.capID.serviceProviderCode == agencyCode && displayAddress == addressName)
                {
                    list.Add(index, capPaymentResultWithAddressModel);
                }

                index++;
            }

            foreach (DictionaryEntry de in list)
            {
                caps.Add(de.Value);
            }

            return caps;
        }

        /// <summary>
        /// show permit link
        /// </summary>
        /// <param name="subCapID">sub cap id</param>
        /// <param name="hlCAPPermit">the permit button.</param>
        /// <param name="buttonProperty">Report button property object</param>
        /// <param name="subModuleName">sub module name</param>
        private void DisplayPermitLink(CapIDModel4WS subCapID, AccelaLinkButton hlCAPPermit, ReportButtonPropertyModel4WS buttonProperty, string subModuleName)
        {
            string errorInfo = buttonProperty.errorInfo;
            string reportID = buttonProperty.reportId;
            string reportName = I18nStringUtil.GetString(buttonProperty.resReportName, buttonProperty.reportName);
            bool isDisplayed = buttonProperty.isDisplayed;

            if (string.IsNullOrEmpty(errorInfo) && isDisplayed)
            {
                hlCAPPermit.Visible = true;
                BindPermitLinkUrl(hlCAPPermit, reportID, subCapID, subModuleName);
            }
            else
            {
                Loggers.Error(errorInfo);
                hlCAPPermit.Visible = false;
            }
        }

        /// <summary>
        /// show permit summary link
        /// </summary>
        /// <param name="subCapID">sub cap id</param>
        /// <param name="hlCAPSummary">the cap summary link button.</param>
        /// <param name="buttonProperty">Report button property object</param>
        /// <param name="subModuleName">sub module name</param>
        private void DisplayPermitSummaryLink(CapIDModel4WS subCapID, AccelaLinkButton hlCAPSummary, ReportButtonPropertyModel4WS buttonProperty, string subModuleName)
        {
            string errorInfo = buttonProperty.errorInfo;
            string reportID = buttonProperty.reportId;
            string reportName = I18nStringUtil.GetString(buttonProperty.resReportName, buttonProperty.reportName);
            bool isDisplayed = buttonProperty.isDisplayed;

            if (string.IsNullOrEmpty(errorInfo) && isDisplayed)
            {
                hlCAPSummary.Visible = true;
                BindSummaryLinkUrl(hlCAPSummary, reportID, subCapID, subModuleName);
            }
            else
            {
                Loggers.Error(errorInfo);
                hlCAPSummary.Visible = false;
            }
        }

        /// <summary>
        /// show receipt receipt link
        /// </summary>
        /// <param name="subCapID">sub cap id</param>
        /// <param name="receiptNbr">Receipt number</param>
        /// <param name="hlCAPReceipt">the receipt link button.</param>
        /// <param name="buttonProperty">Report button property object</param>
        /// <param name="subModuleName">sub module name</param>
        private void DisplayReceiptLink(CapIDModel4WS subCapID, string receiptNbr, AccelaLinkButton hlCAPReceipt, ReportButtonPropertyModel4WS buttonProperty, string subModuleName)
        {
            string errorInfo = buttonProperty.errorInfo;
            string reportID = buttonProperty.reportId;
            string reportName = I18nStringUtil.GetString(buttonProperty.resReportName, buttonProperty.reportName);
            bool isDisplayed = buttonProperty.isDisplayed;

            if (string.IsNullOrEmpty(errorInfo) && isDisplayed && !string.IsNullOrEmpty(receiptNbr))
            {
                hlCAPReceipt.Visible = true;
                BindReceiptLinkUrl(hlCAPReceipt, receiptNbr, reportID, subCapID, subModuleName);
            }
            else
            {
                if (!string.IsNullOrEmpty(errorInfo))
                {
                    Loggers.Error(errorInfo);
                }

                if (string.IsNullOrEmpty(receiptNbr))
                {
                    Loggers.Error("Because no receipt number, Receipt Report Button will disappears in the page.");
                }

                hlCAPReceipt.Visible = false;
            }
        }

        /// <summary>
        /// show report link of permit/receipt/summary
        /// </summary>
        /// <param name="paymentResult">Payment result model</param>
        /// <param name="hlCAPPermit">permit button</param>
        /// <param name="hlCAPReceipt">receipt button</param>
        /// <param name="hlCAPSummary">summary button</param>
        /// <param name="lnkLabelPrint">the label print button.</param>
        private void DisplayReportLink(CapPaymentResultWithAddressModel paymentResult, AccelaLinkButton hlCAPPermit, AccelaLinkButton hlCAPReceipt, AccelaLinkButton hlCAPSummary, AccelaLinkButton lnkLabelPrint)
        {
            CapIDModel4WS subCapID = paymentResult.capID;
            string receiptNbr = paymentResult.receiptNbr;
            string subModuleName = paymentResult.moduleName;
            bool hasFee = paymentResult.hasFee;
            bool isRenewal = string.Equals(ACTIONSOURCE_RENEWLICENSE, paymentResult.actionSource);
            bool isEnableReportForAnonymousUser = StandardChoiceUtil.IsEnableReportForAnonymousUser();

            IReportBll reportBll = (IReportBll)ObjectFactory.GetObject(typeof(IReportBll));

            ReportButtonPropertyModel4WS[] arrayRBTModel = reportBll.GetReportButtonProperty(subCapID, AppSession.User.PublicUserId, subModuleName);
            foreach (ReportButtonPropertyModel4WS ws in arrayRBTModel)
            {
                string buttonName = ws.buttonName;

                if (ACAConstant.PRINT_PERMIT_REPORT.Equals(buttonName, StringComparison.InvariantCulture) && isEnableReportForAnonymousUser && !isRenewal)
                {
                    DisplayPermitLink(subCapID, hlCAPPermit, ws, subModuleName);
                    continue;
                }
                else if (hasFee && ACAConstant.PRINT_PAYMENT_RECEIPT_REPORT.Equals(buttonName, StringComparison.InvariantCulture) && isEnableReportForAnonymousUser)
                {
                    DisplayReceiptLink(subCapID, receiptNbr, hlCAPReceipt, ws, subModuleName);
                    continue;
                }
                else if (ACAConstant.PRINT_PERMIT_SUMMARY_REPORT.Equals(buttonName, StringComparison.InvariantCulture) && !isRenewal)
                {
                    DisplayPermitSummaryLink(subCapID, hlCAPSummary, ws, subModuleName);
                    continue;
                }
                else if (ACAConstant.PRINT_LABEL_VIEWID.Equals(buttonName, StringComparison.InvariantCulture) && AppSession.User.IsAuthAgentNeedPrinter)
                {
                    try
                    {
                        DisplayLabelPrintLink(subCapID, lnkLabelPrint, ws, receiptNbr, subModuleName);
                    }
                    catch (Exception ex)
                    {
                        MessageUtil.ShowMessage(this.Page, MessageType.Error, ex.Message);
                    }

                    continue;
                }
            }
        }

        /// <summary>
        /// Displays the label print link.
        /// </summary>
        /// <param name="subCapID">The sub cap ID.</param>
        /// <param name="lnkLabelPrint">The LNK label print.</param>
        /// <param name="buttonProperty">The button property.</param>
        /// <param name="receiptNbr">The receipt number.</param>
        /// <param name="subModuleName">Name of the sub module.</param>
        private void DisplayLabelPrintLink(CapIDModel4WS subCapID, AccelaLinkButton lnkLabelPrint, ReportButtonPropertyModel4WS buttonProperty, string receiptNbr, string subModuleName)
        {
            if ((AppSession.User.IsAgentClerk || AppSession.User.IsAuthorizedAgent) && !string.IsNullOrEmpty(buttonProperty.reportId))
            {
                lnkLabelPrint.Visible = true;
                IReportBll reportBll = ObjectFactory.GetObject<IReportBll>();

                ReportDetailModel4WS detailModel = reportBll.GetReportDetail(buttonProperty.reportId, ConfigManager.AgencyCode);

                if (detailModel != null)
                {
                    if (!ReportUtil.HandlerReprint(detailModel, subCapID.customID, false))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "disablebutton" + Guid.NewGuid(), "<script>DisableButton('" + lnkLabelPrint.ClientID + "', true);</script>", false);
                    }
                    else
                    {
                        ParameterModel4WS[] parameters = ReportUtil.GetReportParameters(buttonProperty.reportId, ACAConstant.PRINT_LABEL_VIEWID, subModuleName);
                        var visibleParameters = parameters == null
                                                    ? null
                                                    : parameters.Where(o => ValidationUtil.IsYes(o.parameterVisible)).ToList();

                        string receiptLabelUrl =
                            string.Format(
                                "{0}?Module={1}&reportType={2}&RecepitNbr={3}&reportID={4}&batchTransactionNbr={5}&subID1={6}&subID2={7}&subID3={8}&subCustomerID={9}&{10}={11}",
                                ACAConstant.REPORT_PARAMETER_PAGE,
                                subModuleName,
                                ACAConstant.PRINT_LABEL_VIEWID,
                                receiptNbr,
                                buttonProperty.reportId,
                                string.Empty,
                                subCapID.id1,
                                subCapID.id2,
                                subCapID.id3,
                                subCapID.customID,
                                UrlConstant.AgencyCode,
                                ConfigManager.AgencyCode);

                        receiptLabelUrl = FileUtil.AppendApplicationRoot(receiptLabelUrl.Replace("..", string.Empty));
                        bool hasParameters = visibleParameters != null && visibleParameters.Count > 0;

                        string onclickScript = string.Format(
                                                    "handlerPrint('{0}','{1}','{2}','{3}','{4}',{5},'{6}');return false;",
                                                    lnkLabelPrint.ClientID,
                                                    subModuleName,
                                                    buttonProperty.reportId,
                                                    ConfigManager.AgencyCode,
                                                    subCapID.customID,
                                                    hasParameters.ToString().ToLower(),
                                                    receiptLabelUrl);
                        lnkLabelPrint.Attributes.Add("onclick", onclickScript);

                        bool isReprint = ReportUtil.IsLabelReprint(subCapID.customID);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "registerrepring", "var isLabelRePrint=" + isReprint.ToString().ToLower() + ";", true);
                    }
                }
            }
        }

        /// <summary>
        /// display message 
        /// </summary>
        private void DisplayUIMsgAndBtns()
        {
            _isSuperAgency = StandardChoiceUtil.IsSuperAgency();
            ReportBtn.CapID = GetCapID();
            ReportBtnTop.CapID = ReportBtn.CapID;
            ReportBtn.HasFee = IsOneCapHasFee();
            ReportBtnTop.HasFee = ReportBtn.HasFee;
            int successStatus = SuccessStatus();
            switch (successStatus)
            {
                case PAYMENT_ALLSECCESSFULL:
                    resultMessage.Show(MessageType.Success, "per_permitissuance_text_payallsuccessinfo", MessageSeperationType.Bottom);
                    ReportBtn.Visible = true;
                    ReportBtnTop.Visible = true;
                    this.ReportBtn.Failed = false;
                    ReportBtnTop.Failed = false;
                    break;
                case PAYMENT_PARTSECCESSFULL:
                    resultMessage.Show(MessageType.Error, "per_permitissuance_text_paypartsuccessinfo", MessageSeperationType.Bottom);
                    ReportBtn.Visible = true;
                    ReportBtnTop.Visible = true;
                    ReportBtn.Failed = false;
                    ReportBtnTop.Failed = false;
                    break;
                case PAYMENT_FAILED:
                    resultMessage.Show(MessageType.Error, "per_permitissuance_text_payfailedinfo", MessageSeperationType.Bottom);
                    break;
            }

            if (AppSession.IsAdmin)
            {
                resultMessage.Show(MessageType.Success, "per_permitissuance_text_payallsuccessinfo", MessageSeperationType.Bottom);
            }
        }

        /// <summary>
        /// Format the service name.
        /// </summary>
        /// <param name="servicesNameList">the services name list.</param>
        /// <returns>service name after format.</returns>
        private string FormatServiceName(string[] servicesNameList)
        {
            string formatedServicesName = string.Empty;

            if (servicesNameList != null)
            {
                foreach (string servicesName in servicesNameList)
                {
                    formatedServicesName += servicesName + "<br/>";
                }
            }

            return formatedServicesName;
        }

        /// <summary>
        /// Get address from payment result model.
        /// </summary>
        /// <param name="capPaymentResultWithAddressModels">payment result model.</param>
        /// <returns>payment result model list.</returns>
        private ArrayList GetAddress(CapPaymentResultWithAddressModel[] capPaymentResultWithAddressModels)
        {
            ArrayList addressArray = new ArrayList();
            SortedList list = new SortedList();
            if (capPaymentResultWithAddressModels == null)
            {
                return null;
            }

            string tempAddressName = string.Empty;
            bool hasEmptyAddressName = false;
            CapPaymentResultWithAddressModel emptyCapPaymentResultWithAddressModel = new CapPaymentResultWithAddressModel();
            for (int i = 0; i < capPaymentResultWithAddressModels.Length; i++)
            {
                string displayAddress = ShoppingCartUtil.FormatAddress(capPaymentResultWithAddressModels[i].address, (SimpleViewElementModel4WS[])simpleViewElements[capPaymentResultWithAddressModels[i].moduleName]);

                if (string.IsNullOrEmpty(displayAddress))
                {
                    hasEmptyAddressName = true;
                    emptyCapPaymentResultWithAddressModel = capPaymentResultWithAddressModels[i];
                    continue;
                }
                else
                {
                    tempAddressName = displayAddress;
                }

                if (!list.ContainsKey(tempAddressName))
                {
                    list.Add(tempAddressName, capPaymentResultWithAddressModels[i]);
                }
            }

            foreach (DictionaryEntry de in list)
            {
                addressArray.Add(de.Value);
            }

            if (hasEmptyAddressName)
            {
                addressArray.Add(emptyCapPaymentResultWithAddressModel);
            }

            return addressArray;
        }

        /// <summary>
        /// Get Batch Transaction Number From Session
        /// </summary>
        /// <returns>Batch transaction number</returns>
        private string GetBatchTransactionNbr()
        {
            string batchNbr = string.Empty;
            OnlinePaymentResultModel onlinePaymentResultModel = AppSession.GetOnlinePaymentResultModelFromSession();

            if (onlinePaymentResultModel != null && onlinePaymentResultModel.batchNbr != null)
            {
                batchNbr = onlinePaymentResultModel.batchNbr;
            }

            return batchNbr;
        }

        /// <summary>
        /// Get Cap ID Module.
        /// </summary>
        /// <returns>Cap ID Module</returns>
        private CapIDModel4WS GetCapID()
        {
            CapIDModel4WS capID = new CapIDModel4WS();
            capID.serviceProviderCode = ConfigManager.AgencyCode;
            return capID;
        }

        /// <summary>
        /// Get module List From Session.
        /// </summary>
        /// <returns>cap module list.</returns>
        private string GetModuleList()
        {
            string moduleList = string.Empty;

            CapPaymentResultModel[] capPaymentResultModels = GetCapPaymentResultModelFromSession();
            capPaymentResultModels = GetSuccessFullCapPaymentResult(capPaymentResultModels);

            if (capPaymentResultModels == null)
            {
                return string.Empty;
            }

            foreach (CapPaymentResultModel capPaymentResultModel in capPaymentResultModels)
            {
                if (moduleList.IndexOf(capPaymentResultModel.moduleName) < 0)
                {
                    moduleList += capPaymentResultModel.moduleName + ",";
                }
            }

            if (moduleList.IndexOf(",") > 0)
            {
                moduleList = moduleList.Substring(0, moduleList.Length - 1);
            }

            return moduleList;
        }

        /// <summary>
        /// Get Cap ID list string from session for sent email. 
        /// </summary>
        /// <returns>cap id list, the format looks like "FLAGSTAF-09EST-00000-12345,FLAGSTAF-09EST-00000-23456"</returns>
        private string GetCapIDArray()
        {
            StringBuilder capIDArray = new StringBuilder();
            string retureCapIDArray = string.Empty;
            CapPaymentResultModel[] capPaymentResultModels = GetCapPaymentResultModelFromSession();
            capPaymentResultModels = GetSuccessFullCapPaymentResult(capPaymentResultModels);

            if (capPaymentResultModels != null && capPaymentResultModels.Length > 0)
            {
                foreach (CapPaymentResultModel capPaymentResultModel in capPaymentResultModels)
                {
                    if (!string.IsNullOrEmpty(capPaymentResultModel.capID.customID))
                    {
                        capIDArray.Append(ConstructCapID(capPaymentResultModel.capID)).Append(",");
                    }
                }
            }

            retureCapIDArray = capIDArray.ToString();

            if (retureCapIDArray.IndexOf(",") > 0)
            {
                retureCapIDArray = retureCapIDArray.Substring(0, retureCapIDArray.Length - 1);
            }

            return retureCapIDArray;
        }

        /// <summary>
        /// Constructs the cap id like "FLAGSTAFF-09CAP-00000-00032"
        /// </summary>
        /// <param name="capID">it is CAP ID.</param>
        /// <returns>a CAP ID string.</returns>
        private string ConstructCapID(CapIDModel capID)
        {
            string customID = string.Empty;

            if (capID != null && !string.IsNullOrEmpty(capID.ID1))
            {
                customID = string.Format("{0}{1}{2}{3}{4}{5}{6}", capID.serviceProviderCode, ACAConstant.SPLIT_CHAR4, capID.ID1, ACAConstant.SPLIT_CHAR4, capID.ID2, ACAConstant.SPLIT_CHAR4, capID.ID3);
            }

            return customID;
        }

        /// <summary>
        /// Get Successful payment result model.
        /// </summary>
        /// <param name="capPaymentResultModels">payment result module.</param>
        /// <returns>payment result model list.</returns>
        private CapPaymentResultModel[] GetSuccessFullCapPaymentResult(CapPaymentResultModel[] capPaymentResultModels)
        {
            if (capPaymentResultModels == null || capPaymentResultModels.Length < 1)
            {
                return null;
            }

            ArrayList capPaymentArray = new ArrayList();

            foreach (CapPaymentResultModel capPaymentResultModel in capPaymentResultModels)
            {
                if (capPaymentResultModel.paymentStatus)
                {
                    capPaymentArray.Add(capPaymentResultModel);
                }
            }

            return (CapPaymentResultModel[])capPaymentArray.ToArray(typeof(CapPaymentResultModel));
        }

        /// <summary>
        /// if one successful cap has fee, return true.
        /// </summary>
        /// <returns>true or false.</returns>
        private bool IsOneCapHasFee()
        {
            CapPaymentResultModel[] capPaymentResultModels = GetCapPaymentResultModelFromSession();
            capPaymentResultModels = GetSuccessFullCapPaymentResult(capPaymentResultModels);

            if (capPaymentResultModels == null)
            {
                return false;
            }

            foreach (CapPaymentResultModel capPaymentResultModel in capPaymentResultModels)
            {
                if (capPaymentResultModel.hasFee)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Set SimpleViewElements model.
        /// </summary>
        private void GetGviewElementByModules()
        {
            ArrayList moduleList = new ArrayList();
            CapPaymentResultModel[] capPaymentResultModels = GetCapPaymentResultModelFromSession();
            IAdminBll admin = (IAdminBll)ObjectFactory.GetObject(typeof(IAdminBll));

            if (capPaymentResultModels != null && capPaymentResultModels.Length > 0)
            {
                foreach (CapPaymentResultModel model in capPaymentResultModels)
                {
                    if (model == null)
                    {
                        continue;
                    }

                    if (!moduleList.Contains(model.moduleName))
                    {
                        moduleList.Add(model.moduleName);
                    }
                }
            }

            simpleViewElements = ShoppingCartUtil.GetGviewElementByModules(moduleList);
        }

        /// <summary>
        /// payment status.
        /// </summary>
        /// <returns>the payment status.</returns>
        private int SuccessStatus()
        {
            CapPaymentResultModel[] capPaymentResultModel = GetCapPaymentResultModelFromSession();

            if (capPaymentResultModel == null)
            {
                return PAYMENT_OTHERS;
            }

            int failedCount = 0;
            int allCount = capPaymentResultModel.Length;

            for (int i = 0; i < allCount; i++)
            {
                if (!capPaymentResultModel[i].paymentStatus)
                {
                    failedCount = failedCount + 1;
                }
            }

            if (failedCount == 0)
            {
                return PAYMENT_ALLSECCESSFULL;
            }
            else if (failedCount == allCount)
            {
                return PAYMENT_FAILED;
            }
            else
            {
                return PAYMENT_PARTSECCESSFULL;
            }
        }

        #endregion Methods
    }
}
