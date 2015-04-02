<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="AssociatedForms.aspx.cs" Inherits="Accela.ACA.Web.Cap.AssociatedForms" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <script type="text/javascript">
        function SaveAndResume() {
            if ($.global.isAdmin == false) {
                __doPostBack("SaveAndResume", "");
            }
        }

        function DisableContinueButton(disable) {
            var continueBtnId = '<%=btnContinue.ClientID %>';
            var btnContinue = $get(continueBtnId);
            if (btnContinue) {
                if (disable) {
                    btnContinue.parentNode.setAttribute("class", "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
                    btnContinue.parentNode.setAttribute("className", "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
                    DisableButton(continueBtnId, true);
                }
                else {
                    btnContinue.parentNode.setAttribute("class", "ACA_LgButton ACA_LgButton_FontSize");
                    btnContinue.parentNode.setAttribute("className", "ACA_LgButton ACA_LgButton_FontSize");
                    DisableButton(continueBtnId, false);
                }
            }
        }

        function DisableSaveAndResumeButton() {
            $("#<%=divShowSaveandResume.ClientID %>").addClass("ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
            DisableButton("btnSaveAndResumeLater", true);
        }
    </script>
    <div class="ACA_Content Associated_Forms">
        <ACA:BreadCrumpToolBar ID="breadCrumbToolBar" runat="server"></ACA:BreadCrumpToolBar>
        <div class="title">
            <ACA:AccelaLabel ID="lblTitle" LabelType="LabelText" LabelKey="aca_associated_forms_title" runat="server"></ACA:AccelaLabel>
        </div>
        <div class="desc">
            <ACA:AccelaLabel ID="lblListedItemsDesc" LabelType="BodyText" LabelKey="aca_associated_forms_listed_items_desc" runat="server"></ACA:AccelaLabel>
        </div>
        <div class="itemsdesc">
            <ACA:AccelaLabel ID="lblAddedItems" LabelType="LabelText" LabelKey="aca_associated_forms_added_items" runat="server"></ACA:AccelaLabel>
        </div>
        <div id="divPatternConfig" class="patternconfig" visible="false" runat="server">
            <div class="patternconfigdesc">
                <p>Click the area below to edit your list formats.</p>
                <p>Available variables:</p>
                <p>&nbsp;&nbsp;&nbsp;&nbsp;<%=PATTERN_RECORDTYPE_ALIAS%>: Record type alias.</p>
                <p>&nbsp;&nbsp;&nbsp;&nbsp;<%=PATTERN_APPLICATION_NAME%>: Record's Application Name.</p>
                <p>&nbsp;&nbsp;&nbsp;&nbsp;<%=PATTERN_GENERAL_DESCRIPTION%>: General Description of the Record.</p>
            </div>
            <br />
            <div>
                <ACA:AccelaLabel ID="lblChildCapPattern" LabelType="BodyText" LabelKey="aca_associated_forms_childcap_pattern" runat="server"></ACA:AccelaLabel>
            </div>
        </div>
        <table role="presentation" class="childcaplist">
        <asp:Repeater ID="childCapList" OnItemDataBound="ChildCapList_ItemDataBound" OnItemCommand="ChildCapList_ItemCommand" runat="server">
            <ItemTemplate>
                <tr class="ACA_TabRow_Odd ACA_TabRow_Odd_FontSize">
                    <td class="childcaptitle">
                        <span id="lblChildCapTitle" runat="server"></span>
                    </td>
                    <td class="childcapaction">
                        <ACA:AccelaLinkButton ID="lnkChildCapAction" runat="server"></ACA:AccelaLinkButton>
                    </td>
                    <td class="childcapaction">
                        <ACA:AccelaLinkButton ID="lnkRemoveAction" runat="server"></ACA:AccelaLinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="ACA_TabRow_Even ACA_TabRow_Even_FontSize">
                    <td class="childcaptitle">
                        <span id="lblChildCapTitle" runat="server"></span>
                    </td>
                    <td class="childcapaction">
                        <ACA:AccelaLinkButton ID="lnkChildCapAction" runat="server"></ACA:AccelaLinkButton>
                    </td>
                    <td class="childcapaction">
                        <ACA:AccelaLinkButton ID="lnkRemoveAction" runat="server"></ACA:AccelaLinkButton>
                    </td>
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
        </table>
        <div class="ACA_LgButton ACA_LgButton_FontSize">
          <ACA:AccelaButton ID="btnContinue" LabelKey="per_permitConfirm_label_continue" runat="server" OnClick="ContinueButton_Click"></ACA:AccelaButton>
        </div>
        <div class="ACA_Title_Button ACA_AlignRightOrLeft" id="divShowSaveandResume" runat="server">
            <table role='presentation' border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <ACA:AccelaLabel runat="server" ID="lbl" CssClass="ACA_Label font13px" LabelKey="per_permitReg_label_saveAndResume"></ACA:AccelaLabel>
                    </td>
                    <td>
                        <div class="ACA_SaveAndResumeLater_Icon">
                            <a id="btnSaveAndResumeLater" href="javascript:void(0);" class="NeedValidate" onclick="SaveAndResume();" title="<%=GetTitleByKey("per_permitReg_label_saveAndResume|tip","") %>">
                                <img alt="<%=LabelUtil.RemoveHtmlFormat(GetTextByKey("per_permitReg_label_saveAndResume|tip")) %>" onmouseout="this.src='<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("save-normal.gif") %>'"
                                    onmouseover="this.src='<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("save-over.gif") %>'"
                                    onmousedown="this.src='<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("save-down.gif") %>'"
                                    src="<%= ImageUtil.GetImageURL("save-normal.gif")%>" class="ACA_NoBorder" style="cursor:pointer;" />
                            </a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
