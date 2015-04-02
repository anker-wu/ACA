<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadInspectionList.ascx.cs" Inherits="Accela.ACA.Web.Component.UploadInspectionList" %>
<%@ Import Namespace="Accela.ACA.Common" %>

<asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvInspectionList" runat="server" AllowPaging="True" AllowSorting="true" AutoGenerateColumns="False" PageSize="10" ShowExportLink="true" CheckBoxColumnIndex="0"
            SummaryKey="aca_summary_inspection_list" CaptionKey="aca_caption_inspection_list"
            ShowCaption="true" BreakWord="true" PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom" OnRowDataBound="InspectionList_RowDataBound"
            OnPageIndexChanging="InspectionList_PageIndexChanging" OnPreRender="InspectionList_PreRender" OnGridViewDownload="InspectionList_GridViewDownload" OnGridViewSort="InspectionList_GridViewSort">
            <Columns>
                <ACA:AccelaTemplateField ColumnId="AttachmentExpandLogo" ShowHeader="false" ControlStyle-Width="15px">
                    <ItemTemplate>
                        <ACA:AccelaDiv ID="divLogo" runat="server">
                            <a id="linkExpand_<%=ClientID %>_<%# DataBinder.Eval(Container.DataItem, "InspectionSeqNbr") %>" href="javascript:ExpandAttachmentRow('<%# DataBinder.Eval(Container.DataItem, "InspectionEntity") %>', 'divAttachment_<%=ClientID %>_<%# DataBinder.Eval(Container.DataItem, "InspectionSeqNbr") %>','imgExpand_<%=ClientID %>_<%# DataBinder.Eval(Container.DataItem, "InspectionSeqNbr") %>','linkExpand_<%=ClientID %>_<%# DataBinder.Eval(Container.DataItem, "InspectionSeqNbr") %>')"
                                title="<%=GetTitleByKey("img_alt_expand_icon", string.Empty) %>" class="NotShowLoading">
                                <img id="imgExpand_<%=ClientID %>_<%# DataBinder.Eval(Container.DataItem, "InspectionSeqNbr") %>" alt="<%=GetTextByKey("img_alt_expand_icon") %>" src='<% = ImageUtil.GetImageURL("caret_collapsed.gif") %>' class="ACA_ActionIMG" />
                            </a>
                        </ACA:AccelaDiv>
                    </ItemTemplate>
                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"  />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkRecordIdHeader" ExportDataField="RecordId">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkRecordIdHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="RecordId" 
                                LabelKey="aca_newinspectionlist_label_recordid">
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <asp:HyperLink ID="hlRecordId" runat ="server" >
                                <ACA:AccelaLabel ID="lblRecordId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "RecordId") %>' />
                            </asp:HyperLink>
                            <ACA:AccelaLabel ID="lblRecordIdLabel" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "RecordId") %>' Visible="False" />
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="110px"/>
                    <headerstyle Width="110px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkInspectionTypeHeader" ExportDataField="InspectionType">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkInspectionTypeHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="InspectionType" 
                                LabelKey="aca_newinspectionlist_label_inspectiontype">
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <asp:HyperLink ID="hlInspectionType" runat="server">
                                <ACA:AccelaLabel ID="lblInspectionType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "InspectionType") %>' />
                            </asp:HyperLink>
                            <ACA:AccelaLabel ID="lblInspectionTypeLabel" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "InspectionType") %>' Visible="False" />
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="110px"/>
                    <headerstyle Width="110px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAddressHeader" ExportDataField="Address">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkAddressHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="Address" 
                                LabelKey="aca_newinspectionlist_label_address">
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAddress" Text='<%#DataBinder.Eval(Container.DataItem, "Address") %>' runat="server" />
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="200px"/>
                    <headerstyle Width="200px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkScheduledDateHeader" ExportDataField="ScheduledDate" ExportFormat="ShortDate">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkScheduledDateHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="ScheduledDate" 
                                LabelKey="aca_newinspectionlist_label_scheduleddate">
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div style="margin-right:5px;">
                            <ACA:AccelaDateLabel ID="lblScheduledDate" runat="server" CSS="ACA_SmLabel ACA_SmLabel_FontSize" DateType="ShortDate" Text2='<%#DataBinder.Eval(Container.DataItem, "ScheduledDate") %>' />
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="60px"/>
                    <headerstyle Width="60px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkScheduledStartTimeHeader" ExportDataField="ScheduledStartTime">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkScheduledStartTimeHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="ScheduledStartTime" 
                                LabelKey="aca_newinspectionlist_label_scheduledstarttime">
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div style="margin-right:5px;">
                            <ACA:AccelaLabel ID="lblScheduledStartTime" runat="server" CSS="ACA_SmLabel ACA_SmLabel_FontSize" Text='<%#DataBinder.Eval(Container.DataItem, "ScheduledStartTime") %>' />
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="60px"/>
                    <headerstyle Width="60px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkScheduledEndTimeHeader" ExportDataField="ScheduledEndTime">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkScheduledEndTimeHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="ScheduledEndTime" 
                                LabelKey="aca_newinspectionlist_label_scheduledendtime">
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div style="margin-right:5px;">
                            <ACA:AccelaLabel ID="lblScheduledEndTime" runat="server" CSS="ACA_SmLabel ACA_SmLabel_FontSize" Text='<%#DataBinder.Eval(Container.DataItem, "ScheduledEndTime") %>' />
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="60px"/>
                    <headerstyle Width="60px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkInspectionDateHeader" ExportDataField="InspectionDate" ExportFormat="ShortDate">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkInspectionDateHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="InspectionDate" />
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div style="margin-right:5px;">
                            <ACA:AccelaDateLabel ID="lblInspectionDate" runat="server" CSS="ACA_SmLabel ACA_SmLabel_FontSize" DateType="ShortDate" Text2='<%#DataBinder.Eval(Container.DataItem, "InspectionDate") %>' />
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="60px"/>
                    <headerstyle Width="60px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkStatusHeader" ExportDataField="Status">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkStatusHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="Status" 
                                LabelKey="aca_newinspectionlist_label_status">
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblStatus" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Status") %>' />
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="110px"/>
                    <headerstyle Width="110px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkScoreHeader" ExportDataField="Score">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkScoreHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="Score" 
                                LabelKey="aca_resultedinspectionlist_label_score">
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblScore" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Score") %>' />
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="110px"/>
                    <headerstyle Width="110px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkGradeHeader" ExportDataField="Grade">
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkGradeHeader" runat="server" CausesValidation="false" CommandName="Header" SortExpression="Grade" 
                                LabelKey="aca_resultedinspectionlist_label_grade">
                            </ACA:GridViewHeaderLabel>
                        </div>
                    </headertemplate>
                    <itemtemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblGrade" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Grade") %>' />
                        </div>
                    </itemtemplate>
                    <ItemStyle Width="110px"/>
                    <headerstyle Width="110px"/>
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField ColumnId="AttachmentExpandContent">
                    <ItemTemplate>
                        <tr>
                            <td colspan="100%">
                                <div id='divAttachment_<%=ClientID %>_<%# DataBinder.Eval(Container.DataItem, "InspectionSeqNbr") %>' class="inspection_attchment" style="display:none;">
                                    <div class="upload_section">
                                        <div>Attachment(s): </div>
                                        <a id='lnkAttachment_<%=ClientID %>_<%# DataBinder.Eval(Container.DataItem, "InspectionSeqNbr") %>' title='<%=GetTextByKey("aca_uploadinspresult_label_tip_uploadattachment") %>'
                                            class="NotShowLoading" href="#" onclick="return BrowseFile('<%# DataBinder.Eval(Container.DataItem, "InspectionEntity") %>', '<%=DocumentEntityType.Inspection %>', 'lnkAttachment_<%=ClientID %>_<%# DataBinder.Eval(Container.DataItem, "InspectionSeqNbr") %>');">
                                            <img alt="<%=GetTextByKey("aca_uploadinspresult_label_tip_uploadattachment") %>" src='<% = ImageUtil.GetImageURL("upload.png") %>' class="ACA_ActionIMG" />
                                        </a>
                                    </div>
                                    <div class="file_section"></div>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
        <asp:LinkButton ID="lnkDownloadInspectionAttchment" runat="server" CssClass="ACA_Hide" OnClick="DownloadInspectionAttchmentLink_Click" TabIndex="-1"></asp:LinkButton>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    var CTreeTop = '<%=ImageUtil.GetImageURL("caret_collapsed.gif") %>';
    var ETreeTop = '<%=ImageUtil.GetImageURL("caret_expanded.gif") %>';
    var deleteIcon = '<%=ImageUtil.GetImageURL("delete.png") %>';
    var deleteIconHover = '<%=ImageUtil.GetImageURL("delete_hover.png") %>';
    var altCollapsed = '<%=GetTextByKey("img_alt_collapse_icon").Replace("'","\\'") %>';
    var altExpanded = '<%=GetTextByKey("img_alt_expand_icon").Replace("'","\\'") %>';
    
    function ExpandAttachmentRow(inspectionEntity, divObjId, imgObjId, lnkObjId) {
        if (divObjId == 'undefined' || imgObjId == 'undefined') return;

        var $divObj = $('#' + divObjId);
        var $lnkObj = $('#' + lnkObjId);
        var img = document.getElementById(imgObjId);
       
        if ($divObj.is(':visible') == false) {
            $divObj.show();

            Expanded(img, ETreeTop, altCollapsed);
            $lnkObj.attr('title', altCollapsed);

            GetInspectionAttachments(inspectionEntity);
        }
        else {
            $divObj.hide();
            Collapsed(img, CTreeTop, altExpanded);
            $lnkObj.attr('title', altExpanded);
        }
        
        // set the upload file
        var arrInspectionEntity = inspectionEntity.split('\f');
        
        if (arrInspectionEntity.length >= 7) {
            var inspectionSeqNbr = arrInspectionEntity[4];
            var uploadRight = arrInspectionEntity[6];

            if (uploadRight.toUpperCase() != 'Y') {
                $('#lnkAttachment_<%=ClientID %>_' + inspectionSeqNbr).hide();
            }
        }
    }

    function GetInspectionAttachments(inspectionEntity) {
        // get inspection attachment from server
        PageMethods.GetInspectionAttachments(inspectionEntity, function (result) {
            if (result == undefined || result == null || result.length == 0) {
                HideLoading();
                return "";
            }

            var inspectionSeqNbr;
            var arrInspectionEntity = inspectionEntity.split('\f');

            if (arrInspectionEntity.length >= 5) {
                inspectionSeqNbr = arrInspectionEntity[4];
            }

            var attachmentHtml = "";
            var $divObj = $('#divAttachment_<%=ClientID %>_' + inspectionSeqNbr);
            var $fileSection = $divObj.children('.file_section');

            $fileSection.html("");

            for (var i = 0; i < result.length; i++) {
                var file = result[i].split("\f");

                if (file != undefined && file.length >= 10) {
                    var agencyCode = file[0];
                    var documentNo = file[1];
                    var fileKey = file[2];
                    var entityId = file[3];
                    var entityType = file[4];
                    var fileName = file[5];
                    var moduleName = file[6];
                    var viewRight = file[7];
                    var downloadRight = file[8];
                    var deleteRight = file[9];
                    var documentValue = agencyCode + "\f" + documentNo + "\f" + fileKey + "\f" + entityId + "\f" + entityType + "\f" + moduleName;

                    if (viewRight.toUpperCase() == 'Y') {
                        if (downloadRight.toUpperCase() == 'Y') {
                            attachmentHtml = attachmentHtml + '<div><div><a href="javascript:DownloadAttachment(\''+ documentValue +'\');">'+ fileName +'</a></div>';
                        } else {
                            attachmentHtml = attachmentHtml + '<div><div>'+ fileName +'</div>';
                        }

                        if (deleteRight.toUpperCase() == 'Y') {
                            attachmentHtml = attachmentHtml + '<a title="<%=GetTextByKey("aca_uploadinspresult_label_tip_deleteattachment") %>" href="#" onclick="DeleteAttachments(this, \'' + documentValue +'\');"><img class="ACA_ActionIMG" onmouseover="toggleImageSrc(this)" onmouseout="toggleImageSrc(this)" src="'+ deleteIcon +'" alt="<%=GetTextByKey("aca_uploadinspresult_label_tip_deleteattachment") %>"></a></div>';   
                        } else {
                            attachmentHtml = attachmentHtml + '<div class="space"></div></div>';
                        }
                    }
                }
            }

            if (attachmentHtml != "") {
                $fileSection.append(attachmentHtml);
            }

            HideLoading();
        });
    }

    function DownloadAttachment(documentValue) {
        __doPostBack('<%=lnkDownloadInspectionAttchment.UniqueID %>', documentValue);
    }
    
    function DeleteAttachments(obj, documentValue) {
        var warnMsg = '<%=GetTextByKey("aca_common_delete_alert_message").Replace("'", "\\'") %>';

        if (confirmMsg(warnMsg)) {
            ShowLoading();

            PageMethods.DeleteAttachments(documentValue, function() {
                $(obj).parent('div').hide();
                HideLoading();
            });
        }
    }
    
    function ShowInspectionDetail(pageUrl, objectTargetId) {
        ACADialog.popup({ url: pageUrl, width: 780, height: 456, objectTarget: objectTargetId });
    }

    function toggleImageSrc(obj) {
        if ($(obj).attr('src') == deleteIconHover) {
            $(obj).attr('src', deleteIcon);
        } else {
            $(obj).attr('src', deleteIconHover);
        }
    }
</script>