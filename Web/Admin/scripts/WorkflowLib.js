/**
* <pre>
* 
*  Accela Citizen Access
*  File: workFlowLib.js
* 
*  Accela, Inc.
*  Copyright (C): 2007-2014
* 
*  Description:
* 
*  Notes:
* $Id: workFlowLib.js 72643 2008-04-24 09:52:06Z $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* 04/24/2008     		daly.zeng				Initial.  
* </pre>
*/


Ext.form.Label = Ext.extend(Ext.BoxComponent, {
    initComponent: function () {
        Ext.form.Label.superclass.initComponent.call(this);
        if (this.cls == null) {
            this.addClass('x_label_text_default');
        }
    },
    onRender: function (ct, position) {
        if (!this.el) {
            this.el = document.createElement('label');
            this.el.innerHTML = this.text;
            if (this.forId) {
                this.el.setAttribute('htmlFor', this.forId);
            }
        }
        Ext.form.Label.superclass.onRender.call(this, ct, position);
    },
    setText: function (v) {
        this.el.update(v);
    }
});

Ext.reg('label', Ext.form.Label);

Ext.tree.CapPanel = Ext.extend(Ext.tree.TreePanel, {
    onRender: function (ct, position) {
        Ext.tree.CapPanel.superclass.onRender.call(this, ct, position);

        this.on('append', this.update, this);
        this.on('remove', this.update, this);
        this.on('beforemovenode', this.beforemovenode, this);
    },
    getNodes: function () {
        return this.root.childNodes;
    },
    getNodesCount: function () {
        return this.root.childNodes.length;
    },
    update: function () {
        this.updateHeader();
    },
    updateHeader: function () {
        var aa = this.root.childNodes;
        var bb = this.root.childNodes.length;
        var reg = /\d+/;
        this.title = this.title.replace(reg, this.getNodesCount());
        this.header.child('span').update(this.title);

        this.fireEvent('titlechange', this, this.title);
    },
    createNode: function (attr) {
        return (attr.leaf ?
                        new Ext.tree.TreeNode(attr) :
                        new Ext.tree.AsyncTreeNode(attr));
    },
    createTree: function (response, callback) {
        this.isLoaded = false;

        this.clear();

        var o = response;
        var node = this.root;

        try {
            node.beginUpdate();
            for (var i = 0, len = o.length; i < len; i++) {
                if (o[i] == null) {
                    break;
                }

                var n = this.createNode(o[i]);
                if (n) {
                    node.appendChild(n);
                }
            }
            node.endUpdate();
            if (typeof callback == "function") {
                callback(this, node);
            }

            this.isLoaded = true;
            this.setNodeWidth(this.id);

        } catch (e) {
            // do nothing
        }
    },
    clear: function () {
        while (this.root.firstChild) {
            this.root.removeChild(this.root.firstChild);
        }
    },
    beforemovenode: function () {
        return false;
    },
    setNodeWidth: function (treeId) {
        var tree = document.getElementById(treeId);
        if (tree) {
            // tree.childNodes[0] is header.
            var outerDiv = tree.childNodes[1].firstChild;
            var outerUL = outerDiv.firstChild;

            if (outerUL && outerDiv.scrollWidth > 0) {
                // set the tree node's width as the tree's scroll width.
                outerUL.style.width = outerDiv.scrollWidth + "px";
            }
        }
    }
});

Ext.reg('CapPanel', Ext.tree.CapPanel);

Ext.link = Ext.extend(Ext.BoxComponent, {
    clickEvent: 'click',
    toggled: false,
    initComponent: function () {
        Ext.link.superclass.initComponent.call(this);

        this.addEvents("click");
    },
    onRender: function (ct, position) {
        if (!this.el) {
            this.el = document.createElement('A');
            this.el.innerHTML = this.text;
            this.el.href = "javascript:void(0);";
        }
        Ext.link.superclass.onRender.call(this, ct, position);
        this.el.addClass('ACA_UnderLine');
        this.addEvents('click');
        this.el.on(this.clickEvent, this.onClick, this);
    },
    onClick: function (e) {
        if (e) {
            e.preventDefault();
        }
        if (!this.disabled) {
            if (this.toggled) {
                if (this.alternateHandler) {
                    this.alternateHandler.call(this.scope || this, this, e);

                    this.displayShowLink()
                }
            }
            else {
                if (this.handler) {
                    this.handler.call(this.scope || this, this, e);

                    this.displayHideLink();
                }
            }
        }
    },
    setHandler: function (handler, scope) {
        this.handler = handler;
        this.scope = scope;
    },
    setAlternateHandler: function (handler, scope) {
        this.alternateHandler = handler;
        this.scope = scope;
    },
    setAlternateText: function (v) {
        this.alternateText = v;
    },
    displayHideLink: function () {
        this.toggled = true;
        this.text = Ext.LabelKey.Admin_Pageflow_lnkHideDetails;
        this.el.update(this.text);
    },
    displayShowLink: function () {
        this.toggled = false;
        this.text = Ext.LabelKey.Admin_Pageflow_lnkShowDetails;
        this.el.update(this.text);
    }
});

Ext.reg('link', Ext.link);

function StringBuffer() {
    this.data = [];
};

StringBuffer.prototype.append = function () {
    this.data.push(arguments[0]);
    return this;
};

StringBuffer.prototype.toString = function () {
    return this.data.join("");
};

String.prototype.format = function () {
    var str = this;
    var matchs = str.match(/\{\d\}/ig);
    for (var i = 0; i < arguments.length; i++) {
        str = str.replace(eval('/\\{' + i + '\\}/ig'), arguments[i]);
    }
    return str;
};

//==global=====:function(){}

/*************************************************************************/
//global var
Ext.workflowWidth = 730;
Ext.workflowMaxWidth = 820;
Ext.workflowFixWidth = 0;


Ext.WestPanelWidth = 0; //208
Ext.EastPanelWidth = 180;
Ext.ScreenMaxWidth = 1024;
Ext.WorkFlowWidth = 730;
Ext.IsResize = false;


Ext.loadStep = true;
Ext.cmp = null; //current component,use in right click delete component	
Ext.propertyOwnerId = null; //when click step,page,save the id so with property(title) change,reflesh the title
Ext.pagePropertyList = new Array();
Ext.stepPropertyList = new Array();

Ext.isFirstLoad = true;
Ext.isFirstCreatePage = true;
Ext.isFirstExpend = true;
Ext.FirstPropertyTabWidth = 0;
Ext.FirstPropertyTabWidth_Correct = 0;
Ext.isFirstExpand = true;
Ext.curStepId = null;
Ext.curPageId = null;
Ext.curComponentId = null;
Ext.oldStepId = null;
Ext.oldPageId = null;
Ext.oldComponentId = null;

Ext.isChanged = false;
Ext.isFirstLoadStandardChoice = true;
Ext.curComponent = null;
Ext.FirstStep = null;
Ext.canSave = true;
Ext.Applicant_FirstValue = 'select';
Ext.IsSupportMultiLanguage = false;
Ext.isCreatePageflow = true;
Ext.ASIGroupEmptyText = Ext.LabelKey.DropDownDefaultText;
Ext.RecordTypeEmptyText = Ext.LabelKey.DropDownDefaultText;

var COMPONENT_TAB_ID = "tabs1"; //the component tab in right
var PAGE_SHOW_IN_STEP = 3;
var MAX_STEP_NUMBER = 6;
var PAGE_HTML = '<div><center><p>&nbsp;&nbsp;Drag and drop component here</p></center></div>';
var Component_Property_Tab_Id;

var ContactTypeOptions = 'Contact Type Options';
var DocumentTypeOptions = 'Document Type Options';

Ext.ux.WorkPanel = Ext.extend(Ext.Panel, {
    layout: 'column'
});

Ext.reg('WorkPanel', Ext.ux.WorkPanel);

//******************************************************************
//==StepPanel=====:function(){}
Ext.ux.StepPanel = Ext.extend(Ext.Panel, {
    cls: 'x-StepPanel',
    layout: 'column',
    columnWidth: .99,  //100%
    autoScroll: false,
    title: 'StepPanel title',
    defaultTitle: '',
    defaultType: 'panel',
    firstPageIndex: 0,
    stepOrder: 0,

    tools: [
	    { id: 'up',
	        handler: function (e, target, panel) {
	            var pane = Ext.getCmp("pnlPageflow");
	            panel.setStepActive();
	            pane.setPanelActive();
	            pane.changeStepPosion(e, 0, null);
	        }
	    },
        { id: 'down',
            handler: function (e, target, panel) {
                var pane = Ext.getCmp("pnlPageflow");
                panel.setStepActive();
                pane.setPanelActive();
                pane.changeStepPosion(e, 1, null);
            }
        },
		{ id: 'close',
		    handler: function (e, target, panel) {
		        var pane = Ext.getCmp("pnlPageflow");
		        var workpanel = panel.ownerCt;
		        if (workpanel.items.length == 1) return;

		        Ext.Msg.confirm('Confirm', 'Are you sure you want to remove this step and all its components?',
  					function (btn) {
  					    if (btn == 'yes') {
  					        panel.deleteStepPanel();
  					    }
  					});
		    } //handler
		}
	],

    listeners: {
        'render': function (obj) {
            obj.el.on('click', function (e, target, panel) {
                obj.setStepActive();

                if (((target.className.indexOf("x-panel-header") > -1 && target.parentNode.className.indexOf("x-StepPanel") > -1)) ||
 				 		((target.parentNode.parentNode.className.indexOf("x-StepPanel") > -1) && target.tagName == "SPAN")) {
                    // save old property value before click
                    var tab = Ext.getCmp(COMPONENT_TAB_ID);
                    tab.activate(tab.getComponent(1).id);
                    if (!Ext.stepPropertyList[obj.id]) {
                        if (Ext.IsSupportMultiLanguage) {
                            Ext.stepPropertyList[obj.id] = { " Step Name(Default Language)": obj.defaultTitle, "Step Name": obj.title };
                        } else {
                            Ext.stepPropertyList[obj.id] = { "Step Name": obj.title };
                        }

                    }
                    tab.getComponent(1).setSource(Ext.stepPropertyList[obj.id]);

                    Ext.propertyOwnerId = obj.id;
                    tab.doLayout();
                    Ext.getCmp('propertyPanel').getLayout().container.expand();
                } else {
                    if (target.parentNode.className.indexOf("x-StepPanel") > -1) {
                        var tab = Ext.getCmp(COMPONENT_TAB_ID);
                        tab.activate(tab.getComponent(0).id);
                    }
                }

                RenderContactTypeGenericTemplate("");
            });
            obj.el.on('resize', function () {//for ie
                obj.doLayout();
            });
            obj.el.on('mouseover', function (sour, target, panel) {
                if (target.className.indexOf("x-panel-header") > -1 && target.parentNode.className.indexOf("x-StepPanel") > -1) {
                    propertyTabClick();
                }
            });

        }
    },
    setStepActive: function (curId) {
        if (curId) {
            if (Ext.oldStepId == curId) {
                Ext.oldStepId = null;
            }
        } else {
            if (this.id == Ext.curStepId) {
                return;
            }

            Ext.oldStepId = Ext.curStepId;
            Ext.curStepId = this.id;
        }

        var pane = Ext.getCmp("pnlPageflow");
        var chk = false;
        var tmpPageId = null;
        var _index = 0;
        if (!Ext.curPageId) {
            for (var i = 0; i < this.items.length; i++) {
                if (this.items.items[i].getXType() != "PageContainer") {
                    continue;
                }

                if (this.firstPageIndex == _index) {
                    Ext.curPageId = this.items.items[i].items.items[0].id;
                }
                _index++;
            }
        } else {
            for (var i = 0; i < this.items.length; i++) {
                if (this.items.items[i].getXType() != "PageContainer") {
                    continue;
                }

                if (this.items.items[i].items.items[0].id == Ext.curPageId) {
                    chk = true;
                    break;
                }
                if (this.firstPageIndex == _index) {
                    tmpPageId = this.items.items[i].items.items[0].id;
                }
                _index++;
            }
            if (!chk) {
                pane.deletePageCloseBar();
                Ext.oldPageId = Ext.curPageId;
                Ext.curPageId = tmpPageId;

            }
        }
        pane.setPanelActive();
    },

    onRender: function (ct, position) {
        Ext.ux.StepPanel.superclass.onRender.call(this, ct, position);
        this.insert(0, new Ext.ux.ArrowTemplate('<div id="page-move-next|' + this.id + '" style="display:block"><div style="height:70px;width:10px"></div><div id="page-move-next-btn" class="x-arrowNext"></div><div style="height:10px;width:10px"></div></div>'));
        this.insert(0, new Ext.ux.ArrowTemplate('<div id="page-move-prev|' + this.id + '" style="display:block"><div style="height:70px;width:10px"></div><div id="page-move-prev-btn" class="x-arrowPrev"></div><div style="height:10px;width:10px"></div></div>'));
    },

    initComponent: function () {
        Ext.ux.StepPanel.superclass.initComponent.call(this);
        this.addEvents({
            validatedrop: true,
            beforedragover: true,
            dragover: true,
            beforedrop: true,
            drop: true
        });
    },

    initEvents: function () {
        Ext.ux.StepPanel.superclass.initEvents.call(this);
    },

    setArrowVisible: function () {
        var btnNext = this.items.items[this.items.items.length - 1];
        var btnPrev = this.items.items[0];
        if (this.items.length <= PAGE_SHOW_IN_STEP + 2) {
            btnPrev.setVisible(false);
            btnNext.setVisible(false);

            this.body.addClass("x-StepPanel-LeftSpace");
        } else {
            if (this.firstPageIndex + PAGE_SHOW_IN_STEP + 2 == this.items.length) {
                btnPrev.setVisible(true);
                btnNext.setVisible(false);

                this.body.removeClass("x-StepPanel-LeftSpace");
            } else if (this.firstPageIndex == 0) {
                btnPrev.setVisible(false);
                btnNext.setVisible(true);

                this.body.addClass("x-StepPanel-LeftSpace");
            } else {
                btnNext.setVisible(true);
                btnPrev.setVisible(true);

                this.body.removeClass("x-StepPanel-LeftSpace");
            }
        }
    },

    deleteStepPanel: function (isLoad) {
        var pane = Ext.getCmp("pnlPageflow");
        Ext.stepPropertyList[this.id] = { "Step Name": '' };
        if (Ext.propertyOwnerId == this.id) {
            Ext.propertyOwnerId = null;
        }
        if (this.items) {
            for (var i = this.items.length - 1; i >= 0; i--) {
                if (!this.items.items[i].items) {
                    continue;
                }

                if (this.items.items[i].getXType() != "PageContainer") {
                    continue;
                }

                this.items.items[i].items.items[0].deletePagePanel(isLoad);
            }
        }
        var tab = Ext.getCmp(COMPONENT_TAB_ID);
        tab.getComponent(1).setSource(null);
        tab.activate(Component_Property_Tab_Id);

        if (isLoad) {
            this.ownerCt.remove(this, true);
        } else {
            if (Ext.oldStepId == this.id) {
                Ext.oldStepId = null;
            }

            if (Ext.curStepId == this.id) {
                Ext.curStepId = null;
            }

            this.ownerCt.remove(this, true);
            var pane = Ext.getCmp("pnlPageflow");
            if (Ext.curStepId == null) {
                Ext.curStepId = pane.pane.items.items[0].id;
                pane.pane.items.items[0].setStepActive(true);
            }

            pane.setStepToolsStatus();
        }
    },

    setPageToolsStatus: function (isDelete) {
        if (!this.items) {
            return;
        }
        var page;
        if (!isDelete && this.items.length == 3) {//add page
            page = this.items.items[1].items.items[0];
            page.tools.close.setVisible(false);
            //if old page has close bar,set false
            if (Ext.oldPageId) {
                var oldPage = Ext.getCmp(Ext.oldPageId);
                oldPage.tools.close.setVisible(false);
            }
        } else if (this.items.length == 4 && isDelete) {//delete page
            for (var i = 0; i < this.items.length; i++) {
                if (this.items.items[i].getXType() != "PageContainer") {
                    continue;
                }

                page = this.items.items[i].items.items[0];
                page.tools.close.setVisible(false);
            }
        } else {
            for (var i = 0; i < this.items.length; i++) {
                if (this.items.items[i].getXType() != "PageContainer") {
                    continue;
                }

                page = this.items.items[i].items.items[0];
                if (Ext.curPageId == page.id) {
                    page.tools.close.setVisible(true);
                } else {
                    page.tools.close.setVisible(false);
                }
            }
        }
    },

    setFirstPageIndex: function () {
        if (this.items.length <= PAGE_SHOW_IN_STEP + 2) {
            this.firstPageIndex = 0;
        }
    },
    pageMove: function (moveType) {

        var pane = Ext.getCmp("pnlPageflow");
        var curActiveIndex = 0;
        var firstPageId;
        var lastPageId;
        var _index = 0;
        if (moveType == "next") {
            if (this.firstPageIndex < this.items.length - 2 - PAGE_SHOW_IN_STEP) {
                this.firstPageIndex++;
            }
        } else {//prev
            if (this.firstPageIndex > 0) {
                this.firstPageIndex--;
            }
        }
        var pagesLen = this.items.length;
        for (var i = 0; i < pagesLen; i++) {
            var container = this.items.items[i];
            if (container.getXType() != "PageContainer") {
                continue;
            }

            if (container.items.items[0].id == Ext.curPageId) {
                curActiveIndex = _index;
            }
            if (_index == this.firstPageIndex) {
                firstPageId = container.items.items[0].id;
            }
            if (moveType == "prev") {
                if (_index == (this.firstPageIndex + PAGE_SHOW_IN_STEP - 1)) {
                    lastPageId = container.items.items[0].id;
                }
            } else {
                if (_index == this.firstPageIndex + PAGE_SHOW_IN_STEP) {
                    lastPageId = container.items.items[0].id;
                }
            }
            _index++;
            if (i > this.firstPageIndex && i <= this.firstPageIndex + PAGE_SHOW_IN_STEP) {
                container.setVisible(true);
                var page = container.items.items[0];
                //if page's width is 0,that means it's a new created page ,no need to fix
                if (page.getSize().width < 190 && page.getSize().width > 0) {
                    page.setWidth(190);
                    page.doLayout();
                    page.syncSize();
                }
            } else {
                container.setVisible(false);
            }
        }
        //change active page
        var isActive = false;
        if (moveType == "next") {
            if (curActiveIndex < this.firstPageIndex) {
                Ext.oldPageId = Ext.curPageId;
                Ext.curPageId = firstPageId;
            }
        } else {//prev
            if (curActiveIndex >= this.firstPageIndex + PAGE_SHOW_IN_STEP) {
                Ext.oldPageId = Ext.curPageId;
                Ext.curPageId = lastPageId;
            }
        }
        if (isActive) {
            pane.setPanelActive();
            this.doLayout();
        }
        this.setArrowVisible();
    }
});

Ext.reg('StepPanel', Ext.ux.StepPanel);
//*******************************************************************
//==PageHTML=====:function(){}
Ext.ux.PageHTML = Ext.extend(Ext.Panel, {
    cls: 'x-PagePanel-Html',
    autoScroll: false,
    draggable: false,
    html: PAGE_HTML

});

Ext.reg('PageHTML', Ext.ux.PageHTML);

//*******************************************************************
//==PageContainer=====:function(){}
Ext.ux.PageContainer = Ext.extend(Ext.Panel, {
    cls: 'x-PageContainer',
    autoScroll: false,
    draggable: false

});

Ext.reg('PageContainer', Ext.ux.PageContainer);

//**********************************************************************
//==PagePanel=====:function(){}
Ext.ux.PagePanel = Ext.extend(Ext.Panel, {
    cls: 'x-PagePanel',
    autoScroll: true,
    title: 'PagePanel title',
    defaultTitle: '',
    onloadEventName: DEFALUT_EVENT_NAME,
    beforeClickEventName: DEFALUT_EVENT_NAME,
    afterClickEventName: DEFALUT_EVENT_NAME,
    defaultType: 'panel',
    draggable: true,
    lastPost: 0,
    AcaMovePos: null,
    tools: [{
        id: 'close',
        handler: function (e, target, panel) {
            var step = panel.ownerCt.ownerCt;
            if (step.items.length == 3) return;

            Ext.Msg.confirm('Confirm', 'Are you sure you want to remove this page and all its components?',
			    function (btn) {
			        if (btn == 'yes') {
			            panel.deletePagePanel();
			        }
			    });
        } //handler
    }],
    onRender: function (ct, position) {
        Ext.ux.PagePanel.superclass.onRender.call(this, ct, position);
    },

    listeners: {
        'render': function (obj) {
            if (!Ext.pagePropertyList[obj.id]) {
                var pageProp = new pageProperty();
                pageProp.defalutPageName = obj.defaultTitle;
                pageProp.pageName = obj.title;
                pageProp.onloadEvent = obj.onloadEventName;
                pageProp.beforeButtonEvent = obj.beforeClickEventName;
                pageProp.afterButtonEvent = obj.afterClickEventName;
                pageProp.defaultLangInstruction = obj.defaultLangInstruction;
                pageProp.instruction = obj.instruction;
                Ext.pagePropertyList[obj.id] = pageProp.setProperty(Ext.IsSupportMultiLanguage);
            }

            obj.el.on('click', function (e, target, panel) {
                Ext.getCmp('propertyPanel').getLayout().container.expand();
                obj.setPageActive();
                //click title
                if ((target.className.indexOf("x-panel-header") > -1 && target.parentNode.className.indexOf("x-PagePanel") > -1) ||
	 			((target.parentNode.parentNode.className.indexOf("x-PagePanel") > -1) && target.tagName == "SPAN")) {

                    var tab = Ext.getCmp(COMPONENT_TAB_ID);
                    var propTab = tab.getComponent(1);

                    tab.activate(propTab.id);

                    //disable sorting
                    tab.getComponent(1).store.sortInfo = null;
                    tab.getComponent(1).getColumnModel().setConfig([
                { header: 'Names', width: 80, sortable: false, dataIndex: 'name', id: 'name', allowBlank: true }
               , { header: 'Value', width: 120, sortable: false, dataIndex: 'value', id: 'value', allowBlank: true }
                ]);
                    tab.getComponent(1).setSource(Ext.pagePropertyList[obj.id]);
                    var cells = tab.getComponent(1).el.dom.getElementsByTagName('td');
                    if (cells) {
                        var instructionIndex = cells.length - 1;
                        cells[instructionIndex].className += ' watermark';
                        cells[instructionIndex].childNodes[0].innerHTML = Ext.LabelKey.Admin_ClickToEdit_Watermark;
                    }

                    Ext.curComponent = obj;
                    Ext.propertyOwnerId = obj.id;
                } else {
                    if (target.className.indexOf("x-panel-header") == -1) {
                        var tab = Ext.getCmp(COMPONENT_TAB_ID);
                        tab.activate(tab.getComponent(0).id);

                        tab.doLayout();
                    }
                }

                RenderContactTypeGenericTemplate("");
            });
            obj.el.on('mouseover', function (sour, target, panel) {
                if (target.className.indexOf("x-panel-header") > -1 && target.parentNode.className.indexOf("x-PagePanel") > -1) {
                    obj.blur();
                    obj.focus();
                }
            });
        }
    },

    setPageActive: function () {
        if (this.id == Ext.curPageId) {
            return;
        }

        Ext.oldPageId = Ext.curPageId;
        Ext.curPageId = this.id;
        var pane = Ext.getCmp("pnlPageflow");
        pane.setPanelActive();

    },
    resetComponentWidth: function () {
        if (this.items.length == 1 && this.items.items[0].getXType() != "ComponentPanel") {
            this.doLayout();
            return;
        }
        var _width = normalwidth;
        var chk = false;
        for (var i = 0; i < this.items.length; i++) {
            if (this.items.items[i].maxWidth > _width) {
                _width = this.items.items[i].maxWidth;
            }
            chk = true;
        }
        if (!chk) {
            return;
        }
        if (this.items.length < 5) {
            _width += 18;
        }
        _width += "px";
        for (var i = 0; i < this.items.length; i++) {
            this.items.items[i].el.applyStyles({ width: _width });
        }
    },
    deleteComponent: function (isLoad) {
        var componentTab = Ext.getCmp(Component_Property_Tab_Id);
        var tab = Ext.getCmp(COMPONENT_TAB_ID);
        var check = false;
        for (var j = this.items.length - 1; j >= 0; j--) {
            var pos = parseInt(this.items.items[j].sortID);
            var chk = false;
            for (var i = 0; i < componentTab.items.length; i++) {
                var _sortId = parseInt(componentTab.items.items[i].sortID)
                if (pos < _sortId) {
                    pos = i;
                    chk = true;
                    break;
                }
            }
            if (!chk) {
                if (pos >= componentTab.items.length) {
                    pos = componentTab.items.length;
                }
            }

            var panelItem = this.items.items[j];
            panelItem.el.applyStyles({ width: '' });
            if (panelItem.getXType() == "ComponentPanel") {
                if (Ext.oldComponentId == panelItem.id) {
                    Ext.oldComponentId = null;
                }
                if (Ext.curComponentId == panelItem.id) {
                    Ext.curComponentId = null;
                }
                initComponentSource(panelItem, tab, true);

                // ASI/ASIT NEED NOT insert into component tab
                if (!panelItem.isSupportMultiply) {
                    componentTab.insert(pos, panelItem);
                }
            }
        }

        componentTab.doLayout();
        componentTab.syncSize();
        changeSaveStatus(isLoad);
        var tab = Ext.getCmp(COMPONENT_TAB_ID);
        tab.getComponent(1).setSource(null);
    },

    deletePagePanel: function (isLoad) {
        var pane = Ext.getCmp("pnlPageflow");
        if (this.items) {
            this.deleteComponent(isLoad);
        }
        var step = this.ownerCt.ownerCt;
        this.ownerCt.ownerCt.setPageToolsStatus(true);
        if (Ext.oldPageId == this.id) {
            Ext.oldPageId = null;
        }
        if (Ext.curPageId == this.id) {
            Ext.curPageId = null;
        }
        var step = this.ownerCt.ownerCt;

        var tab = Ext.getCmp(COMPONENT_TAB_ID);
        tab.activate(tab.getComponent(1).id);
        var pageProp = new pageProperty();
        Ext.pagePropertyList[this.id] = pageProp.initProperty(Ext.IsSupportMultiLanguage);

        tab.getComponent(1).setSource(null);
        if (Ext.propertyOwnerId == this.id) {
            Ext.propertyOwnerId = null;
        }

        step.remove(this.ownerCt, true);

        if (step.items.length >= PAGE_SHOW_IN_STEP + 2) {
            if (step.items.length - 2 - step.firstPageIndex < PAGE_SHOW_IN_STEP) {//right
                step.firstPageIndex--;
            }
        }
        for (var i = 0; i < step.items.length; i++) {
            var container = step.items.items[i];
            if (container.getXType() != "PageContainer") {
                continue;
            }
            if (i > step.firstPageIndex && i <= step.firstPageIndex + PAGE_SHOW_IN_STEP) {
                container.setVisible(true);
            } else {
                container.setVisible(false);
            }
        }

        var chk = false;
        var _index = 0;
        for (var i = 0; i < step.items.length; i++) {
            if (step.items.items[i].getXType() != "PageContainer") {
                continue;
            }
            if (step.firstPageIndex == _index) {
                Ext.curPageId = step.items.items[i].items.items[0].id;
            }
            _index++;
        }

        step.setArrowVisible();
        pane.setPanelActive();
        changeSaveStatus(isLoad);
        step.doLayout();
        checkTimer();
    },

    checkPageHtml: function (isAddHtml) {
        //if no component here ,set html, else clear
        if (isAddHtml && this.items.length == 1) {
            var tmp = new Ext.ux.PageHTML();
            this.add(tmp);
            this.doLayout();
            return;
        }
        var isExist = false;
        for (var i = 0; i < this.items.length; i++) {
            if (this.items.items[i].getXType() == "ComponentPanel") {
                isExist = true;
                break;
            }
        }
        if (isExist) {
            for (var i = 0; i < this.items.length; i++) {
                if (this.items.items[i].getXType() == "PageHTML") {
                    this.remove(this.items.items[i], true);
                    break;
                }
            }
        }
    },

    initComponent: function () {
        Ext.ux.PagePanel.superclass.initComponent.call(this);

        this.addEvents({
            validatedrop: true,
            beforedragover: true,
            dragover: true,
            beforedrop: true,
            drop: true
        });
    },

    initEvents: function () {
        Ext.ux.PagePanel.superclass.initEvents.call(this);
        this.dd = new Ext.ux.PagePanel.DropZone(this, this.dropConfig);
    }
});

Ext.reg('PagePanel', Ext.ux.PagePanel);

Ext.ux.PagePanel.DropZone = function (PagePanel, cfg) {
    this.PagePanel = PagePanel;
    Ext.dd.ScrollManager.register(PagePanel.body);
    Ext.ux.PagePanel.DropZone.superclass.constructor.call(this, PagePanel.id, cfg);
    PagePanel.body.ddScrollConfig = this.ddScrollConfig;
};

Ext.extend(Ext.ux.PagePanel.DropZone, Ext.dd.DropTarget, {
    ddScrollConfig: {
        vthresh: 50,
        hthresh: -1,
        animate: true,
        increment: 200
    },

    createEvent: function (dd, e, data, col, c, pos) {
        return {
            PagePanel: this.PagePanel,
            panel: data.panel,
            columnIndex: col,
            column: c,
            position: pos,
            data: data,
            source: dd,
            rawEvent: e,
            status: this.dropAllowed
        };
    },

    notifyOut: function () {
        delete this.grid;
    },

    notifyOver: function (dd, e, data) {
        var xy = e.getXY(), PagePanel = this.PagePanel, px = dd.proxy;
        this.PagePanel.AcaMovePos = xy;
        if (dd.panel.getXType() == "ComponentPanel") {//Component    0
            //*************************************************************************
            if (dd.panel.ownerCt.id == Component_Property_Tab_Id) {// drag component to page  1
                //------------------------------------------------------------------
                // case column widths
                if (!this.grid) {
                    this.grid = this.getGrid();
                }

                // handle case scroll where scrollbars appear during drag
                var cw = PagePanel.body.dom.clientWidth;
                if (!this.lastCW) {
                    this.lastCW = cw;
                } else if (this.lastCW != cw) {
                    this.lastCW = cw;
                    PagePanel.doLayout();

                    this.grid = this.getGrid();
                }
                var _pos = 0;
                // determine column
                var col = 0, xs = this.grid.columnX, cmatch = false;
                for (var len = xs.length; col < len; col++) {
                    if (xy[0] < (xs[col].x + xs[col].w)) {
                        cmatch = true;
                        break;
                    }
                }
                // no match, fix last index
                if (!cmatch) {
                    col--;
                }

                // find insert position
                var p, match = false, pos = 0, c = this.PagePanel;
                var itemLen = 0;
                if (this.PagePanel.items) {
                    itemLen = this.PagePanel.items.length;
                }

                for (var len = itemLen; pos < len; pos++) {
                    p = this.PagePanel.items.items[pos];
                    var h = p.el.getHeight();
                    if (h !== 0 && (p.el.getY() + (h / 2)) > xy[1]) {
                        match = true;
                        break;
                    }
                }

                //Use the spacer(target container) to position.
                var spacerTop = px.getProxy().getTop();

                for (var i = xs.length - 1; i >= 0; i--) {
                    if (spacerTop > xs[i].w) {
                        _pos = i + 1;
                        break;
                    }
                }

                var overEvent = this.createEvent(dd, e, data, _pos, this.PagePanel, match && p ? pos : itemLen);

                if (PagePanel.fireEvent('validatedrop', overEvent) !== false && PagePanel.fireEvent('beforedragover', overEvent) !== false) {

                    // make sure proxy width is fluid
                    if (px.getProxy() != undefined) {
                        px.getProxy().setWidth('auto');
                    }

                    if (p) {
                        px.moveProxy(p.el.dom.parentNode, match ? p.el.dom : null);
                    } else {
                        px.moveProxy(c.el.dom, null);
                    }

                    this.lastPos = { c: c, _pos: _pos, p: match && p ? pos : false };
                    this.scrollPos = PagePanel.body.getScroll();

                    PagePanel.fireEvent('dragover', overEvent);

                    return overEvent.status;
                } else {
                    return overEvent.status;
                }
                //------------------------------------------------------------------    
            } else if (dd.panel.ownerCt.id == this.dragElId) {//drag component  inner page
                //------------------------------------------------------------------
                // case column widths
                if (!this.grid) {
                    this.grid = this.getGrid();
                }

                // handle case scroll where scrollbars appear during drag
                var cw = PagePanel.body.dom.clientWidth;
                if (!this.lastCW) {
                    this.lastCW = cw;
                } else if (this.lastCW != cw) {
                    this.lastCW = cw;
                    PagePanel.doLayout();
                    this.grid = this.getGrid();
                }

                // determine column
                var col = 0, xs = this.grid.columnX, cmatch = false;
                for (var len = xs.length; col < len; col++) {
                    if (xy[0] < (xs[col].x + xs[col].w)) {
                        cmatch = true;
                        break;
                    }
                }
                // no match, fix last index
                if (!cmatch) {
                    col--;
                }

                for (var i = xs.length - 1; i >= 0; i--) {
                    if (xy[1] > xs[i].w) {
                        _pos = i;
                        PagePanel.lastPost = i;
                        break;
                    }
                }
                // find insert position
                var p, match = false, pos = 0,
                c = PagePanel,
                items = c.items.items;

                for (var len = items.length; pos < len; pos++) {
                    p = items[pos];
                    var h = p.el.getHeight();
                    if (h !== 0 && (p.el.getY() + (h / 2)) > xy[1]) {
                        match = true;
                        break;
                    }
                }

                var overEvent = this.createEvent(dd, e, data, col, c, match && p ? pos : c.items.getCount());

                if (PagePanel.fireEvent('validatedrop', overEvent) !== false && PagePanel.fireEvent('beforedragover', overEvent) !== false) {

                    // make sure proxy width is fluid
                    px.getProxy().setWidth('auto');

                    if (p) {
                        px.moveProxy(p.el.dom.parentNode, match ? p.el.dom : null);
                    } else {
                        px.moveProxy(c.el.dom, null);
                    }

                    this.lastPos = { c: c, col: col, p: match && p ? pos : false };
                    this.scrollPos = PagePanel.body.getScroll();

                    PagePanel.fireEvent('dragover', overEvent);

                    return overEvent.status; ;
                } else {
                    return overEvent.status;
                }
                //------------------------------------------------------------------
            } else if (dd.panel.ownerCt.ownerCt.id != Ext.getCmp(this.dragElId).ownerCt.id && this.PagePanel.ownerCt.ownerCt.id == dd.panel.ownerCt.ownerCt.ownerCt.id) {//drag component to another page but in one step

            } else {//drage component to another step

            }
            //************************************************************************* 
        } else {//other event  0
            //todo
        }
    },

    notifyDrop: function (dd, e, data) {
        if (dd.panel.getXType() == "ComponentPanel") {
            // Component drag to page
            var pos, sour;
            var com;

            sour = Ext.getCmp(dd.id);
            pos = dd.panel.ownerCt.AcaMovePos;
            var target = this.PagePanel;
            if (dd.panel.ownerCt.id == Component_Property_Tab_Id) {
                // dd from a component propertites panel.
                delete this.grid;
                if (!this.lastPos) {
                    return;
                }
                var c = this.lastPos.c, _pos = this.lastPos._pos, pos = this.lastPos.p;
                dd.panel.componentSeqNbr = 0;
                var pane = Ext.getCmp("pnlPageflow");
                if (dd.panel.titleA == "Contact1" || dd.panel.titleA == "Contact2" || dd.panel.titleA == "Contact3" || dd.panel.titleA == "Applicant") {
                    var __tab = Ext.getCmp(COMPONENT_TAB_ID);
                    initComponentSource(dd.panel, __tab, false);
                }

                if (dd.panel.isSupportMultiply) {
                    // Because support multiple components, add new ComponentPanel, not refer.
                    // If use refer, the component panel will delete the component that drop to the page panel.
                    var newComponentPanel = new Ext.ux.ComponentPanel;
                    var itm = ComponentCfg[dd.panel.titleA];
                    newComponentPanel.title = itm.title;
                    newComponentPanel.titleA = itm.titleA;
                    newComponentPanel.maxWidth = itm.maxWidth;
                    newComponentPanel.iconCls = "x-component-" + itm.componentID;
                    newComponentPanel.sortID = itm.sortID;
                    newComponentPanel.componentId = itm.componentID;
                    newComponentPanel.isSupportMultiply = itm.isSupportMultiply;

                    target.insert(_pos, newComponentPanel);

                    // MUST call initComponentSource after insert.
                    // the component need initialization, ex: the [Source] object.
                    var tab = Ext.getCmp(COMPONENT_TAB_ID);
                    initComponentSource(newComponentPanel, tab, false);
                }
                else {
                    target.insert(_pos, dd.panel);
                }

                target.doLayout();
                target.syncSize();
                target.resetComponentWidth();
                changeSaveStatus();

                if (Ext.curPageId != target.id) {
                    Ext.oldPageId = Ext.curPageId;
                    Ext.curPageId = target.id;
                }
                if (Ext.curStepId != target.ownerCt.ownerCt.id) {
                    Ext.oldStepId = Ext.curStepId;
                    Ext.curStepId = target.ownerCt.ownerCt.id;
                }
                pane.setPanelActive();
            }
            else if (dd.panel.ownerCt.id == this.dragElId) {
                // dd in a page inner
                delete this.grid;
                if (!this.PagePanel.lastPost) {
                    return;
                }
                if (Ext.curPageId != this.PagePanel.id) {
                    Ext.oldPageId = Ext.curPageId;
                    Ext.curPageId = this.PagePanel.id;
                }
                if (Ext.curStepId != this.PagePanel.ownerCt.ownerCt.id) {
                    Ext.oldStepId = Ext.curStepId;
                    Ext.curStepId = this.PagePanel.ownerCt.ownerCt.id;
                }
                var pane = Ext.getCmp("pnlPageflow");
                pane.setPanelActive();
                var c = this.lastPos.c, col = this.lastPos.col, pos = this.lastPos.p;

                var dropEvent = this.createEvent(dd, e, data, col, c,
                pos !== false ? pos : c.items.getCount());

                if (this.PagePanel.fireEvent('validatedrop', dropEvent) !== false && this.PagePanel.fireEvent('beforedrop', dropEvent) !== false) {

                    dd.proxy.getProxy().remove();
                    dd.panel.el.dom.parentNode.removeChild(dd.panel.el.dom);
                    if (pos !== false) {
                        c.insert(pos, dd.panel);
                    } else {
                        c.add(dd.panel);
                    }

                    c.doLayout();

                    this.PagePanel.fireEvent('drop', dropEvent);

                    // scroll position is lost on drop, fix it
                    var st = this.scrollPos.top;
                    if (st) {
                        var d = this.PagePanel.body.dom;
                        setTimeout(function () {
                            d.scrollTop = st;
                        }, 10);
                    }

                }
                changeSaveStatus();
                delete this.lastPos;
            }
            else if (dd.panel.ownerCt.ownerCt.id != Ext.getCmp(this.dragElId).ownerCt.id && this.PagePanel.ownerCt.ownerCt.id == dd.panel.ownerCt.ownerCt.ownerCt.id) {
                // dd in diff page but in one step
                var oldPageId = dd.panel.ownerCt.id;
                var targetPage = Ext.getCmp(this.dragElId);
                if (Ext.curPageId != targetPage.id) {
                    Ext.oldPageId = Ext.curPageId;
                    Ext.curPageId = targetPage.id;
                }
                if (Ext.curStepId != targetPage.ownerCt.ownerCt.id) {
                    Ext.oldStepId = Ext.curStepId;
                    Ext.curStepId = targetPage.ownerCt.ownerCt.id;
                }
                var pane = Ext.getCmp("pnlPageflow");
                pane.setPanelActive();
                dd.panel.ownerCt.checkPageHtml(true);
                if (targetPage.items) {
                    targetPage.insert(targetPage.items.length, dd.panel);
                } else {
                    targetPage.insert(0, dd.panel);
                }

                var oldPage = Ext.getCmp(oldPageId);
                oldPage.resetComponentWidth();
                targetPage.resetComponentWidth();
                targetPage.syncSize();
                targetPage.doLayout();
                changeSaveStatus();
            }
            else {
                // dd in diff page and deffirent step
                var targetPage = Ext.getCmp(this.dragElId);
                var oldPageId = dd.panel.ownerCt.id;
                dd.panel.ownerCt.checkPageHtml(true);
                if (targetPage.items) {
                    targetPage.insert(targetPage.items.length, dd.panel);
                } else {
                    targetPage.insert(0, dd.panel);
                }
                if (Ext.curPageId != targetPage.id) {
                    Ext.oldPageId = Ext.curPageId;
                    Ext.curPageId = targetPage.id;
                }
                if (Ext.curStepId != targetPage.ownerCt.ownerCt.id) {
                    Ext.oldStepId = Ext.curStepId;
                    Ext.curStepId = targetPage.ownerCt.ownerCt.id;
                }

                var oldPage = Ext.getCmp(oldPageId);
                oldPage.resetComponentWidth();
                targetPage.resetComponentWidth();
                var pane = Ext.getCmp("pnlPageflow");
                pane.setPanelActive();
                targetPage.syncSize();
                targetPage.doLayout();
                changeSaveStatus();
            }
            target.checkPageHtml();
        } else if (dd.panel.getXType() == "PagePanel" && this.PagePanel.ownerCt.ownerCt.id == dd.panel.ownerCt.ownerCt.id) {
            // PagePanel drag to another page but in one step
            var sourContainer = dd.panel.ownerCt;
            var targetContainer = this.PagePanel.ownerCt;
            var targetPage;
            if (this.PagePanel.ownerCt.items) {
                targetPage = this.PagePanel.ownerCt.items.items[0];
            }
            if (targetPage) {
                sourContainer.insert(0, targetPage);
            }

            targetContainer.insert(0, dd.panel);
            targetContainer.doLayout();
            sourContainer.doLayout();
            if (Ext.curPageId != dd.panel.id) {
                Ext.oldPageId = Ext.curPageId;
                Ext.curPageId = dd.panel.id;
            }
            var pane = Ext.getCmp("pnlPageflow");
            pane.setPanelActive();
            changeSaveStatus();
        } else if (dd.panel.getXType() == "PagePanel") {
            // PagePanel drag to another page but to other step
            if (Ext.curPageId != dd.panel.id) {
                Ext.oldPageId = Ext.curPageId;
                Ext.curPageId = dd.panel.id;
            }
            var pane = Ext.getCmp("pnlPageflow");
            pane.setPanelActive();
        } else {//others?
            //todo
        }
    },
    // internal cache of body and column coords
    getGrid: function () {
        var box = this.PagePanel.bwrap.getBox();
        box.columnX = [];
        if (this.PagePanel.items) {
            this.PagePanel.items.each(function (c) {
                box.columnX.push({ x: c.getPosition()[0], w: c.getPosition()[1], title: c.title });
            });
        }

        return box;
    }
});

//*****************************************************************************************
//==ComponentPanel=====:function(){}
Ext.ux.ComponentPanel = Ext.extend(Ext.Panel, {
    cls: 'x-ComponentPanel',
    autoScroll: true,
    title: 'ComponentPanel title',
    titleA: null,
    maxWidth: normalwidth,
    type: null,
    draggable: true,
    defaultType: 'panel',
    height: 0,
    componentId: 0,
    componentSeqNbr: 0,
    defaultCustomHeading: '',
    isSupportMultiply: false,
    listeners: {
        'render': function (obj) {
            // add the ASI/ASIT with group and sub group to title
            if (obj.ownerCt.getXType() == "PagePanel" && (obj.componentId == 10 || obj.componentId == 11)) {
                appendSubgroupToTitle(obj);
            }

            obj.el.on('click', function (e, target) {
                Ext.propertyOwnerId = null;
                // save old property value before click
                if (obj.ownerCt.getXType() != "PagePanel") {
                    return;
                }
                var tab = Ext.getCmp(COMPONENT_TAB_ID);
                Ext.oldComponentId = Ext.curComponentId;
                Ext.curComponentId = obj.id;
                Ext.curComponent = obj;
                Ext.propertyOwnerId = null;

                // set the ASI/ASIT group/subgroup data source that bind to the combobox.
                if (obj.titleA == "ApplicationSpecificInformation" || obj.titleA == "ApplicationSpecificInformationTable") {
                    var groupAction = 'GetASIGroups';
                    var subgroupAction = 'GetASISubGroups';

                    if (obj.titleA == "ApplicationSpecificInformationTable") {
                        groupAction = 'GetASITGroups';
                        subgroupAction = 'GetASITSubGroups';
                    }

                    // set group store
                    Ext.apply(asiGroupStore.baseParams, {
                        action: groupAction
                    });
                    asiGroupStore.reload();

                    // set sub group store
                    var groupCode = Ext.getCmp('asi_combobox_id').getValue();
                    if (asiGroupIsEmpty(groupCode)) {
                        groupCode = '';
                    }

                    Ext.apply(asiSubgroupStore.baseParams, {
                        action: subgroupAction,
                        groupCode: groupCode
                    });
                    asiSubgroupStore.reload();
                }
                // set the cap type data source that bind to the combobox.
                else if (obj.titleA == "Attachment") {
                    var pageflowGrpCode = Ext.getCmp('ddlSmartchoiceGroup').getValue();
                    getCapTypeDataSource(pageflowGrpCode);
                }

                initComponentSource(obj, tab, false);
                tab.activate(tab.getComponent(1).id);

                //Display the associated ASI Subgroup for current Contact Type for single Contact Component.
                if (obj.titleA == 'Applicant' || obj.titleA == 'Contact1' || obj.titleA == 'Contact2' || obj.titleA == 'Contact3') {
                    var source = eval("ComponentCfg." + obj.titleA + ".source");
                    var cptKey = obj.componentSeqNbr;

                    if (!cptKey || cptKey == 0) {
                        cptKey = obj.id;
                    }

                    if (!source[cptKey]) {
                        // if it is null, set default value. ex: drag from component panel.
                        source[cptKey] = cloneKeyVaulePair(source["Component_" + obj.componentId]);
                    }

                    source = source[cptKey];
                    var contValue = source["Custom Heading "];

                    if (source["Custom Heading "] == "select...") {
                        contValue = "";
                    }

                    RenderContactTypeGenericTemplate(contValue);
                }
                else {
                    RenderContactTypeGenericTemplate("");
                }

                //Add watermark style to Instructions field in Component Property.
                if (obj.titleA != 'ApplicationSpecificInformation' && obj.titleA != 'ApplicationSpecificInformationTable') {
                    var cells = tab.getComponent(1).el.dom.getElementsByTagName('td');

                    if (cells) {
                        var instructionIndex = cells.length - 1;
                        cells[instructionIndex].className += ' watermark';
                        cells[instructionIndex].childNodes[0].innerHTML = Ext.LabelKey.Admin_ClickToEdit_Watermark;

                        //When user click 'Attachment' section, set the Document Type configuration cell as 'Readonly' by default.
                        if (obj.titleA == 'Attachment') {
                            /*
                            The property grid including 2 columns per row, and the grid including a header, the header also including 2 cells.
                            And the value cell is second cell, so we use 2*{index}+3 to find the cell.
                            */
                            var docTypeConfigCell = cells[2 * tab.getComponent(1).store.indexOfId(DocumentTypeOptions) + 3];
                            var docTypeConfigCellObj = Ext.get(docTypeConfigCell);
                            var extDocTypeConfigCellContainer = docTypeConfigCellObj.findParent('div', null, true);
                            docTypeConfigCellObj.addClass('watermark');
                            extDocTypeConfigCellContainer.setStyle('display', 'none');
                        }
                    }
                }

                if (obj.ownerCt.getXType() == "PagePanel") {
                    if (Ext.curPageId != obj.ownerCt.id) {
                        Ext.oldPageId = Ext.curPageId;
                        Ext.curPageId = obj.ownerCt.id;
                    }
                    if (Ext.curStepId != obj.ownerCt.ownerCt.ownerCt.id) {
                        Ext.oldStepId = Ext.curStepId;
                        Ext.curStepId = obj.ownerCt.ownerCt.ownerCt.id;
                    }
                    var pane = Ext.getCmp("pnlPageflow");
                    pane.setPanelActive();
                }
            });
            obj.el.on('mousedown', function (e, target) {
                if (obj.ownerCt.getXType() != "PagePanel")
                    return;
                var pos = avo.position.getAbsolutePos(obj.el.dom);
                if (Ext.isIE) {
                    if (event.button == 2) {
                        obj.rightClick(obj, pos);
                    }
                } else {
                    var _which = e.browserEvent.which;
                    if (_which == 2 || _which == 3) {
                        obj.rightClick(obj, pos);
                    }
                }
            });
            obj.el.on('mouseover', function (sour, target, panel) {
                if (obj.ownerCt.getXType() == "PagePanel") {
                    obj.blur();
                    obj.focus();
                }
            });
        } //render
    },

    rightClick: function (cmp, pos) {
        Ext.cmp = cmp;

        if (Ext.curPageId != Ext.cmp.ownerCt.id) {
            Ext.oldPageId = Ext.curPageId;
            Ext.curPageId = Ext.cmp.ownerCt.id;
        }
        if (Ext.curStepId != Ext.cmp.ownerCt.ownerCt.ownerCt.id) {
            Ext.oldStepId = Ext.curStepId;
            Ext.curStepId = Ext.cmp.ownerCt.ownerCt.ownerCt.id;
        }
        var pane = Ext.getCmp("pnlPageflow");
        pane.setPanelActive();
        if (Ext.isIE) {
            showmenuFF(cmp, pos);
        } else {
            showmenuFF(cmp, pos);
        }
        document.body.onclick = hidemenu;

    }
});

Ext.reg('ComponentPanel', Ext.ux.ComponentPanel);
//******************************************************************************//
function delComponent() {
    var componentTab = Ext.getCmp(Component_Property_Tab_Id);
    var pos = parseInt(Ext.cmp.sortID);
    var chk = false;
    for (var i = 0; i < componentTab.items.length; i++) {
        var _sortId = parseInt(componentTab.items.items[i].sortID)
        if (pos < _sortId) {
            pos = i;
            chk = true;
            break;
        }
    }
    if (!chk) {
        if (pos >= componentTab.items.length) {
            pos = componentTab.items.length;
        }
    }
    Ext.cmp.ownerCt.checkPageHtml(true);
    var tab = Ext.getCmp(COMPONENT_TAB_ID);
    initComponentSource(Ext.cmp, tab, true);

    var curPage = Ext.getCmp(Ext.curPageId);

    if (Ext.cmp.isSupportMultiply) {
        if (curPage) {
            curPage.remove(Ext.cmp, true);
        }
    }
    else {
        componentTab.insert(pos, Ext.cmp);
    }

    Ext.cmp.el.applyStyles({ width: '' });
    componentTab.doLayout();
    componentTab.syncSize();
    changeSaveStatus();
    _div_menu.style.visibility = "hidden";

    tab.getComponent(1).setSource(null);

    if (curPage) {
        curPage.resetComponentWidth();
    }

    document.oncontextmenu = new Function("return true");
}
//******************************************************************************//
//==ComponentListPanel=====:function(){}
Ext.ux.ComponentListPanel = Ext.extend(Ext.Panel, {
    onRender: function (ct, position) {
        Ext.ux.ComponentListPanel.superclass.onRender.call(this, ct, position);
    },

    initComponent: function () {
        Ext.ux.ComponentListPanel.superclass.initComponent.call(this);
    },
    initEvents: function () {
        Ext.ux.ComponentListPanel.superclass.initEvents.call(this);
        this.dd = new Ext.ux.ComponentListPanel.DropZone(this, this.dropConfig);
    }
});

Ext.reg('ComponentListPanel', Ext.ux.ComponentListPanel);

Ext.ux.ComponentListPanel.DropZone = function (ComponentListPanel, cfg) {
    this.ComponentListPanel = ComponentListPanel;
    Ext.dd.ScrollManager.register(ComponentListPanel.body);
    Ext.ux.ComponentListPanel.DropZone.superclass.constructor.call(this, ComponentListPanel.id, cfg);
    ComponentListPanel.body.ddScrollConfig = this.ddScrollConfig;
};

Ext.extend(Ext.ux.ComponentListPanel.DropZone, Ext.dd.DropTarget, {
    notifyDrop: function (dd, e, data) {
        if (dd.panel.getXType() != "ComponentPanel") {
            return;
        }
        var componentTab = Ext.getCmp(Component_Property_Tab_Id);
        var check = false;
        if (dd.panel.ownerCt.getXType() == "ComponentListPanel") {
            return;
        }

        if (Ext.curPageId != dd.panel.ownerCt.id) {
            Ext.oldPageId = Ext.curPageId;
            Ext.curPageId = dd.panel.ownerCt.id;
        }
        if (Ext.curStepId != dd.panel.ownerCt.ownerCt.ownerCt.id) {
            Ext.oldStepId = Ext.curStepId;
            Ext.curStepId = dd.panel.ownerCt.ownerCt.ownerCt.id;
        }
        var pane = Ext.getCmp("pnlPageflow");
        pane.setPanelActive();
        var pos = parseInt(dd.panel.sortID);
        var chk = false;
        for (var i = 0; i < componentTab.items.length; i++) {
            var _sortId = parseInt(componentTab.items.items[i].sortID)
            if (pos < _sortId) {
                pos = i;
                chk = true;
                break;
            }
        }
        if (!chk) {
            if (pos >= componentTab.items.length) {
                pos = componentTab.items.length;
            }
        }
        var oldPageId = dd.panel.ownerCt.id;
        dd.panel.ownerCt.checkPageHtml(true);
        var tab11 = Ext.getCmp(COMPONENT_TAB_ID);
        initComponentSource(dd.panel, tab11, true)

        if (dd.panel.isSupportMultiply) {
            // only one component lie in the component panel, remove the other same components.
            dd.panel.ownerCt.remove(dd.panel, true);
        }
        else {
            componentTab.insert(pos, dd.panel);
            dd.panel.el.applyStyles({ width: '' });
        }

        var _page = Ext.getCmp(oldPageId);
        _page.resetComponentWidth();
        componentTab.doLayout();
        componentTab.syncSize();
        changeSaveStatus();
    }
});

//******************************************************************************//
//==ArrowTemplate=====:function(){}
Ext.ux.ArrowTemplate = Ext.extend(Ext.XTemplate, {
    cls: 'x-arrowCainter',
    autoScroll: false,
    draggable: false,
    listeners: {
        'render': function (obj) {
            obj.el.on('click', function (e, target, panel) {
                var pane;
                var moveType;
                if (target.id == "page-move-next-btn") {
                    moveType = "next";
                } else if (target.id == "page-move-prev-btn") {
                    moveType = "prev";
                }
                if (moveType) {
                    stepId = target.parentNode.id.split("|")[1];
                    step = Ext.getCmp(stepId);
                    step.pageMove(moveType);

                }
            });
        } //RENDER
    }
});

Ext.reg('ArrowTemplate', Ext.ux.ArrowTemplate);

//******************************************************************************//
//==menu=====:function(){}
if (typeof (avo) == 'undefined') {
    var avo = new Object();
}
avo.position = new Object();

avo.position.getAbsolutePos = function (el) {
    var SL = 0, ST = 0;
    var is_div = true;
    if (is_div && el.scrollLeft) {
        SL = el.scrollLeft;
    }
    if (is_div && el.scrollTop) {
        ST = el.scrollTop;
    }
    var r = { x: el.offsetLeft - SL, y: el.offsetTop - ST };
    if (el.offsetParent) {
        var tmp = this.getAbsolutePos(el.offsetParent);
        r.x += tmp.x;
        r.y += tmp.y;
    }
    return r;
}

function getCookieVal(offset) {
    var endstr = document.cookie.indexOf(";", offset);
    if (endstr == -1) {
        endstr = document.cookie.length;
    }
    return unescape(document.cookie.substring(offset, endstr));
}

function getCookie(name) {
    var arg = name + "=";
    var alen = arg.length;
    var clen = document.cookie.length;
    var i = 0;
    while (i < clen) {
        var j = i + alen;
        if (document.cookie.substring(i, j) == arg) {
            return getCookieVal(j);
        }
        i = document.cookie.indexOf(" ", i) + 1;
        if (i == 0) {
            break;
        }
    }
    return null;
}

avo.attachEvent = function (type, ctrlObj, funcName, parmObj) {
    var eventHandler = funcName;
    if (parmObj) {
        eventHander = function (e) {
            funcName.call(parmObj, e);
        }
    }
    if (window.document.all) {
        ctrlObj.attachEvent("on" + type, eventHander);
    } else {
        ctrlObj.addEventListener(type, eventHander, false);
    }
}
avo.doClick = function (obj) {
    if (document.createEvent) {
        var evObj = document.createEvent('MouseEvents');
        evObj.initEvent('click', true, false);
        obj.dispatchEvent(evObj);
    } else if (document.createEventObject) {
        obj.fireEvent('onclick');
    }
}

var menuskin = "menuskin";
var _div_menu;
var subDiv;
function showIEMenu() {
    _div_menu = document.getElementById("_div_menu");
    subDiv = document.getElementById("menu_subDiv");
    var rightedge = document.body.clientWidth - event.clientX;
    var bottomedge = document.body.clientHeight - event.clientY;

    if (rightedge < _div_menu.offsetWidth) {
        _div_menu.style.left = document.body.scrollLeft + event.clientX - _div_menu.offsetWidth;
    } else {
        _div_menu.style.left = document.body.scrollLeft + event.clientX;
    }

    if (bottomedge < _div_menu.offsetHeight) {
        _div_menu.style.top = document.body.scrollTop + event.clientY - _div_menu.offsetHeight;
    } else {
        _div_menu.style.top = document.body.scrollTop + event.clientY;
    }

    _div_menu.style.visibility = "visible";
    return false;
}
function showmenuFF(cmp, pos) {
    _div_menu = document.getElementById("_div_menu");
    subDiv = document.getElementById("menu_subDiv");
    var left = parseInt(pos.x + 138);
    var top = parseInt(pos.y + 2);
    _div_menu.style.left = left + "px";
    _div_menu.style.top = top + "px";
    _div_menu.style.visibility = "visible";
    document.oncontextmenu = new Function("return false");
}
function hidemenu() {
    _div_menu.style.visibility = "hidden";
    document.oncontextmenu = new Function("return true");
}

function highlight() {
    if (Ext.isIE) {
        if (event.srcElement.className == "menuitems") {
            event.srcElement.style.backgroundColor = "highlight";
            event.srcElement.style.color = "white";
        }
    } else if (subDiv.className == "menuitems") {
        subDiv.style.backgroundColor = "highlight";
        subDiv.style.color = "white";
    }

}

function lowlight() {
    if (Ext.isIE && event.srcElement.className == "menuitems") {
        event.srcElement.style.backgroundColor = "";
        event.srcElement.style.color = "black";
    } else if (subDiv.className == "menuitems") {
        subDiv.style.backgroundColor = "";
        subDiv.style.color = "black";
    }
}

function jump() {
    if (navigator.userAgent.indexOf("MSIE 6.0") > -1) {
        var seltext = window.document.selection.createRange().text

        if (event.srcElement.className == "menuitems") {
            if (event.srcElement.getAttribute("target") != null) {
                window.open(event.srcElement.url, event.srcElement.getAttribute("target"));
            }
            else {
                window.location = event.srcElement.url;
            }
        }
    }
}

//******************************************************************************//
//==data=====:function(){}
var propertyArr = new Array();
propertyArr.push("Custom Heading");
propertyArr.push("Required");
propertyArr.push("Data Source");
propertyArr.push("Custom Heading ");
propertyArr.push("title");
propertyArr.push("Editable");
propertyArr.push("Group");
propertyArr.push("Subgroup");
propertyArr.push("Path");

var propertyArrTitle = new Array();
propertyArrTitle.push("customHeading");
propertyArrTitle.push("requiredFlag");
propertyArrTitle.push("validateFlag");
propertyArrTitle.push("Custom Heading ");
propertyArrTitle.push("title");
propertyArrTitle.push("editableFlag");
propertyArrTitle.push("portletRange1");
propertyArrTitle.push("portletRange2");
propertyArrTitle.push("portletRange1");

var normalwidth = 155;
var maxwidth = 250;

//construct component data source where it comes from.
//Reference: get information by search button
//Transactional: get information by user input
//No Limitation: get information both reference and transaction
var ComponentDataSource = {
    R: 'Reference',
    T: 'Transactional',
    N: 'No Limitation',

    Reference: 'R',
    Transactional: 'T',
    'No Limitation': 'N'
};

function CreateComponentSource(title, componentID, sortID, defaultLangHeading, heading, isSupportMultiComponent) {
    if (!isSupportMultiComponent) {
        isSupportMultiComponent = false;
    }

    // remove space for titleA
    var titleA = title.replace(/(\s*)/g, "");
    if (title == 'ASI') {
        titleA = 'ApplicationSpecificInformation';
    }
    else if (title == 'ASI Table') {
        titleA = 'ApplicationSpecificInformationTable';
    }

    // Default Heading, Heading(Default Language)(in I18N),  Custom Heading
    var source = {};
    source["Default Heading"] = heading;

    if (IsSupportMultiLanguage()) {
        source["Heading(Default Language)"] = defaultLangHeading;
    }

    if (5 <= componentID && componentID <= 8) {
        source["Custom Heading "] = 'select...';   //for Applicant, Contact 1/2/3.
    }
    else {
        source["Custom Heading"] = heading;
    }

    /*
    (1-8: A,P,O,L,Applicant,Contact1/2/3 | 14: Contact List | 18: LPList) -- Required/Validate/Editable
    (15-17: Education,Continuing Education,Examination) -- Required/Editable
    (9,12: Additional Information,Detail Information) -- Editable
    */
    if ((1 <= componentID && componentID <= 8) || componentID == 14 || componentID == 18) {
        source["Required"] = true;
        source["Data Source"] = ComponentDataSource['N'];
        source["Editable"] = true;

        if (componentID == 14) {
            source["Contact Type Options"] = Ext.LabelKey.Admin_FieldProperty_ChoiceTip;
        }
    }
    else if (15 <= componentID && componentID <= 17) {
        source["Required"] = true;
        source["Editable"] = true;
    }
    else if (componentID == 9 || componentID == 12 || componentID == 19) {
        source["Editable"] = true;
    }
    else if (componentID == 10 || componentID == 11) {
        source["Group"] = Ext.ASIGroupEmptyText;
        source["Subgroup"] = Ext.ASIGroupEmptyText;
    }    
    else if (componentID == 20) {
        source["Path"] = "";
    }
    else if (componentID == 13) {//attachment
        source["Record Types"] = Ext.RecordTypeEmptyText;
        source["Document Type Options"] = Ext.LabelKey.Admin_FieldProperty_ChoiceTip;
    }
    else if(componentID == 22){
    	// Asset
        source["Required"] = true;
    }

    // Add Instruction to each component except for ASI/ASIT property.
    if (componentID != 10 && componentID != 11) {
        source["Instructions"] = "";
    }

    // set the source to components which support multiply instance
    if (isSupportMultiComponent) {
        var cptDefaultId = "Component_" + componentID;

        var temp = source;
        source = {};
        source[cptDefaultId] = temp;
    }

    var dataSoure = {
        "title": title,
        "titleA": titleA,
        "componentID": componentID,
        "sortID": sortID,
        "maxWidth": normalwidth,
        "display": true,
        "isSupportMultiply": isSupportMultiComponent,
        "source": source
    };

    return dataSoure;
}

function CreateComponents() {
    var components = {
        "Address": CreateComponentSource('Address', 1, 1,
                   Ext.LabelKey.ACA_Pageflow_Component_Address_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_Address),
        "Parcel": CreateComponentSource('Parcel', 2, 2,
                   Ext.LabelKey.ACA_Pageflow_Component_Parcel_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_Parcel),
        "Owner": CreateComponentSource('Owner', 3, 3,
                   Ext.LabelKey.ACA_Pageflow_Component_Owner_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_Owner),
        "Assets": CreateComponentSource('Assets', 22, 4,
                   Ext.LabelKey.ACA_Pageflow_Component_Assets_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_Assets),
        "LicensedProfessional": CreateComponentSource('Licensed Professional', 4, 5,
                   Ext.LabelKey.ACA_Pageflow_Component_LP_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_LP, true),
        "LicensedProfessionalList": CreateComponentSource('Licensed Professional List', 18, 6,
                   Ext.LabelKey.ACA_Pageflow_Component_LPList_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_LPList),
        "Applicant": CreateComponentSource('Applicant', 5, 7,
                   Ext.LabelKey.ACA_Pageflow_Component_Applicant_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_Applicant, true),
        "Contact1": CreateComponentSource('Contact 1', 6, 8,
                   Ext.LabelKey.ACA_Pageflow_Component_Contact1_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_Contact1, true),
        "Contact2": CreateComponentSource('Contact 2', 7, 9,
                   Ext.LabelKey.ACA_Pageflow_Component_Contact2_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_Contact2, true),
        "Contact3": CreateComponentSource('Contact 3', 8, 10,
                   Ext.LabelKey.ACA_Pageflow_Component_Contact3_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_Contact3, true),
        "ContactList": CreateComponentSource('Contact List', 14, 11,
                   Ext.LabelKey.ACA_Pageflow_Component_ContactList_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_ContactList, true),
        "AdditionalInformation": CreateComponentSource('Additional Information', 9, 12,
                   Ext.LabelKey.ACA_Pageflow_Component_AdditionalInfo_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_AdditionalInfo),
        "ApplicationSpecificInformation": CreateComponentSource('ASI', 10, 13,
                   Ext.LabelKey.ACA_Pageflow_Component_ASI_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_ASI, true),
        "ApplicationSpecificInformationTable": CreateComponentSource('ASI Table', 11, 14,
                   Ext.LabelKey.ACA_Pageflow_Component_ASIT_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_ASIT, true),
        "DetailInformation": CreateComponentSource('Detail Information', 12, 15,
                   Ext.LabelKey.ACA_Pageflow_Component_DetailInfo_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_DetailInfo),
        "Attachment": CreateComponentSource('Attachment', 13, 16,
                   Ext.LabelKey.ACA_Pageflow_Component_Attachment_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_Attachment, true),
        "Education": CreateComponentSource('Education', 15, 17,
                   Ext.LabelKey.ACA_Pageflow_Component_Education_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_Education),
        "ContinuingEducation": CreateComponentSource('Continuing Education', 16, 18,
                   Ext.LabelKey.ACA_Pageflow_Component_ContinuingEducation_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_ContinuingEducation),
        "Examination": CreateComponentSource('Examination', 17, 19,
                   Ext.LabelKey.ACA_Pageflow_Component_Examination_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_Examination),
        "ValuationCalculator": CreateComponentSource('Valuation Calculator', 19, 20,
                   Ext.LabelKey.ACA_Pageflow_Component_ValuationCalculator_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_ValuationCalculator),
        "CustomComponent": CreateComponentSource('Custom Component', 20, 21,
                   Ext.LabelKey.ACA_Pageflow_Component_CustomComponent_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_CustomComponent),
        "ConditionDocument": CreateComponentSource('Condition Document', 21, 22,
                   Ext.LabelKey.ACA_Pageflow_Component_ConditionDocument_DefaultLanguage, Ext.LabelKey.ACA_Pageflow_Component_ConditionDocument)
    };

    return components;
}

var ComponentCfg = CreateComponents();
var ComponentCfg_Template = CreateComponents();

function orderStep(x, y) {
    if (x.stepOrder > y.stepOrder) {
        return 1;
    } else {
        return -1;
    }
}
function orderPage(x, y) {
    if (x.pageOrder > y.pageOrder) {
        return 1;
    } else {
        return -1;
    }
}
function orderComponent(x, y) {
    if (x.displayOrder > y.displayOrder) {
        return 1;
    } else {
        return -1;
    }
}

function IsSupportMultiLanguage() {
    var conn = Ext.lib.Ajax.getConnectionObject().conn;
    conn.open("GET", "../TreeJSON/I18nCulture.aspx?action=MultiLanguageSupportEnable", false);
    conn.send(null);
    return conn.responseText == 'Y';
}

//page property setting,do not change it to LabelKey,it cann't support i18n
var PROPERTY_PAGE_NAME = 'Page Name';
var PROPERTY_STEP_NAME = 'Step Name';
var DEFALUT_PAGE_NAME = ' Page Name(Default Language)';
var ONLOAD_EVENT = 'Onload Event';
var BEFORE_BUTTON_EVENT = 'BeforeButton Event';
var AFTER_BUTTON_EVENT = 'AfterButton Event';
var DEFALUT_EVENT_NAME = '--Scripts List--';
var GET_EMSE_EVENTS = "GetEmseEventNames"; //for ajax picker
var PAGE_INSTRUCTION_DEFAULT_LANGUAGE = 'Instructions(Default Language)';
var PAGE_INSTRUCTION = 'Instructions';

var pageProperty = function() {
    this.pageName = PROPERTY_PAGE_NAME,
        this.defalutPageName = DEFALUT_PAGE_NAME,
        this.onloadEvent = DEFALUT_EVENT_NAME,
        this.beforeButtonEvent = DEFALUT_EVENT_NAME,
        this.afterButtonEvent = DEFALUT_EVENT_NAME,
        this.defaultLangInstruction = PAGE_INSTRUCTION_DEFAULT_LANGUAGE,
        this.instruction = PAGE_INSTRUCTION
};

//init dropdownlist for emse
pageProperty.prototype.initProperty = function(isSupportMultiLanguage) {
    var multiLanguage = '';
    var defaultLangInstruction = '';
    if (isSupportMultiLanguage) {
        multiLanguage = "'" + EncodeSpecialChar(DEFALUT_PAGE_NAME) + "':'',";
        defaultLangInstruction = String.format("'{0}':'',", PAGE_INSTRUCTION_DEFAULT_LANGUAGE);
    }
    var jsonText = "{" + multiLanguage + "'" + EncodeSpecialChar(PROPERTY_PAGE_NAME) + "':'','" + EncodeSpecialChar(ONLOAD_EVENT) + "':'','" +
        EncodeSpecialChar(BEFORE_BUTTON_EVENT) + "':'','" + EncodeSpecialChar(AFTER_BUTTON_EVENT) + "':''," +
        defaultLangInstruction + "'" + PAGE_INSTRUCTION + "':''}";

    eval("var json=" + jsonText);
    return json;
};

pageProperty.prototype.setProperty = function(isSupportMultiLanguage) {
    var multiLanguage = '';
    var defaultLangInstruction = '';
    if (isSupportMultiLanguage) {
        multiLanguage = "'" + EncodeSpecialChar(DEFALUT_PAGE_NAME) + "':'" + EncodeSpecialChar(this.defalutPageName) + "',";
        defaultLangInstruction = String.format("'{0}':'{1}',", PAGE_INSTRUCTION_DEFAULT_LANGUAGE, EncodeSpecialChar(this.defaultLangInstruction));
    }
    var jsonText = "{" + multiLanguage + "'" + EncodeSpecialChar(PROPERTY_PAGE_NAME) + "':'" + EncodeSpecialChar(this.pageName) + "','" + EncodeSpecialChar(ONLOAD_EVENT) + "':'" + EncodeSpecialChar(this.onloadEvent) + "','" +
        EncodeSpecialChar(BEFORE_BUTTON_EVENT) + "':'" + EncodeSpecialChar(this.beforeButtonEvent) + "','" +
        EncodeSpecialChar(AFTER_BUTTON_EVENT) + "':'" + EncodeSpecialChar(this.afterButtonEvent) + "'," +
        defaultLangInstruction + "'" + PAGE_INSTRUCTION + "':'" + EncodeSpecialChar(this.instruction) + "'}";

    eval("var json=" + jsonText);
    return json;
};
