<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="CustomerDetail.aspx.cs" Inherits="Accela.ACA.Web.AuthorizedAgent.FishingAndHunttingLicense.CustomerDetail" %>
<%@ Register Src="~/Component/AuthorizedAgentCustomerEdit.ascx" TagName="AuthorizedAgentCustomerEdit" TagPrefix="ACA" %>
<%@ Register Src="~/Component/CapList.ascx" TagName="CapList" TagPrefix="ACA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/MyCollectionMethods.js") %>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/ShoppingCartMethods.js") %>"></script>
        
    <iframe width="0" height="0" id="iframeExport" class="ACA_Hide" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>">
        <%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
<asp:UpdatePanel ID="panelCustomerForm" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divAuthAgentCustomerDetail" class="ACA_Page CustomerDetail ACA_Content">
            <!-- Page header and page flow -->
            <span ID="lblPageHeader" runat="server" class="PageHeader"></span>
            <div class="PageFlowBar">
                <ACA:AccelaLabel ID="lblPageflowStep1" LabelKey="aca_authagent_customer_label_pageflowstep1" runat="server" />
                <span>></span>
                <ACA:AccelaLabel ID="lblPageflowStep2" LabelKey="aca_authagent_customer_label_pageflowstep2" CssClass="CurrentStep" runat="server" />
                <span>></span>
                <ACA:AccelaLabel ID="lblPageflowStep3" LabelKey="aca_authagent_customer_label_pageflowstep3" runat="server" />
            </div>

            <div class="ACA_Section">
                <ACA:AccelaLabel ID="lblSectionPerson" LabelKey="aca_authagent_customerdetail_label_contactheader" runat="server" LabelType="SectionEditable"></ACA:AccelaLabel>
                <!-- The customer form section -->
                <ACA:AuthorizedAgentCustomerEdit ID="customerForm" SectionPosition="DetailForm" ContactSectionPosition="AuthAgentCustomerDetail" runat="server" />
            </div>

            <div id="divLicenses" class="ACA_Section CustomerList" runat="server">
                <ACA:AccelaLabel ID="lblSectionLicense" LabelKey="aca_authagent_customerdetail_label_licensesheader" runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
                <!--The Associated License List-->
                <ACA:CapList ID="LicenseList" ShowPermitAddress="true" runat="server" GViewID="60158" IsHideMap="True"/>
            </div> 
            <!-- The save button section -->
            <ACA:AccelaCheckBox ID="chkVerified" LabelKey="aca_authagent_customerdetail_label_verified" CssClass="ACA_SmLabel ACA_SmLabel_FontSize" runat="server" />

            <div class="CustomerDetailButtons ACA_LiLeft">
	            <ul>
		            <li>
			            <ACA:AccelaButton ID="btnNextStep" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" 
                            LabelKey="aca_authagent_customerdetail_label_save" 
                            OnClientClick="return SubmitEP(this);" OnClick="NextStepButton_Click" runat="server">
                        </ACA:AccelaButton>
                    </li>
                    <li>
                        <ACA:AccelaButton ID="lnkRelocate" LabelKey="aca_authagent_customerdetail_label_relocate" OnClick="RelocateButton_Click" 
                            CssClass="ACA_LinkButton" CausesValidation="false" runat="server" />
                    </li>
                </ul>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
        
<div id="divAdded"class="ACA_Loading_Message ACA_SmLabel ACA_SmLabel_FontSize">
</div>
<asp:HiddenField ID="hfGridId" runat="server" />

<script type="text/javascript">
    with (Sys.WebForms.PageRequestManager.getInstance()) {
        add_endRequest(function (sender, args) {
            if (!$.global.isAdmin) {
                //export file.
                ExportCSV(sender, args);
            }
        });
        
        add_pageLoaded(function () {
            if (!$.global.isAdmin) {
                var chkVerified = document.getElementById("<%=chkVerified.ClientID %>");
                var isEditable = <%=(string.IsNullOrEmpty(ContactSeqNbr) ? true : IsEditable).ToString().ToLower() %>;
                SetWizardButtonDisable('<%=btnNextStep.ClientID %>', (chkVerified ? !chkVerified.checked : true) && isEditable);
            }
        });
    }
    
    //Below two variables is useless, just to prevent js error in Expression.js.
    var LICENSE_CLEAR_BUTTON = "";
    var LICENSE_SEARCH_BUTTON = "";
</script>
</asp:Content>