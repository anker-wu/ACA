/**
* <pre>
* 
*  Accela Citizen Access
*  File: ViewDetails.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2011
* 
*  Description:
*  VO for Additional Information object. 
* 
*  Notes:
*      $Id: ViewDetails.aspx.cs 77905 2007-10-15 12:49:28Z ACHIEVO\Jackie.Yu $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  08/05/2008           Dave Brewster           Updated the breadcrumbs, back link, fonts and formatting to comply with
*                                               the 2008 redesign document.
*  10-09-2008           Dave Brewster           2008 Mobile ACA 6.7.0 interface redesign
*  10-10-2009           Dave Brewster           Added new breadcrumb logic.
* </pre>
*/
//using Accela.ACA.Web.Common;
//using Accela.ACA.WSProxy;
//using Accela.ACA.WSProxy.WSModel;
using System;
using System.Data;
//using System.Configuration;
//using System.Collections;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using System.Text;

/// <summary>
/// used to display long comments & Description based various scenarios
/// </summary>
public partial class ViewDetails : AccelaPage
{
    public string Details = string.Empty;
    public StringBuilder BackLink = new StringBuilder();
    public string DetailsHeader = string.Empty;
    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();

    private string SearchType = string.Empty;
    private string SearchBy = string.Empty;
    private string PermitNo = string.Empty;
    private string PermitType = string.Empty;
    private string Mode = string.Empty;
    private string ViewPermitPageNo = string.Empty;
    private string InspectionsPageNo = string.Empty;
    private string Agency = string.Empty;

    
    /// <summary>
    /// Used to to display lengthy comments & description
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Page validation
        ValidationChecks("View.Details.aspx");
        string CollectionModuleName = string.Empty;

        SearchType = GetFieldValue("SearchType", false);
        SearchBy = GetFieldValue("SearchBy", false);
        Mode = GetFieldValue("Mode", false);
        PermitNo = GetFieldValue("PermitNo", false);
        PermitType = GetFieldValue("PermitType", false);
        ViewPermitPageNo = GetFieldValue("ViewPermitPageNo", false);
        InspectionsPageNo = GetFieldValue("InspectionsPageNo", false);
        Agency = GetFieldValue("Agency", false);

        if (Request.QueryString["Type"] != null)
        {
            Permit ThisPermit = new Permit();
            Inspection ThisInspection = new Inspection();

            switch (Request.QueryString["Type"])
            {
                case "Permit":
                    #region Permit
                    if (Request.QueryString["Id"] != null)
                    {
                        ThisPermit.Number = Request.QueryString["Id"].ToString();
                        ThisPermit.Agency = Agency;
                        // gets permit details
                        ThisPermit = MyProxy.PermitRetrieve(ThisPermit);
                        
                        if (MyProxy.OnErrorReturn)
                        {  // Proxy Exception 
                            ErrorMessage.Append(MyProxy.ExceptionMessage);
                        }
                        Details = ThisPermit.Desc;
                        iPhonePageTitle = "Permit-Description";
                   }
                    break;

                    #endregion
                case "Inspection":
                    #region inspections
                    if (GetFieldValue("DTRow",false) != null)
                    {   // The inspection ID is no longer returned to the inspectlist.
                        // Search.Results.List stores the long comment in session varialbles.
                        // This code retrieves the long comment from the sessions state
                        string commentKey = GetFieldValue("DTKey", false);
                        if (commentKey != string.Empty && Session[commentKey] != null)
                        {
                            Details = (string)Session[commentKey];
                        }
                        iPhonePageTitle = "Inspection-Comments";
                    }
                    break;

                #endregion
                case "InspectionResult":
                    #region  InspectionResults
                    if (Request.QueryString["Id"] != null)
                    {
                        ThisInspection.Id = Request.QueryString["Id"];
                        // gets inspection details
                        ThisInspection = MyProxy.InspectionRetrieve(ThisInspection);
                        
                        if (MyProxy.OnErrorReturn)
                        {  // Proxy Exception 
                            ErrorMessage.Append(MyProxy.ExceptionMessage);
                        }
                        Details = ThisInspection.Comments;
                        iPhonePageTitle = "Inspection-Comments";
                    }
                    break;

                    #endregion
                case "InspectionCancel":
                    #region Inspection Cancel
                    if (Request.QueryString["Id"] != null)
                    {
                        ThisInspection.Id = Request.QueryString["Id"];
                        // gets inspection details
                        ThisInspection = MyProxy.InspectionRetrieve(ThisInspection);
                        
                        if (MyProxy.OnErrorReturn)
                        {  // Proxy Exception 
                            ErrorMessage.Append(MyProxy.ExceptionMessage);
                        }
                        Details = ThisInspection.Comments;
                        iPhonePageTitle = "Inspection-Comments";
                    }
                    break;
                case "Workflow":
                    Details = GetFieldValue("commentText", false);
                    iPhonePageTitle = GetFieldValue("pageTitle",false) + " Comment";
                    break;
                    #endregion
            }   
        }
        if (isiPhone == false)
        {
            DetailsHeader = "<div id=\"pageTitle\">" + iPhonePageTitle + "</div>";
        }
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
        Breadcrumbs = BreadCrumbHelper("Vuew.Details.aspx", sbWork, "View.Details", breadCrumbIndex, isElipseLink, false, false, true);
        BackLink.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));
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
