/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: DashboardController.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: DashboardController.js 72643 2014-06-19 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/18/2008     		Kevin.Feng				Initial.  
 * </pre>
 */
var alonePopovers = ["linkActionsRequiredPopover", "linkdivRecentActivitypopover", "likeCollectionPopover"];

app.controller('DashboardCtrl', function ($scope, $http, $window, $compile, $location, LabelKeyService) {
    $scope.route = LabelKeyService.routeName.Dashboard;
    $scope.ShowLanguage(false);

    $scope.DashPageInit = function () {
        //get label key
        if (LabelKeyService.getLabelKey($scope.route, $scope.selectedLanguage)) {
            $scope.DashboardData = LabelKeyService.DataKeys;
        }

        $scope.GetLoginInfo();

        $scope.isEnabledAccountManagement = $scope.isEnabledAccountManagement();
    };

    //bind login info and collection info
    $scope.GetLoginInfo = function () {
        var userInfo = JSON.parse(getSessionStorage("userInformation"));

        $http({
            method: "GET",
            params: {},
            url: servicePath + "api/publicuser/login-info"
        })
       .success(function (response, status, headers, config) {
           var obj = response;
           var loginInfo = obj.LoginInfo; //LoginInfo
           $scope.shortName = loginInfo[0].shorthandName;
           $.each(loginInfo, function (i, item) {
               $scope.loginName = item.name;
               $scope.collectionsCount = item.CollectionsCount;
           });
       })
       .error(function (response, status, headers, config) {
           ShowModalWindow($scope.HomeData.aca_newui_home_label_error, response.Message);
       });
    };

    ////Edit Profile Event
    $scope.showEditProfile = function () {
        var url = "Account/AccountManager.aspx";
        setIframeSrc(url);
    };

    $scope.showCollectionDetail = function (id) {
        var url = "MyCollection/MyCollectionDetail.aspx?collectionId=" + id;
        setIframeSrc(url);
        $scope.isRefreshCollcetion = true;
    };

    //Link to MyCollections
    $scope.showAllCollections = function () {
        var url = "MyCollection/MyCollectionManagement.aspx";
        setIframeSrc(url);
        $scope.isRefreshCollcetion = true;
    };


    $scope.owlCarousel = function() {
        var owlObj = $("#dash-collection");
        owlObj.find('.owl-controls,.owl-buttons,.owl-wrapper-outer').remove();

        var h = '<div id="viewAll" class="item" ng-show="isShowViewCollcetion" ng-click="showAllCollections()">' +
            '<span class="view-all-coll-text">View all Collections</span>' +
            '<h3><span class="ico-stack-list view-all-coll"></span> </h3>' +
            '</div>';
        var template = angular.element(h);
        var linkFn = $compile(template);
        linkFn($scope);
        owlObj.append(template);

        var owlOptions = {
            navigationText: ["", ""],
            itemsCustom: [
                [0, 3],
                [450, 4],
                [600, 4],
                [700, 7],
                [1000, 5],
                [1200, 5],
                [1400, 5],
                [1600, 5]
            ],
            navigation: true
        };
        owlObj.find('.owl-controls,.owl-buttons,.owl-wrapper-outer').remove();
        owlObj.data("owl-init", "false");
        owlObj.owlCarousel(owlOptions);
    };

    $scope.closeIframe = function () {
        closeHomeIframe();
        if ($scope.isRefreshCollcetion) {
            $scope.getCollcetion();
            $scope.isRefreshCollcetion = false;
        }

        if (aca.data.isRefreshShoppingCart) {
            $scope.GetAllShoppingCart();
            aca.data.isRefreshShoppingCart = false;
        }
    };

    $scope.isEnabledAccountManagement = function() {
        if (isAdmin) {
            return true;
        }

        var isEnabledAccountManagement = false;

        $.ajax({
            type: 'get',
            url: servicePath + 'api/actioncenter/accountManagementFlag',
            async: false,
            cache: false,
            success: function(r) {
                isEnabledAccountManagement = IsTrue(r);
            }
        });

        return isEnabledAccountManagement;
    };

    //actionRequired Refresh
    $scope.$on('actionRequired-parent', function (d, data) {
         if (data == "MyRecordRefresh") {
             $scope.$broadcast('dashBoard-child', 'MyRecordRefresh');
         }
    });
});
