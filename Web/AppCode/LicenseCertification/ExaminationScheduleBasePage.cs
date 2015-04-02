#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExaminationScheduleBasePage.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ExamScheduleViewModel.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Web.UI;

using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Examination
{
    /// <summary>
    /// Examination Schedule Base Page
    /// </summary>
    public class ExaminationScheduleBasePage : PopupDialogBasePage
    {
        #region Fields

        /// <summary>
        /// examination javascript key
        /// </summary>
        private const string EXAMINATION_JAVASCRIPT_KEY2 = "ExaminationJavaScriptKey2";

        /// <summary>
        /// examination javascript file
        /// </summary>
        private const string EXAMINATION_JAVASCRIPT_FILE2 = "../Scripts/Examination.js";

        #endregion Fields

        #region Perproties

        /// <summary>
        /// Gets or sets the examination wizard parameter.
        /// </summary>
        /// <value>The examination wizard parameter.</value>
        protected ExaminationParameter ExaminationWizardParameter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current agency code.
        /// </summary>
        protected string CurrentAgency
        {
            get
            {
                if (ViewState["CurrentAgency"] == null)
                {
                    ViewState["CurrentAgency"] = CapUtil.GetAgencyCode(ModuleName);
                }

                return ViewState["CurrentAgency"] as string;
            }
        }

        #endregion Properties

        /// <summary>
        /// Raises the load event.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            if (!ClientScript.IsClientScriptIncludeRegistered(this.GetType(), EXAMINATION_JAVASCRIPT_KEY2))
            {
                ClientScript.RegisterClientScriptInclude(this.GetType(), EXAMINATION_JAVASCRIPT_KEY2, EXAMINATION_JAVASCRIPT_FILE2);
            }

            ExaminationWizardParameter = BuildParameters();

            var master = (Dialog)Master;
            master.PageTitleVisible = true;

            base.OnLoad(e);
        }

        /// <summary>
        /// Override the RecordUrl, so that Session[ACAConstant.CURRENT_URL] not set.
        /// </summary>
        protected override void RecordUrl()
        {
        }

        /// <summary>
        /// Change the master page.
        /// </summary>
        protected override void ChangeMasterPage()
        {
            MasterPageFile = ApplicationRoot + "Dialog.Master";
        }

        /// <summary>
        /// Set the wizard's button disable 
        /// </summary>
        /// <param name="clientID">The button's client id</param>
        /// <param name="disable">Disable or not</param>
        protected void SetWizardButtonDisable(string clientID, bool disable)
        {
            string messageKey = "SetWizardButtonDisable" + CommonUtil.GetRandomUniqueID().Substring(0, 6);
            string script = string.Format("SetWizardButtonDisable('{0}',{1});", clientID, disable.ToString().ToLower());

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), messageKey, script, true);
        }

        /// <summary>
        /// Builds the parameters.
        /// </summary>
        /// <returns>Examination Parameter</returns>
        private ExaminationParameter BuildParameters()
        {
            var result = new ExaminationParameter();

            if (!AppSession.IsAdmin)
            {
                result = ExaminationParameterUtil.BuildModelFromURL();

                if (!string.IsNullOrEmpty(result.ParameterStoreKey))
                {
                    result = ExaminationParameterUtil.GetParameters(result.ParameterStoreKey);
                }
            }

            return result;
        }
    }
}