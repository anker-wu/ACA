<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnnouncementOnNavigation.ascx.cs"
    Inherits="Accela.ACA.Web.Component.AnnouncementOnNavigation" %>
<div class="ACA_FRight">
    <div>
        <table role='presentation' border='0' cellspacing='0' cellpadding='0'>
            <tr>
                <td class="ACA_TabRow_Line">
                    <span class="ACA_FLeft"><a id="A2" runat="server" href="~/Announcement/AnnouncementList.aspx">
                        <ACA:AccelaLabel runat="server" ID="lblAnnounementLink" IsDisplaySuperAgencyText="true"
                            LabelKey="aca_announcements_navigation_link" LabelType="BodyText"></ACA:AccelaLabel></a></span><span
                                class="ACA_FLeft" id="spanAnnouncementCount" style="display: none"><a id="A3" runat="server"
                                    href="~/Announcement/AnnouncementList.aspx">(<span id="announcementCount"></span>)</a></span>
                </td>
                <td valign="top" class="ACA_TabRow_Line">
                    <div id="divExpandAnnouncement" style="display: block;">
                        <a id="A4" href="javascript:void(0)" <% if (!AppSession.IsAdmin){%>onclick="OpenAnnouncementAuto(); return false;"
                            <%} %> class="nav_more_arrow NotShowLoading" title="<%=GetTitleByKey("img_alt_announcement_arrow","") %>">
                            <img alt="<%=GetSuperAgencyTextByKey("img_alt_announcement_arrow") %>" src="<%=ImageUtil.GetImageURL("caret_expanded.gif") %>" /></a>
                    </div>
                </td>
                <td class="ACA_TabRow_Line">
                    <span ID="lblSplitAnnouncement" runat="server">&nbsp; | &nbsp;</span>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <div id='divAnnouncement' class="ACA_Anouncement_Popup_Window ACA_Report_Menu">
            <div id="announcementId" class="ACA_Hide">
            </div>
            <div id="announcementContentFull" class="ACA_Hide">
            </div>
            <div id="announcementContentTitle" class="ACA_Hide">
            </div>
            <div id="announcementContentPart" class="ACA_Anouncement_Poupu_Window_Part_Height">
            </div>
            <div class="ACA_Anouncement_Poupu_Window_MarkAsRead">
                <div id="closeMarkAsReadOfAnnouncementBtn" class="ACA_FRight">
                    <a href="javascript:void(0);" onclick="MarkAsReadOfAnnouncement(); return false;" class="ACA_LinkButton NotShowLoading">
                        <%=GetSuperAgencyTextByKey("aca_announcement_markasread_button") %></a></div>
            </div>
        </div>
    </div>
</div>