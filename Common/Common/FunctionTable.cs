#region Header

/**
 *  Accela Citizen Access
 *  File: FunctionTable.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *   This is the function table which control the function switch.
 *
 *  Notes:
 * $Id: FunctionTable.cs 199670 2011-07-18 08:47:32Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System.Collections.Generic;
using Accela.ACA.CustomizeAPI;

namespace Accela.ACA.Common.Common
{
    /// <summary>
    /// This is the function table which control the function switch.
    /// </summary>
    public static class FunctionTable
    {
        #region Methods

        /// <summary>
        /// Determines whether it enable [clone record].
        /// </summary>
        /// <returns>Return true if it enable [clone record]; otherwise false.</returns>
        public static bool IsEnableCloneRecord()
        {
            return IsEnableCreateApplication() && GetFunctionItemEnable(FunctionItem.CloneRecord);
        }

        /// <summary>
        /// Determines whether it enable [create application].
        /// </summary>
        /// <returns>Return true if it enable [create application]; otherwise false.</returns>
        public static bool IsEnableCreateApplication()
        {
            return GetFunctionItemEnable(FunctionItem.CreateApplication);
        }

        /// <summary>
        /// Determines whether it enable [create amendment].
        /// </summary>
        /// <returns>Return true if it enable [create amendment]; otherwise false.</returns>
        public static bool IsEnableCreateAmendment()
        {
            return GetFunctionItemEnable(FunctionItem.CreateAmendment);
        }

        /// <summary>
        /// Determines whether it enable [make payment].
        /// </summary>
        /// <returns>Return true if it enable [make payment]; otherwise false.</returns>
        public static bool IsEnableMakePayment()
        {
            return GetFunctionItemEnable(FunctionItem.MakePayment);
        }

        /// <summary>
        /// Determines whether it enable [obtain fee estimate].
        /// </summary>
        /// <returns>Return true if it enable [obtain fee estimate]; otherwise false.</returns>
        public static bool IsEnableObtainFeeEstimate()
        {
            return GetFunctionItemEnable(FunctionItem.ObtainFeeEstimate);
        }

        /// <summary>
        /// Determines whether it enable [renew record].
        /// </summary>
        /// <returns>Return true if it enable [renew record]; otherwise false.</returns>
        public static bool IsEnableRenewRecord()
        {
            return GetFunctionItemEnable(FunctionItem.RenewRecord);
        }

        /// <summary>
        /// Determines whether it enable [schedule inspection].
        /// </summary>
        /// <returns>Return true if it enable [schedule inspection]; otherwise false.</returns>
        public static bool IsEnableScheduleInspection()
        {
            return GetFunctionItemEnable(FunctionItem.ScheduleInspection);
        }

        /// <summary>
        /// Determines whether it enable [upload document].
        /// </summary>
        /// <returns>Return true if it enable [upload document]; otherwise false.</returns>
        public static bool IsEnableUploadDocument()
        {
            return GetFunctionItemEnable(FunctionItem.UploadDocument);
        }

        /// <summary>
        /// Initialize the customize permission.
        /// </summary>
        /// <param name="configPath">The config path.</param>
        public static void InitCustomizePermission(string configPath)
        {
            IGrantPermission grantPermissionBll = null;

            // use try catch block, if not make customize configuartion, do not throw exception. 
            try
            {
                grantPermissionBll = ObjectFactory.GetObjectByConfiguration<IGrantPermission>(configPath, "IGrantPermission");
            }
            catch
            {
                // do nothing, NOT throw exception
            }

            if (grantPermissionBll != null)
            {
                grantPermissionBll.GrantPermission();
            }
        }

        /// <summary>
        /// Gets the function item enable or not.
        /// </summary>
        /// <param name="item">The function item.</param>
        /// <returns>Return the function item enable or not. Default is true.</returns>
        private static bool GetFunctionItemEnable(FunctionItem item)
        {
            IDictionary<FunctionItem, bool> dictPermission = BaseCustomizeComponent.UserContext.Permissions;

            if (dictPermission != null && dictPermission.ContainsKey(item))
            {
                return dictPermission[item];
            }

            return true;
        }

        #endregion Methods
    }
}
