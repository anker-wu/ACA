<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CapReviewCertification.aspx.cs" Inherits="Accela.ACA.Web.Admin.CapReviewCertification" %>

<%@ Register src="~/Component/CapReviewCertification.ascx" tagname="CapReviewCertification" tagprefix="ACA" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:fb="https://www.facebook.com/2008/fbml">
<head id="Head1" runat="server">
    <title>Re-Certification Page</title>
</head>
<body>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/jquery.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/GlobalConst.aspx")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/global.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/jquery.corner.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/ProcessLoading.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/GlobalSearch.js")%>"></script>
    <script type="text/javascript">
        $.global.isRTL = IsTrue('<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft %>');
        $.global.isAdmin = IsTrue('<%= AppSession.IsAdmin %>');
        
        //fix the issue that the confirm/alert box should also follows the current culture.
        if ($.global.isRTL) {
            document.dir = "rtl";
        } else {
            document.dir = "";
        }
    </script>
    
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnableScriptGlobalization="true" EnableScriptLocalization="true"
        runat="server" EnablePageMethods="true" ScriptMode="Release" />
    <div class="ACA_RightItem" id="divContainer">
        <div>
            <h1>
                <ACA:AccelaLabel ID="lblReCertification" LabelType="PageTitle" LabelKey="acaadmin_recertification_label_title"
                    runat="server" TargetCtrlId="divContainer"></ACA:AccelaLabel>
            </h1>
        </div>
        <ACA:CapReviewCertification ID="capReviewCertification1" runat="server" />
    </div>
    </form>
</body>
</html>
