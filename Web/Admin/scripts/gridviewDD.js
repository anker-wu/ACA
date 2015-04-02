/**
* <pre>
* 
*  Accela Citizen Access Admin
*  File: gridviewDD.js
* 
*  Accela, Inc.
*  Copyright (C): 2010-2014
* 
*  Description:Realize Drag&Drop.
* 
*  Notes:
*      $Id: gridviewDD.js 77905 2010-03-31 12:49:28Z ACHIEVO\grady.lu $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

Ext.namespace('AcceleGridView');

AcceleGridView.RowDragDrap = function(id, group, config, obj) {
    AcceleGridView.RowDragDrap.superclass.constructor.call(this, id, group, config);
    $('#' + this.dragElId).css('opacity', 0.67);
    this.goingUp = false;
    this.lastY = 0;
    this.data = config;
    this.gridview = obj;
    this.currentColumn = null;
    this.place = null;
};

Ext.extend(AcceleGridView.RowDragDrap, Ext.dd.DDProxy, {
    startDrag: function(x, y) {
        // make the proxy look like the source element
        this.currentColumn = null;
        this.place = null;
        var dragEl = Ext.get(this.getDragEl());
        var clickEl = Ext.get(this.getEl());
        clickEl.setStyle('visibility', 'hidden');

        //dragEl.dom.innerHTML = clickEl.dom.innerHTML;
        var padding = clickEl.getPadding('t') + 'px '
            + clickEl.getPadding('r') + 'px '
            + clickEl.getPadding('b') + 'px '
            + clickEl.getPadding('l') + 'px';

        dragEl.dom.innerHTML = '<div style="padding:' + padding + '">' + clickEl.dom.innerHTML + '</div>';

        dragEl.setStyle(clickEl.getStyles('background-color', 'color', 'font-family', 'font-size'));
        dragEl.setStyle('border', '1px solid gray');
    },

    endDrag: function(e) {
        var srcEl = Ext.get(this.getEl());
        var proxy = Ext.get(this.getDragEl());
        // Hide the proxy and show the source element when finished with the animation
        var onComplete = function() {
            proxy.setStyle('visibility', 'hidden');
            srcEl.setStyle('visibility', '');
            if (this.currentColumn != null && this.place != null) {
                if (this.data.elementName != this.currentColumn) {
                    moveColumnOrder(this.place, this.data.elementId, this.data.elementName, this.currentColumn);
                    updategridviewSections(this.gridview);
                    var grid = GetGridBeElementId(this.data.elementId);
                    if (grid != null) {
                        FixLastColumnWidth(grid);
                    }
                }
            }
        };

        // Show the proxy element and animate it to the src element's location
        proxy.setStyle('visibility', '');
        proxy.shift({
            x: srcEl.getX(),
            y: srcEl.getY(),
            easing: 'easeOut',
            duration: .2,
            callback: onComplete,
            scope: this
        });
    },

    onDragDrop: function(e, id) {
    },

    onDrag: function(e) {
        // Keep track of the direction of the drag for use during onDragOver
        var y = e.getPageY();

        if (y < this.lastY) {
            this.goingUp = true;
        } else if (y > this.lastY) {
            this.goingUp = false;
        }

        this.lastY = y;
    },

    onDragOver: function(e, id) {
        var srcEl = Ext.get(this.getEl());
        var destEl = Ext.get(id);
        this.currentColumn = destEl.data.elementName;
        // We are only concerned with list items, we ignore the dragover
        // notifications for the list.
        if (destEl.is('div.dragblock')) {
            if (this.goingUp) {
                // insert above
                this.place = "before";
                srcEl.insertBefore(destEl);
            } else {
                // insert below
                this.place = "after";
                srcEl.insertAfter(destEl);
            }

            //DDM.refreshCache();
        } else if (destEl.is('div.slot')) {
            this.place = "end";
            destEl.appendChild(srcEl);
            // DDM.refreshCache();
        }
    }
});

function moveColumnOrder(place, gridViewId, currentEl, refEl) {
    try {
        var div = GetDIVElementForCurrentDom(gridViewId, true);
        var rows = div.getElementsByTagName("TR");
        var curRow = null;
        
        if (rows.length == 2) {
            curRow = rows[0];
        }
        else {
            curRow = rows[2];
        }

        var thHeaders = curRow.getElementsByTagName("TH");
        var cObj = Ext.DomQuery.selectNode("*[id$=" + currentEl + "]", Ext.get(curRow).dom);
        var currentTh = GetHeaderElement(cObj);
        var rObj = Ext.DomQuery.selectNode("*[id$=" + refEl + "]", Ext.get(curRow).dom);
        var refTh = GetHeaderElement(rObj);
        curRow.removeChild(currentTh);
        if (place == "before") {
            curRow.insertBefore(currentTh, refTh);
        }
        else if (place == "after") {
            refTh = refTh.nextSibling;
            curRow.insertBefore(currentTh, refTh);
        }
        else {
            curRow.insertBefore(currentTh, refTh);
        }      
        
    }
    catch (ex) {

    }
}

function GetHeaderElement(el) {
    if (el.tagName == "th" || el.tagName == "TH") {
        return el;
    }
    else {
        return GetHeaderElement(el.parentNode);
    }
}

// Check the sorted section fields whether existing the same view element, 
//  prevent if section fileds exist the same label, the changed item would be exist the same view element.
function checkSortedFieldsExist(sectionFields, viewElementId) {
    if (sectionFields != null) {
        for (var k = 0; k < sectionFields.length; k++) {
            if (sectionFields[k].ViewElementId == viewElementId) {
                return true;
                break;
            }
        }
    }

    return false;
}

function updategridviewSections(obj) {
    if (getGridView != null) {
        var dragEls = Ext.get('divField').query('.dragblock');
        var fields = new Array();
        for (var i = 0; i < dragEls.length; i++) {
            var str = dragEls[i].title;
            fields.push(str);
        }
        
        /// reorder sectionfields.
        var items = new Array();
        for (var i = 0; i < fields.length; i++) {
            for (var j = 0; j < sectionFields.length; j++) {
                if (sectionFields[j].Label == fields[i] && !checkSortedFieldsExist(items, sectionFields[j].ViewElementId)) {
                    var item = {
                    Label: sectionFields[j].Label,
                    Visible: sectionFields[j].Visible,
                    Required: sectionFields[j].Required,
                    ElementName: sectionFields[j].ElementName,
                    OriginalElementName: sectionFields[j].OriginalElementName,
                    Order: 10 + i,
                    Left: sectionFields[j].Left,
                    Top: sectionFields[j].Top,
                    Width: sectionFields[j].Width,
                    Height: sectionFields[j].Height,
                    ViewElementId: sectionFields[j].ViewElementId,
                    Standard: sectionFields[j].Standard,
                    ControlPrefix: sectionFields[j].ControlPrefix,
                    ElementType: sectionFields[j].ElementType,
                    LabelId: sectionFields[j].LabelId
                    };
                    items.push(item);
                    break;
                }
            }
        }

        obj._sectionFields = items;        
        /// update model value.
        changeItem.SectionItems = items;
        changeItems.UpdateItem(4, changeItem);
        ModifyMark();
    }
}

