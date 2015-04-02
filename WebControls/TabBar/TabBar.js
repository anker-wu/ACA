/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TabBar.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TabBar.js 196747 2011-05-20 07:00:21Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/20/2009     		cary.cao				Initial.
 * </pre>
 */

//Navigation bar id.
DIV_NAV_BAR_ID = "divNaviMenu";

//tab bar container
DIV_PARENT_CONTAINER = "nav_parent_container";

//tab menu list container id.
DIV_MENU_ID = "divNavMenu";

SPAN_ITEM_PLACEHOLDER="tab_item_place_holder";
SPAN_MORE_TAB = "span_more_tab";

LINK_PLACE_HOLDER = "nav_link_place_holder";
LINK_BAR_CONTENT = "nav_link_content";
SPAN_MORE_LINK = "nav_span_more_link";
SPAN_LINKS = "nav_span_links";
DIV_LINK_MENU_ID = "divLinkMenu";

var TabBar=Class.create();
TabBar.prototype = {
    initialize: function () {
        this.width = 0;
        this.domNavBar = $GetObject(SPAN_ITEM_PLACEHOLDER);
        this.domLinkBar = $GetObject(LINK_PLACE_HOLDER); //link bar.
        this.domLinkContent = $GetObject(LINK_BAR_CONTENT); //link bar contents.
        this.domNormalLinks = $GetObject(SPAN_LINKS); //Normal links container.
        this.domMoreLinkButton = $GetObject(SPAN_MORE_LINK); //More link button.
        this.moreTab = new MoreButton(SPAN_MORE_TAB);
        this.moreLinkButton = new MoreButton(SPAN_MORE_LINK); //more link button object, to handle events.
        this.dropDownMenu = new DropDownMenu(DIV_MENU_ID);
        this.dropDownLinks = new DropDownMenu(DIV_LINK_MENU_ID); //link menu object, ot handle events.
        this.lastIndex = 0; //last index of the tab in the right.
        this.TabItems = null; //Tabs & Links' data
        this.TabCollection = new Array();
        this.Cells = new Array();
        this.Menus = new Array();
        this.Links = new Array();
        this.MoreLinks = new Array();
        this.ItemTemplate = "";
        this.SelectedItemTemplate = "";
        this.DropDownMenuTemplate = "";
        this.LinkItemTemplate = "";
        this.IsAdmin = false;
        this.IsRTL = false; //Right to Left
        this.TabBarMaxWidth = 0; //tab bar max width.
        this.LinkBarMaxWidth = 0; //link bar max width.
        this.CurrentTabIndex = 0;
        this.CurrentTabName = '';
        this.DefaultTabName = '';
        this.CurrentKey = '';
        this.IsFirstLoad = false;
        this.reOrderCount = 0; //Limit the number of times for tabs re-order to prevent memory overflow.
        this.showMenu = false;
        this.showLinkMenu = false;
        this.menuWidth = 0;
        this.sign = 'navbar';
    },
    preRender: function () {
        this.getMaxWidth();
        this.generateTabItems();
        this.generateMoreTab();
        this.generateLinks();
        this.generateMoreLinkButton();
        this.render();
    },
    //Get the padding and margin width of the tab bar's container
    getMaxWidth: function () {
        var tabBarContainer = $GetObject(DIV_PARENT_CONTAINER);
        var paddingLeft = getStyleValue(tabBarContainer, "padding-left");
        var paddingRight = getStyleValue(tabBarContainer, "padding-right");
        this.TabBarMaxWidth = tabBarContainer.clientWidth - paddingLeft - paddingRight;

        paddingLeft = getStyleValue(this.domLinkBar, "padding-right");
        paddingRight = getStyleValue(this.domLinkBar, "padding-right");
        this.LinkBarMaxWidth = this.domLinkBar.clientWidth - paddingLeft - paddingRight;
    },
    //Generate tabs depends on the data from ItaItems properties
    generateTabItems: function () {
        var tabs = this.TabItems[0][1];
        var existTab = false;
        for (var i = 0; i < tabs.length; i++) {
            var tab = new Tab();
            tab.setProperties(tabs[i]);
            tab.Order = i;
            if (tab.Key.toLowerCase() == this.CurrentTabName.toLowerCase()) {
                existTab = true;
                this.CurrentTabIndex = tab.Order;
                this.CurrentKey = tab.Key;
            }
            if (tab.Links != undefined && tab.Links.length > 0) {
                for (var k = 0; k < tab.Links.length; k++) {
                    var linkItem = new Link();
                    linkItem.setProperties(tab.Links[k]);
                    linkItem.Order = tab.Order;
                    tab.LinkItems.push(linkItem);
                }
            }
            tab.setTemplate(this.ItemTemplate);
            this.TabCollection.push(tab);
        }
        if (!existTab && !this.IsAdmin) {
            this.CurrentTabName = this.DefaultTabName;
            for (var k = 0; k < this.TabCollection.length; k++) {
                if (this.TabCollection[k].Key.toLowerCase() == this.CurrentTabName.toLowerCase()) {
                    this.CurrentTabIndex = k;
                    break;
                }
            }
        }

    },
    //Reorder Tabs if first login ACA and 'home' tab in 'more' dropdown menu
    resetHomeTab: function (lastIndex, tab) {

        var orgIndex = tab.Order;
        var targetIndex = 0;
        var width = this.moreTab.width + tab.width;

        for (var i = 0; i < this.TabCollection.length; i++) {
            width += this.TabCollection[i].width;
            if (width > this.TabBarMaxWidth) {
                break;
            }
            targetIndex++;
        }

        for (var i = 0; i < this.TabCollection.length; i++) {
            if (i < targetIndex) continue;
            if (i > orgIndex) break;

            var tab = this.TabCollection[i];
            if (tab.Order == orgIndex) {
                tab.Order = targetIndex;
                this.CurrentTabIndex = targetIndex;
            } else {
                tab.Order += 1;
            }
        }
        this.TabCollection.sort(this.comparer);
        this.render(lastIndex);
    },
    //Build current tab
    generateActiveTab: function (tabItem, i) {
        if (tabItem.SelectedItemTemplate == undefined) {
            tabItem.setTemplate(this.SelectedItemTemplate, "active");
        }
        this.Cells[this.Cells.length] = "<span style='display:inline-block;' id='span_tab_" + i + "'>";
        this.Cells[this.Cells.length] = tabItem.SelectedItemTemplate;
        this.Cells[this.Cells.length] = "</span>";
    },
    //Build normal tab
    generateTabItem: function (tabItem, i) {
        this.Cells[this.Cells.length] = "<span style='display:inline-block;' id='span_tab_" + i + "'>";
        this.Cells[this.Cells.length] = tabItem.innerHTML;
        this.Cells[this.Cells.length] = "</span>";
    },
    generateMenu: function (tabItem, i) {
        tabItem.setTemplate(this.DropDownMenuTemplate, "menu");
        this.Menus[this.Menus.length] = "<div class='tab_bar_menu_item'><table role='presentation' border='0' cellspacing='0' cellpadding='0'><tr><td>";
        this.Menus[this.Menus.length] = "<nobr><span style='word-break:keep-all' id='span_tab_" + i + "'>";
        this.Menus[this.Menus.length] = tabItem.DropDownMenuTemplate;
        this.Menus[this.Menus.length] = "</span></nobr>";
        this.Menus[this.Menus.length] = "</td></tr></table></div>";
    },
    generateMoreTab: function () {
        var divMoreButton = $GetObject("__divMoreTemplate");
        divMoreButton.parentNode.removeChild(divMoreButton);
        this.moreTab.setTemplate(divMoreButton.innerHTML);
        var arr = new Array();
        arr[arr.length] = "<span style='display:inline-block;'>";
        arr[arr.length] = this.moreTab.innerHTML;
        arr[arr.length] = "</span>";
        this.moreTab.innerHTML = arr.join("");
    },
    generateMoreLinkButton: function () {
        var divMoreLinkBtn = $GetObject("__divMoreLinkBtnTemplate");
        divMoreLinkBtn.parentNode.removeChild(divMoreLinkBtn);
        this.moreLinkButton.setTemplate(divMoreLinkBtn.innerHTML);
        this.domMoreLinkButton.innerHTML = this.moreLinkButton.innerHTML;
    },
    generateLinks: function () {
        var tab = this.TabCollection[this.CurrentTabIndex]
        if (tab.LinkItems && tab.LinkItems.length > 0) {
            for (var i = 0; i < tab.LinkItems.length; i++) {
                var link = tab.LinkItems[i];
                link.setTemplate(this.LinkItemTemplate, "link");
                var linkHTML = "<span id='span_link_" + i + "'>";

                if (typeof (link.title) == "undefined" || link.title == '') {
                    linkHTML += link.LinkItemTemplate.replace("title=''", "");
                }

                linkHTML += "</span>";
                this.Links[this.Links.length] = linkHTML;
            }
        }
    },
    clear: function () {
        this.showMenu = false;
        this.showLinkMenu = false;
        this.domNavBar.innerHTML = "";
        this.domNormalLinks.innerHTML = "";
        this.dropDownMenu.element.innerHTML = "";
        this.showMoreButton = true;
        this.width = this.moreTab.width;
        this.Cells = new Array();
        this.Menus = new Array();
        this.MoreLinks = new Array();
        Event.removeEvents(this.sign);
    },
    bindEvent: function () {
        for (var i = 0; i < this.TabCollection.length; i++) {
            var tab = this.TabCollection[i];
            var span = $GetObject("span_tab_" + i);
            if (!this.IsAdmin) {
                var parm = new Object();
                parm.navBar = this;
                parm.tab = tab;
                Event.attachEvent(span, "click", tab.onLinkClick, parm, this.sign);
                //bind link events
                if (tab.Order == this.CurrentTabIndex) {
                    if (tab.LinkItems && tab.LinkItems.length > 0) {
                        for (var k = 0; k < tab.LinkItems.length; k++) {
                            var link = tab.LinkItems[k];
                            var span2 = $GetObject("span_link_" + k);
                            var parm2 = new Object();
                            parm2.navBar = this;
                            parm2.tab = link;
                            Event.attachEvent(span2, "click", link.onLinkClick, parm2, this.sign);
                        }
                    }
                }
            }

        }

        var morTab = $GetObject(SPAN_MORE_TAB);
        if (this.showMenu) {
            var paramObj = { moreBtn: morTab, menuObj: this.dropDownMenu };
            Event.attachEvent(morTab, "mousemove", this.moreTab.onMouseOver, paramObj, this.sign);
            Event.attachEvent(morTab, "mouseout", this.moreTab.onMouseOut, paramObj, this.sign);
            Event.attachEvent(this.dropDownMenu.element, "mousemove", this.dropDownMenu.onMouseOver, paramObj, this.sign);
            Event.attachEvent(this.dropDownMenu.element, "mouseout", this.dropDownMenu.onMouseOut, paramObj, this.sign);
        }

        if (this.showLinkMenu) {
            var paramObj = { moreBtn: this.domMoreLinkButton, menuObj: this.dropDownLinks };
            Event.attachEvent(this.domMoreLinkButton, "mousemove", this.moreLinkButton.onMouseOver, paramObj, this.sign);
            Event.attachEvent(this.domMoreLinkButton, "mouseout", this.moreLinkButton.onMouseOut, paramObj, this.sign);
            Event.attachEvent(this.dropDownLinks.element, "mousemove", this.dropDownLinks.onMouseOver, paramObj, this.sign);
            Event.attachEvent(this.dropDownLinks.element, "mouseout", this.dropDownLinks.onMouseOut, paramObj, this.sign);

            //bind onfocus/onblur for all links which are in link menu.
            var linksInMenu = this.dropDownLinks.element.getElementsByTagName('a');

            for (var i = 0; i < linksInMenu.length; i++) {
                Event.attachEvent(linksInMenu[i], "focus", this.dropDownLinks.onLinkFocus, paramObj, this.sign);
                Event.attachEvent(linksInMenu[i], "blur", this.dropDownLinks.onLinkBlur, paramObj, this.sign);
            }
        }

        if (this.IsAdmin && this.IsFirstLoad)
            Event.attachEvent(morTab, "click", this.moreTab.onClick, this, this.sign);

        if (this.IsAdmin) {
            Event.attachEvent($GetObject(DIV_NAV_BAR_ID), "click", this.onClick, this, this.sign);
        }

    },
    reOrder: function (orgIndex, targetIndex) {
        for (var i = 0; i < this.TabCollection.length; i++) {
            if (i < targetIndex) continue;
            if (i > orgIndex) break;

            var tab = this.TabCollection[i];
            if (tab.Order == orgIndex) {
                tab.Order = targetIndex;
            } else {
                tab.Order += 1;
            }
        }
        this.TabCollection.sort(this.comparer);
        if (this.IsAdmin)
            this.render();
        else
            this.rewriteUrl(targetIndex, null);
    },
    reOrderAdmin: function (tabArr) {
        for (var i = 0; i < tabArr.length; i++) {
            for (var k = 0; k < this.TabCollection.length; k++) {
                if (this.TabCollection[k].Key == tabArr[i]) {
                    this.TabCollection[k].Order = i;
                    break;
                }
            }
        }
        this.TabCollection.sort(this.comparer);

        for (var j = 0; j < this.TabCollection.length; j++) {
            if (this.TabCollection[j].Key == this.CurrentKey) {
                this.CurrentTabIndex = j;
                break;
            }
        }
        if (this.IsAdmin)
            this.render();
        else
            this.rewriteUrl(targetIndex, null);
    },
    updateMoreTabName: function () {
        var domMoreTab = $GetObject("more_tab_place_holder");
        //reset width
        var domTD = $GetObject("tdTabContainer");
        domTD.innerHTML = domMoreTab.innerHTML;
        this.moreTab.width = domTD.offsetWidth;

        domTD.innerHTML = "";
        this.render();
    },
    updateMoreLinkButton: function () {
        this.render();
    },
    //when user update tab label, the tab bar will reset immediately.
    updateTabName: function (moduleName, innerHTML, coreText, isTextUpdated) {
        for (var i = 0; i < this.TabCollection.length; i++) {
            var tab = this.TabCollection[i];

            if (tab.Module == undefined) continue;

            if (tab.Module.toLowerCase() == moduleName.toLowerCase()) {
                var span = $GetObject("span_tab_" + tab.Order);

                if (span == undefined) break;
                var as = span.getElementsByTagName('a');
                for (var k = 0; k < as.length; k++) {
                    if (as[k].getAttribute('module') == moduleName) {
                        as[k].innerHTML = innerHTML;
                        if (!tab.isInMenu) {
                            tab.innerHTML = span.innerHTML;
                        }
                        tab.newTebInnerHTML = innerHTML;
                        if (isTextUpdated) {
                            tab.coreText = coreText;
                        }

                        //reset width
                        var domTD = $GetObject("tdTabContainer");
                        domTD.innerHTML = tab.innerHTML;
                        tab.width = domTD.offsetWidth;
                        tab.height = domTD.offsetHeight;
                        domTD.innerHTML = "";
                        this.render();
                        break;
                    }
                }
                break;
            }
        }
    },
    rewriteUrl: function (targetIndex, link) {
        var arr = new Array();
        var url = link == null ? "" : link.URL;
        var split = "|";

        for (var i = 0; i < this.TabCollection.length; i++) {
            var tab = this.TabCollection[i];
            arr[arr.length] = tab.Key + split + i;
            if (i == targetIndex && link == null) {
                url = tab.URL;
            }
        }

        arr[arr.length] = "CurrentTabIndex" + split + targetIndex;
        if (url.indexOf("?") < 0)
            url += "?TabList=";
        else
            url += "&TabList=";

        url += escape(arr.join(split));

        setCookie("TabNav", arr.join(split), null, "/");

        window.location.href = url;
    },
    redirect: function (tab) {
        if (tab.Type == "Link") {
            if (this.IsAdmin) return;
            this.rewriteUrl(tab.Order, tab);
            return;
        }

        var orgIndex = tab.Order;
        var targetIndex = 0;
        if (tab.Order <= this.lastIndex) {
            this.rewriteUrl(tab.Order, null);
            return;
        }
        var width = this.moreTab.width + tab.width;

        for (var i = 0; i < this.TabCollection.length; i++) {
            width += this.TabCollection[i].width;
            if (width > this.TabBarMaxWidth) {
                break;
            }
            targetIndex++;
        }

        this.reOrder(orgIndex, targetIndex, tab);
    },
    comparer: function (a, b) {
        return eval(a.Order) - eval(b.Order);
    },
    onClick: function () {
        $GetObject(DIV_MENU_ID).style.display = "none";
        $GetObject(DIV_LINK_MENU_ID).style.display = "none";
        showTabPropertyPanel(this);
    },
    render: function () {
        this.clear();
        var len = this.TabCollection.length;
        var homeIndex = 0;
        //indicate the tab which name has been updated
        //if home is in dropdown menu,reset tab bar
        if (arguments.length == 1) {
            homeIndex = arguments[0];
        }

        this.Menus[this.Menus.length] = "<div id='submenu' class='submenu' style='position:relative;top:-1px;'><ul>";

        for (var i = 0; i < len; i++) {
            this.width += this.TabCollection[i].width;
            this.TabCollection[i].isCurrentTab = false;
            this.TabCollection[i].isInMenu = false;
            //fix last tab
            if (i == len - 1 && (this.TabCollection[i].width <= this.moreTab.width) && (this.width <= this.TabBarMaxWidth) && !this.IsAdmin) {
                this.width -= this.moreTab.width;
            }
            if (this.width <= this.TabBarMaxWidth) {//current tab
                this.lastIndex = i;
                if (this.CurrentTabIndex == i) {//selected tab
                    this.TabCollection[i].isCurrentTab = true;
                    this.generateActiveTab(this.TabCollection[i], i);
                } else {//normal tab
                    this.generateTabItem(this.TabCollection[i], i);
                }
            }
            else {//dropdown menu
                if (!this.IsAdmin) {
                    if (this.TabCollection[i].Key.toLowerCase() == this.CurrentTabName.toLowerCase() && homeIndex == 0
                        && this.reOrderCount < len) {
                        //if home tab is in dropdown menu, reset it
                        this.reOrderCount++;
                        this.resetHomeTab(this.lastIndex, this.TabCollection[i]);
                        return;
                    }
                }
                this.TabCollection[i].isInMenu = true;
                this.showMenu = true;
                this.generateMenu(this.TabCollection[i], i);
                if (this.TabCollection[i].width > this.TabBarMaxWidth) {
                    this.TabBarMaxWidth = this.TabCollection[i].width;
                }
            }
        }

        this.reOrderCount = 0;
        this.Menus[this.Menus.length] = "</ul></div>";

        this.domNavBar.innerHTML = this.Cells.join("");

        var domMoreTab = $GetObject("more_tab_place_holder");
        if (this.IsAdmin) {
            if (this.IsFirstLoad) {
                domMoreTab.innerHTML = this.moreTab.innerHTML;
            }
        } else {
            if (this.showMenu) {
                domMoreTab.innerHTML = this.moreTab.innerHTML;
            }
        }

        if (this.showMenu) {
            if (this.dropDownMenu.element == null) {
                this.dropDownMenu.element = $GetObject(DIV_MENU_ID);
            }
            this.dropDownMenu.element.innerHTML += this.Menus.join("");

            //reset menu width
            var domTD = $GetObject("tdTabContainer");
            if (domTD == undefined) {
                alert('Can not find id "tdTabContainer", please check if this dom has been removed from TabBar.cs file.');
                return;
            }

            domTD.innerHTML = this.dropDownMenu.element.innerHTML;
            this.menuWidth = domTD.offsetWidth;
            domTD.innerHTML = "";
            domMoreTab.style.display = '';
        } else {
            domMoreTab.style.display = 'none';
        }

        //Generate links.
        var lnkslen = this.Links.length;
        var lnkSeparator = "<span class='tab_tar_separator'>|</span>";

        for (var i = 0; i < lnkslen; i++) {
            var linkHTML = this.Links[i];

            if (this.MoreLinks.length > 0) {
                this.MoreLinks[this.MoreLinks.length] = linkHTML;
                continue;
            }

            var nextLinkHTML = i < lnkslen - 1 ? this.Links[i + 1] : '';
            var existHTML = this.domNormalLinks.innerHTML;
            this.domNormalLinks.innerHTML += (i == 0 ? '' : lnkSeparator) + linkHTML;

            if (this.domLinkContent.offsetWidth > this.LinkBarMaxWidth) {
                this.domNormalLinks.innerHTML = existHTML;
                this.MoreLinks[this.MoreLinks.length] = linkHTML;
            }
            else if (nextLinkHTML != '' && this.domNormalLinks.offsetWidth + this.domMoreLinkButton.offsetWidth > this.LinkBarMaxWidth) {
                this.domNormalLinks.innerHTML += (i == 0 ? '' : lnkSeparator) + nextLinkHTML;

                if (i == lnkslen - 2 && this.domLinkContent.offsetWidth <= this.LinkBarMaxWidth) {
                    //only have one next link.
                    break;
                }

                this.domNormalLinks.innerHTML = existHTML;
                this.MoreLinks[this.MoreLinks.length] = linkHTML;
            }
        }

        //Generate more link menu.
        if (this.MoreLinks.length > 0) {
            this.showLinkMenu = true;
            var objLinksMenu = $get(DIV_LINK_MENU_ID);
            var len = this.MoreLinks.length;
            var lnkMenu = new Array();

            for (var i = 0; i < len; i++) {
                lnkMenu[lnkMenu.length] = '<ul>';
                lnkMenu[lnkMenu.length] = this.MoreLinks[i];
                lnkMenu[lnkMenu.length] = '</ul>';
            }

            objLinksMenu.innerHTML = lnkMenu.join('');
            this.domMoreLinkButton.style.display = '';
        }
        else {
            this.domMoreLinkButton.style.display = 'none';

            if (!this.IsAdmin) {
                //Daily side need not to render the more button if have not more links.
                this.domMoreLinkButton.parentNode.removeChild(this.domMoreLinkButton);
            }
        }

        //fix tab name when it's current tab and tab name has been changed

        for (var j = 0; j < this.TabCollection.length; j++) {
            var currentTab = this.TabCollection[j];
            if (currentTab != undefined && currentTab.newTebInnerHTML != "") {
                var currentSpan = $GetObject("span_tab_" + currentTab.Order);
                if (currentSpan == undefined) return;
                var as = currentSpan.getElementsByTagName('a');
                for (var k = 0; k < as.length; k++) {
                    if (as[k].getAttribute('module') == currentTab.Module) {
                        as[k].innerHTML = currentTab.newTebInnerHTML;
                        break;
                    }
                }
            }
        }

        this.bindEvent();
        this.IsFirstLoad = false;
        this.domNavBar.style.visibility = "";
    }
};

/*
 *	Link Class
 */
 
var Link=Class.create();
Link.prototype = {
    initialize: function () {
        this.Label = '';
        this.Key = '';
        this.Title = '';
        this.URL = '';
        this.Order = '';
        this.Type = 'Link';
    },
    _getOffset: function () {
        //use this dom to hold the tab in master page
        var domTD = $GetObject("tdTabContainer");
        if (domTD == undefined) {
            alert('Can not find id "tdTabContainer", please check if this dom has been removed from TabBar.cs file.');
            return;
        }
        domTD.innerHTML = this.innerHTML;
        this.width = domTD.offsetWidth;
        this.height = domTD.offsetHeight;
        domTD.innerHTML = "";
    },
    getItem: function (dataField) {
        return eval('this.' + dataField);
    },
    setProperties: function (props) {
        for (var i = 0; i < props.length; i++) {
            this[props[i][0]] = props[i][1];
        }
    },
    setTemplate: function (template, type) {
        var arr = template.split('##');
        for (var i = 1; i < arr.length; i += 2) {
            arr[i] = eval('this.' + arr[i]);
        }
        if (type == "active") {
            this.SelectedItemTemplate = arr.join("");
        } else if (type == "menu") {
            this.DropDownMenuTemplate = arr.join("");
        } else if (type == "link") {
            this.LinkItemTemplate = arr.join("");
        } else {
            this.innerHTML = arr.join("");
            this._getOffset();
        }
    },
    closeMenu: function (menuObj) {
        clearTimeout(menuObj.timerId);
        menuObj.timerId = null;
        menuObj.timerId = setTimeout("$get('" + menuObj.element.id + "').style.display = 'none';", 500);
    },
    onClick: function () {
        var evt = getEvent();
        $GetObject(DIV_MENU_ID).style.display = "none";
        $GetObject(DIV_LINK_MENU_ID).style.display = "none";
        Event.stop(evt, true);
        //showTabPropertyPanel(this);
    },
    onLinkClick: function () {
        var evt = getEvent();
        Event.stop(evt, true);
        this.navBar.redirect(this.tab);
    }
};

/*
 *	DropDownMenu Class
 */

var DropDownMenu=Class.create();
DropDownMenu.prototype = Class.inherit(new Link(), {
    initialize: function (id) {
        this.element = $GetObject(id);
        this.Type = 'Tab';
    },
    onMouseOver: function () {
        clearTimeout(this.menuObj.timerId);
        this.menuObj.timerId = null;
        this.menuObj.element.style.display = "";
    },
    onMouseOut: function () {
        if (this.menuObj.timerId != null) {
            return;
        }
        var e = getEvent();
        var el = $GetObject(this.menuObj.element.id);
        var target = e.target || e.srcElement;
        var relatedTarget = e.relatedTarget || e.toElement;
        if ($GetObject(relatedTarget) == undefined) {
            return;
        }
        if (!Event.descendantOf($GetObject(relatedTarget), el) && $GetObject(relatedTarget) != el) {
            //If relatedTarget not itself or not child nodes
            this.menuObj.closeMenu(this.menuObj);
        }
    },
    onLinkFocus: function () {
        clearTimeout(this.menuObj.timerId);
        this.menuObj.timerId = null;
    },
    //Link blur event for link menu item.
    onLinkBlur: function () {
        window.LinkMenuParamObj=this;
        clearTimeout(this.menuObj.timerId);
        this.menuObj.timerId = setTimeout(function () {
            if(window.LinkMenuParamObj)
            {
                window.LinkMenuParamObj.menuObj.closeMenu(window.LinkMenuParamObj.menuObj);
                window.LinkMenuParamObj = null;
            }
        }, 700);
    }
});

/*
 *	MoreButton Class
 */
var MoreButton = Class.create();
MoreButton.prototype = Class.inherit(new Link(), {
    initialize:function(id){
        this.id = id;
    },
    setTemplate: function (template) {
        this.innerHTML = template;
        this._getOffset();
    },
    onMouseOver: function () {
        window.popUp(this.menuObj.element, this.moreBtn, $.global.isRTL);
        clearTimeout(this.menuObj.timerId);
        this.menuObj.timerId = null;
    },
    onMouseOut: function () {
        this.menuObj.closeMenu(this.menuObj);
    },
    onClick: function () {
        var evt = getEvent();
        $GetObject(DIV_MENU_ID).style.display="none";
        $GetObject(DIV_LINK_MENU_ID).style.display="none";
        Event.stop(evt, false);
    }
});

/*
 *	Tab Class
 */
var Tab=Class.create();
Tab.prototype=Class.inherit(new Link(),{
    initialize:function(){
        this.LinkItems=new Array();
        this.Type="Tab";
        //maintains the changed of tab name by admin
        this.newTebInnerHTML='';
        this.coreText='';
        this.isCurrentTab=false;
        this.isInMenu=false;
    }
});

window.NavBar_Loaded=true;
