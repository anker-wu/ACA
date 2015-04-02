/**
* <pre>
* 
*  Accela
*  File: GlobalSearch.js
* 
*  Accela, Inc.
*  Copyright (C): 2009-2014
* 
*  Description:
* To deal with expression logic in ACA model
*  Notes:
* $Id: GlobalSearch.js 79503 2007-11-08 07:04:56Z ACHIEVO\jackie.yu $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
*  08/21/2009     		cary.cao				Initial.
* </pre>
*/

var Class={
    create:function(){
        return function(){this.initialize.apply(this,arguments);}
    }
};

function SetTimeOut(func, ms, param) {
    var args = Array.prototype.slice.call(arguments,2);
    
    if (typeof(func) == "function") {
        var callback = function(){
            func.apply(null,args);
        }
    
        return window.setTimeout(callback,ms);
    }else{
        return window.setTimeout(func,ms);
    }
}

function HTMLEncode(html)
{
    var temp = document.createElement ("div");
    (temp.textContent != null) ? (temp.textContent = html) : (temp.innerText = html);
    var output = temp.innerHTML;
    temp = null;
    return output;
}

function HTMLDecode(text)
{
    var temp = document.createElement("div");
    temp.innerHTML = text;
    var output = temp.innerText || temp.textContent;
    temp = null;
    return output;
}

function GotoResultPage(queryText) {
    var queryText = encodeURIComponent(queryText);
    var isNewQueryPart = queryText != "" ? "isNewQuery=yes&" : "";
    var url = rootURL + "Cap/GlobalSearchResults.aspx?" + isNewQueryPart + "QueryText=" + queryText;
    window.location.href = url;
}

function RedirectToResultPage(index) {
    if (typeof(queryKeys) == "undefined" || queryKeys.length<index+1) {
        return;
    }
    var key = queryKeys[index].toString();
    if (typeof (SetNotAsk) != "undefined" && document.all) {
        SetNotAsk();
    }
    GotoResultPage(key);
}

//********************************************
// global search history list
//********************************************
var GlobalSearchHistory = Class.create();
GlobalSearchHistory.prototype = {
    initialize:function (id,noResult,searchResult) {
        this.id = id;
        this.maxLength = 20;
        this.container = $("#" +this.id);
        this.noResultText = noResult;
        this.searchResultText = searchResult;
        this.queryKeys = null; //HistoryItem List
        this.queryTexts = null;
    },
    rebuild: function (propList) {
        if (globalSearch != null && queryKeys.length > 0) {
            var lastQueryText = queryKeys[0].toString();
            globalSearch.setText(lastQueryText);
        }
        
        this.clearAll();
        if (this.queryTexts != null) {
            for(var i = 0; i<this.queryTexts.length; i++){
                //var key = this.queryKeys[i].toString();
                var text = this.queryTexts[i].toString();
                this.insert(i,text);
            }
        }
    },
    setNoResult:function () {
        this.clearAll();
        var dom = $("<div><p><i><a onfocus=\"SetFocusInForSearchHistory();\" class=\"NotShowLoading\" style=\"text-decoration:none;cursor:default;color:#666666;font-weight:normal;\" onblur=\"SetBlurOutForSearchHistory();\" href='javascript:void(0);'>" + this.noResultText + "</a></i></p></div>");
        this.container.append(dom);
    },
    clearAll:function () {
        this.container.empty();
        var dom = $("<div class='Header_h4' style='width:100%'>" + this.searchResultText + "</div>");
        this.container.append(dom);
    },
    insert:function (index,quertText) {
        var item = new HistoryItem(index,quertText);
        this.container.append(item.dom);
    }
};

var HistoryItem = Class.create();
HistoryItem.prototype = {
    initialize:function (index,text) {
        this.key = null;
        this.queryText = text;
        this.dom = null;
        this.index = index;
        this.maxLength = 20;
        this.createElement();
    },
    createElement:function () {
        var html = new Array();
        html[html.length] = "<div style='margin-top:2px; margin-bottom:2px;'>";
        html[html.length] = "<p><a";
        html[html.length] = " onfocus=\"SetFocusInForSearchHistory();\" onblur=\"SetBlurOutForSearchHistory();\" href=javascript:RedirectToResultPage("+this.index+");";
        html[html.length] = ">";
        html[html.length] = HTMLEncode(this.queryText);
        html[html.length] = "</a></p>";
        html[html.length] = "</div>";
        
        this.dom = $(html.join(""));
    }
};
//********************************************
// watermark text box interface
//********************************************
var GlobalSearch=Class.create();
var oToolTips;
var moveFlag = false;
GlobalSearch.prototype = {
    initialize:function (inputBoxId, searchBoxId, watermarkText, msg) {
        this.inputBoxId = inputBoxId; // the input text box id
        this.searchBtnId = searchBoxId; // search button id
        this.watermarkText = watermarkText; // the watermark
        this.minLimitMessage = msg;
        this.maxLength = 200;
        this.minLength = 3;
        this.inputBox = null;
        this.searchBtn = null;
        instance = this;
        // verify the control
        this.check();
        $("#" + this.inputBoxId).attr("maxLength",this.maxLength);
        // set title
        $("#" + this.inputBoxId).attr("title",this.minLimitMessage);
        // set watermark to input box
        $("#" + this.inputBoxId).Watermark(this.watermarkText);
        // bind events
        this.bindEvents();
        // Indicate whether the search button should be disabled.
        this.disabledSearchButton();
    },
    getText:function () {
        if ($.trim(this.inputBox.value) == this.watermarkText) {
            return "";
        }else{
            return $.trim(this.inputBox.value);
        }
    },
    setText:function(queryText){
        $(this.inputBox).removeClass("watermark");
        this.inputBox.value = queryText;
        this.disabledSearchButton();
    },
    disabledSearchButton:function() {
        var isDisabled = true;

        if (this.getText().length < this.minLength) {
            isDisabled = true;
        } else {
            isDisabled = false;
        }

        var cursor = isDisabled? "default" : "pointer";
        var imgUrl = isDisabled? searchDisabledImg : searchImg;
        var bg = "url(" + imgUrl + ") no-repeat";
        var ctl = $(this.searchBtn).children("img");

        if (ctl) {
            if (ctl.length > 0) {
                ctl[0].src = imgUrl;
            }

            ctl.css({ "cursor": cursor, "background": bg });
        }

        DisableButton(this.searchBtnId, isDisabled);

        if (isDisabled) {
            $(this.searchBtn).removeAttr("href");
        }
    },
    bindEvents: function () {
        $(this.searchBtn).bind("click", function () {
            if(instance.getText() == ""){
                return false;
            }else if (instance.getText().length <instance.minLength) {
                $(instance.inputBox).after(oToolTips);
                moveFlag = true;
                return false;
            } else {
                ShowLoading();
                GotoResultPage(instance.getText());
                return false;
            }
        });
        $(instance.inputBox).keyup(function(e){
           instance.disabledSearchButton();
        });
       $(instance.inputBox).keypress(function (e) {
            if(e.keyCode == "13"){
                e.stopPropagation();
            
                if (instance.getText() == "") {
                    return false;
                }

                if (instance.getText().length >= instance.minLength) {
                    ShowLoading();
                    GotoResultPage(instance.getText());
                }else{
                    $(instance.inputBox).after(oToolTips);
                    moveFlag = true;
                }
                return false;
            }
       });
    },
    check:function () {
        this.inputBox = $get(this.inputBoxId);
        this.searchBtn = $get(this.searchBtnId);
        
        if (this.inputBox == null || this.searchBtn == null) {
            alert("Error: can not find the input box id.");
        }
    }
};

var watermark = {Text:"",TextBox:null};

(function($){
    $.Watermark = {
        show: function() {
            if (watermark.TextBox.val() == "") {
                watermark.TextBox.val(watermark.Text);
            }
        },
        hide: function() {
            if (watermark.TextBox.val() == watermark.Text) {
                watermark.TextBox.val("");
            }
        }
    };
    
    $.fn.Watermark = function(text) {
        return this.each(function(){
            var input=$(this);
            
            function clear() {
                if($.trim(input.val()) == text){
                    input.val("");
                    input.removeClass("watermark");
                }
            }
            function write() {
                if($.trim(input.val()) == "" || $.trim(input.val()) == text){
					input.addClass("watermark");
					input.val(text);
				}else{
					input.removeClass("watermark");
				}	
            }
            
            // initial
            input.focus(clear);
			input.blur(write);								
			input.change(write);
				
			write();
        });
    };
})(jQuery);

//********************************************
// ui layout control
//********************************************
var Popup = Class.create();
Popup.prototype = {
    initialize:function(config){
        this.param = jQuery.extend({
                oTrigger:null,
                oTarget:null, // if null, auto generate
                oBlock:null, // if not null, the unblankLayer will covered this area
                align: 'left', // left or right
                loadingMsg:'Loading...',
                offsetLeft: 0, // offset left of the trigger control
                offsetTop: 0, // offset top of the trigger control
                isCenter: false, // if true, the offsetLeft and offsertTop will not be used
                unblankLayer: false // if true, the user can not click other area of current page
        }, config);
        this.position = {left:0, top:0};
        this.oBackground = null;
        this.timer = null;
        this.isCancel = false;
        
        if (typeof(this.param.oTrigger) == "string") {
            this.param.oTrigger = document.getElementById(this.param.oTrigger);
        }
        if (typeof(this.param.oTarget) == "string") {
            this.param.oTarget =  document.getElementById(this.param.oTarget);
        }else if (this.param.oTarget == null) {
            var targetId = this.param.oTrigger.id +"_target";
            var html ="<div></div>";
            var divLoadingTemplate = document.getElementById("divLoadingTemplate");
            if (divLoadingTemplate) {
                html = '<table role="presentation" name="gv_loading_container" id="'+targetId+'" class="ACA_Loading_Message" style="width:auto"><tr><td>';
                html +=divLoadingTemplate.innerHTML;
                html +='</td></tr></table>';
            }
            this.param.oTarget = $(html);
            $(this.param.oTrigger).after(this.param.oTarget);
        }
        
         if (typeof(this.param.oBlock) == "string") {
            this.param.oBlock =  document.getElementById(this.param.oBlock);
        }else if (this.param.oBlock == null) {
            this.param.oBlock = this.param.oTrigger;
        }
        
        $(this.param.oTarget).css({'position' : 'absolute', 'z-index' : '9999'});
    },
    getWidth:function () {
        var oTip = $("<div></div>");
        var oContainer = $("<table role='presentation'><tr><td><div>" + this.param.oTarget.innerHTML + "</div></td></tr></table>");
        var temp = $(oTip).append(oContainer);
        $("#gsContainer").after(temp);
        
        var width = oContainer.innerWidth();
        $(temp).remove();
        
        return width;
    },
    show:function() {
        if ($.global.isAdmin) {
            return;
        }
        this._setPosition();
        this._showBackground();
        
        $(this.param.oTarget).css('top', this.position.top + 'px').css('left', this.position.left + 'px').css('display', 'block');
    },
    hide:function(instance) {
        if ($.global.isAdmin) {
            return;
        }

        if (instance && instance.isCancel == false) {
            $(instance.param.oTarget).css('display', 'none');
        }else if(this.param != null && this.param.oTarget != null){
            $(this.param.oTarget).css('display', 'none');
            this._hideBackground();
        }
    },
    hideMenu:function () {
        clearTimeout(this.timer);
        if (this.timer) {
            this.timer = null;
        }else{
            this.isCancel = false;
		    this.timer = SetTimeOut(this.hide,1000, this);
        }
    },
    cancelHideMenuEvent:function () {
         clearTimeout(this.timer);
		this.timer = null;
		this.isCancel = true;
    },
    _setPosition:function() {
        if (this.param.isCenter) {
            var st,sl,outerWidth,outerHeight,targetWidth,targetHeight;

            if(this.param.oBlock){
                st=$(this.param.oBlock).position().top;
	            sl=$(this.param.oBlock).position().left;
                outerWidth = $(this.param.oBlock).outerWidth();
                outerHeight = $(this.param.oBlock).outerHeight();
            }else{
                st=$(document.documentElement).scrollTop();
	            sl=$(document.documentElement).scrollLeft();
                outerWidth = $(document.documentElement).outerWidth();
                outerHeight = $(document.documentElement).outerHeight();
            }
            targetWidth=$(this.param.oTarget).width();
            targetHeight=$(this.param.oTarget).height();
            
            this.position.left = Number(sl)+(Number(outerWidth)-Number(targetWidth))/2;
            this.position.top = Number(st)+(Number(outerHeight)-Number(targetHeight))/2;
        }else{
            try{
                var pos = $(this.param.oTrigger).position();
                var height = height = $(this.param.oTrigger).outerHeight();
                var width = $(this.param.oTrigger).outerWidth();
                var w = $(this.param.oTarget).outerWidth();
                
                if((this.param.align == "left" && $.global.isRTL == false) || (this.param.align == "right" && $.global.isRTL == true)){
                    this.position.left = pos.left + this.param.offsetLeft;
                }else{
                    this.position.left = pos.left + width + this.param.offsetLeft - w;
                }
                
                this.position.left -= $(document.documentElement).scrollLeft();
                
                this.position.top = pos.top + height + this.param.offsetTop;
            }catch(e){}
        }
    },
    _showBackground:function() {
        if (this.param.unblankLayer) {
            this.oBackground = $("<div name ='gv_loading_container_bg' id='"+this.param.oTrigger.id +"_target_bg"+"' style='z-index:9998; position:absolute; filter:Alpha(Opacity=30); -moz-opacity:0.4; opacity:0.4; background-color: gray'></div>");
            var w,h, top,left;
            
            if(this.param.oBlock){
                w = $(this.param.oBlock).width();
                h = $(this.param.oBlock).height();
                top = $(this.param.oBlock).position().top;
                left = $(this.param.oBlock).position().left;
            }else{
                w = $("body").width();
                h = $("body").height();
                top = "0px";
                left = "0px";
            }
            
            this.oBackground.css({'display':'block', 'width':w, 'height':h, 'top':top, 'left':left});
	        $(this.param.oTarget).after(this.oBackground);
        }
    },
    _hideBackground:function() {
        if (this.param.unblankLayer && this.oBackground) {
            this.oBackground.remove();
        }
    }
};

(function($){
    $.global = {isRTL : false, isAdmin : false};
    
    // Examples:
    // #1: $("input").showTips();
    // #2: $("input").showTips({msg:'this is the example text'});
    // #3: $("input").showTips({eventName:'click', cssName:'myClassName'});
    $.fn.showTips = function(config){
        $(this).each(function(){
            var options = jQuery.extend({
                cssName : "ui-tooltips",
                tipsPosition:"up", // -- up or down
                tipsWidth: $(this).outerWidth(),
                tipsHeight: 20,
                msg : $(this).attr("title"),
                eventName:"mouseover"
                },
                config);
            
            // check if the title is null or msg parameter is empty
            if(!options.msg)
            {
                // please set text to [title] attribute or refer to sample #2
                return;
            }
            
            // remove title attribute
            $(this).removeAttr("title");
            
            // get the container's width with the tips
            var oTip = $("<div class='ui-tooltips-container'></div>");
            var oContainer = $("<table role='presentation'><tr><td><div>" + options.msg + "</div></td></tr></table>").attr("class",options.cssName);
            var temp = $(oTip).append(oContainer);
            $("#gsContainer").after(temp);
            
            if (oContainer.outerHeight() > options.tipsHeight) {
                options.tipsHeight = oContainer.outerHeight();
            }
            
            options.tipsWidth = 160;
            //options.tipsWidth = (oContainer.innerWidth() < 145) ? 145 : oContainer.outerWidth();
            $(temp).remove();
            
            // show tips
            $(this).bind(options.eventName, function(){
                if (moveFlag) {
                    $(oToolTips).remove();
                    moveFlag = false;
                }
                var oTipContainer = $("<div class='ui-tooltips-container'></div>");
                var oTipMessage = $("<div>" + options.msg + "</div>").attr("class",options.cssName).css("width",options.tipsWidth + "px");
                var oPointer = null;
                if (options.tipsPosition == "up") {
                    oPointer = $("<div class='ui-tooltips-pointer-up'><div class='ui-tooltips-pointer-up-inner'></div></div>");
                }else{
                    oPointer = $("<div class='ui-tooltips-pointer-down'><div class='ui-tooltips-pointer-down-inner'></div></div>");
                }
                
                 oToolTips = $(oTipContainer).append(oTipMessage).append(oPointer);
                 $(this).after(oToolTips);
                 
                 var position = $(this).position();
                 var top = null;
                 if (options.tipsPosition == "up") {
                    top = eval(position.top + options.tipsHeight +8);
                 }else{
                    top = eval(position.top - options.tipsHeight -8);
                 }
                 
                 var left = position.left;
                 
                 if ($.global.isRTL) {
                    left += $(this).outerWidth() - options.tipsWidth;
                 }
                 
                 $(oToolTips).css("top" , top + "px").css("left" , left + "px");
            });
            
            // move tips
            $(this).blur(function(){
                $(oToolTips).remove();
            });
            $(this).mouseout(function(){
                $(oToolTips).remove();
            });
        });
        return this;
    };
    
    // Example:
    // $("input").popUp({oTarget:'divPopup', onShowEvent:'click', onHideEvent:'blur'});
    $.fn.popUp = function(config){
        return this.each(function(){
            if (config.oTrigger == null) {
                config.oTrigger = $(this)[0].id;
            }
            
            // if true, when mouse over the div menu, it will cancel the hideMenu event
            if (config.enabledMouseOver == null) {
                config.enabledMouseOver = true;
            }
            
            var popup = new Popup(config);
            
            $(this).bind(config.onShowEvent, function(){
                popup.show();
            });
            
            if (config.onShowEvent != "click") {
                $(this).bind("click", function(){
                popup.show();
            });
            }
            
            $(this).bind(config.onHideEvent, function(){
                popup.hideMenu();
            });
            
            $(this).bind(config.onFocusEvent, function() {
                popup.show();           
            });

            if (config.oTarget != null && IsTrue(config.enabledMouseOver)) {
                $("#" + config.oTarget).bind("mouseover", function(){
                    popup.cancelHideMenuEvent();
                });
                $("#" + config.oTarget).bind("mouseout", function(){
                    popup.hideMenu();
                });
            }
        });
    };
    
})(jQuery);

var container = null;
function showLoadingPanel(trigger) {
    if(typeof(SetNotAsk) == "function"){
        SetNotAsk();
    }
    
    var grid = null;
    var obj = null;
    if (typeof(trigger) == "string") {
        obj = document.getElementById(trigger);
    }else{
        obj = trigger;
    }
    
    $(obj).parents("[loadingpanel]").each(function () {
        grid = this;
        return;
    });
    
    if ($.global.isAdmin || grid == null) {
        return;
    }
    
    container = grid;
    var popup = new Popup({oTrigger:grid,oBlock:grid,isCenter:true,unblankLayer:true});
    popup.show();
}

function hideLoadingPanel() {
    if ($.global.isAdmin) {
        return;
    }

    if (container != null) {
        $("div[name='gv_loading_container_bg']").each(function () {
           $(this).remove();
        });
        $("[name='gv_loading_container']").each(function () {
           $(this).css("display","none");
        });
    }
}
