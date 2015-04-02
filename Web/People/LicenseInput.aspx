<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LicenseInput.aspx.cs" 
    MasterPageFile="~/Dialog.Master" Inherits="Accela.ACA.Web.People.LicenseInput" %>
<%@ Register Src="~/Component/Conditions.ascx" TagName="Conditions" TagPrefix="ACA" %>
<%@ Register Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="ACA" %>
<%@ Register Src="../Component/LicenseInput.ascx" TagName="LicenseInput" TagPrefix="ACA" %>
<%@ Register Src="../Component/RefLicenseList.ascx" TagName="RefLicenseList" TagPrefix="ACA" %>
<asp:content id="Content1" contentplaceholderid="phPopup" runat="server">
    <script src="../Scripts/global.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pressEnterFlag = false;
        
        $(document).ready(function () {
            SetWizardButtonDisable("<%=btnContinue.ClientID %>", !<%=(AppSession.IsAdmin || refLicenseList.HasCheckedItems).ToString().ToLower() %>);
            
            $("#<%=divNewLicenseEditForm.ClientID %>").keydown(function (event) {
                if (pressEnterFlag) {
                    return;
                }
                
                var isEffective = pressEnter2TriggerClick(event, $("#<%=btnSearch.ClientID %>"));
                if (isEffective) {
                    pressEnterFlag = true;
                    setTimeout(InitFlag, 1000);
                }
            });
        });

        function InitFlag() {
            pressEnterFlag = false;
        }
        
        // This function will be used by UserControl [RefLicenseList.ascx], do NOT delete.
        function GetCurrentContinueButtonClientID() {
            return "<%=btnContinue.ClientID %>";
        }

        function ClosePopup(actionType) {
            var parentFocusObj = parent.ACADialog.focusObject;
            parent.ACADialog.close();
            SetParentLastFocus(parentFocusObj);
            RefreshLicense(actionType);
        }

        function RefreshLicense(actionType) 
        {
            if (<%= IsMultipleLPList.ToString().ToLowerInvariant() %>) 
            {
                parent.<%=ParentControlID %>_RefreshLPList(actionType, '<%=ComponentName %>');
            } 
            else 
            {
                parent.<%=ParentControlID %>_RefreshLP(actionType, '<%=ComponentName %>');
            }
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndlicenseRequest);

        if (typeof (myValidationErrorPanel) != "undefined") {
            if ($("#<%=lnkCancel.ClientID %>").is(":visible")) {
                myValidationErrorPanel.registerIDs4Recheck("<%=lnkCancel.ClientID %>");
            }
            
            if ($("#<%=lnkSearchDisCard.ClientID %>").is(":visible")) {
                myValidationErrorPanel.registerIDs4Recheck("<%=lnkSearchDisCard.ClientID %>");
            }
        }
        
        function showMorelicenseCondition(div,a)
        {
            if(div=='undefined') return;
            if(initialLicenseConditionStatus=="0"){
                IsShowlicenseCondition=false;
                initialLicenseConditionStatus="1";
            }
            
            $get(div).style.display= IsShowlicenseCondition?'none': '';
            $get(a).innerHTML = IsShowlicenseCondition?'<%=GetTextByKey("per_condition_Label_show").Replace("'","\\'") %>':'<%=GetTextByKey("per_condition_Label_hide").Replace("'","\\'") %>';
            IsShowlicenseCondition = !IsShowlicenseCondition;
        }
        
        var IsShowlicenseCondition = false;
        var initialLicenseConditionStatus="0";
    
        function ResetIsShowLicenseConditionFlg() 
        {
            IsShowlicenseCondition = false;
        }
    
        //Call function is defined in Conditions.cs at line 346.
        function EndlicenseRequest(linkId,divConditions)
        {
            var lnk = document.getElementById(linkId);
            if (lnk != null) 
            {
                if (IsShowlicenseCondition) 
                {
                    lnk.innerHTML = '<%=GetTextByKey("per_condition_Label_hide").Replace("'","\\'") %>'
                    $get(divConditions).style.display=  '';
                }
                else 
                {
                    lnk.innerHTML = '<%=GetTextByKey("per_condition_Label_show").Replace("'","\\'") %>'                
                    $get(divConditions).style.display=  'none';
                }
            }
        }
    </script>
    <div id="divNewLicenseForm" class="new_license_edit">
    <ACA:AccelaLabel ID="lblNewLicenseForm" runat="server" LabelType="SectionExText" Visible="False"/>
    <ACA:MessageBar id="licenseNoticeBar" runat="server" />
    <ACA:Conditions ID="licenseCondition" runat="server" Visible="false" />
    <div ID="divNewLicenseEditForm" runat="server" Visible="False" class="ACA_Row">
        <ACA:LicenseInput ID="licenseInput" runat="server" />
    </div>
    <div id="divSearchButton" class="ACA_Row ACA_LiLeft action_buttons" runat="server" Visible="False">
        <ul>
            <li id="liSearch" runat="server" Visible="False">
                <ACA:AccelaButton ID="btnSearch" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" LabelKey="LicenseEdit_LicensePro_label_buttonSearchEdit" OnClick="BtnSearchClick" CausesValidation="false" runat="server"/>
            </li>
            <li id="liSearchClear" runat="server" Visible="False">
                <ACA:AccelaButton ID="btnSearchClear" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" LabelKey="LicenseEdit_LicensePro_label_buttonClear" OnClick="BtnClearClick" CausesValidation="false" runat="server"/>
            </li>
            <li id="liSearchDisCard" runat="server"  Visible="False">
                <div class="ACA_vertical_align">
                    <ACA:AccelaLinkButton ID="lnkSearchDisCard"  CssClass="ACA_LinkButton"  LabelKey="aca_newlicenseedit_label_cancel" CausesValidation="false" OnClientClick="parent.ACADialog.close();return false;" runat="server"/>
                </div>
            </li>
        </ul>
    </div>
    <div id="divRefLicenseList" runat="server" Visible="False" class="ACA_Row">
            <div>
                <ACA:AccelaLinkButton ID="lnkReviseSearch" class="ACA_LinkButton" runat="server" LabelKey="aca_newlicenseform_label_revisesearch" Visible="False" OnClick="LnkBackSearchForm"></ACA:AccelaLinkButton>
            </div>
            <ACA:AccelaLabel ID="lblSearchLicenseList" runat="server" LabelKey="aca_newlicenseform_label_searchresult" Visible="false" />
        <ACA:RefLicenseList id="refLicenseList" runat="server" />
    </div>
    <div id="divNewLicenseButton" class="ACA_Row ACA_LiLeft action_buttons" runat="server" Visible="False">
        <ul>
            <li id="liContinue" runat="server" Visible="False">
                <ACA:AccelaButton ID="btnContinue" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"  LabelKey="aca_newlicenseedit_label_continue" OnClick="BtnContinueClick" runat="server"/>
            </li>
            <li id="liSaveAndClose" runat="server" Visible="False">
                <ACA:AccelaButton ID="btnSaveAndClose" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" LabelKey="aca_newlicenseedit_label_saveandclose" OnClientClick="return SubmitEP(this);" OnClick="BtnSaveAndCloseClick" runat="server" />
            </li>
            <li id="liClear" runat="server" Visible="False">
                <ACA:AccelaButton ID="btnClear" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" LabelKey="LicenseEdit_LicensePro_label_buttonClear" OnClick="BtnClearClick" CausesValidation="false" runat="server"/>
            </li>
            <li id="liCancel" runat="server" Visible="False">
                <div class="ACA_vertical_align">
                    <ACA:AccelaLinkButton ID="lnkCancel" LabelKey="aca_newlicenseedit_label_cancel" CssClass="ACA_LinkButton" CausesValidation="false" OnClientClick="parent.ACADialog.close();return false;" runat="server"/>
                </div>
            </li>
        </ul>
    </div>
    </div>
</asp:content>