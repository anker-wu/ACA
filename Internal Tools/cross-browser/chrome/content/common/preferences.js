/*
 * <pre>
 *  Accela Citizen Access
 *  File: preferences.js
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

var crossbrowser_preferencesService = null;

// Deletes a preference
function crossbrowser_deletePreference(preference)
{
    // If the preference is set
    if(preference)
    {
        // If a user preference is set
        if(crossbrowser_isPreferenceSet(preference))
        {
            crossbrowser_getPreferencesService().clearUserPref(preference);
        }
    }
}

// Deletes a preference branch
function crossbrowser_deletePreferenceBranch(branch)
{
    // If the branch is set
    if(branch)
    {
        crossbrowser_getPreferencesService().deleteBranch(branch);
    }
}

// Gets a boolean preference, returning false if the preference is not set
function crossbrowser_getBooleanPreference(preference, userPreference)
{
    // If the preference is set
    if(preference)
    {
        // If not a user preference or a user preference is set
        if(!userPreference || crossbrowser_isPreferenceSet(preference))
        {
            try
            {
                return crossbrowser_getPreferencesService().getBoolPref(preference);
            }
            catch(exception)
            {
                // Do nothing
            }
        }
    }

    return false;
}

// Gets an integer preference, returning 0 if the preference is not set
function crossbrowser_getIntegerPreference(preference, userPreference)
{
    // If the preference is set
    if(preference)
    {
        // If not a user preference or a user preference is set
        if(!userPreference || crossbrowser_isPreferenceSet(preference))
        {
            try
            {
                return crossbrowser_getPreferencesService().getIntPref(preference);
            }
            catch(exception)
            {
                // Do nothing
            }
        }
    }

    return 0;
}

// Gets the preferences service
function crossbrowser_getPreferencesService()
{
    // If the preferences service is not set
    if(!crossbrowser_preferencesService)
    {
        crossbrowser_preferencesService = Components.classes["@mozilla.org/preferences-service;1"].getService(Components.interfaces.nsIPrefService).getBranch("");
    }

    return crossbrowser_preferencesService;
}

// Gets a string preference, returning null if the preference is not set
function crossbrowser_getStringPreference(preference, userPreference)
{
    // If the preference is set
    if(preference)
    {
        // If not a user preference or a user preference is set
        if(!userPreference || crossbrowser_isPreferenceSet(preference))
        {
            try
            {
                return crossbrowser_trim(crossbrowser_getPreferencesService().getComplexValue(preference, Components.interfaces.nsISupportsString).data);
            }
            catch(exception)
            {
                // Do nothing
            }
        }
    }

    return null;
}

// Is a preference set
function crossbrowser_isPreferenceSet(preference)
{
    // If the preference is set
    if(preference)
    {
        return crossbrowser_getPreferencesService().prefHasUserValue(preference);
    }

    return false;
}

// Sets a boolean preference
function crossbrowser_setBooleanPreference(preference, value)
{
    // If the preference is set
    if(preference)
    {
        crossbrowser_getPreferencesService().setBoolPref(preference, value);
    }
}

// Sets a boolean preference if it is not already set
function crossbrowser_setBooleanPreferenceIfNotSet(preference, value)
{
    // If the preference is not set
    if(!crossbrowser_isPreferenceSet(preference))
    {
        crossbrowser_getPreferencesService().setBoolPref(preference, value);
    }
}

// Sets an integer preference
function crossbrowser_setIntegerPreference(preference, value)
{
    // If the preference is set
    if(preference)
    {
        crossbrowser_getPreferencesService().setIntPref(preference, value);
    }
}

// Sets an integer preference if it is not already set
function crossbrowser_setIntegerPreferenceIfNotSet(preference, value)
{
    // If the preference is not set
    if(!crossbrowser_isPreferenceSet(preference))
    {
        crossbrowser_setIntegerPreference(preference, value);
    }
}

// Sets a string preference
function crossbrowser_setStringPreference(preference, value)
{
    // If the preference is set
    if(preference)
    {
        var supportsStringInterface = Components.interfaces.nsISupportsString;
        var string                  = Components.classes["@mozilla.org/supports-string;1"].createInstance(supportsStringInterface);

        string.data = value;

        crossbrowser_getPreferencesService().setComplexValue(preference, supportsStringInterface, string);
    }
}

// Sets a string preference if it is not already set
function crossbrowser_setStringPreferenceIfNotSet(preference, value)
{
    // If the preference is not set
    if(!crossbrowser_isPreferenceSet(preference))
    {
        crossbrowser_setStringPreference(preference, value);
    }
}