<%@ Page Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Report.ReportPreview"
    ValidateRequest="false" CodeBehind="ReportPreview.aspx.cs" %>

<%@ Import Namespace="Accela.ACA.Common.Common" %>
<%@ import namespace="Accela.ACA.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" xml:lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>">
<head id="Head1" runat="server">
    <title><%=LabelUtil.GetGlobalTextByKey("aca_reportpreview_label_title|tip")%></title>
    <script type="text/javascript">
        //Esc hot key for escape current window
        function EscapeWindow(e) {
            if (e.keyCode == 27) {
                window.close();
            }
        }
        function print_onclick(url) {
            var a = window.open(url, "_blank", "top=200,left=200,height=600,width=800,status=yes,toolbar=0,menubar=no,location=no,scrollbars=yes");
        }
    </script>
</head>
<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/jquery.js")%>"></script>
<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/GlobalConst.aspx")%>"></script>
<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/global.js")%>"></script>
<body class="ACA_ReportWindow" onkeydown="EscapeWindow(event)">
    <form id="form1" runat="server" style="height: 600px">
    <span style="display:none">
        <ACA:AccelaHideLink ID="hlSkipToolBar" runat="server" AltKey="img_alt_modulemenu_skiplink" IsSkippingToAnchor="true" NextControlClientID="FirstAnchorInACAMainContent" TabIndex="-1"/>
        <a name="FirstAnchorInACAMainContent" id="FirstAnchorInACAMainContent" tabindex="-1"></a>
    </span>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
    <div><span id="messageSpan" name="messageSpan"></span></div>    
    <div id="sendEmailDiv">
        <table role="presentation" cellspacing="10px">
            <tr>
                <td>
                    <ACA:AccelaButton ID="btnSendEmail" runat="server" LabelKey="aca_report_email_sendemail_button"
                        DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"></ACA:AccelaButton>
                </td>
            </tr>
        </table>
    </div>
    <iframe id="iframeReport" src='<%=ReportURL+"&isDisplaySendEmail=N"%>' width="100%"
        height="100%" frameborder="0" marginwidth="0" marginheight="0" scrolling="yes"
        title="<%=LabelUtil.GetGlobalTextByKey("iframe_capdetail_attachmentlist_title").Replace("'","\\'") %>">
        <%=String.Format(LabelUtil.GetGlobalTextByKey("iframe_nonsupport_message"), ReportURL+"&isDisplaySendEmail=N")%>
    </iframe>
    <input type="submit" name="Submit" value="Submit" class="HiddenButton" onclick="return false;" />
    </form>
</body>
</html>
