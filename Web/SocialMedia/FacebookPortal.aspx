<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FacebookPortal.aspx.cs"
    Inherits="Accela.ACA.Web.FacebookPortal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=LabelUtil.GetGlobalTextByKey("ACA_TopPage_Title")%>
    </title>
</head>
<body>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/jquery.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/jquery.corner.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/GlobalConst.aspx")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/global.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/ProcessLoading.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/GlobalSearch.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/popUpDialog.js")%>"></script>	    
    <script type="text/javascript">
        $.global.isRTL = IsTrue('<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft %>');
        $.global.isAdmin = IsTrue('<%= AppSession.IsAdmin %>');
    </script>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnableScriptGlobalization="true" EnableScriptLocalization="true"
        runat="server" EnablePageMethods="true" ScriptMode="Release" />
    <div class="fb_connect_frame">
        <div id="fb_div_container">
            <div id="fb_div_title">
                <div id="fb_div_title_app" class="fb_div_title_app">
                    <div id="fb_app_logo">
                    </div>
                    <div id="fb_div_title_appname" class="fb_div_title_appname">
                        <ACA:AccelaLabel ID="lblAppName" CssClass="fb_connect_title" LabelKey="aca_fb_connect_label_app_name"
                            runat="server">
                        </ACA:AccelaLabel>
                    </div>
                    <div id="fb_div_title_desc" class="fb_div_title_desc">
                        <ACA:AccelaLabel ID="lblDecription" CssClass="fb_connect_title_desc" LabelKey="aca_fb_connect_label_app_description"
                            runat="server">
                        </ACA:AccelaLabel>
                    </div>
                </div>
            </div>
            <div class="ACA_Content ACA_Hide fb_connect_message_div">
                <span id="messageSpan" name="messageSpan"></span>
            </div>
            <div id='divLoginBack' class="fb_connect_div_connect">
                <div class="fb_connect_desc_div">
                    <ACA:AccelaLabel ID="lblDescription" CssClass="align_span" LabelKey="aca_fb_connect_label_description" LabelType="BodyText"
                        runat="server">
                    </ACA:AccelaLabel>
                </div>
                <asp:UpdatePanel ID="upFacebookConnect" runat="server" UpdateMode="conditional">
                    <ContentTemplate>
                        <div id="divBox">
                            <div id="divConnect">
                                <div id="fb_div_haveaccount" class="fb_div_haveaccount">
                                    <ACA:AccelaLabel ID="lblHaveAccount" CssClass="fb_connect_body_text" LabelKey="aca_fb_connect_label_have_account"
                                        runat="server">
                                    </ACA:AccelaLabel>
                                </div>
                                <div id="fb_div_btnconnect" class="fb_div_btnconnect">
                                        <ACA:AccelaLinkButton ID="btnConnect" OnClientClick="showPopupDiv(false);return false;" runat="server" CssClass="fb_div_btnconnect_bg fb_connect_body_button NotShowLoading"
                                            LabelKey="aca_fb_connect_label_account" CausesValidation="false">
                                        </ACA:AccelaLinkButton>
                                </div>
                                <div id="divAutoCreateAccount" runat="server">
                                    <div id="fb_div_havenotaccount" class="fb_div_havenotaccount">
                                        <ACA:AccelaLabel ID="lblHaveNotAccount" CssClass="fb_connect_body_text" LabelKey="aca_fb_connect_label_not_account"
                                            runat="server">
                                        </ACA:AccelaLabel>
                                    </div>
                                    <div id="fb_div_btncreate" class="fb_div_btncreate">
                                        <ACA:AccelaLinkButton ID="btnAutoCreateAccount" runat="server" CssClass="fb_div_btncreate_bg fb_connect_body_button"
                                            LabelKey="aca_fb_connect_label_havenot_account" CausesValidation="false" OnClick="AutoCreatAccoutButton_Click">
                                        </ACA:AccelaLinkButton>
                                    </div>
                                </div>
                            </div>
                            <div id="divLogin" class="ACA_Hide divloginbox">
                                <ACA:AccelaHideLink ID="hlBegin" AltKey="img_alt_form_begin" Width="0" runat="server" NextControlID="btnSaveModule" />
                                <div id="fb_div_login_bg">
                                    <div id="fb_div_login_title" class="fb_div_login_title">
                                        <ACA:AccelaLabel ID="lblLoginTitle" CssClass="fb_connect_button" LabelKey="aca_fb_connect_label_login_title"
                                            runat="server">
                                        </ACA:AccelaLabel>
                                        <div id="divCloseButton" class="ACA_AlignRightOrLeft CloseImage ACA_FRight fb_connect_div_connect_close">
                                            <ACA:AccelaLinkButton id="lnkCancelModuleSave" CssClass="NotShowLoading" runat="server" CausesValidation="false" OnClientClick="closePopupDiv();return false;">
                                                <img class="ACA_ActionIMG" src='<%=ImageUtil.GetImageURL("closepopup.png") %>' alt="close" />
                                            </ACA:AccelaLinkButton>
                                        </div>
                                    </div>
                                    <div id="showContent">
                                        <div id="divError" class="ACA_Content fb_connect_div_error_in_login">
                                            <span id="Span1" name="messageSpan">
                                                <div id="messageSpan_messages">
                                                    <div candelete="candelete" class="ACA_Message_Error ACA_Message_Error_FontSize">
                                                        <table role="presentation" class="ACA_InsContent" border="0" cellpadding="0" cellspacing="0">
                                                            <tbody>
                                                                <tr>
                                                                    <td class="ACA_Message_Icon">
                                                                        <div class="ACA_Error_Icon">
                                                                            <img src="<%=ImageUtil.GetImageURL("error_24.gif") %>" alt="An error has occurred."
                                                                                title="An error has occurred."></div>
                                                                    </td>
                                                                    <td class="ACA_XShoter">
                                                                        <div>
                                                                            &nbsp;</div>
                                                                    </td>
                                                                    <td class="ACA_Message_Content">
                                                                        <div>
                                                                            <% =LabelUtil.GetGlobalTextByKey("aca_global_js_showerror_title").Replace("'", "\\'")%><br>
                                                                            <ACA:AccelaLabel ID="lblLoginError" runat="server">
                                                                            </ACA:AccelaLabel>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </span>
                                        </div>
                                        <div class="fb_div_login">
                                            <ACA:AccelaTextBox runat="server" ID="txtUserId" MaxLength="50" Width="242px" CssClass="ACA_NLonger"
                                                Validate="required;maxlength" LabelKey="aca_fb_connect_label_login_uid" HideRequireIndicate="true" />
                                        </div>
                                        <div class="fb_div_login">
                                            <ACA:AccelaPasswordText runat="server" ID="txtPassword" Width="242px" CssClass="ACA_NLonger"
                                                Validate="required" LabelKey="aca_fb_connect_label_login_pwd" EnableStrengthValidate="false"
                                                HideRequireIndicate="true" autocomplete="off" />
                                        </div>
                                    </div>
                                    <div id="fb_div_login_btn" class="fb_div_login_btn">
                                        <table role="presentation" class="ACA_TDAlignLeftOrRightTop ACA_FRight" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <div id="fb_div_login_btnlogin" class="fb_button_container">
                                                        <ACA:AccelaLinkButton ID="btnConnectToFacebook" runat="server" CssClass="uiButtonConfirm uiButton"
                                                            LabelKey="aca_fb_connect_label_login_button" OnClick="ConnectToFacebookButton_Click">
                                                        </ACA:AccelaLinkButton>
                                                        <asp:Button ID="btnConnectHidden" runat="server" OnClick="ConnectToFacebookButton_Click" UseSubmitBehavior="false" Style="display: none;" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div id="fb_div_login_btncancel">
                                                        <ACA:AccelaLinkButton ID="lblCancel" OnClientClick="closePopupDiv();return false;" runat="server"
                                                            CssClass="uiButton NotShowLoading" LabelKey="aca_fb_connect_label_login_cancel"
                                                            CausesValidation="false">
                                                        </ACA:AccelaLinkButton>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div id="showLogin">
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <ACA:AccelaHeightSeparate ID="height" Height="20" runat="server"></ACA:AccelaHeightSeparate>
        </div>
    </div>
    <div id="divLoadingTemplate" class="ACA_Loading_Message" style="width: auto">
        <img alt="<%=GetTextByKey("aca_global_msg_loading") %>" src="<%=ImageUtil.GetImageURL("loading.gif") %>" />
    </div>
    <script type="text/javascript">
        var moduleListPopUpDialog;

        function showPopupDiv(showError) {
            $('#divLogin').css('position', 'absolute');
            $('#divLogin').css('z-index', '5');

            if (showError) {
                $('#divError').show();
            }
            else {
                $('#divError').hide();
            }

            moduleListPopUpDialog = new popUpDialog($get('divLogin'), $get("<%=hlBegin.ClientID%>"), null, $get('fb_div_container'), $get("<%=btnConnectToFacebook.ClientID%>"));
            moduleListPopUpDialog.showPopUp();
        }

        function closePopupDiv() {
            moduleListPopUpDialog.cancel();
        }

        $(document).ready(function () {
            $('#divError').hide();

            if ($.global.isAdmin) {
                $('#divLogin').show();
                $('#divBox').css('height', '50em')
                $('#divLoginBack').css('height', 'auto')
            }
        });
    </script>
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

            addPrintErrors2SubmitButton();

            $('#<%=txtPassword.ClientID %>').keydown(function (event) {
                if (event.which == 13) {
                    $('#<%=btnConnectHidden.ClientID %>').click();
                }
            });
        }

        function onEndRequest(sender, arg) {
            var sourceElement = sender._postBackSettings.sourceElement;

            if (sourceElement == null || (sourceElement != null && $(sourceElement).hasClass('NotShowLoading') == false)) {
                processLoading.hideLoading();
            }
        }
    </script>
    <input type="submit" name="Submit" value="Submit" class="HiddenButton" onclick="return false;" />
    </form>
</body>
</html>
