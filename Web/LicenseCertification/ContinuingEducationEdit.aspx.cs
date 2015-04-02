#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContinuingEducationEdit.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ContinuingEducationEdit.aspx.cs 238264 2013-10-7 09:31:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Continuing Education Edit page
    /// </summary>
    public partial class ContinuingEducationEdit : PopupDialogBasePage
    {
        #region Fields

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(ContinuingEducationEdit));

        /// <summary>
        /// Gets or sets the education number.
        /// </summary>
        private long _rowIndex;

        /// <summary>
        /// indicate the the Education section whether can editable.
        /// </summary>
        private bool _isEditable = true;

        #endregion Fields

        #region Properties

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
        /// Gets or sets the Education list.
        /// </summary>
        protected IList<ContinuingEducationModel4WS> ContinuingEducationModelList
        {
            get
            {
                if (ViewState["ContEducationModels"] == null)
                {
                    ViewState["ContEducationModels"] = new List<ContinuingEducationModel4WS>();
                }

                return ViewState["ContEducationModels"] as IList<ContinuingEducationModel4WS>;
            }

            set
            {
                ViewState["ContEducationModels"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is update Education.
        /// </summary>
        private bool IsUpdate
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsAddContEducation"]);
            }

            set
            {
                ViewState["IsAddContEducation"] = value;
            }
        }

        #endregion Properties

        #region Method

        /// <summary>
        /// Page load event.
        /// </summary>
        /// <param name="sender">event object</param>
        /// <param name="e">event argument</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            IsFromRefContact = Request.QueryString.AllKeys.Contains(UrlConstant.CONTACT_SEQ_NUMBER);
            ContactIsFromExternal = ValidationUtil.IsYes(Request.QueryString[UrlConstant.CONTACT_IS_FROM_EXTERNAL]);
            SetDialogMaxHeight("600");
            _rowIndex = Convert.ToInt64(Request.QueryString[UrlConstant.ROW_INDEX]);
            IsUpdate = _rowIndex >= 0;
            ContactSeqNbr = Request.QueryString["contactSeqNbr"];
            string titleKey = IsFromRefContact ? "aca_contact_continuing_education_edit_label_title" : "aca_continuing_education_edit_label_title";

            if (!AppSession.IsAdmin)
            {
                SetPageTitleKey(titleKey);
            }

            if (IsFromRefContact)
            {
                btnSaveAndClose.LabelKey = "aca_contact_continuing_education_edit_label_save_and_close";
                btnCancel.LabelKey = "aca_contact_continuing_education_edit_label_cancel";
            }
            else
            {
                btnSaveAndClose.LabelKey = "aca_continuing_education_edit_label_save_and_close";
                btnCancel.LabelKey = "aca_continuing_education_edit_label_cancel";
            }

            if (!IsPostBack)
            {
                IsEditable = AppSession.IsAdmin || IsFromRefContact || ValidationUtil.IsYes(Request.QueryString["editable"]);
                DialogUtil.RegisterScriptForDialog(this.Page);
                continuingEducationDetailEdit.IsFromRefContact = IsFromRefContact;

                if (IsFromRefContact)
                {
                    ContinuingEducationModelList = ObjectConvertUtil.ConvertArrayToList(AppSession.GetContactContEducationListFromSession(ContactSeqNbr));
                }
                else
                {
                    CapModel4WS cap = AppSession.GetCapModelFromSession(ModuleName);

                    if (cap != null)
                    {
                        ContinuingEducationModelList = ObjectConvertUtil.ConvertArrayToList(cap.contEducationList);
                    }
                }

                if (!AppSession.IsAdmin)
                {
                    if (IsUpdate)
                    {
                        ContinuingEducationModel4WS contEducationModel = ContinuingEducationModelList.First(itme => itme.RowIndex == _rowIndex);
                        continuingEducationDetailEdit.FillContEducation(contEducationModel);
                    }
                    else
                    {
                        continuingEducationDetailEdit.FillContEducation(null);
                    }

                    // Disable continuing education edit form when the property is disable. Need put it after the method FillContEducation, it will create template.
                    if (!IsEditable || ContactIsFromExternal)
                    {
                        continuingEducationDetailEdit.DisableContEducationForm();
                        btnSaveAndClose.Enabled = false;
                    }
                }
                else
                {
                    continuingEducationDetailEdit.FillContEducation(null);
                }
            }

            if (AppSession.IsAdmin)
            {
                sectionTitleBar.Visible = true;
                AccelaDropDownList dropDownList = new AccelaDropDownList();
                dropDownList.ID = "ddlContEducationType";

                IRefContinuingEducationBll refContEducationBll = ObjectFactory.GetObject<IRefContinuingEducationBll>();
                RefContinuingEducationModel4WS[] contEducations = refContEducationBll.GetRefContEducationList(new RefContinuingEducationModel4WS() { serviceProviderCode = ConfigManager.AgencyCode });

                IList<ListItem> listItems = new List<ListItem>();

                if (contEducations != null && contEducations.Length != 0)
                {
                    foreach (RefContinuingEducationModel4WS obj in contEducations)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = obj.contEduName;
                        listItem.Value = obj.refContEduNbr.ToString();
                        listItems.Add(listItem);
                    }
                }

                dropDownList.AutoPostBack = false;
                dropDownList.Attributes.Add("onfocus", "javascript:if(typeof(parent.GetOldValueBeforeValueChange)!='undefined'){parent.GetOldValueBeforeValueChange(this);}");
                dropDownList.Attributes.Add("onchange", "javascript:if(typeof(parent.ChangedItemsValidateBeforePostBack)!='undefined'){parent.ChangedItemsValidateBeforePostBack(this,window);}");

                DropDownListBindUtil.BindDDL(listItems, dropDownList);
                sectionTitleBar.AddToolBarControls(dropDownList);
                sectionTitleBar.PermissionValueId = dropDownList.ClientID;
                sectionTitleBar.LabelKey = titleKey;

                sectionTitleBar.SectionID = string.Format(
                    "{0}" + ACAConstant.SPLIT_CHAR + "{1}" + ACAConstant.SPLIT_CHAR + "{2}",
                    ModuleName,
                    IsFromRefContact ? GviewID.RefContactContinuingEducationEdit : GviewID.ContinuingEducationEdit,
                    continuingEducationDetailEdit.ClientID + "_");

                dropDownList.SelectedIndexChanged += (obj, args) =>
                {
                    AccelaDropDownList senderControl = obj as AccelaDropDownList;

                    if (senderControl != null)
                    {
                        continuingEducationDetailEdit.SetPermissionValue(senderControl.SelectedValue);
                        ChangeFormLayout(senderControl.SelectedValue);
                    }
                };
            }
        }

        /// <summary>
        /// Save education event.
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">event argument.</param>
        protected void SaveAndCloseButton_Click(object sender, EventArgs e)
        {
            ContinuingEducationModel4WS contEducationModel4Ws = continuingEducationDetailEdit.GetContEducation();
            ContinuingEducationModel4WS updateModel = null;

            if (IsUpdate)
            {
                updateModel = ContinuingEducationModelList.Single(f => f.RowIndex == _rowIndex);
                contEducationModel4Ws.continuingEducationPKModel = updateModel.continuingEducationPKModel;
                contEducationModel4Ws.contactSeqNumber = updateModel.contactSeqNumber;
                contEducationModel4Ws.entityID = updateModel.entityID;
                contEducationModel4Ws.entityType = updateModel.entityType;
                contEducationModel4Ws.approvedFlag = ValidationUtil.IsYes(updateModel.approvedFlag) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                contEducationModel4Ws.syncFlag = updateModel.syncFlag;
                contEducationModel4Ws.associatedContEduCount = updateModel.associatedContEduCount;
                contEducationModel4Ws.RowIndex = updateModel.RowIndex;
            }
            else
            {
                contEducationModel4Ws.approvedFlag = ACAConstant.COMMON_N;
            }

            ILicenseCertificationBll licenseCertificationBll = ObjectFactory.GetObject<ILicenseCertificationBll>();
            IContinuingEducationBll continuingEducationBll = ObjectFactory.GetObject<IContinuingEducationBll>();

            try
            {
                bool isSuccess = true;

                if (IsFromRefContact)
                {
                    if (!continuingEducationBll.IsPassedContEdu(contEducationModel4Ws.gradingStyle, contEducationModel4Ws.finalScore, contEducationModel4Ws.passingScore))
                    {
                        string msgText = string.Format(GetTextByKey("aca_register_education_exam_to_contact_msg_uncompleted"), contEducationModel4Ws.contEduName);
                        MessageUtil.ShowMessageInPopupScrollTop(Page, MessageType.Error, msgText);
                        educationEditPanel.Update();

                        return;
                    }

                    ContinuingEducationModel model = TempModelConvert.ConvertToContEducationModel(contEducationModel4Ws);

                    if (IsUpdate)
                    {
                        isSuccess = licenseCertificationBll.AddOrUpdateRefPeopleContEdu(model);
                    }
                    else
                    {
                        model.contactSeqNumber = Convert.ToInt64(ContactSeqNbr);
                        model.entityType = ACAConstant.REF_CONTACT_CONT_EDUCATION_ENTITY_TYPE;
                        isSuccess = licenseCertificationBll.AddOrUpdateRefPeopleContEdu(model);

                        if (isSuccess)
                        {
                            List<ContinuingEducationModel4WS> tempList = ContinuingEducationModelList.ToList();
                            tempList.Add(contEducationModel4Ws);
                            ContinuingEducationModelList = tempList;
                        }
                    }
                }
                else
                {
                    if (updateModel != null
                        && !string.IsNullOrEmpty(updateModel.entityID)
                        && !continuingEducationBll.IsPassedContEdu(contEducationModel4Ws.gradingStyle, contEducationModel4Ws.finalScore, contEducationModel4Ws.passingScore))
                    {
                        MessageUtil.ShowMessageInPopupScrollTop(Page, MessageType.Error, GetTextByKey("aca_continuing_education_label_restriction_message"));
                        return;
                    }

                    // validate duplicate when Add/Edit continuing education record.
                    if (EducationUtil.ExistDuplicateContEducation(contEducationModel4Ws, ContinuingEducationModelList))
                    {
                        MessageUtil.ShowMessageInPopupScrollTop(Page, MessageType.Error, GetTextByKey("continuing_education_validation_duplicate_message"));
                        return;
                    }

                    if (IsUpdate)
                    {
                        for (int i = 0; i < ContinuingEducationModelList.Count; i++)
                        {
                            if (ContinuingEducationModelList[i].RowIndex == _rowIndex)
                            {
                                ContinuingEducationModelList[i] = contEducationModel4Ws;
                            }
                        }
                    }
                    else
                    {
                        contEducationModel4Ws.entityType = ACAConstant.CAP_CONT_EDUCATION_ENTITY_TYPE;
                        List<ContinuingEducationModel4WS> tempList = ContinuingEducationModelList.ToList();
                        tempList.Add(contEducationModel4Ws);
                        ContinuingEducationModelList = tempList;
                    }

                    CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                    capModel.contEducationList = ContinuingEducationModelList.ToArray();
                    AppSession.SetCapModelToSession(ModuleName, capModel);
                }

                if (isSuccess)
                {
                    AppSession.SetContactContEducationListToSession(ContactSeqNbr, ContinuingEducationModelList.ToArray());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SaveContEducation", "CloseContEduDialog(" + IsUpdate.ToString().ToLower() + ");", true);
                }
            }
            catch (ACAException ex)
            {
                Logger.Error(ex);
                MessageUtil.ShowMessageInPopupScrollTop(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// Change Form Layout.
        /// </summary>
        /// <param name="refEduId">The ref EDU id.</param>
        private void ChangeFormLayout(string refEduId)
        {
            continuingEducationDetailEdit.SetPermissionValue(refEduId);
            continuingEducationDetailEdit.DisplayGenericTemplate(refEduId);
        }

        #endregion Method
    }
}