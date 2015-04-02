/**
 * <pre>
 * 
 *  Accela Citizen Access Admin
 *  File: tabDragDrop.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:Realize Drag&Drop.
 * 
 *  Notes:
 *      $Id: tabDragDrop.js 77905 2008-04-10 12:49:28Z ACHIEVO\levin.feng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */



Ext.namespace('Tipos');
var DDM = Ext.dd.DragDropMgr;

Tipos.DDList = function(id, sGroup, config) {
    Tipos.DDList.superclass.constructor.call(this, id, sGroup, config);

    // The proxy is slightly transparent
    $('#' + this.dragElId).css('opacity', 0.67);

    this.goingUp = false;
    this.lastY = 0;
};



Ext.extend(Tipos.DDList, Ext.dd.DDProxy, {
    startDrag: function(x, y) {
        // make the proxy look like the source element
        var dragEl = Ext.get(this.getDragEl());
        var clickEl = Ext.get(this.getEl());
        clickEl.setStyle('visibility', 'hidden');

        //dragEl.dom.innerHTML = clickEl.dom.innerHTML;
        var padding = clickEl.getPadding('t') + 'px '
            + clickEl.getPadding('r') + 'px '
            + clickEl.getPadding('b') + 'px '
            + clickEl.getPadding('l') + 'px';

        dragEl.dom.innerHTML = '<div style="padding:' + padding + '">' + clickEl.dom.innerHTML + '</div>';

        dragEl.setStyle(clickEl.getStyles('background-color','color','font-family','font-size'));
        dragEl.setStyle('border', '1px solid gray');
    },

    endDrag: function(e) {
        var srcEl = Ext.get(this.getEl());
        var proxy = Ext.get(this.getDragEl());

        // Hide the proxy and show the source element when finished with the animation
        var onComplete = function() {
            proxy.setStyle('visibility', 'hidden');
            srcEl.setStyle('visibility', '');
            refreshTabs();
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
       // var pt = e.getPoint();
        
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


        // We are only concerned with list items, we ignore the dragover
        // notifications for the list.
        if (destEl.is('div.dragblock')) {
            if (this.goingUp) {
                // insert above
                srcEl.insertBefore(destEl);
            } else {
                // insert below
                srcEl.insertAfter(destEl);
            }

            //DDM.refreshCache();
        } else if (destEl.is('div.slot')) {
            destEl.appendChild(srcEl);
           // DDM.refreshCache();
        }
    }
});








