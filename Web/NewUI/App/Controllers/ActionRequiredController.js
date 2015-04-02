/**
* <pre>
* 
*  Accela Citizen Access
*  File: ActionReuqiredController.js
* 
*  Accela, Inc.
*  Copyright (C): 2014
* 
*  Description:
* 
*  Notes:
* $Id: ActionReuqiredController.js 72643 2014-07-17 09:52:06Z $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
*  07/17/2014     		Leeson				Initial.  
* </pre>
*/

app.controller('ActionRequiredCtrl', function ($scope, $http, $q) {

    //init ActionRequired Pagination (private)
    $scope.initialActionRequiredPagination = function (pageCount, isInitial) {

        //Recursive reset pageTotal
        if (!isInitial) {
            $("#actionRequiredPagination").pagination('updateItems', pageCount);
            return;
        }

        //actionRequiredPagination
        $("#actionRequiredPagination").pagination({
            items: pageCount,
            itemsOnPage: 1,
            cssStyle: 'compact-theme',
            onPageClick: function (pageNumber, event) {
                var array = [];
                for (var i = (pageNumber - 1) * 10; i < pageNumber * 10 && i < $scope.AllActionRequiredList.length; i++) {
                    array.push($scope.AllActionRequiredList[i]);
                }

                $scope.$apply(function() {
                    $scope.ActionRequiredList = array;
                });
            }
        });
    };

    //Get UrgentRecordList (private)
    $scope.getUrgentRecordList = function () {
        $scope.UrgentRecordList = [];

        for (var i = 0; i < $scope.AllActionRequiredList.length; i++) {
            var dueDate;

            if ($scope.AllActionRequiredList[i].inspectionExpiration) {
                dueDate = new Date(Date.parse($scope.AllActionRequiredList[i].inspectionExpirationDate.replace(/-/g, "/")));
            }
            else if ($scope.AllActionRequiredList[i].recordExpiration) {
                dueDate = new Date(Date.parse($scope.AllActionRequiredList[i].recordExpirationDate.replace(/-/g, "/")));
            }
            else if ($scope.AllActionRequiredList[i].feeDueExpiration) {
                dueDate = new Date(Date.parse($scope.AllActionRequiredList[i].feeDueExpirationDate.replace(/-/g, "/")));
            }

            if (typeof (dueDate) == "undefined" || dueDate == null || dueDate == "Invalid Date") {
                continue;
            }

            var currentDate = new Date();
            var days = Math.floor((dueDate.getTime() - currentDate.getTime()) / (24 * 3600 * 1000));

            if (days >= 0 && days <= 7) {
                $scope.AllActionRequiredList[i].dueDays = days;
                $scope.UrgentRecordList.push($scope.AllActionRequiredList[i]);
            }
        }
    };

    //process ActionRequired List
    $scope.processActionRequiredList = function(list) {
        var records = [];

        if (list.status != 200) {
            return records;
        }

        $.each(list.result, function(i, item) {

            var tmp;

            //if one record has more than one expiration, push more.
            if (item.inspectionExpiration) {
                tmp = jQuery.extend(true, {}, item);
                tmp.feeDueExpiration = false;
                tmp.recordExpiration = false;
                records.push(tmp);
            }

            if (item.recordExpiration) {
                tmp = jQuery.extend(true, {}, item);
                tmp.feeDueExpiration = false;
                tmp.inspectionExpiration = false;
                records.push(tmp);
            }

            if (item.feeDueExpiration) {
                tmp = jQuery.extend(true, {}, item);
                tmp.inspectionExpiration = false;
                tmp.recordExpiration = false;
                records.push(tmp);
            }
        });

        records.sort(function(a, b) {
            //override sorting algorithm
            try {
                var data1;
                var data2;

                //lookup expiration date
                if (a.inspectionExpiration) {
                    data1 = new Date(Date.parse(a.inspectionExpirationDate.replace(/-/g, "/")));
                    //data1 = a.inspectionExpirationDate;
                } else if (a.recordExpiration) {
                    data1 = new Date(Date.parse(a.recordExpirationDate.replace(/-/g, "/")));
                    //data1 = a.recordExpirationDate;
                } else if (a.feeDueExpiration) {
                    data1 = new Date(Date.parse(a.feeDueExpirationDate.replace(/-/g, "/")));
                    //data1 = a.feeDueExpirationDate;
                }

                if (b.inspectionExpiration) {
                    data2 = new Date(Date.parse(b.inspectionExpirationDate.replace(/-/g, "/")));
                    //data2 = b.inspectionExpirationDate;
                } else if (b.recordExpiration) {
                    data2 = new Date(Date.parse(b.recordExpirationDate.replace(/-/g, "/")));
                    //data2 = b.recordExpirationDate;
                } else if (b.feeDueExpiration) {
                    data2 = new Date(Date.parse(b.feeDueExpirationDate.replace(/-/g, "/")));
                    //data2 = b.feeDueExpirationDate;
                }

                //if expired date is same, then sort by customId
                if (data1.toLocaleDateString() == data2.toLocaleDateString()) return a.customId.localeCompare(b.customId);
                return data1 > data2 ? 1 : -1;
            } catch(e) {
                return 0;
            }
        });

        return records;
    };

    //get ActionRequired
    $scope.getActionRequiredInfo = function (isInitial) {

        var deferred = null;
        if ($q != null) {
            deferred = $q.defer();
        }

        $http({
            method: "GET",
            //params: { "pageIndex": pageIndex },
            url: servicePath + "api/ActionRequired/GetActionRequiredInfo"
        }).success(function (response, status, headers, config) {
            $scope.AllActionRequiredList = $scope.processActionRequiredList(response);

            $scope.getUrgentRecordList();
            $scope.ActionRequiredList = [];

            for (var i = 0; i < 10 && i < $scope.AllActionRequiredList.length; i++) {
                $scope.ActionRequiredList.push($scope.AllActionRequiredList[i]);
            }

            $scope.initialActionRequiredPagination(Math.ceil($scope.AllActionRequiredList.length / 10), isInitial);
        }).error(function (data, status, headers, config) {
            if ($q != null) {
                deferred.resolve(status);
            }
            ShowModalWindow($scope.HomeData.aca_newui_home_label_error, data);
        });
    };

    // init ActionRequired Info execute(private)
    $scope.initialActionRequiredInfo = function () {

        $scope.isShowMore = false;
        $scope.getActionRequiredInfo(true);

    };

    // init ActionRequiredList
    $scope.ActionRequiredList = new Array();

    // init ActionRequired Info 
    $scope.initialActionRequiredInfo();

    //default not ShowMore
    $scope.isShowMore = false;

    $scope.ShowMore = function () {
        $scope.isShowMore = !$scope.isShowMore;
    };

    // ActionRequired  Operation
    $scope.ActionRequiredOperation = function (action, id, module, serviceProviderCode) {
        var url = "";
        var capId1 = "";
        var capId2 = "";
        var capId3 = "";

        if (id != null) {
            var capId = id.split("-");
            if (capId.length == 4) {
                capId1 = capId[1];
                capId2 = capId[2];
                capId3 = capId[3];
            }
            else {
                ShowModalWindow($scope.HomeData.aca_newui_home_label_error, "There are an error occurs, please try it later.");
            }
        }

        switch (action) {
            case "detail":
                url = "Cap/CapDetail.aspx?Module=" + encodeURIComponent(module) + "&TabName=" + encodeURIComponent(module) + "&capID1=" + capId1 + "&capID2=" + capId2 + "&capID3=" + capId3 + "&agencyCode=" + encodeURIComponent(serviceProviderCode) + "&IsToShowInspection=true";
                break;

            case "renew":
                var isSuperAgency = "";

                var deferred = null;
                if ($q != null) {
                    deferred = $q.defer();
                }

                $http({
                    method: "GET",
                    //params: { "pageIndex": pageIndex },
                    url: servicePath + "api/ActionRequired/GetIsSuperAgency"
                }).success(function (response, status, headers, config) {
                    isSuperAgency = response;

                }).error(function (data, status, headers, config) {
                    if ($q != null) {
                        deferred.resolve(status);
                    }
                    ShowModalWindow($scope.HomeData.aca_newui_home_label_error, data);
                });

                url = "urlrouting.ashx?type=1006&permitType=renewal&Module=" + encodeURIComponent(module) + "&capID1=" + capId1 + "&capID2=" + capId2 + "&capID3=" + capId3 + "&agencyCode=" + encodeURIComponent(serviceProviderCode) + "&stepNumber=2&pageNumber=1&isFeeEstimator=N&isRenewal=Y&isSubAgencyCap=" + isSuperAgency;

                break;
            case "pay":
                url = "urlrouting.ashx?type=1009&permitType=PayFees&agencyCode=" + encodeURIComponent(serviceProviderCode) + "&Module=" + encodeURIComponent(module) + "&capID1=" + capId1 + "&capID2=" + capId2 + "&capID3=" + capId3 + "&stepNumber=0&isPay4ExistingCap=Y";

                break;
            default:
                return false;
        }

        setIframeSrc(url, function () {
            $scope.initialActionRequiredInfo();
            $scope.$emit('actionRequired-parent', 'MyRecordRefresh');
        });
    };
});

