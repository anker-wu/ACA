<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CertifiedExperienceList.ascx.cs" Inherits="Accela.ACA.Web.Component.CertifiedExperienceList" %>
<br />
<asp:UpdatePanel ID="certifiedExperiencePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvCertExperienceList" GridViewNumber="60123" runat="server" ShowExportLink="true" AllowPaging="True" 
        SummaryKey="gdv_certbusiness_experiencelist_summary" CaptionKey="aca_caption_certbusiness_experiencelist"
            AllowSorting="true" ShowCaption="true" PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom" OnRowDataBound="CertExperienceList_RowDataBound">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkClientNameHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkClientNameHeader" runat="server" 
                            CommandName="Header" SortExpression="ClientName" 
                            LabelKey="aca_certbusiness_label_experience_clientname"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                         </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel id="lnkClientName" runat ="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClientName") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="200px"/>
                    <headerstyle Width="200px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkJobValueHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkJobValueHeader" runat="server" 
                            CommandName="Header" SortExpression="JobValue" 
                            LabelKey="aca_certbusiness_label_experience_jobvalue"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                         </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel id="lblJobValue" runat ="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="200px"/>
                    <headerstyle Width="200px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkWorkDateHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkWorkDateHeader" runat="server" 
                            CommandName="Header" SortExpression="WorkDate" 
                            LabelKey="aca_certbusiness_label_experience_workdate"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                         </div>                 
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel id="lblWorkDate" runat ="server"></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="600px"/>
                    <headerstyle Width="600px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkDescriptionHeader">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkDescriptionHeader" runat="server" 
                            CommandName="Header" SortExpression="Description" 
                            LabelKey="aca_certbusiness_label_experience_description"  CausesValidation="false"></ACA:GridViewHeaderLabel>
                         </div>                 
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel id="lnkDescription" runat ="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="600px"/>
                    <headerstyle Width="600px"/>
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
    </ContentTemplate>
</asp:UpdatePanel>
<br />
