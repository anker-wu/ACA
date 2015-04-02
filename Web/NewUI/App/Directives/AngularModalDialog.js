/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AngulariModal.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: AngulariModal.js 72643 2008-04-24 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/18/2008     		Kevin.Feng				Initial.  
 * </pre>
 */
app.directive('angularmodaldialog', function factory() {
    return {
        priority: 100,
        template: [
            '<div class="modal fade" id="hintModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">'+
            '<div class="modal-dialog">' +
            '<div class="modal-content">' + 
            '<div class="modal-header">'+
            '<button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only" ng-labelkey="HomeData.aca_newui_home_label_close"></span></button>' +
            '<h4 class="modal-title">NOTICE</h4>  </div>'+
            '<div class="modal-body"> body  </div>   <div class="modal-footer">    <button type="button" class="btn btn-default" data-dismiss="modal" ng-labelkey="HomeData.aca_newui_home_label_close"></button>' +
            '</div></div></div></div>'
            ].join(""),
        replace: true,
        transclude: true,
        restrict: 'E'
    };
});