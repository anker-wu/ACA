#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  LabelUtil for getting label by label key.
 *  UI should call this class if need to get text in .cs.
 *
 *  Notes:
 *      $Id: LabelUtil.cs 278432 2014-09-03 11:28:33Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.View;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// The utility for getting label by label key.
    /// UI should call this class if need to get text in .cs.
    /// </summary>
    public static class LabelUtil
    {
        #region Fields

        /// <summary>
        /// Hard-Coded GUI Text Key Index Table
        /// </summary>
        private static Dictionary<HardCodedTextType, Dictionary<string, string>> _hardCodedGuiTextKeyIndexTable = new Dictionary<HardCodedTextType, Dictionary<string, string>>();

        /// <summary>
        /// <c>IViewBll</c> interface.
        /// </summary>
        private static IViewBll _viewBll = ObjectFactory.GetObject<IViewBll>();

        #endregion Fields

        #region Enumerations

        /// <summary>
        /// hard coded type
        /// </summary>
        private enum HardCodedTextType
        {
            /// <summary>
            /// Smart Choice
            /// </summary>
            OfSmartChoice,

            /// <summary>
            /// Cap Condition Severity
            /// </summary>
            OfCapConditionSeverity,

            /// <summary>
            /// Inspection Action
            /// </summary>
            OfInspectionAction,

            /// <summary>
            /// Inspection Result
            /// </summary>
            OfInspectionResult
        }

        #endregion Enumerations

        #region Methods

        /// <summary>
        /// Build unique label key by XUIText model
        /// </summary>
        /// <param name="textModel">XUITextModel object.</param>
        /// <returns>label key with different level.</returns>
        public static string BuildLabelKey(XUITextModel textModel)
        {
            string cacheKey = string.Empty;

            if (ACAConstant.GLOBAL.Equals(textModel.textLevelType, StringComparison.OrdinalIgnoreCase))
            {
                cacheKey = textModel.stringKey;
            }
            else if (ACAConstant.LEVEL_TYPE_AGENCY.Equals(textModel.textLevelType, StringComparison.OrdinalIgnoreCase))
            {
                cacheKey = textModel.servProvCode + "_" + textModel.stringKey;
            }
            else if (ACAConstant.LEVEL_TYPE_PAGEFLOW.Equals(textModel.textLevelType, StringComparison.InvariantCultureIgnoreCase))
            {
                cacheKey = textModel.servProvCode + "_" + ACAConstant.LEVEL_TYPE_PAGEFLOW + "_" + textModel.textLevelName + "_" + textModel.stringKey;
            }
            else
            {
                cacheKey = textModel.servProvCode + "_" + textModel.textLevelName + "_" + textModel.stringKey;
            }

            return cacheKey;
        }

        /// <summary>
        /// Gets the admin UI text by key.
        /// </summary>
        /// <param name="key">The Admin UI key.</param>
        /// <returns>the Admin UI text</returns>
        public static string GetAdminUITextByKey(string key)
        {
            return GetDefaultLanguageGlobalTextByKey(key);
        }

        /// <summary>
        /// Get the default language global text(label message) by label key.
        /// </summary>
        /// <param name="key">The label key.</param>
        /// <returns>The default language global text(label message).</returns>
        public static string GetDefaultLanguageGlobalTextByKey(string key)
        {
            return GetGlobalTextByKey(key, _viewBll.GetLabelKeys(ConfigManager.AgencyCode, I18nCultureUtil.DefaultCulture));
        }

        /// <summary>
        /// Get the default language global text(label message) by label key.
        /// </summary>
        /// <param name="key">The label key.</param>
        /// <param name="moduleName">module name.</param>
        /// <returns>The default language global text(label message).</returns>
        public static string GetDefaultLanguageTextByKey(string key, string moduleName)
        {
            return GetTextByKey(ConfigManager.AgencyCode, key, _viewBll.GetLabelKeys(ConfigManager.AgencyCode, I18nCultureUtil.DefaultCulture), moduleName);
        }

        /// <summary>
        /// Get the  text(label message) by label key in agency level.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The label text(label message) according the key.if can't find the key, it should return 'N/A'.</returns>
        public static string GetGlobalTextByKey(string key)
        {
            return GetTextByKey(key, string.Empty);
        }

        /// <summary>
        /// Get the GUI text by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The GUI text</returns>
        public static string GetGUITextByKey(string key)
        {
            string result = string.Empty;
            Hashtable labels = _viewBll.GetLabelKeys(ConfigManager.AgencyCode);

            if (labels.Contains(key))
            {
                result = labels[key].ToString();
            }

            if (result == key && key.EndsWith("|sub", StringComparison.OrdinalIgnoreCase))
            {
                result = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// get GUI text by cap condition severity key
        /// </summary>
        /// <param name="key">the key of condition severity.</param>
        /// <returns>the concrete text according the key.</returns>
        public static string GetGuiTextForCapConditionSeverity(string key)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(key))
            {
                return result;
            }

            string guiTextKey = GetHardCodedGuiTextKey(HardCodedTextType.OfCapConditionSeverity, key);

            if (!string.IsNullOrEmpty(guiTextKey))
            {
                result = GetTextByKey(guiTextKey, string.Empty);
            }

            if (string.IsNullOrEmpty(result))
            {
                result = key;
            }

            return result;
        }

        /// <summary>
        /// get GUI text by inspection action key
        /// </summary>
        /// <param name="key">The label key.</param>
        /// <returns>The GUI text.</returns>
        public static string GetGuiTextForInspectionAction(string key)
        {
            string result = string.Empty;
            string guiTextKey = GetHardCodedGuiTextKey(HardCodedTextType.OfInspectionAction, key);

            if (!string.IsNullOrEmpty(guiTextKey))
            {
                result = GetTextByKey(guiTextKey, string.Empty);
            }

            if (string.IsNullOrEmpty(result))
            {
                result = key;
            }

            return result;
        }

        /// <summary>
        /// get GUI text by inspection result key
        /// </summary>
        /// <param name="key">the key for inspection.</param>
        /// <returns>the GUI text value.</returns>
        public static string GetGuiTextForInspectionResult(string key)
        {
            string result = string.Empty;
            string guiTextKey = GetHardCodedGuiTextKey(HardCodedTextType.OfInspectionResult, key);

            if (!string.IsNullOrEmpty(guiTextKey))
            {
                result = GetTextByKey(guiTextKey, string.Empty);
            }

            if (string.IsNullOrEmpty(result))
            {
                result = key;
            }

            return result;
        }

        /// <summary>
        /// Get I18N tab name by module name.
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <returns>Tab label name.</returns>
        public static string GetI18NModuleName(string moduleName)
        {
            IList<TabItem> tabList = TabUtil.GetTabList();

            if (tabList == null)
            {
                return string.Empty;
            }

            foreach (TabItem tab in tabList)
            {
                if (tab.Module == null || !tab.Module.Equals(moduleName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string label = LabelUtil.GetTextByKey(tab.Label, tab.Module);

                if (label == LabelUtil.GetGlobalTextByKey("aca_sys_default_module_name"))
                {
                    label = DataUtil.AddBlankToString(tab.Module);
                }

                return RemoveHtmlFormat(label);
            }

            return moduleName;
        }

        /// <summary>
        /// Get the text content by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <param name="moduleName">module name.</param>
        /// <returns>The text content according the key.</returns>
        public static string GetTextContentByKey(string key, string moduleName)
        {
            return GetTextByKey(key + ACAConstant.LABEL_CONTENT_SUFFIX, moduleName);
        }

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <param name="moduleName">module name.</param>
        /// <returns>The text(label message) according the key.if can't find the key, it should return 'N/A'.</returns>
        public static string GetTextByKey(string key, string moduleName)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            Hashtable labels = _viewBll.GetLabelKeys(ConfigManager.AgencyCode);

            return GetTextByKey(ConfigManager.AgencyCode, key, labels, moduleName);
        }

        /// <summary>
        /// Get the text(label message) by label key and agency code.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <param name="agencyCode">agency Code</param>
        /// <param name="moduleName">module name.</param>
        /// <returns>The text(label message) according the key.if can't find the key, it should return 'N/A'.</returns>
        public static string GetTextByKey(string key, string agencyCode, string moduleName)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            Hashtable labels = _viewBll.GetLabelKeys(agencyCode);

            return GetTextByKey(agencyCode, key, labels, moduleName);
        }

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <param name="moduleName">module name.</param>
        /// <returns>The text(label message) according the key.if can't find the key, it should return 'N/A'.</returns>
        public static string GetSuperAgencyTextByKey(string key, string moduleName)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            string agencyCode = ConfigManager.AgencyCode;

            if (!AppSession.IsAdmin)
            {
                agencyCode = ConfigManager.SuperAgencyCode;
            }

            Hashtable labels = _viewBll.GetLabelKeys(agencyCode);

            return GetTextByKey(agencyCode, key, labels, moduleName);
        }

        /// <summary>
        /// Remove html format
        /// </summary>
        /// <param name="value">the input value</param>
        /// <returns>the value without html tag</returns>
        public static string RemoveHtmlFormat(string value)
        {
            if (!string.IsNullOrEmpty(value) && value.IndexOf("<", StringComparison.Ordinal) > -1)
            {
                Regex reg = new Regex(@"(?></?\w+)(?>(?:[^<>'""]+|'[^']*'|""[^""]*"")*)>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                value = reg.Replace(value, string.Empty);
            }

            return value;
        }

        /// <summary>
        /// word after break.(use for long word break line.)
        /// </summary>
        /// <param name="len">break length</param>
        /// <param name="content">the string content</param>
        /// <returns>word after break.</returns>
        public static string BreakWord(int len, string content)
        {
            string newString = string.Empty;

            if (len > 0 && !string.IsNullOrEmpty(content))
            {
                while (content.Length > len)
                {
                    newString += content.Substring(0, len) + "<br/>";
                    content = content.Substring(len, content.Length - len);
                }

                newString += content;
            }

            return newString;
        }

        /// <summary>
        /// Gets the title by key.
        /// </summary>
        /// <param name="alt">The alt string.</param>
        /// <param name="title">The title string.</param>
        /// <param name="needEncode">Indicating whether the text needs to be encoded or not</param>
        /// <param name="moduleName">module name.</param>
        /// <returns>the last title string</returns>
        public static string GetTitleByKey(string alt, string title, bool needEncode, string moduleName)
        {
            string[] newTitle = { GetTextByKey(alt, moduleName), GetTextByKey(title, moduleName) };
            string rawTitle = RemoveHtmlFormat(DataUtil.ConcatStringWithSplitChar(newTitle, ACAConstant.BLANK));
            string finalTitle = needEncode ? ScriptFilter.AntiXssHtmlEncode(rawTitle) : rawTitle;
            return finalTitle;
        }

        /// <summary>
        /// Get the global text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <param name="labels">label and key hashtable from cache</param>
        /// <returns>The global text(label message) according the key.if can't find the key, it should return 'N/A'.</returns>
        private static string GetGlobalTextByKey(string key, Hashtable labels)
        {
            string result = string.Empty;

            if (labels.Contains(key))
            {
                result = labels[key].ToString();
            }

            if (result == key && key.EndsWith("|sub", StringComparison.OrdinalIgnoreCase))
            {
                result = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// Get the global text(label message) by label key.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="key">label key which is unique.</param>
        /// <param name="labels">label and key hashtable from cache</param>
        /// <param name="moduleName">The module name.</param>
        /// <returns>The global text(label message) according the key.if can't find the key, it should return 'N/A'.</returns>
        private static string GetTextByKey(string agencyCode, string key, Hashtable labels, string moduleName)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            string cacheKey = string.Empty;
            string result = string.Empty;

            //get label by agency and apptype
            if (AppSession.GetCapModelFromSession(moduleName) != null)
            {
                CapTypeModel capType = AppSession.GetCapModelFromSession(moduleName).capType;
                if (capType != null)
                {
                    string appType = capType.group + "/" + capType.type + "/" + capType.subType + "/" + capType.category;
                    cacheKey = agencyCode + "_" + appType + "_" + key;

                    if (labels.Contains(cacheKey))
                    {
                        result = labels[cacheKey].ToString();
                    }
                }
            }

            //get label by pageFlow 
            if (string.IsNullOrEmpty(result))
            {
                string pageFlowName = PageFlowUtil.GetPageFlowName();

                if (!string.IsNullOrEmpty(pageFlowName))
                {
                    cacheKey = agencyCode + "_" + ACAConstant.LEVEL_TYPE_PAGEFLOW + "_" + pageFlowName + "_" + key;

                    if (labels.Contains(cacheKey))
                    {
                        result = labels[cacheKey].ToString();
                    }
                }
            }

            cacheKey = agencyCode + "_" + moduleName + "_" + key;

            //get label by agency and module
            if (string.IsNullOrEmpty(result) && labels.Contains(cacheKey))
            {
                result = (string)labels[cacheKey];
            }

            //get label by agency
            if (string.IsNullOrEmpty(result))
            {
                cacheKey = agencyCode + "_" + key;
                if (labels.Contains(cacheKey))
                {
                    result = Convert.ToString(labels[cacheKey]);
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                //get global label
                result = GetGlobalTextByKey(key, labels);
            }

            return result;
        }

        /// <summary>
        /// get hard-coded GUI text key
        /// </summary>
        /// <param name="hardCodedType">The hard-coded type.</param>
        /// <param name="key">The label key.</param>
        /// <returns>The hard-coded GUI text.</returns>
        private static string GetHardCodedGuiTextKey(HardCodedTextType hardCodedType, string key)
        {
            if (null == _hardCodedGuiTextKeyIndexTable || _hardCodedGuiTextKeyIndexTable.Count == 0)
            {
                InitHardCodedGuiTextKeyIndexTable();
            }

            if (_hardCodedGuiTextKeyIndexTable.ContainsKey(hardCodedType))
            {
                Dictionary<string, string> tempIndexTable = _hardCodedGuiTextKeyIndexTable[hardCodedType];
                if (null != tempIndexTable &&
                    tempIndexTable.ContainsKey(key))
                {
                    return tempIndexTable[key];
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// initial hard-coded GUI text key Index table
        /// </summary>
        private static void InitHardCodedGuiTextKeyIndexTable()
        {
            Dictionary<string, string> _capConditionSeverityGuiTextKeyIndexTable = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, string> _inspectionActionGuiTextKeyIndexTable = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, string> _inspectionResultGuiTextKeyIndexTable = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            _capConditionSeverityGuiTextKeyIndexTable.Add("lock", "ACA_CapCondition_Severity_Lock");
            _capConditionSeverityGuiTextKeyIndexTable.Add("hold", "ACA_CapCondition_Severity_Hold");
            _capConditionSeverityGuiTextKeyIndexTable.Add("notice", "ACA_CapCondition_Severity_Notice");
            _capConditionSeverityGuiTextKeyIndexTable.Add("required", "ACA_CapCondition_Severity_Required");
            
            _inspectionActionGuiTextKeyIndexTable.Add("reschedule", "ACA_Inspection_Action_Reschedule");
            _inspectionActionGuiTextKeyIndexTable.Add("schedule", "ACA_Inspection_Action_Schedule");
            _inspectionActionGuiTextKeyIndexTable.Add("cancel", "ACA_Inspection_Action_Cancel");
            _inspectionActionGuiTextKeyIndexTable.Add("cancelled", "ACA_Inspection_Action_Cancelled");
       
            _inspectionResultGuiTextKeyIndexTable.Add("pending", "ACA_Inspection_Result_Pending");
            _inspectionResultGuiTextKeyIndexTable.Add("scheduled", "ACA_Inspection_Result_Scheduled");
   
            //_hardCodedGuiTextKeyIndexTable.Add(HardCodedTextType.OfSmartChoice, _smartChoiceGuiTextKeyIndexTable);
            _hardCodedGuiTextKeyIndexTable.Add(HardCodedTextType.OfCapConditionSeverity, _capConditionSeverityGuiTextKeyIndexTable);
            _hardCodedGuiTextKeyIndexTable.Add(HardCodedTextType.OfInspectionAction, _inspectionActionGuiTextKeyIndexTable);
            _hardCodedGuiTextKeyIndexTable.Add(HardCodedTextType.OfInspectionResult, _inspectionResultGuiTextKeyIndexTable);
        }

        #endregion Methods
    }
}
