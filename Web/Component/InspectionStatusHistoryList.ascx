<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InspectionStatusHistoryList.ascx.cs"
    Inherits="Accela.ACA.Web.Component.InspectionStatusHistoryList" %>
<%@ Import Namespace="Accela.ACA.Web.Inspection" %>
<asp:UpdatePanel ID="InspectionStatusHistoryPanel" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvInspectionStatusHistoryList" runat="server" AllowPaging="true"
            GridViewNumber="60109" AllowSorting="True" 
            SummaryKey="inspection_statushistorylist_summary" CaptionKey="aca_caption_inspection_statushistorylist"
            ShowCaption="true"
            AutoGenerateColumns="False" PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom" 
            ShowHorizontalScroll="false" CssClass="PopupDialogGridWidth">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkStatus">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkStatus" runat="server" SortExpression="StatusText"
                                LabelKey="ins_inspectionList_label_status" CausesValidation="false" CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblStatus" runat="server" Text='<%# ((InspectionViewModel)Container.DataItem).StatusText %>' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="100px" />
                    <ItemStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkStatusDateTime">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkStatusDateTime" runat="server" SortExpression="StatusDateTime"
                                LabelKey="inspection_statushistorylist_statusdate" CausesValidation="false" CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblStatusDateTime" runat="server" Text='<%# ((InspectionViewModel)Container.DataItem).StatusDateTimeText %>' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                    <ItemStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkInspector">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkInspector" runat="server" SortExpression="Inspector"
                                LabelKey="inspection_statushistorylist_inspector" CausesValidation="false" CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblInspector" runat="server" Text='<%# ((InspectionViewModel)Container.DataItem).Inspector %>' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="100px" />
                    <ItemStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAuditDateTime">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkAuditDateTime" runat="server" SortExpression="LastUpdatedDateTime"
                                LabelKey="inspection_statushistorylist_auditdate" CausesValidation="false" CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblAuditDate" runat="server" Text='<%# ((InspectionViewModel)Container.DataItem).LastUpdatedDateTimeText %>' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                    <ItemStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAuditID">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkAuditID" runat="server" SortExpression="LastUpdatedBy"
                                LabelKey="inspection_statushistorylist_updatedby" CausesValidation="false" CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblAuditID" runat="server" Text='<%# ((InspectionViewModel)Container.DataItem).LastUpdatedBy%>' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="100px" />
                    <ItemStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkResultComments">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkResultComments" runat="server" SortExpression="ResultComments"
                                LabelKey="inspection_statushistorylist_resultcomments" CausesValidation="false" CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblResultComments" EnableEllipsis="true" runat="server" Text='<%# ((InspectionViewModel)Container.DataItem).ResultComments%>' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                    <ItemStyle Width="150px" />
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
    </ContentTemplate>
</asp:UpdatePanel>
