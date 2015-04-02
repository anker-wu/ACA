/**
* <pre>
* 
*  Accela Citizen Access Admin
*  File: LicenseVerificationGrid.js
* 
*  Accela, Inc.
*  Copyright (C): 2010-2014
* 
*  Description:
* 
*  Notes:
*      $Id: LicenseVerificationGrid.js 266252 2014-02-20 10:26:50Z ACHIEVO\james.shi $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

var LicenseVerificationGrid;
var LicenseVerificationGridName = "LicenseVerificationGrid";
var licenseVerificationColumnHeader;

Ext.onReady(function() {   // render capType grid and buttons
    Ext.state.Manager.setProvider(new Ext.state.CookieProvider());
    document.moduleName = getparmByUrl('moduleName');
    InitLicenseVerificationUI();
     
});

//inital cap type filter grid.
function InitLicenseVerificationUI() {

    licenseVerificationColumnHeader = ["Key", Ext.LabelKey.Admin_Searchrole_Gridtitle_Recordtype, "Sections"];

    // the column model has information about grid columns
    // dataIndex maps the column to the specific data field in
    // the data store (created below)
    var cm = new Ext.grid.ColumnModel([
        { id: licenseVerificationColumnHeader[0], header: licenseVerificationColumnHeader[0], hidden: true, hideable: false, width: 0, dataIndex: licenseVerificationColumnHeader[0] },
        { header: licenseVerificationColumnHeader[1], align: 'left', width: 500, dataIndex: licenseVerificationColumnHeader[1] },
        { header: licenseVerificationColumnHeader[2], align: 'center', width: 150, renderer: addSectionConfigButton, dataIndex: licenseVerificationColumnHeader[2] }
    ]);

    var store = ChangeLicenseVerificationStore(LicenseVerificationData);

    var gridWidth = 720;

    // create the editor grid
    LicenseVerificationGrid = new Ext.grid.GridPanel({
        id: 'licenseVerificationGrid',
        store: store,
        cm: cm,
        style: 'border-width: 2px; border-color: #B6D2E5; border-style: solid',
        renderTo: 'LicenseVerificationGrid',
        autoExpandColumn: 'Key',
        border: false,
        height: 250,
        width: '100%',
        title: 'License Detail Display by Record Types'

    });

    // Sort License Verification list by cap type name
    LicenseVerificationData.sort(function (x, y) {
        return x[1].localeCompare(y[1]);
    });
    // trigger the data store load
    store.load();
};

//change to store by array.
function ChangeLicenseVerificationStore(arrayData) {
    if (arrayData != null && arrayData.length > 0 && arrayData[0][0] != '') {
        for (var i = 0; i < arrayData.length; i++) {
            var capType = arrayData[i];

            if (capType == null) {
                continue;
            }

            arrayData[i][0] = capType[0];
            arrayData[i][1] = capType[1];
        }
    }
    // ArrayReader
    var ds = new Ext.data.Store({
        proxy: new Ext.data.MemoryProxy(arrayData),
        reader: new Ext.data.ArrayReader({}, [
             { name: licenseVerificationColumnHeader[0], mapping: 0 },
             { name: licenseVerificationColumnHeader[1], mapping: 1 }
        ])
    });
    ds.load();

    return ds;
}

function addSectionConfigButton(cellValue, cell, currentRowRecord){
    var capTypeName = currentRowRecord.data[licenseVerificationColumnHeader[0]];
    var buttonID = "licenseVerificationButton" + currentRowRecord.id;
    
    var htmlString = '<input id="'+buttonID+'" title="Configure" value="Configure" type="button" width="100" onclick="openSectionList(\''+capTypeName+'\',this)" />';
    
    return htmlString;
}

var licenseVerificationContext = {
    dialogProperty: {
        itemContainerID: 'licenseVerificationPopupContainer',
        itemIDPrefix: 'chkLicenseVerification',
        positionObjID: null,
        sectionTitle: 'Section Display',
        items: null,
        saveButtonID: 'btnSaveLicenseVerfication',
        save: saveConfigedSections
    }
};

var licenseVerificationDialog = null;

function openSectionList(capTypeName, obj) {
    var moduleName = document.moduleName;
    Accela.ACA.Web.WebService.AdminConfigureService.GetLicenseVerificationSectionList(moduleName, capTypeName, function SectionListCallback(callBackSectionList) {
        var response = eval('(' + callBackSectionList + ')');
        var options;

        if (response != null && response.SectionList != null) {
            options = response.SectionList;
        }

        if (options != null && options.length > 0) {
            licenseVerificationContext.recordType = capTypeName;
            licenseVerificationContext.dialogProperty.items = options;
            licenseVerificationContext.dialogProperty.positionObjID = obj.id;

            if (!licenseVerificationDialog) {
                licenseVerificationDialog = new CheckBoxListDialog(licenseVerificationContext);
            }

            licenseVerificationDialog.Show();
        }
    })

}

// this function is save the changed items
// params:
//      saveobj:    the webcontrol which will be saved
function saveConfigedSections(context) {
    var arrVisible = new Array();

    if (context.checkBoxList == null) {
        return;
    }

    $(context.checkBoxList).each(function () {
            arrVisible.push(this.value + Ext.Const.SplitChar + this.checked);
    })

    Accela.ACA.Web.WebService.AdminConfigureService.SaveSectionListByCapType(moduleName, context.recordType, arrVisible, function saveLicenseTypeListCallBack(callBackSaveLicenseList) {

        licenseVerificationDialog.Close();

            if (!callBackSaveLicenseList) {
                alert('Save unsuccessfully for License Verification Sections.');
            }
        })
    };

function RemoveColon4Label(labelText) {
    var label = labelText.replace(/(\s*$)/g, "");  // trim right space.

    if (label != '' && label.substr(label.length - 1, 1) == ':') {
        label = label.substr(0, label.length - 1);
    }

    return label;
}