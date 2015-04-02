<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.FeeList" Codebehind="FeeList.ascx.cs" %>
<script type="text/javascript">
    function feesSection_report_onclick(url)
    {
        hideMessage();
	    var a=window.open(url, "_blank","top=200,left=200,height=600,width=800,status=yes,toolbar=1,menubar=no,location=no,scrollbars=yes");
    }      
</script>
<asp:UpdatePanel ID="LicenseEditPanel" runat="server" UpdateMode="conditional">
 <ContentTemplate>
<div id="divFeeDetailSection" runat="server" visible="false">

    <div id="divOutstanding" runat="server" visible="false">
        <div class="ACA_TabRow">
            <h1>
                <i>
                    <ACA:AccelaLabel ID="AccelaLabel2" LabelKey="per_feeDetails_label_outstandingList"
                        runat="server"></ACA:AccelaLabel>
                </i>
            </h1>
        </div>
        <ACA:AccelaGridView ID="gdvFeeUnpaidList" runat="server" AllowPaging="true" AllowSorting="True" 
            SummaryKey="aca_summary_fee_unpaid" CaptionKey="aca_caption_fee_unpaid"
            AutoGenerateColumns="False" Width="738px" PageSize="5" PagerStyle-HorizontalAlign="center"
            OnRowCommand="Payfees_RowCommand" PagerStyle-VerticalAlign="bottom">
            <Columns>
                <ACA:AccelaTemplateField>
                    <headertemplate>
                    </headertemplate>
                <itemstyle CssClass="ACA_AlignLeftOrRight"/>
                <headerstyle CssClass="ACA_AlignLeftOrRight"/>
                    <itemtemplate></itemtemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField>
                    <headertemplate>
            <div class="Header_h4 ACA_NShot">
            <ACA:AccelaLabel id="lnkFeeUnpaidDateHeader" runat="server" LabelKey="per_feeDetails_label_unpaidDate" />
            </div>
            </headertemplate>
                <itemstyle CssClass="ACA_AlignLeftOrRight"/>
                <headerstyle CssClass="ACA_AlignLeftOrRight"/>
                    <itemtemplate>
            <div class="ACA_NShot">
                    <ACA:AccelaDateLabel id="lblFeeUnpaidDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "Date")%>'></ACA:AccelaDateLabel>
                </div>
                </itemtemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField>
                    <headertemplate>
            <div class="Header_h4 ACA_Medium">
            <ACA:AccelaLabel  ID="lnkFeeUnpaidInvoiceNbrHeader" runat="server" LabelKey="per_feeDetails_label_unpaidInvoiceNbr" />
            </div>
            </headertemplate>
                <itemstyle CssClass="ACA_AlignLeftOrRight"/>
                <headerstyle CssClass="ACA_AlignLeftOrRight"/>
                    <itemtemplate>
                <div class="ACA_Medium">
                    <ACA:AccelaLabel ID="lblFeeUnpaidInvoiceNbr" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceNbr") %>' />
                </div>
                </itemtemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField>
                    <headertemplate>
            <div class="Header_h4 ACA_Medium">
            <ACA:AccelaLabel ID="lnkFeeUnpaidAmountHeader" runat="server" LabelKey="per_feeDetails_label_unpaidAmount" />
            </div>
            </headertemplate>
                <itemstyle CssClass="ACA_AlignLeftOrRight"/>
                <headerstyle CssClass="ACA_AlignLeftOrRight"/>
                    <itemtemplate>
                <div class="ACA_Medium">
                    <ACA:AccelaLabel ID="lblFeeUnpaidAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount") %>' />
                </div>
                </itemtemplate>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField>
                    <headertemplate>
                <div class="Header_h4 ACA_NLonger">
                </div>
            </headertemplate>
                    <itemtemplate>
            <div class="ACA_NLonger">
            <ACA:AccelaLinkButton ID="lblPayFeesOperation"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Operation")%>' CommandName="payfees" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "InvoiceNbr")%>' />
             </div>
            </itemtemplate>
                <itemstyle CssClass="ACA_AlignLeftOrRight"/>
                <headerstyle CssClass="ACA_AlignLeftOrRight"/>
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
        <div class="ACA_TabRow ACA_SmLabel ACA_SmLabel_FontSize">
            <strong><i>
                <ACA:AccelaLabel ID="per_feeDetails_label_unpaidTotal" LabelKey="per_feeDetails_label_unpaidTotal"
                    runat="server" />
                <ACA:AccelaLabel ID="lblUnpaidFeeAmount" runat="server" />
            </i></strong>
        </div>
    </div>
    <div id="divPaid" runat="server" visible="false">
        <div class="ACA_TabRow">
            <h1>
                <i>
                    <ACA:AccelaLabel ID="AccelaLabel3" LabelKey="per_feeDetails_label_paidList" runat="server"
                        Text="Paid:"></ACA:AccelaLabel>
                </i>
            </h1>
        </div>
        <ACA:AccelaGridView ID="gdvFeePaidedList" runat="server" AllowPaging="True" AllowSorting="true" 
            SummaryKey="aca_summary_fee_paid" CaptionKey="aca_caption_fee_paid"
            AutoGenerateColumns="False" Width="738px"
            PageSize="5" PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom" OnRowCommand="ViewDetails_RowCommand">
            <Columns>
                <ACA:AccelaTemplateField>
                    <headertemplate>
            </headertemplate>
                    <itemtemplate>
                </itemtemplate>
                <itemstyle CssClass="ACA_AlignLeftOrRight"/>
                <headerstyle CssClass="ACA_AlignLeftOrRight"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField>
                    <headertemplate>
            <div class="Header_h4 ACA_NShot" >
            <ACA:GridViewHeaderLabel ID="lnkFeePaidDateHeader" runat="server" LabelKey="per_feeDetails_label_paidDate" CommandName="Header" SortExpression="Date"/>
            </div>
            </headertemplate>
                    <itemtemplate>
            <div class="ACA_NShot">
                    <ACA:AccelaDateLabel id="lblFeePaidDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "Date")%>'></ACA:AccelaDateLabel>
                </div>
            </itemtemplate>
                <itemstyle CssClass="ACA_AlignLeftOrRight"/>
                <headerstyle CssClass="ACA_AlignLeftOrRight"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField>
                    <headertemplate>
            <div class="Header_h4 ACA_Medium" >
            <ACA:GridViewHeaderLabel ID="lnkFeePaidInvoiceNbrHeader" runat="server" LabelKey="per_feeDetails_label_paidInvoiceNbr" CommandName="Header" SortExpression="InvoiceNbr"/>
            </div>
            </headertemplate>
                    <itemtemplate>
                <div class="ACA_Medium">
                    <ACA:AccelaLabel ID="lblFeePaidInvoiceNbr" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceNbr") %>'/>
                </div>
            </itemtemplate>
                <itemstyle CssClass="ACA_AlignLeftOrRight"/>
                <headerstyle CssClass="ACA_AlignLeftOrRight"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField>
                    <headertemplate>
            <div class="Header_h4 ACA_Medium" >
            <ACA:AccelaLabel ID="lnkFeePaidAmountHeader" runat="server" LabelKey="per_feeDetails_label_paidAmount" />
            </div>
            </headertemplate>
                    <itemtemplate>
                <div class="ACA_Medium">
                    <ACA:AccelaLabel ID="lblFeePaidAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount") %>' />
                </div>
            </itemtemplate>
                <itemstyle CssClass="ACA_AlignLeftOrRight"/>
                <headerstyle CssClass="ACA_AlignLeftOrRight"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField>
                    <headertemplate>
                <div class="Header_h4 ACA_NLonger">
                </div>
            </headertemplate>
                    <itemtemplate>
            <div class="ACA_NLonger">
            <ACA:AccelaLinkButton ID="lblViewDetailsOperation"  runat="server" ToolTip='<%# DataBinder.Eval(Container.DataItem, "ToolTipInfo") %>'  href='<%# DataBinder.Eval(Container.DataItem, "URL") %>'  Text='<%# DataBinder.Eval(Container.DataItem, "Operation") %>' />
            </div>
            </itemtemplate>
                <itemstyle CssClass="ACA_AlignLeftOrRight"/>
                <headerstyle CssClass="ACA_AlignLeftOrRight"/>
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
        <div class="ACA_TabRow ACA_SmLabel ACA_SmLabel_FontSize">
            <strong><i>
                <ACA:AccelaLabel ID="per_feeDetails_label_paidTotal" LabelKey="per_feeDetails_label_paidTotal"
                    runat="server" />
                <ACA:AccelaLabel ID="lblPaidedFeeAmount" runat="server" />
            </i></strong>
        </div>
    </div>
</div>
</ContentTemplate>
</asp:UpdatePanel>