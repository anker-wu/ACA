<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadScoreForm.ascx.cs"
    Inherits="Accela.ACA.Web.Component.UploadScoreForm" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Web.Util" %>
<%@ Register TagPrefix="ACA" TagName="FileUpload" Src="~/Component/FileUpload.ascx" %>
<div id="divAttachmentField" visible="false" runat="server" class="ACA_TabRow ACA_Page">
    <span>Click to edit and format the section instruction using the available file upload variables:</span><br />
    <span><%=AttachmentUtil.FileUploadVariables.MaximumFileSize%>: The maximum file size allowed</span>
</div>
<div class="ACA_Page">
    <ACA:AccelaLabel IsNeedEncode="false" ID="lblSizeIntroduction" LabelKey="aca_fileupload_label_sizelimitation"
        runat="server"></ACA:AccelaLabel>
</div>
<asp:UpdatePanel ID="plUploadScoreEdit" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdatePanel ID="plUploadScores" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="ACA_TabRow_NoMargin" id="divLicense" runat="server">
                    <ACA:AccelaDropDownList ID="ddlLicenseID" LabelKey="examination_upload_document_providers"
                        Required="true" runat="server">
                    </ACA:AccelaDropDownList>
                </div>
                <div class="ACA_TabRow_NoMargin">
                    <ACA:AccelaNameValueLabel ID="lblFileName" Font-Bold="true" LabelKey="per_attachment_Label_file"
                        runat="server"></ACA:AccelaNameValueLabel>
                </div>
                <asp:LinkButton ID="btnFillDocEditForm" runat="Server" CssClass="ACA_Hide" OnClick="FillDocEditForm"
                    TabIndex="-1"></asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="uploadscore_edit">
            <script type="text/javascript">
                //To keeping previous selected file.
                var prevFileInfos = [];
        
                function <%=ClientID %>_FillDocEditForm(fileInfos, silverlightObj) {
                    var fileInfoArray = [];

                    try {
                        fileInfoArray = eval(fileInfos);
                    }
                    catch (e) { }
                    finally {
                        if (fileInfoArray && fileInfoArray.length && fileInfoArray.length > 0) {
                            //Delete previous file.
                            DeletePrevFile();

                            for (var i = 0; i < fileInfoArray.length; i++) {
                                prevFileInfos.push(fileInfoArray[i]);
                            }

                            window.transferObj = silverlightObj;
                            __doPostBack('<%=btnFillDocEditForm.UniqueID %>', fileInfos);
                        }
                    }
                }

                function BrowseFile() {
                    var url = '<%=FileUtil.AppendApplicationRoot("FileUpload/FileUploadPage.aspx") %>?CallbackFunc=afterExaminationAttachmentUpload&attachmentCtrlID=<%=ClientID %>';
                    ACADialog.popup({ url: url, width: 485, height: 400, objectTarget: '<%=btnBrowse.ClientID %>' });

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
                        success: function () {
                        }
                    });
                }

                //Disable or Enable Finish button.
                function DisableUploadScoreSaveButton(disabled) {
                    var btnSave = document.getElementById('<%=btnSave.ClientID %>');

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

                function GetAdvanceButtonWidth() {
                    return $('#<%=btnBrowse.ClientID %>').width();
                }

                //Silverlight will call this function when onload this silverlight button
                function GetSilverlightButtonMinWidth() {
                    var btnBrowser = $('#<%=btnBrowse.ClientID %>');

                    if ($.exists(btnBrowser)) {
                        return (btnBrowser.width() - 12);
                    }
                    return (btnBrowser.width() - 12);
                }
                
                var upload_<%=ClientID %>;
                function <%=ClientID %>_InitialFileUploadControl(fileUploader) {
                    fileUploader.initFileUploadControl("messageSpan", '<%=DisallowedFileTypes %>', <%=MaxFileSizeByByte %>,
                        '<%= GetTextByKey("aca_fileupload_msg_typeerror").Replace("'", "\\'")%>', '<%= GetTextByKey("aca_fileupload_msg_sizeerror").Replace("'", "\\'")%>',
                        '<%= GetTextByKey("aca_fileupload_msg_zerosizeerror").Replace("'", "\\'")%>', '<%=ParentAgencyCode %>', '<%=ParentDocumentNo %>',
                        '<%= GetTextByKey("aca_html5upload_msg_notsupport").Replace("'", "\\'")%>', '<%= GetTextByKey("aca_html5upload_msg_uploaderror").Replace("'", "\\'")%>');
                    
                    //add event (optional)
                    fileUploader.selectCompletedCallBack = function(files) { 
                        var loading = new ProcessLoading();
                        loading.showLoading();
                        <%=ClientID %>_FillDocEditForm(files); 
                    };
                }
                
                function InitButtons(isShow) {
                    var btnSave = $('#<%=btnSaveContainer.ClientID %>');
                    var divFileBrowser = $('#<%=divFileBrowser.ClientID %>');
                    var btnBasicSilverlight = $('#<%=divFileSelect.ClientID %>');
                    var btnAdvancedSilverlight = $('#<%=btnBrowse.ClientID %>');
                    var btnRemoveAll = $('#<%=btnRemoveContainer.ClientID %>');

                    if (!$.exists(btnAdvancedSilverlight)) {
                        return;
                    }

                    if (isShow) {
                        btnSave.removeClass("ACA_Hide");
                        btnRemoveAll.removeClass("ACA_Hide");
                    }
                    else {
                        btnSave.addClass("ACA_Hide");
                        btnRemoveAll.addClass("ACA_Hide");
                    }
                    
                    if(<%=(AppSession.IsAdmin).ToString().ToLower()%>) {
                        btnBasicSilverlight.addClass("ACA_Hide");
                        divFileBrowser.addClass("ACA_Hide");
                        btnAdvancedSilverlight.removeClass("ACA_Hide");
                    }
                    else {
                        
                        if (<%=(CurrentFileUploadBehavior == FileUploadBehavior.Html5).ToString().ToLower() %>) {
                            btnBasicSilverlight.addClass("ACA_Hide");
                            btnAdvancedSilverlight.addClass("ACA_Hide");
                            divFileBrowser.removeClass("ACA_Hide");
                            
                            upload_<%=ClientID %> = new FileUploader();
                            upload_<%=ClientID %>.register4UploadController('<%=divFileBrowser.ClientID %>', '<%= GetTextByKey("aca_uploadscore_label_browsefile").Replace("'", "\\'")%>', false, "ACA_LgButton ACA_LgButton_FontSize html5FileUpload");
                            <%=ClientID %>_InitialFileUploadControl(upload_<%=ClientID %>);
                        }
                        else if (<%=(CurrentFileUploadBehavior == FileUploadBehavior.Advanced).ToString().ToLower() %>) {
                            btnBasicSilverlight.addClass("ACA_Hide");
                            btnAdvancedSilverlight.removeClass("ACA_Hide");
                            divFileBrowser.addClass("ACA_Hide");
                        } else {
                            btnBasicSilverlight.removeClass("ACA_Hide");
                            btnAdvancedSilverlight.addClass("ACA_Hide");
                            divFileBrowser.addClass("ACA_Hide");
                        }
                    }
                }
        
                function RemoveAllFiles() {
                    SetNotAskForSPEAR();
                    $('#messageSpan').html('');
                    <%=fileSelect.FunctionRemoveAllFile %>();
                }
                
                if (typeof (myValidationErrorPanel) != "undefined") {
                    myValidationErrorPanel.registerIDs4Recheck("<%=btnRemoveAll.ClientID %>");
                }
            </script>
            <div id="divUploadButton" class="ACA_Row ACA_LiLeft action_buttons">
                <ul>
                    <li id="btnSaveContainer" runat="server" class="ACA_Hide">
                        <ACA:AccelaButton ID="btnSave" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                            CausesValidation="true" runat="server" LabelKey="aca_uploadscore_label_savedocinfo"
                            OnClick="Save" />
                    </li>
                    <li>
                        <ACA:AccelaButton ID="btnBrowse" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                            CausesValidation="false" runat="server" LabelKey="aca_uploadscore_label_browsefile"
                            OnClientClick="return BrowseFile();" />
                        <div id="divFileSelect" runat="server">
                            <ACA:FileUpload ID="fileSelect" runat="server" IsMultipleUpload="N" 
                                ErrorMsgContainerId="messageSpan" 
                                AdvanceButtonWidth="GetAdvanceButtonWidth()"
                                FunctionRunInStartUpload="DisableUploadScoreSaveButton" 
                                FunctionRunInAllFilesFinished="DisableUploadScoreSaveButton"
                                SilverlightButtonLabelKey="aca_uploadscore_label_browsefile"/>
                        </div>
                        <div id="divFileBrowser" runat="server" />
                    </li>
                    <li id="btnRemoveContainer" runat="server" class="ACA_Hide">
                        <ACA:AccelaButton ID="btnRemoveAll" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                            CausesValidation="false" runat="server" LabelKey="aca_uploadscore_label_remove"
                            OnClick="RemoveAll" OnClientClick="RemoveAllFiles()" />
                    </li>
                </ul>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
