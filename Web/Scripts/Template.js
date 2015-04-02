/**
 * <pre>
 * 
 *  Accela
 *  File: Template.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: Template.js 145398 2009-08-31 09:47:37Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *
 * </pre>
 */
function GetEMSEDDLOption(obj, ddlLPType, licenseAgency)
{
    ////'obj' is the licensed professional number control, 'ddlLPType' is the LP Type control client id,'templateEdit' is the template user control client id
    if(obj == null  || obj.readOnly || obj.disabled  || ddlLPType == null || ddlLPType == "" || templateEdit == null || templateEdit == ""
    || licenseAgency == null || licenseAgency == "")
    {
        return;
    }
    
    ClearEMSEDDLOption();
    
    //get licensed professional type control
    var lpType = $get(ddlLPType);
    if(lpType == null)
    {
        return;
    }
    
    //get license agency control 
    var lpAgency = $get(licenseAgency);
    if(licenseAgency == null)
    {
        return;
    }
    
    var lpSelectedValue = lpType.value;
    
    if(lpSelectedValue != "" && obj.value != null && obj.value.replace(/\s/g,"") !="")
    {
        var templateContainer = $get(templateEdit); 
        if(templateContainer != null)
        {
            var controls = templateContainer.getElementsByTagName("select");
            var hasEMSEDDL = false;
            
            for (var i = 0; i < controls.length; i++) 
            { 
                if (controls[i].attributes[templateEMSEDDLATTRIBUTEFORNAME] != null 
                && controls[i].attributes[templateEMSEDDLATTRIBUTEFORNAME].nodeValue != '') 
                {
                    hasEMSEDDL = true;
                    break;
                }
            }    
        
            if(hasEMSEDDL)
            {
                var lpNum = obj.value;
                Accela.ACA.Web.WebService.TemplateService.GetEMSEDDOption(lpSelectedValue, lpNum, lpAgency.value, CallbackEMSEDDLOption, null);
            }
        }    
    }
}

function CallbackEMSEDDLOption(result)
{
    if(result == null || result == '' || result == 'undefined ')
    {
        return;
    }
    var templateContainer = $get(templateEdit); 
    
    if(templateContainer == null)
    {
        return;
    }
    
    var valueList = eval(result);    
        
    var controls = templateContainer.getElementsByTagName("select");
    
    //circle to find the EMSE drop down list.
    for (var i = 0; i < controls.length; i++) 
    { 
       if (controls[i].attributes["EMSETemplate"] != null && controls[i].attributes["EMSETemplate"].nodeValue != '') 
       {
           var templateName = controls[i].attributes["EMSETemplate"].nodeValue;
           var curDDL = controls[i];
           //clear old options.
           curDDL.options.length = 0;
           //add a default option("--Select--").
           curDDL.options.add(new Option(defaultSelectText, ''));
           
           //circle to find the EMSE Drop down list's options.
           for(var j = 0 ; j < valueList.length ; j++)
           {
               if (templateName == valueList[j].AttributeName)
               {
                    //if no option, add the 
                    curDDL.options.add(new Option(valueList[j].ListText ,valueList[j].ListValue));                
               }
           }
       }
    }    
}

function ClearEMSEDDLOption()
{
    var templateContainer = $get(templateEdit); 
    
    if(templateContainer == null)
    {
        return;
    }
        
    var controls = templateContainer.getElementsByTagName("select");
    
    //circle to find the EMSE drop down list.
    for (var i = 0; i < controls.length; i++) 
    { 
       if (controls[i].attributes["EMSETemplate"] != null && controls[i].attributes["EMSETemplate"].nodeValue != '') 
       {
            var curDDL = controls[i];
           //clear old options.
           curDDL.options.length = 0;
           //add a default option("--Select--").
           curDDL.options.add(new Option(defaultSelectText, ''));
       }
    }    
}