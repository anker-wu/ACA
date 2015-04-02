#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapEditButtonBar.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapEditButtonBar.aspx.cs 247660 2013-04-15 09:38:44Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provides the continue/save/resume button bar.
    /// </summary>
    public partial class PageFlowActionBar : BaseUserControl
    {
        #region Event

        /// <summary>
        /// The continue button's click event.
        /// </summary>
        public event EventHandler ContinueButtonClick;

        /// <summary>
        /// The Pay at counter click.
        /// </summary>
        public event EventHandler PayAtCounterClick;

        #endregion Event

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether from confirm page or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if from confirm page; otherwise, <c>false</c>.
        /// </value>
        public bool IsConfirmPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is4 fee estimator].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is4 fee estimator]; otherwise, <c>false</c>.
        /// </value>
        public bool Is4FeeEstimator { get; set; }

        /// <summary>
        /// Gets the continue client ID.
        /// </summary>
        /// <value>
        /// The continue client ID.
        /// </value>
        public string BtnContinueClientID
        {
            get
            {
                return btnContinue.ClientID;
            }
        }

        /// <summary>
        /// Gets the save button's CSS class
        /// </summary>
        protected string BtnSaveCssClass
        {
            get
            {
                return IsConfirmPage ? "NeedValidate" : "NotShowLoading";
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Disables the button by APO lock.
        /// </summary>
        public void DisableButtonByAPOLock()
        {
            btnContinue.Enabled = false;

            btnSave.Disabled = true;
            btnSave.Style.Add(HtmlTextWriterStyle.Color, "grey");
            btnSave.Attributes.Remove("onclick");
            btnSave.Attributes.Remove("onmouseover");
        }

        /// <summary>
        /// Disable button when save data fail.
        /// </summary>
        public void DisableButtonWhenSaveDataFail()
        {
            btnContinue.Enabled = false;
            lnkPayAtCounter.Enabled = false;
        }

        /// <summary>
        /// Display Pay At Counter Button
        /// </summary>
        public void DisplayPayAtCounterButton()
        {
            liPayAtCounter.Visible = true;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnContinue.AccessKey = AccessibilityUtil.GetAccessKey(AccessKeyType.SubmitForm);

                if ((AppSession.User != null && (AppSession.User.IsAuthorizedAgent || AppSession.User.IsAgentClerk)) || 
                    (!IsConfirmPage && Is4FeeEstimator))
                {
                    divShowSaveandResume.Visible = false;
                }
                else
                {
                    CapUtil.ShowSaveAndResumeLaterButton(divShowSaveandResume, ModuleName);
                }

                if (divShowSaveandResume.Visible)
                {
                    btnSave.Attributes["class"] = BtnSaveCssClass;
                    btnSave.Attributes["title"] = GetTitleByKey("per_permitReg_label_saveAndResume|tip", string.Empty);
                }

                if (AppSession.IsAdmin)
                {
                    btnBackToAssoForm.Visible = false;
                    btnBackToAssoFormAdmin.Visible = true;
                }
                else
                {
                    if (CapUtil.IsAssoFormChild(ModuleName))
                    {
                        liBackToAssoForm.Visible = true;
                        btnBackToAssoForm.HRef = CapUtil.GetAssoFormUrl();
                    }
                    else
                    {
                        liBackToAssoForm.Visible = false;
                    }

                    btnBackToAssoFormAdmin.Visible = false;
                }
            }

            if (IsConfirmPage && !AppSession.User.IsAnonymous && BizDomainConstant.Create_Application_Model.SaveDataInConfirmPageModel.Equals(StandardChoiceUtil.CreateApplicationModel(), StringComparison.OrdinalIgnoreCase))
            {
                btnContinue.LabelKey = "aca_permitconfirm_label_submitapplication";
            }
            else
            {
                btnContinue.LabelKey = CapUtil.GetContinueButtonLabelKey(ModuleName, Is4FeeEstimator ? ACAConstant.COMMON_Y : string.Empty);
            }
        }

        /// <summary>
        /// Handles the Click event of the ContinueToConfirm button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            if (ContinueButtonClick != null)
            {
                ContinueButtonClick(sender, e);
            }
        }

        /// <summary>
        /// Handles the Click event of the PayAtCounter link control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void PayAtCounterLink_Click(object sender, EventArgs e)
        {
            if (PayAtCounterClick != null)
            {
                PayAtCounterClick(sender, e);
            }
        }

        #endregion
    }
}