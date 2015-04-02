/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AmendmentButton.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: menu.js 72643 2009-07-28 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,       &lt;Who&gt;,            &lt;What&gt;  
 * </pre>
 */
    
var buttonSettingDic = new Dictionary();

var oldNodeID;
var combinedButtonNames ={
    CREAT_AMENDMENT: "CreateAmendment", 
    CREAT_DOCUMENET_DELETE: "DeleteDocument" 
};

var currentButtonSettingModel;

function RestoreAmendmentSelectedCaps() {
    Accela.ACA.Web.WebService.AdminConfigureService.GetCapTypeListByModule(moduleName, document.selectedButton, InitAssociatedCaps);
    buttonSettingDic.clear();
    window.parent.RemoveMark(false, 'combinButton');
    document.changed = false;
}

function InitAmendMentFilteName(filterNames) {
    var option = null;
    $("#ddlFilterName")[0].options.add(new Option("--Select a Record type filter--", ''));
    for (var i = 0; i < filterNames.length; i++) {
        if (filterNames[i] != '') {
            option = new Option(filterNames[i][0], filterNames[i][0]);
            $("#ddlFilterName")[0].options.add(option);
        }
    }
}

function SetAmendmentFilterValue(filterName) {
    $("#ddlFilterName")[0].options.add(new Option(filterName, filterName));
}

function RemoveAmendmentFilter(filterName) {
    for (var i = 0; i < $("#ddlFilterName")[0].options.length; i++) {
        if ($("#ddlFilterName")[0].options[i].value == filterName) {
            $("#ddlFilterName")[0].options.remove(i);
            break;
        }
    }
}

function ChangeButtonSetting() {
    oldNodeID = "";
    $('#divFilterName').hide();
    if (!document.changed) {
        GetAssociatedCapTypeList();
    } else {
        if (confirmMsg(Ext.LabelKey.Admin_CreateAmendment_Message_btnCancel)) {
            buttonSettingDic.remove(document.selectedButton);
            GetAssociatedCapTypeList();

            window.parent.RemoveMark(false, 'combinButton');
            document.changed = false;
        } else {
            document.getElementById('ddlAmendmentButtonName').value = document.selectedButton;
        }
    }
}

function GetAssociatedCapTypeList() {
    var selectedValue = document.getElementById("ddlAmendmentButtonName").value;

    // Show corresponding instruction when user change drop down
    var instruction = Ext.LabelKey.Admin_Global_Setting_Label_CombineHead;

    if (selectedValue == combinedButtonNames.CREAT_DOCUMENET_DELETE) {
        instruction = Ext.LabelKey.Admin_Global_Setting_Label_CombineHead_DeleteDocument;
    }

    document.getElementById('lblCombineHead').innerHTML = instruction;

    if (selectedValue == "") {
        Ext.get('divAmendment').dom.style.display = 'none';
        return;
    }

    document.selectedButton = selectedValue;
    Ext.get('divAmendment').dom.style.display = 'block';

    if (buttonSettingDic.containsKey(selectedValue)) {
        LoadCapTypes();
    } else {
        Accela.ACA.Web.WebService.AdminConfigureService.GetCapTypeListByModule(moduleName, selectedValue, InitAssociatedCaps);
    }
}

function InitAssociatedCaps(response) {
    $('#divFilterName').hide();
    var caps = eval("(" + response + ")");

    var buttonSettingModel = new ButtonSettingModel();

    buttonSettingModel.availableCapTypes = caps.availableCaps;
    buttonSettingModel.selectedCapTypes = caps.selectedCaps;

    buttonSettingDic.add(document.selectedButton, buttonSettingModel);

    LoadCapTypes();
}

function LoadCapTypes() {
    currentButtonSettingModel = buttonSettingDic.tryGetValue(document.selectedButton);
    var avaliableCapTypes = currentButtonSettingModel.availableCapTypes;
    var selectedCapTypes = currentButtonSettingModel.selectedCapTypes;

    CreateAvailableCapTypeTree(avaliableCapTypes);
    CreateSelectedCapTypeTree(selectedCapTypes);
    ClearStatusTree();

    Ext.get('capAssociation').dom.style.display = 'block';

    Ext.getCmp('btnOKAmendment').disable();
    Ext.getCmp('btnCancelAmendment').disable();
}

function CreateAvailableCapTypeTree(availableCapTyps) 
{
    if (availableCapTyps != null && availableCapTyps.length > 0) {
        var availableTree = Ext.getCmp('pnlCapAmendmentAvailable');
        var selectedTree = Ext.getCmp('pnlCapAmendmentSelected');

        if (availableTree.root.childNodes.length == 0) {
            availableTree.createTree(availableCapTyps, '');
            isFirstLoad = true;

            return;
        }

        isFirstLoad = false;

        // Append the previous selected CAP types to the avaliable CAP type list.
        // So the available CAP type list contains all CAP types.
        if (selectedTree.root.childNodes.length > 0) {
            availableTree.root.beginUpdate();
            selectedTree.root.beginUpdate();

            while (selectedTree.root.childNodes.length > 0) {
                availableTree.root.appendChild(selectedTree.root.childNodes[0].remove());
            }

            selectedTree.root.endUpdate();
            availableTree.root.endUpdate();
        }
    }
}

function CreateSelectedCapTypeTree(selectedCapTypes) {
    var selectedTree = Ext.getCmp('pnlCapAmendmentSelected');
    var availableTree = Ext.getCmp('pnlCapAmendmentAvailable');
    selectedTree.clear();

    if (selectedCapTypes != null && selectedCapTypes.length > 0) {
        if (isFirstLoad) {
            selectedTree.createTree(selectedCapTypes, '');

            return;
        }

        availableTree.root.beginUpdate();
        selectedTree.root.beginUpdate();

        for (var i = 0; i < selectedCapTypes.length; i++) {
            for (var j = 0; j < availableTree.root.childNodes.length; j++) {
                if (selectedCapTypes[i].text == availableTree.root.childNodes[j].text) {
                    var selectedNode = availableTree.root.childNodes[j];

                    // remove Selected Cap Type from Available CAP Type Tree
                    availableTree.root.removeChild(selectedNode);

                    // append Select Cap Type to Selected CAP Type Tree  
                    selectedTree.root.appendChild(selectedNode);
                    break;
                }
            }
        }
        availableTree.root.endUpdate();
        selectedTree.root.endUpdate();
    }
}

function InitAmendmentCapAssociation() {
    var btnAddAmendmentCap = new Ext.Button({
        id: 'btnAddAmendmentCap',
        disabled: true,
        minWidth: 80,
        text: Ext.LabelKey.Admin_CreateAmendment_btnAddCap,
        renderTo: 'addCap'
    });
    var btnRemoveAmendment = new Ext.Button({
        disabled: true,
        minWidth: 80,
        text: Ext.LabelKey.Admin_CreateAmendment_btnRemoveCap,
        renderTo: 'removeCap'
    });
    var btnOKAmendment = new Ext.Button({
        id: 'btnOKAmendment',
        disabled: true,
        minWidth: 80,
        text: Ext.LabelKey.Admin_CreateAmendment_btnOK,
        handler: SaveAmendmentType,
        renderTo: 'OK'
    });

    var btnCancelAmendment = new Ext.Button({
        id: 'btnCancelAmendment',
        minWidth: 80,
        text: Ext.LabelKey.Admin_CreateAmendment_btnCancel,
        handler: function() {
            if (!document.changed) {
                return;
            }
            if (confirmMsg(Ext.LabelKey.Admin_CreateAmendment_Message_btnCancel)) {
                RestoreAmendmentSelectedCaps();
            }
        },
        renderTo: 'Cancel'
    });


    var divAmendment = $('#divAmendment');
    var panelWidth = divAmendment.width() * 0.3;

    var capAmendmentAvailable = new Ext.tree.CapPanel({
        id: 'pnlCapAmendmentAvailable',
        title: Ext.LabelKey.Admin_CreateAmendment_pnlCapAvailableTitle,
        animate: true,
        style: 'padding:10px 10px 20px 20px',
        height: 300,
        width: panelWidth,
        lines: false,
        root: new Ext.tree.AsyncTreeNode({
            id: 'rootCapAmendmentAvailable',
            childNodes: { }
        }),
        rootVisible: false,
        enableDD: true,
        containerScroll: true,
        autoScroll: true,
        selModel: new Ext.tree.MultiSelectionModel()
    });
    capAmendmentAvailable.render('availableCap');

    var capAmendmentSelected = new Ext.tree.CapPanel({
        id: 'pnlCapAmendmentSelected',
        title: Ext.LabelKey.Admin_CreateAmendment_pnlCapSelectedTitle,
        animate: true,
        style: 'padding:10px 10px 20px 20px',
        height: 300,
        width: panelWidth,
        lines: false,
        root: new Ext.tree.AsyncTreeNode({
            id: 'rootCapAmendmentSelected',
            childNodes: { }
        }),
        rootVisible: false,
        enableDD: true,
        enableDrop: true,
        containerScroll: true,
        autoScroll: true,
        dropConfig: { allowContainerDrop: true },
        selModel: new Ext.tree.MultiSelectionModel()//FixedMultiSelectionModel()
    });
    capAmendmentSelected.render('selectedCap');

    var dropTree1 = new Ext.dd.DropTarget(capAmendmentAvailable.el.dom.childNodes[1], {
        ddGroup: 'TreeDD',
        notifyDrop: function(dd, e, data) {
            OnNodeDrop(dd, e, data, capAmendmentAvailable);
        }
    });
    var dropTree2 = new Ext.dd.DropTarget(capAmendmentSelected.el.dom.childNodes[1], {
        ddGroup: 'TreeDD',
        notifyDrop: function(dd, e, data) {
            return OnNodeDrop(dd, e, data, capAmendmentSelected);
        }
    });

    new Ext.tree.TreeSorter(capAmendmentAvailable, { folderSort: true });
    new Ext.tree.TreeSorter(capAmendmentSelected, { folderSort: true });

    capAmendmentAvailable.on('click', function(node, e) {
        btnAddAmendmentCap.enable();
    });

    capAmendmentAvailable.on('remove', function() {
        if (capAmendmentAvailable.getRootNode().childNodes.length < 1) {
            btnAddAmendmentCap.disable();
        }
    });

    capAmendmentSelected.on('click', function(node, e) {
        if (oldNodeID == node.id) {
            return false;
        }

        oldNodeID = node.id;
        $('#divFilterName').hide();
        btnRemoveAmendment.enable();
        GetAppStatus();
    });

    capAmendmentSelected.on('append', function() {
        btnAddAmendmentCap.disable();
    });

    capAmendmentSelected.on('remove', function() {
        if (capAmendmentSelected.getRootNode().childNodes.length < 1) {
            btnRemoveAmendment.disable();
        }
    });

    btnRemoveAmendment.on('click', function(e) {
        var selectedNodes = capAmendmentSelected.getSelectionModel().getSelectedNodes();
        var availableRoot = capAmendmentAvailable.getRootNode();
        var selectedRoot = capAmendmentSelected.getRootNode();

        RemoveCapTypeAndStatuses(selectedNodes);

        while (selectedNodes.length > 0) {
            availableRoot.appendChild(selectedNodes[0].remove());
        }

        btnRemoveAmendment.disable();
        document.changed = true;
        CheckNewGroupCondition();
    });

    btnAddAmendmentCap.on('click', function() {
        var selectedNodes = capAmendmentAvailable.getSelectionModel().getSelectedNodes();
        var selectedRoot = capAmendmentSelected.getRootNode();

        AddCapTypeAndStatuses(selectedNodes);
        AddCapAssociation(selectedRoot, selectedNodes);

        document.changed = true;
        CheckNewGroupCondition();
    });

    var root = new Ext.tree.TreeNode();
    var statusTree = CreateStatusTree(root, false);
}

function SaveAmendmentType() {
    if (!document.changed) {
        return;
    }
    var selectedCapTypeValues = new Array();
    var appStatuses = currentButtonSettingModel.appStatuses;
    var selectedCapTypes = currentButtonSettingModel.selectedCapTypes;

    for (var i = 0; i < selectedCapTypes.length; i++) {
        selectedCapTypeValues.push(selectedCapTypes[i].key);
    }

    Accela.ACA.Web.WebService.AdminConfigureService.UpdateButtonSetting4CapType(
        selectedCapTypeValues,
        appStatuses,
        document.selectedButton,
        moduleName,
        updateAmendableCallBack);
}

function updateAmendableCallBack() {
    if (window.parent.length != 0) {
        window.parent.RemoveMark(false, 'combinButton');
    }
    document.changed = false;

    Ext.getCmp('btnOKAmendment').disable();
    Ext.getCmp('btnCancelAmendment').disable();

}

function AddCapAssociation(root, selectedNodes) {
    var confirmed = true;

    var i = 0;
    while (selectedNodes.length > 0 && selectedNodes[i] != null) {
        root.appendChild(selectedNodes[i].remove());
    }

    CheckNewGroupCondition();

    if (selectedNodes.length < 1) {
        Ext.getCmp('btnAddAmendmentCap').disable();
    }

    return confirmed;
}

function OnNodeDrop(dd, e, data, targetTree) {
    if (dd.tree == targetTree) {
        e.cancel = true;
        return false;
    }

    //To resolve the ExtJs bug, the notifyDrop is called twice
    if (!dd.DDM.dragCurrent.dragging) {
        if (e.target.lastChild && e.target.lastChild.nodeName == 'UL') {
            return false;
        }
    }

    // Stores the dragged CAP type and corresponding Application statuses to
    // the ButtonSettingModel
    var hasContainedCapType = currentButtonSettingModel.hasContainCapType(data.node.attributes.key);

    if (targetTree.id == 'pnlCapAmendmentSelected' && !hasContainedCapType) {
        AddCapTypeAndStatuses(new Array(data.node));
    } else if (targetTree.id == 'pnlCapAmendmentAvailable' && hasContainedCapType) {
        RemoveCapTypeAndStatuses(new Array(data.node));
    }

    var attributes = data.node.attributes;
    targetTree.getRootNode().appendChild(dd.tree.getRootNode().removeChild(data.node));
    document.changed = true;
    CheckNewGroupCondition();
    return true;
}

function CheckNewGroupCondition(){
    var btnOKAmendment = Ext.getCmp('btnOKAmendment');
    var btnCancelAmendment = Ext.getCmp('btnCancelAmendment');
    if(document.changed){
        btnOKAmendment.enable();
        btnCancelAmendment.enable();
        window.parent.ModifyMark('combinButton');
    }
    else{
        btnOKAmendment.disable();
        btnCancelAmendment.disable();
    }
}
/*
 * Stores the selected CAP type(s) and corresponding Application statuses to
   the ButtonSettingModel when user clicks Add button or drags CAP type.
 */
function AddCapTypeAndStatuses(selectedCapTypes) {
   if (selectedCapTypes == null || selectedCapTypes.length == 0) {
        return;
   }
   
   // add the cap types to selected cap type list
   currentButtonSettingModel.addCapTypes(selectedCapTypes); 
   
   // get the Application statuses of the cap types  
   var capTypeValues = new Array(selectedCapTypes.length);
    
   for (var i = 0; i < selectedCapTypes.length; i++) {
       capTypeValues[i] = selectedCapTypes[i].attributes.key;
   }

   Accela.ACA.Web.WebService.AdminConfigureService.GetApplicationStatus(document.selectedButton, capTypeValues, moduleName, AddAppStatuses);
}

/*
 * Stores related Application status of the added cap types to 
   the corresponding ButtonSettingModel
 */
function AddAppStatuses(appStatusList) {
    if (appStatusList == null) {
        return;
    }

    for (var i = 0; i < appStatusList.length; i++) {
        // when add cap types, all statuses should be marked as default.
        // entityKey1- app status group, entityKey2- app status
        if (!String.IsNullOrEmpty(appStatusList[i].entityKey1) &&
            !String.IsNullOrEmpty(appStatusList[i].entityKey2)) {
            appStatusList[i].entityPermission = '1';
        }
    }

    currentButtonSettingModel.addAppStatuses(appStatusList);
}

/*
 *  When user clicks Remove button or drag a selected CAP type:
    1.Remove the selected CAP type(s) and the corresponding Application statuses
    from the ButtonSettingModel; 
    2.Unmark the changed icon for CAP types;
    3.Empty the Status tree panel
 */
function RemoveCapTypeAndStatuses(selectedCapTypes) {
    if (selectedCapTypes != null) {
        currentButtonSettingModel.removeCapTypes(selectedCapTypes);
        currentButtonSettingModel.removeAppStatusByCapTypes(selectedCapTypes);

        $('#divFilterName').hide();
        MarkCapType(false, selectedCapTypes);
        ClearStatusTree();
    }
}

/*
 *  Get corresponding Application Status for the clicked CAP type 	
 */
function GetAppStatus() {
    var selectedNodes = GetSelectedCapNodes();

    if (selectedNodes == null || selectedNodes.length > 1) {
        // if selected multiple CAP type, empty the Status tree and show a message
        var msg = Ext.LabelKey.Admin_CombineButton_Message_SelectOneCapType;
        ClearStatusTree(msg);
    } else {
        var capTypeValue = selectedNodes[0].attributes.key; // cap type key

        // 1. get Application status from stored dictionary
        // 2. get Application status from web service
        var appStatusArray = currentButtonSettingModel.getAppStatusByCapType(capTypeValue);

        if (appStatusArray != null && appStatusArray.length > 0) {
            LoadAppStatus(appStatusArray, true);
        } else {
            var capTypeValues = new Array(capTypeValue);
            Accela.ACA.Web.WebService.AdminConfigureService.GetApplicationStatus(document.selectedButton, capTypeValues, moduleName, LoadAppStatus);
        }
    }
}

/*
 *  Load the Application status to the status panel
    entityKey1- application status group, 
    entityKey2- app status, 
    entityKey3- resStatus.
    entityPermission- checked("1") or not 
 */
function LoadAppStatus(appStatusArray, isExist) {
    if (appStatusArray == null ||
        appStatusArray.length == 0 ||
        String.IsNullOrEmpty(appStatusArray[0].entityKey1) ||
        String.IsNullOrEmpty(appStatusArray[0].entityKey2)) {

        // Display a message when the selected CAP type has no Application status group
        var msg = Ext.LabelKey.Admin_CombineButton_Message_NoApplicationStatus;
        ClearStatusTree(msg);
        return;
    }

    // create root node - Application status group code
    var root = new Ext.tree.TreeNode({
        id: "root",
        text: JsonEncode(appStatusArray[0].entityKey1),
        iconCls: "x-tree-nodes-setting",
        leaf: false
    });

    // create child nodes - Application statuses
    for (var i = 0; i < appStatusArray.length; i++) {
        var statusName = appStatusArray[i].entityKey3 == null || appStatusArray[i].entityKey3 == '' ?
            appStatusArray[i].entityKey2 : appStatusArray[i].entityKey3;
        var checked = appStatusArray[i].entityPermission == '1';
        var recordTypeValue = currentButtonSettingModel.getCapTypeKeyByStatus(appStatusArray[i]);

        var node = new Ext.tree.TreeNode({
            key: JsonEncode(appStatusArray[i].entityKey2),
            text: JsonEncode(statusName),
            iconCls: "x-tree-nodes-setting",
            checked: checked,
            leaf: true,
            filterName: JsonEncode(appStatusArray[i].entityKey4),
            recordType: JsonEncode(recordTypeValue)
        });

        root.appendChild(node);
    }

    var hasSelectedChild = false;

    var childNodes = root.childNodes;

    for (var i = 0; i < childNodes.length; i++) {
        if (childNodes[i].attributes.checked) {
            hasSelectedChild = true;
            break;
        }
    }

    // if has one Application status is marked, mark the group code;
    // otherwise, unmark the group code.
    root.attributes.checked = hasSelectedChild;

    var statusTree = CreateStatusTree(root, true);

    statusTree.root.expand();
    statusTree.on('checkchange', statusNode_cicked);

    if (document.getElementById("ddlAmendmentButtonName").value == combinedButtonNames.CREAT_AMENDMENT) {
        statusTree.on('click', function(node, e) {

            if (node.isLeaf() && $("#ddlFilterName")[0].options.length > 1) {
                var filterName;
                var recordTypeValue = node.attributes.recordType; // group/type/subtype/category
                var appStatusArray = currentButtonSettingModel.getAppStatusByCapType(recordTypeValue);

                if (appStatusArray != null && appStatusArray.length > 0) {
                    for (var i = 0; i < appStatusArray.length; i++) {
                        if (JsonEncode(appStatusArray[i].entityKey2) == node.attributes.key) {
                            filterName = appStatusArray[i].entityKey4 == null ? "" : appStatusArray[i].entityKey4.toUpperCase();
                            break;
                        }
                    }
                }

                var isFindFilterName = false;
                for (var i = 0; i < $("#ddlFilterName")[0].options.length; i++) {
                    if ($("#ddlFilterName")[0].options[i].value == filterName) {
                        $("#ddlFilterName")[0].options[i].selected = true;
                        isFindFilterName = true;
                        break;
                    }
                }

                if (!isFindFilterName) {
                    $("#ddlFilterName")[0].options[0].selected = true;
                }

                $('#divFilterName').show();
            } else {
                $('#divFilterName').hide();
            }
        });
    }

    // Add the Application status of the clicked cap type to dictionary
    if (!isExist) {
        currentButtonSettingModel.addAppStatuses(appStatusArray);
    }
}

function UpdateFilteName(selectedFilterName) {

    var appstatusSelected = Ext.getCmp("pnlStatus");
    var selectedNode = appstatusSelected.getSelectionModel().getSelectedNode();

    if (selectedNode == null) {
        return;
    }

    document.changed = true;
    CheckNewGroupCondition();
    var recordTypeValue = selectedNode.attributes.recordType;
    var appStatusArray = currentButtonSettingModel.getAppStatusByCapType(recordTypeValue);

    if (appStatusArray != null && appStatusArray.length > 0) {
        for (var i = 0; i < appStatusArray.length; i++) {
            if (JsonEncode(appStatusArray[i].entityKey2) == selectedNode.attributes.key) {
                appStatusArray[i].entityKey4 = selectedFilterName.value.toUpperCase();
                break;
            }
        }
    }
}

/*
 *  Create a tree for Application status
 */
function CreateStatusTree(root, isRootVisible) {
    var width = $('#divAmendment').width() * 0.25;
    $('#tdStatus').width(width);

    var divTree = document.getElementById("divStatusTree");
    divTree.innerHTML = "";

    var tree = new Ext.tree.TreePanel({
        id: 'pnlStatus',
        height: 300,
        style: 'padding:10px 10px 20px 20px',
        width: width,
        title: Ext.LabelKey.Admin_CombineButton_pnlApplicationStatusTitle,
        autoScroll: true,
        containerScroll: true,
        animate: true,
        enableDD: false,
        border: true,
        root: root,
        rootVisible: isRootVisible,
        renderTo: 'divStatusTree'
    });

    tree.setRootNode(root);

    return tree;
}

function ClearStatusTree(content) {
    var width = $('#divAmendment').width() * 0.25;
    $('#tdStatus').width(width);

    var divTree = document.getElementById("divStatusTree");
    divTree.innerHTML = "";

    if (content) {
        content = '<a href="javascript:void(0);" style="text-decoration:none;color:#666666; cursor:default">' + content + '</a>';
    }

    var panel = new Ext.Panel({
        id: 'pnlStatus',
        height: 300,
        width: width,
        style: 'padding:10px 10px 20px 20px',
        title: Ext.LabelKey.Admin_CombineButton_pnlApplicationStatusTitle,
        html: content,
        bodyStyle: 'padding: 5px 3px 4px 5px',
        renderTo: 'divStatusTree'
    });
}

function GetSelectedCapNodes() {
    var capAmendmentSelected = Ext.getCmp("pnlCapAmendmentSelected");
    var selectedNodes = capAmendmentSelected.getSelectionModel().getSelectedNodes();

    return selectedNodes;
}

function statusNode_cicked(node, checked) {
    if (node.isLeaf()) {
        hasSelectedChild = false;
        var childNodes = node.parentNode.childNodes;

        for (var i = 0; i < childNodes.length; i++) {
            if (childNodes[i].attributes.checked) {
                hasSelectedChild = true;
                break;
            }
        }

        // if has one Application status is marked, mark the group code.
        // if all Application stause if unmarked, unmark the group code.
        node.parentNode.ui.toggleCheck(hasSelectedChild);
        node.parentNode.attributes.checked = hasSelectedChild;
    } else {
        node.eachChild(function(child) {
            child.ui.toggleCheck(checked);
            child.attributes.checked = checked;

        });
    }

    document.changed = true;
    var selectedNodes = GetSelectedCapNodes();
    MarkCapType(true, selectedNodes);
    UpdateAppstatus(node);

    CheckNewGroupCondition();
}

/*
 *  Add a icon before the corresponding CAP type when user mark/unmark Application status
    or remove the icon when user remove the CAP type from selected CAP type panel.
 */
function MarkCapType(isMark, selectedCapTypeNodes) {
    if (selectedCapTypeNodes != null) {
        for (var i = 0; i < selectedCapTypeNodes.length; i++) {
            var nodeIcon = selectedCapTypeNodes[i].ui.ecNode.src;
            var newIcon;
            var defaultIcon = 'images/default/s.gif';
            var changedIcon = 'images/default/grid/dirty.gif';

            if (isMark) {
                // change cap type icon if its Application status has been changed.
                newIcon = nodeIcon.replace(defaultIcon, changedIcon);

            } else {
                // restore cap type icon for the removed cap types
                newIcon = nodeIcon.replace(changedIcon, defaultIcon);
            }

            selectedCapTypeNodes[i].ui.ecNode.src = newIcon;
        }
    }
}

/*
 *  Update Application statuses when user clicks Application status group code or status
    entityKey2- app status. it is used as key of status nodes.
    entityPermission- checked("1") or not 
 */
function UpdateAppstatus(node) {
    var selectedNodes = GetSelectedCapNodes();

    if (selectedNodes == null && selectedNodes.length == 0) {
        return;
    }

    var capTypeValue = selectedNodes[0].attributes.key; // group/type/subtype/category
    var appStatusArray = currentButtonSettingModel.getAppStatusByCapType(capTypeValue);

    if (appStatusArray != null && appStatusArray.length > 0) {
        if (node.isLeaf()) {
            for (var i = 0; i < appStatusArray.length; i++) {
                if (JsonEncode(appStatusArray[i].entityKey2) == node.attributes.key) {
                    appStatusArray[i].entityPermission = node.attributes.checked ? '1' : '0';
                    break;
                }
            }
        } else {
            for (var i = 0; i < appStatusArray.length; i++) {
                appStatusArray[i].entityPermission = node.attributes.checked ? '1' : '0';
            }
        }
    }
}
