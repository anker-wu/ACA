<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="Default.aspx.cs" Inherits="Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" xml:lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>">
<head id="Head1" runat="server">
    <title>
    <%=LabelUtil.GetGlobalTextByKey("ACA_TopPage_Title")%>
    </title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
</head>
<body>
<span id="SecondAnchorInACAMainContent"></span>
<ACA:AccelaLabel ID="lblmsg" runat="server" Visible="false" />
<noscript><%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</body>
</html>