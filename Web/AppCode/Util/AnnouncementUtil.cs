#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AnnouncementUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 * </pre>
 */
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;

using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Util
{
    /// <summary>
    /// announcement utility class for related logic.
    /// </summary>   
    public static class AnnouncementUtil
    {
        /// <summary>
        /// Get read announcements from cookie
        /// </summary>
        /// <returns>cookie value list</returns>
        public static List<string> GetReadAnnouncementsFromCookie()
        {
            List<string> readAnns = new List<string>();
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(CookieConstant.USER_READ_ANNOUNCEMENT);

            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                char split = '|';
                string[] parms = cookie.Value.Split(split);
                foreach (string readId in parms)
                {
                    if (!string.IsNullOrEmpty(readId))
                    {
                        readAnns.Add(readId);
                    }
                }
            }

            return readAnns;
        }

        /// <summary>
        /// set read announcement to cookie
        /// </summary>
        /// <param name="readAnn">read announcement</param>
        public static void SetReadAnnouncementsToCookie(string readAnn)
        {
            if (!string.IsNullOrEmpty(readAnn))
            {
                HttpCookie cookie = new HttpCookie(CookieConstant.USER_READ_ANNOUNCEMENT, readAnn);
                cookie.HttpOnly = true;
                HttpContext.Current.Response.Cookies.Add(cookie);
                HttpContext.Current.Response.Cookies[CookieConstant.USER_READ_ANNOUNCEMENT].Expires = DateTime.Now.AddDays(1);
            }
        }

        /// <summary>
        /// Append read announcement record to cookie
        /// </summary>
        /// <param name="annInCookie">announcement in cookie</param>
        /// <returns>cookie value</returns>
        public static string AppendReadAnnouncementsToCookie(string annInCookie)
        {
            string cookieValue = string.Empty;
            List<string> readAnnouncementsInCookie = GetReadAnnouncementsFromCookie();

            if (!string.IsNullOrEmpty(annInCookie) && readAnnouncementsInCookie.IndexOf(annInCookie) == -1)
            {
                readAnnouncementsInCookie.Add(annInCookie);

                foreach (string readId in readAnnouncementsInCookie)
                {
                    if (!string.IsNullOrEmpty(readId))
                    {
                        cookieValue = cookieValue + readId + ACAConstant.SPLIT_CHAR4URL1;
                    }
                }
            }

            return cookieValue;
        }

        /// <summary>
        /// Convert announcement model of web service to data model of daily
        /// </summary>
        /// <param name="messageModels">web service model</param>
        /// <returns>daily model</returns>
        public static List<AnnouncementModel> ConstructDailyModelFromWSModel(MessageModel[] messageModels)
        {
            if (messageModels == null || messageModels.Length < 1)
            {
                return null;
            }

            List<string> readAnnsInCookie = GetReadAnnouncementsFromCookie();

            List<AnnouncementModel> announcements = new List<AnnouncementModel>();
            foreach (MessageModel mM in messageModels)
            {
                if (mM.messageID != null)
                {
                    AnnouncementModel aM = new AnnouncementModel();

                    aM.AuditID = (long)mM.messageID;
                    string more = LabelUtil.GetGlobalTextByKey("aca_announcement_content_more");
                    string msgText = I18nStringUtil.GetString(mM.resMessageText, mM.messageText).Trim();
                    string msgTitle = !string.IsNullOrEmpty(mM.messageTitle) ? ScriptFilter.RemoveHTMLTag(mM.messageTitle.Replace("\r\n", " ")) + ": " : string.Empty;
                    string nowrapText = !string.IsNullOrEmpty(msgText) ? ScriptFilter.RemoveHTMLTag(msgText.Replace("\r\n", " ")) : string.Empty;
                    string popupText = msgTitle + nowrapText;
                    string tempTitle = string.Empty;
                    string tempContent = string.Empty;

                    if (!string.IsNullOrEmpty(popupText) && popupText.Length > 200)
                    {
                        // Combine title and content in the popup window, but limit length in 200 chars.
                        tempTitle = ScriptFilter.AntiXssHtmlEncode(msgTitle);
                        tempContent = ScriptFilter.AntiXssHtmlEncode(nowrapText.Substring(0, msgTitle.Length > 200 ? 0 : 200 - msgTitle.Length));
                        aM.AnnouncementContentPart = string.Format("<b>{0}</b><span>{1}</span>... <u><b>{2}</b></u>", tempTitle, tempContent, more);
                    }
                    else
                    {
                        tempTitle = ScriptFilter.AntiXssHtmlEncode(msgTitle);
                        tempContent = ScriptFilter.AntiXssHtmlEncode(nowrapText);
                        aM.AnnouncementContentPart = string.Format("<b>{0}</b><span>{1}</span>", tempTitle, tempContent);
                    }

                    if (!string.IsNullOrEmpty(nowrapText) && nowrapText.Length > 200)
                    {
                        tempContent = ScriptFilter.AntiXssHtmlEncode(nowrapText.Substring(0, 200));
                        aM.AnnouncementContentPartForList = string.Format("{0}... <u><b>{1}</b></u>", tempContent, more);
                    }
                    else
                    {
                        aM.AnnouncementContentPartForList = ScriptFilter.AntiXssHtmlEncode(nowrapText);
                    }

                    aM.AnnouncementContentFull = !string.IsNullOrEmpty(msgText) ? msgText.Replace("\r\n", "<br/>") : string.Empty;
                    aM.AnnouncementContentTitle = mM.messageTitle;
                    aM.StartDate = mM.startEffectDate;
                    aM.EndDate = mM.endEffectDate;
                    aM.RecDate = mM.recDate;
                    aM.AnnouncementAgencyCode = mM.servProvCode;

                    aM.IsRead = ACAConstant.COMMON_Y.ToString().Equals(mM.isRead, StringComparison.InvariantCultureIgnoreCase);
                    string annInCookie = GetAnnKeyInCookie(aM);

                    if (readAnnsInCookie != null && readAnnsInCookie.Count > 0)
                    {
                        string readId = readAnnsInCookie.Find(a => a == annInCookie);

                        if (!string.IsNullOrEmpty(readId))
                        {
                            aM.IsRead = true;
                        }
                        else
                        {
                            if (aM.IsRead)
                            {
                                readAnnsInCookie.Add(annInCookie);
                            }
                        }
                    }
                    else
                    {
                        if (aM.IsRead)
                        {
                            readAnnsInCookie.Add(annInCookie);
                        }
                    }

                    announcements.Add(aM);
                }
            }

            string updateAnnInCookie = string.Empty;

            foreach (string annInCookie in readAnnsInCookie)
            {
                if (!string.IsNullOrEmpty(annInCookie))
                {
                    updateAnnInCookie += annInCookie + ACAConstant.SPLIT_CHAR4URL1;
                }
            }

            SetReadAnnouncementsToCookie(updateAnnInCookie);

            announcements.Sort(
                delegate(AnnouncementModel x, AnnouncementModel y)
                {
                    int useStartDate = DateTime.Compare(Convert.ToDateTime(y.StartDate), Convert.ToDateTime(x.StartDate));
                    return useStartDate != 0 ? useStartDate : DateTime.Compare(Convert.ToDateTime(y.RecDate), Convert.ToDateTime(x.RecDate));
                });

            return announcements;
        }

        /// <summary>
        /// Update one announcement of session
        /// </summary>
        /// <param name="announcementId">announcement id</param>
        public static void MarkAsReadForOneInSession(string announcementId)
        {
            List<AnnouncementModel> announcements = AppSession.GetAnnouncementsFromSession();

            foreach (AnnouncementModel announcement in announcements)
            {
                if (announcementId.Equals(announcement.AuditID.ToString(CultureInfo.InvariantCulture)))
                {
                    announcement.IsRead = true;
                    string afterAddCookie = AnnouncementUtil.AppendReadAnnouncementsToCookie(GetAnnKeyInCookie(announcement));
                    AnnouncementUtil.SetReadAnnouncementsToCookie(afterAddCookie);
                    break;
                }
            }
        }

        /// <summary>
        /// Combine the announcement key for indicates a message form cookie.
        /// </summary>
        /// <param name="ann">the announcement model</param>
        /// <returns>the announcement model's key.</returns>
        public static string GetAnnKeyInCookie(AnnouncementModel ann)
        {
            return ann.AuditID.ToString();
        }
    }
}