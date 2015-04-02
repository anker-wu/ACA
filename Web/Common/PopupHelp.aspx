<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true"
    CodeBehind="PopupHelp.aspx.cs" Inherits="Accela.ACA.Web.Common.PopupHelp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <div class="ACA_Content_Popup ACA_Dialog_Content">
        <ACA:AccelaLabel ID="lblHelpText" runat="server" LabelType="BodyText"></ACA:AccelaLabel>
        <div id="divAdditionalTextID">
        </div>

        <script type="text/javascript">
            var additionalTextID = "<%=AdditionalTextID %>";
            var win = window.parent;
            if (additionalTextID != "" && typeof (eval("win." + additionalTextID)) != "undefined") {
                var additionalText = eval("win." + additionalTextID);

                $("#divAdditionalTextID").html(additionalText);
            }
        </script>

    </div>
</asp:Content>
