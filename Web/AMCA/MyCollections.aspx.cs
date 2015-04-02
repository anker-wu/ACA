/**
* <pre>
* 
*  Accela Citizen Access
*  File: MyCollections.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2010
* 
*  Description:
*  View My Collection List. 
* 
*  Notes:
*      $Id: MyCollections.aspx.cs 77905 2009-07-18 12:49:28Z  dave.brewster $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  07-17-2009           Dave Brewster           New page added for version 7.0
*  10-10-2009           Dave Brewster           Added new breadcrumb logic.
* 
* </pre>
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using Accela.ACA.Web;
using Accela.ACA.Web.Common;
using Accela.ACA.BLL.MyCollection;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;


/// <summary>
/// 
/// </summary>
public partial class MyCollections : AccelaPage 
{
    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder CollectionsList = new StringBuilder();
    public StringBuilder BackForwardLinks = new StringBuilder();
    public StringBuilder HiddenFields = new StringBuilder();
    public string PageTitle = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("MyCollections.aspx");
        State = GetFieldValue("State", false);

        iPhonePageTitle = LocalGetTextByKey("mycollection_collectionmanagement_collectionname");
        if (!isiPhone == true)
        {
            PageTitle = "<div id=\"pageTitle\">" + iPhonePageTitle + "</div>";
        }

        MyCollectionProxy myCollectionProxy = new MyCollectionProxy();
        MyCollectionModel[] myCollectionList = myCollectionProxy.getMyCollectionsList();

        int rowCnt = 0;
        if (isiPhone == true)
        {

            foreach (MyCollectionModel myCollection in myCollectionList)
            {
                MyCollectionModel collectionDetail = myCollectionProxy.getMyCollectionDetail(myCollection);
                rowCnt++;
                StringBuilder aLink = new StringBuilder();
                aLink.Append("<a href=\"");
                aLink.Append("MyCollections.Detail.aspx?State=" + State);
                aLink.Append("&CollectionId=" + collectionDetail.collectionId + "\">");
                aLink.Append("<table cellpadding=\"5px\" cellspacing=\"0\" width=\"100%\">");
                aLink.Append("<tr><td width=\"90%\" class=\"pageListLinkBold\">");
                aLink.Append(collectionDetail.collectionName);
                aLink.Append("</td><td>");
                {
                    // aLink.Append(" (" +collectionDetail.simpleCapModels.Length.ToString() + ")");
                    aLink.Append(collectionDetail.simpleCapModels.Length.ToString() + " ");
                }
                aLink.Append("</td><td>");
                if (isiPhone == true)
                {
                    aLink.Append("<img style=\"float:right; vertical-align:middle;\" src=\"img/chevron.png\" /> ");
                }
                aLink.Append("</td></tr></table>");
                aLink.Append("</a>");
                CollectionsList.Append(MyProxy.CreateSelectListCell(aLink.ToString(), rowCnt - 1, rowCnt - 1, myCollectionList.Length, 0, 9999, isiPhone, true));
            }
        }
        else
        {
            CollectionsList.Append("<table cellpadding=\"5px\" cellspacing=\"0\" width=\"100%\">");
            foreach (MyCollectionModel myCollection in myCollectionList)
            {
                MyCollectionModel collectionDetail = myCollectionProxy.getMyCollectionDetail(myCollection);
                rowCnt++;
                StringBuilder aLink = new StringBuilder();
                aLink.Append("<a class=\"pageListLinkBold\" href=\"");
                aLink.Append("MyCollections.Detail.aspx?State=" + State);
                aLink.Append("&CollectionId=" + collectionDetail.collectionId + "\">");
                aLink.Append(collectionDetail.collectionName);
                if (collectionDetail.simpleCapModels != null)
                {
                    aLink.Append(" (" + collectionDetail.simpleCapModels.Length.ToString() + ")");
                }
                aLink.Append("</a>");
                CollectionsList.Append(MyProxy.CreateSelectListCell(aLink.ToString(), rowCnt - 1, rowCnt - 1, myCollectionList.Length, 0, 9999, isiPhone, true));
            }
            CollectionsList.Append("</table>");
        }

        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        Breadcrumbs = BreadCrumbHelper("MyCollections.aspx", Breadcrumbs, iPhonePageTitle, breadCrumbIndex, isElipseLink, false, false, false);
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
