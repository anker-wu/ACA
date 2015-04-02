#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IAnnouncementBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using System.Data;

using Accela.ACA.UI.Model;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Announcement
{
    /// <summary>
    /// Interface Announcement business
    /// </summary>
    public interface IAnnouncementBll
    {
        #region Methods

        /// <summary>
        /// Delete announcement.
        /// </summary> 
        /// <param name="annoucementID">announcement id</param> 
        /// <param name="annAgencyCode">announcement agency code</param> 
        void DeleteAnnouncementFromServer(string annoucementID, string annAgencyCode); 
 
        /// <summary>
        /// Update announcements in server
        /// </summary>
        /// <param name="announcements">announcement model objects</param> 
        void UpdateAnnouncementFromServer(MessageModel[] announcements); 

        /// <summary>
        /// Get announcements From server
        /// </summary>
        /// <returns>message model</returns>
        MessageModel[] GetAnnouncementsFromServer();

        /// <summary>
        /// Get announcement summary
        /// </summary>
        /// <param name="announcements">announcement information</param>
        /// <returns>announcement summary</returns>
        string GetAnnouncementSummary(List<AnnouncementModel> announcements);
         
        /// <summary>
        /// convert announcement model to data table.
        /// </summary>
        /// <param name="announcements">announcement model List</param>
        /// <returns>data table</returns>
        DataTable ConvertAnnouncementModelToDataTable(List<AnnouncementModel> announcements);

        #endregion Methods
    }
}
