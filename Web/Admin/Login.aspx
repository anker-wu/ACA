<%@ Page Language="C#" AutoEventWireup="true" Inherits="ACA.Admin.Login" Codebehind="Login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <span style="display:none">
        <ACA:AccelaHideLink ID="hlSkipToolBar" runat="server" AltKey="img_alt_modulemenu_skiplink" IsSkippingToAnchor="true" NextControlClientID="FirstAnchorInACAMainContent" TabIndex="-1"/>
        <a name="FirstAnchorInACAMainContent" id="FirstAnchorInACAMainContent" tabindex="-1"></a>
    </span>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/jquery.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/GlobalConst.aspx")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/global.js")%>"></script>
    <script type="text/javascript">
        var GLOBAL_VALIDATION_RESULTS_ACCESSKEY = "<%=Accela.ACA.Common.Util.AccessibilityUtil.GetAccessKey(Accela.ACA.Common.AccessKeyType.ValidationResults) %>";
        var isRTLstring = '<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft %>';
        $.global = {isRTL : false};
        $.global.isRTL = isRTLstring=='true' ? true : false;
        
        //fix the issue that the confirm/alert box should also follows the current culture.
        if ($.global.isRTL) {
            document.dir = "rtl";
        } else {
            document.dir = "";
        }
        
        var getText=new function(){
            this.global_js_showError_alt='<%=LabelUtil.GetGlobalTextByKey("aca_global_js_showerror_alt").Replace("'","\\'") %>';
            this.global_js_showError_src='<%=ImageUtil.GetImageURL("error_24.gif") %>';
            this.global_section508_more = '<%=LabelUtil.GetGlobalTextByKey("aca_section508_validation_more").Replace("'","\\'") %>';
            this.global_section508_required = '<%=LabelUtil.GetGlobalTextByKey("aca_required_field").Replace("'","\\'") %>';
            this.global_section508_errornotice1 = '<%=LabelUtil.GetGlobalTextByKey("aca_section508_validation_errornotice1").Replace("'","\\'") %>';
            this.global_section508_errornotice2 = '<%=LabelUtil.GetGlobalTextByKey("aca_section508_validation_errornotice2").Replace("'","\\'") %>';
        }
    </script>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table role="presentation" align="center" style="margin: 0 auto">
            <tr>
                <td>
                    <span id="ErrorList1" class="" tabindex="-1"></span>
                </td>
            </tr>
        </table>
        
        <div class="ACA_Admin_Div">
            <div class="ACA_TabRow">
                <img src="<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("admin_logo.png") %>" alt="" />
            </div>
            <table role="presentation" cellspacing="8" cellpadding="0" runat="server" id="tblMsg" visible="false">
                <tr>
                    <td valign="Top">
                        <img src="<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("error_16.gif") %>" alt="<%=LabelUtil.GetAdminUITextByKey("img_alt_mark_required") %>"/></td>
                    <td>
                        <div class="ACA_Admin_LoginMsg font11px">
                            <strong>
                                <ACA:AccelaLabel ID="lblMsg" runat="server" Text='<%#LabelUtil.GetAdminUITextByKey("aca_admin_login_timeout_msg") %>'></ACA:AccelaLabel>
                            </strong>
                        </div>
                    </td>
                </tr>
            </table>
            <table role="presentation" cellspacing="0" cellpadding="0" class="ACA_Admin_LoginBox">
                <tr>
                    <td>
                        <img src="<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("admin_login.png") %>" alt="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="content">
                            <div class="ACA_TabRow font8px">
                                <h3>
                                    <b>
                                        <ACA:AccelaTextBox runat="server" ID="txtUserId" MaxLength="50" CssClass="ACA_NLong"
                                            Validate="required;maxlength" Label='<%#LabelUtil.GetAdminUITextByKey("aca_admin_sign_label_username") %>' HideRequireIndicate="true" /></b></h3>
                            </div>
                            <div class="ACA_TabRow font8px">
                                <h3>
                                    <b>
                                        <ACA:AccelaPasswordText runat="server" ID="txtPassword" CssClass="ACA_NLong" Validate="required" autocomplete="off"
                                            Label='<%#LabelUtil.GetAdminUITextByKey("aca_admin_sign_label_password") %>' EnableStrengthValidate="false" HideRequireIndicate="true" /></b></h3>
                            </div>
                            <div class="ACA_DivRow">
                                &nbsp;</div>
                            <ACA:AccelaButton ID="lbLogin" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" runat="server" Text='<%#LabelUtil.GetAdminUITextByKey("aca_admin_sign_label_loginBtn")%>' OnClick="LoginButton_Click">
                            </ACA:AccelaButton>
                            <asp:Button ID="btnHidden" runat="server" OnClick="LoginButton_Click" UseSubmitBehavior="false"
                                Style="display: none;" />
                            <div class="ACA_DivRow">
                                &nbsp;</div>
                            <div class="ACA_TabRow ACA_BkGray">
                                &nbsp;</div>
                            <div class="aca_checkbox">
                                    <input type="checkbox" name="publicly_owned" runat="server" id="chkRemember" /><ACA:AccelaLabel ID="acc_sign_label_rememberMe" Text='<%#LabelUtil.GetAdminUITextByKey("aca_admin_sign_label_rememberMe") %>'
                                        runat="server" CssClass="ACA_SmLabel ACA_SmLabel_FontSize" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
    <noscript><%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</body>
</html>


<script type="text/javascript" language="javascript">
    //Init validation error panel for form validation.
    if (typeof (InitValidationErrorPanel) == "function") {
        $(document).bind("ready", InitValidationErrorPanel);
    }
</script>

