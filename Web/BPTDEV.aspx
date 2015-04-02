<!--*** Language declaration goes ***-->
<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" %>
<!--*** Language declaration end ***-->

<%@ Import namespace="Accela.ACA.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html >
<head id="Head1">
    <title>
        City of Bridgeview - Citizen Portal
    </title>

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
        //-->
    </script>
    <%= Accela.ACA.Common.Util.AccessibilityUtil.AccessibilityEnabled ? "<link rel='stylesheet' type='text/css' href='App_Themes/Accessibility/AccessibilityDefault.css' />" : ""%>
    <link type="text/css" rel="stylesheet" href="<%=FileUtil.CustomizeFolderRootWithoutLang %>stylesheet.css" />
    <link type="text/css" rel="Stylesheet" href="Customize/font.css" />
    <%= FileUtil.IsCustomizeStyleExisting("stylesheet.css") ? "<link rel='stylesheet' type='text/css' href='" + FileUtil.CustomizeFolderRoot + "stylesheet.css' />" : ""%>
    <%= Accela.ACA.Common.Util.AccessibilityUtil.AccessibilityEnabled ? "<link rel='stylesheet' type='text/css' href='App_Themes/Accessibility/Accessibility.css' />" : ""%>
</head>
<body style="margin: 0 0 0 0; background-image: url(<%=FileUtil.CustomizeFolderRootWithoutLang %>images/page_pinstripe_bg.gif);"
    onload="MM_preloadImages('<%=FileUtil.CustomizeFolderRootWithoutLang %>images/nav_business-over.gif','<%=FileUtil.CustomizeFolderRootWithoutLang %>images/nav_residents-over.gif','<%=FileUtil.CustomizeFolderRootWithoutLang %>images/nav_visitors-over.gif','<%=FileUtil.CustomizeFolderRootWithoutLang %>images/button_search-over.gif','<%=FileUtil.CustomizeFolderRootWithoutLang %>images/nav_home-over.gif','<%=FileUtil.CustomizeFolderRootWithoutLang %>images/nav_government-over.gif')">
    <form runat="server" id="theForm">
    <!--Section 508 Support goes-->
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/jquery.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/GlobalConst.aspx")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/global.js")%>"></script>
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/dialog.js")%>"></script>
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

        //Make links 'tab-able' in Opera
        $(document).ready(function () {
            if ($.browser.opera) {
                SetTabIndexForOpera();
            }
        });
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
            <td valign="top">
                <table role="presentation" cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr> 
                            <td rowspan="2"><a href="./default.aspx" title="City of Bridgeview [home]"><img style="width:6em; border:0px;" height="94" id="header_logo" alt="City of Bridgeview [home]" src="<%=FileUtil.ApplicationRoot %>Customize/images/header_logo.jpg" /></a></td>
                            <td><img style="width:6.8em; border:0;" height="40" src="<%=FileUtil.ApplicationRoot %>Customize/images/decorative_city_of.jpg" alt=""/></td>
                        </tr>
                        <tr> 
                            <td><img style="width:6.8em;" height="54" id="header_bottom_left" alt="" src="<%=FileUtil.ApplicationRoot %>Customize/images/decorative_bottom_left.jpg"/></td>
                        </tr>
                    </tbody>
                </table>
            </td>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" style="border:0;" role="presentation" >
                    <tbody>
                        <tr> 
                            <td><img style="width:50.2em;" height="40" src="<%=FileUtil.ApplicationRoot %>Customize/images/decorative_bridgeview.jpg" alt=""/></td>
                        </tr>
                        <tr> 
                            <td style="background-image:url(<%=FileUtil.CustomizeFolderRootWithoutLang %>images/nav_bar_bg.gif);">
                                <table role="presentation" cellspacing="0" cellpadding="0" style="width:37.5em; border:0;background-image:url(<%=FileUtil.CustomizeFolderRootWithoutLang %>images/nav_bar_bg.gif);">
                                    <tbody>
                                        <tr> 
                                            <td colspan="11"><img height="5" alt="" src="<%=FileUtil.ApplicationRoot %>Customize/images/spacer.gif"/></td>
                                        </tr>
                                        <tr> 
                                            <td><a onmouseover="MM_swapImage('nav_home11','','<%=FileUtil.ApplicationRoot %>Customize/images/nav_home-over.gif',1)" onmouseout="MM_swapImgRestore()" href="./default.aspx" title="Home"><img width="63" height="16" style="border:0;"  alt="Home" src="<%=FileUtil.ApplicationRoot %>Customize/images/nav_home.gif"/></a></td>
                                            <td><a onmouseover="MM_swapImage('nav_government11','','<%=FileUtil.ApplicationRoot %>Customize/images/nav_government-over.gif',1)" onmouseout="MM_swapImgRestore()" href="../government/government.htm" title="<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_submenu_government"))%>"><img width="103" height="16" style="border:0;"  alt="Government" src="<%=FileUtil.ApplicationRoot %>Customize/images/nav_government.gif"/></a></td>
                                            <td><a onmouseover="MM_swapImage('nav_business11','','<%=FileUtil.ApplicationRoot %>Customize/images/nav_business-over.gif',1)" onmouseout="MM_swapImgRestore()" href="../business/business.htm" title="<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_submenu_business"))%>"><img width="83" height="16" style="border:0;"  alt="Business" src="<%=FileUtil.ApplicationRoot %>Customize/images/nav_business.gif"/></a></td>
                                            <td><a onmouseover="MM_swapImage('nav_residents11','','<%=FileUtil.ApplicationRoot %>Customize/images/nav_residents-over.gif',1)" onmouseout="MM_swapImgRestore()" href="../residents/residents.htm" title="<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_submenu_residents"))%>"><img width="89" height="16" style="border:0;"  alt="Residents" src="<%=FileUtil.ApplicationRoot %>Customize/images/nav_residents.gif"/></a></td>
                                            <td><a onmouseover="MM_swapImage('nav_visitors11','','<%=FileUtil.ApplicationRoot %>Customize/images/nav_visitors-over.gif',1)" onmouseout="MM_swapImgRestore()" href="../visitors/visitors.htm" title="<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_submenu_visitors"))%>"><img width="77" height="16" style="border:0;"  alt="Visitors" src="<%=FileUtil.ApplicationRoot %>Customize/images/nav_visitors.gif"/></a></td>
                                            <td><img width="10" height="16" alt="" src="<%=FileUtil.ApplicationRoot %>Customize/images/spacer.gif"/></td>
                                            <td><input type="text" id="Text1" class="inputshort" name="search22" title="<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_text_search"))%>" /></td>
                                            <td><img width="10" height="16" alt="" src="<%=FileUtil.ApplicationRoot %>Customize/images/spacer.gif"/></td>
                                            <td><a onmouseover="MM_swapImage('button_search11','','<%=FileUtil.ApplicationRoot %>Customize/images/button_search-over.gif',1)" onmouseout="MM_swapImgRestore()" href="#" title="<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_text_search"))%>"><img width="51" height="16" style="border:0;"  id="Img1" alt="Search" src="<%=FileUtil.ApplicationRoot %>Customize/images/button_search.gif"/></a></td>
                                            <td><img width="10" height="16" alt="" src="<%=FileUtil.ApplicationRoot %>Customize/images/spacer.gif"/></td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr> 
                                            <td colspan="11"><img height="6" alt="" src="<%=FileUtil.ApplicationRoot %>Customize/images/spacer.gif"/></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr> 
                            <td style="background-color:#ffffff;"><img height="24" alt="" src="<%=FileUtil.ApplicationRoot %>Customize/images/spacer.gif"/></td>
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
                <td valign="top" style="background-color: #f1f5fc; border:0;">
                    <table style="width:12.8em;" cellspacing="0" cellpadding="0" border="0" role="presentation">
                        <tbody>
                            <tr>
                                <td align="center">
                                    <table cellspacing="0" cellpadding="0" border="0" role="presentation">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <label id="lblDepartment" for="ddlDepartment"></label>
                                                    <select id="ddlDepartment" style="width:16em;margin:5;"onchange="MM_jumpMenu('parent',this,0)" name="select" title="City Departments,<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_dorpdown_help_message"))%>">
                                                        <option  value="../government/departments.htm">City Departments</option>
                                                        <option value="#">-------------------------</option>
                                                        <option value="../government/departments.htm">View All Departments</option>
                                                        <option value="../government/animalcare.htm">Animal Care and Control</option>
                                                        <option value="../government/building.htm">Building</option>
                                                        <option value="../government/council.htm">City Council</option>
                                                        <option value="../government/emergency.htm">Emergency Services</option>
                                                        <option value="../government/fire.htm">Fire</option>
                                                        <option value="../government/health.htm">Health Services</option>
                                                        <option value="../government/library.htm">Library</option>
                                                        <option value="../government/mayor.htm">Mayor's Office</option>
                                                        <option value="../government/parks.htm">Parks and Recreation</option>
                                                        <option value="../government/planning.htm">Planning</option>
                                                        <option value="../government/police.htm">Police</option>
                                                        <option value="../government/publicworks.htm">Public Works</option>
                                                        <option value="../government/schooldistrict.htm">School District</option>
                                                        <option value="../government/transit.htm">Transportation Authority</option>
                                                        <option value="../government/treasurer.htm">Treasurer/Tax Collector</option>
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
                                                   <select id="ddlService" style="width:16em;margin:5;" onchange="MM_jumpMenu('parent',this,0)" name="select2" title="On-line Services,<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_dorpdown_help_message"))%>">
                                                        <option value="../virtualcityhall/onlineservices.htm">On-line Services</option>
                                                        <option value="#">-------------------------</option>
                                                        <option value="Default.aspx">Access the Citizen Portal</option>
                                                        <option value="../virtualcityhall/contractor.htm">Look-up a Contractor</option>
                                                        <option value="../virtualcityhall/renewlicense.htm">Renew Licenses</option>
                                                        <option value="../virtualcityhall/newpermit.htm">Apply for a Permit</option>
                                                        <option value="../virtualcityhall/servicerequest.htm">Submit a Request</option>
                                                 </select>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="background-image: url(<%=FileUtil.CustomizeFolderRootWithoutLang %>images/sidenav_primary.gif);">
                                    <table align="center" cellspacing="0" cellpadding="0" border="0" role="presentation">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <p>
                                                        <a class="verdana10boldnavy" href="#">
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_submenu_government"))%>
                                                        </a>
                                                    </p>
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
                                                    <p>
                                                        <a class="verdana10boldnavy" href="#">
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_submenu_business"))%>
                                                        </a>
                                                    </p>
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
                                                    <p>
                                                        <a class="verdana10boldnavy" href="#">
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_submenu_residents"))%>
                                                        </a>
                                                    </p>
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
                                                    <p>
                                                        <a class="verdana10boldnavy" href="#">
                                                            <%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_submenu_visitors"))%>
                                                        </a>
                                                    </p>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </tbody>
                    </table>
                    <ACA:AccelaHideLink ID="hlSkipToContent" runat="server" TabIndex="0"/>
                </td>
                <td style="margin-top: 0; background-color: #ffffff; vertical-align: top;">
                    <table cellspacing="0" cellpadding="0" border="0" role="presentation">
                        <tbody>
                            <tr>
                                <td style="vertical-align: top;">
                                    <table cellspacing="0" cellpadding="0" border="0" role="presentation">
                                        <tbody>
                                            <tr>
                                                <td align="center">
                                                 <img style="width:50.2em;" height="24" id="header_online_services" alt="<%=Server.HtmlEncode(LabelUtil.GetGlobalTextByKey("aca_cus_defaultmaster_header_online_services_alt")) %>"
                                                                        src="<%=FileUtil.CustomizeFolderRootWithoutLang %>images/header_online_services.gif" /></td>
                                            </tr>
                                            <tr>
                                                <td valign="top" class="ACA_AlignLeftOrRight">
                                                    <!-- **** ACA iframe goes here ****-->
                                                    <iframe id="ACAFrame" name="ACAFrame" height="600" style="overflow-y:auto; width:50.2em;" scrolling="no" frameborder="0" marginwidth="15" title="<%=LabelUtil.GetGlobalTextByKey("iframe_bridgeview_title") %>"
                                                        src='<%=CurrentURL%>'><%=String.Format(LabelUtil.GetGlobalTextByKey("iframe_nonsupport_message"), CurrentURL)%></iframe>
                                                    <!-- **** ACA iframe end ****-->
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img  style="width:50.2em;" height="24" alt="" src="<%=FileUtil.CustomizeFolderRootWithoutLang %>images/spacer.gif" /></td>
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
                    <img  style="width:62.5em;" height="10" alt="" src="<%=FileUtil.CustomizeFolderRootWithoutLang %>images/spacer.gif" /></td>
            </tr>
            <tr>
                <td>
                    <p style="text-align: center">
                        <%=DateTime.Now.Year%> City of Bridgeview All rights reserved.
                    </p>
                </td>
            </tr>
            <tr>
                <td>
                    <img  style="width:62.5em;" height="10" alt="" src="<%=FileUtil.CustomizeFolderRootWithoutLang %>images/spacer.gif" /></td>
            </tr>
        </tbody>
    </table>
    <input type="submit" name="Submit" value="Submit" style="display: none; z-index: -1;"/>
    </form>
</body>
</html>

