<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InspectionResultCommentList.ascx.cs"
    Inherits="Accela.ACA.Web.Component.InspectionResultCommentList" %>
<%@ Import Namespace="Accela.ACA.Web.Inspection" %>
<%@ Import Namespace="Accela.ACA.WSProxy" %>
<asp:UpdatePanel ID="InspectionResultCommentListPanel" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvInspectionResultCommentList" runat="server" AllowPaging="true"
            AllowSorting="True" role="presentation" ShowCaption="true"
            AutoGenerateColumns="False" PagerStyle-HorizontalAlign="center" OnRowDataBound="InspectionResultCommentList_RowDataBound"
            PagerStyle-VerticalAlign="bottom" ShowHeader="false" PageSize="5" AlternatingRowStyle-CssClass="ACA_TabRow_Odd ACA_TabRow_Odd_FontSize"
             ShowHorizontalScroll="false" CssClass="PopupDialogGridWidth">
            <Columns>
                <ACA:AccelaTemplateField>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <strong>
                                <ACA:AccelaLabel ID="lblUpdatedBy" runat="server" Text='<%# ((InspectionViewModel)Container.DataItem).LastUpdatedBy  %>' /></strong>
                            <ACA:AccelaLabel ID="lblUpdatedDateTime" runat="server" Text='<%# ((InspectionViewModel)Container.DataItem).LastUpdatedDateTimeText %>' />
                        </div>
                        <ACA:AccelaHeightSeparate ID="Separate1" runat="server" Height="3">
                        </ACA:AccelaHeightSeparate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblResultComment" EnableEllipsis="true" runat="server" Text='<%# ((InspectionViewModel)Container.DataItem).ResultComments %>' CssClass="InspectionResultComment" />
                        </div>
                        <ACA:AccelaHeightSeparate ID="Separate2" runat="server" Height="15">
                        </ACA:AccelaHeightSeparate>
                    </ItemTemplate>
                    <ItemStyle Width="700px" />
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
    </ContentTemplate>
</asp:UpdatePanel>
