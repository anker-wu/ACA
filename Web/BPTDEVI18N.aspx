<!--*** Language declaration goes ***-->
<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" %>
<!--*** Language declaration end ***-->

<!--I18n Support namespace import goes-->
<%@ Import namespace="Accela.ACA.Web" %>
<%@ Import namespace="System.Threading"%> 
<%@ Import namespace="System.Globalization"%>
<%@ Import namespace="Accela.ACA.Common.Util" %>
<script language="C#" runat="server">
protected override void InitializeCulture()
{
    Thread.CurrentThread.CurrentUICulture = new CultureInfo(I18nCultureUtil.UserPreferredCulture);
    base.InitializeCulture();
}
</script>
<!--I18n Support namespace import end-->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html >
<head id="Head1">
    <title>
        <%=LabelUtil.GetGlobalTextByKey("ACA_TopPage_Title")%>
    </title>
    <!--I18n Support goes-->
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <!--I18n Support end-->
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/jquery.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/GlobalConst.aspx")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/global.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/dialog.js")%>"></script>
    <script type="text/JavaScript" language="JavaScript">
        <!--
        function MM_preloadImages() { //v3.0
          var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
            var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
            if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
        }

        function MM_swapImgRestore() { //v3.0
          var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
        }

        function MM_findObj(n, d) { //v4.01
          var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
            d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
          if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
          for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
          if(!x && d.getElementById) x=d.getElementById(n); return x;
        }

        function MM_swapImage() { //v3.0
          var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
           if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
        }

        function MM_jumpMenu(targ,selObj,restore){ //v3.0
          eval(targ+".location='"+selObj.options[selObj.selectedIndex].value+"'");
          if (restore) selObj.selectedIndex=0;
        }

        //Make links 'tab-able' in Opera
        $(document).ready(function () {
            if ($.browser.opera) {
                SetTabIndexForOpera();
            }
        });
        //-->
    </script>

    <%= Accela.ACA.Common.Util.AccessibilityUtil.AccessibilityEnabled ? "<link rel='stylesheet' type='text/css' href='App_Themes/Accessibility/AccessibilityDefault.css' />" : ""%>
    <link type="text/css" rel="stylesheet" href="<%=FileUtil.CustomizeFolderRootWithoutLang %>stylesheet.css" />
    <link type="text/css" rel="Stylesheet" href="Customize/font.css" />
    <%= FileUtil.IsCustomizeStyleExisting("stylesheet.css") ? "<link rel='stylesheet' type='text/css' href='" + FileUtil.CustomizeFolderRoot + "stylesheet.css' />" : ""%>
    <%= Accela.ACA.Common.Util.AccessibilityUtil.AccessibilityEnabled ? "<link rel='stylesheet' type='text/css' href='App_Themes/Accessibility/Accessibility.css' />" : ""%>

</head>
     

<body style="margin: 0 0 0 0; background-image: url(<%=FileUtil.CustomizeFolderRootWithoutLang %>images/page_pinstripe_bg.gif);"
    onload="MM_preloadImages('<%=FileUtil.CustomizeFolderRoot %>images/nav_business-over.gif','<%=FileUtil.CustomizeFolderRoot %>images/nav_residents-over.gif','<%=FileUtil.CustomizeFolderRoot %>images/nav_visitors-over.gif','<%=FileUtil.CustomizeFolderRoot %>images/button_search-over.gif','<%=FileUtil.CustomizeFolderRoot %>images/nav_home-over.gif','<%=FileUtil.CustomizeFolderRoot %>images/nav_government-over.gif')">
    <form runat="server" id="theForm">
    <!--Section 508 Support goes-->
    <script type="text/JavaScript" language="JavaScript">
    <!--
        function skipTo(iframeID) {
            var expectedArgLength = skipTo.length;
            var oIframe = null;
            var oDoc = document;
            
            if (iframeID) {
                oIframe = window.frames[iframeID] ? window.frames[iframeID] : document.getElementById(iframeID);
                oDoc = oIframe ? oIframe.document || oIframe.contentDocument : null;
            }
            else {
                oIframe = window;
                oDoc = window.document;
            }

            var oAnchor = null;
            if (oDoc && arguments.length > expectedArgLength) {
                for (i = expectedArgLength; i < arguments.length; i++) {
                    oAnchor = oDoc.getElementById(arguments[i]);
                    if (oAnchor != null) {
                        break;
                    }
                }
            }

            var originalNeedAsk = oIframe && oIframe.NeedAsk ? oIframe.NeedAsk : null;
            if (originalNeedAsk != null) {
                oIframe.SetNotAsk(false);
            }
            if (oAnchor) {
                oAnchor.scrollIntoView();
                oAnchor.focus();
            }
            if (originalNeedAsk != null) {
                oIframe.SetNotAsk(originalNeedAsk);
            }
        }

        function skipToBeginningOfACA() {
            skipTo("ACAFrame", "ctl00_hlSkipToolBar", "SecondAnchorInACAMainContent");
        }

        function skipToMainContent() {
            skipTo("ACAFrame", "SecondAnchorInACAMainContent", "FirstAnchorInACAMainContent");
        }
    //-->
    </script>
    <ACA:AccelaHideLink ID="hlSkipNavigation" runat="server" AltKey="img_alt_externalmenu_skiplink" OnClientClick="skipToBeginningOfACA();"/>
    <ACA:AccelaHideLink ID="hlSkipToolBar" runat="server" AltKey="img_alt_modulemenu_skiplink" OnClientClick="skipToMainContent();" TabIndex="-1"/>
    <span style="display:none">
        <ACA:AccelaHideLink ID="hlFakeLink" runat="server" AltKey="img_alt_modulemenu_skiplink" IsSkippingToAnchor="true" NextControlClientID="targetFakeLink" TabIndex="-1"/>
        <a name="targetFakeLink" id="targetFakeLink" tabindex="-1"></a>
    </span>
    <script runat="server">
        /// <summary>
        /// Get the current url from AppSession or URL paramter
        /// </summary>
        public string CurrentURL
        {
            get
            {
                string currentURL = Accela.ACA.Web.Common.AppSession.CurrentURL;
                string tmpCurrentURL = Request.Params["CurrentURL"];

                // Only support internal url with the same domain
                if (!string.IsNullOrEmpty(tmpCurrentURL) && !Accela.ACA.Web.Common.FileUtil.IsExternalUrl(tmpCurrentURL))
                {
                    string bridgeViewPage = ConfigurationManager.AppSettings["DefaultPageFile"];

                    // The special url paramter CurrentURL is only supported on the default page or bridgeview page.             
                    if (Request.Url.AbsolutePath.IndexOf("Default.aspx", StringComparison.InvariantCultureIgnoreCase) >= 0
                        || (!string.IsNullOrEmpty(bridgeViewPage)
                            && Request.Url.AbsolutePath.IndexOf(bridgeViewPage, StringComparison.InvariantCultureIgnoreCase) >= 0))
                    {
                        currentURL = tmpCurrentURL;
                    }
                }

                return currentURL;
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hlSkipNavigation.AccessKey = Accela.ACA.Common.Util.AccessibilityUtil.GetAccessKey(Accela.ACA.Common.AccessKeyType.SkipNavigation);
                
                hlSkipToolBar.AccessKey = Accela.ACA.Common.Util.AccessibilityUtil.GetAccessKey(Accela.ACA.Common.AccessKeyType.SkipToolBar);
                //to resolve the issue that Safari can't detect accesskey raised from another iframe page.
                string userAgent = Request.UserAgent.ToLowerInvariant();
                bool isOpera = userAgent.Contains("opera");
                hlSkipToolBar.Visible = userAgent.Contains("safari") || isOpera;
                hlSkipToContent.Visible = isOpera;
            }
        }
    </script>
    <!--Section 508 Support end-->
	<table style="width:62.5em;" cellspacing="0" cellpadding="0" border="0" role="presentation">
        <tbody>
            <tr>
                <td style="vertical-align: top; background-color: #f1f5fc; width: 217px;">
                    <table width="200" cellspacing="0" cellpadding="0" style="border: 0;" role="presentation">
                        <tbody>
                            <tr style="vertical-align: top;">
                                <td rowspan="2" align="center" style="background-color: #f1f5fc; width: 217px;">
                                    <a href="<%=FileUtil.AppendApplicationRoot("index.htm")  %>"></a>
                                </td>
                                <td>
                                    <!--img width="103" height="40" style="border: 0;" src="<%=FileUtil.CustomizeFolderRoot %>images/header_title_city_of.jpg"
                                        alt="" /-->
                                </td>
                            </tr>
                            <tr style="vertical-align: top">
                                <td>
                                    <!--img width="103" height="54" style="border: 0;" id="header_bottom_left" alt="" src="<%=FileUtil.CustomizeFolderRoot %>images/header_bottom_left.jpg" /-->
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <td style="vertical-align: top; background-color: #f1f5fc; width: 217px;">
                </td>
            </tr>
        </tbody>
    </table>
<table role="presentation" style="width:62.5em;" cellspacing="0" cellpadding="0" border="0">
        <tbody>
            <tr>
                <td valign="top" style="background-color: #f1f5fc; border:0;">
                    <table style="width:12.8em;" cellspacing="0" cellpadding="0" border="0" role="presentation">
                        <tbody>
                            <tr>
                                <td align="center">
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    <table cellspacing="0" cellpadding="0" border="0" role="presentation">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <label id="lblDepartment" for="ddlDepartment"></label>
                                                    <select id="ddlDepartment" onchange="//MM_jumpMenu('parent',this,0)" name="select" style="width: 16em;font-size:12px;margin: 5;" title="<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_citydepartments_name"))%>,<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_dorpdown_help_message"))%>">
                                                        <option value="../government/departments.htm">
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_citydepartments_name"))%>
                                                        </option>
                                                        <option value="#">------------------------------</option>
                                                        <!-- Department of Planning & Economy (DPE)//-->
                                                        <option value="../government/dpe.htm">
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_citydepartments_ae_dpe"))%>
                                                        </option>
                                                        <!--Department of Finance (DOF) //-->
                                                        <option value="../government/dof.htm">
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_citydepartments_ae_dof"))%>
                                                        </option>
                                                        <!-- Family Development Foundation (FDF)//-->
                                                        <option value="../government/fdf.htm">
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_citydepartments_ae_fdf"))%>
                                                        </option>
                                                        <!--Health Authority - Abu Dhabi //-->
                                                        <option value="../government/ha.htm">
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_citydepartments_ae_health_authority"))%>
                                                        </option>
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 6px">
                                                    <img height="6" alt="" src="<%=FileUtil.CustomizeFolderRootWithoutLang %>images/spacer.gif" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblService" for="ddlService"></label>
                                                    <select id="ddlService" onchange="//MM_jumpMenu('parent',this,0)" name="select2" style="width: 16em;font-size:12px;margin: 5;" title="<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_onlineservices_name"))%>,<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_dorpdown_help_message"))%>">
                                                        <option value="../virtualcityhall/onlineservices.htm">
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_onlineservices_name"))%>
                                                        </option>
                                                        <option value="#">------------------------------</option>
                                                        <option>
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_onlineservices_trade_name_search"))%>
                                                        </option>
                                                        <!-- TRADE NAME SEARCH //-->
                                                        <option>
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_onlineservices_trade_name_reservation"))%>
                                                        </option>
                                                        <!-- TRADE NAME RESERVATION  //-->
                                                        <option>
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_onlineservices_trade_name_renewal"))%>
                                                        </option>
                                                        <!-- TRADE NAME RENEWAL //-->
                                                        <option>
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_onlineservices_license_procedure"))%>
                                                        </option>
                                                        <!-- LICENSE PROCEDURE //-->
                                                        <option>
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_onlineservices_trade_license_registration"))%>
                                                        </option>
                                                        <!-- TRADE LICENSE REGISTRATION //-->
                                                        <option>
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_onlineservices_trade_license_renewal"))%>
                                                        </option>
                                                        <!-- TRADE LICENSE RENEWAL //-->
                                                        <option>
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_onlineservices_commercial_directory"))%>
                                                        </option>
                                                        <!-- COMMERCIAL DIRECTORY//-->
                                                        <option>
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_onlineservices_initial_approval"))%>
                                                        </option>
                                                        <!--INITIAL APPROVAL //-->
                                                        <option>
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_onlineservices_contractor_registration"))%>
                                                        </option>
                                                        <!-- CONTRACTOR REGISTRATION //-->
                                                        <option>
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_onlineservices_consultant_registration"))%>
                                                        </option>
                                                        <!-- CONSULTANT REGISTRATION //-->
                                                        <option>
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_onlineservices_contractors_search"))%>
                                                        </option>
                                                        <!-- CONTRACTORS SEARCH //-->
                                                        <option>
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_onlineservices_consultant_search"))%>
                                                        </option>
                                                        <!-- CONSULTANT SEARCH //-->
                                                    </select>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td style="background-image: url(<%=FileUtil.CustomizeFolderRootWithoutLang %>images/sidenav_primary.gif);">
                                    <table align="center" cellspacing="0" cellpadding="0" border="0" role="presentation">
                                        <tbody>
                                            <tr>
                                                <td>                                                    
                                                    <a class="verdana10boldnavy verdana10boldnavy_fontsize" href="#">
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_submenu_government"))%>
                                                    </a>                                                    
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-image: url(<%=FileUtil.CustomizeFolderRootWithoutLang %>images/sidenav_primary.gif);">
                                    <table align="center" cellspacing="0" cellpadding="0" border="0" role="presentation">
                                        <tbody>
                                            <tr>
                                                <td>                                                    
                                                        <a class="verdana10boldnavy verdana10boldnavy_fontsize" href="#">
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_submenu_business"))%>
                                                        </a>
                                                    
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-image: url(<%=FileUtil.CustomizeFolderRootWithoutLang %>images/sidenav_primary.gif);">
                                    <table align="center" cellspacing="0" cellpadding="0" border="0" role="presentation">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    
                                                        <a class="verdana10boldnavy verdana10boldnavy_fontsize" href="#">
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_submenu_residents"))%>
                                                        </a>
                                                    
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-image: url(<%=FileUtil.CustomizeFolderRootWithoutLang %>images/sidenav_primary.gif);">
                                    <table align="center" cellspacing="0" cellpadding="0" border="0" role="presentation">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    
                                                        <a class="verdana10boldnavy verdana10boldnavy_fontsize" href="#">
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_submenu_visitors"))%>
                                                        </a>
                                                    
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <br />
                                        <!--I18n Support goes-->
                                        <%=I18nUtil.GetLanguageSwitchPanel()%>
                                        <!--I18n Support end-->
                                        <br />
                                        <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <td style="margin-top: 0; background-color: #ffffff; vertical-align: top;">
                    <table cellspacing="0" cellpadding="0" border="0" role="presentation">
                        <tbody>
                            <tr>
                                <td style="vertical-align: top;">
                                    <table cellspacing="0" cellpadding="0" border="0" role="presentation">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <table cellspacing="0" cellpadding="0" border="0" role="presentation">
                                                        <tbody>
                                                            <tr>
                                                                <td align="center">
                                                                    <table cellspacing="0" cellpadding="0" style="border: 0;" role="presentation">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td>
                                                                                    <table width="100%" role="presentation">
                                                                                        <tr>
                                                                                            <td width="70%" align="left">
                                                                                                <img style="border-top-width: 0px; border-left-width: 0px; border-bottom-width: 0px;
                                                                                                    border-right-width: 0px" id="header_logo" alt="<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_header_logo_alt")) %>"
                                                                                                    src="<%=FileUtil.CustomizeFolderRoot %>images/header_logo.jpg" />
                                                                                            </td>
                                                                                            <td width="30%" align="right">
                                                                                                <img style="border: 0;" src="<%=FileUtil.CustomizeFolderRoot %>images/TowardsAnIdealEconomy.jpg"
                                                                                                    alt="" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="background-image: url(<%=FileUtil.CustomizeFolderRootWithoutLang %>images/nav_bar_bg.gif);">
                                                                                    <table cellspacing="0" cellpadding="0" style="border: 0; background-image: url(<%=FileUtil.CustomizeFolderRootWithoutLang %>images/nav_bar_bg.gif);" role="presentation">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td colspan="16">
                                                                                                    <img width="600" height="5" alt="" src="<%=FileUtil.CustomizeFolderRootWithoutLang %>images/spacer.gif" /></td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="MenuItemTdStyle">
                                                                                                    &nbsp;&nbsp;
                                                                                                </td>
                                                                                                <td class="MenuItemTdStyle">
                                                                                                    <a href="<%=FileUtil.AppendApplicationRoot("default.aspx?resetUrl=yes")%>" class="MenuItemLinkStyle">
                                                                                                        <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_menu_home_alt"))%>
                                                                                                    </a>
                                                                                                </td>
                                                                                                <td class="MenuItemTdStyle">
                                                                                                    &nbsp;
                                                                                                    <img src="<%=FileUtil.CustomizeFolderRootWithoutLang %>images/MenuItemSeparator.JPG" alt="" />
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                                <td class="MenuItemTdStyle">
                                                                                                    <a href="#" class="MenuItemLinkStyle">
                                                                                                        <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_menu_government_alt"))%>
                                                                                                    </a>
                                                                                                </td>
                                                                                                <td class="MenuItemTdStyle">
                                                                                                    &nbsp;
                                                                                                    <img src="<%=FileUtil.CustomizeFolderRoot %>images/MenuItemSeparator.JPG" alt="" />
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                                <td class="MenuItemTdStyle">
                                                                                                    <a href="#" class="MenuItemLinkStyle">
                                                                                                        <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_menu_business_alt"))%>
                                                                                                    </a>
                                                                                                </td>
                                                                                                <td class="MenuItemTdStyle">
                                                                                                    &nbsp;
                                                                                                    <img src="<%=FileUtil.CustomizeFolderRoot %>images/MenuItemSeparator.JPG" alt="" />
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                                <td class="MenuItemTdStyle">
                                                                                                    <a href="#" class="MenuItemLinkStyle">
                                                                                                        <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_menu_residents_alt"))%>
                                                                                                    </a>
                                                                                                </td>
                                                                                                <td class="MenuItemTdStyle">
                                                                                                    &nbsp;
                                                                                                    <img src="<%=FileUtil.CustomizeFolderRoot %>images/MenuItemSeparator.JPG" alt="" />
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                                <td class="MenuItemTdStyle">
                                                                                                    <a href="#" class="MenuItemLinkStyle">
                                                                                                        <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_menu_visitors_alt"))%>
                                                                                                    </a>
                                                                                                </td>
                                                                                                <td style="white-space: nowrap">
                                                                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                    <img width="10" height="16" alt="" src="<%=FileUtil.CustomizeFolderRootWithoutLang %>images/spacer.gif" /></td>
                                                                                                <td>
                                                                                                    <input type="text" id="search24" class="inputshort" name="search22" title="<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_text_search"))%>"/></td>
                                                                                                <td>
                                                                                                    <img width="10" height="16" alt="" src="<%=FileUtil.CustomizeFolderRootWithoutLang %>images/spacer.gif" /></td>
                                                                                                <td>
                                                                                                    <a onmouseover="MM_swapImage('button_search11','','<%=FileUtil.CustomizeFolderRoot %>images/button_search-over.gif',1)"
                                                                                                        onmouseout="MM_swapImgRestore()" href="#" title="<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_search_alt")) %>">
                                                                                                        <img width="51" height="16" style="border: 0;" id="button_search1" alt="<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_search_alt")) %>"
                                                                                                            src="<%=FileUtil.CustomizeFolderRoot %>images/button_search.gif" /></a>
                                                                                                    <ACA:AccelaHideLink ID="hlSkipToContent" runat="server" TabIndex="0"/></td>
                                                                                                <td>
                                                                                                    <img width="10" height="16" alt="" src="<%=FileUtil.CustomizeFolderRootWithoutLang %>images/spacer.gif" /></td>
                                                                                                <td style="white-space: nowrap; width: 25%;">
                                                                                                    &nbsp;</td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="16">
                                                                                                    <img style="width:50.2em;" height="6" alt="" src="<%=FileUtil.CustomizeFolderRootWithoutLang %>images/spacer.gif" /></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="background-color: #ffffff;">
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                    <br />
                                                                    <img style="width:100%;" height="24" id="header_online_services" alt="<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_header_online_services_alt")) %>"
                                                                        src="<%=FileUtil.CustomizeFolderRoot %>images/header_online_services.gif" /></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" class="ACA_AlignLeftOrRight">
                                                    <!-- **** ACA iframe goes here ****-->
                                                    <iframe id="ACAFrame" height="600" style="overflow-y:auto; width:50.2em;" scrolling="no" frameborder="0" marginwidth="15" title="<%=LabelUtil.GetGlobalTextByKey("iframe_bridgeview_title") %>"
                                                        src='<%=CurrentURL%>' onload="DoIframeOnload()"><%=String.Format(LabelUtil.GetGlobalTextByKey("iframe_nonsupport_message"), CurrentURL)%></iframe>
                                                    <!-- **** ACA iframe end ****-->
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img style="width:50.2em;" height="24" alt="" src="<%=FileUtil.CustomizeFolderRootWithoutLang %>images/spacer.gif" /></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
<table style="width:62.5em;" cellspacing="0" cellpadding="0" border="0" role="presentation">
        <tbody>
            <tr>
                <td>
                    <img style="width:62.5em;" height="10" alt="" src="<%=FileUtil.CustomizeFolderRootWithoutLang %>images/spacer.gif" /></td>
            </tr>
            <tr>
                <td>
                    <p style="text-align: center;font-size:0.75em;">
                        <%=LabelUtil.GetGlobalTextByKey("ACA_TopPage_CopyRight")%>
                    </p>
                </td>
            </tr>
            <tr>
                <td>
                    <img style="width:62.5em;" height="10" alt="" src="<%=FileUtil.CustomizeFolderRootWithoutLang %>images/spacer.gif" /></td>
            </tr>
        </tbody>
    </table>
    <input type="submit" name="Submit" value="Submit" style="display: none; z-index: -1;"/>
    </form>
</body>
</html>

