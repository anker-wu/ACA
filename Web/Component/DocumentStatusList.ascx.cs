#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DocumentStatusList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: DocumentStatusList.ascx.cs 276239 2014-08-04 03:13:27Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for DocumentStatusList
    /// </summary>
    public partial class DocumentStatusList : BaseUserControl
    {
        #region private
        
        /// <summary>
        /// Template document status list.
        /// </summary>
        private const string TEMPLATE_DOCUMENT_STATUS_LIST =
            @"<table role='presentation' class='document_status_list'>            
            $$Loop$$
                <tr><td colspan='2'>
                    <div $$DocNameStyle$$>
                        <a id='lnkDocumentStatusExpandComment$$RowIndex$$$$ClientID$$' href='#' onclick='ExpandDocumentStatusComment($$RowIndex$$)'
                            class='ACA_Content_Collapse NotShowLoading' title='$$DocNameTitle$$'>
                            <img id='lnkDocumentStatusToggledivComment$$RowIndex$$$$ClientID$$'
                                class='ACA_NoBorder' alt='$$ExpandIconName$$' src='$$ExpandIconUrl$$' />
                        </a>
                    </div>
                    <span class='document_status_documentname'>$$DocName$$</span>
                </td></tr>
                <tr><td>
                    <div id='divDocumentStatusComment$$RowIndex$$$$ClientID$$' style='display:none'>
                        <div class='document_status_left'>
                            <span>$$ReviewStatus$$</span>
                        </div>
                    </div>
                </td></tr>
            $$/Loop$$
            </table>";

        #endregion

        #region Public Methods

        /// <summary>
        /// Load document status list.
        /// </summary>
        /// <param name="clientId">The control client id.</param>
        /// <param name="moduleName">module name.</param>
        /// <returns>document status list</returns>
        public static string LoadDocStatuses(string clientId, string moduleName)
        {
            DataTable dtDocumentList = HttpContext.Current.Session[SessionConstant.DOCUMENT_STATUS_LIST] as DataTable;

            if (dtDocumentList == null || dtDocumentList.Rows.Count == 0)
            {
                return string.Empty;
            }

            Regex reg = new Regex(@"(?<=(\$\$Loop\$\$))[.\s\S]*?(?=(\$\$/Loop\$\$))", RegexOptions.Multiline | RegexOptions.Singleline);
            string bodyTemplateStr = reg.Match(TEMPLATE_DOCUMENT_STATUS_LIST).Value;

            if (string.IsNullOrWhiteSpace(bodyTemplateStr))
            {
                return string.Empty;
            }

            int rowIndex = 0;
            string bodyStr = string.Empty;

            foreach (DataRow row in dtDocumentList.Rows)
            {
                bodyStr += bodyTemplateStr;
                bodyStr = bodyStr.Replace("$$RowIndex$$", (rowIndex++).ToString());
                bodyStr = bodyStr.Replace("$$ClientID$$", clientId);
                bodyStr = ReplaceNameAndStatus(row, bodyStr, moduleName);
            }

            string tableStr = reg.Replace(TEMPLATE_DOCUMENT_STATUS_LIST, string.Empty);
            tableStr = tableStr.Replace("$$Loop$$$$/Loop$$", bodyStr);

            return tableStr;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Replace in the template content field.
        /// </summary>
        /// <param name="row">Data row</param>
        /// <param name="templateStr">Template string.</param>
        /// <param name="moduleName">module name.</param>
        /// <returns>Template content after replace.</returns>
        private static string ReplaceNameAndStatus(DataRow row, string templateStr, string moduleName)
        {
            if (row == null)
            {
                templateStr = templateStr.Replace("$$DocName$$", string.Empty);
                templateStr = templateStr.Replace("$$ReviewStatus$$", string.Empty);
                templateStr = templateStr.Replace("$$DocNameStyle$$", "class='ACA_Hide'");
                templateStr = templateStr.Replace("$$DocNameTitle$$", string.Empty);
                templateStr = templateStr.Replace("$$ExpandIconName$$", string.Empty);
                templateStr = templateStr.Replace("$$ExpandIconUrl$$", string.Empty);

                return templateStr;
            }

            string[] statusList = row[ColumnConstant.Attachment.ReviewStatus.ToString()] as string[];

            if (statusList != null)
            {
                string status = string.Empty;
                string name = row[ColumnConstant.Attachment.Name.ToString()].ToString();
                foreach (string s in statusList)
                {
                    status += s + ACAConstant.HTML_BR;
                }

                templateStr = templateStr.Replace("$$DocName$$", name);
                templateStr = templateStr.Replace("$$ReviewStatus$$", status);
                templateStr = templateStr.Replace("$$DocNameStyle$$", "class='ACA_Show'");
                templateStr = templateStr.Replace("$$DocNameTitle$$", LabelUtil.GetTitleByKey("img_alt_expand_icon", "ACA_Common_Label_Comment", true, moduleName));
                templateStr = templateStr.Replace("$$ExpandIconName$$", LabelUtil.GetTextByKey("img_alt_expand_icon", moduleName));
                templateStr = templateStr.Replace("$$ExpandIconUrl$$", ImageUtil.GetImageURL("caret_collapsed.gif"));
            }
            else
            {
                templateStr = templateStr.Replace("$$DocName$$", string.Empty);
                templateStr = templateStr.Replace("$$ReviewStatus$$", string.Empty);
                templateStr = templateStr.Replace("$$DocNameStyle$$", "class='ACA_Hide'");
                templateStr = templateStr.Replace("$$DocNameTitle$$", string.Empty);
                templateStr = templateStr.Replace("$$ExpandIconName$$", string.Empty);
                templateStr = templateStr.Replace("$$ExpandIconUrl$$", string.Empty);
            }

            return templateStr;
        }

        #endregion
    }
}