<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailReport.aspx.cs" Inherits="Accela.ACA.Web.Report.EmailReport" %>

<%@ Register Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" xml:lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>">

<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/jquery.js")%>"></script>
<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/GlobalConst.aspx")%>"></script>
<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/global.js")%>"></script>
<head runat="server">
    <title><%=LabelUtil.GetGlobalTextByKey("aca_emailreport_label_title|tip")%></title>
    <script type="text/javascript">
        //Esc hot key for escape current window
        function EscapeWindow(e) {
            if (e.keyCode == 27) {
                window.close();
            }
        }
    </script>

</head>
<body class="ACA_ReportWindow" onkeydown="EscapeWindow(event)">
    <form id="form1" runat="server">
    <div class="report_body">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <span style="display:none">
            <ACA:AccelaHideLink ID="hlSkipToolBar" runat="server" AltKey="img_alt_modulemenu_skiplink" IsSkippingToAnchor="true" NextControlClientID="FirstAnchorInACAMainContent" TabIndex="-1"/>
            <a name="FirstAnchorInACAMainContent" id="FirstAnchorInACAMainContent" tabindex="-1"></a>
        </span>
        <div class="ACA_Row">
            <uc1:MessageBar ID="resultMessage" runat="Server" />
        </div>
        <div class="email_list">
            <ACA:AccelaGridView runat="server" Width="100%" ID="EmailList" OnRowDataBound="EmailList_RowDataBound" 
                SummaryKey="aca_summary_email_list" CaptionKey="aca_caption_email_list"
                AutoGenerateColumns="false" PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom"
                AllowPaging="false" ShowHorizontalScroll="false" AlternatingRowStyle-CssClass="ACA_ReportWindow"
                RowStyle-CssClass="ACA_ReportWindow">
                <Columns>
                    <ACA:AccelaTemplateField>
                        <HeaderTemplate>
                            <div>
                                <ACA:AccelaLabel ID="lblHeader" LabelKey="aca_report_email_emailto_label"
                                    runat="server" CssClass="ACA_Label font14px" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table role='presentation' cellspacing="8px" border="0" width="100%">
                                <tr>
                                    <td width="25%">
                                        <ACA:AccelaCheckBox ID="chkEmailName" runat="server" CssClass="font11px"/>
                                    </td>
                                    <td width="20%">
                                        <ACA:AccelaLabel ID="lblPeopleType" Text='<%#DataBinder.Eval(Container.DataItem, "contactType") %>'
                                            runat="server" CssClass="font11px" />
                                    </td>
                                    <td>
                                        <ACA:AccelaEmailText ID="txtEmailAddress" CssClass="ACA_XLong" Validate="Emails"
                                            MaxLength="250" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "email")%>'></ACA:AccelaEmailText>
                                        <div id="divInstruction" class="ACA_TabRow" runat="Server" visible="false">
                                            <ACA:AccelaLabel ID="lblInstruction" LabelKey="aca_report_email_instruction_label"
                                                runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <ItemStyle Width="100%" />
                        <HeaderStyle Height="30px" CssClass="ACA_ReportWindow" />
                    </ACA:AccelaTemplateField>
                </Columns>
            </ACA:AccelaGridView>
        </div>
        <div id="divButtons" class="ACA_PaddingStyle20">
            <table role='presentation'>
                <tr>
                    <td>
                        <ACA:AccelaButton ID="btnSendEmail" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                            runat="Server" LabelKey="aca_report_email_sendemail_button" OnClick="SendEmailButton_Click">
                        </ACA:AccelaButton>
                    </td>
                    <td class="ACA_PaddingStyle20">
                        <ACA:AccelaButton ID="btnCancel" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                            runat="Server" LabelKey="aca_report_email_cancel_button" OnClientClick="javascript:window.close();">
                        </ACA:AccelaButton>
                    </td>
                </tr>
            </table>
            <input type="submit" name="Submit" value="Submit" class="HiddenButton" onclick="return false;" />
        </div>
    </div>
    </form>
</body>
</html>
