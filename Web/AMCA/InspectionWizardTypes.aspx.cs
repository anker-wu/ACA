/**
* <pre>
* 
*  Accela Citizen Access
*  File: InspectionWizardTypes.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2012
* 
*  Description:
*  Allows user to choose an insepction type to schedule. 
* 
*  Notes:
*      $Id: InspectionWizardTypes.aspx.cs 230199 2012-08-10 01:49:02Z ACHIEVO\daly.zeng $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  01/24/2011           Dave Brewster           Created New Page./
* </pre>
*/
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Text;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.Inspection;



/// <summary>
/// 
/// </summary>
public partial class InspectionWizardTypes : AccelaPage
{
    private const string CATEGORY_OTHERS_KEY = "\fOthers\f";

    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder InspectionList = new StringBuilder();
    public StringBuilder ResultHeader = new StringBuilder();
    public StringBuilder PagingFooter = new StringBuilder();
    public StringBuilder BackForwardLinks = new StringBuilder();
    public StringBuilder HiddenFields = new StringBuilder();
    public string PageTitle = string.Empty;


    public string SearchType = string.Empty;
    public string SearchBy = string.Empty;
    public string SearchValue = string.Empty;
    public string Mode = string.Empty;
    private string PermitType = string.Empty;
    private string AltID = string.Empty;
    private string Agency = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("InspectionWizardType.aspx");
        State = GetFieldValue("State", false);

        int ResultCount = 0;
        string moduleSearchPattern = string.Empty;

        //number of records shown per page - from web.config
        AppSettingsReader Settings = new AppSettingsReader();
        int ResultRecordsPerPage = int.Parse(Settings.GetValue("SearchRecordsPerPage", typeof(string)).ToString());
        int CurrentResultPageNumber = 1;

        //get current page number , by default '1'- used while paging
        string PageResultPageNo = GetFieldValue("TypesResultPage", false);
        if (PageResultPageNo == string.Empty)
        {
            PageResultPageNo = GetFieldValue("PageResultPageNo", false);
        }
        if (PageResultPageNo != string.Empty)
        {

            CurrentResultPageNumber = int.Parse(PageResultPageNo) + 1;
        }

        PermitType = GetFieldValue("PermitType", false);
        AltID = GetFieldValue("AltID", false);
        SearchBy = GetFieldValue("SearchBy", false);
        SearchValue = GetFieldValue("SearchValue", false);
        Mode = GetFieldValue("Mode", false); 
        Agency = GetFieldValue("Agency", false); 
        bool pagingMode = GetFieldValue("PagingMode", false) == "Y";

        CapIDModel4WS capId = new CapIDModel4WS();
        if (!string.IsNullOrEmpty(Request.QueryString["Id"].ToString()))
        {
            string[] ids = Request.QueryString["Id"].ToString().Split('-');

            capId.id1 = ids[0];
            capId.id2 = ids.Length > 1 ? ids[1] : string.Empty;
            capId.id3 = ids.Length > 2 ? ids[2] : string.Empty;
        }
        capId.serviceProviderCode = ConfigManager.AgencyCode;

        InspectionProxy inspectionProxy = new InspectionProxy();

        List<InspectionTypeDataModel> inspectionsList = new List<InspectionTypeDataModel>();
        List<InspectionTypeDataModel> inspectionsVisible = new List<InspectionTypeDataModel>();

        if (Session["AMCA_WIZARD_INSPECTIONS"] != null)
        {
            inspectionsList = (List<InspectionTypeDataModel>)Session["AMCA_WIZARD_INSPECTIONS"];
        }
        string currentOptionalRequiredSetting = "Y";
        if (Session["AMCA_WIZARD_SHOW_OPTIONAL"] != null)
        {
            if (GetFieldValue("SwitchShowOptional", false) != string.Empty)
            {
                currentOptionalRequiredSetting = GetFieldValue("SwitchShowOptional", false).ToString();
            }
            else
            {
                currentOptionalRequiredSetting = Session["AMCA_WIZARD_SHOW_OPTIONAL"].ToString();
            }
        }
        bool showOptional = currentOptionalRequiredSetting == "Y";
        Session["AMCA_WIZARD_SHOW_OPTIONAL"] = currentOptionalRequiredSetting;

        StringBuilder inspectionWizardLink = new StringBuilder();
        string theCategory = GetFieldValue("CategoryID", false);
        theCategory = theCategory == "xOTHERSx" ? CATEGORY_OTHERS_KEY : theCategory;
        if (GetFieldValue("CategoryID", false) != string.Empty)
        {
            inspectionWizardLink.Append(GetFieldValue("CategoryName", false));
            InspectionList.Append("<div id=\"pageSectionTitle\">Category: " + inspectionWizardLink + "</div>");
        }
        iPhonePageTitle = "Available Inspections";
        if (isiPhone != true)
        {
            PageTitle = "<div id=\"pageTitle\">" + iPhonePageTitle + "</div><hr>";
        }
        //Build inspections type list
        if (inspectionsList.Count != 0)
        {
            StringBuilder showOrHideOptionalLink = new StringBuilder();
            showOrHideOptionalLink.Append("<a href=\"");
            showOrHideOptionalLink.Append("InspectionWizardTypes.aspx?State=" + State);
            showOrHideOptionalLink.Append("&PermitNo=" + GetFieldValue("PermitNo", false));
            showOrHideOptionalLink.Append("&PermitType=" + PermitType);
            showOrHideOptionalLink.Append("&SearchBy=" + GetFieldValue("SearchBy", false));
            showOrHideOptionalLink.Append("&SearchType=" + GetFieldValue("SearchType", false));
            showOrHideOptionalLink.Append("&SearchValue=" + GetFieldValue("SearchValue", false));
            showOrHideOptionalLink.Append("&Mode=" + Mode.ToString());
            showOrHideOptionalLink.Append("&Module=" + ModuleName.ToString());
            showOrHideOptionalLink.Append("&Agency=" + GetFieldValue("Agency", false));
            showOrHideOptionalLink.Append("&Id=" + GetFieldValue("Id", false));
            showOrHideOptionalLink.Append("&AltID=" + GetFieldValue("AltID", false));
            showOrHideOptionalLink.Append("&ViewPermitPageNo=" + GetFieldValue("ViewPermitPageNo", false));
            showOrHideOptionalLink.Append("&InspectionsPageNo=" + GetFieldValue("InspectionsPageNo", false));
            showOrHideOptionalLink.Append("&PagingMode=Y");
            showOrHideOptionalLink.Append("&SwitchShowOptional=" + (showOptional ? "N" : "Y"));
            showOrHideOptionalLink.Append("&CategoryID=" + GetFieldValue("CategoryID", false));
            showOrHideOptionalLink.Append("&CategoryName=" + GetFieldValue("CategoryName", false));
            showOrHideOptionalLink.Append("\">");
            showOrHideOptionalLink.Append(showOptional ? " (Hide Optional)" : " (Show Optional)");
            showOrHideOptionalLink.Append("</a>");

            ResultHeader.Append("<div id=\"pageSectionTitle\">Inspection Types: " + showOrHideOptionalLink + "</div>");
        }
        else
        {
            ResultHeader.Append("<div id=\"pageSectionTitle\">Inspection Types: </div>");
        }
        if (isiPhone != true)
        {
            InspectionList.Append("<div id=\"pageTextIndented\">");
            InspectionList.Append("<table cellpadding=\"5px\" cellspacing=\"0\" width=\"100%\">");
        }
       
        int rowCnt = 0;
        foreach (InspectionTypeDataModel aRow in inspectionsList)
        {
            rowCnt++;
            bool includeInList = false;
            if (aRow.AvailableOperations.Length != 0
                && aRow.AvailableOperations[0] != InspectionAction.DoPrerequisiteNotMet
                && aRow.AvailableOperations[0] != InspectionAction.None)
            {
                if (theCategory == CATEGORY_OTHERS_KEY)
                {
                    if (aRow.Categories.Length == 0)
                    {
                        includeInList = true;
                    }
                }
                else if (theCategory != CATEGORY_OTHERS_KEY && theCategory != string.Empty)
                {
                    includeInList = false;
                    if (aRow.Categories.Length > 0)
                    {
                        foreach (InspectionCategoryDataModel aCategory in aRow.Categories)
                        {
                            if (aCategory.ID == theCategory)
                            {
                                includeInList = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    includeInList = true;
                }
            }
            if (includeInList == true && (showOptional == true || (showOptional == false && aRow.Required == true)))
            {
                aRow.Group = (rowCnt - 1).ToString(); // Store the rowNumber for use by Inspection.ScheduleOneScreen
                inspectionsVisible.Add(aRow);
            }
        }

        int ResultPageStart = (CurrentResultPageNumber - 1) * ResultRecordsPerPage;
        int rowNumber = 0;
        rowCnt = 0;
        int rowsToDisplay = (inspectionsVisible.Count - ResultPageStart);
        if (rowsToDisplay > ResultRecordsPerPage)
        {
            rowsToDisplay = ResultRecordsPerPage;
        }
        ResultCount = inspectionsVisible.Count;
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
        if (inspectionsVisible.Count > 0)
        {
            ResultHeader.Append("<div style=\"margin-top:5px; margin-bottom:5px; \">" + "Showing ");
            ResultHeader.Append(FirstRecord.ToString() + "-");
            ResultHeader.Append(LastRecord.ToString() + " of " + ResultCount.ToString());
            ResultHeader.Append("</div>");
            rowCnt = 0;
            rowNumber = 0;
            foreach (InspectionTypeDataModel aRow in inspectionsVisible)
            {
                if (rowCnt >= ResultPageStart)
                {
                    rowNumber++;
                    string LinkHTML = "&Id=" + GetFieldValue("Id", false)
                       + "&PermitNo=" + GetFieldValue("PermitNo", false)
                       + "&AltID=" + AltID
                       + "&PermitType=" + PermitType
                       + "&SearchBy=" + SearchBy
                       + "&SearchType=" + GetFieldValue("SearchType", false)
                       + "&SearchValue=" + SearchValue
                       + "&ViewPermitPageNo=" + GetFieldValue("ViewPermitPageNo", false)
                       + "&InspectionsPageNo=" + GetFieldValue("InspectionPageNo", false)
                       + "&TypesResultPage=" + (CurrentResultPageNumber - 1).ToString()
                       + "&Action=" + (aRow.AvailableOperations.Length == 0 ? "" : aRow.AvailableOperations[0].ToString())
                       + "&Mode=" + (aRow.AvailableOperations.Length == 0 ? "" : aRow.AvailableOperations[0].ToString())
                       + "&Module=" + ModuleName //  aRow["ModuleName"].ToString()
                       + "&TypeRowNumber=" + aRow.Group.ToString() //This is used to hold the row number of the inspection
                       + "&InspSeqNum" + aRow.TypeID.ToString()
                       + "&inspUnits" + aRow.Units.ToString()
                       + "&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "=" + HttpUtility.UrlEncode(aRow.RestrictionSettings4Reschedule.ToString())
                       + "&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "=" + HttpUtility.UrlEncode(aRow.RestrictionSettings4Cancel.ToString());
                    StringBuilder aLink = new StringBuilder();
                    if (isiPhone == true)
                    {

                        aLink.Append("<a href=\"");
                        aLink.Append("Inspections.ScheduleOneScreen.aspx?State=" + State);
                        aLink.Append(LinkHTML);
                        aLink.Append("\">");
                        aLink.Append("<table cellpadding=\"5px\" cellspacing=\"0\" width=\"100%\">");
                        aLink.Append("<tr><td width=\"90%\" class=\"pageListLinkBold\">");
                        aLink.Append(aRow.Type);
                        aLink.Append("<span class=\"pageListText\">");
                        aLink.Append((aRow.Required ? " (required)" : " (optional)"));
                        aLink.Append("</span></td><td>");
                        if (isiPhone == true)
                        {
                            aLink.Append("<img style=\"float:right; vertical-align:middle;\" src=\"img/chevron.png\" /> ");
                        }
                        aLink.Append("</td></tr></table>");
                        aLink.Append("</a>");
                    }
                    else
                    {

                        aLink.Append("<a class=\"pageListLinkBold\" href=\"");
                        aLink.Append("Inspections.ScheduleOneScreen.aspx?State=" + State);
                        aLink.Append(LinkHTML);
                        aLink.Append("\">");
                        aLink.Append(aRow.Type);
                        aLink.Append("<span class=\"pageListText\">");
                        aLink.Append((aRow.Required ? " (required)" : " (optional)"));
                        aLink.Append("</span></a>");
                    }
                    InspectionList.Append(MyProxy.CreateSelectListCell(aLink.ToString(), rowNumber - 1, rowCnt, ResultCount, ResultPageStart, ResultRecordsPerPage, isiPhone, false));
                    if (rowNumber == rowsToDisplay)
                    {
                        break;
                    }
                }
                rowCnt++;
            }
        }
        else
        {
            InspectionList.Append("There are no available inspections for this record.");
        }
        if (isiPhone != true)
        {
            InspectionList.Append("</table>");
            InspectionList.Append("</div>");
        }

        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;

        StringBuilder sbWork = new StringBuilder();
        sbWork.Append("&PermitNo=" + GetFieldValue("PermitNo", false));
        sbWork.Append("&PermitType=" + PermitType);
        sbWork.Append("&SearchBy=" + GetFieldValue("SearchBy", false));
        sbWork.Append("&SearchType=" + GetFieldValue("SearchType", false));
        sbWork.Append("&SearchValue=" + GetFieldValue("SearchValue", false));
        sbWork.Append("&Mode=" + Mode.ToString());
        sbWork.Append("&Module=" + ModuleName.ToString());
        sbWork.Append("&Agency=" + GetFieldValue("Agency", false));
        sbWork.Append("&Id=" + GetFieldValue("Id", false));
        sbWork.Append("&AltID=" + GetFieldValue("AltID", false));
        sbWork.Append("&ViewPermitPageNo=" + GetFieldValue("ViewPermitPageNo", false));
        sbWork.Append("&InspectionsPageNo=" + GetFieldValue("InspectionsPageNo", false));
        sbWork.Append("&TypesResultPage=" + (CurrentResultPageNumber - 1).ToString());
        sbWork.Append("&ShowOptioal=" + GetFieldValue("ShowOptioal", false));
        sbWork.Append("&CategoryID=" + GetFieldValue("CategoryID", false));
        sbWork.Append("&CategoryName=" + GetFieldValue("CategoryName", false));

        Breadcrumbs = BreadCrumbHelper("InspectionWizardTypes.aspx", sbWork, iPhonePageTitle, breadCrumbIndex, isElipseLink, pagingMode, pagingMode, false);
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

            PagingLink.Append(sLinkFormat + "InspectionWizardTypes.aspx?State=" + State);
            PagingLink.Append("&PagingMode=Y");
            PagingLink.Append("&PermitNo=" + GetFieldValue("PermitNo", false));
            PagingLink.Append("&PermitType=" + PermitType);
            PagingLink.Append("&SearchBy=" + GetFieldValue("SearchBy", false));
            PagingLink.Append("&SearchType=" + GetFieldValue("SearchType", false));
            PagingLink.Append("&SearchValue=" + GetFieldValue("SearchValue", false));
            PagingLink.Append("&Mode=" + Mode.ToString());
            PagingLink.Append("&Module=" + ModuleName.ToString());
            PagingLink.Append("&Agency=" + GetFieldValue("Agency", false));
            PagingLink.Append("&Id=" + GetFieldValue("Id", false));
            PagingLink.Append("&AltID=" + GetFieldValue("AltID", false));
            PagingLink.Append("&ViewPermitPageNo=" + GetFieldValue("ViewPermitPageNo", false));
            PagingLink.Append("&InspectionsPageNo=" + GetFieldValue("InspectionsPageNo", false));
            PagingLink.Append("&ShowOptioal=" + GetFieldValue("ShowOptioal", false));
            PagingLink.Append("&CategoryID=" + GetFieldValue("CategoryID", false));
            PagingLink.Append("&CategoryName=" + GetFieldValue("CategoryName", false));


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
                PagingFooter.Append("&TypesResultPage=" + (PreviousPageNumber - 1).ToString());
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
                    PagingFooter.Append("&TypesResultPage=" + (page - 1).ToString());
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
                PagingFooter.Append("&TypesResultPage=" + (NextPageNumber - 1).ToString());
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
}
