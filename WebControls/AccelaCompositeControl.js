/**
* <pre>
*
*  Accela Citizen Access
*  File: AccelaCompositeControl.js
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: Client scripts for AccelaCompositeControl control.
*
*  Notes:
* $Id: AccelaCompositeControl.js
*  Revision History
*  Date,            Who,        What
*  March 06, 2014   Ian Chen     Initial.
* </pre>
*/

var AccelaCompositeControl = new function () {
    this.getChildControlValue = getChildControlValue;
    this.getChildControl = getChildControl;
    
    // Get the child control value
    function getChildControlValue(childId) {
        var childControl = $get(childId);

        if (typeof (childControl) == 'undefined') {
            return null;
        }

        var watermarkFrom = '';
        if (typeof (childControl.TextBoxWatermarkBehavior) != 'undefined') {
            watermarkFrom = childControl.TextBoxWatermarkBehavior._watermarkText;
        }

        var childValue = childControl.value.toString().replace(watermarkFrom, '');
        return childValue;
    }

    function getChildControl(parentId) {
        var idNumber = 0;
        var idPrefix = parentId + GlobalConst.ChildControlIdPrefix;
        var $child = $('#' + idPrefix + idNumber);
        var children = new Array();

        while ($.exists($child)) {
            children[idNumber] = $child;

            idNumber++;
            $child = $('#' + idPrefix + idNumber);
        }
        return children;
    }
};
