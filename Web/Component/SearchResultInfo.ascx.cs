#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SearchResultInfo.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: SearchResultInfo.ascx.cs 257891 2014-07-16 09:56:18Z ACHIEVO\canon.wu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// The search result info summary control
    /// </summary>
    public partial class SearchResultInfo : BaseUserControl
    {
        /// <summary>
        /// Gets or sets the label key for the search result count number summary.
        /// </summary>
        public string CountSummaryLabelKey 
        { 
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label key for the search result click prompt info.
        /// </summary>
        public string PromptLabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Display the count number summary info.
        /// </summary>
        /// <param name="count">The amount of search result</param>
        public void Display(int count)
        {
            string countSummary = count > 0 ? count.ToString() : string.Empty;
            Display(countSummary);
        }

        /// <summary>
        /// Display the count number summary info.
        /// </summary>
        /// <param name="countSummary">The amount of search result</param>
        public void Display(string countSummary)
        {
            if (string.IsNullOrEmpty(countSummary))
            {
                divSearchResultList.Visible = true;
                divSearchResultCount.Visible = false;

                noDataMessageForSearchResultList.Show(MessageType.Notice, "per_permitList_Error_noResult", MessageSeperationType.NoOne);
            }
            else
            {
                string countNumSummary = DataUtil.StringFormat(LabelUtil.GetTextByKey(CountSummaryLabelKey, string.Empty), countSummary);
                string clickPromptInfo = LabelUtil.GetTextByKey(PromptLabelKey, string.Empty);

                noDataMessageForSearchResultList.Hide();
                divSearchResultList.Visible = true;
                divSearchResultCount.Visible = true;
                lblSearchResultCountNumSummary.Text = countNumSummary;
                lblClickPromptSearchResult.Text = clickPromptInfo;

                updatePanel.Update();
            }
        }

        /// <summary>
        /// Hide search result information summary.
        /// </summary>
        public void Hide()
        {
            noDataMessageForSearchResultList.Hide();
            divSearchResultList.Visible = false;
            divSearchResultCount.Visible = false;
        }
    }
}