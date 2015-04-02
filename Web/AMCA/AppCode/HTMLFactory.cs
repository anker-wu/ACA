/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: HTMLFactory.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2013
 * 
 *  Description:
 *  All of global session objects should be definted in this class.
 *  Notes:
 *      $Id: AppSession.cs 77905 2007-10-15 12:49:28Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text; 

/// <summary>
/// Summary description for HTMLFactory
/// </summary>
public class HTMLFactory
{
    /// <summary>
    /// Link button with new line
    /// </summary>
    /// <param name="URL"> Link URL</param>
    /// <param name="Label"> Link Label</param>
    /// <returns></returns>
    public string PresentLinkBr(string URL, string Label)
    {
        return "<a href=\"" + URL + "\">" + Label + "</a><br />";
    }

    /// <summary>
    /// Link button
    /// </summary>
    /// <param name="URL">Link URL</param>
    /// <param name="Label">Link Label</param>
    /// <returns></returns>
    public string PresentLink(string URL, string Label)
    {
        return "<a href=\"" + URL + "\">" + Label + "</a>";
    }

    public string Bold(string value)
    {
        return "<b>" + value + "</b>";
    }

    /// <summary>
    /// Table row with colspan 2 
    /// </summary>
    /// <param name="Label">column content</param>
    /// <param name="ClassName"> column style CSS </param>
    /// <returns></returns>
    public string Present2ColumnTableRow(string Label, string ClassName)
    {
        return "<tr><td colspan=\"2\" class=\"" + ClassName + "\">" + Label + "</td></tr>";
    }

    /// <summary>
    /// Table column
    /// </summary>
    /// <param name="Label">column content</param>
    /// <param name="ClassName">column style CSS </param>
    /// <param name="Colspan">column span</param>
    /// <returns></returns>
    public string PresentTableColumn(string Label, string ClassName, string Colspan)
    {
        return "<td colspan=\"" + Colspan + "\" class=\"" + ClassName + "\">" + Label + "</td>";
    }

    /// <summary>
    /// Table row with colspan 2 with no CSS
    /// </summary>
    /// <param name="Label"> column content</param>
    /// <returns></returns>
    public string Present2ColumnTableRow(string Label )
    {
        return Present2ColumnTableRow(Label, "");
    }

    /// <summary>
    /// Option dropdown
    /// </summary>
    /// <param name="FreezeDriedArray"> dropdown array</param>
    /// <param name="SelectName"> control name</param>
    /// <returns></returns>
    public string PresentSelectFromCachedArray(string FreezeDriedArray, string SelectName)
    {
        string[] InputArray = FreezeDriedArray.Split(',');
        StringBuilder Output = new StringBuilder();

        if (InputArray.Length > 0)
        {
            Output.Append("<select name=\"" + SelectName + "\">");

           Output.Append(@"<option value='' selected>" + "--select--" + "</option>");
           for (int counter = 0; counter < InputArray.Length; counter++)
            {
                string[] InputArrayDetail = InputArray[counter].Split(';');
                if (InputArrayDetail.Length == 2)
                {
                    Output.Append("<option value=\"" + InputArrayDetail[1] + "\">" + InputArrayDetail[0] + "</option>");
                }
                else
                {
                    Output.Append("<option value=\"" + InputArrayDetail[0] + "\">" + InputArrayDetail[0] + "</option>");
                }
            }
            Output.Append("</select>");
        }

        return Output.ToString();
    }

    /// <summary>
    /// Option dropdown
    /// </summary>
    /// <param name="FreezeDriedArray">dropdown array</param>
    /// <param name="SelectName">control name</param>
    /// <param name="ValueSelected">Value Selected</param>
    /// <returns></returns>
    public string PresentSelectFromCachedArray(string FreezeDriedArray, string SelectName, string ValueSelected)
    {
        string[] InputArray = FreezeDriedArray.Split(',');
        StringBuilder Output = new StringBuilder();

        if (InputArray.Length > 0)
        {
            Output.Append("<select name=\"" + SelectName + "\">");

            for (int counter = 0; counter < InputArray.Length; counter++)
            {
                if (InputArray[counter] == ValueSelected)
                    Output.Append("<option selected=\"" + InputArray[counter] + "\"  value=\"" + InputArray[counter] + "\">" + InputArray[counter] + "</option>");
                else
                    Output.Append("<option value=\"" + InputArray[counter] + "\">" + InputArray[counter] + "</option>");
            }
            Output.Append("</select>");
        }

        return Output.ToString();
    }

    /// <summary>
    /// Option button list
    /// </summary>
    /// <param name="FreezeDriedArray">List values</param>
    /// <param name="Name">Control name</param>
    /// <param name="Type">Type</param>
    /// <returns></returns>
    public string PresentOptionButtonList(string FreezeDriedArray, string Name, string Type)
    {
        string[] InputArray = FreezeDriedArray.Split(',');
        StringBuilder Output = new StringBuilder();

        if (InputArray.Length > 0)
        {
            for (int counter = 0; counter < InputArray.Length; counter++)
            {
                if (counter == 0)
                {
                    Output.Append("<input name=\"" + Name + "\" checked=\"checked\" type=\"" + Type + "\" value=\"" + InputArray[counter] + "\" />" + InputArray[counter] + "<br />");
                }
                else
                {
                    Output.Append("<input name=\"" + Name + "\" type=\"" + Type + "\" value=\"" + InputArray[counter] + "\" />" + InputArray[counter] + "<br />");
                }
            }
        }
        return Output.ToString();
    }

    /// <summary>
    /// From hidden field
    /// </summary>
    /// <param name="Name">Name </param>
    /// <param name="Value">Value</param>
    /// <returns></returns>
    public string PresentHiddenField( string Name, string Value )
    {
        return "<input name=\"" + Name + "\"  id=\"" + Name + "\" type=\"hidden\" value=\"" + Value + "\" />";
    }

    /// <summary>
    /// Submit button with New line
    /// </summary>
    /// <param name="Name">Name</param>
    /// <param name="Value">value</param>
    /// <returns></returns>
    public string PresentSubmitItemBr(string Name, string Value)
    {
        return "<input type=\"submit\" name=\"" + Name + "\"  value=\"" + Value + "\"><br />";
    }

    /// <summary>
    /// If the browser is Opera , then use button tag to present submit button, otherwise use input tag.
    /// </summary>
    /// <param name="isOpera">is Opera browser</param>
    /// <param name="name">name</param>
    /// <param name="value">value</param>
    /// <param name="style">style</param>
    /// <returns></returns>
    public string PresentSubmitButton(bool isOpera, string name, string value, string style)
    {
        if (!string.IsNullOrEmpty(style))
        {
            style = "style=\"" + style + "\"";
        }

        if (isOpera)
        {
            return "<button type=\"submit\" name=\"" + name + "\" " + style + "><span>" + value + "</span></button>";
        }
        else
        {
            return "<input type=\"submit\" name=\"" + name + "\" value=\"" + value + "\" " + style + "/>";
        }
    }

    /// <summary>
    /// If the browser is Opera , then use button tag to present submit button, otherwise use input tag.
    /// </summary>
    /// <param name="isOpera">is Opera browser</param>
    /// <param name="name">name</param>
    /// <param name="value">value</param>
    /// <returns></returns>
    public string PresentSubmitButton(bool isOpera, string name, string value)
    {
        return PresentSubmitButton(isOpera, name, value, string.Empty);
    }
}
