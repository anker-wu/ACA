<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.LicenseEdit"
    CodeBehind="LicenseEdit.ascx.cs" %>
<%@ Register Src="LicenseView.ascx" TagName="LicenseView" TagPrefix="ACA" %>
<%@ Register Src="~/Component/Conditions.ascx" TagName="Conditions" TagPrefix="uc1" %>
<%@ Register Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="uc2" %>
    <script language="javascript" type="text/javascript">
        var defaultSelectText = "<%=WebConstant.DropDownDefaultText%>";
        var templateEMSEDDLATTRIBUTEFORNAME = "<%=ACAConstant.TEMPLATE_FIELD_NAME_ATTRIBUTE_FOR_EMSEDDL%>";
        var licenseAgency = "<%=hfLicenseAgencyCode.ClientID %>";
    </script>
    <script type="text/javascript" src="../Scripts/Template.js"></script>
    <ACA:AccelaHideLink ID="hlBegin" runat="server" AltKey="img_alt_form_begin" CssClass="ACA_Hide" />
    <asp:UpdatePanel ID="LicenseEditPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <ACA:AccelaLabel ID="errorMessageLabel" runat="server"></ACA:AccelaLabel>
            <ACA:AccelaTextBox ID="txtValidateSectionRequired" IsHidden="True" Validate="required" runat="server" />
            <div id="divLicenseButton" class="license_edit" runat="server">
                <div class="ACA_Row ACA_LiLeft action_buttons">
                    <ul>
                        <li>
                            <ACA:AccelaButton ID="btnSelectLPFromAccount" runat="server" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                                LabelKey="aca_licenseedit_label_selectfromaccount" CausesValidation="false" OnClientClick="SelectLPFromAccount()" Visible="False"></ACA:AccelaButton>
                        </li>
                        <li>
                            <ACA:AccelaButton ID="btnAddNewLP" runat="server" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                                LabelKey="aca_licenseedit_label_addnewlp" CausesValidation="false" OnClientClick="AddNewLP()" Visible="False"></ACA:AccelaButton>
                        </li>
                        <li>
                            <ACA:AccelaButton ID="btnLookUpLP" runat="server" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" 
                                LabelKey="aca_licenseedit_label_lookup" CausesValidation="false" OnClientClick="LookUpLP()" Visible="False"></ACA:AccelaButton>
                        </li>
                    </ul>
                </div>
            </div>
            <div id="divActionNotice" runat="server" visible="False">
                <div class="ACA_Error_Icon" runat="server" EnableViewState="False" id="divImgSuccess" visible="false">
                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("img_alt_workflow_complete") %>" src="<%=ImageUtil.GetImageURL("complete.png") %>" />
                </div>
                <div class="ACA_Error_Icon" runat="server" EnableViewState="False" id="divImgFailed" visible="false">
                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>"
                        src="<%=ImageUtil.GetImageURL("error_16.gif") %>" />
                </div>
                <div class="ACA_Notice font12px">
                    <ACA:AccelaLabel ID="lblActionNoticeAddSuccess" class="Notice_Message_Success" runat="server" EnableViewState="False" LabelKey="aca_lp_label_addedsuccessfully" Visible="false" />
                    <ACA:AccelaLabel ID="lblActionNoticeDeleteSuccess" class="Notice_Message_Success"  runat="server" EnableViewState="False" LabelKey="aca_lp_label_removedsuccessfully" Visible="false" />
                    <ACA:AccelaLabel ID="lblActionNoticeEditSuccess" class="Notice_Message_Success"  runat="server" EnableViewState="False" LabelKey="aca_lp_label_editedsuccessfully" Visible="false" />
                    <ACA:AccelaLabel ID="lblActionNoticeAddFailed" class="Notice_Message_Failure" runat="server" EnableViewState="False" LabelKey="aca_lp_label_addedfailed" Visible="false" />
                    <ACA:AccelaLabel ID="lblActionNoticeDeleteFailed" class="Notice_Message_Failure" runat="server" EnableViewState="False" LabelKey="aca_lp_label_removedfailed" Visible="false" />
                    <ACA:AccelaLabel ID="lblActionNoticeEditFailed" class="Notice_Message_Failure" runat="server" EnableViewState="False" LabelKey="aca_lp_label_editedfailed" Visible="false" />
                </div>
            </div>
            <div id="divLicenseView" runat="server" visible="False">
                <ACA:LicenseView ID="LicenseSummary" IsInConfirmPage="False" runat="server" />
                <div id="divLicenseViewAction" runat="server" class="ACA_LinkButton">
                    <div>
                        <ACA:AccelaLinkButton ID="btnEdit" runat="server" LabelKey="aca_licenseedit_label_edit" CausesValidation="false"></ACA:AccelaLinkButton>
                        <ACA:AccelaLinkButton ID="btnRemove" runat="server" class="ACA_Unit" LabelKey="aca_licenseedit_label_remove" OnClick="BtnRemoveClick" CausesValidation="false"></ACA:AccelaLinkButton>
                    </div>
                </div>
            </div>
            <uc2:MessageBar ID="spanLicenseNotice" runat="server" />
            <uc1:Conditions ID="ucConditon" runat="server" />
            <asp:HiddenField ID="hfLicenseProId" runat="server" />
            <asp:HiddenField ID="hfLicenseAgencyCode" runat="server" />
            <asp:HiddenField ID="hfLicenseType" runat="server" />
            <asp:HiddenField ID="hfLicenseNbr" runat="server" />
            <asp:HiddenField ID="hfLicenseTempID" runat="server"/>
            <asp:HiddenField ID="hfSSN" runat="server" Visible="false" />
            <asp:HiddenField ID="hfFEIN" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <ACA:AccelaHideLink ID="hlEnd" runat="server" AltKey="aca_common_msg_formend" CssClass="ACA_Hide" />
    <script type="text/javascript">
    var LicenseTypeConfigurationErrorMessage = '<%= GetTextByKey("aca_licensetype_readonly_error").Replace("'","\\'")%>';

    function MultiLicenseButtonClientClick()
    {
        if(typeof(SetNotAsk) != 'undefined')
        {
            SetNotAsk();
        }
        SetCurrentValidationSectionID('<%=ClientID %>'); 
    }
  
    function AfterLicenseButtonClick() 
    {
        SetNotAskForSPEAR();
        ResetIsShowLicenseConditionFlg();
    }

    var IsShowlicenseCondition = false;
    var initialLicenseConditionStatus="0";
    
    function ResetIsShowLicenseConditionFlg() 
    {
        IsShowlicenseCondition = false;
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
    
    //Call function is defined in Conditions.cs at line 346.
    function EndlicenseRequest(linkId,divConditions)
    {
        var lnk = document.getElementById(linkId);
        if (lnk != null) 
        {
            if (IsShowlicenseCondition) 
            {
                lnk.innerHTML = '<%=GetTextByKey("per_condition_Label_hide").Replace("'","\\'") %>';
                $get(divConditions).style.display=  '';
            }
            else 
            {
                lnk.innerHTML = '<%=GetTextByKey("per_condition_Label_show").Replace("'","\\'") %>';               
                $get(divConditions).style.display=  'none';
            }
        }
    }
    
    function <%=ClientID %>_SelectLPFromAccount() {
        if (typeof (SetNotAsk) != 'undefined') {
            SetNotAsk();
        }
        var url = '<%=FileUtil.AppendApplicationRoot("People/LicenseInput.aspx") %>?action=S&isLPList=<%=IsMultipleLicensedProfessional %>&agencyCode=<%=ConfigManager.AgencyCode %>&Module=<%=ModuleName %>&componentName=<%=ComponentName %>&parentControlId=<%=ParentControlID %>&isValidate=<%=IsValidate %>&isEditable=<%=IsEditable %>&isSubAgencyCap=<%=IsSubAgencyCap %>&isSectionRequired=<%=IsSectionRequired %>';
        var btnId = '<%=btnSelectLPFromAccount.ClientID %>';
        ACADialog.popup({ url: url, width: 800, height: 550, objectTarget: btnId });
        return false;
    }

    function <%=ClientID %>_AddNewLP() {
        if (typeof (SetNotAsk) != 'undefined') {
            SetNotAsk();
        }
        var url = '<%=FileUtil.AppendApplicationRoot("People/LicenseInput.aspx") %>?action=A&isLPList=<%=IsMultipleLicensedProfessional %>&agencyCode=<%=ConfigManager.AgencyCode %>&Module=<%=ModuleName %>&componentName=<%=ComponentName %>&parentControlId=<%=ParentControlID %>&isValidate=<%=IsValidate %>&isEditable=<%=IsEditable %>&isSubAgencyCap=<%=IsSubAgencyCap %>&isSectionRequired=<%=IsSectionRequired %>';
        var btnId = '<%=btnAddNewLP.ClientID %>';
        ACADialog.popup({ url: url, width: 800, height: 550, objectTarget: btnId });
        return false;
    }

    function <%=ClientID %>_LookUpLP() {
        if (typeof (SetNotAsk) != 'undefined') {
            SetNotAsk();
        }
        var url = '<%=FileUtil.AppendApplicationRoot("People/LicenseInput.aspx") %>?action=L&isLPList=<%=IsMultipleLicensedProfessional %>&agencyCode=<%=ConfigManager.AgencyCode %>&Module=<%=ModuleName %>&componentName=<%=ComponentName %>&parentControlId=<%=ParentControlID %>&isValidate=<%=IsValidate %>&isEditable=<%=IsEditable %>&isSubAgencyCap=<%=IsSubAgencyCap %>&isSectionRequired=<%=IsSectionRequired %>';
        var btnId = '<%=btnLookUpLP.ClientID %>';
        ACADialog.popup({ url: url, width: 800, height: 550, objectTarget: btnId });
        return false;
    }
            
    function <%=EditLicenseFunction %>() {
        var btnId = '<%=btnEdit.ClientID %>';
        var licenseType = $get('<%=hfLicenseType.ClientID %>').value;
        var licenseNbr = escape($get('<%=hfLicenseNbr.ClientID %>').value);
        var licenseTempID = $get('<%=hfLicenseTempID.ClientID %>').value;
        var url = '<%=FileUtil.AppendApplicationRoot("People/LicenseInput.aspx") %>?action=E&isLPList=<%=IsMultipleLicensedProfessional %>&agencyCode=<%=ConfigManager.AgencyCode %>&Module=<%=ModuleName %>&componentName=<%=ComponentName %>&parentControlId=<%=ParentControlID %>&isValidate=<%=IsValidate %>&isEditable=<%=IsEditable %>&isSubAgencyCap=<%=IsSubAgencyCap %>&isSectionRequired=<%=IsSectionRequired %>&licensetype='+licenseType+'&licenseNbr='+licenseNbr+'&licenseTempID='+encodeURIComponent(licenseTempID);
        ACADialog.popup({ url: url, width: 800, height: 550, objectTarget: btnId });
        return false;
    }
    
    function <%=ClientID %>_RemoveLicense() {
        if (confirm('<%=GetTextByKey("aca_common_delete_alert_message").Replace("'", "\\'") %>')) {
            SetNotAsk();
            return true;
        } else {
            return false;
        }   
    }
            
    function <%=ClientID %>_RefreshLP(actionType, componentName) {
        window.setTimeout(function () {
            delayShowLoading();
        }, 100);

        __doPostBack('<%=LicenseEditPanel.UniqueID + REFRESH_LICENSE%>',componentName+'$'+actionType);
    }
    
    function <%= ClientID %>_CheckSectionRequired4License() {
        if (!$.exists('#<%=divLicenseView.ClientID %>')) {
            var imgUrl = '<%=ImageUtil.GetImageURL("error_16.gif")%>';
            var errorMsg ='<%= GetTextByKey("aca_common_msg_validaterequiredfailed").Replace("'", "\\'") %>';
            ShowSectionRequiredMsg('<%=errorMessageLabel.ClientID %>', errorMsg, imgUrl);
            return false;
        }
       
        return true;
    }
    
    function <%=ClientID %>_ValidateFieldRequired4License() {
        var errorMsg ='<%= GetTextByKey("aca_license_msg_required_validate").Replace("'", "\\'")%>';
        showMessage('<%=errorMessageLabel.ClientID %>', errorMsg, "Error", true, 1, false);
        
        return false;
    }
    </script>
