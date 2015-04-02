/**
* <pre>
* 
*  Accela
*  File: SessionTimeout.js
* 
*  Accela, Inc.
*  Copyright (C): 2014
* 
*  Description:
*  
*  Notes:
* $Id: SessionTimeout.js 185465 2014-11-04 ACHIEVO\grady.lu $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

(function ($) {
    var localLatestTime = new Date().getTime();
    var bakTime;
    var isOpened = false;
    var isCanceled = false;
    var timeoutTimerId;
    var refreshWarnMsgId;
    var opts = {};
    
    function showDialog() {
        // if the dialog has been opened, then without opening again.
        if (isOpened) {
            return;
        }
        var warningTime = opts.WarningTime;
        //Set refresh warning message.
        refreshWarnMsgId = setInterval(refreshWarnTime, 1000 * 60);
        var content = opts.Message.replace("\{0\}", warningTime);
        warningTime--;

        var maktemp = '<div id="markdiag" class="ACA_MaskDiv"></div>';
        var tempstr = '<div class="divSessionTimeoutDialog" id="centerBox">' +
                        '<a id="warningDialog" tabindex="1" name="warningDialog" href="#" onkeydown="OverrideTabKey(event, false, \'btnOk\')" title="' + content + opts.MsgHelp + '"></a>' +
                        '<div class="boxTitle">' +
                        '<span class="titleText">' + opts.Title + '</span>' +
                        '</div>' +
                        '<div class="boxEntry">' +
                        '    <span class="entryContent" id="message">' + content + '</span>' +
                        '</div>' +
                        '<div class="boxEntry">' +
                        '    <input id="btnOk" type="button" value="' + opts.BtnOK + '" class="entryBtn" onkeydown="OverrideTabKey(event, false, \'btnCancel\')">&nbsp;&nbsp;&nbsp;&nbsp;' +
                        '    <input id="btnCancel" type="button" value="' + opts.BtnCancel + '" class="entryBtn" onkeydown="OverrideTabKey(event, false, \'btnOk\')">' +
                        '</div>' +
                      '</div>';
        $("body").append(maktemp);
        $("body").append(tempstr);

        //Set the status of dialog is opened.
        isOpened = true;

        //Adjust the dialog
        adjustDialog();

        //Set the focus on <a/> element. Notice: this setting must place after adjustDialog(). Otherwise, the dialog's top is invalid.
        if (isFireFox()) {
            window.location.hash = "warningDialog";
        } else {
            $("#warningDialog").focus();
        }

        function adjustDialog() {
            var popupMask = document.getElementById("markdiag");
            var container = document.getElementById("centerBox");

            ACAGlobal.Dialog.adjustPosition(container, popupMask, "em", opts);

            //Set the timeout dialog on the top level
            var timeoutDialog = $("#centerBox");
            var timeoutDialogMask = $("#markdiag");
            timeoutDialog.css('z-index', 9001);
            timeoutDialogMask.css('z-index', 9000);
        }

        $("#btnOk").click(function () {
            closeDialog();
            if (warningTime >= 0) {
                sendBlankRequest();
            } else {
                cancelSessionTimeout();
            }
        });
        $("#btnCancel").click(function () {
            closeDialog();
            cancelSessionTimeout();
        });

        function cancelSessionTimeout() {
            isCanceled = true;
        }

        function refreshWarnTime() {
            if (warningTime <= 0) {
                isCanceled = true;
                //Close dialog.
                closeDialog();
                return;
            }
            var newMessage = opts.Message.replace("\{0\}", warningTime);
            $('#message').text(newMessage);
            warningTime--;
        }

        function sendBlankRequest() {
            var randomData = Math.random();

            try {
                $.ajax({
                    type: "GET",
                    url: opts.UrlRefresh,
                    data: "data=" + randomData,
                    async: false,
                    success: function (data , textStatus, jqXHR) {
                        //do nothing
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        //do nothing
                    }
                });
            } catch (ex) {
                //do nothing
            }
        }
    }

    function closeDialog() {
        if ($("#centerBox").length > 0) {
            $("#centerBox").remove();
        }
        if ($("#markdiag").length > 0) {
            $("#markdiag").remove();
        }
        if (refreshWarnMsgId != null) {
            clearInterval(refreshWarnMsgId);
        }

        isOpened = false;
    }

    function logout() {
        if (typeof (SetNotAsk) != 'undefined') {
            SetNotAsk(true);
        }

        isCanceled = true;

        if (opts.IsAnonymous) {
            window.location.href = opts.UrlWelcome + "?ReturnMessageKey=aca_sessiontimeout_msg_timeout";
        } else {
            window.location.href = opts.UrlLogin + "?ReturnMessageKey=aca_sessiontimeout_msg_timeout";
        }
    }

    function timeoutTimer () {
        var latestTime = getCookie("LASTEST_REQUEST_TIME");
        if (latestTime != null && latestTime != "") {
            latestTime = parseInt(latestTime);
            if (latestTime != bakTime) {
                localLatestTime = new Date().getTime();
                bakTime = latestTime;
            }
        }

        var tmpWaitingTime = (opts.TimeoutTime - opts.WarningTime) * 1000 * 60;
        var tmpTimeoutTime = opts.TimeoutTime * 1000 * 60;
        var tmpTime = new Date().getTime() - localLatestTime;
        if (tmpTime > tmpWaitingTime && !isOpened && !isCanceled && opts.WarningTime != 0) {
            showDialog();
        } else if (tmpTime > tmpTimeoutTime) {
            logout();
        }

       //utility function called by getCookie()
       function getCookieVal(offset) {
           var endstr = document.cookie.indexOf(";", offset);
           if (endstr == -1) {
               endstr = document.cookie.length;
           }
           return unescape(document.cookie.substring(offset, endstr));
       }

        // primary function to retrieve cookie by name
        function getCookie(name) {
            var arg = name + "=";
            var alen = arg.length;
            var clen = document.cookie.length;
            var i = 0;
            while (i < clen) {
                var j = i + alen;
                if (document.cookie.substring(i, j) == arg) {
                    return getCookieVal(j);
                }
                i = document.cookie.indexOf(" ", i) + 1;
                if (i == 0) break;
            }
            return null;
        }
    };

    $.fn.SessionTimeoutTimer = function(options) {
        opts = $.extend({}, $.fn.SessionTimeoutTimer.defaults, options);
        if (timeoutTimerId != null) {
            clearInterval(timeoutTimerId);
        }
        timeoutTimerId = setInterval(timeoutTimer, 1000);
    };
    $.fn.SessionTimeoutTimer.defaults = {
        width: 450,
        IsAnonymous: true,
        TimeoutTime: 60,
        WarningTime: 5,
        Title: "Warning",
        Message: "Your session will time out in {0} minutes. Would you like to continue?",
        MsgHelp: "Please press the Tab key to move the focus to the OK or Cancel button.",
        BtnOK: "OK",
        BtnCancel: "Cancel",
        UrlWelcome: '/Welcome.aspx',
        UrlLogin: '/Login.aspx',
        UrlRefresh: '/Handlers/RefreshSessionHandler.ashx'
    };
})(jQuery); 