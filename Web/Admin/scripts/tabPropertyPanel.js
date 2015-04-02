/**
 * <pre>
 * 
 *  Accela Citizen Access Admin
 *  File: tabPropertyPanel.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 * 
 *  Description:Load Tabs Information.
 * 
 *  Notes:
 *      $Id: tabPropertyPanel.js 77905 2008-04-10 12:49:28Z ACHIEVO\levin.feng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

//Load Tab control
var getObj;
var oldTabNames = '';

function LoadTabInfo(obj)
{
    CloseWindow();
    getObj = obj;
    delTabs(obj);
    getTabs(obj);
}

//Get page tabs information
function getTabs(navBar)
{
    for(var i=navBar.TabCollection.length-1;i>=0;i--){
        var tab=navBar.TabCollection[i];
        if(tab.Module==undefined){
            tab.Module='_'+i.toString()+'_';
        }
        insertDiv(tab);
    }
};

//more tab setting
function moreTabSetting(navBar)
{
    var obj=new Object();
    obj.navBar=navBar;
    obj.type=0;
    obj._element=$GetObject("lblMore");
    obj._defaultLanguageTextValue="more";
    obj._defaultLabelValue="more";
    LoadHeader(obj);
}

//Clear div elements
function delTabs(obj) 
{
    var dragEls = Ext.get('divField');
    dragEls.dom.innerHTML = "";
};

//Refresh page tabs
function refreshTabs() 
{
    if(getObj){
        var tabNames = '';
        var dragEls = Ext.get('divField').query('.dragblock');
        var newTabs = new Array();
        for(var i = 0; i < dragEls.length; i++) {
            var str = dragEls[i].title;
            //var reg =  /id=.* style/im;
            //str = str.replace(reg,'style'); 
            newTabs.push(str);
            tabNames = tabNames + dragEls[i].id + "|";
        }
        
        getObj.reOrderAdmin(newTabs);
        
        tabNames = tabNames.substring(0,tabNames.length - 1);
        var tabItem = new TabObj(Ext.Const.OpenedId,getObj.Key,tabNames);
        changeItems.UpdateItem(5,tabItem);
        ModifyMark();
    }
};

//Insert div elements
function insertDiv(tab)
{
    var newDiv = Ext.DomHelper.insertFirst('divField', {
        tag:'div',
        id:'div' + tab.Module,
        width: 'auto',
        title:tab.Key,
        cls:'dragblock',
        //html:'<table><tr><td width = 140 height=12>'+div+'</td></tr></table>'
        html:tab.coreText==""?tab.Title:tab.coreText
    }, true);
   new Tipos.DDList(newDiv);
};