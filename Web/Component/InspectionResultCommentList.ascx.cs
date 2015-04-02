#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: InspectionResultCommentList.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 *
 * </pre>
 */
#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Inspection;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// display inspection result comment
    /// </summary>
    public partial class InspectionResultCommentList : BaseUserControl
    {
        #region Fields

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets Inspection ResultComment Collection
        /// </summary>
        public IList<InspectionViewModel> DataSource
        {
            get
            {
                if (ViewState["InspectionResultCommentDataSource"] == null)
                {
                    ViewState["InspectionResultCommentDataSource"] = new List<InspectionViewModel>();
                }

                return (List<InspectionViewModel>)ViewState["InspectionResultCommentDataSource"];
            }

            set
            {
                ViewState["InspectionResultCommentDataSource"] = value;
            }
        }

        #endregion Properties

        /// <summary>
        /// bind data list for related inspection.
        /// </summary>
        public void Bind()
        {
            gdvInspectionResultCommentList.DataSource = DataSource;
            gdvInspectionResultCommentList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("ins_inspectionList_message_noRecord");
            gdvInspectionResultCommentList.DataBind();
        }

        /// <summary>
        /// <c>OnInit</c> event method
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvInspectionResultCommentList, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }
        
        /// <summary>
        /// GridView InspectionResultCommentList row data bound event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void InspectionResultCommentList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AccelaLabel lblUpdatedDateTime = (AccelaLabel)e.Row.FindControl("lblUpdatedDateTime");

                if (lblUpdatedDateTime != null && !string.IsNullOrEmpty(lblUpdatedDateTime.Text))
                {
                    lblUpdatedDateTime.Text = string.Format("({0})", lblUpdatedDateTime.Text);
                }
            }
        }
    }
}