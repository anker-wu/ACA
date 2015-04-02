<%@ Page Language="C#" AutoEventWireup="true" Inherits="Brettle.Web.NeatUpload.FileUploadPage" %>
<%@ Register TagPrefix="Upload" Namespace="Brettle.Web.NeatUpload" Assembly="Brettle.Web.NeatUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
<!--
		.ProgressBar {
			margin: 0px;
			border: 0px;
			padding: 0px;
			width: 738px;
			height: 2em;
		}
-->
		</style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <Upload:InputFile ID="inputFile" runat="server" Width="665px" />
            <asp:Button ID="submitButton" runat="server" Text="Upload" OnClick="submitButton_Click" />
            <br />
            <Upload:ProgressBar ID="progressBar" runat="server" Triggers="submitButton" Inline="true">
                <asp:Label ID="label" runat="server" Text="Check Progress" />
            </Upload:ProgressBar>
        </div>
    </form>
</body>
</html>
