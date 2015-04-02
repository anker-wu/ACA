#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionWizardInputLocation.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description: Input the location and contact.
 *
 *  Notes:
 *      $Id: InspectionWizardInputLocation.aspx.cs 170366 2010-09-08 05:34:25Z ACHIEVO\daly.zeng $.
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
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Regional;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Inspection
{
    /// <summary>
    /// This class provide the ability to input the location and contact.
    /// </summary>
    public partial class InspectionWizardInputLocation : InspectionWizardBasePage
    {
        #region Fields

        /// <summary>
        /// The contact type is contact
        /// </summary>
        private const string DATASOURCE_CONTACT = "CONTACT";

        /// <summary>
        /// The contact type is LP
        /// </summary>
        private const string DATASOURCE_LP = "LP";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the contact name.
        /// </summary>
        protected string ContactName { get; set; }

        /// <summary>
        /// Gets or sets the contact phone.
        /// </summary>
        protected string ContactPhone { get; set; }

        /// <summary>
        /// Gets a value indicating whether display default contact as inspection's contact configuration or not.
        /// </summary>
        protected bool DisplayDefaultContact4Inspection
        {
            get
            {
                IXPolicyBll policyBll = ObjectFactory.GetObject<IXPolicyBll>();
                string displayPrimaryContact = policyBll.GetValueByKey(XPolicyConstant.INSPECITON_DISPLAY_DEFAULT_CONTACT, ModuleName);

                // Enable display default contact for inspection(default is enable).
                return !ValidationUtil.IsNo(displayPrimaryContact);
            }
        }

        /// <summary>
        /// Gets the View ID.
        /// </summary>
        private string ViewID
        {
            get { return GviewID.InspectionSpecifyContact; }
        }

        #endregion Properties

        /// <summary>
        /// Raises the page load event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitleKey("aca_inspection_title_scheduleorrequestinspection");
            SetDialogMaxHeight("600");
            ControlUtil.SetMaskForPhoneAndZip(!IsPostBack, true, null, null, true, txtPhoneNumber);

            // Set the fields as requred an need validate
            var permission = new GFilterScreenPermissionModel4WS { permissionLevel = GViewConstant.SECTION_CONTACT_ADDRESS };
            ControlBuildHelper.AddValidationForStandardFields(ViewID, ModuleName, permission, Controls);

            if (!DisplayDefaultContact4Inspection)
            {
                lblLocationContactContext.LabelKey = "aca_inspection_locationcontact_label_instruction";
            }

            if (!Page.IsPostBack)
            {
                if (!AppSession.IsAdmin)
                {
                    MarkCurrentPageTrace(InspectionWizardPage.Location, false);
                }

                lblInpectionType.Text = string.Format(GetTextByKey("aca_inspection_type_label"), InspectionWizardParameter.TypeText);

                // display the location and contact
                CapModel4WS capModel = GetCapModel();
                if (capModel != null && !AppSession.IsAdmin)
                {
                    addressView.Display(capModel.addressModel, false);
                }
                else
                {
                    divLocationContent.Visible = false;
                    divLocationContentEmpty.Visible = true;
                }

                Dictionary<string, PeopleModel4WS[]> contactDataSource = GetContactDataSource(capModel);

                // bind the contact list
                if (!AppSession.IsAdmin)
                {
                    BindContactList(contactDataSource);
                }

                // set the contact UI
                SetContactUI(contactDataSource);

                // set the contact by permission
                SetContactByPermission();

                if (!AppSession.IsAdmin)
                {
                    MarkCurrentPageTrace(InspectionWizardPage.Location, true);

                    // to make the back button show/hide.
                    bool isShowBack = IsShowBack(InspectionWizardPage.Location);

                    tdBack.Visible = isShowBack;
                    tdBackSpace.Visible = isShowBack;
                }
            }

            if (AppSession.IsAdmin)
            {
                rbContactSpecify.SectionID = string.Format(
                                                        "{1}{0}{2}{0}{3}",
                                                        ACAConstant.SPLIT_CHAR,
                                                        ModuleName,
                                                        ViewID,
                                                        divChangeContactSpecify.ClientID.Replace(divChangeContactSpecify.ID, string.Empty));

                rbContactSpecify.SubContainerClientID = divChangeContactSpecify.ClientID;
                rbContactSpecify.ModuleName = ModuleName;
            }
        }

        /// <summary>
        /// Raises the contact change submit button click event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ContactChangeButton_Click(object sender, EventArgs e)
        {
            string firstName = string.Empty;
            string middleName = string.Empty;
            string lastName = string.Empty;
            string phoneIDD = string.Empty;
            string phone = string.Empty;
            string countryCode = string.Empty;

            if (rbContactSelect.Checked)
            {
                string selectedContact = ddlContactList.SelectedValue;
                string[] contactItem = selectedContact.Split(ACAConstant.SPLIT_CHAR);

                if (contactItem.Length >= 6)
                {
                    firstName = contactItem[0];
                    middleName = contactItem[1];
                    lastName = contactItem[2];
                    phoneIDD = contactItem[3];
                    phone = contactItem[4];
                    countryCode = contactItem[5];
                }
            }
            else
            {
                firstName = txtFirstName.Text;
                middleName = txtMiddleName.Text;
                lastName = txtLastName.Text;

                // set the default country code.
                RegionalUtil.GetDefaultCountryCode();

                phone = txtPhoneNumber.GetPhone(countryCode);
                phoneIDD = ControlUtil.GetCountryCodeText(txtPhoneNumber);
            }

            ContactName = UserUtil.FormatToFullName(ScriptFilter.AntiXssHtmlEncode(firstName), ScriptFilter.AntiXssHtmlEncode(middleName), ScriptFilter.AntiXssHtmlEncode(lastName));
            ContactPhone = ModelUIFormat.FormatPhoneShow(phoneIDD, phone, countryCode);
            divContactContentEmpty.Visible = false;

            hdContactContent.Value = string.Format(
                "{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}",
                ACAConstant.SPLIT_CHAR,
                firstName,
                middleName,
                lastName,
                phoneIDD,
                phone,
                countryCode);

            //Focus the Change Contact link after clicking Submit button.
            Page.FocusElement(string.Format("{0} a", tblCollapsesOrExpandChangeContact.ClientID));
        }

        /// <summary>
        /// Raises the Continue button click event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            //"divContactChangeLink.Visible" means that, the inspection contact edit form is visible and could be edit.
            if (!DisplayDefaultContact4Inspection && divContactChangeLink.Visible)
            {
                ContactChangeButton_Click(sender, e);
            }

            InspectionParameter inspectionParameter = InspectionWizardParameter;

            string[] contactContents = null;
            if (hdContactContent.Value != null)
            {
                contactContents = hdContactContent.Value.Split(ACAConstant.SPLIT_CHAR);
            }

            if (contactContents != null && contactContents.Length >= 6)
            {
                inspectionParameter.ContactFirstName = contactContents[0];
                inspectionParameter.ContactMiddleName = contactContents[1];
                inspectionParameter.ContactLastName = contactContents[2];
                inspectionParameter.ContactPhoneIDD = contactContents[3];
                inspectionParameter.ContactPhoneNumber = contactContents[4];
                inspectionParameter.ContactCountryCode = contactContents[5];
            }

            // set the radio button in contact change section
            inspectionParameter.ContactChangeOption = InspectionContactChangeOption.Unknown;

            if (rbContactSelect.Visible && rbContactSelect.Checked)
            {
                inspectionParameter.ContactChangeOption = InspectionContactChangeOption.Select;
            }
            else if (rbContactSpecify.Visible && rbContactSpecify.Checked)
            {
                inspectionParameter.ContactChangeOption = InspectionContactChangeOption.Specify;
            }

            string url = string.Format("{0}?{1}", "InspectionWizardConfirm.aspx", Request.QueryString.ToString());
            url = InspectionParameterUtil.UpdateURLAndSaveParameters(url, inspectionParameter);

            Response.Redirect(url);
        }

        /// <summary>
        /// Raises the Back button click event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void BackButton_Click(object sender, EventArgs e)
        {
            string previousURL = InspectionActionViewUtil.GetWizardPreviousURL(InspectionWizardParameter);

            if (!string.IsNullOrEmpty(previousURL))
            {
                Response.Redirect(previousURL);
            }
        }

        /// <summary>
        /// Gets the contact's data source from CAP model.
        /// </summary>
        /// <param name="capModel">The CAP model.</param>
        /// <returns>The contact data source, key: Contact/LP, value: people model list.</returns>
        private Dictionary<string, PeopleModel4WS[]> GetContactDataSource(CapModel4WS capModel)
        {
            Dictionary<string, PeopleModel4WS[]> result = new Dictionary<string, PeopleModel4WS[]>();

            CapContactModel4WS[] capContactGroups = CapUtil.CombineApplicantToContactsGroup(capModel.applicantModel, capModel.contactsGroup);

            // add the contact
            if (capContactGroups != null && capContactGroups.Length > 0)
            {
                PeopleModel4WS[] peopleList = capContactGroups.Select(model => model.people).ToArray();

                result.Add(DATASOURCE_CONTACT, peopleList);
            }

            LicenseProfessionalModel[] lpModelList = TempModelConvert.ConvertToLicenseProfessionalModelList(capModel.licenseProfessionalList);

            // add the LP contact
            if (lpModelList != null && lpModelList.Length > 0)
            {
                PeopleModel4WS[] peopleList = lpModelList.Select(model => new PeopleModel4WS
                {
                    firstName = model.contactFirstName,
                    middleName = model.contactMiddleName,
                    lastName = model.contactLastName,
                    phone1CountryCode = model.phone1CountryCode,
                    phone1 = model.phone1,
                    countryCode = model.countryCode,
                    flag = model.printFlag
                }).ToArray();

                result.Add(DATASOURCE_LP, peopleList);
            }

            return result;
        }

        /// <summary>
        /// Bind the contact list.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        private void BindContactList(Dictionary<string, PeopleModel4WS[]> dataSource)
        {
            if (dataSource == null || dataSource.Count == 0)
            {
                return;
            }

            foreach (PeopleModel4WS[] peoplelist in dataSource.Values)
            {
                foreach (PeopleModel4WS people in peoplelist)
                {
                    ListItem listItem = FormatContactListItem(people);

                    if (!ddlContactList.Items.Contains(listItem))
                    {
                        ddlContactList.Items.Add(listItem);
                    }
                }
            }
        }

        /// <summary>
        /// Format the contact ListItem
        /// </summary>
        /// <param name="people">The people model</param>
        /// <returns>The formatted ListItem.</returns>
        private ListItem FormatContactListItem(PeopleModel4WS people)
        {
            bool isRTL = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft;
            string contactDisplayPattern = isRTL ? "{2}{1}{0}" : "{0}{1}{2}";

            string fullName = UserUtil.FormatToFullName(people.firstName, people.middleName, people.lastName);
            string phone = ModelUIFormat.FormatPhoneShow(people.phone1CountryCode, people.phone1, people.countryCode);
            phone = LabelUtil.RemoveHtmlFormat(phone);

            phone = string.IsNullOrEmpty(phone) ? string.Empty : string.Format("({0})", phone);
            string space4Contact = string.IsNullOrEmpty(phone) ? string.Empty : " ";

            ListItem result = new ListItem();
            result.Text = string.Format(contactDisplayPattern, fullName, space4Contact, phone);
            result.Value = string.Format(
                            "{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}",
                            ACAConstant.SPLIT_CHAR,
                            people.firstName,
                            people.middleName,
                            people.lastName,
                            people.phone1CountryCode,
                            people.phone1,
                            people.countryCode);

            return result;
        }

        /// <summary>
        /// Gets contact info by below priority:
        ///     1. primary contact
        ///     2. contact
        ///     3. primary LP
        ///     4. LP
        /// </summary>
        /// <param name="dataSource">The contact data source</param>
        /// <returns>String array of contact info.</returns>
        private PeopleModel4WS GetContact4View(Dictionary<string, PeopleModel4WS[]> dataSource)
        {
            if (dataSource == null || dataSource.Count == 0)
            {
                return null;
            }

            // get contact for view from contact people
            if (dataSource.ContainsKey(DATASOURCE_CONTACT))
            {
                PeopleModel4WS[] contactPeoplelist = dataSource[DATASOURCE_CONTACT];

                if (contactPeoplelist != null && contactPeoplelist.Length > 0)
                {
                    foreach (PeopleModel4WS people in contactPeoplelist)
                    {
                        if (ValidationUtil.IsYes(people.flag))
                        {
                            return people;
                        }
                    }

                    return contactPeoplelist[0];
                }
            }

            // get contact for view from LP
            if (dataSource.ContainsKey(DATASOURCE_LP))
            {
                PeopleModel4WS[] lpPeoplelist = dataSource[DATASOURCE_LP];

                if (lpPeoplelist != null && lpPeoplelist.Length > 0)
                {
                    foreach (PeopleModel4WS people in lpPeoplelist)
                    {
                        if (ValidationUtil.IsYes(people.flag))
                        {
                            return people;
                        }
                    }

                    return lpPeoplelist[0];
                }
            }

            return null;
        }

        /// <summary>
        /// Set the Contact UI.
        /// </summary>
        /// <param name="contactDataSource">The contact data source.</param>
        private void SetContactUI(Dictionary<string, PeopleModel4WS[]> contactDataSource)
        {
            if (AppSession.IsAdmin)
            {
                // show the change contact box
                string css = divChangeContact.Attributes["class"];
                divChangeContact.Attributes["class"] = css.Replace("ACA_Hide", "ACA_Show");

                divContactContent.Visible = false;
                divContactContentEmpty.Visible = true;
                spAboveButton.Visible = true;
            }
            else
            {
                string firstName = InspectionWizardParameter.ContactFirstName;
                string middleName = InspectionWizardParameter.ContactMiddleName;
                string lastName = InspectionWizardParameter.ContactLastName;
                string phoneIDD = InspectionWizardParameter.ContactPhoneIDD;
                string phone = InspectionWizardParameter.ContactPhoneNumber;
                string countryCode = InspectionWizardParameter.ContactCountryCode;
                bool isContactEmpty = string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(middleName) && string.IsNullOrEmpty(lastName) && string.IsNullOrEmpty(phoneIDD) && string.IsNullOrEmpty(phone);
                InspectionContactChangeOption contactChangeOption = InspectionWizardParameter.ContactChangeOption;
                bool isSubmittedData = contactChangeOption != InspectionContactChangeOption.Unknown;

                // set the default value
                if (!isSubmittedData && contactDataSource != null && contactDataSource.Count > 0 && isContactEmpty)
                {
                    PeopleModel4WS people = GetContact4View(contactDataSource);

                    if (people != null)
                    {
                        firstName = people.firstName;
                        middleName = people.middleName;
                        lastName = people.lastName;
                        phoneIDD = people.phone1CountryCode;
                        phone = people.phone1;
                        countryCode = people.countryCode;
                        isContactEmpty = false;
                    }
                }

                firstName = string.IsNullOrEmpty(firstName) ? string.Empty : firstName;
                middleName = string.IsNullOrEmpty(middleName) ? string.Empty : middleName;
                lastName = string.IsNullOrEmpty(lastName) ? string.Empty : lastName;
                phoneIDD = string.IsNullOrEmpty(phoneIDD) ? string.Empty : phoneIDD;
                phone = string.IsNullOrEmpty(phone) ? string.Empty : phone;
                countryCode = string.IsNullOrEmpty(countryCode) ? string.Empty : countryCode;

                hdContactContent.Value = string.Format(
                            "{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}",
                            ACAConstant.SPLIT_CHAR,
                            firstName,
                            middleName,
                            lastName,
                            phoneIDD,
                            phone,
                            countryCode);

                //try to find the selected value and set the contactChangeOption
                if (contactChangeOption == InspectionContactChangeOption.Select || contactChangeOption == InspectionContactChangeOption.Unknown)
                {
                    contactChangeOption = InspectionContactChangeOption.Specify;

                    foreach (ListItem item in ddlContactList.Items)
                    {
                        if (item.Value.StartsWith(hdContactContent.Value))
                        {
                            contactChangeOption = InspectionContactChangeOption.Select;

                            //maybe the hidden field don't have country code.
                            hdContactContent.Value = item.Value;
                            break;
                        }
                    }
                }

                if (contactChangeOption == InspectionContactChangeOption.Select)
                {
                    rbContactSelect.Checked = true;
                    ddlContactList.SelectedValue = hdContactContent.Value;
                    txtFirstName.Enabled = false;
                    txtMiddleName.Enabled = false;
                    txtLastName.Enabled = false;
                    txtPhoneNumber.Enabled = false;
                }
                else
                {
                    rbContactSpecify.Checked = true;
                    txtFirstName.Text = firstName;
                    txtMiddleName.Text = middleName;
                    txtLastName.Text = lastName;

                    /*
                     * Following case use the inputted phone IDD, else use the default country code.
                     * 1. Not set default country code in V360.
                     * 2. InspectionWizardParameter.ContactCountryCode exists in parameters indicates that the page has navigated back.
                     */
                    string defaultPhoneIDD = txtPhoneNumber.CountryCodeText;

                    if (string.IsNullOrEmpty(defaultPhoneIDD) || !string.IsNullOrEmpty(InspectionWizardParameter.ContactCountryCode))
                    {
                        txtPhoneNumber.CountryCodeText = phoneIDD;
                    }

                    if (string.IsNullOrEmpty(countryCode))
                    {
                        countryCode = RegionalUtil.GetDefaultCountryCode();
                    }

                    txtPhoneNumber.Text = ModelUIFormat.FormatPhone4EditPage(phone, countryCode);
                    ddlContactList.Enabled = false;
                }
                
                // assign the contact value
                ContactName = UserUtil.FormatToFullName(ScriptFilter.AntiXssHtmlEncode(firstName), ScriptFilter.AntiXssHtmlEncode(middleName), ScriptFilter.AntiXssHtmlEncode(lastName));
                ContactPhone = ModelUIFormat.FormatPhoneShow(phoneIDD, phone, countryCode);

                // set the contact's visible and UI
                if (contactDataSource == null || contactDataSource.Count == 0)
                {
                    rbContactSelect.Visible = false;
                    divChangeContactSelect.Visible = false;

                    rbContactSpecify.Visible = false;
                    divChangeContactSpecify.Attributes.Remove("class");
                }

                // set the contact content's visible
                if (!isSubmittedData && isContactEmpty)
                {
                    divContactContentEmpty.Visible = true;
                }
                else
                {
                    divContactContentEmpty.Visible = false;
                }
            }

            /*
            * Notes: set the contact display in schedule inspection 
            * If 'Display a default contact when scheduling an inspection' was set as No in ACA Admin, 
            * then do not need display primary contact info. And do not set a default contact for inspection.
            * So hide the contact info area and display the contact edit section.
            */
            if (!DisplayDefaultContact4Inspection)
            {
                divContactContent.Visible = false;
                divContactContentEmpty.Visible = false;
                AccelaHeightSeparate4.Visible = false;
                tblCollapsesOrExpandChangeContact.Visible = false;
                AccelaHeightSeparate6.Visible = false;
                divChangeContactButton.Visible = false;
                divChangeContact.Attributes["class"] = "SmallPopUpDlg";
            }
        }

        /// <summary>
        /// Set contact by permission
        /// </summary>
        private void SetContactByPermission()
        {
            if (!AppSession.IsAdmin)
            {
                var inspectionPermissionBll = ObjectFactory.GetObject<IInspectionPermissionBll>();

                CapModel4WS capModel = GetCapModel();
                bool right = inspectionPermissionBll.CheckContactRight(capModel, InspectionWizardParameter.AgencyCode, ModuleName, ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_INPUT_CONTACT);

                if (right == false)
                {
                    lblContact.Visible = false;
                    divContactContent.Visible = false;
                    divContactContentEmpty.Visible = false;
                    divContactChangeLink.Visible = false;
                }
            }
        }
    }
}
