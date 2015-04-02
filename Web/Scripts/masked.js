/**
 * Accela Automation
 * File: masked.js
 * Accela, Inc.
 * Copyright (C): 2008-2014
 * 
 * Description: This javascript implement the functions of input mask.
 * 
 * Notes:
 * $Id: masked.js 81447 2007-12-12 09:50:19Z ACHIEVO\jack.su $
 * 
 * Revision History:
 * 9/24/2007, Oscar Huang, Initial.
 */
function __AV360MaskAPI()
{
	this.version = "1.0";
	this.instances = 0;
	this.objects = {};
}

var AV360MaskAPI = new __AV360MaskAPI();

function AV360Mask(mask)
{	
	this.__charMap = {
		'#':"[0-9]",
		'U':"[A-Z]",
		'L':"[a-z]",
		'A':"[a-zA-Z]",
		'?':"[0-9a-zA-Z]"
	};
	
	this.__escapecharMap = {
		'`#':"\\#",
		'`U':"\\U",
		'`L':"\\L",
		'`A':"\\A",
		'`?':"\\?"
	};
	
	this.id = AV360MaskAPI.instances++;
	AV360MaskAPI.objects[this.id] = this;
	
	this.__sOriginalMask = mask;
	this.__sMask = "";
	this.__sFormat = "";
	this.__aEscapeChar = new Array();
	this.__buffer;
	this.__resetBuffer;
	this.__target;
	this.__attached = false;
	this.__enableMaskInput = false;
	this.__ref = "AV360MaskAPI.objects['" + this.id + "']";
	
	this.__initialize = function()
	{	
		this.__buildFormatandMask();
	}
	
	this.__buildFormatandMask = function()
	{		
		for(var i = 0 ; i < this.__sOriginalMask.length ; i++)
		{
			var s = this.__sOriginalMask.substr(i,2);
			var bEscapeChar = false;
			
			if (this.__escapecharMap[s] != null)
			{
				bEscapeChar = true;
				this.__sFormat += s.substr(1);
				this.__sMask += s.substr(1);
				this.__aEscapeChar.push(bEscapeChar);
				i++;
				continue;
			}
			
			s = this.__sOriginalMask.charAt(i);
			
			if (this.__charMap[s] != null)
			{
				this.__sFormat += '_';
			} else 
			{
				this.__sFormat += s;
			}
			
			this.__sMask += s;
			this.__aEscapeChar.push(bEscapeChar);
		}
		
		/*
		var text = this.__target.value;
		
		if (text != "")
		{	
			var len = text.length;

			if (len > this.__sFormat.length)
			{
				len = this.__sFormat.length;
				text = text.substr(0,len);
			}

			var arrayBuffer = text.split("");
			
			if (this.__matchMaskRule(arrayBuffer))
			{
				var buffer = arrayBuffer.join("");
				
				if (len < this.__sFormat.length)
				{
					buffer = buffer + this.__sFormat.substr(len);
				}
				
				this.__buffer = buffer.split("");
				this.__resetBuffer = buffer;
				this.__writeBuffer(0);
				
				return ;		
			}
		}

		this.__buffer = this.__sFormat.split("");
		this.__resetBuffer = this.__sFormat;
		this.__writeBuffer(0);
		*/
	}
	
	this.__focus = function(objEvent)
	{
		/*************************************************************************************
		*   If the value of the text was changed via javascript code, We must check whether
		*   the changed value follow the mask rules, If yes, allow to set the value for this text
		*   object, Otherwise the mask doesn't work well.
		**************************************************************************************/
		
		var text = this.__target.value;
		
		if (text != "")
		{	
			var len = text.length;

			if (len > this.__sFormat.length)
			{
				this.__enableMaskInput = false;
				
				return ;
			}

			var arrayBuffer = text.split("");
			
			if (this.__matchMaskRule(arrayBuffer))
			{
				var buffer = arrayBuffer.join("");
				
				if (len < this.__sFormat.length)
				{
					buffer = buffer + this.__sFormat.substr(len);
				}
				
				this.__enableMaskInput = true;
				this.__buffer = buffer.split("");
				this.__resetBuffer = buffer;
				this.__writeBuffer(len);
						
			}
			else
			{
				this.__enableMaskInput = false;
			}
		}
		else
		{
			this.__enableMaskInput = true;
			this.__buffer = this.__sFormat.split("");
			this.__resetBuffer = this.__sFormat;
			this.__writeBuffer(0);
		}
		
		//var pos = this.__getCursorPos().begin;
		
		//this.__writeBuffer(pos);
	}

	this.__keydown = function (objEvent) {
	    if (!this.__enableMaskInput) {
	        return;
	    }

	    var k = 0;
	    var nPos = this.__getCursorPos().begin;
	    var env = window.event || objEvent;
	    k = env.keyCode || env.which;


	    if (k == 13) //enter
	    {
	        this.__formatBufferOnLostFocus();

	        //We should allow user to submit the form when the enter key was pressed down.
	        return true;
	    }

	    if (k == 8) //backspace
	    {
	        this.__moveLeft(nPos);

	        return false;
	    }

	    if (k == 27) // allow to send 'ESC' command
	    {
	        this.__buffer = this.__sFormat.split("");
	        this.__writeBuffer(0);

	        return false;
	    }

	    if (k == 39) // Move Left
	    {
	        s = this.__sMask.charAt(nPos);

	        if (this.__buffer[nPos] == '_' && this.__charMap[s] != null) {
	            return false;
	        }
	    }

	    if (k == 46) // clean the selected text
	    {
	        var range = this.__getCursorPos();
	        var c;

	        for (var i = range.begin; i < range.end; i++) {
	            c = this.__sMask.charAt(i);

	            if (this.__charMap[c] != null) {
	                this.__buffer[i] = "_";
	            }
	        }

	        this.__writeBuffer(range.begin);

	        return false;
	    }

	    //ignore (k < 16 || (k > 16 && k < 32 ) || (k > 32 && k < 41));
	    //if ((k < 16) || (k > 16 && k < 32) || (k > 32 && k < 41)) return ;
	}

	this.__keypress = function (objEvent) {
	    if (!this.__enableMaskInput) {
	        return;
	    }

	    var env = window.event || objEvent;
	    var nPos = this.__getCursorPos().begin;
	    var k = env.keyCode || env.which;

	    while (nPos < this.__sFormat.length) {
	        c = this.__sMask.charAt(nPos);

	        if (this.__charMap[c] != null && !this.__aEscapeChar[nPos]) {
	            var ch = this.__getEnteredCharacter(env);
	            var regExpChar = new RegExp(this.__charMap[c]);

	            //convert the letter associate with the mask format automatically.
	            if (this.__sMask.charAt(nPos) == 'U') {
	                ch = ch.toUpperCase();
	            }
	            else if (this.__sMask.charAt(nPos) == 'L') {
	                ch = ch.toLowerCase();
	            }

	            if (ch.match(regExpChar)) {
	                this.__buffer[nPos] = ch;
	                this.__writeBuffer(nPos + 1);
	            }
	            break;
	        }
	        else {
	            nPos++;
	        }
	    }

	    //We should allow user to submit the form when the enter key was pressed down.
	    if (k == 13) return true;

	    if (window.event) window.event.returnValue = false;

	    return false;
	}

	this.__blur = function (objEvent) {
	    if (!this.__enableMaskInput) {
	        return;
	    }

	    /*
	    var sText = this.__target.value;
		
	    if (sText == this.__sFormat)
	    {
	    this.__target.value = "";
	    }
	    */
	    this.__formatBufferOnLostFocus();

	}

	this.__oncut = function (objEvent) {
	    if (!this.__enableMaskInput) {
	        return;
	    }

	    var data = document.selection.createRange().text;
	    var range = this.__getCursorPos();
	    var c;

	    for (var i = range.begin; i < range.end; i++) {
	        c = this.__sMask.charAt(i);

	        if (this.__charMap[c] != null) {
	            this.__buffer[i] = "_";
	        }
	    }

	    this.__writeBuffer(range.begin);

	    window.clipboardData.setData("Text", data);
	}

	this.__onclick = function (objEvent) {
	    if (!this.__enableMaskInput) {
	        return;
	    }

	    var destPos = -1;
	    var c;

	    var nPos = this.__getCursorPos().begin;

	    for (var i = 0; i < nPos; i++) {
	        c = this.__buffer[i];
	        s = this.__sMask.charAt(i);

	        if (c == '_' && this.__charMap[s] != null) {
	            destPos = i;
	            break;
	        }
	    }

	    //Move the cursor to the proper position.
	    if (destPos != -1) {
	        this.__setCursorPos(destPos);
	    }
	}

	this.__onresetmask = function () {
	    if (!this.__enableMaskInput) {
	        return;
	    }

	    this.__buffer = this.__resetBuffer.split("");
	}
	
	this.__getCursorPos = function()
	{
		var result = {begin: 0, end: 0 };

		if (this.__target.setSelectionRange)
		{
			result.begin = this.__target.selectionStart;
			result.end  = this.__target.selectionEnd;
		}
		else if (document.selection && document.selection.createRange)
		{
			var range = document.selection.createRange();			
			result.begin = 0 - range.duplicate().moveStart('character', -100000);
			result.end = result.begin + range.text.length;
		}
		
		return result;
	}

	this.__setCursorPos = function (pos) {
	    if (pos < 0 || pos > this.__buffer.length) {
	        return;
	    }

	    if (this.__target.setSelectionRange) {
	        this.__target.focus();
	        this.__target.setSelectionRange(pos, pos);
	    }
	    else if (this.__target.createTextRange) {
	        var range = this.__target.createTextRange();
	        range.collapse(true);
	        range.moveEnd('character', pos);
	        range.moveStart('character', pos);
	        range.select();
	    }
	}
	
	/**
	 * When we delete character. If there are value on the right the deleted character, moves it left.
	 * @param {Object} currPos
	 */
	this.__moveLeft = function(currPos)
	{
		while(currPos > 0)
		{
			c = this.__sMask.charAt(currPos - 1);

			if (this.__charMap[c] != null && !this.__aEscapeChar[currPos - 1])
			{
				var buffer2 = new Array(this.__buffer.length);
				var destPos = currPos ;

				for(var i = 0 ; i < this.__buffer.length ; i++)
				{	
					//If there are value on the left the deleted character, keep it fixedly.
					if (i < currPos - 1)
					{
						buffer2[i] = this.__buffer[i];
					}
					else
					{
						//If there are value on the right the deleted character ,move it left.					
						c1 = this.__sMask.charAt(i);
						
						if (destPos < this.__buffer.length
							&& !this.__aEscapeChar[destPos]
							&& this.__charMap[c1] != null)
						{
							var bFound = false;
								
							while(destPos < this.__buffer.length)
							{
								c2 = this.__sMask.charAt(destPos);
								
								if(this.__charMap[c2] != null && !this.__aEscapeChar[destPos])
								{
									buffer2[i] = this.__buffer[destPos++];
									bFound = true;
									break;
								}
								destPos++;
							}
								
							if (!bFound)
							{
								buffer2[i] = this.__sFormat.charAt(i);
							}
						}
						else
						{
							buffer2[i] = this.__sFormat.charAt(i);
						}
					}
				}
				
				if (this.__matchMaskRule(buffer2))
				{
					this.__buffer = buffer2;
				}
				else
				{
					this.__buffer[currPos - 1] = '_'
				}
				
				this.__writeBuffer(currPos - 1);

				break;
			}
			else
			{
				this.__setCursorPos(--currPos);
			}
		}
	}
	
	/**
	 * Check whether the value follow the mask rule.
	 * @param {Object} value
	 */
	this.__matchMaskRule = function (value) {
	    var isMatch = true;
	    var ch, c, s;
	    var regExpChar;

	    for (var i = 0; i < value.length; i++) {
	        ch = value[i];
	        c = this.__sMask.charAt(i);
	        s = this.__sFormat.charAt(i);

	        //convert the letter associate with the mask format automatically.
	        if (c == 'U') {
	            ch = ch.toUpperCase();
	            value[i] = ch;
	        }
	        else if (c == 'L') {
	            ch = ch.toLowerCase();
	            value[i] = ch;
	        }

	        if (s == '_') {
	            if (ch == '_') {
	                continue;
	            }

	            regExpChar = new RegExp(this.__charMap[c]);
	            if (!ch.match(regExpChar)) {
	                isMatch = false;
	                break;
	            }
	        }
	        else {
	            if (ch != c) {
	                isMatch = false;
	                break;
	            }
	        }
	    }

	    return isMatch;
	}
	
	this.__getEnteredCharacter = function(env)
	{
		var nKeyNum = env.keyCode || env.which;
		
		return String.fromCharCode(nKeyNum);
	}
	
	this.__writeBuffer = function(pos)
	{
		var sText = "";

		for(var i = 0 ; i < this.__buffer.length ;i++)
		{
			if (this.__aEscapeChar[i])
			{
				sText += this.__sMask.charAt(i);
			}
			else
			{
				sText += this.__buffer[i];
			}
		}

		this.__target.value = sText;
		this.__setCursorPos(pos);
	}

	this.__formatBufferOnLostFocus = function () {
	    var sText = "";

	    for (var i = 0; i < this.__buffer.length; i++) {
	        var c = this.__buffer[i];
	        var m = this.__sMask.charAt(i);

	        if (c == '_' && m != '_') {
	            break;
	        }

	        sText += c;
	    }

	    if (this.__sMask.indexOf(sText) == 0) {
	        var pos = 0;

	        for (var j = 0; j < sText.length; j++) {
	            var f = this.__sFormat.charAt(j);
	            if (f == '_') {
	                pos++;
	            }
	        }

	        sText = sText.substr(0, pos);

	    }

	    this.__target.value = sText;
	}
	
	this.__registerEventForHTMLForm = function(obj)
	{
		var parentObj = obj.parentElement;

		while(parentObj != null || parentObj != undefined)
		{
			if (parentObj.tagName == 'FORM')
			{
				parentObj.onreset = new Function("resetMaskForHTMLForm(this)");
				break;
			}
			
			parentObj = parentObj.parentElement;
		}
	}

	this.attach = function (target) {
	    if (this.__attached) {
	        //alert("The AV360Mask object has already been attached by another element.")
	        alert(getText.masked_js_AV360Mask_attach);
	        return;
	    }
	    if (target == null || target == undefined) {
	        return;
	    }

	    this.__target = target;
	    this.__initialize();

	    this.__target["onfocus"] = new Function("event", "return " + this.__ref + ".__focus(event)");
	    this.__target["onkeydown"] = new Function("event", "return " + this.__ref + ".__keydown(event)");
	    this.__target["onkeypress"] = new Function("event", "return " + this.__ref + ".__keypress(event)");
	    this.__target["onblur"] = new Function("event", "return " + this.__ref + ".__blur(event)");
	    this.__target["onpaste"] = new Function("event", "return false");
	    this.__target["oncut"] = new Function("event", "return " + this.__ref + ".__oncut(event)");
	    this.__target["onclick"] = new Function("event", "return " + this.__ref + ".__onclick(event)");
	    this.__target["onresetmask"] = new Function("event", "return " + this.__ref + ".__onresetmask()");
	    this.__registerEventForHTMLForm(this.__target);
	    this.__attached = true;
	}
}

function resetMaskForHTMLForm(form)
{
    if (form.tagName != "FORM") {
        return;
    }

    var elements = form.document.getElementsByTagName("INPUT");
	
	for(var i = 0 ; i < elements.length ; i++ )		
	{
		if (elements[i]['onresetmask'])
		{
			elements[i].onresetmask();
		}
	}
}