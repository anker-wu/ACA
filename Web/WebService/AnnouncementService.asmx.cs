#region Header

/*
 * <pre>
 *  Accela Citizen Access
 *  File: AnnouncementService.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web.Services;

using Accela.ACA.BLL.Announcement;
using Accela.ACA.Common;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.WebService
{
    /// <summary>
    /// This is the Announcement Web Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class AnnouncementService 
    {
        /// <summary>
        /// Build announcement information with JSON format to client from server according to timer interval
        /// </summary>
        /// <returns>announcement summary</returns>
        [System.Web.Services.WebMethod(Description = "GetAnnouncementByTimer", EnableSession = true)]
        public string GetAnnouncementByTimer()
        {
            return GetAnnouncementSummaryFromServer();            
        }

        /// <summary>
        /// Build announcement information with JSON format to client from session
        /// </summary>
        /// <returns>JSON string value</returns>
        [System.Web.Services.WebMethod(Description = "GetAnnouncementOfSession", EnableSession = true)]
        public string GetAnnouncementOfSession()
        {
            return GetAnnouncementSummaryFromSession(); 
        }

        /// <summary>
        /// Build announcement information with JSON format to client
        /// Update announcement read status
        /// 1)When the user click mark as read button
        /// 2)When the user click announcement content from popup notice window
        /// 3)When the user click announcement title or content from announcement list page
        /// </summary>
        /// <param name="announcementId">announcement id</param>
        /// <returns>JSON string value</returns>
        [System.Web.Services.WebMethod(Description = "UpdateAnnouncementStatus", EnableSession = true)]
        public string UpdateAnnouncementStatus(string announcementId)
        {
            return UpdateAnnouncement(announcementId);
        }

        /// <summary>
        /// Get announcement summary from server
        /// </summary>
        /// <returns>announcement summary</returns>
        private string GetAnnouncementSummaryFromServer()
        {
            IAnnouncementBll announcementBll = (IAnnouncementBll)ObjectFactory.GetObject(typeof(IAnnouncementBll));
            List<AnnouncementModel> announcements = GetAnnoucementListFromServer();
            return announcementBll.GetAnnouncementSummary(announcements);
        }

        /// <summary>
        /// Get announcement summary of session
        /// </summary>
        /// <returns>announcement summary</returns>
        private string GetAnnouncementSummaryFromSession()
        {
            IAnnouncementBll announcementBll = (IAnnouncementBll)ObjectFactory.GetObject(typeof(IAnnouncementBll));
            List<AnnouncementModel> announcements = GetAnnoucementListFromSession();
            return announcementBll.GetAnnouncementSummary(announcements);
        }

        /// <summary>
        /// Update announcement status
        /// </summary>
        /// <param name="announcementId">announcement id</param>
        /// <returns>announcement summary</returns>
        private string UpdateAnnouncement(string announcementId)
        {
            try
            {
                AnnouncementModel am = AppSession.GetAnnouncementsFromSession().Find(o => o.AuditID.ToString() == announcementId);

                if (AppSession.User != null && !string.IsNullOrEmpty(AppSession.User.UserID) && !AppSession.User.IsAnonymous && am != null && !am.IsRead)
                {
                    MessageModel[] messages = new MessageModel[1];
                    messages[0] = new MessageModel();
                    messages[0].messageID = long.Parse(announcementId);
                    messages[0].servProvCode = am.AnnouncementAgencyCode;
                    IAnnouncementBll announcementBll = (IAnnouncementBll)ObjectFactory.GetObject(typeof(IAnnouncementBll));
                    announcementBll.UpdateAnnouncementFromServer(messages);
                }

                if (am != null && !am.IsRead)
                {
                    AnnouncementUtil.MarkAsReadForOneInSession(announcementId);
                }

                return GetAnnouncementSummaryFromSession();
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get announcement list from server
        /// </summary>
        /// <returns>Announcement model list</returns>
        private List<AnnouncementModel> GetAnnoucementListFromServer()
        {
            try
            {
                IAnnouncementBll announcementBll = (IAnnouncementBll)ObjectFactory.GetObject(typeof(IAnnouncementBll));
                MessageModel[] announcementArray = announcementBll.GetAnnouncementsFromServer();

                List<AnnouncementModel> announcementList = AnnouncementUtil.ConstructDailyModelFromWSModel(announcementArray);

                AppSession.SetAnnouncementsToSession(announcementList);
                 
                return announcementList;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }
        
        /// <summary>
        /// Get announcement list from session
        /// </summary>
        /// <returns>announcement model list</returns>
        private List<AnnouncementModel> GetAnnoucementListFromSession()
        {
            try
            {
                List<AnnouncementModel> announcements = AppSession.GetAnnouncementsFromSession();

                if (announcements == null && AppSession.GetAnnouncementFlagFromSession() == null)
                {
                    announcements = GetAnnoucementListFromServer();
                    AppSession.SetAnnouncementFlagToSession("true");
                }

                return announcements;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }
    }
}
