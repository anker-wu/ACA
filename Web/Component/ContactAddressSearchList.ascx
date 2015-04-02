<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactAddressSearchList.ascx.cs" Inherits="Accela.ACA.Web.Component.ContactAddressSearchList" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<div id="divContactAddressList" class="ACA_TabRow" runat="server">
    <ACA:AccelaGridView ID="gdvContactAddressList" runat="server" AllowPaging="true" PagerStyle-VerticalAlign="bottom"
        AllowSorting="True" SummaryKey="aca_contactaddresslist_summary" CaptionKey="aca_caption_contactaddresslist" ShowCaption="true" GridViewNumber="60134"
        CheckBoxColumnIndex="0" AutoGenerateCheckBoxColumn="True"
        AutoGenerateColumns="False" PagerStyle-HorizontalAlign="center" OnRowDataBound="ContactAddressList_RowDataBound"
        OnRowCommand="ContactAddressList_RowCommand" OnGridViewSort="ContactAddressList_GridViewSort" IsInSPEARForm="true" IsAutoWidth="true">
        <Columns>
            <ACA:AccelaTemplateField AttributeName="lnkAddressType">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkAddressType" runat="server" LabelKey="aca_contactaddresslist_label_addresstype"
                            SortExpression="addressType" CommandName="Header" CausesValidation="false" />
                        <ACA:AccelaLabel ID="lblAddressTypeTitle" runat="server" Visible="false" LabelKey="aca_contactaddresslist_label_addresstype"
                            IsGridViewHeadLabel="true" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblAddressType" runat="server" Text='<%#StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_CONTACT_ADDRESS_TYPE, Convert.ToString(DataBinder.Eval(Container.DataItem, "addressType")))%>' />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkRecipient">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkRecipient" runat="server" LabelKey="aca_contactaddresslist_label_recipient"
                            SortExpression="recipient" CausesValidation="false" />
                        <ACA:AccelaLabel ID="lblRecipientTitle" runat="server" Visible="false" LabelKey="aca_contactaddresslist_label_recipient"
                            IsGridViewHeadLabel="true" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblRecipient" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "recipient")%>' />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkAddress">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:AccelaLabel ID="lnkAddress" runat="server" LabelKey="aca_contactaddresslist_label_address"
                            IsGridViewHeadLabel="true" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblAddress" runat="server" />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="400px" />
                <HeaderStyle Width="400px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkStartDate">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkStartDate" runat="server" LabelKey="aca_contactaddresslist_label_startdate"
                            SortExpression="effectiveDate" CommandName="Header" CausesValidation="false" />
                        <ACA:AccelaLabel ID="lblStartDateTitle" runat="server" Visible="false" LabelKey="aca_contactaddresslist_label_startdate"
                            IsGridViewHeadLabel="true" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaDateLabel ID="lblStartDate" DateType="ShortDate" runat="server" Text2='<%# DataBinder.Eval(Container.DataItem, "effectiveDate")%>' />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="80px" />
                <HeaderStyle Width="80px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkEndDate">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkEndDate" runat="server" LabelKey="aca_contactaddresslist_label_enddate"
                            SortExpression="expirationDate" CommandName="Header" CausesValidation="false" />
                        <ACA:AccelaLabel ID="lblEndDatTitle" runat="server" Visible="false" LabelKey="aca_contactaddresslist_label_enddate"
                            IsGridViewHeadLabel="true" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaDateLabel ID="lblEndDate" DateType="ShortDate" runat="server" Text2='<%# DataBinder.Eval(Container.DataItem, "expirationDate")%>' />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="80px" />
                <HeaderStyle Width="80px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkPhone">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkPhone" runat="server" LabelKey="aca_contactaddresslist_label_phone"
                            SortExpression="phone" CommandName="Header" CausesValidation="false" />
                        <ACA:AccelaLabel ID="lblPhoneTitle" runat="server" Visible="false" LabelKey="aca_contactaddresslist_label_phone"
                            IsGridViewHeadLabel="true" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblPhone" IsNeedEncode="false" runat="server" Text='<%# ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, "phoneCountryCode") as string, DataBinder.Eval(Container.DataItem, "phone") as string,  DataBinder.Eval(Container.DataItem, "countryCode") as string) %>' />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="120px" />
                <HeaderStyle Width="120px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkFax">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkFax" runat="server" LabelKey="aca_contactaddresslist_label_fax"
                            SortExpression="fax" CommandName="Header" CausesValidation="false" />
                        <ACA:AccelaLabel ID="lblFaxTitle" runat="server" Visible="false" LabelKey="aca_contactaddresslist_label_fax"
                            IsGridViewHeadLabel="true" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblFax" IsNeedEncode="false" runat="server" Text='<%# ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, "faxCountryCode") as string, DataBinder.Eval(Container.DataItem, "fax") as string,  DataBinder.Eval(Container.DataItem, "countryCode") as string) %>' />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="120px" />
                <HeaderStyle Width="120px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkPrimaryHeader">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkPrimaryHeader" runat="server" LabelKey="aca_contactaddresslist_label_primary"
                            SortExpression="primary" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblPrimary" IsNeedEncode="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "primary") %>' />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkStatusHeader">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkStatusHeader" runat="server" LabelKey="aca_contactaddresslist_label_status"
                            SortExpression="auditModel.auditStatus" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblStatus" IsNeedEncode="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "auditModel.auditStatus") %>' />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkValidatedHeader">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkValidatedHeader" runat="server" LabelKey="aca_contactaddresslist_label_validated"
                            SortExpression="validateFlag" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblValidated" IsNeedEncode="false" runat="server" />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkStreetName">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkStreetName" runat="server" LabelKey="aca_contactaddresslist_label_streetname"
                            SortExpression="streetName" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblStreetName" IsNeedEncode="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "streetName") %>' />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkAddressLine1">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkAddressLine1" runat="server" LabelKey="aca_contactaddresslist_label_addressline1"
                            SortExpression="addressLine1" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblAddressLine1" IsNeedEncode="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "addressLine1") %>' />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkAddressLine2">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkAddressLine2" runat="server" LabelKey="aca_contactaddresslist_label_addressline2"
                            SortExpression="addressLine2" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblAddressLine2" IsNeedEncode="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "addressLine2") %>' />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkAddressLine3">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkAddressLine3" runat="server" LabelKey="aca_contactaddresslist_label_addressline3"
                            SortExpression="addressLine3" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblAddressLine3" IsNeedEncode="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "addressLine3") %>' />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkCountry">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkCountry" runat="server" LabelKey="aca_contactaddresslist_label_country"
                            SortExpression="countryCode" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblCountry" runat="server" Text='<%#StandardChoiceUtil.GetCountryByKey(Convert.ToString(DataBinder.Eval(Container.DataItem, "countryCode")))%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkCity">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkCity" runat="server" LabelKey="aca_contactaddresslist_label_city"
                            SortExpression="city" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblCity" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "city")%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkState">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkState" runat="server" LabelKey="aca_contactaddresslist_label_state"
                            SortExpression="state" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblState" runat="server" Text='<%#I18nUtil.DisplayStateForI18N(DataBinder.Eval(Container.DataItem, "state") as string, DataBinder.Eval(Container.DataItem, "countryCode") as string)%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkZip">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkZip" runat="server" LabelKey="aca_contactaddresslist_label_zip"
                            SortExpression="zip" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblZip" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "zip")%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkHouseAlphaStart">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkHouseAlphaStart" runat="server" LabelKey="aca_contactaddresslist_label_housealphastart"
                            SortExpression="houseNumberAlphaStart" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblHouseAlphaStart" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "houseNumberAlphaStart")%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkHouseAlphaEnd">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkHouseAlphaEnd" runat="server" LabelKey="aca_contactaddresslist_label_housealphaend"
                            SortExpression="houseNumberAlphaEnd" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblHouseAlphaEnd" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "houseNumberAlphaEnd")%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkLevelNbrStart">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkLevelNbrStart" runat="server" LabelKey="aca_contactaddresslist_label_levelnumberstart"
                            SortExpression="levelNumberStart" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblLevelNbrStart" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "levelNumberStart")%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkLevelNbrEnd">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkLevelNbrEnd" runat="server" LabelKey="aca_contactaddresslist_label_levelnumberend"
                            SortExpression="levelNumberEnd" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblLevelNbrEnd" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "levelNumberEnd")%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkLevelPrefix">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkLevelPrefix" runat="server" LabelKey="aca_contactaddresslist_label_levelprefix"
                            SortExpression="levelPrefix" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblLevelPrefix" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "levelPrefix")%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkFullAddress">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkFullAddress" runat="server" LabelKey="aca_contactaddresslist_label_fulladdress"
                            SortExpression="fullAddress" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblFullAddress" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "fullAddress")%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkStreetStart">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkStreetStart" runat="server" LabelKey="aca_contactaddresslist_label_streetstart"
                            SortExpression="houseNumberStart" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblStreetStart" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "houseNumberStart")%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkStreetEnd">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkStreetEnd" runat="server" LabelKey="aca_contactaddresslist_label_streetend"
                            SortExpression="houseNumberEnd" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblStreetEnd" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "houseNumberEnd")%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkStreetDirection">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkStreetDirection" runat="server" LabelKey="aca_contactaddresslist_label_direction"
                            SortExpression="streetDirection" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblStreetDirection" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "streetDirection")%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkPrefix">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkPrefix" runat="server" LabelKey="aca_contactaddresslist_label_prefix"
                            SortExpression="streetPrefix" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblPrefix" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "streetPrefix")%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkStreetType">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkStreetType" runat="server" LabelKey="aca_contactaddresslist_label_streettype"
                            SortExpression="streetSuffix" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblStreetType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "streetSuffix")%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkUnitType">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkUnitType" runat="server" LabelKey="aca_contactaddresslist_label_unittype"
                            SortExpression="unitType" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblUnitType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "unitType")%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkUnitStart">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkUnitStart" runat="server" LabelKey="aca_contactaddresslist_label_unitstart"
                            SortExpression="unitStart" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblUnitStart" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "unitStart")%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkUnitEnd">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkUnitEnd" runat="server" LabelKey="aca_contactaddresslist_label_unitend"
                            SortExpression="unitEnd" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblUnitEnd" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "unitEnd")%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkStreetSuffixDirection">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkStreetSuffixDirection" runat="server" LabelKey="aca_contactaddresslist_label_streetsuffixdirection"
                            SortExpression="streetSuffixDirection" CommandName="Header" CausesValidation="false" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <ACA:AccelaLabel ID="lblStreetSuffixDirection" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "streetSuffixDirection")%>' />
                </ItemTemplate>
                <ItemStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
        </Columns>
    </ACA:AccelaGridView>
    <asp:HiddenField ID="hfRowIndex" runat="server" />
</div>