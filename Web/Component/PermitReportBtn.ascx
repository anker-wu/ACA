<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.PermitReportBtn" CodeBehind="PermitReportBtn.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<div class="ACA_TabRow ACA_LiLeft">
    <script type="text/javascript">
        function ShowClonePage() {

            if ($.global.isAdmin == false) {
                var url = "<%=GetClonePageUrl()%>";
                var objectTargetID = "<%=lnkCloneRecord.ClientID %>";
                window.ACADialog.popup({ url: url, width: 745, height: 380, objectTarget: objectTargetID });
            }
            return false;
        }

        function handlerPrint(clientId, moduleName, reportId, agencyCode, capId, hasParamters, url) {
            hideMessage();

            if (typeof (isLabelRePrint) != 'undefined' && isLabelRePrint == false) {
                isLabelRePrint = true;
                print_onclick(url);
                return;
            }

            PageMethods.HandlerReport(agencyCode, moduleName, reportId, capId, function (result) {
                if (!result) {
                    return;
                }

                var json = eval('(' + result + ')');

                if (!json.RunPrint) {
                    DisablePrintButton(clientId, true);
                    return;
                }

                if (json.IsReprint) {
                    ACADialog.returnUrl = url;
                    ACADialog.popup({ url: '<%=FileUtil.AppendApplicationRoot("AuthorizedAgent/FishingAndHunttingLicense/ReprintReason.aspx")%>?<%=ACAConstant.MODULE_NAME%>=' + moduleName + "&hasparameters=" + hasParamters + "&reportId=" + reportId + "&LastChance=" + json.LastChance + "&clientId=" + clientId + "&recordId=" + capId, width: 600, height: 400 });
                } else {
                    print_onclick(url);
                }
            }, function (error) {
                showNormalMessage(error.get_message(), 'Error');
            });
        }

        function printLabelReport(queryString) {
            $.ajax({
                type: "get",
                async: false,
                dataType: "jsonp",
                contentType: "application/json;utf-8",
                url: "http://127.0.0.1:32012/printerlist?action=PrinterReport&" + queryString,
                jsonp: "callback",
                timeout: 3000,
                jsonpCallback: "callBack",
                success: function (info) {
                }
            });
        }

        //PrintLabel button disable
        function DisablePrintButton(clientId, isDisalbe) {
            var btnlnkPrintLabel = document.getElementById(clientId);

            if (btnlnkPrintLabel) {
                /*
                When current page is CapCompletion.aspx, need to set parent node style to disable the print button,
                when CapCompletions.aspx, the button is link, needn't set print button parent node style.
                */
                if (window.location.pathname.lastIndexOf('CapCompletion.aspx') != -1) {
                    btnlnkPrintLabel.parentNode.setAttribute("class", "ACA_SmButtonDisable ACA_SmButtonDisable_FontSize");
                    btnlnkPrintLabel.parentNode.setAttribute("className", "ACA_SmButtonDisable ACA_SmButtonDisable_FontSize");
                }

                DisableButton(btnlnkPrintLabel.id, isDisalbe);
            }
        }
    </script>
    <table role='presentation'>
        <tr>
            <td>
                <div id="divPrintLicense" runat="server" visible="false" class="ACA_RightPadding">
                    <ACA:AccelaButton ID="lnkPrintLicense" DivEnableCss="ACA_SmButton ACA_SmButton_FontSize" CssClass="NotShowLoading" LabelKey="per_renewalLicensePermit_label_printLicense"
                        runat="server" ReportID="0" />
                </div>
            </td>
            <td>
                <div id="divPrintPermit" runat="server" visible="false" class="ACA_RightPadding">
                    <ACA:AccelaButton ID="lnkPrintPermit" DivEnableCss="ACA_SmButton ACA_SmButton_FontSize" CssClass="NotShowLoading" LabelKey="per_permitissuance_label_printrecord"
                        runat="server" ReportID="0" />
                </div>
            </td>
            <td>
                <div id="divPrintReceipt" runat="server" visible="false" class="ACA_RightPadding">
                    <ACA:AccelaButton ID="lnkPrintReceipt" DivEnableCss="ACA_SmButton ACA_SmButton_FontSize" CssClass="NotShowLoading" LabelKey="per_permitIssuance_label_printReceipt"
                        runat="server" ReportID="0" />
                </div>
            </td>
            <td>
                <div id="divPrintSummary" runat="server" visible="false" class="ACA_RightPadding">
                    <ACA:AccelaButton ID="lnkPrintSummary" DivEnableCss="ACA_SmButton ACA_SmButton_FontSize" CssClass="NotShowLoading" LabelKey="per_permitIssuance_label_printSummary"
                        runat="server" ReportID="0" />
                </div>
            </td>
            <td>
                <div id="divPrintLabel" runat="server" visible="false" class="ACA_RightPadding">
                    <ACA:AccelaButton ID="lnkPrintLabel" DivEnableCss="ACA_SmButton ACA_SmButton_FontSize" CssClass="NotShowLoading" LabelKey="aca_authagent_receipt_label_printlabel"
                        runat="server"  ReportID="0" />
                </div>
            </td>
            <td>
                <div id="divCloneRecord" runat="server" visible="false">
                    <ACA:AccelaButton ID="lnkCloneRecord" DivEnableCss="ACA_SmButton ACA_SmButton_FontSize" CssClass="NotShowLoading" LabelKey="aca_capcompletion_label_clone_record"
                        runat="server" OnClientClick="return ShowClonePage();" />
                </div>
            </td>
        </tr>
    </table>
</div>
