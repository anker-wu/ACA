 /**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ServiceGroup.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ServiceGroup.js 72643 2013-09-17 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * 09/17/2013     		Canon Wu				Initial.  
 * </pre>
 */


Ext.onReady(function () {
    Ext.QuickTips.init();
    Ext.state.Manager.setProvider(new Ext.state.CookieProvider());

    document.GroupChanged = false;
    var serviceGroup = new ServiceGroup();
    serviceGroup.Init();
});

function ServiceGroupItem() {
    this.ServiceList = new Array();
    this.GroupInfo = new Object();
}

function ServiceGroup() {
    this.ServiceList = new Array();
    this.GroupInfo = new Object();

    this.Init = function () {
        var title = new Ext.form.Label({
            cls: 'ACA_New_Title_Label font12px',
            text: Ext.LabelKey.Admin_ServiceGroup_Label_title
        });

        title.render('divServiceGroupTitle');

        Accela.ACA.Web.WebService.AdminConfigureService.GetAllServiceGroup(this.InitServiceGroupCallback); //Get all available service group.
    };

    this.InitServiceGroupCallback = function (groupNames) {
        groupNames = eval('(' + groupNames + ')');
        ServiceGroup.prototype.InitGroupHeader(groupNames);
        ServiceGroup.prototype.InitGroupServiceAssociation();

        if (GLOBAL_IS_MASTER_LANGUAGE) {
            Ext.getCmp('resServiceGroupNamePanel').hide();
        } else {
            Ext.getCmp('txtGroupOrder').disable();
            Ext.getCmp('txtServiceGroupName').disable();
            Ext.getCmp("btnCreateNewGroup").disable();
            Ext.getCmp('btnGroupDelete').disable();
        }

        Ext.get('groupServiceAssociation').dom.style.display = 'none';
    };
};

ServiceGroup.prototype.InitGroupHeader = function(groupNames) {
    var groupStore = new Ext.data.SimpleStore({
        fields: ['serviceGroupSeqNbr', 'groupName', 'resGroupName', 'sortOrder'],
        data: groupNames
    });

    var header = new Ext.FormPanel({
        header: false,
        cls: 'ACA_New_Label font11px',
        bodyStyle: 'padding:8px;background-color:#ECF5FE;margin-top:5px;border-color: #B6D2E5; border-width: 2px;border-style: solid;',
        id: 'pnlHeader',
        labelAlign: 'left',
        layout: 'column',
        border: false,
        labelSeparator: ' :',
        monitorResize: true,
        listeners: {
            'afterlayout': function () {
                ServiceGroup.prototype.AdjustServicePanel();
            }
        },
        items: [{
                columnWidth: 1,
                layout: 'form',
                bodyStyle: 'padding-bottom:8px;background-color:#ECF5FE;font-weight: bold;font-family: Arial, sans-serif;',
                border: false,
                items: [{
                    xtype: 'label',
                    text: Ext.LabelKey.Admin_ServiceGroup_Label_instruction
                }]
            }, {
                columnWidth: .45,
                layout: 'form',
                bodyStyle: 'padding-bottom:8px;background-color:#ECF5FE',
                border: false,
                items: [{
                    xtype: 'combo',
                    id: 'ddlServiceGroup',
                    store: groupStore,
                    displayField: "resGroupName",
                    valueField: "groupName",
                    mode: 'local',
                    emptyText: Ext.LabelKey.Admin_ServiceGroup_Label_ddlemptytext,
                    editable: false,
                    triggerAction: 'all',
                    height: '100px',
                    hideLabel: true,
                    forceSelection: true,
                    anchor: '90%',
                    loadingText: 'Loading....'
                }]
            }, {
                columnWidth: .05,
                bodyStyle: 'padding-bottom:8px;background-color:#ECF5FE',
                layout: 'form',
                border: false,
                items: [{
                    xtype: 'label',
                    id: 'lblOr',
                    minWidth: 20,
                    text: Ext.LabelKey.Admin_ServiceGroup_Label_lblor
                }]
            },
            {
                columnWidth: .5,
                layout: 'form',
                bodyStyle: 'padding-bottom:8px;background-color:#ECF5FE',
                border: false,
                items: [{
                    xtype: 'button',
                    id: 'btnCreateNewGroup',
                    minWidth: 80,
                    text: Ext.LabelKey.Admin_ServiceGroup_Label_btncreatenew,
                    handler: function(e) {
                        if (!ServiceGroup.prototype.IsChangeGroupSetting() && !document.GroupChanged) {
                            ServiceGroup.prototype.CreateAvailableTree();
                            ServiceGroup.prototype.InitNewServiceGroupUI();
                            return;
                        }

                        if (confirmMsg(Ext.LabelKey.Admin_ServiceGroup_Msg_cancelgroup)) {
                            ServiceGroup.prototype.CreateAvailableTree();
                            ServiceGroup.prototype.InitNewServiceGroupUI();

                            window.parent.RemoveMark(false, 'serviceGroup');
                            document.GroupChanged = false;
                        }
                    }
                }]
            }]
    });

    header.render('divGroupHeader');
    header.doLayout();
    this.ReloadServiceGroup(groupNames);
};

ServiceGroup.prototype.ReloadServiceGroup = function (groupNames) {
    var select = $('#pnlHeader input[name="ddlServiceGroup"]');
    var selWidth = select.width();
    var divSelect = select.parent("div");
    var parentWidth = divSelect.width();

    divSelect.html('');
    var serviceGroupSelect = this.CreateSelect(divSelect);
    serviceGroupSelect.style.width = selWidth;
    divSelect.width(parentWidth);

    serviceGroupSelect.options.length = 0;
    serviceGroupSelect.options.add(new Option("--" + Ext.LabelKey.Admin_ServiceGroup_Label_ddlemptytext + "--", ""));

    for (var i = 0; i < groupNames.length; i++) {
        if (groupNames[i] != '') {
            ServiceGroup.prototype.AddGroupOption(groupNames[i].serviceGroupSeqNbr, groupNames[i].sortOrder, JsonDecode(groupNames[i].groupName), JsonDecode(groupNames[i].resGroupName));
        }
    }
};

ServiceGroup.prototype.CreateSelect = function(divPanel) {
    var selectType = document.createElement("select");
    selectType.setAttribute("id", "ddlServiceGroup");
    selectType.setAttribute("title", "Please select a Service Group");
    selectType.className = 'ACA_New_Label';
    selectType.style.width = '100%';
    selectType.style.height = '20px';
    selectType.onchange = this.TypeSelectIndexChanged;
    divPanel.append(selectType);
    return selectType;
};

ServiceGroup.prototype.AddGroupOption = function (serviceGroupSeqNbr, groupOrder, groupName, resGroupName) {

    var selectType = document.getElementById('ddlServiceGroup');
    var newOption = new Option();
    newOption.setAttribute("sortOrder", groupOrder);
    newOption.setAttribute("serviceGroupSeqNbr", serviceGroupSeqNbr);
    newOption.setAttribute("resGroupName", resGroupName);
    newOption.value = groupName;
    newOption.text = (resGroupName != "") ? resGroupName : groupName;
    selectType.options.add(newOption);
};

ServiceGroup.prototype.SetServiceGroupValue = function (value) {
    document.getElementById('ddlServiceGroup').value = value;
};

ServiceGroup.prototype.TypeSelectIndexChanged = function () {
    var selecedValue = document.getElementById('ddlServiceGroup').value.trim();

    if (selecedValue != null && selecedValue != "") {
        var btnOK = Ext.getCmp('btnGroupOK');

        if ((btnOK.disabled || !ServiceGroup.prototype.IsChangeGroupSetting()) && !document.GroupChanged) {
            ServiceGroup.prototype.SelectGroup(document.getElementById('ddlServiceGroup'));
            return;
        }

        if ((document.GroupChanged || !btnOK.disabled) && confirmMsg(Ext.LabelKey.Admin_ServiceGroup_Msg_cancelgroup)) {
            ServiceGroup.prototype.SelectGroup(document.getElementById('ddlServiceGroup'));
            window.parent.RemoveMark(false, 'serviceGroup');
            document.GroupChanged = false;
        }
    } else {
        Ext.getCmp('btnGroupDelete').disable();
        Ext.get('txtMode').dom.value = 'New';
        Ext.get('groupServiceAssociation').dom.style.display = 'none';
        Ext.getCmp('txtServiceGroupName').setValue('');
        Ext.getCmp('txtResServiceGroupName').setValue('');
        Ext.getCmp('txtGroupOrder').setValue('');
        Ext.getCmp('txtServiceGroupSeqNbr').setValue('');
        window.parent.RemoveMark(false, 'serviceGroup');
        document.GroupChanged = false;
    }
};

ServiceGroup.prototype.DeleteSuccess = function () {
    Ext.get('txtMode').dom.value = 'New';
    ServiceGroup.prototype.SetServiceGroupValue('');
    Ext.getCmp('btnGroupCancel').disable();
    Ext.getCmp('btnGroupOK').disable();
    Ext.getCmp('btnGroupDelete').disable();
    Ext.get('groupServiceAssociation').dom.style.display = 'none';
    Ext.getCmp('txtServiceGroupName').setValue('');
    Ext.getCmp('txtResServiceGroupName').setValue('');
    Ext.getCmp('txtGroupOrder').setValue('');
    Ext.getCmp('txtServiceGroupSeqNbr').setValue('');
    ServiceGroup.prototype.AdjustServicePanel();
    document.GroupChanged = false;
    window.parent.RemoveMark(false, 'serviceGroup');
};

ServiceGroup.prototype.InitGroupServiceAssociation = function () {
    var btnAddCap = new Ext.Button({
        id: 'btnAddGroupCap',
        disabled: true,
        minWidth: 80,
        text: Ext.LabelKey.Admin_ServiceGroup_Label_btnaddservice,
        renderTo: 'addGroupCap'
    });
    var btnRemove = new Ext.Button({
        disabled: true,
        minWidth: 80,
        text: Ext.LabelKey.Admin_ServiceGroup_Label_btnremoveservice,
        renderTo: 'removeGroupCap'
    });
    var btnOK = new Ext.Button({
        id: 'btnGroupOK',
        disabled: true,
        minWidth: 80,
        text: Ext.LabelKey.Admin_ServiceGroup_Label_btnok,
        handler: ServiceGroup.prototype.SaveServiceGroup,
        renderTo: 'btnGroupOK'
    });
    var btnCancel = new Ext.Button({
        id: 'btnGroupCancel',
        minWidth: 80,
        disabled: true,
        text: Ext.LabelKey.Admin_ServiceGroup_Label_btncancel,
        handler: function () {
            if (!ServiceGroup.prototype.IsChangeGroupSetting() && !document.GroupChanged) {
                return;
            }

            if (confirmMsg(Ext.LabelKey.Admin_ServiceGroup_Msg_cancelgroup)) {
                var serviceGroupSeqNbr = document.getElementById('txtServiceGroupSeqNbr').value.trim();
                Accela.ACA.Web.WebService.AdminConfigureService.GetServicesByGroupSeqNbr(serviceGroupSeqNbr, ServiceGroup.prototype.CancelServiceList);
            }
        },
        renderTo: 'btnGroupCancel'
    });

    var btnDelete = new Ext.Button({
        id: 'btnGroupDelete',
        disabled: true,
        minWidth: 80,
        text: Ext.LabelKey.Admin_ServiceGroup_Label_btndelete,
        handler: ServiceGroup.prototype.DeleteServiceGroup,
        renderTo: 'btnGroupDelete'
    });


    var formServiceGroupName = new Ext.form.FormPanel({
        labelAlign: 'Left',
        header: false,
        bodyStyle: 'padding-top:10px;margin-top:1px',
        renderTo: 'NewServiceGroupName',
        layout: 'column',
        labelWidth: 122,
        border: false,
        items: [{
            layout: 'form',
            columnWidth: .44,
            border: false,
            items: [{
                xtype: 'textfield',
                emptyText: Ext.LabelKey.Admin_ServiceGroup_Label_txtgroupnameemptytext,
                id: 'txtServiceGroupName',
                style: 'margin-top: 1px',
                fieldLabel: Ext.LabelKey.Admin_ServiceGroup_Label_servicegroupname,
                maxLength: 100,
                minLength: 1,
                width: 260,
                listeners: {
                    'blur': function () {
                        ServiceGroup.prototype.CheckGroupStatus();
                    }
                }
            }]
        }, {
            columnWidth: .03,
            bodyStyle: 'padding-bottom:8px;',
            layout: 'form',
            border: false,
            items: [{
                xtype: 'label',
                minWidth: 20
            }]
        }, {
            layout: 'form',
            columnWidth: .29,
            id: 'resServiceGroupNamePanel',
            border: false,
            items: [{
                xtype: 'textfield',
                allowBlank: true,
                emptyText: Ext.LabelKey.Admin_ServiceGroup_Label_txtgroupnameemptytext,
                style: 'margin-top: 1px',
                id: 'txtResServiceGroupName',
                hideLabel: true,
                maxLength: 100,
                width: 260,
                listeners: {
                    'blur': function () {
                        ServiceGroup.prototype.CheckGroupStatus();
                    }
                }
            }]
        }, {
            layout: 'form',
            labelWidth: 50,
            columnWidth: .24,
            border: false,
            items: [{
                xtype: 'numberfield',
                allowBlank: true,
                allowDecimals: false,
                allowNegative: false,
                fieldLabel: Ext.LabelKey.Admin_ServiceGroup_Label_txtgroupordertext,
                style: 'margin-top: 1px',
                id: 'txtGroupOrder',
                maxLength: 6,
                width: 50,
                listeners: {
                    'blur': function () {
                        ServiceGroup.prototype.CheckGroupStatus();
                    }
                }
            }]
        }, {
            columnWidth: 1,
            border: false,
            items: [{
                xtype: 'hidden',
                id: 'txtMode'
            }]
        }, {
            columnWidth: 1,
            border: false,
            items: [{
                xtype: 'hidden',
                id: 'txtServiceGroupSeqNbr'
            }]
        }]
    });

    var capAvailable = new Ext.tree.CapPanel({
        id: 'pnlGroupCapAvailable',
        title: Ext.LabelKey.Admin_ServiceGroup_Label_pnlavailabletitle,
        animate: true,
        style: 'padding:10px 10px 20px 20px',
        height: 300,
        lines: false,
        root: new Ext.tree.AsyncTreeNode({
            id: 'rootCapAvailable',
            childNodes: {}
        }),
        rootVisible: false,
        enableDD: true,
        containerScroll: true,
        autoScroll: true,
        selModel: new Ext.tree.MultiSelectionModel()
    });

    if (GLOBAL_IS_MASTER_LANGUAGE) {
        capAvailable.enableDD = true;
    } else {
        capAvailable.enableDD = false;
    }
    capAvailable.render('availableGroupCap');


    var capSelected = new Ext.tree.CapPanel({
        id: 'pnlGroupCapSelected',
        title: Ext.LabelKey.Admin_ServiceGroup_Label_pnlselectedtitle,
        animate: true,
        style: 'padding:10px 10px 20px 20px',
        height: 300,
        lines: false,
        root: new Ext.tree.AsyncTreeNode({
            id: 'rootCapSelected',
            childNodes: {}
        }),
        rootVisible: false,
        enableDD: true,
        enableDrop: true,
        containerScroll: true,
        autoScroll: true,
        dropConfig: { allowContainerDrop: true },
        selModel: new Ext.tree.MultiSelectionModel()//FixedMultiSelectionModel()
    });
    if (GLOBAL_IS_MASTER_LANGUAGE) {
        capSelected.enableDD = true;
    } else {
        capSelected.enableDD = false;
    }
    capSelected.render('selectedGroupCap');

    new Ext.tree.TreeSorter(capAvailable, { sortType: function (node) { return parseInt(node.attributes.sortOrder, 0); } });

    var dropTree1 = new Ext.dd.DropTarget(capAvailable.el.dom.childNodes[1], {
        ddGroup: 'TreeDD',
        notifyDrop: function (dd, e, data) {
            ServiceGroup.prototype.OnGroupNodeDropForAvailablePanel(dd, e, data, capAvailable);
        }
    });

    var dropTree2 = new Ext.dd.DropTarget(capSelected.el.dom.childNodes[1], {
        ddGroup: 'TreeDD',
        notifyDrop: function (dd, e, data) {
            return ServiceGroup.prototype.OnGroupNodeDropForSelectedPanel(dd, e, data, capSelected);
        }
    });


    capAvailable.on('click', function (node, e) {
        if (GLOBAL_IS_MASTER_LANGUAGE) {
            btnAddCap.enable();
        } else {
            btnAddCap.disable();
        }
    });

    capAvailable.on('remove', function () {
        if (capAvailable.getRootNode().childNodes.length < 1) {
            btnAddCap.disable();
        }
    });

    capSelected.on('click', function () {
        if (GLOBAL_IS_MASTER_LANGUAGE) {
            btnRemove.enable();
        } else {
            btnRemove.disable();
        }
    });

    capSelected.on('append', function () {
        btnAddCap.disable();
    });

    capSelected.on('remove', function () {
        btnRemove.disable();
    });

    btnRemove.on('click', function (e) {
        var selectedNodes = capSelected.getSelectionModel().getSelectedNodes();
        var availableRoot = capAvailable.getRootNode();
        var selectedRoot = capSelected.getRootNode();

        while (selectedNodes.length > 0) {
            var node = selectedNodes[0].remove();
            node.attributes.binded = false;

            availableRoot.appendChild(node);
        }
        capSelected.setNodeWidth(capSelected.id);
        capAvailable.setNodeWidth(capAvailable.id);

        btnRemove.disable();
        document.GroupChanged = true;
        ServiceGroup.prototype.CheckGroupStatus();
    });

    btnAddCap.on('click', function () {
        var selectedNodes = capAvailable.getSelectionModel().getSelectedNodes();
        var selectedRoot = capSelected.getRootNode();

        document.GroupChanged = true;
        ServiceGroup.prototype.AddGroupServiceAssociation(selectedRoot, selectedNodes);
        ServiceGroup.prototype.CheckGroupStatus();
        capAvailable.setNodeWidth(capAvailable.id);
        capSelected.setNodeWidth(capSelected.id);
    });
};

ServiceGroup.prototype.OnGroupNodeDropForAvailablePanel = function(dd, e, data, targetTree) {
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

    var node = dd.tree.getRootNode().removeChild(data.node);
    targetTree.getRootNode().appendChild(node);
    document.GroupChanged = true;
    this.CheckGroupStatus();
    targetTree.setNodeWidth(targetTree.id);

    return true;
};

ServiceGroup.prototype.OnGroupNodeDropForSelectedPanel = function (dd, e, data, targetTree) {

    //To resolve the ExtJs bug, the notifyDrop is called twice
    if (!dd.DDM.dragCurrent.dragging) {
        if (e.target.lastChild && e.target.lastChild.nodeName == 'UL') {
            return false;
        }
    }

    var preEle = dd.getDragData(e);
    var pNode = preEle ? preEle.node : undefined;
    if (pNode != undefined && data.node.id == pNode.id) {
        return false;
    }

    var node = dd.tree.getRootNode().removeChild(data.node);

    targetTree.getRootNode().insertBefore(node, pNode);
    var cs = targetTree.getRootNode().childNodes;
    for (var i = 0, len = cs.length; i < len; i++) {
        cs[i].render(true);
    }

    targetTree.updateHeader();

    document.GroupChanged = true;
    this.CheckGroupStatus();
    targetTree.setNodeWidth(targetTree.id);
    return true;
};

ServiceGroup.prototype.CancelServiceList = function (response) {
    var serviceList = eval('(' + response + ')');
    var mode = Ext.get('txtMode').dom.value;
    ServiceGroup.prototype.RefreshSelectedServiceGroupAssociation(serviceList, mode);

    Ext.getCmp('btnGroupCancel').disable();
    Ext.getCmp('btnGroupOK').disable();

    if (mode == 'New') {
        Ext.getCmp('txtServiceGroupName').setValue('');
        Ext.getCmp('txtResServiceGroupName').setValue('');
        Ext.getCmp('txtGroupOrder').setValue('');
        Ext.getCmp('txtServiceGroupSeqNbr').setValue('');
    } else {
        var $ddlServiceGroup = $('#ddlServiceGroup');
        var $selectedOption = $ddlServiceGroup.children('option:selected');

        $('#txtServiceGroupName').val($ddlServiceGroup.val());
        $('#txtResServiceGroupName').val($selectedOption.attr("resGroupName"));
        $('#txtGroupOrder').val($selectedOption.attr("sortOrder"));
    }

    window.parent.RemoveMark(false, 'serviceGroup');
    document.GroupChanged = false;
};

ServiceGroup.prototype.SelectGroup = function (groupOption) {
    var selOption = groupOption.options[groupOption.selectedIndex];

    var txtServiceGroupName = Ext.getCmp('txtServiceGroupName');
    txtServiceGroupName.setValue(selOption.value.trim());

    var txtGroupOrder = Ext.getCmp('txtGroupOrder');
    txtGroupOrder.setValue(selOption.getAttribute("sortOrder"));

    var txtServiceGroupSeqNbr = Ext.getCmp('txtServiceGroupSeqNbr');
    txtServiceGroupSeqNbr.setValue(selOption.getAttribute("serviceGroupSeqNbr"));

    var txtResServiceGroupName = Ext.getCmp('txtResServiceGroupName');
    txtResServiceGroupName.setValue(selOption.getAttribute("resGroupName"));

    Ext.get('txtMode').dom.value = 'Edit';
    Ext.get('groupServiceAssociation').dom.style.display = 'block';

    //Get all available service.
    Accela.ACA.Web.WebService.AdminConfigureService.GetServicesByGroupSeqNbr(selOption.getAttribute("serviceGroupSeqNbr").trim(), ServiceGroup.prototype.LoadGroupCallback); 
};

ServiceGroup.prototype.LoadGroupCallback = function(response) {
    var serviceList = eval('(' + response + ')');

    ServiceGroup.prototype.RefreshSelectedServiceGroupAssociation(serviceList, 'Edit');
    if (GLOBAL_IS_MASTER_LANGUAGE) {
        Ext.getCmp('btnGroupDelete').enable();
    } else {
        Ext.getCmp('btnGroupDelete').disable();
    }
    Ext.getCmp('btnGroupCancel').disable();
    Ext.getCmp('btnGroupOK').disable();
};

ServiceGroup.prototype.DeleteServiceGroup = function () {
    var confirmed = confirm(Ext.LabelKey.Admin_ServiceGroup_Msg_deletegroup);

    if (confirmed) {
        var serviceGroupSeqNbr = Ext.getCmp('txtServiceGroupSeqNbr').getValue().trim();
        
        if (serviceGroupSeqNbr != '') {

            Accela.ACA.Web.WebService.AdminConfigureService.DeleteServiceGroupSetting(serviceGroupSeqNbr, ServiceGroup.prototype.DeleteSuccess); //deleteServiceGroupSetting(String servProvCode, String serviceGroupCode)
            var selectType = document.getElementById("ddlServiceGroup");

            for (var i = 0; i < selectType.options.length; i++) {
                if (selectType.options[i].getAttribute("serviceGroupSeqNbr") == serviceGroupSeqNbr) {
                    selectType.options.remove(i);
                    break;
                }
            }
        }
    }
};

ServiceGroup.prototype.SaveServiceGroup = function () {
    if (!ServiceGroup.prototype.IsChangeGroupSetting() && !document.GroupChanged) {
        return;
    }
    var mode = Ext.get('txtMode').dom.value;
    var txtServiceGroupName = Ext.getCmp('txtServiceGroupName');
    var groupName = txtServiceGroupName.getValue().trim();

    var txtResServiceGroupName = Ext.getCmp('txtResServiceGroupName');
    var resGroupName = txtResServiceGroupName.getValue().trim();

    if (groupName == "") {
        alert(Ext.LabelKey.Admin_ServiceGroup_Msg_cannotempty);
        return -1;
    }

    if (groupName.length > 100) {
        alert(Ext.LabelKey.Admin_ServiceGroup_Msg_invalidgroupnamelength);
        return -1;
    }

    if (ServiceGroup.prototype.CheckExistingGroup(mode, groupName, resGroupName)) {
        alert(Ext.LabelKey.Admin_ServiceGroup_Msg_invalidfiltername);
        return -1;
    }

    var groupItem = ServiceGroup.prototype.GetGroupItem();
    Accela.ACA.Web.WebService.AdminConfigureService.SaveServiceGroupSetting(groupItem, ServiceGroup.prototype.SaveSuccess); //SaveServicesGroupSetting()
};

ServiceGroup.prototype.SaveSuccess = function (serviceGroupSeqNbr) {
    var selectType = document.getElementById("ddlServiceGroup");

    if (selectType.selectedIndex > 0) {
        var oldserviceGroupSeqNbr = selectType.options[selectType.selectedIndex].getAttribute("serviceGroupSeqNbr");

        if (serviceGroupSeqNbr != '' && oldserviceGroupSeqNbr != '') {
            for (var i = 0; i < selectType.options.length; i++) {
                if (selectType.options[i].getAttribute("serviceGroupSeqNbr") == oldserviceGroupSeqNbr) {
                    selectType.options.remove(i);
                    break;
                }
            }
        }
    }

    var txtServiceGroupName = Ext.getCmp('txtServiceGroupName');
    var txtResServiceGroupName = Ext.getCmp('txtResServiceGroupName');
    var txtGroupOrder = Ext.getCmp('txtGroupOrder');
    var txtServiceGroupSeqNbr = Ext.getCmp("txtServiceGroupSeqNbr");
    var groupName = txtServiceGroupName.getValue().trim();
    var resGroupName = txtResServiceGroupName.getValue().trim();
    var groupOrder = txtGroupOrder.getValue();
    ServiceGroup.prototype.AddGroupOption(serviceGroupSeqNbr, groupOrder, groupName, resGroupName);

    txtServiceGroupSeqNbr.setValue(serviceGroupSeqNbr);
    ServiceGroup.prototype.SetServiceGroupValue(groupName);

    Ext.get('txtMode').dom.value = 'Edit';
    Ext.getCmp('btnGroupCancel').disable();
    Ext.getCmp('btnGroupOK').disable();
    if (GLOBAL_IS_MASTER_LANGUAGE) {
        Ext.getCmp('btnGroupDelete').enable();
    } else {
        Ext.getCmp('btnGroupDelete').disable();
    }
    document.GroupChanged = false;

    try {
        window.parent.RemoveMark(false, 'serviceGroup');
    } catch (e) {
    }

    alert(Ext.LabelKey.Admin_ServiceGroup_Msg_savesuccessfully);
};

ServiceGroup.prototype.GetGroupItem = function() {
    var groupItem = new ServiceGroupItem();
    groupItem.ServiceList = this.GetServiceList();
    groupItem.GroupInfo = this.GetGroupInfo();

    return groupItem;
};

ServiceGroup.prototype.GetGroupInfo = function () {
    var group = new Object();

    var txtServiceGroupName = Ext.getCmp('txtServiceGroupName');
    var txtResServiceGroupName = Ext.getCmp('txtResServiceGroupName');
    var groupName = "";
    
    if (GLOBAL_IS_MASTER_LANGUAGE) {
        groupName = txtServiceGroupName.getValue().trim();
    } else {
        groupName = txtResServiceGroupName.getValue().trim();
    }

    var txtGroupOrder = Ext.getCmp('txtGroupOrder');
    var groupOrder = txtGroupOrder.getValue();

    var txtServiceGroupSeqNbr = Ext.getCmp("txtServiceGroupSeqNbr");
    var serviceGroupSeqNbr = txtServiceGroupSeqNbr.getValue();

    group.GroupName = groupName;
    group.sortOrder = groupOrder;
    group.serviceGroupSeqNbr = serviceGroupSeqNbr;

    return group;
};

ServiceGroup.prototype.GetServiceList = function () {
    var nodes = Ext.getCmp('pnlGroupCapSelected').getNodes();
    var serviceList = new Array();

    for (var i = 0; i < nodes.length; i++) {
        var serviceItem = new Object();

        serviceItem.agencyCode = nodes[i].attributes.agencyCode;
        serviceItem.sourceNumber = nodes[i].attributes.key;
        serviceItem.sortOrder = i + 1;
        serviceList[serviceList.length] = serviceItem;
    }

    return serviceList;
};

ServiceGroup.prototype.CheckExistingGroup = function (mode, name, resName) {
    var groupName = name.trim().toUpperCase();
    var $selectType = $('#ddlServiceGroup');
    
    var checkExisting = false;
    $selectType.children('option').each(function () {
        if ((mode != 'Edit' && $(this).val().toUpperCase() == groupName)
            || (mode == 'Edit' && $(this).text().toUpperCase() == resName.trim().toUpperCase())) {
            checkExisting = true;
            return false;
        }
    });

    return checkExisting;
};
/****************************/

ServiceGroup.prototype.RefreshSelectedServiceGroupAssociation = function(serviceList, action) {
    this.CreateAvailableTree(serviceList.SelectedServiceList);

    this.AdjustServicePanel();
};

ServiceGroup.prototype.InitNewServiceGroupUI = function () {
    Ext.get('txtMode').dom.value = 'New';
    this.SetServiceGroupValue('');
    Ext.get('groupServiceAssociation').dom.style.display = 'block';
    Ext.getCmp('txtServiceGroupName').setValue('');
    Ext.getCmp('txtResServiceGroupName').setValue('');
    Ext.getCmp('txtGroupOrder').setValue('');
    Ext.getCmp('txtServiceGroupSeqNbr').setValue('');
    Ext.getCmp('btnGroupDelete').disable();
    Ext.getCmp('btnGroupCancel').disable();
    Ext.getCmp('btnGroupOK').disable();

    this.AdjustServicePanel();
};

ServiceGroup.prototype.CreateAvailableTree = function (serviceList) {
    var availableTree = Ext.getCmp('pnlGroupCapAvailable');
    var selectedTree = Ext.getCmp('pnlGroupCapSelected');

    if (availableTree.root.childNodes.length > 0 || selectedTree.root.childNodes.length > 0) {
        if (selectedTree.root.childNodes.length > 0) {
            availableTree.root.beginUpdate();
            selectedTree.root.beginUpdate();

            for (var i = selectedTree.root.childNodes.length - 1; i >= 0; i--) {
                availableTree.root.appendChild(selectedTree.root.removeChild(selectedTree.root.childNodes[i]));
            }
            availableTree.setNodeWidth(availableTree.id);
            availableTree.root.endUpdate();
            selectedTree.root.endUpdate();
        }

        if (serviceList != null && serviceList.length > 0) {
            this.CreateSelectedTree(serviceList);
        }
    } else {
        document.selectedServiceList = serviceList;
        Accela.ACA.Web.WebService.AdminConfigureService.GetServicesByGroupSeqNbr("", ServiceGroup.prototype.IntialAvailableTree); //Get all available services.
    }
};

ServiceGroup.prototype.IntialAvailableTree = function(response) {
    var serviceList = eval('(' + response + ')');
    var availableTree = Ext.getCmp('pnlGroupCapAvailable');
    availableTree.createTree(serviceList.AvailableServiceList, '');
    ServiceGroup.prototype.CreateSelectedTree(document.selectedServiceList);
};

ServiceGroup.prototype.CreateSelectedTree = function(selectedServiceList) {
    var selectedTree = Ext.getCmp('pnlGroupCapSelected');
    selectedTree.clear();

    if (selectedServiceList != null && selectedServiceList.length > 0) {
        var availableTree = Ext.getCmp('pnlGroupCapAvailable');
        availableTree.root.beginUpdate();
        selectedTree.root.beginUpdate();

        for (var i = 0; i < selectedServiceList.length; i++) {
            if (selectedServiceList[i]) {
                for (var j = 0; j < availableTree.root.childNodes.length; j++) {
                    if (selectedServiceList[i].text == availableTree.root.childNodes[j].text) {
                        var selectedNode = availableTree.root.childNodes[j];
                        availableTree.root.removeChild(selectedNode);
                        selectedTree.root.appendChild(selectedNode);
                        break;
                    }
                }
            }
        }
        selectedTree.setNodeWidth(selectedTree.id);
        availableTree.root.endUpdate();
        selectedTree.root.endUpdate();
    }
};

ServiceGroup.prototype.CheckGroupStatus = function () {
    var btnOK = Ext.getCmp('btnGroupOK');
    var btnCancel = Ext.getCmp('btnGroupCancel');
    
    if (ServiceGroup.prototype.IsChangeGroupSetting()) {
        btnOK.enable();
        btnCancel.enable();
        window.parent.ModifyMark('serviceGroup');
    } else {
        btnOK.disable();
        btnCancel.disable();
        window.parent.RemoveMark(false,'serviceGroup'); 
    }
};

ServiceGroup.prototype.IsChangeGroupSetting = function () {
    var txtServiceGroupName = Ext.getCmp('txtServiceGroupName');
    var txtResServiceGroupName = Ext.getCmp('txtResServiceGroupName');
    var txtGroupOrder = Ext.getCmp('txtGroupOrder');
    var oldServiceGroupName = "";
    var oldResServiceGroupName = "";
    var oldGroupOrder = "";

    var groupOptions = document.getElementById('ddlServiceGroup');

    if (groupOptions.selectedIndex > -1) {
        var selOption = groupOptions.options[groupOptions.selectedIndex];

        if (selOption.value.trim() != "") {
            oldServiceGroupName = selOption.value.trim();
            oldResServiceGroupName = selOption.getAttribute("resGroupName");
            oldGroupOrder = selOption.getAttribute("sortOrder");
        }
    }

    if (txtServiceGroupName.getValue().trim().length > 0
        && (document.GroupChanged
        || (txtServiceGroupName.getValue().trim() != oldServiceGroupName && txtServiceGroupName.isValid)
        || (txtResServiceGroupName.getValue().trim() != oldResServiceGroupName && txtResServiceGroupName.isValid)
        || (txtGroupOrder.getValue() != oldGroupOrder && txtGroupOrder.isValid))) {
        return true;
    }
    else {
        return false;
    }
};

ServiceGroup.prototype.AddGroupServiceAssociation = function(root, selectedNodes) {
    while (selectedNodes.length > 0 && selectedNodes[0] != null) {
        var node = selectedNodes[0].remove();
        node.attributes.binded = true;
        root.appendChild(node);
    }

    var cs = root.childNodes;
    for (var i = 0, len = cs.length; i < len; i++) {
        cs[i].render(true);
    }

    this.CheckGroupStatus();

    if (selectedNodes.length < 1) {
        Ext.getCmp('btnAddGroupCap').disable();
    }
};

ServiceGroup.prototype.AdjustServicePanel = function() {
    var width = Ext.getCmp('pnlHeader').body.dom.offsetWidth;

    if (width) {
        var capAvailable = Ext.getCmp("pnlGroupCapAvailable");
        var capSelected = Ext.getCmp("pnlGroupCapSelected");
        var adjWidth = width * 0.4;

        if (capSelected != null) {
            capSelected.setWidth(adjWidth);
        }
        if (capAvailable != null) {
            capAvailable.setWidth(adjWidth);
        }

        if (!Ext.isIE) {
            document.getElementById('addGroupCap').parentNode.width = width * 0.1;
        } else {
            document.getElementById('addGroupCap').parentElement.width = width * 0.1;
        }
    }
};

Ext.tree.CapPanel = Ext.extend(Ext.tree.TreePanel, {
    onRender: function (ct, position) {
        Ext.tree.CapPanel.superclass.onRender.call(this, ct, position);

        this.on('append', this.update, this);
        this.on('remove', this.update, this);
        this.on('beforemovenode', this.beforemovenode, this);
    },
    getNodes: function () {
        return this.root.childNodes;
    },
    getNodesCount: function () {
        return this.root.childNodes.length;
    },
    update: function () {
        this.updateHeader();
    },
    updateHeader: function () {
        var aa = this.root.childNodes;
        var bb = this.root.childNodes.length;
        var reg = /\d+/;
        this.title = this.title.replace(reg, this.getNodesCount());
        this.header.child('span').update(this.title);

        this.fireEvent('titlechange', this, this.title);
    },
    createNode: function (attr) {
        return (attr.leaf ?
                        new Ext.tree.TreeNode(attr) :
                        new Ext.tree.AsyncTreeNode(attr));
    },
    createTree: function (response, callback) {
        this.isLoaded = false;

        this.clear();

        var o = response;
        var node = this.root;

        try {
            node.beginUpdate();
            for (var i = 0, len = o.length; i < len; i++) {
                if (o[i] == null) break;

                var n = this.createNode(o[i]);
                if (n) {
                    node.appendChild(n);
                }
            }
            node.endUpdate();
            if (typeof callback == "function") {
                callback(this, node);
            }

            this.isLoaded = true;
            this.setNodeWidth(this.id);

        } catch (e) {
            // do nothing
        }
    },
    clear: function () {
        while (this.root.firstChild) {
            this.root.removeChild(this.root.firstChild);
        }
    },
    beforemovenode: function () {
        return false;
    },
    setNodeWidth: function (treeId) {
        var tree = document.getElementById(treeId);
        if (tree) {
            // tree.childNodes[0] is header.
            var outerDiv = tree.childNodes[1].firstChild;
            var outerUL = outerDiv.firstChild;

            if (outerUL && outerDiv.scrollWidth > 0) {
                // set the tree node's width as the tree's scroll width.
                outerUL.style.width = outerDiv.scrollWidth + "px";
            }
        }
    }
});

Ext.reg('CapPanel', Ext.tree.CapPanel);