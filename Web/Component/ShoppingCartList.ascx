<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.ShoppingCartList" Codebehind="ShoppingCartList.ascx.cs" %> 
<%@ Import Namespace="Accela.ACA.Common.Util" %>
    <script type="text/javascript">
    function ShowFee(feeListTableID,imgID,lnkObj)
    { 
        var imgCollapsedURL='<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("caret_collapsed.gif") %>';
        var imgExpandedURL='<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("caret_expanded.gif") %>';
        var obj = document.getElementById(feeListTableID);
        var imgObj = document.getElementById(imgID);
        var lnkObj = document.getElementById(lnkObj);
        var altCollapsed = '<%=GetTextByKey("img_alt_collapse_icon").Replace("'","\\'") %>';
        var altExpanded = '<%=GetTextByKey("img_alt_expand_icon").Replace("'","\\'") %>';
        
        if(obj.style.display =="none")
        {
            obj.style.display = "";
            Expanded(imgObj, imgExpandedURL, altCollapsed);
            AddTitle(lnkObj, altCollapsed, null);
        }
        else
        {
            obj.style.display = "none";
            Collapsed(imgObj, imgCollapsedURL, altExpanded);
            AddTitle(lnkObj, altExpanded, null);      
        }
    }
    </script>
<div style="width: 720px">
    <asp:DataList ID="addressList" runat="server" Width="100%" EnableViewState="true" OnItemDataBound="AddressList_ItemDataBound" role='presentation'>
        <ItemTemplate>        
            <div id="divAddress" class="Header_h2">
                <asp:Label ID="lblAddress" runat="server"></asp:Label> 
            </div> 
            <div class="Header_h2">
                <asp:Label runat="server" ID="lblApplicationNumber"></asp:Label>
                <ACA:AccelaLabel runat="server" ID="lblApplicationLabel" LabelKey="per_shoppingcartcomponent_label_Applications"></ACA:AccelaLabel> |
                <asp:Label runat="server" ID="lblAddressTotalFee"></asp:Label>
             </div>
                 
             <div style="width:700px">
                <asp:DataList ID="capList" Width="100%" runat="server" EnableViewState="true" OnItemCommand="CapList_ItemCommand" OnItemDataBound="CapList_ItemDataBound" role='presentation'>
                    <ItemTemplate>
                        <!--   list all CAPS Summary -->
                        <table role='presentation' runat="server" width="100%" id="tableCAPSum" cellpadding="0" cellspacing="0">
                            <tr style="height:25px; vertical-align:top;">
                                <td>
                                    <img runat="server" id="imgErrCap4Payment" Visible="False" />
                                </td>
                                <td style="width: 3%">
                                   <a href="javascript:void(0)" runat="server" id="linkShowFeeItem" class="NotShowLoading">
                                   <img  runat="server" id="imgShowFeeItem" style="border-width:0px" /></a> 
                                </td>
                                <td style="width: 29%" id="tdCapType" runat="server">                                
                                    <div class="ShoppingCart_FeeItem Header_h4">                                
                                        <ACA:AccelaLabel ID="lblCapName" runat="server" Text='<%#ShowCapType(DataBinder.Eval(Container.DataItem, "capType"))%>'></ACA:AccelaLabel>
                                        <br />
                                        <ACA:AccelaLabel ID="lblCapID"  runat="server"></ACA:AccelaLabel>
                                     </div>
                                </td>
                                <td style="width: 12%"> 
                                    <div class="Header_h4 ACA_FRight">
                                        <ACA:AccelaLabel ID="lblCAPTotalFee" runat="server"></ACA:AccelaLabel>
                                    </div> 
                                </td>
                                <td style="width:2%;"></td>
                                <td style="width: 23%" id="tdAgency" runat="server">                                 
                                    <table role='presentation' id="tbAgency" width="100%"  runat="server" >
                                        <tr>
                                            <td style="width:15%;" id="tbAgencyLogo" runat="server">
                                                <img id="imgAgencyLogo" runat="server"  height="22"/>
                                            </td>
                                            <td style="width:85%;">
                                                <div class="Header_h4">
                                                    <ACA:AccelaLabel ID="lblAgenceName" Text='<%#DataBinder.Eval(Container.DataItem, "capID.serviceProviderCode")%>' runat="server" ></ACA:AccelaLabel>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>  
                                </td>
                                <td style="width: 9%">
                                    <div class="Header_h4"> 
                                        <ACA:AccelaLinkButton runat="server" ID="btnEditApplication" LabelKey="per_shoppingcartcomponent_button_editapplication" CommandName="EditApplication"></ACA:AccelaLinkButton>
                                    </div>
                                </td>
                                <td style="width: 14%;" align="center">  
                                    <div  class="Header_h4" style="width:100%" runat="server" id="divSelected">
                                        <ACA:AccelaLinkButton runat="server" ID="btnSaveLater" LabelKey="per_shoppingcartcomponent_button_savelater" CommandName="SaveLater"></ACA:AccelaLinkButton>
                                    </div>
                                    <div class="Header_h4" style="width:100%" runat="server" visible="false" id="divSaved">
                                        <ACA:AccelaLinkButton runat="server" ID="btnAddForPayment" LabelKey="per_shoppingcartcomponent_button_addforpayment" CommandName="AddForPayment"></ACA:AccelaLinkButton>
                                    </div>
                                </td>
                                <td style="width:2%"></td>
                                <td style="width: 6%"> 
                                    <div class="Header_h4">
                                        <ACA:AccelaLinkButton runat="server" ID="btnRemove" LabelKey="per_shoppingcartcomponent_button_remove" CommandName="RemoveApplication"></ACA:AccelaLinkButton>
                                    </div>
                                </td>
                            </tr>
                        </table> 
                        <table role='presentation' id="tablePaymentMethod" runat="server" visible="false" class="ACA_FullWidthTable">
                             <tr>
                             <td class="aca_shoppingcartlist_payment Header_h4">
                               <ACA:AccelaLabel ID="lblPaymentList" LabelKey="aca_payment_label_paymentmethod" runat="server" > </ACA:AccelaLabel> <ACA:AccelaLabel runat="server" ID="lblPaymentMethodItem"></ACA:AccelaLabel>
                                </td>
                             </tr>
                        </table>
                        <table role='presentation' id="tbFeeList" runat="server" style="width:100%;">
                             <tr>
                                <td style="width: 3%; padding-right:16px;"></td>
                                <td style="width: 97%;">
                        <ACA:AccelaGridView ID="feeList" Width="100%" runat="server" AutoGenerateColumns="False"
                          PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom" OnRowDataBound="FeeList_RowDataBound"
                          SummaryKey="aca_shoppingcart_feelistsummary" CaptionKey="aca_caption_shoppingcart_feelist" ShowHorizontalScroll="false">
                        <Columns>
                            <ACA:AccelaTemplateField>
                                <ItemTemplate>
                                    <img ID="imgErrFeeItem" runat="server" Visible="False">
                                </ItemTemplate>
                                <HeaderStyle Width="2%"/>
                                <ItemStyle Width="2%"/>
                            </ACA:AccelaTemplateField>
                            <ACA:AccelaTemplateField>
                                <HeaderTemplate>
                                    <ACA:AccelaLabel ID="lblFee" LabelKey="per_shoppingcartcomponent_label_fee" runat="server" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <ACA:AccelaLabel ID="lblItemDescription" runat="server" Text='<%#GetFeeDescription(DataBinder.Eval(Container.DataItem, "resFeeDescription"),DataBinder.Eval(Container.DataItem, "feeDescription"))%>' />
                                    <asp:HiddenField ID="hidFieldFeeSeqNbr" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "feeSeqNbr") %>'/>
                                </ItemTemplate>
                                <HeaderStyle Width="37%"/>
                                <ItemStyle Width="37%"/>
                            </ACA:AccelaTemplateField>
                            <ACA:AccelaTemplateField>
                                <HeaderTemplate>
                                    <span class="ACA_FRight">
                                        <ACA:AccelaLabel ID="lblQuanity" LabelKey="per_shoppingcartcomponent_label_quanity" runat="server" />
                                    </span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="ACA_FRight">
                                          <ACA:AccelaLabel ID="lblQuantity" runat="server" Text='<%#I18nNumberUtil.ConvertDecimalForUI(DataBinder.Eval(Container.DataItem, "feeUnit").ToString())%>' />
                                    </span>
                                </ItemTemplate>
                                <HeaderStyle Width="8%"/>
                                <ItemStyle Width="8%"/>
                            </ACA:AccelaTemplateField>
                            <ACA:AccelaTemplateField>
                                <HeaderTemplate>
                                    <span id="spanAccountHeader" runat="server" class="ACA_FRight ACA_ShoppingAccountHeader">
                                        <ACA:AccelaLabel ID="lblAmount" LabelKey="per_shoppingcartcomponent_label_amount"
                                            runat="server" />
                                    </span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <nobr>
                                    <span id="spanAccountItem" runat="server" class="ACA_FRight ACA_ShoppingAccountItem">
                                        <ACA:AccelaLabel ID="lblAmount" runat="server" Text='<%# I18nNumberUtil.FormatMoneyForUI(DataBinder.Eval(Container.DataItem, "fee"))%>' />
                                    </span></nobr>
                                </ItemTemplate>
                                <HeaderStyle Width="52%" />
                                <ItemStyle Width="52%" />
                            </ACA:AccelaTemplateField>
                        </Columns>
                        </ACA:AccelaGridView>
                            </td>
                          </tr>
                        </table>
                        <table role='presentation' runat="server" id="tableCapId" border="0" cellpadding="0" cellspacing="0"
                            visible="false">
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hdnCapID1" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "capID.id1")%>' />
                                    <asp:HiddenField ID="hdnCapID2" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "capID.id2")%>' />
                                    <asp:HiddenField ID="hdnCapID3" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "capID.id3")%>' />
                                    <asp:HiddenField ID="hdnAgence" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "capID.serviceProviderCode")%>' />
                                    <asp:HiddenField ID="hdnCartSeqNumber" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "cartSeqNumber")%>' />
                                    <asp:HiddenField ID="hdnModuleName" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "capType.moduleName")%>' />
                                    <asp:HiddenField ID="hdnCapClass" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "capClass")%>' />
                                    <asp:HiddenField ID="hdnRenewStatus" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "processType")%>' />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate> 
                </asp:DataList>
                <div style="margin-bottom:15px"></div>   
     
        </div>
            <hr id="hrDivision" runat="server" />
        </ItemTemplate>
    </asp:DataList>
</div>
