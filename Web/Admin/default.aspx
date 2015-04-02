<%@ Page Language="C#" AutoEventWireup="true" Inherits="ACA.Admin.Default" Codebehind="Default.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="resources/css/ext-all.css" />
    <link rel="stylesheet" type="text/css" href="styles/menus.css" />
    
    <script type="text/javascript" src="../Scripts/jquery.js"></script>
    <script type="text/javascript" src="adapter/yui/yui-utilities.js"></script>
    <script type="text/javascript" src="adapter/yui/ext-yui-adapter.js"></script>
    <script type="text/javascript" src="adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="scripts/ext-all.js"></script>
    <script type="text/javascript" src="scripts/ext-extension.js"></script>
    <script type="text/javascript" src="scripts/changedItems.js"></script> 
    <script type="text/javascript" src="scripts/pageSave.js"></script>
    <script type="text/javascript" src="TreeJSON/LabelKey.aspx"></script>
    <script type="text/javascript" src="scripts/common.js"></script>
    <script type="text/javascript" src="scripts/layout.js"></script>
    <script type="text/javascript" src="scripts/outlookBar.js"></script>
    <script type="text/javascript" src="TreeJSON/LoadModuleNodes.aspx"></script>
    <script type="text/javascript" src="scripts/menus.js"></script>
    <script type="text/javascript" src="scripts/tabCloseMenu.js"></script>
    <script type="text/javascript" src="scripts/PropertyPanelHelper.js"></script>
    <script type="text/javascript" src="scripts/dropListPanel.js"></script>
    <script type="text/javascript" src="scripts/gridviewDD.js"></script>
    <script type="text/javascript" src="scripts/fieldProperties.js"></script>
    <script type="text/javascript" src="scripts/ctlObj.js"></script>
    <script type="text/javascript" src="scripts/propertyPanel.js"></script>
    <script type="text/javascript" src="scripts/tabPropertyPanel.js"></script>
    <script type="text/javascript" src="scripts/tabDragDrop.js"></script>
    <script type="text/javascript" src="scripts/sectionProperties.js"></script>
    <script type="text/javascript" src="scripts/ExtConst.js"></script>
    <script type="text/javascript" src="../Scripts/GlobalConst.aspx"></script>
    <script type="text/javascript" src="../Scripts/global.js"></script>
</head>
<body>
    <span style="display:none">
        <ACA:AccelaHideLink ID="hlSkipToolBar" runat="server" AltKey="img_alt_modulemenu_skiplink" IsSkippingToAnchor="true" NextControlClientID="FirstAnchorInACAMainContent" TabIndex="-1"/>
        <a name="FirstAnchorInACAMainContent" id="FirstAnchorInACAMainContent" tabindex="-1"></a>
    </span>
    <link rel="stylesheet" type="text/css" href="styles/main.css" />
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            <Services>
                <asp:ServiceReference Path="../WebService/AdminConfigureService.asmx" />
            </Services>
        </asp:ScriptManager>
        <div id="north">
            <div id="toolbar">
            </div>
        </div>
        
        <div id="center1">
            <div id="ifrm">
            </div>
        </div>
    </form>
    <noscript><%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</body>
<script type="text/javascript">
    var V360URL = '<%=Accela.ACA.Common.Common.ScriptFilter.EncodeUrlEx(V360URL)%>';
    var Form_Designer_Title = '<%=GetTextByKey("aca_form_designer_title").Replace("'","\\'")%>';
    var ACA_Form_Designer_No_Saving_Description = '<%=GetTextByKey("aca_form_designer_no_saving_description").Replace("'","\\'")%>';
    var ACA_Form_Designer_Without_Saving_Msg = '<%=GetTextByKey("aca_form_designer_without_saving").Replace("'","\\'")%>';
    var ACA_Form_Designer_Change_Type_Without_Saving_Msg = '<%=GetTextByKey("aca_formdesigner_change_type_without_saving").Replace("'","\\'")%>';
    document.body.onbeforeunload=function(){
        var tabs = Ext.getCmp("tabs");
        for(var i=0;i<tabs.items.length;i++){
            if(!IsSesstionTimeout() && tabs.items.items[i].title.indexOf('*')>0){
                return(Ext.LabelKey.Admin_Frame_CloseWindow);
            }
        }
    }

    var isNeedRefreshSection = false;
    function ChangeRefreshTag(tag) {
        isNeedRefreshSection = tag;
    }
    
    var isDataChanged = false;
    function DataChangedTag(tag) {
        isDataChanged = tag;
    }
    
    function closeRearrangeWin() {
        silverlightWin = new Ext.Window({
            title: Form_Designer_Title,
            layout: 'fit',
            width: 710,
            height: 500,
            modal: true,
            closeAction: 'close',
            plain: true
        });
    }

    function CheckDataChangeTarget() {
        if (isDataChanged) {
            if (!confirm(ACA_Form_Designer_Change_Type_Without_Saving_Msg)) {
                return false;
            }
            else {
                isDataChanged = false;
                return true;
            }
        }
        else {
            return true;
        }
    }
</script>
<script type="text/javascript">
    //Get web application root, ends with /
    function getAppRoot() {
        return '<%=Accela.ACA.Web.Common.FileUtil.ApplicationRoot %>';
    }
</script>
</html>