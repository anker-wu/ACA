<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeoDocumentList.ascx.cs" Inherits="Accela.ACA.Web.Component.GeoDocumentList" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<ACA:AccelaGridView ID="gdvDocumentList" runat="server" AllowPaging="True" GridViewNumber="60117" ShowCaption="true"
    SummaryKey="aca_summary_document_list" CaptionKey="aca_caption_document_list"
    HeaderStyle-CssClass="ACA_TabRow_Header ACA_TabRow_Header_FontSize" AlternatingRowStyle-CssClass="ACA_TabRow_Even ACA_TabRow_Even_FontSize"
    RowStyle-CssClass="ACA_TabRow_Odd ACA_TabRow_Odd_FontSize" AllowSorting="true" AutoGenerateColumns="False" PageSize="10" BreakWord="true" 
    OnRowDataBound="DocumentList_RowDataBound" OnRowCommand="DocumentList_RowCommand" OnPageIndexChanging="DocumentList_PageIndexChanging"
    OnGridViewSort="DocumentList_GridViewSort" PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom">
    <Columns>
        <ACA:AccelaTemplateField AttributeName="lnkNameHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkNameHeader" runat="server" CommandName="Header" SortExpression="Name" CausesValidation="false" 
                        LabelKey ="aca_showgeodocuments_label_name" >
                    </ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <asp:LinkButton ID="lnkFileName"  runat="server" Visible='<%# DataBinder.Eval(Container.DataItem,"DownloadRight") %>' CommandName="download" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' CssClass='NotShowLoading' >
                        <ACA:AccelaLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></ACA:AccelaLabel>
                    </asp:LinkButton>
                    <ACA:AccelaLabel ID="lblFileName" runat="server" Visible='<%# DataBinder.Eval(Container.DataItem, "ReadOnly") %>' Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="120px"/>
            <headerstyle Width="120px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkParcelNumberHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkParcelNumberHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="ParcelNumber" 
                        LabelKey="aca_showgeodocuments_label_parcelnumber" >
                    </ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel id="lblParcelNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ParcelNumber")%>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkRecordIdHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkRecordIdHeader" runat="server"  CausesValidation="false" CommandName="Header" SortExpression="RecordNumber" 
                        LabelKey="aca_showgeodocuments_label_recordid" >
                    </ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel id="lblRecordNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RecordNumber")%>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkRecordTypeHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkRecordTypeHeader" runat="server"  CausesValidation="false" CommandName="Header" SortExpression="RecordType" 
                        LabelKey="aca_showgeodocuments_label_recordtype" >
                    </ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <ACA:AccelaLabel id="lblRecordType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RecordType")%>'></ACA:AccelaLabel>
            </itemtemplate>
            <ItemStyle Width="130px"/>
            <headerstyle Width="130px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkEntityType">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkEntityType" runat="server"  CausesValidation="false" CommandName="Header" SortExpression="EntityType" 
                        LabelKey="aca_showgeodocuments_label_entitytype" >
                    </ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel id="lblEntityType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EntityType")%>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="130px"/>
            <headerstyle Width="130px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkDocumentType">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkDocumentType" runat="server"  CausesValidation="false" CommandName="Header" SortExpression="DocType" 
                        LabelKey="aca_showgeodocuments_label_doctype" >
                    </ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocType") %>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="130px"/>
            <headerstyle Width="130px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkSize">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkSize" runat="server"  CausesValidation="false" CommandName="Header" SortExpression="Size" 
                        LabelKey="aca_showgeodocuments_label_docsize" >
                    </ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel ID="lblSize" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Size") %>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="130px"/>
            <headerstyle Width="130px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkDate">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkDate" runat="server"  CausesValidation="false" CommandName="Header" SortExpression="Date" 
                        LabelKey="aca_showgeodocuments_label_date" >
                    </ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaDateLabel id="lblDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "Date")%>'></ACA:AccelaDateLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="80px"/>
            <headerstyle Width="80px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AttributeName="lnkAgencyHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkAgencyHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="Agency" 
                        LabelKey="aca_showgeodocuments_label_agency" >
                    </ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel id="lblAgency" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Agency")%>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkDescriptionHeader" AttributeName="lnkDescriptionHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:AccelaLabel ID="lnkDescriptionHeader" runat="server" LabelKey="aca_geodocumentlist_label_description" ></ACA:AccelaLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel id="lblDescription" EnableEllipsis="true" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description")%>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="150px"/>
            <headerstyle Width="150px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkDocumentStatusHeader" AttributeName="lnkDocumentStatusHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkDocumentStatusHeader" runat="server" SortExpression="DocumentStatus" LabelKey="aca_geodocumentlist_label_documentstatus" ></ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel id="lblDocumentStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentStatus")%>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkStatusDateHeader" AttributeName="lnkStatusDateHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkStatusDateHeader" runat="server" SortExpression="StatusDate" LabelKey="aca_geodocumentlist_label_statusdate" ></ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaDateLabel id="lblStatusDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "StatusDate")%>'></ACA:AccelaDateLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkUploadDateHeader" AttributeName="lnkUploadDateHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkUploadDateHeader" runat="server" SortExpression="UploadDate" LabelKey="aca_geodocumentlist_label_uploaddate" ></ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaDateLabel id="lblUploadDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, "UploadDate")%>'></ACA:AccelaDateLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkVirtualFoldersHeader" AttributeName="lnkVirtualFoldersHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkVirtualFoldersHeader" runat="server" SortExpression="VirtualFolders" LabelKey="aca_geodocumentlist_label_virtualfolders" ></ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel id="lblVirtualFolders" runat="server" IsNeedEncode="False"></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
    </Columns>
</ACA:AccelaGridView>
