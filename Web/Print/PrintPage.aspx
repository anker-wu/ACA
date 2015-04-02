<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" Inherits="Accela.ACA.Web.Print.PrintPage"
    CodeBehind="PrintPage.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" xml:lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>">
<head id="Head" runat="server">
    <title><%=LabelUtil.GetGlobalTextByKey("aca_printpage_label_title|tip")%></title>
</head>
<body>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/jquery.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/GlobalConst.aspx")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/global.js")%>"></script>
    <form id="form" runat="server">
    <div class="ACA_Content">
        <span id="spanContent"></span>
    </div>

    <script type="text/javascript">
        document.getElementById("spanContent").innerHTML = opener.window.document.forms[0].innerHTML;
        $("div.ACA_SmButton>a").addClass("ACA_Hide");
        $("a").attr("href", "javascript:void(0)").attr("onclick", "return false");
        $('.ACA_NaviTitle').parent().hide();
    </script>
    <input type="submit" name="Submit" value="Submit" class="HiddenButton" onclick="return false;" />
    </form>
    <noscript>
        <%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</body>
</html>
