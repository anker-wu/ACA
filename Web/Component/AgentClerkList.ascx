<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgentClerkList.ascx.cs" Inherits="Accela.ACA.Web.Component.AgentClerkList" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Common" %>

<ACA:AccelaGridView ID="gdvClerkList" AllowPaging="true" AllowSorting="True" GridViewNumber="60151" 
    ShowCaption="true" AutoGenerateColumns="False" PagerStyle-HorizontalAlign="center"
    runat="server" PagerStyle-VerticalAlign="bottom" SummaryKey="aca_authagent_clerklist_summary"
    CaptionKey="aca_caption_authagent_clerklist"
    IsInSPEARForm="true" OnRowCommand="ClerkList_RowCommand" OnRowDataBound="ClerkList_RowDataBound">
    <Columns>
        <ACA:AccelaTemplateField AttributeName="lnkUserName">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkUserName" runat="server" LabelKey="aca_authagent_clerklist_label_username"
                        SortExpression="userID" CommandName="Header" CausesValidation="false" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <asp:LinkButton role='presentation' ID="btnUserName" runat="server" CausesValidation="false" CommandName="edit"
                        CommandArgument='<%#DataBinder.Eval(Container.DataItem, "userSeqNum") %>'>
                        <strong><%#DataBinder.Eval(Container.DataItem, "userID")%></strong></asp:LinkButton>
                </div>
            </ItemTemplate>
            <ItemStyle Width="120px" />
            <HeaderStyle Width="120px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkEmail">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkEmail" runat="server" LabelKey="aca_authagent_clerklist_label_email"
                        SortExpression="email" CommandName="Header" CausesValidation="false" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblEmail" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "email")%>' />
                </div>
            </ItemTemplate>
            <ItemStyle Width="410px" />
            <HeaderStyle Width="410px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkStatus">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkStatus" runat="server" LabelKey="aca_authagent_clerklist_label_status"
                        SortExpression="status" CommandName="Header" CausesValidation="false" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblStatus" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "status") %>' />
                </div>
            </ItemTemplate>
            <ItemStyle Width="120px" />
            <HeaderStyle Width="120px" />
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkAction">
            <HeaderTemplate>
                <div>
                    <ACA:AccelaLabel ID="lnkAction" runat="server" LabelKey="aca_authagent_clerklist_label_action"
                        IsGridViewHeadLabel="true"></ACA:AccelaLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="ACA_FLeft">
                    <asp:LinkButton role='presentation' ID="btnAction" runat="server" CommandName="ChangeStatus" CausesValidation="false" 
                        CommandArgument='<%#DataBinder.Eval(Container.DataItem, "userSeqNum") %>'></asp:LinkButton>
                </div>
            </ItemTemplate>
            <ItemStyle Width="120px" />
            <HeaderStyle Width="120px" />
        </ACA:AccelaTemplateField>
    </Columns>
</ACA:AccelaGridView>