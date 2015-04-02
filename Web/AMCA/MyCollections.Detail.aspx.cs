/**
* <pre>
* 
*  Accela Citizen Access
*  File: MyCollections.Detail.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2011
* 
*  Description:
*  View Collection Summary Detail. 
* 
*  Notes:
*      $Id: MyCollections.Detail.aspx.cs 77905 2009-07-17 12:49:28Z  dave.brewster $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  07-17-2009           Dave Brewster           New page added for version 7.0
*  10-10-2009           Dave Brewster           Added new breadcrumb logic.
* 
* </pre>
*/
using System;
using System.Data;
//using System.Configuration;
using System.Collections;
//using System.Web;
using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using System.Text;
using Accela.ACA.Common;
//using Accela.ACA.Web;
using Accela.ACA.Web.Common;
using Accela.ACA.BLL.MyCollection;
using Accela.ACA.WSProxy;
//using Accela.ACA.WSProxy.WSModel;

/// <summary>
/// 
/// </summary>
public partial class MyCollectionsDetail : AccelaPage
{
    /// <summary>
    /// HTML empty
    /// </summary>
    private const string HTML_EMPTY = "&nbsp;";

    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder TotalRecords = new StringBuilder();
    public StringBuilder Inspections = new StringBuilder();
    public StringBuilder FeeSummary = new StringBuilder();
    public StringBuilder BackForwardLinks = new StringBuilder();
    public StringBuilder HiddenFields = new StringBuilder();
    public StringBuilder PageMessage = new StringBuilder();
    public StringBuilder PageTitle = new StringBuilder();

    public string CollectionName = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("MyCollections.Detail.aspx");
        State = GetFieldValue("State", false);

        // Breadcrumbs built here
        //string linkHTML = "<a style=\"color:#040478;  font-size:10pt; text-decoration:underline;\" href=\"";
        string collectionId = Request.QueryString["CollectionId"].ToString();
        IMyCollectionBll myCollectionBll = (IMyCollectionBll)ObjectFactory.GetObject(typeof(IMyCollectionBll));
        MyCollectionModel myCollectionModel = myCollectionBll.GetCollectionDetailInfo(ConfigManager.AgencyCode, AppSession.User.PublicUserId, collectionId, null);
        MyCollectionModel[] myCollections = new MyCollectionModel[1];
        myCollections[0] = myCollectionModel;

        AppSession.SetMyCollectionsToSession(myCollections);

        MyCollectionProxy myCollectionProxy = new MyCollectionProxy();
        Hashtable htSimpleCapModel = myCollectionProxy.GetSimpleCapModelListByModuleName(myCollectionModel.simpleCapModels);
        //Initialization my collection summary information.
        myCollectionProxy.BuildSummaryInformation(myCollectionModel.collectionName, myCollectionModel.collectionDescription, htSimpleCapModel, myCollectionModel.myCollectionSummaryModel);
        CollectionName = myCollectionModel.collectionName;

        iPhonePageTitle = myCollectionProxy.lblMyCollectionNameText.ToString();
        if (isiPhone == false || iPhoneTitleHasBeenClipped == true)
        {
            PageTitle.Append("<div id=\"pageTitle\">" + iPhonePageTitle + "</div>");
        }       
        PageMessage.Append("<div  id=\"pageTextIndented\">" );
        PageMessage.Append(myCollectionProxy.lblMyCollectionDescText == null ? string.Empty : myCollectionProxy.lblMyCollectionDescText.ToString());
        PageMessage.Append("</div>");

        // TotalRecords.Append("<div>");
        TotalRecords.Append("<div id=\"pageSectionTitle\">");
        TotalRecords.Append("Total Records: " + myCollectionProxy.lblTotalRocordsNumText.ToString());
        TotalRecords.Append("</div>");

        //linkHTML = "<a style=\"color:#040478; font-weight:normal; font-size:10pt; text-decoration:underline;\" href=\"";
        StringBuilder moduleList = new StringBuilder();
        if (htSimpleCapModel != null)
        {
            moduleList.Append("<table cellpadding=\"5px\" cellspacing=\"0\" width=\"100%\">");
            if (isiPhone == true)
            {
                moduleList.Append("<tr><td>");
            }
            int rowCnt = 0;
            foreach (DictionaryEntry simpleCapModel in htSimpleCapModel)
            {
                string moduleName = LabelUtil.GetI18NModuleName(simpleCapModel.Key.ToString());
                ArrayList simpleCapModelList = (ArrayList)simpleCapModel.Value;
                rowCnt++;
                StringBuilder aLink = new StringBuilder();
                aLink.Append("<a class=\"pageListLinkBold\" href=\"");
                aLink.Append("MyCollections.List.aspx?State=" + State + "&Module=" + (ModuleName == null || moduleName == string.Empty ? moduleName : ModuleName));
                aLink.Append("&Mode=MyCollections");
                aLink.Append("&CollectionId=" + collectionId);
                aLink.Append("&CollectionModule=" + simpleCapModel.Key.ToString());
                aLink.Append("&CollectionModuleName=" + moduleName + "\">");
                aLink.Append(moduleName +  " (" + simpleCapModelList.Count.ToString() + ")");
                if (isiPhone == true)
                {
                    aLink.Append("<img style=\"float:right\" src=\"img/chevron.png\" /> ");
                }
                aLink.Append("</a>");
                moduleList.Append(MyProxy.CreateSelectListCell(aLink.ToString(), rowCnt - 1, rowCnt - 1, htSimpleCapModel.Count, 0, 9999, isiPhone, true));
            }
            if (isiPhone == true)
            {
                moduleList.Append("</td></tr>");
            }
            moduleList.Append("</table>");
        }
        TotalRecords.Append(moduleList.ToString());
        // TotalRecords.Append("</div>");

        Inspections.Append("<div id=\"pageSectionTitle\">");
        Inspections.Append("Inspections Summary: "+ myCollectionProxy.lblInspectionSummaryNumText.ToString());
        Inspections.Append("</div>");
        Inspections.Append("<div id=\"pageTextIndented\">");
        Inspections.Append(myCollectionProxy.lblInspectionSummaryDetailText.ToString());
        Inspections.Append("</div>");

        FeeSummary.Append("<div id=\"pageSectionTitle\">");
        FeeSummary.Append("Fee Summary: ");
        FeeSummary.Append("</div>");
        FeeSummary.Append("<div id=\"pageTextIndented\">");
        FeeSummary.Append(myCollectionProxy.lblFeeSummaryDetailText.ToString());
        FeeSummary.Append("</div>");
        //Reset Session Variables 
        //Clear MyCollections.list.aspx.cs list filter.
        if (Session["AMCA_MyCollection_DataTable"] != null)
        {
            Session.Remove("AMCA_MyCollection_DataTable");
        }
        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        string truncateBreadcrumb = GetFieldValue("TruncateBreadcrumb", false);
        if (truncateBreadcrumb != string.Empty)
        {
            breadCrumbIndex = truncateBreadcrumb;
        }
        StringBuilder sbWork = new StringBuilder();
        sbWork.Append("&CollectionId=" + myCollections[0].collectionId);
        sbWork.Append("&CollectionName=" + myCollections[0].collectionName);
        Breadcrumbs = BreadCrumbHelper("MyCollections.Detail.aspx", sbWork, CollectionName, breadCrumbIndex, isElipseLink, false, false, false);
        BackForwardLinks.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));

    
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
