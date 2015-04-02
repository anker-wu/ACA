<%@ Page Language="C#" AutoEventWireup="true" Inherits="Logout" Codebehind="Logout.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" xml:lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>">
<head runat="server">
    <title>User Logout</title>
</head>
<body>
    <span style="display:none">
        <ACA:AccelaHideLink ID="hlSkipToolBar" runat="server" AltKey="img_alt_modulemenu_skiplink" IsSkippingToAnchor="true" NextControlClientID="FirstAnchorInACAMainContent" TabIndex="-1"/>
        <a name="FirstAnchorInACAMainContent" id="FirstAnchorInACAMainContent" tabindex="-1"></a>
    </span>
    <span id="SecondAnchorInACAMainContent"></span>
    <form id="form1" runat="server">
        <asp:LinkButton runat="server" ID="loginLink" OnClientClick="gotoUrl()" OnClick="LoginLink_Click"></asp:LinkButton>
    </form>

    <script type="text/javascript">
                function window_onbeforeunload() { 
        location.replace(this.href); 

        event.returnValue=false; 
        }
        var lnk=document.getElementById("loginLink");
        if(document.all){
            window.location.href="Login.aspx";
        }else{
            var evt = document.createEvent("MouseEvents");  
            evt.initEvent("click",true,true);  
            lnk.dispatchEvent(evt);  
        }
        
        function gotoUrl(){
            __doPostBack("loginLink");
        }
    </script>
</body>
</html>
