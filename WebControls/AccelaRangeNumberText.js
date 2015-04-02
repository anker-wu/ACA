/**
* <pre>
*
*  Accela Citizen Access
*  File: AccelaRangeNumberText.js
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: Client scripts for AccelaRangeNumberText control.
*
*  Notes:
* $Id: AccelaRangeNumberText.js
*  Revision History
*  Date,            Who,        What
*  March 06, 2014   Ian Chen     Initial.
* </pre>
*/

var AccelaRangeNumberText = new function () {
    this.validatorForRequired = validatorForRequired;
    this.validatorForRangeValue = validatorForRangeValue;

    /*
    * Func: validatorForRequired
    * Desc: To validate required of the range number texts, in required case, less one of value of from and to should be input.
    * Para: source  
    *       args  
    */
    function validatorForRequired(source, args) {
        var parentId = source.ValidatorCallbackBehavior._callbackControlID;
        var rangeFromId = parentId + GlobalConst.ChildControlIdPrefix + "0";
        var rangeToId = parentId + GlobalConst.ChildControlIdPrefix + "1";
        var rangeFrom = AccelaCompositeControl.getChildControlValue(rangeFromId);
        var rangeTo = AccelaCompositeControl.getChildControlValue(rangeToId);

        var validChildIds = new Array();
        var invalidChildIds = new Array();

        if (!isNullOrEmpty(rangeFrom) || !isNullOrEmpty(rangeTo)) {
            validChildIds = [rangeFromId, rangeToId];
        } else {
            invalidChildIds = [rangeFromId, rangeToId];
        }

        source.isCompositeControl = true;
        source.validateType = 'required';
        source.validChildIds = validChildIds;
        source.invalidChildIds = invalidChildIds;

        args.IsValid = (invalidChildIds.length == 0);
        return args.IsValid;
    }

    /*
    * Func: validatorForRangeValue
    * Desc: To validate the range value of from and to, while value of to is less than from, the values is invalid and renturn false.
    * Para: source  
    *       args     
    */
    function validatorForRangeValue(source, args) {
        var parentId = source.ValidatorCallbackBehavior._callbackControlID;
        var rangeFromId = parentId + GlobalConst.ChildControlIdPrefix + "0";
        var rangeToId = parentId + GlobalConst.ChildControlIdPrefix + "1";
        var rangeFrom = AccelaCompositeControl.getChildControlValue(rangeFromId);
        var rangeTo = AccelaCompositeControl.getChildControlValue(rangeToId);

        var validChildIds = [rangeFromId, rangeToId];
        var invalidChildIds = new Array();
        
        if (!isNullOrEmpty(rangeFrom) && !isNullOrEmpty(rangeTo)) {
            if (parseInt(rangeTo) < parseInt(rangeFrom)) {
                var currentChildId = source.controltovalidate;
                if (currentChildId == rangeFromId) {
                    validChildIds = [rangeToId];
                    invalidChildIds = [rangeFromId];
                } else {
                    validChildIds = [rangeFromId];
                    invalidChildIds = [rangeToId];
                }
            }
        }

        source.isCompositeControl = true;
        source.validateType = 'range';
        source.validChildIds = validChildIds;
        source.invalidChildIds = invalidChildIds;

        args.IsValid = (invalidChildIds.length == 0);
        return args.IsValid;
    }
}
