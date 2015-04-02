<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true"
    CodeBehind="ConditionOfApprovalDetail.aspx.cs" Inherits="Accela.ACA.Web.Cap.ConditionOfApprovalDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <div class="ACA_Page conditiondetailpage">
        <ACA:AccelaLabel ID="lblConditionOfApprovalInfo" IsNeedEncode="false" runat="server" />
        <div id="divConditionOfApprovalDetail" runat="server" visible="false">
            <span><%=ConditionsUtil.ListItemVariables.ConditionName%>: The condition name</span><br />
            <span><%=ConditionsUtil.ListItemVariables.Status %>: The condition status</span><br />
            <span><%=ConditionsUtil.ListItemVariables.Severity %>: The condition severity</span><br />
            <span><%=ConditionsUtil.ListItemVariables.Priority %>: The condition priority</span><br />
            <span><%=ConditionsUtil.ListItemVariables.StatusDate%>: The status date</span><br />
            <span><%=ConditionsUtil.ListItemVariables.AppliedDate %>: The applied date</span><br />
            <span><%=ConditionsUtil.ListItemVariables.ActionByDept %>: The action department</span><br />
            <span><%=ConditionsUtil.ListItemVariables.ActionByUser %>: The action user</span><br />
            <span><%=ConditionsUtil.ListItemVariables.AppliedByDept %>: The applied department</span><br />
            <span><%=ConditionsUtil.ListItemVariables.AppliedByUser%>: The applied user</span><br />
            <span><%=ConditionsUtil.ListItemVariables.ShortComments%>: The short comments</span><br />
            <span><%=ConditionsUtil.ListItemVariables.LongComments%>: The long comments</span><br />
            <span><%=ConditionsUtil.ListItemVariables.AdditionalInformation%>: The additional information</span><br /><br />
            <span>Click the area below to format the condition details using the available variables
                above.</span><br />
            <ACA:AccelaLabel ID="lblConditionsOfApprovalField" LabelType="BodyText" runat="server" />
        </div>
    </div>
    <script type="text/javascript">
            $(document).ready(function () {
                $(".conditiondetailpage a").addClass("NotShowLoading");
            });
    </script>
</asp:Content>
