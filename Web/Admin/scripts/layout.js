/*
 * <pre>
 *  Accela Citizen Access
 *  File: layout.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: layout.js 77905 2007-10-15 12:49:28Z ACHIEVO\lytton.cheng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
*/

/*
    ||Feature:
    ||  Layout with ExtJS
    ||Paraments:
    ||  none
    ||Create By:
    ||  Kevin.Feng@achievo.com
    ||Create Date:
    ||  Mar 18,2008
*/

Ext.onReady(function () {
    Ext.QuickTips.init();
    /*
    ||If need to keep views state, you can use code here. 
    */
    //Ext.state.Manager.setProvider(new Ext.state.CookieProvider());

    var viewport = new Ext.Viewport({
        id: 'lo',
        layout: 'border',
        items: [
            new Ext.BoxComponent({
                id: 'north',
                region: 'north',
                el: 'north',
                height: '31px'
            }),
            {
                region: 'east',
                id: 'east-panel',
                title: '<div id="toolsPanel">Tools</div>',
                collapsible: true,
                split: true,
                width: 280,
                minSize: 175,
                autoScroll: true,
                maxSize: 400,
                html: '<div id="divField" style="width: 100%;"></div>',
                layout: 'accordion',
                listeners: {
                    'beforehide': function (obj) {
                        if (Ext.isIE) {
                            if (obj.body.dom.offsetParent.offsetParent != null) {
                                obj.body.dom.offsetParent.offsetParent.style.width = 2001;
                            } else {
                                obj.body.dom.offsetParent.style.width = 2001;
                            }
                        } else {
                            var parentDiv = document.getElementById("east-panel");
                            if (parentDiv) {
                                parentDiv.style.width = "1px";
                            }
                        }
                    },
                    'beforeshow': function (obj) {
                        if (Ext.isIE) {
                            if (obj.body.dom.offsetParent.offsetParent != null) {
                                obj.body.dom.offsetParent.offsetParent.style.width = 2000;
                            } else {
                                obj.body.dom.offsetParent.style.width = 2000;
                            }

                        } else {
                            var parentDiv = document.getElementById("east-panel");
                            if (parentDiv) {
                                parentDiv.style.width = "280px";
                            }
                        }
                    }
                },
                margins: '0 5 0 0'
            },
            {
                region: 'west',
                id: 'west-panel',
                title: [Ext.LabelKey.Admin_LeftPanel_Navigation],
                split: true,
                width: 208,
                minSize: 208,
                maxSize: 400,
                collapsible: true,
                margins: '0 0 0 5',
                html: '<div id="outlookBar" style="width:100%;height:100%"></div>'
            },
            new Ext.TabPanel({
                id: 'tabs',
                region: 'center',
                deferredRender: true,
                enableTabScroll: true,
                width: '100%',
                activeTab: 0,
                hideBorders: true,
                plugins: new Ext.ux.TabCloseMenu(),
                listeners: {
                    /*
                    * When closed last the tab, set Preview button is disable, and Ext.Const.OpenedId is reset.
                    */
                    beforeremove: function (self, p) {
                        var tabs = Ext.getCmp('tabs');

                        if (!IsSesstionTimeout() && p.title.indexOf('*') > 0) {
                            var currTabTxt = DecodeHTMLTag(p.title.replace(' *', ''));

                            if (confirm(Ext.LabelKey.Admin_Frame_SaveAlert + ' ' + currTabTxt + '?')) {
                                isCloseOtherTab = true;
                                var saveOK = SaveHandle(p.id);
                                //undefined is saveOK!
                                if (saveOK == false) {
                                    isCloseOtherTab = false;
                                    return false;
                                }
                            } else {
                                RemoveLPSectionFields(Ext.Const.ModuleName);
                            }
                        }
                        //Call Jack method, clear data of object
                        //changeItems.RemoveChangedItemsByPageId(p.id);
                        ClearDataEntries();

                        var tabCount = tabs.items.length;
                        if (tabCount == 1) {
                            Ext.Const.ModuleName = '';

                            var eastPanel = Ext.getCmp('east-panel');
                            eastPanel.collapse();

                            var divField = document.getElementById('divField');
                            divField.innerHTML = '';

                            var toolsPanel = document.getElementById('toolsPanel');
                            toolsPanel.innerHTML = 'Tools';

                            var btnSave = Ext.getCmp("btnSave");
                            btnSave.disable();
                            isCloseOtherTab = false;

                        }
                        CloseWindow(p.id);

                        //for bug #42955, before close tab, clear the Iframe src.
                        //Because IE8 has some bug, the page timer still run when tab close.
                        var idf = p.id.split(Ext.Const.SplitChar);
                        var ifrId = 'IFR_' + idf[0];
                        document.getElementById(ifrId).src = '';
                        return true;
                    }
                }
            })
        ]
    });

    Ext.getCmp('east-panel').collapse();
});


function setCookie(name, value) {
    var argc = arguments.length;
    var argv = arguments;
    var path = (argc > 3) ? argv[3] : null;
    var domain = (argc > 4) ? argv[4] : null;
    var secure = (argc > 5) ? argv[5] : false;
    document.cookie = name + "=" + value + ((path == null) ? "" : ("; path=" + path)) + ((domain == null) ? "" : ("; domain=" + domain)) + ((secure == true) ? "; secure" : "");
}
