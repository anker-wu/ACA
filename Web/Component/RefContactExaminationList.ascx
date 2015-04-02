<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RefContactExaminationList.ascx.cs" Inherits="Accela.ACA.Web.Component.RefContactExaminationList" %>

<asp:UpdatePanel ID="listPanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <div class="ACA_MutipleContactNewLine">
            <ACA:AccelaGridView ID="gdvExaminationList" GridViewNumber="60085" runat="server" AllowPaging="False" AutoGenerateColumns="false" 
                SummaryKey="gdv_examination_examinalist_summary" CaptionKey="aca_caption_examination_examinalist" IsInSPEARForm="true" ShowCaption="true" AutoGenerateCheckBoxColumn="True"
                EnableViewState = "true" OnRowDataBound="ExaminationList_RowDataBound"
                AllowSorting="true" PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom" ShowHeader="true">
                <Columns>
                    <ACA:AccelaTemplateField ShowHeader="False">
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <asp:HiddenField ID="hfExamNbr" runat="server"/>
                            </div>
                        </ItemTemplate>
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField ShowHeader="false" ControlStyle-Width="15px">
                        <ItemTemplate>
                            <ACA:AccelaDiv ID="divLogo" runat="server">
                                <a id="linkExpandComment<%# Container.DataItemIndex %>" href="javascript:expandExamComment('<%# Container.DataItemIndex %>')"
                                    title="<%=GetTitleByKey("img_alt_expand_icon","examination_detail_comments") %>" class="NotShowLoading">
                                    <img id="imgExpand<%# Container.DataItemIndex %>" alt="<%=GetTextByKey("img_alt_mark_required") %>"
                                        src='<% = ImageUtil.GetImageURL("caret_collapsed.gif") %>' style="cursor: pointer;
                                        border-width: 0px" />
                                </a>
                            </ACA:AccelaDiv>
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"  />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkExaminationName">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkExaminationName" runat="server" SortExpression="examName" LabelKey="examination_list_name" CausesValidation="false" CommandName="Header" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblExamName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "examName") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkRequested">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkRequested" runat="server" SortExpression="requiredFlag" LabelKey="examination_list_required" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblRequired" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "requiredFlag") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkProviderName">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkProviderName" runat="server" SortExpression="providerName" LabelKey="examination_list_provider_name" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblProviderName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "providerName") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkProviderNumber">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkProviderNumber" runat="server" SortExpression="providerNo" LabelKey="examination_list_provider_number"  CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblProviderNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "providerNo") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="200px" />
                        <headerstyle Width="200px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkExaminationDate">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkExaminationDate" runat="server" SortExpression="examDate"  LabelKey="examination_list_exam_date" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblExaminationDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "examDate") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="200px" />
                        <headerstyle Width="200px" />
                    </ACA:AccelaTemplateField>

                    <ACA:AccelaTemplateField AttributeName="lnkExaminationStartTime">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkExaminationStartTime" runat="server" SortExpression="startTime" LabelKey="examination_list_start_time" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblExaminationStartTime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "startTime") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="200px" />
                        <headerstyle Width="200px" />
                    </ACA:AccelaTemplateField>

                    <ACA:AccelaTemplateField AttributeName="lnkExaminationEndTime">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkExaminationEndTime" runat="server" SortExpression="endTime" LabelKey="examination_list_end_time" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblExaminationEndTime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "endTime") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="200px" />
                        <headerstyle Width="200px" />
                    </ACA:AccelaTemplateField>

                    <ACA:AccelaTemplateField AttributeName="lnkFinalScore">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkFinalScore" runat="server" SortExpression="finalScore"  LabelKey="examination_list_final_score" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblFinalScore" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "finalScore") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="200px" />
                        <headerstyle Width="200px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkProviderAddress1">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkProviderAddress1" runat="server" SortExpression="examProviderDetailModel.address1" LabelKey="examination_list_address1"  CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">                                                                               
                                <ACA:AccelaLabel ID="lblProviderAddress1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "examProviderDetailModel.address1") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="200px" />
                        <headerstyle Width="200px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField  AttributeName="lnkProviderAddress2">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkProviderAddress2" runat="server" SortExpression="examProviderDetailModel.address2" LabelKey="examination_list_address2"  CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblProviderAddress2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "examProviderDetailModel.address2") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="200px" />
                        <headerstyle Width="200px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkProviderAddress3">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkProviderAddress3" runat="server" SortExpression="examProviderDetailModel.address3" LabelKey="examination_list_address3"  CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblProviderAddress3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "examProviderDetailModel.address3") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField ControlStyle-Width="100px" AttributeName="lnkProviderCity">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkProviderCity" runat="server" SortExpression="examProviderDetailModel.city" LabelKey="examination_list_city"  CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblProviderCity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "examProviderDetailModel.city") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField ControlStyle-Width="100px" AttributeName="lnkProviderState">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkProviderState" runat="server" SortExpression="examProviderDetailModel.state" LabelKey="examination_list_state"  CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblProviderState" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "examProviderDetailModel.state") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField ControlStyle-Width="100px" AttributeName="lnkProviderZipCode">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkProviderZipCode" runat="server" SortExpression="examProviderDetailModel.zip" LabelKey="examination_list_zipcode"  CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblProviderZip" runat="server" Text='<%# ModelUIFormat.FormatZipShow(DataBinder.Eval(Container.DataItem, "examProviderDetailModel.zip") as string, DataBinder.Eval(Container.DataItem, "examProviderDetailModel.countryCode") as string)  %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField ControlStyle-Width="100px" AttributeName="lnkProviderPhoneNumber1">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkProviderPhoneNumber1" runat="server" SortExpression="examProviderDetailModel.phone1" LabelKey="examination_list_phonenumber1"  CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblProviderPhone1" IsNeedEncode="false" runat="server" Text='<%#ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, "examProviderDetailModel.phone1CountryCode") as string, DataBinder.Eval(Container.DataItem, "examProviderDetailModel.phone1") as string, DataBinder.Eval(Container.DataItem, "examProviderDetailModel.countryCode") as string) as string %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkProviderPhoneNumber2">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkProviderPhoneNumber2" runat="server" SortExpression="examProviderDetailModel.phone2" LabelKey="examination_list_phonenumber2"  CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblProviderPhone2" IsNeedEncode="false" runat="server" Text='<%# ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, "examProviderDetailModel.phone2CountryCode") as string, DataBinder.Eval(Container.DataItem, "examProviderDetailModel.phone2") as string, DataBinder.Eval(Container.DataItem, "examProviderDetailModel.countryCode") as string) as string %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkProviderFax">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkProviderFax" runat="server" SortExpression="examProviderDetailModel.fax" LabelKey="examination_list_fax"  CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblProviderFax" IsNeedEncode="false" runat="server" Text='<%# ModelUIFormat.FormatPhoneShow(DataBinder.Eval(Container.DataItem, "examProviderDetailModel.faxCountryCode") as string, DataBinder.Eval(Container.DataItem, "examProviderDetailModel.fax") as string, DataBinder.Eval(Container.DataItem, "examProviderDetailModel.countryCode") as string) %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkProviderEmail">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkProviderEmail" runat="server" SortExpression="examProviderDetailModel.email" LabelKey="examination_list_email"  CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblProviderEmail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "examProviderDetailModel.email") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField> 
                    <ACA:AccelaTemplateField AttributeName="lnkExamStatus">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkExamStatus" runat="server" SortExpression="examStatus" LabelKey="examination_list_exam_status" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblExamStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "examStatus") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkUserExamID">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkUserExamID" runat="server" SortExpression="userExamID" LabelKey="examination_list_user_examid" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblUserExamID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "userExamID") %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkCountry">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkCountry" runat="server" SortExpression="examProviderDetailModel.countryCode" LabelKey="aca_examination_list_label_country" CausesValidation="false" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblCountry" runat="server" Text='<%# StandardChoiceUtil.GetCountryByKey(DataBinder.Eval(Container.DataItem, "examProviderDetailModel.countryCode") as string) %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkApprovedHeader">
                        <HeaderTemplate>
                            <div class="ACA_CapListStyle">
                                <ACA:GridViewHeaderLabel ID="lnkApprovedHeader" runat="server" CommandName="Header"
                                    SortExpression="approvedFlag" LabelKey="aca_examination_list_label_approved" CausesValidation="false"></ACA:GridViewHeaderLabel>
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
                                <ACA:AccelaLabel ID="lblActionHeader" runat="server" LabelKey="aca_examinationlist_label_action" IsGridViewHeadLabel="true"></ACA:AccelaLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div id="divAction">
                                <ACA:AccelaLinkButton ID="lnkViewExamination" runat="server" LabelKey="aca_examination_list_label_view" CausesValidation="false"
                                     CommandArgument='<%# Container.DataItemIndex%>' />
                                <ACA:AccelaLinkButton ID="lnkEditExamination" LabelKey="aca_examinationlist_label_lnkedit" runat="server" OnClientClick="SetNotAsk();"
                                                        CausesValidation="false" ></ACA:AccelaLinkButton>
                                <span>&nbsp;</span>
                                <ACA:AccelaLinkButton ID="lnkDeleteExamination" runat="server" LabelKey="examination_list_delete" CausesValidation="false"
                                     CommandArgument='<%# Container.DataItemIndex%>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                        <headerstyle Width="100px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField>
                        <ItemTemplate>
                            <tr>
                                <td colspan="100%">
                                    <div id='divExamComment<%# Container.DataItemIndex %>' class="ACA_TabRow" style="display:none;width:100%">
                                        <table role='presentation' class='ACA_TDAlignLeftOrRightTop'>
                                            <tr>
                                                <td style="width: 21px">
                                                        &nbsp;
                                                </td>
                                                <td style="white-space:nowrap;vertical-align:top;">
                                                    <ACA:AccelaLabel ID="lblCommonsTitle" runat="server" LabelKey="examination_detail_comments" CssClass="ACA_SmLabel ACA_SmLabel_FontSize" Font-Bold="true" />
                                                </td>
                                                <td>&nbsp;&nbsp;</td>
                                                <td>
                                                    <ACA:AccelaLabel ID="lblCommonsValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "comments") %>' CssClass="ACA_SmLabel ACA_SmLabel_FontSize"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </ACA:AccelaTemplateField>
                </Columns>
                <PagerSettings Position="Bottom" Mode="numericfirstlast" NextPageText="Next" PreviousPageText="Previous" />
            </ACA:AccelaGridView>
            <asp:HiddenField ID="hdnRequiredFlags" runat="server"/>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    function expandExamComment(rowIndex) {
        var examComment = '<%=GetTextByKey("examination_detail_comments").Replace("'", "\\'") %>';
        expandComment('divExamComment' + rowIndex, 'imgExpand' + rowIndex, 'linkExpandComment' + rowIndex, examComment);
    }
</script>