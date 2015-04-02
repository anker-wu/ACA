<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaymentTrustAccount.ascx.cs"
    Inherits="Accela.ACA.Web.Component.PaymentTrustAccount" %>
<%@ Import Namespace="Accela.ACA.Web.Payment" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<!-- Trust Account Form begin -->
<div>
    <div class="ACA_TabRow fontbold font11px">
        <ACA:AccelaRadioButtonList ID="rdlAssociatedType" AutoPostBack="true" Validate="required;"
            ListType="TrustAccountAssociatedType" Required="true" LabelKey="aca_payment_trustaccount_associatedtype"
            OnSelectedIndexChanged="AssociatedTypeDropdown_IndexChanged" runat="server">
        </ACA:AccelaRadioButtonList>
    </div>
    <div runat="server" id="divRowLine" class="ACA_TabRow ACA_Line_Content">
    </div>
    <div id="divTrustAccountDetail" runat="server" visible="false">
        <div class="ACA_TabRow ACA_Label ACA_Label_FontSize_Restore ACA_LiLeft">
            <ul>
                <li>
                    <ACA:AccelaDropDownList ID="ddlAssociatedType" LabelKey="aca_payment_trustaccount_associatedtype_licenses"
                        onchange="HideErrMsg();" AutoPostBack="true" Required="true"
                        runat="server" OnSelectedIndexChanged="AssociatedTypeDropdown_SelectedIndexChanged" ToolTipLabelKey="aca_common_msg_dropdown_updateassociatedvalue_tip">
                    </ACA:AccelaDropDownList>
                    <ACA:AccelaDropDownList ID="ddlRecord" LabelKey="aca_payment_trustaccount_associatedtype_record"
                        Required="true" runat="server" Visible="false" ToolTipLabelKey="aca_common_msg_dropdown_updateassociatedvalue_tip">
                    </ACA:AccelaDropDownList>
                    <ACA:AccelaDropDownList ID="ddlContacts" LabelKey="aca_payment_trustaccount_associatedtype_contacts"
                        Required="true" runat="server" Visible="false" ToolTipLabelKey="aca_common_msg_dropdown_updateassociatedvalue_tip">
                    </ACA:AccelaDropDownList>
                    <ACA:AccelaDropDownList ID="ddlAddresses" LabelKey="aca_payment_trustaccount_associatedtype_addresses"
                        Required="true" runat="server" Visible="false" ToolTipLabelKey="aca_common_msg_dropdown_updateassociatedvalue_tip">
                    </ACA:AccelaDropDownList>
                    <ACA:AccelaDropDownList ID="ddlParcels" LabelKey="aca_payment_trustaccount_associatedtype_parcels"
                        Required="true" runat="server" Visible="false" ToolTipLabelKey="aca_common_msg_dropdown_updateassociatedvalue_tip">
                    </ACA:AccelaDropDownList>
                </li>
                <li>
                    <ACA:AccelaDropDownList ID="ddlTrustAccount" LabelKey="per_permitPayFee_label_trustAccount"
                        onchange="javascript:trustAccount_onchange();" runat="server" Required="true">
                    </ACA:AccelaDropDownList>
                </li>
            </ul>
        </div>
    </div>
    <div class="ACA_TabRow ACA_Label ACA_Label_FontSize_Restore ACA_LiLeft">
            <ul>
                <li>
                    <div runat="server" ID="divTrustAccountInfo" class="table_text font12px"></div>
                </li>
            </ul>
        </div>
    <asp:HiddenField ID="hdnAcctSeq" runat="server" Value="0" />
    <ACA:AccelaInlineScript runat="server" ID="scriptForTrustAccount">

        <script type="text/javascript">
            function trustAccount_onchange() {
                HideErrMsg();
                var ddlTrustAccount = document.getElementById('<%=ddlTrustAccount.ClientID %>');


                if (ddlTrustAccount == "undefined" || ddlTrustAccount == null)
                    return;

                var value = ddlTrustAccount.value;

                if (value == '') {
                    var divTrustAccountInfo = GetDivForTrustAccount("<%=divTrustAccountInfo.ClientID %>");
                    SetTrustAccountControlValue('<%=hdnAcctSeq.ClientID %>', '0');
                    divTrustAccountInfo.style.display = 'none';
                }
                else {
                    PageMethods.GetTrustAccountInfo(value, ShowInfo, ShowErrMsg);
                }
            }

            function SetTrustAccountControlValue(controlId, value) {
                var control = document.getElementById(controlId);
                var currentVal = GetValue(control);
                if (currentVal != value) {
                    SetValue(control, value);
                }
            }

            function GetDivForTrustAccount(id) {
                var div = document.getElementById(id)
                    || document.getElementById('ctl00_PlaceHolderMain_Payment_' + id)
                    || document.getElementById('ct100_PaceHolderMain_Payment_TrustAccount_' + id);

                return div;
            }

            function ShowInfo(result) {
                var values = eval('(' + result + ')');
                $.global.RealBalanceValue = values.RealBalance;
                if (values.Error != '') {
                    showNormalMessage(values.Error, 'Error');
                }
                else {
                    var divTrustAccountInfo = GetDivForTrustAccount("<%=divTrustAccountInfo.ClientID %>");
                    var innerHtml =
                        '<br/>' +
                            '<table role="presentation"><tr>' +
                            '<td><%= GetTextByKey("ACA_CapPayment_NameOnTrustAccount").Replace("'","\\'") %></td>' +
                            '<td>' + values.Name + '</td></tr><tr>' +
                            '<td><%=GetTextByKey("ACA_CapPayment_AmountAvailable").Replace("'","\\'") %></td>' +
                            '<td>' + values.Balance + '</td></tr></table>';

                    divTrustAccountInfo.style.display = 'block';
                    divTrustAccountInfo.innerHTML = innerHtml;
                    SetTrustAccountControlValue('<%=hdnAcctSeq.ClientID %>', values.Balance);
                }
            }

            function ShowErrMsg(e) {
                showNormalMessage('<%=GetTextByKey("per_getTrustAccount_error").Replace("'","\\'")%>', 'Error');
                isTrustAccountPassed = false;
            }

            function HideErrMsg() {
                hideMessage();
                isTrustAccountPassed = true;
            }

            function ChangeAssociatedType() {
                if ($.global.isAdmin) {
                    var prefix = "#ctl00_PlaceHolderMain_Payment_TrustAccount_";

                    var rdoAssociatedTypes = document.getElementById('<%=rdlAssociatedType.ClientID%>');

                    var strTrustAccountDetail = '<%=divTrustAccountDetail.ClientID%>';

                    $(prefix + "ddlAssociatedType" + "_table").hide();
                    $(prefix + "ddlContacts" + "_table").hide();
                    $(prefix + "ddlAddresses" + "_table").hide();
                    $(prefix + "ddlParcels" + "_table").hide();
                    $(prefix + "ddlRecord" + "_table").hide();
                    $("#" + strTrustAccountDetail).hide();

                    var selectedAssociatedType = GetRadioButtonSelectedValue(rdoAssociatedTypes);

                    if (selectedAssociatedType == '') {
                        return;
                    }

                    $("#" + strTrustAccountDetail).show();

                    switch (selectedAssociatedType) {
                        case '<%= ACAConstant.PaymentAssociatedType.Licenses.ToString()%>':
                            $(prefix + "ddlAssociatedType" + "_table").show();
                            break;
                        case '<%= ACAConstant.PaymentAssociatedType.Contacts.ToString()%>':
                            $(prefix + "ddlContacts" + "_table").show();
                            break;
                        case '<%= ACAConstant.PaymentAssociatedType.Addresses.ToString()%>':
                            $(prefix + "ddlAddresses" + "_table").show();
                            break;
                        case '<%= ACAConstant.PaymentAssociatedType.Parcels.ToString()%>':
                            $(prefix + "ddlParcels" + "_table").show();
                            break;
                        case '<%=ACAConstant.PaymentAssociatedType.Record.ToString()%>':
                            $(prefix + "ddlRecord" + "_table").show();
                            break;
                    }
                }
            }
        </script>

    </ACA:AccelaInlineScript>
</div>
<!-- Trust Account Form end -->
