#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionListItemViewController.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionListItemViewUtil.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;

using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Inspection
{
    /// <summary>
    /// Inspection List Item view utility
    /// </summary>
    public static class InspectionListItemViewUtil
    {
        /// <summary>
        /// Gets the upcoming view models.
        /// </summary>
        /// <param name="viewModels">The view models.</param>
        /// <returns>the upcoming view models.</returns>
        public static List<InspectionListItemViewModel> GetUpcomingViewModels(List<InspectionListItemViewModel> viewModels)
        {
            var results = new List<InspectionListItemViewModel>();

            if (viewModels != null)
            {
                foreach (var viewModel in viewModels)
                {
                    if (viewModel.IsUpcomingInspection)
                    {
                        results.Add(viewModel);
                    }
                }

                //as the schedule dates are in desc order, so only do reverse to get list with asc order.
                results.Reverse();
            }

            return results;
        }

        /// <summary>
        /// Gets the completed view models.
        /// </summary>
        /// <param name="viewModels">The view models.</param>
        /// <returns>the completed view models.</returns>
        public static List<InspectionListItemViewModel> GetCompletedViewModels(List<InspectionListItemViewModel> viewModels)
        {
            var results = new List<InspectionListItemViewModel>();

            if (viewModels != null)
            {
                foreach (var viewModel in viewModels)
                {
                    if (!viewModel.IsUpcomingInspection)
                    {
                        results.Add(viewModel);
                    }
                }

                results = results.OrderBy(p => p.InspectionViewModel.InspectionDataModel.LastUpdated).ToList();
            }

            return results;
        }

        /// <summary>
        /// Gets the resulted status statistics with status/count pairs;
        /// </summary>
        /// <param name="viewModels">The view models.</param>
        /// <returns>the resulted status statistics.</returns>
        public static Dictionary<string, int> GetResultedStatusStatistics(List<InspectionListItemViewModel> viewModels)
        {
            var result = new Dictionary<string, int>();

            if (viewModels != null)
            {
                var tempResult = from model in viewModels
                                 group model by model.InspectionViewModel.StatusText into g
                                 orderby g.Key
                                 select new { Name = g.Key, Count = g.Count() };
                result = tempResult.ToDictionary(p => p.Name, p => p.Count);
            }

            return result;
        }

        /// <summary>
        /// Builds the inspection list item view models.
        /// </summary>
        /// <param name="serviceProviderCode">The service provider code.</param>
        /// <param name="recordIDModel">The record ID model.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="publicUserID">The public user ID.</param>
        /// <param name="inspectionViewModels">The inspection view models.</param>
        /// <returns>the inspection list item view models</returns>
        public static List<InspectionListItemViewModel> BuildViewModels(string serviceProviderCode, CapIDModel recordIDModel, string moduleName, string publicUserID, List<InspectionViewModel> inspectionViewModels)
        {
            List<InspectionListItemViewModel> result = new List<InspectionListItemViewModel>();

            if (inspectionViewModels != null && inspectionViewModels.Count > 0)
            {
                foreach (var inspectionViewModel in inspectionViewModels)
                {
                    var row = new InspectionListItemViewModel();
                    row.InspectionViewModel = inspectionViewModel;
                    row.IsUpcomingInspection = inspectionViewModel.InspectionDataModel.IsUpcomingInspection;
                    string pattern = GetInspectionListRowPattern(inspectionViewModel, moduleName);

                    if (inspectionViewModel.InspectionDataModel.Status == InspectionStatus.PendingByACA || inspectionViewModel.InspectionDataModel.Status == InspectionStatus.PendingByV360)
                    {
                        pattern = pattern.Replace(ListItemVariables.ScheduledOrRequestedDateTime, inspectionViewModel.RequestedDateTimeText);
                        pattern = pattern.Replace(ListItemVariables.ScheduledOrRequestedDate, inspectionViewModel.RequestedDateText);
                        pattern = pattern.Replace(ListItemVariables.ScheduledOrRequestedTime, inspectionViewModel.RequestedTimeText);
                    }
                    else
                    {
                        pattern = pattern.Replace(ListItemVariables.ScheduledOrRequestedDateTime, inspectionViewModel.ScheduledDateTimeText);
                        pattern = pattern.Replace(ListItemVariables.ScheduledOrRequestedDate, inspectionViewModel.ScheduledDateText);
                        pattern = pattern.Replace(ListItemVariables.ScheduledOrRequestedTime, inspectionViewModel.ScheduledTimeText);
                    }

                    pattern = pattern.Replace(ListItemVariables.ResultedDateTime, inspectionViewModel.ResultedDateTimeText);
                    pattern = pattern.Replace(ListItemVariables.ResultedDate, inspectionViewModel.ResultedDateText);
                    pattern = pattern.Replace(ListItemVariables.ResultedTime, inspectionViewModel.ResultedTimeText);
                    pattern = pattern.Replace(ListItemVariables.ReadyTimeDateTime, inspectionViewModel.ReadyTimeDateTimeText);
                    pattern = pattern.Replace(ListItemVariables.ReadyTimeDate, inspectionViewModel.ReadyTimeDateText);
                    pattern = pattern.Replace(ListItemVariables.ReadyTimeTime, inspectionViewModel.ReadyTimeTimeText);
                    pattern = pattern.Replace(ListItemVariables.Operator, inspectionViewModel.LastUpdatedBy);
                    pattern = pattern.Replace(ListItemVariables.OperationDateTime, inspectionViewModel.LastUpdatedDateTimeText);
                    pattern = pattern.Replace(ListItemVariables.OperationDate, inspectionViewModel.LastUpdatedDateText);
                    pattern = pattern.Replace(ListItemVariables.OperationTime, inspectionViewModel.LastUpdatedTimeText);
                    pattern = pattern.Replace(ListItemVariables.InspectionType, inspectionViewModel.TypeText);
                    pattern = pattern.Replace(ListItemVariables.InspectionTypeSequenceNumber, inspectionViewModel.TypeID.ToString());
                    pattern = pattern.Replace(ListItemVariables.InspectionSequenceNumber, inspectionViewModel.ID.ToString());
                    var inspectorUnassignedText = LabelUtil.GetTextByKey("aca_inspection_list_unassigned", moduleName);
                    string inspectorText = inspectionViewModel.IsInspectorUnassigned ? inspectorUnassignedText : inspectionViewModel.Inspector;
                    pattern = pattern.Replace(ListItemVariables.Inspector, inspectorText);
                    pattern = pattern.Replace(ListItemVariables.Status, inspectionViewModel.StatusText);
                    pattern = pattern.Replace(ListItemVariables.RequiredOrOptional, inspectionViewModel.RequiredOrOptional);

                    // replace the estimated start time, end time.
                    string estStartTime = string.Empty;
                    string estEndTime = string.Empty;

                    if (inspectionViewModel.InspectionDataModel != null &&
                        inspectionViewModel.InspectionDataModel.InspectionModel != null && 
                        inspectionViewModel.InspectionDataModel.InspectionModel.activity != null)
                    {
                        estStartTime = inspectionViewModel.InspectionDataModel.InspectionModel.activity.estimatedStartTime;
                        estEndTime = inspectionViewModel.InspectionDataModel.InspectionModel.activity.estimatedEndTime;
                    }

                    string estimatedArrivalTime = InspectionViewUtil.GetEstimatedArrivateTime(estStartTime, estEndTime, moduleName);
                    pattern = pattern.Replace(ListItemVariables.EstimatedArrivalTime, estimatedArrivalTime);

                    row.CombinedInfo = string.Format(pattern, string.Empty);
                    List<InspectionActionViewModel> availableActions = InspectionActionViewUtil.BuildViewModels(serviceProviderCode, recordIDModel, moduleName, publicUserID, inspectionViewModel);
                    row.AvailableActions = availableActions == null ? null : availableActions.ToArray();

                    result.Add(row);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the inspection list row pattern.
        /// </summary>
        /// <param name="inspectionViewModel">The inspection view model.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>the inspection list row pattern.</returns>
        private static string GetInspectionListRowPattern(InspectionViewModel inspectionViewModel, string moduleName)
        {
            string result = string.Empty;

            if (inspectionViewModel != null && inspectionViewModel.InspectionDataModel != null)
            {
                if (inspectionViewModel.InspectionDataModel.IsUpcomingInspection)
                {
                    if (inspectionViewModel.InspectionDataModel.ReadyTimeAvailable)
                    {
                        //TBD at TBD, Ready Time: < Ready Date > at  < Ready Time >
                        result = LabelUtil.GetTextByKey("aca_inspection_upcominglist_readytime_combofield_pattern", moduleName);
                    }
                    else
                    {
                        result = LabelUtil.GetTextByKey("aca_inspection_upcominglist_combofield_pattern", moduleName);
                    }
                }
                else
                {
                    if (inspectionViewModel.InspectionDataModel.Status == InspectionStatus.Rescheduled || inspectionViewModel.InspectionDataModel.Status == InspectionStatus.Canceled)
                    {
                        //By <Operator> on <operation date> at <operate time>
                        if (inspectionViewModel.InspectionDataModel.Status == InspectionStatus.Rescheduled)
                        {
                            result = LabelUtil.GetTextByKey("aca_inspection_completedlist_rescheduled_combofield_pattern", moduleName);
                        }
                        else
                        {
                            result = LabelUtil.GetTextByKey("aca_inspection_completedlist_cancelled_combofield_pattern", moduleName);
                        }
                    }
                    else
                    {
                        result = LabelUtil.GetTextByKey("aca_inspection_completedlist_combofield_pattern", moduleName);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// list item variable keys
        /// </summary>
        public struct ListItemVariables
        {
            /// <summary>
            /// Ready Time datetime variable
            /// </summary>
            public const string ReadyTimeDateTime = "$$ReadyTimeDateTime$$";

            /// <summary>
            /// Ready Time date variable
            /// </summary>
            public const string ReadyTimeDate = "$$ReadyTimeDate$$";

            /// <summary>
            /// Ready Time time variable
            /// </summary>
            public const string ReadyTimeTime = "$$ReadyTimeTime$$";

            /// <summary>
            /// Scheduled Date Time variable
            /// </summary>
            public const string ScheduledOrRequestedDateTime = "$$ScheduledOrRequestedDateTime$$";

            /// <summary>
            /// Scheduled Date variable
            /// </summary>
            public const string ScheduledOrRequestedDate = "$$ScheduledOrRequestedDate$$";

            /// <summary>
            /// Scheduled Time variable
            /// </summary>
            public const string ScheduledOrRequestedTime = "$$ScheduledOrRequestedTime$$";

            /// <summary>
            /// Resulted Date Time variable
            /// </summary>
            public const string ResultedDateTime = "$$ResultedDateTime$$";

            /// <summary>
            /// Resulted Date variable
            /// </summary>
            public const string ResultedDate = "$$ResultedDate$$";

            /// <summary>
            /// Resulted Time variable
            /// </summary>
            public const string ResultedTime = "$$ResultedTime$$";

            /// <summary>
            /// Inspection Type variable
            /// </summary>
            public const string InspectionType = "$$InspectionType$$";

            /// <summary>
            /// Inspection Type Sequence Number variable
            /// </summary>
            public const string InspectionTypeSequenceNumber = "$$InspectionTypeSequenceNumber$$";

            /// <summary>
            /// Inspection Sequence Number variable
            /// </summary>
            public const string InspectionSequenceNumber = "$$InspectionSequenceNumber$$";

            /// <summary>
            /// Inspector variable
            /// </summary>
            public const string Inspector = "$$Inspector$$";

            /// <summary>
            /// Status variable
            /// </summary>
            public const string Status = "$$Status$$";

            /// <summary>
            /// Operator variable
            /// </summary>
            public const string Operator = "$$Operator$$";

            /// <summary>
            /// OperatorDateTime variable
            /// </summary>
            public const string OperationDateTime = "$$OperationDateTime$$";

            /// <summary>
            /// OperatorDate variable
            /// </summary>
            public const string OperationDate = "$$OperationDate$$";

            /// <summary>
            /// OperatorTime variable
            /// </summary>
            public const string OperationTime = "$$OperationTime$$";

            /// <summary>
            /// Required or optional variable
            /// </summary>
            public const string RequiredOrOptional = "$$RequiredOrOptional$$";

            /// <summary>
            /// The estimated arrival time.
            /// </summary>
            public const string EstimatedArrivalTime = "$$EstimatedArrivalTime$$";
        }
    }
}
