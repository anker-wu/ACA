#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefLicenseList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefLicenseList.ascx.cs 257891 2013-09-28 09:56:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// RefLicenseList Control.
    /// </summary>
    public partial class RefLicenseList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// A value indicating whether the form be used in Multiple LP edit form.
        /// </summary>
        private bool _isMultipleLicensedProfessional;

        #endregion Fields

        /// <summary>
        /// grid view page index changing event.
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        /// <summary>
        /// grid view sorted event.
        /// </summary>
        public event GridViewSortedEventHandler GridViewSort;

        /// <summary>
        /// Gets a value indicating whether checked item or not.
        /// </summary>
        public bool HasCheckedItems
        {
            get
            {
                return SelectedRowIndex >= 0;
            }
        }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        public DataTable GridViewDataSource
        {
            get
            {
                return (DataTable)ViewState["LicensePros"];
            }

            set
            {
                ViewState["LicensePros"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the smart choice validate flag for license
        /// </summary>
        public bool IsValidate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets reference license list page size
        /// </summary>
        public int PageSize
        {
            get { return gdvLicenseList.PageSize; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the form be used in Multiple LP edit form.
        /// </summary>
        public bool IsMultipleLicensedProfessional
        {
            get
            {
                return _isMultipleLicensedProfessional;
            }

            set
            {
                _isMultipleLicensedProfessional = value;

                if (value)
                {
                    gdvLicenseList.AutoGenerateCheckBoxColumn = true;
                }
                else
                {
                    gdvLicenseList.AutoGenerateRadioButtonColumn = true;
                }
            }
        }

        /// <summary>
        /// Gets the selected row Index
        /// </summary>
        public int SelectedRowIndex
        {
            get
            {
                List<int> indexes = SelectedRowIndexes;
                return (indexes == null || indexes.Count == 0) ? -1 : indexes[0];
            }
        }

        /// <summary>
        /// Gets the selected row Index
        /// </summary>
        public List<int> SelectedRowIndexes
        {
            get
            {
                return gdvLicenseList.GetSelectedRowIndexes();
            }
        }

        /// <summary>
        /// Gets the selected item
        /// </summary>
        public LicenseModel4WS[] SelectedItems
        {
            get
            {
                if (SelectedRowIndexes == null || SelectedRowIndexes.Count == 0 || GridViewDataSource == null)
                {
                    return null;
                }

                IList<LicenseModel4WS> selectedLicenses = new List<LicenseModel4WS>();
                DataTable datasource = gdvLicenseList.GetSelectedData(GridViewDataSource);

                if (datasource != null && datasource.Rows.Count > 0)
                {
                    foreach (DataRow row in datasource.Rows)
                    {
                        LicenseModel4WS model = (LicenseModel4WS)row["LicenseModel"];

                        if (model != null)
                        {
                            selectedLicenses.Add(model);
                        }
                    }
                }

                return selectedLicenses.ToArray();
            }
        }

        /// <summary>
        /// Gets the selected item
        /// </summary>
        public LicenseModel4WS SelectedItem
        {
            get
            {
                if (SelectedRowIndex >= 0)
                {
                    return (LicenseModel4WS)GridViewDataSource.Rows[SelectedRowIndex]["LicenseModel"];
                }

                return null;
            }
        }

        /// <summary>
        /// Bind license list
        /// </summary>
        public void BindLicenseList()
        {
            gdvLicenseList.DataSource = GridViewBuildHelper.MergeTemplateAttributes2DataTable(gdvLicenseList, GridViewDataSource);
            gdvLicenseList.DataBind();
        }

        /// <summary>
        /// reset grid view.
        /// </summary>
        public void ResetGridView()
        {
            GridViewDataSource = null;
            gdvLicenseList.PageIndex = 0;
            gdvLicenseList.IsClearSelectedItems = true;
        }

        /// <summary>
        /// Generate No Search Result Message.
        /// </summary>
        public void GenerateNoSearchResultMessage()
        {
            gdvLicenseList.EmptyDataText = IsValidate ? GetTextByKey("per_licensePro_label_NoLicenseProFound") : GetTextByKey("per_licensePro_label_ManuallyEnterLicensePro");
            BindLicenseList();
        }

        /// <summary>
        /// On initial event method.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.InitializeGridWithTemplate(gdvLicenseList, ModuleName, BizDomainConstant.STD_CAT_LICENSE_TYPE);
            base.OnInit(e);
        }

        /// <summary>
        /// GridView LicenseList RowCommand
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LicenseList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            BindLicenseList();
        }

        /// <summary>
        /// GridView LicenseList RowDataBound.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LicenseList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (IsMultipleLicensedProfessional)
                {
                    // When there is only one row and no select, need select the row.
                    if (GridViewDataSource.Rows.Count == 1)
                    {
                        gdvLicenseList.SelectRow(e.Row);
                    }
                }
            }
        }

        /// <summary>
        /// Response permit GridView page index changing event to record the latest page index.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void LicenseList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);   
            }
        }

        /// <summary>
        /// Response permit GridView sorted event to record the latest sort expression.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void LicenseList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            if (GridViewSort != null)
            {
                GridViewSort(sender, e);
            }

            GridViewDataSource = GridViewBuildHelper.MergeTemplateAttributes2DataTable(gdvLicenseList, GridViewDataSource);
            GridViewDataSource.DefaultView.Sort = e.GridViewSortExpression;
            GridViewDataSource = GridViewDataSource.DefaultView.ToTable();
        }
    }
}