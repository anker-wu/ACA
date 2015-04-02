#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExaminationEdit.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ExaminationEdit.aspx.cs 238264 2013-10-7 09:31:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using log4net;

namespace Accela.ACA.Web.LicenseCertification
{
    /// <summary>
    /// Examination edit page
    /// </summary>
    public partial class ExaminationEdit : PopupDialogBasePage
    {
        #region Fields

        /// <summary>
        /// indicate the default index value when no examination is selected in the examination list.
        /// </summary>
        private const int NO_EXAMINATION_SELECTED = -1;

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(ExaminationEdit));

        /// <summary>
        /// Gets or sets the education row index at the education list.
        /// </summary>
        private int _rowIndex = 0;

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
                examinationDetailEdit.IsEditable = value;
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
        /// Gets or sets the contact sequence number.
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
        /// Gets or sets a value indicating whether this page is open from reference contact edit page.
        /// </summary>
        public bool IsFromRefContact
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsFromRefContact"]);
            }

            set
            {
                ViewState["IsFromRefContact"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Examination list.
        /// </summary>
        private IList<ExaminationModel> ExaminationModelList
        {
            get
            {
                if (ViewState["Examinations"] == null)
                {
                    return new List<ExaminationModel>();
                }

                return ViewState["Examinations"] as IList<ExaminationModel>;
            }

            set
            {
                ViewState["Examinations"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is add Examination.
        /// </summary>
        private bool IsAddExamination
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsAddExamination"]);
            }

            set
            {
                ViewState["IsAddExamination"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is update Education.
        /// </summary>
        private bool IsUpdate
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsAddEducation"]);
            }

            set
            {
                ViewState["IsAddEducation"] = value;
            }
        }

        #endregion Properties

        #region Method

        /// <summary>
        /// Change Form Layout.
        /// </summary>
        /// <param name="refExamId">The ref exam id.</param>
        public void ChangeFormLayout(string refExamId)
        {
            examinationDetailEdit.SetPermissionValue(refExamId);
            examinationDetailEdit.DisplayGenericTemplate(refExamId);
        }

        /// <summary>
        /// Page load event.
        /// </summary>
        /// <param name="sender">event object</param>
        /// <param name="e">event argument</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            IsFromRefContact = Request.QueryString.AllKeys.Contains("contactSeqNbr");
            ContactIsFromExternal = ValidationUtil.IsYes(Request.QueryString[UrlConstant.CONTACT_IS_FROM_EXTERNAL]);
            examinationDetailEdit.ContactIsFromExternal = ContactIsFromExternal;
            SetDialogMaxHeight("600");
            ContactSeqNbr = Request.QueryString["contactSeqNbr"];

            if (IsFromRefContact)
            {
                examinationDetailEdit.ExaminationSectionPosition = EducationOrExamSectionPosition.AccountContactEdit;
            }
            else
            {
                examinationDetailEdit.ExaminationSectionPosition = EducationOrExamSectionPosition.CapEdit;
            }

            int.TryParse(Request.QueryString[UrlConstant.ROW_INDEX], out _rowIndex);
            IsUpdate = _rowIndex > NO_EXAMINATION_SELECTED;
            examinationDetailEdit.DataChanged += new CommonEventHandler(ExaminationChanged);
            string titleKey = IsFromRefContact ? "aca_contact_examination_edit_label_title" : "aca_examination_edit_label_title";

            if (!AppSession.IsAdmin)
            {
                this.SetPageTitleKey(titleKey);
            }

            if (!IsPostBack)
            {
                DialogUtil.RegisterScriptForDialog(this.Page);
                IsEditable = IsFromRefContact || ValidationUtil.IsYes(Request.QueryString["editable"]);

                if (IsFromRefContact)
                {
                    ExaminationModelList =
                        ObjectConvertUtil.ConvertArrayToList(
                            AppSession.GetContactExaminationListFromSession(ContactSeqNbr));
                }
                else
                {
                    CapModel4WS cap = AppSession.GetCapModelFromSession(ModuleName);

                    if (cap != null)
                    {
                        ExaminationModelList = ObjectConvertUtil.ConvertArrayToList(cap.examinationList);
                    }
                }

                if (!AppSession.IsAdmin)
                {
                    if (!IsEditable || ContactIsFromExternal)
                    {
                        examinationDetailEdit.DisableExamination();
                    }

                    if (IsUpdate)
                    {
                        examinationDetailEdit.DataSource = ExaminationModelList.First(f => f.RowIndex == _rowIndex);
                    }
                }
                else
                {
                    sectionTitleBar.Visible = true;
                }
            }

            if (AppSession.IsAdmin)
            {
                AccelaDropDownList dropDownList = new AccelaDropDownList();
                dropDownList.ID = "examinationtype";

                IRefExaminationBll refExaminationBll = ObjectFactory.GetObject<IRefExaminationBll>();
                RefExaminationModel4WS[] refExaminations = refExaminationBll.GetRefExaminationList(
                        new RefExaminationModel4WS() { serviceProviderCode = ConfigManager.AgencyCode });
                List<ListItem> listItems = new List<ListItem>();

                if (refExaminations != null && refExaminations.Length != 0)
                {
                    foreach (var obj in refExaminations)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = obj.examName;
                        listItem.Value = obj.refExamNbr.ToString();
                        listItems.Add(listItem);
                    }
                }

                dropDownList.AutoPostBack = false;
                dropDownList.Attributes.Add("onfocus", "javascript:if(typeof(parent.GetOldValueBeforeValueChange)!='undefined'){parent.GetOldValueBeforeValueChange(this);}");
                dropDownList.Attributes.Add("onchange", "javascript:if(typeof(parent.ChangedItemsValidateBeforePostBack)!='undefined'){parent.ChangedItemsValidateBeforePostBack(this,window);}");

                dropDownList.SelectedIndexChanged += (obj, arg) =>
                {
                    AccelaDropDownList senderControl = obj as AccelaDropDownList;
                    ChangeFormLayout(senderControl.SelectedValue);
                };

                DropDownListBindUtil.BindDDL(listItems, dropDownList);
                string viewId = IsFromRefContact ? GviewID.RefContactExaminationEdit : GviewID.ExaminationEdit;
                sectionTitleBar.Visible = true;
                sectionTitleBar.AddToolBarControls(dropDownList);
                sectionTitleBar.PermissionValueId = dropDownList.ClientID;
                sectionTitleBar.SectionID = string.Format("{0}" + ACAConstant.SPLIT_CHAR + "{1}" + ACAConstant.SPLIT_CHAR + "{2}", ModuleName, viewId, examinationDetailEdit.ClientID + "_");
                sectionTitleBar.LabelKey = titleKey;
            }
        }

        /// <summary>
        /// Triggered after examination saved
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="arg">A CommonEventArgs object containing the examination models.</param>
        private void ExaminationChanged(object sender, CommonEventArgs arg)
        {
            if (arg == null || arg.ArgObject == null)
            {
                return;
            }

            ExaminationModel examinationModel = examinationDetailEdit.DataSource;
            bool isSuccess = true;

            if (!IsUpdate)
            {
                examinationModel.approvedFlag = ACAConstant.COMMON_N;
            }
                
            try
            {
                if (!IsValidExamination(IsUpdate, examinationModel))
                {
                    return;
                }

                if (IsFromRefContact)
                {
                    IExaminationBll examinationBll = ObjectFactory.GetObject<IExaminationBll>();

                    if (!ExaminationUtil.IsPassedExamination(examinationModel.gradingStyle, examinationModel.finalScore, examinationModel.passingScore))
                    {
                        ShowNotice(string.Format(GetTextByKey("aca_register_education_exam_to_contact_msg_uncompleted"), examinationModel.examName));
                        return;
                    }

                    if (IsUpdate)
                    {
                        isSuccess = examinationBll.AddOrUpdateRefPeopleExam(examinationModel);
                    }
                    else
                    {
                        examinationModel.contactSeqNumber = long.Parse(ContactSeqNbr);
                        examinationModel.entityType = ACAConstant.REF_CONTACT_EXAMINATION_ENTITY_TYPE;
                        isSuccess = examinationBll.AddOrUpdateRefPeopleExam(examinationModel);
                    }
                }
                else
                {
                    if (IsUpdate)
                    {
                        var tempExams = ExaminationModelList.FirstOrDefault(o => _rowIndex == o.RowIndex);

                        if (tempExams != null
                            && tempExams.entityID != null
                            && (!ACAConstant.EXAMINATION_STATUS_COMPLETED_PENDING.Equals(examinationModel.examStatus)
                                || !ExaminationUtil.IsPassedExamination(examinationModel.gradingStyle, examinationModel.finalScore, examinationModel.passingScore)))
                        {
                            MessageUtil.ShowMessageInPopupScrollTop(Page, MessageType.Error, GetTextByKey("aca_examination_label_restriction_message"));
                            return;
                        }

                        int index = ExaminationModelList.IndexOf(tempExams);
                        ExaminationModelList[index] = examinationModel;
                    }
                    else
                    {
                        List<ExaminationModel> tmpExaminationList = new List<ExaminationModel>();

                        if (ExaminationModelList != null)
                        {
                            tmpExaminationList.AddRange(ExaminationModelList);
                        }

                        examinationModel.entityType = ACAConstant.CAP_EXAMINATION_ENTITY_TYPE;
                        tmpExaminationList.Add(examinationModel);
                        ExaminationModelList = tmpExaminationList;
                    }

                    CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                    capModel.examinationList = ExaminationModelList.ToArray();
                }

                if (isSuccess)
                {
                    AppSession.SetContactExaminationListToSession(ContactSeqNbr, ExaminationModelList.ToArray());
                    string script = "CloseExamDialog(" + IsUpdate.ToString().ToLower() + ");";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SaveExam", script, true);
                }
            }
            catch (ACAException ex)
            {
                Logger.Error(ex);
                MessageUtil.ShowMessageInPopupScrollTop(Page, MessageType.Error, ex.Message);
            }
        }
        
        /// <summary>
        /// Check the examination is valid or not
        /// </summary>
        /// <param name="isUpdate">is update</param>
        /// <param name="examModel">examination model</param>
        /// <returns>true or false</returns>
        private bool IsValidExamination(bool isUpdate, ExaminationModel examModel)
        {
            bool isValid = true;

            if (ACAConstant.EXAMINATION_STATUS_PENDING.Equals(examModel.examStatus, StringComparison.InvariantCultureIgnoreCase) && ExaminationModelList != null)
            {
                List<ExaminationModel> checkList = isUpdate ? ExaminationModelList.Where(o => o.RowIndex != examModel.RowIndex
                    && o.examStatus.Equals(examModel.examStatus, StringComparison.InvariantCultureIgnoreCase)
                    && o.examName.Equals(examModel.examName, StringComparison.InvariantCultureIgnoreCase)).ToList() :
                    ExaminationModelList.Where(o => o.examStatus.Equals(examModel.examStatus, StringComparison.InvariantCultureIgnoreCase)
                        && o.examName.Equals(examModel.examName, StringComparison.InvariantCultureIgnoreCase)).ToList();

                if (checkList != null && checkList.Count > 0)
                {
                    ShowNotice(string.Format(GetTextByKey("examination_edit_add_examination_name_check"), examModel.examName));
                    return false;
                }

                if (examModel.endTime != null)
                {
                    if (examModel.endTime < DateTime.Now)
                    {
                        ShowNotice(GetTextByKey("aca_examination_edit_check_datatime_later"));
                        return false;
                    }
                }
                else
                {
                    if (examModel.examDate != null && examModel.examDate < DateTime.Now)
                    {
                        ShowNotice(GetTextByKey("aca_examination_edit_check_datatime_later"));
                        return false;
                    }
                }
            }

            if (ACAConstant.EXAMINATION_STATUS_COMPLETED_PENDING.Equals(examModel.examStatus, StringComparison.InvariantCultureIgnoreCase)
                || ACAConstant.EXAMINATION_STATUS_COMPLETED.Equals(examModel.examStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                if (examModel.endTime != null)
                {
                    if (examModel.endTime > DateTime.Now)
                    {
                        ShowNotice(GetTextByKey("aca_examination_edit_check_datatime_before"));
                        return false;
                    }
                }
                else
                {
                    if (examModel.examDate != null && examModel.examDate > DateTime.Now)
                    {
                        ShowNotice(GetTextByKey("aca_examination_edit_check_datatime_before"));
                        return false;
                    }
                }
            }

            if (examModel.endTime != null && examModel.startTime != null && examModel.endTime <= examModel.startTime)
            {
                ShowNotice(GetTextByKey("aca_examination_edit_check_starttime_endtime"));
                return false;
            }

            if (!IsFromRefContact)
            {
                IExaminationBll examinationBLL = ObjectFactory.GetObject<IExaminationBll>();
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                bool isWrokflowRestricted = examinationBLL.IsWrokflowRestricted(examModel, capModel, ACAConstant.SPEAR_FORM, AppSession.User.UserID);

                if (isWrokflowRestricted)
                {
                    ShowNotice(string.Format(GetTextByKey("examination_edit_add_restrict"), examModel.examName));
                    return false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// Show notice when some error occurred.
        /// </summary>
        /// <param name="noticeContent">The notice content.</param>
        private void ShowNotice(string noticeContent)
        {
            MessageUtil.ShowMessageInPopupScrollTop(Page, MessageType.Error, noticeContent);
            this.examEditPanel.Update();
        }

        #endregion Method
    }
}