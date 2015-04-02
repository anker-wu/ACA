<%@ Control Inherits="Accela.ACA.Web.Component.ContactAddressList" Language="C#"
    AutoEventWireup="true" CodeBehind="ContactAddressList.ascx.cs" ClassName="ContactAddressList" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="~/Component/PopupActions.ascx" TagName="PopupActions" TagPrefix="ACA" %>
<script src="<%=FileUtil.AppendApplicationRoot("Scripts/popUpDialog.js") %>" type="text/javascript"></script>
<asp:UpdatePanel ID="pnlContactAdressList" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <ACA:AccelaHeightSeparate ID="sepForContactAddressTitle" runat="server" Height="5" />
        <div id="divAddContactAddress" class="ACA_Link_Text ACA_HyperLink" runat="server">
            <a id="<%=ClientID %>_lnkAddContactAddress" onclick="SetNotAskForSPEAR();" href="javascript:<%=ClientID %>_ToggleEditForm();" class="NotShowLoading">
                <img id="<%=ClientID %>_imgAddContactAddress" class="ACA_NoBorder" src="<%=ImageUtil.GetImageURL("caret_collapsed.gif") %>" alt="<%=GetTextByKey("img_alt_collapse_icon").Replace("'", "\\'") %>" />
                <ACA:AccelaLabel ID="lblAddContactAddress" runat="server" LabelKey="aca_contactaddresslist_label_title" CssClass="ACA_FontSize12 font12px" />
            </a>
        </div>
        <fieldset class="contactaddressform" id="<%=ClientID %>_contactAddressEditForm">
            <div id="divContactAddressList" runat="server" class="ContactAddressListForm">
                <div id="divButtons" runat="server" Visible="False">
                    <ACA:AccelaHeightSeparate ID="sepForSearch" runat="server" Height="5" />
                    <div class="ACA_Row ACA_LiLeft">
                        <ul>
                            <span class="ACA_Error_Indicator" runat="server" id="imgErrorIcon" style="position: relative;
                                margin-top: 4px;" visible="false">
                                <img class="ACA_Error_Indicator_In_Same_Row" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>"
                                    src="<%=ImageUtil.GetImageURL("error_16.gif") %>" />
                            </span>
                            <li id="liSearchBtn" runat="server">
                                <ACA:AccelaButton ID="btnAddContactAddress" runat="server" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                                    DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" LabelKey="aca_contactaddresslist_label_addcontactaddress"
                                    CausesValidation="false">
                                </ACA:AccelaButton>
                            </li>
                        </ul>
                    </div>
                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" runat="server" Height="5" />
                </div>
                <ACA:AccelaLabel ID="lblContactAddressListInstruction" runat="server" LabelKey="aca_contactaddress_label_instruction" CssClass="ACA_Section_Instruction ACA_Section_Instruction_FontSize" />
                <div id="divIncompleteMark" runat="server" visible="false">
                    <table role='presentation' class="message_error_contact"
                        border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="ACA_Message_Icon">
                                <div class="ACA_Error_Icon">
                                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>"
                                        src="<%=ImageUtil.GetImageURL("error_24.gif") %>" />
                                </div>
                            </td>
                            <td class="ACA_XShoter">
                            </td>
                            <td class="ACA_Message_Content">
                                <asp:Literal runat="server" ID="ltScriptForIncomplete" Mode="PassThrough"></asp:Literal>
                                <ACA:AccelaLabel ID="lblIncomplete" runat="server" LabelKey="aca_contactaddresslist_msg_requiredincomplete" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divDuplicateMark" runat="server" visible="false">
                    <table role='presentation' class="message_error_contact" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="ACA_Message_Icon">
                                <div class="ACA_Error_Icon">
                                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>"
                                        src="<%=ImageUtil.GetImageURL("error_24.gif") %>" />
                                </div>
                            </td>
                            <td class="ACA_XShoter">
                            </td>
                            <td class="ACA_Message_Content">
                                <asp:Literal runat="server" ID="Literal1" Mode="PassThrough"></asp:Literal>
                                <ACA:AccelaLabel ID="lblDuplicateContactAddress" runat="server" LabelKey="aca_contactaddress_error_duplicatecontactaddress" />
                            </td>
                        </tr>
                    </table>
                </div>

                <div class="ACA_Error_Icon" runat="server" enableviewstate="false" id="divImgSuccess" visible="false">
                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("img_alt_workflow_complete") %>" src="<%=ImageUtil.GetImageURL("complete.png") %>" />
                </div>
                <div id="<%=ClientID %>_divMessage" class="ACA_Notice font12px">
                    <ACA:AccelaLabel ID="lblActionNoticeAddSuccess" class="Notice_Message_Success" EnableViewState="false" runat="server" LabelKey="aca_contactaddress_msg_addsuccessfully" Visible="false" />
                    <ACA:AccelaLabel ID="lblActionNoticeDeleteSuccess" class="Notice_Message_Success" EnableViewState="false" runat="server" LabelKey="aca_contactaddress_msg_removesuccessfully" Visible="false" />
                    <ACA:AccelaLabel ID="lblActionNoticeUpdateSuccess" class="Notice_Message_Success" EnableViewState="false" runat="server" LabelKey="aca_contactaddress_msg_updatesuccessfully" Visible="false" />
                    <ACA:AccelaLabel ID="lblActionSetPrimarySuccess" class="Notice_Message_Success" EnableViewState="false" runat="server" LabelKey="aca_contactaddress_msg_setprimarysuccessfully" Visible="false" />
                    <ACA:AccelaLabel ID="lblActionDeactivatedSuccess" class="Notice_Message_Success" EnableViewState="false" runat="server" LabelKey="aca_contactaddress_msg_deactivatedsuccessfully" Visible="false" />
                    <ACA:AccelaLabel ID="lblDuplicateContactAddressMessage" class="Notice_Message_Failure" EnableViewState="false" runat="server" LabelKey="aca_contactaddress_msg_duplicate" Visible="false" />
                    <ACA:AccelaLabel ID="lblDuplicateToSelf" class="Notice_Message_Failure" EnableViewState="false" runat="server" LabelKey="aca_contactaddress_msg_duplicatetoself" Visible="false" />
                </div>
                <ACA:AccelaGridView ID="gdvContactAddressList" runat="server" AllowPaging="true"
                    AllowSorting="True" SummaryKey="aca_contactaddresslist_summary" CaptionKey="aca_caption_contactaddresslist" ShowCaption="true"
                    AutoGenerateColumns="False" PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom"
                    OnRowDataBound="ContactAddressList_RowDataBound" OnRowCommand="ContactAddressList_RowCommand"
                    IsInSPEARForm="true" IsAutoWidth="true">
                    <Columns>
                        <ACA:AccelaTemplateField ShowHeader="false">
                            <ItemTemplate>
                                <ACA:AccelaDiv ID="divDuplicatedMark" runat="server" Visible="false">
                                    <img alt="<%=GetTextByKey("aca_contactaddress_error_duplicatecontactaddress") %>" src="<%= ImageUtil.GetImageURL("error_16.gif") %>" />
                                </ACA:AccelaDiv>
                            </ItemTemplate>
                            <ItemStyle CssClass="ACA_VerticalAlign" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField ShowHeader="false">
                            <ItemTemplate>
                                <ACA:AccelaDiv ID="divRequiredMark" runat="server" Visible="false">
                                    <img alt="<%=GetTextByKey("img_alt_mark_required") %>" src="<%= ImageUtil.GetImageURL("error_16.gif") %>" />
                                </ACA:AccelaDiv>
                            </ItemTemplate>
                            <ItemStyle CssClass="ACA_VerticalAlign" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField ColumnId="colSelector">
                            <ItemTemplate>
                                <div>
                                    <ACA:AccelaRadioButton ID="rdoSelector" runat="server" GroupName="SelectValidatedContactAddress" />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="20px" />
                            <HeaderStyle Width="20px" />
                        </ACA:AccelaTemplateField>
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
                                    <ACA:AccelaLinkButton ID="lnkAddress" runat="server" CommandName='<%# COMMAND_SELECT_CONTACT_ADDRESS%>'
                                        CausesValidation="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RowIndex")%>'></ACA:AccelaLinkButton>
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
                        <ACA:AccelaTemplateField AttributeName="lnkActionHeader">
                            <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                    <ACA:AccelaLabel ID="lnkActionHeader" runat="server" LabelKey="aca_contactaddresslist_label_action"
                                        IsGridViewHeadLabel="true" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_FLeft">
                                    <ACA:PopupActions ID="actionMenu" Visible="false" runat="server" />
                                    <ACA:AccelaButton ID="lnkEdit" runat="server" CausesValidation="false" TabIndex="-1"></ACA:AccelaButton>
                                    <ACA:AccelaButton ID="lnkDelete" runat="server" CommandName='<%# COMMAND_DELETE_CONTACT_ADDRESS %>'
                                        OnClick="ContactAddressList_ActionCommand" CausesValidation="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RowIndex")%>' TabIndex="-1"></ACA:AccelaButton>
                                    <ACA:AccelaButton ID="lnkDeactivate" runat="server" CommandName='<%# COMMAND_DEACTIVATE_PRIMARY_CONTACT_ADDRESS %>'
                                        OnClick="ContactAddressList_ActionCommand" CausesValidation="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RowIndex")%>' TabIndex="-1"></ACA:AccelaButton>
                                    <ACA:AccelaButton ID="lnkPrimary" runat="server" CommandName='<%# COMMAND_SET_PRIMARY_CONTACT_ADDRESS %>'
                                        OnClick="ContactAddressList_ActionCommand" CausesValidation="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RowIndex")%>' TabIndex="-1"></ACA:AccelaButton>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <HeaderStyle Width="100px" />
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
                <div id="divPopup" runat="server" class="ACA_Hide PopUpDlg ACA_XLong">
                    <a id='lnkBeginFocusDlg' class="NullBlock NotShowLoading" href='#' onkeydown='OverrideTabKey(event, true, "lnkClose")' title="<%=GetTextByKey("img_alt_form_begin") %>">
                        <img src='<%=ImageUtil.GetImageURL("spacer.gif") %>' class='ACA_NoBorder' alt="" />
                    </a>
                    <div>
                        <div class="ACA_TabRow ACA_Popup_Title">
                            <div class="ACA_AlignRightOrLeft CloseImage ACA_FRight">
                                <ACA:AccelaLinkButton ID="lnkClose" CssClass="NotShowLoading" runat="server" CausesValidation="false"
                                    OnClientClick="deactivatePopUpDialog.cancel();return false;"><img class="ACA_ActionIMG" src='<%=ImageUtil.GetImageURL("closepopup.png") %>' alt="<%=GetTextByKey("aca_common_close") %>" /></ACA:AccelaLinkButton>
                            </div>
                            <ACA:AccelaLabel ID="lblDeactivateDes" runat="server" LabelKey="aca_contactaddress_label_deactivate"></ACA:AccelaLabel>
                        </div>
                        <ACA:AccelaCalendarText ID="catEndDate" LabelKey="aca_contactaddress_label_deactivate_enddate"
                            runat="server"></ACA:AccelaCalendarText>
                    </div>
                    <div>
                        <table class="ACA_Page font11px" role='presentation' cellspacing="0" cellpadding="0" border="0">
                            <tr valign="bottom">
                                <td>
                                    <div class="ACA_LgButton ACA_LgButton_FontSize aca_popup_ok_button">
                                        <ACA:AccelaButton ID="btnDeactivate" LabelKey="aca_deactivatepopup_label_submit" runat="server"
											OnClientClick="SetNotAsk();deactivatePopUpDialog.cancel();" CausesValidation="false"
                                            OnClick="Deactivate_OnClick"></ACA:AccelaButton>
                                    </div>
                                </td>
                                <td class="PopupButtonSpace">&nbsp;</td>
                                <td>
                                    <div class="ACA_LinkButton">
                                        <ACA:AccelaLinkButton ID="lnkCancel" LabelKey="aca_deactivatepopup_label_cancel" runat="server"
											OnClientClick="SetNotAsk();deactivatePopUpDialog.cancel();return false;" CausesValidation="false"></ACA:AccelaLinkButton>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <a id='lnkEndFocusDlg' class="NullBlock NotShowLoading" href='#' onkeydown='OverrideTabKey(event, false, "lnkBeginFocusDlg")' title="<%=GetTextByKey("img_alt_form_end") %>">
                        <img src='<%=ImageUtil.GetImageURL("spacer.gif") %>' class='ACA_NoBorder' alt="" />
                    </a>
                </div>
                <asp:LinkButton ID="btnRefresh" runat="Server" CssClass="ACA_Hide" OnClick="RefreshButton_Click" TabIndex="-1">
                </asp:LinkButton>
                <asp:HiddenField ID="hfRowIndex" runat="server" />
                <asp:HiddenField ID="hfIsForNew" runat="server" />
            </div>
        </fieldset>
        <asp:HiddenField ID="hdfFormStatus" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    var deactivatePopUpDialog;
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    var imgCollapsed = '<%=ImageUtil.GetImageURL("caret_collapsed.gif") %>';
    var imgExpanded = '<%=ImageUtil.GetImageURL("caret_expanded.gif") %>';
    var altCollapsed = '<%=GetTextByKey("img_alt_collapse_icon").Replace("'", "\\'") %>';
    var altExpanded = '<%=GetTextByKey("img_alt_expand_icon").Replace("'", "\\'") %>';
    
    prm.add_pageLoaded(<%=ClientID %>_PageLoaded);
    
    if (typeof (myValidationErrorPanel) != "undefined")
    {
        myValidationErrorPanel.registerIDs4Recheck("<%=btnAddContactAddress.ClientID %>");
    }

    function <%=ClientID %>_PageLoaded(sender, args) {
        if ($.global.isAdmin) {
            //Expand the edit form in Admin.
            <%=ClientID %>_ToggleEditForm(true);
        } else {
            //Restore the previous status.
            <%=ClientID %>_ToggleEditForm(null, true);
        }

        //Prepare the excluded fields for Contact edit from validation.
        <%=ClientID %>_ContactAddressFormControlIDs = [];
        $("input[id^='<%=ClientID %>'],select[id^='<%=ClientID %>']").each(function() {
            <%=ClientID %>_ContactAddressFormControlIDs.push(this.id);
        });

        if ('<%=ContactSectionPosition %>' == '<%=ACAConstant.ContactSectionPosition.RegisterAccountComplete %>' || '<%=ContactSectionPosition %>' == '<%=ACAConstant.ContactSectionPosition.RegisterClerkComplete %>'){
            $('#<%=ClientID %>_contactAddressEditForm').css("width", $('#<%=pnlContactAdressList.ClientID %>').width() - 10);
            $('#<%=divContactAddressList.ClientID %>').css("width", $('#<%=pnlContactAdressList.ClientID %>').width() - 10);
        }
    }

    function <%=ClientID %>_scrollIntoView(ctrlId) {
        // If current page is a popup dialog, need to do scrollIntoView
        if (window.location.href.indexOf("isPopup") > -1 && (ACADialog == null || !ACADialog.isOpened)) {
            scrollIntoView(ctrlId);

            if (typeof (parent.ACADialog) != 'undefined') {
                parent.ACADialog.setFocus();
            }
        } else {
            ACADialog.setFocus();
        }
    }

    function <%=SessionParameterFunctionName %> (contactAddressIndex, processesType, focusId, func) {
        var parameters = {deactivateActionFlag: 'N',targetObjectId: focusId};
        PageMethods.OperationContactAddressSession('<%=ModuleName %>', contactAddressIndex, processesType, '<%=CallbackFunctionName %>', '<%= NeedCreateSession ? SessionParameterString : string.Empty %>', func, null, parameters);
    }
    
    function CallPostBackFunction(uniqueID)
    {
        var p = new ProcessLoading();
        p.showLoading();
        __doPostBack(uniqueID, '');

        return false;
    }

    function RemoveContactAddress(uniqueID) 
    {
        var warnMsg = '<%=GetTextByKey("aca_common_delete_alert_message").Replace("'","\\'") %>';

        if(confirmMsg(warnMsg)) 
        {
            CallPostBackFunction(uniqueID);
        }

        return false;
    }

    function <%=ClientID %>_DeactivateContactAddress(rowIndex, contactAddressID, focusId) 
    {
        $('#<%=hfRowIndex.ClientID %>').val(rowIndex);
        var parameters = {deactivateActionFlag: 'Y', rowIndex: rowIndex, targetObjectId: focusId};
        PageMethods.GetContactAddressUsedInDailyAsPrimary(contactAddressID, '<%=ConfigManager.AgencyCode %>', <%=ClientID %>_callbackDeactivateContactAddress, null, parameters);
        return false;
    }

    function <%=ClientID %>_callbackDeactivateContactAddress(result, parameters)
    {
        if (!result) {
            return;
        }
    
        var json = eval('(' + result + ')');
        SetValueById('<%=catEndDate.ClientID%>',json.AgencyDate);
        
        if(json.IsPrimary == "Y"){
            /*
            Selected contact address is a primary contact address, need to user
             the new contact address to replace the deactivated contact address.
            */
            PageMethods.OperationContactAddressSession('<%=ModuleName %>', parameters.rowIndex, <%=ContactAddressProcessType.Add.ToString("D") %>, '<%=CallbackFunctionName %>', '<%= NeedCreateSession ? SessionParameterString : string.Empty %>', <%= CallBack4CreateSessionFunction%>, null, parameters);
        }
        else
        {
            deactivatePopUpDialog = new popUpDialog($get('<%=divPopup.ClientID %>'), null, null, $get('<%=divContactAddressList.ClientID %>'), parameters.targetObjectId);
            deactivatePopUpDialog.showPopUp();
            $get('<%=divPopup.ClientID %>').focus();
        }

        return false;
    }

    function <%=CallBack4CreateSessionFunction %>(result, parameters) {
        <%=ClientID %>_ClearMessage();
        var url = '<%= Page.ResolveUrl("~/People/ContactAddressAddNew.aspx") %>';
        url += '?<%= ACAConstant.MODULE_NAME + "=" + ModuleName %>';
        url += '&<%= UrlConstant.AgencyCode + "=" + ConfigManager.AgencyCode %>';
        url += '&isDeactivateAction=' + parameters.deactivateActionFlag;
        
        var popWidth = 800;
        var popHeight = 0;

        if(window.location.href.indexOf("isPopup") > -1) {
            popWidth = 780;
            popHeight = 880;
        }
        ACADialog.popup({ url: url, width: popWidth, height: popHeight, objectTarget: parameters.targetObjectId });
        return false;
    }
    
    // Refresh the contact address list after saving the single contact address.
    function <%=CallbackFunctionName %> (isFromAddress, isForNew, focusObjID) {
        if (isForNew != undefined && isForNew) {
            $("#<%=hfIsForNew.ClientID %>").val("1");
        } else {
            $("#<%=hfIsForNew.ClientID %>").val("0");
        }

        if (focusObjID == undefined) {
            focusObjID = '';
        }
        __doPostBack('<%=btnRefresh.UniqueID %>', focusObjID);
    }
    
    function <%=ClientID %>_ClearMessage() {
        $('#<%=divImgSuccess.ClientID %>').hide();
        $('#<%=lblActionNoticeAddSuccess.ClientID %>').hide();
        $('#<%=lblActionNoticeDeleteSuccess.ClientID %>').hide();
        $('#<%=lblActionNoticeUpdateSuccess.ClientID %>').hide();
        $('#<%=lblActionDeactivatedSuccess.ClientID %>').hide();
        $('#<%=lblDuplicateContactAddressMessage.ClientID %>').hide();
        $('#<%=lblDuplicateToSelf.ClientID %>').hide();
    }
    
    /*
    To expand ,collapse the Contact Address Edit form or to restore previous status.
    visible is true - To expand the edit form.
    visible is false - To collapse the edit form.
    visible is null - To toggle the edit form.
    restore is true - the visible parameter will be ignored and to restore previous status of Contact Address Edit form.
    */
    function <%=ClientID %>_ToggleEditForm(visible, restore) {
        var link = [$get('<%=ClientID %>_lnkAddContactAddress'), $get('<%=ClientID %>_lnkEditContactAddress')];
        var image = [$get('<%=ClientID %>_imgAddContactAddress'), $get('<%=ClientID %>_imgEditContactAddress')];
        var label = [$get('<%=lblAddContactAddress.ClientID %>')];
        var formStatus = $get('<%=hdfFormStatus.ClientID %>');

        if (formStatus == null) {
            return;
        }
        
        if(restore) { 
            if(formStatus.value == '0') {
                visible = false;
            }
            else {
                visible = true;
            }
        }

        if(visible!=null) {
            if(visible) {
                $('#<%=ClientID %>_contactAddressEditForm').show();
            }
            else
            {
                $('#<%=ClientID %>_contactAddressEditForm').hide();
            }
        }
        else {
            $('#<%=ClientID %>_contactAddressEditForm').toggle();
            visible = $('#<%=ClientID %>_contactAddressEditForm').is(':visible');
        }

        formStatus.value = visible ? '1' : '0';

        for(var i=0; i < link.length; i++) {
            if(link[i] == null || image[i] == null || label[i] == null) {
                continue;
            }

            if(visible) {
                Expanded(image[i], imgExpanded, altCollapsed);
                AddTitle(link[i], altCollapsed, label[i]);
            }
            else {
                Collapsed(image[i], imgCollapsed, altExpanded);
                AddTitle(link[i], altExpanded, label[i]);
            }
        }
    }
</script>
