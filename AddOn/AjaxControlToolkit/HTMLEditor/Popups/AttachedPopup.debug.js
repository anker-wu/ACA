Type.registerNamespace("AjaxControlToolkit.HTMLEditor.Popups");

AjaxControlToolkit.HTMLEditor.Popups.AttachedPopup = function(element) {
    AjaxControlToolkit.HTMLEditor.Popups.AttachedPopup.initializeBase(this, [element]);

    this._relatedElement = null;
}

AjaxControlToolkit.HTMLEditor.Popups.AttachedPopup.prototype = {

    get_relatedElement: function() {
        return this._relatedElement;
    },

    set_relatedElement: function(value) {
        this._relatedElement = value;
    },

    open: function(callback) {
        if (this._relatedElement != null) {
            var popup = this;

            if (!this.checkCorrectLoaded(function() { popup.open(callback); })) {
                return;
            }

            var location = $common.getLocation(this._relatedElement);
            var x = location.x;
            var y = location.y + this._relatedElement.offsetHeight;
            var viewportElement = AjaxControlToolkit.HTMLEditor.getClientViewportElement(this._iframe);
            var theVisibleWidth = viewportElement.clientWidth + viewportElement.scrollLeft;
            var theVisibleHeight = viewportElement.clientHeight + viewportElement.scrollTop;

            if (y < viewportElement.scrollTop) y = viewportElement.scrollTop;
            if (x < viewportElement.scrollLeft) x = viewportElement.scrollLeft;

            if (y + this._iframe.offsetHeight > theVisibleHeight) y -= y + this._iframe.offsetHeight - theVisibleHeight;
            if (x + this._iframe.offsetWidth > theVisibleWidth) x -= x + this._iframe.offsetWidth - theVisibleWidth;

            AjaxControlToolkit.HTMLEditor.Popups.AttachedPopup.callBaseMethod(this, "open", [callback, y, x]);
        }
    },

    close: function(callback) {
        AjaxControlToolkit.HTMLEditor.Popups.AttachedPopup.callBaseMethod(this, "close", [callback]);
    },

    initialize: function() {
        AjaxControlToolkit.HTMLEditor.Popups.AttachedPopup.callBaseMethod(this, "initialize");
    },

    dispose: function() {
        AjaxControlToolkit.HTMLEditor.Popups.AttachedPopup.callBaseMethod(this, "dispose");
    }
}

AjaxControlToolkit.HTMLEditor.Popups.AttachedPopup.registerClass("AjaxControlToolkit.HTMLEditor.Popups.AttachedPopup", AjaxControlToolkit.HTMLEditor.Popups.Popup);
