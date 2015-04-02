<%@ Page Language="C#" AutoEventWireup="true" Inherits="Admin_Module_Workflow" Codebehind="Workflow.aspx.cs" %>
<%@ Import Namespace="Accela.ACA.Web.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" type="text/css" href="../resources/css/ext-all.css" />
    <link rel="stylesheet" type="text/css" href="../styles/Pageflow.css" />
    <link rel="Stylesheet" type="text/css" href="../styles/menus.css" />
    
    <script type="text/javascript" src="../../Scripts/jquery.js"></script>
    <script type="text/javascript" src="../adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../scripts/ext-all.js"></script>
    <script type="text/javascript" src="../scripts/ext-extension.js"></script>
    <script type="text/javascript" src="../TreeJSON/LabelKey.aspx"></script>
    <script type="text/javascript" src="../scripts/PropertyPanelHelper.js"></script>
    <script type="text/javascript" src="../scripts/WorkflowLib.js"></script>
    <script type="text/javascript" src="../scripts/Workflow.js"></script>
    <script type="text/javascript" src="../../Scripts/GlobalConst.aspx"></script>
    <script type="text/javascript" src="../../Scripts/global.js"></script>
</head>
<body>
    <form id="form1" method="post" runat="server">
    </form>
    <div id="Workflow" style="padding: 5px; width: 98%">
            <div id="divHeader">
            </div>
            <div id="divBody">
                <div id="pageflow" style="padding-top:10px;"></div>
                <div id="capList"></div>
                <div id="capAssociation">
                    <div id="smartchoiceGroupName">
                    </div> 
                    <div id="newSmartchoiceGroup">
                        <table border="0" width="98%" role="presentation">
                            <tr>
                                <td colspan="3">
                                    <div id="txtSmartchoiceGroupNewName">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="availableCap">
                                    </div>
                                </td>
                                <td valign="middle" align="center">
                                    <div id="addCap">
                                    </div>
                                    <br />
                                    <br />
                                    <div id="removeCap">
                                    </div>
                                </td>
                                <td>
                                    <div id="selectedCap">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="buttonArea" class="ACA_LgButton ACA_LgButton_FontSize ACA_LiLeft">
                        <ul>
                            <li>
                                <div id="OK" />
                            </li>
                            <li>
                                <div id="Cancel" />
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
    </div>
    <div id="property"></div>
    
    <!--menu-->
    <div onselectstart="return false" id="_div_menu" class="menuskin" onMouseover="highlight()" onMouseout="lowlight()" onClick="jump();"> 
        <div id="menu_subDiv" class="menuitems" url="javascript:void;" onclick="delComponent();">Remove Component</div> 
    </div> 
    <div id="properpyBlur"></div>
</body>
</html>
