#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AdminPageflowBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AdminPageflowBll.cs 277080 2014-08-11 06:28:07Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/16/2008    Weiky.Chen    Initial version.
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Web;
using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// This class is operation page flow in ACA admin. 
    /// </summary>
    internal class AdminPageflowBll : BaseBll, IPageflowBll
    {
        #region Fields

        /// <summary>
        /// Logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(AdminPageflowBll));

        #endregion Fields

        #region Properties

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
                PageFlowGroupModel pageFlowGroup = PageFlowConfigService.getPageFlowGroup(AgencyCode, moduleName, groupCode, ACAConstant.ADMIN_CALLER_ID); //groupType, User.PublicUserId);

                return pageFlowGroup;
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
            return null;
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
                return null;
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
        public PageFlowGroupModel GetPageflowGroupByCapType(CapTypeModel capType)
        {
            string serverPath = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath) + ACAConstant.ADMIN_PAGE_FLOW_DUMMY_DATA;
            return ObjectXMLSerializer<PageFlowGroupModel>.Load(serverPath);
        }

        #endregion Methods

        #region Nested Types

        /// <summary>
        /// This class is page comparer in ACA admin. 
        /// </summary>
        public class PageComparer : IComparer
        {
            #region Methods

            /// <summary>
            /// Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            /// </summary>
            /// <param name="x">x: x page position.</param>
            /// <param name="y">y: y page position.</param>
            /// <returns>compare the order</returns>
            int IComparer.Compare(object x, object y)
            {
                PageModel page1 = x as PageModel;
                PageModel page2 = y as PageModel;

                return page1.pageOrder.CompareTo(page2.pageOrder);
            }

            #endregion Methods
        }

        /// <summary>
        /// This class is cap type filter comparer
        /// </summary>
        public class StepComparer : IComparer
        {
            #region Methods

            /// <summary>
            /// Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            /// </summary>
            /// <param name="x">x: x page position.</param>
            /// <param name="y">y: y page position.</param>
            /// <returns>compare the order</returns>
            int IComparer.Compare(object x, object y)
            {
                StepModel step1 = x as StepModel;
                StepModel step2 = y as StepModel;

                return step1.stepOrder.CompareTo(step2.stepOrder);
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}