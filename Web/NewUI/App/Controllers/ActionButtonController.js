/**
* <pre>
* 
*  Accela Citizen Access
*  File: ActionButtonController.js
* 
*  Accela, Inc.
*  Copyright (C): 2014
* 
*  Description:
* 
*  Notes:
* $Id: ActionButtonController.js 72643 2014-06-19 09:52:06Z $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
*  07/09/2014     		Shine				Initial.  
* </pre>
*/
app.controller('ActionButtonCtrl', function ($scope, $http, $q) {
    $scope.notice = $scope.HomeData.aca_newui_home_label_notice;

    //Add to Cart
    $scope.AddToCart = function (id, agencyCode, capClass, hasNopaidFee, renewalStatus) {
        $http({
            method: "GET",
            params: { "id": id, 'agencyCode': agencyCode, 'capClass': capClass, 'hasNopaidFee': hasNopaidFee, 'renewalStatus': renewalStatus },
            url: servicePath + "api/ActionButton/AddRecordToCart"
        }).success(function (response, status, headers, config) {

            try {
                var isSuccess = response.result[0].isSuccess;
                if (isSuccess) {
                    var message = $scope.HomeData.aca_newui_home_msg_addcartsuccess;
                    ShowModalWindow($scope.notice, message);
                    $scope.GetAllShoppingCart();
                }
            } catch (ex) {
                ShowModalWindow($scope.notice, $scope.HomeData.aca_newui_home_msg_addcartfailure);
            }
        }).error(function (data, status, headers, config) {
            try {
                var errorMessage = eval('(' + data.ExceptionMessage + ')');
                var message = errorMessage.message;
                ShowModalWindow($scope.notice, message);
            } catch (ex) {
                ShowModalWindow($scope.notice, data.Message);
            }
        });
    };

    //Add Collection.
    //TODO
    $scope.navToAddCollection = function (CapID, CapClass) {
        var title = $scope.HomeData.aca_newui_home_label_createcollection;
        var cap = {};
        cap[CapID] = CapClass;
        ShowCollcetionWindow(title, cap, false);
    };

    //Add  record to Collection.
    $scope.AddCollection = function (capid, collectionId, capclass, isAddCapAmount) {
        var collectionModel = new Object();
        var caps = [];
        collectionModel.CollcetionName = null;
        collectionModel.CollectionDescription = null;
        collectionModel.CollectionId = collectionId;
        collectionModel.Capid = capid;
        collectionModel.CapClass = capclass;
        caps.push(collectionModel);

        $http({
            method: "POST",
            data: caps,
            url: servicePath + "api/ActionButton/AddToCollection"
        }).success(function (response, status, headers, config) {

            if (response.isSuccessful && isAddCapAmount) {
                $scope.addCollcetionCapAmount(collectionId);
            }
            ShowModalWindow($scope.notice, response.message);

        })
       .error(function (data) {
           ShowModalWindow($scope.notice, data.ExceptionMessage);
       });
    };

    //Show Collection
    $scope.showCollectionDetail = function (id) {
        var url = "MyCollection/MyCollectionDetail.aspx?collectionId=" + id;
        setIframeSrc(url);
    };

    //Show ViewDetail
    $scope.ShowViewDetail = function (url) {
        url = url.toString().trim();
        url = url.replace("~/", '');
        setIframeSrc(url);
    };

    //Show ViewDetail
    $scope.ShowViewDetail = function (url, moduleName) {
        $http({
            method: "get",
            params: { "url": url, "moduleName": moduleName},
            url: servicePath + "api/Actioncenter/CapDetail"
        }).success(function (response, status, headers, config) {
                if (response.isForceLogin) {
                    ShowModalWindow(CommonData.aca_newui_home_label_notice, CommonData.acc_login_label_forceLoginNote);
                    redirectByRoute("Login");
                    return;
                } else {
                    url = url.toString().trim();
                    url = url.replace("~/", '');
                    setIframeSrc(url);
                }

            })
       .error(function (data) {
           ShowModalWindow($scope.notice, data.ExceptionMessage);
       });
    };

    //  link to pay
    $scope.PayFeesInDeepLink = function (url) {
        setIframeSrc(url);
    };

    // link to Resume
    $scope.ResumeApplication = function (url) {
        if ($scope.callBackResume != undefined) {
            setIframeSrc(url, $scope.callBackResume);
        } else {
            setIframeSrc(url);
        }
    };

    //Record the report generation
    $scope.generatReportBut = function (moduleName, reportID, reportType, AgencyCode) {
        var url = servicePath + "Report/ReportParameter.aspx?module=" + moduleName + "&reportType=" + reportType + "&reportID=" + reportID + "&agencyCode=" + AgencyCode;
        window.open(url, "_blank", "top=200,left=200,height=600,width=800,status=yes,toolbar=1,menubar=no,location=no,scrollbars=yes");
    };

    // link to upload document on detail page
    $scope.UpLoad = function (url) {
        url = url.replace("~/", '');
        url += "&isUpLoadFromNewUI=Y";
        setIframeSrc(url, UpLoadCallBack);

        var iFrame = document.getElementById('capIfram');

        if (navigator.userAgent.indexOf("MSIE") <= 0) {
            iFrame.addEventListener("load", showUpLoad, false);
        } else {
            iFrame.onreadystatechange = function () {
                if (this.readyState && this.readyState != 'complete') return;
                else {
                    var btnUpLoad = $(iFrame.contentWindow.document).find('[id$=divHtml5Upload]').find('a');
                    if (btnUpLoad.length > 0) {
                        iFrame.contentWindow.invokeClick(btnUpLoad, true);
                        iFrame.onreadystatechange = null;
                    }
                }
            };
        }
    };

    function UpLoadCallBack() {
        $scope.RefreshDocumentList();
    }

    function showUpLoad() {
        var iFrame = document.getElementById('capIfram');
        var btnUpLoad = $(iFrame.contentWindow.document).find('[id$=divHtml5Upload]').find('a');

        if (btnUpLoad.length > 0) {
            iFrame.contentWindow.invokeClick(btnUpLoad, true);
            iFrame.removeEventListener("load", showUpLoad, false);
        };

        /*
        var btnUpLoad = iFrame.contentWindow.document.getElementById('fileInput_ctl00_PlaceHolderMain_attachmentEdit_divHtml5Upload');

        if (typeof (btnUpLoad)!="undefined") {
        iFrame.contentWindow.invokeClick(btnUpLoad, true);
        iFrame.removeEventListener("load", showUpLoad, false);
        }
        */
    }

    // link to copy record on detail page
    $scope.CopyRecord = function (url) {
        url = url.replace("~/", '');
        setIframeSrc(url);

        var iFrame = document.getElementById('capIfram');

        if (navigator.userAgent.indexOf("MSIE") <= 0) {
            iFrame.addEventListener("load", showCopyRecord, false);
        } else {
            iFrame.onreadystatechange = function () {
                if (this.readyState && this.readyState != 'complete') return;
                else {
                    if (typeof (iFrame.contentWindow.ShowClonePage) != "undefined") {
                        iFrame.contentWindow.ShowClonePage();
                        iFrame.onreadystatechange = null;
                    }
                }
            };
        }
    };

    function showCopyRecord() {
        var iFrame = document.getElementById('capIfram');
        if (typeof (iFrame.contentWindow.ShowClonePage) != "undefined") {
            iFrame.contentWindow.ShowClonePage();
            iFrame.removeEventListener("load", showCopyRecord, false);
        }
    }

    // link to Schedule on detail page
    $scope.Schedule = function (url) {
        url = url.replace("~/", '');
        setIframeSrc(url);
        var iFrame = document.getElementById('capIfram');
        if (navigator.userAgent.indexOf("MSIE") <= 0) {
            iFrame.addEventListener("load", showSchedule, false);
        } else {
            iFrame.onreadystatechange = function () {
                if (this.readyState && this.readyState != 'complete') return;
                else {
                    var btnSchedule = $(iFrame.contentWindow.document).find('[id$=lnkInspectionSchedule]');
                    if (btnSchedule.length > 0) {
                        iFrame.contentWindow.invokeClick(btnSchedule, true);
                        iFrame.onreadystatechange = null;
                    }
                }
            };
        }
    };

    function showSchedule() {
        var iFrame = document.getElementById('capIfram');
        var btnSchedule = $(iFrame.contentWindow.document).find('[id$=lnkInspectionSchedule]');
        if (btnSchedule.length > 0) {
            iFrame.contentWindow.invokeClick(btnSchedule, true);
            iFrame.removeEventListener("load", showSchedule, false);
        }
    }

    //    $scope.InitCollectionModel = function() {
    //        $http({
    //            method: "GET",
    //            url: servicePath + "api/ActionButton/GetCollectionModel"
    //        }).success(function(response, status, headers, config) {
    //            $scope.CollectionModel = response.CollectionsInfo;
    //        });
    //    };
});