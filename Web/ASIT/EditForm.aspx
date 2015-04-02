<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="EditForm.aspx.cs" Inherits="Accela.ACA.Web.ASIT.EditForm" %>
<%@ Register Src="~/Component/AppSpecInfoTableEdit.ascx" TagName="ASITEdit" TagPrefix="ACA" %>
<%@ Import Namespace="Accela.ACA.Web.Util" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
<script type="text/jscript" src="../Scripts/Validation.js"></script>
<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    function ReloadListAndCloseDialog(tableKeys) {
        var postBackArg = parent.ASIT_BuildPostBackArgument(tableKeys);
        parent.SetNotAskForSPEAR();

        //Set focus object for Section508.
        //defined in Dialog.Master page.
        SetParentLastFocus(parent.ACADialog.focusObject);
        parent.__doPostBack(postBackArg.EventTarget + '<%=ASITUIModelUtil.ASIT_POSTEVENT_SYNCUICOPYDATA %>', postBackArg.EventArgument);

        CloseDialog();
    }

    function CancelEditForm() {
        //Clear the ASIT UI data from session.
        parent.ASIT_ClearUIData();
        CloseDialog();
    }

    function CloseDialog() {
        //Restore the click event for ACADialog close button.
        var dialogCloseBtn = parent.$get(parent.ACADialog.close_id);

        if (dialogCloseBtn) {
            dialogCloseBtn.onclick = parent.ACADialog.close;
        }

        parent.ACADialog.close();
    }

    prm.add_pageLoaded(function (sender, args) {
        //Override ACADialog.close event.
        var dialogCloseBtn = parent.$get(parent.ACADialog.close_id);

        if (dialogCloseBtn) {
            dialogCloseBtn.onclick = function () {
                var parentIframe = parent.$get(parent.ACADialog.iframe_id);

                if (parentIframe) {
                    parent.SetNotAskForSPEAR();
                    parentIframe.contentWindow.CancelEditForm();
                }

                return false;
            };
        }

        //Render expression result to new added fields.
        if (parent.ASITInsertFieldCollection) {
            HandleNormalResult(parent.ASITInsertFieldCollection, false);
            parent.ASITInsertFieldCollection = [];
        }
        
        //Focus on top of the dialog after the page loaded - may be by expression post-back.
        parent.ACADialog.setFocus();
    });
</script>
    <div class="asit_editform">
        <asp:CustomValidator ID="cvOnlyForJSOutput" runat="server"></asp:CustomValidator>
        <asp:Repeater runat="server" ID="rptAppInfoTableList" OnItemDataBound="AppInfoTableList_ItemDataBound">
            <ItemTemplate>
                <ACA:ASITEdit ID="asitEdit" runat="server" />
            </ItemTemplate>
        </asp:Repeater>

        <div class="ACA_TabRow">
            <table role='presentation'>
                <tr valign="bottom">
                    <td>
                        <ACA:AccelaButton ID="btnSubmit" runat="server" LabelKey="aca_asit_label_editform_submit" CssClass="NotShowLoading"
                            DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"
                            OnClick="SubmitEditForm" OnClientClick="return SubmitEP(this);"/>
                    </td>
                    <td class="PopupButtonSpace">
                        &nbsp;
                    </td>
                    <td>
                        <ACA:AccelaLinkButton ID="btnCancel" runat="server" LabelKey="aca_asit_label_editform_cancel" CssClass="NotShowLoading ACA_LinkButton" 
                            OnClientClick="CancelEditForm();return false;" CausesValidation="False" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>