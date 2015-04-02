#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FaceBookWrapperPage.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 *
 *  Description:
 *
 * </pre>
 */
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.SocialMedia;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.AppCode.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.SocialMedia;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web
{
    /// <summary>
    /// facebook user's home page
    /// </summary>
    public partial class FaceBookWrapperPage : BasePageWithoutMaster
    {
        #region Fields

        /// <summary>
        /// the Facebook invite dialog title field.
        /// </summary>
        private string _inviteFiendsDialogTitle;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the name of the facebook user.
        /// </summary>
        /// <value>The name of the facebook user.</value>
        public string FacebookUserName
        {
            get
            {
                FacebookUser facebookUser = Session[SessionConstant.FACEBOOK_USER] as FacebookUser;

                if (facebookUser == null)
                {
                    return string.Empty;
                }

                return facebookUser.Name;
            }
        }

        /// <summary>
        /// Gets Invite Friends dialog title
        /// </summary>
        protected string InviteFiendsDialogTitle
        {
            get
            {
                return _inviteFiendsDialogTitle;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the <c>PreInit</c> event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = I18nCultureUtil.UserPreferredCulture;
        }

        /// <summary>
        /// On Initial event
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //add the special custom css.
            HtmlHead header;

            if (this.Master != null)
            {
                header = this.Master.FindControl("Head1") as HtmlHead;
            }
            else
            {
                header = this.Header;
            }

            if (header != null)
            {
                HtmlGenericControl cssWrapperFile = new HtmlGenericControl("link");
                cssWrapperFile.ID = "FacebookWrapperCssStyle";
                cssWrapperFile.Attributes["type"] = "text/css";
                cssWrapperFile.Attributes["rel"] = "stylesheet";
                cssWrapperFile.Attributes["href"] = Page.ResolveUrl("~/App_Themes/SocialMedia/FBWrapper.css");

                if (header.FindControl(cssWrapperFile.ID) == null)
                {
                    header.Controls.Add(cssWrapperFile);
                }
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (AppSession.IsAdmin)
            {
                gdvSocialMediaList.BindCapList(null);
                return;
            }

            string userAccessToken = SocialMediaUtil.GetAccessToken(string.Empty, false);

            if (string.IsNullOrEmpty(userAccessToken))
            {
                Response.Redirect(SocialMediaUtil.FacebookLoginUrl, true);
            }
            
            List<FacebookFriend> fbfriends = new List<FacebookFriend>();

            string fbAppUrl = "https://graph.facebook.com/" + ConfigManager.FacebookAppId + "?";

            FacebookApplication fbApp = SocialMediaUtil.GetObjectSocialMedia<FacebookApplication>(fbAppUrl, string.Empty);
            _inviteFiendsDialogTitle = string.Format(LabelUtil.GetGUITextByKey("aca_fb_wrapper_page_invite_friend_request_title_label"), fbApp.Name);
            string fbProfileUrl = "https://graph.facebook.com/me/friends?fields=installed,name&";

            GetAllUsingAppFriends(ref fbfriends, fbProfileUrl, string.Empty);
            dlFriends.DataSource = fbfriends;
            dlFriends.DataBind();

            if (!GlobalSearchUtil.IsGlobalSearchEnabled())
            {
                dvGlobalSearchContainer.Visible = false;
            }

            gdvSocialMediaList.ExportFileName = "MySharedList";
            gdvSocialMediaList.InitialExport(StandardChoiceUtil.IsEnableExport2CSV());

            if (!IsPostBack)
            {
                QueryMyPermit(0, null);
            }

            //set announcement visiblity
            SetAnnouncementVisiblity();

            //set accoument managemente visiblity
            SetAccountManagementVisiblity();
        }

        /// <summary>
        /// Handles the GridViewDownloadAll event of the <c>gdvSocialMediaList</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        protected void SocialMediaList_GridViewDownloadAll(object sender, GridViewDownloadEventArgs e)
        {
            GridViewBuildHelper.DownloadAll(sender, e, GetPermitGCByQueryFormat);
        }

        /// <summary>
        /// PermitList GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void PermitList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            //when user trigger this Gridview pageIndexChanged event, permitList.pageIndex will be set to zero
            int currentPageIndex = 0;
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(gdvSocialMediaList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;

            //when sorting gridview, we use GridViewDataSource as the data, so ,needn't pass the first parameters
            gdvSocialMediaList.BindCapList(null, currentPageIndex, e.GridViewSortExpression);
        }

        /// <summary>
        /// permit list grid view index change event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void PermitList_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(gdvSocialMediaList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                QueryMyPermit(e.NewPageIndex, pageInfo.SortExpression);
            }
        }

        /// <summary>
        /// Query my permit
        /// </summary>
        /// <param name="currentPageIndex">Index of the current page.</param>
        /// <param name="sortExpression">The sort expression.</param>
        private void QueryMyPermit(int currentPageIndex, string sortExpression)
        {
            if (AppSession.User.IsAnonymous)
            {
                return;
            }

            string[] hiddenViewEltNames = ControlBuildHelper.GetHiddenViewElementNames(gdvSocialMediaList.GViewID, ModuleName);

            ISocialMediaBll socialBll = ObjectFactory.GetObject<ISocialMediaBll>();
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(gdvSocialMediaList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = gdvSocialMediaList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            SimpleCapModel[] capResult = socialBll.GetMyShareCapList(ConfigManager.AgencyCode, hiddenViewEltNames, AppSession.User.PublicUserId, queryFormat);
            DataTable capList = PaginationUtil.MergeDataSource<DataTable>(gdvSocialMediaList.GridViewDataSource, gdvSocialMediaList.CreateDataSource(capResult), pageInfo);
            gdvSocialMediaList.BindCapList(capList);
        }

        /// <summary>
        /// Gets all using app friends.
        /// </summary>
        /// <param name="fbfriends">The facebook friends.</param>
        /// <param name="nextUrl">The next URL.</param>
        /// <param name="fbCode">The Facebook <c>OAuth Code</c>.</param>
        private void GetAllUsingAppFriends(ref List<FacebookFriend> fbfriends, string nextUrl, string fbCode)
        {
            FacebookFriends subscribers = SocialMediaUtil.GetObjectSocialMedia<FacebookFriends>(nextUrl, fbCode);

            if (subscribers != null)
            {
                fbfriends.AddRange(subscribers.Data.Where(o => o.Installed).ToList());

                if (subscribers.Paging != null && !string.IsNullOrEmpty(subscribers.Paging.Next))
                {
                    nextUrl = subscribers.Paging.Next;
                    GetAllUsingAppFriends(ref fbfriends, nextUrl, fbCode);
                }
            }
        }

        /// <summary>
        /// set announcement visibility
        /// </summary>
        private void SetAnnouncementVisiblity()
        {
            if (AppSession.User != null && !AppSession.User.IsAnonymous)
            {
                tdAnnouncement.Visible = StandardChoiceUtil.IsUseAnnouncement();
            }
            else
            {
                tdAnnouncement.Visible = false;
            }

            if (!tdAnnouncement.Visible)
            {
                tdSpliter1.Visible = false;
            }
        }

        /// <summary>
        /// set account management visibility
        /// </summary>
        private void SetAccountManagementVisiblity()
        {
            if (AppSession.User != null && !AppSession.User.IsAnonymous)
            {
                tdAccountManager.Visible = StandardChoiceUtil.IsAccountManagementEnabled();
            }
            else
            {
                tdAccountManager.Visible = false;
            }

            if (!tdAccountManager.Visible)
            {
                tdSpliter2.Visible = false;
            }
        }

        /// <summary>
        /// Get general search by query criteria
        /// </summary>
        /// <param name="queryFormat">query format</param>
        /// <returns>download result model that contains general search list</returns>
        private DownloadResultModel GetPermitGCByQueryFormat(QueryFormat queryFormat)
        {
            string[] hiddenViewEltNames = ControlBuildHelper.GetHiddenViewElementNames(gdvSocialMediaList.GViewID, ModuleName);

            ISocialMediaBll socialBll = ObjectFactory.GetObject<ISocialMediaBll>();
            SimpleCapModel[] capResult = socialBll.GetMyShareCapList(ConfigManager.AgencyCode, hiddenViewEltNames, AppSession.User.PublicUserId, queryFormat);
            DataTable capList = gdvSocialMediaList.CreateDataSource(capResult);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = queryFormat.startRow;
            model.DataSource = capList;

            return model;
        }

        #endregion
    }
}