#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AnnouncementBll.cs
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.UI.Model;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Announcement
{
    /// <summary>
    /// Announcement business logic.
    /// </summary>
    public class AnnouncementBll : BaseBll, IAnnouncementBll
    {        
        #region Properties

        /// <summary>
        /// Gets an instance of announcement.
        /// </summary>
        private AnnouncementWebServiceService AnnouncementService
        {
            get
            {
                return WSFactory.Instance.GetWebService<AnnouncementWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Update announcement to server
        /// </summary>
        /// <param name="announcements">announcement model objects</param>
        public void UpdateAnnouncementFromServer(MessageModel[] announcements)
        {
            try
            {
                AnnouncementService.markAnnouncementsAsRead(AgencyCode, announcements, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get announcements on server
        /// </summary>
        /// <returns>message model</returns>
        public MessageModel[] GetAnnouncementsFromServer()
        {
            try
            {
                MessageModel[] announcementArray = AnnouncementService.getEffectiveAnnouncement(AgencyCode, User == null ? string.Empty : User.PublicUserId);
                return announcementArray;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Deletes the announcement from server.
        /// </summary>
        /// <param name="announcementId">The announcement id.</param>
        /// <param name="annAgencyCode">The announcement agency code.</param>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode, announcementId, usrId</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void DeleteAnnouncementFromServer(string announcementId, string annAgencyCode)
        {
            if (string.IsNullOrEmpty(AgencyCode) || string.IsNullOrEmpty(User.UserID))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "announcementId", "usrId" });
            }

            try
            {
                MessageModel[] announcements = new MessageModel[1];
                announcements[0] = new MessageModel();
                announcements[0].messageID = Convert.ToInt64(announcementId);
                announcements[0].servProvCode = annAgencyCode;
                AnnouncementService.deleteAnnouncements(AgencyCode, announcements, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Convert announcement model to data table.
        /// </summary>
        /// <param name="announcementList">announcement list</param>
        /// <returns>data table</returns>
        public DataTable ConvertAnnouncementModelToDataTable(List<AnnouncementModel> announcementList)
        {
            DataTable dtAnnouncements = ConstructAnnouncementDataTable();

            if (announcementList == null || announcementList.Count == 0)
            {
                return dtAnnouncements;
            }

            string title = string.Empty;
            string contentFull = string.Empty;
            string contentPart = string.Empty;
            string contentPartForList = string.Empty;

            foreach (AnnouncementModel announcement in announcementList)
            {
                title = announcement.AnnouncementContentTitle ?? string.Empty;
                contentFull = announcement.AnnouncementContentFull ?? string.Empty;
                contentPart = announcement.AnnouncementContentPart ?? string.Empty;
                contentPartForList = announcement.AnnouncementContentPartForList ?? string.Empty;

                DataRow dr = dtAnnouncements.NewRow();
                dr["AnnouncementID"] = announcement.AuditID.ToString();
                dr["ValidDate"] = announcement.StartDate == null ? DBNull.Value : (object)announcement.StartDate;
                dr["AnnouncementContentTitle"] = title;
                dr["ContentPart"] = contentPart;
                dr["ContentPartForList"] = contentPartForList;
                dr["AnnouncementContentFull"] = contentFull;
                dr["IsRead"] = announcement.IsRead.ToString();
                dtAnnouncements.Rows.Add(dr);
            }

            return dtAnnouncements;
        }

        /// <summary>
        /// Construct announcement data table format.
        /// </summary>
        /// <returns>data table</returns>
        public DataTable ConstructAnnouncementDataTable()
        {
            //reuse Construct Announcement DataTable function.
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("AnnouncementID", typeof(string)));
            dt.Columns.Add(new DataColumn("AnnouncementContentTitle", typeof(string)));
            dt.Columns.Add(new DataColumn("ContentPart", typeof(string)));
            dt.Columns.Add(new DataColumn("ContentPartForList", typeof(string)));
            dt.Columns.Add(new DataColumn("ValidDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("AnnouncementContentFull", typeof(string)));
            dt.Columns.Add(new DataColumn("AnnouncementModel", typeof(AnnouncementModel)));
            dt.Columns.Add(new DataColumn("IsRead", typeof(string)));
            return dt;
        }

        /// <summary>
        /// Get announcement summary
        /// </summary>
        /// <param name="announcements">announcement information</param>
        /// <returns>announcement summary</returns>
        public string GetAnnouncementSummary(List<AnnouncementModel> announcements)
        {
            if (announcements != null && announcements.Count > 0)
            {
                List<AnnouncementModel> ppInfoList = new List<AnnouncementModel>();

                foreach (AnnouncementModel am in announcements)
                {
                    if (!am.IsRead)
                    {
                        ppInfoList.Add(am);
                    }
                }

                if (ppInfoList.Count > 0)
                {
                    return GetAnnouncementKeyValue(ppInfoList[0], ppInfoList.Count.ToString(), "true");
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get announcement as Json format.
        /// </summary>
        /// <param name="am">The parameter.</param>
        /// <param name="count">The count.</param>
        /// <param name="isDisplay">is display</param>
        /// <returns>announcement json string</returns>
        private string GetAnnouncementKeyValue(AnnouncementModel am, string count, string isDisplay)
        {
            StringBuilder sb = new StringBuilder("{");
            BllUtil.AddKeyValue(sb, "IsDisplay", isDisplay);
            BllUtil.AddKeyValue(sb, "AnnouncementCount", count);
            BllUtil.AddKeyValue(sb, "AnnouncementContentFull", ScriptFilter.EncodeJson(am.AnnouncementContentFull));
            BllUtil.AddKeyValue(sb, "AnnouncementContentPart", ScriptFilter.EncodeJson(am.AnnouncementContentPart));
            BllUtil.AddKeyValue(sb, "AnnouncementContentPartForList", ScriptFilter.EncodeJson(am.AnnouncementContentPartForList));
            BllUtil.AddKeyValue(sb, "AnnouncementContentTitle", ScriptFilter.EncodeJson(am.AnnouncementContentTitle));
            BllUtil.AddKeyValue(sb, "AnnouncementId", am.AuditID.ToString());
            sb.Length -= 1;
            sb.Append("}");

            return sb.ToString();
        } 

        #endregion Methods
    }
}