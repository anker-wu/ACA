/**
 * <pre>
 * 
 *  Accela Citizen Access Admin
 *  File: fieldProperties.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 * 
 *  Description:
 *  
 *  Notes:
 *      $Id: fieldProperties.js 77905 2008-04-10 12:49:28Z ACHIEVO\jack.feng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

function InsertStr(src,str)
{
    var start = src.toLowerCase().lastIndexOf('style="');
    if(start>=0)
    {
        var pre = src.substring(0,start+7);
        var after = src.substring(start+7);
        return pre + str + after;
    }
    else
    {
        return '<FONT style=" ' + str + '">' + src + "</FONT>";
    }
}

function InsertFontStr(src,str)
{
    var start = src.toUpperCase().indexOf('<FONT');
    if(start>=0)
    {
        var pre = src.substring(0,start+5);
        var after = src.substring(start+5);
        return pre + str + after;
    }
    else
    {
        return '<FONT ' + str + '>' + src + "</FONT>";
    } 
}

// true: browser's type is fireFox
// false: browser's type isn't fireFox
function isFireFox()
{
    if(navigator.userAgent.indexOf("Firefox")>0)
    {
        return true;
    } 
    else
    {
        return false; 
    }  
}

// true: browser's type is Safari
// false: browser's type isn't Safari
function isSafari()
{
    if(navigator.userAgent.indexOf("Safari")>0)
    {
        return true;
    } 
    else
    {
        return false; 
    }  
}

function GetStyleValue(src,regex)
{
    var re = regex.exec(src);
    if(re == null)
    {
        return '';
    }
    return re[0];
}

var  isSupportI18N=IsSupportMultiLanguage();

var cp = new Ext.ColorPalette({
    id:'colorPalette',
    listeners:{
        select :function(colorPan,color)
        {
            var grid = Ext.getCmp('pgHeader');
            grid.store.getById('Admin_FieldProperty_ForeColor').set('value',color);
            ModifyMark();
            
            var colorpan = Ext.getCmp('colorPan');
            if(colorpan)
            {
                colorpan.hide();
            }
        }
    }
});  

var colorWin = new Ext.Window({
    id:'colorPan',
    width:160,
    height:110,
    closeAction:'hide',
    plain: true,
    resizable: false,
    items: [cp],
    listeners:{
        beforehide:function(n){
            isColorWinShow=false;
        }
    }
});

 //properties of header label
function LoadHeader(obj) {
    if (obj.type == 29 && obj._element.attributes["EnableExpand"] != null && obj._element.attributes["EnableExpand"].value == "Y") {
        Accela.ACA.Web.WebService.AdminConfigureService.GetPolicyValueForData4AsKey(Ext.Const.ModuleName, Ext.Constant.IS_EXPANDED_SECTION, obj._labelKeyValue, function (sectionExpandFlag) {
            loadNormalHeader(obj, sectionExpandFlag);
        });
    } else {
        loadNormalHeader(obj, null);
    }
}

function loadNormalHeader(obj, sectionExpandFlag) {
    CloseWindow();

    var selectedRow;
    var selectedCol = -1;

    var store = new Ext.data.SimpleStore({
            fields: [ 'state'], 
            data : [
            ['Arial, Helvetica, sans-serif'],
            ['Times New Roman, Times, serif'],
            ['Georgia, Times New Roman, Times, serif'],
            ['Verdana, Arial, Helvetica, sans-serif'],
            ['Geneva, Arial, Helvetica, sans-serif']
            ]
    });
    
    var applicantStore = new Ext.data.SimpleStore({
        fields:['restrict'],
        data:[
            ['All ACA Users'],
            ['CAP Creator'],
            ['Associated License Professional & CAP Creator']
        ]
    });
    
    var pgHeader = new Ext.grid.PropertyGrid({
        id:'pgHeader',
        closable: true,
        autoHeight: true,
        customEditors:{
            "Default Label":new DisabledGridEditor(),
            "Default Instructions":new DisabledGridEditor(),
            "Text Heading(Default Language)":new DisabledGridEditor(),
            "Text Heading":new Ext.grid.GridEditor(new Ext.form.Field({
                disabled: false,
                disabledClass:'x-DefaultHeading',
          	    cls:"x-DefaultHeading"})),
            "Font-Bold": new BoolComboBoxGridEditor(),
            "Font-Family":new Ext.grid.GridEditor(new Ext.form.ComboBox({
                store: store,
                displayField:'state',
                typeAhead: true,
                mode: 'local',
                triggerAction: 'all',
                emptyText:'Select a font...',
                selectOnFocus:true,
                listeners: {
                    'select': function (combo, rec, idx) {                      
                         Ext.getCmp(combo.container.id).completeEdit();                            
                    }
                }
            }),
            { allowBlur:true })
        },
        listeners: {
            propertychange:function(source,recordId,value,oldValue ){
                //check if text changed
                isTextUpdated=false;
                if(source.Admin_FieldProperty_TextHeading==value){
                    isTextUpdated=true;
                }
                if(selectedCol == 1)
                {
                    if(recordId == 'Admin_FieldProperty_TextHeading')
                    {
                        SetTextHeadingValue(obj,value);
                        UpdateDefaultLangValue(pgHeader, value, 'Admin_FieldProperty_TextHeading_DefaultLanguage');
                    }
                    
                    if(recordId == 'Font-Bold')
                    {
                        //remove the <b> tag
                        var reg=/<b.*?>/gi
                        var regSlash=/<\/b>/gi
                        obj._element.innerHTML = obj._element.innerHTML.replace(reg,'');
                        obj._element.innerHTML = obj._element.innerHTML.replace(regSlash,'');
                        
                        if (Boolean.parse(value)) 
                        {
                            obj._element.innerHTML = '<b style="font-weight:bold;">' + obj._element.innerHTML + '</b>';
                        }
                        else
                        {
                            obj._element.innerHTML = '<b style="font-weight:normal;">' + obj._element.innerHTML + '</b>';
                        }
                    }
                    
                    if(recordId == 'Admin_FieldProperty_Font_Italic')
                    {
                        var reg=/<i.*?>|<\/i>/gi;
                        obj._element.innerHTML = obj._element.innerHTML.replace(reg,'');
                        var fontStyle = Boolean.parse(value) ? 'font-style:italic;' : 'font-style:normal;';
                        
                        // DO NOT use the String.format, it is used the Ajax's string format which parse wrong if the replace string exists '{0}'
                        obj._element.innerHTML = "<i style=\"" + fontStyle + "\">" + obj._element.innerHTML + "</i>";
                    }

                    if(recordId == 'Font-Family')
                    {
                        var reg = /face=[a-zA-Z0-9_\^\$\.\|\{\[\}\]\(\)\*\+\?\\~`!@#%&-=;:'",/\n\s]*"/;
                        obj._element.innerHTML = obj._element.innerHTML.replace(reg,'');
                        if(value != '')
                        {
                            obj._element.innerHTML = InsertFontStr(obj._element.innerHTML,' face="' + value + '"');
                        }
                    }
                    if(recordId == 'Admin_FieldProperty_Font_Size')
                    {
                        var reg_fontsize_px = /font-size\:\s*\d+px/gi;
                        var reg_fontsize_em = /font-size\:\s*\d+(\.\d)?em/gi;
                        if(reg_fontsize_px.test(obj._element.innerHTML)) {
                            obj._element.innerHTML = obj._element.innerHTML.replace(reg_fontsize_px,'');
                        }
                        
                        if(reg_fontsize_em.test(obj._element.innerHTML)) {
                            obj._element.innerHTML = obj._element.innerHTML.replace(reg_fontsize_em,'');
                            //if configured font-size is empty and label element exists class 'FontSizeRestore', it need remove lable element class 'FontSizeRestore'.
                            var className = obj._element.className;
                            if (className != null && className.indexOf('ACA_Label_FontSize_Restore') > -1){
                                obj._element.className = className.replace('ACA_Label_FontSize_Restore', '');
                            }
                            else if (className != null && className.indexOf('FontSizeRestore') > -1){
                                obj._element.className = className.replace('FontSizeRestore', '');
                            }
                        }

                        if(value != '')
                        {
                            var pat=/^\d+$/
                            var chk=pat.test(value);
                            
                            if (!chk && Math.floor(value) != NaN)
                            {
                                pgHeader.store.getById('Admin_FieldProperty_Font_Size').set('value',Math.floor(value));
                            }
                            
                            if (chk) {
                                obj._element.innerHTML = InsertStr(obj._element.innerHTML,' font-size:' + value / 10 + 'em;');
                                
                                 //if element not-exists class 'FontSizeRestore', it need add class 'FontSizeRestore' to lable element.
                                 var className = obj._element.className;
                                 if (className != null && className.indexOf("font", "px") >-1){
                                    obj._element.className += ' ACA_Label_FontSize_Restore';
                                }
                                else if (className != null && className.indexOf('FontSizeRestore') == -1){
                                    obj._element.className += ' FontSizeRestore';
                                }
                            }else{
                                showMessage(Ext.LabelKey.Admin_Message_Title_Warning,Ext.LabelKey.Admin_Message_Number_Check);
                            }
                        }    
                    }
                    if(recordId == 'Admin_FieldProperty_ForeColor')
                    {
                        var reg = /color\=("{0,1})(#){1}([a-fA-F0-9]){6}("{0,1})/i;
                        obj._element.innerHTML = obj._element.innerHTML.replace(reg,'');
                        
                        if(value != null && value != '')
                        {
                            var pat = /^#?([a-fA-F0-9]){6}$/;
                            var chk=pat.test(value);
                            
                            if (chk) {
                                obj._element.innerHTML = InsertFontStr(obj._element.innerHTML,' color="#' + value.replace('#','') + '"');
                            }else{
                                showMessage(Ext.LabelKey.Admin_Message_Title_Warning,Ext.LabelKey.Admin_Message_Color_Check);
                            }
                        }
                    }

                    if(oldValue != value && recordId != 'Width')
                    {
                        // just for tab header change
                        if(obj._moduleNameValue)
                        {
                            obj._updateTabName(obj._element.innerHTML,value,isTextUpdated);
                        }
                        //update more tab in tab bar
                        if (obj._labelKeyValue == "aca_tabbar_more_label")
                        {
                            obj._updateMoreTabName();
                        }
                        //update more link button in tab bar
                        if (obj._labelKeyValue == "aca_tabbar_label_morelink") 
                        {
                            obj._updateMoreLinkButton();
                        }
                        //end
                        var changeItem;
                        var heading = pgHeader.store.getById('Admin_FieldProperty_TextHeading').get('value');
                        if(heading == '')
                        {
                            obj._element.innerHTML = '';
                        }
                        if(obj.type == 0)
                        {  
                            var displayLabel = obj._getLabel();
                               
                            if(obj._labelKeyValue == "aca_user_name")
                            {
                                displayLabel = obj._getLabel().replace("Administrator","{0}");
                            }
                        
                            changeItem = new TextObj(Ext.Const.OpenedId,obj._controlIdValue,obj._labelKeyValue,displayLabel,obj._moduleNameValue);
                            SetPageFlowCode4Item(changeItem);
                        }
                        else
                        {
                            if(recordId == Ext.LabelKey.Admin_FieldProperty_Is_Expanded_Section) {
                               obj._expanded = value;
                               changeItem = new ApplicantObj(Ext.Const.OpenedId,obj._controlIdValue,obj._labelKeyValue,obj._getLabel(),obj._restrictDisplayValue, null, value);
                            } else {
                                changeItem = new ApplicantObj(Ext.Const.OpenedId,obj._controlIdValue,obj._labelKeyValue,obj._getLabel(),obj._restrictDisplayValue);
                            }
                        }
                        
                        changeItems.UpdateItem(0,changeItem);
                        ModifyMark();
                    }
                }
                
                if(recordId == 'Width' && selectedCol == 1)
                {
                    ResponseWidthChange(obj,oldValue,recordId,value,pgHeader);
                }
            },
            beforeedit: function(e) {
                var record = e.grid.store.getAt(e.row);
                if (record.id == 'Section Instructions') {
                    e.cancel = true;
                    if (!isWinShow) {
                        PopupHtmlEditor(e.grid, obj._getSubLabel());
                        htmlEditorTargetType = htmlEditorTargetTypes.Section;
                        htmlEditorTargetCtrl = obj;
                    }
                }
            },
            cellclick: function(grid, rowIndex, columnIndex, e) {
                selectedCol = columnIndex;
                var cell = grid.getBox();
                var record = grid.store.getAt(rowIndex);
                if(columnIndex == 1)
                {
                    if(record.id == 'Admin_FieldProperty_ForeColor')
                    {
                        var top = cell.y + cell.height;
                        var left = cell.x + cell.width - colorWin.width;
                        colorWin.setPosition(left,top);
                        try
                        {
                            colorWin.show();
                            isColorWinShow=true;
                        }
                        catch(err)
                        {
                        }
                    }
                }
                else
                {
                    selectedCol = 1;
                    if(record.id == 'Admin_FieldProperty_ForeColor')
                    {
                        var top = cell.y + cell.height;
                        var left = cell.x + cell.width - colorWin.width;
                        colorWin.setPosition(left,top);
                        try
                        {
                            colorWin.show();
                            isColorWinShow=true;
                        }
                        catch(err)
                        {
                        }
                    }
                }
            }
        }
    });    

    pgHeader.customEditors[Ext.LabelKey.Admin_FieldProperty_Font_Italic] = new BoolComboBoxGridEditor();

    pgHeader.store.sortInfo = null;
    if(obj.type == 0 || obj.type == 25)
    {
        var defaultLanLabel = Ext.Const.IsSupportMultiLang? "'Admin_FieldProperty_TextHeading_DefaultLanguage':''," : '';
        var isHeadLabel = obj._viewElementIdValue != null;
        var gridHeadLabel = isHeadLabel? ",'Width':''" : '';    
        eval("pgHeader.setSource({'Default Label': ''," + defaultLanLabel + "'Admin_FieldProperty_TextHeading': ''" + (obj.type == 0 ? ",'Font-Bold': '','Admin_FieldProperty_Font_Italic': false,'Font-Family':'','Admin_FieldProperty_Font_Size': '','Admin_FieldProperty_ForeColor': ''" : "") + gridHeadLabel + "});");
        
        if(obj._labelKeyValue == "aca_user_name")
        {
            pgHeader.customEditors["Text Heading"].field.disable();
            pgHeader.customEditors["Text Heading"].disabledClass = 'x-DefaultHeading';
        }
    }
    else
    {
        var store = {};
        store['Default Label'] = '';
        
        if(Ext.Const.IsSupportMultiLang)
        {
           store['Admin_FieldProperty_TextHeading_DefaultLanguage'] = '';
        }
        
        store['Admin_FieldProperty_TextHeading'] = '';
        store['Font-Bold'] = '';
        store['Admin_FieldProperty_Font_Italic'] = false;
        store['Font-Family'] = '';
        store['Admin_FieldProperty_Font_Size'] = '';
        store['Admin_FieldProperty_ForeColor'] = '';
        
        if(obj.type == 28 || obj.type == 29 || obj.type == 38)  // sub section or section title
        {
            store['Section Instructions'] = Ext.LabelKey.Admin_ClickToEdit_Watermark;
        }
        pgHeader.setSource(store);
        
    }
          
    if (obj.type == 29 && obj._element.attributes["EnableExpand"] != null && obj._element.attributes["EnableExpand"].value == "Y") {
        pgHeader.customEditors[Ext.LabelKey.Admin_FieldProperty_Is_Expanded_Section] = new BoolComboBoxGridEditor();
        store[Ext.LabelKey.Admin_FieldProperty_Is_Expanded_Section] = false;
        pgHeader.setSource(store);
        var expanded = false;

        if (typeof(obj._expanded) != 'undefined') {
            expanded = obj._expanded;
        } else if(sectionExpandFlag == "Y" 
            || ((obj._labelKeyValue=="ins_inspectionList_label_inspection" 
                || obj._labelKeyValue=="per_permitDetail_label_workLocation" 
                || obj._labelKeyValue=="per_permitDetail_label_detail") && sectionExpandFlag == "")){
            // inspection, work location, detail section is expand by default
            expanded = true;
        }
        
        pgHeader.store.getById(Ext.LabelKey.Admin_FieldProperty_Is_Expanded_Section).set('name', "Expanded");
        pgHeader.store.getById(Ext.LabelKey.Admin_FieldProperty_Is_Expanded_Section).set('value', expanded);
    }

    pgHeader.getColumnModel().setConfig([
          {header: 'Name',sortable: false,dataIndex:'name', id: 'name',renderer:RenderDisabled},
          {header: 'Value',sortable: false,dataIndex: 'value', id: 'value'}
        ]);

    function RenderDisabled(value)
    {
         return "<span style='color:green;'>Test</span>";
    }
        
    RenderGrid(0,pgHeader);
    //GetLabelKey
    //TODO
    
    pgHeader.store.getById('Admin_FieldProperty_TextHeading').set('name',Ext.LabelKey.Admin_FieldProperty_TextHeading);
    
    if(obj.type != 25)
    {
        //pgHeader.store.getById('Admin_FieldProperty_Font_Bold').set('name',Ext.LabelKey.Admin_FieldProperty_Font_Bold);
        pgHeader.store.getById('Admin_FieldProperty_Font_Italic').set('name',Ext.LabelKey.Admin_FieldProperty_Font_Italic);
        //pgHeader.store.getById('Admin_FieldProperty_Font_Names').set('name',Ext.LabelKey.Admin_FieldProperty_Font_Names);
        pgHeader.store.getById('Admin_FieldProperty_Font_Size').set('name',Ext.LabelKey.Admin_FieldProperty_Font_Size);
        pgHeader.store.getById('Admin_FieldProperty_ForeColor').set('name',Ext.LabelKey.Admin_FieldProperty_ForeColor);
    }

    if(Ext.Const.IsSupportMultiLang){
         var defaultLangValue = GetDefaultLangValue(obj);
         var defaultLangCell = pgHeader.store.getById('Admin_FieldProperty_TextHeading_DefaultLanguage');
         defaultLangCell.set('name',Ext.LabelKey.Admin_FieldProperty_TextHeading+'(Default Language)');
         defaultLangCell.set('value', RemoveLabelStyle(defaultLangValue));
    }
    
    if(isHeadLabel)
    {
        pgHeader.store.getById('Width').set('value',obj._getColumnWidth());
    }
    if((obj._element.innerHTML.indexOf('<font')<0) && (obj._element.innerHTML.indexOf('<FONT')<0))
    {
        obj._element.innerHTML = '<font style=" text-align:left;">' + obj._element.innerHTML + '</font>';
    }
    
    pgHeader.store.getById('Default Label').set('value',DecodeHTMLTag(obj._defaultLabelValue));
    pgHeader.store.getById('Admin_FieldProperty_TextHeading').set('value',obj._element.innerText?obj._element.innerText:obj._element.textContent);
    //if item is more tab, the default value is false
    //'font-weight:normal;' and 'font-weight:bold;' must be same with onchange evet set the label value
    var regFontWeightNormal = /(font-weight:\s*normal)/gi;
    var regFontWeightBold = /(font-weight:\s*bold)|(<b>)/gi;
    if(obj.type != 25 && (obj._element.innerHTML.toLowerCase().indexOf('lighter') > 0 || regFontWeightNormal.test(obj._element.innerHTML)))
    {
        pgHeader.store.getById('Font-Bold').set('value',false);
    }
    else if (regFontWeightBold.test(obj._element.innerHTML)) 
    {
        pgHeader.store.getById('Font-Bold').set('value',true);
    }
    
    var regFontStyle = /(font-style:\s*italic)|(<i>)/gi;
    //if the default style for label is italic(ACA_TabRow_Italic) and user has do no change for it...
    var isOriginalItalic = /ACA_TabRow_Italic/.test(obj._element.className) && !(/(font-style:\s*normal)/gi.test(obj._element.innerHTML));
    
    if(obj.type != 25 && (regFontStyle.test(obj._element.innerHTML) || isOriginalItalic))
    {
        pgHeader.store.getById('Admin_FieldProperty_Font_Italic').set('value',true);
    }
    
    if (obj.type != 25 && ((obj._element.innerHTML.indexOf('SIZE') >= 0) || (obj._element.innerHTML.indexOf('size') >= 0)))
    {
        var reg_fontsize_px = /font-size\:\s*\d+px/i;
        var reg_fontsize_em = /font-size\:\s*\d+(\.\d)?em/i;
        
        var isPx = reg_fontsize_px.test(obj._element.innerHTML);
        var isPm = reg_fontsize_em.test(obj._element.innerHTML);
        var fontsize = '';
        var fontsize_number='';
        if(isPx) {
            fontsize = GetStyleValue(obj._element.innerHTML,reg_fontsize_px);
            fontsize_number=fontsize.match(/\d+/)[0];
        }
        else if(isPm){
            fontsize = GetStyleValue(obj._element.innerHTML,reg_fontsize_em);
            fontsize_number=fontsize.match(/\d+(\.\d)?/)[0];
            fontsize_number=parseFloat(fontsize_number)*10;
        }

        if(/^\d+$/.test(fontsize_number+'')) {
            pgHeader.store.getById('Admin_FieldProperty_Font_Size').set('value',fontsize_number);
        }
    }
    if (obj.type != 25 && ((obj._element.innerHTML.indexOf('face') >= 0) || (obj._element.innerHTML.indexOf('FACE') >= 0)))
    {
        var fontnames = '';
        if (isFireFox())
        {
            fontnames = GetStyleValue(obj._element.innerHTML,new RegExp(/face=[a-zA-Z0-9_\^\$\.\|\{\[\}\]\(\)\*\+\?\\~`!@#%&-=;:'",/\n\s]*"/));
            fontnames = fontnames.replace('face=','');
            fontnames = fontnames.replace('\\','');
            fontnames = fontnames.replace('"','');
            pgHeader.store.getById('Font-Family').set('value',fontnames);
        }
        else
        {
            fontnames = GetStyleValue(obj._element.innerHTML,new RegExp(/face=[a-zA-Z0-9_\^\$\.\|\{\[\}\]\(\)\*\+\?\\~`!@#%&-=;:'",/\n\s]*"/));
            fontnames = fontnames.replace('face=','');
            fontnames = fontnames.replace('\\','');
            fontnames = fontnames.replace('"','');
            pgHeader.store.getById('Font-Family').set('value',fontnames);
        }
    }
    if (obj.type != 25 && ((obj._element.innerHTML.indexOf('color') >= 0) || (obj._element.innerHTML.indexOf('COLOR') >= 0)))
    {
        var fontcolor = '';
        if (isFireFox() || isSafari())
        {
            fontcolor = GetStyleValue(obj._element.innerHTML,new RegExp(/color\=(")(#){1}([a-fA-F0-9]){6}/));
            fontcolor = fontcolor.replace('color="','');
            fontcolor = fontcolor.replace(' ','');
            pgHeader.store.getById('Admin_FieldProperty_ForeColor').set('value',fontcolor);
        }
        else
        {
            fontcolor = GetStyleValue(obj._element.innerHTML,new RegExp(/color\=(#){1}([a-fA-F0-9]){6}/));
            fontcolor = fontcolor.replace('color=','');
            fontcolor = fontcolor.replace(' ','');
            pgHeader.store.getById('Admin_FieldProperty_ForeColor').set('value',fontcolor);
        }
    }

    if(obj._defaultSubLabelValue)
    {
        pgHeader.store.getById('Default Instructions').set('value',obj._defaultSubLabelValue);
    }
};

//proerties of the body label
function LoadBody(obj){

    CloseWindow();

    var pgBody = new Ext.grid.PropertyGrid({
        id:'propertyGrid',
        closable: true,
        autoEncode:true,
        autoHeight: true,
        listeners: {
            propertychange:function(source,recordId,value,oldValue ){
                if(oldValue != null && oldValue != '')
                {
                    if(recordId == 'Admin_FieldProperty_BodyText')
                    {
                        if(isWinShow)
                        {
                        }
                        else
                        {
                            obj._setLabel(value);
                        }
                    }
                    if( !isWinShow && oldValue != value)
                    {
                        var changeItem = new ButtonObj(Ext.Const.OpenedId,obj._controlIdValue,obj._labelKeyValue,value);
                        SetPageFlowCode4Item(changeItem);
                        
                        changeItems.UpdateItem(2,changeItem);
                        ModifyMark();
                    }
                }
            },
            beforeedit: function(e) {
                e.cancel = true;
                if(!isWinShow)
                {
                    var cell = e.grid.getBox();
                    var record = e.grid.store.getAt(e.row);
                    if(record.id == 'Admin_FieldProperty_BodyText')
                    {
                        PopupHtmlEditor(e.grid, obj._getLabel());
                        htmlEditorTargetCtrl = obj;
                        htmlEditorTargetType = htmlEditorTargetTypes.BodyText;
                    }
                }
            },
            cellclick: function(grid, rowIndex, columnIndex, e){
                DisabledHtmlEditorButtons(true);
            }
        }
    });    
    
    // if report link, Load reports
    if(obj._labelKeyValue == "aca_report_label"){
        var array = Ext.Const.OpenedId.split(Ext.Const.SplitChar);
        var pageID = array[0].substring(array[0].length - 4); // The last 4 numbers is page id.
         
        GetReportsByPageID(Ext.Const.ModuleName,pageID);
    }

    obj.fieldLabelId = 'Admin_FieldProperty_BodyText';
    obj.fieldLabelName = Ext.LabelKey.Admin_FieldProperty_BodyText;
    BindPropertyGrid(obj, pgBody);
    
    RenderGrid(0,pgBody);
};

//properties of the input control
function LoadInput(obj){
    CloseWindow();

    var selectCol = -1; 
    var pgInput = new Ext.grid.PropertyGrid({
        closable: true,
        autoHeight: true,
        listeners:{
            propertychange:function(source,recordId,value,oldValue ){
                if(selectCol == 1)
                {
                    if(recordId == 'Admin_FieldProperty_FieldLabel')
                    {
                        obj._setLabel(EncodeHTMLTag(value));
                        UpdateDefaultLangValue(pgInput, value);
                    }
                    if(recordId == 'Admin_FieldProperty_WatermarkText')
                    {
                        obj._setWatermarkText('watermarkText', value);
                    }
                    
                    if(recordId == 'Admin_FieldProperty_WatermarkText1')
                    {
                        obj._setWatermarkText('watermarkText1', value);
                    }
                    
                    if(recordId == 'Admin_FieldProperty_WatermarkText2')
                    {
                        obj._setWatermarkText('watermarkText2', value);
                    }

                    if(recordId == 'Admin_FieldProperty_Instructions')
                    {
                        obj._setSubLabel(EncodeHTMLTag(value));
                    }
                    if(oldValue != value)
                    {
		    	// For range text box, there are two input area. So add two watermark setting.
                        var changeItem = null;
                        if(obj._elementTypeValue == "AccelaRangeNumberText") {
                            changeItem = new InputObj(Ext.Const.OpenedId,obj._controlIdValue,obj._labelKeyValue,obj._getLabel(),obj.get_IsTemplateField(),obj.get_TemplateAttribute(),obj._labelKeyValue + '|sub',obj._getSubLabel(),obj._labelKeyValue + '|watermark',obj._getWatermarkText("watermarkText"),obj._labelKeyValue + '|watermark1',obj._getWatermarkText("watermarkText1"),obj._labelKeyValue + '|watermark2',obj._getWatermarkText("watermarkText2"));
                        } else {
                            changeItem = new InputObj(Ext.Const.OpenedId,obj._controlIdValue,obj._labelKeyValue,obj._getLabel(),obj.get_IsTemplateField(),obj.get_TemplateAttribute(),obj._labelKeyValue + '|sub',obj._getSubLabel(),obj._labelKeyValue + '|watermark',obj._getWatermarkText("watermarkText"));    
                        }
                        
                        changeItems.UpdateItem(1,changeItem);
                        ModifyMark();
                    }
                }
            },
            beforeedit: function(e) {
                var record = e.grid.store.getAt(e.row);
                if (record.id == 'Admin_FieldProperty_Instructions') {
                    e.cancel = true;
                    if (!isWinShow) {
                        PopupHtmlEditor(e.grid, obj._getSubLabel());
                        htmlEditorTargetType = htmlEditorTargetTypes.Field;
                        htmlEditorTargetCtrl = obj;
                    }
                }
            },
            cellclick: function(grid, rowIndex, columnIndex, e) {
                selectCol = columnIndex;
            }
        }
    });
        
    obj.needInstruction = true; 
    if (obj._elementTypeValue != 'AccelaRadioButtonList' && obj._elementTypeValue != 'AccelaCheckBoxList' && obj._elementTypeValue != 'AccelaListBox' && obj._elementTypeValue != 'AccelaAKA'){
        if (obj._elementTypeValue != 'AccelaMultipleControl' || $(obj._element).attr('ControlType') != 'DropdownList') {
            obj.needWatermarkText = true;
        }
    }

    if (obj._elementTypeValue == 'AccelaMultipleControl' && $(obj._element).attr('ControlType') != 'DropdownList') {
        obj.needWatermarkText = true;
    }
    
    if(obj._labelKeyValue == "aca_proxy_personnote")
    {
        BindWaterMarkInput(obj, pgInput);
    }
    else
    {
        BindPropertyGrid(obj, pgInput);
    }
    
    RenderGrid(0,pgInput);
};

//properties of the readonly label for template field. -james.shi add
function LoadReadonly(obj){
    CloseWindow();
    
    var pgReadonly = new Ext.grid.PropertyGrid({
        closable: true,
        autoHeight: true,
        customEditors: {
            'Field Label': new Ext.grid.GridEditor(new Ext.form.Field({
                disabled: true,
                disabledClass:'x-DefaultHeading',
          	    cls:"x-DefaultHeading"}))
        }
    });
    
    pgReadonly.getColumnModel().setConfig([
        {header: 'Name',sortable: false,dataIndex:'name', id: 'name'},
        {header: 'Value',sortable: false,dataIndex: 'value', id: 'value'}
    ]);
    
    pgReadonly.store.sortInfo = null;
    pgReadonly.setSource({
        'Admin_FieldProperty_FieldLabel': ''
    });
    
    pgReadonly.store.getById('Admin_FieldProperty_FieldLabel').set('name', Ext.LabelKey.Admin_FieldProperty_FieldLabel);
    
    RenderGrid(0, pgReadonly);
    
    pgReadonly.store.getById('Admin_FieldProperty_FieldLabel').set('value',DecodeHTMLTag(obj._getLabel()));
}

//properties of the input control
function LoadAutoFillCityInput(obj){
    CloseWindow();
    var positon;
    var selectCol = -1; 
    
    var moduleName = Ext.Const.ModuleName;
    
    Accela.ACA.Web.WebService.AdminConfigureService.GetPolicyValueForData4AsKey(moduleName,Ext.Constant.AUTOFILL_CITY_ENABLED,obj._positionValue, function GetConfigureUrlCallback(callBackAutoFill){
        
        var autoFillCity = false;
        
        if(typeof(obj.AutoFillCity) != 'undefined')
        {
            autoFillCity = obj.AutoFillCity;
        }
        else if(callBackAutoFill == "Y")
        {
            autoFillCity = true;
        } 

        var pgInput = new Ext.grid.PropertyGrid({
            closable: true,
            autoHeight: true,
            listeners:{
                propertychange:function(source,recordId,value,oldValue ){
                    if(selectCol == 1)
                    {
                        if(recordId == 'Admin_FieldProperty_FieldLabel')
                        {
                            obj._setLabel(EncodeHTMLTag(value));
                            UpdateDefaultLangValue(pgInput, value);
                        }
                        if(recordId == 'Admin_FieldProperty_WatermarkText')
                        {
                            obj._setWatermarkText("watermarkText",value);
                        }
                        if(recordId == 'Admin_FieldProperty_Instructions')
                        {
                            obj._setSubLabel(EncodeHTMLTag(value));
                        }
                        if(oldValue != value)
                        {                            
                            if(recordId == 'Admin_FieldProperty_AutoFill')
                            {
                              autoFillCity = value;
                              obj.AutoFillCity = value;
                              positon = obj._positionValue;
                            }

                            var changeItem = new InputObj(Ext.Const.OpenedId,obj._controlIdValue,obj._labelKeyValue,obj._getLabel(),obj.get_IsTemplateField(),obj.get_TemplateAttribute(),obj._labelKeyValue + '|sub',obj._getSubLabel(),obj._labelKeyValue + '|watermark',obj._getWatermarkText("watermarkText"),obj._labelKeyValue + '|watermark1',obj._getWatermarkText("watermarkText1"),obj._labelKeyValue + '|watermark2',obj._getWatermarkText("watermarkText2"),autoFillCity,Ext.Constant.AUTOFILL_CITY_ENABLED,positon);
                            changeItems.UpdateItem(1,changeItem);
                            ModifyMark();
                        }
                    }
                },
                beforeedit: function(e) {
                    var record = e.grid.store.getAt(e.row);
                    if (record.id == 'Admin_FieldProperty_Instructions') {
                        e.cancel = true;
                        if (!isWinShow) {
                            PopupHtmlEditor(e.grid, obj._getSubLabel());
                            htmlEditorTargetType = htmlEditorTargetTypes.Field;
                            htmlEditorTargetCtrl = obj;
                        }
                    }
                },
                cellclick: function(grid, rowIndex, columnIndex, e) {
                    selectCol = columnIndex;
                }
            }
        });
            
        obj.needInstruction = true;
        obj.needWatermarkText=true;
        obj.needAutoFill = true;
        obj.autoFillValue = autoFillCity;
        BindPropertyGrid(obj, pgInput);
        RenderGrid(0,pgInput);
    })
};

//properties of the input control
function LoadAutoFillStateInput(obj){
    CloseWindow();
    var positon;
    var selectCol = -1; 
    
    var moduleName = Ext.Const.ModuleName;
    
    Accela.ACA.Web.WebService.AdminConfigureService.GetPolicyValueForData4AsKey(moduleName,Ext.Constant.AUTOFILL_STATE_ENABLED,obj._positionValue, function GetConfigureUrlCallback(callBackAutoFill){
        var autoFillState = false;
        
        if(typeof(obj.AutoFillStateValue) != 'undefined')
        {
            autoFillState = obj.AutoFillStateValue;
        }
        else if(callBackAutoFill == "Y")
        {
            autoFillState = true;
        } 

        var pgInput = new Ext.grid.PropertyGrid({
            closable: true,
            autoHeight: true,
            listeners:{
                propertychange:function(source,recordId,value,oldValue ){
                    if(selectCol == 1)
                    {
                        if(recordId == 'Admin_FieldProperty_FieldLabel')
                        {
                            obj._setLabel(EncodeHTMLTag(value));
                            UpdateDefaultLangValue(pgInput, value);
                        }
                        if(recordId == 'Admin_FieldProperty_WatermarkText')
                        {
                            obj._setWatermarkText("watermarkText", value);
                        }
                        if(recordId == 'Admin_FieldProperty_Instructions')
                        {
                            obj._setSubLabel(EncodeHTMLTag(value));
                        }
                        if(oldValue != value)
                        {                           
                            if(recordId == 'Admin_FieldProperty_AutoFill')
                            {
                              autoFillState = value;
                              positon = obj._positionValue;
                              obj.AutoFillStateValue = value;
                            }
                            var changeItem = new InputObj(Ext.Const.OpenedId,obj._controlIdValue,obj._labelKeyValue,obj._getLabel(),obj.get_IsTemplateField(),obj.get_TemplateAttribute(),obj._labelKeyValue + '|sub',obj._getSubLabel(),obj._labelKeyValue + '|watermark',obj._getWatermarkText("watermarkText"),obj._labelKeyValue + '|watermark1',obj._getWatermarkText("watermarkText1"),obj._labelKeyValue + '|watermark2',obj._getWatermarkText("watermarkText2"),autoFillState,Ext.Constant.AUTOFILL_STATE_ENABLED,positon);
                            changeItems.UpdateItem(1,changeItem);
                            ModifyMark();
                        }
                    }
                },
                beforeedit: function(e) {
                    var record = e.grid.store.getAt(e.row);
                    if (record.id == 'Admin_FieldProperty_Instructions') {
                        e.cancel = true;
                        if (!isWinShow) {
                            PopupHtmlEditor(e.grid, obj._getSubLabel());
                            htmlEditorTargetType = htmlEditorTargetTypes.Field;
                            htmlEditorTargetCtrl = obj;
                        }
                    }
                },
                cellclick: function(grid, rowIndex, columnIndex, e) {
                    selectCol = columnIndex;
                }
            }
        });
            
        obj.needInstruction = true;
        obj.needWatermarkText=true;
        obj.needAutoFill = true;
        obj.autoFillValue = autoFillState;
        BindPropertyGrid(obj, pgInput);
        RenderGrid(0,pgInput);
    })
};

//properties of the checkbox control
function LoadCheck(obj){
    CloseWindow();

    var selectCol = -1;
    var pgCheck = new Ext.grid.PropertyGrid({
        closable: true,
        autoHeight: true,
        listeners:{
            propertychange:function(source,recordId,value,oldValue ){
                if(selectCol == 1)
                {
                    if(recordId == 'Admin_FieldProperty_FieldLabel')
                    {
                        obj._setLabel(EncodeHTMLTag(value));
                        UpdateDefaultLangValue(pgCheck, value);
                    }
                    if(recordId == 'Admin_FieldProperty_Instructions')
                    {
                        obj._setSubLabel(EncodeHTMLTag(value));
                    }
                    if(oldValue != value)
                    {
                        var changeItem = new InputObj(Ext.Const.OpenedId,obj._controlIdValue,obj._labelKeyValue,obj._getLabel(),obj.get_IsTemplateField(),obj.get_TemplateAttribute(),obj._labelKeyValue + '|sub',obj._getSubLabel());
                        changeItems.UpdateItem(1,changeItem);
                        ModifyMark();
                    }
                }
            },
            beforeedit: function(e) {
                var record = e.grid.store.getAt(e.row);
                if (record.id == 'Admin_FieldProperty_Instructions') {
                    e.cancel = true;
                    if (!isWinShow) {
                        PopupHtmlEditor(e.grid, obj._getSubLabel());
                        htmlEditorTargetType = htmlEditorTargetTypes.Field;
                        htmlEditorTargetCtrl = obj;
                    }
                }
            },
            cellclick: function(grid, rowIndex, columnIndex, e) {
                selectCol = columnIndex;
            }
        }
    });

    obj.needInstruction = true;
    BindPropertyGrid(obj, pgCheck);

    RenderGrid(0,pgCheck);
};

//properties of the hyperlink control
function LoadHyperLink(obj){
    CloseWindow();

    var selectCol = -1;
    var pgHyperLink = new Ext.grid.PropertyGrid({
        closable: true,
        autoHeight: true,
        listeners:{
            propertychange:function(source,recordId,value,oldValue ){
                if(selectCol == 1)
                {
                    if(recordId == 'Admin_FieldProperty_HyperLinkText')
                    {
                        obj._setLabel(EncodeHTMLTag(value));
                        UpdateDefaultLangValue(pgHyperLink, value, 'Admin_FieldProperty_HyperLinkText_DefaultLanguage');
                    }
                    if(oldValue != value)
                    {
                        var changeItem = new ButtonObj(Ext.Const.OpenedId,obj._controlIdValue,obj._labelKeyValue,obj._getLabel(),obj._moduleNameValue);
                        changeItems.UpdateItem(2,changeItem);
                        ModifyMark();
                    }
                }
            },
            cellclick: function(grid, rowIndex, columnIndex, e) {
                selectCol = columnIndex;
            }
        }
    });

    obj.fieldLabelId = "Admin_FieldProperty_HyperLinkText_DefaultLanguage";
    obj.fieldLabelName = Ext.LabelKey.Admin_FieldProperty_HyperLinkText;
    BindPropertyGrid(obj, pgHyperLink);
    
    RenderGrid(0,pgHyperLink);
};

//properties of the button control
function LoadButton(obj,enableRecordType){
    CloseWindow();
    
    if(enableRecordType)
    {
        LoadFilterButton(obj);
    }
    else
    {
        LoadButtonWithoutFilter(obj)
    }
};

//properties of the button which have report proprety
function LoadReportButton(obj){
    CloseWindow();

    var pgReportButton = new Ext.grid.PropertyGrid({
        closable: true,
        autoHeight: true,
        customEditors: {
             'Report': new Ext.grid.GridEditor(new Ext.form.Field({
               id:'reportFld',
               readOnly:true,
               style:'color:Blue;text-decoration:underline'})),
             'Default Label': new Ext.grid.GridEditor(new Ext.form.Field({
                disabled:true,
          	    disabledClass:'x-DefaultHeading',
          	    cls:"x-DefaultHeading"})),
          	 'Button Label(Default Language)': new Ext.grid.GridEditor(new Ext.form.Field({
                disabled:true,
          	    disabledClass:'x-DefaultHeading',
          	    cls:"x-DefaultHeading"})),
          	 'Default Instructions': new Ext.grid.GridEditor(new Ext.form.Field({
                disabled:true,
          	    disabledClass:'x-DefaultHeading',
          	    cls:"x-DefaultHeading"}))
        },
        listeners:{
            propertychange:function(source,recordId,value,oldValue ){
                if(oldValue != null && oldValue != '')
                {
                    if(recordId == 'Admin_FieldProperty_ButtonLabel')
                    {
                        obj._setLabel(EncodeHTMLTag(value));
                        UpdateDefaultLangValue(pgReportButton, value, 'Admin_FieldProperty_ButtonLabel_DefaultLanguage');
                    }
                    if(oldValue != value)
                    {
                        var changeItem = new ButtonObj(Ext.Const.OpenedId,obj._controlIdValue,obj._labelKeyValue,obj._getLabel());
                        SetPageFlowCode4Item(changeItem);
                        changeItems.UpdateItem(2,changeItem);
                        ModifyMark();
                    }
                }
            },
            beforeedit: function(e) {
                var record = e.grid.store.getAt(e.row);
                if (record.id == 'Admin_FieldProperty_Instructions') {
                    e.cancel = true;
                    if (!isWinShow) {
                        PopupHtmlEditor(e.grid, obj._getSubLabel());
                        htmlEditorTargetType = htmlEditorTargetTypes.Field;
                        htmlEditorTargetCtrl = obj;
                    }
                }
            },
            cellclick:function(grid, rowIndex, columnIndex, e){
                var record = grid.store.getAt(rowIndex);
                if(record.id == 'Report')
                {
                    if(V360URL != '')
                    {
                        window.open(V360URL + '/portlets/reports/reportsMain.jsp', null, 'status:no;resizable:yes;dialogWidth:800px;dialogHeight:545px');
                    }
                }
            }
        }
    });
      
    pgReportButton.store.sortInfo = null;
    
    if(Ext.Const.IsSupportMultiLang)
    {
         pgReportButton.setSource({
            'Default Label': '',
            'Admin_FieldProperty_ButtonLabel_DefaultLanguage':'',
            'Admin_FieldProperty_ButtonLabel': '',
            'Report':''
        });
    }
    else
    {
        pgReportButton.setSource({
            'Default Label': '',
            'Admin_FieldProperty_ButtonLabel': '',
            'Report':''
        });
    }
    pgReportButton.getColumnModel().setConfig([
          {header: 'Name',sortable: false,dataIndex:'name', id: 'name'},
          {header: 'Value',sortable: false,dataIndex: 'value', id: 'value'}
        ]);
    
    pgReportButton.store.getById('Admin_FieldProperty_ButtonLabel').set('name',Ext.LabelKey.Admin_FieldProperty_ButtonLabel);
    
    if(Ext.Const.IsSupportMultiLang)
    {
         pgReportButton.store.getById('Admin_FieldProperty_ButtonLabel_DefaultLanguage').set('name',Ext.LabelKey.Admin_FieldProperty_ButtonLabel+'(Default Language)');
    }
    
    pgReportButton.store.getById('Default Label').set('value',DecodeHTMLTag(obj._defaultLabelValue));
    pgReportButton.store.getById('Admin_FieldProperty_ButtonLabel').set('value',DecodeHTMLTag(obj._getLabel()));
    pgReportButton.store.getById('Report').set('value','Configure Report(s)');
    if(Ext.Const.IsSupportMultiLang)
    {
       var defaultLangValue = GetDefaultLangValue(obj);
       pgReportButton.store.getById('Admin_FieldProperty_ButtonLabel_DefaultLanguage').set('value',DecodeHTMLTag(defaultLangValue));
    }

    RenderGrid(0,pgReportButton);
};

//properties of radio button control
function LoadRadio(obj){
    CloseWindow();

    var selectCol = -1;
    var pgRadio = new Ext.grid.PropertyGrid({
        closable: true,
        autoHeight: true,
        listeners:{
            propertychange:function(source,recordId,value,oldValue ){
                if(selectCol == 1)
                {
                    if(recordId == 'Admin_FieldProperty_FieldLabel')
                    {
                        obj._setLabel(EncodeHTMLTag(value));
                        UpdateDefaultLangValue(pgRadio, value);
                    }
                    if(oldValue != value)
                    {
                        var changeItem = new ButtonObj(Ext.Const.OpenedId,obj._controlIdValue,obj._labelKeyValue,obj._getLabel());
                        changeItems.UpdateItem(2,changeItem);
                        ModifyMark();
                    }
                }
            },
            cellclick: function(grid, rowIndex, columnIndex, e) {
                selectCol = columnIndex;
            }
        }
    });

    BindPropertyGrid(obj, pgRadio);
    
    RenderGrid(0,pgRadio);
};

function LoadLogo(obj){
    CloseWindow();
    
    var selectCol = -1;
    var pgLogo = new Ext.grid.PropertyGrid({
        closable: true,
        autoHeight: true,
        listeners:{
            propertychange:function(source,recordId,value,oldValue ){
                if(selectCol == 1)
                {
                    if(recordId == 'Admin_Logo_Display')
                    {
                        obj._clientVisibleValue = value;
                    }
                    if(oldValue != value)
                    {
                        var changeItem = new ApplicantObj(Ext.Const.OpenedId,obj._controlIdValue,obj._labelKeyValue,obj._getLabel(),null,obj._clientVisibleValue);
                        changeItems.UpdateItem(0,changeItem);
                        ModifyMark();
                    }
                }
            },
            cellclick: function(grid, rowIndex, columnIndex, e) {
                selectCol = columnIndex;
            }
        }
    });
    pgLogo.store.sortInfo = null;
    pgLogo.setSource({
        'Admin_Logo_Display':false
    });
    
    pgLogo.getColumnModel().setConfig([
          {header: 'Name',sortable: false,dataIndex:'name', id: 'name'},
          {header: 'Value',sortable: false,dataIndex: 'value', id: 'value'}
        ]);
    
    pgLogo.store.getById('Admin_Logo_Display').set('name',Ext.LabelKey.Admin_Logo_Display);

    RenderGrid(0,pgLogo);

    pgLogo.store.getById('Admin_Logo_Display').set('value',obj._clientVisibleValue);
};

function LoadButtonWithoutFilter(obj){
    var selectedCol = -1;
    var pgButton = new Ext.grid.PropertyGrid({
        closable: true,
        autoHeight: true,
        customEditors: {
            'Default Label': new DisabledGridEditor(),
            'Button Label(Default Language)': new DisabledGridEditor()
        },
        listeners:{
            propertychange:function(source,recordId,value,oldValue ){
                if(selectedCol == 1)
                {
                    if(recordId == 'Admin_FieldProperty_Instructions')
                    {
                        obj._setSubLabel(EncodeHTMLTag(value));
                    }

                    ResponseWidthChange(obj,oldValue,recordId,value,pgButton); 
                }
            },
            cellclick: function(grid, rowIndex, columnIndex, e) {
               selectedCol = columnIndex;
               DisabledHtmlEditorButtons(true);
            },
            beforeedit: function(e) {
                var record = e.grid.store.getAt(e.row);
                if (record.id == 'Admin_FieldProperty_Instructions') {
                    e.cancel = true;
                    if (!isWinShow) {
                        PopupHtmlEditor(e.grid, obj._getSubLabel());
                        htmlEditorTargetType = htmlEditorTargetTypes.Field;
                        htmlEditorTargetCtrl = obj;
                    }
                }
            }
        }        
    });
   
    pgButton.store.sortInfo = null;
    var defaultLanLabel = Ext.Const.IsSupportMultiLang? "'Admin_FieldProperty_ButtonLabel_DefaultLanguage':''," : '';
    var isHeadLabel = obj._elementTypeValue == 'GridViewHeaderLabel';
    var gridHeadLabel = isHeadLabel? ",'Width':''" : '';  
    eval("pgButton.setSource({'Default Label': ''," + defaultLanLabel + "'Admin_FieldProperty_ButtonLabel': ''" + gridHeadLabel + "});");
    pgButton.getColumnModel().setConfig([
          {header: 'Name',sortable: false,dataIndex:'name', id: 'name'},
          {header: 'Value',sortable: false,dataIndex: 'value', id: 'value'}
        ]);
    
    pgButton.store.getById('Admin_FieldProperty_ButtonLabel').set('name',Ext.LabelKey.Admin_FieldProperty_ButtonLabel);
    if(Ext.Const.IsSupportMultiLang)
    {
         pgButton.store.getById('Admin_FieldProperty_ButtonLabel_DefaultLanguage').set('name',Ext.LabelKey.Admin_FieldProperty_ButtonLabel+'(Default Language)');
    }

    RenderGrid(0,pgButton);
    
    pgButton.store.getById('Default Label').set('value',DecodeHTMLTag(obj._defaultLabelValue));
    pgButton.store.getById('Admin_FieldProperty_ButtonLabel').set('value',DecodeHTMLTag(obj._getLabel()));

    if(Ext.Const.IsSupportMultiLang)
    {
       var defaultLangValue = GetDefaultLangValue(obj);
       pgButton.store.getById('Admin_FieldProperty_ButtonLabel_DefaultLanguage').set('value',DecodeHTMLTag(defaultLangValue));
    }
    
    if(isHeadLabel)
    {
        pgButton.store.getById('Width').set('value',obj._getColumnWidth());
    }
    
    // if report link, Load reports
    if(obj._labelKeyValue == "aca_report_label")
    {
        var array = Ext.Const.OpenedId.split(Ext.Const.SplitChar);
        var pageID = array[0].substring(array[0].length - 4); // The last 4 numbers is page id.
         
        GetReportsByPageID(Ext.Const.ModuleName,pageID);
    }
};

function LoadConfigureUrlButton(obj){ 

var moduleName = Ext.Const.ModuleName;

    Accela.ACA.Web.WebService.AdminConfigureService.GetConfigureUrlOfLinkButton(moduleName, function GetConfigureUrlCallback(callBackConfigureUrl) {
        var urlList = eval('(' + callBackConfigureUrl[0] + ')');
        var selectedUrlList = callBackConfigureUrl[1];

        var pgButton = new Ext.grid.PropertyGrid({
            closable: true,
            autoHeight: true,
            customEditors: {
                'Default Label': new Ext.grid.GridEditor(new Ext.form.Field({
                    disabled: true,
                    disabledClass: 'x-DefaultHeading',
                    cls: "x-DefaultHeading"
                })),
                'Button Label(Default Language)': new Ext.grid.GridEditor(new Ext.form.Field({
                    disabled: true,
                    disabledClass: 'x-DefaultHeading',
                    cls: "x-DefaultHeading"
                })),
                'URL': new Ext.grid.GridEditor(new Ext.form.ComboBox({
                        id: 'cboCapFilterName',
                        store: new Ext.data.SimpleStore({
                            fields: ['id', 'urlName'],
                            data: urlList
                        }),
                        displayField: 'urlName',
                        valueField: "id",
                        editable: false,
                        triggerAction: 'all',
                        typeAhead: true,
                        mode: 'local',
                        emptyText: '',
                        listeners: {
                            'select': function(combo, record, index) {
                                var configureUrlId = record.data['id'];
                                var configureUrlName = record.data['urlName'];

                                combo.setValue(configureUrlName);

                                var changeItem = new ConfigureUrlButtonObj(Ext.Const.OpenedId, obj._controlIdValue, obj._labelKeyValue, obj._getLabel(), moduleName, configureUrlId);
                                changeItems.UpdateItem(7, changeItem);
                                Ext.getCmp(combo.container.id).completeEdit();
                            }
                        }
                    }),
                    { allowBlur: true })
            },
            listeners: {
                propertychange: function(source, recordId, value, oldValue) {
                    if (oldValue != null && oldValue != '') {
                        if (recordId == 'Admin_FieldProperty_ButtonLabel') {
                            obj._setLabel(EncodeHTMLTag(value));
                            UpdateDefaultLangValue(pgButton, value);
                        }

                        if (oldValue != value) {
                            var changeItem = new ButtonObj(Ext.Const.OpenedId, obj._controlIdValue, obj._labelKeyValue, obj._getLabel(), moduleName);
                            changeItems.UpdateItem(2, changeItem);
                            ModifyMark();
                        }
                    }
                },
                cellclick: function(grid, rowIndex, columnIndex, e) {
                    DisabledHtmlEditorButtons(true);
                },
                beforeedit: function(e) {
                    var record = e.grid.store.getAt(e.row);
                    if (record.id == 'Admin_FieldProperty_Instructions') {
                        e.cancel = true;
                        if (!isWinShow) {
                            PopupHtmlEditor(e.grid, obj._getSubLabel());
                            htmlEditorTargetType = htmlEditorTargetTypes.Field;
                            htmlEditorTargetCtrl = obj;
                        }
                    }
                }
            }
        });

        pgButton.store.sortInfo = null;

        if (Ext.Const.IsSupportMultiLang) {
            pgButton.setSource({
                'Default Label': '',
                'Admin_FieldProperty_ButtonLabel_DefaultLanguage': '',
                'Admin_FieldProperty_ButtonLabel': '',
                'Admin_FieldProperty_ButtonUrl': ''
            });
        } else {
            pgButton.setSource({
                'Default Label': '',
                'Admin_FieldProperty_ButtonLabel': '',
                'Admin_FieldProperty_ButtonUrl': ''
            });
        }
        pgButton.getColumnModel().setConfig([
            { header: 'Name', sortable: false, dataIndex: 'name', id: 'name' },
            { header: 'Value', sortable: false, dataIndex: 'value', id: 'value' }
        ]);

        pgButton.store.getById('Admin_FieldProperty_ButtonLabel').set('name', Ext.LabelKey.Admin_FieldProperty_ButtonLabel);
        pgButton.store.getById('Admin_FieldProperty_ButtonUrl').set('name', Ext.LabelKey.Admin_FieldProperty_ButtonUrl);

        if (Ext.Const.IsSupportMultiLang) {
            pgButton.store.getById('Admin_FieldProperty_ButtonLabel_DefaultLanguage').set('name', Ext.LabelKey.Admin_FieldProperty_ButtonLabel + '(Default Language)');
        }

        pgButton.store.getById('Default Label').set('value', DecodeHTMLTag(obj._defaultLabelValue));
        pgButton.store.getById('Admin_FieldProperty_ButtonLabel').set('value', DecodeHTMLTag(obj._getLabel()));
        pgButton.store.getById('Admin_FieldProperty_ButtonUrl').set('value', selectedUrlList);

        if (Ext.Const.IsSupportMultiLang) {
            var defaultLangValue = GetDefaultLangValue(obj);
            pgButton.store.getById('Admin_FieldProperty_ButtonLabel_DefaultLanguage').set('value', DecodeHTMLTag(defaultLangValue));
        }

        RenderGrid(0, pgButton);
    });
}

function LoadFilterButton(obj){ 
    var moduleName = Ext.Const.ModuleName;
    
    if(obj._moduleNameValue != null && obj._moduleNameValue != ''){
        moduleName = obj._moduleNameValue;
    }
    
    if(!obj._enableRecordTypeFilterValue){
        LoadButtonWithoutFilter(obj);
        
        return;
    }
    
    var selectedCol = -1;

    Accela.ACA.Web.WebService.AdminConfigureService.GetCapTypeFilterList(obj._labelKeyValue, moduleName, function LoadFilterCallback(json) {

        var filterNameList = eval('(' + json[0] + ')');

        for (var i = 0; i < filterNameList.length; i++) {
            filterNameList[i][1] = JsonDecode(filterNameList[i][1]);
        }

        var selectFilter = json[1];

        var pgButton = new Ext.grid.PropertyGrid({
            closable: true,
            autoHeight: true,
            customEditors: {
                'Default Label': new DisabledGridEditor(),
                'Button Label(Default Language)': new DisabledGridEditor(),
                'Record Type Filter': new Ext.grid.GridEditor(new Ext.form.ComboBox({
                        id: 'cboCapFilterName',
                        store: new Ext.data.SimpleStore({
                            fields: ['id', 'filter'],
                            data: filterNameList
                        }),
                        displayField: 'filter',
                        valueField: "id",
                        editable: false,
                        triggerAction: 'all',
                        typeAhead: true,
                        mode: 'local',
                        emptyText: '',
                        listeners: {
                            'select': function(combo, record, index) {
                                var filterName = record.data['id'];

                                if (filterName == 'ModuleName') {
                                    filterName = '';
                                } else {
                                    filterName = record.data['filter'];
                                }

                                var selectModuleName = moduleName;

                                //For contact Amendent button in account management page, the Module data is come from select item.
                                if (obj._labelKeyValue == 'aca_account_management_contact_amendment') {
                                    selectModuleName = record.data['id'];
                                }

                                combo.setValue(filterName);

                                var changeItem = new FilterObj(Ext.Const.OpenedId, obj._controlIdValue, obj._labelKeyValue, selectModuleName, filterName);
                                changeItems.UpdateItem(6, changeItem);
                                ModifyMark();
                                Ext.getCmp(combo.container.id).completeEdit();
                            },
                            'beforeselect': function(combo, record, index) {
                                var module = record.data['id'];

                                if (module == 'ModuleName') {
                                    return true;
                                }

                                if (module != moduleName && moduleName != '') {
                                    Ext.Msg.alert('', Ext.LabelKey.Admin_CapTypeFilter_Message_MultipleModule);

                                    return false;
                                }
                            },
                            'blur': function(cboCapFilterName) {
                                if (cboCapFilterName) {
                                    cboCapFilterName.collapse();
                                }
                            }
                        }
                    }),
                    { allowBlur: true })
            },
            listeners: {
                propertychange: function(source, recordId, value, oldValue) {
                    if (selectedCol == 1) {
                        ResponseWidthChange(obj, oldValue, recordId, value, pgButton);
                    }
                },
                cellclick: function(grid, rowIndex, columnIndex, e) {
                    selectedCol = columnIndex;
                }
            }
        });

        pgButton.store.sortInfo = null;
        var defaultLanLabel = Ext.Const.IsSupportMultiLang ? "'Admin_FieldProperty_ButtonLabel_DefaultLanguage':''," : '';
        var isHeadLabel = obj._elementTypeValue == 'GridViewHeaderLabel';
        var gridHeadLabel = isHeadLabel ? "'Width':''," : '';
        eval("pgButton.setSource({'Default Label': ''," + defaultLanLabel + "'Admin_FieldProperty_ButtonLabel': ''," + gridHeadLabel + "'Admin_FieldProperty_CapFilterName': ''});");

        pgButton.getColumnModel().setConfig([
            { header: 'Name', sortable: false, dataIndex: 'name', id: 'name' },
            { header: 'Value', sortable: false, dataIndex: 'value', id: 'value' }
        ]);

        pgButton.store.getById('Admin_FieldProperty_ButtonLabel').set('name', Ext.LabelKey.Admin_FieldProperty_ButtonLabel);
        pgButton.store.getById('Admin_FieldProperty_CapFilterName').set('name', 'Record Type Filter');
        pgButton.customEditors["Record Type Filter"].allowBlur = true;

        if (Ext.Const.IsSupportMultiLang) {
            pgButton.store.getById('Admin_FieldProperty_ButtonLabel_DefaultLanguage').set('name', Ext.LabelKey.Admin_FieldProperty_ButtonLabel + '(Default Language)');
        }

        RenderGrid(0, pgButton);

        pgButton.store.getById('Default Label').set('value', DecodeHTMLTag(obj._defaultLabelValue));
        pgButton.store.getById('Admin_FieldProperty_ButtonLabel').set('value', DecodeHTMLTag(obj._getLabel()));
        if (isHeadLabel) {
            pgButton.store.getById('Width').set('value', obj._getColumnWidth());
        }
        pgButton.store.getById('Admin_FieldProperty_CapFilterName').set('value', selectFilter);

        if (Ext.Const.IsSupportMultiLang) {
            var defaultLangValue = GetDefaultLangValue(obj);
            pgButton.store.getById('Admin_FieldProperty_ButtonLabel_DefaultLanguage').set('value', DecodeHTMLTag(defaultLangValue));
        }
    });
}

var isErrorInput = false;
function ResponseWidthChange(obj,oldValue,recordId,value,pgButton)
{      
    if(oldValue != null)
    {
        if(recordId != 'Width' )
        {
            if(recordId == 'Admin_FieldProperty_ButtonLabel')
            {
                obj._setLabel(EncodeHTMLTag(value));
                UpdateDefaultLangValue(pgButton, value, 'Admin_FieldProperty_ButtonLabel_DefaultLanguage');
            }
            
            if(oldValue != value)
            {
                var moduleName = Ext.Const.ModuleName;
    
                if(obj._moduleNameValue != null && obj._moduleNameValue != ''){
                    moduleName = obj._moduleNameValue;
                }
                
                var changeItem = new ButtonObj(Ext.Const.OpenedId,obj._controlIdValue,obj._labelKeyValue,obj._getLabel(), moduleName);
                SetPageFlowCode4Item(changeItem);

                changeItems.UpdateItem(2,changeItem);
                ModifyMark();
            }
        }
        else if(recordId == 'Width' && oldValue != value)
        {
            var pn = GetColumnTH(obj._element);
            
            if(pn)
            {
                if(value.trim() == '')
                {
                    alert(Ext.LabelKey.Admin_Width_Nullvalue_Message);
                    isErrorInput = true;
                    pgButton.store.getById('Width').set('value',oldValue);
                    return;    
                }
                else if(value.trim() != '')
                {
                    if (isErrorInput)
                    {
                        isErrorInput = false;
                        return; 
                    }
                    
                    var reg = /\d+(px)?/i;
                    if(value.trim().replace(reg,'') == '')
                    {
                        if(value.indexOf('px') == -1)
                        {
                            value += 'px';
                        }
                        pn.style.width = value;
                        pn.columnWidth = value;
                    }
                    else
                    {
                        alert(Ext.LabelKey.Admin_Width_Validate_Message);
                        isErrorInput = true;
                        pgButton.store.getById('Width').set('value',oldValue);
                        return;               
                    }
                }
                else
                {
                    pn.style.width = '';
                }
                
                if(value.trim() == '')
                {
                    value = 0;
                }
                else
                {
                    value = parseFloat(value.replace('px','').trim());
                }
                
                if(oldValue.trim() == '')
                {
                    oldValue = 0;
                }
                else
                {
                    oldValue = parseFloat(oldValue.replace('px','').trim());
                }
                
                pn.originalWidth = pn.style.width;
                pn.columnWidth=pn.style.width;
                obj._setColumnWidth(pn.style.width);
                var changeItem = new GridViewHeadWidthObj(Ext.Const.OpenedId,obj);
                changeItems.UpdateItem(9,changeItem);
                ModifyMark();
            }
        }
    }
    
    isErrorInput = false;
    if(typeof(ResetColumnWidth) !="undefined" && recordId == 'Width' && oldValue != undefined && oldValue !=""){
        ResetColumnWidth(obj,oldValue,value);
    } 
}

function GetColumnTH(object)
{
    var pn = object.parentNode;
    while(pn && pn.tagName != 'TH')
    {
        pn = pn.parentNode;
    }
    
    return pn;
}

function GetColumnTable(th)
{
    var pn = th.parentNode;
    while(pn && pn.tagName != 'TABLE')
    {
        pn = pn.parentNode;
    }
    
    return pn;
}

function GetReportsByPageID(moduleName,pageID)
{
    PageMethods.GetReportsByPage(moduleName,pageID,BuildReportsTable); 
}

// create reports table
function BuildReportsTable(reports)
{
   if(reports == "")  // no reports
   {
      return;
   }
   var reports = eval('(' + reports + ')');
   
   // order report array by report name
   reports.sort(function(x,y){
       return x["reportName"].localeCompare(y["reportName"]);
   });
   
   var div = document.createElement("div");
   div.style.margin = "10px";
   
   var table = document.createElement("table");
   table.id = "reportsTable";
   table.className = "itemListContainer";
   div.appendChild(table);
   
   //create the tbody
   var tbody = document.createElement("tbody");
   table.appendChild(tbody);
   
   //create rows
   for(var i = 0; i< reports.length; i++)
   {
        // create checkbox
        var chkBox = document.createElement("input");
        chkBox.type = "checkbox";
        chkBox.setAttribute("onClick","UpdateReportsSetting();");
        chkBox.id = "chkReport";

        // set checkbox checked or not   
        if(reports[i].visible != "N")   // default : visible
        {
           chkBox.checked = chkBox.defaultChecked  = true;
        }
        // hidden field for report id
        var hdf = document.createElement("input");
        hdf.type = "hidden";
        hdf.id = "hdfReportID";
        hdf.value = reports[i].reportID;
        
        // span for report name
        var span = document.createElement("span");
        span.innerHTML = reports[i].reportName;
        span.setAttribute("style","word-break:break-all;");
        
        // insert table cells - checkbox, report id, report name
        tbody.insertRow(i);      
        tbody.rows[i].insertCell(0);
        tbody.rows[i].cells[0].appendChild(chkBox);
        
        tbody.rows[i].insertCell(1);
        tbody.rows[i].cells[1].appendChild(hdf);
        
        tbody.rows[i].insertCell(2);
        tbody.rows[i].cells[2].appendChild(span);       
   }
   
   var instruction = document.createElement("span");
   instruction.innerHTML = "Available Reports";
   
   // Append the report table into the property panel  
   var divField = document.getElementById('divField');  
   divField.appendChild(instruction);       
   divField.appendChild(div);
}

function UpdateReportsSetting()
{
   var reportTable = document.getElementById("reportsTable");
   var reports = new Array();

   //get reports id and visible property
   for(var i=0; i<reportTable.rows.length; i++)
   {
      // cells[0]-checkBox, cells[1]-reportID, cells[2]-reportName
      var isVisible = reportTable.rows[i].cells[0].childNodes[0].checked;
      var reportID = reportTable.rows[i].cells[1].childNodes[0].value;
      
      reports[i] = new reportItem(reportID,isVisible);
   }
   
   var changeItem = new ReportsObj(Ext.Const.OpenedId,reports);
   changeItems.UpdateItem(8,changeItem);
   ModifyMark();
}

function reportItem(reportID,visible)
{
   this.reportID = reportID;
   this.visible = visible;

}

//set the text heading value
function SetTextHeadingValue(obj,value){
    // Resolve the special characters.
    var dom = obj._element;
    
    dom.innerHTML=EncodeHTMLTag(value);
}

function showMessage(title,context) {
      Ext.MessageBox.show({   
           title:title,
           msg:context,
           width:400,
           progressText: '',
           icon:Ext.MessageBox.ERROR,
           closable:true
       });
}

function RemoveLabelStyle(value)
{
   if(value)
   {
      value = value.replace(/<font.*?>|<b.*?>|<i.*?>|<\/font>|<\/b>|<\/i>/gi, '');
   }
   
   return value;
}

//******************************************************************************
// Load page flow combo box and handle page flow selection
//******************************************************************************
function CreatePageFlowComBox()
{   
    var pageFlowStore = new Ext.data.SimpleStore({
                url:'Pageflow/WorkflowContent.aspx',
                baseParams: {
                    action:'GetPageFlowGroupNameList',
                    defaultSelectValue: '--select--'
                },
                fields: ['Name']
            });
            
    pageFlowStore.load();

    var comboBox = new Ext.form.ComboBox({
  	    id:'cboPageFlowGroup',
        store: pageFlowStore,
        displayField: "Name",
        valueField:"Name",
        editable:false,
        mode: 'local',
        triggerAction:'all',
        selectOnFocus:true
    });
    
    var beforeSelectValue;
    
    comboBox.on('beforeselect', function(){
          beforeSelectValue = JsonDecode(this.getValue());
       }
    );
    comboBox.on('select', function(combo, record, index){
          SwitchPageFlow(record.data['Name'], beforeSelectValue);
        }
    );
         
    return comboBox;
}

function SwitchPageFlow(pageFlow, formerValue)
{     
     if(!pageFlow)
     { 
         pageFlow = '--select--';
     }
    
     var tab = Ext.getCmp(Ext.Const.OpenedId);
     var reg = /\*$/;
        
     if(reg.test(tab.title))  // page has been changed
     {
          Ext.Msg.show({
              msg: Ext.LabelKey.Admin_Pageflow_Message_SwitchPageFlow,
              buttons: Ext.Msg.OKCANCEL,
              icon: Ext.MessageBox.WARNING,
              fn: function(btn){
                      if(btn == 'ok'){
                          changeItems.RemoveChangedItemsByPageId(Ext.Const.OpenedId);
                          RemoveMark();
                          RefreshPage(pageFlow);
                            
                          if(!formerValue)
                          {
                             LoadPageFlowProperty(pageFlow);
                          }                            
                      }
                      else
                      {
                         if(formerValue)
                         {
                            Ext.getCmp('cboPageFlowGroup').setValue(formerValue);
                         }
                      }
               }
         });
     }
     else 
     {
         RefreshPage(pageFlow);
          
         if(!formerValue)
         {
           LoadPageFlowProperty(pageFlow);
         }
     }
}

// When user swich page flow code, the page should be re-loaded to get the label keys for the new code.
function RefreshPage(pageFlowCode)
{
    Ext.getCmp('cboPageFlowGroup').setValue(JsonDecode(pageFlowCode));  // page flow group has been encoded, so we need to decode it to display

    var dom = GetFrame();
    var iframe = window.frames[dom.id];
    var url = iframe.location.href;
    pageFlowCode = encodeURIComponent(pageFlowCode);  //page flow code should be encoded for url
    
    if(pageFlowCode == '--select--')
    {
       iframe.location.href = url.replace(/&PageFlow=.+/, '');
       
       return;
    }
   
    if(url.indexOf('PageFlow=') == -1)
    {
       // append page flow to the url.
       iframe.location.href = url + '&PageFlow=' + pageFlowCode;
    }
    else
    {
       iframe.location.href = url.replace(/PageFlow=.+/, 'PageFlow=' + pageFlowCode);
    }
}

function LoadPageFlowProperty(pageFlow)
{
    var pgPageFlowConfig = new Ext.grid.PropertyGrid({
        closable: true,
        autoHeight: true,
        customEditors: {
           'Select Page Flow' : new Ext.grid.GridEditor(CreatePageFlowComBox())
        }
   });
   
   pgPageFlowConfig.customEditors['Select Page Flow'].allowBlur = true;
   
   if(!pageFlow)
   {
       //if user edit other fields in the page and then click 'Receipt' label, should re-set the value.
       pageFlow = GetPageFlowCode();
   }
   
   pgPageFlowConfig.store.sortInfo = null;
   
   pgPageFlowConfig.setSource({
         'Select Page Flow': pageFlow ? pageFlow : '--select--'
   });
        
   RenderGrid(0, pgPageFlowConfig);
}

// Get page flow code from page url
function GetPageFlowCode()
{
    var pageFlow;
    var activeTab = Ext.getCmp(Ext.Const.OpenedId);
    
    if(activeTab)
    {
        if(!activeTab.body || !activeTab.body.dom)
        {
           return pageFlow;
        }
        
        var dom = activeTab.body.dom.firstChild;
        var iframe = window.frames[dom.id];

        if(iframe)
        {
           var url = iframe.location.href;
           var index = url.indexOf('PageFlow=');
          
           if(index > 0)
           {
             pageFlow = JsonDecode(decodeURIComponent(url.substring(index + 9)));
           }
        }
    }
    
    return pageFlow;
}

function SetPageFlowCode4Item(changeItem)
{
    var pageFlow = GetPageFlowCode();
    
    if(pageFlow)
    {
       changeItem.PageFlow = pageFlow;
    }
}


//Load page properties. 
function LoadPageProperty(obj){
    CloseWindow();
    
    var pgProperty = new Ext.grid.PropertyGrid({
        id:'pgPageProperty',
        closable: true,
        autoEncode:true,
        autoHeight: true,
        customEditors: {
                'Instructions(Default Language)': new DisabledGridEditor()
        },
        listeners: {
            beforeedit: function(e) {
                var record = e.grid.store.getAt(e.row);
                if(record.id == PropertyObj.standardProperties.Instruction)
                {
                    e.cancel = true;
                    if(!isWinShow)
                    {
                        PopupHtmlEditor(e.grid, obj._getLabel());
                        htmlEditorTargetCtrl = obj;
                        htmlEditorTargetType = htmlEditorTargetTypes.PageInstruction;
                    }
                }
            },
            cellclick: function(grid, rowIndex, columnIndex, e){
                DisabledHtmlEditorButtons(true);
            }
        }
    });    

    pgProperty.store.sortInfo = null;   
    var store = {};
    
    if(Ext.Const.IsSupportMultiLang)
    {
       store['Instructions(Default Language)'] = GetDefaultLangValue(obj);
    }
    
    store[PropertyObj.standardProperties.Instruction] = Ext.LabelKey.Admin_ClickToEdit_Watermark;  
    pgProperty.setSource(store);
    
    pgProperty.getColumnModel().setConfig([
          { header: 'Name', sortable: false, dataIndex: 'name', id: 'name' },
          { header: 'Value', sortable: false, dataIndex: 'value', id: 'value' }
        ]);

    pgProperty.store.getById(PropertyObj.standardProperties.Instruction).set('name', 'Instructions');

    RenderGrid(0, pgProperty);
}

function LoadComponent(obj, isCustomize){
    CloseWindow();    

    var grid = new Ext.grid.PropertyGrid({
        id: 'pgComponent',
        closable: true,
        autoHeight: true,
        customEditors:{            
            "Visible": new BoolComboBoxGridEditor()
        },
        listeners: {
            propertychange: function(source, recordId, value, oldValue){
                var resId = obj._sectionIdValue.split(Ext.Const.SplitChar)[0];                         
                var objGrid = this;

                if (resId > 0) {
                    objGrid.changeCustomComponentItem(source, resId);
                }
                else {
                    // confirm the resId, it will display resId as 0 when it exists:
                    // 1. The DB not exists the data, so resId is 0. 
                    // 2. Chang and save to DB, then it exists resId.
                    // 3. Change some value, but the resId is not refresh from DB, so it is remain 0(it is wrong).
                    var oldComponentName = source["Name"];
                    if (recordId == "Name") {
                        oldComponentName = oldValue;
                    }

                    var elementId = Ext.Const.OpenedId.split(Ext.Const.SplitChar)[0];
                    Accela.ACA.Web.WebService.AdminConfigureService.GetCustomComponentByElementId(elementId, function (json) {
                        // get the real resId if it display as 0.
                        var existsComponents = eval(json);
                        for (var i = 0; i < existsComponents.length; i++) {
                            var existsName = existsComponents[i]["ComponentName"];

                            if (oldComponentName == existsName) {
                                resId = existsComponents[i]["ResID"];
                                break;
                            }
                        }

                        objGrid.changeCustomComponentItem(source, resId);
                    });
                }               
            },
            beforepropertychange: function (source, recordId, value, oldValue) {
                // validate empty component name
                if (recordId == "Name" && (value == null || value == "")) {
                        ShowErrorMessage("Name can not be empty.");
                        return false;
                }

                // validate path
                if (recordId == "Path") {
                    var reg = /[\"<>|:*?]/;
                     if (value != null && value != "" && reg.test(value)){
                        ShowErrorMessage("Invalid path.");
                        return false;
                    }
                }
            }
        },
        changeCustomComponentItem: function (source, resId){
            var elementId = Ext.Const.OpenedId.split(Ext.Const.SplitChar)[0];
            var componentName = source["Name"];
            var visible = Boolean.parse(source["Visible"].toString()) ? "Y" : "N";
            var path = "";

            if (source["Path"] != null) {
                path = source["Path"];
            }

            // set the changed _sectionIdValue that click the object again it will display the changed value.
            obj._sectionIdValue = resId + Ext.Const.SplitChar + componentName + Ext.Const.SplitChar + visible + Ext.Const.SplitChar + path;

            var changeItem = new CustomComponentObj(Ext.Const.OpenedId, obj._id, resId, elementId, componentName, visible, path);
            changeItems.UpdateItem(11, changeItem);
            ModifyMark();
        }
    });
    
    // set the grid data source
    var store = {};
    store["Name"] = obj._sectionIdValue.split(Ext.Const.SplitChar)[1];
    store["Visible"] = obj._sectionIdValue.split(Ext.Const.SplitChar)[2] == "Y";

    if (!isCustomize) {
        grid.customEditors["Name"] = new DisabledGridEditor();
    }
    else {
        store["Path"] = obj._sectionIdValue.split(Ext.Const.SplitChar)[3];
    }

    grid.setSource(store);

    // render grid
    RenderGrid(0, grid);
}

// load the grid view's template field
function LoadGridViewTemplateField(obj){
    CloseWindow();

    var grid = new Ext.grid.PropertyGrid({
        id: 'pgTemplateField',
        closable: true,
        autoHeight: true,
        customEditors:{            
            "Button Label": new DisabledGridEditor(),
            "Button Label(Default Language)": new DisabledGridEditor()  
        },
        listeners: {
            propertychange:function(source, recordId, value, oldValue){
                if (recordId == 'Width'){
                    ResponseWidthChange(obj, oldValue, recordId, value, grid);
                }
            }
        }
    });

    grid.store.sortInfo = null;

    var store = {};

    if(Ext.Const.IsSupportMultiLang)
    {
        store['Admin_ButtonLabel_DefaultLanguage'] = '';
    }

    store['Button Label'] = DecodeHTMLTag(obj._getLabel());
    store['Width'] = obj._getColumnWidth();
    grid.setSource(store);
    
    grid.getColumnModel().setConfig([
        {header: 'Name', sortable: false, dataIndex: 'name', id: 'name'},
        {header: 'Value', sortable: false, dataIndex: 'value', id: 'value'}
    ]);

    if(Ext.Const.IsSupportMultiLang)
    {
        grid.store.getById('Admin_ButtonLabel_DefaultLanguage').set('name', Ext.LabelKey.Admin_FieldProperty_ButtonLabel + '(Default Language)');
        grid.store.getById('Admin_ButtonLabel_DefaultLanguage').set('value', DecodeHTMLTag(GetDefaultLangValue(obj, true)));
    }

    // render grid
    RenderGrid(0, grid);
}