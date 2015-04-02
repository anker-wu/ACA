<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.LicenseView" Codebehind="LicenseView.ascx.cs" %>
<%@ Register Src="TemplateView.ascx" TagName="TemplateView" TagPrefix="uc1" %>
<div id="divLicenseViewPattern" visible="false" runat="server" class="ACA_Page">
    <span>Click the area below to edit license available variables according to format:</span><br />
    <span>$$Salutation$$: The license salutation</span><br />
    <span>$$FirstName$$: The license first name</span><br />
    <span>$$MiddleName$$: The license middle name</span><br />
    <span>$$LastName$$: The license last name</span><br />
    <span>$$Suffix$$: The license suffix</span><br />
    <span>$$ChSalutation$$: The license chinese salutation</span><br />
    <span>$$BirthDate$$: The license birth date</span><br />
    <span>$$Gender$$: The license gender</span><br />
    <span>$$BusinessName$$: The license business name</span><br />
    <span>$$BusinessName2$$: The license business name2</span><br />
    <span>$$BusinessLicense$$: The license business license</span><br />
    <span>$$AddressLine1$$: The license address line1</span><br />
    <span>$$AddressLine2$$: The license address line2</span><br />
    <span>$$AddressLine3$$: The license address line3</span><br />
    <span>$$City$$: The license city</span><br />
    <span>$$State$$: The license state</span><br />
    <span>$$Zip$$: The license zip</span><br />
    <span>$$Country$$: The license country</span><br />
    <span>$$PostOfficeBox$$: The license post office box</span><br />
    <span>$$HomePhone$$: The license home phone</span><br />
    <span>$$MobilePhone$$: The license mobile phone</span><br />
    <span>$$Fax$$: The license fax</span><br />
    <span>$$ClassCode$$: The license class code</span><br />
    <span>$$LicState$$: The license state</span><br />
    <span>$$LicenseType$$: The license type</span><br />
    <span>$$ClassCode2$$: The license class code2</span><br />
    <span>$$LicState2$$: The license state2</span><br />
    <span>$$LicenseNumber$$: The license number</span><br />
    <span>$$ContactType$$: The license contact type</span><br />
    <span>$$SocialSecurityNumber$$: The license SSN</span><br />
    <span>$$Fein$$: The license FEIN</span><br />
    <span>$$ContractorLicNo$$: The license contractor license number</span><br />
    <span>$$ContractorBusName$$: The license contractor business name</span><br />
    <span>$$Email$$: The license email</span><br/><br/>
    <ACA:AccelaLabel ID="lblLicenseViewPattern" LabelKey="aca_licenseview_label_licensepattern" LabelType="BodyText" runat="server" />
</div>
<div id="divSpearFormLicenseView" visible="false" runat="server" class="ACA_SmLabel ACA_SmLabel_FontSize ACA_Row">
    <ACA:AccelaLabel ID="lblLicenseViewInfo" runat="server" IsNeedEncode="false" />
</div>
<div id="divConfirmLicenseView" visible="false" runat="server" class="ACA_SmLabel ACA_SmLabel_FontSize">
    <div class="ACA_FLeft" runat="server">    
        <table role='presentation' id="tbLicenseBasicDetail" class="Confirm_table" border='0' cellpadding='0' cellspacing='0' runat="server">
            <tr><td>
                    <strong>
                        <ACA:AccelaLabel ID="IblSalutation" IsNeedEncode="false" runat="server" />
                        <ACA:AccelaLabel ID="IblFirstName" IsNeedEncode="false" runat="server" />
                        <ACA:AccelaLabel ID="IblMiddleName" IsNeedEncode="false" runat="server" />
                        <ACA:AccelaLabel ID="IblLastName" IsNeedEncode="false" runat="server" />
                        <ACA:AccelaLabel ID="lblSuffix" IsNeedEncode="false" runat="server" />
                        <ACA:AccelaLabel ID="IblChSalutation" IsNeedEncode="false" runat="server" />
                    </strong>
            </td></tr>
            <tr><td>
                <ACA:AccelaLabel ID="IblBirthDate" IsNeedEncode="false" runat="server" />
            </td></tr>
            <tr><td>
                <ACA:AccelaLabel ID="IblGender" IsNeedEncode="false" runat="server" />
            </td></tr>
            <tr><td>
                <ACA:AccelaLabel ID="IblNameBusiness" IsNeedEncode="false" runat="server" />
                <ACA:AccelaLabel ID="IblNameBusiness2" IsNeedEncode="false" runat="server" />
            </td></tr>
            <tr><td>
                <ACA:AccelaLabel ID="lblBusinessLicense" IsNeedEncode="false" runat="server" />
            </td></tr>
            <tr><td>
                <ACA:AccelaLabel ID="IblAddress1" IsNeedEncode="false" runat="server" />
            </td></tr>
            <tr><td>
                <ACA:AccelaLabel ID="IblAddress2" IsNeedEncode="false" runat="server" />
            </td></tr>
            <tr><td>
                <ACA:AccelaLabel ID="IblAddress3" IsNeedEncode="false" runat="server" />
            </td></tr>
            <tr><td>
                <ACA:AccelaLabel ID="IblCity" IsNeedEncode="false" runat="server" />
                <ACA:AccelaLabel ID="IblState" IsNeedEncode="false" runat="server" />
                <ACA:AccelaLabel ID="IblZip" IsNeedEncode="false" runat="server" />
            </td></tr>
            <tr><td>
                <ACA:AccelaLabel ID="IblCountry" IsNeedEncode="false" runat="server" />
            </td></tr>
            <tr><td>
                <ACA:AccelaLabel ID="IblPOBox" IsNeedEncode="false" runat="server" />
            </td></tr>
        </table>      
    </div>
    <div class="ACA_FLeft Confirm_table" runat="server">        
        <table role='presentation' id="tbLicenseExtDetail" style="line-height:16px;" border='0' cellpadding='0' cellspacing='0' runat="server">
           <tr>
            <td>
                <ACA:AccelaLabel ID="IblHomePhone" IsNeedEncode="false" runat="server" />
            </td></tr>
                        <tr><td>
                <ACA:AccelaLabel ID="IblMobilePhone" IsNeedEncode="false" runat="server" />
            </td></tr>
                        <tr><td>
                <ACA:AccelaLabel ID="IblFax" IsNeedEncode="false" runat="server" />
            </td></tr>
                        <tr><td>
                <ACA:AccelaLabel ID="IblClassCode" IsNeedEncode="false" runat="server" />
                <ACA:AccelaLabel ID="IblLicState" IsNeedEncode="false" runat="server" />      
                <ACA:AccelaLabel ID="IblLicenseType" IsNeedEncode="false" runat="server" />
                <ACA:AccelaLabel ID="IblClassCode2" IsNeedEncode="false" runat="server" />
                <ACA:AccelaLabel ID="IblLicState2" IsNeedEncode="false" runat="server" />
                <ACA:AccelaLabel ID="IblLicenseNumber" IsNeedEncode="false" runat="server" />
            </td></tr>
                        <tr><td>
                <ACA:AccelaLabel ID="IblContactType" IsNeedEncode="false" runat="server" />
            </td></tr>
                        <tr><td>
                <ACA:AccelaLabel ID="IblSSN" IsNeedEncode="false" runat="server" />
            </td></tr>
                        <tr><td>
                <ACA:AccelaLabel ID="IblFEIN" IsNeedEncode="false" runat="server" />
            </td></tr>
                        <tr><td>
                <ACA:AccelaLabel ID="IblContractorLicNo" IsNeedEncode="false" runat="server" />
            </td></tr>
                        <tr><td>
                <ACA:AccelaLabel ID="IblContractorBusNam" IsNeedEncode="false" runat="server" />
            </td></tr>
            <tr>
                <td>
                    <ACA:AccelaLabel ID="lblEmail" IsNeedEncode="false" runat="server" />
                </td>
            </tr>
        </table>      
    </div>
</div>
<div class="ACA_FLeft ACA_Row">
    <uc1:TemplateView ID="templateView" runat="server" />
</div>