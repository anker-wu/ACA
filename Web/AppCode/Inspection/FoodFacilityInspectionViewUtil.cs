#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FoodFacilityInspectionViewUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: FoodFacilityInspectionViewUtil.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
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
    /// Food Facility Inspection View Utility
    /// </summary>
    public static class FoodFacilityInspectionViewUtil
    {
        /// <summary>
        /// Gets the view models.
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="licenseType">The license type.</param>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>the view models.</returns>
        public static List<InspectionViewModel> GetViewModels(CapModel4WS recordModel, string licenseType, QueryFormat queryFormat)
        {
            var inspectionBll = ObjectFactory.GetObject<IInspectionBll>();
            var inspectionModels = inspectionBll.GetInspectionListForFoodFacility(recordModel, licenseType, queryFormat);

            var result = InspectionViewUtil.BuildViewModels(null, inspectionModels);
            return result;
        }

        /// <summary>
        /// Gets the most inspection view model.
        /// </summary>
        /// <param name="viewModels">The view models.</param>
        /// <returns>the most inspection view model.</returns>
        public static InspectionViewModel GetTheMostInspectionViewModel(List<InspectionViewModel> viewModels)
        {
            InspectionViewModel result = null;

            if (viewModels != null)
            {
                result = viewModels.OrderByDescending(
                        p =>
                        p != null
                        && p.InspectionDataModel != null
                        && p.InspectionDataModel.ResultedDateTime != null
                        ? p.InspectionDataModel.ResultedDateTime.Value : DateTime.MinValue).FirstOrDefault();
            }

            return result;
        }
    }
}
