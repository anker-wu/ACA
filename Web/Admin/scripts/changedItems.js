
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: changeItems.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: changeItems.js 72643 2008-07-11 17:47:06Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *
 * </pre>
 */

// JScript File
function ChangeItems()
{   
    this.Add = Add;
    this.UpdateItem = UpdateItem;
    this.GetItem = GetItem;
    this.GetChangedItemsByPageId=GetChangedItemsByPageId;
    this.RemoveChangedItemsByPageId = RemoveChangedItemsByPageId;
    this.RemoveAllChangeItems = RemoveAllChangeItems;
    this.AgencyCode;
    this.ModuleName;
    this.Items = {};
    this.ItemType = new ItemTypeEnum();

    for (var i = 0; i < this.ItemType.length; i++) {
        this.Items[i] = new Array();
    }

    function GetItem(type, item) {
        var arr = this.Items[type];

        if (arr != null) {
            var index = -1;
            var isExist = false;

            for (var i = 0; i < arr.length; i++) {
                if (arr[i].PageId == item.PageId && arr[i].ControlId == item.ControlId && arr[i].ModuleName == item.ModuleName && arr[i].LabelKey == item.LabelKey) {
                    // if section is License or Contact,need add permissionValue condition to indicating it's same view.
                    var isPermissionSameViewID = (type == 4) && (item.Permission != null && arr[i].Permission != null
                        && arr[i].Permission.permissionLevel == item.Permission.permissionLevel
                        && arr[i].Permission.permissionValue == item.Permission.permissionValue
                        && arr[i].Permission.viewId == item.Permission.viewId);

                    if (type != 4 || isPermissionSameViewID) {
                        isExist = true;
                        index = i;
                        break;
                    }
                }
            }

            // update/add change item.
            if (isExist) {
                return arr[index];
            }
        }

        return null;
    }

    // Update item into changeItems, if it is not in changeItems, add it.
    function UpdateItem(type, item) {
        var existItem = this.GetItem(type, item);

        // update/add change item.
        if (existItem != null) {
            existItem = UpdateItemValue(existItem, item);
        }
        else {
            this.Add(type, item);
        }
    }

    //Add a change item in changeItems
    function Add(type, itm) {
        var arr = this.Items[type];

        if (arr != null) {
            var length = arr.length;
            arr[length] = itm;
        }
    }

    //Get changed items by id which is given
    function GetChangedItemsByPageId(pageId) {
        var resultItems = new ChangeItems();

        for (var type = 0; type < this.ItemType.length; type++) {
            var arr = this.Items[type];
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].PageId == pageId) {
                    if (type == this.ItemType.arrTab) {
                        arr[i].TabNames = arr[i].TabNames.replace(/_\d+_/g, '');
                    }

                    resultItems.Add(type, arr[i]);
                }
            }
        }

        resultItems.AgencyCode = GetAgencyCode();
        resultItems.ModuleName = Ext.Const.ModuleName;

        return resultItems;
    }

    //remove the saved item from changeItems
    function RemoveChangedItemsByPageId(pageId) {
        var resultItems = new ChangeItems();

        for (var type = 0; type < this.ItemType.length; type++) {
            var arr = this.Items[type];
            for (var i = 0; i < arr.length; i++) {
                if (type == this.ItemType.arrReports) {
                    continue;
                }

                if (arr[i].PageId != pageId) {
                    resultItems.Add(type, arr[i]);
                }
                else if (type == this.ItemType.arrDropDown) {
                    UpdateValue(changeItems.Items[type][i]);
                }
            }
        }

        changeItems = resultItems;
    }

    //remove all saved item from changeItems.
    function RemoveAllChangeItems() {
        changeItems = new ChangeItems();
    }

    // update item value
    function UpdateItemValue(existedItem, newItem) {
        for (var prop in newItem) {
            if (typeof (newItem[prop]) != 'undefined' && newItem[prop] != null) {
                if (prop == "ExtenseObjects") {
                    if (typeof (existedItem[prop]) == 'undefined') {
                        existedItem.ExtenseObjects = new Array();
                    }

                    UpdateExtenseObjects(existedItem[prop], newItem[prop]);
                } else if (typeof (existedItem[prop]) != 'undefined') {
                    existedItem[prop] = newItem[prop];
                }
            }
        }

        return existedItem;
    }

    //Get agency code by selected GS agency dropDownList.
    function GetAgencyCode() {
        var selectedAgencyCode = GetDom().getElementById("ctl00_PlaceHolderMain_ddlGSSubAgency");
        var agencyCode = "";

        if (selectedAgencyCode != null) {
            agencyCode = selectedAgencyCode.value;
        }

        return agencyCode;
    }

    function UpdateValue(items) {
        if (needUpdateKey()) {
            var controlId = items.ControlId;
            var ddl = GetDom().getElementById(controlId);
            if (ddl != null && items.Type == 'StandardChoice') {
                if (items["CategoryValue"] == 'CONTACT TYPE')//If contact type, it needn't update ddl value.
                {
                    return;
                }

                // if Items is undefined, it needn't update dll value, e.g. ddlLicensingBoard click license type options
                if (typeof (items.Items) == "undefined") {
                    return;
                }

                var start = (ddl.length > 0 && ddl[0].value == '') ? 1 : 0;
                var j = 0;
                for (var i = start; i < ddl.length; i++) {
                    var item = items.Items[j].Label;
                    dataArray = item.split('||');

                    //dataArray[0] is show type. 1:show description, 2: show value and description
                    if (dataArray[0] == 1 || dataArray[0] == 2) 
                    {
                        var newValue = Ext.Const.IsSupportMultiLang ? dataArray[2] : dataArray[1];
                        var newDescr = Ext.Const.IsSupportMultiLang ? dataArray[4] : dataArray[2];
                        // update the dropdownlist key when add/update a bizdomian value
                        ddl[i].value = newValue + "||" + newValue + "||" + newDescr + "||" + newDescr;
                        j++;
                    }
                    else {
                        ddl[i].value = ddl[i].text;
                    }
                }
            }
        }
    }

    function needUpdateKey() {
        return IsDefaultLanguage();
    }

    function ItemTypeEnum() {
        this.arrText = 0;
        this.arrInput = 1;
        this.arrButton = 2;
        this.arrDropDown = 3;
        this.arrSection = 4;
        this.arrTab = 5;
        this.arrFilter = 6;
        this.arrConfigureButton = 7;
        this.arrReports = 8;
        this.arrHeadWith = 9;
        this.arrGridViewPageSize = 10;
        this.arrCustomComponent = 11;
        this.arrCollapseLine = 12;
        this.arrSectionEditable = 13;
        this.arrExtense = 14;

        // it is the change item's length
        this.length = 15;
    }
}

// color theme changed items object.
function ColorThemeChangeItems() {
    var changedItems;

    this.getChangedItems = function () {
        return changedItems;
    };

    this.updateChangedItem = function (val) {
        changedItems = val;
    };
}