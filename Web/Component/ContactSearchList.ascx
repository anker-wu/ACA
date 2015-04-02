<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactSearchList.ascx.cs" Inherits="Accela.ACA.Web.Component.ContactSearchList" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="~/Component/Conditions.ascx" TagName="Conditions" TagPrefix="uc1" %>

<div id="divConditon">
    <uc1:Conditions ID="ucConditon" runat="server" Visible="False" />
</div>
<ACA:AccelaGridView ID="gdvSearchContactList" AllowPaging="true" AllowSorting="True" ShowCaption="true" AutoGenerateColumns="False" PagerStyle-HorizontalAlign="center" 
    runat="server" PagerStyle-VerticalAlign="bottom" SummaryKey="gdv_contact_contactlist_summary" CaptionKey="aca_caption_contact_contactlist" IsInSPEARForm="true" AutoGenerateRadioButtonColumn="True"
    OnRowDataBound="SearchContactList_RowDataBound" OnRowCommand="SearchContactList_RowCommand" OnPageIndexChanging="RefContactList_PageIndexChanging">
    <Columns>
        <ACA:AccelaTemplateField AttributeName="lnkSearchAgency">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkSearchAgency" runat="server" LabelKey="aca_searchcontactlist_label_agencyheader" SortExpression="SearchAgencyText" CausesValidation="false" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblSearchAgency" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SearchAgencyText")%>' />
                </div>
            </ItemTemplate>
            <ItemStyle Width="80px" />
            <HeaderStyle Width="80px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkSearchCategory">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkSearchCategory" runat="server" LabelKey="aca_searchcontactlist_label_categoryheader" SortExpression="SearchCategoryText" CausesValidation="false" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblSearchCategory" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SearchCategoryText")%>' />
                </div>
            </ItemTemplate>
            <ItemStyle Width="80px" />
            <HeaderStyle Width="80px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkSearchType">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkSearchType" runat="server" LabelKey="aca_searchcontactlist_label_typeheader" SortExpression="SearchContactType" CausesValidation="false" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblSearchType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SearchContactType")%>' />
                </div>
            </ItemTemplate>
            <ItemStyle Width="80px" />
            <HeaderStyle Width="80px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkSearchName">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkSearchName" runat="server" LabelKey="aca_searchcontactlist_label_nameheader"
                        SortExpression="SearchContactName" CausesValidation="false" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblSearchName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SearchContactName")%>' />
                </div>
            </ItemTemplate>
            <ItemStyle Width="100px" />
            <HeaderStyle Width="100px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkSalutation">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkSalutation" runat="server" LabelKey="aca_refcontactlist_label_salutation"
                        SortExpression="salutation" CommandName="Header" CausesValidation="false" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblSalutation" runat="server" Text='<%#StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_SALUTATION,Convert.ToString(DataBinder.Eval(Container.DataItem, "salutation")))%>' />
                </div>
            </ItemTemplate>
            <ItemStyle Width="100px" />
            <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkTitle">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkTitle" runat="server" LabelKey="aca_refcontactlist_label_title"
                    SortExpression="title" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <ACA:AccelaLabel ID="lblTitle" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "title")%>' />
            </div>
        </ItemTemplate>
        <ItemStyle Width="80px" />
        <HeaderStyle Width="80px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkFirstName">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkFirstName" runat="server" LabelKey="aca_refcontactlist_label_firstname"
                    SortExpression="firstName" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <strong>
                    <ACA:AccelaLabel  ID="lblFirstName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "firstName")%>' /></strong>
            </div>
        </ItemTemplate>
        <ItemStyle Width="80px" />
        <HeaderStyle Width="80px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkMiddleName">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkMiddleName" runat="server" LabelKey="aca_refcontactlist_label_middlename"
                    SortExpression="middleName" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <strong>
                    <ACA:AccelaLabel ID="lblMiddleName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "middleName")%>' /></strong>
            </div>
        </ItemTemplate>
        <ItemStyle Width="80px" />
        <HeaderStyle Width="80px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkLastName">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkLastName" runat="server" LabelKey="aca_refcontactlist_label_lastname"
                    SortExpression="lastName" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <strong>
                    <ACA:AccelaLabel ID="lblLastName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "lastName")%>' /></strong>
            </div>
        </ItemTemplate>
        <ItemStyle Width="80px" />
        <HeaderStyle Width="80px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkFullName">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkFullName" runat="server" LabelKey="aca_refcontactlist_label_fullname"
                    SortExpression="fullName" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <strong>
                    <ACA:AccelaLabel ID="lblFullName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "fullName")%>' />
                </strong>
            </div>
        </ItemTemplate>
        <ItemStyle Width="80px" />
        <HeaderStyle Width="80px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkBirthDate">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkBirthDate" runat="server" LabelKey="aca_refcontactlist_label_birthdate"
                    SortExpression="birthDate" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <ACA:AccelaDateLabel ID="lblBirthDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "birthDate")%>'></ACA:AccelaDateLabel>
            </div>
        </ItemTemplate>
        <ItemStyle Width="80px" />
        <HeaderStyle Width="80px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkGender">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkGender" runat="server" LabelKey="aca_refcontactlist_label_gender"
                    SortExpression="gender" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <ACA:AccelaLabel ID="lblGender" runat="server" Text='<%#StandardChoiceUtil.GetGenderByKey(Convert.ToString(DataBinder.Eval(Container.DataItem, "gender")))%>' />
            </div>
        </ItemTemplate>
        <ItemStyle Width="50px" />
        <HeaderStyle Width="50px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkBusiness">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkBusiness" runat="server" LabelKey="aca_refcontactlist_label_business"
                    SortExpression="businessName" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <strong>
                    <ACA:AccelaLabel ID="lblBusiness" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "businessName")%>' /></strong>
            </div>
        </ItemTemplate>
        <ItemStyle Width="100px" />
        <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkTradeName">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkTradeName" runat="server" LabelKey="aca_refcontactlist_label_tradename"
                    SortExpression="tradeName" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <ACA:AccelaLabel ID="lblTradeName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "tradeName")%>' />
        </ItemTemplate>
        <ItemStyle Width="130px" />
        <HeaderStyle Width="130px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkSSN">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkSSN" runat="server" LabelKey="aca_refcontactlist_label_ssn"
                    SortExpression="socialSecurityNumber" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <ACA:AccelaLabel ID="lblSSN" runat="server" Text='<%#Accela.ACA.Common.Util.MaskUtil.FormatSSNShow(Convert.ToString(DataBinder.Eval(Container.DataItem, "socialSecurityNumber")))%>' />
        </ItemTemplate>
        <ItemStyle Width="100px" />
        <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkFein">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkFein" runat="server" LabelKey="aca_refcontactlist_label_fein"
                    SortExpression="fein" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <ACA:AccelaLabel ID="lblFein" runat="server" Text='<%#Accela.ACA.Common.Util.MaskUtil.FormatFEINShow(Convert.ToString(DataBinder.Eval(Container.DataItem, "fein")), StandardChoiceUtil.IsEnableFeinMasking())%>' />
        </ItemTemplate>
        <ItemStyle Width="100px" />
        <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkContactType">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkContactType" runat="server" LabelKey="aca_refcontactlist_label_contacttype"
                    SortExpression="contactType" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <strong>
                    <ACA:AccelaLabel ID="lblContactType" runat="server" Text='<%#StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_CONTACT_TYPE,Convert.ToString(DataBinder.Eval(Container.DataItem,"contactType")))%>' /></strong>
            </div>
        </ItemTemplate>
        <ItemStyle Width="140px" />
        <HeaderStyle Width="140px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkContactTypeFlag">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkContactTypeFlag" runat="server" LabelKey="aca_refcontactlist_label_contacttypeflag"
                    SortExpression="contactTypeFlag" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <ACA:AccelaLabel ID="lblContactTypeFlag" runat="server" Text='<%#DropDownListBindUtil.GetTypeFlagTextByValue(Convert.ToString(DataBinder.Eval(Container.DataItem, "contactTypeFlag")))%>' />
            </div>
        </ItemTemplate>
        <ItemStyle Width="130px" />
        <HeaderStyle Width="130px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkAddressLine1">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkAddressLine1" runat="server" LabelKey="aca_refcontactlist_label_address1"
                    SortExpression="compactAddress.addressLine1" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <ACA:AccelaLabel ID="lblAddressLine1" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "compactAddress.addressLine1")%>' />
            </div>
        </ItemTemplate>
        <ItemStyle Width="130px" />
        <HeaderStyle Width="130px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkAddressLine2">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkAddressLine2" runat="server" LabelKey="aca_refcontactlist_label_address2"
                    SortExpression="compactAddress.addressLine2" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <ACA:AccelaLabel ID="lblAddressLine2" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "compactAddress.addressLine2")%>' />
            </div>
        </ItemTemplate>
        <ItemStyle Width="130px" />
        <HeaderStyle Width="130px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkAddressLine3">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkAddressLine3" runat="server" LabelKey="aca_refcontactlist_label_address3"
                    SortExpression="compactAddress.addressLine3" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <ACA:AccelaLabel ID="lblAddressLine3" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "compactAddress.addressLine3")%>' />
        </ItemTemplate>
        <ItemStyle Width="130px" />
        <HeaderStyle Width="130px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkCity">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkCity" runat="server" LabelKey="aca_refcontactlist_label_city"
                    SortExpression="compactAddress.city" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <ACA:AccelaLabel ID="lblCity" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "compactAddress.city")%>' />
            </div>
        </ItemTemplate>
        <ItemStyle Width="80px" />
        <HeaderStyle Width="80px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkState">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkState" runat="server" LabelKey="aca_refcontactlist_label_state"
                    SortExpression="compactAddress.state" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <ACA:AccelaLabel ID="lblState" runat="server" Text='<%#I18nUtil.DisplayStateForI18N(DataBinder.Eval(Container.DataItem, "compactAddress.state") as string, DataBinder.Eval(Container.DataItem, "compactAddress.countryCode") as string)%>' />
            </div>
        </ItemTemplate>
        <ItemStyle Width="50px" />
        <HeaderStyle Width="50px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkZip">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkZip" runat="server" LabelKey="aca_refcontactlist_label_zip"
                    SortExpression="compactAddress.zip" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <ACA:AccelaLabel ID="lblZip" runat="server" Text='<%#ModelUIFormat.FormatZipShow(Convert.ToString(DataBinder.Eval(Container.DataItem, "compactAddress.zip")), Convert.ToString(DataBinder.Eval(Container.DataItem, "compactAddress.countryCode"))) %>' />
            </div>
        </ItemTemplate>
        <ItemStyle Width="80px" />
        <HeaderStyle Width="80px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkPOBox">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkPOBox" runat="server" LabelKey="aca_refcontactlist_label_pobox"
                    SortExpression="postOfficeBox" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <ACA:AccelaLabel ID="lblPOBox" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "postOfficeBox")%>' />
            </div>
        </ItemTemplate>
        <ItemStyle Width="50px" />
        <HeaderStyle Width="50px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkCountry">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkCountry" runat="server" LabelKey="aca_refcontactlist_label_country"
                    SortExpression="compactAddress.countryCode" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <ACA:AccelaLabel ID="lblCountry" runat="server" Text='<%#StandardChoiceUtil.GetCountryByKey(Convert.ToString(DataBinder.Eval(Container.DataItem, "compactAddress.countryCode")))%>' />
        </ItemTemplate>
        <ItemStyle Width="100px" />
        <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkHomePhone">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkHomePhone" runat="server" LabelKey="aca_refcontactlist_label_homephone"
                    SortExpression="phone1" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <ACA:AccelaLabel ID="lblHomePhone" runat="server" IsNeedEncode="false" Text='<%#ModelUIFormat.FormatPhoneShow(Convert.ToString(DataBinder.Eval(Container.DataItem, "phone1CountryCode")) ,Convert.ToString(DataBinder.Eval(Container.DataItem, "phone1")), Convert.ToString(DataBinder.Eval(Container.DataItem, "compactAddress.countryCode")))%>' />
            </div>
        </ItemTemplate>
        <ItemStyle Width="130px" />
        <HeaderStyle Width="130px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkWorkPhone">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkWorkPhone" runat="server" LabelKey="aca_refcontactlist_label_workphone"
                    SortExpression="phone3" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <ACA:AccelaLabel ID="lblWorkPhone" runat="server" IsNeedEncode="false" Text='<%#ModelUIFormat.FormatPhoneShow(Convert.ToString(DataBinder.Eval(Container.DataItem, "phone3CountryCode")), Convert.ToString(DataBinder.Eval(Container.DataItem, "phone3")), Convert.ToString(DataBinder.Eval(Container.DataItem, "compactAddress.countryCode")))%>' />
            </div>
        </ItemTemplate>
        <ItemStyle Width="130px" />
        <HeaderStyle Width="130px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkMobilePhone">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkMobilePhone" runat="server" LabelKey="aca_refcontactlist_label_mobilephone"
                    SortExpression="phone2" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <ACA:AccelaLabel ID="lblMobilePhone" runat="server" IsNeedEncode="false" Text='<%#ModelUIFormat.FormatPhoneShow(Convert.ToString(DataBinder.Eval(Container.DataItem, "phone2CountryCode")), Convert.ToString(DataBinder.Eval(Container.DataItem, "phone2")), Convert.ToString(DataBinder.Eval(Container.DataItem, "compactAddress.countryCode")))%>' />
            </div>
        </ItemTemplate>
        <ItemStyle Width="130px" />
        <HeaderStyle Width="130px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkFax">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkFax" runat="server" LabelKey="aca_refcontactlist_label_fax"
                    SortExpression="fax" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <ACA:AccelaLabel ID="lblFax" runat="server" IsNeedEncode="false" Text='<%#ModelUIFormat.FormatPhoneShow(Convert.ToString(DataBinder.Eval(Container.DataItem, "faxCountryCode")), Convert.ToString(DataBinder.Eval(Container.DataItem, "fax")), Convert.ToString(DataBinder.Eval(Container.DataItem, "compactAddress.countryCode")))%>' />
            </div>
        </ItemTemplate>
        <ItemStyle Width="130px" />
        <HeaderStyle Width="130px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkEmail">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkEmail" runat="server" LabelKey="aca_refcontactlist_label_email"
                    SortExpression="email" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <ACA:AccelaLabel ID="lblEmail" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "email")%>' />
            </div>
        </ItemTemplate>
        <ItemStyle Width="160px" />
        <HeaderStyle Width="160px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkStatusHeader">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkStatusHeader" runat="server" LabelKey="aca_refcontactlist_label_status"
                    SortExpression="contractorPeopleStatus" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <ACA:AccelaLabel ID="lblStatus" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "contractorPeopleStatus")%>' />
            </div>
        </ItemTemplate>
        <ItemStyle Width="100px" />
        <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkBusinessName2">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkBusinessName2" runat="server" LabelKey="aca_refcontactlist_label_businessname2"
                    SortExpression="businessName2" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <ACA:AccelaLabel ID="lblBusinessName2" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "businessName2")%>' />
        </ItemTemplate>
        <ItemStyle Width="100px" />
        <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkBirthplaceCity">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkBirthplaceCity" runat="server" LabelKey="aca_refcontactlist_label_birthplacecity"
                    SortExpression="birthCity" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <ACA:AccelaLabel ID="lblBirthplaceCity" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "birthCity")%>' />
        </ItemTemplate>
        <ItemStyle Width="100px" />
        <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkBirthplaceState">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkBirthplaceState" runat="server" LabelKey="aca_refcontactlist_label_birthplacestate"
                    SortExpression="birthState" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <ACA:AccelaLabel ID="lblBirthplaceState" runat="server" Text='<%#I18nUtil.DisplayStateForI18N(DataBinder.Eval(Container.DataItem, "birthState") as string, DataBinder.Eval(Container.DataItem, "birthRegion") as string)%>' />
        </ItemTemplate>
        <ItemStyle Width="100px" />
        <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkBirthCountry">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkBirthCountry" runat="server" LabelKey="aca_refcontactlist_label_birthcountry"
                    SortExpression="birthRegion" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <ACA:AccelaLabel ID="lblBirthCountry" runat="server" Text='<%#StandardChoiceUtil.GetCountryByKey(Convert.ToString(DataBinder.Eval(Container.DataItem, "birthRegion")))%>' />
        </ItemTemplate>
        <ItemStyle Width="100px" />
        <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkRace">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkRace" runat="server" LabelKey="aca_refcontactlist_label_race"
                    SortExpression="race" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <ACA:AccelaLabel ID="lblRace" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "race")%>' />
        </ItemTemplate>
        <ItemStyle Width="100px" />
        <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkDeceasedDate">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkDeceasedDate" runat="server" LabelKey="aca_refcontactlist_label_deceaseddate"
                    SortExpression="deceasedDate" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <ACA:AccelaDateLabel ID="lblDeceasedDate" runat="server" DateType="ShortDate" Text2='<%#DataBinder.Eval(Container.DataItem, "deceasedDate")%>' />
        </ItemTemplate>
        <ItemStyle Width="100px" />
        <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkPassportNumber">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkPassportNumber" runat="server" LabelKey="aca_refcontactlist_label_passportnumber"
                    SortExpression="passportNumber" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <ACA:AccelaLabel ID="lblPassportNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "passportNumber")%>' />
        </ItemTemplate>
        <ItemStyle Width="100px" />
        <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkDriverLicenseNumber">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkDriverLicenseNumber" runat="server" LabelKey="aca_refcontactlist_label_driverlicensenumber"
                    SortExpression="driverLicenseNbr" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <ACA:AccelaLabel ID="lblDriverLicenseNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "driverLicenseNbr")%>' />
        </ItemTemplate>
        <ItemStyle Width="100px" />
        <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkDriverLicenseState">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkDriverLicenseState" runat="server" LabelKey="aca_refcontactlist_label_driverlicensestate"
                    SortExpression="driverLicenseState" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <ACA:AccelaLabel ID="lblDriverLicenseState" runat="server" Text='<%#I18nUtil.DisplayStateForI18N(DataBinder.Eval(Container.DataItem, "driverLicenseState") as string, DataBinder.Eval(Container.DataItem, "countryCode") as string)%>' />
        </ItemTemplate>
        <ItemStyle Width="100px" />
        <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkStateIdNumber">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkStateIdNumber" runat="server" LabelKey="aca_refcontactlist_label_stateidnumber"
                    SortExpression="stateIDNbr" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <ACA:AccelaLabel ID="lblStateIdNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "stateIDNbr")%>' />
        </ItemTemplate>
        <ItemStyle Width="100px" />
        <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkPreferredChannel" ExportDataField="PreferredChannel">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkPreferredChannel" runat="server" LabelKey="aca_refcontactlist_label_preferredchannel"
                    SortExpression="preferredChannel" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <ACA:AccelaLabel ID="lblPreferredChannel" runat="server" />
        </ItemTemplate>
        <ItemStyle Width="100px" />
        <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    <ACA:AccelaTemplateField AttributeName="lnkComment" ExportDataField="Comment">
        <HeaderTemplate>
            <div class="ACA_Header_Row">
                <ACA:GridViewHeaderLabel ID="lnkComment" runat="server" LabelKey="aca_refcontactlist_label_comment"
                    SortExpression="comment" CommandName="Header" CausesValidation="false" />
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <ACA:AccelaLabel ID="lblComment" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "comment")%>' />
        </ItemTemplate>
        <ItemStyle Width="100px" />
        <HeaderStyle Width="100px" />
    </ACA:AccelaTemplateField>
    </Columns>
</ACA:AccelaGridView>
<asp:LinkButton ID="btnConditionsCheck" runat="Server" CssClass="ACA_Hide"
    OnClick="ConditionsCheckButton_Click" TabIndex="-1"></asp:LinkButton>
