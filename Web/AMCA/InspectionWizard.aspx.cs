/**
* <pre>
* 
*  Accela Citizen Access
*  File: InspectionWizard.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2010
* 
*  Description:
*  Allows user to view and select from the available inspection categories list. 
* 
*  Notes:
*      $Id: InspectionWizard.aspx.cs 211504 2012-01-06 01:47:30Z ACHIEVO\daly.zeng $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  01/24/2011            Dave Brewster           New page added for version 7.1
 
* </pre>
*/


using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using System.Text;
//using Accela.ACA.BLL.Common;
//using Accela.ACA.BLL.Inspection;
//using Accela.ACA.Common;
//using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
//using Accela.ACA.WSProxy.WSModel;
//using Accela.ACA.Html.Inspection;
using Accela.ACA.Inspection;



/// <summary>
/// 
/// </summary>
public partial class InspectionWizard : AccelaPage
{
    private const string CATEGORY_OTHERS_KEY = "\fOthers\f";

    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder InspectionList = new StringBuilder();
    public StringBuilder CategoryList = new StringBuilder();

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
        ValidationChecks("InspectionWizard.aspx");
        State = GetFieldValue("State", false);

        iPhonePageTitle = "Inspection Categories";
        if (!isiPhone == true)
        {
            PageTitle = "<div id=\"pageTitle\">" + iPhonePageTitle + ":</div><hr>";
        }

        PermitType = GetFieldValue("PermitType", false); 
        AltID = GetFieldValue("AltID", false); 
        SearchBy = GetFieldValue("SearchBy", false); 
        SearchValue = GetFieldValue("SearchValue", false); 
        Mode = GetFieldValue("Mode", false); 
        Agency = GetFieldValue("Agency", false); 
        bool pagingMode = GetFieldValue("PagingMode", false) == "Y";
        CapIDModel4WS capId = new CapIDModel4WS();
        if (GetFieldValue("Id",false) != string.Empty)
        {
            string[] ids =GetFieldValue("Id",false).ToString().Split('-');

            capId.id1 = ids[0];
            capId.id2 = ids.Length > 1 ? ids[1] : string.Empty;
            capId.id3 = ids.Length > 2 ? ids[2] : string.Empty;
        }
        capId.serviceProviderCode = ConfigManager.AgencyCode;

        InspectionProxy inspectionProxy = new InspectionProxy();

        List<InspectionTypeDataModel> inspectionsList = new List<InspectionTypeDataModel>();
        List<InspectionCategoryDataModel> categoryList = new List<InspectionCategoryDataModel>();
        if (pagingMode)
        {
            if (Session["AMCA_WIZARD_INSPECTIONS"] != null)
            {
                inspectionsList = (List<InspectionTypeDataModel>)Session["AMCA_WIZARD_INSPECTIONS"];
            }
            if (Session["AMCA_WIZARD_CATEGORIES"] != null)
            {
                categoryList = (List<InspectionCategoryDataModel>)Session["AMCA_WIZARD_CATEGORIES"];
            }
        }
        if (!pagingMode || inspectionsList.Count == 0 || categoryList.Count == 0)
        {
            inspectionsList = inspectionProxy.GetInspectionTypeModelsByCapID(capId, AppSession.User.UserSeqNum);
            Session["AMCA_WIZARD_INSPECTIONS"] = inspectionsList;

            categoryList = inspectionProxy.GetInspectionCategories(inspectionsList, capId, AppSession.User.UserSeqNum);
            Session["AMCA_WIZARD_CATEGORIES"] = categoryList;
        }
        
        string theCategory = GetFieldValue("CategoryID", false);
        theCategory = theCategory == "xOTHERSx" ? CATEGORY_OTHERS_KEY : theCategory;

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

        if (inspectionsList.Count != 0)
        {
            StringBuilder showOrHideOptionalLink = new StringBuilder();
            showOrHideOptionalLink.Append("<a href=\"");
            showOrHideOptionalLink.Append("InspectionWizard.aspx?State=" + State);
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

            CategoryList.Append("<div id=\"pageSectionTitle\">Available Catgories: " + showOrHideOptionalLink + "</div>");
        }
        else
        {
            CategoryList.Append("<div id=\"pageSectionTitle\">Available Categories: </div>");
        }
        if (isiPhone != true)
        {
            CategoryList.Append("<div id=\"pageTextIndented\">");
            CategoryList.Append("<table cellpadding=\"5px\" cellspacing=\"0\" width=\"100%\">");
        }
        int rowCnt = 0;

        foreach (InspectionCategoryDataModel aRow in categoryList)
        {
            rowCnt++;

            int recCount = 0;
            foreach (InspectionTypeDataModel aRow2 in inspectionsList)
            {
                bool includeInList = false;
                if (showOptional == true || (showOptional == false && aRow2.Required == true))
                {
                    if (aRow2.AvailableOperations.Length != 0 
                        && aRow2.AvailableOperations[0] != InspectionAction.DoPrerequisiteNotMet
                        && aRow2.AvailableOperations[0] != InspectionAction.None )
                    {
                        if (aRow.ID == CATEGORY_OTHERS_KEY)
                        {
                            if (aRow2.Categories.Length == 0)
                            {
                                includeInList = true;
                            }
                        }
                        else if (aRow.ID != CATEGORY_OTHERS_KEY && aRow.ID != string.Empty)
                        {
                            if (aRow2.Categories.Length > 0)
                            {
                                foreach (InspectionCategoryDataModel aCategory in aRow2.Categories)
                                {
                                    if (aCategory.ID == aRow.ID)
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
                }
                if (includeInList == true)
                {
                    recCount++;
                }
            }
            if (recCount > 0)
            {
                string LinkHTML = "&Id=" + GetFieldValue("Id", false)
                   + "&PermitNo=" + GetFieldValue("PermitNo", false)
                   + "&AltID=" + AltID
                   + "&PermitType=" + PermitType
                   + "&SearchBy=" + SearchBy
                   + "&SearchType=" + GetFieldValue("SearchType", false)
                   + "&SearchValue=" + SearchValue
                   + "&ViewPermitPageNo=" + GetFieldValue("ViewPermitPageNo", false)
                   + "&InspectionsPageNo=" + GetFieldValue("InspectionPageNo", false)
                   + "&Module=" + ModuleName
                   + "&CategoryName=" + aRow.Category
                   + "&CategoryID=" + ((aRow.ID == CATEGORY_OTHERS_KEY) ? "xOTHERSx" : aRow.ID);

                StringBuilder aLink = new StringBuilder();
                if (isiPhone == true)
                {

                    aLink.Append("<a href=\"");
                    aLink.Append("InspectionWizardTypes.aspx?State=" + State);
                    aLink.Append(LinkHTML);
                    aLink.Append("\">");
                    aLink.Append("<table cellpadding=\"5px\" cellspacing=\"0\" width=\"100%\">");
                    aLink.Append("<tr><td width=\"90%\" class=\"pageListLinkBold\">");
                    aLink.Append(aRow.Category);
                    aLink.Append(" (");
                    aLink.Append(recCount.ToString());
                    aLink.Append(")");
                    aLink.Append("</td><td>");
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
                    aLink.Append("InspectionWizardTypes.aspx?State=" + State);
                    aLink.Append(LinkHTML);
                    aLink.Append("\">");
                    aLink.Append(aRow.Category);
                    aLink.Append(" (");
                    aLink.Append(recCount.ToString());
                    aLink.Append(")");
                    aLink.Append("</a>");
                }
                CategoryList.Append(MyProxy.CreateSelectListCell(aLink.ToString(), rowCnt - 1, rowCnt - 1, categoryList.Count, 0, 9999, isiPhone, true));
            }
        }
        
        //Build inspections type list
        if (categoryList.Count < 2)
        {
            Session["AMCA_BREADCRUMB_ADJUSTMENT"] = "-3";
            StringBuilder showOrHideOptionalLink = new StringBuilder();
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

            Response.Redirect(showOrHideOptionalLink.ToString());
            Response.End();
            return;
        }
        else
        {
            Session["AMCA_BREADCRUMB_ADJUSTMENT"] = "-4";
        }
        if (isiPhone != true)
        {
            CategoryList.Append("</table>");
            CategoryList.Append("</div>");
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
        sbWork.Append("&PagingMode=Y");

        Breadcrumbs = BreadCrumbHelper("InspectionWizard.aspx", sbWork, iPhonePageTitle, breadCrumbIndex, isElipseLink, false, false, false);
        iPhoneHideHeaderCollectionsButton = true;
        BackForwardLinks.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));
        iPhoneHideHeaderCollectionsButton = false;

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
