<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.ParcelSearchResult" Codebehind="ParcelSearchResult.ascx.cs" %>
<%@ Register Src="~/Component/AddressSearchList.ascx" TagName="AddressSearchList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/RefAPOParcelList.ascx" TagName="ParcelList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/RefAPOOwnerList.ascx" TagName="OwnerList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/Conditions.ascx" TagName="Conditions" TagPrefix="ACA" %>
<div class="Parcels">
    <ACA:AccelaLabel ID="lblParcelSession" runat="server" LabelKey="aca_parcelsearchresult_label_parcel_title" LabelType="SectionTitle"></ACA:AccelaLabel>
    <ACA:ParcelList ID="ucParcelList" runat="server" IsShowMap="false" OnSelect="Parcel_Select"
        AllowSelectingByRadioButton="True" AutoPostBackOnSelect="True" GridViewNumber="60187"
        OnPageIndexChanging="Parcel_GridViewIndexChanging" OnGridViewSort="Parcel_GridViewSort" />
    <div class="Conditions" id="divConditon" runat="server" Visible="False">
        <ACA:Conditions ID="ucConditon" runat="server" Type="Parcel" Visible="False" />
    </div>
</div>
<div class="Associates" id="divParcelAssociates" runat="server" Visible="False">
    <div class="Addresses">
        <ACA:AccelaLabel ID="lblAddressSession" runat="server" LabelKey="aca_parcelsearchresult_label_address_title" LabelType="SectionTitle"></ACA:AccelaLabel>
        <ACA:AddressSearchList ID="ucAddressList" runat="server"
            AllowSelectingByRadioButton="True" AutoPostBackOnSelect="False" GridViewNumber="60185" />
    </div>
    <div class="Owners">
        <ACA:AccelaLabel ID="lblOwnerSession" runat="server" LabelKey="aca_parcelsearchresult_label_owner_title" LabelType="SectionTitle"></ACA:AccelaLabel>
        <ACA:OwnerList ID="ucOwnerList" runat="server"
            AllowSelectingByRadioButton="True" GridViewNumber="60189"
            OnPageIndexChanging="Owner_GridViewIndexChanging" OnGridViewSort="Owner_GridViewSort" />
    </div>
</div>
<script type="text/javascript">
    // Note: This method is created by combining in method RegisterJavaScript() of Conditions.ascx.cs, so this name cannot be changed.
    function showMoreparcelCondition(containerId, linkId) {
        showMoreCondition(containerId, linkId);
    }
</script>