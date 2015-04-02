/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: Angulariframe.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: Angulariframe.js 72643 2008-04-24 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/18/2008     		Kevin.Feng				Initial.  
 * </pre>
 */
app.directive('angulariframe', function factory() {
    return {
        priority: 100,
        template: [
        '<div class="container-fluid" id="IframeControl" style="display:none;">' +
            '<div class="frame-screen">' +
                '<button class="btn btn-default btnColse" id="iframeClose" ng-click="closeIframe()"  ng-labelkey="HomeData.aca_newui_home_label_close"></button>' +
                '<iframe width="100%" class="alliframe" id="capIfram" scrolling="yes" name="capIfram" onload="javascript: loadedIframe(this);" src=""></iframe>' +
            '</div>' +
        '</div>'].join(""),
        replace: true,
        transclude: true,
        restrict: 'E'
    };
});