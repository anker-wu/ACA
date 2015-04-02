<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.MultiLicensesEdit" Codebehind="MultiLicensesEdit.ascx.cs" %>
<%@ Register Src="~/Component/LicenseList.ascx" TagName="LicenseList" TagPrefix="uc1" %>
<%@ Register Src="~/Component/LicenseEdit.ascx" TagName="LicenseEdit" TagPrefix="uc2" %>

<asp:UpdatePanel ID="MultiLicensePanel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
    <ACA:AccelaLabel ID="errorMessageLabel" runat="server"></ACA:AccelaLabel>
    <div id="divLicense">
        <uc2:LicenseEdit ID="licenseEdit" runat="server"/>
    </div>
    <div class="ACA_MutipleContactNewLine SectionBottom">
        <uc1:LicenseList ID="licenseList" runat="server" />
    </div>
</ContentTemplate>
</asp:UpdatePanel>
<script language="javascript" type="text/javascript">
    
    AddValidationSectionID('<%=licenseEdit.ClientID %>');

    if (typeof (myValidationErrorPanel) != "undefined") {
        myValidationErrorPanel.registerIDs4Recheck("<%=licenseList.ClientID %>");
    }

    function <%=ClientID %>_RefreshLPList(actionType, componentName) {
        window.setTimeout(function () {
            delayShowLoading();
        }, 100);

        __doPostBack('<%=MultiLicensePanel.UniqueID + REFRESH_LICENSE_LIST %>', componentName + '$' + actionType);
    }

</script>
