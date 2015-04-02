<%@ Page Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" 
    CodeBehind="ConditionAdditionalInfoDetail.aspx.cs" Inherits="Accela.ACA.Web.Cap.ConditionAdditionalInfoDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <div class="ACA_Page conditiondetailpage">
        <ACA:AccelaLabel ID="lblAdditionalInformationDetail" IsNeedEncode="false" runat="server" />
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".conditiondetailpage a").addClass("NotShowLoading");
        });
    </script>
</asp:Content>

