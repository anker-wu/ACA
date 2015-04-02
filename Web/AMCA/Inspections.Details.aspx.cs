/**
* <pre>
* 
*  Accela Citizen Access
*  File: InspectionWizardTypes.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2011
* 
*  Description:
*  View My Collection List. 
* 
*  Notes:
*      $Id: Inspections.Details.aspx.cs 209210 2011-12-07 10:10:23Z ACHIEVO\alan.hu $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
* 
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
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Inspection;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using Accela.ACA.Html.Inspection;
using Accela.ACA.Inspection;

/// <summary>
/// 
/// </summary>
public partial class InspectionsDetails : AccelaPage
{
    private const string CATEGORY_OTHERS_KEY = "\fOthers\f";

    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder InspectionList = new StringBuilder();
    public StringBuilder ResultHeader = new StringBuilder();
    public StringBuilder PagingFooter = new StringBuilder();
    public StringBuilder BackForwardLinks = new StringBuilder();
    public StringBuilder HiddenFields = new StringBuilder();
    public StringBuilder RecordInfo = new StringBuilder();
    public string PageTitle = string.Empty;

    private string PermitNo = string.Empty;
    private string AltID = string.Empty;
    private string PermitType = string.Empty;
    private string SearchBy = string.Empty;
    private string SearchType = string.Empty;
    private string ViewPermitPageNo = string.Empty;  // ResultPage for "View Permits" breadcrumb link.
    private string InspectionsPageNo = string.Empty; // ResultPage for "View Permits > Inspections" Breadcrumb link
    private string Mode = string.Empty;
    private StringBuilder listSectionHeader = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("InspectionWizardType.aspx");
        State = GetFieldValue("State", false);
 
        int rowNumber = int.Parse(GetFieldValue("RowNumber", false));
        // string ID1 = GetFieldValue("ID1",false);
        // string ID2 = GetFieldValue("ID2",false);
        // string ID3 = GetFieldValue("ID3",false);
        string Agency = GetFieldValue("Agency",false);
        string trackingID = GetFieldValue("trackingID",false);
        string changeListMode = GetFieldValue("ChangeListMode", false);
        int newListMode = 0;
        int currentListMode = 0;
        bool isListModeChange = (changeListMode != null && changeListMode.Length > 0) ? int.TryParse(changeListMode, out newListMode) : false;
        string listMode = GetFieldValue("CurrentListMode", false);
        if (isListModeChange)
        {
            currentListMode = newListMode;
        }
        else
        {
            if (!int.TryParse(listMode, out currentListMode))
            {
                currentListMode = 0;
            }
        }
        listMode = "&CurrentListMode=" + currentListMode.ToString();

        InspectionViewModel theInspection = null;
        InspectionDataModel theInspModel = null;
        List<InspectionViewModel> inspectionViewModels = (List<InspectionViewModel>)Session["AMCA_INSPECTION_MODELS"];
        long inspectionID = 0;
        if (long.TryParse(GetFieldValue("InspectionId", false), out inspectionID))
        {
            for (rowNumber = 0; rowNumber < inspectionViewModels.Count; rowNumber++)
            {
                if (inspectionID == inspectionViewModels[rowNumber].ID)
                {
                    theInspection = inspectionViewModels[rowNumber];
                    theInspModel = theInspection.InspectionDataModel;
                    inspectionID = theInspection.ID;
                    break;
                }
            }
        }
         
        InspectionProxy inspectionProxy = new InspectionProxy();
        InspectionTreeNodeModel relatedInsp = inspectionProxy.GetInspectionRelatedTree(inspectionID, ModuleName);
        IList<InspectionViewModel> inspectionStatusHistory = inspectionProxy.GetInspectionStatusHistory(inspectionID, ModuleName);
        IList<InspectionViewModel> inspectionCommentHistory = inspectionProxy.GetInspectionCommentHistory(inspectionID, ModuleName);

        int statusHistoryCount = inspectionStatusHistory != null && inspectionStatusHistory.Count > 0 ? inspectionStatusHistory.Count : 0;
        int commentHistoryCount = inspectionStatusHistory != null && inspectionCommentHistory.Count > 0 ? inspectionCommentHistory.Count : 0;
        int relatedInspCount = relatedInsp.children[0].children != null ? relatedInsp.children[0].children.Length : 0;
        relatedInspCount += relatedInsp.inspectionModel != null ? 1 : 0;

        // get current state and parameters
        PermitNo = GetFieldValue("PermitNo", false);
        AltID = GetFieldValue("AltID", false);
        PermitType = GetFieldValue("PermitType", false);
        SearchBy = GetFieldValue("SearchBy", false);
        SearchType = GetFieldValue("SearchType", false);
        ViewPermitPageNo = GetFieldValue("ViewPermitPageNo", false);
        InspectionsPageNo = GetFieldValue("InspectionsPageNo", false);
        Mode = GetFieldValue("Mode", false);
        bool pagingMode = GetFieldValue("PagingMode", false) == "Y";
      
        StringBuilder linkURL = new StringBuilder();
        linkURL.Append("&PermitNo=" + PermitNo.ToString());
        linkURL.Append("&AltID=" + AltID.ToString());
        linkURL.Append("&PermitType=" + PermitType);
        linkURL.Append("&SearchBy=" + SearchBy.ToString());
        linkURL.Append("&SearchType=" + SearchType.ToString());
        linkURL.Append("&InspectionsPageNo=" + InspectionsPageNo.ToString());
        linkURL.Append("&ViewPermitPageNo=" + ViewPermitPageNo.ToString());
        linkURL.Append("&Mode=" + Mode.ToString());
        linkURL.Append(listMode);
        StringBuilder historyURL = new StringBuilder();
        historyURL.Append(linkURL);
        historyURL.Append("&InspectionId=" + inspectionID.ToString());
        historyURL.Append("&PagingMode=Y");

        iPhonePageTitle = LabelUtil.RemoveHtmlFormat(LocalGetTextByKey("aca_inspection_sectionname_relatedinspection"));
        iPhonePageTitle = theInspection.TypeText;
        if (isiPhone != true)
        {
            PageTitle = "<div id=\"pageTitle\">" + iPhonePageTitle + "</div>";
        }
        var inspectionPermissionBll = ObjectFactory.GetObject<IInspectionPermissionBll>();
        CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
        bool isContactVisible = inspectionPermissionBll.CheckContactRight(capModel, capModel.capID.serviceProviderCode, ModuleName, ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_VIEW_CONTACT);
        buildRecordDetails(theInspModel.InspectionModel.activity.capIDModel,theInspection, rowNumber, linkURL.ToString(), RecordInfo, isContactVisible, commentHistoryCount, statusHistoryCount, relatedInspCount, currentListMode);

        //Module must be added to the URL after the inspection detail is populated to avoid duplicate parameters
        //in the scheduile, request, reschedule, or cancel links
        linkURL.Append("&Module=" + ModuleName.ToString());

        int ResultCount = 0;
        //number of records shown per page - from web.config
        AppSettingsReader Settings = new AppSettingsReader();
        int ResultRecordsPerPage = int.Parse(Settings.GetValue("SearchRecordsPerPage", typeof(string)).ToString());
        int CurrentResultPageNumber = 1;

        //get current page number , by default '1'- used while paging
        string PageResultPageNo = GetFieldValue("ResultPage", false);
        if (!isListModeChange)
        {
            if (PageResultPageNo == string.Empty)
            {
                PageResultPageNo = GetFieldValue("ResultPageNo", false);
            }
            if (PageResultPageNo != string.Empty)
            {
                CurrentResultPageNumber = int.Parse(PageResultPageNo) + 1;
            }
        }

        //Build inspections type list

        int rowNum = 0;
        int recIndex = 0;

        int ResultPageStart = (CurrentResultPageNumber - 1) * ResultRecordsPerPage;
        switch (currentListMode)
        {
            case 0:
                ResultCount = relatedInsp.children[0].children != null ? relatedInsp.children[0].children.Length : 0;
                ResultCount += relatedInsp.inspectionModel != null ? 1 : 0;
                break;
            case 1:
                ResultCount = statusHistoryCount;
                break;
            case 2:
                ResultCount = commentHistoryCount;
                break;
        }
        
        int FirstRecord = ResultPageStart + 1;
        int LastRecord = ResultCount;
        if (ResultCount >= ResultRecordsPerPage)
        {
            LastRecord = (FirstRecord - 1) + ResultRecordsPerPage;
            if (LastRecord > ResultCount)
            {
                LastRecord = ResultCount;
            }
        }

        int rowsToDisplay = (ResultCount - ResultPageStart);
        if (rowsToDisplay > ResultRecordsPerPage)
        {
            rowsToDisplay = ResultRecordsPerPage;
        }

        if (ResultCount > 0)
        {
            ResultHeader.Append(listSectionHeader);
            ResultHeader.Append("<div style=\"margin-top:5px; margin-bottom:5px; \">" + "Showing ");
            ResultHeader.Append(FirstRecord.ToString() + "-");
            ResultHeader.Append(LastRecord.ToString() + " of " + ResultCount.ToString());
            ResultHeader.Append("</div>");
            recIndex = 0; //Current record being parsed
            rowNum = 0;  //Visisble row displaye on page 
            switch (currentListMode)
            {
                case 0:
                    if (relatedInsp.inspectionModel != null)
                    {
                         if (recIndex >= ResultPageStart)
                        {
                            rowNum++;
                            string aLink = BuildRow(rowNumber, "Parent", relatedInsp.inspectionModel, linkURL.ToString());
                            InspectionList.Append(MyProxy.CreateSelectListCell(aLink, rowNum - 1, recIndex, ResultCount, ResultPageStart, rowsToDisplay, isiPhone, false));
                            recIndex++;
                        }
                    }
                    if (relatedInsp.children != null && relatedInsp.children.Length > 0)
                    {
                       if (relatedInsp.children[0].children != null && relatedInsp.children[0].children.Length > 0)
                        {
                            
                            for (int index = 0; index < relatedInsp.children[0].children.Length; index++)
                            {
                                if (recIndex >= ResultPageStart)
                                {
                                    rowNum++;

                                    string aLink = BuildRow(rowNumber, "Child", relatedInsp.children[0].children[index].inspectionModel, linkURL.ToString());
                                    InspectionList.Append(MyProxy.CreateSelectListCell(aLink, rowNum - 1, recIndex, ResultCount, ResultPageStart, rowsToDisplay, isiPhone, false));

                                    if (rowNum == rowsToDisplay)
                                    {
                                        break;
                                    }
                                }
                                recIndex++;
                            }
                        }
                    }
                    break;
                case 1:
                       if (statusHistoryCount > 0)
                        {
                            for (int index = 0; index < inspectionStatusHistory.Count; index++)
                            {
                                if (index >= ResultPageStart)
                                {
                                    rowNum++;

                                    string aLink = BuildHistoryRow(rowNumber, inspectionStatusHistory[index], historyURL.ToString(), currentListMode);
                                    InspectionList.Append(MyProxy.CreateSelectListCell(aLink, rowNum - 1, index, ResultCount, ResultPageStart, rowsToDisplay, isiPhone, false));

                                    if (rowNum == rowsToDisplay)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    break;
                case 2:
                       if (commentHistoryCount > 0)
                        {
                            for (int index = 0; index < inspectionCommentHistory.Count; index++)
                            {
                                 if (index >= ResultPageStart)
                                {
                                    rowNumber++;

                                    string aLink = BuildHistoryRow(rowNumber, inspectionCommentHistory[index], historyURL.ToString(), currentListMode);
                                    InspectionList.Append(MyProxy.CreateSelectListCell(aLink, rowNumber - 1, index, ResultCount, ResultPageStart, rowsToDisplay, isiPhone, false));

                                    if (rowNumber == rowsToDisplay)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                   break;
            }
        }
        else
        {
            switch (currentListMode)
            {
                case 0:
                    InspectionList.Append("There are no related inspections for this inspection.");
                    break;
                case 1:
                    InspectionList.Append("There is no status history for this inspection.");
                    break;
                case 2:
                    InspectionList.Append("There are no result comments for this inspection.");
                    break;
            }
        }
        if (isiPhone != true)
        {
             //InspectionList.Append("</div>");
        }

        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        StringBuilder breadCrumbURL = new StringBuilder();
        breadCrumbURL.Append(linkURL);
        breadCrumbURL.Append("&RowNumber=" + rowNumber.ToString());
        breadCrumbURL.Append("&InspectionId=" + inspectionID.ToString());

        Breadcrumbs = BreadCrumbHelper("Inspections.Details.aspx", breadCrumbURL, iPhonePageTitle, breadCrumbIndex, isElipseLink, false, pagingMode, false);
        iPhoneHideHeaderCollectionsButton = true;
        BackForwardLinks.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));
        iPhoneHideHeaderCollectionsButton = false;

        #region Paging Part
        // Paging Part, appends to table footer
        if ((double)ResultCount > (double)ResultRecordsPerPage)
        {
            int ResultPagesCount = (int)Math.Ceiling((double)ResultCount / (double)ResultRecordsPerPage);
            if ((int)Math.Ceiling((double)ResultPagesCount * (double)ResultRecordsPerPage) < ResultCount)
            {
                ResultPagesCount++;
            }
            int NextPageNumber = 0;
            int PageCount = 0;
            int PageStart = 0;
            int PreviousPageNumber = CurrentResultPageNumber - 1;
            if (CurrentResultPageNumber < 6)
            {
                PageStart = 1;
            }
            else
            {
                PageStart = CurrentResultPageNumber - 5;
            }

            string sLinkFormat = "<a id=\"pageNavigationButton\" href=\"";
            StringBuilder PagingLink = new StringBuilder();
            PagingLink.Append(sLinkFormat + "Inspections.Details.aspx?State=" + State);
            PagingLink.Append("&PermitNo=" + PermitNo.ToString());
            PagingLink.Append("&AltID=" + AltID.ToString());
            PagingLink.Append("&PermitType=" + PermitType);
            PagingLink.Append("&SearchBy=" + SearchBy.ToString());
            PagingLink.Append("&SearchType=" + SearchType.ToString());
            PagingLink.Append("&Mode=" + Mode.ToString());
            PagingLink.Append("&Module=" + ModuleName.ToString());
            PagingLink.Append("&RowNumber=" + rowNumber.ToString());
            PagingLink.Append("&InspectionsPageNo=" + InspectionsPageNo.ToString());
            PagingLink.Append("&ViewPermitPageNo=" + ViewPermitPageNo.ToString());
            PagingLink.Append("&InspectionId=" + inspectionID.ToString());
            PagingLink.Append("&PagingMode=Y");
            PagingLink.Append(listMode);

            PagingFooter.Append("<br>");
            PagingFooter.Append("<div id=\"pageNavigation\">");
            string delimiter = string.Empty;
            if (isiPhone == true)
            {
                PagingFooter.Append("<center>");
            }
            if (PreviousPageNumber > 0)
            {
                PagingFooter.Append(PagingLink.ToString());
                PagingFooter.Append("&SlidePage=LeftToRight");
                PagingFooter.Append("&ResultPage=" + (PreviousPageNumber - 1).ToString());
                PagingFooter.Append("\">&lt;</a>");
                delimiter = " | ";
            }
            for (int page = PageStart; page <= ResultPagesCount; page++)
            {
                PageCount++;
                if (PageCount > 10)
                {
                    break;
                }
                PagingFooter.Append(delimiter);
                if (CurrentResultPageNumber == page)
                {
                    PagingFooter.Append("<span id=\"pageNavigationSelected\">" + page + "</span>");
                    NextPageNumber = page + 1;
                    delimiter = " | ";
                }
                else
                {
                    PagingFooter.Append(PagingLink.ToString());
                    if (CurrentResultPageNumber > page)
                    {
                        PagingFooter.Append("&SlidePage=LeftToRight");
                    }
                    PagingFooter.Append("&ResultPage=" + (page - 1).ToString());
                    PagingFooter.Append("\">");
                    PagingFooter.Append(page.ToString());
                    PagingFooter.Append("</a>");
                    delimiter = " | ";
                }
            }
            if (NextPageNumber <= ResultPagesCount)
            {
                PagingFooter.Append(delimiter);
                PagingFooter.Append(PagingLink.ToString());
                PagingFooter.Append("&ResultPage=" + (NextPageNumber - 1).ToString());
                PagingFooter.Append("\">&gt;</a>");
            }
            PagingFooter.Append("</div>");
            if (isiPhone == true)
            {
                PagingFooter.Append("</center>");
            }
        }
        #endregion
    }

    /// <summary>
    /// Get Field value from Form, Request, or QueryString.
    /// </summary>
    /// <param name="FieldName"> Field name </param>
    /// <param name="IsRequired"> Indcates if the user must enterer data into the field or not</param>
    /// <returns>String</returns>
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
    /// Edit result comment for greater than 90 characters.
    /// </summary>
    /// <param name="commentToEdit"> Comment to Edit </param>
    /// <param name="rowID"> DataModel row for comment</param>
    /// <param name="LinkHTML"> link parametrs to pass to for to view comments </param>
    /// <returns>String</returns>
    // DWB - 07-18-2008 - Added new funciton.
    private string editResultComment(string commentToEdit, int rowID, string LinkHTML)
    {
        string TheValue = string.Empty;
        if (commentToEdit != null)
        {
            if (commentToEdit.Length > 90)
            {
                Session["AMCA_Result_Comment" + rowID.ToString() + "0"] = commentToEdit;
                TheValue = commentToEdit.Substring(0, 90)
                    + "..." + "<a id=\"inspectionResultCommentLink\" href=\"" + "View.Details.aspx?State=" + State
                    + "&Type=Inspection"
                    + "&DTRow=" + rowID.ToString()
                    + "&DTkey=AMCA_Result_Comment" + rowID.ToString() + "0"
                    + LinkHTML
                    + "&Module=" + ModuleName
                    + "\">" + " More</a>";
            }
            else
            {
                if (commentToEdit.Length > 0)
                {
                    TheValue = "<span id=\"inspecitonResultLineComment\">" + commentToEdit + "</span>";
                }
            }
        }
        return TheValue;
    }

    /// <summary>
    /// Format an InspectionViewModel for presentation in the inspection detail page.
    /// </summary>
    /// <param name="rowNumber"> The current row number used for view comment link parameter </param>
    /// <param name="inspection"> The InspectionViewModel to be formatted </param>
    /// <param name="LinkURL"> The link parameters for the Detail.View used to read the entire comment </param>
    /// <param name="currentListMode"> Specified is status history or comment history list is being built. </param>
    /// <returns>String</returns>
    // DWB - 07-18-2008 - Added new funciton.
    private string BuildHistoryRow(int rowNumber, InspectionViewModel inspection, string linkURL, int currentListMode)
    {
        StringBuilder aLink = new StringBuilder();

        switch (currentListMode)
        {
            case 1: // status
                aLink.Append("<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
                aLink.Append("<tr>");
                aLink.Append("<td class=\"pageListText\">");
                aLink.Append(inspection.StatusDateTimeText);
                aLink.Append(" ");
                aLink.Append(inspection.StatusText);
                aLink.Append("</td>");
                aLink.Append("</tr>");
                aLink.Append("<tr>");
                aLink.Append("<td class=\"pageListText\">");
                aLink.Append("Inspector: ");
                aLink.Append(" ");
                aLink.Append(inspection.Inspector);
                aLink.Append("</td>");
                aLink.Append("</tr>");
                aLink.Append("<tr>");
                aLink.Append("<td class=\"pageListText\">");
                aLink.Append("Last Updated ");
                aLink.Append(inspection.LastUpdatedDateTimeText);
                aLink.Append(" by ");
                aLink.Append(inspection.LastUpdatedBy);
                aLink.Append("</td>");
                aLink.Append("</tr>");
                aLink.Append("</table>");
                aLink.Append("<br>");
                break;
            case 2: // comments
                string inspectionComments = editResultComment(inspection.ResultComments, rowNumber, linkURL);

                aLink.Append("<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
                aLink.Append("<tr>");
                aLink.Append("<td class=\"pageListText\">");
                aLink.Append(inspection.ResultedDateTimeText);
                aLink.Append(" by ");
                aLink.Append(inspection.LastUpdatedBy);
                aLink.Append("</td>");
                aLink.Append("</tr>");
                aLink.Append("<tr>");
                aLink.Append("<td class=\"pageListText\">");
                aLink.Append(inspectionComments);
                aLink.Append("</td>");
                aLink.Append("</tr>");
                aLink.Append("</table>");
                aLink.Append("<br>");
                break;
        }
        return aLink.ToString();
    }

    /// <summary>
    /// Format a row for the related inspection list.
    /// </summary>
    /// <param name="rowNumber"> The current row number that is being formatted </param>
    /// <param name="inspection"> The InspectionModel to be formatted </param>
    /// <param name="LinkURL"> The link parameters for the navigatin related inspections list. </param>
    /// <param name="currentListMode"> Specified is status history or comment history list is being built. </param>
    /// <returns>String</returns>
    // DWB - 07-18-2008 - Added new funciton.
    private string BuildRow(int rowNumber, string parentOrChild, InspectionModel inspection, string linkURL)
    {
        StringBuilder aLink = new StringBuilder();
        StringBuilder theURL = new StringBuilder();

        theURL.Append(linkURL);
        theURL.Append("&InspectionId=" + inspection.activity.idNumber);
        theURL.Append("&DetailRowNumber=" + rowNumber.ToString());
        theURL.Append("&RowNumber=-1");
        theURL.Append("&ID1=" + inspection.activity.capIDModel.ID1);
        theURL.Append("&ID2=" + inspection.activity.capIDModel.ID2);
        theURL.Append("&ID3=" + inspection.activity.capIDModel.ID3);
        theURL.Append("&Agency=" + inspection.activity.capIDModel.serviceProviderCode);
        theURL.Append("&trackingID=" + inspection.activity.capIDModel.trackingID);
        theURL.Append("&PagingMode=Y");

        if (isiPhone)
        {
            aLink.Append("<a href=\"");
            aLink.Append("Inspections.Details.aspx?State=" + State);
            aLink.Append(theURL);
            aLink.Append("\">");
            aLink.Append("<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
            aLink.Append("<tr><td width=\"90%\" colspan=\"3\"  class=\"pageListText\">");
            aLink.Append("<span class=\"pageListLinkBold\">");
            aLink.Append(inspection.activity.activityType);
            aLink.Append("</span> (");
            aLink.Append(inspection.activity.idNumber);
            aLink.Append(")</td><td rowspan=\"2\">");
            aLink.Append("<img style=\"float:right; vertical-align:middle;\" src=\"img/chevron.png\" /> ");
            aLink.Append("</td>");
            aLink.Append("</tr>");
            aLink.Append("<tr><td width=\"45%\" class=\"pageListText\">Relationship: ");
            aLink.Append(parentOrChild);
            aLink.Append("</td><td width=\"45%\" class=\"pageListText\">Status: ");
            aLink.Append(inspection.activity.status);
            aLink.Append("</td></tr></table>");
            aLink.Append("</a>");
        }
        else
        {
            aLink.Append("<table cellpadding=\"5\" cellspacing=\"0\" width=\"100%\">");
            aLink.Append("<tr><td width=\"100%\" colspan=\"2\" class=\"pageListText\">");
            aLink.Append("<a class=\"pageListLink\"href=\"");
            aLink.Append("Inspections.Details.aspx?State=" + State);
            aLink.Append(theURL);
            aLink.Append("\">");
            aLink.Append(inspection.activity.activityType);
            aLink.Append("</a>");
            aLink.Append(" (");
            aLink.Append(inspection.activity.idNumber); // Change This
            aLink.Append(")</td><tr>");
            aLink.Append("<td width=\"50%\" class=\"pageListText\">Relationship: ");
            aLink.Append(parentOrChild);
            aLink.Append("</td><td width=\"50%\" class=\"pageListText\">Status: ");
            aLink.Append(inspection.activity.status);
            aLink.Append("</td></tr></table>");
            aLink.Append("<br>");
        }
        return aLink.ToString();
    }

    /// <summary>
    /// Format the inspection details for display.
    /// </summary>
    /// <param name="capIdModel"> The CAP id of the record that the inspection is assocated to </param>
    /// <param name="theRow"> The current visisble row number </param>
    /// <param name="x"> The current row number of the inspection list that is being parsed</param>
    /// <param name="baseLinkHTML"> link parametrs to pass to when navigating to other pages </param>
    /// <param name="inspectionDetails"> The page element that will display the inspection details </param>
    /// <param name="isContactVisible"> Indicates if the contact info is displayed or not </param>
    /// <param name="commentHistoryCount"> Number of available comments to display on comment history link </param>
    /// <param name="statusHistoryCount"> Number of available statuses display on status history link </param>
    /// <param name="relatedInspectionCount"> Number of available related records display on related hisotry link </param>
    /// <param name="currentListMode"> Deterines which related items links are displayed on the page. </param>
    /// <returns>void</returns>
    private void buildRecordDetails(CapIDModel capIdModel,
        InspectionViewModel theRow,
        int x,
        string baseLinkHTML,
        StringBuilder inspectionDetails,
        bool isContactVisible,
        int commentHistoryCount,
        int statusHistoryCount,
        int relatedInspCount,
        int currentListMode)
    {
        InspectionDataModel aRow = theRow.InspectionDataModel;

        List<InspectionActionViewModel> availableActions = InspectionActionViewUtil.BuildViewModels(ConfigManager.AgencyCode, capIdModel, ModuleName, AppSession.User.PublicUserId, theRow);

        string LinkHTML = baseLinkHTML
                        + "&RowNumber=" + x.ToString()
                        + "&InspectionId=" + theRow.ID
                        + "&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "=" + HttpUtility.UrlEncode(aRow.RestrictionSettings4Reschedule.ToString())
                        + "&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "=" + HttpUtility.UrlEncode(aRow.RestrictionSettings4Cancel.ToString());

        string ActionLinkHTML = "<a id=\"inspectionLineLink\" href=\"";
        Inspection ThisInspection = new Inspection();

        string ScheduleLink = string.Empty;
        string ReScheduleLink = string.Empty;
        string actionAllowedLink = string.Empty;
        string relatedIsnpLink = string.Empty;
        string commentsLink = string.Empty;
        string statusLink = string.Empty;
        string CancelLink = string.Empty;
        StringBuilder ViewDetailsLink = new StringBuilder();

        string textTBD = StripHTMLFromLabelText(LabelUtil.GetTextByKey("ACA_Inspection_BlankInspector_TBD", ModuleName), "TBD");
        string aca_inspection_sectionname_relatedinspection = StripHTMLFromLabelText(LocalGetTextByKey("aca_inspection_sectionname_relatedinspection"), "Related Inspections");
        string inspectiondetail_resultcomments = StripHTMLFromLabelText(LocalGetTextByKey("inspectiondetail_resultcomments"), "Result Comments");
        string inspectiondetail_statushistory = StripHTMLFromLabelText(LocalGetTextByKey("inspectiondetail_statushistory"), "Status History");

        string pattern = string.Empty;

        bool useRequestDateTime = false;
        bool useRequestDate = false;
        bool useRequestTime = false;
        bool useResultDateTime = false;
        bool useResultDate = false;
        bool useResultTime = false;
        bool useScheduledDateTime = false;
        bool useScheduledDate = false;
        bool useScheduledTime = false;
        bool useReadyDateTime = false;
        bool useReadyDate = false;
        bool useReadyTime = false;
        bool useLastUpdateDateTime = true;
        bool useLastUpdateDate = false;
        bool useLastUpdateTime = true;
        bool useLastUpdateBy = true;
        bool useInspSequeceNumber = false;
        bool useInspector = false;
        bool isRequestPending = false;
        String actionByLabel = " by ";
        if (aRow.IsUpcomingInspection)
        {
            if (aRow.ScheduleType == InspectionScheduleType.RequestOnlyPending && aRow.ReadyTimeEnabled)
            {
                //TBD at TBD, Ready Time: < Ready Date > at  < Ready Time >
                pattern = LabelUtil.GetTextByKey("aca_inspection_upcominglist_readytime_combofield_pattern", ModuleName);
                if (pattern != null && pattern.Length > 0)
                {
                    useReadyDateTime = pattern.Contains(ListItemVariables.ReadyTimeDateTime);
                    useReadyDate = pattern.Contains(ListItemVariables.ReadyTimeDate);
                    useReadyTime = pattern.Contains(ListItemVariables.ReadyTimeTime);
                }
            }
            else
            {
                isRequestPending = aRow.ScheduleType == InspectionScheduleType.RequestOnlyPending;
                pattern = LabelUtil.GetTextByKey("aca_inspection_upcominglist_combofield_pattern", ModuleName);
            }
        }
        else
        {
            if (aRow.Status == InspectionStatus.Rescheduled || aRow.Status == InspectionStatus.Canceled)
            {
                //By <Operator> on <operation date> at <operate time>
                if (aRow.Status == InspectionStatus.Rescheduled)
                {
                    pattern = LabelUtil.GetTextByKey("aca_inspection_completedlist_rescheduled_combofield_pattern", ModuleName);
                    actionByLabel = " Rescheduled by: ";
                }
                else
                {
                    pattern = LabelUtil.GetTextByKey("aca_inspection_completedlist_cancelled_combofield_pattern", ModuleName);
                    actionByLabel = " Cancelled by: ";
                }
                if (pattern != null && pattern.Length > 0)
                {
                    useLastUpdateBy = pattern.Contains(ListItemVariables.Operator);
                    useLastUpdateDateTime = pattern.Contains(ListItemVariables.OperationDateTime);
                    useLastUpdateDate = pattern.Contains(ListItemVariables.OperationDate);
                    useLastUpdateTime = pattern.Contains(ListItemVariables.OperationTime);
                }
            }
            else
            {
                pattern = LabelUtil.GetTextByKey("aca_inspection_completedlist_combofield_pattern", ModuleName);
                actionByLabel = " Resulted by: ";
            }
        }

        if (aRow.Status == InspectionStatus.PendingByACA || aRow.Status == InspectionStatus.PendingByV360)
        {
            useRequestDateTime = pattern.Contains(ListItemVariables.ScheduledOrRequestedDateTime);
            useRequestDate = pattern.Contains(ListItemVariables.ScheduledOrRequesteddDate);
            useRequestTime = pattern.Contains(ListItemVariables.ScheduledOrRequestedTime);
        }
        else
        {
            useScheduledDateTime = pattern.Contains(ListItemVariables.ScheduledOrRequestedDateTime);
            useScheduledDate = pattern.Contains(ListItemVariables.ScheduledOrRequesteddDate);
            useScheduledTime = pattern.Contains(ListItemVariables.ScheduledOrRequestedTime);

            useResultDateTime = pattern.Contains(ListItemVariables.ResultedDateTime);
            useResultDate = pattern.Contains(ListItemVariables.ResultedDate);
            useResultTime = pattern.Contains(ListItemVariables.ResultedTime);
        }

        useInspSequeceNumber = pattern.Contains(ListItemVariables.InspectionSequenceNumber);
        useInspector = pattern.Contains(ListItemVariables.Inspector);

        string aDash = string.Empty;
        string aSpace = string.Empty;

        DateTime resultDateTimeValue, scheduleDateTimeValue, readyDateTimeValue, requestedDateTimeValue, lastUpdatedDateTimeValue;

        bool scheduleDateTimeValueOk = DateTime.TryParse(theRow.ScheduledDateTimeText, out scheduleDateTimeValue);
        StringBuilder scheduleOrRequestDate = new StringBuilder();
        if (useScheduledDateTime)
        {
            scheduleOrRequestDate.Append((scheduleDateTimeValueOk && isRequestPending == false) ? I18nDateTimeUtil.FormatToDateTimeStringForUI(scheduleDateTimeValue) : textTBD);
        }
        else
        {
            if (useScheduledDate)
            {
                scheduleOrRequestDate.Append((scheduleDateTimeValueOk && isRequestPending == false) ? I18nDateTimeUtil.FormatToDateStringForUI(scheduleDateTimeValue) : textTBD);
                aSpace = " at ";
            }
            if (useScheduledTime)
            {
                scheduleOrRequestDate.Append(aSpace);
                scheduleOrRequestDate.Append((scheduleDateTimeValueOk && isRequestPending == false) ? I18nDateTimeUtil.FormatToTimeStringForUI(scheduleDateTimeValue, true) : textTBD);
            }
        }

        bool requestedDateTimeValueOk = DateTime.TryParse(theRow.RequestedDateTimeText, out requestedDateTimeValue);
        aSpace = string.Empty;
        if (useRequestDateTime)
        {
            scheduleOrRequestDate.Append(requestedDateTimeValueOk ? I18nDateTimeUtil.FormatToDateTimeStringForUI(requestedDateTimeValue) : textTBD);
        }
        else
        {
            if (useRequestDate)
            {
                scheduleOrRequestDate.Append(requestedDateTimeValueOk ? I18nDateTimeUtil.FormatToDateStringForUI(requestedDateTimeValue) : textTBD);
                aSpace = " at ";
            }
            if (useRequestTime)
            {
                scheduleOrRequestDate.Append(aSpace);
                scheduleOrRequestDate.Append(requestedDateTimeValueOk ? I18nDateTimeUtil.FormatToTimeStringForUI(requestedDateTimeValue, true) : textTBD);
            }
        }

        bool resultDateTimeValueOk = DateTime.TryParse(theRow.ResultedDateTimeText, out resultDateTimeValue);
        StringBuilder resultDate = new StringBuilder();
        aSpace = string.Empty;
        if (useResultDateTime)
        {
            resultDate.Append(resultDateTimeValueOk ? I18nDateTimeUtil.FormatToDateTimeStringForUI(resultDateTimeValue) : textTBD);
        }
        else
        {
            if (useResultDate)
            {
                resultDate.Append(resultDateTimeValueOk ? I18nDateTimeUtil.FormatToDateStringForUI(resultDateTimeValue) : textTBD);
                aSpace = " at ";
            }
            if (useResultTime)
            {
                resultDate.Append(aSpace);
                resultDate.Append(resultDateTimeValueOk ? I18nDateTimeUtil.FormatToTimeStringForUI(resultDateTimeValue, true) : textTBD);
            }
        }

        bool readyDateTimeValueOk = DateTime.TryParse(theRow.ReadyTimeDateTimeText, out readyDateTimeValue);
        StringBuilder readyDate = new StringBuilder();
        aSpace = string.Empty;
        if (useReadyDateTime)
        {
            readyDate.Append(readyDateTimeValueOk ? I18nDateTimeUtil.FormatToDateTimeStringForUI(readyDateTimeValue) : textTBD);
        }
        else
        {
            if (useReadyDate)
            {
                readyDate.Append(readyDateTimeValueOk ? I18nDateTimeUtil.FormatToDateStringForUI(readyDateTimeValue) : textTBD);
                aSpace = " at ";
            }
            if (useReadyTime)
            {
                readyDate.Append(aSpace);
                readyDate.Append(readyDateTimeValueOk ? I18nDateTimeUtil.FormatToTimeStringForUI(readyDateTimeValue, true) : textTBD);
            }
        }

        bool lastUpdatedDateTimeValueOk = DateTime.TryParse(theRow.LastUpdatedDateTimeText, out lastUpdatedDateTimeValue);
        StringBuilder lastUpdatedDate = new StringBuilder();
        aSpace = string.Empty;
        if (useLastUpdateDateTime)
        {
            lastUpdatedDate.Append(theRow.LastUpdatedDateTimeText);
        }
        else
        {
            if (useLastUpdateDate)
            {
                lastUpdatedDate.Append(theRow.LastUpdatedDateText);
                aSpace = " at ";
            }
            if (useLastUpdateTime)
            {
                lastUpdatedDate.Append(aSpace);
                lastUpdatedDate.Append(theRow.LastUpdatedTimeText);
            }
        }
        StringBuilder tempUpdatedBy = new StringBuilder();
        if (useLastUpdateBy)
        {
            tempUpdatedBy.Append(aRow.LastUpdatedBy != null && aRow.LastUpdatedBy.Length > 0 ? aRow.LastUpdatedBy : textTBD);
        }

        string tempStatusString = aRow.StatusString;
        if (string.IsNullOrEmpty(tempStatusString))
        {
            string statusLabelKey = GetLabelKey(aRow.Status);
            tempStatusString = String.IsNullOrEmpty(statusLabelKey) ? String.Empty : LabelUtil.GetTextByKey(statusLabelKey, ModuleName);
            tempStatusString = I18nStringUtil.GetString(tempStatusString, aRow.InspectionModel.activity.status);
        }

        string tempInspector = String.IsNullOrEmpty(theRow.Inspector) ? LabelUtil.GetTextByKey("ACA_Inspection_BlankInspector_TBD", ModuleName) : theRow.Inspector;

        string tempRequiredOrOptional = aRow.Required ? LabelUtil.GetTextByKey("ACA_Inspection_Status_Required", ModuleName) : LabelUtil.GetTextByKey("ACA_Inspection_Status_Optional", ModuleName);
        if (relatedInspCount > 0 && currentListMode != 0)
        {
            ViewDetailsLink.Append(ActionLinkHTML + "Inspections.Details.aspx?State=" + State
                 + LinkHTML
                 + "&changeListMode=0"
                 + "&PagingMode=Y"
                 + getTargetURL(availableActions[0].TargetURL)
                 + "\">" + aca_inspection_sectionname_relatedinspection + " (" + relatedInspCount.ToString() + ")</a>  ");
        }
        if (statusHistoryCount > 0 && currentListMode != 1)
        {
            ViewDetailsLink.Append(ActionLinkHTML + "Inspections.Details.aspx?State=" + State
                 + LinkHTML
                 + "&changeListMode=1"
                 + "&PagingMode=Y"
                 + getTargetURL(availableActions[0].TargetURL)
                 + "\">" + inspectiondetail_statushistory + " (" + statusHistoryCount.ToString() + ")</a>  ");
        }
        if (commentHistoryCount > 0 && currentListMode != 2)
        {
            ViewDetailsLink.Append(ActionLinkHTML + "Inspections.Details.aspx?State=" + State
                 + LinkHTML
                 + "&changeListMode=2"
                 + "&PagingMode=Y"
                 + getTargetURL(availableActions[0].TargetURL)
                 + "\">" + inspectiondetail_resultcomments + " (" + commentHistoryCount.ToString() + ")</a> ");
        }
        if (aRow.AvailableOperations.Length > 0 && aRow.IsUpcomingInspection)
        {
            switch (aRow.AvailableOperations[0])
            {
                case InspectionAction.Request:
                    ScheduleLink = ActionLinkHTML + "Inspections.ScheduleOneScreen.aspx?State=" + State
                         + LinkHTML
                         + getTargetURL(availableActions[0].TargetURL)
                         + "\">" + aRow.AvailableOperations[0] + "</a>";
                    actionAllowedLink = ScheduleLink;
                    break;
                case InspectionAction.Schedule:
                    ScheduleLink = ActionLinkHTML + "Inspections.ScheduleOneScreen.aspx?State=" + State
                        + LinkHTML
                        + getTargetURL(availableActions[0].TargetURL)
                        + "\">" + aRow.AvailableOperations[0] + "</a>";
                    actionAllowedLink = ScheduleLink;
                    break;
                case InspectionAction.Reschedule:
                    ReScheduleLink = ActionLinkHTML + "Inspections.ScheduleOneScreen.aspx?State=" + State
                        + LinkHTML
                        + getTargetURL(availableActions[0].TargetURL)
                        + "\">" + aRow.AvailableOperations[0] + "</a>";
                    actionAllowedLink = ReScheduleLink;
                    break;
                case InspectionAction.Cancel:
                    CancelLink = ActionLinkHTML + "Inspections.Cancel.aspx?State=" + State
                       + LinkHTML
                       + getTargetURL(availableActions[0].TargetURL)
                       + "\">" + aRow.AvailableOperations[0] + "</a>";
                    break;
            }
            if (aRow.AvailableOperations.Length > 2)
            {
                switch (aRow.AvailableOperations[1])
                {
                    case InspectionAction.Request:
                        ScheduleLink = ActionLinkHTML + "Inspections.ScheduleOneScreen.aspx?State=" + State
                             + LinkHTML
                             + getTargetURL(availableActions[1].TargetURL)
                             + "\">" + aRow.AvailableOperations[1] + "</a>";
                        break;
                    case InspectionAction.Schedule:
                        ScheduleLink = ActionLinkHTML + "Inspections.ScheduleOneScreen.aspx?State=" + State
                            + LinkHTML
                            + getTargetURL(availableActions[1].TargetURL)
                            + "\">" + aRow.AvailableOperations[1] + "</a>";
                        break;
                    case InspectionAction.Reschedule:
                        ReScheduleLink = ActionLinkHTML + "Inspections.ScheduleOneScreen.aspx?State=" + State
                             + LinkHTML
                             + getTargetURL(availableActions[1].TargetURL)
                             + "\">" + aRow.AvailableOperations[1] + "</a>";
                        break;
                    case InspectionAction.Cancel:
                        CancelLink = ActionLinkHTML + "Inspections.Cancel.aspx?State=" + State
                           + LinkHTML
                           + getTargetURL(availableActions[1].TargetURL)
                           + "\">" + aRow.AvailableOperations[1] + "</a>";
                        break;
                }
            }
        }

        // Build the list section header
        listSectionHeader.Append("<div id=\"pageSectionTitle\">");
        switch (currentListMode)
        {
            case 0:
                listSectionHeader.Append(aca_inspection_sectionname_relatedinspection);
                break;
            case 1:
                listSectionHeader.Append(inspectiondetail_statushistory);
                break;
            case 2:
                listSectionHeader.Append(inspectiondetail_resultcomments);
                break;
        }
        listSectionHeader.Append("</div>");
        //Line 1 - schedule, resulted, or updated date followed by status and sequence number
        inspectionDetails.Append("<hr>");
        inspectionDetails.Append("<div id=\"inspectionLineHistory\">");

        StringBuilder line1 = new StringBuilder();
        if (aRow.IsUpcomingInspection)
        {
            if (readyDate.Length != 0)
            {
                line1.Append(readyDate);
            }
            else
            {
                line1.Append(scheduleOrRequestDate);
                scheduleOrRequestDate = new StringBuilder();
            }
        }
        else
        {
            if (resultDate.Length != 0)
            {
                line1.Append(resultDate);
            }
            else if (lastUpdatedDate.Length != 0)
            {
                line1.Append(lastUpdatedDate);
            }
        }
        line1.Append(" ");
        line1.Append("<span id=\"pageSectionTitle\">");
        if (tempStatusString == "Fail")
        {
            line1.Append(tempStatusString + "ed");
        }
        else
        {
            line1.Append(tempStatusString);
        }
        line1.Append("</span>");
        if (useInspSequeceNumber)
        {
            line1.Append(" (");
            line1.Append(aRow.ID.ToString());
            line1.Append(")");
        }

        line1.Append("<br>");

        StringBuilder line2 = new StringBuilder();

        aSpace = string.Empty;
        if (aRow.IsUpcomingInspection && useInspector)
        {
            //Line 2 for upcomeing shows inspector that is assigned to inspection
            if (tempInspector.Length > 0)
            {
                aSpace = " ";
                line2.Append("<span id=\"pageSectionTitle\">");
                line2.Append("Inspector: ");
                line2.Append("</span>");
                line2.Append(tempInspector);
                line2.Append("<br>");
            }
        }
        if (actionAllowedLink != string.Empty || CancelLink != string.Empty)
        {
            if (isiPhone)
            {
                line2.Append("<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-top:0px; margin-bottom:0px;\"><td width=\"80%\"> </td>");
                line2.Append("<td>" + actionAllowedLink + "</td>");
                line2.Append("<td>" + CancelLink + "</td>");
                line2.Append("</table>");
            }
            else
            {
                line2.Append(actionAllowedLink + " " + CancelLink + " ");
                line2.Append("<br>");
            }
        }

        //Line 3 - show schedule, request, cancel, or reschedule links
        StringBuilder line3 = new StringBuilder();
        if (ViewDetailsLink.Length > 0)
        {
            if (isiPhone)
            {
                line3.Append("<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-top:0px; margin-bottom:0px;\">");
                // line3.Append("<td width=\"10%\"> </td>");
                line3.Append("<td>" + ViewDetailsLink.ToString() + "</td>");
                line3.Append("</table>");
            }
            else
            {
                line3.Append( ViewDetailsLink.ToString());
               line3.Append("");
            }
        }

        //Line 4 - show contact name
        StringBuilder line4 = new StringBuilder();

        if (isContactVisible == true && ((theRow.ContactFirstName != null && theRow.ContactFirstName.Length > 0)
            || (theRow.ContactLastName != null && theRow.ContactLastName.Length > 0)))
        {

            line4.Append("<div id=\"pageSectionTitle\">");
            line4.Append("Contact: ");
            line4.Append("</div>");
            line4.Append("<div id=\"pageTextIndented\">");
            line4.Append(theRow.ContactFirstName != null ? theRow.ContactFirstName : string.Empty);
            if (theRow.ContactMiddleName != null && theRow.ContactMiddleName.Length > 0)
            {
                if (line4.Length > 0)
                {
                    line4.Append(" ");
                }
                line4.Append(theRow.ContactMiddleName);
            }
            if (theRow.ContactLastName != null && theRow.ContactLastName.Length > 0)
            {
                if (line4.Length > 0)
                {
                    line4.Append(" ");
                }
                line4.Append(theRow.ContactLastName);
            }
            if (theRow.ContactPhoneNumber != null && theRow.ContactPhoneNumber.Length > 0)
            {
                line4.Append(" - ");
                if (theRow.ContactPhoneIDD != null && theRow.ContactPhoneIDD.Length > 0)
                {
                    line4.Append("(" + theRow.ContactPhoneIDD + ")");
                }
                line4.Append(theRow.ContactPhoneNumber);
            }
            line4.Append("</div>");
        }
        
        StringBuilder line5 = new StringBuilder();
        if (!aRow.IsUpcomingInspection)
        {
             if (useInspector && tempInspector.Length > 0)
            {
                line5.Append(actionByLabel + tempInspector);
            }
        }

        //Line 6 displays last updated date and by
        StringBuilder line6 = new StringBuilder();
        if (useLastUpdateBy || useLastUpdateDate)
        {
            line6.Append("<div id=\"pageSectionTitle\">");
            line6.Append("Last Updated: ");
            line6.Append("</div>");
            line6.Append("<div id=\"pageTextIndented\">"); 
            line6.Append(lastUpdatedDate);
            line6.Append(actionByLabel);
            line6.Append(tempUpdatedBy);
            line6.Append("</div>");
        }

        CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
        if (capModel.addressModel != null && capModel.addressModel.displayAddress != null)
        {
            inspectionDetails.Append("<div id=\"pageLineText\"><Label id=\"pageLineText\">");
            inspectionDetails.Append(capModel.addressModel.displayAddress);
            inspectionDetails.Append("</Label></div>");
        }
        inspectionDetails.Append(line1);
        inspectionDetails.Append(line2);
        inspectionDetails.Append(line4);
        inspectionDetails.Append(line5);
        inspectionDetails.Append(line6);

        inspectionDetails.Append(line3); //Moved scheule links to last item for each inspection

        inspectionDetails.Append("</div>");
    }
    /// <summary>
    /// Get the inspection action view parameters for 
    /// </summary>
    private string getTargetURL(string actionURL)
    {
        int questionMark = actionURL.IndexOf('?');
        if (questionMark != 0)
        {
            return "&" + actionURL.Substring(questionMark + 1);
        }
        return string.Empty;

    }
    /// <summary>
    /// list item variable keys
    /// </summary>
    public struct ListItemVariables
    {
        /// <summary>
        /// Ready Time datetime variable
        /// </summary>
        public const string ReadyTimeDateTime = "$$ReadyTimeDateTime$$";

        /// <summary>
        /// Ready Time date variable
        /// </summary>
        public const string ReadyTimeDate = "$$ReadyTimeDate$$";

        /// <summary>
        /// Ready Time time variable
        /// </summary>
        public const string ReadyTimeTime = "$$ReadyTimeTime$$";

        /// <summary>
        /// Scheduled Date Time variable
        /// </summary>
        public const string ScheduledOrRequestedDateTime = "$$ScheduledOrRequestedDateTime$$";

        /// <summary>
        /// Scheduled Date variable
        /// </summary>
        public const string ScheduledOrRequesteddDate = "$$ScheduledOrRequestedDate$$";

        /// <summary>
        /// Scheduled Time variable
        /// </summary>
        public const string ScheduledOrRequestedTime = "$$ScheduledOrRequestedTime$$";

        /// <summary>
        /// Resulted Date Time variable
        /// </summary>
        public const string ResultedDateTime = "$$ResultedDateTime$$";

        /// <summary>
        /// Resulted Date variable
        /// </summary>
        public const string ResultedDate = "$$ResultedDate$$";

        /// <summary>
        /// Resulted Time variable
        /// </summary>
        public const string ResultedTime = "$$ResultedTime$$";

        /// <summary>
        /// Inspection Type variable
        /// </summary>
        public const string InspectionType = "$$InspectionType$$";

        /// <summary>
        /// Inspection Type Sequence Number variable
        /// </summary>
        public const string InspectionTypeSequenceNumber = "$$InspectionTypeSequenceNumber$$";

        /// <summary>
        /// Inspection Sequence Number variable
        /// </summary>
        public const string InspectionSequenceNumber = "$$InspectionSequenceNumber$$";

        /// <summary>
        /// Inspector variable
        /// </summary>
        public const string Inspector = "$$Inspector$$";

        /// <summary>
        /// Status variable
        /// </summary>
        public const string Status = "$$Status$$";

        /// <summary>
        /// Operator variable
        /// </summary>
        public const string Operator = "$$Operator$$";

        /// <summary>
        /// OperatorDateTime variable
        /// </summary>
        public const string OperationDateTime = "$$OperationDateTime$$";

        /// <summary>
        /// OperatorDate variable
        /// </summary>
        public const string OperationDate = "$$OperationDate$$";

        /// <summary>
        /// OperatorTime variable
        /// </summary>
        public const string OperationTime = "$$OperationTime$$";
    }

    /// <summary>
    /// Get status label key
    /// </summary>
    /// <param name="actualStatus">the instance of InspectionStatus</param>
    /// <returns>status label key</returns>
    private static string GetLabelKey(InspectionStatus actualStatus)
    {
        string result = string.Empty;

        switch (actualStatus)
        {
            case InspectionStatus.Canceled:
                result = "ins_inspectionStatus_label_Cancelled";
                break;

            case InspectionStatus.InitialRequired:
                result = "ACA_Inspection_Status_Required";
                break;

            case InspectionStatus.InitialOptional:
                result = "ACA_Inspection_Status_Optional";
                break;

            case InspectionStatus.PendingByACA:
            case InspectionStatus.PendingByV360:
            case InspectionStatus.ResultPending:
                result = "ins_inspectionStatus_label_Pending";
                break;

            case InspectionStatus.Rescheduled:
                result = "ins_inspectionStatus_label_Rescheduled";
                break;

            case InspectionStatus.Scheduled:
                result = "ins_inspectionStatus_label_Scheduled";
                break;

            case InspectionStatus.ResultApproved:
            case InspectionStatus.ResultDenied:
            case InspectionStatus.FlowCompleted:
            case InspectionStatus.FlowCompletedButActive:
            case InspectionStatus.FlowPrerequisiteNotMet:
            case InspectionStatus.Unknown:
            default:
                result = string.Empty;
                break;
        }
        return result;
    }

}