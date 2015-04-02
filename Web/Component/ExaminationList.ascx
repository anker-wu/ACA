<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.ExaminationList" Codebehind="ExaminationList.ascx.cs" %>
<%@ Register Src="~/Component/PopupActions.ascx" TagName="PopupActions" TagPrefix="ACA" %>
<asp:UpdatePanel ID="listPanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <div class="ACA_TabRow">
            <!-- Begin Examination List Description -->
            <div id="divExaminationInfo" runat="server">
                <div class="ACA_MutipleContactNewLine">
                     <ACA:AccelaLabel ID="lblEducationList" LabelKey="examination_list_title" runat="server" CssClass="ACA_TabRow_Italic" LabelType="SubSectionText"></ACA:AccelaLabel>
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
                    <ACA:AccelaLabel ID="lblActionNoticeAddSuccess" class="Notice_Message_Success" runat="server" EnableViewState="False" LabelKey="aca_examinationlist_label_addedsuccessfully" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeDeleteSuccess" class="Notice_Message_Success" runat="server" EnableViewState="False" LabelKey="aca_examinationlist_label_removedsuccessfully" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeEditSuccess" class="Notice_Message_Success" runat="server" EnableViewState="False" LabelKey="aca_examinationlist_label_editedsuccessfully" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeAddFailed" class="Notice_Message_Failure" runat="server" EnableViewState="False" LabelKey="aca_examinationlist_label_addedfailed" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeDeleteFailed" class="Notice_Message_Failure" runat="server" EnableViewState="False" LabelKey="aca_examinationlist_label_removedfailed" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeEditFailed" class="Notice_Message_Failure" runat="server" EnableViewState="False" LabelKey="aca_examinationlist_label_editedfailed" Visible="false"/>
                </div>
            </div>
            <!-- End Examination List Description -->
            <!-- Begin Examination List -->
            <div class="ACA_MutipleContactNewLine">
                <ACA:AccelaGridView ID="gdvExaminationList" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                    SummaryKey="gdv_examination_examinalist_summary" CaptionKey="aca_caption_examination_examinalist"
                    IsInSPEARForm="true" PageSize="10" ShowCaption="true" AutoGenerateCheckBoxColumn="false"
                    EnableViewState = "true" OnRowDataBound="ExaminationList_RowDataBound" IsAutoWidth="True"
                    AllowSorting="true" PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom" ShowHeader="true">
                    <Columns>
                        <ACA:AccelaTemplateField ShowHeader="false" ControlStyle-Width="15px">
                            <ItemTemplate>
                                <ACA:AccelaDiv ID="divLogo" runat="server">
                                    <a id="linkExpandComment<%# DataBinder.Eval(Container.DataItem, "RowIndex") %>" href="javascript:ExpandExaminationRow('divCommon<%# DataBinder.Eval(Container.DataItem, "RowIndex") %>','imgExpand<%# DataBinder.Eval(Container.DataItem, "RowIndex") %>','linkExpandComment<%# DataBinder.Eval(Container.DataItem, "RowIndex") %>')"
                                     title="<%=GetTitleByKey("img_alt_expand_icon","examination_detail_comments") %>" class="NotShowLoading">
                                        <img id="imgExpand<%# DataBinder.Eval(Container.DataItem, "RowIndex") %>" alt="<%=GetTextByKey("img_alt_mark_required") %>"
                                            src='<% = ImageUtil.GetImageURL("caret_collapsed.gif") %>' style="cursor: pointer;
                                            border-width: 0px" />
                                    </a>
                                </ACA:AccelaDiv>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"  />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField ShowHeader="false" ControlStyle-Width="15px">
                            <ItemTemplate>
                                <ACA:AccelaDiv ID="divImg" runat="server">
                                    <img id="imgMarkRequired" src="<%= ImageUtil.GetImageURL("error_16.gif") %>" alt="<%=GetTextByKey("img_alt_expand_icon") %>" />
                                </ACA:AccelaDiv>
                            </ItemTemplate>
                            <ItemStyle CssClass="ACA_VerticalAlign" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lnkExaminationName">
                            <HeaderTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:GridViewHeaderLabel ID="lnkExaminationName" runat="server" SortExpression="examName" LabelKey="examination_list_name" CausesValidation="false" CommandName="Header" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <strong><ACA:AccelaLinkButton ID="ExaminationName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "examName") %>' CausesValidation = "false" 
                                       CommandName="<%# EXAMINATION_SELECTED %>" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RowIndex")%>' OnClick="ExamList_ActionCommand"/></strong>
                                    <ACA:AccelaLabel ID="lblExamName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "examName") %>' Visible="False" />
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
                            <ItemStyle Width="100px" />
                            <headerstyle Width="100px" />
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
                                        SortExpression="approvedFlag" LabelKey="aca_contact_examination_list_label_approved" CausesValidation="false"></ACA:GridViewHeaderLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_CapListStyle">
                                    <ACA:AccelaLabel ID="lblApproved" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ApprovedFlag") %>' />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                            <headerstyle Width="80px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField AttributeName="lblActionHeader">
                            <HeaderTemplate>
                                <div>
                                    <ACA:AccelaLabel ID="lblActionHeader" runat="server" LabelKey="aca_examinationlist_label_action" IsGridViewHeadLabel="true"></ACA:AccelaLabel>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_FLeft">
                                    <ACA:PopupActions ID="actionMenu" Visible="false" runat="server" />
                                    <ACA:AccelaLinkButton ID="lnkViewExamination" runat="server" CausesValidation="false" TabIndex="-1" />
                                    <ACA:AccelaLinkButton ID="lnkEditExamination" runat="server" OnClientClick="SetNotAsk();" OnClick="ExamList_ActionCommand" CausesValidation="false"
                                        CommandName="<%# EXAMINATION_SELECTED %>" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RowIndex") %>' TabIndex="-1"></ACA:AccelaLinkButton>
                                    <ACA:AccelaLinkButton ID="lnkDeleteExamination" runat="server" CausesValidation="false" OnClick="ExamList_ActionCommand"
                                        CommandName="<%# EXAMINATION_DELECTED %>" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RowIndex")%>' TabIndex="-1"/>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                            <HeaderStyle Width="80px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField>
                            <ItemTemplate>
                                <tr style="display:none">
                                    <td colspan="100%">
                                        <div id='divCommon<%# DataBinder.Eval(Container.DataItem, "RowIndex") %>' class="ACA_TabRow" style="display:none;width:100%">
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
            </div>
            <!-- End Examination List -->
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    var CTreeTop = '<%=ImageUtil.GetImageURL("caret_collapsed.gif") %>';
    var ETreeTop = '<%=ImageUtil.GetImageURL("caret_expanded.gif") %>';
    var altCollapsed = '<%=GetTextByKey("img_alt_collapse_icon").Replace("'","\\'") %>';
    var altExpanded = '<%=GetTextByKey("img_alt_expand_icon").Replace("'","\\'") %>';
    var comment = '<%=GetTextByKey("examination_detail_comments").Replace("'","\\'") %>';

    function ExpandExaminationRow(divObj, imgObj, lnkObj)
    { 
        if(divObj=='undefined' || imgObj == 'undefined' ) return;
        
        var div = document.getElementById(divObj);
        var img = document.getElementById(imgObj);
        var trTag = div != null && div.parentNode && div.parentNode.parentNode && div.parentNode.parentNode.tagName == "TR" ? div.parentNode.parentNode : null;
        
        if (div.style.display == "none")
        {
            trTag.style.display = "";
            div.style.display = "";
            Expanded(img, ETreeTop, altCollapsed);
            lnkObj.title = altCollapsed + comment;
        }
        else
        {
            trTag.style.display = "none";
            div.style.display = "none";
            Collapsed(img, CTreeTop, altExpanded);
            lnkObj.title = altExpanded + comment;
        }
    }
</script>
