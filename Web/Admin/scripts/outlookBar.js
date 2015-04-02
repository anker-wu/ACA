/*
 * <pre>
 *  Accela Citizen Access
 *  File: outlookBar.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: outlookBar.js 77905 2007-10-15 12:49:28Z ACHIEVO\lytton.cheng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
*/

/*
    ||Feature:
    ||  Component of outlook 2003 styles
    ||Paraments:
    ||  title - Module name
    ||  index - Index value of title
    ||  item - Name of item panels in the module
    ||  height - Height of panels
    ||  object - Dom ojbect,it will load OutlookBar to there
    ||Example for:
    ||--------------------------------------------------------------
    ||  var myBar = new OutlookBar();
	||          myBar.addTitle(string title);
	||          myBar.addItem(int index,string item,string height);
	||  myBar.renderTo(string object);
    ||--------------------------------------------------------------
    ||Create By:
    ||  Kevin.Feng@achievo.com
    ||Create Date:
    ||  Apr 14,2008
*/



function OutlookBar()
{
    var THIS1 = this;
    this.titles = new Array();
    this.addTitle = function(name, moduleName) {

        function title(name, moduleName) {
            var THIS2 = this;
            this.name = name;
            this.moduleName = moduleName;
            this.items = new Array();
            this.addItem = function(name, height, treeJsonUrl) {
                function item() {
                    this.height = height;
                    this.name = name;
                    this.treeJsonUrl = treeJsonUrl;
                }

                var _item = new item(name, height, treeJsonUrl);
                THIS2.items[THIS2.items.length] = _item;
                return _item;
            };
        }

        var _title = new title(name, moduleName);
        this.titles[this.titles.length] = _title;
        return _title;
    };
    /*
    ||When clicking you can load current panel 
    */
    this.selectedIndex = 0;
    this.select = function(index) {
        THIS1.selectedIndex = index;
        for (var i = 0; i < this.titles.length; i++) {
            var tabT = document.getElementById('Item_' + i);
            tabT.style.display = 'none';

            var tabB = document.getElementById('Title_' + i);
            tabB.className = 'navTitle';
        }
        var currTab = document.getElementById('Item_' + index);
        currTab.style.display = 'block';
        Ext.Const.DisplayModuleName = THIS1.titles[index].name;

        var tabS = document.getElementById('Title_' + index);
        tabS.className = 'navOver';
    };

    this.mouseOver = function(index) {

        if (index != THIS1.selectedIndex) {
            var tabB = document.getElementById('Title_' + index);
            tabB.className = 'navOver';
        }
    };

    this.mouseOut = function(index) {
        if (index != THIS1.selectedIndex) {
            var tabIndex = document.getElementById('Title_' + index);
            tabIndex.className = 'navTitle';
        }
    };

    this._treeNodesObj;
    this.createTreeNodesObj = function(json) {
        this._treeNodesObj = eval(json);
    };

    this.build = function (container) {
        if (container != null) {
            /*
            ||Render each panel of module
            */

            function getItem(index, width, height, display, title, moduleTreeNodes) {
                var tabs = document.createElement("DIV");
                tabs.style.width = width;
                tabs.style.height = height;
                tabs.style.display = display;
                tabs.id = 'Item_' + index;
                if (title != null && typeof (title.items) != "undefined") {

                    var panelHeader = document.createElement("DIV");
                    panelHeader.className = "panelHeader";
                    tabs.appendChild(panelHeader);

                    var lblP = document.createElement("LABEL");
                    lblP.className = 'tabPanel';
                    lblP.style.whiteSpace = 'nowrap';
                    lblP.innerHTML = JsonDecode(title.name);

                    var childNodeBegin = document.createElement("A"); //section 508
                    childNodeBegin.id = "img_" + title.name;
                    childNodeBegin.href = "javascript:void(0);";
                    childNodeBegin.innerHTML = '<img class=\"ACA_NoBorder\" alt=\"Press tab key to focus child node.\" src=\"../Customize/images/spacer.gif\"/>';
                    panelHeader.appendChild(childNodeBegin);

                    panelHeader.appendChild(lblP);

                    for (var i = 0; i < title.items.length; i++) {
                        var tabHead = document.createElement("DIV");
                        tabHead.className = "tabHeader";

                        var calcHeight = 0;
                        if (i == 0) {
                            // The frame height of the General Settings
                            calcHeight = 75; 
                        }
                        if (i == 1 && i != title.items.length - 1) {
                            calcHeight = 30;
                        }

                        if (i == 1 && i == title.items.length - 1) {
                            calcHeight = (document.documentElement.clientHeight - menuBarHeight) - 215;
                        }
                        if (i == 2) {
                            calcHeight = (document.documentElement.clientHeight - menuBarHeight) - 273;
                        }

                        var tabBody = document.createElement("DIV");
                        tabBody.className = "tabBody";

                        if (calcHeight < 0) {
                            calcHeight = 20;
                        }
                        tabBody.id = 'Item_' + index + '_' + 'Tab_' + i;

                        var lblT = document.createElement("DIV");
                        tabBody.appendChild(lblT);

                        if (i == 2 || i == 1 && i == title.items.length - 1) {
                            var childNodeEnd = document.createElement("A"); //section 508
                            childNodeEnd.href = "#Title_" + index;
                            childNodeEnd.innerHTML = '<img class=\"ACA_NoBorder\" alt=\"It is the end of child tree. Press the Enter key to focus father node.\" src=\"../Customize/images/spacer.gif\"/>';
                            tabBody.appendChild(childNodeEnd);
                        }

                        var lbl = document.createElement("LABEL");
                        lbl.innerHTML = title.items[i].name;

                        tabHead.appendChild(lbl);
                        tabs.appendChild(tabHead);
                        tabs.appendChild(tabBody);

                        createSectionTree(title.items[i], moduleTreeNodes, calcHeight, lblT);
                    }
                }
                return tabs;
            }

            /*
            ||Layout
            */
            var containerObj = document.getElementById(container);

            // 508
            var hiddenLink = document.createElement("A");
            hiddenLink.href = "javascript:skipToAdminMainContent();";
            hiddenLink.innerHTML = '<img class=\"ACA_NoBorder\" alt="Skip to Navigation. Accesskey 1" src="../Customize/images/spacer.gif"/>';
            hiddenLink.accessKey = '1';
            hiddenLink.tabIndex = '1';

            var tabsContainer = document.createElement("DIV");
            tabsContainer.style.width = '100%';

            var navContainer = document.createElement("DIV");
            navContainer.style.width = '100%';
            navContainer.style.overflow = 'auto';
            navContainer.style.maxHeight = '180px'; // 8 * 23		    
            navContainer.style.bottom = '0px';

            var divContainer = document.createElement("DIV");
            divContainer.appendChild(hiddenLink);
            divContainer.appendChild(tabsContainer);
            divContainer.appendChild(navContainer);

            /*
            ||Render module button
            */
            for (var i = 0; i < this.titles.length; i++) {
                var navBtn = document.createElement("DIV");
                navBtn.className = 'navTitle';
                navBtn.tabIndex = '2'; // 508
                navBtn.id = 'Title_' + i;

                var lbl = document.createElement("LABEL");
                lbl.innerHTML = this.titles[i].name;

                navBtn.id = 'Title_' + i;
                navBtn.appendChild(lbl);

                navContainer.appendChild(navBtn);
            }

            containerObj.appendChild(divContainer);
            var menuBarHeight = divContainer.scrollHeight;
            if (Ext.isIE7) {
                menuBarHeight = menuBarHeight - 13;
            }

            /*
            || Add Pages and Settings infos.
            */
            for (var i = 0; i < this.titles.length; i++) {
                var moduleName = this.titles[i].moduleName;
                var moduleTreeNodes;
                for (var j = 0; j < THIS1._treeNodesObj.length; j++) {
                    if (THIS1._treeNodesObj[j].module == moduleName) {
                        moduleTreeNodes = THIS1._treeNodesObj[j];
                        break;
                    }
                }

                var tabX = getItem(i, "100%", "100%", "none", this.titles[i], moduleTreeNodes);
                tabsContainer.appendChild(tabX);
            }

            /*
            ||Event delegate
            */
            for (var i = 0; i < this.titles.length; i++) {
                var title = document.getElementById('Title_' + i);
                if (title) {

                    function delegate(index) {
                        try {
                            title.onkeypress = function () {
                                if (window.event.keyCode == 13) {
                                    THIS1.select(index);
                                    focusCtl("img_" + Ext.Const.DisplayModuleName);
                                }
                            }; // 508
                            title.onclick = function () { THIS1.select(index); };
                            title.onmouseover = function () { THIS1.mouseOver(index); };
                            title.onmouseout = function () { THIS1.mouseOut(index); };
                            title.oncontextmenu = function () { return false; };
                            title.nextSibling.oncontextmenu = function() { return false; };
                        } catch (e) {
                        }
                    }

                    delegate(i);
                }
            }
        }
    };
    
    /*
    ||Add item of panel
    */
    this.addItem = function(index, name, height, treeJsonUrl) {
        if (index >= 0 && index < this.titles.length) {
            return this.titles[index].addItem(name, height, treeJsonUrl);
        }
    };
    
    /*
    ||Render to ojbect
    */
    this.renderTo = function(id) {
        this.build(id);
        /*
        ||Init loaded panel
        */

        try {
            var defaultLoad = document.getElementById('Item_' + 0);
            defaultLoad.style.display = 'block';
            Ext.Const.DisplayModuleName = THIS1.titles[0].name;

            var tabDefault = document.getElementById('Title_' + 0);
            tabDefault.className = 'navOver';
        } catch(e) {

        }
    };
}

/*
||Focus focus control.
 */
function focusCtl(ctlName){
    var ctl = document.getElementById(ctlName);
    if (ctl){
        ctl.focus();
    }
}

/*
    ||Feature:
    ||  When click tree leaf,load module to center area.
    ||Paraments:
    ||  n - Current leaf notes object
    ||Create By:
    ||  Kevin.Feng@achievo.com
    ||Create Date:
    ||  Mar 18,2008
*/
Ext.LoadModule = function(n, e) {
    if (n.leaf == true) {
        with (n.attributes) {
            OpenTab(url, idft, text, moduleName, isUsedDaily);
        }
    }
};

/*
    ||Feature:
    ||  Settings tools accessibility
    ||Paraments:
    ||  n - Current leaf notes object
    ||Create By:
    ||  Kevin.Feng@achievo.com
    ||Create Date:
    ||  Apr 21,2008
*/
Ext.ToolsAccessibility = function(isUsedDaily) {
    var eastPanel = Ext.getCmp('east-panel');
    var divField = document.getElementById('divField');
    var toolsPanel = document.getElementById('toolsPanel');

    switch (isUsedDaily) {
    case 'Y':
        eastPanel.show();
        eastPanel.expand();
        break;
    case 'N':
        divField.innerHTML = '';
        toolsPanel.innerHTML = 'Tools';
        if (!Ext.isIE) {
            eastPanel.collapse();
        }
        eastPanel.hide();
        break;
    }

    //Related bug #37643
    if (Ext.isIE9 || Ext.isIE10) {
        Ext.EventManager.fireWindowResize();
    }
};

function OpenTab(pageUrl, tabId, tabName, module, isUsedDaily, pageFlow) {
    var idf = tabId.split(Ext.Const.SplitChar);
    var ifrId = 'IFR_' + idf[0];

    if (Ext.getCmp(tabId)) {
        Ext.getCmp("tabs").activate(tabId);
        Ext.ToolsAccessibility(isUsedDaily);
        /*
        ||Current opened page id
        */
        Ext.Const.OpenedId = tabId;

        if (pageUrl.indexOf("cap/capcompletion.aspx") != -1 || pageUrl.indexOf("pageflow/capreviewcertification.aspx") != -1) {
            // for CapCompletion.aspx or CapReviewCertification.aspx, need re-load page flow peroperty
            SwitchPageFlow(pageFlow);
        }

        focusCtl(ifrId);
        return;
    }
    if (!IsSesstionTimeout()) {
        var tabs = Ext.getCmp("tabs");
        /*
        ||Tab changed event
        */
        tabs.on('beforetabchange', function(self, newTab, currentTab) {
            // beforetabchange event excutes more than once when change tab.
            // Actually, it excutes as the number as the opened tabs.
            // Add this condition to ensure it's excuted only one time.
            if (newTab != null && newTab.id == Ext.Const.OpenedId) {
                return;
            }

            CloseWindow();

            var ids = newTab.id.split(Ext.Const.SplitChar);
            var isUsedDaily = ids[1];

            if (ids[3] == 'GlobalSettings' || ids[3] == 'FeatureSettings' || ids[3] == 'RegistrationSettings') {
                Ext.Const.ModuleName = '';
            }

            switch (ids[2]) {
                case 'General':
                    Ext.Const.ModuleName = '';
                    break;
                case 'Permit':
                    Ext.Const.ModuleName = 'Building';
                    break;
                case 'Service Request':
                    Ext.Const.ModuleName = 'ServiceRequest';
                    break;
                default:
                    Ext.Const.ModuleName = ids[2];
                    break;
            }

            Ext.Const.OpenedId = newTab.id;

            if (newTab.title.indexOf('*') > 0) {
                var btnSave = Ext.getCmp("btnSave");
                btnSave.enable();
            }
            else {
                var btnSave = Ext.getCmp("btnSave");
                btnSave.disable();
            }

            Ext.ToolsAccessibility(isUsedDaily);
            SwitchProperty(newTab);
        });

        var tabTitle, isFilter;
        isFilter = tabName.indexOf(idf[2]);
        if (isFilter >= 0) {

            tabTitle = tabName.replace('Module ', '').trim();
        }
        else {
            var tabHeader = Ext.Const.DisplayModuleName.replace('General', '');
            var tabBody = tabName.replace('Module ', '');
            if (tabBody.indexOf(tabHeader) >= 0) {
                tabHeader = '';
            }
            tabTitle = (tabHeader + ' ' + tabBody).trim();
        }

        if (pageFlow) {
            pageUrl += "&PageFlow=" + encodeURIComponent(JsonEncode(pageFlow));
        }
        
        // encode "'" and """
        var url = pageUrl.replace(/'/g, "&#39;").replace(/\u0022/g, "&quot;");

        tabs.add({
            id: tabId,
            title: tabTitle,
            html: String.format('<iframe src="{0}" id="{1}" width="100%" height="100%" frameborder="0" title="{2}" onload="OutsideAccessHandler(this);" ><p>If you can see this text, your browser does not support iframes. <a href="{3}">View the content of this inline frame</a> within your browser.</p></iframe>', url, ifrId, tabTitle, url),
            iconCls: 'tabs',
            closable: true
        }).show();

        Ext.Const.OpenedId = tabId;

        Ext.ToolsAccessibility(isUsedDaily);

        switch (module) {
            case 'General':
                Ext.Const.ModuleName = '';
                break;
            case 'Permit':
                Ext.Const.ModuleName = 'Building';
                break;
            case 'Service Request':
                Ext.Const.ModuleName = 'ServiceRequest';
                break;
            default:
                Ext.Const.ModuleName = module;
                break;
        }

        var divField = document.getElementById('divField');
        divField.innerHTML = '';
        focusCtl(ifrId);
        
        if (pageFlow || url.indexOf("cap/capcompletion.aspx") != -1) {
            // for CapCompletion.aspx, need load page flow peroperty drop-down
            LoadPageFlowProperty(pageFlow);
        }
    }
    else {
        if (pageFlow) {
            window.location.href = '../login.aspx?timeout=true';
        }
        else {
            RedirectToLoginPage();
        }
    }
}

function createSectionTree(titleItem, moduleTreeNodes, calcHeight, lblT) {
    var Tree = Ext.tree;
    var tree = new Tree.TreePanel({
        autoScroll: true,
        containerScroll: true,
        layout: 'fit',
        autoWidth: true,
        border: false,
        height: calcHeight,
        rootVisible: false,
        listeners: {
            click: function (n, e) {
                Ext.LoadModule(n, e);
            }
        }
    });

    var root = new Tree.TreeNode({
        text: 'Loading...',
        draggable: false,
        id: 'source'
    });

    var sectionTreeNodes;
    if (moduleTreeNodes != null && moduleTreeNodes.section != null) {
        for (var sIndex = 0; sIndex < moduleTreeNodes.section.length; sIndex++) {
            if (moduleTreeNodes.section[sIndex].root == getparmByUrl('root', titleItem.treeJsonUrl)) {
                sectionTreeNodes = moduleTreeNodes.section[sIndex];
                break;
            }
        }
    }

    // load nodes in a section
    if (sectionTreeNodes != null && sectionTreeNodes.nodes != null && sectionTreeNodes.nodes.length > 0) {
        for (var nodeIndex = 0; nodeIndex < sectionTreeNodes.nodes.length; nodeIndex++) {
            var cfgNode = sectionTreeNodes.nodes[nodeIndex];
            appendTreeNodes(root, cfgNode);
        }
    }

    tree.setRootNode(root);
    tree.render(lblT);
    tree.expandAll();
}

function appendTreeNodes(root, cfgNode){
    var parentNode = new Ext.tree.TreeNode({
        idft: cfgNode.idft,
        moduleName: cfgNode.moduleName,
        text: cfgNode.text,
        isUsedDaily: cfgNode.isUsedDaily,
        url: cfgNode.url,
        iconCls: cfgNode.iconCls,
        leaf: cfgNode.leaf
    });

    root.appendChild(parentNode);

    // have sub nodes
    if (!cfgNode.leaf && (cfgNode.children != undefined || cfgNode.children != null)) {
        for (var subIndex = 0; subIndex < cfgNode.children.length; subIndex++){
            appendTreeNodes(parentNode, cfgNode.children[subIndex]);
        }
    }
}