
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: common.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: common.js 72643 2008-04-24 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/18/2008     		Kevin.Feng				Initial.  
 * </pre>
 */

/*
    ||Define the array object which will save the changed items
*/

var changeItems = new ChangeItems();
var colorThemeChangeItems = new ColorThemeChangeItems();
var pageGlobalItems = new PageGlobalItems();
var pageModuleItems = new PageModuleItems();
var pageRegistrationItems = new PageRegistrationItems();
var pageInspectionItems = new PageInspectionItems();
var pageAppStatusItems = new PageAppStatusItems();

var isColorWinShow = false;
var isHtmlWinShow = false;
var isDDLWinShow = false;
var isCloseOtherTab = false;
var changedItemList = new Array();

var pattern = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/; 
/*
    ||Settings star mark
*/
var openId;
///
//sectionName: Identify current modified section name. this param only needs in global setting page and module setting page
//if the section has own "Save" button, then pass the section name to this param. Otherwise pass "global" if in global setting page
// or pass "module" in module setting page
///
function ModifyMark(sectionName) {
    var tab = Ext.getCmp(Ext.Const.OpenedId);
    setChangedItem(sectionName);
    
    if(tab != undefined)
    {         
        if(tab.title.indexOf('*') == -1)
        {     
            var btnSave = Ext.getCmp("btnSave");
            btnSave.enable();
            tab.setTitle(tab.title + ' *');
        }
    }
}

///
//isExist: if pageflow name exists already.
//sectionName: Identify current modified section name. this param only needs in global setting page and module setting page
//if the section has own "Save" button, then pass the section name to this param. Otherwise pass "global" if in global setting page
// or pass "module" in module setting page
///
function RemoveMark(isExist, sectionName){
    var tab;
    Ext.MessageBox.hide();
    if(isExist){
    	alert(Ext.LabelKey.Admin_Pageflow_Message_InvalidSmartchoice);
    }
    if (Ext.Const.OpenedId != -1)
    {
        //alert(isCloseOtherTab);
        if (isCloseOtherTab == false)
        {
            // when user clicks [OK] button in gloable setting or module setting page to save the specific items,
            // we need to judge whether other items have changed.
            removeChangedItem(sectionName);
            if (sectionName && hasOtherChangedItem()) 
            {
                return;
            }
            
            tab = Ext.getCmp(Ext.Const.OpenedId);   
            if(tab != undefined)
            {         
                if(tab.title.indexOf('*')>0)
                {     
                    var btnSave = Ext.getCmp("btnSave");
                    btnSave.disable();
                    tab.setTitle(tab.title.replace(' *',''));
                }
            }
        }
        else {
            tab = Ext.getCmp(Ext.Const.OpenedId);
            if(tab != undefined)
            {         
                if(tab.title.indexOf('*')>0)
                {     
                    var btnSave = Ext.getCmp("btnSave");
                    btnSave.enable();
                    isCloseOtherTab = false;
                }
            }
            
        }
    }
}

//Add changed item to changed item list.
function setChangedItem(itemName) {

    if (itemName) 
    {
        var hasContainedItem;
        var changedItem = Ext.Const.OpenedId + Ext.Const.SplitChar + itemName;
        
        for (var i = 0; i < changedItemList.length; i++) 
        {
            if (changedItemList[i] == changedItem) 
            {
                hasContainedItem = true;
                break;
            }
        }

        if (!hasContainedItem) 
        {
            changedItemList[changedItemList.length] = changedItem;
        }
    }
}

//Remove changed item from list.
function removeChangedItem(itemName) 
{
    var changedItem = Ext.Const.OpenedId + Ext.Const.SplitChar + itemName;
    var index = changedItemList.indexOf(changedItem);

    if (index > -1) 
    {
        changedItemList.splice(index, 1);
    }
}

//Remove all current page's changed items.
function removeChangedItem4CurrentPage() 
{
    for (var i = changedItemList.length - 1; i > -1; i--) 
    {
        if (changedItemList[i].indexOf(Ext.Const.OpenedId) != -1) 
        {
            changedItemList.splice(i, 1);
        }
    }
}

//Check if the current page has changed items.
function hasOtherChangedItem() 
{
    var pageID = Ext.Const.OpenedId;
    if (pageID.indexOf('GlobalSettings') != -1 || pageID.indexOf('ModuleSettings') != -1)
    {
        for (var i = 0; i < changedItemList.length; i++)
        {
            if (changedItemList[i].indexOf(pageID) != -1) 
            {
                return true;
            }
        }
    }

    return false;
}

//Check is a valid email.
function CheckEmailByArrayTo(obj)
{
    var result;
    var emails = obj.split(";");
    
    for(var i = 0;i < emails.length;i++)
    {
        if(!pattern.test(emails[i])) 
        {
            result = false;
            break;
        } 
        else 
        {
            result = true;
        }
    }
    
    return result;
} 

//Check is a valid email.
function CheckEmailByArrayFrom(obj)
{
    var result;
    
    if(!pattern.test(obj)) 
    {
        result = false;
    } 
    else 
    {
        result = true;
    }
    
    return result;
} 

//Clear data entries.
function ClearDataEntries()
{
    var menuName = Ext.Const.OpenedId.split(Ext.Const.SplitChar);
    
    if(Ext.Const.OpenedId.indexOf('GlobalSettings') != -1){
    pageGlobalItems.RemovePageChangedItemsByPageId('global');
    }
    else if(Ext.Const.OpenedId.indexOf('FeatureSettings') != -1)
    {
        pageModuleItems.RemovePageChangedItemsByPageId('module');
    }
    else if(Ext.Const.OpenedId.indexOf('RegistrationSettings') != -1)
    {
        pageRegistrationItems.RemovePageChangedItemsByPageId('registration');
    }
    else if(Ext.Const.OpenedId.indexOf('ModuleSettings') != -1)
    {
        pageInspectionItems.RemovePageChangedItemsByPageId('inspection');
        pageAppStatusItems.RemovePageChangedItemsByPageId('appstatus');
    }
    else
    {
        changeItems.RemoveChangedItemsByPageId(Ext.Const.OpenedId);
    } 
}

function CallBack(result)
{ 
    var btnSave = Ext.getCmp("btnSave");
    try{
        Ext.MessageBox.hide();  
        
        if(result == true){
            alert(Ext.LabelKey.Admin_Common_SaveSuccess);
            
            //TODO:Call setTimeout function  
            ClearDataEntries();
            RemoveMark();
        }
        else{
            alert(Ext.LabelKey.Admin_Common_Error);
            btnSave.enable();
        }
    }
    catch(e){
        Ext.MessageBox.hide();
        btnSave.enable();
    }
}

/* 
 * Hides current window and drag div.
 * @param win (Ext.Window) The window for hide
 */
function HideWindowAndDragDiv(win)
{
    try
    {
        //Try to hide the ghost div.
        if (win.activeGhost && win.activeGhost.visible && Ext.dd.DragDropMgr) {
            if (Ext.dd.DragDropMgr.clickTimeout != null) {
                clearTimeout(Ext.dd.DragDropMgr.clickTimeout);
            }

            Ext.dd.DragDropMgr.stopDrag();
        }
    }
    catch (e)
    {
    }

    try
    {
        win.hide();
    }
    catch (e)
    {
    }
}

function CloseWindow(closeId)
{
    if(closeId != null && closeId != Ext.Const.OpenedId)
    {
        return;
    }

    saveobj = null;

    if (isHtmlWinShow)
    {
        HideWindowAndDragDiv(win);
    }

    if (isColorWinShow) {
        HideWindowAndDragDiv(colorWin);
    }
    
    try {
        if (Ext.Const.OpenedId.indexOf('PageFlowConfiguration') != -1) {
            var ifrId = Ext.getCmp("tabs").activeTab.body.dom.firstChild.id;
            eval(ifrId + '.CloseWorkFlowDDLWin();');
        }

        if(isDDLWinShow)
        {
            CloseDDLWin();
        }
    }
    catch(err)
    {
    }
}
/*
    ||Save
*/
function showMessageBox(progressMsg) {
    //  document.body.style.cursor = "wait";
    if (progressMsg == null || progressMsg == undefined) {
        progressMsg = 'Saving';
    }
    Ext.MessageBox.show({
        msg: progressMsg + ' your data, please wait...',
        progressText: progressMsg + '...',
        width:300,
        wait:true,
        waitConfig: {interval:100},
        icon:Ext.MessageBox.INFO
    });
}

function ShowErrorMessage(msg)
{
    alert('An error has occurred.\n' + msg.replace(/<br>|<br\/>/gi, '\n'));
}

function SaveFilters(arrFilter) {
    if (arrFilter == null || arrFilter.length < 1) {
        return;
    }

    for (var i = 0; i < arrFilter.length; i++) {
        if (arrFilter[i].FilterName != null) {
            Accela.ACA.Web.WebService.AdminConfigureService.CreateOrEditFilter4Button(arrFilter[i].ModuleName, arrFilter[i].LabelKey, arrFilter[i].FilterName, null);
        }
    }
}

function SaveConfigureButtonUrl(arrConfigureButton) {
    if (arrConfigureButton == null || arrConfigureButton.length < 1) {
        return;
    }

    for (var i = 0; i < arrConfigureButton.length; i++) {
        if (arrConfigureButton[i].ConfigureUrlId != null) {
            Accela.ACA.Web.WebService.AdminConfigureService.SaveConfigureButtonUrl(arrConfigureButton[i].ModuleName, arrConfigureButton[i].LabelKey, arrConfigureButton[i].ConfigureUrlId, null);
        }
    }
}

function SaveExpandedSectionTitle(dataEntries) {
    var arrText = dataEntries.Items[dataEntries.ItemType.arrText];

    if (arrText != null && arrText.length > 0) {
        for (var i = 0; i < arrText.length; i++) {
            var expanded = arrText[i].Expanded;
            var policyName = Ext.Constant.IS_EXPANDED_SECTION;
            var labelKey = arrText[i].LabelKey;

            if (expanded != null && labelKey != null) {
                Accela.ACA.Web.WebService.AdminConfigureService.SavePolicyValueForData4AsKey(Ext.Const.ModuleName, expanded, policyName, labelKey, null);
            }
        }
    }
}

function SaveAutoFill(dataEntries) {
    var arrInput = dataEntries.Items[dataEntries.ItemType.arrInput];
    if(arrInput != null && arrInput.length > 0)
    {
        for(var i = 0;i<arrInput.length;i++)
        {
            var autoFillValue = arrInput[i].AutoFillValue;
            var policyName = arrInput[i].PolicyName;
            var positionID = arrInput[i].PositionID;
            if(autoFillValue != null && positionID != null)
            {
                Accela.ACA.Web.WebService.AdminConfigureService.SavePolicyValueForData4AsKey(Ext.Const.ModuleName, autoFillValue, policyName, positionID, null);
            }
        }
    }

    var arrDropDown = dataEntries.Items[dataEntries.ItemType.arrDropDown];
    if(arrDropDown != null && arrDropDown.length > 0)
    {
        for(var i = 0;i<arrDropDown.length;i++)
        {
            var autoFillValue = arrDropDown[i].AutoFillValue;
            var policyName = arrDropDown[i].PolicyName;
            var positionID = arrDropDown[i].PositionID;
            if(autoFillValue != null && positionID != null)
            {
                Accela.ACA.Web.WebService.AdminConfigureService.SaveAutoFillValue(Ext.Const.ModuleName, autoFillValue, policyName, positionID, null);
            }
        }
    }
}

function SaveReports(arrayReports) 
{
    if (arrayReports == null || arrayReports.length < 1) 
    {
        return;
    }

    if (arrayReports[0].reports != null && arrayReports[0].reports.length > 0) 
    {
        var array = arrayReports[0].PageId.split(Ext.Const.SplitChar);
        var pageID = array[0].substring(array[0].length - 4); // the last 4 data is element id.
        Accela.ACA.Web.WebService.AdminConfigureService.SaveReportsForPage(pageID, Ext.Const.ModuleName, arrayReports[0].reports, null);
    }
}

function GetChildNode(parentNode) 
{
    var returnNode = null;

    if (parentNode != null) 
    {
        for (var i = 0; i < parentNode.childNodes.length; i++) 
        {
            if (!parentNode.childNodes[i].nodeValue) 
            {
                returnNode = parentNode.childNodes[i];
            }
        }
    }

    return returnNode;
}

function JudgeContentLength(content)
{
    if(content.length > 4000)//The Email content is larger than 4000 length.
    {
        var btnSave = Ext.getCmp("btnSave"); 
        btnSave.enable();
        ShowErrorMessage(Ext.LabelKey.Admin_Email_Body_IsLarger);
        return false; 
    } 
   
  return true;
}

function ExcuteMethod(methodName) 
{
    var ifrId;
    var activeTab = Ext.getCmp("tabs").activeTab;
    if (activeTab && activeTab.body) 
    {
      ifrId = activeTab.body.dom.firstChild.id;
    }
    if(Ext.isIE)
    {
       eval(ifrId+'.'+ methodName + ';');
    }
    else
    {
       var iframeObj=document.getElementById(ifrId);
       eval('iframeObj.contentWindow.' + methodName);
    }
}

function FormatNumberString(numberString) {
    if (numberString != "" && numberString != 'undefined') {
        var tempString = numberString;
        var lastIndex = numberString.length - 1;

        for (var i = 0; i < tempString.length; i++) {
            var temp = tempString.substring(i, i + 1);
            if (temp > 0) {
                numberString = tempString.substring(i, lastIndex + 1);
                break;
            } else if (i == lastIndex) {
                numberString = 0;
            }
        }
    } else {
        numberString = 0;
    }

    return numberString;
}

//This function will return false, if save-operation is failed or aborted!
function SaveHandle(currId)
{
    var saveResult = 0;
    if(IsSesstionTimeout()){
      RedirectToLoginPage();
    }
    else {
        if(Ext.Const.OpenedId != -1){        
            var menuName = currId.split(Ext.Const.SplitChar) ;
            Ext.Const.CurrId = currId;
            var btnSave = Ext.getCmp("btnSave");
            btnSave.disable();

            if (Ext.Const.OpenedId.indexOf('GlobalSettings') != -1) {
                var dataEntries = pageGlobalItems.GetPageChangedItemsByPageId('global');

                if (dataEntries.arrGlobal[0] == undefined || dataEntries.arrGlobal[0] == null) {
                    ExcuteMethod('SaveReportRoles()');
                    showMessageBox();
                    return;
                }

                if (dataEntries.arrGlobal[0].chkGlobalSearchSwitch == true && dataEntries.arrGlobal[0].chkGlobalSearchCAPResultGroup == false
                    && dataEntries.arrGlobal[0].chkGlobalSearchLPResultGroup == false && dataEntries.arrGlobal[0].chkGlobalSearchAPOResultGroup == false) {
                    ShowErrorMessage(Ext.LabelKey.Admin_GlobalSearch_SaveAlert_Message);
                    return;
                }

                dataEntries.arrGlobal[0].txtSelectExpiration = FormatNumberString(dataEntries.arrGlobal[0].txtSelectExpiration);
                dataEntries.arrGlobal[0].txtSaveExpiration = FormatNumberString(dataEntries.arrGlobal[0].txtSaveExpiration);
                // Validate delegate user settings
                dataEntries.arrGlobal[0].txtProxyUserExpiredDate = FormatNumberString(dataEntries.arrGlobal[0].txtProxyUserExpiredDate);
                dataEntries.arrGlobal[0].txtProxyUserExpiredRemoveDate = FormatNumberString(dataEntries.arrGlobal[0].txtProxyUserExpiredRemoveDate);
            
                ExcuteMethod('SaveReportRoles()');

                showMessageBox();
                removeChangedItem4CurrentPage();
                Accela.ACA.Web.WebService.AdminConfigureService.SaveGlobalSettingInfo(dataEntries, CallBack);
            }
            else if (Ext.Const.OpenedId.indexOf('FeatureSettings') != -1) {
                var dataEntries = pageModuleItems.GetPageChangedItemsByPageId('module');
                showMessageBox();
                Accela.ACA.Web.WebService.AdminConfigureService.SaveModuleSetupInfo(dataEntries, CallBack);
            }
            else if (Ext.Const.OpenedId.indexOf('RegistrationSettings') != -1) {
                var dataEntries = pageRegistrationItems.GetPageChangedItemsByPageId('registration');

                if (dataEntries.arrRegistration[0].chkInterval && (dataEntries.arrRegistration[0].txtInterval == "" || dataEntries.arrRegistration[0].txtInterval < 1)) {
                    btnSave.enable();
                    ShowErrorMessage(Ext.LabelKey.Admin_Registration_Verification_Expired_Day_IsNull);     
                    return;
                }

                // Check password settings
                if (dataEntries.arrRegistration[0].chkPasswordExpiration
                && (dataEntries.arrRegistration[0].txtPasswordExpirationDays == "" || dataEntries.arrRegistration[0].txtPasswordExpirationDays < 1)) {
                    btnSave.enable();
                    ShowErrorMessage(Ext.LabelKey.Admin_Registration_Verification_Expired_Day_IsNull);
                    return;
                }

                if (dataEntries.arrRegistration[0].chkPasswordFailedAttempts
                && (dataEntries.arrRegistration[0].txtPasswordFailedTimes == "" || dataEntries.arrRegistration[0].txtPasswordFailedTimes < 1
                    || dataEntries.arrRegistration[0].txtPasswordFailedDurations == "" || dataEntries.arrRegistration[0].txtPasswordFailedDurations < 1)) {
                    btnSave.enable();
                    ShowErrorMessage(Ext.LabelKey.Admin_Registration_Verification_Expired_Day_IsNull);
                    return;
                }

                if (dataEntries.arrRegistration[0].chkSecurityQuestion) {
                    var compulsoryQuantity = dataEntries.arrRegistration[0].txtSecurityQuestionQuantity;

                    if (compulsoryQuantity == "" || parseInt(compulsoryQuantity) < 1) {
                        btnSave.enable();
                        ShowErrorMessage(Ext.LabelKey.Admin_Registration_Verification_Expired_Day_IsNull);
                        return;
                    }
                }

                showMessageBox();
                Accela.ACA.Web.WebService.AdminConfigureService.SaveRegistrationSettingInfo(dataEntries, CallBack);
            }
            else if (Ext.Const.OpenedId.indexOf('ModuleSettings') != -1) {

                var dataEntries1 = pageInspectionItems.GetPageChangedItemsByPageId('inspection');
                var dataEntries2 = pageAppStatusItems.GetPageChangedItemsByPageId('appstatus');

                var data1 = dataEntries1.arrInspection[0] == null ? null : dataEntries1;
                var data2 = dataEntries2.arrAppStatus[0] == null ? null : dataEntries2;
                //save Amendment Cap Types.
                var ifrId = Ext.getCmp("tabs").activeTab.body.dom.firstChild.id;
                if (Ext.isIE) {
                    eval(ifrId + '.SaveAmendmentType();');
                    saveResult =   eval(ifrId + '.SaveCapTypeFilter(0);');
                    eval(ifrId + '.SaveRoleData();');
                }
                else {
                    var iframeObj = document.getElementById(ifrId);
                    iframeObj.contentWindow.SaveAmendmentType();
                    saveResult =  iframeObj.contentWindow.SaveCapTypeFilter(0);
                    iframeObj.contentWindow.SaveRoleData();
                }
                ExcuteMethod('SaveReportRoles()');
                ExcuteMethod('SaveCapTypeRoles(false)');
                ExcuteMethod('SaveRecordDetailSectionRoles(false)');
                ExcuteMethod('SaveInsActionPermissionSettings(false)');
            
                if(saveResult != -1){    
                    showMessageBox();
                    removeChangedItem4CurrentPage();
                    
                    Accela.ACA.Web.WebService.AdminConfigureService.SaveInspectionSettingInfo(Ext.Const.ModuleName, data1, data2, CallBack);
                }
            }
            else if (Ext.Const.OpenedId.indexOf('PageFlowConfiguration') != -1) {
                var ifrId = Ext.getCmp("tabs").activeTab.body.dom.firstChild.id;
                var isShowMsgBox;
                if (Ext.isIE) {
                    isShowMsgBox = eval(ifrId + '.checkPageFlow();');
                    if (isShowMsgBox) {
                        showMessageBox();
                    }
                    var saveOK = eval(ifrId + '.SavePageFlowGroup();');
                    if (!saveOK) {
                        btnSave.enable();
                        return false;
                    }
                } else {
                    var iframeObj = document.getElementById(ifrId);
                    isShowMsgBox = iframeObj.contentWindow.checkPageFlow();
                    if (isShowMsgBox) {
                        showMessageBox();
                    }
                    var saveOK = iframeObj.contentWindow.SavePageFlowGroup();
                    if (!saveOK) {
                        btnSave.enable();
                        return false;
                    }
                }
            }
            else if (Ext.Const.OpenedId.indexOf('AuthorizedServiceSettings') != -1) {
                ExcuteMethod('SaveServiceSetting()');
            }
            else if (Ext.Const.OpenedId.indexOf('HostedAgencySettings') != -1) {
                ExcuteMethod('ServiceGroup.prototype.SaveServiceGroup()');
            } else if (Ext.Const.OpenedId.indexOf('ColorTheme') != -1) {
                var changeditem = colorThemeChangeItems.getChangedItems();
                Accela.ACA.Web.WebService.AdminConfigureService.SaveCustomThemeInfo(changeditem, CallBack);
            }
            else {
                var dataEntries = changeItems.GetChangedItemsByPageId(Ext.Const.CurrId);

                if (typeof (dataEntries.Items[1]) != 'undefined' && typeof (dataEntries.Items[4]) != 'undefined') {
                    /* when change available fields properties have create a changeItem with change type is "input".this item include section instruction label.
                     * when change section instruction label have create a changeItem with change type is "section" 
                     * save data to DB "input" type item instruction label override "section" type instruction label.
                     */
                    if (dataEntries.Items[4][0] != null
                        && typeof (dataEntries.Items[1][0]) != 'undefined'
                        && dataEntries.Items[4][0].LabelKey == dataEntries.Items[1][0].LabelKey
                        && typeof (dataEntries.Items[1][0].SubLabel) != 'undefined') {
                        dataEntries.Items[4][0].SubLabel = dataEntries.Items[1][0].SubLabel;
                    }
                }
                var changedComponents = dataEntries.Items[dataEntries.ItemType.arrCustomComponent];

                if (changedComponents == null || changedComponents.length < 1) {
                    SaveDataEntries(dataEntries);
                }
                else {
                    var elementId = changedComponents[0]["ElementID"];
                    Accela.ACA.Web.WebService.AdminConfigureService.GetCustomComponentByElementId(elementId, function (json) {
                        var existsComponents = eval(json);

                        if (validCustomComponentName(changedComponents, existsComponents)) {
                            SaveDataEntries(dataEntries);
                        }
                    });
                }
            }
        }  
    }
}

function SaveDataEntries(dataEntries) {
    var arrFilter = dataEntries.Items[dataEntries.ItemType.arrFilter];
    var arrConfigureButton = dataEntries.Items[dataEntries.ItemType.arrConfigureButton];
    var arrReports = dataEntries.Items[dataEntries.ItemType.arrReports];
    var arrCustomComponent = dataEntries.Items[dataEntries.ItemType.arrCustomComponent];

    SaveFilters(arrFilter);
    SaveConfigureButtonUrl(arrConfigureButton);
    SaveReports(arrReports);
    SaveAutoFill(dataEntries);
    SaveExpandedSectionTitle(dataEntries);
    SaveCustomComponentConfig(arrCustomComponent);

    showMessageBox();
    if (Ext.Const.CurrId == Ext.Const.OpenedId) {
        isCloseOtherTab = false;
    }
    Accela.ACA.Web.WebService.AdminConfigureService.SaveAdminConfigurationData(dataEntries, CallBack); 
}

function GetOldValueBeforeValueChange(obj) {
    switch (obj.tagName) {
        case 'SELECT':
            if (typeof (obj.OldValue) == 'undefined' || obj.OldValue == null) {
                obj.OldValue = obj.selectedIndex;
            }
            break;
    }
}

function ChangedItemsValidateBeforePostBack(obj, win) {
    if (obj && typeof (obj.OldValue) != 'undefined' && obj.OldValue != null) {
        var currTab = Ext.getCmp(Ext.Const.OpenedId);

        if (currTab.title.indexOf('*') > 0) {
            var currTabTxt = currTab.title.replace(' *', '');
            if (confirm(Ext.LabelKey.Admin_Frame_SaveAlert + ' ' + currTabTxt + '?')) {
                SaveHandle(currTab.id);
            }
            else {
                //switch to the old value
                switch (obj.tagName) {
                    case 'SELECT':
                        obj.selectedIndex = obj.OldValue;
                        break;
                }

                return false;
            }
        }
    }
    obj.OldValue = null;
    win.setTimeout('__doPostBack(\'' + obj.name + '\',\'\')', 0);
}

function SaveCustomComponentConfig(arrCustomComponent) {
    if (arrCustomComponent == null || arrCustomComponent.length < 1) {
        return;
    }

    Accela.ACA.Web.WebService.AdminConfigureService.SaveCustomComponentConfig(arrCustomComponent);
}

function validCustomComponentName(changedComponents, existsComponents) {
    var mergeComponentNames = new Array();    

    for (var i = 0; i < existsComponents.length; i++) {
        var componentName = existsComponents[i]["ComponentName"];

        for (var j = 0; j < changedComponents.length; j++) {
            if (existsComponents[i]["ResID"] == changedComponents[j]["ResID"]) {
                componentName = changedComponents[j]["ComponentName"];
                break;
            }
        }

        mergeComponentNames[mergeComponentNames.length] = componentName;
    }

    if (mergeComponentNames.containsDuplicate()) {
        ShowErrorMessage("The customize component's name has already exists.");
        return false;
    }

    return true;
}