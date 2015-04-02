<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProviderExaminationDetail.aspx.cs" MasterPageFile="~/Dialog.master" Inherits="Accela.ACA.Web.Examination.ProviderExaminationDetail" %>

<%@ Register src="../Component/RefProviderExaminationDetail.ascx" tagname="RefProviderExaminationDetail" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="Server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    <div class="font11px">
        <div class="ACA_TabRow">
            <uc1:RefProviderExaminationDetail ID="refProviderExaminationDetail"  runat="server" />
        </div>
    </div>
</asp:Content>