<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ACAMap.ascx.cs" Inherits="Accela.ACA.Web.Component.ACAMap" %>

        <ACA:AccelaInlineScript runat="server" ID="inlinescriptOfACAMap">

            <script type="text/javascript">
                function <%=ScriptFunctionName%>(){
                    var xml = document.getElementById("<%=hfldXml.ClientID %>");
                    try{
                        var win =document.getElementById("<%=frmMap.ClientID %>").contentWindow;
                        if(win.SendRequest!=null && win.SendRequest!='undefined'){
                            win.SendRequest(xml.value);
                        }
                    }
                    catch(e){
                    }
                }
            </script>
        </ACA:AccelaInlineScript>
        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
        <div id="dvCancel" class="Map_Top" visible="false" runat="server">
            <div id="dvInstruction" runat="server"> 
                <ACA:AccelaLabel ID="lblTitle" LabelType="PageInstruction" LabelKey="aca_map_label_instruction" runat="server"></ACA:AccelaLabel>
            </div>
            <table role='presentation'>
                <tr>
                    <td>
                        <ACA:AccelaButton DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" ID="btnCancel" LabelKey="aca_gis_cancel_button_label" CausesValidation="false" runat="server" OnClick="CancelButton_Click" OnClientClick="javascript:NeedAsk=false;" />
                    </td>
                </tr>
            </table>
        </div>        
        <div id="dvMap" class="Map_Container" visible="false" runat="server">
            <iframe id="frmMap" visible="false" width="100%" height="100%" frameborder="0" runat="server">
                <%=String.Format(LabelUtil.GetGlobalTextByKey("iframe_nonsupport_message"), AppSession.CurrentURL)%>
            </iframe>
        </div>
         </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="dvGISButton" class="ACA_FRight Map_Bottom" runat="server">
            <table role='presentation'>
                <tr>
                    <td>
                        <ACA:AccelaImageButton ID="btnGIS" LabelKey="aca_gis_open_map_label" CssClass="ACA_SaveAndResumeLater_Icon"
                            CausesValidation="false" runat="server" OnClick="GISButton_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="ACA_MutipleContactNewLine" visible="false">
        </div>
        <div id="mapPopup" class="ACA_Hide" runat="server">
            <div id="popopContent" runat="server">
            <div class="ACAMap_Popup_Content aca_map_select_agency_radio">
            <ACA:AccelaRadioButtonList ID="rblAgency" runat="server"></ACA:AccelaRadioButtonList>
            </div>
            </div>
            <div id="divbutton" runat="server">
            <table class="ACA_Page font11px aca_map_popup_button_margin" role='presentation'cellspacing="0" cellpadding="0" border="0">
                <tr valign="bottom">
                 <td>
                 <div class="ACA_LgButton ACA_LgButton_FontSize aca_popup_ok_button">
                    <ACA:AccelaButton OnClientClick="SetNotAsk();ACADialog.close();" CausesValidation="false" ID="btnSubmit" OnClick="SubmitButton_Click" LabelKey="aca_mappopup_label_submit" runat="server"></ACA:AccelaButton>
                    </div>
                </td>
                <td  class="ACA_XShoter"></td>
                <td>
                    <div class="ACA_LinkButton">
                    <ACA:AccelaLinkButton ID="lnkPopupCancel" LabelKey="aca_mappopup_label_cancel" runat="server" OnClientClick="SetNotAsk();ACADialog.close();"></ACA:AccelaLinkButton>
                    </div>
                </td>
                </tr>
            </table>
            </div>
        </div>
        <div id="mappopupdiv" class="ACAMap_Popup_Div" runat="server"></div>
        <asp:HiddenField ID="hfldXml" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
