/**
* <pre>
* 
*  Accela Citizen Access
*  File: MapViewController.js
* 
*  Accela, Inc.
*  Copyright (C): 2014
* 
*  Description:
* 
*  Notes:
* $Id: MapViewController.js 72643 2014-06-19 09:52:06Z $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
*  03/18/2008     		Kevin.Feng				Initial.  
* </pre>
*/
app.controller('MapViewCtrl', function ($scope, $http, $compile, $location, $routeParams, $window, $q, MapService, LabelKeyService, GlobalSearchParameterService) {
    $scope.route = LabelKeyService.routeName.MapView;
    $scope.ShowLanguage(false);

    //refresh DocumentList
    $scope.RefreshDocumentList = function () {
    };

    $scope.initMapView = function () {
        $scope.DefaultSideBarStyle(true);

        //get label key
        if (LabelKeyService.getLabelKey($scope.route, $scope.selectedLanguage)) {
            $scope.MapViewData = LabelKeyService.DataKeys;
        }

        $scope.IsShowCheckeRecord();
    };

    //Get document icon
    $scope.GetDocumentIcon = function (postfix) {
        if (postfix == "pdf") {
            return 'doc-icon pdf';

        } else if (postfix == "txt") {
            return 'doc-icon txt';

        } else if (postfix == "xls" || postfix == "xlsx") {
            return 'doc-icon excel';

        } else if (postfix == "pptx" || postfix == "ppt") {
            return 'doc-icon ppt';

        } else if (postfix == "doc" || postfix == "docx") {
            return 'doc-icon word';

        } else {
            return 'doc-icon unknown';
        }
    };

    //Map source The default GIS
    $scope.MapTypeSource = ["GIS", "GlobalSearch"];

    //current Map source
    $scope.CurrentMapTypeSource = $scope.MapTypeSource[1];

    //Gis ShowRecords Count
    $scope.GisRecordsCount = 0;

    //Gis Description
    $scope.GisDescription = "";

    // View on Map Count
    $scope.RecordsCount = 0;

    // View on Map Description
    $scope.RecordsDescription = "";

    //load map success event
    $scope.loadMapSucess = function (map) {
        aca.mapLoadSuccess = true;
        map.addEventListener("SendGISFeature", $scope.sendGisFeature);
        map.addEventListener("enlargeFeatureID", $scope.enlargeFeatureID);
        $("#map-view,#WorldMap").css("position", "relative");
        aca.mapLoadSuccess = true;
        $scope.DefaultSideBarStyle(false);
    };

    //Side Default Style
    $scope.DefaultSideBarStyle = function (state) {
        if ($scope.IsMapView()) return;
        if (!state) {
            $('#SearchListInMap-SideBar').css("top", "10px");
            $('#SearchListInMap').css("top", "10px");
        } else {
            $('#SearchListInMap-SideBar').css("top", "40px");
            $('#SearchListInMap').css("top", "40px");
        }
    };

    //loading map
    $scope.loadMap = function () {
        aca.mapLoadSuccess = false;
        var route = "#" + $window.location.href.split("#")[1];
        if (iframeMinHeight == undefined) setContentInterlayer();
        var searchMapHeight = iframeMinHeight - 55;
        var mapHeight = iframeMinHeight;

        if (route == "#/SearchView") {
            if (searchMapHeight > 400) {
                $("#WorldMap").css("min-height", searchMapHeight + "px");
            } else {
                $("#WorldMap").css("min-height", "400px");
            }
        }
        else {
            if (mapHeight > 400) {
                $("#WorldMap").css("min-height", mapHeight + "px");
            } else {
                $("#WorldMap").css("min-height", "400px");
            }
        }

        if ($scope.isGetGisApi()) {
            $scope.loadmapForMap();
        } else {
            $.cachedScript(loadMapApi()).done(function (script, textStatus) {
                $scope.loadmapForMap();
            });
        }
    };

    //whether the api have load
    $scope.isGetGisApi = function () {
        var esriJs = "jsapi.js";
        var isLoadMapjs = false;

        $("script").each(function (i, item) {
            if (item.src.indexOf(esriJs) > -1) {
                isLoadMapjs = true;
            }
        });

        return isLoadMapjs;
    };

    //load map by gisapi
    $scope.loadmapForMap = function () {
        if (MapService.loadedmap) {
            $scope.loadMapSucess(MapService.loadedmap);
        } else {
            MapService.loadedfun = $scope.loadMapSucess;
        }

        if (typeof ($routeParams.isFromDashboard) != "undefined") {
            MapService.Loader($routeParams.isFromDashboard);
            aca.ui.showSlidePanel();
        } else {
            MapService.Loader("");
            aca.ui.hideSlidePanel();
        }
    };

    //register  sendGisFeature event
    $scope.sendGisFeature = function (event) {
        var data = JSON.stringify(event.eventData);
        aca.data.mapJsonData = data;
        var command = event.eventData.command;

        if (command == aca.data.commond.creat) {
            var message = $scope.getGisMessage(data);

            if (message=="") {
                $window.location.href = window.location.href.split("#")[0] + "#/?IsFromMap=true";
            } else {
                ShowModalWindow($scope.HomeData.aca_newui_home_label_notice, message);
            }
        }

        if (command == aca.data.commond.show) {
            $scope.CurrentMapTypeSource = $scope.MapTypeSource[0];
            $scope.actionByData(data);
        }

        if (command == aca.data.commond.request) {
            $window.location.href = window.location.href.split("#")[0] + "#/?IsFromMap=true";
        }

        if (command == aca.data.commond.viewPropertyInfo) {
            $scope.CurrentMapTypeSource = $scope.MapTypeSource[0];
            $scope.actionByData(data);
        }
    };

    //enlargeFeatureID for map
    $scope.enlargeFeatureID = function (event) {
        var data = event.typeID;
        var dom;
        var miniView;

        $('.miniView').each(function (index, item) {
            if ($(item).css("display") != "none") {
                miniView = $(item);
                dom = miniView.find('.SearchList-Item-Content span:contains("' + data + '")');
                return false;
            }
        });

        if (data == null) {
            if (miniView) {
                dom = miniView.find('.SearchList-Item-Content .res-content.selected .rec-action');
                var actionBtnStyle = dom.css("display");

                if (actionBtnStyle == "block" || actionBtnStyle == "inline") {
                    var clickTarget = dom.prev().prev();
                    clickTarget.click();
                }
            }

            return;
        }

        if (dom.length > 0) {
            var scrollTo = dom.parent().parent().parent().parent();
            var scrollHeight = 0;
            var prevAll = scrollTo.prevAll();
            var modules = scrollTo.parent().prevAll();

            if (prevAll.length > 0) {
                $.each(prevAll, function (i, item) {
                    scrollHeight += $(item).height() + 10;
                });
            }

            if (modules.length > 0) {
                $.each(modules, function (i, item) {
                    scrollHeight += $(item).height() + 10;
                });
            }

            $('.SearchList-Item-Content').slimScroll({ scrollTo: scrollHeight + 'px' });
            $('.SearchList-Item-Content').css("width", "100%");

            var actionBtnDisplay = scrollTo.find('.actionbutton').css("display");

            if (actionBtnDisplay == "block" || actionBtnDisplay == "inline") {
                return;
            }

            scrollTo.attr("data-Target", "true");

            var clickTalget = dom.parent().parent().parent();
            clickTalget.click();
            return;
        }
    };

    //wheather the data have gis information
    $scope.getGisMessage = function (mapData) {
        if (!mapData) {
            return false;
        }

        var message = "";

        $.ajax({
            url: servicePath + "api/map/GisSession",
            async: false,
            datatype: "json",
            type: "get",
            data: "mapData=" + mapData,
            success: function (response) {
                var data = JSON.parse(response);
                message = data.message;
            }
        });

        return message;
    };

    //view parcel detail information
    $scope.viewParcelDetail = function (item) {
        aca.ui.toggleSearchResult();
        $scope.parceItem = item;
        $scope.number = item.ParcelNumber;
        $scope.type = "apo";
        $scope.isShowCAP = false;
    };

    //view parcel or record detail information
    $scope.viewCapDetail = function (itemNumber) {
        $http({
            method: "GET",
            params: { "itemNumber": itemNumber },
            url: servicePath + "api/Map/CapDetailInfo"
        }).success(function (response, status, headers, config) {
            var obj = response.capDetail;

            if (typeof (obj.Record) != "undefined" && obj.Record.length > 0) {
                $scope.CapDetailInfo = obj.Record[0];
                $scope.number = $scope.CapDetailInfo.capID;
                $scope.type = "cap";
                $scope.isShowCAP = true;
                $scope.GetCAPDocuments(obj.Record[0].capID, obj.Record[0].agencyCode);
                $scope.RefreshDocumentList = function() {
                    $scope.GetCAPDocuments(obj.Record[0].capID, obj.Record[0].agencyCode);
                };
            }

            aca.ui.toggleSearchResult();
        }).error(function (response, status, headers, config) {
            ShowModalWindow($scope.HomeData.aca_newui_home_label_error, response.Message);
        });
    };

    $scope.searchViewDatail = function (itemNumber, itemType) {
        aca.ui.toggleSearchResult();
        var datas = $scope.resultdetail == undefined ? $scope.AdaptCheckeRecordData[itemType] : $scope.resultdetail(itemType);

        $scope.CurrentMapTypeSource = $scope.MapTypeSource[1];
        angular.forEach(datas, function (value, key) {
            var itemKey = "";
            if (itemType == "Record") {
                itemKey = value.PermitNumber;
            } else if (itemType == "Properties") {
                itemKey = value.SortID;
            } else if (itemType == "Professionals") {
                itemKey = value.SortID;
            }
            if (itemNumber == itemKey) {
                var selectData = new Array();
                selectData.push(value);

                $scope.globalSearchDataInMap = selectData;
                $scope.globalSearchDataInMapType = itemType;

                // Show document
                if (itemType == "Professionals") {
                    $scope.GetLPDocuments(value.ResLicenseType, value.LicenseType, value.LicenseNumber);
                } else if (itemType == "Record") {
                    $scope.GetCAPDocuments(value.CapID, value.AgencyCode);
                    $scope.RefreshDocumentList = function () {
                        $scope.GetCAPDocuments(value.CapID, value.AgencyCode);
                    };
                }
                return;
            }
        });
    };

    //locate in map
    $scope.singleLocateOnMap = function (parame, type) {
        var paramsData;
        var url;

        if (type == "cap") {
            paramsData = { "capIds": parame };
            url = servicePath + "api/Map/locate";
        }
        else if (type == "apo") {
            paramsData = { "addressNumber": "", "parcelNumber": parame };
            url = servicePath + "api/Map/APOLocateInfo";
        }
        else {
            return;
        }

        $http({
            method: "GET",
            params: paramsData,
            url: url
            }).success(function (response, status, headers, config) {
                var locateInfo = JSON.stringify(response);
                MapService.locate(locateInfo);

            }).error(function (response, status, headers, config) {
                ShowModalWindow($scope.HomeData.aca_newui_home_label_error, data.Message);
            });
    };

    //single record locate on the map by capIds
    $scope.singleLocateOnMapFormGloble = function (parame, type) {
        //cap
        if (type == "cap") {
            if (typeof (parame) == "undefined" && parame == "") {
                return;
            }

            $http({
                method: "GET",
                params: { "capIds": parame },
                url: servicePath + "api/Map/locate"
            }).success(function (response, status, headers, config) {
                var locateInfo = JSON.stringify(response);
                if ($scope.showMapView != undefined) {
                    $scope.showMapView(true);
                }
                MapService.locate(locateInfo);

            }).error(function (data, status, headers, config) {
                ShowModalWindow($scope.HomeData.aca_newui_home_label_error, data.Message);
            });
        }

        //APO
        if (type == "apo") {
            if (typeof (parame) == "undefined" && parame == "") {
                return;
            }
            var addressNumber = parame.AddressSourceNumber + "&" + parame.AddressDescription;
            var parcelNumber = parame.ParcelNumber;

            $http({
                method: "GET",
                params: { "addressNumber": addressNumber, "parcelNumber": parcelNumber },
                url: servicePath + "api/Map/APOLocateInfo"
            }).success(function (response, status, headers, config) {
                var locateInfo = JSON.stringify(response);
                if ($scope.showMapView != undefined) {
                    $scope.showMapView(true);
                }
                MapService.locate(locateInfo);

            }).error(function (data, status, headers, config) {
                ShowModalWindow($scope.HomeData.aca_newui_home_label_error, data.Message);
            });
        }

    };

    //action by map data's command
    $scope.actionByData = function (mapData) {
        if (typeof (mapData) != "undefined") {
            var data = JSON.parse(mapData);
            var command = data.command;
            var gisId = data.gISObjects[0].key;
            var layerId = data.gISObjects[0].type;
            var serviceId = data.mapServiceId;

            if (layerId.toLowerCase() == "address") {
                ShowModalWindow($scope.HomeData.aca_newui_home_label_notice, $scope.HomeData.aca_newui_home_label_nodata);
                return;
            }

            if (gisId == "undefined" || layerId == "undefined") {
                ShowModalWindow($scope.HomeData.aca_newui_home_label_notice, $scope.HomeData.aca_newui_home_label_nodata);
                return;
            }

            //show record
            if (command == aca.data.commond.show || command == aca.data.commond.viewPropertyInfo) {
                $("#map-view").show();

                $http({
                    method: "GET",
                    params: { "gisId": gisId, "layerId": layerId, "serviceId": serviceId },
                    url: servicePath + "api/Map/SearchResult4Map"
                })
                .success(function (response, status, headers, config) {
                    var obj = response.searchResult;
                    var parcelCount = obj.Parcel.length;
                    var recordCount = obj.Record.length;

                    if (parcelCount + recordCount < 1) {
                        ShowModalWindow($scope.HomeData.aca_newui_home_label_notice, $scope.MapViewData.aca_newui_mapview_msg_norecord);
                        return;
                    }
                    $scope.GisRecordsCount = parcelCount + recordCount;
                    $scope.GisDescription = parcelCount + " Properties  , " + recordCount + " Records";
                    $scope.isShowParcel = parcelCount > 0 ? true : false;
                    $scope.isShowRecord = recordCount > 0 ? true : false;

                    $scope.apoList = obj.Parcel;
                    $scope.capList = obj.Record;

                    //if the commadn is view property info then show property,otherwise show property and record
                    if (command == aca.data.commond.viewPropertyInfo) {
                        $scope.GisRecordsCount = parcelCount;
                        $scope.GisDescription = parcelCount + " Properties ";
                        $scope.isShowParcel = true;
                        $scope.isShowRecord = false;
                    }

                    aca.ui.showSlidePanel();
                    if ($("#SearchList-Item-Detail")[0].style.display != "none") {
                        aca.ui.toggleSearchResult();
                    }
                })
                .error(function (response, status, headers, config) {
                    ShowModalWindow($scope.HomeData.aca_newui_home_label_error, response.Message);
                });
            }
            //others
        }

    };

    //Home or GlobalSerch Broadcast  Data type(object,string)
    $scope.$on('globalSearchMapBroadcast', function (d, data) {

        if (typeof (data) == 'string') {
            $scope.CurrentMapTypeSource = data;
            $scope.IsShowCheckeRecord(false);
        } else {
            $scope.CurrentMapTypeSource = data.key;
            var urlDate = JSON.parse(data.value);
            if (urlDate.queryText == "" || urlDate.types == "") return;
            $scope.IsShowCheckeRecord(false);
            $scope.cacheGlobalSearchBroadcastData(urlDate);
            $scope.synchronousQueryGlobalSearch(urlDate, true, function () {
                $scope.multipleLocateOnMapService($scope.resultdetail('Properties'), $scope.resultdetail('Record'));
            }, function () {
                $scope.multipleLocateOnMapService($scope.resultdetail('Properties'), $scope.resultdetail('Record'));
            });
        }
        aca.ui.showSlidePanel();
        if ($("#SearchList-Item-Detail")[0].style.display != "none") {
            aca.ui.toggleSearchResult();
        }
    });

    //Zoom icon    number: parcelNumber RecordNumber 
    $scope.enlarge = function (number, target) {
        if (typeof (number) != "undefined" && number != "") {
            var obj = $(target.currentTarget);
            var isDataTarget = obj.attr("data-Target");

            if (typeof (isDataTarget) != "undefined" && isDataTarget != "" && isDataTarget == "true") {
                obj.removeAttr("data-Target");
                return;
            }

            var actionBtnDisplay = obj.parent().parent().find('.actionbutton').css("display");
            var isEnlarge = true;

            if (actionBtnDisplay == "block" || actionBtnDisplay == "inline") {
                isEnlarge = false;
            }

            var enlargeInfo = {
                "GraphicLayerID":
                 {
                     "id": number,
                     "isEnlarge": isEnlarge
                 }
            };

            MapService.enlarge(enlargeInfo);

        }
    };

    //adaptation Checke Data TO on map
    $scope.AdaptCheckeRecord = function (type, typeData) {
        $scope.locationSelectedDataType = new Array();
        $scope.AdaptCheckeRecordData = new Array();

        var datasource = type == "Properties" ? typeData : (type != undefined ? new Array() : ($scope.SelectedData["Properties"] || new Array()));

        var data = new Array();
        var parcelCount = 0;
        for (var keyData in datasource) {

            data.push(datasource[keyData]);
        }
        parcelCount = data.length;
        if (parcelCount > 0) {
            $scope.locationSelectedDataType.push("Properties");
            $scope.AdaptCheckeRecordData["Properties"] = data;
        }

        data = new Array(); //Clear Data
        datasource = type == "Record" ? typeData : (type != undefined ? new Array() : ($scope.SelectedData["Record"] || new Array()));
        for (var keyData in datasource) {

            data.push(datasource[keyData]);
        }

        if (data.length > 0) {
            $scope.locationSelectedDataType.push("Record");
            $scope.AdaptCheckeRecordData["Record"] = data;
        }

        if (data.length + parcelCount > 0) {
            $scope.RecordsCount = data.length + parcelCount;
            $scope.RecordsDescription = parcelCount + " Properties  , " + data.length + " Records";
            $scope.IsShowCheckeRecord(true);
        } else {
            $scope.IsShowCheckeRecord(false);
        }
    };

    //is Show Check Recod
    $scope.IsShowCheckeRecord = function (isShow) {
        //init 
        if (isShow == undefined) {
            $scope.IsShowCheckeRecordData = false;
            $scope.AdaptCheckeRecord("Record", MapService.parameter("dataSource"));
            return;
        }

        $scope.IsShowCheckeRecordData = isShow;
    };

    //Back Map List to Root
    $scope.BackShowRecord = function () {
        $("#SearchList-Item").addClass("animation animating fadeInLeft")
            .one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend",
                function () {
                    $(this).removeClass("animation animating fadeInLeft");
                });
        $scope.IsShowCheckeRecord(false);
        //current Map source
        $scope.CurrentMapTypeSource = $scope.MapTypeSource[1];
    };

    //Get dataSource 
    $scope.MyRecordsDataSource = function () {

        return $scope.ExistsParameterKey("dataSource");
    };
    //Exist Parameter key
    $scope.ExistsParameterKey = function (key) {
        var dataSource = MapService.parameter(key);
        if (dataSource != undefined) {
            return true;
        } else {
            return false;
        }
    };

    //Show Map ListView  parameter[type,typeData]  attribute
    $scope.$on('showMapListView', function (d, data) {
        var type = data["type"];
        var typeData = data["typeData"];
        $scope.AdaptCheckeRecord(type, typeData);
        aca.ui.showSlidePanel();
    });

    //default init DocumentList
    $scope.DocumentList = new Array();

    //Get CAP DocumentList
    $scope.GetCAPDocuments = function (capId, agencyCode) {
        var jsonParams = { "agencyCode": agencyCode, "capID": capId };
        $scope.GetDocuments(jsonParams, "CapDocuments");
    };

    //Get LP DocumentList
    $scope.GetLPDocuments = function (resLicenseType, licenseType, licenseNumber) {
        var jsonParams = { "resLicenseType": resLicenseType, "licenseType": licenseType, "licenseNumber": licenseNumber };
        $scope.GetDocuments(jsonParams, "LpDocuments");
    };

    //Get Documents  (http action)
    $scope.GetDocuments = function (jsonParams, actionName) {
        $http({
            method: "GET",
            params: jsonParams,
            url: servicePath + "api/Map/" + actionName
        }).success(function (response, status, headers, config) {
            if (response.isOk) {
                $scope.DocumentList = response.data;
            } else {
                $scope.DocumentList = new Array();
            }
        }).error(function (data, status, headers, config) {
            ShowModalWindow(HomeData.aca_newui_home_label_error, data.Message);
        });
    };

    //Download  Document
    $scope.DownloadDocument = function (document) {
        if (document.isDownload) {
            $window.open((servicePath + "api/Map/DownloadDocument?agencyCode=" + document.agencyCode + "&altId=" + document.altId + "&capClass=" + document.capClass + "&documentNo=" + document.documentNo + "&entityID=" + document.entityID + "&entityType=" + document.entityType + "&fileKey=" + document.fileKey + "&moduleName=" + document.moduleName), '_self');
        }
    };

    //is MapView
    $scope.IsMapView = function () {
        return ($location.$$path == "/MapView");
    };

    //Link to the Apply
    $scope.ApplyFromGlobalSearch = function (actionBar, parcelNumber, addressDescription) {
        var applyParameter = { "applyFromGS": actionBar, "ParcelNumber": parcelNumber, "AddressDescription": addressDescription };
        GlobalSearchParameterService.value = applyParameter;
        $location.path("#/");
    };
});