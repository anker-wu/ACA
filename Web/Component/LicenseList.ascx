<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.LicenseList"
    CodeBehind="LicenseList.ascx.cs" %>
<asp:UpdatePanel ID="LicenseListPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvLicenseList" runat="server" AllowPaging="true" AllowSorting="True" 
            SummaryKey="aca_summary_license_licenselist" CaptionKey="aca_caption_license_licenselist"
            ShowCaption="true" AutoGenerateColumns="False" GridViewNumber="60081"
            PagerStyle-HorizontalAlign="center" PageSize="10" OnRowDataBound="LicenseList_RowDataBound"
            OnRowCommand="LicenseList_RowCommand" OnPageIndexChanging="LicenseList_PageIndexChanging"
            OnGridViewSort="LicenseList_GridViewSort" IsInSPEARForm="true">
            <Columns>
                <ACA:AccelaTemplateField ShowHeader="false">
                    <ItemTemplate>
                        <ACA:AccelaDiv ID="divImg" runat="server">
                            <img id="imgMarkRequired" src="<%= ImageUtil.GetImageURL("error_16.gif") %>" alt="<%=GetTextByKey("img_alt_mark_required") %>" />
                        </ACA:AccelaDiv>
                    </ItemTemplate>
                    <ItemStyle CssClass="ACA_VerticalAlign" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLicenseNumberHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkLicenseNumberHeader" runat="server" CommandName="Header"
                                SortExpression="LicenseNumber" LabelKey="per_licenseList_label_licenseNumber"
                                CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLinkButton ID="lnkLicenseNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LicenseNumber") %>'
                                CausesValidation="false" CommandName="SelectLicense"
                                CommandArgument='<%#Container.DataItemIndex%>'></ACA:AccelaLinkButton>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLicenseTypeHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkLicenseTypeHeader" runat="server" CommandName="Header"
                                SortExpression="LicenseType" LabelKey="per_licenseList_label_licenseType" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblLicenseType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "LicenseTypeText") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkContactTypeHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkContactTypeHeader" runat="server" CommandName="Header"
                                SortExpression="ContactType" LabelKey="per_licenseList_contacttype" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblContactType" runat="server" Text='<%#DropDownListBindUtil.GetTypeFlagTextByValue(Convert.ToString(DataBinder.Eval(Container.DataItem, "ContactType"))) %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkSSNHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkSSNHeader" runat="server" CommandName="Header" SortExpression="MaskedSSN"
                                LabelKey="per_licenseList_ssn" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblSSN" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaskedSSN") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkFEINHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkFEINHeader" runat="server" CommandName="Header" SortExpression="FEIN"
                                LabelKey="per_licenseList_fein" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblFEIN" runat="server" Text='<%#Accela.ACA.Common.Util.MaskUtil.FormatFEINShow(Convert.ToString(DataBinder.Eval(Container.DataItem, "FEIN")), StandardChoiceUtil.IsEnableFeinMasking()) %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkSalutationHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkSalutationHeader" runat="server" CommandName="Header"
                                SortExpression="Salutation" LabelKey="per_licenseList_label_salution" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblSalutation" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Salutation") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkContactNameHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkContactNameHeader" runat="server" CommandName="Header"
                                SortExpression="ContractName" LabelKey="per_licenseList_label_contactName" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblContactName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ContractName").ToString() %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="160px" />
                    <HeaderStyle Width="160px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkBirthDateHeader">
                    <HeaderTemplate>
                        <div>
                        <ACA:GridViewHeaderLabel ID="lnkBirthDateHeader" runat="server" SortExpression="BirthDate"
                            LabelKey="per_licenseList_label_birthDate"></ACA:GridViewHeaderLabel>
                        </div>    
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaDateLabel ID="lblBirthDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "BirthDate")%>'></ACA:AccelaDateLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkGenderHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkGenderHeader" runat="server" LabelKey="per_licenseList_label_gender"
                                SortExpression="Gender" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblGender" runat="server" Text='<%#StandardChoiceUtil.GetGenderByKey(Convert.ToString(DataBinder.Eval(Container.DataItem, "Gender")))%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkBusinessNameHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkBusinessNameHeader" runat="server" CommandName="Header"
                                SortExpression="BusinessName" LabelKey="per_licenseList_label_businessName" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblBusinessName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BusinessName") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkBusinessName2Header">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkBusinessName2Header" runat="server" CommandName="Header"
                                SortExpression="BusinessName2" LabelKey="per_licenseList_label_businessName2"
                                CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblBusinessName2" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "BusinessName2") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkBusinessLicenseHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkBusinessLicenseHeader" runat="server" CommandName="Header"
                                SortExpression="BusinessName2" LabelKey="per_licenselist_label_businesslicense"
                                CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblBusinessLicense" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "BusinessLicense") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddress1Header">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkAddress1Header" runat="server" LabelKey="per_licenseList_label_address1"
                                SortExpression="Address1" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAddress1" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Address1")%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddress2Header">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkAddress2Header" runat="server" LabelKey="per_licenseList_label_address2"
                                SortExpression="Address2" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAddress2" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Address2")%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddress3Header">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkAddress3Header" runat="server" LabelKey="per_licenseList_label_address3"
                                SortExpression="Address3" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAddress3" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Address3")%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkCityHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkCityHeader" runat="server" LabelKey="per_licenseList_label_city"
                                SortExpression="City" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblCity" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "City")%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkStateHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkStateHeader" runat="server" LabelKey="per_licenseList_label_state"
                                SortExpression="State" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                            <ACA:AccelaLabel ID="lblState" runat="server" Text='<%#I18nUtil.DisplayStateForI18N(DataBinder.Eval(Container.DataItem, "State") as string, DataBinder.Eval(Container.DataItem, "CountryCode") as string)%>' />
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkZipCodeHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkZipCodeHeader" runat="server" LabelKey="per_licenseList_label_zipCode"
                                SortExpression="Zip" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblZip" runat="server" Text='<%#ModelUIFormat.FormatZipShow(Convert.ToString(DataBinder.Eval(Container.DataItem, "Zip")), Convert.ToString(DataBinder.Eval(Container.DataItem, "CountryCode")))%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkPOBoxHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkPOBoxHeader" runat="server" LabelKey="per_licenseList_label_POBox"
                                SortExpression="PostOfficeBox" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblPOBox" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "PostOfficeBox")%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkHomePhoneHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkHomePhoneHeader" runat="server" LabelKey="per_licenseList_label_homePhoneNumber"
                                SortExpression="Phone" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblPhone" IsNeedEncode="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Phone")%>' />
                            <asp:HiddenField ID="hdnPhoneIDD" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "PhoneIDD")%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkMobilePhoneHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkMobilePhoneHeader" runat="server" LabelKey="per_licenseList_label_mobilePhone"
                                SortExpression="MobilePhone" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblMobilePhone" IsNeedEncode="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "MobilePhone")%>' />
                            <asp:HiddenField ID="hdnMobilePhoneIDD" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MobilePhoneIDD")%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkFaxHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkFaxHeader" runat="server" LabelKey="per_licenseList_label_faxNumber"
                                SortExpression="Fax" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblFax" IsNeedEncode="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Fax")%>' />
                            <asp:HiddenField ID="hdnFaxIDD" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "FaxIDD")%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkCountryHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkCountryHeader" runat="server" LabelKey="per_licenseList_label_country"
                                SortExpression="Country" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblCountry" runat="server" Text='<%#StandardChoiceUtil.GetCountryByKey(Convert.ToString(DataBinder.Eval(Container.DataItem, "CountryCode")))%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkContractorLicNOHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkContractorLicNOHeader" runat="server" CommandName="Header"
                                SortExpression="ContractorLicNO" LabelKey="per_licenseList_label_contractorLicNO"
                                CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblContractorLicNO" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ContractorLicNO")%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="160px" />
                    <HeaderStyle Width="160px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkContractorBusiNameHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkContractorBusiNameHeader" runat="server" CommandName="Header"
                                SortExpression="ContractorBusiName" LabelKey="per_licenseList_label_contractorbusiname"
                                CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblContractorBusiName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ContractorBusiName")%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="200px" />
                    <HeaderStyle Width="200px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkEmailHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkEmailHeader" runat="server" LabelKey="per_licenselist_label_email"
                                SortExpression="Email" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblEmail" IsNeedEncode="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Email")%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkActionHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:AccelaLabel ID="lnkActionHeader" runat="server" LabelKey="aca_licenselist_label_action" IsGridViewHeadLabel="true"></ACA:AccelaLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div runat="server" id="divAction">
                        <ACA:AccelaLinkButton ID="lnkEdit" LabelKey="aca_licenselist_label_lnkedit" runat="server"
                                CommandName="SelectLicense" CausesValidation="false"
                                CommandArgument='<%#Container.DataItemIndex%>'></ACA:AccelaLinkButton>
                        <span>&nbsp;</span>
                        <ACA:AccelaLinkButton ID="btnDelete" LabelKey="per_contactList_label_delete" runat="server"
                                CommandName="DeleteLicense" CausesValidation="false"
                                CommandArgument='<%#Container.DataItemIndex%>'></ACA:AccelaLinkButton>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
            function <%=EditLicenseFunction %>(licenseSeqNo, licenseNo, licenseType, TemporaryID) {            
                var url = '<%=FileUtil.AppendApplicationRoot("People/LicenseInput.aspx") %>?action=E&isLPList=true&agencyCode=<%=ConfigManager.AgencyCode %>&isSubAgencyCap=<%=IsSubAgencyCap %>&Module=<%=ModuleName %>&componentName=<%=ComponentName %>&parentControlId=<%=ParentControlID %>&isValidate=<%=IsValidate %>&isEditable=<%=IsEditable %>&isSectionRequired=<%=IsSectionRequired %>&licenseSeqNbr=' +licenseSeqNo + '&licenseNbr=' + encodeURIComponent(licenseNo) + '&licenseType=' + licenseType + '&licenseTempID=' + TemporaryID;
                ACADialog.popup({ url: url, width: 800, height: 550, objectTarget : $("#__LASTFOCUS_ID").val()});
                return false;
            }
</script>
