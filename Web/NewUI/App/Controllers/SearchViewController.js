/**
* <pre>
* 
*  Accela Citizen Access
*  File: SearchViewController.js
* 
*  Accela, Inc.
*  Copyright (C): 2014
* 
*  Description:
* 
*  Notes:
* $Id: LoginController.js 72643 2014-06-19 09:52:06Z $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
*  06/24/2014     		Awen				Initial.  
* </pre>
*/
app.controller('SearchCtrl', function ($scope, $http, $q, $window, $location, MapService, GlobalSearchParameterService, LabelKeyService) {
    $scope.route = LabelKeyService.routeName.SearchView;
    $scope.ShowLanguage(false);

    $scope.initSearchView=function () {
        //get label key
        if (LabelKeyService.getLabelKey($scope.route, $scope.selectedLanguage)) {
            $scope.SearchViewData = LabelKeyService.DataKeys;
        }

        //is Show Search map
        $scope.isShowSearchMap = false;
        $scope.ShowMapOrList($scope.isShowSearchMap);
    };

    // show map or show searchlist
    $scope.showMapView = function (isShow) {
        $scope.isShowSearchMap = isShow;
        $scope.ShowMapOrList($scope.isShowSearchMap);
    };

    $scope.ShowMapOrList = function (isShowSearchMap) {
        if (isShowSearchMap) {
            $("#search-list-view").css("display", "none");
            $("#search-map-view").css("z-index", "1");
            aca.ui.showSlidePanel();
        } else {
            $("#search-list-view").css("display", "block");
            $("#search-map-view").css("z-index", "-10");
        }
    };

    //GlobalSearch  query
    $scope.$on('globalSearchParameter', function (d, data) {
        var urlDate = JSON.parse(data);
        $scope.cacheGlobalSearchBroadcastData(urlDate);

        $scope.$broadcast("globalSearchMapBroadcast", "GlobalSearch");
        $scope.synchronousQueryGlobalSearch(urlDate, true, function () {
            if (!$scope.isShowSearchMap) return;
            try {
                $scope.multipleLocateOnMapService($scope.resultdetail('Properties'), $scope.resultdetail('Record'));
            } catch (e) { }
        }, function () {
            if (!$scope.isShowSearchMap) { return; }
            try {
                $scope.multipleLocateOnMapService($scope.resultdetail('Properties'), $scope.resultdetail('Record'));

            } catch (e) { }
        });
    });

    //init GlobalSearch
    var initGlobalSearch = function ($scope, $http, $q, GlobalSearchParameterService) {
        if (GlobalSearchParameterService.value == "") return;
        var data = JSON.parse(GlobalSearchParameterService.value);
        $scope.cacheGlobalSearchBroadcastData(data);
        $scope.synchronousQueryGlobalSearch(data, true, function () {
            if (!$scope.isShowSearchMap) return;
            $scope.multipleLocateOnMapService($scope.resultdetail('Properties'), $scope.resultdetail('Record'));
        }, function () {
            if (!$scope.isShowSearchMap) return;
            $scope.multipleLocateOnMapService($scope.resultdetail('Properties'), $scope.resultdetail('Record'));
        });
    };

    //init 
    initGlobalSearch($scope, $http, $q, GlobalSearchParameterService);

    //Check in Select Cache data
    $scope.ShowCacheSelectData = function (id) {
        var result = false;
        angular.forEach($scope.resultTypes, function (value, key) {
            var dateSource = $scope.resultdetail(value) || new Array();
            if (dateSource.length > 0) {
                var eachData = $scope.SelectedData[value] || new Array();
                for (var itemData in eachData) {
                    if (itemData == id) {
                        result = true;
                        return;
                    }
                }
            }
        });
        return result;
    };

    // Check Record   existence of.
    $scope.CheckCacheRecordType = function(resultType) {
        var globallyData = $scope.SelectedData[resultType] || new Array();
        var count = 0;
        for (var item in globallyData) {
            count++;
        }
        return count > 0 ? true : false;
    };
    
    // Check Record
    $scope.CheckedRecord = function (jsonDate, resultType, control) {
        var key = control.currentTarget.id;
        var checked = control.currentTarget.checked;

        var globallyData = $scope.SelectedData[resultType] || new Array();
        if (checked) {
            globallyData[key] = jsonDate;
        } else {
            delete globallyData[key];
        }
        $scope.SelectedData[resultType] = globallyData;
    };

    //DownloadResult
    $scope.DownloadResult = function (resultType) {
        var datasource = $scope.SelectedData[resultType]||  new Array();
        datasource.sort();
        var data = new Array();
        for (var keyData in datasource) {

            data.push(datasource[keyData]);
        }
        var myDate = new Date();
        var showfield = new Array();
        
        if (resultType == "Properties") {
            showfield.push("ParcelNumber");
            showfield.push("OwnerName");
            showfield.push("AddressDescription");
        }
        if (resultType == "Professionals") {
            showfield.push("LicenseNumber");
            showfield.push("LicenseType");
            showfield.push("LicenseNumber");
            showfield.push("LicensedProfessionalName");
            showfield.push("BusinessName");
        }
        if (resultType == "Record") {
           showfield.push("RelatedRecords");
            showfield.push("Inspections");
            showfield.push("AgencyCode");
            showfield.push("ModuleName");
            showfield.push("CreatedDate");
            showfield.push("PermitNumber");
            showfield.push("PermitType");
            showfield.push("Address");
            showfield.push("Status");
        } 
        if (data.length <= 0) {
            if (resultType == "Professionals") {
                resultType = "LP";
            }
            if (resultType == "Properties") {
                resultType = "APO";
            }
            if (resultType == "Record") {
                resultType = "CAP";
            }
            $http({
                method: "GET",
                params: { "type": resultType },
                url: servicePath + "api/GlobalSearch/globalSearch-download"
            }).success(function(response, status, headers, config) {
                if (response.isOK) {
                    data = response.message;
                    DownloadJsonResult(data, showfield, "SearchList" + myDate.getFullYear() + myDate.getMonth() + myDate.getDate());
                }

            }).error(function(response, status, headers, config) {
                ShowModalWindow($scope.HomeData.aca_newui_home_label_error, response.Message);
            });
        } else {
            DownloadJsonResult(data, showfield, "SearchList" + myDate.getFullYear() + myDate.getMonth() + myDate.getDate());
        }

    };
 
      //List resize event
    $("#search-list-view").resize(function () {
        $scope.Listresize();
    });

    //List resize function
    $scope.Listresize = function() {
       var tabView = iframeMinHeight;
       var searchmenuswitch = GetControlClientHeight('#SearchResult >.search-menu-switch');
       var tabHeader = GetControlClientHeight('#search-list-view >.tab-header');
       var tabcontentMargin = parseInt($('#search-list-view').css("margin-top").replace("px", ''));
         $("#Search-View-List").css("overflow-y","scroll");
         $("#Search-View-List").css("max-height",(tabView - searchmenuswitch - tabHeader - tabcontentMargin )+ "px");
    };

    //show map or show searchlist or location
    $scope.locationShowMapView = function () {
        if (!$scope.isShowSearchMap) {
            $scope.showMapView(true);
            if ($scope.resultdetail != undefined)
                $scope.multipleLocateOnMapService($scope.resultdetail('Properties'), $scope.resultdetail('Record'));
        }
    };

    //Show ViewDetail
    $scope.ShowViewDetail = function (url) {
        url = url.toString().trim();
        url = url.replace("~/", '');
        setIframeSrc(url);
    };

    //locate on map by multiple selected
    $scope.viewOnMapByCb = function () {
    };

    //single record locate on the map by capIds
    $scope.singleLocateOnMap = function (parame, type) {
        
            if (type == "cap") {
                if (typeof (parame.CapID) == "undefined" && parame.CapID == "") {
                    return;
                }

                $http({
                    method: "GET",
                    params: { "capIds": parame.CapID },
                    url: servicePath + "api/Map/locate"
                }).success(function (response, status, headers, config) {
                    var locateInfo = JSON.stringify(response);
                    $scope.isShowSearchMap = true;
                    $scope.ShowMapOrList($scope.isShowSearchMap);
                    MapService.locate(locateInfo);
                    //open map list
                    var jsondataArray = new Array();
                    jsondataArray.push(parame);

                    var broadcastArray = new Array();
                    broadcastArray["type"] = "Record";
                    broadcastArray["typeData"] = jsondataArray;
                    $scope.$broadcast("showMapListView", broadcastArray);


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
                    $scope.isShowSearchMap = true;
                    $scope.ShowMapOrList($scope.isShowSearchMap);
                    MapService.locate(locateInfo);
                    //open map list
                    var jsondataArray = new Array();
                    jsondataArray.push(parame);

                    var broadcastArray = new Array();

                    if (type == "cap") {
                        broadcastArray["type"] = "Record";
                    }else if (type == "apo") {
                        broadcastArray["type"] = "Properties"; 
                    }

                    broadcastArray["typeData"] = jsondataArray;
                    $scope.$broadcast("showMapListView", broadcastArray);

                }).error(function (data, status, headers, config) {
                    ShowModalWindow($scope.HomeData.aca_newui_home_label_error, data.Message);
                });
            }
    };

    //locate records by checkbox
    $scope.multipleLocateOnMap = function (type) {
        var data = $scope.SelectedData[type] || new Array();
        var propertiesData = (type == "Properties") ? data : new Array();
        var recordData = (type == "Record") ? data : new Array();
        var isExistData = false;

        for (var keyData in data) {
            isExistData = true;
            break;
        }

        if (!isExistData) {
            ShowModalWindow($scope.HomeData.aca_newui_home_label_notice, $scope.SearchViewData.aca_newui_searchview_label_selectone);
            return;
        }

        $scope.isShowSearchMap = true;
        $scope.ShowMapOrList($scope.isShowSearchMap);
        $scope.multipleLocateOnMapService(propertiesData, recordData);
        //open map list
        var broadcastArray = new Array();
        broadcastArray["type"] = type;
        broadcastArray["typeData"] = data;
        $scope.$broadcast("showMapListView", broadcastArray);
    };

    //Link to the Apply
    $scope.ApplyFromGlobalSearch = function (actionBar, parcelNumber, addressDescription) {
        var applyParameter = { "applyFromGS": actionBar, "ParcelNumber": parcelNumber, "AddressDescription": addressDescription };
        GlobalSearchParameterService.value = applyParameter;
        
        $location.path("#/");
    };
    
     //add record to collection and create new collection
    $scope.addtocollection = function (pageName,type) {
        var caps= {};
        var data = $scope.SelectedData[type] || new Array();
        var count = 0;
          for (var item in data) {
               caps[data[item].CapID]= data[item].CapClass ;
              count++;
          }
         
        if (count>0) {
            aca.data.isShowExistCollection = true;
            var title;

            if (pageName=="Dashboard") {
                 title = $scope.DashboardData.aca_newui_dashboard_label_createcollection;
             }
             if (pageName == "SearchView") {
                 title = $scope.SearchViewData.aca_newui_searchview_label_createcollection;
             }
            
            ShowCollcetionWindow(title, caps,true);
        } else {
            ShowModalWindow($scope.HomeData.aca_newui_home_label_notice, $scope.SearchViewData.aca_newui_searchview_label_selectone);
        }
    };
});
