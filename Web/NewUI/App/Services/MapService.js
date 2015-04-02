/**
* <pre>
* 
*  Accela Citizen Access
*  File: MapService.js
* 
*  Accela, Inc.
*  Copyright (C): 2014
* 
*  Description:
* 
*  Notes:
* $Id: MapService.js 72643 2008-04-24 09:52:06Z $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
*  07/10/2014     		Reid.Wang				Initial.  
* </pre>
*/
app.factory("MapService", function ($window, $http, $q, GlobalSearchParameterService) {
    var mapService = {};
    mapService.loaded = false;
    mapService.mapInstance = null;
    mapService.loadedfun = null;
    mapService.loadedfunp = null;
    mapService.loadedmap = null;

    mapService.parameter = function(key) {
        var dataSource = GlobalSearchParameterService.value[key];
        return dataSource;
    };

    mapService.Loader = function (fromOtherPage) {
        if (!mapService.loaded) {
            require(['AGIS'], function (map) {
                var initParam = {
                    "agencyCode": aca.data.agency
                };

                /*
                map.addEventListener("AGISMapLoaded", function (event) {
                    mapService.mapInstance = event.eventData;  //Map
                    if (mapService.loadedfun != null) {
                        mapService.loadedfun(event.eventData);
                        mapService.loadedfun = null;
                    }
                });
                */

                map.initMap("WorldMap", initParam);
                mapService.loadedmap = map;
                mapService.mapInstance = map;

                if (mapService.tempLocateInfo && mapService.tempLocateInfo != "") {
                    map.Locate(mapService.tempLocateInfo);
                    mapService.tempLocateInfo = null;
                }

                if (mapService.loadedfun != null) {
                    mapService.loadedfun(map);
                    mapService.loadedfun = null;
                }

                if (fromOtherPage) {
                    var locateData = mapService.parameter("locateData");

                    if (locateData) {
                        var locatelinfoJson = JSON.parse(locateData);
                        map.Locate(locatelinfoJson);
                    }
                }
            });
        }
    };

    mapService.locate = function (locateInformation) {
        if (typeof (locateInformation) == "string") {
            locateInformation = JSON.parse(locateInformation);
        }

        if (mapService.mapInstance != null && mapService.mapInstance != undefined) {
            mapService.mapInstance.Locate(locateInformation, mapService.mapInstance.Map);
        } else {
            mapService.tempLocateInfo = locateInformation;
        }
    };

    mapService.enlarge = function (enlargeInfo) {
        if (typeof (enlargeInfo) == "string") {
            enlargeInfo = JSON.parse(enlargeInfo);
        }

        if (mapService.mapInstance != null && mapService.mapInstance != undefined) {
            mapService.mapInstance.enlargeIcon(enlargeInfo, mapService.mapInstance.Map);
        }
    };

    return mapService;
});