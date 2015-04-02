<%@ Import Namespace="Accela.ACA.Common.Util" %>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.PlanFeeItem" Codebehind="PlanFeeItem.ascx.cs" %>

<!--   list all Agences  -->
<table role='presentation' class="ACA_Title_Bar" style="height: 35px; margin-top: 6px;" runat="server" id="tablePlanSum">
    <tr style="padding-left: -10px;">
        <td class="ACA_XxShot">
            &nbsp;</td>
        <td class="ACA_Column_XLong">            
            <h1>
                <ACA:AccelaLabel ID="lblPlanName" runat="server"></ACA:AccelaLabel>
            </h1>                    
        </td>
        <td>
            <div class="ACA_Column_Short" style="width: 70px;"> &nbsp;</div>
        </td>
        <td style="float: none">
            <div class="ACA_TabRow ACA_RightColumn_Short">
                <div class="ACA_FRight">
                    <div class="Header_h3"><ACA:AccelaLabel ID="lblPlanTotalFee"  runat="server"></ACA:AccelaLabel></div>
                </div>
            </div>
        </td>
    </tr>
</table>
<!-- list fee item field headers under plan -->
<table role='presentation' border="0" cellpadding="0" cellspacing="0">
    <tr class="ACA_BkTit ACA_TabTitle" style="padding-left: -10px;">
        <td class="ACA_XxShot">
            &nbsp;</td>
        <td class="ACA_Column_XLong">
            <div class="Header_h4">
                <ACA:AccelaLabel ID="per_feeItemList_label_fee" LabelKey="per_feeItemList_label_fee" runat="server" />
            </div>
        </td>
        <td>
            <div class="Header_h4 ACA_Column_Short" style="width: 70px;"> 
                <ACA:AccelaLabel ID="per_feeItemList_label_qty" LabelKey="per_feeItemList_label_qty" runat="server" />
            </div>
        </td>
        <td width="8px" id="flexibleTD" runat="server">&nbsp;</td>
        <td class="ACA_RightColumn_Short" style="float: none;">
            <div class="Header_h4 ACA_FRight">
                <ACA:AccelaLabel ID="per_feeItemList_label_amount" LabelKey="per_feeItemList_label_amount" runat="server" />
            </div>
        </td>
        <td width="5px">&nbsp;</td>
    </tr>
</table>        
<!-- List every plan Fee Items -->
<asp:DataList ID="feeItemList" runat="server" EnableViewState="true" OnItemDataBound="FeeList_ItemDataBound" role='presentation'>
    <ItemTemplate>
        <table role='presentation' class='ACA_TDAlignLeftOrRightTop'border="0" cellpadding="0" cellspacing="0">
            <tr class="ACA_TabRow" style="padding-left: -10px;">
                <td id="flexibleTD" runat="server" style="width:25px" visible="false">&nbsp;</td>
                <td style="display: none">
                    <asp:HiddenField ID="hdnFeeSeq" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "feeSeqNbr")%>' />
                    <asp:HiddenField ID="hdnStatus" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "feeitemStatus")%>' />
                    <asp:HiddenField ID="hdnAutoInvoice" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "autoInvoiceFlag")%>' />
                </td>
                <td class="ACA_Column_XLong" style="width: 273px;">
                    <ACA:AccelaLabel ID="lblItemDescription" runat="server" Text='<%#GetFeeDescription(DataBinder.Eval(Container.DataItem, "resFeeDescription"),DataBinder.Eval(Container.DataItem, "feeDescription"))%>' />
                </td>
                <td>
                    <div class="ACA_Column_Short" style="width: 70px;">
                        <ACA:AccelaLabel ID="lblQuantity" runat="server" Text='<%#I18nNumberUtil.ConvertDecimalForUI(DataBinder.Eval(Container.DataItem, "feeUnit").ToString())%>' />
                    </div>
                </td>
                <td>
                    <div class="ACA_RightColumn_Short">
                        <span class="ACA_FRight">
                            <ACA:AccelaLabel ID="lblAmount" runat="server" Text='<%# I18nNumberUtil.FormatMoneyForUI(DataBinder.Eval(Container.DataItem, "fee"))%>' /></span>
                    </div>
                </td>
            </tr>
        </table>
    </ItemTemplate>
</asp:DataList>
