#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapWrapperModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapWrapperModel.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Linq;
using Accela.ACA.WSProxy;

namespace Accela.ACA.ComponentService.Model
{
    /// <summary>
    /// This class provides one simple cap wrapper model for custom component
    /// </summary>
    public class CapWrapperModel
    {
        /// <summary>
        /// cap model
        /// </summary>
        private CapModel4WS cap;

        /// <summary>
        /// Initializes a new instance of the CapWrapperModel class
        /// </summary>
        /// <param name="capModel4Ws">a capModel</param>
        internal CapWrapperModel(CapModel4WS capModel4Ws)
        {
            cap = capModel4Ws;
        }

        /// <summary>
        /// Get ASI Field Value
        /// </summary>
        /// <param name="fieldName">field Name</param>
        /// <param name="asiGroupCode">asi Group Code</param>
        /// <param name="asiSubGroupName">asi Sub Group Name</param>
        /// <returns>ASI Field Value</returns>
        public string GetASIFieldValue(string fieldName, string asiGroupCode, string asiSubGroupName)
        {
            string fieldValue = string.Empty;
            AppSpecificInfoGroupModel4WS[] asiGroups = cap.appSpecificInfoGroups;

            AppSpecificInfoGroupModel4WS selectedAsi = asiGroups.FirstOrDefault(t => (!string.IsNullOrEmpty(t.groupCode) && t.groupCode.ToLower() == asiGroupCode.ToLower()) && (!string.IsNullOrEmpty(t.groupName) && t.groupName.ToLower() == asiSubGroupName.ToLower()));

            if (selectedAsi != null)
            {
                AppSpecificInfoModel4WS[] fields = selectedAsi.fields;

                foreach (AppSpecificInfoModel4WS field in fields)
                {
                    if (field.checkboxDesc.ToLower() == fieldName.ToLower())
                    {
                        fieldValue = field.checklistComment;
                        break;
                    }
                }
            }

            return fieldValue;
        }
    }
}
