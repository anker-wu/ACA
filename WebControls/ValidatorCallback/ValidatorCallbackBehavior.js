/**
* <pre>
* 
*  Accela Citizen Access
*  File: ValidatorCallbackBehavior.js
* 
*  Accela, Inc.
*  Copyright (C): 2007-2014
* 
*  Description:
* 
*  Notes:
* $Id: ValidatorCallbackBehavior.js 72643 2007-07-10 21:52:06Z vernon.crandall $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

Type.registerNamespace('AjaxControlToolkit');

AjaxControlToolkit.ValidatorCallbackBehavior = function AjaxControlToolkit$ValidatorCallbackBehavior(element) {
    AjaxControlToolkit.ValidatorCallbackBehavior.initializeBase(this, [element]);

    this._highlightCssClass = null;
    this._invalid = false;
    this._originalValidationMethod = null;
    this._validationMethodOverride = null;
    this._elementToValidate = null;
    this._focusAttached = false;
    this._focusHandler = Function.createDelegate(this, this._onfocus);
    this._callbackFailFunction = null;
    this._callbackControlID = null;
    this._currentValidateControlClientID = null;
    this._checkControlValueValidateFunction = null;
    this._validationByHiddenTextBox = false;
    this._validationIgnoreCase = false;
};

AjaxControlToolkit.ValidatorCallbackBehavior.prototype = {
    get_validationIgnoreCase: function() {
        return this._validationIgnoreCase;
    },
    set_validationIgnoreCase: function(value) {
        if (this._validationIgnoreCase != value) {
            this._validationIgnoreCase = value;
            this.raisePropertyChanged("validationIgnoreCase");
        }
    },

    get_highlightCssClass: function() {
        return this._highlightCssClass;
    },
    set_highlightCssClass: function(value) {

        if (this._highlightCssClass != value) {
            this._highlightCssClass = value;
            this.raisePropertyChanged("highlightCssClass");
        }
    },

    get_validationByHiddenTextBox: function() {
        return this._validationByHiddenTextBox;
    },
    set_validationByHiddenTextBox: function(value) {
        if (this._validationByHiddenTextBox != value) {
            this._validationByHiddenTextBox = value;
            this.raisePropertyChanged("validationByHiddenTextBox");
        }
    },

    get_callbackFailFunction: function() {
        return this._callbackFailFunction;
    },
    set_callbackFailFunction: function(value) {

        if (this._callbackFailFunction != value) {
            this._callbackFailFunction = value;
            this.raisePropertyChanged("callbackFailFunction");
        }
    },

    get_callbackControlID: function() {
        return this._callbackControlID;
    },
    set_callbackControlID: function(value) {
        if (this._callbackControlID != value) {
            this._callbackControlID = value;
            this.raisePropertyChanged("callbackControlID");
        }
    },

    get_currentValidateControlClientID: function () {
        return this._currentValidateControlClientID;
    },
    set_currentValidateControlClientID: function (value) {
        if (this._currentValidateControlClientID != value) {
            this._currentValidateControlClientID = value;
            this.raisePropertyChanged("currentValidateControlClientID");
        }
    },

    get_checkControlValueValidateFunction: function() {
        return this._checkControlValueValidateFunction;
    },
    set_checkControlValueValidateFunction: function(value) {

        if (this._checkControlValueValidateFunction != value) {
            this._checkControlValueValidateFunction = value;
            this.raisePropertyChanged("checkControlValueValidateFunction");
        }
    },

    initialize: function() {
        AjaxControlToolkit.ValidatorCallbackBehavior.callBaseMethod(this, 'initialize');
        var elt = this.get_element();
        //               
        // Override the evaluation method of the current validator
        //
        if (elt.evaluationfunction) {
            if (typeof(elt.evaluationfunction) == "string") {
                elt.evaluationfunction = eval(elt.evaluationfunction);
            }
            this._originalValidationMethod = Function.createDelegate(elt, elt.evaluationfunction);
            this._validationMethodOverride = Function.createDelegate(this, this._onvalidate);
            elt.evaluationfunction = this._validationMethodOverride;
        }

        if (elt.controltovalidate) {
            this._elementToValidate = $get(elt.controltovalidate);
        } else {
            this._elementToValidate = $get(elt.getAttribute('TargetValidator')) || $get(elt.TargetValidator);
        }

        this._validateControl = elt;
    },

    dispose: function() {

        if (this._focusAttached) {
            $removeHandler(this._elementToValidate, "focus", this._focusHandler);
            this._focusAttached = false;
        }

        AjaxControlToolkit.ValidatorCallbackBehavior.callBaseMethod(this, 'dispose');
    },

    _getErrorMessage: function() {
        var elt = this.get_element() || this._validateControl;
        if (typeof(elt) != 'undefined') {
            return elt.errormessage || elt.getAttribute('InvalidValueMessage') || elt.InvalidValueMessage || AjaxControlToolkit.Resources.ValidatorCallback_DefaultErrorMessage;
        }
    },

    _onfocus: function(e) {
        //as below code could cause issue which user can't focus on input box when accessibility mode is on.
        //after confirmed by Daly Zeng, we disabled below code.
        //        if (!this._originalValidationMethod(this.get_element())) {
        //            if (this._highlightCssClass) {
        //                Sys.UI.DomElement.addCssClass(this._elementToValidate, this._highlightCssClass);
        //            }
        //            return false;
        //        } else {
        //            return true;
        //        }
    },

    _checkDisable: function() {
        if (this._elementToValidate != null) {
            if (this._elementToValidate.attributes["ValidateDisabledControl"] != null
                && typeof(this._elementToValidate.attributes["ValidateDisabledControl"]) != 'undefined') {
                if (this._elementToValidate.attributes["ValidateDisabledControl"].nodeValue == 'Y') {
                    return true;
                }
            }

            var isReadOnly = false;
            if (typeof(this._elementToValidate.tagName) != 'undefined' && this._elementToValidate.tagName.toUpperCase() == 'DIV') {
                var inputs = this._elementToValidate.getElementsByTagName('input');
                if (inputs.length > 0 && typeof(inputs[0].readOnly) != 'undefined') {
                    isReadOnly = inputs[0].readOnly;
                } else if (typeof(this._elementToValidate.readOnly) != 'undefined') {
                    isReadOnly = this._elementToValidate.readOnly;
                } else if (typeof(this._elementToValidate.attributes["readOnly"]) != 'undefined') {
                    isReadOnly = this._elementToValidate.attributes["readOnly"].value;
                }
            } else if (typeof(this._elementToValidate.readOnly) != 'undefined') {
                isReadOnly = this._elementToValidate.readOnly;
            } else if (typeof(this._elementToValidate.attributes["readOnly"]) != 'undefined') {
                isReadOnly = this._elementToValidate.attributes["readOnly"].value;
            }

            var isDisable = false;
            if (typeof(this._elementToValidate.disabled) != 'undefined') {
                isDisable = this._elementToValidate.disabled;
            } else if (typeof(this._elementToValidate.attributes["disabled"]) != 'undefined') {
                isDisable = this._elementToValidate.attributes["disabled"].value == "disabled";
            }

            if (isDisable == false && typeof (ddlLPType) != 'undefined' && ddlLPType == this._callbackControlID) {
                /*
                 License Type in LP must be validated even if be set to readonly by expression.
                 Has attribute "ReadOnlyByExp" means the field is readonly by expression.
                 */
                var ddlLicenseType = $get(ddlLPType);

                if (ddlLicenseType != null && (!isReadOnly|| ddlLicenseType.attributes["ReadOnlyByExp"]) && ddlLicenseType.className.indexOf('ACA_ReadOnly') > -1 && ddlLicenseType.value == '') {
                    if (typeof(showNormalMessage) != 'undefined') {
                        showNormalMessage(LicenseTypeConfigurationErrorMessage, 'Error');
                    }
                    return false;
                }
            }

            return isReadOnly || isDisable;
        }

        return false;
    },

    _checkValidate4Hidden: function() {
        var $validateObj = $("#" + this._elementToValidate.id);

        if ($validateObj.is(":hidden")) {
            // Special validate logic for AccelaGridView control.
            if (this._validationByHiddenTextBox) {
                return true;
            }

            return false;
        }

        return true;
    },

    _evalcheckControlValueValidateFunction: function() {
        if (this._checkControlValueValidateFunction != null && this._checkControlValueValidateFunction != '') {
            var result;
            eval('result = ' + this._checkControlValueValidateFunction + '("' + this._elementToValidate.id + '");');
            return result;
        }
        return false;
    },

    //if current page is SPEAR page and user click the "save and resume later" button ,then the control that has empty value does not need to validate
    _isNeedValidate: function() {
        if (this._elementToValidate != null && $get(this._elementToValidate.id) != null) {
            if (typeof(IsNeedValidation) != 'undefined' && !IsNeedValidation(this._elementToValidate.id)) {
                return false;
            }

            if (typeof(CapEditPageNotValidateEmpetyValueControlFlag) == 'undefined') {
                return true;
            } else {
                if (CapEditPageNotValidateEmpetyValueControlFlag) {
                    var value = GetValue(this._elementToValidate);
                    if (typeof(value) != 'undefined') {
                        // For checkbox
                        if (this._elementToValidate.type == 'checkbox')
                            return false;
                        else
                            return value.trim() != '';
                    } else {
                        return true;
                    }
                } else {
                    return true;
                }
            }
        }

        return false;
    },

    _getValidateResults: function() {
        var vs = this._elementToValidate.needDoValidate;
        if (typeof(vs) == 'undefined') {
            vs = new Array();
            this._elementToValidate.needDoValidate = vs;
        }

        return vs;
    },

    _addValidateResult: function(id, succeed) {
        var vs = this._getValidateResults();
        for (var i = 0; i < vs.length; i++) {
            if (vs[i].id == id) {
                vs.length = 0;
                break;
            }
        }
        var vr = new Object();
        vr.id = id;
        vr.succeed = succeed;
        vs[vs.length] = vr;
    },

    _succeed: function() {
        var vs = this._getValidateResults();
        for (var i = 0; i < vs.length; i++) {
            if (!vs[i].succeed) {
                return false;
            }
        }

        return true;
    },

    _onvalidate: function (val) {
        var childObj = { childId: "", validateType: "", validChildIds: "", invalidChildIds: "" };

        if (val.isNeedValidate || this._isNeedValidate()) {
	
            if (val.isNeedValidate) {
                val.isNeedValidate = false;
            }

            if (!this._evalcheckControlValueValidateFunction() && !this._checkDisable() && this._checkValidate4Hidden()) {
                var originalValidationResult = this._originalValidationMethod(val);

                if (typeof (val.isCompositeControl) != "undefined") {
                    childObj.childId = this._currentValidateControlClientID ? this._currentValidateControlClientID : "";
                    childObj.validateType = val.validateType ? val.validateType : "";
                    childObj.validChildIds = val.validChildIds.join(',');
                    childObj.invalidChildIds = val.invalidChildIds.join(',');
                }

                if (!originalValidationResult || (originalValidationResult && this._elementToValidate != null && this._elementToValidate.type == "checkbox" && this._elementToValidate.checked == false)) {
                    if (this._highlightCssClass) {
                        Sys.UI.DomElement.addCssClass(this._elementToValidate, this._highlightCssClass);
                    }
                    if (!this._focusAttached) {
                        $addHandler(this._elementToValidate, "focus", this._focusHandler);
                        this._focusAttached = true;
                    }
                    if (this._callbackFailFunction) {
                        eval(this._callbackFailFunction + "('" + this._getErrorMessage() + "','" + this._callbackControlID + "', 2,'" + childObj.childId + "','" + childObj.validateType + "', '" + childObj.validChildIds + "','" + childObj.invalidChildIds + "');");
                    }
                    this._invalid = true;
                    val.isvalid = false;
                    this._addValidateResult(val.id, false);
                    return false;
                }
            }
        }

        this._addValidateResult(val.id, true);

        if (!this._succeed()) {
            if (val.isCompositeControl && typeof (myValidationErrorPanel) != "undefined") {
                myValidationErrorPanel.updateError(this._callbackControlID, "", childObj);
                if (myValidationErrorPanel.errorPanelVisible) {
                    var isAccessibilityEnabled = typeof (accessibilityEnabled) == "function" ? accessibilityEnabled() : false;
                    myValidationErrorPanel.printErrors(isAccessibilityEnabled);
                }
            }
            return;
        }
        if (this._highlightCssClass && (val.isCompositeControl || this._invalid)) {
            Sys.UI.DomElement.removeCssClass(this._elementToValidate, this._highlightCssClass);
            if (this._callbackFailFunction) {
                eval(this._callbackFailFunction + "('','" + this._callbackControlID + "', 0,'" + childObj.childId + "','" + childObj.validateType + "','" + childObj.validChildIds + "','" + childObj.invalidChildIds + "');");
            }
        }
        this._invalid = false;
        val.isvalid = true;
        return true;
    }
};
AjaxControlToolkit.ValidatorCallbackBehavior.registerClass('AjaxControlToolkit.ValidatorCallbackBehavior', AjaxControlToolkit.BehaviorBase);
