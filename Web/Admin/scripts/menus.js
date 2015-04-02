/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: menu.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: menu.js 72643 2008-04-24 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/18/2008     		Kevin.Feng				Initial.  
 * </pre>
 */
var currentLanguage;
var currentAgency;

function IsSupperAgencyEnabled() {
    var conn = Ext.lib.Ajax.getConnectionObject().conn;
    conn.open("GET", "Pageflow/WorkflowContent.aspx?action=IsSupperAgencyEnabled&isGetParentAgency=true&temp=" + new Date(), false);
    conn.send(null);
    return conn.responseText == 'Y';
}

Ext.onReady(function () {

    Ext.QuickTips.init();

    var menu = new Ext.menu.Menu({
        id: 'mainMenu'
    });

    var tb = new Ext.Toolbar();
    tb.render('toolbar');
    if (tb.el != null && tb.el.dom != null && tb.el.dom.childNodes != null) {
        tb.el.dom.childNodes[0].setAttribute("Role", "presentation");
    }

    tb.add(
        {
            id: 'btnSave',
            text: [Ext.LabelKey.Admin_Menu_Save],
            iconCls: 'btnSave',
            disabled: true,
            listeners: {
                click: function (obj) {
                    SaveHandle(Ext.Const.OpenedId);
                    obj.enable();
                }
            }
        },
        '-',
        {
            text: [Ext.LabelKey.Admin_Menu_Close],
            iconCls: 'btnClose',
            listeners: {
                click: function () {
                    PageMethods.ClearAllCache(true);
                    window.close();
                }
            }
        },
        '-',
        {
            text: [Ext.LabelKey.Admin_Menu_Help],
            iconCls: 'btnHelp',
            id: 'btnHelp',
            listeners: {
                click: function () {
                    window.open('../help/Help.pdf', 'Help', "top=0, left=0, toolbar=yes, menubar=yes, scrollbars=yes, resizable=yes,location=yes, status=yes, width=" + (screen.availWidth - 10) + ",height=" + screen.availHeight);
                }
            }
        },
        '-',
        {
            text: [Ext.LabelKey.Admin_Global_Setting_Label_Cache_Clear],
            id: 'btnClearCache',
            iconCls: 'btnClearCache',
            listeners: {
                click: function () {
                    PageMethods.ClearAllCache(false);
                    alert('Clear cache successfully');
                }
            }
        }
        );

    SetAccessKey4MenuButton('btnSave', 'S');
    SetAccessKey4MenuButton('btnHelp', 'Z');

    Ext.Const.IsSupportMultiLang = IsSupportMultiLanguage();

    if (Ext.Const.IsSupportMultiLang) {
        Ext.Const.DefaultLang = GetDefaultLanguage();

        var languageStore = new Ext.data.SimpleStore({
            url: 'TreeJSON/I18nCulture.aspx',
            baseParams: {
                action: 'GetLanguageList'
            },
            fields: ['id', 'text'],
            listeners: {
                load: function () {
                    SetPreferredCultrue();
                }
            }
        });
        languageStore.load();

        var combo = new Ext.form.ComboBox({
            id: 'ddlLanguage',
            store: languageStore,
            editable: false,
            displayField: "text",
            tpl: '<tpl for="."><div ext:qtip="{text}" class="x-combo-list-item">{text}</div></tpl>',
            valueField: "id",
            mode: 'local',
            triggerAction: 'all',
            selectOnFocus: true,
            forceSelection: true,
            width: 100

        });

        tb.addFill();
        tb.addField(combo);
        currentLanguage = combo.getValue();
        combo.on('select', ChangeLanguage);
    }

    if (IsSupperAgencyEnabled()) {
        var agencyStore = new Ext.data.SimpleStore({
            url: 'Pageflow/WorkFlowContent.aspx',
            baseParams: {
                action: 'GetAgencyList'
            },
            fields: ['id', 'text'],
            listeners: {
                load: function () {
                    LoadAgency();
                }
            }
        });

        agencyStore.load();

        var comboAgency = new Ext.form.ComboBox({
            id: 'ddlAgency',
            store: agencyStore,
            editable: false,
            displayField: "text",
            valueField: "id",
            mode: 'local',
            tpl: '<tpl for="."><div ext:qtip="{text}" class="x-combo-list-item">{text}</div></tpl>',  
            triggerAction: 'all',
            selectOnFocus: true,
            forceSelection: true,
            width: 150

        });

        tb.addFill();
        tb.addField(comboAgency);
        currentAgency = comboAgency.getValue();
        comboAgency.on('select', ChangeAgency);
    }
});

function LoadAgency() {
    Ext.Ajax.request
   ({
       url: 'Pageflow/WorkFlowContent.aspx',
       params: { action: 'GetCurrentAgencyCode' },
       method: "GET",
       success: function (req) {
           currentAgency = req.responseText;
           Ext.getCmp('ddlAgency').setValue(currentAgency);
       }
   }); 
}

function SetAccessKey4MenuButton(btnId, accessKey) {
    if (Ext.get(btnId)) {
        var dom = Ext.get(btnId).dom;
        if (dom && dom.getElementsByTagName('BUTTON')[0]) {
            dom.getElementsByTagName('BUTTON')[0].accessKey = accessKey;
        }
    }
}

function IsSupportMultiLanguage()
{ 
   var conn = Ext.lib.Ajax.getConnectionObject().conn; 
   conn.open("GET", "TreeJSON/I18nCulture.aspx?action=MultiLanguageSupportEnable",false); 
   conn.send(null);
   return conn.responseText == 'Y';
}

function SetPreferredCultrue()
{
   Ext.Ajax.request
   ({
        url : 'TreeJSON/I18nCulture.aspx',
        params : { action : 'GetPreferredCulture'},
        method:"GET",
        success:function(req)
        {
           currentLanguage=req.responseText;
           Ext.getCmp('ddlLanguage').setValue(currentLanguage);
        }
   }); 
}

function GetDefaultLanguage()
{
    var conn = Ext.lib.Ajax.getConnectionObject().conn; 
    conn.open("GET", "TreeJSON/I18nCulture.aspx?action=GetDefaultLanguage",false); 
    conn.send(null);
    return conn.responseText;
}

function IsSesstionTimeout()
{
    var conn = Ext.lib.Ajax.getConnectionObject().conn; 
    conn.open("POST", 'GetSessionState.aspx',false); 
    conn.setRequestHeader('Content-Type','application/x-www-form-urlencoded'); 
    conn.send('../Admin/GetSessionState.aspx');
    return conn.responseText != 'Y'; 
}

function RedirectToLoginPage()
{
    window.location.href = 'login.aspx?timeout=true';
}

// when user change language and there are unsaved changes,display a confirm window to prompt user
function ChangeLanguage(combo, record) {
    if (Ext.getCmp('ddlLanguage').getValue() == currentLanguage) {
        return;
    } else {
        if (Ext.Const.OpenedId == -1)   // no opened tabs
        {
            currentLanguage = Ext.getCmp('ddlLanguage').getValue();
            ReloadPage();
        } else {
            var changedNum = 0;
            var tabs = Ext.getCmp("tabs");
            
            for (var i = 0; i < tabs.items.length; i++) {
                if (tabs.items.items[i].title.indexOf('*') > 0) {
                    changedNum = changedNum + 1;
                }
            }

            if (changedNum == 0) { // no tabs changed,just close opened tabs and refresh page.
                currentLanguage = Ext.getCmp('ddlLanguage').getValue();

                CloseOpenedTabs();
                ReloadPage();
            } else {
                if (confirmMsg(Ext.LabelKey.Admin_Frame_ChangeLanguage_Confirm)) {
                    currentLanguage = Ext.getCmp('ddlLanguage').getValue();

                    CloseOpenedTabs();
                    ReloadPage();

                    //Clear all change items data if change language and some changes haven't been saved.
                    changeItems.RemoveAllChangeItems();
                } else {
                    Ext.getCmp('ddlLanguage').setValue(currentLanguage);
                }
            }
        }
    }
}

function GetSelectedAgencyCode() {
    return Ext.getCmp('ddlAgency').getValue();
 }

 // when user change language and there are unsaved changes,display a confirm window to prompt user
 function ChangeAgency(combo, record) {
     if (Ext.getCmp('ddlAgency').getValue() == currentAgency) {
         return;
     }
     else {
         currentAgency = Ext.getCmp('ddlAgency').getValue();
     }

     Ext.Ajax.request
        ({
            url: 'Pageflow/WorkFlowContent.aspx',
            params: { action: 'SetAgencyCode', agencyCode: currentAgency },
            method: "POST",
            success: function () {
                window.location.reload();
            }
        });
 }

function CloseOpenedTabs()
{
   var tabs = Ext.getCmp("tabs");
   tabs.items.each(function(item){
      if(item.closable){
             item.setTitle(item.title.replace(' *',''));  // don't need save,remove the flag
             tabs.remove(item);
      }
   });
}

// when language changed, refresh page.
function ReloadPage()
{
       Ext.Ajax.request({
                url : 'TreeJSON/I18nCulture.aspx',
                params : { action : 'ChangeLanguage', language:currentLanguage},
                method:"GET"
       });
}


function IsDefaultLanguage() {
    var isDefaultLang = false;

    if (Ext.Const.IsSupportMultiLang) {
        var currentLang = Ext.getCmp('ddlLanguage').getValue();

        if (Ext.Const.DefaultLang.indexOf('-') == -1 && currentLang.indexOf('-') != -1) //defaultLanguage:en,ar.. currentLanguage:en-US...
        {
            currentLang = currentLang.substring(0, currentLang.indexOf('-'));
        }

        if (Ext.Const.DefaultLang == currentLang) {
            isDefaultLang = true;
        }
    }

    return isDefaultLang;
}