/**
* <pre>
*
*  Accela
*  File: Common.js
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description:
* To deal with inspection action menu .
*  Notes:
* $Id: Common.js 185465 2010-11-29 08:27:07Z karthikeyan.rajmohan $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

//if ("undefined" == typeof jQuery) throw new Error("This application requires jQuery");
jQuery.namespace = function () {
    var a = arguments, o = null, i, j, d;
    for (i = 0; i < a.length; i = i + 1) {
        d = a[i].split(".");
        o = window;
        for (j = 0; j < d.length; j = j + 1) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
    return o;
};
jQuery.fn.exists = function () { return this.length > 0; };
jQuery.extend({
    //currency: function (val) {
    //	return val.replace(/$/gi, "").replace(/,/gi, "");
    //},
    stringify: function stringify(obj) {
        var t = typeof (obj);
        if (t != "object" || obj === null) {
            // simple data type
            if (t == "string") obj = '"' + obj + '"';
            return String(obj);
        } else {
            // recurse array or object
            var n, v, json = [], arr = (obj && obj.constructor == Array);

            for (n in obj) {
                v = obj[n];
                t = typeof (v);
                if (obj.hasOwnProperty(n)) {
                    if (t == "string") v = '"' + v + '"'; else if (t == "object" && v !== null) v = jQuery.stringify(v);
                    json.push((arr ? "" : '"' + n + '":') + String(v));
                }
            }
            return (arr ? "[" : "{") + String(json) + (arr ? "]" : "}");
        }
    },
    stringifyAsText: function stringifyAsText(obj) {

        var t = typeof (obj);
        if (t != "object" || obj === null) {
            // simple data type
            if (t == "string") obj = "'" + obj + "'";
            return String(obj);
        } else {
            // recurse array or object
            var n, v, json = [], arr = (obj && obj.constructor == Array);

            for (n in obj) {
                v = obj[n];
                t = typeof (v);
                if (obj.hasOwnProperty(n)) {
                    if (t == "string")
                        v = "'" + v.replace(/'/g, "\\\\'") + "'";
                    else if (t == "object" && v !== null)
                        v = jQuery.stringifyAsText(v);

                    json.push((arr ? '' : "'" + n + "':") + String(v));
                }
            }
            return (arr ? '[' : '{') + String(json) + (arr ? ']' : '}');
        }


    }
});

jQuery.namespace('aca', 'aca.ui', 'aca.constant', 'aca.data', 'aca.util');
var filename = "NewUI/Scripts/app/common.js";
var servicePath = getRootWebSitePath(filename);//get host url
var routeName = {
    "Home": "Home",
    "LaunchPad": "LaunchPad",
    "Dashboard": "Dashboard",
    "Login": "Login",
    "MapView": "MapView",
    "SearchView": "SearchView"
};

var CommonData = getLabelKeys("Home");
var iframeMinHeight;
$(document).ready(function () {
    initTheme();
    initCustomizedStyle();
    setContentInterlayer(); //set the content interlayer

    //change the window size
    $(window).resize(function () {
        setContentInterlayer(); //set the content interlayer
    });
   
});

//init page theme
function initTheme() {
    var languageData = sessionStorage.getItem("cultureLanguage");

    if (languageData) {
        var languages = JSON.parse(languageData);

        if (typeof (languages) == "string") {
            languages = JSON.parse(languages);
        }

        var selectedLanguage = languages[0].languageKey;
        loadThemeBySelectLanguage(selectedLanguage);
    }
    $.ajax({
        url: servicePath + "api/I18N/Culture-Language",
        async: false,
        data:{ isAdmin : isAdmin },
        type: 'get',
        success: function (response) {
            sessionStorage.setItem("cultureLanguage", JSON.stringify(response));

            if (typeof (response) == "string") {
                response = JSON.parse(response);
            }

            var selectlanguage = response[0].languageKey;
            loadThemeBySelectLanguage(selectlanguage);
        }
    });
}

function initCustomizedStyle() {
    $('#customizedCss').attr('href', servicePath + "api/csshandler/css");
}

function loadThemeBySelectLanguage(languageKey) {
    $("#languageCss").attr("href", "Content/" + languageKey + "/Site.css");
}

aca.data.commond = {
    "creat": "CREATE_CAP",
    "show": "SHOW_RECORD",
    "request": "SERVICE_REQUEST",
    "viewPropertyInfo": "VIEW_PROPERTY"
};
aca.mapLoadSuccess = false;
aca.data.isActionBtnApply = "false";
aca.data.ParcelNumber = "";
aca.data.AddressDescription = "";
aca.data.isRefreshShoppingCart = false;

var isAdmin = location.search.indexOf('isAdmin=Y') > -1;
var isColorTheme = location.search.indexOf('isColorTheme') > -1;

//get the host url
function getRootWebSitePath(filename) {
    if (typeof (filename) == "string" && filename.search(/\.js/i) > 0) {
        var scripts = document.getElementsByTagName('script');
        var len = scripts.length;
        var scriptIndex;
        var src;

        for (var i = 0; i < len; i++) {
            src = scripts[i].src;
            scriptIndex = src.toLowerCase().indexOf(filename.toLowerCase());

            if (scriptIndex != -1) {
                src = src.substring(0, scriptIndex);
                break;
            }
        }

        if (src) {
            if (src.search(/[(https?:\/\/)\/]/i) != 0) {
                // not start with http:// or https:// or /
                var url = location.href;
                var index = url.indexOf("?");
                if (index != -1) {
                    url = url.substring(0, index - 1);
                }
                src = getRootWebSitePath(src, url);
            }
        }
    }

    return src;
}

//General Functions
aca.util = {
    device: {
        isPhone: false,
        isTablet: false,
        isDesktop: false
    },
    init: function () {
        this.properties();
        this.themeInit();
        this.footerInit();
    },
    initFastClick: function () {
        var fastClickButton = document.querySelector('.fastclick');

        new FastClick(fastClickButton);
    },
    properties: function () {
        if ($("#IsPhone").is(":visible"))
            this.device.isPhone = true;
        else if ($("#IsTablet").is(":visible"))
            this.device.isTablet = true;
        else
            this.device.isDesktop = true;
    },
    themeInit: function () {
        try {
            var themeName = $.cookie('acatheme');
            if (themeName != undefined && themeName != null) {
                $("head link#theme-switcher-stylesheet").attr("href", '/Content/theme/' + themeName + '.css');
            }

        } catch (err) { }
    },
    footerInit: function () {
      $("#main").css("min-height", ($(window).height() - 120) + "px");
    },
    loadingMask: {
        show: function (message) {
            if ($('.loading_overlay').length > 0) {
                return;
            }
            if (!$('.loadmask').exists())
                $('body').append('<div class="indicator loadmask"><div class="loading-message"><span class="spinner"></span><span class="spinner-text">Loading..</span></div></div>');

            $('.loadmask').addClass('show').css("min-height", $(document).height() + "px");
            if (message != undefined && message != null && message.length != 0)
                $('.spinner-text').text(message);
            else
                $('.spinner-text').text('Loading...');
        },
        hide: function () {
            $('.indicator').removeClass('show');
            $(".loadmask").removeClass('show').removeAttr('style');
            $(".loadmask").remove();
        }
    }
};

//public parameter
aca.data.IsFromMap = false;
aca.data.mapJsonData = ""; //the json data from map api

(function () {
    //Saerch Result action
    $(document).on("click", "[data-toggle~=action]", function (e) {
        if ($(e.target).parent().hasClass('custom-checkbox'))
            return;

        e.preventDefault();
        var $parent = $(this).parent();

        if ($parent.hasClass("selected"))
            $parent.removeClass("selected");
        else {
            $(".res-content").removeClass("selected");
            $parent.addClass("selected");
        }
    });

    //    //Search Box on Phone view
    //    $(document).on("click", "[data-toggle~=searchbox]", function (e) {
    //        e.preventDefault();
    //        $(".search-bar").toggle();
    //    });

    //Welcome Message toggle
    $(document).on("click", "[data-toggle~=welcomemsg], #btnWelcomeClose", function (e) {
        e.preventDefault();
        $(".welcome-msg-container").toggle();
    });

    $(document).on("click", "[data-toggle~=activity]", function (e) {
        e.preventDefault();
        $this = $(this).parent();
        if ($this.hasClass("collapsed"))
            $this.removeClass("collapsed");
        else {
            $this.addClass("collapsed");
            hidePopover();
        }
    });

    $(document).on("click", "[data-toggle~=slider]", function (e) {
        e.preventDefault();
        $this = $(this).parent();
        if ($this.hasClass("left-selected"))
            $this.removeClass("left-selected");
        else
            $this.addClass("left-selected");
    });

    $(document).on("click", ".close-link-ico", function (e) {
        e.preventDefault();
        $("#welcome").remove();
        setContentInterlayer();
    });

})();

$(function () {
    aca.util.init();
});

var anonymousUser = {
    "isLoggedIn": false,
    "firstName": "",
    "lastName": ""
};

//Search Control
$(document).on('keyup blur click', '#txtGlobalSearh', function (e) {
    e.stopPropagation();

    // Cache our selectors
    var $this = $(this),
        $parent = $this.parent().parent();

    if ($this.val() == '' || $this.val().length < 3)
        $parent.removeClass('show-label').removeClass('show-cc');
    else {
        if (e.keyCode != 13) {
            $parent.addClass('show-label').addClass('show-cc');
        } else {
            $parent.removeClass('show-label').removeClass('show-cc');
        }
    }
});

$(document).on('keyup blur focus click', '.custom-form-control', function (e) {
    e.stopPropagation();

    // Cache our selectors
    var $this = $(this),
        $parent = $this.parent();

    if ($this.val() == '' || $this.val().length == 0)
        $parent.removeClass('show-label');
    else
        $parent.addClass('show-label');
});


$(document).click(function () {
    $("#dropdown-searchform").removeClass('show-cc');
});

$(document).on('click', '.search-options', function (e) {
    e.stopPropagation();
});


$(document).on('click', '.default-search ul li', function (e) {
    e.stopPropagation();
    if ($(this).data("forbidden")) {
        $(this).toggleClass('selected');
    }
});


$(document).on('click', '.advancesearch-content ul li', function (e) {
    e.stopPropagation();
    $('.advancesearch-content ul li').each(function (index, item) {
        $(item).removeClass("selected");
    });
    $(this).addClass("selected");
});

$(document).on('click', '.dashboard-container', function (e) {
    //alert($("#txtGlobalSearh").val());
    window.location = "/dashboard";
});
 
$(window).resize(function () {
    aca.util.footerInit();
});


Handlebars.registerHelper('ifvalue', function (conditional, options) {
    if (options.hash.value == conditional) {
        return options.fn(this);
    } else {
        return options.inverse(this);
    }
});

//set the content interlayer and save the iframe min height
function setContentInterlayer() {
    var welcome = $("#welcome").length > 0 ? $("#welcome")[0].clientHeight : $("#welcome").height();
    var headers = $("#header").length > 0 ? $("#header")[0].clientHeight : $("#header").height();  
    var main = $("#main").height();
    var footer = $("#footer").length > 0 ? $("#footer")[0].clientHeight : $("#footer").height(); 
    var winWidth = document.documentElement.clientWidth;
       
    $("#header").css("margin-top",(winWidth < 768 ? 0: welcome) + "px");
    $("#main").css("margin-top", (welcome + headers) + "px");
    $("#main").css("margin-bottom", footer + "px");
    if (winWidth >= 768 && winWidth <= 898) {
            $('.navbar-toolbar').css("margin-right", -250 + "px"); 
    } else {
        $('.navbar-toolbar').css("margin-right", 0 + "px");
    }
    if (document.documentElement && document.documentElement.clientHeight && document.documentElement.clientWidth) {
        var winHeight = document.documentElement.clientHeight;
        var iframeH = winHeight - footer - welcome - headers;
        //$('#main').css("height", (iframeH - 30) + "px");
       iframeMinHeight = (iframeH - 10);
       // iframeMinHeight = ($(window).height() - 120);
   }
}

//Get html Contrl ClientHeight
function GetControlClientHeight(control) {
    if (typeof control == "object") {
        return control.length > 0 ? control[0].clientHeight : control.height();
    } else {
        return $(control).length > 0 ? $(control)[0].clientHeight : $(control).height();
    }
}

//Iframe init Auto height
function SetWinHeight(obj) {
    $(".alliframe").css("height", iframeMinHeight - 50 + "px");
    var firstloading = new loadingControl($("body"));
    firstloading.CancelLoad();
}

function setIframeSrc(url, callBack) {
    $(".contentContainer").hide();
    var $iframe = $("#IframeControl");
    if ($iframe.length <= 0) return;

    $iframe.prev().hide();
    $iframe.show().find(".frame-screen").css("min-height", iframeMinHeight + "px");

    if (callBack != null && callBack != undefined && callBack!="") {
        $iframe.data("callBack", callBack);
    } else {
        $iframe.data("callBack", "");
    }

    if (url != "") {
        if (url.search(/https?:/) < 0) {
            url = servicePath + url;
        }

        $("#capIfram").attr("src", url);
        var firstloading = new loadingControl($("body"));
        firstloading.initControl();
        firstloading.beginLoad();
    } else {
        var acaiFrame = document.getElementById("capIfram");

        if (acaiFrame != 'undefined' && typeof(acaiFrame.contentWindow.SetNotAsk) != 'undefined') {
            acaiFrame.contentWindow.SetNotAsk();
        }
        $(acaiFrame).attr("src", "");
    }
}

function closeHomeIframe() {
    if ($("#IframeControl").css("display") != "none") {
        var callBack = $("#IframeControl").data("callBack");
        setIframeSrc("");
        var $iframe = $("#IframeControl");
        $iframe.hide();
        $iframe.prev().show();
        $(".contentContainer").show();
        if (callBack != null && callBack != undefined && callBack != "") {
            if (typeof callBack == 'function') {
                callBack();
            }

        }
    }
};

/*
* save data into cookie.
* params:
*   @key: cookie name.
*   @value: cookie value.
*   @exDay: expired days.
*/
function save2Cookie(key, value) {
    var d = new Date();
    // 86400000 = 1000*60*60*24: milliseconds per day. default one day.
    d.setTime(d.getTime() + 86400000);

    document.cookie = key + "=" + encodeURIComponent(value) + ";expires=" + d.toUTCString();
}

/*
* get from cookie.
* params:
*   @key: cookie name.
*/
function getCookie(key) {
    var arr = document.cookie.replace(key + "=", "#cookie#=").match(/#cookie#=.*?;/g);

    if (arr) {
        return unescape(arr[0].replace("#cookie#=", "")).replace(";", "");
    }

    return "";
}

/*
* remove from cookie.
* params:
*   @key: cookie name.
*/
function removeCookie(key) {
    var d = new Date();
    d.setTime(d.getTime() - 1);

    var cookie = getCookie(key);
    if (cookie) {
        document.cookie = key + "=" + encodeURIComponent(cookie) + ";expires=" + d.toUTCString();
    }
}

/*
* save data into SessionStorage.
* params:
*   @key: storage name.
*   @value: storage value.
*/
function save2SessionStorage(key, value) {
    if (sessionStorage == null) {
        console.log("SessionStorage not supported by browser.");
    } else {
        sessionStorage.setItem(key, value);
    }
}

/*
* get from SessionStorage.
* params:
*   @key: storage name.
*/
function getSessionStorage(key) {
    if (sessionStorage == null) {
        console.log("SessionStorage not supported by browser.");
        return "";
    } else {
        return sessionStorage.getItem(key);
    }
}

/*
* remove from SessionStorage.
* params:
*   @key: storage name.
*/
function removeSessionStorage(key) {
    if (sessionStorage == null) {
        console.log("SessionStorage not supported by browser.");
    } else {
        sessionStorage.removeItem(key);
    }
}

/*
* create link tag.
* params:
*   @d: document.
*   @h: link href.
*/
function createLink(d, h) {
    var element = d.createElement("link");
    element.rel = "stylesheet";
    element.href = servicePath + h;
    return element;
}

/*
* create script tag.
* params:
*   @d: document.
*   @src: src.
*/
function createScript(d, src) {
    var element = d.createElement("script");
    element.src = servicePath + src;
    return element;
}

function changeStyle(val) {
    var iframe = document.capIfram;

    if (val && iframe) {
        var head = iframe.document.getElementsByTagName("head")[0];

        if (changedStyles) {
            head.removeChild(changedStyles);
        }

        var link = createLink(iframe.document, val);
        head.appendChild(link);
        changedStyles = link;
    }
}

function ShowModalWindow(title, body) {
    $('#hintModal>.modal-dialog>.modal-content>.modal-header>.modal-title')[0].innerHTML = title;
    $('#hintModal>.modal-dialog>.modal-content>.modal-body')[0].innerHTML = body;
    $('#hintModal').modal('show');
}

//show add collection ModalWindow
function ShowCollcetionWindow(title, Caps,ismultiple) {
    $('#collectiondropdown').get(0).options[0].selected = true;
    $("#prompt")[0].innerHTML = "";
    $("#nameText").val("");
    $("#descriptionText").val("");
    disabledButton();
    $('#textBoxModal>.modal-dialog>.modal-content>.modal-header>.modal-title')[0].innerHTML = title;
    $('#textBoxModal').data('caps', Caps);
    $('#textBoxModal').modal('show');

    if (ismultiple) {
        $("input[name='rdocollectionbtn']").get(0).checked = true;
        $('#collectiondropdown').removeAttr("disabled");
        $('#nameText').attr('disabled', "true");
        $('#descriptionText').attr('disabled', "disabled");

    } else {
        $("input[name='rdocollectionbtn']").get(1).checked = true;
        $('#nameText').removeAttr("disabled");
        $('#collectiondropdown').attr('disabled', "true");
        $('#descriptionText').removeAttr("disabled");
    }
}

//When collection name change
function collectionNameChanged() {
    var collcetionName = $("#nameText").val();
    if (collcetionName == "") {
        disabledButton();
    } else {
        enableButton();
    }
};

//disable add collection button
function disabledButton() {
    $('#addCollcetionBtn').attr('disabled', "true");
    $('#addCollcetionBtn').attr("className", "disabled-button");
    $('#addCollcetionBtn').removeClass("employ-button");
    $('#addCollcetionBtn').addClass("disabled-button");
}

//enable add collection button
function enableButton() {
    $('#addCollcetionBtn').removeAttr("disabled");
    $('#addCollcetionBtn').removeClass("disabled-button");
    $('#addCollcetionBtn').addClass("employ-button");
}

//click collection radio button
function rdoCollection() {
    if ($("input[name='rdocollectionbtn']").get(0).checked) {
        $('#nameText').attr('disabled', "true");
        $('#descriptionText').attr('disabled', "disabled");
        $('#collectiondropdown').removeAttr("disabled");
        MyCollectionChanged();
    } else {
        $('#collectiondropdown').attr('disabled', "true");
        $('#nameText').removeAttr("disabled");
        $('#descriptionText').removeAttr("disabled");
        collectionNameChanged();
    }
};

//When my Collection change
function MyCollectionChanged() {
    if ($("#collectiondropdown").get(0).selectedIndex == 0) {
        disabledButton();
    } else {
        enableButton();
    }
}

function loadMapApi() {
    var gisApi = "";

    $.ajax({
        url: servicePath + "api/map/gisServerUrl",
        async: false,
        datatype: "json",
        type: 'get',
        success: function (data) {
            var dataJson = JSON.parse(data);
            gisApi = dataJson.gisApiUrl;
            aca.data.agency = dataJson.agencyCode;
        }
    });

    return gisApi;
}


$.ajaxSetup({
    cache: true
});
jQuery.cachedScript = function (url, options) {

    // Allow user to set any option except for dataType, cache, and url
    options = $.extend(options || {}, {
        dataType: "script",
        cache: true,
        url: url
    });

    // Use $.ajax() since it is more flexible than $.getScript
    // Return the jqXHR object so we can chain callbacks
    return jQuery.ajax(options);
};




function redirectToLogin(msg) {
    closeHomeIframe();
    window.location.href = window.location.href.split("#")[0] + "#/Login";

    if (msg) {
        $("#txtLoginMsg").html(msg);
    }
}

function redirectToNewUIHome() {
    closeHomeIframe();
    window.location.href = window.location.href.split("#")[0] + "#/";
}

function redirectToNewUIHomeShowMessage(altID) {
    closeHomeIframe();
    if (altID != '')
        ShowModalWindow(CommonData.aca_newui_home_label_notice, stringFormat(CommonData.aca_createanotherapplication_success, altID));
}

//String Format
function stringFormat() {
    if (arguments.length == 0)
        return null;
    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
} 

function redirectToLaunchpad(firstName, lastName) {
    save2SessionStorage("noNeedLoadCurrentUrl", true);
    save2SessionStorage("userInformation", JSON.stringify({
        "isLoggedIn": true,
        "firstName": firstName,
        "lastName": lastName
    }));

    window.location.href = servicePath;
}

function Login(userId, password) {
    if (userId && password) {
        var user = {
            Name: userId,
            Pwd: password,
            IsRemember: $("#rememberme").prop("checked") ? 1 : 0
        };

        userLogin(user);
    } else {
        var route = window.location.href.split("#")[1];

        if (route == "/Login") {
            window.location.reload();
        } else {
            window.location.href = window.location.href.split("#")[0] + "#/Login";
        }
    }
}

 function userLogin(user) {
     $.ajax({
         url: servicePath + "api/publicuser/signin",
         async: false,
         data: user,
         datatype: "json",
         type: 'POST',
         success: function(response) {
             var r = response;

             if (typeof (response) == "string") {
                 r = JSON.parse(response);
             }

             if (r.type == "success") {
                 if (user.IsRemember == 1) {
                     save2Cookie("REMEMBERED_USER_NAME", user.Name);
                 } else {
                     removeCookie("REMEMBERED_USER_NAME");
                 }

                 save2SessionStorage("userInformation", JSON.stringify({
                     "isLoggedIn": true,
                     "firstName": r.firstName ? r.firstName.substr(0, 1) : "",
                     "lastName": r.lastName ? r.lastName.substr(0, 1) : ""
                 }));

                 window.location.href = window.location.href.split("#")[0] + "#/Dashboard";
                 window.location.reload();

             } else if (r.type == "redirect") {
                 setIframeSrc(r.url);
             } else {
                 closeHomeIframe();
                 ShowModalWindow(CommonData.aca_newui_home_label_notice, r.message);
                 return false;
             }
         }
 });
};

function showNormalMessage(msg, msgtype) {
    ShowModalWindow(msgtype, msg);  
}

function getLabelKeys(route) {
    var keysValue = sessionStorage.getItem(route);
    var commonData;

    if (keysValue) {
        commonData = JSON.parse(keysValue);
        return commonData;
    }

    var keys = dataKeyJson;
    var labelKey = {
        "route": route,
        "keys": getKeysByRoute(route, keys),
        "cultureName": ""
    };

    $.ajax({
        type: "POST",
        url: servicePath + "api/LabelKey/Label-Key",
        async: false,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(labelKey),
        success: function (response) {
            sessionStorage.setItem(route, JSON.stringify(response));
            commonData = response;
        }
    });

    return commonData;
}

function getKeysByRoute(route, keys) {
    switch (route) {
        case routeName.Home:
            keys = keys.Home;
            break;
        case routeName.LaunchPad:
            keys = keys.LaunchPad;
            break;
        case routeName.Dashboard:
            keys = keys.Dashboard;
            break;
        case routeName.MapView:
            keys = keys.MapView;
            break;
        case routeName.Login:
            keys = keys.Login;
            break;
        case routeName.SearchView:
            keys = keys.SearchView;
            break;
        default:;
    }

    return keys;
};

function removeUiSessionStorage() {
    sessionStorage.removeItem(routeName.Home);
    sessionStorage.removeItem(routeName.LaunchPad);
    sessionStorage.removeItem(routeName.Dashboard);
    sessionStorage.removeItem(routeName.MapView);
    sessionStorage.removeItem(routeName.Login);
    sessionStorage.removeItem(routeName.SearchView);
    
    sessionStorage.removeItem("cultureLanguage");
    sessionStorage.removeItem("selectedLanguage");
}

function decodeHTMLTag(value) {
    if (value) {
        value = value.replace(/&lt;/gi, "<").replace(/&gt;/gi, ">").replace(/&quot;/g, "\"").replace(/&#39;/g, "\'").replace(/&raquo;/gi, "\u00BB").replace(/&laquo;/gi, "\u00AB").replace(/&nbsp;/gi, " ").replace(/&nbsp;/gi, " ").replace(/&amp;/gi, "&");
    }

    return value;
}

function trimHtmlTag(val) {
    if (!val) {
        return '';
    }

    var regHtmlTag = /(<.*?>)|(&lt;.*?&gt;)/g;
    return val.replace(regHtmlTag, '');
}

function IsTrue(obj) {
    var isTrue = false;

    if (typeof (obj) == "boolean" && obj == true) {
        isTrue = true;
    } else if (typeof (obj) == "string" && obj.toUpperCase() == "TRUE") {
        isTrue = true;
    }

    return isTrue;
}