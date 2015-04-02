<%@ Page Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Admin.ModuleSetup" Codebehind="ModuleSetup.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Module Setup</title>
    <link rel="stylesheet" type="text/css" href="../resources/css/ext-all.css" />
    
    <script type="text/javascript" src="../../scripts/jquery.js"></script>
    <script type="text/javascript" src="../adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../scripts/ext-all.js"></script>
    <script type="text/javascript" src="../scripts/ext-extension.js"></script>
    <script type="text/javascript" src="../../scripts/GlobalConst.aspx"></script>
    <script type="text/javascript" src="../../Scripts/global.js"></script>

    <style type="text/css">
        .x-tree-node-icon {display: none;}
    </style>

    <script type="text/javascript">  

    Ext.BLANK_IMAGE_URL="../resources/images/default/s.gif";

    var tree;
    var moduleName;

    Ext.onReady(function () {
        // shorthand   
        var Tree = Ext.tree;

        // set the root node   
        var root = new Tree.AsyncTreeNode({
            text: 'root',
            id: 'root'

        });
        tree = new Tree.TreePanel({
            autoScroll: true,
            animate: true,
            enableDD: false,
            containerScroll: true,
            root: root,
            lines: true,
            rootVisible: false,
            loader: new Tree.TreeLoader({
                dataUrl: '../TreeJSON/TreeModule.aspx',
                baseAttrs: { checked: false }

            }),

            border: false
        });

        tree.on('checkchange', function (node, checked) {
            node.expand();
            node.ui.removeClass('icondoc');
            node.attributes.checked = checked;

            if (node.attributes.hiberarchy == 1) {
                if (node.parentNode.id != "root") {
                    hasSelectChild = false;
                    var cs = node.parentNode.childNodes;
                    for (var i = 0; i < cs.length; i++) {
                        if (cs[i].attributes.checked === true) {
                            hasSelectChild = true;
                            break;
                        }
                    }

                    if (hasSelectChild) {
                        node.parentNode.ui.toggleCheck(true);
                        node.parentNode.attributes.checked = true;
                        ChangeItem(node.parentNode.attributes.id + "@" + node.parentNode.attributes.checked + "@" + node.parentNode.attributes.pagetype + "@" + node.parentNode.attributes.elementid + "@" + node.parentNode.attributes.elementname + "@" + node.parentNode.attributes.parentid + "@" + node.parentNode.attributes.labelkey, node);
                    }
                    else {
                        node.parentNode.ui.toggleCheck(false);
                        node.parentNode.attributes.checked = false;
                        ChangeItem(node.parentNode.attributes.id + "@" + node.parentNode.attributes.checked + "@" + node.parentNode.attributes.pagetype + "@" + node.parentNode.attributes.elementid + "@" + node.parentNode.attributes.elementname + "@" + node.parentNode.attributes.parentid + "@" + node.parentNode.attributes.labelkey, node);
                    }
                }
            }
            else if (node.attributes.hiberarchy == 2) {

                if (node.attributes.forcelogin) {
                    if (node.attributes.forcelogin.toUpperCase() == "YES" || node.attributes.forcelogin.toUpperCase() == "Y") {
                        node.attributes.forcelogin = "NO";
                        node.parentNode.attributes.forcelogin = "NO";
                    }
                    else {
                        node.attributes.forcelogin = "YES";
                        node.parentNode.attributes.forcelogin = "YES";

                        node.parentNode.ui.toggleCheck(true);
                        node.parentNode.attributes.checked = true;
                        ChangeItem(node.parentNode.attributes.id + "@" + node.parentNode.attributes.checked + "@" + node.parentNode.attributes.pagetype + "@" + node.parentNode.attributes.elementid + "@" + node.parentNode.attributes.elementname + "@" + node.parentNode.attributes.parentid + "@" + node.parentNode.attributes.labelkey, node);
                        node.parentNode.parentNode.ui.toggleCheck(true);
                        node.parentNode.parentNode.attributes.checked = true;
                        ChangeItem(node.parentNode.parentNode.attributes.id + "@" + node.parentNode.parentNode.attributes.checked + "@" + node.parentNode.parentNode.attributes.pagetype + "@" + node.parentNode.parentNode.attributes.elementid + "@" + node.parentNode.parentNode.attributes.elementname + "@" + node.parentNode.parentNode.attributes.parentid + "@" + node.parentNode.parentNode.attributes.labelkey, node);
                    }
                }

                if (node.attributes.singleserviceonly) {
                    if (node.attributes.singleserviceonly.toUpperCase() == "YES" || node.attributes.singleserviceonly.toUpperCase() == "Y") {
                        node.attributes.singleserviceonly = "NO";
                        node.parentNode.attributes.singleserviceonly = "NO";
                    }
                    else {
                        node.attributes.singleserviceonly = "YES";
                        node.parentNode.attributes.singleserviceonly = "YES";

                        node.parentNode.ui.toggleCheck(true);
                        node.parentNode.attributes.checked = true;
                        ChangeItem(node.parentNode.attributes.id + "@" + node.parentNode.attributes.checked + "@" + node.parentNode.attributes.pagetype + "@" + node.parentNode.attributes.elementid + "@" + node.parentNode.attributes.elementname + "@" + node.parentNode.attributes.parentid + "@" + node.parentNode.attributes.labelkey, node);
                        node.parentNode.parentNode.ui.toggleCheck(true);
                        node.parentNode.parentNode.attributes.checked = true;
                        ChangeItem(node.parentNode.parentNode.attributes.id + "@" + node.parentNode.parentNode.attributes.checked + "@" + node.parentNode.parentNode.attributes.pagetype + "@" + node.parentNode.parentNode.attributes.elementid + "@" + node.parentNode.parentNode.attributes.elementname + "@" + node.parentNode.parentNode.attributes.parentid + "@" + node.parentNode.parentNode.attributes.labelkey, node);
                    }
                }

                ChangeItem(node.parentNode.attributes.id + "@" + node.parentNode.attributes.checked + "@" + node.parentNode.attributes.pagetype + "@" + node.parentNode.attributes.elementid + "@" + node.parentNode.attributes.elementname + "@" + node.parentNode.attributes.parentid + "@" + node.parentNode.attributes.labelkey, node);
            }

            if (node.attributes.hiberarchy != 2) {
                ChangeItem(node.attributes.id + "@" + node.attributes.checked + "@" + node.attributes.pagetype + "@" + node.attributes.elementid + "@" + node.attributes.elementname + "@" + node.attributes.parentid + "@" + node.attributes.labelkey, node);
            }

            node.eachChild(function (child) //hiberarchy=0(Module CheckBox change will trigger this metrod)
            {
                if (child.attributes.checked != checked && child.attributes.hiberarchy != 2) {
                    child.ui.toggleCheck(checked);
                    child.attributes.checked = checked;
                    child.fireEvent('checkchange', child, checked);
                    ChangeItem(node.attributes.id + "@" + node.attributes.checked + "@" + node.attributes.pagetype + "@" + node.attributes.elementid + "@" + node.attributes.elementname + "@" + node.attributes.parentid + "@" + node.attributes.labelkey, node);
                }
                else if (child.attributes.hiberarchy == 2 && child.attributes.checked == true) {
                    child.ui.toggleCheck(false);
                    child.attributes.checked = false;
                    child.fireEvent('checkchange', child, false);
                    ChangeItem(node.parentNode.attributes.id + "@" + node.parentNode.attributes.checked + "@" + node.parentNode.attributes.pagetype + "@" + node.parentNode.attributes.elementid + "@" + node.parentNode.attributes.elementname + "@" + node.parentNode.attributes.parentid + "@" + node.parentNode.attributes.labelkey, node);
                }
            });
        }, tree);

        tree.on('dblclick', function (node, e) {

        }, tree);

        tree.setRootNode(root);

        // render the tree   
        root.expand();
        //tree.expandAll();

        var divTree = document.getElementById("divTree");
        //var divTreeParent = document.getElementById("divTreeParent");

        tree.render(divTree);
        //divTreeParent.insertAdjacentElement('beforeEnd',divTree);
    });  
    
    //Create global setting page information array.
    function PageItems(pageId, moduleName, id, checked, pagetype, elementid, elementname, parentid, labelkey, forcelogin, singleselectiononly) {
        this.PageId = pageId;
        this.ModuleName = moduleName;
        this.NodeId = id;
        this.Checked = checked;
        this.PageType = pagetype;
        this.ElementId = elementid;
        this.ElementName = elementname;
        this.ParentId = parentid;
        this.LabelKey = labelkey;
        this.ForceLogin = forcelogin;
        this.SingleSelectionOnly = singleselectiononly;        
    }
            
    //Change element to packge array.
    function ChangeItem(value, node) {
        moduleName = parent.parent.Ext.Const.ModuleName;
        var item = value.split("@");
        
        var forceLogin;
        var singleSelectionOnly;
        var childrenNodes = node.parentNode.childNodes;

        for (var i = 0; i < childrenNodes.length; i++) {
            if (childrenNodes[i].attributes.singleserviceonly) {
                singleSelectionOnly = childrenNodes[i].attributes.singleserviceonly;
            }

            if (childrenNodes[i].attributes.forcelogin) {
                forceLogin = childrenNodes[i].attributes.forcelogin;
            }
        }

        var pageItems = new PageItems('module', moduleName, item[0], item[1], item[2], item[3], item[4], item[5], item[6], forceLogin, singleSelectionOnly);
        parent.parent.pageModuleItems.UpdatePageItem('module', pageItems);
        parent.parent.ModifyMark();
    }
    
    //Get Gis information.
    function GetModuleSetupLabelKeyInfo()
    {
        Accela.ACA.Web.WebService.AdminConfigureService.GetModuleSetupLabelKeyInfo(CallBack);
    }
    
    //Fill Gis information.
    function CallBack(result)
    {
        var divModuleHead = document.getElementById("divModuleHead");
        
        divModuleHead.innerHTML = result[0];
    }
    
    </script>

</head>
<body style="margin-left:6px;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            <Services>
                <asp:ServiceReference Path="../../WebService/AdminConfigureService.asmx" />
            </Services>
        </asp:ScriptManager>
        <span style="display:none">
            <ACA:AccelaHideLink ID="hlSkipToolBar" runat="server" AltKey="img_alt_modulemenu_skiplink" IsSkippingToAnchor="true" NextControlClientID="FirstAnchorInACAMainContent" TabIndex="-1"/>
            <a name="FirstAnchorInACAMainContent" id="FirstAnchorInACAMainContent" tabindex="-1"></a>
        </span>        
        <span id="FirstAnchorInAdminMainContent" tabindex="-1"></span>
        <div class="ACA_PaddingStyle">
            <fieldset style="padding: 8px;">
                <legend>
                        <ACA:AccelaLabel ID="lblModuleSetupTitle" LabelKey="admin_module_setup_label_module_setup_title"
                            runat="server" CssClass="ACA_Label ACA_Label_FontSize"></ACA:AccelaLabel>
                </legend>
                <div class="ACA_NewDiv_Text">
                    <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                        <div id="divModuleHead" class="ACA_New_Head_Label_Width_100 font11px">
                        </div>
                    </div>
                </div>
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_15 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                    <table role="presentation">
                        <tr>
                            <td>
                                <div id="divTree">
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </fieldset>
        </div>
    </form>
    <noscript><%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</body>
<script type="text/javascript">
    //Get web application root, ends with /
    function getAppRoot() {
        return '<%=Accela.ACA.Web.Common.FileUtil.ApplicationRoot %>';
    }
</script>
</html>