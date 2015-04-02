<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.RefLicensedProfessionalList" Codebehind="RefLicensedProfessionalList.ascx.cs" %>

<asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvRefLicenseeList" runat="server" AllowPaging="True" 
            SummaryKey="gdv_reflicenselist_licenselist_summary" CaptionKey="aca_caption_reflicenselist_licenselist"
            AllowSorting="true" AutoGenerateColumns="False" ShowExportLink="true"
            ShowCaption="true" OnGridViewSort="LicenseeList_GridViewSort" 
            PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom" OnRowCommand="LicenseeList_RowCommand"
            OnPageIndexChanging="LicenseeList_PageIndexChanging" OnRowDataBound="LicenseeList_RowDataBound" 
            OnGridViewDownload="RefLicenseeList_GridViewDownload">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkRefLicenseNbrHeader" ExportDataField="StateLicense">
                    <headertemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkRefLicenseNbrHeader" runat="server" CommandName="Header" SortExpression="stateLicense" 
                            LabelKey="aca_licenseelist_license_number"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                     </div>                 
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLinkButton id="lnkLicenseRefNumber" runat ="server" Text='<%# DataBinder.Eval(Container.DataItem,"stateLicense") %>' 
                            CausesValidation="false" CommandName="selectedLicensee" CommandArgument='<%#Container.DataItemIndex%>'></ACA:AccelaLinkButton>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLicenseTypeHeader" ExportDataField="LicenseType">
                    <headertemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkLicenseTypeHeader" runat="server" CommandName="Header" SortExpression="licenseType"
                            LabelKey="aca_licenseelist_license_type" CausesValidation="false"></ACA:GridViewHeaderLabel>
                     </div>                   
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblLicenseType" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "resLicenseType")) %>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkContactTypeHeader" ExportDataField="TypeFlag">
                    <headertemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkContactTypeHeader" runat="server" CommandName="Header" SortExpression="typeFlag" LabelKey="aca_licenseelist_license_contacttype"
                            CausesValidation="false"></ACA:GridViewHeaderLabel>
                    </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblContactType" runat="server" Text='<%# Accela.ACA.Web.Common.DropDownListBindUtil.GetTypeFlagTextByValue(Convert.ToString(DataBinder.Eval(Container.DataItem, "typeFlag"))) %>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkSSNHeader" ExportDataField="MaskedSSN" ExportFormat="SSN">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkSSNHeader" runat="server" CommandName="Header" SortExpression="maskedSsn" 
                                LabelKey="aca_licenseelist_license_ssn"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblSSN" runat="server" Text='<%# Accela.ACA.Common.Util.MaskUtil.FormatSSNShow(Convert.ToString(DataBinder.Eval(Container.DataItem, "socialSecurityNumber"))) %>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkFEINHeader" ExportDataField="Fein" ExportFormat="FEIN">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkFEINHeader" runat="server" CommandName="Header" SortExpression="fein" 
                                LabelKey="aca_licenseelist_license_fein"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblFEIN" runat="server" Text='<%#Accela.ACA.Common.Util.MaskUtil.FormatFEINShow(Convert.ToString(DataBinder.Eval(Container.DataItem, "fein")), StandardChoiceUtil.IsEnableFeinMasking())%>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkBusinessNameHeader" ExportDataField="BusinessName">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkBusinessNameHeader" runat="server" CommandName="Header" SortExpression="businessName"
                                LabelKey="aca_licenseelist_license_businessname" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLinkButton id="lnkBusinessName" runat ="server" Text='<%# DataBinder.Eval(Container.DataItem,"businessName") %>' 
                                CausesValidation="false" CommandName="selectedLicensee" CommandArgument='<%#Container.DataItemIndex%>'></ACA:AccelaLinkButton>                        
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <headerstyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkBusinessLicenseHeader" ExportDataField="BusinessLicense">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkBusinessLicenseHeader" runat="server" CommandName="Header" SortExpression="businessLicense"
                                LabelKey="aca_licenseelist_license_businesslicense" CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblBusinessLicense" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "businessLicense")) %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <headerstyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLicenseFirstNameHeader" ExportDataField="ContactFirstName">
                    <headertemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkLicenseFirstNameHeader" runat="server" CommandName="Header" SortExpression="contactFirstName" 
                            LabelKey="aca_licenseelist_license_firstame"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                    </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblLicenseFirstName" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "contactFirstName")) %>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLicenseMiddleNameHeader" ExportDataField="ContactMiddleName">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkLicenseMiddleNameHeader" runat="server" CommandName="Header" SortExpression="contactMiddleName" 
                                LabelKey="aca_licenseelist_license_middlename"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblLicenseMiddleName" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "contactMiddleName")) %>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLicenseLastNameHeader" ExportDataField="ContactLastName">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkLicenseLastNameHeader" runat="server" CommandName="Header" SortExpression="contactLastName" 
                                LabelKey="aca_licenseelist_license_lastname"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblLicenseLastName" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "contactLastName")) %>'></ACA:AccelaLabel>
                        </div>  
                    </itemtemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddressHeader" ExportDataField="FullAddress">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkAddressHeader" runat="server" SortExpression="address1" LabelKey="aca_licenseelist_license_address" ></ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAddress" runat="server" Text='<%#Convert.ToString(DataBinder.Eval(Container.DataItem, "address1"))%>' IsNeedEncode="false" />
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
               </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLicenseExpirationDateHeader" ExportDataField="LicenseExpirationDate" ExportFormat="ShortDate">
                    <headertemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkLicenseExpirationDateHeader" runat="server" SortExpression="licenseExpirationDate" 
                                LabelKey="aca_licenseelist_license_expirationdate" ></ACA:GridViewHeaderLabel> 
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaDateLabel ID="lblLicExpDate" DateType="ShortDate" runat="server" Text2='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "licenseExpirationDate"))%>' ></ACA:AccelaDateLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkInsuranceExpirationDateHeader" ExportDataField="InsuranceExpDate" ExportFormat="ShortDate">
                    <headertemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkInsuranceExpirationDateHeader" runat="server" SortExpression="insuranceExpDate" 
                                LabelKey="aca_licenseelist_license_insuranceexpirationdate" ></ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                    <div>
                        <ACA:AccelaDateLabel ID="lblLicInsuranceExpirationDate" DateType="ShortDate" runat="server" Text2='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "insuranceExpDate"))%>'></ACA:AccelaDateLabel>
                    </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLicensingBoardHeader" ExportDataField="LicenseBoard">
                    <headertemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkLicensingBoardHeader" runat="server" SortExpression="licenseBoard" LabelKey="aca_licenseelist_license_licensingboard" ></ACA:GridViewHeaderLabel> 
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                             <ACA:AccelaLabel ID="lblLicensingBoard" runat="server" Text='<%#Convert.ToString(DataBinder.Eval(Container.DataItem, "licenseBoard"))%>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLicenseIssueDateHeader" ExportDataField="LicenseIssueDate">
                <headertemplate>
                <div>
                    <ACA:GridViewHeaderLabel ID="lnkLicenseIssueDateHeader" runat="server" SortExpression="licenseIssueDate" 
                        LabelKey="aca_licenseelist_license_issuedate" ></ACA:GridViewHeaderLabel>   
                </div>
                </headertemplate>
                <itemtemplate>
                    <div>
                        <ACA:AccelaDateLabel ID="lblLicIssueDate" DateType="ShortDate" runat="server" Text2='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "licenseIssueDate"))%>'></ACA:AccelaDateLabel>
                    </div>
                </itemtemplate>
                <ItemStyle Width="100px"/>
                <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLicenseLastRenewalDateHeader" ExportDataField="LicenseLastRenewalDate">
                    <headertemplate>
                    <div>
                        <ACA:GridViewHeaderLabel ID="lnkLicenseLastRenewalDateHeader" runat="server" SortExpression="licenseLastRenewalDate" 
                            LabelKey="aca_licenseelist_license_renewaldate" ></ACA:GridViewHeaderLabel>  
                    </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaDateLabel ID="lblLicRenewalDate" DateType="ShortDate" runat="server" Text2='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "licenseLastRenewalDate"))%>'></ACA:AccelaDateLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkCityHeader" ExportDataField="City">
                    <headertemplate>
                    <div>
                        <ACA:GridViewHeaderLabel ID="lnkCityHeader" runat="server" SortExpression="city" LabelKey="aca_licenseelist_label_city" ></ACA:GridViewHeaderLabel>  
                    </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblCity" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "city"))%>'>
                            </ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkStateHeader" ExportDataField="State">
                    <headertemplate>
                    <div>
                        <ACA:GridViewHeaderLabel ID="lnkStateHeader" runat="server" SortExpression="state" LabelKey="aca_licenseelist_label_state" ></ACA:GridViewHeaderLabel>  
                    </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblState" runat="server" Text='<%# I18nUtil.DisplayStateForI18N(Convert.ToString(DataBinder.Eval(Container.DataItem, "state")), Convert.ToString(DataBinder.Eval(Container.DataItem, "countryCode")))%>'>
                            </ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkZipHeader" ExportDataField="Zip">
                    <headertemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkZipHeader" runat="server" SortExpression="zip" LabelKey="aca_licenseelist_label_zip" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblZip" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "zip"))%>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddress1Header" ExportDataField="Address1">
                    <headertemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkAddress1Header" runat="server" SortExpression="address1" LabelKey="aca_licenseelist_label_address1" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAddress1" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "address1"))%>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddress2Header" ExportDataField="Address2">
                    <headertemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkAddress2Header" runat="server" SortExpression="address2" LabelKey="aca_licenseelist_label_address2" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAddress2" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "address2"))%>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddress3Header" ExportDataField="Address3">
                    <headertemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkAddress3Header" runat="server" SortExpression="address3" LabelKey="aca_licenseelist_label_address3" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAddress3" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "address3"))%>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
               <%--BusiName2--%>
                <ACA:AccelaTemplateField AttributeName="lnkBusiName2Header" ExportDataField="BusName2">
                    <headertemplate>
                    <div>
                        <ACA:GridViewHeaderLabel ID="lnkBusiName2Header" runat="server" SortExpression="busName2" LabelKey="aca_licenseelist_label_businame2" ></ACA:GridViewHeaderLabel>  
                    </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblBusiName2" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "busName2"))%>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <%--Title--%>
                <ACA:AccelaTemplateField AttributeName="lnkTitleHeader" ExportDataField="Title">
                    <headertemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkTitleHeader" runat="server" SortExpression="title" LabelKey="aca_licenseelist_label_title" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblTitle" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "title"))%>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <%--InsurancePolicy--%>
                <ACA:AccelaTemplateField AttributeName="lnkInsurancePolicyHeader" ExportDataField="Policy">
                    <headertemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkInsurancePolicyHeader" runat="server" SortExpression="policy" LabelKey="aca_licenseelist_label_insurancepolicy" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblInsurancePolicy" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "policy"))%>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <%--InsuranceCompany--%>
                <ACA:AccelaTemplateField AttributeName="lnkInsuranceCompanyHeader" ExportDataField="InsuranceCo">
                    <headertemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkInsuranceCompanyHeader" runat="server" SortExpression="insuranceCo" LabelKey="aca_licenseelist_label_insurancecompany" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblInsuranceCompany" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "insuranceCo"))%>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkCountryHeader" ExportDataField="countryCode">
                    <headertemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkCountryHeader" runat="server" SortExpression="countryCode" LabelKey="aca_licenseelist_label_country" ></ACA:GridViewHeaderLabel>  
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblCountry" runat="server" Text='<%# StandardChoiceUtil.GetCountryByKey(Convert.ToString(DataBinder.Eval(Container.DataItem, "countryCode")))%>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
    </ContentTemplate>
</asp:UpdatePanel>
<br />
