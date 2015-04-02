#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GeneralPopupSelect.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: GeneralPopupSelect.aspx.cs 189386 2011-01-24 01:51:47Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.Web.Controls;

namespace Accela.ACA.Web.GeneralProperty
{
    /// <summary>
    /// The popup select page.
    /// </summary>
    public partial class GeneralPopupSelect : PopupDialogBasePage
    {
        #region Fields

        /// <summary>
        /// The url parameter for select location.
        /// </summary>
        protected const string URL_SELECT_PAGE_LOCATION = "location";

        /// <summary>
        /// The url parameter for select zip code.
        /// </summary>
        protected const string URL_SELECT_PAGE_ZIPCODE = "zipcode";

        /// <summary>
        /// The url parameters constant.
        /// </summary>
        private const string URL_SELECT_PAGE = "select";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the first step's grid view data source
        /// </summary>
        public DataTable GridViewDataSource
        {
            get
            {
                if (ViewState["GridViewDataSource"] != null)
                {
                    return (DataTable)ViewState["GridViewDataSource"];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                ViewState["GridViewDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected page value
        /// </summary>
        protected string SelectPageValue
        {
            get;

            set;
        }

        #endregion Properties

        #region Method

        /// <summary>
        /// Raises the page load event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SelectPageValue = Request[URL_SELECT_PAGE];

            SetPageTitleAndHeaderStyle();

            BindGridView();
        }

        /// <summary>
        /// Handles the RowDataBound event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void PopupList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ListItemType itemType = (ListItemType)e.Row.RowType;

            switch (itemType)
            {
                case ListItemType.Header:
                    CheckBox chkAll = e.Row.FindControl("chkAll") as CheckBox;
                    
                    if (chkAll != null)
                    {
                        chkAll.Visible = true;
                        chkAll.InputAttributes.Add("title", GetTextByKey("aca_selectallrecords_checkbox"));
                    }
                    
                    SetGridViewColumnHeaderLabel(e);
                    break;
                case ListItemType.Item:
                    // set the checkbox status.
                    CheckBox cb = e.Row.FindControl("chk") as CheckBox;
                    AccelaLabel lblKey = e.Row.FindControl("lblKey") as AccelaLabel;

                    if (cb != null && lblKey != null)
                    {
                        cb.InputAttributes.Add("value", lblKey.Text);
                        cb.InputAttributes.Add("title", GetTextByKey("aca_selectonerecord_checkbox"));

                        string selectedValue = hdSelectedValue.Value;

                        if (!string.IsNullOrEmpty(selectedValue) && selectedValue.IndexOf(ACAConstant.COMMA + lblKey.Text + ACAConstant.COMMA) != -1)
                        {
                            cb.Checked = true;
                        }
                    }

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Set the page title, instruction and grid view header style.
        /// </summary>
        private void SetPageTitleAndHeaderStyle()
        {
            switch (SelectPageValue)
            {
                case URL_SELECT_PAGE_LOCATION:
                    SetPageTitleKey("aca_certbusiness_label_popuplocation_title");
                    lblPageInstruction.LabelKey = "aca_certbusiness_label_popuplocation_instruction";

                    gvPopupList.Columns[1].HeaderStyle.Width = Unit.Pixel(340);
                    gvPopupList.Columns[2].Visible = false;

                    break;
                case URL_SELECT_PAGE_ZIPCODE:
                    SetPageTitleKey("aca_certbusiness_label_popupzipcode_title");
                    lblPageInstruction.LabelKey = "aca_certbusiness_label_popupzipcode_instruction";

                    gvPopupList.Columns[1].HeaderStyle.Width = Unit.Pixel(140);
                    gvPopupList.Columns[2].HeaderStyle.Width = Unit.Pixel(200);

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Set the grid view columns header's label.
        /// </summary>
        /// <param name="e">The GridViewRowEventArgs.</param>
        private void SetGridViewColumnHeaderLabel(GridViewRowEventArgs e)
        {
            GridViewHeaderLabel lblKey = e.Row.FindControl("lblKeyHeader") as GridViewHeaderLabel;
            GridViewHeaderLabel lblValue = e.Row.FindControl("lblValueHeader") as GridViewHeaderLabel;

            switch (SelectPageValue)
            {
                case URL_SELECT_PAGE_LOCATION:
                    if (lblKey != null)
                    {
                        lblKey.LabelKey = "aca_certbusiness_label_popuplocationlist_location";
                    }

                    break;
                case URL_SELECT_PAGE_ZIPCODE:
                    if (lblKey != null)
                    {
                        lblKey.LabelKey = "aca_certbusiness_label_popupzipcodelist_zipcode";
                    }

                    if (lblValue != null)
                    {
                        lblValue.LabelKey = "aca_certbusiness_label_popupzipcodelist_description";
                    }

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Get the page data source.
        /// </summary>
        /// <returns>Return the page data source.</returns>
        private DataTable GetPageDataSource()
        {
            DataTable dtDataSource = new DataTable();

            switch (SelectPageValue)
            {
                case URL_SELECT_PAGE_LOCATION:
                    dtDataSource = GetStandardChoiceDataSource(BizDomainConstant.STD_CERT_BUSINESS_LOCATION);

                    break;
                case URL_SELECT_PAGE_ZIPCODE:
                    dtDataSource = GetStandardChoiceDataSource(BizDomainConstant.STD_CERT_BUSINESS_ZIP_CODE);

                    break;
                default:
                    break;
            }

            return dtDataSource;
        }

        /// <summary>
        /// Bind the grid view.
        /// </summary>
        private void BindGridView()
        {
            DataTable dtDataSource = null;

            if (GridViewDataSource == null)
            {
                dtDataSource = GetPageDataSource();
            }
            else
            {
                dtDataSource = GridViewDataSource;
            }
     
            if (!IsPostBack)
            {
                // sort the data source defaultly by ascending.
                gvPopupList.GridViewSortExpression = "Key";
                gvPopupList.GridViewSortDirection = ACAConstant.ORDER_BY_ASC;
                dtDataSource.DefaultView.Sort = "Key ASC";
            }

            // set the data source and bind the grid view
            GridViewDataSource = dtDataSource;

            // bind the grid datasource
            gvPopupList.DataSource = dtDataSource;
            gvPopupList.EmptyDataText = GetTextByKey("per_permitList_messagel_noRecord");
            gvPopupList.DataBind();
            gvPopupList.Attributes.Add("PageCount", gvPopupList.PageCount.ToString());
        }

        /// <summary>
        /// Get the data source in standard choice.
        /// </summary>
        /// <param name="standardChoice">The standard choice item.</param>
        /// <returns>Return the data source.</returns>
        private DataTable GetStandardChoiceDataSource(string standardChoice)
        {
            DataTable dtDataSource = new DataTable();
            dtDataSource.Columns.Add("Key", typeof(string));
            dtDataSource.Columns.Add("Value", typeof(string));

            IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> stdItems = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, standardChoice, false);

            foreach (ItemValue item in stdItems)
            {
                DataRow dr = dtDataSource.NewRow();

                dr["Key"] = item.Key;
                dr["Value"] = item.Value;

                dtDataSource.Rows.Add(dr);
            }

            return dtDataSource;
        }

        #endregion Method
    }
}