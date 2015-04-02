/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: LoginController.js
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
 *  03/18/2008     		dennis.fu				Initial.  
 * </pre>
 */
app.controller('loginCtrl', function ($scope, $window, $http, $location, LabelKeyService) {
    var enableRecaptchaForLogin;
    $scope.route = LabelKeyService.routeName.Login;
    $scope.ShowLanguage(false);

    //init page
    $scope.init = function () {
        //get label key
        if (LabelKeyService.getLabelKey($scope.route, $scope.selectedLanguage)) {
            $scope.LoginData = LabelKeyService.DataKeys;
        }

        $scope.GetRecaptchaSetting();

        var userInfo = getSessionStorage("userInformation");

        if (userInfo && JSON.parse(userInfo).isLoggedIn) {
            // has logined, redirect to home page.
            //$window.location.href = window.location.href.split("#")[0] + "#/";
            $location.$$search = undefined;
            $location.path("/");
        } else {
            var cookie = getCookie("REMEMBERED_USER_NAME");
            if (cookie) {
                $("#txtLogin").val(cookie);
                $("#txtPassword").focus();
                $("#rememberme").prop("checked", "checked");
            } else {
                $("#txtLogin").focus();
            }

            // Enable Recaptcha For Login.
            $http({
                method: "Get",
                url: servicePath + "api/publicuser/EnableRecaptchaForLogin"
            }).success(function (response, status, headers, config) {
                enableRecaptchaForLogin = response;
                if (response == "true") {
                    $scope.recaptcha = false;
                    $.ajax({
                        method: "get",
                        url: "//www.google.com/recaptcha/api/js/recaptcha_ajax.js",
                        dataType: "script",
                        success: function () {
                            Recaptcha.create($scope.recaptchaPublicKey, "recaptcha", {
                                theme: "custom",
                                custom_theme_widget: 'recaptcha_widget',
                                tabIndex: 0
                            });
                        },
                        error: function () {
                            console.log("recaptcha error!");
                        }
                    });
                } else {
                    $scope.recaptcha = true;
                }
            }).error(function(response, status, headers, config) {
                $("#txtLoginMsg").html($scope.LoginData.aca_newui_login_label_recaptchaerror + response);
            });
        }
    };

    //init Recaptcha setting
    $scope.GetRecaptchaSetting = function() {
        $.ajax({
            type: 'GET',
            url: servicePath + "api/Publicuser/Recaptcha-Setting",
            async: false,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            data: "",
            success: function (response) {
                $scope.recaptchaPublicKey = response.recaptchaPublicKey;
                $scope.rcaptchaPrivateKey = response.rcaptchaPrivateKey;
            },
            error: function (response) {
                ShowModalWindow($scope.HomeData.aca_newui_home_label_error, response.Message);
            }
        });
    };

    $scope.onLogin = function() {
        var user = {
            Name: $("#txtLogin").val(),
            Pwd: $("#txtPassword").val(),
            IsRemember: $("#rememberme").prop("checked") ? 1 : 0
        };

        if ($scope.validate(user)) {
            if (enableRecaptchaForLogin == "true") {
                $http({
                    method: 'GET',
                    url: servicePath + 'api/publicuser/verify-recaptcha',
                    params: {
                        postUrl: "http://www.google.com/recaptcha/api/verify",
                        postData: "privatekey=" + $scope.rcaptchaPrivateKey + "&" +
                            "challenge=" + document.getElementById("recaptcha_challenge_field").value + "&" +
                            "response=" + document.getElementById("recaptcha_response_field").value
                    }
                }).success(function(response, status, headers, config) {
                    if (response.result == "true") {
                        $scope.submit(user);
                    } else {
                        $("#txtLoginMsg").html($scope.LoginData.aca_newui_login_msg_validate);
                    }
                }).error(function(response, status, headers, config) {
                    $("#txtLoginMsg").html($scope.HomeData.aca_newui_home_label_error);
                });
            } else {
                $scope.submit(user);
            }
        }
    };

    $scope.validate = function (user) {
        var errMsg = '';
        // user name.
        if ($.trim(user.Name).length == 0) {
            errMsg = $scope.LoginData.aca_newui_login_msg_usermsg;
        }
        // password.
        if ($.trim(user.Pwd).length == 0) {
            if (errMsg) {
                errMsg += '<br />';
            }

            errMsg += $scope.LoginData.aca_newui_login_msg_passwordmsg;
        }

        if (errMsg) {
            $("#txtLoginMsg").html(errMsg);
            return false;
        }

        return true;
    };

    $scope.submit = function(user) {
        $http({
                method: 'POST',
                url: servicePath + 'api/publicuser/signin',
                data: user
            })
            .success(function (response, status, headers, config) {
                var r = response;

                if (r.type == "success") {
                    if (user.IsRemember == 1) {
                        save2Cookie("REMEMBERED_USER_NAME", user.Name);
                    } else {
                        removeCookie("REMEMBERED_USER_NAME");
                    }

                    save2SessionStorage("userInformation", JSON.stringify({
                        "isLoggedIn": true,
                        "firstName": r.firstName ? r.firstName.substr(0, 1) : "",
                        "lastName": r.lastName ? r.lastName.substr(0, 1) : ""
                    }));

                    $scope.isLogin();
                    $scope.loginClose();
                } else if (r.type == "redirect") {
                    setIframeSrc(r.url);
                } else {
                    $("#txtLoginMsg").html(r.message);
                    return false;
                }
            })
            .error(function(response, status, headers, config) {
                $("#txtLoginMsg").html($scope.LoginData.aca_newui_login_msg_errorlogin + status);
            });
    };

    // forgot password.
    $scope.getBackPwd = function() {
        setIframeSrc("Account/ForgotPassword.aspx?isFromNewUi=Y");
    };

    // new user.
    $scope.register = function() {
        setIframeSrc("Account/RegisterDisclaimer.aspx?isFromNewUi=Y");
    };

    $scope.loginClose = function () {
        save2SessionStorage("noNeedLoadCurrentUrl", true);
        $window.location.href = window.location.href.split("#")[0];
    };

    $scope.rememberMe = function () {
        var remember = $("#rememberme");
        if (remember != "undefined") {
            if (remember.prop("checked")) {
                remember.prop("checked", false);
            } else {
                remember.prop("checked", true); 
            }
        }
    };
});

