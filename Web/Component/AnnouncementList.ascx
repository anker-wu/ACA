<%@ Import namespace="System.ComponentModel"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnnouncementList.ascx.cs" Inherits="Accela.ACA.Web.Component.AnnouncementList" %> 
<asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"> 
<ContentTemplate> 
    <div class="ACA_Hide">
        <ACA:AccelaButton runat="server" ID="btnRefreshAnnouncementList" OnClick="RefreshAnnouncementListButton_Click"></ACA:AccelaButton>
    </div>
    <div>
        <ACA:AccelaGridView ID="gdvAnnouncementList" GridViewNumber="60128" runat="server" AllowPaging="True" AutoGenerateCheckBoxColumn="false" 
            SummaryKey="aca_summary_announcement_collapselist" CaptionKey="aca_caption_announcement_collapselist"
            AllowSorting="true" AutoGenerateColumns="False"  PageSize="10" ShowCaption="true" 
            DataKeyNames="AnnouncementId"
            OnRowDataBound="AnnouncementList_RowDataBound"  OnRowCommand="AnnouncementList_RowCommand"
            PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom"  OnGridViewSort="AnnouncementList_GridViewSort">
                <Columns> 
                    <ACA:AccelaTemplateField AttributeName="lnkDateHeader">
                        <headertemplate>
                            <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkDateHeader" runat="server" CommandName="Header" SortExpression="ValidDate" 
                                CausesValidation="false"  LabelKey ="announcement_list_label_date" >
                                </ACA:GridViewHeaderLabel>
                            </div>
                        </headertemplate>
                        <itemtemplate>
                            <div>
                                <ACA:AccelaDateLabel id="lblUpdatedTime" runat="server" CSS="ACA_SmLabel ACA_SmLabel_FontSize" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "ValidDate")%>'></ACA:AccelaDateLabel>
                            </div>
                        </itemtemplate>
                        <ItemStyle Width="100px"/>
                        <headerstyle Width="100px"/>
                    </ACA:AccelaTemplateField> 
                    <ACA:AccelaTemplateField AttributeName="lnkTitleHeader">
                        <headertemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkTitleHeader" IsGridViewHeadLabel="true" CommandName="Header"  SortExpression="AnnouncementContentTitle"  runat="server" LabelKey="announcement_list_label_title"></ACA:GridViewHeaderLabel>
                        </div>
                        </headertemplate>
                        <itemtemplate>
                            <div class="ACA_CapListStyle"> 
                                <asp:HyperLink onmouseout="this.style.textDecoration ='none'" onmousemove="this.style.textDecoration ='underline'" href="javascript:void(0);" class="SectionTextDecoration NotShowLoading" ID="lnkAnnouncementTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AnnouncementContentTitle") %>'/>
                            </div>
                        </itemtemplate>
                        <ItemStyle Width="200px"/>
                        <headerstyle Width="200px"/>
                    </ACA:AccelaTemplateField> 
                    <ACA:AccelaTemplateField AttributeName="lnkContentHeader">
                        <headertemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                        <ACA:AccelaLabel ID="lnkContentHeader" IsGridViewHeadLabel="true" runat="server" LabelKey="announcement_list_label_content"></ACA:AccelaLabel>
                        </div>
                        </headertemplate>
                        <itemtemplate>
                            <div class="ACA_CapListStyle"> 
                                <asp:HyperLink onmouseout="this.style.textDecoration ='none'" onmousemove="this.style.textDecoration ='underline'" href="javascript:void(0);" class="SectionTextDecoration NotShowLoading" ID="lnkAnnouncementPart" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ContentPartForList") %>'/>
                            </div>
                        </itemtemplate>
                        <ItemStyle Width="400px"/>
                        <headerstyle Width="400px"/>
                    </ACA:AccelaTemplateField> 
                    <ACA:AccelaTemplateField Visible="false"> 
                        <itemtemplate>
                            <div class="ACA_CapListStyle ACA_Hide">  
                                <ACA:AccelaLabel ID="lblAnnouncementId" runat="server"  LabelType="bodyText" Text='<%# DataBinder.Eval(Container.DataItem, "AnnouncementID") %>'></ACA:AccelaLabel>
                            </div>
                        </itemtemplate>
                        <ItemStyle Width="0px" />
                        <HeaderStyle Width="0px" />
                    </ACA:AccelaTemplateField> 
                    <ACA:AccelaTemplateField Visible="false"> 
                        <itemtemplate>
                            <div class="ACA_CapListStyle ACA_Hide">  
                                <ACA:AccelaLabel ID="lblAnnouncementContentFull" runat="server"  LabelType="bodyText" Text='<%# DataBinder.Eval(Container.DataItem, "AnnouncementContentFull") %>'></ACA:AccelaLabel>
                            </div>
                        </itemtemplate>
                        <ItemStyle Width="0px" />
                        <HeaderStyle Width="0px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField Visible="false"> 
                        <itemtemplate>
                            <div class="ACA_CapListStyle ACA_Hide">  
                                <ACA:AccelaLabel ID="lblIsRead" runat="server"  LabelType="bodyText" Text='<%# DataBinder.Eval(Container.DataItem, "IsRead") %>'></ACA:AccelaLabel>
                            </div>
                        </itemtemplate>
                        <ItemStyle Width="0px" />
                        <HeaderStyle Width="0px" />
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkActionHeader">
                        <HeaderTemplate>
                            <div class="ACA_Header_Row">
                                <ACA:AccelaLabel ID="lnkActionHeader" runat="server" LabelKey="announcement_list_label_action" IsGridViewHeadLabel="true"></ACA:AccelaLabel>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div runat="server" id="divAction"> 
                            <ACA:AccelaLinkButton ID="btnDelete" LabelKey="announcement_list_label_delete" runat="server"
                                    OnClientClick="SetNotAsk();" CommandName="DeleteAnnouncement" CausesValidation="false"
                                    CommandArgument='<%#Container.DataItemIndex%>'></ACA:AccelaLinkButton>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="30px"/>
                        <headerstyle Width="30px"/>
                    </ACA:AccelaTemplateField> 
                </Columns>
            </ACA:AccelaGridView>
        </div>
    </ContentTemplate>  
</asp:UpdatePanel>
