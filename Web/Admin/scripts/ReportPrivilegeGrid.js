/**
* <pre>
* 
*  Accela Citizen Access Admin
*  File: ReportPrivilegeGrid.js
* 
*  Accela, Inc.
*  Copyright (C): 2009-2014
* 
*  Description:
* 
*  Notes:
*      $Id: ContactTypePrivilegegrid.js 2009-03-20 12:49:28Z ACHIEVO\vera.zhao $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

var btnOK;
var btnCancel;
var reportGrid;
var gridName = "Report";
var columnHeader;
var isModuleSetting;

Ext.onReady(function() {   // render report grid and buttons
    Ext.state.Manager.setProvider(new Ext.state.CookieProvider());

    //ReportGridColumns: 0 is id, 1 is report name, 2-5 are user privilege.
    columnHeader = ["Id", Ext.LabelKey.Admin_Report_Role_Available_Report,
    Ext.LabelKey.Admin_Report_Role_All_ACA_Users, Ext.LabelKey.Admin_Report_Role_Anon_Users,
    Ext.LabelKey.Admin_Report_Role_Registered_User, Ext.LabelKey.Admin_Report_Role_Licensed_Professinal,
    Ext.LabelKey.Admin_Report_Role_Agent_User, Ext.LabelKey.Admin_Report_Role_AgentClerk_User];

    isModuleSetting = isModule == "Y" ? true : false;
    
    // Sort report role list by report name (column 1)
    ReportRoleList.sort(function(x, y) {
        return x[1].localeCompare(y[1]);
    });
    
    // create the data store
    var dataStore = new Ext.data.SimpleStore({
        fields: columnHeader
    });
    dataStore.loadData(ReportRoleList);

    // create the grid and render it
    var columns =
    [
        { id: columnHeader[0], header: columnHeader[0], hidden: true, width: 0, sortable: false, dataIndex: columnHeader[0] },
        { header: columnHeader[1], align: 'left', width: 220, sortable: false, renderer: renderLabel, dataIndex: columnHeader[1] },
        { header: columnHeader[2], align: 'center', width: 100, sortable: false, renderer: renderCheckBox, dataIndex: columnHeader[2] },
        { header: columnHeader[3], align: 'center', width: 100, sortable: false, renderer: renderCheckBox, dataIndex: columnHeader[3] },
        { header: columnHeader[4], align: 'center', width: 110, sortable: false, renderer: renderCheckBox, dataIndex: columnHeader[4] },
        { header: columnHeader[5], align: 'center', width: 130, sortable: false, renderer: renderCheckBox, dataIndex: columnHeader[5] },
        { header: columnHeader[6], align: 'center', width: 130, sortable: false, renderer: renderCheckBox, dataIndex: columnHeader[6] },
        { header: columnHeader[7], align: 'center', width: 130, sortable: false, renderer: renderCheckBox, dataIndex: columnHeader[7] }
    ];

    reportGrid = new Ext.grid.GridPanel({
        store: dataStore,
        columns: columns,
        autoExpandColumn: columnHeader[0],
        stripeRows: true,
        enableColumnMove: true,
        enableHdMenu: false,
        height: 250,
        width: '100%',
        shadow: true,
        title: 'Role Assignment'
    });

    reportGrid.render('ReportGrid');
    reportGrid.getSelectionModel().selectFirstRow();

    if (ReportRoleList.toString() != "") {  // if report list is null, set checkboxs disable or not.
        reportGrid.addListener("beforerender", LoadGrid(reportGrid));
    }

    // render OK and Cancel buttons
    btnOK = new Ext.Button({ id: "btnOK", text: " OK ", type: "button", minWidth: "80", handler: SaveReportRoles, disabled: true });
    btnOK.render('tdSaveReport');

    btnCancel = new Ext.Button({ id: "btnCancel", text: " Cancel ", type: "button", minWidth: "80", handler: btnCancel_Clicked, disabled: true });
    btnCancel.render('tdCancelReport');
    
    // render header column with label
    var latestLabel = null;
    function renderLabel(cellValue, cell, currentRowRecord) {
        latestLabel = cellValue;
        var returnValue = String.format("<label>{0}</label>", cellValue);
        return returnValue;
    }

    function renderCheckBox(cellValue, cell, currentRowRecord) {
        if (cellValue.toString() == "") { // no reports
            return;
        }
        var controlID = gridName + Ext.Const.SplitChar + currentRowRecord.fields.items[cell.id].name + Ext.Const.SplitChar + currentRowRecord.id;
        var title = latestLabel + ', ' + currentRowRecord.fields.items[cell.id].name;
        var returnValue;
        if (cellValue == 1) {
            returnValue = String.format("<input type='checkbox' id='{0}' title='{1}' checked onclick=\"DisabledReportButtons(false);checkBox_Clicked('{0}')\">",
                      controlID, title);
        }
        else {
            returnValue = String.format("<input type='checkbox' id='{0}' title='{1}' onclick=\"DisabledReportButtons(false);checkBox_Clicked('{0}')\">",
                      controlID, title);
        }
        return returnValue;
    }
});                      // end Ext.onReady

// save datas and reload the grid
function SaveReportRoles() 
{
    if (ReportRoleList.toString() != "") 
    {
        UpdateReportRoleList(reportGrid);
        Accela.ACA.Web.WebService.AdminConfigureService.SaveRolesForReports(ReportRoleList, isModuleSetting, SaveReportRoleCallBack);
    }
}

function btnCancel_Clicked() 
{
    DisabledReportButtons(true);
    reportGrid.getStore().loadData(ReportRoleList);
    reportGrid.addListener("render", LoadGrid(reportGrid));
}

//update report role datas when user click OK or Save button
function UpdateReportRoleList(reportGrid) {
    for (var i = 0; i < ReportRoleList.length; i++)  // grid rows
    {
        for (var j = 2; j < columnHeader.length; j++)   // grid columns
        {
            var chkCtl = getRoleCheckBox(columnHeader[j], i);
            if (chkCtl.disabled)    // column 1 is id, column 2 is name. disabled means checked...
            {
                ReportRoleList[i][j] = 0;
            }
            else    
            {
                ReportRoleList[i][j] = chkCtl.checked ? 1 : 0;
            }
        }
    }
}  	 


function LoadGrid(reportGrid) 
{
    for (var i = 0; i < ReportRoleList.length; i++) 
    {
        for (var j = 2; j < columnHeader.length; j++) 
        {
            var chkCtl = getRoleCheckBox(columnHeader[j], i);
            
            if (chkCtl.checked) 
            {
                checkBox_Clicked(chkCtl.id,true);
                break;
            }
        }
    }
}

function getRoleCheckBox(userRole,rowIndex) 
{
    var startIndex = parseInt(reportGrid.getStore().getAt(0).id);
    var checkBox = $get(gridName + Ext.Const.SplitChar + userRole + Ext.Const.SplitChar + (startIndex + rowIndex).toString());
    return checkBox;
}

function checkBox_Clicked(chkID,isFirstLoad) 
{
    var userRole = chkID.split(Ext.Const.SplitChar)[1];
    var rowIndex = chkID.split(Ext.Const.SplitChar)[2];
    var id = gridName + Ext.Const.SplitChar + '{0}' + Ext.Const.SplitChar + rowIndex;
    var chkAllACA = $get(String.format(id, columnHeader[2]));
    var chkAnonUser = $get(String.format(id, columnHeader[3]));
    var chkRegistered = $get(String.format(id, columnHeader[4]));
    var chkLicensed = $get(String.format(id, columnHeader[5]));
    var chkAgent = $get(String.format(id, columnHeader[6]));
    var chkAgentClerk = $get(String.format(id, columnHeader[7]));

    var checkBoxs = new Array(chkAnonUser, chkRegistered, chkLicensed, chkAgent, chkAgentClerk);
    
    // if all aca user checked, set all user roles checked and disabled
    if (userRole == Ext.LabelKey.Admin_Report_Role_All_ACA_Users) 
    {
        for (var i = 0; i < checkBoxs.length; i++) 
        {
            checkBoxs[i].checked = chkAllACA.checked;
            checkBoxs[i].disabled = chkAllACA.checked;
        }
    }
    // if register user checked, set licensed professinal user checked and disabled.
    else if (userRole == Ext.LabelKey.Admin_Report_Role_Registered_User) 
    {
        chkLicensed.checked = chkRegistered.checked;
        chkLicensed.disabled = chkRegistered.checked;
    }

    if (!isFirstLoad) 
    {
        parent.parent.ModifyMark('report');
    }
}

// set the OK and Cancel Button disabled or not
function DisabledReportButtons(isDisabled) 
{
    if (isDisabled)    
    {
        if (typeof (parent.parent.RemoveMark) != "undefined") 
        {
            parent.parent.RemoveMark(false, 'report');
        }

        btnOK.disable();
        btnCancel.disable();
    }
    else 
    {
        btnOK.enable();
        btnCancel.enable();
    }
}

function SaveReportRoleCallBack(result) 
{
    if (!result) 
    {
        alert('Save unsuccessfully for reports role setting.');
    }
    DisabledReportButtons(true);
}
