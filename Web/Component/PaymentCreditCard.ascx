<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="PaymentCreditCard.ascx.cs"
    Inherits="Accela.ACA.Web.Component.PaymentCreditCard" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<div>
    <div class="ACA_TabRow ACA_LiLeft">
        <h1>
            <ACA:AccelaLabel ID="per_permitPayFee_label_CreditCardInfo" LabelKey="per_permitPayFee_label_CreditCardInfo"
                runat="server" /></h1>
    </div>
    <div id="divCreditCardInfo">
        <div class="ACA_TabRow ACA_LiLeft">
            <ul>
                <li>
                    <ACA:AccelaDropDownList ID="ddlCardType" LabelKey="per_permitPayFee_label_cardType"
                        runat="server" Required="true" />
                </li>
                <li>
                    <ACA:AccelaTextBox ID="txtCardNumber" CssClass="ACA_NLonger" runat="server" Validate="required"
                        LabelKey="per_permitPayFee_label_cardNum" MaxLength="20">
                    </ACA:AccelaTextBox></li>
                <li>
                    <ACA:AccelaNumberText ID="txtCCV" CssClass="field_short" runat="server" Validate="required"
                        LabelKey="per_permitPayFee_label_ccv" IsNeedDot="false" MaxLength="4">
                    </ACA:AccelaNumberText></li>
            </ul>
        </div>
        <div class="ACA_TabRow ACA_LiLeft">
            <ul>
                <li id="liCardName" runat="server">
                    <ACA:AccelaTextBox ID="txtCardName" CssClass="ACA_NLonger" runat="server" Validate="required"
                        LabelKey="per_permitPayFee_label_cardName" MaxLength="80" />
                </li>
                <li><span class="ACA_Required_Indicator">*</span>
                    <ACA:AccelaLabel ID="per_permitPayFee_label_expAndDate" LabelKey="per_permitPayFee_label_expAndDate"
                        runat="server" CssClass="ACA_Label ACA_Label_FontSize" />
                    <div style="margin-top: 2.5px;">
                        <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
                            <tr>
                                <td>
                                    <ACA:AccelaDropDownList runat="server" ID="ddlExpMonth">
                                    </ACA:AccelaDropDownList>
                                </td>
                                <td>
                                    <ACA:AccelaDropDownList runat="server" ID="ddlExpYear">
                                    </ACA:AccelaDropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div class="ACA_TabRow ACA_LiLeft">
        <h1>
            <ACA:AccelaLabel ID="per_permitPayFee_label_ccDefaultContactInfo" LabelKey="per_permitPayFee_label_ccDefaultContactInfo"
                runat="server" /></h1>
    </div>
    <div class="ACA_TabRow ACA_LiLeft" id="dvAutoFill" runat="server">
       <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
            <tr>
                <td>
                    <ACA:AccelaCheckBox ID="chkAutoFill" runat="server" AutoPostBack="false" LabelKey="aca_creditcard_label_autofill" />
                </td>
                <td>
                    <ACA:AccelaDropDownList ID="ddlAutoFill" runat="server" AutoPostBack="false" IsHiddenLabel="true" LabelKey="aca_creditcard_label_autofilldropdown"
                         ToolTipLabelKey="aca_common_msg_dropdown_autofillrecord_tip" />
                </td>
            </tr>
         </table>
    </div>
    <div id="divAccountName" runat="server" class="ACA_TabRow  ACA_LiLeft">
        <ul>
            <li>
                <ACA:AccelaTextBox ID="txtFirstName" runat="server" Validate="required" CssClass="ACA_NShot"
                    MaxLength="70" LabelKey="per_permitPayFee_label_firstName">
                </ACA:AccelaTextBox></li>
            <li>
                <ACA:AccelaTextBox ID="txtMiddleName" MaxLength="70" runat="server" CssClass="ACA_Shot"
                    LabelKey="per_permitPayFee_label_middleName"></ACA:AccelaTextBox></li>
            <li>
                <ACA:AccelaTextBox ID="txtLastName" runat="server" Validate="required" CssClass="ACA_NShot"
                    MaxLength="70" LabelKey="per_permitPayFee_label_lastName">
                </ACA:AccelaTextBox>
            </li>
        </ul>
    </div>
    <div class="ACA_TabRow ACA_LiLeft">
        <ACA:AccelaCountryDropDownList ID="ddlCCCountry" LabelKey="per_permitPayFee_label_creditCard_country"
            runat="server">
        </ACA:AccelaCountryDropDownList>
    </div>
    <div class="ACA_TabRow ACA_LiLeft">
        <ACA:AccelaTextBox ID="txtCCStreetAdd1" CssClass="ACA_XLong" runat="server" Validate="required"
            LabelKey="per_permitPayFee_label_street1" MaxLength="40">
        </ACA:AccelaTextBox>
    </div>
    <div id="divAddress2" runat="server" class="ACA_TabRow ACA_LiLeft" visible="false">
        <ACA:AccelaTextBox ID="txtCCStreetAdd2" CssClass="ACA_XLong" runat="server" LabelKey="per_permitPayFee_label_street2"
            MaxLength="40">
        </ACA:AccelaTextBox>
    </div>
    <div class="ACA_TabRow ACA_LiLeft">
        <ul>
            <li>
                <ACA:AccelaTextBox ID="txtCCCity" PositionID="CreditCardPaymentCity" AutoFillType="City"
                    CssClass="ACA_Medium" runat="server" Validate="required" LabelKey="per_permitPayFee_label_city"
                    MaxLength="30"></ACA:AccelaTextBox></li>
            <li>
                <ACA:AccelaStateControl ID="ddlCCState" PositionID="CreditCardPaymentState" AutoFillType="State"
                    LabelKey="per_permitPayFee_label_creditCard_state" runat="server" Validate="required" />
            </li>
            <li>
                <ACA:AccelaZipText ID="txtCCZip" CssClass="ACA_Medium" runat="server" Validate="required;zip"
                    LabelKey="per_permitPayFee_label_zip" MaxLength="10" /></li>
        </ul>
    </div>
    <div class="ACA_TabRow ACA_LiLeft">
        <ACA:AccelaPhoneText ID="txtCCPhone" LabelKey="per_permitPayFee_label_chkPhoneNbr"
            Validate="required" runat="server"></ACA:AccelaPhoneText>
    </div>
    <div class="ACA_TabRow ACA_LiLeft">
        <ACA:AccelaEmailText ID="txtCCEmail" CssClass="ACA_NLonger" LabelKey="per_permitPayFee_label_chkEmail"
            runat="server" MaxLength="40"></ACA:AccelaEmailText>
    </div>
</div>
<ACA:AccelaInlineScript runat="server" ID="scriptForCreditCard">
    <script type="text/javascript">
        function creditcard_contactchanged() {
            myValidationErrorPanel.clearErrors();
            var ddlautofill = $('#<%=ddlAutoFill.ClientID%>');
            var selectValue = ddlautofill.val();
            if (selectValue != null) {
                var seqNumeber = selectValue.split('|')[1];
                PageMethods.GetContactModel(seqNumeber, Fill_CreditCardInfo);
            }
        }

        function creditcard_chkAutoFillclick(obj) {
            var ddlautofill = document.getElementById('<%=ddlAutoFill.ClientID%>');
            ddlautofill.disabled = !obj.checked;

            if (obj.checked) {
                creditcard_contactchanged();
            }
        }

        function Fill_CreditCardInfo(info) {
            if (typeof (myValidationErrorPanel) != 'undefined') {
                myValidationErrorPanel.clearErrors();
            }
            var json = eval('(' + info + ')');
            SetValueById('<%=txtFirstName.ClientID%>', JsonDecode(json.FirstName));
            SetValueById('<%=txtMiddleName.ClientID%>', JsonDecode(json.MiddleName));
            SetValueById('<%=txtLastName.ClientID%>', JsonDecode(json.LastName));
            SetValueById('<%=txtCCStreetAdd1.ClientID%>', JsonDecode(json.Address));
            SetValueById('<%=txtCCCity.ClientID%>', json.City);
            SetValueById('<%=ddlCCState.ClientID%>', json.State);
            SetValueById('<%=txtCCZip.ClientID%>', json.Zip);
            SetValueById('<%=txtCCPhone.CountryCodeClientID%>', json.HomePhoneIDD);
            SetValueById('<%=txtCCPhone.ClientID%>', json.HomePhone);
            SetValueById('<%=txtCCEmail.ClientID%>', json.Email);
                    
            var countrySettingJason = '';
            var countryCtrl = $('#<% = ddlCCCountry.ClientID %>');
                    
            if (countryCtrl.val() != json.Country) {
                if (countryCtrl.is(':visible')) {
                    countryCtrl.val(json.Country);
                }
             
                countrySettingJason = '[{"countryClientID":"' + '<% = ddlCCCountry.ClientID %>' + '" ,"countryCode":"' + json.Country + '", "state":"' + json.State + '", "zip" : "' + json.Zip + '",';
                countrySettingJason += '"phone":[';
                countrySettingJason += '{"name":"txtCCPhone", "value":"' + json.HomePhone + '", "iddvalue":"' + json.HomePhoneIDD + '"}';
                countrySettingJason += ']}]';               
                
                //If auto-fill changed Country field, need send a postback to apply the regional settings.
                window.setTimeout(function () {
                    delayShowLoading();
                }, 100);
                __doPostBack('<% = ddlCCCountry.UniqueID %>', '<%=ACAConstant.ACA_COUNTRY_AUTOFILL_FLAG %>' + countrySettingJason);
            }
        }

        $(function(){
            AddValidationSectionID('<%=ClientID %>');
        });
    </script>

</ACA:AccelaInlineScript>