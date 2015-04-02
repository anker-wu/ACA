/**
* <pre>
* 
*  Accela Citizen Access
*  File: PermitsView.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2012
* 
*  Description:
*  VO for Additional Information object. 
* 
*  Notes:
*      $Id: PermitsView.aspx.cs 77905 2007-10-15 12:49:28Z ACHIEVO\Jackie.Yu $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  10-09-2008           DWB                     2008 Mobile ACA 6.7.0 interface redesign
*  04/01/2009           Dave Brewster           Modiifed to display AltID instetad of Cap ID, CAP type alias.
*  10-10-2009           Dave Brewster           Added new breadcrumb logic.
* </pre>
*/
using Accela.ACA.BLL.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
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
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
/// <summary>
/// Permit's View
/// </summary>
public partial class PermitsView : AccelaPage
{
    public Permit ThisPermit = new Permit();
    public StringBuilder OutputLinks = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder PageTitle = new StringBuilder();
    public StringBuilder LicensedPro = new StringBuilder();
    public StringBuilder Collections = new StringBuilder();
    public string BackForwardLinks = string.Empty;
    public StringBuilder ProcessingStatusView = new StringBuilder();

    public string Description = string.Empty;
    public string SearchMode = string.Empty;
    public string CurrentResultPageNumber = "0";
    public string DisplayType = string.Empty;
    public string AddToCollection = string.Empty;
    string sLinkHTML = string.Empty;
    private string AltID = string.Empty;
    private string DisplayNumber = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    { 
        // page validate
        ValidationChecks("Permits.View.aspx");
        if (Session["AMCA_PermitView_Breadcrumbs"] != null)
        {
            Session.Remove("AMCA_PermitView_Breadcrumbs");
        }

        string mycollection_managepage_label_name = StripHTMLFromLabelText(LocalGetTextByKey("mycollection_collectionmanagement_collectionname"), "Collections");

        SearchMode = GetFieldValue("Mode", false).ToString();
        string CollectionModule = GetFieldValue("CollectionModule", false);
        if (Request.QueryString["ResultPage"] != null)
        {
            CurrentResultPageNumber = Request.QueryString["ResultPage"];
        }
        // gets and retrieves Permits for view
        ThisPermit.Number = GetFieldValue("PermitNumber", false);// Request.QueryString["PermitNumber"].ToString();
        ThisPermit.Type = GetFieldValue("PermitType", false); // Request.Form["PermitType"];
        ThisPermit.Alias = GetFieldValue("AltID", false).ToString();
        ThisPermit.Agency = GetFieldValue("Agency", false);
        if (ThisPermit.Alias.ToString() == string.Empty)
        {
            DisplayNumber = ThisPermit.Number;
        }
        else
        {
            DisplayNumber = ThisPermit.Alias;
        }
        ThisPermit = MyProxy.PermitRetrieve(ThisPermit);
        if (MyProxy.OnErrorReturn == true)
        {
            ErrorMessage.Append(ErrorFormat);
            ErrorMessage.Append(MyProxy.ExceptionMessage);
            ErrorMessage.Append(ErrorFormatEnd);
        }
        if (ThisPermit != null)
        {
            if (ThisPermit.Alias.ToString() == string.Empty)
            {
                DisplayNumber = ThisPermit.Number;
            }
            else
            {
                DisplayNumber = ThisPermit.Alias;
            }

            if (ThisPermit.TypeAlias.ToString() == string.Empty)
            {
                DisplayType = ThisPermit.Type.ToString();
            }
            else
            {
                DisplayType = ThisPermit.TypeAlias.ToString();
            }
            SimpleCapModel[] caps = new SimpleCapModel[1];
            SimpleCapModel capModel = new SimpleCapModel();
            capModel.capID = new CapIDModel();
            capModel.capID.ID1 = ThisPermit.capId1;
            capModel.capID.ID2 = ThisPermit.capId2;
            capModel.capID.ID3 = ThisPermit.capId3;
            capModel.altID = ThisPermit.Alias;
            capModel.capID.serviceProviderCode = ThisPermit.Agency;
            capModel.moduleName = ThisPermit.Module;
            capModel.capClass = ThisPermit.capClass;
            caps[0] = capModel;
            Session["MyCollection_SelectedCaps"] = caps;
        }

        DisplayType = "<div id=\"pageText\">" + DisplayType + "</div>";

        if (ThisPermit == null)
            this.ThisPermit = new Permit();

        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        StringBuilder sbWork = new StringBuilder();
        sbWork.Append("&Module=" + ModuleName.ToString());
        sbWork.Append("&Mode=" + SearchMode.ToString());
        sbWork.Append("&PermitNumber=" + GetFieldValue("PermitNumber", false));
        sbWork.Append("&PermitType=" + GetFieldValue("PermitType", false));
        sbWork.Append("&AltID=" + GetFieldValue("AltID", false));
        sbWork.Append("&ResultPage=" + GetFieldValue("ResultPage", false));
        sbWork.Append("&Agency=" + GetFieldValue("Agency", false));
        if (SearchMode == "MyCollections")
        {
            sbWork.Append("&CollectionModule=" + CollectionModule);
        }
        else if (isGlobalSearchMode == true)
        {
            SearchMode = "View Permits";
        }
        iPhonePageTitle = DisplayNumber;
        Breadcrumbs = BreadCrumbHelper("Permits.View.aspx", sbWork, GetFieldValue("AltID", false).ToString(), breadCrumbIndex, isElipseLink, false, false, true);
        BackForwardLinks = BackLinkHelper(CurrentBreadCrumbIndex.ToString());

        if (isiPhone == false || iPhoneTitleHasBeenClipped == true)
        {
            PageTitle.Append("<div id=\"pageTitle\">");
            if (isiPhone == false)
            {
                PageTitle.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
                PageTitle.Append("<tr><td width=\"60%\">");
            }
            PageTitle.Append(DisplayNumber);
            if (isiPhone == false)
            {
                PageTitle.Append("</td><td>");
            }

            if (MyProxy.OnErrorReturn != true)
            {
                if (isiPhone == true)
                {
                    Collections.Append("<center>");
                }
                Collections.Append("<a class=\"collectionActionLink\" href=\"MyCollections.Update.aspx?State=" + State);
                Collections.Append("&Mode=" + SearchMode);
                Collections.Append("&Module=" + ModuleName);
                Collections.Append("&CollectionOperation=Add");
                Collections.Append("&ResultPage=" + CurrentResultPageNumber);
                Collections.Append("&PermitNumber=" + ThisPermit.Number);
                Collections.Append("&PermitType=" + ThisPermit.Type);
                Collections.Append("&AltID=" + ThisPermit.Alias);
                Collections.Append("&DisplayNumber=" + DisplayNumber);
                Collections.Append("&Agency=" + ThisPermit.Agency);
                Collections.Append("&PermitsDetailPage=true");
                if (SearchMode == "MyCollections")
                {
                    Collections.Append("&PageBreadcrumbIndex=" + breadCrumbIndex);
                    Collections.Append("&CollectionModule=" + CollectionModule);
                }
                Collections.Append("\">");
                Collections.Append("Add To " + mycollection_managepage_label_name + "</a>");
                if (isiPhone == true)
                {
                    Collections.Append("</center>");
                }
                else
                {
                    PageTitle.Append(Collections.ToString());
                    Collections = new StringBuilder();
                }
            }


            if (isiPhone == false)
            {
                PageTitle.Append("</td></tr></table>");
            }
            PageTitle.Append("</div>");
        }

        //Owner Info will go here:

        Dictionary<string, UserRolePrivilegeModel> sectionPermissions = AccelaProxy.GetSectionPermissions(ModuleName);
        bool isLPVisible = AccelaProxy.GetSectionVisibility(CapDetailSectionType.LICENSED_PROFESSIONAL.ToString(), sectionPermissions, ModuleName);
        bool isOwnerVisible = AccelaProxy.GetSectionVisibility(CapDetailSectionType.OWNER.ToString(), sectionPermissions, ModuleName);
        bool isWorkflowVisible = AccelaProxy.GetSectionVisibility(CapDetailSectionType.PROCESSING_STATUS.ToString(), sectionPermissions, ModuleName);
        bool isInspectionVisible = AccelaProxy.GetSectionVisibility(CapDetailSectionType.INSPECTIONS.ToString(), sectionPermissions, ModuleName);
        bool isRelatedRecordsVisible = AccelaProxy.GetSectionVisibility(CapDetailSectionType.RELATED_RECORDS.ToString(), sectionPermissions, ModuleName);
        
        if (MyProxy.OnErrorReturn != true)
        {
            if (ThisPermit.LicensedPro.Trim() != "" && isLPVisible == true )
            {
                LicensedPro.Append("<div id=\"pageSectionTitle\">");
                LicensedPro.Append("License Professional: <br>");
                LicensedPro.Append("</div>");
                LicensedPro.Append("<div id=\"pageText\">");

                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                string newLine = string.Empty;
                for (int aRow = 0; aRow < capModel.licenseProfessionalList.Length; aRow++)
                {
                    LicensedPro.Append(newLine);
                    newLine = "<br>";

                    LicenseProfessionalModel4WS aLicense = capModel.licenseProfessionalList[aRow];
                    ThisPermit.LicensedPro = aLicense.licenseNbr;
                    ThisPermit.LicensedProType = aLicense.licenseType;

                    if (aLicense.contactFirstName != null || aLicense.contactLastName != null || aLicense.contactMiddleName != null)
                    {
                        LicensedPro.Append(aLicense.contactFirstName != null ? aLicense.contactFirstName + " " : string.Empty);
                        LicensedPro.Append(aLicense.contactMiddleName != null ? aLicense.contactMiddleName + " " : string.Empty);
                        LicensedPro.Append(aLicense.contactLastName != null ? aLicense.contactLastName + " " : string.Empty);
                        LicensedPro.Append("<br>");
                    }
                    else if (aLicense.contLicBusName != null)
                    {
                        LicensedPro.Append(aLicense.contLicBusName + "<br>");
                    }
                    LicensedPro.Append("<a id=\"pageLineLink\" href=\"License.info.aspx?State=" + State
                        + "&Type=Permit"
                        + "&PermitNumber=" + ThisPermit.Number
                        + "&AltID=" + ThisPermit.Alias
                        + "&PermitType=" + ThisPermit.Type
                        + "&LicenseNumber=" + ThisPermit.LicensedPro
                        + "&LicenseType=" + ThisPermit.LicensedProType
                        + "&Agency=" + ThisPermit.Agency
                        + "&Breadcrumbs=" + SearchMode
                        + "&ViewPermitPageNo=" + CurrentResultPageNumber
                        + "&Module=" + ThisPermit.Module
                        + "\">" + ThisPermit.LicensedPro + "</a>");
                    LicensedPro.Append("(" + ThisPermit.LicensedProType + ")");
                }
                LicensedPro.Append("</div>");
            }

            Description = ThisPermit.Desc;

            sLinkHTML = "<a class=\"pageListLinkBold\" href=\"";
            string iPhoneLink = (isiPhone == true) ? "<img style=\"float:right\" src=\"img/chevron.png\" /> " : string.Empty;

            if (isiPhone != true)
            {
                OutputLinks.Append("<hr id=\"pageTextHR\" />");
            }
            StringBuilder listCell = new StringBuilder();
            int rows = 2
                + (isInspectionVisible ? 1 : 0)
                + (isRelatedRecordsVisible ? 1 : 0)
                + (isWorkflowVisible ? 1 : 0);

            int rowNum = 0;
            
            // Inspections Link
            if (isInspectionVisible)
            {
                listCell.Append(sLinkHTML);
                listCell.Append("Search.Results.aspx?State=" + State
                    + "&PermitType=" + ThisPermit.Type
                    + "&SearchBy=Permit"
                    + "&SearchType=Inspections"
                    + "&SearchValue="
                    + "&Mode=" + SearchMode
                    + "&Id=" + ThisPermit.Number
                    + "&AltID=" + ThisPermit.Alias
                    + "&Agency=" + ThisPermit.Agency
                    + "&Module=" + ThisPermit.Module
                    + "&ViewPermitPageNo=" + CurrentResultPageNumber
                    + "\">Inspections");
                listCell.Append(iPhoneLink);
                listCell.Append("</a>");
                OutputLinks.Append(MyProxy.CreateSelectListCell(listCell.ToString(), rowNum, rowNum, rows, 0, 9999, isiPhone, true));
                rowNum++;
            }

            listCell = new StringBuilder();
            listCell.Append(sLinkHTML);
                listCell.Append("Conditions.List.aspx?State=" + State
                    + "&Module=" + ThisPermit.Module
                    + "&ResetList=true"
                    + "\">Conditions");
                listCell.Append(iPhoneLink);
                listCell.Append("</a>");
                OutputLinks.Append(MyProxy.CreateSelectListCell(listCell.ToString(), rowNum, rowNum, rows, 0, 9999, isiPhone, true));
                rowNum++;

            // More Permit Details link.
            listCell = new StringBuilder();
            listCell.Append(sLinkHTML);
            listCell.Append("Permits.MoreDetail.aspx?State=" + State
                + "&PermitType=" + ThisPermit.Type
                + "&SearchBy=Permit"
                + "&SearchType=Inspections"
                + "&SearchValue="
                + "&Mode=" + SearchMode
                + "&Module=" + ModuleName
                + "&Agency=" + ThisPermit.Agency
                + "&Id=" + ThisPermit.Number
                + "&AltID=" + ThisPermit.Alias
                + "&ViewPermitPageNo=" + CurrentResultPageNumber
                + "\">More Record Details");
            listCell.Append(iPhoneLink);
            listCell.Append("</a>");
            OutputLinks.Append(MyProxy.CreateSelectListCell(listCell.ToString(), rowNum, rowNum, rows, 0, 9999, isiPhone, true));
            rowNum++;

            // Related permits list link.
            if (isRelatedRecordsVisible)
            {
                listCell = new StringBuilder();
                listCell.Append(sLinkHTML);
                listCell.Append("AdvancedSearch.Results.aspx?State=" + State
                     + "&Mode=Related Permits"
                     + "&Module=" + ModuleName
                     + "&PermitNumber=" + ThisPermit.Number
                     + "&AltID=" + ThisPermit.Alias
                     + "&PermitType=" + ThisPermit.Type
                     + "&ViewPermitPageNo=" + CurrentResultPageNumber
                     + "&ResultPage=0"
                     + "&SearchBy=" + SearchMode
                     + "&Agency=" + ThisPermit.Agency
                     + "\">Related Records");
                listCell.Append(iPhoneLink);
                listCell.Append("</a>");
                OutputLinks.Append(MyProxy.CreateSelectListCell(listCell.ToString(), rowNum, rowNum, rows, 0, 9999, isiPhone, true));
                rowNum++;
            }

            // Related permits list link.
            if (isWorkflowVisible)
            {
                string aLabel = StripHTMLFromLabelText(LocalGetTextByKey("per_workStatus_label_proceeStatus"),"Process Status");
                listCell = new StringBuilder();
                listCell.Append(sLinkHTML);
                listCell.Append("WorkFlow.View.aspx?State=" + State
                    + "&PermitType=" + ThisPermit.Type
                    + "&SearchBy=Permit"
                    + "&SearchType=Inspections"
                    + "&SearchValue="
                    + "&Mode=" + SearchMode
                    + "&Module=" + ModuleName
                    + "&Id=" + ThisPermit.Number
                    + "&AltID=" + ThisPermit.Alias
                    + "&Agency=" + ThisPermit.Agency
                    + "&ViewPermitPageNo=" + CurrentResultPageNumber
                    + "\">");
                listCell.Append(aLabel);
                listCell.Append(iPhoneLink);
                listCell.Append("</a>");
                OutputLinks.Append(MyProxy.CreateSelectListCell(listCell.ToString(), rowNum, rowNum, rows, 0, 9999, isiPhone, true));
            }
        }
    }

    /// <summary>
    /// Get Field value from Form, Request, or QueryString.
    /// </summary>+
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

    #region code copied from ACA and new procedures added for 7.1 relase
    /// <summary>
    /// get the user's initial or the user's name. if the biz domain is true, return the user's initial,
    /// otherwise return the user's initial or the user's name by the option the user selected.
    /// </summary>
    /// <param name="user">SysUserModel4WS object</param>
    /// <returns>string user name</returns>
    public static string GetUserName(SysUserModel4WS user)
    {
        if (user == null)
        {
            return String.Empty;
        }

        // if enable to display user initial name, which means all users will display the initial name.
        if (StandardChoiceUtil.IsDisplayUserInitial())
        {
            return I18nStringUtil.GetString(user.resInitial, user.initial, user.resFullName, user.fullName);
        }
        else
        {
            if (user.isDisplayInitial)
            {
                return I18nStringUtil.GetString(user.resInitial, user.initial, user.resFullName, user.fullName);
            }
            else
            {
                return I18nStringUtil.GetString(user.resFullName, user.fullName);
            }
        }
    }

    #endregion

}
