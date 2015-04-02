<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExaminationFeeItemTemplate.ascx.cs" Inherits="Accela.ACA.Web.Component.ExaminationFeeItemTemplate" %>
<%@ Import Namespace="Accela.ACA.Common.Util" %>

<div class="ExaminationFeeItem">
    <ACA:AccelaGridView runat="server" Width="100%" ID="feeItemList"
        AutoGenerateColumns="false" PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom"
            ShowHorizontalScroll="false" SummaryKey="aca_capfee_summary" CaptionKey="aca_caption_examination_feelist"
            HeaderStyle-CssClass="ACA_TabRow_Even_Auto ACA_BkTit ACA_TabTitle"
            AlternatingRowStyle-CssClass="ACA_TabRow_Even_Auto" RowStyle-CssClass="ACA_TabRow_Odd_Auto">
        <Columns>
            <ACA:AccelaTemplateField>
                <HeaderTemplate>
                    <div class="ExaminationFeeItem_feeitemdesc ACA_Label_FontSize_Smaller">
                            <ACA:AccelaLabel ID="per_feeItemList_label_fee" LabelKey="per_feeItemList_label_fee"
                                runat="server" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="ExaminationFeeItem_feeitemdesc">
                    <table role="presentation">
                    <tr>
                    <td><ACA:AccelaLabel ID="lblPlaceHolder" runat="server" Visible="false" Width="16px"></ACA:AccelaLabel></td>
                    <td><ACA:AccelaLabel ID="lblItemDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "feeDesc") %>' /></td>
                    </tr>
                    </table>
                    </div>
                </ItemTemplate>
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField>
                <HeaderTemplate>
                        <div class="ACA_FRight ACA_Label_FontSize_Smaller">
                            <ACA:AccelaLabel ID="per_feeItemList_label_amount" LabelKey="per_feeItemList_label_amount"
                                runat="server" />
                        </div>
                </HeaderTemplate>
                <ItemTemplate>
                        <div>
                            <ACA:AccelaNumberLabel NumberLabelType="Money" ID="lblAmount" runat="server" NumericText='<%# DataBinder.Eval(Container.DataItem, "feeAmount") %>' CssClass="ACA_FRight" />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="130px"/>
                <headerstyle Width="130px"/>
            </ACA:AccelaTemplateField>
        </Columns>
    </ACA:AccelaGridView>
    <div class="ACA_Line_Content ACA_TabRow">
        &nbsp;
    </div>
    <div class="ACA_TabRow">
        <div class="ACA_SmLabel">
            <table role="presentation">
                <tr>
                    <td class="ExaminationFeeItem_totalfee">
                       <div>
                            <strong>
                                <ACA:AccelaLabel ID="per_feeItemList_label_totalFee" LabelKey="per_feeItemList_label_totalFee" runat="server" />
                            </strong>
                        </div>
                    </td>
                    <td>
                        <div class="ACA_RightColumn_Short">
                            <span class="ACA_FRight">
                                <strong><ACA:AccelaNumberLabel NumberLabelType="Money" ID="lblFeeAmount" runat="server" /></strong>
                            </span>
                        </div>  
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>