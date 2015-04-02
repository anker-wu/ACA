/*
 * <pre>
 *  Accela Citizen Access
 *  File: handlevalue.js
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

// Tests if a string ends with the given string
function crossbrowser_endsWith(string, endsWithString)
{
	return (string.substr(string.length - endsWithString.length) == endsWithString);
}

// Removes a substring from a string
function crossbrowser_removeSubstring(string, substring)
{
    // If the strings are not empty
    if(string && substring)
    {
        var substringStart = string.indexOf(substring);

        // If the substring is found in the string
        if(substring && substringStart != -1)
        {
            return string.substring(0, substringStart) + string.substring(substringStart + substring.length, string.length);
        }

        return string;
    }

    return "";
}

// Trims leading and trailing spaces from a string
function crossbrowser_trim(string)
{
    return string.replace(new RegExp("^\\s+|\\s+$", "gi"), "");
}