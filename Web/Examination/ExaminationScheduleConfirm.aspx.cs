#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExaminationScheduleConfirm.aspx.cs
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
using System.Linq;
using System.Web.UI;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Finance;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Examination
{
    /// <summary>
    /// Examination Schedule Confirm
    /// </summary>
    public partial class ExaminationScheduleConfirm : ExaminationScheduleBasePage
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is no fee.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is no fee; otherwise, <c>false</c>.
        /// </value>
        public bool IsNoFeeItem { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the scheduling Exam is need payment
        /// <para>1. The exam is not in ready to schedule and it is has fee items.</para>
        /// <para>2. The exam is ready to schedule and the cap has not paid fee items.</para>
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is cap not payment fee items; otherwise, <c>false</c>.
        /// </value>
        public bool NeedPayment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is existing account.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is existing account; otherwise, <c>false</c>.
        /// </value>
        private bool IsExsitingAccount
        {
            get { return bool.Parse(ViewState["IsExsitingAccount"].ToString()); }
            set { ViewState["IsExsitingAccount"] = value; }
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
            if (!AppSession.IsAdmin)
            {
                RefFeeDsecVO[] feeItems = GetFeeItems();
                IsNoFeeItem = feeItems == null || feeItems.Length == 0;

                if (!IsPostBack)
                {
                    GetScheduleViewModel();

                    if (bool.Parse(ExaminationWizardParameter.IsExternal))
                    {
                        if (bool.Parse(ExaminationWizardParameter.IsOnline))
                        {
                            string buttonIds = "['" + lnkContinue.ClientID + "','" + lnkConfirm.ClientID + "','" + lnkPayNow.ClientID + "']";
                            SetPageTitleKey("aca_exam_schedule_onlineconfirm_page_title");
                            SetOnlineControl(feeItems);

                            string script = string.Format(
                                            "SelectExaminationConfirmRadio(this,'tableOnsite','{0}','{1}',{2})",
                                            txtExsitingAccount.ClientID,
                                            txtCreateAccount.ClientID,
                                            buttonIds);
                            rdExsitingAccount.Attributes.Add("onclick", script);

                            script = string.Format(
                                            "SelectExaminationConfirmRadio(this,'tableOnsite','{0}','{1}',{2})",
                                            txtCreateAccount.ClientID,
                                            txtExsitingAccount.ClientID,
                                            buttonIds);
                            rdCreateAccount.Attributes.Add("onclick", script);

                            SetExternalOnlineControlStatus();
                        }
                        else
                        {
                            SetPageTitleKey("aca_exam_schedule_onsiteconfirm_page_title");
                            RefExaminationExtenalOnSiteScheduleConfirm.Display(ExaminationWizardParameter, feeItems);
                            divExaminationExternalOnSite.Visible = true;
                            txtEMailAddress.Text = ExaminationWizardParameter.RecordContactEMail;
                        }
                    }
                    else
                    {
                        SetPageTitleKey("aca_exam_schedule_internalconfirm_page_title");
                        RefExaminationScheduleConfirm.Display(ExaminationWizardParameter, feeItems);
                    }

                    rdExsitingAccount.ToolTip = GetTextByKey(txtExsitingAccount.LabelKey, ModuleName);
                    rdCreateAccount.ToolTip = GetTextByKey(txtCreateAccount.LabelKey, ModuleName);
                    txtCreateAccount.Attributes.Add("onchange", "hideMessage();");
                    txtExsitingAccount.Attributes.Add("onchange", "hideMessage();");
                }

                IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
                bool isDeferPaymentEnabled = ValidationUtil.IsYes(bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_DEFER_PAYMENT_ENABLED));

                bool isNotReadyToSchdule = string.IsNullOrEmpty(ExaminationWizardParameter.ReadyToScheduleStatus);
                bool isPaidSchdule = !isNotReadyToSchdule &&
                    ACAConstant.EXAMINATION_STATUS_READY_TO_SCHEDULE_PAID.Equals(
                        ExaminationWizardParameter.ReadyToScheduleStatus, StringComparison.OrdinalIgnoreCase);
                bool isUnPaidSchdule = !isNotReadyToSchdule &&
                    ACAConstant.EXAMINATION_STATUS_READY_TO_SCHEDULE_UNPAID.Equals(
                        ExaminationWizardParameter.ReadyToScheduleStatus, StringComparison.OrdinalIgnoreCase);
                bool isReschdule = bool.Parse(ExaminationWizardParameter.IsReschedule);

                var feeBll = ObjectFactory.GetObject<IFeeBll>();
                CapModel4WS recordModel = AppSession.GetCapModelFromSession(ModuleName);
                var notPaidFeeItems = feeBll.GetNoPaidFeeItemByCapID(recordModel.capID, AppSession.User.PublicUserId);

                // NeedPayment is true below:
                // 1. The exam is not in ready to schedule and it is has fee items.
                // 2. The exam is ready to schedule and the cap has not paid fee items.
                NeedPayment = (isNotReadyToSchdule && !IsNoFeeItem)
                              || (!isNotReadyToSchdule && (notPaidFeeItems != null && notPaidFeeItems.Length != 0));

                lnkContinue.Visible = NeedPayment && !isReschdule && isDeferPaymentEnabled && !isPaidSchdule;
                lnkConfirm.Visible = !NeedPayment || isReschdule || isPaidSchdule || IsNoFeeItem;
                lnkPayNow.Visible = NeedPayment && !isReschdule && (isNotReadyToSchdule || isUnPaidSchdule);

                if (!string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleLocationId)
                    && !isNotReadyToSchdule
                    && Convert.ToInt32(ExaminationWizardParameter.ExamScheduleSeats) <= 0
                    && !bool.Parse(ExaminationWizardParameter.IsExternal))
                {
                    SetWizardButtonDisable(lnkContinue.ClientID, true);
                    SetWizardButtonDisable(lnkPayNow.ClientID, true);
                    SetWizardButtonDisable(lnkConfirm.ClientID, true);
                    MessageUtil.ShowMessageInPopup(this.Page, MessageType.Error, this.GetTextByKey("aca_exam_schedule_message_nomoreseats"));
                }

                //when examination schedule is expired, not allow user to schedule this examination. click back to select other session
                if (!string.IsNullOrEmpty(ExaminationWizardParameter.IsAllowedBeyondDate) && bool.Parse(ExaminationWizardParameter.IsAllowedBeyondDate))
                {
                    SetWizardButtonDisable(lnkContinue.ClientID, true);
                    SetWizardButtonDisable(lnkPayNow.ClientID, true);
                    SetWizardButtonDisable(lnkConfirm.ClientID, true);
                    MessageUtil.ShowMessageInPopup(this.Page, MessageType.Error, this.GetTextByKey("aca_exam_schedule_message_exam_expired"));
                }

                TemplateModel model = ExaminationWizardParameter.Template;

                if (model == null && !string.IsNullOrEmpty(ExaminationWizardParameter.DailyExaminationNbr))
                {
                    if (recordModel.examinationList != null && recordModel.examinationList.Length > 0)
                    {
                        ExaminationModel examModel =
                            recordModel.examinationList.FirstOrDefault(
                                o => o.examinationPKModel.examNbr == Convert.ToInt64(ExaminationWizardParameter.DailyExaminationNbr));

                        if (examModel != null && Convert.ToString(examModel.refExamSeq) == ExaminationWizardParameter.ExaminationNbr)
                        {
                            model = examModel.template;
                        }
                    }
                }

                if (model == null)
                {
                    ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
                    model = templateBll.GetGenericTemplateStructureByEntityPKModel(
                        new EntityPKModel()
                            {
                                serviceProviderCode = ExaminationWizardParameter.RecordAgencyCode,
                                entityType = (int)GenericTemplateEntityType.RefExamination,
                                seq1 = Convert.ToInt64(ExaminationWizardParameter.ExaminationNbr)
                            },
                        false,
                        AppSession.User.UserSeqNum);
                }

                if (model != null && !IsPostBack)
                {
                    this.genericTemplate.Display(model);
                }
            }

            this.SetAdminUI();

            this.SetDialogMaxHeight("600");
        }

        /// <summary>
        /// Handles the Click event of the Continue Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            MessageUtil.HideMessageByControl(Page);

            if (!ValidationExternal())
            {
                SetExternalOnlineControlStatus();
                return;
            }

            ScheduleExamParamVO paramVo = GetScheduleModel();

            try
            {
                this.ScheduleProcessWithoutPayment(paramVo);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                MessageUtil.ShowMessageInPopup(Page, MessageType.Error, err);

                if (bool.Parse(ExaminationWizardParameter.IsExternal)
                        && bool.Parse(ExaminationWizardParameter.IsOnline))
                {
                    SetExternalOnlineControlStatus();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the PayNow button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void PayNowButton_Click(object sender, EventArgs e)
        {
            try
            {
                MessageUtil.HideMessageByControl(Page);

                if (!ValidationExternal())
                {
                    SetExternalOnlineControlStatus();
                    return;
                }

                IExaminationBll examinationBll = ObjectFactory.GetObject<IExaminationBll>();
                ScheduleExamParamVO paramVo = GetScheduleModel();

                if (bool.Parse(ExaminationWizardParameter.IsReschedule) || IsNoFeeItem)
                {
                    this.ScheduleProcessWithoutPayment(paramVo);
                }
                else if ((string.IsNullOrEmpty(ExaminationWizardParameter.ReadyToScheduleStatus) && NeedPayment)
                    || ACAConstant.EXAMINATION_STATUS_READY_TO_SCHEDULE_UNPAID.Equals(ExaminationWizardParameter.ReadyToScheduleStatus))
                {
                    if (examinationBll.PreSchduleExam(paramVo))
                    {
                        this.Back2ParentPage();
                    }
                    else
                    {
                        GotoPayFeePage();
                    }
                }
                else if (ACAConstant.EXAMINATION_STATUS_READY_TO_SCHEDULE_PAID.Equals(ExaminationWizardParameter.ReadyToScheduleStatus) || !NeedPayment)
                {
                    if (!examinationBll.PreSchduleExam(paramVo))
                    {
                        var feeBll = ObjectFactory.GetObject<IFeeBll>();
                        var notPaidFeeItems = feeBll.GetNoPaidFeeItemByCapID(TempModelConvert.Add4WSForCapIDModel(paramVo.capID), AppSession.User.PublicUserId);

                        if (notPaidFeeItems != null && notPaidFeeItems.Length > 0)
                        {
                            GotoPayFeePage();
                        }
                        else
                        {
                            examinationBll.SchedulePaidExamByPK(
                                new ExaminationPKModel()
                                {
                                    serviceProviderCode = ExaminationWizardParameter.RecordAgencyCode,
                                    examNbr = Convert.ToInt64(ExaminationWizardParameter.DailyExaminationNbr)
                                },
                                AppSession.User.PublicUserId);   
                        }
                    }

                    this.Back2ParentPage();
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                MessageUtil.ShowMessageInPopup(Page, MessageType.Error, err);

                if (bool.Parse(ExaminationWizardParameter.IsExternal)
                        && bool.Parse(ExaminationWizardParameter.IsOnline))
                {
                    SetExternalOnlineControlStatus();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the Back button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BackButton_Click(object sender, EventArgs e)
        {
            //clear exam expired status
            ExaminationWizardParameter.IsAllowedBeyondDate = string.Empty;
            string previousURL = ExaminationScheduleUtil.GetWizardPreviousURL(ExaminationWizardParameter);
            ExaminationWizardParameter.Template = genericTemplate.GetTemplateModel(true);

            if (!string.IsNullOrEmpty(previousURL))
            {
                Response.Redirect(ExaminationParameterUtil.UpdateURLAndSaveParameters(previousURL, ExaminationWizardParameter));
            }
        }

        /// <summary>
        /// Goto the pay fee page.
        /// </summary>
        private void GotoPayFeePage()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RedirectToPayFeePage", "RedirectToPayFeePage();", true);
        }

        /// <summary>
        /// Runs the script.
        /// </summary>
        private void SetExternalOnlineControlStatus()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "disablebutton" + new Guid().ToString(), "<script>disablebutton();</script>", false);
        }

        /// <summary>
        /// Gets the schedule view model.
        /// </summary>
        private void GetScheduleViewModel()
        {
            var examBll = ObjectFactory.GetObject<IExaminationBll>();
            var examScheduleViewModel = new ExamScheduleViewModel();

            examScheduleViewModel.serviceProviderCode = ExaminationWizardParameter.RecordAgencyCode;
            examScheduleViewModel.weekday = ExaminationWizardParameter.ExamScheduleWeekDay;

            if (string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleScheduleId))
            {
                examScheduleViewModel.scheduleID = null;
            }
            else
            {
                examScheduleViewModel.scheduleID = long.Parse(ExaminationWizardParameter.ExamScheduleScheduleId);
            }

            examScheduleViewModel.availableSeats = int.Parse(ExaminationWizardParameter.ExamScheduleSeats);

            if (string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleCalendarId))
            {
                examScheduleViewModel.calendarID = null;
            }
            else
            {
                examScheduleViewModel.calendarID = long.Parse(ExaminationWizardParameter.ExamScheduleCalendarId);
            }

            if (string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleLocationId))
            {
                examScheduleViewModel.locationID = null;
            }
            else
            {
                examScheduleViewModel.locationID = long.Parse(ExaminationWizardParameter.ExamScheduleLocationId);
            }

            if (string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleStartTime))
            {
                examScheduleViewModel.startTime = null;
            }
            else
            {
                examScheduleViewModel.startTime = I18nDateTimeUtil.ParseFromUI(ExaminationWizardParameter.ExamScheduleDate + " " + ExaminationWizardParameter.ExamScheduleStartTime);
            }

            if (string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleEndTime))
            {
                examScheduleViewModel.endTime = null;
            }
            else
            {
                examScheduleViewModel.endTime = I18nDateTimeUtil.ParseFromUI(ExaminationWizardParameter.ExamScheduleEndTime + " " + ExaminationWizardParameter.ExamScheduleDate);
            }

            if (string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleDate))
            {
                examScheduleViewModel.date = null;
            }
            else
            {
                examScheduleViewModel.date = I18nDateTimeUtil.ParseFromUI(ExaminationWizardParameter.ExamScheduleDate);
            }

            examScheduleViewModel.examName = ExaminationWizardParameter.ExaminationName;
            examScheduleViewModel.refExamNbr = long.Parse(ExaminationWizardParameter.ExaminationNbr);

            examScheduleViewModel.providerNbr = long.Parse(string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleProviderNbr) ? "0" : ExaminationWizardParameter.ExamScheduleProviderNbr);

            var examViewModel = examBll.GetExamScheduleViewModel(examScheduleViewModel);

            ExaminationWizardParameter.ExamInstructions = examViewModel.instructions;
            ExaminationWizardParameter.ExamScheduleLang = examViewModel.supportLang;

            RProviderLocationModel lc = examViewModel.locationModel;

            if (lc != null)
            {
                ExaminationWizardParameter.DrivingDesc = lc.drivingDirections;
                ExaminationWizardParameter.AccessiblityDesc = lc.handicapAccessible;
            }
        }

        /// <summary>
        /// Schedules the or reschedule examination.
        /// </summary>
        /// <param name="paramVo">The parameter vo.</param>
        /// <returns>the schedule status</returns>
        private bool ScheduleOrRescheduleExamination(ScheduleExamParamVO paramVo)
        {
            IExaminationBll examBll = ObjectFactory.GetObject<IExaminationBll>();

            if (!bool.Parse(ExaminationWizardParameter.IsReschedule))
            {
                return examBll.ScheduleExam(paramVo);
            }
            else
            {
                return examBll.RescheduleExam(paramVo);
            }
        }

        /// <summary>
        /// Sets the online control.
        /// </summary>
        /// <param name="feeItems">The fee items.</param>
        private void SetOnlineControl(RefFeeDsecVO[] feeItems)
        {
            if (string.IsNullOrEmpty(ExaminationWizardParameter.RecordContactEMail))
            {
                IsExsitingAccount = false;
                RefExaminationExtenalOnlineScheduleConfirm.Display(ExaminationWizardParameter, feeItems, null);
                divExaminationExternalOnline.Visible = true;
                SetWizardButtonDisable(lnkContinue.ClientID, true);
                SetWizardButtonDisable(lnkPayNow.ClientID, true);
                SetWizardButtonDisable(lnkConfirm.ClientID, true);
            }
            else
            {
                var accountBll = (IAccountBll)ObjectFactory.GetObject(typeof(IAccountBll));
                PublicUserModel4WS userModel = null;
                string userSeqNbr = null;
                bool flag = false;

                try
                {
                    userSeqNbr = accountBll.ValidatePublicUserAccount(ExaminationWizardParameter.RecordAgencyCode, ExaminationWizardParameter.RecordContactEMail);
                }
                catch (Exception ex)
                {
                    // It means the public account might inactive or disable
                    flag = true;
                    MessageUtil.ShowMessageInPopup(Page, MessageType.Error, ex.Message);
                }

                if (!string.IsNullOrEmpty(userSeqNbr))
                {
                    userModel = accountBll.GetPublicUserByEmailOrUserId(ExaminationWizardParameter.RecordAgencyCode, ExaminationWizardParameter.RecordContactEMail);
                    IsExsitingAccount = true;
                    divExaminationExternalOnline.Visible = false;
                }
                else if (flag)
                {
                    IsExsitingAccount = false;
                    divExaminationExternalOnline.Visible = true;
                    txtExsitingAccount.Text = ExaminationWizardParameter.RecordContactEMail;
                    rdExsitingAccount.Checked = true;
                }
                else
                {
                    IsExsitingAccount = false;
                    divExaminationExternalOnline.Visible = true;
                    txtCreateAccount.Text = ExaminationWizardParameter.RecordContactEMail;
                    rdCreateAccount.Checked = true;
                }

                RefExaminationExtenalOnlineScheduleConfirm.Display(ExaminationWizardParameter, feeItems, userModel);
            }
        }

        /// <summary>
        /// Gets the fee item.
        /// </summary>
        /// <returns>fee item arrays</returns>
        private RefFeeDsecVO[] GetFeeItems()
        {
            IProviderBll providerBll = (IProviderBll)ObjectFactory.GetObject(typeof(IProviderBll));
            RefFeeDsecVO[] refFeeDsecVo = providerBll.GetProviderFeeItems(
                                                                          ExaminationWizardParameter.RecordAgencyCode,
                                                                          ExaminationWizardParameter.ExaminationNbr,
                                                                          "EXAM",
                                                                          ExaminationWizardParameter.ExamScheduleProviderNbr);
            return refFeeDsecVo;
        }

        /// <summary>
        /// Gets the schedule model.
        /// </summary>
        /// <returns> schedule parameter </returns>
        private ScheduleExamParamVO GetScheduleModel()
        {
            ScheduleExamParamVO paramVo = new ScheduleExamParamVO();

            if (string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleCalendarId))
            {
                paramVo.calendarID = null;
            }
            else
            {
                paramVo.calendarID = long.Parse(ExaminationWizardParameter.ExamScheduleCalendarId);
            }

            paramVo.callerID = AppSession.User.PublicUserId;

            if (string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleEndTime))
            {
                paramVo.endTime = null;
            }
            else
            {
                paramVo.endTime = I18nDateTimeUtil.ParseFromUI(ExaminationWizardParameter.ExamScheduleEndTime + " " + ExaminationWizardParameter.ExamScheduleDate);
            }

            if (string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleDate))
            {
                paramVo.examDate = null;
            }
            else
            {
                paramVo.examDate = I18nDateTimeUtil.ParseFromUI(ExaminationWizardParameter.ExamScheduleDate);
            }

            paramVo.examName = ExaminationWizardParameter.ExaminationName;
            paramVo.refExamNbr = long.Parse(ExaminationWizardParameter.ExaminationNbr);

            if (string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleLocationId))
            {
                paramVo.locationID = null;
            }
            else
            {
                paramVo.locationID = long.Parse(ExaminationWizardParameter.ExamScheduleLocationId);
            }

            //paramVo.proctorID = 0; //what?
            paramVo.providerNbr = long.Parse(string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleProviderNbr) ? "0" : ExaminationWizardParameter.ExamScheduleProviderNbr);

            if (string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleDate))
            {
                paramVo.scheduleDate = null;
            }
            else
            {
                paramVo.scheduleDate = I18nDateTimeUtil.ParseFromUI(ExaminationWizardParameter.ExamScheduleDate);
            }

            if (string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleScheduleId))
            {
                paramVo.scheduleID = null;
            }
            else
            {
                paramVo.scheduleID = long.Parse(ExaminationWizardParameter.ExamScheduleScheduleId);
            }

            if (!bool.Parse(ExaminationWizardParameter.IsReschedule))
            {
                paramVo.scheduleType = ACAConstant.EXAMINATION_SCHEDULE_TYPE_SCHEDULE;
            }
            else
            {
                paramVo.scheduleType = ACAConstant.EXAMINATION_SCHEDULE_TYPE_RESCHEDULE;
            }

            if (!string.IsNullOrEmpty(ExaminationWizardParameter.DailyExaminationNbr))
            {
                paramVo.examSeqNbr = long.Parse(ExaminationWizardParameter.DailyExaminationNbr);
            }

            paramVo.reschedCancelReason = GetReasonString();
            paramVo.serviceProviderCode = ExaminationWizardParameter.RecordAgencyCode;

            if (string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleStartTime))
            {
                paramVo.startTime = null;
            }
            else
            {
                paramVo.startTime = I18nDateTimeUtil.ParseFromUI(ExaminationWizardParameter.ExamScheduleDate + " " + ExaminationWizardParameter.ExamScheduleStartTime);
            }

            paramVo.capID = new CapIDModel()
                                {
                                    ID1 = ExaminationWizardParameter.RecordID1,
                                    ID2 = ExaminationWizardParameter.RecordID2,
                                    ID3 = ExaminationWizardParameter.RecordID3,
                                    serviceProviderCode = ExaminationWizardParameter.RecordAgencyCode
                                };

            if (bool.Parse(ExaminationWizardParameter.IsExternal))
            {
                SetExternalFields2Model(paramVo);
            }
            else
            {
                paramVo.existingACAUser = true;
            }

            paramVo.isBeyondAllowanceDate = string.IsNullOrEmpty(ExaminationWizardParameter.IsAllowedBeyondDate)
                                                ? false
                                                : bool.Parse(ExaminationWizardParameter.IsAllowedBeyondDate);
            paramVo.fromACA = true;

            paramVo.template = this.genericTemplate.GetTemplateModel(true);

            return paramVo;
        }

        /// <summary>
        /// Sets the external fields to model.
        /// </summary>
        /// <param name="paramVo">The parameter vo.</param>
        private void SetExternalFields2Model(ScheduleExamParamVO paramVo)
        {
            var providerBll = (IAccountBll)ObjectFactory.GetObject(typeof(IAccountBll));

            paramVo.studentInfo = new StudentInfoVO()
                                      {
                                          email = ExaminationWizardParameter.RecordContactEMail,
                                          firstName = ExaminationWizardParameter.RecordContactFirstName,
                                          fullName = ExaminationWizardParameter.RecordContactFullName,
                                          lastName = ExaminationWizardParameter.RecordContactLastName
                                      };

            if (bool.Parse(ExaminationWizardParameter.IsOnline))
            {
                if (IsExsitingAccount)
                {
                    paramVo.existingACAUser = true;
                    paramVo.inputEmail = ExaminationWizardParameter.RecordContactEMail;
                }
                else
                {
                    GetOnlineExistingAccount(paramVo);
                }
            }
            else
            {
                paramVo.existingACAUser = false;
                var userModel = providerBll.GetPublicUserByEmailOrUserId(ExaminationWizardParameter.RecordAgencyCode, txtEMailAddress.Text);

                if (userModel != null && !string.IsNullOrEmpty(userModel.userSeqNum))
                {
                    paramVo.existingACAUser = true;
                }

                paramVo.inputEmail = txtEMailAddress.Text;
            }
        }

        /// <summary>
        /// Gets the on site existing account.
        /// </summary>
        /// <param name="paramVo">The parameter vo.</param>
        private void GetOnlineExistingAccount(ScheduleExamParamVO paramVo)
        {
            if (rdCreateAccount.Checked)
            {
                paramVo.existingACAUser = false;
                paramVo.inputEmail = txtCreateAccount.Text;
                paramVo.studentInfo.email = txtCreateAccount.Text;
            }

            if (rdExsitingAccount.Checked)
            {
                paramVo.existingACAUser = true;
                paramVo.inputEmail = txtExsitingAccount.Text;
                paramVo.studentInfo.email = txtExsitingAccount.Text;
            }
        }

        /// <summary>
        /// Sets the admin UI.
        /// </summary>
        private void SetAdminUI()
        {
            if (AppSession.IsAdmin)
            {
                string adminType = Request.QueryString["Type"];
                var feeItems = new RefFeeDsecVO[0];

                if (adminType == "internal")
                {
                    this.SetPageTitleKey("aca_exam_schedule_internalconfirm_page_title");
                    RefExaminationScheduleConfirm.Display(ExaminationWizardParameter, feeItems);
                }
                else if (adminType == "online")
                {
                    this.SetPageTitleKey("aca_exam_schedule_onlineconfirm_page_title");
                    RefExaminationExtenalOnlineScheduleConfirm.Display(ExaminationWizardParameter, feeItems, new PublicUserModel4WS());

                    if (string.IsNullOrEmpty(ExaminationWizardParameter.RecordContactEMail))
                    {
                        divExaminationExternalOnline.Visible = true;
                    }
                }
                else if (adminType == "onsite")
                {
                    this.SetPageTitleKey("aca_exam_schedule_onsiteconfirm_page_title");
                    RefExaminationExtenalOnSiteScheduleConfirm.Display(ExaminationWizardParameter, feeItems);

                    divExaminationExternalOnSite.Visible = true;
                    txtEMailAddress.Text = ExaminationWizardParameter.RecordContactEMail;
                }
            }
        }

        /// <summary>
        /// Validations the external.
        /// </summary>
        /// <returns> validation status</returns>
        private bool ValidationExternal()
        {
            bool isValidation = true;

            if (bool.Parse(ExaminationWizardParameter.IsExternal))
            {
                if (bool.Parse(ExaminationWizardParameter.IsOnline))
                {
                    if (!IsExsitingAccount)
                    {
                        try
                        {
                            string emailID = rdCreateAccount.Checked ? txtCreateAccount.Text.Trim() : txtExsitingAccount.Text.Trim();
                            var accountBll = (IAccountBll)ObjectFactory.GetObject(typeof(IAccountBll));

                            //The input data can be user name or email address in the textbox of register user.
                            string userSeqNbr = accountBll.ValidatePublicUserAccount(ExaminationWizardParameter.RecordAgencyCode, emailID);

                            if (rdCreateAccount.Checked)
                            {
                                if (!string.IsNullOrEmpty(userSeqNbr))
                                {
                                    isValidation = false;
                                    MessageUtil.ShowMessageInPopup(Page, MessageType.Error, string.Format(GetTextByKey("aca_exam_schedule_message_exsitingaccount"), txtCreateAccount.Text.Trim()));
                                }
                            }
                            else if (rdExsitingAccount.Checked)
                            {
                                if (string.IsNullOrEmpty(userSeqNbr))
                                {
                                    isValidation = false;
                                    MessageUtil.ShowMessageInPopup(Page, MessageType.Error, GetTextByKey("aca_exam_schedule_message_notexsitingaccount"));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageUtil.ShowMessageInPopup(Page, MessageType.Error, ex.Message);
                            isValidation = false;
                        }
                    }
                }
                else
                {
                    //onsite validation
                }

                if (string.IsNullOrEmpty(ExaminationWizardParameter.RecordContactFirstName) || string.IsNullOrEmpty(ExaminationWizardParameter.RecordContactLastName))
                {
                    isValidation = false;
                    string msg = string.Format(GetTextByKey("aca_exam_schedule_msg_noname"), ExaminationWizardParameter.ExamScheduleProviderName);
                    MessageUtil.ShowMessageInPopup(Page, MessageType.Error, msg);
                }
            }

            return isValidation;
        }

        /// <summary>
        /// Gets the reason string.
        /// </summary>
        /// <returns>The reason.</returns>
        private string GetReasonString()
        {
            string reasonString = null;

            if (bool.Parse(ExaminationWizardParameter.IsReschedule))
            {
                IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
                var domainModel = bizBll.GetBizDomainListByModel(
                                                                new BizDomainModel4WS()
                                                                    {
                                                                        serviceProviderCode = ExaminationWizardParameter.RecordAgencyCode,
                                                                        dispositionID = long.Parse(ExaminationWizardParameter.ReasonID),
                                                                        bizdomain = BizDomainConstant.BIZDOMAIN_RESCHED_CANCEL_REASON
                                                                    },
                                                                AppSession.User.PublicUserId);

                if (domainModel != null)
                {
                    reasonString = string.IsNullOrEmpty(domainModel.resBizdomainValue)
                                       ? domainModel.bizdomainValue
                                       : domainModel.resBizdomainValue;
                }
            }

            return reasonString;
        }

        /// <summary>
        /// Schedule process without payment.
        /// </summary>
        /// <param name="paramVo">The parameter vo.</param>
        private void ScheduleProcessWithoutPayment(ScheduleExamParamVO paramVo)
        {
            if (ScheduleOrRescheduleExamination(paramVo))
            {
                Back2ParentPage();
            }
        }

        /// <summary>
        /// back to parent page when schedule successful.
        /// </summary>
        private void Back2ParentPage()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "GotoSuccessfulPage", "BackSuccessful();", true);
        }

        #endregion
    }
}