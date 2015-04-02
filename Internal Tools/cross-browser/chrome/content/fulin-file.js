/*
 * <pre>
 *  Accela Citizen Access
 *  File: fulin-file.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: fulin-file.js 77905 2011-06-22 17:49:28Z ACHIEVO\daniel.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
*/
 
var MTB_RDONLY      = 0x01;
var MTB_WRONLY      = 0x02;
var MTB_RDWR        = 0x04;
var MTB_CREATE_FILE = 0x08;
var MTB_APPEND      = 0x10;
var MTB_TRUNCATE    = 0x20;
var PERM_IROTH      = 420;
var PERM_CONVERT     = 0666;

// Get local file directory.
function MTB_getLocalFileDir(){
	var dirService = Cc["@mozilla.org/file/directory_service;1"]
						.getService(Ci.nsIProperties);
	var localFile = dirService.get("ProfD", Ci.nsIFile);
	// append the file-string to our dir file-obj
	localFile.append("FulinToolbarData");
	if( !localFile.exists() )
		localFile.create(1, parseInt("0700", 8) );
	return localFile;
};

function MTB_File(filename){
	var localfile = MTB_getLocalFileDir();
	localfile.append(filename);
	this.nowFile = localfile;
};

MTB_File.prototype.nowFile   = null;

MTB_File.prototype.nowStream = null;

MTB_File.prototype.tmpStream = null;

MTB_File.prototype.open = function(mode){
	if( this.nowFile == null )
		return;
	if( this.nowStream != null )
		this.nowStream.close();
	
	switch(mode){
		case "r":
			var instream = Cc["@mozilla.org/network/file-input-stream;1"]
									.createInstance(Ci.nsIFileInputStream);
			instream.init(this.nowFile, MTB_RDONLY, PERM_IROTH, 0);
			this.tmpStream = instream;
			var sctinstream = Cc["@mozilla.org/scriptableinputstream;1"]
									.createInstance(Ci.nsIScriptableInputStream);
			sctinstream.init(this.tmpStream);
			this.nowStream = sctinstream;
			break;		
		case "w":
			var outstream = Cc["@mozilla.org/network/file-output-stream;1"]
									.createInstance(Ci.nsIFileOutputStream);
			outstream.init(this.nowFile, MTB_RDWR, PERM_CONVERT, 0);
			this.nowStream = outstream;
			break;
		case "w+":
			var outstream = Cc["@mozilla.org/network/file-output-stream;1"]
									.createInstance(Ci.nsIFileOutputStream);
			if( this.exists() )
				this.nowFile.remove( false );
			outstream.init(this.nowFile, MTB_WRONLY|MTB_CREATE_FILE|MTB_TRUNCATE, PERM_CONVERT, 0);
			this.nowStream = outstream;
			break;
	}
};

MTB_File.prototype.read = function(count){
	if( this.nowStream == null )
		return;
	return this.nowStream.read(count);
};

MTB_File.prototype.write = function(str){
	if( this.nowStream == null )
		return;
	var converter = Cc["@mozilla.org/intl/converter-output-stream;1"]
                     .createInstance(Ci.nsIConverterOutputStream);
	converter.init(this.nowStream, "UTF-8", 0, 0);
	converter.writeString(str);
	converter.close();
	//this.nowStream.write( str, str.length );
	//this.nowStream.flush();
};

MTB_File.prototype.close = function(){
	if(this.nowStream != null)
		this.nowStream.close();
	if(this.tmpStream != null)
		this.tmpStream.close();
	this.nowFile = null;
	this.nowStream = null;
	this.tmpStream = null;
};

MTB_File.prototype.exists = function(){
	if( this.nowFile )
		return this.nowFile.exists();
	else
		return;
};

MTB_File.prototype.isFile = function(){
	if( this.nowFile )
		return this.nowFile.isFile();
	else
		return;
};

MTB_File.prototype.isDir = function(){
	if( this.nowFile )
		return this.nowFile.isDirectory();
	else
		return;
};

MTB_File.prototype.isExe = function(){
	if( this.nowFile )
		return this.nowFile.isExecutable();
	else
		return;
};

MTB_File.prototype.EOF = function(){
	if( this.nowStream == null )
		return;
	if( this.nowStream.available() > 0 )
		return false;
	else
		return;
};

MTB_File.prototype.fileSize = function(){
	if( this.nowFile )
		return this.nowFile.fileSize;
};

function mtbFulinXml(){
	var fulinXml = [
	 "<SearchPlugin xmlns=\"http://www.mozilla.org/2006/browser/search/\">"
	,"\n<ShortName>Fulin</ShortName>"
	,"\n<Description>Fulin Search</Description>"
	,"\n<InputEncoding>UTF-8</InputEncoding>"
	,"\n<Image width=\"16\" height=\"16\">"
	,"data:image/x-icon;base64,"
	,"iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABGdBTUEAALGOfPtRkwAA"
	,"ACBjSFJNAAB6JQAAgIMAAPn/AACA6AAAdTAAAOpgAAA6lwAAF2+XqZnUAAABfElEQVR4"
	,"nGL8//8/AyUAIIBYQMTvFSv+s9y+jSoDM5iREUPT348fGX40NDDw8PAwAgQQE0jgx48f"
	,"CA0wzSCN6Jqhan79+sXw4sULMBsggCAu+PObgeH7d+xuBBmC5s0/v38xfPnyBcwGCCCw"
	,"Af9Bij5/xtSARTMI/PvzF84GCCBGUCA+ePDgPwsLC8Pz58/gJuMDbGzsDOLi4gwqKiqM"
	,"AAEEdgHvwQMMQqtWM0gT1AoB36SlGT43NYHZAAEE8QIDIwPju3f4df39CwptMPOnoCDc"
	,"pQABBDbgDxPQrxcuQBRCFeEDP1RU4AYABBDYAHBgEaER7phfP+FsgABiwabgXHoaw3Nf"
	,"XwZ2dg4GZmYmFDlGoGWcnJwMonx8YD5AAGE1ABQjEhIS4JAGhTi6ASB5QWA4gABAAEEN"
	,"QE1xTExMoGTKICMji5mO0QBAADHBNJALAAIIrJOVlZVsAwACiBGanf8/f/6c4cmTx+Do"
	,"4eTkYpCWlmKQlZUj6AWAAAMA4Xpxy1skKcIAAAAASUVORK5CYII="
	,"</Image>"
	,"\n<Url type=\"text/html\" method=\"GET\" template=\"http://www.google.cn/search\">"
	,"\n  <Param name=\"q\" value=\"{searchTerms}\"/>"
	,"\n  <Param name=\"hl\" value=\"zh-CN\"/>"
	,"\n  <Param name=\"meta\" value=\"\"/>"
	,"\n  <Param name=\"aq\" value=\"f\"/>"
	,"\n</Url>"
	,"\n<SearchForm>http://www.google.cn/</SearchForm>"
	,"\n</SearchPlugin>"
	];
	return fulinXml;
};

function mtbFulinSrc(){
	var fulinSrc = [
	 "<search "
	,"\n   name=\"Fulin\""
	,"\n   description=\"Fulin Web page search\""
	,"\n   method=\"GET\""
	,"\n   action=\"http://www.google.cn/search\""
	,"\n   queryEncoding=\"utf-8\""
	,"\n   queryCharset=\"utf-8\""
	,"\n>"
	,"\n<input name=\"q\" user>"
	,"\n<input name=\"hl\" value=\"zh-CN\">"
	,"\n<input name=\"meta\" value=\"\">"
	,"\n<input name=\"aq\" value=\"f\">"
	,"\n<interpret "
	,"\n    browserResultType=\"result\""
	,"\n    charset = \"UTF-8\""
	,"\n    resultListStart=\"<!--a-->\""
	,"\n    resultListEnd=\"<!--z-->\""
	,"\n    resultItemStart=\"<!--m-->\""
	,"\n    resultItemEnd=\"<!--n-->\""
	,"\n>"
	,"\n</search>"
	];
	return fulinSrc;
};

function mtbFulinPng(){
	var fulinPng = [
	 "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABGdBTUEAALGOfPtRkwAA"
	,"ACBjSFJNAAB6JQAAgIMAAPn/AACA6AAAdTAAAOpgAAA6lwAAF2+XqZnUAAABfElEQVR4"
	,"nGL8//8/AyUAIIBYQMTvFSv+s9y+jSoDM5iREUPT348fGX40NDDw8PAwAgQQE0jgx48f"
	,"CA0wzSCN6Jqhan79+sXw4sULMBsggCAu+PObgeH7d+xuBBmC5s0/v38xfPnyBcwGCCCw"
	,"Af9Bij5/xtSARTMI/PvzF84GCCBGUCA+ePDgPwsLC8Pz58/gJuMDbGzsDOLi4gwqKiqM"
	,"AAEEdgHvwQMMQqtWM0gT1AoB36SlGT43NYHZAAEE8QIDIwPju3f4df39CwptMPOnoCDc"
	,"pQABBDbgDxPQrxcuQBRCFeEDP1RU4AYABBDYAHBgEaER7phfP+FsgABiwabgXHoaw3Nf"
	,"XwZ2dg4GZmYmFDlGoGWcnJwMonx8YD5AAGE1ABQjEhIS4JAGhTi6ASB5QWA4gABAAEEN"
	,"QE1xTExMoGTKICMji5mO0QBAADHBNJALAAIIrJOVlZVsAwACiBGanf8/f/6c4cmTx+Do"
	,"4eTkYpCWlmKQlZUj6AWAAAMA4Xpxy1skKcIAAAAASUVORK5CYII="
	];
	return fulinPng;
};


var keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
    
function encode64(input) {
	var output = "";
	var chr1, chr2, chr3 = "";
	var enc1, enc2, enc3, enc4 = "";
	var i = 0;
	do { 
		chr1 = input.charCodeAt(i++);
		chr2 = input.charCodeAt(i++);
		chr3 = input.charCodeAt(i++);

		enc1 = chr1 >> 2;
		enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
		enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
		enc4 = chr3 & 63;
	
		if (isNaN(chr2)) {
		  enc3 = enc4 = 64;
		} else if (isNaN(chr3)) {
		  enc4 = 64;
		}

		output = output +   
				keyStr.charAt(enc1) +   
				keyStr.charAt(enc2) +   
				keyStr.charAt(enc3) +   
				keyStr.charAt(enc4);
		chr1 = chr2 = chr3 = "";
		enc1 = enc2 = enc3 = enc4 = "";
	} while (i < input.length);

	return output;
};

/*
convert input string to base64 string
input: string
*/
function decode64(input) { 
	var output = "";
	var chr1, chr2, chr3 = "";
	var enc1, enc2, enc3, enc4 = "";
	var i = 0;
	
	// remove all characters that are not A-Z, a-z, 0-9, +, /, or = 
	var base64test = /[^A-Za-z0-9\+\/\=]/g;
	input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");
	
	do {
		enc1 = keyStr.indexOf(input.charAt(i++));
		enc2 = keyStr.indexOf(input.charAt(i++));
		enc3 = keyStr.indexOf(input.charAt(i++));
		enc4 = keyStr.indexOf(input.charAt(i++));
		chr1 = (enc1 << 2) | (enc2 >> 4);
		chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
		chr3 = ((enc3 & 3) << 6) | enc4;
		output = output + String.fromCharCode(chr1);
		if (enc3 != 64)
			output = output + String.fromCharCode(chr2);
		if (enc4 != 64)
			output = output + String.fromCharCode(chr3);
		chr1 = chr2 = chr3 = "";
		enc1 = enc2 = enc3 = enc4 = "";
	} while (i < input.length);
	
	return output;
};

function mtbStringToUnicode(str){
	var unistr = "";
	for (var i= 0; i< str.length; i++ ){
		unistr += str.charCodeAt(i);
	}
	return unistr;
};

function mtbUnicodeToString(unistr){
	return String.fromCharCode(unistr);
};