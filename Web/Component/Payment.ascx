<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="Payment.ascx.cs" Inherits="Accela.ACA.Web.Component.Payment" %>
<%@ Register Src="../Component/PaymentCreditCard.ascx" TagName="PaymentCreditCard"
    TagPrefix="uc1" %>
<%@ Register Src="../Component/PaymentTrustAccount.ascx" TagName="PaymentTrustAccount"
    TagPrefix="uc1" %>
<%@ Register Src="../Component/PaymentCheckProcessing.ascx" TagName="PaymentCheckProcessing"
    TagPrefix="uc1" %>
<%@ Import Namespace="Accela.ACA.Web.Payment" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<asp:UpdatePanel ID="updatePanel" runat="server">
    <ContentTemplate>
        <div id="divAmount">
            <table role='presentation' style="border: 0px; width: 98%" id="tbTrustAccountAmount"
                runat="Server">
                <tr>
                    <td style="width: 50%" class="ACA_Label font12px">
                        <ACA:AccelaLabel ID="lblTrustAccount" LabelKey="aca_trustaccountdeposit_label_accountid"
                            runat="server" />
                        <br />
                        <ACA:AccelaLabel ID="lblTrustAccountValue" runat="server" />
                    </td>
                    <td>
                        <ACA:AccelaNumberText ID="txtDepositAmount" runat="server" LabelKey="aca_trustaccountdeposit_label_depositamount"
                            CssClass="ACA_NLonger" MaxLength="16" Validate="required;MaxLength;DecimalDigitsLength"
                            DecimalDigitsLength="2" OnTextChanged="DepositAmount_TextChanged" AutoPostBack="true"></ACA:AccelaNumberText>
                    </td>
                </tr>
            </table>
            <table role='presentation' style="border: 0px;">
                <tr>
                    <td>
                        <div class="ACA_Label ACA_Label_FontSize" id="divPermitAmount" runat="Server">
                            <ACA:AccelaLabel ID="per_permitPayFee_label_amount" LabelKey="per_permitPayFee_label_amount"
                                runat="server" />
                            <ACA:AccelaLabel ID="lblChargedAmount" runat="server" />
                            <asp:HiddenField ID="hdnChargedAmount" runat="server" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divConveninenceFee" visible = "false" runat="server" class="ACA_Label ACA_Label_FontSize">
                            <ACA:AccelaLabel ID="lblConveninenceFeeLabel" LabelKey="per_permitPayFee_label_convfee" runat="server"></ACA:AccelaLabel>
                            <ACA:AccelaLabel ID="lblConveninenceFee" runat="server"></ACA:AccelaLabel>
                            <br />
                            <ACA:AccelaLabel ID="lblTotalAmountLabel" LabelKey="per_permitPayFee_label_totalamount" runat="server"></ACA:AccelaLabel>
                            <ACA:AccelaLabel ID="lblTotalAmount" runat="server"></ACA:AccelaLabel>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div style="display: none">
            <%-- Temporarily use this control to register a javascript source code --%>
            <ACA:AccelaCalendarText ID="txtHiddenDate" runat="server" title="This is a hidden field." />
        </div>
        <!-- Payment Radio button list begin -->
        <div class="fontbold font11px">
            <ACA:AccelaRadioButtonList ID="rdlPaymentMethod" ListType="PaymentMethod" AutoPostBack="true" role='presentation'
                OnSelectedIndexChanged="PaymentMethodDropdown_IndexChanged" runat="server">
            </ACA:AccelaRadioButtonList>
        </div>
        <!-- Payment Radio button list end -->
        <div runat="server" id="divRowLine" class="ACA_TabRow ACA_Line_Content">
        </div>
        <!-- Credit Card Form begin -->
        <div id="divCreditCard" runat="server" visible="false">
            <uc1:PaymentCreditCard ID="CreditCard" runat="Server" ></uc1:PaymentCreditCard>
        </div>
        <!-- Credit Card Form end -->
        <!-- Trust Account Form begin -->
        <div id="divTrustAccount" runat="server" visible="false">
            <uc1:PaymentTrustAccount ID="TrustAccount" runat="Server"></uc1:PaymentTrustAccount>
        </div>
        <!-- Trust Account Form end -->
        <!-- Check Processing Form begin-->
        <div id="divCheckProcessing" runat="server" visible="false">
            <uc1:PaymentCheckProcessing ID="CheckProcessing" runat="Server"></uc1:PaymentCheckProcessing>
        </div>
        <!-- Check Processing Form end-->
    </ContentTemplate>
</asp:UpdatePanel>
<div id="divEnd" class="ACA_TabRow">
    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" runat="server" Height="30" />
    <table role='presentation' style="width: 100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <ACA:AccelaButton ID="lnkSubmitPaymentPage" LabelKey="per_permitPayFee_label_sumbitPayment"
                    OnClientClick="return SubmitPage();" OnClick="SubmitPaymentPageLink_Click" 
                    runat="server" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize ACA_Button_Text"
                    DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" />
                <div>
                    <span class="ACA_SmLabel ACA_SmLabel_FontSize">
                        <ACA:AccelaLabel ID="per_permitPayFee_sublabel_sumbitPayment" LabelKey="per_permitPayFee_sublabel_sumbitPayment"
                            runat="server" />
                    </span>
                </div>
            </td>
        </tr>
    </table>
</div>

<script type="text/javascript">
    //ctl00_PlaceHolderMain_Payment_divCheckProcessing
    var prefix = "#ctl00_PlaceHolderMain_Payment_";
    //Indicates whether the Trust Account is verify passed.
    var isTrustAccountPassed = false;

    ChangeType();
    
    // Disable validation for Amount field after change the field value and focus moved out.
    $(function(){
        AddValidationSectionID('<%=txtDepositAmount.ClientID %>');
    });

    function HideAllSections() {
        $(prefix + "divCheckProcessing").hide();
        $(prefix + "divCreditCard").hide();
        $(prefix + "divTrustAccount").hide();
        $(prefix + "txtCheckNbr" + "_table").hide();
        $(prefix + "txtFederalTaxNbr" + "_table").hide();
        $(prefix + "txtDriverLicNbr" + "_table").hide();
    }

    function ChangeType() {
        if ($.global.isAdmin) {
            var rdl = $get('<%=rdlPaymentMethod.ClientID %>');
            HideAllSections();
            var divId;
            var selectedSection = GetRadioButtonSelectedValue(rdl).split('||')[0].replace("-", "");

            switch (selectedSection) {
                case '2':
                    divId = "divCheckProcessing";
                    break;
                case '0':
                    divId = "divCreditCard";
                    break;
                case '1':
                    divId = "divTrustAccount";
                    break;
            }
            $(prefix + divId).show();
            ChangeProcessingMethod($get('ctl00_PlaceHolderMain_Payment_CheckProcessing_ddlProcessingMethod'));
            ChangeAccountType($get('ctl00_PlaceHolderMain_Payment_CheckProcessing_ddlAccountType'));
        }
    }

    function ChangeProcessingMethod(ddl) {
        if (ddl == null || ddl.value == "" || ddl.value.split('||').length < 1) {
            return;
        }

        var checkProcessing = "CheckProcessing_";

        switch (ddl.value.split('||')[0]) {
            case 'Account Debit':
                $(prefix + checkProcessing + "txtCheckNbr" + "_table").hide();
                break;
            case 'Electronic Check':
                $(prefix + checkProcessing + "txtCheckNbr" + "_table").show();
                break;
            default:
                $(prefix + checkProcessing + "txtCheckNbr" + "_table").show();
                break;
        }
    }

    function ChangeAccountType(ddl) {
        if (ddl == null || ddl.value == "" || ddl.value.split('||').length < 1) {
            return;
        }
        var checkProcessing = "CheckProcessing_";

        switch (ddl.value.split('||')[0]) {
            case 'Personal':
                $(prefix + checkProcessing + "txtDriverLicNbr" + "_table").show();
                $(prefix + checkProcessing + "txtFederalTaxNbr" + "_table").hide();
                break;
            case 'Business':
                $(prefix + checkProcessing + "txtDriverLicNbr" + "_table").hide();
                $(prefix + checkProcessing + "txtFederalTaxNbr" + "_table").show();
                break;
            default:
                $(prefix + checkProcessing + "txtDriverLicNbr" + "_table").show();
                $(prefix + checkProcessing + "txtFederalTaxNbr" + "_table").show();
                break;
        }
    }

    function CheckTrustAccount() {
        if (isTrustAccountPayment()) {
            var ddlTrustAccount = $get('<%=TrustAccount.ClientID + this.ClientIDSeparator %>ddlTrustAccount');

            if (ddlTrustAccount != null && typeof(ddlTrustAccount) != 'undefined' && ddlTrustAccount.value != '' && !isTrustAccountPassed) {
                return false;
            }
        }

        return true;
    }

    function SubmitPage() {
        SetNotAsk();

        //To validate whole payment section.
        SetCurrentValidationSectionID('<%=ClientID %>');
        return CheckTrustAccount();
    }

    function isTrustAccountPayment() {
        var isUseTrustAccount = false;
        var paymentMethods = "<%= string.Join(",", Enum.GetNames(typeof(PaymentMethod)))%>";

        var paymentMethodArray = paymentMethods.split(',');

        if (paymentMethodArray != null) {
            for (var i = 0; i < paymentMethodArray.length; i++) {
                var radioElement = document.getElementById('<%=rdlPaymentMethod.ClientID %>' + '_' + i);

                if (radioElement != null && radioElement.checked) {
                    var value = radioElement.value;

                    if (value == "<%=(int)PaymentMethod.TrustAccount%>") {
                        isUseTrustAccount = true;
                    }

                    break;
                }
            }
        }

        return isUseTrustAccount;
    }
</script>

