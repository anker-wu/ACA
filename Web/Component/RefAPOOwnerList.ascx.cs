#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefAPOOwnerList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefAPOOwnerList.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation RefAPOOwnerList.
    /// </summary>
    public partial class RefAPOOwnerList : BaseUserControl
    {
        #region Events

        /// <summary>
        /// grid view sorted event.
        /// </summary>
        public event GridViewSortedEventHandler GridViewSort;

        /// <summary>
        /// grid view page index changing event
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        /// <summary>
        /// grid view download event.
        /// </summary>
        public event GridViewDownloadEventHandler GridViewDownloadAll;

        /// <summary>
        /// Event occur after selecting an owner.
        /// </summary>
        public event OwnerListSelectEventHandler OwnerSelected;

        /// <summary>
        /// grid view row command event.
        /// </summary>
        public event CommonEventHandler OwnerRetrieved;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets the page size.
        /// </summary>
        public int PageSize
        {
            get
            {
                return gv.PageSize;
            }
        }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        public DataTable OwnerDataSource
        {
            get
            {
                return ViewState["DataOwnerSrc"] as DataTable;
            }

            set
            {
                ViewState["DataOwnerSrc"] = value;
            }
        }

        /// <summary>
        /// Gets the display count.
        /// </summary>
        public string CountSummary
        {
            get
            {
                return gv.CountSummary;
            }
        }

        /// <summary>
        /// Gets or sets a string to identify the GridView in whole aca system
        /// </summary>
        public string GridViewNumber
        {
            get
            {
                return gv.GridViewNumber;
            }

            set
            {
                gv.GridViewNumber = value;
            }
        }

        /// <summary>
        /// Gets the selected row index.
        /// </summary>
        public int SelectedRowIndex
        {
            get
            {
                List<int> indexes = gv.GetSelectedRowIndexes();
                return indexes.Count == 0 ? -1 : indexes[0];
            }
        }

        /// <summary>
        /// Gets the selected row.
        /// </summary>
        public DataRow SelectedRow
        {
            get
            {
                int dataIndex = SelectedRowIndex;

                return dataIndex == -1 ? null : OwnerDataSource.Rows[SelectedRowIndex];
            }
        }

        /// <summary>
        /// Gets the selected owner key model.
        /// </summary>
        public OwnerModel SelectedOwnerKey
        {
            get
            {
                return GetOwnerKey(SelectedRow);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether allow to select by Radio Button.
        /// </summary>
        public bool AllowSelectingByRadioButton
        {
            get
            {
                return gv.AutoGenerateRadioButtonColumn;
            }

            set
            {
                gv.AutoGenerateRadioButtonColumn = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether select address/parcel associated owner to view owner detail or not.
        /// </summary>
        public bool IsRedirectFromSearchPage
        {
            get
            {
                if (ViewState["IsRedirectFromSearchPage"] == null)
                {
                    return false;
                }

                return (bool)ViewState["IsRedirectFromSearchPage"];
            }

            set
            {
                ViewState["IsRedirectFromSearchPage"] = value;
            }
        }

        /// <summary>
        /// Gets or sets command type.
        /// </summary>
        public OwnerListRowCommandType RowCommandType
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the text to display in the empty data row rendered when a System.Web.UI.WebControls.GridView
        /// control is bound to a data source that does not contain any records.
        /// </summary>
        public string EmptyDataText
        {
            get
            {
                return gv.EmptyDataText;
            }

            set
            {
                gv.EmptyDataText = value;
            }
        }

        /// <summary>
        /// Gets or sets page index.
        /// </summary>
        public int PageIndex
        {
            get
            {
                return gv.PageIndex;
            }

            set
            {
                gv.PageIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show export link.
        /// </summary>
        public bool ShowExportLink
        {
            get
            {
                return gv.ShowExportLink;
            }

            set
            {
                gv.ShowExportLink = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating export GridView's name 
        /// </summary>
        public string ExportFileName
        {
            get
            {
                return gv.ExportFileName;
            }

            set
            {
                gv.ExportFileName = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the grid view is used in SPEAR form
        /// </summary>
        public bool IsInSPEARForm
        {
            get
            {
                return gv.IsInSPEARForm;
            }

            set
            {
                gv.IsInSPEARForm = value;
            }
        }

        #endregion Properties     

        #region Public Methods

        /// <summary>
        /// bind reference address data source
        /// </summary>
        /// <param name="dt">data table.</param>
        public void BindDataSource(DataTable dt)
        {
            BindDataSource(dt, false);
        }

        /// <summary>
        /// Bind Data Source
        /// </summary>
        /// <param name="dt">Data table.</param>
        /// <param name="resetPageIndex">reset page index flag</param>
        /// <param name="pageIndex">GridView page index</param>
        /// <param name="pageSort">GridView sort</param>
        public void BindDataSource(DataTable dt, bool resetPageIndex, int pageIndex = 0, string pageSort = null)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                gv.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
                gv.DataSource = null;
                gv.DataBind();

                return;
            }

            OwnerDataSource = dt;
            DataView dv = new DataView(dt);
            string gridViewSort = GetGridViewSort();

            if (!string.IsNullOrEmpty(pageSort))
            {
                dv.Sort = pageSort;
            }
            else if (!string.IsNullOrEmpty(gv.GridViewSortExpression))
            {
                dv.Sort = gv.GridViewSortExpression + " " + gv.GridViewSortDirection;
                OwnerDataSource = dv.ToTable();
            }
            else if (!string.IsNullOrEmpty(gridViewSort))
            {
                dv.Sort = gridViewSort;
            }

            if (resetPageIndex)
            {
                gv.PageIndex = 0;
            }
            else if (pageIndex != 0)
            {
                gv.PageIndex = pageIndex;
            }

            gv.Visible = true;
            gv.DataSource = OwnerDataSource;
            gv.DataBind();
        }

        /// <summary>
        /// Select the specified row
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <returns>true or false, indicating whether success to select the specified row.</returns>
        public bool SelectRow(int rowIndex)
        {
            gv.SelectRow(gv.Rows[rowIndex]);

            return true;
        }

        /// <summary>
        /// Clear selected items.
        /// </summary>
        public void ClearSelectedItems()
        {
            gv.IsClearSelectedItems = true;
        }

        /// <summary>
        /// Get duplicated APO keys for owner.
        /// </summary>
        /// <param name="ownerModel">An OwnerModel will be updated</param>
        public void GetDuplicatedAPOKeys(OwnerModel ownerModel)
        {
            if (ownerModel == null)
            {
                return;
            }

            DataRow dr = SelectedRow;

            if (dr != null && dr["OwnerAPOKeys"] != DBNull.Value && !string.IsNullOrEmpty(dr["OwnerAPOKeys"].ToString()))
            {
                ownerModel.duplicatedAPOKeys = dr["OwnerAPOKeys"] as DuplicatedAPOKeyModel[];
            }
        }

        /// <summary>
        /// Retrieve associated records for the selected owner record.
        /// </summary>
        /// <param name="ownerRow">Owner data row</param>
        public void RetrieveAssociatedData(DataRow ownerRow)
        {
            OwnerRetrieved(null, new CommonEventArgs(ownerRow));
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Raises the <c>System.Web.UI.Control.Init event</c>.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gv, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Grid row data bound.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void RefOwnerList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkName = (LinkButton)e.Row.FindControl("lnkName");

                if (AllowSelectingByRadioButton)
                {
                    lnkName.Visible = false;

                    AccelaLabel lblName = (AccelaLabel)e.Row.FindControl("lblName");
                    lblName.Visible = true;
                }
                else
                {
                    lnkName.CommandName = RowCommandType.ToString();
                }
            }
        }

        /// <summary>
        /// GV RowCommand
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void RefOwnerList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Bind owner data source.
            BindDataSource(OwnerDataSource);

            if (e.CommandName == "Header")
            {
                gv.GridViewSortExpression = e.CommandArgument.ToString();
            }
            else if (e.CommandName == "RetrieveData" || e.CommandName == APOShowType.ShowOwner.ToString())
            {
                int index = int.Parse(e.CommandArgument.ToString());
                GridViewRow row = gv.Rows[index];
                DataRow ownerRow = ((DataTable)gv.DataSource).Rows[row.DataItemIndex];

                //Update the selected pageIndex and sortExpression to session.
                UpdateAPOQueryInfo();

                if (e.CommandName == "RetrieveData")
                {
                    OwnerRetrieved(sender, new CommonEventArgs(ownerRow));
                }
                else
                {
                    RedirectToOwnerDetail(ownerRow);
                }
            }
            else if (e.CommandName == OwnerListRowCommandType.SelectOwner.ToString())
            {
                if (OwnerSelected != null)
                {
                    int rowIndex = e.GetRow().DataItemIndex;
                    OwnerListSelectEventArgs selectEventArgs = new OwnerListSelectEventArgs();
                    selectEventArgs.SelectedOwner = APOUtil.GetOwnerModel(OwnerDataSource.Rows[rowIndex]);
                    OwnerSelected(this, selectEventArgs);
                }
            }
        }

        /// <summary>
        /// fire page index changing event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RefOwnerList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Bind owner data source.
            BindDataSource(OwnerDataSource);

            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// fire sorted event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RefOwnerList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            //Bind owner data source.
            BindDataSource(OwnerDataSource);

            if (GridViewSort != null)
            {
                GridViewSort(sender, e);
            }
        }

        /// <summary>
        /// RefOwnerList download event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A GridViewDownloadEventArgs object containing the event data.</param>
        protected void RefOwnerList_GridViewDownload(object sender, GridViewDownloadEventArgs e)
        {
            //Bind owner data source.
            BindDataSource(OwnerDataSource);

            // execute the download event that setting the exported content.
            if (GridViewDownloadAll != null)
            {
                GridViewDownloadAll(sender, e);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Gets the owner's keys from data row.
        /// </summary>
        /// <param name="dataRow">A data row</param>
        /// <returns>An <see cref="OwnerModel"/></returns>
        private OwnerModel GetOwnerKey(DataRow dataRow)
        {
            if (dataRow == null)
            {
                return null;
            }

            OwnerModel owner = new OwnerModel();
            owner.ownerNumber = StringUtil.ToLong(Convert.ToString(dataRow["OwnerNumber"]));
            owner.UID = Convert.ToString(dataRow["OwnerUID"]);

            return owner;
        }

        /// <summary>
        /// Updates the APO query info.
        /// </summary>
        private void UpdateAPOQueryInfo()
        {
            ApoQueryInfo apoQueryInfo = (ApoQueryInfo)Session[SessionConstant.SESSION_APO_QUERY];

            if (apoQueryInfo != null)
            {
                apoQueryInfo.IsFromSearchPage = true;
                apoQueryInfo.APOOwnerPageIndex = gv.PageIndex;

                if (!string.IsNullOrEmpty(gv.GridViewSortExpression))
                {
                    apoQueryInfo.APOOwnerPageSort = gv.GridViewSortExpression + " " + gv.GridViewSortDirection;
                }

                Session[SessionConstant.SESSION_APO_QUERY] = apoQueryInfo;
            }
        }

        /// <summary>
        /// Redirect to Owner Detail page.
        /// </summary>
        /// <param name="ownerRow">Owner row</param>
        private void RedirectToOwnerDetail(DataRow ownerRow)
        {
            string ownerNum = ownerRow["OwnerNumber"] != null ? ownerRow["OwnerNumber"].ToString() : string.Empty;
            string ownerSeq = ownerRow["OwnerSequenceNumber"] != null ? ownerRow["OwnerSequenceNumber"].ToString() : string.Empty;
            string ownerUID = ownerRow["OwnerUID"] != null ? ownerRow["OwnerUID"].ToString() : string.Empty;

            if (IsRedirectFromSearchPage)
            {
                ApoQueryInfo apoQueryInfo = (ApoQueryInfo)Session[SessionConstant.SESSION_APO_QUERY];

                if (apoQueryInfo != null)
                {
                    apoQueryInfo.IsFromSearchPage = true;
                    Session[SessionConstant.SESSION_APO_QUERY] = apoQueryInfo;
                }

                //Reset IsRedirectFromSearchPage flag.
                IsRedirectFromSearchPage = false;
            }

            string url = string.Format(
                                        "../APO/OwnerDetail.aspx?{0}={1}&{2}={3}&{4}={5}&{6}={7}&{8}={9}&{10}={11}&{12}={13}&HideHeader={14}",
                                        ACAConstant.REQUEST_PARMETER_OWNER_NUMBER,
                                        UrlEncode(ownerNum),
                                        ACAConstant.REQUEST_PARMETER_OWNER_SEQUENCE,
                                        UrlEncode(ownerSeq),
                                        ACAConstant.REQUEST_PARMETER_OWNER_UID,
                                        UrlEncode(ownerUID),
                                        ACAConstant.REQUEST_PARMETER_REFADDRESS_ID,
                                        Request.Params[ACAConstant.REQUEST_PARMETER_REFADDRESS_ID],
                                        ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE,
                                        Request.Params[ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE],
                                        ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER,
                                        Request.Params[ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER],
                                        ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE,
                                        Request.Params[ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE],
                                        Page.Request.QueryString["HideHeader"]);
            Response.Redirect(url);
        }

        /// <summary>
        /// Gets the grid view sort.
        /// </summary>
        /// <returns>The grid view sort.</returns>
        private string GetGridViewSort()
        {
            ApoQueryInfo apoQueryInfo = (ApoQueryInfo)Session[SessionConstant.SESSION_APO_QUERY];

            if (apoQueryInfo != null && !string.IsNullOrEmpty(apoQueryInfo.APOOwnerPageSort))
            {
                return apoQueryInfo.APOOwnerPageSort;
            }

            return string.Empty;
        }

        #endregion Private Methods
    }
}