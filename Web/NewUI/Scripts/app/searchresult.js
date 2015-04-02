/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: searchresult.js
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

//Common function available for each page
//Make all page realted on load call in this 'aca.pageInit' function 

aca.pageInit = function() {
    aca.ui.initMapTabHeight();
    aca.ui.initScrollBar();
};

aca.ui = {
    searchMapHeight: function() {
        if (window.location.hash == "#/SearchView") {
            return iframeMinHeight - 65;
        } else {
            return iframeMinHeight-10;
        }
    } ,
    isMapLoaded: false,
    initScrollBar: function () {
        $('.SearchList-Item-Content').slimScroll({
            height: ($(window).height() - 280)+ "px",
            wheelStep: 5,
        });

        $('#List-View-Row').slimScroll({
            height: aca.ui.searchMapHeight() + "px",
            wheelStep: 5,
        });

    },
    initDetailedPanelScrollBar: function () {
        $('#SearchList-Item-Detail-Content').slimScroll({
            height: ($(window).height() - 280) + "px",
            wheelStep: 5,
        });

        $('#SearchList-Item-Detail-Content').css("width", "100%");
    },
    destroyDetailedPanelScrollBar: function () {
        this.destroySlimscroll('SearchList-Item-Detail-Content');
    },
    destroySlimscroll: function (objectId) {
        $("#" + objectId).parent().replaceWith($("#" + objectId));
    },
    reInitSlimScroller: function () {

        $('.SearchList-Item-Content, #SearchList-Item-Detail-Content').css("height", ($(window).height() - 280) + "px")
            .parent().css("height", ($(window).height() - 280) + "px");

        $('#List-View-Row').css("height", aca.ui.searchMapHeight() + "px")
           .parent().css("height",aca.ui.searchMapHeight() + "px");
    },
    initMapTabHeight: function () {
      $("#SearchListInMap, #SearchListInMap-SideBar").css("min-height", aca.ui.searchMapHeight() + "px");
    },
    toggleMapControls: function (left) {
        if (left)
            $(".accelajs-widgets").animate({ right: "365" }, 300);
        else
            $(".accelajs-widgets").css("right", "30px");

    },
    toggleSlidePanel: function () {
        if ($("#SearchListInMap")[0].style.display == "none") {
            aca.ui.showSlidePanel();
        }
        else
        {
            aca.ui.hideSlidePanel();
        }
    },
    showSlidePanel:function()
    {
        this.toggleMapControls(true);
        $("#SearchListInMap-SideBar").addClass("animation animating slideOutRight")
          .one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend",
          function () {
              $(this).css("display", "none").removeClass("animation animating slideOutRight");
          });

        $("#SearchListInMap").css("display", "block").addClass("animation animating slideInRight")
              .one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend",
              function () {
                  $(this).removeClass("animation animating slideInRight");
              });
    },
    hideSlidePanel: function () {
        this.toggleMapControls(false);
        $("#SearchListInMap").addClass("animation animating slideOutRight")
           .one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend",
           function () {
               $(this).css("display", "none").removeClass("animation animating slideOutRight");
           });

        $("#SearchListInMap-SideBar").css("display", "block").addClass("animation animating slideInRight")
              .one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend",
              function () {
                  $(this).removeClass("animation animating slideInRight");
                  $(this).css("display", "block");
              });
    },
    toggleSearchResult: function() {
        var isShow = false;
        isShow = $("#SearchList-Item-Detail")[0].style.display == "none";

        if (isShow) {
            aca.ui.showSearchResultDetail();

        } else {
            aca.ui.showSearchResult();
        }
    },
    showSearchResultDetail: function () {
        $("#SearchList-Item").hide();
        $("#SearchList-Item-Detail").show();
        aca.ui.initDetailedPanelScrollBar();

        //Show SearchList-Item-Detail
        $("#SearchList-Item-Detail").addClass("animation animating fadeInRight")
            .one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend",
                function () {
                    $(this).removeClass("animation animating fadeInRight");
                });
    },
    showSearchResult: function () {
        $("#SearchList-Item-Detail").hide();
        $("#SearchList-Item").show();
        //aca.ui.destroyDetailedPanelScrollBar();

        $("#SearchList-Item").addClass("animation animating fadeInLeft")
            .one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend",
                function () {
                    $(this).removeClass("animation animating fadeInLeft");

                });
    }
};

$(window).resize(function () {
    aca.ui.initMapTabHeight();
    aca.ui.reInitSlimScroller();
});

$(document).on('click', '.record-back', function (e) {
    e.preventDefault();
    aca.ui.showSearchResult();
});


$(document).on('click', '.record-minimize', function (e) {
    e.preventDefault();
    aca.ui.hideSlidePanel();
});

$(document).on('click', '.maplist-sidebar', function (e) {
    e.preventDefault();
    aca.ui.showSlidePanel();
});

$(document).on('shown.bs.tab', 'a[data-toggle="tab"]', function (e) {
    if ($(this).attr("href") == "#map-view" && aca.ui.isMapLoaded == false) {
        aca.ui.isMapLoaded = true;
        aca.initMaps();
    }
})


aca.initMaps = function () {
    aca.ui.initMapTabHeight();
    var timer;
    timer = setInterval(function () {
        if ($(".accelajs-widgets").exists()) {
            clearTimeout(timer);
            aca.ui.toggleMapControls(true);
        }
    }, 1000);
      
    function json2str(o) {
        var arr = [];
        var fmt = function(s) {
            if (typeof s == 'object' && s != null) return json2str(s);
            return /^(string|number)$/.test(typeof s) ? "'" + s + "'" : s;
        }
        for (var i in o) arr.push("'" + i + "':" + fmt(o[i]));
        return '{' + arr.join(',') + '}';
    }

};
