<%@ Page Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Cap.CapApplyDisclaimer"
    MasterPageFile="~/Default.master" CodeBehind="CapApplyDisclaimer.aspx.cs" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="PlaceHolderMain">

    <script type="text/javascript">
        function chbAccept_onclick() {
            var obj = document.getElementById("<%=termAccept.ClientID %>");
            if (obj.checked) {
                //hideMessage();
                //__doPostBack("btnRedirect");
            }
            else {
                showNormalMessage('<%=GetTextByKey("acc_regHome_error_acceptTerms").Replace("'","\\'")%>', 'Error');
                document.getElementById("div_error_icon").style.display = "";
                return false;
            }
        }

        function chbSelected_onclick() {
            var obj = document.getElementById("<%=termAccept.ClientID %>");
            if (obj.checked) {
                hideMessage();
                document.getElementById("div_error_icon").style.display = "none";
            }
        }  
          
    </script>

    <div class="ACA_RightContent">
        <h1>
            <ACA:AccelaLabel LabelKey="per_permitHome_label_onlinePermit" ID="per_permitHome_label_onlinePermit"
                runat="server"></ACA:AccelaLabel>
        </h1>
        <ACA:AccelaHeightSeparate ID="sepLine" runat="server" Height="15" />
        <div class="ACA_Page ACA_Page_FontSize_Restore">
            <ACA:AccelaLabel LabelKey="per_permitHome_text_welcomeInfo" ID="per_permitHome_text_welcomeInfo"
                runat="server" LabelType="BodyText"></ACA:AccelaLabel>
        </div>
        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" runat="server" Height="0" />
        <div class="DisclaimerContainerStyle ACA_ContainerStyle">
            <table role='presentation' width="100%" border="0" cellspacing="0" cellpadding="8">
                <tr>
                    <td>
                        <ACA:AccelaLabel LabelKey="per_disclaimer_text_disclaimer" ID="per_disclaimer_text_disclaimer"
                            runat="server" LabelType="BodyText" />
                        <ACA:AccelaLabel LabelKey="per_disclaimer_text_disclaimer1" ID="per_disclaimer_text_disclaimer1"
                            runat="server" LabelType="BodyText" />
                        <ACA:AccelaLabel LabelKey="per_disclaimer_text_disclaimer2" ID="per_disclaimer_text_disclaimer2"
                            runat="server" LabelType="BodyText" />
                        <ACA:AccelaLabel LabelKey="per_disclaimer_text_disclaimer3" ID="per_disclaimer_text_disclaimer3"
                            runat="server" LabelType="BodyText" />
                    </td>
                </tr>
            </table>
        </div>
        <ACA:AccelaHeightSeparate ID="sepForCheckBox" runat="server" Height="16" />
        <div class="ACA_TabRow ACA_SmallError_Icon" id="div_error_icon" style="display: none">
            <img class="ACA_NoBorder" alt="<%=GetTextByKey("img_alt_mark_required") %>" src="<%=ImageUtil.GetImageURL("error_16.gif") %>"/>
        </div>
        <div id="divCheckText" class="aca_checkbox aca_checkbox_fontsize">
            <input type="checkbox" id="termAccept" runat="server" onclick="chbSelected_onclick(this)" />
            <ACA:AccelaLabel LabelKey="per_permitHome_label_acceptTerms" ID="per_permitHome_label_acceptTerms"
                runat="server" class="ACA_SmLabel ACA_SmLabel_FontSize" AssociatedControlID="termAccept"/>
        </div>
        <ACA:AccelaHeightSeparate ID="sepForButton" runat="server" Height="25" />
        <div id="divNextStep" runat="server" class="ACA_LgButtonHeight">
            <ACA:AccelaButton ID="btnNextStep" runat="server" LabelKey="per_permitHome_label_continueRegBtn"
                OnClientClick="return chbAccept_onclick()" OnClick="NextStepButton_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize ACA_Button_Text">
            </ACA:AccelaButton>
        </div>
    </div>
</asp:Content>
