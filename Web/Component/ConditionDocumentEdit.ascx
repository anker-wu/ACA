<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.ConditionDocumentEdit" CodeBehind="ConditionDocumentEdit.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Web.Util" %>
<%@ Register Src="~/Component/FileSelected.ascx" TagName="FileSelected" TagPrefix="ACA" %>

<script type="text/javascript">
    $(document).ready(function () {
        if (!$.global.isAdmin && !<%=(CurrentFileUploadBehavior == FileUploadBehavior.Html5).ToString().ToLower() %> && !CheckSilverlightInstalled()) {
            $('#divConditionDocument').hide();
            $('#divNoSilverLightInstalled').attr("class", "ACA_Show");
        }
    });
    
    var isRemoveErrorMsg = true;

    function CheckDocIsUploaded() {
        var isUploaded = true;
        var fileSelectControls = $('object[name="fileUploadObj"]');

        fileSelectControls.each(function () {
            var fileList = $(this)[0].Content.Files.FileList;

            for (var index = 0; index < fileList.length; index++) {
                if (fileList[index].StateString == 'Uploading') {
                    isUploaded = false;
                    break;
                }
            }
        });
        
        if (<%=(CurrentFileUploadBehavior == FileUploadBehavior.Html5).ToString().ToLower() %>) {
            var html5UploadControls = $('.btnBrowser_html5');
            html5UploadControls.each(function() {
                if ($(this).parent().hasClass("ACA_LgButtonDisable")) {
                    isUploaded = false;
                }
            });
        }

        if (!isUploaded) {
            var errorMsg = '<%= GetTextByKey("aca_attachment_msg_cannotcontinuewhenuploading")%>';
            showMessage('errorMessageLabel', errorMsg, "Error", true, 1, true);
        }

        return isUploaded;
    }

</script>
<div id="divUploadLink" class="ACA_TabRow" runat="server">
    <div id="divAttachmentField4Admin" visible="false" runat="server" class="ACA_TabRow ACA_Page">
        <span>Click to edit and format the section instruction using the available file upload variables:</span><br />
        <span><%=AttachmentUtil.FileUploadVariables.MaximumFileSize%>: The maximum file size allowed</span><br />
        <span><%=AttachmentUtil.FileUploadVariables.ForbiddenFileFormats%>: The forbidden format types</span><br /> 
    </div>
    <div class="ACA_Section_Instruction ACA_Section_Instruction_FontSize">
        <div>
            <ACA:AccelaLabel IsNeedEncode="false" ID="lblSizeIntroduction" LabelKey="aca_conditiondocument_label_sizelimitation"
                runat="server"></ACA:AccelaLabel>
        </div>
        <div>
            <ACA:AccelaLabel IsNeedEncode="false" ID="lblTypeIntroduction" LabelKey="aca_conditiondocument_label_typelimitation"
                runat="server"></ACA:AccelaLabel>
        </div>
    </div>
    <div id="divNoSilverLightInstalled" class="ACA_Hide">
        <a class="NotShowLoading" onclick="javascript:CheckAndSetNoAsk();window.open('<%=Request.Url.Scheme %>://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0');" href="javascript:void(0)" style="text-decoration: none">
            <ACA:AccelaLabel CssClass="ACA_Show ACA_Label_FontSize" ID="lblInstallMsg" runat="server" LabelKey="aca_fileupload_msg_silverlight_install"></ACA:AccelaLabel>
            <img src='<%= ImageUtil.GetImageURL("installSilverlight.png") %>' alt="Get Microsoft Silverlight" style="border-style: none;" />
        </a>
    </div>
    <div class="attachment_edit" id="divConditionDocument">
        <asp:UpdatePanel ID="attachmentEditPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <span id="errorMessageLabel"></span>
                <ACA:AccelaTextBox ID="txtValidateContinueAction" IsHidden="True" Validate="required" runat="server" CheckControlValueValidateFunction="CheckDocIsUploaded" ValidationByHiddenTextBox="True" />
                <div id="divInstruction" runat="server" class="Condition_Document_Title">
                    <div class="ACA_Required_Indicator">*</div>
                    <ACA:AccelaLabel ID="lnkDocumentNameHeader" runat="server" CssClass="fontbold" CommandName="Header" LabelKey="aca_requireddocument_label_documentname" CausesValidation="false"></ACA:AccelaLabel>
                </div>
                <ACA:AccelaGridView role="presentation" ID="gdvRequiredDocument" runat="server" AllowPaging="False" AllowSorting="False" IsInSPEARForm="True"
                SummaryKey="gdv_attachment_attachmentlist_summary" OnRowDataBound="RequiredDocument_RowDataBound" 
                AutoGenerateColumns="False" ShowHeader="False" AlternatingRowStyle-CssClass="Condition_Document_Body" RowStyle-CssClass="Condition_Document_Body" OnRowCommand="RequiredDocument_RowCommand">
                    <Columns>
                        <ACA:AccelaTemplateField>
                            <itemtemplate>
                                <div>
                                    <ACA:AccelaLabel ID="labDocumentIndex" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "DocumentIndex")%>'></ACA:AccelaLabel>
                                    <span id="spanConditionGrop">
                                        <ACA:AccelaLabel ID="lblConditionGroup" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ConditionGroup")%>'></ACA:AccelaLabel>
                                    </span>
                                    <span id="spanSplit"> - </span>
                                    <span id="spanConditionName">
                                        <ACA:AccelaLabel ID="lblDispConditionDescription" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "DispConditionDescription")%>'></ACA:AccelaLabel>
                                    </span>
                                </div>
                            </itemtemplate>
                            <ItemStyle Width="200px" CssClass="Condition_Document_ListItem"/>
                         </ACA:AccelaTemplateField>
                        <ACA:AccelaTemplateField>
                            <itemtemplate>
                                <ACA:FileSelected ID="fileSelected" runat="server" />
                            </itemtemplate>
                            <ItemStyle Width="350px"/>
                        </ACA:AccelaTemplateField>
                    </Columns>
                </ACA:AccelaGridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
