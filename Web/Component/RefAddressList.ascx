<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.RefAddressList"
    CodeBehind="RefAddressList.ascx.cs" %>
<%@ Register Assembly="Accela.Web.Controls" Namespace="Accela.Web.Controls" TagPrefix="ACA" %>
<asp:UpdatePanel ID="refAddressUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="dgvRefAddress" GridViewNumber="60032" runat="server" AutoGenerateColumns="false"
            ShowCaption="true" AllowSorting="true" AllowPaging="true" PagerStyle-HorizontalAlign="center"
            OnRowCommand="RefAddress_RowCommand" CaptionCssClass="ACA_SmLabel ACA_SmLabel_FontSize"
            ShowExportLink="true" SummaryKey="gdv_caphome_addresslist_summary" CaptionKey="aca_caption_caphome_addresslist">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkAddressHeader">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkAddressHeader" runat="server" SortExpression="Address"
                                LabelKey="per_permitList_label_addressListAddress">
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lbAddress" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.RefAddressList.Address.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="478px" />
                    <HeaderStyle Width="478px" />
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
    </ContentTemplate>
</asp:UpdatePanel>
