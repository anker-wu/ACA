/**
* <pre>
*
*  Accela
*  File: admin.js
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description:
* To deal with inspection action menu .
*  Notes:
* $Id: admin.js 185465 2014-08-21 08:27:07Z dennis.fu $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

var regFontWeightNormal = /(font-weight:\s*normal)/gi;
var regFontWeightBold = /(font-weight:\s*bold)|(<b>)/gi;
var regFontStyleItalic = /(font-style:\s*italic)|(<i>)/gi;
var regFontStyleNormal = /(font-style:\s*normal)/gi;
var regBtag = /<b.*?>|<\/b>/gi;
var regItag = /<i.*?>|<\/i>/gi;

$(function() {
    if (isAdmin) {
        $('a').each(function() {
            if ($(this).data('controlType')) {
                $(this).attr('href', 'javascript:void(0);');
                $(this).removeAttr('target');
            }
        });

        $(document).on("click", "[data-control-type]", function(e) {
            e.preventDefault();
            // in color theme tab or click current selected element, no operation.
            if (isColorTheme || this == document.selectedEle) {
                return false;
            }

            var control = this;
            var controlType = $(control).data('controlType');
            if (controlType && controlType != 'none') {
                selectedElement(control);
                loadEastPanel(controlType, control);
            }
        });
    }
});

// load panel.
function loadEastPanel(controlType, obj) {
    initEastPanel(controlType, obj);
}

// init panel.
function initEastPanel(controlType, obj) {
    switch (controlType) {
        case 'text':
            renderText(obj);
            parent.LoadEastPanelTitle(0);
            break;
        case 'link':
            renderLink(obj);
            parent.LoadEastPanelTitle(0);
            break;
        default:
            break;
    }
}

/*
 * text.
 */
function renderText(textObj) {
    var $text = $(textObj);

    var selectedCol = -1;
    var pgHeader = new parent.Ext.grid.PropertyGrid({
        id: 'pgHeader',
        nameText: 'Properties Grid',
        closable: true,
        autoHeight: true,
        customEditors: {
            "Default Label": new parent.DisabledGridEditor(),
            "Text Heading(Default Language)": new parent.DisabledGridEditor(),
            "Text Heading": new parent.Ext.grid.GridEditor(new parent.Ext.form.Field({
                disabled: false,
                disabledClass: 'x-DefaultHeading',
                cls: "x-DefaultHeading"
            })),
            'Font-Bold': new parent.BoolComboBoxGridEditor(),
            'Font-Italic': new parent.BoolComboBoxGridEditor()
        },
        listeners: {
            cellclick: function(grid, rowIndex, columnIndex, e) {
                selectedCol = (columnIndex != 1 ? 1 : columnIndex);
            },
            propertychange: function(source, recordId, value, oldValue) {
                //check if text changed
                var isTextUpdated = (value != oldValue);

                if (selectedCol == 1) {
                    if (recordId == 'Admin_FieldProperty_TextHeading') {
                        SetTextHeadingValue(textObj, value);
                        parent.UpdateDefaultLangValue(pgHeader, value, 'Admin_FieldProperty_TextHeading_DefaultLanguage');
                    }else if (recordId == 'Font-Bold') {
                        // tag <b>
                        $text.html($text.html().replace(regBtag, ''));
                        $text.html('<b style="font-weight:' + (parent.IsTrue(value) ? 'bold' : 'normal') + ';">' + $text.html() + "</b>");
                    } else if (recordId == 'Admin_FieldProperty_Font_Italic') {
                        // tag <i>
                        $text.html($text.html().replace(regItag, ''));
                        $text.html('<i style="font-style:' + (parent.IsTrue(value) ? 'italic' : 'normal') + ';">' + $text.html() + "</i>");
                    }

                    if (isTextUpdated) {
                        var changeItem;

                        var heading = pgHeader.store.getById('Admin_FieldProperty_TextHeading').get('value');
                        if (heading == '') {
                            $text.html('');
                            $text.removeClass('ACA_Control_HighLight');
                        }

                        var displayLabel = $text.html();

                        changeItem = new parent.TextObj(parent.Ext.Const.OpenedId, $text.attr("id"), getLabelKey($text.attr("ng-labelkey")), displayLabel, '');
                        parent.SetPageFlowCode4Item(changeItem);

                        parent.changeItems.UpdateItem(0, changeItem);
                        parent.ModifyMark();
                    }
                }
            },
        }
    });

    pgHeader.store.sortInfo = null;
    var store = {};
    store['Default Label'] = '';
    if (parent.Ext.Const.IsSupportMultiLang) {
        store['Admin_FieldProperty_TextHeading_DefaultLanguage'] = "";
    }
    store['Admin_FieldProperty_TextHeading'] = '';
    store['Font-Bold'] = '';
    store['Admin_FieldProperty_Font_Italic'] = false;

    pgHeader.setSource(store);

    pgHeader.getColumnModel().setConfig([
        { header: 'Name', sortable: false, dataIndex: 'name', id: 'name', renderer: RenderDisabled },
        { header: 'Value', sortable: false, dataIndex: 'value', id: 'value' }
    ]);

    function RenderDisabled(value) {
        return "<span style='color:green;'>Test</span>";
    }

    parent.RenderGrid(0, pgHeader);

    pgHeader.store.getById('Admin_FieldProperty_TextHeading').set('name', parent.Ext.LabelKey.Admin_FieldProperty_TextHeading);
    pgHeader.store.getById('Admin_FieldProperty_Font_Italic').set('name', parent.Ext.LabelKey.Admin_FieldProperty_Font_Italic);

    // set value into pgHeader
    if (parent.Ext.Const.IsSupportMultiLang) {
        var defaultLangValue = getLabelValue(getLabelKey($text.attr("ng-labelkey")), "defaultLanguageText");
        var defaultLangCell = pgHeader.store.getById('Admin_FieldProperty_TextHeading_DefaultLanguage');
        defaultLangCell.set('name', parent.Ext.LabelKey.Admin_FieldProperty_TextHeading + '(Default Language)');
        defaultLangCell.set('value', parent.RemoveLabelStyle(defaultLangValue));
    }

    // add <font> tag if don't have
    if ($text.html().search(/<font/i) <0) {
        $text.html('<font style="text-align:left;">' + $text.html() + "</font>");
    }

    pgHeader.store.getById('Default Label').set('value', parent.DecodeHTMLTag(getLabelValue(getLabelKey($text.attr("ng-labelkey")), "defaultLabel")));
    pgHeader.store.getById('Admin_FieldProperty_TextHeading').set('value', parent.RemoveLabelStyle($text.html()));

    if ($text.html().toLowerCase().indexOf('lighter') > 0 || regFontWeightNormal.test($text.html())) {
        pgHeader.store.getById('Font-Bold').set('value', false);
    }
    else if (regFontWeightBold.test($text.html())) {
        pgHeader.store.getById('Font-Bold').set('value', true);
    }

    var isOriginalItalic = $text.hasClass("ACA_TabRow_Italic") && !regFontStyleNormal.test($text.html());
    if (regFontStyleItalic.test($text.html()) || isOriginalItalic) {
        pgHeader.store.getById('Admin_FieldProperty_Font_Italic').set('value', true);
    }
}

/*
 * link.
 */
function renderLink(obj) {
    var $obj = $(obj);
    var $link = $obj.find('a');

    var selectedCol = -1;
    var pgHeader = new parent.Ext.grid.PropertyGrid({
        id: 'pgHeader',
        nameText: 'Properties Grid',
        closable: true,
        autoHeight: true,
        customEditors: {
            "Default Label": new parent.DisabledGridEditor(),
            "Text Heading(Default Language)": new parent.DisabledGridEditor(),
            "Text Heading": new parent.Ext.grid.GridEditor(new parent.Ext.form.Field({
                disabled: false,
                disabledClass: 'x-DefaultHeading',
                cls: "x-DefaultHeading"
            })),
            'Font-Bold': new parent.BoolComboBoxGridEditor(),
            'Font-Italic': new parent.BoolComboBoxGridEditor()
        },
        listeners: {
            cellclick: function (grid, rowIndex, columnIndex, e) {
                selectedCol = (columnIndex != 1 ? 1 : columnIndex);
            },
            propertychange: function (source, recordId, value, oldValue) {
                //check if text changed
                var isTextUpdated = (value != oldValue);

                if (selectedCol == 1) {
                    if (recordId == 'Admin_FieldProperty_TextHeading') {
                        SetTextHeadingValue($link[0], value);
                        parent.UpdateDefaultLangValue(pgHeader, value, 'Admin_FieldProperty_TextHeading_DefaultLanguage');
                    } else if (recordId == 'Font-Bold') {
                        // tag <b>
                        $link.html($link.html().replace(regBtag, ''));
                        $link.html('<b style="font-weight:' + (parent.IsTrue(value) ? 'bold' : 'normal') + ';">' + $link.html() + "</b>");
                    } else if (recordId == 'Admin_FieldProperty_Font_Italic') {
                        // tag <i>
                        $link.html($link.html().replace(regItag, ''));
                        $link.html('<i style="font-style:' + (parent.IsTrue(value) ? 'italic' : 'normal') + ';">' + $link.html() + "</i>");
                    } else if (recordId == 'Admin_FieldProperty_LinkURL') {
                        if (value && value.indexOf('http') != 0) {
                            value = "http://" + value;
                        }

                        $link.attr('href', value);
                    }

                    if (isTextUpdated) {
                        var changeItem;

                        var heading = pgHeader.store.getById('Admin_FieldProperty_TextHeading').get('value');
                        if (heading == '') {
                            $obj.html('');
                        }

                        var displayLabel = $obj.html();

                        changeItem = new parent.TextObj(parent.Ext.Const.OpenedId, $obj.attr("id"), getLabelKey($obj.attr("ng-labelkey")), displayLabel, '');
                        parent.SetPageFlowCode4Item(changeItem);

                        parent.changeItems.UpdateItem(0, changeItem);
                        parent.ModifyMark();
                    }
                }
            },
        }
    });

    pgHeader.store.sortInfo = null;
    var store = {};
    store['Default Label'] = '';
    if (parent.Ext.Const.IsSupportMultiLang) {
        store['Admin_FieldProperty_TextHeading_DefaultLanguage'] = "";
    }
    store['Admin_FieldProperty_TextHeading'] = '';
    store['Font-Bold'] = '';
    store['Admin_FieldProperty_Font_Italic'] = false;
    store['Admin_FieldProperty_LinkURL'] = '';

    pgHeader.setSource(store);

    pgHeader.getColumnModel().setConfig([
        { header: 'Name', sortable: false, dataIndex: 'name', id: 'name', renderer: RenderDisabled },
        { header: 'Value', sortable: false, dataIndex: 'value', id: 'value' }
    ]);

    function RenderDisabled(value) {
        return "<span style='color:green;'>Test</span>";
    }

    parent.RenderGrid(0, pgHeader);

    // set pgHeader's name column
    pgHeader.store.getById('Admin_FieldProperty_TextHeading').set('name', parent.Ext.LabelKey.Admin_FieldProperty_TextHeading);
    pgHeader.store.getById('Admin_FieldProperty_Font_Italic').set('name', parent.Ext.LabelKey.Admin_FieldProperty_Font_Italic);

    if (parent.Ext.Const.IsSupportMultiLang) {
        var defaultLangValue = getLabelValue(getLabelKey($obj.attr("ng-labelkey")), "defaultLanguageText");
        var defaultLangCell = pgHeader.store.getById('Admin_FieldProperty_TextHeading_DefaultLanguage');
        defaultLangCell.set('name', parent.Ext.LabelKey.Admin_FieldProperty_TextHeading + '(Default Language)');
        defaultLangCell.set('value', trimHtmlTag(defaultLangValue));
    }

    pgHeader.store.getById('Admin_FieldProperty_LinkURL').set('name', parent.Ext.LabelKey.Admin_FieldProperty_LinkURL);

    // set pgHeader's value conlumn
    // add <font> tag if don't have
    if ($link.html().search(/<font/i) < 0) {
        $link.html('<font style="text-align:left;">' + $link.html() + "</font>");
    }

    pgHeader.store.getById('Default Label').set('value', trimHtmlTag(getLabelValue(getLabelKey($obj.attr("ng-labelkey")), "defaultLabel")));
    pgHeader.store.getById('Admin_FieldProperty_TextHeading').set('value', parent.RemoveLabelStyle($link.html()));

    if ($link.html().toLowerCase().indexOf('lighter') > 0 || regFontWeightNormal.test($link.html())) {
        pgHeader.store.getById('Font-Bold').set('value', false);
    }
    else if (regFontWeightBold.test($link.html())) {
        pgHeader.store.getById('Font-Bold').set('value', true);
    }

    var isOriginalItalic = $link.hasClass("ACA_TabRow_Italic") && !regFontStyleNormal.test($link.html());
    if (regFontStyleItalic.test($link.html()) || isOriginalItalic) {
        pgHeader.store.getById('Admin_FieldProperty_Font_Italic').set('value', true);
    }

    pgHeader.store.getById('Admin_FieldProperty_LinkURL').set('value', $link.attr('href'));
}

/**
 * @description
 * 
 * Get the label key literals from ng-labelkey attribute.
 *
 * The ng-labelkey may be like the follows:
 * format(object.labelkey,formatstr)
 * format(object.labelkey)
 * object.labelkey
 * labelkey
 * 
 * @param {string} str The ng-labelkey attribute value.
 * @returns {string} labelkey literal.
 *  
 */
function getLabelKey(str) {
    if (str) {
        var arr = str.split(/\.|,/,2);
        if (arr.length ==1) {
            return str;
        }

        return arr[1];
    }

    return '';
}

function getLabelValue(labelKey,actionName) {
    var text;

    if (!labelKey) {
        text= '';
    } else {
        $.ajax({
            type: "get",
            async: false,
            url: servicePath + "api/LabelKey/" + actionName,
            data: { labelKey: labelKey },
            success: function(data) {
                text = data;
            },
            error: function() {
                text = '';
            }
        });
    }

    return text;
}

function InsertFontTag($obj) {
    if (($obj).find("font").length == 0) {
        $obj.html("<font style='text-align:left;'>" + $obj.html() + "</font>");
    }
}

//set the text heading value
function SetTextHeadingValue(obj, value) {
    var dom;
    //we should find out the last tag, for the reason that the text heading object has inner tags, 
    //and we only need to set the value to last one
    //e.g. <b><i><font>the core text</font></i></b>
    for (var lastDom = obj.firstChild; lastDom && lastDom.hasChildNodes(); lastDom = lastDom.firstChild) {
        dom = lastDom;
    }

    if (typeof dom == "undefined") {
        dom = obj;
    }

    dom.innerHTML = parent.EncodeHTMLTag(value);
}

function selectedElement(obj) {
    unselectedElement();

    $(obj).addClass('ACA_Control_HighLight');
    document.selectedEle = obj;
    document.loadEastPanel = function(controlType, obj) {
        loadEastPanel(controlType, obj);
    };
}

function unselectedElement() {
    var ele = document.selectedEle;

    if (ele) {
        $(ele).removeClass('ACA_Control_HighLight');
        document.loadEastPanel = undefined;
    }
}
