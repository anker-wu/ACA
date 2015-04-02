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
 *      $Id: CapDetail.aspx.cs 278675 2014-09-10 09:11:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;

using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Finance;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.BLL.Report;
using Accela.ACA.BLL.ShoppingCart;
using Accela.ACA.BLL.SocialMedia;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Examination;
using Accela.ACA.Web.SocialMedia;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using log4net;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// This class provide the ability to operation CAP detail. 
    /// </summary>
    public partial class CapDetail : BasePage
    {
        #region Fields

        /// <summary>
        /// the string "true" come from ajax
        /// </summary>
        private const string TRUE_FLAG = "true";

        /// <summary>
        /// the external link parameter for alt id.
        /// </summary>
        private const string EXTERNAL_LINK_PARAMETER_ALTID = "$$ALTID$$";

        /// <summary>
        /// Receipt number.
        /// </summary>
        private const string RECEIPT_NUMBER = "RECEIPT_NUMBER";

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(CapDetail));
        
        /// <summary>
        /// the agency code.
        /// </summary>
        private string _agencyCode = string.Empty;

        /// <summary>
        /// page indicator.
        /// </summary>
        private string _pageIndicator = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the shared comments.
        /// </summary>
        /// <value>The shared comments.</value>
        public string SharedComments
        {
            get
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                string value = LabelUtil.GetTextByKey(ACAConstant.ACA_SOCIALMEDIA_LABEL_COMMENTS_PATTERN, ModuleName);

                if (capModel != null)
                {
                    IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
                    string addressString = addressBuilderBll.BuildAddressByFormatType(
                        capModel.addressModel, null, AddressFormatType.SHORT_ADDRESS_NO_FORMAT);
                    value = value.Replace("$$ModuleName$$", ModuleName).Replace("$$capType$$", lblPermitType.Text).Replace(
                            "$$Status$$", capModel.capStatus).Replace("$$capID$$", lblPermitNumber.Text).Replace("$$Address$$", addressString)
                            .Replace("$$StatusDate$$", I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(capModel.capStatusDate));
                }

                return value;
            }
        }

        /// <summary>
        /// Gets the get data URL.
        /// </summary>
        /// <value>The get data URL.</value>
        public string DataUrl
        {
            get
            {
                CapIDModel4WS capIdModelWS = AppSession.GetCapModelFromSession(this.ModuleName).capID;
                string queryString = string.Format(
                    "?Module={0}&capID1={1}&capID2={2}&capID3={3}&{4}={5}",
                    this.ModuleName,
                    capIdModelWS.id1,
                    capIdModelWS.id2,
                    capIdModelWS.id3,
                    UrlConstant.AgencyCode,
                    capIdModelWS.serviceProviderCode);

                return SocialMediaUtil.TinyUrl(ConfigManager.Protocol + "://" + this.Request.Url.Host + this.Request.FilePath + queryString);
            }
        }

        /// <summary>
        /// Gets or sets page indicator.
        /// </summary>
        public string PageIndicator
        {
            get
            {
                return _pageIndicator;
            }

            set
            {
                _pageIndicator = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is right to left.
        /// </summary>
        protected bool IsRightToLeft { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether display receipt report
        /// </summary>
        protected bool DisplayReceiptReport
        {
            get
            {
                if (ViewState["DisplayReceiptReport"] == null)
                {
                    ViewState["DisplayReceiptReport"] = false;
                }

                return (bool)ViewState["DisplayReceiptReport"];
            }

            set
            {
                ViewState["DisplayReceiptReport"] = value;
            }
        }

        /// <summary>
        /// Gets or sets receipt number.
        /// </summary>
        protected string ReceiptNbr
        {
            get
            {
                if (ViewState[RECEIPT_NUMBER] == null)
                {
                    ViewState[RECEIPT_NUMBER] = ACAConstant.NONASSIGN_NUMBER;
                }

                return (string)ViewState[RECEIPT_NUMBER];
            }

            set
            {
                ViewState[RECEIPT_NUMBER] = value;
            }
        }

        /// <summary>
        /// Gets or sets report ID.
        /// </summary>
        protected string ReportID
        {
            get
            {
                object o = ViewState["CapDetailReportID"];
                return o != null ? (string)o : ACAConstant.NONASSIGN_NUMBER;
            }

            set
            {
                ViewState["CapDetailReportID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets report name.
        /// </summary>
        protected string ReportName
        {
            get
            {
                object o = ViewState["CapDetailReportName"];
                return o != null ? (string)o : null;
            }

            set
            {
                ViewState["CapDetailReportName"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the renewal label key. use to restore button label key when post back
        /// </summary>
        /// <value>
        /// The BTN renewal label key.
        /// </value>
        private string RenewalBtnLabelKey
        {
            get
            {
                return ViewState["RenewalBtnLabelKey"] == null ? string.Empty : ViewState["RenewalBtnLabelKey"].ToString();
            }

            set
            {
                ViewState["RenewalBtnLabelKey"] = value;
            }
        }

        /// <summary>
        /// Gets or sets stores permit report ID.
        /// </summary>
        private string PermitReportID
        {
            get
            {
                if (ViewState[ACAConstant.PRINT_PERMIT_REPORT] == null)
                {
                    ViewState[ACAConstant.PRINT_PERMIT_REPORT] = ACAConstant.NONASSIGN_NUMBER;
                }

                return (string)ViewState[ACAConstant.PRINT_PERMIT_REPORT];
            }

            set
            {
                ViewState[ACAConstant.PRINT_PERMIT_REPORT] = value;
            }
        }

        /// <summary>
        /// Gets or sets stores summary report ID.
        /// </summary>
        private string SummaryReportID
        {
            get
            {
                if (ViewState[ACAConstant.PRINT_PERMIT_SUMMARY_REPORT] == null)
                {
                    ViewState[ACAConstant.PRINT_PERMIT_SUMMARY_REPORT] = ACAConstant.NONASSIGN_NUMBER;
                }

                return (string)ViewState[ACAConstant.PRINT_PERMIT_SUMMARY_REPORT];
            }

            set
            {
                ViewState[ACAConstant.PRINT_PERMIT_SUMMARY_REPORT] = value;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Show fee details,no paid
        /// </summary>
        /// <param name="pageNum">page number.</param>
        /// <param name="moduleName">the module name.</param>
        /// <returns> fee List Html</returns>
        [WebMethod(Description = "DisplayFeeNoPaid", EnableSession = true)]
        public static string DisplayFeeNoPaid(string pageNum, string moduleName)
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
            bool readOnly = capModel.IsForRenew;

            FeeListData feeListData = new FeeListData(capModel.capID);
            feeListData.ModuleName = moduleName;
            feeListData.LoadNopaidFeeItem();

            BuildFeeListTable buildFeeHtml = new BuildFeeListTable(feeListData.NoPaidDataSource, feeListData.HeadColumns);
            buildFeeHtml.Paid = false;
            buildFeeHtml.TotalFees = feeListData.NoPaidTotalAccount;
            buildFeeHtml.CurrentPageIndex = Convert.ToInt32(pageNum);
            buildFeeHtml.ModuleName = moduleName;
            string feeListHtml = buildFeeHtml.GetBuildTable(readOnly);
            return feeListHtml;
        }

        /// <summary>
        /// Show fee details,paid
        /// </summary>
        /// <param name="pageNum">page number.</param>
        /// <param name="moduleName">the module name.</param>
        /// <param name="reportName">report name</param>
        /// <param name="receiptNbr">string for report number.</param>
        /// <param name="reportID">string for report id.</param>
        /// <param name="displayReceiptReport">display receipt report</param>
        /// <returns>fee List Html</returns>
        [WebMethod(Description = "DisplayFeePaid", EnableSession = true)]
        public static string DisplayFeePaid(string pageNum, string moduleName, string reportName, string receiptNbr, string reportID, bool displayReceiptReport)
        {
            FeeListData feeListData = new FeeListData(AppSession.GetCapModelFromSession(moduleName).capID);
            feeListData.ModuleName = moduleName;
            feeListData.ReportName = reportName;
            feeListData.LoadPaidFeeItem();

            BuildFeeListTable buildFeeHtml = new BuildFeeListTable(feeListData.PaidDataSource, feeListData.HeadColumns);
            buildFeeHtml.Paid = true;
            buildFeeHtml.DisplayReceiptReport = displayReceiptReport;
            buildFeeHtml.TotalFees = feeListData.PaidTotalAccount;
            buildFeeHtml.CurrentPageIndex = Convert.ToInt32(pageNum);
            buildFeeHtml.ModuleName = moduleName;
            buildFeeHtml.ReceiptNbr = receiptNbr;
            buildFeeHtml.ReceiptReportID = reportID;
            string feeListHtml = buildFeeHtml.GetBuildTable(false);
            return feeListHtml;
        }

        /// <summary>
        /// Get CAP Tree Data
        /// </summary>
        /// <param name="moduleName">the module name.</param>
        /// <param name="isShowAll">a value indicating whether display completed tree or not.</param>
        /// <returns>cap tree html</returns>
        [WebMethod(Description = "GetBuildCapTree", EnableSession = true)]
        public static string GetBuildCapTree(string moduleName, bool isShowAll)
        {
            string capTreeHtml = moduleName;

            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
            CapIDModel4WS capIdModel = capModel.capID;
            bool readOnly = capModel != null ? capModel.IsForRenew : false;

            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
            ProjectTreeNodeModel4WS capListTree = null;

            if (isShowAll)
            {
                capListTree = capBll.GetRelatedCapTree(capIdModel, null, null);
            }
            else
            {
                CapIDModel capId = TempModelConvert.Trim4WSOfCapIDModel(capIdModel);
                QueryFormat queryFormat = new QueryFormat();
                capListTree = capBll.GetSectionalRelatedCapTree(capId, queryFormat);
            }

            if (!AppSession.IsAdmin || (capListTree != null && capListTree.children != null))
            {
                RelatedCapTreeData permitList = new RelatedCapTreeData();
                capTreeHtml = permitList.GetBuildHtml(capListTree, capIdModel.id1 + capIdModel.id2 + capIdModel.id3, moduleName, false, readOnly);
            }
            else
            {
                capTreeHtml = string.Empty;
            }

            return capTreeHtml;
        }

        /// <summary>
        /// Get Processing Data
        /// </summary>
        /// <param name="agencyCode">Agency code.</param>
        /// <param name="moduleName">the module name.</param>
        /// <returns>processing Content</returns>
        [WebMethod(Description = "GetProcessingData", EnableSession = true)]
        public static string GetProcessingData(string agencyCode, string moduleName)
        {
            WorkStatus workStatus = new WorkStatus(agencyCode, moduleName);
            string processingContent = workStatus.GetProcessingContent();
            return processingContent;
        }

        /// <summary>
        /// Gets the SSO link.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="providerNbr">The provider NBR.</param>
        /// <param name="userExamId">The user exam id.</param>
        /// <returns>Return the SSO link.</returns>
        [WebMethod(Description = "GetSSOLink", EnableSession = true)]
        public static string GetSSOLink(string agencyCode, string providerNbr, string userExamId)
        {
            try
            {
                IExaminationBll examinationBll = (IExaminationBll)ObjectFactory.GetObject(typeof(IExaminationBll));

                var providerPkModel = new ProviderPKModel()
                {
                    serviceProviderCode = agencyCode,
                    providerNbr = long.Parse(providerNbr)
                };

                return examinationBll.GetSSOLink(userExamId, providerPkModel);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return string.Empty;
            }
        }

        /// <summary>
        /// Check the examination has work flow restriction or not
        /// </summary>
        /// <param name="examNumber">The examine number</param>
        /// <param name="moduleName">The module name</param>
        /// <param name="status">The status</param>
        /// <param name="action">The action.</param>
        /// <param name="pageUrl">The page url</param>
        /// <param name="objectTargetID">The object's target ID</param>
        /// <returns>true or false</returns>
        [WebMethod(Description = "HasWrokflowRestricted", EnableSession = true)]
        public static ArrayList HasWrokflowRestricted(long? examNumber, string moduleName, string status, string action, string pageUrl, string objectTargetID)
        {
            ArrayList array = new ArrayList();

            IExaminationBll examinationBLL = (IExaminationBll)ObjectFactory.GetObject(typeof(IExaminationBll));
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
            ExaminationModel exam = capModel.examinationList.First(p => p.examinationPKModel.examNbr == examNumber);
            bool isWrokflowRestricted = false;

            if (ACAConstant.EXAMINATION_STATUS_PENDING.Equals(status, StringComparison.InvariantCultureIgnoreCase)
                || ACAConstant.EXAMINATION_STATUS_SCHEDULE.Equals(status, StringComparison.InvariantCultureIgnoreCase))
            {
                isWrokflowRestricted = examinationBLL.IsWrokflowRestricted(exam, capModel, "DETAIL", AppSession.User.UserID);
            }

            array.Add(isWrokflowRestricted.ToString().ToLower());
            array.Add(pageUrl);
            array.Add(objectTargetID);

            if (ACAConstant.EXAMINATION_STATUS_PENDING.Equals(status, StringComparison.InvariantCultureIgnoreCase)
                && ExaminationAction.Edit.ToString().Equals(action, StringComparison.InvariantCultureIgnoreCase))
            {
                array.Add(GetStaticTextByKey("examination_edit_pending_restrict"));
            }
            else
            {
                array.Add(string.Format(GetStaticTextByKey("examination_edit_add_restrict"), exam.examName));
            }

            return array;
        }

        /// <summary>
        /// Delete examination from daily
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <param name="capId1">The cap id1</param>
        /// <param name="capId2">The cap id2</param>
        /// <param name="capId3">The cap id3</param>
        /// <param name="examNbr">The examine number</param>
        /// <returns>true or false</returns>
        [WebMethod(Description = "DeleteExamination", EnableSession = true)]
        public static bool DeleteExamination(string agencyCode, string capId1, string capId2, string capId3, string examNbr)
        {
            IExaminationBll examinationBll = (IExaminationBll)ObjectFactory.GetObject(typeof(IExaminationBll));

            ExaminationModel examinationModel = new ExaminationModel();
            examinationModel.b1PerId1 = capId1;
            examinationModel.b1PerId2 = capId2;
            examinationModel.b1PerId3 = capId3;
            examinationModel.examinationPKModel = new ExaminationPKModel();
            examinationModel.examinationPKModel.examNbr = long.Parse(examNbr);
            examinationModel.examinationPKModel.serviceProviderCode = agencyCode;
            examinationModel.auditModel = new SimpleAuditModel { auditID = AppSession.User.PublicUserId };
            return examinationBll.DeleteExam(examinationModel);
        }

        /// <summary>
        /// Post to the social media.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="actionSource">The action source.</param>
        /// <param name="actionType">Type of the action.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>Indicating post success or not.</returns>
        [WebMethod(Description = "Post2SocialMedia", EnableSession = true)]
        public static bool Post2SocialMedia(string moduleName, string actionSource, string actionType, string entityType)
        {
            CapModel4WS cap = AppSession.GetCapModelFromSession(moduleName);
            XSocialMediaEntityModel socialMediaEntityModel = new XSocialMediaEntityModel()
                {
                    actionSource = actionSource,
                    actionType = actionType,
                    auditModel = new SimpleAuditModel(),
                    entityAgency = cap.capID.serviceProviderCode,
                    entityID = cap.capID.id1 + "-" + cap.capID.id2 + "-" + cap.capID.id3,
                    entityType = entityType,
                    publicUserSeq = Convert.ToInt32(AppSession.User.UserSeqNum),
                    serviceProviderCode = ConfigManager.AgencyCode
                };

            ISocialMediaBll business = ObjectFactory.GetObject<ISocialMediaBll>();
            business.SaveSocialMediaEntity(socialMediaEntityModel);

            return true;
        }

        /// <summary>
        /// Resets the ready to schedule examination.
        /// </summary>
        /// <param name="dailyExamNumber">The daily exam number.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>The error message if throw exception.</returns>
        [WebMethod(Description = "ResetReady2ScheduleExamiation", EnableSession = true)]
        public static string ResetReady2ScheduleExamiation(string dailyExamNumber, string moduleName)
        {
            string result = string.Empty;

            try
            {
                CapModel4WS cap = AppSession.GetCapModelFromSession(moduleName);

                if (cap.examinationList != null && cap.examinationList.Length > 0)
                {
                    ExaminationModel examinationModel =
                        cap.examinationList.FirstOrDefault(
                            o => o.examinationPKModel.examNbr == Convert.ToInt64(dailyExamNumber));

                    if (examinationModel != null)
                    {
                        IExaminationBll examinationBll = ObjectFactory.GetObject<IExaminationBll>();
                        examinationBll.ResetReady2ScheduleExam(examinationModel.examinationPKModel, TempModelConvert.Trim4WSOfCapIDModel(cap.capID), AppSession.User.PublicUserId);
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Show fee details,no paid
        /// </summary>
        /// <param name="moduleName">the module name.</param>
        /// <returns> fee List Html</returns>
        [WebMethod(Description = "AddToShoppingCart", EnableSession = true)]
        public static string AddToShoppingCart(string moduleName)
        {
            string script = string.Empty;
            DataTable dt = ConstructDataTable(moduleName);
            ShoppingCartItemModel4WS[] cartItems = ShoppingCartUtil.FilterAndConstructCartItems(dt);
            IShoppingCartBll shoppingCartBll = (IShoppingCartBll)ObjectFactory.GetObject(typeof(IShoppingCartBll));

            if (cartItems == null || cartItems.Length == 0)
            {
                script = string.Format("displayCartAlertMessage('{0}')", LabelUtil.GetTextByKey("per_caplist2cart_message_theonefailed", moduleName).Replace("'", "\\'"));
            }
            else
            {
                shoppingCartBll.CreateShoppingCart(cartItems, false, false);
                string cartNumberHtml = ShoppingCartUtil.GetCartNumberHTML(moduleName);
                script = string.Format("displayCartMessage('{0}','{1}','{2}')", LabelUtil.GetTextByKey("per_caplist2cart_message_successfull", moduleName).Replace("'", "\\'"), true, cartNumberHtml);
            }

            return script;
        }

        /// <summary>
        /// Load document status list.
        /// </summary>
        /// <param name="clientId">The control client id.</param>
        /// <param name="moduleName">module name.</param>
        /// <returns>document status list</returns>
        [WebMethod(Description = "load document status list", EnableSession = true)]
        public static string LoadDocStatuses(string clientId, string moduleName)
        {
            return Component.DocumentStatusList.LoadDocStatuses(clientId, moduleName);
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Gets clone url used by UI page.
        /// </summary>
        /// <returns>Return clone url used by UI page.</returns>
        protected string GetClonePageUrl()
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            string cloneUrl = CloneRecordUtil.BuildClonePageUrl(TempModelConvert.Trim4WSOfCapIDModel(capModel.capID), ModuleName);
            return cloneUrl;
        }

        /// <summary>
        /// override method ChangeMasterPage
        /// </summary>
        protected override void ChangeMasterPage()
        {
            if (ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FOR_RENEW]))
            {
                MasterPageFile = FileUtil.AppendApplicationRoot("Dialog.master");
                return;
            }

            base.ChangeMasterPage();
        }

        /// <summary>
        /// override method RecordUrl
        /// </summary>
        protected override void RecordUrl()
        {
            if (ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FOR_RENEW]))
            {
                return;
            }

            base.RecordUrl();
        }

        /// <summary>
        /// On Initial event method
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                //Clear UI data before initialize sections.
                UIModelUtil.ClearUIData();
            }

            base.OnInit(e);
            if (StandardChoiceUtil.IsShowMap4ShowObject(this.ModuleName))
            {
                mapCapDetail.Visible = true;
            }
            else
            {
                mapCapDetail.Visible = false;
            }

            switchTreeViewBtnContainerForAdmin.Visible = AppSession.IsAdmin;

            DialogUtil.RegisterScriptForDialog(this.Page);
        }

        /// <summary>
        /// Show CapModel on Map
        /// </summary>
        /// <param name="sender">Map component</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void CapDetailMap_ShowACAMap(object sender, EventArgs e)
        {
            CapIDModel capIdModel = new CapIDModel();
            CapIDModel4WS capIdModelWS = AppSession.GetCapModelFromSession(ModuleName).capID;
            capIdModel.ID1 = capIdModelWS.id1;
            capIdModel.ID2 = capIdModelWS.id2;
            capIdModel.ID3 = capIdModelWS.id3;
            capIdModel.serviceProviderCode = capIdModelWS.serviceProviderCode;

            List<CapIDModel> capIds = new List<CapIDModel>();
            capIds.Add(capIdModel);
            ACAGISModel gisModel = GISUtil.CreateACAGISModel();
            gisModel.CapIDModels = capIds.ToArray();
            gisModel.Context = mapCapDetail.AGISContext;
            gisModel.IsMiniMap = mapCapDetail.IsMiniMode;
            gisModel.Windowless = true;
            gisModel.Agency = capIdModel.serviceProviderCode;
            if (AppSession.User.IsAnonymous)
            {
                gisModel.UserGroups.Add(GISUserGroup.Anonymous.ToString());
            }
            else
            {
                gisModel.UserGroups.Add(GISUserGroup.Register.ToString());
            }

            GISUtil.SetPostUrl(this, gisModel);
            gisModel.ModuleName = ModuleName;
            mapCapDetail.ACAGISModel = gisModel;
        }

        /// <summary>
        /// Handles the Initial event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            InitInspectionListControl();
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //get agency from url parameter
            _agencyCode = GetAgencyCode();

            if (!IsPostBack)
            {
                bool isForRenew = ValidationUtil.IsYes(HttpContext.Current.Request.QueryString[UrlConstant.IS_FOR_RENEW]);

                if (AppSession.IsAdmin)
                {
                    lblAddCap2Collection.Visible = true;
                }
                else if (isForRenew || (AppSession.User != null && AppSession.User.IsAnonymous))
                {
                    lblAddCap2Collection.Visible = false;
                }

                if (isForRenew)
                {
                    dvContent.Attributes["class"] = "ACA_RightItem_Popup";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "HideLoading4Parent", "parent.HideLoading(true);", true);
                }

                IsRightToLeft = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft;

                ClearSession();
                PageIndicator = BasePage.GetStaticTextByKey("ACA_AccelaGridView_PageIndicator");

                // get the cap model
                CapIDModel4WS capIdModel = new CapIDModel4WS();

                if (Request.QueryString["capID1"] == null)
                {
                    capIdModel = AppSession.GetCapModelFromSession(ModuleName).capID;
                }
                else
                {
                    capIdModel.serviceProviderCode = _agencyCode;
                    capIdModel.id1 = UpperQueryString("capID1");
                    capIdModel.id2 = UpperQueryString("capID2");
                    capIdModel.id3 = UpperQueryString("capID3");
                }

                CapModel4WS capModel = new CapModel4WS();
                CapWithConditionModel4WS capWithConditionModel = CapUtil.GetCapWithConditionModel4WS(capIdModel, AppSession.User.UserSeqNum, true, isForRenew, null);

                // When return null, it means that it can not find CAP or current user can not read this CAP
                if (capWithConditionModel == null || capWithConditionModel.capModel == null
                    || ACAConstant.PAYMENT_STATUS_PAID.Equals(capWithConditionModel.capModel.paymentStatus, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (AppSession.User.IsAnonymous)
                    {
                        GotoLoginPage();
                    }
                    else
                    {
                        throw new InvalidOperationException(GetTextByKey("per_permitList_error_access"));
                    }
                }

                capModel = capWithConditionModel.capModel;

                if (capModel != null)
                {
                    capModel.IsForRenew = isForRenew;
                }

                // Fill template information
                FillCapModelTemplateValue(capModel);

                AppSession.SetCapModelToSession(ModuleName, capModel);

                // display/hide sections
                DisplaySections(capWithConditionModel, !isForRenew);

                BindReportButtonUrl(capModel);

                InspectionList.Visible = divInspection.Visible;

                if (divInspection.Visible)
                {
                    InspectionList.InitList(capModel);
                }

                if (AppSession.IsAdmin)
                {
                    lnkPrintPermit.Visible = true;
                    lnkPrintReceipt.Visible = true;
                    lnkPrintSummary.Visible = true;
                    lnkCloneRecord.Visible = true;
                }
            }
            else
            {
                if (this.btnRenewal.Visible)
                {
                    this.btnRenewal.LabelKey = RenewalBtnLabelKey;
                }
                
                SetSelectedCAPsToAddingControl();
            }

            if (!AppSession.IsAdmin && StandardChoiceUtil.IsShowMap4ShowObject(this.ModuleName))
            {
                if (!IsPostBack)
                {
                    mapCapDetail.ShowMap();

                    if (!string.IsNullOrEmpty(Request["Command"]) && string.Equals(Request["Command"], ACAConstant.AGIS_COMMAND_SCHEDULE_INSPECTION)
                        && divInspection.Visible)
                    {
                        string gotoInspectionScript = string.Format("document.getElementById('{0}').scrollIntoView();", shInspection.ClientID);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "GotoInspection", gotoInspectionScript, true);
                    }
                }
            }
            else
            {
                mapCapDetail.Visible = false;
            }

            LoadDialogCss();

            if (EnableSocialMediaButton())
            {
                SocialMediaUtil.SetOpenGraphInfo(lblPermitNumber.Text, SharedComments, DataUrl, this.Header, Request);
            }
        }

        /// <summary>
        /// Examinations the list_ refresh cap contact.
        /// </summary>
        protected void ExaminationList_RefreshCapContact()
        {
            PermitDetailList1.RefreshControl();
        }

        /// <summary>
        /// CreateAmendment Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void CreateAmendmentButton_Click(object sender, EventArgs e)
        {
            CapModel4WS capModel4MS = AppSession.GetCapModelFromSession(ModuleName);

            string url = CapUtil.CreateUrlForAmendment(capModel4MS);
            string agencyParam = "&" + UrlConstant.AgencyCode + "=" + capModel4MS.capID.serviceProviderCode;
            url += agencyParam;
            Response.Redirect(url);
        }

        /// <summary>
        /// Handle the RowCommand event for contact address list.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void BtnRenewalAndRequestTradeLicenseClick(object sender, EventArgs e)
        {
            AccelaButton actionButton = sender as AccelaButton;

            if (actionButton != null)
            {
                string url = CapUtil.GetActionCommandUrl(Page, actionButton.CommandArgument);

                if (string.IsNullOrEmpty(url))
                {
                    return;
                }

                if (url.StartsWith("~/", StringComparison.InvariantCultureIgnoreCase))
                {
                    url = ResolveUrl(url);
                }

                Response.Redirect(url);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Get Receipt Number
        /// </summary>
        /// <param name="capIdModel">capId Model</param>
        /// <returns>Receipt number</returns>
        private static string GetReceiptNbr(CapIDModel4WS capIdModel)
        {
            string receiptNbr = string.Empty;

            try
            {
                IOnlinePaymenBll paymentBll = (IOnlinePaymenBll)ObjectFactory.GetObject(typeof(IOnlinePaymenBll));
                receiptNbr = paymentBll.GetReceiptNoByCapID(capIdModel, null, AppSession.User.PublicUserId);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            return receiptNbr;
        }

        /// <summary>
        /// Construct DataTable using capModel
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>The data table</returns>
        private static DataTable ConstructDataTable(string moduleName)
        {
            var capBll = ObjectFactory.GetObject<ICapBll>();
            var cap = AppSession.GetCapModelFromSession(moduleName);
            DataTable result = capBll.BuildPermitDataTable(new[] { cap });

            return result;
        }

        /// <summary>
        ///  Gets a value indicating whether need to display the Optional inspection.
        ///  True - Display all inspection, needn't to filter by optional status.
        ///  No  -  Need to filter the inspections with 'Optional' status.The optional status inspection shouldn't be displayed to list.
        /// </summary>
        /// <param name="sectionPermissions">section Permissions</param>
        /// <returns>true or false</returns>
        private bool IsDisplayInspections(Dictionary<string, UserRolePrivilegeModel> sectionPermissions)
        {
            bool isInspectionSectionVisible = CapUtil.GetSectionVisibility(CapDetailSectionType.INSPECTIONS.ToString(), sectionPermissions, this.ModuleName);
            return isInspectionSectionVisible;
        }

        /// <summary>
        /// Load Dialog CSS
        /// </summary>
        private void LoadDialogCss()
        {
            string hideHeader = Request.QueryString["HideHeader"];
            if (string.Equals(hideHeader, TRUE_FLAG, StringComparison.CurrentCultureIgnoreCase))
            {
                string className = dvContent.Attributes["class"];
                className += " ACA_Dialog_Content";
                dvContent.Attributes.Add("class", className);
            }
        }

        /// <summary>
        /// Initial the inspection list control.
        /// </summary>
        private void InitInspectionListControl()
        {
            InspectionList.CurrentUser = AppSession.User;
        }

        /// <summary>
        /// Displays or hides the sections according to the specific settings in ACA admin
        /// </summary>
        /// <param name="capWithConditionModel">The CapWithConditionModel4WS.</param>
        /// <param name="isEditable">section is editable</param>
        private void DisplaySections(CapWithConditionModel4WS capWithConditionModel, bool isEditable)
        {
            CapModel4WS capModel = capWithConditionModel.capModel;

            if (!AppSession.IsAdmin)
            {
                if (capModel != null && !string.IsNullOrEmpty(capModel.capStatus))
                {
                    lblRecordStatus.Text = capModel.capStatus;
                    divRecordStatus.Visible = true;
                }

                if (capModel != null && capModel.b1ExpirationModel != null && !string.IsNullOrEmpty(capModel.b1ExpirationModel.expDateString))
                {
                    lblExpirtionDate.Text = I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(capModel.b1ExpirationModel.expDateString);
                    divRecordExpirationDate.Visible = true;
                }

                lblPermitNumber.Text = string.IsNullOrEmpty(capModel.altID) ? string.Empty : capModel.altID;
                lblPermitType.Text = CAPHelper.GetAliasOrCapTypeLabel(capModel);
                lnkLink4MoreInfo.Visible = false;

                if (capModel.capDetailModel != null && !string.IsNullOrEmpty(capModel.capDetailModel.url))
                {
                    lnkLink4MoreInfo.HRef = Regex.Replace(capModel.capDetailModel.url, EXTERNAL_LINK_PARAMETER_ALTID.Replace("$", "\\$"), capModel.altID, RegexOptions.IgnoreCase);
                    lnkLink4MoreInfo.Target = "_blank";
                    lnkLink4MoreInfo.Visible = true;
                }
            }
            else
            {
                divRecordStatus.Visible = true;
                divRecordExpirationDate.Visible = true;

                lnkLink4MoreInfo.Visible = true;
            }

            // Get section permissions
            Dictionary<string, UserRolePrivilegeModel> sectionPermissions = CapUtil.GetSectionPermissions(_agencyCode, ModuleName);

            // display cap condition
            DisplayCapCondition(capWithConditionModel);

            // display work location
            DisplayWorkLocation(sectionPermissions);

            // display cap detail info
            DisplayCapDetail(capModel, sectionPermissions, isEditable);

            // fee section
            DisplayFeeSection(sectionPermissions);

            //display inspection list
            DisplayInspections(sectionPermissions, isEditable);

            // workflow section
            DisplayWorkflowSection(sectionPermissions);

            // attachment section
            DisplayAttachmentsSection(capModel, sectionPermissions, isEditable);

            // related records section
            DisplayRelatedRecordsSection(sectionPermissions);

            // judge whether display education list.            
            DisplayEducations(capModel, sectionPermissions);

            // judge whether display continuing education list.
            DisplayContEducations(capModel, sectionPermissions);

            //judge whether display examination list
            DisplayExamination(capModel, sectionPermissions, isEditable);

            // judge whether display valuation calculators list.           
            DisplayValuationCalculator(capModel, sectionPermissions);

            // judge whether display trust account list.           
            DisplayTrustAccount(capModel, sectionPermissions, isEditable);

            // judge whether display assets list. 
            DisplayAssets(capModel, sectionPermissions);
        }

        /// <summary>
        /// Determines whether the related records section is visible for the current user 
        /// according to the section permissions
        /// </summary>
        /// <param name="sectionPermissions">the section permissions storing section permissions</param>
        private void DisplayRelatedRecordsSection(Dictionary<string, UserRolePrivilegeModel> sectionPermissions)
        {
            bool isRelatedRecordsVisible = CapUtil.GetSectionVisibility(CapDetailSectionType.RELATED_RECORDS.ToString(), sectionPermissions, this.ModuleName);
            if (isRelatedRecordsVisible || AppSession.IsAdmin)
            {
                this.divRelatedPermits.Visible = true;
                RegisterExpandScript(shRelatedPermit, "per_permitDetail_label_relatedPermit", "ExpandRelatedPermit", "ExpandRelatedPermitSection(false);");
            }
            else
            {
                this.divRelatedPermits.Visible = false;
            }
        }

        /// <summary>
        /// Determines whether the workflow section is visible for the current user 
        /// according to the section permissions
        /// </summary>
        /// <param name="sectionPermissions">the section permissions storing section permissions</param>
        private void DisplayWorkflowSection(Dictionary<string, UserRolePrivilegeModel> sectionPermissions)
        {
            bool isWorkflowVisible = CapUtil.GetSectionVisibility(CapDetailSectionType.PROCESSING_STATUS.ToString(), sectionPermissions, this.ModuleName);
            if (isWorkflowVisible || AppSession.IsAdmin)
            {
                this.divWorkflow.Visible = true;
                RegisterExpandScript(shProcessStatus, "per_workStatus_label_proceeStatus", "ExpandWorkflow", "ExpandWorkflowSection();");
            }
            else
            {
                this.divWorkflow.Visible = false;
            }
        }

        /// <summary>
        /// Register expand section title script.
        /// </summary>
        /// <param name="sh">the section header.</param>
        /// <param name="labelKey">the label key.</param>
        /// <param name="scriptKey">the script key.</param>
        /// <param name="script">the script.</param>
        private void RegisterExpandScript(Component.SectionHeader sh, string labelKey,  string scriptKey, string script)
        {
            if (!AppSession.IsAdmin)
            {
                bool isExpanded = StandardChoiceUtil.IsEnableForData4AsKey(XPolicyConstant.IS_EXPANDED_SECTION, ModuleName, labelKey);
                sh.Collapsed = !isExpanded;

                if (isExpanded && !string.IsNullOrEmpty(scriptKey))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), scriptKey, script, true);
                }
            }
        }

        /// <summary>
        /// Determines whether the fee section is visible for the current user 
        /// according to the section permissions
        /// </summary>
        /// <param name="sectionPermissions">the section permissions storing section permissions</param>
        private void DisplayFeeSection(Dictionary<string, UserRolePrivilegeModel> sectionPermissions)
        {
            bool isFeeVisible = CapUtil.GetSectionVisibility(CapDetailSectionType.FEE.ToString(), sectionPermissions, this.ModuleName);
            if (isFeeVisible || AppSession.IsAdmin)
            {
                this.divFee.Visible = true;
                RegisterExpandScript(shFee, "per_feeDetails_label_feeTitel", "ExpandFee", "ExpandFeeSection();");
            }
            else
            {
                this.divFee.Visible = false;
            }
        }

        /// <summary>
        /// Bind Report Button Url
        /// </summary>
        /// <param name="capModel">a CapModel4WS</param>
        private void BindReportButtonUrl(CapModel4WS capModel)
        {
            //Permit Report Button
            string permitReportUrl = string.Empty;
            string summaryReportUrl = string.Empty;
            CapIDModel4WS capID = capModel.capID;

            if (StandardChoiceUtil.IsSuperAgency() && capID != null && !ConfigManager.AgencyCode.Equals(capID.serviceProviderCode, StringComparison.InvariantCulture))
            {
                permitReportUrl = string.Format(
                    ACAConstant.REPORT_PARAMETER_PAGE + "?Module={0}&reportType={1}&reportID={2}&subID1={3}&subID2={4}&subID3={5}&{6}={7}&subCustomerID={8}&SubModule={9}",
                    ScriptFilter.AntiXssUrlEncode(ModuleName),
                    ACAConstant.PRINT_PERMIT_REPORT,
                    PermitReportID,
                    Server.UrlEncode(capID.id1),
                    Server.UrlEncode(capID.id2),
                    Server.UrlEncode(capID.id3),
                    UrlConstant.AgencyCode,
                    capID.serviceProviderCode,
                    Server.UrlEncode(capID.customID),
                    capModel.moduleName);

                summaryReportUrl = string.Format(
                    ACAConstant.REPORT_PARAMETER_PAGE + "?Module={0}&reportType={1}&reportID={2}&subID1={3}&subID2={4}&subID3={5}&{6}={7}&subCustomerID={8}&SubModule={9}",
                    ScriptFilter.AntiXssUrlEncode(ModuleName),
                    ACAConstant.PRINT_PERMIT_SUMMARY_REPORT,
                    SummaryReportID,
                    Server.UrlEncode(capID.id1),
                    Server.UrlEncode(capID.id2),
                    Server.UrlEncode(capID.id3),
                    UrlConstant.AgencyCode,
                    capID.serviceProviderCode,
                    Server.UrlEncode(capID.customID),
                    capModel.moduleName);
            }
            else
            {
                permitReportUrl = string.Format(ACAConstant.REPORT_PARAMETER_PAGE + "?Module={0}&{1}={2}&{3}={4}&{5}={6}", ScriptFilter.AntiXssUrlEncode(ModuleName), "reportType", ACAConstant.PRINT_PERMIT_REPORT, "reportID", this.PermitReportID, UrlConstant.AgencyCode, ConfigManager.AgencyCode);

                //Summary Report Button
                summaryReportUrl = string.Format(ACAConstant.REPORT_PARAMETER_PAGE + "?Module={0}&{1}={2}&{3}={4}&{5}={6}", ScriptFilter.AntiXssUrlEncode(ModuleName), "reportType", ACAConstant.PRINT_PERMIT_SUMMARY_REPORT, "reportID", this.SummaryReportID, UrlConstant.AgencyCode, ConfigManager.AgencyCode);
            }

            if (!AppSession.IsAdmin)
            {
                this.lnkPrintPermit.Attributes.Add("onclick", "print_onclick('" + permitReportUrl + "');return false;");

                this.lnkPrintSummary.Attributes.Add("onclick", "print_onclick('" + summaryReportUrl + "');return false;");
            }
        }

        /// <summary>
        /// Clear Session
        /// </summary>
        private void ClearSession()
        {
            HttpContext.Current.Session.Remove("capWithConditionModel");
            HttpContext.Current.Session.Remove("edmsPolicyModel");
            HttpContext.Current.Session.Remove("licenseModel4WS");

            //for fee list in FeeListData.cs
            HttpContext.Current.Session.Remove("NoPaidDataSource");
            HttpContext.Current.Session.Remove("PaidDataSource");
            HttpContext.Current.Session.Remove("NoPaidTotalAccount");
            HttpContext.Current.Session.Remove("PaidTotalAccount");
        }

        /// <summary>
        /// Display Cap Detail
        /// </summary>
        /// <param name="capModel">The CAP model.</param>
        /// <param name="sectionPermissions">storing section permissions information</param>
        /// <param name="isEditable">is editable.</param>
        private void DisplayCapDetail(CapModel4WS capModel, Dictionary<string, UserRolePrivilegeModel> sectionPermissions, bool isEditable)
        {
            bool isCapDetailVisible = CapUtil.GetSectionVisibility(CapDetailSectionType.PROCESSING_STATUS.ToString(), sectionPermissions, this.ModuleName);

            if (isCapDetailVisible || AppSession.IsAdmin)
            {
                divPermitDetail.Visible = true;

                //RegisterExpandScript(shPermitDetail, "per_permitDetail_label_detail", "ExpandPermitDetail", "ExpandPermitDetailSection();");
                if (!AppSession.IsAdmin)
                {
                    ExpandOrCollapseSection(shPermitDetail, "per_permitDetail_label_detail");
                }
            }
            
            if (AppSession.IsAdmin)
            {
                divPermitDetail.Visible = true;
                PermitDetailList1.HiddenSections.Clear();
                return;
            }

            // the value need to replace section name with enum values
            List<string> sectionList = new List<string>();
            sectionList.AddRange(new[]
            { 
                CapDetailSectionType.APPLICANT.ToString(),
                CapDetailSectionType.LICENSED_PROFESSIONAL.ToString(),
                CapDetailSectionType.PROJECT_DESCRIPTION.ToString(),
                CapDetailSectionType.OWNER.ToString(),
                CapDetailSectionType.RELATED_CONTACTS.ToString(),
                CapDetailSectionType.ADDITIONAL_INFORMATION.ToString(),
                CapDetailSectionType.APPLICATION_INFORMATION.ToString(),
                CapDetailSectionType.APPLICATION_INFORMATION_TABLE.ToString(),
                CapDetailSectionType.PARCEL_INFORMATION.ToString()
            });

            // indicate whether exist visible permit detail section
            bool existVisibleSection = false;

            // display/hide permit detail section
            foreach (string sectionName in sectionList)
            {
                bool isVisible = CapUtil.GetSectionVisibility(sectionName, sectionPermissions, ModuleName);

                if (!isVisible)
                {
                    PermitDetailList1.HiddenSections.Add(sectionName);
                }

                existVisibleSection = existVisibleSection || isVisible;
            }

            DisplayButtons(capModel, isEditable);

            // set the container DIV visible
            divPermitDetail.Visible = existVisibleSection || ExistVisibleReportButtons();
        }

        /// <summary>
        /// Expand or collapse section.
        /// </summary>
        /// <param name="sh">the section header.</param>
        /// <param name="labelKey">the label key.</param>
        private void ExpandOrCollapseSection(Component.SectionHeader sh, string labelKey)
        {
            IXPolicyBll xpolicyBll = ObjectFactory.GetObject<IXPolicyBll>();
            string policyValue = xpolicyBll.GetPolicyValueForData4AsKey(
                                                                        XPolicyConstant.IS_EXPANDED_SECTION,
                                                                        ACAConstant.LEVEL_TYPE_MODULE,
                                                                        ModuleName,
                                                                        labelKey);
            bool isExpanded = !ValidationUtil.IsNo(policyValue);
            sh.Collapsed = !isExpanded;
        }

        /// <summary>
        /// Display/hide more detail buttons
        /// </summary>
        /// <param name="capModel">cap model</param>
        /// <param name="isEditable">is editable.</param>
        private void DisplayButtons(CapModel4WS capModel, bool isEditable)
        {
            DataTable dt = ConstructDataTable(capModel.moduleName);

            // Display/hide report buttons
            DisplayReportButton(capModel.capID);

            // Display/hide Amendment buttons
            bool isAmendment = IsAmendableCapAvailable(ModuleName);
            liAmendment.Visible = isEditable && isAmendment && (AppSession.IsAdmin || FunctionTable.IsEnableCreateAmendment());

            // Display\hide Renew buttons
            liRenewal.Visible = isEditable && CapUtil.InitRenewalButton(btnRenewal, dt.Rows[0]);
            
            // Temp solution to keep the Renewal LabelKey after postback, should change to sperate button in 7.3.1.
            RenewalBtnLabelKey = btnRenewal.LabelKey;

            if (!AppSession.IsAdmin)
            {
                divRenewalAdminBtnList.Visible = false;
            }

            // Display\hide trade license buttons
            liRequestTradeLicense.Visible = (isEditable && CapUtil.InitTradeLicenseButton(btnRequestLicense, dt.Rows[0])) || AppSession.IsAdmin;

            // Display\hide shooping cart link
            lblAddToShoppingCart.Visible = (isEditable && !AppSession.User.IsAnonymous && StandardChoiceUtil.IsEnableShoppingCart()) || AppSession.IsAdmin;

            // Display\hide clone record button
            if (CloneRecordUtil.IsDisplayCloneButton(capModel.capType, TempModelConvert.Trim4WSOfCapIDModel(capModel.capID), ModuleName, true)
                && !IsForceLoginToApplyPermit(ModuleName)
                && FunctionTable.IsEnableCloneRecord()
                && CloneRecordUtil.IsAnonymousCloneRecordAllowed(capModel.capType)
                && isEditable)
            {
                lnkCloneRecord.Visible = true;
            }
        }

        /// <summary>
        /// Displays the cap condition
        /// </summary>
        /// <param name="capWithConditionModel">cap conditionModel4WS</param>
        private void DisplayCapCondition(CapWithConditionModel4WS capWithConditionModel)
        {
            capConditions.Display(capWithConditionModel);
        }

        /// <summary>
        /// Displays the work location.
        /// </summary>
        /// <param name="sectionPermissions">storing section permissions information.</param>
        private void DisplayWorkLocation(Dictionary<string, UserRolePrivilegeModel> sectionPermissions)
        {
            bool isWorkLocationVisible = CapUtil.GetSectionVisibility(CapDetailSectionType.WORKLOCATION.ToString(), sectionPermissions, ModuleName);
            workLocation.IsHidden = !isWorkLocationVisible;
            divWorkLocation.Visible = isWorkLocationVisible;

            if (isWorkLocationVisible || AppSession.IsAdmin)
            {
                this.divWorkLocation.Visible = true;
                ExpandOrCollapseSection(shWorkLocation, "per_permitDetail_label_workLocation");
            }
            else
            {
                this.divWorkLocation.Visible = false;
            }
        }

        /// <summary>
        /// determines whether the attachments section is visible 
        /// for the current user according to section permissions
        /// </summary>
        /// <param name="capModel">the data model storing CAP information</param>
        /// <param name="sectionPermissions">storing section permissions information</param>
        /// <param name="isEditable">is editable</param>
        private void DisplayAttachmentsSection(CapModel4WS capModel, Dictionary<string, UserRolePrivilegeModel> sectionPermissions, bool isEditable)
        {
            bool isAttachmentVisible = CapUtil.GetSectionVisibility(CapDetailSectionType.ATTACHMENTS.ToString(), sectionPermissions, this.ModuleName);

            if (isAttachmentVisible || AppSession.IsAdmin)
            {
                divAttachmentContainer.Visible = true;
                attachmentEdit.IsEditable = isEditable;
                RegisterExpandScript(shAttachment, "per_attachment_Label_attachTitle", "ExpandAttachment", "ExpandAttachmentSection();");
            }
            else
            {
                divAttachmentContainer.Visible = false;
            }
        }

        /// <summary>
        /// Determines whether Inspection Section will be displayed according to the corresponding section permissions
        /// </summary>
        /// <param name="sectionPermissions">storing section permissions information</param>
        /// <param name="isEditable">is editable</param>
        private void DisplayInspections(Dictionary<string, UserRolePrivilegeModel> sectionPermissions, bool isEditable)
        {
            if (IsDisplayInspections(sectionPermissions) || AppSession.IsAdmin)
            {
                divInspection.Visible = true;
                InspectionList.IsEditable = isEditable;

                if (!AppSession.IsAdmin)
                {
                    IXPolicyBll xpolicyBll = ObjectFactory.GetObject<IXPolicyBll>();
                    string policyValue = xpolicyBll.GetPolicyValueForData4AsKey(XPolicyConstant.IS_EXPANDED_SECTION, ACAConstant.LEVEL_TYPE_MODULE, ModuleName, "ins_inspectionList_label_inspection");
                    bool isExpanded = !ValidationUtil.IsNo(policyValue);
                    shInspection.Collapsed = !isExpanded;

                    if (isExpanded)
                    {
                        string script = string.Format("scrollIntoView('{0}');", divInspection.ClientID);
                        Page.ClientScript.RegisterStartupScript(GetType(), "ScrollIntoInspectionSection", script, true);
                    }
                }
            }
            else
            {
                divInspection.Visible = false;
            }
        }

        /// <summary>
        /// Show educations in CAP detail page.
        /// </summary>        
        /// <param name="capModel">CAP model contains Education model list.</param>
        /// <param name="sectionPermissions">storing section permissions information</param>
        private void DisplayEducations(CapModel4WS capModel, Dictionary<string, UserRolePrivilegeModel> sectionPermissions)
        {
            bool isEducationSectionVisible = CapUtil.GetSectionVisibility(CapDetailSectionType.EDUCATION.ToString(), sectionPermissions, this.ModuleName);

            // judge whether display education list.
            if (isEducationSectionVisible || AppSession.IsAdmin)
            {
                divEducation.Visible = true;
                IList<EducationModel4WS> educationModelList = null;

                if (!AppSession.IsAdmin && capModel != null && capModel.educationList != null)
                {
                    educationModelList = ObjectConvertUtil.ConvertArrayToList(capModel.educationList);
                }

                educationList.GridViewDataSource = educationModelList;
                educationList.EducationSectionPosition = EducationOrExamSectionPosition.CapDetail;
                educationList.BindEducations();
                RegisterExpandScript(shEducation, "per_detail_education_section_name", string.Empty, string.Empty);
            }
            else
            {
                divEducation.Visible = false;
            }
        }

        /// <summary>
        /// Display continuing education list.
        /// </summary>
        /// <param name="capModel">cap model which contains continuing education models</param>
        /// <param name="sectionPermissions">storing section permissions information</param>
        private void DisplayContEducations(CapModel4WS capModel, Dictionary<string, UserRolePrivilegeModel> sectionPermissions)
        {
            bool isContEducationSectionVisible = CapUtil.GetSectionVisibility(CapDetailSectionType.CONTINUING_EDUCATION.ToString(), sectionPermissions, this.ModuleName);
            if (isContEducationSectionVisible || AppSession.IsAdmin)
            {
                divContEducation.Visible = true;

                // bind continuing education summary information.
                contEducationSummaryList.IsCapDetailPage = true;
                contEducationSummaryList.BindSummaryContEducation(capModel.contEducationList);

                // Convert array to list.
                List<ContinuingEducationModel4WS> contEducations = new List<ContinuingEducationModel4WS>();

                //if capModel.contEducation isn't null, it need convert to IList<ContinuingEducationModel4WS>.
                if (capModel != null && capModel.contEducationList != null)
                {
                    contEducations.AddRange(capModel.contEducationList);
                }

                // bind continuing education list.
                contEducationList.GridViewDataSource = contEducations;
                contEducationList.ContEducationSectionPosition = EducationOrExamSectionPosition.CapDetail;
                contEducationList.BindContEducations();
                RegisterExpandScript(shContEducation, "continuing_education_capdetail_section_name", string.Empty, string.Empty);
            }
            else
            {
                divContEducation.Visible = false;
            }
        }

        /// <summary>
        /// Show Examinations in CAP detail Page.
        /// </summary>
        /// <param name="capModel">CAP model contains Examination model List</param>
        /// <param name="sectionPermissions">storing section permissions information</param>
        /// <param name="isEditable">is editable</param>
        private void DisplayExamination(CapModel4WS capModel, Dictionary<string, UserRolePrivilegeModel> sectionPermissions, bool isEditable)
        {
            bool isExaminationSectionVisible = CapUtil.GetSectionVisibility(CapDetailSectionType.EXAMINATION.ToString(), sectionPermissions, this.ModuleName);

            if (isExaminationSectionVisible || AppSession.IsAdmin)
            {
                divExamination.Visible = true;
                ExaminationList.IsEditable = isEditable;

                IList<ExaminationModel> examinations = new List<ExaminationModel>();

                if (!AppSession.IsAdmin && capModel.examinationList != null)
                {
                    examinations = ObjectConvertUtil.ConvertArrayToList(capModel.examinationList);
                }

                ExaminationList.BindExaminationList();
                RegisterExpandScript(shExamination, "examination_title", string.Empty, string.Empty);
            }
            else
            {
                divExamination.Visible = false;
            }
        }

        /// <summary>
        /// Show valuation calculator in CAP detail Page.
        /// </summary>
        /// <param name="capModel">CAP model contains valuation calculator model List</param>
        /// <param name="sectionPermissions">storing section permissions information</param>
        private void DisplayValuationCalculator(CapModel4WS capModel, Dictionary<string, UserRolePrivilegeModel> sectionPermissions)
        {
            bool isValuationCalculatorsSectionVisible = CapUtil.GetSectionVisibility(CapDetailSectionType.VALUATION_CALCULATOR.ToString(), sectionPermissions, this.ModuleName);

            if (isValuationCalculatorsSectionVisible || AppSession.IsAdmin)
            {
                divValuationCalculator.Visible = true;

                if (!AppSession.IsAdmin)
                {
                    if (capModel.bCalcValuationListField != null)
                    {
                        ValuationCalculatorView.HideGroupName = true;
                    }
                }

                ValuationCalculatorView.DisplayValuationCalculator(capModel.bCalcValuationListField);
                RegisterExpandScript(shValuationCalculator, "valuationcalculator_title", string.Empty, string.Empty);
            }
            else
            {
                divValuationCalculator.Visible = false;
            }
        }

        /// <summary>
        /// Display Permit Report Button
        /// </summary>
        /// <param name="errorInfo">error Info message</param>
        /// <param name="isDisplayed">is displayed.</param>
        /// <param name="reportID">report id string.</param>
        /// <param name="reportName">report name</param>
        private void DisplayPermitReportBtn(string errorInfo, bool isDisplayed, string reportID, string reportName)
        {
            if (!string.IsNullOrEmpty(reportID))
            {
                lnkPrintPermit.ReportID = reportID;
            }

            if (string.IsNullOrEmpty(errorInfo) && isDisplayed)
            {
                this.lnkPrintPermit.Visible = true;
                this.lnkPrintPermit.ToolTip = reportName;
                this.PermitReportID = reportID;
            }
            else
            {
                Logger.Info(errorInfo);
                this.lnkPrintPermit.Visible = false;
            }
        }

        /// <summary>
        /// Display Permit Summary Report Button
        /// </summary>
        /// <param name="errorInfo">error Info message.</param>
        /// <param name="isDisplayed">Is Displayed</param>
        /// <param name="reportID">report ID string.</param>
        /// <param name="reportName">report name</param>
        private void DisplayPermitSummaryReportBtn(string errorInfo, bool isDisplayed, string reportID, string reportName)
        {
            if (!string.IsNullOrEmpty(reportID))
            {
                lnkPrintSummary.ReportID = reportID;
            }

            if (string.IsNullOrEmpty(errorInfo) && isDisplayed)
            {
                this.lnkPrintSummary.Visible = true;
                this.lnkPrintSummary.ToolTip = reportName;
                this.SummaryReportID = reportID;
            }
            else
            {
                Logger.Info(errorInfo);
                this.lnkPrintSummary.Visible = false;
            }
        }

        /// <summary>
        /// Display Receipt Report Button
        /// </summary>
        /// <param name="errorInfo">error Info message</param>
        /// <param name="isDisplayed">is displayed.</param>
        /// <param name="receiptNbr">receipt Number</param>
        /// <param name="reportID">report id string.</param>
        /// <param name="reportName">report name</param>
        private void DisplayReceiptReportBtn(string errorInfo, bool isDisplayed, string receiptNbr, string reportID, string reportName)
        {
            if (!string.IsNullOrEmpty(reportID))
            {
                lnkPrintReceipt.ReportID = reportID;
            }

            this.ReceiptNbr = receiptNbr;

            if (string.IsNullOrEmpty(errorInfo) && isDisplayed && !string.IsNullOrEmpty(receiptNbr))
            {
                this.DisplayReceiptReport = true;
                this.ReportName = reportName;
                this.ReportID = reportID;
            }
            else
            {
                if (!string.IsNullOrEmpty(errorInfo))
                {
                    Logger.Info(errorInfo);
                }

                if (string.IsNullOrEmpty(receiptNbr))
                {
                    Logger.Info("Because no receipt number, Receipt Report Button will disappears in the page.");
                }

                this.DisplayReceiptReport = false;
            }
        }

        /// <summary>
        /// Determines whether the trust account section is visible for the current user 
        /// according to the section permissions
        /// </summary>
        /// <param name="capModel">the current cap model.</param>
        /// <param name="sectionPermissions">the current cap model permissions.</param>
        /// <param name="isEditable">is editable.</param>
        private void DisplayTrustAccount(CapModel4WS capModel, Dictionary<string, UserRolePrivilegeModel> sectionPermissions, bool isEditable)
        {
            bool isTrustAcctSectionVisible = CapUtil.GetSectionVisibility(CapDetailSectionType.TRUST_ACCOUNT.ToString(), sectionPermissions, ModuleName);

            if (isTrustAcctSectionVisible || AppSession.IsAdmin)
            {
                divTrustAcct.Visible = true;
                trustAcctList.IsEditable = isEditable;

                IList<TrustAccountModel> trustAcct = null;

                if (!AppSession.IsAdmin)
                {
                    ITrustAccountBll trustAccountBll = (ITrustAccountBll)ObjectFactory.GetObject(typeof(ITrustAccountBll));

                    CapIDModel capID = new CapIDModel();
                    if (capModel != null && capModel.capID != null)
                    {
                        capID.serviceProviderCode = capModel.capID.serviceProviderCode;
                        capID.ID1 = capModel.capID.id1;
                        capID.ID2 = capModel.capID.id2;
                        capID.ID3 = capModel.capID.id3;
                    }

                    trustAcct = trustAccountBll.GetTrustAccountListByCAPID(capID);
                }

                trustAcctList.BindList(trustAcct);
                RegisterExpandScript(shTrustAcct, "per_permitdetail_trustaccount_title", string.Empty, string.Empty);
            }
            else
            {
                divTrustAcct.Visible = false;
            }
        }

        /// <summary>
        /// Displays the assets.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="sectionPermissions">The section permissions.</param>
        private void DisplayAssets(CapModel4WS capModel, Dictionary<string, UserRolePrivilegeModel> sectionPermissions)
        {
            bool isAssetSectionVisible = CapUtil.GetSectionVisibility(CapDetailSectionType.ASSETS.ToString(), sectionPermissions, this.ModuleName);

            if (isAssetSectionVisible || AppSession.IsAdmin)
            {
                divAsset.Visible = true;
                assetList.ConfirmOrDetailPage = ACAConstant.ASSETLIST_LAYOUT_CAP_DETAIL;

                if (capModel != null && capModel.assetList != null)
                {
                    assetList.Display(capModel.assetList.ToList<AssetMasterModel>());
                }

                RegisterExpandScript(shAsset, "aca_capdetail_label_assettitle", string.Empty, string.Empty);
            }
            else
            {
                divAsset.Visible = false;
            }
        }

        /// <summary>
        /// Gets a boolean value representing whether any report button is visible for the current user.
        /// </summary>
        /// <returns>true if any report button is visible for the current user; otherwise, false</returns>
        private bool ExistVisibleReportButtons()
        {
            bool isExistingVisibleButton = lnkPrintSummary.Visible || lnkPrintPermit.Visible || lnkPrintReceipt.Visible || lnkCreateAmendment.Visible || btnRenewal.Visible || btnRequestLicense.Visible || lnkCloneRecord.Visible;

            return isExistingVisibleButton;
        }

        /// <summary>
        /// Displays report buttons which are visible for the current user according to the Admin settings
        /// </summary>
        /// <param name="capIdModel">a capIdModel</param>        
        private void DisplayReportButton(CapIDModel4WS capIdModel)
        {
            string receiptNbr = GetReceiptNbr(capIdModel);
            bool isEnableReportForAnonymousUser = StandardChoiceUtil.IsEnableReportForAnonymousUser();

            try
            {
                IReportBll reportBll = (IReportBll)ObjectFactory.GetObject(typeof(IReportBll));
                ReportButtonPropertyModel4WS[] arrayRBTModel = reportBll.GetReportButtonProperty(capIdModel, AppSession.User.PublicUserId, ModuleName);
                foreach (ReportButtonPropertyModel4WS ws in arrayRBTModel)
                {
                    string buttonName = ws.buttonName;
                    string errorInfo = ws.errorInfo;
                    string reportID = ws.reportId;
                    string reportName = I18nStringUtil.GetString(ws.resReportName, ws.reportName);
                    bool isDisplayed = ws.isDisplayed;

                    //if (ACAConstant.PRINT_PERMIT_REPORT.Equals(buttonName) && isEnableReportForAnonymousUser)
                    if (ACAConstant.PRINT_PERMIT_REPORT.Equals(buttonName, StringComparison.InvariantCulture) && isEnableReportForAnonymousUser)
                    {
                        DisplayPermitReportBtn(errorInfo, isDisplayed, reportID, reportName);
                        continue;
                    }
                    else if (ACAConstant.PRINT_PAYMENT_RECEIPT_REPORT.Equals(buttonName, StringComparison.InvariantCulture) && isEnableReportForAnonymousUser)
                    {
                        DisplayReceiptReport = isDisplayed;
                        DisplayReceiptReportBtn(errorInfo, isDisplayed, receiptNbr, reportID, reportName);
                        continue;
                    }
                    else if (ACAConstant.PRINT_PERMIT_SUMMARY_REPORT.Equals(buttonName, StringComparison.InvariantCulture))
                    {
                        DisplayPermitSummaryReportBtn(errorInfo, isDisplayed, reportID, reportName);
                        continue;
                    }
                }
            }
            catch (ACAException ex)
            {
                Logger.Error(ex);
            }
        }

        /// <summary>
        /// Get agency code.
        /// </summary>
        /// <returns>agency code.</returns>
        private string GetAgencyCode()
        {
            //set default agency, if no super agency, we due with old logic
            string agency = ConfigManager.AgencyCode;

            //we will get AgencyCode from QueryString,and this parameter indicates which agency the record belongs to
            if (Request.QueryString[UrlConstant.AgencyCode] != null)
            {
                agency = Request.QueryString[UrlConstant.AgencyCode].ToUpper();
            }

            return agency;
        }

        /// <summary>
        /// Get simple CAP model.
        /// </summary>
        /// <returns>SimpleCapModel4WS array</returns>
        private SimpleCapModel[] GetSelectedCAPs()
        {
            SimpleCapModel[] simpleCapModelList = new SimpleCapModel[1];
            SimpleCapModel simpleCapModel = new SimpleCapModel();

            CapIDModel capIdModel = new CapIDModel();

            if (Request.QueryString["capID1"] == null)
            {
                CapIDModel4WS capId = AppSession.GetCapModelFromSession(ModuleName).capID;
                capIdModel.serviceProviderCode = capId.serviceProviderCode;
                capIdModel.ID1 = capId.id1;
                capIdModel.ID2 = capId.id2;
                capIdModel.ID3 = capId.id3;
            }
            else
            {
                capIdModel.serviceProviderCode = _agencyCode;
                capIdModel.ID1 = UpperQueryString("capID1");
                capIdModel.ID2 = UpperQueryString("capID2");
                capIdModel.ID3 = UpperQueryString("capID3");
            }

            simpleCapModel.capID = capIdModel;
            simpleCapModelList[0] = simpleCapModel;

            return simpleCapModelList;
        }

        /// <summary>
        /// Is amendable Cap Available
        /// </summary>
        /// <param name="moduleName">the module name.</param>
        /// <returns>true or false.</returns>
        private bool IsAmendableCapAvailable(string moduleName)
        {
            //Check if the CAP type is configured to be amended.
            CapModel4WS capModel4MS = AppSession.GetCapModelFromSession(ModuleName);
            bool isAmendable = false;

            if (AppSession.IsAdmin)
            {
                isAmendable = true;
            }
            else if (!AppSession.User.IsAnonymous && capModel4MS != null && capModel4MS.capType != null)
            {
                //If owner, contact and LP of the cap is associated with public user, or public user created this cap, this public user
                //has right to amend this cap.
                var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();
                UserRolePrivilegeModel userRole = userRoleBll.GetRecordSearchRole(ConfigManager.AgencyCode, ModuleName, capModel4MS.capType);

                if (userRole.allAcaUserAllowed || userRole.registeredUserAllowed)
                {
                    userRole = userRoleBll.GetDefaultRole();
                }

                var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
                bool hasRight = proxyUserRoleBll.HasPermission(AppSession.GetCapModelFromSession(moduleName), userRole, ProxyPermissionType.AMENDMENT);
                bool isModelAmendable = capModel4MS.capType.isAmendable == ACAConstant.COMMON_Y ? true : false;
                isAmendable = hasRight && isModelAmendable;
            }

            return isAmendable;
        }

        /// <summary>
        /// Set the selected caps to 'Pop Adding' control.
        /// </summary>
        private void SetSelectedCAPsToAddingControl()
        {
            //Set selected items to SimpleCapModelList property for CollectionEdit control.
            if (!string.IsNullOrEmpty(Request.Form["__EVENTTARGET"]))
            {
                if (Request.Form["__EVENTTARGET"].IndexOf("btnAdd") > -1)
                {
                    addForDetailPage.SimpleCapModelList = GetSelectedCAPs();
                }
            }
        }

        /// <summary>
        /// get template value from web service then fill the template to specified cap model.
        /// In cap detail page, only parcel, contact list need show template.
        /// </summary>
        /// <param name="capModel">need to be filled cap model with template.</param>
        private void FillCapModelTemplateValue(CapModel4WS capModel)
        {
            if (capModel == null ||
                capModel.capID == null)
            {
                return;
            }

            ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));

            if (capModel.parcelModel != null && capModel.parcelModel.parcelModel != null
                && !string.IsNullOrEmpty(capModel.parcelModel.parcelModel.parcelNumber))
            {
                capModel.parcelModel.parcelModel.templates = templateBll.GetDailyAPOTemplateAttributes(TemplateType.CAP_PARCEL, capModel.capID, capModel.parcelModel.parcelModel.parcelNumber, ConfigManager.AgencyCode, AppSession.User.PublicUserId);
            }

            CapUtil.FillAllContactTemplateValue(templateBll, capModel);
        }

        /// <summary>
        /// Uppers the query string.
        /// </summary>
        /// <param name="queryKey">The query key.</param>
        /// <returns>Convert the query string to upper case.</returns>
        private string UpperQueryString(string queryKey)
        {
            string value = Server.UrlDecode(Request.QueryString[queryKey]);

            return string.IsNullOrEmpty(value) ? null : value.ToUpper();
        }

        /// <summary>
        /// Set the social media bar's status and return.
        /// </summary>
        /// <returns>Social media bar's status. true -  visible, false -  invisible.</returns>
        private bool EnableSocialMediaButton()
        {
            bool enableSocialMediaBar = false;

            if (!AppSession.IsAdmin)
            {
                IXPolicyBll xPolicyBll = ObjectFactory.GetObject<IXPolicyBll>();
                string buttonPermession = xPolicyBll.GetValueByKey(
                    XPolicyConstant.SOCIAL_MEDIA_SHARE_BUTTON_PERMISSION, ACAConstant.LEVEL_TYPE_MODULE, ModuleName);

                if (SocialMediaButtonStatus.All.Equals(buttonPermession))
                {
                    enableSocialMediaBar = true;
                }
                else if (SocialMediaButtonStatus.Creator.Equals(buttonPermession))
                {
                    CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                    if (capModel.createdBy == AppSession.User.PublicUserId)
                    {
                        enableSocialMediaBar = true;
                    }
                }
            }

            divSocialMedia.Visible = enableSocialMediaBar;

            return enableSocialMediaBar;
        }

        #endregion Private Methods
    }
}