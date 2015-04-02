<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" Inherits="PlanList"  Codebehind="PlanList.aspx.cs" %>
<%@ Register Assembly="Accela.Web.Controls" Namespace="Accela.Web.Controls" TagPrefix="ACA" %> 
<%@ Register Src="~/Component/PlanList.ascx" TagName="PlanList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <div class="ACA_TabRow">
             <ACA:AccelaLabel ID="plan_planList_label_history" LabelKey="plan_planList_label_history" runat="server"></ACA:AccelaLabel>
              <uc1:PlanList ID="planListComponent" runat="server"/>
    </div>
</asp:Content>
