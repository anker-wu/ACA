<%@ Page Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Admin.JSONLabelKey" Codebehind="LabelKey.aspx.cs" %>

var GLOBAL_TEXT_CULTURE = "<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>";
var GLOBAL_SERVICE_PROVIDER_CULTURE = "<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>";
var GLOBAL_IS_MASTER_LANGUAGE = "<%=Accela.ACA.Common.Util.I18nCultureUtil.IsInMasterLanguage %>".toLowerCase() == "true";
var GLOBAL_AGENCY_CODE = "<%=ConfigManager.AgencyCode %>";
var GLOBAL_CURRENCY_SYMBOL = "<%=Accela.ACA.Common.Util.I18nNumberUtil.CurrencySymbol %>";
var GLOBAL_CURRENCY_PATTERN = "<%=Accela.ACA.Common.Util.I18nNumberUtil.CurrencyPattern %>";
var GLOBAL_CURRENCY_GROUP_SEPARATOR = "<%=Accela.ACA.Common.Util.I18nNumberUtil.CurrencyGroupSeparator %>";
var GLOBAL_CURRENCY_DECIMAL_SEPARATOR = "<%=Accela.ACA.Common.Util.I18nNumberUtil.CurrencyDecimalSeparator %>";
var GLOBAL_NUMBER_DECIMAL_SEPARATOR = "<%=Accela.ACA.Common.Util.I18nNumberUtil.NumberDecimalSeparator %>";
var GLOBAL_NEGATIVE_SIGN = "<%=Accela.ACA.Common.Util.I18nNumberUtil.NegativeSign %>";
var GLOBAL_VALIDATION_RESULTS_ACCESSKEY = "<%=Accela.ACA.Common.Util.AccessibilityUtil.GetAccessKey(Accela.ACA.Common.AccessKeyType.ValidationResults) %>";
var GLOBAL_ACCESSIBILITY_ENABLED = "<%=Accela.ACA.Common.Util.AccessibilityUtil.AccessibilityEnabled %>".toLowerCase() == "true";
var GLOBAL_APPLICATION_ROOT = "<%=FileUtil.ApplicationRoot %>";

Ext.LabelKey = function(){ 
	return { 
		init : function(){ 
		} 
	}; 
}();

Ext.Constant = function(){ 
	return { 
		init : function(){ 
		} 
	}; 
}(); 
  
Ext.LabelKey.DropDownDefaultText = '<%= GetTextByKey("aca_common_select").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Common_SaveSuccess = '<%= GetLabelKey("acaadmin_global_msg_savesuccess").Replace("'", "\\'") %>';
Ext.LabelKey.Admin_Common_Error = 'An error has occurred.\nWe are experiencing technical difficulties.\nPlease try again later or contact the Agency for assistance.';
Ext.LabelKey.Admin_MessageBox_Warning_Title = '<%= GetLabelKey("admin_messagebox_warning_title").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_DefaultLabel = '<%= GetLabelKey("Admin_FieldProperty_DefaultLabel").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_TextHeading = '<%= GetLabelKey("Admin_FieldProperty_TextHeading").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_ForeColor = '<%= GetLabelKey("Admin_FieldProperty_ForeColor").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_LinkURL = '<%= GetLabelKey("acaadmin_newui_fieldproperty_linkurl").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_BodyText = '<%= GetLabelKey("Admin_FieldProperty_BodyText").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_FieldLabel = '<%= GetLabelKey("Admin_FieldProperty_FieldLabel").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_WatermarkText = '<%= GetLabelKey("admin_fieldproperty_watermarktext").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_WatermarkText1 = '<%= GetLabelKey("acaadmin_fieldproperty_watermarktext_from").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_WatermarkText2 = '<%= GetLabelKey("acaadmin_fieldproperty_watermarktext_to").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_Instructions = '<%= GetLabelKey("Admin_FieldProperty_Instructions").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_DefaultValue = '<%= GetLabelKey("Admin_FieldProperty_DefaultValue").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_Choice = '<%= GetLabelKey("Admin_FieldProperty_Choice").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_ChoiceTip = '<%= GetLabelKey("Admin_FieldProperty_ChoiceTip").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_HyperLinkText = '<%= GetLabelKey("Admin_FieldProperty_HyperLinkText").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_ButtonLabel = '<%= GetLabelKey("Admin_FieldProperty_ButtonLabel").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_Report = '<%= GetLabelKey("Admin_FieldProperty_Report").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_SectionProperty_DefaultLabel = '<%= GetLabelKey("Admin_SectionProperty_DefaultLabel").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_SectionProperty_SectionHeading = '<%= GetLabelKey("Admin_SectionProperty_SectionHeading").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_SectionProperty_AvailableFields = '<%= GetLabelKey("Admin_SectionProperty_AvailableFields").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_SectionProperty_RemoveDateRangeFields = '<%= GetLabelKey("admin_sectionproperty_removedaterangefields").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_Font_Bold = '<%= GetLabelKey("Admin_FieldProperty_Font_Bold").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_Font_Italic = '<%= GetLabelKey("Admin_FieldProperty_Font_Italic").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_Font_Name = '<%= GetLabelKey("Admin_FieldProperty_Font_Name").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_Font_Names ='<%= GetLabelKey("Admin_FieldProperty_Font_Names").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_Font_Overline = '<%= GetLabelKey("Admin_FieldProperty_Font_Overline").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_Font_Size = '<%= GetLabelKey("Admin_FieldProperty_Font_Size").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_Font_Strikeout = '<%= GetLabelKey("Admin_FieldProperty_Font_Strikeout").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_Font_Underline = '<%= GetLabelKey("Admin_FieldProperty_Font_Underline").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_DefaultInstructions = '<%= GetLabelKey("Admin_FieldProperty_DefaultInstructions").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_AddButton = '<%= GetLabelKey("Admin_FieldProperty_AddButton").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_ChoiceTitle = '<%= GetLabelKey("Admin_FieldProperty_ChoiceTitle").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_LicenseType = '<%= GetLabelKey("aca_licensee_licenseType").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_ConflictedItem = '<%= GetLabelKey("aca_admin_dropdownlist_conflicteditem_errormes").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Logo_Display = '<%= GetLabelKey("Admin_Logo_Display").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_Pageflow_SameStepNameInfo= '<%= GetLabelKey("Admin_Pageflow_SameStepNameInfo").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_SamePageNameInfo= '<%= GetLabelKey("Admin_Pageflow_SamePageNameInfo").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_Menu_Preview = '<%= GetLabelKey("admin_menu_preview").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Menu_Save = '<%= GetLabelKey("admin_menu_save").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Menu_Close = '<%= GetLabelKey("admin_menu_close").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Menu_Help = '<%= GetLabelKey("admin_menu_help").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_LeftPanel_Navigation = '<%= GetLabelKey("admin_leftpanel_navigation").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Frame_SaveAlert = '<%= GetLabelKey("Admin_Frame_SaveAlert").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Frame_CloseWindow = '<%= GetLabelKey("Admin_Frame_CloseWindow").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_Global_Gis_IsNull = '<%= GetLabelKey("Admin_Global_Gis_IsNull").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Global_Email_To_Or_From_IsNull = '<%= GetLabelKey("Admin_Global_Email_To_Or_From_IsNull").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Global_Email_Subject_IsNull = '<%= GetLabelKey("Admin_Global_Email_Subject_IsNull").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Global_Email_Body_IsNull = '<%= GetLabelKey("Admin_Global_Email_Body_IsNull".Replace("'", "\\'"))%>';
Ext.LabelKey.Admin_Email_Body_IsLarger = '<%= GetLabelKey("Admin_Email_Body_IsLarger").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_Registration_Verification_Expired_Day_IsNull = '<%= GetLabelKey("Admin_Registration_Verification_Expired_Day_IsNull").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_Global_Setting_Label_Cache_Clear = '<%= GetLabelKey("admin_global_setting_label_cache_clear").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_Pageflow_Message_Cancel = '<%= GetLabelKey("Admin_Pageflow_Message_Cancel").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_Message_CancelPageFlow = '<%= GetLabelKey("Admin_Pageflow_Message_CancelPageFlow").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_Message_Save = '<%= GetLabelKey("Admin_Pageflow_Message_Save").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_Message_InvalidSmartchoice = '<%= GetLabelKey("Admin_Pageflow_Message_InvalidSmartchoice").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_Message_DeleteSmartchoice = '<%= GetLabelKey("Admin_Pageflow_Message_DeleteSmartchoice").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_Message_RemoveCapAssociation= '<%= GetLabelKey("Admin_Pageflow_Message_RemoveCapAssociation").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_lblCapSummary = '<%= GetLabelKey("Admin_Pageflow_lblCapSummary").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_lblSmartchoiceInstruction = '<%= GetLabelKey("Admin_Pageflow_lblSmartchoiceInstruction").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_btnCreateNew= '<%= GetLabelKey("Admin_Pageflow_btnCreateNew").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_btnModifyAssociation= '<%= GetLabelKey("Admin_Pageflow_btnModifyAssociation").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_btnModifyGroup= '<%= GetLabelKey("Admin_Pageflow_btnModifyGroup").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_btnDelete= '<%= GetLabelKey("Admin_Pageflow_btnDelete").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_btnOK= '<%= GetLabelKey("Admin_Pageflow_btnOK").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_btnCancel= '<%= GetLabelKey("Admin_Pageflow_btnCancel").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_btnAddCap= '<%= GetLabelKey("Admin_Pageflow_btnAddCap").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_btnRemoveCap= '<%= GetLabelKey("Admin_Pageflow_btnRemoveCap").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_pnlCapAvailableTitle= '<%= GetLabelKey("Admin_Pageflow_pnlCapAvailableTitle").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_pnlCapSelectedTitle= '<%= GetLabelKey("Admin_Pageflow_pnlCapSelectedTitle").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_lnkShowDetails= '<%= GetLabelKey("Admin_Pageflow_lnkShowDetails").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_lnkHideDetails= '<%= GetLabelKey("Admin_Pageflow_lnkHideDetails").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_ddlEmptyText = '<%= GetLabelKey("Admin_Pageflow_ddlEmptyText").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_txtSmartChoiceEmptyText = '<%= GetLabelKey("Admin_Pageflow_txtSmartChoiceEmptyText").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_txtSmartChoicelabel = '<%= GetLabelKey("Admin_Pageflow_txtSmartChoicelabel").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_lblOr = '<%= GetLabelKey("Admin_Pageflow_lblOr").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_Message_InvalidSmartchoiceLength = '<%= GetLabelKey("Admin_Pageflow_Message_InvalidSmartchoiceLength").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_Message_RequiredDocumentTitle = '<%= GetLabelKey("acaadmin_pageflow_required_document_msg_title_failed").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_Message_RequiredDocumentSubTitle = '<%= GetLabelKey("acaadmin_pageflow_required_document_msg_sub_title_failed").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Pageflow_Message_RequiredDocumentTypeBodys = '<%= GetLabelKey("acaadmin_pageflow_required_document_msg_body_failed").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Frame_ChangeLanguage_Confirm = '<%= GetLabelKey("Admin_Frame_ChangeLanguage_Confirm").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_Global_SelectCapTypeFilter_btnOk = '<%= GetLabelKey("Admin_Global_SelectCapTypeFilter_btnOk").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Global_SelectCapTypeFilter_btnCannel = '<%= GetLabelKey("Admin_Global_SelectCapTypeFilter_btnCannel").Replace("'", "\\'")%>' ;
Ext.LabelKey.Admin_FieldProperty_CapFilterName = '<%= GetLabelKey("Admin_FieldProperty_CapFilterName").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CapTypeFilter_lblCapTypeFilterInstruction = '<%= GetLabelKey("Admin_CapTypeFilter_lblCapTypeFilterInstruction").Replace("'", "\\'")%>' ;
Ext.LabelKey.Admin_CapTypeFilter_Message_CancelFilter = '<%= GetLabelKey("Admin_CapTypeFilter_Message_CancelFilter").Replace("'", "\\'")%>' ;
Ext.LabelKey.Admin_CapTypeFilter_lblOr = '<%= GetLabelKey("Admin_CapTypeFilter_lblOr").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CapTypeFilter_btnCreateNew = '<%= GetLabelKey("Admin_CapTypeFilter_btnCreateNew").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CapTypeFilter_lblFilterSummary = '<%= GetLabelKey("Admin_CapTypeFilter_lblFilterSummary").Replace("'", "\\'")%>' ;
Ext.LabelKey.Admin_CapTypeFilter_lnkShowDetails = '<%= GetLabelKey("Admin_CapTypeFilter_lnkShowDetails").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CapTypeFilter_lnkHideDetails = '<%= GetLabelKey("Admin_CapTypeFilter_lnkHideDetails").Replace("'", "\\'")%>' ;
Ext.LabelKey.Admin_CapTypeFilter_btnSaveUserRole = '<%= GetLabelKey("Admin_CapTypeFilter_btnSaveUserRole").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CapTypeFilter_btnCancelUserRole = '<%= GetLabelKey("Admin_CapTypeFilter_btnCancelUserRole").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CapTypeFilter_btnAddCap= '<%= GetLabelKey("Admin_CapTypeFilter_btnAddCap").Replace("'", "\\'")%>' ;
Ext.LabelKey.Admin_CapTypeFilter_btnRemoveCap= '<%= GetLabelKey("Admin_CapTypeFilter_btnRemoveCap").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CapTypeFilter_btnOK= '<%= GetLabelKey("Admin_CapTypeFilter_btnOK").Replace("'", "\\'")%>' ;
Ext.LabelKey.Admin_CapTypeFilter_btnCancel= '<%= GetLabelKey("Admin_CapTypeFilter_btnCancel").Replace("'", "\\'")%>' ;
Ext.LabelKey.Admin_CapTypeFilter_btnDelete= '<%= GetLabelKey("Admin_CapTypeFilter_btnDelete").Replace("'", "\\'")%>' ;
Ext.LabelKey.Admin_CapTypeFilter_txtFilterNameEmptyText = '<%= GetLabelKey("Admin_CapTypeFilter_txtFilterNameEmptyText").Replace("'", "\\'")%>' ;
Ext.LabelKey.Admin_CapTypeFilter_txtCapTypeFilterNamelabel = '<%= GetLabelKey("Admin_CapTypeFilter_txtCapTypeFilterNamelabel").Replace("'", "\\'")%>' ;
Ext.LabelKey.Admin_CapTypeFilter_pnlCapAvailableTitle= '<%= GetLabelKey("Admin_CapTypeFilter_pnlCapAvailableTitle").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CapTypeFilter_pnlCapSelectedTitle= '<%= GetLabelKey("Admin_CapTypeFilter_pnlCapSelectedTitle").Replace("'", "\\'")%>' ;
Ext.LabelKey.Admin_CapTypeFilter_lblCapTypeFilterTitle = '<%= GetLabelKey("Admin_CapTypeFilter_lblCapTypeFilterTitle").Replace("'", "\\'")%>' ;
Ext.LabelKey.Admin_CapTypeFilter_Message_InvalidFilterNameLength = '<%= GetLabelKey("Admin_CapTypeFilter_Message_InvalidFilterNameLength").Replace("'", "\\'")%>' ;
Ext.LabelKey.Admin_CapTypeFilter_Message_InvalidFilterName = '<%= GetLabelKey("Admin_CapTypeFilter_Message_InvalidFilterName").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CapTypeFilter_Message_SaveSuccessfully = '<%= GetLabelKey("Admin_CapTypeFilter_Message_SaveSuccessfully").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CapTypeFilter_Message_DeleteFilter = '<%= GetLabelKey("Admin_CapTypeFilter_Message_DeleteFilter").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CapTypeFilter_Message_MultipleModule = '<%= GetLabelKey("Admin_CapTypeFilter_Message_MultipleModule").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CapTypeFilter_Message_RemoveCapAssociation = '<%= GetLabelKey("Admin_CapTypeFilter_Message_RemoveCapAssociation").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CapTypeFilter_Message_DeleteBindedFilter = '<%= GetLabelKey("Admin_CapTypeFilter_Message_DeleteBindedFilter").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CapTypeFilter_ddlEmptyText = '<%= GetLabelKey("Admin_CapTypeFilter_ddlEmptyText").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_ServiceGroup_Label_title = '<%= GetLabelKey("acaadmin_servicegroup_label_title").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Label_instruction = '<%= GetLabelKey("acaadmin_servicegroup_label_instruction").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Label_ddlemptytext = '<%= GetLabelKey("acaadmin_servicegroup_label_ddlemptytext").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Label_lblor = '<%= GetLabelKey("acaadmin_servicegroup_label_lblor").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Label_btncreatenew = '<%= GetLabelKey("acaadmin_servicegroup_label_btncreatenew").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Label_btnaddservice = '<%= GetLabelKey("acaadmin_servicegroup_label_btnaddservice").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Label_btnremoveservice = '<%= GetLabelKey("acaadmin_servicegroup_label_btnremoveservice").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Label_btnok = '<%= GetLabelKey("acaadmin_servicegroup_label_btnok").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Label_btncancel = '<%= GetLabelKey("acaadmin_servicegroup_label_btncancel").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Label_btndelete = '<%= GetLabelKey("acaadmin_servicegroup_label_btndelete").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Label_txtgroupnameemptytext = '<%= GetLabelKey("acaadmin_servicegroup_label_txtgroupnameemptytext").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Label_pnlavailabletitle = '<%= GetLabelKey("acaadmin_servicegroup_label_pnlavailabletitle").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Label_pnlselectedtitle = '<%= GetLabelKey("acaadmin_servicegroup_label_pnlselectedtitle").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Label_servicegroupname = '<%= GetLabelKey("acaadmin_servicegroup_label_servicegroupname").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Label_txtgroupordertext = '<%= GetLabelKey("acaadmin_servicegroup_label_txtgroupordertext").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Msg_cancelgroup = '<%= GetLabelKey("acaadmin_servicegroup_msg_cancelgroup").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Msg_deletegroup = '<%= GetLabelKey("acaadmin_servicegroup_msg_deletegroup").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Msg_cannotempty = '<%= GetLabelKey("acaadmin_servicegroup_msg_cannotempty").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Msg_invalidgroupnamelength = '<%= GetLabelKey("acaadmin_servicegroup_msg_invalidgroupnamelength").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Msg_invalidfiltername = '<%= GetLabelKey("acaadmin_servicegroup_msg_invalidfiltername").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ServiceGroup_Msg_savesuccessfully = '<%= GetLabelKey("acaadmin_servicegroup_msg_savesuccessfully").Replace("'", "\\'")%>';

// Feature:09ACC-08040_Board_Type_Selection
Ext.LabelKey.Admin_CapTypeFilter_BoardTypeSelection_Label = '<%= GetLabelKey("admin_captypefilter_boardtypeselection_label").Replace("'", "\\'")%>';

 
Ext.LabelKey.Admin_CreateAmendment_btnOK= '<%= GetLabelKey("Admin_CreateAmendment_btnOK").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CreateAmendment_btnCancel= '<%= GetLabelKey("Admin_CreateAmendment_btnCancel").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CreateAmendment_btnAddCap= '<%= GetLabelKey("Admin_CreateAmendment_btnAddCap").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CreateAmendment_btnRemoveCap=  '<%= GetLabelKey("Admin_CreateAmendment_btnRemoveCap").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CreateAmendment_pnlCapAvailableTitle= '<%= GetLabelKey("Admin_CreateAmendment_pnlCapAvailableTitle").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CreateAmendment_pnlCapSelectedTitle= '<%= GetLabelKey("Admin_CreateAmendment_pnlCapSelectedTitle").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_CreateAmendment_Message_btnCancel = '<%= GetLabelKey("Admin_CreateAmendment_Message_btnCancel").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_SaveError_Message = '<%= GetLabelKey("ACA_Pageflow_SaveError_Message").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_SaveError_Title = '<%= GetLabelKey("ACA_Pageflow_SaveError_Title").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_UserTypeRole_SaveAlert_Message = '<%= GetLabelKey("admin_user_type_role_atleast_selected_one_message").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CapTypeRole_SaveAlert_Message = '<%= GetLabelKey("admin_cap_type_role_atleat_selected_one_message").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_FieldProperty_Choice_LookUp= '<%= GetLabelKey("Admin_FieldProperty_Choice_LookUp").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_Choice_Search= '<%= GetLabelKey("Admin_FieldProperty_Choice_Search").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_Choice_Payment= '<%= GetLabelKey("Admin_FieldProperty_Choice_Payment").Replace("'", "\\'")%>';
Ext.LabelKey.admin_fieldproperty_choice_education_lookup= '<%= GetLabelKey("admin_fieldproperty_choice_education_lookup").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_FieldProperty_ButtonUrl = "<%= GetLabelKey("admin_fieldproperty_buttonurl").Replace("'", "\\'")%>";

Ext.LabelKey.Admin_Report_Role_Available_Report= '<%= GetLabelKey("admin_report_role_available_report").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Report_Role_All_ACA_Users= '<%= GetLabelKey("admin_report_role_all_aca_users").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Report_Role_Anon_Users= '<%= GetLabelKey("admin_report_role_anon_users").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Report_Role_Registered_User= '<%= GetLabelKey("admin_report_role_registered_user").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Report_Role_Licensed_Professinal= '<%= GetLabelKey("admin_report_role_licensed_professinal").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Report_Role_Agent_User= '<%= GetLabelKey("acaadmin_reportrole_agentuser").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Report_Role_AgentClerk_User= '<%= GetLabelKey("acaadmin_reportrole_agentclerkuser").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Report_Label= '<%= GetLabelKey("aca_report_label").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_Message_Title_Warning= '<%= GetLabelKey("admin_message_title_warning").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Message_Number_Check= '<%= GetLabelKey("admin_message_number_check").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Message_Color_Check= '<%= GetLabelKey("admin_message_color_check").Replace("'", "\\'")%>';

Ext.Constant.APO_LOOKUP_TYPE = '<%=Accela.ACA.Common.BizDomainConstant.STD_CAT_APO_LOOKUP_TYPE%>'
Ext.Constant.CAP_SEARCH_TYPE = '<%=Accela.ACA.Common.BizDomainConstant.STD_CAT_CAP_SEARCH_TYPE%>'
Ext.Constant.CAP_PAYMENT_TYPE = '<%=Accela.ACA.Common.BizDomainConstant.STD_CAT_CAP_PAYMENT_TYPE%>'
Ext.Constant.EDUCATION_LOOKUP_TYPE = '<%=Accela.ACA.Common.ACAConstant.EDUCATION_LOOKUP%>'
Ext.Constant.AUTOFILL_CITY_ENABLED = '<%=Accela.ACA.Common.ACAConstant.STD_AUTOFILL_CITY_ENABLED%>'
Ext.Constant.AUTOFILL_STATE_ENABLED = '<%=Accela.ACA.Common.ACAConstant.STD_AUTOFILL_STATE_ENABLED%>'
Ext.Constant.IS_EXPANDED_SECTION = '<%=Accela.ACA.Common.XPolicyConstant.IS_EXPANDED_SECTION%>'

Ext.Constant.INSPECTION_CONTACT_RIGHT = '<%=Accela.ACA.Common.BizDomainConstant.STD_ITEM_INSPECTION_CONTACT_RIGHT%>'
Ext.Constant.INSPECTION_PERMISSION_USER_ROLES = '<%=Accela.ACA.Common.XPolicyConstant.INSPECTION_PERMISSION_USER_ROLES%>'
Ext.Constant.CAPDETAIL_SECTIONROLES = '<%=Accela.ACA.Common.BizDomainConstant.STD_ITEM_CAPDETAIL_SECTIONROLES %>'

Ext.LabelKey.admin_pageflow_message_multicontactsconflictcontact = '<%= GetLabelKey("admin_pageflow_message_multicontactsconflictcontact").Replace("'", "\\'")%>';
Ext.LabelKey.admin_pageflow_message_multilpsconflictlp = '<%= GetLabelKey("admin_pageflow_message_multilpsconflictlp").Replace("'", "\\'")%>';
Ext.LabelKey.admin_pageflow_message_information = '<%= GetLabelKey("admin_pageflow_message_information").Replace("'", "\\'")%>';
Ext.LabelKey.acaadmin_pageflow_msg_subgroupnotempty = '<%= GetLabelKey("acaadmin_pageflow_msg_subgroupnotempty").Replace("'", "\\'")%>';
Ext.LabelKey.acaadmin_pageflow_msg_duplicatesubgroup = '<%= GetLabelKey("acaadmin_pageflow_msg_duplicatesubgroup").Replace("'", "\\'")%>';

Ext.onReady(Ext.LabelKey.init, Ext.LabelKey);
Ext.onReady(Ext.Constant.init, Ext.Constant);

Ext.LabelKey.admin_dropdownlist_nullvalue_errormes = '<%= GetLabelKey("admin_dropdownlist_nullvalue_errormes").Replace("'", "\\'")%>';
Ext.LabelKey.admin_dropdownlist_existitem_errormes = '<%= GetLabelKey("admin_dropdownlist_existitem_errormes").Replace("'", "\\'")%>';
Ext.LabelKey.admin_fieldproperty_autofill = '<%= GetLabelKey("admin_fieldproperty_autofill").Replace("'", "\\'")%>';
Ext.LabelKey.admin_fieldproperty_searchrequired =  '<%= GetLabelKey("admin_fieldproperty_searchrequired").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_GlobalSearch_SaveAlert_Message = '<%= GetLabelKey("admin_global_setting_globalsearch_atleastone").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_CombineButton_pnlApplicationStatusTitle = '<%= GetLabelKey("admin_combinebutton_pnlapplicationstatus_title").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Global_Setting_Label_CombineHead = "<%= GetLabelKey("admin_global_setting_label_CombineHead").Replace("'", "\\'")%>";
Ext.LabelKey.Admin_Global_Setting_Label_CombineHead_DeleteDocument = '<%= GetLabelKey("admin_global_setting_label_combinehead_deletedocument").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_CombineButton_Message_NoApplicationStatus = "<%= GetLabelKey("admin_combinebutton_message_noapplicationstatus").Replace("'", "\\'")%>";
Ext.LabelKey.Admin_CombineButton_Message_SelectOneCapType = '<%= GetLabelKey("admin_combinebutton_message_selectonecaptype").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_Searchrole_Gridtitle_Recordtype = '<%= GetLabelKey("admin_searchrole_gridtitle_recordtype").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Searchrole_Gridtitle_Acausers = '<%= GetLabelKey("admin_searchrole_gridtitle_acausers").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Searchrole_Gridtitle_RegisteredUsers = '<%= GetLabelKey("admin_searchrole_gridtitle_registeredusers").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Searchrole_Gridtitle_RecordCreator = '<%= GetLabelKey("admin_searchrole_gridtitle_recordcreator").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Searchrole_Gridtitle_Contact = '<%= GetLabelKey("admin_searchrole_gridtitle_contact").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Searchrole_Gridtitle_Owner = '<%= GetLabelKey("admin_searchrole_gridtitle_owner").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Searchrole_Gridtitle_AgentUsers = '<%= GetLabelKey("acaadmin_searchrole_gridtitle_agentuser").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Searchrole_Gridtitle_AgentClerkUsers = '<%= GetLabelKey("acaadmin_searchrole_gridtitle_agentclerkuser").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Searchrole_Gridtitle_LicensedProfessional = '<%= GetLabelKey("admin_searchrole_gridtitle_licensedprofessional").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_SearchRole_ModuleLevelTitle = '<%= GetLabelKey("admin_searchrole_moduleleveltitle").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_SearchRole_CapTypeLevelTitle = '<%= GetLabelKey("admin_searchrole_captypeleveltitle").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_SearchRole_Message_CancelRoleChange = '<%= GetLabelKey("admin_searchrole_message_cancelrolechange").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_Width_Validate_Message = '<%= GetLabelKey("admin_width_validate_message").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Width_Nullvalue_Message = '<%= GetLabelKey("admin_width_nullvalue_message").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_RecordTypeFilter_SelectCAPTypes = '<%= GetLabelKey("admin_recordtypefilter_selectcaptypes").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_RecordTypeFilter_SpecificLicensedProfessional = '<%= GetLabelKey("admin_recordtypefilter_specificlicensedprofessional").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_Page_Message_MustBeLessThanMaxLength = '<%= GetLabelKey("ACA_AccelaTextBox_MustBeLessThanMaxLength").Replace("'", "\\'")%>';

Ext.LabelKey.ACA_Pageflow_Component_Address = '<%= GetTextByKey("aca_pageflow_component_address").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_Parcel = '<%= GetTextByKey("aca_pageflow_component_parcel").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_Owner = '<%= GetTextByKey("aca_pageflow_component_owner").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_LP = '<%= GetTextByKey("aca_pageflow_component_licenseprofessional").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_LPList = '<%= GetTextByKey("aca_pageflow_component_licenseprofessionallist").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_Applicant = '<%= GetTextByKey("aca_pageflow_component_applicant").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_Contact1 = '<%= GetTextByKey("aca_pageflow_component_contact1").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_Contact2 = '<%= GetTextByKey("aca_pageflow_component_contact2").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_Contact3 = '<%= GetTextByKey("aca_pageflow_component_contact3").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_ContactList = '<%= GetTextByKey("aca_pageflow_component_contactlist").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_AdditionalInfo = '<%= GetTextByKey("aca_pageflow_component_additionalinformation").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_ASI = '<%= GetTextByKey("aca_pageflow_component_asi").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_ASIT = '<%= GetTextByKey("aca_pageflow_component_asit").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_DetailInfo = '<%= GetTextByKey("aca_pageflow_component_detailinformation").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_Attachment = '<%= GetTextByKey("aca_pageflow_component_attachment").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_Education = '<%= GetTextByKey("aca_pageflow_component_education").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_ContinuingEducation = '<%= GetTextByKey("aca_pageflow_component_continuingeducation").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_Examination = '<%= GetTextByKey("aca_pageflow_component_examination").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_StepName = '<%= GetTextByKey("aca_pageflow_stepname").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_PageName = '<%= GetTextByKey("aca_pageflow_pagename").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_ValuationCalculator = '<%= GetTextByKey("aca_pageflow_component_valuationcalculator").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_CustomComponent = '<%= GetTextByKey("aca_pageflow_component_customizecomponent").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_Assets = '<%= GetTextByKey("aca_pageflow_component_assets").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_ConditionDocument = '<%= GetTextByKey("aca_pageflow_component_conditiondocument").Replace("'", "\\'")%>';
Ext.LabelKey.admin_pageflow_message_conflictattachment = '<%= GetLabelKey("admin_pageflow_message_conflictattachment").Replace("'", "\\'")%>';

Ext.LabelKey.ACA_Pageflow_Component_Address_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_address")%>';
Ext.LabelKey.ACA_Pageflow_Component_Parcel_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_parcel")%>';
Ext.LabelKey.ACA_Pageflow_Component_Owner_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_owner")%>';
Ext.LabelKey.ACA_Pageflow_Component_LP_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_licenseprofessional")%>';
Ext.LabelKey.ACA_Pageflow_Component_LPList_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_licenseprofessionallist")%>';
Ext.LabelKey.ACA_Pageflow_Component_Applicant_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_applicant")%>';
Ext.LabelKey.ACA_Pageflow_Component_Contact1_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_contact1")%>';
Ext.LabelKey.ACA_Pageflow_Component_Contact2_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_contact2")%>';
Ext.LabelKey.ACA_Pageflow_Component_Contact3_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_contact3")%>';
Ext.LabelKey.ACA_Pageflow_Component_ContactList_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_contactlist")%>';
Ext.LabelKey.ACA_Pageflow_Component_AdditionalInfo_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_additionalinformation")%>';
Ext.LabelKey.ACA_Pageflow_Component_ASI_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_asi")%>';
Ext.LabelKey.ACA_Pageflow_Component_ASIT_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_asit")%>';
Ext.LabelKey.ACA_Pageflow_Component_DetailInfo_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_detailinformation")%>';
Ext.LabelKey.ACA_Pageflow_Component_Attachment_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_attachment")%>';
Ext.LabelKey.ACA_Pageflow_Component_Education_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_education")%>';
Ext.LabelKey.ACA_Pageflow_Component_ContinuingEducation_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_continuingeducation")%>';
Ext.LabelKey.ACA_Pageflow_Component_Examination_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_examination")%>';
Ext.LabelKey.ACA_Pageflow_StepName_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_stepname")%>';
Ext.LabelKey.ACA_Pageflow_PageName_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_pagename")%>';
Ext.LabelKey.ACA_Pageflow_Component_ValuationCalculator_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_valuationcalculator")%>';
Ext.LabelKey.ACA_Pageflow_Component_CustomComponent_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_customizecomponent").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_Assets_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_assets").Replace("'", "\\'")%>';
Ext.LabelKey.ACA_Pageflow_Component_ConditionDocument_DefaultLanguage = '<%= GetDefaultLanguageTextByKey("aca_pageflow_component_conditiondocument").Replace("'", "\\'")%>';

Ext.LabelKey.Admin_Pageflow_Message_SwitchPageFlow = '<%= GetTextByKey("admin_pageflow_message_confirmswich").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FilterName_Cannotempty = '<%= GetTextByKey("admin_filtername_cannotempty").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_ClickToEdit_Watermark = '<%= GetLabelKey("aca_admin_clicktoedit_watermark").Replace("'", "\\'")%>';
Ext.LabelKey.admin_application_inspectionpermiassion_title = '<%= GetLabelKey("admin_application_inspectionpermiassion_title").Replace("'", "\\'")%>';
Ext.LabelKey.admin_application_inspectionpermiassion_inspectiontype = '<%= GetLabelKey("admin_application_inspectionpermiassion_inspectiontype").Replace("'", "\\'")%>';
Ext.LabelKey.admin_application_inspectionpermission_requestschedule = '<%= GetLabelKey("acaadmin_inspectionsetting_label_permission_requestschedule").Replace("'", "\\'")%>';
Ext.LabelKey.admin_application_inspectionpermiassion_reschedule = '<%= GetLabelKey("admin_application_inspectionpermiassion_reschedule").Replace("'", "\\'")%>';
Ext.LabelKey.admin_application_inspectionpermiassion_cancel = '<%= GetLabelKey("admin_application_inspectionpermiassion_cancel").Replace("'", "\\'")%>';
Ext.LabelKey.admin_application_inspectionpermiassion_message_cancel_alert = '<%= MessageUtil.FilterQuotation(GetLabelKey("admin_application_inspectionpermiassion_message_cancel_alert"),true)%>';
Ext.LabelKey.admin_application_inspectionpermiassion_message_save_unsuccess = '<%= GetLabelKey("admin_application_inspectionpermiassion_message_save_unsuccess").Replace("'", "\\'")%>';
Ext.LabelKey.admin_application_inspectionpermiassion_message_save_success = '<%= GetLabelKey("admin_application_inspectionpermiassion_message_save_success").Replace("'", "\\'")%>';
Ext.LabelKey.admin_gridview_pagesize = '<%= GetLabelKey("admin_gridview_pagesize").Replace("'", "\\'")%>';
Ext.LabelKey.acaadmin_gridview_pagesizemessage = '<%= GetLabelKey("acaadmin_gridview_pagesizemessage").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_Licensee_CollapseLine = '<%= GetLabelKey("aca_admin_licenseedetail_label_collapseline").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_SectionProperty_Editable = '<%= GetLabelKey("aca_admin_authagent_customerdetail_label_editable").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_FieldProperty_Is_Expanded_Section = '<%= GetLabelKey("aca_admin_recorddetail_label_expanded").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_UserInfo_SecurityQuestion_ConfigWarning = '<%= GetLabelKey("acaadmin_securityquestion_config_msg_minwarning").Replace("'", "\\'")%>';
Ext.LabelKey.Admin_SecurityQuestionsEdit_DuplicateValue = '<%=GetLabelKey("acaadmin_securityquestionsedit_msg_duplicatevalue")%>';
Ext.LabelKey.Admin_SecurityQuestionsEdit_EmptyDescription = '<%=GetLabelKey("acaadmin_securityquestionsedit_msg_emptydescription")%>';
Ext.LabelKey.Admin_SecurityQuestionsEdit_DuplicateDescription = '<%=GetLabelKey("acaadmin_securityquestionsedit_msg_duplicatedescription")%>';