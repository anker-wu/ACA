/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: App.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: App.js 72643 2008-04-24 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/18/2008     		Kevin.Feng				Initial.  
 * </pre>
 */
var app = angular.module('AcaFaceLiftApp', ['ngRoute', 'ngSanitize'], function ($compileProvider) {
    // when set an angularjs variable to tag A's href, filter the prefix 'unsafe:'.
    $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|file|javascript):/);
});

app.config(['$httpProvider' , function ($httpProvider) {

   $httpProvider.interceptors.push('ACAFaceliftHttpInterceptor');
}]);

app.factory('ACAFaceliftHttpInterceptor', function ($q) {
    return {
        request: function(config) {
                aca.util.loadingMask.show();
            return config || $q.when(config);
        },
        requestError: function (rejection) {
            aca.util.loadingMask.hide();
            return $q.reject(rejection);
        },
        response: function (response) {
            aca.util.loadingMask.hide();
            return response || $q.when(response);
        },
        responseError: function (rejection) {
            aca.util.loadingMask.hide();
            return $q.reject(rejection);
        }
    };
});
 