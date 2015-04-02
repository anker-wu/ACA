<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HostedAgencySettings.aspx.cs" Inherits="Accela.ACA.Web.Admin.HostedAgencySettings" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import namespace ="Accela.ACA.Web.WebService" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hosted Agency Settings</title>
    <link rel="stylesheet" type="text/css" href="../resources/css/ext-all.css" />
    <link rel="stylesheet" type="text/css" href="../styles/main.css" />
    <script type="text/javascript" src="../../Scripts/jquery.js"></script>
    <script type="text/javascript" src="../adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../scripts/ext-all-debug.js"></script>
    <script type="text/javascript" src="../scripts/ext-extension.js"></script>
    <script type="text/javascript" src="../scripts/ServiceGroup.js"></script> 
    <script type="text/javascript" src="../TreeJSON/LabelKey.aspx"></script>    
    <script type="text/javascript" src="../scripts/ExtConst.js"></script>
    <script type="text/javascript" src="../../scripts/GlobalConst.aspx"></script>
    <script type="text/javascript" src="../../Scripts/global.js"></script>
    <style type="text/css">
        .x-tree-nodes-setting
        {
            display: none;
        }
    </style>

    <script type="text/javascript">
        Ext.BLANK_IMAGE_URL = "../resources/images/default/s.gif";
    </script>
</head>
<body>
 <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        <Services>
            <asp:ServiceReference Path="../../WebService/AdminConfigureService.asmx" />
        </Services>
    </asp:ScriptManager>
    
    </form>
    <div id="divGroup" class="ACA_PaddingStyle">
        <!--service group section -->
        <fieldset class="module_settings_section">
            <legend>
                <ACA:AccelaLabel ID="AccelaLabel9" runat="server"></ACA:AccelaLabel>
                <span id="divServiceGroupTitle"></span></legend>
            <div id="ServiceGroup">
                <div id="divGroupHeader" style="margin-bottom: -4px">
                </div>
                <div id="divGroupBody">
                    <div id="groupServiceAssociation">
                        <div id="newServiceGroup">
                            <table border="0" width="98%" role="presentation">
                                <tr>
                                    <td colspan="3">
                                        <div id="NewServiceGroupName">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="availableGroupCap">
                                        </div>
                                    </td>
                                    <td valign="middle" align="center">
                                        <div id="addGroupCap">
                                        </div>
                                        <br />
                                        <br />
                                        <div id="removeGroupCap">
                                        </div>
                                    </td>
                                    <td>
                                        <div id="selectedGroupCap">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="buttonArea" class="ACA_LgButton ACA_LgButton_FontSize ACA_LiLeft">
                            <ul>
                                <li>
                                    <div id="btnGroupOK" />
                                </li>
                                <li>
                                    <div id="btnGroupCancel" />
                                </li>
                                <li>
                                    <div id="btnGroupDelete">
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <!--End CAP type group section -->
    </div>
    <noscript>
        <%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</body>
</html>
