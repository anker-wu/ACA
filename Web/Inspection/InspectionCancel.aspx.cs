#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionCancel.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description: Provide the ablity to cancel inspection.
 *
 *  Notes:
 *      $Id: InspectionCancel.aspx.cs 170366 2010-09-08 05:34:25Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.Services;
using System.Web.UI;

using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Inspection
{
    /// <summary>
    /// This class provide the ability to cancel inspection.
    /// </summary>
    public partial class InspectionCancel : InspectionWizardBasePage
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [contact visible].
        /// </summary>
        /// <value><c>true</c> if [contact visible]; otherwise, <c>false</c>.</value>
        protected bool ContactVisible
        {
            get;
            set;
        }

        #endregion Properties

        #region Public and Proected Methods

        /// <summary>
        /// Do cancel inspection operation.
        /// </summary>
        /// <param name="queryString">The query string that pass to this page.</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>Return the error message, if not empty, show that there is error.</returns>
        [WebMethod(Description = "InspectionCancelSubmit", EnableSession = true)]
        public static string InspectionCancelSubmit(string queryString, string agencyCode)
        {
            string publicUserId = AppSession.User.PublicUserId;

            InspectionParameter inspectionParameter = InspectionParameterUtil.BuildModelFromQueryString(queryString);

            SysUserModel inspector = new SysUserModel();
            inspector.userID = publicUserId;
            inspector.agencyCode = agencyCode;

            int result = 0;
            try
            {
                InspectionModel[] inspectionlist = new InspectionModel[1];

                InspectionModel inspectionModel = GetInspectionModel(inspectionParameter);
                inspectionlist[0] = inspectionModel;

                IInspectionBll inspectionBll = (IInspectionBll)ObjectFactory.GetObject(typeof(IInspectionBll));

                //Cancel schedule inspection by given agency code, public user id, inspection list and inspector
                result = inspectionBll.CancelInspection(agencyCode, publicUserId, inspectionlist, inspector);
            }
            catch (ACAException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        /// <summary>
        /// Raises the page load event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetPageTitleKey("aca_inspection_title_cancelinspection");

            if (!Page.IsPostBack)
            {
                string cancelConfirmTextPattern = GetTextByKey("aca_inspection_confirm_context");
                string cancelInspectionText = GetTextByKey("aca_inspection_action_cancelinspection");
                string cancelActionText = GetTextByKey("ACA_Inspection_Action_Cancel");
                lblConfirmContext.Text = string.Format(cancelConfirmTextPattern, cancelInspectionText, cancelActionText);

                if (!AppSession.IsAdmin)
                {
                    lblInspectionTypeValue.Text = InspectionWizardParameter.TypeText;

                    if (InspectionWizardParameter.ScheduledDateTime == null)
                    {
                        lblDatetimeValue.Text = GetTextByKey("ACA_Inspection_BlankScheduleDate_TBD");
                    }
                    else if (InspectionWizardParameter.Status == InspectionStatus.PendingByACA &&
                         InspectionWizardParameter.ScheduleType == InspectionScheduleType.RequestSameDayNextDay)
                    {
                        lblDatetimeValue.Text = I18nDateTimeUtil.FormatToDateStringForUI(InspectionWizardParameter.ScheduledDateTime.Value);
                    }
                    else
                    {
                        lblDatetimeValue.Text = InspectionViewUtil.BuildDateTimeText(InspectionWizardParameter.ScheduledDateTime.Value, InspectionWizardParameter.TimeOption, ModuleName);
                    }

                    //show confirm message if inspection havs next inspection in advance
                    string agencyCode = InspectionWizardParameter.AgencyCode;

                    InspectionModel[] inspectionlist = new InspectionModel[1];                    
                    InspectionModel inspectionModel = GetInspectionModel(InspectionWizardParameter);
                    inspectionlist[0] = inspectionModel;

                    IInspectionBll inspectionBll = (IInspectionBll)ObjectFactory.GetObject(typeof(IInspectionBll));

                    string message = inspectionBll.GetConfirmMessageWhenCancel(inspectionlist, agencyCode, AppSession.User.PublicUserId);
                    
                    if (!string.IsNullOrEmpty(message))
                    {
                        MessageUtil.ShowMessageInPopup(Page, MessageType.Notice, message);
                    }
                }
                else
                {
                    divReschedulePolicy.Visible = false;
                    divCancellationPolicy.Visible = false;
                    divAdminPartOfPolicy.Visible = true;
                }

                // display the location
                CapModel4WS capModel = GetCapModel();

                if (capModel != null && !AppSession.IsAdmin)
                {
                    addressView.Display(capModel.addressModel, false);
                }

                // set the contact
                lblContactContent.Text = GetFormattedContactContent(InspectionWizardParameter);

                SetupActionRestrictionPolicyUI();

                SetContactByPermission();
            }
        }

        #endregion Public and Proected Methods

        #region Private Methods

        /// <summary>
        /// restore inspection type from session
        /// </summary>
        /// <param name="inspectionParameter">The inspection parameter.</param>
        /// <returns>
        /// the inspection type which is selected in previous step
        /// </returns>
        private static InspectionModel GetInspectionModel(InspectionParameter inspectionParameter)
        {
            CapIDModel capIDModel = new CapIDModel();
            capIDModel.ID1 = inspectionParameter.RecordID1;
            capIDModel.ID2 = inspectionParameter.RecordID2;
            capIDModel.ID3 = inspectionParameter.RecordID3;
            capIDModel.serviceProviderCode = inspectionParameter.AgencyCode;

            int idNumber = int.Parse(inspectionParameter.ID);
            ActivityModel activityModel = new ActivityModel();
            activityModel.idNumber = idNumber;
            activityModel.capID = capIDModel;
            activityModel.activityGroup = inspectionParameter.Group;
            activityModel.activityType = inspectionParameter.Type;
            activityModel.activityDescription = inspectionParameter.Type;
            activityModel.serviceProviderCode = inspectionParameter.AgencyCode;

            InspectionModel inspectionModel = new InspectionModel();
            inspectionModel.activity = activityModel;

            return inspectionModel;
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
        /// Set contact by permission
        /// </summary>
        private void SetContactByPermission()
        {
            ContactVisible = true;

            if (!AppSession.IsAdmin)
            {
                var inspectionPermissionBll = ObjectFactory.GetObject<IInspectionPermissionBll>();

                CapModel4WS capModel = GetCapModel();
                bool right = inspectionPermissionBll.CheckContactRight(capModel, InspectionWizardParameter.AgencyCode, ModuleName, ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_VIEW_CONTACT);

                if (right == false)
                {
                    lblContact.Visible = false;
                    lblContactContent.Visible = false;
                    ContactVisible = false;
                }
            }
        }

        #endregion Private Methods
    }
}
