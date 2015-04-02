<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.RefAPOOwnerList" Codebehind="RefAPOOwnerList.ascx.cs" %>
<div class="ACA_TabRow_NoScoll ACA_WrodWrap" id="gvOwnerResult" runat="server" visible="true">
    <ACA:AccelaGridView ID="gv" runat="server" AutoGenerateColumns="false" GridViewNumber="60048" 
        SummaryKey="gdv_apoownerlist_ownerlist_summary" CaptionKey="aca_caption_apoownerlist_ownerlist"
        ShowCaption="true" AllowSorting="true" AllowPaging="true" RowStyle-CssClass="ACA_TabRow_Odd ACA_TabRow_Odd_FontSize"
        AlternatingRowStyle-CssClass="ACA_TabRow_Even ACA_TabRow_Even_FontSize" HeaderStyle-CssClass="ACA_TabRow_Header ACA_TabRow_Header_FontSize" 
        PagerStyle-HorizontalAlign="center" CaptionCssClass="ACA_SmLabel ACA_SmLabel_FontSize" OnRowCommand="RefOwnerList_RowCommand"
        OnRowDataBound="RefOwnerList_RowDataBound" OnPageIndexChanging="RefOwnerList_PageIndexChanging" OnGridViewSort="RefOwnerList_GridViewSort" OnGridViewDownload="RefOwnerList_GridViewDownload">
        <Columns>
            <ACA:AccelaTemplateField AttributeName="lnkNameHeader" ExportDataField="OwnerFullName">
                <headertemplate>
                   <div>
                    <ACA:GridViewHeaderLabel ID="lnkNameHeader" runat="server" SortExpression="OwnerFullName" CausesValidation="False" LabelKey="APO_OwnerView_Name_Header" ></ACA:GridViewHeaderLabel>
                   </div>
                </headertemplate>
                <ItemStyle Width="268px" />
                <headerstyle Width="268px" />
                <itemtemplate>
                    <div id="divLnkNum" runat="server">
                        <asp:LinkButton CausesValidation="False" ID="lnkName" runat="server" CommandName="showOwner" Visible="true" commandargument="<%#((GridViewRow)Container).RowIndex%>" >
                            <strong>
                                <%#DataBinder.Eval(Container.DataItem, "OwnerFullName")%>
                         </strong>
                        </asp:LinkButton>
                        <ACA:AccelaLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OwnerFullName") %>' Visible="False"></ACA:AccelaLabel>
                   </div>
                
             </itemtemplate>
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkAddressHeader" ExportDataField="MailAddress">
                <headertemplate>
                <div>
                    <ACA:GridViewHeaderLabel ID="lnkAddressHeader" runat="server" SortExpression="MailAddress" CausesValidation="False" LabelKey="APO_OwnerView_Address_Header" ></ACA:GridViewHeaderLabel>
                </div>
                </headertemplate>
                <ItemStyle Width="478px" />
                <headerstyle Width="478px" />
                <itemtemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblAddress" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "MailAddress")%>'></ACA:AccelaLabel>
                    </div>
                
               </itemtemplate>
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkRetrieveAssociatedDataHeader">
                <HeaderTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lnkRetrieveAssociatedDataHeader" runat="server" LabelKey="aca_apo_label_ownerlist_action" IsGridViewHeadLabel="true">
                        </ACA:AccelaLabel>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <asp:LinkButton ID="lblRetrieveAssociatedData" runat="server" Visible="true"
                            CommandName="RetrieveData" CommandArgument="<%#((GridViewRow)Container).RowIndex%>">
                            <strong>
                                <%#LabelUtil.GetTextByKey("aca_apo_label_ownerlist_retrieveassociateddata", string.Empty)%>
                            </strong>                                                              
                        </asp:LinkButton>
                    </div>
                </ItemTemplate>
                <ItemStyle Width="312px" />
                <HeaderStyle Width="312px" />
            </ACA:AccelaTemplateField>
        </Columns>
        <EmptyDataRowStyle CssClass="ACA_SmLabel ACA_SmLabel_FontSize" />
        <HeaderStyle CssClass="ACA_TabRow_Header ACA_TabRow_Header_FontSize" />
        <RowStyle CssClass="ACA_TabRow_Odd ACA_TabRow_Odd_FontSize" />
        <PagerStyle CssClass="ACA_Table_Pages ACA_Table_Pages_FontSize" VerticalAlign="Bottom" />
        <AlternatingRowStyle CssClass="ACA_TabRow_Even ACA_TabRow_Even_FontSize" />
    </ACA:AccelaGridView>
</div>