<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InspectionDetails.aspx.cs"
    MasterPageFile="~/Dialog.Master" Inherits="Accela.ACA.Web.Inspection.InspectionDetails" %>

<%@ Register Src="~/Component/InspectionDetail.ascx" TagName="InspectionDetail" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="Server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    <div>
        <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_inspectiondetails_label_pageinstruction" LabelType="PageInstruction" runat="server" />
        <uc1:InspectionDetail ID="Inspection" runat="server" />
    </div>
</asp:Content>
