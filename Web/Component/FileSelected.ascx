<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileSelected.ascx.cs" Inherits="Accela.ACA.Web.Component.FileSelected" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<script type="text/javascript">
    //Silverlight control loaded, attach events handler to control.
    var fileSelect_<%=divFileSelect.ClientID %> = null;
    function FileSelect_Loaded_<%=divFileSelect.ClientID %>() {
        try {
            fileSelect_<%=divFileSelect.ClientID %> = document.getElementById("fileSelect_<%=divFileSelect.ClientID %>"); 
            fileSelect_<%=divFileSelect.ClientID %>.Content.Control.SelectFailed = FileAddingFailed_<%=divFileSelect.ClientID %>;
            fileSelect_<%=divFileSelect.ClientID %>.Content.Files.SingleFileUploadFinished = SingleFileFinished_<%=divFileSelect.ClientID %>;
            fileSelect_<%=divFileSelect.ClientID %>.Content.Control.SelectCompleted = SelectCompleted_<%=divFileSelect.ClientID %>;
            SetSilverlightObjectWidth_<%=divFileSelect.ClientID %>();
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

    function FileAddingFailed_<%=divFileSelect.ClientID %>() {
        hideMessage();
        var errorMessage = '';
        isRemoveErrorMsg = false;
        var fileSelectObj = fileSelect_<%=divFileSelect.ClientID %>;
                
        //extErrorMessage
        if (fileSelectObj.Content.Files.FailedExtensionCount != null && fileSelectObj.Content.Files.FailedExtensionCount != "0") {
            var typeErrorCount = fileSelectObj.Content.Files.FailedExtensionCount;
            var typeErrorContent = fileSelectObj.Content.Files.FailedNamesByExtension;
            var typeErrorPattern = '<%= GetTextByKey("aca_fileupload_msg_typeerror").Replace("'", "\\'")%>';
            errorMessage = String.format(typeErrorPattern, typeErrorCount, typeErrorContent);
        }

        //sizeErrorMessage
        if (fileSelectObj.Content.Files.FailedSizeCount != null && fileSelectObj.Content.Files.FailedSizeCount != "0") {
            if (errorMessage != '') {
                errorMessage += "<br/>";
            }

            var sizeErrorCount = fileSelectObj.Content.Files.FailedSizeCount;
            var sizeErrorContent = fileSelectObj.Content.Files.FailedNamesBySize;
            var sizeErrorPattern = '<%= GetTextByKey("aca_fileupload_msg_sizeerror").Replace("'", "\\'")%>';
            errorMessage += String.format(sizeErrorPattern, sizeErrorCount, sizeErrorContent);
        }
            
        //zeroSizeErrorMessage
        if (fileSelectObj.Content.Files.FailedZeroSizeCount != null && fileSelectObj.Content.Files.FailedZeroSizeCount != "0") {
            if (errorMessage != '') {
                errorMessage += "<br/>";
            }

            var zeroSizeErrorCount = fileSelectObj.Content.Files.FailedZeroSizeCount;
            var zeroSizeErrorContent = fileSelectObj.Content.Files.FailedNamesByZeroSize;
            var zeroSizeErrorPattern = '<%= GetTextByKey("aca_fileupload_msg_zerosizeerror").Replace("'", "\\'")%>';
            errorMessage += String.format(zeroSizeErrorPattern, zeroSizeErrorCount, zeroSizeErrorContent);
        }

        showMessage('errorMessageLabel', errorMessage, "Error", true, 1, true);
    }

    function SelectCompleted_<%=divFileSelect.ClientID %>() {
        var fileInfoArray = [];
        var fileList = fileSelect_<%=divFileSelect.ClientID %>.Content.Files.FileList;
        for (var i = 0; i < fileList.length; i++) {
            var userFile = fileList[i];

            if (userFile.StateString == 'Pending') {
                var fileInfo = ConvertUserFileToFileInfo_<%=divFileSelect.ClientID %>(userFile);
                fileInfoArray.push(fileInfo);
                
                $("#<%= hdFileId.ClientID %>").val(fileInfo.FileId);
                $("#<%= hdFileName.ClientID %>").val(fileInfo.FileName);
            }
        }
            
        if (fileInfoArray.length > 0) {
            if (isRemoveErrorMsg) {
                $('#errorMessageLabel').html('');
            }

            UpdateFileInfo_<%=divFileSelect.ClientID %>(window.Sys.Serialization.JavaScriptSerializer.serialize(fileInfoArray));
        }

        isRemoveErrorMsg = true;
    }
        
    function UpdateFileInfo_<%=divFileSelect.ClientID %>(fileInfos) {
        $('#errorMessageLabel').html('');
        var fileInfoArray = [];

        try {
            fileInfoArray = eval(fileInfos);
        }
        catch (e) { }
        finally {
            if (fileInfoArray && fileInfoArray.length && fileInfoArray.length > 0) {
                SetNotAskForSPEAR();
                __doPostBack('<%=btnUpdateFileInfo.UniqueID %>', fileInfos);
            }
        }
    }
    
    // Note: Need to ensure the GetSilverlightButtonMinWidth() exist. Othersize, this width setting is invalid.
    function SetSilverlightObjectWidth_<%=divFileSelect.ClientID %>() {
        var btnBrowser = $('#<%=btnBrowser.ClientID %>');

        /*
         * Note: Can't get btnBrowser'width property accurately in dispaly is hide status. 
         * So need show it first then hide it again.
         */
        btnBrowser.removeClass("ACA_Hide");
        $('#fileSelect_<%=divFileSelect.ClientID %>').width(btnBrowser.width());
        btnBrowser.addClass("ACA_Hide");
    }

    //Silverlight will call this function when onload this silverlight button
    function GetSilverlightButtonMinWidth() {
        var btnBrowser = $('#<%=btnBrowser.ClientID %>');

        /*
         * Note: Can't get btnBrowser'width property accurately in dispaly is hide status. 
         * So need show it first then hide it again.
         */
        btnBrowser.removeClass("ACA_Hide");
        var width = btnBrowser.width() - 12;
        btnBrowser.addClass("ACA_Hide");
        return width;
    }

    function SingleFileFinished_<%=divFileSelect.ClientID %>(sender, args) {
        var fileInfo = ConvertUserFileToFileInfo_<%= divFileSelect.ClientID %>(args.UserFile);
        SetNotAskForSPEAR();
        
        var conditionNumber = $("#<%=divFileSelect.ClientID %>").find("#conditionNumber").val();
        PageMethods.SaveConditionDocToEDMS("<%= ModuleName %>", conditionNumber, '<%=divFileSelect.ClientID %>', JSON.stringify(fileInfo), function(data) {
            var result = eval("(" + data + ");");
            var state = result.Success ? "Success" : "Error";
            showMessage('errorMessageLabel', result.Messages, state, true, 1, true);

            $("#" + result.ClientId).find("object[id=fileProgress]").hide();
            //Clear file name value.
            if (!result.Success) {
                $("#" + result.ClientId).find("input[id$='txtFileName']").val("");
            }
        }); 

        var fileSelectControl = $("#" + fileInfo.FileSelectedID)[0];
        if (fileSelectControl != null) {
            fileSelectControl.Content.Control.SetIsEnabled(true);
        }
    }

    function ConvertUserFileToFileInfo_<%=divFileSelect.ClientID %>(userFile) {
        var fileInfo = null;

        if (userFile) {
            fileInfo = new Object();
            fileInfo.FileId = userFile.FileId;
            fileInfo.FileSize = userFile.FileSize;
            fileInfo.FileName = userFile.FileName;
            fileInfo.StateString = userFile.StateString;
            fileInfo.MaxFileSize = <%=MaxFileSizeByByte %>;
            fileInfo.FileSelectedID = "fileSelect_<%=divFileSelect.ClientID %>";
            fileInfo.ParentAgencyCode='<%=ParentAgencyCode %>';
            fileInfo.ParentDocumentNo='<%=ParentDocumentNo %>';
        }

        return fileInfo;
    }
        
    function StartUpload_<%=divFileSelect.ClientID %>(fileId, fileSelectId) {
        
        var fileSelectControl = $("#" + fileSelectId)[0];
        if (fileSelectControl != null) {
            fileSelectControl.Content.Control.StartUpload(fileId);
            fileSelectControl.Content.Control.SetIsEnabled(false);
        }
    }

    function RemoveSingleFile_<%=divFileSelect.ClientID %>(fileId, fileSelectId) {
        SetNotAskForSPEAR();
        $('#errorMessageLabel').html('');
        var fileSelectControl = $("#" + fileSelectId)[0];
        if (fileSelectControl != null) {
            fileSelectControl.Content.Control.RemoveSingleFile(fileId);
            fileSelectControl.Content.Control.SetIsEnabled(true);
        }
    }

    var upload_<%=ClientID %>;
    function <%=ClientID %>_InitialFileUploadControl(fileUploader) {
        $('#<%=divHtml5Upload.ClientID %>').width($('#<%=btnBrowser.ClientID %>').width());
        
        fileUploader.initFileUploadControl("errorMessageLabel", '<%=DisallowedFileTypes %>', <%=MaxFileSizeByByte %>,
            '<%= GetTextByKey("aca_fileupload_msg_typeerror").Replace("'", "\\'")%>', '<%= GetTextByKey("aca_fileupload_msg_sizeerror").Replace("'", "\\'")%>',
            '<%= GetTextByKey("aca_fileupload_msg_zerosizeerror").Replace("'", "\\'")%>', '<%=ParentAgencyCode %>', '<%=ParentDocumentNo %>',
            '<%= GetTextByKey("aca_html5upload_msg_notsupport").Replace("'", "\\'")%>', '<%= GetTextByKey("aca_html5upload_msg_uploaderror").Replace("'", "\\'")%>');

        //add event (optional)
        fileUploader.selectCompletedCallBack = function(fileInfos) {
            var loading = new ProcessLoading();
            loading.showLoading();
            UpdateFileInfo_<%=divFileSelect.ClientID %>(fileInfos);
        };
        
        fileUploader.startUploadCallBack = function () {
            //disable the add button
            var btnId = fileUploader.fileInputObjId;
            DisableButton(btnId, true);
   
            var $btnWithContainer = $("#"+ btnId);
            if ($.exists($btnWithContainer)) {
                $btnWithContainer.parent().parent().removeClass("ACA_LgButton ACA_LgButton_FontSize");
                $btnWithContainer.parent().parent().addClass("ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
            }
        };
        
        fileUploader.removeSingleCallBack = function() {
            $('#errorMessageLabel').html('');

            //enable the add button
            var btnId = fileUploader.fileInputObjId;
            DisableButton(btnId, false);
  
            var $btnWithContainer = $("#"+ btnId);
            if ($.exists($btnWithContainer)) {
                $btnWithContainer.parent().parent().removeClass("ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
                $btnWithContainer.parent().parent().addClass("ACA_LgButton ACA_LgButton_FontSize");
            }
        };

        fileUploader.allFilesFinishedCallBack = function () {
            SetNotAskForSPEAR();
            var conditionNumber = $("#<%=divFileSelect.ClientID %>").find("#conditionNumber").val();
            
            $("#<%= hdFileId.ClientID %>").val(fileUploader.fileInfoArray[0].FileId);
            $("#<%= hdFileName.ClientID %>").val(fileUploader.fileInfoArray[0].FileName);

            if (fileUploader.fileInfoArray.length == 0) {
                return;
            }

            PageMethods.SaveConditionDocToEDMS("<%= ModuleName %>", conditionNumber, '<%=divFileSelect.ClientID %>', JSON.stringify(fileUploader.fileInfoArray[0]), function(data) {
                var result = eval("(" + data + ");");
                var state = result.Success ? "Success" : "Error";
                showMessage('errorMessageLabel', result.Messages, state, true, 1, true);

                $("#" + result.ClientId).find(".divProgressBar").hide();
                //Clear file name value.
                if (!result.Success) {
                    $("#" + result.ClientId).find("input[id$='txtFileName']").val("");
                }

                fileUploader.filesArray = [];
                fileUploader.fileInfoArray = [];

                var btnId = fileUploader.fileInputObjId;
                DisableButton(btnId, false);

                var $btnWithContainer = $("#" + btnId);
                if ($.exists($btnWithContainer)) {
                    $btnWithContainer.parent().parent().removeClass("ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
                    $btnWithContainer.parent().parent().addClass("ACA_LgButton ACA_LgButton_FontSize");
                }
            });
        };
    }
    
    function RemoveSingleFileByHTML5_<%= ClientID %>(fileId) {
        upload_<%=ClientID %>.removeSingleFile(fileId);
    }

</script>
<div id="divFileSelect" class="FileSelect" runat="server">
    <table role="presentation" style="width: 100%;">
        <tr>
            <td class="fileInfo">
                <asp:UpdatePanel ID="attatchmentPanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="divFileName" class="ACA_FLeft ACA_DivMargin6">
                            <ACA:AccelaNameValueLabel ID="lblFileName" IsDBRequired="true" Font-Bold="true" runat="server"></ACA:AccelaNameValueLabel>
                        </div>
                        <div class="ACA_FLeft">
                            <ACA:AccelaTextBox runat="server" ID="txtFileName" CssClass="UploadFileName" ReadOnly="True"/>
                        </div>
                        <asp:LinkButton ID="btnUpdateFileInfo" runat="Server" CssClass="ACA_Hide" OnClick="UpdateFileInfo_Click" TabIndex="-1"></asp:LinkButton>
                        <ACA:AccelaButton ID="btnBrowser" CssClass="ACA_Hide" DivEnableCss="ACA_LgButton fontSize4Condition" CausesValidation="false" TabIndex="-1" runat="server" LabelKey="aca_attachment_label_browsefile"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td class="addInfo">
                <div runat="server" id="btnUploadDocument" visible="False">
                    <object name="fileUploadObj" class="silverlight_button" id="fileSelect_<%=divFileSelect.ClientID %>" data="data:application/x-silverlight-2," type="application/x-silverlight-2" height="24" tabindex="0">
                        <param name="source" value="../ClientBin/mpost.SilverlightMultiFileSelect.xap?<%=Guid.NewGuid().ToString() %>" />
                        <param name="initParams" value="<%=InitParams %>" />
                        <param name="background" value="white" />
                        <param name="onload" value="FileSelect_Loaded_<%=divFileSelect.ClientID %>" />
                        <param name="minRuntimeVersion" value="4.0.50401.0" />
                        <param name="autoUpgrade" value="true" />
                        <param name="windowless" value="<%=Windowless %>" />
                    </object>
                </div>
                <input type="hidden" id="maxSize" value="<%=MaxFileSizeWithUnit %>" />
                <input type="hidden" id="disAllowedTypes" value="<%=DisallowedFileTypes %>" />
                <input type="hidden" id="filekeyPrefix" value='<%=FileKeyPrefix %>' />
                <input type="hidden" id="languageSetting" value='<%=LanguageCode %>' />
                <input type="hidden" id="isMultipleSelect" value='<%=ACAConstant.COMMON_N %>' />
                <input type="hidden" id="selectFileLabel" value='<%= GetTextByKey("aca_attachment_label_browsefile").Replace("'", "\\'")%>' />
                <input type="hidden" id="mouseEnterLeft" value="/Image/btn_bg_l_m.png" />
                <input type="hidden" id="mouseEnterMiddle" value="/Image/bg_btn_center_over.png" />
                <input type="hidden" id="mouseEnterRight" value="/Image/btn_bg_r_m.png" />
                <input type="hidden" id="mouseOutLeft" value="/Image/btn_bg_l_t.png" />
                <input type="hidden" id="mouseOutMiddle" value="/Image/bg_btn_center_normal.png" />
                <input type="hidden" id="mouseOutRight" value="/Image/btn_bg_r_t.png" />
                <input type="hidden" id="conditionNumber" value="<%= ConditionNumber %>" />
                <div id="divHtml5Upload" runat="server" />
                <script type="text/javascript">
                    $(document).ready(function() {
                        if (<%=(CurrentFileUploadBehavior == FileUploadBehavior.Html5).ToString().ToLower() %>) {
                            upload_<%=ClientID %> = new FileUploader();
                            upload_<%=ClientID %>.register4UploadController('<%=divHtml5Upload.ClientID %>', '<%= GetTextByKey("aca_attachment_label_browsefile").Replace("'", "\\'")%>', false, "ACA_LgButton fontSize4Condition html5FileUpload");
                            <%=ClientID %>_InitialFileUploadControl(upload_<%=ClientID %>);
                        }
                    });
                </script>
            </td>
            <td class="removeInfo">
                <asp:UpdatePanel ID="removeButtonPanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <ACA:AccelaLinkButton ID="btnRemoveDocument" runat="server" CausesValidation="false"
                            Visible="False" OnClientClick="SetNotAskForSPEAR();$('#errorMessageLabel').html('');" OnClick="RemoveDocumentButton_Click"><img src='<%=ImageUtil.GetImageURL("remove.png") %>' class="Condition_Document_Remove_Icon" alt="<%=GetTextByKey("aca_fileselected_label_remove") %>"/></ACA:AccelaLinkButton>
                            <asp:HiddenField runat="server" ID="hdFileId" />
                            <asp:HiddenField runat="server" ID="hdFileName" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</div>
