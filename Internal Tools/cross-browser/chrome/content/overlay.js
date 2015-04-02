/*
 * <pre>
 *  Accela Citizen Access
 *  File: overlay.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: jquery.js 77905 2011-06-22 17:49:28Z ACHIEVO\daniel.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
*/

var HTMLHEADER = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\">";
var HTMLHEADER_NONEDOCUMENT = "<html xmlns=\"http://www.w3.org/1999/xhtml\">";
var http_Header = "";
var ALL_BROWSER = ["IE7","IE8","IE9","Safari","Chrome"];
var requestHeader = null;
var CROSSBROWSER_LOCALSERVICE = "http://localhost:8081/crossweb/";

var crossBrowser = {
	onLoad: function () {
		window.removeEventListener ("load", crossBrowser.onLoad, false);
		crossBrowser.init ();
	},

	init: function () {
		Cu.import ('resource://cross-browser/crossbrowsercore.jsm');
		CrossBrowserCore.loadList(false);
		change_options();
		
		var toolbarbuttons = document.getElementById("cross-browser-toolbar");
		for(var i=0;i<toolbarbuttons.childNodes.length;i++){
			var toolbarbuttonitem = toolbarbuttons.childNodes[i];
			toolbarbuttonitem.setAttribute ('image',GetTheBrowserICON(toolbarbuttonitem.getAttribute("crossbrowser-tag")));
		}
	}
}

window.addEventListener ("load", crossBrowser.onLoad, false);

/*
do click the browser button
*/
function DoCommand(type){
	var fileName = GetHTMLFile();
	var item = GetCommand(type);
	var requestURL = requestHeader + fileName;
	RunCommand(item,requestURL);
}

/*
open the all browser
*/
function DoAllCommand(){
	var fileName = GetHTMLFile();
	var requestURL = requestHeader + fileName;
	for(var i=0;i<CrossBrowserCore.list.length;i++){
		var item = CrossBrowserCore.list[i];
		if( item.name != "Internet Explorer"
			&& item.name != "IETester_IE9"
			&& item.name != "Mozilla Firefox"){
			RunCommand(item,requestURL);
		}
	}
}

/*
set the params ,run the browser;
*/
function RunCommand(item,requestURL){
	var command = item.command;
	var params = [];
	for(var i=0;i<item.params.length;i++){
		params.push(item.params[i]);
	}
	params.push(requestURL);
	
	RunBrowser(command,params);
}
/*
get the browser's params
*/
function GetCommand(type){
	for(var i=0;i<CrossBrowserCore.list.length;i++){
		if(CrossBrowserCore.list[i].name.indexOf(type) >= 0){
			return CrossBrowserCore.list[i];
		}
	}
	
	return { auto: true,
			keyName: "",
			name: "",
			command: "",
			params: "",
			icon: ""};
}

/*
find browser icon
*/
function GetTheBrowserICON(type){
	if(type == "ALL"){
		return "chrome://cross-browser/skin/openwith16.png";
	} else if(type == "options") {
		return "chrome://cross-browser/skin/options.png";
	} else {
		var item = GetCommand(type);
		return item.icon;
	}
}

/*
run the browser
*/
function RunBrowser(browser,params){
	var file = Cc["@mozilla.org/file/local;1"].createInstance (Ci.nsILocalFile);
	file.initWithPath (browser);
	
	if (!file.exists ()) {
		throw "File not found";
	}
	
	var fileToRun;
	if (/\.app$/.test (file.path)) {
		fileToRun = Cc ["@mozilla.org/file/local;1"].createInstance (Ci.nsILocalFile);
		fileToRun.initWithPath ("/usr/bin/open");
		var oldParams = params;
		params = ["-a", file.path];
		for (var i = 0, iCount = oldParams.length; i < iCount; i++) {
			params.push (oldParams [i]);
		}
	} else {
		fileToRun = file;
	}

	//Services.console.logStringMessage ('OpenWith: opening\n\tCommand: ' + fileToRun.path + '\n\tParams: ' + params.join (' '));
	var process = Cc ["@mozilla.org/process/util;1"].createInstance (Ci.nsIProcess);
	process.init (fileToRun);
	if ('runw' in process) {
		process.runw (false, params, params.length);
	} else {
		process.run (false, params, params.length);
	}
}


function GetHTMLFile(){
	//var frameDocument = window.top.getBrowser().selectedBrowser;
	//alert(window.content.document.body);
	//alert(window.content.document.getElementsByTagName("head")[0].innerHTML);
	//alert(window.content.document.location.host);
	//alert(window.content.document.location.href);
	//alert(window.content.document.domain);
	//BrowserViewSourceOfDocument(frameDocument);
	
	//savefileto_save_page();
	//alert(MTB_File.open);
	
	//var entryController = new EntryController();
	//alert(entryController);
	//MTB_File("c:\\daniel.txt");
	var docmentClone = window.content.document;
	var httpVar = docmentClone.location.href.split(":");
	if(httpVar.length >0){
		http_Header = httpVar[0];
	} else {
		http_Header = "http";
	}
	var htmlHeader_Actual = getActualHtmlHeader(docmentClone.compatMode);
	
	var csslink = docmentClone.getElementsByTagName("link");

	if(csslink != null){
		for(var i=0;i<csslink.length;i++){
			//alert(csslink[i].href);
			csslink[i].href = csslink[i].href;
		}
	}
	var jslink = docmentClone.getElementsByTagName("script");

	if(jslink != null){
		for(var i=0;i<jslink.length;i++){
			if(jslink[i].src != null && jslink[i].src.length != 0){
				if(jslink[i].src == http_Header + "://www.google.com/recaptcha/api/challenge?k=6LeMLL8SAAAAAIYgHs0VrUWMqrtozUXmq7LnvvJQ"
					|| jslink[i].src == http_Header + "://www.google.com/recaptcha/api/js/recaptcha.js"){
					jslink[i].src = "";
				} else {
					jslink[i].src = jslink[i].src;
				}
			}
		}
	}
	var iframes = docmentClone.getElementsByTagName("iframe");
	var fileNameReplaceFramesrc = [];
	for(var j=0;j<iframes.length;j++){
		var iframe = iframes[j];
		if(iframe.src.length !=0){
			var iframe_Document = iframe.contentWindow.document;
			var iframe_FileName = documentIFramesSave(iframe_Document);
			fileNameReplaceFramesrc.push({ src:ReplaceURLSpecifalChar(replaceHandler(docmentClone,iframe.src)),filename:iframe_FileName});
		}
	}
	
	var header = outerHtml(docmentClone,docmentClone.getElementsByTagName("head")[0]);
	var body = outerHtml(docmentClone,docmentClone.body);
	var bodyHtml = pathConvert(docmentClone,body);
	for(var x=0;x<fileNameReplaceFramesrc.length;x++){
		var pathexp = new RegExp(fileNameReplaceFramesrc[x].src.replace("?","\\?"),"gm");
		bodyHtml = bodyHtml.replace(pathexp,fileNameReplaceFramesrc[x].filename);
	}
	bodyHtml = bodyHtml.replace("Recaptcha.widget = Recaptcha.$(\"recaptcha_widget_div\"); Recaptcha.challenge_callback();","//Recaptcha.widget = Recaptcha.$(\"recaptcha_widget_div\"); Recaptcha.challenge_callback();").replace("__Tab.preRender();","//__Tab.preRender();");
	bodyHtml = bodyHtml.replace("new SectionHeaderManager('ctl00_PlaceHolderMain_shInspection').toggle();LoadInitInspectionList();","");
	var htmlFileString = htmlHeader_Actual + header + "\r\n" + bodyHtml + "</html>";
	var fileName = getTheFileName(docmentClone.location.href)+".html";
	var mtb_File = new MTB_File(fileName);
	mtb_File.open("w+");
	mtb_File.write(htmlFileString);
	mtb_File.close();
	
	return fileName;
}
/*
case the docCompatMode to return the html header
*/
function getActualHtmlHeader(docCompatMode){
	if(docCompatMode == "BackCompat"){
		return HTMLHEADER_NONEDOCUMENT;
	} else {
		return HTMLHEADER;
	}
}
/*
get the element's outter HTML
doc: document
elem: html element
*/
function outerHtml(doc,elem){
	if(typeof elem === 'string') elem = doc.getElementById(elem);
	var div = doc.createElement('div');
	div.appendChild(elem.cloneNode(true));
	var res = div.innerHTML;
	return res;
}

/*
convert the path to the absolute path.
doc: the document
Html: Html path
*/
function pathConvert(doc,Html){
	var prefix = doc.location.href.split(":");
	var preLink = prefix[0] + "://" + doc.location.host;
	var reg = /[\w|:|/|\.]*[/]{1}([\w-\.]+[/])*[\w-]+[\.]{1}([\w-./?%&=-]*)?/g;
	//var reg = /[abc]/g
	var result = Html.replace(reg, function(){ return replaceHandler(doc,arguments[0]); });
	return result;
	//http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)? https://a.b.c/b/a.aspx?a=1&d=2 "www.regexlab.com/zh/workshop.asp?pat=c&txt=abcde
	//([\.][\.][/])+([\w]+[/])+([\w-./?%&=]*)
}

/*
get the special file name
location: the full url
*/
function getTheFileName(location){
	var locations = location.split("/");
	if(locations.length == 0){
		return newGuid();
	}
	else{
		var filename = locations[locations.length-1].split("?");
		return filename[0]+newGuid();
	}
}
/* 
get the guid string;
*/
function newGuid(){
    var guid = "";
    for (var i = 1; i <= 32; i++){
      var n = Math.floor(Math.random()*16.0).toString(16);
      guid +=   n;
      if((i==8)||(i==12)||(i==16)||(i==20))
        guid += "-";
    }
    return guid;    
}

/* 
recursion the document's iframe html save  
doc: document
*/
function documentIFramesSave(doc){
	//inner iframes
	var iframes = doc.getElementsByTagName("iframe");
	var fileNameReplaceFramesrc = [];
	for(var j=0;j<iframes.length;j++){
		var iframe = iframes[j];
		if(iframe.src.length !=0){
			var iframe_Document = iframe.contentWindow.document;
			var iframe_FileName = documentIFramesSave(iframe_Document);
			//iframe.src = iframe.src;
			fileNameReplaceFramesrc.push({ src:ReplaceURLSpecifalChar(replaceHandler(doc,iframe.src)),filename:iframe_FileName});
		}
	}
	
	var csslink = doc.getElementsByTagName("link");
	if(csslink != null){
		for(var i=0;i<csslink.length;i++){
			//alert(csslink[i].href);
			csslink[i].href = csslink[i].href;
		}
	}
	var jslink = doc.getElementsByTagName("script");
	if(jslink != null)
	{
		for(var i=0;i<jslink.length;i++)
		{
			if(jslink[i].src != null && jslink[i].src.length != 0){
				if(jslink[i].src == http_Header + "://www.google.com/recaptcha/api/challenge?k=6LeMLL8SAAAAAIYgHs0VrUWMqrtozUXmq7LnvvJQ"
					|| jslink[i].src == http_Header + "://www.google.com/recaptcha/api/js/recaptcha.js"){
					jslink[i].src = "";
				} 
				else {
					jslink[i].src = jslink[i].src;
				}
			}
		}
	}

	var htmlHeader_Actual = getActualHtmlHeader(doc.compatMode);
	var header = outerHtml(doc,doc.getElementsByTagName("head")[0]);
	var body = outerHtml(doc,doc.body);
	var bodyHtml = pathConvert(doc,body);
	for(var x=0;x<fileNameReplaceFramesrc.length;x++){
		var pathexp = new RegExp(fileNameReplaceFramesrc[x].src.replace("?","\\?"),"gm");
		bodyHtml = bodyHtml.replace(pathexp,fileNameReplaceFramesrc[x].filename);
	}
	//new SectionHeaderManager('ctl00_PlaceHolderMain_shInspection').toggle();LoadInitInspectionList();
	bodyHtml = bodyHtml.replace("Recaptcha.widget = Recaptcha.$(\"recaptcha_widget_div\"); Recaptcha.challenge_callback();","//Recaptcha.widget = Recaptcha.$(\"recaptcha_widget_div\"); Recaptcha.challenge_callback();").replace("__Tab.preRender();","//__Tab.preRender();");
	bodyHtml = bodyHtml.replace("new SectionHeaderManager('ctl00_PlaceHolderMain_shInspection').toggle();LoadInitInspectionList();","");
	var htmlFileString = htmlHeader_Actual + header + "\r\n" + bodyHtml + "</html>";
	var filename = getTheFileName(doc.location.href)+".html";
	var mtb_File = new MTB_File(filename);
	mtb_File.open("w+");
	mtb_File.write(htmlFileString);
	mtb_File.close();
	return filename;
}

/* 
replace handler 
doc: document
replaceString: need replace url
*/
function replaceHandler(doc,replaceString){
	var replaceresult = "";
	var documenthrefarray = doc.location.href.split("/");
	var replaceStringArray = replaceString.split("/");
	
	if(replaceString.indexOf("http") == 0 || replaceString.indexOf("//") == 0){
		replaceresult = replaceString;
	}
	else if(replaceString.indexOf("..") == 0){
		var finds = 0;
		var endURL = "";
		for(var i=0;i<replaceStringArray.length;i++){
			if(replaceStringArray[i] == ".."){
				finds++;
			}
			else{
				if(i == replaceStringArray.length -1){
					endURL += replaceStringArray[i];
				}
				else{
					endURL += replaceStringArray[i] +"/";
				}
			}
		}
		
		if(finds>0){
			for(var j=0;j<documenthrefarray.length-finds-1;j++){
				replaceresult += documenthrefarray[j]+"/";
			}
			
			replaceresult += endURL;
		}
	}
	else if(replaceString.indexOf(".") == 0){
		for(var j=0;j<documenthrefarray.length-1;j++){
				replaceresult += documenthrefarray[j]+"/";
		}
		replaceresult += replaceString.substr(2);
	}
	else if(replaceString.indexOf("/") == 0){
		var prefix = doc.location.href.split(":");
		var preLink = prefix[0] + "://" + doc.location.host;
		replaceresult = preLink + replaceString;
	}
	else {
		for(var j=0;j<documenthrefarray.length-1;j++){
				replaceresult += documenthrefarray[j]+"/";
		}
		replaceresult += replaceString;
	}

	return replaceresult;
}
/*
replace the html string
& to &amp;
| to %7C
*/
function ReplaceURLSpecifalChar(input){
	return input.replace(/&/g,"&amp;").replace(/\|/g,"%7C");
}

// Displays the options dialog
function open_options(openPage)
{
    // If an open page is set
    if(openPage)
    {
        window.openDialog("chrome://cross-browser/content/options/options.xul", "cross-browser-options-dialog", "centerscreen,chrome,modal,resizable", openPage);
    }
    else
    {
        window.openDialog("chrome://cross-browser/content/options/options.xul", "cross-browser-options-dialog", "centerscreen,chrome,modal,resizable");
    }

    change_options();
}

function change_options()
{
	requestHeader = crossbrowser_getStringPreference("crossbrowser.edit.update.service", true);
	if(requestHeader){
		if(!crossbrowser_endsWith(requestHeader,"/")){
			requestHeader+="/";
		}
	} else {
		requestHeader = CROSSBROWSER_LOCALSERVICE;
	}	
}