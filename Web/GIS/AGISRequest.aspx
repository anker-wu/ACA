<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AGISRequest.aspx.cs" Inherits="Accela.ACA.Web.GIS.AGISRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" xml:lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>">
<head runat="server">
    <title><%=LabelUtil.GetGlobalTextByKey("aca_gisrequest_label_title|tip")%></title>
    <link rel="stylesheet" type="text/css" title="default" href="../App_Themes/default/form.css" />
    <%= Accela.ACA.Common.Util.AccessibilityUtil.AccessibilityEnabled ? "<link rel='stylesheet' type='text/css' href='../App_Themes/Accessibility/AccessibilityDefault.css' />" : ""%>
    <link rel="stylesheet" type="text/css" title="default" href="../App_Themes/default/navigation.css" />
    <%= Accela.ACA.Common.Util.AccessibilityUtil.AccessibilityEnabled ? "<link rel='stylesheet' type='text/css' href='../App_Themes/Accessibility/Accessibility.css' />" : ""%>

    <script type="text/javascript">
        function SendRequest(xml) {
            var cancelSend = document.getElementById("CancelSend");
            if (cancelSend!=null && cancelSend.value == "true") {
                return;
            }

            var vs = document.getElementById("__VIEWSTATE");
            if (vs != null) {
                vs.parentNode.removeChild(vs);
            }
            var GovXMLRequest = document.getElementById("GovXMLRequest");
            GovXMLRequest.value = xml;
            var frm = document.getElementById("<%=form1.ClientID%>");
            frm.submit();
        }
    </script>
</head>
<body class="fb_gis_body">
    <form method="post"  id="form1" runat="server">
    <div>
        <input id="Origin" name="Origin" type="hidden" value="ACA" title="" />
        <input id="GovXMLRequest" name="GovXMLRequest" type="hidden" value="" />
    </div>
    <div class="ACA_GIS_Message_Error" id="dvError" runat="server" visible="false"></div>
    <input type="submit" name="Submit" value="Submit" class="HiddenButton" onclick="return false;" />
    </form>
    <script type="text/javascript">
        var de = document.getElementById("dvError");
        if (de != null) {
            var h = document.documentElement.clientHeight;
            de.style.top = h / 2 + "px";
            var w = document.documentElement.clientWidth;
            var l = (w - de.clientWidth) / 2;
            de.style.left = l + "px";
        }
    </script>
</body>
</html>
