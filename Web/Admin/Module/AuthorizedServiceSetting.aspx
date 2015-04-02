<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthorizedServiceSetting.aspx.cs" Inherits="Accela.ACA.Web.Admin.Module.AuthorizedServiceSetting" %>

<%@ Import Namespace="Accela.ACA.Web.AuthorizedAgent" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Authorized Agent Settings </title>
    <link rel="stylesheet" type="text/css" href="../resources/css/ext-all.css" />
    <link rel="stylesheet" type="text/css" href="../styles/main.css" />
    
    <script type="text/javascript" src="../../scripts/jquery.js"></script>
    <script type="text/javascript" src="../../scripts/GlobalConst.aspx"></script>
    <script type="text/javascript" src="../../scripts/global.js"></script>
    <script type="text/javascript" src="../adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../scripts/ext-all.js"></script>
    <script type="text/javascript" src="../scripts/ext-extension.js"></script>
    <script type="text/javascript" src="../TreeJSON/LabelKey.aspx"></script> 

</head>
<body class="authagent_setting_body">
    <script type="text/javascript">
        var btnOK;
        var btnCancel;
        var moduleStore;
        var filterStore;
        var mouduleCombo;
        var capTypeFiltercombo;
		var hasAuthorizedServiceConfig = <%=AuthorizedAgentServiceUtil.HasAuthorizedServiceConfig().ToString().ToLower()%>;
        
        Ext.BLANK_IMAGE_URL = "../resources/images/default/s.gif";
        
        Ext.onReady(function () {   // render capType grid and buttons
            Ext.state.Manager.setProvider(new Ext.state.CookieProvider());
			
			if (!hasAuthorizedServiceConfig) {
                    Ext.Msg.show({
                    title:'Notice',
                    msg: '<%= GetTextByKey("acaadmin_authagent_msg_nostddefined").Replace("'", "\\'")%>',
                    closable: false,
                    fn: function(){},
                    animEl: 'elId',
                    icon: Ext.MessageBox.INFO
                });
            }

            // render OK and Cancel buttons
            btnOK = new Ext.Button({
                id: "btnOK", text: " OK ", type: "button", minWidth: "80", handler: function () {
                    SaveServiceSetting();
            }});
            btnOK.render('tdSaveFilter');

            btnCancel = new Ext.Button({
                id: "btnCancel", text: " Cancel ", type: "button", minWidth: "80", handler: function () {
                    GetServiceSetting();
                }
            });
            btnCancel.render('tdCancelSaveFilter');

            filterStore = new Ext.data.SimpleStore({
                fields: ['filter'],
                data: []
            });

            Accela.ACA.Web.WebService.AdminConfigureService.GetModules(function (modules) {
                moduleStore = new Ext.data.SimpleStore({
                    fields: ['key', 'text'],
                    data: eval('(' + modules + ')')
                });
                
                mouduleCombo = new Ext.form.ComboBox({
                    fieldLabel: 'Module Name',
                    store: moduleStore,
                    displayField: 'text',
                    valueField: 'key',
                    emptyText: Ext.LabelKey.Admin_CapTypeFilter_ddlEmptyText,
                    id: 'ddlModuleList',
                    editable: false,
                    forceSelection: true,
                    triggerAction: 'all',
                    mode : 'local',
                    allowBlank: true,
                    listeners: {
                        select: function (combo, record, index, capTypeFilter) {
                           if (hasAuthorizedServiceConfig) {
                                capTypeFiltercombo.clearValue();
                                Accela.ACA.Web.WebService.AdminConfigureService.GetCapTypeFilterListByModule(combo.value, function (filterNames) {
                                    var filterJson = eval('(' + filterNames + ')');
                                    var savedCapTypeFilter = '';

                                    for (var i = 0; i < filterJson.length; i++) {
                                        filterJson[i][0] = JsonDecode(filterJson[i][0]);
                                        
                                        if (filterJson[i][0] == capTypeFilter) {
                                            savedCapTypeFilter = capTypeFilter;
                                        }
                                    }

                                    capTypeFiltercombo.clearValue();
                                    capTypeFiltercombo.store.loadData(filterJson);

                                    if (savedCapTypeFilter) {
                                        capTypeFiltercombo.setValue(savedCapTypeFilter);
                                    }
                               });

                                btnOK.enable();
                                btnCancel.enable();
                                parent.parent.ModifyMark('huntting');

                                document.FishAndHunttingChanged = true;
                           }
                        }
                    },
                });
                
                mouduleCombo.render('tdModuleList');
                
                GetServiceSetting();
            });

            capTypeFiltercombo = new Ext.form.ComboBox({
                store: filterStore,
                displayField: 'filter',
                valueField: 'filter',
                emptyText: Ext.LabelKey.Admin_CapTypeFilter_ddlEmptyText,
                id: 'ddlCaptypeFilterList',
                editable: false,
                forceSelection: true,
                triggerAction: 'all',
                mode: 'local',
                allowBlank: true,
                listeners: {
                    select: function (combo, record, index) {
                        if (hasAuthorizedServiceConfig) {
                            btnOK.enable();
                            btnCancel.enable();
                            parent.parent.ModifyMark('AuthorizedServiceSettings');
                            document.FishAndHunttingChanged = true;
                            }
                        }
                    }
                });
            
            capTypeFiltercombo.render('tdCapTypeFilterList');

        });
        
        function SaveServiceSetting() {
            Accela.ACA.Web.WebService.AdminConfigureService.SaveFishAndHuntingSetting(Ext.getCmp('ddlModuleList').value, Ext.getCmp('ddlCaptypeFilterList').value, function () {
                btnOK.disable();
                btnCancel.disable();
                parent.parent.RemoveMark(false, 'AuthorizedServiceSettings');
            });
        }

        function GetServiceSetting() {
            Accela.ACA.Web.WebService.AdminConfigureService.GetFishAndHuntingSetting(function (settings) {
                settings = eval('(' + settings + ')');
                
                if (settings.ModuleName) {
                    mouduleCombo.setValue(settings.ModuleName);
                    mouduleCombo.fireEvent('select', mouduleCombo, null, null, settings.CapTypeFilterName);
                }
                
                btnOK.disable();
                btnCancel.disable();
                parent.parent.RemoveMark(false, 'AuthorizedServiceSettings');
            });
        }
    </script>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            <Services>
                <asp:ServiceReference Path="../../WebService/AdminConfigureService.asmx" />
            </Services>
        </asp:ScriptManager>
        <!--Begin Cap Search Role section -->
        <fieldset class="module_settings_section">
            <legend>
                <span ID="lblFHHeader" runat="server" class="ACA_New_Title_Label font12px"></span>
            </legend>
            <table class="module_setting_section_wrapper" role="presentation">
                <tr>
                    <td>
                        <div class="ACA_NewDiv_Text">
                            <div class="sectionheader">
                                <ACA:AccelaLabel ID="AccelaLabel8" LabelKey="aca_admin_auth_agent_service_label_description"
                                    runat="server" CssClass="ACA_New_Head_Label_Width_90 font11px"></ACA:AccelaLabel>
                            </div>
                        </div>
                        <div class="sectionbody">
                            <table id="Table1" cellspacing="10" cellpadding="4" role="presentation">
                                <tr>
                                    <td class="ACA_New_Title_Label font12px">Module Name:</td>
                                    <td id="tdModuleList"></td>
                                    <td class="ACA_New_Title_Label font12px">Record Type Filter:</td>
                                    <td id="tdCapTypeFilterList"></td>
                                </tr>
                            </table>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10" id="CapTypeSearchRoleGrid">
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                            <table id="Table2" cellspacing="10" cellpadding="4" role="presentation">
                                <tr>
                                    <td id="tdSaveFilter"></td>
                                    <td id="tdCancelSaveFilter"></td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
        <!--End Cap Search Role section -->
    </form>
</body>
</html>
