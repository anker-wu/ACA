<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="True" 
    Inherits="Accela.ACA.Web.APO.AddressDetail" ValidateRequest="false" Codebehind="AddressDetail.aspx.cs" %>
<%@ Register Src="~/Component/RefAPOParcelList.ascx" TagName="ParcelList" TagPrefix="uc1" %>
<%@ Register Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="uc1" %>
<%@ Register Src="~/Component/ACAMap.ascx" TagName="ACAMap" TagPrefix="ACA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    
    <div id="MainContent" class="ACA_Content" runat="server">
        <div id="infoAddress" runat="server">
            <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_page_instruction_address_detail" LabelType="PageInstruction"
                runat="server" />
            <ACA:ACAMap ID="mapAddress" AGISContext="AddressDetail" OnShowOnMap="MapAddress_ShowOnMap" runat="server" />
            <h1 style="margin-bottom:8px;">
                <ACA:AccelaLabel LabelKey="APO_AddressDetail_lbl_Title_Display" ID="APO_AddressDetail_lbl_Title_Display" 
                runat="server"></ACA:AccelaLabel>
                <br />
                <ACA:AccelaLabel ID="lblPropertyInfo" runat="server"></ACA:AccelaLabel>
            </h1>
            <div class="ACA_Row" style="margin-bottom:8px;">
                <uc1:MessageBar ID = "disableMessage" runat="Server" />
            </div>
            <div runat="server" id="spanRelatedInfo">
                <br />
                <ACA:AccelaLabel LabelKey="APO_AddressDetail_lbl_Session_Detail" ID="APO_AddressDetail_lbl_Session_Detail"
                    runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
                <div class="td_mini_map_left">
                    <div class="ACA_Address_Detail_Info">
                        <ACA:AccelaLabel ID="lblAddressDetail" runat="server"></ACA:AccelaLabel>
                        <ACA:AccelaLabel ID="lblAddressStatus" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                    </div>
                </div>
                <div class="td_mini_map_right">
                    <ACA:ACAMap ID="miniMapAddress" AGISContext="AddressDetail" IsMiniMode="true" OnShowOnMap="MapAddress_ShowOnMap" runat="server" />
                </div>
                <br />
                <ACA:AccelaLabel LabelKey="APO_AddressDetail_lbl_Session_Parcels" ID="APO_AddressDetail_lbl_Session_Parcels"
                    runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
                <div class="ACA_FLeft" id="ucParcelList" runat="server" style="clear: both;">
                    <uc1:ParcelList ID="parcellList" runat="server" IsShowMap="false" OnPageIndexChanging="Parcel_GridViewIndexChanging" OnGridViewSort="Parcel_GridViewSort" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
