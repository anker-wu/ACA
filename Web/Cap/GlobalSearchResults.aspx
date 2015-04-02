<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="GlobalSearchResults.aspx.cs"
    Inherits="Accela.ACA.Web.Cap.GlobalSearchResults" Title="Global Search Result" %>

<%@ Register Src="../Component/GlobalSearchCapView.ascx" TagName="GlobalSearchCapView"
    TagPrefix="uc1" %>
<%@ Register Src="../Component/GlobalSearchLPView.ascx" TagName="GlobalSearchLPView"
    TagPrefix="uc2" %>
<%@ Register Src="../Component/GlobalSearchAPOView.ascx" TagName="GlobalSearchAPOView"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <iframe width="0" height="0" id="iframeExport" style="visibility: hidden" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>"><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
    <div id="MainContent" class="ACA_Content gs_panel">
        <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_page_instruction_globalsearch" LabelType="PageInstruction"
                runat="server" />
        <table role='presentation' border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <!-- title begin -->
                    <h1>
                        <ACA:AccelaLabel ID="lblSearchResults" runat="server" LabelKey="per_globalsearch_label_searchresults" />
                    </h1>
                    <!-- title end -->
                    <!-- loading begin -->
                    <asp:Panel runat="server" ID="pnlLoading">
                        <p>
                            <ACA:AccelaLabel ID="lblLoading" runat="server" LabelKey="capdetail_message_loading" />
                        </p>
                    </asp:Panel>
                    <!-- loading end -->
                    <!-- no results panel begin -->
                    <asp:Panel runat="server" ID="pnlNoResults">
                        <p>
                            <ACA:AccelaLabel ID="lblNoResults" runat="server" LabelKey="per_globalsearch_label_noresultsnotice" />
                        </p>
                    </asp:Panel>
                    <!-- no results panel end -->
                    <!-- results panel begin -->
                    <asp:Panel runat="server" ID="pnlResults">
                        <asp:UpdatePanel ID="upResultsHeader" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="pnlResultsHeader">
                                    <p>
                                        <ACA:AccelaLabel ID="lblSearchConditionNotice" runat="server" LabelKey="per_globalsearch_label_conditionnotice" /> 
                                    </p>
                                        <table role='presentation' runat="server">
                                            <tr>
                                                <td runat="server" id="tdNavigationTitle">
                                                    <p>
                                                        <ACA:AccelaLabel ID="lblNavigationTitle" runat="server" LabelKey="per_globalsearch_label_navigationtitle" />
                                                    </p>
                                                </td>
                                                <td runat="server" id="tdLinkCAPList">
                                                    <p>
                                                        <a runat="server" id="linkCAPList" href="#CAPList" class="NotShowLoading">
                                                            <ACA:AccelaLabel ID="lblCAPNavigation" runat="server" LabelKey="per_globalsearch_label_caplink" /></a>&nbsp;
                                                    </p>
                                                </td>
                                                <td runat="server" id="tdLinkLPList">
                                                    <p>
                                                        <a runat="server" id="linkLPList" href="#LPList" class="NotShowLoading">
                                                            <ACA:AccelaLabel ID="lblLPNavigation" runat="server" LabelKey="per_globalsearch_label_lplink" /></a>&nbsp;
                                                    </p>
                                                </td>
                                                <td runat="server" id="tdLinkAPOList">
                                                    <p>
                                                        <a runat="server" id="linkAPOList" href="#APOList" class="NotShowLoading">
                                                            <ACA:AccelaLabel ID="lblAPONavigation" runat="server" LabelKey="per_globalsearch_label_apolink" /></a>&nbsp;
                                                    </p>
                                                </td>
                                            </tr>
                                        </table>
                                   
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="CapView" EventName="ListLoading" />
                                <asp:AsyncPostBackTrigger ControlID="CapView" EventName="ListLoaded" />
                                <asp:AsyncPostBackTrigger ControlID="APOView" EventName="ListLoading" />
                                <asp:AsyncPostBackTrigger ControlID="APOView" EventName="ListLoaded" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <table role='presentation' border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Panel runat="server" ID="pnlCAPList">
                                        <a name="CAPList"></a>
                                        <div class="gs_list_width gs_list">
                                            <asp:UpdatePanel ID="upCAPView" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <uc1:GlobalSearchCapView ID="CapView" runat="server" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel runat="server" ID="pnlLPList">
                                        <a name="LPList"></a>
                                        <div class="gs_list_width gs_list">
                                            <asp:UpdatePanel ID="upLPView" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <uc2:GlobalSearchLPView ID="LPView" runat="server" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel runat="server" ID="pnlAPOList">
                                        <a name="APOList"></a>
                                        <div class="gs_list_width gs_list">
                                            <asp:UpdatePanel ID="upAPOView" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <uc3:GlobalSearchAPOView ID="APOView" runat="server" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <!-- results panel end -->
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        var exportText = '<%=GetTextByKey("aca_nextPrevNumbericPagerTemplate_exportLink") %>';
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequest);

        function EndRequest(sender, args) {
            hideLoadingPanel();

            //export file.
            ExportCSV(sender, args);
        }
    </script>

</asp:Content>
