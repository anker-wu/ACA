<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetDetailView.ascx.cs" Inherits="Accela.ACA.Web.Component.AssetDetailView" %>
<%@ Register Src="~/Component/AssetTemplateView.ascx" TagPrefix="ACA" TagName="AssetTemplateView" %>
<div class="detailfield">
    <div class="asset_detail_left_column">
        <ACA:AccelaNameValueLabel ID="lblAssetId" CssClass="fieldlabel" LayoutType="Horizontal" LabelKey="aca_assetdetail_label_assetid" runat="server"/>
    </div>
    <div class="asset_detail_right_column">
        <ACA:AccelaNameValueLabel ID="lblAssetName" CssClass="fieldlabel" LayoutType="Horizontal" LabelKey="aca_assetdetail_label_assetname" runat="server"/>
    </div>
    <div class="ACA_Row">
    </div>
    <div class="asset_detail_left_column">
        <ACA:AccelaNameValueLabel ID="lblClassType" CssClass="fieldlabel" LayoutType="Horizontal" LabelKey="aca_assetdetail_label_classtype" runat="server"/>
    </div>
    <div class="asset_detail_right_column">
        <ACA:AccelaNameValueLabel ID="lblAssetGroup" CssClass="fieldlabel" LayoutType="Horizontal" LabelKey="aca_assetdetail_label_assetgroup" runat="server"/>
    </div>
    <div class="ACA_Row">
    </div>
    <div class="asset_detail_left_column">
        <ACA:AccelaNameValueLabel ID="lblAssetType" CssClass="fieldlabel" LayoutType="Horizontal" LabelKey="aca_assetdetail_label_assettype" runat="server"/>
    </div>
    <ACA:AssetTemplateView runat="server" ID="AssetTemplateView" />
</div>
