<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FaceBookWrapperPage.aspx.cs"
    Inherits="Accela.ACA.Web.FaceBookWrapperPage" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Common" %>
<%@ Register Src="~/Component/GlobalSearchConditions.ascx" TagName="GlobalSearchConditions"
    TagPrefix="uc1" %>
<%@ Register Src="~/Component/CapList.ascx" TagName="PermitList" TagPrefix="uc1" %>
<%@ Register TagPrefix="ACA" TagName="TabLinkList" Src="~/Component/TabLinkList.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:fb="https://www.facebook.com/2008/fbml">
<head id="Head1" runat="server">
    <title>Facebook Wrapper Page</title>
</head>
<body>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/jquery.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/jquery.corner.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/GlobalConst.aspx")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/global.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/ProcessLoading.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/GlobalSearch.js")%>"></script>
    <script type="text/javascript">
        $.global.isRTL = IsTrue('<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft %>');
        $.global.isAdmin = IsTrue('<%= AppSession.IsAdmin %>');
    </script>
    <script type="text/javascript" src="//connect.facebook.net/en_US/all.js#xfbml=1&appId=<%=ConfigManager.FacebookAppId %>"></script>
    <script type="text/javascript">
        //init Facebook sdk object.
        window.fbAsyncInit = function() {
            if (!$.global.isAdmin) {
                FB.init({
                    appId: '<%=ConfigManager.FacebookAppId %>',
                    frictionlessRequests: true
                });
            }
        };

        //invite the friends. 
        function sendRequestViaMultiFriendSelector() {
            processLoading.showLoading();
            var loading = $('#' + processLoading.loadingId);
            var loadingMask = $('#' + processLoading.loadingMaskId);
            if ($.exists(loading)) {
                loading.hide();
            }
            FB.ui({method: 'apprequests',
                title:'<%=InviteFiendsDialogTitle %>',
                message: '<%=LabelUtil.GetTextByKey("aca_fb_wrapper_page_invite_friend_request_instruction_msg",string.Empty).Replace("'","\\'") %>'
            }, function(){
                if ($.exists(loadingMask)) {
                    loadingMask.hide();
                }
            });
        }
    </script>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnableScriptGlobalization="true" EnableScriptLocalization="true"
        runat="server" EnablePageMethods="true" ScriptMode="Release" />
    <div id="fb-root">
    </div>
    <div id="fb_home_frame">
        <div id="aca_wrapper">
            <div id="divBanner" class="banner">
            </div>
            <div id="dvGlobalSearchContainer" class="globalsearch-container" runat="server">
                <div class="globalsearch-wrapper">
                    <uc1:GlobalSearchConditions ID="ucGlobalSearch" runat="server" />
                </div>
            </div>
            <div class="ACA_Row"></div>
            <table border="0" class="aca_wrapper" cellpadding="0" cellspacing="0" role="presentation">
                <tr>
                    <td class="fb_wrapper_left_td">
                        <div id="divLeft">
                            <div class="div_description">
                                <ACA:AccelaLabel ID="lblWelcomeInfo" LabelKey="aca_fb_wrapper_page_welcome_label"
                                    LabelType="BodyText" runat="server"></ACA:AccelaLabel>
                            </div>
                            <div class="servicelist-header">
                                <ACA:AccelaLabel ID="com_welcome_text_startInfo" LabelKey="aca_fb_wrapper_page_select_service_label"
                                    runat="server" LabelType="LabelText"></ACA:AccelaLabel>
                            </div>
                            <div id="divContentLink" runat="server">
                                <div>
                                    <!-- start custom content -->
                                    <ACA:TabLinkList ID="TabDataList" runat="server"></ACA:TabLinkList>
                                    <!-- end custom content -->
                                </div>
                            </div>
                            <br />
                            <!--my share list start-->
                            <div class="mysharedlist-header">
                                <ACA:AccelaLabel ID="AccelaLabel1" LabelKey="aca_fb_wrapper_page_my_shared_list_label"
                                    runat="server" LabelType="LabelText"></ACA:AccelaLabel>
                            </div>
                            <uc1:PermitList ID="gdvSocialMediaList" IsHideMap="true" AutoGenerateCheckBoxColumn="false"
                                IsForLicensee="true" ShowPermitAddress="true" runat="server" GViewID="60138" OnGridViewDownloadAll="SocialMediaList_GridViewDownloadAll" />
                            <!--end my share list-->
                        </div>
                    </td>
                    <td class="divSpliter">
                    </td>
                    <td class="fb_wrapper_right_td">
                        <table class="profile" border="0" cellpadding="0" cellspacing="0" role="presentation">
                            <tr>
                                <td id="tdAnnouncement" class="profile-settings" valign="middle" align="center" runat="server">
                                    <a href="../Announcement/Announcementlist.aspx">
                                        <img alt="<%=ScriptFilter.RemoveHTMLTag(GetTextByKey("aca_announcements_navigation_link")) %>"
                                            title="<%=ScriptFilter.RemoveHTMLTag(GetTextByKey("aca_announcements_navigation_link")) %>"
                                            src="<% =ImageUtil.GetImageURL("u13_normal.png") %>" />
                                    </a>
                                </td>
                                <td id="tdSpliter1" class="fb_wrapper_nav_split" runat="server">
                                    <div class="profile-spliter">
                                    </div>
                                </td>
                                <td id="tdCollection" class="profile-settings" valign="middle" align="center" runat="server">
                                    <a href="../MyCollection/MyCollectionManagement.aspx">
                                        <img alt="<%=ScriptFilter.RemoveHTMLTag(GetTextByKey("mycollection_global_label_collection")) %>"
                                            title="<%=ScriptFilter.RemoveHTMLTag(GetTextByKey("mycollection_global_label_collection")) %>"
                                            src="<% =ImageUtil.GetImageURL("u11_normal.png") %>" />
                                    </a>
                                </td>
                                <td id="tdSpliter2" class="fb_wrapper_nav_split" runat="server">
                                    <div class="profile-spliter">
                                    </div>
                                </td>
                                <td id="tdAccountManager" class="profile-settings" valign="middle" align="center"
                                    runat="server">
                                    <a href="../Account/AccountManager.aspx">
                                        <img alt="<%=ScriptFilter.RemoveHTMLTag(GetTextByKey("com_headNav_label_accountManagement")) %>"
                                            title="<%=ScriptFilter.RemoveHTMLTag(GetTextByKey("com_headNav_label_accountManagement")) %>"
                                            src="<% =ImageUtil.GetImageURL("u15_normal.png") %>" />
                                    </a>
                                </td>
                            </tr>
                        </table>
                        <div id="divFriendList">
                            <div class="friendlist-header">
                                <ACA:AccelaLabel ID="lblApp" LabelKey="aca_fb_wrapper_page_friend_list_label" runat="server"></ACA:AccelaLabel>
                            </div>
                        </div>
                        <div id="divRight">
                            <div>
                                <ul class="fb_wrapper_friend_list_ul">
                                    <asp:DataList ID="dlFriends" RepeatColumns="3" runat="server" role='presentation'>
                                        <ItemTemplate>
                                            <li class="fb_wrapper_friend_list_li">
                                                <img src='https://graph.facebook.com/<%# DataBinder.Eval(Container.DataItem, "id") %>/picture?<%=Session[SessionConstant.SESSION_SOCIAL_MEDIA_ACCESS_TOKEN]%>'
                                                    alt='<%# DataBinder.Eval(Container.DataItem, "name") %>' title='<%#DataBinder.Eval(Container.DataItem, "name") %>' />
                                            </li>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </ul>
                            </div>
                            <div class="ACA_Row">
                            </div>
                            <div>
                                <div id="div-invitefriend-button" class="fb_button_container">
                                    <ACA:AccelaLinkButton ID="btnInvFriends" OnClientClick="sendRequestViaMultiFriendSelector(); return false;"
                                        CssClass="uiButtonConfirm uiButton" runat="server" LabelKey="aca_fb_wrapper_page_invite_friends_label"
                                        DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"></ACA:AccelaLinkButton>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <iframe width="0" height="0" id="iframeExport" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>"><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
    <div id="divLoadingTemplate" class="ACA_Loading_Message" style="width: auto">
        <img alt="<%=GetTextByKey("aca_global_msg_loading") %>" src="<%=ImageUtil.GetImageURL("loading.gif") %>" />
    </div>
    <script type="text/javascript">
        with (Sys.WebForms.PageRequestManager.getInstance()) {
            if ('<%=AppSession.IsAdmin%>'.toLowerCase() == 'false') {
                add_pageLoaded(onPageLoaded);
                add_endRequest(onEndRequest);
            }
        }

        var processLoading = new ProcessLoading();
        function onPageLoaded(sender, args) {
            processLoading.initControlLoading();
            //addPrintErrors2SubmitButton();
        }

        function onEndRequest(sender, arg) {
            var sourceElement = sender._postBackSettings.sourceElement;
            if (sourceElement == null || (sourceElement != null && $(sourceElement).hasClass('NotShowLoading') == false)) {
                processLoading.hideLoading();
            }

            //export file.
            ExportCSV(sender, arg);
        }

    </script>
    </form>
</body>
</html>
