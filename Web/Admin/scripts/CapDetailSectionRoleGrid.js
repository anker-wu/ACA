/**
* <pre>
* 
*  Accela Citizen Access Admin
*  File: CapDetailSectionRoleGrid.js
* 
*  Accela, Inc.
*  Copyright (C): 2010-2014
* 
*  Description:
* 
*  Notes:
*      $Id: CapDetailSectionRoleGrid.js 266252 2014-02-20 10:26:50Z ACHIEVO\james.shi $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

var sectionRoleBtnOK;
var sectionRoleBtnCancel;
var CapDetailSectionRoleGrid;
var sectionRoleGridName = "CapDetailSectionRoleGrid";
var sectionRoleColumnHeader;
var specificLPLabel = "Specific Licensed Professional";
var btnInputContactUserTypeConfig;
var btnViewContactUserTypeConfig;
var btnScheduleInspectionUserTypeConfig;
var inputContactRightName = "INPUT_RIGHT";
var viewContactRightName = "VIEW_RIGHT";

// Cap Detail Section Role Grid column enum
var SectionRoleGridColumn = {
    LP: 7,
    LPConfig: 8,
    Agent: 9,
    AgentClerk: 10
};

Ext.onReady(function () { // render sectionType grid and buttons
    Ext.state.Manager.setProvider(new Ext.state.CookieProvider());
    document.moduleName = getparmByUrl('moduleName');

    InitRolesForSections();

    // render config button in inspection user role for input contact
    btnInputContactUserTypeConfig = new Ext.Button({
        id: "btnInputContactUserTypeConfig",
        text: specificLPLabel,
        type: "button",
        disabled: true,
        handler: function() {
            openSectionRoleLicenseList(Ext.Constant.INSPECTION_CONTACT_RIGHT, inputContactRightName, this);
        }
    });
    btnInputContactUserTypeConfig.render('divInputContactUserTypeConfig');

    // render config button in inspection user role for view contact
    btnViewContactUserTypeConfig = new Ext.Button({
        id: "btnViewContactUserTypeConfig",
        text: specificLPLabel,
        type: "button",
        disabled: true,
        handler: function() {
            openSectionRoleLicenseList(Ext.Constant.INSPECTION_CONTACT_RIGHT, viewContactRightName, this);
        }
    });
    btnViewContactUserTypeConfig.render('divViewContactUserTypeConfig');

    // render config button in inspection user role for different license type.
    btnScheduleInspectionUserTypeConfig = new Ext.Button({
        id: "btnScheduleInspectionUserTypeConfig",
        text: specificLPLabel,
        type: "button",
        disabled: true,
        handler: function() {
            openSectionRoleLicenseList(Ext.Constant.INSPECTION_PERMISSION_USER_ROLES, "N/A", this);
        }
    });
    btnScheduleInspectionUserTypeConfig.render('divScheduleInspectionUserTypeConfig');

    // render OK and Cancel buttons
    sectionRoleBtnOK = new Ext.Button({ id: "sectionRoleBtnOK", text: " OK ", type: "button", minWidth: "80", handler: btnSectionRoleOK_Clicked, disabled: true });
    sectionRoleBtnOK.render('tdSaveSectionRole');

    sectionRoleBtnCancel = new Ext.Button({ id: "sectionRoleBtnCancel", text: " Cancel ", type: "button", minWidth: "80", handler: btnSectionRoleCancel_Clicked, disabled: true });
    sectionRoleBtnCancel.render('tdCancelSaveSectionRole');
});

function btnSectionRoleCancel_Clicked() {
    DisabledSectionRoleButtons(true);

    RefreshSectionRoleDataStore();
}

//inital Role For Sections.
function InitRolesForSections() {
    sectionRoleColumnHeader = ["Key", "Page Sections",
        Ext.LabelKey.Admin_Searchrole_Gridtitle_Acausers, Ext.LabelKey.Admin_Searchrole_Gridtitle_RegisteredUsers,
        Ext.LabelKey.Admin_Searchrole_Gridtitle_RecordCreator, Ext.LabelKey.Admin_Searchrole_Gridtitle_Contact, Ext.LabelKey.Admin_Searchrole_Gridtitle_Owner,
        Ext.LabelKey.Admin_Searchrole_Gridtitle_LicensedProfessional, specificLPLabel, Ext.LabelKey.Admin_Searchrole_Gridtitle_AgentUsers, Ext.LabelKey.Admin_Searchrole_Gridtitle_AgentClerkUsers];
    var chkAllACAUser = new Ext.grid.SectionRoleCheckColumn({ header: sectionRoleColumnHeader[2], align: 'center', width: 100, dataIndex: sectionRoleColumnHeader[2] });
    var chkRegUser = new Ext.grid.SectionRoleCheckColumn({ header: sectionRoleColumnHeader[3], align: 'center', width: 100, dataIndex: sectionRoleColumnHeader[3] });
    var chkCapCreater = new Ext.grid.SectionRoleCheckColumn({ header: sectionRoleColumnHeader[4], align: 'center', width: 100, dataIndex: sectionRoleColumnHeader[4] });
    var chkContact = new Ext.grid.SectionRoleCheckColumn({ header: sectionRoleColumnHeader[5], align: 'center', width: 100, dataIndex: sectionRoleColumnHeader[5] });
    var chkOwner = new Ext.grid.SectionRoleCheckColumn({ header: sectionRoleColumnHeader[6], align: 'center', width: 100, dataIndex: sectionRoleColumnHeader[6] });
    var chkLP = new Ext.grid.SectionRoleCheckColumn({ header: sectionRoleColumnHeader[SectionRoleGridColumn.LP], align: 'center', width: 130, dataIndex: sectionRoleColumnHeader[SectionRoleGridColumn.LP] });
    var btnLPConfig = new Ext.grid.SectionRoleCheckColumn({ header: sectionRoleColumnHeader[SectionRoleGridColumn.LPConfig], align: 'center', width: 150, renderer: addRDLicenseTypeButton, dataIndex: sectionRoleColumnHeader[SectionRoleGridColumn.LPConfig] });
    var chkAgent = new Ext.grid.SectionRoleCheckColumn({ header: sectionRoleColumnHeader[SectionRoleGridColumn.Agent], align: 'center', width: 100, dataIndex: sectionRoleColumnHeader[SectionRoleGridColumn.Agent] });
    var chkAgentClerk = new Ext.grid.SectionRoleCheckColumn({ header: sectionRoleColumnHeader[SectionRoleGridColumn.AgentClerk], align: 'center', width: 100, dataIndex: sectionRoleColumnHeader[SectionRoleGridColumn.AgentClerk] });

    // the column model has information about grid columns
    // dataIndex maps the column to the specific data field in
    // the data store (created below)
    var cm = new Ext.grid.ColumnModel([
        { id: sectionRoleColumnHeader[0], header: sectionRoleColumnHeader[0], hidden: true, hideable: false, width: 0, dataIndex: sectionRoleColumnHeader[0] },
        { header: sectionRoleColumnHeader[1], align: 'left', width: 250, renderer: renderCapTypeLabel, dataIndex: sectionRoleColumnHeader[1] },
        chkAllACAUser,
        chkRegUser,
        chkCapCreater,
        chkContact,
        chkOwner,
        chkLP,
        btnLPConfig,
        chkAgent,
        chkAgentClerk
    ]);

    // Sort record section role list by first column.
    SectionRoleList.sort(function(x, y) {
        return x[1].localeCompare(y[1]);
    });

    var store = ChangeRecordDetailRolesStore(SectionRoleList);

    var gridWidth = 720;
    if (isFireFox()) {
        gridWidth = 700;
    }
    // create the editor grid
    CapDetailSectionRoleGrid = new Ext.grid.GridPanel({
        id: 'recordDetailRoleGrid',
        store: store,
        cm: cm,
        style: 'border-width: 2px; border-color: #B6D2E5; border-style: solid',
        renderTo: 'CapDetailSectionRoleGrid',
        plugins: [chkAllACAUser, chkRegUser, chkCapCreater, chkContact, chkOwner, chkAgent, chkAgentClerk, chkLP],
        autoExpandColumn: 'Key',
        border: false,
        height: 250,
        width: '100%',
        title: 'Role Assignment for Record Detail Page Sections'
    });

    chkAllACAUser.readonly = true;
};

//change to store by array.
function ChangeRecordDetailRolesStore(arrayData) {
    if (arrayData != null && arrayData.length > 0 && arrayData[0][0] != '') {
        for (var i = 0; i < arrayData.length; i++) {
            var sectionType = arrayData[i];

            if (sectionType == null) {
                continue;
            }

            arrayData[i][0] = sectionType[0];
            arrayData[i][1] = RemoveColon4Label(sectionType[1]);
            arrayData[i][2] = sectionType[2]; //All ACA User
            arrayData[i][3] = sectionType[3]; //Cap Creator
            arrayData[i][4] = sectionType[4]; //LP
            arrayData[i][5] = sectionType[5]; //Contact
            arrayData[i][6] = sectionType[6]; //Owner
            arrayData[i][7] = sectionType[7]; //registered.
            arrayData[i][8] = sectionType[8]; //Agent.
            arrayData[i][9] = sectionType[9]; //Clerk.
        }
    }
    // ArrayReader
    var ds = new Ext.data.Store({
        proxy: new Ext.data.MemoryProxy(arrayData),
        reader: new Ext.data.ArrayReader({ }, [
            { name: sectionRoleColumnHeader[0], mapping: 0 },
            { name: sectionRoleColumnHeader[1], mapping: 1 },
            { name: sectionRoleColumnHeader[2], mapping: 2 },
            { name: sectionRoleColumnHeader[4], mapping: 3 },
            { name: sectionRoleColumnHeader[SectionRoleGridColumn.Agent], mapping: 8 },
            { name: sectionRoleColumnHeader[5], mapping: 5 },
            { name: sectionRoleColumnHeader[6], mapping: 6 },
            { name: sectionRoleColumnHeader[3], mapping: 7 },
            { name: sectionRoleColumnHeader[SectionRoleGridColumn.AgentClerk], mapping: 9 },
            { name: sectionRoleColumnHeader[SectionRoleGridColumn.LP], mapping: 4 }
        ])
    });
    ds.load();

    return ds;
}

function GetRecordDetailRoleList() {
    var store = Ext.getCmp('recordDetailRoleGrid').getStore();

    for (var i = 0; i < store.data.length; i++) {
        var record = store.getAt(i);
        var sectionType = new Object();

        sectionType[0] = record.get(sectionRoleColumnHeader[0]);
        sectionType[1] = record.get(sectionRoleColumnHeader[1]);
        sectionType[2] = record.get(sectionRoleColumnHeader[2]) ? 1 : 0;
        sectionType[3] = record.get(sectionRoleColumnHeader[3]) ? 1 : 0;
        sectionType[4] = record.get(sectionRoleColumnHeader[4]) ? 1 : 0;
        sectionType[5] = record.get(sectionRoleColumnHeader[5]) ? 1 : 0;
        sectionType[6] = record.get(sectionRoleColumnHeader[6]) ? 1 : 0;
        sectionType[7] = record.get(sectionRoleColumnHeader[SectionRoleGridColumn.Agent]) ? 1 : 0;
        sectionType[8] = record.get(sectionRoleColumnHeader[SectionRoleGridColumn.AgentClerk]) ? 1 : 0;
        sectionType[9] = record.get(sectionRoleColumnHeader[SectionRoleGridColumn.LP]) ? 1 : 0;

        SectionRoleList[i][0] = sectionType[0];
        SectionRoleList[i][1] = sectionType[1];
        SectionRoleList[i][2] = sectionType[2]; //All ACA User 
        SectionRoleList[i][3] = sectionType[4]; //Cap Creator
        SectionRoleList[i][4] = sectionType[9]; //LP
        SectionRoleList[i][5] = sectionType[5]; //Contact
        SectionRoleList[i][6] = sectionType[6]; //Owner
        SectionRoleList[i][7] = sectionType[3]; //registered.
        SectionRoleList[i][8] = sectionType[7]; //Agent.
        SectionRoleList[i][9] = sectionType[8]; //Clerk.
    }
}

function renderCapTypeLabel(cellValue, cell, currentRowRecord) {
    return String.format("<label>{0}</label>", cellValue);
}

Ext.grid.SectionRoleCheckColumn = function (config) {
    Ext.apply(this, config);
    if (!this.id) {
        this.id = Ext.id();
    }

    this.renderer = this.renderer.createDelegate(this);
};

Ext.grid.SectionRoleCheckColumn.prototype = {
    init: function (grid) {
        this.grid = grid;
    },

    renderer: function (v, p, record) {
        var checked = "";
        var disabled = "";
        var isInAllACAUsersColumn = this.dataIndex == sectionRoleColumnHeader[2];
        var isAllACAUsersChecked = record.data[sectionRoleColumnHeader[2]];
        var isRegisteredUsersChecked = record.data[sectionRoleColumnHeader[3]];
        var isInRegisteredUsersGroup = (
            this.dataIndex == sectionRoleColumnHeader[4]
            || this.dataIndex == sectionRoleColumnHeader[5]
            || this.dataIndex == sectionRoleColumnHeader[6]
            || this.dataIndex == sectionRoleColumnHeader[SectionRoleGridColumn.LP]
        );

        if ((isAllACAUsersChecked && !isInAllACAUsersColumn)
            || (isRegisteredUsersChecked && isInRegisteredUsersGroup)
        ) {
            disabled = "disabled";
            checked = "checked";
        }
        else if (v) {
            checked = "checked";
        }

        var title = record.data[sectionRoleColumnHeader[1]] + ', ' + this.dataIndex;  // Detail section name + user role

        return String.format('<input type="checkbox" id="CapDetailSectionRole_{0}_{1}" title="{2}" {3} {4} onclick="recordDetailRoleGrid_CheckboxClick(\'{0}\', this)">',
        	this.dataIndex, record.id, title, checked, disabled);
    }
};

function recordDetailRoleGrid_CheckboxClick(dataIndex, checkbox) {
    var grid = Ext.getCmp('recordDetailRoleGrid');
    var index = grid.getView().findRowIndex(checkbox);
    var record = grid.store.getAt(index);
    record.data[dataIndex] = checkbox.checked;
    var isClickingAllACAUsers = dataIndex == sectionRoleColumnHeader[2];
    var isClickingRegisteredUsers = dataIndex == sectionRoleColumnHeader[3];

    if (isClickingAllACAUsers || isClickingRegisteredUsers) {
        // if check "All ACA Users" or "Regestered Users", need check and disabel other check boxes
        var startDataIndex = dataIndex == sectionRoleColumnHeader[2] ? 3 : 4;

        for (var i = startDataIndex; i < 11; i++) {
            // exception index: for agent and agent clerk columns
            if (i == SectionRoleGridColumn.LPConfig
                || isClickingRegisteredUsers & (i == SectionRoleGridColumn.Agent || i == SectionRoleGridColumn.AgentClerk)) {
                continue;
            }
            var chkUserRole = $get(checkbox.id.replace(dataIndex, sectionRoleColumnHeader[i]));

            if (chkUserRole) {
                chkUserRole.checked = checkbox.checked;
                chkUserRole.disabled = checkbox.checked;
                record.data[sectionRoleColumnHeader[i]] = checkbox.checked;
            }
        }

        $get("RDRoleLicenseType" + record.id).disabled = true;
    } else if (dataIndex == sectionRoleColumnHeader[SectionRoleGridColumn.LP]) //LP
    {
        $get("RDRoleLicenseType" + record.id).disabled = !checkbox.checked;
    }

    DisabledSectionRoleButtons(false);
    document.SectionRoleChanged = true;
    parent.parent.ModifyMark('recordDetailSectionRole');
}

function addRDLicenseTypeButton(cellValue, cell, currentRowRecord) {
    var sectionName = currentRowRecord.data[sectionRoleColumnHeader[0]];
    var buttonID = "RDRoleLicenseType" + currentRowRecord.id;
    var disabled = "";
    if (currentRowRecord.data[sectionRoleColumnHeader[SectionRoleGridColumn.LP]] == "0" || currentRowRecord.data[sectionRoleColumnHeader[2]] == "1" || currentRowRecord.data[sectionRoleColumnHeader[3]] == "1") {
        disabled = 'disabled = "disabled"';
    }

    var htmlString = '<input ' + disabled + ' id="' + buttonID + '" title="Configure" value="Configure" type="button" width="100" onclick="openSectionRoleLicenseList(\'' + Ext.Constant.CAPDETAIL_SECTIONROLES + '\',\'' + sectionName + '\',this)" />';

    return htmlString;
}

var rightContexts = [];
rightContexts[Ext.Constant.CAPDETAIL_SECTIONROLES] = {
    dialogProperty: {
        itemContainerID: 'recordDetailContainer',
        itemIDPrefix: 'chkrecordDetailLPItem',
        positionObjID: null,
        sectionTitle: 'Licensed Professional Types',
        items: null,
        saveButtonID: 'btnSaveRecordDetail',
        save: SaveConfigSettings
    },
    xpolicyKey: Ext.Constant.CAPDETAIL_SECTIONROLES
};

rightContexts[Ext.Constant.INSPECTION_CONTACT_RIGHT] = {
    dialogProperty: {
        itemContainerID: 'inspectionPopupContainer',
        itemIDPrefix: 'chkInspectionLPItem',
        positionObjID: null,
        sectionTitle: 'Licensed Professional Types',
        items: null,
        saveButtonID: 'btnSaveLicenseList',
        save: SaveConfigSettings
    },
    xpolicyKey: Ext.Constant.INSPECTION_CONTACT_RIGHT
};

rightContexts[Ext.Constant.INSPECTION_PERMISSION_USER_ROLES] = {
    dialogProperty: {
        itemContainerID: 'inspectionSchedulePopupContainer',
        itemIDPrefix: 'chkInspectionScheduleLPItem',
        positionObjID: null,
        sectionTitle: 'Licensed Professional Types',
        items: null,
        saveButtonID: 'btnSaveLicenseProfessionalList',
        save: SaveConfigSettings
    },
    xpolicyKey: Ext.Constant.INSPECTION_PERMISSION_USER_ROLES
};

var LPListDialog = null;

function openSectionRoleLicenseList(xpolicyKey, sectionName, obj) {
    var moduleName = document.moduleName;

    Accela.ACA.Web.WebService.AdminConfigureService.GetLicenseTypeListByXpolicyKey(xpolicyKey, moduleName, sectionName, function sectionRoleLicenseTypeListCallback(callBackLicenseTypeList) {
        showLPListPopup(xpolicyKey, sectionName, obj, callBackLicenseTypeList)
    });
}

function showLPListPopup(xpolicyKey, sectionName, obj, callBackLicenseTypeList) {
    rightContexts[xpolicyKey].dialogProperty.positionObjID = obj.id;
    rightContexts[xpolicyKey].sectionName = sectionName;

    var response = eval('(' + callBackLicenseTypeList + ')');

    var options;
    var selectedAllChecked = true;

    if (response != null && response.LisenTypeList != null) {
        options = response.LisenTypeList;
    }

    if (options != null && options.length > 0) {
        rightContexts[xpolicyKey].dialogProperty.items = options;

        if (!LPListDialog) {
            LPListDialog = new CheckBoxListDialog(rightContexts[xpolicyKey]);
        } else {
            //in order to display the dialog(including position and items) correctly, the following properties  must be reset
            LPListDialog.context.dialogProperty.positionObjID = obj.id;
            LPListDialog.context.sectionName = sectionName;
            LPListDialog.context.xpolicyKey = xpolicyKey;
            LPListDialog.context.dialogProperty.items = options;
        }

        LPListDialog.Show();
    }
}

function SaveConfigSettings(context) {
    var arrVisible = new Array();

    if (context.checkBoxList == null) {
        return;
    }

    $(context.checkBoxList).each(function() {
        if (this.checked) {
            arrVisible.push(this.value);
        }
    });

    var moduleName = document.moduleName;
    var sectionName = context.sectionName;
    var xpolicyKey = context.xpolicyKey;

    Accela.ACA.Web.WebService.AdminConfigureService.SaveLicenseTypeListByXpolicyKey(xpolicyKey, moduleName, sectionName, arrVisible, function saveRoleLicenseTypeListCallback() {
        LPListDialog.Close();
    });
}

function btnSectionRoleOK_Clicked() {
    parent.showMessageBox();
    SaveRecordDetailSectionRoles(true);
}

// save datas and reload the grid
function SaveRecordDetailSectionRoles(showMessage) {
    var moduleName = document.moduleName;
    if (SectionRoleList.toString() != "" && document.SectionRoleChanged) {
        GetRecordDetailRoleList();
        Accela.ACA.Web.WebService.AdminConfigureService.SaveRecordDetailSectionRoles(SectionRoleList, moduleName, SaveSectionRoleCallBack, null, showMessage);
    }
}

//get latest data and display for cap type filter grid.
function RefreshSectionRoleDataStore() {
    var store = ChangeRecordDetailRolesStore(SectionRoleList);
    var grid = Ext.getCmp('recordDetailRoleGrid');
    var columnModel = grid.getColumnModel();
    grid.reconfigure(store, columnModel);
    grid.getView().refresh();
}

// set the OK and Cancel Button disabled or not
function DisabledSectionRoleButtons(isDisabled) {
    if (isDisabled) {
        if (typeof(parent.parent.RemoveMark) != "undefined") {
            parent.parent.RemoveMark(false, 'recordDetailSectionRole');
        }

        sectionRoleBtnOK.disable();
        sectionRoleBtnCancel.disable();
    } else {
        sectionRoleBtnOK.enable();
        sectionRoleBtnCancel.enable();
    }
}

function SaveSectionRoleCallBack(result, showMessage) {
    if (result) {
        DisabledSectionRoleButtons(true);
        document.SectionRoleChanged = false;
        Ext.MessageBox.hide();
        if (showMessage) {
            alert('Save successfully for record detail role setting.');
        }
    } else {
        Ext.MessageBox.hide();
        alert('Save unsuccessfully for record detail role setting.')
    }
}

function EnableOrDisableLPButton(id, obj) {
    if (id == 'cblInputContactUserType') {
        if (obj.checked) {
            btnInputContactUserTypeConfig.enable();
        } else {
            btnInputContactUserTypeConfig.disable();
        }
    } else if (id == 'cblViewContactUserType') {
        if (obj.checked) {
            btnViewContactUserTypeConfig.enable();
        } else {
            btnViewContactUserTypeConfig.disable();
        }
    } else if (id == 'cblUserType') {
        if (obj.checked) {
            btnScheduleInspectionUserTypeConfig.enable();
        } else {
            btnScheduleInspectionUserTypeConfig.disable();
        }
    }
}