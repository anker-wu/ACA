#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExaminationUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ExaminationUtil.cs 142445 2009-08-07 08:19:22Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Examination;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Some Common methods for examination model
    /// </summary>
    public static class ExaminationUtil
    {
        #region Fields

        /// <summary>
        /// Examination url of view details page
        /// </summary>
        private const string EXAMINATION_URL_VIEWDETAILS = "Examination/ExaminationScheduleView.aspx";

        /// <summary>
        /// Examination url of edit page
        /// </summary>
        private const string EXAMINATION_URL_EDIT = "Examination/ExaminationEdit.aspx";

        /// <summary>
        /// Examination url of schedule examination page
        /// </summary>
        private const string EXAMINATION_URL_SCHEDULE = "Examination/AvailableExaminationScheduleList.aspx";

        /// <summary>
        /// Examination url of reschedule Reason page
        /// </summary>
        private const string EXAMINATION_URL_RESCHEDULE_REASON = "Examination/ExaminationRescheduleReason.aspx";

        /// <summary>
        /// Examination url of delete page
        /// </summary>
        private const string EXAMINATION_URL_DELETE = "Examination/ExaminationDelete.aspx";

        /// <summary>
        /// Examination url of cancel page
        /// </summary>
        private const string EXAMINATION_URL_CANCEL = "Examination/ExaminationCancellation.aspx";

        /// <summary>
        /// Examination url of schedule confirm page
        /// </summary>
        private const string EXAMINATION_URL_SCHEDULE_CONFIRM = "Examination/ExaminationScheduleConfirm.aspx";

        #endregion Fields

        #region For ref examinations util

        /// <summary>
        /// Converts to examination model array.
        /// </summary>
        /// <param name="refExaminationModels">The reference examination models.</param>
        /// <returns>The examination model array.</returns>
        public static ExaminationModel[] ConvertToExaminationModelArray(RefExaminationModel[] refExaminationModels)
        {
            List<ExaminationModel> examModels = new List<ExaminationModel>();

            foreach (var refExam in refExaminationModels)
            {
                examModels.Add(ConvertToExaminationModel(refExam));
            }

            return examModels.ToArray();
        }

        /// <summary>
        /// Converts to examination model.
        /// </summary>
        /// <param name="refExamination">The reference examination.</param>
        /// <returns>The examination model.</returns>
        public static ExaminationModel ConvertToExaminationModel(RefExaminationModel refExamination)
        {
            ExaminationModel examinationModel = new ExaminationModel();
            ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();

            examinationModel.examinationPKModel = new ExaminationPKModel();
            examinationModel.examName = refExamination.examName;
            examinationModel.gradingStyle = refExamination.gradingStyle;
            examinationModel.requiredFlag = ACAConstant.COMMON_Y;
            examinationModel.approvedFlag = ACAConstant.COMMON_N;
            examinationModel.examinationPKModel.serviceProviderCode = refExamination.serviceProviderCode;

            double passingScore = 0;

            if (I18nNumberUtil.TryParseNumberFromWebService(refExamination.passingScore, out passingScore))
            {
                examinationModel.passingScore = passingScore;
            }

            string defaultCountryCode = StandardChoiceUtil.GetDefaultCountry();

            if (!string.IsNullOrEmpty(defaultCountryCode))
            {
                IBizdomainProvider bizProvider = ObjectFactory.GetObject<IBizdomainProvider>();
                IList<ItemValue> countryIDDList = bizProvider.GetBizDomainList(BizDomainConstant.STD_CAT_COUNTRY_IDD);
                var iddItem = countryIDDList.FirstOrDefault(o => defaultCountryCode.Equals(o.Key));
                string iddString = iddItem == null ? string.Empty : iddItem.Value.ToString();

                examinationModel.examProviderDetailModel = new ExamProviderDetailModel()
                {
                    countryCode = defaultCountryCode,
                    faxCountryCode = iddString,
                    phone1CountryCode = iddString,
                    phone2CountryCode = iddString
                };
            }

            examinationModel.auditModel = new SimpleAuditModel();
            examinationModel.auditModel.auditID = AppSession.User.PublicUserId;
            examinationModel.auditModel.auditStatus = ACAConstant.VALID_STATUS;
            examinationModel.refExamSeq = refExamination.refExamNbr;
            examinationModel.examStatus = ACAConstant.EXAMINATION_STATUS_PENDING;
            examinationModel.entityType = ACAConstant.CAP_EXAMINATION_ENTITY_TYPE;
            examinationModel.FromCapAssociate = true;

            examinationModel.template = templateBll.GetGenericTemplateStructureByEntityPKModel(
                new EntityPKModel()
                {
                    serviceProviderCode = refExamination.serviceProviderCode,
                    entityType = (int)GenericTemplateEntityType.RefExamination,
                    seq1 = refExamination.refExamNbr
                },
                false,
                AppSession.User.PublicUserId);
            examinationModel.entityType = ACAConstant.CAP_EXAMINATION_ENTITY_TYPE;

            return examinationModel;
        }

        /// <summary>
        /// Convert RefExamination[] to Examination[]
        /// </summary>
        /// <param name="refExaminations">RefExamination ArrayList</param>
        /// <returns>Examination ArrayList</returns>
        public static ExaminationModel[] ConvertRefExaminationsToExaminations(RefExaminationModel4WS[] refExaminations)
        {
            IList<ExaminationModel> examinationList = new List<ExaminationModel>();

            if (refExaminations != null && refExaminations.Length > 0)
            {
                ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
                foreach (RefExaminationModel4WS refExamination in refExaminations)
                {
                    ExaminationModel examination = new ExaminationModel();
                    examination.examinationPKModel = new ExaminationPKModel();
                    examination.examName = refExamination.examName;
                    examination.gradingStyle = refExamination.gradingStyle;
                    examination.requiredFlag = ACAConstant.COMMON_Y;
                    examination.approvedFlag = ACAConstant.COMMON_N;
                    examination.examinationPKModel.serviceProviderCode = refExamination.serviceProviderCode;

                    double passingScore = 0;
                    if (I18nNumberUtil.TryParseNumberFromWebService(refExamination.passingScore, out passingScore))
                    {
                        examination.passingScore = passingScore;
                    }

                    string defaultCountryCode = StandardChoiceUtil.GetDefaultCountry();

                    if (!string.IsNullOrEmpty(defaultCountryCode))
                    {
                        IBizdomainProvider bizProvider = ObjectFactory.GetObject<IBizdomainProvider>();
                        IList<ItemValue> countryIDDList = bizProvider.GetBizDomainList(BizDomainConstant.STD_CAT_COUNTRY_IDD);
                        var iddItem = countryIDDList.FirstOrDefault(o => defaultCountryCode.Equals(o.Key));
                        string iddString = iddItem == null ? string.Empty : iddItem.Value.ToString();

                        examination.examProviderDetailModel = new ExamProviderDetailModel()
                        {
                            countryCode = defaultCountryCode,
                            faxCountryCode = iddString,
                            phone1CountryCode = iddString,
                            phone2CountryCode = iddString
                        };
                    }

                    examination.auditModel = new SimpleAuditModel();
                    examination.auditModel.auditID = AppSession.User.PublicUserId;
                    examination.auditModel.auditStatus = ACAConstant.VALID_STATUS;
                    examination.refExamSeq = refExamination.refExamNbr;
                    examination.examStatus = ACAConstant.EXAMINATION_STATUS_PENDING;
                    examination.entityType = ACAConstant.CAP_EXAMINATION_ENTITY_TYPE;
                    examination.FromCapAssociate = true;

                    examination.template = templateBll.GetGenericTemplateStructureByEntityPKModel(
                        new EntityPKModel()
                        {
                            serviceProviderCode = refExamination.serviceProviderCode,
                            entityType = (int)GenericTemplateEntityType.RefExamination,
                            seq1 = refExamination.refExamNbr
                        },
                        false,
                        AppSession.User.PublicUserId);
                    examination.entityType = ACAConstant.CAP_EXAMINATION_ENTITY_TYPE;

                    examinationList.Add(examination);
                }
            }

            ExaminationModel[] examinations = new ExaminationModel[examinationList.Count];
            examinationList.CopyTo(examinations, 0);
            return examinations;
        }

        /// <summary>
        /// Gets the examination required status.
        /// </summary>
        /// <param name="examinationModel">The examination model.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>The examination required status.</returns>
        public static string GetExamationRequiredStatus(RefExaminationModel4WS examinationModel, string moduleName)
        {
            string requiredValue = BasePage.GetStaticTextByKey("ACA_Common_No");

            if (examinationModel != null && examinationModel.refExamAppTypeModels != null && examinationModel.refExamAppTypeModels.Length > 0)
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

                if (capModel == null)
                {
                    return requiredValue;
                }

                foreach (XRefExaminationAppTypeModel4WS xRefEaxamAppType in examinationModel.refExamAppTypeModels)
                {
                    if (xRefEaxamAppType.group == capModel.capType.group 
                        && xRefEaxamAppType.type == capModel.capType.type
                        && xRefEaxamAppType.subType == capModel.capType.subType
                        && xRefEaxamAppType.category == capModel.capType.category)
                    {
                        requiredValue = EducationUtil.ConvertRequiredField2Display(xRefEaxamAppType.required);
                        break;
                    }
                }
            }

            return requiredValue;
        }

        /// <summary>
        /// Add RowIndex Property
        /// </summary>
        /// <param name="examinationList">ExaminationModel4WS List</param>
        /// <returns>ExaminationModel4WS List Collection</returns>
        public static IList<ExaminationModel> AddRowIndex(IList<ExaminationModel> examinationList)
        {
            IList<ExaminationModel> addIndexExaminationList = examinationList;

            if (addIndexExaminationList != null && addIndexExaminationList.Count > 0)
            {
                for (int i = 0; i < addIndexExaminationList.Count; i++)
                {
                    addIndexExaminationList[i].RowIndex = i;
                }
            }

            return addIndexExaminationList;
        }

        #endregion

        #region For examination action util

        /// <summary>
        /// Builds the examination action view models.
        /// </summary>
        /// <param name="moduleName">The module name</param>
        /// <param name="examinationModel">The examination model</param>
        /// <param name="ssoLink">The SSO link.</param>
        /// <param name="isCurrentUser">if set to <c>true</c> [is current user].</param>
        /// <param name="readOnly">is readonly</param>
        /// <returns>The action menu.</returns>
        public static List<ExaminationActionViewModel> GetActionsMenu(string moduleName, ExaminationModel examinationModel, string ssoLink, bool isCurrentUser, bool readOnly)
        {
            List<ExaminationActionViewModel> availableActions = new List<ExaminationActionViewModel>();

            if (examinationModel == null)
            {
                return availableActions;
            }

            if (ACAConstant.EXAMINATION_STATUS_SCHEDULE.Equals(examinationModel.examStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                availableActions.Add(GetActionsMenuItem(ExaminationAction.ViewDetails, moduleName, examinationModel.examinationPKModel.examNbr.ToString(), examinationModel.examinationPKModel.serviceProviderCode));
                IRefExaminationBll refExaminationBll = ObjectFactory.GetObject<IRefExaminationBll>();
                bool isBeyondAllowanceDate = examinationModel.refExamSeq != null && examinationModel.startTime != null && refExaminationBll.IsBeyondAllowanceDate(examinationModel.examinationPKModel.serviceProviderCode, examinationModel.providerNo, examinationModel.startTime.Value, examinationModel.refExamSeq.Value);

                if (!readOnly && !isBeyondAllowanceDate)
                {
                    availableActions.Add(GetActionsMenuItem(ExaminationAction.Reschedule, moduleName, examinationModel.examinationPKModel.examNbr.ToString(), examinationModel.examinationPKModel.serviceProviderCode));
                    availableActions.Add(GetActionsMenuItem(ExaminationAction.Cancel, moduleName, examinationModel.examinationPKModel.examNbr.ToString(), examinationModel.examinationPKModel.serviceProviderCode));

                    if (isCurrentUser && !string.IsNullOrEmpty(ssoLink))
                    {
                        availableActions.Add(GetActionsMenuItemToTakeExamination(ExaminationAction.TakeExamination, moduleName, ssoLink));
                    }
                }
            }

            if (ACAConstant.EXAMINATION_STATUS_PENDING.Equals(examinationModel.examStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                availableActions.Add(GetActionsMenuItem(ExaminationAction.Edit, moduleName, examinationModel.examinationPKModel.examNbr.ToString(), examinationModel.examinationPKModel.serviceProviderCode));

                if (!readOnly)
                {
                    if (examinationModel.refExamSeq != null && examinationModel.refExamSeq.Value != 0)
                    {
                        availableActions.Add(GetActionsMenuItem(ExaminationAction.Schedule, moduleName, examinationModel.examinationPKModel.examNbr.ToString(), examinationModel.examinationPKModel.serviceProviderCode));
                    }

                    if (!ACAConstant.COMMON_Y.Equals(examinationModel.requiredFlag, StringComparison.InvariantCultureIgnoreCase))
                    {
                        availableActions.Add(GetActionsMenuItem(ExaminationAction.Delete, moduleName, examinationModel.examinationPKModel.examNbr.ToString(), examinationModel.examinationPKModel.serviceProviderCode));
                    }
                }
            }

            if (ACAConstant.EXAMINATION_STATUS_COMPLETED_PENDING.Equals(examinationModel.examStatus, StringComparison.InvariantCultureIgnoreCase)
                || ACAConstant.EXAMINATION_STATUS_COMPLETED.Equals(examinationModel.examStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                bool hasSync2RefContact = examinationModel.entityID.HasValue && examinationModel.entityID.Value > 0;
                availableActions.Add(GetActionsMenuItem(ExaminationAction.Edit, moduleName, examinationModel.examinationPKModel.examNbr.ToString(), examinationModel.examinationPKModel.serviceProviderCode));

                if (!readOnly && !hasSync2RefContact && !ACAConstant.COMMON_Y.Equals(examinationModel.requiredFlag, StringComparison.InvariantCultureIgnoreCase))
                {
                    availableActions.Add(GetActionsMenuItem(ExaminationAction.Delete, moduleName, examinationModel.examinationPKModel.examNbr.ToString(), examinationModel.examinationPKModel.serviceProviderCode));
                }
            }

            if (ACAConstant.EXAMINATION_STATUS_COMPLETED_SCHEDULE.Equals(examinationModel.examStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                availableActions.Add(GetActionsMenuItem(ExaminationAction.ViewDetails, moduleName, examinationModel.examinationPKModel.examNbr.ToString(), examinationModel.examinationPKModel.serviceProviderCode));                  
            }

            if (!readOnly
                && (ACAConstant.EXAMINATION_STATUS_READY_TO_SCHEDULE_PAID.Equals(examinationModel.examStatus, StringComparison.InvariantCultureIgnoreCase)
                    || ACAConstant.EXAMINATION_STATUS_READY_TO_SCHEDULE_UNPAID.Equals(examinationModel.examStatus, StringComparison.InvariantCultureIgnoreCase)))
            {
                availableActions.Add(GetActionsMenuItem(ExaminationAction.RetrySchedule, moduleName, examinationModel.examinationPKModel.examNbr.ToString(), examinationModel.examinationPKModel.serviceProviderCode));
                availableActions.Add(GetActionsMenuItem(ExaminationAction.Reset, moduleName, examinationModel.examinationPKModel.examNbr.ToString(), examinationModel.examinationPKModel.serviceProviderCode));
            }

            return availableActions;
        }

        /// <summary>
        /// Get actions menu item
        /// </summary>
        /// <param name="examAction">The exam action</param>
        /// <param name="moduleName">The module name</param>
        /// <param name="examNum">The exam number</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>The action menu item.</returns>
        public static ExaminationActionViewModel GetActionsMenuItem(ExaminationAction examAction, string moduleName, string examNum, string agencyCode)
        {
            ExaminationActionViewModel examActionVeiwModel = new ExaminationActionViewModel();
            examActionVeiwModel.Action = examAction;
            examActionVeiwModel.ActionLabel = GetLabelText(examAction, moduleName);

            string targetURL = string.Empty;
            string queryString = "ExaminationNum=" + examNum + "&" + ACAConstant.MODULE_NAME + "=" + moduleName + "&" + UrlConstant.AgencyCode + "=" + agencyCode;
            switch (examAction)
            {
                case ExaminationAction.ViewDetails:
                    targetURL = EXAMINATION_URL_VIEWDETAILS;
                    break;
                case ExaminationAction.Edit:
                    targetURL = EXAMINATION_URL_EDIT;
                    break;
                case ExaminationAction.Schedule:
                    targetURL = EXAMINATION_URL_SCHEDULE;
                    break;
                case ExaminationAction.Reschedule:
                    targetURL = EXAMINATION_URL_RESCHEDULE_REASON;
                    break;
                case ExaminationAction.Delete:
                    targetURL = EXAMINATION_URL_DELETE;
                    break;
                case ExaminationAction.Cancel:
                    targetURL = EXAMINATION_URL_CANCEL;
                    break;
                case ExaminationAction.RetrySchedule:
                    targetURL = EXAMINATION_URL_SCHEDULE_CONFIRM;
                    break;
                default:
                    targetURL = string.Empty;
                    break;
            }

            targetURL = FileUtil.AppendApplicationRoot(targetURL);
            targetURL = string.IsNullOrEmpty(queryString) ? targetURL : string.Format("{0}?{1}", targetURL, queryString);
            examActionVeiwModel.TargetURL = targetURL;
            return examActionVeiwModel;
        }

        /// <summary>
        /// Gets the actions menu item to take examination.
        /// </summary>
        /// <param name="examAction">The exam action.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="ssoLink">The SSO link.</param>
        /// <returns>The actions menu item to take examination.</returns>
        public static ExaminationActionViewModel GetActionsMenuItemToTakeExamination(ExaminationAction examAction, string moduleName, string ssoLink)
        {
            ExaminationActionViewModel examActionViewModel = new ExaminationActionViewModel();
            examActionViewModel.Action = examAction;
            examActionViewModel.ActionLabel = GetLabelText(examAction, moduleName);
            examActionViewModel.TargetURL = ssoLink;
            return examActionViewModel;
        }
        
        #endregion

        #region For examination list item util

        /// <summary>
        /// Gets the Exam by specific status.
        /// </summary>
        /// <param name="viewModels">The view models.</param>
        /// <param name="examStatus">The exam status.</param>
        /// <returns>
        /// the completed view models.
        /// </returns>
        public static List<ExaminationListItemViewModel> GetExaminationsByStatus(List<ExaminationListItemViewModel> viewModels, params string[] examStatus)
        {
            var results = new List<ExaminationListItemViewModel>();

            if (viewModels != null && viewModels.Count > 0 && examStatus != null && examStatus.Length > 0)
            {
                results = viewModels.Where(e => examStatus.Contains(e.ExaminationViewModel.examStatus, StringComparer.InvariantCultureIgnoreCase)).ToList();
            }

            return results.Count == 0 ? new List<ExaminationListItemViewModel>() : results.OrderBy(p => p.ExaminationViewModel.examDate).ToList();
        }

        /// <summary>
        /// Builds the examination list item view models.
        /// </summary>
        /// <param name="examinationModels">The examination models</param>
        /// <param name="moduleName">The module name</param>
        /// <param name="isCurrentUser">if set to <c>true</c> [is current user].</param>
        /// <param name="readOnly">is readOnly.</param>
        /// <returns>The examination list item view models.</returns>
        public static List<ExaminationListItemViewModel> BuildExaminationView(List<ExaminationModel> examinationModels, string moduleName, bool isCurrentUser, bool readOnly)
        {
            List<ExaminationListItemViewModel> result = new List<ExaminationListItemViewModel>();
            IExaminationBll examinationBll = (IExaminationBll)ObjectFactory.GetObject(typeof(IExaminationBll));
            IProviderBll prividerBll = (IProviderBll)ObjectFactory.GetObject(typeof(IProviderBll));
            IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject(typeof(IAddressBuilderBll)) as IAddressBuilderBll;

            if (examinationModels != null && examinationModels.Count > 0)
            {
                foreach (var examinationModel in examinationModels)
                {
                    var row = new ExaminationListItemViewModel();
                    row.ExaminationViewModel = examinationModel;
                    string pattern = GetExaminationListRowPattern(examinationModel, moduleName);

                    if (ACAConstant.EXAMINATION_STATUS_COMPLETED_PENDING.Equals(examinationModel.examStatus, StringComparison.InvariantCultureIgnoreCase) 
                        || ACAConstant.EXAMINATION_STATUS_COMPLETED_SCHEDULE.Equals(examinationModel.examStatus, StringComparison.InvariantCultureIgnoreCase)
                        || ACAConstant.EXAMINATION_STATUS_COMPLETED.Equals(examinationModel.examStatus, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (examinationModel.finalScore != null)
                        {
                            if (IsPassedExamination(examinationModel.gradingStyle, examinationModel.finalScore, examinationModel.passingScore))
                            {
                                string strPassIcon = string.Format("<img class='ACA_NoBorder' src='{0}' alt='{1}' title='{1}'>", ImageUtil.GetImageURL("icon_exam_score_passing.png"), LabelUtil.GetGlobalTextByKey("aca_common_pass"));
                                pattern = pattern.Replace(ListItemVariables.ExaminationFinalScore, EducationUtil.FormatScore(examinationModel.gradingStyle, Convert.ToString(examinationModel.finalScore, CultureInfo.InvariantCulture)) + strPassIcon);
                            }
                            else
                            {
                                string strFailIcon = string.Format("<img class='ACA_NoBorder' src='{0}' alt='{1}' title='{1}'>", ImageUtil.GetImageURL("icon_exam_score_fail.png"), LabelUtil.GetGlobalTextByKey("aca_common_fail"));
                                pattern = pattern.Replace(ListItemVariables.ExaminationFinalScore, EducationUtil.FormatScore(examinationModel.gradingStyle, Convert.ToString(examinationModel.finalScore, CultureInfo.InvariantCulture)) + strFailIcon);
                            }
                        }
                        else
                        {
                            pattern = pattern.Replace(ListItemVariables.ExaminationFinalScore, EducationUtil.FormatScore(examinationModel.gradingStyle, examinationModel.finalScore.ToString()));
                        }
                    }

                    string undefined = "<span><i>" + LabelUtil.GetTextByKey("aca_examination_examlist_field_undefined", moduleName) + "</i></span>";
                    pattern = pattern.Replace(ListItemVariables.ExaminationName, examinationModel.examName);
                    string examWeekDay = examinationModel.examDate == null ? string.Empty : I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.DayNames[(int)examinationModel.examDate.Value.DayOfWeek];
                    string examDate = examinationModel.examDate == null ? string.Empty : I18nDateTimeUtil.FormatToDateStringForUI(examinationModel.examDate.Value);
                    string startTime = examinationModel.startTime == null ? string.Empty : I18nDateTimeUtil.FormatToTimeStringForUI(examinationModel.startTime.Value, false);
                    string endTime = examinationModel.endTime == null ? string.Empty : " ~ " + I18nDateTimeUtil.FormatToTimeStringForUI(examinationModel.endTime.Value, false);

                    pattern = pattern.Replace(ListItemVariables.ExaminationDateTime, string.IsNullOrEmpty(examDate) ? undefined : examDate + " " + examWeekDay + " " + startTime + endTime);

                    string address = addressBuilderBll.BuildAddress4Provider(examinationModel.examProviderDetailModel);

                    pattern = pattern.Replace(ListItemVariables.ExaminationAddress, string.IsNullOrEmpty(address) ? undefined : address);
                    pattern = pattern.Replace(ListItemVariables.ExaminationRequired, GetRequiredOrOptional(examinationModel.requiredFlag, moduleName));
                    pattern = pattern.Replace(ListItemVariables.ProviderName, string.IsNullOrEmpty(examinationModel.providerName) ? undefined : examinationModel.providerName);

                    pattern = pattern.Replace(ListItemVariables.ExaminationRosterID, string.IsNullOrEmpty(examinationModel.userExamID) ? undefined : examinationModel.userExamID);

                    string ssoLink = string.Empty;

                    if (examinationModel.examProviderDetailModel != null)
                    {
                        pattern = pattern.Replace(ListItemVariables.ExaminationSupportLanguage, string.IsNullOrEmpty(examinationModel.examProviderDetailModel.supportedLanguages) ? undefined : examinationModel.examProviderDetailModel.supportedLanguages);

                        string accessibility = string.Empty;
                        if (!string.IsNullOrEmpty(examinationModel.examProviderDetailModel.isHandicapAccessible))
                        {
                            if (ValidationUtil.IsYes(examinationModel.examProviderDetailModel.isHandicapAccessible))
                            {
                                string strAccessibilityIcon = string.Format("<img class='ACA_NoBorder' src='{0}' alt='{1}' title='{1}'>", ImageUtil.GetImageURL("accessibility-directory.png"), LabelUtil.GetGlobalTextByKey("aca_common_label_accessibility"));
                                accessibility = "<span>" + strAccessibilityIcon + "</span>";
                            }

                            accessibility = accessibility + "<span>" + ModelUIFormat.FormatYNLabel(examinationModel.examProviderDetailModel.isHandicapAccessible) + "</span>";
                        }

                        pattern = pattern.Replace(ListItemVariables.ExaminationAccessibility, string.IsNullOrEmpty(accessibility) ? undefined : accessibility);
                    }

                    if (ACAConstant.EXAMINATION_STATUS_SCHEDULE.Equals(examinationModel.examStatus) &&
                        !string.IsNullOrEmpty(examinationModel.providerName) && !string.IsNullOrEmpty(examinationModel.providerNo))
                    {
                        var providerModel = new ProviderModel4WS()
                        {
                            serviceProviderCode = examinationModel.examinationPKModel.serviceProviderCode,
                            providerName = examinationModel.providerName,
                            providerNo = examinationModel.providerNo
                        };

                        var provider = prividerBll.GetProviderList(providerModel, null);

                        if (provider != null && provider.Length > 0
                            && !string.IsNullOrEmpty(provider[0].externalExamURL))
                        {
                            ssoLink = string.Format("javascript:getSSOLink('{0}','{1}','{2}')", provider[0].serviceProviderCode, provider[0].providerNbr, examinationModel.userExamID);
                        }
                    }

                    row.CombinedInfo = string.Format(pattern, string.Empty);
                    List<ExaminationActionViewModel> availableActions = GetActionsMenu(moduleName, examinationModel, ssoLink, isCurrentUser, readOnly);
                    row.AvailableActions = availableActions == null ? null : availableActions.ToArray();
                    result.Add(row);
                }
            }

            return result;
        }

        /// <summary>
        /// Get status label text value.
        /// </summary>
        /// <param name="examStatus">The examine status</param>
        /// <param name="moduleName">The module name.</param>
        /// <returns>The status label</returns>
        public static string GetExamStatusLabel(string examStatus, string moduleName)
        {
            string statusLabel = string.Empty;

            if (ACAConstant.EXAMINATION_STATUS_COMPLETED_PENDING.Equals(examStatus, StringComparison.InvariantCultureIgnoreCase) 
                || ACAConstant.EXAMINATION_STATUS_COMPLETED_SCHEDULE.Equals(examStatus, StringComparison.InvariantCultureIgnoreCase)
                || ACAConstant.EXAMINATION_STATUS_COMPLETED.Equals(examStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                statusLabel = LabelUtil.GetTextByKey("aca_common_examination_status_completed", moduleName);
            }

            if (ACAConstant.EXAMINATION_STATUS_PENDING.Equals(examStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                statusLabel = LabelUtil.GetTextByKey("aca_common_examination_status_pending", moduleName);
            }

            if (ACAConstant.EXAMINATION_STATUS_SCHEDULE.Equals(examStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                statusLabel = LabelUtil.GetTextByKey("aca_common_examination_status_scheduled", moduleName);
            }

            return statusLabel;
        }

        /// <summary>
        /// Can show schedule link
        /// </summary>
        /// <param name="currentUser">Current User</param>
        /// <param name="recordModel">Record Model</param>
        /// <returns>true or false</returns>
        public static bool CanShowScheduleLink(User currentUser, CapModel4WS recordModel)
        {
            bool result = false;
            bool isCurrentUserAnonymous = currentUser != null ? currentUser.IsAnonymous : false;

            bool isCapLockedOrHold = ConditionsUtil.IsConditionLockedOrHold(recordModel.capID, currentUser.UserSeqNum);

            result = !isCapLockedOrHold && !isCurrentUserAnonymous;
            return result;
        }

        /// <summary>
        /// Get required or optional for the current examination
        /// </summary>
        /// <param name="requiredFlag">Required Flag</param>
        /// <param name="moduleName">Module Name</param>
        /// <returns>The required or optional.</returns>
        public static string GetRequiredOrOptional(string requiredFlag, string moduleName)
        {
            string required = string.Empty;
            if (ACAConstant.COMMON_Y.Equals(requiredFlag, StringComparison.InvariantCultureIgnoreCase))
            {
                required = LabelUtil.GetTextByKey("aca_examination_required", moduleName);
            }
            else
            {
                required = LabelUtil.GetTextByKey("aca_examination_optional", moduleName);
            }

            return required;
        }

        /// <summary>
        /// Get examination result passing or not
        /// </summary>
        /// <param name="gradingStyle">grading style</param>
        /// <param name="finalScore">final score</param>
        /// <param name="passingScore">passing score</param>
        /// <returns>true or false</returns>
        public static bool IsPassedExamination(string gradingStyle, double? finalScore, double? passingScore)
        {
            bool result = false;

            string formatScoreString = string.Empty;

            if (string.IsNullOrEmpty(gradingStyle) || GradingStyle.None.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (GradingStyle.Percentage.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase) || GradingStyle.Score.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase))
            {
                return finalScore >= passingScore;
            }

            if (GradingStyle.Passfail.ToString().Equals(gradingStyle, StringComparison.OrdinalIgnoreCase) && finalScore != null && ACAConstant.COMMON_ONE.Equals(finalScore.ToString()))
            {
                return true;
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Gets the examination list row pattern.
        /// </summary>
        /// <param name="examinationModel">The examination model.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>
        /// The examination list row pattern.
        /// </returns>
        private static string GetExaminationListRowPattern(ExaminationModel examinationModel, string moduleName)
        {
            string result = string.Empty;

            if (examinationModel != null && !string.IsNullOrEmpty(examinationModel.examStatus))
            {
                switch (examinationModel.examStatus.ToUpper())
                {
                    case ACAConstant.EXAMINATION_STATUS_PENDING:
                        {
                            result = LabelUtil.GetTextByKey("aca_examination_pendinglist_combofield_pattern", moduleName);
                            break;
                        }

                    case ACAConstant.EXAMINATION_STATUS_SCHEDULE:
                        {
                            result = LabelUtil.GetTextByKey("aca_examination_scheduledlist_combofield_pattern", moduleName);
                            break;
                        }

                    case ACAConstant.EXAMINATION_STATUS_READY_TO_SCHEDULE_PAID:
                    case ACAConstant.EXAMINATION_STATUS_READY_TO_SCHEDULE_UNPAID:
                        {
                            result = LabelUtil.GetTextByKey("aca_examlist_combofield_pattern_readytoschedule", moduleName);
                            break;
                        }

                    default:
                        {
                            result = LabelUtil.GetTextByKey("aca_examination_completedlist_combofield_pattern", moduleName);
                            break;
                        }
                }
            }

            return result;
        }

        /// <summary>
        /// Get action label key 
        /// </summary>
        /// <param name="action">The examination action</param>
        /// <param name="moduleName">The module name</param>
        /// <returns>The label text</returns>
        private static string GetLabelText(ExaminationAction action, string moduleName)
        {
            string result = string.Empty;

            switch (action)
            {
                case ExaminationAction.ViewDetails:
                    result = LabelUtil.GetTextByKey("aca_examination_label_viewdetails", moduleName);
                    break;
                case ExaminationAction.Edit:
                    result = LabelUtil.GetTextByKey("aca_examination_label_viewdetails", moduleName);
                    break;
                case ExaminationAction.Schedule:
                    result = LabelUtil.GetTextByKey("aca_examination_label_schedule", moduleName);
                    break;
                case ExaminationAction.Reschedule:
                    result = LabelUtil.GetTextByKey("aca_examination_label_reschedule", moduleName);
                    break;
                case ExaminationAction.Delete:
                    result = LabelUtil.GetTextByKey("aca_examination_label_delete", moduleName);
                    break;
                case ExaminationAction.Cancel:
                    result = LabelUtil.GetTextByKey("aca_examination_label_cancel", moduleName);
                    break;
                case ExaminationAction.TakeExamination:
                    result = LabelUtil.GetTextByKey("aca_exam_schedule_takeexamination", moduleName);
                    break;
                case ExaminationAction.RetrySchedule:
                    result = LabelUtil.GetTextByKey("aca_examlist_label_readytoschedule_retryschedule", moduleName);
                    break;
                case ExaminationAction.Reset:
                    result = LabelUtil.GetTextByKey("aca_examlist_label_readytoschedule_reset", moduleName);
                    break;
                case ExaminationAction.None:
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        #region Struct

        /// <summary>
        /// list item variable keys
        /// </summary>
        public struct ListItemVariables
        {
            /// <summary>
            /// Examination date
            /// </summary>
            public const string ExaminationDateTime = "$$ExaminationDateTime$$";

            /// <summary>
            /// Examination name
            /// </summary>
            public const string ExaminationName = "$$ExaminationName$$";

            /// <summary>
            /// Examination RosterID
            /// </summary>
            public const string ExaminationRosterID = "$$RosterID$$";

            /// <summary>
            /// Examination support languages
            /// </summary>
            public const string ExaminationSupportLanguage = "$$SupportLanguages$$";

            /// <summary>
            /// Examination address
            /// </summary>
            public const string ExaminationAddress = "$$ExaminationAddress$$";

            /// <summary>
            /// Examination accessibility
            /// </summary>
            public const string ExaminationAccessibility = "$$ExaminationAccessibility$$";

            /// <summary>
            /// Examination required or not
            /// </summary>
            public const string ExaminationRequired = "$$ExaminationRequired$$";

            /// <summary>
            /// Examination final score
            /// </summary>
            public const string ExaminationFinalScore = "$$ExaminationFinalScore$$";

            /// <summary>
            /// Examination provider name
            /// </summary>
            public const string ProviderName = "$$ProviderName$$";
        }

        #endregion Struct
    }
}
