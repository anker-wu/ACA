#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: BreadCrumpDesigner.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: BreadCrumpDesigner.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.Design.WebControls;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Design bread crump
    /// </summary>
    public class BreadCrumpDesigner : PanelDesigner
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BreadCrumpDesigner class.
        /// </summary>
        public BreadCrumpDesigner()
        {
            this.ReadOnly = true;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// get html in design time
        /// </summary>
        /// <returns>Html for time control</returns>
        public override string GetDesignTimeHtml()
        {
            try
            {
                BreadCrumpToolBar wb = (BreadCrumpToolBar)Component;
                StringWriter sw = new StringWriter();
                HtmlTextWriter tw = new HtmlTextWriter(sw);
                LiteralControl lctl;
                lctl = new LiteralControl("BreadCrumpToolBar");
                lctl.RenderControl(tw);
                return sw.ToString();
            }
            catch (Exception e)
            {
                return GetErrorDesignTimeHtml(e);
            }
        }

        /// <summary>
        ///  throw error message 
        /// </summary>
        /// <param name="e">the thrown exception</param>
        /// <returns>Html for error time control</returns>
        protected override string GetErrorDesignTimeHtml(Exception e)
        {
            string errorstr = "create control error!" + e.Message;
            return CreatePlaceHolderDesignTimeHtml(errorstr);
        }

        #endregion Methods
    }
}