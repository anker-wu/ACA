/*
* <pre>
*  Accela Citizen Access
*  File: extendFilter.js
* 
*  Accela, Inc.
*  Copyright (C): 2014
* 
*  Description:
* 
*  Notes:
*      $Id: extendFilter.js 77905 2014-09-01 12:49:28Z ACHIEVO\eric.he $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

app.filter('ellipsis', function () {
    return function (item, length) {
        if (item != null && item.length > length) {
            item = item.substring(0, length) + "...";
        }
        return item;
    };
});

app.filter('aleph', function () {
    return function (item) {
        if (!item) {
            return '';
        }
        return item.substr(0,1);
    };
});