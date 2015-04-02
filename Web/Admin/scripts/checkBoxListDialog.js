/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: checkBoxListDialog.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2012
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: checkBoxListDialog.js 77905 2007-10-15 12:49:28Z ACHIEVO\lytton.cheng $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */

function CheckBoxListDialog(context) {
    this.context = context;

    this.Show = function () {
        if (this.context && this.context.dialogProperty.items && this.context.dialogProperty.items.length && this.context.dialogProperty.items.length > 0) {
            var items = this.context.dialogProperty.items;
            $('#' + this.context.dialogProperty.itemContainerID).html('');

            var selectedAllChecked = true;

            for (var i = 0; i < items.length; i++) {
                if (!items[i].Checked) {
                    selectedAllChecked = false;
                    break;
                }
            }

            this.CreateItem(this.context.dialogProperty.itemContainerID + "selectAll", "-1", "<b>Select All</b>", this.context, selectedAllChecked);

            for (var i = 0; i < items.length; i++) {
                this.CreateItem(this.context.dialogProperty.itemIDPrefix + i, items[i].Key, items[i].Text, this.context, items[i].Checked);
            }

            Ext.DomHelper.append(context.dialogProperty.itemContainerID, {
                tag: 'div',
                html: '<div style="height:5px"></div>'
            }, true);
        }

        var button = $('#' + this.context.dialogProperty.positionObjID);
        this.SetPosition(button, this.context);
        this.configWindow.orginalFocusObj = button;
        
        this.saveButton.disable();
        this.configWindow.show();
    };

    this.CreateItem = function (inputID, optionKey, optionText, context, checked) {
        var check;

        if (checked) {
            check = 'checked="checked"';
        }
        else {
            check = '';
        }

        Ext.DomHelper.append(context.dialogProperty.itemContainerID, {
            tag: 'div',
            cls: 'checkboxlistdialog_item',
            html: '<table><tr>' +
            '<td><input id="' + inputID + '" value="' + optionKey
            + '" onclick="CheckBoxChange(this,\'' + context.dialogProperty.saveButtonID + '\',\'' + context.dialogProperty.itemContainerID + '\')" type="checkbox" ' + check + ' /></td>'
            + '<td><label for="' + inputID + '">' + optionText + '</label></td></tr></table>'
        }, true);
    };

    this.init = function (context) {
        this.saveButton = new Ext.Toolbar.Button({
            text: "OK",
            id: context.dialogProperty.saveButtonID,
            pressed: true,
            minWidth: 50,
            handler: function () {
                context.checkBoxList = $('#' + context.dialogProperty.itemContainerID + ' [type=checkbox][value!=-1]');
                context.dialogProperty.save(context);
            }
        });

        this.cancelButton = new Ext.Toolbar.Button({
            text: "Cancel",
            pressed: true,
            minWidth: 50,
            handler: function () {
                this.containWindow.hide();
            }
        });

        this.configWindow = new Ext.Window({
            title: context.dialogProperty.sectionTitle,
            layout: 'fit',
            width: 215,
            height: 245,
            closeAction: 'hide',
            resizable: false,
            plain: true,
            renderTo: Ext.getBody(),
            listeners: {
                show: function () {
                    var el = this.getEl();
                    el.focus.defer(10, el);
                },
                hide: function () {
                    if (this.orginalFocusObj) {
                        this.orginalFocusObj.focus();
                    }
                }
            },
            html: '<div id = "' + context.dialogProperty.itemContainerID + '" style="WIDTH:99%; HEIGHT: 185px;overflow-x:hidden;overflow-y:auto;"></div>',
            bbar: [this.saveButton, new Ext.Toolbar.Spacer(), new Ext.Toolbar.Spacer(), new Ext.Toolbar.Spacer(), this.cancelButton]
        });

        this.cancelButton.containWindow = this.configWindow;
    };

    this.init(this.context);

    this.SetPosition = function (obj, context) {
        var width = 215;
        var height = 245;
        var top = obj.offset().top - height + obj.height();
        var left = obj.offset().left - width;
        
        if (context.dialogProperty.isPositionRight != null && context.dialogProperty.isPositionRight) {
            left = obj.offset().left + obj.width();
        }

        var win = this.configWindow;
        win.setWidth(width);
        win.setHeight(height);
        win.setPosition(left, top);
    };

    this.Close = function () {
        try {
            this.configWindow.hide();
        }
        catch (err) {
        }
    };
}

function CheckBoxChange(obj, saveButtonID, containerID) {
    Ext.getCmp(saveButtonID).enable();

    if (obj.value == "-1") {
        $('#' + containerID + ' [type=checkbox][value!=-1]').each(function () {
            this.checked = obj.checked;
        });
    }
    else {
        Ext.get(containerID + "selectAll").dom.checked = false;
    }
}