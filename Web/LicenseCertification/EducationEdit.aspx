<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="EducationEdit.aspx.cs" Inherits="Accela.ACA.Web.LicenseCertification.EducationEdit" %>
<%@ Register src="~/Component/EducationDetailEdit.ascx" tagname="EducationDetailEdit" tagprefix="ACA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    <script src="<%=FileUtil.AppendApplicationRoot("Scripts/GeneralNameList.js")%>" type="text/javascript"></script>
    <script src="<%=FileUtil.AppendApplicationRoot("Scripts/Education.js")%>" type="text/javascript"></script>
    
    <asp:UpdatePanel ID="educationEditPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="Body">
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
                                <ACA:AccelaLabel ID="lblIncomplete" LabelKey="education_validate_required_errror_message" runat="server"> 
                                </ACA:AccelaLabel>
                            </td>
                        </tr>
                    </table>
                    <ACA:AccelaHeightSeparate ID="sepForMark" runat="server" Height="15" />
                </div>
                <div>
                    <ACA:AccelaSectionTitleBar ID="sectionTitleBar" ShowType="OnlyAdmin" Visible="False" runat="server" />
                    <ACA:EducationDetailEdit ID="educationDetailEdit" runat="server" />
                </div>
            </div>
            <div class="ACA_Row ACA_LiLeft Footer">
                <ul>
                    <li>
                        <ACA:AccelaButton ID="btnSave" runat="server" CausesValidation="true"
                            OnClientClick="ButtonClientClickEvent()" OnClick="SaveButton_Click" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"
                            DivEnableCss="ACA_LgButton ACA_LgButton_FontSize">
                        </ACA:AccelaButton>
                    </li>
                    <li>
                        <ACA:AccelaLinkButton ID="btnCancel" CausesValidation="False" CssClass="ACA_LinkButton" OnClientClick="parent.ACADialog.close();return false;" runat="server" />
                    </li>
                </ul>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <script type="text/javascript">
        $("#lnkBeginFocus", getParentDocument()).focus();
        
        function ButtonClientClickEvent() {
            if (typeof (SetNotAsk) != 'undefined') {
                SetNotAsk();
            }

            SetCurrentValidationSectionID('<%= educationDetailEdit.ClientID %>');
        }

        function CloseEduDialog(isUpdate) {
            var parentFocusObj = parent.ACADialog.focusObject;
            parent.ACADialog.close();
            SetParentLastFocus(parentFocusObj);
            parent.RefreshEducationList(isUpdate);
        }
    </script>
</asp:Content>
