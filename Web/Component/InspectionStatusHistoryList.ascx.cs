#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: InspectionStatusHistoryList.cs
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
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Inspection;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// display inspection status history
    /// </summary>
    public partial class InspectionStatusHistoryList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// ellipsis script list
        /// </summary>
        private List<string> _ellipsisScriptList = new List<string>();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets Inspection StatusHistory Collection
        /// </summary>
        public IList<InspectionViewModel> DataSource
        {
            get
            {
                if (ViewState["InspectionStatusHistoryDataSource"] == null)
                {
                    ViewState["InspectionStatusHistoryDataSource"] = new List<InspectionViewModel>();
                }

                return (List<InspectionViewModel>)ViewState["InspectionStatusHistoryDataSource"];
            }

            set
            {
                ViewState["InspectionStatusHistoryDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the GView ID.
        /// </summary>
        /// <value>The GView ID.</value>
        public string GViewID
        {
            get
            {
                return gdvInspectionStatusHistoryList.GridViewNumber;
            }

            set
            {
                gdvInspectionStatusHistoryList.GridViewNumber = value;
            }
        }

        #endregion Properties

        /// <summary>
        /// bind data list for related inspection.
        /// </summary>
        public void Bind()
        {
            gdvInspectionStatusHistoryList.DataSource = DataSource;
            gdvInspectionStatusHistoryList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("ins_inspectionList_message_noRecord");
            gdvInspectionStatusHistoryList.DataBind();
        }

        /// <summary>
        /// <c>OnInit</c> event method
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvInspectionStatusHistoryList, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }
    }
}