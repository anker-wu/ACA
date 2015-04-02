#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: UserLicenseList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: UserLicenseList.ascx.cs 278033 2014-08-26 07:06:09Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation UserLicenseView.
    /// </summary>
    public partial class UserLicenseList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// The edit state.
        /// </summary>
        private string _editState;

        #endregion Fields

        #region Events

        /// <summary>
        /// Grid view page index changing event.
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        /// <summary>
        /// Grid view sorted event.
        /// </summary>
        public event GridViewSortedEventHandler GridViewSort;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets control states which includes View or Edit to control the remove button whether requires or not.
        /// </summary>
        public string EditState
        {
            get
            {
                return string.IsNullOrEmpty(_editState) ? "View" : _editState;
            }

            set
            {
                _editState = value;
            }
        }

        /// <summary>
        /// Gets or sets licenses list.
        /// </summary>
        private List<ContractorLicenseModel4WS> GridViewDataSource
        {
            get
            {
                if (ViewState["GridViewDataSource"] == null)
                {
                    return new List<ContractorLicenseModel4WS>();
                }

                return ViewState["GridViewDataSource"] as List<ContractorLicenseModel4WS>;
            }

            set
            {
                ViewState["GridViewDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets user sequence number.
        /// </summary>
        private string UserSeqNum
        {
            get
            {
                return ViewState["User_Seq_Num"].ToString();
            }

            set
            {
                ViewState["User_Seq_Num"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Display license list for current user.
        /// </summary>
        /// <param name="userSeqNum">User sequence number.</param>
        public void DisplayLicenseList(string userSeqNum)
        {
            if (AppSession.IsAdmin)
            {
                GridViewDataSource = null;
                BindLicenseList();
                return;
            }

            // stores the userSeqNum to be used when remove click event raise.
            UserSeqNum = userSeqNum;
            ContractorLicenseModel4WS[] contractorLicenses = LicenseUtil.GetContractorLicenseByUserSeqNumber(ConfigManager.AgencyCode, userSeqNum);

            GridViewDataSource = contractorLicenses != null ? contractorLicenses.ToList() : null;
            BindLicenseList();
        }

        /// <summary>
        /// Initial event handler.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV() && EditState == "EDIT")
                {
                    gdvLicenseList.ShowExportLink = true;
                    gdvLicenseList.ExportFileName = "UserLicenseList";
                }
                else
                {
                    gdvLicenseList.ShowExportLink = false;
                }
            }

            GridViewBuildHelper.SetSimpleViewElements(gdvLicenseList, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Format license state information for displaying in UI.
        /// </summary>
        /// <param name="licenseState">The license state</param>
        /// <param name="punctuation">The punctuation for connecting license information,such as a blank or a hyphen</param>
        /// <returns>String after format license state.</returns>
        protected string Format4LicenseState(string licenseState, string punctuation)
        {
            string licenseInfo = string.Empty;

            if (!string.IsNullOrEmpty(licenseState) && StandardChoiceUtil.IsDisplayLicenseState())
            {
                licenseInfo = string.Format("{0}{1}", licenseState, punctuation);
            }

            return licenseInfo;
        }

        /// <summary>
        /// Removes the selected license.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RemoveLicenseItemLink_OnClick(object sender, EventArgs e)
        {
            LinkButton lbndel = (LinkButton)sender;
            string seqNbr = lbndel.Attributes["SeqNbr"];
            string type = lbndel.Attributes["Type"];

            LicenseModel4WS licenseModel = new LicenseModel4WS();
            licenseModel.licenseType = type;
            licenseModel.licSeqNbr = seqNbr;
            licenseModel.serviceProviderCode = ConfigManager.AgencyCode;

            ILicenseBLL licenseBll = ObjectFactory.GetObject<ILicenseBLL>();
            licenseBll.DeleteContracotrLicensePK(ConfigManager.AgencyCode, licenseModel, AppSession.User.PublicUserId, AppSession.User.UserSeqNum);

            // Update license of user session.
            LicenseModel4WS[] licenses = AppSession.User.Licenses;

            if (licenses != null && licenses.Length > 0)
            {
                List<LicenseModel4WS> licenseList = new List<LicenseModel4WS>();

                foreach (LicenseModel4WS license in licenses)
                {
                    if (license.licenseType.Equals(type, StringComparison.InvariantCultureIgnoreCase)
                        && license.licSeqNbr.Equals(seqNbr, StringComparison.InvariantCultureIgnoreCase)
                        && license.serviceProviderCode.Equals(ConfigManager.AgencyCode, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    licenseList.Add(license);
                }

                AppSession.User.UserModel4WS.licenseModel = licenseList.ToArray();
            }

            AppSession.User.AllContractorLicenses = null;
            DisplayLicenseList(UserSeqNum);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "RefreshAttachmentList", "RefreshAttachmentList();", true);
        }

        /// <summary>
        /// Convert the status for I18N display
        /// </summary>
        /// <param name="objStatus">standard english status</param>
        /// <returns>I18N status</returns>
        protected string GetStatusForI18NDisplay(object objStatus)
        {
            if (objStatus == null || string.IsNullOrEmpty(objStatus.ToString()))
            {
                return string.Empty;
            }

            string status = objStatus.ToString();
            string resultStatus = status;

            switch (status.ToLowerInvariant())
            {
                case ContractorLicenseStatus.Pending:
                    resultStatus = GetTextByKey("ACA_ContractorLicense_Status_Pending");
                    break;
                case ContractorLicenseStatus.Rejected:
                    resultStatus = GetTextByKey("ACA_ContractorLicense_Status_Rejected");
                    break;
                case ContractorLicenseStatus.Approved:
                    resultStatus = GetTextByKey("ACA_ContractorLicense_Status_Approved");
                    break;
            }

            return resultStatus;
        }

        /// <summary>
        /// GridView LicenseList row data bound event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void LicenseList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                BuildActionMenu(e);
            }
        }

        /// <summary>
        /// Fire sorted event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LicenseList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            if (GridViewSort != null)
            {
                GridViewSort(sender, e);
            }
        }

        /// <summary>
        /// Page index changing event
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LicenseList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// Change button style when disable button. 
        /// </summary>
        /// <param name="lbnDel">Delete link button.</param>
        private void DisableButton(LinkButton lbnDel)
        {
            lbnDel.OnClientClick = string.Empty;
        }

        /// <summary>
        /// Need disable remove button by option value in stander choice.
        /// </summary>
        /// <returns>True or false.</returns>
        private bool IsDisableRevButton()
        {
            bool isRegisterLPuser = StandardChoiceUtil.IsRequiredLicense();

            //if require license to register and license count is one, the remove Button should disable.
            if (GridViewDataSource.Count == 1 && isRegisterLPuser)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Bind license item data
        /// </summary>
        private void BindLicenseList()
        {
            gdvLicenseList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_licensePro_label_NoLicenseProFound");
            gdvLicenseList.DataSource = GridViewDataSource;
            gdvLicenseList.DataBind();
        }

        /// <summary>
        /// Bind action menu.
        /// </summary>
        /// <param name="eventArgs">GridView Row Event Arguments</param>
        private void BuildActionMenu(GridViewRowEventArgs eventArgs)
        {
            // the SeqNbr and Type are used to get the seqNbr and type value when removing a license.            
            HiddenField hdLicType = (HiddenField)eventArgs.Row.FindControl("hdnLicType");
            HiddenField hdLicSeqNbr = (HiddenField)eventArgs.Row.FindControl("hdnLicSeqNbr");
            PopupActions actionMenu = eventArgs.Row.FindControl("actionMenu") as PopupActions;
            LinkButton btnRemoveLicense = eventArgs.Row.FindControl("btnRemoveLicenseItem") as LinkButton;
            string actionStyle = StandardChoiceUtil.GetPopActionItemStyle();

            ActionViewModel actionView;
            string licenseType = ScriptFilter.FilterJSChar(hdLicType.Value);
            string licenseSeqNbr = hdLicSeqNbr.Value;
            var actionList = new List<ActionViewModel>();

            if (!string.IsNullOrEmpty(licenseSeqNbr) && !string.IsNullOrEmpty(licenseType))
            {
                //View details action
                actionView = new ActionViewModel();
                actionView.ActionLabel = GetTextByKey("aca_accountmanager_licenselist_label_action_view");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_view.png");
                actionView.ActionId = actionMenu.ClientID + "_View";

                actionView.ClientEvent = string.Format(
                                                        "return ViewUserLicenseDetails('{0}','{1}','{2}','{3}','{4}');",
                                                        ConfigManager.AgencyCode, 
                                                        licenseType, 
                                                        licenseSeqNbr, 
                                                        UserSeqNum,
                                                        actionStyle.Equals(ACAConstant.POP_ACTION_ICO, StringComparison.CurrentCultureIgnoreCase) ? actionView.ActionId  : actionMenu.ActionsLinkClientID);
                actionList.Add(actionView);

                //Delete action
                if (btnRemoveLicense != null && !IsHiddenRemoveLink())
                {
                    btnRemoveLicense.Attributes.Add("SeqNbr", hdLicSeqNbr.Value);
                    btnRemoveLicense.Attributes.Add("Type", hdLicType.Value);

                    actionView = new ActionViewModel();
                    actionView.ActionLabel = GetTextByKey("aca_accountmanager_licenselist_label_action_remove");
                    actionView.IcoUrl = ImageUtil.GetImageURL("popaction_delete.png");
                    actionView.ActionId = actionMenu.ClientID + "_Remove";

                    actionView.ClientEvent = string.Format("return RemoveLicense('{0}');", btnRemoveLicense.UniqueID);
                    actionList.Add(actionView);
                }
            }

            if (actionList.Count > 0)
            {
                actionMenu.Visible = true;
                actionMenu.ActionLableKey = "aca_accountmanager_licenselist_label_actionlink";
                actionMenu.AvailableActions = actionList.ToArray();
                actionMenu.BindListAction();
            }
        }

        /// <summary>
        /// Initial Remove Button.
        /// </summary>
        /// <returns>IsHiddenRemoveLink flag</returns>
        private bool IsHiddenRemoveLink()
        {
            bool isHidden = false;

            // view state
            if (EditState.Equals("VIEW", StringComparison.InvariantCultureIgnoreCase))
            {
                isHidden = true;
            }
            else
            {
                //The remove licnese link enable is configured in ACA admin.
                if (StandardChoiceUtil.DisabledRemoveLicense())
                {
                    isHidden = true;
                }
                else
                {
                    if (IsDisableRevButton())
                    {
                        isHidden = true;
                    }
                    else
                    {
                        isHidden = false;
                    }
                }
            }

            return isHidden;
        }

        #endregion Methods
    }
}