<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RefContactContinuingEducationList.ascx.cs" Inherits="Accela.ACA.Web.Component.RefContactContinuingEducationList" %>

<asp:UpdatePanel ID="listPanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <div id="divContEduactionList" style="float:left;">
            <ACA:AccelaGridView ID="gdvContEducationList" GridViewNumber="60083" runat="server" AllowPaging="False" AutoGenerateColumns="False" 
                SummaryKey="gdv_conteducation_conteducationlist_summary" CaptionKey="aca_caption_refeducationlist_educationlist" IsInSPEARForm="true" ShowCaption="true" AutoGenerateCheckBoxColumn="True"
                AllowSorting="true" OnRowDataBound="ContEducationList_RowDataBound"
                PagerStyle-HorizontalAlign="center" PagerStyle-CssClass="ACA_Table_Pages ACA_Table_Pages_FontSize" PagerStyle-VerticalAlign="bottom">
                <Columns>
                    <ACA:AccelaTemplateField ShowHeader="False">
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <asp:HiddenField ID="hfContEduNbr" runat="server"/>
                            </div>
                        </ItemTemplate>
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField ShowHeader="false">
                        <ItemTemplate>
                            <div>
                                <ACA:AccelaDiv ID="divLogo" runat="server">
                                    <a id="linkExpandContEduComment<%#Container.DataItemIndex%>" class="NotShowLoading" href="javascript:expandContEduComment('<%#Container.DataItemIndex%>')"
                                        title="<%=GetTitleByKey("img_alt_expand_icon","per_conteducationlist_display_comments") %>">
                                        <img id="imgExpandContEduComment<%#Container.DataItemIndex%>" alt="<%=GetTextByKey("img_alt_expand_icon") %>"
                                            src="<%= ImageUtil.GetImageURL("caret_collapsed.gif") %>" style="cursor: pointer; border-width: 0px" />
                                    </a>
                                </ACA:AccelaDiv>
                            </div>
                        </ItemTemplate>
                        <ItemStyle CssClass="ACA_VerticalAlign" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkContEducationName">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkContEducationName" runat="server" SortExpression="contEduName"
                                    LabelKey="per_conteducationlist_name" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblContEducationName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "contEduName") %>'>
                                </ACA:AccelaLabel>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="160px" />
                        <headerstyle Width="160px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkRequiredOptional">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkRequiredOptional" runat="server" SortExpression="requiredFlag"
                                    LabelKey="per_conteducationlist_required_option" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblRequired" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "requiredFlag") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="50px" />
                        <headerstyle Width="50px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkProviderName">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkProviderName" runat="server" SortExpression="providerName"
                                    LabelKey="per_conteducationlist_provider_name" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblProviderName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "providerName") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="130px" />
                        <headerstyle Width="130px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkProviderNumber">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkProviderNumber" runat="server" SortExpression="providerNo"
                                    LabelKey="per_conteducationlist_provider_number" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblProviderNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "providerNo") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkClass">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkClass" runat="server" SortExpression="className"
                                    LabelKey="per_conteducationlist_class" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblClass" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "className") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkDateOfClass">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkDateOfClass" runat="server" SortExpression="dateOfClass"
                                    LabelKey="per_conteducationlist_date_class" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaDateLabel ID="lblDateOfClass" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "dateOfClass") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkCompletedHours">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle ACA_FRight">
                                <ACA:GridViewHeaderLabel ID="lnkCompletedHours" runat="server" SortExpression="hoursCompleted"
                                    LabelKey="per_conteducationlist_completedhours" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblCompletedHours" runat="server" CssClass="ACA_FRight" Text='<%# DataBinder.Eval(Container.DataItem, "hoursCompleted") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkFinalScore">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkFinalScore" runat="server" SortExpression="finalScore"
                                    LabelKey="per_conteducationlist_final_score" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblFinalScore" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "finalScore") %>' />
                                <asp:HiddenField ID="hdnGradingStyle" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "gradingStyle")%>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="80px" />
                        <headerstyle Width="80px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkAddress1">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkAddress1" runat="server" SortExpression="address1"
                                    LabelKey="per_conteducationlist_address1" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblAddress1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "address1") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkAddress2">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkAddress2" runat="server" SortExpression="address2"
                                    LabelKey="per_conteducationlist_address2" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblAddress2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "address2") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkAddress3">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkAddress3" runat="server" SortExpression="address3"
                                    LabelKey="per_conteducationlist_address3" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblAddress3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "address3") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkCity">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkCity" runat="server" SortExpression="city" LabelKey="per_conteducationlist_city"
                                    CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "city") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="80px" />
                        <headerstyle Width="80px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkState">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkState" runat="server" SortExpression="state" LabelKey="per_conteducationlist_state"
                                    CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblState" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "state") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="80px" />
                        <headerstyle Width="80px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkZip">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkZip" runat="server" SortExpression="zip" LabelKey="per_conteducationlist_zip"
                                    CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblZip" runat="server" Text='<%# ModelUIFormat.FormatZipShow(DataBinder.Eval(Container.DataItem, "zip") as string, DataBinder.Eval(Container.DataItem, "countryCode") as string)  %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkPhoneNumber1">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkPhoneNumber1" runat="server" SortExpression="phone1"
                                    LabelKey="per_conteducationlist_phone1" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblNumber1" IsNeedEncode="false" runat="server" Text='<%# ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, "phone1CountryCode") as string, DataBinder.Eval(Container.DataItem, "phone1") as string, DataBinder.Eval(Container.DataItem, "countryCode") as string) %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkPhoneNumber2">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkPhoneNumber2" runat="server" SortExpression="phone2"
                                    LabelKey="per_conteducationlist_phone2" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblNumber2" IsNeedEncode="false" runat="server" Text='<%# ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, "phone2CountryCode") as string, DataBinder.Eval(Container.DataItem, "phone2") as string, DataBinder.Eval(Container.DataItem, "countryCode") as string) %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkFax">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkFax" runat="server" LabelKey="per_conteducationlist_fax"
                                    SortExpression="fax" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblFax" runat="server" IsNeedEncode="false" Text='<%#ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, "faxCountryCode") as string, DataBinder.Eval(Container.DataItem, "fax") as string, DataBinder.Eval(Container.DataItem, "countryCode") as string)%>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkEmail">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkEmail" runat="server" LabelKey="per_conteducationlist_email"
                                    SortExpression="email" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblEmail" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "email")%>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="140px" />
                        <headerstyle Width="140px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkCountry">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkCountry" runat="server" SortExpression="countryCode" LabelKey="aca_continuingeducationlist_label_country" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblCountry" runat="server" Text='<%# StandardChoiceUtil.GetCountryByKey(DataBinder.Eval(Container.DataItem, "countryCode") as string) %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkApprovedHeader">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkApprovedHeader" runat="server" CommandName="Header"
                                        SortExpression="approvedFlag" LabelKey="aca_conteducation_list_label_approved" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblApproved" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "approvedFlag") %>' />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                            <headerstyle Width="80px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField ColumnId="Action" AttributeName="lblActionHeader">
                            <HeaderTemplate>
                                <div class="ACA_Header_Row">
                                    <ACA:AccelaLabel ID="lblActionHeader" runat="server" LabelKey="aca_continuingeducationlist_label_action" IsGridViewHeadLabel="true"></ACA:AccelaLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <HeaderStyle Width="100px" />
                        </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField>
                        <ItemTemplate>
                            <tr>
                                <td colspan="100%">
                                    <div id="divContEduComment<%#Container.DataItemIndex%>" style="display: none;width:100%">
                                        <div id="commentPanel" runat="server">
                                            <table role='presentation' cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td style="width: 21px">
                                                        &nbsp;
                                                    </td>
                                                    <td style="white-space:nowrap;vertical-align:top;">
                                                        <ACA:AccelaLabel ID="lblComments" runat="server" CssClass="ACA_SmLabel ACA_SmLabel_FontSize" Font-Bold="true"
                                                            LabelKey="per_conteducationlist_display_comments" />
                                                    </td>
                                                    <td style="width: 3px;">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <ACA:AccelaLabel ID="lblComment" runat="server" CssClass="ACA_SmLabel ACA_SmLabel_FontSize" Text='<%# DataBinder.Eval(Container.DataItem, "comments") %>' />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </ACA:AccelaTemplateField>
                </Columns>
                <EmptyDataRowStyle CssClass="ACA_SmLabel ACA_SmLabel_FontSize" />
                <PagerSettings Position="Bottom" Visible="true" Mode="numericfirstlast" NextPageText="Next"
                    PreviousPageText="Previous" />
            </ACA:AccelaGridView>
            <asp:HiddenField ID="hdnRequiredFlags" runat="server"/>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    function expandContEduComment(rowIndex) {
        var contEduComment = '<%=GetTextByKey("per_conteducationlist_display_comments").Replace("'", "\\'") %>';
        expandComment('divContEduComment' + rowIndex, 'imgExpandContEduComment' + rowIndex, 'linkExpandContEduComment' + rowIndex, contEduComment);
    }
</script>