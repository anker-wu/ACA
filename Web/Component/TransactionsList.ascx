<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Util" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionsList.ascx.cs"
    Inherits="Accela.ACA.Web.Component.TransactionsList" %>
<asp:UpdatePanel ID="TrustAccountListPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvTransactionList" runat="server" AllowPaging="true" AllowSorting="True"
            GridViewNumber="60107" Width="77em" SummaryKey="gdv_transactionlist_summary" CaptionKey="aca_caption_transactionlist"
            ShowCaption="true" AutoGenerateColumns="False" BorderWidth="0px" PagerStyle-HorizontalAlign="center"
            OnRowCommand="TransactionList_RowCommand"
            PagerStyle-VerticalAlign="bottom" ShowExportLink="true">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkID">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkID" runat="server" LabelKey="per_transaction_id"
                                SortExpression="TransID" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.TransID.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAccountID">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkAccountID" runat="server" LabelKey="per_transaction_accountid"
                                SortExpression="AccountID" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAccountID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.AccountID.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkTransType">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkTransType" runat="server" LabelKey="per_transaction_type"
                                SortExpression="TransType" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblTransType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.TransType.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkTransAmount">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkTransAmount" runat="server" LabelKey="per_transaction_amount"
                                SortExpression="TransAmount" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblTransAmount" runat="server" Text='<%# I18nNumberUtil.FormatMoneyForUI(DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.TransAmount.ToString())) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkTargetAccountID">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkTargetAccountID" runat="server" LabelKey="per_transaction_target_accountid"
                                SortExpression="TargetAccountID" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblTargetAccountID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.TargetAccountID.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="120px" />
                    <HeaderStyle Width="120px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkRecordID">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkRecordID" runat="server" LabelKey="per_transaction_record_id"
                                SortExpression="RecordID" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblRecordID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.RecordID.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="110px" />
                    <HeaderStyle Width="110px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkALTID">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkALTID" runat="server" LabelKey="per_transaction_altid"
                                SortExpression="ALTID" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblALTID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.ALTID.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkClientTransNumber">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkClientTransNumber" runat="server" LabelKey="per_transaction_client_trans_number"
                                SortExpression="ClientTransNumber" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblClientTransNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.ClientTransNumber.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkClientReceiptNumber">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkClientReceiptNumber" runat="server" LabelKey="per_transaction_client_receipt_number"
                                SortExpression="ClientReceiptNumber" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblClientReceiptNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.ClientReceiptNumber.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkOfficeCode">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkOfficeCode" runat="server" LabelKey="per_transaction_office_code"
                                SortExpression="OfficeCode" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblOfficeCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.OfficeCode.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkTransDate">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkTransDate" runat="server" LabelKey="per_transaction_date"
                                SortExpression="TransDate" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaDateLabel ID="lblTransDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.TransDate.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkDepositMethod">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkDepositMethod" runat="server" LabelKey="per_transaction_deposit_method"
                                SortExpression="DepositMethod" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblDepositMethod" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.DepositMethod.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="120px" />
                    <HeaderStyle Width="120px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkTransactionCode">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkTransactionCode" runat="server" LabelKey="per_transaction_code"
                                SortExpression="TransactionCode" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblTransactionCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.TransactionCode.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="120px" />
                    <HeaderStyle Width="120px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkCashDrawerID">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkCashDrawerID" runat="server" LabelKey="per_transaction_cash_drawerid"
                                SortExpression="CashDrawerID" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblCashDrawerID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.CashDrawerID.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="120px" />
                    <HeaderStyle Width="120px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkComments">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkComments" runat="server" LabelKey="per_transaction_comments"
                                SortExpression="Comments" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblComments" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.Comments.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkCustomizedReceiptNumber">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkCustomizedReceiptNumber" runat="server" LabelKey="per_transaction_customized_receipt_number"
                                SortExpression="CustomizedReceiptNumber" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblCustomizedReceiptNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.CustomizedReceiptNumber.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="200px" />
                    <HeaderStyle Width="200px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkReferenceNumber">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkReferenceNumber" runat="server" LabelKey="per_transaction_reference_number"
                                SortExpression="ReferenceNumber" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblReferenceNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.ReferenceNumber.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkCCAuthCode">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkCCAuthCode" runat="server" LabelKey="per_transaction_cc_authcode"
                                SortExpression="CCAuthCode" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblCCAuthCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.CCAuthCode.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkPayor">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkPayor" runat="server" LabelKey="per_transaction_payor"
                                SortExpression="Payor" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblPayor" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.Payor.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkReceived">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkReceived" runat="server" LabelKey="per_transaction_received"
                                SortExpression="Received" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblReceived" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Transaction.Received.ToString()) %>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
    </ContentTemplate>
</asp:UpdatePanel>
