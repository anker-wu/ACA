<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="AssetResult.aspx.cs" Inherits="Accela.ACA.Web.Asset.AssetResult" %>

<%@ Register Src="~/Component/AssetResult.ascx" TagName="AssetResult" TagPrefix="ACA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <!--   Asset Result Form Begin -->
    <div>
        <ACA:AccelaLabel ID="lblAssetResultSection" LabelKey="aca_assetresult_label_sectiontitle" runat="server" LabelType="SectionTitle" />
        <div>
            <ACA:AssetResult ID="assetResult" runat="server" />
        </div>
    </div>
    <!--   Asset Result Form Begin -->
</asp:Content>
