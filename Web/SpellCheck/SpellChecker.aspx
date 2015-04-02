<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpellChecker.aspx.cs" Inherits="Accela.ACA.Web.SpellChecker" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>" xml:lang="<%=Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture %>">
<head runat="server">
    <title>Spell Check</title>
    <style type="text/css">
        a
        {
            text-decoration: none;
            cursor:default;
        }
        
        a:hover
        {
            text-decoration: none;
        }
    </style>
    <script type="text/javascript" src="../Scripts/jquery.js"></script>
    <script type="text/javascript" src="../Scripts/GlobalConst.aspx"></script>
    <script type="text/javascript" src="../Scripts/global.js"></script>
    <script type="text/javascript">
        // The master Array
        var checkedElements = new Array(0);
        var currentElement = 0;
        var currentEvent = -1;

        // The skip Array
        var skipArray = new Array(0);

        // The processed Objects Associative Array
        var singleIgnoredArray = new Object();

        //Results for a new element:
        checkedElements[0] = new Array(3);

        //Esc hot key for escape current window
        function EscapeWindow(e) {
                if (e.keyCode == 27) {
                    window.close();
                }
        }
        // Disable the right click context menu
        //document.oncontextmenu = new Function("return false");

        /*
        * Handles when the cancelAll button is pressed.
        *
        * Undo's the changes made by this spell check run.
        */
        function cancelAll() {
            if (!responseButton) {
                return;
            }
            
            try {
                for (var x = 0; x < checkedElements.length; x++) {
                    restoreField(checkedElements[x][0], checkedElements[x][2]);
                }
                window.close();
            }
            catch (e) {
                alert(exceptionMsg);
            }
        }

        /*
        * Handles the enter key being pressed on the select box.
        */
        function suggestionsKeyPress() {
            if (window.event && window.event.keyCode == 13) // Enter Key
            {
                suggestionsDoubleClick();
                return false;
            }
        }

        /*
        * Handles the single clicking on the suggestions select box.
        */
        function suggestionsSingleClick() {
            var selIndex = document.correct.suggestions.selectedIndex;
            if (selIndex > -1) {
                document.correct.replaceWith.value = document.correct.suggestions.options[selIndex].text;
            }
        }

        /*
        * Handles the double clicking on the suggestions select box.
        */
        function suggestionsDoubleClick() {
            var selIndex = document.correct.suggestions.selectedIndex;
            if (selIndex > -1) {
                document.correct.replaceWith.value = document.correct.suggestions.options[selIndex].text;
                replaceWord(false);
            }
        }

        /*
        * Handles the pressing of the escape key.
        */
        function miscKeyPress() {
            if (window.event && window.event.keyCode == 27) // Escape Key
            {
                window.close();
                return false;
            }
        }

        /*
        * Called when the replaceAll button is pressed.
        *
        * Adds the word to the skip array and calls replaceWord( true ).
        */
        function replaceAllInstances() {
            if (!responseButton) {
                return;
            }

            addToSkipArray();
            replaceWord(true);
        }

        /*
        * Called when the ignoreAll button is pressed.
        *
        * Adds the word to the skip array and processes the next mistake.
        */
        function ignoreAllInstances() {
            if (!responseButton) {
                return;
            }
            
            addToSkipArray();
            processNextMistake();
        }

        /*
        * Called when the ignore button is pressed.
        * 
        * Increases the count of the word in the singleIgnoredArray.
        */
        function ignoreWord() {
            if (!responseButton) {
                return;
            }
            
            var word = document.correct.original.value;

            var instance = 0;
            if (singleIgnoredArray[word] != null) {
                instance = singleIgnoredArray[word];
            }
            instance++;
            singleIgnoredArray[word] = instance;
            processNextMistake();
        }

        /*
        * Adds a word to the skip array.
        */
        function addToSkipArray() {
            skipArray[skipArray.length] = document.correct.original.value;
        }

        /*
        * Function to replace a word with the word in the replaceWith field.
        *
        * The single parameter is a boolean representing if all of the instances of that
        *  word should be replaced.
        */
        function replaceWord(replaceAll) {
            if (!responseButton) {
                return;
            }
            
            try {
                replaceWordInField(document.correct.original.value,
                    checkedElements[currentElement][0], document.correct.replaceWith.value, replaceAll);
                processNextMistake();
            }
            catch (e) {
                alert(exceptionMsg);
            }
        }

        /*
        * Function to process the next spelling mistake.
        */
        function processNextMistake() {
            currentEvent++;

            if (!(currentElement >= checkedElements.length) && currentEvent >= checkedElements[currentElement][1].length) {
                // This currentEvent is too high. I need to cycle to the next Element
                currentElement++;
                skipArray = new Array(0); // reset the skipArray - we can only skip easily within one element (a slight limitation for now)
                singleIgnoredArray = new Object(); // reset the singleIgnoredArray - duplicate ignore's on the same word needs to skip the first one
                currentEvent = 0;
            }

            if (currentElement >= checkedElements.length) {
                // I am out of elements
                var winOpener = window.opener;
                window.close();
                winOpener.alert(browserNotSupportMsg);
                return;
            }

            if (checkedElements[currentElement][1].length == 0) {
                processNextMistake();
                return;
            }

            // Get the original word from the array
            var originalWord = checkedElements[currentElement][1][currentEvent][0];

            // If this word is in the skip array - skip it!
            for (var x = 0; x < skipArray.length; x++) {
                if (skipArray[x] == originalWord) {
                    processNextMistake();
                    return;
                }

            }

            // Invalid Word
            document.correct.original.value = originalWord;

            // Clear suggestions box
            document.correct.suggestions.options.length = 0;

            // Add suggestion words
            for (var sug = 0; sug < checkedElements[currentElement][1][currentEvent][2].length; sug++) {
                document.correct.suggestions.options[sug] = new Option(checkedElements[currentElement][1][currentEvent][2][sug]);
            }

            // Fill replaceWith box
            if (document.correct.suggestions.options.length > 0) {
                document.correct.replaceWith.value = document.correct.suggestions.options[0].text;
                document.correct.suggestions.options[0].selected = true;
            }
            else {
                document.correct.replaceWith.value = originalWord;
            }

            try {
                selectWordInField(originalWord, checkedElements[currentElement][0])
            }
            catch (e) {
                alert(exceptionMsg);
            }
        }


        /*
        * Function to highlight a word in a particular field.
        *
        * The formAndField parameter will be appended to a 'document.' to find the field.
        */
        function selectWordInField(word, formAndField) {
            var element = eval('parent.opener.document.' + formAndField);

            if (element.createTextRange) {
                selectWordInFieldIE(word, formAndField);
            }
            else if (element.setSelectionRange) {
                selectWordInFieldNotIE(word, formAndField);
            }
            else {
                alert(browserNotSupportMsg);
            }
        }

        /*
        * Function to replace a word in a particular field.
        *
        * The formAndField parameter will be appended to a 'document.' to find the field.
        */
        function replaceWordInField(word, formAndField, newWord, replaceAll) {
            var element = eval('parent.opener.document.' + formAndField);

            if (element.setSelectionRange) {
                replaceWordInFieldNotIE(word, formAndField, newWord, replaceAll);
            }
            else if (element.createTextRange) {
                replaceWordInFieldIE(word, formAndField, newWord, replaceAll);
            }
            else {
                alert(browserNotSupportMsg);
            }
        }

        /*
        * Function to highlight a word in a particular field.
        *
        * The formAndField parameter will be appended to a 'document.' to find the field.
        */
        function selectWordInFieldIE(word, formAndField) {
            var element = eval('parent.opener.document.' + formAndField);
            var textRange = element.createTextRange();
            findAndSelectIE(textRange, word);
        }

        /*
        * Finds and Selects the text in the given textRange.
        *
        * This method looks at the singleIgnoredArray to know if it should find
        *  the n-th instance of a word.
        *
        */
        function findAndSelectIE(textRange, word) {
            var instance = 0;
            if (singleIgnoredArray[word] != null) {
                instance = singleIgnoredArray[word];
            }

            for (var x = 0; x <= instance; x++) {
                if (x > 0)
                    textRange.move('character', 1);
                var found = textRange.findText(word, 1, 6);
                if (!found)
                    textRange.findText(word, 1, 4);
            }
            textRange.select();
        }


        function replaceWordInFieldIE(word, formAndField, newWord, replaceAll) {
            var element = eval('parent.opener.document.' + formAndField);

            var textRange = element.createTextRange();
            findAndSelectIE(textRange, word);
            textRange.text = newWord;

            if (replaceAll) {
                var keepGoing = true;
                while (keepGoing) {
                    keepGoing = textRange.findText(word, 1, 6);
                    if (keepGoing /*|| textRange.text == word */) {
                        textRange.select();
                        textRange.text = newWord;
                    }
                }
            }
        }

        /*
        * Function to highlight a word in a particular field.
        *
        * The formAndField parameter will be appended to a 'document.' to find the field.
        */
        function selectWordInFieldNotIE(word, formAndField) {
            var element = eval('parent.opener.document.' + formAndField);
            findAndSelectNotIE(element, word);
        }

        function findAndSelectNotIE(element, word) {
            var instance = 0;
            if (singleIgnoredArray[word] != null) {
                instance = singleIgnoredArray[word];
            }

            var offSet = 0;
            var match = false;
            var re = new RegExp('\\b' + word + '\\b');
            var str = element.value;
            var match = re.exec(str);

            for (var x = 1; x <= instance; x++) {
                offSet = offSet + match.index + word.length;
                str = str.substring(match.index + word.length);
                match = re.exec(str);
            }

            if (match) {
                var left = match.index + offSet;
                var right = left + word.length;
                element.setSelectionRange(left, right);
                return match;
            }
            else {
                return false;
            }
        }

        function replaceWordInFieldNotIE(word, formAndField, newWord, replaceAll) {
            var element = eval('parent.opener.document.' + formAndField);

            findAndSelectNotIE(element, word);

            var selectionStart = element.selectionStart;
            var selectionEnd = element.selectionEnd;
            element.value = element.value.substring(0, selectionStart)
                      + newWord
                      + element.value.substring(selectionEnd);

            if (replaceAll && word != newWord) {
                var keepGoing = true;
                while (keepGoing) {
                    keepGoing = findAndSelectNotIE(element, word);
                    if (keepGoing) {
                        selectionStart = element.selectionStart;
                        selectionEnd = element.selectionEnd;
                        element.value = element.value.substring(0, selectionStart)
                                  + newWord
                                  + element.value.substring(selectionEnd);
                    }                 
                }
            }

            element.setSelectionRange(0, 0);
        }


        /*
        * Function to restore a field to it's original value.
        *
        */
        function restoreField(formAndField, originalValue) {
            var element = eval('parent.opener.document.' + formAndField);
            element.value = originalValue;
        }

        $(document).ready(function () {
            if ($.browser.opera) {
                //Make links 'tab-able' in Opera
                SetTabIndexForOpera();
            }
        });

    </script>
</head>
<body onload="DoSpellCheck();" onkeypress="miscKeyPress();" onkeydown="EscapeWindow(event)">

    <script type="text/javascript">
        var exceptionMsg = '<%=GetTextByKey("acc_spell_check_lable_tip").Replace("'","\\'") %>';
        var browserNotSupportMsg = '<%=GetTextByKey("acc_spell_check_lable_msg").Replace("'","\\'") %>';
        var notSupportLng = "<%=GetTextByKey("acc_spell_check_lng_msg").Replace("'","\\'") %>";
        var element = eval('parent.opener.document.getElementById("<%=Request["id"] %>")');
        var responseButton = false;
        
        function DoSpellCheck() {
            PageMethods.DoSpellCheck(GetValue(element), ExeSpellCheck);
        }

        function ExeSpellCheck(result) {
            if (result == '') {
                alert(browserNotSupportMsg);
                close();
            }
            else if (result == "-1") {
                alert(notSupportLng);
                close();
            }
            else {
                var o = eval('(' + result + ')');
                //Element Name (opener.document.xxx):
                checkedElements[0][0] = 'forms[0].elements["' + element.id + '"]';
                // Number of spelling errors:
                checkedElements[0][1] = new Array(o.length);
                // Original Value:
                checkedElements[0][2] = element.value;

                for (var i = 0; i < o.length; i++) {
                    // Holder for the information about the event:
                    checkedElements[0][1][i] = new Array(3);
                    //Invalid Word:
                    checkedElements[0][1][i][0] = JsonDecode(o[i].Word);
                    //Word Context Position:
                    checkedElements[0][1][i][1] = o[i].WordContextPosition;
                    //Number of Suggestions (size of next array):
                    checkedElements[0][1][i][2] = new Array(o[i].Suggestions.length);

                    for (var j = 0; j < o[i].Suggestions.length; j++) {
                        checkedElements[0][1][i][2][j] = o[i].Suggestions[j];
                    }
                }

                processNextMistake();

                responseButton = true;
            }
        }

        function changeButtonStyle(idPrex, classNamePrex) {
            var divLeft = document.getElementById(idPrex + "left");
            var divCenter = document.getElementById(idPrex + "center");
            var divRight = document.getElementById(idPrex + "right");

            divLeft.className = 'ACA_LgButtonLeft' + classNamePrex;
            divCenter.className = 'ACA_LgButtonCenter' + classNamePrex;
            divRight.className = 'ACA_LgButtonRight' + classNamePrex;
        }
    </script>

    <form id="correct" runat="server">
    <span style="display:none">
        <ACA:AccelaHideLink ID="hlSkipToolBar" runat="server" AltKey="img_alt_modulemenu_skiplink" IsSkippingToAnchor="true" NextControlClientID="FirstAnchorInACAMainContent" TabIndex="-1"/>
        <a name="FirstAnchorInACAMainContent" id="FirstAnchorInACAMainContent" tabindex="-1"></a>
    </span>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <table role='presentation' cellspacing="5">
        <tr>
            <td>
                <table role='presentation'>
                    <tr>
                        <td>
                            <ACA:AccelaLabel ID="AccelaLabel1" AssociatedControlID="original" CssClass="ACA_Label ACA_Label_FontSize" runat="server" LabelKey="acc_spell_check_lable_not_in_dictionary"></ACA:AccelaLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="original" runat="server" Width="168px" ReadOnly="true" CssClass="ACA_ReadOnly"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ACA:AccelaLabel ID="AccelaLabel3" AssociatedControlID="replaceWith" CssClass="ACA_Label ACA_Label_FontSize" runat="server" LabelKey="acc_spell_check_lable_change_to"></ACA:AccelaLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="replaceWith" runat="server" Width="168px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ACA:AccelaLabel ID="AccelaLabel4" AssociatedControlID="suggestions" CssClass="ACA_Label ACA_Label_FontSize" runat="server" LabelKey="acc_spell_check_lable_suggestion"></ACA:AccelaLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ListBox ID="suggestions" runat="server" Height="88px" Width="174px" onkeypress="suggestionsKeyPress();"
                                onclick="suggestionsSingleClick();" onchange="suggestionsSingleClick();" ondblclick="suggestionsDoubleClick();">
                            </asp:ListBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table role='presentation' cellspacing="3">
                    <tr>
                        <td>
                            <ACA:AccelaButton ID="btnChange" runat="server" LabelKey="acc_spell_check_button_change" OnClientClick="replaceWord(false);return false;"
                                DivEnableCss="ACA_LgButton ACA_LgButton_FontSize SpellCheck_Button" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize SpellCheck_Button">
                            </ACA:AccelaButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ACA:AccelaButton ID="btnChangeAll" runat="server" LabelKey="acc_spell_check_button_change_all" OnClientClick="replaceAllInstances();return false;"
                                DivEnableCss="ACA_LgButton ACA_LgButton_FontSize SpellCheck_Button" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize SpellCheck_Button">
                            </ACA:AccelaButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ACA:AccelaButton ID="btnIgnore" runat="server" LabelKey="acc_spell_check_button_ignore" OnClientClick="ignoreWord();return false;"
                                DivEnableCss="ACA_LgButton ACA_LgButton_FontSize SpellCheck_Button" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize SpellCheck_Button">
                            </ACA:AccelaButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ACA:AccelaButton ID="btnIgnoreAll" runat="server" LabelKey="acc_spell_check_button_ignore_all" OnClientClick="ignoreAllInstances();return false;"
                                DivEnableCss="ACA_LgButton ACA_LgButton_FontSize SpellCheck_Button" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize SpellCheck_Button">
                            </ACA:AccelaButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ACA:AccelaButton ID="btnCancelAll" runat="server" LabelKey="acc_spell_check_button_undo_all" OnClientClick="cancelAll();return false;"
                                DivEnableCss="ACA_LgButton ACA_LgButton_FontSize SpellCheck_Button" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize SpellCheck_Button">
                            </ACA:AccelaButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ACA:AccelaButton ID="btnClose" runat="server" LabelKey="acc_spell_check_button_close" OnClientClick="window.close();return false;"
                                DivEnableCss="ACA_LgButton ACA_LgButton_FontSize SpellCheck_Button" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize SpellCheck_Button">
                            </ACA:AccelaButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="submit" name="Submit" value="Submit" class="HiddenButton" onclick="return false;" />
    </form>
    <noscript><%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</body>
</html>
<!-- Manually load AjaxControlToolkit framework -->
<script type="text/javascript">
    if (typeof (AjaxControlToolkit) == 'undefined') {
        var script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = '<%=Page.ClientScript.GetWebResourceUrl(typeof(AjaxControlToolkit.CommonToolkitScripts),"AjaxControlToolkit.Common.Common.js")%>';
        document.getElementsByTagName('head').item(0).appendChild(script);
    }
</script>