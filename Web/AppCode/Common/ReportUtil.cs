#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ReportUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 *      $Id: ReportUtil.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;

using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Report;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// report utility.
    /// </summary>
    public static class ReportUtil
    {
        /// <summary>
        /// Fixed Parameter prefix.
        /// </summary>
        private const string FIXED_PARAMETER_PREFIX = "FixedParameter.";

        /// <summary>
        /// empty parameter.
        /// </summary>
        private const string REPORT_BLANK_PARAM = "-1";

        /// <summary>
        /// Logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(ReportUtil));

        #region PROPERTY
        /// <summary>
        /// Gets selected ALtIDs in permit list.
        /// </summary>
        private static string AltIDs
        {
            get
            {
                string id = HttpContext.Current.Request.QueryString[ACAConstant.ID];
                return string.IsNullOrEmpty(id) ? string.Empty : id;
            }
        }

        /// <summary>
        /// Gets the module name
        /// </summary>
        private static string SubModule
        {
            get
            {
                string subModule = HttpContext.Current.Request.QueryString["SubModule"];
                return string.IsNullOrEmpty(subModule) ? string.Empty : subModule;
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// Generates the report file.
        /// </summary>
        /// <param name="reportType">Type of the report.</param>
        /// <param name="reportId">The report id.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="capId">The cap id.</param>
        /// <param name="reprintReason">The reprint reason.</param>
        /// <param name="page">The page.</param>
        public static void GenerateReportFile(string reportType, string reportId, string moduleName, string capId, string reprintReason, Page page)
        {
            string servericePath = GenerateReportFile(reportType, reportId, moduleName, capId, reprintReason);

            ScriptManager.RegisterStartupScript(
                page,
                page.GetType(),
                "Printer Report",
                "window.opener.printLabelReport('filepath=" + servericePath + "');window.close();",
                true);
        }

        /// <summary>
        /// Generates the report file.
        /// </summary>
        /// <param name="reportType">Type of the report.</param>
        /// <param name="reportId">The report id.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="capId">The cap id.</param>
        /// <param name="reprintReason">The reprint reason.</param>
        /// <returns>The service path for the generated report file.</returns>
        public static string GenerateReportFile(string reportType, string reportId, string moduleName, string capId, string reprintReason)
        {
            //set parameter to session, handle the invisible parameter re-send to generate report.
            ParameterModel4WS[] reportParameters = GetReportParameters(reportId, ACAConstant.PRINT_LABEL_VIEWID, moduleName);
            AppSession.SetReportParameterToSession(reportParameters);
            string fileName = GetReportDetail(reportType, reportId, moduleName, capId, reprintReason);

            string servericePath = FileUtil.AppendAbsolutePath("AuthorizedAgent/ReportManagerHandler.ashx?filename=" + fileName);
            return HttpContext.Current.Server.HtmlEncode(servericePath);
        }

        /// <summary>
        /// Handlers the reprint.
        /// </summary>
        /// <param name="detailModel">The detail model.</param>
        /// <param name="capId">The cap id.</param>
        /// <param name="needCount">if set to <c>true</c> [need count].</param>
        /// <returns>Indicate handler success or not.</returns>
        public static bool HandlerReprint(ReportDetailModel4WS detailModel, string capId, bool needCount = true)
        {
            bool isReprint = false;
            bool lastChance = false;
            return HandlerReprint(detailModel, capId, needCount, ref isReprint, ref lastChance);
        }

        /// <summary>
        /// Determines whether the specified detail model is reprint.
        /// </summary>
        /// <param name="detailModel">The detail model.</param>
        /// <param name="capId">The cap id.</param>
        /// <param name="needCount">if set to <c>true</c> [need count].</param>
        /// <param name="isReprint">if set to <c>true</c> [is reprint].</param>
        /// <param name="isLastChance">if set to <c>true</c> [last chance].</param>
        /// <returns>Indicate handler success or not.</returns>
        public static bool HandlerReprint(ReportDetailModel4WS detailModel, string capId, bool needCount, ref bool isReprint, ref bool isLastChance)
        {
            bool accessable = true;
            Hashtable ht = new Hashtable();

            if (HttpContext.Current.Session[ACAConstant.SESSION_AUTHORIZED_AGENT_REPRINT_LIMIT] != null)
            {
                ht = HttpContext.Current.Session[ACAConstant.SESSION_AUTHORIZED_AGENT_REPRINT_LIMIT] as Hashtable;
            }

            if (ht == null)
            {
                ht = new Hashtable();
            }

            int printNumber = 0;

            if (ht.ContainsKey(capId))
            {
                printNumber = (int)ht[capId];
                isReprint = true;

                if (needCount)
                {
                    ht.Remove(capId);
                }
            }

            if (printNumber < detailModel.reprintLimit4ACA && detailModel.reprintLimit4ACA != -1)
            {
                printNumber += 1;
            }
            else if (printNumber == detailModel.reprintLimit4ACA && detailModel.reprintLimit4ACA != -1)
            {
                printNumber += 1;
                isLastChance = true;
            }
            else if (detailModel.reprintLimit4ACA != -1)
            {
                accessable = false;
            }
            else
            {
                printNumber += 1;
            }

            if (needCount)
            {
                ht.Add(capId, printNumber);
            }

            HttpContext.Current.Session[ACAConstant.SESSION_AUTHORIZED_AGENT_REPRINT_LIMIT] = ht;

            return accessable;
        }

        /// <summary>
        /// Gets a value indicating whether is reprint label.
        /// </summary>
        /// <param name="capId">Record ID.</param>
        /// <returns>Indicate whether is reprint label.</returns>
        public static bool IsLabelReprint(string capId)
        {
            bool isReprint = false;
            Hashtable ht = new Hashtable();

            if (AppSession.AuthorizedAgentReprintLimit != null)
            {
                ht = AppSession.AuthorizedAgentReprintLimit as Hashtable;
            }

            if (ht == null)
            {
                ht = new Hashtable();
            }

            // If the record ID existing in the session, treated as re-print.
            if (ht.ContainsKey(capId))
            {
                isReprint = true;
            }
            else
            {
                ht.Add(capId, 1);
                AppSession.AuthorizedAgentReprintLimit = ht;
            }

            return isReprint;
        }

        /// <summary>
        /// Get report parameter by report id and session variables
        /// </summary>
        /// <param name="reportID">string report id</param>
        /// <param name="reportType">report portlet.</param>
        /// <param name="moduleName">the module name.</param>
        /// <returns>
        /// ParameterModel4WS array
        /// </returns>
        public static ParameterModel4WS[] GetReportParameters(string reportID, string reportType, string moduleName)
        {
            string collectionID = HttpContext.Current.Request["collectionID"] ?? string.Empty;
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

            //build session variables
            Hashtable sessionVariables = new Hashtable();
            sessionVariables.Add(SessionVaraibleType.CapModel.ToString(), capModel);
            sessionVariables.Add(SessionVaraibleType.CollectionID.ToString(), collectionID);
            sessionVariables.Add(SessionVaraibleType.ModuleName.ToString(), moduleName);

            string emailReportName = GetEmailReportName();
            CapIDModel4WS capIDModel = capModel == null ? null : capModel.capID;
            ReportInfoModel4WS reportInfoModel = GetReportInfoModel4WS(reportType, reportID, capIDModel, moduleName, emailReportName);

            IReportBll reportBll = (IReportBll)ObjectFactory.GetObject(typeof(IReportBll));
            ParameterModel4WS[] parameters = reportBll.GetParametersByReportId(sessionVariables, reportInfoModel, reportType);

            if (parameters == null || parameters.Length == 0)
            {
                return null;
            }

            foreach (ParameterModel4WS param in parameters)
            {
                // Get I18N module name
                if (ParameterType.SessionVariable.ToString().Equals(param.parameterType, StringComparison.InvariantCultureIgnoreCase) && SessionContextValue.module.ToString().Equals(param.defaultValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(moduleName))
                    {
                        string tmpModuleName = LabelUtil.RemoveHtmlFormat(LabelUtil.GetTextByKey("aca_sys_default_module_name", moduleName));

                        // if no user defined label for module name, get the module from url request.
                        if (tmpModuleName == LabelUtil.GetGlobalTextByKey("aca_sys_default_module_name") && !string.IsNullOrEmpty(moduleName))
                        {
                            tmpModuleName = DataUtil.AddBlankToString(moduleName);
                        }

                        param.dispDefaultValue = tmpModuleName;
                    }
                }
            }

            return parameters;
        }

        /// <summary>
        /// return error message or null.
        /// </summary>
        /// <param name="reportResultModel">reportResultModel model.</param>
        /// <returns>true / false.</returns>
        public static bool IsValidReportResultModel(ReportResultModel4WS reportResultModel)
        {
            if (reportResultModel == null ||
                IsErrorMessages(reportResultModel.reportMessages) ||
                reportResultModel.content == null ||
                reportResultModel.responseHeaders == null ||
                reportResultModel.responseHeadersValues == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get agency code.
        /// </summary>
        /// <param name="reportType">The report type</param>
        /// <param name="subCapID">The cap id</param>
        /// <returns>agency code</returns>
        public static string GetAgency(string reportType, CapIDModel4WS subCapID)
        {
            // In super agency, report list in navigation always get super agency code to get report link no matter what process.
            string agencyCode = subCapID == null ? ConfigManager.AgencyCode : subCapID.serviceProviderCode;

            if (ACAConstant.PRINT_REPORT_LIST.Equals(reportType, StringComparison.InvariantCultureIgnoreCase))
            {
                agencyCode = ConfigManager.SuperAgencyCode;
            }

            return agencyCode;
        }

        /// <summary>
        /// get cap type
        /// </summary>
        /// <param name="subCapID">The cap id</param>
        /// <param name="moduleName">The module name</param>
        /// <returns>cap id model</returns>
        public static CapIDModel4WS GetCapID(CapIDModel4WS subCapID, string moduleName)
        {
            CapIDModel4WS capID = null;

            if (subCapID != null)
            {
                capID = subCapID;
            }
            else
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
                capID = capModel == null ? new CapIDModel4WS() : capModel.capID;
            }

            return capID;
        }

        /// <summary>
        /// Gets ReportInfoModel for report web service.
        /// </summary>
        /// <param name="reportType">report type include:1.PRINT_PERMIT_REPORT; 2.PRINT_PERMIT_SUMMARY_REPORT; 3.PRINT_PAYMENT_RECEIPT_REPORT.</param>
        /// <param name="reportId">The report id.</param>
        /// <param name="subCapID">The Cap ID</param>
        /// <param name="moduleName">The Module Name</param>
        /// <param name="emailReportName">The email report name</param>
        /// <returns>
        /// ReportInfoModel4WS model.
        /// </returns>
        public static ReportInfoModel4WS GetReportInfoModel4WS(string reportType, string reportId, CapIDModel4WS subCapID, string moduleName, string emailReportName)
        {
            CapIDModel4WS capID = GetCapID(subCapID, moduleName);
            string agencyCode = GetAgency(reportType, subCapID);

            if (string.IsNullOrEmpty(reportId))
            {
                reportId = HttpContext.Current.Request.QueryString["reportID"];
            }

            IReportBll reportBll = (IReportBll)ObjectFactory.GetObject(typeof(IReportBll));
            ReportDetailModel4WS reportDetail = reportBll.GetReportDetail(reportId, agencyCode);

            ReportInfoModel4WS reportInfoModel4WS = new ReportInfoModel4WS();
            reportInfoModel4WS.edMSEntityIdModel = GetEdmsEntityIDModel(capID, reportType);

            reportInfoModel4WS.servProvCode = agencyCode;

            if (SubModule != string.Empty)
            {
                reportInfoModel4WS.module = SubModule;
            }
            else if (reportType != ACAConstant.PRINT_HOMEPAGE_REPORT)
            {
                reportInfoModel4WS.module = string.IsNullOrEmpty(moduleName) ? ACAConstant.MODULE_ALL : moduleName;
            }

            reportInfoModel4WS.callerId = AppSession.User.PublicUserId;
            reportInfoModel4WS.ssOAuthId = GetSessionID();

            if (!string.IsNullOrEmpty(reportId) && ValidationUtil.IsNumber(reportId))
            {
                reportInfoModel4WS.reportId = long.Parse(reportId);
            }

            string reportOutputType = reportDetail == null ? string.Empty : reportDetail.reportType;

            reportInfoModel4WS = AddFixedParams(reportInfoModel4WS, reportOutputType, reportType, capID);

            if (ACAConstant.PRINT_REPORT_LIST.Equals(reportOutputType, StringComparison.InvariantCulture))
            {
                reportInfoModel4WS = AddSelectedAltIDs(reportInfoModel4WS);
            }

            reportInfoModel4WS = AddDynamicParams(reportInfoModel4WS, reportType);

            if (ACAConstant.REPORT_RTF_OUTPUT_TYPE.Equals(reportOutputType) || ACAConstant.REPORT_URL_OUTPUT_TYPE.Equals(reportOutputType))
            {
                reportInfoModel4WS = AddSessionVariables(reportInfoModel4WS, moduleName);
            }

            reportInfoModel4WS.emailReportName = emailReportName;

            if (ACAConstant.PRINT_LABEL_VIEWID.Equals(reportType, StringComparison.InvariantCultureIgnoreCase))
            {
                reportInfoModel4WS.reprintReason = GetReprintReason();
            }

            return reportInfoModel4WS;
        }

        /// <summary>
        /// Get sub cap id model.
        /// </summary>
        /// <returns>cap id model.</returns>
        public static CapIDModel4WS GetSubCapID()
        {
            CapIDModel4WS subCapID = null;

            if (HttpContext.Current.Request.QueryString["subID1"] != null)
            {
                string subID1 = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.QueryString["subID1"]);
                string subID2 = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.QueryString["subID2"]);
                string subID3 = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.QueryString["subID3"]);
                string subAgency = HttpContext.Current.Request.QueryString[UrlConstant.AgencyCode];
                string subCustomerID = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.QueryString["subCustomerID"]);
                subCapID = new CapIDModel4WS();
                subCapID.id1 = subID1;
                subCapID.id2 = subID2;
                subCapID.id3 = subID3;
                subCapID.serviceProviderCode = subAgency;
                subCapID.customID = subCustomerID;
            }

            return subCapID;
        }

        /// <summary>
        /// Gets the name of the email report.
        /// </summary>
        /// <returns>the name of the email report.</returns>
        public static string GetEmailReportName()
        {
            string emailReportName = HttpContext.Current.Request.QueryString[ACAConstant.EMAIL_REPORT_NAME];
            return !string.IsNullOrEmpty(emailReportName) ? emailReportName : HttpContext.Current.Session.SessionID + "|" + CommonUtil.GetRandomUniqueID().Substring(0, 18);
        }

        /// <summary>
        /// show config error
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="labelKey">The label key.</param>
        public static void ShowError(Page currentPage, string moduleName, string labelKey)
        {
            string msg = LabelUtil.GetTextByKey(labelKey, moduleName);
            string script = "if(window.opener != 'undefined' && window.opener != null){window.opener.showNormalMessage('"
                + MessageUtil.FilterQuotation(msg) + "','Error');} else if (window.parent.opener !='undefined' && window.parent.opener != null){window.parent.opener.showNormalMessage('"
                + MessageUtil.FilterQuotation(msg) + "','Error');}window.close();if(window.parent != 'undefined' || window.parent != null){window.parent.close();}";
            currentPage.ClientScript.RegisterClientScriptBlock(currentPage.GetType(), "ShowReportErrorJS", script, true);
        }

        /// <summary>
        /// Check if the report is available in ACA
        /// </summary>
        /// <param name="reportType">Report Type</param>
        /// <param name="reportId">Report ID</param>
        /// <param name="capID">The CAP ID.</param>
        /// <param name="moduleName">Module Name</param>
        /// <returns>Whether the report is available in ACA.</returns>
        public static bool CheckPermissionOnReport(string reportType, string reportId, CapIDModel4WS capID, string moduleName)
        {
            bool hasPermission = true;

            // No report id and report type need to be checked, so it is always false.
            if (string.IsNullOrEmpty(reportId) || string.IsNullOrEmpty(reportType))
            {
                hasPermission = false;
                return hasPermission;
            }

            try
            {
                IReportBll reportBll = (IReportBll)ObjectFactory.GetObject(typeof(IReportBll));

                // 1. This function is for 3 kinds of report template.
                if (ACAConstant.PRINT_PERMIT_REPORT.Equals(reportType, StringComparison.InvariantCulture)
                    || ACAConstant.PRINT_PAYMENT_RECEIPT_REPORT.Equals(reportType, StringComparison.InvariantCulture)
                    || ACAConstant.PRINT_PERMIT_SUMMARY_REPORT.Equals(reportType, StringComparison.InvariantCulture)
                    || ACAConstant.PRINT_LICENSE_REPORT.Equals(reportType, StringComparison.InvariantCulture)
                    || ACAConstant.PRINT_REQUIREMENTS_REPORT.Equals(reportType, StringComparison.InvariantCulture))
                {
                    hasPermission = false;

                    // Below code is to initialize one CAP ID Model when Shopping Cart is enabled. 
                    if (capID == null)
                    {
                        capID = new CapIDModel4WS();
                        capID.serviceProviderCode = ConfigManager.AgencyCode;
                    }

                    // 1.1 Check if current module support this kind report.
                    ReportButtonPropertyModel4WS[] arrayRBTModel = reportBll.GetReportButtonProperty(capID, AppSession.User.PublicUserId, moduleName);

                    foreach (ReportButtonPropertyModel4WS ws in arrayRBTModel)
                    {
                        if (reportType.Equals(ws.buttonName, StringComparison.InvariantCulture)
                            && ws.isDisplayed
                            && string.IsNullOrEmpty(ws.errorInfo)
                            && reportId.Equals(ws.reportId))
                        {
                            hasPermission = true;
                            break;
                        }
                    }

                    // 1.2 Check if anonymouse user has permission to access report.
                    if (hasPermission && AppSession.User.IsAnonymous)
                    {
                        if (ACAConstant.PRINT_PERMIT_REPORT.Equals(reportType, StringComparison.InvariantCulture)
                            || ACAConstant.PRINT_PAYMENT_RECEIPT_REPORT.Equals(reportType, StringComparison.InvariantCulture))
                        {
                            if (!StandardChoiceUtil.IsEnableReportForAnonymousUser())
                            {
                                hasPermission = false;
                            }
                        }
                    }

                    // 1.3 Check if current user has permission to access this application.
                    if (hasPermission)
                    {
                        if (!string.IsNullOrEmpty(capID.id1)
                            && !string.IsNullOrEmpty(capID.id2)
                            && !string.IsNullOrEmpty(capID.id3)
                            && !string.IsNullOrEmpty(capID.serviceProviderCode))
                        {
                            // Here, it will throw exception when CAP ID does not exist.
                            CapWithConditionModel4WS capWithConditionModel = CapUtil.GetCapWithConditionModel4WS(capID, AppSession.User.UserSeqNum, true);
                            if (capWithConditionModel == null || capWithConditionModel.capModel == null)
                            {
                                hasPermission = false;
                            }
                        }
                    }
                }
                else if (ACAConstant.PRINT_REPORT_LIST.Equals(reportType, StringComparison.InvariantCulture))
                {
                    // 2. This function is for report list.
                    hasPermission = false;

                    ReportButtonPropertyModel4WS[] reports = reportBll.GetReportLinkProperty(capID, moduleName, null);
                    foreach (ReportButtonPropertyModel4WS ws in reports)
                    {
                        if (reportType.Equals(ws.buttonName, StringComparison.InvariantCulture)
                             && reportId.Equals(ws.reportId)
                             && ws.isDisplayed
                             && string.IsNullOrEmpty(ws.errorInfo))
                        {
                            hasPermission = true;
                            break;
                        }
                    }
                }
                else if (ACAConstant.PRINT_LABEL_VIEWID.Equals(reportType, StringComparison.InvariantCultureIgnoreCase))
                {
                    // 3. This branch is for Print Tag report
                    hasPermission = false;

                    if ((AppSession.User.IsAgentClerk || AppSession.User.IsAuthorizedAgent) 
                        && AppSession.User.IsAuthAgentNeedPrinter)
                    {
                        hasPermission = true;
                    }
                }
            }
            catch (Exception ex)
            {
                hasPermission = false;
            }

            return hasPermission;
        }

        /// <summary>
        /// Login V360 system to get session ID.
        /// </summary>
        /// <returns>Session ID</returns>
        private static string GetSessionID()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string callID = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_V360_WEB_ACTION_USERNAME);
            string passWord = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_V360_WEB_ACTION_PASSWORD);

            ISSOBll ssoBll = (ISSOBll)ObjectFactory.GetObject(typeof(ISSOBll));
            return ssoBll.Signon(ConfigManager.AgencyCode, callID, passWord);
        }

        /// <summary>
        /// Gets EDMSEntityIDModel for web service.
        /// </summary>
        /// <param name="capID">Cap ID Model for web service.</param>
        /// <param name="reportType">report type to distinguish whether click report button or not.</param>
        /// <returns>EDMSEntityIdModel4WS model</returns>
        private static EDMSEntityIdModel4WS GetEdmsEntityIDModel(CapIDModel4WS capID, string reportType)
        {
            EDMSEntityIdModel4WS edmsEntityID = new EDMSEntityIdModel4WS();

            if (!string.IsNullOrEmpty(AltIDs))
            {
                edmsEntityID.ID = AltIDs;
            }

            edmsEntityID.altId = capID.customID;
            edmsEntityID.capId = string.Format("{0}{1}{2}{3}{4}", capID.id1, ACAConstant.SPLIT_CHAR4, capID.id2, ACAConstant.SPLIT_CHAR4, capID.id3);

            return edmsEntityID;
        }

        /// <summary>
        /// Get Reprint Reason.
        /// For Label print, user need to provider reason for re-print, the reason id will be passed to <c>ReprintReason.aspx</c> page as a url parameter.
        /// </summary>
        /// <returns>The reprint reason.</returns>
        private static string GetReprintReason()
        {
            string reprintReason = null;
            string reprintReasonId = HttpContext.Current.Request.QueryString["ReasonId"];

            // get the reprint reason
            if (!string.IsNullOrEmpty(reprintReasonId))
            {
                IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();

                var domainModel =
                    bizBll.GetBizDomainListByModel(
                        new BizDomainModel4WS()
                        {
                            serviceProviderCode = ConfigManager.AgencyCode,
                            dispositionID = long.Parse(reprintReasonId),
                            bizdomain = BizDomainConstant.STD_REPRINT_REASONS
                        },
                        AppSession.User.PublicUserId);

                reprintReason = string.IsNullOrEmpty(domainModel.resBizdomainValue)
                                       ? domainModel.bizdomainValue
                                       : domainModel.resBizdomainValue;
            }

            return reprintReason;
        }

        /// <summary>
        /// add fixed parameters
        /// </summary>
        /// <param name="reportInfo">Report Info Model.</param>
        /// <param name="reportOutputType">report output type.</param>
        /// <param name="reportType">report type.</param>
        /// <param name="capID">it is CAP ID.</param>
        /// <returns>ReportInfoModel4WS model</returns>
        private static ReportInfoModel4WS AddFixedParams(ReportInfoModel4WS reportInfo, string reportOutputType, string reportType, CapIDModel4WS capID)
        {
            ReportInfoModel4WS fixedParams = reportInfo;

            List<string> fixParamKeys = new List<string>();
            List<string> fixParamValues = new List<string>();

            string fixedParamPrefix = FIXED_PARAMETER_PREFIX;

            if (!ACAConstant.REPORT_RTF_OUTPUT_TYPE.Equals(reportOutputType) && !ACAConstant.REPORT_URL_OUTPUT_TYPE.Equals(reportOutputType))
            {
                fixedParamPrefix = string.Empty;
            }

            string agencyCode = reportInfo.servProvCode;

            //get report by batch Transaction number , set custom id to -1.(reporting service need value of each paramter)
            string customID = ConstructCustomID(capID);

            if (string.IsNullOrEmpty(customID))
            {
                customID = REPORT_BLANK_PARAM;
            }

            ////get report by cap id, set batch Transaction number to -1.
            string batchTransactionNbr = HttpContext.Current.Request.QueryString["batchTransactionNbr"];

            if (string.IsNullOrEmpty(batchTransactionNbr))
            {
                batchTransactionNbr = REPORT_BLANK_PARAM;
            }
            else
            {
                customID = REPORT_BLANK_PARAM;
            }

            if (ACAConstant.PRINT_PAYMENT_RECEIPT_REPORT.Equals(reportType, StringComparison.InvariantCulture))
            {
                string receiptNbr = HttpContext.Current.Request.QueryString["recepitNbr"];

                if (!StandardChoiceUtil.IsEnableShoppingCart())
                {
                    fixParamKeys.Add(fixedParamPrefix + ACAConstant.REPORT_RECEIPT_NUMBER);
                    fixParamValues.Add(receiptNbr);
                }

                string[] parameterKeys = new string[] { fixedParamPrefix + ACAConstant.REPORT_AGENCY_ID, fixedParamPrefix + ACAConstant.REPORT_CAP_ID };

                fixParamKeys.AddRange(parameterKeys);

                fixParamValues.AddRange(new string[] { agencyCode, customID });
            }
            else if (ACAConstant.PRINT_PERMIT_REPORT.Equals(reportType, StringComparison.InvariantCulture) || ACAConstant.PRINT_PERMIT_SUMMARY_REPORT.Equals(reportType, StringComparison.InvariantCulture))
            {
                string[] parameterKeys = new string[] { fixedParamPrefix + ACAConstant.REPORT_AGENCY_ID, fixedParamPrefix + ACAConstant.REPORT_CAP_ID };

                fixParamKeys.AddRange(parameterKeys);

                fixParamValues.AddRange(new string[] { agencyCode, customID });
            }
            else if (ACAConstant.PRINT_LICENSE_REPORT.Equals(reportType, StringComparison.InvariantCulture))
            {
                string[] parameterKeys = new string[] { fixedParamPrefix + ACAConstant.REPORT_AGENCY_ID, fixedParamPrefix + ACAConstant.REPORT_CAP_ID };

                fixParamKeys.AddRange(parameterKeys);
                fixParamValues.AddRange(new string[] { agencyCode, customID });
            }
            else if (ACAConstant.PRINT_TRUST_ACCOUNT_RECEIPT_REPORT.Equals(reportType, StringComparison.InvariantCulture))
            {
                string receiptNbr = HttpContext.Current.Request.QueryString["recepitNbr"];

                string[] parameterKeys = new string[] { fixedParamPrefix + ACAConstant.REPORT_AGENCY_ID, fixedParamPrefix + ACAConstant.REPORT_RECEIPT_NUMBER };

                fixParamKeys.AddRange(parameterKeys);
                fixParamValues.AddRange(new string[] { agencyCode, receiptNbr });
            }

            if (StandardChoiceUtil.IsEnableShoppingCart())
            {
                fixParamKeys.Add(fixedParamPrefix + ACAConstant.REPORT_BATCHTRANSACTION_NBR);
                fixParamValues.Add(batchTransactionNbr);
            }

            fixedParams.reportParameterMapKeys = fixParamKeys.ToArray();
            fixedParams.reportParametersMapValues = fixParamValues.ToArray();

            return fixedParams;
        }

        /// <summary>
        /// Construct custom id.
        /// </summary>
        /// <param name="capID">it is CAP ID.</param>
        /// <returns>a CAPID string.</returns>
        private static string ConstructCustomID(CapIDModel4WS capID)
        {
            string customID = string.Empty;

            if (StandardChoiceUtil.IsEnableShoppingCart())
            {
                //Pass Cap id string to reporting service. use it to search cap in transaction table.
                customID = ConstructCapID(capID);
            }
            else
            {
                //in order to following old logic, Pass customID to reporting service,when disable shopping cart.
                customID = Convert.ToString(capID.customID);
            }

            return customID;
        }

        /// <summary>
        /// Constructs the cap id like "09CAP-00000-00032"
        /// </summary>
        /// <param name="capID">it is CAP ID.</param>
        /// <returns>a CAPID string.</returns>
        private static string ConstructCapID(CapIDModel4WS capID)
        {
            string customID = string.Empty;

            if (capID != null && !string.IsNullOrEmpty(capID.id1))
            {
                customID = string.Format("{0}{1}{2}{3}{4}", capID.id1, ACAConstant.SPLIT_CHAR4, capID.id2, ACAConstant.SPLIT_CHAR4, capID.id3);
            }

            return customID;
        }

        /// <summary>
        /// add selected caps' altIDs to parameter for permit list
        /// </summary>
        /// <param name="reportInfo">Report Info Model.</param>
        /// <returns>ReportInfoModel4WS model.</returns>
        private static ReportInfoModel4WS AddSelectedAltIDs(ReportInfoModel4WS reportInfo)
        {
            ReportInfoModel4WS altIDParam = reportInfo;

            if (!string.IsNullOrEmpty(AltIDs))
            {
                List<string> param = new List<string>();
                param.AddRange(altIDParam.reportParameterMapKeys);
                param.Add(ACAConstant.ID);
                altIDParam.reportParameterMapKeys = param.ToArray();

                param.Clear();

                param.AddRange(altIDParam.reportParametersMapValues);
                param.Add(AltIDs);
                altIDParam.reportParametersMapValues = param.ToArray();
            }

            return altIDParam;
        }

        /// <summary>
        /// add user-defined parameters to ReportInfoModel4WS 
        /// </summary>
        /// <param name="reportInfo">Report Info Model.</param>
        /// <param name="reportType">report port let</param>
        /// <returns>ReportInfoModel4WS model.</returns>
        private static ReportInfoModel4WS AddDynamicParams(ReportInfoModel4WS reportInfo, string reportType)
        {
            ReportInfoModel4WS dynamicParams = reportInfo;

            ParameterModel4WS[] parameters = AppSession.GetReportParameterFromSession();

            if (parameters != null && parameters.Length > 0)
            {
                string[] parameterKeys = new string[parameters.Length];
                string[] parameterValues = new string[parameters.Length];
                int index = 0;
                foreach (ParameterModel4WS parameter in parameters)
                {
                    parameterKeys[index] = parameter.parameterSource;
                    parameterValues[index] = parameter.dispDefaultValue;
                    ++index;
                }

                List<string> param = new List<string>();
                param.AddRange(dynamicParams.reportParameterMapKeys);
                param.AddRange(parameterKeys);
                dynamicParams.reportParameterMapKeys = param.ToArray();

                param.Clear();

                param.AddRange(dynamicParams.reportParametersMapValues);
                param.AddRange(parameterValues);
                dynamicParams.reportParametersMapValues = param.ToArray();

                AppSession.SetReportParameterToSession(null);
            }

            return dynamicParams;
        }

        /// <summary>
        /// add session variable parameters to ReportInfoModel4WS for RTF/URL reports.
        /// </summary>
        /// <param name="reportInfo">Report Info model.</param>
        /// <param name="moduleName">The model name</param>
        /// <returns>ReportInfoModel4WS model.</returns>
        private static ReportInfoModel4WS AddSessionVariables(ReportInfoModel4WS reportInfo, string moduleName)
        {
            ReportInfoModel4WS sessionParams = reportInfo;
            IReportBll reportBll = (IReportBll)ObjectFactory.GetObject(typeof(IReportBll));

            ////the capModel need be got from session ,only for RTF/URL report sessionVariable .
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

            var currentRequest = HttpContext.Current.Request;

            var collectionID = currentRequest.QueryString["collectionID"];
            collectionID = collectionID == null ? string.Empty : collectionID;

            var trustAccountReceiptID = currentRequest.QueryString["RecepitNbr"];
            trustAccountReceiptID = trustAccountReceiptID == null ? string.Empty : trustAccountReceiptID;

            var transactionID = currentRequest.QueryString["batchTransactionNbr"];
            transactionID = transactionID == null ? string.Empty : transactionID;

            Hashtable sessionVariable = new Hashtable();

            sessionVariable.Add(SessionVaraibleType.CapModel.ToString(), capModel);
            sessionVariable.Add(SessionVaraibleType.CollectionID.ToString(), collectionID);
            sessionVariable.Add(SessionVaraibleType.TrustAccountReceptID.ToString(), trustAccountReceiptID);
            sessionVariable.Add(SessionVaraibleType.TransactionID.ToString(), transactionID);

            sessionParams = reportBll.AddSessionVariableToParam(reportInfo, sessionVariable);

            return sessionParams;
        }

        /// <summary>
        /// Have Error?
        /// </summary>
        /// <param name="reportActionMessages"> ReportActionMessageModel4WS model.</param>
        /// <returns>True / False.</returns>
        private static bool IsErrorMessages(ReportActionMessageModel4WS[] reportActionMessages)
        {
            bool isError = false;

            if (reportActionMessages != null && reportActionMessages.Length > 0)
            {
                foreach (ReportActionMessageModel4WS model in reportActionMessages)
                {
                    if (model.action == ACAConstant.ACTION_REPORT_RUN && model.code == ACAConstant.CODE_ERROR)
                    {
                        Logger.Error(model.message);
                        isError = true;
                    }
                    else if (model.code == ACAConstant.CODE_ERROR)
                    {
                        Logger.Info(model.message);
                    }
                }
            }

            return isError;
        }

        /// <summary>
        /// Gets the report detail.
        /// </summary>
        /// <param name="reportType">Type of the report.</param>
        /// <param name="reportId">The report id.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="capId">The cap id.</param>
        /// <param name="reprintReasonId">The reprint reason id.</param>
        /// <returns>The report detail.</returns>
        private static string GetReportDetail(string reportType, string reportId, string moduleName, string capId, string reprintReasonId)
        {
            CapIDModel4WS subCapId = GetSubCapID();

            if (subCapId == null || string.IsNullOrEmpty(subCapId.customID))
            {
                ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
                subCapId = capBll.GetCapIDByAltID(ConfigManager.AgencyCode, capId);
            }

            IReportBll reportBll = ObjectFactory.GetObject<IReportBll>();

            string fileName = "sample" + Guid.NewGuid() + ".pdf";

            string emailReportName = GetEmailReportName();
            ReportInfoModel4WS reportInfoModel = GetReportInfoModel4WS(reportType, reportId, subCapId, moduleName, emailReportName);
            reportInfoModel.reprintReason = GetReprintReason();
            ReportResultModel4WS reportResultModel = reportBll.HandleReport(reportInfoModel, reportType);

            // generate report file
            if (IsValidReportResultModel(reportResultModel))
            {
                byte[] reportContent = reportResultModel.content;

                if (!Directory.Exists(ConfigManager.TempDirectory))
                {
                    Directory.CreateDirectory(ConfigManager.TempDirectory);
                }

                Stream streamWrite = new FileStream(ConfigManager.TempDirectory + "\\" + fileName, FileMode.OpenOrCreate);
                streamWrite.Write(reportContent, 0, reportContent.Length);
                streamWrite.Close();

                Logger.InfoFormat("The file {0} is stored successfully for {1}.", fileName, AppSession.User.PublicUserId);
            }
            else
            {
                Logger.ErrorFormat("The file cannot be opened for {0}, and the file {1} is not generated as well. Please double check the report configuration.", AppSession.User.PublicUserId, fileName);
            }

            return fileName;
        }

        #endregion
    }
}
