<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.TrustAccountList"
    CodeBehind="TrustAccountList.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Util" %>
<asp:UpdatePanel ID="TrustAccountListPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvTrustAcctList" runat="server" AllowPaging="true" AllowSorting="True"
            SummaryKey="gdv_trustaccountlist_summary" CaptionKey="aca_caption_trustaccountlist" ShowCaption="true" AutoGenerateColumns="False"
            BorderWidth="0px" PagerStyle-HorizontalAlign="center" OnRowCommand="TrustAccountList_RowCommand" 
            OnRowDataBound="TrustAccountList_RowDataBound" PagerStyle-VerticalAlign="bottom" ShowExportLink="true">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkAcctID">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkAcctID" runat="server" LabelKey="per_trustaccountlist_accountid" 
                                SortExpression="AccountID" CommandName="Header" CausesValidation="false" OnClientClick="SetNotAsk()" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <strong>
                                <ACA:AccelaLinkButton ID="btnAcctID" runat="server" CommandName="SelectTrustAccountID"
                                    CausesValidation="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.TrustAccount.AccountID.ToString()) %>'
                                    Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.TrustAccount.AccountID.ToString()) %>' />
                                 <ACA:AccelaLabel ID="lblAcctID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.TrustAccount.AccountID.ToString()) %>' Visible="false" />   
                             </strong>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkPrimary">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkPrimary" runat="server" LabelKey="per_trustaccountlist_primary"
                                SortExpression="Primary" CommandName="Header" CausesValidation="false" OnClientClick="SetNotAsk()"/>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblPrimary" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.TrustAccount.Primary.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="50px" />
                    <HeaderStyle Width="50px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkBalance">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkBalance" runat="server" LabelKey="per_trustaccountlist_balance"
                                SortExpression="Balance" CommandName="Header" CausesValidation="false" OnClientClick="SetNotAsk()"/>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblBalance" runat="server" Text='<%# I18nNumberUtil.FormatMoneyForUI(DataBinder.Eval(Container.DataItem, ColumnConstant.TrustAccount.Balance.ToString())) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkDescription">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkDescription" runat="server" LabelKey="per_trustaccountlist_description"
                                SortExpression="Description" CommandName="Header" CausesValidation="false" OnClientClick="SetNotAsk()"/>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.TrustAccount.Description.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkStatus">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkStatus" runat="server" LabelKey="per_trustaccountlist_status"
                                SortExpression="Status" CommandName="Header" CausesValidation="false" OnClientClick="SetNotAsk()"/>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.TrustAccount.Status.ToString()).ToString() %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="InkLedgerAccount">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="InkLedgerAccount" runat="server"
                                LabelKey="per_trustaccountlist_ledger_account" SortExpression="LedgerAccount"
                                CommandName="Header" CausesValidation="false" OnClientClick="SetNotAsk()" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblLedgerAccount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.TrustAccount.LedgerAccount.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lblAction">
                    <HeaderTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAction" runat="server" IsGridViewHeadLabel="true" LabelKey="per_trustaccountlist_action" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <strong>
                                <ACA:AccelaLinkButton ID="btnDeposit" runat="server" CommandName="SelectTrustAccountDeposit"
                                    CausesValidation="false" OnClientClick="SetNotAsk()" CommandArgument='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.TrustAccount.AccountID.ToString()) %>'/>
                            </strong>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
        <ACA:AccelaHeightSeparate ID="sepHeightForTrustAccountList" runat="server" Height="10" />
    </ContentTemplate>
</asp:UpdatePanel>
