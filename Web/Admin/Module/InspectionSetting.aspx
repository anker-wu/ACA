<%@ Page Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Admin.InspectionSetting"
    CodeBehind="InspectionSetting.aspx.cs" %>

<%@ Import Namespace="Accela.ACA.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inspection Setting</title>
    <link rel="stylesheet" type="text/css" href="../resources/css/ext-all.css" />
    <link rel="stylesheet" type="text/css" href="../styles/main.css" />
    <script type="text/javascript" src="../../Scripts/jquery.js"></script>
    <script type="text/javascript" src="../adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../scripts/ext-all.js"></script>
    <script type="text/javascript" src="../scripts/ext-extension.js"></script>
    <script type="text/javascript" src="../scripts/CombinButtonUtil.js"></script>
    <script type="text/javascript" src="../TreeJSON/LabelKey.aspx"></script>
    <script type="text/javascript" src="../scripts/WorkflowLib.js"></script>
    <script type="text/javascript" src="../scripts/CapTypeFilter.js"></script>
    <script type="text/javascript" src="../scripts/checkBoxListDialog.js"></script>
    <script type="text/javascript" src="../scripts/AmendmentButton.js"></script>
    <script type="text/javascript" src="../scripts/ContactTypePrivilegegrid.js"></script>
    <script type="text/javascript" src="../scripts/ReportPrivilegeGrid.js"></script>
    <script type="text/javascript" src="../scripts/InspectionActionPermissionGrid.js"></script>
    <script type="text/javascript" src="../scripts/CapTypeSearchRoleGrid.js"></script>
    <script type="text/javascript" src="../scripts/LicenseVerificationGrid.js"></script>
    <script type="text/javascript" src="../scripts/CapDetailSectionRoleGrid.js"></script>
    <script type="text/javascript" src="../scripts/ExtConst.js"></script>
    <script type="text/javascript" src="../../scripts/GlobalConst.aspx"></script>
    <script type="text/javascript" src="../../Scripts/global.js"></script>
    <style type="text/css">
        .x-tree-nodes-setting
        {
            display: none;
        }
    </style>
    <script type="text/javascript">

    Ext.BLANK_IMAGE_URL="../resources/images/default/s.gif";
    
    var moduleName;
    var getUrl;
    var tree;   
    var hasSelectChild;
    var mappingUserRole2ChkContact = new Array(0, 2, 5, 3, 4, 1);
    var mappingUserRole2ScheduleInspection = new Array(0, 2, 5, 3, 4, 1);
    
    Ext.onReady(function(){   
        // shorthand   
        var Tree = Ext.tree;   
        moduleName = parent.parent.Ext.Const.ModuleName;
        getUrl = "../TreeJSON/TreeAppStatus.aspx?ModuleName=" + moduleName; 

		// set the root node   
        var root = new Tree.AsyncTreeNode({   
            text: 'root',   
            id:'root'
        });   

        tree = new Tree.TreePanel({
            width: 400,
            autoScroll:true,
            animate:true,   
            enableDD:false,   
            containerScroll: true,
		    root : root,
		    lines:true,
		    rootVisible: false,
		    iconCls:'nav',
            loader: new Tree.TreeLoader({
                dataUrl:[getUrl],
                baseAttrs: {checked: false}
            }),
	        border : false
        });

        tree.on('expandnode', function(node){
            // change the checkbox's title that not contain the html code
            if (node.childNodes && node.childNodes.length > 0) {
                for (var i = 0; i < node.childNodes.length; i++) {
                    var title = node.childNodes[i].attributes['defaultText'];
                    node.childNodes[i].ui.checkbox.title = title;
                }
            }
        });

        tree.on('checkchange', function(node, checked) {
            node.expand();
            //node.ui.removeClass('icondoc');
            node.attributes.checked = checked;
            
            var splitChar = Ext.Const.SplitChar;

            if(node.attributes.hiberarchy==1)
            {
                if(node.parentNode.id != "root")
                { 
                    hasSelectChild = false;
                    var cs = node.parentNode.childNodes;
                    for(var i = 0;i < cs.length; i++) 
                    {
                        if(cs[i].attributes.checked === true)
                        {
                            hasSelectChild = true;
                            break;
                        }
                    }

                    if(hasSelectChild)
                    {
                        node.parentNode.ui.toggleCheck(true);   
                        node.parentNode.attributes.checked = true;
                    }
                    else
                    {
                        node.parentNode.ui.toggleCheck(false);   
                        node.parentNode.attributes.checked = false;
                    }
                }

                ChangeItem(node.attributes.checked + splitChar + node.attributes.groupname + splitChar + node.attributes.defaultText + splitChar + node.attributes.displayRequestTradeLicense); 
            }
            else if(node.attributes.hiberarchy==2)
            {
                if(node.attributes.displayRequestTradeLicense.toUpperCase()=="Y")
                {
                    node.attributes.displayRequestTradeLicense = "N";
                    node.parentNode.attributes.displayRequestTradeLicense = "N";
                }
                else
                {
                    node.attributes.displayRequestTradeLicense = "Y";
                    node.parentNode.attributes.displayRequestTradeLicense = "Y";
                            
                    node.parentNode.ui.toggleCheck(true);   
                    node.parentNode.attributes.checked = true;

                    node.parentNode.parentNode.ui.toggleCheck(true);   
                    node.parentNode.parentNode.attributes.checked = true;
                }

                ChangeItem(node.parentNode.attributes.checked + splitChar + node.parentNode.attributes.groupname + splitChar + node.parentNode.attributes.defaultText + splitChar + node.parentNode.attributes.displayRequestTradeLicense);
            }

            node.eachChild(function(child) 
            {   
                if(child.attributes.checked != checked && child.attributes.hiberarchy!=2)
                {
                    child.ui.toggleCheck(checked);   
                    child.attributes.checked = checked;   
                    child.fireEvent('checkchange', child, checked);                    
                }
                else if(child.attributes.checked==true && child.attributes.hiberarchy==2)
                {
                    child.ui.toggleCheck(false);   
                    child.attributes.checked = false;   
                    child.fireEvent('checkchange', child, false); 
                } 
            });
        }, tree); 

        tree.on('dblclick', function(node, e) {
        }, tree);
                
        tree.on('click', function(node, e) {
            var isCancelled = false;

            if (e!= null && e.target != null && e.target.parentElement != null && e.target.parentElement.href != null){
                var params = e.target.parentElement.href.replace('javascript:', '').split('\f');
                if (params !=null && params.length >= 2){                        
                    isCancelled = (GetInsActionPermissionSettings(params[0], params[1]) == false);                           
                }
            }

            // if cancelled, return false that stay on the original node.
            return !isCancelled;
        }, tree);

        tree.setRootNode(root);   

        // render the tree   
        root.expand();   
        //tree.expandAll();
        
        var divTree = document.getElementById("divTree");
        //var divTreeParent = document.getElementById("divTreeParent");

        tree.render(divTree);
        //divTreeParent.insertAdjacentElement('beforeEnd',divTree);
          
        InitAmendmentCapAssociation();
        Ext.get('divAmendment').dom.style.display='none'; 
    });
    
    var appStatusGroupCode = null;
    var appStatus = null;
    var inspectionGroup = null;
    
    //Display Inspection Action permission settings
    function GetInsActionPermissionSettings(obj1, obj2){
        var inspectionGroupSelectValue = $get("ddlInsGroup").value;
        
        if (obj1 != null && obj2 != null && inspectionGroupSelectValue != null)
        {
            if (typeof(updateRecords) !='undefined' && updateRecords !=null)
            {
                if (!confirmMsg(Ext.LabelKey.admin_application_inspectionpermiassion_message_cancel_alert))
                {
                    $get("ddlInsGroup").value = inspectionGroup;
                    return false;
                }
            }
            
            parent.showMessageBox('Loading');
            appStatusGroupCode = obj1;
            appStatus = obj2;
            inspectionGroup = inspectionGroupSelectValue;
            Accela.ACA.Web.WebService.AdminConfigureService.GetInsActionPermissionSettings(appStatusGroupCode, appStatus, inspectionGroupSelectValue, CallBackForGetInsActionPermissionSettings);
            
            return true;
        }
    }
    
    //Call back for inspection action permissions.
    function CallBackForGetInsActionPermissionSettings(result){
        if (Ext.MessageBox.isVisible()){
            Ext.MessageBox.hide();
        }
        
        if (result != null){
            var divInspectionActionSetting = document.getElementById('divInspectionActionSetting');
            if (divInspectionActionSetting != null){
                 divInspectionActionSetting.className = '';
            }
            
            insActionPermissionDataSource = result;
            
            if (typeof(RefreshInsActionDataStore) != 'undefined'){
                RefreshInsActionDataStore();
            }
        }
    }
    
    //change inspection group dropdown list.
    function UpdateInsGroupDataInfo(){
         //Only selected application status. it need display inspection action permission setting.
         if (appStatusGroupCode != null && appStatus != null){
            GetInsActionPermissionSettings(appStatusGroupCode, appStatus);
         }
    }
    
    //Create application status information array.
    function PageStatusItems(pageId, moduleName,id, checked, groupname, itemname, displayRequestTradeLicense)
    {
        this.PageId = pageId;
        this.ModuleName = moduleName;
        this.NodeId = id;
        this.Checked = checked;
        this.GroupName = groupname;
        this.ItemName = itemname;
        this.DisplayRequestTradeLicense = displayRequestTradeLicense;
    }
            
    //Change element to packge array.
    function ChangeItem(value)
    {
        moduleName = parent.parent.Ext.Const.ModuleName;
        var item = value.split(Ext.Const.SplitChar);
        
        //item[0]: checked or not; item[1]: status group name; item[2]: status name; item[3]: displayRequestTradeLicense.
        if(item[1] != "")
        {
            var nodeId = item[1] + Ext.Const.SplitChar + item[2];
            var pageStatusItems = new PageStatusItems('appstatus',moduleName,nodeId,item[0],item[1],item[2], item[3]);
            parent.parent.pageAppStatusItems.UpdatePageItem('appstatus',pageStatusItems);
            parent.parent.ModifyMark('module');
        }
    }
    
    //Create page information array.
    function PageUserTypeItems(
    pageId,
    moduleName,
    displayMap4ShowObject,
    displayMap4SelectObject,
    userType,
    userTypeInputContact,
    userTypeViewContact,
    displayOption,
    displayEmail,
    isMultipleEnabled,
    displayDefaultContact4Inspection,
    checkedModules,
    displayPayFeeLink,
    EnableCloneRecord,
    enabelSearchASIAdditionCriteria,
    enabelSearchContactTemplateAdditionCriteria,
    enableBoardTypeSelection, /* Feature:09ACC-08040_Board_Type_Selection */
    socialMediaShareButtonPermission,
    sharedComments
    ) {
        this.PageId = pageId;
        this.ModuleName = moduleName;
        this.DisplayMap4ShowObject = displayMap4ShowObject;
        this.DisplayMap4SelectObject = displayMap4SelectObject;
        this.UserType = userType;
        this.UserTypeInputContact = userTypeInputContact;
        this.UserTypeViewContact = userTypeViewContact;
        this.DisplayOption = displayOption;
        this.DisplayEmail = displayEmail;
        this.IsMultipleEnabled = isMultipleEnabled;
        this.DisplayDefaultContact4Inspection = displayDefaultContact4Inspection;
        this.chkSearchCrossModule = checkedModules;
        this.DisplayPayFeeLink = displayPayFeeLink;
        this.EnableCloneRecord = EnableCloneRecord;
        this.EnabelSearchASIAdditionCriteria = enabelSearchASIAdditionCriteria;
        this.EnabelSearchContactTemplateAdditionCriteria = enabelSearchContactTemplateAdditionCriteria;
        this.EnableBoardTypeSelection = enableBoardTypeSelection;
        this.SocialMediaShareButtonPermission=socialMediaShareButtonPermission;
        this.SharedComments = sharedComments;
    }
    
    //Update inspection setting data.
    function UpdateDataInfo(e) {
        moduleName = parent.parent.Ext.Const.ModuleName;
        var chkDisplayMap4ShowObject = document.getElementById("chkDisplayMap4ShowObject");
        var displayMap4ShowObject = chkDisplayMap4ShowObject.checked;
        
        var chkDisplayMap4SelectObject = document.getElementById("chkDisplayMap4SelectObject");
        var displayMap4SelectObject = chkDisplayMap4SelectObject.checked;

        //get user type role code.
        var userType = GetRoleCode("cblUserType");
        var userTypeInputContact = GetRoleCode("cblInputContactUserType");
        var userTypeViewContact = GetRoleCode("cblViewContactUserType");

        var rdoDisplayOptionYes = document.getElementById("rdoDisplayOptionYes");
        var displayOption;

        //option for display email address.
        var chkDisplayEmail = document.getElementById("chkDisplayEmail");
        var displayEmail = chkDisplayEmail.checked;
        
        var displayPayFeeLink = document.getElementById("chkDisplayPayFeeLink").checked;
        var EnableCloneRecord = document.getElementById("chkCloneRecord").checked;
        var enabelSearchASIAdditionCriteria = document.getElementById("chkEnableSearchASI").checked;
        var enabelSearchContactTemplateAdditionCriteria = document.getElementById("chkEnableSearchContactTemplate").checked;

        var rdoAllowMultipleYes = document.getElementById("rdoAllowMultipleYes");

        // Feature:09ACC-08040_Board_Type_Selection
        var chxEnableBoardTypeSelection = document.getElementById("chxBoardTypeSelectionSwitch");
        var enabledBoardTypeSelection = false;
        var socialMediaShareButtonPermission;
        
        var isMultipleEnabled;

        var rdoDisplayDefaultContact4InspectionNo = document.getElementById("rdoDisplayDefaultContact4InspectionNo");
        var displayDefaultContact4Inspection;
        
        if (rdoDisplayDefaultContact4InspectionNo.checked) {
            displayDefaultContact4Inspection = "No";
        } else {
            displayDefaultContact4Inspection = "Yes";  
        }

        if (rdoDisplayOptionYes.checked == true) {
            displayOption = "Yes";
        }
        else {
            displayOption = "No";
        }

        if (rdoAllowMultipleYes.checked) {
            isMultipleEnabled = "Yes";
        }
        else {
            isMultipleEnabled = "No";
        }
        
        // Feature:09ACC-08040_Board_Type_Selection
        if (chxEnableBoardTypeSelection.checked) {
            enabledBoardTypeSelection = "Yes";
        }
        else {
            enabledBoardTypeSelection = "No";
        }

        var rdoShareButtonWithAll = document.getElementById("rdoShareButtonWithAll");
        var rdoShareButtonWithOwner = document.getElementById("rdoShareButtonWithOwner");
        var rdoShareButtonWithNone = document.getElementById("rdoShareButtonWithNone");
        if(rdoShareButtonWithAll.checked){
            socialMediaShareButtonPermission="All";
        }
        else if(rdoShareButtonWithOwner.checked){
            socialMediaShareButtonPermission="Creator";
        }
        else if(rdoShareButtonWithNone.checked){
            socialMediaShareButtonPermission="None";
        }
        
        var checkedModules = GetCrossModels();
        var sharedComments = document.getElementById("txtSharedComments").value;
        var pageUserTypeItems = new PageUserTypeItems(
        'inspection',
        moduleName,
        displayMap4ShowObject,
        displayMap4SelectObject,
        userType,
        userTypeInputContact,
        userTypeViewContact,
        displayOption,
        displayEmail,
        isMultipleEnabled,
        displayDefaultContact4Inspection,
        checkedModules,
        displayPayFeeLink,
        EnableCloneRecord,
        enabelSearchASIAdditionCriteria,
        enabelSearchContactTemplateAdditionCriteria,
        enabledBoardTypeSelection,
        socialMediaShareButtonPermission,
        sharedComments
        );
        
        parent.parent.pageInspectionItems.UpdatePageItem('inspection', pageUserTypeItems);
        parent.parent.ModifyMark('module');
    } 
     
    //Display checkboxlist selected status.
    function DisplayCblStatus(cblID)
    {
        if (document.getElementById(cblID + "_" + 0).checked)
        {
            ShowOrHiddenChkStatus(cblID,true);
        }
        else
        {
           ShowOrHiddenChkStatus(cblID,false); 
        }  
        
        // set the specific LP disabled
        if (cblID == 'cblInputContactUserType') {
            btnInputContactUserTypeConfig.disable();
        }
        
        if (cblID == 'cblViewContactUserType') {
            btnViewContactUserTypeConfig.disable();
        }

        if (cblID == 'cblUserType') {
            btnScheduleInspectionUserTypeConfig.disable();
        }
    }
    
    function ShowOrHiddenChkStatus(cblID,flag)
    { 
        for (var i=1; i<document.getElementById(cblID).getElementsByTagName("input").length; i++)
        {  
            document.getElementById(cblID + "_" + i).disabled = flag;
            document.getElementById(cblID + "_" + i).checked =flag;
        } 
    }
  
     
    //Get role code.
    function GetRoleCode(cblID)
    { 
        var cblValue = "";  
       
        for (var i=0;i<document.getElementById(cblID).getElementsByTagName("input").length;i++)
        {  
            if (document.getElementById(cblID + "_" + i).checked)
            {
                cblValue +=  "1";
            }
            else
            {
                cblValue +=  "0";
            }
        }         
        
        switch (cblID) {
            case "cblInputContactUserType":
            case "cblViewContactUserType":
                cblValue = GetMappingRoleValue(cblValue, mappingUserRole2ChkContact);
                break;
            case "cblUserType":            
                cblValue = GetMappingRoleValue(cblValue, mappingUserRole2ScheduleInspection);
                break;
            default:
            break;
        }
        
        return cblValue;
    }

    function GetMappingRoleValue(cblValue, mappingRule){
        var convertValue = '';
        for (var i = 0; i < mappingRule.length; i++){
            convertValue += cblValue.substr(mappingRule[i], 1);
        }

        return convertValue;
    }

    function GetCrossModels() {
        var ctl = document.getElementById("chkCMSearchModules");

        if (ctl == null) {
            return;
        }

        var chkSearchModels = ctl.getElementsByTagName("input");              
        var checkedValue = "";

        for (var i = 0; i < chkSearchModels.length; i++) {
            var chkModel = chkSearchModels[i];
            var mainLanguageModule = ctl.childNodes[i].getAttribute("mainLanguageValue");
            
            if (typeof (chkModel.nextSibling) != "undefined") {
                var chkValue = chkModel.checked ? mainLanguageModule + '\b' + 'Y' : mainLanguageModule + '\b' + 'N';
                checkedValue = checkedValue == "" ? chkValue : checkedValue + "\f" + chkValue;
            }
        }

        return checkedValue;
    }

    function SetCrossModels(value) {
        var ctl = document.getElementById("chkCMSearchModules");

        if (value == null || ctl == null) {
            return;
        }

        chkSearchModels = ctl.getElementsByTagName("input");
        var selectedModules = value.split('\f');

        for (var j = 0; j < selectedModules.length; j++) {

            for (var i = 0; i < chkSearchModels.length; i++) {
                var chkModel = chkSearchModels[i];
                var kayValue = selectedModules[j].split('\b');
                var mainLanguageModule = ctl.childNodes[i].getAttribute("mainLanguageValue");

                if (typeof (chkModel.parentNode) != "undefined" && mainLanguageModule.toLowerCase() == kayValue[0].toLowerCase()) {
                    var isSelected = kayValue[1] == 'Y';
                    chkModel.checked = isSelected;
                }
            }
        }
    }
    
    //Set the role code to checkboxlist.
    function SetRoleCode(cblID,value)
    {  
        if(value=='') return;        
        
        var valueList = value.split("");   
        
        if (valueList[0]== "1")
        {
            document.getElementById(cblID + "_" + 0).checked =true;
            for(var i =1;i<valueList.length;i++)
            {
                document.getElementById(cblID + "_" + i).disabled = true;
                document.getElementById(cblID + "_" + i).checked =true;
            }
        }
        else
        {
            for(var i=1;i<valueList.length;i++)
            {
                var index = i;

                switch (cblID) {
                    case "cblInputContactUserType":
                    case "cblViewContactUserType":
                        index = mappingUserRole2ChkContact[i];
                        break;
                    case "cblUserType":            
                        index = mappingUserRole2ScheduleInspection[i];
                        break;
                    default:
                    break;
                }
                
                if(valueList[i] == "1")
                {
                    document.getElementById(cblID + "_" + index).checked  = true;
                }
                else
                {
                    document.getElementById(cblID + "_" + index).checked  = false;
                } 
                document.getElementById(cblID + "_" + index).disabled = false
            }
            
            // set the input contact button status
            if (cblID == "cblInputContactUserType" && valueList[2] == "1"){
                btnInputContactUserTypeConfig.enable();
            }
            
            // set the view contact button status
            if (cblID == "cblViewContactUserType" && valueList[2] == "1"){
                btnViewContactUserTypeConfig.enable();
            }

             /*
            Enable the "Specific Licensed Professional" button 
            when "Licensed Professional" be checked for schedule inspection function.
            */
            if (cblID == "cblUserType" && valueList[2] == "1"){
                btnScheduleInspectionUserTypeConfig.enable();
            }        
        }
    }
   
      
    function GetInspectionSDInfo()
    {
        moduleName = parent.parent.Ext.Const.ModuleName;
        Accela.ACA.Web.WebService.AdminConfigureService.GetInspectionSDInfo(moduleName, CallBack);
    } 
    
    //Fill SD information.
    function CallBack(result)
    {
        var chkDisplayMap4ShowObject = document.getElementById("chkDisplayMap4ShowObject");
        var chkDisplayMap4SelectObject = document.getElementById("chkDisplayMap4SelectObject");
        var rdoDisplayOptionYes = document.getElementById("rdoDisplayOptionYes");
        var rdoDisplayOptionNo = document.getElementById("rdoDisplayOptionNo");
        var rdoAllowMultipleYes = document.getElementById("rdoAllowMultipleYes");
        var rdoAllowMultipleNo = document.getElementById("rdoAllowMultipleNo");
        var chkDisplayPayFeeLink = document.getElementById("chkDisplayPayFeeLink");
        var chkCloneRecord = document.getElementById("chkCloneRecord");
        var chkEnableSearchASI = document.getElementById("chkEnableSearchASI");
        var chkEnableSearchContactTemplate = document.getElementById("chkEnableSearchContactTemplate");
        var displayDefaultContact4InspectionYes = document.getElementById("rdoDisplayDefaultContact4InspectionYes");
        var displayDefaultContact4InspectionNo = document.getElementById("rdoDisplayDefaultContact4InspectionNo");
        
        // Feature:09ACC-08040_Board_Type_Selection
        var chxEnableBoardTypeSelection = document.getElementById("chxBoardTypeSelectionSwitch");

        //Display Map for show object
        var val = result.<%# BizDomainConstant.STD_DISPLAY_MAP_FOR_SHOWOBJECT %>;
        if(typeof(val) == 'boolean' && val)
        {
            chkDisplayMap4ShowObject.checked = true;
        }
        else
        {
            chkDisplayMap4ShowObject.checked = false;
        }

        //Display Map for select object
        val = result.<%# BizDomainConstant.STD_DISPLAY_MAP_FOR_SELECTOBJECT %>;
        if(typeof(val) == 'boolean' && val)
        {
            chkDisplayMap4SelectObject.checked = true;
        }
        else
        {
            chkDisplayMap4SelectObject.checked = false;
        }

        //set role code to user type.
        val = result.<%#ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_SCHEDULE%>;
        SetRoleCode("cblUserType",val);
        
        // set role code for inspection input contact
        val = result.<%#ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_INPUT_CONTACT%>;
        SetRoleCode("cblInputContactUserType",val);
        
        // set role code for inspection view contact
        val = result.<%#ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_VIEW_CONTACT%>;
        SetRoleCode("cblViewContactUserType",val);

        val = result.<%#BizDomainConstant.STD_INSPECTION_DISPLAYOPTION%>;
        if(typeof(val) == 'boolean' && val)
        {
            rdoDisplayOptionYes.checked = true;
        }
        else
        {
            rdoDisplayOptionNo.checked = true;
        }
        
        //set allow multiple inspections. default value: No
        val = result.<%#BizDomainConstant.STD_MULTIPLE_INSPECTIONS_ENABLED%>;
        if(typeof(val) == 'boolean' && val)
        {
            rdoAllowMultipleYes.checked = true;
        }
        else
        {
            rdoAllowMultipleNo.checked = true;
        }

        //set enable display default contact of inspection. default value: Yes
        val = result.<%#XPolicyConstant.INSPECITON_DISPLAY_DEFAULT_CONTACT %>;
        if (typeof(val) == "boolean" && !val) {
            displayDefaultContact4InspectionNo.checked = true;
        } else {
            displayDefaultContact4InspectionYes.checked = true;
        }

        val = result.<%#ACAConstant.ACA_ENABLE_SEARCH_CROSS_MODULE%>;
        SetCrossModels(val);
        
        val = result.<%#XPolicyConstant.PAY_FEE_LINK_DISP%>;
        if(typeof(val) == 'boolean' && val)
        {
            chkDisplayPayFeeLink.checked = true;
        }
        else
        {
            chkDisplayPayFeeLink.checked = false;
        }

        val = result.<%#XPolicyConstant.ENABLE_CLONE_RECORD%>;
        if(typeof(val) == 'boolean' && val)
        {
            chkCloneRecord.checked = true;
        }
        else
        {
            chkCloneRecord.checked = false;
        }        
        
        val = result.<%#XPolicyConstant.ENABLE_SEARCHASI_ADDITIONALCRITERIA%>;
        if(typeof(val) == 'boolean')
        {
            chkEnableSearchASI.checked = val;
        }
        else
        {
            chkEnableSearchASI.checked = true;
        }

        val = result.<%#XPolicyConstant.ENABLE_SEARCHCONTACT_ADDITIONALCRITERIA%>;
        if(typeof(val) == 'boolean')
        {
            chkEnableSearchContactTemplate.checked = val;
        }
        else
        {
            chkEnableSearchContactTemplate.checked = true;
        }

        // Feature:09ACC-08040_Board_Type_Selection
        // Board Type Selection mode is not enabled by default
        val = result.<%#XPolicyConstant.ENABLE_BOARD_TYPE_SELECTION%>;
        if (typeof(val) == 'boolean' && val) {
            chxEnableBoardTypeSelection.checked = true;
        }
        else {
            chxEnableBoardTypeSelection.checked = false;
        }
    }
   
    function GetDisplayEmailSDInfo()
    {
        moduleName = parent.parent.Ext.Const.ModuleName;
        Accela.ACA.Web.WebService.AdminConfigureService.GetDisplayEmailDataInfo(moduleName, CallBackForDisplayEmail); 
    }
    
    //Fill display email address information.
    function CallBackForDisplayEmail(result)
    {
        var chkDisplayEmail = document.getElementById("chkDisplayEmail");
        
        if(result != null)
        {
            if(result.toLowerCase() == "yes"  || result.toLowerCase() == "y")
            {
                chkDisplayEmail.checked = true;
            }
            else
            {
                chkDisplayEmail.checked = false;
            }
        }
    } 
  
    function GetInspectionLabelKeyInfo()
    {
        Accela.ACA.Web.WebService.AdminConfigureService.GetInspectionLabelKeyInfo(CallBack1);
    }
    
    //Fill lablekey information.
    function CallBack1(result)
    {
        var divMapHead = document.getElementById("divMapHead");
        divMapHead.innerHTML = result[0];
        
        var divScheduleHead = document.getElementById("divScheduleHead");
        divScheduleHead.innerHTML = result[1];
        
        var divDisplayInspectionHead = document.getElementById("divDisplayInspectionHead");
        divDisplayInspectionHead.innerHTML = result[2];
        
        var divApplicationHead = document.getElementById("divApplicationHead");
        divApplicationHead.innerHTML = result[3];
        
        var divAllowMultipleInspection = document.getElementById("divAllowMultipleInspection");
        divAllowMultipleInspection.innerHTML = result[4];        
        
        var divInspectionInputContactHead = document.getElementById("divInspectionInputContactHead");
        divInspectionInputContactHead.innerHTML = result[6];
        
        var divInspectionViewContactHead = document.getElementById("divInspectionViewContactHead");
        divInspectionViewContactHead.innerHTML = result[7];

        var dvSocialMediaSettingsHeader = document.getElementById("dvSocialMediaSettingsHeader");
        dvSocialMediaSettingsHeader.innerHTML=result[8];
        
        var divDisplayDefaultContactHead = document.getElementById("divDisplayDefaultContactHead");
        divDisplayDefaultContactHead.innerHTML = result[9];
    }

    //remove current module from changed module array when page load.
    parent.parent.removeChangedItem4CurrentPage();
    
    </script>
</head>
<body style="margin-left: 6px;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        <Services>
            <asp:ServiceReference Path="../../WebService/AdminConfigureService.asmx" />
        </Services>
    </asp:ScriptManager>
    <span style="display: none">
        <ACA:AccelaHideLink ID="hlSkipToolBar" runat="server" AltKey="img_alt_modulemenu_skiplink"
            IsSkippingToAnchor="true" NextControlClientID="FirstAnchorInACAMainContent" TabIndex="-1" />
        <a name="FirstAnchorInACAMainContent" id="FirstAnchorInACAMainContent" tabindex="-1">
        </a></span><span id="FirstAnchorInAdminMainContent" tabindex="-1"></span>
    <div class="ACA_PaddingStyle">
        <fieldset class="module_settings_section">
            <legend>
                <ACA:AccelaLabel ID="lblDisplayMapTitle" runat="server" CssClass="ACA_New_Title_Label font12px"
                    LabelKey="admin_module_setting_label_map_title"></ACA:AccelaLabel>
            </legend>
            <div class="ACA_NewDiv_Text">
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                    <div id="divMapHead" class="ACA_New_Head_Label_Width_90 font11px">
                    </div>
                </div>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <table border="0" cellspacing="0" cellpadding="0" role="presentation">
                    <tr><td>
                        <ACA:AccelaCheckBox ID="chkDisplayMap4ShowObject" runat="server" LabelKey="admin_module_setting_label_map_activate">
                        </ACA:AccelaCheckBox></td><td>
                        <ACA:AccelaCheckBox ID="chkDisplayMap4SelectObject" runat="server" LabelKey="acaadmin_modulesetting_label_map_activate_for_select">
                        </ACA:AccelaCheckBox></td>
                    </tr>
                </table>
            </div>
        </fieldset>
        <fieldset class="module_settings_section">
            <legend>
                <ACA:AccelaLabel ID="lblInspectionTitle" runat="server" CssClass="ACA_New_Title_Label font12px"
                    LabelKey="admin_inspection_setting_label_title"></ACA:AccelaLabel>
            </legend>
            <div class="ACA_NewDiv_Text">
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15">
                    <div id="divScheduleHead" class="ACA_New_Head_Label_Width_90 font11px">
                    </div>
                </div>
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15">
                    <div id="divInspectionInputContactHead" class="ACA_New_Head_Label_Width_90 font11px">
                    </div>
                </div>
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15">
                    <div id="divInspectionViewContactHead" class="ACA_New_Head_Label_Width_90 font11px">
                    </div>
                </div>
                
                <div class="module_settings_desc_item_container">
                    <div id="divDisplayDefaultContactHead" class="module_settings_desc_item">
                    </div>
                </div>

                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15">
                    <div id="divDisplayInspectionHead" class="ACA_New_Head_Label_Width_90 font11px">
                    </div>
                </div>
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                    <div id="divAllowMultipleInspection" class="ACA_New_Head_Label_Width_100 font11px">
                    </div>
                </div>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <ACA:AccelaLabel ID="lblUserType" runat="server" CssClass="ACA_New_Head_Label_Bold font11px"
                    LabelKey="admin_inspection_setting_label_schedule"></ACA:AccelaLabel>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <div class="ACA_FLeft">
                    <ACA:AccelaCheckBoxList ID="cblUserType" runat="server" CssClass="ACA_New_Label font11px"
                        RepeatDirection="Horizontal" RepeatLayout="Flow">
                    </ACA:AccelaCheckBoxList>
                </div>
                <div id="divScheduleInspectionUserTypeConfig">
                </div>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <ACA:AccelaLabel ID="lblInspectionInputContact" runat="server" CssClass="ACA_New_Head_Label_Bold font11px"
                    LabelKey="admin_inspection_setting_label_inputcontact"></ACA:AccelaLabel>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <div class="ACA_FLeft">
                    <ACA:AccelaCheckBoxList ID="cblInputContactUserType" runat="server" CssClass="ACA_New_Label font11px"
                        RepeatDirection="Horizontal" RepeatLayout="Flow">
                    </ACA:AccelaCheckBoxList>
                </div>
                <div id="divInputContactUserTypeConfig">
                </div>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <ACA:AccelaLabel ID="lblInspectionViewContact" runat="server" CssClass="ACA_New_Head_Label_Bold font11px"
                    LabelKey="admin_inspection_setting_label_viewcontact"></ACA:AccelaLabel>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <div class="ACA_FLeft">
                    <ACA:AccelaCheckBoxList ID="cblViewContactUserType" runat="server" CssClass="ACA_New_Label font11px"
                        RepeatDirection="Horizontal" RepeatLayout="Flow">
                    </ACA:AccelaCheckBoxList>
                </div>
                <div id="divViewContactUserTypeConfig">
                </div>
            </div>
            <div class="module_setting_section_title">
                <ACA:AccelaLabel runat="server" ID="lblDisplayDefaultContact4Inspection" CssClass="ACA_New_Head_Label_Bold font11px"
                    LabelKey="acaadmin_modulesetting_label_displaydefaultcontact4inspection" />
            </div>
            <div class="module_setting_section_item">
                <ACA:AccelaRadioButton runat="server" ID="rdoDisplayDefaultContact4InspectionYes" GroupName="DisplayPrimaryContact" 
                    LabelKey="admin_inspection_setting_label_display_yes" CssClass="ACA_New_Label font11px"/>
                <ACA:AccelaRadioButton runat="server" ID="rdoDisplayDefaultContact4InspectionNo" GroupName="DisplayPrimaryContact"
                    LabelKey="admin_inspection_setting_label_display_no" CssClass="ACA_New_Label font11px"/>                
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <ACA:AccelaLabel ID="lblDisplayOption" runat="server" CssClass="ACA_New_Head_Label_Bold font11px"
                    LabelKey="admin_inspection_setting_label_display"></ACA:AccelaLabel>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8 ACA_NewDiv_Text_TabRow_Color_6">
                <ACA:AccelaRadioButton ID="rdoDisplayOptionYes" GroupName="DisplayOption" LabelKey="admin_inspection_setting_label_display_yes"
                    runat="server" CssClass="ACA_New_Label font11px"></ACA:AccelaRadioButton>
                <ACA:AccelaRadioButton ID="rdoDisplayOptionNo" GroupName="DisplayOption" LabelKey="admin_inspection_setting_label_display_no"
                    runat="server" CssClass="ACA_New_Label font11px"></ACA:AccelaRadioButton>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <ACA:AccelaLabel ID="lblMultipleInspections" runat="server" CssClass="ACA_New_Head_Label_Bold font11px"
                    LabelKey="aca_admin_allow_multiple_inspections"></ACA:AccelaLabel>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8 ACA_NewDiv_Text_TabRow_Color_6">
                <ACA:AccelaRadioButton ID="rdoAllowMultipleYes" GroupName="AllowMultipleInspections"
                    LabelKey="admin_inspection_setting_label_display_yes" runat="server" CssClass="ACA_New_Label font11px">
                </ACA:AccelaRadioButton>
                <ACA:AccelaRadioButton ID="rdoAllowMultipleNo" GroupName="AllowMultipleInspections"
                    LabelKey="admin_inspection_setting_label_display_no" runat="server" CssClass="ACA_New_Label font11px">
                </ACA:AccelaRadioButton>
            </div>
        </fieldset>
        <fieldset class="module_settings_section">
            <legend>
                <ACA:AccelaLabel ID="lblDisplayEmailTile" runat="server" CssClass="ACA_New_Title_Label font12px"
                    LabelKey="admin_module_setting_label_display_email_title" />
            </legend>
            <div class="ACA_NewDiv_Text">
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15">
                    <ACA:AccelaLabel ID="lblDisplayEmailDisclaimer" runat="server" CssClass="ACA_New_Head_Label_Width_90 font11px"
                        LabelKey="admin_module_setting_label_display_email_disclaimer" />
                </div>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <ACA:AccelaCheckBox ID="chkDisplayEmail" runat="server" LabelKey="admin_module_setting_label_display_email_activate" />
            </div>
        </fieldset>
        <fieldset class="module_settings_section">
            <legend>
                <ACA:AccelaLabel ID="lblFeeTitle" runat="server" CssClass="ACA_New_Title_Label font12px"
                    LabelKey="admin_module_setting_label_fees_title" />
            </legend>
            <div class="ACA_NewDiv_Text">
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                    <ACA:AccelaLabel ID="lblDisplayPayFeeLink" runat="server" CssClass="ACA_New_Head_Label_Width_90 font11px"
                        LabelKey="admin_module_setting_label_display_payfee_disclaimer" />
                </div>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <ACA:AccelaCheckBox ID="chkDisplayPayFeeLink" runat="server" LabelKey="admin_module_setting_label_display_payfee_activate" />
            </div>
        </fieldset>
        <fieldset class="module_settings_section">
            <legend>
                <ACA:AccelaLabel ID="lblCloneTitle" runat="server" CssClass="ACA_New_Title_Label font12px"
                    LabelKey="acaadmin_inspectionsetting_label_clone_title" />
            </legend>
            <div class="ACA_NewDiv_Text">
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                    <ACA:AccelaLabel ID="lblCloneRecord" runat="server" CssClass="ACA_New_Head_Label_Width_90 font11px"
                        LabelKey="acaadmin_inspectionsetting_label_clone_disclaimer" />
                </div>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <ACA:AccelaCheckBox ID="chkCloneRecord" runat="server" LabelKey="acaadmin_inspectionsetting_label_clone_activate" />
            </div>
        </fieldset>
        <fieldset class="module_settings_section">
            <legend>
                <ACA:AccelaLabel ID="AccelaLabel1" LabelKey="ACA_Contact_Display_Controls" runat="server"
                    CssClass="ACA_New_Title_Label font12px"></ACA:AccelaLabel>
            </legend>
            <div class="ACA_NewDiv_Text">
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                    <ACA:AccelaLabel ID="AccelaLabel2" LabelKey="ACA_Contact_Display_Controls_MSG" runat="server"
                        CssClass="ACA_New_Head_Label_Width_90 font11px"></ACA:AccelaLabel>
                </div>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10" id="ContactGrid">
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <table id="tbOptionalBtn" cellspacing="10" cellpadding="4" role="presentation">
                    <tr>
                        <td id="tdOK">
                        </td>
                        <td id="tdCancel">
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
        <!--Begin Cap Search Role section -->
        <fieldset class="module_settings_section">
            <legend>
                <ACA:AccelaLabel ID="AccelaLabel7" LabelKey="admin_global_setting_label_cap_filter_title"
                    runat="server" CssClass="ACA_New_Title_Label font12px"></ACA:AccelaLabel>
            </legend>
            <table class="module_setting_section_wrapper" role="presentation">
                <tr>
                    <td>
                        <div class="ACA_NewDiv_Text">
                            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                                <ACA:AccelaLabel ID="AccelaLabel8" LabelKey="admin_inspection_setting_label_captypesearch_role_instruction"
                                    runat="server" CssClass="ACA_New_Head_Label_Width_90 font11px"></ACA:AccelaLabel>
                            </div>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10">
                            <ACA:AccelaRadioButton ID="rdoModuleLevel" GroupName="SearchRole" LabelKey="admin_inspection_setting_label_modulelevelrole"
                                onclick="ChangeSearchRoleLevelType(this)" value="0" runat="server" CssClass="ACA_New_Label font11px">
                            </ACA:AccelaRadioButton>
                            <ACA:AccelaRadioButton ID="rdoEachCapTypeLevel" GroupName="SearchRole" LabelKey="admin_inspection_setting_label_captypelevelrole"
                                runat="server" onclick="ChangeSearchRoleLevelType(this)" value="1" CssClass="ACA_New_Label font11px">
                            </ACA:AccelaRadioButton>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10" id="CapTypeSearchRoleGrid">
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                            <table id="Table2" cellspacing="10" cellpadding="4" role="presentation">
                                <tr>
                                    <td id="tdSaveCapSearchRole">
                                    </td>
                                    <td id="tdCancelSaveCapSearchRole">
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
        <!--End Cap Search Role section -->
        <fieldset class="module_settings_section">
            <legend>
                <ACA:AccelaLabel ID="lblInspectionApplicationTitle" runat="server" CssClass="ACA_New_Title_Label font12px"
                    LabelKey="admin_inspection_setting_label_application_title"></ACA:AccelaLabel>
            </legend>
            <div class="ACA_NewDiv_Text">
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                    <div id="divApplicationHead" class="ACA_New_Head_Label_Width_90 font11px">
                    </div>
                    <ACA:AccelaLabel ID="lblInspectionActionPermissDeclaimer" runat="server" CssClass="ACA_New_Head_Label_Width_90 font11px"
                        LabelKey="admin_application_inspectionpermiassion_setting_desclaimer" />
                </div>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <ACA:AccelaLabel ID="lblInspectionApplicationGroup" runat="server" CssClass="ACA_New_Head_Label_Bold font11px"
                    LabelKey="admin_inspection_setting_label_application_group"></ACA:AccelaLabel>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <table role="presentation">
                    <tr>
                        <td>
                            <div id="divTree">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <!--Begin Inspection Setting -->
            <div id="divInspectionActionSetting" class="ACA_Hide">
                <ACA:AccelaHeightSeparate ID="sepForInspection" runat="server" Height="10" />
                <div class="ACA_NewDiv_Text">
                    <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                        <ACA:AccelaLabel ID="lblDesclaimer" LabelKey="admin_application_inspectiongroup_desclaimer"
                            runat="server" CssClass="ACA_New_Head_Label_Bold font11px" />
                    </div>
                    <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8 ACA_NewDiv_Text_TabRow_Padding_Bottom_2">
                        <ACA:AccelaDropDownList ID="ddlInsGroup" runat="server" CssClass="ACA_New_Label font11px"
                            ToolTip="Please select a inspection group." />
                        <br />
                    </div>
                </div>
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10" style="width: 100%;" id="divInsActionPermissionsList">
                </div>
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                    <table id="Table3" role="presentation">
                        <tr>
                            <td id="tdInspectionOk">
                            </td>
                            <td id="tdInspectionCannel" class="ACA_CapListStyle">
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <!--End Inspection Setting -->
        </fieldset>
    </div>
    <div id="divAmendmentFieldset" class="ACA_PaddingStyle">
        <fieldset class="module_settings_section">
            <legend>
                <ACA:AccelaLabel ID="lblCombineButtonWithCAPType" LabelKey="admin_global_setting_label_CombineButtonWithCAPType"
                    runat="server" CssClass="ACA_New_Title_Label font12px"></ACA:AccelaLabel>
            </legend>
            <div class="ACA_NewDiv_Text">
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                    <ACA:AccelaLabel ID="lblCombineHead" runat="server" CssClass="ACA_New_Head_Label_Bold font11px"
                        LabelKey="admin_global_setting_label_CombineHead"></ACA:AccelaLabel>
                </div>
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8 ACA_NewDiv_Text_TabRow_Padding_Bottom_2">
                    <ACA:AccelaDropDownList ID="ddlAmendmentButtonName" runat="server" CssClass="ACA_New_Label font11px"
                        ToolTip="Please select a amendment button name.">
                    </ACA:AccelaDropDownList>
                    <br />
                </div>
            </div>
            <div id="divAmendment">
                <div id="capAssociation">
                    <div id="newSmartchoiceGroup" class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8 ACA_NewDiv_Text_TabRow_Padding_Bottom_2">
                        <table border="0" width="98%" role="presentation">
                            <tr>
                                <td>
                                    <div id="availableCap">
                                    </div>
                                </td>
                                <td valign="middle" align="center">
                                    <div id="addCap">
                                    </div>
                                    <br />
                                    <br />
                                    <div id="removeCap">
                                    </div>
                                </td>
                                <td>
                                    <div id="selectedCap">
                                    </div>
                                </td>
                                <td id="tdStatus">
                                    <div id="divStatusTree">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divFilterName" class="ACA_NewDiv_Text module_setting_amendment_margin">
                        <div class="module_setting_amendment_filtername">
                            <ACA:AccelaLabel ID="lblFilterTitle" runat="server" CssClass="ACA_New_Head_Label_Bold font11px"
                                LabelKey="acaadmin_modulesetting_label_selectamendmentfilter"></ACA:AccelaLabel>
                            <ACA:AccelaDropDownList ID="ddlFilterName" onchange="UpdateFilteName(this)" runat="server"
                                CssClass="ACA_New_Label font12px ACA_Amendment_Filter" ToolTip="Please select a CAP type filter">
                            </ACA:AccelaDropDownList>
                        </div>
                    </div>
                    <div id="buttonAreaAmendment" class="ACA_LgButton ACA_LgButton_FontSize ACA_LiLeft">
                        <ul>
                            <li>
                                <div id="OK" />
                            </li>
                            <li>
                                <div id="Cancel" />
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </fieldset>
    </div>
    <!--begin cross module search -->
    <div id="divCMSearch" class="ACA_PaddingStyle">
        <fieldset style="padding: 8px; width: 96%;">
            <legend>
                <ACA:AccelaLabel ID="lblSearchTitle" runat="server" LabelKey="aca_record_search_title"
                    CssClass="ACA_New_Title_Label font12px" />
            </legend>
            <div class="ACA_NewDiv_Text">
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                    <div id="div4" class="ACA_New_Head_Label_Width_100 font11px">
                        <ACA:AccelaLabel ID="AccelaLabel6" runat="server" LabelKey="aca_cross_module_search_head" />
                    </div>
                    <div class="ACA_New_Head_Label_Width_100 font11px">
                        <ACA:AccelaLabel ID="lbladditionalsearchcriteriahead" runat="server" LabelKey="aca_additionalsearchcriteria_head" />
                    </div>
                </div>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <ACA:AccelaLabel ID="lblCrossModuleSettings" runat="server" CssClass="ACA_New_Head_Label_Bold font11px"
                    LabelKey="admin_crossmodulesettings_title"></ACA:AccelaLabel>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <ACA:AccelaCheckBoxList ID="chkCMSearchModules" runat="server" CssClass="ACA_New_Label font11px"
                    RepeatDirection="Horizontal" RepeatLayout="Flow" DataTextField="Value" DataValueField="Key">
                </ACA:AccelaCheckBoxList>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                <ACA:AccelaLabel ID="lblAdditionalSearchSettings" runat="server" CssClass="ACA_New_Head_Label_Bold font11px"
                    LabelKey="admin_additionalsearchsetting_title"></ACA:AccelaLabel>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8 ACA_NewDiv_Text_TabRow_Color_6">
                <ACA:AccelaCheckBox ID="chkEnableSearchASI" runat="server" LabelKey="admin_search_asi_additionalcriteria" />
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8 ACA_NewDiv_Text_TabRow_Color_6">
                <ACA:AccelaCheckBox ID="chkEnableSearchContactTemplate" runat="server" LabelKey="admin_search_contacttemplate_additionalcriteria" />
            </div>
        </fieldset>
    </div>
    <!--End cross module search -->
    </form>
    <div id="divFilter" class="ACA_PaddingStyle">
        <!--CAP type filter section -->
        <fieldset class="module_settings_section">
            <legend>
                <ACA:AccelaLabel ID="AccelaLabel9" runat="server"></ACA:AccelaLabel>
                <span id="divCapFilterTitle"></span></legend>
            <div id="BoardTypeSelectionSection">
                <div class="ACA_NewDiv_Text">
                    <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                        <ACA:AccelaLabel ID="AccelaLabel45" LabelKey="admin_inspection_setting_cap_filter_board_type_selection_title"
                            runat="server" CssClass="ACA_New_Head_Label_Width_90 font11px"></ACA:AccelaLabel>
                    </div>
                </div>
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8 ACA_NewDiv_Text_TabRow_Color_6"
                    style="width: 100%;" id="divBoardTypeSectionSwitch">
                    <span class="ACA_New_Head_Label_Bold_11 font11px aca_checkbox">
                        <input name="chxBoardTypeSelectionSwitch" id="chxBoardTypeSelectionSwitch" type="checkbox"
                            onclick="UpdateDataInfo();" class="ACA_Label" />
                        <label id="lblBoarcTypeSelectionSwitchLabel" for="chxBoardTypeSelectionSwitch">
                        </label>
                    </span>
                </div>
            </div>
            <div id="CapTypeFilter">
                <div id="divFilterHeader" style="margin-bottom: -4px">
                </div>
                <div id="divFilterBody">
                    <div id="filterCapList">
                    </div>
                    <div id="filterCapAssociation">
                        <div id="newCapTypeFilter">
                            <table border="0" width="98%" role="presentation">
                                <tr>
                                    <td colspan="3">
                                        <div id="NewCapTypefilterName">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="availableFilterCap">
                                        </div>
                                    </td>
                                    <td valign="middle" align="center">
                                        <div id="addFilterCap">
                                        </div>
                                        <br />
                                        <br />
                                        <div id="removeFilterCap">
                                        </div>
                                    </td>
                                    <td>
                                        <div id="selectedFilterCap">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="buttonArea" class="ACA_LgButton ACA_LgButton_FontSize ACA_LiLeft">
                            <ul>
                                <li>
                                    <div id="btnFilterOK" />
                                </li>
                                <li>
                                    <div id="btnFilterCancel" />
                                </li>
                                <li>
                                    <div id="btnFilterDelete">
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <!--End CAP type filter section -->
        <!--admin_inspection_setting_label_Board_Type_Selection_Switch_Label-->
        <!--admin_inspection_setting_label_Board_Type_Selection_Header -->
    </div>
    <!--Begin Report Previlege section -->
    <div id="divReportPrivilege" class="ACA_PaddingStyle">
        <fieldset class="module_settings_section">
            <legend>
                <ACA:AccelaLabel ID="AccelaLabel3" LabelKey="admin_report_display_controls" runat="server"
                    CssClass="ACA_New_Title_Label font12px"></ACA:AccelaLabel>
            </legend>
            <div class="ACA_NewDiv_Text">
                <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                    <ACA:AccelaLabel ID="AccelaLabel4" LabelKey="admin_inspection_setting_label_report_role_instruction"
                        runat="server" CssClass="ACA_New_Head_Label_Width_90 font11px"></ACA:AccelaLabel>
                </div>
            </div>
            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10" style="width: 100%;" id="ReportGrid">
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
        </fieldset>
    </div>
    <!--End Report Previlege section -->
    <!--Begin LicenseVerification settings -->
    <div class="ACA_PaddingStyle">
        <fieldset class="module_settings_section">
            <legend>
                <ACA:AccelaLabel ID="lblLicenseVerificationTitle" LabelKey="license_verifacation_gridtitle"
                    runat="server" CssClass="ACA_New_Title_Label font12px"></ACA:AccelaLabel>
            </legend>
            <table class="module_setting_section_wrapper" role="presentation">
                <tr>
                    <td>
                        <div class="ACA_NewDiv_Text">
                            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                                <ACA:AccelaLabel ID="lblLicenseVerificationInstruction" LabelKey="license_verifacation_instruction"
                                    runat="server" CssClass="ACA_New_Head_Label_Width_90  font11px"></ACA:AccelaLabel>
                            </div>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10" id="LicenseVerificationGrid">
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <!--End LicenseVerification settings -->
    <!--Begin Record detail Role section -->
    <div class="ACA_PaddingStyle">
        <fieldset class="module_settings_section">
            <legend>
                <ACA:AccelaLabel ID="lblRecordDetailSectionRoleTitle" LabelKey="record_detail_role_title"
                    runat="server" CssClass="ACA_New_Title_Label font12px"></ACA:AccelaLabel>
            </legend>
            <table class="module_setting_section_wrapper" role="presentation">
                <tr>
                    <td>
                        <div class="ACA_NewDiv_Text">
                            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                                <ACA:AccelaLabel ID="lblRecordDetailRoleInstruction" LabelKey="record_detail_role_instruction"
                                    runat="server" CssClass="ACA_New_Head_Label_Width_90 font11px"></ACA:AccelaLabel>
                            </div>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10" id="CapDetailSectionRoleGrid">
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                            <table id="tbSectionRole" cellspacing="10" cellpadding="4" role="presentation">
                                <tr>
                                    <td id="tdSaveSectionRole">
                                    </td>
                                    <td id="tdCancelSaveSectionRole">
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <!--End Record detail Role section -->
    <!--Begin Social Media Settings section -->
    <div class="ACA_PaddingStyle">
        <fieldset class="module_settings_section">
            <legend>
                <ACA:AccelaLabel ID="lblSocialMediaSettingsSectionTitle" Text="Social Media Settings"
                    runat="server" CssClass="ACA_New_Title_Label font12px"></ACA:AccelaLabel>
            </legend>
            <table class="module_setting_section_wrapper" role="presentation">
                <tr>
                    <td>
                        <div class="ACA_NewDiv_Text">
                            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                                <div id="dvSocialMediaSettingsHeader" class="ACA_New_Head_Label_Width_90 font11px">
                                </div>
                            </div>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                            <ACA:AccelaLabel ID="AccelaLabel5" runat="server" CssClass="ACA_New_Head_Label_Bold font11px"
                                LabelKey="acaadmin_modulesettings_label_sharebutton"></ACA:AccelaLabel>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8 ACA_NewDiv_Text_TabRow_Color_6">
                            <span class="ACA_New_Label font11px aca_checkbox aca_checkbox_fontsize">
                                <input type="radio" id="rdoShareButtonWithAll" name="ShareButton" runat="server"
                                    value="All" onclick="UpdateDataInfo()" /><label id="lblShareButtonWithAll" for="rdoShareButtonWithAll" runat="server"></label>
                             </span>
                             <span class="ACA_New_Label font11px aca_checkbox aca_checkbox_fontsize">
                                <input type="radio" id="rdoShareButtonWithOwner" name="ShareButton" runat="server"
                                    value="Creator" onclick="UpdateDataInfo()" /><label for="rdoShareButtonWithOwner" id="lblShareButtonWithOwner" runat="server"></label>
                             </span>

                             <span class="ACA_New_Label font11px aca_checkbox aca_checkbox_fontsize">
                                <input type="radio" id="rdoShareButtonWithNone" name="ShareButton" runat="server"  value="None" onclick="UpdateDataInfo()" /><label for="rdoShareButtonWithNone" id="lblShareButtonWithNone" runat="server"></label>
                            </span>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                            <ACA:AccelaLabel ID="lblShareComments" runat="server" CssClass="ACA_New_Head_Label_Bold font11px"
                                LabelKey="acaadmin_modulesettings_label_sharedcomments"></ACA:AccelaLabel>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                            <span class="ACA_New_Head_Label_Width_90 font11px">Click the area below to edit general
                                condition available variables according to format:</span>
                            <br />
                            <span class="ACA_New_Head_Label_Width_90 font11px">$$capID$$: The Record ID.</span>
                            <br />
                            <span class="ACA_New_Head_Label_Width_90 font11px">$$ModuleName$$: The Record Module
                                Name.</span>
                            <br />
                            <span class="ACA_New_Head_Label_Width_90 font11px">$$capType$$: The Record type.</span>
                            <br />
                            <span class="ACA_New_Head_Label_Width_90 font11px">$$Address$$: The Record address.</span>
                            <br />
                            <span class="ACA_New_Head_Label_Width_90 font11px">$$Status$$: The Record status.</span>
                            <br />
                            <span class="ACA_New_Head_Label_Width_90 font11px">$$StatusDate$$: The Date of cap status change.</span>
                            <br />
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                            <textarea runat="server" id="txtSharedComments" class="aca_css_editor"></textarea>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <!--End Socia Media Settings section -->
    <noscript>
        <%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</body>
<script type="text/javascript">
    //Get web application root, ends with /
    function getAppRoot() {
        return '<%=Accela.ACA.Web.Common.FileUtil.ApplicationRoot %>';
    }
</script>
</html>
