<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Accela.ACA.Common" %>

var GLOBAL_TEXT_CULTURE = "<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>";
var GLOBAL_SERVICE_PROVIDER_CULTURE = "<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>";
var GLOBAL_CURRENCY_SYMBOL = "<%=Accela.ACA.Common.Util.I18nNumberUtil.CurrencySymbol %>";
var GLOBAL_CURRENCY_PATTERN = "<%=Accela.ACA.Common.Util.I18nNumberUtil.CurrencyPattern %>";
var GLOBAL_CURRENCY_GROUP_SEPARATOR = "<%=Accela.ACA.Common.Util.I18nNumberUtil.CurrencyGroupSeparator %>";
var GLOBAL_CURRENCY_DECIMAL_SEPARATOR = "<%=Accela.ACA.Common.Util.I18nNumberUtil.CurrencyDecimalSeparator %>";
var GLOBAL_NUMBER_DECIMAL_SEPARATOR = "<%=Accela.ACA.Common.Util.I18nNumberUtil.NumberDecimalSeparator %>";
var GLOBAL_NEGATIVE_SIGN = "<%=Accela.ACA.Common.Util.I18nNumberUtil.NegativeSign %>";
var GLOBAL_VALIDATION_RESULTS_ACCESSKEY = "<%=Accela.ACA.Common.Util.AccessibilityUtil.GetAccessKey(Accela.ACA.Common.AccessKeyType.ValidationResults) %>";
var GLOBAL_ACCESSIBILITY_ENABLED = "<%=Accela.ACA.Common.Util.AccessibilityUtil.AccessibilityEnabled %>".toLowerCase()=="true";
var GLOBAL_DISABLE_EXPRESSION_SECTION508 ="<%=StandardChoiceUtil.IsDisableExpessionAlert()%>".toLowerCase() == "true";
var GLOBAL_APPLICATION_ROOT = "<%=FileUtil.ApplicationRoot %>";
var GLOBAL_CALENDARTEXT_CLIENTSTATE = "<%= ACAConstant.CLIENT_STATE %>";
var GLOBAL_CALENDARTEXT_ISLAMIC_CALENDAR = "<%= ACAConstant.ISLAMIC_CALENDAR %>";

var getText = new function() {
    this.global_js_showError_title = '<%=LabelUtil.GetGlobalTextByKey("aca_global_js_showerror_title").Replace("'","\\'") %>';
    this.global_js_showError_alt = '<%=LabelUtil.GetGlobalTextByKey("aca_global_js_showerror_alt").Replace("'","\\'") %>';
    this.global_js_showError_src = '<%=ImageUtil.GetImageURL("error_24.gif") %>';
    this.global_js_showConfirm_title = '<%=LabelUtil.GetGlobalTextByKey("aca_global_js_showconfirm_title").Replace("'","\\'") %>';
    this.global_js_showConfirm_src = '<%=ImageUtil.GetImageURL("confirmation_24.gif") %>';
    this.global_js_showNotice_title = '<%=LabelUtil.GetGlobalTextByKey("aca_global_js_shownotice_title").Replace("'","\\'") %>';
    this.global_js_showNotice_src = '<%=ImageUtil.GetImageURL("notice_24.gif")%>';
    this.global_js_showLoading_title = '<%=LabelUtil.GetGlobalTextByKey("aca_global_msg_loading").Replace("'", "\\'")%>';
    this.global_js_showLoading_src = '<%=ImageUtil.GetImageURL("loading.gif")%>';

    this.FileUploadProgress_js_NeatUploadUpdateHtml_error = '<%= LabelUtil.GetGlobalTextByKey("aca_fileuploadprogress_js_neatuploadupdatehtml_error").Replace("\"","\\\"") %>';
    this.masked_js_AV360Mask_attach = '<%=LabelUtil.GetGlobalTextByKey("aca_masked_js_av360mask_attach").Replace("'","\\'") %>';
    this.masked_iframe_title = '<%=LabelUtil.GetGlobalTextByKey("aca_silverlight_masked_label_title").Replace("'","\\'") %>';

    this.global_section508_more = '<%=LabelUtil.GetGlobalTextByKey("aca_section508_validation_more").Replace("'","\\'") %>';
    this.global_section508_required = '<%=LabelUtil.GetGlobalTextByKey("aca_required_field").Replace("'","\\'") %>';
    this.global_section508_errornotice1 = '<%=LabelUtil.GetGlobalTextByKey("aca_section508_validation_errornotice1").Replace("'","\\'") %>';
    this.global_section508_errornotice2 = '<%=LabelUtil.GetGlobalTextByKey("aca_section508_validation_errornotice2").Replace("'","\\'") %>';
    this.global_js_expression = '<%=LabelUtil.GetGlobalTextByKey("global_js_expression").Replace("'","\\'") %>';
        
    this.global_js_section_CTreeTop = '<%=ImageUtil.GetImageURL("caret_collapsed.gif") %>';
    this.global_js_section_ETreeTop = '<%=ImageUtil.GetImageURL("caret_expanded.gif") %>';
    this.global_js_section_altCollapsed = '<%=LabelUtil.GetTextByKey("img_alt_collapse_icon",string.Empty).Replace("'","\\'") %>';
    this.global_js_section_altExpanded = '<%=LabelUtil.GetTextByKey("img_alt_expand_icon",string.Empty).Replace("'","\\'") %>';
        
    this.global_js_changevalue_tip = '<%=LabelUtil.GetTextByKey("aca_common_msg_changevalue_tip",string.Empty).Replace("'","\\'") %>';
    this.global_js_close_alt = '<%=LabelUtil.GetTextByKey("aca_common_close",string.Empty).Replace("'","\\'") %>';

    this.iframe_nonsupport_message = '<%=LabelUtil.GetTextByKey("iframe_nonsupport_message",string.Empty).Replace("'","\\'") %>';
    this.iframe_nonsrc_nonsupport_message = '<%=LabelUtil.GetTextByKey("iframe_nonsrc_nonsupport_message",string.Empty).Replace("'","\\'") %>';
};