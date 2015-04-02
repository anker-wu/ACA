<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true"
 Inherits="Accela.ACA.Web.MyCollection.MyCollectionManagement" ValidateRequest="false" Codebehind="MyCollectionManagement.aspx.cs" %>

<%@ Register src="~/Component/MyCollectionList.ascx" tagname="MyCollectionList" tagprefix="uc1" %>
<%@ Register src="~/Component/CAPs2MyCollection.ascx" TagName="CAPs2MyCollection" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" Runat="Server"> 
    <div class="ACA_Content">
         <div class="ACA_TabRow">
            <ACA:AccelaLabel ID="lblcollections" LabelKey="mycollection_collectionmanagement_collectionname"
                runat="server" LabelType="SubSectionText" />
        </div>
        <div>
            <uc1:MyCollectionList ID="MyCollectionList" runat="server" /> 
        </div><br />
        <div id="divAddCaps2Collection"  class="ACA_Add2CollectionForm" style="display:block;" runat="server" visible="false"> 
          <uc2:CAPs2MyCollection ID="OperateCAPs2Collection" runat="server" />  
        </div> 
    </div>
</asp:Content>



