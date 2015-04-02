/**
* <pre>
* 
*  Accela Citizen Access
*  File: sectionProperties.js
* 
*  Accela, Inc.
*  Copyright (C): 2007-2014
* 
*  Description:
* 
*  Notes:
* $Id: sectionProperties.js 72643 2007-07-10 21:52:06Z weiky.chen $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

var changeItem;
var sectionFields;
var IsRequiredConfigured;
var CurrentTab;
var isPermitDetailSection = false;
var getGridView;
var silverlightWin;
var MultipleTypesSectionFields = new Array();
var EXTENSE_TYPE_CONTACT_TYPE_EDITABLE = "ACA_REFERENCE_CONTACT_EDITABLE_BY_CONTACT_TYPE";

function GetSectionFields(obj, newTab, isRequiredConfigured) {
    var sectionArg = GetSectionArgument(obj);
    IsRequiredConfigured = isRequiredConfigured;
    isPermitDetailSection = false;
    if (obj.type != 16) {
        CloseWindow();
    }
    var strFields = '';

    if (sectionArg.permissionLevel == Ext.Const.People || sectionArg.permissionLevel == Ext.Const.Attachment) {
        //obj._sectionFields = null;
        var fields = GetLPSectionFields(sectionArg);
        obj._sectionFields = fields;

    }
    
    var sectionId = obj._sectionIdValue.split(Ext.Const.SplitChar);
    if ((sectionId.length != 3 && sectionId.length != 4) || !sectionId[1]) //no view id, just need load property grid
    {
        LoadSectionProperty(obj, newTab);
        return;
    }
    
    if (obj._sectionFields) {
        HandleSectionFields(obj, newTab);
    }
    else {
        if (!obj.InProgressOfGetSectionFields) {
            obj.InProgressOfGetSectionFields = true;

            PageMethods.GetSectionFields(obj._sectionIdValue, sectionArg.permissionLevel, sectionArg.permissionValue, SetSectionFields, FailedToGetSectionFields, obj);
        }
    }
}

function SetSectionFields(result, obj) {
    delete obj.InProgressOfGetSectionFields;

    if (result) {
        var json = eval('(' + result + ')');
        for (var i = 0; i < json.length; i++) {
            json[i].Visible = Boolean.parse(json[i].Visible);
            json[i].Required = Boolean.parse(json[i].Required);
            json[i].Editable = Boolean.parse(json[i].Editable);
            //Hide inactive fields when switch hard code drop down item.
            if (obj.type == 16 && !json[i].Visible) {
                SetFieldVisible(null, json[i].ElementName, false, null);
            }
        }
        obj._sectionFields = json;
        var sectionArg = GetSectionArgument(obj);
        if (sectionArg.permissionLevel == Ext.Const.People || sectionArg.permissionLevel == Ext.Const.Attachment) {
            SetLPSectionFields(sectionArg, json);
        }

        HandleSectionFields(obj);
    }
}

function FailedToGetSectionFields(errObj, obj) {
    delete obj.InProgressOfGetSectionFields;

    if (errObj) {
        ShowErrorMessage(errObj.get_message());
    }
}

function GetDom(activeTab) {
    if (!activeTab) {
        activeTab = Ext.getCmp("tabs").activeTab;
    }

    if (activeTab && activeTab.body) {
        var iframe = activeTab.body.dom.firstChild;
        if (!iframe) {
            return null;
        }
        var dom;
        if (navigator.appName != "Microsoft Internet Explorer") {
            dom = iframe.contentDocument;
        }
        else {
            var iframeId = iframe.id.replace('-', '');
            iframe.id = iframeId;
            dom = eval(iframeId + '.document');
        }

        return dom;
    }

    return null;
}

function GetFrame(activeTab) {
    if (!activeTab) {
        activeTab = Ext.getCmp("tabs").activeTab;
    }

    if (activeTab && activeTab.body) {
        var iframe = activeTab.body.dom.firstChild;
        if (!iframe) {
            return null;
        }

        return iframe;
    }

    return null;
}

function SetFieldVisible(fieldIdPrefix, fieldId, visible, elementId) {
    if (elementId)//for grid view
    {
        var grid = GetDIVElementForCurrentDom(elementId, true);

        if (!grid) {
            return;
        }

        var headers = grid.getElementsByTagName('TH');
        var nodeIndex = Ext.isIE ? 0 : 1;

        for (var k = 0; k < headers.length; k++) {
            var childNodes = headers[k].childNodes;
            for (var i = 0; i < childNodes.length; i++) {
                var node = childNodes[i];

                while (node) {
                    var id = node == null ? null : node.id;

                    if (id != null && id.substr(id.lastIndexOf('_') + 1) == fieldId) {
                        childNodes[i].style.display = visible ? '' : 'none';
                        var th = GetColumnTH(node);
                        if (visible && typeof (th.originalWidth) != 'undefined') {
                            th.style.display = "";
                            var w = th.originalWidth;

                            if (w.indexOf("px") == -1) {
                                w = w.toString() + "px";
                            }
                            $(th).css("width", w);
                        }
                        else {
                            th.originalWidth = th.style.width;
                            th.style.display = "none";
                        }
                        return;
                    }

                    node = Ext.isIE9 || Ext.isIE10 ? node.firstElementChild : node.childNodes[nodeIndex];
                }
            }
        }
    }
    else//for section field
    {
        var div = GetDIVElementForCurrentDom(GetRealID(fieldId), false);

        if (div) {
            div.style.display = visible ? '' : 'none';

            if (isPermitDetailSection) {
                ControlMoreDetailDisplay(fieldIdPrefix, div, visible);
            }
            else {
                SetFieldParentVisible(div, visible);
            }
        }
    }
}

// Control the [More Details] section in permit detail to display or hidden
function ControlMoreDetailDisplay(controlIdPrefix, control, visible) {
    var divMoreDetail = GetDIVElementForCurrentDom(controlIdPrefix + 'tbMoreDetail', false);

    // The "More Details" - Related Contacts, Additional Info, ASI ASIT and Parcel sections contain tables
    if (!divMoreDetail || control.getElementsByTagName('TABLE').length == 0) {
        return;
    }

    control.style.display = visible ? '' : 'none';

    if (!visible) {
        var tables = divMoreDetail.getElementsByTagName('TABLE');

        if (tables) {
            for (var i = 0; i < tables.length; i++) {
                var moreDetailSpan = tables[i].parentNode;

                // has one "More Detail section" span display, show the "More Detail" icon and label.
                if (moreDetailSpan != null &&
                     moreDetailSpan.nodeName == 'SPAN' &&
                     moreDetailSpan.style.display != 'none') {
                    visible = true;
                    break;
                }
            }
        }
    }

    divMoreDetail.style.display = visible ? '' : 'none';
}


function SetFieldParentVisible(element, visible) {
    if (element.id.indexOf("radioListContactPermission") > -1) {
        if (visible) {
            var div = element.parentNode;
            div.style.display = '';
        }
        else {
            var div = element.parentNode;
            div.style.display = 'none';
        }

        return;
    }

    var tr = element.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode;
    if (tr.tagName != "TR")//only one element
    {
        if (element.parentNode.parentNode.parentNode.parentNode.parentNode.tagName == "DIV") {
            //For edit form.
            element.parentNode.parentNode.parentNode.parentNode.parentNode.style.display = visible ? '' : 'none';
        } else {
            //For view form.
            element.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.style.display = visible ? '' : 'none';
        }

        return;
    }
    var needSetParent = true;
    for (var i = 0; i < tr.childNodes.length; i++) {
        var ev;
        if (tr.childNodes[i].childNodes.length == 0) {
            ev = false;
        }
        else {
            try {
                var tempChildNode = GetChildNode(GetChildNode(GetChildNode(tr.childNodes[i])));
                ev = IsVisiable(tempChildNode);
            }
            catch (e) {
                ev = false;
            }
        }

        if (!visible && ev) {
            needSetParent = false;
            break;
        }
    }

    if (needSetParent) {
        var div = tr.parentNode.parentNode.parentNode;
        div.style.display = visible ? '' : 'none';
    }
}

function IsVisiable(obj) {
    var isVisiable = false;

    if (obj == null || obj.childNodes == null || obj.childNodes.length == 0) {
        return isVisiable;
    }

    for (var i = 0; i < obj.childNodes.length; i++) {
        if (obj.childNodes[i].childNodes != null && obj.childNodes[i].childNodes.length > 0) {
            for (var j = 0; j < obj.childNodes[i].childNodes.length; j++) {
                if (obj.childNodes[i].childNodes[j].style != null && obj.childNodes[i].childNodes[j].style.display != 'none') {
                    isVisiable = true;
                    break;
                }
            }
            if (isVisiable) {
                break;
            }
        }
    }

    return isVisiable;
}

function GetRealID(id) {
    return id.replace('Contact1Edit', 'Contact1');
}

function RemoveColon4Label(labelText) {
    var label = labelText.replace(/(\s*$)/g, "");  // trim right space.
    label = labelText.replace(/&nbsp;/g, " ");
    label = label.replace(/:\s*$/, "");
    if (label != '' && label.substr(label.length - 1, 1) == ':') {
        label = label.substr(0, label.length - 1);
    }

    return label;
}

function GetFieldLables(obj, newTab) {
    var dom = GetDom(newTab);

    if (dom) {
        for (var i = 0; i < obj._sectionFields.length; i++) {
            if (obj._sectionFields[i].Standard == "N" || obj._sectionFields[i].ElementType.toLowerCase() == "line") {
                continue;
            };

            var label = dom.getElementById(GetRealID(obj._sectionFields[i].ElementName) + '_label_1');
            if (label == null) {
                label = dom.getElementById(GetRealID(obj._sectionFields[i].ElementName) + '_State1_label_1');
            }

            if (label) {
                obj._sectionFields[i].Label = RemoveColon4Label(label.innerHTML);
            }
            else {
                label = dom.getElementById(obj._sectionFields[i].ElementName);
                if (label && label.tagName == 'INPUT') {
                    obj._sectionFields[i].Label = label.parentNode.lastChild.innerHTML;
                }
                else if (label) {
                    var labelText;
                    if (label.getElementsByTagName('SPAN').length > 0) {
                        labelText = label.getElementsByTagName('SPAN')[0].innerHTML;
                    }
                    else {
                        labelText = label.innerHTML;
                    }

                    // remove colon and html flags.
                    obj._sectionFields[i].Label = RemoveColon4Label(labelText.replace(/<.+?>/gim, ''));
                }
            }
        }
        return true;
    }

    return false;
}

function GetColumnLables(obj) {
    var nodes = null;
    if (Ext.isIE) {
        if (Ext.isIE9 || Ext.isIE10) {
            nodes = obj._element.lastChild.childNodes;
        } else {
            nodes = obj._element.firstChild.childNodes;
        }
    }
    else {
        nodes = obj._element.lastChild.childNodes;
    }
    for (var j = 0; j < obj._sectionFields.length; j++) {
        for (var k = 0; k < nodes.length; k++) {
            var childNodes = nodes[k].childNodes;
            for (var i = 0; i < childNodes.length; i++) {
                if (!childNodes[i]) {
                    continue;
                }
                var node = childNodes[i];
                var id = '';
                while (node) {
                    if (Ext.isIE) {
                        if (id == '' && node.firstChild) {
                            if (Ext.isIE9 || Ext.isIE10) {
                                node = node.firstElementChild;
                                id = node == null ? null : node.id;
                            } else {
                                id = node.firstChild.id;
                                node = node.firstChild;
                            }
                            if (id != '') {
                                break;
                            }
                        }
                        else {
                            node = node.firstChild;
                        }
                    }
                    else {
                        if (id == '' && node.childNodes[1]) {
                            id = node.childNodes[1].id;
                            node = node.childNodes[1];
                            if (id != '') {
                                break;
                            }
                        }
                        else {
                            node = node.childNodes[1];
                        }
                    }
                }
                if (id && id != '') {
                    id = id.split('_');
                    if (id[id.length - 1] == obj._sectionFields[j].ElementName) {
                        if (Ext.isIE) {
                            obj._sectionFields[j].Label = childNodes[i].innerText;
                        }
                        else {
                            obj._sectionFields[j].Label = node.textContent;
                        }
                        break;
                    }
                }
            }
        }
    }
}

function SetChangeItem4Section(obj) {
    var permission = GetSectionArgument(obj);
    changeItem = new SectionObj(Ext.Const.OpenedId, obj._sectionIdValue.split(Ext.Const.SplitChar)[1], obj._labelKeyValue, obj._getLabel(), obj._getSubLabel(), null, permission);
}

//Load property for section
function LoadSectionProperty(obj, newTab) {
    CurrentTab = newTab;
    SetChangeItem4Section(obj);

    if (obj.type != 16) {
        var pgSection = new Ext.grid.PropertyGrid({
            closable: true,
            autoHeight: true,
            id: 'pgSectionProperty',
            customEditors: {
                'Default Label': new Ext.grid.GridEditor(new Ext.form.Field({ readOnly: true })),
                'Admin_FieldProperty_DefaultInstructions': new Ext.grid.GridEditor(new Ext.form.Field({ readOnly: true }))
            },
            listeners: {
                propertychange: function (source, recordId, value, oldValue) {
                    if (recordId == 'Admin_FieldProperty_Licensee_CollapseText') {
                        var chk = true;
                        if (value != '') {
                            var pat = /^\d+$/;
                            chk = pat.test(value);

                            //&& Math.floor(value) != NaN
                            if (!chk) {
                                pgSection.store.getById('Admin_FieldProperty_Licensee_CollapseText').set('value', Math.floor(value));
                            }
                        }

                        if (chk && typeof oldValue != 'undefined' && oldValue != value) {
                            var sectionids = obj._sectionIdValue.split(Ext.Const.SplitChar);
                            sectionids[3] = value;
                            var sectionidstring = "";
                            for (var i = 0; i < sectionids.length; i++) {
                                sectionidstring += sectionids[i] + Ext.Const.SplitChar;
                            }
                            obj._sectionIdValue = sectionidstring.substring(0, sectionidstring.length - 1);
                            var changeItem1 = new CollapseLineObj(Ext.Const.OpenedId, obj._id, value);
                            changeItems.UpdateItem(12, changeItem1);
                            ModifyMark();
                        }
                    }
                    else if (recordId == 'Admin_SectionProperty_Editable') {
                        if (value != '') {
                            pgSection.store.getById('Admin_SectionProperty_Editable').set('value', value);
                        }

                        if (typeof oldValue != 'undefined' && oldValue != value) {
                            var sectionids = obj._sectionIdValue.split(Ext.Const.SplitChar);
                            sectionids[3] = value;
                            var sectionidstring = "";
                            for (var i = 0; i < sectionids.length; i++) {
                                sectionidstring += sectionids[i] + Ext.Const.SplitChar;
                            }
                            obj._sectionIdValue = sectionidstring.substring(0, sectionidstring.length - 1);
                            changeItem = new SectionEditableObj(Ext.Const.OpenedId, obj._id, value);
                            changeItems.UpdateItem(13, changeItem);
                            ModifyMark();
                        }
                    }
                    else {
                        if (oldValue != null && oldValue != '') {
                            if (recordId == 'Admin_SectionProperty_SectionHeading') {
                                SetSectionLabel(obj, EncodeHTMLTag(value));
                            }

                            if (oldValue != value) {
                                changeItem.Label = value;
                                changeItems.UpdateItem(4, changeItem);
                                ModifyMark();
                            }
                        }
                    }
                },
                beforeedit: function (e) {
                    DisabledDropDownButtons(true);
                    var record = e.grid.store.getAt(e.row);
                    var cell = e.grid.getBox();
                    if (record.id == 'Section Instructions') {
                        e.cancel = true;
                        if (!isWinShow) {
                            PopupHtmlEditor(e.grid, obj._getSubLabel());
                            htmlEditorTargetCtrl = obj;
                            htmlEditorTargetType = htmlEditorTargetTypes.Section;
                        }
                    }
                    else if (record.id == 'Admin_SectionProperty_ContactTypeLevel_Editable') {
                        e.cancel = true;
                        //Clear the previous showType value.
                        showType = 0;
                        RenderContactTypeEditableControl();

                        SetFormWinPosition(cell);
                        newFormWin.setTitle('Contact Type Editable');
                        newFormWin.show();
                        isDDLWinShow = true;
                    }
                },
                cellclick: function (grid, rowIndex, columnIndex, e) {
                    DisabledHtmlEditorButtons(true);
                }
            }
        });

        var viewId = obj._sectionIdValue.split(Ext.Const.SplitChar)[1];
        pgSection.customEditors[Ext.LabelKey.Admin_SectionProperty_Editable] = new BoolComboBoxGridEditor();

        pgSection.store.sortInfo = null;
        if (obj._labelKeyValue == "per_licensee_generalinfo_section_title") {
            pgSection.setSource({
                'Default Label': '',
                'Admin_SectionProperty_SectionHeading': '',
                'Section Instructions': Ext.LabelKey.Admin_ClickToEdit_Watermark,
                'Admin_FieldProperty_Licensee_CollapseText': obj._sectionIdValue.split(Ext.Const.SplitChar)[3]
            });
        }
        else if (obj.get_ElementType() == 'AccelaLabel' && obj.get_LabelType() == 41) {
            //FieldSet title
            pgSection.setSource({
                'Default Label': '',
                'Admin_SectionProperty_SectionHeading': ''
            });
        }
        else if (obj.get_ElementType() == 'AccelaLabel' && obj.get_LabelType() == 42) {
            var editable = '';
            var sectionIds = obj._sectionIdValue.split(Ext.Const.SplitChar);

            if (sectionIds.length == 4) {
                editable = sectionIds[3];
            }

            pgSection.setSource({
                'Default Label': '',
                'Admin_SectionProperty_SectionHeading': '',
                'Section Instructions': Ext.LabelKey.Admin_ClickToEdit_Watermark,
                'Admin_SectionProperty_Editable': editable
            });

            pgSection.store.getById('Admin_SectionProperty_Editable').set('name', Ext.LabelKey.Admin_SectionProperty_Editable);
        }
        else if (obj.get_ElementType() == 'AccelaRadioButton' && obj.get_LabelType() == 50) {
            pgSection.setSource({
                'Default Label': '',
                'Admin_SectionProperty_SectionHeading': ''
            });
        }
        else if (viewId == "60142") {
            pgSection.setSource({
                'Default Label': '',
                'Admin_SectionProperty_SectionHeading': '',
                'Section Instructions': Ext.LabelKey.Admin_ClickToEdit_Watermark,
                'Admin_SectionProperty_ContactTypeLevel_Editable': Ext.LabelKey.Admin_ClickToEdit_Watermark
            });

            pgSection.store.getById('Admin_SectionProperty_ContactTypeLevel_Editable').set('name', Ext.LabelKey.Admin_SectionProperty_Editable);
        }
        else {
            pgSection.setSource({
                'Default Label': '',
                'Admin_SectionProperty_SectionHeading': '',
                'Section Instructions': Ext.LabelKey.Admin_ClickToEdit_Watermark
            });
        }

        pgSection.getColumnModel().setConfig([
              { header: 'Name', sortable: false, dataIndex: 'name', id: 'name' },
              { header: 'Value', sortable: false, dataIndex: 'value', id: 'value' }
            ]);

        RenderGrid(1, pgSection);

        needSave = true;
        
        //GetLabelKey
        pgSection.store.getById('Admin_SectionProperty_SectionHeading').set('name', Ext.LabelKey.Admin_SectionProperty_SectionHeading);
        if (obj._labelKeyValue == "per_licensee_generalinfo_section_title") {
            pgSection.store.getById('Admin_FieldProperty_Licensee_CollapseText').set('name', Ext.LabelKey.Admin_FieldProperty_Licensee_CollapseLine);
        }

        //GetValue
        pgSection.store.getById('Default Label').set('value', obj._defaultLabelValue);
        pgSection.store.getById('Admin_SectionProperty_SectionHeading').set('value', GetSectionLabel(obj));
    }
}

function RenderContactTypeEditableControl() {
    var dataSource = "Reference";

    Accela.ACA.Web.WebService.AdminConfigureService.GetAvailableContactTypes(dataSource, function (response) {

        $('#container').html('');

        var contactTypes = eval('(' + response + ')');
        if (contactTypes == "undefined" || contactTypes.length == 0) {
            Ext.DomHelper.insertFirst('container', {
                id: 'btn',
                tag: 'div',
                html: '<table><tr><th scope="col"><label>No contact type found.</label></th></tr></table>'
            }, true);

            return;
        }

        var chkSelectAll = 'checked';
        var columnWidth = Ext.Const.IsSupportMultiLang ? 202 : 115;

        var contactTypeEditableContent = '<div class="Content_SelectAll"><table style="padding-left:5px;"><tr>' +
            '<input type="hidden" id="hidExtenseType" value="' + EXTENSE_TYPE_CONTACT_TYPE_EDITABLE + '"/>' +
            '<th style="word-break:break-all; padding-right: 10px; font-weight:bold; width:' + columnWidth + 'px;"><label for="chkSelectAll"><span>Select All</span></label></th>' +
            '<th style="padding-right: 10px; text-align:right; width:25px;"><input name="chkSelectAll" id="chkSelectAll" onclick="SelectAllContactTypeEditable(this);" value="Select All" type="checkbox"' + chkSelectAll + ' /></th></tr></table>' +
            '<input type="hidden" id="isEditableChkSelectAll"/>';

        Ext.DomHelper.append('container', {
            tag: 'div',
            cls: 'dropblockContactTypeSelectAll',
            html: contactTypeEditableContent
        }, true);

        var contactTypeEditableList = "";
        var updateItems = new ExtenseObj();
        updateItems.PageId = Ext.Const.OpenedId;
        var existItem = changeItems.GetItem(changeItems.ItemType.arrExtense, updateItems);

        if (existItem != null && typeof (existItem.ExtenseObjects) != "undefined" && existItem.ExtenseObjects[0].extenseItems[0].updateOptions.length > 0) {
            var existItems = existItem.ExtenseObjects[0].extenseItems[0].updateOptions;
            var contactType = "";
            var editable = "";
            var existingOptions = new Array();

            for (var index = 0; index < existItems.length; index++) {
                contactType = existItems[index].entityId2;
                editable = existItems[index].value;
                
                var existingOption = { contactType: JsonEncode(contactType), editable: editable };
                existingOptions[index] = existingOption;
            }

            contactTypeEditableList = existingOptions;

            RenderContactTypeEditableData(contactTypes, contactTypeEditableList);
        }
        else {
            Accela.ACA.Web.WebService.AdminConfigureService.GetContactTypeEditableSettings(function (responseData) {
                if (!isNullOrEmpty(responseData)) {
                    contactTypeEditableList = eval('(' + responseData + ')');
                }

                for (var i = 0; i < contactTypes.length; i++) {
                    InsertDataItem(contactTypes[i], 'container', contactTypeEditableList);
                }
            });
        }
    });
}

function RenderContactTypeEditableData(contactTypes, contactTypeEditableList) {    
    for (var i = 0; i < contactTypes.length; i++) {
        InsertDataItem(contactTypes[i], 'container', contactTypeEditableList);
    };
}

function InsertDataItem(contactType, itemName, contactTypeEditableList) {
    var check = 'checked';

    if (contactTypeEditableList.length > 0) {
        for (var i = 0; i < contactTypeEditableList.length; i++) {
            if (contactType.Key == contactTypeEditableList[i].contactType && contactTypeEditableList[i].editable == "N") {
                check = '';
                $('#chkSelectAll').attr("checked", false);
            }
        }
    }

    var dataWidth = Ext.Const.IsSupportMultiLang ? 202 : 115;
    var index = $("input[name^='chkContactTypeEditable']").length;
    var id = "chkContactTypeEditable" + index;
    var newDragItem = Ext.DomHelper.append(itemName, {
        tag: 'div',
        cls: 'dropblockContactType',
        html: '<div class="Content_Option"><table style="padding-left:5px;"><tr>' +
            '<td style="word-break:break-all; padding-right: 10px; width:' + dataWidth + 'px;"><label for="' + id + '"><span>' + contactType.Value + '</span></label></td>' +
            '<td style="padding-right: 10px; text-align:right; width:25px;"><input name="' + id + '" id="' + id + '" onclick="ChangeContactTypeEditableStatus();" value="' + contactType.Key + '" type="checkbox"' + check + ' /></td></tr></table>'
    }, true);
}

function SelectAllContactTypeEditable(obj) {
    var dragEls = Ext.get('container').query('.Content_Option');

    var selectAllchkStatus = '';

    if (obj && obj.checked) {
        selectAllchkStatus = 'checked';
    }

    for (var i = 1; i <= dragEls.length; i++) {
        var checkobj = dragEls[i-1].getElementsByTagName("input")[0];

        if (checkobj != null) {
            checkobj.checked = selectAllchkStatus;
        }
    }

    DisabledDropDownButtons(false);
}

function ChangeContactTypeEditableStatus() {
    if (IsContactTypeEditableSelectAll()) {
        $('#chkSelectAll').attr("checked", true);
    } else {
        $('#chkSelectAll').attr("checked", false);
    }
    
    DisabledDropDownButtons(false);
}

function IsContactTypeEditableSelectAll() {
    var dragEls = Ext.get('container').query('.Content_Option');

    for (var i = 1; i <= dragEls.length; i++) {
        var checkobj = dragEls[i-1].getElementsByTagName("input")[0];

        if (checkobj != null && checkobj.checked == '') {
            return false;
        }
    }

    return true;
}

function HandleSectionFields(obj, newTab) {
    //TODO:handle fields here.
    //obj._sectionFields[i].ElementName : control id
    //obj._sectionFields[i].Label       : control label
    //obj._sectionFields[i].Visible     : control visible true/false
    //obj._sectionFields[i].Required    : control required true/false, only not required field can be set to invisible
    //CAP detail section
    if (obj._sectionIdValue.split(Ext.Const.SplitChar)[1] == '60099') {
        isPermitDetailSection = true;
    }

    if (obj._elementTypeValue == 'AccelaGridView') {
        SetChangeItem4Section(obj);
        GetColumnLables(obj);
        LoadGridViewCol(obj);
        return;
    }
    else {
        if (obj._sectionFields && !GetFieldLables(obj, newTab)) {
            return;
        }
    }

    if (obj.type == 37) {
        GetFieldLables(obj);
    }

    LoadSectionProperty(obj, newTab);

    var showFormDesigner = false;
    var viewId = obj._sectionIdValue.split(Ext.Const.SplitChar)[1];
    if (viewId == '60075' || viewId == '60076' || viewId == '60086' || viewId == '60115' || viewId == '60119' || viewId == '60010' || viewId == '60011' || viewId == '60012' || viewId == '60013' || viewId == '60016' || viewId == '60007') {
        showFormDesigner = true;
    }

    var avaliableLabel;
    if (obj.type != 16) {
        avaliableLabel = Ext.DomHelper.insertAfter('pgSectionProperty', {
            tag: 'div',
            html: '<span >&nbsp;</span>'
        }, true);
    }
    else {
        var avaliableText = Ext.LabelKey.Admin_SectionProperty_AvailableFields;
        if (showFormDesigner) {
            avaliableText = '&nbsp;';
        }

        avaliableLabel = Ext.DomHelper.insertAfter('propsGrid', {
            tag: 'div',
            html: '<span>' + avaliableText + '</span>'
        }, true);
    }

    Ext.DomHelper.insertAfter(avaliableLabel.id, {
        tag: 'div',
        html: '<div id = "divFields" style="WIDTH: 100%; HEIGHT: 100%;overflow-x:auto;overflow-y:auto;"></div>'
    }, true);

    sectionFields = obj._sectionFields;
    if (!IsRequiredConfigured && !showFormDesigner) {
        LoadFieldsList('divFields', obj, 'section');
    }
    else {
        var tempDiv = document.getElementById("divFields");
        tempDiv.style.overflowY = "hidden";
        GeneratePropertyList('divFields', obj, 'section');
    }
}

function getSectionField(elName) {
    var seObj = null;
    for (var i = 0; i < sectionFields.length; i++) {
        if (sectionFields[i].ElementName == elName) {
            seObj = sectionFields[i];
            break;
        }
    }

    return seObj;
}

function sortField(a, b) {
    if (a.order == "" || b.order == "") {
        return 0;
    }

    var order1 = parseInt(a.order);
    var order2 = parseInt(b.order);
    var r = order1 - order2;
    if (r > 0) {
        return 1;
    }
    else if (r == 0) {
        return 0;
    }
    else {
        return -1;
    }
}

function LoadFieldsList(divId, obj, type) {
    // generate records
    getGridView = obj;
    var records = new Array();

    for (var i = 0; i < obj._sectionFields.length; i++) {
        var item = {
            name: obj._sectionFields[i].Label,
            visable: obj._sectionFields[i].Visible,
            Editable:obj._sectionFields[i].Editable,
            disable: obj._sectionFields[i].Required == true ? 'disabled' : '',
            prefix: obj._sectionIdValue.split(Ext.Const.SplitChar)[2],
            elementName: obj._sectionFields[i].ElementName,
            elementId: obj._element.id,
            order: obj._sectionFields[i].Order
        };
        records.push(item);
    }
    // generate dragdrop target.
    records = records.sort(sortField);

    for (var i = records.length - 1; i >= 0; i--) {
        var html = "";
        if (type == 'section') {
            if (records[i].visable == true) {
                html = String.format("<table><tr><td><INPUT TYPE='checkbox' checked {0} onclick=\"javascript:ChangeVisable(\'{1}\',\'{2}\',null,arguments[0] || window.event)\" ></td><td> {3}</td></tr></table>",
                    records[i].disable, records[i].prefix, records[i].elementName, records[i].name);
            }
            else {
                html = String.format("<table><tr><td><INPUT TYPE='checkbox'  {0} onclick=\"javascript:ChangeVisable(\'{1}\',\'{2}\',null,arguments[0] || window.event)\" ></td><td> {3}</td></tr></table> ",
                 records[i].disable, records[i].prefix, records[i].elementName, records[i].name);
            }
        }
        else {
            if (records[i].visable == true) {
                html = String.format("<table><tr><td><INPUT TYPE='checkbox' checked {0} onclick=\"javascript:ChangeVisable(\'{1}\',\'{2}\',\'{3}\',arguments[0] || window.event)\" ></td><td> {4}</td></tr></table>",
                    records[i].disable, records[i].prefix, records[i].elementName, records[i].elementId, records[i].name);
            }
            else {
                html = String.format("<table><tr><td><INPUT TYPE='checkbox'  {0} onclick=\"javascript:ChangeVisable(\'{1}\',\'{2}\',\'{3}\',arguments[0] || window.event)\" ></td><td> {4}</td></tr></table>",
                records[i].disable, records[i].prefix, records[i].elementName, records[i].elementId, records[i].name);
            }
        }

        var dv = Ext.DomHelper.insertAfter(divId, {
            tag: 'div',
            id: 'gvRow' + i,
            title: records[i].name,
            cls: 'dragblock',
            html: html
        }, true);

        dv.data = records[i];
        if (type == "gridview") {
            new AcceleGridView.RowDragDrap(dv, 'gridview', records[i], obj);
        }
    }
}

function renderPropertiesUI(cellValue, cell, currentRowRecord) {
    var returnValue;
    var controlID = currentRowRecord.fields.items[cell.id].name + "-" + currentRowRecord.id;
    var rowIndex=currentRowRecord.id;
    var elementName = currentRowRecord.data.ElementName;
    if (cellValue) {
        returnValue = String.format("<INPUT TYPE='checkbox' checked id='{0}' onclick=\"RangeChecked(this,'{1}','{2}')\">", controlID, rowIndex, elementName);
    }
    else {
        returnValue = String.format("<INPUT TYPE='checkbox' id='{0}'  onclick=\"RangeChecked(this,'{1}','{2}')\">", controlID, rowIndex, elementName);
    }
    return returnValue;
    
}

function RangeChecked(currentCtl, rowIndex, targetControlID) {
    var strVisible = "Visible";
    var strRequired = "Required";
    var strEditable = "Editable";
    var visibleControl = document.getElementById(strVisible + '-' + rowIndex);
    var requiredControl = document.getElementById(strRequired + '-' + rowIndex);
    var editableControl = document.getElementById(strEditable + '-' + rowIndex);
    var targetControl = GetDom(CurrentTab).getElementById(GetRealID(targetControlID));
    if (visibleControl != null && !visibleControl.checked &&
        (targetControlID == "ctl00_PlaceHolderMain_generalSearchForm_txtGSStartDate" ||
         targetControlID == "ctl00_PlaceHolderMain_generalSearchForm_txtGSEndDate")) {
        ShowErrorMessage(Ext.LabelKey.Admin_SectionProperty_RemoveDateRangeFields);
    }

    var prefix = "";
    if (targetControl == null) {
        if (targetControlID.indexOf("txtFinalScore") > -1) {
            //gradingstyle
            prefix = "_txtFinalScore";
        }
        else {
            //state control
            prefix = "_State1";
        }

        targetControl = GetDom(CurrentTab).getElementById(GetRealID(targetControlID + prefix));
    }

    if (targetControl == null) {
        prefix = "";
    }

    if (currentCtl.id.indexOf(strVisible) > -1) {

        var visabled = visibleControl == null ? true : visibleControl.checked;
        if (prefix != "") {
            SetFieldVisible(null, targetControlID + prefix, visabled, null);
        }
        else {
            SetFieldVisible(null, targetControlID, visabled, null);
        }
    }
    if (requiredControl != null) {
        //control checkbox status
        if (visibleControl != null && !visibleControl.checked) {
            requiredControl.checked = false;
            requiredControl.disabled = true;
        }
        else {
            if (targetControl != null && typeof (targetControl.attributes["IsFieldRequired"]) != "undefined") {
                var isFieldRequired = targetControl.attributes["IsFieldRequired"];
                if (isFieldRequired.nodeValue.toLowerCase() == "true") {
                    requiredControl.disabled = true;
                    requiredControl.checked = true;
                }
            }
            else if (currentCtl != null && currentCtl.id != null && currentCtl.id.indexOf("Visible") != -1) {
                requiredControl.disabled = false;
            }
            if (targetControl != null && (targetControl.type == "checkbox" || targetControlID.indexOf("txtAKAName") != -1)) {
                requiredControl.disabled = true;
            }
        }

        if (targetControlID.indexOf("radioListContactPermission") > 0) {
            ControlTheAsterisk(targetControlID + "_radioListContactPermission" + prefix, requiredControl.checked);
        }
        else {
            ControlTheAsterisk(targetControlID + prefix, requiredControl.checked);
        }
    }

    if (editableControl != null) {
        if (visibleControl != null && !visibleControl.checked) {
            editableControl.disabled = true;
            editableControl.checked = false;
        }
        else {
            if (currentCtl.id.indexOf(strVisible) >= 0) {
                editableControl.checked = true;
                editableControl.disabled = false;
            }
        }
    }

    UpdateSectionFieldsData(visibleControl, requiredControl,editableControl, targetControlID);
}


//Generate properties list table for section.	
function GeneratePropertyList(elementID, obj, type) {
    prefixOfSection = obj._sectionIdValue.split(Ext.Const.SplitChar)[2];
    var viewId = obj._sectionIdValue.split(Ext.Const.SplitChar)[1];
    var isDisplayRequired = true;
    
    if (viewId == "60010" || viewId == "60011" || viewId == "60012" || viewId == "60013" || viewId == '60007' || viewId == '60127' || viewId == '60153' || viewId == '60180' || viewId == "60181") {
        isDisplayRequired = false;
    }

	if (!isDisplayRequired) {
	    cm = new Ext.grid.ColumnModel([
		    { id: 'TargetControlID', header: 'TargetControlID', hidden: true, dataIndex: 'ElementName' },
			{ header: 'Field', width: 132, dataIndex: 'Label' },
			{ header: 'Visible', width: 120, renderer: renderPropertiesUI, dataIndex: 'Visible' },
			{ header: 'Required', hidden: true, dataIndex: 'Required' },
            { header: 'Editable', hidden: true, dataIndex: 'Editable' }
		]);
	} else if (viewId == "60142" || viewId == "60154" || viewId == "60159") {
	    cm = new Ext.grid.ColumnModel([
		    { id: 'TargetControlID', header: 'TargetControlID', hidden: true, dataIndex: 'ElementName' },
			{ header: 'Field', width: 112, dataIndex: 'Label' },
			{ header: 'Visible', width: 50, renderer: renderPropertiesUI, dataIndex: 'Visible' },
			{ header: 'Required', width: 60, renderer: renderPropertiesUI, dataIndex: 'Required' },
            { header: 'Editable', width: 50, renderer: renderPropertiesUI, dataIndex: 'Editable' }
		]);
	} else if (viewId == "60020") {
	    cm = new Ext.grid.ColumnModel([
		    { id: 'TargetControlID', header: 'TargetControlID', hidden: true, dataIndex: 'ElementName' },
			{ header: 'Field', width: 132, dataIndex: 'Label' },
			{ header: 'Visible', hidden: true, dataIndex: 'Visible' },
			{ header: 'Required', width:120, renderer: renderPropertiesUI, dataIndex: 'Required' },
            { header: 'Editable', hidden: true, dataIndex: 'Editable' }
        ]);
	} else {
	    cm = new Ext.grid.ColumnModel([
		    { id: 'TargetControlID', header: 'TargetControlID', hidden: true, dataIndex: 'ElementName' },
			{ header: 'Field', width: 132, dataIndex: 'Label' },
			{ header: 'Visible', width: 60, renderer: renderPropertiesUI, dataIndex: 'Visible' },
			{ header: 'Required', width: 60, renderer: renderPropertiesUI, dataIndex: 'Required' },
            { header: 'Editable', hidden: true, dataIndex: 'Editable' }
		]);
	}

    var fields = new Array();
    for (var i = 0; i < obj._sectionFields.length; i++) {
        if (obj._sectionFields[i].Standard != "N" && obj._sectionFields[i].ElementType.toLowerCase()!= "line") {
            fields.push(obj._sectionFields[i]);
        }
    }

    var data1 = { 'DataSource': fields };

    var ds = new Ext.data.Store({
        proxy: new Ext.data.MemoryProxy(data1),
        reader: new Ext.data.JsonReader({ root: 'DataSource' }, [
            { name: 'ElementName' },
            { name: 'Label' },
            { name: 'Visible' },
            { name: 'Required' },
            { name: 'Editable' }
        ])
    });

    ds.load();
    var grid2;
    // Do not need setting form designer
    if (viewId == "60020") {
        grid2 = new Ext.grid.GridPanel({
            el: elementID,
            ds: ds,
            cm: cm,
            title: Ext.LabelKey.Admin_SectionProperty_AvailableFields,
            stripeRows: true,
            enableColumnMove: false,
            enableHdMenu: false,
            border: true,
            autoHeight: true
        });
    } else {
        grid2 = new Ext.grid.GridPanel({
            el: elementID,
            ds: ds,
            cm: cm,
            title: Ext.LabelKey.Admin_SectionProperty_AvailableFields,
            stripeRows: true,
            enableColumnMove: false,
            enableHdMenu: false,
            border: true,
            autoHeight: true,
            tbar: [{
                text: 'Rearrange Fields',
                iconCls: 'x-btn-text x-tree-node-toolbar',
                handler: function() {
                    var tab = Ext.getCmp(Ext.Const.OpenedId);
                    if (tab != undefined) {
                        if (tab.title.indexOf('*') > 0) {
                            var currTabTxt = tab.title.replace(' *', '');
                            if (confirm(Ext.LabelKey.Admin_Frame_SaveAlert + ' ' + currTabTxt + '?')) {
                                isCloseOtherTab = true;
                                var saveOK = SaveHandle(tab.id);
                                if (saveOK == false) {
                                    isCloseOtherTab = false;
                                    return false;
                                }

                            RemoveLPSectionFields(Ext.Const.ModuleName);
                        }
                        else {
                            return false;
                        }
                    }
                }

                    ChangeRefreshTag(false);
                    DataChangedTag(false);
                    ShowSilverlightWin(obj);
                }
            }]
        });
    }

    grid2.render();

    /// if section is attachment, hide toolbar.
    var sectionId = obj._sectionIdValue.split(Ext.Const.SplitChar)[1];
    var isShowTool = false;
    for (var i = 0; i < Ext.Const.SectionDict.length; i++) {
        if (Ext.Const.SectionDict[i].sectionId == sectionId) {
            isShowTool = true;
            break;
        }
    }

    if (!isShowTool) {
        grid2.topToolbar.hide();
    }

    var rowIndex = 0;
    for (var i = 0; i < obj._sectionFields.length; i++) {
        if (obj._sectionFields[i].Standard != "N" && obj._sectionFields[i].ElementType.toLowerCase()!= "line") {
            var sectionField = obj._sectionFields[i];
            if (sectionField.ElementName.indexOf("radioListContactPermission") > 0) {
                ControlTheAsterisk(sectionField.ElementName + "_radioListContactPermission", sectionField.Required);
            }
            else {
                ControlTheAsterisk(sectionField.ElementName, sectionField.Required);
            }
            InitVisibleAndRequiredControl(grid2.store.data.items[rowIndex].data.ElementName, grid2.store.data.keys[rowIndex], CurrentTab);
            rowIndex++;
        }
    }
}

function ShowSilverlightWin(obj) {
    if (!silverlightWin) {
        silverlightWin = new Ext.Window({
            title: Form_Designer_Title,
            layout: 'fit',
            width: 710,
            height: 560,
            modal: true,
            closeAction: 'close',
            plain: true
        });
    }

    silverlightWin.on("beforeclose", function () {
        if (isDataChanged) {
            if (!confirm(ACA_Form_Designer_Without_Saving_Msg)) {
                return false;
            }
        }
    });

    silverlightWin.on("close", function () {
        try {
            //close Rearrange window.
            closeRearrangeWin();
            if (isNeedRefreshSection) {
                var sectionArg = GetSectionArgument(obj);
                var frm = GetFrame(CurrentTab);
                //refresh form page.window.location.reload(true)
                RemoveLPSectionFields(Ext.Const.ModuleName);
                frm.contentWindow.location.reload(true);
            }
        } catch (e) {
        }

        //clear tool panel
        if (isNeedRefreshSection) {
            var tempDiv = document.getElementById('divField');
            tempDiv.innerHTML = "";
        }
    });

    var sectionArg = GetSectionArgument(obj);
    var slSrc = 'ACAFormDesigner.aspx?module=' + encodeURIComponent(sectionArg.module) + '&viewId=' + sectionArg.viewId + '&permissionLevel=' + encodeURIComponent(sectionArg.permissionLevel) + '&permissionValue=' + encodeURIComponent(sectionArg.permissionValue) + "&sectionName=" + encodeURIComponent(sectionArg.sectionName) + "&rnd=" + Math.random();
    silverlightWin.html = '<iframe width=100% height=100% frameborder=0 scrolling=auto src=' + slSrc + ' tiltle="The silverlight content iframe." ><p>If you can see this text, your browser does not support iframes. <a href=\"' + slSrc + '\">View the content of this inline frame</a> within your browser.</p></iframe>';
    silverlightWin.show();
}

//The visible and required are checked and disabled when control's IsDBRquired property is true.
//Current, parcel number, license type and license number's IsDBRequired is true only.
function InitVisibleAndRequiredControl(targetControlID, rowIndex, activeTab) {
    var currentDom = GetDom(activeTab);
    if (!currentDom) {
        return;
    }
    var strVisible = "Visible";
    var strRequired = "Required";
    var strEditable = "Editable";
    var visibleControl = document.getElementById(strVisible + '-' + rowIndex);
    var requiredControl = document.getElementById(strRequired + '-' + rowIndex);
    var editabelControl = document.getElementById(strEditable + "-" + rowIndex);

    var targetControl = currentDom.getElementById(targetControlID);
    if (targetControl != null && typeof (targetControl.attributes["IsDBRequired"]) != "undefined") {
        var isDBRequired = targetControl.attributes["IsDBRequired"];
        if (visibleControl != null) {
            visibleControl.disabled = isDBRequired;
            visibleControl.checked = isDBRequired;
        }
        if (requiredControl != null) {
            requiredControl.disabled = isDBRequired;
            requiredControl.checked = isDBRequired;
        }
    }

    if (targetControl != null && typeof (targetControl.attributes["IsFieldRequired"]) != "undefined") {
        if (requiredControl.checked) {
            requiredControl.disabled = true;
        }
    }

    if (visibleControl != null && !visibleControl.checked) {
        if (requiredControl != null) {
            requiredControl.checked = false;
            requiredControl.disabled = true;
        }

        if (editabelControl != null) {
            editabelControl.checked = false;
            editabelControl.disabled = true;
        }
    }

    if (targetControl != null && targetControl.type == "checkbox" && requiredControl!=null) {
        requiredControl.checked = false;
        requiredControl.disabled = true;
    }

    if (targetControl != null && targetControlID.indexOf("txtFinalScore_txtFinalScore")!=-1) {
        requiredControl.checked = false;
        requiredControl.disabled = true;
        if (visibleControl != null) {
            visibleControl.disabled = true;
            visibleControl.checked = true;
        }
    }

    if (targetControl != null && targetControlID.indexOf("txtAKAName") != -1) {
        requiredControl.checked = false;
        requiredControl.disabled = true;
    }

    if (visibleControl != null && !visibleControl.checked && editabelControl != null) {
        editabelControl.checked = false;
        editabelControl.disabled = true;
    }

    if (targetControl != null && targetControlID.endWith("ddlContactType") && targetControl.value == "" && editabelControl!=null) {
        editabelControl.disabled = true;
        editabelControl.checked = true;
    }
}

//Add/Remove * symbol that is abover target control
function ControlTheAsterisk(targetControlID, isRequired) {
    var el = GetDom(CurrentTab).getElementById(targetControlID + '_label_0');
    if (el == null) {
        el = GetDom(CurrentTab).getElementById(targetControlID + '_State1_label_0');
    }

    if (el == null) {
        var attachmentObj = null;
        attachmentObj = GetDom(CurrentTab).getElementById("divNewAttachment");
        if (attachmentObj != null) {
            el = attachmentObj.contentWindow.document.getElementById(targetControlID + '_label_0');
        }

    }

    if (el == null) {
        return;
    }

    if (isRequired) {
        if (el.innerHTML.indexOf("*") == -1) {
            el.innerHTML = "<DIV class='ACA_Required_Indicator'>*</DIV>";
        }
    }
    else {
        el.innerHTML = '';
    }
}

//Get DIV element group from current document DOM model.
//May be the current doucment is from iframe document of active tab.
function GetDIVElementForCurrentDom(targetControlID, isGridView) {
    var currentDom = GetDom(CurrentTab);
    if (!currentDom) {
        return null;
    }
    var div = currentDom.getElementById(GetRealID(targetControlID) + '_element_group');
    // if the control is state, here need to append sub control id 'State1', which is defined AccelaStateControl.cs
    if (div == null && targetControlID.indexOf("radioListContactPermission") > 0) {
        div = currentDom.getElementById(GetRealID(targetControlID) + '_divContact');
    }

    if (div == null) {
        div = currentDom.getElementById(GetRealID(targetControlID) + '_State1_element_group');
    }

    if (div == null) {
        div = currentDom.getElementById(targetControlID);
    }

    //Get targetControlID of control object when this object is in iframe object  of current document.
    if (div == null) {
        var attachmentObj = null;
        if (!isGridView) {
            attachmentObj = currentDom.getElementById("divNewAttachment");
            if (attachmentObj != null) {
                div = attachmentObj.contentWindow.document.getElementById(targetControlID + '_element_group');
                
                var txtDescription = attachmentObj.contentWindow.document.getElementById("txtAFileDescription");
                var divHeight = 340;
                attachmentObj.height = (txtDescription != null && txtDescription.parentNode.parentNode.style.display == "none") ? 220 : divHeight;
            }
        }
        else {
            if (currentDom.currentSelectedObj) {
                div = currentDom.currentSelectedObj._div;
            }
        }
    }

    return div;
}

function UpdateSectionFieldsData(visibleControl, requiredControl,editableControl, targetControlID) {
    var itms = new Array();
    var currentDom = GetDom(CurrentTab);

    for (var i = 0; i < sectionFields.length; i++) {
        if (sectionFields[i].ElementName == targetControlID) {
            sectionFields[i].Visible = visibleControl == null ? true : visibleControl.checked;
            if (requiredControl != null) {
                sectionFields[i].Required = requiredControl.checked;
            }

            if (editableControl != null) {
                sectionFields[i].Editable = editableControl.checked;
            }
        }

        var currentObj = currentDom.getElementById(sectionFields[i].ElementName);

        var isReuired = false;
        var isVisible = true;
        if (currentObj != null && currentObj.attributes["IsDBRequired"] != null) {
            isReuired = currentObj.attributes["IsDBRequired"].nodeValue == "true" ? true : false;
        }
        else {
            isReuired = sectionFields[i].Required;
            isVisible = sectionFields[i].Visible;
        }

        // old code
        //itms[i] = new FieldObj(sectionFields[i].ElementName.replace(prefixOfSection, ''), isVisible, isReuired);
        itms[i] = {
            Label: sectionFields[i].Label,
            Visible: sectionFields[i].Visible,
            Required: sectionFields[i].Required,
            Editable: sectionFields[i].Editable,
            ElementName: sectionFields[i].ElementName,
            OriginalElementName:sectionFields[i].OriginalElementName,
            Order: sectionFields[i].Order,
            Left: sectionFields[i].Left,
            Top: sectionFields[i].Top,
            Width: sectionFields[i].Width,
            InputWidth: sectionFields[i].InputWidth,
            LabelWidth: sectionFields[i].LabelWidth,
            UnitWidth: sectionFields[i].UnitWidth,
            Height: sectionFields[i].Height,
            ViewElementId: sectionFields[i].ViewElementId,
            Standard: sectionFields[i].Standard,
            ControlPrefix: sectionFields[i].ControlPrefix,
            ElementType: sectionFields[i].ElementType,
            LabelId: sectionFields[i].LabelId,
            OldStatus: sectionFields[i].OldStatus
        };
    }

    SetLPSectionFields(changeItem.Permission, itms);
    changeItem.SectionItems = itms;
    changeItems.UpdateItem(4, changeItem);

    ModifyMark();
}

function ChangeVisable(prefix, controlId, elementId, e) {
    var src = e.srcElement || e.target;
    var itms = new Array();
    // Special Handle: When Admin hidden Start Date and End Date, we should inform them that it will bring a performance issue
    if (!src.checked &&
        (controlId == "ctl00_PlaceHolderMain_txtStartDate" ||
         controlId == "ctl00_PlaceHolderMain_txtEndDate" ||
         controlId == "ctl00_PlaceHolderMain_txtAPO_Search_by_Permit_StartDate" ||
         controlId == "ctl00_PlaceHolderMain_txtAPO_Search_by_Permit_EndDate"
         )) {
        ShowErrorMessage(Ext.LabelKey.Admin_SectionProperty_RemoveDateRangeFields);
    }

    if (elementId) {
        SetFieldVisible(prefix, controlId, src.checked, elementId);
        var grid = GetGridBeElementId(elementId);
        if (grid != null) {
            FixLastColumnWidth(grid);
        }
    }
    else {
        SetFieldVisible(prefix, controlId, src.checked, null);
    }
    
    var minTop = 0;
    var currentIndex = 0;
    for (var i = 0; i < sectionFields.length; i++) {
        if (minTop > sectionFields[i].Top) {
            minTop = sectionFields[i].top;
        }

        if (sectionFields[i].ElementName == controlId) {
            sectionFields[i].Visible = src.checked;
            currentIndex = i;
        }
        //old code
        //itms[i] = new FieldObj(sectionFields[i].ElementName.replace(prefix, ''), sectionFields[i].Visible, sectionFields[i].Required);
        itms[i] = {
            Label: sectionFields[i].Label,
            Visible: sectionFields[i].Visible,
            Required: sectionFields[i].Required,
            Editable: sectionFields[i].Editable,
            ElementName: sectionFields[i].ElementName,
            OriginalElementName: sectionFields[i].OriginalElementName,
            Order: sectionFields[i].Order,
            Left: sectionFields[i].Left,
            Top: sectionFields[i].Top,
            Width: sectionFields[i].Width,
            InputWidth: sectionFields[i].InputWidth,
            LabelWidth: sectionFields[i].LabelWidth,
            UnitWidth: sectionFields[i].UnitWidth,
            Height: sectionFields[i].Height,
            ViewElementId: sectionFields[i].ViewElementId,
            Standard: sectionFields[i].Standard,
            ControlPrefix: sectionFields[i].ControlPrefix,
            ElementType: sectionFields[i].ElementType,
            LabelId: sectionFields[i].LabelId,
            OldStatus: sectionFields[i].OldStatus
        };
    }

    if (!src.checked) {

        itms[currentIndex].Top = minTop - 1;
    }

    SetLPSectionFields(changeItem.Permission, itms);
    changeItem.SectionItems = itms;
    changeItems.UpdateItem(4, changeItem);
    ModifyMark();
}

function LoadGridViewCol(obj) {
    var tblx = document.getElementById('divField');
    tblx.innerHTML = '';

    if (obj._gridViewAllowPaging) {
        ConfigPageSize(obj);
        var avaliableLabel = Ext.DomHelper.insertAfter('pgSectionProperty', {
            tag: 'div',
            html: '<span>' + Ext.LabelKey.Admin_SectionProperty_AvailableFields + '</span>'
        }, true);
    } else {
        var avaliableLabel = Ext.DomHelper.insertFirst('divField', {
            tag: 'div',
            html: '<span>' + Ext.LabelKey.Admin_SectionProperty_AvailableFields + '</span>'
        }, true);
    }

    sectionFields = obj._sectionFields;
    LoadFieldsList(avaliableLabel.id, obj, 'gridview');
}

function FixLastColumnWidth(grid, scrollLeft) {
    if (!grid) return;
    var isTR = false;
    var defaultWidth = 760;//ie 9's width need 
    if (grid.defaultWidth) {
        defaultWidth = grid.defaultWidth - 10; //ie 9's width need 
    } else if (grid.tagName == "TR") {
        isTR = true;
    }

    var headers = grid.getElementsByTagName('TH');
    var hiddenWidth = 0;
    var parentDiv = null;
    if (headers != null && headers.length > 0) {
        var tempTH = headers[0];
        var table = GetColumnTable(tempTH);

        if (isTR && table.defaultWidth) {
            defaultWidth = table.defaultWidth;
        }
        
        if (table) {
            parentDiv = GetGridViewContainer(table);
        }
        $(table).css("width", defaultWidth.toString() + "px");
    }

    var visibleColumnList = new Array();
    for (var k = 0; k < headers.length; k++) {
        var th = headers[k];

        if (th.style && th.style.display != "none" && th.style.visibility != "hidden" && th.columnWidth != undefined) {
            visibleColumnList.push(th);
        }
        if ($(th).css("visibility") == "hidden") {
            var hw = th.style.width.replace("px", "");
            if (isNumber(hw)) {
                hiddenWidth += parseInt(hw);
            }
        }
    }

    if (visibleColumnList.length > 0) {
        SetColulmnWidth(visibleColumnList);

        var width = 0;
        for (var i = 0; i < visibleColumnList.length - 1; i++) {
            var th = visibleColumnList[i];
            var w = 0;

            if (th.originalWidth) {
                w = th.originalWidth;
            } else {
                w = th.style.width;
            }

            if (w) {
                w = w.replace("px", "");
            }

            if (isNumber(w)) {
                width += parseInt(w);
            } else {
                // default width
                width += 100;
            }
        }

        var lastColumn = visibleColumnList[visibleColumnList.length - 1];
        var lastColumnWidth = lastColumn.style.width.replace("px", "");

        if (!isNumber(lastColumnWidth)) {
            lastColumnWidth = 100;
        }

        var totalWidth = width + parseInt(lastColumnWidth) + hiddenWidth;

        if (totalWidth < defaultWidth) {
            lastColumnWidth = defaultWidth - width - hiddenWidth - 10;
            var width = lastColumnWidth.toString() + "px";
            $(lastColumn).css("width", width);
            $(parentDiv).css("overflow-x", "visible");
        } else {
            $(parentDiv).css("overflow-x", "auto");
        }

        var table = GetColumnTable(lastColumn);
        if (totalWidth < defaultWidth) {
            $(table).css("width", defaultWidth.toString() + "px");
        } else {
            var tw = totalWidth.toString() + "px";
            $(table).css("width", tw);
        }
    }

    // reset the parent scroll left position
    if (arguments.length == 2 && parentDiv) {
        parentDiv.scrollLeft = scrollLeft;
    }
}

function ResetColumnWidth(obj, oldValue, value) {
    if (oldValue == value || oldValue == null) return;
    var grid = GetGridRow(obj._element);
    var parentDiv = null;
    var scrollLeft = 0;

    if (grid) {
        parentDiv = GetGridViewContainer(grid);

        if (parentDiv && parentDiv.tagName == "DIV") {
            scrollLeft = parentDiv.scrollLeft;
        }
    }

    var headers = GetColumns(grid);
    if (headers && headers.length > 0) {
        SetColulmnWidth(headers);
    }

    FixLastColumnWidth(grid, scrollLeft);
}

function GetGridRow(obj) {
    var o = obj.parentNode;
    while (o && o.tagName != 'TR') {
        o = o.parentNode;
    }

    return o;
}

function GetGridBeElementId(elementId) {
    if (!elementId) {
        return null;
    }

    var grid = GetDIVElementForCurrentDom(elementId, true);

    return grid;
}

function GetColumns(grid) {
    var columns = new Array();
    var headers = grid.getElementsByTagName('TH');

    for (var i = 0; i < headers.length; i++) {
        var th = headers[i];
        if (th.style && th.style.display != "none" && th.style.visibility != "hidden") {
            columns.push(th);
        }
    }

    return columns;
}

function SetColulmnWidth(headers) {
    for (var i = 0; i < headers.length; i++) {
        var th = headers[i];
        if (th.columnWidth != undefined) {
            //th.style.width=th.columnWidth;
            var width = th.columnWidth;
            if (width.indexOf("px") == "-1") {
                width = width.toString() + "px";
            }
            $(th).css("width", width);
            th.originalWidth = th.columnWidth;
        }
    }
}

function GetGridViewContainer(obj) {
    var o = obj.parentNode;
    while (o && o.tagName != 'DIV') {
        o = o.parentNode;
    }

    return o;
}

// get the section argument, module, viewId, permissionLevel, permissionValue
function GetSectionArgument(obj) {
    var module = obj.get_SectionID().split(Ext.Const.SplitChar)[0];
    var viewId = obj.get_SectionID().split(Ext.Const.SplitChar)[1];
    var permissionLevel = '';
    var permissionValue = '';
    var sectionName = obj.get_DefaultLabel();

    var templateGenus = obj._templateGenus;
    if (templateGenus != undefined && templateGenus != null && templateGenus != "") {
        permissionLevel = Ext.Const.TemplateGenusType;
        permissionValue = templateGenus;
    }
    else {
        // get Permission Level and Value from Ext.Const
        for (var i = 0; i < Ext.Const.SectionDict.length; i++) {
            if (viewId == Ext.Const.SectionDict[i].sectionId) {
                permissionLevel = Ext.Const.SectionDict[i].permissionLevel;
                permissionValue = Ext.Const.SectionDict[i].permissionValue;
                break;
            }
        }

        /*
         For Contact/LP/Document sections (except Account contact):
         When customize the form layout, needs to pass through the permission value as the Type 
         to separate the diffenente layout in form-designer view.
         1. If Contact look up form(view id = 60180), the layout change without contact type.
        */
        if (obj.get_PermissionValueId()) {
            permissionValue = obj.getPermissionValue();
        }
        else if ((permissionLevel == Ext.Const.People && viewId != '60180' && viewId != '60181') || viewId == '60016' || viewId == '60137') {
            var typeControlId = '';
            if (permissionValue == 'License') {
                typeControlId = 'ddlLicenseType';
            }
            else if (permissionLevel == Ext.Const.Attachment) {
                typeControlId = 'ddlDocType';
            }
            else {
                typeControlId = 'ddlContactType';
            }

            var prefixOfSection = obj._sectionIdValue.split(Ext.Const.SplitChar)[2];
            var typeControl = GetDom().getElementById(prefixOfSection + typeControlId);
            if (typeControl != null && typeControl.value != '') {
                permissionValue = typeControl.value.replace('-||', '');
            }
            else {
                permissionValue = '';
            }
        }
    }

    var sectionArg = { module: module, viewId: viewId, permissionLevel: permissionLevel, permissionValue: permissionValue, sectionName: sectionName };
    return sectionArg;
}

function GetLPSectionFields(sectionArg) {
    var fields = null;
    var key = sectionArg.module + '_' + sectionArg.viewId + '_' + sectionArg.permissionLevel;
    if (sectionArg.permissionValue != null) {
        key = key + '_' + sectionArg.permissionValue;
    }

    for (var i = 0; i < MultipleTypesSectionFields.length; i++) {
        if (MultipleTypesSectionFields[i].key == key) {
            fields = MultipleTypesSectionFields[i].Fields;
            break;
        }
    }

    return fields;
}

function SetLPSectionFields(sectionArg, fields) {
    var isStore = false;
    var key = sectionArg.module + '_' + sectionArg.viewId + '_' + sectionArg.permissionLevel;
    if (sectionArg.permissionValue != null) {
        key = key + '_' + sectionArg.permissionValue;
    }

    for (var i = 0; i < MultipleTypesSectionFields.length; i++) {
        if (MultipleTypesSectionFields[i].key == key) {
            isStore = true;
            MultipleTypesSectionFields[i].Fields = fields;
            break;
        }
    }

    if (!isStore) {
        MultipleTypesSectionFields.push({ key: key, Fields: fields });
    }
}

function RemoveLPSectionFields(moduleName) {
    for (var i = MultipleTypesSectionFields.length - 1; i >= 0; i--) {
        if (MultipleTypesSectionFields[i].key.indexOf(moduleName) >= 0) {
            MultipleTypesSectionFields.splice(i, 1);
        }
    }
}

function SetPageSize(obj, pgSection) {
    var levelData = obj._sectionIdValue.split(Ext.Const.SplitChar)[0];
    var GridViewId = obj._sectionIdValue.split(Ext.Const.SplitChar)[1];
    var oldPageSize = obj._element.getAttribute("pageSizeAttribute");

    if (typeof (oldPageSize) == 'undefined' || oldPageSize == '' || oldPageSize == null) {
        var objList = new Array(pgSection, obj._gridViewPageSize);
        PageMethods.GetPageSizeByGviewId(levelData, GridViewId, GetPageSizeSuccess, null, objList);
    }
    else {
        pgSection.store.getById('AdminGridViewPageSize').set('name', Ext.LabelKey.admin_gridview_pagesize);
        pgSection.store.getById('AdminGridViewPageSize').set('value', oldPageSize);
    }
}

function GetPageSizeSuccess(result, objList) {
    var pageSize = result;
    var pgSection = objList[0];
    var tempPageSize = objList[1];

    if (pageSize == null || pageSize == "" || typeof (pageSize) == 'undefined') {
        if (tempPageSize <= 0) {
            pageSize = "10";
        } else {
            pageSize = tempPageSize;
        }
    }

    pgSection.store.getById('AdminGridViewPageSize').set('name', Ext.LabelKey.admin_gridview_pagesize);
    pgSection.store.getById('AdminGridViewPageSize').set('value', pageSize);
}

function ConfigPageSize(obj) {
    var isNeedModifyMark = false;
    var pgSection = new Ext.grid.PropertyGrid({
        closable: true,
        autoHeight: true,
        id: 'pgSectionProperty',
        listeners: {
            propertychange: function (source, recordId, value, oldValue) {
                if (value != '' && oldValue != '') {
                    if (recordId == 'AdminGridViewPageSize') {
                        //positive integer validate
                        var express = /^[0-9]*[1-9][0-9]*$/;

                        if (!express.test(value)) {
                            ShowErrorMessage(Ext.LabelKey.acaadmin_gridview_pagesizemessage);
                            SetPageSize(obj, pgSection);
                            return;
                        } else {
                            if (oldValue != value) {
                                if (express.test(oldValue)) {
                                    isNeedModifyMark = true;
                                }
                                pgSection.store.getById(recordId).set('value', value);
                                var changeItem = new GridViewPageSize(Ext.Const.OpenedId, value, obj);
                                changeItems.UpdateItem(10, changeItem);
                                obj._element.setAttribute("pageSizeAttribute", value);
                                if (isNeedModifyMark) {
                                    ModifyMark();
                                }
                            }
                        }
                    }
                }
            }
        }
    });

    pgSection.store.sortInfo = null;
    pgSection.setSource({
        'AdminGridViewPageSize': ''
    });
    pgSection.getColumnModel().setConfig([
              { header: 'Name', sortable: false, dataIndex: 'name', id: 'name' },
              { header: 'Value', sortable: false, dataIndex: 'value', id: 'value' }
            ]);

    SetPageSize(obj, pgSection);
    RenderGrid(1, pgSection);
}

function SetSectionLabel(obj, value) {
    if (obj._elementTypeValue == 'AccelaCheckBox' || obj._elementTypeValue == 'AccelaRadioButton') {
        obj._element.parentNode.lastChild.innerHTML = value;
    }
    else {
        obj._element.innerHTML = value;
    }
}

function GetSectionLabel(obj) {
    if (obj._elementTypeValue == 'AccelaCheckBox' || obj._elementTypeValue == 'AccelaRadioButton') {
        return obj._element.parentNode.lastChild.innerHTML;
    }
    else {
        return obj._element.innerHTML;
    }
}
