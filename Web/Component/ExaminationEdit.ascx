<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.ExaminationEdit" Codebehind="ExaminationEdit.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="ExaminationList.ascx" TagName="ExaminationList" TagPrefix="uc1" %>

<asp:UpdatePanel ID="editPanel" runat="server" UpdateMode="conditional" >
    <ContentTemplate>
        <div class="ACA_Row">
            <div id="divAddButton" class="ACA_Row ACA_LiLeft" runat="server">
                <ul>
                    <li id="liAddFromSaved" runat="server">
                        <ACA:AccelaButton ID="btnAddFromSaved" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"
                            DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" CausesValidation="False"
                            LabelKey="aca_examination_list_label_add_from_saved" runat="server" />
                    </li>
                    <li id="liAddNew" runat="server">
                        <ACA:AccelaButton ID="btnAddNew" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"
                            DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" CausesValidation="False" runat="server" />
                    </li>
                </ul>
                <ACA:AccelaHeightSeparate ID="sepForAddButton" runat="server" Height="15" />
            </div>
            <!-- start Examination Instruction -->
            <div id="divIncompleteMark" runat="server" visible="false">
                <table role='presentation' class="ACA_Message_Error ACA_Message_Error_FontSize" style="width:100%">
                    <tr>
                        <td>
                            <div class="ACA_Error_Icon" >
                                <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>" src="<%=ImageUtil.GetImageURL("error_24.gif") %>"/>
                            </div> 
                        </td>
                        <td>
                            <asp:Literal runat="server" ID="ltScriptForIncomplete" Mode="PassThrough"></asp:Literal>
                            <ACA:AccelaLabel ID="lblIncomplete" LabelKey="examination_list_validate_required_error_message" runat="server" Width="100%" />
                        </td>
                    </tr>
                </table>
                <ACA:AccelaHeightSeparate ID="sepForMark" runat="server" Height="10" />
            </div>            
            <!-- end Examiantion Instruction -->
        </div>
        <div class="ACA_Row"> 
            <!-- start Examination List -->
            <div class="ACA_TabRow">
                <uc1:ExaminationList ID="examinationList" runat="server" />
                <asp:LinkButton ID="btnRefreshExamList" runat="Server" CssClass="ACA_Hide" OnClick="RefreshExamListButton_Click" TabIndex="-1"></asp:LinkButton>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<ACA:AccelaHeightSeparate ID="sepForExaminationEdit" runat="server" Height="25" Visible="False" />

<script language="javascript" type="text/javascript"> 
    if (typeof (myValidationErrorPanel) != "undefined") {
        myValidationErrorPanel.registerIDs4Recheck("<%=examinationList.ClientID %>");
    }

    function openExamFormDialog(sender, isFromRefContact, rowIndex, editable, contactIsFromExternal) {
        var url = '<%=FileUtil.AppendApplicationRoot("LicenseCertification/ExaminationEdit.aspx") %>?rowIndex=' + rowIndex;

        if (isFromRefContact == "Y") {
            url += '&contactSeqNbr=<%=ContactSeqNbr %>&<%=UrlConstant.CONTACT_IS_FROM_EXTERNAL %>=' + contactIsFromExternal;
        }
        else {
            url += '&editable=' + editable + '&Module=<%=ModuleName %>&<%=UrlConstant.CONTACT_IS_FROM_EXTERNAL %>=' + contactIsFromExternal;
        }

        ACADialog.popup({ url: url, width: 800, height: 550, objectTarget: sender.id });
        return false;
    }

    function RefreshExamList(isUpdate) {
        window.setTimeout(function () {
            delayShowLoading();
        }, 100);

        __doPostBack('<%=btnRefreshExamList.UniqueID %>', isUpdate);
    }
</script>

