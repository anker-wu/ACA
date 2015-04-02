<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InspectionWizardInputType.aspx.cs"
    MasterPageFile="~/Dialog.Master" Inherits="Accela.ACA.Web.Inspection.InspectionWizardInputType" %>

<asp:content id="Content1" contentplaceholderid="phPopup" runat="server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    <div class="font11px">
        <asp:UpdatePanel ID="InspectionTypePanel" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hdInspectionType" runat="server" />
                <div>
                    <div class="ACA_TabRow ACA_Popup_Title">
                        <ACA:AccelaLabel ID="lblAvailableInspections" LabelKey="aca_inspection_available_lable" runat="server"></ACA:AccelaLabel>
                    </div>
                    <div class="ACA_Row ACA_LiLeft">
                        <ul> 
                            <li>
                                <ACA:AccelaDropDownList ID="ddlCategory" OnSelectedIndexChanged="CategoryDropDownList_SelectedIndexChanged" AutoPostBack="true" ToolTipLabelKey="aca_inspection_msg_changecategory_tip" runat="server"></ACA:AccelaDropDownList>
                            </li>
                            <li>
                                <ACA:AccelaCheckBox ID="chkShowOptional" OnCheckedChanged="ShowOptionalCheckBox_CheckedChanged" AutoPostBack="true" LabelKey="aca_inspection_show_optional_check" runat="server" />
                            </li>
                        </ul>
                    </div>
                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" Height="10" runat="server" />
                    <div class="ACA_TabRow_Italic">
                        <ACA:AccelaLabel ID="lblNoInspectionTypesFound" LabelKey="aca_inspection_types_notfound" runat="server"></ACA:AccelaLabel>
                    </div>
                    <div class="ACA_Label InspectionTypeGridView">
                        <ACA:AccelaGridView ID="gvInspectionType" CssClass="PopUpInspectionRow" OnPageIndexChanging="InspectionTypeGridView_PageIndexChanging"
                            runat="server" ShowHeader="false" ShowHeaderWhenEmpty="false" AlternatingRowStyle-CssClass="InspectionTypeRow" role="presentation"
                            RowStyle-CssClass="InspectionTypeRow" AllowPaging="True" PagerStyle-VerticalAlign="bottom" AutoGenerateCheckBoxColumn="false"
                            OnRowDataBound="InspectionTypeGridView_RowDataBound" IsAutoWidth="true">
                        <Columns>
                            <ACA:AccelaTemplateField>
                                <ItemTemplate>
                                    <ACA:AccelaRadioButton ID="rdInspectionType" Enabled='<%# DataBinder.Eval(Container.DataItem, "Enabled")%>' runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InspTypeLabel")%>' value='<%# DataBinder.Eval(Container.DataItem, Accela.ACA.Web.Inspection.InspectionParameter.Keys.TypeID)%>' 
                                            Checked='<%# DataBinder.Eval(Container.DataItem, "Selected")%>' onclick="SelectInspectionRadio(this)" onkeydown="SetGroupName()" />
                                </ItemTemplate>
                            </ACA:AccelaTemplateField>
                        </Columns>
                        </ACA:AccelaGridView>
                    </div>
                </div>
                <!-- button list -->
                <div class="ACA_TabRow">
                    <table role='presentation'>
                        <tr valign="bottom">
                            <td>
                                <div class="ACA_LgButton ACA_LgButton_FontSize">
                                    <ACA:AccelaButton ID="lnkContinue" OnClick="ContinueButton_Click" LabelKey="aca_inspection_action_continue" runat="server"/>
                                </div>
                                </td>
                                <td class="PopupButtonSpace">&nbsp;</td>
                                <td>
                                <div class="ACA_LinkButton">
                                    <ACA:AccelaLinkButton ID="lnkCancel" OnClientClick="parent.ACADialog.close(); return false;" LabelKey="ACA_Inspection_Action_Cancel" runat="server" />
                                </div>
                                </td>
                        </tr>
                    </table> 
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        function SetGroupName() {
            var inputs = document.getElementById('<%=gvInspectionType.ClientID%>').getElementsByTagName('input');
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == 'radio') {
                    if (inputs[i].name != "grpInspectionType") {
                        inputs[i].name = "grpInspectionType";
                    }
                } 
            } 
        }

        function SelectInspectionRadio(obj) {
            var inputs = document.getElementById('<%=gvInspectionType.ClientID%>').getElementsByTagName('input');
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == 'radio') {
                    if (inputs[i].id == obj.id) {
                        document.getElementById('<%=hdInspectionType.ClientID%>').value = inputs[i].value;
                        inputs[i].checked = true;
                    }
                    else {
                        inputs[i].checked = false;
                    }
                }
            }

            SetWizardButtonDisable('<%=lnkContinue.ClientID%>', false);
        }
    </script>    
</asp:content>
