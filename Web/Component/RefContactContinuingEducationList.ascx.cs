#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefContactContinuingEducationList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefContactContinuingEducationList.ascx.cs 238264 2013-10-7 09:31:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Display the continuing education list which look up by the ref contact.
    /// </summary>
    public partial class RefContactContinuingEducationList : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets continuing education data source.
        /// </summary>
        public IList<ContinuingEducationModel4WS> GridViewDataSource
        {
            get
            {
                if (ViewState["ContEducationList"] == null)
                {
                    ViewState["ContEducationList"] = new List<ContinuingEducationModel4WS>();
                }

                return ViewState["ContEducationList"] as IList<ContinuingEducationModel4WS>;
            }

            set
            {
                ViewState["ContEducationList"] = value;
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// Gets selected continuing educations.
        /// </summary>
        /// <returns>Continuing Education Model</returns>
        public IList<ContinuingEducationModel4WS> GetSelectedContEducation()
        {
            IList<ContinuingEducationModel4WS> contEducationModel = new List<ContinuingEducationModel4WS>();

            if (!string.IsNullOrWhiteSpace(hdnRequiredFlags.Value))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                Dictionary<string, string> dicRequiredMapping = javaScriptSerializer.Deserialize<Dictionary<string, string>>(hdnRequiredFlags.Value);
                IEnumerable<KeyValuePair<string, string>> dicSelectedMapping = dicRequiredMapping.Where(f => ValidationUtil.IsYes(f.Value));

                foreach (var keyValuePair in dicSelectedMapping)
                {
                    contEducationModel.Add(GridViewDataSource.FirstOrDefault(o => o.continuingEducationPKModel.contEduNbr == long.Parse(keyValuePair.Key)));
                }
            }

            return contEducationModel;
        }

        /// <summary>
        /// Bind continuing education with data source.
        /// </summary>
        public void BindEducationList()
        {
            gdvContEducationList.DataSource = GridViewDataSource;
            SetRequiredMappingData();

            //Display the required continuing education in the front. 
            gdvContEducationList.Sort("requiredFlag", SortDirection.Ascending);
            gdvContEducationList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvContEducationList.DataBind();
        }

        /// <summary>
        /// <c>OnInit</c> event method
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (!AppSession.IsAdmin)
            {
                GridViewBuildHelper.SetHiddenColumn(gdvContEducationList, new[] { "Action" });
            }

            GridViewBuildHelper.SetSimpleViewElements(gdvContEducationList, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Continuing education row bind event.
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">event arguments.</param>
        protected void ContEducationList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ContinuingEducationModel4WS contEducation = e.Row.DataItem as ContinuingEducationModel4WS;

                if (contEducation == null)
                {
                    return;
                }

                HiddenField hfContEduNbr = (HiddenField)e.Row.FindControl("hfContEduNbr");
                AccelaLabel lblRequired = (AccelaLabel)e.Row.FindControl("lblRequired");
                AccelaLabel lblApproved = (AccelaLabel)e.Row.FindControl("lblApproved");
                hfContEduNbr.Value = contEducation.continuingEducationPKModel.contEduNbr.ToString();
                lblRequired.Text = ModelUIFormat.FormatYNLabel(contEducation.requiredFlag, true);
                lblApproved.Text = ModelUIFormat.FormatYNLabel(contEducation.approvedFlag, true);

                AccelaDiv divLogo = (AccelaDiv)e.Row.FindControl("divLogo");
                HiddenField hdnGradingStyle = (HiddenField)e.Row.FindControl("hdnGradingStyle");
                AccelaLabel lblFinalScore = (AccelaLabel)e.Row.FindControl("lblFinalScore");

                //validate comments is empty or null to hide logo.
                if (string.IsNullOrEmpty(contEducation.comments))
                {
                    divLogo.Visible = false;
                }

                var divComment = e.Row.FindControl("commentPanel");
                var dataCell4Comment = divComment == null ? null : divComment.Parent as DataControlFieldCell;

                //if Expand icon is invisible, set the cell container to invisible also for section 508
                if (dataCell4Comment != null && divLogo != null && divLogo.Visible == false)
                {
                    dataCell4Comment.Visible = false;
                }

                //set Expand column visibile for section 508
                AccelaGridView currentGridView = sender as AccelaGridView;
                var dataCell4ExpandIcon = divLogo == null ? null : divLogo.Parent as DataControlFieldCell;

                if (currentGridView != null && dataCell4ExpandIcon != null && e.Row.RowIndex >= 0 && AccessibilityUtil.AccessibilityEnabled)
                {
                    if (e.Row.RowIndex == 0)
                    {
                        dataCell4ExpandIcon.ContainingField.Visible = false;
                    }

                    if (e.Row.RowIndex >= 0 && divLogo.Visible)
                    {
                        dataCell4ExpandIcon.ContainingField.Visible = true;
                    }
                }

                //format display final socre.
                if (hdnGradingStyle != null && lblFinalScore != null)
                {
                    lblFinalScore.Text = EducationUtil.FormatScore(hdnGradingStyle.Value, lblFinalScore.Text);
                }
            }
        }

        /// <summary>
        /// Set the required mapping data to hidden field.
        /// </summary>
        private void SetRequiredMappingData()
        {
            IDictionary<string, string> requiredMapping = new Dictionary<string, string>();

            foreach (ContinuingEducationModel4WS contEdu in GridViewDataSource)
            {
                requiredMapping.Add(Convert.ToString(contEdu.continuingEducationPKModel.contEduNbr), ACAConstant.COMMON_N);
            }

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            hdnRequiredFlags.Value = javaScriptSerializer.Serialize(requiredMapping);
        }

        #endregion Method
    }
}