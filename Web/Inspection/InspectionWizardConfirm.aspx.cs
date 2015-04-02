#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionWizardConfirm.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description: Input comments and confirm the information.
 *
 *  Notes:
 *      $Id: InspectionWizardConfirm.aspx.cs 170366 2010-09-08 05:34:25Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Inspection
{
    /// <summary>
    /// This class provide the ability to input comments and confirm the information.
    /// </summary>
    public partial class InspectionWizardConfirm : InspectionWizardBasePage
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [contact visible].
        /// </summary>
        /// <value><c>true</c> if [contact visible]; otherwise, <c>false</c>.</value>
        protected bool ContanctVisible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is reschedule action.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is reschedule action; otherwise, <c>false</c>.
        /// </value>
        protected bool IsRescheduleAction
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Do schedule / reschedule / request operation.
        /// </summary>
        /// <param name="queryString">The query string that pass to this page.</param>
        /// <param name="agency">The agency code.</param>
        /// <param name="moduleName">The module name.</param>
        /// <param name="additionNotes">The addition notes.</param>
        /// <returns>Return the error message, if not empty, show that there is error.</returns>
        [WebMethod(Description = "InspectionSubmit", EnableSession = true)]
        public static string InspectionSubmit(string queryString, string agency, string moduleName, string additionNotes)
        {
            string message = string.Empty;
            var inspectionBll = ObjectFactory.GetObject<IInspectionBll>();

            queryString = new JavaScriptSerializer().Deserialize<string>(queryString);
            InspectionParameter inspectionParameter = InspectionParameterUtil.BuildModelFromQueryString(queryString);

            additionNotes = new JavaScriptSerializer().Deserialize<string>(additionNotes);

            //prepare inspectionModel and array list.
            InspectionModel inspectionModel = AssembleInspectionModel(inspectionParameter, agency, moduleName, additionNotes);
            if (inspectionModel == null)
            {
                return string.Empty;
            }

            InspectionModel[] inspectionModelList = new InspectionModel[1];
            inspectionModelList[0] = inspectionModel;

            //prepare actMode
            string actMode = inspectionBll.GetActionMode(inspectionParameter.Action);

            //prepare inspector
            SysUserModel inspector = new SysUserModel();
            inspector.userID = AppSession.User.PublicUserId;
            inspector.agencyCode = agency;

            long[] result = new long[1];

            try
            {
                if (inspectionModel.activity.inAdvanceFlag == ACAConstant.COMMON_Y)
                {
                    //validate schedule date according to the inspection flow
                    string messageKey = inspectionBll.ValidateScheduleDateByFlow(inspectionModel.activity.capID, inspectionModel);

                    if (!string.IsNullOrEmpty(messageKey))
                    {
                        message = LabelUtil.GetTextByKey("ins_inspectionList_inadvance_invalid_scheduledate", moduleName);
                        message = string.Format(message, inspectionParameter.TypeText);

                        return message;
                    }
                }

                inspectionBll.BatchScheduleInspections(agency, inspectionModelList, actMode, inspector);
            }
            catch (ACAException ex)
            {
                return ex.Message;
            }

            return message;
        }

        /// <summary>
        /// Raises the page load event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetPageTitleKey("aca_inspection_title_scheduleorrequestinspection");
            this.SetDialogMaxHeight("600");
            this.IsRescheduleAction = InspectionWizardParameter.Action == InspectionAction.Reschedule;

            if (!Page.IsPostBack)
            {
                if (!AppSession.IsAdmin)
                {
                    MarkCurrentPageTrace(InspectionWizardPage.Confirm, true);
                }

                // set the confirm context
                string operation = GetTextByKey("ins_inspectionList_label_scheduleInspection").ToLower();

                if (InspectionWizardParameter.Action == InspectionAction.Request)
                {
                    operation = GetTextByKey("ins_inspectionlist_label_requestinspection").ToLower();
                }

                lblConfirmContext.Text = string.Format(GetTextByKey("aca_inspection_confirm_context"), GetTextByKey("aca_inspection_action_finish"), operation);

                if (!AppSession.IsAdmin)
                {
                    lblInspectionTypeValue.Text = InspectionWizardParameter.TypeText;
                    lblContactContent.Text = GetFormattedContactContent(InspectionWizardParameter);

                    if (InspectionWizardParameter.ScheduledDateTime == null)
                    {
                        lblDatetimeValue.Text = GetTextByKey("ACA_Inspection_BlankScheduleDate_TBD");
                    }
                    else
                    {
                        lblDatetimeValue.Text = InspectionViewUtil.BuildDateTimeText(InspectionWizardParameter.ScheduledDateTime, InspectionWizardParameter.TimeOption, ModuleName);
                    }
                }

                // display the location and contact
                CapModel4WS capModel = GetCapModel();
                if (capModel != null && !AppSession.IsAdmin)
                {
                    addressView.Display(capModel.addressModel, false);
                }

                SetupActionRestrictionPolicyUI();

                SetAdminUI();

                // hide the back button if the previous is empty
                string previousURL = InspectionActionViewUtil.GetWizardPreviousURL(InspectionWizardParameter);

                if (!AppSession.IsAdmin && string.IsNullOrEmpty(previousURL))
                {
                    tdBack.Visible = false;
                    tdBackSpace.Visible = false;
                }

                //set inspection comment limit by standard choice"INPECTION SETTINGS".
                SetCommentLengthValidation();
            }

            SetContactByPermission();
        }

        /// <summary>
        /// Raises the Back button click event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void BackButton_Click(object sender, EventArgs e)
        {
            string previousURL = InspectionActionViewUtil.GetWizardPreviousURL(InspectionWizardParameter);

            if (!string.IsNullOrEmpty(previousURL))
            {
                Response.Redirect(previousURL);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Formats the estimated time.
        /// </summary>
        /// <param name="time1String">The time1 string.</param>
        /// <param name="time2String">AM/PM value.</param>
        /// <returns>Return the format estimated time.</returns>
        private static string FormatEstimatedTime(string time1String, string time2String)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(time1String) && !string.IsNullOrEmpty(time2String))
            {
                result = string.Format("{0} {1}", time2String, time1String);
            }
            else if (!string.IsNullOrEmpty(time1String))
            {
                result = time1String;
            }

            return result;
        }

        /// <summary>
        /// Get Assemble Inspection Model.
        /// </summary>
        /// <param name="inspectionParameter">The inspection parameter.</param>
        /// <param name="agency">The agency code.</param>
        /// <param name="moduleName">The module name.</param>
        /// <param name="additionNotes">The addition notes.</param>
        /// <returns>Return a InspectionModel.</returns>
        private static InspectionModel AssembleInspectionModel(InspectionParameter inspectionParameter, string agency, string moduleName, string additionNotes)
        {
            string time1String = string.Empty;
            string time2String = string.Empty;
            string endtime1String = string.Empty;
            string endtime2String = string.Empty;
            string activityType = string.Empty;
            string statusAfterAction = string.Empty;

            InspectionAction inspectionAction = inspectionParameter.Action;

            if (inspectionAction != InspectionAction.None)
            {
                var inspectionBll = ObjectFactory.GetObject<IInspectionBll>();

                //prepare actMode
                statusAfterAction = inspectionBll.GetStatusAfterAction(inspectionAction);
            }

            DateTime? activityDate = inspectionParameter.ScheduledDateTime;
            DateTime? endActivityDate = inspectionParameter.EndScheduledDateTime;

            if (activityDate != null && activityDate != DateTime.MinValue)
            {
                InspectionScheduleType scheduleType = inspectionParameter.ScheduleType;
                var readyTimeEnabled = inspectionParameter.ReadyTimeEnabled != null ? inspectionParameter.ReadyTimeEnabled.Value : false;

                if (scheduleType == InspectionScheduleType.ScheduleUsingCalendar
                    || (scheduleType == InspectionScheduleType.RequestOnlyPending && readyTimeEnabled)
                    || scheduleType == InspectionScheduleType.Unknown)
                {
                    InspectionViewUtil.BuildTimeValue(activityDate, inspectionParameter.TimeOption, out time1String, out time2String);
                    InspectionViewUtil.BuildTimeValue(endActivityDate, inspectionParameter.TimeOption, out endtime1String, out endtime2String);
                }
            }

            //prepare activityType
            activityType = inspectionParameter.Type;

            //prepare CapIDModel
            CapIDModel4WS capIDModel = new CapIDModel4WS();
            capIDModel.id1 = inspectionParameter.RecordID1;
            capIDModel.id2 = inspectionParameter.RecordID2;
            capIDModel.id3 = inspectionParameter.RecordID3;
            capIDModel.serviceProviderCode = inspectionParameter.AgencyCode;

            InspectionModel inspectionmodel = new InspectionModel();

            CommentModel commentModel = new CommentModel();
            commentModel.text = additionNotes;
            inspectionmodel.comment = commentModel;

            ActivityModel activityModel = new ActivityModel();
            activityModel.capID = TempModelConvert.Trim4WSOfCapIDModel(capIDModel);
            activityModel.activityDate = activityDate;
            activityModel.time1 = time1String;
            activityModel.time2 = time2String;
            activityModel.auditID = AppSession.User.PublicUserId;
            activityModel.auditDate = DateTime.Now;
            activityModel.auditStatus = ACAConstant.VALID_STATUS;
            activityModel.activityType = activityType;
            activityModel.serviceProviderCode = agency;
            activityModel.inAdvanceFlag = inspectionParameter.InAdvance == true ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

            activityModel.estimatedStartTime = FormatEstimatedTime(time1String, time2String);
            activityModel.estimatedEndTime = FormatEstimatedTime(endtime1String, endtime2String);

            PeopleModel4WS[] peoples = AppSession.User.ApprovedContacts;

            if (peoples != null && peoples.Length == 1 && peoples[0] != null)
            {
                PeopleModel4WS people = peoples[0];
                activityModel.requestorFname = people.firstName;
                activityModel.requestorMname = people.middleName;
                activityModel.requestorLname = people.lastName;
            }

            activityModel.requestorUserID = AppSession.User.UserID;

            activityModel.contactFname = inspectionParameter.ContactFirstName;
            activityModel.contactMname = inspectionParameter.ContactMiddleName;
            activityModel.contactLname = inspectionParameter.ContactLastName;

            //prepare inspectionId
            long inspectionId = 0;
            long.TryParse(inspectionParameter.ID, out inspectionId);

            activityModel.idNumber = inspectionId;

            long sequenceNumber = 0;
            long.TryParse(inspectionParameter.TypeID, out sequenceNumber);

            activityModel.inspSequenceNumber = sequenceNumber;
            activityModel.status = statusAfterAction;
            activityModel.activityDescription = activityType;
            activityModel.requiredInspection = inspectionParameter.Required == true ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            activityModel.contactPhoneNum = inspectionParameter.ContactPhoneNumber;
            activityModel.contactPhoneNumIDD = inspectionParameter.ContactPhoneIDD;

            // set the requestor phone number
            activityModel.reqPhoneNum = AppSession.User.CellPhone;
            activityModel.reqPhoneNumIDD = AppSession.User.CellPhoneCountryCode;

            inspectionmodel.activity = activityModel;

            return inspectionmodel;
        }

        /// <summary>
        /// Setups the action restriction policy UI.
        /// </summary>
        private void SetupActionRestrictionPolicyUI()
        {
            if (!AppSession.IsAdmin)
            {
                string rescheduleRestrictionPolicy = BuildRescheduleRestrictionPolicy();
                string cancellationRestrictionPolicy = BuildCancellationRestrictionPolicy();
                lblReschedulePolicy.Text = rescheduleRestrictionPolicy;
                lblCancellationPolicy.Text = cancellationRestrictionPolicy;

                if (string.IsNullOrEmpty(rescheduleRestrictionPolicy) && string.IsNullOrEmpty(cancellationRestrictionPolicy))
                {
                    divActionPolicy.Visible = false;
                }

                if (string.IsNullOrEmpty(rescheduleRestrictionPolicy))
                {
                    divReschedulePolicy.Visible = false;
                }

                if (string.IsNullOrEmpty(cancellationRestrictionPolicy))
                {
                    divCancellationPolicy.Visible = false;
                }
            }
        }

        /// <summary>
        /// Set the admin UI.
        /// </summary>
        private void SetAdminUI()
        {
            if (AppSession.IsAdmin)
            {
                divReschedulePolicy.Visible = false;
                divCancellationPolicy.Visible = false;
                divAdminPartOfPolicy.Visible = true;

                string css = divAdditionNotesInput.Attributes["class"];
                divAdditionNotesInput.Attributes["class"] = css.Replace("ACA_Hide", "ACA_Show");
            }
        }

        /// <summary>
        /// Set contact by permission
        /// </summary>
        private void SetContactByPermission()
        {
            ContanctVisible = true;

            if (!AppSession.IsAdmin)
            {
                var inspectionPermissionBll = ObjectFactory.GetObject<IInspectionPermissionBll>();

                CapModel4WS capModel = GetCapModel();
                bool right = inspectionPermissionBll.CheckContactRight(capModel, InspectionWizardParameter.AgencyCode, ModuleName, ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_INPUT_CONTACT);

                if (right == false)
                {
                    lblContact.Visible = false;
                    lblContactContent.Visible = false;
                    ContanctVisible = false;
                }
            }
        }

        /// <summary>
        /// Set the inspection comment length validation according to standard choice settings.
        /// By default, the comment length is limited to 2000 characters.
        /// </summary>
        private void SetCommentLengthValidation()
        {
            if (StandardChoiceUtil.IsInspectionCommentLengthUnlimited())
            {
                tbAdditionNotes.Validate = tbAdditionNotes.Validate.Replace("MaxLength", string.Empty);
                tbAdditionNotes.MaxLength = 0;
            }
        }

        #endregion Private Methods
    }
}
