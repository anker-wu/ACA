<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="AssetDetail.aspx.cs" Inherits="Accela.ACA.Web.Asset.AssetDetail" %>

<%@ Register Src="~/Component/AssetDetailView.ascx" TagName="AssetDetail" TagPrefix="ACA" %>
<%@ Register Src="~/Component/AssetAddressList.ascx" TagName="AssetAddressList" TagPrefix="ACA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <div class="asset_detail">
        <!--   Asset Detail Begin -->
        <ACA:AccelaLabel ID="lblAssetDetailSection" LabelKey="aca_assetdetail_label_assetdetailsectiontitle" runat="server" LabelType="SectionTitle" />
        <div>
            <ACA:AssetDetail ID="assetDetail" runat="server" />
        </div>
        <!--   Asset Detail End -->
        <!--   Asset Address List Begin -->
        <div class="addresslist ACA_Grid_OverFlow">
            <ACA:AccelaLabel ID="lblAssetAddressSection" LabelKey="aca_assetdetail_label_assetaddresssectiontitle" runat="server" LabelType="SectionTitle" />
            <div>
                <ACA:AssetAddressList ID="assetAddressList" runat="server" />
            </div>
        </div>
        <!--   Asset Address List End -->
    </div>

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            if (typeof (parent.AdjustPopDialogHeight) != "undefined") {
                parent.AdjustPopDialogHeight();
            }
        });

        function CloseDetailDialog() {
            parent.minHeight = '';
            SetNotAskForSPEAR();
            parent.ACADialog.close();
            
            if (typeof (parent.RefreshAddress) != "undefined") {
                parent.RefreshAddress();
            }

            return false;
        }

        if (!$.global.isAdmin) {
            var dialogCloseBtn = parent.$get(parent.ACADialog.close_id);

            if (dialogCloseBtn) {
                dialogCloseBtn.onclick = CloseDetailDialog;
            }
        }
    </script>
</asp:Content>
