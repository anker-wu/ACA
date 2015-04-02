<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.ContactList"
    CodeBehind="ContactList.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Common" %>
<asp:UpdatePanel ID="ContactListPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvContactList" runat="server" AllowPaging="true" AllowSorting="True"
            SummaryKey="gdv_contact_contactlist_summary" CaptionKey="aca_caption_contact_contactlist" ShowCaption="true" AutoGenerateColumns="False"
            OnRowDataBound="ContactList_RowDataBound" PagerStyle-HorizontalAlign="center"
            OnRowCommand="ContactList_RowCommand" OnPageIndexChanging="ContactList_PageIndexChanging"
            OnGridViewSort="ContactList_GridViewSort" PagerStyle-VerticalAlign="bottom"
            OnGridViewDownload="ContactList_GridViewDownload">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="RequiredImageColumn" ShowHeader="false">
                    <ItemTemplate>
                        <ACA:AccelaDiv ID="divImg" runat="server">
                            <img id="imgMarkRequired" alt="<%=GetTextByKey("img_alt_mark_required") %>" src="<%= ImageUtil.GetImageURL("error_16.gif") %>" />
                        </ACA:AccelaDiv>
                    </ItemTemplate>
                    <ItemStyle CssClass="ACA_VerticalAlign" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAdditionalAddresses" ExportDataField="AdditionalAddresses" ColumnId="AdditionalAddresses">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkAdditionalAddresses" runat="server" LabelKey="aca_contactlist_label_additionaladdresses"
                                SortExpression="AdditionalAddresses" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAdditionalAddresses" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.AdditionalAddresses.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkSalutation" ExportDataField="Salutation">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkSalutation" runat="server" LabelKey="per_contactList_label_salutation"
                                SortExpression="Salutation" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblSalutation" runat="server" Text='<%#StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_SALUTATION,Convert.ToString(DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.Salutation.ToString())))%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkTitle" ExportDataField="Title">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkTitle" runat="server" LabelKey="per_contactList_label_title"
                                SortExpression="Title" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblTitle" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.Title.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkFirstName" ExportDataField="FirstName"
                    ExportFormat="FilterScript">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkFirstName" runat="server" LabelKey="per_contactList_label_firstName"
                                SortExpression="FirstName" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <strong><ACA:AccelaLinkButton ID="btnFirstName" runat="server" CommandName="SelectContact" HideOnEmptyText="True"
                                CausesValidation="false" CommandArgument='<%#Container.DataItemIndex%>' Text='<%#ScriptFilter.FilterScript(DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.FirstName.ToString()).ToString())%>'></ACA:AccelaLinkButton></strong>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkMiddleName" ExportDataField="MiddleName"
                    ExportFormat="FilterScript">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkMiddleName" runat="server" LabelKey="per_contactList_label_middleName"
                                SortExpression="MiddleName" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <strong><ACA:AccelaLinkButton ID="btnMiddleName" runat="server" CommandName="SelectContact" HideOnEmptyText="True"
                                CausesValidation="false" CommandArgument='<%#Container.DataItemIndex%>' Text='<%#ScriptFilter.FilterScript(DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.MiddleName.ToString()).ToString())%>'></ACA:AccelaLinkButton></strong>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLastName" ExportDataField="LastName" ExportFormat="FilterScript">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkLastName" runat="server" LabelKey="per_contactList_label_lastName"
                                SortExpression="LastName" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <strong><ACA:AccelaLinkButton ID="btnLastName" runat="server" CommandName="SelectContact" HideOnEmptyText="True"
                                CausesValidation="false" CommandArgument='<%#Container.DataItemIndex%>' Text='<%#ScriptFilter.FilterScript(DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.LastName.ToString()).ToString())%>'></ACA:AccelaLinkButton></strong>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkFullName" ExportDataField="FullName" ExportFormat="FilterScript">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkFullName" runat="server" LabelKey="per_contactList_label_fullName"
                                SortExpression="FullName" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <strong><ACA:AccelaLinkButton ID="btnFullName" runat="server" CommandName="SelectContact" HideOnEmptyText="True"
                                Visible="true" CausesValidation="false" CommandArgument='<%#Container.DataItemIndex%>' Text='<%#ScriptFilter.FilterScript(DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.FullName.ToString()).ToString())%>'></ACA:AccelaLinkButton></strong>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkBirthDate" ExportDataField="BirthDate"
                    ExportFormat="ShortDate">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkBirthDate" runat="server" LabelKey="per_contactList_label_birthDate"
                                SortExpression="BirthDate" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaDateLabel ID="lblBirthDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.BirthDate.ToString())%>'></ACA:AccelaDateLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkGender" ExportDataField="Gender">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkGender" runat="server" LabelKey="per_contactList_label_gender"
                                SortExpression="Gender" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblGender" runat="server" Text='<%#StandardChoiceUtil.GetGenderByKey(Convert.ToString(DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.Gender.ToString())))%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="50px" />
                    <HeaderStyle Width="50px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkBusiness" ExportDataField="Business" ExportFormat="FilterScript">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkBusiness" runat="server" LabelKey="per_contactList_label_business"
                                SortExpression="Business" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <strong><ACA:AccelaLinkButton ID="btnBusiness" runat="server" CommandName="SelectContact" HideOnEmptyText="True"
                                Visible="true" CausesValidation="false" CommandArgument='<%#Container.DataItemIndex%>' Text='<%#ScriptFilter.FilterScript(DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.Business.ToString()).ToString())%>'></ACA:AccelaLinkButton></strong>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkTradeName" ExportDataField="TradeName">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkTradeName" runat="server" LabelKey="per_contactlist_label_tradename"
                                SortExpression="TradeName" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ACA:AccelaLabel ID="lblTradeName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.TradeName.ToString())%>' />
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkSSNHeader" ExportDataField="SocialSecurityNumber"
                    ExportFormat="SSN">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkSSNHeader" runat="server" LabelKey="per_contactlist_ssn"
                                SortExpression="SocialSecurityNumber" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ACA:AccelaLabel ID="lblSSN" runat="server" Text='<%#Accela.ACA.Common.Util.MaskUtil.FormatSSNShow(Convert.ToString(DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.SocialSecurityNumber.ToString())))%>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkFein" ExportDataField="Fein" ExportFormat="FEIN">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkFein" runat="server" LabelKey="per_contactlist_label_fein"
                                SortExpression="Fein" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ACA:AccelaLabel ID="lblFein" runat="server" Text='<%#Accela.ACA.Common.Util.MaskUtil.FormatFEINShow(Convert.ToString(DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.Fein.ToString())), StandardChoiceUtil.IsEnableFeinMasking())%>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkContactType" ExportDataField="ContactType">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkContactType" runat="server" LabelKey="per_contactList_label_contactType"
                                SortExpression="ContactType" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <strong><ACA:AccelaLinkButton ID="btnContactType" runat="server" CommandName="SelectContact" HideOnEmptyText="True"
                                CausesValidation="false" CommandArgument='<%#Container.DataItemIndex%>' Text='<%#StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_CONTACT_TYPE,Convert.ToString(DataBinder.Eval(Container.DataItem,ColumnConstant.Contact.ContactType.ToString())))%>'></ACA:AccelaLinkButton></strong>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="140px" />
                    <HeaderStyle Width="140px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkContactTypeFlag" ExportDataField="ContactTypeFlag">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkContactTypeFlag" runat="server" LabelKey="aca_contactlist_label_contacttype_flag"
                                SortExpression="ContactTypeFlag" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblContactTypeFlag" runat="server" Text='<%#DropDownListBindUtil.GetTypeFlagTextByValue(Convert.ToString(DataBinder.Eval(Container.DataItem, "contactTypeFlag")))%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddressLine1" ExportDataField="AddressLine1">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkAddressLine1" runat="server" LabelKey="per_contactList_label_address1"
                                SortExpression="AddressLine1" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAddressLine1" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.AddressLine1.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddressLine2" ExportDataField="AddressLine2">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkAddressLine2" runat="server" LabelKey="per_contactList_label_address2"
                                SortExpression="AddressLine2" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAddressLine2" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.AddressLine2.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddressLine3" ExportDataField="AddressLine3">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkAddressLine3" runat="server" LabelKey="per_contactlist_label_address3"
                                SortExpression="AddressLine3" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ACA:AccelaLabel ID="lblAddressLine3" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.AddressLine3.ToString())%>' />
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkCity" ExportDataField="City">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkCity" runat="server" LabelKey="per_contactList_label_city"
                                SortExpression="City" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblCity" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.City.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkState" ExportDataField="State">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkState" runat="server" LabelKey="per_contactList_label_state"
                                SortExpression="State" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblState" runat="server" Text='<%#I18nUtil.DisplayStateForI18N(DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.State.ToString()) as string, DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.Country.ToString()) as string)%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="50px" />
                    <HeaderStyle Width="50px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkZip" ExportDataField="Zip">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkZip" runat="server" LabelKey="per_contactList_label_zip"
                                SortExpression="Zip" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblZip" runat="server" Text='<%# ModelUIFormat.FormatZipShow(DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.Zip.ToString()) as string,  DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.Country.ToString()) as string)%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkPOBox" ExportDataField="POBox">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkPOBox" runat="server" LabelKey="per_contactList_label_poBox"
                                SortExpression="POBox" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblPOBox" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.POBox.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="50px" />
                    <HeaderStyle Width="50px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkCountry" ExportDataField="Country">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkCountry" runat="server" LabelKey="per_contactList_label_country"
                                SortExpression="Country" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ACA:AccelaLabel ID="lblCountry" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.Country.ToString())%>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkHomePhone" ExportDataField="HomePhone">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkHomePhone" runat="server" LabelKey="per_contactList_label_homePhone"
                                SortExpression="HomePhone" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblHomePhone" runat="server" IsNeedEncode="false" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.HomePhone.ToString())%>' />
                            <asp:HiddenField ID="hdnHomePhoneCode" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.HomePhoneCode.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkWorkPhone" ExportDataField="WorkPhone">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkWorkPhone" runat="server" LabelKey="per_contactList_label_workPhone"
                                SortExpression="WorkPhone" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblWorkPhone" runat="server" IsNeedEncode="false" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.WorkPhone.ToString())%>' />
                            <asp:HiddenField ID="hdnWorkPhoneCode" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.WorkPhoneCode.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkMobilePhone" ExportDataField="MobilePhone">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkMobilePhone" runat="server" LabelKey="per_contactList_label_mobilePhone"
                                SortExpression="MobilePhone" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblMobilePhone" runat="server" IsNeedEncode="false" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.MobilePhone.ToString())%>' />
                            <asp:HiddenField ID="hdnMobilePhoneCode" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.MobilePhoneCode.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkFax" ExportDataField="Fax">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkFax" runat="server" LabelKey="per_contactList_label_fax"
                                SortExpression="Fax" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblFax" runat="server" IsNeedEncode="false" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.Fax.ToString())%>' />
                            <asp:HiddenField ID="hdnFaxCode" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.FaxCode.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkEmail" ExportDataField="Email">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkEmail" runat="server" LabelKey="per_contactList_label_email"
                                SortExpression="Email" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblEmail" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.Email.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="160px" />
                    <HeaderStyle Width="160px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkContactPermission" ExportDataField="ContactPermission">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkContactPermission" runat="server" LabelKey="per_contactlist_contactpermission"
                                SortExpression="ContactPermission" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblContactPermission" IsNeedEncode="false" runat="server" Text='<%#DropDownListBindUtil.GetContactPermissionTextByValue(Convert.ToString(DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.ContactPermission.ToString())), ModuleName)%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="230px" />
                    <HeaderStyle Width="230px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkActionHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:AccelaLabel ID="lnkActionHeader" runat="server" LabelKey="aca_contactlist_label_action"
                                IsGridViewHeadLabel="true" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div runat="server" id="divAction">
                            <ACA:AccelaLinkButton ID="lnkEdit" LabelKey="aca_contactlist_label_lnkedit" runat="server"
                             CommandName="SelectContact" CausesValidation="false"
                                CommandArgument='<%#Container.DataItemIndex%>'></ACA:AccelaLinkButton>
                            <span>&nbsp;</span>
                            <ACA:AccelaLinkButton ID="btnDelete" LabelKey="per_contactList_label_delete" runat="server"
                                CommandName="DeleteContact" CausesValidation="false"
                                CommandArgument='<%#Container.DataItemIndex%>'></ACA:AccelaLinkButton>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkSuffix" ExportDataField="SuffixName">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkSuffix" runat="server" LabelKey="aca_contactlist_label_suffix"
                                SortExpression="SuffixName" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblSuffix" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.SuffixName.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkBusinessName2" ExportDataField="BusinessName2">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkBusinessName2" runat="server" LabelKey="aca_contactlist_label_businessname2"
                                SortExpression="BusinessName2" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ACA:AccelaLabel ID="lblBusinessName2" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.BusinessName2.ToString())%>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkBirthplaceCity" ExportDataField="BirthplaceCity">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkBirthplaceCity" runat="server" LabelKey="aca_contactlist_label_birthplacecity"
                                SortExpression="BirthplaceCity" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ACA:AccelaLabel ID="lblBirthplaceCity" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.BirthplaceCity.ToString())%>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkBirthplaceState" ExportDataField="BirthplaceState">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkBirthplaceState" runat="server" LabelKey="aca_contactlist_label_birthplacestate"
                                SortExpression="BirthplaceState" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ACA:AccelaLabel ID="lblBirthplaceState" runat="server" Text='<%#I18nUtil.DisplayStateForI18N(DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.BirthplaceState.ToString()) as string, DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.BirthplaceCountry.ToString()) as string)%>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkBirthCountry" ExportDataField="BirthplaceCountry">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkBirthCountry" runat="server" LabelKey="aca_contactlist_label_birthcountry"
                                SortExpression="BirthplaceCountry" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ACA:AccelaLabel ID="lblBirthCountry" runat="server" Text='<%#StandardChoiceUtil.GetCountryByKey(Convert.ToString(DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.BirthplaceCountry.ToString())))%>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkRace" ExportDataField="Race">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkRace" runat="server" LabelKey="aca_contactlist_label_race"
                                SortExpression="Race" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ACA:AccelaLabel ID="lblRace" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.Race.ToString())%>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkDeceasedDate" ExportDataField="DeceasedDate">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkDeceasedDate" runat="server" LabelKey="aca_contactlist_label_deceaseddate"
                                SortExpression="DeceasedDate" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ACA:AccelaLabel ID="lblDeceasedDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.DeceasedDate.ToString())%>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkPassportNumber" ExportDataField="PassportNumber">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkPassportNumber" runat="server" LabelKey="aca_contactlist_label_passportnumber"
                                SortExpression="PassportNumber" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ACA:AccelaLabel ID="lblPassportNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.PassportNumber.ToString())%>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkDriverLicenseNumber" ExportDataField="DriverLicenseNumber">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkDriverLicenseNumber" runat="server" LabelKey="aca_contactlist_label_driverlicensenumber"
                                SortExpression="DriverLicenseNumber" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ACA:AccelaLabel ID="lblDriverLicenseNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.DriverLicenseNumber.ToString())%>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkDriverLicenseState" ExportDataField="DriverLicenseState">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkDriverLicenseState" runat="server" LabelKey="aca_contactlist_label_driverlicensestate"
                                SortExpression="DriverLicenseState" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ACA:AccelaLabel ID="lblDriverLicenseState" runat="server" Text='<%#I18nUtil.DisplayStateForI18N(DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.DriverLicenseState.ToString()) as string, DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.Country.ToString()) as string)%>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkStateIdNumber" ExportDataField="StateIdNumber">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkStateIdNumber" runat="server" LabelKey="aca_contactlist_label_stateidnumber"
                                SortExpression="StateIdNumber" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ACA:AccelaLabel ID="lblStateIdNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.StateIdNumber.ToString())%>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkPreferredChannel" ExportDataField="PreferredChannel">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkPreferredChannel" runat="server" LabelKey="aca_contactlist_label_preferredchannel"
                                SortExpression="PreferredChannel" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ACA:AccelaLabel ID="lblPreferredChannel" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.PreferredChannel.ToString())%>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkComment" ExportDataField="Comment">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkComment" runat="server" LabelKey="aca_contactlist_label_comment"
                                SortExpression="Comment" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <ACA:AccelaLabel ID="lblComment" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Contact.Comment.ToString())%>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
        <asp:HiddenField ID="hfIsContactTypeNull" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
