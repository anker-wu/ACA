/*
 * <pre>
 *  Accela Citizen Access
 *  File: InitAlonePopovers.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: InitAlonePopovers.js 77905 2014-05-30 12:49:28Z ACHIEVO\dennis.fu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
*/

var currentPopover = null;
$(document).ready(function () {
    if ('undefined' != alonePopovers) {
        alonePopovers.forEach(function (e) {
            $("#" + e).popover();
            $("#" + e).on('show.bs.popover', function () {
                currentPopover = $("#" + e);
                alonePopovers.forEach(function (item) {
                    if (item != e) {
                        $("#" + item).popover('hide');
                    }
                });
            });
            
//            $("#" + e).on('hidden.bs.popover', function () {
//                var popoversin = $('.popover.fade.bottom').remove();
//            });

        });
        window.onresize = function() {
            autoScroll($('body'));
        };
    }
});

$(document).click(function () {
        var evt = window.event || (typeof (event) == "undefined" ? undefined : event) || arguments.callee.caller.arguments[0];
        var objTarget = evt.srcElement || evt.target;
        if (objTarget == undefined || $.inArray(objTarget.id, alonePopovers) < 0) {
          hidePopover();
      }
});

function hidePopover() {
    if (currentPopover != null){
        currentPopover.popover('hide');
    }
}

function autoScroll(control) {
    var popoversin = $('.popover.fade.bottom.in');
    if (popoversin.size() > 0) {
        $(currentPopover.selector).popover('show');
    }
}