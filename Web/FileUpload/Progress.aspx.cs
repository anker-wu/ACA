#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Progress.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: Progress.aspx.cs 277932 2014-08-22 10:29:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Globalization;
using System.Threading;
using System.Web.UI.WebControls;

using Accela.ACA.Common.Util;
using Accela.ACA.Web;

/// <summary>
/// The file upload progress.
/// </summary>
public partial class FileUpload_Progress : Brettle.Web.NeatUpload.ProgressPage
{
    #region Properties

    /// <summary>
    /// Gets or sets Style Sheet Theme.
    /// </summary>
    public override string StyleSheetTheme
    {
        get
        {
            return I18nCultureUtil.UserPreferredCulture;
        }

        set
        {
            base.StyleSheetTheme = value;
        }
    }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Initialize Culture.
    /// </summary>
    protected override void InitializeCulture()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(I18nCultureUtil.UserPreferredCulture);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(I18nCultureUtil.UserPreferredCulture);
        base.InitializeCulture();
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
            if (FractionComplete > 0 && TimeRemaining.TotalSeconds < 1)
            {
                FractionComplete = 1;
                barDetailsDiv.Width = Unit.Percentage(100);
            }

            lblNormalInProgress.Text = string.Format(
                                                    "<table role='presentation' class='StatusMessage'><tr><td>{0} of {1}{2}</td><td>{3}</td><td>-</td><td>{4}</td></tr></table>",
                                                    string.Format("{0:0%}", FractionComplete),
                                                    FormatCount(BytesTotal),
                                                    CountUnits,
                                                    string.Format(BasePage.GetStaticTextByKey("ACA_FileUploadProcess_NormalInProgress_At"), string.Empty, FormatRate(BytesPerSec)),
                                                    string.Format(BasePage.GetStaticTextByKey("ACA_FileUploadProcess_NormalInProgress_Left"), FormatTimeSpan(TimeRemaining)));

            lblChunkedInProgress.Text = string.Format(
                                                    "<table role='presentation' class='StatusMessage'><tr><td>{0}/{1}</td><td>{2}</td><td>-</td><td>{3}</td></tr></table>",
                                                    FormatCount(BytesRead),
                                                    CountUnits,
                                                    string.Format(BasePage.GetStaticTextByKey("ACA_FileUploadProcess_ChunkedInProgress_At"), FormatRate(BytesPerSec)),
                                                    string.Format(BasePage.GetStaticTextByKey("ACA_FileUploadProcess_ChunkedInProgress_Elapsed"), FormatTimeSpan(TimeElapsed)));
            lblRejected.Text = string.Format(
                                            "<table role='presentation' class='StatusMessage'><tr><td>{0}</td><td>{1}</td></tr></table>",
                                            BasePage.GetStaticTextByKey("ACA_FileUploadProcess_Rejected"),
                                            Rejection != null ? GetLabelText(Rejection.Message) : string.Empty);

            lblError.Text = string.Format(
                                        "<table role='presentation' class='StatusMessage'><tr><td>{0}</td><td>{1}</td></tr></table>",
                                        BasePage.GetStaticTextByKey("ACA_FileUploadProcess_Error"),
                                        Failure != null ? GetLabelText(Failure.Message) : string.Empty);

            lblSub.Text = BasePage.GetStaticTextByKey("ACA_FileUploadProcess_Submit");

            if (Failure != null && !string.IsNullOrEmpty(Failure.Message))
            {
                hfCallbackFunc.Value = string.Format("Callback('{0}','{1}','{2}');", GetLabelText(Failure.Message), true, "AfterAttachmentUpload");
            }

            if (Rejection != null && !string.IsNullOrEmpty(Rejection.Message))
            {
                hfCallbackFunc.Value = string.Format("Callback('{0}','{1}','{2}');", GetLabelText(Rejection.Message), true, "AfterAttachmentUpload");
            } 
        }
    }

    /// <summary>
    /// Get real label text
    /// </summary>
    /// <param name="label">the label</param>
    /// <returns>label text</returns>
    private string GetLabelText(string label)
    {
        string labelText = string.Empty;
        string labelKey = string.Empty;
        string[] labelValues = null;

        // Step-1: Check if label contains values
        if (label.Contains("$$"))
        {
            int index = label.IndexOf("$$");
            labelKey = label.Substring(0, index);

            // 2 means the length of $$
            labelValues = label.Substring(index + 2).Split(new string[] { "$$" }, StringSplitOptions.None);
        }
        else
        {
            labelKey = label;
        }

        // Step-2: Get Label Text
        labelText = BasePage.GetStaticTextByKey(labelKey);

        // Step-3: Format Label Text with values
        if (string.IsNullOrEmpty(labelText))
        {
            // This is not a label key.
            labelText = label;
        }
        else
        {
            // Format Label Text with values
            labelText = string.Format(labelText, labelValues);
        }

        return labelText; 
    }

    #endregion Methods
}
