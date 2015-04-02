<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageFlowActionBar.ascx.cs" Inherits="Accela.ACA.Web.Component.PageFlowActionBar" %>

<script type="text/javascript">
    var isConfirmPage = '<%=IsConfirmPage %>' == 'True';
    
    function IsSaveButtonEnabled() {
        // confirm page always enable the SaveAndResume button
        return isConfirmPage || !DisabelSave;
    }
 
    function doSaveAndResume() {
        if ($.global.isAdmin == false && IsSaveButtonEnabled()) {
            SetNotAsk();

            if (isConfirmPage) {
                __doPostBack("SaveAndResume", "");
            } else {
                return SaveAndResume();
            }
        }
  
        return false;
    }

    function Continue(btn) {
        if (isConfirmPage) {
            
            if(typeof(chbReviewAccept_OnClick)!="undefined") {
                var result= chbReviewAccept_OnClick();
                
                if(!result) {
                    return false;
                }
            }

            if (typeof (ShowEducationMessage) != "undefined") {
                ShowEducationMessage();
            }

            SetCurrentValidationSectionID('');
            if (typeof (HasPendingDocs) != 'undefined') {
                if (HasPendingDocs()) {
                    hideMessage();
                    var divTipMsg = $get("divTipMsg"); // this is exist in CapConfirm page.
                    divTipMsg.style.top = getElementTop($get('divContent')) + 'px';
                    divTipMsg.style.height = getElementTop(btn) + 'px';
                    divTipMsg.style.display = "block";
                    divTipMsg.scrollIntoView();
                    return false;
                }
            }
            SetNotAsk(true, btn);
        }

        if (typeof(SubmitEP) != "undefined") {
            return SubmitEP(btn);   
        }
        
        return true;
    }

    //Currently including the Continue/Back to Associated Forms/Save and Resume later buttons
    function DisableActionButtons(disabled) {
        DisableContinueButton(disabled);
        DisableBackToAFButton(disabled);
        DisableSaveButton(disabled);
    }

    //Continue button disable or not
    function DisableContinueButton(disabled) {
        var btnContinue = document.getElementById('<%=btnContinue.ClientID %>');

        if (btnContinue) {
            if (disabled) {
                btnContinue.parentNode.setAttribute("class", "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
                btnContinue.parentNode.setAttribute("className", "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
            }
            else {
                btnContinue.parentNode.setAttribute("class", "ACA_LgButton ACA_LgButton_FontSize");
                btnContinue.parentNode.setAttribute("className", "ACA_LgButton ACA_LgButton_FontSize");
            }

            DisableButton(btnContinue.id, disabled);
        }
    }

    //BackToAssoForm button disable or not
    function DisableBackToAFButton(disabled) {
        if ('<%=liBackToAssoForm.Visible %>' == 'False') {
            return;
        }

        var btnBackToAssoForm = $get('<%=btnBackToAssoForm.ClientID %>');
        if (btnBackToAssoForm) {
            DisableButton(btnBackToAssoForm.id, disabled);
        }
    }

    //Save and Resume later button disable or not
    function DisableSaveButton(disabled) {
        if ('<%=divShowSaveandResume.Visible %>' == 'False') {
            return;
        }

        DisableButton('<%=lblSaveAndResume.ClientID %>', disabled);
        DisableButton('<%=btnSave.ClientID %>', disabled);
        DisabelSave = disabled;
    }
    

    function focusContinueButton() {
        if ($get('<%=btnContinue.ClientID %>') != null) {
            $get('<%=btnContinue.ClientID %>').focus();
        }
    }
    
    window.onbeforeunload = function () {
        if (NeedAsk && '<%=divShowSaveandResume.Visible %>' == 'True' && $.global.isAdmin == false) {
            var p = new ProcessLoading();
            p.hideLoading();
            return '<%=GetTextByKey("per_cap_edit_exitAsk").Replace("'","\\'") %>';
        }
    };
</script>
<div class="ACA_Row">
    <div class="ACA_FLeft ACA_LiLeft">
        <ul>
            <li>
                <ACA:AccelaButton ID="btnContinue" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" runat="server" OnClick="ContinueButton_Click" OnClientClick="return Continue(this);"></ACA:AccelaButton>
            </li>
            <li id="liBackToAssoForm" runat="server">
                <div class="ACA_LinkButton">
                    <a id="btnBackToAssoForm" runat="server">
                        <span><%=LabelUtil.GetTextByKey("per_applypermit_label_backto_associatedforms",ModuleName)%></span>
                    </a>
                    <ACA:AccelaLinkButton ID="btnBackToAssoFormAdmin" LabelKey="per_applypermit_label_backto_associatedforms" runat="server"></ACA:AccelaLinkButton>
                </div>
            </li>
            <li id="liPayAtCounter" runat="server" Visible="false">
                <div class="ACA_LinkButton">
                        <ACA:AccelaButton ID="lnkPayAtCounter" LabelKey="per_permitFee_label_payAtCounter" runat="server" OnClick="PayAtCounterLink_Click" OnClientClick="SetNotAsk(true);" 
                        DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" />
                </div>
            </li>
        </ul>
    </div>
    <div class="ACA_FRight ACA_AlignRightOrLeft" id="divShowSaveandResume" runat="server" visible="False">
        <table role='presentation' border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <ACA:AccelaLabel runat="server" ID="lblSaveAndResume" CssClass="ACA_Label font13px" LabelKey="per_permitReg_label_saveAndResume"></ACA:AccelaLabel>
                </td>
                <td>
                    <div class="ACA_SaveAndResumeLater_Icon">
                        <a id="btnSave" runat="server" href="javascript:void(0);" onclick="SetNotAsk(); doSaveAndResume();">
                            <img alt="<%=LabelUtil.RemoveHtmlFormat(GetTextByKey("per_permitReg_label_saveAndResume|tip")) %>" onmouseout="if(IsSaveButtonEnabled())this.src='<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("save-normal.gif") %>'"
                                onmouseover="if(IsSaveButtonEnabled())this.src='<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("save-over.gif") %>'"
                                onmousedown="if(IsSaveButtonEnabled())this.src='<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("save-down.gif") %>'"
                                src="<%= ImageUtil.GetImageURL("save-normal.gif")%>" class="ACA_NoBorder"/>
                        </a>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
<div class="clear"></div>