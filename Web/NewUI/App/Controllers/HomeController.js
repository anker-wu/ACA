/**
* <pre>
* 
*  Accela Citizen Access
*  File: HomeController.js
* 
*  Accela, Inc.
*  Copyright (C): 2014
* 
*  Description:
* 
*  Notes:
* $Id: HomeController.js 72643 2014-06-19 09:52:06Z $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
*  03/18/2008     		Kevin.Feng				Init    ial.  
* </pre>
*/
app.controller('HomeController', function ($scope, $http, $compile, $location, $window, $q, $sce, MapService, GlobalSearchParameterService, LabelKeyService) {
    $scope.route = LabelKeyService.routeName.Home;
    $scope.isAdmin = isAdmin;
    $scope.isColorTheme = isColorTheme;

    //hide Search advancedsearch
    $scope.hidSearch = true;

    // Is download CSV
    $scope.EnableExportCSV = false;

    //Set Is download CSV
    $scope.SetEnableExportCSV = function(isEnable) {
        $scope.EnableExportCSV = isEnable;
    };

    //get Is download CSV
    $scope.GetEnableExportCSV = function () {
        return $scope.EnableExportCSV;
    };

    $scope.initHome = function () {
        //get label key
        if (LabelKeyService.getLabelKey($scope.route, "")) {
            $scope.HomeData = LabelKeyService.DataKeys;
        }
        $scope.getCollcetion();
        $scope.initDdlLanguage();
        $scope.GetGlobalSearchCheckData();
    };

    //GlobalSearch type style
    $scope.globalSearchStyle = function (key) {
        var globalSearchStyleClass = new Array();
        globalSearchStyleClass['Professionals'] = 'ico-users4';
        globalSearchStyleClass['Properties'] = 'ico-location2';
        globalSearchStyleClass['Record'] = 'ico-list';
        return globalSearchStyleClass[key];
    };

    //GlobalSearch  Switch
    $scope.GlobalSearchSwitch = new Array();

    //Get GlobalSearch Check Data 
    $scope.GetGlobalSearchCheckData = function() {
        $http({
            method: "GET",
            url: servicePath + "api/Home/GlobalSearchSwitch"
        }).success(function(response) {
            $scope.GlobalSearchSwitch =response;
        });
    };

    //whether to show the language dropdowmbox
    $scope.ShowLanguage = function(isShowLanguage) {
        $scope.isShowLanguage = isShowLanguage && !isAdmin;
    };

    //change the dafault language
    $scope.ChangeLanguage = function () {
        removeUiSessionStorage();

        $http({
            method: "GET",
            params: { "culture": $scope.selectedLanguage },
            url: servicePath + "api/I18N/Change-Language"
        })
           .success(function (response) {
               sessionStorage.setItem("selectedLanguage", $scope.selectedLanguage);

               //get label key
               if (LabelKeyService.getLabelKey($scope.route, $scope.selectedLanguage)) {
                   $scope.HomeData = LabelKeyService.DataKeys;
                   loadThemeBySelectLanguage($scope.selectedLanguage);
                   $location.path("/").search({ language: $scope.selectedLanguage });
               }
           })
           .error(function (response, status, headers, config) {
               ShowModalWindow($scope.HomeData.aca_newui_home_label_error, response.Message);
           });
    };

    // init language
    $scope.initDdlLanguage = function () {
        var languageData = sessionStorage.getItem("cultureLanguage");
        var selectedLanguage = sessionStorage.getItem("selectedLanguage");

        if (languageData) {
            $scope.languageSetting(languageData, selectedLanguage);
            return;
        }

        $http({
            method: "GET",
            params: { isAdmin : isAdmin },
            url: servicePath + "api/I18N/Culture-Language"
        })
            .success(function (response) {
                $scope.languageSetting(response, null);
            })
            .error(function (response, status, headers, config) {
                ShowModalWindow($scope.HomeData.aca_newui_home_label_error, response.Message);
            });
    };

    //bind language
    $scope.languageSetting = function (languageData, selectedLanguage) {
        if (typeof (languageData) == "string") {
            languageData = JSON.parse(languageData);
        }

        $scope.languages = languageData;

        if (selectedLanguage) {
            $scope.selectedLanguage = selectedLanguage;
        } else {
            $scope.selectedLanguage = languageData[0].languageKey;
        }

        sessionStorage.setItem("cultureLanguage", JSON.stringify(languageData));

        //if the language only have one then not display,otherwise show it
        if ($scope.languages.length > 1 && !isAdmin) {
            $scope.ShowLanguage(true);
        } else {
            $scope.ShowLanguage(false);
        }
    };

    $scope.loadLaunchPad = function () {
        if (!isAdmin) {
            $window.location.href = window.location.href.split("#")[0];
            save2SessionStorage("noNeedLoadCurrentUrl", true);
        }
    };

    $scope.bindAdvSearch = function (action) {
        $(".default-search, .search-title").toggle();
        $scope.hidSearch = !$scope.hidSearch;

        if (!$scope.hidSearch) {
            $http({
                method: "GET",
                params: { actionKey: action },
                url: servicePath + "api/actioncenter/Modules"
            })
                .success(function (response) {
                    $scope.modules = response.modules;
                })
                .error(function (response, status, headers, config) {
                    ShowModalWindow($scope.HomeData.aca_newui_home_label_error, response.Message);
                });
        }
    };

    $scope.goSearch = function (url, moduleName) {
        if (url) {
            if (!moduleName) {
                moduleName = "";
            }

            $http({
                method: "GET",
                params: { "url": url, "moduleName": moduleName },
                url: servicePath + "api/home/ForceLogin"
                })
                .success(function (response) {
                    if (response.isForceLogin) {
                        ShowModalWindow(CommonData.aca_newui_home_label_notice, CommonData.acc_login_label_forceLoginNote);
                        redirectByRoute("Login");
                        return;
                    }
                    var $parent = angular.element("#txtGlobalSearh").parent().parent();
                    $parent.removeClass('show-cc');

                    if (url.indexOf("/") == 0) {
                        setIframeSrc(url.substr(1, url.length - 1));
                    }
                    else {
                        setIframeSrc(url.substr(0, url.length - 1));
                    }
                })
                .error(function (response, status, headers, config) {
                    ShowModalWindow($scope.HomeData.aca_newui_home_label_error, response.Message);
                });
        }
    };

    $scope.DefaultGlobalSearch = function () {
        if (isAdmin) {
            return false;
        }

        var queryText = $("#txtGlobalSearh").val();
        if (queryText == undefined || queryText.trim() == "" || queryText.length < 3) return;
        var keys = "";
        $('.search-content ul li').each(function () {
            if ($(this)[0].className == "selected") {
                keys += $(this).data('key') + ",";
            }
        });
        if (keys.length > 0) {
            keys = keys.substring(0, keys.length - 1);

            var globalSearchParameter = { "queryText": queryText, "types": keys, "sort": '', "isAsc": true, 'isFilter': false };
            GlobalSearchParameterService.value = JSON.stringify(globalSearchParameter);

            if ($location.$$path == "/MapView") {
                $location.$$search = undefined;
                this.$broadcast("globalSearchMapBroadcast", { "key": "GlobalSearch", "value": JSON.stringify(globalSearchParameter) });
                return;
            }

            if ($location.$$path != "/SearchView") {
                $location.$$search = undefined;
                $location.path("/SearchView");
            } else {
                this.$broadcast("globalSearchParameter", JSON.stringify(globalSearchParameter));
            }
        }
            closeHomeIframe();
    };

    $scope.styleModel = [
        {
            text: "css1",
            value: "newui/content/css1.css"
        },
        {
            text: "css2",
            value: "newui/content/css2.css"
        }
    ];

    $scope.styleSelected = $scope.styleModel[0];

    $scope.changeStyle = function (val) {
        $http.post(servicePath + 'api/publicuser/saveStyle', '{"furl":"' + val + '"}').success(function (response) {
            $scope.loadStyle(response);
        }).error(function (response) {
            console.error(response);
        });
    };

    var changedStyle;

    window.loadedIframe = function (obj) {
        if (obj.contentDocument == null) return;
        if (obj.contentDocument.body.innerHTML) {
            SetWinHeight(obj);

            //document.getElementById("selStyle").style.display = "";
            $http({
                method: "Get",
                url: servicePath + "api/publicuser/getStyle"
            }).success(function (response) {
                if (!response) {
                    return false;
                }
                $scope.loadStyle(response);

            }).error(function (response) {
                ShowModalWindow($scope.HomeData.aca_newui_home_label_error, response.Message);
            });
        } else {
            //document.getElementById("selStyle").style.display = "none";
        }
    };

    $scope.loadStyle = function (response) {
        var obj = angular.element("capIfram");

        if (obj) {
            var head = obj.contentDocument.getElementsByTagName("head");

            if (head) {
                if (changedStyle) {
                    head[0].removeChild(changedStyle);
                }

                var link = createLink(obj.contentDocument, response);
                head[0].appendChild(link);
                changedStyle = link;
            }
        }
    };

    $scope.closeIframe = function () {
        closeHomeIframe();
        
        if (aca.data.isRefreshShoppingCart) {
            $scope.GetAllShoppingCart();
            aca.data.isRefreshShoppingCart = false;
        }
    };

    // Responsive design Search Box
    $scope.phoneSearchbox = function () {
        if (!isAdmin) {
            $(".search-bar").toggle();
        }
    };

    //TODO: service 

    //synchronous  Query  GlobalSearch
    $scope.synchronousQueryGlobalSearch = function (jsonDate, newSearch, pageTurnCallback, successCallback) {
        if (newSearch) {
            //cache data source
            $scope.SelectedData = new Array();
        }
        var firstloading = new loadingControl($("body"));
        firstloading.initControl();
        firstloading.beginLoad();
        var promise = $scope.queryGlobalSearchData(jsonDate);
        promise.then(function (data) {
            if ($scope.resultTypes != null && $scope.resultTypes.length > 0) {
                $scope.paginationPage(false, pageTurnCallback);
            }
            else {
                $('.PagingMyRecords').empty();
                $('.PagingMyRecords').text($scope.HomeData.aca_newui_home_label_norecord);
                // GET MyRecordsDescription
                $scope.globalSearchDescription();
            }
            // success Callback
            if (successCallback != undefined && successCallback != "") {
                successCallback();
            }
            firstloading.CancelLoad();
        }, function (data) {
            ShowModalWindow($scope.HomeData.aca_newui_home_label_notice, data);
            firstloading.CancelLoad();
        });
    };

    //query GlobalSearchData
    $scope.queryGlobalSearchData = function (urlDate) {
        var deferred = null;
        if (urlDate == undefined || urlDate == "" || urlDate == []) {
            if ($q != null) {
                return deferred.promise;
            } else {
                return null;
            }
        }
        if ($q != null) {
            deferred = $q.defer();
        }
        $http({
            method: "GET",
            params: urlDate,
            url: servicePath + "api/GlobalSearch/GlobalSearch"
        }).success(function (response, status, headers, config) {
            if (response.record != undefined)
                $scope.globalSearchFillJsonToData(response.record[0]);
            if (response.message != "") {
                $scope.resultWarning = true;
            } else {
                $scope.resultWarning = false;
            }
            $scope.SetEnableExportCSV(response.enableExport);
            if ($q != null) {
                deferred.resolve(status);
            }

        }).error(function (response, status, headers, config) {
            if ($q != null) {
                deferred.reject(response.Message);
            }
        });
        if ($q != null) {
            return deferred.promise;
        } else {
            return null;
        }
    };

    //Fill  GlobalSearch Json To Html
    $scope.globalSearchFillJsonToData = function (response) {
        var resultType = new Array();
        var resultData = new Array();
        var size = 0;

        if (response.LP != undefined) {
            resultType.push("Professionals");
            resultData["Professionals"] = response.LP;
            size += response.LP.length;
        }
        if (response.APO != undefined) {
            resultType.push("Properties");
            resultData["Properties"] = response.APO;
            size += response.APO.length;
        }
        if (response.CAP != undefined) {
            resultType.push("Record");
            resultData["Record"] = response.CAP;
            size += response.CAP.length;
        }
        $scope.resultTypes = resultType;
        $scope.resultdetail = function (type) {
            return resultData[type];
        };
        $scope.resultSize = size;
    };

    // pagination Page 
    $scope.paginationPage = function (isRecursive, pageTurnCallback) {
        $http({
            method: "GET",
            url: servicePath + "api/GlobalSearch/globalSearch-page-count"
        }).success(function (response, status, headers, config) {
            var pageCount = response;
            //Recursive reset pageTotal
            if (isRecursive) {
                $scope.globalSearchDescription();
                $(".PagingMyRecords").pagination('updateItems', pageCount);
                return;
            }
            //PagingMyRecords
            $(".PagingMyRecords").pagination({
                items: pageCount,
                itemsOnPage: 1,
                displayedPages: 3,
                edges: 1,
                cssStyle: 'compact-theme',
                onPageClick: function (pageNumber, event) {
                    var promise = $scope.bindGlobalSearch(pageNumber - 1);
                    promise.then(function (data) {
                        var pageTotal = $(".PagingMyRecords").pagination('getPagesCount');
                        if (pageTotal == pageNumber) {
                            $scope.paginationPage(true);
                        }
                        if (pageTurnCallback != undefined && pageTurnCallback != "") {
                            pageTurnCallback();
                        }
                    }, function (data) {
                        ShowModalWindow($scope.HomeData.aca_newui_home_label_notice, data);
                    });
                }
            });
            // GET MyRecordsDescription
            $scope.globalSearchDescription();
        });
    };

    //bind GlobalSearch
    $scope.bindGlobalSearch = function (pageSize) {
        var deferred = null;
        if ($q != null) {
            deferred = $q.defer();
        }
        $http({
            method: "GET",
            params: { "currentPageIndex": pageSize },
            url: servicePath + "api/GlobalSearch/globalSearch-page-data"
        }).success(function (response, status, headers, config) {
            if (response.error != undefined) {
                ShowModalWindow($scope.HomeData.aca_newui_home_label_error, response.error);
            } else {
                $scope.globalSearchFillJsonToData(response);
            }

            if ($q != null) {
                deferred.resolve(response);
            }
        }).error(function (data, status, headers, config) {

            if ($q != null) {
                deferred.reject(data.Message);
            }
        });

        if ($q != null) {
            return deferred.promise;
        } else {
            return null;
        }
    };

    //Get GlobalSearch Description
    $scope.globalSearchDescription = function () {
        $http({
            method: "GET",
            url: servicePath + "api/GlobalSearch/globalSearch-description"
        }).success(function (response, status, headers, config) {
            var desc = response.descriptions;
            var size = response.resultSize;
            $scope.globalSearchDesc = desc;
            $scope.globalSearchSize = size;

        });
    };

    // Filter Global Search View
    $scope.FilterGlobalSearchView = function (type) {
        var dataUrl = $scope.GlobalSearchParameter;
        if ($scope.GlobalSearchParameterTypes.indexOf(type) > -1) {
            dataUrl.types = type =='' ? $scope.GlobalSearchParameterTypes : type;
            dataUrl.isFilter = true;
            $scope.synchronousQueryGlobalSearch(dataUrl, false);
        }
    };

    // Sort Global Search View
    $scope.SortGlobalSearchView = function (sort) {
        var dataUrl = $scope.GlobalSearchParameter;
        if (dataUrl.sort == sort && dataUrl.isAsc == true) {
            dataUrl.isAsc = false;
        } else {
            dataUrl.isAsc = true;
        }
        dataUrl.sort = sort;

        $scope.synchronousQueryGlobalSearch( dataUrl, false);
    };

    //Cache Global Search  query command
    $scope.cacheGlobalSearchBroadcastData = function (queryParameter) {
        var urlDate = queryParameter;
        if (urlDate.queryText == "" || urlDate.types == "") return;
        $scope.GlobalSearchParameter = urlDate;
        $scope.GlobalSearchParameterTypes = urlDate.types;
    };

    //locate records by checkbox
    $scope.multipleLocateOnMapService = function (apoData, capData) {
 
            //APO
            var properties = "";
            var propertiesData = apoData || new Array();
            var addressNumber = "";
            var parcelNumbers = "";

            for (var item in propertiesData) {
                if (typeof (propertiesData[item].ParcelNumber) == "undefined" ||
                    propertiesData[item].ParcelNumber == "" ||
                    propertiesData[item].ParcelNumber == null) {
                    if (typeof (propertiesData[item].AddressSourceNumber) != "undefined" &&
                                 propertiesData[item].AddressSourceNumber != "" &&
                                 propertiesData[item].AddressSeqNumber != "undefined" &&
                                 propertiesData[item].AddressSeqNumber != "") {
                        addressNumber += propertiesData[item].AddressSourceNumber + "&" + propertiesData[item].AddressDescription + "|";
                    }
                }

                parcelNumbers += propertiesData[item].ParcelNumber + ",";
            }

            //Record
            var capIds = "";
            var recordData = capData || new Array();

            for (var itemData in recordData) {
                capIds += recordData[itemData].CapID + ",";
            }

            if (addressNumber == "" && parcelNumbers == "" && capIds == "") {
                return;
            }
            $http({
                method: "GET",
                params: { "addressNumbers": addressNumber, "parcelNumbers": parcelNumbers, "capIds": capIds },
                url: servicePath + "api/Map/ApoOrCapInfo"
            }).success(function (response, status, headers, config) {
                var locateInfo = JSON.stringify(response);
                MapService.locate(locateInfo);

            }).error(function (data, status, headers, config) {
                ShowModalWindow($scope.HomeData.aca_newui_home_label_error, data.Message);
            });
        
    };

    //Get user information
    $scope.GetuserInfo = function () {
        $scope.GetReports();
        $scope.GetAnnouncements();
        $scope.GetAllShoppingCart();
    };
    //GetReports
    $scope.GetReports = function () {
        $http({
            method: "GET",
            url: servicePath + "api/Home/Reports"
        }).success(function (response, status, headers, config) {
            if (response) {
                $scope.Reports = response;
            }
        });
    };

    //get Announcements
    $scope.GetAnnouncements = function () {
        $http({
            method: "GET",
            url: servicePath + "api/Home/Announcements"
        }).success(function (response, status, headers, config) {
            $scope.Announcements = response.annList.result;
            $scope.isShowAnn = response.isShowAnn;
        });
    };

    //GetAllShoppingCart
    $scope.GetAllShoppingCart = function () {
        $http({
            method: "GET",
            url: servicePath + "api/Home/ShoppingCarts"
        }).success(function (response, status, headers, config) {
            if (response) {
                $scope.ShoppingCart = response.cartList;
                $scope.isShowCart = response.isShowCart;
                $scope.total = 0;
                for (var i = 0; i < $scope.ShoppingCart.length; i++) {
                    $scope.total += $scope.ShoppingCart[i].totalFee;
                    $scope.ShoppingCart[i].totalFee = $scope.changeTwoDecimal_f($scope.ShoppingCart[i].totalFee);
                }
                $scope.total = $scope.changeTwoDecimal_f($scope.total);
            }
        });
    };

    //two decimal point behind
    $scope.changeTwoDecimal_f = function(floatvar) {
        var f_x = parseFloat(floatvar);
        var f_x = Math.round(f_x * 100) / 100;
        var s_x = f_x.toString();
        var pos_decimal = s_x.indexOf('.');
        if (pos_decimal < 0) {
            pos_decimal = s_x.length;
            s_x += '.';
        }
        while (s_x.length <= pos_decimal + 2) {
            s_x += '0';
        }
        return s_x;
    };

    //get login user
    $scope.isLogin = function () {       
        if (getSessionStorage("userInformation") == null) {
            $scope.userInfo = {
                "isLoggedIn": false,
                "firstName": "",
                "lastName": ""
            };
        } else {
            $scope.userInfo = JSON.parse(getSessionStorage("userInformation"));
        }

        if (!$scope.userInfo.isLoggedIn) {
             $scope.GetReports();
             $scope.GetAnnouncements();
        }
    };

    // if loginDropMenu greater than 450 Show SlimScroll
    $scope.slimScroll = function() {
        if ($("#loginDropMenu").height() > 450) {        
        $("#loginDropMenu").slimScroll({
            height: 450 + "px",
            wheelstep: 100
        });
       }
    };

    //show menu after
    $scope.initProfileDropdown = function (elementID) {
        $('#' + elementID).on('shown.bs.dropdown', function () {
            $scope.slimScroll();
        });
    };

    //show Announcement
    $scope.ShowAnnouncement = function (AnnouncementTitle, AnnouncementText) {
        ShowModalWindow(AnnouncementTitle, AnnouncementText);
    };

    //Reports url
    $scope.ReportUrl = function (Report) {
        var url = servicePath + "Report/ReportParameter.aspx?module=" + "" + "&reportID=" + Report.reportId + "&reportType=" + Report.buttonName;
        window.open(url, 'popwindow', 'height=600,width=750,top=200,left=300,toolbar=no, menubar=no, scrollbars=no, resizable=no,location=no,status=no');
    };

    //ShoppingCart url
    $scope.ShoppingCartUrl = function () {
        var url = "ShoppingCart/ShoppingCart.aspx?TabName=Home&stepNumber=2&isFromNewUi=Y";
        aca.data.isRefreshShoppingCart = true;
        setIframeSrc(url);
    };

    //Get official WebSite
    $scope.getOfficialWebSite = function () {
        $http({
            method: "GET",
            url: servicePath + "api/Home/officialWebSite"
        }).success(function(response) {
            $scope.officialWebSite = response.officialWebSite;

            if (isAdmin) {
                $scope.officialWebSiteHref = $sce.trustAsJs('javascript:void(0);');
            } else {
                $scope.officialWebSiteHref = $sce.trustAsUrl(response.officialWebSite);
            }
        });
    };

    //Get Agency Logo
    $scope.getAgencyLogo = function () {
        $http({
            method: "GET",
            url: servicePath + "api/Home/agencyLogo"
        }).success(function (response) {
            $scope.logoSource = response.logoSource;
        });
    };
    //TODO: service

    $scope.format = function() {
        try {
            var args = arguments;
            if (args.length == 0) {
                return '';
            }
	    
            var str = arguments[0];
            if (!isAdmin) {
                str = str.replace(/\{(\d+)\}/g, function(m, i, o, n) {
                    return args[++i];
                });
            }

            return str;
        } catch (e) {
            return arguments[0];
        }
    };

    $scope.ShowCollectionByLogin = function () {
        var isLogin = false;
        var userInformation = sessionStorage.getItem("userInformation");

        if (userInformation) {
            isLogin = Boolean(JSON.parse(userInformation).isLoggedIn);
        }

        return isLogin;
    };

    //SignOut
    $scope.SignOut = function () {
        $http({
            method: "GET",
            url: servicePath + "api/publicuser/sign-out"
        }).success(function (response) {
            removeSessionStorage("userInformation");
            save2SessionStorage("userInformation", JSON.stringify(anonymousUser));
            $window.location.href = servicePath;
        });
    };

    $scope.unescape = function (value) {
        return unescape(value);
    };

    //get all Collcetion items
    $scope.getCollcetion = function () {
        $http({
            method: "GET",
            url: servicePath + "api/ActionButton/GetAllCollcetion"
        }).success(function (response) {
            $scope.Collecions = response;
            $scope.isShowViewCollcetion = $scope.Collecions != "" ? true : false;
        });
    };

    //add new collection
    $scope.addToCollcetion = function () {
        var collcetionName="";
        var descriptionText="";
        var collectionId;
        var cap = $('#textBoxModal').data('caps');
        var caps = [];
        if ($("input[name='rdocollectionbtn']").get(0).checked) {
            collectionId = $("#collectiondropdown").val();
        } else {
            collcetionName = $("#nameText").val();
            descriptionText = $("#descriptionText").val();
        }

        for (var capItem in cap) {
            var collectionModel = new Object();
            collectionModel.CollcetionName = collcetionName;
            collectionModel.CollectionDescription = descriptionText;
            collectionModel.CollectionId = collectionId;
            collectionModel.Capid = capItem;
            collectionModel.CapClass = cap[capItem];
            caps.push(collectionModel);
        }    

        $http({
            method: "POST",
            data: caps,
            url: servicePath + "api/ActionButton/AddToCollection"
        }).success(function (response, status, headers, config) {
            $scope.notice = $scope.HomeData.aca_newui_home_label_notice;
            if (response.isSuccessful) {
                document.getElementById("btnModalCancel").click();
                ShowModalWindow($scope.notice,response.message);
                $scope.Collecions = response.collectionList;
                $scope.isShowViewCollcetion = true;
            } else {
                $("#prompt")[0].innerHTML = response.message;
                $("#prompt").addClass("msg-text");
            }
        }).error(function (data) {
            $("#prompt")[0].innerHTML = data.ExceptionMessage;
            $("#prompt").addClass("msg-text");
        });
    };

    //collection record increase inside
    $scope.addCollcetionCapAmount = function (collectionId) {
        $($scope.Collecions).each(function () {
            if (this.collectionId == collectionId) {
                this.capAmount++;
            }
        });
    };
});
