<%@ Page Language="C#" AutoEventWireup="true"  Theme="default" Inherits="Accela.ACA.Web.ProductLicenseError" Codebehind="ProductLicenseError.aspx.cs" %>
<%@ Import Namespace="Accela.ACA.Web"%>
<%@ Register Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" xml:lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>">
<head id="Head1" runat="server">
    <title><%=LabelUtil.GetGlobalTextByKey("aca_global_js_showerror_title")%></title>
</head>
<body style="margin-top:10px;">
    <span style="display:none">
        <ACA:AccelaHideLink ID="hlSkipToolBar" runat="server" AltKey="img_alt_modulemenu_skiplink" IsSkippingToAnchor="true" NextControlClientID="FirstAnchorInACAMainContent" TabIndex="-1"/>
        <a name="FirstAnchorInACAMainContent" id="FirstAnchorInACAMainContent" tabindex="-1"></a>
    </span>
    <span id="SecondAnchorInACAMainContent"></span>
    <form id="form1" runat="server">
        <div id="divDaily" runat="server" class="ACA_Content">
           <div class="ACA_Row">
               <uc1:MessageBar ID = "dailyMessage" runat="Server" />
           </div>
        </div>
        <div id="divAdmin" runat="server" visible="false" class="ACA_Content">
            <div class="ACA_Admin_Div" style="width:460px;">
                <div class="ACA_TabRow">
                    <img src="<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("admin_logo.png") %>" alt="" visible="false"/>
                </div>
                <div class="ACA_Row">
                     <uc1:MessageBar ID = "adminMessage" runat="Server" />
                </div>
            </div>
        </div>
        <input type="submit" name="Submit" value="Submit" class="HiddenButton" onclick="return false;" />
    </form>
    <noscript><%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</body>
</html>
