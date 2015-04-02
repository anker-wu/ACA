#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionPermissionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: InspectionPermissionUtil.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
  * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Inspection
{
    /// <summary>
    /// This class provides the ability to get inspection permission.
    /// </summary>
    internal static class InspectionPermissionUtil
    {
        #region Fields

        /// <summary>
        /// Restriction option days prior
        /// </summary>
        private const string RESTRICTION_OPTION_DAYSPRIOR = "1";

        /// <summary>
        /// Restriction option hours prior
        /// </summary>
        private const string RESTRICTION_OPTION_HOURSPRIOR = "2";

        /// <summary>
        /// Restriction option days prior at specific time
        /// </summary>
        private const string RESTRICTION_OPTION_DAYSPRIORATTIME = "3";

        #endregion Fields

        #region Methods

        /// <summary>
        /// Determines whether current action can be operated.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>
        /// <c>true</c> if current action can be operated; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanBeOperated(InspectionAction action)
        {
            bool result = false;

            switch (action)
            {
                case InspectionAction.Request:
                case InspectionAction.Schedule:
                case InspectionAction.Reschedule:
                case InspectionAction.Cancel:
                    result = true;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Determines whether current action can be grayed out.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>
        /// <c>true</c> if current action can be grayed out; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanBeGrayedOut(InspectionAction action)
        {
            bool result = false;

            switch (action)
            {
                case InspectionAction.RestrictedReschedule:
                case InspectionAction.RestrictedCancel:
                    result = true;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Determines whether current action can be updated.
        /// </summary>
        /// <param name="actualStatus">The actual status.</param>
        /// <param name="action">The action.</param>
        /// <returns>
        /// <c>true</c> if current action can be updated; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanBeUpdated(InspectionStatus actualStatus, InspectionAction action)
        {
            bool result = false;

            switch (actualStatus)
            {
                case InspectionStatus.PendingByACA:
                case InspectionStatus.PendingByV360:
                case InspectionStatus.Scheduled:
                case InspectionStatus.ResultPending:
                    result = CanBeOperated(action);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified inspection model can be cancelled.
        /// </summary>
        /// <param name="inspectionTypeModel">The inspection model.</param>
        /// <returns>
        /// <c>true</c> if the specified inspection model can be cancelled; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanBeCancelled(InspectionTypeModel inspectionTypeModel)
        {
            string switch4CancelAction = string.Empty;

            if (inspectionTypeModel != null &&
                inspectionTypeModel.calendarInspectionType != null)
            {
                switch4CancelAction = inspectionTypeModel.calendarInspectionType.cancelInspectionInACA;
            }

            return string.IsNullOrEmpty(switch4CancelAction) || ACAConstant.COMMON_Y.Equals(switch4CancelAction, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified inspection model can be rescheduled.
        /// </summary>
        /// <param name="inspectionTypeModel">The inspection model.</param>
        /// <returns>
        /// <c>true</c> if the specified inspection model can be rescheduled; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanBeRescheduled(InspectionTypeModel inspectionTypeModel)
        {
            string switch4RescheduleAction = string.Empty;

            if (inspectionTypeModel != null &&
                inspectionTypeModel.calendarInspectionType != null)
            {
                switch4RescheduleAction = inspectionTypeModel.calendarInspectionType.rescheduleInspectionInACA;
            }

            return string.IsNullOrEmpty(switch4RescheduleAction) || ACAConstant.COMMON_Y.Equals(switch4RescheduleAction, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Check whether allows display of optional inspections.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>
        /// <c>true</c> if allow display of optional inspections; otherwise, <c>false</c>.
        /// </returns>
        public static bool AllowDisplayOfOptionalInspections(string agencyCode, string moduleName)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(moduleName))
            {
                IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
                string displayOption = bizBll.GetValueForACAConfig(agencyCode, moduleName + "_" + BizDomainConstant.STD_INSPECTION_DISPLAYOPTION, ACAConstant.COMMON_Y);

                return ValidationUtil.IsYes(displayOption);
            }

            return result;
        }

        /// <summary>
        /// Check whether allows multiple inspections.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="allowMultipleFlagInAAClassic">The allow multiple flag in AA classic.</param>
        /// <returns>
        /// <c>true</c> if allow multiple inspections; otherwise, <c>false</c>.
        /// </returns>
        public static bool AllowMultipleInspections(string moduleName, string allowMultipleFlagInAAClassic)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(moduleName) && !string.IsNullOrEmpty(allowMultipleFlagInAAClassic))
            {
                IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
                string isMultipleInspectionsEnabled = policyBll.GetValueByKey(BizDomainConstant.STD_MULTIPLE_INSPECTIONS_ENABLED, moduleName);
                bool allowMultipleInspectionsInACAAdmin = ValidationUtil.IsYes(isMultipleInspectionsEnabled);

                bool isMultipleInspectionsEnabledInAAClassic = ACAConstant.COMMON_Y.Equals(allowMultipleFlagInAAClassic, StringComparison.InvariantCultureIgnoreCase);
                result = allowMultipleInspectionsInACAAdmin && isMultipleInspectionsEnabledInAAClassic;
            }

            return result;
        }

        /// <summary>
        /// Check whether allows the schedule action.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="isCurrentUserAnonymous">if set to <c>true</c> [is current user anonymous].</param>
        /// <param name="recordModel">The record model.</param>
        /// <returns>
        /// true:allow schedule, false:not allow schedule
        /// </returns>
        public static bool AllowSchedule(string agencyCode, string moduleName, bool isCurrentUserAnonymous, CapModel4WS recordModel)
        {
            bool result = true;

            var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
            XpolicyUserRolePrivilegeModel policy = GetUserRolePolicyForSchedule(agencyCode, moduleName);

            if (policy != null)
            {
                var selectedLPList = GetSelectedLPList4ScheduleInspectionPermission(agencyCode, moduleName);
                policy.userRolePrivilegeModel.licenseTypeRuleArray = selectedLPList;

                //If only set contact permission,we must be consider the permission set in capcontact model in spear form.
                result = proxyUserRoleBll.HasSchedulePermission(recordModel, policy.userRolePrivilegeModel);

                if (isCurrentUserAnonymous)
                {
                    result = result & AllowAnonymousToSchedule(agencyCode);
                }
            }

            return result;
        }

        /// <summary>
        /// Check the contact right (input or view) according to the provided policy name
        /// </summary>
        /// <param name="capModel">the cap model</param>
        /// <param name="agencyCode">the agency code</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="policyName">the policy name</param>
        /// <returns>Return true if have right, else false.</returns>
        public static bool CheckContactRight(CapModel4WS capModel, string agencyCode, string moduleName, string policyName)
        {
            bool result = true;

            var userRolePrivilege = GetUserRolePrivilegeModel4ContactPermission(agencyCode, moduleName, policyName);

            if (userRolePrivilege != null)
            {
                var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
                result = proxyUserRoleBll.HasPermission(capModel, userRolePrivilege, ProxyPermissionType.MANAGE_INSPECTIONS);
            }

            return result;
        }

        /// <summary>
        /// Determines whether specific inspection type is actionable by app status.
        /// </summary>
        /// <param name="recordStatusGroupCode">The record status group code.</param>
        /// <param name="recordStatus">The record status.</param>
        /// <param name="inspectionGroupCode">The inspection group code.</param>
        /// <param name="inspectionType">Type of the inspection.</param>
        /// <param name="action">The action.</param>
        /// <returns>
        /// <c>true</c> if is actionable by app status; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsActionableByAppStatus(string recordStatusGroupCode, string recordStatus, string inspectionGroupCode, string inspectionType, InspectionAction action)
        {
            bool result = true;
            var inspectionTypePermissionBll = ObjectFactory.GetObject<IInspectionTypePermissionBll>();

            IList<InspectionActionPermissionModel> actionPermissionModelList = inspectionTypePermissionBll.GetInspectionActionPermissions(recordStatusGroupCode, recordStatus, inspectionGroupCode);

            if (actionPermissionModelList != null)
            {
                //note: as permission settings have combined request and schedule action into schedule action, so need to use schedule action to get permission for request action.
                action = action == InspectionAction.Request ? InspectionAction.Schedule : action;

                var matchedItem = (from p in actionPermissionModelList
                                   let tempAction = EnumUtil<InspectionAction>.Parse(p.actionCode, InspectionAction.None)
                                   where p.inspectionTypeModel != null
                                       && !string.IsNullOrEmpty(p.inspectionTypeModel.type)
                                       && p.inspectionTypeModel.type.Equals(inspectionType, StringComparison.OrdinalIgnoreCase)
                                       && tempAction == action
                                   select p).FirstOrDefault();
                result = matchedItem == null || matchedItem.enabled ? true : false;
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified inspection type model is out of time to reschedule.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="inspectionTypeModel">The inspection type model.</param>
        /// <param name="inspectionModel">The inspection model.</param>
        /// <returns>
        /// <c>true</c> if the specified inspection type model is out of time to reschedule; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOutOfTimeToReschedule(string agencyCode, InspectionTypeModel inspectionTypeModel, InspectionModel inspectionModel)
        {
            bool result = false;
            var timeOption = InspectionTimeOption.Unknow;
            var activityDateTime = InspectionUtil.GetActivityDate(inspectionModel, out timeOption);

            if (inspectionTypeModel != null && inspectionModel != null && !string.IsNullOrEmpty(inspectionTypeModel.reScheduleOptionInACA) && activityDateTime != null)
            {
                int daysPrior = inspectionTypeModel.reScheduleDaysInACA == null ? 0 : inspectionTypeModel.reScheduleDaysInACA.Value;
                int hoursPrior = inspectionTypeModel.reScheduleHoursInACA == null ? 0 : inspectionTypeModel.reScheduleHoursInACA.Value;
                var timeBll = ObjectFactory.GetObject<ITimeZoneBll>();
                DateTime currentAgencyDateTime = timeBll.GetAgencyCurrentDate(agencyCode);
                result = IsOutOfTimeToOperate(currentAgencyDateTime, activityDateTime.Value, inspectionTypeModel.reScheduleOptionInACA, daysPrior, hoursPrior, inspectionTypeModel.reScheduleTimeInACA);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified inspection type model is out of time to cancel.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="inspectionTypeModel">The inspection type model.</param>
        /// <param name="inspectionModel">The inspection model.</param>
        /// <returns>
        /// <c>true</c> if the specified inspection type model is out of time to cancel; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOutOfTimeToCancel(string agencyCode, InspectionTypeModel inspectionTypeModel, InspectionModel inspectionModel)
        {
            bool result = false;
            var timeOption = InspectionTimeOption.Unknow;
            var activityDateTime = InspectionUtil.GetActivityDate(inspectionModel, out timeOption);

            if (inspectionTypeModel != null && inspectionModel != null && !string.IsNullOrEmpty(inspectionTypeModel.cancelOptionInACA) && activityDateTime != null)
            {
                int daysPrior = inspectionTypeModel.cancelDaysInACA == null ? 0 : inspectionTypeModel.cancelDaysInACA.Value;
                int hoursPrior = inspectionTypeModel.cancelHoursInACA == null ? 0 : inspectionTypeModel.cancelHoursInACA.Value;
                var timeBll = ObjectFactory.GetObject<ITimeZoneBll>();
                DateTime currentAgencyDateTime = timeBll.GetAgencyCurrentDate(agencyCode);
                result = IsOutOfTimeToOperate(currentAgencyDateTime, activityDateTime.Value, inspectionTypeModel.cancelOptionInACA, daysPrior, hoursPrior, inspectionTypeModel.cancelTimeInACA);
            }

            return result;
        }

        /// <summary>
        /// Determines whether [is ready time enabled] [the specified inspection type model].
        /// </summary>
        /// <param name="inspectionTypeModel">The inspection type model.</param>
        /// <returns>
        /// <c>true</c> if [is ready time enabled] [the specified inspection type model]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsReadyTimeEnabled(InspectionTypeModel inspectionTypeModel)
        {
            var inspectionBll = ObjectFactory.GetObject<IInspectionBll>();
            bool isReadyTimeEnabled = false;

            if (inspectionTypeModel != null &&
                inspectionTypeModel.calendarInspectionType != null)
            {
                InspectionScheduleType scheduleType = InspectionScheduleTypeUtil.GetScheduleType(inspectionTypeModel);
                bool isRequestOnlyPending = scheduleType == InspectionScheduleType.RequestOnlyPending;
                isReadyTimeEnabled = ACAConstant.COMMON_Y.Equals(inspectionTypeModel.calendarInspectionType.enableReadyTimeInACA, StringComparison.InvariantCultureIgnoreCase);
                isReadyTimeEnabled = isReadyTimeEnabled & isRequestOnlyPending;
            }

            return isReadyTimeEnabled;
        }

        /// <summary>
        /// Determines whether the ready time is available or not
        /// </summary>
        /// <param name="isReadyTimeEnabled">is ReadyTime Enabled</param>
        /// <param name="actualStatus">The actual status.</param>
        /// <returns>
        /// <c>true</c> if the isReadyTimeEnabled is true and inspection status is PendingByACA or PendingByV360; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsReadyTimeAvailable(bool isReadyTimeEnabled, InspectionStatus actualStatus)
        {
            bool result = false;

            result = isReadyTimeEnabled && (actualStatus == InspectionStatus.PendingByACA || actualStatus == InspectionStatus.PendingByV360);

            return result;
        }

        /// <summary>
        /// Gets the user role policy for schedule.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>the user role policy for schedule</returns>
        private static XpolicyUserRolePrivilegeModel GetUserRolePolicyForSchedule(string agencyCode, string moduleName)
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            XpolicyUserRolePrivilegeModel policy = policyBll.GetPolicy(agencyCode, ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_SCHEDULE, ACAConstant.LEVEL_TYPE_MODULE, moduleName);

            return policy;
        }

        /// <summary>
        /// Check whether allow anonymous user to schedule the inspection.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>
        /// true - allow anonymous user to schedule, false - don't allow anonymous user to schedule.
        /// </returns>
        private static bool AllowAnonymousToSchedule(string agencyCode)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(agencyCode))
            {
                IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
                string forbidAnonymousUser = bizBll.GetValueForACAConfig(agencyCode, ACAConstant.NO_ANONYMOUS_USER);

                result = !ValidationUtil.IsYes(forbidAnonymousUser);
            }

            return result;
        }

        /// <summary>
        /// Determines whether current agency is out of time to do based on the specific parameters.
        /// </summary>
        /// <param name="currentAgencyDateTime">The current agency date time.</param>
        /// <param name="schdeuleDateTime">The schedule date time.</param>
        /// <param name="theOption">The option.</param>
        /// <param name="daysPrior">The days prior.</param>
        /// <param name="hoursPrior">The hours prior.</param>
        /// <param name="specificTimeString">The specific time string.</param>
        /// <returns>
        /// <c>true</c> current agency is out of time based on the specific parameters; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsOutOfTimeToOperate(DateTime currentAgencyDateTime, DateTime schdeuleDateTime, string theOption, int daysPrior, int hoursPrior, string specificTimeString)
        {
            bool result = false;

            if (RESTRICTION_OPTION_DAYSPRIOR.Equals(theOption, StringComparison.InvariantCultureIgnoreCase))
            {
                result = (daysPrior > 0) && currentAgencyDateTime.Date >= schdeuleDateTime.Date.AddDays((-1 * daysPrior) + 1);
            }
            else if (RESTRICTION_OPTION_HOURSPRIOR.Equals(theOption, StringComparison.InvariantCultureIgnoreCase))
            {
                result = currentAgencyDateTime >= schdeuleDateTime.AddHours(-1 * hoursPrior);
            }
            else if (RESTRICTION_OPTION_DAYSPRIORATTIME.Equals(theOption, StringComparison.InvariantCultureIgnoreCase))
            {
                DateTime lastOpportunityDate = schdeuleDateTime.Date.AddDays(-1 * daysPrior);
                string lastOpportunityDateTimeString = string.Concat(I18nDateTimeUtil.FormatToDateStringForWebService(lastOpportunityDate), " ", specificTimeString);
                DateTime lastOpportunityDateTime = I18nDateTimeUtil.ParseFromWebService(lastOpportunityDateTimeString);
                result = currentAgencyDateTime >= lastOpportunityDateTime;
            }

            return result;
        }

        /// <summary>
        /// Gets the user role privilege model4 contact permission.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns>
        /// the user role privilege model4 contact permission.
        /// </returns>
        private static UserRolePrivilegeModel GetUserRolePrivilegeModel4ContactPermission(string agencyCode, string moduleName, string policyName)
        {
            UserRolePrivilegeModel result = null;
            const string DEFAULT_USER_ROLE_STRING_4_VIEW = "010000";
            const string DEFAULT_USER_ROLE_STRING_4_INPUT = "100000";
            var isForInputPermission = ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_INPUT_CONTACT.Equals(policyName);

            var xPolicyBll = ObjectFactory.GetObject<IXPolicyBll>();
            var xpolicys = xPolicyBll.GetPolicyListByCategory(policyName, ACAConstant.LEVEL_TYPE_MODULE, moduleName);

            string userRoleString = string.Empty;

            if (xpolicys != null && xpolicys.Length > 0)
            {
                userRoleString = (from x in xpolicys
                                  where x != null
                                  && !string.IsNullOrEmpty(x.serviceProviderCode)
                                  && x.serviceProviderCode.Equals(agencyCode, StringComparison.OrdinalIgnoreCase)
                                  select x.data2).FirstOrDefault();
            }

            //if no configured settings, use default value.
            if (string.IsNullOrEmpty(userRoleString) || userRoleString.Trim() == string.Empty)
            {
                userRoleString = isForInputPermission ? DEFAULT_USER_ROLE_STRING_4_INPUT : DEFAULT_USER_ROLE_STRING_4_VIEW;
            }

            if (!string.IsNullOrEmpty(userRoleString))
            {
                var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();
                result = userRoleBll.ConvertToUserRolePrivilegeModel(userRoleString);
            }

            if (result != null)
            {
                const string CONTACT_INPUT_PERMISSION_NAME = "INPUT_RIGHT";
                const string CONTACT_VIEW_PERMISSION_NAME = "VIEW_RIGHT";
                string permissionName = isForInputPermission ? CONTACT_INPUT_PERMISSION_NAME : CONTACT_VIEW_PERMISSION_NAME;
                var selectedLPList = GetSelectedLPList4ContactPermission(agencyCode, moduleName, permissionName);
                result.licenseTypeRuleArray = selectedLPList;
            }

            return result;
        }

        /// <summary>
        /// Gets the selected LP list4 contact permission.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="permissionName">Name of the permission.</param>
        /// <returns>
        /// the selected LP list4 contact action permission.
        /// </returns>
        private static string[] GetSelectedLPList4ContactPermission(string agencyCode, string moduleName, string permissionName)
        {
            string[] result = null;

            var xPolicyBll = ObjectFactory.GetObject<IXPolicyBll>();
            var xpolicys = xPolicyBll.GetPolicyListByCategory(BizDomainConstant.STD_ITEM_INSPECTION_CONTACT_RIGHT, ACAConstant.LEVEL_TYPE_MODULE, moduleName);

            if (xpolicys != null && xpolicys.Length > 0 && !string.IsNullOrEmpty(permissionName))
            {
                string lpListString = (from x in xpolicys
                                where x != null
                                && EntityType.LICENSETYPE.ToString().Equals(x.data3, StringComparison.OrdinalIgnoreCase)
                                && permissionName.Equals(x.data4, StringComparison.OrdinalIgnoreCase)
                                select x.data2).FirstOrDefault();

                if (!string.IsNullOrEmpty(lpListString))
                {
                    result = lpListString.Split(new string[] { ACAConstant.SPLIT_DOUBLE_VERTICAL }, StringSplitOptions.None);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the selected LP list 4 schedule inspection permission.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>
        /// the selected LP list 4  schedule inspection permission.
        /// </returns>
        private static string[] GetSelectedLPList4ScheduleInspectionPermission(string agencyCode, string moduleName)
        {
            string[] result = null;

            var xPolicyBll = ObjectFactory.GetObject<IXPolicyBll>();
            var xpolicys = xPolicyBll.GetPolicyListByCategory(XPolicyConstant.INSPECTION_PERMISSION_USER_ROLES, ACAConstant.LEVEL_TYPE_MODULE, moduleName);

            if (xpolicys != null && xpolicys.Length > 0)
            {
                string lpListString = (from x in xpolicys
                                       where x != null
                                       && EntityType.LICENSETYPE.ToString().Equals(x.data3, StringComparison.OrdinalIgnoreCase)
                                       select x.data2).FirstOrDefault();

                if (!string.IsNullOrEmpty(lpListString))
                {
                    result = lpListString.Split(new string[] { ACAConstant.SPLIT_DOUBLE_VERTICAL }, StringSplitOptions.None);
                }
            }

            return result;
        }

        #endregion
    }
}
