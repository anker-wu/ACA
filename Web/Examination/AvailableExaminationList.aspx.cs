#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AvailableExaminationList.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ExamScheduleViewModel.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Examination
{
    /// <summary>
    /// Available Examination List
    /// </summary>
    public partial class AvailableExaminationList : ExaminationScheduleBasePage
    {
        #region Properties

        /// <summary>
        /// Gets or sets data source.
        /// </summary>
        private DataTable GridViewDataSource
        {
            get
            {
                string moduleName = string.IsNullOrEmpty(ModuleName) ? ACAConstant.DEFAULT_MODULE_NAME : ModuleName;

                if (ViewState[moduleName] != null)
                {
                    return (DataTable)ViewState[moduleName];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                string moduleName = string.IsNullOrEmpty(ModuleName) ? ACAConstant.DEFAULT_MODULE_NAME : ModuleName;

                ViewState[moduleName] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetPageTitleKey("aca_exam_schedule_title");
            this.SetDialogMaxHeight("600");
            
            if (!Page.IsPostBack)
            {
                if (!AppSession.IsAdmin)
                {
                    SetDataSource(ExaminationWizardParameter.ExaminationNbr);

                    SetContinueButtonStatus();

                    ////set no available examination notice//
                    lblNoInspectionTypesFound.Visible = GridViewDataSource.Rows.Count == 0;

                    lblScheduleExamination.Text = string.Format(GetTextByKey("aca_exam_schedule_availableexaminations_title"), GridViewDataSource.Rows.Count);

                    BindDataSource(GridViewDataSource, 0);
                }

                // set the admin UI
                SetAdminUI();
            }
        }

        /// <summary>
        /// Handles the Click event of the Continue Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdAvailableExamination.Value))
            {
                ExaminationParameter examinationParameter = ExaminationWizardParameter;
                examinationParameter.ExamScheduleProviderNbr = string.Empty;

                /* When the selected examination name was changed in schedule a pending examination process,
                 *  due to related template maybe different, so need to clear Template data, 
                 *  then retrieve the Template data from server in the examination schedule comfirm page.
                 */
                if (examinationParameter.ExaminationNbr != hdAvailableExamination.Value)
                {
                    examinationParameter.Template = null;
                }

                foreach (DataRow row in GridViewDataSource.Rows)
                {
                    if (row["ExaminationID"].ToString() == hdAvailableExamination.Value)
                    {
                        examinationParameter.ExaminationName = row["ExaminationName"].ToString();
                        examinationParameter.ExaminationNbr = row["ExaminationID"].ToString();
                    }
                }

                ExaminationScheduleUtil.ClearSearchParameter(examinationParameter);

                string url = "AvailableExaminationScheduleList.aspx";
                url = string.Format("{0}?{1}", url, Request.QueryString.ToString());
                url = ExaminationParameterUtil.UpdateURLAndSaveParameters(url, examinationParameter);

                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the <c>gvAvailableExamination</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void AvailableExamination_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AccelaRadioButton accelaRadioButton = e.Row.Cells[0].FindControl("rdAvailableExamination") as AccelaRadioButton;
                var dataRowView = e.Row.DataItem as DataRowView;
                string selectedexaminationid = hdAvailableExamination.Value;
                string examinationid = dataRowView == null ? string.Empty : dataRowView.Row["ExaminationID"].ToString();
                
                if (accelaRadioButton != null)
                {
                    if (selectedexaminationid == examinationid)
                    {
                        accelaRadioButton.Checked = true;
                    }
                    else
                    {
                        accelaRadioButton.Checked = false;
                    }

                    accelaRadioButton.Attributes.Add("onclick", "SelectExaminationRadio(this,'" + gvAvailableExamination.ClientID + "','" + hdAvailableExamination.ClientID + "');SetButtonStatus();");
                    accelaRadioButton.InputAttributes.Add("title", GetTextByKey("aca_selectonerecord_checkbox"));
                }
            }
        }

        /// <summary>
        /// Raises the page index changing event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.GridViewPageEventArgs object containing the event data.</param>
        protected void AvailableExamination_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int selectedRowIndex = e.NewPageIndex * gvAvailableExamination.PageSize;
            SetDataSource(hdAvailableExamination.Value);
            BindDataSource(GridViewDataSource, selectedRowIndex);

            SetContinueButtonStatus();
        }

        /// <summary>
        /// Sets the continue button status.
        /// </summary>
        private void SetContinueButtonStatus()
        {
            bool wizardButtonDisabled = string.IsNullOrEmpty(hdAvailableExamination.Value);
            SetWizardButtonDisable(lnkContinue.ClientID, wizardButtonDisabled);
        }

        /// <summary>
        /// Sets the admin UI.
        /// </summary>
        private void SetAdminUI()
        {
            if (AppSession.IsAdmin)
            {
            }
        }

        /// <summary>
        /// Binds the data source.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="selectedRowIndex">Index of the selected row.</param>
        private void BindDataSource(DataTable dataTable, int selectedRowIndex)
        {
            int pageIndex = 0;

            if (selectedRowIndex > 0)
            {
                pageIndex = selectedRowIndex / gvAvailableExamination.PageSize;
            }

            dataTable.DefaultView.Sort = "ExaminationName ASC";
            gvAvailableExamination.DataSource = dataTable.DefaultView;
            gvAvailableExamination.PageIndex = pageIndex;
            gvAvailableExamination.DataBind();
            gvAvailableExamination.Attributes.Add("PageCount", gvAvailableExamination.PageCount.ToString());
        }

        /// <summary>
        /// Sets the data source.
        /// </summary>
        /// <param name="selectExamiantion">The select examination.</param>
        private void SetDataSource(string selectExamiantion)
        {
            IRefExaminationBll refExamBll = (IRefExaminationBll)ObjectFactory.GetObject(typeof(IRefExaminationBll));
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            var refExaminationModel4WS = new RefExaminationModel4WS()
            {
                serviceProviderCode = ExaminationWizardParameter.RecordAgencyCode
            };
            refExaminationModel4WS.refExamAppTypeModels = new XRefExaminationAppTypeModel4WS[1];
            refExaminationModel4WS.refExamAppTypeModels[0] = new XRefExaminationAppTypeModel4WS()
            {
                category = capModel.capType.category,
                group = capModel.capType.group,
                serviceProviderCode = capModel.capType.serviceProviderCode,
                subType = capModel.capType.subType,
                type = capModel.capType.type
            };
            RefExaminationModel4WS[] refExamModels = refExamBll.GetRefExaminationListFilterByWFRestriction(refExaminationModel4WS, capModel.capDetailModel.capID, "DETAIL");

            GridViewDataSource = ExaminationScheduleUtil.ConvertRefExaminationModelToDataTable(refExamModels, selectExamiantion);

            if (!string.IsNullOrEmpty(selectExamiantion))
            {
                hdAvailableExamination.Value = selectExamiantion;
            }
        }

        #endregion
    }
}