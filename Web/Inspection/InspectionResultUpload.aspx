<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="InspectionResultUpload.aspx.cs" Inherits="Accela.ACA.Web.Inspection.InspectionResultUpload" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="~/Component/UploadInspectionList.ascx" TagName="UploadInspectionList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/UploadInspectionLogFileList.ascx" TagName="UploadInspectionLogFileList" TagPrefix="ACA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <div class="ACA_Content">
       <div class="Message_Span">
          <asp:UpdatePanel ID="msgUpdatePanel" runat="server"></asp:UpdatePanel>
       </div>
        <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_uploadinspresult_label_page_instruction" LabelType="PageInstruction" runat="server" />
        <!-- New Inspection List Begin -->
        <asp:UpdatePanel ID="newInspectionUpdatePanel" runat="server">
            <ContentTemplate>
                <div class="ACA_Section">
                    <div class="ACA_SectionHeader">
                        <ACA:AccelaLabel ID="lblSectionNewInspection" LabelKey="aca_uploadinspresult_label_sectionheader_newinspection" runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
                    </div>
                    <div class="ACA_SectionBody">
                        <ACA:UploadInspectionList ID="NewInspectionList" GridViewNumber="60162" runat="server" />
                    </div>
                    <div class="inspection_result_button ACA_LinkButton">
                        <ACA:AccelaLinkButton ID="lnkDownloadNewInspections" OnClick="DownloadNewInspectionsButton_Click" CssClass="NotShowLoading" LabelKey="aca_uploadinspresult_label_downloadnewinspection" runat="server"></ACA:AccelaLinkButton>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- New Inspection List End -->
        
        <!-- Resulted Inspection List Begin -->
        <div class="ACA_Section">
            <div class="ACA_SectionHeader">
                <ACA:AccelaLabel ID="lblSectionResultedInspection" LabelKey="aca_uploadinspresult_label_sectionheader_resultedinspection" runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
            </div>
            <div class="ACA_SectionBody">
                <ACA:UploadInspectionList ID="ResultedInspectionList" GridViewNumber="60163" runat="server" />
            </div>
            <div id="divUploadResult" runat="server" />
            <div class="inspection_result_button ACA_LinkButton">
                <ACA:AccelaLinkButton ID="lnkUploadResultedInspection" OnClientClick="return BrowseFile('', 'LP');" LabelKey="aca_uploadinspresult_label_uploadinspectionresult" runat="server"></ACA:AccelaLinkButton>
            </div>
        </div>
        <!-- New Inspection List End -->
        
        <!-- Log File List Begin -->
        <div class="ACA_Section">
            <div class="ACA_SectionHeader">
                <ACA:AccelaLabel ID="lblSectionLogFile" LabelKey="aca_uploadinspresult_label_sectionheader_logfile" runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
            </div>
            <div class="ACA_SectionBody">
                <asp:UpdatePanel ID="upLogFile" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <ACA:UploadInspectionLogFileList runat="server" ID="InspectionLogFileList"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- Log File List End -->
    </div>
    <asp:LinkButton ID="lnkUploadFileCallback" runat="server" CssClass="ACA_Hide" OnClick="UploadFileCallback" TabIndex="-1" />
    <input type="hidden" id="hdnEntity" runat="server"/>
    <input type="hidden" id="hdnEntityType" runat="server"/>
    <input type="hidden" id="hdnFileInfos" runat="server" />

    <script type="text/javascript">
        var prevFileInfos = [];

        // this function will be invoked by FileUploadPage.aspx
        function _FillDocEditForm(fileInfos) {
            var fileInfoArray = [];

            try {
                fileInfoArray = eval(fileInfos);
            }
            catch (e) { }
            finally {
                if (fileInfoArray && fileInfoArray.length && fileInfoArray.length > 0) {
                    var p = new ProcessLoading();
                    p.showLoading();

                    //Delete previous file.
                    DeletePrevFile();

                    for (var i = 0; i < fileInfoArray.length; i++) {
                        prevFileInfos.push(fileInfoArray[i]);
                    }
                    
                    var isUploadLP = ($('#<%= hdnEntityType.ClientID %>').val() == '<%= DocumentEntityType.LP %>');
                    
                    if (!isUploadLP) {
                        var inspectionEntity = $('#<%= hdnEntity.ClientID %>').val();

                        PageMethods.UploadInspectionAttachment(inspectionEntity, fileInfos, function(errorMsg) {
                            if (errorMsg == '') {
                                // because [GetInspectionAttachments] is a asynchronous method, so put the [HideLoading] to this method. 
                                GetInspectionAttachments(inspectionEntity);
                            } else {
                                hideMessage("messageSpan");
                                showNormalMessage(errorMsg, "Error");
                                HideLoading();
                            }
                        });
                    } else if (<%=LPTotalCount %> > 1) {
                        // navigate to select the license professional
                        $('#<%= hdnFileInfos.ClientID %>').val(fileInfos);

                        // [ShowLPSelect] method should be called after the [parent.ACADialog.close();] execute in FileUploadPage.aspx. Or else it will show javascript error message in IE. 
                        //setTimeout("ShowLPSelect()", 0);
                    } else {
                        __doPostBack('<%=lnkUploadFileCallback.UniqueID %>', fileInfos);
                    }
                }
            }
        }
        
        function ShowLPSelect() {
            var url = '<%=FileUtil.AppendApplicationRoot("GeneralProperty/LPPopupSelect.aspx") %>';
            ACADialog.notDispose = true;
            ACADialog.popup({ url: url, width: 485, height: 400, objectTarget: '<%=lnkUploadResultedInspection.ClientID %>' });
        }
        
        // this function will be invoked by GeneralProperty/LPPopupSelect.aspx
        function SelectLPCallBack(licSeqNumber) {
            var fileInfos = $('#<%= hdnFileInfos.ClientID %>').val();
            $('#<%= hdnEntity.ClientID %>').val(licSeqNumber);

            __doPostBack('<%=lnkUploadFileCallback.UniqueID %>', fileInfos);
        }

        function BrowseFile(entity, entityType, targetID) {
            var isLpUpload = false;
            
            if (entityType == 'LP') {
                isLpUpload = true;
                entityType = '<%= DocumentEntityType.LP %>';
            }
            
            if (entityType == '<%= DocumentEntityType.LP %>' && <%=LPTotalCount %> == 0) {
                var message = '<%= GetTextByKey("aca_uploadinspresult_msg_professionalaccessdeny") %>';
                showNormalMessage(message, "Error");
                return false;
            }

            $("#<%= hdnEntity.ClientID %>").val(entity);
            $("#<%= hdnEntityType.ClientID %>").val(entityType);
            var objectTarget = '<%=lnkUploadResultedInspection.ClientID %>';

            if (entityType == '<%= DocumentEntityType.Inspection %>') {
                objectTarget = targetID;
            }

            var url = '<%=FileUtil.AppendApplicationRoot("FileUpload/FileUploadPage.aspx") %>?multipleupload=Y&isLpUpload=' + isLpUpload;
            ACADialog.popup({ url: url, width: 485, height: 400, objectTarget: objectTarget });

            return false;
        }

        //Delete previous file from server side.
        function DeletePrevFile() {
            if (!prevFileInfos || !prevFileInfos.length || prevFileInfos.length <= 0) {
                return;
            }

            var fileInfo = Sys.Serialization.JavaScriptSerializer.serialize(prevFileInfos);

            $.ajax({
                type: "POST",
                url: '<%=UPLOAD_HANDLER_URL %>?action=Delete',
                data: "FileList=" + encodeURIComponent(fileInfo),
                success: function () {}
            });
        }

        if (typeof (ExportCSV) != 'undefined') {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function (sender, args) {
                ExportCSV(sender, args);
            });
        }
    </script>
    <iframe width="0" height="0" id="iframeExport" class="ACA_Hide" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>"><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
</asp:Content>
