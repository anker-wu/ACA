/**
* <pre>
* 
*  Accela Citizen Access
*  File: LabelKeyService.js
* 
*  Accela, Inc.
*  Copyright (C): 2014
* 
*  Description:
* 
*  Notes:
* $Id: LabelKeyService.js 72643 2014-08-27 09:52:06Z $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
*  07/10/2014     		Reid.Wang				Initial.  
* </pre>
*/
app.factory("LabelKeyService", function ($window, $http) {
    var labelKeyService = {};
    labelKeyService.DataKeys = "";
    labelKeyService.routeName = {
        "Home": "Home",
        "LaunchPad": "LaunchPad",
        "Dashboard": "Dashboard",
        "Login": "Login",
        "MapView": "MapView",
        "SearchView": "SearchView"
    };

    labelKeyService.getLabelKey = function (route, cultureLanguage) {
        var keys = dataKeyJson;
        var keysValue = sessionStorage.getItem(route);

        if (keysValue) {
            labelKeyService.DataKeys = JSON.parse(keysValue);
            return true;
        }

        keys = labelKeyService.getKeysByRoute(route, keys);

        if (keys==undefined) {
            return true;
        }

        var labelKey = {
            "Route": route,
            "Keys": keys,
            "CultureName": cultureLanguage
        };
        var isSuccess = false;

        $.ajax({
            type: 'POST',
            url: servicePath + "api/LabelKey/Label-Key",
            async: false,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(labelKey),
            success: function (response) {
                var type = response.routeType;
                sessionStorage.setItem(type, JSON.stringify(response));
                labelKeyService.DataKeys = response;
                isSuccess= true;
            },
            error:function(response)
            {
                isSuccess= false;
            }
        });

        return isSuccess;
    };

    labelKeyService.getKeysByRoute = function (route,keys) {
        switch (route) {
            case labelKeyService.routeName.Home:
                keys = keys.Home;
                break;
            case labelKeyService.routeName.LaunchPad:
                keys = keys.LaunchPad;
                break;
            case labelKeyService.routeName.Dashboard:
                keys = keys.Dashboard;
                break;
            case labelKeyService.routeName.MapView:
                keys = keys.MapView;
                break;
            case labelKeyService.routeName.Login:
                keys = keys.Login;
                break;
            case labelKeyService.routeName.SearchView:
                keys = keys.SearchView;
                break;
            default:;
        }

        return keys;
    };

    return labelKeyService;
});