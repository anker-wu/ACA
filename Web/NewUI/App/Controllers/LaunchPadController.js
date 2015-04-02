/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: LaunchPadController.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: LaunchPadController.js 72643 2014-06-19 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/18/2008     		Kevin.Feng				Initial.  
 * </pre>
 */
app.controller('LaunchPadCtrl', function ($scope, $http, $compile, $window, $location, $routeParams, GlobalSearchParameterService, LabelKeyService) {
    $scope.route =  LabelKeyService.routeName.LaunchPad;

    $scope.init = function () {
        //get label key
        if (LabelKeyService.getLabelKey($scope.route, $scope.selectedLanguage)) {
            $scope.LaunchPadData = LabelKeyService.DataKeys;
        }

        if (typeof ($routeParams.IsFromMap) == "undefined") {
            aca.data.IsFromMap = false;
            aca.data.mapJsonData = "";
        } else {
            try {

                aca.data.IsFromMap = true;
                // if this page come from map then show the module by commond
                if (aca.data.mapJsonData == "") {
                    return;
                }

                var mapjson = JSON.parse(aca.data.mapJsonData);
                var commond = mapjson.command;

                aca.launchPad.LoadControl(aca.constant.filterLevel.Base);

                if (commond == aca.data.commond.creat) //Apply
                {
                    aca.launchPad.LoadControl(aca.constant.filterLevel.Module);
                } else if (commond == aca.data.commond.request) //Request
                {
                    aca.launchPad.LoadControl(aca.constant.filterLevel.GroupType);
                }
            } catch (e) {
            }
        }

        //TODO Split address description into some fields for example Address No.
        if (typeof (GlobalSearchParameterService.value) != "undefined"
            && typeof (GlobalSearchParameterService.value.applyFromGS) != "undefined"
            && typeof (GlobalSearchParameterService.value.ParcelNumber) != "undefined"
            && typeof (GlobalSearchParameterService.value.AddressDescription) != "undefined") {
            aca.data.isActionBtnApply = "Y";
            aca.data.ParcelNumber = GlobalSearchParameterService.value.ParcelNumber;
            aca.data.AddressDescription = GlobalSearchParameterService.value.AddressDescription;
            aca.launchPad.LoadControl(aca.constant.filterLevel.Base);
            aca.launchPad.LoadControl(aca.constant.filterLevel.Module, GlobalSearchParameterService.value.applyFromGS);
        } else {
            aca.data.isActionBtnApply = "";
            aca.data.ParcelNumber = "";
            aca.data.AddressDescription = "";
        }

        $scope.initCurrentUrl();
        $scope.isEnabledRegister();
        $scope.isEnabledLogin();
    };

    $scope.closeIframe = function () {
        closeHomeIframe();
        
        if (aca.data.isRefreshShoppingCart) {
            $scope.GetAllShoppingCart();
            aca.data.isRefreshShoppingCart = false;
        }
    };

    $("input[name=layoutoptions]:radio").change(function (e) {
        aca.launchPad.changeLayout($(this).val());
    });

    $scope.compiledTemp = function ($scope, moduleHtml) {
        // Step 1: parse HTML into DOM element
        var template = angular.element(moduleHtml);
        // Step 2: compile the template
        var linkFn = $compile(template);
        // Step 3: link the compiled template with the scope.
        linkFn($scope);
        return template;
    };

    //Initialize current url.
    $scope.initCurrentUrl = function () {
        $http({
            method: "GET",
            url: servicePath + "api/actioncenter/CurrentUrl"
        }).success(function (response) {
            if (response && !getSessionStorage("noNeedLoadCurrentUrl")) {
                setIframeSrc(response);
            } else {
                removeSessionStorage("noNeedLoadCurrentUrl");
            }
        });
    };

    $scope.isEnabledRegister = function () {
        if (isAdmin) {
            return true;
        }

        var isEnabledRegister = false;

        $.ajax({
            type: 'get',
            url: servicePath + 'api/actioncenter/registFlag',
            async: false,
            cache: false,
            success: function(r) {
                isEnabledRegister = IsTrue(r);
            }
        });

        if (isEnabledRegister) {
            $(".divRegister").css("display", "block");
        } else {
            $(".divRegister").css("display", "none");
        }
    };

    $scope.isEnabledLogin = function () {
        if (isAdmin) {
            return true;
        }

        var isEnabledLogin = false;

        $.ajax({
            type: 'get',
            url: servicePath + 'api/actioncenter/loginFlag',
            async: false,
            cache: false,
            success: function (r) {
                isEnabledLogin = IsTrue(r);
            }
        });

        if (isEnabledLogin) {
            $(".divLogin").css("display", "block");
            $(".login-section").css("display", "block");
        } else {
            $(".divLogin").css("display", "none");
            $(".login-section").css("display", "none");
        }
    };
});