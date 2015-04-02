<%@ Page Language="C#" AutoEventWireup="true" Inherits="FileUpload_Progress" Codebehind="Progress.aspx.cs" %>

<%@ Register TagPrefix="Upload" Namespace="Brettle.Web.NeatUpload" Assembly="Brettle.Web.NeatUpload" %>
<html lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" xml:lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>">
<head runat="server">
    <title>Progress</title>
    <%= Accela.ACA.Common.Util.AccessibilityUtil.AccessibilityEnabled ? "<link rel='stylesheet' type='text/css' href='../App_Themes/Accessibility/AccessibilityDefault.css' />" : ""%>
    <link rel="stylesheet" type="text/css" title="default" href="../App_Themes/default/_progressbar.css" />
    <link rel="stylesheet" type="text/css" title="default" href="../App_Themes/default/navigation.css" />
    <link rel="stylesheet" type="text/css" title="default" href="../App_Themes/default/form.css" />
    <link rel="stylesheet" type="text/css" title="default" href="../App_Themes/<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>/_progressbar.css" />
    <link rel="stylesheet" type="text/css" title="default" href="../App_Themes/<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>/navigation.css" />
    <link rel="stylesheet" type="text/css" title="default" href="../App_Themes/<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>/form.css" />
    <link type="text/css" rel="stylesheet" href="../Handlers/CustomizedCssStyle.ashx" />
    <script src="../Scripts/FileUploadProgress.js" type="text/javascript"></script>
</head>
<body>    
    
    <%= Accela.ACA.Common.Util.AccessibilityUtil.AccessibilityEnabled ? "<link rel='stylesheet' type='text/css' href='../App_Themes/Accessibility/Accessibility.css' />" : ""%>
    <span style="display:none">
        <ACA:AccelaHideLink ID="hlSkipToolBar" runat="server" AltKey="img_alt_modulemenu_skiplink" IsSkippingToAnchor="true" NextControlClientID="FirstAnchorInACAMainContent" TabIndex="-1"/>
        <a name="FirstAnchorInACAMainContent" id="FirstAnchorInACAMainContent" tabindex="-1"></a>
    </span>
<script type="text/javascript">
        function IsStopTimer() {
            return  '<%=IsNormalStatus %>' == 'False';
        }

        function Callback(result, isExceptionOccur, callbackFunc) {
            var index = callbackFunc.indexOf('(');
            if (index > -1) {
                callbackFunc = callbackFunc.substring(0, index);
            }
            if (callbackFunc != '' && eval('typeof(window.parent.parent.' + callbackFunc + ')') != 'undefined') {
                var executeString = 'window.parent.parent.' + callbackFunc + "('" + isExceptionOccur + "','" + result + "');";
                eval(executeString);
            }
        }
    </script>
    <form id="dummyForm" runat="server">
        <table role='presentation'>
            <tr>
                <td>
                    <table role='presentation' id="ACA_ProgressDisplayCenterer">
                        <tr>
                            <td>
                                <table role='presentation' id="ACA_ProgressDisplay" class="ACA_ProgressDisplay ACA_ProgressDisplay_FontSize" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td id="barTd">
                                            <div id="statusDiv" runat="server" class="StatusMessage">
                                                <Upload:DetailsSpan ID="normalInProgress" runat="server" WhenStatus="NormalInProgress"
                                                    Style="font-weight: normal; white-space: nowrap;">
                                                    <ACA:AccelaLabel ID="lblNormalInProgress" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                                                </Upload:DetailsSpan>
                                                <Upload:DetailsSpan ID="chunkedInProgress" runat="server" WhenStatus="ChunkedInProgress"
                                                    Style="font-weight: normal; white-space: nowrap;">
                                                    <ACA:AccelaLabel ID="lblChunkedInProgress"  IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                                                </Upload:DetailsSpan>
                                                <Upload:DetailsSpan ID="completed" runat="server" WhenStatus="Completed">
                                                    
                                                </Upload:DetailsSpan>
                                                <Upload:DetailsSpan ID="cancelled" runat="server" WhenStatus="Cancelled">
                                                    
                                                </Upload:DetailsSpan>
                                                <Upload:DetailsSpan ID="rejected" runat="server" WhenStatus="Rejected">
                                                    <ACA:AccelaLabel ID="lblRejected"  IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                                                </Upload:DetailsSpan>
                                                <Upload:DetailsSpan ID="error" runat="server" WhenStatus="Failed">
                                                    <ACA:AccelaLabel ID="lblError"  IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                                                </Upload:DetailsSpan>
                                                <Upload:DetailsDiv ID="barDetailsDiv" runat="server" UseHtml4="true" Width='<%# Unit.Percentage(Math.Floor(100*FractionComplete)) %>'
                                                    class="ProgressBar">
                                                </Upload:DetailsDiv>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <div class="ACA_SmButton ACA_SmButton_FontSize">
                            <a href="<%=CancelUrl%>">
                            <ACA:AccelaLabel ID="lblSub"  IsNeedEncode="false" runat="server" style="font-size:0.625em;"></ACA:AccelaLabel>
                        </a>
                    </div>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hfCallbackFunc" runat="server" Value="1" />
         <script type="text/javascript">
             var callbackFunc = document.getElementById('hfCallbackFunc').value;
             if (callbackFunc != '') {
                 eval(callbackFunc);
             } 
        </script>
        <input type="submit" name="Submit" value="Submit" class="HiddenButton" onclick="return false;" />
    </form>
    <noscript><%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</body>
</html>
