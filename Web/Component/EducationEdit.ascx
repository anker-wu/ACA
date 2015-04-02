<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.EducationEdit"
    CodeBehind="EducationEdit.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="EducationList.ascx" TagName="EducationList" TagPrefix="uc1" %>

<asp:UpdatePanel ID="educationEditPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="ACA_Row">
            <div id="divAddButton" class="ACA_Row ACA_LiLeft" runat="server">
                <ul>
                    <li id="liAddFromSaved" runat="server">
                        <ACA:AccelaButton ID="btnAddFromSaved" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"
                            DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" CausesValidation="False"
                            LabelKey="aca_education_list_label_add_from_saved" runat="server" />
                    </li>
                    <li id="liAddNew" runat="server">
                        <ACA:AccelaButton ID="btnAddNew" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"
                            DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" CausesValidation="False" runat="server" />
                    </li>
                </ul>
                <ACA:AccelaHeightSeparate ID="sepForAddButton" runat="server" Height="15" />
            </div>
            <div id="divIncompleteMark" runat="server" visible="false">
                <table role='presentation' class="ACA_Message_Error ACA_Message_Error_FontSize" style="width:100%">
                    <tr>
                        <td>
                            <div class="ACA_Error_Icon">
                                <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>" src="<%=ImageUtil.GetImageURL("error_24.gif") %>"/>
                            </div>
                        </td>
                        <td>
                            <asp:Literal runat="server" ID="ltScriptForIncomplete" Mode="PassThrough"></asp:Literal>
                            <ACA:AccelaLabel ID="lblIncomplete" LabelKey="education_validate_required_errror_message"
                                runat="server"> 
                            </ACA:AccelaLabel>
                        </td>
                    </tr>
                </table>
                <ACA:AccelaHeightSeparate ID="sepForMark" runat="server" Height="15" />
            </div>
            <div>
                <uc1:EducationList ID="EducationList" runat="server" />
                <asp:LinkButton ID="btnRefreshEduList" runat="Server" CssClass="ACA_Hide" OnClick="RefreshEduListButton_Click" TabIndex="-1"></asp:LinkButton>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<ACA:AccelaHeightSeparate ID="sepForEducationEdit" runat="server" Height="25" Visible="False" />

<script language="javascript" type="text/javascript"> 
    if (typeof (myValidationErrorPanel) != "undefined") {
        myValidationErrorPanel.registerIDs4Recheck("<%= EducationList.ClientID %>");
    }

    function openEducationFormDialog(sender, isFromRefContact, rowIndex, editable, contactIsFromExternal) {
        var url = '<%=FileUtil.AppendApplicationRoot("LicenseCertification/EducationEdit.aspx") %>?rowIndex=' + rowIndex;
        
        if(isFromRefContact == "Y") {
            url += '&contactSeqNbr=<%=ContactSeqNbr %>&<%=UrlConstant.CONTACT_IS_FROM_EXTERNAL %>=' + contactIsFromExternal;
        }
        else {
            url += '&editable=' + editable + '&Module=<%=ModuleName %>&<%=UrlConstant.CONTACT_IS_FROM_EXTERNAL %>=' + contactIsFromExternal;
        }

        ACADialog.popup({ url: url, width: 800, height: 550, objectTarget: sender.id });
        return false;
    }

    function RefreshEducationList(data) {
        window.setTimeout(function () {
            delayShowLoading();
        }, 100);
        
        __doPostBack('<%=btnRefreshEduList.UniqueID %>', data);
    }
</script>

