<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.Navigation"
    CodeBehind="Navigation.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Web" %>
<%@ Import Namespace="Accela.ACA.Common.Util" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="System.Text.RegularExpressions" %>
<%@ Register Assembly="Accela.Web.Controls" Namespace="Accela.Web.Controls.Navigation"
    TagPrefix="ACA" %>
<%@ Register Src="GlobalSearchConditions.ascx" TagName="GlobalSearchConditions" TagPrefix="uc1" %>
<%@ Register Src="~/Component/AnnouncementOnNavigation.ascx" TagName="AnnouncementOnNavigation" TagPrefix="uc1" %>
<script src="<%=FileUtil.AppendApplicationRoot("Scripts/Announcement.js") %>" type="text/javascript"></script>
<script language="javascript" type="text/javascript">

    function RedirectTo(url) {
        location.href = '<%=FileUtil.ApplicationRoot %>' + url;
        return false;
    }

    function showTabPropertyPanel(navBar) {
        parent.LoadTabInfo(navBar);
        parent.LoadEastPanelTitle(2);
    }

    function updateCartNumber(cartNumber) {
        $get("<%=lblShoppingCart.ClientID %>").innerHTML = cartNumber;
    }

    // hide report link and report arrow if no reports available.
    function hideReportLink(hasLogin) {
        var reportLinkId = hasLogin ? 'reportLink2' : 'reportLink';

        $get(reportLinkId).style.display = 'none';
    }

    var menu;
    var reportMenuTimeOutId;
    var menuId;
    var isRTL = false;   // in Arabic, it's true

    // bind reports menu
    function bindReports(reports, hasLogin, reportLabel) {
        if ($.global.isAdmin || typeof (reports) == "undefined" || reports == null) {
            return;
        }

        var targetId;
        //var isRTL = false;   // in Arabic, it's true

        if (!hasLogin) {
            menuId = 'reportList';
            targetId = 'reportLink';
            $get('<%=lblReports.ClientID %>').innerHTML = reportLabel;
        }
        else {
            menuId = 'reportList2';
            targetId = 'reportLink2';
            $get('<%=lblReports2.ClientID %>').innerHTML = reportLabel;
        }

        if ('<%=I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft%>' == 'True') {
            isRTL = true;
        }

        var prop = [['isRTL', isRTL], ['align', 'right'], ['menuId', menuId], ['targetId', targetId], ['sign', 'report_menu']];
        var items = reports;
        var events = [[['eventType', 'click'], ['func', openReport]]];

        menu = new Menu(items, prop, events);
    }

    // when clicking report link, show report list and set time out 
    function showReports() {
        CheckAndSetNoAsk();
        if (menu == undefined) return;
        $get(menuId).style.display = 'block';
        menu.showMenu();

        hideReportByTimer();

        //when click report link or a report, don't need pop up the query window.
        SetNotAskWhenClick();
    }

    if ($.global.isAdmin == false && '<%=UseAnnouncement %>'.toLowerCase() == 'true') {
        if ('<%=AnnouncementInterval %>' != null && '<%=AnnouncementInterval %>' != '') {
            setInterval("ShowAnnouncementByTimer()", '<%=AnnouncementInterval %>' * 60000);
        }
        else {
            setInterval("ShowAnnouncementByTimer()", 5*60000);
        }
    }

    function hideReportByTimer() {
        clearTimeout(reportMenuTimeOutId);
        reportMenuTimeOutId = null;
        reportMenuTimeOutId = setTimeout(hideReport, 3000);
    }


    function SetFocusIn() {
        clearTimeout(reportMenuTimeOutId);
        reportMenuTimeOutId = null;
    }

    function SetBlurOut() {
        hideReportByTimer();
    }

    function SetNotAskWhenClick() {
        if (typeof (SetNotAsk) != 'undefined') {
            SetNotAsk();
        }
    }

    function onMouseOver() {
        if (menu) {
            menu.showMenu();
            clearTimeout(reportMenuTimeOutId);
            reportMenuTimeOutId = null;
        }
    }

    function onMouseOut() {
        var e = getEvent();
        var el = $get(menuId);
        var target = e.target || e.srcElement;
        var relatedTarget = e.relatedTarget || e.toElement;

        if (relatedTarget == undefined) {
            return;
        }
        //closeMenu();
        clearTimeout(reportMenuTimeOutId);
        reportMenuTimeOutId = null;
        reportMenuTimeOutId = setTimeout(hideReport, 700);
    }

    var isMultiWindow = false;
    var url;

    function openReport(e) {
        var obj = e.srcElement ? e.srcElement : e.target;
        if (obj.nodeName != "A")  // if click img or space, don't open parameter window
        {
            return;
        }
        isMultiWindow = this.isMultiWindow;
        url = this.url;
        // do post back for getting the latest cap ids.
        __doPostBack('<%=btnPostForReport.ClientID %>');
    }

    function openParamWin(altIDs) {
        var IDs = altIDs;
        var idStr = '';
        // the requried number of parameter windows. 
        // If single window or no alt ids, open a window. 1 means one window
        var openNumbers = (IDs.length > 0 && isMultiWindow) ? IDs.length : 1;

        // if single window, concat the cap alt ids with ",".
        if (IDs.length > 0 && openNumbers == 1) {
            for (var i = 0; i < IDs.length; i++) {
                idStr += IDs[i] + ",";
            }
            idStr = idStr.substring(0, idStr.length - 1);  // remove the last ","         
        }

        var paramWindows = new Array(); // use the array to store the opened windows.

        for (var i = 0; i < openNumbers; i++) {
            var newUrl = url;
            if (IDs.length > 0) {
                var id = openNumbers > 1 ? IDs[i] : idStr;  // if multiple window, pass each id to each window.
                //newUrl = url + "&ID=" + id;
                var appendUrl = String.format("&{0}={1}", '<%=ACAConstant.ID %>', id)
                newUrl = url + appendUrl;
            }

            var iLeft = 200;
            if (i > 0) {
                // resize the window. Set it padding 50px to the previous window. 
                // if the open window is more than 5(i>3), set it overlapped the previous window(i=3).
                var m = i > 3 ? 3 : i - 1;
                var preWinLeft = paramWindows[m].screenLeft == undefined ?
                    paramWindows[m].screenX : paramWindows[m].screenLeft;  //FF use screeX

                iLeft = isRTL ? preWinLeft - 50 : preWinLeft + 50;  //if in Arabic, set the next window padding right the previous one.
            }

            var parameters =
            String.format("top=200,left={0},height=600,width=800,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes", iLeft);

            paramWindows[i] = window.open(newUrl, "_blank", parameters);
        }

        hideReport();

        SetNotAskWhenClick();
    }

    function hideReport() {
        $GetObject(menuId).style.display = 'none';
    } 
</script>

<div id="overlayAnnouncement" class="ACA_Announcement_Window_Detail_Overlayer"></div>
<div id="winAnnouncement" class="ACA_Hide PopUpDlg ACA_Announcement_Window">
  
    <a id='lnkBeginFocusAnn' class="NullBlock NotShowLoading" href='#' onkeydown='OverrideTabKey(event, true, "closeWinAnn")' title="<%=GetTextByKey("aca_announcement_popup_window_description") %>">
        <img src='<%=ImageUtil.GetImageURL("spacer.gif") %>' class='ACA_NoBorder' alt="" />
    </a> 
     
    <div id="announcementWindowHeader" class="ACA_Announcement_Window_Header">
        <span id="close">
            <span class="ACA_AlignRightOrLeft CloseImage ACA_FRight" >        
                <input id="closeWinAnn" onclick="return false;" type="image" src='<%=ImageUtil.GetImageURL("closepopup.png") %>' class="ACA_ActionIMG" alt="<%=GetTextByKey("aca_common_close") %>" title="<%=GetTextByKey("aca_common_close") %>"/>
            </span>
        </span>
    </div>
    <div class="ACA_Announcement_Window_Detail">
        <div id="winAnnouncementContentTitle" class="ACA_Popup_Title ACA_Announcement_Title_Center"></div> 
        <div class="ACA_TabRow ACA_Line_Content"></div>
        <div id="winAnnouncementContentAll" class="ACA_Announcement_Content"></div> 
    </div>
    
    <a id='lnkEndFocusAnn' class="NullBlock NotShowLoading" href='#' onkeydown='OverrideTabKey(event, false, "lnkBeginFocusAnn")'title="<%=GetTextByKey("img_alt_form_end") %>">
        <img src='<%=ImageUtil.GetImageURL("spacer.gif") %>' class='ACA_NoBorder' alt=""  />
    </a>
</div>

<!-- Start HeadNavigation Block -->
<ACA:AccelaHeightSeparate ID="hsLine" runat="server" Height="2" />
<div id="divNavigation" runat="server">
    <div class="ACA_NaviTitle">
        <div class="ACA_RegisterLogin ACA_RegisterLogin_FontSize">
            <table role='presentation' border="0" cellpadding="0" cellspacing="0">
                <tr valign="bottom">
                    <td>
                        <div id="afterLogin" runat="server" visible="false">
                            <!--Logout link-->
                            <div class="ACA_FRight">
                                <table role='presentation' border='0' cellspacing='0' cellpadding='0'>
                                    <tr>
                                        <td>
                                            <a id="btnLogout" href="~/Logout.aspx" runat="server">
                                                <ACA:AccelaLabel ID="com_headNav_label_logout" IsDisplaySuperAgencyText="true" LabelKey="com_headNav_label_logout"
                                                    LabelType="bodyText" runat="server"></ACA:AccelaLabel>
                                            </a>
                                        </td>
                                        <td class="ACA_TabRow_Line">
                                            &nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!--Logout link-->
                            <% if (AppSession.IsAdmin || (AppSession.User != null && (AppSession.User.IsAgentClerk || AppSession.User.IsAuthorizedAgent)))
                            {%>
                            <!--Search Cutomer Link for authorized agent-->
                            <div class="ACA_FRight">
                                <table role='presentation' border='0' cellspacing='0' cellpadding='0'>
                                    <tr>
                                        <td class="ACA_TabRow_Line">
                                            <a id="A2" class="NotShowLoading" href="~/AuthorizedAgent/FishingAndHunttingLicense/SearchCustomer.aspx" runat="server">
                                                <ACA:AccelaLabel ID="lblSearchCustomer" IsDisplaySuperAgencyText="true"
                                                    LabelKey="aca_auth_agent_navigation_label_search_customer" LabelType="bodyText" runat="server"></ACA:AccelaLabel>
                                            </a>
                                        </td>
                                        <td class="ACA_TabRow_Line">
                                            <ACA:AccelaLabel ID="lblSpaceHolder" IsNeedEncode="false" runat="server">&nbsp; | &nbsp;</ACA:AccelaLabel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!--Search Cutomer Link for authorized agent-->
                            <% }%>
                            <!--Account manager-->
                            <div class="ACA_FRight">
                                <table role='presentation' border='0' cellspacing='0' cellpadding='0'>
                                    <tr>
                                        <td class="ACA_TabRow_Line">
                                            <a id="managerURL" href="~/Account/AccountManager.aspx" runat="server">
                                                <ACA:AccelaLabel ID="com_headNav_label_accountManagement" IsDisplaySuperAgencyText="true"
                                                    LabelKey="com_headNav_label_accountManagement" LabelType="bodyText" runat="server"></ACA:AccelaLabel>
                                            </a>
                                        </td>
                                        <td class="ACA_TabRow_Line">
                                            <ACA:AccelaLabel ID="lblSplit3" IsNeedEncode="false" runat="server">&nbsp; | &nbsp;</ACA:AccelaLabel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!--Account manager-->
                            <!--Report-->
                            <div class="ACA_FRight">
                                <div>
                                    <table role='presentation' border='0' cellspacing='0' cellpadding='0'>
                                        <tr id='reportLink2'>
                                            <td class="ACA_TabRow_Line">
                                                <a href="javascript:void(0);" onclick="showReports();" class="nav_more_arrow ACA_Report_Arrow NotShowLoading" title="<%=GetTitleByKey("img_alt_report_arrow","") %>">
                                                    <ACA:AccelaLabel ID="lblReports2" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                                                    <ACA:AccelaLabel ID="lblAdminReport2" runat="server" LabelType="bodyText"></ACA:AccelaLabel>
                                                    <img alt="<%=GetSuperAgencyTextByKey("img_alt_report_arrow") %>" src="<%=ImageUtil.GetImageURL("caret_expanded.gif") %>"/>
                                                </a>
                                            </td>
                                            <td class="ACA_TabRow_Line">
                                                <span id="reportSplit2" isneedencode="false" runat="server">&nbsp; | &nbsp;</span>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style='display: none; position: absolute; z-index: 2;'>
                                    <div id='reportList2' class='ACA_Report_Menu ActionMenu_Link' onmouseover="onMouseOver();"
                                        onmouseout="onMouseOut();">
                                    </div>
                                </div>
                            </div>
                            <!--Report-->
                            <!--Shopping Cart-->
                            <div class="ACA_FRight" runat="server" id="divShoppingCart">
                                <table role='presentation' border='0' cellspacing='0' cellpadding='0'>
                                    <tr>
                                        <td class="ACA_TabRow_Line">
                                            <img id="imgShoppingCart" src="<%=ImageUtil.GetImageURL("shoppingcart.png") %>" alt="<%=GetTextByKey("aca_shoppingcart_msg_alt") %>"
                                                style="border-width: 0px; top: 0px;" />
                                        </td>
                                        <td class="ACA_TabRow_Line">
                                            <a id="A1" href="~/ShoppingCart/ShoppingCart.aspx?TabName=Home&stepNumber=2" runat="server">
                                                <ACA:AccelaLabel ID="lblShoppingCart" LabelType="bodyText" IsDisplaySuperAgencyText="true"
                                                    LabelKey="com_headNav_label_carttitle" runat="server"></ACA:AccelaLabel>
                                            </a>
                                        </td>
                                        <td class="ACA_TabRow_Line">
                                            <ACA:AccelaLabel ID="lblSplitShoppingCart" IsNeedEncode="false" runat="server">&nbsp; | &nbsp;</ACA:AccelaLabel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!--Shopping Cart-->
                            <!--Collection-->
                            <% if (!AppSession.User.IsAgentClerk && !AppSession.User.IsAuthorizedAgent)
                                {%>
                            <div class="ACA_FRight">
                                <table role='presentation' id='tbCollection' border='0' cellspacing='0' cellpadding='0'>
                                    <tr>
                                        <td class="ACA_TabRow_Line">
                                            <a href="<%=RedirectCollectionManagementPage %>">
                                                <ACA:AccelaLabel ID="lblMyCollection" IsDisplaySuperAgencyText="true" LabelKey="mycollection_global_label_collection"
                                                    LabelType="bodyText" runat="server">
                                                </ACA:AccelaLabel></a>
                                        </td>
                                        <td class="ACA_TabRow_Line">
                                            <% if (!AppSession.IsAdmin && !IsAmountEqualZero)
                                               {%>
                                            <a href="javascript:void(0);" onclick="showMenu(this)" class="nav_more_arrow NotShowLoading"
                                                title="<%=GetTitleByKey("img_alt_mycollection_button","") %>">
                                                <img id="imgMyCollection" style="cursor: pointer; border-width: 0px" alt="<%=GetSuperAgencyTextByKey("img_alt_mycollection_button") %>"
                                                    src="<%=CollectionImgUrl %>" /></a>
                                            <% }%>
                                        </td>
                                        <td class="ACA_TabRow_Line">
                                            <span id="collectionSplit" isneedencode="false" runat="server">&nbsp; | &nbsp;</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style='display: none; position: absolute; z-index: 2;'>
                                                <div id='dropmenu' class='collection_dropmenu ActionMenu_Link'>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <% }%>
                            <!--Collection-->
                            <!--Support Accessibility-->
                            <div class="ACA_FRight" runat="server" id="divAccessibilityAfter" visible="false">
                                <table role='presentation' border='0' cellspacing='0' cellpadding='0'>
                                    <tr>
                                        <td class="ACA_TabRow_Line">
                                            <input type="checkbox" id="chkAccessibilityAfter" runat="server" class="NAVIGATION_INPUT_CHKBOX" />
                                        </td>
                                        <td class="ACA_TabRow_Line">
                                            <ACA:AccelaLabel ID="lblAccessibilityAfter" IsDisplaySuperAgencyText="true" LabelKey="aca_daily_accessibility"
                                                runat="server" CssClass="ACA_Header_Row" LabelType="BodyText" AssociatedControlID="chkAccessibilityAfter"></ACA:AccelaLabel>
                                        </td>
                                        <td class="ACA_TabRow_Line">
                                            <span id="accessibilitySplit" isneedencode="false" runat="server">&nbsp; | &nbsp;</span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!--Support Accessibility-->
                            <div class="ACA_FRight">
                                <table role='presentation' border='0' cellspacing='0' cellpadding='0'>
                                    <tr>
                                        <td class="ACA_TabRow_Line">
                                            <ACA:AccelaLabel ID="com_headNav_label_loggedinas" IsDisplaySuperAgencyText="true"
                                                LabelKey="com_headNav_label_loggedinas" runat="server" LabelType="BodyText"></ACA:AccelaLabel>
                                        </td>
                                        <td class="ACA_TabRow_Line">
                                            <strong>
                                                <ACA:AccelaLabel ID="lblUserName" IsDisplaySuperAgencyText="true" LabelKey="aca_user_name"
                                                    runat="server" LabelType="BodyText"></ACA:AccelaLabel>
                                            </strong>
                                        </td>
                                        <td class="ACA_TabRow_Line">
                                            &nbsp; | &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!--Announcement-->
                            <% if (AppSession.IsAdmin || UseAnnouncement)
                            {%>
                            <uc1:AnnouncementOnNavigation runat="server" ID="ucAnnAfterLogin" />
                            <%} %>
                        </div>
                        <div id="beforeLogin" runat="server" visible="false">
                            <!--Login link-->
                            <div class="ACA_FRight">
                                <table role='presentation' border='0' cellspacing='0' cellpadding='0'>
                                    <tr>
                                        <td>
                                            <a id="btnLogin" runat="server" href="~/Login.aspx">
                                                <ACA:AccelaLabel ID="lblLogin" LabelType="BodyText" IsDisplaySuperAgencyText="true"
                                                    LabelKey="com_headNav_label_login" runat="server" />
                                            </a>
                                        </td>
                                        <td class="ACA_TabRow_Line">
                                            &nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!--Login link-->
                            <!--Report-->
                            <div class="ACA_FRight">
                                <div>
                                    <table role='presentation' border='0' cellspacing='0' cellpadding='0'>
                                        <tr id='reportLink'>
                                            <td class="ACA_TabRow_Line">
                                                <a href="javascript:void(0);" onclick="showReports();" class="nav_more_arro NotShowLoading">
                                                    <ACA:AccelaLabel ID="lblReports" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                                                    <ACA:AccelaLabel ID="lblAdminReports" LabelType="BodyText" runat="server" />
                                                    <img alt="<%=GetSuperAgencyTextByKey("img_alt_report_arrow") %>" src="<%=ImageUtil.GetImageURL("caret_expanded.gif") %>"
                                                        style="border-width: 0px;" />
                                                </a>
                                            </td>
                                            <td class="ACA_TabRow_Line">
                                                <span id="reportSplit" isneedencode="false" runat="server">&nbsp; | &nbsp;</span>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style='display: none; position: absolute; z-index: 2;'>
                                    <div id='reportList' class='ACA_Report_Menu ActionMenu_Link' onmouseover="onMouseOver();"
                                        onmouseout="onMouseOut();">
                                    </div>
                                </div>
                            </div>
                            <!--Report-->
                            <!--Register link-->
                            <div class="ACA_FRight">
                                <table role='presentation' border='0' cellspacing='0' cellpadding='0'>
                                    <tr>
                                        <td class="ACA_TabRow_Line">
                                            <a id="btnRegister" runat="server" href="~/Account/RegisterDisclaimer.aspx">
                                                <ACA:AccelaLabel ID="lblRegister" IsDisplaySuperAgencyText="true" LabelKey="com_headNav_label_registerAccount"
                                                    LabelType="BodyText" runat="server" />
                                            </a>
                                        </td>
                                        <td class="ACA_TabRow_Line">
                                            <ACA:AccelaLabel ID="lblSplit2" IsNeedEncode="false" runat="server">&nbsp; | &nbsp;</ACA:AccelaLabel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!--Register link-->
                            <!--Support Accessibility-->
                            <div class="ACA_FRight" runat="server" id="divAccessibilityBefore" visible="false">
                                <table role='presentation' border='0' cellspacing='0' cellpadding='0'>
                                    <tr>
                                        <td class="ACA_TabRow_Line">
                                            <input type="checkbox" id="chkAccessibilityBefore" runat="server" class="NAVIGATION_INPUT_CHKBOX" />
                                        </td>
                                        <td class="ACA_TabRow_Line">
                                            <ACA:AccelaLabel ID="lblAccessibilityBefore" IsDisplaySuperAgencyText="true" LabelKey="aca_daily_accessibility"
                                                runat="server" CssClass="ACA_Header_Row" LabelType="BodyText" AssociatedControlID="chkAccessibilityBefore"></ACA:AccelaLabel>
                                        </td>
                                        <td class="ACA_TabRow_Line">
                                            <span id="lblSplitAccessibility" runat="server">&nbsp; | &nbsp;</span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!--Support Accessibility-->
                            <!--Announcement-->
                            <% if (AppSession.IsAdmin || UseAnnouncement)
                            {%>
                            <uc1:AnnouncementOnNavigation runat="server" ID="ucAnnBeforeLogin" />
                            <%} %>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ACA_NavBanner">
            <ACA:AccelaLabel ID="lblNavBanner" LabelType="PageInstruction" IsDisplaySuperAgencyText="true" LabelKey="aca_navigation_bannerinfo"
                runat="server"></ACA:AccelaLabel>
        </div>
        <uc1:GlobalSearchConditions ID="ucGlobalSearch" runat="server" />
    </div>
    <div class="nav_bar_container">
        <ACA:AccelaDiv ID="divMenu" runat="Server">
            <div id="divNaviMenu">
                <ACA:TabBar ID="ucTabBar" runat="server" CssClass="ACA_NaviMenu" LinkClass="ACA_SubMenuList font11px">
                    <ItemTemplate>
                        <table role='presentation' tag="navbar" border="0" cellspacing="0" cellpadding="0" class='tab_bar_table ACA_Nowrap'>
            	            <tr>
            		            <td class="ACA_ItemLeft ACA_LeftOff"></td>
            		                <td class="ACA_ItemCenter ACA_ItemCenter_FontSize ACA_CenterOff">
                                        <div>
                                            <a  title="## getItem('Title') ##" href="javascript:void(0);" module="## getItem('Module') ##">## getItem('Label') ##</a>
                                        </div>
            		                </td>
            		                <td class="ACA_ItemRight ACA_RightOff"></td>
            	            </tr>
                        </table>
                    </ItemTemplate>
                    <SelectedItemTemplate>
                        <table role='presentation' tag="navbar" border="0" cellspacing="0" cellpadding="0" class='tab_bar_table ACA_Nowrap'>
            	            <tr>
            		            <td class="ACA_ItemLeft ACA_LeftOn"></td>
            		            <td class="ACA_ItemCenter ACA_ItemCenter_FontSize ACA_CenterOn">
                                    <div>
                                        <a title="## getItem('Title') ##" href="javascript:void(0);" module="## getItem('Module') ##">## getItem('Label') ##</a>
                                    </div>
            		            </td>
            		            <td class="ACA_ItemRight ACA_RightOn"></td>
            	            </tr>
                        </table>
                    </SelectedItemTemplate>
                    <MoreTemplate>
                        <Template>
                            <table role='presentation' id="span_more_tab" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td class="ACA_MoreItemLeft ACA_LeftOff">
                                    </td>
                                    <td class="ACA_MoreItemCenter ACA_CenterOff more_tab">
                                        <a href="#" onfocus="onfocusMore();" onblur="SetBlurOutForMore();" class="NotShowLoading">
                                            <ACA:AccelaLabel module="more" class='more_button font12px' IsDisplaySuperAgencyText="true" ID="lblMoreTab"
                                                runat="server" LabelKey="aca_tabbar_more_label"></ACA:AccelaLabel>
                                        </a>
                                    </td>
                                    <td class='ACA_CenterOff' style="width: 15px;">
                                        <a href="javascript:void(0)" onclick="CheckAndSetNoAsk();" id="moreArrowLink" class="nav_more_arrow NotShowLoading"
                                            title="<%=GetTitleByKey("aca_tabbar_more_label","") %>">
                                            <img alt="<%=GetTitleByKey("aca_tabbar_more_label","") %>" src="<%=ImageUtil.GetImageURL("caret_arrow.gif") %>"
                                                style="border-width: 0px;" /></a>
                                    </td>
                                    <td class="ACA_MoreItemRight ACA_RightOff">
                                    </td>
                                </tr>
                            </table>
                        </Template>
                    </MoreTemplate>
                    <LinkItemTemplate>
                        <span style="display: inline-block;">
                            <a title='## getItem('Title') ##'  href="javascript:void(0);">## getItem('Label') ##</a>
                        </span>
                    </LinkItemTemplate>
                    <MoreLinkButtonTemplate>
                        <Template>
                            <a href="javascript:void(0)" onfocus="onfocusMoreLink();" class="NotShowLoading" title="<%=GetTitleByKey("aca_tabbar_label_morelink","") %>">
                                <ACA:AccelaLabel ID="lblMoreLinks" class='more_button font12px' runat="server" IsDisplaySuperAgencyText="true" LabelKey="aca_tabbar_label_morelink"></ACA:AccelaLabel>
                                <img alt="" src="<%=ImageUtil.GetImageURL("caret_arrow.gif") %>" />
                            </a>
                        </Template>
                    </MoreLinkButtonTemplate>
                    <DropDownMenuTemplate>
                        <div class="ACA_MenuItem ACA_MenuItem_FontSize">
                            <a title='## getItem('Title') ##' onfocus="SetFocusInForMore();" onblur="SetBlurOutForMore();"  href="javascript:void(0);"  module="## getItem('Module') ##">## getItem('Label') ##</a>
                        </div>
                    </DropDownMenuTemplate>
                </ACA:TabBar>
            </div>
        </ACA:AccelaDiv>
    </div>
</div>
<asp:HiddenField ID="hdnShoppingCartItemNumber" runat="server" />
<asp:HiddenField ID="hdnShowReportLink" runat="server" />
<asp:UpdatePanel ID="panButton" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaLinkButton ID="btnPostForReport" runat="server" TabIndex="-1" Style="display: none"></ACA:AccelaLinkButton>
    </ContentTemplate>
</asp:UpdatePanel>
<!-- End HeadNavigation Block -->

<script language="javascript" type="text/javascript">

    var objTimeout = null;

    function hideMenu() {
        clearTimeout(objTimeout);
        objTimeout = null;
        objTimeout = setTimeout(DelayShowMenu, 3000);
    }

    function SetFocusInForCollection() {
        clearTimeout(objTimeout);
        objTimeout = null;
    }

    function SetBlurOutForCollection() {
        hideMenu();
    }

    function displayMenu() {
        clearTimeout(objTimeout);
        objTimeout = null;
    }

    function DelayShowMenu() {
        collection_menu.hideMenu();
    }

    function showMenu(target) {
        CheckAndSetNoAsk();
        if (collection_menu == undefined) return;
        collection_menu.showMenu();
        hideMenu();
    }

    var objTimerForMore = null;
    function onfocusMore(obj) {
        window.popUp(window.__Tab.dropDownMenu.element, $get('span_more_tab'),$.global.isRTL);
        //$("#divNavMenu").find('a').get(0).focus();
        $("#moreArrowLink").attr("tabindex", "-1");
        clearTimeout(objTimerForMore);
        objTimerForMore = null;
    }

    function onfocusMoreLink() {
        window.popUp($get('divLinkMenu'), $get('nav_span_more_link'), $.global.isRTL);
    }

    function SetFocusInForMore() {
        clearTimeout(objTimerForMore);
        objTimerForMore = null;
    }

    function SetBlurOutForMore() {
        clearTimeout(objTimerForMore);
        objTimerForMore = null;
        objTimerForMore = setTimeout(onblurMore, 700);
    }

    function onblurMore() {
        window.__Tab.moreTab.closeMenu(window.__Tab.dropDownMenu);
    }

    //The method defined in Bage page.
    function SupportAccessibility(obj) {
        if (obj) {
            PageMethods.SetAccessibilityCookie(obj.checked, RefreshPage);
        }
    }

    // When check/uncheck Accessibility Support, refresh the whole page.
    function RefreshPage() {
        window.parent.location.reload(true);
    }

    var OnAnnouncementList = '<%=OnAnnouncementList%>';

    $(document).ready(function () {
        if ($.global.isAdmin == false && '<%=UseAnnouncement %>'.toLowerCase() == 'true') {
            ShowAnnouncementInit();
        }
    });
</script>