<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Default.master"
    ValidateRequest="false" Inherits="Accela.ACA.Web.Account.RegisterLicense" Codebehind="RegisterLicense.aspx.cs" %>
    
<%@ import namespace="Accela.ACA.Common.Util" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <div class="ACA_RightContent">
        <div id="divConfirmLicense" runat="server" visible="false">
            <h1>
                <ACA:AccelaLabel ID="lblConfirmLicenseTitle" LabelKey="lic_searchResult_label_confirmLicense"
                    runat="server" />
            </h1>
            <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" runat="server" Height="10" />
            <ACA:AccelaLabel ID="lblConfirmLicenseIntroduce" LabelKey="lic_searchResult_text_introduce"
                runat="server" LabelType="bodyText" />
            <h1>
                <ACA:AccelaLabel ID="lblConfirmLicenseResult" LabelKey="lic_searchResult_label_result"
                    runat="server" />
            </h1>
            <ACA:AccelaHeightSeparate ID="sepForTitle" runat="server" Height="5" />
            <div class="ACA_TabRow">
                <asp:DataList ID="RepLicenseList" CssClass="ACA_TabRow" runat="server" OnItemDataBound="Repeater1_ItemDataBound" role="presentation">
                    <HeaderTemplate>
                        <div class="ACA_TabTitle ACA_BkTit">
                            <p>
                                <ACA:AccelaLabel ID="lblSearchResultLicenseTile" LabelKey="lic_searchResult_label_license"
                                    runat="server" />
                            </p>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_TabRow">
                            <div class="ACA_ConfigInfo ACA_FLeft ACA_Page ACA_Page_FontSize ACA_DivMargin6 ACA_RegisterLicense_Lookup">
                                        <!-- start dynamic content -->
                                        <table role='presentation'>
                                            <tr>
                                                <td colspan="2">
                                                    <strong>
                                                        <%# DataBinder.Eval(Container.DataItem, "contactFirstName")%>
                                                        &nbsp;
                                                        <%# DataBinder.Eval(Container.DataItem, "contactLastName")%>
                                                    </strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <br>
                                                   <%# DataBinder.Eval(Container.DataItem, "businessName")%><%# DataBinder.Eval(Container.DataItem, "busName2","/{0}")%> 
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <%# DataBinder.Eval(Container.DataItem, "address1")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <%# DataBinder.Eval(Container.DataItem, "city")%>
                                                    <%# I18nUtil.DisplayStateForI18N(DataBinder.Eval(Container.DataItem, "state") as string, DataBinder.Eval(Container.DataItem, "countryCode") as string)%>
                                                    <%# ModelUIFormat.FormatZipShow(DataBinder.Eval(Container.DataItem, "zip") as string, DataBinder.Eval(Container.DataItem, "countryCode") as string)%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <%# StandardChoiceUtil.GetCountryByKey(DataBinder.Eval(Container.DataItem, "countryCode") as string) %>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <ACA:AccelaLabel ID="lblTel" LabelKey="lic_searchResult_label_tel" runat="server" />
                                                </td>
                                                <td>
                                                    <%# ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, "phone1CountryCode") as string, DataBinder.Eval(Container.DataItem, "phone1") as string, DataBinder.Eval(Container.DataItem, "countryCode") as string) as string%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <ACA:AccelaLabel ID="lblFax" LabelKey="lic_searchResult_label_fax" runat="server" />
                                                </td>
                                                <td>
                                                    <%# ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, "faxCountryCode") as string, DataBinder.Eval(Container.DataItem, "fax") as string, DataBinder.Eval(Container.DataItem, "countryCode") as string)%>
                                                </td>
                                            </tr>
                                        </table>
                                        <!-- end dynamic content -->
                            </div>
                            <div class="ACA_ConfigInfo ACA_FLeft ACA_Page ACA_Page_FontSize ACA_RegisterLicense_Lookup">
                                <ul>
                                        <table role='presentation'>
                                            <tr>
                                                <td>
                                                    <strong>
                                                        <%# I18nUtil.DisplayStateForI18N(DataBinder.Eval(Container.DataItem, "licState") as string, DataBinder.Eval(Container.DataItem, "countryCode") as string)%>
                                                        &nbsp;
                                                        <%# I18nStringUtil.GetString(DataBinder.Eval(Container.DataItem, "resLicenseType") as string, DataBinder.Eval(Container.DataItem, "licenseType") as string)%>
                                                    </strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <%# I18nUtil.DisplayStateForI18N(DataBinder.Eval(Container.DataItem, "licState") as string, DataBinder.Eval(Container.DataItem, "countryCode") as string)%>
                                                    -
                                                    <%# DataBinder.Eval(Container.DataItem, "stateLicense")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <ACA:AccelaLabel ID="lblIssued" LabelKey="lic_searchResult_label_issued" runat="server" />
                                                    <span dir="ltr">
                                                    <ACA:AccelaDateLabel id="lblIssuedDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "licenseIssueDate")%>'></ACA:AccelaDateLabel>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <ACA:AccelaLabel ID="lblExpires" LabelKey="lic_searchResult_label_expires" runat="server" />
                                                    <span dir="ltr">
                                                    <ACA:AccelaDateLabel id="lblExpiresDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "licenseExpirationDate")%>'></ACA:AccelaDateLabel>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <ACA:AccelaLabel ID="lblInsureMax" LabelKey="lic_searchResult_label_InsuredMax" runat="server" />
                                                </td>
                                                <td>
                                                    <%# I18nNumberUtil.FormatMoneyForUI(DataBinder.Eval(Container.DataItem, "insuranceAmount"))%>
                                                </td>
                                            </tr>
                                        </table>
                                    
                                </ul>
                            </div>
                            <div class="ACA_ConfigInfo ACA_FLeft ACA_Page ACA_Page_FontSize_Restore">
                                <ACA:AccelaButton ID="removeLicenseItem" DivEnableCss="ACA_SmButton ACA_SmButton_FontSize" LabelKey="lic_searchResult_label_remove"
                                    runat="server" CausesValidation="false" OnClick="RemoveLicenseItemButton_OnClick">
                                </ACA:AccelaButton>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" runat="server" Height="5" />
                <div class="ACA_Page ACA_Page_FontSize">
                    <ACA:AccelaLabel ID="lblOperatorLicenseExplain" LabelKey="acc_reg_text_operatorLicense"
                        runat="server" LabelType="bodyText" />
                </div>
                <div class="ACA_Line_Content ACA_FullWidthTable">
                    &nbsp;</div>
            </div>
            <div class="ACA_Page ACA_Page_FontSize">
                <ACA:AccelaLabel ID="lblHaveLicenseExplain" LabelKey="acc_reg_text_havaLicense" runat="server"
                    LabelType="bodyText" />
            </div>
            <ACA:AccelaHeightSeparate ID="sepForRegLicButton" runat="server" Height="25" />
            <table role='presentation' class="ACA_DivRow" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <table role='presentation' class="ACA_MLong">
                            <tr>
                                <td>
                                    <ACA:AccelaButton ID="btnNextAccountInfo" LabelKey="acc_reg_label_continueReg" runat="server"
                                        OnClick="NextAccountInfoButton_OnClick" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" />
                                </td>
                            </tr>
                            <tr>
                                <td class="ACA_SmLabel ACA_SmLabel_FontSize">
                                    <ACA:AccelaLabel ID="lblNextAccountInfo" LabelKey="acc_reg_label_addLicense" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table role='presentation' class="ACA_MLong">
                            <tr>
                                <td>
                                    <ACA:AccelaButton ID="btnFindAntherLicense" LabelKey="acc_reg_label_addAnotherLicense"
                                        runat="server" OnClick="FindAnotherLicenseButton_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" />
                                </td>
                            </tr>
                            <tr>
                                <td class="ACA_SmLabel ACA_SmLabel_FontSize">
                                    <ACA:AccelaLabel ID="lblFindAntherLicense" LabelKey="acc_reg_label_moreLicense" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divLicenseDisclaimer" runat="server">
            <div class="RegisterLicense_Title">
                <ACA:AccelaLabel ID="lblStepInfo" runat="server" LabelKey="aca_selectAccountType_label_step" />
                    <div class="ACA_Row">
                        <ACA:AccelaLabel ID="lblEnterLicenseTitle" LabelKey="acc_reg_label_enterLicense" runat="server" />
                    </div>
                <ACA:AccelaHeightSeparate ID="sepForTile" runat="server" Height="10" />
            </div>
            <ACA:AccelaLabel ID="lblSearchLicenseDisclaimer" LabelKey="acc_reg_text_searchLicenseDisclaimer"
                runat="server" LabelType="bodyText" />
            <ACA:AccelaHeightSeparate ID="sepForIndicate" runat="server" Height="10" />
            <div class="ACA_TabRow">
                <p class="ACA_FRight">
                    <span class="ACA_Required_Indicator">*</span>
                    <ACA:AccelaLabel ID="lblIndicate" LabelKey="acc_reg_label_indicate" runat="server" /></p>
            </div>
            <ACA:AccelaLabel ID="lblSearchLicenseTitle" LabelKey="lic_licenseSearch_label_licenseInfo"
                runat="server" LabelType="SectionTitle" />
            <div class="ACA_TabRow ACA_LiLeft">
                <ul>
                    <li>
                        <ACA:AccelaDropDownList ID="ddlLicenseType" LabelKey="lic_licenseSearch_label_licenseType"
                            runat="server" Required="true" /></li>
                    <li>
                        <ACA:AccelaTextBox ID="txtLicenseNum" CssClass="ACA_NLong" runat="server" LabelKey="lic_licenseSearch_label_stateLicenseNum"
                            MaxLength="20" Validate="required;maxlength" /></li>
                </ul>
            </div> 
            <ACA:AccelaHeightSeparate ID="sepForButton" runat="server" Height="25" />
            <ACA:AccelaButton ID="lbkFindLicense" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" LabelKey="acc_reg_label_findLicense" runat="server"
                OnClick="FindLicenseButton_Click" />
        </div>
    </div>
</asp:Content>
