<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Report.ReportParameter" Codebehind="ReportParameter.aspx.cs" %>
<%@ Import Namespace="Accela.ACA.Common.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" xml:lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>">
<% if (ShowProcessLoading4ShowReport)
   { %>
<script type="text/javascript">
    // Try to place before all the other codes.
    var showProcessLoading = true;
</script>
<% } %>
<head runat="server">
    <title><%=LabelUtil.GetGlobalTextByKey("aca_reportparameter_label_title|tip")%></title>
    <script type="text/javascript">
        //Esc hot key for escape current window
        function EscapeWindow(e) {
            if (e.keyCode == 27) {
                window.close();
            }
        }
    </script>
</head>
<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/jquery.js")%>"></script>
<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/jquery.corner.js")%>"></script>
<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/GlobalConst.aspx")%>"></script>
<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/global.js")%>"></script>
<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/ProcessLoading.js")%>"></script>
<body class="ACA_ReportWindow" onkeydown="EscapeWindow(event)">
    <span style="display:none">
        <ACA:AccelaHideLink ID="hlSkipToolBar" runat="server" AltKey="img_alt_modulemenu_skiplink" IsSkippingToAnchor="true" NextControlClientID="FirstAnchorInACAMainContent" TabIndex="-1"/>
        <a name="FirstAnchorInACAMainContent" id="FirstAnchorInACAMainContent" tabindex="-1"></a>
    </span>
    <form id="form1" runat="server" >
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
    <div id="divReportParamter" class="ACA_Content" runat="server">
        <div class="ACA_TabRow">
            &nbsp;</div>
        <div id="divInstruction" runat="server">
            <h1>
                <ACA:AccelaLabel ID="lblInstruction" LabelKey="aca_report_parameter_label_instruction" runat="server" LabelType="BodyText" />
            </h1>
            <br />
        </div>
        <asp:PlaceHolder ID="phParameterControls" runat="Server"></asp:PlaceHolder>
        <div class="ACA_TabRow">
            &nbsp;</div>
        <div id="divButtons">
            <table role='presentation'>
                <tr>
                    <td>
                        <ACA:AccelaButton ID="btnSave" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" runat="Server" LabelKey="aca_report_parameter_button_submit"
                            OnClick="SaveButton_Click"></ACA:AccelaButton>
                    </td>
                    <td class="ACA_PaddingStyle20">
                        <ACA:AccelaButton ID="btnCancel" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" runat="Server" LabelKey="aca_report_parameter_button_cancel"
                            OnClientClick="javascript:window.close();"></ACA:AccelaButton>
                    </td>
                </tr>
            </table>            
        </div>
    </div>
    <input type="submit" name="Submit" value="Submit" class="HiddenButton" onclick="return false;" />
    </form>
    <script type="text/javascript">
        with (Sys.WebForms.PageRequestManager.getInstance()) {
            add_endRequest(onEndRequest);
            add_pageLoaded(onPageLoaded);
        }

        var processLoading = new ProcessLoading();

        $(document).ready(function () {
            if (typeof (showProcessLoading) != 'undefined' && showProcessLoading == true) {
                window.location.href = '<% ="ShowReport.aspx?" + ScriptFilter.EncodeUrlEx(Request.QueryString.ToString())%>';
                processLoading.showLoading(false);
            }
        });

        function onPageLoaded(sender, args) {
            processLoading.initControlLoading();
        }

        function onEndRequest(sender, arg) {
            processLoading.hideLoading();
        }
    </script>
    <noscript><%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</body>
</html>
