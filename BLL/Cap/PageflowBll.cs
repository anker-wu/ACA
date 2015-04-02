#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IPageflowBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: PageflowBll.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/16/2008       daly.zeng       Initial version.
 * </pre>
 */

#endregion Header

using System;
using System.Collections;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

using log4net;

namespace Accela.ACA.BLL.Cap
{
    /// <summary>
    /// This class provide the ability to operation page flow.
    /// </summary>
    internal class PageflowBll : BaseBll, IPageflowBll
    {
        #region Fields

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of CapTypeService.
        /// </summary>
        private CapTypeWebServiceService CapTypeService
        {
            get
            {
                return WSFactory.Instance.GetWebService<CapTypeWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of PageFlowConfigService.
        /// </summary>
        private PageFlowConfigWebServiceService PageFlowConfigService
        {
            get
            {
                return WSFactory.Instance.GetWebService<PageFlowConfigWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create a PageFlow group
        /// </summary>
        /// <param name="pageFlowGroupModel">the PageFlowGroupModel model</param>
        /// <param name="moduleName">The module this group belongs to</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void CreatePageFlowGroup(PageFlowGroupModel pageFlowGroupModel, string moduleName)
        {
            try
            {
                pageFlowGroupModel.serviceProviderCode = AgencyCode;
                PageFlowConfigService.createPageFlowGroup(pageFlowGroupModel, moduleName, ACAConstant.ADMIN_CALLER_ID); //User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Edit a PageFlow group
        /// </summary>
        /// <param name="pageFlowGroupModel">the PageFlowGroupModel model</param>
        /// <param name="moduleName">The module this group belongs to</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void EditePageFlowGroup(PageFlowGroupModel pageFlowGroupModel, string moduleName)
        {
            try
            {
                pageFlowGroupModel.serviceProviderCode = AgencyCode;
                PageFlowConfigService.editePageFlowGroup(pageFlowGroupModel, moduleName, ACAConstant.ADMIN_CALLER_ID); //groupType, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get a PageFlow group
        /// </summary>
        /// <param name="moduleName">The module this group belongs to</param>
        /// <param name="groupCode">The page flow code</param>
        /// <param name="agencyCode">The agency code which the page flow belongs to</param>
        /// <returns>a Page Flow Group information</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public PageFlowGroupModel GetPageFlowGroup(string moduleName, string groupCode, string agencyCode = null)
        {
            try
            {
                if (agencyCode == null)
                {
                    agencyCode = AgencyCode;
                }

                PageFlowGroupModel pageFlowGroup = PageFlowConfigService.getPageFlowGroup(agencyCode, moduleName, groupCode, ACAConstant.ADMIN_CALLER_ID);
                return SortByDisplayOrder(pageFlowGroup);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get a string array which contains all smart choice names
        /// </summary>
        /// <param name="groupType">the page flow type</param>
        /// <returns>a string array</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string[] GetPageFlowGroupNameList(string groupType)
        {
            try
            {
                return PageFlowConfigService.getPageFlowGroupNameList(AgencyCode, groupType);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get page flow group by Cap ID.
        /// </summary>
        /// <param name="capID">Cap ID object</param>
        /// <returns>Page Flow Group information</returns>
        PageFlowGroupModel IPageflowBll.GetPageFlowGroupByCapID(CapIDModel capID)
        {
            PageFlowGroupModel pageFlowGroup = PageFlowConfigService.getPageFlowGroupByCapID(capID, User.PublicUserId);
            return SortByDisplayOrder(pageFlowGroup);
        }

        /// <summary>
        /// Get common page flow list by cap id
        /// </summary>
        /// <param name="capID">cap id model</param>
        /// <param name="callerID">caller id.</param>
        /// <returns>PageFlowComponentModel list.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        PageFlowComponentModel[] IPageflowBll.GetPageFlowComponentsByCapID(CapIDModel capID, string callerID)
        {
            try
            {
                return PageFlowConfigService.getPageFlowComponentsByCapID(capID, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get page flow group from cache
        /// </summary>
        /// <param name="capType">cap type object.</param>
        /// <returns>a PageFlowGroupModel model</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public PageFlowGroupModel GetPageflowGroupByCapType(CapTypeModel capType)
        {
            try
            {
                CapTypeDetailModel capTypeDetail = CapTypeService.getCapTypeByPK(capType);

                if (capTypeDetail == null || string.IsNullOrEmpty(capTypeDetail.smartchoiceCode4ACA))
                {
                    return null;
                }

                string agencyCode = capTypeDetail.serviceProviderCode;

                ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                string cacheKey = agencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_PAGEFLOWGROUP);
                Hashtable htPageflowGroups = cacheManager.GetCachedItem(agencyCode, cacheKey);

                if (htPageflowGroups.ContainsKey(capTypeDetail.smartchoiceCode4ACA) && htPageflowGroups[capTypeDetail.smartchoiceCode4ACA] != null)
                {
                    PageFlowGroupModel pageflowGroup = htPageflowGroups[capTypeDetail.smartchoiceCode4ACA] as PageFlowGroupModel;
                    return SortByDisplayOrder(pageflowGroup);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Sort by display order
        /// </summary>
        /// <param name="pageflow">Page Flow Group Model</param>
        /// <returns>a PageFlowGroupModel</returns>
        private PageFlowGroupModel SortByDisplayOrder(PageFlowGroupModel pageflow)
        {
            if (pageflow == null || pageflow.stepList == null)
            {
                return null;
            }

            ArrayList stepList = new ArrayList(pageflow.stepList);
            StepComparer stepComparer = new StepComparer();
            PageComparer pageComparer = new PageComparer();
            stepList.Sort(stepComparer);

            foreach (StepModel step in stepList)
            {
                ArrayList pageList = new ArrayList(step.pageList);
                pageList.Sort(pageComparer);

                step.pageList = (PageModel[])pageList.ToArray(typeof(PageModel));
            }

            pageflow.stepList = (StepModel[])stepList.ToArray(typeof(StepModel));

            return pageflow;
        }

        #endregion Methods

        #region Nested Types

        /// <summary>
        /// the class for PageComparer.
        /// </summary>
        public class PageComparer : IComparer
        {
            #region Methods

            /// <summary>
            /// Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            /// </summary>
            /// <param name="x">x: page1 model.</param>
            /// <param name="y">x: page2  model.</param>
            /// <returns>compare result.</returns>
            int IComparer.Compare(object x, object y)
            {
                PageModel page1 = x as PageModel;
                PageModel page2 = y as PageModel;

                //compare the order
                return page1.pageOrder.CompareTo(page2.pageOrder);
            }

            #endregion Methods
        }

        /// <summary>
        /// the class for StepComparer.
        /// </summary>
        public class StepComparer : IComparer
        {
            #region Methods

            /// <summary>
            /// Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            /// </summary>
            /// <param name="x">x: step model</param>
            /// <param name="y">y: step model</param>
            /// <returns>compare result</returns>
            int IComparer.Compare(object x, object y)
            {
                StepModel step1 = x as StepModel;
                StepModel step2 = y as StepModel;

                //compare the order
                return step1.stepOrder.CompareTo(step2.stepOrder);
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}