/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: styleswitcher.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: styleswitcher.js 72643 2014-06-19 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/18/2008     		Kevin.Feng				Initial.  
 * </pre>
 */

if ("undefined" == typeof jQuery) throw new Error("This application requires jQuery");
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
jQuery.fn.exists = function () { return this.length > 0; }
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
jQuery.namespace('theme', 'theme.ui', 'theme.data', 'theme.website');


theme.data = {
    "theme": [
        {
            "ThemeId": "T1",
            "Background": "#fcfcfc",
            "BackgroundImage": "none",
            "Banner": "#355561",
            "BannerText": "#c9c7c9",
            "BannerSecondary": "#243e47",
            "Headlines": "#222222",
            "HeadlinesText": "#818181",
            "PrimaryButton": "#e07e2d",
            "Footer": "#000000",
            "FooterText": "#ffffff"
        },
        {
            "ThemeId": "T2",
            "Background": "#fff7ec",
            "BackgroundImage": "none",
            "Banner": "#8f7859",
            "BannerText": "#c9c7c9",
            "BannerSecondary": "#77664a",
            "Headlines": "#222222",
            "HeadlinesText": "#818181",
            "PrimaryButton": "#97bd09",
            "Footer": "#000000",
            "FooterText": "#ffffff"
        },
        {
            "ThemeId": "T3",
            "Background": "#e5e5e5",
            "BackgroundImage": "url(/Images/bg/bright_squares.png)",
            "Banner": "#a7a7a7",
            "BannerText": "#dddddd",
            "BannerSecondary": "#5c5e60",
            "Headlines": "#222222",
            "HeadlinesText": "#818181",
            "PrimaryButton": "#fcfcfc",
            "Footer": "#000000",
            "FooterText": "#ffffff"
        },
        {
            "ThemeId": "T4",
            "Background": "#fcfcfc",
            "BackgroundImage": "none",
            "Banner": "#575b5d",
            "BannerText": "#c9c7c9",
            "BannerSecondary": "#222222",
            "Headlines": "#222222",
            "HeadlinesText": "#818181",
            "PrimaryButton": "#b53e2e",
            "Footer": "#000000",
            "FooterText": "#ffffff"
        }, {
            "ThemeId": "T5",
            "Background": "#fcfcfc",
            "BackgroundImage": "none",
            "Banner": "#748281",
            "BannerText": "#c3cecd",
            "BannerSecondary": "#606e6d",
            "Headlines": "#222222",
            "HeadlinesText": "#818181",
            "PrimaryButton": "#50dfab",
            "Footer": "#000000",
            "FooterText": "#ffffff"
        }, {
            "ThemeId": "T6",
            "Background": "#fcfcfc",
            "BackgroundImage": "none",
            "Banner": "#687285",
            "BannerText": "#c3cecd",
            "BannerSecondary": "#4e5666",
            "Headlines": "#222222",
            "HeadlinesText": "#818181",
            "PrimaryButton": "#2bbce0",
            "Footer": "#000000",
            "FooterText": "#ffffff"
        }, {
            "ThemeId": "T7",
            "Background": "#fcfcfc",
            "BackgroundImage": "none",
            "Banner": "#11677c",
            "BannerText": "#c3cecd",
            "BannerSecondary": "#075061",
            "Headlines": "#222222",
            "HeadlinesText": "#818181",
            "PrimaryButton": "#ffffff",
            "Footer": "#000000",
            "FooterText": "#ffffff"
        }, {
            "ThemeId": "T8",
            "Background": "#fcfcfc",
            "BackgroundImage": "none",
            "Banner": "#5c5c5c",
            "BannerText": "#ffffff",
            "BannerSecondary": "#312f2f",
            "Headlines": "#cccccc",
            "HeadlinesText": "#434343",
            "PrimaryButton": "#00d5ff",
            "Footer": "#cccccc",
            "FooterText": "#434343"
        }
    ]
};

theme.data.background = {
    "ImageBg": [
        {
            "ImageUrl": "/Images/bg/batthern.png"
        }, {
            "ImageUrl": "Images/bg/bgnoise_lg.png"
        }, {
            "ImageUrl": "/Images/bg/brickwall.png"
        }, {
            "ImageUrl": "/Images/bg/bright_squares.png"
        }, {
            "ImageUrl": "/Images/bg/brillant.png"
        }, {
            "ImageUrl": "/Images/bg/brushed_alu.png"
        }, {
            "ImageUrl": "/Images/bg/brushed_alu_dark.png"
        }, {
            "ImageUrl": "/Images/bg/circles.png"
        }, {
            "ImageUrl": "/Images/bg/connect.png"
        }, {
            "ImageUrl": "/Images/bg/cutcube.png"
        }, {
            "ImageUrl": "/Images/bg/diagonal_striped_brick.png"
        }, {
            "ImageUrl": "/Images/bg/graphy.png"
        }, {
            "ImageUrl": "/Images/bg/knitted-netting.png"
        }, {
            "ImageUrl": "/Images/bg/paven.png"
        }, {
            "ImageUrl": "/Images/bg/perforated_white_leather.png"
        }, {
            "ImageUrl": "/Images/bg/purty_wood.png"
        }, {
            "ImageUrl": "/Images/bg/45degreee_fabric.png"
        }, {
            "ImageUrl": "/Images/bg/60degree_gray.png"
        }, {
            "ImageUrl": "/Images/bg/absurdidad.png"
        }, {
            "ImageUrl": "/Images/bg/always_grey.png"
        }, {
            "ImageUrl": "/Images/bg/argyle.png"
        }, {
            "ImageUrl": "/Images/bg/assault.png"
        }, {
            "ImageUrl": "/Images/bg/az_subtle.png"
        }, {
            "ImageUrl": "/Images/bg/back_pattern.png"
        }, {
            "ImageUrl": "/Images/bg/beige_paper.png"
        }, {
            "ImageUrl": "/Images/bg/cubes.png"
        }, {
            "ImageUrl": "/Images/bg/diamonds.png"
        }, {
            "ImageUrl": "/Images/bg/dimension.png"
        }, {
            "ImageUrl": "/Images/bg/elastoplast.png"
        }, {
            "ImageUrl": "/Images/bg/rubber_grip.png"
        }
    ]
};

theme.data.style = {
    "StyleList": [{
        "Type": "Background",
        "Prop": "body",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "BackgroundImage",
        "Prop": "body",
        "Css": "background-image",
        "Luminance": "0"
    }, {
        "Type": "Banner",
        "Prop": "#header.navbar",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "Banner",
        "Prop": ".navbar#header .navbar-toolbar",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "Banner",
        "Prop": ".search-menu-switch",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "Banner",
        "Prop": ".modal-header",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "Banner",
        "Prop": ".search-bar",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "Banner",
        "Prop": ".lp-button-layout .btn-default",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "Banner",
        "Prop": "#dropdown-searchform .search-options .adv-search-title",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "Banner",
        "Prop": "#dropdown-searchform .search-options .search-title",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "Banner",
        "Prop": ".lp-button-layout .btn-default",
        "Css": "border-color",
        "Luminance": "0"
    }, {
        "Type": "Banner",
        "Prop": ".modal-header",
        "Css": "border-color",
        "Luminance": "0"
    }, {
        "Type": "BannerText",
        "Prop": "#header.navbar .navbar-header .navbar-brand .logo-text",
        "Css": "color",
        "Luminance": "0"
    }, {
        "Type": "BannerSecondary",
        "Prop": "#header.navbar",
        "Css": "border-bottom-color",
        "Luminance": "0"
    }, {
        "Type": "BannerSecondary",
        "Prop": ".lp-panel .lp-header",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "BannerSecondary",
        "Prop": "#header.navbar .navbar-toolbar .navbar-nav > li > a.user .m-avatar span.rendering",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "BannerSecondary",
        "Prop": ".top-info .m-avatar span.rendering .inner-cir",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "BannerSecondary",
        "Prop": "#SearchResult .nav-tabs > li.active > a, #SearchResult .nav-tabs > li.active > a:hover, #SearchResult .nav-tabs > li.active > a:focus, #SearchResult .nav-tabs > li > a:hover, #SearchResult .nav-tabs > li > a:focus",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "BannerSecondary",
        "Prop": "#dropdown-searchform .search-options .adv-search-title:hover",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "BannerSecondary",
        "Prop": ".navbar#header .navbar-toolbar .navbar-nav > li > a:hover, .navbar#header .navbar-toolbar .navbar-nav > li > a:focus",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "BannerSecondary",
        "Prop": ".lp-panel .lp-header",
        "Css": "border-color",
        "Luminance": "0"
    }, {
        "Type": "BannerSecondary",
        "Prop": "#dropdown-searchform .form-control",
        "Css": "background-color",
        "Luminance": "0.1"
    }, {
        "Type": "BannerSecondary",
        "Prop": ".lp-button-layout .btn-default:hover, .lp-button-layout .btn-default:focus, .lp-button-layout .btn-default:active, .lp-button-layout .btn-default.active",
        "Css": "background-color",
        "Luminance": "0.1"
    }, {
        "Type": "BannerSecondary",
        "Prop": "#dropdown-searchform .form-control",
        "Css": "border-color",
        "Luminance": "-0.05"
    }, {
        "Type": "BannerSecondary",
        "Prop": "#dropdown-searchform .form-control-icon",
        "Css": "background-color",
        "Luminance": "-0.3"
    }, {
        "Type": "BannerSecondary",
        "Prop": ".tile",
        "Css": "background-color",
        "Luminance": "-0.2"
    }, {
        "Type": "BannerSecondary",
        "Prop": ".flip-container .flipper .front .lp-font-icon",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "BannerSecondary",
        "Prop": ".banner-login-icon",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "Banner",
        "Prop": ".flip-container .flipper .front",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "BannerSecondary",
        "Prop": ".flip-container .flipper .back",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "Headlines",
        "Prop": "#welcome",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "HeadlinesText",
        "Prop": "#welcome",
        "Css": "color",
        "Luminance": "0"
    }, {
        "Type": "HeadlinesText",
        "Prop": "#welcome .back-link a",
        "Css": "color",
        "Luminance": "0"
    }, {
        "Type": "Footer",
        "Prop": "footer",
        "Css": "background-color",
        "Luminance": "0"
    }, {
        "Type": "FooterText",
        "Prop": "footer .links ul li a",
        "Css": "color",
        "Luminance": "0"
    }, {
        "Type": "FooterText",
        "Prop": "footer .brand a",
        "Css": "color",
        "Luminance": "0"
    }, {
        "Type": "FooterText",
        "Prop": "footer",
        "Css": "color",
        "Luminance": "0"
    }, {
        "Type": "PrimaryButton",
        "Prop": "#dropdown-searchform .form-control",
        "Css": "color",
        "Luminance": "0"
    }, {
        "Type": "PrimaryButton",
        "Prop": ".flip-container .flipper .front .lp-font-icon",
        "Css": "color",
        "Luminance": "0"
    }, {
        "Type": "PrimaryButton",
        "Prop": " .banner-login-icon",
        "Css": "color",
        "Luminance": "0"
    }]
};



$(function () {
    //var e = '<h2>Template Styles <a href="#"><img class="s-s-icon" src="images/setings.png" alt="Style switcher" /></a></h2>     <div class="content2">   <h3>Color Style</h3>     <div class="switcher-box">     <a id="yellow" class="styleswitch color current"></a> <a id="aqua" class="styleswitch color"></a> <a id="blue" class="styleswitch color"></a> <a id="nice" class="styleswitch color"></a>  <a id="red" class="styleswitch color"></a>   <a id="orange" class="styleswitch color"></a> 	<a id="green" class="styleswitch color"></a>          </div><!-- End switcher-box -->     <div class="clear"></div>      <div class="bg hidden">      <h3>Background Image</h3>      <a id="bg-1" class="pattern current"></a>     <a id="bg-2" class="pattern"></a>     <a id="bg-3" class="pattern"></a>     <a id="bg-4" class="pattern"></a>     <a id="bg-5" class="pattern"></a>     <a id="bg-6" class="pattern"></a>     <a id="bg-7" class="pattern"></a>     <a id="bg-8" class="pattern"></a>     <a id="bg-9" class="pattern"></a>     <a id="bg-10" class="pattern"></a>   </div>     <div class="clear"></div>   <h3>Layout Style</h3> 	<div class="layout-switcher">     <a id="wide" class="layout current button ">WIDE</a>     <a id="boxed" class="layout button ">BOXED</a>     </div>     <div class="clear"></div>  <br> 	<a id="reset" class="dark-style button ">RESET</a>     </div><!-- End content --> 	';
    //$(".switcher").prepend(e)

    //hideThemeSwicher();
    loadThemeTemplates();
    var colorpallet = $("#color, #background").find(".color-text");
    $.each(colorpallet, function (index, element) {
        createPikerControl(element);
    });

    //Update Search Control:
    var $content = $("#iframe-website").contents();
    $content.find("#txtGlobalSearh").val("Search...");

    $("#iframe-website, #tc_theme").css("height", $(window).height() + "px");
});

function loadThemeTemplates() {
    //Load the Panel Template
    var source = $("#theme-tile-template").html();

    if (source != undefined) {
        var template = Handlebars.compile(source);
        var html = template(theme.data);
        $(html).prependTo("#theme");
    }

    var sourceBg = $("#theme-imagebg-template").html();

    if (sourceBg != undefined) {
        var templateBg = Handlebars.compile(sourceBg);
        var htmlBg = templateBg(theme.data.background);
        $("#theme-BackgroundImage").html(htmlBg);
    }

    var elems = Array.prototype.slice.call(document.querySelectorAll('.js-switch'));
    elems.forEach(function (html) {
        var switchery = new Switchery(html);
    });

}

function colorLuminance(hex, lum) {
    //ColorLuminance("#69c", 0);		// returns "#6699cc"
    //ColorLuminance("6699CC", 0.2);	// "#7ab8f5" - 20% lighter
    //ColorLuminance("69C", -0.5);	// "#334d66" - 50% darker
    //ColorLuminance("000", 1);		// "#000000" - true black cannot be made lighter!


    // validate hex string
    hex = String(hex).replace(/[^0-9a-f]/gi, '');
    if (hex.length < 6) {
        hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
    }
    lum = lum || 0;

    // convert to decimal and change luminosity
    var rgb = "#", c, i;
    for (i = 0; i < 3; i++) {
        c = parseInt(hex.substr(i * 2, 2), 16);
        c = Math.round(Math.min(Math.max(0, c + (c * lum)), 255)).toString(16);
        rgb += ("00" + c).substr(c.length);
    }
    return rgb;
}

function createPikerControl(obj) {

    $(obj).spectrum({
        color: "#000000",
        showInput: true,
        containerClassName: "full-spectrum",
        showInitial: true,
        showPalette: true,
        showSelectionPalette: true,
        maxPaletteSize: 10,
        preferredFormat: "hex",
        change: function (color) {
            var colorLabel = $(this).parent().find(".color-cont").removeClass("color-validation-error");
            $(colorLabel).val(color);
            theme.website.updateStyle($(this).parent().attr("id"), color.toString());
        },
        move: function (color) {

        },
        show: function () {

        },
        beforeShow: function () {

        },
        hide: function (color) {
            //updateBorders(color);
        },

        palette: [
            ["rgb(0, 0, 0)", "rgb(67, 67, 67)", "rgb(102, 102, 102)",
            "rgb(204, 204, 204)", "rgb(217, 217, 217)", "rgb(255, 255, 255)"],
            ["rgb(152, 0, 0)", "rgb(255, 0, 0)", "rgb(255, 153, 0)", "rgb(255, 255, 0)", "rgb(0, 255, 0)",
            "rgb(0, 255, 255)", "rgb(74, 134, 232)", "rgb(0, 0, 255)", "rgb(153, 0, 255)", "rgb(255, 0, 255)"],
            ["rgb(230, 184, 175)", "rgb(244, 204, 204)", "rgb(252, 229, 205)", "rgb(255, 242, 204)", "rgb(217, 234, 211)",
            "rgb(208, 224, 227)", "rgb(201, 218, 248)", "rgb(207, 226, 243)", "rgb(217, 210, 233)", "rgb(234, 209, 220)",
            "rgb(221, 126, 107)", "rgb(234, 153, 153)", "rgb(249, 203, 156)", "rgb(255, 229, 153)", "rgb(182, 215, 168)",
            "rgb(162, 196, 201)", "rgb(164, 194, 244)", "rgb(159, 197, 232)", "rgb(180, 167, 214)", "rgb(213, 166, 189)",
            "rgb(204, 65, 37)", "rgb(224, 102, 102)", "rgb(246, 178, 107)", "rgb(255, 217, 102)", "rgb(147, 196, 125)",
            "rgb(118, 165, 175)", "rgb(109, 158, 235)", "rgb(111, 168, 220)", "rgb(142, 124, 195)", "rgb(194, 123, 160)",
            "rgb(166, 28, 0)", "rgb(204, 0, 0)", "rgb(230, 145, 56)", "rgb(241, 194, 50)", "rgb(106, 168, 79)",
            "rgb(69, 129, 142)", "rgb(60, 120, 216)", "rgb(61, 133, 198)", "rgb(103, 78, 167)", "rgb(166, 77, 121)",
            "rgb(91, 15, 0)", "rgb(102, 0, 0)", "rgb(120, 63, 4)", "rgb(127, 96, 0)", "rgb(39, 78, 19)",
            "rgb(12, 52, 61)", "rgb(28, 69, 135)", "rgb(7, 55, 99)", "rgb(32, 18, 77)", "rgb(76, 17, 48)"]
        ]
    });
}

theme.getStyleProp = function (type) {
    var retVal = [];
    $.each(theme.data.style.StyleList, function (index, element) {
        if (element.Type == type)
            retVal.push(element);
    });
    return retVal;
}

$(document).on("change", ".js-check-change", function () {
    backgroundImageCheckChange(this.checked);
});

function backgroundImageCheckChange(isChecked) {
    var colorElementCtl = "theme-BackgroundImage";
    $("#" + colorElementCtl).find(".color-cont").val("");
    if (isChecked) {
        $(".pattern-scroller").find(".color-image-background").addClass("color-imagebg").removeClass("color-image-background");
    } else {
        $(".pattern-scroller").find(".color-imagebg").addClass("color-image-background").removeClass("color-imagebg");
        theme.website.updateStyle(colorElementCtl, "none");
        $(".color-imagebg, .color-image-background").removeClass("selected");
    }
}

$(document).on("click", ".switcher .switcher-setting", function (e) {
    e.preventDefault();

    var t = $(".switcher");
    if (t.css("left") === "-375px") {
        $(".switcher").animate({
            left: "0px"
        })
    } else {
        $(".switcher").animate({
            left: "-375px"
        })
    }
});

$(document).on("click", ".btn-theme-cancel", function (e) {
    e.preventDefault();
    hideThemeSwicher();
});

$(document).on("click", ".btn-theme-apply", function (e) {
    e.preventDefault();
    var btn = $(this);
    btn.button('loading');

    var inputData = [],
        themeId = $(this).data("themeid");

    $.each(theme.data.theme[0], function (key, Css) {
        $.each(theme.getStyleProp(key), function (ind, el) {
            var _tval = $("#theme-" + key).find(".color-cont").val()/*.replace(/\//g, "-").replace(/\(/g, "|").replace(/\)/g, "|")*/;
            if (_tval != undefined && _tval != null && _tval.length != 0) {
                inputData.push({
                    Style: el.Prop,
                    Porp: el.Css + ":" + (el.Luminance == 0 ? _tval : colorLuminance(_tval, el.Luminance))
                });
            }
        });
    });


    var iData = {
        CurrentStylesheetName: ($.cookie()['acatheme'] == undefined ? "invalid" : $.cookie()['acatheme']),
        StyleProperties: inputData
    };

    $.ajax({
        url: "/CssHandler.ashx",
        contentType: "application/json; charset=utf-8",
        data: { 'InputData': jQuery.stringify(iData) },
        dataType: "json",
        responseType: "json",
        success: function (response) {
            //alert(response.Guid + ".css");
            $.cookie('acatheme', response.Guid, { expires: 365, path: '/' });
            //$("#iframe-website head link#theme-switcher-stylesheet").attr("href", '/Content/theme/' + response.Guid + '.css');

            $("#theme-action-sec").append('<div class="alert alert-info fade in text-left" style="margin-top: 10px;" role="alert"><button type="button" class="close" data-dismiss="alert"><span class="ico-close3"></span></button><strong>Saved & Applied!</strong><br />"' + response.Guid + '.css" has been create and applied.</div>');
            $("#iframe-website").contents().find("head link#theme-switcher-stylesheet").attr("href", '/Content/theme/' + response.Guid + '.css');
            btn.button('reset');
        },
        error: function (response) {
            btn.button('reset');
            $("#theme-action-sec").append('<div class="alert alert-info fade in text-left" style="margin-top: 10px;" role="alert"><button type="button" class="close" data-dismiss="alert"><span class="ico-close3"></span></button><strong>Saved & Applied!</strong><br />"eer34-efv42-34rvtt-3fbgd-56gtu.css" has been create and applied.</div>');
        }
    });

});

$(document).on("click", ".color-imagebg", function (e) {
    e.preventDefault();
    var ctl = $(this).parent().find(".color-cont"),
        parentId = $(this).parent().attr("id"),
        $this = $(this),
         url = $(this).data("bgurl");

    if ($this.hasClass("selected")) {
        $this.removeClass("selected");
        ctl.val("");
        theme.website.updateStyle(parentId, "");
    } else {
        ctl.val(url);
        $(".color-imagebg").removeClass("selected");
        $this.addClass("selected");
        theme.website.updateStyle(parentId, url);
    }

});

$(document).on("click", ".theme-style", function(e) {
    e.preventDefault();
    var themeId = $(this).attr("id"),
        rec = [];

    $(".btn-theme-apply").data("themeid", themeId);

    $.each(theme.data.theme, function(index, element) {
        if (element.ThemeId == themeId)
            rec.push(element);
    });

    var updatedStyles = [];
    $.each(rec, function(ind, obj) {
        $.each(obj, function(key, value) {
            var controlId;
            if (key == "BackgroundImage") {
                //Select the backgrounf Image pattern
                controlId = "theme-" + key;
                updatedStyles = updatedStyles.concat(theme.website.updateStyle(controlId, value));
                if (value != "none") {
                    $('.js-check-change').attr("checked", true);
                    backgroundImageCheckChange(true);
                } else {
                    $('.js-check-change').removeAttr('checked');
                    backgroundImageCheckChange(false);
                }

                $(".switchery").remove();
                var elems = Array.prototype.slice.call(document.querySelectorAll('.js-switch'));
                elems.forEach(function(html) {
                    var switchery = new Switchery(html);
                });

                $("#" + controlId).find(".color-cont").val(value);
            } else {
                controlId = "theme-" + key;
                updatedStyles = updatedStyles.concat(theme.website.updateStyle(controlId, value));
            }
        });
    });

    $('#switcher-tab a[href="#color"]').parent().show();

    // save items.
    parent.colorThemeChangeItems.updateChangedItem(updatedStyles.join(' '));
    parent.ModifyMark();
});

$(document).on("blur", ".color-cont", function (e) {
    e.preventDefault();
    var ctl = $(this).parent().find(".color-text"), 
        $this = $(this),
        parentId = $(this).parent().attr("id");


    $this.removeClass("color-validation-error");
    if ($this.val().length != 0) {
        var tiny = tinycolor($this.val());
        if (!tiny.ok)
            $this.addClass("color-validation-error");

        $(ctl).spectrum("set", $this.val());
        theme.website.updateStyle(parentId, $this.val());
    }

});

$(document).on("click", '#viewport-switcher img', function (e) {
    var width = $(this).data("width");
    var height = $(this).data("height");

    if (height == "100%") {
        height = $(window).height() + "px";
    }

    $('#viewport-switcher img').removeClass('active');
    $("#iframe-website, #tc_theme").css("width", width).css("height", height);
    $(this).addClass('active');
    theme.website.updateCommonStyle();
});

theme.website.updateCommonStyle = function () {
    var content = $("#iframe-website").contents();
    content.find("body").css("overflow", "hidden");
};

theme.website.updateStyle = function (controlId, color) {
    var $content = $("#iframe-website").contents();
    var cId = controlId.split("-")[1];
    var styles = theme.getStyleProp(cId);

    var val;
    var arr = [];

    $.each(styles, function (ind, el) {
        val = (el.Luminance == 0 ? color : colorLuminance(color, el.Luminance));
        $content.find(el.Prop).css(el.Css, val);
        arr.push(el.Prop + "{" + el.Css + ":" + val + "}");
    });

    return arr;
};

