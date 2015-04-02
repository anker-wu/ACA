<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.TopShoppingCartItems" Codebehind="TopShoppingCartItems.ascx.cs" %> 
 
<div style="width:23em;">
    <div style="margin:2px">
        <div class="topshoppingcart_title">
            <ACA:AccelaLabel runat="server" ID="lblCartName" LabelKey="per_topshoppingcartitem_label_carttitle"></ACA:AccelaLabel>
        </div>
    </div>
    <div style="margin:8px">
        <div class="topshoppingcart_emptynote" >
            <ACA:AccelaLabel ID="lblEmpty" Visible="false" LabelKey="per_topshoppingcartitem_label_emptynote" runat="server"></ACA:AccelaLabel> 
        </div>
    </div>
    <div id="divCaps" runat="server"  style="margin:2px">
        <asp:DataList ID="capList" Width="100%" runat="server" EnableViewState="true"   OnItemDataBound="CapList_ItemDataBound" role='presentation'>
         <ItemTemplate>  
             <table role='presentation' style="width:100%;table-layout: fixed;" runat="server">
                <tr> 
                    <td style="width:55%;word-break:break-all;">                                
                        <div class="topshoppingcart_item">
                            <ACA:AccelaLabel ID="lblCapID" runat="server"></ACA:AccelaLabel>
                         </div>
                    </td>
                    <td style="width:35%"> 
                        <div class="topshoppingcart_item ACA_FRight">
                            <ACA:AccelaLabel ID="lblCAPTotalFee" runat="server"></ACA:AccelaLabel>
                        </div> 
                    </td>
                    <td style="width:10%"> 
                    </td>
                </tr>                             
             </table>                                     
         </ItemTemplate> 
    </asp:DataList> 
    </div> 
    <div style="margin:2px;" class="ACA_ARight fontbold font11px">
        <ACA:AccelaLinkButton runat="server" ID="btnMore" LabelKey="per_topshoppingcartitem_label_more" OnClick="MoreButton_Click"></ACA:AccelaLinkButton>
    </div>
</div>
