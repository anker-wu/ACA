<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.ContactView" Codebehind="ContactView.ascx.cs" %>
<%@ Register Src="TemplateView.ascx" TagName="TemplateView" TagPrefix="uc1" %>
<%@ Register Src="~/Component/GenericTemplateView.ascx" TagName="GenericTemplate" TagPrefix="ACA" %>

<div class="ACA_Row">
    <div id="divContactInfo" runat="server" Visible="False">
        <div class="ACA_ConfigInfo ACA_FLeft">
            <ASP:Literal ID="lblContractBasic" runat="server"></ASP:Literal>
        </div>
        <div class="ACA_ConfigInfo ACA_FLeft Confirm_table">
            <ACA:AccelaLabel ID="lblContractExt" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
        </div>
    </div>
    <div id="divContactViewInfo" runat="server" Visible="False" class="ACA_SmLabel ACA_SmLabel_FontSize">
        <ACA:AccelaLabel ID="lblContactViewInfo" IsNeedEncode="false" runat="server" />
    </div>
    <asp:PlaceHolder runat="server" ID="pnlTemplate" Visible="False">
        <div class="ACA_FLeft ACA_Row">
            <uc1:TemplateView ID="templateView" runat="server" />
        </div>
        <ACA:GenericTemplate ID="genericTemplate" runat="server" />
    </asp:PlaceHolder>
    <div id="divContactView" visible="false" runat="server" class="ACA_Page">
        <span>Click the area below to edit contact available variables according to format:</span><br />
        <span>$$ContactType$$: The contact type</span><br />
        <span>$$ContactTypeFlag$$: The contact type flag</span><br />
        <span>$$Salutation$$: The contact salutation</span><br />
        <span>$$Title$$: The contact title</span><br />
        <span>$$FirstName$$: The contact first name</span><br />
        <span>$$MiddleName$$: The contact middle name</span><br />
        <span>$$LastName$$: The contact last name</span><br />
        <span>$$FullName$$: The contact full name</span><br />
        <span>$$NameSuffix$$: The contact Suffix</span><br />
        <span>$$BirthDate$$: The contact birth date</span><br />
        <span>$$Gender$$: The contact gender</span><br />
        <span>$$BusinessName$$: The contact business name</span><br />
        <span>$$BusinessName2$$: The contact business name2</span><br />
        <span>$$TradeName$$: The contact trade name</span><br />
        <span>$$SocialSecurityNumber$$: The contact SSN</span><br />
        <span>$$Fein$$: The contact fein</span><br />
        <span>$$AddressLine1$$: The contact address line1</span><br />
        <span>$$AddressLine2$$: The contact address line2</span><br />
        <span>$$AddressLine3$$: The contact address line3</span><br />
        <span>$$City$$: The contact city</span><br />
        <span>$$State$$: The contact state</span><br />
        <span>$$Zip$$: The contact zip</span><br />
        <span>$$CountryCode$$: The contact country code</span><br />
        <span>$$PostOfficeBox$$: The contact post office box</span><br />
        <span>$$Phone1$$: The contact phone1</span><br />
        <span>$$Phone2$$: The contact phone2</span><br />
        <span>$$Phone3$$: The contact phone3</span><br />
        <span>$$Fax$$: The contact fax</span><br />
        <span>$$Email$$: The contact email</span><br />
        <% if (ShowAccessLevel)
           { %>
        <span>$$AccessLevel$$: The contact access level</span><br />
        <% } %>
        <span>$$BirthplaceCity$$: The birth place city</span><br />
        <span>$$BirthplaceState$$: The birth place state</span><br />
        <span>$$BirthplaceCountry$$: The birth place country</span><br />
        <span>$$Race$$: The race</span><br />
        <span>$$DeceasedDate$$: The deceased date</span><br />
        <span>$$PassportNumber$$: The passport number</span><br />
        <span>$$DriverLicenseNumber$$: The driver's license number</span><br />
        <span>$$DriverLicenseState$$: The driver's license state</span><br />
        <span>$$StateNumber$$: The state ID number</span><br />
        <span>$$PreferredChannel$$: The preferred channel</span><br />
        <span>$$Notes$$: The notes</span>
        <div style="margin-top: 15px">
            <ACA:AccelaLabel ID="lblContactViewPattern" LabelType="BodyText" runat="server" />
        </div>
    </div>
</div>
