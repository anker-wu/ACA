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
app.controller('RecordCtrl', function ($scope, $http, $q, $location, $routeParams, GlobalSearchParameterService) {
    $scope.ModuleNamesStyle = function (key) {
        var moduleNamesStyleClass = new Array();
        moduleNamesStyleClass['Building'] = 'icon-property';
        moduleNamesStyleClass['Licenses'] = 'icon-licensed_pro';
        return moduleNamesStyleClass[key];
    };

    //Show View Detail
    $scope.ShowViewDetail = function (url) {
        url = url.toString().trim();
        url = url.replace("~/", '');
        setIframeSrc(url);
    };

    $scope.SearchRecordByModule = function (module) {

        $scope.getPagedRecord(0, 10, module, false);

    };

    //init Pagination (private)
    $scope.initialPagination = function (pageCount, isInitial) {

        //Recursive reset pageTotal
        if (!isInitial) {
            $("#PagingMyRecords").pagination('updateItems', pageCount);
            return;
        }

        //PagingMyRecords
        $("#PagingMyRecords").pagination({
            items: pageCount,
            itemsOnPage: 1,
            cssStyle: 'compact-theme',
            onPageClick: function (pageNumber, event) {
                $scope.getPagedRecord(pageNumber - 1, 10, $scope.CurrentModule, false);
            }
        });
    };

    // Set Record description
    $scope.setRecordDesc = function (desc, size) {
        if (parseInt(size) == 0) {
            $scope.myRecordsdesc = $scope.DashboardData.aca_newui_dashboard_label_norecords;
            $scope.recordsSize = 0;
            return;
        }

        $scope.myRecordsdesc = desc;

        /* display 10 page by default
        1. if total count less than 100, display the total count.
        2. if page count more than 10 and it is not the last page, show the n00+.
        3. others,  display the total count.
        */
        if (parseInt(size) <= 100) {
            $scope.recordsSize = size;
        }
        else if (parseInt(size) % 100 > 0 && ($scope.CurrentPageIndex + 1) != (Math.floor(parseInt(size) / 10) + (parseInt(size) % 10 > 0 ? 1 : 0))) {
            $scope.recordsSize = size - (parseInt(size) % 10) + '+';
        }
        else {
            $scope.recordsSize = parseInt(size);
        }
    };

    // Get Page Record (private)
    $scope.getPagedRecord = function (pageIndex, pageSize, module, isInitial, asyn) {

        var deferred = null;
        if (asyn == true) {
            deferred = $q.defer();
        }

        if (typeof (pageIndex) == "undefined") {
            pageIndex = 0;
        }

        if (typeof (pageSize) == "undefined") {
            pageSize = 10;
        }

        if (typeof (module) == "undefined") {
            module = "";
        }
        $scope.CurrentModule = module;

        var sortBy = $scope.sortBy;
        var isAsc = $scope.isAsc;

        $http({
            method: "GET",
            params: { "pageIndex": pageIndex, "pageSize": pageSize, "module": module, "sortBy": sortBy, "isAsc": isAsc, "isInitial": isInitial },
            url: servicePath + "api/MyRecord/getPagedRecord"
        }).success(function (response, status, headers, config) {
            var records = response.Records.RecordsList;
            var moduleNames = new Array();
            var moduleNamesClass = new Array();
            var moduleNameDate = new Array();
            //Set  Is download CSV
            $scope.SetEnableExportCSV(response.enableExport);
            $.each(records, function (i, item) {
                if ($.inArray(item.ModuleName, moduleNames) < 0) {
                    moduleNames.push(item.ModuleName);
                    moduleNamesClass[item.ModuleName] = $scope.ModuleNamesStyle(item.ModuleName);
                    var temp = new Array();
                    temp.push(item);
                    moduleNameDate[item.ModuleName] = temp;
                } else {
                    var tempRepeat = moduleNameDate[item.ModuleName];
                    tempRepeat.push(item);
                    moduleNameDate[item.ModuleName] = tempRepeat;
                }
            });

            $scope.RecordsTypes = moduleNames;
            $scope.Recordsdetail = function (moduleType) {
                return moduleNameDate[moduleType];
            };
            $scope.Modules = response.Modules;
            $scope.CurrentPageIndex = pageIndex;
            //aca.data.facebookAppId = response.FacebookAppID;

            $scope.initialPagination(response.pageCount, isInitial);
            $scope.setRecordDesc(response.descriptions, response.totalCount);

            if (asyn == true) {
                deferred.resolve(response);
            }

        }).error(function (data, status, headers, config) {
            $scope.CurrentModule = "";
            if (asyn == true) {
                deferred.reject(data);
            } else {
                ShowModalWindow($scope.HomeData.aca_newui_home_label_error, data);
            }
        });

        if (asyn == true) {
            return deferred.promise;
        }
    };

    // soft page records
    $scope.SortPagedRecords = function (sortBy) {

        if (typeof ($scope.SortParameter) == "undefined") {
            $scope.SortParameter = null;
        }

        if (typeof ($scope.sortBy) != "undefined" && $scope.sortBy == sortBy && $scope.isAsc == true) {
            $scope.isAsc = false;
        } else {
            $scope.isAsc = true;
        }

        $scope.sortBy = sortBy;

        $scope.getPagedRecord(0, 10, $scope.CurrentModule, false);

    };

    // page init (private)
    $scope.pageInit = function () {
        $scope.sortBy = "";
        $scope.isAsc = true;
        $scope.isLastPage = false;
        var firstloading = new loadingControl($("body"));
        firstloading.initControl();
        firstloading.beginLoad();
        var promise = $scope.getPagedRecord(0, 10, "", true, true);
        promise.then(function (data) {
            firstloading.CancelLoad();
        }, function (data) {
            firstloading.CancelLoad();
            ShowModalWindow($scope.HomeData.aca_newui_home_label_error, data);
        });
    };

    // page init Execution
    $scope.pageInit();

    // PayFees 
    $scope.PayFeesInDeepLink = function (url) {
        setIframeSrc(url);
    };

    //Resume
    $scope.ResumeApplication = function (url) {
        setIframeSrc(url, callBackResume);
    };

    //Call back Resume
    $scope.callBackResume = function () {
        $scope.pageInit();
    };

    //cache data source
    $scope.SelectedData = new Array();

    //Check the data
    $scope.ShowCacheSelectData = function (id) {
        var result = false;
        angular.forEach($scope.RecordsTypes, function (value, key) {
            var dateSource = $scope.Recordsdetail(value) || new Array();
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
    $scope.CheckCacheRecordType = function (resultType) {
        var globallyData = $scope.SelectedData[resultType] || new Array();
        var count = 0;
        for (var item in globallyData) {
            count++;
        }
        return count > 0 ? true : false;
    };

    //Check Record
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

    // Download Check Record
    $scope.DownloadResult = function (resultType) {
        var datasource = $scope.SelectedData[resultType] || new Array();
        datasource.sort();
        var data = new Array();
        for (var keyData in datasource) {

            data.push(datasource[keyData]);
        }
        var myDate = new Date();
        var showfield = new Array();
        showfield.push("AgencyStateZip");
        showfield.push("ExpirationDate");
        showfield.push("CreatedBy");
        showfield.push("AuditDate");
        showfield.push("AgencyCode");
        showfield.push("ModuleName");
        showfield.push("CreatedDate");
        showfield.push("PermitNumber");
        showfield.push("PermitType");
        showfield.push("Address");
        showfield.push("Status");
        showfield.push("RelatedRecords");

        if (data.length <= 0) {
            $http({
                method: "GET",
                params: { "type": resultType },
                url: servicePath + "api/MyRecord/myRecord-download"
            }).success(function (response, status, headers, config) {

                data = response.Records.RecordsList;
                DownloadJsonResult(data, showfield, "MyRecordList" + myDate.getFullYear() + myDate.getMonth() + myDate.getDate());

            }).error(function (response, status, headers, config) {
                ShowModalWindow($scope.HomeData.aca_newui_home_label_error, response.Message);
            });

        } else {
            DownloadJsonResult(data, showfield, "MyRecordList" + myDate.getFullYear() + myDate.getMonth() + myDate.getDate());
        }
    };

    //single record locate on the map by capIds
    $scope.singleLocateOnMap = function (capids, jsonData) {
        if (typeof (capids) == "undefined" && capids == "") {
            return;
        }

        $http({
            method: "GET",
            params: { "capIds": capids },
            url: servicePath + "api/Map/locate"
        }).success(function (response, status, headers, config) {
            var locateInfo = response;
            var count = 0;
            try {
                count = response.records[0].record_by_GISObject.length +
                            response.records[0].record_by_address.length +
                            response.records[0].record_by_pacerl.length +
                            response.records[0].CAP_Parcel_Item.length +
                            response.records[0].CAP_Parcel_Item_address.length;
            } catch (e) { }

            if (count > 0) {
                var parmetes = new Array();
                parmetes["dataSource"] = [jsonData];
                parmetes["locateData"] = JSON.stringify(locateInfo);
                GlobalSearchParameterService.value = parmetes;
                $location.path("/MapView").search({ isFromDashboard: true });
            } else {
                ShowModalWindow("NOTICE", "This Record can't locate to the map !");
            }

        }).error(function (data, status, headers, config) {
            ShowModalWindow($scope.HomeData.aca_newui_home_label_error, data.Message);
        });
    };

    //locate records by checkbox
    $scope.multipleLocateOnMap = function () {
        var capIds = "";
        //locate on map Array
        var locateOnMapArray = new Array();
        angular.forEach($scope.RecordsTypes, function (value, key) {
            var eachData = $scope.SelectedData[value] || new Array();
            for (var itemData in eachData) {
                capIds += eachData[itemData].CapID + ",";
                locateOnMapArray.push(eachData[itemData]);
            }
        });

        if (capIds.charAt(capIds.length - 1) == ",") {
            capIds = capIds.substring(0, capIds.length - 1);
        }

        if (capIds != "") {
            $http({
                method: "GET",
                params: { "CkCapIds": capIds },
                url: servicePath + "api/Map/locate-All-Record"
            }).success(function (response, status, headers, config) {
                var locateInfo = response;
                var parmetes = new Array();
                parmetes["dataSource"] = locateOnMapArray;
                parmetes["locateData"] = JSON.stringify(locateInfo);
                GlobalSearchParameterService.value = parmetes;
                $location.path("/MapView").search({ isFromDashboard: true });
            }).error(function (data, status, headers, config) {
                ShowModalWindow($scope.HomeData.aca_newui_home_label_error, data.Message);
            });
        } else {
            ShowModalWindow($scope.HomeData.aca_newui_home_label_notice, $scope.DashboardData.aca_newui_dashboard_msg_noselect);
        }
    };

    //get selected recrod capId
    $scope.add2Collection = function (pageName) {
        var caps = {};
        var count = 0;
        angular.forEach($scope.RecordsTypes, function (value, key) {
            var eachData = $scope.SelectedData[value] || [];

            for (var itemData in eachData) {
                caps[eachData[itemData].CapID] = eachData[itemData].CapClass;
                count++;
            }
        });

        if (count > 0) {
            aca.data.isShowExistCollection = true;
            var title;

            if (pageName == "Dashboard") {
                title = $scope.DashboardData.aca_newui_dashboard_label_createcollection;
            }
            if (pageName == "SearchView") {
                title = $scope.SearchViewData.aca_newui_searchview_label_createcollection;
            }

            ShowCollcetionWindow(title, caps, true);
        } else {
            ShowModalWindow($scope.HomeData.aca_newui_home_label_notice, $scope.DashboardData.aca_newui_dashboard_msg_noselect);
        }
    };

    //dashBoard broadcast 
    $scope.$on('dashBoard-child', function (d, data) {
        if (data == "MyRecordRefresh") {
            $scope.pageInit();
        }
    });
});

