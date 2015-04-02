/**
 * <pre>
 * 
 *  Accela
 *  File: Education.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2013
 * 
 *  Description:
 * To deal with some logic for Education.
 *  Notes:
 * $Id: Education.js 269289 2014-04-09 10:23:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
 
    var maxLength = 38;
    
    // create table by list and method name.  
    function CreateTable(list,methodName)
    {
        var tb = document.createElement("table");
        var recordStyle;
        
        for (var i=0;i<list.length;i++)
        {
           var row = document.createElement("tr");   
           var cell = document.createElement("td");  
           
           if(i%2 == 0)
           {
                recordStyle = 'ACA_TabRow_Single_Line font12px';
           }
           else
           {
                recordStyle = 'ACA_TabRow_Double_Line font12px';
           }  
           
           cell.innerHTML = "<a href='#' title=\""+list[i]+"\" onclick='" + methodName + "(\""+list[i]+"\")' class=\"" + recordStyle +"\"><span>" + TruncateString(list[i]) + "</span></a>";
        
           row.appendChild(cell);   
           tb.appendChild(row);  
        } 
        
        return tb;
    }
    
    // truncate string.
    function TruncateString(value)
    {
        var name;
        
        if (value.length > maxLength)
        {
            name = value.toString().substring(0,maxLength) + "...";
        }
        else
        {
            name = value;
        }
        
        return name;
    }

    function openEduExamEditDialog(ctrlId1, ctrlId2, actionMenuId) {
 
        var ctrlId = ctrlId1;
        var scrollId = ctrlId1;

        if (!$('#' + ctrlId1).is(':visible')) {
            ctrlId = ctrlId2;
            scrollId = actionMenuId;
        }

        scrollIntoView(scrollId);
        
        $(document).ready(function () {
            invokeClick($get(ctrlId));
        });
    }