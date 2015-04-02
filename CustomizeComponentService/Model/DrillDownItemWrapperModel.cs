#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DrillDownItemWrapperModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: DrillDownItemWrapperModel.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion

using System.Collections.Generic;
using System.Text;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.ComponentService.Model
{
    /// <summary>
    /// Drill DownSeries Item Wrapper Model
    /// </summary>
    public class DrillDownItemWrapperModel
    {
        /// <summary>
        /// id of item
        /// </summary>
        private string id = string.Empty;

        /// <summary>
        /// value of item
        /// </summary>
        private string val = string.Empty;

        /// <summary>
        /// name path of item
        /// </summary>
        private string namePath = string.Empty;

        /// <summary>
        /// Initializes a new instance of the DrillDownItemWrapperModel class.
        /// </summary>
        /// <param name="paraId">the id of the item</param>
        /// <param name="paraVal">the value of the item</param>
        internal DrillDownItemWrapperModel(string paraId, string paraVal)
        {
            id = paraId;
            val = paraVal;
            namePath = paraVal;
        }

        /// <summary>
        /// Initializes a new instance of the DrillDownItemWrapperModel class.
        /// </summary>
        /// <param name="paraId">para Id</param>
        /// <param name="paraVal">para Val</param>
        /// <param name="paraNamePath">para Name Path</param>
        internal DrillDownItemWrapperModel(string paraId, string paraVal, string paraNamePath)
        {
            id = paraId;
            val = paraVal;
            namePath = paraNamePath;
        }

        /// <summary>
        /// Initializes a new instance of the DrillDownItemWrapperModel class.
        /// </summary>
        /// <param name="item">WSDL model ASITableDrillDValMapModel4WS of AA service</param>
        /// <param name="drillDownSeries">WSDL model ASITableDrillDSeriesModel4WS of AA service</param>
        internal DrillDownItemWrapperModel(ASITableDrillDValMapModel4WS item, ASITableDrillDSeriesModel4WS drillDownSeries)
        {
            if (string.IsNullOrEmpty(drillDownSeries.uniqueLabel))
            {
                id = item.childValueId;
            }
            else
            {
                id = drillDownSeries.uniqueLabel + "_" + item.childValueId;
            }

            val = I18nStringUtil.GetString(item.resChildValueName, item.childValueName);
        }

        /// <summary>
        /// Gets item id ,format:parentId_currentId
        /// </summary>
        public string ItemId
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// Gets item value, like "Floor" in the Unit Type.
        /// </summary>
        public string ItemValue
        {
            get
            {
                return val;
            }
        }

        /// <summary>
        /// Gets name path, like this :"first/second/.../last"
        /// </summary>
        public string NamePath
        {
            get
            {
                return namePath;
            }
        }

        /// <summary>
        /// Convert string[][] search result to list of DrillDownItemWrapperModel
        /// </summary>
        /// <param name="searchResult">the search result of the AA service</param>
        /// <returns>list of DrillDownItemWrapperModel</returns>
        public static List<DrillDownItemWrapperModel> ConvertFromSearchResult(string[][] searchResult)
        {
            List<DrillDownItemWrapperModel> list = new List<DrillDownItemWrapperModel>();

            if (searchResult != null && searchResult.Length > 0)
            {
                for (int i = 1; i < searchResult.Length; i++)
                {
                    string[] resultItem = searchResult[i];
                    string idPath = GetIdPathFromSearchResultItem(resultItem);
                    string name = GetNameFromSearchResultItem(resultItem);
                    string namePath = GetNamePathFromSearchResultItem(resultItem);

                    DrillDownItemWrapperModel item = new DrillDownItemWrapperModel(idPath, name, namePath);
                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// Create empty list for showing header in grid view
        /// </summary>
        /// <returns>list of DrillDownItemWrapperModel</returns>
        public static List<DrillDownItemWrapperModel> CreateEmptyList()
        {
            List<DrillDownItemWrapperModel> list = new List<DrillDownItemWrapperModel>();
            list.Add(new DrillDownItemWrapperModel("test", "test"));

            return list;
        }

        /// <summary>
        /// join the element of string[] named from id 1 to id n
        /// the format of element is "id:name"
        /// </summary>
        /// <param name="searchResultItem">the id path</param>
        /// <returns>the id path of the item</returns>
        private static string GetIdPathFromSearchResultItem(string[] searchResultItem)
        {
            StringBuilder idPath = new StringBuilder(string.Empty);

            foreach (string item in searchResultItem)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    string[] splitArr = item.Split(':');

                    if (!string.IsNullOrEmpty(splitArr[0]))
                    {
                        if (idPath.Length > 0)
                        {
                            idPath.Append("_");
                        }

                        idPath.Append(splitArr[0]);
                    }
                }
            }

            return idPath.ToString();
        }

        /// <summary>
        /// get the last element name of string[] the format of element is "id:name"
        /// </summary>
        /// <param name="searchReslutItem">id:name is the format of item.</param>
        /// <returns>the name of the item</returns>
        private static string GetNameFromSearchResultItem(string[] searchReslutItem)
        {
            string name = string.Empty;

            string lastItem = searchReslutItem[searchReslutItem.Length - 1];

            string[] splitArr = lastItem.Split(':');

            if (!string.IsNullOrEmpty(splitArr[1]))
            {
                name = splitArr[1];
            }

            return name;
        }

        /// <summary>
        /// get the name path of the last item of search result,the format of element is "id: name"
        /// </summary>
        /// <param name="searchReslutItem">id: name is the format of item.</param>
        /// <returns>return string "first/second/.../last"</returns>
        private static string GetNamePathFromSearchResultItem(string[] searchReslutItem)
        {
            StringBuilder namePath = new StringBuilder();

            foreach (string sItem in searchReslutItem)
            {
                string[] splitArr = sItem.Split(':');
                if (!string.IsNullOrEmpty(splitArr[1]))
                {
                    namePath.Append(" ");
                    namePath.Append(splitArr[1]);
                    namePath.Append(" ");
                    namePath.Append("/");
                }
            }

            namePath.Remove(namePath.Length - 1, 1);

            return namePath.ToString();
        }
    }
}
