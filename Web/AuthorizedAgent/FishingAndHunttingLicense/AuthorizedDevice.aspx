<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthorizedDevice.aspx.cs"
    Inherits="Accela.ACA.Web.AuthorizedAgent.FishingAndHunttingLicense.AuthorizedDevice" %>

<%@ Import Namespace="Accela.ACA.Web.AuthorizedAgent" %>
<%@ Import Namespace="Accela.ACA.Web.Util" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" xml:lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>">
<head id="Head1" runat="server">
     <title><%=LabelUtil.GetGlobalTextByKey("aca_authorizeddevice_label_title|tip")%></title>
</head>
<body class="ACA_Page_NoScrollBar">
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/jquery.js") %>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/GlobalConst.aspx")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/global.js") %>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/popUpDialog.js") %>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/ProcessLoading.js") %>"></script>
    <script type="text/javascript" language="javascript">
        var timerId;
        var authServiceUnDefined = !<%=AuthorizedAgentServiceUtil.HasAuthorizedServiceConfig().ToString().ToLower()%>;
        
        function Download() {
            var iframe = $('#iframeExport');
            var d = new Date();
            iframe.attr("src", "../DownloadClient.ashx?flag=" + d.getSeconds() + d.getMinutes());
        }

        $(document).ready(function () {
            if (!<%=AppSession.IsAdmin.ToString().ToLower()%> && 
                (!<%=AppSession.User.AgentClientInstalled.ToString().ToLower()%> 
                || !<%=AppSession.User.PrinterConnected.ToString().ToLower()%>
                || authServiceUnDefined)) {
                parent.ShowLoading();
                checkStatus();
            }
        });

        function checkStatus() {
            clearTimeout(timerId);
            
            if (authServiceUnDefined) {
                parent.HideLoading();
                parent.PopUpAuthorizedDevice();
                setDeviceCheckHeight();
                return;
            }
            
            jQuery.event.trigger("ajaxStop");
            var printerConnected = "";
            var clientStatus = '<%=AppSession.User.AgentClientInstalled.ToString()%>'.toLowerCase();
            var printerStatus = '<%=AppSession.User.PrinterConnected.ToString()%>'.toLowerCase();
            $('#clientRun').val('0');

            $.ajax({
                type: "get",
                async: false,
                dataType: "jsonp",
                contentType: "application/json;utf-8",
                url: "http://127.0.0.1:32012/printerlist?action=PrinterList",
                jsonp: "callback",
                timeout: 3000,
                jsonpCallback: "callBack",
                success: function (info) {
                    clearTimeout(timerId);
                    parent.HideLoading();
                    $('#clientRun').val('1');
                    var json = info.Printers;
                    printerConnected = "false";

                    for (var i = 0; i < json.length; i++) {
                        if ('<%=AuthorizedAgentServiceUtil.GetPrinterConfig()%>'.toLowerCase().indexOf(json[i].toLowerCase()) > -1) {
                            printerConnected = "true";
                            break;
                        }
                    }

                    if ((printerConnected && printerStatus != printerConnected) || clientStatus == 'false') {
                        __doPostBack("<%=this.UniqueID%>$<%=PRINT_STATUS_CHANGE%>", printerConnected);
                    } else {
                        parent.PopUpAuthorizedDevice();
                        setDeviceCheckHeight();
                    }
                }
            });

            timerId = setTimeout(function () {
                parent.HideLoading();
                parent.PopUpAuthorizedDevice();
                setDeviceCheckHeight();
                jQuery.event.trigger("ajaxStop");
                    
                if ($('#clientRun').val() == '0' && clientStatus == 'true' && '<%=LastPostbackSource%>'.indexOf('<%=CLIENT_STATUS_CHANGE%>') == -1) {
                    clearTimeout(timerId);
                    __doPostBack("<%=this.UniqueID%>$<%=CLIENT_STATUS_CHANGE%>", "false");
                } else {
                    checkStatus();
                }
            }, 10000);
        }

        function RedirectPage() {
            if ('<%=AppSession.User.IsAuthorizedAgent.ToString()%>'.toLowerCase() == 'true') {
                parent.location.href = '<%=FileUtil.AppendApplicationRoot("/Account/AccountManager.aspx")%>';
            } else if ('<%=AppSession.User.IsAgentClerk.ToString()%>'.toLowerCase() == 'true') {
                parent.location.href = '<%=AuthenticationUtil.LogoutUrl%>';
            }
    }
        
    function setDeviceCheckHeight() {
        // set the Device Check iframe's height to its container.
        var iframe = getOwnIframe(window);

        if (iframe) {
            iframe.height = document.body.offsetHeight;
        }
    }
    </script>
    <form id="form1" runat="server">
    <input type="hidden" id="clientRun" runat="server" />
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" ScriptMode="Release" />
    <iframe width="0" height="0" id="iframeExport" class="ACA_Hide" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>">
        <%=GetTextByKey("iframe_nonsrc_nonsupport_message")%></iframe>
    <asp:HiddenField runat="server" ID="hdnConnectedPrinter" />
    <div class="AuthorizedDevice">
    <!-- button list -->
        <div class="ACA_Hide">
            <span id="messageSpan" name="messageSpan"></span>
        </div>
        <div runat="server" id="divButtonList" visible="False" class="ACA_TabRow ACA_LiLeft action_buttons">
            <table role='presentation'>
                <tr valign="bottom">
                    <td>
                        <ul>
                            <li runat="server" Visible="False" id="liOK">
                                <div class="ACA_LgButton ACA_LgButton_FontSize ACA_FLeft">
                                    <ACA:AccelaButton ID="lnkOK" OnClientClick="Download();return false;"
                                        LabelKey="aca_auth_agent_label_download" runat="server" />
                                </div>
                            </li>
                            <li runat="server" Visible="False" id="liRefresh">
                                <div class="ACA_LgButton ACA_LgButton_FontSize ACA_FLeft">
                                    <ACA:AccelaButton ID="lnkRefresh" OnClientClick="window.location.href = window.location.href;return false;"
                                        LabelKey="aca_auth_agent_label_refresh" runat="server" />
                                </div>
                            </li>
                        </ul>
                    </td>
                    <td>
                        <div class="ACA_LinkButton">
                            <ACA:AccelaLinkButton ID="lnkCancel" CssClass="NotShowLoading" OnClientClick="RedirectPage();return false;"
                                LabelKey="aca_auth_agent_label_download_cancel" runat="server" />
                        </div>
                    </td>
                </tr>
            </table>
            <input type="submit" name="Submit" value="Submit" class="HiddenButton" onclick="return false;" />
        </div>
    </div>
    </form>
</body>
</html>
