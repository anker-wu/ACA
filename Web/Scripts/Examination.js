/**
* <pre>
* 
*  Accela
*  File: Examination.js
* 
*  Accela, Inc.
*  Copyright (C): 2011-2013
* 
*  Description:
* To deal with some logic for Education.
*  Notes:
* $Id: Education.js 176611 2011-09-20 12:09:02Z ACHIEVO\daniel.shi $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

function SetGroupName(gridid) {
    var inputs = document.getElementById(gridid).getElementsByTagName('input');
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].type == 'radio') {
            if (inputs[i].name != "grpAvailableExamination") {
                inputs[i].name = "grpAvailableExamination";
            }
        }
    }
}

function SelectExaminationRadio(obj, gridid, hiddenid) {
    var inputs = document.getElementById(gridid).getElementsByTagName('input');
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].type == 'radio') {
            if (inputs[i].id == obj.id) {
                document.getElementById(hiddenid).value = inputs[i].value;
                inputs[i].checked = true;
            }
            else {
                inputs[i].checked = false;
            }
        }
    }
}

function SelectExaminationConfirmRadio(obj, tableid, controlid1, controlid2, buttonids) {
    var inputs = document.getElementById(tableid).getElementsByTagName('input');
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].type == 'radio') {
            if (inputs[i].id == obj.id) {
                inputs[i].checked = true;
                $("#" + controlid1).attr("disabled", "");
                $("#" + controlid2).attr("disabled", true);
                
                for (var j = 0; j < buttonids.length; j++) {
                    SetWizardButtonDisable(buttonids[j], false);
                }

                doErrorCallbackFun('', controlid2, 0);
            }
            else {
                inputs[i].checked = false;
            }
        }
    }

    hideMessage();
}
