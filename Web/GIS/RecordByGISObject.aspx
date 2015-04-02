<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="RecordByGISObject.aspx.cs"
 MasterPageFile="~/Dialog.Master" Inherits="Accela.ACA.Web.GIS.RecordByGISObject" %>

<%@ Register Src="../Component/RefAPOAddressList.ascx" TagName="RefAPOAddressList" TagPrefix="uc1" %>
<%@ Register Src="~/Component/CapList.ascx" TagName="CapList" TagPrefix="uc2" %>
<%@ Register Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <div id="dvAPOList" class="ACA_Row" runat="server">
        <ACA:AccelaLabel LabelKey="aca_gis_object_lbl_section_parcels" ID="APO_AddressDetail_lbl_Session_Parcels"
            runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
        <uc1:RefAPOAddressList ID="RefAPOAddressList1" GViewID="60122" ComponentType="1" IsShowMap="false" runat="server" 
                OnPageIndexChanging="RefAddress_GridViewIndexChanging" OnGridViewSort="RefAddress_GridViewSort" />
    </div>
    <div id="dvCapList" class="ACA_Row" runat="server">
        <ACA:AccelaLabel LabelKey="aca_gis_object_lbl_section_records" ID="AccelaLabel1"
            runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
        <uc2:CapList ID="capList" IsHideMap="true" IsForLicensee="true" AutoGenerateCheckBoxColumn="false" IsMyPermitList="true" runat="server" GViewID="60121" OnPageIndexChanging="PermitList_GridViewIndexChanging" OnGridViewSort="PermitList_GridViewSort" />
    </div>
</asp:Content>