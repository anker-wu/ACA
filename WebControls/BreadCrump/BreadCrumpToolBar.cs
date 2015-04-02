/**
 * <pre>
 *
 *  Accela
 *  File: BreadCrumpToolBar.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: BreadCrumpToolBar.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;

namespace Accela.Web.Controls
{
    #region Delegates

    /// <summary>
    /// when step changed raise a event to deal with.
    /// </summary>
    /// <param name="src">the object that raises event.</param>
    /// <param name="e">event type.</param>
    public delegate void StepChangedEventHandler(object src, StepChangedEventArgs e);

    #endregion Delegates

    /// <summary>
    /// Mark the process of application  a permit.
    /// </summary>
    [DefaultProperty("CurrentStep")]
    [DefaultEvent("StepChanged")]
    [ParseChildren(false)]
    [PersistChildren(false)]
    [Description("BreadCrump ToolBar for ACA")]
    [Designer(typeof(BreadCrumpDesigner))]
    [ToolboxData("<{0}:BreadCrumpToolBar runat=server></{0}:BreadCrumpToolBar>")]
    public class BreadCrumpToolBar : Panel, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
    {
        #region Fields

        /// <summary>
        /// default length that the breadcrumb contains characters.
        /// </summary>
        private const int MAX_CHARACTER_LENGTH = 31;

        /// <summary>
        /// the max expand step count in the breadcrumb
        /// </summary>
        private const int MAX_EXPAND_STEP_COUNT = 5;

        #endregion Fields

        #region Events

        /// <summary>
        /// When step changed the event is worked.
        /// </summary>
        public event StepChangedEventHandler StepChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets current page step number.
        /// </summary>
        [ReadOnly(true)]
        [Browsable(true)]
        [Description("Current Step Index")]
        [Category("Properties")]
        [DefaultValue(1)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentStepIndex
        {
            get
            {
                object cindex = ViewState["CurrentStepIndex"];

                return cindex == null ? 1 : (int)cindex;
            }

            set
            {
                ViewState["CurrentStepIndex"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether enable view state or not.
        /// </summary>
        [ReadOnly(true)]
        [Browsable(true)]
        [Description("view state must be true")]
        [DefaultValue(true)]
        [Category("Properties")]
        public override bool EnableViewState
        {
            get
            {
                return base.EnableViewState;
            }

            set
            {
                base.EnableViewState = true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the breadcrumb is used for shopping cart or not.
        /// </summary>
        [ReadOnly(false)]
        [Browsable(true)]
        [Description("breadcrumb used in shopping cart")]
        [DefaultValue(false)]
        [Category("Properties")]
        public bool IsForShoppingCart
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets current page flow information.e.g: step list and last index.
        /// </summary>
        [ReadOnly(true)]
        [Browsable(true)]
        [Description("Init Data")]
        [Category("Properties")]
        [DefaultValue(1)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<int, BreadCrumpInfo> PageFlow
        {
            get
            {
                if (ViewState["stepList"] != null)
                {
                    return (Dictionary<int, BreadCrumpInfo>)ViewState["stepList"];
                }

                return new Dictionary<int, BreadCrumpInfo>();
            }

            set
            {
                ViewState["stepList"] = value;
            }
        }

        /// <summary>
        /// Gets current breadcrumb information that store session.
        /// IsForShoppingCart or not.
        /// </summary>
        private string BreadCrumb
        {
            get
            {
                if (IsForShoppingCart)
                {
                    return SessionConstant.SESSION_SHOPPING_CART_BREADCRUMB;
                }

                return SessionConstant.SESSION_BREADCRUMB;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// load the data when post back.
        /// </summary>
        /// <param name="pkey">PostBack key value.</param>
        /// <param name="pcol">PostBack col value.</param>
        /// <returns>success or not.</returns>
        public virtual bool LoadPostData(string pkey, NameValueCollection pcol)
        {
            string str = pcol[UniqueID + "_input"];
            if (str != null && str.Trim().Length > 0)
            {
                try
                {
                    int pindex = int.Parse(str);
                    CurrentStepIndex = pindex;
                    Page.RegisterRequiresRaiseEvent(this);
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException("error");
                }
                catch (Exception ex)
                {
                    throw new Exception("error:" + ex.Message);
                }
            }

            return false;
        }

        /// <summary>
        /// awoke a post back event.
        /// </summary>
        /// <param name="stepIndex">stepIndex number.</param>
        public void RaisePostBackEvent(string stepIndex)
        {
            try
            {
                if (string.IsNullOrEmpty(stepIndex))
                {
                    stepIndex = CurrentStepIndex.ToString();
                }

                int cindex = int.Parse(stepIndex);
                OnStepChanged(new StepChangedEventArgs(cindex));
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException("input error");
            }
            catch (Exception ex)
            {
                throw new Exception("error:" + ex.Message);
            }
        }

        /// <summary>
        /// Get offset index(for Fee Estimate)
        /// </summary>
        /// <param name="convertSession">The convert session.</param>
        /// <returns>offset index</returns>
        public int GetOffsetIndex(BreadCrumbParmsInfo convertSession = null)
        {
            int offsetIndex = 0;

            if (convertSession == null && HttpContext.Current.Session[BreadCrumb] != null)
            {
                convertSession = (BreadCrumbParmsInfo)HttpContext.Current.Session[BreadCrumb];
            }

            if (convertSession == null)
            {
                return offsetIndex;
            }

            // set offset index
            if (convertSession.PageFrom == PageFrom.PayFeeDue || !convertSession.IsConvertToApp)
            {
                return offsetIndex;
            }

            if (convertSession.HasFeeEstimate)
            {
                offsetIndex++;
            }

            if (convertSession.HasFeeForm)
            {
                offsetIndex++;
            }

            return offsetIndex;
        }

        /// <summary>
        /// awoke the event that post data changed.
        /// </summary>
        public virtual void RaisePostDataChangedEvent()
        {
        }

        /// <summary>
        /// trigger when current page changed.
        /// </summary>
        /// <param name="e">StepChangedEventArgs e</param>
        protected virtual void OnStepChanged(StepChangedEventArgs e)
        {
            if (StepChanged != null)
            {
                StepChanged(this, e);
            }
        }

        /// <summary>
        /// Render the breadcrumb to the current page.
        /// </summary>
        /// <param name="writer">output stream.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (PageFlow.Count == 0)
            {
                return;
            }

            int offsetIndex = 1;

            // set offset index
            if (HttpContext.Current.Session[BreadCrumb] != null)
            {
                BreadCrumbParmsInfo converSession = (BreadCrumbParmsInfo)HttpContext.Current.Session[BreadCrumb];

                if (converSession.PageFrom == PageFrom.PayFeeDue)
                {
                    return;
                }

                offsetIndex = offsetIndex + GetOffsetIndex(converSession);
            }

            // set the current step index, it begins from 1
            string stepNumber = HttpContext.Current.Request.QueryString["stepNumber"];
            if (string.IsNullOrEmpty(stepNumber) || stepNumber == "0")
            {
                stepNumber = "1";
            }

            int stepIndex = 1;
            if (!int.TryParse(stepNumber, out stepIndex))
            {
                return;
            }

            stepIndex = stepIndex - offsetIndex;
            CurrentStepIndex = stepIndex;
            if (HttpContext.Current.Session[BreadCrumb] == null)
            {
                BreadCrumbParmsInfo breadcrumbParmsInfo = new BreadCrumbParmsInfo();
                breadcrumbParmsInfo.HasFeeForm = false;
                breadcrumbParmsInfo.HasLicenseList = true;
                breadcrumbParmsInfo.LastIndex = 1;

                HttpContext.Current.Session[BreadCrumb] = breadcrumbParmsInfo;
            }
            else
            {
                BreadCrumbParmsInfo sInfo = (BreadCrumbParmsInfo)HttpContext.Current.Session[BreadCrumb];
                string strCurrentStepIndex = HttpContext.Current.Request.QueryString["stepNumber"];

                if (!string.IsNullOrEmpty(strCurrentStepIndex))
                {
                    int curStepIndex = 0;
                    int.TryParse(strCurrentStepIndex, out curStepIndex);
                    CurrentStepIndex = curStepIndex;
                    if (sInfo.IsConvertToApp && (curStepIndex == offsetIndex + 1) && !sInfo.CheckFeeFormVisible)
                    {
                        sInfo.LastIndex = curStepIndex - offsetIndex;
                        sInfo.CheckFeeFormVisible = true;
                        HttpContext.Current.Session[BreadCrumb] = sInfo;
                    }
                    else
                    {
                        if (sInfo.LastIndex < stepIndex)
                        {
                            sInfo.LastIndex = stepIndex;
                            HttpContext.Current.Session[BreadCrumb] = sInfo;
                        }
                    }
                }
            }
           
            StringBuilder sb = new StringBuilder();

            // 1. Set the top HTML of the breadcrumb's title
            RenderHeader(sb);

            if (!ValidationUtil.IsYes(HttpContext.Current.Request[UrlConstant.BREADCRUMB_HIDDEN_NAVIGATE]))
            {
                BreadCrumpInfo currentStepInfo;

                // 2. Set the body HTML of breadcrumb
                RenderBody(sb, stepIndex, out currentStepInfo);

                // 3. Set the bottom HTML of page title and page instruction
                RenderBottom(sb, currentStepInfo);
            }

            writer.Write(sb.ToString());
        }

        /// <summary>
        /// Render the breadcrumb bottom.
        /// </summary>
        /// <param name="sb">The StringBuilder to append the HTML.</param>
        /// <param name="currentStepInfo">The current step info.</param>
        private static void RenderBottom(StringBuilder sb, BreadCrumpInfo currentStepInfo)
        {
            if (currentStepInfo == null)
            {
                return;
            }

            // set the page title html
            string pageTitle = GetPageTitleHtml(currentStepInfo);

            sb.Append("<div class='breadcrump-pagetitle font13px'>");
            sb.Append(pageTitle);
            sb.Append("</div>");

            // set the current page instruction
            string pageInstruction = string.Empty;

            if (HttpContext.Current.Request.QueryString["pageNumber"] != null)
            {
                int pi = int.Parse(HttpContext.Current.Request.QueryString["pageNumber"]);

                if (currentStepInfo.PageInstructions != null && pi <= currentStepInfo.PageInstructions.Count)
                {
                    pageInstruction = currentStepInfo.PageInstructions[pi - 1];
                }
            }

            if (!string.IsNullOrEmpty(pageInstruction))
            {
                sb.AppendFormat("<div><span class='ACA_Page_Instruction ACA_Page_Instruction_FontSize'>{0}</span></div>", pageInstruction);
            }
        }

        /// <summary>
        /// Get the HTML of the page title.
        /// </summary>
        /// <param name="info">The BreadCrumpInfo.</param>
        /// <returns>return the html of the Page title.</returns>
        private static string GetPageTitleHtml(BreadCrumpInfo info)
        {
            string pageTitle = string.Empty;
            string[] pageTitleArray = new[] { string.Empty };

            if (!string.IsNullOrEmpty(info.Pagetitle))
            {
                pageTitleArray = info.Pagetitle.Split('|');
            }

            // get the page title
            if (HttpContext.Current.Request.QueryString["pageNumber"] != null)
            {
                int pi = int.Parse(HttpContext.Current.Request.QueryString["pageNumber"]);

                if (pageTitleArray.Length > 0 && pi <= pageTitleArray.Length)
                {
                    pageTitle = pageTitleArray[pi - 1];
                }
            }
            else
            {
                pageTitle = pageTitleArray[0];
            }

            // set the page title html
            StringBuilder result = new StringBuilder();
            string stepIndexFormat = LabelConvertUtil.GetTextByKey("ACA_BreadCrumpToolBar_StepIndex", string.Empty);
            string stepIndexString = string.Format(stepIndexFormat, info.StepIndexTitle);

            result.Append("<table role='presentation'><tr>");
            result.AppendFormat("<td>{0}</td>", stepIndexString);
            result.AppendFormat("<td>{0}</td>", ":");
            result.AppendFormat("<td>{0}</td>", info.Title);

            if (!string.IsNullOrEmpty(pageTitle))
            {
                result.AppendFormat("<td>{0}</td>", ">");
                result.AppendFormat("<td>{0}</td>", pageTitle);
            }

            result.Append("</tr></table>");

            return result.ToString();
        }

        /// <summary>
        /// Get the HTML of the collapsed breadcrumb block.
        /// </summary>
        /// <param name="stepIndex">The step index.</param>
        /// <param name="blockTip">The block's tip</param>
        /// <returns>
        /// Return the HTML of the collapsed breadcrumb block.
        /// </returns>
        private static string GetCollapsedBlock(int stepIndex, string blockTip)
        {
            StringBuilder sb = new StringBuilder();
            string title = string.Empty;

            if (!string.IsNullOrEmpty(blockTip))
            {
                title = string.Format("title='{0}'", blockTip);
            }

            sb.AppendFormat("<div class='breadcrumb_number_padding' {0}>", title);
            sb.Append("<span class='breadcrump-number breadcrump-number_fontsize'>");
            sb.Append(stepIndex);
            sb.Append("</span>");
            sb.Append("</div>");

            return sb.ToString();
        }

        /// <summary>
        /// Get the HTML of the expanded breadcrumb block.
        /// Expand block is contain step index and the name.
        /// </summary>
        /// <param name="stepIndex">The step index.</param>
        /// <param name="stepName">The step name.</param>
        /// <returns>Return the HTML of the expanded breadcrumb block.</returns>
        private static string GetExpandedBlock(int stepIndex, string stepName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table role='presentation' class='breadcrump-tt breadcrump-tt_fontsize'>");
            sb.Append("<tr>");
            sb.Append("<td class='spacecolumn'>");
            sb.Append("</td>");
            sb.Append("<td class='numbercolumn'>");
            sb.Append("<span class='breadcrump-number breadcrump-number_fontsize'>");
            sb.Append(stepIndex);
            sb.Append("</span>");
            sb.Append("</td>");
            sb.Append("<td class='namecolumn'>");
            sb.Append(stepName);
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");

            return sb.ToString();
        }

        /// <summary>
        /// get the CSS class that according to the state of breadcrumb.
        /// </summary>
        /// <param name="isExpand">is expand or not.</param>
        /// <param name="isActive">is active or not.</param>
        /// <param name="isLastStep">is last step or not.</param>
        /// <param name="isCurrentStep">is current step or not.</param>
        /// <returns>
        /// Return the different CSS class according to the state.
        /// </returns>
        private static string GetCssClass(bool isExpand, bool isActive, bool isLastStep, bool isCurrentStep)
        {
            string className = string.Empty;

            // set current step class
            if (isCurrentStep)
            {
                className = isLastStep ? "breadcrump-selected-end" : "breadcrump-selected";
                return className;
            }

            // set the non-current step class
            if (isActive)
            {
                if (isExpand)
                {
                    className = isLastStep ? "breadcrump-end" : "breadcrump";
                }
                else
                {
                    className = isLastStep ? "breadcrump-collapsed-end" : "breadcrump-collapsed";
                }
            }
            else
            {
                if (isExpand)
                {
                    className = isLastStep ? "breadcrump-disable-end" : "breadcrump-disable";
                }
                else
                {
                    className = isLastStep ? "breadcrump-collapsed-disable-end" : "breadcrump-collapsed-disable";
                }
            }

            return className;
        }

        /// <summary>
        /// Render the breadcrumb header.
        /// </summary>
        /// <param name="sb">The StringBuilder to append the HTML.</param>
        private void RenderHeader(StringBuilder sb)
        {
            // The CapName is use for the current title. 
            // It share the same CapName in all BreadCrumpInfo of PageFlow.
            string currentTitle = PageFlow[1].CapName;

            sb.Append("<div class='fontbold font14px ACA_Title_Color'>");
            sb.Append(currentTitle);
            sb.Append("</div>");
        }

        /// <summary>
        /// Render the breadcrumb body.
        /// </summary>
        /// <param name="sb">The StringBuilder to append the HTML.</param>
        /// <param name="curStepIndex">The current step index.</param>
        /// <param name="currentStepInfo">The current step info.</param>
        private void RenderBody(StringBuilder sb, int curStepIndex, out BreadCrumpInfo currentStepInfo)
        {
            // the max step index which acorssed
            int maxStepIndexAcrossed = 1;

            if (HttpContext.Current.Session[BreadCrumb] != null)
            {
                BreadCrumbParmsInfo info = (BreadCrumbParmsInfo)HttpContext.Current.Session[BreadCrumb];
                maxStepIndexAcrossed = info.LastIndex;

                if (info.IsFirstLoadFeeForm)
                {
                    info.IsFirstLoadFeeForm = false;
                    HttpContext.Current.Session[BreadCrumb] = info;
                }
            }

            int last = PageFlow.Count - curStepIndex;

            if (last == 0)
            {
                last = 2;
            }
            else if (last >= 2)
            {
                last = 0;
            }

            BreadCrumbParmsInfo hideApplicationSession = new BreadCrumbParmsInfo();
            if (HttpContext.Current.Session[BreadCrumb] != null)
            {
                hideApplicationSession = (BreadCrumbParmsInfo)HttpContext.Current.Session[BreadCrumb];
            }

            currentStepInfo = null;
            sb.Append("<table role='presentation' class='breadcrump-table'><tr>");

            for (int i = 0; i < PageFlow.Count; i++)
            {
                BreadCrumpInfo info = PageFlow[i + 1];

                string stepName = GetSubString(info.Title, MAX_CHARACTER_LENGTH);
                bool isExpanded = IsExpendStep(i + 1, curStepIndex, last, PageFlow.Count, hideApplicationSession.IsExpendAllStep);
                bool isLastStep = i == (PageFlow.Count - 1);
                bool isCurrentStep = info.StepIndex == curStepIndex;
                bool isActiveStep = false;

                if (IsForShoppingCart)
                {
                    // shopping cart's breadcrumb, it cannot navigate forward step, need validate by [Checkout] button.
                    isActiveStep = Enabled && info.Enable && (curStepIndex >= i + 1);
                }
                else
                {
                    isActiveStep = Enabled && info.Enable && (info.StepIndex <= curStepIndex);
                }

                string stepHtml = string.Empty;

                if (isCurrentStep)
                {
                    // 2.1 Set the current step HTML of breadcrumb
                    stepHtml = GetExpandedBlock(info.StepIndex, stepName);

                    currentStepInfo = info;
                }
                else if ((maxStepIndexAcrossed != PageFlow.Count || hideApplicationSession.IsHideApplicationForm) &&
                         !GetShoppingCartCondition(hideApplicationSession))
                {
                    // 2.2 Set the step HTML of breadcrumb
                    if (isExpanded)
                    {
                        if (isActiveStep && (!isLastStep || hideApplicationSession.IsHideApplicationForm))
                        {
                            stepName = string.Format("<a href='{0}'>{1}</a>", info.Url, stepName);
                        }

                        stepHtml = GetExpandedBlock(info.StepIndex, stepName);
                    }
                    else
                    {
                        // collapse step (inactive and active)
                        stepHtml = GetCollapsedBlock(info.StepIndex, stepName);
                    }
                }
                else
                {
                    // 2.3 Set the step HTML of breadcrumb
                    //current step is the last
                    if (isExpanded)
                    {
                        stepHtml = GetExpandedBlock(info.StepIndex, stepName);
                    }
                    else
                    {
                        stepHtml = GetCollapsedBlock(info.StepIndex, string.Empty);
                    }
                }

                // combine the HTML of step
                string stepCssClass = GetCssClass(isExpanded, isActiveStep, isLastStep, isCurrentStep);
                sb.AppendFormat("<td class='{0}'>", stepCssClass);
                sb.Append(stepHtml);
                sb.Append("</td>");
            }

            sb.Append("</tr></table>");
        }

        /// <summary>
        /// indicate the step is expend or not.
        /// </summary>
        /// <param name="index">index number.</param>
        /// <param name="curIndex">curIndex number.</param>
        /// <param name="last">last step index.</param>
        /// <param name="stepCount">The total step count.</param>
        /// <param name="isExpandAllSteps">if set to <c>true</c> [is expand all steps].</param>
        /// <returns> indicate the style of current step./// </returns>
        private bool IsExpendStep(int index, int curIndex, int last, int stepCount, bool isExpandAllSteps)
        {
            // the all steps expend, if step count less than/equal MAX_EXPAND_STEP_COUNT
            if (stepCount <= MAX_EXPAND_STEP_COUNT)
            {
                return true;
            }

            if (index < curIndex)
            {
                if ((index + 2) >= curIndex)
                {
                    return true;
                }
                else
                {
                    if (isExpandAllSteps)
                    {
                        return true;
                    }

                    if ((index + 2 + last) >= curIndex)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (index <= 5)
                {
                    return true;
                }
                else
                {
                    if (index - 2 <= curIndex)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 1.when the current user is super agency and staying at cap fee page, the former breadcrumb are not clicked.
        /// 2.when the current user is super agency,come from the shopping cart and where ever stay at cap confirm or 
        ///  cap fee, the former breadcrumb are not clicked.
        /// </summary>
        /// <param name="breadCrumbParmsInfo">bread Crumb Parameters information.</param>
        /// <returns>DisablePreviousSteps or not.</returns>
        private bool GetShoppingCartCondition(BreadCrumbParmsInfo breadCrumbParmsInfo)
        {
            return breadCrumbParmsInfo.IsSuperAgency
                   && (breadCrumbParmsInfo.IsCapFeePage || breadCrumbParmsInfo.IsConfirmPageFromShoppingCart);
        }

        /// <summary>
        /// Get substring 
        /// </summary>
        /// <param name="input">origin input string</param>
        /// <param name="length">the length of sub string</param>
        /// <returns>get the sub string.</returns>
        private string GetSubString(string input, int length)
        {
            int len = input.Length;
            int start = 0;
            int end = len;

            //the character less than 255 in ASCII(that means it's one single char)
            int single = 0;
            char[] chars = input.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (System.Convert.ToInt32(chars[i]) > 255)
                {
                    start += 2;
                }
                else
                {
                    start += 1;
                    single++;
                }

                if (start >= length)
                {
                    if (end % 2 == 0)
                    {
                        if (single % 2 == 0)
                        {
                            end = i + 1;
                        }
                        else
                        {
                            end = i;
                        }
                    }
                    else
                    {
                        end = i + 1;
                    }

                    break;
                }
            }

            string temp = input.Substring(0, end);
            return temp;
        }

        #endregion Methods
    }
}