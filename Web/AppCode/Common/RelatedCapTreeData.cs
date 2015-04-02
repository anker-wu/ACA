#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RelatedCapTreeData.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  Build Cap tree for related Cap section of Cap detail form.
 *
 *  Notes:
 *      $Id: RelatedCapTreeData.cs 278232 2014-08-29 08:50:29Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// This class provide the related cap tree data.
    /// </summary>
    public class RelatedCapTreeData
    {
        #region Fields

        /// <summary>
        /// The space of the tree level.
        /// </summary>
        private const int TREE_LEVEL_SPACE = 20;

        /// <summary>
        /// The expand url.
        /// </summary>
        private const string ExpandURL = "ACA_CapTree_Expand";

        /// <summary>
        /// The cap tree's tag url.
        /// </summary>
        private static readonly string TagURL = ImageUtil.GetImageURL("tag.png");

        /// <summary>
        /// The caret expand url
        /// </summary>
        private static readonly string CaretExpandURL = ImageUtil.GetImageURL("caret_expanded.gif");

        /// <summary>
        /// The module name
        /// </summary>
        private string treeModuleName;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the cap type list for Authorized Agent or Clerk role.
        /// </summary>
        public IList<string> CapTypeListFilter
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Build Cap tree by generate HTML statement
        /// </summary>
        /// <param name="capList">Tree node model.</param>
        /// <param name="parentPermitNumber">Record number.(format is ID1+ID2+ID3)</param>
        /// <param name="moduleName">Module name. To be used to get the labels.</param>
        /// <param name="isPopup">Indicating whether the tree is displaying in a iFrame.
        /// If <see cref="isPopup"/> is true, the 'View' link will navigate to the parent window.
        /// </param>
        /// <param name="readOnly">if is readonly, hide the view link</param>
        /// <returns>HTML statement string.</returns>
        public string GetBuildHtml(ProjectTreeNodeModel4WS capList, string parentPermitNumber, string moduleName, bool isPopup, bool readOnly)
        {
            StringBuilder sbHtmlTable = new StringBuilder();
            if (capList == null || capList.children == null ||
                capList.children.Length == 0)
            {
                return string.Empty;
            }

            if (capList.children.Length == 1)
            {
                ProjectTreeNodeModel4WS capRelated = capList.children[0];
                if (capRelated.children == null ||
                    capRelated.children.Length == 0)
                {
                    return string.Empty;
                }
            }

            treeModuleName = moduleName;

            DataTable dtHeader = new DataTable();
            dtHeader.Columns.Add(LabelUtil.GetTextByKey("per_permitList_Label_permitNumber", treeModuleName));
            dtHeader.Columns.Add(LabelUtil.GetTextByKey("per_permitList_Label_permitType", treeModuleName));
            dtHeader.Columns.Add(LabelUtil.GetTextByKey("per_permitList_label_permitSearchProjectName", treeModuleName));
            dtHeader.Columns.Add(LabelUtil.GetTextByKey("per_permitList_Label_date", treeModuleName));
            dtHeader.Columns.Add(LabelUtil.GetTextByKey("per_permitList_Label_View", treeModuleName));

            DataTable dtCap = CreateDataSource(capList);

            sbHtmlTable.Append("<div class='related_records'>");
            sbHtmlTable.Append("<table summary='" + LabelUtil.GetTextByKey("aca_relatedcap_summary", treeModuleName) + "' cellspacing=\"0\" border=\"0\" id=\"tableCapTreeList\">\r");
            sbHtmlTable.Append("<caption>").Append(LabelUtil.GetTextByKey("aca_caption_capdetail_relatedrecords", treeModuleName)).Append("</caption>\r");
            sbHtmlTable.Append(BuildHeader(dtHeader));
            sbHtmlTable.Append(BuildBody(dtCap, parentPermitNumber, isPopup, readOnly));
            sbHtmlTable.Append("</table>");
            sbHtmlTable.Append("</div>");

            return sbHtmlTable.ToString();
        }

        /// <summary>
        /// create blank structure for cap list of tree view
        /// </summary>
        /// <returns>blank table for cap list</returns>
        private static DataTable CreateTable()
        {
            DataTable dtCap = new DataTable();
            dtCap.Columns.Add(new DataColumn("CapIndex", typeof(string))); //0
            dtCap.Columns.Add(new DataColumn("Date", typeof(DateTime))); //1
            dtCap.Columns.Add(new DataColumn("PermitNumber", typeof(string))); //2

            dtCap.Columns.Add(new DataColumn("ParentPermitNumber", typeof(string))); //3
            dtCap.Columns.Add(new DataColumn("Indent", typeof(string))); //4
            dtCap.Columns.Add(new DataColumn("IndexCode", typeof(string))); //5
            dtCap.Columns.Add(new DataColumn("ImageURL", typeof(string))); //6

            dtCap.Columns.Add(new DataColumn("PermitType", typeof(string))); //7
            dtCap.Columns.Add(new DataColumn("Description", typeof(string))); //8
            dtCap.Columns.Add(new DataColumn("Status", typeof(string))); //9
            dtCap.Columns.Add(new DataColumn("CapClass", typeof(string))); //10
            dtCap.Columns.Add(new DataColumn("capID1", typeof(string))); //11
            dtCap.Columns.Add(new DataColumn("capID2", typeof(string))); //12
            dtCap.Columns.Add(new DataColumn("capID3", typeof(string))); //13
            dtCap.Columns.Add(new DataColumn("AgencyCode", typeof(string))); //14

            dtCap.Columns.Add(new DataColumn("hasPrivilegeToHandleCap", typeof(string))); //15
            dtCap.Columns.Add(new DataColumn("ApplicationName", typeof(string))); //16
            dtCap.Columns.Add(new DataColumn("accessByACA", typeof(string)));
            dtCap.Columns.Add("PaymentStatus", typeof(string));
            dtCap.Columns.Add("ModuleName", typeof(string));

            return dtCap;
        }

        /// <summary>
        /// Build the tree body.
        /// </summary>
        /// <param name="dtCap">CAP data.</param>
        /// <param name="parentPermitNumber">Record number.(format is ID1+ID2+ID3)</param>
        /// <param name="isPopup">Indicating whether the tree is displaying in a iFrame.
        /// If <see cref="isPopup"/> is true, the 'View' link will navigate to the parent window.
        /// </param>
        /// <param name="readOnly">if is readonly, hide the view link</param>
        /// <returns>A string builder, including the HTML statement.</returns>
        private StringBuilder BuildBody(DataTable dtCap, string parentPermitNumber, bool isPopup, bool readOnly)
        {
            StringBuilder strBody = new StringBuilder();
            string strLink = string.Empty;

            for (int i = 0; i < dtCap.Rows.Count; i++)
            {
                string strCap = dtCap.Rows[i][11].ToString() + dtCap.Rows[i][12].ToString() + dtCap.Rows[i][13].ToString();

                //Highlight for currect permit
                if (parentPermitNumber.Equals(strCap)) 
                {
                    if (i % 2 == 0)
                    {
                        strBody.Append("<tr name=\"" + dtCap.Rows[i][5].ToString() + "\" id=\"" + dtCap.Rows[i][5].ToString() + "\" class=\"ACA_RelatedCap_Highlight font11px\">\r"); //IndexCode
                    }
                    else
                    {
                        strBody.Append("<tr name=\"" + dtCap.Rows[i][5].ToString() + "\" id=\"" + dtCap.Rows[i][5].ToString() + "\" class=\"ACA_BkBlueD ACA_RelatedCap_Highlight font11px\">\r"); //IndexCode
                    }
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        strBody.Append("<tr name=\"" + dtCap.Rows[i][5].ToString() + "\" id=\"" + dtCap.Rows[i][5].ToString() + "\" class=\"ACA_RelatedCap_Normal font11px\">\r"); //IndexCode
                    }
                    else
                    {
                        strBody.Append("<tr name=\"" + dtCap.Rows[i][5].ToString() + "\" id=\"" + dtCap.Rows[i][5].ToString() + "\" class=\"ACA_BkBlueD ACA_RelatedCap_Normal font11px\">\r"); //IndexCode
                    }
                }

                strBody.Append("<td class=\"ACA_AlignLeftOrRight\">\r");
                strBody.Append("<table role='presentation' style=\"width: 100%\" cellpadding=\"0\" cellspacing=\"0\">\r");
                strBody.Append("<tr>\r");
                strBody.Append("<td class=\"ACA_AlignRightOrLeft\" width=\"" + dtCap.Rows[i][4].ToString() + "\">\r"); //Indent

                //ImageUrl
                if (!string.IsNullOrEmpty(dtCap.Rows[i][6].ToString()))
                {
                    string altText = ScriptFilter.AntiXssHtmlEncode(LabelUtil.RemoveHtmlFormat(LabelUtil.GetTextByKey("img_alt_collapse_icon", treeModuleName)));
                    string elementID = dtCap.Rows[i][5].ToString();
                    strBody.Append("<a id=\"lnk" + elementID + "\" href='javascript:void(0);' title=\"" + altText + "\" onclick=\"CapTreeAction('" + elementID + "')\" class=\"NotShowLoading\"><img id=\"img" + elementID + "\" alt=\"" + altText + "\" name=\"img" + elementID + "\" class=\"ACA_CapTree_Expand\"  src=\"" + CaretExpandURL + "\"  /></a>\r");
                }

                strBody.Append("</td>\r");

                strBody.Append("<td width=\"" + TREE_LEVEL_SPACE + "\" class=\"ACA_AlignLeftOrRight\">\r");
                strBody.Append("<img alt=\"" + LabelUtil.GetTextByKey("img_alt_captree_tag", treeModuleName) + "\" src=\"" + TagURL + "\" />");
                strBody.Append("</td>\r");

                strBody.Append("<td class=\"ACA_AlignLeftOrRight\">\r");
                strBody.Append(dtCap.Rows[i][2].ToString() + "\r"); //PermitNumber
                strBody.Append("</td>\r");
                strBody.Append("</tr>\r");
                strBody.Append("</table></td>\r");

                strBody.Append("<td class=\"ACA_AlignLeftOrRight\">");
                strBody.Append(dtCap.Rows[i][7].ToString()); //PermitType
                strBody.Append("</td>");

                strBody.Append("<td class=\"ACA_AlignLeftOrRight\">");
                strBody.Append(dtCap.Rows[i]["ApplicationName"].ToString()); //Application Name
                strBody.Append("</td>");

                strBody.Append("<td class=\"ACA_AlignLeftOrRight\">\r");
                strBody.Append("<div class=\"ACA_NShot\">\r");
                strBody.Append(I18nDateTimeUtil.FormatToDateStringForUI(dtCap.Rows[i][1]) + "\r"); //Date
                strBody.Append("</div></td>\r");

                strBody.Append("<td class=\"ACA_AlignLeftOrRight\">\r");
                strBody.Append("<div class=\"ACA_Shot\">\r");
                strLink = string.Format("../Cap/CapDetail.aspx?Module={0}&capID1={1}&capID2={2}&capID3={3}&{4}={5}", dtCap.Rows[i]["ModuleName"].ToString(), dtCap.Rows[i][11].ToString(), dtCap.Rows[i][12].ToString(), dtCap.Rows[i][13].ToString(), UrlConstant.AgencyCode, dtCap.Rows[i][14].ToString());
                
                // Do not display View link for currect permit / no privilege permits / partial permit / completed paid but convert failed.
                if (!readOnly &&
                    !parentPermitNumber.Equals(strCap) &&
                    !CapUtil.IsPartialCap(dtCap.Rows[i]["CapClass"].ToString()) &&
                    !string.IsNullOrEmpty(dtCap.Rows[i]["hasPrivilegeToHandleCap"].ToString()) &&
                    !ValidationUtil.IsNo(dtCap.Rows[i]["accessByACA"].ToString()) &&
                    !ACAConstant.PAYMENT_STATUS_PAID.Equals(dtCap.Rows[i]["PaymentStatus"]))
                {
                    strBody.Append("<a id='detail' href='" + strLink);

                    if (isPopup)
                    {
                        strBody.Append("' target='_parent");
                    }

                    strBody.Append("'>" + LabelUtil.GetTextByKey("per_permitList_Label_View", treeModuleName) + "</a>"); //Link
                }

                strBody.Append("</div></td>\r");

                strBody.Append("</tr>\r");
            }

            return strBody;
        }

        /// <summary>
        /// Build the header
        /// </summary>
        /// <param name="dtHeader">The header data table.</param>
        /// <returns>The header html.</returns>
        private StringBuilder BuildHeader(DataTable dtHeader)
        {
            StringBuilder strHeader = new StringBuilder();
            strHeader.Append("<tr class=\"ACA_BkTit\">\r");

            for (int i = 0; i < dtHeader.Columns.Count; i++)
            {
                string className = string.Empty;

                switch (i)
                {
                    case 2:
                        className = "ACA_NMLong"; // Project Name
                        break;
                    case 3:
                        className = "ACA_NShot"; // Date
                        break;
                    case 4:
                        className = "ACA_Shot"; // View Link
                        break;
                    default:
                        className = "ACA_MLong";
                        break;
                }

                strHeader.Append("<th class=\"ACA_AlignLeftOrRight\" scope='col'>\r");
                strHeader.Append("<div class=\"Header_h4 " + className + " fontbold\">\r");
                strHeader.Append(dtHeader.Columns[i].ColumnName.Replace('_', ' ') + "\r");
                strHeader.Append("</div></th>\r");
            }

            strHeader.Append("</tr>\r");
            return strHeader;
        }

        /// <summary>
        /// create data table by given cap list of tree view
        /// </summary>
        /// <param name="capList">ProjectTreeNodeModel4WS model tree list</param>
        /// <returns>data source for UI</returns>
        private DataTable CreateDataSource(ProjectTreeNodeModel4WS capList)
        {
            DataTable dtAllTree = CreateTable();

            if (capList.children == null ||
                capList.children.Length == 0)
            {
                return dtAllTree;
            }

            string indexCode = string.Empty;
            string permitNumber = (capList.CAP != null && capList.CAP.altID != null) ? capList.CAP.altID : string.Empty;

            dtAllTree = GetChildCap(dtAllTree, capList, permitNumber, indexCode);

            return dtAllTree;
        }

        /// <summary>
        /// get agency code from cap model.
        /// </summary>
        /// <param name="cap">The cap model.</param>
        /// <returns>The agency code.</returns>
        private string GetAgencyCode(CapModel4WS cap)
        {
            string agencyCode = ACAConstant.AgencyCode;

            if (cap.capID != null &&
                !string.IsNullOrEmpty(cap.capID.serviceProviderCode))
            {
                agencyCode = cap.capID.serviceProviderCode;
            }

            return agencyCode;
        }

        /// <summary>
        /// Create data table by given related cap list of tree view
        /// </summary>
        /// <param name="dtCap">The cap information.</param>
        /// <param name="capTree">ProjectTreeNodeModel4WS model tree list</param>
        /// <param name="parentPermitNumber">The parent permit number.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns>data source for UI</returns>
        private DataTable GetChildCap(DataTable dtCap, ProjectTreeNodeModel4WS capTree, string parentPermitNumber, string prefix)
        {
            if (capTree == null || capTree.children == null ||
                capTree.children.Length == 0)
            {
                return dtCap;
            }

            int index = 1;

            foreach (ProjectTreeNodeModel4WS capChild in capTree.children)
            {
                if (capChild == null)
                {
                    continue;
                }

                CapModel4WS capChildren = capChild.CAP;
                string permitNumber = capChildren.altID ?? string.Empty;
                string permitType = BLL.Cap.CAPHelper.GetAliasOrCapTypeLabel(capChildren);
                string status = I18nStringUtil.GetString(capChildren.resCapStatus, capChildren.capStatus);
                string capStatus = capChildren.capClass ?? string.Empty;

                DataRow drCap = dtCap.NewRow();
                drCap["CapIndex"] = index;
                drCap["PermitNumber"] = permitNumber;
                drCap["PermitType"] = permitType;
                drCap["Status"] = status;
                drCap["CapClass"] = capStatus;
                drCap["capID1"] = capChildren.capID.id1;
                drCap["capID2"] = capChildren.capID.id2;
                drCap["capID3"] = capChildren.capID.id3;
                drCap["AgencyCode"] = GetAgencyCode(capChildren);
                drCap["ParentPermitNumber"] = parentPermitNumber;
                drCap["PaymentStatus"] = capChildren.paymentStatus;

                string privilege = capChildren.hasPrivilegeToHandleCap ? capChildren.hasPrivilegeToHandleCap.ToString() : string.Empty;

                // The Authorized Agent or Clerk role have not the privilege to View the record detail info, which record type beyond the record type filter.
                if ((AppSession.User.IsAuthorizedAgent || AppSession.User.IsAgentClerk) && CapTypeListFilter != null)
                {
                    if (!CapTypeListFilter.Contains(permitType))
                    {
                        privilege = string.Empty;
                    }
                }

                drCap["hasPrivilegeToHandleCap"] = privilege;
                drCap["accessByACA"] = capChildren.accessByACA;
                drCap["Date"] = I18nDateTimeUtil.ParseFromWebService4DataTable(capChildren.fileDate);
                string indexCode = prefix + "_" + string.Format("{0:000}", index);
                drCap["IndexCode"] = indexCode;
                drCap["Indent"] = (indexCode.Length / 3 * TREE_LEVEL_SPACE) + "px";
                drCap["ApplicationName"] = capChildren.specialText;
                dtCap.Rows.Add(drCap);

                if (capChild.children != null && capChild.children.Length > 0)
                {
                    drCap["ImageURL"] = ExpandURL;
                    dtCap = GetChildCap(dtCap, capChild, permitNumber, indexCode);
                }
                else
                {
                    drCap["ImageURL"] = string.Empty;
                }

                drCap["ModuleName"] = capChildren.moduleName;

                index++;
            }

            return dtCap;
        }

        #endregion Methods
    }
}
