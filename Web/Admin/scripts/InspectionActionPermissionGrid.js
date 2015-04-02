/**
* <pre>
* 
*  Accela Citizen Access Admin
*  File: InspectionActionPermissionGrid.js
* 
*  Accela, Inc.
*  Copyright (C): 2010-2014
* 
*  Description:
* 
*  Notes:
*      $Id: InspectionActionPermissionGrid.js 2009-03-20 12:49:28Z ACHIEVO\vera.zhao $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/
var btnInsActionPermissionOK;
var btnInsActionPermissionCancel;
var insActionPermissionsGrid;
var insActionPermissionDataSource = [[]];
var insActionPermissionDataStore;
var updateRecords;

Ext.onReady(function () {
    Ext.state.Manager.setProvider(new Ext.state.CookieProvider());
    //InspectionGridColumns: 0 is inspeciton sequence number, 1 is inspection type, 2~4 is inspection type status value.
    var columnHeader = ["Id",
                              Ext.LabelKey.admin_application_inspectionpermiassion_inspectiontype,
                              Ext.LabelKey.admin_application_inspectionpermission_requestschedule,
                              Ext.LabelKey.admin_application_inspectionpermiassion_reschedule,
                              Ext.LabelKey.admin_application_inspectionpermiassion_cancel];
    // create the grid and render it
    var columns =
    [
        { id: columnHeader[0], hidden: true, width: 0, sortable: false, dataIndex: columnHeader[0] },
        { header: columnHeader[1], align: 'left', width: 220, sortable: false, renderer: renderLabel, dataIndex: columnHeader[1] },
        { header: columnHeader[2], align: 'center', width: 150, sortable: false, renderer: renderCheckBox, dataIndex: columnHeader[2] },
        { header: columnHeader[3], align: 'center', width: 110, sortable: false, renderer: renderCheckBox, dataIndex: columnHeader[3] },
        { header: columnHeader[4], align: 'center', width: 130, sortable: false, renderer: renderCheckBox, dataIndex: columnHeader[4] }
    ];

    //create store
    insActionPermissionDataStore = new Ext.data.SimpleStore({
        fields: columnHeader
    });

    insActionPermissionsGrid = new Ext.grid.GridPanel({
        id: 'insActionPermissionsGrid',
        store: insActionPermissionDataStore,
        columns: columns,
        stripeRows: true,
        enableColumnMove: true,
        enableHdMenu: false,
        autoExpandColumn: 'Id',
        renderTo: 'divInsActionPermissionsList',
        height: 250,
        width: '100%',
        shadow: true,
        title: Ext.LabelKey.admin_application_inspectionpermiassion_title
    });

    // trigger the data store load
    insActionPermissionDataStore.loadData(insActionPermissionDataSource);

    // render header column with label to support I18N
    var latestLabel = null;
    function renderLabel(cellValue, cell, currentRowRecord) {
        latestLabel = cellValue;
        var returnValue = String.format("<label>{0}</label>", cellValue);
        return returnValue;
    }

    //render checkBox column.
    function renderCheckBox(cellValue, cell, currentRowRecord) {
        var splitChar = Ext.Const.SplitChar;
        var insTypeSeqNbr = currentRowRecord.data.Id;

        var insActionAgency = '';
        var insActionSeqNbr = '';
        var insActionCode = '';
        var insActionEnabled = ''; //1: checed; 0: unchecked; default value check;

        if (cellValue != null) {
            var params = cellValue.split(splitChar);
            if (params != null && params.length == 4) {
                insActionAgency = params[0];
                insActionSeqNbr = params[1];
                insActionCode = params[2];
                insActionEnabled = params[3];
            }
        }

        var controlID = String.format("{0}{1}{2}{3}{4}{5}{6}", insTypeSeqNbr, splitChar, insActionAgency, splitChar, insActionSeqNbr, splitChar, insActionCode);
        var title = latestLabel + ', ' + currentRowRecord.fields.items[cell.id].name;
        var returnValue;

        if (insActionEnabled == 0) {
            returnValue = String.format("<input type='checkbox' id='{0}' title='{1}' onclick=\"DisabledButtons(false);InsChkBox_Clicked('{0}')\">",
                      controlID, title);
        }
        else {
            returnValue = String.format("<input type='checkbox' id='{0}' title='{1}' checked onclick=\"DisabledButtons(false);InsChkBox_Clicked('{0}')\">",
                      controlID, title);
        }

        return returnValue;
    }

    // render OK and Cancel buttons
    btnInsActionPermissionOK = new Ext.Button({ id: "btnInsActionPermissionOK", text: " OK ", type: "button", minWidth: "80", handler: btnInsActionPermissionOK_Clicked, disabled: true });
    btnInsActionPermissionCancel = new Ext.Button({ id: "btnInsActionPermissionCancel", text: " Cancel ", type: "button", minWidth: "80", handler: CancelInsActionPermissions, disabled: false });
    btnInsActionPermissionOK.render('tdInspectionOk');
    btnInsActionPermissionCancel.render('tdInspectionCannel');
});

//get latest data and display for cap type filter grid.
function RefreshInsActionDataStore() {
    insActionPermissionDataStore.loadData(FormatStoreData(insActionPermissionDataSource));
    var grid = Ext.getCmp('insActionPermissionsGrid');
    var columnModel = grid.getColumnModel();
    grid.reconfigure(insActionPermissionDataStore, columnModel);
    grid.getView().refresh();

    //Initial inspection action permission setting status.
    InitialInsActionPermissionSetting()
}

//string to array; e.g. '1','2','3' change to [1][2][3];
function FormatStoreData(array) {
    if (array != null) {
        for (var i = 0; i < array.length; i++) {
            if (array[i] != null) {
                array[i] = array[i].split(',');
            }
        }
    }

    return array;
}

function btnInsActionPermissionOK_Clicked() {
    parent.showMessageBox('Saving');
    SaveInsActionPermissionSettings(true);
}

// save datas and reload the grid
function SaveInsActionPermissionSettings(needShowMessage) {
    if (IsSesstionTimeout()) {
        RedirectToLoginPage();
    }
    else {
        if (updateRecords != null && updateRecords.length > 0) {
            Accela.ACA.Web.WebService.AdminConfigureService.SaveActionPermissionSettings(FormatSaveDataArray(updateRecords), CallBackSaveActionPermissionSettings, null, needShowMessage);
        }
        else {
            DisabledButtons(true);
        }
    }
}

function FormatSaveDataArray(obj) {
    var array = Array();
    if (obj != null) {
        for (var i = 0; i < obj.length; i++) {
            if (updateRecords != null) {
                if (obj[i] == null) {
                    continue;
                }

                var row = Array();
                row.push(obj[i].insTypeSeqNbr);
                row.push(obj[i].insType);
                row.push(obj[i].insActionAgency);

                if (obj[i].insActionSeqNbr == 0) {
                    hasSeqNbr = false;
                }

                row.push(obj[i].insActionSeqNbr);
                row.push(obj[i].insActionCode);
                row.push(obj[i].insActionEnabled);

                row.push(obj[i].appStatusGroupCode);
                row.push(obj[i].appStatus);

                array.push(row);
            }
        }

        return array;
    }
    return obj;
}

var hasSeqNbr = true;
function CallBackSaveActionPermissionSettings(result, needShowMessage) {
    if (result) {
        //Initial inspection action permission setting status.
        InitialInsActionPermissionSetting();
        if (hasSeqNbr == false) {
            UpdateInsGroupDataInfo();
            hasSeqNbr = true;
        }

        if (Ext.MessageBox.isVisible()) {
            Ext.MessageBox.hide();
        }
        if (needShowMessage) {
            alert(Ext.LabelKey.admin_application_inspectionpermiassion_message_save_success);
        }
    }
    else {
        if (Ext.MessageBox.isVisible()) {
            Ext.MessageBox.hide();
        }
        alert(Ext.LabelKey.admin_application_inspectionpermiassion_message_save_unsuccess);
    }
}

//click cannel button.
function CancelInsActionPermissions() {
    var isNeedCollapse = true;

    if (!btnInsActionPermissionOK.disabled) {
        isNeedCollapse = confirmMsg(Ext.LabelKey.admin_application_inspectionpermiassion_message_cancel_alert);
    }

    if (isNeedCollapse) {
        //1.Initial inspection action permission setting status.
        InitialInsActionPermissionSetting();

        //2. collapse inspection action permissions setting pad.
        var divInspectionActionSetting = document.getElementById('divInspectionActionSetting');

        if (divInspectionActionSetting != null) {
            divInspectionActionSetting.className = 'ACA_Hide';
        }
    }
}

//Initial inspection action permission setting status.
function InitialInsActionPermissionSetting() {
    //1. clear update records.
    updateRecords = null;

    //2. setting button as disable stutus.
    DisabledButtons(true);
}

function InsChkBox_Clicked(chkID, isFirstLoad) {
    var info = null;
    if (chkID != null) {
        var params = chkID.split(Ext.Const.SplitChar);
        if (params != null && params.length == 4) {
            info = new updateInsActionItems();

            info.insTypeSeqNbr = params[0];
            info.insActionAgency = params[1]
            info.insActionSeqNbr = params[2];
            info.insActionCode = params[3];
            info.insActionEnabled = $get(chkID).checked ? "1" : "0";

            info.appStatusGroupCode = appStatusGroupCode;
            info.appStatus = appStatus;
        }

        UpdateItems(info);
    }

    if (!isFirstLoad && info != null) {
        parent.parent.ModifyMark('inspectionActionPermissionSettings');
    }
}

function UpdateItems(info) {
    if (info != null) {
        if (updateRecords != null && updateRecords.length > 0) {
            var isExist = false;
            for (var i = 0; i < updateRecords.length; i++) {
                recordItem = updateRecords[i];
                if (recordItem != null) {
                    //entity id: inspection type sequence number + inspection action code.
                    if (recordItem.insTypeSeqNbr == info.insTypeSeqNbr && recordItem.insActionCode == info.insActionCode) {
                        //update record in record list.
                        updateRecords[i] = info;
                        isExist = true;
                        break;
                    }
                }
            }

            //if the record not exist in record list. 
            if (!isExist) {
                updateRecords.push(info);
            }
        } else {
            updateRecords = Array();
            updateRecords.push(info);
        }
    }
}

// set the OK and Cancel Button disabled or not
function DisabledButtons(isDisabled) {
    if (isDisabled) {
        if (typeof (parent.parent.RemoveMark) != "undefined") {
            parent.parent.RemoveMark(false, 'inspectionActionPermissionSettings');
        }

        btnInsActionPermissionOK.disable();
    }
    else {
        btnInsActionPermissionOK.enable();
    }
}

//Update a check control and include information. Entity ID: insTypeSeqNbr + insActionCode
function updateInsActionItems() {
    var insType;
    var insTypeSeqNbr;

    var insActionCode;
    var insActionEnabled;
    var insActionAgency;
    var insActionSeqNbr;

    var appStatusGroupCode;
    var appStatus;
}

function IsSesstionTimeout() {
    var conn = Ext.lib.Ajax.getConnectionObject().conn;
    conn.open("POST", '../GetSessionState.aspx', false);
    conn.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    conn.send('../GetSessionState.aspx');
    return conn.responseText != 'Y';
}

function RedirectToLoginPage() {
    parent.location.href = '../Login.aspx?timeout=true';
}
