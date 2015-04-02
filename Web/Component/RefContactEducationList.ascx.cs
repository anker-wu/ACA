#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefContactEducationList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefContactEducationList.ascx.cs 238264 2013-10-7 09:31:18Z ACHIEVO\alan.hu $.
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

using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Display the education list which look up by the ref contact.
    /// </summary>
    public partial class RefContactEducationList : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets Education data source.
        /// </summary>
        public IList<EducationModel4WS> GridViewDataSource
        {
            get
            {
                if (ViewState["EducationList"] == null)
                {
                    ViewState["EducationList"] = new List<EducationModel4WS>();
                }

                return ViewState["EducationList"] as IList<EducationModel4WS>;
            }

            set
            {
                ViewState["EducationList"] = value;
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// Gets the selected educations.
        /// </summary>
        /// <returns>Education Model</returns>
        public IList<EducationModel4WS> GetSelectedEducation()
        {
            IList<EducationModel4WS> educationModel = new List<EducationModel4WS>();

            if (!string.IsNullOrWhiteSpace(hdnRequiredFlags.Value))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                Dictionary<string, string> dicRequiredMapping = javaScriptSerializer.Deserialize<Dictionary<string, string>>(hdnRequiredFlags.Value);
                IEnumerable<KeyValuePair<string, string>> dicSelectedMapping = dicRequiredMapping.Where(f => ValidationUtil.IsYes(f.Value));

                foreach (var keyValuePair in dicSelectedMapping)
                {
                    educationModel.Add(GridViewDataSource.FirstOrDefault(o => o.educationPKModel.educationNbr == long.Parse(keyValuePair.Key)));
                }
            }

            return educationModel;
        }

        /// <summary>
        /// Bind education with data source.
        /// </summary>
        public void BindEducationList()
        {
            gdvEducationList.DataSource = GridViewDataSource;
            SetRequiredMappingData();

            //Display the required educations in the front. 
            gdvEducationList.Sort("requiredFlag", SortDirection.Ascending);
            gdvEducationList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvEducationList.DataBind();
        }

        /// <summary>
        /// <c>OnInit</c> event method
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (!AppSession.IsAdmin)
            {
                GridViewBuildHelper.SetHiddenColumn(gdvEducationList, new[] { "Action" });
            }

            GridViewBuildHelper.SetSimpleViewElements(gdvEducationList, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Education row bind event.
        /// </summary>
        /// <param name="sender">Event object.</param>
        /// <param name="e">Event arguments.</param>
        protected void EducationList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                EducationModel4WS education = e.Row.DataItem as EducationModel4WS;

                if (education == null)
                {
                    return;
                }

                HiddenField hfEduNbr = (HiddenField)e.Row.FindControl("hfEduNbr");
                AccelaLabel lblRequired = (AccelaLabel)e.Row.FindControl("lblRequired");
                AccelaLabel lblApproved = (AccelaLabel)e.Row.FindControl("lblApproved");
                hfEduNbr.Value = education.educationPKModel.educationNbr.ToString();
                lblRequired.Text = ModelUIFormat.FormatYNLabel(education.requiredFlag, true);
                lblApproved.Text = ModelUIFormat.FormatYNLabel(education.approvedFlag, true);

                AccelaDiv divLogo = (AccelaDiv)e.Row.FindControl("divLogo");
                var divComment = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.FindControl("commentPanel");

                //Education comment.
                string educationComment = education.comments;

                //validate comments is empty or null to hide logo.
                if (string.IsNullOrEmpty(educationComment))
                {
                    divLogo.Visible = false;
                }

                divComment.Visible = divLogo.Visible;

                var dataCell4Comment = divComment.Parent as DataControlFieldCell;

                //if Expand icon is invisible, set the cell container to invisible also for section 508
                if (dataCell4Comment != null && divLogo.Visible == false)
                {
                    dataCell4Comment.Visible = false;
                }

                //set Expand column visibile for section 508
                AccelaGridView currentGridView = sender as AccelaGridView;
                var dataCell4ExpandIcon = divLogo.Parent as DataControlFieldCell;

                if (currentGridView != null && dataCell4ExpandIcon != null && e.Row.RowIndex >= 0 && AccessibilityUtil.AccessibilityEnabled)
                {
                    if (e.Row.RowIndex == 0)
                    {
                        dataCell4ExpandIcon.ContainingField.Visible = false;
                    }

                    if (e.Row.RowIndex >= 0 && divLogo.Visible == true)
                    {
                        dataCell4ExpandIcon.ContainingField.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Set the required mapping data to hidden field.
        /// </summary>
        private void SetRequiredMappingData()
        {
            IDictionary<string, string> requiredMapping = new Dictionary<string, string>();

            foreach (EducationModel4WS edu in GridViewDataSource)
            {
                requiredMapping.Add(Convert.ToString(edu.educationPKModel.educationNbr), edu.requiredFlag);
            }

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            hdnRequiredFlags.Value = javaScriptSerializer.Serialize(requiredMapping);
        }

        #endregion Method
    }
}