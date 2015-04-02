#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GridViewHyperlinkTemplate.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: GridViewHyperlinkTemplate.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Provide hyper link template for grid view 
    /// </summary>
    public class GridViewHyperlinkTemplate : ITemplate
    {
        #region Fields

        /// <summary>
        /// grid view column name
        /// </summary>
        private string _columnName;

        /// <summary>
        /// CSS class for template
        /// </summary>
        private string _cssClass;

        /// <summary>
        /// template type
        /// </summary>
        private DataControlRowType _templateType;

        /// <summary>
        /// text for link
        /// </summary>
        private string _text;

        /// <summary>
        /// url for link
        /// </summary>
        private string _url;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GridViewHyperlinkTemplate class.
        /// </summary>
        /// <param name="type">row type of grid view</param>
        /// <param name="colName">column name</param>
        /// <param name="url">url of the link</param>
        /// <param name="text">text of the link</param>
        /// <param name="cssClass">CSS of the link</param>
        public GridViewHyperlinkTemplate(DataControlRowType type, string colName, string url, string text, string cssClass)
        {
            _templateType = type;
            _columnName = colName;
            _url = url;
            _text = text;
            _cssClass = cssClass;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// InstantiateIn method
        /// </summary>
        /// <param name="container">Control object</param>
        public void InstantiateIn(System.Web.UI.Control container)
        {
            switch (_templateType)
            {
                case DataControlRowType.Header:
                    Literal literal = new Literal();
                    literal.Text = "<b>" + _columnName + "</b>";
                    container.Controls.Add(literal);
                    break;

                case DataControlRowType.DataRow:
                    HyperLink hyperlink = new HyperLink();
                    hyperlink.Target = "_blank";
                    hyperlink.CssClass = _cssClass;
                    hyperlink.DataBinding += new EventHandler(Hyperlink_DataBinding);

                    container.Controls.Add(hyperlink);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Raise data binding event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        private void Hyperlink_DataBinding(object sender, EventArgs e)
        {
            HyperLink hyperlink = (HyperLink)sender;
            GridViewRow row = (GridViewRow)hyperlink.NamingContainer;
            hyperlink.NavigateUrl = DataBinder.Eval(row.DataItem, _url).ToString();
            hyperlink.Text = DataBinder.Eval(row.DataItem, _text).ToString();
        }

        #endregion Methods
    }
}