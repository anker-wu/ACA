/**
* <pre>
* 
*  Accela
*  File: Expression.js
* 
*  Accela, Inc.
*  Copyright (C): 2009-2014
* 
*  Description:
* To deal with expression logic in ACA model
*  Notes:
* $Id: Expression.js 79503 2007-11-08 07:04:56Z ACHIEVO\jackie.yu $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
*  05/14/2007     		lytton.cheng				Initial.
* </pre>
*/

var ExpressionControlType = { "HTML_TEXTBOX": 1, "HTML_TEXTBOX_OF_DATE": 2, "HTML_RADIOBOX": 3, "HTML_TEXTBOX_OF_NUMBER": 4, "HTML_SELECTBOX": 5, "HTML_TEXTAREABOX": 6, "HTML_TEXTBOX_OF_TIME": 7, "HTML_TEXTBOX_OF_CURRENCY": 8, "HTML_CHECKBOX": 9 };
var ExpressionType = { "ASI": "-1", "ASI_Table": "-2", "Fee_Item": "20002", "License_Professional": "122" };
var ObjectCollection = new Array();
var SpitChar = '\f';
//the message for block submit when validating
var ValidationMessage = "";
var IsLockLicense = false;
var INSERT_FIRST_ROW = "InsertFirstRow";
var RunningExpCount = 0;
// indicate that the submit button is triggered, and wait all expressions complete
var isSubmitted4Exp = false;
var isBlockSubmit = false;
var ASITFieldCollection = [];
var ASITInsertFieldCollection = [];
/***********************************************************************************************
 *
 * Func: RunExpression
 * Desc: 
 * Para: argumentsModel     -    
 *       inputParams        -    
 *       isSubmitFire       -
 *       fireObject         -    Fire Object
 ***********************************************************************************************/
function RunExpression(argumentsModel, inputFieldNames, isSubmitFire, fireObject) {

    if (typeof fireObject != "undefined" && fireObject != null &&
       typeof fireObject.readOnly != "undefined" && fireObject.readOnly == true) {
        return;
    }

    // 0. running expression count add 1
    RunningExpCount++;

    // 1. Get these fields' value from page
    var arrayFieldProperties = new Array();
    var currentWindow = $.global.isForPopUp ? parent : window;

    var fieldListInHiddenControl = null;
    var $hdExpression = GetExpResultStoreField();
    if ($hdExpression.length > 0 && $hdExpression.val() != "") {
        fieldListInHiddenControl = Sys.Serialization.JavaScriptSerializer.deserialize($hdExpression.val());
    }

    for (var index = 0; index < inputFieldNames.length; index++) {

        var inputFieldName = inputFieldNames[index];
        var control = $get(inputFieldName);

        if (control == null) {
            control = currentWindow.$get(inputFieldName);
        }

        if (control == null) {
            control = getElementByAttribute4ASI(inputFieldName, currentWindow);
        }

        if (control == null) {
            Array.add(arrayFieldProperties, null);
            continue;
        }

        var realControl = getRealyControl(control);
        var realValue = GetValue(realControl);

        // Special Logic for checkbox control: its control value is always 'on'.
        if (typeof (realControl.type) != "undefined" && realControl.type == "checkbox") {
            if (realControl.checked) {
                realValue = "CHECKED";
            } else {
                realValue = "UNCHECKED";
            }
        }

        // Encode some sepcial characters in control value
        if (typeof (realValue) != "undefined") {
            realValue = EncoderSpecialChars(realValue);
        }
        else {
            realValue = "";
        }

        var isNotFound = true;
        var fieldProperties = realValue;
        if (fieldListInHiddenControl != null && fieldListInHiddenControl.length > 0) {
            for (var i = 0; i < fieldListInHiddenControl.length; i++) {

                if (fieldListInHiddenControl[i].name == inputFieldNames[index]) {
                    fieldProperties += "\f" + (fieldListInHiddenControl[i].required ? fieldListInHiddenControl[i].required : ""); // true or false
                    fieldProperties += "\f" + (fieldListInHiddenControl[i].readOnly ? fieldListInHiddenControl[i].readOnly : ""); // true or false
                    fieldProperties += "\f" + (fieldListInHiddenControl[i].hidden ? fieldListInHiddenControl[i].hidden : ""); // true or false
                    fieldProperties += "\f" + (fieldListInHiddenControl[i].message ? fieldListInHiddenControl[i].message : "");
                    isNotFound = false;
                    break;
                }
            }
        }

        if (isNotFound) {
            fieldProperties += "\f" + "";
            fieldProperties += "\f" + "";
            fieldProperties += "\f" + "";
            fieldProperties += "\f" + "";
        }

        Array.add(arrayFieldProperties, fieldProperties);
    }

    var executeFieldName = argumentsModel && argumentsModel.executeFieldVariableKey ? argumentsModel.executeFieldVariableKey : "";

    if (IsTrue(isSubmitFire)) {
        Accela.ACA.Web.WebService.ExpressionService.RunExpression(
                Sys.Serialization.JavaScriptSerializer.serialize(argumentsModel),
                inputFieldNames,
                arrayFieldProperties,
                OnSucceededBySubmit,
                OnFailedBySubmit,
                executeFieldName);
    }
    else {
        isSubmitted4Exp = false;
        Accela.ACA.Web.WebService.ExpressionService.RunExpression(
                Sys.Serialization.JavaScriptSerializer.serialize(argumentsModel),
                inputFieldNames,
                arrayFieldProperties,
                OnSucceeded,
                OnFailed,
                executeFieldName);
    }
}

// This is the callback function invoked if the Web service
// succeeded.
// It accepts the result object as a parameter.
function OnSucceeded(result, executeFieldName) {
    RunningExpCount--;

    if (isContaintError(result)) {
        alert(result);
    } else {
        HandleResult(result, false, executeFieldName);
    }

    DoSubmit();
}

function OnSucceededBySubmit(result, executeFieldName) {
    RunningExpCount--;

    if (isContaintError(result)) {
        alert(result);
    } else {
        HandleResult(result, true, executeFieldName);
    }

    DoSubmit();
}

// This is the callback function invoked if the Web service
// failed.
// It accepts the error object as a parameter.
function OnFailed(error, executeFieldName) {
    RunningExpCount--;

    if (error.get_statusCode() != 0) {
        // Display the error.    
        var rsltElem = "Service Error: " + error.get_message();
        alert(rsltElem);
        DoSubmit();
    }

    return true;
}

// This is the callback function invoked if the Web service
// failed.
// It accepts the error object as a parameter.
function OnFailedBySubmit(error, executeFieldName) {
    isSubmitted4Exp = false;
    return OnFailed(error, executeFieldName);
}

function HandleResult(result, isOnSubmit, executeFieldName) {
    var finalResult = eval('(' + result + ')');

    if (finalResult != null && finalResult.fields != null) {
        HandleNormalResult(finalResult.fields);
        // 1. check if block by validation result
        var isBlock = CheckValidationResult(finalResult);
        CollectASITFields(finalResult,isOnSubmit);

        if (isOnSubmit && isBlock && !isBlockSubmit) {
            isBlockSubmit = true;
        }

        if (!isBlock) {
            var isOnLoad = executeFieldName && executeFieldName.toString().toLowerCase() == "onload";

            if (!isOnSubmit && !isOnLoad && accessibilityEnabled()
                && typeof(GLOBAL_DISABLE_EXPRESSION_SECTION508) != "undefined"
                && !GLOBAL_DISABLE_EXPRESSION_SECTION508) {
                var msgExpress = getText.global_js_expression;
                var expressionName = finalResult.fields[0].expressionName;

                if (msgExpress != "" && msgExpress != null
                    && expressionName != "" && expressionName != null) {
                    msgExpress = msgExpress.replace('{0}', expressionName);
                    alert(msgExpress);
                }
            }
        }
    }

    if (typeof (myValidationErrorPanel) != "undefined") {
        myValidationErrorPanel.recheckAllErrors(true);
    }
}

function IsASITExpField(variableKey) {
    var isASITField = false;

    if (variableKey && (variableKey.startWith('ASIT::') || variableKey.startWith('CONTACT1TPLTABLE::')
        || variableKey.startWith('CONTACT2TPLTABLE::') || variableKey.startWith('CONTACT3TPLTABLE::')
        || variableKey.startWith('APPLICANTTPLTABLE::') || variableKey.startWith('REFCONTACTTPLTABLE::')
        || variableKey.startWith('CONTACTTPLTABLE::') || variableKey.startWith('CUSTOMERTPLTABLE::')
    )) {
        isASITField = true;
    }

    return isASITField;
}

function IsASITExpEvent(executeFieldName) {
    return (executeFieldName.toString().toUpperCase() == "ONASITROWSUBMIT" || IsASITExpField(executeFieldName));
}

function IsASITEditForm() {
    return window.location.href.toUpperCase().indexOf("EDITFORM.ASPX") > -1;
}

function CollectASITFields(result, isOnSubmit) {
    if (result == null || result.fields == null) {
        return;
    }

    //Collect info: insertedFields/updateFields/tableKeys for insertedFields and updateFields | updatedTableKeys for updateFields | uiDataKeys for insertedFields.
    for (var i = 0; i < result.fields.length; i++) {
        var field = result.fields[i];

        if (IsASITExpField(field.variableKey) &&
            field.blockSubmit != true && !String.IsNullOrEmpty(field.subGroup)) {

            //Notes: Does not support the OnASITRowSubmit event to insert new row. So clear the inserted field list.
            if (isOnSubmit && field.name.startWith(INSERT_FIRST_ROW)) {
                continue;
            }

            ASITFieldCollection.push(field);
        }
    }
}

/***********************************************************************************************
*
* Func: HandleASITResult
* Desc: To handle the ASIT expression.
*       For Update Row: Send postback to refresh to data list.
*       For Insert Row: Pops the popup window to show the Added new row by expression.
*       For Insert & Update Row: Send postback to refresh the data list, and then register
*                                the scripts to show popup window for added rows.
* Para: result          - Expression result.
*       isOnSubmit      - Indicating whether the expression is triggerd by submit event.
***********************************************************************************************/
function HandleASITResult() {
    if (ASITFieldCollection == null || ASITFieldCollection.length == 0) {
        return;
    }

    if (RunningExpCount > 0) {
        return;
    }

    var isInAsitEditForm = IsASITEditForm();
    var tableKeys = [];
    var updatedTableKeys = [];
    var uiDataKeys = [];
    var insertedFields = [];
    var updatedFields = 0;

    //Collect info: insertedFields/updateFields/tableKeys for insertedFields and updateFields | updatedTableKeys for updateFields | uiDataKeys for insertedFields.
    for (var i = 0; i < ASITFieldCollection.length; i++) {
        var field = ASITFieldCollection[i];

        var tableInfo = field.subGroup.split('::');
        var tableKey = tableInfo[1];

        if (!tableKeys.contains(tableKey)) {
            tableKeys.push(tableKey);
        }

        if (field.name.startWith(INSERT_FIRST_ROW)) {
            //Inserted rows.
            var uiKey;

            if (isInAsitEditForm) {
                uiKey = parent.asitUIDataKeys[tableKey];
            }
            else {
                uiKey = asitUIDataKeys[tableKey];
            }

            if (!uiKey) {
                continue;
            }

            if (!uiDataKeys.contains(uiKey)) {
                uiDataKeys.push(uiKey);
            }

            //Replace field's name to the actual control id.
            var splitCharPos = field.name.indexOf(SpitChar);
            field.name = field.name.substr(splitCharPos + 1);
            insertedFields.push(field);
        }
        else {
            //Updated rows.
            if (!updatedTableKeys.contains(tableKey)) {
                updatedTableKeys.push(tableKey);
            }

            updatedFields++;
        }
    }

    ASITFieldCollection = [];

    if (insertedFields.length > 0) {
        // if there are insert asit field collection, then don't excute submit event.
        isSubmitted4Exp = false;
    }

    if (isInAsitEditForm) {
        //In ASIT edit form, only need to hand the inserted fields.
        if (insertedFields.length > 0) {
            //Refresh the edit form to render the new added fields.
            parent.ASITInsertFieldCollection = insertedFields;
            var postBackArg = parent.ASIT_BuildPostBackArgument(tableKeys, window);
            __doPostBack(postBackArg.EventTarget, postBackArg.EventArgument);
            ShowLoading();
        }
    }
    else {
        if (updatedFields > 0 && insertedFields.length > 0) {
            //Have 'Update Row' and 'Insert Row', only 'Update Row' associated Update-Panel needs to be refresh.
            var postBackArg = ASIT_BuildPostBackArgument(updatedTableKeys);

            //Create hidden field and post the expression info to backend.
            var expResultFieldId = ASIT_EXP_INSERTROW_FIELD_ID;
            var expResultField = $get(expResultFieldId);

            if (expResultField == null) {
                expResultField = document.createElement('input');
                expResultField.id = expResultFieldId;
                expResultField.name = expResultFieldId;
                expResultField.type = 'hidden';

                //Append hidden field to the update panel.
                var updatePanelID = postBackArg.EventTarget.replace(ASIT_POSTEVENT_TARGET_ID, '').replace(/\$/g, '_');
                var updatePanel = $get(updatePanelID);

                if (updatePanel) {
                    updatePanel.appendChild(expResultField);
                }
            }

            expResultField.value
                = Sys.Serialization.JavaScriptSerializer.serialize(insertedFields) + '||'
                + Sys.Serialization.JavaScriptSerializer.serialize(tableKeys) + '||'
                + Sys.Serialization.JavaScriptSerializer.serialize(uiDataKeys);

            SetNotAsk(true);
            __doPostBack(postBackArg.EventTarget + ASIT_EXP_INSERTROW_TARGET_ID, postBackArg.EventArgument);
        }
        else if (insertedFields.length > 0) {
            //Only to handle the insert row behavior by client scripts.
            ASIT_InsertRow(insertedFields, tableKeys, uiDataKeys);
        }
        else if (updatedFields > 0) {
            //Only send postback to sync ASIT list.
            var postBackArg = ASIT_BuildPostBackArgument(tableKeys);

            SetNotAsk(true);
            __doPostBack(postBackArg.EventTarget, postBackArg.EventArgument);
        }
    }
}

/* 
* In Super agency environment, if have multiple agency to Insert ASIT Row in onload event, 
*     will generated multiple "RunExpression" scripts statement;
* In order to prevent the Edit form popped repeatedly; need to merge the edit data for each agency.
*/
function ASIT_InsertRow(insertAsitFields, tableKeys, uiDataKeys) {

    if (insertAsitFields.length == 0) {
        return;
    }

    if (IsASITEditForm()) {
        parent.ASITInsertFieldCollection = insertAsitFields;
    } else {
        ASITInsertFieldCollection = insertAsitFields;
    }

    ASIT_AddRow(tableKeys, 1, true, uiDataKeys, null);
}

function getElementByAttribute4ASI(elementId, currentWindow) {
    var result = null;
    elementId = elementId.replace(/'/g, "''");

    var asiWindow = currentWindow;
    var isExist = false;

    while (!isExist && asiWindow) {
        var jqueryElement = asiWindow.$("[expCombinedKey='" + elementId + "']");

        if (jqueryElement == null || jqueryElement.length == 0) {
            /*
            * "asiWindow != asiWindow.parent" can avoid an endless loop.
            * Reproduce steps:
            * 1. Save this popup page as html file in Firefox.
            * 2. Open this html file in Firefox.
            */
            if (asiWindow.$.global.isForPopUp && asiWindow != asiWindow.parent) {
                asiWindow = asiWindow.parent;
            } else {
                break;
            }
        } else {
            result = jqueryElement[0];
            isExist = true;
        }
    }

    return result;
}

function getRealyControl(fieldCtl)
{
    var finalCtl = fieldCtl;

    if (fieldCtl.tagName.toUpperCase() == "DIV" || fieldCtl.tagName.toUpperCase() == "TABLE") 
    {
        var radios = fieldCtl.getElementsByTagName("input");
  
        for (var i = 0; i < radios.length; i++) 
        {
            if (radios[i].type.toUpperCase() == "RADIO" && radios[i].checked) 
            {
                finalCtl = radios[i];
                break;
            }
        }
    }
    
    return finalCtl;
}

function isContaintError(result) {
    
    var isFailed = false;
    
    if (result == undefined || result.length == 0 || result.trim().length == 0) {        
        isFailed = true;        
    }
    else {
        isFailed = result.toUpperCase().indexOf("SERVICE ERROR:") >= 0;
    }

    return isFailed;
}

function DoSubmit() {
    SetNotAsk(true);
    HandleASITResult();

    if (RunningExpCount > 0 || !isSubmitted4Exp) {
        return;
    }

    isSubmitted4Exp = false;

    if (isBlockSubmit) {
        HandleBlockSubmit();
        HideLoading();
        return;
    }

    var regExp = /_/g;
    var callControlID = "";

    if (typeof (btnSubmit) != "undefined" && btnSubmit != null) {
        callControlID = btnSubmit.id.replace(regExp, "$");
    }

    if (callControlID == "") {
        return;
    }

    var options = new WebForm_PostBackOptions(callControlID, "", true, "", "", false, true);
    WebForm_DoPostBackWithOptions(options);

    if (typeof (Page_IsValid) == "undefined" || Page_IsValid) {
        ShowLoading();
    } else {
        HideLoading();
    }
}

function SetReqiredAndMsgInfoByExp(fieldControl,fieldExpression) {

    var strRequired = "<SPAN class='aca_expression_label aca_expression_required font11px' id='strRequiredMK_" + fieldControl.id + "'>*</SPAN><SPAN id='ExpRequiredValidator_" + fieldControl.id + "' stype='display:none'></SPAN>";
    var strMessage = "<SPAN class='aca_expression_label font11px' id='strMessageMK_" + fieldControl.id + "'>" + fieldExpression.message + "</SPAN>";
    
    var isDisplayExpRequired = IsTrue(fieldExpression.required);
    var isRequiredControl = isDisplayExpRequired;
    var isDisplayMessage = (fieldExpression.message != null && fieldExpression.message != "");
    
    var requiredLabel = document.getElementById(fieldControl.id+"_label_0");
    if (requiredLabel && requiredLabel.firstChild !=null) {
        isDisplayExpRequired = false;
        isRequiredControl = true;
    }
    
    if (getAttributeValue(fieldControl, "ishidden") == "true") {
        isDisplayExpRequired = false;
        isRequiredControl = false;
    }
    
     if (isDisplayMessage || isDisplayExpRequired) 
    {       
        // if checkbox, remove the padding
        RemoveMargin4Checkbox(fieldControl);
    }

    // 4. Display required flag in this control
    var fieldLabel = $get(fieldControl.id + "_label_1");

    if (fieldLabel != null) {
        var html = fieldLabel.innerHTML;
        // 4.1. Add control handled to collections
        if(html.indexOf("strRequiredMK") == -1 && 
           html.indexOf("strMessageMK") == -1)
        {
            SetCollection(fieldControl.id, fieldControl.id, html);
        }

        // remove the last empty character
        RemoveSpaceText(fieldLabel);

        if (isDisplayExpRequired && html.indexOf("strRequiredMK") == -1) {
            var tip = getText.global_section508_required + ' ' + $(fieldControl).attr("title");
            $(fieldControl).attr('title', tip.trim());
        }

        // 4.2. Display required flag in this control
        if (isDisplayExpRequired && html.indexOf("strRequiredMK") == -1) {
            // 4.2.1. Expression required flag display behind field label
            fieldLabel.innerHTML = html + strRequired;

            // 4.2.2. Only readonly or disabled control does not need validation
            AddValidationToRequiedControl(fieldControl.id, "ExpRequiredValidator_" + fieldControl.id, true);
        } else if (!isDisplayExpRequired && !isRequiredControl) {
            if ($('#' + fieldControl.id + '_err_indicator').is(':visible')) {
                // For unrequired field, the required error and message should be hidden.
                doErrorCallbackFun("", fieldControl.id, 0);
            }
        }
    }
    
    // 5. Display message in this control.
    // Note: 1) In the Vertical layout, it displays behind Field Label;
    //       2) In the Horizontal layout, it displays behind Field Unit Label;
    var messageLabel = $get(fieldControl.id + "_label_exp");

    if (messageLabel != null) 
    {
        if (isDisplayMessage) {
            messageLabel.innerHTML = strMessage;
        } else {
            messageLabel.innerHTML = "";
        }
    }

    return { isDisplayExpRequired: isDisplayExpRequired, isRequiredControl: isRequiredControl, isDisplayMessage: isDisplayMessage };
}

function SetControlHiddenByExp(fieldControl, fieldExpression) {
    var isHidden = IsTrue(fieldExpression.hidden);
    
    // If the field is set None to security property, it should be always hidden.
    if (getAttributeValue(fieldControl, "SecurityType") == "N") {
        isHidden = true;
        fieldExpression.hidden = true;
    }

    if (getAttributeValue(fieldControl, "ishidden") == "true") {
        isHidden = true;
    }
    
    // 8. Hidden or display field control
    SetFieldDispalyStyle(fieldControl, isHidden);

    var isContactPermissionFld = /^(CONTACT\d*|APPLICANT)::(contactsModel\d*|applicant)\*accessLevel$/i.test(fieldExpression.variableKey);
    
     if (isContactPermissionFld && fieldControl.attributes["CustomListControlID"] != null) {
         //Fill custom checkbox list for access permission level control.
         var contactPermssionCheckList = $get(fieldControl.attributes["CustomListControlID"].value);
         
         if (contactPermssionCheckList) {
             
             // Set hidden status for contact permission control.
             SetFieldDispalyStyle(contactPermssionCheckList, isHidden);
         }
     }

    return isHidden;
}

function SetControlReadOnlyByExp(fieldControl, fieldExpression) {

    var isReadOnly = null;

    /*
    if the control readonly attribute is set as true by the expression,
    it should add the ReadOnlyByExp attribute
    else should remove the ReadOnlyByExp attribute
    */

    if (IsTrue(getAttributeValue(fieldControl, "ReadOnlyByExp"))) {
        if (!IsTrue(fieldExpression.readOnly)) {
            isReadOnly = false;
            $(fieldControl).removeAttr("ReadOnlyByExp");
        }
        else if (!$(fieldControl).hasClass("ACA_ReadOnly")) {
            isReadOnly = true;
        }
    } else {
        if (!$(fieldControl).hasClass("ACA_ReadOnly") && IsTrue(fieldExpression.readOnly)) {
            isReadOnly = true;
            $(fieldControl).attr("ReadOnlyByExp", true);
        }
    }

    //if isReadOnly is null,means keeping the readonly state
    if (isReadOnly != null) {

        var isContactTypeFlag = /^((CONTACT\d*|APPLICANT)::(contactsModel\d*|applicant)\*contactTypeFlag|(REFCONTACT|CUSTOMER)::contactTypeFlag)$/i.test(fieldExpression.variableKey);
        // 7. Set Control's Status when it is readonly
        var fieldExpType = isContactTypeFlag ? ExpressionControlType.HTML_SELECTBOX : fieldExpression.type;
        SetReadOnlyProperty(fieldControl, fieldExpType, isReadOnly);

        // Set readonly status for contact permission control.
        var isContactPermissionFld = /^(CONTACT\d*|APPLICANT)::(contactsModel\d*|applicant)\*accessLevel$/i.test(fieldExpression.variableKey);

        if (isContactPermissionFld && fieldControl.attributes["CustomListControlID"] != null) {
            //Fill custom checkbox list for access permission level control.
            var contactPermssionCheckList = $get(fieldControl.attributes["CustomListControlID"].value);

            if (contactPermssionCheckList) {

                var checkboxes = contactPermssionCheckList.getElementsByTagName("input");

                for (var i = 0; i < checkboxes.length; i++) {
                    SetReadOnlyProperty(checkboxes[i], ExpressionControlType.HTML_CHECKBOX, isReadOnly);
                }
            }
        }
    }

    return $(fieldControl).hasClass("ACA_ReadOnly");
}

/***********************************************************************************************
*
* Func: SetResultValueToControl
* Desc: Apply some settings in expression to control
* Para: fieldControl       -    Field Value Control
*       fieldExpression    -    Field Expression Model
***********************************************************************************************/
function SetResultValueToControl(fieldControl, fieldExpression) {
    // 1. Check func's parameters to ensure them not null

    /*
    Some fields disallow to change by expression. such as:
    1. Contact Type in sincle contact section.
    2. validateFlag in contact address section.
    */
    var isFieldDisallowToChange = /^((CONTACT\d*|APPLICANT)::(contactsModel\d*|applicant)\*contactType|REFCONTACT::contactType|CONTACTADDR::validateFlag)$/i.test(fieldExpression.variableKey);

    if (fieldControl == null || fieldExpression == null || isFieldDisallowToChange) {
        return;
    }

    var reqiredAndMsgInfoObj = SetReqiredAndMsgInfoByExp(fieldControl, fieldExpression);
    SetControlHiddenByExp(fieldControl, fieldExpression);
    SetControlReadOnlyByExp(fieldControl, fieldExpression);

    /*
    adjust field shadow div's width if the field relarge.
    the shadow div is used to prevent mouse click for SELECT element.
    */
    var shadeDiv = $('#' + fieldControl.id + '_div');

    if (shadeDiv && shadeDiv.width() < fieldControl.offsetWidth) {
        shadeDiv.css("width", fieldControl.offsetWidth + 2);
    }

    var isDisplayExpRequired = false;

    if (reqiredAndMsgInfoObj) {
    isDisplayExpRequired = reqiredAndMsgInfoObj.isDisplayExpRequired;
    }
    
    /*
    Contact Gender field: /^(CONTACT\d*|APPLICANT)::(contactsModel\d*|applicant)\*gender$/i
    Contact AccessPermissionLevel: /^(CONTACT\d*|APPLICANT)::(contactsModel\d*|applicant)\*accessLevel$/i
    Because the field-type of the Gender/AccessPermissionLevel field is incorrect in v360(view id: 121/123/124/125).
    So there is a special logic to handle the Gender field and AccessPermissionLevel field of the Contact section.
    Special logic for ContactTypeFlag: In AA, field type is radion button list. In ACA, field type is Dropdown list.
    */
    var isContactGenderFld = /^((CONTACT\d*|APPLICANT)::(contactsModel\d*|applicant)\*gender|(REFCONTACT|CUSTOMER)::gender)$/i.test(fieldExpression.variableKey);
    var isContactPermissionFld = /^(CONTACT\d*|APPLICANT)::(contactsModel\d*|applicant)\*accessLevel$/i.test(fieldExpression.variableKey);
    var isContactTypeFlag = /^((CONTACT\d*|APPLICANT)::(contactsModel\d*|applicant)\*contactTypeFlag|(REFCONTACT|CUSTOMER)::contactTypeFlag)$/i.test(fieldExpression.variableKey);
    var contactPermssionCheckList;

    // 6. Set expression value to the field value control. 
    /* If the control is disabled, the expression value won't be stored in database*/
    if (fieldExpression.value != null && fieldControl.disabled != true) {
        if (!isContactTypeFlag && (ExpressionControlType.HTML_RADIOBOX == fieldExpression.type || isContactGenderFld || isContactPermissionFld)) {
            //To correct the field type.
            fieldExpression.type = ExpressionControlType.HTML_RADIOBOX;

            // 6.1 Radio Button List
            var radios = fieldControl.getElementsByTagName("input");
            var isContactCustomPermission = false;
            var expValue = fieldExpression.value == null ? '' : fieldExpression.value.toUpperCase();

            for (var i = 0; i < radios.length; i++) {
                var radio = radios[i];
                var itmValue = radio.value.toUpperCase();
                var checked = IsYes(itmValue, expValue);

                if (isContactPermissionFld) {
                    checked = expValue.indexOf(itmValue) > -1;

                    if (checked && itmValue == 'C') {
                        isContactCustomPermission = true;
                    }
                }

                radio.checked = checked;

                if ($.browser.msie && checked == true) {
                    radio.defaultChecked = true;
                }

                if (checked) {
                    break;
                }
            }

            if (isContactPermissionFld && fieldControl.attributes["CustomListControlID"] != null) {
                //Fill custom checkbox list for access permission level control.
                contactPermssionCheckList = $get(fieldControl.attributes["CustomListControlID"].value);
                ContactRelationShipsRadioListChange(fieldControl.id, contactPermssionCheckList.id);

                if (contactPermssionCheckList) {
                    var checkboxes = contactPermssionCheckList.getElementsByTagName("input");

                    for (var i = 0; i < checkboxes.length; i++) {
                        var itmValue = checkboxes[i].value.toUpperCase();
                        checkboxes[i].checked = isContactCustomPermission && expValue.indexOf(itmValue) > -1;
                    }
                }
            }
        }
        else if (isContactTypeFlag
            || ExpressionControlType.HTML_SELECTBOX == fieldExpression.type && fieldControl.tagName.toUpperCase() == 'SELECT') {

            //For Dropdown list fields
            var valueIdx = fieldControl.selectedIndex;
            var valueList = fieldControl.options;

            for (var i = 0; i < valueList.length; i++) {
                if (valueList[i].value == fieldExpression.value
                    || valueList[i].text == fieldExpression.value) {
                    valueIdx = i;
                    $(valueList[i]).removeAttr('disabled');
                    break;
                }
                else if (fieldExpression.readOnly && (typeof ($(valueList[i]).attr('disabled')) == "undefined" || !$(valueList[i]).attr('disabled'))) {
                    $(valueList[i]).attr('disabled', 'disabled');
                }
            }

            fieldControl.selectedIndex = valueIdx;
        }
         else if (typeof (fieldControl.type) != "undefined" && fieldControl.type == "checkbox") {
            //For CheckBox fields.
            var chk = IsYes("Yes", fieldExpression.value);
            fieldControl.checked = chk;

            if ($.browser.msie && chk == true) {
                fieldControl.defaultChecked = true;
            }
        }
        else {
            var isHijriCalendar = isHijriCalendarControl(fieldControl.id);

            if (isHijriCalendar) {
                if (fieldExpression.value) {
                    var dateFormat = $(fieldControl).attr("format");
                    var dateValue = Date.parseLocale(fieldExpression.value, dateFormat);
                    if (dateValue != null) {
                        var curAdjustDate = new AdjustDate({ isHijriDate: true, gDate: dateValue });
                        SetValue(fieldControl, curAdjustDate.localeFormat(dateFormat));
                        $(fieldControl).attr("gDate", fieldExpression.value);
                    } else {
                        SetValue(fieldControl, "");
                        $(fieldControl).attr("gDate", "");
                    }
                } else {
                    SetValue(fieldControl, "");
                    $(fieldControl).attr("gDate", "");
                }
            } else {
                // 6.2 Other field control but Radio Button List
                SetValue(fieldControl, fieldExpression.value);
            }

            // 6.3 For Number and Currency Control
            if (fieldExpression.type == ExpressionControlType.HTML_TEXTBOX_OF_NUMBER
               || fieldExpression.type == ExpressionControlType.HTML_TEXTBOX_OF_CURRENCY) 
            {
                var fldval = GetValue(fieldControl);
                SetValue(fieldControl, fldval);
            }
            
            // 6.4 For Check Box
            if (typeof (fieldControl.type) != "undefined" 
                && fieldControl.type == "checkbox") 
            {
                var chk = IsYes("Yes", fieldControl.value);
                fieldControl.checked = chk;                

                if($.browser.msie && chk == true){
                    fieldControl.defaultChecked=true;
                }
            }
        }
    }
    
    //if dropdownlist control, Only one choice is available in a dropdown and required “—Select—“ will not be displayed.
    if (IsDropDownListCtl(fieldControl)){
        SetAvailableItemSelected(isDisplayExpRequired, fieldControl);
        TriggerDropDownOnchange(fieldControl);
    }
    
    // 9. Apply Expression to Hidden Control
    SetResultParamsToHiddenField(fieldControl.id, isDisplayExpRequired, fieldExpression);
}

/***********************************************************************************************
 *
 * Func: SetReadOnlyProperty
 * Desc: 
 *       Have these control types Dropdown-List, CheckBox, RadioButton-List and Calendar Date Read Only
 * Para: 
 *       fieldValueCtl      -   Field Control  
 *       controlType        -   Field Control Type
 *       isReadOnly         -   Is read Only
 ***********************************************************************************************/
function SetReadOnlyProperty(fieldValueCtl, controlType, isReadOnly) 
{
    if (typeof fieldValueCtl == "undefined" || fieldValueCtl == null || isReadOnly == null)
    {
        return;
    }

    fieldValueCtl.readOnly = isReadOnly;

    var enabled = (fieldValueCtl.disabled != true);   
       
    // 1) Dropdown List Control
    if (ExpressionControlType.HTML_SELECTBOX == controlType) {

        SetFieldToDisabled(fieldValueCtl.id, isReadOnly);

    }
    // 2) CheckBox Control
    else if (ExpressionControlType.HTML_CHECKBOX == controlType) {

        if (enabled) {
            fieldValueCtl.parentNode.disabled = false;
        }

        SetFieldToDisabled(fieldValueCtl.id, isReadOnly);
    }
    // 3) Radio Button List Control
    else if (ExpressionControlType.HTML_RADIOBOX == controlType) {

        //Expression : the radio' container(div) should set ACA_ReadOnly
        isReadOnly = GetReadonlyStatus(fieldValueCtl, isReadOnly);
        SetFieldToDisabled(fieldValueCtl.id, isReadOnly);
        
        var radios = fieldValueCtl.getElementsByTagName("input");

        for (var j = 0; j < radios.length; j++) {

            var radio = radios[j];
            radio.readOnly = isReadOnly;

            if (enabled) {
                radio.disabled = false;
                radio.parentNode.disabled = false;
            }
        }
    }
    // 4) Calendar Date Control and Text and TextArea
    else {

        var calendarImg = $get(fieldValueCtl.id + "_calendar_button");
        if (enabled && calendarImg != null) {
            calendarImg.disabled = false;
        }

        SetFieldToDisabled(fieldValueCtl.id, isReadOnly);
    }    
}

function  CheckLicenseLocked(resultCtl) {
    if (resultCtl==null || resultCtl.variableKey==null) {
        return false;
    }
    
    var keys=resultCtl.variableKey.toUpperCase().split('::');
    var isLicenseLocked=false;
    if (keys.length>1 && keys[0]=="LP" && IsLockLicense) {
        isLicenseLocked=true;
    }
    
    return isLicenseLocked;
}

/***********************************************************************************************
 *
 * Func: ReSetControlParams
 * Desc: Reset control's properties, such as readonly, required and message, when the update panel is changed.
 * Para: N/A
 ***********************************************************************************************/
function ReSetControlParams() {
    // 1. Get Expression model from special hidden control whose ID is "ctl00_PlaceHolderMain_HDExpressionParam"
    var $hdExpression = GetExpResultStoreField();

    if ($hdExpression.length == 0 || $hdExpression.val() == "") {
        return;
    }

    // 2. Get field's expressions collestion.
    var fieldExpressions = Sys.Serialization.JavaScriptSerializer.deserialize($hdExpression.val());

    // 3. Apply each field expression.
    for (var i = 0; i < fieldExpressions.length; i++) {
        var fieldExpression = fieldExpressions[i];

        // 3.1. Check if  field expression model is null
        if (fieldExpression == null) {
            continue;
        }

        // 3.2. Get Field's control ID 
        var clientID = fieldExpression.name;

        // 3.3. Get field's value, label and message label
        var fieldValueCtl = $get(clientID);

        if (fieldValueCtl == null) {
            continue;
        }

        var reqiredAndMsgInfoObj = SetReqiredAndMsgInfoByExp(fieldValueCtl, fieldExpression);
        SetControlHiddenByExp(fieldValueCtl, fieldExpression);
        SetControlReadOnlyByExp(fieldValueCtl, fieldExpression);

        if (fieldValueCtl.type == "checkbox" && !fieldValueCtl.checked) {
            var checked = fieldExpression.IsChecked;
            fieldValueCtl.checked = checked;
        }

        RemoveSpaceText(fieldValueCtl);

        //if dropdownlist control, Only one choice is available in a dropdown and required “—Select—“ will not be displayed.
        if (IsDropDownListCtl(fieldValueCtl) && IsTrue(reqiredAndMsgInfoObj.isDisplayExpRequired) || IsTrue(reqiredAndMsgInfoObj.isRequiredControl)) {
            SetAvailableItemSelected(true, fieldValueCtl);
        }
    }
}

//Only one choice is available in a dropdown and required “—Select—“ will not be displayed.
function SetAvailableItemSelected(isRequired, fieldValueCtl){
    if (isRequired && fieldValueCtl != null && fieldValueCtl.length == 2){
        if (fieldValueCtl.options !=  null && fieldValueCtl.options.length == 2 && typeof(defaultSelectText) != 'undefined' && fieldValueCtl.options[0].text ==defaultSelectText && fieldValueCtl.options[0].value == '')
        {
            fieldValueCtl.remove(0);
            return true;
        }
    }
    
    return false;
}

//The flag of whether dropdownlist control?
function IsDropDownListCtl(fieldValueCtl){
    if (fieldValueCtl != null && fieldValueCtl.tagName != null && fieldValueCtl.tagName.toUpperCase() == "SELECT"){
        return true;
    }
    
    return false;
}

//Trigger drop down list on change event.
function TriggerDropDownOnchange(fieldControl){
    if (fieldControl != null && fieldControl.attributes['onchange'] != null && fieldControl.attributes['onchange'].value.indexOf('GetNextDrillDown(this);') > -1 && typeof (GetNextDrillDown) != 'undefined') {
        GetNextDrillDown(fieldControl);
    }
}

function SetFieldParentVisible(element, visible) {
    try {
        var tr = element.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode;
        if (tr.tagName != "TR")//only one element
        {
            element.parentNode.parentNode.parentNode.parentNode.parentNode.style.display = visible ? '' : 'none';
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
                    ev = tempChildNode.childNodes[1].childNodes[0].style.display != 'none';
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
    catch (e) {
    }
}

function GetChildNode(parentNode) 
{
    var returnNode = null;

    if (parentNode != null) 
    {
        for (var i = 0; i < parentNode.childNodes.length; i++) 
        {
            if (!parentNode.childNodes[i].nodeValue) 
            {
                returnNode = parentNode.childNodes[i];
            }
        }
    }

    return returnNode;
}

/***********************************************************************************************
 *
 * Func: SetResultParamsToHiddenField
 * Desc: 
 *       
 * Para: 
 *       fieldControlID     -   
 *       isRequired         -       
 *       isReadOnly         -
 *       fieldExpression    -
 *   
 ***********************************************************************************************/
function SetResultParamsToHiddenField(fieldControlID, isRequired, fieldExpression) {
    if (String(fieldExpression.viewID) == ExpressionType.License_Professional && CheckLicenseLocked(fieldExpression)) {
        return;
    }
    
    var ctrl = $get(fieldControlID);
    var checked = false;
    if (ctrl && ctrl.type == "checkbox") {
        checked = ctrl.checked;
    }
    
    var resultParams = {    
                            "name": fieldControlID,
                            "required": isRequired,
                            "readOnly": fieldExpression.readOnly,
                            "message": fieldExpression.message,
                            "type": fieldExpression.type,
                            "hidden": fieldExpression.hidden, 
                            "IsChecked":checked
                        };

    var $hdExpression = GetExpResultStoreField();

    if ($hdExpression.length == 0) {
        return;
    }

    if ($hdExpression.val() == "") {
        $hdExpression.val("[" + Sys.Serialization.JavaScriptSerializer.serialize(resultParams) + "]");
        return;
    }

    var results = Sys.Serialization.JavaScriptSerializer.deserialize($hdExpression.val());
    var hasSameID = false;
    
    for (var i = 0; i < results.length; i++) {
    
        if (results[i].name == resultParams.name) 
        {
            results[i].required = resultParams.required;
            results[i].readOnly = resultParams.readOnly;
            results[i].message = resultParams.message;
            results[i].type = resultParams.type;
            results[i].hidden = resultParams.hidden;
            
            hasSameID = true;            
        }
        
    }

    if (!hasSameID) {
        Array.enqueue(results, resultParams);
    }

    $hdExpression.val(Sys.Serialization.JavaScriptSerializer.serialize(results));
}

function GetExpResultStoreField() {
    var $epParamObj = $("input[class='HDExpressionResultCss']");
    return $epParamObj;
}

/***********************************************************************************************
*
* Func: ClearMessageAndRequireChar
* Desc: 
*       Clear expression's message and reqired flag 
* Para: 
*       inputFieldNames        -       Field Name List 
*       ExpressionType         -       Expression Type
***********************************************************************************************/
function ClearMessageAndRequireChar(inputFieldNames, expressionType) {
    for (var par in inputFieldNames) {
        if (expressionType != -1) {
            continue;
        }

        for (obj in ObjectCollection) {
            if (inputFieldNames[par] == ObjectCollection[obj].Name) {
                RemoveRequiredValidation(ObjectCollection[obj]);
            }
        }
    }
}

/***********************************************************************************************
 *
 * Func: RemoveRequiredValidation
 * Desc: 
 *       Clear expression's message and reqired flag 
 * Para: 
 *       Parameters             -       Field Control Object
 ***********************************************************************************************/
function RemoveRequiredValidation(objControlCtl)
{
    if(objControlCtl == null || typeof objControlCtl == "undefined" ||
       objControlCtl.ControlClientID == null || 
       typeof objControlCtl.ControlClientID == "undefined")
    {
        return;
    }
    
    var clientID = objControlCtl.ControlClientID;

    // 1.1. Get Field's value control and label control
    var fieldValueCtl = $get(clientID);
    var fieldLabelCtl = $get(clientID + "_label_1");
    
    // 1.2. Reset field label's text                
    if (fieldLabelCtl != null && typeof fieldLabelCtl != "undefined") {
        var html = fieldLabelCtl.innerHTML;

        if (html.indexOf("strRequiredMK") > -1 && !isNull(fieldValueCtl) && !isNullOrEmpty($(fieldValueCtl).attr("title"))) {
            var tip = $(fieldValueCtl).attr("title").replace(getText.global_section508_required, '');
            $(fieldValueCtl).attr('title', tip.trim());
        }

        fieldLabelCtl.innerHTML = objControlCtl.ControlText;
    }

    // 1.4. If field control does not exists, return
    if (fieldValueCtl == null || typeof fieldValueCtl == "undefined") {
        return;
    }

    // 1.5. Remove required validation expression.
    RemoveValidationFromRequiredControl(clientID, "ExpRequiredValidator_" + clientID);
                    
    // 1.6. Remove this control from ObjectCollection
    Array.remove(ObjectCollection, objControlCtl);
}

/***********************************************************************************************
 *
 * Func: SetCollection
 * Desc: 
 *       Store the orignal control label into ObjectCollection
 * Para: 
 *       controlID      -   Field Control ID
 *       fieldName      -   Field Control Name  
 *       controlText    -   Field Control Label
 ***********************************************************************************************/
function SetCollection(controlID, fieldName, controlText)
{
    var obj={
                "ControlClientID": controlID,
                "Name": fieldName,
                "ControlText": controlText
            };

    Array.add(ObjectCollection,obj);
}

function EncoderSpecialChars(strings) {
    strings = strings.replace("\\", "\\\\");
    strings = strings.replace("\"", "\\\"");
    return strings;
}

if (typeof (Sys) != "undefined") {
    Sys.Application.notifyScriptLoaded();
}

//show message at the top of the current page.
//if the result contains more than two message, only show the first one.
function HandleBlockSubmit() {
    if (ValidationMessage != null && ValidationMessage != "") {

        if (!isCrossDomain() && typeof (window.parent) != "undefined" && window.parent.location.href.indexOf("isPopup") > -1) {
            showMessage("messageSpan", ValidationMessage, 'Error', true, 1, true);

            // two-layer popup(e.g. contact address), it's parent window call scrollTo method to position the message.
            var scrollToHeight = $(window.parent.document).height() - $(window.document).height();
            window.parent.scrollTo(0, scrollToHeight);

        }
        else {
            showNormalMessage(ValidationMessage, 'Error');
        }
    }

    isBlockSubmit = false;
}

function HandleNormalResult(finalResultFields) {
    if (finalResultFields == null || finalResultFields.length == 0) {
        return;
    }

    var clearInputFieldNames = new Array();

    for (var i = 0; i < finalResultFields.length; i++) {
        var oneResult = finalResultFields[i];
        Array.add(clearInputFieldNames, oneResult.name);
    }

    ClearMessageAndRequireChar(clearInputFieldNames, -1);

    for (var i = 0; i < finalResultFields.length; i++) {
        var field = finalResultFields[i];
        var ctl = $get(field.name);

        if (ctl != null) {
            SetResultValueToControl(ctl, field);
        }
    }
}

function CheckValidationResult(finalResult) {
    var isBlockByValidation = false;
    var formKeyMessage = new Object();

    for (var i = 0; i < finalResult.fields.length; i++) {
        var oneResult = finalResult.fields[i];
        var splits = oneResult.variableKey.toUpperCase().split('::');

        //LP::FORM, ASI:FORM, ASIT::GroupName::FORM
        if ((splits.length == 2 && splits[1] == "FORM") || (splits.length == 3 && splits[2] == "FORM")) {
            var groupPrefix = splits[0];

            if (oneResult.blockSubmit && (groupPrefix != "LP" || !IsLockLicense)) {
                isBlockByValidation = true;

                if (oneResult.message == null || oneResult.message == "") {
                    continue;
                }

                if (oneResult.viewID == ExpressionType.ASI || oneResult.viewID == ExpressionType.ASI_Table) {
                    formKeyMessage[oneResult.viewID] = oneResult.message;
                } else {
                    formKeyMessage[oneResult.viewID] = oneResult.message + "<br/>";
                }
            }
        }
    }

    for (var prop in formKeyMessage) {
        ValidationMessage += formKeyMessage[prop];
    }

    return isBlockByValidation;
}

//hidden parent table
//if has "parent_table" attribute, only hide it
//else hide the first parent table
function SetFieldDispalyStyle(ctrl,displayStyle) {
    var style=displayStyle;
    var visibilityStyle="";
    
    if (typeof displayStyle=="boolean") {
        style = displayStyle ? "none" : "";
        visibilityStyle=displayStyle?"hidden":"";
    }else{
        if (displayStyle=="none") {
            visibilityStyle="hidden";
        }
    }

    // get the parent grid which generate from 'Form Designer'.
    var gridParentID = "";
    var gridParent = null;
    if (ctrl && ctrl.id) {
        gridParentID = ctrl.id + "_parentGrid";
        gridParent = $get(gridParentID);
    }

    if (gridParent != null) {
        gridParent.style.display = style;
    }

    // get the control's table container
    var containerID = "";
    var container = null;
    if (ctrl && ctrl.id) {
        containerID = ctrl.id+"_table";
        container = $get(containerID);

        // For resolved the issue #49009, if current validate control is hidden, it needn't to be focus when validate fail.
        if ((style == "none" || visibilityStyle == "hidden") && typeof (Page_InvalidControlToBeFocused) != "undefined") {
            Page_InvalidControlToBeFocused = null;
        }
    }

    if (container != null) {
        container.style.display=style;

        if (getAttributeValue(ctrl, "isasi") || getAttributeValue(ctrl, "isasitcontrol")) {
            var asiTable = container.parentNode.offsetParent;

            //offsetParent can't get table in IE8, FF3.6 and SAFARI
            if (asiTable == null || asiTable.nodeName != 'TABLE') {
                asiTable = container.parentNode.parentNode.parentNode.parentNode;
            }

            if (getAttributeValue(asiTable, "columnArrangement")) {
                // asi layout with two columns
                // need re-layout ASI table when meet the below coditions:
                // 1)hide the control (displayStyle == true)
                // 2)show the control and control cell is hiden(asiCellCls == "ACA_Hide")
                var asiCellCls = container.parentNode.className;

                if (displayStyle || (!displayStyle && asiCellCls == "ACA_Hide")) {
                    container.parentNode.className = displayStyle ? "ACA_Hide" : "";
                    LayoutASI(asiTable);
                }
            }
            else if (container.parentNode && container.parentNode.nodeName == 'DIV') {
                // asi layout with one column
                container.parentNode.className = displayStyle ? "ACA_Hide" : "ACA_Show";
            }
        }

        return;
    }

    if ($(ctrl).parents("[attr='parent_table']").length==0) {
        if ($(ctrl).parents("[parentcontrol]").length>0) {
            $(ctrl).parents("[parentcontrol]").each(function () {
                this.style.display=style;
                return false;
            });
        }else if ($(ctrl).parents("[isasitcontrol]").length>0) {
            // support the checkbox in asi table
           $(ctrl).parents("[isasitcontrol]").each(function () {
                this.style.display=style;
                return false;
            });
        }else{
             $(ctrl).parents().each(function () {
                if (this.nodeName.toUpperCase()=="TABLE") {
                    this.style.display=style;
                    return false;
                }
            });
        }
    }else{
        $(ctrl).parents("[attr='parent_table']").each(function () {
            this.style.display=style;
            return false;
        });
    }
}

/***********************************************************************************************
 *
 * Func: getAttributeValue
 * Desc: 
 *       This function support to get customized attribute value under Firefox and IE
 * Para:
 *      control       -     control object
 *      attributeName -     attribute Name     
 ***********************************************************************************************/
function getAttributeValue(control, attributeName)
{
    if(control == null || control.attributes[attributeName] == "undefined" || control.attributes[attributeName] == null)
    {
        return null;
    }
    
    return control.attributes[attributeName].value;
}

function RemoveMargin4Checkbox(fieldControl) {
    if (fieldControl && fieldControl.type == "checkbox") {
        var label = fieldControl.nextSibling;
        if (label != null && typeof (label) != 'undefined' && label.tagName.toUpperCase() == "LABEL") {
            $.global.isRTL ? $(label).css({ "margin-left": "2px" }) : $(label).css({ "margin-right": "2px" });
        }

        var outerDiv = fieldControl.offsetParent;
        if (outerDiv && outerDiv.tagName == 'TD') {
            outerDiv = outerDiv.firstChild;
        }
        if (outerDiv && outerDiv.tagName == 'DIV') {
            $.global.isRTL ? $(outerDiv).css({ "margin-left": "2px" }) : $(outerDiv).css({ "margin-right": "2px" });
        }
    }
}
 
 function IsYes(fieldValue, value) {
    if (typeof(value) !="string" || isNullOrEmpty(fieldValue) || isNullOrEmpty(value)) {
        return false;
    }

    var checkValue = value.toUpperCase();

    if (checkValue == "CHECK"
    || checkValue == "CHECKED"
    || checkValue == "TRUE"
    || checkValue == "SELECT"
    || checkValue == "SELECTED"
    || checkValue == "ON"
    || checkValue == "YES"
    || checkValue == "Y") {
        value = "Yes";
    }
    else if (checkValue != "FEMALE"
        && checkValue != "F"
        && checkValue != "MALE"
        && checkValue != "M") {
        value = "No";
    }

    return fieldValue.substring(0,1).toUpperCase() == value.substring(0,1).toUpperCase();
 }

 function RemoveSpaceText(fieldLabel) {
     if (!fieldLabel) {
         return;
     }
     var html = fieldLabel.innerHTML;

     if (html != "" && html.length > 0) {
         var space = html.substring(html.length - 1, html.length);
         if (space == " ") {
             fieldLabel.innerHTML = html.substring(0, html.length - 1);
         }
     }
 }

// Re-layout ASI/ASIT table if it has control(s) hidden.
function LayoutASI(table) {
    if (!table) {
        return; 
    }

    var arrVisibleTD = new Array();
    var arrHiddenTD = new Array();

    for (var i = 0; i < table.rows.length; i++) {
        for (var j = 0; j < table.rows[i].cells.length; j++) {
            if (table.rows[i].cells[j].className == 'ACA_Hide') {
                arrHiddenTD.push(table.rows[i].cells[j]);
            }
            else {
                arrVisibleTD.push(table.rows[i].cells[j]);
            }
        }
    }

    arrVisibleTD.sort(function (td1, td2) {
        return parseFloat(td1.attributes["index"].value) - parseFloat(td2.attributes["index"].value);
    });

    var newTable;

    var columnArragement = table.attributes["columnArrangement"].value;
    var columnLayout = parseInt(table.attributes["columnLayout"].value);
    if (columnArragement == "Horizontal") {  
        // table column layout with horizontal
        newTable = CreateTableAsHorizonStyle(arrVisibleTD, arrHiddenTD, columnLayout);
    }
    else {
        newTable = CreateTableAsVerticalStyle(arrVisibleTD, arrHiddenTD, columnLayout);
    }

    var tbody = table.children[0];
    if (newTable.children[0]) {
        table.replaceChild(newTable.children[0], tbody);
    }
}

function CreateTableAsHorizonStyle(arrVisibleTD, arrHiddenTD, columnLayout) {
    var arrTD = new Array();
    
    for (var i = 0; i < arrVisibleTD.length; i++) {
        arrTD.push(arrVisibleTD[i]);
    }
    for (var i = 0; i < arrHiddenTD.length; i++) {
        arrTD.push(arrHiddenTD[i]);
    }

    var table = document.createElement('table');

    for (var i = 0; i < arrTD.length; i++) {
        var rowIndex = parseInt(i / columnLayout);

        if (i % columnLayout == 0) {
            // add a new row
            table.insertRow(rowIndex);
        }
        table.rows[rowIndex].appendChild(arrTD[i]);
    }

    return table;
}

function CreateTableAsVerticalStyle(arrVisibleTD, arrHiddenTD, columnLayout) {
    var table = document.createElement('table');

    if (!arrVisibleTD || arrVisibleTD.length == 0 || !arrHiddenTD || arrHiddenTD.length == 0) {
        return table;
    }

    var visibleRowLen = Math.ceil(arrVisibleTD.length / columnLayout);

    for (var i = 0; i < arrVisibleTD.length; i++) {
        if (i < visibleRowLen) {
            //inert row and cell
            table.insertRow(i);
            table.rows[i].appendChild(arrVisibleTD[i]);
        }
        else {
            table.rows[i - visibleRowLen].appendChild(arrVisibleTD[i]);
        }
    }

    //insert a hidden cell
    if (arrVisibleTD.length % columnLayout != 0) {
        table.rows[visibleRowLen - 1].appendChild(arrHiddenTD[0]);
        arrHiddenTD.shift();
    }
    
    // append hidden cells to the table.
    for (var i = 0; i < arrHiddenTD.length; i++) {
        var rowIndex = parseInt(i / columnLayout) + visibleRowLen;

        if (i % columnLayout == 0) {
            table.insertRow(rowIndex);
        }
        
        table.rows[rowIndex].appendChild(arrHiddenTD[i]);
    }

    return table;
}

function initialSubmit4Exp(hasSubmitExpressionJs) {
    ValidationMessage = "";
    isBlockSubmit = false;
    hideMessage();
    isSubmitted4Exp = true;

    if (IsTrue(hasSubmitExpressionJs) || RunningExpCount > 0) {
        ShowLoading();
    }
}

function finishSubmit4Exp() {
    // RunningExpCount means that it's not running expression now.
    if (RunningExpCount <= 0) {
        return true;
    } else {
        return false;
    }
}