<%@ Page Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Plan.PlanFees"
    MasterPageFile="~/Default.master" ValidateRequest="false" Codebehind="PlanFees.aspx.cs" %>
<%@ Register Src="~/Component/PlanFeeItem.ascx" TagName="PlanFeeItem" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <script type="text/javascript">
    window.history.forward(1);     
    </script>

    <script type="text/javascript">    
    function applyCoupon()
    {
        var code = document.getElementById("<%= txtCouponCode.ClientID %>");
        
        return code != null && code.value != '';
    }
    
    function clickButton(e) 
    {
        var keynum;
        if(window.event) // IE
        {
            keynum = e.keyCode;
        }
        else if(e.which) {
            keynum = e.which;
        }    

        if (keynum == 13) 
        {
            return false;
        }
    }
    </script>

    <div class="ACA_Container">
        <ACA:AccelaLabel ID="AccelaLabel3" runat="server" LabelKey="planreview_planfees_label_payfees" />
        <div class="ACA_Page ACA_Page_FontSize">
            <ACA:AccelaLabel ID="planreview_planfees_label_planfeesnote" runat="server" LabelKey="planreview_planfees_label_planfeesnote"
                LabelType="BodyText" /></div>
        <br />        
        <ACA:AccelaLabel ID="planreview_planfees_label_planfees" runat="server" LabelKey="planreview_planfees_label_planfees" />
        <div>
            <asp:DataList ID="planList" runat="server" EnableViewState="true" OnItemDataBound="PlanList_ItemDataBound" role='presentation'>
                <ItemTemplate>
                    <!--   list all Agences  -->
                    <uc1:PlanFeeItem ID="PlanFeeItem" runat="server"/>
                </ItemTemplate>
            </asp:DataList>
            <div class="ACA_Line_Content ACA_RLong">
                &nbsp;
            </div>
            <div class="ACA_TabRow">
                <span class="ACA_SmLabel ACA_SmLabel_FontSize">
                    <div>
                        <strong>
                            <ACA:AccelaLabel ID="planreview_planfees_label_discount" LabelKey="planreview_planfees_label_discount"
                                runat="server" />
                        </strong>
                        <br />
                        <ACA:AccelaLabel ID="planreview_planfees_label_discountnote" LabelKey="planreview_planfees_label_discountnote"
                            runat="server" />
                    </div>
                </span>
            </div>
            <div class="ACA_TabRow">
                <span class="ACA_FRight ACA_SmLabel ACA_SmLabel_FontSize">
                    <table role="presentation" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="text-align:center; padding-right:10px">
                                <ACA:AccelaLabel ID="planreview_planfees_label_couponcode" LabelKey="planreview_planfees_label_couponcode" runat="server" /><br />
                                <ACA:AccelaLabel ID="planreview_planfees_label_optional" LabelKey="planreview_planfees_label_optional" runat="server" />
                            </td>
                            <td style="text-align:center">
                                <ACA:AccelaTextBox ID="txtCouponCode" runat="server" size="15"/>
                                <div class="ACA_SmButton ACA_SmButton_FontSize ACA_Button_Text">
                                    <ACA:AccelaLinkButton ID="lnkApplyCoupon" LabelKey="planreview_planfees_label_applycoupon"
                                        runat="server" OnClick="ApplyCouponButton_Click" OnClientClick="javascript: return applyCoupon();" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </span>
            </div><br />
            <div class="ACA_TabRow">
                <span class="ACA_SmLabel ACA_SmLabel_FontSize">
                    <div>
                        <strong>
                            <ACA:AccelaLabel ID="planreview_planfees_label_totalfees" LabelKey="planreview_planfees_label_totalfees"
                                runat="server" />
                        </strong>
                        <br />
                        <ACA:AccelaLabel ID="planreview_planfees_label_totalfeesnote" LabelKey="planreview_planfees_label_totalfeesnote"
                            runat="server" />
                        <br />
                        <br />
                    </div>
                </span>
            </div>
            <table role="presentation" class="ACA_Title_Bar" style="height: 35px; margin-top: 6px;" runat="server" id="tablePlanSum">
                <tr style="padding-left: -10px;">
                    <td class="ACA_XxShot">
                        &nbsp;</td>
                    <td class="ACA_Column_XLong">            
                        <h1>
                            <ACA:AccelaLabel ID="planreview_planfees_graybox_totalfees" LabelKey="planreview_planfees_graybox_totalfees" runat="server"></ACA:AccelaLabel>
                        </h1>                    
                    </td>
                    <td>
                        <div class="ACA_Column_Short" style="width: 70px;"> &nbsp;</div>
                    </td>
                    <td style="float: none">
                        <div class="ACA_TabRow ACA_RightColumn_Short">
                            <div class="ACA_FRight Header_h3">
                                <ACA:AccelaLabel ID="lblFeeAmount" runat="server" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        </div>
        <div class="ACA_Row fee_button_container">
            <div class="ACA_TabRow">
                &nbsp;</div>

            <div class="ACA_LgButton ACA_LgButton_FontSize"> <ACA:AccelaLinkButton ID="lnkContinueApplication" LabelKey="per_permitFee_label_continueApp"
                                runat="server" OnClick="ContinueApplicationButton_Click" OnClientClick="SetNotAsk(true)" /></div>
        </div>
</asp:Content>
