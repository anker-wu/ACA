<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.SupervisorList" Codebehind="SupervisorList.ascx.cs" %>
<%@ Register src="SupervisorEdit.ascx" TagName="SupervisorEdit" TagPrefix="uc1" %>

 <div> 
    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
       <div style="padding-left:8px;padding-right:5px;padding-top:8px;padding-bottom:5px;">
            <div style="padding-bottom:5px;">
                <ACA:AccelaLabel ID="lblInstruction"  LabelKey="supervisor_edit_instruction" runat="server" CssClass="ACA_Label ACA_Label_FontSize"></ACA:AccelaLabel>
           </div>
           <div style="overflow-y:auto; overflow-x:hidden; height:135px; margin-bottom:5px;">
           <asp:DataList ID="dlAttributesList4EMSE" runat="server" OnItemDataBound="AttributesList4EMSE_ItemDataBound" role='presentation'>
                <ItemTemplate> 
                    <h1> 
                        <ACA:AccelaLabel ID="lblAgencyCode" runat="server"></ACA:AccelaLabel>&nbsp;
                        <ACA:AccelaLabel ID="lblLicenseType" runat="server"></ACA:AccelaLabel>&nbsp;
                        <ACA:AccelaLabel ID="lblLicenseNbr" runat="server"></ACA:AccelaLabel>
                    </h1>
                    <div style="padding-left:8px;">
                    <uc1:SupervisorEdit ID="SupervisorList"  runat="server" /></div> 
                </ItemTemplate>
            </asp:DataList>
            </div>
            <table role='presentation'>
                <tr>
                    <td>
                        <div class="ACA_SmButton ACA_LgButton ACA_LgButton_FontSize">
                            <ACA:AccelaLinkButton ID="btnSave" runat="server" LabelKey="supervisor_supervisoredit_control_save" CausesValidation="false" OnClientClick="SetNotAsk();"  OnClick="BtnSave_Click">
                            </ACA:AccelaLinkButton>
                        </div>
                    </td><td>&nbsp;&nbsp;</td>
                    <td>
                         <div class="ACA_SmButton ACA_LgButton ACA_LgButton_FontSize">
                            <ACA:AccelaLinkButton ID="btnCancel" runat="server"  LabelKey="supervisor_supervisoredit_control_cancel">
                            </ACA:AccelaLinkButton>
                        </div>
                    </td>
                </tr>
            </table>  
        </div> 
    </ContentTemplate>
    </asp:UpdatePanel>
</div>

<script language="javascript" type="text/javascript">
if (typeof (myValidationErrorPanel) != "undefined") {
    myValidationErrorPanel.registerIDs4Recheck("<%=btnCancel.ClientID %>");
}

function CloseAddForm_<%=btnSave.ClientID %>(obj)
{ 
    var divSupervisor = window.document.getElementById("divSupervisor");;
    divSupervisor.style.display='none';
}

 function ExitAddForm_<%=btnCancel.ClientID %>(obj)
 {
    var divSupervisor = obj.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode;
    document.getElementById(divSupervisor.id).style.display='none';
 }
</script>