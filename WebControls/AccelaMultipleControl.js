/**
* <pre>
*
*  Accela Citizen Access
*  File: AccelaMultipleControl.js
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: Client scripts for AccelaMultipleControl control.
*
*  Notes:
* $Id: AccelaMultipleControl.js
*  Revision History
*  Date,            Who,        What
*  March 06, 2014   Ian Chen     Initial.
* </pre>
*/

var AccelaMultipleControl = new function () {
    this.validatorForRequired = validatorForRequired;
    this.validatorForDuplicate = validatorForDuplicate;
    this.initializeControl = initializeControl;
    this.adjustSubFieldLayout = adjustSubFieldLayout;

    function initializeControl(currentParentId, nextParentId, controlWidth) {
        this.controlWidth = controlWidth;

        if (typeof (Sys) != "undefined" && Sys.WebForms && Sys.WebForms.PageRequestManager && Sys.WebForms.PageRequestManager.getInstance) {
            with (Sys.WebForms.PageRequestManager.getInstance()) {
                add_pageLoaded(function() {
                    setSubControlTabOrder(currentParentId, nextParentId);
                });
            }
        }
    }

    /*
    * Func: ValidatorForRequired
    * Desc: To validate required of the multiple control.
    * Para: source  
    *       args  
    */
    function validatorForRequired(source, args) {
        var parentId = source.ValidatorCallbackBehavior._callbackControlID;
        var validChildIds = new Array();
        var invalidChildIds = new Array();

        $(AccelaCompositeControl.getChildControl(parentId)).each(function () {
            var childId = $(this).attr('id');
            var childValue = AccelaCompositeControl.getChildControlValue(childId);

            if (!isNullOrEmpty(childValue)) {
                validChildIds[validChildIds.length] = childId;
            } else {
                invalidChildIds[invalidChildIds.length] = childId;
            }
        });

        // these propeties will be used to validation, for ValidatorCallbackBehavior.js use. 
        source.isCompositeControl = true;
        source.validateType = 'required';
        source.validChildIds = validChildIds;
        source.invalidChildIds = invalidChildIds;

        args.IsValid = (invalidChildIds.length == 0);
        return args.IsValid;
    }

    /*
    * Func: ValidatorForDuplicate
    * Desc: To validate duplicate of the multiple control.
    * Para: source  
    *       args  
    */
    function validatorForDuplicate(source, args) {
        var parentId = source.ValidatorCallbackBehavior._callbackControlID;
        var validChildIds = new Array();
        var invalidChildIds = new Array();

        var duplicateObjs = new Array();
        $(AccelaCompositeControl.getChildControl(parentId)).each(function () {
            var childId = $(this).attr('id');
            var childValue = AccelaCompositeControl.getChildControlValue(childId);

            var isContainValue = false;
            $.each(duplicateObjs, function () {
                var thisValue = source.ValidatorCallbackBehavior._validationIgnoreCase ? childValue.toLowerCase() : childValue;
                var otherValue = source.ValidatorCallbackBehavior._validationIgnoreCase ? this.childValue.toLowerCase() : this.childValue;

                if (thisValue.trim() == otherValue.trim()) {
                    this.childIds[this.childIds.length] = childId;

                    isContainValue = true;
                    return false;
                }
            });

            if (!isContainValue) {
                var childIds = new Array();
                childIds[0] = childId;
                duplicateObjs[duplicateObjs.length] = { childValue: childValue, childIds: childIds };
            }
        });

        var foundInvalid = false;
        $.each(duplicateObjs, function() {
            if (foundInvalid || isNullOrEmpty(this.childValue) || this.childIds.length == 1) {
                validChildIds = validChildIds.concat(this.childIds);
            } else if (!foundInvalid && this.childIds.length > 1) {
                invalidChildIds = this.childIds;
                foundInvalid = true;
            }
        });

        // these propeties will be used to validation, for ValidatorCallbackBehavior.js use. 
        source.isCompositeControl = true;
        source.validateType = 'duplicate';
        source.validChildIds = validChildIds;
        source.invalidChildIds = invalidChildIds;

        args.IsValid = (invalidChildIds.length == 0);
        return args.IsValid;
    }

    /*
    * Func: setSubControlTabOrder
    * Desc: Set the sub control's tab order between two related multiple control.
    * Para: currentParentId  
    *       nextParentId  
    */
    function setSubControlTabOrder(currentParentId, nextParentId) {
        if (!currentParentId || !nextParentId) {
            return;
        }

        var childControlCount = AccelaCompositeControl.getChildControl(currentParentId).length;
        if (childControlCount <= 1) {
            return;
        }

        var arrTabOrderId = [];
        var nextHelpId = nextParentId + '_help';

        for (var i = 0; i < childControlCount; i++) {
            var currentChildId = currentParentId + GlobalConst.ChildControlIdPrefix + i;
            var nextChildId = nextParentId + GlobalConst.ChildControlIdPrefix + i;
            arrTabOrderId.push(currentChildId);

            if (i == 0 && $.exists($('#' + nextHelpId))) {
                arrTabOrderId.push(nextHelpId);
            }

            arrTabOrderId.push(nextChildId);
        }

        $('#' + currentParentId + ',#' + nextParentId + ', #' + nextHelpId).keydown(function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which;
            if (keyCode == 9 || e.shiftKey && keyCode == 9) {
                var obj = e.target || e.srcElement;
                var index = arrTabOrderId.indexOf(obj.id);
                var nextObjId;

                if (e.shiftKey) {
                    if (index - 1 >= 0) {
                        nextObjId = arrTabOrderId[index - 1];
                    }
                } else if (index + 1 < arrTabOrderId.length) {
                    nextObjId = arrTabOrderId[index + 1];
                }

                if (nextObjId) {
                    e.preventDefault();
                    $('#' + nextObjId).focus();
                }
            }
        });
    }

    /*
    * Func: adjustSubFieldLayout
    * Desc: Adjust sub label and sub control layout.
    * Para: parentId  
    *       controlType
    *       layoutType
    */
    function adjustSubFieldLayout(parentId, controlType, layoutType) {
        var parentControl = $('#' + parentId);
        var childrenControlCount = AccelaCompositeControl.getChildControl(parentId).length;
        var tdSubLabels = parentControl.find('td.SubLabel');
        var subControls = parentControl.find('td.SubControl ' + controlType);

        var controlWidthWithUnit = isNullOrEmpty(AccelaMultipleControl.controlWidth) ? parentControl.css('width') : AccelaMultipleControl.controlWidth;
        var parentControlWidth = controlWidthWithUnit.lastIndexOf('em') != -1 ? parseFloat(controlWidthWithUnit) * 10 : parseInt(controlWidthWithUnit);

        var subLabelSpace = 0;
        var subLabelWidth = 0;

        if (childrenControlCount > 1) {
            subLabelSpace = 8;
            subLabelWidth = $(tdSubLabels[childrenControlCount - 1]).width();
        }

        var subControlWidth = parentControlWidth - subLabelWidth - subLabelSpace;

        if (controlType == 'select') {
            if (!isNullOrEmpty(AccelaMultipleControl.controlWidth)) {
                if (isFireFox()) {
                    subControlWidth += 2;
                } else {
                    subControlWidth += 5;
                }
            } else {
                if (childrenControlCount == 1) {
                    if ($.browser.msie) {
                        subControlWidth += 1;
                    } else if (isFireFox()) {
                        subControlWidth += 6;
                    } else {
                        subControlWidth += 4;
                    }
                } else {
                    if (isFireFox()) {
                        subControlWidth += 3;
                    } else if (!$.browser.msie) {
                        subControlWidth += 1;
                    }
                }
            }
        }
        else if (controlType == 'input[type=text]') {
            if (isNullOrEmpty(AccelaMultipleControl.controlWidth)) {
                if (childrenControlCount > 1) {
                    subControlWidth -= 3;
                } else if ($.browser.msie) {
                    subControlWidth -= 2;
                }
            }
        }

        if (childrenControlCount == 1) {
            $('#' + parentId + GlobalConst.ChildControlIdPrefix + '0').width(subControlWidth/10 + 'em');
        } else {
            for (var i = 0; i < childrenControlCount; i++) {
                $(tdSubLabels[i]).width(subLabelWidth + subLabelSpace);
                $(subControls[i]).width(subControlWidth/10 + 'em');
            }
        }

        if (layoutType == 'Vertical') {
            $('#' + parentId + '_help').css(($.global.isRTL ? 'margin-left' : 'margin-right'), '0');
        }
    }
};