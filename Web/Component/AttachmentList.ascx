<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.AttachmentList" Codebehind="AttachmentList.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="PopupActions.ascx" TagName="PopupActions" TagPrefix="ACA" %>
<%@ Register src="FileUpload.ascx" TagName="FileUpload" TagPrefix="ACA" %>

<script language="javascript" type="text/javascript">
    function ViewDocumentDetails(agencyCode, documentNo, specificEntity, focusObjectID) {
        SetNotAskForSPEAR();
        var url = '<%=FileUtil.AppendApplicationRoot("FileUpload/DocumentDetail.aspx") %>?<%=ACAConstant.MODULE_NAME %>=<%=ModuleName %>&isPeopleDocument=<%=IsPeopleDocument %>&<%=UrlConstant.AgencyCode %>=' + agencyCode + '&<%=UrlConstant.DOCUMENT_NO%>=' + documentNo + '&specificEntity=' + specificEntity;
        parent.ACADialog.popup({ url: url, width: 700, height: 280, objectTarget: $get(focusObjectID) });

        return false;
    }

    //resubmit call function
    function BrowseFile(agencyCode, documentNo, focusObjectID) {
        SetNotAskForSPEAR();
        var url = '<%=FileUtil.AppendApplicationRoot("FileUpload/FileUploadPage.aspx") %>?multipleupload=y&<%=ACAConstant.MODULE_NAME %>=<%=ModuleName %>&<%=UrlConstant.AgencyCode %>=' + agencyCode + '&<%=UrlConstant.DOCUMENT_NO%>=' + documentNo + '&attachmentCtrlID=<%= Request[UrlConstant.IFRAME_ID] %>';
        parent.ACADialog.popup({ url: url, width: 485, height: 400, objectTarget: $get(focusObjectID) });

        return false;
    }

    function RemoveDocument(uniqueID) {
        var warnMsg = '<%=GetTextByKey("aca_common_delete_alert_message").Replace("'", "\\'") %>';

        if(confirmMsg(warnMsg)) {
            parent.CreateShowLoading();
            __doPostBack(uniqueID, '');
        }

        return false;
    }

    function BindDocumentList() {
        if (typeof("window.parent")!= "undefined" && typeof("window.parent.UploadAuditedDocumentList")!= "undefined") {
            window.parent.UploadAuditedDocumentList();
        }
    }
</script>
<div id="divActionNotice" runat="server" visible="false" >
    <div class="ACA_Error_Icon" enableviewstate="False" runat="server" id="divImgSuccess" visible="false">
        <img class="ACA_NoBorder" alt="<%=GetTextByKey("img_alt_workflow_complete") %>" src="<%=ImageUtil.GetImageURL("complete.png") %>"/>           
    </div>
    <div class="ACA_Error_Icon" enableviewstate="False" runat="server" id="divImgFailed" visible="false">
        <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>" src="<%=ImageUtil.GetImageURL("error_16.gif") %>"/>            
    </div>
    <div class="ACA_Notice font12px">
        <ACA:AccelaLabel ID="lblActionNoticeDeleteSuccess" class="Notice_Message_Success" runat="server" EnableViewState="False" LabelKey="aca_attachmentlist_label_removedsuccessfully" Visible="false"/>                
        <ACA:AccelaLabel ID="lblActionNoticeDeleteFailed" class="Notice_Message_Failure" runat="server" EnableViewState="False" LabelKey="aca_attachmentlist_label_removedfailed" Visible="false"/>
        <ACA:AccelaLabel ID="lblActionNoticeDeleteCheckResult" class="Notice_Message_Failure" runat="server" EnableViewState="False" LabelKey="aca_accountmanager_msg_deletecheckresult" Visible="false"/>     
    </div>
</div>
<ACA:AccelaHideLink ID="lnkFocusAnchor" AltKey="aca_section508_msg_list" TabIndex="-1" runat="server"/>
<ACA:AccelaGridView ID="gdvAttachmentList" runat="server" AllowPaging="True" AllowSorting="true"
    SummaryKey="gdv_attachment_attachmentlist_summary" CaptionKey="aca_caption_attachment_attachmentlist"
    OnRowDataBound="AttachmentList_RowDataBound" AutoGenerateColumns="False"  PageSize="5" PagerStyle-HorizontalAlign="center"
    PagerStyle-VerticalAlign="bottom" OnRowCommand="AttachmentList_RowCommand">
    <Columns>
        <ACA:AccelaTemplateField AccessibleHeaderText="lnkNameHeader" AttributeName="lnkNameHeader">
            <headertemplate>
                <div class="ACA_Header_Row">
                    <ACA:GridViewHeaderLabel ID="lnkNameHeader" runat="server" SortExpression="Name" LabelKey="per_permitDetail_UploadDocument_Name" ></ACA:GridViewHeaderLabel>
                </div>
            </headertemplate>
            <itemtemplate>
                <div>
                    <asp:LinkButton ID="lnkFileName" runat="server" Visible="false" CommandName='<%#COMMAND_DOWNLOAD %>' CssClass="NotShowLoading"
                        CommandArgument='<%# Eval(ColumnConstant.Attachment.EntityInfo.ToString())%>'>
                        <ACA:AccelaLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Attachment.Name.ToString()) %>'></ACA:AccelaLabel>
                    </asp:LinkButton>
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
        <ACA:AccelaTemplateField AttributeName="lblActionHeader">
            <HeaderTemplate>
                <div >
                    <ACA:AccelaLabel ID="lblActionHeader" runat="server" LabelKey="aca_attachmentlist_label_action" IsGridViewHeadLabel="true"></ACA:AccelaLabel>
                </div>
            </HeaderTemplate>
            <itemtemplate>
                <div class="ACA_FLeft">
                    <ACA:FileUpload ID="resubmitLink" Visible="False" runat="server"
                        IsMultipleUpload="Y"
                        SilverlightButtonLabelKey="aca_attachmentlist_label_action_resubmit"/>
                    <div class="ACA_LinkButton">
                        <ACA:AccelaLinkButton ID="lnkAdvanceResubmit" CssClass="ACA_Hide" LabelKey="aca_attachmentlist_label_action_resubmit" runat="server"></ACA:AccelaLinkButton>
                    </div>
                    <div id="divFileBrowser" class="ACA_Hide" runat="server" />
                    <div class="ACA_FLeft">
                        <ACA:PopupActions ID="actionMenu" Visible="false" runat="server" />
                    </div>
                    <asp:LinkButton ID="lnkRemove" Visible="false" runat="server" CommandName='<%#COMMAND_REMOVE %>'
                        CommandArgument='<%# Eval(ColumnConstant.Attachment.EntityInfo.ToString())%>'></asp:LinkButton>
                </div>
            </itemtemplate>
            <ItemStyle Width="60px"/>
            <HeaderStyle Width="60px"/>
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
        <ACA:AccelaTemplateField AttributeName="lnkReviewStatusHeader">
            <HeaderTemplate>
                <div class="ACA_Header_Row">
                    <ACA:AccelaLabel ID="lnkReviewStatusHeader" runat="server" LabelKey="aca_attachmentlist_label_review_status"></ACA:AccelaLabel>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div>
                    <ACA:AccelaLabel ID="lblReviewStatus" IsNeedEncode="False" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.Attachment.ReviewStatus.ToString())%>'></ACA:AccelaLabel>
                </div>
            </ItemTemplate>
            <ItemStyle Width="280px" />
            <HeaderStyle Width="280px" />
        </ACA:AccelaTemplateField>
    </Columns>
</ACA:AccelaGridView>
<ACA:AccelaHideLink ID="lnkAttachmentEnd" TabIndex="0" runat="server" />
