<%@ Import namespace="System.ComponentModel"%>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.MyCollectionList" Codebehind="MyCollectionList.ascx.cs" %>

<asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
       <ACA:AccelaGridView ID="gdvMyCollectionList" GridViewNumber="60060" runat="server" AllowPaging="True" AutoGenerateCheckBoxColumn="false"
            SummaryKey="gdv_mycollection_collapselist_summary" CaptionKey="aca_caption_mycollection_collapselist"
            AllowSorting="true" AutoGenerateColumns="False" PageSize="10" ShowCaption="true" 
            DataKeyNames="CollectionId" OnRowDeleting="MyCollectionList_RowDeleting" 
            OnRowDataBound="MyCollectionList_RowDataBound"  OnRowCommand="MyCollectionList_RowCommand"
            PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom">
            <Columns> 
                <ACA:AccelaTemplateField AttributeName="lnkDateHeader">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkDateHeader" runat="server" CommandName="Header" SortExpression="UpdateDate" 
                          CausesValidation="false"   LabelKey ="mycollection_managepage_label_createdate" >
                            </ACA:GridViewHeaderLabel>
                            
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaDateLabel id="lblUpdatedTime" runat="server" CSS="ACA_SmLabel ACA_SmLabel_FontSize" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "UpdateDate")%>'></ACA:AccelaDateLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="100px"/>
                    <headerstyle Width="100px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkMyCollectionNameHeader">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkMyCollectionNameHeader" runat="server" CommandName="Header" SortExpression="CollectionName" 
                       CausesValidation="false"   LabelKey="mycollection_managepage_label_name" >
                         </ACA:GridViewHeaderLabel>
                        
                    </div>
                    </headertemplate>
                    <itemtemplate>
                            <div>
                            <asp:HyperLink ID="hlCollectionName" runat ="server" ><ACA:AccelaLabel ID="lblCollectionName" runat ="server"  Text = '<%# DataBinder.Eval(Container.DataItem, "CollectionName") %>'/></asp:HyperLink>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="150px"/>
                    <headerstyle Width="150px"/>
                </ACA:AccelaTemplateField>
                 <ACA:AccelaTemplateField AttributeName="lnkDescHeader">
                    <headertemplate>
                    <div class="ACA_CapListStyle ACA_Header_Row">
                    <ACA:AccelaLabel ID="lnkDescHeader" IsGridViewHeadLabel="true" runat="server" LabelKey="mycollection_managepage_label_description"></ACA:AccelaLabel>
                    </div>
                    </headertemplate>
                    <itemtemplate>
                        <div class="ACA_CapListStyle">
                            <%--use AccelaLabel control instead of asp.Label control, updated by Peter.Pan --%>
                            <ACA:AccelaLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CollectionDesc") %>'></ACA:AccelaLabel>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
                 <ACA:AccelaTemplateField AttributeName="lnkRecordNumberHeader">
                    <headertemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                    <ACA:AccelaLabel ID="lnkRecordNumberHeader" IsGridViewHeadLabel="true" runat="server" LabelKey="mycollection_managepage_label_rocordnumber"></ACA:AccelaLabel>
                        
                    </div>
                </headertemplate>
                <itemtemplate>
                        <div class="ACA_CapListStyle">
                        <ACA:AccelaLabel ID="lblRecordsAmount" runat ="server" Text = '<%# DataBinder.Eval(Container.DataItem, "RecordsAmount") %>' ></ACA:AccelaLabel>
                    </div>
                </itemtemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
                 <ACA:AccelaTemplateField ShowHeader="false" AttributeName="lnkDelHeader">
                    <headertemplate>
                         <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkDelHeader" runat="server" TabIndex="-1" >
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                            <div class="ACA_CapListStyle">
                            <ACA:AccelaLinkButton ID="lnkDelete" runat="server" CSS="ACA_SmLabel ACA_SmLabel_FontSize" CommandName="Delete" LabelKey="mycollection_managepage_label_delete">                             
                            </ACA:AccelaLinkButton>
                            &nbsp;
                            </div>    
                    </itemtemplate>
                    <ItemStyle Width="130px"/>
                    <headerstyle Width="130px"/>
                </ACA:AccelaTemplateField>
            </Columns>
    </ACA:AccelaGridView>
    </ContentTemplate> 
</asp:UpdatePanel>


