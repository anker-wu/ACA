 /**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CapTypeFilter.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: CapTypeFilter.js 72643 2008-04-24 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * 04/24/2008     		daly.zeng				Initial.  
 * </pre>
 */
 function CapTypeFilterItem(){
    this.CapTypeList = new Array();
    this.FilterName;
    this.ModuleName;
    this.AddFilter = AddFilter;
}

function AddFilter(list,filterName,moduleName){
    CapTypeList = list;
    FilterName = filterName;
    ModuleName = moduleName;
}

Ext.onReady(function() {
    //doLayoutStepPanel();
    Ext.QuickTips.init();
    Ext.state.Manager.setProvider(new Ext.state.CookieProvider());
    //document.moduleName = getparmByUrl('moduleName');
    document.moduleName = parent.parent.Ext.Const.ModuleName;
    setupBoardTypeSelectionSwitch();
    
    var title = new Ext.form.Label({
        cls:'ACA_New_Title_Label font12px',
        text: Ext.LabelKey.Admin_CapTypeFilter_lblCapTypeFilterTitle
    });
    title.render('divCapFilterTitle');

    Accela.ACA.Web.WebService.AdminConfigureService.GetCapTypeFilterListByModule(document.moduleName, InitCallback);
});

/*
Feature:09ACC-08040_Board_Type_Selection
Setups board type selection switch onto the InspectionSetting page.
Requires InspectionSetting page to provide a placeholder named 'divBoardTypeSectionSwitch'
*/
function setupBoardTypeSelectionSwitch() {
    var label = document.getElementById('lblBoarcTypeSelectionSwitchLabel');
    label.innerHTML = Ext.LabelKey.Admin_CapTypeFilter_BoardTypeSelection_Label; //"dd";
}

function InitCallback(filterNames){
    filterNames= eval('(' + filterNames + ')');
    InitFilterHeader(filterNames);
    InitAmendMentFilteName(filterNames);
    Ext.isFirstLoad=false;
    InitCapTypeFilterRole();
    InitFilterCapAssociation();
    
    Ext.get('filterCapList').dom.style.display='none';
    Ext.get('filterCapAssociation').dom.style.display='none';
}

function InitFilterHeader(filterNames) {
    var filterStore = new Ext.data.SimpleStore({
        fields: ['filter'],
        data: filterNames
    });

    var header = new Ext.FormPanel({
        header: false,
        cls: 'ACA_New_Label font11px',
        bodyStyle: 'padding:8px;background-color:#ECF5FE;margin-top:5px;border-color: #B6D2E5; border-width: 2px;border-style: solid;',
        id: 'pnlHeader',
        labelAlign: 'left',
        layout: 'column',
        //width:Ext.workflowWidth,
        border: false,
        labelSeparator: ' :',
        //defaults:{style:'margin-top:5px'},
        monitorResize: true,
        listeners: {
            'afterlayout': function() {
                AdjustCapPanel();
            }
        },
        items: [{
                columnWidth: 1,
                layout: 'form',
                bodyStyle: 'padding-bottom:8px;background-color:#ECF5FE;font-weight: bold;font-family: Arial, sans-serif;',
                border: false,
                items: [{
                    xtype: 'label',
                    text: Ext.LabelKey.Admin_CapTypeFilter_lblCapTypeFilterInstruction
                }]
            }, {
                columnWidth: .45,
                layout: 'form',
                bodyStyle: 'padding-bottom:8px;background-color:#ECF5FE',
                border: false,
                items: [{
                    xtype: 'combo',
                    id: 'ddlCapTypeFilter',
                    store: filterStore,
                    displayField: "filter",
                    valueField: "filter",
                    mode: 'local',
                    emptyText: Ext.LabelKey.Admin_CapTypeFilter_ddlEmptyText,
                    editable: false,
                    triggerAction: 'all',
                    height: '100px',
                    hideLabel: true,
                    forceSelection: true,
                    anchor: '90%',
                    loadingText: 'Loading....'
                }]
            }, {
                columnWidth: .05,
                bodyStyle: 'padding-bottom:8px;background-color:#ECF5FE',
                layout: 'form',
                border: false,
                items: [{
                    xtype: 'label',
                    id: 'lblOr',
                    minWidth: 20,
                    text: Ext.LabelKey.Admin_CapTypeFilter_lblOr
                }]
            },
            {
                columnWidth: .5,
                layout: 'form',
                bodyStyle: 'padding-bottom:8px;background-color:#ECF5FE',
                border: false,
                items: [{
                    xtype: 'button',
                    id: 'btnCreateNewFilter',
                    minWidth: 80,
                    text: Ext.LabelKey.Admin_CapTypeFilter_btnCreateNew,
                    handler: function(e) {
                        if (!document.FilterChanged) {
                            CreateAvailableTree();
                            //CreateFilterCapAssociation();
                            //InitNewPageFlowUI();
                            InitNewCapTypeFilterUI();
                            return;
                        }

                        if (confirmMsg(Ext.LabelKey.Admin_CapTypeFilter_Message_CancelFilter)) {
                            CreateAvailableTree();
                            InitNewCapTypeFilterUI();

                            window.parent.RemoveMark(false, 'capTypeFilter');
                            document.FilterChanged = false;
                        }
                    }
                }]
            }, {
                columnWidth: 1,
                layout: 'form',
                bodyStyle: 'padding-top:1px;background-color:#ECF5FE',
                border: false,
                items: [{
                        xtype: 'label',
                        style: 'padding-right:50px',
                        id: 'lblCapSummary',
                        hidden: true,
                        text: Ext.LabelKey.Admin_CapTypeFilter_lblFilterSummary
                    }, {
                        xtype: 'link',
                        id: 'lnkShowCap',
                        hidden: true,
                        style: 'cursor:hand',
                        text: Ext.LabelKey.Admin_CapTypeFilter_lnkShowDetails,
                        alternateText: Ext.LabelKey.Admin_CapTypeFilter_lnkHideDetails,
                        alternateHandler: function() {
                            if (!document.FilterChanged) {
                                Ext.get('filterCapList').dom.style.display = 'none';
                                Ext.get('filterCapAssociation').dom.style.display = 'block';

                                return;
                            }

                            if (confirmMsg(Ext.LabelKey.Admin_CapTypeFilter_Message_CancelFilter)) {
                                Ext.get('filterCapList').dom.style.display = 'none';
                                Ext.get('filterCapAssociation').dom.style.display = 'block';
                                Ext.getCmp('btnFilterOK').disable();
                                Ext.getCmp('btnFilterCancel').disable();

                                window.parent.RemoveMark(false, 'capTypeFilter');
                                document.FilterChanged = false;
                            } else {
                                Ext.getCmp('lnkShowCap').displayHideLink();
                            }
                        },
                        handler: function() {
                            if (!document.FilterChanged) {
                                Ext.get('filterCapList').dom.style.display = 'block';
                                Ext.get('filterCapAssociation').dom.style.display = 'none';

                                var filterName = GetCAPTypeFilterValue();
                                RefreshCapTypeFilterList();

                                return;
                            }

                            if (confirmMsg(Ext.LabelKey.Admin_CapTypeFilter_Message_CancelFilter)) {
                                var filterName = GetCAPTypeFilterValue();
                                Accela.ACA.Web.WebService.AdminConfigureService.GetCapFilterCapTypeList(document.moduleName, filterName, RestoreCapTypeList)
                            } else {
                                Ext.getCmp('lnkShowCap').displayShowLink();
                            }
                        }
                    }]
            }]
    });

    header.render('divFilterHeader');
    header.doLayout();
    ReloadTypeFilter(filterNames);
}

function ReloadTypeFilter(filterNames) {
    var select = $('#pnlHeader input[name="ddlCapTypeFilter"]');
    var divSelect = select.parent("div");
    divSelect.html('');
    var typeSelect = CreateSelect(divSelect);
    typeSelect.options.length = 0;
    typeSelect.options.add(new Option("--Select a Record type filter--", ''));
    for (var i = 0; i < filterNames.length; i++) {
        if (filterNames[i] != '') {
            typeSelect.options.add(new Option(JsonDecode(filterNames[i][0]), JsonDecode(filterNames[i][0])));
        }
    }
}

function CreateSelect(divPanel) {
    var selectType = document.createElement("select");
    selectType.setAttribute("id", "ddlCapTypeFilter");
    selectType.setAttribute("title", "Please select a CAP type filter");
    selectType.className = 'ACA_New_Label';
    selectType.style.width = '100%';
    selectType.style.height = '20px';
    selectType.onchange = TypeSelectIndexChanged;
    divPanel.append(selectType);
    return selectType;
}

function SetCAPTypeFilterValue(value) {
    document.getElementById('ddlCapTypeFilter').value = value;
}

function GetCAPTypeFilterValue() {
    return document.getElementById('ddlCapTypeFilter').value.trim();
}

function TypeSelectIndexChanged() {
    var selecedValue = GetCAPTypeFilterValue();
    if (selecedValue != null && selecedValue != "") {
        if (!document.FilterChanged) {
            SelectFilter(JsonDecode(selecedValue));
            return;
        }

        if (confirmMsg(Ext.LabelKey.Admin_CapTypeFilter_Message_CancelFilter)) {
            SelectFilter(JsonDecode(selecedValue));
            window.parent.RemoveMark(false, 'capTypeFilter');
            document.FilterChanged = false;
        }
    } else {
        Ext.getCmp('btnFilterDelete').disable();
        Ext.get('txtMode').dom.value = 'New';
        ShowSummary(false);
        Ext.get('filterCapList').dom.style.display = 'none';
        Ext.get('filterCapAssociation').dom.style.display = 'none';
    }
}

function RestoreCapTypeList(response){
    var capTypeList = eval('(' + response + ')');
    RefreshFilterCapAssociation(capTypeList,Ext.get('txtMode').dom.value);
    
    Ext.get('filterCapList').dom.style.display='block';
    Ext.get('filterCapAssociation').dom.style.display='none';
    
    RefreshCapTypeFilterList();

    window.parent.RemoveMark(false, 'capTypeFilter');
    document.FilterChanged = false;
}

function addLicenseButton(cellValue, cell, currentRowRecord) {
    var capTypeName = currentRowRecord.data.key;
    var buttonID = "LicenseType" + currentRowRecord.id;
    var disabled = "";
    if (!currentRowRecord.data.LP) {
        disabled = 'disabled = "disabled"';
    }

    var htmlString = '<input ' + disabled + ' id="' + buttonID + '" title="Configure" value="Configure" type="button" width="100" onclick="openLicenseTypeList(\'' + capTypeName + '\',this)" />';

    return htmlString;
}

var recordTypeFilterContext = {
    dialogProperty: {
        itemContainerID: 'recordTypeFilterPopupContainer',
        itemIDPrefix: 'chkrecordTypeFilter',
        positionObjID: null,
        sectionTitle: 'Licensed Professional Types',
        items: null,
        saveButtonID: 'btnSaverecordTypeFilter',
        save: saveList
    }
};

var recordTypeFilterDialog = null;

function openLicenseTypeList(capTypeName,obj)
{
    var moduleName = document.moduleName;
    Accela.ACA.Web.WebService.AdminConfigureService.GetLicenseTypeList(moduleName, capTypeName, "0", function licenseTypeListCallback(callBackLicenseTypeList) {
        var response = eval('(' + callBackLicenseTypeList + ')');
        var options;
        var selectedAllChecked = true;

        if (response != null && response.LisenTypeList != null) {
            options = response.LisenTypeList;
        }

        if (options != null && options.length > 0) {
            recordTypeFilterContext.recordType = capTypeName;
            recordTypeFilterContext.dialogProperty.items = options;
            recordTypeFilterContext.dialogProperty.positionObjID = obj.id;

            if (!recordTypeFilterDialog) {
                recordTypeFilterDialog = new CheckBoxListDialog(recordTypeFilterContext);
            }

            recordTypeFilterDialog.Show();
        }
    });
}

// this function is save the changed items
// params:
//      saveobj:    the webcontrol which will be saved
function saveList(context) {
    var moduleName = document.moduleName;
    var arrVisible = new Array();

    if (context.checkBoxList == null) {
        return;
    }

    $(context.checkBoxList).each(function() {
        if (this.checked) {
            arrVisible.push(this.value);
        }
    })

    Accela.ACA.Web.WebService.AdminConfigureService.SaveLicenseTypeListByCapType(moduleName, context.recordType, "0", arrVisible, function saveLicenseTypeListCallBack(callBackSaveLicenseTypeList) {
        recordTypeFilterDialog.Close();
    })
};

//inital cap type filter grid.
function InitCapTypeFilterRole(){

    var chkAnonymous = new Ext.grid.CapTypeFilterCheckColumn({ header: 'Anonymous', dataIndex: 'anonymous', align: "center", width: 100 });
    var chkReg = new Ext.grid.CapTypeFilterCheckColumn({ header: 'Registered', dataIndex: 'registered', align: "center", width: 100 });
    var chkLP = new Ext.grid.CapTypeFilterCheckColumn({ header: 'Licensed Professional', dataIndex: 'LP', align: "center", width: 120 });
    
    // the column model has information about grid columns
    // dataIndex maps the column to the specific data field in
    // the data store (created below)
    var cm = new Ext.grid.ColumnModel([
        {id:'capType', header: Ext.LabelKey.Admin_RecordTypeFilter_SelectCAPTypes,dataIndex: 'capType',width: 220},
        chkAnonymous,
        chkReg,
        chkLP,
        {header: Ext.LabelKey.Admin_RecordTypeFilter_SpecificLicensedProfessional,renderer:addLicenseButton,align:"center",width: 150},
        {id:'id',hidden:true,hideable:false,dataIndex: 'id'},
        {id:'key',hidden:true,hideable:false,dataIndex: 'key'}
    ]);
    var store = ChangetoStore([['','','','']]);
    // by default columns are sortable
    cm.defaultSortable = true;

    // create the editor grid
    var grid = new Ext.grid.GridPanel({
        id:'grdCayTypeFilter',
        store: store,
        cm: cm,
        autoHeight: true,
        style: 'border-width: 2px; border-color: #B6D2E5; border-style: solid',
        renderTo: 'filterCapList',
        plugins:[chkAnonymous,chkReg,chkLP],
        autoExpandColumn:'capType',
        border:false,
        width:'99.5%'
     });
     
     //create the pannel to add two buttons.
     var panel = new Ext.Panel({
     renderTo: 'filterCapList',
     buttonAlign:'left',
     header:false,
	 
	 border:false,
     buttons:[{
			    id:'btnCapTypeFilterOk',
			    text:Ext.LabelKey.Admin_Global_SelectCapTypeFilter_btnOk,
			    handler:ClickCapTypeFilterSave
		    },
		    {
			    id:'btnCapTypeFilterCannel',
			    text:Ext.LabelKey.Admin_Global_SelectCapTypeFilter_btnCannel,
			    handler:ClickCapTypeFilterCannel
             }
		    ],
     items:[grid]
     });
          
    // trigger the data store load
    store.load();
};
function ClickCapTypeFilterSave()
{
    SaveCapTypeFilter();
}

function ClickCapTypeFilterCannel()
{
    if (confirmMsg(Ext.LabelKey.Admin_CapTypeFilter_Message_CancelFilter)) {
        RefreshCapTypeFilterList();
        window.parent.RemoveMark(false, 'capTypeFilter');
        document.FilterChanged = false;
        IsEnableBtn(false);
    }
}

function IsEnableBtn(flag)
{
    var controlFlag = flag ? false : true;
    
    Ext.getCmp('btnCapTypeFilterOk').setDisabled(controlFlag);
    Ext.getCmp('btnCapTypeFilterCannel').setDisabled(controlFlag);
}

//get latest data and display for cap type filter grid.
function RefreshCapTypeFilterList()
{
    IsEnableBtn(false);
    var arrayData = LoadUserRoleList();
    var store = ChangetoStore(arrayData);
    var grid = Ext.getCmp('grdCayTypeFilter');
    var columnModel = grid.getColumnModel(); 
    grid.reconfigure(store,columnModel);
    grid.getView().refresh();
}

//change to store by array.
function ChangetoStore(arrayData) {
    if (arrayData != null && arrayData.length > 0 && arrayData[0][0] != '') {
        for (var i = 0; i < arrayData.length; i++) {
            var capType = arrayData[i];

            if (capType == null) {
                continue;
            }

            arrayData[i][0] = JsonDecode(capType.text);
            arrayData[i][1] = capType.anon.toUpperCase() == 'N' ? false : true;
            arrayData[i][2] = capType.registered.toUpperCase() == 'N' ? false : true;
            arrayData[i][3] = capType.LP.toUpperCase() == 'N' ? false : true;
            arrayData[i][4] = capType.id;
            arrayData[i][5] = capType.key;
            arrayData[i][6] = capType.aliasName;
        }
    }
    // ArrayReader
    var ds = new Ext.data.Store({
        proxy: new Ext.data.MemoryProxy(arrayData),
        reader: new Ext.data.ArrayReader({ }, [
            { name: 'capType', mapping: 0 },
            { name: 'anonymous', mapping: 1 },
            { name: 'registered', mapping: 2 },
            { name: 'LP', mapping: 3 },
            { name: 'id', mapping: 4 },
            { name: 'key', mapping: 5 },
            { name: 'aliasName', mapping: 6 }
        ])
    });
    ds.load();

    return ds;
}

Ext.grid.CapTypeFilterCheckColumn = function(config) {
    Ext.apply(this, config);
    if(!this.id){
        this.id = Ext.id();
    }
    this.renderer = this.renderer.createDelegate(this);
};

Ext.grid.CapTypeFilterCheckColumn.prototype = {
    init: function(grid) {
        this.grid = grid;
    },
    renderer: function(v, p, record) {
        var checked = v ? "checked" : "";
        var disabled = "";
        var title = record.data['capType'] + ', ' + this.dataIndex;

        return String.format('<input type="checkbox" id="CapTypeFilterRole_{0}_{1}"  title="{2}" {3} {4} onclick="grdCayTypeFilter_CheckboxClick(\'{0}\', this)">',
        	this.dataIndex, record.id, title, checked, disabled);
    }
}

function grdCayTypeFilter_CheckboxClick(dataIndex, checkbox) {
    var grid = Ext.getCmp('grdCayTypeFilter');
    var index = grid.getView().findRowIndex(checkbox);

    var record = grid.store.getAt(index);
    record.data[dataIndex] = checkbox.checked;
    window.parent.ModifyMark('capTypeFilter');
    IsEnableBtn(true);
    document.FilterChanged = true;

    if (dataIndex == "LP") {
        $get("LicenseType" + record.id).disabled = !record.data[dataIndex];
    }
}

function InitCapTypeFilterUI(){
    Ext.get('txtMode').dom.value = 'New';
    SetCAPTypeFilterValue('');
    ShowSummary(false);
    Ext.get('filterCapList').dom.style.display='none';
    Ext.get('filterCapAssociation').dom.style.display='none';  
    AdjustCapPanel();
}

function InitUIAfterCancel(){
    if(Ext.get('txtMode').dom.value == 'New'){
        Ext.get('filterCapList').dom.style.display='none';
        ShowSummary(false);
    }
    else{
        Ext.get('filterCapList').dom.style.display='block';
        ShowSummary(true);
    }
    Ext.get('filterCapAssociation').dom.style.display='none';
}

function InitFilterCapAssociation(){
    var btnAddCap = new Ext.Button({
            id:'btnAddFilterCap',
            disabled:true,
            minWidth:80,
            text:Ext.LabelKey.Admin_CapTypeFilter_btnAddCap,
            renderTo:'addFilterCap'
        });
    var btnRemove = new Ext.Button({
            disabled:true,
            minWidth:80,
            text:Ext.LabelKey.Admin_CapTypeFilter_btnRemoveCap,
            renderTo:'removeFilterCap'
        });
    var btnOK = new Ext.Button({
            id:'btnFilterOK',
            disabled:true,
            minWidth:80,
            text:Ext.LabelKey.Admin_CapTypeFilter_btnOK,
            handler: SaveCapTypeFilter,
            renderTo:'btnFilterOK'
        });
    var btnCancel = new Ext.Button({
            id: 'btnFilterCancel',
            minWidth: 80,
            disabled: true,
            text: Ext.LabelKey.Admin_CapTypeFilter_btnCancel,
            handler: function() {
                if (!document.FilterChanged) {
                    return;
                }

                if (confirmMsg(Ext.LabelKey.Admin_CapTypeFilter_Message_CancelFilter)) {
                    var filterName = GetCAPTypeFilterValue();
                    Accela.ACA.Web.WebService.AdminConfigureService.GetCapFilterCapTypeList(document.moduleName, filterName, CancelCapTypeList);
                }
            },
            renderTo: 'btnFilterCancel'
        });
    var btnDelete = new Ext.Button({
            id:'btnFilterDelete',
            disabled:true,
            minWidth:80,
            text:Ext.LabelKey.Admin_CapTypeFilter_btnDelete,
            handler: DeleteCapTypeFilter,
            renderTo:'btnFilterDelete'
        });
        
    var formCapTypeFilterName = new Ext.form.FormPanel({
        labelAlign:'Left',
	    header:false,
	    bodyStyle : 'padding-top:10px',
	    renderTo:'NewCapTypefilterName',
	    //frame:true,
	    labelWidth:150,
	    cls:'padding:5px',
	    border:false,
	    items:[{
		    layout:'column',
		    border:false,
		    items:[{
			    columnWidth:1,
			    layout:'form',
			    border:false,
			    items:[{
					    xtype:'textfield',
					    //allowBlank: false,
					    emptyText :Ext.LabelKey.Admin_CapTypeFilter_txtFilterNameEmptyText,
					    id: 'txtCapTypeFilterName',
					    style: 'margin-top: 1px',
                        fieldLabel:Ext.LabelKey.Admin_CapTypeFilter_txtCapTypeFilterNamelabel,
                        maxLength:50,
                        minLength:1,
                        width:300,
                        listeners: {
                             'blur': function(){
                                    //accela requires a page flow needs to be created without selecting any cap
                                    document.FilterChanged = true;
                                    
                                    CheckFilterStatus();
                                }
                        }
				    },{
				        xtype:'hidden',
					    id:'txtMode'
				    }]
		    }]
	    }]
	});
    	
    var capAvailable = new Ext.tree.CapPanel({
        id:'pnlFilterCapAvailable',
    	title:Ext.LabelKey.Admin_CapTypeFilter_pnlCapAvailableTitle,
    	animate:true,
		style:'padding:10px 10px 20px 20px',
		height:300,
		lines:false,
		root:new Ext.tree.AsyncTreeNode({
		        id:'rootCapAvailable',
		        childNodes:{}
		    }),
		rootVisible:false,
		enableDD:true,
		containerScroll: true,
		autoScroll:true,
		selModel:new Ext.tree.MultiSelectionModel()
    });
    capAvailable.render('availableFilterCap');
    
    var capSelected = new Ext.tree.CapPanel({
        id:'pnlFilterCapSelected',
        title:Ext.LabelKey.Admin_CapTypeFilter_pnlCapSelectedTitle,
    	animate:true,
		style:'padding:10px 10px 20px 20px',
		height:300,
		lines:false,
		root:new Ext.tree.AsyncTreeNode({
		        id:'rootCapSelected',
		        childNodes:{}
		     }),
		rootVisible:false,
		enableDD:true,
		enableDrop: true,
		containerScroll: true,
		autoScroll:true,
		dropConfig:{allowContainerDrop:true},
		selModel:new Ext.tree.MultiSelectionModel()//FixedMultiSelectionModel()
    });
    capSelected.render('selectedFilterCap');
    
    var dropTree1 = new Ext.dd.DropTarget(capAvailable.el.dom.childNodes[1], {
              ddGroup : 'TreeDD',
              notifyDrop : function(dd, e, data){
                OnFilterNodeDrop(dd,e,data,capAvailable);
              }
    });
    var dropTree2 = new Ext.dd.DropTarget(capSelected.el.dom.childNodes[1], {
          ddGroup : 'TreeDD',
          notifyDrop : function(dd, e, data){
              return OnFilterNodeDrop(dd,e,data,capSelected);
          }
    });

    new Ext.tree.TreeSorter(capAvailable,{folderSort:true}); 
    new Ext.tree.TreeSorter(capSelected,{folderSort:true}); 

    capAvailable.on('click',function(node,e){
        btnAddCap.enable();
    });
    
    capAvailable.on('remove',function(){
        if(capAvailable.getRootNode().childNodes.length<1){
            btnAddCap.disable();
        }
    });

    capSelected.on('click',function(){
        btnRemove.enable();
    });
    
    capSelected.on('append',function(){
        btnAddCap.disable();
    });
    
    capSelected.on('remove',function(){
        if(capSelected.getRootNode().childNodes.length<1){
            btnRemove.disable();
        }
    });
    
    btnRemove.on('click',function(e){
        var selectedNodes = capSelected.getSelectionModel().getSelectedNodes();
        var availableRoot = capAvailable.getRootNode();
        var selectedRoot = capSelected.getRootNode();
        
        while(selectedNodes.length>0){
            var node = selectedNodes[0].remove();
            node.attributes.binded = false;
            
            availableRoot.appendChild(node);
        }
        
        btnRemove.disable();
        document.FilterChanged = true;
        CheckFilterStatus();
    });
    
    btnAddCap.on('click',function(){
        var selectedNodes = capAvailable.getSelectionModel().getSelectedNodes();
        var selectedRoot = capSelected.getRootNode();
        
        document.FilterChanged = true;
        AddFilterCapAssociation(selectedRoot,selectedNodes);
        CheckFilterStatus();
    });
    
//    btnCancel.on('click',CancelEdit);
};

function OnFilterNodeDrop(dd, e, data, targetTree) {
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

    var node = dd.tree.getRootNode().removeChild(data.node);
    node.attributes.anon = 'Y';
    node.attributes.registered = 'Y';
    node.attributes.LP = 'Y';

    targetTree.getRootNode().appendChild(node);
    document.FilterChanged = true;
    CheckFilterStatus();

    return true;
}

function CancelCapTypeList(response){
    var capTypeList = eval('(' + response + ')');
    var mode = Ext.get('txtMode').dom.value;
    RefreshFilterCapAssociation(capTypeList,mode);
    
    //RefreshCapTypeFilterList();
    Ext.getCmp('btnFilterCancel').disable();
    Ext.getCmp('btnFilterOK').disable();
    
    if(mode == 'New'){
        Ext.getCmp('txtCapTypeFilterName').setValue('');
    }

    window.parent.RemoveMark(false, 'capTypeFilter');
    document.FilterChanged = false;
}

function SelectFilter(filter){
    var txtCapTypeFilterName = Ext.getCmp('txtCapTypeFilterName');
    txtCapTypeFilterName.setValue(filter);
    txtCapTypeFilterName.disable();
    
    Ext.get('txtMode').dom.value = 'Edit';
    if(filter.length >0){
        ShowSummary(true);
    }
    else{
        ShowSummary(false);
    }

    Ext.getCmp('lnkShowCap').displayShowLink();
    
    Ext.get('filterCapList').dom.style.display='none';
    Ext.get('filterCapAssociation').dom.style.display='block';
    
    //LoadCapTypeFilter(filter);
    Accela.ACA.Web.WebService.AdminConfigureService.GetCapFilterCapTypeList(document.moduleName, filter, LoadFilterCallback);
}

function ShowSummary(show){
    if(show){
         Ext.getCmp('lblCapSummary').show();
         Ext.getCmp('lnkShowCap').show();
    }
    else{
        Ext.getCmp('lblCapSummary').hide();
        Ext.getCmp('lnkShowCap').hide();
    }  
}

function LoadFilterCallback(response){
    var capTypeList = eval('(' + response + ')');
    
    //RefreshCapList(filter);
    RefreshFilterCapAssociation(capTypeList,'Edit');
    
    var count = 0;
    
    if(capTypeList.SelectedCapTypes!=null && capTypeList.SelectedCapTypes.length >0){
        count = capTypeList.SelectedCapTypes.length - 1;

        if (count < 0) {
            count = 0;
        }
    }

    RefreshFilterHeader(count);
    
    Ext.getCmp('btnFilterDelete').enable();
    Ext.getCmp('btnFilterCancel').disable();
}

function DeleteCapTypeFilter(){
    var confirmed = confirm(Ext.LabelKey.Admin_CapTypeFilter_Message_DeleteFilter);
    
    if(confirmed){
        var filterName = Ext.getCmp('txtCapTypeFilterName').getValue().trim();
        
        if(filterName != ''){
            Accela.ACA.Web.WebService.AdminConfigureService.CheckFilterRelation(filterName, document.moduleName, CheckCallback);
        }
    }
}

function CheckCallback(response){
    var filterName = Ext.getCmp('txtCapTypeFilterName').getValue().trim();
    
    if(response){
         if (confirmMsg(Ext.LabelKey.Admin_CapTypeFilter_Message_DeleteBindedFilter)) {
               Accela.ACA.Web.WebService.AdminConfigureService.DeleteCapTypeFilter(filterName, document.moduleName, DeleteCallback);
         }
    }
    else{
        Accela.ACA.Web.WebService.AdminConfigureService.DeleteCapTypeFilter(filterName, document.moduleName, DeleteCallback);
    }

    var selectType = document.getElementById("ddlCapTypeFilter");
    for (var i = 0; i < selectType.options.length; i++) {
        if (selectType.options[i].value == filterName) {
            selectType.options.remove(i);
            break;
        }
    }

    RemoveAmendmentFilter(filterName);
}

function DeleteCallback(response){
    InitCapTypeFilterUI();
}

function SaveCapTypeFilter(parent) {
    if (!document.FilterChanged) {
        return;
    }
    var mode = Ext.get('txtMode').dom.value;
    var txtCapTypeFilterName = Ext.getCmp('txtCapTypeFilterName');
    var filterName = txtCapTypeFilterName.getValue().trim();

    if (filterName == "") {
        alert(Ext.LabelKey.Admin_FilterName_Cannotempty);
        return -1;
    }

    if (filterName.length > 50) {
        alert(Ext.LabelKey.Admin_CapTypeFilter_Message_InvalidFilterNameLength);
        return -1;
    }

    if (CheckExistingFilter(mode, filterName)) {
        alert(Ext.LabelKey.Admin_CapTypeFilter_Message_InvalidFilterName);
        return -1;
    }

    if (parent == null) {
        document.callFromParent = false;
    } else {
        document.callFromParent = true;
    }

    var filterItem = GetFilterItem(filterName);

    if (Ext.get('txtMode').dom.value == 'New') {
        Accela.ACA.Web.WebService.AdminConfigureService.CreateCapTypeFilter(filterItem, SaveSuccess);
    } else {
        Accela.ACA.Web.WebService.AdminConfigureService.UpdatControlFilter(filterItem, SaveSuccess);
    }

    IsEnableBtn(false);
}

function SaveFail(){
   window.parent.Ext.MessageBox.hide();
   alert('An error has occurred.\nWe are experiencing technical difficulties.\nPlease try again later or contact the City for assistance.');
}

function SaveSuccess(response) {
    if (Ext.get('txtMode').dom.value == 'New') {
        var txtCapTypeFilterName = Ext.getCmp('txtCapTypeFilterName');
        var filterName = txtCapTypeFilterName.getValue().trim();
        var selectType = document.getElementById('ddlCapTypeFilter');
        selectType.options.add(new Option(filterName, filterName));
        SetCAPTypeFilterValue(filterName);
        SetAmendmentFilterValue(filterName);
    }

    RefreshFilterHeader(Ext.getCmp('pnlFilterCapSelected').getNodes().length);
    Ext.get('txtMode').dom.value = 'Edit';
    Ext.getCmp('btnFilterCancel').disable();
    Ext.getCmp('btnFilterOK').disable();
    Ext.getCmp('btnFilterDelete').enable();

    if (txtCapTypeFilterName != null) {
        txtCapTypeFilterName.disable();
    }

    ShowSummary(true);
    document.FilterChanged = false;

    try {
        window.parent.RemoveMark(false, 'capTypeFilter');
    } catch(e) {
    }

    if (!document.callFromParent) {
        alert(Ext.LabelKey.Admin_CapTypeFilter_Message_SaveSuccessfully);
        document.callFromParent = false;
    }
}

function ReloadCboFilterName(filterNames){
    var filterName = Ext.getCmp('txtCapTypeFilterName').getValue().trim();
    SetCAPTypeFilterValue(filterName);
        
    var filterStore = new Ext.data.SimpleStore({
        fields:['filter'],
        data:filterNames
    });

    if(Ext.get('txtMode').dom.value == 'New'){    
        SetCAPTypeFilterValue(filterName);
    }
}

function GetFilterItem(filterName){
    var filterItem = new CapTypeFilterItem();

    if(Ext.get('filterCapAssociation').dom.style.display == 'block'){
        filterItem.CapTypeList = GetCapTypeList();
    }
    else{
        filterItem.CapTypeList = GetUserRoleList();
        
        UpdateSelectedTree(filterItem.CapTypeList);
    }
    
    filterItem.FilterName = filterName;
    filterItem.ModuleName = document.moduleName;
    
    return filterItem;
}

function LoadUserRoleList(){
    var userRoleList = new Array();
    var nodes = Ext.getCmp('pnlFilterCapSelected').getNodes();
    //var length = filterItem.CapTypeList.length;
        
    for(var i=0;i<nodes.length;i++){
        userRoleList[userRoleList.length] = nodes[i].attributes;
    }
    return userRoleList;
}

function GetCapTypeList(){
    var nodes = Ext.getCmp('pnlFilterCapSelected').getNodes();
    var capTypeList = new Array();
        
    for(var i=0;i<nodes.length;i++){
        var capType = new Object();
        
        capType.text = JsonEncode(nodes[i].text);
        capType.key = JsonEncode(nodes[i].attributes.key);
        capType.aliasName = JsonEncode(nodes[i].attributes.aliasName);
        capType.anon = nodes[i].attributes.anon;
        capType.registered = nodes[i].attributes.registered;
        capType.LP = nodes[i].attributes.LP;
        capType.id = nodes[i].attributes.id;
        
        capTypeList[capTypeList.length] = capType;
    }
    
    return capTypeList;
}

function GetUserRoleList(){
    
    var store =   Ext.getCmp('grdCayTypeFilter').getStore();
    var resUserRoleList = new Array();
    
    for (var i = 0; i < store.data.length; i++) 
    {
        var record = store.getAt(i);
        var capType = new Object();
        
        capType.text = JsonEncode(record.get("capType"));
        capType.anon = record.get('anonymous') ? 'Y' : 'N';
        capType.registered = record.get("registered") ? 'Y' : 'N'; 
        capType.LP = record.get('LP') ? 'Y' : 'N';
        capType.binded = true;
        capType.draggable = true;
        capType.leaf = true;
        capType.id = record.get("id");
        capType.key = record.get("key");
        capType.aliasName = record.get("aliasName");
        
        resUserRoleList[resUserRoleList.length]= capType; 
    }
    
    return resUserRoleList;
}

function CheckExistingFilter(mode,name){
    if(mode== 'Edit'){
        return false;
    }

    var filterName = name.trim().toUpperCase();
    var selectType = document.getElementById("ddlCapTypeFilter");
    for (var i = 0; i < selectType.options.length; i++) {
        if (selectType.options[i].value.toUpperCase() == filterName) {
            return true;
        }
    }
    
    return false;
}
/****************************/


function RefreshFilterHeader(count){
    var lblSummary = Ext.getCmp('lblCapSummary')
    var reg = /\d+/;
        
    lblSummary.setText(lblSummary.text.replace(reg,count));
}

function RefreshFilterCapAssociation(capTypeFilter,action){
    CreateAvailableTree(capTypeFilter.SelectedCapTypes);
    //CreateSelectedTree()
    
    AdjustCapPanel();
}

function CreateFilterCapAssociation(){
    Accela.ACA.Web.WebService.AdminConfigureService.GetCapFilterCapTypeList(document.moduleName, '', CreateFilterCallback);
}

function CreateFilterCallback(response){
    var capTypeList = eval('(' + response + ')');
    
    CreateAvailableTree(capTypeList.AvailableCapTypes);
    CreateSelectedTree(capTypeList.SelectedCapTypes);
    
    InitNewCapTypeFilterUI();
}

function InitNewCapTypeFilterUI(){
    Ext.get('txtMode').dom.value = 'New';
    SetCAPTypeFilterValue('');
    //Ext.get('divFilterBody').dom.style.display = 'block';
    Ext.get('filterCapList').dom.style.display='none';
    Ext.get('filterCapAssociation').dom.style.display='block';
    Ext.getCmp('txtCapTypeFilterName').setDisabled(false);
    Ext.getCmp('txtCapTypeFilterName').setValue('');
    Ext.getCmp('btnFilterDelete').disable();
    Ext.getCmp('btnFilterCancel').disable();
    Ext.getCmp('btnFilterOK').disable();
    Ext.getCmp('lnkShowCap').displayShowLink();
    ShowSummary(false);
    
    AdjustCapPanel();
}

function CreateAvailableTree(selectedCaps){
    var availableTree = Ext.getCmp('pnlFilterCapAvailable');
    var selectedTree = Ext.getCmp('pnlFilterCapSelected');
    if(availableTree.root.childNodes.length > 0 || selectedTree.root.childNodes.length > 0) 
    {
        if(selectedTree.root.childNodes.length > 0) 
        {
            availableTree.root.beginUpdate();
            selectedTree.root.beginUpdate();
            for(var i=selectedTree.root.childNodes.length-1;i>=0;i--)
            {
                 availableTree.root.appendChild(selectedTree.root.removeChild(selectedTree.root.childNodes[i]));
            }
            availableTree.root.endUpdate();  
            selectedTree.root.endUpdate();  
        }
        if(selectedCaps != null && selectedCaps.length > 0){
            CreateSelectedTree(selectedCaps)
        }
    } 
    else
    {
        document.selectedCaps = selectedCaps;
        Accela.ACA.Web.WebService.AdminConfigureService.GetAllCapTypeList(document.moduleName, IntialAvailableTree); 
    }
}

function IntialAvailableTree(response) {
    var capTypeList = eval('(' + response + ')');
    var availableTree = Ext.getCmp('pnlFilterCapAvailable');
    availableTree.createTree(capTypeList.AvailableCapTypes, '');
    CreateSelectedTree(document.selectedCaps);
}

function CreateSelectedTree(selectedCapTypes) {
    var selectedTree = Ext.getCmp('pnlFilterCapSelected');
    selectedTree.clear();
    if (selectedCapTypes != null && selectedCapTypes.length > 0) {
        var availableTree = Ext.getCmp('pnlFilterCapAvailable');
        availableTree.root.beginUpdate();
        selectedTree.root.beginUpdate();
        for (var i = 0; i < selectedCapTypes.length; i++) {
            if (selectedCapTypes[i]) {
                for (var j = 0; j < availableTree.root.childNodes.length; j++) {
                    if (selectedCapTypes[i].text == availableTree.root.childNodes[j].text) {
                        var selectedNode = availableTree.root.childNodes[j];

                        // Remove Selected Cap Type from Available Tree                       
                        availableTree.root.removeChild(selectedNode)

                        // Reset Selected Cap Type's Attributes to show in list correctly 
                        selectedNode.attributes.LP = selectedCapTypes[i].LP;
                        selectedNode.attributes.anon = selectedCapTypes[i].anon;
                        selectedNode.attributes.registered = selectedCapTypes[i].registered;

                        // Append Select Cap Type to Available Tree  
                        selectedTree.root.appendChild(selectedNode);
                        break;
                    }
                }
            }
        }
        availableTree.root.endUpdate();
        selectedTree.root.endUpdate();
    }
}

function UpdateSelectedTree(selectedCaps){    
   if(selectedCaps != null && selectedCaps.length > 0)
   {
       var selectedTree = Ext.getCmp('pnlFilterCapSelected');
        selectedTree.createTree(selectedCaps,'');
        var availableTree = Ext.getCmp('pnlFilterCapAvailable');
        if(availableTree.root.childNodes.length > 0) 
        {
            CreateAvailableTree();  
            CreateSelectedTree(selectedCaps);     
        }   
   }  
}

function CheckFilterStatus(){
    var btnOK = Ext.getCmp('btnFilterOK');
    var btnCancel = Ext.getCmp('btnFilterCancel');
    var txtFilterName = Ext.getCmp('txtCapTypeFilterName');
    
    //Accela requires a flow needs to be created without selecting any cap
    //if(txtSmartchoice.getValue().trim().length>0 && txtSmartchoice.isValid && pnlCap.getNodesCount()>0 && document.FilterChanged){
    if(txtFilterName.getValue().trim().length>0 && txtFilterName.isValid && document.FilterChanged){
        btnOK.enable();
        btnCancel.enable();
        window.parent.ModifyMark('capTypeFilter');
    }
    else{
        btnOK.disable();
    }
}

function AddFilterCapAssociation(root, selectedNodes) {
    while (selectedNodes.length > 0 && selectedNodes[0] != null) {
        var node = selectedNodes[0].remove();
        node.attributes.anon = 'Y';
        node.attributes.registered = 'Y';
        node.attributes.LP = 'Y';
        node.attributes.binded = true;
        root.appendChild(node);
    }

    CheckFilterStatus();

    if (selectedNodes.length < 1) {
        Ext.getCmp('btnAddFilterCap').disable();
    }
}

function AdjustCapPanel() {
    var width = Ext.getCmp('pnlHeader').body.dom.offsetWidth;

    if (width) {
        var capAvailable = Ext.getCmp("pnlFilterCapAvailable");
        var capSelected = Ext.getCmp("pnlFilterCapSelected");
        var adjWidth = width * 0.4; //(Ext.workflowWidth -257)/2;

        if (capSelected != null) {
            capSelected.setWidth(adjWidth);
        }
        if (capAvailable != null) {
            capAvailable.setWidth(adjWidth);
        }

        if (!Ext.isIE) {
            //document.getElementById('addCap').parentNode.width = Ext.workflowWidth - adjWidth * 2 - 40;
            document.getElementById('addFilterCap').parentNode.width = width * 0.1;
        } else {
            //document.getElementById('addCap').parentElement.width = Ext.workflowWidth - adjWidth * 2 - 40;
            document.getElementById('addFilterCap').parentElement.width = width * 0.1;
        }
    }
}