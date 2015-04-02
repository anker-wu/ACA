/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: MaskedEditValidator.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: MaskedEditValidator.js 72643 2007-07-10 21:52:06Z vernon.crandall $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
 
// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.
// Product      : MaskedEdit Validator Control
// Version      : 1.0.0.0
// Date         : 11/08/2006
// Development  : Fernando Cerqueira 
//

function MaskedEditSetMessage(value,msg)
{
    value.errormessage = msg;
    value.innerHTML = msg;
}
function MaskedEditMessageShow(value,IsValid)
{
    if (typeof(value.display) == "string") 
    {    
        if (value.display == "None") {
            return;
        }
        if (value.display == "Dynamic") {
            value.style.display = IsValid ? "none" : "inline";
            return;
        }
    }
    value.style.visibility = IsValid ? "hidden" : "visible";
}
function MaskedEditSetCssClass(value,Css)
{
    var target = $get(value.getAttribute("TargetValidator")); 
    Sys.UI.DomElement.removeCssClass(target,value.getAttribute("InvalidValueCssClass"));
    Sys.UI.DomElement.removeCssClass(target,value.getAttribute("CssBlurNegative"));
    Sys.UI.DomElement.removeCssClass(target,value.getAttribute("CssFocus"));
    Sys.UI.DomElement.removeCssClass(target,value.getAttribute("CssFocusNegative"));
    if (Css != "")
    {
        Sys.UI.DomElement.addCssClass(target,Css);
    }
}
function MaskedEditValidatorDate(value)
{

    MaskedEditSetMessage(value,"")
    MaskedEditSetCssClass(value,"");
    MaskedEditMessageShow(value,true);
    if (value.getAttribute("IsMaskedEdit") == "false")
    {
        return true;
    }
    var target = $get(value.getAttribute("TargetValidator")); 
    if (value.getAttribute("ValidEmpty")  == "false")
    {
        if (target.value == value.getAttribute("InitialValue"))
        {
            MaskedEditSetMessage(value,value.getAttribute("EmptyValueMessage"));
            MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
            MaskedEditMessageShow(value,false);
            return false;
        }
    }
    if (target.value == "")
    {
        return true;
    }

    if (AjaxControlToolkit.TextBoxWrapper.get_Wrapper(target).get_IsWatermarked()) {
        return true;
    }

    var ret = true;
    var mask = target.value.substring(parseInt(value.getAttribute("FirstMaskPosition"),10),parseInt(value.getAttribute("LastMaskPosition"),10));
    if (value.getAttribute("ValidationExpression") != "" )
    {
        var rx = new RegExp(value.getAttribute("ValidationExpression"));
        var matches = rx.exec(mask);
        ret = (matches != null && mask == matches[0]);
        if (!ret)
        {
            MaskedEditSetMessage(value,value.getAttribute("InvalidValueMessage"));
            MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
            MaskedEditMessageShow(value,false);
            return false;
        }
    }
    var m_arrDate = mask.split(value.getAttribute("DateSeparator"));
    if (parseInt(m_arrDate.length,10) != 3)
    {
        MaskedEditSetMessage(value,value.getAttribute("InvalidValueMessage"));
        MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
        ret = false;
    }
    if (value.getAttribute("DateFormat").indexOf("D") == -1 || value.getAttribute("DateFormat").indexOf("M") == -1 || value.getAttribute("DateFormat").indexOf("Y") == -1)
    {
        MaskedEditSetMessage(value,value.getAttribute("InvalidValueMessage"));
        MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
        ret = false;
    }
    var D = -1;
    var M = -1;
    var Y = -1;
    if (ret)
    {
        D = parseInt(m_arrDate[value.getAttribute("DateFormat").indexOf("D")],10);
        M = parseInt(m_arrDate[value.getAttribute("DateFormat").indexOf("M")],10);
        Y = parseInt(m_arrDate[value.getAttribute("DateFormat").indexOf("Y")], 10);
        if (value.getAttribute("IsHijriDate") != "true") {
           if (Y < 100) {
               Y = parseInt(Y + value.getAttribute("Century"), 10);
           } else if (Y < 999) {
               Y += parseInt(value.getAttribute("Century").substring(0, 1) + Y, 10);
           }
        }

        if (value.getAttribute("IsHijriDate") == "true") {
            ret = (D > 0 && M > 0 && Y > 0 && (D <= [, 30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 29][M] || D == 30 && M == 12 && ((Y * 11 + 14) % 30 < 11) && (Y % 100 > 0 || Y % 400 == 0)));
        } else {
            ret = (D > 0 && M > 0 && Y > 0 && (D <= [, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][M] || D == 29 && M == 2 && Y % 4 == 0 && (Y % 100 > 0 || Y % 400 == 0)));
        }
    }
    if (!ret)
    {
        MaskedEditSetMessage(value,value.getAttribute("InvalidValueMessage"));
        MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
    }
    if(ret && (value.getAttribute("MaximumValue") != "" || value.getAttribute("MinimumValue") != ""))
    {
       var m_arr;
       var Dr=-1;
       var Mr=-1;
       var Yr=-1;
       if (value.getAttribute("MinimumValue") != "")
       {
            m_arr = value.getAttribute("MinimumValue").split(value.getAttribute("DateSeparator"));
            Dr = parseInt(m_arr[value.getAttribute("DateFormat").indexOf("D")],10);
            Mr = parseInt(m_arr[value.getAttribute("DateFormat").indexOf("M")],10);
            Yr = parseInt(m_arr[value.getAttribute("DateFormat").indexOf("Y")],10);
            if (Yr < 100)
            {
                Yr = parseInt(Yr + value.getAttribute("Century"),10);
            }
            else if (Yr < 999)
            {
                Yr += parseInt(value.getAttribute("Century").substring(0,1) + Yr,10)
            }
            ret = (Dr>0 && Mr>0 && Yr>0 && Y > Yr || (Y == Yr && M > Mr) || (Y == Yr && M == Mr && D >= Dr));
            if (!ret)
            {
                MaskedEditSetMessage(value,value.getAttribute("MinimumValueMessage"));
                MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
            }
       }
       if (ret && value.getAttribute("MaximumValue") != "")
       {
            m_arr = value.getAttribute("MaximumValue").split(value.getAttribute("DateSeparator"));
            Dr = parseInt(m_arr[value.getAttribute("DateFormat").indexOf("D")],10);
            Mr = parseInt(m_arr[value.getAttribute("DateFormat").indexOf("M")],10);
            Yr = parseInt(m_arr[value.getAttribute("DateFormat").indexOf("Y")],10);
            if (Yr < 100)
            {
                Yr = parseInt(Yr + value.getAttribute("Century"),10);
            }
            else if (Yr < 999)
            {
                Yr += parseInt(value.getAttribute("Century").substring(0,1) + Yr,10)
            }
            ret = (Dr>0 && Mr>0 && Yr>0 && Y < Yr || (Y == Yr && M < Mr) || (Y == Yr && M == Mr && D <= Dr));
            if (!ret)
            {
                MaskedEditSetMessage(value,value.getAttribute("MaximumValueMessage"));
                MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
            }
       }
    }
    if (ret && value.getAttribute("ClientValidationFunction") != "")
    {
        var args = { Value:mask, IsValid:true };
        eval(value.getAttribute("ClientValidationFunction") + "(value, args);");
        ret = args.IsValid;
        if (!ret)
        {
            MaskedEditSetMessage(value,value.getAttribute("InvalidValueMessage"));
            MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
        }
    }
    if (!ret)
    {
        MaskedEditMessageShow(value,ret);
    }
    return ret;
}

//  Validator time
function MaskedEditValidatorTime(value)
{
    MaskedEditSetMessage(value,"")
    MaskedEditSetCssClass(value,"");
    MaskedEditMessageShow(value,true);
    if (value.getAttribute("IsMaskedEdit") == "false")
    {
        return true;
    }
    var target = $get(value.getAttribute("TargetValidator")); 
    if (value.getAttribute("ValidEmpty")  == "false")
    {
        if (target.value == value.getAttribute("InitialValue"))
        {
            MaskedEditSetMessage(value,value.getAttribute("EmptyValueMessage"));
            MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
            MaskedEditMessageShow(value,false);
            return false;
        }
    }
    if (target.value == "")
    {
        return true;
    }

    if (AjaxControlToolkit.TextBoxWrapper.get_Wrapper(target).get_IsWatermarked()) {
        return true;
    }

    var ret = true;
    var mask = target.value.substring(value.getAttribute("FirstMaskPosition"),value.getAttribute("LastMaskPosition"));
    if (value.getAttribute("ValidationExpression") != "" )
    {
        var rx = new RegExp(value.getAttribute("ValidationExpression"));
        var matches = rx.exec(mask);
        ret = (matches != null && mask == matches[0]);
        if (!ret)
        {
            MaskedEditSetMessage(value,value.getAttribute("InvalidValueMessage"));
            MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
            MaskedEditMessageShow(value,false);
            return false;
        }
    }
    var ret = true;
    // hh:mm or hh:mm:ss  
    var SybTm = value.getAttribute("AmPmSymbol").split(";");
    var tm = value.getAttribute("AmPmSymbol").replace(";","|");
    var reg1 = "^(^([0][0-9]|[1][0-2]):([0-5][0-9]):([0-5][0-9])\\s("+tm+")$)|(^([0][0-9]|[1][0-2]):([0-5][0-9])\\s("+tm+")$)$";
    var reg2 = "^(^([0-1][0-9]|[2][0-3]):([0-5][0-9]):([0-5][0-9])$)|(^([0-1][0-9]|[2][0-3]):([0-5][0-9])$)$";
    var H=-1;
    var M=-1;
    var S=-1;
    var aux = "";
    var m_arrValue = mask.split(value.getAttribute("TimeSeparator"));
    var regex1 = new RegExp(reg1);
    var matches1 = regex1.exec(mask);
    var regex2 = new RegExp(reg2);
    var matches2 = regex2.exec(mask);
    if  (matches1 && (matches1[0] == mask))
    {
        aux = mask.substring(mask.length-2).substring(0,1);
        H = parseInt(m_arrValue[0],10);
        if (aux.toUpperCase() == SybTm[1].substring(0,1).toUpperCase())
        {
            H += 12;
            if (H == 24)
            {
                H = 0;
            }
        }
        M = parseInt(m_arrValue[1],10);
        S = (value.length > 9?parseInt(m_arrValue[2].substring(0,2),10):0);
    }
    else if (matches2 && (matches2[0] == mask))
    {
        H = parseInt(m_arrValue[0],10);
        M = parseInt(m_arrValue[1],10);
        S = (mask.length > 5?parseInt(m_arrValue[2],10):0);
    }
    if (H==-1 || M==-1 || S==-1)
    {
        ret = false;
    }
    if (!ret)
    {
        MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
        MaskedEditSetMessage(value,value.getAttribute("InvalidValueMessage"))
    }
    if(ret && (value.getAttribute("MaximumValue") != "" || value.getAttribute("MinimumValue") != ""))
    {
        var Hr;
        var Mr;
        var Sr;
        var m_arr;
        if (value.getAttribute("MinimumValue") != "" )
        {
            Hr=-1;
            Mr=-1;
            Sr=-1;
            m_arr = value.getAttribute("MinimumValue").split(value.getAttribute("TimeSeparator"));
            matches1 = regex1.exec(value.getAttribute("MinimumValue"));
            matches2 = regex2.exec(value.getAttribute("MinimumValue"));
            if (matches1 && (matches1[0] == value.getAttribute("MinimumValue")))
            {
                aux = value.getAttribute("MinimumValue").substring(value.getAttribute("MinimumValue").length-2).substring(0,1);
                Hr = parseInt(m_arr[0],10);
                if (aux.toUpperCase() == SybTm[1].substring(0,1).toUpperCase())
                {
                    Hr += 12;
                    if (Hr == 24)
                    {
                        Hr = 0;
                    }
                }
                Mr = parseInt(m_arr[1],10);
                Sr = (value.MinimumValue.length > 9?parseInt(m_arr[2].substring(0,2),10):0);
            }
            else if (matches2 && (matches2[0] == value.getAttribute("MinimumValue")))
            {
                Hr = parseInt(m_arr[0],10);
                Mr = parseInt(m_arr[1],10);
                Sr = (value.getAttribute("MinimumValue").length > 5?parseInt(m_arr[2],10):0);
            }
            ret = (H > Hr || (H == Hr && M > Mr) || (H == Hr && M == Mr && S >= Sr));
            if (!ret)
            {
                MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
                MaskedEditSetMessage(value,value.getAttribute("MinimumValueMessage"))
            }
        }
        if (value.getAttribute("MaximumValue") != "" && ret)
        {
            Hr=-1;
            Mr=-1;
            Sr=-1;
            m_arr = value.getAttribute("MaximumValue").split(value.getAttribute("TimeSeparator"));
            matches1 = regex1.exec(value.getAttribute("MaximumValue"));
            matches2 = regex2.exec(value.getAttribute("MaximumValue"));
            if  (matches1 && (matches1[0] == value.getAttribute("MaximumValue")))
            {
                aux = value.getAttribute("MaximumValue").substring(value.getAttribute("MaximumValue").length-2).substring(0,1);
                Hr = parseInt(m_arr[0],10);
                if (aux.toUpperCase() == SybTm[1].substring(0,1).toUpperCase())
                {
                    Hr += 12;
                    if (Hr == 24)
                    {
                        Hr = 0;
                    }
                }
                Mr = parseInt(m_arr[1],10);
                Sr = (value.getAttribute("MaximumValue").length > 9?parseInt(m_arr[2].substring(0,2),10):0);
            }
            else if (matches2 && (matches2[0] == value.getAttribute("MaximumValue")))
            {
                Hr = parseInt(m_arr[0],10);
                Mr = parseInt(m_arr[1],10);
                Sr = (value.getAttribute("MaximumValue").length > 5?parseInt(m_arr[2],10):0);
            }
            ret = (H < Hr || (H == Hr && M < Mr) || (H == Hr && M == Mr && S <= Sr));
            if (!ret)
            {
                MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
                MaskedEditSetMessage(value,value.getAttribute("MaximumValueMessage"))
            }
        }
    }
    if (ret && value.getAttribute("ClientValidationFunction") != "")
    {
        var args = { Value:mask, IsValid:true };
        eval(value.getAttribute("ClientValidationFunction") + "(value, args);");
        ret = args.IsValid;
        if (!ret)
        {
            MaskedEditSetMessage(value,value.getAttribute("InvalidValueMessage"));
            MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
        }
    }
    if (!ret)
    {
        MaskedEditMessageShow(value,ret);
    }
    return ret;        
}
//  Validator Number
function MaskedEditValidatorNumber(value)
{
    MaskedEditSetMessage(value,"")
    MaskedEditSetCssClass(value,"");
    MaskedEditMessageShow(value,true);
    if (value.getAttribute("IsMaskedEdit") == "false")
    {
        return true;
    }
    var target = $get(value.getAttribute("TargetValidator")); 
    if (value.getAttribute("ValidEmpty")  == "false")
    {
        if (target.value == value.getAttribute("InitialValue"))
        {
            MaskedEditSetMessage(value,value.getAttribute("EmptyValueMessage"));
            MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
            MaskedEditMessageShow(value,false);
            return false;
        }
    }
    if (target.value == "")
    {
        return true;
    }

    if (AjaxControlToolkit.TextBoxWrapper.get_Wrapper(target).get_IsWatermarked()) {
        return true;
    }

    var ret = true;
    var mask = target.value.substring(value.getAttribute("FirstMaskPosition"),value.getAttribute("LastMaskPosition"));
    if (value.getAttribute("ValidationExpression") != "" )
    {
        var rx = new RegExp(value.getAttribute("ValidationExpression"));
        var matches = rx.exec(mask);
        ret = (matches != null && mask == matches[0]);
        if (!ret)
        {
            MaskedEditSetMessage(value,value.getAttribute("InvalidValueMessage"));
            MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
            MaskedEditMessageShow(value,false);
            return false;
        }
    }
    ret = false;
    var cleanInput = null;
    var exp  = null;
    var m = null;
    var num = null;
    var Compnum = null;

    mask = mask.replace(new RegExp("(\\" + value.getAttribute("Thousands") + ")", "g"), "");
    mask = mask.replace(new RegExp("(\\" + value.getAttribute("Money") + ")", "g"), "");
    //trim
    m = mask.match(/^\s*(\S+(\s+\S+)*)\s*$/);
    if (m != null)
    {
        mask = m[1];
    }
    //integer
    exp = /^\s*[-\+]?\d+\s*$/;
    if (mask.match(exp) != null) 
    {
        num = parseInt(mask, 10);
        ret = (num == (isNaN(num) ? null : num));
    }
    if (ret)
    {
        if (value.getAttribute("MaximumValue") != "")
        {
            Compnum = parseInt(value.getAttribute("MaximumValue"), 10);
            if (Compnum == (isNaN(Compnum) ? null : Compnum))
            {
                if (num > Compnum)
                {
                    ret = false;
                    MaskedEditSetMessage(value,value.getAttribute("MaximumValueMessage"))
                    MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
                }
            }
        }
        if (ret && value.getAttribute("MinimumValue") != "")
        {
            Compnum = parseInt(value.getAttribute("MinimumValue"), 10);
            if (Compnum == (isNaN(Compnum) ? null : Compnum))
            {
                if (num < Compnum)
                {
                    ret = false;
                    MaskedEditSetMessage(value,value.getAttribute("MinimumValueMessage"))
                    MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
                }
            }
        }
    }
    else
    {
        //float
        exp = new RegExp("^\\s*([-\\+])?(\\d+)?(\\" + value.getAttribute("Decimal") + "(\\d+))?\\s*$");
        m = mask.match(exp);
        if (m != null)
        {
            cleanInput = null;
            if  (typeof(m[1]) != "undefined")
            {
                cleanInput = m[1] + (m[2].length>0 ? m[2] : "0") + "." + m[4];
            }
            else
            {
                cleanInput = (m[2].length>0 ? m[2] : "0") + "." + m[4];
            }
            num = parseFloat(cleanInput);
            ret = (num == (isNaN(num) ? null : num));            
        }
        if (!ret)
        {
            MaskedEditSetMessage(value,value.getAttribute("InvalidValueMessage"));
            MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
        }
        if (ret)
        {
            if (value.getAttribute("MaximumValue") != "")
            {
                Compnum = parseFloat(value.getAttribute("MaximumValue"));
                if (Compnum == (isNaN(Compnum) ? null : Compnum))
                {
                    if (num > Compnum)
                    {
                        ret = false;
                        MaskedEditSetMessage(value,value.getAttribute("MaximumValueMessage"))
                        MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
                    }
                }
            }
            if (ret && value.getAttribute("MinimumValue") != "")
            {
                Compnum = parseFloat(value.getAttribute("MinimumValue"));
                if (Compnum == (isNaN(Compnum) ? null : Compnum))
                {
                    if (num < Compnum)
                    {
                        ret = false;
                        MaskedEditSetMessage(value,value.getAttribute("MinimumValueMessage"))
                        MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
                    }
                }
            }
        }
    }
    if (ret && value.getAttribute("ClientValidationFunction") != "")
    {
        var args = { Value:mask, IsValid:true };
        eval(value.getAttribute("ClientValidationFunction") + "(value, args);");
        ret = args.IsValid;
        if (!ret)
        {
            MaskedEditSetMessage(value,value.getAttribute("InvalidValueMessage"));
            MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
        }
    }
    if (!ret)
    {
        MaskedEditMessageShow(value,ret);
    }
    return ret;        
}
//  Validator None
function MaskedEditValidatorNone(value)
{
    MaskedEditSetMessage(value,"")
    MaskedEditSetCssClass(value,"");
    MaskedEditMessageShow(value,true);
    if (value.getAttribute("IsMaskedEdit") == "false")
    {
        return true;
    }
    var target = $get(value.getAttribute("TargetValidator")); 
    if (value.getAttribute("ValidEmpty")  == "false")
    {
        if (target.value == value.getAttribute("InitialValue"))
        {
            MaskedEditSetMessage(value,value.getAttribute("EmptyValueMessage"));
            MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
            MaskedEditMessageShow(value,false);
            return false;
        }
    }
    if (target.value == "")
    {
        return true;
    }

    if (AjaxControlToolkit.TextBoxWrapper.get_Wrapper(target).get_IsWatermarked()) {
        return true;
    }

    var ret = true;
    var mask = target.value.substring(value.getAttribute("FirstMaskPosition"),value.getAttribute("LastMaskPosition"));
    if (value.getAttribute("ValidationExpression") != "" )
    {
        var rx = new RegExp(value.getAttribute("ValidationExpression"));
        var matches = rx.exec(mask);
        ret = (matches != null && mask == matches[0]);
        if (!ret)
        {
            MaskedEditSetMessage(value,value.getAttribute("InvalidValueMessage"));
            MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
            MaskedEditMessageShow(value,false);
            return false;
        }
    }
    var exp = /^\d+\s*$/;
    var num = null;
    if (value.getAttribute("MaximumValue") != "")
    {
        if (value.getAttribute("MaximumValue").match(exp) != null) 
        {
            num = parseInt(value.getAttribute("MaximumValue"), 10);
            if (num == (isNaN(num) ? null : num))
            {
                if (mask.length > num)
                {
                    ret = false;
                    MaskedEditSetMessage(value,value.getAttribute("MaximumValueMessage"));
                    MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
                }
            }
        }
    }
    if (ret && value.getAttribute("MinimumValue") != "")
    {
        if (value.getAttribute("MinimumValue").match(exp) != null) 
        {
            num = parseInt(value.getAttribute("MinimumValue"), 10);
            if (num == (isNaN(num) ? null : num))
            {
                if (mask.length < num)
                {
                    ret = false;
                    MaskedEditSetMessage(value,value.getAttribute("MinimumValueMessage"));
                    MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
                }
            }
        }
    }
    if (ret && value.getAttribute("ClientValidationFunction") != "")
    {
        var args = { Value:mask, IsValid:true };
        eval(value.getAttribute("ClientValidationFunction") + "(value, args);");
        ret = args.IsValid;
        if (!ret)
        {
            MaskedEditSetMessage(value,value.getAttribute("InvalidValueMessage"));
            MaskedEditSetCssClass(value,value.getAttribute("InvalidValueCssClass"));
        }
    }
    if (!ret)
    {
        MaskedEditMessageShow(value,ret);
    }
    return ret;        
}

/* 
*  Check the length of the valid characters except the "-" and " " for the SSN.
*  If the length more than 9, indicate the SSN is invalid.
*/
function CheckSSNValidLength(source, args) {
    if (!args || !args.IsValid || args.Value.length == 0) {
        return;
    }

    var validChars = args.Value.replace(/-/g, '').replace(/ /g, '');

    if (validChars.length > 9) {
        args.IsValid = false;
    }
}