#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EducationEdit.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EducationEdit.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
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
    /// Add/Edit/Delete Education information.
    /// </summary>
    public partial class EducationEdit : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Gets or sets the education section position.
        /// </summary>
        private EducationOrExamSectionPosition _educationSectionPosition;

        /// <summary>
        /// Add education event by clicking Save button
        /// </summary>
        public event CommonEventHandler EducationsChanged;

        #endregion Fields

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
                return Convert.ToString(ViewState["ContactSeqNbr"]);
            }
            
            set
            {
                ViewState["ContactSeqNbr"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the education section position.
        /// </summary>
        public EducationOrExamSectionPosition EducationSectionPosition
        {
            get
            {
                return _educationSectionPosition;
            }

            set
            {
                _educationSectionPosition = value;
                EducationList.EducationSectionPosition = value;

                switch (value)
                {
                    case EducationOrExamSectionPosition.AccountContactEdit:
                        liAddFromSaved.Visible = false;
                        btnAddNew.LabelKey = "aca_contact_education_list_label_add_new";
                        break;
                    case EducationOrExamSectionPosition.CapEdit:
                        btnAddNew.LabelKey = "aca_education_list_label_add_new";
                        break;
                    case EducationOrExamSectionPosition.CapConfirm:
                        divAddButton.Visible = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the education list data source.
        /// </summary>
        public IList<EducationModel4WS> EducationListDataSource
        {
            get
            {
                return EducationList.GridViewDataSource;
            }

            set
            {
                EducationList.GridViewDataSource = value;
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

        #endregion Properties

        #region Method

        /// <summary>
        /// Display education list.
        /// </summary>
        /// <param name="educations">education model list</param>
        public void DisplayEducations(EducationModel4WS[] educations)
        {
            IList<EducationModel4WS> educationViewList = null;

            if (!AppSession.IsAdmin)
            {
                EducationList.IsEditable = IsEditable;
                EducationList.ContactIsFromExternal = ContactIsFromExternal;
                educationViewList = ObjectConvertUtil.ConvertArrayToList(educations);
            }

            EducationList.GridViewDataSource = educationViewList;
            EducationList.BindEducations();

            ShowRequiredMessage(educations);
        }

        /// <summary>
        /// set the section required property
        /// </summary>
        /// <param name="isRequired">indicate if the section is required</param>
        public void SetSectionRequired(bool isRequired)
        {
            EducationList.SetGridViewRequired(isRequired);
        }

        /// <summary>
        /// Get education model list and set it to session.
        /// </summary>
        /// <returns>education model list</returns>
        public EducationModel4WS[] GetEducationModelList()
        {
            return EducationList.GridViewDataSource.ToArray();
        }

        /// <summary>
        /// Display add from saved notice.
        /// </summary>
        /// <param name="isSuccess">True when adding record successfully; false otherwise.</param>
        /// <param name="msg">The message.</param>
        public void DisplayAddFromSavedNotice(bool isSuccess, string msg)
        {
            EducationList.DisplayAddFromSavedNotice(isSuccess, msg);
        }

        /// <summary>
        /// Display delete action notice
        /// </summary>
        /// <param name="isSuccessfully">True when deleting record successfully; false otherwise.</param>
        public void DisplayDelActionNotice(bool isSuccessfully)
        {
            EducationList.DisplayDelActionNotice(isSuccessfully);
        }

        /// <summary>
        /// Update education list.
        /// </summary>
        /// <param name="educations">education model list</param>
        public void UpdateEduList(IList<EducationModel4WS> educations)
        {
            DisplayEducations(educations == null ? null : educations.ToArray());
            educationEditPanel.Update();
        }

        /// <summary>
        /// Disable or enable the Select from Contact button.
        /// </summary>
        /// <param name="isDisable">is disable or not.</param>
        public void DisableSelectFromContact(bool isDisable)
        {
            if (EducationSectionPosition != EducationOrExamSectionPosition.CapEdit)
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
                btnAddFromSaved.OnClientClick = string.Format("openEduExamLookUpDialog(this, '{0}', '{1}');return false;", PageFlowConstant.SECTION_NAME_EDUCATION, PageFlowComponent.EDUCATION);
            }
        }
       
        /// <summary>
        /// Page load event.
        /// </summary>
        /// <param name="sender">event object</param>
        /// <param name="e">event argument</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //load select and delete event.
            EducationList.EducationsDeleted += EducationList_EducationsDeleted;

            if (EducationSectionPosition == EducationOrExamSectionPosition.CapConfirm)
            {
                EducationList.EducationSelected += Redirect_EditPage;
            }

            if (!IsPostBack && !AppSession.IsAdmin)
            {
                bool isFromAccountEdit = EducationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit;
                btnAddNew.OnClientClick = GetOpenEducationFromDialogJs(isFromAccountEdit, "-1");

                if (isFromAccountEdit)
                {
                    if (!string.IsNullOrEmpty(ContactSeqNbr))
                    {
                        AppSession.SetContactEducationListToSession(ContactSeqNbr, EducationList.GridViewDataSource.ToArray());
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
                                                          componentID = (long)PageFlowComponent.EDUCATION
                                                      };

            CapUtil.BackToPageContainSection(searchComponentModel, PageFlowConstant.SECTION_NAME_EDUCATION, pageflowGroup, rowIndex);
        }

        /// <summary>
        /// Refresh the education list when add or edit the ref contact/cap education.
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">event argument.</param>
        protected void RefreshEduListButton_Click(object sender, EventArgs e)
        {
            string postData = Request.Form[Page.postEventArgumentID];
            bool isUpdate = ValidationUtil.IsTrue(postData);

            if (EducationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit)
            {
                ILicenseCertificationBll licenseCertificationBll = ObjectFactory.GetObject<ILicenseCertificationBll>();
                EducationList.GridViewDataSource =
                    ObjectConvertUtil.ConvertArrayToList(
                        TempModelConvert.ConvertToEducationModel4WS(
                            licenseCertificationBll.GetRefPeopleEduList(ContactSeqNbr)));
            }
            else
            {
                CapModel4WS cap = AppSession.GetCapModelFromSession(ModuleName);

                if (cap.educationList != null)
                {
                    EducationList.GridViewDataSource = ObjectConvertUtil.ConvertArrayToList(cap.educationList);
                }
            }

            if (isUpdate)
            {
                EducationList.DisplayUpdateActionNotice(true);
            }
            else
            {
                EducationList.DisplayAddActionNotice(true);
            }

            EducationList.BindEducations();
            RaiseEducationChangeEvent(sender);
            Page.FocusElement(Request.Form["__LASTFOCUS_ID"]);
        }

        /// <summary>
        /// Handle EducationsChanged event;delete education from education list.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="arg">A CommonEventArgs object.</param>
        private void EducationList_EducationsDeleted(object sender, CommonEventArgs arg)
        {
            if (EducationsChanged != null)
            {
                RaiseEducationChangeEvent(sender);
                educationEditPanel.Update();
            }
        }

        /// <summary>
        /// Handle EducationsChanged event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        private void RaiseEducationChangeEvent(object sender)
        {
            if (EducationsChanged != null && EducationList.GridViewDataSource != null)
            {
                EducationModel4WS[] educationModels = EducationList.GridViewDataSource.ToArray();
                EducationsChanged(sender, new CommonEventArgs(educationModels));
                ShowRequiredMessage(educationModels);

                // Control status for button "Select from Contact" according to if the reference contact exists in current cap.
                DisableSelectFromContact(!IsRefContactExist);
            }
        }

        /// <summary>
        /// Show required message when Education list exist no value of required field. 
        /// </summary>
        /// <param name="educations">Education model list.</param>
        private void ShowRequiredMessage(EducationModel4WS[] educations)
        {
            if (!IsEditable)
            {
                return;
            }

            GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
            {
                permissionLevel = GViewConstant.SECTION_EDUCATOIN
            };

            string viewId = EducationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit
                                ? GviewID.RefContactEducationEdit
                                : GviewID.EducationEdit;

            if (!RequiredValidationUtil.ValidateLicenseCertificationList(ModuleName, permission, viewId, educations))
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
        /// <param name="rowIndex">row index.</param>
        /// <returns>script method.</returns>
        private string GetOpenEducationFromDialogJs(bool isFromAccountEdit, string rowIndex)
        {
            return string.Format(
                "openEducationFormDialog(this, '{0}', '{1}', '{2}', '{3}');return false;",
                isFromAccountEdit ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N,
                rowIndex,
                IsEditable ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N,
                ContactIsFromExternal ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N);
        }

        #endregion Method
    }
}
