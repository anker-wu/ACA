#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: UIModelUtil.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description: UI models convertion utility.
*
*  Notes:
* $Id: UIModelUtil.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Jul 6, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Util
{
    /// <summary>
    /// A enumeration to separate the data type in the data container.
    /// </summary>
    public enum UIDataType
    {
        /// <summary>
        /// UI data for ASI.
        /// </summary>
        ASI,

        /// <summary>
        /// UI data for ASIT.
        /// </summary>
        ASIT,

        /// <summary>
        /// Copy of UI data for ASIT.
        /// </summary>
        ASITCopy,
        
        /// <summary>
        /// Edit UI data for ASIT.
        /// </summary>
        ASITEdit,

        /// <summary>
        /// UI data for Contact.
        /// </summary>
        Contact,

        /// <summary>
        /// UI data for Licensed Professional.
        /// </summary>
        LP
    }

    /// <summary>
    /// UI models convert utility
    /// </summary>
    public static class UIModelUtil
    {
        /// <summary>
        /// Clear the UI date from UI container.
        /// </summary>
        public static void ClearUIData()
        {
            AppSession.SetUIDataToSession(null);
        }

        /// <summary>
        /// Get UI data from UI container by UI data type.
        /// </summary>
        /// <param name="dataType">UI data type.</param>
        /// <returns>A dictionary contains the data keys and data tables.</returns>
        public static Dictionary<string, UITable[]> GetDataFromUIContainer(UIDataType dataType)
        {
            Hashtable uiData = AppSession.GetUIDataFromSession();

            if (uiData == null || !uiData.ContainsKey(dataType))
            {
                return null;
            }

            return uiData[dataType] as Dictionary<string, UITable[]>;
        }

        /// <summary>
        /// Get UI data from UI container.
        /// </summary>
        /// <param name="dataType">UI data type.</param>
        /// <param name="dataKey">UI data key.</param>
        /// <returns>Array of UI table.</returns>
        public static UITable[] GetDataFromUIContainer(UIDataType dataType, string[] dataKey)
        {
            Dictionary<string, UITable[]> uiTableList = GetDataFromUIContainer(dataType);

            if (uiTableList == null)
            {
                return null;
            }

            if (dataKey != null)
            {
                uiTableList = uiTableList.Where(v => dataKey.Contains(v.Key)).ToDictionary(v => v.Key, v => v.Value);
            }

            if (uiTableList.Count > 0)
            {
                switch (dataType)
                {
                    case UIDataType.ASIT:
                    case UIDataType.ASITCopy:
                    case UIDataType.ASITEdit:
                        List<ASITUITable> asitUITables = new List<ASITUITable>();

                        foreach (ASITUITable[] tables in uiTableList.Values)
                        {
                            if (tables != null)
                            {
                                asitUITables.AddRange(tables);
                            }
                        }

                        return asitUITables.ToArray();
                    default:
                        List<UITable> uiTables = new List<UITable>();

                        foreach (UITable[] tables in uiTableList.Values)
                        {
                            if (tables != null)
                            {
                                uiTables.AddRange(tables);
                            }
                        }

                        return uiTables.ToArray();
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Set the UI data to UI container.
        /// </summary>
        /// <param name="uiTableList">A dictionary contains the data keys and data tables.</param>
        /// <param name="dataType">UI data type.</param>
        public static void SetDataToUIContainer(Dictionary<string, UITable[]> uiTableList, UIDataType dataType)
        {
            Hashtable uiData = AppSession.GetUIDataFromSession();

            if (uiData == null)
            {
                uiData = new Hashtable();
            }

            if (!uiData.ContainsKey(dataType))
            {
                uiData.Add(dataType, uiTableList);
            }
            else
            {
                uiData[dataType] = uiTableList;
            }

            AppSession.SetUIDataToSession(uiData);
        }

        /// <summary>
        /// Set UI data to UI container.
        /// </summary>
        /// <param name="uiTables">Array of UI table.</param>
        /// <param name="dataType">UI data type.</param>
        /// <param name="dataKey">UI data key.</param>
        public static void SetDataToUIContainer(UITable[] uiTables, UIDataType dataType, string dataKey)
        {
            Dictionary<string, UITable[]> uiTableList = GetDataFromUIContainer(dataType);
            dataKey = string.IsNullOrEmpty(dataKey) ? string.Empty : dataKey;

            if (uiTableList == null)
            {
                uiTableList = new Dictionary<string, UITable[]>();
            }

            if (uiTableList.ContainsKey(dataKey))
            {
                uiTableList[dataKey] = uiTables;
            }
            else
            {
                uiTableList.Add(dataKey, uiTables);
            }

            SetDataToUIContainer(uiTableList, dataType);
        }
    }
}