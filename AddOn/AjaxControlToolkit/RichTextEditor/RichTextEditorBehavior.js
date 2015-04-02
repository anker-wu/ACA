// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.
Type.registerNamespace('AjaxControlToolkit');
//Start Browser check - Check Browser and DesignMode
var isOpera = (Sys.Browser.agent == Sys.Browser.Opera) ? true : false;
var isInternetExplorer = (Sys.Browser.agent === Sys.Browser.InternetExplorer)? true : false;
var isGecko = (navigator.userAgent.indexOf('Gecko') != -1) ? true : false;
var isSafari = (Sys.Browser.agent == Sys.Browser.Safari) ? true : false;
var isDesignModeSupported = (document.designMode && document.execCommand) ? true : false;
//End
var editor = new Array();
var mouseX, mouseY, winX, winY, scrollLeft, scrollTop;
//End Browser check
//---------------------------------------------------------------------------------------------------------
// Start Language settings - Need to put in seprate file/language
//---------------------------------------------------------------------------------------------------------
var txtParagraph       = AjaxControlToolkit.Resources.RTE_Paragraph;
var txtNormal          = AjaxControlToolkit.Resources.RTE_Normal;
var txtHeading         = AjaxControlToolkit.Resources.RTE_Heading;
var txtClearFormatting = AjaxControlToolkit.Resources.RTE_ClearFormatting;
var txtJustifyLeft     = AjaxControlToolkit.Resources.RTE_JustifyLeft;
var txtJustifyCenter   = AjaxControlToolkit.Resources.RTE_JustifyCenter;
var txtJustifyRight    = AjaxControlToolkit.Resources.RTE_JustifyRight;
var txtJustifyFull     = AjaxControlToolkit.Resources.RTE_JustifyFull;
var txtOrderedList     = AjaxControlToolkit.Resources.RTE_OrderedList;
var txtUnorderedList   = AjaxControlToolkit.Resources.RTE_UnorderedList;
var txtOutdent         = AjaxControlToolkit.Resources.RTE_Outdent;
var txtIndent          = AjaxControlToolkit.Resources.RTE_Indent;
var txtInsertHR        = AjaxControlToolkit.Resources.RTE_InsertHorizontalRule;
var txtInsertTable     = AjaxControlToolkit.Resources.RTE_InsertTable;
var txtInsertImage     = AjaxControlToolkit.Resources.RTE_InsertImage;
var txtInsertText      = AjaxControlToolkit.Resources.RTE_Inserttexthere;
var txtFont            = AjaxControlToolkit.Resources.RTE_Font;
var txtSize            = AjaxControlToolkit.Resources.RTE_Size;
var txtBold            = AjaxControlToolkit.Resources.RTE_Bold;
var txtItalic          = AjaxControlToolkit.Resources.RTE_Italic;
var txtUnderline       = AjaxControlToolkit.Resources.RTE_Underline;
var txtFontColor       = AjaxControlToolkit.Resources.RTE_FontColor;
var txtBGColor         = AjaxControlToolkit.Resources.RTE_BackgroundColor;
var txtHyperlink       = AjaxControlToolkit.Resources.RTE_Hyperlink;
var txtCut             = AjaxControlToolkit.Resources.RTE_Cut;
var txtCopy            = AjaxControlToolkit.Resources.RTE_Copy;
var txtPaste           = AjaxControlToolkit.Resources.RTE_Paste;
var txtUndo            = AjaxControlToolkit.Resources.RTE_Undo;
var txtRedo            = AjaxControlToolkit.Resources.RTE_Redo;
var txtBorder          = AjaxControlToolkit.Resources.RTE_Border;
var txtBorderColor     = AjaxControlToolkit.Resources.RTE_BorderColor;
var txtCellColor       = AjaxControlToolkit.Resources.RTE_CellColor;
var txtCellSpacing     = AjaxControlToolkit.Resources.RTE_CellSpacing;
var txtCellPadding     = AjaxControlToolkit.Resources.RTE_CellPadding;
var txtColumns         = AjaxControlToolkit.Resources.RTE_Columns;
var txtRows            = AjaxControlToolkit.Resources.RTE_Rows;
var txtCreate          = AjaxControlToolkit.Resources.RTE_Create;
var txtCancel          = AjaxControlToolkit.Resources.RTE_Cancel;
var txtValues          = AjaxControlToolkit.Resources.RTE_Values;
var txtLabels          = AjaxControlToolkit.Resources.RTE_Labels;
var txtBarColor        = AjaxControlToolkit.Resources.RTE_BarColor;
var txtLabelColor      = AjaxControlToolkit.Resources.RTE_LabelColor;
var txtViewValues      = AjaxControlToolkit.Resources.RTE_ViewValues;
var txtLegend          = AjaxControlToolkit.Resources.RTE_Legend;
var txtViewSource      = AjaxControlToolkit.Resources.RTE_ViewSource;
var txtViewEditor      = AjaxControlToolkit.Resources.RTE_ViewEditor;
var txtPreviewHtml     = AjaxControlToolkit.Resources.RTE_PreviewHTML;
var txtOK              = AjaxControlToolkit.Resources.RTE_OK;
//End Language settings
//---------------------------------------------------------------------------------------------------------
// Global functions
//---------------------------------------------------------------------------------------------------------

//function editorStore() {
//for(var i = 0; i < editor.length; i++) editor[i].store(false);
//}

function editorGetObj(id) {
var obj = false;
if(document.getElementById) obj = document.getElementById(id);
else if(document.all) obj = document.all[id];
return obj;
}

function editorSetUnselectable(elm) {
if(document.getElementsByTagName) {
if(elm && typeof(elm.tagName) != 'undefined') {
  if(elm.tagName != 'INPUT' && elm.tagName != 'TEXTAREA' && elm.tagName != 'IFRAME') {
    if(elm.hasChildNodes()) {
      for(var i = 0; i < elm.childNodes.length; i++) {
        editorSetUnselectable(elm.childNodes[i]);
      }
    }
    elm.unselectable = 'on';
  }
}
}
}

function editorGetScrollLeft() {
var scrollLeft = 0;
if(window.pageXOffset) scrollLeft = window.pageXOffset;
else if(document.documentElement && document.documentElement.scrollLeft)
scrollLeft = document.documentElement.scrollLeft;
else if(document.body && document.body.scrollLeft)
scrollLeft = document.body.scrollLeft;
return scrollLeft;
}

function editorGetScrollTop() {
var scrollTop = 0;
if(window.pageYOffset) scrollTop = window.pageYOffset;
else if(document.documentElement && document.documentElement.scrollTop)
scrollTop = document.documentElement.scrollTop;
else if(document.body && document.body.scrollTop)
scrollTop = document.body.scrollTop;
return scrollTop;
}

function editorGetWinXY() {
if(window.innerWidth) {
winX = window.innerWidth;
winY = window.innerHeight;
}
else if(document.documentElement && document.documentElement.clientWidth) {
winX = document.documentElement.clientWidth;
winY = document.documentElement.clientHeight;
}
else if(document.body && document.body.clientWidth) {
winX = document.body.clientWidth;
winY = document.body.clientHeight;
}
else {
winX = screen.width;
winY = screen.height;
}
scrollLeft = editorGetScrollLeft();
scrollTop = editorGetScrollTop();
}

function editorGetMouse(e) {
if(e && e.pageX != null) {
mouseX = e.pageX;
mouseY = e.pageY;
}
else if(event && event.clientX != null) {
mouseX = event.clientX + editorGetScrollLeft();
mouseY = event.clientY + editorGetScrollTop();
}
}

AjaxControlToolkit.RichTextEditorBehavior = function(element) {
    /// <summary>
    /// The RichTextEditorBehavior is used to convert panel into texteditor
    /// </summary>
    /// <param name="element" type="Sys.UI.DomElement" domElement="true">
    /// DOM Element the behavior is associated with
    /// </param>
    AjaxControlToolkit.RichTextEditorBehavior.initializeBase(this, [element]);

//---------------------------------------------------------------------------------------------------------
// Start Properties Configuration
//---------------------------------------------------------------------------------------------------------
this.editorBGColor = "buttonface";            // editor background color
this.editorBorder = "2px groove";             // editor border (CSS-spec: "size style color")

this.textWidth = 460;                         // text field width (pixels)
this.textHeight = 120;                        // text field height (pixels)
this.textBGColor = "white";                   // text field background color
this.textBorder = "2px inset";                // text field border (CSS-spec: "size style color")
this.textFont = "Verdana, Arial, Helvetica";  // text field font family (CSS-spec)
this.textFontSize = 12;                       // text field font size (pixels)

this.setFocus = false;                        // focus text field on load (true or false)
this.fieldName = "";                  // default field name
// End Properties Configuration
//Start Image setting
this.boldImage = 'bold.gif';
this.italicImage = 'italic.gif';
this.underlineImage = 'underline.gif';
this.bgColorImage = 'bgcolor.gif';
this.textColorImage = 'color.gif';
this.leftAlignImage = 'justify_left.gif';
this.hyperlinkImage = 'link.gif';
this.rightAlignImage = 'justify_right.gif';
this.justifyAlignImage = 'justify_full.gif';
this.centerAlignImage = 'justify_center.gif';
this.cutImage = 'cut.gif';
this.copyImage = 'copy.gif';
this.pasteImage = 'paste.gif';
this.insertLineImage = 'hrule.gif';
this.insertTableImage = 'table.gif';
this.insertPictureImage = 'image.gif';
this.orderListImage = 'ol.gif';
this.unOrderListImage = 'ul.gif';
this.outdentImage = 'outdent.gif';
this.indentImage = 'indent.gif';
this.undoImage = 'undo.gif';
this.redoImage = 'redo.gif';
this.clearFormatImage = 'remove.gif';
this.bgColorDropDownImage ='arrow.gif';
this.toolbarStartImage = 'start.gif';
this.toolbarSeparatorImage ='separator.gif';
this.viewEditorImage = 'html.gif';
this.viewSourceImage ='source.gif';
this.previewHtmlImage = 'preview.gif';
//End Image setting
//---------------------------------------------------------------------------------------------------------
// Functions
//---------------------------------------------------------------------------------------------------------
//end -rte

    // Properties
this.editor = 0;
this.id = 0;
this.curSelection = 0;
this.field = '';
this.curFontColor = 'black';
this.curBGColor = 'white';
this.mode = 'editor';
this.html = '';
this.text = '';
// Variables
this.richTextEditorID = '';
this.richTextEditorContainerPanel = null;
this.toolBarStyle = ''; 
//this.toolBarStyle = 'style="' +
//'background-color: ' + this.editorBGColor + '; ' +
//'border: 1px solid ' + this.editorBGColor + ';"';
this.iFrameStyle = ' style="' +
'width: ' + (this.textWidth + 4) + 'px; ' +
'height: ' + (this.textHeight + 56) + 'px; ' +
'border: ' + this.textBorder + ';"';
this.divStyle = ' style="' +
'background-color: ' + this.editorBGColor + '; ' +
                 'width: ' + (this.textWidth + 12) + 'px; ' +
                 'border: ' + this.editorBorder + ';"';
this.dialogStyle = ' style="' +
'background-color: ' + this.editorBGColor + '; ' +
                 'border: ' + this.editorBorder + ';"';
this.formFieldStyle = ' style="' +
                 'border: ' + this.editorBorder + ';"';
    // Delegates
}
AjaxControlToolkit.RichTextEditorBehavior.prototype = {
    initialize : function() {
        /// <summary>
        /// Initialize the behavior
        /// </summary>
        AjaxControlToolkit.RichTextEditorBehavior.callBaseMethod(this, 'initialize');
        //this.mode = this.get_Mode();
        this.richTextEditorContainerPanel = this.get_element();
        this.fieldName = this.richTextEditorID + "_textarea";
        this.loadContent(this.text);
        this.toggleSource("editor");
        document.onmousedown = editorGetMouse;
    },
    dispose : function() {
        /// <summary>
        /// Dispose the behavior
        /// </summary>
        AjaxControlToolkit.RichTextEditorBehavior.callBaseMethod(this, 'dispose');
    },
    get_cutImageUrl : function() {
        return this.cutImage;
    },
    set_cutImageUrl : function(value) {

        if (value != this.cutImage) {
            this.cutImage = value;
            this.raisePropertyChanged("cutImageUrl");
        }
    },
    get_copyImageUrl : function() {
        return this.copyImage;
    },
    set_copyImageUrl : function(value) {

        if (value != this.copyImage) {
            this.copyImage = value;
            this.raisePropertyChanged("copyImageUrl");
        }
    },
    get_pasteImageUrl : function() {
        return this.pasteImage;
    },
    set_pasteImageUrl : function(value) {

        if (value != this.pasteImage) {
            this.pasteImage = value;
            this.raisePropertyChanged("pasteImageUrl");
        }
    },
    
    get_boldImageUrl : function() {
        return this.boldImage;
    },
    set_boldImageUrl : function(value) {

        if (value != this.boldImage) {
            this.boldImage = value;
            this.raisePropertyChanged("boldImageUrl");
        }
    },
    get_italicImageUrl : function() {
        return this.italicImage;
    },
    set_italicImageUrl : function(value) {

        if (value != this.italicImage) {
            this.italicImage = value;
            this.raisePropertyChanged("italicImageUrl");
        }
    },
    get_underlineImageUrl : function() {
        return this.underlineImage;
    },
    set_underlineImageUrl : function(value) {

        if (value != this.underlineImage) {
            this.underlineImage = value;
            this.raisePropertyChanged("underlineImageUrl");
        }
    },
    get_outdentImageUrl : function() {
        return this.outdentImage;
    },
    set_outdentImageUrl : function(value) {

        if (value != this.outdentImage) {
            this.outdentImage = value;
            this.raisePropertyChanged("outdentImageUrl");
        }
    },
    get_indentImageUrl : function() {
        return this.indentImage;
    },
    set_indentImageUrl : function(value) {

        if (value != this.indentImage) {
            this.indentImage = value;
            this.raisePropertyChanged("indentImageUrl");
        }
    },
    get_bgcolorImageUrl : function() {
        return this.bgColorImage;
    },
    set_bgcolorImageUrl : function(value) {

        if (value != this.bgColorImage) {
            this.bgColorImage = value;
            this.raisePropertyChanged("bgcolorImageUrl");
        }
    },
    get_colorImageUrl : function() {
        return this.textColorImage;
    },
    set_colorImageUrl : function(value) {
        if (value != this.textColorImage) {
            this.textColorImage = value;
            this.raisePropertyChanged("colorImageUrl");
        }
    },
    get_linkImageUrl : function() {
        return this.hyperlinkImage;
    },
    set_linkImageUrl : function(value) {

        if (value != this.hyperlinkImage) {
            this.hyperlinkImage = value;
            this.raisePropertyChanged("linkImageUrl");
        }
    },
    get_hruleImageUrl : function() {
        return this.insertLineImage;
    },
    set_hruleImageUrl : function(value) {

        if (value != this.insertLineImage) {
            this.insertLineImage = value;
            this.raisePropertyChanged("hruleImageUrl");
        }
    },
    get_insertImageUrl : function() {
        return this.insertPictureImage;
    },
    set_insertImageUrl : function(value) {

        if (value != this.insertPictureImage) {
            this.insertPictureImage = value;
            this.raisePropertyChanged("insertImageUrl");
        }
    },
    get_tableImageUrl : function() {
        return this.insertTableImage;
    },
    set_tableImageUrl : function(value) {

        if (value != this.insertTableImage) {
            this.insertTableImage = value;
            this.raisePropertyChanged("tableImageUrl");
        }
    },
    get_justify_leftImageUrl : function() {
        return this.leftAlignImage;
    },
    set_justify_leftImageUrl : function(value) {

        if (value != this.leftAlignImage) {
            this.leftAlignImage = value;
            this.raisePropertyChanged("justify_leftImageUrl");
        }
    },
    get_justify_centerImageUrl : function() {
        return this.centerAlignImage;
    },
    set_justify_centerImageUrl : function(value) {

        if (value != this.centerAlignImage) {
            this.centerAlignImage = value;
            this.raisePropertyChanged("justify_centerImageUrl");
        }
    },
    get_justify_fullImageUrl : function() {
        return this.justifyAlignImage;
    },
    set_justify_fullImageUrl : function(value) {

        if (value != this.justifyAlignImage) {
            this.justifyAlignImage = value;
            this.raisePropertyChanged("justify_fullImageUrl");
        }
    },
    get_justify_rightImageUrl : function() {
        return this.rightAlignImage;
    },
    set_justify_rightImageUrl : function(value) {

        if (value != this.rightAlignImage) {
            this.rightAlignImage = value;
            this.raisePropertyChanged("justify_rightImageUrl");
        }
    },
    get_olImageUrl : function() {
        return this.orderListImage;
    },
    set_olImageUrl : function(value) {

        if (value != this.orderListImage) {
            this.orderListImage = value;
            this.raisePropertyChanged("olImageUrl");
        }
    },
    get_ulImageUrl : function() {
        return this.unOrderListImage;
    },
    set_ulImageUrl : function(value) {

        if (value != this.unOrderListImage) {
            this.unOrderListImage = value;
            this.raisePropertyChanged("ulImageUrl");
        }
    },
    get_undoImageUrl : function() {
        return this.undoImage;
    },
    set_undoImageUrl : function(value) {

        if (value != this.undoImage) {
            this.undoImage = value;
            this.raisePropertyChanged("undoImageUrl");
        }
    },
    get_redoImageUrl : function() {
        return this.redoImage;
    },
    set_redoImageUrl : function(value) {

        if (value != this.redoImage) {
            this.redoImage = value;
            this.raisePropertyChanged("redoImageUrl");
        }
    },
    get_removeImageUrl : function() {
        return this.clearFormatImage;
    },
    set_removeImageUrl : function(value) {

        if (value != this.clearFormatImage) {
            this.clearFormatImage = value;
            this.raisePropertyChanged("removeImageUrl");
        }
    },
    get_arrowImageUrl : function() {
        return this.bgColorDropDownImage;
    },
    set_arrowImageUrl : function(value) {

        if (value != this.bgColorDropDownImage) {
            this.bgColorDropDownImage = value;
            this.raisePropertyChanged("arrowImageUrl");
        }
    },
    get_startImageUrl : function() {
        return this.toolbarStartImage;
    },
    set_startImageUrl : function(value) {

        if (value != this.toolbarStartImage) {
            this.toolbarStartImage = value;
            this.raisePropertyChanged("startImageUrl");
        }
    },
    get_separatorImageUrl : function() {
        return this.toolbarSeparatorImage;
    },
    set_separatorImageUrl : function(value) {

        if (value != this.toolbarSeparatorImage) {
            this.toolbarSeparatorImage = value;
            this.raisePropertyChanged("separatorImageUrl");
        }
    },
    get_htmlImageUrl : function() {
        return this.viewEditorImage;
    },
    set_htmlImageUrl : function(value) {

        if (value != this.viewEditorImage) {
            this.viewEditorImage = value;
            this.raisePropertyChanged("htmlImageUrl");
        }
    },
    get_sourceImageUrl : function() {
        return this.viewSourceImage;
    },
    set_sourceImageUrl : function(value) {

        if (value != this.viewSourceImage) {
            this.viewSourceImage = value;
            this.raisePropertyChanged("sourceImageUrl");
        }
    },
    get_previewImageUrl : function() {
        return this.previewHtmlImage;
    },
    set_previewImageUrl : function(value) {

        if (value != this.previewHtmlImage) {
            this.previewHtmlImage = value;
            this.raisePropertyChanged("previewImageUrl");
        }
    },//start -rte
    get_Mode : function() {
        return this.mode;
    },
    set_Mode : function(value) {
        if (value != this.mode) {
            this.mode = value;
            this.raisePropertyChanged('Mode');
        }
    },
    get_Text : function() {
        return this.text;
    },
    set_Text : function(value) {
        if (value != this.text) {
            this.text = value;
            this.raisePropertyChanged('Text');
        }
    },
    get_CurBGColor : function() {
        return this.curBGColor;
    },
    set_CurBGColor : function(value) {
        if (value != this.curBGColor) {
            this.curBGColor = value;
            this.raisePropertyChanged('CurBGColor');
        }
    },
    get_CurFontColor : function() {
        return this.curFontColor;
    },
    set_CurFontColor : function(value) {
        if (value != this.curFontColor) {
            this.curFontColor = value;
            this.raisePropertyChanged('CurFontColor');
        }
    },
    get_ID : function() {
        return this.richTextEditorID;
    },
    set_ID : function(value) {
        if (value != this.richTextEditorID) {
            this.richTextEditorID = value;
            this.raisePropertyChanged('ID');
        }
    },
    get_EditorBGColor : function() {
        return this.editorBGColor;
    },
    set_EditorBGColor : function(value) {
        if (value != this.editorBGColor) {
            this.editorBGColor = value;
            this.raisePropertyChanged('EditorBGColor');
        }
    },
    get_EditorBorder : function() {
        return this.editorBorder;
    },
    set_EditorBorder : function(value) {
        if (value != this.editorBorder) {
            this.editorBorder = value;
            this.raisePropertyChanged('EditorBorder');
        }
    },
    get_TextWidth : function() {
        return this.textWidth;
    },
    set_TextWidth : function(value) {
        if (value != this.textWidth) {
            this.textWidth = value;
            this.raisePropertyChanged('TextWidth');
        }
    },
    get_TextHeight : function() {
        return this.textHeight;
    },
    set_TextHeight : function(value) {
        if (value != this.textHeight) {
            this.textHeight = value;
            this.raisePropertyChanged('TextHeight');
        }
    },
    get_TextBGColor : function() {
        return this.textBGColor;
    },
    set_TextBGColor : function(value) {
        if (value != this.textBGColor) {
            this.textBGColor = value;
            this.raisePropertyChanged('TextBGColor');
        }
    },
    get_TextBorder : function() {
        return this.textBorder;
    },
    set_TextBorder : function(value) {
        if (value != this.textBorder) {
            this.textBorder = value;
            this.raisePropertyChanged('TextBorder');
        }
    },
    get_TextFontSize : function() {
        return this.textFontSize;
    },
    set_TextFontSize : function(value) {
        if (value != this.textFontSize) {
            this.textFontSize = value;
            this.raisePropertyChanged('TextFontSize');
        }
    },
   get_TextFont : function() {
        return this.textFont;
    },
    set_TextFont : function(value) {
        if (value != this.textFont) {
            this.textFont = value;
            this.raisePropertyChanged('TextFont');
        }
    },
buildColorChart : function(mode){
var c = new Array();
// red
c[0] = new Array('FFEEEE', 'FFCCCC', 'FFAAAA', 'FF8888', 'FF6666', 'FF4444', 'FF2222', 'FF0000',
               'EE0000', 'CC0000', 'AA0000', '880000', '770000', '660000', '550000', '440000', '330000');
// green
c[1] = new Array('EEFFEE', 'CCFFCC', 'AAFFAA', '88FF88', '66FF66', '44FF44', '22FF22', '00FF00',
               '00EE00', '00CC00', '00AA00', '008800', '007700', '006600', '005500', '004400', '003300');
// blue
c[2] = new Array('EEEEFF', 'CCCCFF', 'AAAAFF', '8888FF', '6666FF', '4444FF', '2222FF', '0000FF',
               '0000EE', '0000CC', '0000AA', '000088', '000077', '000066', '000055', '000044', '000033');
// yellow
c[3] = new Array('FFFFEE', 'FFFFCC', 'FFFFAA', 'FFFF88', 'FFFF66', 'FFFF44', 'FFFF22', 'FFFF00',
               'EEEE00', 'CCCC00', 'AAAA00', '888800', '777700', '666600', '555500', '444400', '333300');
// pink
c[4] = new Array('FFEEFF', 'FFCCFF', 'FFAAFF', 'FF88FF', 'FF66FF', 'FF44FF', 'FF22FF', 'FF00FF',
               'EE00EE', 'CC00CC', 'AA00AA', '880088', '770077', '660066', '550055', '440044', '330033');
// brown
c[5] = new Array('FFF0D0', 'FFEECC', 'FFEEBB', 'FFDDAA', 'FFCC99', 'FFC090', 'EEBB88', 'DDAA77',
               'CC9966', 'BB8855', 'AA7744', '886633', '775522', '664411', '553300', '442200', '331100');
// cyan
c[6] = new Array('EEFFFF', 'CCFFFF', 'AAFFFF', '88FFFF', '66FFFF', '44FFFF', '22FFFF', '00FFFF',
               '00EEEE', '00CCCC', '00AAAA', '008888', '007777', '006666', '005555', '004444', '003333');
// grey
c[7] = new Array('FFFFFF', 'EEEEEE', 'DDDDDD', 'CCCCCC', 'BBBBBB', 'AAAAAA', 'A0A0A0', '999999',
               '888888', '777777', '666666', '555555', '444444', '333333', '222222', '111111', '000000');

var html = '<table border=0 cellspacing=1 cellpadding=1 bgcolor=#808080>';
var style, i, j;

for(i = 0; i < c.length; i++) {
html += '<tr>';

for(j = 0; j < c[i].length; j++) {
//TODO:need to investigate cursor property on firefox
var cursor  = '';
  if(isInternetExplorer) {
  cursor  = 'cursor:hand; ';
  }
  style = 'width:14px; height:14px; font-size:1px; ' + cursor + 'background-color:#' + c[i][j];
  html += '<td width=14 height=14 bgcolor=#' + c[i][j] + '>' +
          '<a href="javascript:editor[' + this.id + '].pickColor(\'#' + c[i][j] + '\', \'' + mode + '\')" ' +
          'title="#' + c[i][j] + '"><div style="' + style + '"></div></a></td>';
}
html += '</tr>';
}
html += '</table>';
return html;
}
,
getEditor : function() {
var e = false;
if(isGecko) e = document.getElementById('editorIFrame' + this.id).contentWindow;
else if(isInternetExplorer) e = document.frames('editorIFrame' + this.id);
if(e && !isDesignModeSupported) e = false;
return e;
}
,
initEditor : function(content) {
if(this.editor = this.getEditor()) {
  var html = '<html><head><style> ' +
             'BODY { ' +
             'margin: 4px; ' +
             'background-color: ' + this.textBGColor + '; ' +
             '} ' +
             'BODY, TD, TH { ' +
             'color: black; ' +
             'font-family: ' + this.textFont + '; ' +
             'font-size: ' + this.textFontSize + 'px; ' +
             '} ' +
             'TD { border: 1px dashed silver; } ' +
             'P { margin: 0px; } ' +
             '</style></head>' +
             '<body>' +
             content.replace(/<STYLE>[^<]+<\/STYLE>(\r?\n)*/gi, '') +
             '</body></html>';
  this.editor.document.designMode = 'on';
  if(isGecko) this.editor.document.execCommand('useCSS', false, false);
  this.editor.document.open();
  this.editor.document.write(html);
  this.editor.document.close();
  if(this.setFocus) this.editor.focus();
  //for(var i = document.forms.length - 1; i > 0 && !this.field; i--) {
    //if(document.forms[i].elements[this.fieldName + this.id]) {
      this.field = document.getElementById(this.fieldName + this.id);
   // }
  //}
  editorSetUnselectable(editorGetObj(this.richTextEditorID +  this.id));
}
else alert("Sorry, your browser doesn't support richtext editing!");
}
,
setButtonStyle : function(name, cls) {
var obj = editorGetObj(name + this.id);
if(obj) obj.className = cls + this.id;
obj = editorGetObj(name + 'Arrow' + this.id);
if(obj) obj.className = cls + this.id;
}
,
pickColor : function(color, mode) {
var obj = editorGetObj('dlg' + mode);
if(obj) obj.style.visibility = 'hidden';
obj = editorGetObj('cur' + mode + this.id);
if(obj) {
  obj.style.backgroundColor = color;
  if(mode == 'FontColor') this.curFontColor = color;
  else this.curBGColor = color;
}
this.setColor(mode);
}
,
setRTEHtml : function(data){
this.html = this.html + data;
}
,
setColor : function(mode) {
if(mode == 'FontColor') this.execCommand('foreColor', this.curFontColor);
else this.execCommand((isGecko ? 'hiliteColor' : 'backColor'), this.curBGColor);
}
,
changeColor : function(mode) {
this.viewDialog(mode,'');
}
,
viewDialog : function(mode, textToFocus) {
var obj = editorGetObj('dlg' + mode);
if(obj) {
  if(isInternetExplorer) {
    this.editor.focus();
    this.curSelection = this.editor.document.selection.createRange();
  }
  if(obj.style.visibility == 'visible') obj.style.visibility = 'hidden';
  else {
    var obj2 = editorGetObj('dlgBGColor');
    obj2.style.visibility = 'hidden';
    obj2 = editorGetObj('dlgFontColor');
    obj2.style.visibility = 'hidden';
    obj2 = editorGetObj('dlgImage');
    obj2.style.visibility = 'hidden';
    obj2 = editorGetObj('dlgTable');
    obj2.style.visibility = 'hidden';

    var wdth = hght = 0;
    var top = mouseY;
    var left = mouseX;

    if(document.getElementById) {
      wdth = obj.offsetWidth;
      hght = obj.offsetHeight;
    }
    else if(isInternetExplorer) {
      wdth = obj.style.pixelWidth;
      hght = obj.style.pixelHeight;
    }

    editorGetWinXY();
    if(left + wdth - scrollLeft > winX) left = winX + scrollLeft - wdth;
    if(top + hght - scrollTop > winY) top = winY + scrollTop - hght - 20;

    obj.style.left = (left - 50) + 'px';
    obj.style.top = top + 'px';
    obj.style.visibility = 'visible';

//TODO:need to fix focus issue
if(isInternetExplorer) {
    if(mode.indexOf('Color') == -1) {
       if(textToFocus != null){
            document.getElementById(textToFocus).focus();
      }
      if(mode == 'Image') document.getElementById(textToFocus).value = 'http://';
    }
    }
  }
}
}
,
execCommand : function(command, option) {
if(this.editor) {
  if(option == 'removeFormat') {
    command = option;
    option = null;
  }
  try {
    this.editor.document.execCommand(command, false, option);
  }
  catch(e) {
    alert(command + ": not supported");
  }
  this.editor.focus();
}
}
,
runCommand : function(cmd, opt) {
if(isInternetExplorer && !this.curSelection) {
  this.editor.focus();
  this.curSelection = this.editor.document.selection.createRange();
}
if(cmd && opt) {
  if(cmd == 'insertHTML' && isInternetExplorer) this.curSelection.pasteHTML(opt);
  else this.execCommand(cmd, opt);
}
else if(cmd) this.execCommand(cmd);
if(isInternetExplorer) this.curSelection = 0;
}

,
insertLink : function() {
if(isInternetExplorer) this.runCommand('createLink');
else {
  var url = prompt('URL:', 'http://');
  if(url && url != 'http://') this.runCommand('createLink', url);
}
}

,
insertImage : function() {
this.viewDialog('Image', 'URL');
}

,
insertTable : function() {
this.viewDialog('Table','Cols');
}

,
createImage : function() {
var obj = editorGetObj('dlgImage');
if(obj) obj.style.visibility = 'hidden';
var f = document.forms[0];
if(f.URL.value && f.URL.value != 'http://') {
  if(this.curSelection) this.runCommand('insertHTML', '<IMG src="' + f.URL.value + '">');
  else this.runCommand('insertImage', f.URL.value);
  }
}

,
createTable : function() {
var obj = editorGetObj('dlgTable');
if(obj) obj.style.visibility = 'hidden';
var f = document.forms[0];
if(f.Cols.value && f.Rows.value) {
  var border = f.Border.options[f.Border.selectedIndex].value;
  var html = '<table border=' + border;
  if(f.Spacing.value) html += ' cellspacing=' + f.Spacing.value;
  if(f.Padding.value) html += ' cellpadding=' + f.Padding.value;
  if(f.BorderColor.value && border > 0) html += ' bordercolor=' + f.BorderColor.value;
  html += '>';

  for(var i = j = 0; i < f.Rows.value; i++) {
    html += '<tr' + (f.CellColor.value ? ' bgcolor=' + f.CellColor.value : '') + '>';
    for(j = 0; j < f.Cols.value; j++) html += '<td>' + txtInsertText + '</td>';
    html += '</tr>';
  }
  html += '</table>';
  this.runCommand('insertHTML', html);
}
}
,
toggleSource : function(mode) {
var s = editorGetObj(this.fieldName + this.id);
var r = editorGetObj('editorIFrame' + this.id);
var t = editorGetObj('editorToolBar' + this.id);
var p = editorGetObj(this.fieldName + "_preview"+ this.id);
this.store(this.mode);
//if(this.mode != mode)
//{
if(mode == 'editor') {
  s.style.visibility = 'hidden';
  p.style.visibility = 'hidden';
  t.style.visibility = 'visible';
  r.style.width = this.textWidth + 'px';
  r.style.visibility = 'visible';
  this.editor.focus();
}
else if(mode == 'source'){
  p.style.visibility = 'hidden';
  t.style.visibility = 'hidden';
  r.style.visibility = 'hidden';
  r.style.width = '0px';
  s.style.visibility = 'visible';
  s.focus();
}
else if(mode == 'preview'){
  p.style.visibility = 'visible';
  p.innerHTML = this.field.value;
  t.style.visibility = 'hidden';
  r.style.visibility = 'hidden';
  r.style.width = '0px';
  s.style.visibility = 'hidden';
}
this.mode = mode;
//}
}
,
store : function(mode){
if(this.field) {
  if(mode == 'source') this.editor.document.body.innerHTML = this.field.value;
  else {
    var content = this.editor.document.body.innerHTML;
    this.field.value = content;
  }
}
}
,
InsertRow : function(tableID, rowIndex){
var tableName = document.getElementById(tableID);
var newRow = tableName.insertRow(rowIndex);
}
,
buildStyles : function(){
 this.setRTEHtml('<style> ' +
                 '#' + this.richTextEditorID + this.id + '{ ' +
                 'position: relative; ' +
                 'background-color: ' + this.editorBGColor + '; ' +
                 'width: ' + (this.textWidth + (isInternetExplorer ? 20 : 12)) + 'px; ' +
                 'margin: 0px; ' +
                 'padding: 4px; ' +
                 'border: ' + this.editorBorder + '; ' +
                 'text-align: left; ' +
                 '} ' +
                 '.cssIFrame' + this.id + ' { ' +
                 'margin: 2px; ' +
                 'padding: 0px; ' +
                 'width: ' + this.textWidth + 'px; ' +
                 'height: ' + this.textHeight + 'px; ' +
                 'border: ' + this.textBorder + '; ' +
                 '} ' +
                 '.cssToolBar' + this.id + ' { ' +
                 'background-color: ' + this.editorBGColor + '; ' +
                 'border: 1px solid ' + this.editorBGColor + '; ' +
                 'padding: 2px; ' +
                 '} ' +
                 '.cssRaised' + this.id + ' { ' +
                 'border-top: 1px solid buttonhighlight; ' +
                 'border-left: 1px solid buttonhighlight; ' +
                 'border-bottom: 1px solid buttonshadow; ' +
                 'border-right: 1px solid buttonshadow; ' +
                 'background-color: ' + this.editorBGColor + '; ' +
                 'padding: 2px; ' +
                 '} ' +
                 '.cssPressed' + this.id + ' { ' +
                 'border-top: 1px solid buttonshadow; ' +
                 'border-left: 1px solid buttonshadow; ' +
                 'border-bottom: 1px solid buttonhighlight; ' +
                 'border-right: 1px solid buttonhighlight; ' +
                 'background-color: ' + this.editorBGColor + '; ' +
                 'padding-left: 3px; ' +
                 'padding-top: 3px; ' +
                 'padding-bottom: 1px; ' +
                 'padding-right: 1px; ' +
                 '} ' +
                 '.cssSource' + this.id + ' { ' +
                 'position: absolute; ' +
                 'top: 0px; ' +
                 'left: 0px; ' +
                 'margin: 6px; ' +
                 'width: ' + (this.textWidth + 4) + 'px; ' +
                 'height: ' + (this.textHeight + 56 + 60) + 'px; ' +
                 'font-family: Courier New, Courier, Monospace; ' +
                 'font-size: 12px; ' +
                 'background-color: ' + this.textBGColor + '; ' +
                 'border: ' + this.textBorder + '; ' +
                 'visibility: hidden; ' +
                 '} ' +
                 '#editorButton' + this.id + ' { ' +
                 'font-family: Verdana, Arial, Helvetica; ' +
                 'font-size: 11px; ' +
                 'font-weight: bold; ' +
                 'background-color: ' + this.editorBGColor + '; ' +
                 '} ' +
                 '#curBGColor' + this.id + ' { ' +
                 'width: 16px; ' +
                 'height: 4px; ' +
                 'font-size: 1px; ' +
                 'background-color:' + this.curBGColor + '; ' +
                 '} ' +
                 '#curFontColor' + this.id + ' { ' +
                 'width: 16px; ' +
                 'height: 4px; ' +
                 'font-size: 1px; ' +
                 'background-color:' + this.curFontColor + '; ' +
                 '} ' +
                 '.cssPreview' + this.id + ' { ' +
                 'position: absolute; ' +
                 'top: 0px; ' +
                 'left: 0px; ' +
                 'margin: 0px; ' +
                 'width: ' + (this.textWidth + 4) + 'px; ' +
                 'height: ' + (this.textHeight + 56 + 60) + 'px; ' +
                 'font-family: Courier New, Courier, Monospace; ' +
                 'font-size: 12px; ' +
                 'background-color: ' + this.textBGColor + '; ' +
                 'border: ' + this.textBorder + '; ' +
                 'visibility: hidden; ' +
                 '} ' +
                 '</style>');
 }
,
buildToolbarButton : function(command, imageName, text){
this.setRTEHtml('<td class="cssToolBar' + this.id + '"' + this.toolBarStyle + ' width=20 height=20 onMouseOver="this.className=\'cssRaised' + this.id + '\'" onMouseOut="this.className=\'cssToolBar' + this.id + '\'" onMouseDown="this.className=\'cssPressed' + this.id + '\'" onMouseUp="this.className=\'cssRaised' + this.id + '\'" onClick="editor[' + this.id + '].runCommand(' + command + ')" title="' + text + '"><img src="' + imageName + '" width=16 height=16></td>');
}
,
buildToolbarButtonWithCustomAction : function(command, imageName, text)
{
  this.setRTEHtml('<td class="cssToolBar' + this.id + '"' + this.toolBarStyle + ' width=20 height=20 onMouseOver="this.className=\'cssRaised' + this.id + '\'" onMouseOut="this.className=\'cssToolBar' + this.id + '\'" onMouseDown="this.className=\'cssPressed' + this.id + '\'" onMouseUp="this.className=\'cssRaised' + this.id + '\'" onClick="editor[' + this.id + '].' + command + '" title="' + text + '"><img src="' + imageName + '" width=16 height=16></td>');
}
,
buildStart : function(){
  this.setRTEHtml('<td><img src="' + this.toolbarStartImage + '"></td>');
}
,
buildSeparator : function(){
  this.setRTEHtml('<td><img src="' + this.toolbarSeparatorImage + '"></td>');
}
,
buildFormatDropDown : function(){
                 this.setRTEHtml('<td><select style="font:menu" onChange="editor[' + this.id + '].runCommand(\'formatBlock\', this[this.selectedIndex].value); this.selectedIndex=0">' +
                 '<option style="background-color:' + this.editorBGColor + '">' + txtParagraph + ':' +
                 '<option value="<P>">' + txtNormal + ' &lt;P&gt;' +
                 '<option value="<H1>">' + txtHeading + ' 1 &lt;H1&gt;' +
                 '<option value="<H2>">' + txtHeading + ' 2 &lt;H2&gt;' +
                 '<option value="<H3>">' + txtHeading + ' 3 &lt;H3&gt;' +
                 '<option value="<H4>">' + txtHeading + ' 4 &lt;H4&gt;' +
                 '<option value="<H5>">' + txtHeading + ' 5 &lt;H5&gt;' +
                 '<option value="<H6>">' + txtHeading + ' 6 &lt;H6&gt;' +
                 '<option value="removeFormat">' + txtClearFormatting +
                 '</select></td>');
}
,
buildFontNameDropDown : function(){
                 this.setRTEHtml('<td><select style="font:menu" onChange="editor[' + this.id + '].runCommand(\'fontName\', this[this.selectedIndex].value); this.selectedIndex=0">' +
                 '<option style="background-color:' + this.editorBGColor + '">' + txtFont + ':' +
                 '<option value="Arial, Helvetica">Arial' +
                 '<option value="Verdana, Arial, Helvetica">Verdana' +
                 '<option value="Times New Roman, Times, Serif">Times' +
                 '<option value="Comic Sans MS">Comic' +
                 '<option value="MS Sans Serif, sans-serif">Sans-Serif' +
                 '<option value="Courier New, Courier, Monospace">Courier' +
                 '<option value="Trebuchet MS, Arial, Helvetica">Trebuchet' +
                 '</select></td>');
}
,
buildFontSizeDropDown : function(){
  this.setRTEHtml('<td><select style="font:menu" onChange="editor[' + this.id + '].runCommand(\'fontSize\', this[this.selectedIndex].text); this.selectedIndex=0">'+
                 '<option style="background-color:' + this.editorBGColor + '">' + txtSize + ':' +
                 '<option value="1">1' +
                 '<option value="2">2' +
                 '<option value="3">3' +
                 '<option value="4">4' +
                 '<option value="5">5' +
                 '<option value="6">6' +
                 '<option value="7">7' +
                 '</select></td>');
}
,
buildColorDropDown : function(command, imageName, dropDownImageName, text){
this.setRTEHtml('<td width=25 height=20 title="' + text + '"><table border=0 cellspacing=0 cellpadding=0><tr>' + 
'<td id="btn' + command + '' + this.id + '" class="cssToolBar' + this.id + '" onMouseOver="editor[' + this.id + '].setButtonStyle(\'btn' + command + '\', \'cssRaised\')" onMouseOut="editor[' + this.id + '].setButtonStyle(\'btn' + command + '\', \'cssToolBar\')" onMouseDown="this.className=\'cssPressed' + this.id + '\'" onMouseUp="this.className=\'cssRaised' + this.id + '\'" onClick="editor[' + this.id + '].setColor(\''+ command + '\')"><img src="' + imageName + '" width=16 height=12><div id="cur' + command + this.id + '"></div></td>' +
'<td id="btn' + command + 'Arrow' + this.id + '" class="cssToolBar' + this.id + '" onMouseOver="editor[' + this.id + '].setButtonStyle(\'' + command + '\', \'cssRaised\')" onMouseOut="editor[' + this.id + '].setButtonStyle(\'' + command + '\', \'cssToolBar\')" onMouseDown="this.className=\'cssPressed' + this.id + '\'" onMouseUp="this.className=\'cssRaised' + this.id + '\'" onClick="editor[' + this.id + '].changeColor(\'' + command + '\')"><img src="' + dropDownImageName + '" width=5 height=16></td>' +
'</tr></table></td>');
}
,
buildToolbars : function(){
this.setRTEHtml('<div id="editorToolBar' + this.id + '">');
this.setRTEHtml('<table border=0 cellspacing=2 cellpadding=0><tr id=Toolbar01 align=center>');
this.buildStart();
this.buildFormatDropDown();
this.buildToolbarButton('\'justifyLeft\'',this.leftAlignImage, txtJustifyLeft);
this.buildToolbarButton('\'justifyCenter\'', this.centerAlignImage, txtJustifyCenter);
this.buildToolbarButton('\'justifyRight\'',this.rightAlignImage ,txtJustifyRight);
this.buildToolbarButton('\'justifyFull\'',this.justifyAlignImage , txtJustifyFull);
this.buildSeparator();
this.buildToolbarButton('\'insertOrderedList\'',this.orderListImage , txtOrderedList);
this.buildToolbarButton('\'insertUnorderedList\'',this.unOrderListImage , txtUnorderedList);
this.buildToolbarButton('\'outdent\'',this.outdentImage , txtOutdent);
this.buildToolbarButton('\'indent\'',this.indentImage , txtIndent);
this.buildSeparator();
this.buildToolbarButton('\'insertHorizontalRule\'', this.insertLineImage, txtInsertHR);
this.buildToolbarButtonWithCustomAction('insertImage()',this.insertPictureImage, txtInsertImage);
this.buildToolbarButtonWithCustomAction('insertTable()',this.insertTableImage, txtInsertTable);
this.setRTEHtml('</tr></table>');
this.setRTEHtml('<div style="border:1px inset"></div>');
this.setRTEHtml('<table border=0 cellspacing=2 cellpadding=0><tr id=Toolbar02 align=center>');
this.buildStart();
this.buildFontNameDropDown();
this.buildFontSizeDropDown();
this.buildToolbarButton('\'bold\'',this.boldImage , txtBold);
this.buildToolbarButton('\'italic\'',this.italicImage , txtItalic);
this.buildToolbarButton('\'underline\'',this.underlineImage , txtUnderline);
this.buildColorDropDown('BGColor', this.bgColorImage, 'arrow.gif', txtBGColor);
this.buildColorDropDown('FontColor', this.textColorImage,'arrow.gif', txtFontColor);
this.buildToolbarButtonWithCustomAction('insertLink()',this.hyperlinkImage, txtHyperlink);
this.buildSeparator();
this.buildToolbarButton('\'cut\'',this.cutImage , txtCut);
this.buildToolbarButton('\'copy\'',this.copyImage , txtCopy);
this.buildToolbarButton('\'paste\'',this.pasteImage , txtPaste);
this.buildToolbarButton('\'undo\'',this.undoImage , txtUndo);
this.buildToolbarButton('\'redo\'',this.redoImage , txtRedo);
this.setRTEHtml('</tr></table>');
this.setRTEHtml('</div>');
}
,
buildEditArea : function(){
this.setRTEHtml('<iframe id="editorIFrame' + this.id + '" frameborder=0 class="cssIFrame' + this.id + '"' + this.iFrameStyle  + '></iframe>');
this.setRTEHtml('<textarea name="' + this.fieldName + this.id + '" id="' + this.fieldName + this.id + '" class="cssSource' + this.id + '"></textarea>');
this.setRTEHtml('<div id="' + this.fieldName + "_preview" + this.id + '" class="cssPreview' + this.id + '"></div>');
}
,
buildModes : function(){
this.setRTEHtml('<div id="editorMode' + this.id + '">');
this.setRTEHtml('<table border=0 cellspacing=0 cellpadding=0><tr id=Toolbar01 align=center>');
this.buildToolbarButtonWithCustomAction('toggleSource(\'editor\')',this.viewEditorImage, txtViewEditor);
this.buildToolbarButtonWithCustomAction('toggleSource(\'source\')',this.viewSourceImage, txtViewSource);
this.buildToolbarButtonWithCustomAction('toggleSource(\'preview\')',this.previewHtmlImage, txtPreviewHtml);
this.setRTEHtml('</tr></table>');
this.setRTEHtml('</div>');
}
,
buildEditor : function(){
this.buildStyles();
this.setRTEHtml('<div id="mainDivR' + this.id + '"' + this.divStyle + '>');
this.buildToolbars();
this.buildEditArea();
this.buildModes();
this.setRTEHtml('</div>');
}
,
showFontColorDialog : function(mode){
this.setRTEHtml('<div id="dlg' + mode + '" class="cssDialog"' + this.dialogStyle + '><center>' +
               this.buildColorChart(mode) +
               '<input type=button value="' + txtCancel + '" class="cssButton" onClick="editor[' + this.id + '].viewDialog(\'' + mode + '\')">' +
               '</center></div>');
}
,
showInsertImageDialog : function(){
this.setRTEHtml('<div id="dlgImage" class="cssDialog"' + this.dialogStyle + '><center>' +
               '<p class="cssFont1">' + txtInsertImage + '</p>' +
               '<table border=0 cellspacing=0>' +
'<tr><td class="cssFont2" nowrap>URL:</td><td>&nbsp;<input type=text name="URL" size=30 maxlength=100 class="cssFormField"' + this.formFieldStyle + '></td></tr>' +
'</table>' +
               '<table border=0 cellspacing=0 cellpadding=0 width=230><tr>' +
               '<td><input type=button value="' + txtCancel + '" class="cssButton" onClick="editor[' + this.id + '].viewDialog(\'Image\')"></td>' +
               '<td align=right><input type=button value=' + txtOK + ' class="cssButton" onclick="javascript:editor[' + this.id + '].createImage()"></td>' +
               '</tr></table>' +
               '</center></div>');
}
,
showInsertTableDialog : function(){
this.setRTEHtml('<div id="dlgTable" class="cssDialog"' + this.dialogStyle + '><center>' +
               '<p class="cssFont1">' + txtInsertTable + '</p>' +
               '<table border=0 cellspacing=0>' +
'<tr align=left><td class="cssFont2" nowrap>' + txtColumns + ':</td><td>&nbsp;<input type=text name="Cols" size=2 maxlength=2 class="cssFormField"' + this.formFieldStyle + '></td><td>&nbsp;</td><td class="cssFont2" nowrap>' + txtCellSpacing + ':</td><td>&nbsp;<input type=text name="Spacing" size=2 maxlength=2 class="cssFormField"' + this.formFieldStyle + ' value="2"></td></tr>' +
'<tr align=left><td class="cssFont2" nowrap>' + txtRows + ':</td><td>&nbsp;<input type=text name="Rows" size=2 maxlength=2 class="cssFormField"' + this.formFieldStyle + '></td><td>&nbsp;</td><td class="cssFont2" nowrap>' + txtCellPadding + ':</td><td>&nbsp;<input type=text name="Padding" size=2 maxlength=2 class="cssFormField"' + this.formFieldStyle + ' value="2"></td></tr>' +
'<tr align=left><td class="cssFont2" nowrap>' + txtBorder + ':</td><td>&nbsp;<select name="Border" class="cssFormField"' + this.formFieldStyle + '><option value="0">0<option value="1" selected>1<option value="2">2<option value="3">3<option value="4">4<option value="5">5</select></td><td>&nbsp;</td><td class="cssFont2" nowrap>' + txtBorderColor + ':</td><td>&nbsp;<input type=text name="BorderColor" size=10 maxlength=10 class="cssFormField"' + this.formFieldStyle + ' value="#000000"></td></tr>' +
'<tr align=left><td colspan=3></td><td class="cssFont2" nowrap>' + txtCellColor + ':</td><td>&nbsp;<input type=text name="CellColor" size=10 maxlength=10 class="cssFormField"' + this.formFieldStyle + '></td></tr>' +
'</table>' +
               '<table border=0 cellspacing=0 cellpadding=0 width=230><tr>' +
               '<td><input type=button value="' + txtCancel + '" class="cssButton" onClick="editor[' + this.id + '].viewDialog(\'Table\')"></td>' +
               '<td align=right><input type=button value="' + txtCreate + '" class="cssButton" onclick="javascript:editor[' + this.id + '].createTable(true)"></td>' +
               '</tr></table>' +
               '</center></div>');
}
,
loadContent : function(content) {
if(content == null) {content = ''};
this.id = editor.length;
if(editor[this.id] = this) {
  if((isInternetExplorer || isGecko) && isDesignModeSupported) {
    this.buildEditor();
        //---------------------------------------------------------------------------------------------------------
        // Global styles / dialog boxes
        //---------------------------------------------------------------------------------------------------------
        if((isInternetExplorer || isGecko) && isDesignModeSupported) {
        this.showFontColorDialog('BGColor');
        this.showFontColorDialog('FontColor');
        this.showInsertImageDialog();
        this.showInsertTableDialog();
        editorSetUnselectable(editorGetObj('dlgBGColor'));
        editorSetUnselectable(editorGetObj('dlgFontColor'));
        editorSetUnselectable(editorGetObj('dlgImage'));
        editorSetUnselectable(editorGetObj('dlgTable'));
        }
    this.richTextEditorContainerPanel.innerHTML = this.html;
    this.initEditor(content);
  }
  else {
    var cols = Math.round(this.textWidth / 10);
    var rows = Math.round(this.textHeight / 20);
    document.writeln('<textarea name="' + this.fieldName + this.id + '" id="' + this.fieldName + this.id + '" style="' +
                   'margin-bottom: 4px; ' +
                   'padding: 4px; ' +
                   'background-color: ' + this.textBGColor + '; ' +
                   'border: ' + this.textBorder +
                   '" cols=' + cols + ' rows=' + rows + ' wrap=virtual>' + content +
                   '</textarea>');
  }
}
else {alert("Could not load rich text editor!")};
}
//END - RTE
}
AjaxControlToolkit.RichTextEditorBehavior.registerClass('AjaxControlToolkit.RichTextEditorBehavior', AjaxControlToolkit.BehaviorBase);
