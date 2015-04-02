#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DrillDownSeriesWrapperModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: DrillDownSeriesWrapperModel.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion

using System.Collections.Generic;
using Accela.ACA.WSProxy;

namespace Accela.ACA.ComponentService.Model
{
    /// <summary>
    /// Drill DownSeries Wrapper Model of ASITableDrillDSeriesModel4WS
    /// </summary>
    public class DrillDownSeriesWrapperModel
    {
        /// <summary>
        /// WSDL model ASITableDrillDSeriesModel4WS of AA service
        /// </summary>
        private ASITableDrillDSeriesModel4WS ddSeries;

        /// <summary>
        /// List of Drill Down map items.
        /// </summary>
        private List<DrillDownItemWrapperModel> list;

        /// <summary>
        /// Initializes a new instance of the DrillDownSeriesWrapperModel class.
        /// </summary>
        /// <param name="drillDownSeries">WSDL model ASITableDrillDSeriesModel4WS of AA service</param>
        internal DrillDownSeriesWrapperModel(ASITableDrillDSeriesModel4WS drillDownSeries)
        {
            ddSeries = drillDownSeries;
            list = new List<DrillDownItemWrapperModel>();

            foreach (ASITableDrillDValMapModel4WS map in ddSeries.asiTableDrillDValMapModel4WSList)
            {
                DrillDownItemWrapperModel item = new DrillDownItemWrapperModel(map, ddSeries);
                list.Add(item);
            }
        }

        /// <summary>
        /// Gets Drill Down Series Id
        /// </summary>
        public string SeriesId
        {
            get
            {
                return ddSeries.seriesId;
            }
        }

        /// <summary>
        /// Gets Parent field of Drill Down Series
        /// </summary>
        public string ParentColName
        {
            get
            {
                return ddSeries.parentColName;
            }
        }

        /// <summary>
        /// Gets Child field of Drill Down Series
        /// </summary>
        public string ChildColName
        {
            get
            {
                return ddSeries.childColName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether support the multi select for drill down.
        /// </summary>
        public bool IsMultipleSelect
        {
            get
            {
                return ddSeries.selectType.ToLower().Equals("m");
            }
        }

        /// <summary>
        /// Gets List item of Drill Down
        /// </summary>
        public List<DrillDownItemWrapperModel> MapList
        {
            get
            {
                return list;
            }
        }
    }
}
