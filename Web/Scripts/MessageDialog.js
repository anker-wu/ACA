/**
 * <pre>
 * 
 *  Accela
 *  File: MessageDialog.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2012
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: MessageDialog.js 234112 2012-09-29 04:20:24Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *
 * </pre>
 */
        
function showContinueDlg(hlBeginClientID){
    var objDeck = $get("deck");
    
    if(objDeck){
        objDeck.className = "ACA_MaskDiv";            
    }

    Sys.UI.DomElement.removeCssClass($get('divBox'), 'ACA_Hide');
    
   // $get('divBox').className='PopUpDlg';
    messageDialogAdjustLocation(hlBeginClientID);    
}

function cancel() {
    Sys.UI.DomElement.addCssClass($get('divBox'), 'ACA_Hide');
    //$get('divBox').className = "ACA_Hide";
    $get("deck").className = "ACA_Hide";
    return false;
}

function messageDialogAdjustLocation(hlBeginClientID) {
    var obox=$get('divBox');
    
    if (obox !=null && obox.className != "ACA_Hide"){     
        var oLeft=(document.body.clientWidth-obox.clientWidth)/2 + "px";
        var oTop=(document.body.clientHeight-obox.clientHeight)/2 + "px";
        
        obox.style.left=oLeft;
        obox.style.top=oTop;
        var hlBegin = document.getElementById(hlBeginClientID);
        if (hlBegin){
            hlBegin.focus();
            obox.scrollIntoView();
        }
    }
}
    
