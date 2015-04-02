#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ConditionsUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ConditionUtil.cs 245866 2013-03-13 03:45:26Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Component;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// The condition utility
    /// </summary>
    public static class ConditionsUtil
    {
        #region Public Methods

        /// <summary>
        /// Build condition.
        /// </summary>
        /// <param name="dt">data table.</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="prefix">the prefix name</param>
        /// <param name="gviewId">GView id</param>
        /// <returns>string builder for condition</returns>
        public static StringBuilder BuildConditionList(DataTable dt, string moduleName, string prefix, string gviewId)
        {
            if (dt == null)
            {
                return null;
            }

            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();
            SimpleViewElementModel4WS[] models = gviewBll.GetSimpleViewElementModel(moduleName, gviewId);
            string prefixName = prefix.Replace("$", string.Empty).Replace("_", string.Empty);

            StringBuilder sb = new StringBuilder();
            string pageIndicator = BasePage.GetStaticTextByKey("ACA_AccelaGridView_PageIndicator");
            string recordCountDesc = string.Format(pageIndicator, "1", dt.Rows.Count, dt.Rows.Count);
            string conditionNameHeader = gviewBll.IsFieldVisible(models, "lnkNameHeader") ? LabelUtil.GetTextByKey("per_conditionList_Label_name", moduleName) : string.Empty;
            string conditionStatusHeader = gviewBll.IsFieldVisible(models, "lnkStatusHeader") ? LabelUtil.GetTextByKey("per_conditionList_Label_status", moduleName) : string.Empty;
            string severityHeader = gviewBll.IsFieldVisible(models, "lnkSeverityHeader") ? LabelUtil.GetTextByKey("per_conditionList_Label_severity", moduleName) : string.Empty;
            string issuedDateHeader = gviewBll.IsFieldVisible(models, "lnkAppliedDateHeader") ? LabelUtil.GetTextByKey("per_conditionList_Label_appliedDate", moduleName) : string.Empty;
            string effectDateHeader = gviewBll.IsFieldVisible(models, "lnkEffectiveDateHeader") ? LabelUtil.GetTextByKey("per_conditionList_Label_effectiveDate", moduleName) : string.Empty;
            string expireDateHeader = gviewBll.IsFieldVisible(models, "lnkExpirationDateHeader") ? LabelUtil.GetTextByKey("per_conditionList_Label_expiredDate", moduleName) : string.Empty;
            sb.Append("<div class='ACA_SmLabel ACA_SmLabel_FontSize'>" + recordCountDesc + "</div>");
            sb.Append("<div><table role='presentation' attr='condition_notice' width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr class='ACA_TabRow_Header font11px'>");
            sb.Append("<th width='3%'></th>");
            sb.Append("<th width='3%'></th>");

            sb.Append("<th class='ACA_AlignLeftOrRightTop' style=\"width:19%\" scope='col'>" + conditionNameHeader + "</th>");

            sb.Append("<th class='ACA_AlignLeftOrRightTop' style=\"width:9%\" scope='col'>" + conditionStatusHeader + "</th>");

            sb.Append("<th class='ACA_AlignLeftOrRightTop' style=\"width:9%\" scope='col'>" + severityHeader + "</th>");

            sb.Append("<th class='ACA_AlignLeftOrRightTop' style=\"width:19%\" scope='col'>" + issuedDateHeader + "</th>");

            sb.Append("<th class='ACA_AlignLeftOrRightTop' style=\"width:19%\" scope='col'>" + effectDateHeader + "</th>");

            sb.Append("<th class='ACA_AlignLeftOrRightTop' style=\"width:19%\" scope='col'>" + expireDateHeader + "</th>");
            sb.Append("</tr>");

            string imgUrl = string.Empty;

            if (dt.Rows.Count > 0)
            {
                int i = 1;
                string commentStyle = string.Empty;

                foreach (DataRow dr in dt.Rows)
                {
                    if (i % 2 == 0)
                    {
                        sb.Append("<tr class='ACA_TabRow_Even ACA_TabRow_Even_FontSize'>");
                        commentStyle = "class='ACA_TabRow_Even ACA_TabRow_Even_FontSize'";
                    }
                    else
                    {
                        sb.Append("<tr class='ACA_TabRow_Odd ACA_TabRow_Odd_FontSize'>");
                        commentStyle = "class='ACA_TabRow_Odd ACA_TabRow_Odd_FontSize'";
                    }

                    string commentId = string.Concat(prefixName, "divComment", dr["RowIndex"].ToString());
                    string imgId = prefixName + "lnkToggledivComment" + dr["RowIndex"].ToString();
                    string lnkId = prefixName + "lnkExpandComment" + dr["RowIndex"].ToString();
                    string altText = LabelUtil.GetTextByKey("img_alt_expand_icon", moduleName);
                    string titleText = string.Format("{0} {1}", altText, LabelUtil.GetTextByKey("ACA_Common_Label_Comment", moduleName));
                    titleText = ScriptFilter.AntiXssHtmlEncode(LabelUtil.RemoveHtmlFormat(titleText));

                    sb.Append("<td style='width:3%;height:19px;' class='ACA_VerticalAlign'>");
                    sb.Append(string.Format("<a id=\"{0}\" href=\"javascript:void(0);\" onclick='javascript:SetNotAsk();expandComment(\"{1}\", \"{2}\",\"{3}\");' class='ACA_Content_Collapse NotShowLoading' title='{4}'>", lnkId, commentId, imgId, lnkId, titleText));
                    sb.Append(string.Format("<img id=\"{0}\" class=\"ACA_NoBorder\" alt=\"{1}\" src=\"{2}\" />", imgId, altText, ImageUtil.GetImageURL("caret_collapsed.gif")));
                    sb.Append("</a></td>");
                    imgUrl = dr["src"].ToString().Replace("~", "..");
                    sb.Append("<td style='width:3%;height:19px;'  class='ACA_VerticalAlign'>");

                    if (dr["defaultImpactCode"].ToString().Equals("Required", StringComparison.InvariantCultureIgnoreCase))
                    {
                        sb.Append("</td>");
                    }
                    else if (!string.IsNullOrEmpty(imgUrl))
                    {
                        sb.Append("<img  id='imgSevrity' src='" + imgUrl + "' alt='" + LabelUtil.GetTextByKey("img_alt_condition_severity", moduleName) + "' /></td>");
                    }
                    else
                    {
                        sb.Append("</td>");
                    }

                    string conditionName = gviewBll.IsFieldVisible(models, "lnkNameHeader") ? dr["objectConditionName"].ToString() : string.Empty;

                    sb.Append("<td class='ACA_AlignLeftOrRightTop' style=\"width:19%\">" + conditionName + "</td>");

                    string condtionStatus = gviewBll.IsFieldVisible(models, "lnkStatusHeader") ? dr["conditionStatus"].ToString() : string.Empty;

                    sb.Append("<td class='ACA_AlignLeftOrRightTop' style=\"width:9%\">" + condtionStatus + "</td>");

                    string impactCode = gviewBll.IsFieldVisible(models, "lnkSeverityHeader") ? dr["impactCode"].ToString() : string.Empty;

                    sb.Append("<td class='ACA_AlignLeftOrRightTop' style=\"width:9%\">" + impactCode + "</td>");

                    string issuedDate = gviewBll.IsFieldVisible(models, "lnkAppliedDateHeader") ? I18nDateTimeUtil.FormatToDateStringForUI(dr["issuedDate"]) : string.Empty;

                    sb.Append("<td class='ACA_AlignLeftOrRightTop' style=\"width:19%\">" + issuedDate + "</td>");

                    string effectDate = gviewBll.IsFieldVisible(models, "lnkEffectiveDateHeader") ? I18nDateTimeUtil.FormatToDateStringForUI(dr["effectDate"]) : string.Empty;

                    sb.Append("<td class='ACA_AlignLeftOrRightTop' style=\"width:19%\">" + effectDate + "</td>");

                    string expireDate = gviewBll.IsFieldVisible(models, "lnkExpirationDateHeader") ? I18nDateTimeUtil.FormatToDateStringForUI(dr["expireDate"]) : string.Empty;

                    sb.Append("<td class='ACA_AlignLeftOrRightTop' style=\"width:19%\">" + expireDate + "</td>");

                    sb.Append("</tr>");

                    sb.Append(BuildConditionComment(dr["RowIndex"].ToString(), dr["conditionDescription"].ToString(), moduleName, commentStyle, prefixName));
                    i++;
                }
            }

            sb.Append("</table></div>");

            return sb;
        }

        /// <summary>
        /// Structure remind message.
        /// </summary>
        /// <param name="hightestConditionModel">high test condition model</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="conditionCollection">condition collection</param>
        /// <param name="conditionType">condition type</param>
        /// <returns>Build Message</returns>
        public static StringBuilder BuildMessage(NoticeConditionModel hightestConditionModel, string moduleName, NoticeConditionModel[] conditionCollection, ConditionType conditionType)
        {
            if (hightestConditionModel == null)
            {
                return null;
            }

            string conditionLock = string.Empty;
            string conditionHold = string.Empty;
            string conditionNotice = string.Empty;

            if (conditionType == ConditionType.Owner)
            {
                conditionLock = LabelUtil.GetTextByKey("per_ownerCondition_locked", moduleName);
                conditionHold = LabelUtil.GetTextByKey("per_ownerCondition_hold", moduleName);
                conditionNotice = LabelUtil.GetTextByKey("per_ownerCondition_notice", moduleName);
            }
            else if (conditionType == ConditionType.Contact)
            {
                conditionLock = LabelUtil.GetTextByKey("per_ContactCondition_locked", moduleName);
                conditionHold = LabelUtil.GetTextByKey("per_ContactCondition_hold", moduleName);
                conditionNotice = LabelUtil.GetTextByKey("per_ContactCondition_notice", moduleName);
            }
            else if (conditionType == ConditionType.License)
            {
                conditionLock = LabelUtil.GetTextByKey("per_LicenseCondition_locked", moduleName);
                conditionHold = LabelUtil.GetTextByKey("per_LicenseCondition_hold", moduleName);
                conditionNotice = LabelUtil.GetTextByKey("per_LicenseCondition_notice", moduleName);
            }
            else if (conditionType == ConditionType.OwnerInContact)
            {
                conditionLock = LabelUtil.GetTextByKey("per_ownerCondition_locked", moduleName);
                conditionHold = LabelUtil.GetTextByKey("per_ownerCondition_hold", moduleName);
                conditionNotice = LabelUtil.GetTextByKey("per_ownerCondition_notice", moduleName);
            }
            else if (conditionType == ConditionType.Address)
            {
                conditionLock = LabelUtil.GetTextByKey("per_addressCondition_locked", moduleName);
                conditionHold = LabelUtil.GetTextByKey("per_addressCondition_hold", moduleName);
                conditionNotice = LabelUtil.GetTextByKey("per_addressCondition_notice", moduleName);
            }
            else if (conditionType == ConditionType.Parcel)
            {
                conditionLock = LabelUtil.GetTextByKey("per_parcel_condition_locked", moduleName);
                conditionHold = LabelUtil.GetTextByKey("per_parcel_condition_hold", moduleName);
                conditionNotice = LabelUtil.GetTextByKey("per_parcel_condition_notice", moduleName);
            }

            string dateDescription = string.Empty;

            if (hightestConditionModel != null && hightestConditionModel.auditDate != null)
            {
                dateDescription = "<span dir=\"ltr\">" + I18nDateTimeUtil.FormatToDateStringForUI(hightestConditionModel.auditDate) + "</span>";
                dateDescription += ".";
            }

            StringBuilder warningInfo = new StringBuilder();

            if (hightestConditionModel != null && !string.IsNullOrEmpty(hightestConditionModel.impactCode))
            {
                //AccelaLabel lblMessage;
                //if (hightestConditionModel.impactCode.ToUpper().Equals(ACAConstant.LOCK_CONDITION))
                warningInfo.Append("<div class=\"ACA_Row\" style=\"height:10px;\"></div>");
                string title = string.Empty;
                string src = string.Empty;

                if (hightestConditionModel.impactCode.Equals(ACAConstant.LOCK_CONDITION, StringComparison.InvariantCultureIgnoreCase))
                {
                    title = LabelUtil.GetTextByKey("aca_condition_notice_locked", moduleName);
                    src = ImageUtil.GetImageURL("locked_24.gif");
                    warningInfo.Append("<div id='ACA_Message' class='ACA_Message_Locked ACA_Message_Locked_FontSize'><div class='ACA_Locked_Icon' title='" + title + "'>");
                    warningInfo.Append("<img class=\"ACA_NoBorder\" alt='" + title + "' src='" + src + "' />");
                    warningInfo.Append("</div>");
                    warningInfo.Append(conditionLock);
                }
                else
                {
                    //ClearViewState();
                    if (hightestConditionModel.impactCode.Equals(ACAConstant.HOLD_CONDITION, StringComparison.InvariantCultureIgnoreCase))
                    {
                        title = LabelUtil.GetTextByKey("aca_condition_notice_hold", moduleName);
                        src = ImageUtil.GetImageURL("hold_24.gif");
                        warningInfo.Append("<div id='ACA_Message' class='ACA_Message_Hold ACA_Message_Hold_FontSize'><div class='ACA_Hold_Icon' title='" + title + "'>");
                        warningInfo.Append("<img class=\"ACA_NoBorder\" alt='" + title + "' src='" + src + "' />");
                        warningInfo.Append("</div>");
                        warningInfo.Append(conditionHold);
                    }
                    else if (hightestConditionModel.impactCode.Equals(ACAConstant.NOTICE_CONDITION, StringComparison.InvariantCultureIgnoreCase))
                    {
                        title = LabelUtil.GetTextByKey("aca_condition_notice_note", moduleName);
                        src = ImageUtil.GetImageURL("note_24.gif");
                        warningInfo.Append("<div id='ACA_Message' class='ACA_Message_Note ACA_Message_Note_FontSize'><div class='ACA_Note_Icon' title='" + title + "'>");
                        warningInfo.Append("<img class=\"ACA_NoBorder\" alt='" + title + "' src='" + src + "' />");
                        warningInfo.Append("</div>");

                        warningInfo.Append(conditionNotice);
                    }
                    else
                    {
                        return null;
                    }
                }

                string conditionDesc = ConditionsUtil.AddSpaceWithFormat(LabelUtil.GetTextByKey("per_condition_description", moduleName));
                string severityDesc = ConditionsUtil.AddSpaceWithFormat(LabelUtil.GetTextByKey("per_severity_descirption", moduleName));

                warningInfo.Append("&nbsp;" + dateDescription);
                warningInfo.Append("<br>");
                warningInfo.Append("<table role='presentation'><tr class='ACA_VerticalAlign'><td style=\"white-space:nowrap;\">");
                warningInfo.Append(conditionDesc);
                warningInfo.Append("</td><td>");
                warningInfo.Append(I18nStringUtil.GetString(hightestConditionModel.resConditionDescription, hightestConditionModel.conditionDescription));
                warningInfo.Append("</td><td>");
                warningInfo.Append(severityDesc);
                warningInfo.Append("</td><td>");
                warningInfo.Append(LabelUtil.GetGuiTextForCapConditionSeverity(hightestConditionModel.impactCode));
                warningInfo.Append("</td></tr>");

                warningInfo.Append("<tr><td colspan='4'>");
                warningInfo.Append(ConditionsUtil.ComposeConditionSummary(conditionCollection));
                warningInfo.Append("</td></tr>");
                warningInfo.Append("</table>");
                warningInfo.Append(GetCommentLink(conditionType, moduleName));
                warningInfo.Append("</div>");
            }

            return warningInfo;
        }

        /// <summary>
        /// Check if the contact condition is locked or not
        /// </summary>
        /// <param name="contactSeqNumber">contact sequence number</param>
        /// <returns>true if locked</returns>
        public static bool IsContactLocked(string contactSeqNumber)
        {
            bool isLocked = false;

            IConditionBll conditionBll = (IConditionBll)ObjectFactory.GetObject(typeof(IConditionBll));
            ConditionNoticeModel notice = conditionBll.GetContactConditionNotices(contactSeqNumber);

            if (notice != null && notice.HighestCondition != null)
            {
                if (ACAConstant.LOCK_CONDITION.Equals(notice.HighestCondition.impactCode, StringComparison.OrdinalIgnoreCase))
                {
                    isLocked = true;
                }
            }

            return isLocked;
        }

        /// <summary>
        /// Judge the condition whether locked.
        /// </summary>
        /// <param name="ownerModel">Owner model</param>
        /// <returns>a boolean value indicated whether the owner is locked</returns>
        public static bool IsOwnerLocked(OwnerModel ownerModel)
        {
            bool isLocked = false;

            if (ownerModel == null || ownerModel.noticeConditions == null
                || ownerModel.noticeConditions.Length == 0)
            {
                return isLocked;
            }

            if (ownerModel.hightestCondition == null || string.IsNullOrEmpty(ownerModel.hightestCondition.impactCode))
            {
                return isLocked;
            }

            NoticeConditionModel hightestConditionModel = ownerModel.hightestCondition;

            if (hightestConditionModel != null && !string.IsNullOrEmpty(hightestConditionModel.impactCode))
            {
                if (hightestConditionModel.impactCode.Equals(ACAConstant.LOCK_CONDITION, StringComparison.InvariantCultureIgnoreCase))
                {
                    isLocked = true;
                }
            }

            return isLocked;
        }

        /// <summary>
        /// Judge the condition whether locked.
        /// </summary>
        /// <param name="licenseModel">The License Model.</param>
        /// <returns>a boolean value indicated whether the license is locked</returns>
        public static bool IsLicenseLocked(LicenseModel4WS licenseModel)
        {
            bool isLocked = false;

            if (licenseModel != null && licenseModel.noticeConditions != null
                && licenseModel.noticeConditions.Length != 0 && licenseModel.hightestCondition != null
                && !string.IsNullOrEmpty(licenseModel.hightestCondition.impactCode))
            {
                NoticeConditionModel hightestConditionModel = licenseModel.hightestCondition;

                if (hightestConditionModel.impactCode.Equals(ACAConstant.LOCK_CONDITION, StringComparison.InvariantCultureIgnoreCase))
                {
                    isLocked = true;
                }
            }

            return isLocked;
        }
        
        /// <summary>
        /// Add a space after a string and limit to layout in one line
        /// </summary>
        /// <param name="content">the content needs to add space.</param>
        /// <returns>the result with append a space.</returns>
        public static string AddSpaceWithFormat(string content)
        {
            content = content == null ? string.Empty : content.Trim();
            StringBuilder result = new StringBuilder();

            result.Append(ACAConstant.HTML_NOBR);
            result.Append(content);
            result.Append(ACAConstant.HTML_NBSP);
            result.Append(ACAConstant.HTML_SLASH_NOBR);

            return result.ToString();
        }

        /// <summary>
        /// Get Condition summary for address, parcel, owner, license and application
        /// </summary>
        /// <param name="conditionArr">NoticeConditionModel array</param>
        /// <returns>Condition summary</returns>
        public static string ComposeConditionSummary(NoticeConditionModel[] conditionArr)
        {
            if (conditionArr == null ||
                conditionArr.Length == 0)
            {
                return string.Empty;
            }

            int total = 0;
            int iLock = 0;
            int iHold = 0;
            int iNotice = 0;
            int iRequired = 0;

            foreach (NoticeConditionModel condition in conditionArr)
            {
                if (condition.impactCode == null ||
                    ValidationUtil.IsNo(condition.displayNoticeOnACA))
                {
                    continue;
                }
                else
                {
                    if (condition.impactCode.Equals(ACAConstant.LOCK_CONDITION, StringComparison.InvariantCultureIgnoreCase))
                    {
                        iLock++;
                    }
                    else if (condition.impactCode.Equals(ACAConstant.HOLD_CONDITION, StringComparison.InvariantCultureIgnoreCase))
                    {
                        iHold++;
                    }
                    else if (condition.impactCode.Equals(ACAConstant.NOTICE_CONDITION, StringComparison.InvariantCultureIgnoreCase))
                    {
                        iNotice++;
                    }
                    else if (condition.impactCode.Equals(ACAConstant.REQUIRED_CONDITION, StringComparison.InvariantCultureIgnoreCase))
                    {
                        iRequired++;
                    }
                    else
                    {
                        continue;
                    }
                }

                total++;
            }

            string totalLabel = AddSpace(LabelUtil.GetTextByKey("aca_condition_total_contitions", string.Empty));
            string lockLabel = AddSpace(LabelUtil.GetTextByKey("aca_condition_lock", string.Empty));
            string holdLabel = AddSpace(LabelUtil.GetTextByKey("aca_condition_hold", string.Empty));
            string noticeLabel = AddSpace(LabelUtil.GetTextByKey("aca_condition_notice", string.Empty));
            string requiredLabel = AddSpace(LabelUtil.GetTextByKey("aca_condition_required", string.Empty));

            string comma = string.Empty;
            StringBuilder buf = new StringBuilder();
            buf.Append("<div>");
            buf.Append(totalLabel);
            buf.Append(total.ToString());
            buf.Append(ACAConstant.HTML_NBSP);
            buf.Append(ACAConstant.HTML_NBSP);
            buf.Append("(");

            if (iLock > 0)
            {
                if (comma == string.Empty)
                {
                    comma = "," + ACAConstant.HTML_NBSP;
                }
                else
                {
                    buf.Append(comma);
                }

                buf.Append(lockLabel);
                buf.Append(iLock.ToString());
            }

            if (iHold > 0)
            {
                if (comma == string.Empty)
                {
                    comma = "," + ACAConstant.HTML_NBSP;
                }
                else
                {
                    buf.Append(comma);
                }

                buf.Append(holdLabel);
                buf.Append(iHold.ToString());
            }

            if (iNotice > 0)
            {
                if (comma == string.Empty)
                {
                    comma = "," + ACAConstant.HTML_NBSP;
                }
                else
                {
                    buf.Append(comma);
                }

                buf.Append(noticeLabel);
                buf.Append(iNotice.ToString());
            }

            if (iRequired > 0)
            {
                if (comma == string.Empty)
                {
                    comma = "," + ACAConstant.HTML_NBSP;
                }
                else
                {
                    buf.Append(comma);
                }

                buf.Append(requiredLabel);
                buf.Append(iRequired.ToString());
            }

            buf.Append(")");
            buf.Append("</div>");

            return buf.ToString();
        }

        /// <summary>
        /// Gets the condition list.
        /// </summary>
        /// <param name="conditions">The condition array.</param>
        /// <param name="isConditionOfApproval">Indicating whether is condition of approval.</param>
        /// <returns>Return the condition list.</returns>
        public static List<ConditionViewModel> GetConditionViewList(NoticeConditionModel[] conditions, bool isConditionOfApproval)
        {
            if (conditions == null)
            {
                return new List<ConditionViewModel>();
            }

            List<ConditionViewModel> result = Convert2ConditionViewModelList(conditions, isConditionOfApproval, string.Empty);
            return result;
        }

        /// <summary>
        /// Gets the condition list.
        /// </summary>
        /// <param name="dictCondition">The condition dictionary.</param>
        /// <param name="isConditionOfApproval">Indicating whether is condition of approval.</param>
        /// <param name="isGroupByRecordType">Indicating whether group by record type.</param>
        /// <returns>Return the condition list.</returns>
        public static List<ConditionViewModel> GetConditionViewList(Dictionary<CapTypeModel, NoticeConditionModel[]> dictCondition, bool isConditionOfApproval, bool isGroupByRecordType)
        {
            List<ConditionViewModel> result = new List<ConditionViewModel>();

            foreach (var dict in dictCondition)
            {
                CapTypeModel capType = dict.Key;
                string capTypeText = CAPHelper.GetAliasOrCapTypeLabel(capType);
                List<ConditionViewModel> conditionViewList = Convert2ConditionViewModelList(dict.Value, isConditionOfApproval, capTypeText);

                result.AddRange(conditionViewList);
            }

            return result;
        }

        /// <summary>
        /// get address condition.
        /// </summary>
        /// <param name="sourceSeqNubmer">the source sequence number.</param>
        /// <param name="refAddressID">the ref address id.</param>
        /// <param name="refAddressUID">the ref address UID.</param>
        /// <param name="moduleName">the module Name.</param>
        /// <returns>address condition string</returns>
        public static StringBuilder GetAddressConditions(string sourceSeqNubmer, string refAddressID, string refAddressUID, string moduleName)
        {
            RefAddressModel refAddressPK = new RefAddressModel();
            refAddressPK.sourceNumber = Convert.ToInt32(sourceSeqNubmer);
            refAddressPK.refAddressId = StringUtil.ToLong(refAddressID);
            refAddressPK.UID = refAddressUID;

            IConditionBll conditionBll = (IConditionBll)ObjectFactory.GetObject(typeof(IConditionBll));
            NoticeConditionModel[] conditions = conditionBll.GetAddressConditions(ConfigManager.AgencyCode, refAddressPK, AppSession.User.PublicUserId);

            DataTable dtAddressCondition = GetConditionDataSource(conditions);
            StringBuilder sb = BuildConditionList(dtAddressCondition, moduleName, "Address", GviewID.AddressConditionList);

            return sb;
        }

        /// <summary>
        /// Get parcel conditions
        /// </summary>
        /// <param name="sourceNumber">Source sequence number</param>
        /// <param name="parcelNumber">Parcel Number</param>
        /// <param name="parcelUID">Parcel external unique id</param>
        /// <param name="moduleName">Module Name</param>
        /// <returns>StringBuilder of Conditions</returns>
        public static StringBuilder GetParcelConditions(string sourceNumber, string parcelNumber, string parcelUID, string moduleName)
        {
            ParcelModel parcelPK = new ParcelModel();
            parcelPK.parcelNumber = parcelNumber;
            parcelPK.UID = parcelUID;
            parcelPK.sourceSeqNumber = StringUtil.ToLong(sourceNumber);

            IConditionBll conditionBll = (IConditionBll)ObjectFactory.GetObject(typeof(IConditionBll));
            NoticeConditionModel[] conditions = conditionBll.GetParcelConditions(ConfigManager.AgencyCode, parcelPK, AppSession.User.PublicUserId);

            DataTable dtParcelCondition = GetConditionDataSource(conditions);
            StringBuilder sb = BuildConditionList(dtParcelCondition, moduleName, "Parcel", GviewID.ParcelConditionList);

            return sb;
        }

        /// <summary>
        /// Get Address Condition Message
        /// </summary>
        /// <param name="sourceSeqNubmer">the source sequence number.</param>
        /// <param name="refAddressID">the reference address id.</param>
        /// <param name="refAddressUID">the reference address UID.</param>
        /// <param name="moduleName">the module name.</param>
        /// <returns>Condition Message of Address</returns>
        public static string GetAddressCondtionMessage(string sourceSeqNubmer, string refAddressID, string refAddressUID, string moduleName)
        {
            RefAddressModel refAddressPK = new RefAddressModel();
            refAddressPK.sourceNumber = Convert.ToInt32(sourceSeqNubmer);
            refAddressPK.refAddressId = StringUtil.ToLong(refAddressID);
            refAddressPK.UID = refAddressUID;

            IConditionBll conditionBll = (IConditionBll)ObjectFactory.GetObject(typeof(IConditionBll));
            NoticeConditionModel[] conditions = conditionBll.GetAddressConditions(ConfigManager.AgencyCode, refAddressPK, AppSession.User.PublicUserId);
            NoticeConditionModel highestCondition = conditionBll.GetHighestCondition(conditions);

            string conditionMessage = string.Empty;

            if ((conditions != null && conditions.Length != 0)
                && (highestCondition != null && !string.IsNullOrEmpty(highestCondition.impactCode)))
            {
                StringBuilder sb = BuildMessage(highestCondition, moduleName, conditions, ConditionType.Address);

                if (sb != null)
                {
                    conditionMessage = sb.ToString();
                }
            }

            return conditionMessage;
        }

        /// <summary>
        /// Get Parcel Condition Message
        /// </summary>
        /// <param name="sourceNumber">Source sequence number</param>
        /// <param name="parcelNumber">Parcel Number</param>
        /// <param name="parcelUID">Parcel external unique id</param>
        /// <param name="moduleName">the module name</param>
        /// <returns>Condition Message of Parcel</returns>
        public static string GetParcelCondtionMessage(string sourceNumber, string parcelNumber, string parcelUID, string moduleName)
        {
            ParcelModel parcelPK = new ParcelModel();
            parcelPK.sourceSeqNumber = StringUtil.ToLong(sourceNumber);
            parcelPK.parcelNumber = parcelNumber;
            parcelPK.UID = parcelUID;

            IConditionBll conditionBll = (IConditionBll)ObjectFactory.GetObject(typeof(IConditionBll));
            NoticeConditionModel[] conditions = conditionBll.GetParcelConditions(ConfigManager.AgencyCode, parcelPK, AppSession.User.PublicUserId);
            NoticeConditionModel highestCondition = conditionBll.GetHighestCondition(conditions);

            string conditionMessage = string.Empty;

            if ((conditions != null && conditions.Length != 0)
                && (highestCondition != null && !string.IsNullOrEmpty(highestCondition.impactCode)))
            {
                StringBuilder sb = BuildMessage(highestCondition, moduleName, conditions, ConditionType.Parcel);

                if (sb != null)
                {
                    conditionMessage = sb.ToString();
                }
            }

            return conditionMessage;
        }

        /// <summary>
        /// get owner condition.
        /// </summary>
        /// <param name="sourceSeqNumber">Source sequence number of APO.</param>
        /// <param name="ownerNumber">owner Number.</param>
        /// <param name="ownerUID">owner UID..</param>
        /// <param name="moduleName">module Name.</param>
        /// <returns>owner condition string</returns>
        public static StringBuilder GetOwnerCondition(string sourceSeqNumber, string ownerNumber, string ownerUID, string moduleName)
        {
            IOwnerBll ownerBll = (IOwnerBll)ObjectFactory.GetObject(typeof(IOwnerBll));
            OwnerModel refOwnerModel = ownerBll.GetOwnerCondition(ConfigManager.AgencyCode, sourceSeqNumber, ownerNumber, ownerUID);

            StringBuilder sb = null;

            if (refOwnerModel != null && refOwnerModel.noticeConditions != null && refOwnerModel.noticeConditions.Length > 0)
            {
                DataTable dtOwnerCondition = GetConditionDataSource(refOwnerModel.noticeConditions);
                sb = ConditionsUtil.BuildConditionList(dtOwnerCondition, moduleName, "Owner", GviewID.OwnerConditionList);
            }

            return sb;
        }

        /// <summary>
        /// Get message according to the owner's condition.
        /// </summary>
        /// <param name="sourceSeqNumber">Source sequence number of APO</param>
        /// <param name="ownerNumer">Owner reference number.</param>
        /// <param name="ownerUID">Owner unique ID. It has value when support XAPO</param>
        /// <param name="moduleName">the module name</param>
        /// <returns>Owner condition message.</returns>
        public static StringBuilder GetOwnerCondtionMessage(string sourceSeqNumber, string ownerNumer, string ownerUID, string moduleName)
        {
            IOwnerBll ownerBll = (IOwnerBll)ObjectFactory.GetObject(typeof(IOwnerBll));
            OwnerModel refOwnerModel = ownerBll.GetOwnerCondition(ConfigManager.AgencyCode, sourceSeqNumber, ownerNumer, ownerUID);

            StringBuilder sb = null;

            if (refOwnerModel != null && refOwnerModel.noticeConditions != null && refOwnerModel.noticeConditions.Length > 0
                && refOwnerModel.hightestCondition != null && !string.IsNullOrEmpty(refOwnerModel.hightestCondition.impactCode))
            {
                sb = ConditionsUtil.BuildMessage(refOwnerModel.hightestCondition, moduleName, refOwnerModel.noticeConditions, ConditionType.Owner);
            }

            return sb;
        }
        
        /// <summary>
        /// Get Condition List
        /// </summary>
        /// <param name="conditions">condition data</param>
        /// <returns>Data table include conditions</returns>
        public static DataTable GetConditionDataSource(NoticeConditionModel[] conditions)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("RowIndex", typeof(int)));
            dt.Columns.Add(new DataColumn("src"));
            dt.Columns.Add(new DataColumn("alt"));
            dt.Columns.Add(new DataColumn("objectConditionName"));
            dt.Columns.Add(new DataColumn("conditionStatus"));
            dt.Columns.Add(new DataColumn("impactCode"));
            dt.Columns.Add(new DataColumn("defaultImpactCode"));
            dt.Columns.Add(new DataColumn("issuedDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("effectDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("expireDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("conditionDescription"));

            if (conditions == null || conditions.Length == 0)
            {
                return dt;
            }

            int rowIndex = 0;
            foreach (NoticeConditionModel condition in conditions)
            {
                // get the src
                string src = string.Empty;
                string alt = string.Empty;

                if (!string.IsNullOrEmpty(condition.impactCode))
                {
                    string impactCode = condition.impactCode.ToUpperInvariant();

                    if (impactCode.Equals(ACAConstant.LOCK_CONDITION, StringComparison.InvariantCulture))
                    {
                        src = ImageUtil.GetImageURL("locked_16_new.gif");
                        alt = LabelUtil.GetGlobalTextByKey("aca_condition_notice_locked");
                    }
                    else if (impactCode.Equals(ACAConstant.HOLD_CONDITION, StringComparison.InvariantCulture))
                    {
                        src = ImageUtil.GetImageURL("hold_16_new.gif");
                        alt = LabelUtil.GetGlobalTextByKey("aca_condition_notice_hold");
                    }
                    else if (impactCode.Equals(ACAConstant.NOTICE_CONDITION, StringComparison.InvariantCulture))
                    {
                        src = ImageUtil.GetImageURL("notice_16_new.gif");
                        alt = LabelUtil.GetGlobalTextByKey("aca_condition_notice_note");
                    }
                    else if (impactCode.Equals(ACAConstant.SERVICE_LOCK_CONDITION, StringComparison.InvariantCulture))
                    {
                        continue;
                    }
                }

                //filter by displayNoticeOnACA option
                if (ValidationUtil.IsNo(condition.displayNoticeOnACA))
                {
                    continue;
                }

                DataRow row = dt.NewRow();
                row["RowIndex"] = rowIndex++;
                row["src"] = src;
                row["alt"] = alt;
                row["objectConditionName"] = I18nStringUtil.GetString(condition.resConditionDescription, condition.conditionDescription);
                row["conditionStatus"] = I18nStringUtil.GetString(GetResConditionStatus(condition.conditionStatus, condition.resConditionStatus), condition.conditionStatus);
                row["impactCode"] = LabelUtil.GetGuiTextForCapConditionSeverity(condition.impactCode);
                row["defaultImpactCode"] = condition.impactCode;
                row["issuedDate"] = condition.issuedDate == null ? DBNull.Value : (object)condition.issuedDate;
                row["effectDate"] = condition.effectDate == null ? DBNull.Value : (object)condition.effectDate;
                row["expireDate"] = condition.expireDate == null ? DBNull.Value : (object)condition.expireDate;
                row["conditionDescription"] = I18nStringUtil.GetString(condition.resConditionComment, condition.conditionComment);

                dt.Rows.Add(row);
            }

            DataView dataView = new DataView(dt);
            dataView.Sort = "issuedDate DESC";
            dt = dataView.ToTable();

            return dt;
        }

        /// <summary>
        /// if the permit condition is locked or hold, return true.
        /// </summary>
        /// <param name="capId">Cap ID Model</param>
        /// <param name="userSeqNumber">User Sequence Number</param>
        /// <returns>is locked or hold</returns>
        public static bool IsConditionLockedOrHold(CapIDModel4WS capId, string userSeqNumber)
        {
            return IsExistConditionBySeverityCode(ACAConstant.LOCK_CONDITION, capId, userSeqNumber)
                    || IsExistConditionBySeverityCode(ACAConstant.HOLD_CONDITION, capId, userSeqNumber);
        }

        /// <summary>
        /// Get filtered cap conditions.
        /// </summary>
        /// <param name="capId">the cap id model</param>
        /// <returns>cap condition model list.</returns>
        public static List<CapConditionModel4WS> GetFilteredCapConditions(CapIDModel4WS capId)
        {
            List<CapConditionModel4WS> filteredCapConditions = null;
            string[] condtionTypes = StandardChoiceUtil.GetConditionTypeFilter();

            if (condtionTypes != null && condtionTypes.Length != 0)
            {
                IConditionBll conditionBll = ObjectFactory.GetObject<IConditionBll>();
                CapConditionModel4WS[] capConditions = conditionBll.GetCapConditions(capId);

                if (capConditions == null || capConditions.Length == 0)
                {
                    return null;
                }

                filteredCapConditions = new List<CapConditionModel4WS>();
                filteredCapConditions.AddRange(capConditions.Where(c => condtionTypes.Contains(c.conditionType)));
            }

            return filteredCapConditions;
        }

        /// <summary>
        /// Gets the condition information by the parameters from other object in <see cref="PeopleModel"/>.
        /// </summary>
        /// <param name="people">A <see cref="PeopleModel"/></param>
        /// <returns>A <see cref="ConditionNoticeModel"/></returns>
        public static ConditionNoticeModel GetConditionInfo(PeopleModel people)
        {
            return GetConditionInfo(people.contactSeqNumber, people.ConditionParameters);
        }

        /// <summary>
        /// Gets the condition information by the parameters from other object in <see cref="PeopleModel"/>.
        /// </summary>
        /// <param name="people">A <see cref="PeopleModel4WS"/></param>
        /// <returns>A <see cref="ConditionNoticeModel"/></returns>
        public static ConditionNoticeModel GetConditionInfo(PeopleModel4WS people)
        {
            return GetConditionInfo(people.contactSeqNumber, people.ConditionParameters);
        }

        /// <summary>
        /// Show conditions for address.
        /// </summary>
        /// <param name="ucConditon">A <see cref="Conditions"/></param>
        /// <param name="refAddressModel">A <see cref="RefAddressModel"/></param>
        /// <returns>true or false. return false if condition is lock, otherwise, return true.</returns>
        public static bool ShowCondition(Conditions ucConditon, RefAddressModel refAddressModel)
        {
            if (refAddressModel == null)
            {
                return true;
            }

            ConditionNoticeModel notice = new ConditionNoticeModel();
            notice.NoticeConditions = refAddressModel.noticeConditions;
            notice.HighestCondition = refAddressModel.hightestCondition;

            return ShowCondition(ucConditon, notice, ConditionType.Address);
        }

        /// <summary>
        /// Show conditions for parcel.
        /// </summary>
        /// <param name="ucConditon">A <see cref="Conditions"/></param>
        /// <param name="parcelModel">A <see cref="ParcelModel"/></param>
        /// <returns>true or false. return false if condition is lock, otherwise, return true.</returns>
        public static bool ShowCondition(Conditions ucConditon, ParcelModel parcelModel)
        {
            if (parcelModel == null)
            {
                return true;
            }

            ConditionNoticeModel notice = new ConditionNoticeModel();
            notice.NoticeConditions = parcelModel.noticeConditions;
            notice.HighestCondition = parcelModel.hightestCondition;

            return ShowCondition(ucConditon, notice, ConditionType.Parcel);
        }

        /// <summary>
        /// if conditionModel is not null, show locked, hold or notice message.
        /// </summary>
        /// <param name="ucConditon">A <see cref="Conditions"/></param>
        /// <param name="people">A <see cref="PeopleModel"/></param>
        /// <returns>true or false.</returns>
        public static bool ShowCondition(Conditions ucConditon, PeopleModel people)
        {
            return ShowCondition(ucConditon, GetConditionInfo(people), ConditionType.Contact);
        }

        /// <summary>
        /// if conditionModel is not null, show locked, hold or notice message.
        /// </summary>
        /// <param name="ucConditon">A <see cref="Conditions"/></param>
        /// <param name="people">A <see cref="PeopleModel4WS"/></param>
        /// <returns>true or false.</returns>
        public static bool ShowCondition(Conditions ucConditon, PeopleModel4WS people)
        {
            return ShowCondition(ucConditon, GetConditionInfo(people), ConditionType.Contact);
        }

        /// <summary>
        /// Indicate license has condition or not.
        /// </summary>
        /// <param name="people">people model</param>
        /// <returns>boolean value</returns>
        public static bool HasContactCondition(PeopleModel4WS people)
        {
            bool hasCondition = true;
            ConditionNoticeModel contactCondition = GetConditionInfo(people);

            if (contactCondition == null
                || contactCondition.NoticeConditions == null
                || contactCondition.NoticeConditions.Length == 0
                || contactCondition.HighestCondition == null
                || string.IsNullOrEmpty(contactCondition.HighestCondition.impactCode))
            {
                hasCondition = false;
            }

            return hasCondition;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// get resource condition status.
        /// </summary>
        /// <param name="conditionStatus">condition status</param>
        /// <param name="resConditionStatus">res condition status</param>
        /// <param name="isConditionOfApproval">Is ConditionOfApproval Flag</param>
        /// <returns>resource condition status.</returns>
        private static string GetResConditionStatus(string conditionStatus, string resConditionStatus, bool isConditionOfApproval = false)
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;

            string statusBizName = isConditionOfApproval ? BizDomainConstant.STD_CAT_CONDITIONS_OF_APPROVAL_STATUS : BizDomainConstant.STD_CAT_CONDITION_STATUS;

            if (ACAConstant.CONDITION_STATUS_APPLIED.Equals(conditionStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                string appliedStatusFromSTD = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, statusBizName, ACAConstant.CONDITION_STATUS_APPLIED);

                if ((!string.IsNullOrEmpty(appliedStatusFromSTD) && appliedStatusFromSTD.Equals(ACAConstant.CONDITION_STATUS_APPLIED, StringComparison.InvariantCultureIgnoreCase)) ||
                    string.IsNullOrEmpty(appliedStatusFromSTD))
                {
                    return LabelUtil.GetTextByKey("ACA_ConditionStatus_Applied", string.Empty);
                }

                return appliedStatusFromSTD;
            }

            if (ACAConstant.CONDITION_STATUS_NOTAPPLIED.Equals(conditionStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                string notAppliedStatusFromSTD = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, statusBizName, ACAConstant.CONDITION_STATUS_NOTAPPLIED);

                if ((!string.IsNullOrEmpty(notAppliedStatusFromSTD) && notAppliedStatusFromSTD.Equals(ACAConstant.CONDITION_STATUS_NOTAPPLIED, StringComparison.InvariantCultureIgnoreCase)) ||
                    string.IsNullOrEmpty(notAppliedStatusFromSTD))
                {
                    return LabelUtil.GetTextByKey("ACA_ConditionStatus_NotApplied", string.Empty);
                }

                return notAppliedStatusFromSTD;
            }

            if (ACAConstant.CONDITION_STATUS_MET.Equals(conditionStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                string metStatusFromSTD = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, statusBizName, ACAConstant.CONDITION_STATUS_MET);

                if ((!string.IsNullOrEmpty(metStatusFromSTD) && metStatusFromSTD.Equals(ACAConstant.CONDITION_STATUS_MET, StringComparison.InvariantCultureIgnoreCase)) ||
                    string.IsNullOrEmpty(metStatusFromSTD))
                {
                    return LabelUtil.GetTextByKey("ACA_ConditionStatus_Met", string.Empty);
                }

                return metStatusFromSTD;
            }

            return resConditionStatus;
        }

        /// <summary>
        /// Structure condition comment.
        /// </summary>
        /// <param name="rowIndex">the row index.</param>
        /// <param name="conditionDescription">condition description</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="commentStyle">comment style.</param>
        /// <param name="prefixName">the prefix name</param>
        /// <returns>Condition Comment</returns>
        private static string BuildConditionComment(string rowIndex, string conditionDescription, string moduleName, string commentStyle, string prefixName)
        {
            string commentId = string.Concat(prefixName, "divComment", rowIndex);

            StringBuilder sb = new StringBuilder();
            sb.Append("<tr " + commentStyle + "><td width='16px'></td><td width='20px'></td><td colspan='6'>");
            sb.Append("<div id='" + commentId + "' style='display:none;width:100%'>");
            sb.Append("<table role='presentation' cellspacing='0' cellpadding='0'><tr><th class='ACA_Comments' scope='row'>");
            sb.Append(LabelUtil.GetTextByKey("ACA_Common_Label_Comment", moduleName) + "&nbsp;&nbsp;");
            sb.Append("</th><td valign='top' height='19px'>");
            sb.Append("<div  id='lblComment'>" + conditionDescription + "</div>");
            sb.Append("</td></tr></table></div></td></tr>");

            return sb.ToString();
        }

        /// <summary>
        /// Get condition comment link
        /// </summary>
        /// <param name="conditionType">condition type</param>
        /// <param name="moduleName">module name</param>
        /// <returns>comment link</returns>
        private static string GetCommentLink(ConditionType conditionType, string moduleName)
        {
            StringBuilder link = new StringBuilder();
            string patt = "<br/><a id=\"{0}\" href=\"javascript:void(0);\" onclick=\"SetNotAsk();{1}(this);\" style=\"cursor:pointer\" class=\"NotShowLoading\">{2}</a>";

            switch (conditionType)
            {
                case ConditionType.Address:
                    string addresslinkId = "lnk" + CommonUtil.GetRandomUniqueID().Substring(0, 8).Replace("-", string.Empty);
                    link.AppendFormat(patt, addresslinkId, "showDetailAddressCondition", LabelUtil.GetTextByKey("per_condition_Label_show", moduleName));
                    break;
                case ConditionType.Parcel:
                    string parcellinkId = "lnk" + CommonUtil.GetRandomUniqueID().Substring(0, 8).Replace("-", string.Empty);
                    link.AppendFormat(patt, parcellinkId, "showDetailParcelCondition", LabelUtil.GetTextByKey("per_condition_Label_show", moduleName));
                    break;
                case ConditionType.Owner:
                    link.AppendFormat(patt, "lnkShowHideOwnerConditionSession", "showDetailOwnerCondition", LabelUtil.GetTextByKey("per_condition_Label_show", moduleName));
                    break;
                case ConditionType.Contact:
                    string contactlinkId = "lnk" + CommonUtil.GetRandomUniqueID().Substring(0, 8).Replace("-", string.Empty);
                    link.AppendFormat(patt, contactlinkId, "showDetailContactCondition", LabelUtil.GetTextByKey("per_condition_Label_show", moduleName));
                    break;
                case ConditionType.License:
                    string licenseLinkId = "lnk" + CommonUtil.GetRandomUniqueID().Substring(0, 8).Replace("-", string.Empty);
                    link.AppendFormat(patt, licenseLinkId, "showDetailLicenseCondition", LabelUtil.GetTextByKey("per_condition_Label_show", moduleName));
                    break;
                case ConditionType.OwnerInContact:
                    string ownerInContactLinkId = "lnk" + CommonUtil.GetRandomUniqueID().Substring(0, 8).Replace("-", string.Empty);
                    link.AppendFormat(patt, ownerInContactLinkId, "showDetailOwnerInContactCondition", LabelUtil.GetTextByKey("per_condition_Label_show", moduleName));
                    break;
            }

            return link.ToString();
        }
        
        /// <summary>
        /// Convert to the ConditionViewModel list.
        /// </summary>
        /// <param name="conditionList">The condition list.</param>
        /// <param name="isConditionOfApproval">Indicating whether is condition of approval.</param>
        /// <param name="recordType">The record type.</param>
        /// <returns>Return the ConditionViewModel list</returns>
        private static List<ConditionViewModel> Convert2ConditionViewModelList(NoticeConditionModel[] conditionList, bool isConditionOfApproval, string recordType)
        {
            if (conditionList == null || conditionList.Length == 0)
            {
                return new List<ConditionViewModel>();
            } 

            List<ConditionViewModel> conditionViewModelList = new List<ConditionViewModel>();
            bool hideRecordType = false;

            var conditionGroups =
                from f in conditionList
                where ValidationUtil.IsYes(f.displayNoticeOnACA) && ValidationUtil.IsYes(f.conditionOfApproval) == isConditionOfApproval
                orderby f.conditionGroup, f.conditionType
                group f by new { f.conditionGroup } into g
                select new { g.Key, g };

            // 1.1 loop by condition group
            foreach (var conditionGroupItem in conditionGroups)
            {
                bool hideConditionGroup = false;

                var conditionTypeGroups =
                    from f in conditionGroupItem.g
                    group f by new { f.conditionType } into g
                    select new { g.Key, g };

                // 1.2 loop by condition type
                foreach (var conditionTypeItem in conditionTypeGroups)
                {
                    List<ConditionViewModel> conditionTypeList = Convert2ConditionViewModelByConditionType(
                        conditionTypeItem.g, recordType, isConditionOfApproval, hideRecordType, hideConditionGroup);

                    conditionViewModelList.AddRange(conditionTypeList);

                    hideRecordType = true;
                    hideConditionGroup = true;
                }
            }

            return conditionViewModelList;
        }

        /// <summary>
        /// Convert to the type of the condition view model by condition.
        /// </summary>
        /// <typeparam name="T">The general type.</typeparam>
        /// <param name="conditionTypeGrouping">The condition type grouping.</param>
        /// <param name="recordType">Type of the record.</param>
        /// <param name="isConditionOfApproval">Indicating whether is condition of approval.</param>
        /// <param name="hideRecordType">if set to <c>true</c> [hide record type].</param>
        /// <param name="hideConditionGroup">if set to <c>true</c> [hide condition group].</param>
        /// <returns>Return the ConditionViewModel list.</returns>
        private static List<ConditionViewModel> Convert2ConditionViewModelByConditionType<T>(IGrouping<T, NoticeConditionModel> conditionTypeGrouping, string recordType, bool isConditionOfApproval, bool hideRecordType, bool hideConditionGroup)
        {
            IBizDomainBll bizDomainBll = ObjectFactory.GetObject<IBizDomainBll>();
            List<ConditionViewModel> result = new List<ConditionViewModel>();

            // get the default applied' value
            string appliedDefaultValue = string.Empty;
            string statusBizName = isConditionOfApproval ? BizDomainConstant.STD_CAT_CONDITIONS_OF_APPROVAL_STATUS : BizDomainConstant.STD_CAT_CONDITION_STATUS;
            IEnumerable<BizDomainModel4WS> statusList = bizDomainBll.GetBizDomainValue(ConfigManager.AgencyCode, statusBizName, new QueryFormat4WS(), false, I18nCultureUtil.UserPreferredCulture);

            if (statusList != null)
            {
                statusList = statusList.OrderBy(p => I18nStringUtil.GetString(p.resBizdomainValue, p.bizdomainValue));

                foreach (BizDomainModel4WS item in statusList)
                {
                    if (item != null && ACAConstant.CONDITION_STATUS_APPLIED.Equals(item.description, StringComparison.InvariantCultureIgnoreCase))
                    {
                        appliedDefaultValue = I18nStringUtil.GetString(item.resBizdomainValue, item.bizdomainValue);
                        break;
                    }
                }
            }

            foreach (var condition in conditionTypeGrouping)
            {
                // set condition applied/not applied status to I18n value.
                string resConditionStatus = GetResConditionStatus(condition.conditionStatus, condition.resConditionStatus, isConditionOfApproval);
                string conditionStatus = I18nStringUtil.GetString(resConditionStatus, condition.conditionStatus);

                // convert the condition model to condition view model for the UI
                ConditionViewModel row = new ConditionViewModel();

                row.ConditionNbr = condition.conditionNumber;
                row.RecordType = recordType;
                row.GroupName = I18nStringUtil.GetString(condition.resConditionGroup, condition.conditionGroup);
                row.ConditionType = I18nStringUtil.GetString(condition.resConditionType, condition.conditionType);
                row.ConditionName = I18nStringUtil.GetString(condition.resConditionDescription, condition.conditionDescription);
                row.AdditionalInformation = ScriptFilter.FilterScript(I18nStringUtil.GetString(condition.resAdditionalInformation, condition.additionalInformation));
                row.Status = !string.IsNullOrEmpty(conditionStatus) ? conditionStatus : appliedDefaultValue;
                row.Severity = LabelUtil.GetGuiTextForCapConditionSeverity(condition.impactCode);
                row.Priority = condition.priority;
                row.PriorityText = bizDomainBll.GetValueForStandardChoice(condition.serviceProviderCode, BizDomainConstant.STD_CONDITION_PRIORITIES, condition.priority.ToString());
                row.ShortComments = ScriptFilter.FilterScript(I18nStringUtil.GetString(condition.resConditionComment, condition.conditionComment));
                row.LongComments = ScriptFilter.FilterScript(I18nStringUtil.GetString(condition.resLongDescripton, condition.longDescripton));
                if (condition.statusByUser != null)
                {
                    if (string.IsNullOrEmpty(condition.statusByUser.dispDeptOfUser))
                    {
                        row.ActionByDept = condition.statusByUser.deptOfUser;
                    }
                    else
                    {
                        row.ActionByDept = condition.statusByUser.dispDeptOfUser;
                    }

                    row.ActionByUser = UserUtil.GetUserName(condition.statusByUser);
                }

                if (condition.issuedByUser != null)
                {
                    if (string.IsNullOrEmpty(condition.issuedByUser.dispDeptOfUser))
                    {
                        row.AppliedByDept = condition.issuedByUser.deptOfUser;
                    }
                    else
                    {
                        row.AppliedByDept = condition.issuedByUser.dispDeptOfUser;
                    }

                    row.AppliedByUser = UserUtil.GetUserName(condition.issuedByUser);
                }

                row.StatusDate = condition.statusDate;
                row.StatusDateString = condition.statusDate != null ? I18nDateTimeUtil.FormatToDateStringForUI(condition.statusDate) : string.Empty;
                row.AppliedDateString = condition.issuedDate != null ? I18nDateTimeUtil.FormatToDateStringForUI(condition.issuedDate) : string.Empty;
                row.EffectiveDateString = condition.effectDate != null ? I18nDateTimeUtil.FormatToDateStringForUI(condition.effectDate) : string.Empty;
                row.ExpirationDateString = condition.expireDate != null ? I18nDateTimeUtil.FormatToDateStringForUI(condition.expireDate) : string.Empty;
                row.IsConditionOfApproval = ValidationUtil.IsYes(condition.conditionOfApproval);
                row.IsApplied = string.IsNullOrEmpty(condition.conditionStatusType) ||
                               condition.conditionStatusType.Equals(ACAConstant.CONDITION_STATUS_APPLIED, StringComparison.InvariantCultureIgnoreCase);
                row.StatusTypeOrder = row.IsApplied ? 1 : 2;
                row.HideRecordType = true;
                row.HideGroup = true;
                row.HideConditionType = true;
                row.ServiceProviderCode = condition.serviceProviderCode;

                // replace with applied date if status date is null
                if (row.StatusDate == null)
                {
                    row.StatusDate = condition.issuedDate;
                    row.StatusDateString = row.AppliedDateString;
                }

                // set the severity order for the orderby statement.
                if (ACAConstant.LOCK_CONDITION.Equals(condition.impactCode, StringComparison.InvariantCultureIgnoreCase))
                {
                    row.SeverityOrder = 1;
                }
                else if (ACAConstant.HOLD_CONDITION.Equals(condition.impactCode, StringComparison.InvariantCultureIgnoreCase))
                {
                    row.SeverityOrder = 2;
                }
                else if (ACAConstant.NOTICE_CONDITION.Equals(condition.impactCode, StringComparison.InvariantCultureIgnoreCase))
                {
                    row.SeverityOrder = 3;
                }
                else if (ACAConstant.REQUIRED_CONDITION.Equals(condition.impactCode, StringComparison.InvariantCultureIgnoreCase))
                {
                    row.SeverityOrder = 4;
                }

                result.Add(row);
            }

            if (result.Count > 0)
            {
                if (!isConditionOfApproval)
                {
                    result = (from f in result
                              orderby f.StatusTypeOrder, f.SeverityOrder, f.StatusDate descending, f.Priority
                              select f).ToList();
                }
                else
                {
                    result = (from f in result
                              orderby f.StatusTypeOrder, f.SeverityOrder, f.Priority, f.StatusDate descending
                              select f).ToList();
                }

                // set the first record to hide record type, condition group, condition type
                result[0].HideRecordType = hideRecordType;
                result[0].HideGroup = hideConditionGroup;
                result[0].HideConditionType = false;
            }

            return result;
        }

        /// <summary>
        /// Check CAP contains specified condition with severity code
        /// </summary>
        /// <param name="severityCode">Severity Code</param>
        /// <param name="capId">Cap ID Model</param>
        /// <param name="userSeqNumber">User Sequence Number</param>
        /// <returns>Contain or not</returns>
        private static bool IsExistConditionBySeverityCode(string severityCode, CapIDModel4WS capId, string userSeqNumber)
        {
            CapWithConditionModel4WS capWithConditionModel = CapUtil.GetCapWithConditionModel4WS(capId, userSeqNumber);

            if (capWithConditionModel != null && capWithConditionModel.conditionModel != null)
            {
                if (severityCode.Equals(capWithConditionModel.conditionModel.impactCode, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Add a space after a string
        /// </summary>
        /// <param name="content">the content needs to add space.</param>
        /// <returns>the result with append a space.</returns>
        private static string AddSpace(string content)
        {
            return content.Trim() + ACAConstant.HTML_NBSP;
        }

        /// <summary>
        /// Show conditions with specified type.
        /// </summary>
        /// <param name="ucConditon">A <see cref="Conditions"/></param>
        /// <param name="notice">A <see cref="ConditionNoticeModel"/></param>
        /// <param name="conditionType">A <see cref="ConditionType"/></param>
        /// <returns>true or false. return false if condition is lock, otherwise, return true.</returns>
        private static bool ShowCondition(Conditions ucConditon, ConditionNoticeModel notice, ConditionType conditionType)
        {
            bool isLocked = false;

            if (notice != null)
            {
                ucConditon.Visible = true;
                isLocked = ucConditon.IsShowCondition(notice.NoticeConditions, notice.HighestCondition, conditionType);
            }
            else
            {
                ucConditon.Visible = false;
            }

            return !isLocked;
        }

        /// <summary>
        /// Gets the condition information by the parameters from other object in <see cref="PeopleModel"/>.
        /// </summary>
        /// <param name="contactSeqNumber">Contact sequence Number</param>
        /// <param name="parameters">A JSON string of <see cref="AutoFillParameter"/> object</param>
        /// <returns>A <see cref="ConditionNoticeModel"/></returns>
        private static ConditionNoticeModel GetConditionInfo(string contactSeqNumber, string parameters)
        {
            ConditionNoticeModel notice = new ConditionNoticeModel();

            if (!string.IsNullOrEmpty(parameters))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                AutoFillParameter parameter = javaScriptSerializer.Deserialize<AutoFillParameter>(parameters);

                switch (parameter.AutoFillType)
                {
                    case ACAConstant.AutoFillType4SpearForm.ContactOwner:
                        //Parameters: "Owner|ownerNumber|sourceSeqNumber|ownerUID".
                        IOwnerBll ownerBll = ObjectFactory.GetObject<IOwnerBll>();
                        OwnerModel ownerModel = ownerBll.GetOwnerCondition(ConfigManager.AgencyCode, parameter.EntityType, parameter.EntityId, parameter.EntityRefId);

                        if (ownerModel != null)
                        {
                            notice.NoticeConditions = ownerModel.noticeConditions;
                            notice.HighestCondition = ownerModel.hightestCondition;
                        }

                        break;
                    case ACAConstant.AutoFillType4SpearForm.License:
                        //Parameters:"License|stateLicense|licenseType|licenseSeqNumber". 
                        ILicenseBLL licenseBll = ObjectFactory.GetObject<ILicenseBLL>();
                        LicenseModel4WS licenseModel = licenseBll.GetLicenseCondition(ConfigManager.AgencyCode, long.Parse(parameter.EntityRefId), AppSession.User.PublicUserId);

                        if (licenseModel != null)
                        {
                            notice.NoticeConditions = licenseModel.noticeConditions;
                            notice.HighestCondition = licenseModel.hightestCondition;
                        }

                        break;
                    default:
                        //Parameters: "Contact|contactSeqNumber|contactType".
                        IConditionBll conditionBll = ObjectFactory.GetObject<IConditionBll>();
                        notice = conditionBll.GetContactConditionNotices(parameter.EntityRefId);
                        break;
                }
            }
            else if (!string.IsNullOrEmpty(contactSeqNumber) && int.Parse(contactSeqNumber) > 0)
            {
                IConditionBll conditionBll = ObjectFactory.GetObject<IConditionBll>();
                notice = conditionBll.GetContactConditionNotices(contactSeqNumber);
            }

            return notice;
        }

        #endregion Private Methods

        /// <summary>
        /// list item variable keys
        /// </summary>
        public struct ListItemVariables
        {
            /// <summary>
            /// Condition Name
            /// </summary>
            public const string ConditionName = "$$ConditionName$$";

            /// <summary>
            /// Condition Status
            /// </summary>
            public const string Status = "$$Status$$";

            /// <summary>
            /// Condition Severity
            /// </summary>
            public const string Severity = "$$Severity$$";

            /// <summary>
            /// Condition Priority
            /// </summary>
            public const string Priority = "$$Priority$$";

            /// <summary>
            /// Condition Short Comments
            /// </summary>
            public const string ShortComments = "$$ShortComments$$";

            /// <summary>
            /// Condition Status Date
            /// </summary>
            public const string StatusDate = "$$StatusDate$$";

            /// <summary>
            /// Condition Applied Date
            /// </summary>
            public const string AppliedDate = "$$AppliedDate$$";

            /// <summary>
            /// Condition Effective Date
            /// </summary>
            public const string EffectiveDate = "$$EffectiveDate$$";

            /// <summary>
            /// Condition Expiration Date
            /// </summary>
            public const string ExpirationDate = "$$ExpirationDate$$";

            /// <summary>
            /// Condition Action By department
            /// </summary>
            public const string ActionByDept = "$$ActionByDept$$";

            /// <summary>
            /// Condition Action By User
            /// </summary>
            public const string ActionByUser = "$$ActionByUser$$";

            /// <summary>
            /// Condition Applied By department
            /// </summary>
            public const string AppliedByDept = "$$AppliedByDept$$";

            /// <summary>
            /// Condition Applied By User
            /// </summary>
            public const string AppliedByUser = "$$AppliedByUser$$";
            
            /// <summary>
            /// Condition Long Comments
            /// </summary>
            public const string LongComments = "$$LongComments$$";
            
            /// <summary>
            /// Condition Long Comments
            /// </summary>
            public const string AdditionalInformation = "$$AdditionalInformation$$";

            /// <summary>
            /// Condition View Detail
            /// </summary>
            public const string ViewDetail = "$$ViewDetails$$";
        }
    }
}
