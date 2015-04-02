#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: MyCollectionSummary.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  MyCollectionSummary user control.
 *
 *  Notes:
 * $Id: MyCollectionSummary.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Web.UI;

using Accela.ACA.BLL.MyCollection;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation MyCollectionSummary.
    /// </summary>
    public partial class MyCollectionSummary : BaseUserControl
    {
        #region Fields

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

        #region Properties

        /// <summary>
        /// Gets or sets collection id.
        /// </summary>
        public string CollectionId
        {
            get
            {
                if (ViewState["CollectionId"] != null)
                {
                    return ViewState["CollectionId"].ToString();
                }

                return string.Empty;
            }

            set
            {
                ViewState["CollectionId"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Is right to left or not.
        /// </summary>
        protected bool IsRightToLeft
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initializes my collection summary information.
        /// </summary>
        /// <param name="collectionName">collection name</param>
        /// <param name="collectionDesc">collection description</param>
        /// <param name="htSimpleCapModel">hash table simple CapModel</param>
        /// <param name="summaryModel">summary model</param>
        public void InitializationSummaryInfo(string collectionName, string collectionDesc, Hashtable htSimpleCapModel, MyCollectionSummaryModel summaryModel)
        {
            lblMyCollectionName.Text = collectionName;
            lblMyCollectionDesc.Text = collectionDesc;

            //Set values to  properties of CollectionEdit control.
            EditForCollection.CollectionId = CollectionId;
            EditForCollection.CollectionName = collectionName;
            EditForCollection.CollectionDesc = collectionDesc;

            InitializeTotalRecordsInfo(htSimpleCapModel);
            InitializeInspectionInfo(summaryModel);
            InitializeFeesInfo(summaryModel);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //todo
            if (!Page.IsPostBack)
            {
                IsRightToLeft = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft;

                if (AppSession.IsAdmin)
                {
                    ScriptManager.RegisterStartupScript(updatePanelSummary, GetType(), "ShowCollectionEditForm", "ShowCollectionForm();", true);
                }
                else
                {
                    string deleteMessage = GetTextByKey("mycollection_message_delete").Replace("'", "\\'");
                    btnDelete.Attributes.Add("onclick", string.Format("javascript:return confirm('{0}')", deleteMessage));
                }
            }
        }

        /// <summary>
        /// Delete current collection.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            IMyCollectionBll myCollectionBll = ObjectFactory.GetObject<IMyCollectionBll>();
            myCollectionBll.DeleteMyCollection(ConfigManager.SuperAgencyCode, CollectionId, AppSession.User.PublicUserId);

            MyCollectionModel[] myCollections = myCollectionBll.GetMyCollection(ConfigManager.SuperAgencyCode, AppSession.User.PublicUserId);
            
            //Add my collection models to session.
            AppSession.SetMyCollectionsToSession(myCollections);

            Response.Redirect("MyCollectionManagement.aspx");
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
            string feePaidLabel = GetTextByKey("mycollection_detailpage_label_paid");
            string feeDue = I18nNumberUtil.FormatMoneyForUI(summaryModel.feeDue);
            string feeDueLabel = GetTextByKey("mycollection_detailpage_label_due");

            lblFeeSummaryDetail.Text = I18nStringUtil.FormatToTableRow(feePaid, HTML_EMPTY, feePaidLabel, HTML_COMMA, feeDue, HTML_EMPTY, feeDueLabel);
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

            lblInspectionSummaryNum.Text = inspectionCout.ToString();

            string inspectionScheduled = summaryModel.inspectionScheduled.ToString(); // inspection count 
            string inspectionScheduledLabel = GetTextByKey("mycollection_detailpage_label_scheduled"); // inspection status
            string inspectionRescheduled = summaryModel.inspectionRescheduled.ToString();
            string inspectionRescheduledLabel = GetTextByKey("mycollection_detailpage_label_rescheduled");
            string inspectionApproved = summaryModel.inspectionApproved.ToString();
            string inspectionApprovedLabel = GetTextByKey("mycollection_detailpage_label_passed");
            string inspectionDenied = summaryModel.inspectionDenied.ToString();
            string inspectionDeniedLabel = GetTextByKey("mycollection_detailpage_label_failed");
            string inspectionPending = summaryModel.inspectionPending.ToString();
            string inspectionPendingLabel = GetTextByKey("mycollection_detailpage_label_pending");
            string inspectionCanceled = summaryModel.inspectionCanceled.ToString();
            string inspectionCanceledLabel = GetTextByKey("mycollection_detailpage_label_canceled");

            string[] inspectionList = 
                                    { 
                                        LEFTBRACKET, 
                                        inspectionScheduled,
                                        HTML_EMPTY,
                                        inspectionScheduledLabel,
                                        HTML_COMMA,
                                        inspectionRescheduled,
                                        HTML_EMPTY,
                                        inspectionRescheduledLabel,
                                        HTML_COMMA,
                                        inspectionApproved,
                                        HTML_EMPTY,
                                        inspectionApprovedLabel,
                                        HTML_COMMA,
                                        inspectionDenied,
                                        HTML_EMPTY,
                                        inspectionDeniedLabel,
                                        HTML_COMMA,
                                        inspectionPending,
                                        HTML_EMPTY,
                                        inspectionPendingLabel,
                                        HTML_COMMA,
                                        inspectionCanceled,
                                        HTML_EMPTY,
                                        inspectionCanceledLabel,
                                        RIGHTBRACKET
                                    };

            // put each element into td so that it is able to suit for multi-language align style
            lblInspectionSummaryDetail.Text = I18nStringUtil.FormatToTableRow(inspectionList);
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
                lblTotalRocordsNum.Text = totalRecords.ToString();
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
            lblTotalRecordsDetail.Text = I18nStringUtil.FormatToTableRow((string[])arrayList.ToArray(typeof(string)));
            lblTotalRocordsNum.Text = totalRecords.ToString();
        }

        #endregion Methods
    }
}
