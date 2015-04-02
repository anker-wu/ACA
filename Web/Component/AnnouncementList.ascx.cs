#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AnnouncementList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *  MyCollectionList user control.
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
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Announcement;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// The AnnouncementList Control.
    /// </summary>
    public partial class AnnouncementList : BaseUserControl
    {
        #region Properties
        /// <summary>
        /// Command name for deleting a announcement
        /// </summary>
        private const string COMMAND_DELETE_ANNOUNCEMENT = "DeleteAnnouncement"; 

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private DataTable GridViewDataSource
        {
            get
            {
                return (DataTable)ViewState["Announcements"];
            }

            set
            {
                ViewState["Announcements"] = value;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Binding announcement list.
        /// </summary> 
        /// <param name="dtAnnouncements">The announcement data table</param>
        public void BindAnnouncements(DataTable dtAnnouncements)
        {
            GridViewDataSource = dtAnnouncements;
            gdvAnnouncementList.DataSource = GridViewDataSource;
            gdvAnnouncementList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvAnnouncementList.DataBind();
        }

        /// <summary>
        /// Delete an announcement from sessions
        /// </summary>
        /// <param name="announcementId">announcement id</param>
        public void DeleteAnnouncementFromSession(string announcementId)
        {
            List<AnnouncementModel> announcements = AppSession.GetAnnouncementsFromSession();

            foreach (AnnouncementModel announcement in announcements)
            {
                if (announcementId.Equals(announcement.AuditID.ToString()))
                {
                    announcements.Remove(announcement);
                    AppSession.SetAnnouncementsToSession(announcements);
                    string afterDeleteCookie = AnnouncementUtil.AppendReadAnnouncementsToCookie(AnnouncementUtil.GetAnnKeyInCookie(announcement));
                    AnnouncementUtil.SetReadAnnouncementsToCookie(afterDeleteCookie);
                    break;
                }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Fire sorted event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void AnnouncementList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            GridViewDataSource.DefaultView.Sort = e.GridViewSortExpression;
            GridViewDataSource = GridViewDataSource.DefaultView.ToTable();
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
             if (!Page.IsPostBack) 
             {
                 SetAnnouncementList();

                 if ((AppSession.User != null && !AppSession.User.IsAnonymous) || AppSession.IsAdmin)
                 {
                     gdvAnnouncementList.Columns[6].Visible = true;
                 }
                 else
                 {
                     gdvAnnouncementList.Columns[6].Visible = false;
                 }
             }
        } 

        /// <summary>
        /// Row command.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void AnnouncementList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == COMMAND_DELETE_ANNOUNCEMENT)
            {
                //delete an announcement
                int dataItemIndex = Convert.ToInt32(e.CommandArgument);
                DeleteAnncouncement(sender, dataItemIndex);
            }
            else
            {
                gdvAnnouncementList.DataSource = GridViewDataSource;
                gdvAnnouncementList.EmptyDataText = GetTextByKey("per_permitList_messagel_noRecord");
                gdvAnnouncementList.DataBind();
            }
        }

        /// <summary>
        /// Data row binding.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void AnnouncementList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink lnkView = (HyperLink)e.Row.FindControl("lnkAnnouncementPart");
                HyperLink lnkViewTitle = (HyperLink)e.Row.FindControl("lnkAnnouncementTitle");
                AccelaDateLabel lblUpdatedTime = (AccelaDateLabel)e.Row.FindControl("lblUpdatedTime");
                AccelaLinkButton btnDelete = (AccelaLinkButton)e.Row.FindControl("btnDelete");
   
                string announcementId = DataBinder.Eval(e.Row.DataItem, "AnnouncementId").ToString();
                string announcementContentFull = DataBinder.Eval(e.Row.DataItem, "AnnouncementContentFull").ToString();   
                string announcementContentTitle = DataBinder.Eval(e.Row.DataItem, "AnnouncementContentTitle").ToString();
                string isRead = DataBinder.Eval(e.Row.DataItem, "IsRead").ToString();
                lnkViewTitle.Text = ScriptFilter.RemoveHTMLTag(announcementContentTitle);
                announcementContentFull = ScriptFilter.EncodeJson(announcementContentFull);
                announcementContentTitle = ScriptFilter.EncodeJson(announcementContentTitle);
                lnkView.Style.Add("cursor", "pointer");
                lnkView.Attributes.Add("onClick", "ShowDetailWindowInList('" + announcementContentFull + "','" + announcementId + "','" + announcementContentTitle + "','" + isRead.ToLower() + "');return false;");
                lnkViewTitle.Style.Add("cursor", "pointer");
                lnkViewTitle.Attributes.Add("onClick", "ShowDetailWindowInList('" + announcementContentFull + "','" + announcementId + "','" + announcementContentTitle + "','" + isRead.ToLower() + "');return false;");
                
                btnDelete.Attributes.Add("onclick", string.Format("javascript:return confirm('{0}')", GetTextByKey("aca_message_confirm_removeannouncement")));

                if (isRead.ToLower() == "true")
                {
                    lnkView.Font.Bold = false;
                    lnkViewTitle.Font.Bold = false;
                    lblUpdatedTime.Font.Bold = false;
                    btnDelete.Font.Bold = false;
                }
                else
                {
                    lnkView.Font.Bold = true;
                    lnkViewTitle.Font.Bold = true;
                    lblUpdatedTime.Font.Bold = true;
                    btnDelete.Font.Bold = true;
                }
            }
        }
         
        /// <summary>
        /// Raises the <c>System.Web.UI.Control.Init</c> event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvAnnouncementList, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Raises the register button event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void RefreshAnnouncementListButton_Click(object sender, EventArgs e)
        {
            SetAnnouncementList();
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Set announcement list according to data source of session.
        /// </summary>
        private void SetAnnouncementList()
        {
            List<AnnouncementModel> announcements = GetAnnoucementListFromSession();
            if (!AppSession.IsAdmin)
            {
                IAnnouncementBll announcementBll = (IAnnouncementBll)ObjectFactory.GetObject(typeof(IAnnouncementBll));
                BindAnnouncements(announcementBll.ConvertAnnouncementModelToDataTable(announcements));
            }
            else
            {
                BindAnnouncements(null);
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
        /// Remove a announcement from list
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="dataItemIndex">the index a announcement record in list</param>
        private void DeleteAnncouncement(object sender, int dataItemIndex)
        {
            if (GridViewDataSource != null)
            {
                DataRow drAnnouncement = GridViewDataSource.Rows[dataItemIndex];
                string announcementId = drAnnouncement["AnnouncementID"].ToString();
                IAnnouncementBll announcementBll = (IAnnouncementBll)ObjectFactory.GetObject(typeof(IAnnouncementBll));
                string annAgencyCode = AppSession.GetAnnouncementsFromSession().Find(o => o.AuditID.ToString() == announcementId).AnnouncementAgencyCode;
                announcementBll.DeleteAnnouncementFromServer(announcementId, annAgencyCode);
                DeleteAnnouncementFromSession(announcementId);
                SetAnnouncementList();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowAnnouncementInit", "ShowAnnouncementInit();", true);
            }
        }

        #endregion Private Methods
    }
}