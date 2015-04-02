/**
* <pre>
* 
*  Accela Citizen Access
*  File: InspectionsSchedule.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2012
* 
*  Description:
*  VO for Additional Information object. 
* 
*  Notes:
*      $Id: InspectionsSchedule.aspx.cs 77905 2007-10-15 12:49:28Z ACHIEVO\Jackie.Yu $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  10-09-2008           DWB                     2008 Mobile ACA 6.7.0 interface redesign
*  03/19/2009           DWB                     Added code format <hr> elements for small devices
*                                               and added code to have failed inspection schedule and
*                                               cancel request return to the ScheduleOneScreen.aspx 
*                                               page so the user can pick a new date (This was a requirement
*                                               for Sac County that was added to ACA.)
*  04/01/2009           Davd Brewster           Modified to display AltID instead of the three segment CAP id.
* </pre>
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Inspection;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

/// <summary>
///  Inspection Schedule 
/// </summary>
public partial class InspectionsSchedule : AccelaPage
{
    public Inspection    ThisInspection = new Inspection();
    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder HiddenFields = new StringBuilder();
    public StringBuilder DateTimeWork = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder RecordInfo = new StringBuilder();
    public StringBuilder SubmitButton = new StringBuilder();
    public StringBuilder ThisContact = new StringBuilder();
    public string BackForwardLinks = string.Empty;
    public string Comments = string.Empty;
    public string PageTitle = string.Empty;
    public string DateTimeValue = string.Empty;
    public string InAdvance = string.Empty;
    public string ThisInspectionDate = string.Empty;

    private const string DATATIME_FORMAT = "MM/dd/yyyy HH:mm:ss";
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // page valid 
        ValidationChecks("Inspections.Schedule.aspx");
        
        string InspectionId = MyProxy.GetFieldValue("InspectionId", false);
        int inspectionID;
        if (!int.TryParse(InspectionId, out inspectionID))
        {
            inspectionID = 0; // this should never happen - 
        }
        string Mode = MyProxy.GetFieldValue("Mode", false);
        string ErrorStyleLeft = "<table cell-padding=\"0\" cell-spacing=\"0\" width=\"100%\"><tr><td valign=\"top\"><img src=\"img\\error.png\"/></td><td style=\"color:#FF6600; font-weight:bold;\">";
        string ErrorStyleRight = "</div></td></tr></table><br>";
        string inspType = MyProxy.GetFieldValue("InspType", false);
        string inspStatus = MyProxy.GetFieldValue("InspStatus", false);
        string ScheduleManner = MyProxy.GetFieldValue(ACAConstant.INSPECTION_SCHEDULING_MANNER, false);
        string PermitNo = MyProxy.GetFieldValue("PermitNo", false);
        string AltID = MyProxy.GetFieldValue("AltID", false);
        string PermitType = MyProxy.GetFieldValue("PermitType", false);
        string SearchBy = MyProxy.GetFieldValue("SearchBy", false);
        string SearchType = MyProxy.GetFieldValue("SearchType", false);
        string ViewPermitPageNo = MyProxy.GetFieldValue("ViewPermitPageNo", false);
        string InspectionsPageNo = MyProxy.GetFieldValue("InspectionsPageNo", false);
        string isRequestPending = MyProxy.GetFieldValue("isRequestPending", false);
        string isReadyTimeEnabled = MyProxy.GetFieldValue(ACAConstant.INSPECTION_IS_READY_TIME_ENABLED, false);
        string RescheduleRestrictionSettings = MyProxy.GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS, false);
        string CancellationRestrictionSettings = MyProxy.GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS, false);
        string Action = MyProxy.GetFieldValue("Action", false);
        string InspSeqNum = MyProxy.GetFieldValue("InspSeqNum", false);
        string InspUnits = MyProxy.GetFieldValue("InspUnits", false);
        string TheComment = MyProxy.GetFieldValue("Comments", false);

        string DisplayNumber = string.Empty;
        string firstName = MyProxy.GetFieldValue("ContactFirstName", false);
        string midName = MyProxy.GetFieldValue("ContactMiddleName", false);
        string lastName = MyProxy.GetFieldValue("ContactLastName", false);
        string phone = MyProxy.GetFieldValue("ContactPhone", false).Replace("plus", "+");
        string country = MyProxy.GetFieldValue("ContactCountry", false).Replace("plus", "+");
        bool pagingMode = MyProxy.GetFieldValue("PagingMode", false) == "Y";
        bool contactChanged = MyProxy.GetFieldValue("ContactChanged", false) == "Y";

        string InspActionType = "Schedule";
        if (RescheduleRestrictionSettings == string.Empty)
        {
            RescheduleRestrictionSettings = MyProxy.GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "0", false);
            RescheduleRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "1", false);
            RescheduleRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "2", false);
            RescheduleRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "3", false);
        }
        if (CancellationRestrictionSettings == string.Empty)
        {
            CancellationRestrictionSettings = MyProxy.GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "0", false);
            CancellationRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "1", false);
            CancellationRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "2", false);
            CancellationRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "3", false);
        }

        RecordInfo.Append("<Label id=\"pageSectionTitle\">Record No: </Label>");
        RecordInfo.Append("<Label id=\"pageLineText\">");
        RecordInfo.Append(AltID);
        RecordInfo.Append("</Label>");
        RecordInfo.Append("<br>");
        RecordInfo.Append("<Label id=\"pageSectionTitle\">Inspection Type: </Label>");
        RecordInfo.Append("<Label id=\"pageLineText\">");
        RecordInfo.Append(inspType);
        RecordInfo.Append("</Label>");

        //get inspection the value
        string typeRowNumber = MyProxy.GetFieldValue("TypeRowNumber", false);
        int rowNumber = typeRowNumber != string.Empty ? int.Parse(typeRowNumber) : int.Parse(MyProxy.GetFieldValue("RowNumber", false));
        InspectionViewModel listRow = null;
        InspectionTypeDataModel typeRow = null;
 
        CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
        if (capModel.addressModel != null && capModel.addressModel.displayAddress != null)
        {
            RecordInfo.Append("<br>");
            RecordInfo.Append("<Label id=\"pageSectionTitle\">Location: </Label>");
            RecordInfo.Append("<br><Label id=\"pageLineText\">");
            RecordInfo.Append(capModel.addressModel.displayAddress);
            RecordInfo.Append("</Label>");
        }        
        if (typeRowNumber != string.Empty)
        {
            List<InspectionTypeDataModel> inspectionTypeModels = (List<InspectionTypeDataModel>)Session["AMCA_WIZARD_INSPECTIONS"];
            typeRow = inspectionTypeModels[rowNumber];
            if (capModel.licenseProfessionalModel != null)
            {
                if (!contactChanged)
                {
                    firstName = capModel.licenseProfessionalModel.contactFirstName != null ? capModel.licenseProfessionalModel.contactFirstName : string.Empty;
                    midName = capModel.licenseProfessionalModel.contactFirstName != null ? capModel.licenseProfessionalModel.contactMiddleName : string.Empty;
                    lastName = capModel.licenseProfessionalModel.contactFirstName != null ? capModel.licenseProfessionalModel.contactLastName : string.Empty;
                    phone = capModel.licenseProfessionalModel.phone1 != null ? capModel.licenseProfessionalModel.phone1 : string.Empty;
                    country = capModel.licenseProfessionalModel.phone1CountryCode != null ? capModel.licenseProfessionalModel.phone1CountryCode : string.Empty;
                }
          }
        }
        else
        {
            List<InspectionViewModel> inspectionViewModels = (List<InspectionViewModel>)Session["AMCA_INSPECTION_MODELS"];
            for (rowNumber = 0; rowNumber < inspectionViewModels.Count; rowNumber++)
            {
                if (inspectionID == inspectionViewModels[rowNumber].ID)
                {
                    if (contactChanged == false)
                    {
                        listRow = inspectionViewModels[rowNumber];
                        firstName = listRow.ContactFirstName;
                        midName = listRow.ContactMiddleName;
                        lastName = listRow.ContactLastName;
                        phone = listRow.ContactPhoneNumber;
                        country = listRow.ContactPhoneIDD;
                    }
                    break;
                }
            }
            ThisInspection = ConvertUtil.convertInspectionDataModelToInspection(inspectionViewModels[rowNumber].InspectionDataModel);
        }

        string scheduledDateTime = MyProxy.GetFieldValue("scheduledDateTime", false);
        string endScheduledDateTime = MyProxy.GetFieldValue("endScheduledDateTime", false);
        string finishTimeOption = MyProxy.GetFieldValue("finishTimeOption", false);
        string rDayOption=MyProxy.GetFieldValue("rDayOption", false);
        
        if (Mode != "")
        {
            InspActionType = isRequestPending != ACAConstant.COMMON_Y ? "Schedule" : "Request";
            string textTBD = LabelUtil.GetTextByKey("ACA_Inspection_BlankInspector_TBD", ModuleName);
            string strDate = textTBD, strTime = textTBD;

            if (!string.IsNullOrEmpty(scheduledDateTime))
            {
                DateTime fActivityDate = I18nDateTimeUtil.ParseFromWebService(scheduledDateTime);

                if(fActivityDate!=DateTime.MinValue)
                {
                    strDate = fActivityDate.ToString("MM/dd/yyyy");

                    if (fActivityDate.ToString("HH:mm") != "00:00")
                    {
                        strTime = fActivityDate.ToString("hh:mm tt");
                    }
                }
            }

            ThisInspectionDate = string.Format("{0} at {1}", strDate, strTime);

            Comments = TheComment;
        }

        //get inspection id
        ThisInspection.Id = InspectionId;
        ThisInspection.PermitNo = PermitNo;
        ThisInspection.ModuleName = ModuleName;

        //retrieves inspections
        ThisInspection = MyProxy.InspectionRetrieve(ThisInspection);
       
        DisplayNumber = ((AltID != string.Empty) ? AltID : PermitNo);
        if (MyProxy.OnErrorReturn)
        {  // Proxy Exception 
            ErrorMessage.Append(MyProxy.ExceptionMessage);
        }

        ThisInspection.Date = DateTimeValue;
        if (ThisInspection.Type == null || ThisInspection.Type == string.Empty)
        {
            ThisInspection.Type = inspType;
        }
        if (ThisInspection.Status == null || ThisInspection.Status == string.Empty)
        {
            ThisInspection.Status = inspStatus;
        }
        ThisInspection.RequestorCountry = country;
        ThisInspection.RequestorFirstName = firstName;
        ThisInspection.RequestorLastName = lastName;
        ThisInspection.RequestorMiddleName = midName;
        ThisInspection.RequestorCountry = country;
        ThisInspection.RequestorPhone = phone;

        Action = MyProxy.GetFieldValue("Action", false);
        string breadCrumbIndex = MyProxy.GetFieldValue("BreadCrumbIndex", false);
        bool isElipseLink = MyProxy.GetFieldValue("IsElipseLink", false) != string.Empty;
        StringBuilder sbWork = new StringBuilder();
        string[] resceduleRestrictions = RescheduleRestrictionSettings.Split('|');
        string[] cancelRestrictions = CancellationRestrictionSettings.Split('|');

        //from schedule one screen
        sbWork.Append("&InspectionId=" + InspectionId);
        sbWork.Append("&scheduledDateTime=" + scheduledDateTime);
        sbWork.Append("&endScheduledDateTime=" + endScheduledDateTime);
        sbWork.Append("&finishTimeOption=" + finishTimeOption);
        sbWork.Append("&rDayOption=" + rDayOption);
        sbWork.Append("&PermitNo=" + PermitNo);
        sbWork.Append("&InAdavnce" + InAdvance);
        sbWork.Append("&Action=" + Action);
        sbWork.Append("&AltID=" + AltID);
        sbWork.Append("&PermitType=" + PermitType);
        sbWork.Append("&SearchBy=" + SearchBy);
        sbWork.Append("&SearchType=" + SearchType);
        sbWork.Append("&InspectionPageNo=" + InspectionsPageNo);
        sbWork.Append("&ViewPermitPageNo=" + ViewPermitPageNo);
        sbWork.Append("&Mode=" + Mode);
        sbWork.Append("&Module=" + ModuleName);
        sbWork.Append("&RowNumber=" + rowNumber);
        sbWork.Append("&TypeRowNumber=" + typeRowNumber);
        sbWork.Append("&InspUnits=" + InspUnits);
        sbWork.Append("&InspSeqNum=" + InspSeqNum);
        sbWork.Append("&" + ACAConstant.INSPECTION_IN_ADVANCE + "=" + InAdvance); //Not populated on this page always empty.
        sbWork.Append("&" + ACAConstant.INSPECTION_SCHEDULING_MANNER + "=" + ScheduleManner);
        sbWork.Append("&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "0=" + resceduleRestrictions[0]);
        sbWork.Append("&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "1=" + resceduleRestrictions[1]);
        sbWork.Append("&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "2=" + resceduleRestrictions[2]);
        sbWork.Append("&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "3=" + resceduleRestrictions[3]);
        sbWork.Append("&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "0=" + cancelRestrictions[0]);
        sbWork.Append("&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "1=" + cancelRestrictions[1]);
        sbWork.Append("&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "2=" + cancelRestrictions[2]);
        sbWork.Append("&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "3=" + cancelRestrictions[3]);
        sbWork.Append("&InspType=" + inspType);
        sbWork.Append("&InspStatus=" + inspStatus);
        sbWork.Append("&ScheduleOneScreenBreadcrumbIndex=" +  MyProxy.GetFieldValue("ScheduleOneScreenBreadcrumbIndex", false));
        sbWork.Append("&" + (string)ACAConstant.INSPECTION_IS_READY_TIME_ENABLED + "=" + isReadyTimeEnabled);
        sbWork.Append("&isRequestPending=" + MyProxy.GetFieldValue("isRequestPending",false));
        sbWork.Append("&dayOption=" + MyProxy.GetFieldValue("dayOption", false));
        sbWork.Append("&Comments=" + Comments);
        sbWork.Append("&ContactFirstName=" + firstName);
        sbWork.Append("&ContactMiddleName=" + midName);
        sbWork.Append("&ContactLastName=" + lastName);
        sbWork.Append("&ContactPhone=" + phone.Replace("+","plus"));
        sbWork.Append("&ContactCountry=" + country.Replace("+","plus"));
        sbWork.Append("&ContactChanged=" + (contactChanged ? "Y" : "N"));
        iPhonePageTitle = "Review";
        if (isiPhone == false)
        {
            PageTitle = "<div id=\"pageTitle\">" + InspActionType + " Inspection - Confirmation</div><hr />";
        }
        if (pagingMode)
        {
            breadCrumbIndex =  "-2";
        }
        Breadcrumbs = BreadCrumbHelper("Inspections.Schedule.aspx", sbWork, "Confirmation", breadCrumbIndex, isElipseLink, pagingMode, pagingMode, true);
        BackForwardLinks = BackLinkHelper(CurrentBreadCrumbIndex.ToString());

        // Contact Page link
        var inspectionPermissionBll = ObjectFactory.GetObject<IInspectionPermissionBll>();
        bool isContactVisible = inspectionPermissionBll.CheckContactRight(capModel, capModel.capID.serviceProviderCode, ModuleName, ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_VIEW_CONTACT);
        bool isContactChangeOk = inspectionPermissionBll.CheckContactRight(capModel, capModel.capID.serviceProviderCode, ModuleName, ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_INPUT_CONTACT);

        StringBuilder contactPageLink = new StringBuilder();
        if (isContactChangeOk)
        {
            contactPageLink.Append("<a href=\"");
            contactPageLink.Append("InspectionWizardContacts.aspx?State=" + State);
            contactPageLink.Append(sbWork);
            contactPageLink.Append("\">");
            contactPageLink.Append("Change");
            contactPageLink.Append("</a>");
        }
        if (isContactVisible)
        {
            bool isRTL = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft;
            ThisContact.Append("<div id=\"pageSectionTitle\">");
            ThisContact.Append("<label>Contact: </label>");
            ThisContact.Append("<span id=\"pageLineText\">");

            ThisContact.Append(contactPageLink.ToString());
            ThisContact.Append("<div id=\"pageTextIndented\">");
            if (isRTL)
            {
                ThisContact.Append(lastName + " ");
                ThisContact.Append(midName + " ");
                ThisContact.Append(firstName);
            }
            else
            {
                ThisContact.Append(firstName + " ");
                ThisContact.Append(midName + " ");
                ThisContact.Append(lastName);
            }
            if (phone != string.Empty && (firstName != string.Empty || midName != string.Empty || lastName != string.Empty))
            {
                ThisContact.Append("<br>");
            }
            string phoneWork = ModelUIFormat.FormatPhoneShow(country, phone, string.Empty);
            phoneWork = LabelUtil.RemoveHtmlFormat(phoneWork);
            phoneWork = String.IsNullOrEmpty(phoneWork) ? String.Empty : phoneWork;
            ThisContact.Append(phoneWork);
            ThisContact.Append("</div>");
            ThisContact.Append("</span>");
            ThisContact.Append("</div>");
       }

        // Save for next form
        HiddenFields.Append(HTML.PresentHiddenField("InspectionId", InspectionId));
        HiddenFields.Append(HTML.PresentHiddenField("scheduledDateTime", scheduledDateTime));
        HiddenFields.Append(HTML.PresentHiddenField("endScheduledDateTime", endScheduledDateTime));
        HiddenFields.Append(HTML.PresentHiddenField("finishTimeOption", finishTimeOption));
        HiddenFields.Append(HTML.PresentHiddenField("rDayOption", rDayOption));
        HiddenFields.Append(HTML.PresentHiddenField("PermitNo", PermitNo));
        HiddenFields.Append(HTML.PresentHiddenField("AltID", AltID));
        HiddenFields.Append(HTML.PresentHiddenField("PermitType", PermitType));
        HiddenFields.Append(HTML.PresentHiddenField("SearchBy", SearchBy));
        HiddenFields.Append(HTML.PresentHiddenField("SearchType", SearchType));
        HiddenFields.Append(HTML.PresentHiddenField("Mode", Mode));
        HiddenFields.Append(HTML.PresentHiddenField("Action", Action));
        HiddenFields.Append(HTML.PresentHiddenField("RowNumber", rowNumber.ToString()));
        HiddenFields.Append(HTML.PresentHiddenField("TypeRowNumber", typeRowNumber));
        HiddenFields.Append(HTML.PresentHiddenField("Module", ModuleName));
        HiddenFields.Append(HTML.PresentHiddenField("ViewPermitPageNo", ViewPermitPageNo));
        HiddenFields.Append(HTML.PresentHiddenField("InspectionsPageNo", InspectionsPageNo));
        HiddenFields.Append(HTML.PresentHiddenField((string)ACAConstant.INSPECTION_SCHEDULING_MANNER, ScheduleManner));
        HiddenFields.Append(HTML.PresentHiddenField((string)ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS, RescheduleRestrictionSettings));
        HiddenFields.Append(HTML.PresentHiddenField((string)ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS, CancellationRestrictionSettings));
        HiddenFields.Append(HTML.PresentHiddenField("InspSeqNum", InspSeqNum));
        HiddenFields.Append(HTML.PresentHiddenField("InspUnits", InspUnits));
        HiddenFields.Append(HTML.PresentHiddenField("InspType", inspType));
        HiddenFields.Append(HTML.PresentHiddenField("InspStatus", inspStatus));
        HiddenFields.Append(HTML.PresentHiddenField("ContactFirstName", firstName));
        HiddenFields.Append(HTML.PresentHiddenField("ContactMiddleName", midName));
        HiddenFields.Append(HTML.PresentHiddenField("ContactLastName", lastName));
        HiddenFields.Append(HTML.PresentHiddenField("ContactPhone", phone));
        HiddenFields.Append(HTML.PresentHiddenField("ContactCountry", country));
        HiddenFields.Append(HTML.PresentHiddenField("Comments", Comments));
        HiddenFields.Append(HTML.PresentHiddenField("dayOption", MyProxy.GetFieldValue("dayOption", false)));
        HiddenFields.Append(HTML.PresentHiddenField("ScheduleOneScreenBreadcrumbIndex", MyProxy.GetFieldValue("ScheduleOneScreenBreadcrumbIndex", false)));
        HiddenFields.Append(HTML.PresentHiddenField((string)ACAConstant.INSPECTION_IS_READY_TIME_ENABLED, isReadyTimeEnabled));
        HiddenFields.Append(HTML.PresentHiddenField("InspectionDateTime", ThisInspectionDate));

        if (ErrorMessage.ToString() == string.Empty) 
        {
            if (isiPhone == true)
            {
                SubmitButton.Append("<center>");
            }
            SubmitButton.Append("<input id=\"Submit1\" type=\"submit\" value=\"Continue\" />");
            if (isiPhone == true)
            {
                SubmitButton.Append("</center>");
            }
        }
    }
}
