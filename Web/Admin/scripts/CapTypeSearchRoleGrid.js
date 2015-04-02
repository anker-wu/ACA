/**
* <pre>
* 
*  Accela Citizen Access Admin
*  File: CapTypeSearchRoleGrid.js
* 
*  Accela, Inc.
*  Copyright (C): 2009-2014
* 
*  Description:
* 
*  Notes:
*      $Id: CapTypeSearchRoleGrid.js 266252 2014-02-20 10:26:50Z ACHIEVO\james.shi $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

var capTypeBtnOK;
var capTypeBtnCancel;
var CapTypeSearchRoleGrid;
var capTypeGridName = "CapTypeSearchRoleGrid";
var capTypeColumnHeader;
var tempIsModuleLevel;

Ext.onReady(function() {   // render capType grid and buttons
    Ext.state.Manager.setProvider(new Ext.state.CookieProvider());
    document.moduleName = getparmByUrl('moduleName');
    
    InitRolesForCapTypes();

    // render OK and Cancel buttons
    capTypeBtnOK = new Ext.Button({ id: "capTypeBtnOK", text: " OK ", type: "button", minWidth: "80", handler: btnCapTypeOK_Clicked, disabled: true });
    capTypeBtnOK.render('tdSaveCapSearchRole');

    capTypeBtnCancel = new Ext.Button({ id: "capTypeBtnCancel", text: " Cancel ", type: "button", minWidth: "80", handler: btnCapTypeCancel_Clicked, disabled: true });
    capTypeBtnCancel.render('tdCancelSaveCapSearchRole'); 
    
    changeGridUI();
     
});   


//inital cap type filter grid.
function InitRolesForCapTypes(){
    
    tempIsModuleLevel = isModuleLevel;
    
    if(tempIsModuleLevel)
    {
        oldLevelType = "0";
        CapTypeRoleList = ModuleLevelCapTypeRoleList; 
    }
    else
    {
        oldLevelType = "1";
        CapTypeRoleList = CapTypeLevelRoleList; 
    }
    
    capTypeColumnHeader = ["Key", Ext.LabelKey.Admin_Searchrole_Gridtitle_Recordtype,
    Ext.LabelKey.Admin_Searchrole_Gridtitle_Acausers, Ext.LabelKey.Admin_Searchrole_Gridtitle_RegisteredUsers,
    Ext.LabelKey.Admin_Searchrole_Gridtitle_RecordCreator, Ext.LabelKey.Admin_Searchrole_Gridtitle_Contact,Ext.LabelKey.Admin_Searchrole_Gridtitle_Owner,Ext.LabelKey.Admin_Searchrole_Gridtitle_LicensedProfessional,"Specific Licensed Professional"];
    var chkAllACAUser = new Ext.grid.CheckColumn({header: capTypeColumnHeader[2], align: 'center', width: 100,dataIndex: capTypeColumnHeader[2]});
    var chkRegUser = new Ext.grid.CheckColumn({header: capTypeColumnHeader[3], align: 'center', width: 100, dataIndex: capTypeColumnHeader[3]});
    var chkCapCreater = new Ext.grid.CheckColumn({header: capTypeColumnHeader[4], align: 'center', width: 100, dataIndex: capTypeColumnHeader[4]});
    var chkContact = new Ext.grid.CheckColumn({header: capTypeColumnHeader[5], align: 'center', width: 100, dataIndex: capTypeColumnHeader[5]});
    var chkOwner= new Ext.grid.CheckColumn({header: capTypeColumnHeader[6], align: 'center', width: 100, dataIndex: capTypeColumnHeader[6]});
    var chkLP= new Ext.grid.CheckColumn({header: capTypeColumnHeader[7], align: 'center', width: 100, dataIndex: capTypeColumnHeader[7] });
    
    // the column model has information about grid columns
    // dataIndex maps the column to the specific data field in
    // the data store (created below)
    var cm = new Ext.grid.ColumnModel([
        { id: capTypeColumnHeader[0], header: capTypeColumnHeader[0], hidden: true,hideable:false, width: 0, dataIndex: capTypeColumnHeader[0] },
        { header: capTypeColumnHeader[1], align: 'left', width: 250, renderer: renderCapType, dataIndex: capTypeColumnHeader[1] },
        chkAllACAUser,
        chkRegUser,
        chkCapCreater,
        chkContact,
        chkOwner,
        chkLP,
        { header: capTypeColumnHeader[8],hidden: true,hideable:false, align: 'center', width: 150, renderer: addLicenseListButton, dataIndex: capTypeColumnHeader[8]}
    ]);
         
    // Sort cap type role list by report name (column 1)
    CapTypeRoleList.sort(function(x, y) {
        return x[1].localeCompare(y[1]);
    });
    
   var store = ChangeCapTypeRolesStore(CapTypeRoleList); 

    var gridWidth = 720;
    if (isFireFox())
    {
        gridWidth = 700;
    }

    // create the editor grid
    CapTypeSearchRoleGrid = new Ext.grid.GridPanel({
        id:'capTypeSearchRoleGrid',
        store: store,
        cm: cm,
        style: 'border-width: 2px; border-color: #B6D2E5; border-style: solid',
        renderTo: 'CapTypeSearchRoleGrid',
        plugins:[chkAllACAUser,chkRegUser,chkCapCreater,chkContact,chkOwner,chkLP],
        autoExpandColumn:'Key',
        border:false,
        height: 250,
        width: '100%',
        title: 'Role Assignment'
     });
     
     
    // trigger the data store load
    store.load();
    //store.setDefaultSort(capTypeColumnHeader[1],'ASC');
    chkAllACAUser.readonly = true;
};

function LoadCapTypeUserRoleList(){
    var userRoleList = new Array();
    var nodes = Ext.getCmp('pnlFilterCapSelected').getNodes();
    //var length = filterItem.CapTypeList.length;
        
    for(var i=0;i<nodes.length;i++){
        userRoleList[userRoleList.length] = nodes[i].attributes;
    }
    return userRoleList;
}

//change to store by array.
function ChangeCapTypeRolesStore(arrayData) {
    if (arrayData != null && arrayData.length > 0 && arrayData[0][0] != '') {
        for (var i = 0; i < arrayData.length; i++) {
            var capType = arrayData[i];

            if (capType == null) {
                continue;
            }

            arrayData[i][0] = capType[0];
            arrayData[i][1] = capType[1];
            arrayData[i][2] = capType[2];
            arrayData[i][3] = capType[3];
            arrayData[i][4] = capType[4];
            arrayData[i][5] = capType[5];
            arrayData[i][6] = capType[6];
            arrayData[i][7] = capType[7];
        }
    }
    // ArrayReader
    var ds = new Ext.data.Store({
        proxy: new Ext.data.MemoryProxy(arrayData),
        reader: new Ext.data.ArrayReader({ }, [
            { name: capTypeColumnHeader[0], mapping: 0 },
            { name: capTypeColumnHeader[1], mapping: 1 },
            { name: capTypeColumnHeader[2], mapping: 2 },
            { name: capTypeColumnHeader[4], mapping: 3 },
            { name: capTypeColumnHeader[7], mapping: 4 },
            { name: capTypeColumnHeader[5], mapping: 5 },
            { name: capTypeColumnHeader[6], mapping: 6 },
            { name: capTypeColumnHeader[3], mapping: 7 }
        ])
    });
    ds.load();

    return ds;
}


function GetCapTypeUserRoleList(){

    var store = Ext.getCmp('capTypeSearchRoleGrid').getStore();
    
    for (var i = 0; i < store.data.length; i++) 
    {
        var record = store.getAt(i);
        var capType = new Object();
        
        capType[0] = record.get(capTypeColumnHeader[0]);
        capType[1] = record.get(capTypeColumnHeader[1]);
        capType[2] = record.get(capTypeColumnHeader[2])?1:0;
        capType[3] = record.get(capTypeColumnHeader[3])?1:0;
        capType[4] = record.get(capTypeColumnHeader[4])?1:0;
        capType[5] = record.get(capTypeColumnHeader[5])?1:0;
        capType[6] = record.get(capTypeColumnHeader[6])?1:0;
        capType[7] = record.get(capTypeColumnHeader[7])?1:0;        
        
        CapTypeRoleList[i][0] = capType[0]; 
        CapTypeRoleList[i][1] = capType[1]; 
        CapTypeRoleList[i][2] = capType[2];//All ACA User 
        CapTypeRoleList[i][3] = capType[4]; //Cap Creator
        CapTypeRoleList[i][4] = capType[7]; //LP
        CapTypeRoleList[i][5] = capType[5]; //Contact
        CapTypeRoleList[i][6] = capType[6]; //Owner
        CapTypeRoleList[i][7] = capType[3]; //registered.
    }
     
}

var oldLevelType;

function changeGridUI()
{
    if(tempIsModuleLevel)
    {
        CapTypeSearchRoleGrid.setTitle(Ext.LabelKey.Admin_SearchRole_ModuleLevelTitle,null);
        CapTypeSearchRoleGrid.colModel.setHidden(8,true);
    }
    else
    {
        CapTypeSearchRoleGrid.setTitle(Ext.LabelKey.Admin_SearchRole_CapTypeLevelTitle,null);     
        CapTypeSearchRoleGrid.colModel.setHidden(8,false);   
    }
}

function renderCapType(cellValue, cell, currentRowRecord) {
    return String.format("<label>{0}</label>", cellValue);
}

Ext.grid.CheckColumn = function(config){
    Ext.apply(this, config);
    if(!this.id){
        this.id = Ext.id();
    }
    this.renderer = this.renderer.createDelegate(this);
};

Ext.grid.CheckColumn.prototype ={
    init : function(grid){
        this.grid = grid;
    },
    renderer : function(v, p, record){
        var checked = "";
        var disabled = "";
        if (this.dataIndex == capTypeColumnHeader[3] && record.data[capTypeColumnHeader[2]])
        {
            disabled = "disabled";
            checked = "checked";
        }
        else if(this.dataIndex!=capTypeColumnHeader[2] && this.dataIndex!=capTypeColumnHeader[3] &&
               (record.data[capTypeColumnHeader[2]] || record.data[capTypeColumnHeader[3]]))
        {
            disabled = "disabled";
            checked = "checked";
        }
        else
        {
            checked =  v ? "checked" : "";
        }

        var title = record.data[capTypeColumnHeader[1]] + ', ' + this.dataIndex;

        return String.format('<input type="checkbox" id="CapTypeSearchRole_{0}_{1}"  title="{2}" {3} {4} onclick="capTypeSearchRoleGrid_CheckboxClick(\'{0}\', this)">',
        	this.dataIndex, record.id, title, checked, disabled);
    }
}


function capTypeSearchRoleGrid_CheckboxClick(dataIndex, checkbox) {
	var grid = Ext.getCmp('capTypeSearchRoleGrid');
	var index = grid.getView().findRowIndex(checkbox);
	var record = grid.store.getAt(index);

	record.data[dataIndex] = checkbox.checked;

	if (dataIndex == capTypeColumnHeader[2] || dataIndex == capTypeColumnHeader[3]) {
	    // if check "All ACA Users" or "Regestered Users", need check and disabel other check boxes
	    var startDataIndex = dataIndex == capTypeColumnHeader[2] ? 3 : 4;

	    for (var i = startDataIndex; i < 8; i++) {
	        var chkUserRole = $get(checkbox.id.replace(dataIndex, capTypeColumnHeader[i]));

	        if (chkUserRole) {
	            chkUserRole.checked = checkbox.checked;
	            chkUserRole.disabled = checkbox.checked;
	            record.data[capTypeColumnHeader[i]] = checkbox.checked;
	        }

	        $get("SearchRoleLicenseType" + record.id).disabled = true;
	    }
	}
	else if (dataIndex == capTypeColumnHeader[7]) //LP
	{
	    $get("SearchRoleLicenseType" + record.id).disabled = !checkbox.checked;
	}
	
    DisabledCapTypeButtons(false);
    document.SearchRoleChanged = true;
    parent.parent.ModifyMark('capTypeSearchRole');

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
        if (parmTemp[0] == parmName) {
            return parmTemp[1];
        }
    }
    return "";
}

// render role columns
function renderCapTypeCheckBox(cellValue, cell, currentRowRecord) {
    if (cellValue.toString() == "") { // no capTypes
        return;
    }
    
    var controlID = capTypeGridName + Ext.Const.SplitChar + currentRowRecord.fields.items[cell.id].name + Ext.Const.SplitChar + currentRowRecord.id;
    var returnValue;
    var checkedFlag = "";
    
    if(cellValue=="1")
    {
        checkedFlag = "checked";
    }
    
    returnValue = String.format("<input type='checkbox' id='{0}' "+checkedFlag+" onclick=\"DisabledCapTypeButtons(false);capTypeCheckBox_Clicked('{0}',{1},this)\">",
              controlID,false);
              
    return returnValue;
}

function ChangeSearchRoleLevelType(obj) {
    var newLevelType = obj.value;
    if (oldLevelType != newLevelType) {
        if (document.SearchRoleChanged) {
            if (confirmMsg(Ext.LabelKey.Admin_SearchRole_Message_CancelRoleChange)) {
                ChangeSearchRoleDataSoure(obj);
                document.SearchRoleChanged = false;
                oldLevelType = newLevelType;
            } else {
                if (obj.value == "0") {
                    document.getElementById("rdoEachCapTypeLevel").checked = true;
                } else {
                    document.getElementById("rdoModuleLevel").checked = true;
                }
            }
        } else {
            ChangeSearchRoleDataSoure(obj);
            oldLevelType = newLevelType;
        }
    }
}

function ChangeSearchRoleDataSoure(obj)
{
    document.SearchTypeChanged = true;
    if(obj.value=="0")
    {
        CapTypeRoleList = ModuleLevelCapTypeRoleList;
        CapTypeSearchRoleGrid.colModel.setHidden(8,true);
        CapTypeSearchRoleGrid.setTitle(Ext.LabelKey.Admin_SearchRole_ModuleLevelTitle,null);
        tempIsModuleLevel = true;
        DisabledCapTypeButtons(false);
        parent.parent.ModifyMark('capTypeSearchRole');
    }
    else
    {
        CapTypeRoleList = CapTypeLevelRoleList;
        CapTypeSearchRoleGrid.colModel.setHidden(8,false);
        CapTypeSearchRoleGrid.setTitle(Ext.LabelKey.Admin_SearchRole_CapTypeLevelTitle,null);
        tempIsModuleLevel = false;
        DisabledCapTypeButtons(false);
        parent.parent.ModifyMark('capTypeSearchRole');
    }
    
    // Sort cap type role list by report name (column 1)
    CapTypeRoleList.sort(function(x, y) {
        return x[1].localeCompare(y[1]);
    });
    
    CapTypeSearchRoleGrid.getStore().loadData(CapTypeRoleList); 
    //LoadCapTypeGrid();
}

// true: browser's type is fireFox
// false: browser's type isn't fireFox
function isFireFox()
{
    if(navigator.userAgent.indexOf("Firefox")>0)
    {
        return true;
    } 
    else
    {
        return false; 
    }  
}

function addLicenseListButton(cellValue, cell, currentRowRecord){
    var capTypeName = currentRowRecord.data[capTypeColumnHeader[0]];
    var buttonID = "SearchRoleLicenseType" + currentRowRecord.id;
    var disabled = "";
    if(currentRowRecord.data[capTypeColumnHeader[7]] == "0" || currentRowRecord.data[capTypeColumnHeader[2]] == "1" || currentRowRecord.data[capTypeColumnHeader[3]] == "1")
    {
        disabled = 'disabled = "disabled"';
    }
    
    var htmlString = '<input '+disabled+' id="'+buttonID+'" title="Configure" value="Configure" type="button" width="100" onclick="openLicenseList(\''+capTypeName+'\',this)" />';
    
    return htmlString;
}

var recordTypeSearchRoleContext = {
    dialogProperty: {
        itemContainerID: 'recordTypeSearchRolePopupContainer',
        itemIDPrefix: 'chkRecordTypeSearchRole',
        positionObjID: null,
        sectionTitle: 'Licensed Professional Types',
        items: null,
        saveButtonID: 'btnSaveRecordTypeSearchRole',
        save: saveLicenseTypeList
    }
};

var recordTypeDialog = null;

function openLicenseList(capTypeName,obj)
{
    var moduleName = document.moduleName;
    Accela.ACA.Web.WebService.AdminConfigureService.GetLicenseTypeList(moduleName, capTypeName, "1", function licenseTypeListCallback(callBackLicenseTypeList){
        var response = eval('(' + callBackLicenseTypeList + ')');
        var options;
        var selectedAllChecked = true;
        
        if(response!=null && response.LisenTypeList!= null)
        {
            options = response.LisenTypeList;
        }
        
        if(options!=null && options.length > 0)
        {
            recordTypeSearchRoleContext.recordType = capTypeName;
            recordTypeSearchRoleContext.dialogProperty.items = options;
            recordTypeSearchRoleContext.dialogProperty.positionObjID = obj.id;

            if (!recordTypeDialog) {
                recordTypeDialog = new CheckBoxListDialog(recordTypeSearchRoleContext);
            }

            recordTypeDialog.Show();
         }
    })
}

// this function is save the changed items
// params:
//      saveobj:    the webcontrol which will be saved
function saveLicenseTypeList(context) {
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

    Accela.ACA.Web.WebService.AdminConfigureService.SaveLicenseTypeListByCapType(moduleName, context.recordType, "1", arrVisible, function saveLicenseTypeListCallBack(callBackSaveLicenseList) {
        recordTypeDialog.Close();
    })
};


function btnCapTypeOK_Clicked()
{
    parent.showMessageBox();
    SaveCapTypeRoles(true);
}

// save datas and reload the grid
function SaveCapTypeRoles(showMessage) 
{
    if(document.SearchTypeChanged)
    {
        SaveSearchLevel();
    }
    
    if(!document.SearchRoleChanged) 
    {
        return;
    }
    
    var moduleName = document.moduleName;
    if (CapTypeRoleList.toString() != "" && document.SearchRoleChanged) 
    {
        GetCapTypeUserRoleList();
        Accela.ACA.Web.WebService.AdminConfigureService.SaveRolesForCapTypes(CapTypeRoleList, moduleName, tempIsModuleLevel, SaveCapTypeRoleCallBack, null, showMessage);
    }
}

function SaveSearchLevel(showMessage)
{
    Accela.ACA.Web.WebService.AdminConfigureService.SaveSearchLevel(ModuleLevelCapTypeRoleList, tempIsModuleLevel, SaveSearchLevelCallBack, null, showMessage);
}

function btnCapTypeCancel_Clicked() {
    if (document.SearchTypeChanged) {
        if (isModuleLevel) {
            document.getElementById("rdoModuleLevel").checked = true;
            CapTypeRoleList = ModuleLevelCapTypeRoleList;
            tempIsModuleLevel = true;
            oldLevelType = "0";
        } else {
            document.getElementById("rdoEachCapTypeLevel").checked = true;
            CapTypeRoleList = CapTypeLevelRoleList;
            tempIsModuleLevel = false;
            oldLevelType = "1";
        }

        document.SearchTypeChanged = false;
    }

    DisabledCapTypeButtons(true);

    RefreshDataStore();
    //CapTypeSearchRoleGrid.getStore().loadData(CapTypeRoleList);
    //LoadCapTypeGrid();
    changeGridUI();
    document.SearchRoleChanged = false;
}

//get latest data and display for cap type filter grid.
function RefreshDataStore()
{
    var store = ChangeCapTypeRolesStore(CapTypeRoleList); 
     var grid = Ext.getCmp('capTypeSearchRoleGrid');
    var columnModel = grid.getColumnModel(); 
    grid.reconfigure(store,columnModel);
     grid.getView().refresh();
}

function LoadCapTypeGrid() 
{
    for (var i = 0; i < CapTypeRoleList.length; i++) 
    {
        for (var j = 2; j < capTypeColumnHeader.length - 1; j++) 
        {
            var chkCtl = getCapTypeRoleCheckBox(capTypeColumnHeader[j], i);
            
            if (chkCtl.checked && !chkCtl.disabled) 
            {
                capTypeCheckBox_Clicked(chkCtl.id,true,chkCtl);
                break;
            }
        }
    }
}

function LoadGridFirstTime() 
{

    var startIndex = parseInt(CapTypeSearchRoleGrid.getStore().getAt(0).id);
    
    for (var i = 0; i < CapTypeRoleList.length; i++) 
    {
        var chkCtlAllACAUser = getCapTypeRoleCheckBox(capTypeColumnHeader[2], i);
        
        if (CapTypeRoleList[i][2]=="1") 
        { 
            chkCtlAllACAUser.checked = true;
        }
        
        var chkCtlRegisterUser = getCapTypeRoleCheckBox(capTypeColumnHeader[3], i);
        
        if (CapTypeRoleList[i][7]=="1") 
        { 
            chkCtlRegisterUser.checked = true;
            if(chkCtlAllACAUser.checked)
            {
                chkCtlRegisterUser.disabled = true;
            }
        }
             
        var chkCtlCapCreater = getCapTypeRoleCheckBox(capTypeColumnHeader[4], i);   
        if (CapTypeRoleList[i][3]=="1") 
        { 
            chkCtlCapCreater.checked = true;
            if(chkCtlAllACAUser.checked || chkCtlRegisterUser.chekced)
            {
                chkCtlCapCreater.disabled = true;
            }
        }
        
        var chkCtlContact = getCapTypeRoleCheckBox(capTypeColumnHeader[5], i);   
        if (CapTypeRoleList[i][5]=="1") 
        { 
            chkCtlContact.checked = true;
            if(chkCtlAllACAUser.checked || chkCtlRegisterUser.chekced)
            {
                chkCtlContact.disabled = true;
            }
        }
        
        var chkCtlOwner = getCapTypeRoleCheckBox(capTypeColumnHeader[6], i);   
        if (CapTypeRoleList[i][6]=="1") 
        { 
            chkCtlOwner.checked = true;
            if(chkCtlAllACAUser.checked || chkCtlRegisterUser.chekced)
            {
                chkCtlOwner.disabled = true;
            }
        }
        
        var chkCtlLP = getCapTypeRoleCheckBox(capTypeColumnHeader[7], i);   
        if (CapTypeRoleList[i][4]=="1") 
        { 
            chkCtlLP.checked = true;
            if(chkCtlAllACAUser.checked || chkCtlRegisterUser.chekced)
            {
                chkCtlLP.disabled = true;
            }
        }
        
        document.getElementById("SearchRoleLicenseType" + (startIndex + i)).disabled =chkCtlAllACAUser.checked || chkCtlRegisterUser.checked || !chkCtlLP.checked;
    }
} 

function getCapTypeRoleCheckBox(userRole,rowIndex) 
{
    var startIndex = parseInt(CapTypeSearchRoleGrid.getStore().getAt(0).id);
    var checkBox = $get(capTypeGridName + Ext.Const.SplitChar + userRole + Ext.Const.SplitChar + (startIndex + rowIndex).toString());
    return checkBox;
}

function capTypeCheckBox_Clicked(chkID,isFirstLoad,obj) 
{
    var userRole = chkID.split(Ext.Const.SplitChar)[1];
    var rowIndex = chkID.split(Ext.Const.SplitChar)[2];
    var id = capTypeGridName + Ext.Const.SplitChar + '{0}' + Ext.Const.SplitChar + rowIndex;
    var chkAllACA = $get(String.format(id, capTypeColumnHeader[2]));
    var chkRegisteredUser = $get(String.format(id, capTypeColumnHeader[3]));
    var chkCAPCreator = $get(String.format(id, capTypeColumnHeader[4]));
    var chkContact = $get(String.format(id, capTypeColumnHeader[5]));
    var chkOwner = $get(String.format(id, capTypeColumnHeader[6]));
    var chkLicensedProfessional = $get(String.format(id, capTypeColumnHeader[7]));
    
    var isChecked = obj.checked;
    
    var allCheckBoxs = new Array(chkRegisteredUser, chkCAPCreator, chkContact, chkOwner, chkLicensedProfessional);
    var registeredCheckBoxs = new Array(chkCAPCreator, chkContact, chkOwner, chkLicensedProfessional);
        
    switch(userRole)
    {
        // if all aca user checked, set all user roles checked and disabled
        case capTypeColumnHeader[2]:
            for (var i = 0; i < allCheckBoxs.length; i++) 
            {
                allCheckBoxs[i].checked = chkAllACA.checked;
                allCheckBoxs[i].disabled = chkAllACA.checked;
            }
            
            if(isChecked)
            {
                document.getElementById("SearchRoleLicenseType" + rowIndex).disabled = isChecked;
            }
            break;
        // if register user checked, set licensed professinal user checked and disabled.
        case capTypeColumnHeader[3]:
            for(var i=0;i< registeredCheckBoxs.length;i++)
            {
                registeredCheckBoxs[i].checked = chkRegisteredUser.checked;
                registeredCheckBoxs[i].disabled = chkRegisteredUser.checked;
            }
                            
            if(isChecked)
            {
                document.getElementById("SearchRoleLicenseType" + rowIndex).disabled = isChecked;
            }
            break;
        case capTypeColumnHeader[7]:
            document.getElementById("SearchRoleLicenseType" + rowIndex).disabled = !isChecked;
            break;

        default: 
            break;
    }
        
    if (!isFirstLoad) 
    {
        parent.parent.ModifyMark('capTypeSearchRole');
        document.SearchRoleChanged = true;
    }
}

// set the OK and Cancel Button disabled or not
function DisabledCapTypeButtons(isDisabled) 
{
    if (isDisabled)    
    { 
        
        if (typeof (parent.parent.RemoveMark) != "undefined") 
        {
            parent.parent.RemoveMark(false, 'capTypeSearchRole');
        }
        
        capTypeBtnOK.disable();
        capTypeBtnCancel.disable();
    }
    else 
    {
        capTypeBtnOK.enable();
        capTypeBtnCancel.enable();
    }
} 

// set the OK Button disabled or not.
function DisabledCapTypeDropDownButtons(isDisabled) 
{
    var btnOK=Ext.getCmp("btnCapTypeDropDownOK");
    
    if(!isDisabled)
    {
        btnOK.enable();
    }
    else
    {
        btnOK.disable();
    }
}

function SaveCapTypeRoleCallBack(result, showMessage) 
{
    if (result) 
    {
        DisabledCapTypeButtons(true);
        isModuleLevel = tempIsModuleLevel;
        Ext.MessageBox.hide();
        if (showMessage) {
            alert('Save successfully for Cap Type role setting.');
        }
    }
    else 
    {
        Ext.MessageBox.hide();
        alert('Save unsuccessfully for Cap Type role setting.');
    }

    document.SearchRoleChanged = false;
}

function SaveSearchLevelCallBack(result, showMessage)
{
    if (result) 
    {
        if (typeof (parent.parent.RemoveMark) != "undefined") 
        {
            parent.parent.RemoveMark(false, 'capTypeSearchRole');
        }
        
        isModuleLevel = tempIsModuleLevel;
        DisabledCapTypeButtons(true);
        Ext.MessageBox.hide();
        if (showMessage) {
            alert('Save successfully for Search Level setting.');
        }
    }
    else 
    {
        Ext.MessageBox.hide();
        alert('Save unsuccessfully for Search Level setting.');
    }
}
