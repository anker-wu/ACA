#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContinuingEducationSummaryList.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ContinuingEducationSummaryList.ascx.cs 144714 2009-08-25 12:44:16Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.Education;

namespace Accela.ACA.Web.Education
{
    /// <summary>
    /// UC for Continuing Education Summary List
    /// </summary>
    public partial class ContinuingEducationSummaryList : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether is CAP detail page.
        /// </summary>
        public bool IsCapDetailPage
        {
            get
            {
                if (ViewState["IsCapDetailPage"] == null)
                {
                    ViewState["IsCapDetailPage"] = false;
                }

                return (bool)ViewState["IsCapDetailPage"];
            }

            set
            {
                ViewState["IsCapDetailPage"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Bind summary continuing education list.
        /// </summary>
        /// <param name="contEducations">ContinuingEducationModel4WS array</param>
        /// <param name="capType">CapType used to get the required continuing education hours.</param>
        public void BindSummaryContEducation(ContinuingEducationModel4WS[] contEducations, CapTypeModel capType = null)
        {
            IContinuingEducationBll contEducationBll = ObjectFactory.GetObject<IContinuingEducationBll>();
            capType = capType ?? this.GetCapTypeModel();
            IList<ContinuingEducationSummary> contEduSummaryList = contEducationBll.GetContEducationSummaryList(capType, contEducations);

            if (contEduSummaryList != null && contEduSummaryList.Count > 0)
            {
                double totalCompletedHours = contEduSummaryList.Sum(o => o.CompletedHours);
                double incompletedRequireCEHours =
                    contEduSummaryList.Where(o => o.IsRequireCE && o.RequiredHours > o.CompletedHours).Sum(
                        o => o.BalanceHours);

                IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();

                string totalRequiredHours = bizBll.GetValueForStandardChoice(
                    ConfigManager.AgencyCode,
                    BizDomainConstant.STD_CONTINUING_EDUCATION_REQUIRED_HOURS,
                    CAPHelper.GetCapTypeValue(capType));

                double remainHours = 0;
                double totalHours = string.IsNullOrEmpty(totalRequiredHours)
                                        ? 0
                                        : I18nNumberUtil.ParseNumberFromWebService(totalRequiredHours);

                if (!totalHours.Equals(0))
                {
                    remainHours = totalHours - totalCompletedHours;
                }

                if (incompletedRequireCEHours > 0 && incompletedRequireCEHours > remainHours)
                {
                    remainHours = incompletedRequireCEHours;
                }

                lblRemainingHoursNum.Text = I18nNumberUtil.FormatNumberForUI(remainHours < 0 ? 0 : remainHours);
            }
            
            gdvSummaryContEduList.DataSource = contEduSummaryList;
            gdvSummaryContEduList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvSummaryContEduList.DataBind();

            //set the empty column to invisible for section 508
            if (AccessibilityUtil.AccessibilityEnabled && gdvSummaryContEduList.Rows.Count > 0)
            {
                var divEmpty = gdvSummaryContEduList.Rows[0].FindControl("divEmpty");
                var dataCell4Empty = divEmpty == null ? null : divEmpty.Parent as DataControlFieldCell;

                if (dataCell4Empty != null)
                {
                    dataCell4Empty.ContainingField.Visible = false;
                }
            }

            if (!AppSession.IsAdmin)
            {
                ShowTotalRequiredHours(capType);
            }
        }

        /// <summary>
        /// <c>OnInit</c> event method
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvSummaryContEduList, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Page load event.
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">event argument.</param>
        protected void Page_Load(object sender, EventArgs e)
        {     
            if (!IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    gdvSummaryContEduList.ShowCaption = true;  
                }
            }
        }

        /// <summary>
        /// Show total required hours.
        /// </summary>
        /// <param name="capType">CapType used to get the required continuing education hours.</param>
        private void ShowTotalRequiredHours(CapTypeModel capType)
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            capType = capType ?? this.GetCapTypeModel();
            string totalRequiredHours = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, BizDomainConstant.STD_CONTINUING_EDUCATION_REQUIRED_HOURS, CAPHelper.GetCapTypeValue(capType));
            lblTotalNum.Text = !string.IsNullOrEmpty(totalRequiredHours) ? I18nNumberUtil.FormatNumberForUI(totalRequiredHours) : Convert.ToString(0);
        }

        /// <summary>
        /// get cap type model by model name.
        /// </summary>
        /// <returns>a CapTypeModel</returns>
        private CapTypeModel GetCapTypeModel()
        {
            CapTypeModel capType = new CapTypeModel();

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            if (capModel != null && capModel.capType != null)
            {
                capType = capModel.capType;
            }

            return capType;
        }

        #endregion Methods
    }
}
