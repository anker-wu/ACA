<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ValuationCalculatorView.ascx.cs" Inherits="Accela.ACA.Web.Component.ValuationCalculatorView" %>
<%@ Import Namespace="Accela.ACA.Web.Common" %>
<%@ import namespace="Accela.ACA.Common.Util" %>
<asp:UpdatePanel ID="editPanel" runat="server" UpdateMode="conditional" >
    <ContentTemplate>        
        <div class="ACA_Row">  
            <div class="ACA_TabRow">            
                <ACA:AccelaGridView ID="gdvValuationCalculatorGroup" role="presentation"
                AlternatingRowStyle-CssClass = "ACA_TabRow_Odd ACA_TabRow_Odd_FontSize" 
                runat="server" ShowHeader="false" OnRowDataBound="ValuationCalGroupList_RowDataBound"
                    Width="760px">
                    <Columns>
                        <ACA:AccelaTemplateField>
                            <itemtemplate>
                                <div class="ACA_Title_Text font13px" id="divValCalTitle" runat="server" visible="false">
                                    <div class="ACA_ValCal_Img" id="divAgencyLogo" runat="server" visible="false">
                                        <asp:Image runat="server" id="divImage"/>
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
                                            <div style="display:block;"  class="ACA_LiLeft">
                                                <ACA:AccelaGridView ID="gdvValuationCalculatorList"  runat="server" AllowPaging="false" AutoGenerateColumns="false"
                                                    IsInSPEARForm="true" PageSize="10" ShowCaption="false" AutoGenerateCheckBoxColumn="false" EnableViewState = "true" 
                                                    AllowSorting="false" PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom" ShowHeader="true"                      
                                                    GridViewNumber="60100" SummaryKey="gdv_valuationcalculator_view_summary" CaptionKey="aca_caption_valuationcalculator_view"
                                                    Width="760px" OnRowCommand="ValCalList_RowCommand">
                                                    <Columns>                                        
                                                        <ACA:AccelaTemplateField AttributeName="lnkOccupancyName">
                                                            <HeaderTemplate>
                                                                <div class="ACA_CapListStyle">
                                                                    <ACA:GridViewHeaderLabel ID="lnkOccupancyName" Width="200px" runat="server" SortExpression="useTyp" LabelKey="valuationcalculator_list_occupancy" CausesValidation="false" CommandName="Header" />
                                                                </div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div class="ACA_CapListStyle">                                    
                                                                    <ACA:AccelaLabel ID="lblOccupancy" Width="200px" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "disUseType") %>'/>
                                                                </div>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="100px" />
                                                            <headerstyle Width="100px" />
                                                        </ACA:AccelaTemplateField>
                                                        <ACA:AccelaTemplateField AttributeName="lnkUnitType">
                                                            <HeaderTemplate>
                                                                <div class="ACA_CapListStyle">
                                                                    <ACA:GridViewHeaderLabel ID="lnkUnitType" Width="100px" runat="server" SortExpression="conTyp"  LabelKey="valuationcalculator_list_unittype" CausesValidation="false" />
                                                                </div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div class="ACA_CapListStyle">                             
                                                                    <ACA:AccelaLabel ID="lblUnitType" Width="100px" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "disConType") %>'/>                                                
                                                                </div>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="100px" />
                                                            <headerstyle Width="100px" />
                                                        </ACA:AccelaTemplateField>
                                                        <ACA:AccelaTemplateField AttributeName="lnkUnitAmount">
                                                            <HeaderTemplate>
                                                                <div class="ACA_FRight">
                                                                    <ACA:GridViewHeaderLabel ID="lnkUnitAmount" runat="server"  SortExpression="unitValue" LabelKey="valuationcalculator_list_unitamount" CausesValidation="false" />
                                                                </div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div class="ACA_Shot ACA_FRight ACA_ARight">
                                                                    <ACA:AccelaLabel ID="lblUnitAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "unitValue") %>' />
                                                                </div>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="100px" />
                                                            <headerstyle Width="100px" />
                                                        </ACA:AccelaTemplateField>
                                                        <ACA:AccelaTemplateField AttributeName="lnkUnit">
                                                            <HeaderTemplate>
                                                                <div class="ACA_CapListStyle ACA_ARight">
                                                                    <ACA:GridViewHeaderLabel ID="lnkUnit" runat="server" SortExpression="unitTyp" LabelKey="valuationcalculator_list_unit"  CausesValidation="false" />
                                                                </div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div class="ACA_CapListStyle ACA_ARight">
                                                                    <ACA:AccelaLabel ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "disUnitType") %>'/>
                                                                </div>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="100px" />
                                                            <headerstyle Width="100px" />
                                                        </ACA:AccelaTemplateField>                                                                          
                                                        <ACA:AccelaTemplateField AttributeName="lnkUnitCost">
                                                            <HeaderTemplate>
                                                                <div  class="ACA_ARight">
                                                                    <ACA:GridViewHeaderLabel ID="lnkUnitCost" runat="server" SortExpression="unitCost" LabelKey="valuationcalculator_list_unitcost"  CausesValidation="false" />
                                                                </div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div class="ACA_ARight">
                                                                    <ACA:AccelaLabel ID="lblUnitCost" runat="server" Text='<%# I18nNumberUtil.FormatMoneyForUI(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "unitCost"))) %>'/>
                                                                </div>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="100px" />
                                                            <headerstyle Width="100px" />
                                                        </ACA:AccelaTemplateField> 
                                                        <ACA:AccelaTemplateField AttributeName="lnkJobValue">
                                                            <HeaderTemplate>
                                                                <div  class="ACA_ARight">
                                                                    <ACA:GridViewHeaderLabel ID="lnkJobValue" runat="server" SortExpression="totalValue" LabelKey="valuationcalculator_list_jobvalue"  CausesValidation="false" />
                                                                </div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div class="ACA_ARight">
                                                                    <ACA:AccelaLabel ID="lblJobValue" CssClass="aca_error_message" runat="server" Text='<%# I18nNumberUtil.FormatMoneyForUI(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "totalValue"))) %>'/>
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
                                            <div runat="server" id="dvSepLineValuationCalculator" class="ACA_TabRow ACA_BkGray ACA_Valuation_Line">&nbsp;</div>
                                        </td>
                                    </tr>
                                </itemtemplate>
                            </ACA:AccelaTemplateField>
                        </Columns>
                    </ACA:AccelaGridView>                     
                </div>
            </div>
        </ContentTemplate>
</asp:UpdatePanel>
