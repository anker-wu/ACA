<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="AssetSearch.aspx.cs" Inherits="Accela.ACA.Web.Asset.AssetSearch" %>

<%@ Register Src="~/Component/AssetSearch.ascx" TagName="AssetSearch" TagPrefix="ACA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <!--   Asset Search Form Begin -->
    <div>
        <ACA:AccelaLabel ID="lblAssetSearchSection" LabelKey="aca_assetsearch_label_sectiontitle" runat="server" LabelType="SectionExText" />
        <div>
            <ACA:AssetSearch ID="assetSearch" runat="server" />
        </div>
    </div>
    <!--   Asset Search Form End -->
    <script language="javascript" type="text/javascript">
        function CloseSearchDialog() {
            SetNotAskForSPEAR();
            parent.ACADialog.close();

            if (parent.HiddenNotice != undefined) {
                parent.HiddenNotice();
            }

            return false;
        }

        if (!$.global.isAdmin) {
            var dialogCloseBtn = parent.$get(parent.ACADialog.close_id);
            
			if (dialogCloseBtn) {
                dialogCloseBtn.onclick = CloseSearchDialog;
            }
        }
    </script>
</asp:Content>
