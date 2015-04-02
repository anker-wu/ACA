/**
* <pre>
*
*  Accela Citizen Access
*  File: AccelaAKA.js
*
*  Accela, Inc.
*  Copyright (C): 2012-2014
*
*  Description: Client scripts for AccelaAKA control.
*
*  Notes:
* $Id: AccelaAKA.js 239070 2012-12-07 07:03:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Dec 7, 2012      Alan Hu     Initial.
* </pre>
*/

/*
* Add an AKA item in specific AKA list.
*/
function AddAKAItem(listId, rowData) {
    if (typeof (JSON) == 'undefined') {
        return;
    }

    var table = $('#' + listId);
    var stateCtl = $('#' + table.attr('stateControlId'));
    var akaState = JSON.parse(stateCtl.val());
    var firstRow = table.find('tbody > tr:first-child');

    //Create new row based on first row.
    var currentRow = firstRow.clone();

    //Calculate maximum row index.
    var maxRowIdx = -1;
    $(akaState.RowIndexes).each(function () {
        if (parseInt(this) > maxRowIdx) {
            maxRowIdx = parseInt(this);
        }
    });

    //Set row index.
    var rowIdx = maxRowIdx + 1;
    currentRow.attr('rowIdx', rowIdx);

    //Append new row to the aka list.
    var currentRowHtml = $('<div></div>').append(currentRow).html();
    currentRowHtml = currentRowHtml.replace(/(\s+(name|id)=['"]?[\$\w]*(FirstName|MiddleName|LastName|FullName))\d+(['"]?)/ig, '$1' + rowIdx + '$4');
    table.append(currentRowHtml);

    //Get the current row in the aka list.
    currentRow = table.find('tbody > tr[rowIdx=' + rowIdx + ']');

    /*
    Indicating whether the aka data is reference data.
    Reference data cannot be edit and delete.
    AKA item data format:{FirstName:'first name',MiddleName:'middle name',LastName:'last name',FullName:'full name',AKAID:'contact number'}
    */
    var hasRefData = rowData && rowData.AKAID && !/^\s*$/.test(rowData.AKAID);

    //Get current data fields and delete link in current row.
    var currentRowFields = currentRow.find('td > input[type=text]');
    var currentDelLink = currentRow.find('td.action > div.del_button > a');

    if (rowData) {
        //Have aka data -> assign aka data to data fields.
        var firstNameField = currentRow.find('td > input[fieldName=FirstName]');
        var middleNameField = currentRow.find('td > input[fieldName=MiddleName]');
        var lastNameField = currentRow.find('td > input[fieldName=LastName]');
        var fullNameField = currentRow.find('td > input[fieldName=FullName]');

        if (rowData.FirstName) {
            firstNameField.val(rowData.FirstName);
        }
        else {
            firstNameField.val('');
        }

        if (rowData.MiddleName) {
            middleNameField.val(rowData.MiddleName);
        }
        else {
            middleNameField.val('');
        }

        if (rowData.LastName) {
            lastNameField.val(rowData.LastName);
        }
        else {
            lastNameField.val('');
        }

        if (rowData.FullName) {
            fullNameField.val(rowData.FullName);
        }
        else {
            fullNameField.val('');
        }
    }
    else {
        //No aka data -> clear all fields' value.
        currentRowFields.val('');
    }

    if (akaState.ReadOnly || hasRefData) {
        //Has aka data and is reference data.
        //Set data fields as readonly.
        currentRowFields.attr('readonly', 'readonly');
        currentRowFields.addClass('ACA_ReadOnly');

        //Hide delete action for reference data.
        currentDelLink.removeAttr('href');
        currentDelLink.hide();
        currentDelLink.attr('tabindex', '-1');
    }
    else {
        //No aka data or isn't reference data.
        //Set data fields as editable.
        currentRowFields.removeAttr('readonly');
        currentRowFields.removeClass('ACA_ReadOnly');

        //Show delete action.
        currentDelLink.attr('href', 'javascript:DelAKAItem(\'' + listId + '\',' + rowIdx + ');');
        currentDelLink.show();

        if ($.browser.opera) {
            currentDelLink.attr('tabindex', '0');
        }
        else {
            currentDelLink.removeAttr('tabindex');
        }
    }

    //Update state data.
    akaState.RowIndexes.push('' + rowIdx);
    akaState.AKAIDs['' + rowIdx] = hasRefData ? rowData.AKAID : null;
    akaState.FullNames['' + rowIdx] = hasRefData ? rowData.FullName : null;
    akaState.StartDates['' + rowIdx] = hasRefData ? rowData.StartDate : null;
    akaState.EndDates['' + rowIdx] = hasRefData ? rowData.EndDate : null;

    //Show delete action for first row if first now is not readonly and there are multiple items in curren aka list.
    if (akaState.RowIndexes.length == 2 && firstRow.find('td > input[readOnly]').length <= 0) {
        var delAction = 'javascript:DelAKAItem(\'' + listId + '\',' + firstRow.attr('rowIdx') + ');';
        var firstDelLink = firstRow.find('td.action > div.del_button > a');
        firstDelLink.attr('href', delAction);
        firstDelLink.show();

        if ($.browser.opera) {
            firstDelLink.attr('tabindex', '0');
        }
        else {
            firstDelLink.removeAttr('tabindex');
        }
    }

    //Save state data to the state control.
    stateCtl.val(JSON.stringify(akaState));
}

/*
* Delete an AKA item in specific AKA list.
*/
function DelAKAItem(listId, rowIdx) {
    if (typeof (JSON) == 'undefined') {
        return;
    }

    var table = $('#' + listId);
    var stateCtl = $('#' + table.attr('stateControlId'));
    var akaState = JSON.parse(stateCtl.val());

    //Delete the specific row from aka list.
    table.find('tbody > tr[rowIdx=' + rowIdx + ']').remove();

    //Update state data.
    Array.remove(akaState.RowIndexes, '' + rowIdx);
    delete akaState.AKAIDs['' + rowIdx];
    delete akaState.FullNames['' + rowIdx];
    delete akaState.StartDates['' + rowIdx];
    delete akaState.EndDates['' + rowIdx];

    //Hide the delete action if only one item in current aka list.
    if (akaState.RowIndexes.length == 1) {
        var firstDelLink = table.find('tbody > tr:first-child > td.action > div.del_button > a');
        firstDelLink.hide();
        firstDelLink.attr('tabindex', '-1');
    }

    //Save state data to the state control.
    stateCtl.val(JSON.stringify(akaState));

    //Focus on Add link after delete a row except to delete the template row (rowIdx == -1).
    if (rowIdx > -1) {
        table.find("tfoot > tr > td.action > div.add_button > a").focus();
    }
}

/*
* Set value for the specific AKA list.
*/
function SetAKAValue(ctlId, value) {
    if (typeof (JSON) == 'undefined') {
        return;
    }

    var listId = ctlId + '_list';
    var table = $('#' + listId);
    
    if (!$.exists(table)) {
        return;
    }

    var stateCtl = $('#' + table.attr('stateControlId'));
    var akaState = JSON.parse(stateCtl.val());

    /*
    AKA list data format:
    [
        {FirstName:'first name1',MiddleName:'middle name1',LastName:'last name1',FullName:'full name1',AKAID:'aka resid 1'},
        {FirstName:'first name2',MiddleName:'middle name2',LastName:'last name2',FullName:'full name2',AKAID:'aka resid 2'},
        ...
    ]
    */

    //If fill the empty data, the list will be cleared.
    if (!value || value.length == 0) {
        value = [{ FirstName: null, MiddleName: null, LastName: null, FullName: null, StartDate: null, EndDate: null, AKAID: null}];
    }

    /*
    Steps:
    1. Find first row.
    2. Clear all existed rows and reset the aka state.
    3. Add the first row to aka list as the template row (row index is -1).
    4. Add the data rows.
    5. Remove the first template row (row index is -1).
    */
    var templateRow = table.find('tbody > tr:first-child');
    templateRow.attr('rowIdx', '-1');
    table.find('tbody > tr').remove();
    stateCtl.val('{"RowIndexes":[],"AKAIDs":{},"FullNames":{},"StartDates":{},"EndDates":{},"ReadOnly":' + akaState.ReadOnly + '}');
    table.append(templateRow);

    for (var i = 0; i < value.length; i++) {
        AddAKAItem(listId, value[i]);
    }

    DelAKAItem(listId, '-1');
}

/*
* Set the specific AKA list as readonly.
*/
function DisableAKAField(ctlId) {
    if (typeof (JSON) == 'undefined') {
        return;
    }

    var listId = ctlId + '_list';
    var table = $('#' + listId);
    
    if (!$.exists(table)) {
        return;
    }
    
    var stateCtl = $('#' + table.attr('stateControlId'));
    var akaState = JSON.parse(stateCtl.val());

    //Set readonly property.
    akaState.ReadOnly = true;

    //Disable add action.
    var addLink = table.find("tfoot > tr > td.action > div.add_button > a");
    addLink.hide();
    addLink.attr('tabindex', '-1');

    //Disable all delete actions.
    var allDelLinks = table.find("tbody > tr > td.action > div.del_button > a");
    allDelLinks.hide();
    allDelLinks.attr('tabindex', '-1');

    //Disable all data fields.
    var allFields = table.find('tbody > tr > td > input[type=text]');
    allFields.attr('readonly', 'readonly');
    allFields.addClass('ACA_ReadOnly');

    //Save state data to the state control.
    stateCtl.val(JSON.stringify(akaState));
}

/*
* Set the specific AKA list as editable.
*/
function EnableAKAField(ctlId) {
    if (typeof (JSON) == 'undefined') {
        return;
    }

    var listId = ctlId + '_list';
    var table = $('#' + listId);
    
    if (!$.exists(table)) {
        return;
    }
    
    var stateCtl = $('#' + table.attr('stateControlId'));
    var akaState = JSON.parse(stateCtl.val());

    //Set readonly property.
    akaState.ReadOnly = false;

    //Enable add action.
    var addLink = table.find("tfoot > tr > td.action > div.add_button >a");
    addLink.show();
    
    if ($.browser.opera) {
        addLink.attr('tabindex', '0');
    }
    else {
        addLink.removeAttr('tabindex');
    }

    //Loop each aka item.
    table.find("tbody > tr").each(function () {
        var currentRow = $(this);
        var rowIdx = currentRow.attr('rowIdx');

        //To determine whether is reference data.
        var hasRefData = akaState.AKAIDs[rowIdx] && !/^\s*$/.test(akaState.AKAIDs[rowIdx]);

        if (!hasRefData) {
            //If isn't reference data.
            var currentRowFields = currentRow.find('td > input[type=text]');

            //Set data fields as editable in current row.
            currentRowFields.removeAttr('readonly');
            currentRowFields.removeClass('ACA_ReadOnly');

            if (akaState.RowIndexes.length > 1) {
                //If have at least 2 rows in current aka list, enable the current row's delete action.
                var currentDelLink = currentRow.find('td.action > div.del_button > a');
                currentDelLink.show();

                if ($.browser.opera) {
                    currentDelLink.attr('tabindex', '0');
                }
                else {
                    currentDelLink.removeAttr('tabindex');
                }
            }
        }
    });

    //Save state data to the state control.
    stateCtl.val(JSON.stringify(akaState));
}

/*
* Adjust AKA field's layout.
*/
function AdjustAKAField(ctlId) {
    var listId = ctlId + '_list';
    var table = $('#' + listId);

    if (table.attr('adjusted')) {
        return;
    }
    
    var firstRow = table.find('tbody > tr:first-child');
    var firstDelLink = firstRow.find('td.action > div.del_button > a');

    /*
    In ACA Admin, the AKA control may be hidden by default.
    In order to adjust the control layout, need to set the control as visible first,
        and then restore the control display status after the control layout be adjusted.
    */

    var allHiddenParents;
    var hiddenParentsDisplayValues;

    if (!table.is(':visible')) {
        allHiddenParents = table.parents(':hidden');
        hiddenParentsDisplayValues = [];

        allHiddenParents.each(function () {
            /*
            If there is a css style made the element hidden, when use $(element).css('display') method to get the style.display attribute,
            will get the 'none' value, the 'none' value is not the really value of style.display, so use element.style.display method to get the really value.
            */
            var display = this.style ? this.style.display : '';
            hiddenParentsDisplayValues.push(display);
        });

        allHiddenParents.show();
    }

    //Use row width, delete link width to calculate field column's width.
    var rowWidth = firstRow.innerWidth();
    var delLinkWidth = firstDelLink.outerWidth(true);
    var dataTdWidth = parseInt((rowWidth - delLinkWidth) / firstRow.find('td.data :visible').length, 0);

    //Set column width for delete link and data field.
    table.find("tbody > tr > td.action").width(delLinkWidth);
    table.find("tbody > tr > td.data :visible").width(dataTdWidth);

    //Calculate and set input fields's width unit with em instead of px.
    var allDataFields = table.find("tbody > tr >td > input[type=text]");
    var fieldPadding = allDataFields.outerWidth(true) - allDataFields.innerWidth();
    var dataTdInnerWidth = firstRow.find('td:first-child :visible').innerWidth();
    allDataFields.css("width", ((dataTdInnerWidth - fieldPadding - 2) / 10.0) + "em");

    table.attr('adjusted', true);

    //Restore the display status of the parent controls.
    if (allHiddenParents && hiddenParentsDisplayValues) {
        allHiddenParents.each(function () {
            $(this).css('display', hiddenParentsDisplayValues.shift());
        });
    }
}
