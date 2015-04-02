<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" Inherits="Accela.ACA.Web.APO.OwnerDetail"
    ValidateRequest="false" CodeBehind="OwnerDetail.aspx.cs" %>

<%@ Register Src="~/Component/RefAPOAddressList.ascx" TagName="AddressList" TagPrefix="uc1" %>
<%@ Register Src="~/Component/RefAPOParcelList.ascx" TagName="ParcelList" TagPrefix="uc1" %>
<%@ Register Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    <div id="MainContent" class="ACA_Content" runat="server">
        <div id="infoAddress" runat="server">
            <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_page_instruction_owner_detail" LabelType="PageInstruction"
                runat="server" />
            <h1>
                <ACA:AccelaLabel LabelKey="APO_OwnerDetail_lbl_Title_Display" ID="APO_OwnerDetail_lbl_Title_Display"
                    runat="server"></ACA:AccelaLabel>
                <br />
                <ACA:AccelaLabel ID="lblPropertyInfo" runat="server"></ACA:AccelaLabel>
            </h1>
            <div class="ACA_Row">
                <uc1:MessageBar ID="disabledMessage" runat="Server" />
            </div>
            <div runat="server" id="spanRelatedInfo">
                <ACA:AccelaLabel LabelKey="APO_OwnerDetail_lbl_Session_Detail" ID="APO_OwnerDetail_lbl_Session_Detail"
                    runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
                <h1>
                    <ACA:AccelaLabel ID="lblOwnerName" runat="server"></ACA:AccelaLabel>
                </h1>
                <div style="width: 200px" class="Header_h3">
                    <ACA:AccelaLabel ID="lblOwnerDetail" IsNeedEncode="False" runat="server"></ACA:AccelaLabel>
                    <ACA:AccelaLabel ID="lblOwnerPhone" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                    <ACA:AccelaLabel ID="lblOwnerFax" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                    <ACA:AccelaLabel ID="lblOwnerEmail" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                    <ACA:AccelaLabel ID="lblOwnerStatus" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
                </div>

                <ACA:AccelaLabel LabelKey="APO_OwnerDetail_lbl_Session_Addresses" ID="APO_OwnerDetail_lbl_Session_Addresses"
                    runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
                <div id="ucAddressList" runat="server" class="ACA_FLeft" style="clear: both;" visible="true">
                    <uc1:AddressList ID="addressList" GViewID="60047" IsShowMap="false" runat="server" Visible="true" OnPageIndexChanging="Address_GridViewIndexChanging" OnGridViewSort="Address_GridViewSort" />
                </div>
                <ACA:AccelaLabel LabelKey="APO_OwnerDetail_lbl_Session_Parcels" ID="APO_OwnerDetail_lbl_Session_Parcels"
                    runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
                <div id="ucParcelList" class="ACA_FLeft" style="clear: both;" runat="server" visible="true">
                    <uc1:ParcelList ID="parcelList" IsShowMap="false" runat="server" Visible="true" OnPageIndexChanging="Parcel_GridViewIndexChanging" OnGridViewSort="Parcel_GridViewSort"/>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
