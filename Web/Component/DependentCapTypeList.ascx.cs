#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DependentCapTypeList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012-2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: DependentCapTypeList.ascx.cs 232438 2012-09-12 07:17:54Z ACHIEVO\foxus.lin $.
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// A custom control to display the dependent cap type list.
    /// </summary>
    public partial class DependentCapTypeList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// The command name to apply application with dependent cap type.
        /// </summary>
        private string _applyDependentCapType = "GoToApply";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this list is used in multiple records page.
        /// </summary>
        public bool IsMutipleRecord
        {
            get;

            set;
        }

        #endregion

        /// <summary>
        /// Display the dependent cap types' link.
        /// </summary>
        /// <param name="capIDModel">The CapIDModel.</param>
        /// <param name="moduleName">The module name.</param>
        public void DisplayDependentCapTypeLink(CapIDModel4WS capIDModel, string moduleName)
        {
            ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();
            CapIDModel currentCapIDModel = TempModelConvert.Trim4WSOfCapIDModel(capIDModel);

            // show dependent cap types.
            CapTypeModel[] dependentCapTypeList = capTypeBll.GetDependentCapTypeList(ConfigManager.AgencyCode, moduleName, currentCapIDModel);

            if (dependentCapTypeList == null)
            {
                return;
            }

            ICapTypeFilterBll capTypFilterBll = ObjectFactory.GetObject<ICapTypeFilterBll>();
            string filterName = capTypFilterBll.GetCapTypeFilterByLabelKey(ConfigManager.AgencyCode, moduleName, "aca_sys_feature_apply_a_permit");
            List<CapTypeModel> capTypeList = new List<CapTypeModel>();

            // filter the cap type that not have permission in ACA
            foreach (CapTypeModel item in dependentCapTypeList)
            {
                bool isCapTypeAccessible = capTypeBll.IsCapTypeAccessible(item, moduleName, filterName, ACAConstant.VCH_TYPE_VHAPP, AppSession.User.PublicUserId);

                if (isCapTypeAccessible)
                {
                    // set the module name as the method invoked will not set it.
                    item.moduleName = moduleName;

                    capTypeList.Add(item);
                }
            }

            if (capTypeList.Count > 0)
            {
                rptDependentCapType.DataSource = capTypeList;
                rptDependentCapType.DataBind();
            }
        }

        /// <summary>
        /// Handle the Page_Load event.
        /// </summary>
        /// <param name="sender">Trigger object.</param>
        /// <param name="e">Event argument.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// bind url for dependent cap type link
        /// </summary>
        /// <param name="source">system sender.</param>
        /// <param name="e">data list event args.</param>
        protected void DependentCapType_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (AppSession.IsAdmin)
            {
                return;
            }

            if (e.CommandName.Equals(_applyDependentCapType, StringComparison.InvariantCulture))
            {
                string[] args = e.CommandArgument.ToString().Split(',');
                string capTypeValue = args[0];
                string capTypeText = args[1];
                string moduleName = args[2];

                CapTypeModel dependentCapType = CapUtil.CreateNewCapType(capTypeValue, capTypeText, moduleName);
                string url = BuildDependentUrl(dependentCapType);

                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Build the apply record url for dependent cap type
        /// </summary>
        /// <param name="capType">the cap type model</param>
        /// <returns>url string</returns>
        private string BuildDependentUrl(CapTypeModel capType)
        {
            List<LinkItem> links = TabUtil.GetCreationLinkItemList(capType.moduleName, false);
            LinkItem creationItem = links[0];

            ICapTypeFilterBll capTypFiltereBll = ObjectFactory.GetObject<ICapTypeFilterBll>();
            string filterName = capTypFiltereBll.GetCapTypeFilterByLabelKey(ConfigManager.AgencyCode, creationItem.Module, creationItem.Label);

            StringBuilder sbUrl = new StringBuilder();
            sbUrl.Append(TabUtil.RebuildUrl(creationItem.Url, creationItem.Module, filterName));

            if (sbUrl.ToString().IndexOf(ACAConstant.QUESTION_MARK) >= 0)
            {
                sbUrl.Append(ACAConstant.AMPERSAND);
            }
            else
            {
                sbUrl.Append(ACAConstant.QUESTION_MARK);
            }

            string capTypeUrlParam = CAPHelper.GetCapTypeValue(capType);
            sbUrl.Append(UrlConstant.CAPTYPE + ACAConstant.EQUAL_MARK + capTypeUrlParam);

            if (StandardChoiceUtil.IsSuperAgency())
            {
                sbUrl.Append("&" + UrlConstant.AgencyCode + "=" + capType.serviceProviderCode);
                sbUrl.Append("&" + ACAConstant.IS_SUBAGENCY_CAP + "=" + ACAConstant.COMMON_Y);
            }

            return FileUtil.AppendApplicationRoot(sbUrl.ToString());
        }
    }
}