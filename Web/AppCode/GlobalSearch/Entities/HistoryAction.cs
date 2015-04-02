#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: HistoryAction.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: HistoryAction.cs 130988 2009-8-20  11:16:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Text;

using Accela.ACA.Common;

namespace Accela.ACA.Web.Common.GlobalSearch
{
    /// <summary>
    /// Action History Item
    /// </summary>
    [Serializable]
    public class HistoryAction
    {
        /// <summary>
        /// max query text for showing
        /// </summary>
        private const int MAX_QUERY_TEXT_FOR_SHOW = 20;

        #region Private Fields

        /// <summary>
        /// The CAP action item detail
        /// </summary>
        private GlobalSearchParameter _capSearchParameter;

        /// <summary>
        /// The License Professional item detail
        /// </summary>
        private GlobalSearchParameter _lpSearchParameter;

        /// <summary>
        /// The APO action item detail
        /// </summary>
        private GlobalSearchParameter _apoSearchParameter;

        /// <summary>
        /// Query text collection
        /// </summary>
        private List<string> _queryTextCollection;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the HistoryAction class.
        /// </summary>
        public HistoryAction()
        {
            this._apoSearchParameter = new GlobalSearchParameter(GlobalSearchType.ADDRESS);
            this._capSearchParameter = new GlobalSearchParameter(GlobalSearchType.CAP);
            this._lpSearchParameter = new GlobalSearchParameter(GlobalSearchType.LP);
            this._queryTextCollection = new List<string>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the CAP action item detail
        /// </summary>
        public GlobalSearchParameter CapSearchParameter
        {
            get { return _capSearchParameter; }
            set { _capSearchParameter = value; }
        }

        /// <summary>
        /// Gets or sets the License Professional action item detail
        /// </summary>
        public GlobalSearchParameter LPSearchParameter
        {
            get { return _lpSearchParameter; }
            set { _lpSearchParameter = value; }
        }

        /// <summary>
        /// Gets or sets the action item detail
        /// </summary>
        public GlobalSearchParameter APOSearchParameter
        {
            get { return _apoSearchParameter; }
            set { _apoSearchParameter = value; }
        }

        /// <summary>
        /// Gets or sets the query text collection
        /// </summary>
        public List<string> QueryTextCollection
        {
            get { return _queryTextCollection; }
            set { _queryTextCollection = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Update query text
        /// </summary>
        /// <param name="queryText">query text</param>
        public void UpdateQueryText(string queryText)
        {
            if (this._queryTextCollection != null)
            {
                for (int i = this._queryTextCollection.Count - 1; i >= 0; i--)
                {
                    if (this._queryTextCollection[i].Equals(queryText, StringComparison.OrdinalIgnoreCase))
                    {
                        this._queryTextCollection.RemoveAt(i);
                        break;
                    }
                }

                // remove the query text if exists or the items' count more than 5
                for (int i = this._queryTextCollection.Count - 1; i >= 4; i--)
                {
                    this._queryTextCollection.RemoveAt(i);
                }
            }
            else
            {
                this._queryTextCollection = new List<string>();
            }

            // insert text to first place
            this._queryTextCollection.Insert(0, queryText);
        }

        /// <summary>
        /// Get last query text
        /// </summary>
        /// <returns>last query text</returns>
        public string GetLastQueryText()
        {
            string queryText = string.Empty;

            if (this._queryTextCollection != null && this._queryTextCollection.Count > 0)
            {
                queryText = this._queryTextCollection[0];
            }

            return queryText;
        }

       /// <summary>
        /// Get query text list
       /// </summary>
       /// <param name="isKey">if used for key</param>
        /// <returns>query text list</returns>
        public string GetQueryTexts(bool isKey)
        {
            StringBuilder list = new StringBuilder();
            list.Append("[");

            if (this._queryTextCollection != null && this._queryTextCollection.Count > 0)
            {
                foreach (string text in this._queryTextCollection)
                {
                    list.Append("[");

                    if (isKey)
                    {
                        string key = GlobalSearchManager.GetJsonString(text);
                        list.Append(key);
                    }
                    else
                    {
                        string html = string.IsNullOrEmpty(text) ? string.Empty : text;

                        if (html.Length > MAX_QUERY_TEXT_FOR_SHOW)
                        {
                            html = string.Concat(html.Substring(0, MAX_QUERY_TEXT_FOR_SHOW), "...");
                        }

                        string queryText = GlobalSearchManager.GetJsonString(html);
                        list.Append(queryText);
                    }
                    
                    list.Append("]");
                    list.Append(ACAConstant.COMMA);
                }

                list.Length--;
            }

            list.Append("]");
            return list.ToString();
        }

        #endregion
    }
}