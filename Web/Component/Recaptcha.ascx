<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Recaptcha.ascx.cs" Inherits="Accela.ACA.Web.Component.Recaptcha" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>

<script type="text/javascript">
    $(document).ready(function () {
        if (typeof (triggerLogin) == 'function') {
            $('#recaptcha_response_field').bind('keydown', triggerLogin);
        }
    });
</script>

<div class="recaptcha">
    <ACA:AccelaLabel ID="lblRecaptchaDesc" CssClass="ACA_Label font12px ACA_LabelHeight" LabelKey="aca_capcha_description" runat="server"></ACA:AccelaLabel>
    <div>
        <ACA:AccelaLabel ID="lblErrorMessage" runat="server" CssClass="CaptchaError"></ACA:AccelaLabel>
    </div>
    <div id="recaptcha_widget" style="display:none;">
        <table class='NoBorder' role='presentation'>
            <tr>
                <td>
                    <div id="recaptcha_image"></div>
                </td>
                <td>
                    <div class="recaptcha_buttons">
                        <div>
                            <a class="NotShowLoading" onclick="javascript:void(0);" title="<%=GetTextByKey("aca_recaptcha_refresh") %>" href="javascript:Recaptcha.reload();">
                                <img src="<%= ImageUtil.GetImageURL("recaptcha_refresh.png") %>" alt="<%=GetTextByKey("aca_recaptcha_refresh") %>" />
                            </a>
                        </div>
                        <div class="recaptcha_only_if_image">
                            <a class="NotShowLoading" onclick="javascript:void(0);" title="<%=GetTextByKey("aca_recaptcha_audio_challenge") %>" href="javascript:Recaptcha.switch_type('audio');">
                                <img src="<%= ImageUtil.GetImageURL("recaptcha_audio.png") %>" alt="<%=GetTextByKey("aca_recaptcha_audio_challenge") %>"/>
                            </a>
                        </div>
                        <div class="recaptcha_only_if_audio">
                            <a class="NotShowLoading" onclick="javascript:void(0);" title="<%=GetTextByKey("aca_recaptcha_visual_challenge") %>" href="javascript:Recaptcha.switch_type('image');">
                                <img src="<%= ImageUtil.GetImageURL("recaptcha_text.png") %>"alt="<%=GetTextByKey("aca_recaptcha_visual_challenge") %>"/>
                            </a>
                        </div>
                        <div>
                            <a class="NotShowLoading" title="<%=GetTextByKey("aca_recaptcha_visual_help") %>" onclick="javascript:void(0);" href="javascript:Recaptcha.showhelp();">
                                <img src="<%= ImageUtil.GetImageURL("recaptcha_help.png") %>" alt="<%=GetTextByKey("aca_recaptcha_visual_help") %>"/>
                            </a>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="ACA_TabRow">
        <ACA:AccelaTextBox id="recaptcha_response_field" IsFixedUniqueID="true" runat="server" />
    </div>
    <ACA:AccelaRecaptcha ID="recaptcha" Theme="custom" CustomThemeWidget="recaptcha_widget" PublicKey="publicKey" PrivateKey="privateKey" runat="server"/>
</div>
