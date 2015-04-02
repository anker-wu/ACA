<%@ Page Language="C#" AutoEventWireup="true" Inherits="Admin_AdminTools" Codebehind="AdminTools.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACA Admin Tools</title>
</head>
<body>
    <form id="form1" runat="server">
        <span style="display:none">
            <ACA:AccelaHideLink ID="hlSkipToolBar" runat="server" AltKey="img_alt_modulemenu_skiplink" IsSkippingToAnchor="true" NextControlClientID="FirstAnchorInACAMainContent" TabIndex="-1"/>
            <a name="FirstAnchorInACAMainContent" id="FirstAnchorInACAMainContent" tabindex="-1"></a>
        </span>
    <div><br/>
    <asp:LinkButton ID="lbDailyHome" runat="server" OnClick="DailyHomeButton_Click">Return to ACA Daily</asp:LinkButton>
        <br/>
        <asp:LinkButton ID="lbAdminHome"
            runat="server" OnClick="AdminHomeButton_Click">Return to ACA Admin Home</asp:LinkButton>
        <br/><br/>
        <input id="btnClearCache" runat="server" type="button" value="Clear Cache" onserverclick="ClearCacheButton_ServerClick" style="width: 180px" />
        <br/><br/>
        <input id="btnExportCache" runat="server" type="button" value="Get Cache Content" style="width: 180px" onserverclick="ExportCacheButton_ServerClick" />
        <br/><br/>
        <input id="btnClearTemp" runat="server" type="button" value="Clear Temporary Documents" style="width: 180px" onserverclick="ClearTempButton_ServerClick" />
        <br/><br/>
        <input id="btnExportTemp" runat="server" type="button" value="Show Temporary Documents" style="width: 180px" onserverclick="ExportTempButton_ServerClick" />
        <br />
        <br/>
        <input id="btnWebConfig" runat="server" type="button" value="Show Web Configureation" style="width: 180px" onserverclick="WebConfigButton_ServerClick"/><br />
        <br />
        <input id="btnIISVersion" runat="server" type="button" value="Show IIS Server Version" style="width: 180px" onserverclick="IISVersionButton_ServerClick"/><br />
        <br />

        
        </div>
    </form>
</body>
</html>
