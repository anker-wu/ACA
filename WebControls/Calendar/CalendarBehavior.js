/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CalendarBehavior.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: CalendarBehavior.js 77229 2007-09-28 05:46:06Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  04/03/2007     		vincent.xu				Initial.
 * </pre>
 */
// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.
Type.registerNamespace("AjaxControlToolkit");

/*A behavior that attaches a calendar date selector to a textbox
* @param element (Sys.UI.DomElement) The element to attach to
*/
AjaxControlToolkit.CalendarBehavior = function(element) {
    AjaxControlToolkit.CalendarBehavior.initializeBase(this, [element]);
    this._isRTL = false;
    this._format = "d";
    this._cssClass = "ajax__calendar";
    this._enabled = true;
    this._animated = true;
    this._id = null;
    this._beginFocusID = null;
    this._endFocusIDOfHeader = null;
    this._beginDayID = null;
    this._endDayID = null;
    this._beginMonthID = null;
    this._endMonthID = null;
    this._beginYearID = null;
    this._endYearID = null;
    this._beginFocusIDOfFooter = null;
    this._endFocusID = null;
    this._buttonID = null;
    this._layoutRequested = 0;
    this._layoutSuspended = false;
    this._bodyClickHandler = null; //Add bodyClickHandler;
    this.gDateAttr = "gDate";
    this._isHijriCalendar = false;

    this._selectedDate = null;
    this._visibleDate = null;
    this._todaysDate = null;
    this._firstDayOfWeek = AjaxControlToolkit.FirstDayOfWeek.Default;

    this._popupDiv = null;
    this._prevArrow = null;
    this._prevArrowImage = null;
    this._prevArrowID = null;
    this._prevArrowTitle = "";
    this._formBeginText = "";
    this._formEndText = "";
    this._nextArrow = null;
    this._nextArrowImage = null;
    this._nextArrowID = null;
    this._nextArrowTitle = "";
    this._title = null;
    this._titleID = null;
    this._titleOnLastFocus = "";
    this._today = null;
    this._todayID = null;
    this._daysRow = null;
    this._monthsRow = null;
    this._yearsRow = null;
    this._daysBody = null;
    this._monthsBody = null;
    this._yearsBody = null;
    this._button = null;
    this._imagePath = "";
    
    this._popupBehavior = null;
    this._modeChangeAnimation = null;
    this._modeChangeMoveTopOrLeftAnimation = null;
    this._modeChangeMoveBottomOrRightAnimation = null;
    this._mode = "days";
    this._selectedDateChanging = false;
    this._isOpen = false;
    this._isAnimating = false;
    this._width = 170;
    this._height = 139;
    this._modes = {"days" : null, "months" : null, "years" : null};
    this._modeOrder = {"days" : 0, "months" : 1, "years" : 2 };

    this._daysCaption = "";
    this._daysSummary = "";
    
    // Safari needs a longer delay in order to work properly
    this._blur = new AjaxControlToolkit.DeferredOperation(((Sys.Browser.agent === Sys.Browser.Safari) ? 1000 : 1), this, this._onblur);
    this._focus = new AjaxControlToolkit.DeferredOperation(((Sys.Browser.agent === Sys.Browser.Safari) ? 1000 : 1), this, this._onfocus);
    
    this._button$delegates = {
        click : Function.createDelegate(this, this._button_onclick)
    };
    this._element$delegates = {
        focus : Function.createDelegate(this, this._element_onfocus),
        focusout : Function.createDelegate(this, this._element_onblur),
        blur : Function.createDelegate(this, this._element_onblur),
        change : Function.createDelegate(this, this._element_onchange)
    };
    this._popup$delegates = { 
        activate : Function.createDelegate(this, this._popup_onfocus),
        focus : Function.createDelegate(this, this._popup_onfocus),
        dragstart: Function.createDelegate(this, this._popup_ondragstart),
        select: Function.createDelegate(this, this._popup_onselect)
    };
    this._cell$delegates = {
        mouseover : Function.createDelegate(this, this._cell_onmouseover),
        mouseout: Function.createDelegate(this, this._cell_onmouseout),
        keydown: Function.createDelegate(this, this._cell_onkeydown),
        click : Function.createDelegate(this, this._cell_onclick)
    };
    this._link$delegates = {
        keydown: Function.createDelegate(this, this._cell_onkeydown),
    };
};

AjaxControlToolkit.CalendarBehavior.prototype = {

    get_id: function() {
        return this._id;
    },
    set_id: function(value) {
        if (this._id != value) {
            this._id = value;
            this.raisePropertyChanged("id");
        }
    },
    /* Get rtl of element 
     * @return (Boolean ) whether current text direction is RTL
     */
    get_isRTL: function () {
        return this._isRTL;
    },
    set_isRTL: function (value) {
        if (this._isRTL != value) {
            this._isRTL = value;
            this.raisePropertyChanged("isRTL");
        }
    },
    /* Get is animated of element
     * @return (Boolean) Whether changing modes is animated
     */
    get_animated: function() {
        return this._animated;
    },
    set_animated: function(value) {
        if (this._animated != value) {
            this._animated = value;
            this.raisePropertyChanged("animated");
        }
    },
    /* Get enabled of element
     * @return (Boolean) Whether this behavior is available for the current element
     */
    get_enabled: function() {
        return this._enabled;
    },
    set_enabled: function(value) {
        if (this._enabled != value) {
            this._enabled = value;
            this.raisePropertyChanged("enabled");
        }
    },
    /* Get button of element
     * @return (Sys.UI.DomElement) The button to use to show the calendar (optional)
     */
    get_button: function() {
        return this._button;
    },
    set_button: function(value) {
        if (this._button != value) {
            if (this._button && this.get_isInitialized()) {
                $common.removeHandlers(this._button, this._button$delegates);
            }
            this._button = value;
            if (this._button && this.get_isInitialized()) {
                $addHandlers(this._button, this._button$delegates);
            }
            this.raisePropertyChanged("button");
        }
    },
    /* Get date format of element
     * @return (String) The format to use for the date value
     */
    get_format: function() {
        return this._format;
    },
    set_format: function(value) {
        if (this._format != value) {
            this._format = value;
            this.raisePropertyChanged("format");
        }
    },
    //The date value represented by the text box
    get_selectedDate: function () {
        var eltVal = this._get_element_value();

        if (eltVal) {
            this._selectedDate = this._parseTextValue(eltVal);
        } else {
            this._selectedDate = null;
        }

        return this._selectedDate;
    },
    set_selectedDate: function(value) {
        if (this._selectedDate != value) {
            this._selectedDate = value;

            this._selectedDateChanging = true;
            var text = "";

            if (value) {
                text = value.localeFormat(this._format);
            }

            if (text != this._get_element_value()) {
                this._set_element_value(value);
                this._fireChanged();
            }

            this._selectedDateChanging = false;
            this.invalidate();
            this.raisePropertyChanged("selectedDate");
        }
    },
    /* Get is visible of element
    *  @return (Boolean) The date currently visible in the calendar
    */
    get_visibleDate: function() {
        return this._visibleDate;
    },
    set_visibleDate: function(value) {
        if (value) value = value.getDateOnly();
        if (this._visibleDate != value) {
            this._switchMonth(value, !this._isOpen);
            this.raisePropertyChanged("visibleDate");
        }
    },
    /*
    The date to use for "Today"
    @return (AdjustDate) the date of today.
    */
    get_todaysDate: function() {
        if (this._todaysDate != null) {
            return this._todaysDate;
        }

        this._todaysDate = new AdjustDate({ isHijriDate: this._isHijriCalendar, gDate: new Date().getDateOnly()});
        return this._todaysDate;
    },
    set_todaysDate: function(value) {
        if (value) value = value.getDateOnly();

        if (this._todaysDate != value) {
            this._todaysDate = new AdjustDate({ isHijriDate: this._isHijriCalendar, gDate: value});
            this.invalidate();
            this.raisePropertyChanged("todaysDate");
        }
    },
    /*
    * The day of the week to appear as the first day in the calendar
    * @return (Number) First day of week
    */
    get_firstDayOfWeek: function() {
        return this._firstDayOfWeek;
    },
    set_firstDayOfWeek: function(value) {
        if (this._firstDayOfWeek != value) {
            this._firstDayOfWeek = value;
            this.invalidate();
            this.raisePropertyChanged("firstDayOfWeek");
        }
    },
    /*
    * The class of element
    * @return (String) The CSS class selector to use to change the calendar's appearance
    */
    get_cssClass: function() {
        return this._cssClass;
    },
    set_cssClass: function(value) {
        if (this._cssClass != value) {

            if (this._cssClass && this.get_isInitialized()) {
                Sys.UI.DomElement.removeCssClass(this._container, this._cssClass);
            }

            this._cssClass = value;

            if (this._cssClass && this.get_isInitialized()) {
                Sys.UI.DomElement.addCssClass(this._container, this._cssClass);
            }

            this.raisePropertyChanged("cssClass");
        }
    },
    /*
    * Get the title of the Prev button in popup dialog.
    * @return (String) The title of the Prev button in popup dialog.
    */
    get_prevArrowTitle: function () {
        return this._prevArrowTitle;
    },
    set_prevArrowTitle: function (value) {
        if (this._prevArrowTitle != value) {
            this._prevArrowTitle = value;
            this.raisePropertyChanged("prevArrowTitle");
        }
    },
    /*
    * Get the title of the Next button in popup dialog.
    * @return (String) The title of the Next button in popup dialog.
    */
    get_nextArrowTitle: function () {
        return this._nextArrowTitle;
    },
    set_nextArrowTitle: function (value) {
        if (this._nextArrowTitle != value) {
            this._nextArrowTitle = value;
            this.raisePropertyChanged("nextArrowTitle");
        }
    },
    /*
   * Get the title of formBeginText.
   * @return (String) The title of the formBeginText.
   */
    get_formBeginText: function () {
        return this._formBeginText;
    },
    set_formBeginText: function (value) {
        if (this._formBeginText != value) {
            this._formBeginText = value;
            this.raisePropertyChanged("formBeginText");
        }
    },
    /*
   * Get the title of the formEndText.
   * @return (String) The title of formEndText.
   */
    get_formEndText: function () {
        return this._formEndText;
    },
    set_formEndText: function (value) {
        if (this._formEndText != value) {
            this._formEndText = value;
            this.raisePropertyChanged("formEndText");
        }
    },
    /*
    * Get the title on the last focused object in popup dialog.
    * @return (String) The title on the last focused object in popup dialog.
    */
    get_titleOnLastFocus: function () {
        return this._titleOnLastFocus;
    },
    set_titleOnLastFocus: function (value) {
        if (this._titleOnLastFocus != value) {
            this._titleOnLastFocus = value;
            this.raisePropertyChanged("titleOnLastFocus");
        }
    },
    /*
    * Get is hijri calendar of element
    * @return (Boolean) whether hijri calendar 
    */
    get_isHijriCalendar: function () {
        return this._isHijriCalendar;
    },
    set_isHijriCalendar: function (value) {
        if (this._isHijriCalendar != value) {
            this._isHijriCalendar = value;
            this.raisePropertyChanged("isHijriCalendar");
        }
    },
    /*
    * Get the caption of days in popup dialog.
    * @return (String) The caption of days in popup dialog.
    */
    get_daysCaption: function () {
        return this._daysCaption;
    },
    set_daysCaption: function (value) {
        if (this._daysCaption != value) {
            this._daysCaption = value;
            this.raisePropertyChanged("daysCaption");
        }
    },
    /*
    * Get the summary of days in popup dialog.
    * @return (String) The summary of days in popup dialog.
    */
    get_daysSummary: function () {
        return this._daysSummary;
    },
    set_daysSummary: function (value) {
        if (this._daysSummary != value) {
            this._daysSummary = value;
            this.raisePropertyChanged("daysSummary");
        }
    },
    /*
   * Get the path of images.
   * @return (String) The path of images.
   */
    get_imagePath: function () {
        return this._imagePath;
    },
    set_imagePath: function (value) {
        if (this._imagePath != value) {
            this._imagePath = value;
            this.raisePropertyChanged("imagePath");
        }
    },
    /* Adds an event handler for the <code>showing</code> event.
    *  @param handler (Function) The handler to add to the event.
    */
    add_showing: function(handler) {
        this.get_events().addHandler("showing", handler);
    },
    /* Removes an event handler for the <code>showing</code> event.
    *  @param handler (Function) The handler to remove from the event.
    */
    remove_showing: function(handler) {
        this.get_events().removeHandler("showing", handler);
    },
    // Raise the <code>showing</code> event
    raiseShowing: function() {
        var handlers = this.get_events().getHandler("showing");

        if (handlers) {
            handlers(this, Sys.EventArgs.Empty);
        }
    },
    /* Adds an event handler for the <code>shown</code> event.
     * @param handler (Function) The handler to add to the event.
     */
    add_shown: function(handler) {
        this.get_events().addHandler("shown", handler);
    },
    /* Removes an event handler for the <code>shown</code> event.
    *  @param handler (Function) The handler to remove from the event.
    */
    remove_shown: function(handler) {
        this.get_events().removeHandler("shown", handler);
    },
    // Raise the <code>shown</code> event
    raiseShown: function() {
        var handlers = this.get_events().getHandler("shown");

        if (handlers) {
            handlers(this, Sys.EventArgs.Empty);
        }
    },
    /* Adds an event handler for the <code>hiding</code> event.
    *  @param handler (Function) The handler to add to the event.
    */
    add_hiding: function(handler) {
        this.get_events().addHandler("hiding", handler);
    },
    /* Removes an event handler for the <code>hiding</code> event.
     * @param handler (Function) The handler to remove from the event.
    */
    remove_hiding: function(handler) {
        this.get_events().removeHandler("hiding", handler);
    },
    // Raise the <code>hiding</code> event
    raiseHiding: function() {
        var handlers = this.get_events().getHandler("hiding");

        if (handlers) {
            handlers(this, Sys.EventArgs.Empty);
        }
    },
    /* Adds an event handler for the <code>hidden</code> event.
    *  @param handler (Function) The handler to add to the event.
    */
    add_hidden: function(handler) {
        this.get_events().addHandler("hidden", handler);
    },
    /* Removes an event handler for the <code>hidden</code> event.
    *  @param handler (Function) The handler to remove from the event.
    */
    remove_hidden: function(handler) {
        this.get_events().removeHandler("hidden", handler);
    },
    // Raise the <code>hidden</code> event
    raiseHidden: function() {
        var handlers = this.get_events().getHandler("hidden");

        if (handlers) {
            handlers(this, Sys.EventArgs.Empty);
        }
    },
    /* Adds an event handler for the <code>dateSelectionChanged</code> event.
     * @param handler (Function) The handler to remove from the event.
    */
    add_dateSelectionChanged: function(handler) {
        this.get_events().addHandler("dateSelectionChanged", handler);
    },
    /* Removes an event handler for the <code>dateSelectionChanged</code> event.
    *  @param handler (Function) The handler to remove from the event.
    */
    remove_dateSelectionChanged: function(handler) {
        this.get_events().removeHandler("dateSelectionChanged", handler);
    },
    // Raise the <code>dateSelectionChanged</code> event
    raiseDateSelectionChanged: function() {
        var handlers = this.get_events().getHandler("dateSelectionChanged");
        if (handlers) {
            handlers(this, Sys.EventArgs.Empty);
        }
    },
    //Initializes the components and parameters for this behavior
    initialize: function() {
        AjaxControlToolkit.CalendarBehavior.callBaseMethod(this, "initialize");

        var elt = this.get_element();

        if (this._isHijriCalendar) {
            var hiddenClientState = $(elt).parent().find("input[id$='" + GLOBAL_CALENDARTEXT_CLIENTSTATE + "']");

            //Id of control in ASIT will changed after render to html 
            hiddenClientState.attr("id", elt.id + GLOBAL_CALENDARTEXT_CLIENTSTATE);
            $(elt).attr(this.gDateAttr, elt.defaultValue);
            $(elt).attr("format", this.get_format());
            $(elt).val("");
        }

        $addHandlers(elt, this._element$delegates);

        if (this._button)
            $addHandlers(this._button, this._button$delegates);

        this._modeChangeMoveTopOrLeftAnimation = new AjaxControlToolkit.Animation.LengthAnimation(null, null, null, "style", null, 0, 0, "px");
        this._modeChangeMoveBottomOrRightAnimation = new AjaxControlToolkit.Animation.LengthAnimation(null, null, null, "style", null, 0, 0, "px");
        this._modeChangeAnimation = new AjaxControlToolkit.Animation.ParallelAnimation(null, .25, null, [this._modeChangeMoveTopOrLeftAnimation, this._modeChangeMoveBottomOrRightAnimation]);

        this._bodyClickHandler = Function.createDelegate(this, this._onBodyClick);
        $addHandler(document.body, 'click', this._bodyClickHandler); //Add bodyClickHandler;

        this._prevArrowID = this._id + "_lnkPrevArrow";
        this._titleID = this._id + "_lnkTitleID";
        this._nextArrowID = this._id + "_lnkNextArrow";
        this._beginDayID = this._id + "_lnkBeginDay";
        this._endDayID = this._id + "_lnkEndDay";
        this._beginMonthID = this._id + "_lnkBeginMonth";
        this._endMonthID = this._id + "_lnkEndMonth";
        this._beginYearID = this._id + "_lnkBeginYear";
        this._endYearID = this._id + "_lnkEndYear";
        this._todayID = this._id + "_lnkToday";

        this._beginFocusID = this._id + "_lnkBeginFocus";
        this._endFocusIDOfHeader = this._titleID;
        this._beginFocusIDOfFooter = this._todayID;
        this._endFocusID = this._id + "_lnkEndFocus";

        var value = this.get_selectedDate();
        if (value) {
            this._set_element_value(value);
        }
    },
    // Disposes this behavior's resources
    dispose: function() {
        if (this._bodyClickHandler) {
            $removeHandler(document.body, 'click', this._bodyClickHandler);
            this._bodyClickHandler = null;
        } //Add bodyClickHandler;

        if (this._popupBehavior) {
            this._popupBehavior.dispose();
            this._popupBehavior = null;
        }

        this._modes = null;
        this._modeOrder = null;

        if (this._modeChangeMoveTopOrLeftAnimation) {
            this._modeChangeMoveTopOrLeftAnimation.dispose();
            this._modeChangeMoveTopOrLeftAnimation = null;
        }

        if (this._modeChangeMoveBottomOrRightAnimation) {
            this._modeChangeMoveBottomOrRightAnimation.dispose();
            this._modeChangeMoveBottomOrRightAnimation = null;
        }

        if (this._modeChangeAnimation) {
            this._modeChangeAnimation.dispose();
            this._modeChangeAnimation = null;
        }

        if (this._container) {
            this._container.parentNode.removeChild(this._container);
            this._container = null;
        }

        if (this._popupDiv) {
            $common.removeHandlers(this._popupDiv, this._popup$delegates);
            this._popupDiv = null;
        }

        if (this._prevArrow) {
            $common.removeHandlers(this._prevArrow, this._cell$delegates);
            this._prevArrow = null;
        }

        if (this._prevArrowImage) {
            $common.removeHandlers(this._prevArrowImage, this._cell$delegates);
            this._prevArrowImage = null;
        }

        if (this._nextArrow) {
            $common.removeHandlers(this._nextArrow, this._cell$delegates);
            this._nextArrow = null;
        }

        if (this._nextArrowImage) {
            $common.removeHandlers(this._nextArrowImage, this._cell$delegates);
            this._nextArrowImage = null;
        }

        if (this._title) {
            $common.removeHandlers(this._title, this._cell$delegates);
            this._title = null;
        }

        if (this._today) {
            $common.removeHandlers(this._today, this._cell$delegates);
            this._today = null;
        }

        if (this._daysRow) {
            for (var i = 0; i < this._daysBody.rows.length; i++) {
                var row = this._daysBody.rows[i];

                for (var j = 0; j < row.cells.length; j++) {
                    $common.removeHandlers(row.cells[j].firstChild, this._cell$delegates);
                }
            }

            this._daysRow = null;
        }

        if (this._monthsRow) {
            for (var i = 0; i < this._monthsBody.rows.length; i++) {
                var row = this._monthsBody.rows[i];

                for (var j = 0; j < row.cells.length; j++) {
                    $common.removeHandlers(row.cells[j].firstChild, this._cell$delegates);
                }
            }

            this._monthsRow = null;
        }

        if (this._yearsRow) {
            for (var i = 0; i < this._yearsBody.rows.length; i++) {
                var row = this._yearsBody.rows[i];

                for (var j = 0; j < row.cells.length; j++) {
                    $common.removeHandlers(row.cells[j].firstChild, this._cell$delegates);
                }
            }

            this._yearsRow = null;
        }

        if (this._button) {
            $common.removeHandlers(this._button, this._button$delegates);
            this._button = null;
        }

        var elt = this.get_element();
        $common.removeHandlers(elt, this._element$delegates);
        AjaxControlToolkit.CalendarBehavior.callBaseMethod(this, "dispose");
    },
    //Shows the calendar
    show: function() {
        this._ensureCalendar();

        if (!this._isOpen) {

            if (this._container) {
                this._container.style.display = "";
            }

            this.raiseShowing();
            this._isOpen = true;
            this._switchMonth(null, true);
            this._popupBehavior.show();

            if (this._isRTL) {
                var element = this._popupBehavior.get_element();
                var bounds = this._popupBehavior.getBounds();

                if (Sys.Browser.agent == Sys.Browser.InternetExplorer && bounds.x >= this._popupBehavior._parentElement.ownerDocument.documentElement.scrollLeft) {
                    bounds.x -= this._popupBehavior._parentElement.ownerDocument.documentElement.scrollLeft;
                }

                if (bounds.x >= 2) {
                    bounds.x -= 2;
                }

                $common.setLocation(element, bounds);
            }

            // Get textbox of the Calendar
            var obj = this.get_element();

            // Reset the absolute top value of the Calendar
            var currentTop = obj.offsetHeight + $(obj).position().top;
            $(this._popupDiv).css('top', currentTop + 'px');

            this.raiseShown();
        }

        $("#" + this._beginFocusID).focus();
    },
    //Hides the calendar
    hide: function() {
        this.raiseHiding();

        if (this._container) {
            this._container.style.display = "none";
            this._popupBehavior.hide();
            this._switchMode("days", true);
        }

        this._isOpen = false;
        this.raiseHidden();

        //To resolve the bug:30852
        if (Sys.Browser.agent === Sys.Browser.Safari) {
            $common.tryFireEvent(this.get_element(), 'click');
        }
    },
    //Suspends layout of the behavior while setting properties
    suspendLayout: function() {
        this._layoutSuspended++;
    },
    //Resumes layout of the behavior and performs any pending layout requests
    resumeLayout: function() {
        this._layoutSuspended--;

        if (this._layoutSuspended <= 0) {
            this._layoutSuspended = 0;

            if (this._layoutRequested) {
                this._performLayout();
            }
        }
    },
    //Performs layout of the behavior unless layout is suspended
    invalidate: function() {
        if (this._layoutSuspended > 0) {
            this._layoutRequested = true;
        } else {
            this._performLayout();
        }
    },
    //Builds the calendar's layout
    _buildCalendar: function() {
        var elt = this.get_element();

        this._container = $common.createElementFromTemplate({
            nodeName: "div",
            cssClasses: [this._cssClass]
        }, elt.parentNode);

        if (this._isHijriCalendar) {
            this._formBeginText = "موجود في بداية النموذج.";
        }

        this._beginLink = $common.createElementFromTemplate({
            nodeName: "a",
            cssClasses: ["ACA_FLeft NotShowLoading"],
            properties: {
                id: this._beginFocusID,
                href: "javascript:void(0);",
                title: this._formBeginText,
            },
            events: this._link$delegates,
        }, this._container);

        $common.createElementFromTemplate({
            nodeName: "img",
            cssClasses: ["ACA_NoBorder"],
            properties: {
                src: this._imagePath + "spacer.gif",
                alt: this._formBeginText,
                width: 0,
                height: 0,
            },
        }, this._beginLink);

        this._popupDiv = $common.createElementFromTemplate({
            nodeName: "div",
            events: this._popup$delegates,
            properties: {
                id: this._id
            },
            cssClasses: ["ajax__calendar_container"],
            visible: false
        }, this._container);
    },
    //Builds the header for the calendar
    _buildHeader: function() {
        this._header = $common.createElementFromTemplate({
            nodeName: "div",
            cssClasses: ["ajax__calendar_header"]
        }, this._popupDiv);

        if (this._isHijriCalendar) {
            this._prevArrowTitle = "{0} السابق.";
            this._nextArrowTitle = "{0} التالي.";
        }

        var prevArrowWrapper = $common.createElementFromTemplate({ nodeName: "div" }, this._header);
        this._prevArrow = $common.createElementFromTemplate({
            nodeName: "div",
            properties: {
                mode: "prev",
                tabIndex: 0,
                id: this._prevArrowID,
                title: this._prevArrowTitle.replace(/\{0\}/g, "month")
            },
            events: this._cell$delegates,
            cssClasses: ["ajax__calendar_prev"]
        }, prevArrowWrapper);

        var nextArrowWrapper = $common.createElementFromTemplate({ nodeName: "div" }, this._header);
        this._nextArrow = $common.createElementFromTemplate({
            nodeName: "div",
            properties: {
                mode: "next",
                tabIndex: 0,
                id: this._nextArrowID,
                title: this._nextArrowTitle.replace(/\{0\}/g, "month")
            },
            events: this._cell$delegates,
            cssClasses: ["ajax__calendar_next"]
        }, nextArrowWrapper);

        var titleWrapper = $common.createElementFromTemplate({ nodeName: "div" }, this._header);
        this._title = $common.createElementFromTemplate({
            nodeName: "div",
            properties: {
                mode: "title",
                tabIndex: 0,
                id: this._titleID
            },
            events: this._cell$delegates,
            cssClasses: ["ajax__calendar_title"]
        }, titleWrapper);

        if (this._isHijriCalendar && Sys.CultureInfo.CurrentCulture.name != "ar-AE") {
            $(this._prevArrow).removeClass("ajax__calendar_prev");
            $(this._prevArrow).addClass("ajax__calendar_next");
            $(this._nextArrow).removeClass("ajax__calendar_next");
            $(this._nextArrow).addClass("ajax__calendar_prev");
        }
    },
    //Builds the body region for the calendar
    _buildBody: function() {
        this._body = $common.createElementFromTemplate({
            nodeName: "div",
            cssClasses: ["ajax__calendar_body"]
        }, this._popupDiv);

        this._buildDays();
        this._buildMonths();
        this._buildYears();
    },
    //Builds the footer for the calendar
    _buildFooter: function() {
        var todayWrapper = $common.createElementFromTemplate({ nodeName: "div" }, this._popupDiv);
        this._today = $common.createElementFromTemplate({
            nodeName: "div",
            properties: {
                mode: "today",
                tabIndex: 0,
                id: this._todayID,
                title: this._isHijriCalendar ? "اليوم." : this._titleOnLastFocus
            },
            events: this._cell$delegates,
            cssClasses: ["ajax__calendar_footer", "ajax__calendar_today"]
        }, todayWrapper);

        if (this._isHijriCalendar) {
            this._formEndText = "هذه هي نهاية النموذج. اضغط على مفتاح علامة التبويب لنقل البؤرة إلى البداية.";
        }

        this._endLink = $common.createElementFromTemplate({
            nodeName: "a",
            cssClasses: ["NotShowLoading"],
            properties: {
                id: this._endFocusID,
                href: "javascript:void(0);",
                title: this._formEndText,
            },
            events: this._link$delegates,
        }, this._container);

        $common.createElementFromTemplate({
            nodeName: "img",
            cssClasses: ["ACA_NoBorder"],
            properties: {
                src: this._imagePath + "spacer.gif",
                alt: this._formEndText,
                width: 0,
                height:0,
            },
        }, this._endLink);
    },
    //Builds a "days of the month" view for the calendar
    _buildDays: function() {
        this._days = $common.createElementFromTemplate({
            nodeName: "div",
            cssClasses: ["ajax__calendar_days"]
        }, this._body);
        this._modes["days"] = this._days;

        this._daysTable = $common.createElementFromTemplate({
            nodeName: "table",
            properties: {
                cellPadding: 0,
                cellSpacing: 0,
                border: 0,
                summary: this._daysSummary,
                style: { margin: "auto" }
            }
        }, this._days);

        var daysTableCaption = $common.createElementFromTemplate({ nodeName: "caption" }, this._daysTable);
        daysTableCaption.innerText = this._daysCaption;

        this._daysTableHeader = $common.createElementFromTemplate({ nodeName: "thead" }, this._daysTable);
        this._daysTableHeaderRow = $common.createElementFromTemplate({ nodeName: "tr" }, this._daysTableHeader);
        this._daysBody = $common.createElementFromTemplate({ nodeName: "tbody" }, this._daysTable);

        for (var i = 0; i < 7; i++) {
            var dayCell = $common.createElementFromTemplate({ nodeName: "th", properties: { scope: "col" } }, this._daysTableHeaderRow);
            var dayDiv = $common.createElementFromTemplate({
                nodeName: "div",
                cssClasses: ["ajax__calendar_dayname"]
            }, dayCell);
        }

        for (var i = 0; i < 6; i++) {
            var daysRow = $common.createElementFromTemplate({ nodeName: "tr" }, this._daysBody);

            for (var j = 0; j < 7; j++) {
                var dayCell = $common.createElementFromTemplate({ nodeName: "td" }, daysRow);
                var dayDiv = $common.createElementFromTemplate({
                    nodeName: "div",
                    properties: {
                        mode: "day",
                        tabIndex: 0,
                        innerHTML: "&nbsp;"
                    },
                    events: this._cell$delegates,
                    cssClasses: ["ajax__calendar_day"]
                }, dayCell);

                if (i == 0 && j == 0) {
                    dayDiv.id = this._beginDayID;
                } else if (i == 5 && j == 6) {
                    dayDiv.id = this._endDayID;
                }
            }
        }
    },
    //Builds a "months of the year" view for the calendar
    _buildMonths: function() {

        var dtf = GetCalendarCultureInfo(this._isHijriCalendar).dateTimeFormat;
        this._months = $common.createElementFromTemplate({
            nodeName: "div",
            cssClasses: ["ajax__calendar_months"],
            visible: false
        }, this._body);
        this._modes["months"] = this._months;

        this._monthsTable = $common.createElementFromTemplate({
            nodeName: "table",
            properties: {
                cellPadding: 0,
                cellSpacing: 0,
                border: 0,
                style: { margin: "auto" }
            }
        }, this._months);

        $(this._monthsTable).attr("role", "presentation");
        this._monthsBody = $common.createElementFromTemplate({ nodeName: "tbody" }, this._monthsTable);

        for (var i = 0; i < 3; i++) {
            var monthsRow = $common.createElementFromTemplate({ nodeName: "tr" }, this._monthsBody);

            for (var j = 0; j < 4; j++) {
                var monthCell = $common.createElementFromTemplate({ nodeName: "td" }, monthsRow);
                var monthDiv = $common.createElementFromTemplate({
                    nodeName: "div",
                    properties: {
                        mode: "month",
                        tabIndex: 0,
                        month: (i * 4) + j,
                        innerHTML: "<br />" + dtf.AbbreviatedMonthNames[(i * 4) + j]
                    },
                    events: this._cell$delegates,
                    cssClasses: ["ajax__calendar_month"]
                }, monthCell);

                if (i == 0 && j == 0) {
                    monthDiv.id = this._beginMonthID;
                } else if (i == 2 && j == 3) {
                    monthDiv.id = this._endMonthID;
                }
            }
        }
    },
    //Builds a "years in this decade" view for the calendar
    _buildYears: function() {
        this._years = $common.createElementFromTemplate({
            nodeName: "div",
            cssClasses: ["ajax__calendar_years"],
            visible: false
        }, this._body);
        this._modes["years"] = this._years;

        this._yearsTable = $common.createElementFromTemplate({
            nodeName: "table",
            properties: {
                cellPadding: 0,
                cellSpacing: 0,
                border: 0,
                style: { margin: "auto" }
            }
        }, this._years);

        $(this._yearsTable).attr("role", "presentation");
        this._yearsBody = $common.createElementFromTemplate({ nodeName: "tbody" }, this._yearsTable);

        for (var i = 0; i < 3; i++) {
            var yearsRow = $common.createElementFromTemplate({ nodeName: "tr" }, this._yearsBody);

            for (var j = 0; j < 4; j++) {
                var yearCell = $common.createElementFromTemplate({ nodeName: "td" }, yearsRow);
                var yearDiv = $common.createElementFromTemplate({
                    nodeName: "div",
                    properties: {
                        mode: "year",
                        tabIndex: 0,
                        year: ((i * 4) + j) - 1
                    },
                    events: this._cell$delegates,
                    cssClasses: ["ajax__calendar_year"]
                }, yearCell);

                if (i == 0 && j == 0) {
                    yearDiv.id = this._beginYearID;
                } else if (i == 2 && j == 3) {
                    yearDiv.id = this._endYearID;
                }
            }
        }
    },
    //Updates the various views of the calendar to match the current selected and visible dates
    _performLayout: function() {
        var elt = this.get_element();

        if (!elt) return;

        if (!this.get_isInitialized()) return;

        if (!this._isOpen) return;

        var dtf = GetCalendarCultureInfo(this._isHijriCalendar).dateTimeFormat;

        var visibleDate = this._getEffectiveVisibleDate();
        var todaysDate = this.get_todaysDate();

        switch (this._mode) {
            case "days":

                var firstDayOfWeek = this._getFirstDayOfWeek();
                var daysToBacktrack = visibleDate.getDay() - firstDayOfWeek;

                if (daysToBacktrack <= 0)
                    daysToBacktrack += 7;

                var startDate = new Date(visibleDate.getFullYear(true), visibleDate.getMonth(true), visibleDate.getDate(true) - daysToBacktrack);
                var currentDate = startDate;

                for (var i = 0; i < 7; i++) {
                    var dayCell = this._daysTableHeaderRow.cells[i].firstChild;

                    if (dayCell.firstChild) {
                        dayCell.removeChild(dayCell.firstChild);
                    }
                    dayCell.appendChild(document.createTextNode(dtf.ShortestDayNames[(i + firstDayOfWeek) % 7]));
                }

                for (var week = 0; week < 6; week++) {
                    var weekRow = this._daysBody.rows[week];

                    for (var dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++) {
                        var dayCell = weekRow.cells[dayOfWeek].firstChild;

                        if (dayCell.firstChild) {
                            dayCell.removeChild(dayCell.firstChild);
                        }

                        var adjustDate = new AdjustDate({ isHijriDate: this._isHijriCalendar, gDate: currentDate });
                        dayCell.appendChild(document.createTextNode(adjustDate.getDate()));
                        dayCell.title = adjustDate.localeFormat("D");
                        dayCell.date = adjustDate;
                        $common.removeCssClasses(dayCell.parentNode, ["ajax__calendar_other", "ajax__calendar_active"]);
                        Sys.UI.DomElement.addCssClass(dayCell.parentNode, this._getCssClass(dayCell.date, 'd'));
                        currentDate = new Date(currentDate.getFullYear(true), currentDate.getMonth(true), currentDate.getDate(true) + 1);
                    }
                }

                this._prevArrow.date = new AdjustDate({ isHijriDate: this._isHijriCalendar, year: visibleDate.getFullYear(), month: visibleDate.getMonth() - 1, day: 1 });
                this._nextArrow.date = new AdjustDate({ isHijriDate: this._isHijriCalendar, year: visibleDate.getFullYear(), month: visibleDate.getMonth() + 1, day: 1 });

                if (this._title.firstChild) {
                    this._title.removeChild(this._title.firstChild);
                }

                this._title.appendChild(document.createTextNode(visibleDate.localeFormat("MMMM, yyyy")));
                this._title.date = visibleDate;
                break;
            case "months":

                for (var i = 0; i < this._monthsBody.rows.length; i++) {
                    var row = this._monthsBody.rows[i];

                    for (var j = 0; j < row.cells.length; j++) {
                        var cell = row.cells[j].firstChild;
                        cell.date = new AdjustDate({ isHijriDate: this._isHijriCalendar, year: visibleDate.getFullYear(), month: cell.month, day: 1 });
                        $common.removeCssClasses(cell.parentNode, ["ajax__calendar_other", "ajax__calendar_active"]);
                        Sys.UI.DomElement.addCssClass(cell.parentNode, this._getCssClass(cell.date, 'M'));
                    }
                }

                if (this._title.firstChild) {
                    this._title.removeChild(this._title.firstChild);
                }

                this._title.appendChild(document.createTextNode(visibleDate.localeFormat("yyyy")));
                this._title.date = visibleDate;
                this._prevArrow.date = new AdjustDate({ isHijriDate: this._isHijriCalendar, year: visibleDate.getFullYear() - 1, month: 0, day: 1 });
                this._nextArrow.date = new AdjustDate({ isHijriDate: this._isHijriCalendar, year: visibleDate.getFullYear() + 1, month: 0, day: 1 });

                break;
            case "years":
                var minYear = (Math.floor(visibleDate.getFullYear() / 10) * 10);

                for (var i = 0; i < this._yearsBody.rows.length; i++) {
                    var row = this._yearsBody.rows[i];

                    for (var j = 0; j < row.cells.length; j++) {
                        var cell = row.cells[j].firstChild;
                        cell.date = new AdjustDate({ isHijriDate: this._isHijriCalendar, year: minYear + cell.year, month: 0, day: 1 });

                        if (cell.firstChild) {
                            cell.removeChild(cell.lastChild);
                        } else {
                            cell.appendChild(document.createElement("br"));
                        }

                        cell.appendChild(document.createTextNode(minYear + cell.year));
                        $common.removeCssClasses(cell.parentNode, ["ajax__calendar_other", "ajax__calendar_active"]);
                        Sys.UI.DomElement.addCssClass(cell.parentNode, this._getCssClass(cell.date, 'y'));
                    }
                }

                if (this._title.firstChild) {
                    this._title.removeChild(this._title.firstChild);
                }

                this._title.appendChild(document.createTextNode(minYear.toString() + "-" + (minYear + 9).toString()));
                this._title.date = visibleDate;
                this._prevArrow.date = new AdjustDate({ isHijriDate: this._isHijriCalendar, year: minYear - 10, month: 0, day: 1 });
                this._nextArrow.date = new AdjustDate({ isHijriDate: this._isHijriCalendar, year: minYear + 10, month: 0, day: 1 });

                break;
        }

        if (this._today.firstChild) {
            this._today.removeChild(this._today.firstChild);
        }

        this._today.appendChild(document.createTextNode(String.format(this._isHijriCalendar ? "اليوم: {0}" : AjaxControlToolkit.Resources.Calendar_Today, todaysDate.localeFormat("MMMM d, yyyy"))));
        this._today.date = todaysDate;
    },

    _ensureCalendar: function() {

        if (!this._container) {

            var elt = this.get_element();

            this._buildCalendar();
            this._buildHeader();
            this._buildBody();
            this._buildFooter();

            var thePositioningMode = this._isRTL ? AjaxControlToolkit.PositioningMode.BottomRight : AjaxControlToolkit.PositioningMode.BottomLeft;
            this._popupBehavior = new $create(AjaxControlToolkit.PopupBehavior, { parentElement: elt, positioningMode: thePositioningMode }, {}, {}, this._popupDiv);

            if (this._isHijriCalendar) {
                $(this._container).css("direction", "rtl");
            }
        }
    },
    //Attempts to fire the change event on the attached textbox
    _fireChanged: function() {
        var elt = this.get_element();

        if (document.createEventObject) {
            elt.fireEvent("onchange");
        } else if (document.createEvent) {
            var e = document.createEvent("HTMLEvents");
            e.initEvent("change", true, true);
            elt.dispatchEvent(e);
        }
    },
    /*
    Switches the visible month in the days view
    @param date (AdjustDate) The visible date to switch to
    @param dontAnimate (Boolean) Prevents animation from occuring if the control is animated
    */
    _switchMonth: function(date, dontAnimate) {
        // Check _isAnimating to make sure we don't animate horizontally and vertically at the same time
        if (this._isAnimating) {
            return;
        }

        var visibleDate = this._getEffectiveVisibleDate();

        if ((date && date.getFullYear() == visibleDate.getFullYear() && date.getMonth() == visibleDate.getMonth())) {
            dontAnimate = true;
        }

        if (this._animated && !dontAnimate) {
            this._isAnimating = true;

            var newElement = this._modes[this._mode];
            var oldElement = newElement.cloneNode(true);
            this._body.appendChild(oldElement);
            if (visibleDate.compareDate(date)) {

                // animating down
                // the newIndex element is the top
                // the oldIndex element is the bottom (visible)

                // move in, fade in
                $common.setLocation(newElement, { x: -162, y: 0 });
                Sys.UI.DomElement.setVisible(newElement, true);
                this._modeChangeMoveTopOrLeftAnimation.set_propertyKey("left");
                this._modeChangeMoveTopOrLeftAnimation.set_target(newElement);
                this._modeChangeMoveTopOrLeftAnimation.set_startValue(-this._width);
                this._modeChangeMoveTopOrLeftAnimation.set_endValue(0);

                // move out, fade out
                $common.setLocation(oldElement, { x: 0, y: 0 });
                Sys.UI.DomElement.setVisible(oldElement, true);
                this._modeChangeMoveBottomOrRightAnimation.set_propertyKey("left");
                this._modeChangeMoveBottomOrRightAnimation.set_target(oldElement);
                this._modeChangeMoveBottomOrRightAnimation.set_startValue(0);
                this._modeChangeMoveBottomOrRightAnimation.set_endValue(this._width);

            } else {
                // animating up
                // the oldIndex element is the top (visible)
                // the newIndex element is the bottom

                // move out, fade out
                $common.setLocation(oldElement, { x: 0, y: 0 });
                Sys.UI.DomElement.setVisible(oldElement, true);
                this._modeChangeMoveTopOrLeftAnimation.set_propertyKey("left");
                this._modeChangeMoveTopOrLeftAnimation.set_target(oldElement);
                this._modeChangeMoveTopOrLeftAnimation.set_endValue(-this._width);
                this._modeChangeMoveTopOrLeftAnimation.set_startValue(0);

                // move in, fade in
                $common.setLocation(newElement, { x: 162, y: 0 });
                Sys.UI.DomElement.setVisible(newElement, true);
                this._modeChangeMoveBottomOrRightAnimation.set_propertyKey("left");
                this._modeChangeMoveBottomOrRightAnimation.set_target(newElement);
                this._modeChangeMoveBottomOrRightAnimation.set_endValue(0);
                this._modeChangeMoveBottomOrRightAnimation.set_startValue(this._width);
            }
            this._visibleDate = date;
            this.invalidate();

            var endHandler = Function.createDelegate(this, function() {
                this._body.removeChild(oldElement);
                oldElement = null;
                this._isAnimating = false;
                this._modeChangeAnimation.remove_ended(endHandler);
            });
            this._modeChangeAnimation.add_ended(endHandler);
            this._modeChangeAnimation.play();
        } else {
            this._visibleDate = date;
            this.invalidate();
        }
    },
    /*Switches the visible view from "days" to "months" to "years"
    * @param mode (String) The view mode to switch to
    * @param dontAnimate (Boolean) Prevents animation from occuring if the control is animated
    */
    _switchMode: function(mode, dontAnimate) {
        // Check _isAnimating to make sure we don't animate horizontally and vertically at the same time
        if (this._isAnimating || (this._mode == mode)) {
            return;
        }

        var moveDown = this._modeOrder[this._mode] < this._modeOrder[mode];
        var oldElement = this._modes[this._mode];
        var newElement = this._modes[mode];
        this._mode = mode;
        var tempPrevArrowTitle;
        var tempNextArrowTitle;

        switch (this._mode) {
            case "days":
                tempPrevArrowTitle = this._prevArrowTitle.replace(/\{0\}/g, "month");
                tempNextArrowTitle = this._nextArrowTitle.replace(/\{0\}/g, "month");
                break;
            case "months":
                tempPrevArrowTitle = this._prevArrowTitle.replace(/\{0\}/g, "year");
                tempNextArrowTitle = this._nextArrowTitle.replace(/\{0\}/g, "year");
                break;
            case "years":
                tempPrevArrowTitle = this._prevArrowTitle.replace(/\{0\}/g, "ten years");
                tempNextArrowTitle = this._nextArrowTitle.replace(/\{0\}/g, "ten years");
                break;
        }

        $("#" + this._prevArrowID).attr("title", tempPrevArrowTitle);
        $("#" + this._nextArrowID).attr("title", tempNextArrowTitle);

        if (this._animated && !dontAnimate) {
            this._isAnimating = true;

            this.invalidate();

            if (moveDown) {
                // animating down
                // the newIndex element is the top
                // the oldIndex element is the bottom (visible)

                // move in, fade in
                $common.setLocation(newElement, { x: 0, y: -this._height });
                Sys.UI.DomElement.setVisible(newElement, true);
                this._modeChangeMoveTopOrLeftAnimation.set_propertyKey("top");
                this._modeChangeMoveTopOrLeftAnimation.set_target(newElement);
                this._modeChangeMoveTopOrLeftAnimation.set_startValue(-this._height);
                this._modeChangeMoveTopOrLeftAnimation.set_endValue(0);

                // move out, fade out
                $common.setLocation(oldElement, { x: 0, y: 0 });
                Sys.UI.DomElement.setVisible(oldElement, true);

                this._modeChangeMoveBottomOrRightAnimation.set_propertyKey("top");
                this._modeChangeMoveBottomOrRightAnimation.set_target(oldElement);
                this._modeChangeMoveBottomOrRightAnimation.set_startValue(0);
                this._modeChangeMoveBottomOrRightAnimation.set_endValue(this._height);

            } else {
                // animating up
                // the oldIndex element is the top (visible)
                // the newIndex element is the bottom

                // move out, fade out
                $common.setLocation(oldElement, { x: 0, y: 0 });
                Sys.UI.DomElement.setVisible(oldElement, true);
                this._modeChangeMoveTopOrLeftAnimation.set_propertyKey("top");
                this._modeChangeMoveTopOrLeftAnimation.set_target(oldElement);
                this._modeChangeMoveTopOrLeftAnimation.set_endValue(-this._height);
                this._modeChangeMoveTopOrLeftAnimation.set_startValue(0);

                // move in, fade in
                $common.setLocation(newElement, { x: 0, y: 139 });
                Sys.UI.DomElement.setVisible(newElement, true);
                this._modeChangeMoveBottomOrRightAnimation.set_propertyKey("top");
                this._modeChangeMoveBottomOrRightAnimation.set_target(newElement);
                this._modeChangeMoveBottomOrRightAnimation.set_endValue(0);
                this._modeChangeMoveBottomOrRightAnimation.set_startValue(this._height);
            }
            var endHandler = Function.createDelegate(this, function() {
                this._isAnimating = false;
                this._modeChangeAnimation.remove_ended(endHandler);
            });
            this._modeChangeAnimation.add_ended(endHandler);
            this._modeChangeAnimation.play();
        } else {
            this._mode = mode;
            Sys.UI.DomElement.setVisible(oldElement, false);
            this.invalidate();
            Sys.UI.DomElement.setVisible(newElement, true);
            $common.setLocation(newElement, { x: 0, y: 0 });
        }
    },
    /* Gets whether the supplied date is the currently selected date
     * @param date (AdjustDate) The date to match
     * @param part (String) The most significant part of the date to test
     */
    _isSelected: function(date, part) {
        var value = this.get_selectedDate();

        if (!value) return false;

        switch (part) {
            case 'd':
                if (date.getDate() != value.getDate()) return false;
                // goto case 'M';
            case 'M':
                if (date.getMonth() != value.getMonth()) return false;
                // goto case 'y';
            case 'y':
                if (date.getFullYear() != value.getFullYear()) return false;
                break;
        }

        return true;
    },
    /* Gets whether the supplied date is in a different view from the current visible month
     * @param date (AdjustDate) The date to match
     * @param part (String) The most significant part of the date to test
     */
    _isOther: function(date, part) {
        var value = this._getEffectiveVisibleDate();

        switch (part) {
            case 'd':
                return (date.getFullYear() != value.getFullYear() || date.getMonth() != value.getMonth());
            case 'M':
                return false;
            case 'y':
                var minYear = (Math.floor(value.getFullYear() / 10) * 10);
                return date.getFullYear() < minYear || (minYear + 10) <= date.getFullYear();
        }

        return false;
    },
    /* Gets the cssClass to apply to a cell based on a supplied date
    *  @param date (AdjustDate) The date to match
    *  @param part (String) The most significant part of the date to test
    */
    _getCssClass: function(date, part) {
        if (this._isSelected(date, part)) {
            return "ajax__calendar_active";
        } else if (this._isOther(date, part)) {
            return "ajax__calendar_other";
        } else {
            return "";
        }
    },
    _getEffectiveVisibleDate: function() {
        var value = this.get_visibleDate();

        if (value == null)
            value = this.get_selectedDate();

        if (value == null)
            value = this.get_todaysDate();

        return new AdjustDate({ isHijriDate: this._isHijriCalendar, year: value.getFullYear(), month: value.getMonth(), day: 1 });
    },
    //Gets the first day of the week
    _getFirstDayOfWeek: function() {
        if (this.get_firstDayOfWeek() != AjaxControlToolkit.FirstDayOfWeek.Default) {
            return this.get_firstDayOfWeek();
        }

        return GetCalendarCultureInfo(this._isHijriCalendar).dateTimeFormat.FirstDayOfWeek;
    },
    /*
    Converts a text value from the textbox into a date
    @param text (string)The text value to parse
    @return (AdjustDate) The adjust by parse text.
    */
    _parseTextValue: function(text) {
        var value = null;

        if (text) {
            value = Date.parseLocale(text, this.get_format());
        }

        if (value == null) {
            return null;
        } else {
            var adjustDate = new AdjustDate({ isHijriDate: this._isHijriCalendar, gDate: value });
            return adjustDate;
        }
    },
    //Handles the completion of a deferred blur operation
    _onblur: function() {
        this._focus.cancel();
        this.hide();
    },
    //Handles the completion of a deferred focus operation
    _onfocus: function() {
        this._blur.cancel();
        this.get_element().focus();
    },
    /*Handles the focus event of the element
    * @param e (Sys.UI.DomEvent) The arguments for the event
    */
    _element_onfocus: function(e) {
        if (this._enabled && this._button == null) {
            this._focus.cancel();
            this._blur.cancel();
            this.show();
        }
    },
    /*Handles the blur event of the element
    * @param e (Sys.UI.DomEvent) The arguments for the event
    */
    _element_onblur: function(e) {
        if ((e.type == 'blur' && Sys.Browser.agent != Sys.Browser.InternetExplorer) ||
            (e.type == 'focusout' && Sys.Browser.agent == Sys.Browser.InternetExplorer)) {

            if (this._button == null) {
                this._focus.cancel();
                this._blur.post();
            }
        }

        if (this._isHijriCalendar) {
            var elt = $(this.get_element());
            var mask = elt.attr("DateMask").replace(new RegExp("9", "gm"), " ");

            if (elt.val() == mask) {
                $(elt).attr(this.gDateAttr, "");
                this._selectedDate = null;
            }
        }
    },
    /*Handles the change event of the element
    * @param e (Sys.UI.DomEvent) The arguments for the event
    */
    _element_onchange: function(e) {
        if (!this._selectedDateChanging) {
            this._selectedDate = this._parseTextValue(this._get_element_value());
            this._switchMonth(this._selectedDate, this._selectedDate == null);
        }
    },
    /* Handles the focus event of the popup
     * @param e (Sys.UI.DomEvent) The arguments for the event
     */
    _popup_onfocus: function(e) {
        if ((e.type == 'focus' && Sys.Browser.agent != Sys.Browser.InternetExplorer) ||
            (e.type == 'activate' && Sys.Browser.agent == Sys.Browser.InternetExplorer) ||
            (Sys.Browser.agent === Sys.Browser.Safari) ||
            (Sys.Browser.agent === Sys.Browser.Opera)) {
            if (this._button == null) {
                this._blur.cancel();
                this._focus.post();
            }
        }
    },
    /* Handles the drag-start event of the popup calendar
     * @param e (Sys.UI.DomEvent) The arguments for the event
     */
    _popup_ondragstart: function(e) {
        e.stopPropagation();
        e.preventDefault();
    },
    /*  Handles the select event of the popup calendar
     *  @param e (Sys.UI.DomEvent) The arguments for the event
     */
    _popup_onselect: function(e) {
        e.stopPropagation();
        e.preventDefault();
    },
    /* Handles the mouseover event of a cell
    *  @param e (Sys.UI.DomEvent) The arguments for the event
    */
    _cell_onmouseover: function(e) {
        if (Sys.Browser.agent === Sys.Browser.Safari) {

            // Safari doesn't reliably call _cell_onmouseout, so clear other cells here to keep the UI correct
            for (var i = 0; i < this._daysBody.rows.length; i++) {
                var row = this._daysBody.rows[i];

                for (var j = 0; j < row.cells.length; j++) {
                    Sys.UI.DomElement.removeCssClass(row.cells[j].firstChild.parentNode, "ajax__calendar_hover");
                }
            }
        }

        var target = e.target;

        Sys.UI.DomElement.addCssClass(target.parentNode, "ajax__calendar_hover");

        e.stopPropagation();
    },
    /*  Handles the mouseout event of a cell
    *  @param e (Sys.UI.DomEvent) The arguments for the event
    */
    _cell_onmouseout: function(e) {
        var target = e.target;

        Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");

        e.stopPropagation();
    },
    /* Handles the keydown event of a cell
     *  @param e (Sys.UI.DomEvent) The arguments for the event
     */
    _cell_onkeydown: function (e) {
        var target = e.target;

        // Tab: 9, Enter: 13, Escape: 27, Space: 32
        switch (e.keyCode) {
            case 9:
                if (e.shiftKey) {

                    if (target.id == this._beginDayID || target.id == this._beginMonthID || target.id == this._beginYearID) {
                        // Focus on the last object in popup's header.
                        FocusObject(e, this._endFocusIDOfHeader);
                    } else if (target.id == this._beginFocusIDOfFooter) {
                        // When current object is first in popup's footer,
                        switch (this._mode) {
                            case "days":
                                // Focus on the last day in popup's body.
                                FocusObject(e, this._endDayID);
                                break;
                            case "months":
                                // Focus on the last month in popup's body.
                                FocusObject(e, this._endMonthID);
                                break;
                            case "years":
                                // Focus on the last year in popup's body.
                                FocusObject(e, this._endYearID);
                                break;
                        }
                    } else if (target.id == this._endFocusID) {
                        FocusObject(e, this._beginFocusIDOfFooter);
                    } else if (target.id == this._beginFocusID) {
                        FocusObject(e, this._endFocusID);
                    }
                } else {
                    if (target.id == this._endFocusID) {
                        // When current object is last in popup, focus on the first object in popup.
                        FocusObject(e, this._beginFocusID);
                    } else if (target.id == this._endFocusIDOfHeader) {
                        // When current object is last in popup's header,
                        e.preventDefault();

                        switch (this._mode) {
                            case "days":
                                // Focus on the first day in popup.
                                $("#" + this._id + " .ajax__calendar_day").first().focus();
                                break;
                            case "months":
                                // Focus on the first month in popup.
                                $("#" + this._id + " .ajax__calendar_month").first().focus();
                                break;
                            case "years":
                                // Focus on the first year in popup.
                                $("#" + this._id + " .ajax__calendar_year").first().focus();
                                break;
                        }
                    } else if (target.id == this._endDayID || target.id == this._endMonthID || target.id == this._endYearID) {
                        // Focus on popup's footer.
                        FocusObject(e, this._beginFocusIDOfFooter);
                    } else if (target.id == this._beginFocusIDOfFooter) {
                        FocusObject(e, this._endFocusID);
                    }
                }

                break;
            case 13:
                e.preventDefault();

                // Select the focused date
                this._cell_onclick(e);
                break;
            case 27:
                this.hide();

                // If use Esc to close the date picker, Focus button if exists, Otherwise focus textbox.
                if (this._button != null) {
                    this._button.focus();
                } else {
                    this.get_element().focus();
                }

                break;
            case 32:
                // Select the focused date
                this._cell_onclick(e);
                break;
        }

        e.stopPropagation();
    },
    /*  Handles the click event of a cell
     *  @param e (Sys.UI.DomEvent) The arguments for the event
     */
    _cell_onclick: function(e) {
        if ((Sys.Browser.agent === Sys.Browser.Safari) ||
            (Sys.Browser.agent === Sys.Browser.Opera)) {
            // _popup_onfocus doesn't get called on Safari or Opera, so we call it manually now
            this._popup_onfocus(e);
        }

        if (!this._enabled)
            return;

        var target = e.target;
        var visibleDate = this._getEffectiveVisibleDate();
        Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");
        switch (target.mode) {
            case "prev":
            case "next":
                this._switchMonth(target.date);
                break;
            case "title":
                switch (this._mode) {
                    case "days": this._switchMode("months"); break;
                    case "months": this._switchMode("years"); break;
                }
                break;
            case "month":
                if (target.month == visibleDate.getMonth()) {
                    this._switchMode("days");
                } else {
                    this._visibleDate = target.date;
                    this._switchMode("days");
                }

                $("#" + this._endFocusIDOfHeader).focus();

                break;
            case "year":
                if (target.date.getFullYear() == visibleDate.getFullYear()) {
                    this._switchMode("months");
                } else {
                    this._visibleDate = target.date;
                    this._switchMode("months");
                }

                $("#" + this._endFocusIDOfHeader).focus();

                break;
            case "day":
                this.set_selectedDate(target.date);
                this._switchMonth(target.date);
                if (this._button != null) {
                    this.hide();
                }
                this.get_element().focus();
                this.raiseDateSelectionChanged();
                break;
            case "today":
                this.set_selectedDate(target.date);
                this._switchMonth(target.date);
                if (this._button != null) {
                    this.hide();
                }
                this.get_element().focus();
                this.raiseDateSelectionChanged();
                break;
        }

        e.stopPropagation();
        e.preventDefault();
    },

    // Handler for the HTML body tag's click event
    _onBodyClick: function() {
        // Hide the popup if something other than our target or popup was clicked 
        if (this._isOpen && this._button != null) {
            this.hide();
        }
    },
    /* Handles the click event of the asociated button
	   @param  e  The arguments for the event */
    _button_onclick: function(e) {
        if (this._enabled) {
            e.preventDefault();
            e.stopPropagation();
        }

        if (!this._isOpen) {
            if (this._enabled) {
                this.show();

                if (e) {
                    e.stopPropagation();
                }
            }
        } else {
            this.hide();
        }
    },
    _get_element_value: function () {
        var elt = this.get_element();

        //If current calendar is hijri calendar 
        if (this._isHijriCalendar) {
            return $(elt).attr(this.gDateAttr);
        }
        
        var wrapper = AjaxControlToolkit.TextBoxWrapper.get_Wrapper(elt);

        if (wrapper && wrapper.get_IsWatermarked()) {
            return '';
        }
        else {
            return elt.value;
        }
    },
    _set_element_value: function (value) {
        var elt = this.get_element();
        var watermarkBhv = Sys.UI.Behavior.getBehaviorByName(elt, 'TextBoxWatermarkBehavior');

        if (watermarkBhv) {
            watermarkBhv._onFocus();
        }

        elt.value = value.localeFormat(this._format);

        //If current calendar is hijri calendar, must refresh hide text of gregorian date after updated hijri date.
        if (this._isHijriCalendar) {
            $(elt).attr(this.gDateAttr, value.getGregorianDateText(this._format));
        }

        if (watermarkBhv) {
            watermarkBhv._onBlur();
        }
    },
};

AjaxControlToolkit.CalendarBehavior.registerClass("AjaxControlToolkit.CalendarBehavior", AjaxControlToolkit.BehaviorBase);
