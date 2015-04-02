/**
 * <pre>
 * 
 *  Accela
 *  File: FileUploadProgress.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: FileUploadProgress.js 79503 2007-11-08 07:04:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/14/2007     		lytton.cheng				Initial.
 * </pre>
 */

function NeatUploadGetMainWindow() 
{
	var mainWindow;
	if (window.opener) {
	    mainWindow = window.opener;
	} else {
	    mainWindow = window.parent;
	}
    return mainWindow;
};

NeatUploadCancelled = false;

function NeatUploadCancel() 
{
	NeatUploadCancelled = true;
	var mainWindow = NeatUploadGetMainWindow();
	if (mainWindow && mainWindow.stop) {
	    mainWindow.stop();
	} else if (mainWindow && mainWindow.document && mainWindow.document.execCommand) {
	    mainWindow.document.execCommand('Stop');
	}
}

function NeatUpload_CombineHandlers(origHandler, newHandler) 
{
    if (!origHandler || typeof (origHandler) == 'undefined') {
        return newHandler;
    }
    return function(e) { origHandler(e); newHandler(e); };
}

NeatUploadReq = null;
function NeatUploadRefreshWithAjax(url) 
{
	NeatUploadReq = null;
	var req = null;
	try
	{
		req = new ActiveXObject('Microsoft.XMLHTTP');
	}
	catch (ex)
	{
		req = null;
	}
	if (!req && typeof(XMLHttpRequest) != 'undefined')
	{
		req = new XMLHttpRequest();
	}
	if (req)
	{
		NeatUploadReq = req;
	}
	if (NeatUploadReq)
	{
		NeatUploadReq.onreadystatechange = NeatUploadUpdateHtml;
		NeatUploadReq.open('GET', url);
		NeatUploadReq.send(null);
	}
	else
	{
		return false;
	}
	return true;
}

function NeatUploadUpdateHtml()
{
	if (typeof(NeatUploadReq) != 'undefined' && NeatUploadReq != null && NeatUploadReq.readyState == 4) 
	{
		try
		{
			var responseXmlDoc = NeatUploadReq.responseXML;
			if (responseXmlDoc.parseError && responseXmlDoc.parseError.errorCode != 0)
			{
//				window.alert('parse error: ' + responseXmlDoc.parseError.reason);
			}
//			window.alert(new XMLSerializer().serializeToString(responseXmlDoc));
			var templates = responseXmlDoc.getElementsByTagName('neatUploadDetails');
			var status = templates.item(0).getAttribute('status');
			for (var t = 0; t < templates.length; t++)
			{
				var srcElem = templates.item(t);
				var innerXml = '';
				for (var i = 0; i < srcElem.childNodes.length; i++)
				{
					var childNode = srcElem.childNodes.item(i);
					var xml = childNode.xml;
					if (xml == null) {
					    xml = new XMLSerializer().serializeToString(childNode);
					}
				    if (typeof(xml) == 'undefined')
					{
						//throw "serializeToString() returned 'undefined' (probably due to a Safari bug) so no AJAX.";  
						throw getText.FileUploadProgress_js_NeatUploadUpdateHtml_error; 
				    }
					innerXml += xml;
				}
				var id = srcElem.getAttribute('id');
				var destElem = document.getElementById(id);
				destElem.innerHTML = innerXml;
				for (var a=0; a < srcElem.attributes.length; a++)
				{
					var attr = srcElem.attributes.item(a);
					if (attr.specified)
					{
					    if (attr.name == 'style' && destElem.style && destElem.style.cssText) {
					        destElem.style.cssText = attr.value;
					    } else {
					        destElem.setAttribute(attr.name, attr.value);
					    }
					}
				}
			}
			if (status != 'NormalInProgress' && status != 'ChunkedInProgress' && status != 'Unknown')
			{
				NeatUploadRefreshPage();
			}
			var lastMillis = NeatUploadLastUpdate.getTime();
			NeatUploadLastUpdate = new Date();
			var delay = Math.max(lastMillis + 1000 - NeatUploadLastUpdate.getTime(), 1);
			NeatUploadReloadTimeoutId = setTimeout('NeatUploadRefresh()', delay);
		}
		catch (ex)
		{
			NeatUploadRefreshPage();
		}
	}
}

NeatUploadLastUpdate = new Date(); 
NeatUploadReloadTimeoutId = null;

window.onunload = NeatUpload_CombineHandlers(window.onunload, function () {
    if (NeatUploadReq && NeatUploadReq.readystate
		&& NeatUploadReq.readystate >= 1 && NeatUploadReq.readystate <= 3) {
        NeatUploadReq.abort();
    }
    NeatUploadReq = null;
    if (NeatUploadReloadTimeoutId) {
        clearTimeout(NeatUploadReloadTimeoutId);
    }
});

NeatUploadMainWindow = NeatUploadGetMainWindow();

function NeatUploadRefresh()
{
	if (!NeatUploadRefreshWithAjax(NeatUploadRefreshUrl + '&useXml=true'))
	{
		NeatUploadRefreshPage();
	}
}

function NeatUploadRefreshPage() 
{
	if (!NeatUploadCancelled)
	{
		window.location.replace(NeatUploadRefreshUrl);
	}
}

function NeatUpload_CancelClicked()
{
    if(typeof(parent.ShowBlock) != 'undefined')
    {
        parent.ShowBlock(false);
    }
    NeatUploadCancel();
    if (typeof (parent.DisableParentContinueBtn) != 'undefined') {
        parent.DisableParentContinueBtn(false);
    }
	//window.parent.CallbackFunc('cancelled');
	if(typeof(NeatUploadRefreshUrl) != 'undefined')
	{
	    window.location.replace(NeatUploadRefreshUrl + '&cancelled=true');
	}
}
