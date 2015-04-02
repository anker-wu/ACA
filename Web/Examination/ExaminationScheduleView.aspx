<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Dialog.master"  CodeBehind="ExaminationScheduleView.aspx.cs" Inherits="Accela.ACA.Web.Examination.ExaminationScheduleView" %>

<%@ Register Src="~/Component/ExaminationScheduleView.ascx" TagName="ExaminationDetail" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="Server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>    
    <div class="font11px">         
        <div class="ACA_TabRow">
            <uc1:ExaminationDetail ID="Examination" runat="server" />
        </div>
    </div>
</asp:Content>
