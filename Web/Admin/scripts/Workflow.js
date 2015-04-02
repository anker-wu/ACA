/**
* <pre>
* 
*  Accela Citizen Access
*  File: workflow.js
* 
*  Accela, Inc.
*  Copyright (C): 2007-2014
* 
*  Description:
* 
*  Notes:
* $Id: workflow.js 72643 2008-04-24 09:52:06Z $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* 04/24/2008     		daly.zeng				Initial.  
* </pre>
*/

var formWinTarget;
var isContactTypeDDLWinShow = false;
var isDocumentTypeDDLWinShow = false;
var IsSupperAgency = false;
var asiGroupStore = getDataSource({ action: 'GetASITGroups' });
var asiSubgroupStore = getASISubGroupDataSource({ action: 'GetASITSubGroups', groupCode: '' });
var capTypeStore = new Ext.data.SimpleStore({ fields: ['Text', 'Value'] });

/*
Required document types for each record type
Data structure:
    {key:Record type, value: {key:Document type, value:I18N value of document type}}
*/
var requiredDocumentTypeStore = {};

// the contact type mapping, use {component sequence number} mapping {contact type jason} 
var _contactTypeMapping = null;

// the document type mapping, use {component sequence number} mapping {document type jason} 
var _documentTypeMapping = null;

/*
The settings store of available document types for each Record Type in each Attachment section.
The store contains the existing setting from DB and contains the new configured settings.
The the existing Attachment removed from the page flow, the relevant data item will not be removed
    from the store.
*/
var _docTypeOptionConfigStore = null;

/*
Used to collect the identity info of all the attachment sections in current page flow before save,
 and to validate the required document type for each attachment section and each record type.
Data structure:
    {id:Ext component ID, seqNbr: Component seq number}
    seqNbr is 0 means it's a new added Attachment section, not yet stored in DB.
*/
var _attachmentSectionInfoList = null;

// attchment component prefix
var ATTACHMENT_COMPONENT_PREFIX = "Attachment_";

function IsSupperAgencyEnabled() {
    var conn = Ext.lib.Ajax.getConnectionObject().conn;
    conn.open("GET", "../Pageflow/WorkflowContent.aspx?action=IsSupperAgencyEnabled&temp=" + new Date(), false);
    conn.send(null);
    return conn.responseText == 'Y';
}

var winDefaultWidth = 195;

var newFormWin = new Ext.Window({
    layout: 'fit',
    width: winDefaultWidth,
    height: 325,
    closeAction: 'hide',
    resizable: false,
    plain: true,
    html: '<div id = "container" style="WIDTH:99%; HEIGHT: 270px;overflow-x:hidden;overflow-y:auto;"></div>',
    bbar: [new Ext.Toolbar.TextItem("&nbsp;&nbsp;&nbsp;"),
               new Ext.Toolbar.Button({ text: "OK", id: "btnDropDownOK", pressed: true, minWidth: 50, handler: SaveTypeListToUI }),
               new Ext.Toolbar.Spacer(), new Ext.Toolbar.Spacer(), new Ext.Toolbar.Spacer(),
               new Ext.Toolbar.Button({ text: "Cancel", id: "btnCancel", pressed: true, minWidth: 50, handler: CloseWorkFlowDDLWin })]
});

Ext.onReady(function () {
    Ext.BLANK_IMAGE_URL = '../images/empty.gif';
    Ext.QuickTips.init();
    Ext.state.Manager.setProvider(new Ext.state.CookieProvider());
    document.moduleName = getparmByUrl('moduleName');
    IsSupperAgency = IsSupperAgencyEnabled();

    var viewport = new Ext.Viewport({
        id: 'myviewport',
        layout: 'border',
        autoScroll: true,
        items: [{
            // the page flow panel in the middle, contains header and body section. 
            region: 'center',
            autoScroll: true,
            id: 'wfUniqueId',
            listeners: {
                'render': function (obj) {
                    obj.el.on('resize', function () {
                        if (Ext.isIE6) {
                            correctComponentLayout();
                            doLayoutStepPanel();
                        }
                    });
                }
            },
            contentEl: 'Workflow'
        }, {
            // the component settings panel in the right.
            region: 'east',
            id: 'propertyPanel',
            title: 'Component Settings',
            collapsible: true,
            collapsed: true,
            collapseFirst: true,
            listeners: {
                'beforecollapse': function (obj) {
                    Ext.workflowFixWidth = 0;
                    Ext.EastPanelWidth = 0;
                    Ext.IsResize = true;
                    doLayoutStepPanel(true);
                },
                'expand': function (obj) {
                    Ext.workflowFixWidth = obj.getSize().width;
                    Ext.EastPanelWidth = obj.getSize().width;
                    Ext.IsResize = true;
                    doLayoutStepPanel(true);
                }
            },
            split: true,
            width: 180,
            minSize: 175,
            maxSize: 260,
            layout: 'fit',
            margins: '0 5 0 0',
            contentEl: 'property'
        }]
    });
    viewport.syncSize();

    Init();

    Ext.IsSupportMultiLanguage = IsSupportMultiLanguage();
});

function Init() {
    InitHeader();

    emseEventList = null;
    emseEventList = new Ext.data.SimpleStore({
        url: '../Pageflow/WorkflowContent.aspx',
        baseParams: {
            action: GET_EMSE_EVENTS
        },
        fields: ['EventName']
    });
    emseEventList.load();

    //page flow section initialization
    InitWorkPanel();

    Ext.isFirstLoad = false;

    //Associated caps grid initialization
    var capStore = new Ext.data.SimpleStore({
        data: [],
        fields: ['capName', 'alias']
    });
    InitGrid(capStore);

    InitCapAssociation();

    Ext.get('capList').dom.style.display = 'none';
    Ext.get('pageflow').dom.style.display = 'none';
    Ext.get('capAssociation').dom.style.display = 'none';

    Ext.ScreenMaxWidth = screen.width;
}

// Initial the header in the middle panel at 'Page Flow Configuration'.
function InitHeader() {
    var groupCodeBeforeCancel;
    var smartchoiceGroupStore = new Ext.data.SimpleStore({
        url: '../Pageflow/WorkflowContent.aspx',
        baseParams: {
            action: 'GetPageFlowGroupNameList'
        },
        fields: ['Name']
    });
    smartchoiceGroupStore.load();

    var header = new Ext.FormPanel({
        header: false,
        bodyStyle: 'padding:5px',
        id: 'pnlHeader',
        labelAlign: 'left',
        layout: 'column',
        border: false,
        labelSeparator: ' :',
        frame: true,
        monitorResize: true,
        items: [{
            // 1. the top section in the header, the column about top instruction label
            columnWidth: 1,
            layout: 'form',
            bodyStyle: 'padding-bottom:8px',
            border: false,
            items: [{
                xtype: 'label',
                text: Ext.LabelKey.Admin_Pageflow_lblSmartchoiceInstruction
            }]
        }, {
            // 2. the middle section in the header
            // 2.1 the column about select page flow dropdownlist
            columnWidth: .45,
            layout: 'form',
            bodyStyle: 'padding-bottom:8px',
            border: false,
            items: [{
                xtype: 'combo',
                id: 'ddlSmartchoiceGroup',
                store: smartchoiceGroupStore,
                displayField: "Name",
                valueField: "Name",
                mode: 'local',
                emptyText: Ext.LabelKey.Admin_Pageflow_ddlEmptyText,
                editable: false,
                triggerAction: 'all',
                height: '100px',
                hideLabel: true,
                forceSelection: true,
                anchor: '90%',
                loadingText: 'Loading....',
                listeners: {
                    'beforeselect': function (combo, record, index) {
                        groupCodeBeforeCancel = Ext.getCmp('ddlSmartchoiceGroup').getValue();
                    },
                    'select': function (combo, record, index) {
                        Ext.isCreatePageflow = false;
                        if (!document.changed) {
                            _contactTypeMapping = null;
                            _documentTypeMapping = null;
                            LoadData(JsonDecode(record.data['Name']));

                            return;
                        }

                        // Show confirm dialog when selected item changed and page flow not saved
                        Ext.Msg.show({
                            msg: Ext.LabelKey.Admin_Pageflow_Message_CancelPageFlow,
                            buttons: Ext.Msg.OKCANCEL,
                            fn: function (btn) {
                                if (btn != null && btn == 'ok') {
                                    _contactTypeMapping = null;
                                    LoadData(JsonDecode(record.data['Name']));

                                    window.parent.RemoveMark();
                                    document.changed = false;
                                }
                                else if (btn != null && btn == 'cancel') {
                                    if (groupCodeBeforeCancel != null && groupCodeBeforeCancel != "") {
                                        Ext.getCmp('ddlSmartchoiceGroup').setValue(groupCodeBeforeCancel);
                                    }
                                }
                            },
                            animEl: '',
                            icon: Ext.MessageBox.WARNING
                        });
                    }
                }
            }]
        }, {
            // 2.2 the column about label 'or'
            columnWidth: .05,
            bodyStyle: 'padding-bottom:8px',
            layout: 'form',
            border: false,
            items: [{
                xtype: 'label',
                id: 'lblOr',
                minWidth: 20,
                text: Ext.LabelKey.Admin_Pageflow_lblOr
            }]
        }, {
            // 2.3 the column about create new page flow button
            columnWidth: .5,
            layout: 'form',
            bodyStyle: 'padding-bottom:8px',
            border: false,
            items: [{
                xtype: 'button',
                id: 'btnCreateNew',
                minWidth: 80,
                text: Ext.LabelKey.Admin_Pageflow_btnCreateNew,
                handler: function (e) {
                    Ext.isCreatePageflow = true;
                    if (!document.changed) {
                        RefreshCapAssociation('', 'New');
                        return;
                    }

                    // Show confirm dialog when create new page flow and exist not saved
                    Ext.Msg.show({
                        msg: Ext.LabelKey.Admin_Pageflow_Message_CancelPageFlow,
                        buttons: Ext.Msg.OKCANCEL,
                        fn: function (btn) {
                            if (btn != null && btn == 'ok') {
                                RefreshCapAssociation('', 'New');
                                InitNewPageFlowUI();

                                window.parent.RemoveMark();
                                document.changed = false;
                            }
                        },
                        animEl: '',
                        icon: Ext.MessageBox.WARNING
                    });
                }
            }]
        }, {
            // 3. the bottom section in the header
            columnWidth: 1,
            layout: 'form',
            bodyStyle: 'padding-top:1px',
            border: false,
            items: [{
                // 3.1 the label about record count
                xtype: 'label',
                style: 'padding-right:50px',
                id: 'lblCapSummary',
                hidden: true,
                text: Ext.LabelKey.Admin_Pageflow_lblCapSummary
            }, {
                // 3.2 the link button about show detail
                xtype: 'link',
                id: 'lnkShowCap',
                hidden: true,
                text: Ext.LabelKey.Admin_Pageflow_lnkShowDetails,
                alternateText: Ext.LabelKey.Admin_Pageflow_lnkHideDetails,
                alternateHandler: function () {
                    Ext.get('capList').dom.style.display = 'none';
                    Ext.get('pageflow').dom.style.display = 'block';
                    resizePages();
                    Ext.get('capAssociation').dom.style.display = 'none';
                },
                handler: function () {
                    Ext.get('capList').dom.style.display = 'block';
                    Ext.get('pageflow').dom.style.display = 'none';
                    Ext.get('capAssociation').dom.style.display = 'none';

                    var groupCode = Ext.getCmp('ddlSmartchoiceGroup').getValue();
                    RefreshCapList(groupCode);
                }
            }]
        }]
    });

    header.render('divHeader');
    header.doLayout();
}

function InitNewPageFlowUI() {
    Ext.get('capList').dom.style.display = 'none';
    Ext.get('capAssociation').dom.style.display = 'block';
    Ext.get('pageflow').dom.style.display = 'none';
    Ext.get('txtMode').dom.value = 'New';
    Ext.getCmp('txtNewSmartchoiceGroupName').setDisabled(false);
    Ext.getCmp('txtNewSmartchoiceGroupName').setValue('');
    Ext.getCmp('ddlSmartchoiceGroup').setValue('');
    Ext.getCmp('lblCapSummary').hide();
    Ext.getCmp('lnkShowCap').hide();

    RefreshHeader(0);

    ClearCapList();
    ClearPageflow();
}

function SaveTypeListToUI() {
    if (formWinTarget == "ContactType") {
        SaveContactTypeListToUI();
    } else if (formWinTarget == "DocumentType") {
        SaveDocumentTypeListToUI();
    }
}

function SaveContactTypeListToUI() {
    var dragEls = Ext.get('container').query('.dropblockContactType');

    var jsonStr = new StringBuffer();
    jsonStr.append("[");
    for (var i = 1; i < dragEls.length; i++) {
        var checked = dragEls[i].getElementsByTagName("input")[0].checked;
        var key = DecodeHTMLTag(dragEls[i].getElementsByTagName("SPAN")[0].innerHTML).replace(/\\/img, '\\\\').replace(/\"/img, '\\"');
        var minNum = dragEls[i].getElementsByTagName("input")[1].value;
        var maxNum = dragEls[i].getElementsByTagName("input")[2].value;

        if (maxNum != "" && minNum != "" && parseInt(maxNum) < parseInt(minNum)) {
            Ext.Msg.alert(Ext.LabelKey.admin_pageflow_message_information, "The max number should be larger than or equal to the min number.");
            return;
        }

        jsonStr.append('{');
        jsonStr.append('"Key":"').append(key);
        jsonStr.append('","Text":"').append(key); //not support i18n in aca admin.
        jsonStr.append('","Checked":').append(checked);
        jsonStr.append(',"MinNum":"').append(minNum);
        jsonStr.append('","MaxNum":"').append(maxNum);
        jsonStr.append('"}');

        if (i != dragEls.length - 1) {
            jsonStr.append(",");
        }
    }

    jsonStr.append("]");

    if (_contactTypeMapping == null) {
        _contactTypeMapping = {};
    }

    _contactTypeMapping[Ext.curComponentId] = { cptSeqNbr: Ext.curComponent.componentSeqNbr, json: jsonStr.toString() };

    document.contactTypeListChanged = true;
    changeSaveStatus();
    newFormWin.hide();
}

function SaveDocumentTypeListToUI() {
    dragEls = Ext.get('container').query('.dropblockContactType');

    var jsonStr = new StringBuffer();
    jsonStr.append("[");
    for (var i = 1; i < dragEls.length; i++) {
        var checked = dragEls[i].getElementsByTagName("input")[0].checked;
        var value = dragEls[i].getElementsByTagName("input")[0].getAttribute("doctype").replace(/\\/img, '\\\\').replace(/\"/img, '\\"');
        var text = DecodeHTMLTag(dragEls[i].getElementsByTagName("SPAN")[0].innerHTML).replace(/\\/img, '\\\\').replace(/\"/img, '\\"');

        jsonStr.append('{');
        jsonStr.append('"ResDocumentType":"').append(text);
        jsonStr.append('","DocumentType":"').append(value);
        jsonStr.append('","Checked":').append(checked);
        jsonStr.append('}');

        if (i != dragEls.length - 1) {
            jsonStr.append(",");
        }
    }

    jsonStr.append("]");

    var alias = Ext.getCmp('record_type_combobox_id').getValue().trim();
    var capTypeKey = GetRecordTypeValueByAlias(alias);
    var documentTypeUIModelJson = { CapTypeKey: capTypeKey, DocumentTypes: jsonStr.toString() };

    if (_documentTypeMapping == null) {
        _documentTypeMapping = {};
    }

    var docTypeMappingItem = _documentTypeMapping[Ext.curComponentId];
    if (!docTypeMappingItem) {
        docTypeMappingItem = {};
    }

    if (!docTypeMappingItem.json) {
        docTypeMappingItem.json = new Array();
    }

    var jsonArray = docTypeMappingItem.json;
    if (IsExistDocumentType(jsonArray, capTypeKey)) {
        jsonArray.remove(GetDocumentTypeByCapTypeKey(jsonArray, capTypeKey));
    }

    jsonArray.push(documentTypeUIModelJson);

    // Update the document type option settings to the data store -- begin.
    var docTypes = eval("(" + documentTypeUIModelJson.DocumentTypes + ");");
    
    if (_docTypeOptionConfigStore == null) {
        _docTypeOptionConfigStore = { };
    }

    var componentName = Ext.curComponent.componentSeqNbr == 0
        ? Ext.curComponentId
        : ATTACHMENT_COMPONENT_PREFIX + Ext.curComponent.componentSeqNbr;

    if(_docTypeOptionConfigStore[componentName] == null) {
        _docTypeOptionConfigStore[componentName] = { };
    }

    _docTypeOptionConfigStore[componentName][capTypeKey] = docTypes;
    // Update the document type option settings to the data store -- end.

    _documentTypeMapping[Ext.curComponentId] = { cptSeqNbr: Ext.curComponent.componentSeqNbr, json: jsonArray };

    document.documentTypeListChanged = true;
    changeSaveStatus();
    newFormWin.hide();
}

// Gets the selected available document types by specified record type.
function GetSelectedDocumentTypes(recordType, compNameList) {
    var selectDocumentTypes = [];

    for(var i = 0; i < compNameList.length; i++) {
        var compName = compNameList[i];
        var docTypeSettingsByRecordType = _docTypeOptionConfigStore[compName];
        var recordTypeConfig =  docTypeSettingsByRecordType[recordType];

        if (recordTypeConfig.length == 0) {
            continue;
        }

        for (var j = 0; j < recordTypeConfig.length; j++) {
            var docTypeOption = recordTypeConfig[j];

            if (!docTypeOption.Checked || selectDocumentTypes.contains(docTypeOption.DocumentType)) {
                continue;
            }

            selectDocumentTypes.push(docTypeOption.DocumentType);
        }
    }

    return selectDocumentTypes;
}

/*
Check if there is an attachment section does not configure any settings for specified record type.
if ture, the validation for the specified record type will be passed.
*/
function IsExistComponentNotDocTypeConfig(recordType, compNameList) {
    var isExist = false;

    for(var i = 0; i < compNameList.length; i++) {
        var compName = compNameList[i];
        var docTypeSettingsByRecordType = _docTypeOptionConfigStore[compName];

        var recordTypeConfig =  docTypeSettingsByRecordType[recordType];

        if (recordTypeConfig == null) {
            isExist = true;
            break;
        }
    }

    return isExist;
}

function CloseWorkFlowDDLWin() {
    var container = document.getElementById('container');

    if (container) {
        container.innerHTML = '';
    }

    try {
        if (win && isWinShow) {
            win.hide();
            isHTMLWinShow = false;
        }

        if (newFormWin && (isContactTypeDDLWinShow || isDocumentTypeDDLWinShow)) {
            newFormWin.hide();
            isContactTypeDDLWinShow = false;
            isDocumentTypeDDLWinShow = false;
        }
    }
    catch (err) {
    }
}

function InitUIAfterCancel() {
    if (Ext.get('txtMode').dom.value == 'New') {
        Ext.get('capList').dom.style.display = 'none';
        ShowSummary(false);
    }
    else {
        Ext.get('capList').dom.style.display = 'block';
        ShowSummary(true);
    }
    Ext.get('capAssociation').dom.style.display = 'none';
}

function InitGrid(capStore) {
    var grid = new Ext.grid.GridPanel({
        id: 'grdCapTypes',
        store: capStore,
        cm: new Ext.grid.ColumnModel([
    		{ header: 'CAP Name', dataIndex: 'CapName', sortable: true, width: 350 },
    		{ header: 'ACA Alias', dataIndex: 'Alias', sortable: true, width: 350 }
        ]),
        autoExpandColumn: 1,
        autoHeight: true,
        viewConfig: { forceFit: true },
        frame: true,
        iconCls: 'icon-grid'
    });

    var caplist = new Ext.Panel({
        //el:'capList',
        id: 'pnlCapList',
        header: false,
        buttonAlign: 'left',
        autoWidth: true,
        width: '98%',
        border: false,
        buttons: [{
            id: 'btnModifyAssociation',
            text: Ext.LabelKey.Admin_Pageflow_btnModifyAssociation,
            handler: function () {
                var groupCode = Ext.getCmp('ddlSmartchoiceGroup').getValue();
                RefreshCapAssociation(groupCode, 'Edit');
            }
        }],
        items: [
            grid
        ]
    });
    caplist.render('capList');

    return grid;
};

function InitCapAssociation() {
    var btnAddCap = new Ext.Button({
        id: 'btnAddCap',
        disabled: true,
        minWidth: 80,
        text: Ext.LabelKey.Admin_Pageflow_btnAddCap,
        renderTo: 'addCap'
    });
    var btnRemove = new Ext.Button({
        disabled: true,
        minWidth: 80,
        text: Ext.LabelKey.Admin_Pageflow_btnRemoveCap,
        renderTo: 'removeCap'
    });
    var btnOK = new Ext.Button({
        id: 'btnOK',
        disabled: true,
        minWidth: 80,
        text: Ext.LabelKey.Admin_Pageflow_btnOK,
        handler: SavePageFlowGroup,
        renderTo: 'OK'
    });
    var btnCancel = new Ext.Button({
        id: 'btnCancel',
        minWidth: 80,
        text: Ext.LabelKey.Admin_Pageflow_btnCancel,
        handler: function () {
            if (!document.changed) {
                InitUIAfterCancel();
                return;
            }

            Ext.Msg.show({
                //title:'Cancel',
                msg: Ext.LabelKey.Admin_Pageflow_Message_Cancel,
                buttons: Ext.Msg.OKCANCEL,
                fn: function (btn) {
                    if (btn != null && btn == 'ok') {
                        InitUIAfterCancel();
                        //RestoreSelectedCaps();
                        window.parent.RemoveMark();
                        document.changed = false;
                    }
                },
                animEl: '',
                icon: Ext.MessageBox.WARNING
            });
        },
        renderTo: 'Cancel'
    });

    var txtSmartchoiceGroupName = new Ext.form.FormPanel({
        labelAlign: 'Left',
        header: false,
        bodyStyle: 'padding-top:10px',
        renderTo: 'smartchoiceGroupName',
        //frame:true,
        labelWidth: 150,
        cls: 'padding:5px',
        border: false,
        items: [{
            layout: 'column',
            border: false,
            items: [{
                columnWidth: 1,
                layout: 'form',
                border: false,
                items: [{
                    xtype: 'textfield',
                    //allowBlank: false,
                    emptyText: Ext.LabelKey.Admin_Pageflow_txtSmartChoiceEmptyText,
                    id: 'txtNewSmartchoiceGroupName',
                    fieldLabel: Ext.LabelKey.Admin_Pageflow_txtSmartChoicelabel,
                    maxLength: 40,
                    minLength: 1,
                    width: 300,
                    listeners: {
                        'blur': function (control) {
                            //accela requires a page flow needs to be created without selecting any cap
                            if (control.getValue().trim() != '') {
                                changeSaveStatus();
                                CheckNewGroupCondition();
                            }
                            else {
                                Ext.getCmp('btnOK').setDisabled(true);
                                window.parent.RemoveMark();
                            }
                        }
                    }
                }, {
                    xtype: 'hidden',
                    id: 'txtMode'
                }]
            }]
        }]
    });

    var capAvailable = new Ext.tree.CapPanel({
        id: 'pnlCapAvailable',
        title: Ext.LabelKey.Admin_Pageflow_pnlCapAvailableTitle,
        animate: true,
        style: 'padding:10px 10px 20px 20px',
        height: 300,
        lines: false,
        root: new Ext.tree.AsyncTreeNode({
            id: 'rootCapAvailable',
            childNodes: {}
        }),
        rootVisible: false,
        enableDD: true,
        containerScroll: true,
        autoScroll: true,
        selModel: new Ext.tree.MultiSelectionModel()
        //dropConfig: {appendOnly:true},
        //loader:new Ext.tree.TreeLoader({url:"../Pageflow/WorkflowData.aspx"})
        //		loader:new Ext.tree.TreeLoader({
        //		    url:"../Pageflow/WorkflowContent.aspx",
        //		    baseParams:{
        //		        ModuleName:'Building',
        //		        action:'GetRelatedCaps',
        //		        groupCode:'DYLAN'}
        //		    })
    });
    capAvailable.render('availableCap');

    var capSelected = new Ext.tree.CapPanel({
        id: 'pnlCapSelected',
        title: Ext.LabelKey.Admin_Pageflow_pnlCapSelectedTitle,
        animate: true,
        style: 'padding:10px 10px 20px 20px',
        height: 300,
        lines: false,
        root: new Ext.tree.AsyncTreeNode({
            id: 'rootCapSelected',
            childNodes: {}
        }),
        rootVisible: false,
        enableDD: true,
        enableDrop: true,
        containerScroll: true,
        autoScroll: true,
        dropConfig: { allowContainerDrop: true },
        selModel: new Ext.tree.MultiSelectionModel()//FixedMultiSelectionModel()
    });
    capSelected.render('selectedCap');

    var dropTree1 = new Ext.dd.DropTarget(capAvailable.el.dom.childNodes[1], {
        ddGroup: 'TreeDD',
        notifyDrop: function (dd, e, data) {
            OnNodeDrop(dd, e, data, capAvailable);
        }
    });
    var dropTree2 = new Ext.dd.DropTarget(capSelected.el.dom.childNodes[1], {
        ddGroup: 'TreeDD',
        notifyDrop: function (dd, e, data) {
            return OnNodeDrop(dd, e, data, capSelected);
        }
    });

    new Ext.tree.TreeSorter(capAvailable, { folderSort: true });
    new Ext.tree.TreeSorter(capSelected, { folderSort: true });

    capAvailable.on('click', function (node, e) {
        btnAddCap.enable();
    });

    capAvailable.on('remove', function () {
        if (capAvailable.getRootNode().childNodes.length < 1) {
            btnAddCap.disable();
        }
    });

    capSelected.on('click', function () {
        btnRemove.enable();
    });

    capSelected.on('append', function () {
        btnAddCap.disable();
    });

    capSelected.on('remove', function () {
        if (capSelected.getRootNode().childNodes.length < 1) {
            btnRemove.disable();
        }
    });

    btnRemove.on('click', function (e) {
        var selectedNodes = capSelected.getSelectionModel().getSelectedNodes();
        var availableRoot = capAvailable.getRootNode();
        var selectedRoot = capSelected.getRootNode();

        while (selectedNodes.length > 0) {
            availableRoot.appendChild(selectedNodes[0].remove());
        }

        btnRemove.disable();
        document.changed = true;
        CheckNewGroupCondition();
    });

    btnAddCap.on('click', function () {
        var selectedNodes = capAvailable.getSelectionModel().getSelectedNodes();
        var selectedRoot = capSelected.getRootNode();

        AddCapAssociation(selectedRoot, selectedNodes);

        document.changed = true;
        CheckNewGroupCondition();
    });

    //    btnCancel.on('click',CancelEdit);
};

function OnNodeDrop(dd, e, data, targetTree) {
    if (dd.tree == targetTree) {
        e.cancel = true;
        return false;
    }

    //To resolve the ExtJs bug, the notifyDrop is called twice
    if (!dd.DDM.dragCurrent.dragging) {
        if (e.target.lastChild && e.target.lastChild.nodeName == 'UL') {
            return false;
        }
    }

    var confirmed = true;
    var attributes = data.node.attributes;

    if (attributes.binded && targetTree.id != 'pnlCapAvailable') {
        confirmed = confirm(String.format(Ext.LabelKey.Admin_Pageflow_Message_RemoveCapAssociation, JsonDecode(attributes.text)));
    }

    if (confirmed) {
        targetTree.getRootNode().appendChild(dd.tree.getRootNode().removeChild(data.node));
        document.changed = true;
        CheckNewGroupCondition();
        return true;
    }
    else {
        return true;
    }
}

function LoadData(groupCode) {
    Ext.getCmp('ddlSmartchoiceGroup').setValue(groupCode);
    Ext.get('txtMode').dom.value = 'Edit';
    if (groupCode.length > 0) {
        ShowSummary(true);
    }
    else {
        ShowSummary(false);
    }

    Ext.getCmp('lnkShowCap').displayShowLink();

    Ext.get('capList').dom.style.display = 'none';
    Ext.get('capAssociation').dom.style.display = 'none';
    Ext.get('pageflow').dom.style.display = 'block';

    LoadPageflowGroup(groupCode);
    RefreshCapList(groupCode);
    RefreshCapAssociation(groupCode, 'load');
}

function ShowSummary(show) {
    if (show) {
        Ext.getCmp('lblCapSummary').show();
        Ext.getCmp('lnkShowCap').show();
    }
    else {
        Ext.getCmp('lblCapSummary').hide();
        Ext.getCmp('lnkShowCap').hide();
    }
}

/******************************/

/*Load and save a pageflow*/
/****************************/
function LoadPageflowGroup(pageflowCode) {
    var txtSmartchoiceName = Ext.getCmp('txtNewSmartchoiceGroupName');
    txtSmartchoiceName.setValue(pageflowCode);

    Ext.Ajax.request({
        method: "post",
        url: '../Pageflow/WorkflowContent.aspx',
        params: { action: 'GetPageFlowGroup', pageFlowGrpCode: pageflowCode, moduleName: document.moduleName },
        success: LoadPageflowCallback,
        headers: { 'Content-Type': 'application/json;utf-8'}//json tag should be added other xml is returned.
    });
}
function LoadPageflowCallback(response) {
    var pageflowGroup = eval('(' + response.responseText + ')');
    var errCode = "";
    if (response.responseText == "-1") {
        errCode = "-1";
    }
    if (errCode == "") {
        Ext.getCmp('pnlPageflow').loadPageflow(pageflowGroup.stepList);
    }
    else {
        Ext.getCmp('pnlPageflow').loadPageflow(errCode);
    }

    var chkDisplayReCertification = document.getElementById("chkDisplayReCertification");

    if (pageflowGroup.displayReCertification == "Y" && typeof (chkDisplayReCertification) != "undefined") {
        chkDisplayReCertification.checked = true;
    } else {
        chkDisplayReCertification.checked = false;
    }

    var count = pageflowGroup.capTypeNameList != null ? pageflowGroup.capTypeNameList.length : 0;
    
    RefreshHeader(count);
}

function DeletePageflow(btn) {
    if (btn != null && btn == 'ok') {
        var txtSmartchoiceGroupName = Ext.getCmp('txtNewSmartchoiceGroupName');
        var groupCode = txtSmartchoiceGroupName.getValue().trim();

        Ext.Ajax.request({
            method: "post",
            url: '../Pageflow/WorkflowContent.aspx',
            params: { action: 'DeletePageFlowGroup', pageFlowGrpCode: groupCode, moduleName: document.moduleName },
            success: DeletePageflowCallback,
            headers: { 'Content-Type': 'application/json;utf-8'}//json tag should be added other xml is returned.
        });
    }
}

function DeletePageflowCallback(response) {
    Ext.get('pageflow').dom.style.display = 'none';
    //Ext.getCmp('pnlPageflow').hide();
    Ext.get('capList').dom.style.display = 'none';
    Ext.get('capAssociation').dom.style.display = 'none';

    var ddlPageflow = Ext.getCmp('ddlSmartchoiceGroup');
    ddlPageflow.store.reload();
    ddlPageflow.setValue('');

    ShowSummary(false);
}

var pageParent = null;

// the mapping for component control id to display order
var cptCtrIdMappingOrder = null;

//If save failed or aborted, return false!
function SavePageFlowGroup() {
    if (!document.changed) { return; }

    pageParent = window.parent;
    var mode = Ext.get('txtMode').dom.value;
    var txtSmartchoiceGroupName = Ext.getCmp('txtNewSmartchoiceGroupName');
    var groupCode = txtSmartchoiceGroupName.getValue().trim();

    if (groupCode.length > 40) {
        Ext.Msg.alert('', Ext.LabelKey.Admin_Pageflow_Message_InvalidSmartchoiceLength);
        return false;
    }

    if (CheckExistingGroup(mode, groupCode)) {
        //txtSmartchoiceGroupName.markInvalid(Ext.LabelKey.Admin_Pageflow_Message_InvalidSmartchoice);
        window.parent.RemoveMark(true);
        document.changed = false;
        //Ext.Msg.alert('', Ext.LabelKey.Admin_Pageflow_Message_InvalidSmartchoice);
        return false;
    }

    var jsonStr = new StringBuffer();
    jsonStr.append('{');

    jsonStr.append('"capTypeNameList":[');
    var nodes = Ext.getCmp('pnlCapSelected').getNodes();
    var nodeJson = '';
    for (var i = 0; i < nodes.length; i++) {
        nodeJson = nodeJson + '"' + (nodes[i].attributes.masterLangCapTypeName) + '",';
    }
    nodeJson = nodeJson.toString();
    if (nodeJson.length > 0) {
        nodeJson = nodeJson.substring(0, nodeJson.length - 1);
    }
    jsonStr.append(nodeJson);
    jsonStr.append('],');

    jsonStr.append('"pageFlowGrpCode":"');
    jsonStr.append(JsonEncode(groupCode));
    jsonStr.append('",');

    jsonStr.append('"displayReCertification":"');
    var chkDisplayReCertification = document.getElementById("chkDisplayReCertification");

    if (typeof (chkDisplayReCertification) != "undefined" && chkDisplayReCertification.checked) {
        jsonStr.append("Y");
    } else {
        jsonStr.append("");
    }

    jsonStr.append('",');

    jsonStr.append('"pageFlowType":"PERMIT",');
    jsonStr.append('"serviceProviderCode":"",');

    jsonPageflow = Ext.getCmp('pnlPageflow').savePageflow();

    if (jsonPageflow == '-1') {
        return false;
    }
    jsonStr.append(jsonPageflow);

    jsonStr.append('}');
    if (!Ext.isIE) {
        window.parent.Ext.MessageBox.hide();
    }

    // if the contact type list not change, set it as empty, so it need NOT do [Update] action in webservice.
    if (!document.contactTypeListChanged) {
        _contactTypeMapping = null;
    } else {
        // mapping the component control id to display order
        for (var cptCtrId in _contactTypeMapping) {
            _contactTypeMapping[cptCtrId].displayOrder = cptCtrIdMappingOrder[cptCtrId];
        }
    }

    var contactTypeString = _contactTypeMapping == null ? "" : JSON.stringify(_contactTypeMapping);

    if (!document.documentTypeListChanged) {
        _documentTypeMapping = null;
    } else {
        // mapping the component control id to display order
        for (var cptCtrId in _documentTypeMapping) {
            _documentTypeMapping[cptCtrId].displayOrder = cptCtrIdMappingOrder[cptCtrId];
        }
    }

    // Sync document type setting changes to the data model "_documentTypeMapping".
    var documentTypeString;
    
    if(_documentTypeMapping == null) {
        documentTypeString = "";
    } else {
        SyncToDocumentTypeMapping();
        documentTypeString = JSON.stringify(_documentTypeMapping);
    }

    Ext.Ajax.request({
        method: "post",
        url: '../Pageflow/WorkflowContent.aspx',
        params: { action: 'SavePageFlowGroup', json: jsonStr.toString(), mode: mode, moduleName: document.moduleName, contactTypeList: contactTypeString, documentTypeList: documentTypeString },
        success: SaveSuccess,
        failure: SaveFail,
        argument: { PageflowCode: groupCode, AssociatedCaps: nodes.length, parentPage: window.parent },
        headers: { 'Content-Type': 'application/json;utf-8'}//json tag should be added other xml is returned.
    });

    return true;
}

function SaveFail() {
    pageParent.Ext.MessageBox.hide();
    Ext.MessageBox.show({
        title: 'An error has occurred.',
        msg: 'We are experiencing technical difficulties.<br />Please try again later or contact the City for assistance.',
        width: 400,
        progressText: '',
        icon: Ext.MessageBox.ERROR,
        closable: true
    });
}

function SaveSuccess(response) {
    var groupCode = response.argument.PageflowCode;
    var associatedCapsCount = response.argument.AssociatedCaps;
    var parentPage = response.argument.parentPage;
    parentPage.Ext.MessageBox.hide();
    //Ext.MessageBox.alert(Ext.LabelKey.Admin_Pageflow_Message_Save);

    if (Ext.get('txtMode').dom.value == 'New') {
        var ddlPageflow = Ext.getCmp('ddlSmartchoiceGroup');
        ddlPageflow.store.reload();
        ddlPageflow.setValue(groupCode);
    }

    RefreshHeader(associatedCapsCount);
    RefreshCapList(groupCode);
    //RefreshCapAssociation(groupCode);

    Ext.get('capList').dom.style.display = 'block';
    Ext.get('pageflow').dom.style.display = 'none';
    //Ext.getCmp('pnlPageflow').hide();
    Ext.get('capAssociation').dom.style.display = 'none';
    Ext.get('txtMode').dom.value = 'Edit';
    Ext.getCmp('lnkShowCap').displayHideLink();

    ShowSummary(true);
    document.changed = false;
    _documentTypeMapping = null;
    _contactTypeMapping = null;

    parentPage.RemoveMark();

    var pane = Ext.getCmp("myWorkPanel");
    if (!pane.items) {
        return;
    }

    GetDocTypeOptConfigHistory(groupCode);
    LoadPageflowGroup(groupCode);
}

function GetDocTypeOptConfigHistory(groupCode) {
     Ext.Ajax.request({
        method: "post",
        url: '../Pageflow/WorkflowContent.aspx',
        params: { action: 'GetDocTypeOptConfigHistory', pageFlowGrpCode: groupCode, moduleName: document.moduleName },
        success: CallBackDocTypeOptConfigHistory,
        headers: { 'Content-Type': 'application/json;utf-8' }
    });
}

function ValidateRequiredDocumentTypes(compNameList) {
    // If has an empty attachment section in the page flow, the valudation should be passed.
    if(IsExistEmtpyAttachmentComponent(compNameList)) {
        return null;
    }

    var missRequiredDocTypes = [];

    for (var recordTypeValue in requiredDocumentTypeStore) {
        /*
        Gets the required document type list
        Data structure: [{Key: document type, Value: res document type}]
        */
        var requiredDocTypesPerCapType = requiredDocumentTypeStore[recordTypeValue];

        if (requiredDocTypesPerCapType == null) {
            continue;
        }

        /*
        Check if there is an attachment section does not configure any settings for specified record type.
        if ture, the validation for the specified record type will be passed.
        */
        if(IsExistComponentNotDocTypeConfig(recordTypeValue, compNameList)) {
            continue;
        }

        var selectedDocTypes = GetSelectedDocumentTypes(recordTypeValue, compNameList);
        var missedDocTypes = [];

        for (var requiredType in requiredDocTypesPerCapType) {
            if (!selectedDocTypes.contains(requiredType)) {
                var resRequiredType = requiredDocTypesPerCapType[requiredType];
                missedDocTypes.push(resRequiredType);
            }
        }

        if (missedDocTypes.length > 0) {
            missRequiredDocTypes.push(
                { recordType: GetAliasOrRecordTypeLabel(recordTypeValue),
                  missedRequiredDocTypes: missedDocTypes
                });
        } 
    }

    return missRequiredDocTypes;
}

/*
Gets a value indicating whether exists empty attachment section in the list.
Empty means there not any document type setting on the attachment component.
*/
function IsExistEmtpyAttachmentComponent(compNameList) {

    for (var index = 0; index < compNameList.length; index++) {
        var compName = compNameList[index];

        var configs = _docTypeOptionConfigStore[compName];

        if (configs == null) {
            return true;
        }
    }

    return false;
}

function GetAliasOrRecordTypeLabel(recordType) {
    if (capTypeStore != null && capTypeStore.data.items.length > 0) {
        for (var index = 0; index < capTypeStore.data.items.length; index++) {
            if (JsonDecode(capTypeStore.data.items[index].data.Value) == recordType) {
                return JsonDecode(capTypeStore.data.items[index].data.Text);
            }
        }
    }

    return recordType;
}

function GetRecordTypeValueByAlias(alias) {
    if (capTypeStore != null && capTypeStore.data.items.length > 0) {
        for (var index = 0; index < capTypeStore.data.items.length; index++) {
            if (JsonDecode(capTypeStore.data.items[index].data.Text) == alias) {
                return JsonDecode(capTypeStore.data.items[index].data.Value);
            }
        }
    }

    return alias;
}

function CheckExistingGroup(mode, groupCode) {
    if (mode == 'Edit') {
        return false;
    }

    var store = Ext.getCmp('ddlSmartchoiceGroup').store;
    var groupCode = JsonEncode(groupCode);
    var isExist = false;
    for (var i = 0; i < store.getCount(); i++) {
        if (store.getAt(i).data['Name'].toUpperCase() == groupCode.toUpperCase()) {
            isExist = true;
            break;
        }
    }

    return isExist;
}
/****************************/


function RefreshHeader(count) {
    var lblSummary = Ext.getCmp('lblCapSummary');
    var reg = /\d+/;

    lblSummary.setText(lblSummary.text.replace(reg, count));
}

function RefreshCapAssociation(groupCode, callbackAction) {
    if (groupCode == '') {
        Ext.getCmp('btnOK').setDisabled(true);
    }

    document.getElementById("chkDisplayReCertification").checked = false;

    Ext.Ajax.request({
        method: "post",
        url: '../Pageflow/WorkflowContent.aspx',
        params: { action: 'GetRelatedCaps', pageFlowGrpCode: groupCode, moduleName: document.moduleName },
        success: InitAssociatedCaps,
        argument: { callbackAction: callbackAction },
        headers: { 'Content-Type': 'application/json;utf-8'}//json tag should be added other xml is returned.
    });

    // load document type option history
    GetDocTypeOptConfigHistory(groupCode);

    //capTypeStore reload
    getCapTypeDataSource(groupCode);
    //reset require document type store
    requiredDocumentTypeStore = {};
}

function InitAssociatedCaps(response) {
    var caps = eval("(" + response.responseText + ")");
    var action = response.argument.callbackAction;

    document.availableCaps = caps.availableCaps;
    document.selectedCaps = caps.selectedCaps;

    CreateAvailableTree();
    CreateSelectedTree();

    if (action == 'New') {
        InitNewPageFlowUI();
    }
    else if (action == 'Edit') {
        Ext.get('capList').dom.style.display = 'none';
        Ext.get('capAssociation').dom.style.display = 'block';
        Ext.getCmp('txtNewSmartchoiceGroupName').setDisabled(true);
        Ext.get('txtMode').dom.value = 'Edit';
        Ext.getCmp('btnOK').disable();
        ShowSummary(false);
    }

    AdjustCapPanel();
}

function CreateAvailableTree() {
    var availableTree = Ext.getCmp('pnlCapAvailable');
    var selectedTree = Ext.getCmp('pnlCapSelected');

    var availableRoot = availableTree.getRootNode();
    var selectedRoot = selectedTree.getRootNode();

    if (document.availableCaps != null && document.availableCaps.length > 0) {
        if (availableRoot.childNodes.length == 0) {
            availableTree.createTree(document.availableCaps, '');
        }

        else {
            // Append the selected CAP types to the avaliable CAP type list. 
            // So the available CAP type list contains all CAP types.
            if (selectedRoot.childNodes.length > 0) {
                var i = 0;

                while (selectedRoot.childNodes.length > 0) {
                    availableRoot.appendChild(selectedRoot.childNodes[i].remove());
                }
            }
        }
    }

    // Remove the current selected CAP types from the available CAP type list.
    if (document.selectedCaps != null) {
        for (var i = 0; i < document.selectedCaps.length; i++) {
            for (var j = 0; j < availableRoot.childNodes.length; j++) {
                if (document.selectedCaps[i].text == availableRoot.childNodes[j].text) {
                    availableRoot.childNodes[j].remove();

                    break;
                }
            }
        }
    }
}

function CreateSelectedTree() {
    var selectedTree = Ext.getCmp('pnlCapSelected');
    selectedTree.clear();
    if (document.selectedCaps != null && document.selectedCaps.length > 0) {
        selectedTree.createTree(document.selectedCaps, '');
    }
}

function RestoreSelectedCaps() {
    var nodes = Ext.getCmp('pnlCapSelected').getNodes();
    var availableRoot = Ext.getCmp('pnlCapAvailable').getRootNode();
    var selectedNodes = document.selectedCaps;

    var existed = false;
    var i = 0;
    while (nodes.length > 0 && nodes[i] != null) {
        for (var j = 0; j < selectedNodes.length; j++) {
            if (nodes[i].text == selectedNodes[j].text) {
                existed = true;
                break;
            }
        }

        if (!existed) {
            availableRoot.appendChild(nodes[i].remove());
            i--;
        }

        i++;
        existed = false;
    }
}

function RefreshCapList(groupCode) {
    var grid = Ext.getCmp('grdCapTypes');

    if (groupCode != null && groupCode != '') {
        capStore = new Ext.data.JsonStore({
            url: '../Pageflow/WorkflowContent.aspx',
            baseParams: {
                action: 'GetRelatedCapList',
                pageFlowGrpCode: groupCode,
                moduleName: document.moduleName
            },
            root: 'CapTypes',
            fields: ['CapName', 'Alias']
        });
        capStore.load();

        grid.reconfigure(capStore, grid.getColumnModel());
        grid.getView().refresh();
    }
    else {
        ClearCapList();
    }
}

function ClearCapList() {
    var grid = Ext.getCmp('grdCapTypes');
    var emptyCapStore = new Ext.data.SimpleStore({ data: [], fields: ['capName', 'alias'] });
    grid.reconfigure(emptyCapStore, grid.getColumnModel());
    grid.getView().refresh();
}

function ClearPageflow() {
    var pageflow = Ext.getCmp('pnlPageflow');
    pageflow.loadPageflow(null);
}

function CheckNewGroupCondition() {
    var btnOK = Ext.getCmp('btnOK');
    var txtSmartchoice = Ext.getCmp('txtNewSmartchoiceGroupName');
    //var pnlCap = Ext.getCmp('pnlCapSelected');

    //Accela requires a flow needs to be created without selecting any cap
    //if(txtSmartchoice.getValue().trim().length>0 && txtSmartchoice.isValid && pnlCap.getNodesCount()>0 && document.changed){
    if (txtSmartchoice.getValue().trim().length > 0 && txtSmartchoice.isValid && document.changed) {
        btnOK.enable();
        changeSaveStatus();
    }
    else {
        btnOK.disable();
    }
}

function AddCapAssociation(root, selectedNodes) {
    var confirmed = true;

    var i = 0;
    while (selectedNodes.length > 0 && selectedNodes[i] != null) {
        if (selectedNodes[i].attributes.binded) {
            confirmed = confirm(String.format(Ext.LabelKey.Admin_Pageflow_Message_RemoveCapAssociation, JsonDecode(selectedNodes[i].attributes.text)));
        }

        if (confirmed) {
            root.appendChild(selectedNodes[i].remove());
            i--;
        }

        i++;
    }

    CheckNewGroupCondition();

    if (selectedNodes.length < 1) {
        Ext.getCmp('btnAddCap').disable();
    }

    return confirmed;
}

function getparmByUrl(parmName) {
    var url = window.location.toString();
    if (url.indexOf("?") == -1) {
        return "";
    }
    var arr = url.split("?");
    var parms = arr[1];
    var parmList = parms.split("&");
    var parmTemp;

    for (var i = 0; i < parmList.length; i++) {
        parmTemp = parmList[i].split("=");
        if (parmTemp[0] == parmName)
            return parmTemp[1];
    }
    return "";
}
/*'CONTACT TYPE','APPLICANT RELATION'*/
function getStandardChoice(stdCatName) {
    var standardchoiceStore = new Ext.data.SimpleStore({
        url: '../Pageflow/WorkflowContent.aspx',
        baseParams: {
            action: 'GetStandardChoice',
            standardChoiceName: stdCatName
        },
        fields: ['Name']
    });
    standardchoiceStore.load();

    return standardchoiceStore;
}

function getDataSource(params) {
    var store = new Ext.data.SimpleStore({
        url: '../Pageflow/WorkflowContent.aspx',
        baseParams: params,
        fields: ['Name']
    });
    store.load();

    return store;
}

function getASISubGroupDataSource(params) {
    var store = new Ext.data.SimpleStore({
        url: '../Pageflow/WorkflowContent.aspx',
        baseParams: params,
        fields: ['Text', 'Value']
    });
    store.load();

    return store;
}

function getCapTypeDataSource(pageflowCode) {
    $.ajax({
        method: "post",
        async: false,
        cache: false,
        url: '../Pageflow/WorkflowContent.aspx',
        data: { action: 'GetCapTypeGroups', pageFlowGrpCode: pageflowCode, moduleName: document.moduleName },
        success: function (response) {
            var capTypes = [];

            if (response != "") {
                capTypes = eval(response);
            }

            capTypeStore.loadData(capTypes, false);
        },
        headers: { 'Content-Type': 'application/json;utf-8'}
    });
}

function getRequireDocumentTypeDataSource(params) {
    var store = new Ext.data.SimpleStore({
        url: '../Pageflow/WorkflowContent.aspx',
        baseParams: params,
        fields: []
    });

    store.load();

    return store;
}

function EncodeSpecialChar(json) {
    if (!json || json == "") return "";
    if (!isNaN(json)) {
        return json;
    }
    return json.replace(/\[/g, "\\\[").replace(/\]/g, "\\\]").replace(/\{/g, "\\\{").replace(/\}/g, "\\\}").replace(/\"/g, "\\\"").replace(/\'/g, "\\\'").replace(/\,/g, "\\\,").replace(/\</g, "\\\<").replace(/\>/g, "\\\>").replace(/\//g, "\\\/").replace(/\^/g, "\\\^").replace(/\n|\r|(\r\n)/g, '\\n');
}

//******************************************************************************
// Create Final Page
//******************************************************************************
function CreateFinalStep() {
    var finalStep = new Ext.Panel({
        cls: 'x-StepPanel',
        layout: 'column',
        id: 'finalStep',
        columnWidth: .99,
        title: 'Final Step'
    });

    finalStep.on('render', function () {
        this.body.addClass("x-StepPanel-LeftSpace");
    });

    //create the Review Certification page.
    var reviewPageContainer = new Ext.ux.PageContainer();

    var reviewPage = new Ext.Panel({
        title: "Review Page",
        cls: 'x-PagePanel',
        draggable: false,
        items: [CreateReviewPageContent()]
    });

    reviewPageContainer.add(reviewPage);
    finalStep.add(reviewPageContainer);

    //create the Receipt page.
    var finalPageContainer = new Ext.ux.PageContainer();

    var finalPage = new Ext.Panel({
        title: "Receipt Page",
        cls: 'x-PagePanel',
        draggable: false,
        items: [CreateFinalPageContent()]
    });

    finalPageContainer.add(finalPage);
    finalStep.add(finalPageContainer);

    return finalStep;
}

// pageId: module index + page id + 'Y' + module name + element name
function ReceiptPageObj(url, elementId, pageName) {
    this.pageUrl = url;
    var elementName = pageName.replace(/\s+/g, '');  // remove the space for element name
    this.pageId = [elementId, 'Y', document.moduleName, elementName].join('\f');
    this.pageName = pageName;
    this.linkText = String.format("Configure {0} page", pageName.toLowerCase());
}

// pageId: module index + page id + 'Y' + module name + element name
function ReviewPageObj(url, elementId, pageName) {
    this.pageUrl = url;
    var elementName = pageName.replace(/\s+/g, '');  // remove the space for element name
    this.pageId = [elementId, 'Y', document.moduleName, elementName].join('\f');
    this.pageName = pageName;
}

function CreateReviewPageContent() {
    var currentTabId = parent.parent.Ext.Const.OpenedId.split('\f');
    var moduleIndex = currentTabId[0].substring(0, currentTabId[0].length - 4); // The last 4 numbers is page id.

    var reviewUrl = String.format("pageflow/capreviewcertification.aspx?isAdmin=Y", document.moduleName, document.moduleName);
    var reviewPage = new ReviewPageObj(reviewUrl, moduleIndex + "CertificationPage", "Certification");

    var reviewPageArray = [reviewPage];
    var divLinks = document.createElement('div');
    divLinks.className = 'x-reviewPage-links';

    for (var i = 0; i < reviewPageArray.length; i++) {
        var divLink = document.createElement('div');
        divLink.style.marginBottom = '8px';

        var chkBox = document.createElement("input");
        chkBox.setAttribute("type", "checkbox");
        chkBox.setAttribute("id", "chkDisplayReCertification");
        chkBox.onclick = function () {
            changeSaveStatus();
        };

        var label = document.createElement('label');
        label.setAttribute("for", "chkDisplayReCertification");
        label.innerHTML = "Enable Certification";

        var link = document.createElement('A');
        link.style.marginLeft = "5px";
        link.onmousemove = function () { this.style.cursor = 'pointer'; };
        var receiptPageObj = reviewPageArray[i];

        with (receiptPageObj) {
            link.innerHTML = "Configure";
            link.onclick = function () {
                var groupCode = Ext.getCmp('ddlSmartchoiceGroup').getValue();
                parent.parent.OpenTab(pageUrl, pageId, pageName, document.moduleName, 'Y', groupCode);
            };
        }

        divLink.appendChild(chkBox);
        divLink.appendChild(label);
        divLink.appendChild(link);
        divLinks.appendChild(divLink);
    }

    var divContainer = document.createElement('div');
    divContainer.className = 'x-reviewPage-container';
    divContainer.appendChild(divLinks);

    return divContainer;
}

function CreateFinalPageContent() {
    var currentTabId = parent.parent.Ext.Const.OpenedId.split('\f');
    var moduleIndex = currentTabId[0].substring(0, currentTabId[0].length - 4); // The last 4 numbers is page id.

    var receiptUrl = String.format("../cap/capcompletion.aspx?stepnumber=5&tabname={0}&module={1}&isAdmin=Y", document.moduleName, document.moduleName);
    var receiptPage = new ReceiptPageObj(receiptUrl, moduleIndex + "1043", "Receipt");

    //url: ../Cap/CapCompletion.aspx?stepNumber=5&tabname=&module=&isRenewal=Y
    var renewalPage = new ReceiptPageObj(receiptUrl + '&isrenewal=y', moduleIndex + "1078", "Renewal Receipt");

    //url: ../Cap/CapCompletion.aspx?stepNumber=5&tabname=&module=&isPay4ExistingCap=Y
    var pageFeeDuePage = new ReceiptPageObj(receiptUrl + '&isPay4ExistingCap=Y', moduleIndex + "1079", "Pay Fee Due Receipt");

    var receiptPageArray = [receiptPage, renewalPage, pageFeeDuePage];
    var divLinks = document.createElement('div');
    divLinks.id = "divReceiptPageLinks";
    divLinks.className = 'x-finalPage-links';

    for (var i = 0; i < receiptPageArray.length; i++) {
        var divLink = document.createElement('div');
        divLink.style.marginBottom = '8px';
        var link = document.createElement('A');
        link.onmousemove = function () { this.style.cursor = 'pointer'; };
        var receiptPageObj = receiptPageArray[i];

        with (receiptPageObj) {
            link.innerHTML = linkText;
            link.onclick = function() {
                OpenReceiptPage(pageUrl, pageId, pageName);
            };
        }

        divLink.appendChild(link);
        divLinks.appendChild(divLink);
    }

    var divContainer = document.createElement('div');
    divContainer.className = 'x-finalPage-container';
    divContainer.appendChild(divLinks);

    var img = document.createElement('img');
    img.setAttribute('src', '../images/info.gif');
    divContainer.appendChild(img);

    var p = document.createElement('p');
    p.innerHTML = 'When the shopping cart / super agency is used customized receipts will not be shown.';
    divContainer.appendChild(p);

    return divContainer;
}

function OpenReceiptPage(url, tabId, tabName) {
    //OpenTab(pageUrl, tabId, tabName, module, isUsedDaily, pageFlow)
    var groupCode = Ext.getCmp('ddlSmartchoiceGroup').getValue();
    parent.parent.OpenTab(url, tabId, tabName, document.moduleName, 'Y', groupCode);
}

/* End create final page */

//******************************************************************************
// ==InitWorkPanel:function()====
//******************************************************************************
function InitWorkPanel() {
    newFormWin.render(Ext.getBody());

    var page = new Ext.Panel({
        frame: true, //important
        region: 'center',
        cls: 'x-WorkPanel',
        id: 'pnlPageflow',
        autoScroll: false,
        pane: null, //this item contianer

        listeners: {
            'render': function (obj) {
                obj.el.on('click', function (e, target, panel) {
                    if (target.type !== 'button') {
                        document.oncontextmenu = new Function("return true");
                        obj.setPanelActive();
                    }
                });
            }
        },
        initComponent: function () {
            var obj = this;
            this.initAcaComponents();
            this.tbar = {
                cls: 'x-WorkPanel-tbar',
                items: [{
                    text: ' Add ',
                    iconCls: 'x-add',
                    menu: new Ext.menu.Menu({
                        id: 'mainMenu',
                        items: [{
                            iconCls: 'x-new-step',
                            text: 'Add new step ',
                            handler: function (item) {
                                obj.addStepPanel();
                            }
                        }, {
                            iconCls: 'x-new-page',
                            text: 'Add new page  ',
                            handler: function (item) {
                                obj.addPageContainer();
                            }
                        }]
                    })
                }]
            };
            Ext.ux.WorkPanel.superclass.initComponent.call(this);
        },
        onRender: function (ct, position) {
            Ext.ux.WorkPanel.superclass.onRender.call(this, ct, position);
            this.pane = Ext.getCmp("myWorkPanel");
            if (Ext.isIE)
                this.width = Ext.workflowWidth
            //this.tbar.setVisible(false);
        },
        deletePageCloseBar: function () {
            var curPage = Ext.getCmp(Ext.curPageId);
            curPage.tools.close.setVisible(false);
        },
        setPanelActive: function () {
            if (Ext.oldStepId) {
                //var oldStep=Ext.getCmp(Ext.oldStepId);
                //oldStep.removeClass("x-StepPanel");
                //oldStep.addClass("x-StepPanel-Disable");
            }
            if (Ext.curStepId) {
                var curStep = Ext.getCmp(Ext.curStepId);
                //curStep.addClass("x-StepPanel");
                //curStep.removeClass("x-StepPanel-Disable");
                curStep.setPageToolsStatus();
            }
            if (Ext.oldPageId) {
                var oldPage = Ext.getCmp(Ext.oldPageId);
                //oldPage.removeClass("x-PagePanel");
                $('#' + Ext.oldPageId).css({ 'background-color': 'white', 'filter': 'Alpha(opacity=65)', '-moz-opacity': .60, 'opacity': 0.60 });
                oldPage.body.applyStyles({ 'position': 'absolute' });
                //oldPage.header.applyStyles({'background-image':'url(../images/grid3-hrow-over.gif)'});
                //oldPage.header.applyStyles({'background-image':'url(../images/panel-title-bg.gif)'});
                //oldPage.addClass("x-PagePanel-Disable");
            }
            if (Ext.curPageId) {
                var curPage = Ext.getCmp(Ext.curPageId);
                //curPage.addClass("x-PagePanel");
                $('#' + Ext.curPageId).css({ 'background-color': 'white', 'filter': 'Alpha(opacity=100)', '-moz-opacity': 1, 'opacity': 1 });
                //curPage.header.applyStyles({'background-image':'url(../images/panel-title-bg.gif)'});
                //curPage.header.applyStyles({'background':'gray'});

                //curPage.removeClass("x-PagePanel-Disable");
            }
        },
        addStepPanel: function (cfg) {
            var isFirstStep = false;
            if (arguments.length == 2 && arguments[1] == true) {
                isFirstStep = true;
            }
            if (this.pane.items && this.pane.items.length >= 6) {
                Ext.Msg.alert("Infomation", "no more step can be added.");
                return;
            }
            if (Ext.FirstPropertyTabWidth == 0) {
                Ext.FirstPropertyTabWidth = Ext.getCmp("propertyPanel").getSize().width;
            }
            var step = new Ext.ux.StepPanel();
            if (cfg) {
                step.title = isNullOrEmpty(cfg.resStepName) ? cfg.stepName : cfg.resStepName;
                step.defaultTitle = cfg.stepName;
                step.stepID = cfg.stepID;
            } else {
                var stepIndex = 0;
                if (this.pane.items) {
                    stepIndex = this.pane.items.length;
                }
                var tmpStepName = "Step " + (stepIndex + 1).toString();
                var chk = true;
                var maxNumber = 0;
                if (this.pane.items) {
                    var patt = /^Step (\d+)$/;
                    for (var k = 0; k < this.pane.items.length; k++) {
                        var stepName = this.pane.items.items[k].title;
                        patt.exec(stepName);
                        var num = RegExp.$1;
                        if (num) {
                            if (num > maxNumber) {
                                maxNumber = num;
                            }
                        }
                        if (stepName == tmpStepName) {
                            chk = false;
                        }
                    }
                    //get the max step number
                    if (chk == false) {
                        maxNumber = parseInt(maxNumber) + 1;
                        tmpStepName = "Step " + maxNumber;
                    }
                }

                step.stepID = 0;
                step.title = tmpStepName.replace("Step", Ext.LabelKey.ACA_Pageflow_StepName);
                step.defaultTitle = tmpStepName.replace("Step", Ext.LabelKey.ACA_Pageflow_StepName_DefaultLanguage);
                if (this.pane.items) {
                    step.stepOrder = this.pane.items.length;
                }
            }

            Ext.oldStepId = Ext.curStepId;
            Ext.curStepId = step.id;
            var pane = Ext.getCmp("pnlPageflow");

            this.pane.add(step);
            this.doLayout();
            if (Ext.FirstStep == null) {
                Ext.FirstStep = step;
            }
            this.addPageContainer(cfg, isFirstStep);
            this.setPanelActive();

            this.setStepToolsStatus();
            //if(Ext.loadStep){
            //var region = Ext.getCmp("propertyPanel");	
            //region.show();
            //doLayoutStepPanel(true);
            //}
            Ext.loadStep = false;
            if (Ext.isIE6 && Ext.isFirstLoad) {//fix
                doLayoutStepPanel();
            } else {
                doLayoutStepPanel();
            }

        },
        items: [{
            columnWidth: .99,
            border: false,
            items: [{
                id: 'myWorkPanel',
                border: false
            }]
        }, {
            columnWidth: .99,
            border: false,
            items: [
                CreateFinalStep()
            ]
        }],
        setStepToolsStatus: function () {
            var workpanel = Ext.getCmp("myWorkPanel");
            if (!workpanel.items) {
                return;
            }

            var curStep;
            if (workpanel.items.length == 1) {
                curStep = workpanel.items.items[0];
                curStep.tools.close.setVisible(false);
                curStep.tools.up.setVisible(false);
                curStep.tools.down.setVisible(false);
            } else {
                //set close visible
                for (var i = 0; i < workpanel.items.length; i++) {
                    curStep = workpanel.items.items[i];
                    curStep.tools.close.setVisible(true);

                    curStep.tools.up.setVisible(true);
                    curStep.tools.down.setVisible(true);

                    if (i == 0) {
                        curStep.tools.up.setVisible(false);
                    }
                    if (i == (workpanel.items.length - 1)) {
                        curStep.tools.down.setVisible(false);
                    }
                }
            }
        },
        addPageContainer: function (cfg) {
            var isFristPage = false;
            if (arguments.length == 2 && arguments[1] == true) {
                isFristPage = true;
            }

            var step = Ext.getCmp(Ext.curStepId);
            var pane = Ext.getCmp("pnlPageflow");
            if (!step) {
                Ext.Msg.alert("Infomation", "a step must be added first");
                return;
            }
            var pageLen = 1;
            var len = 0;
            if (cfg) {
                pageLen = cfg.pageList.length;
            }

            for (var i = 0; i < pageLen; i++) {
                var newContainer = new Ext.ux.PageContainer;
                var newPage = new Ext.ux.PagePanel;

                if (cfg) {
                    newPage.title = isNullOrEmpty(cfg.pageList[i].resPageName) ? cfg.pageList[i].pageName : cfg.pageList[i].resPageName;
                    newPage.defaultTitle = cfg.pageList[i].pageName;
                    newPage.pageID = cfg.pageList[i].pageID;
                    newPage.onloadEventName = isNullOrEmpty(cfg.pageList[i].onloadEventName) == true ? DEFALUT_EVENT_NAME : cfg.pageList[i].onloadEventName;
                    newPage.beforeClickEventName = isNullOrEmpty(cfg.pageList[i].beforeClickEventName) == true ? DEFALUT_EVENT_NAME : cfg.pageList[i].beforeClickEventName;
                    newPage.afterClickEventName = isNullOrEmpty(cfg.pageList[i].afterClickEventName) == true ? DEFALUT_EVENT_NAME : cfg.pageList[i].afterClickEventName;
                    newPage.defaultLangInstruction = cfg.pageList[i].instruction;
                    newPage.instruction = Ext.IsSupportMultiLanguage ? cfg.pageList[i].resInstruction : cfg.pageList[i].instruction;
                }
                else {
                    if (step.items) {
                        len = step.items.length;
                        var maxNum = 0;
                        var newPageIndex = Math.max(1, len - 1);
                        var tempPageTitle = Ext.LabelKey.ACA_Pageflow_PageName + " " + newPageIndex;
                        var existTitle = false;
                        var patten = new RegExp("^" + Ext.LabelKey.ACA_Pageflow_PageName + " (\\d+)$");

                        // get the max page title's index in current step to avoid the same page name when add new page.
                        for (var pageIndex = 0; pageIndex < len - 1; pageIndex++) {
                            if (!step.items.items[pageIndex].items) {
                                continue;
                            }

                            var pageTitle = step.items.items[pageIndex].items.items[0].title;
                            patten.exec(pageTitle);
                            var index = RegExp.$1;
                            maxNum = isNumber(index) ? Math.max(maxNum, index) : maxNum;

                            if (tempPageTitle == pageTitle) {
                                existTitle = true;
                            }
                        }

                        if (existTitle) {
                            maxNum = maxNum + 1;
                            tempPageTitle = Ext.LabelKey.ACA_Pageflow_PageName + " " + maxNum;
                        }

                        newPage.title = tempPageTitle;
                        newPage.defaultTitle = tempPageTitle.replace(Ext.LabelKey.ACA_Pageflow_PageName, Ext.LabelKey.ACA_Pageflow_PageName_DefaultLanguage);
                    }

                    newPage.pageID = 0;
                    newPage.onloadEventName = DEFALUT_EVENT_NAME;
                    newPage.beforeClickEventName = DEFALUT_EVENT_NAME;
                    newPage.afterClickEventName = DEFALUT_EVENT_NAME;
                    newPage.defaultLangInstruction = "";
                    newPage.instruction = "";
                    var htm = new Ext.ux.PageHTML();
                    newPage.add(htm);
                }

                newContainer.insert(0, newPage);

                Ext.oldPageId = Ext.curPageId;
                Ext.curPageId = newPage.id;

                var insertPlace = i + 1;
                if (!cfg) {
                    insertPlace = len - 1;
                }
                step.insert(insertPlace, newContainer);

                var tab = Ext.getCmp(COMPONENT_TAB_ID);
                tab.activate(Component_Property_Tab_Id);
                //reset page visible
                if (step.items.length > PAGE_SHOW_IN_STEP + 2) {
                    step.firstPageIndex = step.items.length - 2 - PAGE_SHOW_IN_STEP;
                }
                for (var kk = 0; kk < step.items.length; kk++) {
                    var container = step.items.items[kk];
                    if (container.getXType() != "PageContainer")
                        continue;
                    if (kk > step.firstPageIndex && kk <= step.firstPageIndex + PAGE_SHOW_IN_STEP) {
                        container.setVisible(true);
                    } else {
                        container.setVisible(false);
                    }
                }

                step.setArrowVisible();
                step.doLayout();
                if (cfg) {//add contianer
                    for (var j = 0; j < cfg.pageList[i].componentList.length; j++) {//for 1
                        var sour = cfg.pageList[i].componentList[j];
                        var componentTab = Ext.getCmp(Component_Property_Tab_Id);
                        for (var k = 0; k < componentTab.items.length; k++) {//for 0
                            var componentID = componentTab.items.items[k].id;
                            var compID = componentID.substring('Component_'.length); //Get the pure number part.
                            if (compID == sour.componentID) {
                                componentTab.items.items[k].setTitle(componentTab.items.items[k].title);
                                componentTab.items.items[k].componentSeqNbr = sour.componentSeqNbr;

                                if (Ext.IsSupportMultiLanguage) {
                                    componentTab.items.items[k].defaultCustomHeading = sour.customHeading;
                                }

                                var compItem = componentTab.items.items[k];

                                if (compItem.isSupportMultiply) {
                                    var tempPanel = new Ext.ux.ComponentPanel;
                                    tempPanel.title = compItem.title;
                                    tempPanel.titleA = compItem.titleA;
                                    tempPanel.maxWidth = compItem.maxWidth;
                                    tempPanel.iconCls = compItem.iconCls;
                                    tempPanel.sortID = compItem.sortID;
                                    tempPanel.componentId = compItem.componentId;
                                    tempPanel.componentSeqNbr = compItem.componentSeqNbr;
                                    tempPanel.isSupportMultiply = compItem.isSupportMultiply;
                                    newPage.add(tempPanel);
                                }
                                else {
                                    newPage.add(componentTab.items.items[k]);
                                }
                                //set component's source
                                var customHeading = sour.resCustomHeading;
                                var isRequired = sour.requiredFlag != "N";
                                var isEditable = sour.editableFlag != "N";
                                var validateFlag = (sour.validateFlag != null && sour.validateFlag != "") ? sour.validateFlag : "N";
                                var asiGroup = Ext.ASIGroupEmptyText;
                                if (sour.portletRange1 != null && sour.portletRange1 != "") {
                                    asiGroup = sour.portletRange1;
                                }

                                var asiSubgroup = Ext.ASIGroupEmptyText;
                                if (sour.portletRange2 != null && sour.portletRange2 != "") {
                                    var asiSubgroup = sour.portletRange2;
                                }

                                // remove space for componentName
                                var componentName = sour.componentName.replace(/(\s*)/g, "");
                                var instruction = Ext.IsSupportMultiLanguage ? sour.resInstruction : sour.instruction;
                                instruction = instruction != null ? instruction : "";

                                // Get the component instruction from DB and set value to data source.
                                if (!compItem.isSupportMultiply && componentName && componentName != "ASI" && componentName != "ASITable") {
                                    ComponentCfg[componentName].source["Instructions"] = instruction;
                                }

                                switch (componentName) {
                                    case "Address":
                                    case "Parcel":
                                    case "Owner":
                                    case "LicensedProfessionalList":
                                        if (customHeading != null && customHeading != "") {
                                            ComponentCfg[componentName].source["Custom Heading"] = customHeading;
                                        }
                                        ComponentCfg[componentName].source.Required = isRequired;
                                        ComponentCfg[componentName].source["Data Source"] = ComponentDataSource[validateFlag];
                                        ComponentCfg[componentName].source.Editable = isEditable;
                                        break;
                                    case "LicensedProfessional":
                                        var contactSource = eval("ComponentCfg." + componentName + ".source");
                                        contactSource[sour.componentSeqNbr] = cloneKeyVaulePair(contactSource[componentID]);

                                        if (customHeading != null && customHeading != "") {
                                            contactSource[sour.componentSeqNbr]["Custom Heading"] = customHeading;
                                        }

                                        contactSource[sour.componentSeqNbr].Required = isRequired;
                                        contactSource[sour.componentSeqNbr].Editable = isEditable;
                                        contactSource[sour.componentSeqNbr]["Data Source"] = ComponentDataSource[validateFlag];
                                        contactSource[sour.componentSeqNbr]["Instructions"] = instruction;
                                        break;
                                    case "Applicant":
                                    case "Contact1":
                                    case "Contact2":
                                    case "Contact3":
                                        var _title = "select...";
                                        if (sour.customHeading != null && sour.customHeading != "") {
                                            _title = sour.customHeading;
                                        }
                                        var contactSource = eval("ComponentCfg." + componentName + ".source");
                                        contactSource[sour.componentSeqNbr] = cloneKeyVaulePair(contactSource[componentID]);

                                        contactSource[sour.componentSeqNbr]["Custom Heading "] = _title;
                                        contactSource[sour.componentSeqNbr].Required = isRequired;
                                        contactSource[sour.componentSeqNbr].Editable = isEditable;
                                        contactSource[sour.componentSeqNbr]["Data Source"] = ComponentDataSource[validateFlag];
                                        contactSource[sour.componentSeqNbr]["Instructions"] = instruction;
                                        break;
                                    case "AdditionalInformation":
                                    case "DetailInformation":
                                    case "ValuationCalculator":
                                        if (customHeading != null && customHeading != "") {
                                            ComponentCfg[componentName].source["Custom Heading"] = customHeading;
                                        }
                                        ComponentCfg[componentName].source.Editable = isEditable;
                                        break;
                                    case "ASI":
                                    case "ASITable":
                                        var asiSource = ComponentCfg.ApplicationSpecificInformation.source;
                                        if (componentName == "ASITable") {
                                            asiSource = ComponentCfg.ApplicationSpecificInformationTable.source;
                                        }

                                        // need clone, or else it use reference as default
                                        asiSource[sour.componentSeqNbr] = cloneKeyVaulePair(asiSource[componentID]);

                                        if (customHeading != null && customHeading != "") {
                                            asiSource[sour.componentSeqNbr]["Custom Heading"] = customHeading;
                                        }
                                        asiSource[sour.componentSeqNbr]["Group"] = asiGroup;

                                        if (!Ext.IsSupportMultiLanguage) {
                                            asiSource[sour.componentSeqNbr]["Subgroup"] = asiSubgroup;
                                        }
                                        else {
                                            var isASIT = (componentName == "ASITable");
                                            var resSubgroup = GetASIResSubgroupBySubgroup(asiGroup, asiSubgroup, isASIT);
                                            asiSource[sour.componentSeqNbr]["Subgroup"] = resSubgroup;
                                        }
                                        break;
                                    case "ContactList":
                                        var contactSource = eval("ComponentCfg." + componentName + ".source");
                                        contactSource[sour.componentSeqNbr] = cloneKeyVaulePair(contactSource[componentID]);

                                        if (customHeading != null && customHeading != "") {
                                            contactSource[sour.componentSeqNbr]["Custom Heading"] = customHeading;
                                        }
                                        contactSource[sour.componentSeqNbr].Required = isRequired;
                                        contactSource[sour.componentSeqNbr].Editable = isEditable;
                                        contactSource[sour.componentSeqNbr]["Data Source"] = ComponentDataSource[validateFlag];
                                        contactSource[sour.componentSeqNbr]["Instructions"] = instruction;
                                        break;

                                    case "Education":
                                    case "ContinuingEducation":
                                    case "Examination":
                                        if (customHeading != null && customHeading != "") {
                                            ComponentCfg[componentName].source["Custom Heading"] = customHeading;
                                        }
                                        ComponentCfg[componentName].source.Required = isRequired;
                                        ComponentCfg[componentName].source.Editable = isEditable;
                                        break;
                                    case "Attachment":
                                        var contactSource = eval("ComponentCfg." + componentName + ".source");
                                        contactSource[sour.componentSeqNbr] = cloneKeyVaulePair(contactSource[componentID]);

                                        if (customHeading != null && customHeading != "") {
                                            contactSource[sour.componentSeqNbr]["Custom Heading"] = customHeading;
                                        }

                                        contactSource[sour.componentSeqNbr]["Instructions"] = instruction;
                                        break;
                                    case "CustomComponent":
                                        var customCptSource = ComponentCfg.CustomComponent.source;

                                        if (customHeading != null && customHeading != "") {
                                            customCptSource["Custom Heading"] = customHeading;
                                        }

                                        customCptSource["Path"] = sour.portletRange1 == null ? "" : sour.portletRange1;
                                        break;
                                    case "ConditionDocument":
                                        var conditionDocumentSource = ComponentCfg.ConditionDocument.source;

                                        if (customHeading != null && customHeading != "") {
                                            conditionDocumentSource["Custom Heading"] = customHeading;
                                        }
                                        break;
                                    case "Assets":
                                        var assetSource = ComponentCfg.Assets.source;
                                        if (customHeading != null && customHeading != "") {
                                            assetSource["Custom Heading"] = customHeading;
                                        }
                                        assetSource.Required = isRequired;
                                        break;
                                    default:
                                        break;
                                }
                                continue;
                            } //end if
                            componentTab.doLayout();
                        } //end for 0
                    } //for 1

                    // remove duplicate components that support multiply in component panel.
                    var removeIndex = 0;
                    while (removeIndex < componentTab.items.length) {
                        var componentItem = componentTab.items.items[removeIndex];

                        // the component.id format is "Component_" + componentId, if its value not accord with the format, it is the duplicate component.
                        if (componentItem.isSupportMultiply && componentItem.id != "Component_" + componentItem.componentId) {
                            componentTab.remove(componentItem, true);
                        }
                        else {
                            removeIndex += 1;
                        }
                    }
                } //cfg
                if (newPage.items.length > 4) {
                    newPage.syncSize();
                }

                newContainer.doLayout();
                newPage.resetComponentWidth();
                pane.setPanelActive();
                changeSaveStatus(isFristPage);
            }
            step.setPageToolsStatus();

            var tab = Ext.getCmp(COMPONENT_TAB_ID);

            if (Ext.isFirstCreatePage) {
                tab.setDisabled(false);
                Ext.isFirstCreatePage = false;
            }
            tab.expand();
            checkTimer();
            step.pageMove("next");
        },
        // initialize the components panel in the right
        initAcaComponents: function () {
            var region = Ext.getCmp("propertyPanel");

            if (region == undefined) {
                return;
            }

            var parTab = new Ext.TabPanel({
                id: COMPONENT_TAB_ID,
                border: false,
                activeTab: 0,
                tabPosition: 'top',
                disabled: true,
                disabledClass: '',
                listeners: {
                    beforetabchange: function () {
                        if (isWinShow) {
                            win.hide();
                        }

                        if (isContactTypeDDLWinShow || isDocumentTypeDDLWinShow) {
                            newFormWin.hide();
                        }
                    }
                }
            });
            region.add(parTab);
            // 1. the components tab in components panel
            parTab.add(new Ext.ux.ComponentListPanel({ autoScroll: true, title: 'Components' }));
            // 2. the property tab in components panel
            parTab.add(
                new Ext.grid.PropertyGrid({
                    title: 'Property',
                    id: 'PropertyGridID2',
                    autoHeight: true,
                    customEditors: {///*'CONTACT TYPE','APPLICANT RELATION'*/
                        "Custom Heading ": new Ext.grid.GridEditor(new Ext.form.ComboBox({
                            store: getStandardChoice('CONTACT TYPE'),
                            displayField: 'Name',
                            valueField: 'Name',
                            editable: false,
                            typeAhead: true,
                            id: 'contact_combobox_id',
                            mode: 'local',
                            listeners: {
                                'select': function (obj, a, b) {
                                    Ext.getCmp('contact_combobox_id').setValue(JsonDecode(obj.value));
                                    if (!Ext.isIE) {
                                        obj.focus();
                                        obj.blur();
                                    } else {
                                        var pobj = this.container.dom.offsetParent;
                                        var ddl = this.container.dom.previousSibling;
                                        this.container.dom.blur();
                                    }
                                    this.fireEvent('blur');
                                    var contactType = JsonDecode(obj.value);
                                    RenderContactTypeGenericTemplate(contactType);
                                }
                            },
                            triggerAction: 'all',
                            emptyText: 'select...',
                            selectOnFocus: true
                        })),
                        "title": new Ext.grid.GridEditor(new Ext.form.ComboBox({
                            store: getStandardChoice('CONTACT TYPE'),
                            displayField: 'Name',
                            valueField: 'Name',
                            editable: false,
                            typeAhead: true,
                            id: 'application_combobox_id',
                            mode: 'local',
                            listeners: {
                                'select': function (obj, a, b) {
                                    this.fireEvent('blur');
                                }
                            },
                            triggerAction: 'all',
                            emptyText: 'select...',
                            selectOnFocus: true
                        })),
                        "Required": new BoolComboBoxGridEditor(),
                        "Data Source": new Ext.grid.GridEditor(new Ext.form.ComboBox({
                            store: InitDataSource(),
                            editable: false,
                            id: 'dataFrom',
                            displayField: 'value',
                            typeAhead: true,
                            mode: 'local',
                            listeners: {
                                'select': function (obj, a, b) {
                                    Ext.getCmp('dataFrom').setValue(JsonDecode(obj.value));
                                    this.fireEvent('blur');
                                }
                            },
                            triggerAction: 'all',
                            emptyText: '',
                            selectOnFocus: true
                        })),
                        "Editable": new BoolComboBoxGridEditor(),
                        "Onload Event": new Ext.grid.GridEditor(new Ext.form.ComboBox({
                            store: GetEventsList(),
                            displayField: 'EventName',
                            resizable: true,
                            valueField: 'EventName',
                            editable: false,
                            typeAhead: true,
                            id: 'OnloadEvent_combobox_id',
                            mode: 'local',
                            listeners: {
                                'select': function (obj, a, b) {
                                    this.fireEvent('blur');

                                    if (Ext.pagePropertyList[Ext.propertyOwnerId]) {
                                        Ext.getCmp('OnloadEvent_combobox_id').setValue(JsonDecode(obj.value));
                                        Ext.pagePropertyList[Ext.propertyOwnerId][ONLOAD_EVENT] = obj.value;
                                    }
                                },
                                'expand': function (obj) {
                                    ResetEMSEPropertyComboBox(obj);
                                }
                            },
                            triggerAction: 'all',
                            emptyText: DEFALUT_EVENT_NAME,
                            selectOnFocus: true
                        })),
                        "BeforeButton Event": new Ext.grid.GridEditor(new Ext.form.ComboBox({
                            store: GetEventsList(),
                            displayField: 'EventName',
                            resizable: true,
                            valueField: 'EventName',
                            editable: false,
                            typeAhead: true,
                            id: 'BeforeButton_combobox_id',
                            mode: 'local',
                            listeners: {
                                'select': function (obj, a, b) {
                                    this.fireEvent('blur');

                                    if (Ext.pagePropertyList[Ext.propertyOwnerId]) {
                                        Ext.getCmp('BeforeButton_combobox_id').setValue(JsonDecode(obj.value));
                                        Ext.pagePropertyList[Ext.propertyOwnerId][BEFORE_BUTTON_EVENT] = obj.value;
                                    }
                                },
                                'expand': function (obj) {
                                    ResetEMSEPropertyComboBox(obj);
                                }
                            },
                            triggerAction: 'all',
                            emptyText: DEFALUT_EVENT_NAME,
                            selectOnFocus: true
                        })),
                        "AfterButton Event": new Ext.grid.GridEditor(new Ext.form.ComboBox({
                            store: GetEventsList(),
                            displayField: 'EventName',
                            resizable: true,
                            valueField: 'EventName',
                            editable: false,
                            typeAhead: true,
                            id: 'AfterButton_combobox_id',
                            mode: 'local',
                            listeners: {
                                'select': function (obj, a, b) {
                                    this.fireEvent('blur');

                                    if (Ext.pagePropertyList[Ext.propertyOwnerId]) {
                                        Ext.getCmp('AfterButton_combobox_id').setValue(JsonDecode(obj.value));
                                        Ext.pagePropertyList[Ext.propertyOwnerId][AFTER_BUTTON_EVENT] = obj.value;
                                    }
                                },
                                'expand': function (obj) {
                                    ResetEMSEPropertyComboBox(obj);
                                }
                            },
                            triggerAction: 'all',
                            emptyText: DEFALUT_EVENT_NAME,
                            selectOnFocus: true
                        })),
                        "Default Heading": new DisabledGridEditor(),
                        "Heading(Default Language)": new DisabledGridEditor(),
                        " Step Name(Default Language)": new DisabledGridEditor(),
                        " Page Name(Default Language)": new DisabledGridEditor(),
                        "Instructions(Default Language)": new DisabledGridEditor(),
                        "Contact Type Options": new Ext.grid.GridEditor(new Ext.form.Field({
                            readOnly: true
                        })),
                        "Document Type Options": new DisabledGridEditor(),
                        "Group": new Ext.grid.GridEditor(new Ext.form.ComboBox({
                            store: asiGroupStore,
                            displayField: 'Name',
                            valueField: 'Name',
                            editable: false,
                            typeAhead: true,
                            id: 'asi_combobox_id',
                            mode: 'local',
                            listeners: {
                                'select': function (combo, rec, idx) {
                                    combo.setValue(JsonDecode(combo.value));

                                    // change ASI sub group items
                                    changeASISubGroup(combo.getValue(), true);
                                    this.fireEvent('blur');
                                }
                            },
                            triggerAction: 'all',
                            emptyText: Ext.ASIGroupEmptyText,
                            selectOnFocus: true
                        })),
                        "Subgroup": new Ext.grid.GridEditor(new Ext.form.ComboBox({
                            store: asiSubgroupStore,
                            displayField: 'Text',
                            valueField: 'Value',
                            editable: false,
                            typeAhead: true,
                            id: 'asi_subgroup_combobox_id',
                            mode: 'local',
                            listeners: {
                                'select': function (combo, rec, idx) {
                                    var selectedValue = JsonDecode(combo.lastSelectionText);
                                    combo.setValue(selectedValue);

                                    this.fireEvent('blur');
                                }
                            },
                            triggerAction: 'all',
                            emptyText: Ext.ASIGroupEmptyText,
                            selectOnFocus: true
                        })),
                        "Record Types": new Ext.grid.GridEditor(new Ext.form.ComboBox({
                            store: capTypeStore,
                            displayField: 'Text',
                            valueField: 'Value',
                            tpl: '<tpl for="."><div class="x-combo-list-item" ext:qtip="{Text}">{Text}</div></tpl>',
                            editable: false,
                            typeAhead: true,
                            id: 'record_type_combobox_id',
                            mode: 'local',
                            listeners: {
                                'select': function (combo, rec, idx) {
                                    var selectedValue = JsonDecode(combo.lastSelectionText);
                                    combo.setValue(selectedValue);
                                    newFormWin.hide();

                                    //Switch the status for Document Type configuration cell when cap type changed.
                                    var selectedCapType = this.getValue().trim();
                                    var propertyTab = Ext.getCmp(COMPONENT_TAB_ID).getComponent(1);
                                    var domPropertyTab = propertyTab.el.dom;
                                    /*
                                    The property grid including 2 columns per row, and the grid including a header, the header also including 2 cells.
                                    And the value cell is second cell, so we use 2*{index}+3 to find the cell.
                                    */
                                    var docTypeConfigCell = domPropertyTab.getElementsByTagName('td')[2 * propertyTab.store.indexOfId(DocumentTypeOptions) + 3];
                                    var elDocTypeConfigCellObj = Ext.get(docTypeConfigCell);
                                    var elDocTypeConfigCellContainer = elDocTypeConfigCellObj.findParent('div', null, true);

                                    if (selectedCapType != '' && selectedCapType != Ext.LabelKey.DropDownDefaultText) {
                                        elDocTypeConfigCellContainer.setStyle('display', '');
                                    } else {
                                        elDocTypeConfigCellContainer.setStyle('display', 'none');
                                    }
                                }
                            },
                            triggerAction: 'all',
                            emptyText: Ext.RecordTypeEmptyText,
                            selectOnFocus: true
                        }))
                    },
                    listeners: {
                        propertychange: function (source, recordId, value, oldValue) {
                            changeSaveStatus();
                            var obj = Ext.getCmp(Ext.propertyOwnerId);
                            if (!obj)
                                obj = Ext.curComponent;
                            if (!obj) return false;

                            if (obj.getXType() == "ComponentPanel" && obj.isSupportMultiply) {
                                var cptSource = eval("ComponentCfg." + obj.titleA + ".source");
                                var cptKey = obj.componentSeqNbr;
                                if (!cptKey || cptKey == 0) {
                                    cptKey = obj.id;
                                }

                                if (!cptSource[cptKey]) {
                                    cptSource[cptKey] = {};
                                }
                                cptSource[cptKey][recordId] = value;
                            }

                            if (obj.getXType() == "ComponentPanel" && recordId == "Subgroup") {
                                appendSubgroupToTitle(obj, value);
                            }

                            if (obj.getXType() == "StepPanel" || obj.getXType() == "PagePanel") {
                                if (recordId == PROPERTY_PAGE_NAME || recordId == PROPERTY_STEP_NAME) {//"Page Name" or "Step Name"
                                    obj.setTitle(value);
                                }
                            }
                        },
                        beforeedit: function (e) {
                            var record = e.grid.store.getAt(e.row);
                            if (record.id == "Instructions") {
                                e.cancel = true;

                                if (Ext.curComponent == null) {
                                    return false;
                                }

                                if (!isWinShow && Ext.curComponent.getXType() == "PagePanel" && Ext.pagePropertyList[Ext.propertyOwnerId]) {
                                    var instruction = Ext.pagePropertyList[Ext.propertyOwnerId][PAGE_INSTRUCTION];
                                    PopupHtmlEditor(e.grid, instruction);
                                    htmlEditorTargetType = htmlEditorTargetTypes.PageFlowInstruction;
                                }
                                else if (!isWinShow && Ext.curComponent.getXType() == "ComponentPanel") {
                                    var source = eval("ComponentCfg." + Ext.curComponent.titleA + ".source");
                                    var cptKey = Ext.curComponent.componentSeqNbr;
                                    if (!cptKey || cptKey == 0) {
                                        cptKey = Ext.curComponent.id;
                                    }

                                    if (Ext.curComponent.isSupportMultiply) {
                                        PopupHtmlEditor(e.grid, source[cptKey].Instructions);
                                    }
                                    else {
                                        PopupHtmlEditor(e.grid, source.Instructions);
                                    }

                                    htmlEditorTargetType = htmlEditorTargetTypes.PageFlowInstruction;
                                }
                            }
                            else if (record.id == ContactTypeOptions) {
                                DisabledDropDownButtons(true);
                                container.innerHTML = '';
                                var pageflowCode = Ext.getCmp('txtNewSmartchoiceGroupName').getValue().trim();

                                formWinTarget = "ContactType";
                                if (_contactTypeMapping == null || _contactTypeMapping[Ext.curComponentId] == null) {
                                    Ext.Ajax.request({
                                        method: "post",
                                        url: '../Pageflow/WorkflowContent.aspx',
                                        params: { action: 'GetContactTypeList', pageFlowGrpCode: pageflowCode, moduleName: document.moduleName, componentSeqNbr: Ext.curComponent.componentSeqNbr },
                                        success: CallBackContatTypeList,
                                        argument: { event: e },
                                        headers: { 'Content-Type': 'application/json;utf-8'}//json tag should be added other xml is returned.
                                    });
                                }
                                else {
                                    ShowContactTypeList(e, _contactTypeMapping[Ext.curComponentId]);
                                }
                            }
                            else if (record.id == DocumentTypeOptions) {
                                e.cancel = true;
                                var alias = Ext.getCmp('record_type_combobox_id').getValue().trim();
                                var capTypeKey = GetRecordTypeValueByAlias(alias);

                                if (capTypeKey != Ext.LabelKey.DropDownDefaultText && capTypeKey != "") {
                                    DisabledDropDownButtons(true);
                                    container.innerHTML = '';
                                    var pageflowCode = Ext.getCmp('txtNewSmartchoiceGroupName').getValue().trim();

                                    if (requiredDocumentTypeStore[capTypeKey] == null) {
                                        Ext.Ajax.request({
                                            method: "post",
                                            url: '../Pageflow/WorkflowContent.aspx',
                                            params: { action: 'GetRequireDocumentTypes', pageFlowGrpCode: pageflowCode, moduleName: document.moduleName },
                                            success: CallBackRequiredDocumentTypeList,
                                            argument: { event: e },
                                            headers: { 'Content-Type': 'application/json;utf-8' }
                                        });
                                    }

                                    formWinTarget = "DocumentType";
                                    if (!_documentTypeMapping
                                        || !_documentTypeMapping[Ext.curComponentId]
                                        || !IsExistDocumentType(_documentTypeMapping[Ext.curComponentId].json, capTypeKey)) {
                                        Ext.Ajax.request({
                                            method: "post",
                                            url: '../Pageflow/WorkflowContent.aspx',
                                            params: { action: 'GetDocumentTypes', pageFlowGrpCode: pageflowCode, moduleName: document.moduleName, capTypeKey: capTypeKey, componentSeqNbr: Ext.curComponent.componentSeqNbr },
                                            success: CallBackDocumentTypeList,
                                            argument: { event: e },
                                            headers: { 'Content-Type': 'application/json;utf-8' }
                                        });
                                    }
                                    else {
                                        ShowDocumentTypeList(e, _documentTypeMapping[Ext.curComponentId]);
                                    }
                                }
                            }
                        },
                        cellclick: function (grid, rowIndex, columnIndex, e) {
                            DisabledHtmlEditorButtons(true);
                        },
                        beforepropertychange: function (source, recordId, value, oldValue) {
                            if (typeof (value) != "boolean") {
                                if (value == "") {
                                    return false;
                                }
                            }

                            var obj = Ext.getCmp(Ext.propertyOwnerId);
                            if (!obj) {
                                obj = Ext.curComponent;
                            }
                            if (!obj) {
                                return false;
                            }

                            // validate path
                            if (obj.getXType() == "ComponentPanel" && recordId == "Path") {
                                var reg = /[\"<>|:*?]/;
                                if (value == null || value == "" || reg.test(value)) {
                                    Ext.Msg.alert(Ext.LabelKey.admin_pageflow_message_information, "Invalid path.");
                                    return false;
                                }
                            }

                            if (obj.getXType() == "StepPanel") {
                                if (value.length > 27) {
                                    Ext.Msg.alert("Information", "The length of StepName must be less than 27 characters");
                                    return false;
                                }
                                for (var i = 0; i < obj.ownerCt.items.length; i++) {
                                    if (obj.ownerCt.items.items[i].title == value) {
                                        Ext.Msg.alert("Information", Ext.LabelKey.Admin_Pageflow_SameStepNameInfo);
                                        return false;
                                    }
                                }
                            } else if (obj.getXType() == "PagePanel") {
                                if (value.length > 27 && recordId == PROPERTY_PAGE_NAME) {
                                    Ext.Msg.alert("Information", "The length of PageName must be less than 27 characters");
                                    return false;
                                }
                                for (var i = 0; i < obj.ownerCt.ownerCt.items.length; i++) {
                                    if (obj.ownerCt.ownerCt.items.items[i].getXType() == "ArrowTemplate" || obj.ownerCt.ownerCt.items.items[i].getXType() == "panel") {
                                        continue;
                                    }
                                    var page = obj.ownerCt.ownerCt.items.items[i].items.items[0];
                                    if (page.title == value) {
                                        Ext.Msg.alert("Information", Ext.LabelKey.Admin_Pageflow_SamePageNameInfo);
                                        return false;
                                    }
                                }
                            } else {//component
                                if (value.length > 255) {
                                    Ext.Msg.alert("Information", "The length of Component Title must be less than 255 characters");
                                    return false;
                                }
                            }

                            return true;
                        }
                    }
                })
		    );

            var obj = parTab.getComponent(0);
            Component_Property_Tab_Id = obj.id;

            if (!obj.items) {
                var index = 0;
                for (var key in ComponentCfg) {
                    if (IsSupperAgency && key == "LicensedProfessional") {
                        continue;
                    }

                    var newComponentPanel = new Ext.ux.ComponentPanel;
                    var itm = ComponentCfg[key];
                    newComponentPanel.title = itm.title;
                    newComponentPanel.titleA = itm.titleA;
                    newComponentPanel.maxWidth = itm.maxWidth;
                    newComponentPanel.iconCls = "x-component-" + itm.componentID;
                    newComponentPanel.id = "Component_" + itm.componentID; //Must NOT use itm.componentID only!
                    newComponentPanel.sortID = itm.sortID;
                    newComponentPanel.componentId = itm.componentID;
                    newComponentPanel.isSupportMultiply = itm.isSupportMultiply;
                    obj.insert(index, newComponentPanel);

                    index++;
                }

                try {
                    region.doLayout();
                } catch (e) { }
            }
        },
        //flags1: 0/1 , up/down  flags2: 0/1 , left/right
        changeStepPosion: function (e, flags1, flags2) {
            var panel;

            if (Ext.curStepId) {
                panel = Ext.getCmp(Ext.curStepId);
            } else {
                return;
            }

            var childs = panel.container.dom.childNodes;

            var targetId;
            var diff, temp = 10000;

            var panelXY = panel.el.getXY();
            var targetXY;
            var tmp = 0;
            for (var i = 0; i < childs.length; i++) {

                if (Ext.getCmp(childs[i].id) !== undefined) {
                    if (Ext.getCmp(childs[i].id).cls == 'x-StepPanel') {
                        targetXY = Ext.getCmp(childs[i].id).el.getXY();

                        if (flags1 == 0 && targetXY[1] < panelXY[1]) {
                            diff = Math.abs(targetXY[1] - panelXY[1]);
                            if (diff > 0 && diff < temp) { //find the nearest StepPanel
                                temp = diff;
                                targetId = childs[i].id;
                                tmp = i;
                            }
                        } else if (flags1 == 1 && targetXY[1] > panelXY[1]) {
                            diff = Math.abs(targetXY[1] - panelXY[1]);
                            if (diff > 0 && diff < temp) { //find the nearest StepPanel
                                temp = diff;
                                targetId = childs[i].id;
                                tmp = i;
                            }
                        }
                    }
                }
            }
            //exchange the positons
            if (targetId) {
                var step1 = Ext.getCmp(targetId);

                panel.el.dom.parentNode.removeChild(panel.el.dom);

                var stepIndex = tmp;
                if (flags1 == 1) {//down
                    stepIndex++;
                }
                this.pane.insert(stepIndex, panel);

                //step1.stepOrder=panel.stepOrder;
                //panel.stepOrder=tmp;
                this.pane.doLayout();
            }
            this.setStepToolsStatus();
            changeSaveStatus();
        },
        loadPageflow: function (stepList) {
            //trace ramdom error
            if (stepList == "-1") {
                stepList = null;
                Ext.MessageBox.show({
                    title: 'An error has occurred.',
                    msg: 'We are experiencing technical difficulties.<br />Please try again later or contact the City for assistance.',
                    width: 400,
                    progressText: '',
                    icon: Ext.MessageBox.ERROR,
                    closable: true
                });
                return;
            }

            Ext.cmp = null;
            Ext.pagePropertyList = new Array();
            Ext.stepPropertyList = new Array();
            Ext.oldStepId = null;
            Ext.oldPageId = null;
            Ext.curStepId = null;
            Ext.curPageId = null;
            Ext.isFirstLoad = true;
            Ext.FirstPropertyTabWidth = 0;
            Ext.propertyOwnerId = null;
            Ext.curComponent = null;
            Ext.FirstStep = null;
            var pane = Ext.getCmp("myWorkPanel");
            if (pane.items) {
                for (var i = pane.items.length - 1; i >= 0; i--) {
                    pane.items.items[i].deleteStepPanel(true);
                }
            }
            if (!stepList || stepList.length < 1) {//add new step
                this.addStepPanel(null, true); //overwrite method, the second parameter means it's the empty step
            } else {//load data
                //order
                stepList.sort(orderStep);
                for (var mm = 0; mm < stepList.length; mm++) {
                    var pages = stepList[mm].pageList;
                    pages.sort(orderPage);
                    for (var nn = 0; nn < pages.length; nn++) {
                        var cmps = pages[nn].componentList;
                        cmps.sort(orderComponent);
                    }
                }

                for (var i = 0; i < stepList.length; i++) {
                    this.addStepPanel(stepList[i], true);
                }
            }

            pane.doLayout();
        },
        savePageflow: function () {
            Ext.canSave = true;

            if (!this.pane.items) {
                return;
            }

            var paras = {
                asiSubgroups: new Array(),
                asitSubgroups: new Array(),
                hasAttachment: false,
                hasConditionDocument: false,
                stepIndex: 0,
                pageIndex: 0
            };
            var stepData = 'stepList:[';
            var stepList = "";
            cptCtrIdMappingOrder = {};

            // reset attachment component list when save page flow.
            _attachmentSectionInfoList = [];

            for (var i = 0; i < this.pane.items.length; i++) {
                var step = this.pane.items.items[i];
                if (stepList != "") {
                    stepList += ",";
                }
                stepList += "{";
                if (Ext.stepPropertyList[step.id]) {
                    stepList += "stepName:'" + JsonEncode(Ext.stepPropertyList[step.id]["Step Name"]) + "',";
                } else {
                    stepList += "stepName:'" + JsonEncode(step.title) + "',";
                }
                stepList += "stepID:" + step.stepID + ",";
                stepList += "stepOrder:" + i + ",";
                stepList += "pageList:[";

                // save page list
                paras.stepIndex = i;
                var pageResult = this.savePageList(step, paras);
                paras = pageResult.paras;

                if (pageResult == this.saveBlankPage()) {
                    return pageResult;
                }

                if (pageResult == -1) {
                    return -1;
                }

                if (paras.asiSubgroups.containsDuplicate() || paras.asitSubgroups.containsDuplicate()) {
                    Ext.Msg.alert(Ext.LabelKey.admin_pageflow_message_information, Ext.LabelKey.acaadmin_pageflow_msg_duplicatesubgroup);
                    Ext.canSave = false;
                    return -1;
                }

                if (paras.hasAttachment && paras.hasConditionDocument) {
                    Ext.Msg.alert(Ext.LabelKey.admin_pageflow_message_information, Ext.LabelKey.admin_pageflow_message_conflictattachment);
                    Ext.canSave = false;
                    return -1;
                }

                stepList += pageResult.pageList + "]}";
            };

            stepData += stepList + "]";

            // Validate required document types for each record type in each attachment section.
            if (_attachmentSectionInfoList.length > 0) {
                var compNameList = CollectAttachmentComponentNameList();
                var errorRequireDocumentTypes = ValidateRequiredDocumentTypes(compNameList);

                if (errorRequireDocumentTypes && errorRequireDocumentTypes.length > 0) {
                    var titleModel = Ext.LabelKey.Admin_Pageflow_Message_RequiredDocumentTitle;
                    var subtitleModel = Ext.LabelKey.Admin_Pageflow_Message_RequiredDocumentSubTitle;
                    var bodyModel = Ext.LabelKey.Admin_Pageflow_Message_RequiredDocumentTypeBodys;

                    var subTitle = "";
                    var bodys = "";
                    var result = "";

                    for (var index = 0; index < errorRequireDocumentTypes.length; index++) {
                        //reset bodys context
                        bodys = "";
                        subTitle += String.format(subtitleModel, errorRequireDocumentTypes[index].recordType);
                        var documents = errorRequireDocumentTypes[index].missedRequiredDocTypes;

                        for (var innerIndex = 0; innerIndex < documents.length; innerIndex++) {
                            bodys += String.format(bodyModel, documents[innerIndex]);
                        }

                        subTitle += bodys;
                    }

                    result = String.format(titleModel, subTitle);

                    Ext.Msg.alert(Ext.LabelKey.admin_pageflow_message_information, result);
                    Ext.canSave = false;
                    return -1;
                }
            }

            return stepData;
        },
        saveBlankPage: function () {
            var stepData = 'stepList:[]';
            return stepData;
        },
        // save page list value
        // parameters:
        //    step: the step model, it is the pageflowModel.stepList[]
        //    paras: { asiSubgroups, asitSubgroups, stepIndex, pageIndex }
        savePageList: function (step, paras) {
            var pageList = "";
            var pageIndex = 0;

            for (var i = 0; i < step.items.length; i++) {
                if (step.items.items[i].getXType() == "ArrowTemplate" ||
                    step.items.items[i].getXType() != "PageContainer") {
                    continue;
                }

                if (pageList != "") {
                    pageList += ",";
                }

                var page = step.items.items[i].items.items[0];
                if (page.items.length == 1 && page.items.items[0].getXType() == "PageHTML") {
                    var flow = Ext.getCmp("pnlPageflow");

                    if (flow.pane.items.length == 1 && step.items.length == 3 && Ext.isCreatePageflow) {
                        return this.saveBlankPage();
                    }
                    else {
                        Ext.Msg.alert("Information", "All pages in a Page Flow must have at least one component. Either add a component to the blank page or delete the blank page");
                        Ext.canSave = false;
                        return -1;
                    }
                }
                pageList += "{";
                if (Ext.pagePropertyList[page.id]) {
                    pageList += "pageName:'" + JsonEncode(Ext.pagePropertyList[page.id][PROPERTY_PAGE_NAME]) + "',";
                } else {
                    pageList += "pageName:'" + JsonEncode(page.title) + "',";
                }

                var instruction;
                if (Ext.pagePropertyList[page.id]) {
                    instruction = Ext.pagePropertyList[page.id][PAGE_INSTRUCTION];
                }
                else {
                    instruction = page.instruction;
                }
                pageList += "instruction:'" + JsonEncode(instruction) + "',";

                //emse
                pageList += "onloadEventName:'" + getEmseEventName(page.id, ONLOAD_EVENT) + "',";
                pageList += "beforeClickEventName:'" + getEmseEventName(page.id, BEFORE_BUTTON_EVENT) + "',";
                pageList += "afterClickEventName:'" + getEmseEventName(page.id, AFTER_BUTTON_EVENT) + "',";
                //end emse
                pageList += "pageID:" + page.pageID + ",";
                pageList += "stepID:" + step.stepID + ",";
                pageList += "pageOrder:" + pageIndex + ",";
                pageList += "componentList:[";

                paras.pageIndex = pageIndex;
                pageIndex++;

                // save the component list
                var cptResult = this.saveComponentList(page, paras);

                if (cptResult == -1) {
                    return -1;
                }

                paras = cptResult.paras;
                pageList += cptResult.componentList + "]}";
            }

            return { pageList: pageList, paras: paras };
        },
        // save component list value
        // parameters:
        //    page: the page model, it is the pageflowModel.stepList[].pageList[]
        //    paras: { asiSubgroups, asitSubgroups, stepIndex, pageIndex }
        saveComponentList: function (page, paras) {

            var cptList = "";
            for (var cptIndex = 0; cptIndex < page.items.length; cptIndex++) {
                if (cptList != "") {
                    cptList += ",";
                }

                var cpt = page.items.items[cptIndex];
                var source = eval("ComponentCfg." + cpt.titleA + ".source");
                var sorceA = eval("ComponentCfg." + cpt.titleA);

                cptList += "{";
                cptList += "componentID:'" + cpt.componentId + "',";
                cptList += "componentName:'" + sorceA.title + "',";
                cptList += "pageID:" + page.pageID + ",";
                cptList += "displayOrder:" + cptIndex + ",";
                cptList += "componentSeqNbr:" + cpt.componentSeqNbr + ",";

                //13:Attachment; 21:Condition Document.
                if (sorceA.componentID == "13") {
                    paras.hasAttachment = true;
                }
                else if (sorceA.componentID == "21") {
                    paras.hasConditionDocument = true;
                }

                // in component which support multiple, need get the sub data source
                var multipleItem = "";
                if (cpt.isSupportMultiply) {
                    var cptKey = cpt.componentSeqNbr;
                    if (!cptKey || cptKey == 0) {
                        cptKey = cpt.id;
                    }

                    multipleItem = "['" + cptKey + "']";

                    if (cpt.titleA == "ContactList" || cpt.titleA == "Attachment") {
                        cptCtrIdMappingOrder[cpt.id] = paras.stepIndex + "_" + paras.pageIndex + "_" + cptIndex;
                    }

                    if (cpt.componentId == 13)//Attachment
                    {
                        _attachmentSectionInfoList.push({ id: cpt.id, seqNbr: cpt.componentSeqNbr });
                    }
                }

                //If the component is ASI/ASIT, set the instruction as NULL in DB instead of 'Undefined'.
                var instruction = "";
                if (cpt.componentId != 10 && cpt.componentId != 11) {
                    instruction = eval("source" + multipleItem + "[\"Instructions\"]");
                    instruction = instruction.replace(/\&/g, "&amp;");
                }

                if (Ext.IsSupportMultiLanguage) {
                    if (window.parent.IsDefaultLanguage()) {
                        cptList += "instruction:'" + JsonEncode(instruction) + "',";
                    }
                    cptList += "resInstruction:'" + JsonEncode(instruction) + "',";
                } else {
                    cptList += "instruction:'" + JsonEncode(instruction) + "',";
                }

                var asiGroup;
                var asiSubgroup;
                var property = "";
                for (var propIndex = 0; propIndex < propertyArr.length; propIndex++) {
                    var val = eval("source" + multipleItem + "[\"" + propertyArr[propIndex] + "\"]");
                    if (val == undefined) {
                        continue;
                    }

                    // set the speicial value
                    if (val.toString() == "true") {
                        val = "Y";
                    }
                    else if (val.toString() == "false") {
                        val = "N";
                    }
                    else if (val == Ext.ASIGroupEmptyText) {
                        // set the ASI/ASIT empty value to save
                        val = "";
                    }

                    // set the property
                    if (property != "") {
                        property += ",";
                    }

                    if (propertyArr[propIndex] == "Group") {
                        asiGroup = val;
                    }

                    // set the ASI/ASIT subgroup list
                    if (propertyArr[propIndex] == "Subgroup") {
                        var isASIT = (cpt.componentId == 11);
                        val = GetASISubgroupByResSubgroup(asiGroup, val, isASIT);

                        if (cpt.componentId == 10) {
                            paras.asiSubgroups[paras.asiSubgroups.length] = EncodeSpecialChar(val);
                        }
                        else if (cpt.componentId == 11) {
                            paras.asitSubgroups[paras.asitSubgroups.length] = EncodeSpecialChar(val);
                        }
                        asiSubgroup = EncodeSpecialChar(val);
                    }

                    if (propertyArr[propIndex] == "Custom Heading " || propertyArr[propIndex] == "title") {
                        property += "customHeading" + ":'" + JsonEncode(val) + "'";
                    }
                    else if (propertyArr[propIndex] == "Custom Heading") {
                        var resCustomHeading = JsonEncode(val);

                        if (Ext.IsSupportMultiLanguage) {
                            var defaultCustomHeading = window.parent.IsDefaultLanguage() ? resCustomHeading : cpt.defaultCustomHeading;

                            if (defaultCustomHeading != null) {
                                property += "customHeading" + ":'" + JsonEncode(defaultCustomHeading) + "',";
                            } else {
                                property += "customHeading" + ":'',";
                            }

                            if (resCustomHeading != null) {
                                property += "resCustomHeading" + ":'" + JsonEncode(resCustomHeading) + "'";
                            } else {
                                property += "resCustomHeading" + ":''";
                            }

                        } else {
                            if (resCustomHeading != null) {
                                property += "customHeading" + ":'" + JsonEncode(resCustomHeading) + "'";
                            } else {
                                property += "customHeading" + ":''";
                            }
                        }
                    }
                    else if (propertyArr[propIndex] == "Data Source") {
                        property += propertyArrTitle[propIndex] + ":'" + EncodeSpecialChar(ComponentDataSource[val]) + "'";
                    }
                    else {
                        property += propertyArrTitle[propIndex] + ":'" + EncodeSpecialChar(val) + "'";
                    }
                }
                cptList += property + "}";

                if (!asiGroupIsEmpty(asiGroup) && asiGroupIsEmpty(asiSubgroup)) {
                    Ext.Msg.alert(Ext.LabelKey.admin_pageflow_message_information, Ext.LabelKey.acaadmin_pageflow_msg_subgroupnotempty);
                    Ext.canSave = false;
                    return -1;
                }
            }

            return { componentList: cptList, paras: paras };
        }
    });
    page.render("pageflow");
}

/*
Collect the attachment component name list from the attachment info array "_attachmentSectionInfoList"
    for required document type validation.
1. For existing attachment component, the name is "Attachment_" + component sequnce number.
2. For new added attachment component, the name is the Ext component Id.
*/
function CollectAttachmentComponentNameList() {
    var componentNameList = [];

    for (var index = 0; index < _attachmentSectionInfoList.length; index++) {
        var compNameInfo = _attachmentSectionInfoList[index];

        if (compNameInfo.seqNbr == 0) {
            componentNameList.push(compNameInfo.id);
        } else {
            componentNameList.push(ATTACHMENT_COMPONENT_PREFIX + compNameInfo.seqNbr);
        }
    }

    return componentNameList;
}

// Sync all the document type settings to the "_documentTypeMapping".
function SyncToDocumentTypeMapping() {
    var needDeleteItems = [];

    for (var compId in _documentTypeMapping) {
        var isExist = false;
        
        for (var i = 0; i < _attachmentSectionInfoList.length; i++) {
            var attachmentComp = _attachmentSectionInfoList[i];
            
            if (attachmentComp.id == compId) {
                isExist = true; 
                break;
            }
        }

       if(!isExist) {
           needDeleteItems.push(compId);
       }
    }

    for (var index = 0; index < needDeleteItems.length; index++) {
        var componentName = needDeleteItems[index];
        delete _documentTypeMapping[componentName];
    }
}

function IsExistDocumentType(documentTypeArray, capTypekey) {
    if (documentTypeArray != null && documentTypeArray.length > 0) {
        for (var index in documentTypeArray) {
            if (documentTypeArray[index].CapTypeKey == capTypekey) {
                return true;
            }
        }
    }

    return false;
}

function GetDocumentTypeByCapTypeKey(documentTypeArray, capTypekey) {
    if (documentTypeArray != null && documentTypeArray.length > 0) {
        for (var index in documentTypeArray) {
            if (documentTypeArray[index].CapTypeKey == capTypekey) {
                return documentTypeArray[index];
            }
        }
    }

    return null;
}

function doLayoutStepPanel(isFirstLoad) {
    var header = Ext.getCmp("pnlHeader");
    var flow = Ext.getCmp("pnlPageflow");

    if (!flow) {
        return;
    }

    if (!Ext.isIE) {
        flow.doLayout();
        return;
    }

    if (isFirstLoad) {
        Ext.isFirstLoad = true;
    }

    var bodyX = document.body.offsetWidth;

    if (bodyX > Ext.workflowMaxWidth && Ext.workflowFixWidth > 0) {
        Ext.workflowWidth = bodyX - 50 - Ext.workflowFixWidth;
    }
    else {
        Ext.workflowWidth = bodyX - 70;
    }

    flow.setWidth(Ext.workflowWidth + 7);
    header.setWidth(Ext.workflowWidth);

    Ext.getCmp('grdCapTypes').setWidth(Ext.workflowWidth);
    AdjustCapPanel();
}

function AdjustCapPanel() {
    var capAvailable = Ext.getCmp("pnlCapAvailable");
    var capSelected = Ext.getCmp("pnlCapSelected");
    var adjWidth = Ext.workflowWidth * 0.4;

    capSelected.setWidth(adjWidth);
    capAvailable.setWidth(adjWidth);
    if (!Ext.isIE) {
        document.getElementById('addCap').parentNode.width = Ext.workflowWidth * 0.1;
    }
    else {
        document.getElementById('addCap').parentElement.width = Ext.workflowWidth * 0.1;
    }

    capAvailable.setNodeWidth('pnlCapAvailable');
    capSelected.setNodeWidth('pnlCapSelected');
}

function correctComponentLayout() {
    if (!Ext.isFirstExpend) return;
    if (Ext.FirstPropertyTabWidth != 0) {
        return;
    }

    Ext.isFirstExpand = false;

    var tab = Ext.getCmp(Component_Property_Tab_Id);
    if (!tab || !tab.items) {
        return;
    }

    for (var i = 0; i < tab.items.length; i++) {
        var cmp = tab.items.items[i];
        cmp.body.applyStyles({ height: '0' });
    }
}
function propertyTabClick() {
    var propertyBlur = document.getElementById("properpyBlur");
    if (!propertyBlur) {
        return;
    }
    var parms = new Object();
    parms.obj = propertyBlur;
    avo.attachEvent("click", propertyBlur, LostFocus, parms);
    avo.doClick(propertyBlur);
}
function LostFocus() {
    if (Ext.isIE6) {
        this.obj.focus();
        this.obj.blur();
    }
}

var _step_width = 0;
var timer;
function checkTimer() {//for FF resize
    if (Ext.isIE) {
        return;
    }
    var header = Ext.getCmp("pnlHeader");

    var step = Ext.getCmp(Ext.curStepId);
    if (!step) {
        return;
    }
    var curWidth = step.getSize().width;
    var len = step.items.length > 6 ? 6 : step.items.length;
    if ((len - 2) * 175 > curWidth) {
        _step_width = (len - 2) * 175;
        timer = setInterval("startTimer()", 500);
    }
}

function SetFormWinPosition(cell) {
    var top = cell.y + cell.height;
    var left;
    var width;

    left = cell.x + cell.width - 305;
    width = 295;

    newFormWin.setPosition(left, top);
    newFormWin.setWidth(width);
}

function startTimer() {
    var pageflow = Ext.getCmp("pnlPageflow");

    var step;
    var chkLen = 0;
    for (var i = 0; i < pageflow.pane.items.length; i++) {
        var _step = pageflow.pane.items.items[i];
        if (_step.items.length >= chkLen) {
            step = _step;
        }
    }
    var curWidth = step.getSize().width;

    if (_step_width < curWidth) {
        pageflow.doLayout();
        _step_min_width = 0;
        curWidth = 0;
        clearInterval(timer);
    }
}

function initComponentSource(obj, tab, isClearData) {
    var source = eval("ComponentCfg." + obj.titleA + ".source");
    var _source = eval("ComponentCfg_Template." + obj.titleA + ".source");

    // in component which support multiple, need get the sub data source
    if (obj.isSupportMultiply) {
        _source = _source["Component_" + obj.componentId];

        // the key for the component that support multiply
        var cptKey = obj.componentSeqNbr;
        if (!cptKey || cptKey == 0) {
            cptKey = obj.id;
        }

        if (!source[cptKey]) {
            // if it is null, set default value. ex: drag from component panel.
            source[cptKey] = cloneKeyVaulePair(source["Component_" + obj.componentId]);
        }
        source = source[cptKey];
    }

    // clear source, set the source to default value.
    if (isClearData == true) {
        for (key in _source) {
            source[key] = _source[key];
        }
    }

    var _cfg = eval("ComponentCfg_Template." + obj.titleA);
    if (obj.titleA == "Applicant") {
        obj.setTitle(_cfg.title);
        var app = Ext.getCmp('application_combobox_id');
        var appValue = app.store.data.items[0].data.Name;
        if (source["Custom Heading "] == "select..." || isClearData == true) {
            source["Custom Heading "] = appValue;
        }
    }
    else if (obj.titleA == "Contact1" || obj.titleA == "Contact2" || obj.titleA == "Contact3") {
        obj.setTitle(_cfg.title);
        var cont = Ext.getCmp('contact_combobox_id');
        contValue = cont.store.data.items[0].data.Name;
        if (source["Custom Heading "] == "select..." || isClearData == true) {
            source["Custom Heading "] = JsonDecode(contValue);
        }
    }
    else if (obj.titleA == "ApplicationSpecificInformation" || obj.titleA == "ApplicationSpecificInformationTable") {
        var asi = Ext.getCmp('asi_combobox_id');
        var asiValue = asi.store.data.items[0].data.Name;

        if (!source["Group"] || source["Group"] == Ext.ASIGroupEmptyText || isClearData == true) {
            source["Group"] = asiValue;
        }

        changeASISubGroup(source["Group"]);
    }
    // set cap type data source that bind to the combobox.
    else if (obj.titleA == "Attachment") {
        // capTypeStore exists a default value '--Select--'
        if (capTypeStore == null || capTypeStore.data.items.length <= 1) {
            source["Record Types"] = null;
            source[DocumentTypeOptions] = null;
        } else {
            source["Record Types"] = Ext.RecordTypeEmptyText;
            source[DocumentTypeOptions] = Ext.LabelKey.Admin_FieldProperty_ChoiceTip;
            Ext.getCmp('record_type_combobox_id').setValue(Ext.LabelKey.DropDownDefaultText);
        }
    }

    tab.getComponent(1).store.sortInfo = null;
    tab.getComponent(1).setSource(source);
}

function checkPageFlow() {
    Ext.getCmp('pnlPageflow').savePageflow();
    return Ext.canSave;
}
//******************************************************************************

//aca 6.7.0
function getEmseEventName(pageId, eventType) {
    if (Ext.pagePropertyList[pageId]) {
        var ret = Ext.pagePropertyList[pageId][eventType];
        if (ret == DEFALUT_EVENT_NAME) {
            return '';
        }
        return EncodeSpecialChar(ret);
    } else {
        return ''; //DEFALUT_EVENT_NAME;
    }
}
var emseEventList;
function GetEventsList() {
    return emseEventList;
}

function ResetEMSEPropertyComboBox(obj) {
    obj.innerList.setWidth(450);
    obj.list.setWidth(450);
    obj.list.dom.lastChild.className = 'x-resizable-handle x-resizable-handle-southwest x-unselectable';
}

function ShowContactTypeList(e, contactTypeMappingItem) {
    var contactTypeJason = contactTypeMappingItem ? contactTypeMappingItem.json : "";
    RenderContactTypeListControl(contactTypeJason);

    var cell = e.grid.getBox();
    SetFormWinPosition(cell);
    newFormWin.show();
    isContactTypeDDLWinShow = true;
}

function CallBackContatTypeList(response) {
    var contactTypeJason = response.responseText;

    if (_contactTypeMapping == null) {
        _contactTypeMapping = {};
    }

    _contactTypeMapping[Ext.curComponentId] = { cptSeqNbr: Ext.curComponent.componentSeqNbr, json: contactTypeJason };

    RenderContactTypeListControl(contactTypeJason);

    var cell = response.argument.event.grid.getBox();
    SetFormWinPosition(cell);
    newFormWin.show();
    isContactTypeDDLWinShow = true;
}

function ShowDocumentTypeList(e, documentTypeMappingItem) {
    var documentTypeJason = documentTypeMappingItem ? documentTypeMappingItem.json : "";
    RenderDocumentTypeListControl(documentTypeJason);

    var cell = e.grid.getBox();
    SetFormWinPosition(cell);
    newFormWin.show();
    isDocumentTypeDDLWinShow = true;
}

function CallBackRequiredDocumentTypeList(response) {
    requiredDocumentTypeStore = eval("(" + response.responseText + ")");
}

function CallBackDocTypeOptConfigHistory(response) {
    _docTypeOptionConfigStore = eval("(" + response.responseText + ")");
}

function CallBackDocumentTypeList(response) {
    var docTypeItemJson = response.responseText;

    if (!_documentTypeMapping) {
        _documentTypeMapping = {};
    }

    var docTypeMappingItem = _documentTypeMapping[Ext.curComponentId];
    if (!docTypeMappingItem) {
        docTypeMappingItem = {};
    }

    if (!docTypeMappingItem.json) {
        docTypeMappingItem.json = new Array();
    }

    var docTypeArrayJson = docTypeMappingItem.json;
    if (docTypeItemJson != "") {
        var result = eval("(" + docTypeItemJson + ");");
        if (result.CapTypeKey != null) {
            if (!IsExistDocumentType(docTypeArrayJson, result.CapTypeKey)) {
                docTypeArrayJson.push(result);
            }
        }
    }

    _documentTypeMapping[Ext.curComponentId] = { cptSeqNbr: Ext.curComponent.componentSeqNbr, json: docTypeArrayJson };

    RenderDocumentTypeListControl(docTypeArrayJson);

    var cell = response.argument.event.grid.getBox();
    SetFormWinPosition(cell);
    newFormWin.show();
    isDocumentTypeDDLWinShow = true;
}

function RenderContactTypeListControl(contactTypeJason) {
    var container = document.getElementById('container');

    if (contactTypeJason == null || contactTypeJason == "") {
        container.innerHTML = '<table summary="Contact Types"><tr><th scope="col"><label>No contact types found.</label></th></tr></table>';
        return;
    }

    var contactTypeListSource = eval('(' + contactTypeJason + ')');
    var checkedString = "Checked";

    if (contactTypeListSource.length > 0) {
        container.innerHTML = '<table summary="Contact Types"><tr><th scope="col"><label>Contact Types</label></th></tr></table>';

        for (var i = 0; i < contactTypeListSource.length; i++) {
            if (!contactTypeListSource[i].Checked) {
                checkedString = "";
                break;
            }
        }

        Ext.DomHelper.append('container', {
            tag: 'div',
            cls: 'dropblockContactType',
            html: '<table><tr>' +
                '<td style="padding-right:10px; width:15px;"><input id="selectAll" type="checkbox" ' + checkedString + ' onclick="selectAllClick(this);"/></td>' +
                '<td style="padding-right:10px; width:110px;"><label for="selectAll"><span><b>Contact Type</b></span></label></td>' +
                '<td style="padding-right:10px; width:25px;"><span><b>Min</b></span></td>' +
                '<td style="padding-right:10px; width:25px;"><span><b>Max</b></span></td></tr></table>'
        }, true);

        for (var i = 0; i < contactTypeListSource.length; i++) {
            InsertDDLItem(contactTypeListSource[i], 'container');
        }
    }
};

//this function is insert the dragdrop itm into form 
//params:
//          Text:       the value which will be display on the itm
//          itemName:   the container's name
//          isSelected: is selected or not
function InsertDDLItem(contactTypeSource, itemName) {
    var selectAllScript = "UnCheckSelectAllOption()";

    var check;

    if (contactTypeSource.Checked) {
        check = 'checked';
    }
    else {
        check = '';
    }

    var newDragItem = Ext.DomHelper.append(itemName, {
        tag: 'div',
        cls: 'dropblockContactType',
        html: '<table><tr>' +
            '<td style="padding-right: 10px;width:15px;"><input name="chkContactType" id="' + contactTypeSource.Key + '" onclick="DisabledDropDownButtons(false);UnCheckSelectAllOption()" type="checkbox" ' + check + ' /></td>' +
            '<td style="word-break:break-all;padding-right: 10px;width: 110px;"><label for="' + contactTypeSource.Key + '"><span>' + contactTypeSource.Key + '</span></label></td>' +
            '<td style="padding-right: 10px;width:25px;"><INPUT name="txtMinNum" style="width:25px" title="Please set Min number." MaxLength="2" onselect="focusInput(this)" onclick="focusInput(this)" onchange="FilterNumber(this, event)" oninput="DisabledDropDownButtons(false);" onkeypress="RestrictNumber(this, event)" value="' + contactTypeSource.MinNum + '" type="text"></td>' +
            '<td style="padding-right: 10px;width:25px;"><INPUT name="txtMaxNum" style="width:25px" title="Please set Max number." MaxLength="2" onselect="focusInput(this)" onclick="focusInput(this)" onchange="FilterNumber(this, event);" oninput="DisabledDropDownButtons(false);" onkeypress="RestrictNumber(this, event)" value="' + contactTypeSource.MaxNum + '" type="text"></td></tr></table>'
    }, true);

};

function RenderDocumentTypeListControl(documentTypeJason) {
    var container = document.getElementById('container');
    var emptyContent = '<table summary="Document Types"><tr><th scope="col"><label>No document types found.</label></th></tr></table>';

    if (documentTypeJason == null || documentTypeJason.length == 0) {
        container.innerHTML = emptyContent;
        return;
    }

    var alias = Ext.getCmp('record_type_combobox_id').getValue().trim();
    var capTypeKey = GetRecordTypeValueByAlias(alias);
    var documentTypeListSource = GetDocumentTypeByCapTypeKey(documentTypeJason, capTypeKey);

    if (documentTypeListSource == null || documentTypeListSource.DocumentTypes.length == 2) {
        container.innerHTML = emptyContent;
        return;
    }

    var checkedString = "Checked";
    container.innerHTML = '<table summary="Document Types"><tr><th scope="col"><label>Document Types</label></th></tr></table>';

    var items = eval("(" + documentTypeListSource.DocumentTypes + ");");

    for (var i = 0; i < items.length; i++) {
        if (!items[i].Checked) {
            checkedString = "";
            break;
        }
    }

    Ext.DomHelper.append('container', {
        tag: 'div',
        cls: 'dropblockContactType',
        html: '<table><tr>' +
                '<td style="padding-right: 10px;width:15px;"><input id="selectAll" type="checkbox" ' + checkedString + ' onclick="selectAllClick(this);"/></td>' +
                '<td style="padding-right: 10px;width: 200px;"><label for="selectAll"><span><b>Select all</b></span></label></td>' +
                '</tr></table>'
    }, true);

    for (var index = 0; index < items.length; index++) {
        InsertDocumentTypeDDLItem(items[index], 'container', index);
    }
};

//this function is insert the dragdrop itm into form 
//params:
//          Text:       the value which will be display on the itm
//          itemName:   the container's name
//          idSuffix:   the form item's id suffix
function InsertDocumentTypeDDLItem(documentTypeSource, itemName, idSuffix) {
    var check = '';
    if (documentTypeSource.Checked) {
        check = 'checked';
    }

    Ext.DomHelper.append(itemName, {
        tag: 'div',
        cls: 'dropblockContactType',
        html: '<table><tr>' +
              '<td style="padding-right: 10px;width:15px;"><input doctype="' + documentTypeSource.DocumentType + '" name="chkDocumentType" id="chkDocumentType_' + idSuffix + '" onclick="DisabledDropDownButtons(false);UnCheckSelectAllOption()" type="checkbox" ' + check + ' /></td>' +
              '<td style="word-break:break-all;padding-right: 10px;width: 200px;"><label for="chkDocumentType_' + idSuffix + '"><span>' + documentTypeSource.ResDocumentType + '</span></label></td>' +
              '</tr></table>'
    }, true);
};

function FilterNumber(control, evt) {
    control.value = control.value.replace(/^0+/g, '');
    DisabledDropDownButtons(false);
}

function focusInput(obj) {
    obj.focus();
}

function UnCheckSelectAllOption() {
    var selectAllObj = $("#selectAll");
    var selectItems = $("#container .dropblockContactType input[type=checkbox]:gt(0)"); 
    UpdateStatus4SelectAll(selectAllObj, selectItems);
}

function selectAllClick(thisObj) {
    var dragEls = $("#container .dropblockContactType input[type=checkbox]");
    SelectAll(thisObj, dragEls);
    DisabledDropDownButtons(false);
}

// Restrict Number from 1 to 9.
function RestrictNumber(control, evt) {

    evt = evt ? evt : window.event;
    var key = evt.keyCode ? evt.keyCode : evt.which;

    if (key > 57 || key < 48) {
        if (window.event) {
            evt.returnValue = false;
        }
        else {
            evt.preventDefault();
        }
    }

    return true;
}

var markStatus = "";
function changeSaveStatus(isNoMark) {
    if (isNoMark == true) {
        return;
    }
    if (window.parent.isCloseOtherTab == undefined) {
        pageParent.ModifyMark();
    } else {
        window.parent.ModifyMark();
    }
    document.changed = true;
}

//reset page width when click show/hide button
function resizePages() {
    var pane = Ext.getCmp("myWorkPanel");
    if (pane == undefined || pane.items.items.length < 1) {
        return;
    }

    for (var i = 0; i < pane.items.items.length; i++) {
        var step = pane.items.items[i];
        for (var j = 0; j < step.items.items.length; j++) {
            if (!step.items.items[j].items) continue;
            if (step.items.items[j].getXType() != "PageContainer") {
                continue;
            }
            var page = step.items.items[j].items.items[0];
            resizePage(page);
        }
    }
    Ext.FirstStep = null;
}

function resizePage(page) {
    if (Ext.getCmp('propertyPanel')) {
        if (Ext.FirstStep) {
            page.syncSize();
            page.doLayout();
            if (page.items.length > 4) {
                page.setWidth(191);
                page.setHeight(170);
                page.ownerCt.syncSize();
                page.ownerCt.doLayout();
            }
        }
    }
}

function setInstruction4PageFlow(value) {
    if (Ext.pagePropertyList[Ext.propertyOwnerId]) {
        Ext.pagePropertyList[Ext.propertyOwnerId][PAGE_INSTRUCTION] = value;

        if (window.parent.IsDefaultLanguage()) {
            Ext.pagePropertyList[Ext.propertyOwnerId][PAGE_INSTRUCTION_DEFAULT_LANGUAGE] = value;

            var propertyGrid = Ext.getCmp('PropertyGridID2');
            if (propertyGrid && propertyGrid.store) {
                propertyGrid.store.getById(PAGE_INSTRUCTION_DEFAULT_LANGUAGE).set('value', value);
            }
        }
    }
    else if (Ext.curComponent && Ext.curComponent.titleA) {
        var source = eval("ComponentCfg." + Ext.curComponent.titleA + ".source");
        var cptKey = Ext.curComponent.componentSeqNbr;
        if (!cptKey || cptKey == 0) {
            cptKey = Ext.curComponent.id;
        }

        if (Ext.curComponent.isSupportMultiply) {
            source[cptKey].Instructions = value;
        }
        else {
            source.Instructions = value;
        }
    }
}

//build component data source option
function InitDataSource() {
    var dataSourceFrom = new Ext.data.SimpleStore({
        fields: ['value'],
        data: [
        [ComponentDataSource['R']],
        [ComponentDataSource['T']],
        [ComponentDataSource['N']]
    ]
    });

    return dataSourceFrom;
}

// change ASI sub group items
function changeASISubGroup(asiGroup, isSelectEvent) {
    Ext.apply(asiSubgroupStore.baseParams, {
        groupCode: asiGroup
    });
    asiSubgroupStore.reload();

    var subgroup = Ext.getCmp('asi_subgroup_combobox_id');
    subgroup.setValue(Ext.ASIGroupEmptyText);

    // if select change event then set the property grid value.
    // if load, the value is set auto, so it NOT need set again.
    if (isSelectEvent) {
        var propertyGrid = Ext.getCmp('PropertyGridID2');
        if (propertyGrid && propertyGrid.store && propertyGrid.store.getById('Subgroup')) {
            propertyGrid.store.getById('Subgroup').set('value', Ext.ASIGroupEmptyText);
        }
    }
}

// clone the key/value pair data struct
function cloneKeyVaulePair(data) {
    var result = {};

    for (key in data) {
        result[key] = data[key];
    }

    return result;
}

// append ASI/ASIT's sub group to the title
// obj: a ComponentPanel
// subgroup: ASI/ASIT subgroup name
function appendSubgroupToTitle(obj, subgroup) {
    // get the sub group
    if (asiGroupIsEmpty(subgroup)) {
        var asiKey = obj.componentSeqNbr;
        if (!asiKey || asiKey == 0) {
            asiKey = obj.id;
        }

        var asiSource = eval("ComponentCfg." + obj.titleA + ".source");
        if (asiSource[asiKey]) {
            var temp = asiSource[asiKey]["Subgroup"];
            if (!asiGroupIsEmpty(temp)) {
                subgroup = temp;
            }
        }
    }

    // set the sub group to title
    var title = eval("ComponentCfg." + obj.titleA + ".title");

    if (!asiGroupIsEmpty(subgroup)) {
        obj.setTitle(title + " (" + subgroup + ")");
    }
    else {
        obj.setTitle(title)
    }
}

// judge the ASI group/subgroup is empty or not
function asiGroupIsEmpty(asiGroup) {
    return (!asiGroup || asiGroup == "" || asiGroup == Ext.ASIGroupEmptyText);
}

// set the OK Button disabled or not.
function DisabledDropDownButtons(isDisabled) {
    var btnOK = Ext.getCmp("btnDropDownOK");

    if (!isDisabled) {
        btnOK.enable();
    }
    else {
        btnOK.disable();
    }
}

// Get ASI/ASIT res subgroup by subgroup
function GetASIResSubgroupBySubgroup(group, subgroup, isASIT) {
    var resSubgroup = "";
    var arr = GetASISubgroupList(group, isASIT);

    if (arr != null && arr.length > 0) {
        for (var index in arr) {
            if (subgroup == JsonDecode(arr[index][1])) {
                resSubgroup = JsonDecode(arr[index][0]);
                break;
            }
        }
    }
    return resSubgroup;
}

// Get ASI/ASIT subgroup by res subgroup
function GetASISubgroupByResSubgroup(group, resSubgroup, isASIT) {
    var subgroup = "";
    var arr = GetASISubgroupList(group, isASIT);

    if (arr != null && arr.length > 0) {
        for (var index in arr) {
            if (resSubgroup == JsonDecode(arr[index][0])) {
                subgroup = JsonDecode(arr[index][1]);
                break;
            }
        }
    }
    return subgroup;
}

// get the ASI/ASIT resSubgroup/subgroup pair list
// the data format: [['resSubgroup1', 'subgroup1'],['resSubgroup2', 'subgroup2'], ...]
function GetASISubgroupList(group, isASIT) {
    var actionFunction = 'GetASISubGroups';
    if (isASIT) {
        actionFunction = 'GetASITSubGroups';
    }

    var url = "../Pageflow/WorkflowContent.aspx?action=" + actionFunction + "&groupCode=" + escape(group) + "&r=" + Math.random();
    var conn = Ext.lib.Ajax.getConnectionObject().conn;
    conn.open("GET", url, false);
    conn.send(null);

    var list;
    if (conn.responseText != null && conn.responseText != "") {
        list = eval(conn.responseText);
    }
    return list;
}

//Add ASI Subgroup Information in Applicant, Contact1, Contact2, Contact3 Component Property.
function RenderContactTypeGenericTemplate(contactType) {
    if (document.getElementById('templatePanel')) {
        document.getElementById('templatePanel').innerHTML = '';
    }

    if (!contactType) {
        return;
    }

    Ext.Ajax.request({
        method: "post",
        url: '../Pageflow/WorkflowContent.aspx',
        params: { action: 'GetContactGenericTemplateGroups', ContactType: contactType },
        success: function (respone) {
            if (document.getElementById('templatePanel')) {
                document.getElementById('templatePanel').innerHTML = '';
            }

            if (!respone.responseText) {
                return;
            }

            var templateGroup = eval('(' + respone.responseText + ')');
            var templateGroupcode = templateGroup.ASIGroupName;
            var templateSubGroupcode = templateGroup.ASISubGroupName;

            var subGroupHtml =
                "<div style='width:100%; heigh:100%; padding-top: 5px; overflow-x:auto;overflow-y:auto;'>" +
                "   <div class='x-panel-header x-unselectable'>ASI SubGroups (" + EncodeHTMLTag(templateGroupcode) + ")</div>";

            // Add the ASI Sub group code items.
            for (var i = 0; i < templateSubGroupcode.length; i++) {
                subGroupHtml += "<div class='x-pageflow-asiSubGroup'>" + EncodeHTMLTag(templateSubGroupcode[i]) + "</div>";
            }
            subGroupHtml += "</div>";

            if (templateGroupcode) {
                Ext.DomHelper.insertAfter('PropertyGridID2', {
                    id: 'templatePanel',
                    tag: 'div',
                    html: subGroupHtml
                }, true);
            } else {
                return;
            }
        },

        //json tag should be added other xml is returned.
        headers: { 'Content-Type': 'application/json;utf-8' }
    });
}