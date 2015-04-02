/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AngularBindHtml.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: AngularBindHtml.js 72643 2014-09-05 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
app.directive('ngLabelkey', function($parse, $compile) {
    return function($scope, $element, $attrs) {
        $scope.$watch($attrs.ngLabelkey, function(newVal, oldVal) {
            try {
                var value = decodeHTMLTag(newVal);

                if ($attrs.controlType == 'link') {
                    // links in UI, not show the 'link1','link2','link3'
                    if (!isAdmin && value) {
                        var vtrim = trimHtmlTag(value).trim().toLowerCase();

                        if (vtrim == "link1" || vtrim == "link2" || vtrim == "link3") {
                            value = '';
                        }
                    }
                }

                $element.html(value);
            } catch (err) {
                $element.html('');
            }
        });
    };
});