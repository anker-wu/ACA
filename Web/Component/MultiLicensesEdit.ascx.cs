#region Header

/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CapDetail.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: MultiLicensesEdit.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using log4net;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Multiple license edit form contains license edit form and license list form.
    /// </summary>
    public partial class MultiLicensesEdit : BaseUserControl
    {
        #region member variable
        /// <summary>
        /// A value to distinguish the refresh single license list section.
        /// </summary>
        protected const string REFRESH_LICENSE_LIST = "$RefreshLicenseList$";

        /// <summary>
        /// indicate the default index value  when no contact is selected in the contact list
        /// </summary>
        private const int NO_LICENSE_SELECTED = -1;

        /// <summary>
        /// indicate the the LP form is expand or collapse.
        /// </summary>
        private const string COLLAPSE = "0";

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(MultiLicensesEdit));

        /// <summary>
        /// R-Reference,need reference data
        /// T-Transactional, user input data.
        /// N-No Limitation,both reference and daily
        /// </summary>
        private string _validateFlag = string.Empty;

        /// <summary>
        /// the validate flag.
        /// </summary>
        private bool _validate;

        /// <summary>
        /// indicate the license form is editable or not.
        /// </summary>
        private bool _isEditable;

        /// <summary>
        /// indicate the license form is in confirm page or not.
        /// </summary>
        private bool _isInConfirmPage;

        /// <summary>
        /// component name
        /// </summary>
        private string _componentName = string.Empty;

        /// <summary>
        /// Has show validation message
        /// </summary>
        private bool _hasShowValidationMessage = false;

        /// <summary>
        /// Add contact event by clicking Save button
        /// </summary>
        public event CommonEventHandler LicensesRemovedEvent;

        #endregion

        #region property

        /// <summary>
        /// Gets or sets a value indicating whether the smart choice validate flag for license
        /// </summary>
        public bool IsValidate
        {
            get
            {
                return _validate;
            }

            set
            {
                _validate = value;
                licenseList.IsValidate = _validate;
                licenseEdit.IsValidate = _validate;
            }
        }

        /// <summary>
        /// Gets or sets the validate flag.
        /// </summary>
        /// <value>The validate flag.</value>
        public string ValidateFlag
        {
            get
            {
                return _validateFlag;
            }

            set
            {
                _validateFlag = value;
                licenseEdit.ValidateFlag = _validateFlag;

                // it need set IsValidate or not, NOT only set true when data source is reference.
                IsValidate = ComponentDataSource.Reference.Equals(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        public bool IsEditable
        {
            get
            {
                return _isEditable;
            }

            set
            {
                _isEditable = value;
                licenseList.IsEditable = _isEditable;
                licenseEdit.IsEditable = _isEditable;
            }
        }

        /// <summary>
        /// Gets Explore License Edit Control
        /// </summary>
        public LicenseEdit LicenseEdit
        {
            get
            {
                return this.licenseEdit;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this control is displayed in the confirm page.
        /// </summary>
        public bool IsInConfirmPage
        {
            get
            {
                return _isInConfirmPage;
            }

            set
            {
                _isInConfirmPage = value;
                licenseList.IsInCapConfirm = value;

                if (_isInConfirmPage)
                {
                    // If in confirm page, disable delete action in LicenseList.
                    licenseList.EnableDeleteAction = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets component name.
        /// </summary>
        public string ComponentName
        {
            get
            {
                return _componentName;
            }

            set
            {
                _componentName = value;
                licenseList.ComponentName = _componentName;
                licenseEdit.ComponentName = _componentName;
            }
        }

        /// <summary>
        /// Gets the data source of the license list.
        /// </summary>
        public DataTable DataSource
        {
            get
            {
                return licenseList.DataSource;
            }
        }

        /// <summary>
        /// Gets edit license function name
        /// </summary>
        public string EditLicenseFunction
        {
            get
            {
                return licenseList.EditLicenseFunction;
            }
        }

        #endregion

        #region public method

        /// <summary>
        /// set the section required property
        /// </summary>
        /// <param name="isRequired">indicate if the section is required</param>
        public void SetSectionRequired(bool isRequired)
        {
            licenseList.SetGridViewRequired(isRequired);
        }

        /// <summary>
        /// Display licensed professional in the current license list.
        /// </summary>
        /// <param name="licenseProfessionals">license Professional model</param>
        public void DisplayLicenses(LicenseProfessionalModel[] licenseProfessionals)
        {
            InitLicenseList(licenseProfessionals);

            InitLicenseForm(licenseProfessionals);
        }

        /// <summary>
        /// Get an array of LicenseProfessionalModel from the licenseList control
        /// </summary>
        /// <returns>an array of LicenseProfessionalModel</returns>
        public LicenseProfessionalModel[] GetLicenseProfessionalFromLicenseList()
        {
            if (licenseList.DataSource == null || (licenseList.DataSource as DataTable).Rows.Count == 0)
            {
                return null;
            }

            DataTable dtLicense = licenseList.DataSource as DataTable;
            List<LicenseProfessionalModel> licenses = new List<LicenseProfessionalModel>();

            foreach (DataRow drLicense in dtLicense.Rows)
            {
                if (drLicense["LicenseProfessionalModel"] != DBNull.Value)
                {
                    LicenseProfessionalModel license = drLicense["LicenseProfessionalModel"] as LicenseProfessionalModel;

                    if (license == null)
                    {
                        continue;
                    }

                    if (!CloneRecordUtil.IsCloneRecord(Request) && StandardChoiceUtil.IsSuperAgency())
                    {
                        CapModel4WS capModel = AppSession.GetCapModelFromSession(this.ModuleName);
                        LicenseProfessionalModel[] licenseProfessionals = LicenseUtil.GetSameTypeNumberLicenses(TempModelConvert.ConvertToLicenseProfessionalModelList(capModel.licenseProfessionalList), license, false);
                        licenses.AddRange(licenseProfessionals);
                    }
                    else
                    {
                        licenses.Add(license);
                    }
                }
            }

            return licenses.ToArray();
        }

        /// <summary>
        /// Check the CAP's license professionals.
        /// </summary>
        /// <returns>true if all license professionals of the CAP are available;otherwise,false.</returns>
        public string CheckLicenses()
        {
            string errorMsg = string.Empty;
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            LicenseProfessionalModel[] licenseProfessions = GetLicenseProfessionalFromLicenseList();

            if (ComponentDataSource.Reference.Equals(ValidateFlag) && licenseProfessions != null)
            {
                if (licenseProfessions.Any(t => ComponentName.Equals(t.componentName)
                                                && (string.IsNullOrEmpty(t.licSeqNbr)
                                                    || ACAConstant.DAILY_LICENSE_NUMBER.Equals(t.licSeqNbr, StringComparison.InvariantCultureIgnoreCase))))
                {
                    errorMsg = GetTextByKey("per_license_error_searchClickedRequired");
                    return errorMsg;
                }
            }

            // Don't need to judge the big CAP in super agency.
            if (!LicenseUtil.EnableExpiredLicense() || StandardChoiceUtil.IsSuperAgency())
            {
                return errorMsg;
            }

            string unAvailableLPNums = LicenseUtil.GetUnAvailableLPNums(licenseProfessions, capModel.capType);

            if (!string.IsNullOrEmpty(unAvailableLPNums))
            {
                string unAvailableMsg = GetTextByKey("per_multiplelicenses_error_unavailablelicense");
                errorMsg = DataUtil.StringFormat(unAvailableMsg, unAvailableLPNums);

                MessageUtil.ShowMessage(Page, MessageType.Error, errorMsg);
            }

            return errorMsg;
        }

        /// <summary>
        /// Shows the validate error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowValidateErrorMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageUtil.ShowMessageByControl(errorMessageLabel, MessageType.Error, message);
            }
        }

        #endregion

        #region protected method

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            licenseList.ParentControlID = ClientID;
            licenseEdit.ParentControlID = ClientID;
            licenseList.LicenseDeleted += new CommonEventHandler(LicenseList_LicenseDeleted);
            licenseEdit.IsMultipleLicensedProfessional = true;
            RefreshLicenseList();

            if (!AppSession.IsAdmin)
            {
                if (IsInConfirmPage)
                {
                    // Hide add and edit section of license list when in Confirm page.
                    licenseEdit.Visible = false;
                }
            }
        }

        /// <summary>
        /// Raises the PreRender event.
        /// </summary>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!AppSession.IsAdmin)
            {
                string errorMessage = CheckLicenses();

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ShowValidateErrorMessage(errorMessage);

                    //if the License validate failed then should return to prevent the required validate message overwrite.
                    return;
                }

                LicenseProfessionalModel[] licenseProfessionals = GetLicenseProfessionalFromLicenseList();
                ShowRequiredMessage(licenseProfessionals);
            }
        }

        #endregion

        #region private method

        /// <summary>
        /// Handle LicenseDelete event ,delete a license
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="arg">A CommonEventArgs object containing the contact model.</param>
        private void LicenseList_LicenseDeleted(object sender, CommonEventArgs arg)
        {
            if (LicensesRemovedEvent != null)
            {
                try
                {
                    RaiseLicenseChangeEvent(sender);
                    MultiLicensePanel.Update();
                    licenseEdit.ShowActionNoticeMessage(ActionType.DeleteSuccess);
                }
                catch (ACAException ex)
                {
                    Logger.Error(ex);
                    licenseEdit.ShowActionNoticeMessage(ActionType.DeleteFailed);
                }        
            }
       
            licenseList.SelectedIndex = NO_LICENSE_SELECTED;
        }

        /// <summary>
        /// Handle LicenseChanged event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        private void RaiseLicenseChangeEvent(object sender)
        {
            if (LicensesRemovedEvent == null || licenseList.DataSource == null)
            {
                return;
            }

            LicenseProfessionalModel[] licenseProfessionals = GetLicenseProfessionalFromLicenseList();

            if (licenseProfessionals != null && licenseProfessionals.Length > 0)
            {
                foreach (LicenseProfessionalModel lp in licenseProfessionals)
                {
                    lp.componentName = ComponentName;
                }

                LicensesRemovedEvent(sender, new CommonEventArgs(licenseProfessionals));
            }
            else
            {
                // If all LPs have deleted from License List, so pass the component name to remove the LP data by component name.
                LicensesRemovedEvent(sender, new CommonEventArgs(ComponentName));
            }

            ShowRequiredMessage(licenseProfessionals);
        }

        /// <summary>
        /// Initial license list grid view.
        /// </summary>
        /// <param name="licenseProfessionals">license professionals model</param>
        private void InitLicenseList(LicenseProfessionalModel[] licenseProfessionals)
        {
            DataTable dtLicense = null;

            if (!AppSession.IsAdmin)
            {
                ILicenseBLL licenseBLL = ObjectFactory.GetObject<ILicenseBLL>();
                dtLicense = licenseBLL.ConvertLPModelToDataTable(licenseProfessionals);
            }

            licenseList.DataSource = dtLicense;
            licenseList.BindLicenseList();
        }

        /// <summary>
        /// Initial license form.
        /// </summary>
        /// <param name="licenseProfessionals">license professionals model</param>
        private void InitLicenseForm(LicenseProfessionalModel[] licenseProfessionals)
        {
            LicenseEdit.HideCondition();

            //initialize license form
            licenseEdit.IsMultipleLicensedProfessional = true;
        }

        /// <summary>
        /// Show required message when license professional list exist no value of required field. 
        /// </summary>
        /// <param name="licensePros">License Professional Model</param>
        private void ShowRequiredMessage(LicenseProfessionalModel[] licensePros)
        {
            if (_hasShowValidationMessage)
            {
                return;
            }

            _hasShowValidationMessage = true;
            string errorMessage = string.Empty;

            if (licensePros != null && (!IsEditable || IsValidate))
            {
                foreach (LicenseProfessionalModel item in licensePros)
                {
                    if (item.templateAttributes == null)
                    {
                        continue;
                    }

                    TemplateAttributeModel[] fields = item.templateAttributes.Where(attribute => attribute.vchFlag == ACAConstant.TEMPLATE_FIIELD_STATUS_ALWAYSEDITABLE).ToArray();

                    if (!RequiredValidationUtil.ValidateFields4Template(fields))
                    {
                        errorMessage = GetTextByKey("per_licenselist_required_validate_msg");
                        ShowValidateErrorMessage(errorMessage);
                        break;
                    }
                }
            }
            else if (!IsValidate
                && IsEditable 
                && (!RequiredValidationUtil.ValidateFields4LPList(ModuleName, GviewID.LicenseEdit, licensePros)
                    || !FormatValidationUtil.ValidateFormat4LPList(ModuleName, GviewID.LicenseEdit, licensePros)))
            {
                errorMessage = GetTextByKey("per_licenselist_required_validate_msg"); 
                ShowValidateErrorMessage(errorMessage);
            }
        }

        /// <summary>
        /// Refresh license section.
        /// </summary>
        private void RefreshLicenseList()
        {
            string postSourceID = Request.Form[Page.postEventSourceID];

            if (!AppSession.IsAdmin && IsPostBackOnCurrentControl())
            {
                licenseEdit.HideActionNoticeMessage();
            }

            if ((MultiLicensePanel.UniqueID + REFRESH_LICENSE_LIST).Equals(postSourceID, StringComparison.InvariantCultureIgnoreCase))
            {
                string senderArgs = Request.Form[Page.postEventArgumentID];
                string[] paras = senderArgs.Split(ACAConstant.SPLIT_CHAR6);
                string componentName = paras[0];
                ActionType actionType = EnumUtil<ActionType>.Parse(paras[1]);
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                LicenseProfessionalModel4WS[] licenseProfessionalList = LicenseUtil.FindLicenseProfessionalsWithComponentName(capModel, componentName);
                LicenseProfessionalModel[] licensees = LicenseUtil.ResetLicenseeAgency(licenseProfessionalList, capModel.capID.serviceProviderCode);
                DisplayLicenses(licensees);

                // Show message
                licenseEdit.ShowActionNoticeMessage(actionType);

                Page.FocusElement(Request.Form["__LASTFOCUS_ID"]);
            }
        }

        #endregion
    }
}
