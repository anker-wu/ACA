/**
* <pre>
* 
*  Accela Citizen Access
*  File: InspectionsConfirmations.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2012
* 
*  Description:
*  VO for Additional Information object. 
* 
*  Notes:
*      $Id: InspectionsConfirmations.aspx.cs 77905 2007-10-15 12:49:28Z ACHIEVO\Jackie.Yu $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  10-09-2008           DWB                     2008 Mobile ACA 6.7.0 interface redesign
*  03/19/2009           DWB                     Added code format <hr> elements for small devices
*                                               and added code to have failed inspection schedule and
*                                               cancel request return to the ScheduleOneScreen.aspx 
*                                               page so the user can pick a new date (This was a requirement
*                                               for Sac County that was added to ACA.)
*  04/01/2009           Davd Brewster           Modified to display AltID instead of the three segment CAP id.
*  10-10-2009           Dave Brewster           Added new breadcrumb logic.
* </pre>
*/
using System;
using System.Collections.Generic;
using System.Text;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Inspection;
using Accela.ACA.WSProxy;

/// <summary>
/// Inspection Confirmations
/// </summary>
public partial class InspectionsConfirmations : AccelaPage
{
    public StringBuilder OutputLinks = new StringBuilder();
    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder Restrictions = new StringBuilder();
    public StringBuilder RecordInfo = new StringBuilder();
    public StringBuilder ActionResult = new StringBuilder();
    public StringBuilder ActionMessage = new StringBuilder();
    public StringBuilder TheDate = new StringBuilder();
    public StringBuilder TheTime = new StringBuilder();
    public StringBuilder TheComment = new StringBuilder();

    public string ScheduleTime = string.Empty;
    public string PageTitle = string.Empty;
    public string ScheduleDate = string.Empty;
    public string Comments = string.Empty;
    public string Mode = string.Empty;
    public string TheType = string.Empty;
    private string PermitNo = string.Empty;
    private string PermitType = string.Empty;
    private string SearchType = string.Empty;
    private string SearchBy = string.Empty;
    private string PrevMode = string.Empty;
    public string InspectionDateTime = string.Empty;
    private string ViewPermitPageNo = string.Empty;  // ResultPage for "View Permits" breadcrumb link.
    private string InspectionsPageNo = string.Empty; // ResultPage for "View Permits > Inspections" Breadcrumb link
    private string InspSeqNum = string.Empty;
    private string InAdvance = string.Empty;
    private string InspUnits = string.Empty;
    private string AltID = string.Empty;
    private string ScheduleManner = string.Empty;
    private string isReadyTimeEnabled = string.Empty;

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
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Mode = MyProxy.GetFieldValue("Action", false);
        iPhonePageTitle = Mode;
        ValidationChecks("Inspections.Confirmations.aspx");

        PermitNo = MyProxy.GetFieldValue("PermitNo", false);
        AltID = MyProxy.GetFieldValue("AltID", false);
        PermitType = MyProxy.GetFieldValue("PermitType", false);
        SearchBy = MyProxy.GetFieldValue("SearchBy", false);
        SearchType = MyProxy.GetFieldValue("SearchType", false);
        ViewPermitPageNo = MyProxy.GetFieldValue("ViewPermitPageNo", false);
        InspectionsPageNo = MyProxy.GetFieldValue("InspectionsPageNo", false);
        PrevMode = MyProxy.GetFieldValue("Mode", false);
        Comments = MyProxy.GetFieldValue("Comments", false);
        InspectionDateTime = MyProxy.GetFieldValue("InspectionDateTime", false);
        InspSeqNum = MyProxy.GetFieldValue("InspSeqNum", false);
        InAdvance = MyProxy.GetFieldValue(ACAConstant.INSPECTION_IN_ADVANCE, false);
        InspUnits = MyProxy.GetFieldValue("InspUnits", false);
        ScheduleManner = MyProxy.GetFieldValue(ACAConstant.INSPECTION_SCHEDULING_MANNER, false);
        string InspectionId = MyProxy.GetFieldValue("InspectionId", false);
        int inspectionID = 0;
        int.TryParse(InspectionId, out inspectionID);

        string inspType = MyProxy.GetFieldValue("InspType", false);
        string inspStatus = MyProxy.GetFieldValue("InspStatus", false);
        string RescheduleRestrictionSettings = MyProxy.GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS, false);
        string firstName = MyProxy.GetFieldValue("ContactFirstName", false);
        string midName = MyProxy.GetFieldValue("ContactMiddleName", false);
        string lastName = MyProxy.GetFieldValue("ContactLastName", false);
        string phone = MyProxy.GetFieldValue("ContactPhone", false);
        string country = MyProxy.GetFieldValue("ContactCountry", false);

        if (RescheduleRestrictionSettings == string.Empty)
        {
            RescheduleRestrictionSettings = MyProxy.GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "0", false);
            RescheduleRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "1", false);
            RescheduleRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "2", false);
            RescheduleRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "3", false);
        }
        string CancellationRestrictionSettings = MyProxy.GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS, false);
        if (CancellationRestrictionSettings == string.Empty)
        {
            CancellationRestrictionSettings = MyProxy.GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "0", false);
            CancellationRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "1", false);
            CancellationRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "2", false);
            CancellationRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "3", false);
        }

        string RescheduleRestrictions = BuildRestrictionPolicy(RescheduleRestrictionSettings, "Reschedule");
        string CancelRestrictions = BuildRestrictionPolicy(CancellationRestrictionSettings, "Cancel");
        string ReschedulePolicyTitle = LabelUtil.GetTextByKey("per_scheduleinspection_label_actionpolicytitle", ModuleName);
        string finishedBreadCrumbIndex = MyProxy.GetFieldValue("ScheduleOneScreenBreadcrumbIndex", false);

        //get inspection the value
        string typeRowNumber = MyProxy.GetFieldValue("TypeRowNumber", false);
        int rowNumber = typeRowNumber != string.Empty ? int.Parse(typeRowNumber) : int.Parse(MyProxy.GetFieldValue("RowNumber", false));
        InspectionViewModel listRow = null;
        InspectionTypeDataModel typeRow = null;
        InspectionScheduleType scheduleType;
        bool inspectionRequired = false;
        string inspectionGroup = string.Empty;

        if (typeRowNumber != string.Empty)
        {
            List<InspectionTypeDataModel> inspectionTypeModels = (List<InspectionTypeDataModel>)Session["AMCA_WIZARD_INSPECTIONS"];
            typeRow = inspectionTypeModels[rowNumber];
            isReadyTimeEnabled = typeRow.ReadyTimeEnabled ? "Y" : "N";
            scheduleType = typeRow.ScheduleType;
            inspectionRequired = typeRow.Required;
            inspectionGroup = typeRow.Group;
        }
        else
        {
            List<InspectionViewModel> inspectionViewModels = (List<InspectionViewModel>)Session["AMCA_INSPECTION_MODELS"];

            for (rowNumber = 0; rowNumber < inspectionViewModels.Count; rowNumber++)
            {
                if (inspectionID == inspectionViewModels[rowNumber].ID)
                {
                    listRow = inspectionViewModels[rowNumber];
                    break;
                }
            }
            isReadyTimeEnabled = listRow.InspectionDataModel.ReadyTimeEnabled ? "Y" : "N";
            scheduleType = listRow.InspectionDataModel.ScheduleType;
            inspectionRequired = listRow.InspectionDataModel.Required;
            inspectionGroup = listRow.InspectionDataModel.Group;
        }

        ActionResult.Append(" - Successful");
        iPhonePageTitle = "Successful";

        InspectionParameter inspectionParameter = new InspectionParameter();

        string strScheduledDateTime = MyProxy.GetFieldValue("scheduledDateTime", false);
        DateTime scheduledDateTime = DateTime.MinValue;
        if (I18nDateTimeUtil.TryParseFromWebService(strScheduledDateTime, out scheduledDateTime))
        {
            inspectionParameter.ScheduledDateTime = scheduledDateTime;
        }

        string strEndScheduledDateTime = MyProxy.GetFieldValue("endScheduledDateTime", false);
        DateTime endScheduledDateTime = DateTime.MinValue;
        if (I18nDateTimeUtil.TryParseFromWebService(strEndScheduledDateTime, out endScheduledDateTime))
        {
            inspectionParameter.EndScheduledDateTime = endScheduledDateTime;
        }

        CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
        CapIDModel recordIDModel = TempModelConvert.Trim4WSOfCapIDModel(capModel.capID);
        string capIDAgency = recordIDModel.serviceProviderCode;
        inspectionParameter.RecordID1 = recordIDModel.ID1;
        inspectionParameter.RecordID2 = recordIDModel.ID2;
        inspectionParameter.RecordID3 = recordIDModel.ID3;
        inspectionParameter.AgencyCode = recordIDModel.serviceProviderCode;

        inspectionParameter.ModuleName = ModuleName;
        inspectionParameter.ScheduleType = scheduleType;
        inspectionParameter.Action = EnumUtil<InspectionAction>.Parse(Mode, InspectionAction.None);
        inspectionParameter.ReadyTimeEnabled = (isReadyTimeEnabled == ACAConstant.COMMON_Y ? true : false);
        string finishTimeOption = MyProxy.GetFieldValue("finishTimeOption", false);
        inspectionParameter.TimeOption = EnumUtil<InspectionTimeOption>.Parse(finishTimeOption, InspectionTimeOption.Unknow);
        inspectionParameter.Type = inspType;
        inspectionParameter.InAdvance = (InAdvance == ACAConstant.COMMON_Y ? true : false);
        inspectionParameter.ContactFirstName = firstName;
        inspectionParameter.ContactMiddleName = midName;
        inspectionParameter.ContactLastName = lastName;
        inspectionParameter.ID = inspectionID.ToString();
        inspectionParameter.TypeID = InspSeqNum;
        inspectionParameter.Required = inspectionRequired;
        inspectionParameter.ContactPhoneIDD = country;
        inspectionParameter.ContactPhoneNumber = phone;
        inspectionParameter.Group = inspectionGroup;

        #region updates the schedule/ reschedule/dispute/cancel of inspection details through proxy

        RecordInfo.Append("<Label id=\"pageSectionTitle\">Record No: </Label>");
        RecordInfo.Append("<Label id=\"pageLineText\">");
        RecordInfo.Append(AltID);
        RecordInfo.Append("</Label>");
        RecordInfo.Append("<br/>");
        RecordInfo.Append("<Label id=\"pageSectionTitle\">Inspection Type: </Label>");
        RecordInfo.Append("<Label id=\"pageLineText\">");
        RecordInfo.Append(inspType);
        RecordInfo.Append("</Label>");

        if (capModel.addressModel != null && capModel.addressModel.displayAddress != null)
        {
            RecordInfo.Append("<br>");
            RecordInfo.Append("<Label id=\"pageSectionTitle\">Location: </Label>");
            RecordInfo.Append("<br><Label id=\"pageLineText\">");
            RecordInfo.Append(capModel.addressModel.displayAddress);
            RecordInfo.Append("</Label>");
        }
        TheType = "<div style=\"margin-top:0px; margin-bottom:6px;\"><b>Type: </b>" + inspType + "</div>";
        Boolean bRedirectToScheduleOneDayPage = false;

        InspectionProxy MyInspectionProxy = new InspectionProxy();

        string breadCrumbIndex = string.Empty;
        switch (Mode)
        {
            case "Request":
            case ACAConstant.INSPECTION_ACTION_SCHEDULE:
                MyInspectionProxy.ScheduleInspection(inspectionParameter, capIDAgency, ModuleName, Comments);

                if (MyInspectionProxy.OnErrorReturn)
                {  // Proxy Exception 
                    ActionResult.Append(" - Failed");
                    iPhonePageTitle = "Failed";
                    ErrorMessage.Append(MyInspectionProxy.ExceptionMessage);
                    bRedirectToScheduleOneDayPage = true;
                }

                breadCrumbIndex = Session["AMCA_BREADCRUMB_ADJUSTMENT"].ToString();
                break;

            case ACAConstant.INSPECTION_ACTION_RESCHEDULE:
                MyInspectionProxy.ScheduleInspection(inspectionParameter, capIDAgency, ModuleName, Comments);

                if (MyInspectionProxy.OnErrorReturn)
                {  // Proxy Exception 
                    ActionResult.Append(" - Failed");
                    iPhonePageTitle = "Failed";
                    ErrorMessage.Append(MyInspectionProxy.ExceptionMessage);
                    bRedirectToScheduleOneDayPage = true;
                }

                breadCrumbIndex = "-2";
                break;
            case ACAConstant.INSPECTION_ACTION_CANCEL:
                MyInspectionProxy.CancelInspectionUpdate(inspectionParameter, capIDAgency);
                TheType = string.Empty;

                if (MyInspectionProxy.OnErrorReturn)
                {  // Proxy Exception 
                    ActionResult.Append(" - Failed");
                    iPhonePageTitle = "Failed";
                    ErrorMessage.Append(MyInspectionProxy.ExceptionMessage);
                }
                break;
        }

        #endregion

        if (ErrorMessage.Length == 0)
        {
            ActionMessage.Append("<div id=\"pageText\">");

            string messageDefault = "The inspection has been successfully ";
            bool showRestrictionsPolicies = false;

            if (Mode == "Schedule")
            {
                string message = LabelUtil.GetGlobalTextByKey("per_inspectionSuccess_text_success");

                if (string.IsNullOrEmpty(message))
                {
                    message = messageDefault + "scheduled.";
                }

                ActionMessage.Append(message);
                showRestrictionsPolicies = true;
            }
            else if (Mode == "Cancel")
            {
                ActionMessage.Append(messageDefault);
                ActionMessage.Append("cancelled.");
            }
            else if (Mode == "Request")
            {
                string message = LabelUtil.GetGlobalTextByKey("per_inspectionSuccess_text_reqyestsuccess");

                if (string.IsNullOrEmpty(message))
                {
                    message = messageDefault + "requested.";
                }

                ActionMessage.Append(message);
            }
            else
            {
                string message = LabelUtil.GetGlobalTextByKey("per_inspectionSuccess_text_success");
                ActionMessage.Append(message + "scheduled.");
                showRestrictionsPolicies = true;
            }
            ActionMessage.Append("</div>");

            if (showRestrictionsPolicies && (CancelRestrictions != string.Empty || RescheduleRestrictions != string.Empty))
            {
                Restrictions.Append("<div id=\"pageText\">");
                Restrictions.Append("<div id=\"pageSectionTitle\">");
                Restrictions.Append(ReschedulePolicyTitle);
                Restrictions.Append("</div>");

                if (RescheduleRestrictions != string.Empty)
                {
                    Restrictions.Append("<div id=\"pageText\">");
                    Restrictions.Append(RescheduleRestrictions);
                    Restrictions.Append("</div>");
                }

                if (CancelRestrictions != string.Empty)
                {
                    Restrictions.Append("<div id=\"pageText\">");
                    Restrictions.Append(CancelRestrictions + "<br>");
                    Restrictions.Append("</div>");
                }
            }
        }

        if (bRedirectToScheduleOneDayPage)
        {
            String sRedirect = String.Empty;

            sRedirect = "Inspections.ScheduleOneScreen.aspx?State=" + State
            + "&Id=" + InspectionId
            + "&InspectionId=" + InspectionId
            + "&PermitNo=" + PermitNo
            + "&AltID=" + AltID
            + "&PermitType=" + PermitType
            + "&" + ACAConstant.INSPECTION_IN_ADVANCE + "=" + InAdvance
            + "&SearchBy=" + SearchBy
            + "&SearchType=" + SearchType
            + "&ViewPermitPageNo=" + ViewPermitPageNo
            + "&InspectionsPageNo=" + InspectionsPageNo
            + "&Mode=" + PrevMode
            + "&InspSeqNum=" + InspSeqNum
            + "&InspUnits=" + InspUnits
            + "&Module=" + ModuleName
            + "&InspType=" + inspType
            + "&InspStatuss=" + inspStatus
            + "&" + ACAConstant.INSPECTION_SCHEDULING_MANNER + "=" + ScheduleManner
            + "&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "=" + RescheduleRestrictionSettings
            + "&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "=" + CancellationRestrictionSettings
            + "&ScheduleOneScreenBreadcrumbIndex=" + MyProxy.GetFieldValue("ScheduleOneScreenBreadcrumbIndex", false)
            + "&ErrorMsg=" + ErrorMessage.ToString();

            Response.Redirect(sRedirect);
            Response.End();
            return;
        }

        if (TheType != string.Empty)
        {
            //secduledate
            if (scheduledDateTime != DateTime.MinValue)
            {
                TheDate.Append("<label id=\"pageSectionTitle\">Date: </label>");
                TheDate.Append("<label id=\"pageLineText\">" + scheduledDateTime.ToLongDateString() + "</label><br>");

                TheTime.Append("<label id=\"pageSectionTitle\">Time: </label>");
                TheTime.Append("<label id=\"pageLineText\">");
                TheTime.Append(scheduledDateTime.ToShortTimeString());
                TheTime.Append("</label><br>");
            }
            TheComment.Append("<label id=\"pageSectionTitle\">Comments: </Label><br>");
            TheComment.Append("<label id=\"inspectionLineComment\">");
            TheComment.Append((Comments.Length > 0) ? Comments.ToString() : string.Empty);
            TheComment.Append("</label><br>");
        }

        bool isElipseLink = MyProxy.GetFieldValue("IsElipseLink", false) != string.Empty;

        StringBuilder sbWork = new StringBuilder();
        sbWork.Append("&PermitNo=" + PermitNo.ToString());

        if (isiPhone == false)
        {
            PageTitle = "<div id=\"pageTitle\">" + Mode + " " + ActionResult + "</div><hr />";
        }

        Breadcrumbs = BreadCrumbHelper("Inspection.Confirm.aspx", sbWork, "Confirmation Page", breadCrumbIndex, isElipseLink, true, false, true);
        OutputLinks = new StringBuilder();
        OutputLinks.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));
    }

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
                    string restrictionPattern = LabelUtil.GetTextByKey("per_scheduleinspection_label_restrictionpolicy1", ModuleName);
                    result = string.Format(restrictionPattern, actionString, daysPrior);
                }
            }
            else if (RESTRICTION_OPTION_HOURSPRIOR.Equals(restrictionOption, StringComparison.InvariantCultureIgnoreCase))
            {
                int hours = 0;
                int.TryParse(hoursPrior, out hours);
                if (hours > 0)
                {
                    string restrictionPattern = LabelUtil.GetTextByKey("per_scheduleinspection_label_restrictionpolicy2", ModuleName);
                    result = string.Format(restrictionPattern, actionString, hoursPrior);
                }
            }
            else if (RESTRICTION_OPTION_DAYSPRIORATTIME.Equals(restrictionOption, StringComparison.InvariantCultureIgnoreCase))
            {
                int days = 0;
                int.TryParse(daysPrior, out days);
                if (days > 0)
                {
                    string restrictionPattern = LabelUtil.GetTextByKey("per_scheduleinspection_label_restrictionpolicy3", ModuleName);
                    result = string.Format(restrictionPattern, actionString, atTime, daysPrior);
                }
                else
                {
                    string restrictionPattern = LabelUtil.GetTextByKey("per_scheduleinspection_label_restrictionpolicy4", ModuleName);
                    result = string.Format(restrictionPattern, actionString, atTime);
                }
            }
        }

        return result;
    }
}
