<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentStatusList.ascx.cs" Inherits="Accela.ACA.Web.Component.DocumentStatusList" %>
<div id="divDocumentStatusList">
</div>
<script type="text/javascript">
    function UploadAuditedDocumentList() {
        if (!$.global.isAdmin) {
            PageMethods.LoadDocStatuses('<%=ClientID %>', '<%=ModuleName %>', function (result) {
                var docStatusList = $("#divDocumentStatusList");
                
                if (result != undefined) {
                    docStatusList.html(result);
                } else {
                    docStatusList.html("");
                }
            });
        }
    }

    function ExpandDocumentStatusComment(rowIndex) {
        var rowIndexId = rowIndex.toString() + "<%= ClientID %>";
        var commentDivId = "divDocumentStatusComment" + rowIndexId;
        var linkID = "lnkDocumentStatusExpandComment" + rowIndexId;
        var imgID = "lnkDocumentStatusToggledivComment" + rowIndexId;
        expandComment(commentDivId, imgID, linkID);
    }
</script>