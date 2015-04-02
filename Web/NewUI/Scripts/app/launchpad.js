/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: launchpad.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: launchpad.js 72643 2014-06-19 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/18/2008     		Kevin.Feng				Initial.  
 * </pre>
 */
//Common function available for each page
//Make all page realted on load call in this 'aca.pageInit' function 

var CommonData = getLabelKeys("Home");
var LaunchPadData = getLabelKeys("LaunchPad");
aca.pageInit = function () {
    aca.launchPad.init();
};

aca.data.launchPadConfig = [];
var launchTitle = "";
var baseTitle = "";
if (CommonData!=undefined  && CommonData.aca_newui_home_label_launchpad) {
    launchTitle = CommonData.aca_newui_home_label_launchpad;
}

if (LaunchPadData != undefined && LaunchPadData.aca_newui_launchpad_label_start) {
    baseTitle = LaunchPadData.aca_newui_launchpad_label_start;
}

aca.data.launchPadConfig[1] = {
    "template": "launchPad-home-template",
    "isDynamicTitle": false,
    "title": launchTitle
};
aca.data.launchPadConfig[2] = {
    "template": "launchPad-base-template",
    "isDynamicTitle": false,
    "title": baseTitle
};
aca.data.launchPadConfig[3] = {
    "template": "dynamicButtons-template",
    "isDynamicTitle": true,
    "title": null
};

aca.constant.layout = {
    timemachine: 1,
    accordion: 2,
    box: 3
};

aca.constant.filterLevel = {
    None: 0,
    Home: 1,
    Base: 2,
    Module: 3,
    SubModule: 4,
    GroupType: 5,
    SubType: 6,
    CategoryOrAlias: 7
};

aca.launchPad = {
    step: 1,
    loadlevel: aca.constant.filterLevel.None,
    layoutType: aca.constant.layout.timemachine,
    reloadPanel: false,
    reloadStep: 1,
    itemsPerPage: 9,
    init: function () {

        this.layoutType = (aca.util.device.isPhone == true ? aca.constant.layout.box : aca.constant.layout.timemachine);
        //Load the Default Step 1 , Home Section     
        this.LoadControl(aca.constant.filterLevel.Home);
    },
    createPanel: function (stepSource) {

        //Paginate your Source

        //Load the Main content
        var contentSource = $("#" + stepSource.template).html();
        var contentTemplate = Handlebars.compile(contentSource);
        var contentHtml = contentTemplate(stepSource);

        //Load the Panel Template
        var source = $("#panel-template").html();
        var template = Handlebars.compile(source);
        var html = template(stepSource);

        if (this.reloadPanel) {
            $("#" + stepSource.id + " > .lp-body").empty().append(contentHtml);
        } else {

            $("#slideContainer").append(html);
            $("#" + stepSource.id + " > .lp-body").append(contentHtml);

            $('body').css('overflow-y', 'hidden');
            $("#" + stepSource.id).addClass("animation animating bounceInUp")
                .one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend",
                function () {
                    $(this).removeClass("animation animating bounceInUp");
                    $('body').css('overflow-y', '');
                });
        }

        if (stepSource.control.length > 1) {
            $("#" + stepSource.pageId).pagination({
                items: stepSource.control.length,
                itemsOnPage: 1,
                onInit: function () {
                    $("#" + stepSource.pageId + "-1").show();
                    $("#" + stepSource.id + " > .lp-body").addClass("page-enabled");
                },
                onPageClick: function (pageNumber, event) {
                    $("div[id*='" + stepSource.pageId + "-']").hide();
                    $("#" + stepSource.pageId + "-" + pageNumber).show();
                }
            });
        } else {
            $("#" + stepSource.pageId + "-1").show();
        }
    },
    removePanel: function () {

    },
    viewPanel: function (step) {

        for (var iCount = this.step; iCount > step; iCount--) {
            if (this.layoutType == aca.constant.layout.box) {
                var $ctl = $("#pnl-" + iCount);
                $('body').css('overflow-y', 'hidden');
                $ctl.addClass("animation animating bounceOutDown position-abs")
                    .one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend",
                    function () {
                        $(this).removeClass("animation animating bounceOutDown");
                        $('body').css('overflow-y', '');
                        $(this).remove();
                    });
            } else
                $("#pnl-" + iCount).remove();
        }

        for (var iCountRow = this.step; iCountRow > 0; iCountRow--)
            $("#pnl-" + iCountRow).removeClass("lp-hover llp-hover lp-shadow-none trans-" + ((this.step - iCountRow) - 1));


        this.step = step;
        this.arrangeCards();
        this.step = this.step + 1;


    },
    LoadControl: function (loadlevel, ActionBar) {
        this.loadlevel = loadlevel;

        switch (this.loadlevel) {
            case aca.constant.filterLevel.Home:
                //Get all the Base Filter options
                aca.launchPad.getHome();
                break;

            case aca.constant.filterLevel.Base:
                //Get all the Base Filter options
                aca.launchPad.getBase();
                break;

            case aca.constant.filterLevel.Module:
                //Get all the Modules
                aca.launchPad.getModule(ActionBar);
                break;

            case aca.constant.filterLevel.SubModule:
                aca.launchPad.getSubModule(aca.data.tempInputFilter.moduleId);
                break;

            case aca.constant.filterLevel.GroupType:
                //Find Module Id
                aca.launchPad.getGroupType(aca.data.tempInputFilter.moduleId);
                break;

            case aca.constant.filterLevel.SubType:
                var filterValue = ActionBar || "";  aca.launchPad.getGroupSubType(aca.data.tempInputFilter.moduleId, aca.data.tempInputFilter.groupTypeId, filterValue);
                break;

            case aca.constant.filterLevel.CategoryOrAlias:
                aca.launchPad.getGroupCategoryOrAlias(aca.data.tempInputFilter.moduleId, aca.data.tempInputFilter.groupTypeId, aca.data.tempInputFilter.groupSubTypeId);
                break;

            case aca.constant.filterLevel.None:
                this.loadingMask.hide();
                var $iframe = $("#IframeControl");

                $iframe.prev().hide();
                $iframe.show().find(".frame-screen").css("min-height", ($(window).height() - 90) + "px");

                break;

            default:
                break;
        }
    },
    getDynamicParams: function () {

        var param = "";
        switch (this.loadlevel) {

            case aca.constant.filterLevel.SubModule:
                param = "data-moduleid=" + aca.data.tempInputFilter.moduleId;
                break;

            case aca.constant.filterLevel.GroupType:
                param = "data-moduleid=" + aca.data.tempInputFilter.moduleId
                    + " data-submoduleid=" + aca.data.tempInputFilter.subModuleId;
                break;

            case aca.constant.filterLevel.SubType:
                param = "data-moduleid=" + aca.data.tempInputFilter.moduleId
                    + " data-submoduleid=" + aca.data.tempInputFilter.subModuleId
                    + " data-grouptypeid=" + aca.data.tempInputFilter.groupTypeId;
                break;

            case aca.constant.filterLevel.CategoryOrAlias:
                param = "data-moduleid=" + aca.data.tempInputFilter.moduleId
                    + " data-submoduleid=" + aca.data.tempInputFilter.subModuleId
                    + " data-grouptypeid=" + aca.data.tempInputFilter.groupTypeId
                    + " data-groupsubtypeid=" + aca.data.tempInputFilter.groupSubTypeId;
                break;
        }

        return param;

    },
    loadStepData: function (controlJsonData) {

        var configId; // Identify the Template to use
        switch (this.loadlevel) {
            case 1:
                configId = 1;
                break;
            case 2:
                configId = 2;
                break;
            default:
                configId = 3;
                break;
        }

        //handlebars and angularjs is conflict, we should assignment to lablekey with the help of intermediate variable 
        var data = {
            "id": "pnl-" + (this.reloadPanel ? this.reloadStep : this.step),
            "pageId": "pnlPage-" + (this.reloadPanel ? this.reloadStep : this.step),
            "stepNumber": (this.reloadPanel ? this.reloadStep : this.step),
            "title": getLaunchpadTitle(aca.data.launchPadConfig[configId].isDynamicTitle, configId),
            "template": aca.data.launchPadConfig[configId].template,
            "dynamicparams": this.getDynamicParams(),
            "currentLevel": this.loadlevel,
            "isLoggedIn": (getSessionStorage("userInformation") ? JSON.parse(getSessionStorage("userInformation")) : anonymousUser).isLoggedIn,
            "totalRecord": 0,
            "control": [],
            "aca_newui_launchpad_label_back": LaunchPadData.aca_newui_launchpad_label_back,
            "aca_newui_launchpad_label_start": LaunchPadData.aca_newui_launchpad_label_start,
            "aca_newui_launchpad_label_starthere": LaunchPadData.aca_newui_launchpad_label_starthere,
            "aca_newui_launchpad_label_startdescription": LaunchPadData.aca_newui_launchpad_label_startdescription,
            "aca_newui_launchpad_label_startbutton": LaunchPadData.aca_newui_launchpad_label_startbutton,
            "aca_newui_launchpad_label_logout": LaunchPadData.aca_newui_launchpad_label_logout,
            "aca_newui_launchpad_label_logoutdescription": LaunchPadData.aca_newui_launchpad_label_logoutdescription,
            "aca_newui_launchpad_label_login": LaunchPadData.aca_newui_launchpad_label_login,
            "aca_newui_launchpad_label_logindescription": LaunchPadData.aca_newui_launchpad_label_logindescription,
            "aca_newui_launchpad_label_logoutbutton": LaunchPadData.aca_newui_launchpad_label_logoutbutton,
            "aca_newui_launchpad_label_loginbutton": LaunchPadData.aca_newui_launchpad_label_loginbutton,
            "aca_newui_launchpad_label_exploremaps": LaunchPadData.aca_newui_launchpad_label_exploremaps,
            "aca_newui_launchpad_label_exploremapshere": LaunchPadData.aca_newui_launchpad_label_exploremapshere,
            "aca_newui_launchpad_label_exploremapsdescription": LaunchPadData.aca_newui_launchpad_label_exploremapsdescription,
            "aca_newui_launchpad_label_viewmapbutton": LaunchPadData.aca_newui_launchpad_label_viewmapbutton,
            "aca_newui_launchpad_label_mydashboard": LaunchPadData.aca_newui_launchpad_label_mydashboard,
            "aca_newui_launchpad_label_yourdashboard": LaunchPadData.aca_newui_launchpad_label_yourdashboard,
            "aca_newui_launchpad_label_dashboarddescription": LaunchPadData.aca_newui_launchpad_label_dashboarddescription,
            "aca_newui_launchpad_label_dashboardbutton": LaunchPadData.aca_newui_launchpad_label_dashboardbutton,
            "aca_newui_launchpad_label_register": LaunchPadData.aca_newui_launchpad_label_register,
            "aca_newui_launchpad_label_havenoaccount": LaunchPadData.aca_newui_launchpad_label_havenoaccount,
            "aca_newui_launchpad_label_registerdescription": LaunchPadData.aca_newui_launchpad_label_registerdescription,
            "aca_newui_launchpad_label_registerbutton": LaunchPadData.aca_newui_launchpad_label_registerbutton,
            "aca_newui_launchpad_label_apply": LaunchPadData.aca_newui_launchpad_label_apply,
            "aca_newui_launchpad_label_applyhere": LaunchPadData.aca_newui_launchpad_label_applyhere,
            "aca_newui_launchpad_label_applydescription": LaunchPadData.aca_newui_launchpad_label_applydescription,
            "aca_newui_launchpad_label_applybutton": LaunchPadData.aca_newui_launchpad_label_applybutton,
            "aca_newui_launchpad_label_request": LaunchPadData.aca_newui_launchpad_label_request,
            "aca_newui_launchpad_label_requestdescription": LaunchPadData.aca_newui_launchpad_label_requestdescription,
            "aca_newui_launchpad_label_feeestimates": LaunchPadData.aca_newui_launchpad_label_feeestimates,
            "aca_newui_launchpad_label_feeestimatesdescription": LaunchPadData.aca_newui_launchpad_label_feeestimatesdescription,
            "aca_newui_launchpad_label_schedule": LaunchPadData.aca_newui_launchpad_label_schedule,
            "aca_newui_launchpad_label_scheduledescription": LaunchPadData.aca_newui_launchpad_label_scheduledescription
        };

        /*----Conent Paging----Start----*/
        var iRecCount = 0, totalRecCount = 0, iRow = 1;
        var temp = [];
        $.each(controlJsonData, function (i, e) {
            temp.push({
                id: e.id,
                text: e.text,
                level: e.level,
                value: e.value
            });

            iRecCount++, totalRecCount++;
            if (iRecCount === aca.launchPad.itemsPerPage) {
                data.control.push({
                    "pagenumber": iRow,
                    "pagecotent": temp
                });

                iRow++, iRecCount = 0, temp = [];
            }
        });
        if (temp.length != 0)
            data.control.push({ "pagenumber": iRow, "pagecotent": temp });

        data.totalRecord = totalRecCount;
        /*----Conent Paging----End----*/

        this.createPanel(data);
        if (this.reloadPanel) {
            this.reloadPanel = false;
            this.reloadStep = 1;
        } else {
            this.arrangeCards();
            this.step++;
            aca.data.tempInputFilter = {};//re intalize
        }
        aca.util.initFastClick();
        this.loadingMask.hide();//Hide Loading Mask
    },
    arrangeCards: function () {

        var $this = null, margintop = 0, _tCal = 0;
        for (var iCount = this.step; iCount > 0; iCount--) {
            $this = $("#pnl-" + iCount);

            if (this.layoutType == aca.constant.layout.timemachine) {
                margintop = ((this.step - iCount) * (30 - (this.step - iCount)));
                $this.css("top", "-" + margintop + "px").css("z-index", iCount);
                $this.addClass("trans-" + (this.step - iCount));
                $this.removeClass("lp-hover llp-hover lp-s1 lp-s2 trans-" + ((this.step - iCount) - 1));

                if (iCount != this.step)
                    $this.addClass("lp-hover");

            } else if (this.layoutType == aca.constant.layout.accordion) {
                margintop = ((this.step - iCount) * (25));
                $this.css("top", "-" + margintop + "px").css("z-index", iCount).addClass("lp-s2");
                $this.removeClass("lp-hover llp-hover lp-s1 lp-s2 trans-" + ((this.step - iCount) - 1));

                if (iCount != this.step)
                    $this.addClass("llp-hover")

            } else if (this.layoutType == aca.constant.layout.box) {
                $this.css("top", "0px").css("z-index", iCount);
                $this.removeClass("lp-hover llp-hover lp-s1 lp-s2 position-abs trans-" + ((this.step - iCount) - 1));


                if (iCount != this.step)
                    $this.addClass("lp-s1")

                if (aca.util.device.isPhone == true && iCount != this.step)
                    $this.addClass("position-abs")
            }
        }
    },
    changeLayout: function (layout) {
        this.layoutType = layout;
        if (aca.constant.layout.accordion == layout) {
            for (var iCount = this.step; iCount > 0; iCount--) {
                $this = $("#pnl-" + iCount);
                $this.removeClass("lp-hover trans-" + ((this.step - iCount) - 1));
            }
        }
        this.step = this.step - 1;
        this.arrangeCards();
        this.step = this.step + 1;
    },
    loadingMask: {
        show: function ($control) {

            if (!$('.loadmask2').exists())
                $('body').append('<div class="indicator loadmask2"></div>');

            //$control.append('<div class="loading-container"><div class="loading-wrap"><div class="clicker">Loading</div><div class="circle angled"></div></div></div>');
            $control.append('<div class="loading-container animation animating bounceIn"><span class="inlinetext">Loading..</span></div>').addClass("loadmask");
            $('.loadmask2').addClass('show').css("min-height", $(document).height() + "px");

        },
        hide: function () {
            $('.loadmask2').removeClass('show').removeAttr('style');
            $('.loading-container').parent().removeClass("loadmask");
            $('.loading-container').remove();

        }
    }
};

$(document).on('click', '.lp-hover > .lp-header, .llp-hover > .lp-header', function (e) {
    e.preventDefault();
    var stepnumber = $(this).parent().data('step');
    aca.launchPad.viewPanel(stepnumber);
});

$(document).on('click', '.lp-back', function (e) {
    e.preventDefault();
    var stepnumber = $(this).parent().parent().data('step');
    aca.launchPad.viewPanel(stepnumber - 1);
});

aca.data.tempInputFilter = {};

$(document).on('click', '.lp-btn', function (e) {
    if (isAdmin) {
        return false;
    }
    lpbtnClick($(this));
});

function lpbtnClick(obj) {
    var loadlevel = obj.data('nextlevel'),
        id = obj.data('id'),
        buttontext = obj.data("title"),
        buttonValue = obj.data("value");

    aca.launchPad.loadingMask.show(obj.hasClass("back") ? obj.parent().parent() : obj);
    var $parent = (obj.hasClass("back") ? obj : obj.parent().parent());

    aca.data.tempInputFilter = {
        "buttonValue": buttonValue,
        "buttonText": buttontext,
        "currentLevel": $parent.data("currentlevel"),
        "moduleId": $parent.data('moduleid'),
        "subModuleId": $parent.data('submoduleid'),
        "groupTypeId": $parent.data('grouptypeid'),
        "groupSubTypeId": $parent.data('groupsubtypeid')
    };

    switch (aca.data.tempInputFilter.currentLevel) {

        case aca.constant.filterLevel.Module:
            aca.data.tempInputFilter.moduleId = id;
            break;

        case aca.constant.filterLevel.SubModule:
            aca.data.tempInputFilter.subModuleId = id;
            break;

        case aca.constant.filterLevel.GroupType:
            aca.data.tempInputFilter.groupTypeId = id;
            break;

        case aca.constant.filterLevel.SubType:
            aca.data.tempInputFilter.groupSubTypeId = id;
            break;

        case aca.constant.filterLevel.CategoryOrAlias:
            break;
    }

    aca.launchPad.LoadControl(loadlevel);
}

aca.launchPad.getHome = function () {
    //var nextlevel = aca.constant.filterLevel.Base;
    var json = [{
        id: "HOME1",
        text: "Start",
        level: aca.constant.filterLevel.Base
    }, {
        id: "HOME2",
        text: "Sign in",
        level: aca.constant.filterLevel.None
    },
    {
        id: "HOME3",
        text: "Explore",
        level: aca.constant.filterLevel.None
    }, {
        id: "HOME4",
        text: "Dashboard",
        level: aca.constant.filterLevel.None
    }];
    aca.launchPad.loadStepData(json);

};

aca.launchPad.getBase = function () {

    //var json = {
    //    column1: [{
    //        id: "B1",
    //        icon: "lp-icon-apply",
    //        text: "Apply",
    //        level: aca.constant.filterLevel.Module,
    //        flipperText: "Apply here",
    //        flipperConetent: "Start your online application for permits, licenses, and other applications. You may need to Register/ Sign In to access some applications.",
    //        linkText: "Click here to get started"
    //    }, {
    //        id: "B3",
    //        icon: "lp-icon-request",
    //        text: "Request",
    //        level: aca.constant.filterLevel.GroupType,
    //        flipperText: "Request",
    //        flipperConetent: "Submit a service request, complaint or an inquiry to us and we will initiate necessary actions.",
    //        linkText: "Click here to get started"
    //    }],
    //    column2: [
    //     {
    //         id: "B4",
    //         icon: "lp-icon-estimate",
    //         text: "Fee Estimate",
    //         level: aca.constant.filterLevel.Module,
    //         flipperText: "Fee Estimate",
    //         flipperConetent: "Obtain fee estimates for applications associated with your project.",
    //         linkText: "Click here to get started"
    //     },{
    //        id: "B2",
    //        icon: "lp-icon-Schedule",
    //        text: "Schedule",
    //        level: aca.constant.filterLevel.Module,
    //        flipperText: "Schedule",
    //        flipperConetent: "Schedule an inspection from here. Pick the time from the available time slots",
    //        linkText: "Click here to get started"
    //    }]
    //};

    //No Json required, it is defined in the template
    //so send empyty json    
    var json = [];
    aca.launchPad.loadStepData(json);
};

aca.launchPad.getModule = function (ActionBar) {
    //var nextlevel = aca.constant.filterLevel.SubModule;
    var json = [];
    var key = aca.data.tempInputFilter.buttonText;

    if (typeof (ActionBar) != "undefined" && ActionBar != "") {
        key = ActionBar;
    }

    if (aca.data.IsFromMap && aca.data.mapJsonData != "") {
        var mapjson = JSON.parse(aca.data.mapJsonData);
        var commond = mapjson.command;

        if (commond == aca.data.commond.creat)//Apply
        {
            key = "Apply";
        }
    }

    $.ajax({
        type: 'GET',
        data: { actionKey: key },
        dataType: "json",
        url: servicePath + "api/actioncenter/Modules",
        success: function (response) {
            var data = response.modules;

            if (data.length < 1) {
                aca.launchPad.loadingMask.hide();
                ShowModalWindow(CommonData.aca_newui_home_label_notice, CommonData.aca_newui_home_label_nodata);
                return;
            }

            if (!data) {
                return;
            }

            //Can be replace with Ajax request
            $.each(data, function (index, element) {
                json.push({
                    id: key + "|" + index,
                    text: element.moduleTitle,
                    level: aca.constant.filterLevel.GroupType,
                    value: element.module
                });
            });
            aca.launchPad.loadStepData(json);
        },
        error: function (response) {
            ShowModalWindow(CommonData.aca_newui_home_label_error, response.statusText);
            aca.launchPad.loadingMask.hide();
        }
    });
};

aca.launchPad.getSubModule = function (moduleId) {
    //var nextlevel = aca.constant.filterLevel.GroupType;
    var json = [];

    //Can be replace with Ajax request
    $.each(aca.data.launchpadstore.Modules, function (index, element) {
        if (element.Id == moduleId) {

            $.each(element.subModules, function (i, e) {
                json.push({
                    id: e.id,
                    text: e.subModule,
                    level: aca.constant.filterLevel.GroupType,
                    value: ""
                });
            });
        }
    });

    //Call to load the Panel
    setTimeout(function () {
        aca.launchPad.loadStepData(json);
    }, 3000);
};

aca.launchPad.getGroupType = function (moduleId) {
    var mapjson = aca.data.mapJsonData;
    var json = [];
    var moduleName = "";
    var key = "";

    if (aca.data.IsFromMap && aca.data.mapJsonData != "") {
        var datajson = JSON.parse(aca.data.mapJsonData);
        var commond = datajson.command;

        if (commond == aca.data.commond.request) //Request
        {
            moduleName = "ServiceRequest";
            key = "Apply";
        } else {
            moduleName = aca.data.tempInputFilter.buttonValue;
            key = moduleId.split("|")[0];
        }
    }
    else if (typeof (moduleId) == "undefined")//if the moduleId is undefined ,it means we click request module to get captype
    {
        moduleName = "ServiceRequest";
        key = "Apply";
    } else {
        moduleName = aca.data.tempInputFilter.buttonValue;
        key = moduleId.split("|")[0];
    }

    $.ajax({
        type: 'GET',
        data: { callerName: moduleName, actionKey: key, mapData: mapjson },
        dataType: "json",
        url: servicePath + "api/actioncenter/SubModules",
        success: function (response) {
            var data = response.subModules;

            if (data.length < 1) {
                aca.launchPad.loadingMask.hide();
                ShowModalWindow(CommonData.aca_newui_home_label_notice, CommonData.aca_newui_home_label_nodata);
                return;
            }

            if (aca.data.isActionBtnApply != "") {
                aca.launchPad.Getparcelmodel(aca.data.ParcelNumber, aca.data.AddressDescription, moduleName);
            }

            if (!data) {
                return;
            }
            else if (data.length == 1) {
                var item = data[0];
                aca.data.tempInputFilter.groupTypeId = key + "|" + moduleName + "|" + item.url + "|0";
                aca.launchPad.LoadControl(aca.constant.filterLevel.SubType, item.filterName);

            }
            else {
                //Can be replace with Ajax request
                $.each(data, function (index, element) {
                    json.push({
                        id: key + "|" + moduleName + "|" + element.url + "|" + index,
                        text: element.subModuleName,
                        level: aca.constant.filterLevel.SubType,
                        value: ""
                    });
                });

                //Call to load the Panel
                setTimeout(function () {
                    aca.launchPad.loadStepData(json);
                }, 3000);
            }
        },
        error: function (response) {
            ShowModalWindow(CommonData.aca_newui_home_label_error, response.statusText);
            aca.launchPad.loadingMask.hide();
        }
    });

};

aca.launchPad.getGroupSubType = function (moduleId, groupTypeId,filterValue) {
    var nextlevel = aca.constant.filterLevel.CategoryOrAlias;
    var json = [];

    var key = groupTypeId.split("|")[0];
    var moduleName = groupTypeId.split("|")[1];
    var url = groupTypeId.split("|")[2];

        $.ajax({
            type: 'GET',
            data: { callerName: moduleName, filterValue: filterValue, url: url },
            dataType: "json",
            url: servicePath + "api/actioncenter/CapTypes",
            success: function (response) {
                var isForceLogin = response.isForceLogin;
                if (isForceLogin) {
                    ShowModalWindow(CommonData.aca_newui_home_label_notice, CommonData.acc_login_label_forceLoginNote);
                    redirectByRoute("Login");
                    return;
                }

                var data = response.capTypes;

                if (data.length < 1) {
                    aca.launchPad.loadingMask.hide();
                    ShowModalWindow(CommonData.aca_newui_home_label_notice, CommonData.aca_newui_home_label_nodata);
                    return;
                }

                if (!data) {
                    return;
                }

                if (key.indexOf("Apply") >= 0 || key.indexOf("ObtainFeeEstimate") >= 0) {
                    $.each(data, function (index, element) {
                        json.push({
                            id: key + "|" + moduleName + "|" + element.capTypeValue + "|" + element.capTypeText + "|" + index,
                            text: element.capTypeText,
                            level: aca.constant.filterLevel.CategoryOrAlias,
                            value: ""
                        });

                    });

                    //Call to load the Panel
                    aca.launchPad.loadStepData(json);
                }
                else {
                    if (url != "") {
                        setIframeSrc(url.indexOf("/") > -1 ? url.substr(1, url.length - 1) : url);
                        aca.launchPad.loadingMask.hide();
                    }
                }
            },
            error: function (response) {
                ShowModalWindow(CommonData.aca_newui_home_label_error, response.statusText);
                aca.launchPad.loadingMask.hide();
            }
        });
      
};

aca.launchPad.getGroupCategoryOrAlias = function(moduleId, groupTypeId, subGroupTypeId) {

    var nextlevel = aca.constant.filterLevel.None;
    var json = [];
    var key = subGroupTypeId.split("|")[0];
    var moduleName = subGroupTypeId.split("|")[1];
    var CapTypeValue = subGroupTypeId.split("|")[2];
    var CapTypeText = subGroupTypeId.split("|")[3];

    $.ajax({
        type: 'GET',
        data: { moduleTitle: moduleName, capTypeValue: CapTypeValue, capTypeText: CapTypeText, actionKey: key },
        dataType: "json",
        url: servicePath + "api/actioncenter/cap-detail",
        success: function (response) {
            var data = response.capTypeDetail;

            if (data.length < 1) {
                aca.launchPad.loadingMask.hide();
                ShowModalWindow(CommonData.aca_newui_home_label_notice, CommonData.aca_newui_home_label_nodata);
                return;
            }

            if (!data) {
                return;
            }

            var url = data[0].url;
            if (url != "") {
                var isFromGIS = "";

                if (aca.data.IsFromMap || aca.data.isActionBtnApply) {
                    isFromGIS = "&IsFromMap=true";
                }

                setIframeSrc("Cap/" + url + isFromGIS);
            } else {
                ShowModalWindow(CommonData.aca_newui_home_label_notice, CommonData.aca_newui_home_msg_nocaptype);
            }

            aca.launchPad.loadingMask.hide();
        },
        error: function (response) {
            ShowModalWindow(CommonData.aca_newui_home_label_error, response.statusText);
            aca.launchPad.loadingMask.hide();
        }
    });
};

//link to Apply
aca.launchPad.Getparcelmodel = function (ParcelNumber, AddressDescription, moduleName) {
    $.ajax({
        type: 'GET',
        data: { ParcelNumber: ParcelNumber, AddressDescription: AddressDescription, moduleName: moduleName },
        dataType: "json",
        url: servicePath + "api/Map/ApoApplyInfo",
        success: function (response) {

        },
        error: function (response) {
            ShowModalWindow(CommonData.aca_newui_home_label_error, response.Message);
        }
    });
};

function getLabelKeys(route) {
    var keysValue = sessionStorage.getItem(route);
    var commonData;

    if (keysValue) {
        commonData = JSON.parse(keysValue);
        return commonData;
    }

    var keys = dataKeyJson;
    var labelKey = {
        "route": route,
        "keys": getKeysByRoute(route, keys),
        "cultureName": ""
    };

    $.ajax({
        type: "POST",
        url: servicePath + "api/LabelKey/Label-Key",
        async: false,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(labelKey),
        success: function (response) {
            sessionStorage.setItem(route, JSON.stringify(response));
            commonData = response;
        }
    });

    return commonData;
}

function getKeysByRoute(route, keys) {
    switch (route) {
        case routeName.Home:
            keys = keys.Home;
            break;
        case routeName.LaunchPad:
            keys = keys.LaunchPad;
            break;
        case routeName.Dashboard:
            keys = keys.Dashboard;
            break;
        case routeName.MapView:
            keys = keys.MapView;
            break;
        case routeName.Login:
            keys = keys.Login;
            break;
        case routeName.SearchView:
            keys = keys.SearchView;
            break;
        default:;
    }

    return keys;
};

var routeName = {
    "Home": "Home",
    "LaunchPad": "LaunchPad",
    "Dashboard": "Dashboard",
    "Login": "Login",
    "MapView": "MapView",
    "SearchView": "SearchView"
};

function getLaunchpadTitle(isDynamicTitle, configId) {
    var title = "";

    if(isDynamicTitle) {
        title = aca.data.tempInputFilter.buttonText;

        switch (title) {
            case "Start":
                title = LaunchPadData.aca_newui_launchpad_label_start;
                break;
            case "Apply":
                title = LaunchPadData.aca_newui_launchpad_label_apply;
                break;
            case "Request":
                title = LaunchPadData.aca_newui_launchpad_label_request;
                break;
            case "ObtainFeeEstimate":
                title = LaunchPadData.aca_newui_launchpad_label_feeestimates;
                break;
            case "ScheduleAnInspection":
                title = LaunchPadData.aca_newui_launchpad_label_schedule;
                break;
            default:
                break;
        }

    } else {
        //when use other page to use apply button ,clear the label key sessionStorage
        if (configId == 1) {
            removeUiSessionStorage();

            CommonData = getLabelKeys("Home");
            LaunchPadData = getLabelKeys("LaunchPad");

            if (CommonData != undefined && CommonData.aca_newui_home_label_launchpad) {
                launchTitle = CommonData.aca_newui_home_label_launchpad;
                aca.data.launchPadConfig[1].title = launchTitle;
            }

            if (LaunchPadData != undefined && LaunchPadData.aca_newui_launchpad_label_start) {
                baseTitle = LaunchPadData.aca_newui_launchpad_label_start;
                aca.data.launchPadConfig[2].title = baseTitle;
            }
        }

        title=  aca.data.launchPadConfig[configId].title;
    }

    return title;
}


function SignOut() {
    $.ajax({
        type: 'GET',
        dataType: "json",
        url: servicePath + "api/publicuser/sign-out",
        success: function (response) {
            removeSessionStorage("userInformation");
            save2SessionStorage("userInformation", JSON.stringify(anonymousUser));
            window.location.href = servicePath;
        }
    });
};

function register() {
    if (isAdmin) {
        return false;
    }

    setIframeSrc("Account/RegisterDisclaimer.aspx?isFromNewUi=Y");
};

function login() {
    if (isAdmin) {
        return false;
    }

    redirectByRoute("Login");
};

function ShowMap() {
    if (isAdmin) {
        return false;
    }

    redirectByRoute("MapView");
    
};

function showDashboard() {
    redirectByRoute("Dashboard");
};

function redirectByRoute(route) {
    window.location.href = window.location.href.split('#')[0] + "#/" + route;
}