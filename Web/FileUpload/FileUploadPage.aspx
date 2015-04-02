<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Dialog.Master" CodeBehind="FileUploadPage.aspx.cs"
    Inherits="Accela.ACA.Web.Attachment.FileUploadPage" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Util" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/FileUpload.js")%>"></script>
    <script type="text/javascript">
        //Remeber the added files.
        var allAddedFileInfos = [];
        //Remeber the deleted files.
        var allDeletedFileInfos = [];
        var fileUploader = null;

        $(document).ready(function () {

            //For resolved the issue that the silverlight button need to click twice under Opera browser.
            if($.browser.opera){
                fileUploader = $('#fileUploader');
                $("#fileUploader param[name='windowless']").remove();
                fileUploader.parent().html(fileUploader[0].outerHTML);
            }

            DisableFinishButton(true);
            SetWizardButtonDisable('<%=btnClearAll.ClientID %>', true);

            //ACADialog close event redirection.
            var dialogCloseBtn = parent.$get(parent.ACADialog.close_id);

            if (dialogCloseBtn) {
                dialogCloseBtn.onclick = function () {
                  return Cancel();
                };
            }
        });

        //Convert IUserFile defined in Silverlight to FileUploadInfo.
        function ConvertUserFileToFileInfo(userFile) {
            var fileInfo = null;

            if (userFile) {
                fileInfo = new Object();
                fileInfo.FileName = userFile.FileName;
                fileInfo.FileId = userFile.FileId;
                fileInfo.FileSize = userFile.FileSize;
                fileInfo.StateString = userFile.StateString;
                fileInfo.MaxFileSize = <%=MaxFileSizeByByte %>;

                //added for resubmit link that need search parent document model by parent document number
                fileInfo.ParentAgencyCode=encodeURIComponent('<%=ParentAgencyCode %>');
                fileInfo.ParentDocumentNo='<%=ParentDocumentNo %>';
            }

            return fileInfo;
        }

        //Silverlight control loaded, attach events handler to control.
        function FileUploader_Loaded() {
            try {
                fileUploader = document.getElementById("fileUploader");

                //File upload finished events.
                fileUploader.Content.Files.AllFilesFinished = AllFilesFinished;
                fileUploader.Content.Files.SingleFileUploadFinished = SingleFileFinished;

                //File removed events.
                fileUploader.Content.Files.AllFileRemoved = AllFileRemoved;
                fileUploader.Content.Files.FileRemoved = SingleFileRemoved;

                //File added event.
                fileUploader.Content.Files.FileAdded = FileAdded;

                //File add-failed event.
                fileUploader.Content.Control.SelectFailed = FileAddingFailed;
            }
            catch (e) {
                /*
                In the Firefox browser, the Sliverlight control's Content property and other ScriptableObject may not be initialized sometimes.
                So we add the special logic to reload the web page if meeting the error, and an alert message will be shown if the reload time is large than 5.
                This is a temporary solution.
                */
                var counter = 0;

                try {
                    var counterStr = '<%=Request.QueryString["counter"] %>';
                    if (/^[0-9]+$/.test(counterStr)) {
                        counter = parseInt(counterStr);
                    }
                }
                catch (err) { }

                if (counter < 5) {
                    var url = window.location.href;
                    var counterPattern = /&counter=\d*/i;
                    counter++;

                    if (counterPattern.test(url)) {
                        window.location.href = url.replace(counterPattern, '&counter=' + counter);
                    }
                    else {
                        window.location.href = url + '&counter=' + counter;
                    }
                }
                else {
                    var errorMsg = 'We are meeting a problem on the Silverlight upload control, please\r\nupgrade your web browser to the latest version or try again later.';
                    alert(errorMsg);
                    parent.ACADialog.close();
                }
            }
        }

        //All files uploading finished
        function AllFilesFinished(sender, args) {
            DisableFinishButton(false);
        }

        //Single file uploading finished.
        function SingleFileFinished(sender, args) {
        }

        /*
        All files removed.
        Disable finish button and remove all files from server.
        */
        function AllFileRemoved(sender, args) {
            DisableFinishButton(true);
            DeleteAllFiles();
            hideMessage();
        }

        /*
        Single file removed.
        Remove single file from server.
        */
        function SingleFileRemoved(sender, args) {
            if (args && args.UserFile) {
                var fileInfo = ConvertUserFileToFileInfo(args.UserFile);

                //Remeber the deleted files.
                allDeletedFileInfos.push(fileInfo);
            }
        }

        //Single file added.
        function FileAdded(sender, args) {
            hideMessage();
            DisableFinishButton(true);

            if (args && args.UserFile) {
                var fileInfo = ConvertUserFileToFileInfo(args.UserFile);
                allAddedFileInfos.push(fileInfo);
            }
        }

        //Show error message when have files add failed.
        function FileAddingFailed(sender, args) {
            hideMessage();
            var errorMessage = '';

            //extErrorMessage
            if (fileUploader.Content.Files.FailedExtensionCount != null && fileUploader.Content.Files.FailedExtensionCount != "0") {
                var typeErrorCount = fileUploader.Content.Files.FailedExtensionCount;
                var typeErrorContent = fileUploader.Content.Files.FailedNamesByExtension;
                var typeErrorPattern = '<%= GetTextByKey("aca_fileupload_msg_typeerror").Replace("'", "\\'")%>';
                errorMessage = String.format(typeErrorPattern, typeErrorCount, typeErrorContent);
            }

            //sizeErrorMessage
            if (fileUploader.Content.Files.FailedSizeCount != null && fileUploader.Content.Files.FailedSizeCount != "0") {
                if (errorMessage != '') {
                    errorMessage += "<br/>";
                }

                var sizeErrorCount = fileUploader.Content.Files.FailedSizeCount;
                var sizeErrorContent = fileUploader.Content.Files.FailedNamesBySize;
                var sizeErrorPattern = '<%= GetTextByKey("aca_fileupload_msg_sizeerror").Replace("'", "\\'")%>';
                errorMessage += String.format(sizeErrorPattern, sizeErrorCount, sizeErrorContent);
            }

            showMessage4Popup(errorMessage, "Error", true, 1, true);
        }

        //Delete all files from server.
        function DeleteAllFiles() {
            DeleteFile(allAddedFileInfos);
            allAddedFileInfos = [];
        }

        //Delete file from server.
        function DeleteFile(fileInfoArray) {
            if (!fileInfoArray || !fileInfoArray.length || fileInfoArray.length <= 0) {
                return;
            }
            
            var fileInfo = Sys.Serialization.JavaScriptSerializer.serialize(fileInfoArray);

            $.ajax({
                type: "POST",
                url: '<%=UPLOAD_HANDLER_URL %>?action=Delete',
                //if file name contains "&", need to encode the data.
                data: "FileList=" + encodeURIComponent(fileInfo),
                success: function () {
                }
            });
        }

        /*
        Click Finish button.
        Send current added files info to server and close dialog.
        */
        function Finish() {
            var fileInfoArray = [];
            
            if($('#fileUploader').is(':visible')) {
                var length = fileUploader.Content.Files.FileList.length;

                if (length > 0) {
                    for (var i = 0; i < length; i++) {
                        var userFile = fileUploader.Content.Files.FileList[i];

                        if (userFile.StateString == '<%=ACAConstant.FINISHED_STATUS %>') {
                            var fileInfo = ConvertUserFileToFileInfo(userFile);
                            fileInfoArray.push(fileInfo);
                        }
                    }
                }
            } else {
                fileInfoArray = upload_<%=ClientID %>.fileInfoArray;
            }

            if (fileInfoArray.length > 0)
            {
                //Remove all deleted files from server.
                DeleteFile(allDeletedFileInfos);

                parent.<%= Request["attachmentCtrlID"] %>_FillDocEditForm(Sys.Serialization.JavaScriptSerializer.serialize(fileInfoArray));

                SelectLP();
                parent.ACADialog.close();
            }

            return false;
        }

        /*
        Click Cancel button.
        Remove all files from server and close dialog.
        */
        function Cancel() {
            //Call the Clear method to cancel the uploading.
            if(fileUploader
               && typeof(fileUploader.Content) != 'unknown' && typeof(fileUploader.Content) != 'undefined' 
               && typeof(fileUploader.Content.Files) != 'unknown'&& typeof(fileUploader.Content.Files) != 'undefined')
            {
                fileUploader.Content.Files.Clear();
            }

            //Remove all added files from server.
            DeleteAllFiles();
            
            parent.ACADialog.notDispose = false;
            parent.ACADialog.close();

            return false;
        }

        //Disable or Enable Finish button.
        function DisableFinishButton(disabled) {
            
            if( !disabled) {
                //if no file uploaded, could not enable button
                if(allAddedFileInfos.length == 0) {
                    return;
                }
                
                //if don't contain Lp, could not enable button
                if($.exists('#<%=divSelectLp.ClientID %>') && $('#<%=divSelectLp.ClientID %>').is(":visible") && $('#<%=ddlProfessional.ClientID %>').val() == "") {
                    return;
                }
            }

            var btnFinish = document.getElementById('<%=btnFinish.ClientID %>');

            if (btnFinish) {
                if (disabled) {
                    btnFinish.parentNode.setAttribute("class", "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
                    btnFinish.parentNode.setAttribute("className", "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
                }
                else {
                    btnFinish.parentNode.setAttribute("class", "ACA_LgButton ACA_LgButton_FontSize");
                    btnFinish.parentNode.setAttribute("className", "ACA_LgButton ACA_LgButton_FontSize");
                }

                DisableButton(btnFinish.id, disabled);
            }
        }
        
        var upload_<%=ClientID %>;
        function <%=ClientID %>_InitialFileUploadControl(fileUploader) {
            fileUploader.initFileUploadControl("messageSpan", '<%=DisallowedFileTypes %>', <%=MaxFileSizeByByte %>,
                '<%= GetTextByKey("aca_fileupload_msg_typeerror").Replace("'", "\\'")%>', '<%= GetTextByKey("aca_fileupload_msg_sizeerror").Replace("'", "\\'")%>',
                '<%= GetTextByKey("aca_fileupload_msg_zerosizeerror").Replace("'", "\\'")%>', '<%=ParentAgencyCode %>', '<%=ParentDocumentNo %>',
                '<%= GetTextByKey("aca_html5upload_msg_notsupport").Replace("'", "\\'")%>', '<%= GetTextByKey("aca_html5upload_msg_uploaderror").Replace("'", "\\'")%>');

            //add event (optional)
            fileUploader.selectCompletedCallBack = function (files) {
                var loading = new ProcessLoading();
                loading.showLoading();
                var fileInfoArray = [];
                try {
                    fileInfoArray = eval(files);
                }
                catch (e) { }
                finally {
                    //display name and progress
                    for (var i = 0; i < fileInfoArray.length; i++) {
                        var fileInfo = fileInfoArray[i];
                        var htmlText = '<div class="fileRow"><div class="fileRowLeft">' + fileInfo.FileName + '</div><div class="fileRowRight" id="' + fileInfo.FileId + '">'
                            + '<div class="divProgressBar"><div class="html5ProgressBar"><div class="bgColor" style="width:0%;"></div><font>0%</font></div></div></div></div></div>';
                        $('#fileList').append(htmlText);
                        fileUploader.startUpload(fileInfo.FileId, '<%=UploadUrl %>', fileInfo.FileId);
                        allAddedFileInfos.push(fileInfo);
                    }
                }
            };
            
            fileUploader.startUploadCallBack = function () {
                var processLoading = new ProcessLoading();
                processLoading.hideLoading();
                
                DisableFinishButton(true);
                SetWizardButtonDisable('<%=btnClearAll.ClientID %>', false);
            };
            
            fileUploader.removeAllCallBack = function() {
                DisableFinishButton(true);
                SetWizardButtonDisable('<%=btnClearAll.ClientID %>', true);
                DeleteAllFiles();
                hideMessage();
            };
            
            fileUploader.allFilesFinishedCallBack = function () {
                DisableFinishButton(false);
                SetWizardButtonDisable('<%=btnClearAll.ClientID %>', false);    
            };
        }
        
        $(document).ready(function() {
            if (<%=(CurrentFileUploadBehavior == FileUploadBehavior.Html5).ToString().ToLower() %>) {
                upload_<%=ClientID %> = new FileUploader();
                upload_<%=ClientID %>.register4UploadController('divFileBrowser', '<%= GetTextByKey("aca_attachment_label_browsefile").Replace("'", "\\'")%>', true, "ACA_LgButton ACA_LgButton_FontSize html5FileUpload");
                <%=ClientID %>_InitialFileUploadControl(upload_<%=ClientID %>);
            }
        });
        
        function ddlProfessional_change(obj) {
            var isEmptyValue = ($(obj).val() == '');

            DisableFinishButton(isEmptyValue);
        }

        function SelectLP() {
            var $ddlProfessional = $('#<%= ddlProfessional.ClientID %>');
            var lpValue = $ddlProfessional.val();

            if (!$.exists('#<%=divSelectLp.ClientID %>') || !$('#<%=divSelectLp.ClientID %>').is(":visible") || lpValue == '') {
                return;
            }

            parent.SelectLPCallBack(lpValue);
            parent.ACADialog.notDispose = false;
            //parent.ACADialog.close();
        }
    </script>
    <div class='fileupload'>
        <div id="divSelectLp" class="ACA_PopupSelectLp_Container" Visible="False" runat="server">
            <div class="ACA_Section_Instruction">
                <ACA:AccelaLabel runat="server" LabelKey="aca_fileuploadlpselect_label_selectprofessional" />
            </div>
            <ACA:AccelaDropDownList runat="server" onchange="ddlProfessional_change(this);" ID="ddlProfessional" />
            <br/>
        </div>
        <div>
            <ACA:AccelaLabel IsNeedEncode="false" ID="lblSizeIntroduction" LabelType="LabelText" runat="server" Visible="false"></ACA:AccelaLabel>
        </div>
        <div>
            <ACA:AccelaLabel IsNeedEncode="false" ID="lblTypeIntroduction" LabelType="LabelText" runat="server" Visible="false"></ACA:AccelaLabel>
        </div>
        <div class='uploadcontrol'>
            <object id="fileUploader" data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="460" height="280">
                <param name="source" value="../ClientBin/mpost.SilverlightMultiFileUpload.xap?<%=CommonUtil.GetRandomUniqueID() %>" />
                <param name="initParams" value="<%=InitParams %>" />
                <param name="background" value="white" />
                <param name="onload" value="FileUploader_Loaded" />
                <param name="minRuntimeVersion" value="4.0.50401.0" />
                <param name="autoUpgrade" value="true" />
                <param name="windowless" value="true" />
                <a class="NotShowLoading" href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0" style="text-decoration: none">
                    <img src='<%= ImageUtil.GetImageURL("installSilverlight.png") %>' alt="Get Microsoft Silverlight"
                        style="border-style: none" />
                </a>
            </object>
        </div>
        <div id="fileList" class="fileList">
        </div>
        <div class="ACA_TabRow">
            <table role='presentation'>
                <tr valign="bottom">
                    <td>
                        <ACA:AccelaButton ID="btnFinish" LabelKey="aca_fileupload_label_finish" runat="server"
                            DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                            DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"
                            CssClass="NotShowLoading" OnClientClick="return Finish();" />
                    </td>
                    <td class="PopupButtonSpace">
                        &nbsp;
                    </td>
                    <td>
                        <div id="divFileBrowser"></div>
                    </td>
                    <td class="PopupButtonSpace">
                        &nbsp;
                    </td>
                    <td>
                        <ACA:AccelaButton ID="btnClearAll" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" CausesValidation="false" 
                        runat="server" LabelKey="per_attachment_label_clearall" OnClientClick="DeleteAllFiles();" />
            
                        <script type="text/javascript">
                            if (<%=(CurrentFileUploadBehavior == FileUploadBehavior.Html5).ToString().ToLower() %>) {
                                $('#fileUploader').addClass("ACA_Hide");
                                $('#fileList').removeClass("ACA_Hide");
                                $('.PopupButtonSpace:eq(0)').removeClass("ACA_Hide");
                                $('.PopupButtonSpace:eq(1)').removeClass("ACA_Hide");
                                $('#divFileBrowser').removeClass("ACA_Hide");
                                $('#<%=btnClearAll.ClientID %>').removeClass("ACA_Hide");
                            } else {
                                $('#fileUploader').removeClass("ACA_Hide");
                                $('#fileList').addClass("ACA_Hide");
                                $('#divFileBrowser').addClass("ACA_Hide");
                                $('.PopupButtonSpace:eq(0)').addClass("ACA_Hide");
                                $('.PopupButtonSpace:eq(1)').addClass("ACA_Hide");
                                $('#<%=btnClearAll.ClientID %>').addClass("ACA_Hide");
                            }
                        </script>
                    </td>
                    <td class="PopupButtonSpace">
                        &nbsp;
                    </td>
                    <td>
                        <ACA:AccelaLinkButton ID="btnCancel" LabelKey="aca_fileupload_label_cancel" runat="server"
                            CssClass="NotShowLoading ACA_LinkButton" OnClientClick="return Cancel();" />
                    </td>
                </tr>
            </table>
        </div>
        <!--Parameter fields for upload control
        ** The following parameters by hidden fields input: **
        maxSize -- file size limitation. e.g.: 30MB
        disAllowedTypes -- disallowed file types. e.g.: .exe,.bat
        filekeyPrefix: -- prefix of the file id.
        languageSetting -- language code for determine the LTR or RTL. e.g.: ar_AE
        isMultipleSelect -- "Y" indicates allow multiple select | other value indicates disallow multiple select.
        selectFileLabel -- label for Select-File button.
        clearListLabel -- label for clear button.
        FilesLable -- label for to showing fiels count.

        ** The following parameters by initParams input: **
        uploadOnSelect -- "Y" indicates auto-upload after files selecting | other value needs click Upload button to start upload.
        uploadURL -- file upload handler url.
        -->

        <input type="hidden" id="maxSize" value="<%=MaxFileSizeWithUnit %>" />
        <input type="hidden" id="disAllowedTypes" value="<%=DisallowedFileTypes %>" />
        <input type="hidden" id="filekeyPrefix" value='<%=FileKeyPrefix %>' />
        <input type="hidden" id="languageSetting" value='<%=LanguageCode %>' />
        <input type="hidden" id="isMultipleSelect" value='<%=IsMultipleUpload %>' />

        <!--Button labels-->
        <input type="hidden" id="selectFileLabel" value='<%= GetTextByKey("aca_fileupload_label_selectfiles").Replace("'", "\\'")%>' />
        <input type="hidden" id="clearListLabel" value='<%= GetTextByKey("aca_fileupload_label_clearlist").Replace("'", "\\'")%>' />
        <input type="hidden" id="FilesLable" value='<%= GetTextByKey("aca_fileupload_label_files").Replace("'", "\\'")%>' />

        <!--Uploading status labels-->
        <input type="hidden" id="FileStates_Pending" value='<%= GetTextByKey("aca_fileupload_label_pending").Replace("'", "\\'")%>' />
        <input type="hidden" id="FileStates_Uploading" value='<%= GetTextByKey("aca_fileupload_label_uploading").Replace("'", "\\'")%>' />
        <input type="hidden" id="FileStates_Deleted" value='<%= GetTextByKey("aca_fileupload_label_deleted").Replace("'", "\\'")%>' />
        <input type="hidden" id="FileStates_Error" value='<%= GetTextByKey("aca_fileupload_label_error").Replace("'", "\\'")%>' />
        <input type="hidden" id="FileStates_Finished" value='<%= GetTextByKey("aca_fileupload_label_finished").Replace("'", "\\'")%>' />
    </div>
</asp:Content>