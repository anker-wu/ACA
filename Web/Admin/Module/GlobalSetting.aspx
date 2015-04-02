<%@ Page Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Admin.GlobalSetting" Codebehind="GlobalSetting.aspx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import namespace ="Accela.ACA.Web.WebService" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Global Setting</title>
    <link rel="stylesheet" type="text/css" href="../resources/css/ext-all.css" />
    <link rel="stylesheet" type="text/css" href="../styles/main.css" />

    <script type="text/javascript" src="../../scripts/jquery.js"></script>
    <script type="text/javascript" src="../../scripts/GlobalConst.aspx"></script>
    <script type="text/javascript" src="../../scripts/global.js"></script>
    <script type="text/javascript" src="../adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../scripts/ext-all.js"></script>
    <script type="text/javascript" src="../scripts/ext-extension.js"></script>
    <script type="text/javascript" src="../TreeJSON/LabelKey.aspx"></script>
    <script type="text/javascript" src="../scripts/ReportPrivilegeGrid.js"></script>
    <script type="text/javascript" src="../scripts/ExtConst.js"></script>
    <script type="text/javascript" src="../scripts/checkBoxListDialog.js"></script>

    <script type="text/javascript">
    var moduleName;

    Ext.onReady(function () {
        peopleSearch.init();
        bindTemplateClickEvent();
    });

    function bindTemplateClickEvent() {
        $("#rdoNewTemplate,#rdoClassicTemplate").click(function () {
            ShowGisMapUrl($(this).attr("id") == "rdoNewTemplate");
        });
    }

    function ShowGisMapUrl(flag) {
        if (flag) {
            $('#divNewGisUrl').show();
            $('#divGisUrl').hide();
        } else {
            $('#divGisUrl').show();
            $('#divNewGisUrl').hide();
        }
    }
    
    //Create global setting page information array.
    function PageItems(pageId, moduleName, chkGIS, txtGIS, txtNewGis,chkDocType, chkCountryCode, scriptName, officialWebSite, chkExport, isDisplayUserInitial,
                                      chkShoppingCart, ddlTransaction, txtSelectExpiration, txtSaveExpiration, chkCheckBoxOption,
                                      chkPayFeeLink, cMSearchIsEnabled, chkSearchMyLicense, chkGlobalSearchSwitch,
                                      chkGlobalSearchCAPResultGroup, chkGlobalSearchLPResultGroup, chkGlobalSearchAPOResultGroup,
                                      chkDecimalFeeItem, chkFeinMasking, chkAccessibility, chkLicenseState, customizedCss, cbxEnableProxyUser,
                                      txtProxyUserExpiredDate, txtProxyUserExpiredRemoveDate, cbxEnableParcelGen, chkAnnouncementActivate, txtAnnouncementInterval, chkEnableAutoUpdate,
                                      chkInspectionResultEnableAutoUpdate, RefContactSearchEnabled, RefLPSearchEnabled, chkEnableAccountAttachment,
                                      chkEnableManualContactAssociation, chkAutoActiveNewAssociatedContact,
                                      chkEnableContactAddressMaintenance, chkEnableContactAddressEdit, chkEnableContactAddressDeactivate, chkEnableAccountEduExamCEInput, rdlTemplateType, chkEnableContactCrossAgency)
    {
        this.PageId = pageId;
        this.ModuleName = moduleName;

        this.chkGIS = chkGIS;
        this.txtGIS = txtGIS;
        this.txtNewGIS = txtNewGis;
        this.chkDocType = chkDocType;
          
        this.chkCountryCode=chkCountryCode; 
        
        this.ddlScriptName = scriptName;
        
        this.txtOfficialWebSite = officialWebSite;
        
        this.chkExport = chkExport;

        this.isDisplayUserInitial = isDisplayUserInitial;
        this.chkShoppingCart = chkShoppingCart;
        this.ddlTransaction = ddlTransaction;
        this.txtSelectExpiration = txtSelectExpiration;
        this.txtSaveExpiration = txtSaveExpiration;
        this.chkCheckBoxOption = chkCheckBoxOption;

        this.chkPayFeeLink = chkPayFeeLink;
        this.chkCrossModuleEnabled = cMSearchIsEnabled;
        this.chkSearchMyLicense = chkSearchMyLicense;

        this.chkGlobalSearchSwitch = chkGlobalSearchSwitch;
        this.chkGlobalSearchCAPResultGroup = chkGlobalSearchCAPResultGroup;
        this.chkGlobalSearchLPResultGroup = chkGlobalSearchLPResultGroup;
        this.chkGlobalSearchAPOResultGroup = chkGlobalSearchAPOResultGroup;
        this.chkDecimalFeeItem = chkDecimalFeeItem;
        this.chkFeinMasking = chkFeinMasking;
        this.chkAccessibility = chkAccessibility;
        this.chkLicenseState = chkLicenseState;
        this.customizedCss = customizedCss;
        this.cbxEnableProxyUser = cbxEnableProxyUser;
        this.txtProxyUserExpiredDate = txtProxyUserExpiredDate;
        this.txtProxyUserExpiredRemoveDate = txtProxyUserExpiredRemoveDate;
        this.cbxEnableParcelGen = cbxEnableParcelGen;
        this.chkAnnouncementActivate = chkAnnouncementActivate;
        this.txtAnnouncementInterval = txtAnnouncementInterval;
        this.chkEnableAutoUpdate = chkEnableAutoUpdate;
        this.chkInspectionResultEnableAutoUpdate = chkInspectionResultEnableAutoUpdate;
        this.chkEnableAccountAttachment = chkEnableAccountAttachment;
        this.chkEnableContactAddressEdit = chkEnableContactAddressEdit;
        this.chkEnableManualContactAssociation = chkEnableManualContactAssociation;
        this.chkAutoActiveNewAssociatedContact = chkAutoActiveNewAssociatedContact;
        this.chkEnableContactAddressMaintenance = chkEnableContactAddressMaintenance;
        //People search settings.
        this.RefContactSearchEnabled = RefContactSearchEnabled;
        this.RefLPSearchEnabled = RefLPSearchEnabled;
        this.chkEnableContactAddressDeactivate = chkEnableContactAddressDeactivate;
        this.chkEnableAccountEduExamCEInput = chkEnableAccountEduExamCEInput;
        this.rdlTemplateType = rdlTemplateType;
        this.chkEnableContactCrossAgency = chkEnableContactCrossAgency;
    }

    //Enable chkTimeout checkbox.
    function EnableTimeout(obj)
    {
        var chkTimeOut = document.getElementById("chkTimeOutActivate").checked;
        var txtTimeOut = document.getElementById("txtTimeOutPeriod");

        if(chkTimeOut)
        {
            txtTimeOut.disabled = false;
        }
        else
        {
            txtTimeOut.disabled = true;
        }
        UpdateDataInfo();
    }
    
    //Enable chkGis checkbox.
    function EnableGis(obj)
    {        
        UpdateDataInfo();
    }
    
     //Enable chkExport checkbox. 
   	function EnableExport(obj)
    {        
        UpdateDataInfo();
    }  
    
    //when close shopping cart, should close the sub items and set the transaction type as per Record.
    function DealTransStatus()
    {
        var ShoppingStatus = document.getElementById("chkShoppingCartActivate");
        var TransType = document.getElementById("ddlTransactionType");
        var ExpirationDay = document.getElementById("txtSelectExpirationDay");
        var SaveExpirationDay = document.getElementById("txtSaveExpirationDay");
        
        if(ShoppingStatus.checked)
        {
            TransType.disabled = false;
            ExpirationDay.disabled =false;
            SaveExpirationDay.disabled =false;
        }
        else
        {
            TransType.value = 1;
            TransType.disabled =true;
            ExpirationDay.disabled =true;
            SaveExpirationDay.disabled =true;
        }
    }
    
    function EnableShoppingCart(obj)
    {
        DealTransStatus();
        
        UpdateDataInfo();
    }
    
    //Update global data info after enabling the license state setting.
    function EnableLicenseState(obj)
    {
        UpdateDataInfo();
    }

    function EnableProxyUser() {
        DealProxyUserStatus();
        UpdateDataInfo();
    }

    function DealProxyUserStatus() {
        var cbxEnableProxyUser = document.getElementById("cbxEnableProxyUser");
        var txtProxyUserExpiredDate = document.getElementById("txtProxyUserExpiredDate");
        var txtProxyUserExpiredRemoveDate = document.getElementById("txtProxyUserExpiredRemoveDate");
        

        if (cbxEnableProxyUser.checked) {
            txtProxyUserExpiredDate.disabled = false;
            txtProxyUserExpiredRemoveDate.disabled = false;
        }
        else {
            txtProxyUserExpiredDate.disabled = true;
            txtProxyUserExpiredRemoveDate.disabled = true;
        }
    }

    function EnableAnnouncement() {
        DealAnnouncementStatus();
        UpdateDataInfo();
    }

    function DealAnnouncementStatus() {
        var chkAnnouncementActivate = document.getElementById("chkAnnouncementActivate");
        var txtAnnouncementInterval = document.getElementById("txtAnnouncementInterval");
        if (chkAnnouncementActivate.checked) {
            txtAnnouncementInterval.disabled = false;
        }
        else {
            txtAnnouncementInterval.disabled = true;
        }
    }
    
    //Enable PayFeeLink checkbox. 
   	function EnablePayFeeLink(obj)
    {        
        UpdateDataInfo();
    }
    
    //Enable range  search checkbox. 
   	function EnableRangeSearch(obj)
    {        
        UpdateDataInfo();
   	}
    
    //Update timeout and gid and email data.
    function UpdateDataInfo()
    {
        var needSave = true;
        if (arguments.length == 1) needSave = arguments[0];
        
        moduleName = parent.parent.Ext.Const.ModuleName;
        var chkGIS = document.getElementById("chkGISActivate").checked;
        var txtGIS = document.getElementById("txtGISPortletURL").value;
        var txtNewGis = document.getElementById("txtNewGISPortletURL").value;

        var chkDocType = document.getElementById("chkDocTypeActivate").checked;
            
        var chkCountryCode=document.getElementById("chkCountryCodeActivate").checked;
        
        var chkCheckBoxOption=document.getElementById("chkCapListCheckBoxOption").checked;
        
        var ddlScriptName = document.getElementById("ddlScriptName");
        var scriptName = ddlScriptName.value;
        
        var officialWebSite=document.getElementById("txtOfficialWebSite").value;
        
        var chkExport = document.getElementById("chkExportActivate").checked;

        var isDisplayUserInitial = document.getElementById("cbxUserInitialDisplay").checked;
        
        var chkShoppingCart = document.getElementById("chkShoppingCartActivate").checked;
        var ddlTransaction = document.getElementById("ddlTransactionType").value;
        var txtSelectExpiration = document.getElementById("txtSelectExpirationDay").value;
        var txtSaveExpiration = document.getElementById("txtSaveExpirationDay").value;
        var chkPayFeeLink = document.getElementById("chkPayFeeLinkActivate").checked;
        var chkDecimalFee = document.getElementById("chkDecimalFeeItem").checked;
        var cMSearchIsEnabled = document.getElementById("chkCrossModuleEnabled").checked;
        var chkSearchMyLicense = document.getElementById("chkSearchMyLicense").checked;
        var chkFeinMasking = document.getElementById("chkFeinMaskingActivate").checked;
        var chkAccessibility = document.getElementById("chkAccessibilityActivate").checked;
        var chkLicenseState = document.getElementById("chkLicenseStateActivate").checked;
        var cbxEnableProxyUser = document.getElementById("cbxEnableProxyUser").checked;
        var txtProxyUserExpiredDate = document.getElementById("txtProxyUserExpiredDate").value;
        var txtProxyUserExpiredRemoveDate = document.getElementById("txtProxyUserExpiredRemoveDate").value;
        var cbxEnableParcelGen = document.getElementById("cbxEnableParcelGen").checked;
        var chkEnableAutoUpdate = document.getElementById("chkEnableAutoUpdate").checked;
        var chkInspectionResultEnableAutoUpdate = document.getElementById("chkInspectionResultEnableAutoUpdate").checked;
        
        var chkGlobalSearchSwitch = document.getElementById("chkGlobalSearchSwitch").checked;
        var chkGlobalSearchCAPResultGroup = document.getElementById("chkGlobalSearchCAPResultGroup").checked;
        var chkGlobalSearchLPResultGroup = document.getElementById("chkGlobalSearchLPResultGroup").checked;
        var chkGlobalSearchAPOResultGroup = document.getElementById("chkGlobalSearchAPOResultGroup").checked;

        //People search settings.
        var RefContactSearchEnabled = $get('chkRefContactSearchEnabled').checked;
        var RefLPSearchEnabled = $get('chkRefLPSearchEnabled').checked;

        var chkAnnouncementActivate = document.getElementById("chkAnnouncementActivate").checked;
        var txtAnnouncementInterval = document.getElementById("txtAnnouncementInterval").value;
        var chkEnableAccountAttachment = $get('chkEnableAccountAttachment').checked;
        var chkEnableContactAddressEdit = $get('chkEnableContactAddressEdit').checked;
        var chkEnableManualContactAssociation = $get('chkEnableManualContactAssociation').checked;
        var chkAutoActiveNewAssociatedContact = $get('chkAutoActiveNewAssociatedContact').checked;

        var chkEnableContactAddressDeactivate = $get('chkEnableContactAddressDeactivate').checked;
        var chkEnableAccountEduExamCEInput = $get('chkEnableAccountEduExamCEInput').checked;
        var chkEnableContactCrossAgency = $get('chkEnableContactCrossAgency').checked;

        // Template
        var rdlTemplateType = $get('rdoNewTemplate').checked;

        if (!chkEnableManualContactAssociation) {
            $get('chkAutoActiveNewAssociatedContact').disabled = true;
        }
        else {
            $get('chkAutoActiveNewAssociatedContact').disabled = false;
        }

        var chkEnableContactAddressMaintenance = $get('chkEnableContactAddressMaintenance').checked;
        
        var pageItems = new PageItems('global', moduleName, chkGIS, txtGIS,txtNewGis, chkDocType, chkCountryCode, scriptName, officialWebSite, chkExport, isDisplayUserInitial,
                                      chkShoppingCart, ddlTransaction, txtSelectExpiration, txtSaveExpiration, chkCheckBoxOption,
                                      chkPayFeeLink, cMSearchIsEnabled, chkSearchMyLicense, chkGlobalSearchSwitch,
                                      chkGlobalSearchCAPResultGroup, chkGlobalSearchLPResultGroup, chkGlobalSearchAPOResultGroup,
                                      chkDecimalFee, chkFeinMasking, chkAccessibility, chkLicenseState, null, cbxEnableProxyUser,
                                      txtProxyUserExpiredDate, txtProxyUserExpiredRemoveDate, cbxEnableParcelGen, chkAnnouncementActivate, txtAnnouncementInterval, chkEnableAutoUpdate,
                                      chkInspectionResultEnableAutoUpdate, RefContactSearchEnabled, RefLPSearchEnabled, chkEnableAccountAttachment,
                                      chkEnableManualContactAssociation, chkAutoActiveNewAssociatedContact,
                                      chkEnableContactAddressMaintenance, chkEnableContactAddressEdit, chkEnableContactAddressDeactivate, chkEnableAccountEduExamCEInput, rdlTemplateType, chkEnableContactCrossAgency);

        var txtCssEditor = document.getElementById('txtCssEditor');
        if (txtCssEditor && txtCssEditor.changed) {
            pageItems.customizedCss = txtCssEditor.value;
        }                              

        parent.parent.pageGlobalItems.UpdatePageItem('global',pageItems);
        
        if(needSave == false) return;
        
        parent.parent.ModifyMark('global');
    }

    //Get Gis information.
    function GetGisDataInfo()
    {
        Accela.ACA.Web.WebService.AdminConfigureService.GetGisDataInfo(CallBack1);
    }
   
    //Get EMSE script name list info and get script name value from DB.
    function GetScriptNameInfo()
    {
        //Bind data source for ddlScriptName .
        Accela.ACA.Web.WebService.AdminConfigureService.GetScriptNameInfo(CallBackScriptNameInfo);
    }
   
  //Get CountryCode information
  function GetCountryCodeDataInfo()
  {
      Accela.ACA.Web.WebService.AdminConfigureService.GetCountryCodeDataInfo(CallBackForCountryCodeActivate);
  }
  
  //Get Fein Masking information
  function GetFeinMaskingDataInfo()
  {
      Accela.ACA.Web.WebService.AdminConfigureService.GetFeinMaskingDataInfo(CallBackForFeinMaskingActivate);
  }

  //Get Accessibility information
  function GetAccessibilityDataInfo() {
      Accela.ACA.Web.WebService.AdminConfigureService.GetAccessibilityDataInfo(CallBackForAccessibilityActivate);
  } 
  
   //Get CheckBox Option information
  function GetCheckBoxOptionInfo()
  {
      Accela.ACA.Web.WebService.AdminConfigureService.GetCheckBoxOptionInfo(CallBackForCheckBoxOption);
  }

    function GetShoppingCartDataInfo()
    {
        Accela.ACA.Web.WebService.AdminConfigureService.GetShoppingCartDataInfo(CallBackForShoppingCartSetting);
    }

    function GetProxyUserDataInfo() {
        Accela.ACA.Web.WebService.AdminConfigureService.GetProxyUserDataInfo(CallBackForProxyUserSetting);
    }

    function GetAnnouncementDataInfo() {
        Accela.ACA.Web.WebService.AdminConfigureService.GetAnnouncementDataInfo(CallBackForAnnouncementSetting);
    }

    function GetParcelGenealogyDataInfo() {
        Accela.ACA.Web.WebService.AdminConfigureService.GetParcelGenealogyDataInfo(CallBackForParcelGenealogySetting);
    }

    //Get Global Search data info
    function GetGlobalSearchDataInfo() 
    {
        Accela.ACA.Web.WebService.AdminConfigureService.GetGlobalSearchDataInfo(CallBackForGlobalSearchSetting);
    }
   
    //Get offical web site url info
    function GetOfficialWebSiteInfo()
    {
         Accela.ACA.Web.WebService.AdminConfigureService.GetOfficialWebSiteInfo(CallBackOfficialWebSite);
    }
     
    //Get Export information.
    function GetExportDataInfo()
    {
        Accela.ACA.Web.WebService.AdminConfigureService.GetExportDataInfo(CallBackForExportActivate);
    }

    function GetUserInitialDisplayInfo() {
        Accela.ACA.Web.WebService.AdminConfigureService.GetUserInitialDisplayInfo(CallBackUserInitialDisplay);
    }
    
    //Get pay fee link status.
    function GetPayFeeLinkStatus()
    {
        Accela.ACA.Web.WebService.AdminConfigureService.GetPayFeeLinkStatus(CallBackForPayFeeLinkActivate);
    }
    
    function GetDecimalStatusForFeeItem() 
    {
        Accela.ACA.Web.WebService.AdminConfigureService.GetDecimalFeeItemResult(GetDecimalStatusForFeeItemCallBack);
    }

    function GetSwitchValueOfCMSearch() {
        Accela.ACA.Web.WebService.AdminConfigureService.GetSwitchValueOfCMSearch(FillValueToCMSearchCtl);
    }

    function GetSearchMyLicense() {
        Accela.ACA.Web.WebService.AdminConfigureService.GetSearchMyLicense(function(result) { document.getElementById('chkSearchMyLicense').checked = result == 'Y'; });
    }
    
    //Get license state.
    function GetLicenseStateInfo()
    {
        Accela.ACA.Web.WebService.AdminConfigureService.GetLicenseStateInfo(CallBackForLicenseStateActivate);
    }
    
    function GetDecimalStatusForFeeItemCallBack(result) 
    {
        var chkDecimalFeeItem = document.getElementById("chkDecimalFeeItem");      
                
        if(result == "Y")
        {
            chkDecimalFeeItem.checked = true;
        }
        else
        {
            chkDecimalFeeItem.checked = false;
        }
    }

    //Get Examination Auto-update setting.
    function GetExaminationStatus() {
        Accela.ACA.Web.WebService.AdminConfigureService.GetExaminationDataInfo(CallBackForExaminationData);
    }

    function GetUploadInspectionResultStatus(policyName) {
        Accela.ACA.Web.WebService.AdminConfigureService.GetXPolicyValueByName(policyName, function (result) {
            var isChecked = (result != null && result.toUpperCase() == "Y");
            $("#chkInspectionResultEnableAutoUpdate").attr("checked", isChecked);
        });
    }

    //Contact cross agency
    function GetEnableContactCrossAgency(policyName) {
        Accela.ACA.Web.WebService.AdminConfigureService.GetXPolicyValueByName(policyName, function (result) {
            var isChecked = (result != null && result.toUpperCase() == "Y");
            $("#chkEnableContactCrossAgency").attr("checked", isChecked);
        });
    }

    //Get people attachment setting information.
    function GetAccountAttachmentDataInfo() {
        Accela.ACA.Web.WebService.AdminConfigureService.GetAccountAttachmentDataInfo(CallBackForAccountAttachment);
    }

    function GetEnableContactAddressEditDataInfo() {
        Accela.ACA.Web.WebService.AdminConfigureService.GetEnableContactAddressEditDataInfo(CallBack4EnableContactAddressEdit);
    }
    
    function GetEnableContactAddressDeactivateInfo(policyName) {
        Accela.ACA.Web.WebService.AdminConfigureService.GetXPolicyValueByName(policyName, function (result) {
            var isChecked = (result != null && result.toUpperCase() == "Y");
            $("#chkEnableContactAddressDeactivate").attr("checked", isChecked);
        });
    }

    //Get the setting for the switch: enable account education, examination and continuing education input.
    function GetEnableAccountEduExamCEInputInfo(policyName) {
        Accela.ACA.Web.WebService.AdminConfigureService.GetXPolicyValueByName(policyName, function (result) {
            var isChecked = (result != null && result.toUpperCase() == "Y");
            $("#chkEnableAccountEduExamCEInput").attr("checked", isChecked);
        });
    }

    // Get the setting for use new template or classic template.
    function GetEnableNewTemplate(policyName) {
        Accela.ACA.Web.WebService.AdminConfigureService.GetXPolicyValueByName(policyName, function (result) {
            if (result && result.toUpperCase() == "TRUE") {
                $("#rdoNewTemplate").attr("checked", "checked");
            } else {
                // no data responsed, or the response is not TRUE, default the classic template.
                $("#rdoClassicTemplate").attr("checked", "checked");
            }

            ShowGisMapUrl($("#rdoNewTemplate").attr("checked"));
        });
    }

    function CallBackForExaminationData(result) {
        var divExaminationEnableAutoUpdate = document.getElementById("divExaminationEnableAutoUpdate");
        divExaminationEnableAutoUpdate.innerHTML = result[0];

        var chkEnableAutoUpdate = document.getElementById("chkEnableAutoUpdate");

        if (result[1] != null && result[1].toUpperCase() == "Y") {
            chkEnableAutoUpdate.checked = true;
        }
        else {
            chkEnableAutoUpdate.checked = false;
        }
    }

    function CallBackForProxyUserSetting(result) {
        var defaultExpirationDay = "30";
        var divDelegateTitle = document.getElementById("divDelegateTitle");
        var cbxEnableProxyUser = document.getElementById("cbxEnableProxyUser");
        var txtProxyUserExpiredDate = document.getElementById("txtProxyUserExpiredDate");
        var txtProxyUserExpiredRemoveDate = document.getElementById("txtProxyUserExpiredRemoveDate");

        divDelegateTitle.innerHTML = result[0];
        
        if (result[1] != null && result[1].toUpperCase() == "Y") {
            cbxEnableProxyUser.checked = true;
        }
        else {
            cbxEnableProxyUser.checked = false;
        }
        
        if(result[2] != null && result[2] != "")
        {
            txtProxyUserExpiredDate.value = result[2];
        }
        else
        {
            txtProxyUserExpiredDate.value = defaultExpirationDay;
        }

        if (result[3] != null && result[3] != "") {
            txtProxyUserExpiredRemoveDate.value = result[3];
        }
        else {
            txtProxyUserExpiredRemoveDate.value = defaultExpirationDay;
        }      

        DealProxyUserStatus();
    }

    function CallBackForAnnouncementSetting(result) {
        var defaultAnnouncementInterval = "5";
        var chkAnnouncementActivate = document.getElementById("chkAnnouncementActivate");
        var txtAnnouncementInterval = document.getElementById("txtAnnouncementInterval");

        if (result[0] != null && result[0].toUpperCase() == "Y") { 
            chkAnnouncementActivate.checked = true;
        }
        else { 
            chkAnnouncementActivate.checked = false;
        }

        if (result[1] != null && result[1] != "") {
            txtAnnouncementInterval.value = result[1];
        }
        else {
            txtAnnouncementInterval.value = defaultAnnouncementInterval;
        }

        DealAnnouncementStatus();
    }

    function CallBackForParcelGenealogySetting(result) {
        var divParcelGenTitle = document.getElementById("divParcelGenTitle");
        var cbxEnableParcelGen = document.getElementById("cbxEnableParcelGen");

        divParcelGenTitle.innerHTML = result[0];

        if (result[1] != null && result[1].toUpperCase() == "Y") {
            cbxEnableParcelGen.checked = true;
        }
        else {
            cbxEnableParcelGen.checked = false;
        } 
    }
    
    function CallBackForShoppingCartSetting(result)
    {
        var defaultExpirationDay = "30";
        var divFeesAndCheckoutHead = document.getElementById("divFeesAndCheckoutHead");
        var chkShoppingCartActivate = document.getElementById("chkShoppingCartActivate");
        var ddlTransactionType = document.getElementById("ddlTransactionType");
        var txtSelectExpirationDay = document.getElementById("txtSelectExpirationDay");
        var txtSaveExpirationDay = document.getElementById("txtSaveExpirationDay");
        
        if(result[1] != null && result[1].toUpperCase() == "Y")
        {
                chkShoppingCartActivate.checked = true;
        }
        else
        {
                chkShoppingCartActivate.checked=false;
        }
        
        if(result[2] != null && result[2] != "")
        {
                ddlTransactionType.value=result[2];
        }
        else
        {
            ddlTransactionType.value="0";
        }
        
        if (result[3] != null && result[3] != "")
        {
            txtSelectExpirationDay.value = result[3];   
        }
        else
        {
            txtSelectExpirationDay.value = defaultExpirationDay;
        }
        
        if(result[4] != null && result[4] != "")
        {
            txtSaveExpirationDay.value = result[4];
        }
        else
        {
            txtSaveExpirationDay.value = defaultExpirationDay;
        }
            
        divFeesAndCheckoutHead.innerHTML = result[0];
        
        DealTransStatus();
    }

    function CallBackForGlobalSearchSetting(result) 
    {
        var chkGlobalSearchSwitch = document.getElementById("chkGlobalSearchSwitch");
        var chkGlobalSearchCAPResultGroup = document.getElementById("chkGlobalSearchCAPResultGroup");
        var chkGlobalSearchLPResultGroup = document.getElementById("chkGlobalSearchLPResultGroup");
        var chkGlobalSearchAPOResultGroup = document.getElementById("chkGlobalSearchAPOResultGroup");

        if (result[0] != null && result[0].toUpperCase() == "Y") 
        {
            chkGlobalSearchSwitch.checked = true;
        }
        else 
        {
            chkGlobalSearchSwitch.checked = false;
        }

        if (result[1] != null && result[1].toUpperCase() == "Y") 
        {
            chkGlobalSearchCAPResultGroup.checked = true;
        }
        else 
        {
            chkGlobalSearchCAPResultGroup.checked = false;
        }

        if (result[2] != null && result[2].toUpperCase() == "Y") 
        {
            chkGlobalSearchLPResultGroup.checked = true;
        }
        else 
        {
            chkGlobalSearchLPResultGroup.checked = false;
        }

        if (result[3] != null && result[3].toUpperCase() == "Y") 
        {
            chkGlobalSearchAPOResultGroup.checked = true;
        }
        else 
        {
            chkGlobalSearchAPOResultGroup.checked = false;
        }

        SetGlobalSearchSubGroups(true);
    }

    // Update global search switch
    function UpdateGlobalSearchSwitch() {
        SetGlobalSearchSubGroups(false);
        UpdateDataInfo();
    }

    // Update global search sub-group
    function UpdateGlobalSearchSubGroup(theCheckbox) {
        UpdateDataInfo();
    }

    // Set global search sub-groups
    function SetGlobalSearchSubGroups(isInitial) {
        var chkGlobalSearchSwitch = document.getElementById("chkGlobalSearchSwitch");
        var chkGlobalSearchCAPResultGroup = document.getElementById("chkGlobalSearchCAPResultGroup");
        var chkGlobalSearchLPResultGroup = document.getElementById("chkGlobalSearchLPResultGroup");
        var chkGlobalSearchAPOResultGroup = document.getElementById("chkGlobalSearchAPOResultGroup");

        chkGlobalSearchCAPResultGroup.disabled = !chkGlobalSearchSwitch.checked;
        chkGlobalSearchLPResultGroup.disabled = !chkGlobalSearchSwitch.checked;
        chkGlobalSearchAPOResultGroup.disabled = !chkGlobalSearchSwitch.checked;

        if (typeof (isInitial) != "undefined" && !isInitial) {
            chkGlobalSearchCAPResultGroup.checked = chkGlobalSearchSwitch.checked;
            chkGlobalSearchLPResultGroup.checked = chkGlobalSearchSwitch.checked;
            chkGlobalSearchAPOResultGroup.checked = chkGlobalSearchSwitch.checked;
        }
    }
   
    //Fill Gis information.
    function CallBack1(result) {
        var divGisHead = document.getElementById("divGisHead");
        var chkGISActivate = document.getElementById("chkGISActivate");
        var txtGISPortletURL = document.getElementById("txtGISPortletURL");
        var chkDocTypeActivate = document.getElementById("chkDocTypeActivate");
        var txtNewGISPortletURL = document.getElementById("txtNewGISPortletURL");

        if (result[0]) {
            chkGISActivate.checked = (result[3] == "A");
        } else { 
            chkGISActivate.checked = (result[1] == "A");
        }

        // classic template
        txtGISPortletURL.value = result[2];
        // new template
        txtNewGISPortletURL.value = result[4];
        // global features's header
        divGisHead.innerHTML = result[5];
        // enable document type filter
        chkDocTypeActivate.checked = (result[6] != "N");
    }
     
    //Bind data source for ddlScriptName . and select ddlScriptName. 
    function CallBackScriptNameInfo(result)
    {
         var ddlIndex = 0;
         var ddlSelectedValue = null;
         
         if(result == null)
         {
            return;
         }
         
         //Get ddl script name selected value.
         if(result.length>1 && result[1] != null)
         {
            ddlSelectedValue = result[1];
         }
         
         //Get ddlScript name list.
         if(result.length>0 && result[0] != null && result[0].length > 0)
         {
            var ddlScriptName = document.getElementById("ddlScriptName");
            
            for(var i=0 ; i<result[0].length; i++)
            {
               var tOption = document.createElement("Option");
               tOption.value=result[0][i][0];
               tOption.innerHTML =result[0][i][1];
               ddlScriptName.appendChild(tOption);
                    
               if(ddlSelectedValue == tOption.value)
               {
                  ddlIndex = i;
               }
             }
             
             ddlScriptName.style.display = "block"; 
             ddlScriptName.selectedIndex = ddlIndex; 
          }
    }

   //Fill CountryCode information
    function CallBackForCountryCodeActivate(result)
    {
        var chkCountryCodeActivate = document.getElementById("chkCountryCodeActivate");
        
        // result stores the rec_status
        if(result[0].toUpperCase()=="YES")
        {
            chkCountryCodeActivate.checked = true;
        }
        else 
        {
            chkCountryCodeActivate.checked = false;
        }
   }  
   
    //Fill Fein Masking information
    function CallBackForFeinMaskingActivate(result)
    {
        var chkFeinMaskingActivate = document.getElementById("chkFeinMaskingActivate");
        
        // result stores the rec_status
        if(result != null)
        {
            chkFeinMaskingActivate.checked = result.toUpperCase()=="Y";
        }
    }

    function CallBackForAccessibilityActivate(result)
    {
        var chkAccessibilityActivate = document.getElementById("chkAccessibilityActivate");

        // result stores the rec_status
        if (result != null)
        {
            chkAccessibilityActivate.checked = result.toUpperCase() == "Y";
        }
    }
   
    //Fill CheckBox Option information
    function CallBackForCheckBoxOption(result)
    {
        var chkCheckBoxOption = document.getElementById("chkCapListCheckBoxOption");
        
        // result stores the rec_status
        if(result.toUpperCase() == 'Y')
        {
            chkCheckBoxOption.checked = true;
        }
        else 
        {
            chkCheckBoxOption.checked = false;
        }
   }  
   
   //Fill offical web site url
   function CallBackOfficialWebSite(result)
   {
      var txtOfficialWebSite=document.getElementById("txtOfficialWebSite");
      
      txtOfficialWebSite.value=result;
   }

   function CallBackUserInitialDisplay(result) {
       var cbxUserInitialDisplay = document.getElementById("cbxUserInitialDisplay");
       if (result.toUpperCase() == 'Y') {
           cbxUserInitialDisplay.checked = true;
       }
   }
   
   //Fill Export information
   function CallBackForExportActivate(result)
   {
        var divSearchSettingsDescription = document.getElementById("divSearchSettingsDescription");
        var chkExportActivate = document.getElementById("chkExportActivate");
        
        if(result[0]=="I" || result[1].toUpperCase()=="N" || result[1]=="")
        {
            chkExportActivate.checked = false;
        }
        else
        {
            chkExportActivate.checked = true;
        }
        
        divSearchSettingsDescription.innerHTML = result[2];
   }
   
   //Fill pay fee link information
   function CallBackForPayFeeLinkActivate(result)
   {
        var chkPayFeeLinkActivate = document.getElementById("chkPayFeeLinkActivate");      
                
        if(result[0] == "A" && result[1] == true)
        {
            chkPayFeeLinkActivate.checked = false;
        }
        else
        {
            chkPayFeeLinkActivate.checked = true;
        }
        
    }

    //Fill switch value for enabled cross module search.
    function FillValueToCMSearchCtl(result) {
        var cMSearchCtl = document.getElementById("chkCrossModuleEnabled");

        if (result == "Y") {
            cMSearchCtl.checked = true;
        }
        else {
            cMSearchCtl.checked = false;
        }
    }
    
    //Fill license setting information.
    function CallBackForLicenseStateActivate(result)
    {
        var chkLicenseStateActivate = document.getElementById("chkLicenseStateActivate");
        
        if(result == "N")
        {
            chkLicenseStateActivate.checked = false;
        }
        else
        {
            chkLicenseStateActivate.checked = true;
        }
    }

    //Fill people attachment setting information.
    function CallBackForAccountAttachment(result) {
        var chkEnableAccountAttachment = document.getElementById("chkEnableAccountAttachment");

        if (result != null) {
            chkEnableAccountAttachment.checked = result.toUpperCase() == "Y";
        }
    }

    function CallBack4EnableContactAddressEdit(result) {
        var chkEnableContactAddressEdit = document.getElementById("chkEnableContactAddressEdit");

        if (result != null) {
            chkEnableContactAddressEdit.checked = result.toUpperCase() == "Y";
        }
    }

    /* --------------------------------------------People Search Scripts <beginning>-------------------------------------------- */
    var contactDialog = null;
    var lpDialog = null;

    var peopleSearch = {
        Items: [{
            type: 'Contact',
            xEntityType: '<%#XEntityPermissionConstant.REFERENCE_CONTACT_SEARCH %>',
            checkBoxID: '<%#chkRefContactSearchEnabled.ClientID %>',
            configButtonContainerID: 'divContactSearchSetting',
            dialogProperty: {
                sectionTitle: 'Contact Types',
                positionObjID: 'btnContactSearchSetting',
                itemContainerID: 'divContactSearchSettingContainer',
                itemIDPrefix: 'chkContactSearchableItem',
                saveButtonID: 'btnContactSearchSettingOK',
                items: null,
                isPositionRight: true,
                save: SavePeopleSearchSettings
            }
        }, {
            type: 'LP',
            xEntityType: '<%#XEntityPermissionConstant.REFERENCE_LICENSED_PROFESSIONAL_SEARCH %>',
            checkBoxID: '<%#chkRefLPSearchEnabled.ClientID %>',
            configButtonContainerID: 'divLPSearchSetting',
            dialogProperty: {
                sectionTitle: 'Licensed Professional Types',
                positionObjID: 'btnLPSearchSetting',
                itemContainerID: 'divLPSearchSettingContainer',
                itemIDPrefix: 'chkLPSearchableItem',
                saveButtonID: 'btnLPSearchSettingOK',
                items: null,
                isPositionRight: true,
                save: SavePeopleSearchSettings
            }
        }],

        init: function () {
            for (var i = 0; i < this.Items.length; i++) {
                var itm = this.Items[i];

                //Create config buttons.
                itm.configButton = new Ext.Button({
                    id: itm.dialogProperty.positionObjID,
                    text: "Configure",
                    handler: function () { GetPeopleSearchSettings(this); },
                    renderTo: itm.configButtonContainerID,
                    disabled: true
                });

                //Pass parameter to button click handler.
                itm.configButton.configItem = itm;

                //Init Config button status and bind event handler to checkbox/
                var checkBox = $get(itm.checkBoxID);
                checkBox.configButton = itm.configButton;
                SetPeopleSearchButtonStatus(checkBox);

                $(checkBox).bind('click', function () {
                    SetPeopleSearchButtonStatus(this);
                    UpdateDataInfo();
                });
            }
        }
    };

    //Set Config button status.
    function SetPeopleSearchButtonStatus(context) {
        if (context.checked) {
            context.configButton.enable();
        }
        else {
            context.configButton.disable();
        }
    }

    //Gets settings from server.
    function GetPeopleSearchSettings(context) {
        Accela.ACA.Web.WebService.AdminConfigureService.GetPeopleSearchSettings(
            context.configItem.type,
            CallBackPeopleSearchSettings,
            CallBackError,
            context.configItem);
    }

    //Process settings and show the config window.
    function CallBackPeopleSearchSettings(response, context) {
        var peopleTypes = eval(response);
        context.dialogProperty.items = peopleTypes;

        if (context.type == "Contact") {
            if (!contactDialog) {
                contactDialog = new CheckBoxListDialog(context);
            }

            contactDialog.Show();
        }
        else {
            if (!lpDialog) {
                lpDialog = new CheckBoxListDialog(context);
            }

            lpDialog.Show();
        }
    }

    //Save changed settings.
    function SavePeopleSearchSettings(context) {
        var xEntities = [];
        var deletePK = {
            servProvCode: '<%#ConfigManager.AgencyCode %>',
            entityType: context.xEntityType
        };

        if (context.checkBoxList == null) {
            return;
        }

        $(context.checkBoxList).each(function () {
            xEntities.push({
                servProvCode: '<%#ConfigManager.AgencyCode %>',
                entityType: context.xEntityType,
                entityId: 'N/A',
                entityId3: this.value,
                permissionValue: this.checked ? '<%#ACAConstant.COMMON_Y %>' : '<%#ACAConstant.COMMON_N %>',
                recFulNam: '<%#ACAConstant.ADMIN_CALLER_ID %>',
                recStatus: '<%#ACAConstant.VALID_STATUS %>'
            });
        });

        Accela.ACA.Web.WebService.AdminConfigureService.UpdateXEntityPermissions(
            Sys.Serialization.JavaScriptSerializer.serialize(deletePK),
            Sys.Serialization.JavaScriptSerializer.serialize(xEntities),
            function () {
                if (context.type == "Contact") {
                    contactDialog.Close();
                }
                else {
                    lpDialog.Close();
                }
            },
            CallBackError);
    }

    //Error handler.
    function CallBackError() {
        alert(Ext.LabelKey.Admin_Common_Error);
    }
    /* --------------------------------------------People Search Scripts <ending>-------------------------------------------- */

    parent.parent.removeChangedItem4CurrentPage();

    </script>
</head>
<body style="margin-left:6px;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            <Services>
                <asp:ServiceReference Path="../../WebService/AdminConfigureService.asmx" />
            </Services>
        </asp:ScriptManager>
        <span style="display:none">
            <ACA:AccelaHideLink ID="hlSkipToolBar" runat="server" AltKey="img_alt_modulemenu_skiplink" IsSkippingToAnchor="true" NextControlClientID="FirstAnchorInACAMainContent" TabIndex="-1"/>
            <a name="FirstAnchorInACAMainContent" id="FirstAnchorInACAMainContent" tabindex="-1"></a>
        </span>
        <span id="FirstAnchorInAdminMainContent" tabindex="-1"></span>
        <div class="ACA_PaddingStyle">
            <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                
                    <!--Official Website begin-->    
                    <!-- config web site when no address result in work location search page-->
                    <fieldset style="padding: 8px; width: 96%;">
                        <legend>
                            <ACA:AccelaLabel ID="lblWebSiteSettingTitle" runat="server" LabelKey="admin_golbal_setting_label_officalWebSite_title" CssClass="ACA_New_Title_Label font12px" />
                        </legend>
                        <br />
                        <div class="ACA_NewDiv_Text">
                            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                                <div id="divInstruction" class="ACA_New_Head_Label_Width_100 font11px">
                                    <ACA:AccelaLabel ID="lblInstruction" runat="server" LabelKey="admin_golbal_setting_label_officalWebSite_disclaimer" />
                                </div>
                            </div>
                        </div>
                       <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                           <ACA:AccelaTextBox ID="txtOfficialWebSite" CssClass="ACA_LLong" runat="server" LabelKey="admin_golbal_setting_label_officalWebSite_txtTitle"></ACA:AccelaTextBox>
                       </div>
                    </fieldset>
                    <!--Official Website end-->  
            
                    <!--Global Features begin-->
                    <fieldset style="padding: 8px; width: 96%;">
                        <!--Gis Setting begin-->
                        <legend>
                            <ACA:AccelaLabel ID="lblGISTitle" LabelKey="admin_global_setting_globalfeatures_title"
                                runat="server" CssClass="ACA_Label ACA_Label_FontSize"></ACA:AccelaLabel>
                        </legend>
                        <br />
                       <div class="ACA_NewDiv_Text">
                            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                                <div id="divGisHead" class="ACA_New_Head_Label_Width_100 font11px">
                                </div>
                            </div>
                        </div>
                        
                        <!--Account settings Settings  begin-->
                        <div class="fieldset_phonesetting">
                            <ACA:AccelaLabel ID="lblAccountAttachmentDescription" LabelKey="acaadmin_global_setting_label_account_settings" runat="server" CssClass="ACA_Label ACA_Label_FontSize"></ACA:AccelaLabel>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                            <ACA:AccelaCheckBox ID="chkEnableContactCrossAgency" LabelKey="acaadmin_globalsetting_label_expose_contact_from_other_agencies" runat="server" />
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                            <ACA:AccelaCheckBox ID="chkEnableManualContactAssociation" LabelKey="aca_label_enable_contact_association" runat="server" />
                            <div style="padding-left:15px">
                                <ACA:AccelaCheckBox ID="chkAutoActiveNewAssociatedContact" LabelKey="aca_label_auto_activate_association" runat="server" />
                            </div>
                            <ACA:AccelaCheckBox ID="chkEnableContactAddressMaintenance" LabelKey="aca_label_auto_enable_contactaddress_maintenance" runat="server" />
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                           <ACA:AccelaCheckBox ID="chkEnableAccountAttachment" runat="server" LabelKey="acaadmin_global_setting_label_account_attachment_active"></ACA:AccelaCheckBox>
                        </div>
                        <div class="fieldset_globalfeature_item">
                           <ACA:AccelaCheckBox ID="chkEnableAccountEduExamCEInput" runat="server" LabelKey="acaadmin_global_setting_label_enable_account_edu_exam_ce_item"></ACA:AccelaCheckBox>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8" style="height:10px"></div>
                        <!--Account settings Settings  end--> 
                        
                        <!--User Interface Settings begin-->
                        <div class="fieldset_phonesetting">
                            <ACA:AccelaLabel ID="lblUISettingsDescription" LabelKey="acaadmin_globalsetting_label_uisettingdescription" runat="server" CssClass="ACA_Label ACA_Label_FontSize"></ACA:AccelaLabel>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                            <ACA:AccelaRadioButton  runat="server" ID="rdoNewTemplate" GroupName="rdlTemplateType" CssClass="fontbold" LabelKey="acaadmin_globalsetting_label_radiobutton_newtemplate"></ACA:AccelaRadioButton>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                            <ACA:AccelaRadioButton  runat="server" ID="rdoClassicTemplate" GroupName="rdlTemplateType" CssClass="fontbold" LabelKey="acaadmin_globalsetting_label_radiobutton_classictemplate"></ACA:AccelaRadioButton>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8" style="height:10px"></div>
                        <!--User Interface Settings end-->
                                  
                        <div class="fieldset_phonesetting">
                            <ACA:AccelaLabel ID="lblGisTitleDescription" LabelKey="admin_global_setting_label_gis_title"
                                runat="server" CssClass="ACA_Label ACA_Label_FontSize"></ACA:AccelaLabel>
                        </div> 
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                            <ACA:AccelaCheckBox ID="chkGISActivate" runat="server" LabelKey="admin_global_setting_label_gis_activate">
                            </ACA:AccelaCheckBox>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8" id="divGisUrl">
                            <ACA:AccelaTextBox ID="txtGISPortletURL" CssClass="ACA_LLong" runat="server" LabelKey="admin_global_setting_label_gis_portlet_url"></ACA:AccelaTextBox>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8" id="divNewGisUrl">
                            <ACA:AccelaTextBox ID="txtNewGISPortletURL" CssClass="ACA_LLong hide" runat="server" LabelKey="admin_global_setting_label_gis_portlet_url"></ACA:AccelaTextBox>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                             <ACA:AccelaCheckBox ID="chkDocTypeActivate" runat="server" LabelKey="admin_global_setting_label_doctypefilter_activate">
                            </ACA:AccelaCheckBox>
                        </div>
                        
                        <!--Gis Setting end-->
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8" style="height:10px"></div>
                        <!--Internationalization Settings  begin-->
                        <div class="fieldset_phonesetting">
                            <ACA:AccelaLabel ID="lblPhoneSettingTitle" LabelKey="admin_global_setting_label_PhoneSetting_title"
                                runat="server" CssClass="ACA_Label ACA_Label_FontSize"></ACA:AccelaLabel>
                        </div> 
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                           <ACA:AccelaCheckBox ID="chkCountryCodeActivate" runat="server" LabelKey="admin_global_setting_label_countrycode_activate">
                           </ACA:AccelaCheckBox>
                        </div>   
                        <!--Internationalization Settings  end-->  
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8" style="height:10px"></div> 
                        <!--Fein Masking Settings  begin-->
                        <div class="fieldset_phonesetting">
                            <ACA:AccelaLabel ID="lblFeinTitle" LabelKey="admin_global_setting_label_feinmasksetting" runat="server" CssClass="ACA_Label ACA_Label_FontSize"></ACA:AccelaLabel>
                        </div> 
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                           <ACA:AccelaCheckBox ID="chkFeinMaskingActivate" runat="server" LabelKey="admin_global_setting_feinmasksettings_switch"></ACA:AccelaCheckBox>
                        </div>   
                        <!--Fein Masking Settings  end-->
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8" style="height:10px"></div> 
                        <!--Accessibility Settings  begin-->
                        <div class="fieldset_phonesetting">
                            <ACA:AccelaLabel ID="IblAccessibility" LabelKey="admin_global_accessibility" runat="server" CssClass="ACA_Label ACA_Label_FontSize"></ACA:AccelaLabel>
                        </div> 
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                           <ACA:AccelaCheckBox ID="chkAccessibilityActivate" runat="server" LabelKey="admin_global_support_accessibility"></ACA:AccelaCheckBox>
                        </div>   
                        <!--Accessibility Settings  end-->
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8" style="height:10px"></div>
                        <!--License Satet Settings  begin-->
                        <div class="fieldset_phonesetting">
                            <ACA:AccelaLabel ID="lblLicenseState" LabelKey="acaadmin_licensestate_msg_checkbox" runat="server" CssClass="ACA_Label ACA_Label_FontSize"></ACA:AccelaLabel>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                           <ACA:AccelaCheckBox ID="chkLicenseStateActivate" runat="server" LabelKey="acaadmin_licensestate_msg_activate"></ACA:AccelaCheckBox>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8" style="height:10px"></div>
                        <div class="fieldset_phonesetting">
                            <ACA:AccelaLabel ID="lblAnnouncementState" LabelKey="acaadmin_globalsetting_label_announcementsetting" runat="server" CssClass="ACA_Label ACA_Label_FontSize"></ACA:AccelaLabel>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                           <ACA:AccelaCheckBox ID="chkAnnouncementActivate" runat="server" LabelKey="acaadmin_announcement_msg_activate"></ACA:AccelaCheckBox>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                            <table cellpadding="0" cellspacing="0" border="0" role='presentation'>
                                <tr>
                                    <td><ACA:AccelaLabel ID="lblAnouncementSettingInterval" AssociatedControlID="txtAnnouncementInterval" CssClass="ACA_Label ACA_Label_FontSize" LabelKey="aca_announcement_interval_description" runat="Server"></ACA:AccelaLabel></td>
                                    <td style="padding-left:12px"><ACA:AccelaNumberText MaxLength="6" ID="txtAnnouncementInterval" IsNeedDot="false" runat="server" Width="50px" ToolTip="Please set a time interval."></ACA:AccelaNumberText></td>
                                    <td><ACA:AccelaLabel ID="AccelaLabel3"  CssClass="ACA_Label ACA_Label_FontSize" LabelKey="aca_announcement_interval_description_minutes" runat="Server"></ACA:AccelaLabel></td>
                                </tr>
                            </table>
                        </div>
                        <div class="fieldset_phonesetting">
                            <ACA:AccelaLabel ID="lblContactSetting" LabelKey="admin_globalsetting_label_contactsetting" runat="server" CssClass="ACA_Label ACA_Label_FontSize"></ACA:AccelaLabel>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                           <ACA:AccelaCheckBox ID="chkEnableContactAddressEdit" runat="server" LabelKey="acaadmin_global_setting_label_enable_contactaddress_edit"></ACA:AccelaCheckBox>
                        </div>
                         <div class="fieldset_globalfeature_item">
                           <ACA:AccelaCheckBox ID="chkEnableContactAddressDeactivate" LabelKey="acaadmin_globalsetting_label_enablecontactaddressdeactivate" runat="server" />
                         </div>
                        <!--License State Settings  end-->
                    </fieldset>
                    <!--Global Features end-->
                </ContentTemplate>
            </asp:UpdatePanel> 
        
            <!--Access and Display Settings begin-->
            <fieldset style="padding: 8px; width: 96%;">
                <legend>
                        <ACA:AccelaLabel ID="AccelaLabel1" runat="server" CssClass="ACA_New_Title_Label font12px" LabelKey="admin_global_setting_accessdisplaysettings_title" />
                </legend>
                <br />
                <div class="ACA_NewDiv_Text">
                    <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                        <div id="div3" class="ACA_New_Head_Label_Width_100 font11px">
                            <ACA:AccelaLabel ID="lblDesclaimer" runat="server" LabelKey="admin_global_setting_label_accessdisplaysettings_disclaimer"  />
                        </div>
                    </div>
                </div>
               
               <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                   <ACA:AccelaCheckBox ID="cbxUserInitialDisplay" runat="server" LabelKey="admin_global_setting_label_userinitial_display"></ACA:AccelaCheckBox>
                   <!--Display Checkbox in Record List for Anonymous Users begin-->
                   <ACA:AccelaCheckBox ID="chkCapListCheckBoxOption" runat="server" LabelKey="admin_global_setting_label_checkboxsetting_option"></ACA:AccelaCheckBox>
                   <!--Display Checkbox in Record List for Anonymous Users end-->
               </div>  
                <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8" style="height:10px"></div>
               <!--Report Display Controls begin-->
               <div>
                   <ACA:AccelaLabel ID="lblReportControlHead" LabelKey="admin_report_display_controls"
                       runat="server" CssClass="ACA_New_Head_Label_Bold_11 font11px"></ACA:AccelaLabel>
               </div>
               <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10" style="width:100%;" id="ReportGrid">
               </div>
               <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                  <table id="Table1" cellspacing="10" cellpadding="4" role="presentation">
                      <tr>
                          <td id="tdSaveReport">
                          </td>
                          <td id="tdCancelReport">
                          </td>
                      </tr>
                  </table>
               </div>
               <!--Report Display Controls end-->
            </fieldset>
            <!--Access and Display Settings end-->
       
            <!--Search Settings  begin-->
            <!--global search setting begin-->
            <fieldset style="padding: 8px; width: 96%">
                <legend>
                    <ACA:AccelaLabel ID="lblGlobalSearchFieldSetTitle" runat="server" CssClass="ACA_Label ACA_Label_FontSize" LabelKey="per_globalsearch_searchsettings"></ACA:AccelaLabel>
                </legend>
                <br />
                <div class="ACA_NewDiv_Text">
                    <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                        <div id="divSearchSettingsDescription" class="ACA_New_Head_Label_Width_100 font11px">
                        </div>
                    </div>
                </div>
                <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8" style="height:10px"></div>
                <div class="fieldset_phonesetting">
                    <ACA:AccelaLabel ID="lblGlobSearchTitle" LabelKey="per_globalsearch_label_globalsearch" runat="server" CssClass="ACA_Label ACA_Label_FontSize"></ACA:AccelaLabel>
                </div>
                <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                    <table role="presentation">
                        <tr>
                            <td>
                                <ACA:AccelaCheckBox ID="chkGlobalSearchSwitch" runat="server" LabelKey="admin_global_setting_globalsearch_switchtext"></ACA:AccelaCheckBox>
                            </td>
                        </tr>
                        <tr>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" role="presentation">
                                <tr>
                                    <td>
                                        <div class="ACA_Sub_Label ACA_Sub_Label_FontSize">
                                            <ACA:AccelaLabel ID="lblResultGroupsTitle" runat="server" CssClass="ACA_Label ACA_Label_FontSize" LabelKey="admin_global_setting_globalsearch_resultgroupstitle"></ACA:AccelaLabel>
                                        </div>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <ACA:AccelaCheckBox ID="chkGlobalSearchCAPResultGroup" runat="server"
                                            LabelKey="per_globalsearch_label_cap"></ACA:AccelaCheckBox>
                                    </td>
                                    <td>
                                        <ACA:AccelaCheckBox ID="chkGlobalSearchLPResultGroup" runat="server"
                                            LabelKey="per_globalsearch_label_lp"></ACA:AccelaCheckBox>
                                    </td>
                                    <td>
                                        <ACA:AccelaCheckBox ID="chkGlobalSearchAPOResultGroup" runat="server"
                                            LabelKey="per_globalsearch_label_apo"></ACA:AccelaCheckBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    </table>
                </div>
                <!--People Search Settings-->
                <div class="fieldset_phonesetting ACA_NewDiv_Text_TabRow_Margin_Top_3">
                    <ACA:AccelaLabel LabelKey="acaadmin_globalsetting_label_peoplesearch" CssClass="ACA_Label ACA_Label_FontSize" runat="server"></ACA:AccelaLabel>
                </div>
                <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                    <div>
                        <table role="presentation">
                            <tr>
                                <!--Contact Search Setting-->
                                <td>
                                    <ACA:AccelaCheckBox ID="chkRefContactSearchEnabled" LabelKey="acaadmin_globalsetting_label_enable_contact_search" runat="Server" />
                                </td>
                                <td>
                                    <div id="divContactSearchSetting"></div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table role="presentation">
                            <tr>
                                <!--Licensed Professional Search Setting-->
                                <td>
                                    <ACA:AccelaCheckBox ID="chkRefLPSearchEnabled" LabelKey="acaadmin_globalsetting_label_enable_lp_search" runat="Server" />
                                </td>
                                <td>
                                    <div id="divLPSearchSetting"></div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="fieldset_phonesetting">
                    <ACA:AccelaLabel ID="AccelaLabel2" LabelKey="per_globalsearch_additionalsettings" runat="server" CssClass="ACA_Label ACA_Label_FontSize"></ACA:AccelaLabel>
                </div>
                
                <div id="div7" class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                    <!--Cross Module Search Settings begin-->
                    <ACA:AccelaCheckBox ID="chkCrossModuleEnabled" LabelKey="aca_enable_cross_module_search" runat="Server" />
                    <!--Cross Module Search Settings end-->
                    
                    <!--Restrict Licensed Professional Results to only those associated with the user begin-->  
                    <ACA:AccelaCheckBox ID="chkSearchMyLicense" LabelKey="admin_global_setting_search_license_label" runat="Server" />
                    <!--Restrict Licensed Professional Results to only those associated with the user end-->  
                
                    <!--Allow Public users to export Search Results (in CSV format) begin--> 
                    <ACA:AccelaCheckBox ID="chkExportActivate" runat="server" LabelKey="admin_global_setting_label_export_activate"></ACA:AccelaCheckBox> 
                    <!--Allow Public users to export Search Results (in CSV format) end--> 
                </div>                     
            </fieldset>
            <!--global search setting end-->
            <!--Search Settings end-->
             
            <!--Fees And Checkout setting begin-->            
            <fieldset style="padding: 8px; width: 96%">
              <legend>
                  <ACA:AccelaLabel ID="lblFeesAndCheckout" runat="server" CssClass="ACA_Label ACA_Label_FontSize" LabelKey="admin_global_setting_feesandcheckout_title"></ACA:AccelaLabel>
              </legend> 
              <br />
              <div class="ACA_NewDiv_Text">
                 <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                     <div id="divFeesAndCheckoutHead" class="ACA_New_Head_Label_Width_100 font11px">
                     </div> 
                 </div>
              </div> 
              <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                  <table role="presentation"> 
                      <tr>
                        <td>
                            <ACA:AccelaCheckBox ID="chkPayFeeLinkActivate" runat="server" LabelKey="admin_global_setting_enable_payfee_activate"></ACA:AccelaCheckBox> 
                        </td>
                    </tr>  
                    <tr>
                    <td>
                            <ACA:AccelaCheckBox ID="chkDecimalFeeItem" runat="server" LabelKey="admin_global_setting_enable_decimal_for_payfee" Checked="true"></ACA:AccelaCheckBox> 
                    </td>
                    </tr>                    
                </table>
             </div> 
              <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                 <ACA:AccelaDropDownList ID="ddlScriptName" runat="server" LabelKey="admin_inspection_setting_label_EMSE_ddlTitle" SourceType="HardCode"/>
              </div>
                <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8" style="height:10px"></div>
                
              <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                 <table role="presentation"> 
                     <tr>
                       <td>
                           <div class="ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                                <ACA:AccelaLabel ID="lblCartDiscription" LabelKey="per_globalsearch_cart_discription" runat="server" CssClass="ACA_Label ACA_Label_FontSize"></ACA:AccelaLabel>
                            </div>
                           <div class="ACA_Row">
                             <ACA:AccelaCheckBox ID="chkShoppingCartActivate" runat="server" LabelKey="admin_global_setting_shoppingcart_activate"></ACA:AccelaCheckBox> 
                           </div>
                       </td>
                     </tr>
                     <tr>
                        <td>
                            <div style="color:#666666;font-size:13px;font-weight:bold;font-family:Arial, sans-serif; margin-right:-33px;">
                                <ACA:AccelaLabel ID="lblTransactionTitle" LabelKey="admin_global_setting_transaction_title" runat="Server"></ACA:AccelaLabel>
                            </div>    
                        </td>
                        <td>
                            <div style="margin-top:-10px;position:absolute;z-index:3">
                                <ACA:AccelaDropDownList ID="ddlTransactionType" runat="server" ToolTip="Please select a transaction type." ></ACA:AccelaDropDownList>
                            </div>
                        </td>
                     </tr>
                     <tr>
                        <td>
                            <div style="color:#666666;font-size:13px;font-weight:bold;font-family:Arial, sans-serif;">
                                <ACA:AccelaLabel ID="lblSelectExpirationDayTitle" LabelKey="admin_global_setting_shoppingcart_expiration_title" runat="Server"></ACA:AccelaLabel>
                            </div>
                        </td>
                        <td>
                            <div style="margin-top:-8px;float:left;position:absolute;z-index:2">
                                <ACA:AccelaNumberText ID="txtSelectExpirationDay" MaxLength="6" IsNeedDot="false" runat="server" ToolTip="Please set expiration days for Selected Items." ></ACA:AccelaNumberText>
                            </div>    
                        </td>
                     </tr>
                     <tr>
                        <td style="margin-right:-33px;">
                           <div style="color:#666666;font-size:13px;font-weight:bold;font-family:Arial, sans-serif;">
                                <ACA:AccelaLabel ID="lblSaveExpirationDayTitle" LabelKey="admin_global_setting_wishexpiration_title" runat="Server"></ACA:AccelaLabel>
                            </div>
                        </td>
                        <td>
                            <div style="float:left; z-index:1">
                                <ACA:AccelaNumberText ID="txtSaveExpirationDay" MaxLength="6" IsNeedDot="false" runat="server" ToolTip="Please set expiration days for Saved Items."></ACA:AccelaNumberText>
                            </div>
                        </td>
                     </tr>                      
                 </table>
              </div> 
            </fieldset>           
            <!--Fees And Checkout setting end-->
                         
            <!--Proxy user settings begin-->            
            <fieldset style="padding: 8px; width: 96%">
              <legend>
                  <ACA:AccelaLabel ID="lblDelegate" runat="server" CssClass="ACA_Label ACA_Label_FontSize" LabelKey="aca_delegates_settings_header"></ACA:AccelaLabel>
              </legend> 
              <br />
              <div class="ACA_NewDiv_Text">
                 <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                     <div id="divDelegateTitle" class="ACA_New_Head_Label_Width_100 font11px"></div> 
                 </div>
              </div> 
              <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                  <table role="presentation"> 
                      <tr>
                        <td colspan="2">
                            <ACA:AccelaCheckBox ID="cbxEnableProxyUser" runat="server" LabelKey="aca_enable_delegates_checkbox"></ACA:AccelaCheckBox> 
                        </td>
                    </tr>
                     <tr>                        
                        <td style="margin-right:-33px;">
                           <div style="color:#666666;font-size:13px;font-weight:bold;font-family:Arial, sans-serif;">
                                <ACA:AccelaLabel ID="lblProxyUserExpiredDateTitle" LabelKey="aca_expired_day_description" runat="Server"></ACA:AccelaLabel>
                            </div>
                        </td>
                        <td>
                            <div style="float:left; z-index:1">
                                <ACA:AccelaNumberText ID="txtProxyUserExpiredDate" MaxLength="6" IsNeedDot="false" runat="server" ToolTip="Please set expiration days for deletate user."></ACA:AccelaNumberText>
                            </div>
                        </td>
                     </tr>   
                     <tr>                        
                        <td style="margin-right:-33px;">
                           <div style="color:#666666;font-size:13px;font-weight:bold;font-family:Arial, sans-serif;">
                                <ACA:AccelaLabel ID="lblProxyUserExpiredRemoveDateTitle" LabelKey="aca_purge_day_description" runat="Server"></ACA:AccelaLabel>
                            </div>
                        </td>
                        <td>
                            <div style="float:left; z-index:1">
                                <ACA:AccelaNumberText ID="txtProxyUserExpiredRemoveDate" MaxLength="6" IsNeedDot="false" runat="server" ToolTip="Please set expiration days for deletate user."></ACA:AccelaNumberText>
                            </div>
                        </td>
                     </tr>               
                </table>
             </div> 
            </fieldset>           
            <!--Proxy user settings end-->
                         
            <!--Parcel Genealogy Settings begin-->            
            <fieldset style="padding: 8px; width: 96%">
              <legend>
                  <ACA:AccelaLabel ID="lblParcelGenHeader" runat="server" CssClass="ACA_Label ACA_Label_FontSize" LabelKey="genealogy_settings_header"></ACA:AccelaLabel>
              </legend> 
              <br />
              <div class="ACA_NewDiv_Text">
                 <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                     <div id="divParcelGenTitle" class="ACA_New_Head_Label_Width_100 font11px"></div> 
                 </div>
              </div> 
              <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                  <table role="presentation"> 
                      <tr>
                        <td colspan="2">
                            <ACA:AccelaCheckBox ID="cbxEnableParcelGen" runat="server" LabelKey="aca_enable_parcelgen_checkbox"></ACA:AccelaCheckBox> 
                        </td>
                    </tr>             
                </table>
             </div> 
            </fieldset>           
            <!--Parcel Genealogy Settings end-->

            <!--Examintions Settings begin-->            
            <fieldset style="padding: 8px; width: 96%">
              <legend>
                  <ACA:AccelaLabel ID="lblExaminationTitle" runat="server" CssClass="ACA_Label ACA_Label_FontSize" LabelKey="aca_exam_setting_title"></ACA:AccelaLabel>
              </legend> 
              <br />
              <div class="ACA_NewDiv_Text">
                 <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                     <div id="divExaminationEnableAutoUpdate" class="ACA_New_Head_Label_Width_100 font11px"></div> 
                 </div>
              </div> 
              <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                  <table role="presentation"> 
                      <tr>
                        <td colspan="2">
                            <ACA:AccelaCheckBox ID="chkEnableAutoUpdate" runat="server" LabelKey="aca_exam_setting_autoupdate_label"></ACA:AccelaCheckBox> 
                        </td>
                    </tr>
                </table>
             </div> 
            </fieldset>           
            <!--Examintions Settings end-->

            <!--Upload Inspection Result Begin-->
            <fieldset class="aca_admin_fieldset">
              <legend>
                  <ACA:AccelaLabel ID="lblInspectionResultSettingTitle" runat="server" CssClass="ACA_Label ACA_Label_FontSize" LabelKey="acaadmin_globalsetting_label_inspection_autoupdate_title"></ACA:AccelaLabel>
              </legend>
              <br />
              <div class="header_outer">
                 <div class="header_inner">
                     <div class="header_innertext"><%=LabelUtil.GetAdminUITextByKey("acaadmin_globalsetting_label_inspection_autoupdate_msg")%></div> 
                 </div>
              </div> 
              <div class="body">
                  <table role="presentation"> 
                      <tr>
                        <td colspan="2">
                            <ACA:AccelaCheckBox ID="chkInspectionResultEnableAutoUpdate" runat="server" onclick="UpdateDataInfo();" LabelKey="acaadmin_globalsetting_label_inspection_autoupdate_label"></ACA:AccelaCheckBox> 
                        </td>
                    </tr>
                </table>
             </div>
            </fieldset>
            <!--Upload Inspection Result End-->

             <!--CSS customizaion begin-->
            <fieldset style="padding: 8px; width: 96%;">
                <legend>
                        <ACA:AccelaLabel ID="lblCssTitle" runat="server" CssClass="ACA_New_Title_Label font12px" LabelKey="admin_global_setting_customizedcss_title" />
                </legend>
                <br />
                <div class="ACA_NewDiv_Text">
                    <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                        <div class="ACA_New_Head_Label_Width_100 font11px">
                            <ACA:AccelaLabel ID="lblCssEditorInstruction" runat="server" LabelKey="admin_global_setting_customizedcss_disclaimer" />
                        </div>
                    </div>
                </div>
               <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                 <ACA:AccelaLabel ID="lblCssEidtorTitle" runat="server" CssClass="ACA_New_Title_Label font12px ACA_Show" LabelKey="admin_global_setting_customizedcss_editor_label" Text="CSS Content:" />
                 <asp:Label runat="server" ID="lblCssEditor" AssociatedControlID="txtCssEditor"></asp:Label>
                 <textarea runat="server" id="txtCssEditor" class="aca_css_editor" onchange="this.changed = true;UpdateDataInfo();"></textarea>
               </div> 
            </fieldset>
            <!--CSS customization end-->
            
            <div>
                <asp:HiddenField ID="hdfFlag" runat="server" />
            </div>
        </div>
    </form>
    <noscript><%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</body>
</html>