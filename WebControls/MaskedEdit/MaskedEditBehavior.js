/**
 * <pre>
 * 
 *  Accela
 *  File: MaskedEditBehavior.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: MaskedEditBehavior.js 77775 2007-10-12 07:28:17Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.
// Product      : MaskedEdit Extend Control
// Version      : 1.0.0.0
// Date         : 10/23/2006
// Development  : Fernando Cerqueira 
// last rev.    : 01/18/2007 Fernando Cerqueira 
// 

Type.registerNamespace('AjaxControlToolkit');
// **************************************************
// MaskedEdit Control
// **************************************************
AjaxControlToolkit.MaskedEditBehavior = function(element) 
{
    AjaxControlToolkit.MaskedEditBehavior.initializeBase(this, [element]);

    // **************************************************
    // Properties
    // **************************************************
    // mask
    this._Mask = "";
    this._MaskType = AjaxControlToolkit.MaskedEditType.None;
    this._Filtered = "";
    this._PromptChar = "_";
    // Indicate whether show star characters in this element's value
    this._ShowStarChar = false;
    this._InputDirection = AjaxControlToolkit.MaskedEditInputDirections.LeftToRight;
    // Message
    this._MessageValidatorTip = true;
    // AutoComplete
    this._AutoComplete = true;
    this._AutoCompleteValue = "";
    // behavior
    this._ClearTextOnInvalid = false;
    this._ClearMaskOnLostfocus = true;
    this._AcceptAmPm = AjaxControlToolkit.MaskedEditShowSymbol.None;
    this._AcceptNegative = AjaxControlToolkit.MaskedEditShowSymbol.None;
    this._DisplayMoney = AjaxControlToolkit.MaskedEditShowSymbol.None;
    // CSS
    this._OnFocusCssClass = "MaskedEditFocus";
    this._OnInvalidCssClass = "MaskedEditError";
    this._OnFocusCssNegative = "MaskedEditFocusNegative";
    this._OnBlurCssNegative = "MaskedEditBlurNegative";
    // globalization 
    this._CultureName = "en-US";
    // globalization Hidden 
    this._CultureDatePlaceholder = "/";
    this._CultureTimePlaceholder = ":";
    this._CultureDecimalPlaceholder = ".";
    this._CultureThousandsPlaceholder = ",";
    this._CultureDateFormat = "MDY";
    this._CultureCurrencySymbolPlaceholder = "$";
    this._CultureAMPMPlaceholder = "AM;PM";
    this._Century = 1900;
    // **************************************************
    // local var mask valid
    // **************************************************
    //  9 = only numeric
    //  L = only letter
    //  u = only lower letter
    //  U = only upper letter
    //  S = only numeric and letter
    //  $ = only letter and spaces
    //  C = only custom - read from this._Filtered
    //  A = only letter and custom
    //  N = only numeric and custom
    //  ? = any digit
    this._CharsEditMask = "9LuUS$CAN?";
    // **************************************************
    // local var special mask
    // **************************************************
    // at runtime replace with culture property
    //  / = Date placeholder
    //  : = Time placeholder
    //  . = Decimal placeholder
    //  , = Thousands placeholder
    this._CharsSpecialMask = "/:.,";
    // **************************************************
    // local converted mask 
    // **************************************************
    // i.e.: 9{2} => 99 , 9{2}/9{2}/9{2} = 99/99/99
    this._MaskConv = "";
    // **************************************************
    // Others local Var
    // **************************************************
    // save the Direction selected Text (only for ie)
    this._DirectSelText = "";
    // save the initial value for verify changed
    this._initialvalue = "";
    // save the symbol Negative or AM/PM 
    this._LogicSymbol = "";
    // save logic mask with text input
    this._LogicTextMask = "";
    // save logic mask without text
    this._LogicMask = "";
    // save logic mask without text and without escape
    this._LogicMaskConv = "";
    // ID prompt char
    this._LogicPrompt = String.fromCharCode(1);
    // ID escape char
    this._LogicEscape = String.fromCharCode(2);
    // first valid position
    this._LogicFirstPos = -1;
    // Last valid position 
    this._LogicLastPos = -1;
    // Qtd Valid input Position
    this._QtdValidInput = 0;
    // Flag to validate in lost focus not duplicate clearMask execute
    this._InLostfocus = false;
    // Save local MessageError from Controls Validator
    this._ExternalMessageError = "";
    // Save local Current MessageError
    this._CurrentMessageError = "";
    // **************************************************
    // local chars ANSI
    // **************************************************
    this._charLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    this._charNumbers = "0123456789";
    this._charEscape = "\\";
    // **************************************************
    // local placeholder delimit for info repeat mask
    // **************************************************
    this._DelimitStartDup = "{";
    this._DelimitEndDup = "}";
    // **************************************************
    // Handler
    // **************************************************
    this._focusHandler = null;
    this._keypressdown = null;
    this._keypressHandler = null;
    this._blurHandler = null;
    this._clickHandler = null;
    // **************************************************
    // end Declaration
    // **************************************************
}
AjaxControlToolkit.MaskedEditBehavior.prototype = {
    initialize: function () {
        var e = this.get_element();

        this._InLostfocus = true;
        // TODO: add your initialization code here         
        AjaxControlToolkit.MaskedEditBehavior.callBaseMethod(this, 'initialize');

        // if this textbox is focused initially
        var hasInitialFocus = false;
        var clientState = this.get_ClientState();

        if (clientState != null && clientState != "") {
            hasInitialFocus = (clientState == "Focused");
            this.set_ClientState(null);
        }

        //only for ie , for firefox see keydown
        if (typeof (document.activeElement) != "unknown" && document.activeElement) {
            if (e.id == document.activeElement.id) {
                hasInitialFocus = true;
            }
        }

        // Create delegates Attach events
        this._focusHandler = Function.createDelegate(this, this._onFocus);
        this._clickHandler = Function.createDelegate(this, this._onClick);

        // Safari doesn't support set curson position on focus event.
        if (Sys.Browser.agent == Sys.Browser.Safari) {
            $addHandler(e, "click", this._clickHandler);
        }
        else {
            $addHandler(e, "focus", this._focusHandler);
        }

        this._keypressdown = Function.createDelegate(this, this._onKeyPressdown);
        $addHandler(e, "keydown", this._keypressdown);

        this._keypressHandler = Function.createDelegate(this, this._onKeyPress);
        $addHandler(e, "keypress", this._keypressHandler);

        this._blurHandler = Function.createDelegate(this, this._onBlur);
        $addHandler(e, "blur", this._blurHandler);

        if (hasInitialFocus) {
            if (Sys.Browser.agent == Sys.Browser.Safari) {
                this._onClick();
            }
            else {
                this._onFocus();
            }
        }
        else if (e.value != "") {
            this._InitValue();
            if (this._ClearMaskOnLostfocus) {
                e.value = (this._getClearMask(e.value));
            }
        }
    }
    //
    // Init value startup
    //
    , _InitValue: function () {
        var masktxt = this._createMask();
        var Inipos = this._LogicFirstPos;
        var initValue = "";
        var e = this.get_element();
        this._LogicSymbol = "";

        if (e.value != "" && e.value != masktxt) {
            initValue = e.value;
        }
        e.value = (masktxt);
        if (initValue != "") {
            if (this._MaskType == AjaxControlToolkit.MaskedEditType.Date) {
                initValue = this.ConvFmtDate(initValue);
            }
            if (this._InputDirection == AjaxControlToolkit.MaskedEditInputDirections.LeftToRight) {
                this.loadValue(initValue, this._LogicFirstPos);
            }
            else if (this._InputDirection == AjaxControlToolkit.MaskedEditInputDirections.RightToLeft) {
                this.loadValue(initValue, this._LogicLastPos);
            }
            if (this._MaskType == AjaxControlToolkit.MaskedEditType.Number) {
                if (this._InLostfocus && this._LogicSymbol == "-" && this._OnBlurCssNegative != "") {
                    this.AddCssClassMaskedEdit(this._OnBlurCssNegative);
                }
            }
        }
        return Inipos;
    }
    //
    // convert format Date
    //
    , ConvFmtDate: function (input) {
        var e = this.get_element();
        var maskvalid = this._MaskConv.substring(this._LogicFirstPos, this._LogicFirstPos + this._LogicLastPos + 1);
        var m_arrDate = input.split(this._CultureDatePlaceholder);
        var m_mask = maskvalid.split(this._CultureDatePlaceholder);
        if (parseInt(m_arrDate.length, 10) != 3) {
            return "";
        }
        var D = m_arrDate[this._CultureDateFormat.indexOf("D")];
        if (D.length < m_mask[this._CultureDateFormat.indexOf("D")].length) {
            while (D.length < m_mask[this._CultureDateFormat.indexOf("D")].length) {
                D = "0" + D;
            }
        }
        m_arrDate[this._CultureDateFormat.indexOf("D")] = D;
        var M = m_arrDate[this._CultureDateFormat.indexOf("M")];
        if (M.length < m_mask[this._CultureDateFormat.indexOf("M")].length) {
            while (M.length < m_mask[this._CultureDateFormat.indexOf("M")].length) {
                M = "0" + M;
            }
        }
        m_arrDate[this._CultureDateFormat.indexOf("M")] = M;
        var Y = m_arrDate[this._CultureDateFormat.indexOf("Y")];
        if (m_mask[this._CultureDateFormat.indexOf("Y")].length == 4) {
            Y = this._AdjustElementDateY(Y, this._Century.toString());
        }
        else {
            Y = this._AdjustElementDateY(Y, this._Century.toString().substring(2));
        }
        m_arrDate[this._CultureDateFormat.indexOf("Y")] = Y;
        return m_arrDate[0] + this._CultureDatePlaceholder + m_arrDate[1] + this._CultureDatePlaceholder + m_arrDate[2];
    }
    //
    // Set/Remove CssClass
    //
    , AddCssClassMaskedEdit: function (CssClass) {
        var e = this.get_element();
        Sys.UI.DomElement.removeCssClass(e, this._OnBlurCssNegative);
        Sys.UI.DomElement.removeCssClass(e, this._OnFocusCssClass);
        Sys.UI.DomElement.removeCssClass(e, this._OnFocusCssNegative);
        Sys.UI.DomElement.removeCssClass(e, this._OnInvalidCssClass);
        if (CssClass != "") {
            Sys.UI.DomElement.addCssClass(e, CssClass);
        }
    }
    //
    // Load initial value in mask
    //
    , loadValue: function (initValue, logicPosition) {
        if (this._InputDirection == AjaxControlToolkit.MaskedEditInputDirections.LeftToRight) {
            var oldfocus = this._InLostfocus;
            var i = 0;
            this._InLostfocus = false;
            if (this._ClearMaskOnLostfocus == false) {
                logicPosition = 0;
            }
            for (i = 0; i < parseInt(initValue.length, 10); i++) {
                var c = initValue.substring(i, i + 1);
                if (this._MaskType == AjaxControlToolkit.MaskedEditType.Time && this.get_CultureFirstLettersAMPM().toUpperCase().indexOf(c.toUpperCase()) != -1) {
                    if (this._AcceptAmPm) {
                        this.InsertAMPM(c);
                    }
                }
                else if (this._MaskType == AjaxControlToolkit.MaskedEditType.Number && this._AcceptNegative != AjaxControlToolkit.MaskedEditShowSymbol.None && "+-".indexOf(c) != -1) {
                    this.InsertSignal(c);
                }
                if ((this._ShowStarChar && c == "*") || this._processKey(logicPosition, c)) {
                    // Let the key is valid
                    c = this._fixKey(logicPosition, c);
                    this._insertContent(c, logicPosition);
                    if (this._ClearMaskOnLostfocus == false) {
                        logicPosition = logicPosition + 1;
                    }
                    else {
                        logicPosition = this._getNextPosition(logicPosition + 1);
                    }
                }
                else {
                    if (this._ClearMaskOnLostfocus == false) {
                        logicPosition = logicPosition + 1;
                    }
                }
            }
            this._InLostfocus = oldfocus;

        }
        else if (this._InputDirection == AjaxControlToolkit.MaskedEditInputDirections.RightToLeft) {
            var oldfocus = this._InLostfocus;
            this._InLostfocus = false;
            if (this._ClearMaskOnLostfocus == false) {
                logicPosition = parseInt(this.get_element().value.length, 10) - 1;
            }
            for (i = parseInt(initValue.length, 10); i > 0; i--) {
                var c = initValue.substring(i - 1, i);
                if (this._MaskType == AjaxControlToolkit.MaskedEditType.Time && this.get_CultureFirstLettersAMPM().toUpperCase().indexOf(c.toUpperCase()) != -1) {
                    if (this._AcceptAmPm) {
                        this.InsertAMPM(c);
                    }
                }
                else if (this._MaskType == AjaxControlToolkit.MaskedEditType.Number && this._AcceptNegative != AjaxControlToolkit.MaskedEditShowSymbol.None && "+-".indexOf(c) != -1) {
                    this.InsertSignal(c);
                }
                if ((this._ShowStarChar && c == "*") || this._processKey(logicPosition, c)) {
                    // Let the key is valid
                    c = this._fixKey(logicPosition, c);
                    this._insertContent(c, logicPosition);
                    if (this._ClearMaskOnLostfocus == false) {
                        logicPosition = logicPosition - 1;
                    }
                    else {
                        logicPosition = this._getPreviousPosition(logicPosition - 1);
                    }
                }
                else {
                    if (this._ClearMaskOnLostfocus == false) {
                        logicPosition = logicPosition - 1;
                    }
                }
            }
            this._InLostfocus = oldfocus;
        }
    }
    //
    // Detach events this.dispose
    //
    , dispose: function () {
        var e = this.get_element();
        // restore maxLength
        if (this._focusHandler) {
            if (Sys.Browser.agent == Sys.Browser.Safari) {
                $removeHandler(e, "click", this._clickHandler);
            }
            else {
                $removeHandler(e, "focus", this._focusHandler);
            }

            this._focusHandler = null;
        }
        if (this._blurHandler) {
            $removeHandler(e, "blur", this._blurHandler);
            this._blurHandler = null;
        }
        if (this._keypressdown) {
            $removeHandler(e, "keydown", this._keypressdown);
            this._keypressdown = null;
        }
        if (this._keypressHandler) {
            $removeHandler(e, "keypress", this._keypressHandler);
            this._keypressHandler = null;
        }
        AjaxControlToolkit.MaskedEditBehavior.callBaseMethod(this, 'dispose');
    }
    //
    // Check Event Argumet
    //
    , _CheckArgsEvents: function (args) {
        var ret = null;
        if (typeof (args) != "undefined" && args != null && typeof (args.rawEvent) != "undefined") {
            ret = args.rawEvent;
        }
        return ret;
    }
    //
    // attachEvent focus
    //
    , _onFocus: function (args) {
        var evt = this._CheckArgsEvents(args);

        var _behavior = Sys.UI.Behavior.getBehaviorByName(this.get_element(), 'TextBoxWatermarkBehavior');
        if (_behavior && AjaxControlToolkit.TextBoxWrapper.get_Wrapper(this.get_element()).get_IsWatermarked()) {
            _behavior._onFocus(evt);
        }

        var originalValue = args && args.target && args.target.value ? args.target.value.replace(new RegExp('-', "gm"), '') : "";
        originalValue = originalValue.replace(new RegExp(" ", "gm"), '');
        var e = this.get_element();
        this._InLostfocus = false;
        if (this._OnFocusCssClass != "") {
            this.AddCssClassMaskedEdit(this._OnFocusCssClass);
        }
        var Inipos = this._InitValue();
        var ClearText = this._getClearMask(e.value);
        this._initialvalue = originalValue;
        var hastip = false;
        if (this._MessageValidatorTip && ClearText == "") {
            hastip = true;
        }
        if (this._MaskType == AjaxControlToolkit.MaskedEditType.Time && this._CultureAMPMPlaceholder != "" && ClearText == "") {
            if (this._AcceptAmPm) {
                this.InsertAMPM("A");
            }
        }
        else if (this._MaskType == AjaxControlToolkit.MaskedEditType.Number && ClearText != "") {
            if (this._LogicSymbol == "-" && this._OnFocusCssNegative != "") {
                this.AddCssClassMaskedEdit(this._OnFocusCssNegative);
            }
        }
        this.setSelectionRange(Inipos, Inipos);
        this.ShowTooltipMessage(false);
        if (hastip) {
            this.ShowTooltipMessage(true);
        }
    }
    //
    // attachEvent click
    //
    , _onClick: function (args) {
        if (!this._InLostfocus) {
            return;
        }

        var evt = this._CheckArgsEvents(args);

        var _behavior = Sys.UI.Behavior.getBehaviorByName(this.get_element(), 'TextBoxWatermarkBehavior');
        if (_behavior && AjaxControlToolkit.TextBoxWrapper.get_Wrapper(this.get_element()).get_IsWatermarked()) {
            _behavior._onFocus(evt);
        }

        var originalValue = args.target.value.replace(new RegExp('-', "gm"), '');
        originalValue = originalValue.replace(new RegExp(" ", "gm"), '');
        var e = this.get_element();
        this._InLostfocus = false;
        if (this._OnFocusCssClass != "") {
            this.AddCssClassMaskedEdit(this._OnFocusCssClass);
        }
        //e.focus();
        var Inipos = this._InitValue();
        var ClearText = this._getClearMask(e.value);
        this._initialvalue = originalValue;
        var hastip = false;
        if (this._MessageValidatorTip && ClearText == "") {
            hastip = true;
        }
        if (this._MaskType == AjaxControlToolkit.MaskedEditType.Time && this._CultureAMPMPlaceholder != "" && ClearText == "") {
            if (this._AcceptAmPm) {
                this.InsertAMPM("A");
            }
        }
        else if (this._MaskType == AjaxControlToolkit.MaskedEditType.Number && ClearText != "") {
            if (this._LogicSymbol == "-" && this._OnFocusCssNegative != "") {
                this.AddCssClassMaskedEdit(this._OnFocusCssNegative);
            }
        }

        //set cursor position.
        this.setSelectionRange(Inipos, Inipos);

        this.ShowTooltipMessage(false);
        if (hastip) {
            this.ShowTooltipMessage(true);
        }
    }

    //
    // Show/hide Message Tip
    //
    , ShowTooltipMessage: function (Visible) {
        if (typeof (Page_Validators) == "undefined") {
            return;
        }
        var msg = "";
        if (!Visible) {
            msg = this._CurrentMessageError;
            this._CurrentMessageError = "";
        }
        var i = 0
        var ctrval = null;
        for (i = 0; i < Page_Validators.length; i++) {
            ctrval = Page_Validators[i];
            if (ctrval.getAttribute("TargetValidator") == this.get_element().id && ctrval.getAttribute("IsMaskedEdit") == "true") {
                if (!Visible) {
                    ctrval.innerHTML = msg;
                    if (typeof (ctrval.display) == "string") {
                        if (ctrval.display == "None") {
                            return;
                        }
                        if (ctrval.display == "Dynamic") {
                            ctrval.style.display = ctrval.isvalid ? "none" : "inline";
                            return;
                        }
                    }
                    ctrval.style.visibility = ctrval.isvalid ? "hidden" : "visible";
                    return;
                }
                this._CurrentMessageError = ctrval.innerHTML;
                ctrval.innerHTML = ctrval.getAttribute("TooltipMessage");
                if (typeof (ctrval.display) == "string") {
                    if (ctrval.display == "None") {
                        return;
                    }
                    if (ctrval.display == "Dynamic") {
                        ctrval.style.display = "inline";
                        return;
                    }
                }
                ctrval.style.visibility = "visible";
                return;
            }
        }
    }
    //
    // Adjust element for auto complete
    //
    , _AdjustElementDateY: function (value, ValueDefault) {
        var emp = true;
        for (i = 0; i < parseInt(value.length, 10); i++) {
            if (value.substring(i, i + 1) != this._PromptChar) {
                emp = false;
            }
        }
        if (emp) {
            return ValueDefault;
        }
        for (i = 0; i < parseInt(value.length, 10); i++) {
            if (value.substring(i, i + 1) == this._PromptChar) {
                value = "0" + value.substring(0, i) + value.substring(i + 1);
            }
        }
        if (parseInt(value, 10) == 0) {
            if (ValueDefault.length == 2) {
                value = this._Century.toString().substring(2);
            }
            else {
                //year
                value = this._Century.toString();
            }
        }
        return value;
    }
    , _AdjustElementDateDM: function (value, ValueDefault) {
        var emp = true;
        for (i = 0; i < parseInt(value.length, 10); i++) {
            if (value.substring(i, i + 1) != this._PromptChar) {
                emp = false;
            }
        }
        if (emp) {
            return ValueDefault;
        }
        for (i = 0; i < parseInt(value.length, 10); i++) {
            if (value.substring(i, i + 1) == this._PromptChar) {
                value = value.substring(0, i) + "0" + value.substring(i + 1);
            }
        }
        if (parseInt(value, 10) == 0) {
            value = "01";
        }
        return value;
    }
    , _AdjustElementTime: function (value, ValueDefault) {
        var emp = true;
        for (i = 0; i < parseInt(value.length, 10); i++) {
            if (value.substring(i, i + 1) != this._PromptChar) {
                emp = false;
            }
        }
        if (emp) {
            return ValueDefault;
        }
        for (i = 0; i < parseInt(value.length, 10); i++) {
            if (value.substring(i, i + 1) == this._PromptChar) {
                value = value.substring(0, i) + "0" + value.substring(i + 1);
            }
        }
        return value;
    }
    //
    // attachEvent Blur
    //
    , _onBlur: function (args) {
        var evt = this._CheckArgsEvents(args);

        var isHijriCalendar = false;
        this._InLostfocus = true;
        ValueText = this.get_element().value;
        ClearText = this._getClearMask(ValueText);
        if (ClearText == "" && this._MaskType == AjaxControlToolkit.MaskedEditType.Number && this._LogicSymbol == "-") {
            this.InsertSignal("+");
        }
        // auto format empty text Time
        if (ClearText != "" && this._AutoComplete && this._MaskType == AjaxControlToolkit.MaskedEditType.Time) {
            var CurDate = new Date();
            var Hcur = CurDate.getHours().toString();
            if (Hcur.length < 2) {
                Hcur = "0" + Hcur;
            }
            if (this._AutoCompleteValue != "") {
                Hcur = this._AutoCompleteValue.substring(0, 2);
            }
            var Symb = ""
            if (this._CultureAMPMPlaceholder != "") {
                var m_arrtm = this._CultureAMPMPlaceholder.split(";");
                var Symb = m_arrtm[0].substring(0, 1);
                if (Hcur > 12) {
                    Hcur = (parseInt(Hcur, 10) - 12).toString();
                    if (Hcur.length < 2) {
                        Hcur = "0" + Hcur;
                    }
                    Symb = m_arrtm[1].substring(0, 1);
                }
                if (!this._AcceptAmPm) {
                    Symb = "";
                }
            }
            var Mcur = CurDate.getMinutes().toString();
            if (Mcur.length < 2) {
                Mcur = "0" + Mcur;
            }
            if (this._AutoCompleteValue != "") {
                Mcur = this._AutoCompleteValue.substring(3, 2);
            }
            var Scur = CurDate.getSeconds().toString();
            if (Scur.length < 2) {
                Scur = "0" + Scur;
            }
            var maskvalid = this._MaskConv.substring(this._LogicFirstPos, this._LogicFirstPos + this._LogicLastPos + 1);
            var PH = ValueText.substring(this._LogicFirstPos, this._LogicFirstPos + 2);
            PH = this._AdjustElementTime(PH, Hcur);
            var PM = ValueText.substring(this._LogicFirstPos + 3, this._LogicFirstPos + 5);
            PM = this._AdjustElementTime(PM, Mcur);
            if (maskvalid == "99" + this._CultureTimePlaceholder + "99" + this._CultureTimePlaceholder + "99") {
                if (this._AutoCompleteValue != "") {
                    Scur = this._AutoCompleteValue.substring(5);
                }
                PS = ValueText.substring(this._LogicFirstPos + 6, this._LogicLastPos + 1);
                PS = this._AdjustElementTime(PS, Scur);
                ValueText = ValueText.substring(0, this._LogicFirstPos) + PH + this._CultureTimePlaceholder + PM + this._CultureTimePlaceholder + PS + ValueText.substring(this._LogicLastPos + 1);
                this._LogicTextMask = this._LogicTextMask.substring(0, this._LogicFirstPos) + PH + this._CultureTimePlaceholder + PM + this._CultureTimePlaceholder + PS + this._LogicTextMask.substring(this._LogicLastPos + 1);
            }
            else {
                ValueText = ValueText.substring(0, this._LogicFirstPos) + PH + this._CultureTimePlaceholder + PM + ValueText.substring(this._LogicLastPos + 1);
                this._LogicTextMask = this._LogicTextMask.substring(0, this._LogicFirstPos) + PH + this._CultureTimePlaceholder + PM + this._LogicTextMask.substring(this._LogicLastPos + 1);
            }
            this.get_element().value = (ValueText);
            ClearText = this._getClearMask(ValueText);
        }
        // auto format empty text Number
        else if (ClearText != "" && this._AutoComplete && this._MaskType == AjaxControlToolkit.MaskedEditType.Number) {
            for (i = 0; i < parseInt(this._LogicTextMask.length, 10); i++) {
                if (this._LogicTextMask.substring(i, i + 1) == this._LogicPrompt) {
                    this._LogicTextMask = this._LogicTextMask.substring(0, i) + "0" + this._LogicTextMask.substring(i + 1);
                    ValueText = ValueText.substring(0, i) + "0" + ValueText.substring(i + 1);
                }
            }
            var okdgt = false;
            for (i = 0; i < parseInt(this._LogicTextMask.length, 10); i++) {
                if (!okdgt) {
                    if (this._LogicMask.substring(i, i + 1) == this._LogicPrompt && this._LogicTextMask.substring(i, i + 1) == "0") {
                        this._LogicTextMask = this._LogicTextMask.substring(0, i) + this._LogicPrompt + this._LogicTextMask.substring(i + 1);
                        ValueText = ValueText.substring(0, i) + this._PromptChar + ValueText.substring(i + 1);
                    }
                    else if (this._LogicMask.substring(i, i + 1) == this._LogicPrompt && "123456789".indexOf(this._LogicTextMask.substring(i, i + 1)) != -1) {
                        okdgt = true;
                    }
                    else if (this._LogicMask.substring(i, i + 1) != this._LogicPrompt && this._LogicTextMask.substring(i, i + 1) == this._CultureDecimalPlaceholder) {
                        this._LogicTextMask = this._LogicTextMask.substring(0, i - 1) + "0" + this._CultureDecimalPlaceholder + this._LogicTextMask.substring(i + 1);
                        ValueText = ValueText.substring(0, i - 1) + "0" + this._CultureDecimalPlaceholder + ValueText.substring(i + 1);
                        okdgt = true;
                    }
                }
            }
            this.get_element().value = (ValueText);
            ClearText = this._getClearMask(ValueText);
        }
        // auto format empty text Date
        else if (ClearText != "" && this._AutoComplete && this._MaskType == AjaxControlToolkit.MaskedEditType.Date) {
            var maskvalid = this._MaskConv.substring(this._LogicFirstPos, this._LogicFirstPos + this._LogicLastPos + 1);
            var Y4 = (maskvalid.indexOf("9999") != -1) ? true : false;
            var CurDate = new Date();
            var utcDay;
            var utcMonth;
            var utcYear;
            isHijriCalendar = isHijriCalendarControl(this.get_element().id);

            if (isHijriCalendar) {
                var utcDate = new Date(CurDate.getUTCFullYear(), CurDate.getUTCMonth(), CurDate.getUTCDate());
                var adjustDate = new AdjustDate({ isHijriDate: true, gDate: utcDate });
                utcDay = adjustDate.getDate();
                utcMonth = adjustDate.getMonth() + 1;
                utcYear = adjustDate.getFullYear();
            } else {
                utcDay = CurDate.getUTCDate();
                utcMonth = CurDate.getUTCMonth() + 1;
                utcYear = CurDate.getUTCFullYear();
            }

            var Dcur = utcDay.toString();
            if (Dcur.length < 2) {
                Dcur = "0" + Dcur;
            }
            var Mcur = utcMonth.toString();
            if (Mcur.length < 2) {
                Mcur = "0" + Mcur;
            }
            var Ycur = utcYear.toString();
            var Ycur2 = Ycur.substring(2);
            if (this._CultureDateFormat == "DMY" || this._CultureDateFormat == "MDY") {
                var P1 = ValueText.substring(this._LogicFirstPos, this._LogicFirstPos + 2);
                var P2 = ValueText.substring(this._LogicFirstPos + 3, this._LogicFirstPos + 5);
                if (this._AutoCompleteValue != "" && Y4) {
                    Ycur = this._AutoCompleteValue.substring(6);
                }
                else if (this._AutoCompleteValue != "" && !Y4) {
                    Ycur2 = this._AutoCompleteValue.substring(6);
                }
                if (this._CultureDateFormat == "DMY") {
                    if (this._AutoCompleteValue != "") {
                        Dcur = this._AutoCompleteValue.substring(0, 2);
                        Mcur = this._AutoCompleteValue.substring(3, 5);
                    }
                    P1 = this._AdjustElementDateDM(P1, Dcur);
                    P2 = this._AdjustElementDateDM(P2, Mcur);
                }
                if (this._CultureDateFormat == "MDY") {
                    if (this._AutoCompleteValue != "") {
                        Dcur = this._AutoCompleteValue.substring(3, 5);
                        Mcur = this._AutoCompleteValue.substring(0, 2);
                    }
                    P1 = this._AdjustElementDateDM(P1, Mcur);
                    P2 = this._AdjustElementDateDM(P2, Dcur);
                }
                var Y = ValueText.substring(this._LogicFirstPos + 6, this._LogicLastPos + 1);
                if (Y4) {
                    Y = this._AdjustElementDateY(Y, Ycur);
                }
                else {
                    Y = this._AdjustElementDateY(Y, Ycur2);
                }
                ValueText = ValueText.substring(0, this._LogicFirstPos) + P1 + this._CultureDatePlaceholder + P2 + this._CultureDatePlaceholder + Y + ValueText.substring(this._LogicLastPos + 1);
                this._LogicTextMask = this._LogicTextMask.substring(0, this._LogicFirstPos) + P1 + this._CultureDatePlaceholder + P2 + this._CultureDatePlaceholder + Y + this._LogicTextMask.substring(this._LogicLastPos + 1);
            }
            else if (this._CultureDateFormat == "DYM" || this._CultureDateFormat == "MYD") {
                var P1 = ValueText.substring(this._LogicFirstPos, this._LogicFirstPos + 2)
                if (this._CultureDateFormat == "DYM") {
                    if (this._AutoCompleteValue != "") {
                        Dcur = this._AutoCompleteValue.substring(0, 2);
                    }
                    P1 = this._AdjustElementDateDM(P1, Dcur);
                }
                if (this._CultureDateFormat == "MYD") {
                    if (this._AutoCompleteValue != "") {
                        Mcur = this._AutoCompleteValue.substring(0, 2);
                    }
                    P1 = this._AdjustElementDateDM(P1, Mcur)
                }
                if (this._AutoCompleteValue != "" && Y4) {
                    Ycur = this._AutoCompleteValue.substring(3, 7);
                }
                else if (this._AutoCompleteValue != "" && !Y4) {
                    Ycur2 = this._AutoCompleteValue.substring(3, 5);
                }
                var Y = null;
                var P2 = null;
                if (Y4) {
                    P2 = ValueText.substring(this._LogicFirstPos + 8, this._LogicLastPos + 1);
                    if (this._CultureDateFormat == "DYM") {
                        if (this._AutoCompleteValue != "") {
                            Mcur = this._AutoCompleteValue.substring(8);
                        }
                        P2 = this._AdjustElementDateDM(P2, Mcur);
                    }
                    if (this._CultureDateFormat == "MYD") {
                        if (this._AutoCompleteValue != "") {
                            Dcur = this._AutoCompleteValue.substring(8);
                        }
                        P2 = this._AdjustElementDateDM(P2, Dcur);
                    }
                    Y = ValueText.substring(this._LogicFirstPos + 3, this._LogicFirstPos + 7);
                    Y = this._AdjustElementDateY(Y, Ycur);
                }
                else {
                    P2 = ValueText.substring(this._LogicFirstPos + 6, this._LogicLastPos + 1);
                    if (this._CultureDateFormat == "DYM") {
                        if (this._AutoCompleteValue != "") {
                            Mcur = this._AutoCompleteValue.substring(6);
                        }
                        P2 = this._AdjustElementDateDM(P2, Mcur);
                    }
                    if (this._CultureDateFormat == "MYD") {
                        if (this._AutoCompleteValue != "") {
                            Dcur = this._AutoCompleteValue.substring(6);
                        }
                        P2 = this._AdjustElementDateDM(P2, Dcur);
                    }
                    Y = ValueText.substring(this._LogicFirstPos + 3, this._LogicFirstPos + 5);
                    Y = this._AdjustElementDateY(Y, Ycur2);
                }
                ValueText = ValueText.substring(0, this._LogicFirstPos) + P1 + this._CultureDatePlaceholder + Y + this._CultureDatePlaceholder + P2 + ValueText.substring(this._LogicLastPos + 1);
                this._LogicTextMask = this._LogicTextMask.substring(0, this._LogicFirstPos) + P1 + this._CultureDatePlaceholder + Y + this._CultureDatePlaceholder + P2 + this._LogicTextMask.substring(this._LogicLastPos + 1);
            }
            else if (this._CultureDateFormat == "YMD" || this._CultureDateFormat == "YDM") {
                var Y = null;
                var P1 = null;
                var P2 = null;
                if (this._AutoCompleteValue != "" && Y4) {
                    Ycur = this._AutoCompleteValue.substring(0, 4);
                }
                else if (this._AutoCompleteValue != "" && !Y4) {
                    Ycur2 = this._AutoCompleteValue.substring(0, 2);
                }
                if (Y4) {
                    Y = ValueText.substring(this._LogicFirstPos, this._LogicFirstPos + 4);
                    Y = this._AdjustElementDateY(Y, Ycur);
                    P1 = ValueText.substring(this._LogicFirstPos + 5, this._LogicFirstPos + 7);
                    if (this._CultureDateFormat == "YMD") {
                        if (this._AutoCompleteValue != "") {
                            Mcur = this._AutoCompleteValue.substring(5, 7);
                        }
                        P1 = this._AdjustElementDateDM(P1, Mcur);
                    }
                    if (this._CultureDateFormat == "YDM") {
                        if (this._AutoCompleteValue != "") {
                            Dcur = this._AutoCompleteValue.substring(5, 7);
                        }
                        P1 = this._AdjustElementDateDM(P1, Dcur);
                    }
                    P2 = ValueText.substring(this._LogicFirstPos + 8, this._LogicLastPos + 1);
                    if (this._CultureDateFormat == "YMD") {
                        if (this._AutoCompleteValue != "") {
                            Mcur = this._AutoCompleteValue.substring(8);
                        }
                        P2 = this._AdjustElementDateDM(P2, Mcur);
                    }
                    if (this._CultureDateFormat == "YDM") {
                        if (this._AutoCompleteValue != "") {
                            Dcur = this._AutoCompleteValue.substring(8);
                        }
                        P2 = this._AdjustElementDateDM(P2, Dcur);
                    }
                }
                else {
                    Y = ValueText.substring(this._LogicFirstPos, this._LogicFirstPos + 2);
                    Y = this._AdjustElementDateY(Y, Ycur2);
                    P1 = ValueText.substring(this._LogicFirstPos + 3, this._LogicFirstPos + 5);
                    if (this._CultureDateFormat == "YMD") {
                        if (this._AutoCompleteValue != "") {
                            Mcur = this._AutoCompleteValue.substring(3, 5);
                        }
                        P1 = this._AdjustElementDateDM(P1, Mcur);
                    }
                    if (this._CultureDateFormat == "YDM") {
                        if (this._AutoCompleteValue != "") {
                            Dcur = this._AutoCompleteValue.substring(3, 5);
                        }
                        P1 = this._AdjustElementDateDM(P1, Dcur);
                    }
                    P2 = ValueText.substring(this._LogicFirstPos + 6, this._LogicLastPos + 1);
                    if (this._CultureDateFormat == "YMD") {
                        if (this._AutoCompleteValue != "") {
                            Mcur = this._AutoCompleteValue.substring(6);
                        }
                        P2 = this._AdjustElementDateDM(P2, Mcur);
                    }
                    if (this._CultureDateFormat == "YDM") {
                        if (this._AutoCompleteValue != "") {
                            Dcur = this._AutoCompleteValue.substring(6);
                        }
                        P2 = this._AdjustElementDateDM(P2, Dcur);
                    }
                }
                ValueText = ValueText.substring(0, this._LogicFirstPos) + Y + this._CultureDatePlaceholder + P1 + this._CultureDatePlaceholder + P2 + ValueText.substring(this._LogicLastPos + 1);
                this._LogicTextMask = this._LogicTextMask.substring(0, this._LogicFirstPos) + Y + this._CultureDatePlaceholder + P1 + this._CultureDatePlaceholder + P2 + this._LogicTextMask.substring(this._LogicLastPos + 1);
            }
            this.get_element().value = (ValueText);
            ClearText = this._getClearMask(ValueText);
        }
        // clear mask and set CSS
        if (this._ClearMaskOnLostfocus) {
            this.get_element().value = (ClearText);
        }
        ValueText = ClearText;
        this.AddCssClassMaskedEdit("");
        if (this._MaskType == AjaxControlToolkit.MaskedEditType.Number && this._LogicSymbol == "-" && this._OnBlurCssNegative != "") {
            this.AddCssClassMaskedEdit(this._OnBlurCssNegative);
        }
        // perform validation
        this.ShowTooltipMessage(false);
        var IsValid = this._CaptureValidatorsControl();
        if (!IsValid) {
            if (isHijriCalendar) {
                $(this.get_element()).attr("gDate", "");
            }
            if (this._OnInvalidCssClass != "") {
                this.AddCssClassMaskedEdit(this._OnInvalidCssClass);
            }
            if (this._ClearTextOnInvalid) {
                this.get_element().value = (this._createMask());
            }
        }
        else {
            // trigger TextChanged with postback
            if (evt != null && typeof (this.get_element().onchange) != "undefined" && this.get_element().onchange != null && !this.get_element().readOnly) {
                // only validate when text changed.
                if (this._initialvalue != this._getClearMask(this.get_element().value)
                || (!this._AutoComplete && this._MaskType == AjaxControlToolkit.MaskedEditType.Date && this._initialvalue != this.get_element().value)) {
                    if (isHijriCalendar) {
                        var curAdjustDate = new AdjustDate({ isHijriDate: true, year: parseInt(Y), month: parseInt(P1) - 1, day: parseInt(P2) });
                        var dateFormat = $(this.get_element()).attr("format");
                        $(this.get_element()).attr("gDate", curAdjustDate.getGregorianDateText(dateFormat));
                    }
                    this.get_element().onchange(evt);
                }
            }
        }

        var _behavior = Sys.UI.Behavior.getBehaviorByName(this.get_element(), 'TextBoxWatermarkBehavior');
        if (_behavior) {
            _behavior._onBlur();
        }
    }
    //
    // Capture and execute validator to control
    //
    , _CaptureValidatorsControl: function () {
        var ret = true;
        // clear local save External Message Error
        this._ExternalMessageError = "";
        // Page_Validators is a Array of script asp.net
        if (typeof (Page_Validators) != "undefined") {
            var i = 0
            var ctrval = null;
            var msg = "";
            for (i = 0; i < Page_Validators.length; i++) {
                ctrval = Page_Validators[i];
                if (typeof (ctrval.enabled) == "undefined" || ctrval.enabled != false) {
                    if (ctrval.getAttribute("TargetValidator") == this.get_element().id) {
                        if (typeof (ctrval.evaluationfunction) == "function") {
                            var crtret = ctrval.evaluationfunction(ctrval);
                            if (!crtret) {
                                ret = false;
                                if (typeof (ctrval.errormessage) == "string") {
                                    if (msg != "") {
                                        msg += ", ";
                                    }
                                    msg += ctrval.errormessage;
                                }
                            }
                        }
                        else if (typeof (ctrval.getAttribute("evaluationfunction")) == "string") {
                            var crtret;
                            eval("crtret = " + ctrval.getAttribute("evaluationfunction") + "(" + ctrval.id + ")");
                            if (!crtret) {
                                ret = false;
                                if (typeof (ctrval.errormessage) == "string") {
                                    if (msg != "") {
                                        msg += ", ";
                                    }
                                    msg += ctrval.errormessage;
                                }
                            }
                        }
                    }
                }
            }
        }
        this._ExternalMessageError = msg;
        return ret;
    }
    //
    // Set Cancel Event for cross browser
    //
    , _SetCancelEvent: function (evt) {
        if (Sys.Browser.agent == Sys.Browser.InternetExplorer) {
            window.event.returnValue = false;
        }
        else {
            if (typeof (evt.returnValue) != "undefined") {
                evt.returnValue = false;
            }
            if (evt.preventDefault) {
                evt.preventDefault();
            }
        }
    }
    //
    // attachEvent keypress 
    //
    , _onKeyPress: function (args) {
        var evt = this._CheckArgsEvents(args);
        if (evt == null || evt.keyCode == 9) {
            return;
        }

        if (this.get_element().readOnly) {
            this._SetCancelEvent(evt);
            return;
        }

        var scanCode;
        var navkey = false;
        if (Sys.Browser.agent == Sys.Browser.InternetExplorer) {
            // IE
            scanCode = evt.keyCode;
        }
        else {
            if (evt.charCode) {
                scanCode = evt.charCode;
            }
            else {
                scanCode = evt.keyCode;
            }
            if (evt.keyIdentifier) {
                // Safari
                //3: 'KEY_ENTER', 13
                //63276: 'KEY_PAGE_UP', 33
                //63277: 'KEY_PAGE_DOWN', 34
                //63275: 'KEY_END', 35
                //63273: 'KEY_HOME', 36
                //63234: 'KEY_ARROW_LEFT', 37
                //63232: 'KEY_ARROW_UP', 38
                //63235: 'KEY_ARROW_RIGHT', 39
                //63233: 'KEY_ARROW_DOWN',40
                //63302: 'KEY_INSERT', 45
                //63272: 'KEY_DELETE', 46
                if (evt.ctrlKey || evt.altKey || evt.metaKey) {
                    return;
                }
                if (evt.keyIdentifier.substring(0, 2) != "U+") {
                    return;
                }
                if (scanCode == 63272) // delete
                {
                    scanCode = 46; // convert to IE code
                    navkey = true;
                }
                else if (scanCode == 63302) {
                    scanCode = 45; // convert to IE code
                    navkey = true;
                }
                else if (scanCode == 63233) {
                    scanCode = 40; // convert to IE code
                    navkey = true;
                }
                else if (scanCode == 63235) {
                    scanCode = 39; // convert to IE code
                    navkey = true;
                }
                else if (scanCode == 63232) {
                    scanCode = 38; // convert to IE code
                    navkey = true;
                }
                else if (scanCode == 63234) {
                    scanCode = 37; // convert to IE code
                    navkey = true;
                }
                else if (scanCode == 63273) {
                    scanCode = 36; // convert to IE code
                    navkey = true;
                }
                else if (scanCode == 63275) {
                    scanCode = 35; // convert to IE code
                    navkey = true;
                }
                else if (scanCode == 63277) {
                    scanCode = 34; // convert to IE code
                    navkey = true;
                }
                else if (scanCode == 63276) {
                    scanCode = 33; // convert to IE code
                    navkey = true;
                }
                else if (scanCode == 3) {
                    scanCode = 13; // convert to IE code
                    navkey = true;
                }
            }
            // key delete/backespace and key navigation for not IE browsers
            if (typeof (evt.which) != "undefined" && evt.which != null) {
                if (evt.which == 0) {
                    navkey = true;
                }
            }
            if (navkey && scanCode == 13) {
                this._onBlur(evt);
                if (!this._CaptureValidatorsControl()) {
                    this._onFocus(evt);
                }
                return;
            }
            if (scanCode == 8) {
                navkey = true;
            }
            if (!this._OnNavigator(scanCode, evt, navkey)) {
                return;
            }
            if (this.SpecialNavKey(scanCode, navkey)) {
                return;
            }
        }
        if (scanCode && scanCode >= 0x20 /* space */) {
            var c = String.fromCharCode(scanCode);
            var curpos = -1;
            if (Sys.Browser.agent == Sys.Browser.InternetExplorer) {
                curpos = this._deleteTextSelection();
            }
            if (curpos == -1) {
                curpos = this._getCurrentPosition();
            }
            if (curpos <= this._LogicFirstPos) {
                this.setSelectionRange(this._LogicFirstPos, this._LogicFirstPos);
                curpos = this._LogicFirstPos;
            }
            else if (curpos >= this._LogicLastPos + 1) {
                this.setSelectionRange(this._LogicLastPos + 1, this._LogicLastPos + 1);
                curpos = this._LogicLastPos + 1;
            }
            var logiccur = curpos;
            if (curpos == this._LogicFirstPos && this._InputDirection == AjaxControlToolkit.MaskedEditInputDirections.RightToLeft) {
                logiccur = this._getLastEmptyPosition();
            }
            if (this._MaskType == AjaxControlToolkit.MaskedEditType.Time && this.get_CultureFirstLettersAMPM().toUpperCase().indexOf(c.toUpperCase()) != -1) {
                if (this._AcceptAmPm) {
                    this.InsertAMPM(c);
                    this.setSelectionRange(curpos, curpos);
                    this._SetCancelEvent(evt);
                    return;
                }
            }
            else if (this._MaskType == AjaxControlToolkit.MaskedEditType.Number && this._AcceptNegative != AjaxControlToolkit.MaskedEditShowSymbol.None && "+-".indexOf(c) != -1) {
                this.InsertSignal(c);
                this.setSelectionRange(curpos, curpos);
                this._SetCancelEvent(evt);
                return;
            }
            else if (this._processKey(logiccur, c)) {
                if (this._MessageValidatorTip) {
                    this.ShowTooltipMessage(false);
                }
                if (this._InputDirection == AjaxControlToolkit.MaskedEditInputDirections.LeftToRight) {
                    // Let the key is valid
                    c = this._fixKey(logiccur, c);
                    this._insertContent(c, curpos);
                    curpos = this._getNextPosition(curpos + 1);
                }
                else if (this._InputDirection == AjaxControlToolkit.MaskedEditInputDirections.RightToLeft) {
                    // Let the key is valid
                    c = this._fixKey(logiccur, c);
                    
                    if (curpos > this._LogicFirstPos) {
                        this._insertContent(c, curpos);
                        curpos = this._getNextPosition(curpos + 1);
                    }
                    else {
                        // shift left and insert at last position 
                        // cursor fix at fist position
                        this._insertContentRight(c);
                        curpos = this._LogicFirstPos;
                    }
                }
                
                this.setSelectionRange(curpos, curpos);
                this._SetCancelEvent(evt);
                return;
            }
            else {
                //valid key navigation for not IE browsers
                //key navigation for IE capture in keydown 
                if (!this.SpecialNavKey(scanCode, navkey)) {
                    this._SetCancelEvent(evt);
                }
                return;
            }
        }
    }
    //
    // Insert Symbol AM/PM
    //
    , InsertAMPM: function (value) {
        var masktext = this.get_element().value;
        var ASymMask = this._CultureAMPMPlaceholder.split(";");
        var symb = "";
        if (ASymMask.length == 2) {
            if (value.toUpperCase() == this.get_CultureFirstLetterAM().toUpperCase()) {
                symb = ASymMask[0];
            }
            else if (value.toUpperCase() == this.get_CultureFirstLetterPM().toUpperCase()) {
                symb = ASymMask[1];
            }
            this._LogicSymbol = symb;
        }
        masktext = masktext.substring(0, this._LogicLastPos + 2) + symb + masktext.substring(this._LogicLastPos + 2 + symb.length);
        this.get_element().value = (masktext);
    }
    //
    // Insert Symbol Negative
    //
    , InsertSignal: function (value) {
        var masktext = this.get_element().value;
        if (value == "-" && this._LogicSymbol == "-") {
            value = "+"
        }
        if (value == "+") {
            value = " ";
            this._LogicSymbol = "";
            if (!this._InLostfocus && this._OnFocusCssClass != "") {
                this.AddCssClassMaskedEdit(this._OnFocusCssClass);
            }
            else if (!this._InLostfocus) {
                this.AddCssClassMaskedEdit("");
            }
        }
        else {
            this._LogicSymbol = "-";
            if (!this._InLostfocus && this._OnFocusCssNegative != "") {
                this.AddCssClassMaskedEdit(this._OnFocusCssNegative);
            }
        }
        if (this._AcceptNegative == AjaxControlToolkit.MaskedEditShowSymbol.Left) {
            masktext = masktext.substring(0, this._LogicFirstPos - 1) + value + masktext.substring(this._LogicFirstPos);
        }
        else if (this._AcceptNegative == AjaxControlToolkit.MaskedEditShowSymbol.Right) {
            masktext = masktext.substring(0, this._LogicLastPos + 1) + value + masktext.substring(this._LogicLastPos + 2);
        }
        this.get_element().value = (masktext);
    }
    //
    // keypress Navigate key (up/down/left/right/pgup/pgdown/home/end)
    // not IE process for event keypress
    //
    , SpecialNavKey: function (keyCode, navkey) {
        if (Sys.Browser.agent == Sys.Browser.InternetExplorer) {
            return false;
        }
        return (keyCode >= 33 && keyCode <= 45 && navkey);
    }
    , _OnNavigator: function (scanCode, evt, navkey) {
        if (!navkey) {
            return true;
        }
        var curpos;
        if (this._processDeleteKey(scanCode)) {
            curpos = this._getCurrentPosition();
            if (curpos <= this._LogicFirstPos && this._InputDirection != AjaxControlToolkit.MaskedEditInputDirections.RightToLeft) {
                this.setSelectionRange(this._LogicFirstPos, this._LogicFirstPos);
            }
            if (this._MessageValidatorTip) {
                if (this._getClearMask(this.get_element().value) == "") {
                    this.ShowTooltipMessage(true);
                }
            }
            this._SetCancelEvent(evt);
            return false;
        }
        if ((evt.ctrlKey || evt.altKey || evt.shiftKey || evt.metaKey)) {
            if (scanCode == 39 && evt.ctrlKey) {
                this._DirectSelText = "R";
                curpos = this._getCurrentPosition();
                if (curpos >= this._LogicLastPos + 1) {
                    this.setSelectionRange(this._LogicLastPos + 1, this._LogicLastPos + 1);
                    this._SetCancelEvent(evt);
                    return false;
                }
                return true;
            }
            else if (scanCode == 37 && evt.ctrlKey) {
                this._DirectSelText = "L";
                curpos = this._getCurrentPosition();
                if (curpos <= this._LogicFirstPos) {
                    this.setSelectionRange(this._LogicFirstPos, this._LogicFirstPos);
                    this._SetCancelEvent(evt);
                    return false;
                }
                return true;
            }
            else if (scanCode == 35 && evt.shiftKey) //END 
            {
                this._DirectSelText = "R";
                curpos = this._getCurrentPosition();
                this.setSelectionRange(curpos, this._LogicLastPos + 1);
                this._SetCancelEvent(evt);
                return false;
            }
            else if (scanCode == 36 && evt.shiftKey) //Home 
            {
                this._DirectSelText = "L";
                curpos = this._getCurrentPosition();
                this.setSelectionRange(this._LogicFirstPos, curpos);
                this._SetCancelEvent(evt);
                return false;
            }
            else if (scanCode == 35 || scanCode == 34) //END or pgdown
            {
                this._DirectSelText = "R";
                this.setSelectionRange(this._LogicLastPos + 1, this._LogicLastPos + 1);
                this._SetCancelEvent(evt);
                return false;
            }
            else if (scanCode == 36 || scanCode == 33) //Home or pgup
            {
                this._DirectSelText = "L";
                this.setSelectionRange(this._LogicFirstPos, this._LogicFirstPos);
                this._SetCancelEvent(evt);
                return false;
            }
            return true;
        }
        if (scanCode == 35 || scanCode == 34) //END or pgdown
        {
            this._DirectSelText = "R";
            this.setSelectionRange(this._LogicLastPos + 1, this._LogicLastPos + 1);
            this._SetCancelEvent(evt);
            return false;
        }
        else if (scanCode == 36 || scanCode == 33) //Home or pgup
        {
            this._DirectSelText = "L";
            this.setSelectionRange(this._LogicFirstPos, this._LogicFirstPos);
            this._SetCancelEvent(evt);
            return false;
        }
        else if (scanCode == 37) {
            this._DirectSelText = "L";
            curpos = this._getCurrentPosition();
            if (curpos <= this._LogicFirstPos) {
                this.setSelectionRange(this._LogicFirstPos, this._LogicFirstPos);
                this._SetCancelEvent(evt);
                return false;
            }
            return true;
        }
        else if (scanCode == 38 || scanCode == 40) {
            this._SetCancelEvent(evt);
            return false;
        }
        else if (scanCode == 39) {
            this._DirectSelText = "R";
            curpos = this._getCurrentPosition();
            if (curpos >= this._LogicLastPos + 1) {
                this.setSelectionRange(this._LogicLastPos + 1, this._LogicLastPos + 1);
                this._SetCancelEvent(evt);
                return false;
            }
            return true;
        }
        return true;
    }
    //
    // attachEvent keypress down for IE (delete , this._backspace)
    // adjust home/enter to mask
    //
    , _onKeyPressdown: function (args) {
        var evt = this._CheckArgsEvents(args);
        if (evt == null || evt.keyCode == 9) {
            return;
        }

        if (this.get_element().readOnly) {
            this._SetCancelEvent(evt);
            return;
        }
        if (evt.keyCode == 13) {
            this._onBlur(evt);
            if (!this._CaptureValidatorsControl()) {
                this._onFocus(evt);
            }
            return;
        }

        // FOR FIREFOX (NOT IMPLEMENT document.activeElement)
        if (Sys.Browser.agent != Sys.Browser.InternetExplorer) {
            if (this._InLostfocus) {
                if (Sys.Browser.agent == Sys.Browser.Safari) {
                    this._onClick(evt);
                }
                else {
                    this._onFocus(evt);
                }
            }
        }

        var scanCode = evt.keyCode;
        this._OnNavigator(scanCode, evt, true)
    }
    //
    // Set Cursor at position in TextBox
    //
    , setSelectionRange: function (selectionStart, selectionEnd) {
        input = this.get_element();
        if (input.createTextRange) {
            var range = input.createTextRange();
            range.collapse(true);
            range.moveEnd('character', selectionEnd);
            range.moveStart('character', selectionStart);
            range.select();
        }
        else if (input.setSelectionRange) {
            input.setSelectionRange(selectionStart, selectionEnd);
        }
    }
    //
    // Process del and this._backspace key
    //
    , _processDeleteKey: function (scanCode) {
        if (scanCode == 46 /*delete*/) {
            var curpos = this._deleteTextSelection();
            if (curpos == -1) {
                curpos = this._getCurrentPosition();
                if (this._InputDirection == AjaxControlToolkit.MaskedEditInputDirections.RightToLeft && curpos <= this._LogicFirstPos) {
                    var logicur = this._getLastEmptyPosition();
                    logicur = this._getNextPosition(logicur + 1);
                    this._backspace(logicur);
                }
                else {
                    this._deleteAtPosition(curpos);
                }
            }
            this.setSelectionRange(curpos, curpos);
            return true;
        }
        else if (scanCode == 8 /*back-space*/) {
            var curpos = this._deleteTextSelection();
            if (curpos == -1) {
                curpos = this._getCurrentPosition()
            }
            curpos = this._getPreviousPosition(this._getCurrentPosition() - 1);
            this._backspace(curpos);
            this.setSelectionRange(curpos, curpos);

            curpos = this._getCurrentPosition();
            if (curpos <= this._LogicFirstPos) {
                return true;
            }
            if (this._InputDirection == AjaxControlToolkit.MaskedEditInputDirections.LeftToRight) {
                this._deleteAtPosition(curpos);
                this.setSelectionRange(curpos, curpos);
            }
            return true;
        }
        return false;
    }
    //
    // delete at current position
    //
    , _deleteAtPosition: function (curpos) {
        var masktext = this.get_element().value;
        if (this._isValidMaskedEditPosition(curpos)) {
            var resttext = masktext.substring(curpos + 1);
            var restlogi = this._LogicTextMask.substring(curpos + 1);
            masktext = masktext.substring(0, curpos) + this._PromptChar;
            this._LogicTextMask = this._LogicTextMask.substring(0, curpos) + this._LogicPrompt;
            // clear rest of mask
            for (i = 0; i < parseInt(resttext.length, 10); i++) {
                if (this._isValidMaskedEditPosition(curpos + 1 + i)) {
                    masktext += this._PromptChar;
                    this._LogicTextMask += this._LogicPrompt;
                }
                else {
                    masktext += resttext.substring(i, i + 1);
                    this._LogicTextMask += restlogi.substring(i, i + 1);
                }
            }
            // insert only valid text
            posaux = this._getNextPosition(curpos);
            for (i = 0; i < parseInt(resttext.length, 10); i++) {
                if (this._isValidMaskedEditPosition(curpos + 1 + i) && restlogi.substring(i, i + 1) != this._LogicPrompt) {
                    masktext = masktext.substring(0, posaux) + resttext.substring(i, i + 1) + masktext.substring(posaux + 1);
                    this._LogicTextMask = this._LogicTextMask.substring(0, posaux) + restlogi.substring(i, i + 1) + this._LogicTextMask.substring(posaux + 1);
                    posaux = this._getNextPosition(posaux + 1);
                }
            }
            this.get_element().value = (masktext);
        }
    }
    //
    // this._backspace at current position
    //
    , _backspace: function (curpos) {
        var masktext = this.get_element().value;
        if (this._isValidMaskedEditPosition(curpos)) {
            masktext = masktext.substring(0, curpos) + this._PromptChar + masktext.substring(curpos + 1);
            this._LogicTextMask = this._LogicTextMask.substring(0, curpos) + this._LogicPrompt + this._LogicTextMask.substring(curpos + 1);
            this.get_element().value = (masktext);
        }
    }
    //
    // delete current Selected
    // return position select or -1 if nothing select
    //
    , _deleteTextSelection: function () {
        var masktext = this.get_element().value;
        var input = this.get_element();
        var ret = -1;
        var lenaux = -1;
        var begin = -1;
        if (document.selection) {
            sel = document.selection.createRange();
            if (sel.text != "") {
                var aux = sel.text + String.fromCharCode(3);
                sel.text = aux;

                if (input.createTextRange) {
                    dummy = input.createTextRange();
                    dummy.findText(aux);
                    dummy.select();
                }

                begin = input.value.indexOf(aux);
                if (this._DirectSelText == "P") {
                    this._DirectSelText = "";
                    ret = begin;
                }
                else {
                    if (this._InputDirection == AjaxControlToolkit.MaskedEditInputDirections.LeftToRight) {
                        ret = begin;
                    }
                    else if (this._InputDirection == AjaxControlToolkit.MaskedEditInputDirections.RightToLeft) {
                        ret = begin + parseInt(aux.length, 10) - 1;
                    }
                }
                document.selection.clear();
                lenaux = parseInt(aux.length, 10) - 1;
            }
        }
        else if (input.setSelectionRange) {
            if (input.selectionStart != input.selectionEnd) {
                var ini = parseInt(input.selectionStart, 10);
                var fim = parseInt(input.selectionEnd, 10);
                lenaux = fim - ini;
                begin = input.selectionStart;
                if (this._DirectSelText == "P") {
                    this._DirectSelText = "";
                    input.selectionEnd = input.selectionStart;
                    ret = begin;
                }
                else {
                    if (this._InputDirection == AjaxControlToolkit.MaskedEditInputDirections.LeftToRight) {
                        input.selectionEnd = input.selectionStart;
                        ret = begin;
                    }
                    else if (this._InputDirection == AjaxControlToolkit.MaskedEditInputDirections.RightToLeft) {
                        input.selectionStart = input.selectionEnd;
                        ret = begin + lenaux;
                    }
                }
            }
        }
        if (ret != -1) {
            for (i = 0; i < lenaux; i++) {
                if (this._isValidMaskedEditPosition(begin + i)) {
                    masktext = masktext.substring(0, begin + i) + this._PromptChar + masktext.substring(begin + i + 1);
                    this._LogicTextMask = this._LogicTextMask.substring(0, begin + i) + this._LogicPrompt + this._LogicTextMask.substring(begin + i + 1);
                }
            }
            this.get_element().value = (masktext);
            if (this._InputDirection == AjaxControlToolkit.MaskedEditInputDirections.RightToLeft) {
                //fix at first position
                if (ret > this._LogicLastPos || ret < this._LogicFirstPos) {
                    ret = this._LogicFirstPos;
                }
            }
        }
        return ret;
    }
    //
    // Insert Content at position in curpos
    //
    , _insertContent: function (value, curpos) {
        var masktext = this.get_element().value;
        masktext = masktext.substring(0, curpos) + value + masktext.substring(curpos + 1);

        if (value != " ") {
            this._LogicTextMask = this._LogicTextMask.substring(0, curpos) + value + this._LogicTextMask.substring(curpos + 1);
        }

        this.get_element().value = (masktext);
    }
    //
    // get last position empty in text 
    //
    , _getLastEmptyPosition: function () {
        var pos = this._LogicLastPos;
        while (pos >= 0 && this._LogicTextMask.substring(pos, pos + 1) != this._LogicPrompt) {
            pos--;
        }
        return pos;
    }
    //
    // get first position not empty in text 
    //
    , _getFirstNotEmptyPosition: function () {
        var pos = this._LogicFirstPos;
        while (pos <= this._LogicLastPos && this._LogicTextMask.substring(pos, pos + 1) == this._LogicPrompt) {
            pos++;
        }
        return pos;
    }
    //
    // Insert Content at last right position 
    //
    , _insertContentRight: function (value) {
        var masktext = this.get_element().value;
        curpos = this._getLastEmptyPosition();
        if (curpos < 0) {
            return;
        }
        var resttext = masktext.substring(curpos + 1);
        var restlogi = this._LogicTextMask.substring(curpos + 1);
        masktext = masktext.substring(0, curpos) + this._PromptChar;
        this._LogicTextMask = this._LogicTextMask.substring(0, curpos) + this._LogicPrompt;
        // clear rest of mask
        for (i = 0; i < parseInt(resttext.length, 10); i++) {
            if (this._isValidMaskedEditPosition(curpos + 1 + i)) {
                masktext += this._PromptChar;
                this._LogicTextMask += this._LogicPrompt;
            }
            else {
                masktext += resttext.substring(i, i + 1);
                this._LogicTextMask += restlogi.substring(i, i + 1);
            }
        }
        // insert only valid text
        posaux = this._getNextPosition(curpos);
        for (i = 0; i < parseInt(resttext.length, 10); i++) {
            if (this._isValidMaskedEditPosition(curpos + 1 + i) && restlogi.substring(i, i + 1) != this._LogicPrompt) {
                masktext = masktext.substring(0, posaux) + resttext.substring(i, i + 1) + masktext.substring(posaux + 1);
                this._LogicTextMask = this._LogicTextMask.substring(0, posaux) + restlogi.substring(i, i + 1) + this._LogicTextMask.substring(posaux + 1);
                posaux = this._getNextPosition(posaux + 1);
            }
        }
        // insert value
        masktext = masktext.substring(0, this._LogicLastPos) + value + masktext.substring(this._LogicLastPos + 1);

        if (value != " ") {
            this._LogicTextMask = this._LogicTextMask.substring(0, this._LogicLastPos) + value + this._LogicTextMask.substring(this._LogicLastPos + 1);
        }
        
        this.get_element().value = (masktext);
    }
    //
    // position is valid edit ?
    //
    , _isValidMaskedEditPosition: function (pos) {
        return (this._LogicMask.substring(pos, pos + 1) == this._LogicPrompt);
    }
    //
    // Next valid Position
    //
    , _getNextPosition: function (pos) {
        while (!this._isValidMaskedEditPosition(pos) && pos < this._LogicLastPos + 1) {
            pos++;
        }
        if (pos > this._LogicLastPos + 1) {
            pos = this._LogicLastPos + 1;
        }
        return pos;
    }
    //
    // Previous valid Position
    //
    , _getPreviousPosition: function (pos) {
        while (!this._isValidMaskedEditPosition(pos) && pos > this._LogicFirstPos) {
            pos--;
        }
        if (pos < this._LogicFirstPos) {
            pos = this._LogicFirstPos;
        }
        return pos;
    }
    //
    // Current Position
    //
    , _getCurrentPosition: function () {
        begin = 0;
        input = this.get_element();
        if (input.setSelectionRange) {
            begin = parseInt(input.selectionStart, 10);
        }
        else if (document.selection) {
            sel = document.selection.createRange();
            if (sel.text != "") {
                var aux = ""
                if (this._DirectSelText == "R") {
                    aux = sel.text + String.fromCharCode(3);
                }
                else if (this._DirectSelText == "L") {
                    aux = String.fromCharCode(3) + sel.text;
                }
                sel.text = aux;
                this._DirectSelText == "";
            }
            else {
                sel.text = String.fromCharCode(3);
                this._DirectSelText == "";
            }

            if (input.createTextRange) {
                dummy = input.createTextRange();
                dummy.findText(String.fromCharCode(3));
                dummy.select();
            }

            begin = input.value.indexOf(String.fromCharCode(3));
            document.selection.clear();
        }
        if (begin > this._LogicLastPos + 1) {
            begin = this._LogicLastPos + 1;
        }
        if (begin < this._LogicFirstPos) {
            begin = this._LogicFirstPos;
        }
        return begin;
    }
    // Validate key at position in mask and/or filter
    //
    , _processKey: function (poscur, key) {
        //  9 = only numeric
        //  L = only letter
        //  u = only lower letter
        //  U = only upper letter
        //  S = only numeric and letter
        //  $ = only letter and spaces
        //  C = only Custom - read from this._Filtered
        //  A = only letter and Custom
        //  N = only numeric and Custom
        //  ? = any digit
        var filter;
        var currChar = this._getMask(poscur);
        if (currChar == "9") {
            filter = this._charNumbers;
        }
        else if (currChar.toUpperCase() == "L") {
            filter = this._charLetters + this._charLetters.toLowerCase();
        }
        else if (currChar == "u") {
            filter = this._charLetters + this._charLetters.toLowerCase();
        }
        else if (currChar == "U") {
            filter = this._charLetters + this._charLetters.toLowerCase();
        }
        else if (currChar == "S") {
            filter = this._charNumbers + this._charLetters + this._charLetters.toLowerCase();
        }
        else if (currChar == "$") {
            filter = this._charLetters + this._charLetters.toLowerCase() + " ";
        }
        else if (currChar.toUpperCase() == "C") {
            filter = this._Filtered;
        }
        else if (currChar.toUpperCase() == "A") {
            filter = this._charLetters + this._charLetters.toLowerCase() + this._Filtered;
        }
        else if (currChar.toUpperCase() == "N") {
            filter = this._charNumbers + this._Filtered;
        }
        else if (currChar == "?") {
            filter = "";
        }
        else {
            return false;
        }
        
        if (filter == "") {
            return true;
        }
        
        // return true if we should accept the character.
        return (!filter || filter.length == 0 || filter.indexOf(key) != -1);
    }
    // Let the key at position in mask and/or filter is valid
    , _fixKey: function (poscur, key) {
        var currChar = this._getMask(poscur);
        if (currChar == "u") {
            key = key.toLowerCase();
        }
        else if (currChar == "U") {
            key = key.toUpperCase();
        }
        return key;
    }
    // Get the mask at the specified position of masks
    , _getMask: function (poscur) {
        return this._LogicMaskConv.substring(poscur, poscur + 1);
    }
    //
    // create mask empty , logic mask empty
    // convert escape code and Placeholder to culture
    //
    , _createMask: function () {
        var text;
        if (this._MaskConv == "" && this._Mask != "") {
            this._convertMask();
        }
        text = this._MaskConv;
        var i = 0;
        var masktext = "";
        var flagescape = false;
        this._LogicTextMask = "";
        this._QtdValidInput = 0;
        while (i < parseInt(text.length, 10)) {
            if (text.substring(i, i + 1) == this._charEscape && flagescape == false) {
                flagescape = true;
            }
            else if (this._CharsEditMask.indexOf(text.substring(i, i + 1)) == -1) {
                if (flagescape == true) {
                    flagescape = false;
                    masktext += text.substring(i, i + 1);
                    this._LogicTextMask += this._LogicEscape;
                }
                else {
                    if (this._CharsSpecialMask.indexOf(text.substring(i, i + 1)) != -1) {
                        this._QtdValidInput++;
                        if (text.substring(i, i + 1) == "/") {
                            masktext += this._CultureDatePlaceholder;
                            this._LogicTextMask += this._CultureDatePlaceholder;
                        }
                        else if (text.substring(i, i + 1) == ":") {
                            masktext += this._CultureTimePlaceholder;
                            this._LogicTextMask += this._CultureTimePlaceholder;
                        }
                        else if (text.substring(i, i + 1) == ",") {
                            masktext += this._CultureThousandsPlaceholder;
                            this._LogicTextMask += this._CultureThousandsPlaceholder;
                        }
                        else if (text.substring(i, i + 1) == ".") {
                            masktext += this._CultureDecimalPlaceholder;
                            this._LogicTextMask += this._CultureDecimalPlaceholder;
                        }
                    }
                    else {
                        masktext += text.substring(i, i + 1);
                        this._LogicTextMask += text.substring(i, i + 1);
                    }
                }
            }
            else {
                if (flagescape == true) {
                    flagescape = false;
                    masktext += text.substring(i, i + 1);
                    this._LogicTextMask += this._LogicEscape;
                }
                else {
                    this._QtdValidInput++;
                    masktext += this._PromptChar;
                    this._LogicTextMask += this._LogicPrompt;
                }
            }
            i++;
        }
        // Set First and last logic position
        this._LogicFirstPos = -1;
        this._LogicLastPos = -1;
        this._LogicMask = this._LogicTextMask;
        for (i = 0; i < parseInt(this._LogicMask.length, 10); i++) {
            if (this._LogicFirstPos == -1 && this._LogicMask.substring(i, i + 1) == this._LogicPrompt) {
                this._LogicFirstPos = i;
            }
            if (this._LogicMask.substring(i, i + 1) == this._LogicPrompt) {
                this._LogicLastPos = i;
            }
        }
        return masktext;
    }
    //
    // return text without mask but with placeholders 
    //
    , _getClearMask: function (masktext) {
        var i = 0;
        var clearmask = "";
        var qtdok = 0;
        while (i < parseInt(this._LogicTextMask.length, 10)) {
            if (qtdok < this._QtdValidInput) {
                if (this._isValidMaskedEditPosition(i) && this._LogicTextMask.substring(i, i + 1) != this._LogicPrompt) {
                    clearmask += this._LogicTextMask.substring(i, i + 1);
                    qtdok++;
                }
                else if (this._LogicTextMask.substring(i, i + 1) != this._LogicPrompt && this._LogicTextMask.substring(i, i + 1) != this._LogicEscape) {
                    if (this._LogicTextMask.substring(i, i + 1) == this._CultureDatePlaceholder) {
                        clearmask += (clearmask == "") ? "" : this._CultureDatePlaceholder;
                    }
                    else if (this._LogicTextMask.substring(i, i + 1) == this._CultureTimePlaceholder) {
                        clearmask += (clearmask == "") ? "" : this._CultureTimePlaceholder;
                    }
                    else if (this._LogicTextMask.substring(i, i + 1) == this._CultureThousandsPlaceholder) {
                        clearmask += (clearmask == "") ? "" : this._CultureThousandsPlaceholder;
                    }
                    else if (this._LogicTextMask.substring(i, i + 1) == this._CultureDecimalPlaceholder) {
                        clearmask += (clearmask == "") ? "" : this._CultureDecimalPlaceholder;
                    }
                }
            }
            i++;
        }
        if (this._LogicSymbol != "" && clearmask != "") {
            if (this._MaskType == AjaxControlToolkit.MaskedEditType.Time) {
                clearmask += " " + this._LogicSymbol;
            }
            else if (this._MaskType == AjaxControlToolkit.MaskedEditType.Number) {
                clearmask = this._LogicSymbol + clearmask;
            }
        }
        return clearmask;
    }
    //
    // Convert notation {Number} in PAD's Number
    //
    , _convertMask: function () {
        this._MaskConv = "";
        var qtdmask = "";
        var maskchar = "";
        for (i = 0; i < parseInt(this._Mask.length, 10); i++) {
            if (this._CharsEditMask.indexOf(this._Mask.substring(i, i + 1)) != -1) {
                if (qtdmask.length == 0) {
                    this._MaskConv += this._Mask.substring(i, i + 1);
                    qtdmask = "";
                    maskchar = this._Mask.substring(i, i + 1);
                }
                else if (this._Mask.substring(i, i + 1) == "9") {
                    qtdmask += "9";
                }
                else if (this._Mask.substring(i, i + 1) == "0") {
                    qtdmask += "0";
                }
            }
            else if (this._CharsEditMask.indexOf(this._Mask.substring(i, i + 1)) == -1 && this._Mask.substring(i, i + 1) != this._DelimitStartDup && this._Mask.substring(i, i + 1) != this._DelimitEndDup) {
                if (qtdmask.length == 0) {
                    this._MaskConv += this._Mask.substring(i, i + 1);
                    qtdmask = "";
                    maskchar = "";
                }
                else {
                    if (this._charNumbers.indexOf(this._Mask.substring(i, i + 1)) != -1) {
                        qtdmask += this._Mask.substring(i, i + 1);
                    }
                }
            }
            else if (this._Mask.substring(i, i + 1) == this._DelimitStartDup && qtdmask == "") {
                qtdmask = "0";
            }
            else if (this._Mask.substring(i, i + 1) == this._DelimitEndDup && qtdmask != "") {
                qtddup = parseInt(qtdmask, 10) - 1;
                if (qtddup > 0) {
                    for (q = 0; q < qtddup; q++) {
                        this._MaskConv += maskchar;
                    }
                }
                qtdmask = "";
                maskchar = "";
            }
        }
        // set first and last position in mask for Symbols
        var FirstPos = -1;
        var LastPos = -1;
        var flagescape = false;
        for (i = 0; i < parseInt(this._MaskConv.length, 10); i++) {
            if (this._MaskConv.substring(i, i + 1) == this._charEscape && !flagescape) {
                flagescape = true;
            }
            else if (this._CharsEditMask.indexOf(this._MaskConv.substring(i, i + 1)) != -1 && !flagescape) {
                if (FirstPos == -1) {
                    FirstPos = i;
                }
                LastPos = i;
            }
            else if (flagescape) {
                flagescape = false;
            }
        }
        // set spaces for Symbols AM/PM
        if (this._MaskType == AjaxControlToolkit.MaskedEditType.Time && this._AcceptAmPm) {
            var ASymMask = this._CultureAMPMPlaceholder.split(";");
            var SymMask = "";
            if (ASymMask.length == 2) {
                SymMask = this._charEscape + " ";
                for (i = 0; i < parseInt(ASymMask[0].length, 10); i++) {
                    SymMask += this._charEscape + " ";
                }
            }
            this._MaskConv = this._MaskConv.substring(0, LastPos + 1) + SymMask + this._MaskConv.substring(LastPos + 1);
        }
        // set spaces for Symbols Currency
        else if (this._MaskType == AjaxControlToolkit.MaskedEditType.Number && this._DisplayMoney == AjaxControlToolkit.MaskedEditShowSymbol.Left) {
            var SymMask = "";
            for (i = 0; i < parseInt(this._CultureCurrencySymbolPlaceholder.length, 10); i++) {
                if (this._CharsEditMask.indexOf(this._CultureCurrencySymbolPlaceholder.substring(i, i + 1)) == -1) {
                    SymMask += this._CultureCurrencySymbolPlaceholder.substring(i, i + 1);
                }
                else {
                    SymMask += this._charEscape + this._CultureCurrencySymbolPlaceholder.substring(i, i + 1);
                }
            }
            SymMask += this._charEscape + " ";
            this._MaskConv = this._MaskConv.substring(0, FirstPos) + SymMask + this._MaskConv.substring(FirstPos);
            FirstPos += SymMask.length;
            LastPos += SymMask.length;
        }
        // set spaces for Symbols negative
        else if (this._MaskType == AjaxControlToolkit.MaskedEditType.Number && this._DisplayMoney == AjaxControlToolkit.MaskedEditShowSymbol.Right) {
            var SymMask = this._charEscape + " ";
            for (i = 0; i < parseInt(this._CultureCurrencySymbolPlaceholder.length, 10); i++) {
                if (this._CharsEditMask.indexOf(this._CultureCurrencySymbolPlaceholder.substring(i, i + 1)) == -1) {
                    SymMask += this._CultureCurrencySymbolPlaceholder.substring(i, i + 1);
                }
                else {
                    SymMask += this._charEscape + this._CultureCurrencySymbolPlaceholder.substring(i, i + 1);
                }
            }
            this._MaskConv = this._MaskConv.substring(0, LastPos + 1) + SymMask + this._MaskConv.substring(LastPos + 1);
        }
        if (this._MaskType == AjaxControlToolkit.MaskedEditType.Number && this._AcceptNegative == AjaxControlToolkit.MaskedEditShowSymbol.Right) {
            this._MaskConv = this._MaskConv.substring(0, LastPos + 1) + this._charEscape + " " + this._MaskConv.substring(LastPos + 1);
        }
        else if (this._MaskType == AjaxControlToolkit.MaskedEditType.Number && this._AcceptNegative == AjaxControlToolkit.MaskedEditShowSymbol.Left) {
            this._MaskConv = this._MaskConv.substring(0, FirstPos) + this._charEscape + " " + this._MaskConv.substring(FirstPos);
        }
        this._convertMaskNotEscape();
    }
    //
    // Convert mask with escape to mask not escape
    // length equal to real position 
    //
    , _convertMaskNotEscape: function () {
        this._LogicMaskConv = "";
        var atumask = this._MaskConv;
        var flagescape = false;
        for (i = 0; i < parseInt(atumask.length, 10); i++) {
            if (atumask.substring(i, i + 1) == this._charEscape) {
                flagescape = true;
            }
            else if (!flagescape) {
                this._LogicMaskConv += atumask.substring(i, i + 1);
            }
            else {
                this._LogicMaskConv += this._LogicEscape;
                flagescape = false;
            }
        }
    }

    , _stopBubble: function (e) {
        if (Sys.Browser.agent != Sys.Browser.InternetExplorer)
            e.stopPropagation();
        else
            window.event.cancelBubble = true;
    }
    //
    // Helper properties
    //
    , get_Mask: function () {
        if (this._MaskConv == "" && this._Mask != "") {
            this._convertMask();
        }
        return this._MaskConv;
    }
    , set_Mask: function (value) {
        this._Mask = value;
        this.raisePropertyChanged('Mask');
    }
    , get_Filtered: function () {
        return this._Filtered;
    }
    , set_Filtered: function (value) {
        this._Filtered = value;
        this.raisePropertyChanged('Filtered');
    }
    , get_InputDirection: function () {
        return this._InputDirection;
    }
    , set_InputDirection: function (value) {
        this._InputDirection = value;
        this.raisePropertyChanged('InputDirection');
    }
    , get_PromptCharacter: function () {
        return this._PromptChar;
    }
    , set_PromptCharacter: function (value) {
        this._PromptChar = value;
        this.raisePropertyChanged('PromptChar');
    }
    , get_ShowStarCharacter: function () {
        return this._ShowStarChar;
    }
    , set_ShowStarCharacter: function (value) {
        this._ShowStarChar = value;
        this.raisePropertyChanged('ShowStarChar');
    }
    , get_OnFocusCssClass: function () {
        return this._OnFocusCssClass;
    }
    , set_OnFocusCssClass: function (value) {
        this._OnFocusCssClass = value;
        this.raisePropertyChanged('OnFocusCssClass');
    }
    , get_OnInvalidCssClass: function () {
        return this._OnInvalidCssClass;
    }
    , set_OnInvalidCssClass: function (value) {
        this._OnInvalidCssClass = value;
        this.raisePropertyChanged('OnInvalidCssClass');
    }
    , get_CultureName: function () {
        return this._CultureName;
    }
    , set_CultureName: function (value) {
        this._CultureName = value;
        this.raisePropertyChanged('Culture');
    }
    , get_CultureDatePlaceholder: function () {
        return this._CultureDatePlaceholder;
    }
    , set_CultureDatePlaceholder: function (value) {
        this._CultureDatePlaceholder = value;
        this.raisePropertyChanged('CultureDatePlaceholder');
    }
    , get_CultureTimePlaceholder: function () {
        return this._CultureTimePlaceholder;
    }
    , set_CultureTimePlaceholder: function (value) {
        this._CultureTimePlaceholder = value;
        this.raisePropertyChanged('CultureTimePlaceholder');
    }
    , get_CultureDecimalPlaceholder: function () {
        return this._CultureDecimalPlaceholder;
    }
    , set_CultureDecimalPlaceholder: function (value) {
        this._CultureDecimalPlaceholder = value;
        this.raisePropertyChanged('CultureDecimalPlaceholder');
    }
    , get_CultureThousandsPlaceholder: function () {
        return this._CultureThousandsPlaceholder;
    }
    , set_CultureThousandsPlaceholder: function (value) {
        this._CultureThousandsPlaceholder = value;
        this.raisePropertyChanged('CultureThousandsPlaceholder');
    }
    , get_CultureDateFormat: function () {
        return this._CultureDateFormat;
    }
    , set_CultureDateFormat: function (value) {
        this._CultureDateFormat = value;
        this.raisePropertyChanged('CultureDateFormat');
    }
    , get_CultureCurrencySymbolPlaceholder: function () {
        return this._CultureCurrencySymbolPlaceholder;
    }
    , set_CultureCurrencySymbolPlaceholder: function (value) {
        this._CultureCurrencySymbolPlaceholder = value;
        this.raisePropertyChanged('CultureCurrencySymbolPlaceholder');
    }
    , get_CultureAMPMPlaceholder: function () {
        return this._CultureAMPMPlaceholder;
    }
    , set_CultureAMPMPlaceholder: function (value) {
        if (value.split(";").length != 2 || value == ";") {
            this._CultureAMPMPlaceholder = "";
        }
        else {
            this._CultureAMPMPlaceholder = value;
        }
        this.raisePropertyChanged('CultureAMPMPlaceholder');
    }
    , get_CultureFirstLettersAMPM: function () {
        if (this._CultureAMPMPlaceholder != "") {
            //            var ASymMask = this._CultureAMPMPlaceholder.split(";");
            //            return (ASymMask[0].substring(0,1) + ASymMask[1].substring(0,1));
        }
        return "";
    }
    , get_CultureFirstLetterAM: function () {
        if (this._CultureAMPMPlaceholder != "") {
            //            var ASymMask = this._CultureAMPMPlaceholder.split(";");
            //            return ASymMask[0].substring(0,1);
        }
        return "";
    }
    , get_CultureFirstLetterPM: function () {
        if (this._CultureAMPMPlaceholder != "") {
            //            var ASymMask = this._CultureAMPMPlaceholder.split(";");
            //            return ASymMask[1].substring(0,1);
        }
        return "";
    }
    , get_ClearMaskOnLostFocus: function () {
        return this._ClearMaskOnLostfocus;
    }
    , set_ClearMaskOnLostFocus: function (value) {
        this._ClearMaskOnLostfocus = value;
        this.raisePropertyChanged('ClearMaskOnLostfocus');
    }
    , get_MessageValidatorTip: function () {
        return this._MessageValidatorTip;
    }
    , set_MessageValidatorTip: function (value) {
        this._MessageValidatorTip = value;
        this.raisePropertyChanged('MessageValidatorTip');
    }
    , get_AcceptAMPM: function () {
        return this._AcceptAmPm;
    }
    , set_AcceptAMPM: function (value) {
        this._AcceptAmPm = value;
        this.raisePropertyChanged('AcceptAmPm');
    }
    , get_AcceptNegative: function () {
        return this._AcceptNegative;
    }
    , set_AcceptNegative: function (value) {
        this._AcceptNegative = value;
        this.raisePropertyChanged('AcceptNegative');
    }
    , get_DisplayMoney: function () {
        return this._DisplayMoney;
    }
    , set_DisplayMoney: function (value) {
        this._DisplayMoney = value;
        this.raisePropertyChanged('DisplayMoney');
    }
    , get_OnFocusCssNegative: function () {
        return this._OnFocusCssNegative;
    }
    , set_OnFocusCssNegative: function (value) {
        this._OnFocusCssNegative = value;
        this.raisePropertyChanged('OnFocusCssNegative');
    }
    , get_OnBlurCssNegative: function () {
        return this._OnBlurCssNegative;
    }
    , set_OnBlurCssNegative: function (value) {
        this._OnBlurCssNegative = value;
        this.raisePropertyChanged('OnBlurCssNegative');
    }
    , get_Century: function () {
        return this._Century;
    }
    , set_Century: function (value) {
        this._Century = value;
        this.raisePropertyChanged('Century');
    }
    , get_AutoComplete: function () {
        return this._AutoComplete;
    }
    , set_AutoComplete: function (value) {
        this._AutoComplete = value;
        this.raisePropertyChanged('AutoComplete');
    }
    , get_AutoCompleteValue: function () {
        return this._AutoCompleteValue;
    }
    , set_AutoCompleteValue: function (value) {
        this._AutoCompleteValue = value;
        this.raisePropertyChanged('AutoCompleteValue');
    }
    , get_MaskType: function () {
        return this._MaskType;
    }
    , set_MaskType: function (value) {
        this._MaskType = value;
        this.raisePropertyChanged('MaskType');
    }
}
AjaxControlToolkit.MaskedEditBehavior.registerClass('AjaxControlToolkit.MaskedEditBehavior', AjaxControlToolkit.DynamicPopulateBehaviorBase);

// **************************************************
// Register enumerations  
// **************************************************
AjaxControlToolkit.MaskedEditType = function() {
    throw Error.invalidOperation();
}

AjaxControlToolkit.MaskedEditInputDirections = function() {
    throw Error.invalidOperation();
}

AjaxControlToolkit.MaskedEditShowSymbol = function() {
    throw Error.invalidOperation();
}

AjaxControlToolkit.MaskedEditType.prototype = {
    None: 0,
    Date: 1,
    Number: 2,
    Time: 3
}

AjaxControlToolkit.MaskedEditInputDirections.prototype = {
    LeftToRight: 0,
    RightToLeft: 1
}

AjaxControlToolkit.MaskedEditShowSymbol.prototype = {
    None: 0,
    Left: 1,
    Right: 2
}

AjaxControlToolkit.MaskedEditType.registerEnum('AjaxControlToolkit.MaskedEditType');
AjaxControlToolkit.MaskedEditInputDirections.registerEnum('AjaxControlToolkit.MaskedEditInputDirections');
AjaxControlToolkit.MaskedEditShowSymbol.registerEnum('AjaxControlToolkit.MaskedEditShowSymbol');
