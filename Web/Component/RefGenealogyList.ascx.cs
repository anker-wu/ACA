#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefGenealogyList.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefGenealogyList.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation RefGenealogyList.
    /// </summary>
    public partial class RefGenealogyList : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// the url split char.
        /// </summary>
        private const string UrlSplitChar = "#";

        /// <summary>
        /// Gets or sets  address data table.
        /// </summary>
        public string RefParcelNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets  address data table.
        /// </summary>
        private DataTable HistoryGenealogy
        {
            get
            {
                if (ViewState["HistoryGenealogy"] == null)
                {
                    ViewState["HistoryGenealogy"] = new DataTable();
                }

                return (DataTable)ViewState["HistoryGenealogy"];
            }

            set
            {
                ViewState["HistoryGenealogy"] = value;
            }
        }

        /// <summary>
        /// Gets or sets  address data table.
        /// </summary>
        private DataTable ChildGenealogy
        {
            get
            {
                if (ViewState["ChildGenealogy"] == null)
                {
                    ViewState["ChildGenealogy"] = new DataTable();
                }

                return (DataTable)ViewState["ChildGenealogy"];
            }

            set
            {
                ViewState["ChildGenealogy"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    BindGenealogyChild();
                    BindGenealogyHistory();
                    return;
                }

                if (StandardChoiceUtil.IsEnableParcelGenealogy())
                {
                    List<GenealogyTransactionModel> genealogyTransations = GetGenealogyTransations();

                    List<GenealogyTransactionModel> historyGenealogyTransations = GetGenealogyTranactionsByType(genealogyTransations, true);

                    HistoryGenealogy = ConstructGenealogyTable(historyGenealogyTransations);

                    List<GenealogyTransactionModel> childGenealogyTransations = GetGenealogyTranactionsByType(genealogyTransations, false);

                    ChildGenealogy = ConstructGenealogyTable(childGenealogyTransations);
                }
            }

            BindGenealogyHistory();
            BindGenealogyChild();
        }

        /// <summary>
        /// GridView RefGenealogyHistory RowDataBound
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void RefGenealogyHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                object actionRowView = rowView[ColumnConstant.RefGenealogy.Action.ToString()];

                if (actionRowView != null)
                {
                    AccelaLabel lblGenHistoryAction = (AccelaLabel)e.Row.FindControl("lblGenHistoryAction");
                    string lableKey = ParcelGenealogyActionType.Merge.ToString().Equals(actionRowView.ToString()) ? "genealogyactiontype_merge_from" : "genealogyactiontype_split_from";
                    lblGenHistoryAction.Text = LabelUtil.GetTextByKey(lableKey, string.Empty);
                }

                PlaceHolder phGenHistoryChildren = (PlaceHolder)e.Row.FindControl("phGenHistoryChildren");
                object childRowView = rowView[ColumnConstant.RefGenealogy.Children.ToString()];
                List<string> childrenParcelIDs = new List<string>();

                if (childRowView != null)
                {
                    childrenParcelIDs = childRowView.ToString().Split(ACAConstant.SPLIT_CHAR).ToList();
                    AddControlToList(childrenParcelIDs, phGenHistoryChildren);
                }

                PlaceHolder phGenHistoryParents = (PlaceHolder)e.Row.FindControl("phGenHistoryParents");
                object parentsRowView = rowView[ColumnConstant.RefGenealogy.Parents.ToString()];

                if (parentsRowView != null)
                {
                    List<string> parentParcelIDs = parentsRowView.ToString().Split(ACAConstant.SPLIT_CHAR).ToList();
                    DisplayParentParcel(GetParcelIDs(childrenParcelIDs), parentParcelIDs, phGenHistoryParents);
                }
            }
        }

        /// <summary>
        /// Raises the <c>System.Web.UI.Control.Init</c> event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(dgvRefGenealogyHistory, ModuleName, AppSession.IsAdmin);
            GridViewBuildHelper.SetSimpleViewElements(dgvRefGenealogyChild, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// GridView RefGenealogyChild RowDataBound
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void RefGenealogyChild_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem; 
                object actionRowView = rowView[ColumnConstant.RefGenealogy.Action.ToString()];

                if (actionRowView != null)
                {
                    AccelaLabel lblGenealogyChildAction = (AccelaLabel)e.Row.FindControl("lblGenealogyChildAction");
                    string lableKey = ParcelGenealogyActionType.Merge.ToString().Equals(actionRowView.ToString()) ? "genealogyactiontype_merge_into" : "genealogyactiontype_split_into";
                    lblGenealogyChildAction.Text = LabelUtil.GetTextByKey(lableKey, string.Empty);
                }

                PlaceHolder phGenealogyChildChildren = (PlaceHolder)e.Row.FindControl("phGenealogyChildChildren");
                object childRowView = rowView[ColumnConstant.RefGenealogy.Children.ToString()];
                List<string> childrenParcelIDs = new List<string>();
                
                if (childRowView != null)
                {
                    childrenParcelIDs = childRowView.ToString().Split(ACAConstant.SPLIT_CHAR).ToList();
                    AddControlToList(childrenParcelIDs, phGenealogyChildChildren);
                }

                PlaceHolder phGenChildParents = (PlaceHolder)e.Row.FindControl("phGenChildParents");
                object parentsRowView = rowView[ColumnConstant.RefGenealogy.Parents.ToString()];

                if (parentsRowView != null)
                {
                    List<string> parentParcelIDs = parentsRowView.ToString().Split(ACAConstant.SPLIT_CHAR).ToList();
                    DisplayParentParcel(GetParcelIDs(childrenParcelIDs), parentParcelIDs, phGenChildParents);
                }
            }
        }

        /// <summary>
        /// Add controls to list control.
        /// </summary>
        /// <param name="parcelIDAndSeqNums">The parcel id and parcel sequence number.</param>
        /// <param name="phParcel">The parcel place holder.</param>
        private void AddControlToList(List<string> parcelIDAndSeqNums, PlaceHolder phParcel)
        {
            if (parcelIDAndSeqNums != null && parcelIDAndSeqNums.Count > 0)
            {
                int parcelCount = parcelIDAndSeqNums.Count;
                int j = 0;

                foreach (string parcelIDAndSeqNum in parcelIDAndSeqNums)
                {
                    string[] parcelArray = parcelIDAndSeqNum.Split(ACAConstant.SPLIT_CHAR1);

                    if (parcelArray == null || parcelArray.Length < 3)
                    {
                        continue;
                    }

                    string parcelID = parcelArray[0];
                    string genSeqNum = parcelArray[1];

                    HyperLink lnkParcelID = new HyperLink();
                    lnkParcelID.Text = parcelID;
                    lnkParcelID.CssClass = "ACA_NoticeTitle";
                    lnkParcelID.NavigateUrl = string.Format(
                                                            "../APO/ParcelDetail.aspx?{0}={1}&{2}={3}&HideHeader={4}&{5}={6}&isForGenealogy=1",
                                                            ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER,
                                                            UrlEncode(parcelID),
                                                            ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE,
                                                            UrlEncode(genSeqNum),
                                                            Page.Request["HideHeader"],
                                                            UrlConstant.AgencyCode,
                                                            Page.Request[UrlConstant.AgencyCode]);
                    phParcel.Controls.Add(lnkParcelID);

                    if (j < parcelCount)
                    {
                        Literal br = new Literal();
                        br.Text = "<br/>";
                        phParcel.Controls.Add(br);
                    }

                    j++;
                }
            }
        }

        /// <summary>
        /// Add controls to list control.
        /// </summary>
        /// <param name="childrenParcelIDs">The child parcel ids.</param>
        /// <param name="parentParcelIDAndSeqNums">The parcel id and parcel sequence number.</param>
        /// <param name="phParcel">The parcel place holder.</param>
        private void DisplayParentParcel(List<string> childrenParcelIDs, List<string> parentParcelIDAndSeqNums, PlaceHolder phParcel)
        {
            if (parentParcelIDAndSeqNums != null && parentParcelIDAndSeqNums.Count > 0)
            {
                int parcelCount = parentParcelIDAndSeqNums.Count;
                int parcelIndex = 0;

                foreach (string parentParcelIDAndSeqNum in parentParcelIDAndSeqNums)
                {
                    string[] parcelArray = parentParcelIDAndSeqNum.Split(ACAConstant.SPLIT_CHAR1);

                    if (parcelArray == null || parcelArray.Length < 3)
                    {
                        continue;
                    }

                    string parcelID = parcelArray[0];
                    string sourceSeqNum = parcelArray[1];
                    string apoSeqNum = parcelArray[2];

                    HyperLink lnkParcelID = new HyperLink();
                    lnkParcelID.Text = parcelID;
                    string parcelIDURLParam = parcelID;
                    string childParcelNum = string.Empty;

                    if (childrenParcelIDs != null && childrenParcelIDs.Count > 0 && childrenParcelIDs.Contains(parcelID))
                    {
                        parcelIDURLParam = string.Format("{0}{1}{2}{3}{4}", UrlSplitChar, sourceSeqNum, UrlSplitChar, apoSeqNum, UrlSplitChar);
                        childParcelNum = "&childParcelNum=" + UrlEncode(parcelID);
                    }

                    lnkParcelID.CssClass = "ACA_NoticeTitle";
                    lnkParcelID.NavigateUrl = string.Format(
                                                            "../APO/ParcelDetail.aspx?{0}={1}&{2}={3}{4}&HideHeader={5}&{6}={7}&isForGenealogy=1",
                                                            ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER,
                                                            UrlEncode(parcelIDURLParam),
                                                            ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE,
                                                            UrlEncode(sourceSeqNum), 
                                                            childParcelNum, 
                                                            Page.Request["HideHeader"], 
                                                            UrlConstant.AgencyCode, 
                                                            Page.Request[UrlConstant.AgencyCode]);
                    phParcel.Controls.Add(lnkParcelID);

                    if (parcelIndex < parcelCount)
                    {
                        Literal br = new Literal();
                        br.Text = "<br/>";
                        phParcel.Controls.Add(br);
                    }

                    parcelIndex++;
                }
            }
        }

        /// <summary>
        /// Get parcel id from parameter.
        /// </summary>
        /// <param name="parcelIDAndSeqNums">The parcel id and sequence number.</param>
        /// <returns>The parcel id list.</returns>
        private List<string> GetParcelIDs(List<string> parcelIDAndSeqNums)
        {
            if (parcelIDAndSeqNums == null || parcelIDAndSeqNums.Count < 1)
            {
                return null;
            }

            List<string> parcelIDs = new List<string>();

            foreach (string parcelIDAndSeqNum in parcelIDAndSeqNums)
            {
                string[] parcelArray = parcelIDAndSeqNum.Split(ACAConstant.SPLIT_CHAR1);

                if (parcelArray == null || parcelArray.Length < 3)
                {
                    continue;
                }

                parcelIDs.Add(parcelArray[0]);
            }

            return parcelIDs;
        }

        /// <summary>
        /// Get Genealogy Transaction model list.
        /// </summary>
        /// <returns>Genealogy Transaction model list</returns>
        private List<GenealogyTransactionModel> GetGenealogyTransations()
        {
            IParcelBll parcelBll = (IParcelBll)ObjectFactory.GetObject(typeof(IParcelBll));
            GenealogyTransactionModel[] genealogyTransactionModel = parcelBll.GetParcelGenealogy(ConfigManager.AgencyCode, RefParcelNumber);
            List<GenealogyTransactionModel> genealogyDataSource = new List<GenealogyTransactionModel>();

            if (genealogyTransactionModel != null && genealogyTransactionModel.Length > 0)
            {
                genealogyDataSource = new List<GenealogyTransactionModel>();
                genealogyDataSource.AddRange(genealogyTransactionModel);
            }

            return genealogyDataSource;
        }

        /// <summary>
        /// Bind Genealogy data.
        /// </summary>
        private void BindGenealogyHistory()
        {
            dgvRefGenealogyHistory.DataSource = HistoryGenealogy;
            dgvRefGenealogyHistory.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("aca_genealogyhistory_message_norecord");
            dgvRefGenealogyHistory.DataBind();
        }

        /// <summary>
        /// Bind Genealogy data.
        /// </summary>
        private void BindGenealogyChild()
        {
            dgvRefGenealogyChild.DataSource = ChildGenealogy;
            dgvRefGenealogyChild.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("aca_genealogychild_message_norecord");
            dgvRefGenealogyChild.DataBind();
        }

        /// <summary>
        /// Construct a new DataTable for Address.
        /// </summary>
        /// <param name="genTransModelList">the GenealogyTransaction Model list.</param>
        /// <returns>A DataTable that contains GenealogyTransaction Model list</returns>
        private DataTable ConstructGenealogyTable(List<GenealogyTransactionModel> genTransModelList)
        {
            DataTable refGenealogyTable = new DataTable();
            refGenealogyTable.Columns.Add(ColumnConstant.RefGenealogy.Date.ToString());
            refGenealogyTable.Columns.Add(ColumnConstant.RefGenealogy.Description.ToString());
            refGenealogyTable.Columns.Add(ColumnConstant.RefGenealogy.Children.ToString());
            refGenealogyTable.Columns.Add(ColumnConstant.RefGenealogy.Action.ToString());
            refGenealogyTable.Columns.Add(ColumnConstant.RefGenealogy.Parents.ToString());

            foreach (GenealogyTransactionModel genTransModel in genTransModelList)
            {
                DataRow dr = refGenealogyTable.NewRow();
                dr[ColumnConstant.RefGenealogy.Date.ToString()] = I18nDateTimeUtil.FormatToDateStringForUI(genTransModel.genTranDate);
                dr[ColumnConstant.RefGenealogy.Description.ToString()] = genTransModel.genTranDesc;
                dr[ColumnConstant.RefGenealogy.Children.ToString()] = GetParcelNumber(genTransModel, false);
                dr[ColumnConstant.RefGenealogy.Action.ToString()] = genTransModel.genTranAction;
                dr[ColumnConstant.RefGenealogy.Parents.ToString()] = GetParcelNumber(genTransModel, true);

                refGenealogyTable.Rows.Add(dr);
            }

            return refGenealogyTable;
        }

        /// <summary>
        /// get the parcel number.
        /// </summary>
        /// <param name="genTransModel">the GenealogyTransaction Model</param>
        /// <param name="isParent">is get parent parcel.</param>
        /// <returns>the parcel number</returns>
        private string GetParcelNumber(GenealogyTransactionModel genTransModel, bool isParent)
        {
            StringBuilder parcelNumber = new StringBuilder();

            if (genTransModel != null)
            {
                GenealogyModel[] genModels = isParent ? genTransModel.parentGenealogyModels : genTransModel.childGenealogyModels;

                if (genModels != null && genModels.Length > 0)
                {
                    foreach (GenealogyModel genModel in genModels)
                    {
                        parcelNumber.Append(genModel.objectNbr);
                        parcelNumber.Append(ACAConstant.SPLIT_CHAR1);
                        parcelNumber.Append(genModel.genSource);
                        parcelNumber.Append(ACAConstant.SPLIT_CHAR1);
                        parcelNumber.Append(genModel.genSeqNbr);
                        parcelNumber.Append(ACAConstant.SPLIT_CHAR);
                    }
                }
            }

            if (parcelNumber.Length > 0)
            {
                parcelNumber.Remove(parcelNumber.Length - 1, 1);
            }

            return parcelNumber.ToString();
        }

        /// <summary>
        /// get GenealogyTransaction Model  list by type
        /// </summary>
        /// <param name="parcelGenealogyTransactions">all GenealogyTransaction Model list.</param>
        /// <param name="isHistory">is get history or not.</param>
        /// <returns>GenealogyTransaction Model  list </returns>
        private List<GenealogyTransactionModel> GetGenealogyTranactionsByType(List<GenealogyTransactionModel> parcelGenealogyTransactions, bool isHistory)
        {
            List<GenealogyTransactionModel> genealogyTranactions = new List<GenealogyTransactionModel>();
            List<GenealogyTransactionModel> childGenealogyTransactions = new List<GenealogyTransactionModel>();

            if (parcelGenealogyTransactions != null && parcelGenealogyTransactions.Count > 0)
            {
                GenealogyTransactionModel childGenealogyTransaction = parcelGenealogyTransactions[0];

                if (!IsUseParentParcelByGenTran(childGenealogyTransaction))
                {
                    GenealogyModel[] parentGenealogyTransactions = childGenealogyTransaction.parentGenealogyModels;

                    foreach (GenealogyModel genealogyModel in parentGenealogyTransactions)
                    {
                        if (RefParcelNumber.Equals(genealogyModel.objectNbr, StringComparison.InvariantCultureIgnoreCase))
                        {
                            childGenealogyTransactions.Add(childGenealogyTransaction);
                            break;
                        }
                    }
                }
            }

            if (isHistory)
            {
                genealogyTranactions.AddRange(parcelGenealogyTransactions);

                if (childGenealogyTransactions != null && childGenealogyTransactions.Count > 0)
                {
                    genealogyTranactions.Remove(childGenealogyTransactions[0]);
                }
            }
            else
            {
                genealogyTranactions = childGenealogyTransactions;
            }

            return genealogyTranactions;
        }

        /// <summary>
        /// Is use parent parcel by genealogy transaction model.
        /// </summary>
        /// <param name="parcelGenealogyTransaction">GenTranModel Genealogy Transaction Model</param>
        /// <returns>Is use parent parcel</returns>
        private bool IsUseParentParcelByGenTran(GenealogyTransactionModel parcelGenealogyTransaction)
        {
            bool isUseParent = false;

            if (parcelGenealogyTransaction == null)
            {
                return isUseParent;
            }

            GenealogyModel[] parentGenealogyTransactions = parcelGenealogyTransaction.parentGenealogyModels;
            GenealogyModel[] childGenealogyTransactions = parcelGenealogyTransaction.childGenealogyModels;
            string parentObjectNbr;
            string childObjectNbr;

            foreach (GenealogyModel parentGenealogy in parentGenealogyTransactions)
            {
                parentObjectNbr = parentGenealogy.objectNbr;

                if (RefParcelNumber.Equals(parentObjectNbr, StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach (GenealogyModel childGenealogy in childGenealogyTransactions)
                    {
                        childObjectNbr = childGenealogy.objectNbr;

                        if (parentObjectNbr.Equals(childObjectNbr))
                        {
                            isUseParent = true;
                            break;
                        }
                    }
                }

                if (isUseParent)
                {
                    break;
                }
            }

            return isUseParent;
        }

        #endregion Methods
    }
}