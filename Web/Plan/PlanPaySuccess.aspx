<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" ValidateRequest="false" Inherits="Accela.ACA.Web.Plan.PlanPaySuccess" Title="Plan Review" Codebehind="~/plan/PlanPaySuccess.aspx.cs" %>
<%@ Register Assembly="Accela.Web.Controls" Namespace="Accela.Web.Controls" TagPrefix="ACA" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">

<script type="text/javascript">
    function print_onclick(url)
    {
	    var a=window.open(url, "_blank","top=200,left=200,height=600,width=800,status=yes,toolbar=1,menubar=no,location=no,scrollbars=yes");
    }   
</script>

   
    <div id="MainContent" class="ACA_Content">
        <ACA:AccelaLabel ID="planreview_success_hdr" LabelKey="planreview_success_hdr" runat ="server" ></ACA:AccelaLabel>
        <br />
        <ACA:AccelaLabel ID="planreview_success_subhdr" LabelKey="planreview_success_subhdr" CssClass="ACA_SmLabel ACA_SmLabel_FontSize" runat="server"></ACA:AccelaLabel>   
        <br /><br /><br /><br /><br /><br /><br /><br />
        <br /><br /><br /><br /><br /><br /><br /><br />
    </div>


</asp:Content>
