#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: GridViewHeaderLabel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:This class is used for customizing AccelaGridview control header
 *
 *  Notes:
 * $Id: GridViewHeaderLabel.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Define header label for grid view
    /// </summary>
    public class GridViewHeaderLabel : AccelaLinkButton
    {
        #region Properties

        /// <summary>
        /// Gets or sets sort expression. 
        /// Because we put custom control in GridView header, so we need to add a attribute to store the sort expression
        /// </summary>
        public string SortExpression
        {
            get
            {
                object o = ViewState["SortExpression"];
                string sortExpression = o != null ? o.ToString() : string.Empty;
                return sortExpression;
            }

            set
            {
                ViewState["SortExpression"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Raises the System.Web.UI.Control.Initial event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            CommandName = "Header";
            CommandArgument = SortExpression;

            base.OnInit(e);
        }

        /// <summary>
        /// illustrates the server code that runs in the pre-render phase and injects any needed OnClick handlers.
        /// </summary>
        /// <param name="output">The output.</param>
        protected override void Render(HtmlTextWriter output)
        {
            AccelaGridView grid = FindGridView(this);

            if (grid != null && grid.ShowLoadingPanel)
            {
                output.AddAttribute("onclick", string.Format("showLoadingPanel('{0}');", this.ClientID));
            }

            base.Render(output);
        }

        /// <summary>
        /// Find AccelaGridView control
        /// </summary>
        /// <param name="ctrl">current control</param>
        /// <returns>The instance of accela GridView</returns>
        private AccelaGridView FindGridView(Control ctrl)
        {
            if (ctrl.Parent == null)
            {
                return null;
            }
            else
            {
                if (ctrl is AccelaGridView)
                {
                    return ctrl as AccelaGridView;
                }
                else
                {
                    return FindGridView(ctrl.Parent);
                }
            }
        }

        #endregion Methods
    }
}