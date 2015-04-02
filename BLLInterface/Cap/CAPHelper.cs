#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CAPHelper.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2010
 *
 *  Description:
 *
 *  Notes:
 * $Id: CAPHelper.cs 178037 2010-07-30 06:25:12Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Cap
{
    /// <summary>
    /// defines cap utile methods.
    /// </summary>
    public sealed class CAPHelper
    {
        #region Enumerations

        /// <summary>
        /// Cap type options
        /// </summary>
        public enum GetCapTypeOption
        {
            /// <summary>
            /// Only GetMasterLanguage CapType
            /// </summary>
            OnlyGetMasterLanguageCapType,

            /// <summary>
            /// Only GetCurrentLanguageCap Type
            /// </summary>
            OnlyGetCurrentLanguageCapType,

            /// <summary>
            /// Get CurrentOrMasterLanguage CapType
            /// </summary>
            GetCurrentOrMasterLanguageCapType
        }

        #endregion Enumerations

        #region Methods

        /// <summary>
        /// Get cap type label from CapModel4WS
        /// </summary>
        /// <param name="capModel">capModel entity</param>
        /// <returns>Alias value Or CapTypeLabel value</returns>
        public static string GetAliasOrCapTypeLabel(CapModel4WS capModel)
        {
            string result = string.Empty;

            if (null != capModel && null != capModel.capType)
            {
                result = GetAliasOrCapTypeLabel(capModel.capType);
            }

            return result;
        }

        /// <summary>
        /// Get cap type label from CapModel4WS
        /// </summary>
        /// <param name="capModel">capModel entity</param>
        /// <returns>Alias value Or CapTypeLabel value</returns>
        public static string GetAliasOrCapTypeLabel(SimpleCapModel capModel)
        {
            string result = string.Empty;

            if (null != capModel && null != capModel.capType)
            {
                result = GetAliasOrCapTypeLabel(capModel.capType);
            }

            return result;
        }

        /// <summary>
        /// Get cap type label from the sequence:
        /// 1.capTypeModel.resAlias
        /// 2.GetCurrentLanguageCapType
        /// 3.capTypeModel.alias
        /// 4.GetMasterLanguageCapType
        /// </summary>
        /// <param name="capTypeModel">capTypeModel entity</param>
        /// <returns>Alias value Or CapTypeLabel value</returns>
        public static string GetAliasOrCapTypeLabel(CapTypeModel capTypeModel)
        {
            if (null == capTypeModel)
            {
                return string.Empty;
            }

            return I18nStringUtil.GetString(string.Format(@"{0}", capTypeModel.resAlias), GetCapTypeLabel(capTypeModel, GetCapTypeOption.OnlyGetCurrentLanguageCapType), string.Format(@"{0}", capTypeModel.alias), GetCapTypeLabel(capTypeModel, GetCapTypeOption.OnlyGetMasterLanguageCapType));
        }

        /// <summary>
        /// Get cap type label according to the GetCapTypeOption enumeration
        /// </summary>
        /// <param name="capTypeModel">capTypeModel entity</param>
        /// <param name="getCapTypeOption">getCapTypeOption entity</param>
        /// <returns>Cap Type Label</returns>
        public static string GetCapTypeLabel(CapTypeModel capTypeModel, GetCapTypeOption getCapTypeOption)
        {
            if (null == capTypeModel)
            {
                return string.Empty;
            }

            string result = string.Empty;
            string pattern = "{0}{4}{1}{4}{2}{4}{3}";
            bool existsInvalidResourceCapType = string.IsNullOrEmpty(capTypeModel.resGroup) || string.IsNullOrEmpty(capTypeModel.resType) || string.IsNullOrEmpty(capTypeModel.resSubType)
                                                || string.IsNullOrEmpty(capTypeModel.resCategory);

            if (!existsInvalidResourceCapType && (GetCapTypeOption.OnlyGetCurrentLanguageCapType == getCapTypeOption || GetCapTypeOption.GetCurrentOrMasterLanguageCapType == getCapTypeOption))
            {
                result = string.Format(pattern, capTypeModel.resGroup, capTypeModel.resType, capTypeModel.resSubType, capTypeModel.resCategory, ACAConstant.SLASH);
            }

            if (GetCapTypeOption.OnlyGetMasterLanguageCapType == getCapTypeOption || (GetCapTypeOption.GetCurrentOrMasterLanguageCapType == getCapTypeOption && existsInvalidResourceCapType))
            {
                result = string.Format(pattern, capTypeModel.group, capTypeModel.type, capTypeModel.subType, capTypeModel.category, ACAConstant.SLASH);
            }

            return result;
        }

        /// <summary>
        /// Get cap type label,
        /// if no current language cap type, then use master language cap type.
        /// </summary>
        /// <param name="capTypeModel">capTypeModel entity</param>
        /// <returns>Cap Type Label</returns>
        public static string GetCapTypeLabel(CapTypeModel capTypeModel)
        {
            return GetCapTypeLabel(capTypeModel, GetCapTypeOption.GetCurrentOrMasterLanguageCapType);
        }

        /// <summary>
        /// Get cap type value
        /// </summary>
        /// <param name="capTypeModel">capTypeModel entity</param>
        /// <returns>Cap Type Value</returns>
        public static string GetCapTypeValue(CapTypeModel capTypeModel)
        {
            if (null == capTypeModel)
            {
                return string.Empty;
            }

            return string.Format("{0}/{1}/{2}/{3}", capTypeModel.group, capTypeModel.type, capTypeModel.subType, capTypeModel.category);
        }

        /// <summary>
        /// Format cap type value. Use '/f' to split.
        /// </summary>
        /// <param name="capTypeModel">capTypeModel entity</param>
        /// <returns>Cap Type Value</returns>
        public static string FormatCapTypeValue(CapTypeModel capTypeModel)
        {
            if (null == capTypeModel)
            {
                return string.Empty;
            }

            return string.Format("{0}{1}{2}{3}{4}{5}{6}", capTypeModel.group, ACAConstant.SPLIT_CHAR, capTypeModel.type, ACAConstant.SPLIT_CHAR, capTypeModel.subType, ACAConstant.SPLIT_CHAR, capTypeModel.category);
        }

        /// <summary>
        /// Get daily cap type label from CapModel4WS
        /// </summary>
        /// <param name="capModel">CapModel4WS entity</param>
        /// <returns>DailyAlias Or CapTypeLabel</returns>
        public static string GetDailyAliasOrCapTypeLabel(CapModel4WS capModel)
        {
            string result = string.Empty;
            CapTypeModel capTypeModel = capModel == null ? null : capModel.capType;
            string appTypeAlias = capModel == null ? string.Empty : capModel.appTypeAlias;

            if (null != capTypeModel)
            {
                if (I18nCultureUtil.IsInMasterLanguage)
                {
                    if (!string.IsNullOrEmpty(appTypeAlias))
                    {
                        result = appTypeAlias;
                    }
                    else
                    {
                        result = GetCapTypeLabel(capTypeModel, GetCapTypeOption.OnlyGetMasterLanguageCapType);
                    }
                }
                else
                {
                    result = GetCapTypeLabel(capTypeModel);
                }
            }

            return result;
        }

        #endregion Methods
    }
}