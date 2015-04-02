/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: Routings.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: menu.js 72643 2008-04-24 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/18/2008     		Kevin.Feng				Initial.  
 * </pre>
 */

// if the user should login then "T" , otherwise "F"
var routeObj = {
    "T": [
            "/Dashboard"
    ]
};

app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
         .when("/", {
             templateUrl: 'LaunchPad.html',
             controller: "LaunchPadCtrl"
         })
        .when("/Dashboard", {
            templateUrl: 'Dashboard.html',
            controller: "DashboardCtrl"
        })
        .when("/MapView", {
            templateUrl: 'MapView.html',
            controller: "MapViewCtrl"
        })
        .when("/Login", {
            templateUrl: 'Login.html',
            controller: "loginCtrl"
        })
        .when("/SearchView", {
            templateUrl: 'SearchView.html',
            controller: "SearchCtrl"
        })
        .otherwise({
            redirectTo: "/"
        });
}]);

app.run(['$rootScope', '$window', '$location', '$log', function ($rootScope, $window, $location, $log) {
    var locationChangeStartOff = $rootScope.$on('$locationChangeStart', locationChangeStart);
    var locationChangeSuccessOff = $rootScope.$on('$locationChangeSuccess', locationChangeSuccess);

    var routeChangeStartOff = $rootScope.$on('$routeChangeStart', routeChangeStart);
    var routeChangeSuccessOff = $rootScope.$on('$routeChangeSuccess', routeChangeSuccess);

    function locationChangeStart(event, newUrl) {
        var route = newUrl.split("#")[1];
        var routeLogin = routeObj.T;

        if (routeLogin.indexOf(route) > -1) {
            var userInfo = getSessionStorage("userInformation");
            if (userInfo && JSON.parse(userInfo).isLoggedIn) {
                $log.log('locationChangeStart');
                //$log.log(arguments);
            } else {
                $window.location.href = window.location.href.split("#")[0] + "#/";
            }
        }
    }

    function locationChangeSuccess(event) {
        //$log.log('locationChangeSuccess');
        //$log.log(arguments);
    }

    function routeChangeStart(event) {
        //$log.log('routeChangeStart');
        //$log.log(arguments);
    }

    function routeChangeSuccess(event) {
        //$log.log('routeChangeSuccess');
        //$log.log(arguments);
    }
}]);