#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: MyCollectionList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  MyCollectionList user control.
 *
 *  Notes:
 * $Id: MyCollectionList.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.MyCollection;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation MyCollectionList.
    /// </summary>
    public partial class MyCollectionList : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private DataTable GridViewDataSource
        {
            get
            {
                return (DataTable)ViewState["MyCollectionModels"];
            }

            set
            {
                ViewState["MyCollectionModels"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Binding my collections.
        /// </summary>
        /// <param name="myCollections">MyCollectionModel4WS array</param>
        public void BindMyCollectionList(MyCollectionModel[] myCollections)
        {
            GridViewDataSource = CreateDataSource(myCollections);

            gdvMyCollectionList.DataSource = GridViewDataSource;
            gdvMyCollectionList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvMyCollectionList.DataBind();
        }

        /// <summary>
        /// Row command.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void MyCollectionList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            gdvMyCollectionList.DataSource = GridViewDataSource;
            gdvMyCollectionList.EmptyDataText = GetTextByKey("per_permitList_messagel_noRecord");
            gdvMyCollectionList.DataBind();
        }

        /// <summary>
        /// Data row binding.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void MyCollectionList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
                string deleteMessage = GetTextByKey("mycollection_message_delete").Replace("'", "\\'");
                lnkDelete.Attributes.Add("onclick", string.Format("javascript:return confirm('{0}')", deleteMessage));

                string collectionId = DataBinder.Eval(e.Row.DataItem, "CollectionId").ToString();

                HyperLink hlMyCollectionName = (HyperLink)e.Row.FindControl("hlCollectionName");
                hlMyCollectionName.Style.Add("cursor", "pointer");
                hlMyCollectionName.NavigateUrl = string.Format("../MyCollection/MyCollectionDetail.aspx?collectionId={0}", collectionId);
            }
        }

        /// <summary>
        /// Delete row data.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void MyCollectionList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            IMyCollectionBll myCollectionBll = (IMyCollectionBll)ObjectFactory.GetObject(typeof(IMyCollectionBll));
            string collectionID = this.gdvMyCollectionList.DataKeys[e.RowIndex].Value.ToString();
            myCollectionBll.DeleteMyCollection(ConfigManager.SuperAgencyCode, collectionID, AppSession.User.PublicUserId);

            //Re-binding my collections.
            MyCollectionModel[] myCollections = myCollectionBll.GetCollections4Management(ConfigManager.SuperAgencyCode, AppSession.User.PublicUserId);

            //Add my collection models to session.
            AppSession.SetMyCollectionsToSession(myCollections);

            Response.Redirect("MyCollectionManagement.aspx");
        }

        /// <summary>
        /// Raises the <c>System.Web.UI.Control.Init</c> event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvMyCollectionList, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Construct data source for my collection.
        /// </summary>
        /// <returns>Data Table for my collection.</returns>
        private static DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("CollectionId", typeof(string)));
            dt.Columns.Add(new DataColumn("CollectionName", typeof(string)));
            dt.Columns.Add(new DataColumn("CollectionDesc", typeof(string)));
            dt.Columns.Add(new DataColumn("UpdateDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("RecordsAmount", typeof(string)));

            return dt;
        }

        /// <summary>
        /// Create my collections data source.
        /// </summary>
        /// <param name="myCollections">MyCollectionModel4WS array.</param>
        /// <returns>DataTable for my collection.</returns>
        private DataTable CreateDataSource(MyCollectionModel[] myCollections)
        {
            DataTable dt = CreateTable();

            if (myCollections == null || myCollections.Length == 0)
            {
                return dt;
            }

            string collectionName = string.Empty;
            string collectionDescription = string.Empty;

            foreach (MyCollectionModel myCollectionModel in myCollections)
            {
                collectionName = myCollectionModel.collectionName != null ? myCollectionModel.collectionName : string.Empty;
                collectionDescription = myCollectionModel.collectionDescription != null ? myCollectionModel.collectionDescription : string.Empty;

                DataRow dr = dt.NewRow();
                dr["UpdateDate"] = myCollectionModel.auditDate == null ? DBNull.Value : (object)myCollectionModel.auditDate;
                dr["CollectionId"] = myCollectionModel.collectionId;
                dr["CollectionName"] = collectionName;
                dr["CollectionDesc"] = collectionDescription;
                dr["RecordsAmount"] = myCollectionModel.capAmount;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        #endregion Methods
    }
}