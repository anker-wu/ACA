<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.FeeList"
    CodeBehind="CapFeeList.ascx.cs" %>
<%@ Register Src="~/Component/CapConditions.ascx" TagName="CapConditions" TagPrefix="ACA" %>
<%@ Import Namespace="Accela.ACA.Common.Util" %>
<script type="text/javascript">
    var feeQuantityAccuracy = <%=FeeQuantityAccuracy %>;
    function CheckDecimal() {
        if (feeQuantityAccuracy > 0) {
            return true;
        }

        var chk = true;

        $("input[expressionmapname]").each(function() {
            if (this.readOnly != true && this.value.indexOf(GLOBAL_CURRENCY_DECIMAL_SEPARATOR) > -1) {
                chk = false;
                showMessage("messageSpan", errMessage, "Error", true, 1);
                return false;
            }
        });

        return chk;
    }
</script>
<div id="divViewTotalfee" runat="server">
    <div class="ACA_FLeft ACA_RefEducation_Font ACA_SmLabel ACA_SmLabel_FontSize">
        <a href="#" class="NotShowLoading" onclick='ControlFeeDisplay()'
            title="<%=GetTitleByKey("img_alt_expand_icon", "per_feeItemList_label_totalFee") %>"
            id="lnkFeeDetail">
            <img style="cursor: pointer; border-width: 0px;" alt="<%=GetTextByKey("img_alt_expand_icon") %>"
                src="<%=ImageUtil.GetImageURL("section_header_collapsed.gif") %>"
                id="imgFeeDetail" /></a>
        <ACA:AccelaLabel ID="lblTotalFeeView" LabelKey="per_feeItemList_label_totalFee" runat="server" />
    </div>
    <div class="ACA_RightColumn_Short ACA_SmLabel ACA_SmLabel_FontSize">
        <span class="ACA_FRight"><strong>
            <ACA:AccelaLabel ID="lblFeeAmountView" runat="server" /></strong></span>
    </div>
    <div class="ACA_TabRow ACA_SmLabel ACA_SmLabel_FontSize">
        <ACA:AccelaLabel ID="lblTotalFeeNoteView" LabelKey="per_feeItemList_textl_totalFeeNote"
            runat="server" />
    </div>
    <br />
</div>
<div id="divTotalFee" runat="server">
    <div class="ACA_Page ACA_Page_FontSize">
        <ACA:AccelaLabel ID="per_permitFee_text_estimateFee" runat="server" LabelKey="per_permitFee_text_estimateFee"
            LabelType="BodyText" />
    </div>
    <div class="ACA_RightPadding">
        <ACA:AccelaButton ID="btnPrintRequirements" DivEnableCss="ACA_SmButton ACA_SmButton_FontSize"
            LabelKey="aca_report_print_requirements_label" runat="server" ReportID="0" Visible="false"
            CssClass="NotShowLoading" />
    </div>
    <ACA:AccelaHeightSeparate ID="sepForHeader" runat="server" Height="12" />
    <h2>
        <ACA:AccelaLabel ID="per_permitFee_label_appFee" runat="server" LabelKey="per_permitFee_label_appFee" />
    </h2>
    <div>
        <asp:DataList ID="agenceList" Width="100%" runat="server" EnableViewState="true" role='presentation'
            OnItemDataBound="AgenceList_ItemDataBound">
            <ItemTemplate>
                <!--   list all Agences  -->
                <table role='presentation' class="ACA_Title_Bar" style="height: 35px; margin-top: 6px;
                    padding: 0px" runat="server" id="tableAgenceSum">
                    <tr>
                        <td class="ACA_Column_XLong ACA_FloatNone">
                            <table role='presentation'>
                                <tr>
                                    <td class="ACA_XxShot">
                                        &nbsp;
                                    </td>
                                    <td id="tdLogo" class="ACA_Logo" runat="server">
                                        <img id="imgAgencyLogo" runat="server" />&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <h1>
                                            <ACA:AccelaLabel ID="lblAgenceName" Text='<%#DataBinder.Eval(Container.DataItem, "capID.serviceProviderCode")%>'
                                                runat="server"></ACA:AccelaLabel>
                                            <asp:HiddenField ID="hdnAgenceName" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "capID.serviceProviderCode")%>' />
                                        </h1>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <div class="ACA_NShot">
                            </div>
                        </td>
                        <td class="ACA_NLong" style="padding: 4px">
                            <span class="ACA_FRight">
                                <h1>
                                    <ACA:AccelaLabel ID="lblAgenceTotalFee" runat="server"></ACA:AccelaLabel></h1>
                            </span>
                        </td>
                    </tr>
                </table>
                <ACA:AccelaGridView runat="server" Width="100%" ID="feeItemList" OnRowDataBound="FeeList_RowDataBound"
                    AutoGenerateColumns="false" PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom"
                    ShowHorizontalScroll="false" SummaryKey="aca_capfee_summary" CaptionKey="aca_caption_capfee">
                    <Columns>
                        <ACA:AccelaTemplateField>
                            <HeaderTemplate>
                                <div class="ACA_Column_XLong ACA_Label_FontSize_Smaller">
                                    <ACA:AccelaLabel ID="per_feeItemList_label_fee" LabelKey="per_feeItemList_label_fee"
                                        runat="server" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="ACA_Column_XLong">
                                    <table role="presentation">
                                        <tr>
                                            <td>
                                                <ACA:AccelaLabel ID="lblPlaceHolder" runat="server" Visible="false" Width="16px"></ACA:AccelaLabel>
                                            </td>
                                            <td>
                                                <ACA:AccelaLabel ID="lblItemDescription" runat="server" Text='<%#GetFeeDescription(DataBinder.Eval(Container.DataItem, "resFeeDescription"),DataBinder.Eval(Container.DataItem, "feeDescription"))%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ItemTemplate>
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField>
                            <HeaderTemplate>
                                <div class="ACA_FRight ACA_Label_FontSize_Smaller ACA_Form">
                                    <ACA:AccelaLabel ID="per_feeItemList_label_qty" LabelKey="per_feeItemList_label_qty"
                                        runat="server" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table role='presentation' class="ACA_FRight" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td class="ACA_XxShot">
                                            <div id="divRequiredIndicator" runat="server" visible="false" class="ACA_Required_Indicator">
                                                *</div>
                                        </td>
                                        <td>
                                            <asp:HiddenField runat="server" ID="hidQuantity" Value='<%#I18nNumberUtil.ConvertDecimalForUI(DataBinder.Eval(Container.DataItem, "feeUnit").ToString())%>' />
                                            <div id="divQuantity" runat="server" class="ACA_Shot ACA_ARight ACA_Form">
                                                <ACA:AccelaLabel ID="lblQuantity" runat="server" Text='<%#I18nNumberUtil.ConvertDecimalForUI(DataBinder.Eval(Container.DataItem, "feeUnit").ToString())%>' />
                                            </div>
                                            <ACA:AccelaNumberText ID="txtQuanity" runat="server" MaxLength="16" CssClass="ACA_Shot ACA_ARight" ToolTipLabelKey="valuationcalculator_list_unitamount"
                                                onchange="SetValue(this,RoundDecimal(GetValue(this),feeQuantityAccuracy, false, true));" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <ItemStyle Width="90px" />
                            <HeaderStyle Width="90px" />
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
                                    <ACA:AccelaLabel ID="lblAmount" runat="server" Text='<%# I18nNumberUtil.FormatMoneyForUI(DataBinder.Eval(Container.DataItem, "fee"))%>'
                                        CssClass="ACA_FRight" />
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="108px" />
                            <HeaderStyle Width="110px" />
                        </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField>
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnFeeSeq" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "feeSeqNbr")%>' />
                                <asp:HiddenField ID="hdnStatus" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "feeitemStatus")%>' />
                                <asp:HiddenField ID="hdnReadOnly" runat="server" />
                                <asp:HiddenField ID="hdnCapID1" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "capID.id1")%>' />
                                <asp:HiddenField ID="hdnCapID2" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "capID.id2")%>' />
                                <asp:HiddenField ID="hdnCapID3" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "capID.id3")%>' />
                                <asp:HiddenField ID="hdnAgence" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "capID.serviceProviderCode")%>' />
                            </ItemTemplate>
                        </ACA:AccelaTemplateField>
                    </Columns>
                </ACA:AccelaGridView>
            </ItemTemplate>
        </asp:DataList>
        <div class="ACA_Line_Content ACA_TabRow">
            &nbsp;
        </div>
        <div id="divNormalTotalfee" runat="server" class="ACA_TabRow">
            <div class="ACA_SmLabel ACA_SmLabel_FontSize">
                <div>
                    <strong>
                        <ACA:AccelaLabel ID="per_feeItemList_label_totalFee" LabelKey="per_feeItemList_label_totalFee"
                            runat="server" />
                    </strong>
                    <br />
                    <ACA:AccelaLabel ID="per_feeItemList_textl_totalFeeNote" LabelKey="per_feeItemList_textl_totalFeeNote"
                        runat="server" />
                    <br />
                    <br />
                </div>
                <div class="ACA_Column_Short">
                    <br />
                </div>
                <div class="ACA_RightColumn_Short">
                    <span class="ACA_FRight"><strong>
                        <ACA:AccelaLabel ID="lblFeeAmount" runat="server" /></strong></span>
                </div>
            </div>
        </div>
        <div class="ACA_TabRow">
            <span class="ACA_FRight">
                <ACA:AccelaButton ID="lnkRecalculate" LabelKey="per_feeItemList_label_recalculate"
                    runat="server" OnClick="LnkRecalculate_Click" Visible="false" OnClientClick="return SubmitEP(this)"
                    DivEnableCss="ACA_SmButton ACA_SmButton_FontSize ACA_Button_Text" />
            </span>
        </div>
    </div>
</div>

<asp:HiddenField ID="hdnFeeExpand" runat="server" />
<ACA:AccelaHeightSeparate ID="AccelaHeightSeparate3" runat="server" Height="10" />
<!--the conditions instruction start-->
<div id="divConditionInstruction" runat="server" visible="false" class="ACA_Page ACA_Page_FontSize">
    <ACA:AccelaLabel ID="lblConditionInstruction" runat="server" LabelKey="aca_capfees_label_condition_instruction"
        LabelType="BodyText" />
</div>
<!--the conditions instruction end-->
<div class="ACA_TabRow">
    <ACA:CapConditions ID="capConditions" HideLink4ViewMet="true" ConditionDateUnconfigurable="true"
        GeneralConditionsTitleLabelKey="aca_capfees_label_generalcondition_sectionheader"
        ConditionsOfApprovalTitleLabelKey="aca_capfees_label_conditionofapproval_sectionheader"
        GeneralConditionsPatternLabelKey="aca_capfees_generalcondition_pattern" ConditionsOfApprovalPatternLabelKey="aca_capfees_conditionofapproval_pattern"
        runat="server" />
</div>
    
<script language="javascript" type="text/javascript">
    var hdnContactExpand = document.getElementById('<%=hdnFeeExpand.ClientID%>');

    function ControlFeeDisplay() {
        var obj = $get("<%=divTotalFee.ClientID %>");
        if (obj.style.display == '' || obj.style.display == "" || obj.style.display == "block") {
            ShowFeeSection(false);
        }
        else {
            ShowFeeSection(true);
        }
    }

    function ShowFeeSection(isShow) {
        var obj = $get("<%=divTotalFee.ClientID %>");
        var CTreeTop = '<%=ImageUtil.GetImageURL("section_header_collapsed.gif") %>';
        var ETreeTop = '<%=ImageUtil.GetImageURL("section_header_expanded.gif") %>';
        var altCollapsed = '<%=GetTextByKey("img_alt_collapse_icon") %>';
        var altExpanded = '<%=GetTextByKey("img_alt_expand_icon") %>';
        var imgObj = $get("imgFeeDetail");
        var lnkObj = $get("lnkFeeDetail");
        var lblValue = $get("<%=lblTotalFeeView.ClientID %>");
        if (isShow) {
            Expanded(imgObj, ETreeTop, altCollapsed);
            AddTitle(lnkObj, altCollapsed, lblValue);

            if (document.all)
                obj.style.display = '';
            else
                obj.style.display = 'block';

            //1:expand
            hdnContactExpand.value = 1;
        } else {
            Collapsed(imgObj, CTreeTop, altExpanded);
            AddTitle(lnkObj, altExpanded, lblValue);
            obj.style.display = 'none';

            //0:Collapse
            hdnContactExpand.value = 0;
        }
    }

    function KeepFeeExpandStatus() {
        var hdValue = hdnContactExpand.value;
        //1:expand. 0:Collapse. 
        
        if (hdValue == "1") {
            ShowFeeSection(true);
        }
        else {
            ShowFeeSection(false);
        }
    }

    if('<%=IsReviewPage%>' == 'True'){
        if ('<%= IsPostBack%>' == 'False'){
            ShowFeeSection(false);
        } else {
            KeepFeeExpandStatus();
        }
    }
</script>
