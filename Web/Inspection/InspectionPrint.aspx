<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" Inherits="Accela.ACA.Web.Inspection.InspectionPrint"
    CodeBehind="InspectionPrint.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" xml:lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>">
<head id="Head1" runat="server">
     <title><%=LabelUtil.GetGlobalTextByKey("aca_inspectionprint_label_title|tip")%></title>
</head>
<body>

    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/jquery.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/GlobalConst.aspx")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/global.js")%>"></script>
    <form id="form1" runat="server">
    <div class="ACA_Content">
        <span id="spanContent"></span>
    </div>

    <script type="text/javascript">
        document.getElementById("spanContent").innerHTML = opener.window.document.forms[0].innerHTML;
        $("div.ACA_SmButton>a").addClass("ACA_Hide");
        $("a[id*='lnkShowStatusHistory']").addClass("ACA_Hide");
        $("a[id*='lnkShowResultComments']").addClass("ACA_Hide");
        $("a[id*='lblResultComments']").addClass("ACA_Hide");
        $("a[id*='lblResultComment']").addClass("ACA_Hide");
        $("a").attr("href", "javascript:void(0)").attr("onclick", "return false");
    </script>
    <input type="submit" name="Submit" value="Submit" class="HiddenButton" onclick="return false;" />
    </form>
    <noscript>
        <%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</body>
</html>
