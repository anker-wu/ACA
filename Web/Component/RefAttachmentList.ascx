<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RefAttachmentList.ascx.cs" Inherits="Accela.ACA.Web.Component.RefAttachmentList" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<ACA:AccelaGridView ID="gdvAttachmentList" runat="server" AllowPaging="true" AllowSorting="true" GridViewNumber="60136" 
    AutoGenerateCheckBoxColumn="true" CheckBoxColumnIndex="0"
    SummaryKey="gdv_attachment_attachmentlist_summary" CaptionKey="aca_caption_attachment_attachmentlist"
    OnRowDataBound="AttachmentList_RowDataBound" PageSize="5" PagerStyle-HorizontalAlign="center"
    PagerStyle-VerticalAlign="bottom" OnGridViewSort="AttachmentList_GridViewSort"
    OnClientSelectAll="RefAttachment_Selected();" OnClientSelectSingle="RefAttachment_Selected();">
    <Columns>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkNameHeader" AttributeName="lnkNameHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkNameHeader" runat="server" SortExpression="Name" LabelKey="per_permitDetail_UploadDocument_Name" ></ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel ID="lblFileName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Attachment.Name.ToString()) %>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkRecordNumberHeader" AttributeName="lnkRecordNumberHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkRecordNumberHeader" runat="server" SortExpression="RecordNumber" LabelKey="per_permitdetail_uploaddocument_recordnumber" ></ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel id="lblRecordNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Attachment.RecordNumber.ToString())%>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkRecordTypeHeader" AttributeName="lnkRecordTypeHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkRecordTypeHeader" runat="server" SortExpression="RecordType" LabelKey="per_permitdetail_uploaddocument_recordtype" ></ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel id="lblRecordType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Attachment.RecordType.ToString())%>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="150px"/>
            <headerstyle Width="150px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkEntityTypeHeader" AttributeName="lnkEntityTypeHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkEntityTypeHeader" runat="server" SortExpression="EntityType" LabelKey="per_permitdetail_uploaddocument_entitytype" ></ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel id="lblEntityType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Attachment.EntityType.ToString())%>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkTypeHeader" AttributeName="lnkTypeHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkTypeHeader" runat="server" SortExpression="Type" LabelKey="per_permitDetail_UploadDocument_Type" ></ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Attachment.ResType.ToString()) %>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkSizeHeader" AttributeName="lnkSizeHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:AccelaLabel ID="lnkSizeHeader" runat="server" IsGridViewHeadLabel="true" LabelKey="per_permitDetail_UploadDocument_Size" ></ACA:AccelaLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel ID="lblSize" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Attachment.Size.ToString()) %>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkDateHeader" AttributeName="lnkDateHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkDateHeader" runat="server" SortExpression="Date" LabelKey="per_permitDetail_UploadDocument_Date" ></ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaDateLabel id="lblDate" runat="server" DateType="ShortDate" Text2='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Attachment.Date.ToString())%>'></ACA:AccelaDateLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkDescriptionHeader" AttributeName="lnkDescriptionHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:AccelaLabel ID="lnkDescriptionHeader" runat="server" LabelKey="aca_attachmentlist_label_description" IsGridViewHeadLabel="true"></ACA:AccelaLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel id="lblDescription" EnableEllipsis="true" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Attachment.Description.ToString())%>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="150px"/>
            <headerstyle Width="150px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkDocumentStatusHeader" AttributeName="lnkDocumentStatusHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkDocumentStatusHeader" runat="server" SortExpression="DocumentStatus" LabelKey="aca_attachmentlist_label_documentstatus" ></ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel id="lblDocumentStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Attachment.DocumentStatus.ToString())%>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkStatusDateHeader" AttributeName="lnkStatusDateHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkStatusDateHeader" runat="server" SortExpression="StatusDate" LabelKey="aca_attachmentlist_label_statusdate"></ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaDateLabel id="lblStatusDate" runat="server" Text2='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Attachment.StatusDate.ToString())%>'></ACA:AccelaDateLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkUploadDateHeader" AttributeName="lnkUploadDateHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkUploadDateHeader" runat="server" SortExpression="UploadDate" LabelKey="aca_attachmentlist_label_uploaddate" ></ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaDateLabel id="lblUploadDate" runat="server" Text2='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Attachment.UploadDate.ToString())%>'></ACA:AccelaDateLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkVirtualFoldersHeader" AttributeName="lnkVirtualFoldersHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkVirtualFoldersHeader" runat="server" SortExpression="VirtualFolders" LabelKey="aca_attachmentlist_label_virtualfolders" ></ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel IsNeedEncode="false" id="lblVirtualFolders" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Attachment.VirtualFolders.ToString())%>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkEntityHeader" AttributeName="lnkEntityHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkEntityHeader" runat="server" SortExpression="Entity" LabelKey="aca_attachmentlist_label_entity" ></ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <ACA:AccelaLabel id="lblEntity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Attachment.Entity.ToString())%>'></ACA:AccelaLabel>
                </div>
            </itemtemplate>
            <ItemStyle Width="100px"/>
            <headerstyle Width="100px"/>
        </ACA:AccelaTemplateField>
    </Columns>
</ACA:AccelaGridView>
<asp:HiddenField ID="hdnSelectLicense" runat="server" />
<script type="text/javascript">
    function RefAttachment_Selected() {
        var ctrId = GetCurrentContinueButtonClientID();
        var selectedValues = $("#<%=gdvAttachmentList.GetSelectedItemsFieldClientID() %>").val();
        var disabled = selectedValues == null || selectedValues == "," ? true : false;
        SetWizardButtonDisable(ctrId, disabled);
    }
</script>