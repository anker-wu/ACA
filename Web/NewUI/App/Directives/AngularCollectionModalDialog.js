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
app.directive('angularcollectionmodaldialog', function factory() {
    return {
        priority: 100,
        template: [
            '<div class="modal fade" id="textBoxModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class="modal-dialog collcetion-dialog">' +
            '<div class="modal-content">' +
            '<div class="modal-header">' +
            '<button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>' +
            '<h4 class="modal-title"><span></span></h4>  </div>' +
            '<div id="prompt"></div>' +
            '<div class="modal-body collcetion-body"> ' +
            '<div id="addCollcetion">' +
            '<div ><div><input type="radio" name="rdocollectionbtn" id="rdoExistCollection"  onclick="rdoCollection()" checked />' +
            '<span class="collcetion-text collection-title" ng-labelkey="HomeData.aca_newui_home_label_existingcollection"></span></div>' +
            '<div><select id="collectiondropdown" onChange="MyCollectionChanged()"><option selected="selected">--Select--</option><option ng-repeat="item in Collecions | orderBy:item" value="{{item.collectionId}}">{{item.collectionName}}</option></select></div>' +
            '<div><input type="radio" name="rdocollectionbtn" id="rdoNewCollection" onclick="rdoCollection()" />' +
            '<span class="collcetion-text  collection-title" ng-labelkey="HomeData.aca_newui_home_label_createcollection"></span></div></div>' +
            '<span class="collcetion_Indicator">*</span><span class="collcetion-text"  ng-labelkey="HomeData.aca_newui_home_label_collectionname"></span><br/>' +
            '<input type="text" id="nameText" oninput="collectionNameChanged()"/><br/>' +
            '<span class="collcetion-text" ng-labelkey="HomeData.aca_newui_home_label_collectiondescription"></span><br/>' +
            '<textarea rows="2" cols="20" id="descriptionText"></textarea><br/>' +
            '<div class="collcetion-btn"><button disabled="disabled"  class="disabled-button" id="addCollcetionBtn" ng-click="addToCollcetion()"  ng-labelkey="HomeData.aca_newui_home_label_addcollectionbtn"></button>' +
            '<button id="btnModalCancel"  class= "collcetion-cancelbtn " data-dismiss="modal"  ng-labelkey="HomeData.aca_newui_home_label_cancelcollectionbtn"></button></div>' +
            '</div>' +
            ' </div> </div></div></div>'
            ].join(""),
        replace: true,
        transclude: true,
        restrict: 'E'
    };
});

