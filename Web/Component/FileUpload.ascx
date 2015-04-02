<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileUpload.ascx.cs"
    Inherits="Accela.ACA.Web.Component.FileUpload" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Util" %>
<script type="text/javascript">
        var <%=ClientID %>_fileSelectObj;
        var isRemoveErrorMsg = true;
        var allFinishedFileInfoArray = [];
       
        $(document).ready(function() {
            //For resolved the issue that the silverlight button need to click twice under Opera browser.
            if ($.browser.opera) {
                $("#<%=ClientID %>_fileSelect param[name='windowless']").remove();
                $('#<%=ClientID %>_fileSelect').parent().html(fileSelectObj[0].outerHTML);
            }

            if (!<%=(StandardChoiceUtil.GetFileUploadBehavior() == FileUploadBehavior.Html5).ToString().ToLower() %> 
                && (<%= (IsDisplayLinkButton || IsDisplayResubmitIconButton).ToString().ToLower() %>)
                && !CheckSilverlightInstalled()) {
                $('#<%=divSilverlightObj.ClientID %>').hide();
            }
        });

        function <%=ClientID %>_SetSilverlightObject() {
            if (<%=IsDisplayResubmitIconButton.ToString().ToLower() %>) {
                // File upload behavior is 'Basic' and Menu action display style with 'ICO'. 
                // Add 'ACA_FRight' class style let all ico menus display in a line. 
                $('#<%=divSilverlightObj.ClientID %>').addClass("ACA_FRight");
                
                $('#<%=ClientID %>_fileSelect').height(16);
                $('#<%=ClientID %>_fileSelect').width(16);
            } else if (<%=IsDisplayLinkButton.ToString().ToLower() %>) {
                $('#<%=ClientID %>_fileSelect').height(14);
                $('#<%=ClientID %>_fileSelect').width(<%=AdvanceButtonWidth %>);
            } else {
                $('#<%=ClientID %>_fileSelect').width(<%=AdvanceButtonWidth %> + 1);
            }
        }
        
        //Convert IUserFile defined in Silverlight to FileUploadInfo.
        function <%=ClientID %>_ConvertUserFileToFileInfo(userFile) {
            var fileInfo = null;

            if (userFile) {
                fileInfo = new Object();
                fileInfo.FileId = userFile.FileId;
                fileInfo.FileSize = userFile.FileSize;
                fileInfo.FileName = userFile.FileName;
                fileInfo.StateString = userFile.StateString;
                fileInfo.MaxFileSize = <%=MaxFileSizeByByte %>;

                //added for resubmit link that need search parent document model by parent document number
                fileInfo.ParentAgencyCode='<%=ParentAgencyCode %>';
                fileInfo.ParentDocumentNo='<%=ParentDocumentNo %>';
            }

            return fileInfo;
        }
        
        //Silverlight control loaded, attach events handler to control.
        function <%=ClientID %>_FileSelect_Loaded() {
            try {
                    <%=ClientID %>_SetSilverlightObject();

                    if (typeof(setAttachmentListHeight) != 'undefined') {
                        setAttachmentListHeight();
                    }

                    <%=ClientID %>_fileSelectObj = document.getElementById('<%=ClientID %>_fileSelect');

                    //File upload finished events.
                    <%=ClientID %>_fileSelectObj.Content.Files.AllFilesFinished = <%=ClientID %>_AllFilesFinished;

                    //File add-failed event.
                    <%=ClientID %>_fileSelectObj.Content.Control.SelectFailed =<%=ClientID %>_FileAddingFailed;

                    <%=ClientID %>_fileSelectObj.Content.Control.SelectCompleted = <%=ClientID %>_SelectCompleted;
                
                    <%=ClientID %>_fileSelectObj.Content.Files.SingleFileUploadFinished = <%=ClientID %>_SingleFileFinished;
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
                }
            }
        }

        //Actions
        function <%= ClientID %>_StartUpload(fileId) {
            /*
             The 'Add' silverlight button will not get self selected file information when set resubmit link silverlight object to Add silverlight object.
                1. For document Resubmit function -- window.transferObj is passed from the child attachment list window since the Resubmit is a Silverlight control and placed in the attachment list iframe.
                2. For normal upload function -- window.transferObj is the current Silverlight control.
            */
            if (window.transferObj != null) {
                window.transferObj.Content.Control.StartUpload(fileId);
                <%=FunctionRunInStartUpload %>(true);
            }
        }

        //Single file uploading finished.
        function <%=ClientID %>_SingleFileFinished(sender, args) {
            var fileInfo = <%=ClientID %>_ConvertUserFileToFileInfo(args.UserFile);
            allFinishedFileInfoArray.push(fileInfo);

            // IsDisplayLinkButton or IsDisplayResubmitIconButton is true means the upload control used in attachment list iframe for Resubmit link button.
            if(<%= (IsDisplayLinkButton || IsDisplayResubmitIconButton).ToString().ToLower() %>){
                if(typeof (parent.Attachment_FinishedFileInfoFieldID) != 'undefined') {
                    
                    // existing finished fileInfo maybe from other upload file control.
                    var existingFinishedFileInfo = $('#' + parent.Attachment_FinishedFileInfoFieldID, parent.document).val();
                    
                    if (existingFinishedFileInfo != "") {
                        var existingFinishedFileInfoArray = eval(existingFinishedFileInfo);
                        existingFinishedFileInfoArray.push(fileInfo);
                        $('#' + parent.Attachment_FinishedFileInfoFieldID, parent.document).val(window.Sys.Serialization.JavaScriptSerializer.serialize(existingFinishedFileInfoArray));                
                    } else {
                        $('#' + parent.Attachment_FinishedFileInfoFieldID, parent.document).val(window.Sys.Serialization.JavaScriptSerializer.serialize(allFinishedFileInfoArray));                
                    }
                }
            }
            else{
                var existingFinishedFileInfo = $('#<%=hdFinishedFileArray.ClientID%>').val();
                
                if (existingFinishedFileInfo != "") {
                    var existingFinishedFileInfoArray = eval(existingFinishedFileInfo);
                    existingFinishedFileInfoArray.push(fileInfo);
                    $('#<%= hdFinishedFileArray.ClientID %>').val(window.Sys.Serialization.JavaScriptSerializer.serialize(existingFinishedFileInfoArray));
                } else {
                    $('#<%= hdFinishedFileArray.ClientID %>').val(window.Sys.Serialization.JavaScriptSerializer.serialize(allFinishedFileInfoArray));
                }
            }
        }

        //All files uploading finished
        function <%=ClientID %>_AllFilesFinished(sender, args) {
            <%=FunctionRunInAllFilesFinished %>(false);
        }

        //Single file uploading finished.
        function <%=ClientID %>_SelectCompleted(sender, args) 
        {
            var fileInfoArray = [];
            var fileList = <%=ClientID %>_fileSelectObj.Content.Files.FileList;

            if (fileList.length > 0) 
            {
                for (var i = 0; i < fileList.length; i++) 
                {
                    var userFile = fileList[i];
                    
                    if(userFile.StateString == 'Pending')
                    {
                        var fileInfo = <%=ClientID %>_ConvertUserFileToFileInfo(userFile);
                        fileInfoArray.push(fileInfo);
                    }
                }
            }

            if(fileInfoArray.length > 0)
            {
                if(isRemoveErrorMsg) 
                {
                    // Currently IsDisplayLinkButton or IsDisplayResubmitIconButton is true only used for Resubmit link in attachment list iframe.
                    if(<%= (IsDisplayLinkButton || IsDisplayResubmitIconButton).ToString().ToLower() %>) 
                    {
                        $('#<%=ErrorMsgContainerId %>', parent.document).html('');
                    } 
                    else 
                    {
                        $('#<%=ErrorMsgContainerId %>').html('');
                    }
                }

                <%=FunctionRunInSelectCompleted %>(window.Sys.Serialization.JavaScriptSerializer.serialize(fileInfoArray),<%=ClientID %>_fileSelectObj);
            }

            isRemoveErrorMsg = true;
        }

        function <%=ClientID %>_RemoveSingleFile(fileId) {
            if (<%=ClientID %>_fileSelectObj != null) {
                <%=ClientID %>_fileSelectObj.Content.Control.RemoveSingleFile(fileId);
            }
        }

        function <%=ClientID %>_RemoveAllFiles() {
            if (<%=ClientID %>_fileSelectObj != null) {
                <%=ClientID %>_fileSelectObj.Content.Control.RemoveAllFiles();
            }
        }

        //Show error message when have files add failed.
        function <%=ClientID %>_FileAddingFailed(sender, args) {
            hideMessage('<%=ErrorMsgContainerId %>');
            var errorMessage = '';
            isRemoveErrorMsg = false;
            
            //extErrorMessage
            if (<%=ClientID %>_fileSelectObj.Content.Files.FailedExtensionCount != null && <%=ClientID %>_fileSelectObj.Content.Files.FailedExtensionCount != "0") {
                var typeErrorCount = <%=ClientID %>_fileSelectObj.Content.Files.FailedExtensionCount;
                var typeErrorContent = <%=ClientID %>_fileSelectObj.Content.Files.FailedNamesByExtension;
                var typeErrorPattern = '<%= GetTextByKey("aca_fileupload_msg_typeerror").Replace("'", "\\'")%>';
                errorMessage = String.format(typeErrorPattern, typeErrorCount, typeErrorContent);
            }

            //sizeErrorMessage
            if (<%=ClientID %>_fileSelectObj.Content.Files.FailedSizeCount != null && <%=ClientID %>_fileSelectObj.Content.Files.FailedSizeCount != "0") {
                if (errorMessage != '') {
                    errorMessage += "<br/>";
                }

                var sizeErrorCount = <%=ClientID %>_fileSelectObj.Content.Files.FailedSizeCount;
                var sizeErrorContent = <%=ClientID %>_fileSelectObj.Content.Files.FailedNamesBySize;
                var sizeErrorPattern = '<%= GetTextByKey("aca_fileupload_msg_sizeerror").Replace("'", "\\'")%>';
                errorMessage += String.format(sizeErrorPattern, sizeErrorCount, sizeErrorContent);
            }

            //zeroSizeErrorMessage
            if (<%=ClientID %>_fileSelectObj.Content.Files.FailedZeroSizeCount != null && <%=ClientID %>_fileSelectObj.Content.Files.FailedZeroSizeCount != "0") {
                if (errorMessage != '') {
                    errorMessage += "<br/>";
                }

                var zeroSizeErrorCount = <%=ClientID %>_fileSelectObj.Content.Files.FailedZeroSizeCount;
                var zeroSizeErrorContent = <%=ClientID %>_fileSelectObj.Content.Files.FailedNamesByZeroSize;
                var zeroSizeErrorPattern = '<%= GetTextByKey("aca_fileupload_msg_zerosizeerror").Replace("'", "\\'")%>';
                errorMessage += String.format(zeroSizeErrorPattern, zeroSizeErrorCount, zeroSizeErrorContent);
            }

            // IsDisplayLinkButton or IsDisplayResubmitIconButton is true means the upload control used in attachment list iframe for Resubmit link button.
            if(<%= (IsDisplayLinkButton || IsDisplayResubmitIconButton).ToString().ToLower() %>) {
                parent.showMessage('<%=ErrorMsgContainerId %>', errorMessage, "Error", true, 1, true);
            } else {
                showMessage('<%=ErrorMsgContainerId %>', errorMessage, "Error", true, 1, true);
            }
    }
</script>
<div id="divSilverlightObj" runat="server">
    <object class="silverlight_button" id="<%=ClientID %>_fileSelect" data="data:application/x-silverlight-2," type="application/x-silverlight-2" height="24" tabindex="0">
        <param name="source" value="../ClientBin/mpost.SilverlightMultiFileSelect.xap?<%=CommonUtil.GetRandomUniqueID() %>" />
        <param name="initParams" value="<%=InitParams %>" />
        <param name="background" value="transparent" />
        <param name="onload" value="<%=ClientID %>_FileSelect_Loaded" />
        <param name="minRuntimeVersion" value="4.0.50401.0" />
        <param name="windowless" value="<%=Windowless %>" />
        <param name="autoUpgrade" value="true" />
        <a class="NotShowLoading" onclick="javascript:CheckAndSetNoAsk();window.open('<%=Request.Url.Scheme %>://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0');" href="javascript:void(0)"
            style="text-decoration: none">
            <ACA:AccelaLabel CssClass="ACA_Show ACA_Label_FontSize" ID="lblInstallMsg" runat="server" LabelKey="aca_fileupload_msg_silverlight_install"></ACA:AccelaLabel>
            <img src='<% = ImageUtil.GetImageURL("installSilverlight.png") %>' alt="Get Microsoft Silverlight"
                style="border-style: none" />
        </a>
    </object>
    <iframe id='_sl_historyFrame'  style='visibility: hidden; height: 0; width: 0; border: 0px' title="<%=LabelUtil.GetGlobalTextByKey("aca_formdesigner_iframe") %>"><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
    <input type="hidden" id="maxSize" value="<%=MaxFileSizeWithUnit %>" />
    <input type="hidden" id="disAllowedTypes" value="<%=DisallowedFileTypes %>" />
    <input type="hidden" id="filekeyPrefix" value='<%=FileKeyPrefix %>' />
    <input type="hidden" id="languageSetting" value='<%=LanguageCode %>' />
    <input type="hidden" id="isMultipleSelect" value='<%=IsMultipleUpload %>' />
    <input type="hidden" id="selectFileLabel" value='<%= GetTextByKey(SilverlightButtonLabelKey).Replace("'", "\\'")%>' />
    <input type="hidden" id="mouseEnterLeft" value="/Image/btn_bg_l_m.png" />
    <input type="hidden" id="mouseEnterMiddle" value="/Image/bg_btn_center_over.png" />
    <input type="hidden" id="mouseEnterRight" value="/Image/btn_bg_r_m.png" />
    <input type="hidden" id="mouseOutLeft" value="/Image/btn_bg_l_t.png" />
    <input type="hidden" id="mouseOutMiddle" value="/Image/bg_btn_center_normal.png" />
    <input type="hidden" id="mouseOutRight" value="/Image/btn_bg_r_t.png" />
    <input type="hidden" id="resubmitIcon" value="/Image/resubmitIcon.png" />
    <asp:HiddenField runat="server" ID="hdFinishedFileArray" />
</div>
