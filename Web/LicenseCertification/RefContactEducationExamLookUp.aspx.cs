#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefContactEducationExamLookUp.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefContactEducationExamLookUp.aspx.cs 238264 2013-10-7 09:31:18Z ACHIEVO\alan.hu $.
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
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Examination;
using Accela.ACA.Web.People;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.UI;

namespace Accela.ACA.Web.LicenseCertification
{
    /// <summary>
    /// Look up Education, Examination, Continuing Education Page.
    /// </summary>
    public partial class RefContactEducationExamLookUp : PeoplePopupBasePage
    {
        #region Properties

        /// <summary>
        /// Gets the component name.
        /// </summary>
        protected string ComponentName
        {
            get
            {
                string componenetName = !string.IsNullOrEmpty(Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER]) 
                                        ? ContactSessionParameterModel.PageFlowComponent.ComponentName
                                        : Request.QueryString[UrlConstant.CONTACT_COMPONENT_NAME];

                return componenetName;
            }
        }

        /// <summary>
        /// Gets or sets the contact sequence number.
        /// </summary>
        protected string ContactSeqNbr
        {
            get; 
            set; 
        }

        /// <summary>
        /// Gets the component id.
        /// </summary>
        protected string ComponentID
        {
            get
            {
                string componentID = !string.IsNullOrEmpty(Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER]) 
                                     ? ContactSessionParameterModel.PageFlowComponent.ComponentID.ToString()
                                     : Request[UrlConstant.COMPONENT_TYPE];

                return componentID;
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitleKey("aca_license_certification_label_title");
            ContactSeqNbr = Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER];

            if (!AppSession.IsAdmin)
            {
                SetDialogMaxHeight("600");

                //From Add from Saved Education、Examination or Continuing Education.
                if (string.IsNullOrEmpty(ContactSeqNbr))
                {
                    liSelectBtn.Visible = true;
                    divLookUpByContacts.Visible = true;
                }
                else
                {
                    //From Add from saved contact.
                    liSaveAndCloseBtn.Visible = true;
                    divLookUpByContact.Visible = true;
                }
            }

            if (!IsPostBack)
            {
                string selectedContactSeqNbr = string.Empty;

                if (!AppSession.IsAdmin)
                {
                    //From [Select from Account] or [Look Up].
                    if (!string.IsNullOrEmpty(ContactSeqNbr))
                    {
                        PeopleModel4WS selectedPeople = ContactUtil.GetPeopleModelFromContactSessionParameter();

                        if (EnumUtil<ContactType4License>.Parse(selectedPeople.contactTypeFlag, ContactType4License.Individual) == ContactType4License.Individual)
                        {
                            selectedContactSeqNbr = ContactSeqNbr;
                        }

                        IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
                        lblContactName.Text = peopleBll.GetContactUserName(selectedPeople);
                        lblContactType.Text = ContactType;
                    }
                    else
                    {
                        //From [Select from Contact] Education、Examination or Continuing Education.
                        CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                        DropDownListBindUtil.BindContactListByCapModel(ddlContactList, capModel);
                        CapContactModel4WS primaryCapContactModel = ExaminationScheduleUtil.GetCapPrimaryContact(capModel);
                        string primaryContactSeqNbr = string.Empty;

                        if (primaryCapContactModel != null)
                        {
                            primaryContactSeqNbr = primaryCapContactModel.refContactNumber;
                            DropDownListBindUtil.SetSelectedValue(ddlContactList, primaryContactSeqNbr);
                            ddlContactList.DisableEdit();
                        }

                        if (!string.IsNullOrWhiteSpace(primaryContactSeqNbr) && primaryContactSeqNbr != ddlContactList.SelectedValue)
                        {
                            ddlContactList.Items.Insert(0, new ListItem(string.Empty, string.Empty));
                            ddlContactList.SelectedIndex = -1;
                        }

                        selectedContactSeqNbr = ddlContactList.SelectedValue;
                    }
                }
                else
                {
                    liSelectBtn.Visible = true;
                    liSaveAndCloseBtn.Visible = true;
                    divLookUpByContact.Visible = true;
                    divLookUpByContacts.Visible = true;
                    divContactInfo.Visible = false;
                }

                BindRefContactEducationExamList(selectedContactSeqNbr);
            }
        }

        /// <summary>
        /// Select education, exam event handler.
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">The event handle</param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            string selectedContactSeqNbr = string.IsNullOrWhiteSpace(ContactSeqNbr) ? ddlContactList.SelectedValue : ContactSeqNbr;
            SelectedContactLicenseCertificationModel selectedLicenseCertification = new SelectedContactLicenseCertificationModel();

            if (refContactEducationList.GridViewDataSource != null)
            {
                selectedLicenseCertification.SelectedEducations = refContactEducationList.GetSelectedEducation();
            }

            if (refContactContEducationList.GridViewDataSource != null)
            {
                selectedLicenseCertification.SelectedContEdus = refContactContEducationList.GetSelectedContEducation();
            }

            if (refContactExaminationList.GridViewDataSource != null)
            {
                selectedLicenseCertification.SelectedExaminations = refContactExaminationList.GetSelectedExamination();
            }

            AppSession.SetSelectedContactLicenseCertification(selectedLicenseCertification);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "SaveEducation", "CloseLookUpDialog('" + selectedContactSeqNbr + "');", true);
        }

        /// <summary>
        /// The event handler for contact change.
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">The event handle</param>
        protected void ContactListDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRefContactEducationExamList(ddlContactList.SelectedValue);
        }

        /// <summary>
        /// Bind the selected reference contact educations, examinations and continuing educations.
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        protected void BindRefContactEducationExamList(string contactSeqNbr)
        {
            if (string.IsNullOrEmpty(contactSeqNbr))
            {
                refContactEducationList.GridViewDataSource = null;
                refContactEducationList.BindEducationList();
                refContactExaminationList.GridViewDataSource = null;
                refContactExaminationList.BindExaminationList();
                refContactContEducationList.GridViewDataSource = null;
                refContactContEducationList.BindEducationList();
                return;
            }

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            ILicenseCertificationBll licenseCertificationBll = ObjectFactory.GetObject<ILicenseCertificationBll>();
            CapAssociateLicenseCertification4WS associateLicense = AppSession.GetCapTypeAssociateLicenseCertification();

            RefContinuingEducationModel[] refContinuingEducationModels = null;
            RefEducationModel[] refEducationModels = null;
            RefExaminationModel[] refExaminationModels = null;

            if (associateLicense != null)
            {
                refContinuingEducationModels = associateLicense.capAssociateContEducation;
                refEducationModels = associateLicense.capAssociateEducation;
                refExaminationModels = associateLicense.capAssociateExamination;
            }

            IList<EducationModel4WS> capEduList = ObjectConvertUtil.ConvertArrayToList(capModel.educationList);
            IList<ExaminationModel> capExamList = ObjectConvertUtil.ConvertArrayToList(capModel.examinationList);
            IList<ContinuingEducationModel4WS> capContEduList = ObjectConvertUtil.ConvertArrayToList(capModel.contEducationList);

            var contactEdus = TempModelConvert.ConvertToEducationModel4WS(licenseCertificationBll.GetRefPeopleEduList(contactSeqNbr));
            IList<EducationModel4WS> contactEduList = contactEdus == null ? new List<EducationModel4WS>() : contactEdus.ToList();
            IList<EducationModel4WS> notUsedEduList = new List<EducationModel4WS>();

            foreach (var contactEdu in contactEduList)
            {
                if (capEduList != null && capEduList.Any(f => f.entityID == contactEdu.educationPKModel.educationNbr))
                {
                    continue;
                }

                contactEdu.requiredFlag = refEducationModels != null && refEducationModels.Any(o => contactEdu.educationName.Equals(o.refEducationName)) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                notUsedEduList.Add(contactEdu);
            }

            refContactEducationList.GridViewDataSource = notUsedEduList;
            refContactEducationList.BindEducationList();

            IExaminationBll examinationBll = ObjectFactory.GetObject<IExaminationBll>();
            var contactExams = examinationBll.GetRefPeopleExamList(contactSeqNbr);
            IList<ExaminationModel> contactExamList = contactExams == null ? new List<ExaminationModel>() : contactExams.ToList();
            IList<ExaminationModel> notUsedExamList = new List<ExaminationModel>();

            foreach (var contactExam in contactExamList)
            {
                if (capExamList != null && capExamList.Any(f => f.entityID == contactExam.examinationPKModel.examNbr))
                {
                    continue;
                }

                contactExam.requiredFlag = refExaminationModels != null && refExaminationModels.Any(o => contactExam.examName.Equals(o.examName)) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                notUsedExamList.Add(contactExam);
            }

            refContactExaminationList.GridViewDataSource = notUsedExamList;
            refContactExaminationList.BindExaminationList();

            var contcatContEdus = TempModelConvert.ConvertToContEducationModel4WS(licenseCertificationBll.GetRefPeopleContEduList(contactSeqNbr));
            IList<ContinuingEducationModel4WS> contactContEduList = contcatContEdus == null ? new List<ContinuingEducationModel4WS>() : contcatContEdus.ToList();
            IList<ContinuingEducationModel4WS> notUsedContEduList = new List<ContinuingEducationModel4WS>();

            foreach (var contactContEdu in contactContEduList)
            {
                if (capContEduList != null && capContEduList.Any(f => f.entityID == contactContEdu.continuingEducationPKModel.contEduNbr.ToString()))
                {
                    continue;
                }

                contactContEdu.requiredFlag = refContinuingEducationModels != null && refContinuingEducationModels.Any(o => contactContEdu.contEduName.Equals(o.contEduName)) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                notUsedContEduList.Add(contactContEdu);
            }

            refContactContEducationList.GridViewDataSource = notUsedContEduList;
            refContactContEducationList.BindEducationList();
        }

        #endregion
    }
}