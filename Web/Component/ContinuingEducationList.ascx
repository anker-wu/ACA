<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.ContinuingEducationList"
    CodeBehind="ContinuingEducationList.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Common" %>
<%@ Register Src="~/Component/PopupActions.ascx" TagName="PopupActions" TagPrefix="ACA" %>
<asp:UpdatePanel ID="listPanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <div class="ACA_Row">
            <div class="ACA_MutipleContactNewLine">
                <table role='presentation'><tr><td id="tdExpandIcon" runat="server">
                <a id="lnkToggledivContEduactionList" href="javascript:void(0);" class="ACA_Row NotShowLoading" onclick="ContEducationListExpandCollapse('divContEduactionList',$get('lnkToggledivContEduactionList'),$get('<%=lblContEducationTitle.ClientID %>'));"
                 title="<%=GetTitleByKey("img_alt_expand_icon","per_conteducationlist_title") %>">
                    <img id="lnkToggledivContEduactionListIcon" class="ACA_NoBorder" alt="<%=GetTextByKey("img_alt_expand_icon") %>" src="<%=ImageUtil.GetImageURL("caret_expanded.gif") %>" />
                </a>
                </td><td>
                <ACA:AccelaLabel ID="lblContEducationTitle" LabelKey="per_conteducationlist_title"
                            runat="server" CssClass="ACA_Title_Text font13px" LabelType="SectionTitleWithBar" />
                </td></tr></table>
                <span ID="lblContEducationTitle_sub_label" class="ACA_Section_Instruction ACA_Section_Instruction_FontSize" runat="server" ></span>
            </div>
            <div id="divActionNotice" EnableViewState="False" runat="server" visible="false" >
                <div class="ACA_Error_Icon" runat="server" id="divImgSuccess" visible="false">
                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("img_alt_workflow_complete") %>" src="<%=ImageUtil.GetImageURL("complete.png") %>"/>           
                </div>
                <div class="ACA_Error_Icon" EnableViewState="False" runat="server" id="divImgFailed" visible="false">
                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>" src="<%=ImageUtil.GetImageURL("error_16.gif") %>"/>            
                </div>
                <div class="licenses_certification_notice_list">
                    <ACA:AccelaLabel ID="lblActionNoticeAddSuccess" class="Notice_Message_Success" EnableViewState="False" runat="server"  LabelKey="aca_coneducationlist_label_addedsuccessfully" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeDeleteSuccess" class="Notice_Message_Success" EnableViewState="False" runat="server"  LabelKey="aca_coneducationlist_label_removedsuccessfully" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeEditSuccess" class="Notice_Message_Success" EnableViewState="False" runat="server"  LabelKey="aca_coneducationlist_label_editedsuccessfully" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeAddFailed" class="Notice_Message_Failure" EnableViewState="False" runat="server"  LabelKey="aca_coneducationlist_label_addedfailed" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeDeleteFailed" class="Notice_Message_Failure" EnableViewState="False" runat="server"  LabelKey="aca_coneducationlist_label_removedfailed" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeEditFailed" class="Notice_Message_Failure" EnableViewState="False" runat="server" LabelKey="aca_coneducationlist_label_editedfailed" Visible="false"/>
                </div>
            </div>
            <div id="divContEduactionList" class="ACA_MutipleContactNewLine">
                <ACA:AccelaGridView ID="gdvContEducationList" runat="server" AllowPaging="True" AutoGenerateColumns="False" 
                SummaryKey="gdv_conteducation_conteducationlist_summary" CaptionKey="aca_caption_conteducation_conteducationlist"
                    IsInSPEARForm="true" PageSize="10" ShowCaption="true" AutoGenerateCheckBoxColumn="false"
                    AllowSorting="true" OnRowDataBound="ContEducationList_RowDataBound" IsAutoWidth="True"
                    PagerStyle-HorizontalAlign="center" PagerStyle-CssClass="ACA_Table_Pages ACA_Table_Pages_FontSize" PagerStyle-VerticalAlign="bottom">
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
                                        <a id="linkExpandComment<%#Container.DataItemIndex%>" class="NotShowLoading" href="javascript:ExpandCollapseComment('divContEducation<%#Container.DataItemIndex%>','linkExpandComment<%#Container.DataItemIndex%>')"
                                         title="<%=GetTitleByKey("img_alt_expand_icon","per_conteducationlist_display_comments") %>">
                                            <img id="lnkToggledivContEducation<%#Container.DataItemIndex%>" alt="<%=GetTextByKey("img_alt_expand_icon") %>"
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
                                    <ACA:AccelaLinkButton ID="btnContEducationName" runat="server" CausesValidation="false" CommandName="<%#SELECT_CONT_EDUCATION %>" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RowIndex")%>' OnClick="ContEducationList_ActionCommand"><strong><%#ScriptFilter.FilterScriptEx(DataBinder.Eval(Container.DataItem, "contEduName"))%></strong></ACA:AccelaLinkButton>
                                    <ACA:AccelaLabel ID="lblContEducationName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "contEduName") %>'
                                        Visible="false">
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
                                    <ACA:AccelaLabel ID="lblRequiredOptional" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "requiredFlag") %>' />
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
                        <ACA:AccelaTemplateField AttributeName="lnkPassingScore">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkPassingScore" runat="server" SortExpression="passingScore" CausesValidation="false" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblPassingScore" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "passingScore") %>' />
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
                                        SortExpression="approvedFlag" LabelKey="aca_contact_continuing_education_list_label_approved" CausesValidation="false"></ACA:GridViewHeaderLabel>
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
                                    <ACA:AccelaLabel ID="lblActionHeader" runat="server" LabelKey="aca_continuingeducationlist_label_action" IsGridViewHeadLabel="true"></ACA:AccelaLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_FLeft">
                                    <ACA:PopupActions ID="actionMenu" Visible="false" runat="server" />
                                    <ACA:AccelaLinkButton ID="lnkView" runat="server" CausesValidation="false" TabIndex="-1"/>
                                    <ACA:AccelaLinkButton ID="lnkEdit" runat="server" OnClientClick="SetNotAsk();" OnClick="ContEducationList_ActionCommand" CausesValidation="false"
                                        CommandName="<%# SELECT_CONT_EDUCATION %>" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RowIndex") %>' TabIndex="-1"></ACA:AccelaLinkButton>
                                    <ACA:AccelaLinkButton ID="lnkDelete" runat="server" CausesValidation="false" OnClick="ContEducationList_ActionCommand"
                                        CommandName="<%# DELETE_CONT_EDUCATION %>" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RowIndex")%>' TabIndex="-1"/>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                            <HeaderStyle Width="80px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField>
                            <ItemTemplate>
                                <tr style="display:none">
                                    <td colspan="100%">
                                        <div id="divContEducation<%#Container.DataItemIndex%>" style="display: none;width:100%">
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
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    var isExpand = null;
    var gridViewId = '<%=gdvContEducationList.ClientID %>';
    var imgCollapsed = '<%=ImageUtil.GetImageURL("caret_collapsed.gif") %>';
    var imgExpanded = '<%=ImageUtil.GetImageURL("caret_expanded.gif") %>';
    var altCollapsed = '<%=GetTextByKey("img_alt_collapse_icon").Replace("'","\\'") %>';
    var altExpanded = '<%=GetTextByKey("img_alt_expand_icon").Replace("'","\\'") %>';
    var comment = '<%=GetTextByKey("per_conteducationlist_display_comments").Replace("'","\\'") %>';

    function ContEducationListExpandCollapse(obj, lnkObj, lblValue)
    {
        if(obj=='undefined') return;
        SetNotAskForSPEAR();
        var div = document.getElementById(obj);
        var lnk = document.getElementById('lnkToggle' + obj + 'Icon');
        
        if (div.className == "ACA_Hide")
        {
            div.className = "ACA_Show";
            Expanded(lnk, imgExpanded, altCollapsed);
            AddTitle(lnkObj, altCollapsed, lblValue);
            isExpand = true;
            showSectionInstruction(true);
        }
        else
        {
            div.className = "ACA_Hide";
            Collapsed(lnk, imgCollapsed, altExpanded);
            AddTitle(lnkObj, altExpanded, lblValue);
            isExpand = false;
            showSectionInstruction(false);   
        }
    }
    
    function ShowEducationMessage() {
        var lnk = document.getElementById("lnkToggledivContEduactionListIcon");
        var lblMessageId = gridViewId+"_4ValidateGridView_lbl_error_msg";
        var lblMessage = document.getElementById(lblMessageId);
        
        if (lblMessage && lnk) {
            var div = document.getElementById('divContEduactionList');
            if (div && div.className == "ACA_Hide") {
                div.className = "ACA_Show";
                Expanded(lnk, imgExpanded, altCollapsed);
                isExpand = true;        
            }
        }
    }
    
    function ExpandCollapseContEducationList(isExistDataSource)
    {
        var div = document.getElementById("divContEduactionList");
        var lnk = document.getElementById("lnkToggledivContEduactionListIcon");
        
        isExpand = isExistDataSource;

        div.className = isExpand ? "ACA_Show" : "ACA_Hide";
        if (isExpand){
            Expanded(lnk, imgExpanded, altCollapsed);
        }else{
            Collapsed(lnk, imgCollapsed, altExpanded);
        }

        showSectionInstruction(isExpand);
    }
    
    function ExpandContEducationList()
    {
        var div = document.getElementById("divContEduactionList");
        var lnk = document.getElementById("lnkToggledivContEduactionListIcon");
        div.className =  "ACA_Show";
        Expanded(lnk, imgExpanded, altCollapsed);
        
        $get("lnkToggledivContEduactionList").onclick='return false;';
        showSectionInstruction(true);
    }

    function showSectionInstruction(isShow)
    {
       var instruction = $get("<%=lblContEducationTitle_sub_label.ClientID %>");
       
       if(isShow && instruction.innerHTML)
       {
           instruction.className = instruction.className.replace("ACA_Hide", "ACA_Section_Instruction ACA_Section_Instruction_FontSize");
       }
       else 
       {
           instruction.className = instruction.className.replace("ACA_Section_Instruction ACA_Section_Instruction_FontSize", "ACA_Hide");
       }
    }
    
    function ConfirmDelete()
    {
        return confirm('<%=GetTextByKey("aca_common_delete_alert_message").Replace("'","\\'")%>');
    }

    function ExpandCollapseComment(obj, lnkObjID)
    { 
        if(obj=='undefined') return;
        var div = document.getElementById(obj);
        var img = document.getElementById('lnkToggle' + obj);
        var trTag = div != null && div.parentNode && div.parentNode.parentNode && div.parentNode.parentNode.tagName == "TR" ? div.parentNode.parentNode : null;
        var lnkObj = document.getElementById(lnkObjID);
        
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
