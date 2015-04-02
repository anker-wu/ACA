<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.AccountContactView" Codebehind="AccountContactView.ascx.cs" %>
<%@ Register Src="ContactAddressList.ascx" TagName="ContactAddressList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/AccountView.ascx" TagName="AccountView" TagPrefix="ACA" %>

<div>
    <div id="divAccountInfo" runat="server">
        <ACA:AccountView ID="accountView" runat="server" />
    </div>
    <ACA:AccelaLabel ID="lblContactInfo" LabelKey="aca_reg_contact_info_label" runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
    <div id="divContactInfo">
        <div class="ACA_FLeft ACA_Page ACA_Page_FontSize account_contactview">
            <table role='presentation'>
                <tr>
                    <td colspan="2" style="height: 21px">
                        <ACA:AccelaLabel ID="lblName" CssClass="contactinfo_fullname" runat="server"></ACA:AccelaLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <ACA:AccelaLabel CssClass="contactinfo_title" runat="server" ID="lblTitle"></ACA:AccelaLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <ACA:AccelaLabel CssClass="contactinfo_businessname" ID="lblBusinessName" runat="server"></ACA:AccelaLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <ACA:AccelaLabel CssClass="contactinfo_businessname2" ID="lblBusinessName2" runat="server"></ACA:AccelaLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <ACA:AccelaLabel CssClass="contactinfo_addressline1" ID="lblConAdd1" runat="server"></ACA:AccelaLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <ACA:AccelaLabel CssClass="contactinfo_email" ID="lblConEmail" runat="server"></ACA:AccelaLabel>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ACA_FLeft ACA_Page ACA_Page_FontSize ACA_ConfigInfo">
            <table role='presentation' class="contact">
                <tr id="lblConTelHomeTr" runat="server">
                    <td style="height: 21px"><table role='presentation' border="0" cellpadding="0" cellspacing="0"><tr><td>
                        <ACA:AccelaLabel ID="lblConTelHomeTitle" LabelKey="acc_contactView_label_homePhone"
                            runat="server"/></td><td class="ACA_CapListStyle">
                        <ACA:AccelaLabel CssClass="contactinfo_homephone" ID="lblConTelHome" IsNeedEncode="false" runat="server"/></td></tr></table></td>
                </tr>
                <tr id="lblConTelWorkTr" runat="server">
                    <td><table role='presentation' border="0" cellpadding="0" cellspacing="0"><tr><td>
                        <ACA:AccelaLabel ID="lblConTelWorkTitle" LabelKey="acc_contactView_label_workPhone"
                            runat="server"/></td><td class="ACA_CapListStyle">
                        <ACA:AccelaLabel CssClass="contactinfo_workphone" ID="lblConTelWork" IsNeedEncode="false" runat="server"/></td></tr></table></td>
                </tr>
                <tr id="lblConMobileTr" runat="server">
                    <td style="height: 21px"><table role='presentation' border="0" cellpadding="0" cellspacing="0"><tr><td>
                        <ACA:AccelaLabel ID="lblConMobileTitle" LabelKey="acc_contactView_label_mobile"
                            runat="server"/></td><td class="ACA_CapListStyle">
                        <ACA:AccelaLabel CssClass="contactinfo_mobile" ID="lblConMobile" IsNeedEncode="false" runat="server"/></td></tr></table></td>
                </tr>
                <tr id="lblConFaxTr" runat="server">
                    <td><table role='presentation' border="0" cellpadding="0" cellspacing="0"><tr><td>
                        <ACA:AccelaLabel ID="lblConFaxTitle" LabelKey="acc_contactView_label_fax"
                            runat="server"/></td><td class="ACA_CapListStyle">
                        <ACA:AccelaLabel CssClass="contactinfo_fax" ID="lblConFax" IsNeedEncode="false" runat="server"/></td></tr></table>
                    </td>
                </tr>
                <tr id="trConPreferredChannel" runat="server">
                    <td><table role='presentation' border="0" cellpadding="0" cellspacing="0"><tr><td>
                        <ACA:AccelaLabel ID="lblPreContactTitle" LabelKey="acc_contactForm_label_contactMethod"
                                        runat="server"></ACA:AccelaLabel></td><td class="ACA_CapListStyle">
                        <ACA:AccelaLabel CssClass="contactinfo_preferredchannel" ID="lblPreContact" IsNeedEncode="false" runat="server"></ACA:AccelaLabel></td></tr></table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <ACA:AccelaHeightSeparate ID="sepHeightForContact" runat="server" Height="5" />
    <div id="divContactAddressList" runat="server">
        <div class="ACA_Row">
            <ACA:AccelaLabel ID="lblContactAddressListTitle" runat="server" LabelKey="aca_contactaddress_label_title_listview" LabelType="SubSectionText" CssClass="ACA_TabRow_Italic"/>
        </div>
        <div>
            <ACA:ContactAddressList ID="contactAddressList" IsForView="true" runat="server"/>
        </div>
    </div>
</div>