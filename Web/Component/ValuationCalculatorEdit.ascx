<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ValuationCalculatorEdit.ascx.cs" Inherits="Accela.ACA.Web.Component.ValuationCalculatorEdit" %>

<%@ Import Namespace="Accela.ACA.Web.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Util" %>
<script language ="javascript" type="text/javascript">
    function CalculateGroup(groupId, jobValueGroupSumObjId, valuationMultiplier, valuationExtraAmount) {
        var quantityObjs = $("input[groupId='" + groupId + "']");
        var jobValueSubtotal = 0;

        quantityObjs.each(function (i) {
            var quantityObj = quantityObjs[i];
            var jobValue = CalculateGroupItem(quantityObj);
            jobValueSubtotal += jobValue;
        });

        var groupSum = (jobValueSubtotal * valuationMultiplier) + valuationExtraAmount;
        var groupSumString = "";

        if (groupSum < 0) {
            groupSumString = "(" + I18nFormatNumberToCurrency(groupSum.toString().replace("-", "")) + ")";
        }
        else {
            groupSumString = I18nFormatNumberToCurrency(groupSum.toString());
        }

        var jobValueGroupSumObj = document.getElementById(jobValueGroupSumObjId);
        jobValueGroupSumObj.innerHTML = groupSumString;
    }

    function CalculateGroupItem(quantityObj) {
        var unitCostObjId = quantityObj.attributes["unitCostObjId"].value;
        var unitCostObj = document.getElementById(unitCostObjId);
        var unitCost = parseFloat(unitCostObj.attributes["rawValue"].value);

        var jobValueObjId = quantityObj.attributes["jobValueObjId"].value;
        var jobValueObj = document.getElementById(jobValueObjId);

        var excludeRegionalModifierObjId = quantityObj.attributes["excludeRegionalModifierObjId"].value;
        var excludeRegionalModifierObj = document.getElementById(excludeRegionalModifierObjId);
        var excludeRegionalModifier = excludeRegionalModifierObj.value;

        var regModifierObjId = quantityObj.attributes["regModifierObjId"].value;
        var regModifierObj = document.getElementById(regModifierObjId);
        var regModifier = 1;

        if (excludeRegionalModifier.toLowerCase() != "y" && regModifierObj.value != "") {
                regModifier = Number(regModifierObj.value);
        }

        if (quantityObj.value.trim() == GLOBAL_NUMBER_DECIMAL_SEPARATOR || quantityObj.value.trim() == "") {
            quantityObj.value = "0";
        }

        var quantity = parseFloat(quantityObj.value);
        var newJobValue = quantity * unitCost * regModifier;

        if (newJobValue < 0) {
            jobValueObj.innerHTML = "(" + I18nFormatNumberToCurrency(newJobValue.toString().replace("-", "")) + ")";
        }
        else {
            jobValueObj.innerHTML = I18nFormatNumberToCurrency(newJobValue.toString());
        }

        return newJobValue;
    }

    function calculator(quantityObj, jobValueGroupSumObjId, valuationMultiplier, valuationExtraAmount) {
        var groupId = quantityObj.attributes["groupId"].value;
        CalculateGroup(groupId, jobValueGroupSumObjId, valuationMultiplier, valuationExtraAmount);
    }

    function FormatCurrenceForNegative(currencyNegative) 
    {
        currencyNegative = currencyNegative.replace("(", "-");
        currencyNegative = currencyNegative.replace(")", "");
        return currencyNegative;
    }
</script>
<ACA:AccelaHideLink ID="hlBegin" runat="server" AltKey="img_alt_form_begin" CssClass="ACA_Hide"/>
<asp:UpdatePanel ID="editPanel" runat="server" UpdateMode="conditional" >
    <ContentTemplate>     
        <!-- start Valuation Calculator List -->  
        <div class="ACA_TabRow">           
             <ACA:AccelaGridView ID="gdvValuationCalculatorGroup" AlternatingRowStyle-CssClass = "ACA_TabRow_Odd ACA_TabRow_Odd_FontSize" runat="server" ShowHeader="false" OnRowDataBound="ValuationCalGroupList_RowDataBound"
                 Width="760px" role="presentation">
                <Columns>
                    <ACA:AccelaTemplateField>
                        <itemtemplate>
                            <div class="ACA_Title_Text font13px" id="divValCalTitle" runat="server" visible="false">
                                <div class="ACA_ValCal_Img" id="divAgencyLogo" runat="server" visible="false">
                                    <asp:Image runat="server" id="divImage" />
                                </div>
                                <div class="ACA_ValCal_Title font10px">
                                    <ACA:AccelaLabel ID="lblCAPTypeDisplayName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "capTypeDisplay") %>'></ACA:AccelaLabel>                              
                                    <span>(</span><ACA:AccelaLabel ID="lblCalculatorGroup" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "valuationCalculatorGroup") %>' /><span>)</span>
                                </div>                                
                            </div>
                        </itemtemplate>
                    </ACA:AccelaTemplateField>
                    <ACA:AccelaTemplateField AttributeName="lnkOccupancyName">
                        <itemtemplate> 
                        <tr>
                            <td colspan="8">
                            <div class="ACA_LiLeft" >
                                <ACA:AccelaGridView ID="gdvValuationCalculatorList"  runat="server" AllowPaging="false" AutoGenerateColumns="false"
                                    IsInSPEARForm="true" PageSize="10" ShowCaption="false" AutoGenerateCheckBoxColumn="false" EnableViewState = "true" 
                                    OnRowDataBound="ValuationCalculatorList_RowDataBound" AllowSorting="false" PagerStyle-HorizontalAlign="center" 
                                    PagerStyle-VerticalAlign="bottom" ShowHeader="true" GridViewNumber="60100" SummaryKey="gdv_valuationcalculator_edit_summary" Width="760px"
                                    CaptionKey="aca_caption_valuation_calculator">
                                <Columns>                                        
                                    <ACA:AccelaTemplateField AttributeName="lnkOccupancyName">
                                        <HeaderTemplate>
                                            <div class="ACA_CapListStyle">                                                    
                                                <ACA:AccelaLabel ID="lnkOccupancyName" runat="server" Width="200px" IsGridViewHeadLabel="true" LabelKey="valuationcalculator_list_occupancy" CausesValidation="false" CommandName="Header" />
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div class="ACA_CapListStyle">                                    
                                                <ACA:AccelaDropDownList ID="ddlOccupancy" Width="200px" runat="server" AutoPostBack ="true"  ToolTip='<%#DataBinder.Eval(Container.DataItem,"disUseType")%>' OnSelectedIndexChanged="Occupancy_SelectedIndexChanged"></ACA:AccelaDropDownList>                                    
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                        <headerstyle Width="100px" />
                                    </ACA:AccelaTemplateField>
                                    <ACA:AccelaTemplateField AttributeName="lnkUnitType">
                                        <HeaderTemplate>
                                            <div class="ACA_CapListStyle">
                                                <ACA:AccelaLabel ID="lnkUnitType" runat="server" Width="100px" IsGridViewHeadLabel="true"  LabelKey="valuationcalculator_list_unittype" CausesValidation="false" />
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div class="ACA_CapListStyle">                             
                                                <ACA:AccelaDropDownList ID="ddlUnitType" Width="100px" runat="server" AutoPostBack="true"  ToolTip='<%#DataBinder.Eval(Container.DataItem,"disConType")%>' OnSelectedIndexChanged="UnitType_SelectedIndexChanged"></ACA:AccelaDropDownList>                                    
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                        <headerstyle Width="100px" />
                                    </ACA:AccelaTemplateField>
                                    <ACA:AccelaTemplateField AttributeName="lnkUnitAmount">
                                        <HeaderTemplate>
                                            <div class="ACA_CapListStyle">
                                                <ACA:AccelaLabel ID="lnkUnitAmount" runat="server" IsGridViewHeadLabel="true" LabelKey="valuationcalculator_list_unitamount" CausesValidation="false" />
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div class="ACA_CapListStyle">                                                
                                                <ACA:AccelaNumberText ID="txtUnitAmount" MaxLength="8" runat="server" CssClass="ACA_Shot ACA_ARight" ToolTipLabelKey="valuationcalculator_list_unitamount" Text='<%#DataBinder.Eval(Container.DataItem,"unitValue")%>' IsNeedDot="false"></ACA:AccelaNumberText>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                        <headerstyle Width="100px" />
                                    </ACA:AccelaTemplateField>
                                    <ACA:AccelaTemplateField AttributeName="lnkUnit">
                                        <HeaderTemplate>
                                            <div class="ACA_CapListStyle">
                                                <ACA:AccelaLabel ID="lnkUnit" runat="server" IsGridViewHeadLabel="true" LabelKey="valuationcalculator_list_unit" CausesValidation="false" />
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div  class="ACA_CapListStyle ACA_Padding_Top_5">
                                                <ACA:AccelaLabel ID="lblUnit" runat="server"   />
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="150px" />
                                        <headerstyle Width="150px" />
                                    </ACA:AccelaTemplateField>                                               
                                    <ACA:AccelaTemplateField AttributeName="lnkUnitCost">
                                        <HeaderTemplate>
                                            <div class="ACA_ARight">
                                                <ACA:AccelaLabel ID="lnkUnitCost" runat="server" IsGridViewHeadLabel="true" LabelKey="valuationcalculator_list_unitcost" CausesValidation="false" />
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div class="ACA_ARight ACA_Padding_Top_5">
                                                 <asp:HiddenField ID="hidUnitCost" runat="server" Value='<%#I18nNumberUtil.ConvertMoneyToInvariantString(DataBinder.Eval(Container.DataItem, "unitCost"))%>' />
                                                 <ACA:AccelaLabel ID="lblUnitCost" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"unitCost")%>'/>        
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                        <headerstyle Width="100px" />
                                    </ACA:AccelaTemplateField>                                    
                                    <ACA:AccelaTemplateField AttributeName="lnkJobValue">
                                        <HeaderTemplate>
                                            <div class="ACA_ARight">
                                                <ACA:AccelaLabel ID="lnkJobValue" runat="server" IsGridViewHeadLabel="true" LabelKey="valuationcalculator_list_jobvalue"  CausesValidation="false" />
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div class="ACA_ARight ACA_Padding_Top_5">                                                                                                          
                                                <asp:HiddenField ID="hidJobValue" runat="server" Value='<%#I18nNumberUtil.ConvertMoneyToInvariantString(DataBinder.Eval(Container.DataItem,"totalValue"))%>'/>
                                                <ACA:AccelaLabel ID="lblJobValue" CssClass="aca_error_message" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"totalValue")%>'/>
                                            </div>
                                            <div><ACA:AccelaLabel ID="lblVersion" runat="server"  Visible="false"   />
                                                <ACA:AccelaNumberText Visible="false" ID="txtcalcValueSeqNbr" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"calcValueSeqNbr")%>'></ACA:AccelaNumberText>                                                   
                                                <ACA:AccelaNumberText Visible="false" ID="txtfeeIndicator" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"feeIndicator")%>'></ACA:AccelaNumberText>                                                                                                   
                                                <ACA:AccelaTextBox Visible="false" ID="txtCAPID1" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"capID.id1")%>'></ACA:AccelaTextBox>                                                                                              
                                                <ACA:AccelaTextBox Visible="false" ID="txtCAPID2" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"capID.id2")%>'></ACA:AccelaTextBox>                                                                                              
                                                <ACA:AccelaTextBox Visible="false" ID="txtCAPID3" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"capID.id3")%>'></ACA:AccelaTextBox>                                                                                              
                                                <asp:HiddenField ID="hdnExcludeRegionalModifier" runat="server" value='<%#DataBinder.Eval(Container.DataItem,"excludeRegionalModifier")%>' />                                                 
                                                <asp:HiddenField ID="lblUnitValue" runat="server" value='<%#DataBinder.Eval(Container.DataItem,"unitTyp")%>' /> 
                                                <asp:HiddenField ID="lblVersionValue" runat="server" value='<%#DataBinder.Eval(Container.DataItem,"version")%>' /> 
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                        <headerstyle Width="100px" />
                                    </ACA:AccelaTemplateField>                                             
                                </Columns> 
                            </ACA:AccelaGridView>
                        </div>
                        <div class="ACA_TabRow_NoMargin">                            
                            <div class="ACA_FRight">                                
                                <div class="ACA_Clear_Left">
                                    <div class="ACA_ValuatonTotal_Left">
                                        <div class="ACA_FRight">
                                        <p>
                                            <strong>
                                                <ACA:AccelaLabel runat="server" ID="lblValuationMultiplier" LabelKey="valuationcalculator_multiplier_lbl"></ACA:AccelaLabel></strong>
                                        </p>
                                        </div>
                                    </div>
                                    <div class="ACA_ValuatonTotal_Right">
                                        <div class="ACA_FLeft">                                      
                                         <p>
                                            <strong>
                                              <ACA:AccelaLabel runat="server" ID="lblValuationMultiplierValue" Text="1.0000"></ACA:AccelaLabel></strong>
                                        </p>
                                        </div>
                                    </div>
                                </div>
                                <div class="ACA_Clear_Left">
                                    <div class="ACA_ValuatonTotal_Left">
                                        <div class="ACA_FRight">
                                         <p>
                                            <strong>
                                               <ACA:AccelaLabel runat="server" ID="lblValuationExtraAmount" LabelKey="valuationcalculator_extraamount_lbl"></ACA:AccelaLabel></strong>
                                        </p>
                                        </div>
                                    </div>
                                    <div class="ACA_ValuatonTotal_Right">
                                        <div class="ACA_FLeft">
                                        <p>
                                            <strong>
                                              <ACA:AccelaLabel runat="server" ID="lblValuationExtraAmountValue" Text="0.00"></ACA:AccelaLabel></strong>
                                        </p>
                                        </div>
                                    </div>
                                </div>
                                <div class="ACA_Clear_Left">
                                    <div class="ACA_ValuatonTotal_Left">
                                        <div class="ACA_FRight">
                                            <p>
                                                <strong>
                                                    <ACA:AccelaLabel ID="lblValulationCalculatorTotalJob" LabelKey="valuationcalculator_totaljob"
                                                        runat="server" />
                                                </strong>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="ACA_ValuatonTotal_Right">
                                        <div class="ACA_FLeft">
                                        <p>
                                            <strong>
                                                <ACA:AccelaLabel ID="lblValulationCalculatorTotalJobValue" runat="server" />
                                            </strong>
                                        </p>
                                        </div>
                                    </div>
                                </div>                               
                           </div>
                         </div>  
                        <div>
                            <asp:HiddenField ID="hdnRegModifier" runat="server" />                           
                         </div>                         
                        <div runat="server" id="dvSepLineValuationCalculator" class="ACA_TabRow ACA_BkGray ACA_Valuation_Line">&nbsp;</div>
                        </td>
                        </tr>
                        </itemtemplate>
                    </ACA:AccelaTemplateField>
                </Columns>
            </ACA:AccelaGridView>
        </div>  
    </ContentTemplate> 
</asp:UpdatePanel>
<ACA:AccelaHideLink ID="hlEnd" runat="server" CssClass="ACA_Hide" AltKey="aca_common_msg_formend"/>   
 
 