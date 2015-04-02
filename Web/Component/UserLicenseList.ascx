<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.UserLicenseList"
    CodeBehind="UserLicenseList.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Web.Common" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Util" %>

<%@ Register Src="PopupActions.ascx" TagName="PopupActions" TagPrefix="ACA" %>
<ACA:AccelaGridView ID="gdvLicenseList" runat="server" AllowPaging="true" AllowSorting="True" IsAutoWidth="true"
    SummaryKey="aca_accountmanager_licenselist_msg_summary" CaptionKey="aca_caption_accountmanager_licenselist" ShowCaption="true" AutoGenerateColumns="False"
    GridViewNumber="60139" PagerStyle-HorizontalAlign="center" PageSize="10" OnRowDataBound="LicenseList_RowDataBound"
    OnPageIndexChanging="LicenseList_PageIndexChanging" OnGridViewSort="LicenseList_GridViewSort">
    <Columns>
        <ACA:AccelaTemplateField AttributeName="lnkLicenseTypeHeader">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkLicenseTypeHeader" runat="server" SortExpression="license.licenseType"
                        LabelKey="aca_accountmanager_licenselist_label_licensetype" CausesValidation="false"></ACA:GridViewHeaderLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblLicenseType" runat="server" Text='<%# I18nStringUtil.GetString(DataBinder.Eval(Container.DataItem, "license.resLicenseType") as string, DataBinder.Eval(Container.DataItem, "license.licenseType") as string)%>'></ACA:AccelaLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="100px" />
            <HeaderStyle Width="100px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkLicenseNumberHeader">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkLicenseNumberHeader" runat="server" SortExpression="license.stateLicense"
                        LabelKey="aca_accountmanager_licenselist_label_statelicense" CausesValidation="false"></ACA:GridViewHeaderLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lnkLicenseNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "license.stateLicense")%>'></ACA:AccelaLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="130px" />
            <HeaderStyle Width="130px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkIssuedDateHeader">
            <HeaderTemplate>
                <div>
                    <ACA:GridViewHeaderLabel ID="lnkIssuedDateHeader" runat="server" SortExpression="license.licenseIssueDate"
                        LabelKey="aca_accountmanager_licenselist_label_issueddate"></ACA:GridViewHeaderLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaDateLabel ID="lblIssuedDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "license.licenseIssueDate")%>'></ACA:AccelaDateLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="100px" />
            <HeaderStyle Width="100px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkExpirationDateHeader">
            <HeaderTemplate>
                <div>
                    <ACA:GridViewHeaderLabel ID="lnkExpirationDateHeader" runat="server" SortExpression="license.licenseExpirationDate"
                        LabelKey="aca_accountmanager_licenselist_label_expirationdate"></ACA:GridViewHeaderLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaDateLabel ID="lblLicenseExpirationDate" runat="server" DateType="ShortDate"
                        Text2='<%# DataBinder.Eval(Container.DataItem, "license.licenseExpirationDate")%>'></ACA:AccelaDateLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="100px" />
            <HeaderStyle Width="100px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkStatusHeader">
            <HeaderTemplate>
                <div>
                    <ACA:GridViewHeaderLabel ID="lnkStatusHeader" runat="server" SortExpression="status"
                        LabelKey="aca_accountmanager_licenselist_label_status"></ACA:GridViewHeaderLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblStatus" runat="server" Text='<%# GetStatusForI18NDisplay(DataBinder.Eval(Container.DataItem, "status"))%>'></ACA:AccelaLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="100px" />
            <HeaderStyle Width="100px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkFirstNameHeader">
            <HeaderTemplate>
                <div>
                    <ACA:GridViewHeaderLabel ID="lnkFirstNameHeader" runat="server" SortExpression="license.contactFirstName"
                        LabelKey="aca_accountmanager_licenselist_label_firstname"></ACA:GridViewHeaderLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblFirstName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "license.contactFirstName")%>'></ACA:AccelaLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="80px" />
            <HeaderStyle Width="80px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkLastNameHeader">
            <HeaderTemplate>
                <div>
                    <ACA:GridViewHeaderLabel ID="lnkLastNameHeader" runat="server" SortExpression="license.contactLastName"
                        LabelKey="aca_accountmanager_licenselist_label_lastname"></ACA:GridViewHeaderLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblLastName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "license.contactLastName")%>'></ACA:AccelaLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="80px" />
            <HeaderStyle Width="80px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkBusinessNameHeader">
            <HeaderTemplate>
                <div>
                    <ACA:GridViewHeaderLabel ID="lnkBusinessNameHeader" runat="server" SortExpression="license.businessName"
                        LabelKey="aca_accountmanager_licenselist_label_businessname"></ACA:GridViewHeaderLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblBusinessName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "license.businessName")%>'></ACA:AccelaLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="80px" />
            <HeaderStyle Width="80px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkBusinessName2Header">
            <HeaderTemplate>
                <div>
                    <ACA:GridViewHeaderLabel ID="lnkBusinessName2Header" runat="server" SortExpression="license.busName2"
                        LabelKey="aca_accountmanager_licenselist_label_businessname2"></ACA:GridViewHeaderLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblBusinessName2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "license.busName2")%>'></ACA:AccelaLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="80px" />
            <HeaderStyle Width="80px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkBusinessLicenseHeader">
            <HeaderTemplate>
                <div>
                    <ACA:GridViewHeaderLabel ID="lnkBusinessLicenseHeader" runat="server" SortExpression="license.businessLicense"
                        LabelKey="aca_accountmanager_licenselist_label_businesslicensenumber"></ACA:GridViewHeaderLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblBusinessLicense" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "license.businessLicense")%>'></ACA:AccelaLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="80px" />
            <HeaderStyle Width="80px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkAddress1Header">
            <HeaderTemplate>
                <div>
                    <ACA:GridViewHeaderLabel ID="lnkAddress1Header" runat="server" SortExpression="license.address1"
                        LabelKey="aca_accountmanager_licenselist_label_address1"></ACA:GridViewHeaderLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblAddress1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "license.address1")%>'></ACA:AccelaLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="80px" />
            <HeaderStyle Width="80px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkAddress2Header">
            <HeaderTemplate>
                <div>
                    <ACA:GridViewHeaderLabel ID="lnkAddress2Header" runat="server" SortExpression="license.address2"
                        LabelKey="aca_accountmanager_licenselist_label_address2"></ACA:GridViewHeaderLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblAddress2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "license.address2")%>'></ACA:AccelaLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="80px" />
            <HeaderStyle Width="80px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkAddress3Header">
            <HeaderTemplate>
                <div>
                    <ACA:GridViewHeaderLabel ID="lnkAddress3Header" runat="server" SortExpression="license.address3"
                        LabelKey="aca_accountmanager_licenselist_label_address3"></ACA:GridViewHeaderLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblAddress3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "license.address3")%>'></ACA:AccelaLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="80px" />
            <HeaderStyle Width="80px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkCityHeader">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkCityHeader" runat="server" LabelKey="aca_accountmanager_licenselist_label_city"
                        SortExpression="license.city" CausesValidation="false" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "license.city")%>' />
                </div>
            </ItemTemplate>
            <ItemStyle Width="80px" />
            <HeaderStyle Width="80px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkStateHeader">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkStateHeader" runat="server" LabelKey="aca_accountmanager_licenselist_label_state"
                        SortExpression="license.state" CausesValidation="false" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <ACA:AccelaLabel ID="lblState" runat="server" Text='<%# I18nUtil.DisplayStateForI18N(Convert.ToString(DataBinder.Eval(Container.DataItem, "license.state")), DataBinder.Eval(Container.DataItem, "license.countryCode") as string)%>' />
            </ItemTemplate>
            <ItemStyle Width="80px" />
            <HeaderStyle Width="80px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkZipCodeHeader">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkZipCodeHeader" runat="server" LabelKey="aca_accountmanager_licenselist_label_zip"
                        SortExpression="license.zip" CausesValidation="false" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblZip" runat="server" Text='<%# ModelUIFormat.FormatZipShow(DataBinder.Eval(Container.DataItem, "license.zip") as string, DataBinder.Eval(Container.DataItem, "license.countryCode") as string)%>' />
                </div>
            </ItemTemplate>
            <ItemStyle Width="80px" />
            <HeaderStyle Width="80px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkHomePhoneHeader">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkHomePhoneHeader" runat="server" LabelKey="aca_accountmanager_licenselist_label_homephone"
                        SortExpression="license.phone1" CausesValidation="false" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblPhone" IsNeedEncode="false" runat="server" Text='<%# ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, "license.phone1CountryCode") as string, DataBinder.Eval(Container.DataItem, "license.phone1") as string, DataBinder.Eval(Container.DataItem, "license.countryCode") as string)%>' />
                </div>
            </ItemTemplate>
            <ItemStyle Width="100px" />
            <HeaderStyle Width="100px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkMobilePhoneHeader">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkMobilePhoneHeader" runat="server" LabelKey="aca_accountmanager_licenselist_label_mobilephone"
                        SortExpression="license.phone2" CausesValidation="false" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblMobilePhone" IsNeedEncode="false" runat="server" Text='<%# ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, "license.phone2CountryCode") as string, DataBinder.Eval(Container.DataItem,"license.phone2") as string, DataBinder.Eval(Container.DataItem, "license.countryCode") as string)%>' />
                </div>
            </ItemTemplate>
            <ItemStyle Width="100px" />
            <HeaderStyle Width="100px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkFaxHeader">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkFaxHeader" runat="server" LabelKey="aca_accountmanager_licenselist_label_fax"
                        SortExpression="license.fax" CausesValidation="false" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblFax" IsNeedEncode="false" runat="server" Text='<%# ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, "license.faxCountryCode") as string, DataBinder.Eval(Container.DataItem, "license.fax") as string, DataBinder.Eval(Container.DataItem, "license.countryCode") as string)%>' />
                </div>
            </ItemTemplate>
            <ItemStyle Width="100px" />
            <HeaderStyle Width="100px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkCountryHeader">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkCountryHeader" runat="server" LabelKey="aca_accountmanager_licenselist_label_country"
                        SortExpression="license.countryCode" CausesValidation="false" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblCountry" IsNeedEncode="false" runat="server" Text='<%# StandardChoiceUtil.GetCountryByKey(DataBinder.Eval(Container.DataItem, "license.countryCode") as string)%>' />
                </div>
            </ItemTemplate>
            <ItemStyle Width="100px" />
            <HeaderStyle Width="100px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkInsuredMaxHeader">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkInsuredMaxHeader" runat="server" LabelKey="aca_accountmanager_licenselist_label_insuredmax"
                        SortExpression="license.insuranceAmount" CausesValidation="false" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblInsuredMax" runat="server" Text='<%# I18nNumberUtil.FormatMoneyForUI(DataBinder.Eval(Container.DataItem, "license.insuranceAmount"))%>' />
                </div>
            </ItemTemplate>
            <ItemStyle Width="80px" />
            <HeaderStyle Width="80px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkContractorLicNOHeader">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkContractorLicNOHeader" runat="server" SortExpression="license.contrLicNo"
                        LabelKey="aca_accountmanager_licenselist_label_contractorlicensenumber" CausesValidation="false"></ACA:GridViewHeaderLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblContractorLicNO" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "license.contrLicNo")%>' />
                </div>
            </ItemTemplate>
            <ItemStyle Width="160px" />
            <HeaderStyle Width="160px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkContractorBusiNameHeader">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkContractorBusiNameHeader" runat="server" SortExpression="license.contLicBusName"
                        LabelKey="aca_accountmanager_licenselist_label_contractorbusiname" CausesValidation="false"></ACA:GridViewHeaderLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblContractorBusiName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "license.contLicBusName")%>' />
                </div>
            </ItemTemplate>
            <ItemStyle Width="200px" />
            <HeaderStyle Width="200px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lblActionHeader">
            <HeaderTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblActionHeader" runat="server" LabelKey="aca_accountmanager_licenselist_label_action"
                        IsGridViewHeadLabel="true"></ACA:AccelaLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="ACA_FLeft">
                    <ACA:PopupActions ID="actionMenu" Visible="false" runat="server" />
                    <asp:HiddenField ID="hdnLicSeqNbr" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "license.licSeqNbr")%>' />
                    <asp:HiddenField ID="hdnLicType" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "license.licenseType")%>' />
                    <ACA:AccelaButton ID="btnRemoveLicenseItem" OnClick="RemoveLicenseItemLink_OnClick"
                        runat="server" CausesValidation="false" TabIndex="-1"/>
                </div>
            </ItemTemplate>
            <ItemStyle Width="80px" />
            <HeaderStyle Width="80px" />
        </ACA:AccelaTemplateField>
    </Columns>
</ACA:AccelaGridView>
<script type="text/javascript">
    function ViewUserLicenseDetails(agencyCode, licenseType, licenseSeqNbr, userSeqNum, focusObjectID) 
    {
        SetNotAskForSPEAR();
        var url = '<%=FileUtil.AppendApplicationRoot("Account/UserLicenseDetail.aspx") %>?<%=UrlConstant.AgencyCode %>=' + agencyCode + '&licenseType=' 
                + licenseType + '&licenseSeqNbr=' + licenseSeqNbr + '&<%=UrlConstant.USER_SEQ_NUM %>=' + userSeqNum;
        ACADialog.popup({ url: url, width: 700, height: 280, objectTarget: $get(focusObjectID) });

        return false;
    }

    function RemoveLicense(uniqueID) 
    {
        var warnMsg = '<%=GetTextByKey("acc_message_confirm_removeLicense").Replace("'","\\'") %>';

        if(confirmMsg(warnMsg)) 
        {
            var p = new ProcessLoading();
            p.showLoading();
            __doPostBack(uniqueID, '');
        }

        return false;
    }
</script>