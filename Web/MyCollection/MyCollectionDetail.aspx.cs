#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: MyCollectionDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  MyCollectionDetail page
 *
 *  Notes:
 * $Id: MyCollectionDetail.aspx.cs 278363 2014-09-02 08:52:32Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Accela.ACA.BLL;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.MyCollection;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Component;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.MyCollection
{
    /// <summary>
    /// the class for my collection detail.
    /// </summary>
    public partial class MyCollectionDetail : BasePage, IReportVariable
    {
        #region Properties

        /// <summary>
        /// Gets the selected caps' alt ids.
        /// </summary>
        string[] IReportVariable.CapAltIDs
        {
            get
            {
                IList<string> selectedAltIDList = new List<string>();

                foreach (DataListItem item in this.dlCAPsGroupByModule.Items)
                {
                    MyCollectionCAPs myCapList = item.FindControl("CAPsForMyCollection") as MyCollectionCAPs;

                    string[] selectedAltIDs = myCapList.SelectedAltIDs;

                    if (selectedAltIDs == null || selectedAltIDs.Length == 0)
                    {
                        continue;
                    }

                    //Add each cap list's selected cap id to the list.
                    foreach (string s in selectedAltIDs)
                    {
                        if (!selectedAltIDList.Contains(s))
                        {
                            selectedAltIDList.Add(s);
                        }
                    }
                }

                string[] altIDs = null;

                if (selectedAltIDList != null && selectedAltIDList.Count > 0)
                {
                    altIDs = new string[selectedAltIDList.Count];
                    selectedAltIDList.CopyTo(altIDs, 0);
                }

                return altIDs;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// page load.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["collectionId"] != null)
                {
                    string collectionId = Request.QueryString["collectionId"].ToString();
                    IMyCollectionBll myCollectionBll = (IMyCollectionBll)ObjectFactory.GetObject(typeof(IMyCollectionBll));
                    MyCollectionModel myCollectionModel = myCollectionBll.GetCollectionDetailInfo(ConfigManager.SuperAgencyCode, AppSession.User.PublicUserId, collectionId, null);

                    //Set value to  collectionId property of SummaryInformation control.
                    SummaryInformation.CollectionId = collectionId;

                    //key is ModuleName; value is SimpleCapModel list.
                    Hashtable htSimpleCapModel = GetSimpleCapModelListByModuleName(myCollectionModel.simpleCapModels);

                    //Initialization my collection summary information.
                    BuildSummaryInformation(myCollectionModel.collectionName, myCollectionModel.collectionDescription, htSimpleCapModel, myCollectionModel.myCollectionSummaryModel);

                    if (htSimpleCapModel != null)
                    {
                        //Construct data structure for my collection by mudule name.
                        DataTable dtCapList = new DataTable();
                        dtCapList.Columns.Add(new DataColumn("MyCollectionId", typeof(long)));
                        dtCapList.Columns.Add(new DataColumn("ModuleName", typeof(string)));
                        dtCapList.Columns.Add(new DataColumn("SimpleCapModelList", typeof(ArrayList)));
                        ConstructCapListData(htSimpleCapModel, dtCapList, collectionId);

                        //Binding CAPsGroupByModule datalist.
                        DataView dataView = new DataView(dtCapList);
                        dataView.Sort = "ModuleName";
                        dtCapList = dataView.ToTable();
                        dlCAPsGroupByModule.DataSource = dtCapList;
                        dlCAPsGroupByModule.DataBind();
                    }
                }
            }
        }

        /// <summary>
        /// item data bound.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">DataListItemEventArgs e</param>
        protected void CAPsGroupByModule_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                MyCollectionCAPs myCollectionCAPs = e.Item.FindControl("CAPsForMyCollection") as MyCollectionCAPs;
                ArrayList simpleCapModelList = (ArrayList)drv["SimpleCapModelList"];
                DataTable dtCapList = capList.CreateDataSource((SimpleCapModel[])simpleCapModelList.ToArray(typeof(SimpleCapModel)));
                string displayModuleName = LabelUtil.GetI18NModuleName(drv["ModuleName"].ToString());

                if (myCollectionCAPs != null)
                {
                    myCollectionCAPs.BindCapList(Convert.ToInt64(drv["MyCollectionId"]), drv["ModuleName"].ToString(), displayModuleName, dtCapList, 0, "Date DESC");
                }
            }
        }

        /// <summary>
        /// Initialize my collection summary information.
        /// </summary>
        /// <param name="collectionName">collection name</param>
        /// <param name="collectionDesc">collection description</param>
        /// <param name="htSimpleCapModel">simple CapModel</param>
        /// <param name="summaryModel">summary CapModel</param>
        private void BuildSummaryInformation(string collectionName, string collectionDesc, Hashtable htSimpleCapModel, MyCollectionSummaryModel summaryModel)
        {
            SummaryInformation.InitializationSummaryInfo(collectionName, collectionDesc, htSimpleCapModel, summaryModel);
        }

        /// <summary>
        /// Construct CAP list data for my collection by module name.
        /// </summary>
        /// <param name="htSimpleCapModel">Simple CapModel</param>
        /// <param name="dtCapList"> data table for CapList</param>
        /// <param name="collectionId"> collection Id.</param>
        private void ConstructCapListData(Hashtable htSimpleCapModel, DataTable dtCapList, string collectionId)
        {
            foreach (DictionaryEntry simpleCapModel in htSimpleCapModel)
            {
                string moduleName = simpleCapModel.Key.ToString();
                ArrayList simpleCapModelList = (ArrayList)simpleCapModel.Value;

                //MyCollectionCAPs cl = Page.FindControl("CAPsForMyCollection") as MyCollectionCAPs;
                dtCapList.Rows.Add(collectionId, moduleName, simpleCapModelList);
            }
        }

        /// <summary>
        /// Get simpleCapModel list and set it to hashtable.
        /// </summary>
        /// <param name="simpleCapModels">Simple Cap Model List</param>
        /// <returns>hash table for SimpleCapModelList</returns>
        private Hashtable GetSimpleCapModelListByModuleName(SimpleCapModel[] simpleCapModels)
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

        #endregion Methods
    }
}