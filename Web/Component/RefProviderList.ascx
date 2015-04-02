<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Common" %>
<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.RefProviderList" Codebehind="RefProviderList.ascx.cs" %>
<asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvRefProviderList" runat="server" AllowPaging="true" AllowSorting="True"
            SummaryKey="gdv_refproviderlist_providerlist_summary" CaptionKey="aca_caption_refproviderlist_providerlist"
            GridViewNumber="60069" ShowCaption="true" AutoGenerateColumns="false" OnRowCommand="RefProviderList_RowCommand"
            PagerStyle-HorizontalAlign="center" OnGridViewDownload="RefProviderList_GridViewDownload"
            OnPageIndexChanging="RefProvidertList_PageIndexChanging" OnGridViewSort="RefProvidertList_GridViewSort"
            PagerStyle-VerticalAlign="bottom" ShowExportLink="true">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkProviderName" ExportDataField="Name" ExportFormat="FilterScript">
                    <HeaderTemplate>
                        <div>                        
                            <ACA:GridViewHeaderLabel ID="lnkProviderName" runat="server" LabelKey="per_refproviderlist_label_name"
                                SortExpression="Name" />                        
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <asp:LinkButton ID="btnProviderName" runat="server" CommandName="selectedProvider"
                                Visible="true" CommandArgument='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.RefProvider.ProviderPKNbr.ToString())%>'><strong><%#ScriptFilter.FilterScript(DataBinder.Eval(Container.DataItem, ColumnConstant.RefProvider.Name.ToString()).ToString())%></strong></asp:LinkButton>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="200px"/>
                    <HeaderStyle Width="200px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkProviderNbr" ExportDataField="Number">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkProviderNbr" runat="server" LabelKey="per_refproviderlist_label_number"
                                SortExpression="Number" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblProviderNbr" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.RefProvider.Number.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddress" ExportDataField="Address">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkAddress" runat="server" LabelKey="per_refproviderlist_label_address"
                                SortExpression="Address" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblAddress" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.RefProvider.Address.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px"/>
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkPhoneNumber" ExportDataField="PhoneNumber">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkPhoneNumber" runat="server" LabelKey="per_refproviderlist_label_phone"
                                SortExpression="PhoneNumber" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblPhoneNumber" runat="server" IsNeedEncode="false" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.RefProvider.PhoneNumber.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
        <br />
    </ContentTemplate>
</asp:UpdatePanel>
