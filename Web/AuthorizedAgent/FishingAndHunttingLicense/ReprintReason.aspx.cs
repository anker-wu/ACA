#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ReprintReason.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ReprintReason.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.AuthorizedAgent.FishingAndHunttingLicense
{
    /// <summary>
    /// Reprint Reason
    /// </summary>
    public partial class ReprintReason : PopupDialogBasePage
    {
        /// <summary>
        /// Gets the report has parameters.
        /// </summary>
        /// <value>
        /// The report has parameters.
        /// </value>
        protected string ReportHasParameters
        {
            get
            {
                return Request.QueryString["hasparameters"];
            }
        }

        /// <summary>
        /// Gets the report id.
        /// </summary>
        /// <value>
        /// The report id.
        /// </value>
        protected string ReportId
        {
            get
            {
                return Request.QueryString["reportid"];
            }
        }

        /// <summary>
        /// Gets the last chance.
        /// </summary>
        /// <value>
        /// The last chance.
        /// </value>
        protected string LastChance
        {
            get
            {
                return Request.QueryString["LastChance"];
            }
        }

        /// <summary>
        /// Gets the parent button client id.
        /// </summary>
        /// <value>
        /// The parent button client id.
        /// </value>
        protected string ParentButtonClientId
        {
            get
            {
                return Request.QueryString["ClientId"];
            }
        }

        /// <summary>
        /// Gets the record id.
        /// </summary>
        /// <value>
        /// The record id.
        /// </value>
        protected string RecordId
        {
            get
            {
                return Request.QueryString["RecordId"];
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitleKey("aca_reprintreason_label_title|tip");
            SetPageTitleVisible(false);
           
            if (!IsPostBack)
            {
                DropDownListBindUtil.BindReprintReason(ddlReprintReason);

                if (ValidationUtil.IsTrue(LastChance))
                {
                    MessageUtil.ShowMessageInPopup(Page, MessageType.Notice, GetTextByKey("aca_auth_agent_label_last_print"));
                }
            }
        }
    }
}