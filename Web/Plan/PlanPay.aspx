<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" Inherits="Accela.ACA.Web.Plan.PlanPay"  Title="Self Plan Check" Codebehind="PlanPay.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" Runat="Server">  
    <!-- No HTML formatting tags except within designated custom content areas. Indent to indicate nesting levels. -->
    <asp:HiddenField ID="hdnChargedAmount" runat="server"  />
    <asp:HiddenField ID="hdnPlanSeqNbr" runat="server"  />
    <div id="MainContent" class="ACA_Content">
        <!-- begin content sub-block -->
        <!-- begin custom content -->
        <h1>
            <ACA:AccelaLabel  ID="planreview_fee_hdr" LabelKey="planreview_fee_hdr" runat ="server" ></ACA:AccelaLabel>
        </h1>
        <br />
        <p>
            <ACA:AccelaLabel ID="planreview_fee_subhdr" LabelKey="planreview_fee_subhdr" runat ="server" ></ACA:AccelaLabel>
        </p>
        <div id="divTitile">
            <div class="ACA_TabRow">
                <span class="ACA_SmLabel ACA_SmLabel_FontSize ACA_FRight"><span class="ACA_Required_Indicator">*</span>
                    <ACA:AccelaLabel ID="per_permitPayFee_label_indicate" LabelKey="per_permitPayFee_label_indicate"
                        runat="server" />
                </span>
            </div>
            <div class="ACA_InfoTitle ACA_InfoTitle_FontSize">
                <h1>
                    <div class="ACA_FLeft">
                        <ACA:AccelaLabel ID="planreview_fee_paywithcard" LabelKey="planreview_fee_paywithcard" runat="server" />
                    </div>
                </h1>
            </div>
            <div class="Header_h3">
                <ACA:AccelaLabel ID="per_permitPayFee_label_amount" LabelKey="per_permitPayFee_label_amount" runat="server" />&nbsp;<ACA:AccelaLabel ID="lblChargedAmount" runat="server" ></ACA:AccelaLabel>
            </div>
        </div>
        <br />
        <div class="ACA_TabRow ACA_LiLeft">
            <h1><ACA:AccelaLabel ID="per_permitPayFee_label_CreditCardInfo" LabelKey="per_permitPayFee_label_CreditCardInfo" runat="server" /></h1>
        </div>
        <div class="ACA_TabRow ACA_LiLeft">
            <ul>
                <li><ACA:AccelaDropDownList ID="ddlCardType" LabelKey="per_permitPayFee_label_cardTypePay" runat="server" Required="true"/></li>
                <li><ACA:AccelaTextBox ID="txtCardNumber" LabelKey="per_permitPayFee_label_cardNumPay"  CssClass="ACA_NLonger" runat="server" Validate="required" MaxLength="20"></ACA:AccelaTextBox></li>
                <li><ACA:AccelaTextBox ID="txtCCV" CssClass="field_short" runat="server" Validate="required" LabelKey="per_permitPayFee_label_ccvPay" MaxLength="4"></ACA:AccelaTextBox></li>
            
            </ul>
            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                TargetControlID="txtCCV" FilterType="Numbers" />
        </div>
        <div class="ACA_TabRow ACA_LiLeft">
            <ul>
                <li><ACA:AccelaTextBox ID="txtCardName" CssClass="ACA_NLonger" runat="server" Validate="required"  LabelKey="per_permitPayFee_label_cardNamePay"  MaxLength="80"></ACA:AccelaTextBox></li>
                <li><span class="ACA_Required_Indicator">*</span>
                    <ACA:AccelaLabel ID="per_permitPayFee_label_expAndDatePay" CssClass="ACA_Label ACA_Label_FontSize" LabelKey="per_permitPayFee_label_expAndDatePay" runat ="server" ></ACA:AccelaLabel>
                    <div style="margin-top:5px">
                    <table role="presentation" class='ACA_TDAlignLeftOrRightTop' border="0" cellspacing="0" cellpadding="0">
                       <tr>
                       <td>
                        <ACA:AccelaDropDownList runat="server" ID="ddlExpMonth" ></ACA:AccelaDropDownList>
                        </td><td>
                        <ACA:AccelaDropDownList runat="server" ID="ddlExpYear"></ACA:AccelaDropDownList>
                        </td>
                       </tr>
                       </table>
                    </div>
                </li>
            </ul>
        </div>
        <div class="ACA_TabRow ACA_LiLeft">
            <h1><ACA:AccelaLabel ID="per_permitPayFee_label_ccDefaultContactInfo" LabelKey="per_permitPayFee_label_ccDefaultContactInfo" runat="server" /></h1>
        </div>
        <div class="ACA_TabRow ACA_LiLeft">
            <ACA:AccelaTextBox ID="txtStreetAdd1" CssClass="ACA_XLong" runat="server" Validate="required"
                LabelKey="per_permitPayFee_label_street1Pay" MaxLength="40">
            </ACA:AccelaTextBox>
        </div>
        <div class="ACA_TabRow ACA_LiLeft" runat="server" visible="false">
            <ACA:AccelaTextBox ID="txtStreetAdd2" CssClass="field_xlong" runat="server" 
                LabelKey="per_permitPayFee_label_street2Pay" MaxLength="40">
            </ACA:AccelaTextBox>
        </div>
        <div class="ACA_TabRow ACA_LiLeft">
            <ul>
                <li><ACA:AccelaTextBox ID="txtCity" CssClass="ACA_Medium" runat="server" Validate="required"
                LabelKey="per_permitPayFee_label_cityPay" MaxLength="30"></ACA:AccelaTextBox></li>
                <li><ACA:AccelaDropDownList ID="ddlState" LabelKey="per_permitPayFee_label_stateFee" runat="server" Required="true"></ACA:AccelaDropDownList></li>
                <li><ACA:AccelaZipText ID="txtZip" LabelKey ="per_permitPayFee_label_zipPay"  CssClass="ACA_NShot" runat="server"></ACA:AccelaZipText></li>
            </ul>
        </div>
        <br />
        <script type="text/javascript">
            if (isFireFox())
            {
                document.write('<br /><br />');
            }
        </script>
        <div class="ACA_TabRow ACA_LiLeft">

            <div class="ACA_TabRow ACA_LgButton ACA_LgButton_FontSize">
                        <ACA:AccelaLinkButton ID="lnkSubmitPayment" runat="server" OnClick="SubmitPaymentButton_OnClick"
                            LabelKey="per_permitPayFee_label_sumbitPaymentPay"></ACA:AccelaLinkButton>
                    </div>
            <div class="form_element_sub_label">
                 <ACA:AccelaLabel ID="per_permitPayFee_sublabel_sumbitPaymentPay" LabelKey="per_permitPayFee_sublabel_sumbitPaymentPay" runat ="server" ></ACA:AccelaLabel>
            </div>
        </div>
    </div>
    <!-- end content sub-block -->

</asp:Content>

