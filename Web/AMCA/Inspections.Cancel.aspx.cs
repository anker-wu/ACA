/**
* <pre>
* 
*  Accela Citizen Access
*  File: InspectionsCancel.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2012
* 
*  Description:
*  VO for Additional Information object. 
* 
*  Notes:
*      $Id: InspectionsCancel.aspx.cs 77905 2007-10-15 12:49:28Z ACHIEVO\Jackie.Yu $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  10-09-2008           DWB                     2008 Mobile ACA 6.7.0 interface redesign
*  04/01/2009           Dave Brewster           Modiifed to display AltID instetad of Cap ID.
*  10-10-2009           Dave Brewster           Added new breadcrumb logic.
* </pre>
*/
using System;
using System.Collections.Generic;
using System.Text;
using Accela.ACA.Common;
using Accela.ACA.Web.Inspection;

/// <summary>
/// 
/// </summary>
public partial class InspectionsCancel : AccelaPage
{
    public Inspection ThisInspection = new Inspection();
    public string Comments = string.Empty;
    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public string BackForwardLinks = string.Empty;
    public string InspectionDateTime = string.Empty;
    public string NextPage = string.Empty;
    public StringBuilder HiddenFields = new StringBuilder();
    public string DisplayNumber = string.Empty;
    public StringBuilder SubmitButton = new StringBuilder();
    public string PageTitle = string.Empty;

    private string SearchType = string.Empty;
    private string SearchBy = string.Empty;
    private string PermitNo = string.Empty;
    private string PermitType = string.Empty;
    private string Mode = string.Empty;
    private string ViewPermitPageNo = string.Empty;
    private string InspectionsPageNo = string.Empty;
    private string AltID = string.Empty;
    
    /// <summary>
    ///  Inspection Cancel Options
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("Inspections.Cancel.aspx");
        iPhonePageTitle = "Cancel Inspection";
        if (isiPhone == false)
        {
            PageTitle = "<div id=\"pageTitle\">Cancel Inspection</div><hr />";
        }
        // DWB - 07-31-2008 - Added new parameters to allow return to 2008 redesign inspections list.
        SearchType = (Request.QueryString["SearchType"] != null) ? Request.QueryString["SearchType"] : (Request.Form["SearchType"] != null ? Request.Form["SearchType"].ToString() : string.Empty);
        SearchBy = (Request.QueryString["SearchBy"] != null) ? Request.QueryString["SearchBy"] : (Request.Form["SearchBy"] != null ? Request.Form["SearchBy"].ToString() : string.Empty);
        Mode = (Request.QueryString["Mode"] != null) ? Request.QueryString["Mode"] : (Request.Form["Mode"] != null ? Request.Form["Mode"].ToString() : string.Empty);
        PermitNo = (Request.QueryString["PermitNo"] != null) ? Request.QueryString["PermitNo"] : string.Empty;
        AltID = (Request.QueryString["AltID"] != null) ? Request.QueryString["AltID"] : string.Empty;
        PermitType = (Request.QueryString["PermitType"] != null) ? Request.QueryString["PermitType"] : (Request.Form["PermitType"] != null ? Request.Form["PermitType"].ToString() : string.Empty);
        ViewPermitPageNo = (Request.QueryString["ViewPermitPageNo"] != null) ? Request.QueryString["ViewPermitPageNo"] : (Request.Form["ViewPermitPageNo"] != null ? Request.Form["ViewPermitPageNo"].ToString() : string.Empty);
        InspectionsPageNo = (Request.QueryString["InspectionsPageNo"] != null) ? Request.QueryString["InspectionsPageNo"] : (Request.Form["InspectionsPageNo"] != null ? Request.Form["InspectionsPageNo"].ToString() : string.Empty);
        string InspectionId = GetFieldValue("InspectionId", false);
        long inspectionID;
        if (!long.TryParse(InspectionId, out inspectionID))
        {
            inspectionID = 0; // this should never happen - 
        }
        string RescheduleRestrictionSettings = GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS, false);
        if (RescheduleRestrictionSettings == string.Empty)
        {
            RescheduleRestrictionSettings = GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "0", false);
            RescheduleRestrictionSettings += "|" + GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "1", false);
            RescheduleRestrictionSettings += "|" + GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "2", false);
            RescheduleRestrictionSettings += "|" + GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "3", false);
        }
        string CancellationRestrictionSettings = GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS, false);
        if (CancellationRestrictionSettings == string.Empty)
        {
            CancellationRestrictionSettings = GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "0", false);
            CancellationRestrictionSettings += "|" + GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "1", false);
            CancellationRestrictionSettings += "|" + GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "2", false);
            CancellationRestrictionSettings += "|" + GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "3", false);
        }

        HiddenFields.Append(HTML.PresentHiddenField("PermitNo", PermitNo));
        HiddenFields.Append(HTML.PresentHiddenField("AltID", AltID));
        HiddenFields.Append(HTML.PresentHiddenField("PermitType", PermitType));
        HiddenFields.Append(HTML.PresentHiddenField("SearchBy", SearchBy));
        HiddenFields.Append(HTML.PresentHiddenField("SearchType", SearchType));
        HiddenFields.Append(HTML.PresentHiddenField("ViewPermitPageNo", ViewPermitPageNo));
        HiddenFields.Append(HTML.PresentHiddenField("InspectionsPageNo", InspectionsPageNo));
        HiddenFields.Append(HTML.PresentHiddenField("Mode", Mode));
        HiddenFields.Append(HTML.PresentHiddenField("Module", ModuleName));
        HiddenFields.Append(HTML.PresentHiddenField("rowNumber", GetFieldValue("RowNumber",false)));
            
        DisplayNumber = ((AltID != string.Empty) ? AltID : PermitNo);

        List<InspectionViewModel> inspectionViewModels = (List<InspectionViewModel>) Session["AMCA_INSPECTION_MODELS"];
        int rowNumber = 0; // int.Parse(GetFieldValue("RowNumber", false));
        InspectionViewModel listRow = null;
        for (rowNumber = 0; rowNumber < inspectionViewModels.Count; rowNumber++)
        {
            if (inspectionID == inspectionViewModels[rowNumber].ID)
            {
                listRow = inspectionViewModels[rowNumber];
                break;
            }
        }
        ThisInspection = ConvertUtil.convertInspectionDataModelToInspection(listRow.InspectionDataModel);

        if (isiPhone == true)
        {
            SubmitButton.Append("<center>");
        }
        SubmitButton.Append("<input id=\"Submit1\" type=\"submit\" value=\"Continue\" />");
        if (isiPhone == true)
        {
            SubmitButton.Append("</center>");
        }

        //inspection comments
        Comments = ThisInspection.Comments;
  
        // Inspection date and time
        DateTime ResultDate;
        if (ThisInspection.Date != null && DateTime.TryParse(ThisInspection.Date.ToString(), out ResultDate) == true)
        {
            if (ResultDate.ToString("HH:mm") != "00:00")
            {
                InspectionDateTime = ResultDate.ToString("MM/dd/yy hh:mm tt");
            }
        }
        // provides hyperlink if comments length > 25 for to detail page
        if (Comments.Length > 90)
        {
            Comments = Comments.Substring(0, 90) + HTML.PresentLink("View.Details.aspx?State=" + State + "&Type=InspectionCancel&Id=" + ThisInspection.Id, "...More");
        }

        NextPage = "Inspections.Confirmations.aspx?State=" + State
            + "&InspectionId=" + ThisInspection.Id
            + "&InspType=" + ThisInspection.Type
            + "&Action=" + "Cancel"
            + "&PermitNo=" + PermitNo
            + "&AltID=" + AltID
            + "&PermitType=" + PermitType
            + "&SearchBy=" + SearchBy
            + "&SearchType=" + SearchType
            + "&ViewPermitPageNo=" + ViewPermitPageNo
            + "&InspectionsPageNo=" + InspectionsPageNo
            + "&Module=" + ModuleName
            + "&Mode=" + Mode;

        string SearchMode = Mode;
        string ResultPage = ViewPermitPageNo;
        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        StringBuilder sbWork = new StringBuilder();
        string[] resceduleRestrictions = RescheduleRestrictionSettings.Split('|');
        string[] cancelRestrictions = CancellationRestrictionSettings.Split('|');

        sbWork.Append("&PermitNo="          + PermitNo.ToString());
        sbWork.Append("&AltID="             + AltID.ToString());
        sbWork.Append("&PermitType="        + PermitType);
        sbWork.Append("&SearchBy="          + SearchBy.ToString());
        sbWork.Append("&SearchType="        + SearchType.ToString());
        sbWork.Append("&Mode="              + Mode.ToString());
        sbWork.Append("&Module="            + ModuleName.ToString());
        sbWork.Append("&InspectionPageNo="  + InspectionsPageNo.ToString());
        sbWork.Append("&ViewPermitPageNo="  + ViewPermitPageNo.ToString());
        sbWork.Append("&AgencyCode="        + GetFieldValue("AgencyCode",false));
        sbWork.Append("&RecordID1"          + GetFieldValue("RecordID1", false));
        sbWork.Append("&RecordID2"          + GetFieldValue("RecordID2", false));
        sbWork.Append("&RecordID3"          + GetFieldValue("RecordID3", false));
        sbWork.Append("&Module="            + GetFieldValue("Module", false));
        sbWork.Append("&PublicUserID="      + GetFieldValue("PublicUserID", false));
        sbWork.Append("&ScheduledType="     + GetFieldValue("ScheduledType", false));
        sbWork.Append("&Required="          + GetFieldValue("Required", false));
        sbWork.Append("&ReadyTimeEnabled="  + GetFieldValue("ReadyTimeEnabled", false));
        sbWork.Append("&Action="            + GetFieldValue("Action", false));
        sbWork.Append("&TypeID="            + GetFieldValue("TypeID", false));
        sbWork.Append("&TypeText="          + GetFieldValue("TypeText", false));
        sbWork.Append("&Group="             + GetFieldValue("Group", false));
        sbWork.Append("&InAdvance="         + GetFieldValue("InAdvance", false));
        sbWork.Append("&ID="                + GetFieldValue("ID", false));
        sbWork.Append("&ResultGroup="       + GetFieldValue("ResultGroup", false));
        sbWork.Append("&Units="             + GetFieldValue("Units", false));
        sbWork.Append("&IsFromWizardPage="  + GetFieldValue("IsFromWizardPage", false));
        sbWork.Append("&ContactFirstName="  + GetFieldValue("ContactFirstName", false));
        sbWork.Append("&ContactLastName="   + GetFieldValue("ContactLastName", false));
        sbWork.Append("&RequestDayOption="  + GetFieldValue("RequestDayOption", false));
        sbWork.Append("&isPopup="           + GetFieldValue("isPopup", false));
        sbWork.Append("&ContactChangeOption="           + GetFieldValue("ContactChangeOption", false));
        sbWork.Append("&RescheduleRestrictionSettings=" + GetFieldValue("RescheduleRestrictionSettings", false));
        sbWork.Append("&CancelRestrictionSettings="     + GetFieldValue("CancelRestrictionSettings", false));

        sbWork.Append("&" + ACAConstant.INSPECTION_IN_ADVANCE + "=" + GetFieldValue(ACAConstant.INSPECTION_IN_ADVANCE, false));
        sbWork.Append("&" + ACAConstant.INSPECTION_SCHEDULING_MANNER + "=" + GetFieldValue(ACAConstant.INSPECTION_SCHEDULING_MANNER, false));
        sbWork.Append("&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "0=" + resceduleRestrictions[0]);
        sbWork.Append("&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "1=" + resceduleRestrictions[1]);
        sbWork.Append("&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "2=" + resceduleRestrictions[2]);
        sbWork.Append("&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "3=" + resceduleRestrictions[3]);
        sbWork.Append("&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "0=" + cancelRestrictions[0]);
        sbWork.Append("&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "1=" + cancelRestrictions[1]);
        sbWork.Append("&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "2=" + cancelRestrictions[2]);
        sbWork.Append("&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "3=" + cancelRestrictions[3]);
        sbWork.Append("&InspUnits=" + GetFieldValue("InspUnits", false));
        sbWork.Append("&InspSeqNum=" + GetFieldValue("InspSeqNum", false));
        sbWork.Append("&InspType=" + GetFieldValue("inspType", false));
        sbWork.Append("&InspStatus=" + GetFieldValue("inspStatus", false));
        sbWork.Append("&Action=" + GetFieldValue("Action", false)); 



        sbWork.Append("&cboYear=" + GetFieldValue("TheMonth", false));
        sbWork.Append("&cboDay=" + GetFieldValue("TheDay", false));

        Breadcrumbs = BreadCrumbHelper("Inspection.Cancel.aspx", sbWork, "Cancel Inspection", breadCrumbIndex, isElipseLink, false, false, true);
        BackForwardLinks = BackLinkHelper(CurrentBreadCrumbIndex.ToString());
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
}
