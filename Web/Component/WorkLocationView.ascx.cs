#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: WorkLocationView.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: WorkLocationView.cs 176401 2010-06-25 12:11:30Z ACHIEVO\Grady.lu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.Web.Component
{
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Accela.ACA.Common;
    using Accela.ACA.Common.Util;
    using Accela.ACA.Web.Common;
    using Accela.ACA.Web.Common.Control;
    using Accela.ACA.WSProxy;
    using Accela.Web.Controls;

    /// <summary>
    /// WorkLocationView user control
    /// </summary>
    public partial class WorkLocationView : PermitDetailBaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the hidden sections or not.
        /// </summary>
        public bool IsHidden
        {
            get
            {
                if (ViewState["IsHidden"] != null)
                {
                    return (bool)ViewState["IsHidden"];
                }

                return false;
            }

            set
            {
                ViewState["IsHidden"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is right to left or not(Arabic language is true, others is false).
        /// </summary>
        protected bool IsRightToLeft
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && !IsHidden)
            {
                DisplayWorkLocation();
                IsRightToLeft = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft;
            }
        }

        /// <summary>
        /// Add displayed section into panel.
        /// </summary>
        /// <param name="input">Output string format</param>
        /// <param name="labelkey">Display name</param>
        /// <param name="htc">Callback panel</param>
        /// <param name="isLicenseSection">Judge the license section</param>
        private void AddCtrlToPanel(string input, string labelkey, ref Panel htc, bool isLicenseSection)
        {
            Literal ltStart = new Literal();
            ltStart.Text = @"<h1 style='font-size:1.4em;'>";
            Literal ltEnd = new Literal();
            ltEnd.Text = @"</h1>";
            Panel tc = new Panel();
            AccelaLabel alb = CreateLabel(labelkey);

            tc.Controls.Add(ltStart);
            tc.Controls.Add(alb);
            tc.Controls.Add(ltEnd);

            if (!AppSession.IsAdmin && !string.IsNullOrEmpty(input))
            {
                alb = CreateLabel(string.Empty);
                alb.IsNeedEncode = false;
                alb.Text = input;
                tc.Controls.Add(alb);
            }

            htc = tc;
        }

        /// <summary>
        /// Create a label.
        /// </summary>
        /// <param name="labelKey">Display name</param>
        /// <returns>Return a label</returns>
        private AccelaLabel CreateLabel(string labelKey)
        {
            AccelaLabel alb = ControlBuildHelper.CreateUnitLabel(string.Empty);

            if (labelKey != string.Empty)
            {
                alb.LabelKey = labelKey;
                alb.ID = labelKey + DateTime.Now.Ticks;
            }
            else
            {
                alb.CssClass = "ACA_SmLabel ACA_SmLabel_FontSize";
            }

            return alb;
        }

        /// <summary>
        /// Display cap detail information
        /// </summary>
        private void DisplayWorkLocation()
        {
            string workLocation = string.Empty;
            Panel htc = null;
            HtmlTableRow tr = null;
            int lines = 0;
            string sectionName = CapDetailSectionType.WORKLOCATION.ToString();
            bool showSection = !AppSession.IsAdmin && !IsHidden;
            if (showSection)
            {
                workLocation = ModelUIFormat.FormatAddress4Display(CapModel.addressModels, ModuleName, out lines);
            }

            AddCtrlToPanel(workLocation, "per_permitDetail_label_address", ref htc, false);

            if ((htc != null) || AppSession.IsAdmin)
            {
                if (AppSession.IsAdmin)
                {
                    // create divs and put them into permit detail table for Admin
                    if (TBPermitDetailTest.Rows.Count == 0)
                    {
                        HtmlTableCell cell = new HtmlTableCell();
                        tr = new HtmlTableRow();
                        tr.Cells.Add(cell);
                        TBPermitDetailTest.Rows.Add(tr);
                    }

                    AccelaDiv div = new AccelaDiv();
                    div.Attributes.Add("class", "ACA_FLeft td_parent_left");
                    div.ID = sectionName;
                    div.Controls.Add(htc);
                    TBPermitDetailTest.Rows[0].Cells[0].Controls.Add(div);
                }
                else
                {
                    tr = new HtmlTableRow();
                    tr.Attributes.Add("class", "ACA_FLeft");
                    tr.Attributes.Add("style", "margin-bottom:5px");

                    HtmlTableCell cell = new HtmlTableCell();
                    cell.Controls.Add(htc);
                    tr.Cells.Add(cell);
                    TBPermitDetailTest.Rows.Add(tr);
                }
            }

            TBPermitDetailTest.Style.Add("table-layout", "fixed");
            TBPermitDetailTest.Style.Add("border-collapse", "collapse");
            TBPermitDetailTest.Width = "404px";
        }

        #endregion Methods
    }
}