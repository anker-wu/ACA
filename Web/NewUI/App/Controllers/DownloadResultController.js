/**
* <pre>
* 
*  Accela Citizen Access
*  File: DownloadResultController.js
* 
*  Accela, Inc.
*  Copyright (C): 2014
* 
*  Description:
* 
*  Notes:
* $Id:DownloadResultController.js 72643 2014-06-19 09:52:06Z $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
*  15/09/2014     		Awen				Initial.  
* </pre>
*/

function DownloadJsonResult(jsonList,showField, fileName) {

    if (jsonList == null || jsonList.length <= 0) return;
    showField == showField || new Array();
    var data = "";
    for (attr in jsonList[0]) {
        if (attr.indexOf('$$') < 0 && $.inArray(attr, showField) >= 0)
            data += "\""+attr + "\",";
    }
    if (data.length > 0) {
        data = data.substr(0, data.length - 1);
        data += "\r\n";
    }

    angular.forEach(jsonList, function (json, key) {

        for (attr in json) {
            if ((attr.indexOf('$$') < 0 && $.inArray(attr, showField) >= 0))
                data +="\"" +(json[attr]+"").replace("\"","\"\"") + "\",";
        }
        data = data.substr(0, data.length - 1);
        data += "\r\n";
    });

    if ((!window.Blob) == false && (!window.navigator.msSaveOrOpenBlob) == false) {

        var blobObject = new Blob([data], { type: "text/csv;charset=UTF-8" });
        window.navigator.msSaveBlob(blobObject, fileName + ".csv");
    } else {

        var dataBlob = new Blob([data], { type: "text/csv;charset=UTF-8" });
        var downloadUrl = window.webkitURL.createObjectURL(dataBlob);
        var anchor = document.createElement("a");
        anchor.href = downloadUrl;
        anchor.download = fileName + ".csv";
        anchor.click();
        window.URL.revokeObjectURL(data);
    }
}