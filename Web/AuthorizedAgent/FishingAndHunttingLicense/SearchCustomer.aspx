<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="SearchCustomer.aspx.cs" 
    Inherits="Accela.ACA.Web.AuthorizedAgent.FishingAndHunttingLicense.SearchCustomer" %>

<%@ Register Src="~/Component/AuthorizedAgentCustomerEdit.ascx" TagName="AuthorizedAgentCustomerEdit"
    TagPrefix="ACA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <script src="<%=FileUtil.AppendApplicationRoot("Scripts/Dialog.js") %>" type="text/javascript"></script>
    <script src="<%=FileUtil.AppendApplicationRoot("Scripts/popUpDialog.js") %>" type="text/javascript"></script>
    <script type="text/ecmascript" language="javascript">
        var deactivatePopUpDialog;

        with (Sys.WebForms.PageRequestManager.getInstance()) {
            add_endRequest(function (sender, args) {
                //export file.
                ExportCSV(sender, args);
            });
        }

        function PopUpAuthorizedDevice() {
            if ($('#divDeviceCheck').is(':hidden')) {
                deactivatePopUpDialog = new popUpDialog($get('divDeviceCheck'), null, null, null, null);
                deactivatePopUpDialog.showPopUp();
            }
        }

        function CloseAuthorizedDevice() {
            HideLoading();

            if (deactivatePopUpDialog) {
                deactivatePopUpDialog.cancel();
            }
        }
    </script>
    <asp:UpdatePanel ID="panelCustomerForm" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divAuthAgentCustomerSearch" class="ACA_Content">
                <!-- Page header and page flow -->
                <span ID="lblPageHeader" runat="server" class="PageHeader"></span>
                <div class="PageFlowBar">
                    <ACA:AccelaLabel ID="lblPageflowStep1" LabelKey="aca_authagent_customer_label_pageflowstep1" CssClass="CurrentStep" runat="server" />
                    <span>></span>
                    <ACA:AccelaLabel ID="lblPageflowStep2" LabelKey="aca_authagent_customer_label_pageflowstep2" runat="server" />
                    <span>></span>
                    <ACA:AccelaLabel ID="lblPageflowStep3" LabelKey="aca_authagent_customer_label_pageflowstep3" runat="server" />
                </div>

                <div class="ACA_Section">
                    <ACA:AccelaLabel ID="lblSectionCustomerHeader" LabelKey="aca_authagent_customersearch_label_locatecusomter" runat="server" LabelType="SectionExText"></ACA:AccelaLabel>

                    <!-- The search form section -->
                    <ACA:AuthorizedAgentCustomerEdit ID="customerSearchForm" NeedRunExpression="False" SectionPosition="SearchForm" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <iframe width="0" height="0" id="iframeExport" class="ACA_Hide" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>">
        <%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
</asp:Content>
