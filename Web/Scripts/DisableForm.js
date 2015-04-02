/**
 * <pre>
 * 
 *  Accela
 *  File: DisableForm.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: DisableForm.js 255414 2013-08-02 12:19:52Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *
 * </pre>
 */

function disableForm(form, chkAutoFill, ddlAutoFill, isDisable) {
    var controlType = ['input', 'select', 'img', 'textarea'];

    for (var i = 0; i < controlType.length; i++) {
        var inputs = form.getElementsByTagName(controlType[i]);

        if (inputs == null || inputs.length < 1) {
            continue;
        }

        for (var j = 0; j < inputs.length; j++) {
            if (chkAutoFill == null || ddlAutoFill == null || chkAutoFill.id == inputs[j].id || ddlAutoFill.id == inputs[j].id) {
                continue;
            }

            /*
            The "data-editable" attribute:
            Usage 1:
                People template has a Always Editable setting,
                    so if "data-editable" is "true", means the field always keep the Editable status.
            Usage 2:
                Standard field can be set as Always Readonly in ACA Admin, such as Contact Edit form,
                    so if "data-editable" is "false", means the standard field always keep the Readonly status.

            So if one field has the "data-editable" attribute, always keep current status.
            */

            if (typeof ($(inputs[j]).attr('data-editable')) != "undefined"
                || $(inputs[j]).attr('ValidateDisabledControl') == 'Y'
                //the control in AKA not need enable/disable in this method.
                || typeof($(inputs[j]).attr("isAKA")) != "undefined") {
                continue;
            }

            if (inputs[j].type && inputs[j].type.toUpperCase() == "RADIO") {
                var radioField = inputs[j].parentNode;

                //Field is readonly by ASI security or has the "data-editable" attribute, keep current status.
                if (radioField
                    && ($(radioField).attr('ValidateDisabledControl') == 'Y' || typeof ($(radioField).attr('data-editable')) != "undefined")){
                    continue;
                }
            }

            if (typeof (inputs[j].type) == "undefined") {
                inputs[j].disabled = isDisable;

                if (isDisable) {
                    $(inputs[j]).addClass("ACA_ReadOnly");
                }
                else {
                    $(inputs[j]).removeClass("ACA_ReadOnly");
                }
            }
            else {
                var inputType = inputs[j].type.toUpperCase();

                switch (inputType) {
                    case "HIDDEN":
                        break;
                    default://TEXT,"SELECT-ONE","RADIO"
                        SetFieldToDisabled(inputs[j].id, isDisable);
                        break;
                }
            }
        }
    }
}
