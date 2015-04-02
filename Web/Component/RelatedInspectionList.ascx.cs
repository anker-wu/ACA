#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RelatedInspectionList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RelatedInspectionList.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Inspection;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// display related inspection
    /// </summary>
    public partial class RelatedInspectionList : BaseUserControl
    {
        #region Fields

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the service provider code
        /// </summary>
        public string ServiceProviderCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets cap model.
        /// </summary>
        /// <value>The cap model.</value>
        public CapModel4WS Cap
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets user model.
        /// </summary>
        /// <value>The current user.</value>
        public User CurrentUser
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is admin.
        /// </summary>
        /// <value><c>true</c> if this instance is admin; otherwise, <c>false</c>.</value>
        public bool IsAdmin
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Inspection TreeNode Collection
        /// </summary>
        public InspectionTreeNodeModel DataSource
        {
            get
            {
                if (ViewState["InspectionTreeNodeDataSource"] == null)
                {
                    ViewState["InspectionTreeNodeDataSource"] = new InspectionTreeNodeModel();
                }

                return (InspectionTreeNodeModel)ViewState["InspectionTreeNodeDataSource"];
            }

            set
            {
                ViewState["InspectionTreeNodeDataSource"] = value;
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
                return gdvRelatedInspectionList.GridViewNumber;
            }

            set
            {
                gdvRelatedInspectionList.GridViewNumber = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// bind data list for related inspection.
        /// the first inspection model is parent level and the other inspection models is child level.
        /// </summary>
        public void Bind()
        {
            gdvRelatedInspectionList.DataSource = ConstructDataSource(DataSource);
            gdvRelatedInspectionList.EmptyDataText = IsAdmin ? string.Empty : GetTextByKey("ins_inspectionList_message_noRecord");
            gdvRelatedInspectionList.DataBind();
        }

        /// <summary>
        /// <c>OnInit</c> event method
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvRelatedInspectionList, ModuleName, IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// convert tree node model to view model for display.
        /// </summary>
        /// <param name="tree">InspectionTreeNode Model</param>
        /// <returns>InspectionRelatedItem ViewModel</returns>
        private IList<InspectionRelatedItemViewModel> ConstructDataSource(InspectionTreeNodeModel tree)
        {
            IList<InspectionRelatedItemViewModel> relatedInspections = new List<InspectionRelatedItemViewModel>();

            if (tree != null)
            {
                string parentText = GetTextByKey("aca_common_label_parent");
                string childText = GetTextByKey("aca_common_label_child");

                //get parent inspection.
                if (tree.inspectionModel != null)
                {
                    Convert2ViewModel(parentText, tree.inspectionModel, relatedInspections);
                }

                //get children inspection.
                if (tree.children != null && tree.children.Length > 0)
                {
                    InspectionTreeNodeModel[] childNode = tree.children[0].children;

                    if (childNode != null && childNode.Length > 0)
                    {
                        for (int i = 0; i < childNode.Length; i++)
                        {
                            Convert2ViewModel(childText, childNode[i].inspectionModel, relatedInspections);
                        }
                    }
                }
            }

            return relatedInspections;
        }

        /// <summary>
        /// Convert tree node model to view model for display.
        /// </summary>
        /// <param name="relationShip">Child or parent</param>
        /// <param name="inspectionModel">Inspection Model</param>
        /// <param name="relatedInspections">InspectionRelatedItem ViewModel</param>
        private void Convert2ViewModel(string relationShip, InspectionModel inspectionModel, IList<InspectionRelatedItemViewModel> relatedInspections)
        {
            if (inspectionModel != null && inspectionModel.activity != null && !ValidationUtil.IsNo(inspectionModel.activity.displayInACA))
            {
                var template = new InspectionRelatedItemViewModel();

                template.InspectionID = Convert.ToString(inspectionModel.activity.idNumber);
                template.InspectionName = I18nStringUtil.GetString(inspectionModel.activity.resActivityType, inspectionModel.activity.activityType);

                var inspectionBll = ObjectFactory.GetObject<IInspectionBll>();
                var inspectionStatus = inspectionBll.GetStatus(inspectionModel);
                var resStatusString = inspectionBll.GetResStatusString(inspectionStatus, inspectionModel);
                template.StatusText = InspectionViewUtil.GetStatusText(resStatusString, inspectionModel.activity.status, inspectionStatus, ModuleName);

                template.RelationShip = relationShip;
                var parameter = new InspectionParameter();
                parameter.ID = template.InspectionID;
                CapIDModel recordIDModel = Cap == null ? null : TempModelConvert.Trim4WSOfCapIDModel(Cap.capID);
                parameter.AgencyCode = ServiceProviderCode;
                parameter.RecordAltID = recordIDModel != null ? recordIDModel.customID : string.Empty;
                parameter.RecordID1 = recordIDModel != null ? recordIDModel.ID1 : string.Empty;
                parameter.RecordID2 = recordIDModel != null ? recordIDModel.ID2 : string.Empty;
                parameter.RecordID3 = recordIDModel != null ? recordIDModel.ID3 : string.Empty;
                parameter.ModuleName = ModuleName;
                parameter.PublicUserID = CurrentUser.PublicUserId;
                parameter.IsPopupPage = ACAConstant.COMMON_Y;
                string queryString = InspectionParameterUtil.BuildQueryString(parameter);

                if (!string.IsNullOrEmpty(Request[UrlConstant.HIDE_ACTION_BUTTON]))
                {
                    queryString += string.Format("&{0}={1}", UrlConstant.HIDE_ACTION_BUTTON, Request[UrlConstant.HIDE_ACTION_BUTTON]);
                }

                string relativeURL = string.Format("Inspection/InspectionDetails.aspx?{0}", queryString);
                template.TargetURL = FileUtil.AppendApplicationRoot(relativeURL);

                relatedInspections.Add(template);
            }
        }

        #endregion Methods
    }
}