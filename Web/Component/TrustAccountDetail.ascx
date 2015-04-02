<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrustAccountDetail.ascx.cs"
    Inherits="Accela.ACA.Web.Component.TrustAccountDetail" %>
<table role='presentation' width="100%">
    <tr>
        <td style="width: 50%; vertical-align: top;">
            <table role='presentation'>
                <tr>
                    <td>
                        <table role='presentation'>
                            <tr>
                                <td>
                                    <p>
                                        <ACA:AccelaLabel ID="lblTrustAcctID" runat="server" LabelKey="per_trustaccountdetail_accountid"
                                            Font-Bold="true" /></p>
                                </td>
                                <td>
                                    <p>
                                        <ACA:AccelaLabel ID="lblTrustAcctIDValue" runat="server" />
                                    </p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table role='presentation'>
                            <tr>
                                <td>
                                    <p>
                                        <ACA:AccelaLabel ID="lblDescription"  runat="server" LabelKey="per_trustaccountdetail_description"
                                            Font-Bold="true" /></p>
                                </td>
                                <td>
                                    <p>
                                        <ACA:AccelaLabel ID="lblDescriptionValue" runat="server" /></p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table role='presentation'>
                            <tr>
                                <td>
                                    <p>
                                        <ACA:AccelaLabel ID="lblLedgerAccount" runat="server" LabelKey="per_trustaccountdetail_ledgeraccount"
                                            Font-Bold="true" /></p>
                                </td>
                                <td>
                                    <p>
                                        <ACA:AccelaLabel ID="lblLedgerAccountValue" runat="server" /></p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
        <td style="width: 50%; vertical-align: top;">
            <table role='presentation'>
                <tr>
                    <td>
                        <table role='presentation'>
                            <tr>
                                <td>
                                    <p>
                                        <ACA:AccelaLabel ID="lblBalance" runat="server" LabelKey="per_trustaccountdetail_balance"
                                            Font-Bold="true" /></p>
                                </td>
                                <td>
                                    <p>
                                        <ACA:AccelaLabel ID="lblBalanceValue" runat="server" IsNeedEncode="false" />
                                    </p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table role='presentation'>
                            <tr>
                                <td>
                                    <p>
                                        <ACA:AccelaLabel ID="lblStatus" runat="server" LabelKey="per_trustaccountdetail_status"
                                            Font-Bold="true" /></p>
                                </td>
                                <td>
                                    <p>
                                        <ACA:AccelaLabel ID="lblStatusValue" runat="server" IsNeedEncode="false" /></p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
