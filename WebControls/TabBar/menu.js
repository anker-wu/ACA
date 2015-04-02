/*
 * <pre>
 *  Accela Citizen Access
 *  File: CapDetail.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2011
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: menu.js 196738 2011-05-20 02:51:42Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
*/

var Menu=Class.create();

Menu.prototype = {
    initialize: function(links, prop, events) {
        this.links = links; //links properties
        this.isRTL = false;
        this.menuId = null; //menu parent container id
        this.targetId = null;
        this.align = 'left';
        this.sign = '';
        this.firstMenuItem = null;


        this.items = new Array(); //menuitem
        this.menu = new Array();

        this.domMenu = null;
        this.domTarget = null;
        //[[['eventType','click'],['func',myFunc]],[['eventType','mouseover'],['func',myFunc2]]]
        this.eventList = new Array();

        setProperties(this, prop);
        //convert event properties to list
        if (arguments.length == 3) {
            if (events != null) {
                for (var i = 0; i < events.length; i++) {
                    var evt = new Object();
                    setProperties(evt, events[i]);
                    this.eventList.push(evt);
                }
            }
        }

        if (this.sign == "") {
            alert("must indecate 'this.sign' property ,such as 'collection_menu_.");
            return;
        }

        if (this.menuId == null) {
            alert("must indecate menu container id.");
            return;
        } else {
            this.domMenu = $GetObject(this.menuId);
            if (this.domMenu == undefined) {
                alert("can not find menu container:" + this.menuId);
                return;
            }
        }
        if (this.targetId == null) {
            alert("must indecate target container id.");
            return;
        } else {
            this.domTarget = $GetObject(this.targetId);
            if (this.domTarget == undefined) {
                alert("can not find target container:" + this.targetId);
                return;
            }
        }

        this.generateItem();
        this.render();
    },
    generateItem: function() {
        if (this.links != undefined && this.links.length > 0) {
            for (var i = 0; i < this.links.length; i++) {
                var item = new MenuItem(this.links[i], this.sign, i);
                if (this.firstMenuItem == null) {
                    if(item.menuFocusId)
                    {
                        this.firstMenuItem = item;
                    }
                    
                }
                this.items.push(item);
            }
        }
    },
    generateMenu: function(item, i) {
        if(item.align=="right"){
            this.menu[this.menu.length] = "<div id='" + this.sign + "container_" + i + "'><table role='presentation' width='100%' border='0' cellspacing='0' cellpadding='0'><tr><td align='right'>";    
        }
        else{
            this.menu[this.menu.length] = "<div id='" + this.sign + "container_" + i + "'><table role='presentation'  border='0' cellspacing='0' cellpadding='0'><tr><td>";
        } 
        
        this.menu[this.menu.length] = "<span id='" + this.sign + i + "'>";
        this.menu[this.menu.length] = item.html;
        this.menu[this.menu.length] = "</span>";
        this.menu[this.menu.length] = "</td></tr></table></div>";
    },
    showMenu: function() {
        if (MzBrowser.ie && MzBrowser.version == 7) {
            var domTD = $GetObject("tdTabContainer");
            domTD.innerHTML = this.domMenu.innerHTML;
            var w = domTD.offsetWidth;

            domTD.innerHTML = "";
            this.domMenu.parentNode.style.width = "0px";
            this.domMenu.style.width = w;
        }

        if (this.domMenu.parentNode.style.display == "none")
            this.domMenu.parentNode.style.display = "";

        var offsetWidth = this.domTarget.offsetWidth;
        var menuWidth = this.domMenu.offsetWidth;
        var l = 0;
        if (this.align == 'right') {
            if (!this.isRTL) {
                l = offsetWidth - menuWidth;
            } else {
                l = menuWidth - offsetWidth;
            }
        }

        this.domMenu.style.left = l + "px";

    },
    hideMenu: function() {
        this.domMenu.parentNode.style.display = "none";
    },
    render: function() {

        //create menu
        for (var i = 0; i < this.items.length; i++) {
            this.generateMenu(this.items[i], i);
        }
        this.domMenu.innerHTML = this.menu.join("");

        //bind event
        if (this.eventList.length > 0) {
            for (var k = 0; k < this.eventList.length; k++) {
                for (var i = 0; i < this.items.length; i++) {
                    if (this.items[i].type == "label" || this.items[i].canBind == false) {
                        continue;
                    }
                    Event.attachEvent(this.items[i].id, this.eventList[k].eventType, this.eventList[k].func, this.items[i], this.sign);
                }
            }
        }
    }
}

var MenuItem=Class.create();

MenuItem.prototype={
    initialize:function(prop,prefix,id){
        this.html='';
        this.type='link';
        this.align='left';
        this.canBind=true;//can bind event
        this.id=prefix+"container_"+id;
        this.menuFocusId = '';
        setProperties(this,prop);
    }
};
