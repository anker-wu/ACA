#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionActionViewController.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionActionViewUtil.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web;

using Accela.ACA.Common;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Inspection
{
    /// <summary>
    /// Inspection Action view utility
    /// </summary>
    public static class InspectionActionViewUtil
    {
        #region Fields

        /// <summary>
        /// relative url of cancel page
        /// </summary>
        private const string RELATIVE_URL_CANCEL = "Inspection/InspectionCancel.aspx";

        /// <summary>
        /// relative url of wizard confirmation page
        /// </summary>
        private const string RELATIVE_URL_INPUT_COMMENTS = "Inspection/InspectionWizardConfirm.aspx";

        /// <summary>
        /// relative url of wizard input date time page
        /// </summary>
        private const string RELATIVE_URL_INPUT_DATETIME = "Inspection/InspectionWizardInputDateTime.aspx";

        /// <summary>
        /// relative url of wizard input location page
        /// </summary>
        private const string RELATIVE_URL_INPUT_LOCATION = "Inspection/InspectionWizardInputLocation.aspx";

        /// <summary>
        /// relative url of wizard input type page
        /// </summary>
        private const string RELATIVE_URL_INPUT_TYPE = "Inspection/InspectionWizardInputType.aspx";

        /// <summary>
        /// relative url of print page
        /// </summary>
        private const string RELATIVE_URL_PRINT = "Inspection/InspectionPrint.aspx";

        /// <summary>
        /// relative url of view details page
        /// </summary>
        private const string RELATIVE_URL_VIEWDETAILS = "Inspection/InspectionDetails.aspx";

        #endregion Fields

        /// <summary>
        /// Builds the inspection action view models.
        /// </summary>
        /// <param name="serviceProviderCode">The service provider code.</param>
        /// <param name="recordIDModel">The record ID model.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="publicUserID">The public user ID.</param>
        /// <param name="inspectionViewModel">The inspection view model.</param>
        /// <returns>the inspection action view models</returns>
        public static List<InspectionActionViewModel> BuildViewModels(string serviceProviderCode, CapIDModel recordIDModel, string moduleName, string publicUserID, InspectionViewModel inspectionViewModel)
        {
            List<InspectionActionViewModel> availableActions = new List<InspectionActionViewModel>();

            if (inspectionViewModel.InspectionDataModel != null && inspectionViewModel.InspectionDataModel.AvailableOperations != null)
            {
                foreach (InspectionAction action in inspectionViewModel.InspectionDataModel.AvailableOperations)
                {
                    InspectionActionViewModel inspectionActionViewModel = BuildViewModel(serviceProviderCode, recordIDModel, moduleName, publicUserID, inspectionViewModel, action);
                    availableActions.Add(inspectionActionViewModel);
                }
            }

            availableActions.Sort((x, y) => x.Action.CompareTo(y.Action));

            return availableActions;
        }

        /// <summary>
        /// Builds the view model.
        /// </summary>
        /// <param name="serviceProviderCode">The service provider code.</param>
        /// <param name="recordIDModel">The record ID model.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="publicUserID">The public user ID.</param>
        /// <param name="inspectionViewModel">The inspection view model.</param>
        /// <param name="currentAction">The current action.</param>
        /// <returns>the view model.</returns>
        public static InspectionActionViewModel BuildViewModel(string serviceProviderCode, CapIDModel recordIDModel, string moduleName, string publicUserID, InspectionViewModel inspectionViewModel, InspectionAction currentAction)
        {
            InspectionActionViewModel result = new InspectionActionViewModel();
            result.Action = currentAction;
            result.Visible = IsActionVisible(currentAction);
            result.Enabled = IsActionEnabled(currentAction);
            result.ActionLabel = LabelUtil.GetTextByKey(GetLabelKey(currentAction), moduleName);
            result.TargetURL = GetTargetURL(serviceProviderCode, recordIDModel, moduleName, publicUserID, inspectionViewModel, currentAction);
            return result;
        }

        /// <summary>
        /// Gets the name of the current page.
        /// </summary>
        /// <param name="currentRequstPath">The current request path.</param>
        /// <returns>the name of the current page.</returns>
        public static InspectionWizardPageName GetCurrentPageName(string currentRequstPath)
        {
            var result = InspectionWizardPageName.Unknown;

            if (currentRequstPath.EndsWith(RELATIVE_URL_CANCEL, StringComparison.OrdinalIgnoreCase))
            {
                result = InspectionWizardPageName.Cancel;
            }
            else if (currentRequstPath.EndsWith(RELATIVE_URL_INPUT_COMMENTS, StringComparison.OrdinalIgnoreCase))
            {
                result = InspectionWizardPageName.InputComments;
            }
            else if (currentRequstPath.EndsWith(RELATIVE_URL_INPUT_DATETIME, StringComparison.OrdinalIgnoreCase))
            {
                result = InspectionWizardPageName.InputDateTime;
            }
            else if (currentRequstPath.EndsWith(RELATIVE_URL_INPUT_LOCATION, StringComparison.OrdinalIgnoreCase))
            {
                result = InspectionWizardPageName.InputLocation;
            }
            else if (currentRequstPath.EndsWith(RELATIVE_URL_INPUT_TYPE, StringComparison.OrdinalIgnoreCase))
            {
                result = InspectionWizardPageName.InputType;
            }
            else if (currentRequstPath.EndsWith(RELATIVE_URL_PRINT, StringComparison.OrdinalIgnoreCase))
            {
                result = InspectionWizardPageName.Print;
            }
            else if (currentRequstPath.EndsWith(RELATIVE_URL_VIEWDETAILS, StringComparison.OrdinalIgnoreCase))
            {
                result = InspectionWizardPageName.ViewDetails;
            }

            return result;
        }

        /// <summary>
        /// Gets the wizard previous URL.
        /// </summary>
        /// <param name="urlParameters">The URL parameters.</param>
        /// <returns>
        /// the wizard previous URL.
        /// </returns>
        public static string GetWizardPreviousURL(InspectionParameter urlParameters)
        {
            string result = string.Empty;
            var beginPage = urlParameters.NavigationBeginPage;
            var currentPage = GetCurrentPageName(HttpContext.Current.Request.Path);
            var previousPage = InspectionWizardPageName.Unknown;

            if (currentPage == InspectionWizardPageName.InputDateTime)
            {
                previousPage = InspectionWizardPageName.InputType;
            }
            else if (currentPage == InspectionWizardPageName.InputLocation)
            {
                var isReadyTimeEnabled = urlParameters.ReadyTimeEnabled != null && urlParameters.ReadyTimeEnabled.Value == true;

                if (urlParameters.ScheduleType == InspectionScheduleType.RequestOnlyPending && !isReadyTimeEnabled)
                {
                    previousPage = InspectionWizardPageName.InputType;
                }
                else
                {
                    previousPage = InspectionWizardPageName.InputDateTime;
                }
            }
            else if (currentPage == InspectionWizardPageName.InputComments)
            {
                previousPage = InspectionWizardPageName.InputLocation;
            }

            if (previousPage < beginPage || previousPage >= currentPage)
            {
                previousPage = InspectionWizardPageName.Unknown;
            }

            if (previousPage != InspectionWizardPageName.Unknown)
            {
                string relativeURL = GetRelativeURL(previousPage);
                string absolateURL = FileUtil.AppendApplicationRoot(relativeURL);
                result = string.Concat(absolateURL, "?", HttpContext.Current.Request.QueryString);
            }

            return result;
        }

        /// <summary>
        /// Gets the target URL.
        /// </summary>
        /// <param name="serviceProviderCode">The service provider code.</param>
        /// <param name="recordIDModel">The record ID model.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="publicUserID">The public user ID.</param>
        /// <param name="inspectionViewModel">The inspection view model.</param>
        /// <param name="currentAction">The current action.</param>
        /// <returns>the target URL.</returns>
        private static string GetTargetURL(string serviceProviderCode, CapIDModel recordIDModel, string moduleName, string publicUserID, InspectionViewModel inspectionViewModel, InspectionAction currentAction)
        {
            var beginPage = GetBeginPageByAction(inspectionViewModel, currentAction);
            string relativeURL = GetRelativeURL(beginPage);
            string absoluteURL = FileUtil.AppendApplicationRoot(relativeURL);
            string queryString = GetTargetURLQueryString(serviceProviderCode, recordIDModel, moduleName, publicUserID, inspectionViewModel, currentAction, beginPage);
            string result = string.Format("{0}?{1}", absoluteURL, queryString);
            return result;
        }

        /// <summary>
        /// Gets the target URL by action.
        /// </summary>
        /// <param name="inspectionViewModel">The inspection view model.</param>
        /// <param name="action">The action.</param>
        /// <returns>
        /// the target URL by action.
        /// </returns>
        private static InspectionWizardPageName GetBeginPageByAction(InspectionViewModel inspectionViewModel, InspectionAction action)
        {
            InspectionWizardPageName result = InspectionWizardPageName.Unknown;

            switch (action)
            {
                case InspectionAction.Cancel:
                    result = InspectionWizardPageName.Cancel;
                    break;
                case InspectionAction.Request:
                    var dataModel = inspectionViewModel.InspectionDataModel;
                    var isRequestOnlyPending = dataModel.ScheduleType == InspectionScheduleType.RequestOnlyPending;

                    if (isRequestOnlyPending && !dataModel.ReadyTimeAvailable)
                    {
                        result = InspectionWizardPageName.InputLocation;
                    }
                    else
                    {
                        result = InspectionWizardPageName.InputDateTime;
                    }

                    break;
                case InspectionAction.Schedule:
                case InspectionAction.Reschedule:
                    result = InspectionWizardPageName.InputDateTime;
                    break;
                case InspectionAction.ViewDetails:
                    result = InspectionWizardPageName.ViewDetails;
                    break;
                case InspectionAction.PrintDetails:
                    result = InspectionWizardPageName.Print;
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets the relative URL.
        /// </summary>
        /// <param name="pageName">Name of the page.</param>
        /// <returns>the relative URL.</returns>
        private static string GetRelativeURL(InspectionWizardPageName pageName)
        {
            string result = string.Empty;

            switch (pageName)
            {
                case InspectionWizardPageName.Cancel:
                    result = RELATIVE_URL_CANCEL;
                    break;
                case InspectionWizardPageName.InputComments:
                    result = RELATIVE_URL_INPUT_COMMENTS;
                    break;
                case InspectionWizardPageName.InputDateTime:
                    result = RELATIVE_URL_INPUT_DATETIME;
                    break;
                case InspectionWizardPageName.InputLocation:
                    result = RELATIVE_URL_INPUT_LOCATION;
                    break;
                case InspectionWizardPageName.InputType:
                    result = RELATIVE_URL_INPUT_TYPE;
                    break;
                case InspectionWizardPageName.Print:
                    result = RELATIVE_URL_PRINT;
                    break;
                case InspectionWizardPageName.ViewDetails:
                    result = RELATIVE_URL_VIEWDETAILS;
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets the target URL query string.
        /// </summary>
        /// <param name="serviceProviderCode">The service provider code.</param>
        /// <param name="recordIDModel">The record ID model.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="publicUserID">The public user ID.</param>
        /// <param name="inspectionViewModel">The inspection view model.</param>
        /// <param name="action">The action.</param>
        /// <param name="navigationBeginPage">The navigation begin page.</param>
        /// <returns>the target URL query string.</returns>
        private static string GetTargetURLQueryString(string serviceProviderCode, CapIDModel recordIDModel, string moduleName, string publicUserID, InspectionViewModel inspectionViewModel, InspectionAction action, InspectionWizardPageName navigationBeginPage)
        {
            string result = string.Empty;

            InspectionParameter parameter = new InspectionParameter();
            parameter.ID = inspectionViewModel.ID.ToString();
            parameter.Action = action;
            parameter.AgencyCode = serviceProviderCode;
            parameter.RecordAltID = recordIDModel != null ? recordIDModel.customID : string.Empty;
            parameter.RecordID1 = recordIDModel != null ? recordIDModel.ID1 : string.Empty;
            parameter.RecordID2 = recordIDModel != null ? recordIDModel.ID2 : string.Empty;
            parameter.RecordID3 = recordIDModel != null ? recordIDModel.ID3 : string.Empty;
            parameter.ModuleName = moduleName;
            parameter.PublicUserID = publicUserID;
            parameter.IsPopupPage = ACAConstant.COMMON_Y;
            parameter.NavigationBeginPage = navigationBeginPage;

            result = InspectionParameterUtil.BuildQueryString(parameter);

            return result;
        }

        /// <summary>
        /// Gets the grayed-out action tip label key.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>grayed-out action tip</returns>
        private static string GetGrayedOutTipLabelKey(InspectionAction action)
        {
            string result = string.Empty;

            switch (action)
            {
                case InspectionAction.RestrictedReschedule:
                    result = "ins_inspectionlist_label_grayedoutreschedule";
                    break;
                case InspectionAction.RestrictedCancel:
                    result = "ins_inspectionlist_label_grayedoutcancel";
                    break;
            }

            return result;
        }

        /// <summary>
        /// get action label key
        /// </summary>
        /// <param name="action">the Inspection action</param>
        /// <returns>action label key</returns>
        private static string GetLabelKey(InspectionAction action)
        {
            string result = string.Empty;

            switch (action)
            {
                case InspectionAction.DoComplete:
                    result = "ins_inspectionList_label_Completed";
                    break;
                case InspectionAction.DoPrerequisiteNotMet:
                    result = "ins_inspectionList_label_PrerequisiteNotMet";
                    break;
                case InspectionAction.Request:
                    result = "ins_inspectionlist_label_requestinspection";
                    break;
                case InspectionAction.Schedule:
                    result = "ins_inspectionList_label_scheduleInspection";
                    break;
                case InspectionAction.Reschedule:
                case InspectionAction.RestrictedReschedule:
                    result = "ins_inspectionList_label_reSchedule";
                    break;
                case InspectionAction.Cancel:
                case InspectionAction.RestrictedCancel:
                    result = "ins_inspectionList_label_cancelSchedule";
                    break;
                case InspectionAction.ViewDetails:
                    result = "aca_inspection_link_viewdetails";
                    break;
                case InspectionAction.None:
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Determines whether action is visible
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>
        /// <c>true</c> if action is visible; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsActionVisible(InspectionAction action)
        {
            bool result = false;

            switch (action)
            {
                case InspectionAction.Cancel:
                case InspectionAction.PrintDetails:
                case InspectionAction.Request:
                case InspectionAction.Reschedule:
                case InspectionAction.Schedule:
                case InspectionAction.ViewDetails:
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Determines whether action is enabled.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>
        /// <c>true</c> if action is enabled; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsActionEnabled(InspectionAction action)
        {
            bool result = false;

            switch (action)
            {
                case InspectionAction.Cancel:
                case InspectionAction.PrintDetails:
                case InspectionAction.Request:
                case InspectionAction.Reschedule:
                case InspectionAction.Schedule:
                case InspectionAction.ViewDetails:
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }
    }
}
