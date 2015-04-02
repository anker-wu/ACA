#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContinuingEducationEdit.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ContinuingEducationEdit.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Add/Edit/Delete continuing education information.
    /// </summary>
    public partial class ContinuingEducationEdit : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Indicate continuing education section whether can editable.
        /// </summary>
        private bool _isEditable = true;

        /// <summary>
        /// Gets or sets the education section position.
        /// </summary>
        private EducationOrExamSectionPosition _contEducationSectionPosition;

        /// <summary>
        /// Add continuing education event by clicking Save button
        /// </summary>
        public event CommonEventHandler ContEducationsChanged;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        public bool IsEditable
        {
            get
            {
                return _isEditable;
            }

            set
            {
                _isEditable = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the contact from external.
        /// </summary>
        public bool ContactIsFromExternal
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a the contact sequence number.
        /// </summary>
        public string ContactSeqNbr
        {
            get
            {
                return ViewState["ContactSeqNbr"] as string;
            }

            set
            {
                ViewState["ContactSeqNbr"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the education section position.
        /// </summary>
        public EducationOrExamSectionPosition ContEducationSectionPosition
        {
            get
            {
                return _contEducationSectionPosition;
            }

            set
            {
                _contEducationSectionPosition = value;
                ContEducationList.ContEducationSectionPosition = value;

                switch (value)
                {
                    case EducationOrExamSectionPosition.AccountContactEdit:
                        liAddFromSaved.Visible = false;
                        divSummaryList.Visible = false;
                        btnAddNew.LabelKey = "aca_contact_continuing_education_list_label_add_new";
                        break;
                    case EducationOrExamSectionPosition.CapEdit:
                        btnAddNew.LabelKey = "aca_continuing_education_list_label_add_new";
                        break;
                    case EducationOrExamSectionPosition.CapConfirm:
                        liAddFromSaved.Visible = false;
                        liAddNew.Visible = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether is ref contact exist or not.
        /// </summary>
        private bool IsRefContactExist
        {
            get
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                return ContactUtil.IsRefContactExist(capModel);
            }
        }

        #endregion Properties

        #region Method

        /// <summary>
        /// Display continuing education.
        /// </summary>
        /// <param name="contEducations">continuing education models</param>
        public void DislayContEducations(ContinuingEducationModel4WS[] contEducations)
        {
            IList<ContinuingEducationModel4WS> contEducationList = null;

            if (!AppSession.IsAdmin)
            {
                ContEducationList.IsEditable = IsEditable;
                ContEducationList.ContactIsFromExternal = ContactIsFromExternal;
                contEducationList = ObjectConvertUtil.ConvertArrayToList(contEducations);
            }

            // binding summary continuing education list.
            BindContEducationSummary(contEducations);

            ContEducationList.GridViewDataSource = contEducationList;
            ContEducationList.BindContEducations();

            ShowRequiredMessage(contEducations);
        }

        /// <summary>
        /// Set continuing education section required property.
        /// </summary>
        /// <param name="isRequired">indicate if the section is required</param>
        public void SetContEducationSectionRequired(bool isRequired)
        {
            ContEducationList.SetGridViewRequired(isRequired);
        }

        /// <summary>
        /// Get continuing education models from section list.
        /// </summary>
        /// <returns>continuing education models</returns>
        public ContinuingEducationModel4WS[] GetContEducations()
        {
            ContinuingEducationModel4WS[] contEducations = ContEducationList.GetContEducationList();

            return contEducations;
        }

        /// <summary>
        /// Display add from saved notice.
        /// </summary>
        /// <param name="isSuccess">True when adding record successfully; false otherwise.</param>
        /// <param name="msg">The message.</param>
        public void DisplayAddFromSavedNotice(bool isSuccess, string msg)
        {
            ContEducationList.DisplayAddFromSavedNotice(isSuccess, msg);
        }

        /// <summary>
        /// Display delete action notice.
        /// </summary>
        /// <param name="isSuccessfully">True when deleting record successfully; false otherwise.</param>
        public void DisplayDelActionNotice(bool isSuccessfully)
        {
            ContEducationList.DisplayDelActionNotice(isSuccessfully);
        }

        /// <summary>
        /// Disable or enable the Select from Contact button.
        /// </summary>
        /// <param name="isDisable">is disable or not.</param>
        public void DisableSelectFromContact(bool isDisable)
        {
            if (ContEducationSectionPosition != EducationOrExamSectionPosition.CapEdit)
            {
                return;
            }

            if (isDisable)
            {
                btnAddFromSaved.OnClientClick = string.Empty;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "disableBtn" + Guid.NewGuid(), "SetWizardButtonDisable('" + btnAddFromSaved.ClientID + "',true);", true);
            }
            else
            {
                btnAddFromSaved.OnClientClick = string.Format("openEduExamLookUpDialog(this, '{0}', '{1}');return false;", PageFlowConstant.SECTION_NAME_CONT_EDUCATION, PageFlowComponent.CONTINUING_EDUCATION);
            }
        }

        /// <summary>
        /// Page load event.
        /// </summary>
        /// <param name="sender">event object</param>
        /// <param name="e">event argument</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && !AppSession.IsAdmin)
            {
                // Hide incomplete mark logo and instruction information in CAP detail.
                if (ContEducationList.ContEducationSectionPosition == EducationOrExamSectionPosition.CapDetail)
                {
                    divIncompleteMark.Visible = false;
                }

                bool isFromAccountContactEdit = ContEducationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit;
                btnAddNew.OnClientClick = GetOpenContEducationFromDialogJs(isFromAccountContactEdit, "new", "-1");

                if (isFromAccountContactEdit)
                {
                    if (!string.IsNullOrEmpty(ContactSeqNbr))
                    {
                        AppSession.SetContactContEducationListToSession(ContactSeqNbr, ContEducationList.GridViewDataSource.ToArray());
                    }
                    else
                    {
                        divAddButton.Visible = false;
                    }
                }
                else if (!PageFlowUtil.IsContactComponentExist())
                {
                    liAddFromSaved.Visible = false;
                }
                else
                {
                    DisableSelectFromContact(!IsRefContactExist);
                }

                if (!IsEditable)
                {
                    divAddButton.Visible = false;
                }
            }

            // load select and delete event.
            ContEducationList.ContEducationsDeleted += new CommonEventHandler(ContEducation_Deleted);

            if (ContEducationSectionPosition == EducationOrExamSectionPosition.CapConfirm)
            {
                ContEducationList.ContEducationSelected += new CommonEventHandler(Redirect_EditPage);
            }
        }

        /// <summary>
        /// Redirect to the specified spear form in Cap Edit page when clicking list in Cap confirm page.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="arg">A System.EventArgs object containing the event data.</param>
        protected void Redirect_EditPage(object sender, CommonEventArgs arg)
        {
            string rowIndex = string.Empty;
            if (arg != null && arg.ArgObject != null && (arg.ArgObject is Array))
            {
                object[] args = (object[])arg.ArgObject;
                rowIndex = Convert.ToString(args[0]);
            }

            PageFlowGroupModel pageflowGroup = AppSession.GetPageflowGroupFromSession();
            ComponentModel searchComponentModel = new ComponentModel
                                                      {
                                                          componentID = (long)PageFlowComponent.CONTINUING_EDUCATION
                                                      };

            CapUtil.BackToPageContainSection(searchComponentModel, PageFlowConstant.SECTION_NAME_CONT_EDUCATION, pageflowGroup, rowIndex);
        }

        /// <summary>
        /// Refresh the education list event.
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">event argument.</param>
        protected void RefreshContEduListButton_Click(object sender, EventArgs e)
        {
            string postArg = Request.Form[Page.postEventArgumentID];
            bool isUpdate = ValidationUtil.IsTrue(postArg);

            if (isUpdate)
            {
                ContEducationList.DisplayUpdateActionNotice(true);
            }
            else
            {
                ContEducationList.DisplayAddActionNotice(true);
            }

            if (ContEducationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit)
            {
                ILicenseCertificationBll licenseCertificationBll = ObjectFactory.GetObject<ILicenseCertificationBll>();
                ContEducationList.GridViewDataSource = ObjectConvertUtil.ConvertArrayToList(TempModelConvert.ConvertToContEducationModel4WS(licenseCertificationBll.GetRefPeopleContEduList(ContactSeqNbr)));
            }
            else
            {
                ContEducationList.GridViewDataSource = ObjectConvertUtil.ConvertArrayToList(AppSession.GetContactContEducationListFromSession(ContactSeqNbr));
            }

            ContEducationList.BindContEducations();
            RaiseContEducationChangeEvent(sender);
            Page.FocusElement(Request.Form["__LASTFOCUS_ID"]);
        }

        /// <summary>
        /// Handle ContinuingEducationsChanged event;delete continuing education from
        /// continuing education list.
        /// </summary>
        /// <param name="sender">an object that contains the event sender.</param>
        /// <param name="arg">a CommonEventArgs object.</param>
        private void ContEducation_Deleted(object sender, CommonEventArgs arg)
        {
            if (ContEducationsChanged != null)
            {
                RaiseContEducationChangeEvent(sender);
                updatePanel.Update();
            }
        }

        /// <summary>
        /// Handle EducationsChanged event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        private void RaiseContEducationChangeEvent(object sender)
        {
            if (ContEducationsChanged != null && ContEducationList.GridViewDataSource != null)
            {
                ContinuingEducationModel4WS[] contEducations = ContEducationList.GridViewDataSource.ToArray();
                ContEducationsChanged(sender, new CommonEventArgs(contEducations));

                // binding summary continuing education list.
                BindContEducationSummary(contEducations);

                ShowRequiredMessage(contEducations);

                // Control status for button "Select from Contact" according to if the reference contact exists in current cap.
                DisableSelectFromContact(!IsRefContactExist);
            }
        }

        /// <summary>
        /// Bind continuing education summary information.
        /// </summary>
        /// <param name="contEducations">continuing education models</param>
        private void BindContEducationSummary(ContinuingEducationModel4WS[] contEducations)
        {
            // binding summary continuing education list.
            if (ContEducationSectionPosition != EducationOrExamSectionPosition.AccountContactEdit)
            {
                SummaryContEducation.BindSummaryContEducation(contEducations);
            }
        }

        /// <summary>
        /// Show required message when continuing education list exist no value of required field. 
        /// </summary>
        /// <param name="contEducations">continuing Education models</param>
        private void ShowRequiredMessage(ContinuingEducationModel4WS[] contEducations)
        {
            if (!IsEditable)
            {
                return;
            }

            GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
            {
                permissionLevel = GViewConstant.SECTION_CONTINUING_EDUCATION
            };

            string viewId = ContEducationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit
                                ? GviewID.RefContactContinuingEducationEdit
                                : GviewID.ContinuingEducationEdit;

            if (!RequiredValidationUtil.ValidateLicenseCertificationList(ModuleName, permission, viewId, contEducations))
            {
                divIncompleteMark.Visible = true;

                if (AccessibilityUtil.AccessibilityEnabled)
                {
                    string errorMessage = LabelUtil.GetTextByKey(lblIncomplete.LabelKey, this.ModuleName);
                    ltScriptForIncomplete.Text = MessageUtil.GetAlertScript(errorMessage);
                }
            }
            else
            {
                divIncompleteMark.Visible = false;
            }
        }

        /// <summary>
        /// Get the open education form dialog script.
        /// </summary>
        /// <param name="isFromAccountEdit">is from AccountEdit.</param>
        /// <param name="opt">opt: new or edit.</param>
        /// <param name="eduNbr">education number.</param>
        /// <returns>script method.</returns>
        private string GetOpenContEducationFromDialogJs(bool isFromAccountEdit, string opt, string eduNbr)
        {
            return string.Format(
                "openContEducationFormDialog(this, '{0}', '{1}', '{2}', '{3}', '{4}');return false;",
                isFromAccountEdit ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N,
                opt,
                eduNbr,
                IsEditable ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N,
                ContactIsFromExternal ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N);
        }
        
        #endregion Method
    }
}
