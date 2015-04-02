/**
 * <pre>
 * 
 *  Accela Citizen Access Admin
 *  File: ContactTypePrivilegegrid.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: ContactTypePrivilegegrid.js 2008-10-28 12:49:28Z ACHIEVO\sniper.xia $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */ 
var tempOKbtn;
var tempCancelbtn;
var tempCHKIDPrefix
var globalContactTypeGrid;
var ContactTypeColumnNames;

var splitChar="-";

Ext.onReady(function () {
    
    ContactTypeColumnNames = ["Contact ID", "Contact Type", "ALL ACA Users", "Record Creator", "Licensed Professional", "Contact", "Owner"];
    Ext.state.Manager.setProvider(new Ext.state.CookieProvider());

    //   var chkIDPrefix =[ColumnValues[2],ColumnValues[3],ColumnValues[4],ColumnValues[5],ColumnValues[6],ColumnValues[7]];
    var chkIDPrefix = [ContactTypeColumnNames[2], ContactTypeColumnNames[3], ContactTypeColumnNames[4], ContactTypeColumnNames[5], ContactTypeColumnNames[6]];
    tempCHKIDPrefix = chkIDPrefix;


    // render header column with label
    var latestLabel = null;
    function renderLabel(cellValue, cell, currentRowRecord) {
        latestLabel = cellValue;
        var returnValue = String.format("<label>{0}</label>", cellValue);
        return returnValue;
    }
    function renderRole(cellValue, cell, currentRowRecord) {
        var returnValue;
        var controlID = currentRowRecord.fields.items[cell.id].name + splitChar + currentRowRecord.id;
        var title = latestLabel + ', ' + currentRowRecord.fields.items[cell.id].name;
        if (cellValue) {
            returnValue = String.format("<INPUT TYPE='checkbox' checked id='{0}' title='{1}' onclick=\"DisabledOKAndCancel(false);RangeChecked('{0}')\">", controlID, title);
        }
        else {
            returnValue = String.format("<INPUT TYPE='checkbox' id='{0}' title='{1}' onclick=\"DisabledOKAndCancel(false);RangeChecked('{0}')\">", controlID, title);
        }
        return returnValue;
    }

    function LimitSelectedOnLoad(contactTypeRoleGrid) {
        var obj = contactTypeRoleGrid.getStore().getAt(0);
        if (typeof (obj) == 'undefined') return;
        var startIndex = parseInt(obj.id);
        var chkIDs = new Array();
        for (var i = 0; i < RoleDataList.length; i++) {
            chkIDs[i] = String(startIndex + i);
            for (var j = 0; j < chkIDPrefix.length; j++) {
                var reallID = chkIDPrefix[j] + splitChar + chkIDs[i];
                var chkCtl = $get(reallID);
                if (chkCtl.checked) {
                    RangeChecked(chkCtl.id);
                    break;
                }
            }
        }
    }

    function CancelChange() {
        DisabledOKAndCancel(true);
        grid.getStore().loadData(RoleDataList);
        grid.getSelectionModel().selectFirstRow();
        grid.addListener("render", LimitSelectedOnLoad(grid));
    }

    function SaveDataAndResetGrid() {
        parent.showMessageBox();
        ReSetRoleDataList(grid.getStore());
        Accela.ACA.Web.WebService.AdminConfigureService.SaveRolesForContactType(RoleDataList, SaveContactRoleCallBack, null, true);
        grid.getStore().loadData(RoleDataList);
        grid.getSelectionModel().selectFirstRow();
        grid.addListener("render", LimitSelectedOnLoad(grid));
    }

    // create the data store
    var store = new Ext.data.SimpleStore({
        fields: [
           { name: ContactTypeColumnNames[0] },
           { name: ContactTypeColumnNames[1], type: 'string' },
           { name: ContactTypeColumnNames[2], type: 'bool' },
           { name: ContactTypeColumnNames[3], type: 'bool' },
           { name: ContactTypeColumnNames[4], type: 'bool' },
           { name: ContactTypeColumnNames[5], type: 'bool' },
           { name: ContactTypeColumnNames[6], type: 'bool' }
        ]
    });
    store.loadData(RoleDataList);

    // create the Grid
    var grid = new Ext.grid.GridPanel({
        store: store,
        columns: [
            { id: ContactTypeDisplayColumnNames[0], header: ContactTypeDisplayColumnNames[0], hidden: true, width: 0, sortable: false, dataIndex: ContactTypeColumnNames[0] },
            { header: ContactTypeDisplayColumnNames[1], align: 'left', width: 220, sortable: false, renderer: renderLabel, dataIndex: ContactTypeColumnNames[1] },
            { header: ContactTypeDisplayColumnNames[2], align: 'center', width: 100, sortable: false, renderer: renderRole, dataIndex: ContactTypeColumnNames[2] },
            { header: ContactTypeDisplayColumnNames[3], align: 'center', width: 100, sortable: false, renderer: renderRole, dataIndex: ContactTypeColumnNames[3] },
            { header: ContactTypeDisplayColumnNames[4], align: 'center', width: 120, sortable: false, renderer: renderRole, dataIndex: ContactTypeColumnNames[4] },
            { header: ContactTypeDisplayColumnNames[5], align: 'center', width: 80, sortable: false, renderer: renderRole, dataIndex: ContactTypeColumnNames[5] },
            { header: ContactTypeDisplayColumnNames[6], align: 'center', width: 80, sortable: false, renderer: renderRole, dataIndex: ContactTypeColumnNames[6] }
        ],
        autoExpandColumn: ContactTypeDisplayColumnNames[0],
        stripeRows: true,
        enableColumnMove: false,
        enableHdMenu: false,
        height: 350,
        width: '101%',
        shadow: true,
        title: 'Role Assignment for Contact Types '
    });

    globalContactTypeGrid = grid;
    grid.render('ContactGrid');
    grid.getSelectionModel().selectFirstRow();
    grid.addListener("beforerender", LimitSelectedOnLoad(grid));

    var Okbtn = new Ext.Button({ id: "btnGridOK", text: " OK ", type: "button", minWidth: "80", handler: SaveDataAndResetGrid, disabled: true });
    Okbtn.render('tdOK');
    tempOKbtn = Okbtn;

    var Cancelbtn = new Ext.Button({ id: "btnGridCancel", text: " Cancel ", type: "button", minWidth: "80", handler: CancelChange, disabled: true });
    Cancelbtn.render('tdCancel');
    tempCancelbtn = Cancelbtn;
});

function RangeChecked(chk) {
    var fieldName = chk.split(splitChar)[0];
    var rowIndex = chk.split(splitChar)[1];
    var chkAllACA = document.getElementById(ContactTypeColumnNames[2] + splitChar + rowIndex);
    var chkCAP = document.getElementById(ContactTypeColumnNames[3] + splitChar + rowIndex);
    var chkLicensed = document.getElementById(ContactTypeColumnNames[4] + splitChar + rowIndex);
    var chkContact = document.getElementById(ContactTypeColumnNames[5] + splitChar + rowIndex);
    var chkOwner = document.getElementById(ContactTypeColumnNames[6] + splitChar + rowIndex);
    var Ctls = new Array(chkAllACA, chkCAP, chkLicensed, chkContact, chkOwner);

    switch (fieldName) {
    case ContactTypeColumnNames[2]:
        var eCtls = new Array(chkAllACA.id);
        UnCheckAndDisabled(Ctls, chkAllACA.checked, eCtls);
        break;
    case ContactTypeColumnNames[3]:
        var eCtls = new Array(chkAllACA.id, chkCAP.id, chkLicensed.id, chkContact.id, chkOwner.id);
        UnCheckAndDisabled(Ctls, chkCAP.checked, eCtls);
        break;
    case ContactTypeColumnNames[4]:
        var eCtls = new Array(chkAllACA.id, chkCAP.id, chkLicensed.id, chkContact.id, chkOwner.id);
        UnCheckAndDisabled(Ctls, chkLicensed.checked, eCtls);
        break;
    case ContactTypeColumnNames[5]:
        var eCtls = new Array(chkAllACA.id, chkCAP.id, chkLicensed.id, chkContact.id, chkOwner.id);
        UnCheckAndDisabled(Ctls, chkContact.checked, eCtls);
        break;
    case ContactTypeColumnNames[6]:
        var eCtls = new Array(chkAllACA.id, chkCAP.id, chkLicensed.id, chkContact.id, chkOwner.id);
        UnCheckAndDisabled(Ctls, chkOwner.checked, eCtls);
        break;
    }
}

///Uncheck or dispalbed controls base on reqiurement.
function UnCheckAndDisabled(ctlList, isChecked, ectlList) {
    for (var i = 0; i < ctlList.length; i++) {
        if (ectlList.toString().indexOf(ctlList[i].id) > -1)
            continue;
        if (isChecked) {
            ctlList[i].checked = true;
            ctlList[i].disabled = true;
        } else {
            if (!HaveCtlChecked(ectlList)) {
                ctlList[i].checked = false;
                ctlList[i].disabled = false;
            }
        }
    }
}


//check appoint controls have whose checked property is checked.
//If have one control's checked is checked then return true
//else return false;
function HaveCtlChecked(ectlList) {
    var isChecked = false;
    for (var j = 0; j < ectlList.length; j++) {
        var ctl = document.getElementById(ectlList[j]);
        isChecked = ctl.checked;
        if (isChecked) {
            break;
        }
    }
    return isChecked;
}


function DisabledOKAndCancel(isDisabled) {
    if (isDisabled) {
        if (typeof(parent.parent.RemoveMark) != "undefined") {
            parent.parent.RemoveMark(false, 'contactType');
        }

        tempOKbtn.disable();
        tempCancelbtn.disable();
    } else {
        if (typeof(parent.parent.ModifyMark) != "undefined") {
            parent.parent.ModifyMark('contactType');
        }

        tempOKbtn.enable();
        tempCancelbtn.enable();
    }
}

//update value of role data list
function UpdateRoleDataList(startIndex, chkControl) {
    var Ids = new Array();
    var isBreakLoop = false;
    for (var i = 0; i < RoleDataList.length; i++) {
        Ids[i] = String(startIndex + i);
        for (var j = 0; j < tempCHKIDPrefix.length; j++) {
            var realID = tempCHKIDPrefix[j] + splitChar + Ids[i];
            if (chkControl.id == realID) {
                var chkCtl = $get(realID);
                RoleDataList[i][j + 2] = chkControl.checked;
                isBreakLoop = true;
                break;
            }
        }
        if (isBreakLoop) {
            break;
        }
    }
}

//Reset value to role data list
function ReSetRoleDataList(roleDatas) {
    if (typeof(roleDatas.getAt(0)) == 'undefined') return;
    var startIndex = parseInt(roleDatas.getAt(0).id);
    var Ids = new Array();
    for (var i = 0; i < RoleDataList.length; i++) {
        Ids[i] = String(startIndex + i);
        for (var j = 0; j < tempCHKIDPrefix.length; j++) {
            var realID = tempCHKIDPrefix[j] + splitChar + Ids[i];
            var chkCtl = $get(realID);
            if (chkCtl.disabled == false) {
                RoleDataList[i][j + 2] = chkCtl.checked;
            } else {
                RoleDataList[i][j + 2] = false;
            }
        }
    }
}

function SaveRoleData() {
    if (RoleDataList.length > 0) {
        ReSetRoleDataList(globalContactTypeGrid.getStore());
        Accela.ACA.Web.WebService.AdminConfigureService.SaveRolesForContactType(RoleDataList, SaveContactRoleCallBack, null, false);
    }
}

function SaveContactRoleCallBack(result, showMessage) {
    if (result) {
        DisabledOKAndCancel(true);
        Ext.MessageBox.hide();
        if (showMessage) {
            alert('Changes role contact type to save successfully.');
        }
    } else {
        Ext.MessageBox.hide();
        alert('Changes role contact type to save unsuccessfully.');
    }
}