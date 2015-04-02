/**
* <pre>
* 
*  Accela Citizen Access
*  File: WorkflowView.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2011
* 
*  Description:
*  view the record's workflow. 
* 
*  Notes:
*      $Id: Workflow.View.aspx.cs 207795 2011-11-18 04:32:10Z ACHIEVO\alan.hu $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  01/24/2011           Dave Brewster           Added new form for 7.1

* </pre>
*/
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
//using Accela.ACA.WSProxy.WSModel;
using System;
using System.Data;
using System.Configuration;
using System.Collections;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using System.Text;

using Accela.ACA.Web.Component;
/// <summary>
/// Permit's View
/// </summary>
public partial class WorkflowView : AccelaPage
{
    public Permit ThisPermit = new Permit();
    public StringBuilder OutputLinks = new StringBuilder();
    public string Description = string.Empty;
    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder Workflow = new StringBuilder();
    public string PageTitle = string.Empty;

    public string ThisPermitDate = string.Empty;
    public string DisplayNumber = string.Empty;
    public string DisplayType = string.Empty;
    private string PermitNo = string.Empty;
    private string PermitType = string.Empty;
    private string SearchBy = string.Empty;
    private string SearchType = string.Empty;
    private string SearchValue = string.Empty;
    private string ViewPermitPageNo = string.Empty;  // ResultPage for "View Permits" breadcrumb link.
    private string InspectionsPageNo = string.Empty; // ResultPage for "View Permits > Inspections" Breadcrumb link
    private string Mode = string.Empty;
    private string AltID = string.Empty;
    private string Agency = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // page validate
        ValidationChecks("Workflow.View.aspx");

        PermitNo = GetFieldValue("Id", false);
        AltID = GetFieldValue("AltID", false);
        PermitType = GetFieldValue("PermitType", false);
        SearchBy = GetFieldValue("SearchBy", false);
        SearchType = GetFieldValue("SearchType", false);
        SearchValue = GetFieldValue("SearchValue", false);
        ViewPermitPageNo = GetFieldValue("ViewPermitPageNo", false);
        Mode = GetFieldValue("Mode", false);
        Agency = GetFieldValue("Agency", false);

        // gets and retrieves Permits for view
        ThisPermit.Number = PermitNo;
        ThisPermit.Type = PermitType;
        ThisPermit.Agency = Agency;
        ThisPermit = MyProxy.PermitRetrieve(ThisPermit);
        ThisPermit.Application = "";
        if (ThisPermit == null)
            this.ThisPermit = new Permit();
        CapModel4WS myCap = AppSession.GetCapModelFromSession(ModuleName);
        if (myCap.capContactModel != null)
        {
            if (myCap.capContactModel.people != null && myCap.capContactModel.people.fullName != null && myCap.capContactModel.people.contactType.Equals("APPLICANT", StringComparison.OrdinalIgnoreCase) == true)
            {
                ThisPermit.Application = GetContactFullName(myCap.capContactModel.people);
            }
        }
        if (MyProxy.OnErrorReturn)
        {  // Proxy Exception 
            ErrorMessage.Append(MyProxy.ExceptionMessage);
        }
        /*
        if (ThisPermit.Alias.ToString() == string.Empty)
        {
            DisplayNumber = ThisPermit.Number;
        }
        else
        {
            DisplayNumber = ThisPermit.Alias;
        }
        */
        iPhonePageTitle = (StripHTMLFromLabelText(LocalGetTextByKey("per_workStatus_label_proceeStatus"), "Processing Status"));

        string CollectionModuleName = string.Empty;
        string SearchMode = Mode;
        string ResultPage = ViewPermitPageNo;
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
        sbWork.Append("&ViewPermitPageNo" + GetFieldValue("ViewPermitPageNo", false));
        if (SearchMode == "MyCollections")
        {
            sbWork.Append("&CollectionId=" + GetFieldValue("CollectionId", false));
            sbWork.Append("&CollectionModuleName=" + GetFieldValue("CollectionModuleName", false));
        }
        if (ThisPermit.Desc.Length > 100)
        {
            Description = ThisPermit.Desc.Substring(0, 95) + HTML.PresentLink("View.Details.aspx?State=" + State + sbWork.ToString() + "&Type=Permit", "...");
        }
        else
        {
            Description = ThisPermit.Desc;
        }
        if (isiPhone == false)
        {
            DisplayNumber = "<div id=\"pageTitle\">" + iPhonePageTitle + "</div><hr />";
        }


        Breadcrumbs = BreadCrumbHelper("Workflow.View.aspx", sbWork, iPhonePageTitle, breadCrumbIndex, isElipseLink, false, false, true);
        // Breadcrumbs.Append("<br>");
        OutputLinks = new StringBuilder();
        OutputLinks.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));

        AMCAWorkStatus ws = new AMCAWorkStatus(ThisPermit.Module, sbWork.ToString(), State, PageTitle);
        Workflow.Append(ws.GetProcessingContent());

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
    /// Construct contact full name which consist of first name, middle name and last name
    /// </summary>
    /// <param name="people">the PeopleModel4WS instance</param>
    /// <returns>the contact full name</returns>
    private string GetContactFullName(PeopleModel4WS people)
    {
        if (people == null)
        {
            return string.Empty;
        }

        string[] fullName = { people.firstName, people.middleName, people.lastName };
        string contactFullName = DataUtil.ConcatStringWithSplitChar(fullName, ACAConstant.BLANK);

        return contactFullName;
    }

}
