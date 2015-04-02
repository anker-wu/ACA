<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaymentCheckProcessing.ascx.cs"
    Inherits="Accela.ACA.Web.Component.PaymentCheckProcessing" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<div class="ACA_TabRow ACA_LiLeft">
    <div class="ACA_TabRow ACA_LiLeft">
        <h1>
            <ACA:AccelaLabel ID="per_permitPayFee_label_bankAccountInfo" LabelKey="per_permitPayFee_label_bankAccountInfo"
                runat="server" /></h1>
    </div>
    <div id="divBankAccountInfo">
        <div id="divProcessingMethod" runat="server" class="ACA_TabRow ACA_LiLeft">
            <ACA:AccelaDropDownList ID="ddlProcessingMethod" LabelKey="per_permitPayFee_label_checkMethod"
                OnSelectedIndexChanged="ProcessingMethodDropdown_indexChanged" AutoPostBack="true"
                runat="server" Required="true">
            </ACA:AccelaDropDownList>
        </div>
        <div class="ACA_TabRow ACA_LiLeft">
            <ACA:AccelaDropDownList ID="ddlAccountType" LabelKey="per_permitPayFee_label_accountType"
                OnSelectedIndexChanged="AccountTypeDropdown_indexChanged" AutoPostBack="true" runat="server"
                Required="true">
            </ACA:AccelaDropDownList>
        </div>
        <div id="divProvide" runat="server">
            <div class="ACA_TabRow ACA_LiLeft">
                <h1>
                    <ACA:AccelaLabel ID="per_permitPayFee_label_provideLeastOne" LabelKey="per_permitPayFee_label_provideLeastOne"
                        runat="server" /></h1>
            </div>
            <div class="ACA_TabRow ACA_LiLeft">
                <ACA:AccelaTextBox ID="txtDriverLicNbr" CssClass="ACA_NLonger" Validate="required"
                    LabelKey="per_permitPayFee_label_driverLicenseNbr" runat="server" MaxLength="33">
                </ACA:AccelaTextBox>
                <ACA:AccelaTextBox ID="txtFederalTaxNbr" CssClass="ACA_NLonger" LabelKey="per_permitPayFee_label_federalTaxIDNbr"
                    runat="server" MaxLength="33">
                </ACA:AccelaTextBox>
            </div>
        </div>
        <div class="ACA_TabRow ACA_LiLeft" style="width: 735px;">
            <ul>
                <li>
                    <ACA:AccelaNumberText ID="txtRoutingNbr" CssClass="ACA_NShot" LabelKey="per_permitPayFee_label_routingNbr"
                        Validate="required;MinLength;MaxLength" runat="server" MinLength="9" MaxLength="9"
                        IsNeedDot="false">
                    </ACA:AccelaNumberText>
                </li>
                <li>
                    <ACA:AccelaNumberText ID="txtCheckNbr" CssClass="ACA_Shot" LabelKey="per_permitPayFee_label_checkNbr"
                        Validate="required" runat="server" MaxLength="7" IsNeedDot="false">
                    </ACA:AccelaNumberText>
                </li>
                <li>
                    <ACA:AccelaNumberText ID="txtAccountNbr" CssClass="ACA_Medium" LabelKey="per_permitPayFee_label_accountNbr"
                        Validate="required" runat="server" MaxLength="17" IsNeedDot="false">
                    </ACA:AccelaNumberText>
                </li>
            </ul>
        </div>
    </div>
    <div class="ACA_Payment_Icon ACA_TabRow">
        <img alt="<%=GetTextByKey("img_alt_bankaccount_icon") %>" src="<%=FileUtil.ApplicationRoot %>Customize/images/checkPaySheet.gif" />
    </div>
    <h1>
        <ACA:AccelaLabel ID="per_permitPayFee_label_accountHolderInfo" LabelKey="per_permitPayFee_label_accountHolderInfo"
            runat="server" />
    </h1>
    <div class="ACA_TabRow ACA_LiLeft" id="dvAutoFill" runat="server">
        <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
        <tr>
            <td>
                <ACA:AccelaCheckBox ID="chkAutoFill" runat="server" AutoPostBack="false" LabelKey="aca_payment_label_autofill" />
            </td>
            <td>
                <ACA:AccelaDropDownList ID="ddlAutoFill" runat="server" AutoPostBack="false" IsHiddenLabel="true" LabelKey="aca_payment_label_autofilldropdown"
                     ToolTipLabelKey="aca_common_msg_dropdown_autofillrecord_tip" />
            </td>
        </tr>
        </table>
    </div>
    <div id="divChkInfoAccountName" runat="server" class="ACA_TabRow  ACA_LiLeft">
        <ul>
            <li>
                <ACA:AccelaTextBox ID="txtChkInfoFirstName" runat="server" Validate="required" CssClass="ACA_NShot"
                    MaxLength="70" LabelKey="per_permitPayFee_label_firstName">
                </ACA:AccelaTextBox></li>
            <li>
                <ACA:AccelaTextBox ID="txtChkInfoMiddleName" MaxLength="70" runat="server" CssClass="ACA_Shot"
                    LabelKey="per_permitPayFee_label_middleName">
                </ACA:AccelaTextBox></li>
            <li>
                <ACA:AccelaTextBox ID="txtChkInfoLastName" runat="server" Validate="required" CssClass="ACA_NShot"
                    MaxLength="70" LabelKey="per_permitPayFee_label_lastName">
                </ACA:AccelaTextBox>
            </li>
        </ul>
    </div>
    <div>
        <ACA:AccelaCountryDropDownList ID="ddlChkInfoCountry" LabelKey="per_permitPayFee_label_check_country"
            runat="server">
        </ACA:AccelaCountryDropDownList>
    </div>
    <div id="divChkInfoAccountFullName" runat="server" class="ACA_TabRow ACA_LiLeft">
        <ACA:AccelaTextBox ID="txtChkInfoAccountName" CssClass="ACA_NLonger" LabelKey="per_permitPayFee_label_chkAccountName"
            Validate="required" runat="server" MaxLength="220">
        </ACA:AccelaTextBox>
    </div>
    <div class="ACA_TabRow ACA_LiLeft">
        <ACA:AccelaTextBox ID="txtChkInfoStreetAddr" CssClass="ACA_XLong" LabelKey="per_permitPayFee_label_chkStreetAddr"
            Validate="required" runat="server" MaxLength="30">
        </ACA:AccelaTextBox>
    </div>
    <div id="divCheckInfoAddress" runat="server" class="ACA_TabRow ACA_LiLeft" visible="false">
        <ACA:AccelaTextBox ID="txtChkInfoStreetAddr2" CssClass="ACA_XLong" runat="server"
            LabelKey="per_permitPayFee_label_street2" MaxLength="40">
        </ACA:AccelaTextBox>
    </div>
    <div class="ACA_TabRow ACA_LiLeft">
        <ul>
            <li>
                <ACA:AccelaTextBox ID="txtChkInfoCity" PositionID="BankAccountPaymentCity" AutoFillType="City"
                    CssClass="ACA_NShot" LabelKey="per_permitPayFee_label_chkCity" Validate="required"
                    runat="server" MaxLength="20">
                </ACA:AccelaTextBox>
            </li>
            <li>
                <ACA:AccelaStateControl ID="ddlChkInfoState" PositionID="BankAccountPaymentState"
                    AutoFillType="State" CssClass="ACA_NShot" LabelKey="per_permitPayFee_label_check_state"
                    runat="server" Validate="required" />
            </li>
            <li>
                <ACA:AccelaZipText ID="txtChkInfoZip" CssClass="ACA_NShot" LabelKey="per_permitPayFee_label_checkZip"
                    Validate="required" runat="server" MaxLength="20">
                </ACA:AccelaZipText>
            </li>
        </ul>
    </div>
    <div class="ACA_TabRow ACA_LiLeft">
        <ACA:AccelaPhoneText ID="txtChkInfoPhoneNbr" LabelKey="per_permitPayFee_label_chkPhoneNbr"
            Validate="required" runat="server">
        </ACA:AccelaPhoneText>
    </div>
    <div class="ACA_TabRow ACA_LiLeft">
        <ACA:AccelaEmailText ID="txtChkInfoEmail" Validate="required" CssClass="ACA_NLonger"
            LabelKey="per_permitPayFee_label_chkEmail" runat="server" MaxLength="40">
        </ACA:AccelaEmailText>
    </div>
</div>
<ACA:AccelaInlineScript runat="server" ID="scriptForCheckProcessing">

    <script type="text/javascript">

        function Fill_CheckInfo(info) {
            if (typeof (myValidationErrorPanel) != 'undefined') {
                myValidationErrorPanel.clearErrors();
            }
            
            var json = eval('(' + info + ')');
            SetValueById('<%=txtChkInfoFirstName.ClientID%>', JsonDecode(json.FirstName));
            SetValueById('<%=txtChkInfoMiddleName.ClientID%>', JsonDecode(json.MiddleName));
            SetValueById('<%=txtChkInfoLastName.ClientID%>', JsonDecode(json.LastName));
            SetValueById('<%=txtChkInfoAccountName.ClientID%>', JsonDecode(json.FirstName + " " + json.MiddleName + " " + json.LastName));
            SetValueById('<%=txtChkInfoStreetAddr.ClientID%>', JsonDecode(json.Address));
            SetValueById('<%=txtChkInfoCity.ClientID%>', json.City);
            SetValueById('<%=ddlChkInfoState.ClientID%>', json.State);
            SetValueById('<%=txtChkInfoZip.ClientID%>', json.Zip);
            SetValueById('<%=txtChkInfoPhoneNbr.CountryCodeClientID%>', json.HomePhoneIDD);
            SetValueById('<%=txtChkInfoPhoneNbr.ClientID%>', json.HomePhone);
            SetValueById('<%=txtChkInfoEmail.ClientID%>', json.Email);
                        
            var countrySettingJason = '';
            var countryCtrl = $('#<% = ddlChkInfoCountry.ClientID %>');
                        
            if (countryCtrl.val() != json.Country) {
                if (countryCtrl.is(':visible')) {
                    countryCtrl.val(json.Country);
                }

                if (countryCtrl.val() != '') {
                    countrySettingJason = '[{"countryClientID":"' + '<% = ddlChkInfoCountry.ClientID %>' + '" ,"countryCode":"' + json.Country + '", "state":"' + json.State + '", "zip" : "' + json.Zip + '",';
                    countrySettingJason += '"phone":[';
                    countrySettingJason += '{"name":"txtChkInfoPhoneNbr", "value":"' + json.HomePhone + '", "iddvalue":"' + json.HomePhoneIDD + '"}';
                    countrySettingJason += ']}]';
                }

                //If auto-fill changed Country field, need send a postback to apply the regional settings.
                window.setTimeout(function () {
                    delayShowLoading();
                }, 100);
                __doPostBack('<% = ddlChkInfoCountry.UniqueID %>', '<%=ACAConstant.ACA_COUNTRY_AUTOFILL_FLAG %>' + countrySettingJason);
            }
        }

        function contact_changed() {
            myValidationErrorPanel.clearErrors();
            var ddlautofill = $('#<%=ddlAutoFill.ClientID%>');
            var selectValue = ddlautofill.val();
            if (selectValue != null) {
                var seqNumeber = selectValue.split('|')[1];
                PageMethods.GetContactModel(seqNumeber, Fill_CheckInfo);
            }
        }

        function chkAutoFill_click(obj) {
            var ddlautofill = document.getElementById('<%=ddlAutoFill.ClientID%>');
            ddlautofill.disabled = !obj.checked;

            if (obj.checked) {
                contact_changed();
            }
        }

        $(function(){
            AddValidationSectionID('<%=ClientID %>');
        });
    </script>

</ACA:AccelaInlineScript>