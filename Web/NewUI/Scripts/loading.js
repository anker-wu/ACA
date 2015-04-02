/*
* <pre>
*  Accela Citizen Access
*  File: loading.js
* 
*  Accela, Inc.
*  Copyright (C): 2014
* 
*  Description:
* 
*  Notes:
*      $Id: MyRecordsController.js 77905 2014-06-12 12:49:28Z ACHIEVO\Awen.deng $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
* </pre>
*/
var loadingControl = function (object) {

    this.jqueryObject = object; //Current jquery Object
    //init control
    this.initControl = function () {
        if ($('.loading_overlay').length <= 0)
            this.jqueryObject.append('<div class="loading_overlay"></div> <div id="loading_AjaxLoading" class="loading_showbox"><div class="loading_loadingWord"><img src="Images/spinner.gif">Loading...</div></div>');
        $(document.body).append(
            '<style type="text/css" id="loading_Style">' +
                '#loading_AjaxLoading{border-radius:2px; color: rgb(51, 51, 51);font-style: normal;font-size:17px;}' +
                '#loading_AjaxLoading div.loading_loadingWord{width:120px; line-height:45px; height:45px;border:1px #fefefe;background:#fff;}' +
                '#loading_AjaxLoading img{margin:15px 10px;float:left;display:inline;}' +
                '.loading_overlay{position:absolute;top:0;right:0;bottom:0;left:0;z-index:998;width:100%;height:100%;_padding:0 20px 0 0;background-color: rgba(0,0, 0, 0.5);display:none;}' +
                '.loading_showbox{position:absolute;top:0;left:0;z-index:9999;opacity:0;filter:alpha(opacity=0);}' +
                '' +
                '</style>');

        var h = this.jqueryObject.height();
        $(".loading_overlay").css({ "height": h + 10 });

    };

    //begin loading
    this.beginLoad = function () {
        this.jqueryObject.resize(function () {
            var pWidth = this.clientWidth;
            var pHeight = this.clientHeight;
            var boxWidth = $(".loading_showbox").width();
            var boxHeight = $(".loading_showbox").height();
            this.style.position = "relative";
            $(".loading_showbox").css({ "left": (pWidth - boxWidth) / 2 });
            $(".loading_showbox").css({ 'margin-top': (pHeight - boxHeight) / 2 + 'px', 'opacity': '1' });
        });

        $(".loading_overlay").css({ 'display': 'block', 'opacity': '0.8' });
        //The first loading
        this.jqueryObject.resize();
    };

    // cance loading
    this.CancelLoad = function() {
        $(".loading_showbox").remove();
        $(".loading_overlay").remove();
        $("#loading_Style").remove();
    };
};

//jQuery resize event
(function ($, window, undefined) { '$:nomunge'; var elems = $([]), jq_resize = $.resize = $.extend($.resize, {}), timeout_id, str_setTimeout = 'setTimeout', str_resize = 'resize', str_data = str_resize + '-special-event', str_delay = 'delay', str_throttle = 'throttleWindow'; jq_resize[str_delay] = 250; jq_resize[str_throttle] = true; $.event.special[str_resize] = { setup: function () { if (!jq_resize[str_throttle] && this[str_setTimeout]) { return false; } var elem = $(this); elems = elems.add(elem); $.data(this, str_data, { w: elem.width(), h: elem.height() }); if (elems.length === 1) { loopy(); } }, teardown: function () { if (!jq_resize[str_throttle] && this[str_setTimeout]) { return false; } var elem = $(this); elems = elems.not(elem); elem.removeData(str_data); if (!elems.length) { clearTimeout(timeout_id); } }, add: function (handleObj) { if (!jq_resize[str_throttle] && this[str_setTimeout]) { return false; } var old_handler; function new_handler(e, w, h) { var elem = $(this), data = $.data(this, str_data); if (!(elem.length > 0 && elem[0] == window)) { data.w = w !== undefined ? w : elem.width(); data.h = h !== undefined ? h : elem.height(); } old_handler.apply(this, arguments); }; if ($.isFunction(handleObj)) { old_handler = handleObj; return new_handler; } else { old_handler = handleObj.handler; handleObj.handler = new_handler; } } }; function loopy() { timeout_id = window[str_setTimeout](function () { elems.each(function () { var elem = $(this), width = elem.width(), height = elem.height(), data = $.data(this, str_data); if (width !== data.w || height !== data.h) { elem.trigger(str_resize, [data.w = width, data.h = height]); } }); loopy(); }, jq_resize[str_delay]); }; })(jQuery, this);