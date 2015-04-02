<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.EducationList"
    CodeBehind="EducationList.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="~/Component/PopupActions.ascx" TagName="PopupActions" TagPrefix="ACA" %>
<asp:UpdatePanel ID="listPanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <div class="ACA_TabRow">
            <div id="divEducationInfo" runat="server">
                <div class="ACA_MutipleContactNewLine">
                    <ACA:AccelaLabel ID="lblEducationList" LabelKey="education_list_label" runat="server" CssClass="ACA_TabRow_Italic" LabelType="SubSectionText"></ACA:AccelaLabel>
                </div>
            </div>
            <div id="divActionNotice" runat="server" visible="false" >
                <div class="ACA_Error_Icon" EnableViewState="False" runat="server" id="divImgSuccess" visible="false">
                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("img_alt_workflow_complete") %>" src="<%=ImageUtil.GetImageURL("complete.png") %>"/>           
                </div>
                <div class="ACA_Error_Icon" EnableViewState="False" runat="server" id="divImgFailed" visible="false">
                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>" src="<%=ImageUtil.GetImageURL("error_16.gif") %>"/>            
                </div>
                <div class="licenses_certification_notice_list">
                    <ACA:AccelaLabel ID="lblActionNoticeAddSuccess" class="Notice_Message_Success" EnableViewState="False" runat="server"  LabelKey="aca_educationlist_label_addedsuccessfully" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeDeleteSuccess" class="Notice_Message_Success" EnableViewState="False" runat="server"  LabelKey="aca_educationlist_label_removedsuccessfully" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeEditSuccess" class="Notice_Message_Success" EnableViewState="False" runat="server"  LabelKey="aca_educationlist_label_editedsuccessfully" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeAddFailed" class="Notice_Message_Failure" EnableViewState="False" runat="server"  LabelKey="aca_educationlist_label_addedfailed" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeDeleteFailed" class="Notice_Message_Failure" EnableViewState="False" runat="server"  LabelKey="aca_educationlist_label_removedfailed" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeEditFailed" class="Notice_Message_Failure" EnableViewState="False" runat="server" LabelKey="aca_educationlist_label_editedfailed" Visible="false"/>
                </div>
            </div>
            <div class="ACA_MutipleContactNewLine">
                <ACA:AccelaGridView ID="gdvEducationList" runat="server" AllowPaging="True" AutoGenerateColumns="False" 
                    SummaryKey="gdv_education_educationlist_summary" CaptionKey="aca_caption_education_educationlist"
                    IsInSPEARForm="true" PageSize="10" ShowCaption="true" AutoGenerateCheckBoxColumn="false"
                    AllowSorting="true" OnRowDataBound="EducationList_RowDataBound"
                    PagerStyle-HorizontalAlign="center" IsAutoWidth="True">
                    <Columns>
                        <ACA:AccelaTemplateField ShowHeader="false">
                            <ItemTemplate>
                                <ACA:AccelaDiv ID="divImg" runat="server">
                                    <img id="imgMarkRequired" src="<%= ImageUtil.GetImageURL("error_16.gif") %>" alt="<%=GetTextByKey("img_alt_mark_required") %>" />
                                </ACA:AccelaDiv>
                            </ItemTemplate>
                            <ItemStyle CssClass="ACA_VerticalAlign" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField ShowHeader="false">
                            <ItemTemplate>
                                <div>
                                    <ACA:AccelaDiv ID="divLogo" runat="server">
                                       <a id="linkExpandComment<%# Eval(ColumnConstant.Education.RowIndex.ToString()) %>" href="javascript:ExpandComment('divEducation<%# Eval(ColumnConstant.Education.RowIndex.ToString()) %>','linkExpandComment<%# Eval(ColumnConstant.Education.RowIndex.ToString()) %>')"
                                        title="<%=GetTitleByKey("img_alt_expand_icon","education_list_display_comments") %>" class="NotShowLoading">
                                       <img id="lnkToggledivEducation<%# Eval(ColumnConstant.Education.RowIndex.ToString()) %>" alt="<%=GetTextByKey("img_alt_expand_icon") %>"
                                            src="<%= ImageUtil.GetImageURL("caret_collapsed.gif") %>" style="cursor:pointer; border-width:0px" /></a>
                                    </ACA:AccelaDiv>
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="ACA_VerticalAlign" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkProviderNameHeader">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkProviderNameHeader" runat="server" CommandName="Header"
                                        SortExpression="providerName" LabelKey="education_list_provider_name" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblProviderName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Education.providerName.ToString()) %>' />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <headerstyle Width="100px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkProviderNumberHeader">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkProviderNumberHeader" runat="server" CommandName="Header"
                                        SortExpression="providerNo" LabelKey="education_list_provider_number" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblProviderNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Education.providerNo.ToString()) %>' />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="130px" />
                            <headerstyle Width="130px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkMajorDisciplineHeader">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkMajorDisciplineHeader" runat="server" CommandName="Header"
                                        SortExpression="educationName" LabelKey="education_list_major_discipline" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <strong>
                                        <ACA:AccelaLinkButton ID="lnkMajorDiscipine" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Education.educationName.ToString()) %>'
                                            CausesValidation="false" CommandName="<%# SELECT_EDUCATION %>" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "RowIndex")%>' OnClick="EducationList_ActionCommand"></ACA:AccelaLinkButton></strong>
                                    <ACA:AccelaLabel ID="lblRefEducationName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Education.educationName.ToString()) %>'
                                        Visible="false">
                                    </ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="130px" />
                            <headerstyle Width="130px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkDegreeHeader">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkDegreeHeader" runat="server" CommandName="Header"
                                        SortExpression="degree" LabelKey="education_list_degree" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblDegree" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Education.degree.ToString()) %>'></ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                            <headerstyle Width="80px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkYearAttendedHeader">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkYearAttendedHeader" runat="server" CommandName="Header"
                                        SortExpression="yearAttended" LabelKey="education_list_attended" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblYearAttended" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Education.yearAttended.ToString()) %>'></ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <headerstyle Width="100px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkYearGraduatedHeader">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkYearGraduatedHeader" runat="server" CommandName="Header"
                                        SortExpression="yearGraduated" LabelKey="education_list_graduateded" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblYearGraduated" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Education.yearGraduated.ToString()) %>'></ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <headerstyle Width="100px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkProviderAddress1Header">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkProviderAddress1Header" runat="server" CommandName="Header"
                                        SortExpression="address1" LabelKey="education_list_address1" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblProviderAddress" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Education.address1.ToString()) %>'></ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <headerstyle Width="100px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkProviderAddress2Header">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkProviderAddress2Header" runat="server" CommandName="Header"
                                        SortExpression="address2" LabelKey="education_list_address2" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblProviderAddress2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Education.address2.ToString()) %>'></ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <headerstyle Width="100px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkProviderAddress3Header">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkProviderAddress3Header" runat="server" CommandName="Header"
                                        SortExpression="address3" LabelKey="education_list_address3" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblProviderAddress3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Education.address3.ToString()) %>'></ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <headerstyle Width="100px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkProviderCityHeader">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkProviderCityHeader" runat="server" CommandName="Header"
                                        SortExpression="city" LabelKey="education_list_city" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblProviderCity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Education.city.ToString()) %>'></ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                            <headerstyle Width="80px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkProviderStateHeader">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkProviderStateHeader" runat="server" CommandName="Header"
                                        SortExpression="state" LabelKey="education_list_state" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblProviderState" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Education.state.ToString()) %>'></ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                            <headerstyle Width="80px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkProviderZipCodeHeader">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkProviderZipCodeHeader" runat="server" CommandName="Header"
                                        SortExpression="zip" LabelKey="education_list_zip" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblProviderZipCode" runat="server" Text='<%# ModelUIFormat.FormatZipShow(DataBinder.Eval(Container.DataItem, ColumnConstant.Education.zip.ToString()) as string, DataBinder.Eval(Container.DataItem, ColumnConstant.Education.countryCode.ToString()) as string) %>'>
                                    </ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <headerstyle Width="100px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkProviderPhoneNumberHeader">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkProviderPhoneNumberHeader" runat="server" CommandName="Header"
                                        SortExpression="phone1" LabelKey="education_list_phone1" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblProviderPhoneNumber" IsNeedEncode="false" runat="server"
                                        Text='<%# ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, ColumnConstant.Education.phone1CountryCode.ToString()) as string, DataBinder.Eval(Container.DataItem, ColumnConstant.Education.phone1.ToString()) as string, DataBinder.Eval(Container.DataItem, ColumnConstant.Education.countryCode.ToString()) as string) %>'>
                                    </ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <headerstyle Width="100px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkMobilePhoneHeader">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkMobilePhoneHeader" IsNeedEncode="false" runat="server"
                                        LabelKey="education_list_phone2" SortExpression="phone2" CommandName="Header"
                                        CausesValidation="false" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblMobilePhone" runat="server" IsNeedEncode="false" Text='<%#ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, ColumnConstant.Education.phone2CountryCode.ToString()) as string, DataBinder.Eval(Container.DataItem, ColumnConstant.Education.phone2.ToString()) as string, DataBinder.Eval(Container.DataItem, ColumnConstant.Education.countryCode.ToString()) as string)%>' />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <headerstyle Width="100px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkFaxHeader">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkFaxHeader" runat="server" LabelKey="education_list_fax"
                                        SortExpression="fax" CommandName="Header" CausesValidation="false" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblFax" runat="server" IsNeedEncode="false" Text='<%#ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, ColumnConstant.Education.faxCountryCode.ToString()) as string, DataBinder.Eval(Container.DataItem, ColumnConstant.Education.fax.ToString()) as string, DataBinder.Eval(Container.DataItem, ColumnConstant.Education.countryCode.ToString()) as string)%>' />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <headerstyle Width="100px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkEmailHeader">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkEmailHeader" runat="server" LabelKey="education_list_email"
                                        SortExpression="email" CommandName="Header" CausesValidation="false" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblEmail" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.Education.email.ToString())%>' />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="140px" />
                            <headerstyle Width="140px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkRequiredHeader">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkRequiredHeader" runat="server" CommandName="Header"
                                        SortExpression="requiredFlag" LabelKey="education_list_required" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblRequired" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Education.requiredFlag.ToString()) %>'></ACA:AccelaLabel>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                            <headerstyle Width="80px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkCountry">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkCountry" runat="server" SortExpression="CountryCode" LabelKey="aca_education_list_label_country" CausesValidation="false" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblCountry" runat="server" Text='<%# StandardChoiceUtil.GetCountryByKey(DataBinder.Eval(Container.DataItem, ColumnConstant.Education.countryCode.ToString()) as string) %>' />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <headerstyle Width="100px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkApprovedHeader">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkApprovedHeader" runat="server" CommandName="Header"
                                        SortExpression="approvedFlag" LabelKey="aca_contact_education_list_label_approved" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblApproved" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Education.ApprovedFlag.ToString()) %>' />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                            <headerstyle Width="80px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lblActionHeader">
                            <HeaderTemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblActionHeader" runat="server" LabelKey="aca_educationlist_label_action" IsGridViewHeadLabel="true"></ACA:AccelaLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_FLeft">
                                    <ACA:PopupActions ID="actionMenu" Visible="false" runat="server" />
                                    <ACA:AccelaLinkButton ID="lnkView" CausesValidation="false" runat="server" TabIndex="-1" />
                                    <ACA:AccelaLinkButton ID="lnkEdit" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "RowIndex") %>' OnClientClick="SetNotAsk();" 
                                        OnClick="EducationList_ActionCommand" CommandName="<%# SELECT_EDUCATION %>" CausesValidation="false" runat="server" TabIndex="-1"></ACA:AccelaLinkButton>
                                    <ACA:AccelaLinkButton ID="lnkDelete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "RowIndex") %>' OnClientClick="SetNotAsk();"
                                        OnClick="EducationList_ActionCommand" CommandName="<%# DELETE_EDUCATION %>" CausesValidation="false" runat="server" TabIndex="-1"></ACA:AccelaLinkButton>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                            <HeaderStyle Width="80px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField>
                            <ItemTemplate>
                                <tr style="display:none">
                                    <td colspan="100%">
                                        <div id="divEducation<%# Eval(ColumnConstant.Education.RowIndex.ToString()) %>" style="display: none;width:100%">
                                            <div id="commentPanel" runat="server">
                                                <table role='presentation' cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="width: 21px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <ACA:AccelaLabel ID="lblComments" runat="server" CssClass="ACA_SmLabel ACA_SmLabel_FontSize" Font-Bold="true"
                                                                LabelKey="education_list_display_comments"></ACA:AccelaLabel>
                                                        </td>
                                                        <td style="width: 3px;">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <ACA:AccelaLabel ID="lblComment" runat="server" CssClass="ACA_SmLabel ACA_SmLabel_FontSize" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Education.comments.ToString()) %>'></ACA:AccelaLabel>
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
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    var imgCollapsed = '<%=ImageUtil.GetImageURL("caret_collapsed.gif") %>';
    var imgExpanded = '<%=ImageUtil.GetImageURL("caret_expanded.gif") %>';
    var altCollapsed = '<%=GetTextByKey("img_alt_collapse_icon").Replace("'","\\'") %>'; 
    var altExpanded = '<%=GetTextByKey("img_alt_expand_icon").Replace("'","\\'") %>';
    var comment = '<%=GetTextByKey("education_list_display_comments").Replace("'","\\'") %>';

    function ExpandComment(obj, lnkObj)
    { 
        if(obj=='undefined') return;
        var div = document.getElementById(obj);
        var trTag = div != null && div.parentNode && div.parentNode.parentNode && div.parentNode.parentNode.tagName == "TR" ? div.parentNode.parentNode : null;
        var img = document.getElementById('lnkToggle' + obj);
        
        if (div.style.display == "none")
        {
            trTag.style.display = "";
            div.style.display = "";
            Expanded(img, imgExpanded, altCollapsed);
            lnkObj.title = altCollapsed + " " + comment;
        }
        else
        {
            trTag.style.display = "none";
            div.style.display = "none";
            Collapsed(img, imgCollapsed, altExpanded);
            lnkObj.title = altExpanded + " " + comment;
        }
    }
</script>
