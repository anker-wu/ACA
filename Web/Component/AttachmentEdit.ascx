<%@ Import Namespace="Accela.ACA.Web.Util" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Common" %>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.AttachmentEdit"
    CodeBehind="AttachmentEdit.ascx.cs" %>
<%@ Register Src="~/Component/DocumentEdit.ascx" TagName="DocumentEdit" TagPrefix="ACA" %>
<%@ Register Src="~/Component/FileUpload.ascx" TagName="FileUpload" TagPrefix="ACA" %>
<div class="attachment_section">
    <div id="divAttachmentField" visible="false" runat="server" class="ACA_TabRow ACA_Page">
        <span>Click to edit and format the section instruction using the available file upload variables:</span><br />
        <span><%=AttachmentUtil.FileUploadVariables.MaximumFileSize%>: The maximum file size allowed</span><br />
        <span><%=AttachmentUtil.FileUploadVariables.ForbiddenFileFormats%>: The forbidden format types</span><br /> 
        <span runat="server" id="spanRequiredDocumentTypesIntroduction" Visible="False"><%=AttachmentUtil.FileUploadVariables.RequiredDocumentTypeFormats%>: Document types required to submit the online application</span>
    </div>
    <div class="ACA_Section_Instruction ACA_Section_Instruction_FontSize">
        <div>
            <ACA:AccelaLabel IsNeedEncode="false" ID="lblSizeIntroduction" LabelKey="aca_fileupload_label_sizelimitation"
                runat="server"></ACA:AccelaLabel>
        </div>
        <div>
            <ACA:AccelaLabel IsNeedEncode="false" ID="lblTypeIntroduction" LabelKey="aca_fileupload_label_typelimitation"
                runat="server"></ACA:AccelaLabel>
        </div>
        <div>
            <ACA:AccelaLabel IsNeedEncode="false" ID="lblRequiredDocumentTypesIntroduction" Visible="False" LabelKey="aca_fileupload_label_required_document_types"
                runat="server"></ACA:AccelaLabel>
        </div>
    </div>
    <div class="ACA_FLeft ACA_LinkButton ACA_Title_Color font12px NotShowLoading ACA_Page">
        <a id="lnkViewPeopleDocument" onclick="ViewPeopleDocument(true)" class="NotShowLoading ACA_Hide"
            href="javascript:void(0);">
            <ACA:AccelaLabel ID="lblViewPeopleDocument" LabelKey="aca_attachmentlist_label_peopleattachment"
                runat="server"></ACA:AccelaLabel>
        </a><a id="lnkViewRecordDocument" onclick="ViewPeopleDocument(false)" class="NotShowLoading ACA_Hide ACA_Unit"
            href="javascript:void(0);">
            <ACA:AccelaLabel ID="lblViewRecordDocument" LabelKey="aca_attachmentlist_label_recordattachment"
                runat="server"></ACA:AccelaLabel>
        </a>
    </div>
    <ACA:AccelaHideLink ID="hlAttachmentListBegin" runat="server" TabIndex="0" ClientIDMode="Static" />
    <iframe id="<%= ClientID %>_iframeAttachmentList"
        src="../FileUpload/AttachmentsList.aspx?<%= UrlConstant.IFRAME_ID %>=<%=ClientID %>&module=<%=ScriptFilter.AntiXssUrlEncode(ModuleName)%>&isInConfirm=<%=IsInConfirmPage%>&isdetail=<%=IsDetailPage %>&isaccountmanager=<%=IsAccountManagerPage %>&isAdmin=<%=AppSession.IsAdmin%>&isPeopleDocument=&<%=UrlConstant.AgencyCode%>=<%=ConfigManager.AgencyCode %>&<%=UrlConstant.IS_FOR_CONDITION_DOCUMENT%>=<%=IsForConditionDocument ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N%>"
        title="<%=LabelUtil.GetGlobalTextByKey(IsDetailPage ? "iframe_capdetail_attachmentlist_title" : "iframe_attachmentedit_attachmentlist_title") %>"
        frameborder="0" width="100%;" height="0px;" scrolling="no">
        <%=String.Format(LabelUtil.GetGlobalTextByKey("iframe_nonsupport_message"), "../FileUpload/AttachmentsList.aspx?module=" + ScriptFilter.AntiXssUrlEncode(ModuleName))%></iframe>
    <ACA:AccelaHideLink ID="hlAttachmentListEnd" runat="server" TabIndex="0" ClientIDMode="Static" />
    <script type="text/javascript">
        // A special veriable used to pass selected file info from the child iframe to current FileUpload control when use Resubmit function to upload file.
        var Attachment_FinishedFileInfoFieldID = '<%=fileSelect.ClientID %>_hdFinishedFileArray';

        $(document).ready(function () {
            AddValidationSectionID('<%=dlDocumentEdit.ClientID %>');
            InitPeopleDocumentLink();
        });

        function InitPeopleDocumentLink() {

            var lnkViewRecordDocument = $('#lnkViewRecordDocument');
            var lnkViewPeopleDocument = $('#lnkViewPeopleDocument');

            if ('<%=AppSession.IsAdmin && IsDetailPage%>'.toLowerCase() == 'true') {
                lnkViewRecordDocument.removeClass("ACA_Hide");
                lnkViewPeopleDocument.removeClass("ACA_Hide");
            }
            else if('<%=IsDetailPage && StandardChoiceUtil.IsEnableAccountAttachment()%>'.toLowerCase() == 'true') {
                lnkViewPeopleDocument.removeClass("ACA_Hide");
            }
        }

        function ViewPeopleDocument(showPeopleDocument) {
            SetNotAskForSPEAR();
            var iframe = $get('<%=ClientID %>_iframeAttachmentList');
            var divUploadLink = $('#<%=divUploadLink.ClientID%>');
            var lnkViewPeopleDocument = $('#lnkViewPeopleDocument');
            var lnkViewRecordDocument = $('#lnkViewRecordDocument');
            var url = iframe.src.replace(/isPeopleDocument=(true|false)?/ig, 'ispeopledocument=' + showPeopleDocument);
            iframe.src = url;

            if (showPeopleDocument) {
                lnkViewRecordDocument.removeClass("ACA_Hide");
                lnkViewPeopleDocument.addClass("ACA_Hide");
                divUploadLink.addClass("ACA_Hide");
                lnkViewRecordDocument.focus();
            }
            else {
                lnkViewRecordDocument.addClass("ACA_Hide");
                lnkViewPeopleDocument.removeClass("ACA_Hide");
                divUploadLink.removeClass("ACA_Hide");
                lnkViewPeopleDocument.focus();
            }
        }

        function CreateShowLoading() {
            var p = new ProcessLoading();
            p.showLoading();
        }

        function HiddenShowLoading() {
            var p = new ProcessLoading();
            p.hideLoading();
        }

        function <%=ClientID %>_btnSelectFromAccount_OnClientClick(targetId) {
            SetNotAskForSPEAR();
            
            $('#<%= ClientID %>_errorMessageLabel').html('');
            
            var openUrl = '<%= Page.ResolveUrl("~/FileUpload/RefAttachmentsList.aspx") %>';
            openUrl += '?<%= UrlConstant.AgencyCode %>=<%= ConfigManager.AgencyCode %>';
            openUrl += "&<%= UrlConstant.IFRAME_ID %>=<%= ClientID %>";
            openUrl += "&<%= ACAConstant.MODULE_NAME %>=<%= ModuleName %>";
            openUrl += "&<%= UrlConstant.SECTION_NAME %>=<%= ComponentName %>";
            
            ACADialog.popup({ url: openUrl, width: 800, height: 500, objectTarget: targetId });           
        }
    </script>
    <div id="divUploadLink" class="ACA_Row" runat="server">
        <script type="text/javascript">
            function <%=this.ClientID %>_SaveDocument() {
                SetNotAskForSPEAR();
                SetCurrentValidationSectionID('<%=dlDocumentEdit.ClientID %>');
                
                if (!<%=(CurrentFileUploadBehavior == FileUploadBehavior.Html5).ToString().ToLower() %>) {
                    <%=ClientID %>_RemoveAllFiles();
                }
                else {
                    SetNotAskForSPEAR();
                    $('#<%= ClientID %>_errorMessageLabel').html('');
                }
            }

            function <%=this.ClientID %>_FillDocEditForm(fileInfos, silverlightObj, uploaderName, isFromAccount) {
                if (typeof (uploaderName) != "undefined") {
                    ShowLoading();
                }

                var fileInfoArray = [];
                try {
                    fileInfoArray = eval(fileInfos);
                }
                catch (e) { }
                finally {
                    if (fileInfoArray && fileInfoArray.length && fileInfoArray.length > 0) {
                        
                        SetNotAskForSPEAR();
                        
                        if (silverlightObj) {
                            window.transferObj = silverlightObj;
                        }
                        
                        if (isFromAccount && <%=(CurrentFileUploadBehavior == FileUploadBehavior.Html5).ToString().ToLower() %>) {
                            for(var i = 0; i < fileInfoArray.length; i++) {
                                <%=ClientID %>_AllFinishedFileInfoArray.push(fileInfoArray[i]);
                            }
                        }
                        
                        __doPostBack('<%=btnFillDocEditForm.UniqueID %>',  fileInfos  + '<%=ACAConstant.SPLIT_CHAR4URL1 %>' + uploaderName);
                    }
                }
            }
            
            function <%=ClientID %>_BrowseFile() {
                SetNotAskForSPEAR();
                var url = '<%=FileUtil.AppendApplicationRoot("FileUpload/FileUploadPage.aspx") %>?multipleupload=y&<%=ACAConstant.MODULE_NAME %>=<%=ModuleName %>&attachmentCtrlID=<%= ClientID %>';
                ACADialog.popup({ url: url, width: 485, height: 400, objectTarget: '<%=btnBrowse.ClientID %>' });

                return false;
            }

            //refresh attachment list iframe to show uploaded files.
            function <%=this.ClientID %>_AfterAttachmentUpload() {
                // clear the file info after save.
				if(<%=(CurrentFileUploadBehavior == FileUploadBehavior.Html5).ToString().ToLower() %>) {
                    <%=ClientID %>_AllFinishedFileInfoArray = [];
                    $('#<%=hdAllFinishedFileArray.ClientID%>').val("");
                    upload_<%=ClientID %>.removeAllFiles();
                }

                $get('<%=this.ClientID %>_iframeAttachmentList').contentWindow.RefreshPage();
            }

            with (Sys.WebForms.PageRequestManager.getInstance()) {
                if ('<%=AppSession.IsAdmin%>'.toLowerCase() == 'false') {
                    add_endRequest(RecheckValidateError);
                }
            }

            function RecheckValidateError() {
                if (typeof (myValidationErrorPanel) != "undefined") {
                    myValidationErrorPanel.recheckAllErrors();
                }
            }
        </script>
        <script type="text/javascript">
            function <%=this.ClientID %>_InitButtons(isShow) {
                <%=this.ClientID %>_InitAddButton();
                <%=this.ClientID %>_InitSaveAndClearAllButtons(isShow);
            }

            function <%=this.ClientID %>_InitAddButton() {
                var btnBasicSilverlight = $('#<%=divFileSelect.ClientID %>');
                var btnAdvancedSilverlight = $('#<%=btnBrowse.ClientID %>');
                var btnHtml5Upload = $('#<%=divHtml5Upload.ClientID %>');
                
                if(!<%= IsShowUploadButton().ToString().ToLower() %>) {
                    btnBasicSilverlight.addClass("ACA_Hide");
                    btnAdvancedSilverlight.addClass("ACA_Hide");
                    btnHtml5Upload.addClass("ACA_Hide");
                    return;
                }

                if (!$.exists(btnAdvancedSilverlight)) {
                    return;
                }

                if(<%=(AppSession.IsAdmin).ToString().ToLower()%>) {
                    btnBasicSilverlight.addClass("ACA_Hide");
                    btnHtml5Upload.addClass("ACA_Hide");
                    btnAdvancedSilverlight.removeClass("ACA_Hide");
                }
                else {
                    if(<%=(CurrentFileUploadBehavior == FileUploadBehavior.Html5).ToString().ToLower() %>) {
                        btnBasicSilverlight.addClass("ACA_Hide");
                        btnAdvancedSilverlight.addClass("ACA_Hide");
                        btnHtml5Upload.removeClass("ACA_Hide");
                    }
                    else if(<%=(CurrentFileUploadBehavior == FileUploadBehavior.Advanced).ToString().ToLower() %>){
                        btnBasicSilverlight.addClass("ACA_Hide");
                        btnAdvancedSilverlight.removeClass("ACA_Hide");
                        btnHtml5Upload.addClass("ACA_Hide");
                    }
                    else{
                        btnBasicSilverlight.removeClass("ACA_Hide");
                        btnAdvancedSilverlight.addClass("ACA_Hide");
                        btnHtml5Upload.addClass("ACA_Hide");
                    }
                }
            }

            function <%=this.ClientID %>_InitSaveAndClearAllButtons(isShow) {
                var btnSaveContainer = $('#<%=btnSaveContainer.ClientID %>');
                var btnSave = $('#<%=btnSave.ClientID %>');
                var btnClearContainer = $('#<%=btnClearContainer.ClientID %>');
                var btnClearAll = $('#<%=btnClearAll.ClientID %>');
                
                if (isShow) {
                    btnSaveContainer.removeClass("ACA_Hide");
                    btnClearContainer.removeClass("ACA_Hide");
                    btnSave.attr("tabindex", "0");
                    btnClearAll.attr("tabindex", "0");
                }
                else {
                    btnSaveContainer.addClass("ACA_Hide");
                    btnClearContainer.addClass("ACA_Hide");
                    btnSave.attr("tabindex", "-1");
                    btnClearAll.attr("tabindex", "-1");
                }
            }

            //Silverlight will call this function when onload this silverlight button
            function GetSilverlightButtonMinWidth() {
                var btnBrowser = $('#<%=btnBrowse.ClientID %>');

                if ($.exists(btnBrowser)) {
                    return (btnBrowser.width() - 12);
                }
                return (btnBrowser.width() - 12);
            }

            function <%=ClientID %>_RemoveSingleFile(fileId) {
                SetNotAskForSPEAR();
                $('#<%= ClientID %>_errorMessageLabel').html('');
                
                if(<%=(CurrentFileUploadBehavior == FileUploadBehavior.Html5).ToString().ToLower() %>) {
                    upload_<%=ClientID %>.removeSingleFile(fileId);

                    for (var i = 0; i < <%=ClientID %>_AllFinishedFileInfoArray.length; i++) {
                        if (fileId == <%=ClientID %>_AllFinishedFileInfoArray[i].FileId) {
                            Array.remove(<%=ClientID %>_AllFinishedFileInfoArray, <%=ClientID %>_AllFinishedFileInfoArray[i]);
                            $('#<%=hdAllFinishedFileArray.ClientID%>').val(window.Sys.Serialization.JavaScriptSerializer.serialize(<%=ClientID %>_AllFinishedFileInfoArray));
                            break;
                        }
                    }
                } else {
                    <%=fileSelect.FunctionRemoveSingleFile %>(fileId);
                }
            }

            function <%=ClientID %>_RemoveAllFiles() {
                SetNotAskForSPEAR();
                $('#<%= ClientID %>_errorMessageLabel').html('');
                
                if(!<%=(CurrentFileUploadBehavior == FileUploadBehavior.Html5).ToString().ToLower() %>) {
                    <%=fileSelect.FunctionRemoveAllFile %>();
                } else {
                    <%=ClientID %>_AllFinishedFileInfoArray = [];
                    $('#<%=hdAllFinishedFileArray.ClientID%>').val("");
                    upload_<%=ClientID %>.removeAllFiles();
                }
            }

            //Disable or Enable Finish button.
            function <%=this.ClientID %>_DisableSaveAttachmentButton(disabled) {
                //only the moment resubmit and add both finished, can enable save button.
                if(!disabled && <%=(CurrentFileUploadBehavior == FileUploadBehavior.Html5).ToString().ToLower() %>) {
                    if($('.divProgressBar').length != <%=ClientID %>_AllFinishedFileInfoArray.length) {
                        return;
                    }
                }

                var btnSave = document.getElementById('<%=btnSave.ClientID %>');

                /* use case:
                 * In page flow 'Add Button' and 'Resubmit Button' or more than one 'Resubmit Button' maybe existing at the same.
                 * Any one can trigger all finish event. but not meaning all upload file control group are finished.
                 * Only Basic mode need control all file array finished, Advance mode don't need, it have finished before data bind.
                 */
                if (!disabled && <%= (CurrentFileUploadBehavior == FileUploadBehavior.Basic).ToString().ToLower() %>){
                    
                    var editingFileIds = eval($("#<%= hdfEditingFileIds.ClientID %>").val());
					
                    // if have more than one attachment section, there are more variable "Attachment_FinishedFileInfoFieldID", it will use the wrong variable.
                    var allFileInfo = $('#<%=fileSelect.ClientID %>_hdFinishedFileArray').val();
                    allFileInfo = eval(allFileInfo);
                    var finishedCount = 0;
                    for(var i = 0; i < editingFileIds.length; i ++) {
                        for(var j = 0; j < allFileInfo.length; j++) {
                            if(editingFileIds[i] == allFileInfo[j].FileId) {
                                finishedCount++;
                                break;
                            }
                        }
                    }
                    
                    if(finishedCount < editingFileIds.length) {
                        return;
                    }
                }
               
                if (btnSave) {
                    if (disabled) {
                        btnSave.parentNode.setAttribute("class", "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
                        btnSave.parentNode.setAttribute("className", "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
                    }
                    else {
                        btnSave.parentNode.setAttribute("class", "ACA_LgButton ACA_LgButton_FontSize");
                        btnSave.parentNode.setAttribute("className", "ACA_LgButton ACA_LgButton_FontSize");
                    }

                    DisableButton(btnSave.id, disabled);
                }
            }

            function <%= ClientID %>_CheckRequired4Document() {
                var btnSave = $('#<%=btnSaveContainer.ClientID %>');

                if (btnSave && btnSave.is(':visible')) {
                    var errorMsg ='<%= GetTextByKey("aca_attachment_message_forceclicksave")%>';
                    showMessage('<%= ClientID %>_errorMessageLabel', errorMsg, "Error", true, 1, true);
                    return false;
                }
                else {
                    return true;
                }
            }

            function GetAdvanceButtonWidth() {
                return $('#<%=btnBrowse.ClientID %>').width();
            }            
            
            function <%=ClientID %>_InitialFileUploadControl(fileUploader) {
                fileUploader.initFileUploadControl("<%= ClientID %>_errorMessageLabel", '<%=DisallowedFileTypes %>', <%=MaxFileSizeByByte %>,
                                                            '<%= GetTextByKey("aca_fileupload_msg_typeerror").Replace("'", "\\'")%>', '<%= GetTextByKey("aca_fileupload_msg_sizeerror").Replace("'", "\\'")%>',
                                                            '<%= GetTextByKey("aca_fileupload_msg_zerosizeerror").Replace("'", "\\'")%>', '<%=ParentAgencyCode %>', '<%=ParentDocumentNo %>',
                                                            '<%= GetTextByKey("aca_html5upload_msg_notsupport").Replace("'", "\\'")%>', '<%= GetTextByKey("aca_html5upload_msg_uploaderror").Replace("'", "\\'")%>');
            
                //add event (optional)
                fileUploader.selectCompletedCallBack = function(files, uploaderName) {
                    var loading = new ProcessLoading();
                    loading.showLoading();
                    <%=ClientID %>_FillDocEditForm(files, null, uploaderName);
                };

                fileUploader.startUploadCallBack = function () { 
                    var loading = new ProcessLoading();
                    loading.hideLoading();
                    <%=ClientID %>_DisableSaveAttachmentButton(true); 
                };

                fileUploader.singleFinishCallBack = <%=ClientID %>_FunctionRunInSingleFinish;

                fileUploader.allFilesFinishedCallBack = function () {
                    <%=ClientID %>_DisableSaveAttachmentButton(false);
                };
            }

            function <%=ClientID %>_FunctionRunInSingleFinish(fileInfo) {
                <%=ClientID %>_AllFinishedFileInfoArray.push(fileInfo);
                $('#<%=hdAllFinishedFileArray.ClientID%>').val(window.Sys.Serialization.JavaScriptSerializer.serialize(<%=ClientID %>_AllFinishedFileInfoArray));
            }

            //the array save the finished file(may include resubmit file), especially for html5 upload.
            var <%=ClientID %>_AllFinishedFileInfoArray = [];
        </script>
        <div class="attachment_edit">
            <asp:UpdatePanel ID="attachmentEditPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <span id="<%= ClientID %>_errorMessageLabel"></span>
                    <ACA:AccelaTextBox ID="txtValidateSaveAction" IsHidden="True" Validate="required"
                        runat="server" ValidationByHiddenTextBox="True" />
                    <ACA:AccelaHideLink ID="lnkFocusAnchor" AltKey="aca_section508_msg_document" TabIndex="-1"
                        runat="server" />
                    <asp:UpdatePanel ID="attatchmentPanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DataList ID="dlDocumentEdit" CssClass="ACA_Row" runat="server" OnItemDataBound="DocumentEdit_ItemDataBound" role='presentation'
                                OnItemCommand="DocumentEdit_ItemCommand">
                                <ItemTemplate>
                                    <div>
                                        <div class='ACA_TabRow ACA_Line_Content'>
                                            &nbsp;</div>
                                        <div class="ACA_Label ACA_Label_FontSize ACA_FRight">
                                            <ACA:AccelaButton ID="btnRemove" CausesValidation="false" CommandName="Remove" runat="server"
                                                LabelKey="per_attachment_label_removefile" />
                                        </div>
                                    </div>
                                    <ACA:DocumentEdit ID="documentEdit" runat="server" />
                                </ItemTemplate>
                            </asp:DataList>
                            <asp:LinkButton ID="btnFillDocEditForm" runat="Server" CssClass="ACA_Hide" OnClick="FillDocEditForm"
                                TabIndex="-1"></asp:LinkButton>
                            <asp:HiddenField runat="server" ID="hdfEditingFileIds"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="divUploadButton" runat="server" class="ACA_Row ACA_LiLeft action_buttons">
                        <ul>
                            <li id="btnSaveContainer" runat="server" class="ACA_Hide">
                                    <ACA:AccelaButton ID="btnSave" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                                    CausesValidation="true" runat="server" LabelKey="aca_attachment_label_savedocinfo"
                                    OnClick="Save" />
                            </li>
                            <li id="btnSelectFromAccountContainer" runat="server" Visible="False">
                                    <ACA:AccelaButton ID="btnSelectFromAccount" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" 
                                        CausesValidation="False" runat="server" LabelKey="aca_attachmentedit_label_selectfromaccount"/>
                            </li>
                            <li>
                                <ACA:AccelaButton ID="btnBrowse" CssClass="ACA_Hide" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                                    CausesValidation="false" runat="server" LabelKey="aca_attachment_label_browsefile" TabIndex="-1"/>
                                <div id="divFileSelect" runat="server">
                                    <ACA:FileUpload ID="fileSelect" runat="server" IsMultipleUpload="Y" AdvanceButtonWidth="GetAdvanceButtonWidth()"
                                        SilverlightButtonLabelKey="aca_attachment_label_browsefile" />
                                </div>
                                <div id="divHtml5Upload" runat="server">
                                </div>
                                <%--to avoid progress bar change from 100% to 0% after post back, use this field to save the finished file.--%>
                                <asp:HiddenField runat="server" ID="hdAllFinishedFileArray" />
                                <ACA:AccelaInlineScript runat="server">
                                    <script type="text/javascript">
                                        if(<%=(CurrentFileUploadBehavior == FileUploadBehavior.Html5).ToString().ToLower() %>) {
                                            var upload_<%=ClientID %> = new FileUploader();
                                            upload_<%=ClientID %>.register4UploadController('<%=divHtml5Upload.ClientID %>', '<%= GetTextByKey("aca_attachment_label_browsefile").Replace("'", "\\'")%>', true, "ACA_LgButton ACA_LgButton_FontSize html5FileUpload");
                                            
                                            <%=ClientID %>_InitialFileUploadControl(upload_<%=ClientID %>);
                                        }

                                        if (typeof (<%= ClientID %>_InitAddButton) != 'undefined') {
                                            <%= ClientID %>_InitAddButton();
                                        }
                                    </script>
                                </ACA:AccelaInlineScript>
                            </li>
                            <li id="btnClearContainer" runat="server" class="ACA_Hide">
                                    <ACA:AccelaButton ID="btnClearAll" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                                    CausesValidation="false" runat="server" LabelKey="per_attachment_label_clearall"
                                    OnClick="ClearAll" />
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
