/*
 * <pre>
 *  Accela Citizen Access
 *  File: options.js
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

var crossbrowser_optionDataString = new Array();
var crossbrowser_localService = "http://localhost:8081/crossweb/";

// Saves the user's options
function crossbrowser_saveOptions()
{
	crossbrowser_storeOptions();
	
	 // Loop through the string options
    for(option in crossbrowser_optionDataString)
    {
        optionValue = crossbrowser_optionDataString[option];

        // If the option value is set or the preference currently has a value
        if(optionValue || crossbrowser_isPreferenceSet(option))
        {
            crossbrowser_setStringPreference(option, optionValue);
        }
    }
}

// Initializes the options dialog
function crossbrowser_initializeOptions(checkDialogParameters)
{
	var pageList   = document.getElementById("cross-browser-options-page-list");
    var selectPage = 0;

    // If check dialog parameters and window arguments are set
	if(checkDialogParameters && window.arguments)
	{
        selectPage = crossbrowser_translatePageNameToIndex(window.arguments[0]);
    }
	else if(crossbrowser_isPreferenceSet("crossbrowser.options.last.page"))
	{
        selectPage = crossbrowser_getIntegerPreference("crossbrowser.options.last.page");
	}

    pageList.selectedIndex = selectPage;

    document.getElementById("cross-browser-options-iframe").setAttribute("src", pageList.selectedItem.value);
}

// Initializes the general page
function crossbrowser_initializeGeneral()
{
	var pageDocument = document.getElementById("cross-browser-options-iframe").contentDocument;

    // If the hide menu preference is set
	if(crossbrowser_isPreferenceSet("crossbrowser.edit.update.service"))
    {
        pageDocument.getElementById("cross-browser.edit.update.service").value = crossbrowser_getStringPreference("crossbrowser.edit.update.service", true);
    }
	else
	{
		pageDocument.getElementById("cross-browser.edit.update.service").value = crossbrowser_localService;
	}
}

function crossbrowser_changePage(pageList)
{
    crossbrowser_storeOptions();
    crossbrowser_setIntegerPreference("crossbrowser.options.last.page", pageList.selectedIndex);

    document.getElementById("crossbrowser-options-iframe").setAttribute("src", pageList.selectedItem.value);
}

// Stores the user's options to be saved later
function crossbrowser_storeOptions()
{
	var iFrame       = document.getElementById("cross-browser-options-iframe");
    var iFrameSrc    = iFrame.getAttribute("src");
	var pageDocument = iFrame.contentDocument;
	
	if(iFrameSrc.indexOf("general") != -1)
    {
		crossbrowser_optionDataString["crossbrowser.edit.update.service"] = pageDocument.getElementById("cross-browser.edit.update.service").value;
	}
}