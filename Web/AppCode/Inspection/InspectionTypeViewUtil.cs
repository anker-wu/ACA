#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionTypeViewController.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionTypeViewUtil.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Inspection
{
    /// <summary>
    /// Inspection type View Utility
    /// </summary>
    public static class InspectionTypeViewUtil
    {
        /// <summary>
        /// the key of category Others
        /// </summary>
        private const string CATEGORY_OTHERS_KEY = "\fOthers\f";

        /// <summary>
        /// Gets the inspection type view models.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="recordModel">The record model.</param>
        /// <param name="showOptionalInspections">if set to <c>true</c> [show optional inspections].</param>
        /// <param name="user">The user model.</param>
        /// <returns>the inspection type view models.</returns>
        public static List<InspectionTypeViewModel> GetInspectionTypeViewModels(string moduleName, CapModel4WS recordModel, bool showOptionalInspections, User user)
        {
            var results = new List<InspectionTypeViewModel>();

            if (recordModel != null)
            {
                var inspectionTypeBll = ObjectFactory.GetObject<IInspectionTypeBll>();

                bool isCapLockedOrHold = ConditionsUtil.IsConditionLockedOrHold(recordModel.capID, user.UserSeqNum);

                // get inspection type list base on Cap type
                var availableInspectionTypes = inspectionTypeBll.GetAvailableInspectionTypes(moduleName, recordModel, isCapLockedOrHold, AppSession.User.UserModel4WS, showOptionalInspections, null, user.PublicUserId);

                if (availableInspectionTypes != null)
                {
                    foreach (var dataModel in availableInspectionTypes)
                    {
                        var viewModel = BuildViewModel(moduleName, dataModel);

                        if (viewModel.Visible)
                        {
                            results.Add(viewModel);
                        }
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Gets the optional inspection count.
        /// </summary>
        /// <param name="inspectionTypeViewModels">The inspection type view models.</param>
        /// <returns>the optional inspection count.</returns>
        public static int GetOptionalInspectionCount(List<InspectionTypeViewModel> inspectionTypeViewModels)
        {
            int result = 0;

            if (inspectionTypeViewModels != null)
            {
                result = (from i in inspectionTypeViewModels
                         where i.InspectionTypeDataModel.Required == false
                         select i).Count();
            }

            return result;
        }

        /// <summary>
        /// Filters the inspection type view models.
        /// </summary>
        /// <param name="viewModels">The view models.</param>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="toShowOptionalInspections">if set to <c>true</c> [to show optional inspections].</param>
        /// <returns>the inspection type view models.</returns>
        public static List<InspectionTypeViewModel> FilterInspectionTypeViewModels(List<InspectionTypeViewModel> viewModels, string categoryID, bool toShowOptionalInspections)
        {
            var results = viewModels;

            if (results != null)
            {
                categoryID = string.IsNullOrEmpty(categoryID) ? string.Empty : categoryID;
                bool filter4Others = CATEGORY_OTHERS_KEY.Equals(categoryID, StringComparison.OrdinalIgnoreCase);

                results =
                    filter4Others ?
                    (from v in viewModels
                     where v.InspectionTypeDataModel != null
                         && (v.InspectionTypeDataModel.Categories == null || v.InspectionTypeDataModel.Categories.Count() == 0)
                         && (v.InspectionTypeDataModel.Required == true
                         || (toShowOptionalInspections && v.InspectionTypeDataModel.Required == false))
                     select v).ToList()
                     :
                    (from v in viewModels
                     where v.InspectionTypeDataModel != null
                     && v.InspectionTypeDataModel.Categories != null
                     && (v.InspectionTypeDataModel.Required == true
                         || (toShowOptionalInspections && v.InspectionTypeDataModel.Required == false))
                     from c in v.InspectionTypeDataModel.Categories
                     where c.ID == categoryID
                     select v).ToList();
            }

            return results;
        }

        /// <summary>
        /// Gets the inspection categories.
        /// </summary>
        /// <param name="inspectionTypeViewModels">The inspection type view models.</param>
        /// <param name="recordIDModel">The record ID model.</param>
        /// <returns>the inspection categories.</returns>
        public static List<InspectionCategoryDataModel> GetInspectionCategories(List<InspectionTypeViewModel> inspectionTypeViewModels, CapIDModel recordIDModel)
        {
            var result = new List<InspectionCategoryDataModel>();
            var inspectionTypeBll = ObjectFactory.GetObject<IInspectionTypeBll>();

            var categoryDataModels = inspectionTypeBll.GetInspectionCategoriesByCapID(recordIDModel);

            //get interset by available categories and categories of inspection types
            if (categoryDataModels != null && inspectionTypeViewModels != null)
            {
                categoryDataModels =
                                     (from c in categoryDataModels
                                      from i in inspectionTypeViewModels
                                      where i.InspectionTypeDataModel != null
                                      from c1 in i.InspectionTypeDataModel.Categories
                                      where c.ID == c1.ID
                                      select c).Distinct().ToList();
            }

            //sort categories by display text
            if (categoryDataModels != null)
            {
                categoryDataModels = categoryDataModels.OrderBy(p => p.Category).ToList();
            }

            //check if exist inspections with no category
            bool existInspectionsWithNoCategory =
                inspectionTypeViewModels == null ? false :
                (from i in inspectionTypeViewModels
                 where i.InspectionTypeDataModel != null
                  && (i.InspectionTypeDataModel.Categories == null || i.InspectionTypeDataModel.Categories.Count() == 0)
                 select i).Count() > 0;

            //add others category if categories count >0 and exist inspections with no category
            if (categoryDataModels != null && categoryDataModels.Count > 0 && existInspectionsWithNoCategory)
            {
                var othersCategory = new InspectionCategoryDataModel();
                othersCategory.ID = CATEGORY_OTHERS_KEY;
                othersCategory.Category = LabelUtil.GetTextByKey("inspection_category_others", string.Empty);
                othersCategory.DisplayOrder = 99999;
                categoryDataModels.Add(othersCategory);
            }

            if (categoryDataModels != null)
            {
                result = categoryDataModels;
            }

            return result;
        }

        /// <summary>
        /// Builds the view model.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="dataModel">The data model.</param>
        /// <returns>the view model.</returns>
        public static InspectionTypeViewModel BuildViewModel(string moduleName, InspectionTypeDataModel dataModel)
        {
            var result = new InspectionTypeViewModel();

            if (dataModel != null)
            {
                InspectionTypeModel inspectionTypeModel = dataModel.InspectionTypeModel;
                result.InspectionTypeDataModel = dataModel;
                result.TypeID = dataModel.TypeID;
                result.TypeText = I18nStringUtil.GetString(inspectionTypeModel.resType, inspectionTypeModel.type);
                result.StatusText = string.Empty;
                result.GroupText = I18nStringUtil.GetString(inspectionTypeModel.resGroupCodeName, inspectionTypeModel.groupCodeName, inspectionTypeModel.resGroupCode, inspectionTypeModel.groupCode);
                result.RequiredOrOptional = dataModel.Required ? LabelUtil.GetTextByKey("ACA_Inspection_Status_Required", moduleName) : LabelUtil.GetTextByKey("ACA_Inspection_Status_Optional", moduleName);
                result.Visible = dataModel.Actionable;
                result.Enabled = dataModel.Actionable;
                result.ShowFlowCompletedIcon = dataModel.Status == InspectionStatus.FlowCompleted || dataModel.Status == InspectionStatus.FlowCompletedButActive;
                result.ShowFlowScheduleActiveIcon = dataModel.MainAction == InspectionAction.Schedule && dataModel.InspectionTypeModel.isConfiguredInInspFlow && !dataModel.InAdvance;
            }

            return result;
        }

        /// <summary>
        /// Get status label key
        /// </summary>
        /// <param name="actualStatus">the instance of InspectionStatus</param>
        /// <returns>status label key</returns>
        private static string GetLabelKey(InspectionStatus actualStatus)
        {
            string result = string.Empty;

            switch (actualStatus)
            {
                case InspectionStatus.InitialRequired:
                    result = "ACA_Inspection_Status_Required";
                    break;

                case InspectionStatus.InitialOptional:
                    result = "ACA_Inspection_Status_Optional";
                    break;

                case InspectionStatus.FlowPrerequisiteNotMet:
                case InspectionStatus.Unknown:
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }
    }
}
