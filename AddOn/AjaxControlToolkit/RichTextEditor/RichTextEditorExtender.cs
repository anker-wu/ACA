// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Drawing;

#region Assembly Resource Attribute
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.RichTextEditorBehavior.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.RichTextEditor.css", "text/css", PerformSubstitution = true)]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.paste.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.bold.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.justify_center.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.justify_left.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.justify_right.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.justify_full.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.ol.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.ul.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.underline.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.outdent.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.indent.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.table.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.image.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.preview.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.source.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.html.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.separator.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.start.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.arrow.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.remove.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.redo.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.undo.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.hrule.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.cut.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.copy.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.link.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.color.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.italic.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.RichTextEditor.icons.bgcolor.gif", "image/gif")]
#endregion

namespace AjaxControlToolkit
{
    [Designer("AjaxControlToolkit.RichTextEditorDesigner, AjaxControlToolkit")]
    [ClientCssResource("AjaxControlToolkit.RichTextEditor.RichTextEditor.css")]
    [ClientScriptResource("AjaxControlToolkit.RichTextEditorBehavior", "AjaxControlToolkit.RichTextEditor.RichTextEditorBehavior.js")]
    [TargetControlType(typeof(Panel))]
    [RequiredScript(typeof(BlockingScripts))]
    public class RichTextEditorExtender : ExtenderControlBase
    {
        /// <summary>
        /// Editor Mode
        /// </summary>
        [DefaultValue("editor")]
        [ExtenderControlProperty]
        public string Mode
        {
            get { return GetPropertyValue("Mode", "editor"); }
            set { SetPropertyValue("Mode", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
                SetPropertyValue("ID", value);
            }
        }
        
        [DefaultValue("")]
        [ExtenderControlProperty]
        public string Text
        {
            get { return GetPropertyValue("Text", string.Empty); }
            set { SetPropertyValue("Text", value); }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("buttonface")]
        [ExtenderControlProperty]
        public string EditorBGColor
        {
            get { return GetPropertyValue("EditorBGColor", "buttonface"); }
            set { SetPropertyValue("EditorBGColor", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("2px groove")]
        [ExtenderControlProperty]
        public string EditorBorder
        {
            get { return GetPropertyValue("EditorBorder", "2px groove"); }
            set { SetPropertyValue("EditorBorder", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(460)]
        [ExtenderControlProperty]
        public int TextWidth
        {
            get { return GetPropertyValue("TextWidth", 460); }
            set { SetPropertyValue("TextWidth", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(120)]
        [ExtenderControlProperty]
        public int TextHeight
        {
            get { return GetPropertyValue("TextHeight", 120); }
            set { SetPropertyValue("TextHeight", value); }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("white")]
        [ExtenderControlProperty]
        public string TextBGColor
        {
            get { return GetPropertyValue("TextBGColor", "white"); }
            set { SetPropertyValue("TextBGColor", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("2px inset")]
        [ExtenderControlProperty]
        public string TextBorder
        {
            get { return GetPropertyValue("TextBorder", "2px inset"); }
            set { SetPropertyValue("TextBorder", value); }
        }

       /// <summary>
        /// 
        /// </summary>#
        [DefaultValue("Verdana, Arial, Helvetica")]
        [ExtenderControlProperty]
        public string TextFont
        {
            get { return GetPropertyValue("TextFont", "Verdana, Arial, Helvetica"); }
            set { SetPropertyValue("TextFont", value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(12)]
        [ExtenderControlProperty]
        public int TextFontSize
        {
            get { return GetPropertyValue("TextFont", 12); }
            set { SetPropertyValue("TextFont", value); }
        }

        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("boldImageUrl")]
        public string BoldImageUrl
        {
            get { return GetPropertyValue("BoldImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.bold.gif"); }
            set { SetPropertyValue("BoldImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("italicImageUrl")]
        public string ItalicImageUrl
        {
            get { return GetPropertyValue("ItalicImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.italic.gif"); }
            set { SetPropertyValue("ItalicImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("underlineImageUrl")]
        public string UnderlineImageUrl
        {
            get { return GetPropertyValue("UnderlineImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.underline.gif"); }
            set { SetPropertyValue("UnderlineImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("bgcolorImageUrl")]
        public string BGColorImageUrl
        {
            get { return GetPropertyValue("BGColorImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.bgcolor.gif"); }
            set { SetPropertyValue("BGColorImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("colorImageUrl")]
        public string ColorImageUrl
        {
            get { return GetPropertyValue("ColorImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.color.gif"); }
            set { SetPropertyValue("ColorImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("justify_leftImageUrl")]
        public string Justify_LeftImageUrl
        {
            get { return GetPropertyValue("Justify_LeftImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.justify_left.gif"); }
            set { SetPropertyValue("Justify_LeftImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("justify_rightImageUrl")]
        public string Justify_RightImageUrl
        {
            get { return GetPropertyValue("Justify_RightImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.justify_right.gif"); }
            set { SetPropertyValue("Justify_RightImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("justify_fullImageUrl")]
        public string Justify_FullImageUrl
        {
            get { return GetPropertyValue("Justify_FullImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.justify_full.gif"); }
            set { SetPropertyValue("Justify_FullImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("justify_centerImageUrl")]
        public string Justify_CenterImageUrl
        {
            get { return GetPropertyValue("Justify_CenterImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.justify_center.gif"); }
            set { SetPropertyValue("Justify_CenterImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("linkImageUrl")]
        public string LinkImageUrl
        {
            get { return GetPropertyValue("LinkImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.link.gif"); }
            set { SetPropertyValue("LinkImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("cutImageUrl")]
        public string CutImageUrl
        {
            get { return GetPropertyValue("CutImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.cut.gif"); }
            set { SetPropertyValue("CutImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("copyImageUrl")]
        public string CopyImageUrl
        {
            get { return GetPropertyValue("CopyImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.copy.gif"); }
            set { SetPropertyValue("CopyImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("pasteImageUrl")]
        public string PasteImageUrl
        {
            get { return GetPropertyValue("PasteImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.paste.gif"); }
            set { SetPropertyValue("PasteImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("hruleImageUrl")]
        public string HRuleImageUrl
        {
            get { return GetPropertyValue("HRuleImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.hrule.gif"); }
            set { SetPropertyValue("HRuleImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("tableImageUrl")]
        public string TableImageUrl
        {
            get { return GetPropertyValue("TableImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.table.gif"); }
            set { SetPropertyValue("TableImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("insertImageUrl")]
        public string InsertImageUrl
        {
            get { return GetPropertyValue("InsertImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.image.gif"); }
            set { SetPropertyValue("InsertImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("olImageUrl")]
        public string OLImageUrl
        {
            get { return GetPropertyValue("OLImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.ol.gif"); }
            set { SetPropertyValue("OLImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("ulImageUrl")]
        public string ULImageUrl
        {
            get { return GetPropertyValue("ULImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.ul.gif"); }
            set { SetPropertyValue("ULImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("outdentImageUrl")]
        public string OutdentImageUrl
        {
            get { return GetPropertyValue("OutdentImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.outdent.gif"); }
            set { SetPropertyValue("OutdentImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("indentImageUrl")]
        public string IndentImageUrl
        {
            get { return GetPropertyValue("IndentImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.indent.gif"); }
            set { SetPropertyValue("IndentImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("undoImageUrl")]
        public string UndoImageUrl
        {
            get { return GetPropertyValue("UndoImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.undo.gif"); }
            set { SetPropertyValue("UndoImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("redoImageUrl")]
        public string RedoImageUrl
        {
            get { return GetPropertyValue("RedoImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.redo.gif"); }
            set { SetPropertyValue("RedoImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("removeImageUrl")]
        public string RemoveImageUrl
        {
            get { return GetPropertyValue("RemoveImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.remove.gif"); }
            set { SetPropertyValue("RemoveImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("arrowImageUrl")]
        public string ArrowImageUrl
        {
            get { return GetPropertyValue("ArrowImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.arrow.gif"); }
            set { SetPropertyValue("ArrowImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("startImageUrl")]
        public string StartImageUrl
        {
            get { return GetPropertyValue("StartImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.start.gif"); }
            set { SetPropertyValue("StartImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("separatorImageUrl")]
        public string SeparatorImageUrl
        {
            get { return GetPropertyValue("SeparatorImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.separator.gif"); }
            set { SetPropertyValue("SeparatorImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("htmlImageUrl")]
        public string HtmlImageUrl
        {
            get { return GetPropertyValue("HtmlImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.html.gif"); }
            set { SetPropertyValue("HtmlImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("sourceImageUrl")]
        public string SourceImageUrl
        {
            get { return GetPropertyValue("SourceImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.source.gif"); }
            set { SetPropertyValue("SourceImageUrl", value); }
        }
        [DefaultValue("")]
        [UrlProperty]
        [ExtenderControlProperty]
        [ClientPropertyName("previewImageUrl")]
        public string PreviewImageUrl
        {
            get { return GetPropertyValue("PreviewImageUrl", (string)null) ?? Page.ClientScript.GetWebResourceUrl(typeof(RichTextEditorExtender), "AjaxControlToolkit.RichTextEditor.icons.preview.gif"); }
            set { SetPropertyValue("PreviewImageUrl", value); }
        }
    }
}
