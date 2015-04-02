﻿/**
* <pre>
* 
*  Accela Citizen Access Admin
*  File: PropertyPanelHelper.js
* 
*  Accela, Inc.
*  Copyright (C): 2010-2014
* 
*  Description:
*  Provide common property and method for property panel
*
*  Notes:
*      $Id: PropertyPanelHelper.js 77905 2010-04-16 12:49:28Z ACHIEVO\jack.feng $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

var PropertyObj = new Object();

function DisabledGridEditor()
{
   return new Ext.grid.GridEditor(new Ext.form.Field({
      disabled: true,
      disabledClass: 'x-DefaultHeading',
      cls: "x-DefaultHeading"
    }));
}

function BoolComboBoxGridEditor(){
    return new Ext.grid.GridEditor(new Ext.form.ComboBox({
        store: new Ext.data.SimpleStore({
            fields: ['value'],
            data: [
                [true],
                [false]
            ]
        }),
        editable: false,
        displayField: 'value',
        typeAhead: true,
        mode: 'local',
        triggerAction: 'all',
        selectOnFocus: true,
        emptyText: '',
        listeners: {
            'select': function (combo, rec, idx) {
                Ext.getCmp(combo.container.id).completeEdit();
            }
        }
    }),
    { allowBlur:true });
}

 PropertyObj.standardProperties =
 {
     Default_Label: 'Default Label',
     Field_Label: 'Admin_FieldProperty_FieldLabel',
     Field_Label_DefaultLang: 'Admin_FieldProperty_FieldLabel_DefaultLanguage',
     Default_Instruction: 'Default Instructions',
     Instruction: 'Admin_FieldProperty_Instructions',
     AutoFill: 'Admin_FieldProperty_AutoFill',
     SearchRequired:'admin_fieldproperty_searchrequired'
 }
 
 PropertyObj.templateProperties=
 {
     Field_Label: 'Admin_FieldProperty_FieldLabel',
     Field_Label_DefaultLang: 'Admin_FieldProperty_FieldLabel_DefaultLanguage',
     Instruction: 'Admin_FieldProperty_Instructions'
 }

 PropertyObj.standardDisabledEditor =
 {       
     'Default Label': new DisabledGridEditor(),
     'Field Label(Default Language)': new DisabledGridEditor(),
     'Default Instructions': new DisabledGridEditor()
 }
 
 PropertyObj.templateDisabledEditor =
 {       
     'Field Label': new DisabledGridEditor(),
     'Field Label(Default Language)': new DisabledGridEditor()
 }

 function CreateDisabledCustomerEditor(arrayFields) {
     var customerEditor = {};
     
     if (arrayFields && arrayFields.length > 0) {
         for (var i = 0; i < arrayFields.length; i++) {
             customerEditor[arrayFields[i]] = new DisabledGridEditor();
         }
     }

     return customerEditor;
 }

 /* 
   Create standard data store for property grid as below:
   Default Label 
   Field Label(Default Language) -- in I18N
   Field Label -- allow user defined label, default value is Field label
   Default Instructions
   'Admin_FieldProperty_Instructions' -- instruction if the object need it
   'Admin_FieldProperty_AutoFill' -- auto fill if the object need this property
 */
function CreatePropertyStore(obj, fieldLabelId)
{
     var store = {};

     var properties;
     if(obj.get_IsTemplateField()) {
         properties=PropertyObj.templateProperties;
     }
     else {
         properties=PropertyObj.standardProperties;
     }

     with (properties) 
     {
         if(!obj.get_IsHiddenLabel())
         {
             if(typeof(Default_Label) != 'undefined') {
                 store[Default_Label] = obj._defaultLabelValue == null ? '' : obj._defaultLabelValue;
             }

             var fieldLabel = Field_Label;
             var fieldLabelDefaultLang = Field_Label_DefaultLang;
         
             if (fieldLabelId) {
                 fieldLabel = fieldLabelId;
                 fieldLabelDefaultLang = fieldLabelId + "_DefaultLanguage";
             }
         
             if (Ext.Const.IsSupportMultiLang) {
                 store[fieldLabelDefaultLang] = GetDefaultLangValue(obj);
             }
         
             var labelValue = obj._getLabel();
             store[fieldLabel] = labelValue == null ? '' : DecodeHTMLTag(labelValue);
         }

         if (obj._defaultSubLabelValue) {
             store[Default_Instruction] = obj._defaultSubLabelValue;
         }

         if (obj.needWatermarkText) {
             var watermark;
             
             if (obj._elementTypeValue == "AccelaRangeNumberText") {
                 watermark = obj._getWatermarkText("watermarkText1");
                 store["Admin_FieldProperty_WatermarkText1"] = watermark == null ? '' : DecodeHTMLTag(watermark);
                 watermark = obj._getWatermarkText("watermarkText2");
                 store["Admin_FieldProperty_WatermarkText2"] = watermark == null ? '' : DecodeHTMLTag(watermark);
             } else {
                 watermark = obj._getWatermarkText("watermarkText");
                 store["Admin_FieldProperty_WatermarkText"] = watermark == null ? '' : DecodeHTMLTag(watermark);    
             }
         }
         
         if (obj.needInstruction) {
             var instruction = obj._getSubLabel();
             store["Admin_FieldProperty_Instructions"] = instruction == null ? '' : Ext.LabelKey.Admin_ClickToEdit_Watermark;
         }

         if (obj.needAutoFill) {
             store[AutoFill] = obj.autoFillValue;
         }
         
         if(obj.needRequire){
             store[SearchRequired] = obj.EnableSearchRequired;            
         }
     }
     
     return store;
}

 function BindPropertyGrid(obj, grid) {

     if (!obj || !grid) {
         return;
     }
     
     var fieldLabelId = obj.fieldLabelId ? obj.fieldLabelId : "Admin_FieldProperty_FieldLabel";   // field label
     var fieldLabel = obj.fieldLabelName ? obj.fieldLabelName : Ext.LabelKey.Admin_FieldProperty_FieldLabel;

     var store = CreatePropertyStore(obj, fieldLabelId);
     grid.store.sortInfo = null;
     grid.setSource(store);
          
     // Set disabled custom editores.
     var disabledFields = [];
     disabledFields.push("Default Label");

     if(obj.get_IsTemplateField())
     {
        //Template fields
        disabledFields.push(fieldLabel);
     }
     
     if (Ext.Const.IsSupportMultiLang) {
         disabledFields.push(fieldLabel + "(Default Language)");
     }
     if (obj._defaultSubLabelValue) {
         disabledFields.push("Default Instructions");
     }

     grid.customEditors = CreateDisabledCustomerEditor(disabledFields);     

     if (obj.needAutoFill && typeof(obj.autoFillValue)=="boolean") {
        grid.customEditors[Ext.LabelKey.admin_fieldproperty_autofill] = new BoolComboBoxGridEditor();
     }

     grid.getColumnModel().setConfig([
              { header: 'Name', sortable: false, dataIndex: 'name', id: 'name' },
              { header: 'Value', sortable: false, dataIndex: 'value', id: 'value' }
            ]);

     // Set label keys for labels.
     grid.store.getById(fieldLabelId).set('name', fieldLabel);

     if (obj.needWatermarkText) {
         if(obj._elementTypeValue == "AccelaRangeNumberText") {
            grid.store.getById('Admin_FieldProperty_WatermarkText1').set('name', Ext.LabelKey.Admin_FieldProperty_WatermarkText1);
            grid.store.getById('Admin_FieldProperty_WatermarkText2').set('name', Ext.LabelKey.Admin_FieldProperty_WatermarkText2);
         } else {
            grid.store.getById('Admin_FieldProperty_WatermarkText').set('name', Ext.LabelKey.Admin_FieldProperty_WatermarkText);    
         }
     }
     if (obj.needInstruction) {
         grid.store.getById('Admin_FieldProperty_Instructions').set('name', Ext.LabelKey.Admin_FieldProperty_Instructions);
     }
     if (obj.needAutoFill) {
         grid.store.getById('Admin_FieldProperty_AutoFill').set('name', Ext.LabelKey.admin_fieldproperty_autofill);
     }
     if (Ext.Const.IsSupportMultiLang) {
         grid.store.getById(fieldLabelId + "_DefaultLanguage").set('name', fieldLabel + "(Default Language)");
     }
 }
 
 //Bind no head water mark input.
 function BindWaterMarkInput(obj, grid) {

     if (!obj || !grid) {
         return;
     }
     
     var store = {};

     if (obj.needWatermarkText) {
         var watermark;
         if(obj._elementTypeValue == "AccelaRangeNumberText") {
             watermark = obj._getWatermarkText("watermarkText1");
             store["Admin_FieldProperty_WatermarkText1"] = watermark == null ? '' : DecodeHTMLTag(watermark);
             watermark = obj._getWatermarkText("watermarkText2");
             store["Admin_FieldProperty_WatermarkText2"] = watermark == null ? '' : DecodeHTMLTag(watermark);
         } else {
             watermark = obj._getWatermarkText("watermarkText");
             store["Admin_FieldProperty_WatermarkText"] = watermark == null ? '' : DecodeHTMLTag(watermark);    
         }
         
     }
     
     grid.store.sortInfo = null;
     grid.setSource(store);
     
     grid.getColumnModel().setConfig([
              { header: 'Name', sortable: false, dataIndex: 'name', id: 'name' },
              { header: 'Value', sortable: false, dataIndex: 'value', id: 'value' }
            ]);

     if (obj.needWatermarkText) {
         grid.store.getById('Admin_FieldProperty_WatermarkText').set('name', Ext.LabelKey.Admin_FieldProperty_WatermarkText);
     }
 }

 /*********************************************************************
 HTML eitor object 
 *********************************************************************/

Ext.form.HtmlEditorWithImg = Ext.extend(Ext.form.HtmlEditor, {
     addImage: function()
     {
        var editor = this;
        var linkText = "Please enter the URL for the image:"
        var url = prompt(linkText, this.defaultLinkValue);
        if(url && url != 'http:/'+'/'){
            var element = document.createElement("img");
            element.border="0";
            element.src = url;
            
            if (Ext.isIE) {
                editor.insertAtCursor(element.outerHTML);
            } else {
                var selection = editor.win.getSelection();
                if (!selection.isCollapsed) {
                    selection.deleteFromDocument();
                }
                selection.getRangeAt(0).insertNode(element);
            }
        }
    },//end addImage function

    //Override the getDocMarkup function.
    getDocMarkup : function(){
        var docType = '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">';
        var htmlElt = '<html><head><style type="text/css">body{border:0;margin:0;padding:3px;height:98%;cursor:text;}</style></head><body></body></html>';
        return docType + htmlElt;
    },

    createToolbar : function(editor) {
        Ext.form.HtmlEditorWithImg.superclass.createToolbar.call(this, editor);
        this.tb.insertButton(16, {
                    iconCls : "btnImage",
                    handler : this.addImage,
                    tooltip: {
                       title: 'Image',
                       text: 'Insert an image.',
                       cls: 'x-html-editor-tip'
                    },
                    scope : this
                });
    }
});

Ext.reg('HtmlEditorWithImg', Ext.form.HtmlEditorWithImg);

var oldEditorValue;

 var htmEdit = new Ext.form.HtmlEditorWithImg({ id: 'htmEdit',
     listeners: {
         sync: function(htmEdit, newValue) {
             if (typeof (oldEditorValue) == 'undefined' || oldEditorValue == '' ) {
                 oldEditorValue = newValue;
             }
             else if (oldEditorValue != newValue) {
                 DisabledHtmlEditorButtons(false);
             }
         },
         render: function(c) {
             var eventName = 'propertychange';

             if (isFireFox()) {
                 eventName = 'input';
             }

             c.getEl().on(eventName, function(e, htmEdit) {
                 if (c.value != htmEdit.value) {
                     DisabledHtmlEditorButtons(false);
                 }
             });
         }
     }
 });

 var isWinShow;
 var win = new Ext.Window({
     layout: 'fit',
     width: 560,
     height: 300,
     closeAction: 'hide',
     plain: true,
     items: [htmEdit],
     bbar: [new Ext.Toolbar.Button({ text: "OK", id: "btnHtmlOK", pressed: true, minWidth: 50, handler: SaveValue }),
               new Ext.Toolbar.Spacer(), new Ext.Toolbar.Spacer(), new Ext.Toolbar.Spacer(),
               new Ext.Toolbar.Button({ text: "Cancel", id: "btnCancel", pressed: true, minWidth: 50, handler: CloseEditorWin })]
 });

 win.addListener('beforehide', ClearValue4Win);

 var htmlEditorTargetCtrl;
 var htmlEditorTargetTypes =
 {
    Section: 0,
    SubSection: 1,
    Field: 2,
    BodyText: 3,
    Search_Section: 4,
    PageInstruction: 5,
    PageFlowInstruction: 6
 }
 
 var htmlEditorTargetType = htmlEditorTargetTypes.Field;
 
 function SaveValue() {
     var val = htmEdit.getValue();
     var valueLength = getByteLength4String(val);

     if (valueLength > 4000) {
         var message = String.format(Ext.LabelKey.Admin_Page_Message_MustBeLessThanMaxLength, '4000');
         Ext.Msg.alert('', message);

         return;
     }

     isWinShow = false;
     isHtmlWinShow = false;

     if (val.toLowerCase() == "<p>&nbsp;</p>") 
     {
         val = '';
     }
     
     switch(htmlEditorTargetType)
     {
        case htmlEditorTargetTypes.BodyText:
           var grid = Ext.getCmp('propertyGrid');

           if (grid) {
              grid.store.getById('Admin_FieldProperty_BodyText').set('value', val);
              UpdateDefaultLangValue(grid, val, 'Admin_FieldProperty_BodyText_DefaultLanguage');
           }
           break;
        case htmlEditorTargetTypes.Search_Section:
           UpdateInstruction4SearchedDDL(htmlEditorTargetCtrl, val);
           break;
        case htmlEditorTargetTypes.PageInstruction:
           var changeItem = new InputObj(Ext.Const.OpenedId, htmlEditorTargetCtrl._controlIdValue, htmlEditorTargetCtrl._labelKeyValue, val, htmlEditorTargetCtrl.get_IsTemplateField(), htmlEditorTargetCtrl.get_TemplateAttribute());
           changeItems.UpdateItem(1, changeItem);
           htmlEditorTargetCtrl._setLabel(val);
           UpdateDefaultLangValue('pgPageProperty', val, 'Instructions(Default Language)');
           break;
        case htmlEditorTargetTypes.PageFlowInstruction:
           setInstruction4PageFlow(val);
           break;
        default:
           with (htmlEditorTargetCtrl) {
              var changeItem = new InputObj(Ext.Const.OpenedId, _controlIdValue, _labelKeyValue, _getLabel(), get_IsTemplateField(), get_TemplateAttribute(), _labelKeyValue + '|sub', val);
              changeItems.UpdateItem(1, changeItem);

              _setSubLabel(val);
           }
           
           break;
     }
     
     if(htmlEditorTargetType == htmlEditorTargetTypes.PageFlowInstruction)
     {
        changeSaveStatus();
     }
     else
     {
       ModifyMark();
     }
     
     win.hide();
 }

function UpdateInstruction4SearchedDDL(obj, instruction) {
    var prefix = 'ctl00_PlaceHolderMain_';
    var activeTab = GetDom(CurrentTab);
    var ddl = activeTab.getElementById(prefix + obj._controlIdValue);
    var spanInstruction = activeTab.getElementById(prefix + 'lblSearchInstruction');

    ddl.SectionInfo.Add(spanInstruction.labelKey, instruction);
    spanInstruction.innerHTML = instruction;
    var changeItem = new InputObj(Ext.Const.OpenedId, null, spanInstruction.labelKey, instruction,obj.get_IsTemplateField(),obj.get_TemplateAttribute());
    changeItems.UpdateItem(1, changeItem);
 }
 
function UpdateDefaultLangValue(grid, value, cellId)
{
    if(IsDefaultLanguage() && grid)
    {
        if(typeof (grid) == "string" )
        {
           grid = Ext.getCmp(grid);
        }
        
        if(!cellId)
        {
           cellId = PropertyObj.standardProperties.Field_Label_DefaultLang;
        }
        
        if(grid && grid.store && grid.store.getById(cellId))
        {
           grid.store.getById(cellId).set('value', value);
        }
    }
}

//Get the field label of the primary language
function GetDefaultLangValue(obj, isGridViewField)
{
    var defaultLanguageValue = '';
    
    if(obj && Ext.Const.IsSupportMultiLang)
    {        
        var defaultLanguageValue;
        if(obj.get_IsTemplateField()) {
            //template fields
            var templateAttribute = obj.get_TemplateAttribute();
            if(obj._defaultLanguageTextValue == null || obj._defaultLanguageTextValue == '') {
                defaultLanguageValue = templateAttribute.AttributeName;
            }
            else {
                defaultLanguageValue = obj._defaultLanguageTextValue;
            }
                        
            if(!isGridViewField && defaultLanguageValue != null && defaultLanguageValue.length > 0 && defaultLanguageValue.substr(defaultLanguageValue.length - 1, 1) != ":"){
                defaultLanguageValue += ":";
            }
        }
        else {
            //standard fields
            defaultLanguageValue = IsDefaultLanguage() ? obj._getLabel() : obj._defaultLanguageTextValue;
        }
        
        if(defaultLanguageValue == null)
        {
           defaultLanguageValue = '';
        }
    }
    
    return defaultLanguageValue;
}

function ClearValue4Win()
{
   isWinShow = false;
   oldEditorValue = '';
}

 function CloseEditorWin() {
     oldEditorValue = '';
     try {
			if(isWinShow)
			{
				isWinShow=false;
				win.hide();
			}
     }
     catch (err) {
     }
 }

 // set the OK Button disabled or not.
 function DisabledHtmlEditorButtons(isDisabled) {
     var btnOK = Ext.getCmp("btnHtmlOK");

     if (!isDisabled) {
         btnOK.enable();
     }
     else {
         btnOK.disable();
     }
 }

 function PopupHtmlEditor(propertyGrid, value) {
     htmEdit.setValue(value);
     var cell = propertyGrid.getBox();
     var top = cell.y + cell.height;
     var left = cell.x + cell.width - win.width;
     win.setPosition(left, top);
     win.show.defer(200, win);
     isHtmlWinShow = true;
     isWinShow = true;
     DisabledHtmlEditorButtons(true);
 }