#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Dialog.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: Dialog.cs 176401 2010-06-25 12:11:30Z ACHIEVO\Grady.lu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web
{
    /// <summary>
    /// Dialog Master Page
    /// </summary>
    public partial class Dialog : System.Web.UI.MasterPage
    {
        /// <summary>
        /// auto height
        /// </summary>
        private bool isAutoHeight = true;

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the inspection title is visible or not
        /// </summary>
        public bool PageTitleVisible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether Auto Height.
        /// </summary>
        public bool IsAutoHeight
        {
            get
            {
                return isAutoHeight;
            }

            set
            {
                isAutoHeight = value;
            }
        }

        /// <summary>
        /// Gets or sets max height;
        /// </summary>
        public string MaxHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets minimized height;
        /// </summary>
        public string MinHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Width
        /// </summary>
        public string FixedWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets TitleKey
        /// </summary>
        public string TitleKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets module name.
        /// </summary>
        public string ModuleName
        {
            get
            {
                IPage iPage = Page as IPage;

                return iPage == null ? string.Empty : iPage.GetModuleName();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Sets the title label key.
        /// </summary>
        /// <param name="newLabelKey">The new label key.</param>
        public void SetTitleLabelKey(string newLabelKey)
        {
            TitleKey = newLabelKey;
            lblPageTitle.LabelKey = newLabelKey;
            divTitle.Visible = true;

            if (!AppSession.IsAdmin)
            {
                // if exists instruction, only show it, NOT show this title because it has render in popup page by javascript.
                if (!string.IsNullOrEmpty(lblPageTitle.GetSubLabel()))
                {
                    divTitle.Visible = true;
                    lblPageTitle.Attributes.Add("class", "ACA_Hide");
                }
                else
                {
                    divTitle.Attributes.Add("class", "ACA_Hide");
                }
            }
        }

        /// <summary>
        /// Set page title visible.
        /// </summary>
        /// <param name="visible">a value indicating whether paget title can display or not.</param>
        public void SetPageTileVisible(bool visible)
        {
            this.lblPageTitle.Visible = visible;
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Resolve the Tab index issue in Opera browser.
                bool isOpera = Request.UserAgent.IndexOf("opera", 0, StringComparison.OrdinalIgnoreCase) != -1;
                hlDialogBegin.Visible = isOpera;
                hlDialogEnd.Visible = isOpera;
            }
        }

        #endregion Methods
    }
}