<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GlobalSearchLPView.ascx.cs"
    Inherits="Accela.ACA.Web.Component.GlobalSearchLPView" %>
<br />
<div>
    <ACA:AccelaLabel ID="lblLPViewTitle" runat="server" LabelKey="per_globalsearch_label_lp" LabelType="SectionTitle" />
    <ACA:AccelaGridView ID="gdvLicenseList" runat="server"  SummaryKey="gdv_globalsearchlpview_licenselist_summary"
        CaptionKey="aca_caption_globalsearchlpview_licenselist"
        ShowExportLink="true" GlobalSearchType="LP" 
        RowStyle-CssClass="" PagerStyle-HorizontalAlign="center" AllowPaging="true" AllowSorting="True"
        OnRowDataBound="LicenseList_RowDataBound" OnPageIndexChanging="LicenseList_PageIndexChanging"
        OnRowCommand="LicenseList_RowCommand" ShowCaption="true" GridViewNumber="60089">
        <Columns>
            <ACA:AccelaTemplateField AttributeName="lnkLicenseProHeader">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkLicenseProHeader" runat="server" CommandName="Header"
                            SortExpression="LicenseNumber" LabelKey="LicenseEdit_LicensePro_LicenseList_License"
                            CausesValidation="false"></ACA:GridViewHeaderLabel>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <asp:HyperLink ID="hlLicenseNumber" runat="server"><strong><ACA:AccelaLabel ID="IblLicenseNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LicenseNumber")%>' /></strong></asp:HyperLink>
                    </div>
                </ItemTemplate>
                <ItemStyle Width="160px" />
                <headerstyle Width="160px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkLicenseTypeHeader">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkLicenseTypeHeader" runat="server" CommandName="Header"
                            SortExpression="LicenseType" LabelKey="LicenseEdit_LicensePro_LicenseList_LicenseType"
                            CausesValidation="false"></ACA:GridViewHeaderLabel>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>                        
                        <ACA:AccelaLabel ID="lblLicenseType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ResLicenseType") %>'></ACA:AccelaLabel>                        
                    </div>
                </ItemTemplate>
                <ItemStyle Width="130px" />
                <headerstyle Width="130px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkLicenseNameHeader">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkLicenseNameHeader" runat="server" CommandName="Header"
                            SortExpression="LicensedProfessionalName" LabelKey="LicenseEdit_LicensePro_LicenseList_LicenseName"
                            CausesValidation="false"></ACA:GridViewHeaderLabel>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblLicenseName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LicensedProfessionalName") %>'></ACA:AccelaLabel>
                    </div>
                </ItemTemplate>
                <ItemStyle Width="200px" />
                <headerstyle Width="200px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkBusinessNameHeader">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                        <ACA:GridViewHeaderLabel ID="lnkBusinessNameHeader" runat="server" CommandName="Header"
                            SortExpression="BusinessName" LabelKey="licenseedit_licensepro_licenselist_businessname"
                            CausesValidation="false"></ACA:GridViewHeaderLabel>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblBusinessName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BusinessName") %>'></ACA:AccelaLabel>
                    </div>
                </ItemTemplate>
                <ItemStyle Width="200px" />
                <headerstyle Width="200px" />
            </ACA:AccelaTemplateField>
        </Columns>
    </ACA:AccelaGridView>
</div>
