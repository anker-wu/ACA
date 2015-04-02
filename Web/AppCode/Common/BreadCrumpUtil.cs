#region Header

/*
 * <pre>
 *  Accela Citizen Access
 *  File: BreadCrumpUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  Breadcrumb util
 *  Notes:
 * $Id: BreadCrumpUtil.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// The utility is tool class to collect the current page information. 
    /// </summary>
    public class BreadCrumpUtil
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether current request come from shopping cart or not.
        /// </summary>
        private static bool IsFromShoppingCart
        {
            get
            {
                string isFromShoppingCart = HttpContext.Current.Request.QueryString[ACAConstant.FROMSHOPPINGCART];

                if (!string.IsNullOrEmpty(isFromShoppingCart) &&
                    isFromShoppingCart.Equals(ACAConstant.COMMON_Y))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get page flow's data for breadcrumb.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="hasFeeForm">has fee form or not.</param>
        /// <returns>Page flow information</returns>
        public static Dictionary<int, BreadCrumpInfo> GetPageFlowConfig(string moduleName, bool hasFeeForm)
        {
            return GetPageFlowConfig(moduleName, hasFeeForm, false);
        }

        /// <summary>
        /// Get page flow's data for breadcrumb.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="hasFeeForm">has fee form</param>
        /// <param name="hideFeeEstimate">hide fee estimate</param>
        /// <returns>Page flow information</returns>
        public static Dictionary<int, BreadCrumpInfo> GetPageFlowConfig(string moduleName, bool hasFeeForm, bool hideFeeEstimate)
        {
            int offsetIndex = 0;

            Dictionary<int, BreadCrumpInfo> stepList = new Dictionary<int, BreadCrumpInfo>();

            bool isFeeEstimator = false;
            int stepLen = 0;
            bool hasFee = false;
            int totalNumber = 1;
            string capName = string.Empty;
            bool isFirstLoad = false;
            CapModel4WS capModel = null;
            PageFrom pageFrom = PageFrom.Normal;
            BreadCrumbParmsInfo breadcrumbParmsInfo = AppSession.BreadcrumbParams;

            if (breadcrumbParmsInfo == null)
            {
                breadcrumbParmsInfo = new BreadCrumbParmsInfo();
            }

            capModel = AppSession.GetCapModelFromSession(moduleName);

            if (!string.IsNullOrEmpty(moduleName))
            {
                if (!string.IsNullOrEmpty(breadcrumbParmsInfo.CapName))
                {
                    capName = breadcrumbParmsInfo.CapName;
                }
                else
                {                    
                    capName = CAPHelper.GetAliasOrCapTypeLabel(capModel);
                }
            }

            isFirstLoad = CheckLoadStatus();

            if (moduleName != null)
            {
                if (ACAConstant.COMMON_YES.CompareTo(breadcrumbParmsInfo.HasFee) == 0)
                {
                    hasFee = true;
                }
                else
                {
                    if (capModel != null)
                    {
                        bool isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());
                        hasFee = CapUtil.HasFee(capModel.capID, CapUtil.IsSuperCAP(moduleName) || isAssoFormEnabled, isAssoFormEnabled);
                    }
                }

                breadcrumbParmsInfo.HasFee = hasFee ? ACAConstant.COMMON_YES : ACAConstant.COMMON_NO;
            }

            breadcrumbParmsInfo.IsExpendAllStep = StandardChoiceUtil.IsExpandBreadcrumbBar(ConfigManager.AgencyCode);

            // if it comes from pay fee due, needn't to show breadcrumb
            if (isFirstLoad)
            {
                pageFrom = GetPageFrom();
                AppSession.BreadcrumbParams = null;

                breadcrumbParmsInfo = new BreadCrumbParmsInfo();
                breadcrumbParmsInfo.PageFrom = pageFrom;

                if (pageFrom == PageFrom.PayFeeDue)
                {
                    AppSession.BreadcrumbParams = breadcrumbParmsInfo;
                    return stepList;
                }
            }

            if (HttpContext.Current.Request.QueryString["isFeeEstimator"] != null)
            {
                isFeeEstimator = HttpContext.Current.Request.QueryString["isFeeEstimator"] == ACAConstant.COMMON_Y;
                if (isFeeEstimator)
                {
                    breadcrumbParmsInfo.HasFeeEstimate = true;
                }
            }

            string orgUrl = HttpContext.Current.Request.RawUrl;
            BreadCrumpInfo info1 = new BreadCrumpInfo();

            if (orgUrl.IndexOf("CapType.aspx", StringComparison.InvariantCulture) > 0)
            {
                AppSession.BreadcrumbParams = null;
                AppSession.ShoppingCartBreadcrumbParams = null;
                breadcrumbParmsInfo = new BreadCrumbParmsInfo();
                breadcrumbParmsInfo.HasLicenseList = true;
                breadcrumbParmsInfo.LastIndex = 1;
                breadcrumbParmsInfo.HasFeeEstimate = isFeeEstimator;
                AppSession.BreadcrumbParams = breadcrumbParmsInfo;
                AppSession.SetPageflowGroupToSession(null);
                return stepList;
            }

            offsetIndex++;

            PageFlowGroupModel pageflowGroup = AppSession.GetPageflowGroupFromSession();

            if (pageflowGroup == null)
            {
                IPageflowBll pageflowBll = (IPageflowBll)ObjectFactory.GetObject(typeof(IPageflowBll));

                if (capModel != null && capModel.capType != null)
                {
                    pageflowGroup = pageflowBll.GetPageflowGroupByCapType(capModel.capType);
                    pageflowGroup = CapUtil.GetPageFlowWithoutBlankPage(capModel, pageflowGroup);
                }
            }

            if (pageflowGroup != null)
            {
                stepLen = pageflowGroup.stepList.Length;
            }

            if (hasFeeForm)
            {
                breadcrumbParmsInfo.HasFeeForm = true;
            }

            string isConvertToApp = HttpContext.Current.Request.QueryString["IsConvertToApp"];
            bool checkConvertToApp = false;

            if (!string.IsNullOrEmpty(isConvertToApp))
            {
                checkConvertToApp = isConvertToApp == ACAConstant.COMMON_Y ? true : false;
                if (checkConvertToApp)
                {
                    breadcrumbParmsInfo.IsConvertToApp = true;
                }
            }

            if (breadcrumbParmsInfo.HasFeeForm &&
                breadcrumbParmsInfo.IsConvertToApp == false)
            {
                BreadCrumpInfo feeFormInfo = new BreadCrumpInfo();
                feeFormInfo.Title = LabelUtil.GetTextByKey("per_breadcrumb_FeeForm", moduleName);
                feeFormInfo.CapName = capName;
                feeFormInfo.Url = GetUrl(totalNumber, offsetIndex, breadcrumbParmsInfo);
                feeFormInfo.StepIndex = totalNumber;
                feeFormInfo.StepIndexTitle = I18nNumberUtil.GetI18Number(totalNumber);
                totalNumber++;
                if (!stepList.ContainsKey(feeFormInfo.StepIndex))
                {
                    stepList.Add(feeFormInfo.StepIndex, feeFormInfo);
                }
            }
            else if (breadcrumbParmsInfo.HasFeeForm &&
                     breadcrumbParmsInfo.IsConvertToApp)
            {
                offsetIndex++;
            }

            //feeFormInfo
            if (breadcrumbParmsInfo.HasFeeEstimate &&
                breadcrumbParmsInfo.IsConvertToApp == false)
            {
                BreadCrumpInfo fee0Info = new BreadCrumpInfo();
                fee0Info.Title = LabelUtil.GetTextByKey("per_breadcrumb_FeeFormList", moduleName);
                fee0Info.CapName = capName;
                fee0Info.StepIndex = totalNumber;
                fee0Info.StepIndexTitle = I18nNumberUtil.GetI18Number(totalNumber);

                fee0Info.Url = GetUrl(totalNumber, offsetIndex, breadcrumbParmsInfo);
                totalNumber++;
                if (!stepList.ContainsKey(fee0Info.StepIndex))
                {
                    stepList.Add(fee0Info.StepIndex, fee0Info);
                }
            }
            else if (breadcrumbParmsInfo.HasFeeEstimate &&
                     breadcrumbParmsInfo.IsConvertToApp)
            {
                offsetIndex++;
            }

            if (hideFeeEstimate)
            {
                breadcrumbParmsInfo.IsHideApplicationForm = true;
                AppSession.BreadcrumbParams = breadcrumbParmsInfo;
                return stepList;
            }

            for (int i = 0; i < stepLen; i++)
            {
                BreadCrumpInfo info = new BreadCrumpInfo();
                info.Title = I18nStringUtil.GetString(pageflowGroup.stepList[i].resStepName, pageflowGroup.stepList[i].stepName);
                info.CapName = capName;
                info.StepIndex = totalNumber;
                info.StepIndexTitle = I18nNumberUtil.GetI18Number(totalNumber);

                if (info.PageInstructions == null)
                {
                    info.PageInstructions = new List<string>();
                }

                for (int j = 0; j < pageflowGroup.stepList[i].pageList.Length; j++)
                {
                    PageModel pageModel = pageflowGroup.stepList[i].pageList[j];
                    string pageTitle = I18nStringUtil.GetString(pageModel.resPageName, pageModel.pageName);
                    info.Pagetitle += pageTitle + "|";

                    string pageInstruction = I18nStringUtil.GetCurrentLanguageString(pageModel.resInstruction, pageModel.instruction);
                    info.PageInstructions.Add(pageInstruction);
                }

                info.Url = GetUrl(totalNumber, offsetIndex, breadcrumbParmsInfo);
                totalNumber++;

                if (!stepList.ContainsKey(info.StepIndex))
                {
                    stepList.Add(info.StepIndex, info);
                }
            }

            if (StandardChoiceUtil.IsSuperAgency())
            {
                breadcrumbParmsInfo.IsSuperAgency = true;
            }

            BreadCrumpInfo confirmInfo = new BreadCrumpInfo();
            confirmInfo.Title = LabelUtil.GetTextByKey("per_breadcrumb_ReviewInformation", moduleName);
            confirmInfo.CapName = capName;
            confirmInfo.StepIndex = totalNumber;
            confirmInfo.StepIndexTitle = I18nNumberUtil.GetI18Number(totalNumber);
            confirmInfo.Url = GetUrl(totalNumber, offsetIndex, breadcrumbParmsInfo);

            if (StandardChoiceUtil.IsEnableShoppingCart() && IsFromShoppingCart)
            {
                if (!string.IsNullOrEmpty(confirmInfo.Url))
                {
                    breadcrumbParmsInfo.IsConfirmPageFromShoppingCart = true;
                }
            }

            totalNumber++;

            if (!stepList.ContainsKey(confirmInfo.StepIndex))
            {
                stepList.Add(confirmInfo.StepIndex, confirmInfo);
            }

            //Associated Forms
            bool isAssoFormChild = false;
            if (CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession()))
            {
                // In super agency and Associated Form, if parent cap is super cap, need to disable previous steps.
                if (HttpContext.Current.Request.Url.AbsolutePath.EndsWith("AssociatedForms.aspx", StringComparison.OrdinalIgnoreCase)
                    && CapUtil.IsSuperCAP(capModel))
                {
                    for (int i = 1; i <= stepList.Count; i++)
                    {
                        BreadCrumpInfo crumbInfo = stepList[i] as BreadCrumpInfo;
                        if (crumbInfo != null)
                        {
                            crumbInfo.Enable = false;
                        }
                    }
                }

                BreadCrumpInfo assoFormInfo = new BreadCrumpInfo();
                assoFormInfo.CapName = capName;
                assoFormInfo.StepIndex = totalNumber;
                assoFormInfo.StepIndexTitle = I18nNumberUtil.GetI18Number(totalNumber);
                assoFormInfo.Title = LabelUtil.GetTextByKey("aca_associated_forms", moduleName);
                assoFormInfo.Url = GetUrl(totalNumber, offsetIndex, breadcrumbParmsInfo);

                totalNumber++;

                if (!stepList.ContainsKey(assoFormInfo.StepIndex))
                {
                    stepList.Add(assoFormInfo.StepIndex, assoFormInfo);
                }

                isAssoFormChild = CapUtil.IsAssoFormChild(moduleName);
            }

            if (!isAssoFormChild)
            {
                if (hasFee && capModel != null)
                {
                    BreadCrumpInfo feeInfo = new BreadCrumpInfo();
                    feeInfo.Title = LabelUtil.GetTextByKey("per_breadcrumb_PayFees", moduleName);
                    feeInfo.CapName = capName;
                    feeInfo.StepIndex = totalNumber;
                    feeInfo.StepIndexTitle = I18nNumberUtil.GetI18Number(totalNumber);
                    feeInfo.Url = GetUrl(totalNumber, offsetIndex, breadcrumbParmsInfo);

                    if (!string.IsNullOrEmpty(feeInfo.Url))
                    {
                        breadcrumbParmsInfo.IsCapFeePage = true;
                    }

                    totalNumber++;

                    if (!stepList.ContainsKey(feeInfo.StepIndex))
                    {
                        stepList.Add(feeInfo.StepIndex, feeInfo);
                    }
                }

                BreadCrumpInfo receiptInfo = new BreadCrumpInfo();

                string labelKeyForLastStep = GetLabelKeyForLastStep(breadcrumbParmsInfo);
                receiptInfo.Title = LabelUtil.GetTextByKey(labelKeyForLastStep, moduleName);
                receiptInfo.CapName = capName;
                receiptInfo.StepIndex = totalNumber;
                receiptInfo.StepIndexTitle = I18nNumberUtil.GetI18Number(totalNumber);
                receiptInfo.Url = GetUrl(totalNumber, offsetIndex, breadcrumbParmsInfo);

                if (!stepList.ContainsKey(receiptInfo.StepIndex))
                {
                    stepList.Add(receiptInfo.StepIndex, receiptInfo);
                }
            }

            AppSession.BreadcrumbParams = breadcrumbParmsInfo;
            return stepList;
        }

        /// <summary>
        /// Get ShoppingCart page flow data.
        /// </summary>
        /// <returns>page flow information.</returns>
        public static Dictionary<int, BreadCrumpInfo> GetShoppingCartFlowConfig()
        {
            Dictionary<int, BreadCrumpInfo> stepList = new Dictionary<int, BreadCrumpInfo>();

            BreadCrumbParmsInfo breadcrumbParmsInfo = AppSession.ShoppingCartBreadcrumbParams;

            for (int i = 1; i <= CreateDataSource().Count; i++)
            {
                if (breadcrumbParmsInfo == null)
                {
                    breadcrumbParmsInfo = new BreadCrumbParmsInfo();
                }

                BreadCrumpInfo receiptInfo = new BreadCrumpInfo();

                receiptInfo.Title = GetLabelKeyByIndex(i);

                receiptInfo.StepIndex = i;
                receiptInfo.StepIndexTitle = I18nNumberUtil.GetI18Number(i);

                receiptInfo.Url = GetShoppingCartUrl(i, breadcrumbParmsInfo);

                if (!stepList.ContainsKey(receiptInfo.StepIndex))
                {
                    stepList.Add(receiptInfo.StepIndex, receiptInfo);
                }
            }

            AppSession.ShoppingCartBreadcrumbParams = breadcrumbParmsInfo;

            return stepList;
        }

        /// <summary>
        /// Rebuild breadcrumb url when the page come from shopping cart or come from AssociatedForms child Cap.
        /// </summary>
        /// <param name="pageflowGroup">a PageFlowGroupModel</param>
        /// <param name="moduleName">The module name</param>
        /// <param name="includeViewStep">Whether rebuild the url for CapConfirm step</param>
        public static void RebuildBreadCrumb(PageFlowGroupModel pageflowGroup, string moduleName, bool includeViewStep)
        {
            StringBuilder url = new StringBuilder();
            url.Append(FileUtil.ApplicationRoot + "Cap/CapEdit.aspx?");
            AddParameter2URL(url);

            BreadCrumbParmsInfo tempBreadCrumb = new BreadCrumbParmsInfo();

            for (int i = 2; i <= pageflowGroup.stepList.Length + 1; i++)
            {
                //stepNumber in Cap edit page begin from 2.
                string currentStep = i == 2 ? string.Empty : (i - 2).ToString();

                tempBreadCrumb.Urls.Add(i, string.Format(url.ToString(), moduleName, i, currentStep));
            }

            if (includeViewStep)
            {
                url = new StringBuilder(FileUtil.ApplicationRoot + "Cap/CapConfirm.aspx?");
                AddParameter2URL(url);
                int stepNumber = pageflowGroup.stepList.Length + 2;
                tempBreadCrumb.Urls.Add(stepNumber, string.Format(url.ToString(), moduleName, stepNumber, pageflowGroup.stepList.Length));
            }

            //Reset the Cap fee breadcrumb.
            bool isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());
            bool isAssoFormChild = false;
            int stepNumberInCapFee = pageflowGroup.stepList.Length + 3;
            if (isAssoFormEnabled)
            {
                stepNumberInCapFee = pageflowGroup.stepList.Length + 4;
                isAssoFormChild = CapUtil.IsAssoFormChild(moduleName);
            }

            //The Child of the Associated Forms has not include the CapFee step.
            if (!isAssoFormEnabled && AppSession.BreadcrumbParams != null && AppSession.BreadcrumbParams.Urls.ContainsKey(stepNumberInCapFee))
            {
                tempBreadCrumb.Urls[stepNumberInCapFee] = AppSession.BreadcrumbParams.Urls[stepNumberInCapFee];
                tempBreadCrumb.LastIndex = AppSession.BreadcrumbParams.LastIndex;
            }

            AppSession.BreadcrumbParams = tempBreadCrumb;
        }

        /// <summary>
        /// add parameters to url.
        /// </summary>
        /// <param name="url">StringBuilder  for url</param>
        private static void AddParameter2URL(StringBuilder url)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("Module", "{0}");
            parameters.Add("stepNumber", "{1}");
            parameters.Add("currentStep", "{2}");
            parameters.Add("pageNumber", "1");
            parameters.Add("currentPage", "0");

            string[] requestParameter = HttpContext.Current.Request.QueryString.AllKeys;

            if (!string.IsNullOrEmpty(requestParameter.ToString()))
            {
                for (int i = 0; i < requestParameter.Length; i++)
                {
                    if (!parameters.Contains(requestParameter[i]))
                    {
                        parameters.Add(requestParameter[i], HttpContext.Current.Request.QueryString[requestParameter[i]]);
                    }
                }
            }

            foreach (string key in parameters.Keys)
            {
                url.Append(key);
                url.Append("=");
                url.Append(parameters[key]);
                url.Append("&");
            }

            //remove the last char '&'.
            url.Remove(url.Length - 1, 1);
        }

        /// <summary>
        /// check if reload breadcrumb, 
        /// true - need to clear breadcrumb session,
        /// false - needn't to change anything.
        /// </summary>
        /// <returns>indication current page status.</returns>
        private static bool CheckLoadStatus()
        {
            string orgUrl = HttpContext.Current.Request.RawUrl;
            bool chk = false;

            // the first page is license selection
            if (orgUrl.IndexOf("UserLicenseList.aspx", StringComparison.InvariantCulture) > 0)
            {
                chk = true;
            }

            // the first page is CapType picker if current user has not the license.
            if (orgUrl.IndexOf("CapType.aspx", StringComparison.InvariantCulture) > 0)
            {
                chk = true;
            }

            // Indicates if it is coming from permit list.
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["permitType"]) &&
                string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["firstLoad"]))
            {
                chk = true;
            }

            return chk;
        }

        /// <summary>
        /// create the steps name list.
        /// </summary>
        /// <returns>Hashtable steps name list.</returns>
        private static Hashtable CreateDataSource()
        {
            Hashtable items = new Hashtable();

            items.Add(1, BasePage.GetStaticTextByKey("shoppingcart_breadcrumb_selectitem_pay"));
            items.Add(2, BasePage.GetStaticTextByKey("shoppingcart_breadcrumb_payment_information"));
            items.Add(3, BasePage.GetStaticTextByKey("shoppingcart_breadcrumb_Receipt_issuance"));

            return items;
        }

        /// <summary>
        /// get the step's name according to the index.
        /// </summary>
        /// <param name="index">index number.</param>
        /// <returns>step's name.</returns>
        private static string GetLabelKeyByIndex(int index)
        {
            Hashtable labelKeys = CreateDataSource();

            if (labelKeys != null && labelKeys[index] != null && labelKeys.Contains(index) &&
                !string.IsNullOrEmpty(labelKeys[index].ToString()))
            {
                return labelKeys[index].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// the title of last step depends on user's operation
        /// </summary>
        /// <param name="breadcrumbParmsInfo">breadcrumb Parameter Information.</param>
        /// <returns>the last step breadcrumb name.</returns>
        private static string GetLabelKeyForLastStep(BreadCrumbParmsInfo breadcrumbParmsInfo)
        {
            string labelKeyForLastStep = "per_breadcrumb_PermitIssuance";

            if (breadcrumbParmsInfo == null)
            {
                return labelKeyForLastStep;
            }

            string filterName = HttpContext.Current.Request.QueryString["filterName"];

            if (!string.IsNullOrEmpty(filterName))
            {
                switch (filterName)
                {
                    case ACAConstant.REQUEST_PARMETER_TRADE_LICENSE:
                        labelKeyForLastStep = "aca_tradelicense_issuance";
                        break;
                    case ACAConstant.REQUEST_PARMETER_TRADE_NAME:
                        labelKeyForLastStep = "aca_tradename_issuance";
                        break;
                }
            }

            if (string.IsNullOrEmpty(breadcrumbParmsInfo.LabelKeyForLastStep))
            {
                breadcrumbParmsInfo.LabelKeyForLastStep = labelKeyForLastStep;
            }

            return breadcrumbParmsInfo.LabelKeyForLastStep;
        }

        /// <summary>
        /// get permitType
        /// </summary>
        /// <returns>Page comes from</returns>
        private static PageFrom GetPageFrom()
        {
            string pageFrom = HttpContext.Current.Request.QueryString["permitType"];

            if (string.IsNullOrEmpty(pageFrom))
            {
                return PageFrom.Normal;
            }

            if (pageFrom == "PayFees")
            {
                return PageFrom.PayFeeDue;
            }

            return PageFrom.Normal;
        }

        /// <summary>
        /// Get ShoppingCart breadcrumb Url.
        /// </summary>
        /// <param name="stepNumber">step number</param>
        /// <param name="breadcrumbParmsInfo">breadcrumb Parameter Information.</param>
        /// <returns>shopping cart page url</returns>
        private static string GetShoppingCartUrl(int stepNumber, BreadCrumbParmsInfo breadcrumbParmsInfo)
        {
            int curStepNumber = 1;

            string step = HttpContext.Current.Request.QueryString["stepNumber"];

            if (ValidationUtil.IsInt(step))
            {
                curStepNumber = int.Parse(step);
            }

            string orgUrl = HttpContext.Current.Request.RawUrl;

            if (AppSession.ShoppingCartBreadcrumbParams == null)
            {
                breadcrumbParmsInfo = new BreadCrumbParmsInfo();
            }

            if (!breadcrumbParmsInfo.Urls.ContainsKey(curStepNumber - 1))
            {
                breadcrumbParmsInfo.Urls[curStepNumber - 1] = orgUrl;
                breadcrumbParmsInfo.LastIndex = curStepNumber - 1;

                AppSession.ShoppingCartBreadcrumbParams = breadcrumbParmsInfo;
            }

            if (breadcrumbParmsInfo.Urls.ContainsKey(stepNumber))
            {
                return breadcrumbParmsInfo.Urls[stepNumber].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get current page Url.
        /// </summary>
        /// <param name="stepNumber">step number of page flow</param>
        /// <param name="offsetIndex">the number to be reduced.</param>
        /// <param name="breadcrumbParmsInfo">breadcrumb Parameter Information that store in session.</param>
        /// <returns>url string.</returns>
        private static string GetUrl(int stepNumber, int offsetIndex, BreadCrumbParmsInfo breadcrumbParmsInfo)
        {
            int curStepNumber = 1;

            object step = HttpContext.Current.Request.QueryString["stepNumber"];
            if (step != null &&
                ValidationUtil.IsInt(step.ToString()))
            {
                curStepNumber = int.Parse(step.ToString());
            }

            string orgUrl = HttpContext.Current.Request.RawUrl;

            //if page from detail list and first load,add a flag so when click the first step,we don't need to clear the session
            string pageFrom = HttpContext.Current.Request.QueryString["permitType"];
            if (!string.IsNullOrEmpty(pageFrom) &&
                pageFrom != "PayFees")
            {
                orgUrl += "&firstLoad=false";
            }

            if (AppSession.BreadcrumbParams == null)
            {
                breadcrumbParmsInfo = new BreadCrumbParmsInfo();
            }

            if (!breadcrumbParmsInfo.Urls.ContainsKey(curStepNumber))
            {
                breadcrumbParmsInfo.Urls[curStepNumber] = orgUrl;
                AppSession.BreadcrumbParams = breadcrumbParmsInfo;
            }

            if (breadcrumbParmsInfo.Urls.ContainsKey(stepNumber + offsetIndex))
            {
                return breadcrumbParmsInfo.Urls[stepNumber + offsetIndex].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Methods
    }
}