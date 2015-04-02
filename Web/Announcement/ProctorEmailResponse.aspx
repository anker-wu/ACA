<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProctorEmailResponse.aspx.cs" Inherits="Accela.ACA.Web.Announcement.ProctorEmailResponse" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" xml:lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>">
<head runat="server">
    <title><%=LabelUtil.GetGlobalTextByKey("aca_emailresponse_label_title|tip")%></title>
</head>
<body>
    <asp:Literal ID="litMessage" runat="server"></asp:Literal>  
</body>
</html>
