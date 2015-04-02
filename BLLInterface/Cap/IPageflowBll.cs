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
 * $Id: IPageflowBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/16/2008    daly.zeng    Initial version.
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Cap
{
    /// <summary>
    /// Defines methods for page flow.
    /// </summary>
    public interface IPageflowBll
    {
        #region Methods

        /// <summary>
        /// Create a PageFlow group
        /// </summary>
        /// <param name="pageFlowGroupModel">the PageFlowGroupModel model</param>
        /// <param name="moduleName">The module this group belongs to</param>
        void CreatePageFlowGroup(PageFlowGroupModel pageFlowGroupModel, string moduleName);

        /// <summary>
        /// Edit a PageFlow group
        /// </summary>
        /// <param name="pageFlowGroupModel">the PageFlowGroupModel model</param>
        /// <param name="moduleName">The module this group belongs to</param>
        void EditePageFlowGroup(PageFlowGroupModel pageFlowGroupModel, string moduleName);

        /// <summary>
        /// Get a PageFlow group
        /// </summary>
        /// <param name="moduleName">The module this group belongs to</param>
        /// <param name="groupCode">The page flow code</param>
        /// <param name="agencyCode">The agency code which the page flow belongs to</param>
        /// <returns>a Page Flow Group information</returns>
        PageFlowGroupModel GetPageFlowGroup(string moduleName, string groupCode, string agencyCode = null);

        /// <summary>
        /// Get a string array which contains all smart choice names
        /// </summary>
        /// <param name="groupType">the page flow type</param>
        /// <returns>a string array </returns>
        string[] GetPageFlowGroupNameList(string groupType);

        /// <summary>
        /// Get page flow group from cache
        /// </summary>
        /// <param name="capType"> cap type object.</param>
        /// <returns>a PageFlowGroupModel model</returns>
        PageFlowGroupModel GetPageflowGroupByCapType(CapTypeModel capType);

        /// <summary>
        /// Get page flow group by Cap ID.
        /// </summary>
        /// <param name="capID">Cap ID object</param>
        /// <returns>Page Flow Group information</returns>
        PageFlowGroupModel GetPageFlowGroupByCapID(CapIDModel capID);

        /// <summary>
        /// Get common page flow list by cap id
        /// </summary>
        /// <param name="capID">cap id model</param>
        /// <param name="callerID">caller id.</param>
        /// <returns>PageFlowComponentModel list.</returns>
        PageFlowComponentModel[] GetPageFlowComponentsByCapID(CapIDModel capID, string callerID);

        #endregion Methods
    }
}