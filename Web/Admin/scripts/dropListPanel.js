/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: dropListPanel.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: dropListPanel.js 77905 2007-10-15 12:49:28Z ACHIEVO\lytton.cheng $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */

var needSave=false;
var currentSelectedDropdownlistId;
var currentColumn = -1;
var saveobj ;
var droptype;
var showType = 0;
var winDefaultWidth = 195;
var updateExtenseObjects = new Array();
var EXTENSE_TYPE_LICENSING_BOARD = "ACA_LICENSING_BOARD";

var newFormWin=new Ext.Window({
    layout:'fit',
    width:winDefaultWidth,
    height:325, 
    closeAction:'hide',
    resizable: false,
    plain: true,
    html: '<div id = "container" style="WIDTH:99%; HEIGHT: 270px;overflow-x:hidden;overflow-y:auto;"></div>',
    bbar: [new Ext.Toolbar.TextItem("&nbsp;&nbsp;&nbsp;"),
               new Ext.Toolbar.Button({text:"OK", id: "btnDropDownOK",pressed:true,minWidth:50,handler: isExistBizDomainValue}),
               new Ext.Toolbar.Spacer(),new Ext.Toolbar.Spacer(),new Ext.Toolbar.Spacer(),
               new Ext.Toolbar.Button({text:"Cancel",id: "btnCancel",pressed:true,minWidth:50,handler: CloseDDLWin})]
});

function CloseDDLWin()
{
    var container = document.getElementById('container');
    
    if(container)
    {    
        container.innerHTML = '';
    }
    
    HideWindowAndDragDiv(newFormWin);
}

function SetFormWinPosition(cell)
{
    var top = cell.y + cell.height;
    var left;
    var width;
    
    if(Ext.Const.IsSupportMultiLang)
    {
       if(showType == 1 || showType == 2)
       {
          left = cell.x + cell.width - 550;
          width = 555;
       }
       else
       {
          left = cell.x + cell.width - 320;
          width = 315;
       }
    }
    
    else
    {
        if(showType == 1 || showType == 2)
        {
           left = cell.x + cell.width - 320;
           width = 315;
        }
        else
        {
           left = cell.x + cell.width - 230;
           width = 215;
        }
    }

    if (saveobj != null)
    {
        var type = saveobj._getType();

        if (type == 20 || type == 16) //20: contact type control. 16: hard code type
        {
            //added check box, setting width and left.
            width += 20;
            left -= 20;
        }
    }
    
    if (Ext.isSafari)
    {
        width += 20;
        left-=20;
    }

    newFormWin.setPosition(left,top);
    newFormWin.setWidth(width);
    newFormWin.setTitle('');
}
    
function LoadRadioButtonList(obj,type)
{
    CloseWindow();
    
    saveobj = obj;
    droptype = type;
    currentSelectedDropdownlistId= obj._id;
    needSave=false;

    newFormWin.render(Ext.getBody());
    
    var container = document.getElementById('container');
    container.innerHTML = '';
   
    var propsGrid = new Ext.grid.PropertyGrid({
        id: 'propsGrid',
        nameText: 'Properties Grid',
        width:300,
        autoHeight:true,
        listeners:{
            propertychange:function(source,recordId,value,oldValue ){
                if(currentColumn == 1 && needSave)
                {
                    if(recordId == 'Admin_FieldProperty_FieldLabel'){
                        obj._setLabel(EncodeHTMLTag(value));
                        UpdateDefaultLangValue(propsGrid, value);
                    }
                    if(recordId == 'Admin_FieldProperty_Instructions')
                    {
                        obj._setSubLabel(EncodeHTMLTag(value));
                    }
                    if(oldValue != value)
                    {
                        var changeItem = new DropDownObj(Ext.Const.OpenedId, obj._controlIdValue, type, obj._labelKeyValue, obj._getLabel(), obj.get_IsTemplateField(), obj.get_TemplateAttribute(), obj._labelKeyValue + '|sub', obj._getSubLabel(), obj._stdCategoryValue, GetDropDownItems());
                        changeItems.UpdateItem(3,changeItem);
                        ModifyMark();
                    }
                }
            },
            beforeedit: function(e) {
                var record = e.grid.store.getAt(e.row);
                if (record.id == 'Admin_FieldProperty_Instructions') {
                    e.cancel = true;
                    if (!isWinShow) {
                        PopupHtmlEditor(e.grid, obj._getSubLabel());
                        htmlEditorTargetType = htmlEditorTargetTypes.Field;
                        htmlEditorTargetCtrl = obj;
                    }
                }
                else if (record.id == 'Admin_FieldProperty_Choice') {
                    e.cancel = true;
                }
            },
            cellclick: function(grid, rowIndex, columnIndex, e){
                currentColumn=1;
                var record = grid.store.getAt(rowIndex)
                var cell = grid.getBox();
                if(record.id == 'Admin_FieldProperty_Choice' && type != 'DB'){
                        DisabledDropDownButtons(true);
                        ValidateTextfield(false);
                        container.innerHTML = '';
                        var dropdownObj = obj._getRadioButtonListItems();
                        getRadioButtonList(type,dropdownObj,obj);
                        
                        SetFormWinPosition(cell);
                        newFormWin.show();
                        isDDLWinShow = true;
                }
            }
        }
    });
    
    propsGrid.store.sortInfo = null;
    if(obj._defaultSubLabelValue)
    {
        if(type == "PaymentHardCode")
	    {
	        propsGrid.setSource({
	            'Admin_FieldProperty_Instructions': Ext.LabelKey.Admin_ClickToEdit_Watermark,
	            "Admin_FieldProperty_Choice": ""
	        });
	    }
    }
    else
    {
        if(type == "PaymentHardCode")
	    {
	        propsGrid.setSource({
	            "Admin_FieldProperty_Choice": ""
	        });
	    }
    }
    propsGrid.getColumnModel().setConfig([
          {header: 'Name',sortable: false,dataIndex:'name', id: 'name'},
          {header: 'Value',sortable: false,dataIndex: 'value', id: 'value'}
        ]);
    

    if(type == 'PaymentHardCode')
    { 
        propsGrid.store.getById('Admin_FieldProperty_Choice').set('name',Ext.LabelKey.Admin_FieldProperty_Choice_Payment);
    }
    
    propsGrid.store.getById('Admin_FieldProperty_Choice').set('value',Ext.LabelKey.Admin_FieldProperty_ChoiceTip);

    RenderGrid(0,propsGrid);
    	    
    var btn = Ext.DomHelper.insertAfter('propsGrid',{
    id:'divContainer',
    tag:'div',
    cls:'dbItemContainer',
    html:   '<div id = "divDBItems" ></div>'
    },true);
 
    if(obj._defaultSubLabelValue)
    {
        propsGrid.store.getById('Default Instructions').set('value',obj._defaultSubLabelValue);
    }
    needSave = true;    
};

function LoadDropListInfo(obj,type)
{
    if(obj._labelKeyValue=="aca_licensee_licensingboard")
    {
        LoadDropListInfoForLicensingBoard(obj, type);
        return;
    }
    
    CloseWindow();
    
    saveobj = obj;
    droptype = type;
    currentSelectedDropdownlistId= obj._id;
    needSave=false;

    newFormWin.render(Ext.getBody());
    
    var container = document.getElementById('container');
    container.innerHTML = '';

    //newFormWin.addListener('beforehide',saveList);
    var disabledEditor = obj.get_IsTemplateField() ? PropertyObj.templateDisabledEditor : PropertyObj.standardDisabledEditor;

    var propsGrid = new Ext.grid.PropertyGrid({
        id: 'propsGrid',
        nameText: 'Properties Grid',
        autoHeight: true,
        customEditors: disabledEditor,
        listeners: {
            propertychange: function (source, recordId, value, oldValue) {
                if (currentColumn == 1 && needSave) {
                    if (recordId == 'Admin_FieldProperty_FieldLabel') {
                        obj._setLabel(EncodeHTMLTag(value));
                        UpdateDefaultLangValue(propsGrid, value);
                    }
                    if (recordId == 'Admin_FieldProperty_Instructions') {
                        obj._setSubLabel(EncodeHTMLTag(value));
                    }
                    if (oldValue != value) {
                        var changeItem = new DropDownObj(Ext.Const.OpenedId, obj._controlIdValue, type, obj._labelKeyValue, obj._getLabel(), obj.get_IsTemplateField(), obj.get_TemplateAttribute(), obj._labelKeyValue + '|sub', obj._getSubLabel(), obj._stdCategoryValue, GetDropDownItems());
                        changeItems.UpdateItem(3, changeItem);
                        ModifyMark();
                    }
                }
            },
            beforeedit: function (e) {
                ValidateTextfield(false, '');
                DisabledDropDownButtons(true);
                currentColumn = 1;
                var record = e.grid.store.getAt(e.row);
                var cell = e.grid.getBox();
                if (record.id == 'Admin_FieldProperty_Choice' && type != 'DB') {
                    e.cancel = true;

                    if (obj._getDropdownItems().length > 0 || type == 'StandardChoice' || type == 'STDandXPolicy') {
                        container.innerHTML = '';
                        var dropdownObj = obj._getDropdownItems();
                        oldDropDownListItems = dropdownObj;
                        showType = type == 'StandardChoice' ? dropdownObj.ShowType : -1;
                        getList(type, dropdownObj, obj);

                        SetFormWinPosition(cell);
                        newFormWin.show();
                        isDDLWinShow = true;
                    }
                }
                else if (record.id == 'Admin_FieldProperty_Instructions') {
                    e.cancel = true;
                    if (!isWinShow) {
                        var subLabel;
                        if (type == "HardCode") {
                            subLabel = GetDom(CurrentTab).getElementById('ctl00_PlaceHolderMain_lblSearchInstruction').innerHTML;
                            htmlEditorTargetType = htmlEditorTargetTypes.Search_Section;
                        }
                        else {
                            subLabel = obj._getSubLabel();
                            htmlEditorTargetType = htmlEditorTargetTypes.Field;
                        }
                        htmlEditorTargetCtrl = obj;
                        PopupHtmlEditor(e.grid, subLabel);
                    }
                }
            }
        }
    });

    propsGrid.store.sortInfo = null;

    obj.needInstruction = true;
    var store = CreatePropertyStore(obj);

    if (type == "DB") {
        propsGrid.setSource(store);
    }

    else if (type == "HardCode") {
        propsGrid.setSource({
            "Admin_FieldProperty_Choice": "",
            "Admin_FieldProperty_Instructions": Ext.LabelKey.Admin_ClickToEdit_Watermark
        });
    }
    else {
        store["Admin_FieldProperty_Choice"] = "";
        propsGrid.setSource(store);
    }
    
    propsGrid.getColumnModel().setConfig([
          {header: 'Name',sortable: false,dataIndex:'name', id: 'name'},
          {header: 'Value',sortable: false,dataIndex: 'value', id: 'value'}
        ]);
    
    if(type != "HardCode")
    {
        if (!obj.get_IsHiddenLabel()) {
            propsGrid.store.getById('Admin_FieldProperty_FieldLabel').set('name', Ext.LabelKey.Admin_FieldProperty_FieldLabel);
            if (Ext.Const.IsSupportMultiLang)
                propsGrid.store.getById('Admin_FieldProperty_FieldLabel_DefaultLanguage').set('name', Ext.LabelKey.Admin_FieldProperty_FieldLabel + '(Default Language)');
        }

        propsGrid.store.getById('Admin_FieldProperty_Instructions').set('name', Ext.LabelKey.Admin_FieldProperty_Instructions);
    }
    if(type != 'DB')
    {
        if(type == 'HardCode')
        {
            var choiceName;
            
            if(obj._stdCategoryValue==Ext.Constant.APO_LOOKUP_TYPE)
            {
                choiceName = Ext.LabelKey.Admin_FieldProperty_Choice_LookUp;
            }
            else if(obj._stdCategoryValue==Ext.Constant.CAP_SEARCH_TYPE)
            {
                choiceName = Ext.LabelKey.Admin_FieldProperty_Choice_Search;
            }
            else if(obj._stdCategoryValue==Ext.Constant.CAP_PAYMENT_TYPE)
            {
                choiceName = Ext.LabelKey.Admin_FieldProperty_Choice_Payment;
            }
            else if(obj._stdCategoryValue==Ext.Constant.EDUCATION_LOOKUP_TYPE)
            {
                choiceName = Ext.LabelKey.admin_fieldproperty_choice_education_lookup;
            }

            propsGrid.store.getById('Admin_FieldProperty_Choice').set('name', choiceName);
            propsGrid.store.getById('Admin_FieldProperty_Instructions').set('name', 'Section Instructions');
        }
        else
        {
           // remove the last char ":" and trim
           var defaultLabel = obj._defaultLanguageTextValue.replace(/(\s*$)/g, "");
           defaultLabel=defaultLabel.replace(/(:$)/g, "");
           
           propsGrid.store.getById('Admin_FieldProperty_Choice').set('name',defaultLabel+' '+Ext.LabelKey.Admin_FieldProperty_Choice);
        }
        
        propsGrid.store.getById('Admin_FieldProperty_Choice').set('value',Ext.LabelKey.Admin_FieldProperty_ChoiceTip);
    }

    RenderGrid(0, propsGrid);
    
    needSave = true;

    if(obj.type == 16)
    {
        obj.type = 16;
        GetSectionFields(obj,null);
    }
    else if (obj.type == 15) 
    {
        var btn = Ext.DomHelper.insertAfter('propsGrid', {
            id: 'divContainer',
            tag: 'div',
            cls: 'dbItemContainer',
            html: '<div id = "divDBItems" ></div>'
        }, true);
        
        var dropdownObj = obj._getDropdownItems();
        oldDropDownListItems = dropdownObj;
        getList(type,dropdownObj,obj);
    }  
    
};

//Use for LicensingBoard drop down list.
function LoadDropListInfoForLicensingBoard(obj,type)
{    
    var licensingBoardRequired = false;
    if (typeof (obj.EnableSearchRequired) != 'undefined') {
        licensingBoardRequired = obj.EnableSearchRequired;
        InitLicensingBoardGrid(obj, type, licensingBoardRequired);
    }
    else{
        Accela.ACA.Web.WebService.AdminConfigureService.GetLicensingBoardRequired(function LicensingBoardCallback(callBackLicensingBoard) {
            InitLicensingBoardGrid(obj, type, callBackLicensingBoard);
        });
    }
};

function InitLicensingBoardGrid(obj,type,licensingBoardRequired)
{    
    CloseWindow();
    saveobj = obj;
    droptype = type;
    currentSelectedDropdownlistId= obj._id;
    needSave=false;

    newFormWin.render(Ext.getBody());
    var container = document.getElementById('container');
    container.innerHTML = '';
    var disabledEditor = obj.get_IsTemplateField() ? PropertyObj.templateDisabledEditor : PropertyObj.standardDisabledEditor;

    var propsGrid = new Ext.grid.PropertyGrid({
        id: 'propsGrid',
        nameText: 'Properties Grid',
        autoHeight: true,
        customEditors: disabledEditor,
        listeners: {
            propertychange: function(source, recordId, value, oldValue) {
                if (currentColumn == 1 && needSave) {
                    if (recordId == 'Admin_FieldProperty_FieldLabel') {
                        obj._setLabel(EncodeHTMLTag(value));
                        UpdateDefaultLangValue(propsGrid, value);
                    }
                    if (recordId == 'Admin_FieldProperty_Instructions') {
                        obj._setSubLabel(EncodeHTMLTag(value));
                    }
                    if (oldValue != value) {

                        if (recordId == 'admin_fieldproperty_searchrequired') {
                            var requireIconID = obj._id.substring(0, obj._id.lastIndexOf("_") + 1) + obj._controlIdValue;
                            var boolValue = Boolean.parse(value);
                            ControlTheAsterisk(requireIconID, boolValue);
                            obj.EnableSearchRequired = boolValue;
                        }

                        var changeItem = new DropDownObj(Ext.Const.OpenedId, obj._controlIdValue, type, obj._labelKeyValue, obj._getLabel(), obj.get_IsTemplateField(), obj.get_TemplateAttribute(), obj._labelKeyValue + '|sub', obj._getSubLabel(), obj._stdCategoryValue, GetDropDownItems(), null, null, null, obj.EnableSearchRequired);
                        changeItems.UpdateItem(3, changeItem);
                        ModifyMark();
                    }
                }
            },
            beforeedit: function(e) {
                ValidateTextfield(false, '');
                DisabledDropDownButtons(true);
                currentColumn = 1;
                var record = e.grid.store.getAt(e.row);
                var cell = e.grid.getBox();
                if (record.id == 'Admin_FieldProperty_Choice') {
                    e.cancel = true;
                    if (obj._getDropdownItems().length > 0 || type == 'StandardChoice' || type == 'STDandXPolicy') {
                        container.innerHTML = '';
                        var dropdownObj = obj._getDropdownItems();
                        oldDropDownListItems = dropdownObj;
                        showType = type == 'StandardChoice' ? dropdownObj.ShowType : -1;
                        getList(type, dropdownObj, obj);

                        SetFormWinPosition(cell);
                        newFormWin.show();
                        isDDLWinShow = true;
                    }
                }
                else if (record.id == 'Admin_FieldProperty_Choice_LicenseType') {
                    e.cancel = true;
                    var $dropDownObj = $(obj._element);
                    container.innerHTML = '';
                    var licensingBoards = obj._getDropdownItems();

                    if (typeof (licensingBoards) != "undefined" && typeof (licensingBoards.Items) != "undefined") {
                        var licensingBoardVal = $dropDownObj.val();
                        
                        //Clear the previous showType value.
                        showType = 0;
                        RenderLicensingBoardsControl(licensingBoards.Items, licensingBoardVal);
                    }

                    SetFormWinPosition(cell);
                    newFormWin.setTitle('License Type');
                    newFormWin.show();
                    isDDLWinShow = true;
                }
                else if (record.id == 'Admin_FieldProperty_Instructions') {
                    e.cancel = true;
                    if (!isWinShow) {
                        var subLabel;
                        if (type == "HardCode") {
                            subLabel = GetDom(CurrentTab).getElementById('ctl00_PlaceHolderMain_lblSearchInstruction').innerHTML;
                            htmlEditorTargetType = htmlEditorTargetTypes.Search_Section;
                        }
                        else {
                            subLabel = obj._getSubLabel();
                            htmlEditorTargetType = htmlEditorTargetTypes.Field;
                        }
                        htmlEditorTargetCtrl = obj;
                        PopupHtmlEditor(e.grid, subLabel);
                    }
                }
            }
        }
    });

    propsGrid.store.sortInfo = null;

    obj.needInstruction = true;
    
    obj.needRequire = true;
    
    var store = CreatePropertyStore(obj);


    store["Admin_FieldProperty_Choice"] = "";
    store["Admin_FieldProperty_Choice_LicenseType"] = "";
    propsGrid.setSource(store);
        
    propsGrid.getColumnModel().setConfig([
          {header: 'Name',sortable: false,dataIndex:'name', id: 'name'},
          {header: 'Value',sortable: false,dataIndex: 'value', id: 'value'}
        ]);
    
    propsGrid.store.getById('Admin_FieldProperty_FieldLabel').set('name',Ext.LabelKey.Admin_FieldProperty_FieldLabel);
    propsGrid.store.getById('Admin_FieldProperty_Instructions').set('name',Ext.LabelKey.Admin_FieldProperty_Instructions);
    if(Ext.Const.IsSupportMultiLang)
        propsGrid.store.getById('Admin_FieldProperty_FieldLabel_DefaultLanguage').set('name',Ext.LabelKey.Admin_FieldProperty_FieldLabel+'(Default Language)');
 
   // remove the last char ":" and trim
   var defaultLabel = obj._defaultLanguageTextValue.replace(/(\s*$)/g, "");
   defaultLabel = defaultLabel.replace(/(:$)/g, "");

   var licenseType = Ext.LabelKey.Admin_FieldProperty_LicenseType.replace(/(\s*$)/g, "");
   licenseType = licenseType.replace(/(:$)/g, "");

   propsGrid.customEditors[Ext.LabelKey.admin_fieldproperty_searchrequired] = new BoolComboBoxGridEditor();
   propsGrid.store.getById('Admin_FieldProperty_Choice').set('name', defaultLabel + ' ' + Ext.LabelKey.Admin_FieldProperty_Choice);
   propsGrid.store.getById('Admin_FieldProperty_Choice_LicenseType').set('name', licenseType + ' ' + Ext.LabelKey.Admin_FieldProperty_Choice);
   propsGrid.store.getById('admin_fieldproperty_searchrequired').set('name',Ext.LabelKey.admin_fieldproperty_searchrequired);


   propsGrid.store.getById('Admin_FieldProperty_Choice').set('value', Ext.LabelKey.Admin_FieldProperty_ChoiceTip);
   propsGrid.store.getById('Admin_FieldProperty_Choice_LicenseType').set('value', Ext.LabelKey.Admin_FieldProperty_ChoiceTip);
 
    propsGrid.store.getById('admin_fieldproperty_searchrequired').set('value', licensingBoardRequired);
    RenderGrid(0, propsGrid);
    
    needSave = true;

    if(obj.type == 16)
    {
        obj.type = 16;
        GetSectionFields(obj,null);
    }
    else if (obj.type == 15) 
    {
        var btn = Ext.DomHelper.insertAfter('propsGrid', {
            id: 'divContainer',
            tag: 'div',
            cls: 'dbItemContainer',
            html: '<div id = "divDBItems" ></div>'
        }, true);
        
        var dropdownObj = obj._getDropdownItems();
        oldDropDownListItems = dropdownObj;
        getList(type,dropdownObj,obj);
    }
}

function RenderLicensingBoardsControl(licensingBoardItems, licensingBoardVal) {
    $('#container').html('');
    
    if (licensingBoardItems == null || licensingBoardItems.length == 0) {
        Ext.DomHelper.insertFirst('container', {
            id: 'btn',
            tag: 'div',
            html: '<table><tr><th scope="col"><label>No licensing boards found.</label></th></tr></table>'
        }, true);

        return;
    }
    
    updateExtenseObjects = new Array();
    var updateItems = new DropDownObj(Ext.Const.OpenedId, saveobj._element.id, droptype, saveobj._labelKeyValue, saveobj._getLabel(), saveobj.get_IsTemplateField(), saveobj.get_TemplateAttribute(), saveobj._labelKeyValue + '|sub', saveobj._getSubLabel(), saveobj._stdCategoryValue);
    var existItem = changeItems.GetItem(changeItems.ItemType.arrDropDown, updateItems);

    if (existItem != null && typeof (existItem.ExtenseObjects) != "undefined") {
        updateExtenseObjects = existItem.ExtenseObjects;
    }

    var licensingBoardContent = "<table style='width:100%;' role='presentation'><tr><td style='width:85px;'>Licensing Board:</td><td><select id='ddlLicesingBoard4Extense' style='width:" + (Ext.Const.IsSupportMultiLang ? 170 : 120) + "px;' onchange='RenderLicenseType4LicenseBoard();'><option value=''>" + Ext.LabelKey.DropDownDefaultText + "</option>";

    for (var index = 0; index < licensingBoardItems.length; index++) {
        licensingBoardContent += "<option value='" + EncodeHTMLTag(licensingBoardItems[index].value) + "'>" + licensingBoardItems[index].resValue + "</option>";
    }

    licensingBoardContent += "</select>";
    licensingBoardContent += "<input type='hidden' id='hidExtensesKey'/><input type='hidden' id='hidExtenseType' value='" + EXTENSE_TYPE_LICENSING_BOARD + "'/>";
    licensingBoardContent += "<input type='hidden' id='hidChangeLicesingBoardStatus'/>";
    licensingBoardContent += "</td><td style='width:5px;'>&nbsp;</td></tr></table>";

    Ext.DomHelper.insertFirst('container', {
        id: 'btn',
        tag: 'div',
        html: licensingBoardContent
    }, true);
    
    Ext.DomHelper.append('container', {
            tag: 'div',
            id: 'extenseOptions'
       }, true);
    
    $("#ddlLicesingBoard4Extense").val(licensingBoardVal);
    RenderLicenseType4LicenseBoard();
};

function RenderLicenseType4LicenseBoard() {
    var emptyContent = '<table><tr><th scope="col"><label>No license type found.</label></th></tr></table>';
    var licensingBoardVal = $("#ddlLicesingBoard4Extense").val();

    if (licensingBoardVal == "") {
        emptyContent = '<table><tr><th scope="col"><label>Please select a licensing board.</label></th></tr></table>';
    }

    if (IsTrue($("#hidChangeLicesingBoardStatus").val())) {
        UpdateItem4ExtenseObjects();
    }

    $("#hidExtensesKey").val(licensingBoardVal);
    $("#hidChangeLicesingBoardStatus").val("false");

    if (licensingBoardVal != "") {
        var isExist = false;
        var licenseTypeMaps = new Array();
        
        for (var i = 0; i < updateExtenseObjects.length; i++) {
            var extenseObject = updateExtenseObjects[i];
            
            if (extenseObject.extenseType != EXTENSE_TYPE_LICENSING_BOARD) {
                continue;
            }

            var extenseItems = extenseObject.extenseItems;
            
            for (var j = 0; j < extenseItems.length; j++) {
                var existExtenseItem = extenseItems[j];

                if (existExtenseItem.extensesKey != licensingBoardVal) {
                    continue;
                }

                isExist = true;
                licenseTypeMaps = existExtenseItem.updateOptions;
                break;
            }
            
            if (isExist) {
                break;
            }
        }

        if (isExist) {
            var htmlExtenseOptions = BuildExtenseOptionsHtml(licenseTypeMaps);
            $("#extenseOptions").html(htmlExtenseOptions);
            UpdateStatus4LicensingBoardDropDown();

        } else {
            var entityId = Ext.Const.ModuleName;
            if (entityId == "") {
                entityId = GLOBAL_AGENCY_CODE;
            }

            Accela.ACA.Web.WebService.AdminConfigureService.GetLicenseTypesByLicensingBoard(licensingBoardVal, entityId, function (response) {
                var result = eval('(' + response + ')');
                var resultLicenseTypeMaps = result.licenseTypesMapping;

                if (resultLicenseTypeMaps.length > 0) {
                    var resultHtmlExtenseOptions = BuildExtenseOptionsHtml(resultLicenseTypeMaps);
                    $("#extenseOptions").html(resultHtmlExtenseOptions);
                    UpdateStatus4LicensingBoardDropDown();
                } else {
                    $("#extenseOptions").html(emptyContent);
                }
            });
        }

        return;
    } else {
       $("#extenseOptions").html(emptyContent);
    }
}

function BuildExtenseOptionsHtml(licenseTypeMaps) {
    var htmlExtenseOptions = '<div class="Content_Option"><table><tr>' +
                    '<td style="padding-right: 10px;width:15px;"><input id="selectAll" type="checkbox" onclick="SelectAllLicenseType4Board();"/></td>' +
                    '<td style="padding-right: 10px;width: 200px;"><label for="selectAll"><span><b>Select all</b></span></label></td>' +
                    '</tr></table></div>';

    for (var index = 0; index < licenseTypeMaps.length; index++) {
        var licenseItem = licenseTypeMaps[index];
        var check = '';

        if (licenseItem.value == "Y") {
            check = 'checked';
        }

        htmlExtenseOptions += '<div class="Content_Option"><table><tr>' +
                        '<td style="padding-right: 10px;width:15px;"><input name="chkLicenseType4Board" id="chkLicenseType4Board' + index + '" onclick="DisabledDropDownButtons4LicesingBoard();" value="' + licenseItem.entityId3 + '" type="checkbox" ' + check + ' /></td>' +
                        '<td style="word-break:break-all;padding-right: 10px;width: 200px;"><label for="chkLicenseType4Board' + index + '"><span>' + licenseItem.resEntityId3 + '</span></label></td>' +
                        '</tr></table></div>';
    }

    return htmlExtenseOptions;
}

function SelectAllLicenseType4Board() {
    var dragEls = Ext.get('container').query('.Content_Option');

    var selectAllObj = dragEls[0].getElementsByTagName("input")[0];

    if (selectAllObj) {
        var selectAllChecked = selectAllObj.checked;
        var len = dragEls.length;

        for (var i = 1; i < len; i++) {
            var checkobj = dragEls[i].getElementsByTagName("input")[0];

            if (checkobj != null) {
                checkobj.checked = selectAllChecked;
            }
        }
    }

    $("#hidChangeLicesingBoardStatus").val("true");
    DisabledDropDownButtons(false);
}

function DisabledDropDownButtons4LicesingBoard() {
    UpdateStatus4LicensingBoardDropDown();
    $("#hidChangeLicesingBoardStatus").val("true");
    DisabledDropDownButtons(false);
}

function UpdateStatus4LicensingBoardDropDown() {
    var $selectAllObj = $(".Content_Option:eq(0)").find("input");
    var $childOptions = $(".Content_Option:gt(0)").find("input");
    UpdateStatus4SelectAll($selectAllObj, $childOptions);
}
    
function LoadAutoFillDropList(obj,type)
{
    CloseWindow();
    
    saveobj = obj;
    droptype = type;
    currentSelectedDropdownlistId= obj._id;
    needSave=false;

    newFormWin.render(Ext.getBody());
    
    var container = document.getElementById('container');
    container.innerHTML = '';

    var moduleName = Ext.Const.ModuleName;
    var positon;

    Accela.ACA.Web.WebService.AdminConfigureService.GetAutoFillPolicyValue(moduleName, Ext.Constant.AUTOFILL_STATE_ENABLED, obj._positionValue, function GetConfigureUrlCallback(callBackAutoFill) {

        var autoFillState = false;
        if (typeof(obj.AutoFillStateValue) != 'undefined') {
            autoFillState = obj.AutoFillStateValue;
        } else if (callBackAutoFill == "Y") {
            autoFillState = true;
        }

        var disabledEditor = obj.get_IsTemplateField() ? PropertyObj.templateDisabledEditor : PropertyObj.standardDisabledEditor;

        var propsGrid = new Ext.grid.PropertyGrid({
            id: 'propsGrid',
            nameText: 'Properties Grid',
            width: 300,
            autoHeight: true,
            customEditors: disabledEditor,
            listeners: {
                propertychange: function(source, recordId, value, oldValue) {
                    if (currentColumn == 1 && needSave) {
                        if (recordId == 'Admin_FieldProperty_FieldLabel') {
                            obj._setLabel(EncodeHTMLTag(value));
                            UpdateDefaultLangValue(propsGrid, value);
                        }
                        if (recordId == 'Admin_FieldProperty_Instructions') {
                            obj._setSubLabel(EncodeHTMLTag(value));
                        }
                        if (oldValue != value) {

                            if (recordId == 'Admin_FieldProperty_AutoFill') {
                                autoFillState = value;
                                obj.AutoFillStateValue = value;
                                positon = obj._positionValue;
                            }
                            var changeItem = new DropDownObj(Ext.Const.OpenedId, obj._controlIdValue, type, obj._labelKeyValue, obj._getLabel(), obj.get_IsTemplateField(), obj.get_TemplateAttribute(), obj._labelKeyValue + '|sub', obj._getSubLabel(), obj._stdCategoryValue, GetDropDownItems(), autoFillState, Ext.Constant.AUTOFILL_STATE_ENABLED, positon);
                            changeItems.UpdateItem(3, changeItem);
                            ModifyMark();
                        }
                    }
                },
                beforeedit: function(e) {
                    var record = e.grid.store.getAt(e.row);
                    if (record.id == 'Admin_FieldProperty_Instructions') {
                        e.cancel = true;
                        if (!isWinShow) {
                            PopupHtmlEditor(e.grid, obj._getSubLabel());
                            htmlEditorTargetType = htmlEditorTargetTypes.Field;
                            htmlEditorTargetCtrl = obj;
                        }
                    } else if (record.id == 'Admin_FieldProperty_Choice') {
                        e.cancel = true;
                    }
                },
                cellclick: function(grid, rowIndex, columnIndex, e) {
                    ValidateTextfield(false, '');
                    DisabledDropDownButtons(true);
                    currentColumn = 1;
                    var record = grid.store.getAt(rowIndex)
                    var cell = grid.getBox();
                    if (record.id == 'Admin_FieldProperty_Choice') {
                        container.innerHTML = '';
                        var dropdownObj = obj._getDropdownItems();
                        oldDropDownListItems = dropdownObj;
                        showType = type == 'StandardChoice' ? dropdownObj.ShowType : -1;
                        getList(type, dropdownObj, obj);

                        SetFormWinPosition(cell);
                        newFormWin.show();
                        isDDLWinShow = true;
                    }
                }
            }
        });

        propsGrid.customEditors[Ext.LabelKey.admin_fieldproperty_autofill] = new BoolComboBoxGridEditor();
        propsGrid.store.sortInfo = null;
        obj.needInstruction = true;
        var store = CreatePropertyStore(obj);
        store["Admin_FieldProperty_Choice"] = Ext.LabelKey.Admin_FieldProperty_ChoiceTip;
        store["Admin_FieldProperty_AutoFill"] = false;
        propsGrid.setSource(store);

        propsGrid.getColumnModel().setConfig([
            { header: 'Name', sortable: false, dataIndex: 'name', id: 'name' },
            { header: 'Value', sortable: false, dataIndex: 'value', id: 'value' }
        ]);

        // remove the last char ":" and trim
        var defaultLabel = obj._defaultLanguageTextValue.replace(/(\s*$)/g, "");
        defaultLabel = defaultLabel.replace(/(:$)/g, "");
        propsGrid.store.getById('Admin_FieldProperty_Choice').set('name', defaultLabel + ' ' + Ext.LabelKey.Admin_FieldProperty_Choice);
        propsGrid.store.getById('Admin_FieldProperty_AutoFill').set('name', Ext.LabelKey.admin_fieldproperty_autofill);
        propsGrid.store.getById('Admin_FieldProperty_FieldLabel').set('name', Ext.LabelKey.Admin_FieldProperty_FieldLabel);
        propsGrid.store.getById('Admin_FieldProperty_Instructions').set('name', Ext.LabelKey.Admin_FieldProperty_Instructions);
        if (Ext.Const.IsSupportMultiLang) {
            propsGrid.store.getById('Admin_FieldProperty_FieldLabel_DefaultLanguage').set('name', Ext.LabelKey.Admin_FieldProperty_FieldLabel + '(Default Language)');
        }

        propsGrid.store.getById('Admin_FieldProperty_AutoFill').set('value', autoFillState);
        RenderGrid(0, propsGrid);
        needSave = true;

        if (obj.type == 16) {
            obj.type = 16;
            GetSectionFields(obj, null);
        } else if (obj.type == 15) {

            var btn = Ext.DomHelper.insertAfter('propsGrid', {
                id: 'divContainer',
                tag: 'div',
                cls: 'dbItemContainer',
                html: '<div id = "divDBItems" ></div>'
            }, true);

            var dropdownObj = obj._getDropdownItems();
            oldDropDownListItems = dropdownObj;
            getList(type, dropdownObj, obj);
        }
    })
};

//this function is order to get the data from obj
//params:
//      itemType:   same with the Load func
//      itms:       the items data array
//      selectIndex:the selected item's index
function getRadioButtonList(itemType,dropdownObj,obj){
    
    var itms = dropdownObj;
    
    switch(itemType)
    {
        case 'PaymentHardCode':
            Ext.MessageBox.show({
                       msg: 'Saving your data, please wait...',
                       progressText: 'Saving...',
                       width:300,
                       wait:true,
                       waitConfig: {interval:200},
                       icon:'ext-mb-download', //custom class in msg-box.html
                       animEl: 'mb7'
                   });
                   
            if(itms.length>0)
            {
                var itmVisibleArr = obj._getRadioButtonItemsActivity();
                Ext.MessageBox.show({
                           msg: 'Saving your data, please wait...',
                           progressText: 'Saving...',
                           width:300,
                           wait:true,
                           waitConfig: {interval:200},
                           icon:'ext-mb-download', //custom class in msg-box.html
                           animEl: 'mb7'
                       });
                for(var i = itms.length-1; i >= 0; i--){
                    InsertHCItem(itms[i],'container',itmVisibleArr[i]);
                }
                Ext.MessageBox.hide();
            }
            
            Ext.MessageBox.hide();
            
            if(Ext.Const.IsSupportMultiLang)
            {
               InsertLabel('container');
            }
            break;
        default:
            return;
    }
};

//this function is order to get the data from obj
//params:
//      itemType:   same with the Load func
//      itms:       the items data array
//      selectIndex:the selected item's index
function getList(itemType,dropdownObj,obj){
    
    var itms = dropdownObj;
    
    switch(itemType)
    {
        case 'DB':
            // this is for permit type only
            if(obj.type == 15)
            {
            
                var itmVisibleArr = obj._getDropdownItemsActivity();
                if(itms.length>0)
                {
                    InsertDBItem('divDBItems',itms,itmVisibleArr);
                }
            }
            break;
        case 'StandardChoice':
            itms = dropdownObj.Items;
            if (itms.length > 0) {
                var itmVisibleArr = obj._getDropdownItemsActivity();
                var maxLength = obj._maxLengthValue ? obj._maxLengthValue : 255;

                for (var i = itms.length - 1; i >= 0; i--) {
                    //20: contact type.
                    var isVisable = obj.type == 20 ? itmVisibleArr != null && itmVisibleArr.length > 0 && itmVisibleArr[i] : null;
                    InsertSCItem(itms[i], 'container', isVisable, maxLength);
                }
            }

            if (!Ext.Const.IsSupportMultiLang) {
                if (showType == 1 || showType == 2) // show description
                {
                    InsertLabel('container');
                }
                InsertAddBtn('container');
            }
            else {
                InsertLabel('container');
                if (IsDefaultLanguage()) {
                    InsertAddBtn('container');
                }
            }

            break;
        case 'HardCode':
            Ext.MessageBox.show({
                       msg: 'Saving your data, please wait...',
                       progressText: 'Saving...',
                       width:300,
                       wait:true,
                       waitConfig: {interval:200},
                       icon:'ext-mb-download', //custom class in msg-box.html
                       animEl: 'mb7'
                   });
                   
            if(itms.length>0)
            {
                var itmVisibleArr = obj._getDropdownItemsActivity();
                Ext.MessageBox.show({
                           msg: 'Saving your data, please wait...',
                           progressText: 'Saving...',
                           width:300,
                           wait:true,
                           waitConfig: {interval:200},
                           icon:'ext-mb-download', //custom class in msg-box.html
                           animEl: 'mb7'
                       });
                for(var i = itms.length-1; i >= 0; i--){
                    InsertHCItem(itms[i],'container',itmVisibleArr[i]);
                }
                Ext.MessageBox.hide();
            }
            
            Ext.MessageBox.hide();
            
            if(Ext.Const.IsSupportMultiLang)
            {
               InsertLabel('container');
            }
            break;
        case 'STDandXPolicy':
            itms = dropdownObj.Items;
            if (itms.length > 0) {
                var itmVisibleArr = obj._getDropdownItemsActivity();
                var maxLength = obj._maxLengthValue ? obj._maxLengthValue : 255;
                
                for (var i = itms.length - 1; i >= 0; i--) {
                    var isVisable = itmVisibleArr[i];
                    InsertSTDandXPolicyItem(itms[i], 'container', isVisable, maxLength);
                }

                var tdHtml = "";
                if (Ext.Const.IsSupportMultiLang) {
                    tdHtml = '<td><input type="text" style="width:110px;font-weight:bold;" disabled="disabled" value="Select All" /></td>' +
                             '<td><input type="text" style="width:110px;font-weight:bold;" disabled="disabled" value="Select All" /></td>';
                }
                else {
                    tdHtml = '<td><span style="font-weight:bold;">Select All</span></td>';
                }
                
                Ext.DomHelper.insertFirst('container', {
                    tag: 'div',
                    cls: 'dropblock_All',
                    html: '<table><tr><td><input id="SelectAllCheckBox" type="checkbox" onclick="selectAllClick(this);" /></td>' + tdHtml + '</tr></table>'
                }, true);

                UnCheckSelectAllOption();
            } else {
                Ext.DomHelper.insertFirst('container', {
                    tag: 'div',
                    html: '<table><tr><th scope="col"><label>No data found.</label></th></tr></table>'
                }, true);
            }
            break;
        default:
            return;
    }
};

//this function is insert the dragdrop itm into form, the data is from STDandXPolicy
//params:
//          item:       the standard choice item which will be display
//          itemName:   the container's name
function InsertSTDandXPolicyItem(item, itemName, itmVisible, maxLength) {
    var defaultValue = FilterSpecialChar(item.value);
    var context = FilterSpecialChar(item.resValue);

    var newDragItem;

    var check;
    var addedChkHtml = '';

    // the control is contact type.
    if (saveobj != null && saveobj._getType() == 20) {
        //if item is activated.
        if (itmVisible) {
            addedChkHtml = '<td><input type="checkbox" onclick="DisabledDropDownButtons(false);" checked/></td>';
        }
        else {
            addedChkHtml = '<td><input type="checkbox" onclick="DisabledDropDownButtons(false);"/></td>';
        }

        //filter "-"
        if (defaultValue != null) {
            var strs = defaultValue.split('||');

            if (strs.length == 2) {
                defaultValue = strs[1];
            }
        }
    }

    var tableBegin = '<table><tr>';
    var tdHtml = '';

    if (Ext.Const.IsSupportMultiLang) {
        tdHtml = addedChkHtml +
                '<td><input type="text" style="width:110px" disabled="disabled" value="' + defaultValue + '" /></td>' +
                '<td><input type="text" disabled="disabled" maxlength="' + maxLength + '" style="width:110px" value="' + context + '" onpropertychange="DisableOkButtonByValue(this);" oninput="DisableOkButtonByValue(this);"/></td>';
    }

    else {
        tdHtml = addedChkHtml +
                    '<td><span>' + defaultValue + '</span></td>';
    }

    var html = tableBegin + tdHtml + "</tr></table>";

    var newDragItem = Ext.DomHelper.insertFirst(itemName, {
        tag: 'div',
        cls: 'dropblock',
        html: html
    }, true);

};

// judge it is drop down ExtenseItems, e.g. licensing board
function isExtenseItems() {
    var $extenseType = $("#hidExtenseType");

    if ($extenseType.length > 0 && $extenseType.val()) {
        return true;
    }
    
    return false;
}

function isExistBizDomainValue()
{
    if(!needSave)
    {
        return;
    }

    if(saveobj != null && saveobj._id == currentSelectedDropdownlistId)
    {
        var defaultValues = new Array();
        var index = 0;
        var items = saveobj._getDropdownItems();
        
        // ExtenseItems
        if (isExtenseItems()) {
            saveList();
            return;
        }

        dragEls = Ext.get('container').query('.dropblock');

        for (var i = 0; i < dragEls.length; i++)
        {
            var checkobj = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[0];
            
            if((checkobj != null && checkobj.childNodes[0].type != 'checkbox'))
            {
                    var isExitItem = false;
                    var objIndex = 0; 
                    
                    if (!(checkobj != null && checkobj.childNodes[0].type != 'checkbox') && saveobj._getType() == 20)
                    {
                        objIndex = 1;
                    }
                    
                    curDefaultValue = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[objIndex].childNodes[0].value.trim();

                    if (Ext.Const.IsSupportMultiLang && IsDefaultLanguage())
                    {
                        objIndex = 1; 
                        
                        if (!(checkobj != null && checkobj.childNodes[0].type != 'checkbox') && saveobj._getType() == 20)
                        {
                            objIndex = 2;
                        }
                      
                        var resValueObj=dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[objIndex];
                        
                        if(resValueObj != null)
                        {
                           curDefaultValue=dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[objIndex].childNodes[0].value.trim();
                        }
                    }
                    
                    if (items != null && items.Items != null && items.Items.length > 0)
                    {
                        for (var j = 0; j < items.Items.length; j++)
                        {
                            var itemValue = items.Items[j].value;
                            
                            if (items.Items[j].value.indexOf("||") != -1)
                            {
                                itemValue = items.Items[j].value.substr(items.Items[j].value.indexOf("||")+2);
                            }
                            
                            if (curDefaultValue.trim() == itemValue.trim())    
                            {
                                isExitItem = true;
                                break;
                            }

                            if ((curDefaultValue.trim().toUpperCase() == itemValue.trim().toUpperCase())) {
                                var msg = Ext.LabelKey.Admin_FieldProperty_ConflictedItem;
                                msg = msg.format(curDefaultValue, curDefaultValue, saveobj._stdCategoryValue);
                                ValidateTextfield(true, msg);
                                return;
                            }
                        }
                    }
                    
                    if (!isExitItem) {
                        defaultValues[index] = curDefaultValue;
                        index++;
                    }
            }
            else    // hard code
            {
                saveList();
                return;
            }
        }

        if (defaultValues != null && defaultValues.length > 0) {
            var duplicateIndex = defaultValues.duplicateIndexOf(true);
            
            if (duplicateIndex > -1) {
                var errorMsg = Ext.LabelKey.Admin_FieldProperty_ConflictedItem;
                errorMsg = errorMsg.format(defaultValues[duplicateIndex], defaultValues[duplicateIndex], saveobj._stdCategoryValue);
                ValidateTextfield(true, errorMsg);
                return;
            }

            Accela.ACA.Web.WebService.AdminConfigureService.IsExistBizDomainValue(saveobj._stdCategoryValue, defaultValues, callSaveFunction);
        }
        else
        {
           saveList();
        }
    }    
    else
    {
        saveList();
    }
}

  function callSaveFunction(response) {
    if (response == null || response == "")
    {
        saveList();
    }
    else
    {
        ValidateTextfield(true, response);
    }
  }

  //Get extense object
  function GetExtenseObject() {
      var extenseEls = Ext.get('container').query('.Content_Option');

      if (extenseEls.length > 0) {
          var extenseObj = new ExtenseObj();
          
          // extense type.e.g. ACA_LICENSING_BOARD
          var extenseType = $("#hidExtenseType").val();
          extenseObj.extenseType = extenseType;
          extenseObj.extenseItems = new Array();
          
          var entityId = Ext.Const.ModuleName;
          if (entityId == "") {
              entityId = GLOBAL_AGENCY_CODE;
          }
          
          //get Extense Items for licensing board
          if (extenseType == EXTENSE_TYPE_LICENSING_BOARD) {
              var extensesKey = $("#hidExtensesKey").val();
              var extenseItem = new ExtenseItem(extensesKey);

              var deleteOption = { entityId: entityId, entityId2: extensesKey };
              extenseItem.deleteOption = deleteOption;

              var updateOptions = new Array();

              for (var index = 1; index < extenseEls.length; index++) {
                  var checkObj = $(extenseEls[index]).find("input[type='checkbox']");

                  var perValue = "N";
                  if ($(checkObj).attr("checked")) {
                      perValue = "Y";
                  }

                  var updateOption = {
                      entityId: entityId,
                      entityId2: extensesKey,
                      entityId3: $(checkObj).val(),
                      resEntityId3: $(checkObj).val(),
                      value: perValue
                  };

                  updateOptions[index - 1] = updateOption;
              }

              extenseItem.updateOptions = updateOptions;
              extenseObj.extenseItems[0] = extenseItem;
          } else if (extenseType == EXTENSE_TYPE_CONTACT_TYPE_EDITABLE) {
              var extenseItems = new ExtenseItem("");
              var updateItems = new Array();
              var deleteItem = {entityId: entityId, entityType: extenseType };

              extenseItems.deleteOption = deleteItem;

              for (var indexNum = 0; indexNum < extenseEls.length; indexNum++) {
                  var editableObj = $(extenseEls[indexNum]).find("input[type='checkbox']");

                  var chkValue = "N";
                  if ($(editableObj).attr("checked")) {
                      chkValue = "Y";
                  }
                  
                  var updateItem = {
                      entityId: entityId,
                      entityId2: $(editableObj).val(),
                      value: chkValue
                  };

                  updateItems[indexNum] = updateItem;
              }
              
              extenseItems.updateOptions = updateItems;
              extenseObj.extenseItems[0] = extenseItems;
          }

          return extenseObj;
      }

      return null;
  }

  // update Extense Objects[{Type,ExtenseItems}]
  function UpdateExtenseObjects(existExtenses, newExtenses) {
      for (var i = 0; i < newExtenses.length; i++) {
          var newExtenseObj = newExtenses[i];
          var isExistExtenseObj = false;

          for (var j = 0; j < existExtenses.length; j++) {
              var existExtenseObj = existExtenses[j];

              if (existExtenseObj.extenseType == newExtenseObj.extenseType) {
                  UpdateExtenseItems(existExtenseObj.extenseItems, newExtenseObj.extenseItems);
                  isExistExtenseObj = true;
                  break;
              }
          }
          
          if (!isExistExtenseObj) {
              existExtenses[existExtenses.length] = newExtenseObj;
          }
      }
  }

  // update Extense Items [{Key,Item}]
  function UpdateExtenseItems(existExtenseItems, newExtenseItems) {
      for (var i = 0; i < newExtenseItems.length; i++) {
          var newExtenseItem = newExtenseItems[i];
          var isExistExtenseItem = false;

          for (var j = 0; j < existExtenseItems.length; j++) {
              if (existExtenseItems[j].extensesKey == newExtenseItem.extensesKey) {
                  existExtenseItems[j] = newExtenseItem;
                  isExistExtenseItem = true;
                  break;
              }
          }

          if (!isExistExtenseItem) {
              existExtenseItems[existExtenseItems.length] = newExtenseItem;
          }
      }
  }

  function UpdateItem4ExtenseObjects() {
      var newExtense = GetExtenseObject();

      if (newExtense != null) {
          var newExtenses = new Array();
          newExtenses[0] = newExtense;
          UpdateExtenseObjects(updateExtenseObjects, newExtenses);
      }
  }
  
// this function is save the changed items
// params:
//      saveobj:    the webcontrol which will be saved
  function saveList() {
      isDDLWinShow = false;
      if (!needSave) {
          return;
      }

      // if it is extense items, e.g. licensing board
      if (isExtenseItems()) {
          //get Extense Items for licensing board
          if (saveobj != null && saveobj._id == currentSelectedDropdownlistId && $("#hidExtenseType").val() == EXTENSE_TYPE_LICENSING_BOARD) {
              if (IsTrue($("#hidChangeLicesingBoardStatus").val())) {
                  UpdateItem4ExtenseObjects();
              }

              var updateItems = new DropDownObj(Ext.Const.OpenedId, saveobj._element.id, droptype, saveobj._labelKeyValue, saveobj._getLabel(), saveobj.get_IsTemplateField(), saveobj.get_TemplateAttribute(), saveobj._labelKeyValue + '|sub', saveobj._getSubLabel(), saveobj._stdCategoryValue);
              updateItems.ExtenseObjects = updateExtenseObjects;
              changeItems.UpdateItem(changeItems.ItemType.arrDropDown, updateItems);
          }
          else if($("#hidExtenseType").val() == EXTENSE_TYPE_CONTACT_TYPE_EDITABLE) {
              //get Extense Items for contact type editable.
              var extenseObjects = new Array();
              extenseObjects[0] = GetExtenseObject();
              
              var updateOptions = {};
              updateOptions.ExtenseObjects = extenseObjects;
              updateOptions.PageId = Ext.Const.OpenedId;
              
              if (updateOptions != null) {
                  changeItems.UpdateItem(changeItems.ItemType.arrExtense, updateOptions);
              }
          }
      }
      else if (saveobj != null && saveobj._id == currentSelectedDropdownlistId) {
          var items = new Array();
          var showItems = new Array(); // items which will be used to show in the dropdownlist
          var itemArray = new Array(); // save temp item in the dropdownlist.
          var arrVisible = new Array();
          var changeitms = new Array();
          var descriptionArray = [];
          var dragEls;

          var defaultValue = '';
          var resValue = '';
          var description = '';
          var resDescripton = '';
          var resValueObj;
          var blockSave = false; //block current save process.
          var isSecurityQuestionsSC = false;

          dragEls = Ext.get('container').query('.dropblock');

          for (var i = 0; i < dragEls.length; i++) {
              var checkobj = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[0];
              var td2Obj = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[1];
              if (checkobj != null && checkobj.childNodes[0].type != 'checkbox') {
                  var duplicateValueMsg = isSecurityQuestionsSC ? Ext.LabelKey.Admin_SecurityQuestionsEdit_DuplicateValue : '';

                  if (Ext.Const.IsSupportMultiLang) {
                      resValueObj = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[1];
                      defaultValue = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[0].childNodes[0].value;

                      if (resValueObj != null) {
                          resValue = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[1].childNodes[0].value;
                      }

                      blockSave = VerifyItemVal(resValue, itemArray, i, duplicateValueMsg);

                      if (showType == 1 || showType == 2) {
                          description = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[2].childNodes[0].value;
                          resDescripton = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[3].childNodes[0].value;
                      }

                      defaultValue = defaultValue == '' ? resValue : defaultValue; // add a new choice,set the default value as the added value
                      description = description == '' ? resDescripton : description;

                      items[i] = showType + "||" + defaultValue + '||' + resValue + '||' + description + '||' + resDescripton;

                      showItems[i] = (showType == 1 || showType == 2) ?
                          defaultValue + '||' + resValue + '||' + description + '||' + resDescripton : defaultValue + '||' + resValue;
                  } else {
                      defaultValue = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[0].childNodes[0].value;

                      blockSave = VerifyItemVal(defaultValue, itemArray, i, duplicateValueMsg);

                      if (showType == 1 || showType == 2) {
                          description = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[1].childNodes[0].value;
                      }

                      items[i] = showType + "||" + defaultValue + '||' + description;
                      showItems[i] = (showType == 1 || showType == 2) ?
                          defaultValue + '||' + defaultValue + '||' + description + '||' + description : defaultValue + "||" + defaultValue;

                  }

                  if ((showType == 1 || showType == 2) && isSecurityQuestionsSC) {
                      var desc = Ext.Const.IsSupportMultiLang ? resDescripton : description;
                      if (blockSave || !ValidateSecurityQuestionsDescription(desc, descriptionArray)) {
                          return;
                      }

                      descriptionArray.push(desc.trim());
                  }
              } else if (saveobj._getType() == 20)// the control is contact type.
              {
                  if (Ext.Const.IsSupportMultiLang) {
                      resValueObj = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[2];
                      if (resValueObj != null) {
                          resValue = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[2].childNodes[0].value;
                      }

                      blockSave = VerifyItemVal(resValue, itemArray, i);

                      defaultValue = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[1].childNodes[0].value;

                      if (defaultValue == '')// if add a new choice
                      {
                          defaultValue = resValue;
                          dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[1].childNodes[0].value = resValue;
                      }

                      items[i] = showType + "||" + defaultValue + '||' + resValue + '||' + '||' + saveobj._stdCategoryValue + "||"; // Build data format: showtype || defaultValue || res Value || description || resDescrition.
                      showItems[i] = defaultValue + '||' + resValue;
                  } else {
                      defaultValue = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[1].childNodes[0].innerText;

                      blockSave = VerifyItemVal(defaultValue, itemArray, i);

                      //description = defaultValue;
                      items[i] = showType + "||" + defaultValue + "||" + saveobj._stdCategoryValue + "||";
                      showItems[i] = defaultValue + "||" + defaultValue;
                  }
              } else    // hard code
              {
                  if (Ext.Const.IsSupportMultiLang) {
                      resValueObj = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[2];
                      defaultValue = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[1].childNodes[0].value;

                      if (resValueObj != null) {
                          resValue = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[2].childNodes[0].value;
                      }

                      if (defaultValue == '')         // add a new choice
                      {
                          defaultValue = resValue;
                          dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[1].childNodes[0].value = resValue;
                      }

                      items[i] = defaultValue + '||' + resValue;
                      showItems[i] = resValue == '' ? defaultValue : resValue;

                      if (saveobj._stdCategoryValue == 'EDUCATION_LOOKUP' && saveobj._element != null && saveobj._element.length > 0) {
                          items[i] = items[i] + '||' + GetIndexString(saveobj._element[i].value);
                      }
                  } else {
                      items[i] = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[1].childNodes[0].innerHTML;
                      showItems[i] = items[i];

                      if (saveobj._stdCategoryValue == 'EDUCATION_LOOKUP' && saveobj._element != null && saveobj._element.length > 0) {
                          items[i] = items[i] + '||' + GetIndexString(saveobj._element[i].value);
                      }
                  }
              }
              var isVisible = true;
              if (checkobj != null && checkobj.childNodes[0].type == 'checkbox') {
                  var check = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[0].childNodes[0].checked;
                  if (check) {
                      isVisible = true;
                  } else {
                      isVisible = false;
                  }
                  arrVisible[i] = isVisible;
              }
              changeitms[i] = new ItemObj('', items[i], isVisible);

              if (blockSave == true) {
                  return;
              }
          }

          if (saveobj.type == 21) {
              saveobj._setRadioButtonItems(showItems, showType);
          } else {
              saveobj._setDropdownItems(showItems, showType);
          }

          // the control is contact type.
          if (saveobj != null && saveobj._getType() == 20 && arrVisible.length > 0) {
              saveobj._setDropdownItemsActivity(arrVisible);
          } else if (saveobj._sourceTypeValue == 2 && arrVisible.length > 0) {
              saveobj._setDropdownItemsActivity(arrVisible);
              //saveobj._setDropdownItems(showItems);       // to support I18N,hard code choices can be changed 
          } else if (saveobj.type == 21) {
              saveobj._setRadioButtonActivity(arrVisible);
              saveobj._stdCategoryValue = Ext.Constant.CAP_PAYMENT_TYPE;
          }

          var changeItem = new DropDownObj(Ext.Const.OpenedId, saveobj._element.id, droptype, saveobj._labelKeyValue, saveobj._getLabel(), saveobj.get_IsTemplateField(), saveobj.get_TemplateAttribute(), saveobj._labelKeyValue + '|sub', saveobj._getSubLabel(), saveobj._stdCategoryValue, changeitms);
          changeItems.UpdateItem(3, changeItem);
      }

      newFormWin.hide();
      ModifyMark();
  };

  function ValidateSecurityQuestionsDescription(description, descriptionArray) {
      var errorMsg;

      if (isNullOrEmpty(description)) {
          errorMsg = Ext.LabelKey.Admin_SecurityQuestionsEdit_EmptyDescription;
      }
      else if (descriptionArray.indexOf(description.trim()) != -1) {
          errorMsg = Ext.LabelKey.Admin_SecurityQuestionsEdit_DuplicateDescription;
      }

      if (!isNullOrEmpty(errorMsg)) {
          ValidateTextfield(true, errorMsg);
          return false;
      }

      return true;
  }

//get index data, eg:0,1...; it from xpolicy.data4.
function GetIndexString(value)
{
    var indexStr = '';
    var strArray = value.split('||');
     
    if (strArray != null && strArray.length > 0)
    {
        indexStr = strArray[0].replace('-','');
    }
    
    return indexStr;
}

function GetDropDownItems()
{
    var items = new Array();

    var dragEls = Ext.get('container').query('.dropblock');
    
    if(dragEls.length == 0)  // user just changes the labels.the drop down list window has not been opend. 
    {
       return null;
    }
    
    for(var i = 0; i < dragEls.length; i++){
        var val = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[0].childNodes[0].value;
        if(val == null)
        {
            var val = dragEls[i].childNodes[0].childNodes[0].childNodes[0].childNodes[0].childNodes[0].innerText;
        }
        var isVisible = true;
        items[i] = new ItemObj('',val,isVisible);
    }
    return items;
}

function VerifyItemVal(val, itemArray, index, msg) {
    var blockSave = false;

    val = val.trim();

    if (val == '')//the field value is empty.
    {
        ValidateTextfield(true, Ext.LabelKey.admin_dropdownlist_nullvalue_errormes); //DisabledDropDownButtons(true);
        blockSave = true;
    }

    for (var j = 0; j < itemArray.length; j++) {
        if (itemArray[j] == val)//exist same text, it should display error.
        {
            var duplicateMsg = isNullOrEmpty(msg) ? Ext.LabelKey.admin_dropdownlist_existitem_errormes : msg;
            ValidateTextfield(true, duplicateMsg); //DisabledDropDownButtons(true);
            blockSave = true;
        }
    }

    itemArray[index] = val; // not-exist same text and not empty, it add val to item array.

    return blockSave;
}

//this function is insert a add button into form
function InsertAddBtn(containerName)
{
    var btn = Ext.DomHelper.insertFirst(containerName,{
        id:'btn',
        tag:'div',
        html:   '<table><tr>' +
                '<td><img src="images/add.gif" id="addbtn" onclick="javascript:InsertEl(arguments[0] || window.event);DisabledDropDownButtons(false);"></td>' +
                '<td><span>' + Ext.LabelKey.Admin_FieldProperty_AddButton + '</span></td></tr></table>'
        },true);
}

// insert value label into form
function InsertLabel(containerName) {
    if (Ext.Const.IsSupportMultiLang) {
        if (showType == 1 || showType == 2) {
            var label = Ext.DomHelper.insertFirst(containerName, {
                id: 'label',
                tag: 'div',
                html: '<table><tr>' +
                    '<td style="width:135px;">  Value(Default Language)</td>' + '<td style="width:110px;">Value</td>' +
                    '<td style="width:115px;">Description(Default Language)</td>' + '<td>Description</td></tr></table>'
            }, true);
        } else {
            var label = Ext.DomHelper.insertFirst(containerName, {
                id: 'label',
                tag: 'div',
                html: '<table><tr>' +
                    '<td style="width:150px;">Value(Default Language)</td>' + '<td>Value</td></tr></table>'
            }, true);
        }
    } else {
        var label = Ext.DomHelper.insertFirst(containerName, {
            id: 'label',
            tag: 'div',
            html: '<table><tr>' +
                '<td style="width:150px;">Value</td>' + '<td>Description</td></tr></table>'
        }, true);
    }
}

//this function is insert the dragdrop itm into form, the data is from standard choice
//params:
//          item:       the standard choice item which will be display
//          itemName:   the container's name
//          insertType: 0 insert at first; 1 insert after the specified item
//          isSelected: is selected or not
function InsertSCItem(item, itemName, itmVisible, maxLength)
{
    var defaultValue = FilterSpecialChar(item.value);
    var context = FilterSpecialChar(item.resValue);

    var newDragItem;
    
    var check;
    var addedChkHtml = '';
    
    // the control is contact type.
	if (saveobj != null && saveobj._getType() == 20)
    {
        //if item is activated.
        if(itmVisible)
        {
           addedChkHtml = '<td><input type="checkbox" onclick="DisabledDropDownButtons(false);" checked/></td>';
        }
        else
        {
            addedChkHtml = '<td><input type="checkbox" onclick="DisabledDropDownButtons(false);"/></td>';
        }
        
        //filter "-"
        if (defaultValue != null)
        {
            var strs = defaultValue.split('||');
            
            if (strs.length == 2)
            {
                defaultValue = strs[1];
            }
        }
    }

    var tableBegin = '<table><tr>';
    var tdHtml = '';
    
    if(Ext.Const.IsSupportMultiLang)
    {
       if(showType == 1 || showType == 2)
       {
           tdHtml = '<td><input type="text" style="width:110px" disabled="disabled" value="' + defaultValue + '" /></td>' +
                    '<td><input type="text" maxlength="' + maxLength + '" style="width:110px" value="' + FilterSpecialChar(item.resValue) + '" onpropertychange="DisableOkButtonByValue(this);" oninput="DisableOkButtonByValue(this);"/></td>' +
                    '<td><input type="text" style="width:110px" disabled="disabled" value="' + FilterSpecialChar(item.description) + '" /></td>' +
                    '<td><input type="text" style="width:110px" value="' + FilterSpecialChar(item.resDescription) + '" onpropertychange="DisableOkButtonByValue(this);" oninput="DisableOkButtonByValue(this);"/></td>';
       }
       else
       {
           tdHtml = addedChkHtml +
                    '<td><input type="text" style="width:110px" disabled="disabled" value="' + defaultValue + '" /></td>' +
                    '<td><input type="text" maxlength="' + maxLength + '" style="width:110px" value="' + context + '" onpropertychange="DisableOkButtonByValue(this);" oninput="DisableOkButtonByValue(this);"/></td>';
       }
    }
    
    else
    {
        if(showType == 1 || showType == 2)
        {
            tdHtml = '<td><input type="text" maxlength="' + maxLength + '" style="width:110px" value="' + defaultValue + '" onpropertychange="DisableOkButtonByValue(this);" oninput="DisableOkButtonByValue(this);"/></td>' +
                     '<td><input type="text" style="width:110px" value="' + FilterSpecialChar(item.description) + '" onpropertychange="DisableOkButtonByValue(this);" oninput="DisableOkButtonByValue(this);"/></td>';
        }
        else
        {
            tdHtml = addedChkHtml +
                     '<td><input type="text" maxlength="' + maxLength + '" size="20" value="' + context + '" onpropertychange="DisableOkButtonByValue(this);" oninput="DisableOkButtonByValue(this);"/></td>';
        }
    }

    tdHtml += '<td><img src="images/dec.gif" onclick="javascript:DelEl(arguments[0] || window.event);DisabledDropDownButtons(false);"></td>';
    var html = tableBegin + tdHtml + "</tr></table>";
    
    var newDragItem = Ext.DomHelper.insertFirst(itemName,{
           tag:'div',
           cls:'dropblock',
           html: html
           },true);

};

function FilterSpecialChar(text)
{
   return text.replace(/\u0022/g, "&quot;");
}

//this function is insert the dragdrop itm into form, the data is from DB
//params:
//          Text:       the value which will be display on the itm
//          itemName:   the container's name
//          isSelected: is selected or not
function InsertDBItem(containerName,items,itemVisibleArr)
{
    Ext.MessageBox.show({
               msg: 'Saving your data, please wait...',
               progressText: 'Saving...',
               width:300,
               wait:true,
               waitConfig: {interval:200},
               icon:'ext-mb-download', //custom class in msg-box.html
               animEl: 'mb7'
           });

    var newDragItem = Ext.DomHelper.append(containerName,{
        tag:'div',
        cls:'itemblock',
        html:   GetDDLItemsHTML(items,itemVisibleArr)
        },true);
    Ext.MessageBox.hide();

};

//this function is insert the dragdrop itm into form, the data is from Hardcode
//params:
//          Text:       the value which will be display on the itm
//          itemName:   the container's name
//          isSelected: is selected or not
function InsertHCItem(Text, itemName, checked) {
    var check;
    var newDragItem;
    if (checked) {
        check = 'checked';
    } else {
        check = '';
    }

    var index = Text.indexOf('||');
    var key = Text.substring(0, index);
    var context = Text.substring(index + 2);

    if (Ext.Const.IsSupportMultiLang) {
        newDragItem = Ext.DomHelper.insertFirst(itemName, {
            tag: 'div',
            cls: 'dropblock',
            html: '<table><tr>' +
                '<td><input type="checkbox"  onclick="DisabledDropDownButtons(false);" ' + check + '/></td>' +
                '<td><input type="text" disabled="disabled" value="' + key + '" /></td>' +
                '<td><input type="text" onpropertychange="DisableOkButtonByValue(this);" oninput="DisableOkButtonByValue(this);" value="' + context + '" style="font-size:10px;" /></td></tr></table>'
        }, true);
    } else {
        newDragItem = Ext.DomHelper.insertFirst(itemName, {
            tag: 'div',
            cls: 'dropblock',
            html: '<table><tr>' +
                '<td><input type="checkbox"  onclick="DisabledDropDownButtons(false);" ' + check + ' /></td>' +
                '<td><span>' + context + '</span></td></tr></table>'
        }, true);
    }

};

//this function is support add item
function InsertEl(e)
{
    var src = e.srcElement || e.target; 
    var newDragItem;
    //get div object
    var objEle = Ext.get('container').query('.dropblock');
    if(objEle.length>0)
    {
        newDragItem = Ext.DomHelper.insertAfter(objEle[objEle.length - 1].id,{
        tag:'div',
        cls: 'dropblock',
        html: GetHtml()  
        },true);
    }
    else
    {
        // if has label, add the item below the label row.
        var lable = document.getElementById('label');
        var labelOrBtn = lable == null ? 'btn': 'label';
        newDragItem = Ext.DomHelper.insertAfter(labelOrBtn,{
        tag:'div',
        cls: 'dropblock',
        html:  GetHtml()
        },true);
    }

    FocusAddedItem();
};

//focus text box in added new item.
function FocusAddedItem()
{
    var objEle = Ext.get('container').query('.dropblock');
        
    if(objEle != null && objEle.length>0)
    {
        var txtBox =  null;
        var index = 0;
            
        if (saveobj != null && saveobj._getType() == 20)
        {
            var index = Ext.Const.IsSupportMultiLang ? 2 : 1;
            txtBox = objEle[objEle.length-1].childNodes[0].childNodes[0].childNodes[0].childNodes[index].childNodes[0];
        }
        else
        {
            index = Ext.Const.IsSupportMultiLang ? 1 : 0;
            txtBox = objEle[objEle.length-1].childNodes[0].childNodes[0].childNodes[0].childNodes[index].childNodes[0];
        }
            
         if (txtBox != null)
         {
            txtBox.focus();
         }
    }
}

function GetHtml()
{
    var html;
    var addedChkHtml = '';
    //the control is contact type.
    if (saveobj != null && saveobj._getType() == 20)
    {
        addedChkHtml = '<td><input type="checkbox" checked onclick="DisabledDropDownButtons(false);" /></td>';
    }

    var maxLength = (saveobj && saveobj._maxLengthValue) ? saveobj._maxLengthValue : 255;
    
    if(Ext.Const.IsSupportMultiLang)
    {
        if(showType == 1 || showType == 2)
        {
            html = '<table><tr>' +
                '<td><input type="text" style="width:110px" disabled="disabled" value=""/></td>' +
                '<td><input type="text" maxlength="' + maxLength + '" style="width:110px" value=""/></td>' +
                '<td><input type="text" style="width:110px" disabled="disabled" value=""/></td>' +
                '<td><input type="text" style="width:110px" value=""/></td>' +
                '<td><img src="images/dec.gif" onclick="javascript:DelEl(arguments[0] || window.event);"></td></tr></table>';
        }
        else
        {

            html = '<table><tr>' +
                addedChkHtml +
                '<td><input type="text" style="width:110px" disabled="disabled" value=""/></td>' +
                '<td><input type="text" maxlength="' + maxLength + '" style="width:110px" value=""/></td>' +
                '<td><img src="images/dec.gif" onclick="javascript:DelEl(arguments[0] || window.event);"></td></tr></table>';
        }
    }
    else
    {
        if(showType == 1 || showType == 2)
        {
            html = '<table><tr>' +
                '<td><input type="text" maxlength="' + maxLength + '" style="width:110px" value=""/></td>' +
                '<td><input type="text" style="width:110px" value=""/></td>' +
                '<td><img src="images/dec.gif" onclick="javascript:DelEl(arguments[0] || window.event);"></td></tr></table>';
        }
        else
        {
            html = '<table><tr>' +
                addedChkHtml +
                '<td><input type="text" maxlength="' + maxLength + '" size="20" value=""/></td>' +
                '<td><img src="images/dec.gif" onclick="javascript:DelEl(arguments[0] || window.event);"></td></tr></table>';
        }
   }
    
    return html;
}

//this function is support remove item
function DelEl(e)
{
    var src = e.srcElement || e.target; 
    var objEle = src.parentNode.parentNode.parentNode.parentNode.parentNode;
    objEle.parentNode.removeChild(objEle);
};

//this function is support DB ddl's item change event
function ChangeSelected()
{
    if(saveobj._id == currentSelectedDropdownlistId)
    {
        var divDBItems = document.getElementById('divDBItems');
        var itmsVisible = new Array();
        var changeitms = new Array();
        
        if(divDBItems != null)
        {
            var items = saveobj._getDropdownItems();
            for(var i=0;i<divDBItems.childNodes[0].childNodes[0].childNodes[0].childNodes.length;i++)
            {
                itmsVisible[i] = divDBItems.childNodes[0].childNodes[0].childNodes[0].childNodes[i].childNodes[0].childNodes[0].checked;
                changeitms[i] = new ItemObj('',items[i].split('||')[0],itmsVisible[i]);                 
            }
        }

        saveobj._setDropdownItemsActivity(itmsVisible);
        var changeItem = new DropDownObj(Ext.Const.OpenedId, saveobj._controlIdValue, droptype, saveobj._labelKeyValue, saveobj._getLabel(), saveobj.get_IsTemplateField(), saveobj.get_TemplateAttribute(), saveobj._labelKeyValue + '|sub', saveobj._getSubLabel(), 'CapTypeConfiguration', changeitms);
        changeItems.UpdateItem(3,changeItem);
        ModifyMark();
    }
}

function GetDDLItemsHTML(itms, itmVisibles) {
    var strTableRows = '';
    var strTableStart = '<table>';
    var strTableEnd = '</table>';

    for (var i = 0; i < itms.length; i++) {
        var keyvalue = itms[i].split('||');
        if (keyvalue.length == 1) {
            keyvalue[1] = keyvalue[0];
        }
        var tdLabel = keyvalue[1];
        var strCheckHTML;

        // is it selected or not
        var tdCheck;
        if (itmVisibles[i]) {
            tdCheck = 'checked';
        } else {
            tdCheck = '';
        }

        strCheckHTML = '<td><input type="checkbox" ' + tdCheck +
            ' onclick="javascript:ChangeSelected();"/></td>';
        // is it required or not

        strTableRows = strTableRows + '<tr>' + strCheckHTML +
            '<td><span>' + tdLabel + '</span></td>' +
            '</tr>';

        var percent = i / itms.length;
        Ext.MessageBox.updateProgress(percent, Math.round(100 * percent) + '% completed');
    }
    return strTableStart + strTableRows + strTableEnd;
}

// set the OK Button disabled or not.
function DisabledDropDownButtons(isDisabled) {
    UnCheckSelectAllOption();
    var btnOK=Ext.getCmp("btnDropDownOK");
    
    if(!isDisabled)
    {
        btnOK.enable();
    }
    else
    {
        btnOK.disable();
    }
}

// set the OK Button disabled or not.
function DisableOkButtonByValue(obj) 
{
    var btnOK=Ext.getCmp("btnDropDownOK");
    
    if(obj.defaultValue != obj.value)
    {
        btnOK.enable();
    }
}

function ValidateTextfield(showFlag, errorMsg)
{
    if(showFlag)
    {
        Ext.Msg.alert("", errorMsg);
    }
}

function selectAllClick(thisObj) {
    var dragEls = $("#container .dropblock input[type=checkbox]");
    SelectAll(thisObj, dragEls);
    DisabledDropDownButtons(false);
}

function UnCheckSelectAllOption() {
    var selectAllObj = $("#SelectAllCheckBox");
    var selectItems = $("#container .dropblock input[type=checkbox]");
    UpdateStatus4SelectAll(selectAllObj, selectItems);
}