<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchResultInfo.ascx.cs" Inherits="Accela.ACA.Web.Component.SearchResultInfo" %>
<%@ Register  Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="ACA"%>
<asp:UpdatePanel ID="updatePanel" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="ACA_TabRow ACA_Line_Content" runat="server" id="divSearchResultList" visible="false">
            <hr style="border-width: 0px" />
        </div>
        <div>
            <ACA:MessageBar ID="noDataMessageForSearchResultList" runat="Server" />
        </div>
        <div class="ACA_TabRow_NoScoll ACA_WrodWrap ACA_TabRow" id="divSearchResultCount" runat="server" visible="false">
            <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap">
                <ACA:AccelaLabel ID="lblSearchResultCountNumSummary" runat="server" CssClass="font13px fontbold"></ACA:AccelaLabel>
            </div>
            <div class="ACA_SmallTable_Row_Reset ACA_WrodWrap">
                <ACA:AccelaLabel ID="lblClickPromptSearchResult" runat="server" CssClass="font13px"></ACA:AccelaLabel>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>