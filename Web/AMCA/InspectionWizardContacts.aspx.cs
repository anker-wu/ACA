/**
* <pre>
* 
*  Accela Citizen Access
*  File: InspectionWizardContacts.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2014
* 
*  Description:
*  Inspection Wizard Update or Change Contact
* 
*  Notes:
*      $Id: InspectionWizardContacts.aspx.cs 269129 2014-04-04 10:32:41Z ACHIEVO\james.shi $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
* 
* </pre>
*/
using System;
using System.Collections.Generic;
using System.Text;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

/// <summary>
/// 
/// </summary>
public partial class InspectionWizardContacts : AccelaPage
{
    private const string HTML_EMPTY = "&nbsp;";

    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder PageTitle = new StringBuilder();
    public StringBuilder ContactsList = new StringBuilder();
    public StringBuilder RecordInfo = new StringBuilder();
    public StringBuilder NewContact = new StringBuilder();
    public StringBuilder Buttons = new StringBuilder();
    public StringBuilder BackForwardLinks = new StringBuilder();
    public StringBuilder HiddenFields = new StringBuilder();
    public StringBuilder PageMessage = new StringBuilder();
    // public string PageBreadcrumbIndex = string.Empty;
    public string LocalModuleName = string.Empty;
    public string ResultPage = string.Empty;
    public string CollectionId = string.Empty;
    public string CollectionModule = string.Empty;
    public string CollectionOperation = string.Empty;
    public string SearchMode = string.Empty;
    private string Filter = string.Empty;

    private string firstName = string.Empty;
    private string middleName = string.Empty;
    private string lastName = string.Empty;
    private string thePhone = string.Empty;
    private string country = string.Empty;
    private string firstNameNew = string.Empty;
    private string middleNameNew = string.Empty;
    private string lastNameNew = string.Empty;
    private string phoneNew = string.Empty;
    private string countryNew = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("InspectionWizardContacts.aspx");

        State = GetFieldValue("State", false);
        string buttonChoice = GetFieldValue("button", false);
        string checkBoxChoice = GetFieldValue("contactOption", false);
        string Id = GetFieldValue("Id", false);
        string InspectionId = GetFieldValue("InspectionId", false);
        int inspectionID;
        if (!int.TryParse(InspectionId, out inspectionID))
        {
            inspectionID = 0; // this should never happen - 
        }
        string SearchMode = GetFieldValue("Mode", false);
        string inspType = GetFieldValue("InspType", false);
        string inspStatus = GetFieldValue("InspStatus", false);
        string ScheduleManner = GetFieldValue(ACAConstant.INSPECTION_SCHEDULING_MANNER, false);
        string PermitNo = GetFieldValue("PermitNo", false);
        string altID = GetFieldValue("AltID", false);
        string permitType = GetFieldValue("PermitType", false);
        string SearchBy = GetFieldValue("SearchBy", false);
        string SearchType = GetFieldValue("SearchType", false);
        string ViewPermitPageNo = GetFieldValue("ViewPermitPageNo", false);
        string InspectionsPageNo = GetFieldValue("InspectionsPageNo", false);
        string isRequestPending = GetFieldValue("isRequestPending", false);
        string isReadyTimeEnabled = GetFieldValue(ACAConstant.INSPECTION_IS_READY_TIME_ENABLED, false);
        string RescheduleRestrictionSettings = GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS, false);
        string CancellationRestrictionSettings = GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS, false);
        string Action = GetFieldValue("Action", false);
        string InspSeqNum = GetFieldValue("InspSeqNum", false);
        string InspUnits = GetFieldValue("InspUnits", false);
        string TheComment = GetFieldValue("Comments", false);
        
        firstName = GetFieldValue("ContactFirstName", false);
        middleName = GetFieldValue("ContactMiddleName", false);
        lastName = GetFieldValue("ContactLastName", false);
        thePhone = GetFieldValue("ContactPhone", false).Replace("plus","+");
        country = GetFieldValue("ContactCountry", false).Replace("plus","+");

        firstNameNew = GetFieldValue("NewFirstName", false);
        middleNameNew = GetFieldValue("NewMiddleName", false);
        lastNameNew = GetFieldValue("NewLastName", false);
        phoneNew = GetFieldValue("PhoneNumber", false).Replace("plus", "+");
        countryNew = GetFieldValue("PhonePrefix", false).Replace("plus", "+");

        if (firstNameNew == string.Empty
            && middleNameNew == string.Empty
            && lastNameNew == string.Empty
            && countryNew == string.Empty
            && phoneNew == string.Empty)
        {
            firstNameNew = firstName;
            middleNameNew = middleName;
            lastNameNew = lastName;
            phoneNew = thePhone;
            countryNew = country;
        }
        CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
        if (capModel.addressModel != null && capModel.addressModel.displayAddress != null)
        {
            RecordInfo.Append("<br>");
            RecordInfo.Append("<Label id=\"pageSectionTitle\">Location: </Label>");
            RecordInfo.Append("<br><Label id=\"pageLineText\">");
            RecordInfo.Append(capModel.addressModel.displayAddress);
            RecordInfo.Append("</Label>");
        }
        string theContactsList = BindContactList(capModel);

        // string InspActionType = "Schedule";
        string Comments = GetFieldValue("Comments", false);
        string typeRowNumber = GetFieldValue("TypeRowNumber", false);
        string rowNumber = GetFieldValue("RowNumber", false);
        string InAdvance = GetFieldValue("InAdvace",false);
        if (RescheduleRestrictionSettings == string.Empty)
        {
            RescheduleRestrictionSettings = GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "0", false);
            RescheduleRestrictionSettings += "|" + GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "1", false);
            RescheduleRestrictionSettings += "|" + GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "2", false);
            RescheduleRestrictionSettings += "|" + GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "3", false);
        }
        if (CancellationRestrictionSettings == string.Empty)
        {
            CancellationRestrictionSettings = GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "0", false);
            CancellationRestrictionSettings += "|" + GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "1", false);
            CancellationRestrictionSettings += "|" + GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "2", false);
            CancellationRestrictionSettings += "|" + GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "3", false);
        }

        StringBuilder urlProperties = new StringBuilder();
        string[] resceduleRestrictions = RescheduleRestrictionSettings.Split('|');
        string[] cancelRestrictions = CancellationRestrictionSettings.Split('|');

        //from schedule confirmation screen
        urlProperties.Append("&Id=" + Id.ToString());
        urlProperties.Append("&InspectionId=" + InspectionId.ToString());
        urlProperties.Append("&scheduledDateTime=" + MyProxy.GetFieldValue("scheduledDateTime", false));
        urlProperties.Append("&endScheduledDateTime=" + MyProxy.GetFieldValue("endScheduledDateTime", false));
        urlProperties.Append("&finishTimeOption=" + MyProxy.GetFieldValue("finishTimeOption", false));
        urlProperties.Append("&rDayOption=" + MyProxy.GetFieldValue("rDayOption", false));
        urlProperties.Append("&PermitNo=" + PermitNo.ToString());
        urlProperties.Append("&InAdavnce" + InAdvance.ToString());
        urlProperties.Append("&Action=" + Action.ToString());
        urlProperties.Append("&AltID=" + altID.ToString());
        urlProperties.Append("&PermitType=" + permitType);
        urlProperties.Append("&SearchBy=" + SearchBy.ToString());
        urlProperties.Append("&SearchType=" + SearchType.ToString());
        urlProperties.Append("&InspectionPageNo=" + InspectionsPageNo.ToString());
        urlProperties.Append("&ViewPermitPageNo=" + ViewPermitPageNo.ToString());
        urlProperties.Append("&isRequestPending=" + isRequestPending.ToString());
        urlProperties.Append("&" + (string)ACAConstant.INSPECTION_IS_READY_TIME_ENABLED + "=" + isReadyTimeEnabled);
        urlProperties.Append("&Mode=" + SearchMode.ToString());
        urlProperties.Append("&Module=" + ModuleName.ToString());
        urlProperties.Append("&RowNumber=" + rowNumber.ToString());
        urlProperties.Append("&TypeRowNumber=" + typeRowNumber.ToString());
        urlProperties.Append("&InspUnits=" + InspUnits.ToString());
        urlProperties.Append("&InspSeqNum=" + InspSeqNum.ToString());
        urlProperties.Append("&" + ACAConstant.INSPECTION_IN_ADVANCE + "=" + InAdvance); //Not populated on this page always empty.
        urlProperties.Append("&" + ACAConstant.INSPECTION_SCHEDULING_MANNER + "=" + ScheduleManner);
        urlProperties.Append("&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "0=" + resceduleRestrictions[0]);
        urlProperties.Append("&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "1=" + resceduleRestrictions[1]);
        urlProperties.Append("&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "2=" + resceduleRestrictions[2]);
        urlProperties.Append("&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "3=" + resceduleRestrictions[3]);
        urlProperties.Append("&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "0=" + cancelRestrictions[0]);
        urlProperties.Append("&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "1=" + cancelRestrictions[1]);
        urlProperties.Append("&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "2=" + cancelRestrictions[2]);
        urlProperties.Append("&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "3=" + cancelRestrictions[3]);
        urlProperties.Append("&InspType=" + inspType.ToString());
        urlProperties.Append("&InspStatus=" + inspStatus.ToString());
        urlProperties.Append("&NextBusinessDateString4WS=" + GetFieldValue("NextBusinessDateString4WS", false));
        urlProperties.Append("&ScheduleOneScreenBreadcrumbIndex=" + GetFieldValue("ScheduleOneScreenBreadcrumbIndex", false));
        urlProperties.Append("&dayOption=" + GetFieldValue("dayOption", false));
        urlProperties.Append("&Comments=" + Comments);

        HiddenFields.Append(HTML.PresentHiddenField("Id", Id));
        HiddenFields.Append(HTML.PresentHiddenField("InspectionId", InspectionId));
        HiddenFields.Append(HTML.PresentHiddenField("scheduledDateTime", MyProxy.GetFieldValue("scheduledDateTime", false)));
        HiddenFields.Append(HTML.PresentHiddenField("endScheduledDateTime", MyProxy.GetFieldValue("endScheduledDateTime", false)));
        HiddenFields.Append(HTML.PresentHiddenField("finishTimeOption", MyProxy.GetFieldValue("finishTimeOption", false)));
        HiddenFields.Append(HTML.PresentHiddenField("rDayOption", MyProxy.GetFieldValue("rDayOption", false)));
        HiddenFields.Append(HTML.PresentHiddenField("PermitNo", PermitNo));
        HiddenFields.Append(HTML.PresentHiddenField("AltID", altID.ToString()));
        HiddenFields.Append(HTML.PresentHiddenField("PermitType", permitType));
        HiddenFields.Append(HTML.PresentHiddenField("SearchBy", SearchBy));
        HiddenFields.Append(HTML.PresentHiddenField("SearchType", SearchType));
        HiddenFields.Append(HTML.PresentHiddenField("Mode", SearchMode));
        HiddenFields.Append(HTML.PresentHiddenField("Action", Action));
        HiddenFields.Append(HTML.PresentHiddenField("RowNumber", rowNumber.ToString()));
        HiddenFields.Append(HTML.PresentHiddenField("TypeRowNumber", typeRowNumber));
        HiddenFields.Append(HTML.PresentHiddenField("Module", ModuleName));
        HiddenFields.Append(HTML.PresentHiddenField("ViewPermitPageNo", ViewPermitPageNo));
        HiddenFields.Append(HTML.PresentHiddenField("InspectionsPageNo", InspectionsPageNo));
        HiddenFields.Append(HTML.PresentHiddenField("isRequestPending", isRequestPending));
        HiddenFields.Append(HTML.PresentHiddenField((string)ACAConstant.INSPECTION_IS_READY_TIME_ENABLED, isReadyTimeEnabled));
        HiddenFields.Append(HTML.PresentHiddenField((string)ACAConstant.INSPECTION_SCHEDULING_MANNER, ScheduleManner));
        HiddenFields.Append(HTML.PresentHiddenField((string)ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS, RescheduleRestrictionSettings));
        HiddenFields.Append(HTML.PresentHiddenField((string)ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS, CancellationRestrictionSettings));
        HiddenFields.Append(HTML.PresentHiddenField("InspSeqNum", InspSeqNum));
        HiddenFields.Append(HTML.PresentHiddenField("InspUnits", InspUnits));
        HiddenFields.Append(HTML.PresentHiddenField("InspType", inspType));
        HiddenFields.Append(HTML.PresentHiddenField("InspStatus", inspStatus));
        HiddenFields.Append(HTML.PresentHiddenField("Comments", Comments));
        HiddenFields.Append(HTML.PresentHiddenField("dayOption", GetFieldValue("dayOption", false)));
        HiddenFields.Append(HTML.PresentHiddenField("NextBusinessDateString4WS", GetFieldValue("NextBusinessDateString4WS", false)));
        HiddenFields.Append(HTML.PresentHiddenField("ScheduleOneScreenBreadcrumbIndex", GetFieldValue("ScheduleOneScreenBreadcrumbIndex", false)));
        //HiddenFields.Append(HTML.PresentHiddenField("ContactFirstName", firstNameNew));
        //HiddenFields.Append(HTML.PresentHiddenField("ContactMiddleName", middleNameNew));
        //HiddenFields.Append(HTML.PresentHiddenField("ContactLastName", lastNameNew));
        //HiddenFields.Append(HTML.PresentHiddenField("ContactPhone", phoneNew));
        //HiddenFields.Append(HTML.PresentHiddenField("ContactCountry", countryNew));

        RecordInfo.Append("<Label id=\"pageSectionTitle\">Record No: </Label>");
        RecordInfo.Append("<Label id=\"pageLineText\">");
        RecordInfo.Append(altID);
        RecordInfo.Append("</Label>");
        RecordInfo.Append("<br>");
        RecordInfo.Append("<Label id=\"pageSectionTitle\">Inspection Type: </Label>");
        RecordInfo.Append("<Label id=\"pageLineText\">");
        RecordInfo.Append(inspType);
        RecordInfo.Append("</Label>");
 
        //Get lables and stuff for contact page
 
        string aca_inspection_contact_link_changecontact    = LocalGetTextByKey("aca_inspection_contact_link_changecontact");
        string aca_inspection_contact_label_select          = LabelUtil.RemoveHtmlFormat(LocalGetTextByKey("aca_inspection_contact_label_select"));
        string aca_inspection_contact_label_specify         = LabelUtil.RemoveHtmlFormat(LocalGetTextByKey("aca_inspection_contact_label_specify"));
        string per_peoplelist_firstname                     = LabelUtil.RemoveHtmlFormat(LocalGetTextByKey("per_peoplelist_firstname"));
        string per_peoplelist_middlename                    = LabelUtil.RemoveHtmlFormat(LocalGetTextByKey("per_peoplelist_middlename"));
        string per_peoplelist_lastname                      = LabelUtil.RemoveHtmlFormat(LocalGetTextByKey("per_peoplelist_lastname"));
        string aca_inspection_contact_label_phonenumber     = LabelUtil.RemoveHtmlFormat(LocalGetTextByKey("aca_inspection_contact_label_phonenumber"));
        string aca_inspection_contact_empty_label           = LabelUtil.RemoveHtmlFormat(LocalGetTextByKey("aca_inspection_contact_empty_label"));
      
         iPhonePageTitle = aca_inspection_contact_link_changecontact;
        if (isiPhone != true)
        {
            PageTitle.Append("<div id=\"pageTitle\">" + iPhonePageTitle);
            PageTitle.Append("</div><hr>");
        }
        bool optionNewChecked = checkBoxChoice.Equals("optionNew");
        if (optionNewChecked == false)
        {
            optionNewChecked = (theContactsList.Length == 0);
        }
        ContactsList.Append("<table cellpadding=\"1px\" cellspacing=\"0\" width=\"95%\">");
        if (theContactsList.Length != 0)
        {
            ContactsList.Append("<tr><td width=\"5px\">");
            ContactsList.Append("<input type=\"radio\" " + (optionNewChecked ?  string.Empty : "CHECKED") + " id=\"contactOption\" name=\"contactOption\" value=\"optionExisting\"/>");
            ContactsList.Append("</td><td id=\"pageSectionTitle\">");
            ContactsList.Append(aca_inspection_contact_label_select);
            ContactsList.Append("</td></tr>");
            ContactsList.Append("<tr><td width=\"5px\"></td><td>");
            ContactsList.Append("<select class=\"pageTextInput\" id=\"Choice\" name=\"Choice\">");
            ContactsList.Append(theContactsList);
            ContactsList.Append("</select>");
            ContactsList.Append("</td></tr>");
        }
        else
        {
            ContactsList.Append("<tr><td>");
            ContactsList.Append(aca_inspection_contact_empty_label);
            ContactsList.Append("</td></tr>");
        }
        ContactsList.Append("</table><br>");

        NewContact.Append("<table cellpadding=\"1px\" cellspacing=\"0\" width=\"95%\">");
        NewContact.Append("<tr>");
        NewContact.Append("<td width=\"5px\">");
        NewContact.Append("<input type=\"radio\"  " + (optionNewChecked  ? "CHECKED" : string.Empty) + " id=\"contactOption\" name=\"contactOption\" value=\"optionNew\"/>");
        NewContact.Append("</td><td id=\"pageSectionTitle\">"); //  colspan=\"5\">");
        NewContact.Append(aca_inspection_contact_label_specify);
        NewContact.Append("</td>");
        NewContact.Append("</tr>");

        NewContact.Append("<tr>");

        NewContact.Append("<td width=\"5px\">");
        NewContact.Append("</td>");

        NewContact.Append("<td>");
        NewContact.Append("<table cellpadding=\"1px\" cellspacing=\"0\" width=\"95%\">");
        NewContact.Append("<tr>");
        NewContact.Append("<td id=\"pageText\">");
        NewContact.Append(per_peoplelist_firstname);
        NewContact.Append("</td>");
        NewContact.Append("<td id=\"pageText\">");
        NewContact.Append(per_peoplelist_middlename);
        NewContact.Append("</td>");
        NewContact.Append("<td id=\"pageText\">");
        NewContact.Append(per_peoplelist_lastname);
        NewContact.Append("</td></tr>");
        NewContact.Append("<tr>");
        NewContact.Append("<td>");
        NewContact.Append(" <input type=\"text\" class=\"contactTextInput\" id=\"NewFirstName\" name=\"NewFirstName\" value=\"" + firstNameNew + "\"/> ");
        NewContact.Append("</td><td>");
        NewContact.Append("<input type=\"text\" class=\"contactTextInput\" id=\"NewMiddleName\" name=\"NewMiddleName\" value=\"" + middleNameNew + "\"/> ");
        NewContact.Append("</td><td>");
        NewContact.Append(" <input type=\"text\" class=\"contactTextInput\" id=\"NewLastName\" name=\"NewLastName\" value=\"" + lastNameNew + "\"/> ");
        NewContact.Append("</td>");
        NewContact.Append("</tr>");
        NewContact.Append("</table>");
        NewContact.Append("</td>");

        NewContact.Append("</tr>");
        NewContact.Append("<tr>");

        NewContact.Append("<td width=\"5px\">");
        NewContact.Append("</td>");
        NewContact.Append("<td id=\"pageText\" colspan=\"4\">");
        NewContact.Append(aca_inspection_contact_label_phonenumber);
        NewContact.Append("</td>");

        NewContact.Append("</tr>");
        NewContact.Append("<tr>");

        NewContact.Append("<td width=\"5px\">");
        NewContact.Append("</td>");
        NewContact.Append("<td>");
        NewContact.Append("<table cellpadding=\"1px\" cellspacing=\"0\" width=\"95%\">");
        NewContact.Append("<tr>");
        NewContact.Append("<td class=\"contactTableLiteralCharacters\">");
        NewContact.Append("(");
        NewContact.Append("</td>");
        NewContact.Append("<td style=\"width:10%;\">");
        NewContact.Append("<input type=\"text\"  class=\"contactPhonePrefixInput\" id=\"PhonePrefix\" name=\"PhonePrefix\" value=\"" + countryNew + "\"/>");
        NewContact.Append("</td>");
        NewContact.Append("<td class=\"contactTableLiteralCharacters\">");
        NewContact.Append(")");
        NewContact.Append("</td>");
        NewContact.Append("<td style=\"width=:85%;\">");
        NewContact.Append("<input type=\"text\" class=\"contactPhoneNumberInput\" id=\"PhoneNumber\" name=\"PhoneNumber\" value=\"" + phoneNew + "\"/>");
        NewContact.Append("</td>");
        NewContact.Append("</tr>");
        NewContact.Append("</table>");
        NewContact.Append("</td>");

        NewContact.Append("</tr>");
        NewContact.Append("</table>");

        Buttons.Append("<div id=\"pageSubmitButton\">");
        if (isiPhone == true)
        {
            Buttons.Append("<center>");
        }
        Buttons.Append("<input id=\"button\" name=\"button\" type=\"submit\" value=\"Finished\" />");

        bool contactcollectionOperationSucceeded = false;

        if (buttonChoice != string.Empty)
        {
            StringBuilder aURL = new StringBuilder();
            if (buttonChoice == "Cancel")
            {
                aURL.Append("Inspections.Schedule.aspx?State=" + State);
                aURL.Append(urlProperties);
                aURL.Append("&PagingMode=Y");
                Response.Redirect(aURL.ToString());
                Response.End();
            }
            if (buttonChoice == "Finished")
            {
                if (checkBoxChoice == "optionExisting")
                {
                    string collectionId = GetFieldValue("Choice", false);
                    if (collectionId == string.Empty || collectionId == "--select--")
                    {
                        ErrorMessage.Append(ErrorFormat);
                        ErrorMessage.Append(aca_inspection_contact_empty_label);
                        ErrorMessage.Append(ErrorFormatEnd);
                        contactcollectionOperationSucceeded = false;
                    }
                    else
                    {
                        // get the selected contact first, mid, last, and phone
                        string[] contactItems = collectionId.Split(ACAConstant.SPLIT_CHAR);
                        firstNameNew = contactItems[0].ToString().Trim();
                        middleNameNew = contactItems[1].ToString().Trim();
                        lastNameNew = contactItems[2].ToString().Trim();
                        countryNew = contactItems[3].ToString().Trim();
                        phoneNew = contactItems[4].ToString().Trim();
                        contactcollectionOperationSucceeded = true;
                    }
                }
                else if (checkBoxChoice == "optionNew")
                {
                    // pass the values back
                    contactcollectionOperationSucceeded = true;
                    bool invalidPhoneNumber = false;
                    string validChars = "1234567890";
                    for (int aChar = 0; aChar < countryNew.Length; aChar++)
                    {
                        if (!validChars.Contains(countryNew.Substring(aChar,1)))
                        {
                            invalidPhoneNumber = true;
                            break;
                        }
                    }
                    if (!invalidPhoneNumber)
                    {
                        for (int aChar = 0; aChar < phoneNew.Length; aChar++)
                        {
                            if (!validChars.Contains(phoneNew.Substring(aChar,1)))
                            {
                                invalidPhoneNumber = true;
                                break;
                            }
                        }
                    }
                    if (invalidPhoneNumber)
                    {
                        ErrorMessage.Append(ErrorFormat);
                        ErrorMessage.Append("Phone number must be numeric characters only.  Please re-enter the phone number");
                        ErrorMessage.Append(ErrorFormatEnd);
                        contactcollectionOperationSucceeded = false;
                    }
                }
                if (contactcollectionOperationSucceeded)
                {
                    aURL.Append("Inspections.Schedule.aspx?State=" + State);
                    aURL.Append(urlProperties);
                    aURL.Append("&ContactFirstName=" + firstNameNew);
                    aURL.Append("&ContactMiddleName=" + middleNameNew);
                    aURL.Append("&ContactLastName=" + lastNameNew);
                    aURL.Append("&ContactPhone=" + phoneNew.Replace("+","plus"));
                    aURL.Append("&ContactCountry=" + countryNew.Replace("+","plus"));
                    aURL.Append("&ContactChanged=Y");
                    aURL.Append("&PagingMode=Y");

                    Response.Redirect(aURL.ToString());
                    Response.End();
                }
            }
        }
        // Set page Title
        // And input controls

        Buttons.Append(HTML_EMPTY + HTML_EMPTY + HTML_EMPTY + HTML_EMPTY + "<input id=\"button\" name=\"button\" type=\"submit\" value=\"Cancel\" />");

        if (isiPhone == true)
        {
            Buttons.Append("</center>");
        }
        Buttons.Append("</div>");

        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        bool isBreadcrumbPagingMode = GetFieldValue("PagingMode", false) == "true";
        // These are appended on before the properites list is used:
        StringBuilder sbWork = new StringBuilder();
        sbWork.Append(urlProperties);
        sbWork.Append("ContactFirstName=" + firstName);
        sbWork.Append("ContactMiddleName=" + middleName);
        sbWork.Append("ContactLastName=" + lastName);
        sbWork.Append("ContactPhone=" + thePhone);
        sbWork.Append("ContactCountry=" + country);

        string breadCrumbIndex = GetFieldValue("PageBreadcrumbIndex", false);
        Breadcrumbs = BreadCrumbHelper("InspectionWizardContact.Update.aspx", sbWork, iPhonePageTitle, "", false, false, isBreadcrumbPagingMode, false);
        BackForwardLinks.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));
        HiddenFields.Append(HTML.PresentHiddenField("PageBreadcrumbIndex", CurrentBreadCrumbIndex.ToString()));
    }
    /// <summary>
    /// Get Field value from Form, Request, or QueryString.
    /// </summary>
    /// <param name="FieldName"> Field name </param>
    /// <param name="IsRequired"> Indcates if the user must enterer data into the field or not</param>
    /// <returns>String</returns>
    // DWB - 07-18-2008 - Added new funciton.

    private string GetFieldValue(string FieldName, bool IsRequired)
    {
        string TheValue = string.Empty;
        try
        {
            TheValue = (Request.QueryString[FieldName] != null)
                   ? Request.QueryString[FieldName] : ((Request[FieldName] != null)
                   ? Request.Form[FieldName].ToString() : string.Empty);
        }
        catch
        {
            TheValue = string.Empty;
        }
        if (IsRequired == true && TheValue.ToString() == "")
        {
            // IsRequiredDataValid = false;
        }
        return TheValue;
    }

    /// <summary>
    /// Bind the contact list.
    /// </summary>
    /// <param name="capModel">The cap model.</param>
    private string BindContactList(CapModel4WS capModel)
    {
        StringBuilder contacts = new StringBuilder();

        CapContactModel4WS[] capContactGroups = CapUtil.CombineApplicantToContactsGroup(capModel.applicantModel, capModel.contactsGroup);
        Dictionary<string, string> contactList = new Dictionary<string, string>();
        bool isRTL = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft;
        string contactDisplayPattern = isRTL ? "{2}{1}{0}" : "{0}{1}{2}";

        // add the contact
        if (capContactGroups != null)
        {
            foreach (CapContactModel4WS model in capContactGroups)
            {
                string fullName = UserUtil.FormatToFullName(model.people.firstName, model.people.middleName, model.people.lastName);
                string phone = ModelUIFormat.FormatPhoneShow(model.people.phone1CountryCode, model.people.phone1, model.people.countryCode);
                phone = LabelUtil.RemoveHtmlFormat(phone);
                phone = String.IsNullOrEmpty(phone) ? String.Empty : phone; //String.Format("({0})", phone);
                string space4Contact = String.IsNullOrEmpty(phone) ? String.Empty : " ";

                string text = String.Format(contactDisplayPattern, fullName, space4Contact, phone);
                string value = model.people.firstName + ACAConstant.SPLIT_CHAR + model.people.middleName + ACAConstant.SPLIT_CHAR + model.people.lastName + ACAConstant.SPLIT_CHAR + model.people.phone1CountryCode + ACAConstant.SPLIT_CHAR + model.people.phone1;

                if (!contactList.ContainsKey(value))
                {
                    contactList.Add(value, text);
                }
            }
        }

        LicenseProfessionalModel[] lpModelList = TempModelConvert.ConvertToLicenseProfessionalModelList(capModel.licenseProfessionalList);

        // add the LP contact
        if (lpModelList != null)
        {
            foreach (LicenseProfessionalModel model in lpModelList)
            {
                string fullName = UserUtil.FormatToFullName(model.contactFirstName, model.contactMiddleName, model.contactLastName);
                string phone = ModelUIFormat.FormatPhoneShow(model.phone1CountryCode, model.phone1, model.countryCode);
                phone = LabelUtil.RemoveHtmlFormat(phone);
                phone = String.IsNullOrEmpty(phone) ? String.Empty : phone; // String.Format("({0})", phone);
                string space4Contact = String.IsNullOrEmpty(phone) ? String.Empty : " ";

                string text = String.Format(contactDisplayPattern, fullName, space4Contact, phone);
                string value = model.contactFirstName + ACAConstant.SPLIT_CHAR + model.contactMiddleName + ACAConstant.SPLIT_CHAR + model.contactLastName + ACAConstant.SPLIT_CHAR + model.phone1CountryCode + ACAConstant.SPLIT_CHAR + model.phone1;

                if (!contactList.ContainsKey(value))
                {
                    contactList.Add(value, text);
                }
            }
        }

        string workName = string.Empty;
        string workKey = string.Empty;
        StringBuilder currentContact = new StringBuilder();
        //The current contact may be new contact that is is not attched to the recod as a contact, owner, or 
        //Licensed professional. If so then the current contact info should fill the edit fields.
        //If the current contact is one of the contact attached to the record, then that contact should be 
        //the first record in the drop down list and it should be pre-selected.
        //Determine if the current contact is a new contact, or is a contact that is already attached
        //to the record.
        //To start with treat current contact as though he/she was a record contact.
        if (!(firstNameNew == string.Empty
            && middleNameNew == string.Empty
            && lastNameNew == string.Empty
            && countryNew == string.Empty
            && phoneNew == string.Empty))
        {
            //format the current contact the same way other contacts are fomatted so that 
            //it can be compared correctly.
            string fullName = UserUtil.FormatToFullName(firstNameNew, middleNameNew, lastNameNew);
            string phone = ModelUIFormat.FormatPhoneShow(countryNew, phoneNew, string.Empty);
            phone = LabelUtil.RemoveHtmlFormat(phone);
            phone = String.IsNullOrEmpty(phone) ? String.Empty : phone; // String.Format("({0})", phone);
            string space4Contact = String.IsNullOrEmpty(phone) ? String.Empty : " ";

            workName = String.Format(contactDisplayPattern, fullName, space4Contact, phone);
            workKey = firstNameNew 
                + ACAConstant.SPLIT_CHAR
                + middleNameNew 
                + ACAConstant.SPLIT_CHAR
                + lastNameNew 
                + ACAConstant.SPLIT_CHAR
                + countryNew 
                + ACAConstant.SPLIT_CHAR
                + phoneNew;
            
            //Append contact as the first contact name in the list.
            currentContact.Append("<option value=\"" + workKey + "\">" + workName + "</option>");
        }        // bind ddlContactList
        bool currentContactIsAssocatedToTheRecord = false;
        foreach (string key in contactList.Keys)
        {
            //ddlContactList.Items.Add(new ListItem(contactList[key], key));
            if (key.Equals(workKey))
            {
                //the current contact is in the list of contacts and
                //should not be used to populate the edit fields.
                firstNameNew = string.Empty;
                middleNameNew = string.Empty;
                lastNameNew = string.Empty;
                countryNew = string.Empty;
                phoneNew = string.Empty;
                //Set this to true to make sure the current contact is the
                //first contact in the list.
                currentContactIsAssocatedToTheRecord = true;
             }
            else
            {
                contacts.Append("<option value=\"" + key + "\">" + contactList[key] + "</option>");
            }
        }
        if (currentContactIsAssocatedToTheRecord == false)
        {
            //The current contact is used to populate record edit fields so do
            //not include him in the drop down list.
            currentContact = new StringBuilder();
        }
        currentContact.Append(contacts.ToString());
        return currentContact.ToString();
    }

}
