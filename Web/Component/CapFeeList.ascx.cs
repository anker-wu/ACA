#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapFeeList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: FeeList.ascx.cs 202488 2011-08-26 09:03:36Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Finance;
using Accela.ACA.BLL.Report;
using Accela.ACA.BLL.ShoppingCart;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation Fee list. 
    /// </summary>
    public partial class FeeList : BaseUserControl
    {
        /// <summary>
        /// true: support decimal, this is a default value
        /// </summary>
        private int _feeQuantityAccuracy = 2;

        /// <summary>
        /// All controls of joining expression to calculate 
        /// </summary>
        private Dictionary<string, WebControl> _expressionControls = new Dictionary<string, WebControl>();

        /// <summary>
        /// cap model collection. That is used in expression only.
        /// </summary>
        private Dictionary<string, CapModel4WS> _subCapModels = new Dictionary<string, CapModel4WS>();

        /// <summary>
        /// fee item collection. It is used in expression only.
        /// </summary>
        private Dictionary<string, List<F4FeeItemModel4WS>> _feeItems = new Dictionary<string, List<F4FeeItemModel4WS>>();

        /// <summary>
        /// all fee items.
        /// </summary>
        private List<F4FeeItemModel4WS> allFeeItems = new List<F4FeeItemModel4WS>();

        /// <summary>
        /// Gets or sets the fee quantity decimal length
        /// </summary>
        public int FeeQuantityAccuracy
        {
            get
            {
                return _feeQuantityAccuracy;
            }

            set
            {
                _feeQuantityAccuracy = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to enable the fee section expanded/collapsed by total fee.
        /// </summary>
        public bool IsReviewPage
        {
            get
            {
                if (this.ViewState["IsReviewPage"] == null)
                {
                    return false;
                }

                return (bool)this.ViewState["IsReviewPage"];
            }

            set
            {
                this.ViewState["IsReviewPage"] = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is Pay Fee Due process.
        /// </summary>
        protected bool IsPayFeeDue
        {
            get
            {
                return ValidationUtil.IsYes(Request.QueryString[ACAConstant.REQUEST_PARMETER_ISPAYFEEDUE]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether request come from shopping cart or not.
        /// </summary>
        private bool IsFromShoppingCart
        {
            get
            {
                return ValidationUtil.IsYes(Request.QueryString[ACAConstant.FROMSHOPPINGCART]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether is Renewal process.
        /// </summary>
        private bool IsRenewal
        {
            get
            {
                return ValidationUtil.IsYes(Request.QueryString[ACAConstant.REQUEST_PARMETER_ISRENEWAL]);
            }
        }

        /// <summary>
        /// Gets a string of Renewal flag as parameters indicate whether is Renewal process.
        /// </summary>
        private string RenewalFlag
        {
            get
            {
                return IsRenewal ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            }
        }

        /// <summary>
        /// Gets a string of Pay Fee Due flag as parameters to indicate whether is Pay Fee Due process.
        /// </summary>
        private string PayFeeDueFlag
        {
            get
            {
                return IsPayFeeDue ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            }
        }

        #region Methods

        /// <summary>
        /// Show all of fee information to UI
        /// </summary>
        public void ShowFeeInformation()
        {
            BindAgencies();
            ShowFeeTotal();
        }

        /// <summary>
        /// Show Negative  Fee Message.
        /// </summary>
        public void ShowNegativeFeeMessage()
        {
            MessageUtil.ShowMessage(Page, MessageType.Notice, GetTextByKey("per_permitFee_message_havenegativefee"));

            //need to show fee items to UI again.
            ShowFeeInformation();
        }

        /// <summary>
        /// Is all Cap Total Fee Positive
        /// </summary>
        /// <returns>true or false.</returns>
        public bool IsAllCapTotalFeePositive()
        {
            allFeeItems = GetFeeItems();
            IFeeBll feeBll = (IFeeBll)ObjectFactory.GetObject(typeof(IFeeBll));
            foreach (DataListItem itemAgence in agenceList.Items)
            {
                HiddenField hdnAgenceName = (HiddenField)itemAgence.FindControl("hdnAgenceName");
                List<F4FeeItemModel4WS> capS = feeBll.GetChildCapsByAgency(allFeeItems, hdnAgenceName.Value);

                if (capS != null && capS.Count > 0)
                {
                    foreach (F4FeeItemModel4WS feeItemCap in capS)
                    {
                        double capTotalFee = feeBll.GetChildCapTotalFee(allFeeItems, feeItemCap.capID);

                        if (capTotalFee < 0)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        
        /// <summary>
        /// Do recalculate
        /// </summary>
        /// <returns>return true if recalculate success</returns>
        public bool DoRecalculate()
        {
            int itemsCount = 0;
            allFeeItems = GetFeeItems();
            IFeeBll feeBll = (IFeeBll)ObjectFactory.GetObject(typeof(IFeeBll));

            foreach (DataListItem singleAgency in agenceList.Items)
            {
                HiddenField hdnAgenceName = (HiddenField)singleAgency.FindControl("hdnAgenceName");
                List<F4FeeItemModel4WS> capS = feeBll.GetChildCapsByAgency(allFeeItems, hdnAgenceName.Value);

                itemsCount += capS.Count;
            }

            if (itemsCount == 0)
            {
                return true;
            }

            RecalculateParameterModel4WS[] recalculateParameters = new RecalculateParameterModel4WS[itemsCount];

            double jobValue = GetJobValue();

            try
            {
                int iIndex = 0;
                foreach (DataListItem itemAgence in agenceList.Items)
                {
                    AccelaGridView feeItemsList = (AccelaGridView)itemAgence.FindControl("feeItemList");
                    RecalculateParameterModel4WS childCapFeeItems = new RecalculateParameterModel4WS();
                    int itemCount = feeItemsList.Rows.Count;
                    int index = 0;

                    if (itemCount == 0)
                    {
                        return true;
                    }

                    long?[] feeSeqs = new long?[itemCount];
                    double?[] feeqty = new double?[itemCount];
                    string[] feeStatus = new string[itemCount];

                    foreach (GridViewRow row in feeItemsList.Rows)
                    {
                        HiddenField hdnFeeSeq = (HiddenField)row.FindControl("hdnFeeSeq");
                        HiddenField hdnStatus = (HiddenField)row.FindControl("hdnStatus");
                        HiddenField hdnReadOnly = (HiddenField)row.FindControl("hdnReadOnly");
                        CapIDModel4WS currentCapID = GetChildCapID(row);

                        feeSeqs[index] = long.Parse(hdnFeeSeq.Value);
                        feeStatus[index] = hdnStatus.Value;

                        string quantity;

                        // if the fee is set to read-only,get quantity from hidden control,
                        // if not get quantity from textbox control.
                        if (hdnReadOnly.Value != null && hdnReadOnly.Value.Equals(ACAConstant.COMMON_Y, StringComparison.InvariantCulture))
                        {
                            HiddenField hidQuantity = (HiddenField)row.FindControl("hidQuantity");
                            quantity = hidQuantity.Value;
                        }
                        else
                        {
                            AccelaNumberText txtQuantity = (AccelaNumberText)row.FindControl("txtQuanity");
                            quantity = txtQuantity.Text;
                        }

                        if (string.IsNullOrEmpty(quantity))
                        {
                            quantity = "0";
                        }

                        feeqty[index] = GetValidQuantity(quantity);
                        index++;

                        GridViewRow nextRow = row.RowIndex < itemCount - 1 ? feeItemsList.Rows[row.RowIndex + 1] : null;
                        CapIDModel4WS nextCapID = GetChildCapID(nextRow);

                        if (currentCapID != null
                            && nextCapID != null
                            && (!string.IsNullOrEmpty(currentCapID.id1) || !string.IsNullOrEmpty(currentCapID.id2) || !string.IsNullOrEmpty(currentCapID.id3)))
                        {
                            if (currentCapID.id1 != nextCapID.id1 || currentCapID.id2 != nextCapID.id2 || currentCapID.id3 != nextCapID.id3)
                            {
                                childCapFeeItems.callID = AppSession.User.PublicUserId;
                                childCapFeeItems.feeIDs = feeSeqs;
                                childCapFeeItems.quantities = feeqty;
                                childCapFeeItems.statuses = feeStatus;
                                childCapFeeItems.capID = currentCapID;
                                childCapFeeItems.feeFactory = jobValue.ToString();
                                recalculateParameters[iIndex] = ObjectCloneUtil.DeepCopy(childCapFeeItems);
                                index = 0;
                                iIndex++;
                                feeSeqs = new long?[itemCount];
                                feeqty = new double?[itemCount];
                                feeStatus = new string[itemCount];
                            }
                        }
                    }
                }
            }
            catch (ACAException ex)
            {
                string message = ex.Message;

                if (string.IsNullOrEmpty(message))
                {
                    ShowQuantityInvalidateMessage();
                }
                else
                {
                    MessageUtil.ShowMessage(Page, MessageType.Error, message);
                }

                return false;
            }

            try
            {
                feeBll.DoRecalculates(recalculateParameters);
            }
            catch (Exception e)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, e.Message);
            }

            return true;
        }

        /// <summary>
        /// Add to shopping cart.
        /// </summary>
        public void AddToShoppingCart()
        {
            bool isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());

            if (!IsFromShoppingCart || isAssoFormEnabled)
            {
                IShoppingCartBll shoppingCartBll = (IShoppingCartBll)ObjectFactory.GetObject(typeof(IShoppingCartBll));
                CapIDModel4WS capIDModel = GetCapIDModel();

                ShoppingCartItemModel4WS cartItem = new ShoppingCartItemModel4WS();
                cartItem.capID = capIDModel;

                //if it is Renew Cap,add parent cap to shopping cart.
                if (IsRenewal && IsPayFeeDue)
                {
                    cartItem.processType = ACAConstant.PAYFEEDUE_RENEWAL;
                }
                else if (IsRenewal)
                {
                    //It's for normal renewal process & pay fee due for renewal process
                    cartItem.processType = ACAConstant.CAP_RENEWAL;
                }
                else if (IsPayFeeDue)
                {
                    //It's just for normal pay fee due process.
                    cartItem.processType = ACAConstant.CAP_PAYFEEDUE;
                }

                ShoppingCartItemModel4WS[] cartItems = { cartItem };
                bool isNormalAssoForm = CapUtil.GetAssoFormType(isAssoFormEnabled, capIDModel) == ACAConstant.AssoFormType.Normal;
                bool isMultiCap = CapUtil.IsSuperCAP(ModuleName) || (isNormalAssoForm && !IsPayFeeDue);
                shoppingCartBll.CreateShoppingCart(cartItems, isMultiCap, isNormalAssoForm);
            }

            ShoppingCartUtil.SetCartItemNumber();
        }

        /// <summary>
        /// Display print requirements button.
        /// </summary>
        public void DisplayReceiptButton()
        {
            btnPrintRequirements.Visible = false;

            if (AppSession.IsAdmin)
            {
                btnPrintRequirements.Visible = true;
                return;
            }

            ReportButtonPropertyModel4WS[] arrayRBTModel = GetReportButtonProperty();

            if (arrayRBTModel != null && arrayRBTModel.Length > 0)
            {
                foreach (ReportButtonPropertyModel4WS ws in arrayRBTModel)
                {
                    //if (ACAConstant.PRINT_REQUIREMENTS_REPORT.Equals(ws.buttonName))
                    if (ACAConstant.PRINT_REQUIREMENTS_REPORT.Equals(ws.buttonName, StringComparison.InvariantCulture) && ws.isDisplayed)
                    {
                        string reportID = ws.reportId;

                        if (string.IsNullOrEmpty(ws.errorInfo))
                        {
                            btnPrintRequirements.ToolTip = I18nStringUtil.GetString(ws.resReportName, ws.reportName);
                            btnPrintRequirements.ReportID = reportID;
                            string receiptReportUrl = string.Empty;
                            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                            CapIDModel4WS capID = capModel == null ? null : capModel.capID;

                            if (StandardChoiceUtil.IsSuperAgency() && capID != null && !ConfigManager.AgencyCode.Equals(capID.serviceProviderCode, StringComparison.InvariantCulture))
                            {
                                receiptReportUrl = string.Format(
                                    ACAConstant.REPORT_PARAMETER_PAGE + "?Module={0}&reportType={1}&reportID={2}&subID1={3}&subID2={4}&subID3={5}&{6}={7}&subCustomerID={8}&SubModule={9}",
                                    ScriptFilter.AntiXssUrlEncode(ModuleName),
                                    ACAConstant.PRINT_REQUIREMENTS_REPORT,
                                    reportID,
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
                                receiptReportUrl = string.Format(ACAConstant.REPORT_PARAMETER_PAGE + "?Module={0}&reportType={1}&reportID={2}", ScriptFilter.AntiXssUrlEncode(ModuleName), ACAConstant.PRINT_REQUIREMENTS_REPORT, reportID);

                                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[UrlConstant.AgencyCode]))
                                {
                                    receiptReportUrl += "&" + UrlConstant.AgencyCode + "=" + HttpContext.Current.Request.QueryString[UrlConstant.AgencyCode];
                                }
                            }

                            btnPrintRequirements.Attributes.Add("href", "javascript:print_onclick('" + receiptReportUrl + "')");
                            btnPrintRequirements.Visible = true;
                        }

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Displays the conditions.
        /// </summary>
        public void DisplayConditions()
        {
            if (allFeeItems == null || allFeeItems.Count == 0)
            {
                return;
            }

            IFeeBll feeBll = (IFeeBll)ObjectFactory.GetObject(typeof(IFeeBll));
            ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();
            Dictionary<string, CapTypeModel> dictRecordType = new Dictionary<string, CapTypeModel>();

            // get the record type from the fee item
            foreach (F4FeeItemModel4WS feeItem in allFeeItems)
            {
                string currentAgenceCode = feeItem.capID.serviceProviderCode;
                List<F4FeeItemModel4WS> childCaps = feeBll.GetChildCapsByAgency(allFeeItems, currentAgenceCode);

                foreach (F4FeeItemModel4WS cap in childCaps)
                {
                    string capIDKey = string.Format("{0}_{1}_{2}_{3}", cap.capID.serviceProviderCode, cap.capID.id1, cap.capID.id2, cap.capID.id3);
                    if (!dictRecordType.ContainsKey(capIDKey))
                    {
                        CapTypeModel capTypeModel = capTypeBll.GetCapTypeByCapID(cap.capID);
                        dictRecordType.Add(capIDKey, capTypeModel);
                    }
                }
            }

            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
            Dictionary<CapTypeModel, NoticeConditionModel[]> dictCondition = new Dictionary<CapTypeModel, NoticeConditionModel[]>();

            // get the conditions by the record type
            foreach (CapTypeModel capType in dictRecordType.Values)
            {
                NoticeConditionModel[] conditions = capBll.GetStdConditionByRecordType(capType, false);

                dictCondition.Add(capType, conditions);
            }

            // merge the conditions and fee conditions
            Dictionary<CapTypeModel, NoticeConditionModel[]> dictConditionEmse = GetFeeConditionFromEMSE();
            dictCondition = MergeConditions(dictCondition, dictConditionEmse);

            bool isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());
            if (CapUtil.IsSuperCAP(ModuleName) || isAssoFormEnabled)
            {
                capConditions.IsGroupByRecordType = true;
            }

            bool display = capConditions.Display(dictCondition);

            divConditionInstruction.Visible = display;
        }

        /// <summary>
        /// Continue to edit application.
        /// </summary>
        public void PayAtCounter()
        {
            AppSession.SetOnlinePaymentResultModelToSession(null);

            // get the step number for parameter.
            string preStepNumber = Request.QueryString["stepNumber"];
            
            // get the record is amendment
            string amendment = Request.QueryString["isAmendment"];

            int stepNumber = 0;

            if (ValidationUtil.IsNumber(preStepNumber))
            {
                stepNumber = short.Parse(preStepNumber) + 1;

                // if it comes from review page's defer payment,it has fee and it should skip pay fee step to Record Issuance.
                if (IsReviewPage)
                {
                    stepNumber++;
                }
            }

            string isSubAgencyCap = Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP];

            string url = string.Format("CapCompletion.aspx?Module={0}&stepNumber={1}&isRenewal={2}&isDeferredPayment=Y&{3}={4}", ModuleName, stepNumber, RenewalFlag, ACAConstant.IS_SUBAGENCY_CAP, isSubAgencyCap);

            //Pay Fee Due no need create a new cap id, redirect to Permit Issuance page directly.
            if (IsPayFeeDue)
            {
                Response.Redirect(url);
            }

            //add all fees to partial cap and recalculate them.
            if (!DoRecalculate())
            {
                return;
            }

            string wotModelSeq = string.Empty;
            CapModel4WS regularCapModel;

            try
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                SysUserModel4WS sysUser = new SysUserModel4WS();
                sysUser.userID = AppSession.User.PublicUserId;
                sysUser.firstName = AppSession.User.FirstName;
                sysUser.lastName = AppSession.User.LastName;
                capModel.sysUser = sysUser;

                bool isAmendment = ValidationUtil.IsYes(amendment);
                ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                regularCapModel = capBll.CreateCapModelFromFeeEstimate(ConfigManager.AgencyCode, capModel, AppSession.User.PublicUserId, wotModelSeq, true, isAmendment);
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
                return;
            }

            // update the cap model to session with fee information
            AppSession.SetCapModelToSession(ModuleName, regularCapModel);

            // redirect it to successful page.
            Response.Redirect(url);
        }

        /// <summary>
        /// validate all text box, true: all text box field value more than zero, false: some text box value less than or equal zero. 
        /// </summary>
        /// <returns>true: all text box field value more than zero, false: some text box value less than or equal zero. </returns>
        public bool ValidateInputValue()
        {
            //do each agency.
            if (agenceList != null && agenceList.Items != null && agenceList.Items.Count > 0)
            {
                foreach (DataListItem itemAgence in agenceList.Items)
                {
                    AccelaGridView feeItemsList = (AccelaGridView)itemAgence.FindControl("feeItemList");

                    if (feeItemsList != null && feeItemsList.Rows != null && feeItemsList.Rows.Count > 0)
                    {
                        if (!ValidateFeeItemInputValue(feeItemsList))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Continue to payment.
        /// </summary>
        public void ContinueToPayment()
        {
            if (!CheckLicense4Permit())
            {
                // Re-bind fee items.
                ShowFeeInformation();

                return;
            }

            // get the step number for parameter.
            string preStepNumber = Request.QueryString["stepNumber"];
            int stepNumber = 0;

            if (ValidationUtil.IsNumber(preStepNumber))
            {
                stepNumber = short.Parse(preStepNumber) + 1;
            }

            //add all fees to partial cap and recalculate them.
            if (!DoRecalculate())
            {
                return;
            }

            //validate all input required.
            if (!ValidateInputValue())
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("aca_capfee_validate_input_message"));

                // After pop up error info, need to show fee items to UI again.
                ShowFeeInformation();

                return;
            }

            // get fee total
            IFeeBll feeBll = (IFeeBll)ObjectFactory.GetObject(typeof(IFeeBll));

            bool isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());

            double feeAmount = 0.0;
            if (IsPayFeeDue)
            {
                feeAmount = feeBll.GetTotalBalanceFee(GetCapIDModel(), AppSession.User.PublicUserId);
            }
            else
            {
                if (CapUtil.IsSuperCAP(ModuleName) || isAssoFormEnabled)
                {
                    feeAmount = feeBll.GetTotalFeeByParentCapID(GetCapIDModel(), isAssoFormEnabled, AppSession.User.PublicUserId);
                }
                else
                {
                    feeAmount = feeBll.GetTotalFee(GetCapIDModel(), AppSession.User.PublicUserId);
                }
            }

            if (feeAmount < 0)
            {
                ShowNegativeFeeMessage();
                return;
            }

            if (StandardChoiceUtil.IsEnableShoppingCart() && !AppSession.User.IsAnonymous)
            {
                AddToShoppingCart();
                string url = FileUtil.AppendApplicationRoot("ShoppingCart/ShoppingCart.aspx?TabName=Home&stepNumber=2");
                Response.Redirect(url);
                return;
            }

            if (feeAmount > 0)
            {
                // if there is fee to be paid, go to payment page.
                string url = "CapPayment.aspx?Module={0}&stepNumber={1}&pageNumber=2&isPay4ExistingCap={2}&isRenewal={3}";

                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                AppSession.SetCapIDModelsToSession(CapUtil.GetChildCapIDs(capModel, ModuleName, isAssoFormEnabled && !IsPayFeeDue));

                AddToShoppingCart();
                Response.Redirect(string.Format(url, ModuleName, IsReviewPage ? stepNumber.ToString() : preStepNumber, PayFeeDueFlag, RenewalFlag));
            }
            else
            {
                // if no fee to be paid, redirect it to successful page.
                try
                {
                    CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                    // In pay fee due process, it stops here when there is no unpaid fee items.
                    if (IsPayFeeDue && (capModel.capClass == null || ACAConstant.COMPLETED.Equals(capModel.capClass.ToUpper())))
                    {
                        MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("aca_capfee_no_feeitems_message"));
                        return;
                    }

                    SysUserModel4WS sysUser = new SysUserModel4WS();
                    sysUser.userID = AppSession.User.PublicUserId;
                    sysUser.firstName = AppSession.User.FirstName;
                    sysUser.lastName = AppSession.User.LastName;
                    capModel.sysUser = sysUser;
                    capModel.accessByACA = ACAConstant.COMMON_Y;

                    ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));

                    if (CapUtil.IsSuperCAP(ModuleName) || isAssoFormEnabled)
                    {
                        bool hasFee = CapUtil.HasFee(capModel.capID, CapUtil.IsSuperCAP(ModuleName) || isAssoFormEnabled, isAssoFormEnabled);
                        OnlinePaymentResultModel onlinePaymentResultModel = capBll.CreateCapModelsFromFeeEstimate(capModel);
                        AppSession.SetOnlinePaymentResultModelToSession(onlinePaymentResultModel);

                        CapUtil.SetBreadCrumbSession(AppSession.User.PublicUserId, this.ModuleName, hasFee);
                    }
                    else
                    {
                        CapIDModel4WS[] capIDs = new CapIDModel4WS[1];
                        capIDs[0] = capModel.capID;
                        AppSession.SetOnlinePaymentResultModelToSession(ShoppingCartUtil.CreateZeroFeeCAPs(capIDs));
                    }
                }
                catch (ACAException ex)
                {
                    // show fee items to UI.
                    ShowFeeInformation();

                    MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
                    return;
                }

                // if it comes from review page's submit application,it has fee and it should skip pay fee step to Record Issuance.
                if (stepNumber > 0 && IsReviewPage)
                {
                    stepNumber++;
                }

                // redirect it to successful page.
                string url = string.Empty;

                if (CapUtil.IsSuperCAP(ModuleName) || IsFromShoppingCart || isAssoFormEnabled)
                {
                    url = string.Format("CapCompletions.aspx?Module={0}&stepNumber={1}", ModuleName, stepNumber);
                }
                else
                {
                    // if the fee items > 0 and fee amount = 0 and IsRenewal then deferPaymentFlag is "Y"
                    string deferPaymentFlag = IsRenewal ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                    url = string.Format("CapCompletion.aspx?Module={0}&stepNumber={1}&isRenewal={2}&isDeferredPayment={3}", ModuleName, stepNumber, RenewalFlag, deferPaymentFlag);
                }

                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Builds a CapIdModel as query parameter from query string.
        /// </summary>
        /// <returns>CapId model.</returns>
        public CapIDModel4WS GetCapIDModel()
        {
            // 1. if the CapId model has existed in Session of CapModel, return it from session.
            // this scene appears when it comes from Apply a permit or obtain a permit.
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            if (capModel != null && capModel.capID != null)
            {
                return capModel.capID;
            }
            else
            {
                CapIDModel4WS capIdModel = new CapIDModel4WS();

                // 2. Cap Id is not existing in session, build it from query string.
                // this scene appears when it comes from cap home.
                capIdModel.id1 = Request.QueryString[ACAConstant.CAP_ID_1];
                capIdModel.id2 = Request.QueryString[ACAConstant.CAP_ID_2];
                capIdModel.id3 = Request.QueryString[ACAConstant.CAP_ID_3];
                capIdModel.serviceProviderCode = ConfigManager.AgencyCode;

                return capIdModel;
            }
        }

        /// <summary>
        /// Handle the PreRender event.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!AppSession.IsAdmin)
            {
                ExpressionUtil.RegisterScriptLibToCurrentPage(Page);

                if (!IsPostBack)
                {
                    // Clear some temporary data stored in seestion for expression 
                    ExpressionUtil.ClearExpressionVariables();
                }

                ExpressionFactory expressionInstance = BindingExpressionFunction();

                if (!IsPostBack)
                {
                    expressionInstance.AttachOnLoadEvent(Page);
                }

                RunExpressionOnSubmit(expressionInstance);
                ExpressionUtil.ResetJsExpression(Page);
            }
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                return;
            }

            if (!AppSession.IsAdmin)
            {
                ShowFeeChangedMessage();
            }

            FeeQuantityAccuracy = StandardChoiceUtil.GetFeeQuantityAccuracy();

            //Expand/Collapse the total fee area only under review page
            HtmlControl divViewTotalfee = (HtmlControl)this.FindControl("divViewTotalfee");
            HtmlControl divNormalTotalfee = (HtmlControl)this.FindControl("divNormalTotalfee");
            HtmlControl divTotalFee = (HtmlControl)this.FindControl("divTotalFee");

            if (this.IsReviewPage)
            {
                divViewTotalfee.Visible = true;
                lblTotalFeeNoteView.Attributes.Add("class", "ACA_TotalFee_Expand");
                divTotalFee.Attributes.Add("class", "ACA_TotalFee_Expand");
                divNormalTotalfee.Visible = false;
            }
            else
            {
                divViewTotalfee.Visible = false;
                divTotalFee.Visible = true;
                divNormalTotalfee.Visible = true;
            }
        }

        /// <summary>
        /// agency List ItemDataBound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void AgenceList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            List<F4FeeItemModel4WS> feeItems = new List<F4FeeItemModel4WS>();

            HtmlTableCell tdLogo = (HtmlTableCell)e.Item.FindControl("tdLogo");
            HtmlImage imgAgencyLogo = (HtmlImage)e.Item.FindControl("imgAgencyLogo");
            AccelaGridView feeItemList = (AccelaGridView)e.Item.FindControl("feeItemList");
            AccelaLabel lblAgenceTotalFee = (AccelaLabel)e.Item.FindControl("lblAgenceTotalFee");

            F4FeeItemModel4WS feeItem = (F4FeeItemModel4WS)e.Item.DataItem;
            string currentAgenceCode = feeItem.capID.serviceProviderCode;

            IFeeBll feeBll = (IFeeBll)ObjectFactory.GetObject(typeof(IFeeBll));
            List<F4FeeItemModel4WS> capS = feeBll.GetChildCapsByAgency(allFeeItems, currentAgenceCode);

            if (capS != null && capS.Count > 0)
            {
                bool isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());
                ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();

                foreach (F4FeeItemModel4WS cap in capS)
                {
                    CapIDModel4WS currentCapId = cap.capID;
                    CapTypeModel capTypeModel = capTypeBll.GetCapTypeByCapID(currentCapId);
                    List<F4FeeItemModel4WS> childFeeItems = feeBll.GetChildCapFeeItems(allFeeItems, currentCapId);

                    string key = GetExpControlNameSuffix(feeItem);
                    if (!_feeItems.ContainsKey(key))
                    {
                        _feeItems.Add(key, childFeeItems);
                    }

                    if (CapUtil.IsSuperCAP(ModuleName) || isAssoFormEnabled)
                    {
                        F4FeeItemModel4WS capSum = new F4FeeItemModel4WS();
                        double capTotalFee = feeBll.GetChildCapTotalFee(allFeeItems, currentCapId);
                        capSum.feeDescription = CAPHelper.GetAliasOrCapTypeLabel(capTypeModel);
                        capSum.fee = capTotalFee;
                        capSum.udes = ACAConstant.CAP_TYPE_ROW;
                        feeItems.Add(capSum);
                    }

                    feeItems.AddRange(childFeeItems);
                }
            }

            feeItemList.DataSource = feeItems;
            feeItemList.DataBind();

            double childAgencyTotalFee = feeBll.GetChildAgencyTotalFee(allFeeItems, currentAgenceCode);
            lblAgenceTotalFee.Text = I18nNumberUtil.FormatMoneyForUI(childAgencyTotalFee);
            lblAgenceTotalFee.CssClass = childAgencyTotalFee < 0
                                             ? lblAgenceTotalFee.CssClass + " fee_tip"
                                             : lblAgenceTotalFee.CssClass.Replace(" fee_tip", string.Empty);

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

            if (!CapUtil.IsSuperCAP(ModuleName))
            {
                HtmlTable tableAgenceSum = (HtmlTable)e.Item.FindControl("tableAgenceSum");
                tableAgenceSum.Visible = false;
            }
        }

        /// <summary>
        /// feeList ItemDataBound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void FeeList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int index = e.Row.RowIndex;
                string cssClass = string.Empty;

                HtmlControl divQty = (HtmlControl)e.Row.FindControl("divQuantity");
                AccelaNumberText textQty = (AccelaNumberText)e.Row.FindControl("txtQuanity");
                HiddenField hdnReadOnly = (HiddenField)e.Row.FindControl("hdnReadOnly");
                AccelaLabel lblPlaceHolder = (AccelaLabel)e.Row.FindControl("lblPlaceHolder");

                F4FeeItemModel4WS feeItem = (F4FeeItemModel4WS)e.Row.DataItem;

                if (!AppSession.IsAdmin)
                {
                    if (feeItem.fee < 0)
                    {
                        AccelaLabel lblAmount = (AccelaLabel)e.Row.FindControl("lblAmount");
                        lblAmount.CssClass += " fee_tip";
                    }

                    string expressionMapName = ExpressionFactory.EXPRESSION_QUANTITY_LABEL + ExpressionFactory.SPLIT_CHAR + index;
                    expressionMapName += GetExpControlNameSuffix(e.Row.DataItem as F4FeeItemModel4WS);
                    textQty.Attributes.Add("ExpressionMapName", expressionMapName);
                    textQty.IsNeedDot = FeeQuantityAccuracy > 0;
                    textQty.IsNeedNegative = true;

                    textQty.Text = I18nNumberUtil.ConvertDecimalForUI(feeItem.feeUnit.ToString());

                    if (!_expressionControls.ContainsKey(expressionMapName))
                    {
                        _expressionControls.Add(expressionMapName, textQty);
                    }
                }

                //if the fee item is set to auto invoiced, the fee quantity should be set to readonly
                if (IsPayFeeDue)
                {
                    //if pay fee for existing CAP, not matter display in ACA status,  it should be display and read only.
                    divQty.Visible = true;
                    hdnReadOnly.Value = ACAConstant.COMMON_Y;
                    textQty.Visible = false;
                }
                else
                {
                    if (ACAConstant.COMMON_READONLY.Equals(feeItem.defaultFlag))
                    {
                        //if display in ACA as "readOnly" , it should be display and readonly
                        divQty.Visible = true;
                        hdnReadOnly.Value = ACAConstant.COMMON_Y;
                        textQty.Visible = false;
                    }
                    else
                    {
                        //if display in ACA as "Y" or "",  it should be display fee item and editable
                        divQty.Visible = false;
                        hdnReadOnly.Value = ACAConstant.COMMON_N;
                        textQty.Visible = true;
                        lnkRecalculate.Visible = true;

                        //setting required.
                        if (ValidationUtil.IsYes(feeItem.acaRequiredFlag))
                        {
                            HtmlGenericControl divRequiredIndicator = (HtmlGenericControl)e.Row.FindControl("divRequiredIndicator");

                            if (divRequiredIndicator != null)
                            {
                                divRequiredIndicator.Visible = true;
                                string contentTitle = LabelUtil.RemoveHtmlFormat(LabelUtil.GetTextByKey("aca_required_field", ModuleName));
                                textQty.Attributes.Add("title", contentTitle);
                            }
                        }
                    }
                }

                bool isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());
                if (CapUtil.IsSuperCAP(ModuleName) || isAssoFormEnabled)
                {
                    if (ACAConstant.CAP_TYPE_ROW.Equals(feeItem.udes))
                    {
                        divQty.Visible = false;
                        textQty.Visible = false;
                        cssClass = "ACA_TabRow_SmallEven ACA_TabRow_Bold";
                    }
                    else
                    {
                        lblPlaceHolder.Visible = true;
                        cssClass = index % 2 == 0 ? "ACA_TabRow_SmallOdd" : "ACA_TabRow_SmallEven2";
                    }
                }
                else
                {
                    cssClass = index % 2 == 0 ? "ACA_TabRow_SmallEven" : "ACA_TabRow_SmallOdd";
                }

                cssClass += " font11px";
                e.Row.CssClass = cssClass;
            }
            else
            {
                e.Row.CssClass = "ACA_TabRow_SmallEven ACA_BkTit ACA_TabTitle";
            }

            e.Row.Cells[3].Style["display"] = "none";
        }

        /// <summary>
        /// Get Fee Description.
        /// </summary>
        /// <param name="objResFeeDescription">object result fee description</param>
        /// <param name="objFeeDescription">object fee description.</param>
        /// <returns>Fee Description</returns>
        protected string GetFeeDescription(object objResFeeDescription, object objFeeDescription)
        {
            string resFeeDescription = (string)objResFeeDescription;
            string feeDescription = (string)objFeeDescription;
            return I18nStringUtil.GetString(resFeeDescription, feeDescription);
        }

        /// <summary>
        /// Recalculates the fee amount when user changed some items or fee value.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LnkRecalculate_Click(object sender, EventArgs e)
        {
            DoRecalculate();

            // After recalculating, need to show fee items to UI again.
            ShowFeeInformation();
            Page.FocusElement(lnkRecalculate.ClientID);
        }

        /// <summary>
        /// Gets fee total and show it to UI.
        /// </summary>
        private void ShowFeeTotal()
        {
            IFeeBll feeBll = (IFeeBll)ObjectFactory.GetObject(typeof(IFeeBll));

            // show fee total
            double feeAmount = 0;

            bool isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());
            if (IsPayFeeDue)
            {
                feeAmount = feeBll.GetTotalBalanceFee(GetCapIDModel(), AppSession.User.PublicUserId);

                //if existing Cap ,Recalcuate button be removed.
                lnkRecalculate.Visible = false;
            }
            else
            {
                if (CapUtil.IsSuperCAP(ModuleName) || isAssoFormEnabled)
                {
                    feeAmount = feeBll.GetTotalFeeByParentCapID(GetCapIDModel(), isAssoFormEnabled, AppSession.User.PublicUserId);
                }
                else
                {
                    feeAmount = feeBll.GetTotalFee(GetCapIDModel(), AppSession.User.PublicUserId);
                }
            }

            lblFeeAmount.Text = I18nNumberUtil.FormatMoneyForUI(feeAmount);

            //Enhancement to move Total Fee to the top of fee section and can expand/collapse the detail info.
            lblFeeAmountView.Text = I18nNumberUtil.FormatMoneyForUI(feeAmount);

            if (feeAmount < 0)
            {
                lblFeeAmount.CssClass += " fee_tip";
                lblFeeAmountView.CssClass += " fee_tip";
            }
            else
            {
                lblFeeAmount.CssClass = lblFeeAmount.CssClass.Replace(" fee_tip", string.Empty);
                lblFeeAmountView.CssClass = lblFeeAmountView.CssClass.Replace(" fee_tip", string.Empty);
            }
        }

        /// <summary>
        /// Generate expression function and binding it into submit control.
        /// </summary>
        /// <param name="expressionInstance">ExpressionFactory instance</param>
        private void RunExpressionOnSubmit(ExpressionFactory expressionInstance)
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered("SubmitExpression"))
            {
                string feeSubmitFunction = string.Empty;

                if (expressionInstance != null)
                {
                    feeSubmitFunction = expressionInstance.GetRunExpFunctionOnSubmit();
                }

                string jsFunctionBeforeExp = "SetNotAsk(true);";
                string jsFunctionAfterExp = "if (CheckDecimal() == false){return false;}";
                string strSubmitFuction = ExpressionUtil.GetExpressionScriptOnSubmit(feeSubmitFunction, jsFunctionBeforeExp, jsFunctionAfterExp);
            
                ScriptManager.RegisterStartupScript(this, typeof(Page), "SubmitExpression", strSubmitFuction, true);
            }
        }

        /// <summary>
        /// Get report button property
        /// </summary>
        /// <returns>a report button property model.</returns>
        private ReportButtonPropertyModel4WS[] GetReportButtonProperty()
        {
            IReportBll reportBll = (IReportBll)ObjectFactory.GetObject(typeof(IReportBll));
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            ReportButtonPropertyModel4WS[] arrayRBTModel = reportBll.GetReportButtonProperty(capModel.capID, AppSession.User.PublicUserId, ModuleName);
            return arrayRBTModel;
        }

        /// <summary>
        /// Get the fee condition information.
        /// </summary>
        /// <returns>Return the fee conditions.</returns>
        private Dictionary<CapTypeModel, NoticeConditionModel[]> GetFeeConditionFromEMSE()
        {
            // get and show fee condition information. super agency has no fee conditions.
            if (CapUtil.IsSuperCAP(ModuleName))
            {
                return null;
            }

            try
            {
                IConditionBll conditionBll = ObjectFactory.GetObject<IConditionBll>();
                ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();
                Dictionary<CapTypeModel, NoticeConditionModel[]> dictCondition = new Dictionary<CapTypeModel, NoticeConditionModel[]>();

                CapModel4WS capModel4WS = AppSession.GetCapModelFromSession(ModuleName);
                bool isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());

                if (isAssoFormEnabled)
                {
                    // Get all fee conditions for parent cap and all the child caps.
                    CapIDModel4WS[] allChildCapIds = CapUtil.GetChildCapIDs(capModel4WS, ModuleName, isAssoFormEnabled);

                    foreach (CapIDModel4WS capID in allChildCapIds)
                    {
                        CapTypeModel capType = capTypeBll.GetCapTypeByCapID(capID);
                        NoticeConditionModel[] conditionList = conditionBll.GetAllCondition4ACAFeeEstimate(capID, null);

                        if (conditionList != null && conditionList.Length > 0)
                        {
                            dictCondition.Add(capType, conditionList);
                        }
                    }
                }
                else
                {
                    NoticeConditionModel[] conditionList = conditionBll.GetAllCondition4ACAFeeEstimate(capModel4WS.capID, null);

                    if (conditionList != null && conditionList.Length > 0)
                    {
                        dictCondition.Add(capModel4WS.capType, conditionList);
                    }
                }

                return dictCondition;
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Merges the conditions.
        /// </summary>
        /// <param name="dictA">The condition A.</param>
        /// <param name="dictB">The condition B.</param>
        /// <returns>Return the merged condition.</returns>
        private Dictionary<CapTypeModel, NoticeConditionModel[]> MergeConditions(Dictionary<CapTypeModel, NoticeConditionModel[]> dictA, Dictionary<CapTypeModel, NoticeConditionModel[]> dictB)
        {
            if (dictA == null || dictA.Count == 0)
            {
                return dictB;
            }

            if (dictB == null || dictB.Count == 0)
            {
                return dictA;
            }

            List<string> mergedCapType = new List<string>();
            Dictionary<CapTypeModel, NoticeConditionModel[]> result = new Dictionary<CapTypeModel, NoticeConditionModel[]>();

            // 1. merge the same cap type to the result
            foreach (KeyValuePair<CapTypeModel, NoticeConditionModel[]> dictOuter in dictA)
            {
                bool hasSameCapType = false;

                foreach (KeyValuePair<CapTypeModel, NoticeConditionModel[]> dictInner in dictB)
                {
                    if (IsSameCapType(dictOuter.Key, dictInner.Key))
                    {
                        NoticeConditionModel[] conditions = MergeConditions(dictOuter.Value, dictInner.Value);
                        result.Add(dictOuter.Key, conditions);
                        mergedCapType.Add(GetCapTypeID(dictOuter.Key));

                        hasSameCapType = true;
                        break;
                    }
                }

                if (!hasSameCapType)
                {
                    result.Add(dictOuter.Key, dictOuter.Value);
                }
            }

            // 2. append the different cap type that not merged in [condition B]
            foreach (var dict in dictB)
            {
                string capTypeID = GetCapTypeID(dict.Key);
                if (!mergedCapType.Contains(capTypeID))
                {
                    result.Add(dict.Key, dict.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// Merges the conditions.
        /// </summary>
        /// <param name="conditionA">The condition A.</param>
        /// <param name="conditionB">The condition B.</param>
        /// <returns>Return the merged condition.</returns>
        private NoticeConditionModel[] MergeConditions(NoticeConditionModel[] conditionA, NoticeConditionModel[] conditionB)
        {
            if (conditionA == null || conditionA.Length == 0)
            {
                return conditionB;
            }

            if (conditionB == null || conditionB.Length == 0)
            {
                return conditionA;
            }

            List<NoticeConditionModel> result = new List<NoticeConditionModel>();
            List<string> conditionNbrList = new List<string>();

            // get the unique key for condition list
            foreach (NoticeConditionModel model in conditionA)
            {
                conditionNbrList.Add(model.serviceProviderCode + model.standardConditionNumber);
            }

            // merge to the first condition list if it not exists.
            result.AddRange(conditionA);

            foreach (NoticeConditionModel model in conditionB)
            {
                if (!conditionNbrList.Contains(model.serviceProviderCode + model.standardConditionNumber))
                {
                    conditionNbrList.Add(model.serviceProviderCode + model.standardConditionNumber);
                    result.Add(model);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Gets the cap type ID.
        /// </summary>
        /// <param name="capType">The cap type.</param>
        /// <returns>Get the cap type ID.</returns>
        private string GetCapTypeID(CapTypeModel capType)
        {
            if (capType != null)
            {
                return capType.serviceProviderCode + capType.group + capType.type + capType.subType + capType.category;
            }

            return string.Empty;
        }

        /// <summary>
        /// Get Child CapID
        /// </summary>
        /// <param name="itemCap">DataList Item</param>
        /// <returns>A CapIDModel4WS</returns>
        private CapIDModel4WS GetChildCapID(GridViewRow itemCap)
        {
            CapIDModel4WS childCapID = new CapIDModel4WS();

            if (itemCap != null)
            {
                HiddenField hdnCapID1 = (HiddenField)itemCap.FindControl("hdnCapID1");
                HiddenField hdnCapID2 = (HiddenField)itemCap.FindControl("hdnCapID2");
                HiddenField hdnCapID3 = (HiddenField)itemCap.FindControl("hdnCapID3");
                HiddenField hdnAgence = (HiddenField)itemCap.FindControl("hdnAgence");

                childCapID.id1 = hdnCapID1.Value.ToString();
                childCapID.id2 = hdnCapID2.Value.ToString();
                childCapID.id3 = hdnCapID3.Value.ToString();
                childCapID.serviceProviderCode = hdnAgence.Value.ToString();
            }

            return childCapID;
        }

        /// <summary>
        /// Determines whether is the same cap type.
        /// </summary>
        /// <param name="capType1">The cap type1.</param>
        /// <param name="capType2">The cap type2.</param>
        /// <returns>return true if is the same cap type; otherwise, false.</returns>
        private bool IsSameCapType(CapTypeModel capType1, CapTypeModel capType2)
        {
            if (capType1.serviceProviderCode == capType2.serviceProviderCode &&
                capType1.group == capType2.group &&
                capType1.type == capType2.type &&
                capType1.subType == capType2.subType &&
                capType1.category == capType2.category)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// each fee list,  validate text box Input value.
        /// </summary>
        /// <param name="feeDataList">fee data list</param>
        /// <returns>true / false</returns>
        private bool ValidateFeeItemInputValue(AccelaGridView feeDataList)
        {
            //do each fee list.
            foreach (GridViewRow row in feeDataList.Rows)
            {
                AccelaNumberText txtQuantity = (AccelaNumberText)row.FindControl("txtQuanity");

                if (txtQuantity != null && txtQuantity.Visible)
                {
                    HtmlGenericControl divRequiredIndicator = (HtmlGenericControl)row.FindControl("divRequiredIndicator");

                    //if requried, and input value as 0,
                    if (divRequiredIndicator != null && divRequiredIndicator.Visible)
                    {
                        double? quantity = txtQuantity.DoubleValue;

                        if (quantity == null || quantity.Value <= 0)
                        {
                            return false;
                        }
                    }

                    try
                    {
                        txtQuantity.DoubleValue = GetValidQuantity(txtQuantity.Text);
                    }
                    catch
                    {
                        ShowQuantityInvalidateMessage();
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// convert input value to double format
        /// </summary>
        /// <param name="quantity">the fee quantity</param>
        /// <returns>fee quantity</returns>
        private double GetValidQuantity(string quantity)
        {
            decimal result = 0;
            decimal parseResult;
            var maxDecimal = new decimal(99999999999999.9);
            bool successed = decimal.TryParse(quantity, out parseResult);

            if (!successed || maxDecimal.CompareTo(parseResult) < 0)
            {
                string errMessage = GetTextByKey("aca_capfee_msg_invalidquantity").Replace("'", "\\'");
                throw new ACAException(new Exception(errMessage));
            }

            result = decimal.Round(parseResult, FeeQuantityAccuracy, MidpointRounding.ToEven);
            return decimal.ToDouble(result);
        }

        /// <summary>
        /// Show the quantity invalidate message to UI.
        /// </summary>
        private void ShowQuantityInvalidateMessage()
        {
            string msg = GetTextByKey("per_feeItemList_error_quantityInvalidate");
            MessageUtil.ShowMessage(Page, MessageType.Error, msg);
        }

        /// <summary>
        /// Gets job value from session, if no job value returns 0.
        /// </summary>
        /// <returns>job value.</returns>
        private double GetJobValue()
        {
            double jobValue = 0;
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            if (capModel.bvaluatnModel != null && !string.IsNullOrEmpty(capModel.bvaluatnModel.estimatedValue))
            {
                jobValue = I18nNumberUtil.ParseMoneyFromWebService(capModel.bvaluatnModel.estimatedValue);
            }

            return jobValue;
        }

        /// <summary>
        /// Bind Agencies.
        /// </summary>
        private void BindAgencies()
        {
            allFeeItems = GetFeeItems();

            IFeeBll feeBll = (IFeeBll)ObjectFactory.GetObject(typeof(IFeeBll));
            ArrayList agences = feeBll.GetChildAgencysByFeeItems(allFeeItems);

            agenceList.DataSource = agences;
            agenceList.DataBind();
        }

        /// <summary>
        /// Get Fee Items.
        /// </summary>
        /// <returns>array list.</returns>
        private List<F4FeeItemModel4WS> GetFeeItems()
        {
            F4FeeModel4WS[] feesforAllCaps;
            CapIDModel4WS capId = GetCapIDModel();

            // the real fee items for daily is stored in f4FeeItemModel propery of F4FeeModel4WS
            var feeItems = new List<F4FeeItemModel4WS>();
            var feeBll = (IFeeBll)ObjectFactory.GetObject(typeof(IFeeBll));
            var isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());
            var isOnlyShowAutoInvoiceFeeItems = StandardChoiceUtil.IsOnlyShowAutoInvoiceFeeItems(capId.serviceProviderCode, ModuleName);

            if (IsPayFeeDue)
            {
                feesforAllCaps = feeBll.GetNoPaidFeeItemByCapID(capId, AppSession.User.PublicUserId);
            }
            else
            {
                if (isAssoFormEnabled || CapUtil.IsSuperCAP(capId))
                {
                    feesforAllCaps = feeBll.GetFeeItemsByParentCapID(capId, isAssoFormEnabled, AppSession.User.PublicUserId);
                }
                else
                {
                    feesforAllCaps = feeBll.GetFeeItemsByCapID(capId, AppSession.User.PublicUserId);
                }
            }

            if (feesforAllCaps != null && feesforAllCaps.Length > 0)
            {
                foreach (var fee in feesforAllCaps)
                {
                    if (fee.f4FeeItemModel == null)
                    {
                        continue;
                    }

                    // When auto invoice disabled and current module is not in the enabled auto-invoice module list, ACA only shows those fee item with auto invoice flag is y.
                    if (!IsPayFeeDue
                        && !ValidationUtil.IsYes(fee.f4FeeItemModel.autoInvoiceFlag)
                        && isOnlyShowAutoInvoiceFeeItems)
                    {
                        continue;
                    }

                    feeItems.Add(fee.f4FeeItemModel);
                }
            }

            return feeItems;
        }

        /// <summary>
        /// Check the license of the permit. show error message if the permit has unavailable license(s).
        /// </summary>
        /// <returns>true if the all of the LPs of this permit are valid;otherwise,false.</returns>
        private bool CheckLicense4Permit()
        {
            bool isAvailableLPs = true;

            //Don't need to judge the big CAP in super agency.
            if (LicenseUtil.EnableExpiredLicense() && !CapUtil.IsSuperCAP(ModuleName))
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                string unAvailaleLPs = LicenseUtil.GetUnAvailableLPNums4Permit(capModel);

                if (string.IsNullOrEmpty(unAvailaleLPs))
                {
                    bool isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());
                    if (isAssoFormEnabled)
                    {
                        //Check the child caps.
                        ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                        CapIDModel4WS[] allChildCapIds = CapUtil.GetChildCapIDs(capModel, ModuleName, false);
                        foreach (CapIDModel4WS capID in allChildCapIds)
                        {
                            //Currently, Hazmat does not supports super agency.
                            CapWithConditionModel4WS capCondition = capBll.GetCapViewBySingle(capID, AppSession.User.UserSeqNum, ACAConstant.COMMON_N, false);

                            if (capCondition != null && capCondition.capModel != null)
                            {
                                unAvailaleLPs = LicenseUtil.GetUnAvailableLPNums4Permit(capCondition.capModel);
                            }

                            if (!string.IsNullOrEmpty(unAvailaleLPs))
                            {
                                break;
                            }
                        }
                    }
                }

                //the CAP has unavailable LP(s).
                if (!string.IsNullOrEmpty(unAvailaleLPs))
                {
                    MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("per_permitfee_error_unavailablelicense"));

                    isAvailableLPs = false;
                }
            }

            return isAvailableLPs;
        }

        /// <summary>
        /// Expression control name suffix. That includes agency code and cap type.
        /// </summary>
        /// <param name="feeItem">feeI item value</param>
        /// <returns>Suffix of expression control name</returns>
        private string GetExpControlNameSuffix(F4FeeItemModel4WS feeItem)
        {
            string expControlNameSuffix = string.Empty;
            CapTypeModel capTypeModel;
            string captype = string.Empty;

            if (feeItem != null && feeItem.capID != null && StandardChoiceUtil.IsSuperAgency())
            {
                expControlNameSuffix = ACAConstant.SPLIT_CHAR + feeItem.capID.serviceProviderCode;
                ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
                capTypeModel = capTypeBll.GetCapTypeByCapID(feeItem.capID);
            }
            else
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                if (capModel != null && capModel.capID != null)
                {
                    expControlNameSuffix = ACAConstant.SPLIT_CHAR + capModel.capID.serviceProviderCode;
                }

                if (capModel != null && capModel.capType != null)
                {
                    capTypeModel = capModel.capType;
                }
                else
                {
                    capTypeModel = null;
                }
            }

            captype = Server.UrlEncode(CAPHelper.GetAliasOrCapTypeLabel(capTypeModel));
            expControlNameSuffix += ACAConstant.SPLIT_CHAR + ExpressionUtil.FilterSpciefCharForControlName(captype);
            InitSubCapModels(feeItem.capID, CAPHelper.GetAliasOrCapTypeLabel(capTypeModel));

            return expControlNameSuffix;
        }

        /// <summary>
        /// Collect cap model instance into "SubCapModels" object.
        /// </summary>
        /// <param name="capID">capID value</param>
        /// <param name="captype">cap type value</param>
        private void InitSubCapModels(CapIDModel4WS capID, string captype)
        {
            if (StandardChoiceUtil.IsSuperAgency() && capID != null)
            {
                StringBuilder key = new StringBuilder(ExpressionFactory.EXPRESSION_QUANTITY_LABEL);
                key.Append(ACAConstant.SPLIT_CHAR);
                key.Append(capID.serviceProviderCode);
                key.Append(ACAConstant.SPLIT_CHAR);
                key.Append(ExpressionUtil.FilterSpciefCharForControlName(Server.UrlEncode(captype)));

                // some sub ASI group may belong to the same cap model.
                if (!_subCapModels.ContainsKey(key.ToString()))
                {
                    ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
                    CapTypeModel capTypeModel = capTypeBll.GetCapTypeByCapID(capID);
                    CapModel4WS capModel = new CapModel4WS();
                    CapModel4WS partialCap = AppSession.GetCapModelFromSession(ModuleName);
                    capModel.capID = capID;
                    capModel.capType = capTypeModel;

                    if (capTypeModel != null && capID != null)
                    {
                        capModel.moduleName = capTypeModel.group;
                        capModel.appSpecificInfoGroups = GetMatchedASIGroup(capTypeModel.serviceProviderCode, capTypeModel.specInfoCode);
                    }

                    if (partialCap != null)
                    {
                        capModel.auditID = partialCap.auditID;
                    }

                    _subCapModels.Add(key.ToString(), capModel);
                }
            }
            else
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                string key = ExpressionUtil.GetFullControlFieldName(capModel, ExpressionFactory.EXPRESSION_QUANTITY_LABEL);

                if (!_subCapModels.ContainsKey(key))
                {
                    _subCapModels.Add(key, capModel);
                }
            }
        }

        /// <summary>
        /// Get matched ASI group from current partial cap.
        /// </summary>
        /// <param name="serviceCode">service code</param>
        /// <param name="groupName">ASI group name</param>
        /// <returns>ASI groups</returns>
        private AppSpecificInfoGroupModel4WS[] GetMatchedASIGroup(string serviceCode, string groupName)
        {
            List<AppSpecificInfoGroupModel4WS> appSpecificInfoGroups = new List<AppSpecificInfoGroupModel4WS>();
            CapModel4WS partialCap = AppSession.GetCapModelFromSession(ModuleName);

            if (partialCap.appSpecificInfoGroups != null)
            {
                foreach (AppSpecificInfoGroupModel4WS item in partialCap.appSpecificInfoGroups)
                {
                    if (item != null &&
                        ((item.capID != null && item.capID.serviceProviderCode.Equals(serviceCode)) || item.capID == null) &&
                        item.groupCode.Equals(groupName))
                    {
                        appSpecificInfoGroups.Add(item);
                    }
                }
            }

            return appSpecificInfoGroups.ToArray();
        }

        /// <summary>
        /// Show Fee Change Message.
        /// </summary>
        private void ShowFeeChangedMessage()
        {
            bool isAssoFormEnabled = CapUtil.IsAssoFormEnabled(AppSession.GetParentCapTypeFromSession());
            CapIDModel4WS[] allCapIds = CapUtil.GetChildCapIDs(AppSession.GetCapModelFromSession(ModuleName), ModuleName, isAssoFormEnabled);
            CapIDModel4WS[] feeChangedCapIds = CapUtil.GetFeeChangedCapIdList(allCapIds);

            if (feeChangedCapIds != null && feeChangedCapIds.Length > 0)
            {
                CapUtil.UpdateFeeScheduleOrFeeItems(allCapIds);
                MessageUtil.ShowMessageAfterPageLoad(Page, MessageType.Notice, GetTextByKey("per_shoppingcart_message_feeschedulechangenote"));
            }
        }

        /// <summary>
        /// Generate calling expression function and bind the function to specify control.
        /// </summary>
        /// <returns>return expression factory</returns>
        private ExpressionFactory BindingExpressionFunction()
        {
            ExpressionFactory expressionInstance = new ExpressionFactory(ModuleName, ExpressionType.Fee_Item, _expressionControls, _subCapModels);
            expressionInstance.FeeItems = _feeItems;

            foreach (ExpressionFieldModel ews in expressionInstance.ExecuteFieldList)
            {
                string keyName = ews.name;
                string functionName = string.Empty;

                if (keyName.IndexOf(ExpressionFactory.EXPRESSION_QUANTITY_LABEL) == 0 &&
                    _subCapModels.ContainsKey(ews.name))
                {
                    ExpressionFieldModel[] inputParams = expressionInstance.GetInputParamList(_subCapModels[ews.name], ews);

                    if (inputParams == null)
                    {
                        continue;
                    }

                    functionName = expressionInstance.GetClientExperssionScripts(_subCapModels[ews.name], ews, inputParams, false);
                    functionName += ";";
                }

                int startIndex = ews.name.IndexOf(ews.servProvCode);
                string captype = (ews.name.Length > startIndex && startIndex != -1) ? ews.name.Substring(startIndex) : string.Empty;

                foreach (KeyValuePair<string, WebControl> item in _expressionControls)
                {
                    if (item.Key.IndexOf(captype) > -1)
                    {
                        expressionInstance.BindingClientMethod(item.Value, ews.fireEvent, functionName);
                    }
                }
            }

            return expressionInstance;
        }

        #endregion Methods
    }
}