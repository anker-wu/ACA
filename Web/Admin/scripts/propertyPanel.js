/**
 * <pre>
 * 
 *  Accela Citizen Access Admin
 *  File: propertyPanel.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: propertyPanel.js 77905 2008-04-10 12:49:28Z ACHIEVO\jack.feng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

function SwitchProperty(newTab) {
    try {
        var dom;
        var newTabId = newTab.id.toLowerCase();

        dom = GetDom(newTab);

        if (dom) {
            if (newTabId.indexOf('header&footer') > -1) {
                var selectedEle = dom.selectedEle;

                if (selectedEle) {
                    dom.loadEastPanel(selectedEle.getAttribute('data-control-type'), selectedEle);
                }
            } else {
                var selectedObj = dom.currentSelectedObj;

                if (typeof (selectedObj) != 'undefined' && typeof (selectedObj._element) != 'undefined') {
                    LoadProperties(selectedObj._getType(), selectedObj, newTab);
                } else {
                    RefreshRenderGrid();
                }
            }
        }
    } catch (e) {
        RefreshRenderGrid();
    }
}

//Handle the outsite access
function OutsideAccessHandler(ifra) {
    var accessDenied = false;

    try {
        var href = ifra.contentWindow.location.href;
    }
    catch (e) {
        accessDenied = true;
    }

    if (accessDenied || href != ifra.src) {
        RefreshRenderGrid();
    }
}

//the fields properties interface
function LoadProperties(controlType,obj,newTab)
{
    // Collapse ComboBox in Record Type Filter Property Grid Panel.
    var cboCapFilterName = Ext.getCmp('cboCapFilterName');        
    if(cboCapFilterName != null && cboCapFilterName!= 'undefined' && cboCapFilterName.collapse != 'undefined')
    {
        cboCapFilterName.collapse();
    }
    
    obj.type = controlType;
    switch(controlType)
    {
        case 0:
        case 14:
        case 25:
        case 28:
        case 29:
        case 38:
            if (obj.get_IsTemplateField()) {
                LoadGridViewTemplateField(obj);
            }
            else {
                LoadHeader(obj);
                LoadEastPanelTitle(0);
            }
            break;
        case 1:            
            LoadBody(obj);
            LoadEastPanelTitle(0);
            break;
        case 2://Section Text
            GetSectionFields(obj,newTab,false);
            LoadEastPanelTitle(1);
            return;            
        case 3:            
            LoadInput(obj);
            LoadEastPanelTitle(0);
            break;            
        case 4: 
            //properties of the DropDownList which data is from DB
            LoadDropListInfo(obj,"DB");
            LoadEastPanelTitle(0);
            break;            
        case 5:            
            LoadDropListInfo(obj,"StandardChoice");
            LoadEastPanelTitle(0);
            break;            
        case 6:            
            LoadDropListInfo(obj,"HardCode");
            LoadEastPanelTitle(0);
           break;            
        case 7:            
            LoadCheck(obj);
            LoadEastPanelTitle(0);
            break;            
        case 8:            
            LoadHyperLink(obj);
            LoadEastPanelTitle(0);
            break;            
        case 9:            
            LoadButton(obj,false);
            LoadEastPanelTitle(0);
            break;            
        case 10:            
            LoadReportButton(obj);
            LoadEastPanelTitle(0);
            break;            
        case 11:
            LoadRadio(obj);
            LoadEastPanelTitle(0);
            break;            
        case 12://Tabs --never used
            //LoadTabInfo(obj);
            //LoadEastPanelTitle(2);
            break;
        case 13://GridView
            GetSectionFields(obj);
            LoadEastPanelTitle(1);
            break;
        case 14://Applicant Label
            LoadHeader(obj);
            LoadEastPanelTitle(0);
            break;
        case 15://Permit Type DropdownList
            LoadDropListInfo(obj,"DB");
            LoadEastPanelTitle(0);
            break;
        case 16:
            LoadDropListInfo(obj,"HardCode");
            LoadEastPanelTitle(0);
            break;
        case 17:
            LoadLogo(obj);
            LoadEastPanelTitle(0);
            break;
        case 18: //Section with required property configuration
        case 41:
        case 42:
            GetSectionFields(obj,newTab,true);
            LoadEastPanelTitle(1);
            break;
       case 19:
            LoadConfigureUrlButton(obj);//Button Can Configure URL.
            LoadEastPanelTitle(0);
            break;     
        case 20://show check box, only contact type.
            LoadDropListInfo(obj,"STDandXPolicy");
            LoadEastPanelTitle(0);
            break;   
        case 21://Load Payment Method.        
            LoadRadioButtonList(obj,"PaymentHardCode");
            LoadEastPanelTitle(0);
            break;  
        case 22:            
            LoadAutoFillCityInput(obj);
            LoadEastPanelTitle(0);
            break; 
        case 23:            
            LoadAutoFillStateInput(obj);
            LoadEastPanelTitle(0);
            break;   
        case 24:          
            LoadAutoFillDropList(obj,"StandardChoice");
            LoadEastPanelTitle(0);
            break;             
        case 26:            
            LoadButton(obj,true);
            LoadEastPanelTitle(0);
            break;
        case 101:
            LoadReadonly(obj);
            LoadEastPanelTitle(4);
            break;
        case 35:
            LoadPageFlowProperty();
            break;
        case 36:
            LoadPageProperty(obj);
            LoadEastPanelTitle(5);
            break;
        case 37: // load section that has available fileds that can setting visable or not
            GetSectionFields(obj);
            LoadEastPanelTitle(1);
            break;
        case 39:
            // div control contains standard component
            LoadComponent(obj, false);
            LoadEastPanelTitle(1);
            break;
        case 40:
            // div control contains customize component
            LoadComponent(obj, true);
            LoadEastPanelTitle(1);
            break;
        case 50:
            GetSectionFields(obj, newTab, true);
            LoadEastPanelTitle(0);
            break;
        default:
            return;
    }

    if (obj.get_IsTemplateField()) {
        LoadEastPanelTitle(4);
    }

    SetIframeSectionCss(obj,newTab);
};

//Render the field properties grid
//params:
//      type:   which div will be rendered to
//              0:  Field Properties
//              1:  Section Properties
//              2:  Page Properties
//              ...
//
//      obj:    which will be rendered
function RenderGrid(type,obj){
    var tblx;
    
    tblx = document.getElementById('divField');
    
    if(tblx)
    {    
        tblx.innerHTML = '';
        obj.render(tblx);
    }

    var _rowIdx = obj.store.indexOfId("Admin_FieldProperty_Instructions");
    if (_rowIdx >= 0) {
        Ext.get(obj.getView().getCell(_rowIdx, 1)).addClass('watermark');
    }

    _rowIdx = obj.store.indexOfId("Section Instructions");
    if (_rowIdx >= 0) {
        Ext.get(obj.getView().getCell(_rowIdx, 1)).addClass('watermark');
    }
};


function RefreshRenderGrid() {
    var tblx;

    tblx = document.getElementById('divField');

    if (tblx) {
        tblx.innerHTML = '';
    }
}

//support to change the right panel's title
//params:
//      type:   0   field properties
//              1   section properties
//              2   tab order properties
function LoadEastPanelTitle(type){
    var pan = document.getElementById('toolsPanel');
    if(pan)
    {
        switch(type)
        {
            case 0:
                pan.innerHTML = Ext.Const.FieldProp;
                break;
            case 1:
                pan.innerHTML = Ext.Const.SectionProp;
                break;
            case 2:
                pan.innerHTML = Ext.Const.TabProp;
                break;
            case 3:
                pan.innerHTML = "grid view"; 
                //replace with ext.const.gridviewprop
                break;
            case 4:
                pan.innerHTML = Ext.Const.TemplateFieldProp;
                break;
            case 5:
                pan.innerHTML = Ext.Const.PageProp;
            default:
                break;
        }
    }
}

//Set CSS in inframe object when it's parent  selected or unselected handle is  triggered.
function SetIframeSectionCss(obj,newTab) {
    var currentDom = GetDom(newTab);
    var attachmentObj = currentDom.getElementById("divNewAttachment");
    var attachmentObjList = currentDom.getElementById("iframeAttachmentList");
    if (attachmentObj == null || attachmentObjList==null) {
        return;
    }

    if (typeof (attachmentObj.contentWindow) == 'undefined' || typeof (attachmentObjList.contentWindow) == 'undefined') {
        return;
    }

    //60016 is ID number of attachment section.
    if (obj != null && obj.type == 18 && obj._sectionIdValue.indexOf('60016') > -1) {
        attachmentObj.contentWindow.document.body.bgColor = '#fcfbc9';
        attachmentObjList.contentWindow.document.body.bgColor = '#fcfbc9';
    }
    else {
        attachmentObj.contentWindow.document.body.bgColor = '';
        attachmentObjList.contentWindow.document.body.bgColor = '';
    }
}