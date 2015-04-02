<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CapClone.aspx.cs" 
    MasterPageFile="~/Dialog.Master" Inherits="Accela.ACA.Web.Cap.CapClone" %>

<asp:content id="Content1" contentplaceholderid="phPopup" runat="server">
    <div>
        <asp:UpdatePanel ID="CloneRecordPanel" runat="server" UpdateMode="conditional">        
        <ContentTemplate>
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" Height="10" runat="server"></ACA:AccelaHeightSeparate>
            <div class="ACA_TabRow">
                <div class="ACA_TabRow font11px">
                    <ACA:AccelaLabel ID="lblCloneComponents" LabelKey="aca_capclone_label_record_components" runat="server" CssClass="ACA_Popup_Title"></ACA:AccelaLabel>
                    <ACA:AccelaLabel ID="lblCloneComponentsNote" LabelKey="aca_capclone_label_record_components_note" runat="server" CssClass ="ACA_Label"></ACA:AccelaLabel>
                </div>
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" Height="10" runat="server"/>        
                <div class="ACA_TabRow">
                    <table role="presentation" runat="server" id="tbComponents" class ="ACA_FullWidthTable">
                        <tr>
                            <td class="ACA_Clone_Container_First_TD">
                                <ACA:AccelaCheckBox ID = "chkAddressList" Enabled="false"  LabelKey="aca_capclone_label_component_address" runat="server" />
                            </td>
                            <td>
                                <ACA:AccelaCheckBox ID = "chkAppSpecificInfo" Enabled="false"  LabelKey="aca_capclone_label_component_asi" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ACA:AccelaCheckBox ID = "chkParcelList" Enabled="false"  LabelKey="aca_capclone_label_component_parcel" runat="server" />
                            </td>
                            <td>
                                <ACA:AccelaCheckBox ID = "chkAppSpecificInfoTable" Enabled="false"  LabelKey="aca_capclone_label_component_asit" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ACA:AccelaCheckBox ID = "chkOwnerList" Enabled="false"  LabelKey="aca_capclone_label_component_owner" runat="server" />
                            </td>
                             <td>
                                <ACA:AccelaCheckBox ID="chkAssetList" Enabled="false"  LabelKey="aca_capclone_label_assets" runat="server" />
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                <ACA:AccelaCheckBox ID = "chkEducation" Enabled="false"  LabelKey="aca_capclone_label_component_education" runat="server" />
                            </td>
                            <td>
                                <ACA:AccelaCheckBox ID = "chkProfessional" Enabled ="false"  LabelKey="aca_capclone_label_component_professional" runat="server" />
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                <ACA:AccelaCheckBox ID = "chkContEducation" Enabled ="false"  LabelKey="aca_capclone_label_component_continue_education" runat="server" />
                            </td>
                            <td>
                                <ACA:AccelaCheckBox ID = "chkContacts" Enabled ="false"  LabelKey="aca_capclone_label_component_contact" runat="server" />
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                <ACA:AccelaCheckBox ID = "chkExamination" Enabled ="false"  LabelKey="aca_capclone_label_component_examination" runat="server" />
                            </td>
                            <td>
                                <ACA:AccelaCheckBox ID = "chkDetailInformation" Enabled ="false"  LabelKey="aca_capclone_label_component_detail_information" runat="server" />
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                <ACA:AccelaCheckBox ID = "chkValuationCalculator" Enabled ="false"  LabelKey="aca_capclone_label_component_valuation_calculator" runat="server" />
                            </td>
                            <td>
                                <ACA:AccelaCheckBox ID = "chkAdditionInfo" Enabled ="false"  LabelKey="aca_capclone_label_component_addition_information" runat="server" />
                            </td>
                            
                        </tr>
                    </table>
                </div>
            </div>
            <div class="ACA_TabRow font11px">
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate3" Height="15" runat="server"></ACA:AccelaHeightSeparate>
                <ACA:AccelaLabel ID="lblCloneBottomDescrition" LabelKey="aca_capclone_label_bottom_description" runat="server" LabelType="BodyText" CssClass="ACA_Label"></ACA:AccelaLabel>
            </div>
            <!-- button list -->
            <div class="ACA_TabRow">
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate4" Height="5" runat="server" />        
                <table role='presentation'>
                    <tr valign="bottom">
                        <td>
                                <ACA:AccelaButton ID="lnkCloneRecord" OnClick="CloneRecordButton_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" LabelKey="aca_capclone_label_action_clone" runat="server"/>
                         </td>
                         <td class="PopupButtonSpace">&nbsp;</td>
                         <td>
                            <div class="ACA_LinkButton">
                                <ACA:AccelaLinkButton ID="lnkCancel" OnClientClick="window.parent.ACADialog.close(); return false;" LabelKey="aca_capclone_label_action_cancel" runat="server" />
                            </div>
                         </td>
                    </tr>
                </table> 
            </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        function EableDisableCloneButton() {
            var hasSelectedComponent = false;
            var tbComponents = document.getElementById("<%=tbComponents.ClientID %>");
            var chkComponents = tbComponents.getElementsByTagName("input");
            var lnkCloneRecord = document.getElementById("<%=lnkCloneRecord.ClientID %>");
            
            if (chkComponents != "undefined")
            {
                for (var i = 0; i < chkComponents.length; i++)
                {
                    if (chkComponents[i].type == "checkbox" && chkComponents[i].checked)
                    {
                        hasSelectedComponent = true;
                        break;
                    }
                }
            }
            
            if (hasSelectedComponent) {
                lnkCloneRecord.parentNode.setAttribute("class", "ACA_LgButton ACA_LgButton_FontSize");
                lnkCloneRecord.parentNode.setAttribute("className", "ACA_LgButton ACA_LgButton_FontSize");
                DisableButton(lnkCloneRecord.id, false);
            }
            else {
                lnkCloneRecord.parentNode.setAttribute("class", "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
                lnkCloneRecord.parentNode.setAttribute("className", "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
                DisableButton(lnkCloneRecord.id, true);
            }
        }
    </script>
</asp:content>