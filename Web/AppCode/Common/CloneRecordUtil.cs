#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CloneRecordUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CloneRecordUtil.cs 154424 2010-11-03 07:23:01Z ACHIEVO\kale.huang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;

using Accela.ACA.BLL;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// a utility class for clone record.
    /// </summary>
    public static class CloneRecordUtil
    {
        /// <summary>
        /// Indicate displaying clone record button or not.
        /// </summary>
        /// <param name="capType">it is cap type.</param>
        /// <param name="capID">cap id model</param>
        /// <param name="moduleName">module name.</param>
        /// <param name="fullPrivileges">judge record type accessible and record lock or not.</param>
        /// <returns>true or false.</returns>
        public static bool IsDisplayCloneButton(CapTypeModel capType, CapIDModel capID, string moduleName, bool fullPrivileges)
        {
            bool displayCloneButton = false;

            displayCloneButton = StandardChoiceUtil.IsEnableCloneRecord(moduleName);

            if (displayCloneButton)
            {
                List<LinkItem> links = TabUtil.GetCreationLinkItemList(moduleName, false);
                displayCloneButton = (links != null && links.Count > 0) ? true : false;
            }

            if (displayCloneButton && fullPrivileges)
            {
                displayCloneButton = IsCapTypeAccessible(capType, moduleName);

                if (displayCloneButton)
                {
                    IConditionBll conditionBll = (IConditionBll)ObjectFactory.GetObject(typeof(IConditionBll));
                    bool isRecordLocked = conditionBll.IsCapLocked(capID);
                    displayCloneButton = !isRecordLocked;
                }
            }

            return displayCloneButton;
        }

        /// <summary>
        /// Clone a record which show in permit list.
        /// </summary>
        /// <param name="page">the page control.</param>
        /// <param name="selectedCaps">the selected Caps.</param>
        /// <param name="currentModuleName">module Name use for get label key</param>
        /// <returns>Indicating it is clone success or not.</returns>
        public static bool ClonePermitListRecord(Page page, DataTable selectedCaps, string currentModuleName)
        {
            bool cloneSuccess = true;

            if (selectedCaps == null || selectedCaps.Rows.Count <= 0)
            {
                MessageUtil.ShowMessageByControl(page, MessageType.Notice, LabelUtil.GetTextByKey("aca_capclone_msg_selectonerecord", currentModuleName));
                cloneSuccess = false;
                return cloneSuccess;
            }

            if (selectedCaps.Rows.Count > 1)
            {
                MessageUtil.ShowMessageByControl(page, MessageType.Notice, LabelUtil.GetTextByKey("aca_capclone_msg_cloneonerecord", currentModuleName));
                cloneSuccess = false;
                return cloneSuccess;
            }

            string capModuleName = selectedCaps.Rows[0]["ModuleName"].ToString();
            CapIDModel capID = new CapIDModel();
            capID.serviceProviderCode = selectedCaps.Rows[0]["agencyCode"].ToString();
            capID.ID1 = selectedCaps.Rows[0]["capID1"].ToString();
            capID.ID2 = selectedCaps.Rows[0]["capID2"].ToString();
            capID.ID3 = selectedCaps.Rows[0]["capID3"].ToString();

            bool isSuperCap = CapUtil.IsSuperCAP(TempModelConvert.Add4WSForCapIDModel(capID));

            //Super Partial cap will not support clone.
            if (isSuperCap)
            {
                MessageUtil.ShowMessageByControl(page, MessageType.Notice, LabelUtil.GetTextByKey("aca_capclone_msg_no_super_partial_record", currentModuleName));
                cloneSuccess = false;
                return cloneSuccess;
            }

            ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
            CapTypeModel capType = capTypeBll.GetCapTypeByCapID(TempModelConvert.Add4WSForCapIDModel(capID));

            if (!StandardChoiceUtil.IsEnableCloneRecord(capModuleName) || !IsCapTypeAccessible(capType, capModuleName))
            {
                MessageUtil.ShowMessageByControl(page, MessageType.Notice, LabelUtil.GetTextByKey("aca_capclone_msg_no_creation_permission", currentModuleName));
                cloneSuccess = false;
                return cloneSuccess;
            }

            IConditionBll conditionBll = (IConditionBll)ObjectFactory.GetObject(typeof(IConditionBll));
            bool isRecordLocked = conditionBll.IsCapLocked(capID);

            if (isRecordLocked)
            {
                MessageUtil.ShowMessageByControl(page, MessageType.Notice, LabelUtil.GetTextByKey("aca_capclone_msg_recordlock", currentModuleName));
                cloneSuccess = false;
                return cloneSuccess;
            }

            if (!IsAnonymousCloneRecordAllowed(capType))
            {
                MessageUtil.ShowMessageByControl(page, MessageType.Notice, LabelUtil.GetTextByKey("acc_login_label_forceLoginNote", currentModuleName));
                cloneSuccess = false;
                return cloneSuccess;
            }
            
            string cloneUrl = BuildClonePageUrl(capID, capModuleName);
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "ShowCloneDialogInPermitList", "ShowCloneDialog('" + cloneUrl + "');", true);
            return cloneSuccess;
        }

        /// <summary>
        /// build a clone record page's url.
        /// </summary>
        /// <param name="capId">cap id model.</param>
        /// <param name="moduleName">module name current cap belong to.</param>
        /// <returns>url string.</returns>
        public static string BuildClonePageUrl(CapIDModel capId, string moduleName)
        {
            if (capId == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(FileUtil.AppendApplicationRoot("Cap/CapClone.aspx"));
            sb.Append("?");
            sb.Append(ACAConstant.MODULE_NAME).Append("=").Append(moduleName);
            sb.Append("&").Append(ACAConstant.CAP_ID_1).Append("=").Append(capId.ID1);
            sb.Append("&").Append(ACAConstant.CAP_ID_2).Append("=").Append(capId.ID2);
            sb.Append("&").Append(ACAConstant.CAP_ID_3).Append("=").Append(capId.ID3);
            sb.Append("&").Append(UrlConstant.AgencyCode).Append("=").Append(capId.serviceProviderCode);

            return sb.ToString();
        }

        /// <summary>
        /// Is Clone record.
        /// </summary>
        /// <param name="request">the request object</param>
        /// <returns>true or false.</returns>
        public static bool IsCloneRecord(HttpRequest request)
        {
            string cloningRecordParam = request.QueryString[ACAConstant.IS_CLONE_RECORD] == null ? string.Empty : request.QueryString[ACAConstant.IS_CLONE_RECORD].ToString();
            bool isCloningRecord = ACAConstant.COMMON_TRUE.Equals(cloningRecordParam);
            return isCloningRecord;
        }

        /// <summary>
        /// Mapping the clone record component to cap detail section.
        /// </summary>
        /// <param name="cloneRecordComponent">clone record component</param>
        /// <returns>cap detail section</returns>
        public static string Mapping2CapDetailSection(CloneRecordComponent cloneRecordComponent)
        {
            string result = string.Empty;

            switch (cloneRecordComponent)
            {
                case CloneRecordComponent.cloneAddressList:
                    result = CapDetailSectionType.WORKLOCATION.ToString();
                    break;
                case CloneRecordComponent.cloneProfessional:
                    result = CapDetailSectionType.LICENSED_PROFESSIONAL.ToString();
                    break;
                case CloneRecordComponent.cloneDetailInformation:
                    result = CapDetailSectionType.PROJECT_DESCRIPTION.ToString();
                    break;
                case CloneRecordComponent.cloneOwnerList:
                    result = CapDetailSectionType.OWNER.ToString();
                    break;
                case CloneRecordComponent.cloneContacts:
                    result = CapDetailSectionType.RELATED_CONTACTS.ToString();
                    break;
                case CloneRecordComponent.cloneAdditionInfo:
                    result = CapDetailSectionType.ADDITIONAL_INFORMATION.ToString();
                    break;
                case CloneRecordComponent.cloneAppSpecificInfo:
                    result = CapDetailSectionType.APPLICATION_INFORMATION.ToString();
                    break;
                case CloneRecordComponent.cloneAppSpecificInfoTable:
                    result = CapDetailSectionType.APPLICATION_INFORMATION_TABLE.ToString();
                    break;
                case CloneRecordComponent.cloneParcelList:
                    result = CapDetailSectionType.PARCEL_INFORMATION.ToString();
                    break;
                case CloneRecordComponent.cloneEducation:
                    result = CapDetailSectionType.EDUCATION.ToString();
                    break;
                case CloneRecordComponent.cloneContEducation:
                    result = CapDetailSectionType.CONTINUING_EDUCATION.ToString();
                    break;
                case CloneRecordComponent.cloneExamination:
                    result = CapDetailSectionType.EXAMINATION.ToString();
                    break;
                case CloneRecordComponent.cloneValuationCalculator:
                    result = CapDetailSectionType.VALUATION_CALCULATOR.ToString();
                    break;
                case CloneRecordComponent.cloneAssetList:
                    result = CapDetailSectionType.ASSETS.ToString();
                    break;
            }

            return result;
        }

        /// <summary>
        /// Is anonymous clone record allowed
        /// </summary>
        /// <param name="capType">The cap type</param>
        /// <returns>return true of false</returns>
        public static bool IsAnonymousCloneRecordAllowed(CapTypeModel capType)
        {
            if (AppSession.User == null || AppSession.User.IsAnonymous)
            {
                if (!FunctionTable.IsEnableCreateApplication())
                {
                    return false;
                }

                ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();
                CapTypeModel newCapType = capTypeBll.GetCapTypeByPK(capType);

                // Judge if the cap type has changed
                if (newCapType != null && ValidationUtil.IsNo(newCapType.anonymousCreateAllowed))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// indicate public user can create current cap type record or not.
        /// </summary>
        /// <param name="capType">The cap type.</param>
        /// <param name="moduleName">The module name.</param>
        /// <returns>true or false.</returns>
        private static bool IsCapTypeAccessible(CapTypeModel capType, string moduleName)
        {
            ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
            CapTypeDetailModel capTypeDetail = capTypeBll.GetCapTypeByPK(capType);

            if (ValidationUtil.IsYes(capTypeDetail.asChildOnly))
            {
                return false;
            }

            bool isCapTypeAccessible = false;

            if (!string.Equals(capType.serviceProviderCode, ConfigManager.SuperAgencyCode, StringComparison.OrdinalIgnoreCase))
            {
                if (IsCapTypeDisabledForACA(capTypeDetail))
                {
                    return false;
                }

                //CapType.auditStatus is null in Cap detail page.
                if (string.IsNullOrEmpty(capType.auditStatus))
                {
                    if (capTypeDetail != null && ACAConstant.VALID_STATUS.Equals(capTypeDetail.auditStatus))
                    {
                        isCapTypeAccessible = true;
                    }
                }
                else if (ACAConstant.VALID_STATUS.Equals(capType.auditStatus, StringComparison.OrdinalIgnoreCase))
                {
                    isCapTypeAccessible = true;
                }

                return isCapTypeAccessible;
            }

            List<LinkItem> links = TabUtil.GetCreationLinkItemList(moduleName, false);

            if (links != null && links.Count > 0)
            {
                ICapTypeFilterBll capTypFiltereBll = (ICapTypeFilterBll)ObjectFactory.GetObject(typeof(ICapTypeFilterBll));
                string filterName = capTypFiltereBll.GetCapTypeFilterByLabelKey(ConfigManager.AgencyCode, links[0].Module, links[0].Label);
                
                isCapTypeAccessible = capTypeBll.IsCapTypeAccessible(capType, moduleName, filterName, ACAConstant.VCH_TYPE_VHAPP, AppSession.User.PublicUserId);
            }

            return isCapTypeAccessible;
        }

        /// <summary>
        /// Is the cap type disabled for ACA?
        /// </summary>
        /// <param name="capTypeDetail">The cap type detail model.</param>
        /// <returns>Indicate the cap type disabled for ACA.</returns>
        private static bool IsCapTypeDisabledForACA(CapTypeDetailModel capTypeDetail)
        {
            if (capTypeDetail == null)
            {
                return true;
            }

            bool isCapTypeDisabledForACA = false;
            string udCode3 = capTypeDetail.udcode3;

            /*
             * NA - Disable CAP type in ACA by check box style.
             * N/A - Disable CAP type in ACA by radio box style.
             * APNANANA - Disable CAP type in ACA by unknown style.
             */
            if (string.IsNullOrEmpty(udCode3) || "NA".Equals(udCode3, StringComparison.OrdinalIgnoreCase) || "N/A".Equals(udCode3, StringComparison.OrdinalIgnoreCase)
                        || "APNANANA".Equals(udCode3, StringComparison.OrdinalIgnoreCase))
            {
                isCapTypeDisabledForACA = true;
            }

            return isCapTypeDisabledForACA;
        }
    }
}
