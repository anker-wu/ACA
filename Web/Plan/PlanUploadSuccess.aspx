<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" Inherits="PlanUploadSuccess"  Codebehind="PlanUploadSuccess.aspx.cs" %>
<%@ Register Assembly="Accela.Web.Controls" Namespace="Accela.Web.Controls" TagPrefix="ACA" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <div id="MainContent" class="ACA_Content">
        <h1>
            Successfully Uploaded Plan
        </h1>
        <br/>
        <p>
            The plan was successfully uploaded and the automated review process is under way.
            You will receive an email once the plan review has completed and the results are
            available for review (typically within 5-10 minutes). You can also check the status
            of the automated review under <asp:HyperLink runat="server" ID="ToPlanList" NavigateUrl="~/plan/PlanList.aspx">Plan Reviews</asp:HyperLink>
            section of this site, and pay for this plan review right now under <asp:HyperLink   ID="urlPlanPay" runat="server" NavigateUrl= "~/plan/PlanPay.aspx?planSeqNbr" >Pay</asp:HyperLink> section of this site.
        </p>
            
    </div>
</asp:Content>
