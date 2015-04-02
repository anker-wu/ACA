#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExaminationEdit.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ExaminationEdit.ascx.cs 139167 2009-07-15 06:20:30Z ACHIEVO\jackie.yu $.
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
    /// Add,Edit,Delete Examination Information
    /// </summary>
    public partial class ExaminationEdit : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Gets or sets the examination section position.
        /// </summary>
        private EducationOrExamSectionPosition _examinationSectionPosition;

        /// <summary>
        /// Gets or sets the value which is indicating whether the contact from external
        /// </summary>
        private bool _contactIsFromExternal;

        /// <summary>
        /// DataSource Data Changed.
        /// </summary>
        public event CommonEventHandler DataChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        public bool IsEditable
        {
            get 
            {
                return Convert.ToBoolean(ViewState["IsEditable"]);
            }

            set 
            {
                ViewState["IsEditable"] = value;
                examinationList.IsEditable = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the contact from external.
        /// </summary>
        public bool ContactIsFromExternal
        {
            get
            {
                return _contactIsFromExternal;
            }

            set
            {
                _contactIsFromExternal = value;
                examinationList.ContactIsFromExternal = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether required the form.
        /// </summary>
        public bool IsRequired
        {
            get
            {
                return (bool)ViewState["IsRequired"];
            }

            set
            {
                ViewState["IsRequired"] = value;

                examinationList.IsRequired = value;
            }
        }

        /// <summary>
        /// Gets or sets Control DataSource
        /// </summary>
        public IList<ExaminationModel> DataSource
        {
            get
            {
                return examinationList.DataSource;
            }

            set
            {
                examinationList.DataSource = AppSession.IsAdmin ? null : value;
            } 
        }

        /// <summary>
        /// Gets or sets a the contact sequence number.
        /// </summary>
        public string ContactSeqNbr
        {
            get
            {
                return Convert.ToString(ViewState["ContactSeqNbr"]);
            }

            set
            {
                ViewState["ContactSeqNbr"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the examination section position.
        /// </summary>
        public EducationOrExamSectionPosition ExaminationSectionPosition
        {
            get
            {
                return _examinationSectionPosition;
            }
            
            set
            {
                _examinationSectionPosition = value;
                examinationList.ExaminationSectionPosition = value;

                switch (value)
                {
                    case EducationOrExamSectionPosition.AccountContactEdit:
                        liAddFromSaved.Visible = false;
                        btnAddNew.LabelKey = "aca_contact_examination_list_label_add_new";
                        break;
                    case EducationOrExamSectionPosition.CapEdit:
                        btnAddNew.LabelKey = "aca_examination_list_label_add_new";
                        break;
                    case EducationOrExamSectionPosition.CapConfirm:
                        divAddButton.Visible = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the examination model list.
        /// </summary>
        public IList<ExaminationModel> ExaminationModelList
        {
            get
            {
                return examinationList.DataSource;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is ref contact exist.
        /// </summary>
        private bool IsRefContactExist
        {
            get
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                return ContactUtil.IsRefContactExist(capModel);
            }
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Examination List Bind
        /// </summary>
        /// <param name="examinations">Examination Model</param>
        public void DisplayExamination(ExaminationModel[] examinations)
        {
            examinationList.DataSource = ObjectConvertUtil.ConvertArrayToList(examinations);
            examinationList.Bind();

            if (!IsEditable)
            {
                return;
            }

            GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
            {
                permissionLevel = GViewConstant.SECTION_EXAMINATION
            };

            string viewId = ExaminationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit
                                ? GviewID.RefContactExaminationEdit
                                : GviewID.ExaminationEdit;

            if (!RequiredValidationUtil.ValidateLicenseCertificationList(ModuleName, permission, viewId, this.examinationList.DataSource.ToArray()))
            {
                lblIncomplete.LabelKey = "examination_list_validate_required_error_message";
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
        /// Display add from saved notice.
        /// </summary>
        /// <param name="isSuccess">True when adding record successfully; false otherwise.</param>
        /// <param name="msg">The message.</param>
        public void DisplayAddFromSavedNotice(bool isSuccess, string msg)
        {
            examinationList.DisplayAddFromSavedNotice(isSuccess, msg);
        }

        /// <summary>
        /// Display delete action notice
        /// </summary>
        /// <param name="isSuccessfully">True when deleting record successfully; false otherwise.</param>
        public void DisplayDelActionNotice(bool isSuccessfully)
        {
            examinationList.DisplayDelActionNotice(isSuccessfully);
        }

        /// <summary>
        /// Update examination list.
        /// </summary>
        /// <param name="examinations">examination model list</param>
        public void UpdateExamList(IList<ExaminationModel> examinations)
        {
            DisplayExamination(examinations == null ? null : examinations.ToArray());
            editPanel.Update();
        }

        /// <summary>
        /// Disable or enable the Select from Contact button.
        /// </summary>
        /// <param name="isDisable">is disable or not.</param>
        public void DisableSelectFromContact(bool isDisable)
        {
            if (ExaminationSectionPosition != EducationOrExamSectionPosition.CapEdit)
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
                btnAddFromSaved.OnClientClick = string.Format("openEduExamLookUpDialog(this, '{0}',  '{1}');return false;", PageFlowConstant.SECTION_NAME_EXAMINATION, PageFlowComponent.EXAMINATION);
            }
        }

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">sender Object</param>
        /// <param name="e">Event Args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Examination List Selected, Deleted and etail Save
            examinationList.ExaminationDeleted += ExaminationList_ItemDeleted;
            
            if (ExaminationSectionPosition == EducationOrExamSectionPosition.CapConfirm)
            {
                examinationList.ExaminationSelected += Redirect_EditPage;
            }

            if (!IsPostBack && !AppSession.IsAdmin)
            {
                bool isFromAccountEdit = ExaminationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit;
                btnAddNew.OnClientClick = GetOpenExamFromDialogJs(isFromAccountEdit, "-1");

                if (isFromAccountEdit)
                {
                    AppSession.SetContactExaminationListToSession(ContactSeqNbr, examinationList.DataSource.ToArray());

                    if (string.IsNullOrEmpty(ContactSeqNbr))
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
                                                          componentID = (long)PageFlowComponent.EXAMINATION
                                                      };

            CapUtil.BackToPageContainSection(searchComponentModel, PageFlowConstant.SECTION_NAME_EXAMINATION, pageflowGroup, rowIndex);
        }

        /// <summary>
        /// Refresh the examination list when add or edit the ref contact/cap education..
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">event argument.</param>
        protected void RefreshExamListButton_Click(object sender, EventArgs e)
        {
            string postData = Request.Form[Page.postEventArgumentID];
            bool isUpdate = ValidationUtil.IsTrue(postData);

            if (ExaminationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit)
            {
                IExaminationBll examinationBll = ObjectFactory.GetObject<IExaminationBll>();
                examinationList.DataSource = ObjectConvertUtil.ConvertArrayToList(examinationBll.GetRefPeopleExamList(ContactSeqNbr));
            }
            else
            {
                CapModel4WS cap = AppSession.GetCapModelFromSession(ModuleName);

                if (cap.examinationList != null)
                {
                    examinationList.DataSource = ObjectConvertUtil.ConvertArrayToList(cap.examinationList);
                }
            }

            if (isUpdate)
            {
                examinationList.DisplayUpdateActionNotice(true);
            }
            else
            {
                examinationList.DisplayAddActionNotice(true);
            }

            examinationList.Bind();
            RaiseExaminationChangeEvent(sender);
            Page.FocusElement(Request.Form["__LASTFOCUS_ID"]);
        }

        /// <summary>
        /// Handle ExaminationsChanged event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        private void RaiseExaminationChangeEvent(object sender)
        {
            if (DataChanged != null && examinationList.DataSource != null)
            {
                DataChanged(sender, new CommonEventArgs(examinationList.DataSource.ToArray()));

                ExaminationModel[] examinations = examinationList.DataSource.ToArray();
                DisplayExamination(examinations);

                // Control status for button "Select from Contact" according to if the reference contact exists in current cap.
                DisableSelectFromContact(!IsRefContactExist);
            }
        }

        /// <summary>
        /// Delete a Examination from Examination List
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">common event args</param>
        private void ExaminationList_ItemDeleted(object sender, CommonEventArgs e)
        {
            IList<ExaminationModel> examinations = examinationList.DataSource;

            if (examinations != null)
            {
                //Update DataSource
                DataSource = examinations;

                //Send DataChanged Event
                RaiseExaminationChangeEvent(sender);
                editPanel.Update();
            }
        }

        /// <summary>
        /// Get the open examination form dialog script.
        /// </summary>
        /// <param name="isFromAccountEdit">is from AccountEdit.</param>
        /// <param name="rowIndex">row index.</param>
        /// <returns>script method.</returns>
        private string GetOpenExamFromDialogJs(bool isFromAccountEdit, string rowIndex)
        {
            return string.Format(
                "openExamFormDialog(this, '{0}', '{1}', '{2}', '{3}');return false;",
                isFromAccountEdit ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N,
                rowIndex,
                IsEditable ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N,
                ContactIsFromExternal ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N);
        }

        #endregion
    }
}
