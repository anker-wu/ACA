<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.ShoppingCart.ShoppingCart" ValidateRequest="false" Codebehind="ShoppingCart.aspx.cs" %>
<%@ Register Src="../Component/ShoppingCartList.ascx" TagName="ShoppingCartList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">  
    <div class="ACA_ContainerLong_ShoppingCart">  
    <div>
        <h1>
            <ACA:AccelaLabel runat="server" ID="lblCart" LabelKey="per_shoppingcart_label_carttitle"></ACA:AccelaLabel>
        </h1>    
    </div>
    <div>
        <ACA:BreadCrumpToolBar IsForShoppingCart="true" ID="BreadCrumpShoppingCart" runat="server"></ACA:BreadCrumpToolBar>        
    </div>
    
    <span id="SecondAnchorInACAMainContent"></span>
    <br />   
    
    <div class="Header_h4">
        <ACA:AccelaLabel runat="server" LabelType="bodyText" ID="lblShoppingCartListNote" LabelKey="per_shoppingcart_label_cartnote"></ACA:AccelaLabel>
    </div> 
    
    <div id="divTitleSelectedBar">
        <div id="divTitleSelected" runat="server" class="ACA_ShoppingCartTitle_Bar">
            <h1>            
                <ACA:AccelaLabel ID="lblTitleSelected" runat="server" LabelKey="per_shoppingcart_label_paynow" LabelType="SectionTitleWithBar"></ACA:AccelaLabel>
            </h1>
        </div>
        <span ID="lblTitleSelected_sub_label" class="ACA_Section_Instruction ACA_Section_Instruction_FontSize" runat="server" ></span>
    </div>
    
    <div runat="server" Visible="false"  id="divNoAddressSelectedSetting">
        <h1>
            <ACA:AccelaLabel ID="lblNoAddressSelectedSetting" runat="server" LabelKey="per_shoppingcart_label_noaddressselected"></ACA:AccelaLabel>
        </h1>
    </div>
              
    <div runat="server" Visible="false"  id="divNoRecord">
        <h1>
            <ACA:AccelaLabel ID="lblNoRecord" runat="server" LabelKey="per_shoppingcart_label_norecord"></ACA:AccelaLabel>
        </h1>
    </div>   
        <uc1:ShoppingCartList IsSelected="true" ID="ShoppingCartListSelected" runat="server" />    
  
      
    <br />
    
    <div>
        <b> 
            <ACA:AccelaLabel ID="lblTotalFeeTitle" class="font15px" LabelKey="per_shoppingcart_label_totalfeetitle" runat="server" />  
            <ACA:AccelaLabel ID="lblFeeAmount" class="font15px" Text="" runat="server" /> 
        </b>
        <div class="Header_h4"> 
            <ACA:AccelaLabel ID="lblTotalFeeNote" LabelKey="per_shoppingcart_label_totalfeenote" runat="server" />                
        </div>
    </div> 
      <br />      
    <table role='presentation' cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <ACA:AccelaButton ID="btnCheckOut" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" LabelKey="per_shoppingcart_label_checkout" runat="server" OnClick="CheckOutButton_Click"></ACA:AccelaButton>
            </td>
            <td style="width:10px"></td> 
            <td>      
                    <ACA:AccelaButton DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" EnableConfigureURL="true" OnClick="CreateAnotherApplicationButton_Click" ID="btnCreateAnotherApplication" LabelKey="per_shoppingcart_label_createanotherapplication" runat="server"></ACA:AccelaButton>
                </td>
        </tr>
    </table> 
    <br />    
    <div id="divTitleSaved" runat="server"> 
         <ACA:AccelaLabel ID="lblTitleSaved" runat="server" LabelKey="per_shoppingcart_label_paylater" LabelType="SectionTitle"></ACA:AccelaLabel>
    </div>
    <div runat="server" Visible="false" id="divNoAddressSavedSetting"> 
        <h1> 
            <ACA:AccelaLabel ID="lblNoAddressSavedSetting" LabelKey="per_shoppingcart_label_noaddressselected" runat="server"></ACA:AccelaLabel>
        </h1> 
    </div> 
        <uc1:ShoppingCartList ID="ShoppingCartListSaved" runat="server" />
    
    </div>  
</asp:Content>
