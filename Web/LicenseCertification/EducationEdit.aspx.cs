#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EducationEdit.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EducationEdit.ascx.cs 238264 2013-10-7 09:31:18Z ACHIEVO\alan.hu $.
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
    /// Education edit page
    /// </summary>
    public partial class EducationEdit : PopupDialogBasePage
    {
        #region Fields

        /// <summary>
        /// indicate the default index value when no education is selected in the education list.
        /// </summary>
        private const int NO_EDUCATION_SELECTED = -1;

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(EducationEdit));

        /// <summary>
        /// indicate the the Education section whether can editable.
        /// </summary>
        private bool _isEditable = true;

        /// <summary>
        /// Gets or sets the education row index at the education list.
        /// </summary>
        private int _rowIndex = 0;

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
        /// Gets or sets the Education list.
        /// </summary>
        protected IList<EducationModel4WS> EducationModelList
        {
            get
            {
                if (ViewState["EducationModels"] == null)
                {
                    ViewState["EducationModels"] = new List<EducationModel4WS>();
                }

                return ViewState["EducationModels"] as IList<EducationModel4WS>;
            }

            set
            {
                ViewState["EducationModels"] = value;
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
        /// Page load event.
        /// </summary>
        /// <param name="sender">event object</param>
        /// <param name="e">event argument</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            IsFromRefContact = Request.QueryString.AllKeys.Contains(UrlConstant.CONTACT_SEQ_NUMBER);
            ContactIsFromExternal = ValidationUtil.IsYes(Request.QueryString[UrlConstant.CONTACT_IS_FROM_EXTERNAL]);
            SetDialogMaxHeight("600");
            ContactSeqNbr = Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER];
            int.TryParse(Request.QueryString[UrlConstant.ROW_INDEX], out _rowIndex);
            IsUpdate = _rowIndex > NO_EDUCATION_SELECTED;
            string titleKey = IsFromRefContact ? "aca_contact_education_edit_label_title" : "aca_education_edit_label_title";

            if (!AppSession.IsAdmin)
            {
                SetPageTitleKey(titleKey);
            }
            
            if (IsFromRefContact)
            {
                btnSave.LabelKey = "aca_contact_education_edit_label_save_and_close";
                btnCancel.LabelKey = "aca_contact_education_edit_label_cancel";
            }
            else
            {
                btnSave.LabelKey = "aca_education_edit_label_save_and_close";
                btnCancel.LabelKey = "aca_education_edit_label_cancel";
            }

            educationDetailEdit.IsFromRefContact = IsFromRefContact;

            if (!IsPostBack)
            {
                DialogUtil.RegisterScriptForDialog(this.Page);
                IsEditable = IsFromRefContact || ValidationUtil.IsYes(Request.QueryString["editable"]);

                if (IsFromRefContact)
                {
                    EducationModelList = ObjectConvertUtil.ConvertArrayToList(AppSession.GetContactEducationListFromSession(ContactSeqNbr));
                }
                else
                {
                    CapModel4WS cap = AppSession.GetCapModelFromSession(ModuleName);

                    if (cap != null)
                    {
                        EducationModelList = ObjectConvertUtil.ConvertArrayToList(cap.educationList);
                    }
                }

                if (!AppSession.IsAdmin)
                {
                    if (IsUpdate)
                    {
                        EducationModel4WS educationModel = EducationModelList.First(f => f.RowIndex == _rowIndex);
                        educationDetailEdit.SetEducationDetailInfo(educationModel);
                    }
                    else
                    {
                        educationDetailEdit.SetEducationDetailInfo(null);
                    }

                    // Need put it after the method SetEducationDetailInfo, it will create template.
                    if (!IsEditable || ContactIsFromExternal)
                    {
                        educationDetailEdit.DisableEducationForm();
                        btnSave.Enabled = false;
                    }
                }
                else
                {
                    educationDetailEdit.SetEducationDetailInfo(null);
                }
            }

            if (AppSession.IsAdmin)
            {
                AccelaDropDownList dropDownList = new AccelaDropDownList();
                dropDownList.ID = "educationtype";
                IRefEducationBll refEducationBll = ObjectFactory.GetObject<IRefEducationBll>();
                RefEducationModel4WS[] refEducations = refEducationBll.GetRefEducationList(
                    new RefEducationModel4WS() { serviceProviderCode = ConfigManager.AgencyCode });
                IList<ListItem> listItems = new List<ListItem>();

                if (refEducations != null && refEducations.Length != 0)
                {
                    foreach (var obj in refEducations)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = obj.refEducationName;
                        listItem.Value = obj.refEducationNbr.ToString();
                        listItems.Add(listItem);
                    }
                }

                dropDownList.AutoPostBack = false;
                dropDownList.Attributes.Add("onfocus", "javascript:if(typeof(parent.GetOldValueBeforeValueChange)!='undefined'){parent.GetOldValueBeforeValueChange(this);}");
                dropDownList.Attributes.Add("onchange", "javascript:if(typeof(parent.ChangedItemsValidateBeforePostBack)!='undefined'){parent.ChangedItemsValidateBeforePostBack(this,window);}");

                DropDownListBindUtil.BindDDL(listItems, dropDownList);
                sectionTitleBar.AddToolBarControls(dropDownList);

                dropDownList.SelectedIndexChanged += (obj, args) =>
                {
                    AccelaDropDownList senderControl = obj as AccelaDropDownList;
                    if (senderControl != null)
                    {
                        educationDetailEdit.SetPermissionValue(senderControl.SelectedValue);
                        ChangeFormLayout(senderControl.SelectedValue);
                    }
                };

                string viewId = IsFromRefContact ? GviewID.RefContactEducationEdit : GviewID.EducationEdit;
                sectionTitleBar.Visible = true;
                sectionTitleBar.PermissionValueId = dropDownList.ClientID;
                sectionTitleBar.SectionID = string.Format("{1}{0}{2}{0}{3}", ACAConstant.SPLIT_CHAR, ModuleName, viewId, educationDetailEdit.ClientID + "_");
                sectionTitleBar.LabelKey = titleKey;
            }
        }

        /// <summary>
        /// Save education event.
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">event argument.</param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            EducationModel4WS newEducationModel = educationDetailEdit.GetEducationModel();

            if (IsUpdate)
            {
                EducationModel4WS originEducationModel = EducationModelList.First(f => f.RowIndex == _rowIndex);
                newEducationModel.educationPKModel = originEducationModel.educationPKModel;
                newEducationModel.contactSeqNumber = originEducationModel.contactSeqNumber;
                newEducationModel.entityID = originEducationModel.entityID;
                newEducationModel.entityType = originEducationModel.entityType;
                newEducationModel.approvedFlag = ValidationUtil.IsYes(originEducationModel.approvedFlag) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                newEducationModel.syncFlag = originEducationModel.syncFlag;
                newEducationModel.associatedEduCount = originEducationModel.associatedEduCount;
                newEducationModel.RowIndex = originEducationModel.RowIndex;
            }
            else
            {
                newEducationModel.RowIndex = NO_EDUCATION_SELECTED;
                newEducationModel.approvedFlag = ACAConstant.COMMON_N;
            }

            try
            {
                ILicenseCertificationBll licenseCertificationBll = ObjectFactory.GetObject<ILicenseCertificationBll>();
                bool isSuccess = true;

                if (IsFromRefContact)
                {
                    EducationModel educationModel = TempModelConvert.ConvertToEducationModel(newEducationModel);

                    if (IsUpdate)
                    {
                        isSuccess = licenseCertificationBll.AddOrUpdateRefPeopleEdu(educationModel);
                    }
                    else
                    {
                        educationModel.contactSeqNumber = long.Parse(ContactSeqNbr);
                        educationModel.entityType = ACAConstant.REF_CONTACT_EDUCATION_ENTITY_TYPE;
                        isSuccess = licenseCertificationBll.AddOrUpdateRefPeopleEdu(educationModel);
                    }
                }
                else
                {
                    if (EducationUtil.IsExistDuplicateEducation(newEducationModel, EducationModelList))
                    {
                        MessageUtil.ShowMessageInPopupScrollTop(Page, MessageType.Error, GetTextByKey("education_validation_duplicate_message"));
                        return;
                    }

                    if (IsUpdate)
                    {
                        for (int i = 0; i < EducationModelList.Count; i++)
                        {
                            if (EducationModelList[i].RowIndex == _rowIndex)
                            {
                                EducationModelList[i] = newEducationModel;
                            }
                        }
                    }
                    else
                    {
                        IList<EducationModel4WS> tmpEducationList = new List<EducationModel4WS>();

                        foreach (var item in EducationModelList)
                        {
                            tmpEducationList.Add(item);
                        }

                        newEducationModel.entityType = ACAConstant.CAP_EDUCATION_ENTITY_TYPE;
                        tmpEducationList.Add(newEducationModel);
                        EducationModelList = tmpEducationList;
                    }

                    CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                    capModel.educationList = EducationModelList.ToArray();
                }

                if (isSuccess)
                {
                    AppSession.SetContactEducationListToSession(ContactSeqNbr, EducationModelList.ToArray());
                    string script = "CloseEduDialog(" + IsUpdate.ToString().ToLower() + ");";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SaveEducation", script, true);
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
            educationDetailEdit.SetPermissionValue(refEduId);
            educationDetailEdit.DisplayGenericTemplate(refEduId);
        }

        #endregion Method
    }
}