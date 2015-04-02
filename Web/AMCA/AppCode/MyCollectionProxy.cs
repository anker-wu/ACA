/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: MyCollectionProxy.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2011
 * 
 *  Description:
 *  Code copied from ACA MyCollections pages for use in AMCA
 *  Notes:
 *      $Id: MyCollectionsProxy.cs 77905 2007-10-15 12:49:28Z dave.brewster $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 *  09-18-2009          DWB                     2008 Mobile ACA interface redesign
 * </pre>
 */

using System.Collections.Generic;
using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

using Accela.ACA.BLL.MyCollection;
using Accela.ACA.Common;
using Accela.ACA.Web;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.Common;
using Accela.ACA.WSProxy.WSModel;
using Accela.ACA.Common.Util;


/// <summary>
/// Summary description for MyCollectionProxy
/// </summary>
public class MyCollectionProxy
{
    #region Fields - copied from Web\MyCollection\MyCollectionSummary.ascx.cs

    /// <summary>
    /// Is right to left.
    /// </summary>
    protected bool _isRightToLeft;

    /// <summary>
    /// HTML comma
    /// </summary>
    private const string HTML_COMMA = ",&nbsp;";

    /// <summary>
    /// HTML empty
    /// </summary>
    private const string HTML_EMPTY = "&nbsp;";

    /// <summary>
    /// HTML left bracket.
    /// </summary>
    private const string LEFTBRACKET = "(";

    /// <summary>
    /// HTML right bracket.
    /// </summary>
    private const string RIGHTBRACKET = ")";

    #endregion Fields
    public bool OnErrorReturn = false;
    public bool TestReturnError = false;
    public string ExceptionMessage = string.Empty;

    public string lblTotalRecordsDetailText = string.Empty;
    public string lblTotalRocordsNumText = string.Empty;
    public string lblMyCollectionNameText = string.Empty;
    public string lblMyCollectionDescText = string.Empty;
    public string lblInspectionSummaryDetailText = string.Empty;
    public string lblFeeSummaryDetailText = string.Empty;
    public string lblInspectionSummaryNumText = string.Empty;
    public string collectionModule = string.Empty;

    /// <summary>
    /// Create list of collections for current user
    /// </summary>
    /// <returns>MyCollectionModel4WS[]</returns>
    public int getMyCollectionsCount()
    {
        MyCollectionModel[] myCollections = null;
        try
        {
            IMyCollectionBll myCollectionBll = (IMyCollectionBll)ObjectFactory.GetObject(typeof(IMyCollectionBll));
            myCollections = myCollectionBll.GetMyCollection(ConfigManager.AgencyCode, AppSession.User.PublicUserId);
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        if (myCollections == null)
        {
            return 0;
        }
        else
        {
            return myCollections.Length;
        }
    }

    /// <summary>
    /// Retrieve the collection details for the collection.
    /// </summary>
    /// <param name="myCollection">MyCollectionModel4WS</param>
    /// <returns>MyCollectionModel4WS</returns>
    public MyCollectionModel getMyCollectionDetail(MyCollectionModel myCollection)
    {
        MyCollectionModel collectionDetail = null;
        try
        {
            IMyCollectionBll myCollectionBll = (IMyCollectionBll)ObjectFactory.GetObject(typeof(IMyCollectionBll));
            collectionDetail = myCollectionBll.GetCollectionDetailInfo(ConfigManager.AgencyCode, AppSession.User.PublicUserId, myCollection.collectionId.ToString(), null);
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        return collectionDetail;
    }

    /// <summary>
    /// Create list of collections for current user
    /// </summary>
    /// <returns>MyCollectionModel4WS[]</returns>
    public MyCollectionModel[] getMyCollectionsList()
    {
        MyCollectionModel[] myCollections = null;
        try
        {
            IMyCollectionBll myCollectionBll = (IMyCollectionBll)ObjectFactory.GetObject(typeof(IMyCollectionBll));
            myCollections = myCollectionBll.GetMyCollection(ConfigManager.AgencyCode, AppSession.User.PublicUserId);
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        return myCollections;
    }

    /// <summary>
    /// Returns list of permits for current user.
    /// </summary>
    /// <param name="permitCondition"></param>
    /// <returns></returns>
    public DataTable getMyCollectionList(Nullable<long> collectionId, string moduleName)
    {
        MyCollectionModel myCollection = new MyCollectionModel();
        myCollection.collectionId = collectionId;
        DataTable tempPermit = null;
        // Permit[] tempPermit = null;
        try
        {
            IMyCollectionBll myCollectionBll = (IMyCollectionBll)ObjectFactory.GetObject(typeof(IMyCollectionBll));
            MyCollectionModel collectionDetail = myCollectionBll.GetCollectionDetailInfo(ConfigManager.AgencyCode, AppSession.User.PublicUserId, myCollection.collectionId.ToString(), null);
            if (collectionDetail != null)
            {
                Permit tempSinglePermit = new Permit();
                if (collectionDetail.simpleCapModels != null)
                {
                    AccelaProxy accelaProxy = new AccelaProxy();
                    tempPermit = accelaProxy.CreateCapListDataSource(collectionDetail.simpleCapModels, moduleName, false);
                    // tempPermit = ConvertUtil.convertSimpleCapsToPermits(collectionDetail.simpleCapModels, moduleName);
                    //for (int i = 0; i < tempPermit.Length; i++)
                    //{
                    //    if (collectionDetail.simpleCapModels[i].addressModel != null)
                    //        tempPermit[i].Address = collectionDetail.simpleCapModels[i].addressModel.displayAddress;
                    //}
                }
            }
        }
        catch (Exception ex)
        {
            OnErrorReturn = true;
            ExceptionMessage = ex.Message.ToString();
        }
        return tempPermit;
    }

    #region Code Copied from Web\MyCollection\MyCollectionDetail.aspx.cs
    /// <summary>
    /// Get simpleCapModel list and set it to hashtable.
    /// </summary>
    /// <param name="simpleCapModels">Simple Cap Model List</param>
    /// <returns>hash table for SimpleCapModelList</returns>
    public Hashtable GetSimpleCapModelListByModuleName(SimpleCapModel[] simpleCapModels)
    {
        Hashtable htSimpleCapModel = new Hashtable();

        if (simpleCapModels == null || simpleCapModels.Length == 0)
        {
            return null;
        }

        foreach (SimpleCapModel simpleCapModel in simpleCapModels)
        {
            string moduleName = simpleCapModel.moduleName;

            IList simpleCapModelList = (ArrayList)htSimpleCapModel[moduleName];

            if (simpleCapModelList == null)
            {
                simpleCapModelList = new ArrayList();
                htSimpleCapModel.Add(moduleName, simpleCapModelList);
            }

            simpleCapModelList.Add(simpleCapModel);
        }

        return htSimpleCapModel;
    }
    /// <summary>
    /// Initializate my collection summary information.
    /// </summary>
    /// <param name="collectionName">collection name</param>
    /// <param name="collectionDesc">collection description</param>
    /// <param name="htSimpleCapModel">simple CapModel</param>
    /// <param name="summaryModel">summary CapModel</param>
    public void BuildSummaryInformation(string collectionName, string collectionDesc, Hashtable htSimpleCapModel, MyCollectionSummaryModel summaryModel)
    {
        // SummaryInformation.InitializationSummaryInfo(collectionName, collectionDesc, htSimpleCapModel, summaryModel);
        this.InitializationSummaryInfo(collectionName, collectionDesc, htSimpleCapModel, summaryModel);
    }
    #endregion Web\MyCollections\MyCollections.aspx.cs

    #region Code Copied from Web\Component\MyCollectionSummary.ascx.cs

    /// <summary>
    /// Initializate my collection summary information.
    /// </summary>
    /// <param name="collectionName">collection name</param>
    /// <param name="collectionDesc">collection desc</param>
    /// <param name="htSimpleCapModel">hash table simple CapModel</param>
    /// <param name="summaryModel">summary model</param>
    private void InitializationSummaryInfo(string collectionName, string collectionDesc, Hashtable htSimpleCapModel, MyCollectionSummaryModel summaryModel)
    {
        //this.lblMyCollectionName.Text = collectionName;
        //this.lblMyCollectionDesc.Text = collectionDesc;
        this.lblMyCollectionNameText = collectionName;
        this.lblMyCollectionDescText = collectionDesc;

        //Set values to  properties of CollectionEdit control.
        //EditForCollection.CollectionId = CollectionId;
        //EditForCollection.CollectionName = collectionName;
        //EditForCollection.CollectionDesc = collectionDesc;

        InitializeTotalRecordsInfo(htSimpleCapModel);
        InitializeInspectionInfo(summaryModel);
        InitializeFeesInfo(summaryModel);
    }

    /// <summary>
    /// Initialize Fees summary for my collection.
    /// </summary>
    /// <param name="summaryModel">summary model</param>
    private void InitializeFeesInfo(MyCollectionSummaryModel summaryModel)
    {
        if (summaryModel == null)
        {
            return;
        }

        string feePaid = I18nNumberUtil.FormatMoneyForUI(summaryModel.feePaid);
        string feePaidLabel = LabelUtil.GetTextByKey("mycollection_detailpage_label_paid", collectionModule);
        string feeDue = I18nNumberUtil.FormatMoneyForUI(summaryModel.feeDue);
        string feeDueLabel = LabelUtil.GetTextByKey("mycollection_detailpage_label_due", collectionModule);

        //this.lblFeeSummaryDetailText = I18nStringUtil.FormatToTableRow(feePaid, HTML_EMPTY, feePaidLabel, HTML_COMMA, feeDue, HTML_EMPTY, feeDueLabel);
        this.lblFeeSummaryDetailText = feePaid + HTML_EMPTY + feePaidLabel + HTML_COMMA + feeDue + HTML_EMPTY + feeDueLabel;
    }

    /// <summary>
    /// Initialize Inspections summary for my collection.
    /// </summary>
    /// <param name="summaryModel">summary model</param>
    private void InitializeInspectionInfo(MyCollectionSummaryModel summaryModel)
    {
        if (summaryModel == null)
        {
            return;
        }

        int inspectionCout = summaryModel.inspectionDenied + summaryModel.inspectionApproved + summaryModel.inspectionScheduled
             + summaryModel.inspectionRescheduled + summaryModel.inspectionPending + summaryModel.inspectionCanceled;
        this.lblInspectionSummaryNumText = inspectionCout.ToString();

        StringBuilder inspectionCounts = new StringBuilder();
        string aComma = "";
        //string inspectionScheduled = summaryModel.inspectionScheduled.ToString(); // inspection count 
        //string inspectionScheduledLabel = LabelUtil.GetTextByKey("mycollection_detailpage_label_scheduled", collectionModule); // inspection status
        if (summaryModel.inspectionScheduled > 0)
        {
            inspectionCounts.Append(summaryModel.inspectionScheduled.ToString() + HTML_EMPTY + "Scheduled");
            aComma = ", ";
        }
        //string inspectionRescheduled = summaryModel.inspectionRescheduled.ToString();
        //string inspectionRescheduledLabel = LabelUtil.GetTextByKey("mycollection_detailpage_label_rescheduled", collectionModule);
        if (summaryModel.inspectionRescheduled > 0)
        {
            inspectionCounts.Append(aComma + summaryModel.inspectionRescheduled.ToString() + HTML_EMPTY + "Rescheduled");
            aComma = ", ";
        }
        //string inspectionApproved = summaryModel.inspectionApproved.ToString();
        //string inspectionApprovedLabel = LabelUtil.GetTextByKey("mycollection_detailpage_label_passed", collectionModule);
        if (summaryModel.inspectionApproved > 0)
        {
            inspectionCounts.Append(aComma + summaryModel.inspectionApproved.ToString() + HTML_EMPTY + "Approved");
            aComma = ", ";
        }
        //string inspectionDenied = summaryModel.inspectionDenied.ToString();
        //string inspectionDeniedLabel = LabelUtil.GetTextByKey("mycollection_detailpage_label_failed", collectionModule);
        if (summaryModel.inspectionDenied > 0)
        {
            inspectionCounts.Append(aComma + summaryModel.inspectionDenied.ToString() + HTML_EMPTY + "Denied");
            aComma = ", ";
        }
        //string inspectionPending = summaryModel.inspectionPending.ToString();
        //string inspectionPendingLabel = LabelUtil.GetTextByKey("mycollection_detailpage_label_pending", collectionModule);
        if (summaryModel.inspectionPending > 0)
        {
            inspectionCounts.Append(aComma + summaryModel.inspectionPending.ToString() + HTML_EMPTY + "Pending");
            aComma = ", ";
        }
        //string inspectionCanceled = summaryModel.inspectionCanceled.ToString();
        //string inspectionCanceledLabel = LabelUtil.GetTextByKey("mycollection_detailpage_label_canceled", collectionModule);
        if (summaryModel.inspectionCanceled > 0)
        {
            inspectionCounts.Append(aComma + summaryModel.inspectionCanceled.ToString() + HTML_EMPTY + " Cancelled");
        }

        //string[] inspectionList = {
        //                                  LEFTBRACKET, inspectionScheduled, HTML_EMPTY, inspectionScheduledLabel, HTML_COMMA, 
        //                                  inspectionRescheduled, HTML_EMPTY, inspectionRescheduledLabel, HTML_COMMA,
        //                                  inspectionApproved, HTML_EMPTY, inspectionApprovedLabel, HTML_COMMA,
        //                                  inspectionDenied, HTML_EMPTY, inspectionDeniedLabel, HTML_COMMA,
        //                                  inspectionPending, HTML_EMPTY, inspectionPendingLabel, HTML_COMMA,
        //                                  inspectionCanceled, HTML_EMPTY, inspectionCanceledLabel, RIGHTBRACKET
        //                              };

        // put each element into td so that it is able to suit for multi-language align style
        this.lblInspectionSummaryDetailText = inspectionCounts.ToString();
    }

    /// <summary>
    /// Initialize total records for my collection.
    /// </summary>
    /// <param name="htSimpleCapModel">hash table simple CapModel</param>
    private void InitializeTotalRecordsInfo(Hashtable htSimpleCapModel)
    {
        int totalRecords = 0;
        if (htSimpleCapModel == null || htSimpleCapModel.Count == 0)
        {
            // this.lblTotalRocordsNum.Text = totalRecords.ToString();
            this.lblTotalRocordsNumText = totalRecords.ToString();
            return;
        }

        ArrayList arrayList = new ArrayList();
        arrayList.Add(LEFTBRACKET);

        foreach (DictionaryEntry simpleCapModel in htSimpleCapModel)
        {
            string moduleName = LabelUtil.GetI18NModuleName(simpleCapModel.Key.ToString());
            ArrayList simpleCapModelList = (ArrayList)simpleCapModel.Value;

            arrayList.Add(simpleCapModelList.Count.ToString());
            arrayList.Add(HTML_EMPTY);
            arrayList.Add(moduleName);
            arrayList.Add(HTML_COMMA);
            totalRecords = totalRecords + simpleCapModelList.Count;
        }

        if (arrayList.Count > 0)
        {
            arrayList.RemoveAt(arrayList.Count - 1);
        }

        arrayList.Add(RIGHTBRACKET);
        //lblTotalRecordsDetail.Text = I18nStringUtil.FormatToTableRow((string[])arrayList.ToArray(typeof(string)));
        //lblTotalRocordsNum.Text = totalRecords.ToString();
        lblTotalRecordsDetailText = I18nStringUtil.FormatToTableRow((string[])arrayList.ToArray(typeof(string)));
        lblTotalRocordsNumText = totalRecords.ToString();
    }
    #endregion Web\Component\MyCollectionSummary.ascx.cs
}
