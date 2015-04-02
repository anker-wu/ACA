#region Header

/**
 * <pre>
 *  Accela Citizen Access
 *  File: RelatedRecords.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description: A new form for the poped Related Records.
 *
 *  Notes:
 * $Id: RelatedRecords.aspx.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
 *  Revision History
 *  Date,           Who,        What
 *  2011/03/15      Alan Hu     Initial.
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Text;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// Related records popup page.
    /// </summary>
    public partial class RelatedRecords : PopupDialogBasePage
    {
        /// <summary>
        /// Build Related Record tree
        /// </summary>
        /// <param name="isShowAll">a value to indicates whether show all related records.</param>
        public void BuildTree(bool isShowAll)
        {
            //Decode the Cap info from args querystring.
            string capArgs = Request.QueryString["args"];

            if (string.IsNullOrEmpty(capArgs))
            {
                return;
            }

            string capIdData = Encoding.UTF8.GetString(Convert.FromBase64String(capArgs));
            string[] args = capIdData.Split(ACAConstant.SPLIT_CHAR);
            ProjectTreeNodeModel4WS capTree = null;
            if (!isShowAll)
            {
                CapIDModel capId = new CapIDModel();
                capId.ID1 = args[0];
                capId.ID2 = args[1];
                capId.ID3 = args[2];
                capId.serviceProviderCode = args[3];
                string moduleName = args[4];

                ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                QueryFormat queryFormat = new QueryFormat();
                capTree = capBll.GetSectionalRelatedCapTree(capId, queryFormat);
            }
            else
            {
                CapIDModel4WS capId = new CapIDModel4WS();
                capId.id1 = args[0];
                capId.id2 = args[1];
                capId.id3 = args[2];
                capId.serviceProviderCode = args[3];
                string moduleName = args[4];

                ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                capTree = capBll.GetRelatedCapTree(capId, null, null);
            }

            if (capTree != null && capTree.children != null)
            {
                RelatedCapTreeData capTreeData = new RelatedCapTreeData();

                // Set the cap type list by filter name which configed for Authorized Agent or Clerk role.
                if (AppSession.User.IsAuthorizedAgent || AppSession.User.IsAgentClerk)
                {
                    ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();
                    string filterName = Request.QueryString[UrlConstant.FILTER_NAME];
                    CapTypeModel[] capTypes = capTypeBll.GetGeneralCapTypeList(ModuleName, filterName, ACAConstant.VCH_TYPE_VHAPP, null, AppSession.User.PublicUserId);
                    IList<string> capTypeList = new List<string>();

                    if (capTypes != null)
                    {
                        foreach (CapTypeModel capType in capTypes)
                        {
                            capTypeList.Add(CAPHelper.GetAliasOrCapTypeLabel(capType));
                        }
                    }

                    capTreeData.CapTypeListFilter = capTypeList;
                }

                litCapTree.Text = capTreeData.GetBuildHtml(capTree, args[0] + args[1] + args[2], args[4], true, false);
            }            
        }

        /// <summary>
        /// change view 
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        protected void ChangeButton_Click(object sender, EventArgs e)
        {
            if (btnChange.CommandArgument == bool.FalseString)
            {
                //Show entry related records tree.
                btnChange.CommandArgument = bool.TrueString;
                btnChange.LabelKey = "aca_relatedrecord_label_entiretree";
                BuildTree(false);
            }
            else
            {
                //Show sectional related records tree.
                btnChange.CommandArgument = bool.FalseString;
                btnChange.LabelKey = "aca_relatedrecord_label_directlyrelatedrecord";
                BuildTree(true);
            }

            this.FocusElement(btnChange.ClientID);
        }

        /// <summary>
        /// Handle the Page_Load event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetPageTitleKey("per_permitlist_label_relatedrecords");
            this.SetDialogMaxHeight("550");
            if (!IsPostBack && !AppSession.IsAdmin)
            {
                BuildTree(false);
            }
        }
    }
}