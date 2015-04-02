/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TabBar.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TabUtil.js 261597 2013-11-27 08:24:22Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/20/2009     		cary.cao				Initial.
 * </pre>
 */
function $GetObject(){
    var elements=new Array();
    for(var i=0;i<arguments.length;i++){
        var element = arguments[i];
        if (typeof element == 'string')
            element = document.getElementById(element);
        if (arguments.length == 1) 
            return element;
        elements.push(element);
    }
    return elements;
}

var isIE=(document.all)?true:false;

var Browser={
    IE:     !!(window.attachEvent && !window.opera),
    Opera:  !!window.opera,
    MobileSafari: !!navigator.userAgent.match(/Apple.*Mobile.*Safari/)
  };
  
getCookie = function (sName) {
	var sCookie = " " + document.cookie;
	var sSearch = " " + sName + "=";
	var sStr = null;
	var iOffset = 0;
	var iEnd = 0;
	if (sCookie.length > 0) 
	{
		iOffset = sCookie.indexOf(sSearch);
		if (iOffset != -1) 
		{
			iOffset += sSearch.length;
			iEnd = sCookie.indexOf(";", iOffset)
			if (iEnd == -1) 
				iEnd = sCookie.length;
			sStr = unescape(sCookie.substring(iOffset, iEnd));
		}
	}
	return(sStr);
}  
//setCookie("test","test23",null,"/","achievo.com",false);
setCookie = function (sName, sVal, iDays, sPath, sDomain, bSecure){
    var sExpires;
	if (iDays)
	{
		sExpires = new Date();
		sExpires.setTime(sExpires.getTime()+(iDays*24*60*60*1000));
	}
	document.cookie = sName + "=" + sVal + ((sExpires) ? "; expires=" + sExpires.toGMTString() : "") + 
		((sPath) ? "; path=" + sPath : "") + ((sDomain) ? "; domain=" + sDomain : "") + ((bSecure) ? "; secure" : "");
	
	if (document.cookie.length > 0)
		return true;
}
//delCookie("test","/","acihevo.com");
delCookie=function(sName, sPath, sDomain) {
	if (this.getCookie(sName)) 
	{
		this.setCookie(sName, '', -1, sPath, sDomain);
		return true;
	}
	return false;
}

window.getStyleValue = function (el, style) {
    if (!document.getElementById) return;

    try {
        var value = el.style[saneStyleName(style)];

        if (!value)
            if (document.defaultView)
                value = document.defaultView.
                 getComputedStyle(el, "").getPropertyValue(style);

            else if (el.currentStyle)
                value = el.currentStyle[saneStyleName(style)];

        if (value.indexOf("px") > 0) {
            return parseInt(value.replace("px", ""));
        }

        if (value == "auto") {
            return 0;
        }

        return value;
    } catch (e) { return 0; }
}

window.isInteger=function(str){
  if(str == parseInt(str))   
    return true;
    
  return false;
}


window.saneStyleName=function(style){
    try{
    if(style.indexOf("-")>0){
        if(isIE){
            var lst=style.split('-')[0];
            var sec=style.split('-')[1];
            sec=sec.charAt(0).toUpperCase() + sec.substring(1)
            return lst+sec;
        }
        return style;       
    }
    }catch(e){return null;}
    return null;
}

window.setProperties=function(o,props){
    while(!props[props.length-1]){
        props.length--;
    }
    for(var i=0;i<props.length;i++){
        o[props[i][0]]=props[i][1];
    }
};
window.getAbsolutePos=function(elementId){
    var el = $GetObject(elementId);
    if(el.parentNode == null || el.style.display == 'none') {
        return false;
    }      
    var parent = null;
    var pos = [];     
    var box;     
    if(el.getBoundingClientRect)    //IE
    {         
        box = el.getBoundingClientRect();
        var scrollTop = Math.max(document.documentElement.scrollTop, document.body.scrollTop);
        var scrollLeft = Math.max(document.documentElement.scrollLeft, document.body.scrollLeft);
        return {x:box.left + scrollLeft, y:box.top + scrollTop};
    }else if(document.getBoxObjectFor)    //   firefox  
    {
        box = document.getBoxObjectFor(el); 
        var borderLeft = (el.style.borderLeftWidth)?parseInt(el.style.borderLeftWidth):0; 
        var borderTop = (el.style.borderTopWidth)?parseInt(el.style.borderTopWidth):0; 
        pos = [box.x - borderLeft, box.y - borderTop];
    }else    // safari    
    {
        pos = [el.offsetLeft, el.offsetTop];  
        parent = el.offsetParent;     
        if (parent != el) { 
            while (parent) {  
                pos[0] += parent.offsetLeft; 
                pos[1] += parent.offsetTop; 
                parent = parent.offsetParent;
            }  
        }   
        if (Browser.Opera || ( Browser.MobileSafari && el.style.position == 'absolute' )) { 
            pos[0] -= document.body.offsetLeft;
            pos[1] -= document.body.offsetTop;         
        }    
    }              
        if (el.parentNode) { 
            parent = el.parentNode;
        } else {
            parent = null;
        }
        while (parent && parent.tagName != 'BODY' && parent.tagName != 'HTML') { // account for any scrolled ancestors
            pos[0] -= parent.scrollLeft;
            pos[1] -= parent.scrollTop;
        if (parent.parentNode) {
            parent = parent.parentNode;
        } else {
            parent = null;
        }
    }
    return {x:pos[0], y:pos[1]};
}

window.popUp = function (div, posObj, isRTL) {
    if (div.style.display == "none")
        div.style.display = "";

    var rect = posObj.getBoundingClientRect();
    var left = 0;

    if (!isRTL) {
        left = rect.right - div.offsetWidth;

        if (left < 0) {
            left = 0;
        }
    }
    else {
        left = rect.left;

        if (left + div.offsetWidth > document.body.clientWidth) {
            left = document.body.clientWidth - div.offsetWidth;
        }
    }

    div.style.left = left + 'px';
    div.style.top = rect.bottom + 'px';
}

var Class={
    create:function(){
        return function(){this.initialize.apply(this,arguments);}
    },
    inherit:function(dest,source){
        for(property in source){
            dest[property]=source[property];
        }
        return dest;
    }
};

if (!window.Event){
    var Event=new Object();
}

Class.inherit(Event,{
    observers:false,
    element:function(event){
        return  event.target  ||  event.srcElement;
    },
    stop:function(event,canBubble){
        if(event.preventDefault){
            event.preventDefault();
            if(!canBubble)
                event.stopPropagation();
        }else{
            event.returnValue=false ;
            if(!canBubble)
                event.cancelBubble=true ;
        } 
    },
    _funcList:function(element,name,observer,nav,sign){
        if(element==null) return;
        if(!this.observers)  this .observers=[];
        if(element.addEventListener){
            this.observers.push([element,name,observer,false,sign]);
            element.addEventListener(name,observer,false);
        }else if(element.attachEvent){
            this.observers.push([element,name,observer,false,sign]);
            element.attachEvent('on'+name,observer);
        } 
    },
    removeEvents:function(sign){
        if(!Event.observers) return ;
        for(var i=0;i<Event.observers.length;i++){
            if(Event.observers[i][4]!=sign){
                continue;
            }
            Event.stopObserving.apply(this,Event.observers[i]);
            Event.observers[i][0]=null;
        } 
        Event.observers=false ;
    },
    attachEvent:function(element,type,func,parmObj,sign){
        var element=$GetObject(element);
        if(element==null) return;
        var observer=func;
        if(parmObj){
            observer = function(e){
                func.call(parmObj,e);
            }
        }
        this._funcList(element,type,observer, false,sign);
    },
    removeEvent:function(element,type){
        if(!Event.observers) return ;
        var element=$GetObject(element);
        for(var i=0;i<Event.observers.length;i++){
            if(Event.observers[i][0]==element && Event.observers[i][1]==type){
                 Event.stopObserving.apply(this,Event.observers[i]);
                 Event.observers[i][0]=null;
                 break;
            }
        } 
    },
    //add event with no parms
    observe:function(element,type,func){
        var element = $GetObject(element);
        this ._funcList(element, type, func, false);
    } ,
    //remove event with no parms
    stopObserving:function (element,name, observer){
        var element = $GetObject(element);
        if(element.removeEventListener){
            element.removeEventListener(name,observer,false);
        }else if(element.detachEvent){
            element.detachEvent('on'+name,observer);
        } 
    },
    //check if tagert element is inside current element when mouseover or out
    descendantOf: function(element, ancestor) {
        element = $GetObject(element), ancestor = $GetObject(ancestor);
        var originalAncestor = ancestor;

        if (element.compareDocumentPosition)
        return (element.compareDocumentPosition(ancestor) & 8) === 8;

        if (element.sourceIndex && !Browser.Opera) {
            var e = element.sourceIndex, a = ancestor.sourceIndex,
            nextAncestor = ancestor.nextSibling;
            if (!nextAncestor) {
                do { ancestor = ancestor.parentNode; }
                while (!(nextAncestor = ancestor.nextSibling) && ancestor.parentNode);
            }
            if (nextAncestor && nextAncestor.sourceIndex)
            return (e > a && e < nextAncestor.sourceIndex);
        }

        while (element = element.parentNode)
            if (element == originalAncestor) return true;
            
        return false;
    }
});

window["MzBrowser"]={};(function()

{

  if(MzBrowser.platform) return;

  var ua = window.navigator.userAgent;

  MzBrowser.platform = window.navigator.platform;



  MzBrowser.firefox = ua.indexOf("Firefox")>0;

  MzBrowser.opera = typeof(window.opera)=="object";

  MzBrowser.ie = !MzBrowser.opera && ua.indexOf("MSIE")>0;

  MzBrowser.mozilla = window.navigator.product == "Gecko";

  MzBrowser.netscape= window.navigator.vendor=="Netscape";

  MzBrowser.safari  = ua.indexOf("Safari")>-1;



  if(MzBrowser.firefox) var re = /Firefox(\s|\/)(\d+(\.\d+)?)/;

  else if(MzBrowser.ie) var re = /MSIE( )(\d+(\.\d+)?)/;

  else if(MzBrowser.opera) var re = /Opera(\s|\/)(\d+(\.\d+)?)/;

  else if(MzBrowser.netscape) var re = /Netscape(\s|\/)(\d+(\.\d+)?)/;

  else if(MzBrowser.safari) var re = /Version(\/)(\d+(\.\d+)?)/;

  else if(MzBrowser.mozilla) var re = /rv(\(\d+(\.\d+)?)/;



  if("undefined"!=typeof(re)&&re.test(ua))

    MzBrowser.version = parseFloat(RegExp.$2);

})(); 


window.TabBar_Utils_Loaded=true;

