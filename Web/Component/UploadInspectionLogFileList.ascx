<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadInspectionLogFileList.ascx.cs" Inherits="Accela.ACA.Web.Component.UploadInspectionLogFileList" %>

<asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvLogFileList" runat="server" AllowPaging="True" 
            SummaryKey="aca_summary_logfile_list" CaptionKey="aca_caption_logfile_list"
            AllowSorting="true" AutoGenerateColumns="False" PageSize="10" ShowExportLink="true" CheckBoxColumnIndex="0"
            ShowCaption="true" BreakWord="true" PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom" GridViewNumber="60164" OnRowCommand="LogFileList_RowCommand" OnGridViewSort="LogFileList_GridViewSort">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkLogFileNameHeader" ExportDataField="FileName">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkLogFileNameHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="FileName" 
                                LabelKey="aca_inspectionlogfilelist_label_logfilename">
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <asp:LinkButton ID="lnkLogFileName" runat="server" CommandName='<%#COMMAND_DOWNLOAD %>' CssClass="NotShowLoading" CommandArgument='<%# Eval("EntityInfo")%>'>
                                <ACA:AccelaLabel ID="lblLogFileName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FileName") %>'></ACA:AccelaLabel>
                            </asp:LinkButton>
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="110px"/>
                    <headerstyle Width="110px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkLogTimeHeader" ExportDataField="DocDate">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkLogTimeHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="DocDate" 
                                LabelKey="aca_inspectionlogfilelist_label_logtime">
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaDateLabel runat="server" ID="lblLogTime" DateType="DateAndTime" Text='<%#DataBinder.Eval(Container.DataItem, "DocDate") %>' />         
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="110px"/>
                    <headerstyle Width="110px"/>
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
    </ContentTemplate>
</asp:UpdatePanel>