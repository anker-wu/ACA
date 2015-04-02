/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: common.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: common.js 72643 2014-06-19 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/18/2008     		Kevin.Feng				Initial.  
 * </pre>
 */

//if ("undefined" == typeof jQuery) throw new Error("This application requires jQuery");


//jQuery.namespace = function () {
//    var a = arguments, o = null, i, j, d;
//    for (i = 0; i < a.length; i = i + 1) {
//        d = a[i].split(".");
//        o = window;
//        for (j = 0; j < d.length; j = j + 1) {
//            o[d[j]] = o[d[j]] || {};
//            o = o[d[j]];
//        }
//    }
//    return o;
//};
//jQuery.namespace('aca', 'aca.ui', 'aca.constant', 'aca.data', 'aca.util');
//jQuery.fn.exists = function () { return this.length > 0; }


//var APP = {

//    init: function () {
//        $("html").Core();
//    },
//    themeInit: function () {
//        try {
//            if ($.cookie('headertheme') != undefined && $.cookie('headertheme') != null && $.cookie('headertheme').length != 0) {
//                $("head link#headerswitcher").attr("href", '/Content/theme/' + $.cookie('headertheme'));
//            }

//            if ($.cookie('theme') != undefined && $.cookie('theme') != null && $.cookie('theme').length != 0) {
//                $("head link#styleswitcher").attr("href", '/Content/theme/' + $.cookie('theme'));
//            }

//            if ($.cookie('sidebar') != undefined && $.cookie('sidebar') != null && $.cookie('sidebar').length != 0 && $.cookie('sidebar') === 'sidebar-minimized') {
//                $('html').addClass('sidebar-minimized');
//            }

//            var elem = document.querySelector('.fixed-header-switch'),
//            init = new Switchery(elem);

//            elem.onchange = function () {
//                if (elem.checked)
//                    $("#header").addClass('navbar-fixed-top');
//                else
//                    $("#header").removeClass('navbar-fixed-top');
//            };

//        } catch (err) { }

//    },
//    menuInit: function () {
//        var targetAnchor;
//        $.each($('ul.topmenu a'), function () {

//            if (this.href == window.location) {
//                targetAnchor = this;
//                return false;
//            }
//        });

//        var parent = $(targetAnchor).closest('li');
//        while (true) {
//            parent.addClass('active');
//            parent.closest('ul.topmenu').show();
//            parent.closest('ul').addClass('in').closest('li').addClass('active open');
//            parent = $(parent).parents('li').eq(0);
//            if ($(parent).parents('ul.topmenu').length <= 0) break;
//        }

//    },
//    loadingMask: {
//        show: function (message) {

//            if (!$('.loadmask').exists())
//                $('body').append('<div class="indicator loadmask"><span class="spinner-mobile spinner3 visible-xs"></span><span class="spinner spinner3 hidden-xs"></span><span class="spinner-text hidden-xs"></span></div>')

//            $('.loadmask').addClass('show');
//            if (message != undefined && message != null && message.length != 0)
//                $('.spinner-text').text(message);
//            else
//                $('.spinner-text').text('Loading...');
//        },
//        hide: function () {
//            $('.loadmask').removeClass('show');
//        }
//    }
//};
//$(function () {
//    APP.init(),
//    APP.themeInit();
//    APP.menuInit()
//});


//$(document).on("click", "#dropdown-search-form", function (e) {
//    e.preventDefault();
//});


//// Demo Color Variation
//// Read the CSS files from data attributes
//$("#theme-color-variations a").click(function () {
//    $("head link#styleswitcher").attr("href", '/Content/theme/' + $(this).data("theme"));
//    $.cookie('theme', $(this).data("theme"));
//    return false;
//});

//$("#theme-header-variations a").click(function () {
//    $("head link#headerswitcher").attr("href", '/Content/theme/' + $(this).data("headertheme"));
//    $.cookie('headertheme', $(this).data("headertheme"));
//    return false;
//});

////Fixed Header 
//$(document).on("click", ".switchery", function (e) {
//    return false;
//});

