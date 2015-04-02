#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionWizardBasePage.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description: Provide the base page for inspection wizard page inherit.
 *
 *  Notes:
 *      $Id: InspectionWizardBasePage.aspx.cs 170366 2010-09-08 05:34:25Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using System.Web.UI;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Inspection
{
    /// <summary>
    /// This class provide the base page for inspection wizard page inherit.
    /// </summary>
    public class InspectionWizardBasePage : PopupDialogBasePage
    {
        #region Fields
        
        /// <summary>
        /// Restriction option days prior
        /// </summary>
        private const string RESTRICTION_OPTION_DAYSPRIOR = "1";

        /// <summary>
        /// Restriction option hours prior
        /// </summary>
        private const string RESTRICTION_OPTION_HOURSPRIOR = "2";

        /// <summary>
        /// Restriction option days prior at specific time
        /// </summary>
        private const string RESTRICTION_OPTION_DAYSPRIORATTIME = "3";

        /// <summary>
        /// page trace session name.
        /// </summary>
        private const string PAGE_TRACE_SESSION = "PAGE_TRACE_SESSION";

        #endregion Fields

        #region Enum

        /// <summary>
        /// Inspection Wizard Page
        /// </summary>
        [Flags]
        protected enum InspectionWizardPage
        {
            /// <summary>
            /// The None.
            /// </summary>
            None = 0,

            /// <summary>
            /// Inspection Wizard Input Type.
            /// </summary>
            SelectTypes = 1,

            /// <summary>
            /// Inspection Wizard Input DateTime.
            /// </summary>
            DateTime = 2,

            /// <summary>
            /// Inspection Wizard Input Location.
            /// </summary>
            Location = 4,

            /// <summary>
            /// Inspection Wizard InputConfirm.
            /// </summary>
            Confirm = 8
        }

        #endregion
        
        #region Properties

        /// <summary>
        /// Gets or sets the wizard page's parameter.
        /// </summary>
        protected InspectionParameter InspectionWizardParameter
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
        
        #region Methods

        /// <summary>
        /// whether to show optional inspections.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <returns>Indicating whether show inspection optional or not.</returns>
        public static bool IsACAShowInspectionOptional(string moduleName)
        {
            var inspectionPermissionBll = ObjectFactory.GetObject<IInspectionPermissionBll>();
            bool isShowInspectionOptional = inspectionPermissionBll.AllowDisplayOfOptionalInspections(ConfigManager.AgencyCode, moduleName);

            return isShowInspectionOptional;
        }

        /// <summary>
        /// Raises the load event.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            InspectionWizardParameter = BuildParameters();

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
        /// Builds the reschedule policy.
        /// </summary>
        /// <returns>the reschedule policy.</returns>
        protected string BuildRescheduleRestrictionPolicy()
        {
            string restrictionSettings = InspectionWizardParameter.RescheduleRestrictionSettings;
            string actionString = GetTextByKey("per_scheduleinspection_label_reschedule");
            return BuildRestrictionPolicy(restrictionSettings, actionString);
        }

        /// <summary>
        /// Builds the cancellation policy.
        /// </summary>
        /// <returns>the cancellation policy.</returns>
        protected string BuildCancellationRestrictionPolicy()
        {
            string restrictionSettings = InspectionWizardParameter.CancelRestrictionSettings;
            string actionString = GetTextByKey("per_scheduleinspection_label_cancellations");
            return BuildRestrictionPolicy(restrictionSettings, actionString);
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
        /// Get CapID model.
        /// </summary>
        /// <returns>Return the CapID model</returns>
        protected CapIDModel4WS GetCapIDModel()
        {
            CapIDModel4WS capIDModel = new CapIDModel4WS();

            capIDModel.id1 = InspectionWizardParameter.RecordID1;
            capIDModel.id2 = InspectionWizardParameter.RecordID2;
            capIDModel.id3 = InspectionWizardParameter.RecordID3;
            capIDModel.serviceProviderCode = InspectionWizardParameter.AgencyCode;

            return capIDModel;
        }

        /// <summary>
        /// Get Cap model.
        /// </summary>
        /// <returns>Return the Cap model.</returns>
        protected CapModel4WS GetCapModel()
        {
            CapModel4WS capModel = new CapModel4WS();

            CapIDModel4WS capIDModel = GetCapIDModel();

            if (capIDModel != null)
            {
                CapWithConditionModel4WS capWithConditionModel = CapUtil.GetCapWithConditionModel4WS(capIDModel, AppSession.User.UserSeqNum, true);

                if (capWithConditionModel != null)
                {
                    capModel = capWithConditionModel.capModel;
                }
            }

            return capModel;
        }

        /// <summary>
        /// Get the formatted contact content.
        /// </summary>
        /// <param name="parameter">Inspection parameter.</param>
        /// <returns>Return the formatted contact content.</returns>
        protected string GetFormattedContactContent(InspectionParameter parameter)
        {
            string result = string.Empty;
            string name = string.Empty;
            string phone = string.Empty;

            if (!string.IsNullOrEmpty(parameter.ContactFirstName) || !string.IsNullOrEmpty(parameter.ContactMiddleName) || !string.IsNullOrEmpty(parameter.ContactLastName))
            {
                name = UserUtil.FormatToFullName(parameter.ContactFirstName, parameter.ContactMiddleName, parameter.ContactLastName);
            }

            if (!string.IsNullOrEmpty(parameter.ContactPhoneNumber))
            {
                phone = ModelUIFormat.FormatPhoneShow(parameter.ContactPhoneIDD, parameter.ContactPhoneNumber, parameter.ContactCountryCode);
                phone = LabelUtil.RemoveHtmlFormat(phone);
            }

            result = string.Format("<div class='ACA_FLeft'>{0}</div><div class='ACA_FLeft'>&nbsp;{1}</div>", ScriptFilter.AntiXssHtmlEncode(name), phone);

            return result;
        }

        /// <summary>
        /// whether to show the back button or not.
        /// </summary>
        /// <param name="currentWizardPage">the page to show back button.</param>
        /// <returns>Indicating whether show back button or not.</returns>
        protected bool IsShowBack(InspectionWizardPage currentWizardPage)
        {
            // hide the back button if the previous is empty
            string previousURL = InspectionActionViewUtil.GetWizardPreviousURL(InspectionWizardParameter);

            if (!AppSession.IsAdmin && string.IsNullOrEmpty(previousURL))
            {
                return false;
            }

            bool isShowBack = false;
            InspectionWizardPage flowTrace = GetPageTrace();

            switch (currentWizardPage)
            {
                case InspectionWizardPage.SelectTypes:
                    isShowBack = false;
                    break;
                case InspectionWizardPage.DateTime:
                    if ((flowTrace & InspectionWizardPage.SelectTypes) == InspectionWizardPage.SelectTypes)
                    {
                        isShowBack = true;
                    }

                    break;
                case InspectionWizardPage.Location:
                    if ((flowTrace & InspectionWizardPage.SelectTypes) == InspectionWizardPage.SelectTypes
                        || (flowTrace & InspectionWizardPage.DateTime) == InspectionWizardPage.DateTime)
                    {
                        isShowBack = true;
                    }

                    break;
                case InspectionWizardPage.Confirm:
                    isShowBack = true;
                    break;
            }

            return isShowBack;
        }

        /// <summary>
        /// mark the current page.
        /// </summary>
        /// <param name="currentPage">the current page</param>
        /// <param name="isShow">whether the current page show or not</param>
        protected void MarkCurrentPageTrace(InspectionWizardPage currentPage, bool isShow)
        {
            InspectionWizardPage pageTrace = GetPageTrace();

            if (isShow)
            {
                // if the page is show, mark the page as load.
                pageTrace = pageTrace | currentPage;
            }
            else
            {
                // if the page is hide, mark the page as unload.
                pageTrace = pageTrace & ~currentPage;
            }

            // mark the page as unload that after the current page.
            switch (currentPage)
            {
                case InspectionWizardPage.SelectTypes:
                    pageTrace = pageTrace & ~InspectionWizardPage.DateTime & ~InspectionWizardPage.Location & ~InspectionWizardPage.Confirm;
                    break;
                case InspectionWizardPage.DateTime:
                    pageTrace = pageTrace & ~InspectionWizardPage.Location & ~InspectionWizardPage.Confirm;
                    break;
                case InspectionWizardPage.Location:
                    pageTrace = pageTrace & ~InspectionWizardPage.Confirm;
                    break;
                case InspectionWizardPage.Confirm:
                    break;
            }

            HttpContext.Current.Session[PAGE_TRACE_SESSION] = pageTrace;
        }

        /// <summary>
        /// get page trace from session.
        /// </summary>
        /// <returns>The page trace.</returns>
        private InspectionWizardPage GetPageTrace()
        {
            object objPageTrace = HttpContext.Current.Session[PAGE_TRACE_SESSION];

            if (objPageTrace != null)
            {
                return (InspectionWizardPage)objPageTrace;
            }

            return InspectionWizardPage.None;
        }

        /// <summary>
        /// Builds the parameters.
        /// </summary>
        /// <returns>the parameters.</returns>
        private InspectionParameter BuildParameters()
        {
            var result = new InspectionParameter();

            if (!AppSession.IsAdmin)
            {
                result = InspectionParameterUtil.BuildModelFromURL();

                if (!string.IsNullOrEmpty(result.ParameterStoreKey))
                {
                    result = InspectionParameterUtil.GetParameters(result.ParameterStoreKey);
                }
                else if (!string.IsNullOrEmpty(result.ID))
                {
                    int inspectionID = int.Parse(result.ID);
                    var inspectionViewModel = InspectionViewUtil.GetViewModelByIDFromCache(inspectionID, ModuleName);
                    InspectionParameterUtil.UpdateParameters(result, inspectionViewModel);
                }
            }

            return result;
        }

        /// <summary>
        /// Builds the restriction policy.
        /// </summary>
        /// <param name="restrictionSettings">The restriction settings.</param>
        /// <param name="actionString">The action string.</param>
        /// <returns>restriction policy</returns>
        private string BuildRestrictionPolicy(string restrictionSettings, string actionString)
        {
            string result = string.Empty;
            string[] restrictionSettingsArray = null;

            if (!string.IsNullOrEmpty(restrictionSettings))
            {
                restrictionSettingsArray = Server.UrlDecode(restrictionSettings).Split(ACAConstant.SPLIT_CHAR4URL1);
            }

            if (restrictionSettingsArray != null && restrictionSettingsArray.Length >= 4)
            {
                string restrictionOption = restrictionSettingsArray[0];
                string daysPrior = restrictionSettingsArray[1];
                string hoursPrior = restrictionSettingsArray[2];
                string atTime = restrictionSettingsArray[3];

                if (RESTRICTION_OPTION_DAYSPRIOR.Equals(restrictionOption, StringComparison.InvariantCultureIgnoreCase))
                {
                    int days = 0;
                    int.TryParse(daysPrior, out days);
                    if (days > 0)
                    {
                        string restrictionPattern = GetTextByKey("per_scheduleinspection_label_restrictionpolicy1");
                        result = string.Format(restrictionPattern, actionString, daysPrior);
                    }
                }
                else if (RESTRICTION_OPTION_HOURSPRIOR.Equals(restrictionOption, StringComparison.InvariantCultureIgnoreCase))
                {
                    int hours = 0;
                    int.TryParse(hoursPrior, out hours);
                    if (hours > 0)
                    {
                        string restrictionPattern = GetTextByKey("per_scheduleinspection_label_restrictionpolicy2");
                        result = string.Format(restrictionPattern, actionString, hoursPrior);
                    }
                }
                else if (RESTRICTION_OPTION_DAYSPRIORATTIME.Equals(restrictionOption, StringComparison.InvariantCultureIgnoreCase))
                {
                    int days = 0;
                    int.TryParse(daysPrior, out days);
                    atTime = I18nDateTimeUtil.ConvertTimeStringFromWebServiceToUI(atTime, true);

                    if (days > 0)
                    {
                        string restrictionPattern = GetTextByKey("per_scheduleinspection_label_restrictionpolicy3");
                        result = string.Format(restrictionPattern, actionString, atTime, daysPrior);
                    }
                    else
                    {
                        string restrictionPattern = GetTextByKey("per_scheduleinspection_label_restrictionpolicy4");
                        result = string.Format(restrictionPattern, actionString, atTime);
                    }
                }
            }

            return result;
        }

        #endregion Methods
    }
}
