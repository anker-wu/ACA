<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.RefProviderDetail"
    CodeBehind="RefProviderDetail.ascx.cs" %>
<div class="ACA_TabRow ACA_SmLabel ACA_SmLabel_FontSize" runat="Server">
    <table role='presentation' id="tbProviderDetail" width="98%" runat="server">
        <tr>
            <td style="width: 50%; vertical-align: top;">
                <table role='presentation'>
                    <tr>
                        <td>
                            <table role='presentation'>
                                <tr>
                                    <td>
                                        <ACA:AccelaLabel ID="lblProviderName" runat="server" LabelKey="provider_detail_provider_name" Font-Bold="true"/>
                                     </td>
                                    <td class="ACA_CapListStyle">
                                        <ACA:AccelaLabel ID="lblProviderNameValue" runat="server"/>
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
                                        <ACA:AccelaLabel ID="lblProviderNumber" runat="server" LabelKey="provider_detail_provider_number" Font-Bold="true"/>
                                    </td>
                                    <td class="ACA_CapListStyle">    
                                        <ACA:AccelaLabel ID="lblProviderNumberValue" runat="server"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table role='presentation'>
                                <tr>
                                    <td>
                                        <ACA:AccelaLabel ID="lblEducationProvider" runat="server" LabelKey="provider_detail_education_provider" Font-Bold="true" />
                                    </td>
                                    <td class="ACA_CapListStyle">
                                        <ACA:AccelaLabel ID="lblEducationProviderValue" runat="server"/>
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
                                        <ACA:AccelaLabel ID="lblContEducationProvider" runat="server" LabelKey="provider_detail_continuing_education_provider" Font-Bold="true"/>
                                    </td>
                                    <td class="ACA_CapListStyle">    
                                        <ACA:AccelaLabel ID="lblContEducationProviderValue" runat="server"/>
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
                                        <ACA:AccelaLabel ID="lblExaminationProvider" runat="server" LabelKey="provider_detail_examination_provider" Font-Bold="true"/>
                                    </td>
                                    <td class="ACA_CapListStyle">      
                                        <ACA:AccelaLabel ID="lblExaminationProviderValue" runat="server"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table role='presentation'>
                                <tr>
                                    <td>
                                        <ACA:AccelaLabel ID="lblLicenseNumber" runat="server" LabelKey="provider_detail_state_license_number" Font-Bold="true"/>
                                    </td>
                                    <td class="ACA_CapListStyle">       
                                        <ACA:AccelaLabel ID="lblLicenseNumberValue" runat="server"/>
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
                                        <ACA:AccelaLabel ID="lblIssueDate" runat="server" LabelKey="provider_detail_license_issue_date" Font-Bold="true"/>
                                     </td>
                                    <td class="ACA_CapListStyle"> 
                                        <ACA:AccelaDateLabel ID="lblIssueDateValue" CSS="ACA_SmLabel ACA_SmLabel_FontSize" DateType="ShortDate"
                                        runat="server" />
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
                                        <ACA:AccelaLabel ID="lblExpirationDate" runat="server" LabelKey="provider_detail_license_expiration_date" Font-Bold="true"/>
                                    </td>
                                    <td class="ACA_CapListStyle"> 
                                        <ACA:AccelaDateLabel ID="lblExpirationDateValue" CSS="ACA_SmLabel ACA_SmLabel_FontSize" DateType="ShortDate"
                                        runat="server" />
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
                                    <td style="vertical-align: top;">
                                        <ACA:AccelaLabel ID="lblContactName" runat="server" LabelKey="provider_detail_contact_name" Font-Bold="true"/>
                                    </td>
                                    <td class="ACA_CapListStyle">
                                        <ACA:AccelaLabel ID="lblContactNameValue" runat="server" IsNeedEncode="false"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table role='presentation'>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <ACA:AccelaLabel ID="lblAddress" runat="server" LabelKey="provider_detail_Address" Font-Bold="true">
                                        </ACA:AccelaLabel>
                                    </td>
                                    <td style="vertical-align: top;" class="ACA_CapListStyle">
                                        <ACA:AccelaLabel ID="lblAddressValue" runat="server" IsNeedEncode="false">
                                        </ACA:AccelaLabel>
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
                                        <ACA:AccelaLabel ID="lblPhone1" runat="server" LabelKey="provider_detail_phone1" Font-Bold="true">
                                        </ACA:AccelaLabel>
                                    </td>
                                    <td class="ACA_CapListStyle">
                                        <ACA:AccelaLabel ID="lblPhone1Value" runat="server" IsNeedEncode="false">
                                        </ACA:AccelaLabel>
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
                                        <ACA:AccelaLabel ID="lblPhone2" runat="server" LabelKey="provider_detail_phone2" Font-Bold="true">
                                        </ACA:AccelaLabel>
                                    </td>
                                    <td class="ACA_CapListStyle">
                                        <ACA:AccelaLabel ID="lblPhone2Value" runat="server" IsNeedEncode="false">
                                        </ACA:AccelaLabel>
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
                                        <ACA:AccelaLabel ID="lblFax" runat="server" LabelKey="provider_detail_fax" Font-Bold="true">
                                        </ACA:AccelaLabel>
                                    </td>
                                    <td class="ACA_CapListStyle">
                                        <ACA:AccelaLabel ID="lblFaxValue" runat="server" IsNeedEncode="false">
                                        </ACA:AccelaLabel>
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
                                        <ACA:AccelaLabel ID="lblEmail" runat="server" LabelKey="provider_detail_email" Font-Bold="true">
                                        </ACA:AccelaLabel>
                                    </td>
                                    <td class="ACA_CapListStyle">
                                        <ACA:AccelaLabel ID="lblEmailValue" runat="server"></ACA:AccelaLabel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
