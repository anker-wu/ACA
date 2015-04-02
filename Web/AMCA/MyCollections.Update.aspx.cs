/**
* <pre>
* 
*  Accela Citizen Access
*  File: MyCollections.Update.aspx.cs
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
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using Accela.ACA.Common;
using Accela.ACA.Web;
using Accela.ACA.Web.Common;
using Accela.ACA.BLL.MyCollection;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

/// <summary>
/// 
/// </summary>
public partial class MyCollectionsUpdate : AccelaPage
{
    private const string HTML_EMPTY = "&nbsp;";

    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder PageTitle = new StringBuilder();
    public StringBuilder ExistingOption = new StringBuilder();
    public StringBuilder NewOption = new StringBuilder();
    public StringBuilder Buttons = new StringBuilder();
    public StringBuilder BackForwardLinks = new StringBuilder();
    public StringBuilder HiddenFields = new StringBuilder();
    public StringBuilder PageMessage = new StringBuilder();
    // public string PageBreadcrumbIndex = string.Empty;
    public string LocalModuleName = string.Empty;
    public string ResultPage = string.Empty;
    public string CollectionId = string.Empty;
    public string CollectionModule = string.Empty;
    public string CollectionOperation = string.Empty;
    public string SearchMode = string.Empty;
    private string Filter = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("MyCollections.Update.aspx");
        string NoErrorFormat = "<div id=\"pageText\"\"><table cellpadding=\"1\" cellspacing=\"0\" width=\"95%\"><tr><td valign=\"top\">";
        string NoErrorFormatEnd = "</div></td></tr></table>";
        string linkHTML = "<a style=\"color:#040478;  font-size:10pt; text-decoration:underline;\" href=\"";

        State = GetFieldValue("State", false);
        SearchMode = GetFieldValue("Mode", false);
        ResultPage = GetFieldValue("ResultPage", false);
        Filter = (GetFieldValue("Status", false) != string.Empty) ? GetFieldValue("Status", false) : GetFieldValue("InspectionStatus", false);
        CollectionOperation = GetFieldValue("CollectionOperation", false);
        CollectionModule = GetFieldValue("CollectionModule", false);
        CollectionId = GetFieldValue("CollectionId", false);
        LocalModuleName = GetFieldValue("Module", false);
        string buttonChoice = GetFieldValue("button", false);
        string checkBoxChoice = GetFieldValue("CollectionOption", false);
        string displayNumber = GetFieldValue("DisplayNumber", false);
        string permitType = GetFieldValue("PermitType", false);
        string permitNumber = GetFieldValue("PermitNumber", false);
        string altID = GetFieldValue("AltID", false);
        string globalSearchMode = GetFieldValue("GlobalSearchMode", false);
        string pageMode = "Collections(" + CollectionOperation + ")";
        string sortColumn = GetFieldValue("SortColumn", false);
        string sortDesc = GetFieldValue("SortDesc", false);
        string FilterType = GetFieldValue("FilterType", false);
        string FilterMask = GetFieldValue("FilterMask", false);
        string PageBreadcrumbIndex = GetFieldValue("PageBreadcrumbIndex", false);
        string permitsDetailPage = GetFieldValue("PermitsDetailPage", false);
        bool isAllModuleView = GetFieldValue("isAllModuleView", false) == "true";
        string viewBaseModuleName = GetFieldValue("ViewBaseModuleName", false);
        HiddenFields.Append(HTML.PresentHiddenField("DisplayNumber", displayNumber));
        HiddenFields.Append(HTML.PresentHiddenField("PermitType", permitType));
        HiddenFields.Append(HTML.PresentHiddenField("PermitNumber", permitNumber));
        HiddenFields.Append(HTML.PresentHiddenField("AltID", altID));
        HiddenFields.Append(HTML.PresentHiddenField("ResultPage", ResultPage));
        HiddenFields.Append(HTML.PresentHiddenField("GlobalSearchMode", globalSearchMode));
        HiddenFields.Append(HTML.PresentHiddenField("SortColumn", sortColumn));
        HiddenFields.Append(HTML.PresentHiddenField("SortDesc", sortDesc));
        HiddenFields.Append(HTML.PresentHiddenField("FilterType", FilterType));
        HiddenFields.Append(HTML.PresentHiddenField("FilterMask", FilterMask));
        HiddenFields.Append(HTML.PresentHiddenField("ViewBaseModuleName", viewBaseModuleName));
        HiddenFields.Append(HTML.PresentHiddenField("isAllModuleView", isAllModuleView == true ? "true" : "false"));
        HiddenFields.Append(HTML.PresentHiddenField("PermitsDetailPage", permitsDetailPage));
        HiddenFields.Append(HTML.PresentHiddenField("CollectionModule", CollectionModule));
        HiddenFields.Append(HTML.PresentHiddenField("CollectionId", CollectionId));
        Buttons.Append("<div id=\"pageSubmitButton\">");
        if (isiPhone == true)
        {
            Buttons.Append("<center>");
        }

        //Collection Related GUI_TEXT entries:

        //VALUES ('aca_mycollection_name_empty', 			        'Collection Name is required', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 
        //VALUES ('mycollection_mypermitspage_label_add', 	        'Add to collection', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 
        //VALUES ('aca_mycollection_name_empty', 			        'Collection Name is required', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 
        //VALUES ('mycollection_global_label_collection', 	        'Collections', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 
        //VALUES ('mycollection_caphomepage_message_havepartial',   'Partial CAPs cannot be added to collection', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 
        //VALUES ('aca_mycollection_name_exist', 			        'The Collection name already exist.', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 
        //VALUES ('mycollection_addpage_lablel_add', 			    'Add', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 
        //VALUES ('mycollection_addpage_lablel_exsisting', 		    'Add to Existing Collection', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 

        //VALUES ('mycollection_detailpage_label_passed', 'Approved', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 
        //VALUES ('mycollection_detailpage_removecaps_message', 	'Are you sure you want to remove selected CAPs from this collection?', 'ACA Admin', SYSDATE, 
        //VALUES ('mycollection_addpage_lablel_cancel', 			'Cancel', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 
        //VALUES ('mycollection_detailpage_label_copy', 			'Copy to...', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 
        //VALUES ('mycollection_addpage_lablel_new', 			    'Create a New Collection', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 
        //VALUES ('mycollection_addpage_lablel_desription', 		'Description:', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 
        //VALUES ('mycollection_detailpage_label_feesummary',       'Fees Summary:', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 
        //VALUES ('mycollection_detailpage_label_move', 			'Move to...', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 
        //VALUES ('mycollection_detailpage_label_remove',           'Remove', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 
        //VALUES ('mycollection_addpage_lablel_onlyone', 			'The record is already a part of the collection.', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 
        //VALUES ('mycollection_addpage_lable_all', 			    'These records are already a part of the collection.', 'ACA Admin', SYSDATE, 'ADMIN', 'A', 'US', 'en'); 
        //VALUES ('mycollection_add2collection_succussfully_message','Your selection has been added to collection.\nSome records are partial caps, they cannot 

        string mycollection_detailpage_removecaps_message = LocalGetTextByKey("mycollection_detailpage_removecaps_message");
        string mycollection_caphomepage_message_havepartial = LocalGetTextByKey("mycollection_caphomepage_message_havepartial");
        string mycollection_caphomepage_message_nocapselected = LocalGetTextByKey("mycollection_caphomepage_message_nocapselected");
        string mycollection_collectiondetailpage_move_message = LocalGetTextByKey("mycollection_collectiondetailpage_move_message");
        string mycollection_mypermitpage_message_added = LocalGetTextByKey("mycollection_mypermitpage_message_added");
        string mycollection_add2collection_succussfully_message = LocalGetTextByKey("mycollection_add2collection_succussfully_message");
        string mycollection_addpage_lablel_onlyone = LocalGetTextByKey("mycollection_addpage_lablel_onlyone");
        string mycollection_addpage_lable_all = LocalGetTextByKey("mycollection_addpage_lable_all");
        string aca_mycollection_name_empty = LocalGetTextByKey("aca_mycollection_name_empty");
        string per_globalsearch_label_cap = LocalGetTextByKey("per_globalsearch_label_cap");
        string per_globalsearch_label_searchresults = LocalGetTextByKey("per_globalsearch_label_searchresults");
        string mycollection_collectionmanagement_collectionname = LocalGetTextByKey("mycollection_collectionmanagement_collectionname");
        string mycollection_addpage_lablel_exsisting = LocalGetTextByKey("mycollection_addpage_lablel_exsisting");
        string mycollection_addpage_lablel_new = LocalGetTextByKey("mycollection_addpage_lablel_new");

        string collectionId = string.Empty;
        string collectionName = string.Empty;
        bool reloadMyCollectionsList = false;
        SimpleCapModel[] caps = (SimpleCapModel[])Session["MyCollection_SelectedCaps"];
        SimpleCapModel[] capList = null;
        MyCollectionModel[] myCollection = AppSession.GetMyCollectionsFromSession();
        bool collectionOperationSucceeded = false;

        bool partialCapsInSelection = false;
        int rowIndex = 0;
        iPhonePageTitle = "";
        foreach (SimpleCapModel selectedCap in caps)
        {
            if (String.IsNullOrEmpty(selectedCap.capClass) || selectedCap.capClass == ACAConstant.COMPLETED)
            {
                rowIndex++;
            }
            else
            {
                partialCapsInSelection = true;
            }
        }
        if (rowIndex != 0)
        {
            capList = new SimpleCapModel[rowIndex];
            rowIndex = 0;
            foreach (SimpleCapModel selectedCap in caps)
            {
                if (String.IsNullOrEmpty(selectedCap.capClass) || selectedCap.capClass == ACAConstant.COMPLETED)
                {
                    SimpleCapModel cap = new SimpleCapModel();
                    cap.capID = selectedCap.capID;
                    capList[rowIndex] = cap;
                    rowIndex++;
                }
            }
        }
        if (buttonChoice != string.Empty)
        {
            if (partialCapsInSelection == true && rowIndex == 0)
            {
                // User has selected all partial caps, none can be added.
                ErrorMessage.Append(ErrorFormat);
                ErrorMessage.Append(mycollection_caphomepage_message_havepartial);
                ErrorMessage.Append(ErrorFormatEnd);
            }
            if (buttonChoice == "Cancel")
            {
                StringBuilder aURL = new StringBuilder();
                if (globalSearchMode != string.Empty)
                {
                    if (permitsDetailPage == "true")
                    {
                        aURL.Append("Permits.View.aspx?State=" + State);
                    }
                    else
                    {
                        aURL.Append("GlobalSearch.List.aspx?State=" + State);
                    }
                    aURL.Append("&SearchMode=" + globalSearchMode);
                }
                else if (SearchMode == "MyCollections")
                {
                    if (permitsDetailPage == "true")
                    {
                        aURL.Append("Permits.View.aspx?State=" + State);
                    }
                    else
                    {
                        aURL.Append("MyCollections.List.aspx?State=" + State);
                    }
                    aURL.Append("&Mode=" + SearchMode);
                    aURL.Append("&CollectionId=" + myCollection[0].collectionId.ToString());
                    aURL.Append("&CollectionModule=" + CollectionModule);
                }
                else
                {
                    if (permitsDetailPage == "true")
                    {
                        aURL.Append("Permits.View.aspx?State=" + State);
                    }
                    else
                    {
                        aURL.Append("AdvancedSearch.Results.aspx?State=" + State);
                    }
                    aURL.Append("&Mode=" + SearchMode);
                }
                aURL.Append("&Module=" + ModuleName);
                aURL.Append("&ResultPage=" + ResultPage);
                aURL.Append("&BreadcrumbIndex=" + PageBreadcrumbIndex);
                aURL.Append("&ViewBaseModuleName=" + GetFieldValue("ViewBaseModuleName", false));
                aURL.Append("&isAllModuleView=" + (isAllModuleView == true ? "true" : "false"));
                aURL.Append("&SortColumn=" + sortColumn);
                aURL.Append("&SortDesc=" + sortDesc);
                aURL.Append("&FilterType=" + FilterType);
                aURL.Append("&FilterMask=" + FilterMask);
                if (permitsDetailPage == "true")
                {
                    aURL.Append("&PermitNumber=" + permitNumber);
                    aURL.Append("&PermitType=" + permitNumber);
                    aURL.Append("&AltID=" + altID);
                }
                Response.Redirect(aURL.ToString());
            }
            IMyCollectionBll myCollectionBll = (IMyCollectionBll)ObjectFactory.GetObject(typeof(IMyCollectionBll));
            if (CollectionOperation == "Remove")
            {
                myCollection[0].simpleCapModels = capList;
                try
                {
                    myCollectionBll.RemoveCapsFromCollection(myCollection[0]);
                    collectionId = myCollection[0].collectionId == null ? String.Empty : myCollection[0].collectionId.ToString();
                    collectionOperationSucceeded = true;
                    NewOption.Append("<table cellpadding=\"1px\" cellspacing=\"0\" width=\"95%\">");
                    NewOption.Append("<tr><td>");
                    NewOption.Append(mycollection_collectiondetailpage_move_message);
                    NewOption.Append("</td></tr>");
                    NewOption.Append("</table>");
                    reloadMyCollectionsList = true;
                }
                catch (Exception ex)
                {
                    ErrorMessage.Append(ErrorFormat);
                    ErrorMessage.Append(ex.Message);
                    ErrorMessage.Append(ErrorFormatEnd);
                }
            }
            else if (checkBoxChoice == "optionExisting")
            {
                collectionId = GetFieldValue("ChoiceID", false);
                if (collectionId == string.Empty || collectionId == "--select--")
                {
                    ErrorMessage.Append(ErrorFormat);
                    ErrorMessage.Append(aca_mycollection_name_empty);
                    ErrorMessage.Append(ErrorFormatEnd);
                    collectionOperationSucceeded = false;
                }
                else if (rowIndex != 0)
                {
                    MyCollectionModel aCollection = myCollectionBll.GetCollectionDetailInfo(ConfigManager.AgencyCode, AppSession.User.PublicUserId, collectionId, null);
                    collectionName = aCollection.collectionName;
                   
                    if (aCollection != null)
                    {
                        int sourceCnt = 0;
                        int dupCnt = 0;
                        if (aCollection.simpleCapModels != null && aCollection.simpleCapModels.Length > 0)
                        {
                            if (capList != null && capList.Length != 0)
                            {
                                foreach (SimpleCapModel myCap in capList)
                                {
                                    sourceCnt++;
                                    foreach (SimpleCapModel capModel in aCollection.simpleCapModels)
                                    {
                                        if (capModel.capID != null)
                                        {
                                            if (capModel.capID.ID1 == myCap.capID.ID1
                                                && capModel.capID.ID2 == myCap.capID.ID2
                                                && capModel.capID.ID3 == myCap.capID.ID3
                                                && capModel.capID.serviceProviderCode == myCap.capID.serviceProviderCode)
                                            {
                                                dupCnt++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (sourceCnt != 0 && sourceCnt == dupCnt)
                        {
                            ErrorMessage.Append(ErrorFormat);
                            if (sourceCnt == 1)
                            {
                                ErrorMessage.Append(mycollection_addpage_lablel_onlyone);
                            }
                            else
                            {
                                ErrorMessage.Append(mycollection_addpage_lable_all);
                            }

                            if (ErrorMessage.Length == 0)
                            {
                                if (sourceCnt == 1)
                                {
                                    ErrorMessage.Append("The record");
                                }
                                else
                                {
                                    ErrorMessage.Append("The records");
                                }
                                ErrorMessage.Append(" you selected ");
                                if (sourceCnt == 1)
                                {
                                    ErrorMessage.Append("is ");
                                }
                                else
                                {
                                    ErrorMessage.Append("are ");
                                }
                                ErrorMessage.Append("already part of the collection.");
                            }
                            ErrorMessage.Append(ErrorFormatEnd);
                            collectionOperationSucceeded = false;
                        }
                        else
                        {
                            if (CollectionOperation == "Add")
                            {
                                if (String.IsNullOrEmpty(collectionId))
                                {
                                    aCollection.collectionId = null;
                                }
                                else
                                {
                                    aCollection.collectionId = Convert.ToInt64(collectionId);
                                }
                                
                                aCollection.simpleCapModels = capList;
                                try
                                {
                                    myCollectionBll.AddCaps2Collection(aCollection);
                                    collectionOperationSucceeded = true;
                                    ErrorMessage.Append(NoErrorFormat);
                                    if (partialCapsInSelection == true)
                                    {
                                        ErrorMessage.Append(mycollection_add2collection_succussfully_message);
                                    }
                                    else
                                    {
                                        ErrorMessage.Append(mycollection_mypermitpage_message_added);
                                    }
                                    ErrorMessage.Append(NoErrorFormatEnd);
                                }
                                catch (Exception ex)
                                {
                                    ErrorMessage.Append(ex.Message);
                                }
                            }
                            else if (CollectionOperation == "Move" || CollectionOperation == "Copy")
                            {
                                myCollection[0].simpleCapModels = capList;
                                try
                                {
                                    if (CollectionOperation == "Move")
                                    {
                                        myCollectionBll.MoveCaps2Collection(myCollection[0], aCollection);
                                        ErrorMessage.Append(NoErrorFormat);
                                        ErrorMessage.Append(mycollection_collectiondetailpage_move_message);
                                        ErrorMessage.Append(NoErrorFormatEnd);
                                        reloadMyCollectionsList = true;
                                    }
                                    else
                                    {
                                        myCollectionBll.CopyCaps2Collection(myCollection[0], aCollection);
                                        ErrorMessage.Append(NoErrorFormat);
                                        ErrorMessage.Append(mycollection_mypermitpage_message_added);
                                        ErrorMessage.Append(NoErrorFormatEnd);
                                    }
                                    collectionOperationSucceeded = true;
                                }
                                catch (Exception ex)
                                {
                                    ErrorMessage.Append(ErrorFormat);
                                    ErrorMessage.Append(ex.Message);
                                    ErrorMessage.Append(ErrorFormatEnd);
                                }
                            }
                        }
                    }
                }
            }
            else if (checkBoxChoice == "optionNew")
            {
                collectionName = GetFieldValue("CollectionName", false);
                if (collectionName.Trim() == string.Empty)
                {
                    ErrorMessage.Append(ErrorFormat);
                    ErrorMessage.Append("A collection name is required.  Please enter a colleciton name.");
                    ErrorMessage.Append(ErrorFormatEnd);
                    collectionOperationSucceeded = false;
                }
                else if (rowIndex != 0)
                {
                    myCollectionBll = (IMyCollectionBll)ObjectFactory.GetObject(typeof(IMyCollectionBll));
                    MyCollectionModel aCollection = new MyCollectionModel();
                    aCollection.collectionName = collectionName;
                    aCollection.serviceProviderCode = ConfigManager.AgencyCode;
                    aCollection.userId = AppSession.User.PublicUserId;
                    aCollection.auditID = aCollection.userId;
                    aCollection.simpleCapModels = capList;
                    aCollection.collectionDescription = GetFieldValue("CollectionDescription", false);

                    try
                    {
                        // myCollectionBll.CreateMyCollection(aCollection);
                        myCollectionBll.AddCaps2Collection(aCollection);
                        collectionOperationSucceeded = true;
                        ErrorMessage.Append(NoErrorFormat);
                        ErrorMessage.Append(mycollection_mypermitpage_message_added);
                        if (partialCapsInSelection == true)
                        {
                            ErrorMessage.Append(mycollection_add2collection_succussfully_message);
                        }
                        ErrorMessage.Append(NoErrorFormatEnd);

                        MyCollectionModel[] currentList = myCollectionBll.GetMyCollection(ConfigManager.AgencyCode, AppSession.User.PublicUserId);
                        foreach (MyCollectionModel aRow in currentList)
                        {
                            if (aRow.collectionName == collectionName)
                            {
                                collectionId = aRow.collectionId == null ? String.Empty : aRow.collectionId.ToString();
                                collectionName = aRow.collectionName;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage.Append(ErrorFormat);
                        ErrorMessage.Append(ex.Message);
                        ErrorMessage.Append(ErrorFormatEnd);
                    }
                }
            }
        }
        // Set page Title
        // And input controls
        iPhonePageTitle = LocalGetTextByKey("mycollection_collectionmanagement_collectionname") + "-" + CollectionOperation;
        switch (CollectionOperation)
        {
            case "Add":
            case "Move":
            case "Copy":
                PageTitle.Append("<div id=\"pageTitle\">" + CollectionOperation + " to");
                if (collectionOperationSucceeded == true)
                {
                    PageTitle.Append(" " + mycollection_collectionmanagement_collectionname);
                    PageTitle.Append("</div>");
                    linkHTML = "<a class=\"pageTextLink\" href=\"";
                    ExistingOption.Append("<br>" + linkHTML);
                    ExistingOption.Append("MyCollections.Detail.aspx?State=" + State);
                    ExistingOption.Append("&Mode=" + SearchMode);
                    ExistingOption.Append("&CollectionId=" + collectionId);
                    ExistingOption.Append("&Module=" + ModuleName);
                    ExistingOption.Append("&TruncateBreadCrumb=-1");
                    ExistingOption.Append("\">View Collection: " + collectionName + "</a><br>");
                }
                else
                {
                    PageTitle.Append("</div>");

                    MyCollectionProxy myCollectionProxy = new MyCollectionProxy();
                    myCollectionProxy.collectionModule = ModuleName;
                    MyCollectionModel[] myCollectionList = myCollectionProxy.getMyCollectionsList();

                    int collectionsCnt = myCollectionList != null ? myCollectionList.Length : 0;
                    if (collectionsCnt != 0)
                    {
                        ExistingOption.Append("<table cellpadding=\"1px\" cellspacing=\"0\" width=\"95%\">");
                        ExistingOption.Append("<tr><td width=\"5px\">");
                        ExistingOption.Append("<input type=\"radio\" " + (collectionsCnt > 0 ? "CHECKED" : string.Empty) + " id=\"CollectionOption\" name=\"CollectionOption\" value=\"optionExisting\"/>");
                        ExistingOption.Append("</td><td id=\"pageSectionTitle\">");
                        ExistingOption.Append("Existing " + mycollection_collectionmanagement_collectionname);
                        ExistingOption.Append("</td></tr>");
                        ExistingOption.Append("<tr><td width=\"5px\"></td><td>");
                        ExistingOption.Append("<select class=\"pageTextInput\" id=\"ChoiceId\" name=\"ChoiceId\">");
                        ExistingOption.Append("<option value=\"--select--\">--select--</option>");
                        if (myCollectionList != null)
                        {
                            foreach (MyCollectionModel aCollection in myCollectionList)
                            {
                                if (CollectionOperation != "Add")
                                {
                                    if (myCollection[0].collectionId == aCollection.collectionId)
                                    {
                                        continue;
                                    }
                                }
                                ExistingOption.Append("<option value=\"" + aCollection.collectionId + "\">" + aCollection.collectionName + "</option>");
                            }
                        }
                        ExistingOption.Append("</select>");
                        ExistingOption.Append("</td></tr>");
                        ExistingOption.Append("</table>");
                    }
                    NewOption.Append("<table cellpadding=\"1px\" cellspacing=\"0\" width=\"95%\">");
                    NewOption.Append("<tr><td width=\"5px\">");
                    NewOption.Append("<input type=\"radio\"  " + (collectionsCnt == 0 ? "CHECKED" : string.Empty) + " id=\"CollectionOption\" name=\"CollectionOption\" value=\"optionNew\"/>");
                    NewOption.Append("</td><td id=\"pageSectionTitle\">");
                    NewOption.Append("New " + mycollection_collectionmanagement_collectionname);
                    NewOption.Append("</td></tr>");

                    NewOption.Append("<tr><td width=\"5px\">");
                    NewOption.Append("</td><td id=\"pageSectionTitle\">");
                    NewOption.Append("*Name:");
                    NewOption.Append("</td></tr>");

                    NewOption.Append("<tr><td width=\"5px\">");
                    NewOption.Append("</td><td>");
                    NewOption.Append("<input type=\"text\" class=\"pageTextInput\" id=\"CollectionName\" name=\"CollectionName\"/>");
                    NewOption.Append("</td></tr>");

                    NewOption.Append("<tr><td width=\"5px\">");
                    NewOption.Append("</td><td id=\"pageSectionTitle\">");
                    NewOption.Append("Description:");
                    NewOption.Append("</td></tr>");

                    NewOption.Append("<tr><td width=\"5px\">");
                    NewOption.Append("</td><td>");
                    NewOption.Append("<textarea name=\"CollectionDescription\" cols=\"20\" rows=\"4\" class=\"pageTextAreaInput\"></textarea>");
                    NewOption.Append("</td></tr>");
                    NewOption.Append("</table><br>");

                    Buttons.Append("<input id=\"button\" name=\"button\" type=\"submit\" value=\"Add\" />");
                }
                break;
            case "Remove":
                PageTitle.Append("<div id=\"pageTitle\">" + CollectionOperation + " from ");
                PageTitle.Append(" " + mycollection_collectionmanagement_collectionname);
                PageTitle.Append("</div>");
                if (collectionOperationSucceeded != true)
                {
                    NewOption.Append("<table cellpadding=\"1px\" cellspacing=\"0\" width=\"95%\" style=\"margin-top:10px; margin-bottom:15px\">");
                    NewOption.Append("<tr><td width=\"95%\">");
                    if (mycollection_detailpage_removecaps_message != null && mycollection_detailpage_removecaps_message != string.Empty)
                    {
                        NewOption.Append(mycollection_detailpage_removecaps_message);
                    }
                    else
                    {
                        NewOption.Append(String.Format("The {0} that you have selected will be removed from this collection.",
                            (caps.Length > 1 ? caps.Length.ToString() + " records" : "record")));
                    }
                    NewOption.Append("</td></tr>");
                    NewOption.Append("</table>");
                    Buttons.Append("<input id=\"button\" name=\"button\" type=\"submit\" value=\"" + CollectionOperation + "\" />");
                }
                break;
        }
        if (collectionOperationSucceeded != true)
        {
            Buttons.Append(HTML_EMPTY + HTML_EMPTY + HTML_EMPTY + HTML_EMPTY + "<input id=\"button\" name=\"button\" type=\"submit\" value=\"Cancel\" />");
        }
        if (isiPhone == true)
        {
            Buttons.Append("</center>");
        }
        Buttons.Append("</div>");
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        bool isBreadcrumbPagingMode = GetFieldValue("PagingMode", false) == "true";

        string breadCrumbIndex = GetFieldValue("PageBreadcrumbIndex", false);
        StringBuilder sbWork = new StringBuilder();
        sbWork.Append("&Mode=" + SearchMode);
        sbWork.Append("&Module=" + ModuleName);
        sbWork.Append("&CollectionOperation=" + CollectionOperation);
        sbWork.Append("&CollectionModule=" + CollectionModule);
        sbWork.Append("&CollectionId=" + CollectionId);
        sbWork.Append("&PermitsDetailPage=" + permitsDetailPage);
        sbWork.Append("&ResultPage=" + ResultPage);
        if (reloadMyCollectionsList == true && collectionOperationSucceeded == true && SearchMode == "MyCollections")
        {
            // Session["AMCA_Reload_Collection_List"] = "true";
            string previousBreadcrumbIndex = breadCrumbIndex;
            if (breadCrumbIndex != null && breadCrumbIndex != string.Empty)
            {
                int intWork = 0;
                if (int.TryParse(breadCrumbIndex, out intWork) == true)
                {
                    previousBreadcrumbIndex = (intWork--).ToString();
                }
                
            }
            BreadcrumbAppendParameter(previousBreadcrumbIndex, "ReloadList", "true");
        }
        Breadcrumbs = BreadCrumbHelper("MyCollections.Update.aspx", sbWork, pageMode, "", false, false, isBreadcrumbPagingMode, false);
        BackForwardLinks.Append(BackLinkHelper(CurrentBreadCrumbIndex.ToString()));
        HiddenFields.Append(HTML.PresentHiddenField("PageBreadcrumbIndex", CurrentBreadCrumbIndex.ToString()));
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
