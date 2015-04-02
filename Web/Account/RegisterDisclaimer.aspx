<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" ValidateRequest="false" Inherits="Accela.ACA.Web.Account.RegisterDisclaimer" Codebehind="RegisterDisclaimer.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">

    <script type="text/javascript">
    function chbAccept_onclick()
    {
        var obj=document.getElementById("<%=termAccept.ClientID %>");
        if (obj.checked)
        {
            hideMessage();
        }
        else
        {
            this.showNormalMessage('<%= GetTextByKey("acc_regHome_error_acceptTerms").Replace("'","\\'")%>', 'Error');
            
            document.getElementById("div_error_icon").style.display = "";
            return false;
        }

        return true;
    }
    
    function chbSelected_onclick()
    {
        var obj=document.getElementById("<%=termAccept.ClientID %>");
        if(obj.checked)
        {
            hideMessage();
            document.getElementById("div_error_icon").style.display="none";
        }    
    }
    
    function chbAccept(obj)
    {
        hideMessage();
        obj.style.background="";
    }        
    </script>

    <div class="ACA_RightContent">
        <h1>
            <ACA:AccelaLabel ID="acc_regHome_label_register" LabelKey="acc_regHome_label_register"
                runat="server" />
        </h1>
        <ACA:AccelaHeightSeparate ID="sepForDesclam" runat="server" Height="10" />
        <div class="ACA_Page ACA_Page_FontSize">
        <ACA:AccelaLabel ID="acc_regHome_text_register2" LabelKey="acc_regHome_text_register"
            runat="server" LabelType="bodyText" /></div>
        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" runat="server" Height="15" />
        <div class="DisclaimerContainerStyle ACA_ContainerStyle">
            <table role='presentation' width="100%" border="0" cellspacing="0" cellpadding="8">
                <tr>
                    <td>
                        <ACA:AccelaLabel ID="acc_disclaimer_label_disclaimer" LabelKey="acc_disclaimer_label_disclaimer"
                            runat="server" LabelType="BodyText" />
                        <ACA:AccelaLabel ID="acc_disclaimer_label_disclaimer1" LabelKey="acc_disclaimer_label_disclaimer1"
                            runat="server" LabelType="BodyText" />
                        <ACA:AccelaLabel ID="acc_disclaimer_label_disclaimer2" LabelKey="acc_disclaimer_label_disclaimer2"
                            runat="server" LabelType="BodyText" />
                        <ACA:AccelaLabel ID="acc_disclaimer_label_disclaimer3" LabelKey="acc_disclaimer_label_disclaimer3"
                            runat="server" LabelType="BodyText" />
                    </td>
                </tr>
            </table>
        </div>
        <ACA:AccelaHeightSeparate ID="sepForCheckBox" runat="server" Height="15" /> 
        <div class="ACA_TabRow ACA_SmallError_Icon" id="div_error_icon" style="display: none">
            <img class="ACA_NoBorder" alt="<%=GetTextByKey("img_alt_mark_required") %>" src="<%=ImageUtil.GetImageURL("error_16.gif") %>"/>
        </div>
        <div id="divCheckText" class="aca_checkbox aca_checkbox_fontsize">
            <input type="checkbox" id="termAccept" onclick="chbSelected_onclick(this);" runat="server" />
            <ACA:AccelaLabel ID="acc_regHome_label_acceptTerms" AssociatedControlID="termAccept" LabelKey="acc_regHome_label_acceptTerms"
                runat="server" CssClass="ACA_SmLabel ACA_SmLabel_FontSize"/>
        </div>
        <ACA:AccelaHeightSeparate ID="sepForButton" runat="server" Height="25" />
        <ACA:AccelaButton ID="btnRegister" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" runat="server" LabelKey="acc_regHome_label_continueReg" OnClick="RegisterButton_Click" OnClientClick="return chbAccept_onclick();"/>
    </div>
</asp:Content>
