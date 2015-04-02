<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Error4Popup.aspx.cs" Inherits="Accela.ACA.Web.Error4Popup" %>
<%@ Register Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=LabelUtil.GetGlobalTextByKey("ACA_TopPage_Title")%>
    </title>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />    
</head>
<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/jquery.js")%>"></script>
<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/GlobalConst.aspx")%>"></script>
<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/global.js")%>"></script>
<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/ProcessLoading.js")%>"></script>

<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnableScriptGlobalization="true" EnableScriptLocalization="true" runat="server" EnablePageMethods="true" ScriptMode="Release" />
    <div id="divErrorPage">
        <div>
            <uc1:MessageBar ID="systemErrorMessage" runat="Server" />
        </div>
    </div>
    <script type="text/javascript">
        var needInitControlLoading = true;
        with (Sys.WebForms.PageRequestManager.getInstance()) {
            if ('<%=AppSession.IsAdmin%>'.toLowerCase() == 'false') {
                add_pageLoaded(onPageLoaded);
                add_endRequest(onEndRequest);
            }
        }

        var processLoading = new ProcessLoading();
        function onPageLoaded(sender, args) {
            if (needInitControlLoading) {
                processLoading.initControlLoading();
            }

            addPrintErrors2SubmitButton();

            //Restore the click event of ACADialog close button for ASIT edit form.
            $('#' + parent.ACADialog.close_id, parent.document).click(function () {
                parent.ACADialog.close();
                return false;
            });
        }

        function onEndRequest(sender, arg) {
            processLoading.hideLoading();
        }
    </script>
    <input type="submit" name="Submit" value="Submit" class="HiddenButton" onclick="return false;" />
    </form>
</body>
</html>
