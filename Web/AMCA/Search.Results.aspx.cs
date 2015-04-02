/**
* <pre>
* 
*  Accela Citizen Access
*  File: SearchResults.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2012
* 
*  Description:
*  View inspection list for current record. 
* 
*  Notes:
*      $Id: SearchResults.aspx.cs 77905 2007-10-15 12:49:28Z ACHIEVO\Jackie.Yu $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  07-18-2008           DWB                     2008 Mobile ACA interface redesign
*  10-09-2008           DWB                     2008 Mobile ACA 6.7.0 interface redesign
*  11/16/2008           DWB                     Additional modfiication to make the ACA Mobile inspections list
*                                               act like the ACA inspections list.
*  04/01/2009           Dave Brewster           Modified to display AltID instead of the three segment CAP id.
*  10-10-2009           Dave Brewster           Added new breadcrumb logic.
* </pre>
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using System.Text;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.WSProxy;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Inspection;
//using Accela.ACA.WSProxy;

// using Accela.ACA.WSProxy.WSModel;
// using Accela.ACA.Html.Inspection;
using Accela.ACA.Inspection;

/// <summary>
/// 
/// </summary>
public partial class SearchResults : AccelaPage
{
    public StringBuilder PageHeading    = new StringBuilder();
    public StringBuilder PagingFooter   = new StringBuilder();
    public string SearchType            = string.Empty;
    public StringBuilder ResultOutput   = new StringBuilder();
    public string SearchBy              = string.Empty;
    public string SearchValue           = string.Empty;
    public string Filter                = string.Empty;
    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder InspectionWizardLink = new StringBuilder();
    
    public string BackForwardLinks = string.Empty;
    public string Mode = string.Empty;
    public string Breadcrumbs = string.Empty;

    public Inspection ThisInspection = new Inspection();
    private string PermitType = string.Empty;
    private string ViewPermitPageNo = string.Empty;
    private string AltID = string.Empty;
    private string Agency = string.Empty;
    // private string Module = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
         ValidationChecks("Search.Results.aspx");

        //number of records shown per page - from web.config
        AppSettingsReader Settings = new AppSettingsReader();
        int ResultRecordsPerPage = int.Parse(Settings.GetValue("SearchRecordsPerPage",typeof(string)).ToString());

        // set start up page number
        int CurrentResultPageNumber = 1;
        if (GetFieldValue("ResultPage", false) != string.Empty)
        {
            if (int.TryParse(GetFieldValue("ResultPage", false).ToString(), out CurrentResultPageNumber))
            {
                CurrentResultPageNumber++;
            }
            else
            {
                CurrentResultPageNumber = 1;
            }
        }

        int ResultCount = 0;

        SearchType = GetFieldValue("SearchType", false);
   
        //get current page number , by default '1'- used while paging
        // get search values - used while paging
        PermitType  = (Request.QueryString["PermitType"] != null) ? Request.QueryString["PermitType"] : (Request.Form["PermitType"] != null ? Request.Form["PermitType"].ToString() : string.Empty);
        AltID       = (Request.QueryString["AltID"] != null) ? Request.QueryString["AltID"] :  string.Empty;
        SearchBy    = (Request.QueryString["SearchBy"] != null) ? Request.QueryString["SearchBy"] : (Request.Form["SearchBy"] != null ? Request.Form["SearchBy"].ToString() : string.Empty);
        SearchValue = (Request.QueryString["SearchValue"] != null) ? Request.QueryString["SearchValue"] : (Request.Form["SearchValue"] != null ? Request.Form["SearchValue"].ToString() : string.Empty);
        Mode        = (Request.QueryString["Mode"] != null) ? Request.QueryString["Mode"] : (Request.Form["Mode"] != null ? Request.Form["Mode"].ToString() : string.Empty);
        Agency      = (Request.QueryString["Agency"] != null) ? Request.QueryString["Agency"] : string.Empty;
        
        // Module      = (Request.QueryString["Module"] != null) ? Request.QueryString["Module"] : string.Empty;

        ViewPermitPageNo = (Request.QueryString["ViewPermitPageNo"] != null) ? Request.QueryString["ViewPermitPageNo"] : (Request.Form["ViewPermitPageNo"] != null ? Request.Form["SearchValue"].ToString() : string.Empty);
        if (ViewPermitPageNo == string.Empty)
        {
            ViewPermitPageNo = "0";
        }

        iPhonePageTitle = SearchType;
        if (isiPhone != true)
        {
            PageHeading.Append("<div id=\"pageTitle\">" + SearchType + "</div>");
            PageHeading.Append("<hr />");
        }

        InspectionWizardLink.Append("<div>");
        InspectionWizardLink.Append("<a class=\"inspectionLineLink\" href=\"");
        InspectionWizardLink.Append("InspectionWizard.aspx?State=" + State);
        InspectionWizardLink.Append("&PermitNo=" + Request.QueryString["Id"].ToString());
        InspectionWizardLink.Append("&PermitType=" + PermitType);
        InspectionWizardLink.Append("&SearchBy=Permit");
        InspectionWizardLink.Append("&SearchType=Inspections");
        InspectionWizardLink.Append("&SearchValue=" + SearchValue);
        InspectionWizardLink.Append("&Mode=" + Mode);
        InspectionWizardLink.Append("&Id=" + GetFieldValue("Id", false));
        InspectionWizardLink.Append("&AltID=" + AltID);
        InspectionWizardLink.Append("&Agency=" + GetFieldValue("Agency", false));
        InspectionWizardLink.Append("&Module=" + ModuleName);
        InspectionWizardLink.Append("&ViewPermitPageNo=" + ViewPermitPageNo);
        InspectionWizardLink.Append("&InspectionsPageNo=" +(CurrentResultPageNumber - 1).ToString());
        InspectionWizardLink.Append("&ShowOptional=Y");
        InspectionWizardLink.Append("\">");
        InspectionWizardLink.Append(LocalGetTextByKey("aca_inspection_schedule_link"));
        InspectionWizardLink.Append("</a>");

        if (Session["AMCA_WIZARD_INSPECTIONS"] != null)
        {
            Session.Remove("AMCA_WIZARD_INSPECTIONS");
        }
        if (Session["AMCA_WIZARD_CATEGORIES"] != null)
        {
            Session.Remove("AMCA_WIZARD_CATEGORIES");
        }
        Session["AMCA_BREADCRUMB_ADJUSTMENT"] = "-2";

#region Inspection Search
 
            Inspection SearchInspection = new Inspection();

            CapIDModel4WS capId = new CapIDModel4WS();
            if (!string.IsNullOrEmpty(Request.QueryString["Id"].ToString()))
            {
                string[] ids = Request.QueryString["Id"].ToString().Split('-');

                capId.id1 = ids[0];
                capId.id2 = ids.Length > 1 ? ids[1] : string.Empty;
                capId.id3 = ids.Length > 2 ? ids[2] : string.Empty;
            }
            capId.serviceProviderCode = ConfigManager.AgencyCode;

           // DataTable inspectionDT = null;
            if (Session["AMCA_INSPECTION_MODELS"] != null)
            {
                Session.Remove("AMCA_INSPECTION_MODELS");
            }
            List<InspectionViewModel> inspectionDT = new List<InspectionViewModel>();
            InspectionProxy inspectionProxy = new InspectionProxy();
            Dictionary<string, UserRolePrivilegeModel> sectionPermissions = AccelaProxy.GetSectionPermissions(ModuleName);
            bool isInspectionVisible = AccelaProxy.GetSectionVisibility(CapDetailSectionType.INSPECTIONS.ToString(), sectionPermissions, ModuleName);

            if (isInspectionVisible)
            {
                inspectionDT = inspectionProxy.GetInspectionDataModelsByCapID(ModuleName); //capId, null, AppSession.User.UserSeqNum);
                if (inspectionDT == null || inspectionDT.Count == 0)
                {
                    // string temp = LocalGetTextByKey("aca_inspection_upcoming_emptydata_label");
                    ///InspectionWizardLink.Append(temp);
                }
                else if (inspectionProxy.OnErrorReturn)
                {  // Proxy Exception 
                    InspectionWizardLink = new StringBuilder();
                    ErrorMessage.Append(inspectionProxy.ExceptionMessage);
                }
                else
                {
                    Session["AMCA_INSPECTION_MODELS"] = inspectionDT;
                }
            }
            else
            {
                ErrorMessage.Append("You do not have permission to view inspection details! Contact you system administrator.");
            }

            StringBuilder[] ListRows = new StringBuilder[((inspectionDT != null && inspectionDT.Count > 0) ? inspectionDT.Count : 1)];
           
            int RowCnt = 0;

            bool isBreadcrumbPagingMode = GetFieldValue("PagingMode", false) == "true";

            string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
            bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;

            StringBuilder sbWork = new StringBuilder();
            sbWork.Append("&SearchBy=" + GetFieldValue("SearchBy", false));
            sbWork.Append("&SearchType=" + GetFieldValue("SearchType", false));
            sbWork.Append("&Mode=" + Mode.ToString());
            sbWork.Append("&Module=" + ModuleName.ToString());
            sbWork.Append("&Agency=" + GetFieldValue("Agency", false));
            sbWork.Append("&Id=" + GetFieldValue("Id", false));
            sbWork.Append("&PermitType=" + PermitType);
            sbWork.Append("&AltID=" + GetFieldValue("AltID", false));
            sbWork.Append("&ViewPermitPageNo=" + GetFieldValue("ViewPermitPageNo", false));
            sbWork.Append("&ResultPage=" + (CurrentResultPageNumber - 1).ToString());

            Breadcrumbs = BreadCrumbHelper("Search.Results.aspx", sbWork, SearchType, breadCrumbIndex, isElipseLink, false, isBreadcrumbPagingMode, true).ToString();

            iPhoneHideFooterBar = (inspectionDT == null || inspectionDT.Count == 0);
            BackForwardLinks = BackLinkHelper(CurrentBreadCrumbIndex.ToString());
            iPhoneHideFooterBar = false;
            int upcomingRows = 0;
            int completedRows = 0;

            var inspectionPermissionBll = ObjectFactory.GetObject<IInspectionPermissionBll>();
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            bool isContactVisible = inspectionPermissionBll.CheckContactRight(capModel, capModel.capID.serviceProviderCode, ModuleName, ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_VIEW_CONTACT);
        
            if (inspectionDT != null && inspectionDT.Count > 0)
            {

                ResultCount = inspectionDT.Count;
                bool[] rowProcessed = new bool[ResultCount];

                int ResultPageStart = (CurrentResultPageNumber - 1) * ResultRecordsPerPage;
                // Populate Records 
                RowCnt = 0;
                // Load upcoming inspection first
                StringBuilder linkURL = new StringBuilder();
                linkURL.Append("&PermitNo=" + GetFieldValue("Id", false));
                linkURL.Append("&AltID=" + AltID);
                linkURL.Append("&PermitType=" + PermitType);
                linkURL.Append("&SearchBy=" + SearchBy);
                linkURL.Append("&SearchType=" + SearchType);
                linkURL.Append("&SearchValue=" + SearchValue);
                linkURL.Append("&ViewPermitPageNo=" + ViewPermitPageNo);
                linkURL.Append("&InspectionsPageNo=" + (CurrentResultPageNumber - 1).ToString());
                linkURL.Append("&Mode=" + Mode);
                linkURL.Append("&ModuleName=" + ModuleName);
                for (int x = 0; x < ResultCount; x++)
                 {
                    InspectionViewModel aRow = inspectionDT[x];
                    StringBuilder rowURL = new StringBuilder();
                    rowURL.Append(linkURL.ToString());
                    rowURL.Append("&InspectionId=" + aRow.ID);
                    rowURL.Append("&RowNumber=" + x.ToString());
                    
                    if (rowProcessed[x] != true && aRow.InspectionDataModel.IsUpcomingInspection == true)
                    {
                        rowProcessed[x] = true;
                        CapIDModel capIdModel = TempModelConvert.Trim4WSOfCapIDModel(capId);

                        createRow(capIdModel, aRow, x, rowURL.ToString(), ListRows, RowCnt, false, isContactVisible);
                        upcomingRows++;

                            // add upcoming inspections first to top of list
                            for (int histRow = 0; histRow < inspectionDT.Count; histRow++)
                            {
                                InspectionViewModel aRow2 = inspectionDT[histRow];
                                if (rowProcessed[histRow] != true 
                                    && aRow.TypeID == aRow2.TypeID
                                    && aRow2.InspectionDataModel.IsUpcomingInspection)
                                {
                                    rowURL = new StringBuilder();
                                    rowURL.Append(linkURL.ToString());
                                    rowURL.Append("&InspectionId=" + aRow2.ID);
                                    rowURL.Append("&RowNumber=" + histRow.ToString());
                                    rowProcessed[histRow] = true;
                                    createRow(capIdModel, aRow2, histRow, rowURL.ToString(), ListRows, RowCnt, true, isContactVisible);
                                    upcomingRows++;
                                }
                            }
                            // then add completed inspections
                            for (int histRow = 0; histRow < inspectionDT.Count; histRow++)
                            {
                                InspectionViewModel aRow2 = inspectionDT[histRow];
                                if (rowProcessed[histRow] != true
                                    && aRow.TypeID == aRow2.TypeID)
                                {
                                    rowURL = new StringBuilder();
                                    rowURL.Append(linkURL.ToString());
                                    rowURL.Append("&InspectionId=" + aRow2.ID);
                                    rowURL.Append("&RowNumber=" + histRow.ToString());
                                    rowProcessed[histRow] = true;
                                    createRow(capIdModel, aRow2, histRow, rowURL.ToString(), ListRows, RowCnt, true, isContactVisible);
                                    completedRows++;
                                }
                            }
                        RowCnt++;
                    }
                }
                // Load completed inspection second
                for (int x = 0; x < ResultCount; x++)
                {
                    InspectionViewModel aRow = inspectionDT[x];
                    StringBuilder rowURL = new StringBuilder();
                    rowURL.Append(linkURL.ToString());
                    rowURL.Append("&InspectionId=" + aRow.ID);
                    rowURL.Append("&RowNumber=" + x.ToString());

                    // + "&Module=" + ModuleName; //  aRow["ModuleName"].ToString()

                    if (rowProcessed[x] != true && aRow.InspectionDataModel.IsUpcomingInspection == false)
                    {
                        rowProcessed[x] = true;
                        CapIDModel capIdModel = TempModelConvert.Trim4WSOfCapIDModel(capId);

                        createRow(capIdModel, aRow, x, rowURL.ToString(), ListRows, RowCnt, false, isContactVisible);
                        completedRows++;

                            for (int histRow = 0; histRow < inspectionDT.Count; histRow++)
                            {
                                InspectionViewModel aRow2 = inspectionDT[histRow];
                                if (rowProcessed[histRow] != true
                                    && aRow.TypeID == aRow2.TypeID)
                                {
                                    rowURL = new StringBuilder();
                                    rowURL.Append(linkURL.ToString());
                                    rowURL.Append("&InspectionId=" + aRow2.ID);
                                    rowURL.Append("&RowNumber=" + histRow.ToString());
                                    rowProcessed[histRow] = true;
                                    createRow(capIdModel, aRow2, histRow, rowURL.ToString(), ListRows, RowCnt, true, isContactVisible);
                                    completedRows++;
                                }
                            }
                        RowCnt++;
                    }
                }
            }

            if (upcomingRows == 0)
            {
                string temp = LocalGetTextByKey("aca_inspection_upcoming_emptydata_label");
                InspectionWizardLink.Append("<br>");
                InspectionWizardLink.Append(temp);
                if (completedRows == 0)
                {
                    InspectionWizardLink.Append("<br>");
                }
            }
            if (completedRows == 0)
            {
                InspectionWizardLink.Append("<br>");
                string temp = LocalGetTextByKey("aca_inspection_completed_emptydata_label");
                InspectionWizardLink.Append(temp);
            }
            InspectionWizardLink.Append("</div>");
            InspectionWizardLink.Append("<br>");

            if (RowCnt != 0)
            {
                ResultCount = RowCnt;
                int ResultPageStart = (CurrentResultPageNumber - 1) * ResultRecordsPerPage;
                int SkipCnt = 0;
                // Populate Records 
                int theCnt = 0;
                for (int x = ResultPageStart; x < (((ResultPageStart + ResultRecordsPerPage) < ResultCount) ? (ResultPageStart + ResultRecordsPerPage) : ResultCount); x++)
                {
                    ResultOutput.Append(MyProxy.InspectionListCell(
                        "<div id=\"inspectionLine\">" + ListRows[x].ToString() + "</div>",
                        theCnt,
                        x, 
                        ResultCount, 
                        ResultPageStart, 
                        ResultRecordsPerPage, 
                        true,
                        false));
                    theCnt++;
                }
                ResultCount -= SkipCnt;
            }

            #endregion

        #region Paging Part

        // Paging Part, appends to table footer
        // DWB - 07-25-2008 - Only include paging links if more than on page of records
        //                    are returned from the search.
        if ((double)ResultCount > (double)ResultRecordsPerPage)
        {
            int ResultPagesCount = (int)Math.Ceiling((double)ResultCount / (double)ResultRecordsPerPage);
            if ((int)Math.Ceiling((double)ResultPagesCount * (double)ResultRecordsPerPage) < ResultCount)
            {
                ResultPagesCount++;
            }
            // DWB - 02-26-2008 - Change paging according to specification is 2008 redesign document.
            int NextPageNumber = 0;
            int PageCount = 0;
            int PreviousPageNumber = CurrentResultPageNumber - 1;
            int PageStart = 0;
            if ((CurrentResultPageNumber - ((CurrentResultPageNumber / 10) * 10)) > 0)
            {
                PageStart = ((CurrentResultPageNumber / 10) * 10) + 1;
            }
            else
            {
                if (CurrentResultPageNumber == ResultPagesCount)
                {
                    PageStart = CurrentResultPageNumber - 9;
                }
                else
                {
                    PageStart = 1;
                }
            }
            string sLinkFormat = "<a id=\"pageNavigationButton\" href=\"";

            PagingFooter.Append("<div id=\"pageNavigation\">");
            if (isiPhone == true)
            {
                PagingFooter.Append("<center>");
            }
            if (PreviousPageNumber > 0)
            {
                PagingFooter.Append(sLinkFormat + "Search.Results.aspx?State=" + State);
                PagingFooter.Append("&id=" + Request.QueryString["Id"].ToString());
                PagingFooter.Append("&AltID=" + AltID.ToString());
                PagingFooter.Append("&SearchBy=" + SearchBy);
                PagingFooter.Append("&SearchType=" + SearchType);
                PagingFooter.Append("&SearchValue=" + SearchValue);
                // PagingFooter.Append("&InspectionStatus=" + Filter);
                PagingFooter.Append("&ViewPermitPageNo=" + ViewPermitPageNo);
                PagingFooter.Append("&ResultPage=" + (PreviousPageNumber - 1).ToString());
                PagingFooter.Append("&Mode=" + Mode);
                PagingFooter.Append("&Module=" + ModuleName);
                PagingFooter.Append("&PagingMode=true");
                PagingFooter.Append("&SlidePage=LeftToRight");
                PagingFooter.Append("\">&lt;</a>&nbsp;");
            }
            for (int page = PageStart; page <= ResultPagesCount; page++)
            {
                PageCount++;
                if (PageCount > 10)
                {
                    break;
                }
                if (CurrentResultPageNumber == page)
                {
                    PagingFooter.Append("<span id=\"pageNavigationSelected\">" + page + "&nbsp;</span>");
                    NextPageNumber = page + 1;
                }
                else
                {
                    PagingFooter.Append(sLinkFormat + "Search.Results.aspx?State=" + State);
                    if (CurrentResultPageNumber > page)
                    {
                        PagingFooter.Append("&SlidePage=LeftToRight");
                    }
                    PagingFooter.Append("&id=" + Request.QueryString["Id"].ToString());
                    PagingFooter.Append("&AltID=" + AltID.ToString());
                    PagingFooter.Append("&SearchBy=" + SearchBy);
                    PagingFooter.Append("&SearchType=" + SearchType);
                    PagingFooter.Append("&SearchValue=" + SearchValue);
                    // PagingFooter.Append("&InspectionStatus=" + Filter);
                    PagingFooter.Append("&ResultPage=" + (page - 1).ToString());
                    PagingFooter.Append("&ViewPermitPageNo=" + ViewPermitPageNo);
                    PagingFooter.Append("&Mode=" + Mode);
                    PagingFooter.Append("&Module=" + ModuleName);
                    PagingFooter.Append("&PagingMode=true");
                    PagingFooter.Append("\">");
                    PagingFooter.Append(page.ToString() + "</a>&nbsp;");
                }
            }
            if (NextPageNumber <= ResultPagesCount)
            {
                PagingFooter.Append(sLinkFormat + "Search.Results.aspx?State=" + State
                    + "&id=" + Request.QueryString["Id"].ToString()
                    + "&AltID=" + AltID.ToString()
                    + "&SearchBy=" + SearchBy
                    + "&SearchType=" + SearchType
                    + "&SearchValue=" + SearchValue
                    // + "&InspectionStatus=" + Filter
                    + "&ResultPage=" + (NextPageNumber - 1).ToString()
                    + "&ViewPermitPageNo=" + ViewPermitPageNo
                    + "&Mode=" + Mode
                    + "&Module=" + ModuleName
                    + "&PagingMode=true"
                    + "\">&gt;</a>&nbsp;");
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
    ///  Indicates whether need to display the schedule, reSchedule or cannel link.
    ///  True - Display all schedule, reSchedule or cannel link to list.
    ///  No  -  The schedule, reSchedule or cannel link shouldn't be displayed to list.
    /// </summary>
    private void WriteToLog(string message)
    {
        try
        {
            string FileLoc = "c:\\ACAMobileDevLog.txt";
            if (FileLoc.ToString().Trim() != "")
            {
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(FileLoc))
                {
                    sw.WriteLine(message);
                    sw.Close();
                }

            }
        }
        catch 
        {
            // ignore errors and continue
        }
    }
   
    /// <summary>
    /// Edit RESULT comment for greater than 90 characters.
    /// </summary>
    /// <param name="commentToEdit"> Comment to Edit </param>
    /// <param name="rowID"> DataModel row for comment</param>
    /// <param name="LinkHTML"> link parametrs to pass to for to view comments </param>
    /// <returns>String</returns>
    // DWB - 07-18-2008 - Added new funciton.
    private string editRequestComment(string commentToEdit, int rowID, string LinkHTML)
    {
        string TheValue = string.Empty;
        if (commentToEdit != null)
        {
            if (commentToEdit.Length > 90)
            {
                Session["AMCA_Request_Comment" + rowID.ToString() + "0"] = commentToEdit;
                TheValue = commentToEdit.Substring(0, 90)
                    + "..." + "<a id=\"inspectionRequestCommentLink\" href=\"" + "View.Details.aspx?State=" + State
                    + "&Type=Inspection"
                    + "&DTRow=" + rowID.ToString()
                    + "&DTkey=AMCA_Request_Comment" + rowID.ToString() + "0"
                    + LinkHTML
                    + "&Module=" + ModuleName
                    + "\">" + " More</a>";
            }
            else
            {
                if (commentToEdit.Length > 0)
                {
                    TheValue = "<span id=\"inspecitonRequestComment\">" + commentToEdit + "</span>";
                }
            }
        }
        return TheValue;
    }

    /// <summary>
    /// Edit REQUEST comment for greater than 90 characters.
    /// </summary>
    /// <param name="commentToEdit"> Comment to Edit </param>
    /// <param name="rowID"> DataModel row for comment</param>
    /// <param name="LinkHTML"> link parametrs to pass to for to view comments </param>
    /// <returns>String</returns>
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
    /// Format an inspection to be displayed in the list.
    /// </summary>
    /// <param name="capIdModel"> The capIdModel that the inspection is associated to </param>
    /// <param name="theRow"> InspectionViewModel to be formatted</param>
    /// <param name="x"> The index row id of the inspections list that is being parsed</param>
    /// <param name="baseLinkHTML"> link parametrs to pass when teh inspection is select to schedule/cancel/reschedule </param>
    /// <param name="x"> The index row id of the list that is being parsed</param>
    /// <param name="RowCnt"> The row in the list that is visible on the page</param>
    /// <param name="scanningForHistory"> Indicates that an inspection type's history list is being formatted</param>
    /// <param name="isContactVisible"> Indicates whether or not the contcat name is visible.</param>
    /// <returns>void</returns>
    // DWB - 07-18-2008 - Added new funciton.
    private void createRow(CapIDModel capIdModel, 
        InspectionViewModel theRow, 
        int x, 
        string baseLinkHTML, 
        StringBuilder[] ListRows, 
        int RowCnt, 
        bool scanningForHistory,
        bool isContactVisible)
    {
        InspectionDataModel aRow = theRow.InspectionDataModel;
   
        List<InspectionActionViewModel> availableActions = InspectionActionViewUtil.BuildViewModels(ConfigManager.AgencyCode, capIdModel, ModuleName, AppSession.User.PublicUserId, theRow);

        string LinkHTML = baseLinkHTML
                        + "&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "=" + HttpUtility.UrlEncode(aRow.RestrictionSettings4Reschedule.ToString())
                        + "&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "=" + HttpUtility.UrlEncode(aRow.RestrictionSettings4Cancel.ToString());

        string ActionLinkHTML = "<a id=\"inspectionLineLink\" href=\"";
        Inspection ThisInspection = new Inspection();

        string ScheduleLink = string.Empty;
        string ReScheduleLink = string.Empty;
        string actionAllowedLink = string.Empty; 
        string CancelLink = string.Empty;
        string ViewDetailsLink = string.Empty;

        string inspectionComments = editResultComment(aRow.ResultComments, x, LinkHTML); // aRow["Comments"].ToString();
        string requestComments = editRequestComment(aRow.RequestComments, x, LinkHTML);
 
        string textTBD = LabelUtil.GetTextByKey("ACA_Inspection_BlankInspector_TBD", ModuleName);
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
        bool useLastUpdateDateTime = false;
        bool useLastUpdateDate = false;
        bool useLastUpdateTime = false;
        bool useLastUpdateBy = false;
        bool useInspSequeceNumber = false;
        bool useInspector = false;
        bool isRequestPending = false;
        bool isRescheduledOrCanceled = false;
        String actionByLabel = string.Empty;

        if (aRow.IsUpcomingInspection)
        {
            isRequestPending = aRow.ScheduleType == InspectionScheduleType.RequestOnlyPending;
            if (isRequestPending && aRow.ReadyTimeEnabled)
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
                pattern = LabelUtil.GetTextByKey("aca_inspection_upcominglist_combofield_pattern", ModuleName);
                if (aRow.Status == InspectionStatus.Scheduled || aRow.Status == InspectionStatus.Rescheduled)
                {
                    isRequestPending = false;
                }
            }
        }
        else
        {
            if (aRow.Status == InspectionStatus.Rescheduled || aRow.Status == InspectionStatus.Canceled)
            {
                isRescheduledOrCanceled = true;
                //By <Operator> on <operation date> at <operate time>
                if (aRow.Status == InspectionStatus.Rescheduled)
                {
                    pattern = LabelUtil.GetTextByKey("aca_inspection_completedlist_rescheduled_combofield_pattern", ModuleName);
                    actionByLabel = "Rescheduled by: ";
                }
                else
                {
                    pattern = LabelUtil.GetTextByKey("aca_inspection_completedlist_cancelled_combofield_pattern", ModuleName);
                    actionByLabel = "Cancelled by: ";
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
                actionByLabel = "Result by: ";
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

        //pattern = pattern.Replace(ListItemVariables.InspectionType, inspectionViewModel.Type);
        //pattern = pattern.Replace(ListItemVariables.InspectionTypeSequenceNumber, inspectionViewModel.TypeID.ToString());
        //pattern = pattern.Replace(ListItemVariables.Status, inspectionViewModel.Status);

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
            lastUpdatedDate.Append(lastUpdatedDateTimeValueOk ? I18nDateTimeUtil.FormatToDateTimeStringForUI(lastUpdatedDateTimeValue) : textTBD);
        }
        else
        {
            if (useLastUpdateDate)
            {
                lastUpdatedDate.Append(lastUpdatedDateTimeValueOk ? I18nDateTimeUtil.FormatToDateStringForUI(lastUpdatedDateTimeValue) : textTBD);
                aSpace = " at ";
            }
            if (useLastUpdateTime)
            {
                lastUpdatedDate.Append(aSpace);
                lastUpdatedDate.Append(lastUpdatedDateTimeValueOk ? I18nDateTimeUtil.FormatToTimeStringForUI(lastUpdatedDateTimeValue, true) : textTBD);
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
        
        if (aRow.AvailableOperations.Length > 0)
            {
            int viewDetailsIndex = -1;

            for (viewDetailsIndex = 0; viewDetailsIndex < aRow.AvailableOperations.Length; viewDetailsIndex++)
            {
                if (aRow.AvailableOperations[viewDetailsIndex] == InspectionAction.ViewDetails)
                {
                    break;
                }
            }
            if (viewDetailsIndex < aRow.AvailableOperations.Length)
            {
                // int children = relatedInsp.children[0].children != null ? relatedInsp.children[0].children.Length : 0;
                // children += relatedInsp.inspectionModel != null ? 1 : 0;
                ViewDetailsLink = ActionLinkHTML + "Inspections.Details.aspx?State=" + State
                  + LinkHTML
                  + "&CurrentListMode=0"
                  + getTargetURL(availableActions[viewDetailsIndex].TargetURL)
                  + "\">View Details</a>";
                  // +"\">" + LabelUtil.RemoveHtmlFormat(LocalGetTextByKey("aca_inspection_sectionname_relatedinspection")) + " (" + children.ToString() + ")</a>";
             }
        }

        if (aRow.AvailableOperations.Length > 0 && aRow.IsUpcomingInspection)
        {
            int actionIndex = 0;
            for (actionIndex = 0; actionIndex < availableActions.Count; actionIndex++)
            {
                if (aRow.AvailableOperations[0] == availableActions[actionIndex].Action)
                {
                    break;
                }
            }
            if (actionIndex > availableActions.Count)
            {
                actionIndex = 0;
            }
            string targetURL = getTargetURL(availableActions[actionIndex].TargetURL);
            switch (aRow.AvailableOperations[0])
            {
                case InspectionAction.Request:
                    ScheduleLink = ActionLinkHTML + "Inspections.ScheduleOneScreen.aspx?State=" + State
                         + LinkHTML
                         + targetURL
                         + "\">" + aRow.AvailableOperations[0] + "</a>";
                          actionAllowedLink = ScheduleLink;
                    break;
                case InspectionAction.Schedule:
                    ScheduleLink = ActionLinkHTML + "Inspections.ScheduleOneScreen.aspx?State=" + State
                        + LinkHTML
                        + targetURL
                        + "\">" + aRow.AvailableOperations[0] + "</a>";
                   actionAllowedLink = ScheduleLink;
                    break;
                case InspectionAction.Reschedule:
                    ReScheduleLink = ActionLinkHTML + "Inspections.ScheduleOneScreen.aspx?State=" + State
                        + LinkHTML
                        + targetURL
                        + "\">" + aRow.AvailableOperations[0] + "</a>";
                    actionAllowedLink = ReScheduleLink;
                    break;
                case InspectionAction.Cancel:
                    CancelLink = ActionLinkHTML + "Inspections.Cancel.aspx?State=" + State
                       + LinkHTML
                       + targetURL
                       + "\">" + aRow.AvailableOperations[0] + "</a>";
                     break;
            }
            if (aRow.AvailableOperations.Length > 2)
            {
                for (actionIndex = 0; actionIndex < availableActions.Count; actionIndex++)
                {
                    if (aRow.AvailableOperations[1] == availableActions[actionIndex].Action)
                    {
                        break;
                    }
                }
                if (actionIndex > availableActions.Count)
                {
                    actionIndex = 1;
                }
                targetURL = getTargetURL(availableActions[actionIndex].TargetURL);
                switch (aRow.AvailableOperations[1])
                {
                    case InspectionAction.Request:
                        ScheduleLink = ActionLinkHTML + "Inspections.ScheduleOneScreen.aspx?State=" + State
                             + LinkHTML
                             + targetURL
                             + "\">" + aRow.AvailableOperations[1] + "</a>";
                        break;
                    case InspectionAction.Schedule:
                        ScheduleLink = ActionLinkHTML + "Inspections.ScheduleOneScreen.aspx?State=" + State
                            + LinkHTML
                            + targetURL
                            + "\">" + aRow.AvailableOperations[1] + "</a>";
                        break;
                    case InspectionAction.Reschedule:
                        ReScheduleLink = ActionLinkHTML + "Inspections.ScheduleOneScreen.aspx?State=" + State
                             + LinkHTML
                             + targetURL
                             + "\">" + aRow.AvailableOperations[1] + "</a>";
                        break;
                    case InspectionAction.Cancel:
                        CancelLink = ActionLinkHTML + "Inspections.Cancel.aspx?State=" + State
                           + LinkHTML
                           + targetURL
                           + "\">" + aRow.AvailableOperations[1] + "</a>";
                        break;
                }
            }
        }

        if (!scanningForHistory)
        {
            StringBuilder aRowWork = new StringBuilder();
            ListRows[RowCnt] = aRowWork;
            ListRows[RowCnt].Append("<span id=\"inspectionLineTitle\">");
            ListRows[RowCnt].Append(theRow.TypeText);
            ListRows[RowCnt].Append(": </span>");
            ListRows[RowCnt].Append("<br>");
        }

        //Line 1 - schedule, resulted, or updated date followed by status and sequence number
        ListRows[RowCnt].Append("<div id=\"inspectionLineHistory\">");

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
                if (!isRescheduledOrCanceled)
                {
                    line1.Append(lastUpdatedDate);
                }
            }
             
        }
        line1.Append(" ");
        if (tempStatusString == "Fail")
            line1.Append(tempStatusString + "ed");
        else
            line1.Append(tempStatusString);

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
                line2.Append("Inspector: " + tempInspector);
                line2.Append("<br>");
            }
        }

        //Line 3 - show schedule, request, cancel, or reschedule links
        StringBuilder line3 = new StringBuilder();
        if (actionAllowedLink != string.Empty || CancelLink != string.Empty || ViewDetailsLink != string.Empty)
        {
            if (isiPhone)
            {
                line3.Append("<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-top:0px; margin-bottom:0px;\">");
                line3.Append("<td width=\"80%\">");
                line3.Append(ViewDetailsLink + "</td>");
                line3.Append("<td>" + actionAllowedLink + "</td>");
                line3.Append("<td>" + CancelLink + "</td>");
                line3.Append("</table>");
            }
            else
            {
                line3.Append(ViewDetailsLink + " " + actionAllowedLink + " " + CancelLink);
                line3.Append("");
            }
        }

        //Line 4 - show contact name
        StringBuilder line4 = new StringBuilder();

        if (isContactVisible == true && ((theRow.ContactFirstName != null && theRow.ContactFirstName.Length > 0)
            || (theRow.ContactLastName != null && theRow.ContactLastName.Length > 0)))
        {
            line4.Append("Contact: ");
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
                aSpace = " ";
            }
            line4.Append("<br>");
        }

        StringBuilder line5 = new StringBuilder();
        if (aRow.IsUpcomingInspection)
        {
            //Line 5  for upcoming shows request comment if it is available
            /* Schedule Comment will not show in 7.1
            if (requestComments.ToString() != "")
            {
                line5.Append("<span id=\"inspectionLineComment\">");
                line5.Append(requestComments);
                line5.Append(": </span>");
                line5.Append("<br>");
            }
            */
        }
        else
        {
            //Line 5  for completed shows inspector and result comment if it is available
             if (useInspector && tempInspector.Length > 0)
            {
                line5.Append(actionByLabel + tempInspector);
                line5.Append("<br>");
            }

            if (inspectionComments.ToString() != "")
            {
                /*
                if (useInspector && tempInspector.Length > 0)
                {
                }
                 */
                line5.Append("<span id=\"inspectionLineComment\">");
                line5.Append(inspectionComments);
                line5.Append(": </span>");
                line5.Append("<br>");
            }
        }

        //Line 6 displays last updated date and by
        StringBuilder line6 = new StringBuilder();
        if (useLastUpdateBy || useLastUpdateDate)
        {
            line6.Append("Last Update: ");
            line6.Append(lastUpdatedDate);
            line6.Append("<br>" + actionByLabel);
            line6.Append(tempUpdatedBy);
            line6.Append("<br>");
        }

        ListRows[RowCnt].Append(line1);
        ListRows[RowCnt].Append(line2);
        ListRows[RowCnt].Append(line4);
        ListRows[RowCnt].Append(line5);
        ListRows[RowCnt].Append(line6);

        ListRows[RowCnt].Append(line3); //Moved scheule links to last item for each inspection

        ListRows[RowCnt].Append("</div>");
    }
    /// <summary>
    /// Get the inspection action view parameters. 
    /// </summary>
    /// <param name="actionURL"> the string that contains a target URL that with parameter to extract.</param>
    /// <returns>the URL parameters or empty string</returns>
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
    /// list item variable keys (copied from ACA for version 7.1 release).
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
    /// Get status label key (copied from ACA for 7.1 release)
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
