<!DOCTYPE html>
<html lang="en" ng-app="AcaFaceLiftApp">
    <head>
        <title ng-labelkey="HomeData.aca_newui_home_label_title"></title>
        <meta charset="utf-8" />
        <meta http-equiv="X-UA-Compatible" content="IE=edge" />
        <meta name="author" content="karjmohan" />
        <meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1.0" />

        <link rel="stylesheet" type="text/css" href="Content/font-icon.css" />
        <link rel="stylesheet" type="text/css" href="Content/bootstrap.min.css" />
        <link rel="stylesheet" type="text/css" href="Content/animation.css" />
        <link rel="stylesheet" type="text/css" href="Content/override.css" />
        <link rel="stylesheet" type="text/css" href="Content/default/Site.css" />
        <link rel="stylesheet" type="text/css" href="" id="languageCss"/>
        <!--Color Picker--> 
        <link rel="stylesheet" id="customizedCss" />

        <script type="text/javascript" src="Scripts/app/LabelKeyJson.js"></script>
        <script type="text/javascript" src="Scripts/jquery-2.1.0.min.js"></script>
        <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
        <script type="text/javascript" src="Scripts/jquery-cookie.js"></script>
        <script type="text/javascript" src="Scripts/fastclick.min.js"></script>
        <script type="text/javascript" src="Scripts/handlebars-v1.3.0.js"></script>
        <script type="text/javascript" src="Scripts/jquery.simplePagination.js"></script>
        <script type="text/javascript" src="Scripts/jquery.slimscroll.min.js"></script>
        <script type="text/javascript" src="Scripts/jquery.simplePagination.js"></script>
        <script type="text/javascript" src="Scripts/angular.min.js"></script>
        <script type="text/javascript" src="Scripts/angular-route.min.js"></script>
        <script type="text/javascript" src="Scripts/angular-resource.js"></script>
        <script type="text/javascript" src="Scripts/angular-sanitize.js"></script>
        <script type="text/javascript" src="Scripts/piwik.js" ></script>

        <script type="text/javascript" src="Scripts/app/common.js"></script>
        <script type="text/javascript" src="Scripts/owl.carousel.min.js"></script>
        <script type="text/javascript" src="Scripts/loading.js"></script>
        <script type="text/javascript" src="Scripts/app/admin.js"></script>

        <script type="text/javascript" src="App/App.js"></script>
        <script type="text/javascript" src="App/Routings.js"></script>
        <script type="text/javascript" src="App/Controllers/ActionRequiredController.js"></script>
        <script type="text/javascript" src="App/Controllers/HomeController.js"></script>
        <script type="text/javascript" src="App/Controllers/LaunchPadController.js"></script>
        <script type="text/javascript" src="App/Controllers/DashboardController.js"></script>
        <script type="text/javascript" src="App/Controllers/LoginController.js"></script>
        <script type="text/javascript" src="App/Controllers/SearchViewController.js"></script>
        <script type="text/javascript" src="App/Controllers/MapViewController.js"></script>
        <script type="text/javascript" src="App/Controllers/MyRecordsController.js"></script>
        <script type="text/javascript" src="App/Controllers/ActionButtonController.js"></script>
        <script type="text/javascript" src="App/Controllers/DownloadResultController.js" ></script>
        <script type="text/javascript" src="App/Directives/AngularIframe.js"></script>
        <script type="text/javascript" src="App/Directives/AngularModalDialog.js"></script>
        <script type="text/javascript" src="App/Directives/AngularBindHtml.js"></script>
        <script type="text/javascript" src="App/Directives/AngularCollectionModalDialog.js" ></script>
        <script type="text/javascript" src="App/Services/MapService.js"></script>
        <script type="text/javascript" src="App/Services/GlobalSearchParameterService.js"></script>
        <script type="text/javascript" src="App/Services/LabelKeyService.js"></script>
        <script type="text/javascript" src="App/Filter/extendFilter.js"></script>
    </head>
<body class="fixed-layout" ontouchstart="" ng-controller="HomeController" ng-init="initHome()">
    <!--class="fixed-layout"-->
    <article id="welcome">
        <div class="welcome-container">
            <div class="back-link col-xs-3" ng-init="getOfficialWebSite()">
                <a class="ellipsis textalign-left" href="{{officialWebSiteHref}}" data-control-type="text" title="{{officialWebSiteHref}}" ng-labelkey="format(HomeData.aca_newui_home_label_officialwebsite,officialWebSiteHref)" target="_blank"></a>
            </div>
            <div class="welcome-message col-xs-6" data-toggle="welcomemsg">
                <span class="ellipsis" data-control-type="text" ng-labelkey="format(HomeData.aca_newui_home_label_welcome,HomeData.agencyName)"></span>
                <span class="help-icon ico-question3"></span>
            </div>
            <div class="acce-option">
                <!--                <span class="checkbox custom-checkbox">
                                    <input type="checkbox" name="accessibility-option" id="accessibility-option">
                                    <label for="accessibility-option">&nbsp; Accessibility Support</label>
                                </span>-->
            </div>
            <div class="close-link">
                <select id="ddlLanguage"  ng-show="isShowLanguage" ng-change="ChangeLanguage()" ng-model="selectedLanguage" ng-options="m.languageKey as m.languageName  for m in languages"></select>
                <span class="close-link-ico ico-close2" ng-if="!isAdmin"></span>
            </div>
            <div class="welcome-msg-container" style="display:none;" ng-if="!isAdmin">
                <div class="welcome-msg-content">
                    <div ng-labelkey="HomeData.com_welcome_text_welcomeInfo"></div>
                    <button class="btn btn-custom btn-sm" id="btnWelcomeClose" ng-labelkey="HomeData.aca_newui_home_label_close"></button>
                </div>
            </div>
        </div>
    </article>

    <!-- START Template Header -->
    <header id="header" class="navbar">
        <div class="header-container">
            <div class="navbar-header">
                <a class="navbar-brand" href="javascript:void(0);" ng-click="loadLaunchPad()">
                    <div class="logo-figure" ng-init="getAgencyLogo()">
                        <img ng-src="{{logoSource}}" class="img-responsive">
                    </div>
                    <span class="logo-text">
                        <span class="logo-name clip" ng-labelkey="format(HomeData.aca_newui_home_label_logoname,HomeData.agencyName)" data-control-type="text"></span>
                        <span class="logo-ss ellipsis" ng-labelkey="HomeData.aca_newui_home_label_logodescription" data-control-type="text"></span>
                    </span>
                </a>
            </div>

            <div class="search-bar">
                <div class="navbar-form navbar-left" id="dropdown-searchform">
                    <div class="has-icon">
                        <label for="txtGlobalSearh" ng-labelkey="HomeData.aca_newui_home_label_search" data-control-type="text"></label>
                        <input type="text" id="txtGlobalSearh" ng-disabled="isAdmin" ng-keyup = "$event.keyCode == 13 && DefaultGlobalSearch()"  class="form-control" title="{{HomeData.aca_newui_home_label_keywordvalid}}" placeholder="{{HomeData.aca_newui_home_label_searchplaceholder}}">
                        <i class="ico-search form-control-icon" ng-click="DefaultGlobalSearch()"></i>
                    </div>
                    <div class="search-options" ng-if="!isAdmin">
                        <div class="search-title" ng-hide="GlobalSearchSwitch.LP==GlobalSearchSwitch.APO && GlobalSearchSwitch.LP== GlobalSearchSwitch.CAP && GlobalSearchSwitch.CAP==false" ng-labelkey="HomeData.aca_newui_home_label_searchtitle"></div>
                        <div class="search-content default-search" ng-hide="GlobalSearchSwitch.LP==GlobalSearchSwitch.APO && GlobalSearchSwitch.LP== GlobalSearchSwitch.CAP && GlobalSearchSwitch.CAP==false" >
                            <ul>
                                <li data-forbidden="{{GlobalSearchSwitch.LP}}" ng-class="GlobalSearchSwitch.LP==true?'selected':'ng-hide'" data-key="LP">
                                    <span class="search-fbutton">
                                        <i class="ico-check-sign"></i>
                                        <span class="icon icon-people"></span>
                                        <span class="search-label" ng-labelkey="HomeData.aca_newui_home_label_searchpeople"></span>
                                    </span>
                                    <span class="search-desc" ng-labelkey="HomeData.aca_newui_home_label_searchpeopledescription"></span>
                                </li>
                                <li data-forbidden="{{GlobalSearchSwitch.APO}}"  ng-class="GlobalSearchSwitch.APO==true?'selected':'ng-hide'"  data-key="APO">
                                    <span class="search-fbutton">
                                        <i class="ico-check-sign"></i>
                                        <span class="icon icon-location"></span>
                                        <span class="search-label" ng-labelkey="HomeData.aca_newui_home_label_searchplaces"></span>
                                    </span>
                                    <span class="search-desc" ng-labelkey="HomeData.aca_newui_home_label_searchplacesdescription"></span>
                                </li>
                                <li data-forbidden="{{GlobalSearchSwitch.CAP}}" ng-class="GlobalSearchSwitch.CAP==true?'selected':'ng-hide'" data-key="CAP">
                                    <span class="search-fbutton">
                                        <i class="ico-check-sign"></i>
                                        <span class="icon icon-list"></span>
                                        <span class="search-label" ng-labelkey="HomeData.aca_newui_home_label_searchthings"></span>
                                    </span>
                                    <span class="search-desc" ng-labelkey="HomeData.aca_newui_home_label_searchthingsdescription"></span>
                                </li>
                            </ul>
                        </div>
                        <div class="adv-search-title" data-toggle="advancesearch" ng-click="bindAdvSearch('Search')" ng-labelkey="HomeData.aca_newui_home_label_advancedsearch"></div>
                        <div class="search-content advancesearch-content" ng-hide="hidSearch">
                            <ul>
                                <li ng-repeat="item in modules" ng-click="goSearch(item.url, item.module)">
                                    <span class="search-abutton">
                                        <span class="adv-label" title="{{item.moduleTitle}}">{{item.moduleTitle}}</span>
                                    </span>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>

            <div class="navbar-toolbar clearfix">
                <ul class="nav navbar-nav navbar-right" id="top-navigation" ng-init="isLogin()">
                    <li class="visible-xs" data-toggle="searchbox">
                        <a href="" ng-click="phoneSearchbox()">
                            <span class="meta">
                                <span class="search-mobile-button ico-search4"></span>
                            </span>
                        </a>
                    </li>
                    <li class="dropdown launch-pad">
                        <a href='' ng-click="loadLaunchPad()">
                            <span class="meta">
                                <span class="launchpad-caret"></span>
                                <span class="launchpad-text" ng-labelkey="HomeData.aca_newui_home_label_launchpad"></span>
                            </span>
                        </a>
                    </li>
                    <li class="dropdown user-profile" id="user-profile" ng-init="initProfileDropdown('user-profile')">
                        <a href="javascript:void(0);" class="user dropdown-toggle" data-toggle="dropdown">

                            <span class="m-avatar" ng-if="userInfo.isLoggedIn">
                                <span class="rendering">{{userInfo.firstName | aleph }}{{userInfo.lastName | aleph}}</span>
                                <span class="badge">{{ShoppingCart.length + Announcements.length + Reports.length;}}</span>
                                <span class="caret"></span>
                            </span>

                            <span class="m-avatar" ng-hide="userInfo.isLoggedIn">
                                <span class="banner-login-icon ico-lock"></span>
                                <span class="badge" ng-if="!isAdmin">{{ShoppingCart.length + Announcements.length + Reports.length;}}</span>                           
                                <span class="caret"></span>
                            </span>
                        </a>
                        <ul class="dropdown-menu arrow" role="menu" ng-if="!isAdmin">
                            <li>
                                <ul id="loginDropMenu">
                                    <li><span class="dropdown-header"><span class="ico-warning-sign"></span> {{HomeData.aca_newui_home_label_reports}} <span class="badge">{{Reports.length}}</span></span></li>
                                    <li>
                                        <ul class="dropdown-cart " id="dropdown-Reports">
                                            <li ng-repeat="items in Reports"><a href="javascript:void(0);" ng-click="ReportUrl(items)">{{items.reportName}}</a></li>

                                        </ul>
                                    </li>
                                    <li ng-if="isShowAnn"><span class="dropdown-header"><span class="ico-bubble"></span> {{HomeData.aca_newui_home_label_announcement}} <span class="badge">{{Announcements.length}}</span></span></li>
                                    <li ng-if="isShowAnn">
                                        <ul class="dropdown-cart " id="dropdown-Announcement">
                                            <li ng-repeat="items in Announcements"><a href="javascript:void(0);" ng-click="ShowAnnouncement(items.title,items.text)">{{items.title}}</a></li>
                                        </ul>
                                    </li>
                                    <li ng-if="isShowCart"><span class="dropdown-header"><span class="ico-shopping-cart"></span> {{HomeData.aca_newui_home_label_shoppingcart}} <span class="badge">{{ShoppingCart.length}}</span></span></li>
                                    <li ng-if="isShowCart">
                                        <ul class="dropdown-cart " id="dropdown-ShoppingCart">
                                            <li ng-repeat="item in ShoppingCart" ><a href="" ng-click="ShoppingCartUrl()" ><span class="shop-text">{{item.capID.customID}}</span><span class="shop-price"><span ng-if="ShoppingCart.length>0">${{item.totalFee}}</span></span></a></li>                                     
                                          
                                        </ul>
                                    </li>
                                </ul>
                            </li>
                            <li class="shopping-cart shopping-total" ng-if="userInfo.isLoggedIn && ShoppingCart.length>0"><span class="dropdown-header total-text"><span class="shop-text" ng-labelkey="HomeData.aca_newui_home_label_total"></span><span class="shop-price">${{total}}</span></span></li>
                            <li class="divider"></li>
                            <li class="signout-section" ng-if="userInfo.isLoggedIn" ng-init="GetuserInfo()" ng-click="SignOut()"><a href="javascript:void(0);" ng-labelkey="HomeData.aca_newui_home_label_signout"></a></li>
                            <li class="login-section" ng-hide="userInfo.isLoggedIn"><a href="#/Login" ng-labelkey="HomeData.aca_newui_home_label_login"></a></li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
    </header>
    <!--/ END Template Header -->

    <section id="main" class="main-container" role="main">
        <div ng-view ng-if="!isAdmin||(isAdmin && isColorTheme)"></div>
    </section>

    <footer role="contentinfo" id="footer">
        <div class="footer-container">
            <div class="company">
                <span ng-labelkey="format(HomeData.aca_newui_home_label_company,HomeData.agencyName)" data-control-type="text"></span>
            </div>
            <div class="links">
                <ul>
                    <li><span ng-labelkey="HomeData.aca_newui_home_label_policy" data-control-type="link"></span></li>
                    <li><span ng-labelkey="HomeData.aca_newui_home_label_contactus" data-control-type="link"></span></li>
                    <li><span ng-labelkey="HomeData.aca_newui_home_label_link3" data-control-type="link"></span></li>             
                </ul>
            </div>
            <div class="brand">
                <a href="https://www.accela.com" ng-labelkey="HomeData.aca_newui_home_label_brand" target="_blank" data-control-type="none"></a>
            </div>
        </div>
    </footer>
    <!--This div will only be displayed when phone-->
    <div id="IsPhone"></div>
    <div id="IsTablet"></div>

    <!--Color Picker-->
    <script type="text/javascript" src="Scripts/plugin/colorpicker/spectrum.js"></script>
    <script type="text/javascript" src="Scripts/app/styleswitcher.js"></script>
    <angularmodaldialog></angularmodaldialog>
    <angularcollectionmodaldialog></angularcollectionmodaldialog>
    <script type="text/javascript" src="Scripts/app/launchpad.js"></script>
</body>
</html>