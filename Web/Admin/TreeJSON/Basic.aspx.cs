#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Basic.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: Basic.aspx.cs 258061 2013-10-09 02:06:24Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using System.Text;

using Accela.ACA.BLL.Admin;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;

/// <summary>
/// Basic page for json tree
/// </summary>
public partial class Admin_TreeJSON_Basic : System.Web.UI.Page
{
    #region Methods

    /// <summary>
    /// Raises the page load event
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!AppSession.IsAdmin)
        {
            Response.Redirect("../login.aspx");
        }

        IAdminBll treeBll = (IAdminBll)ObjectFactory.GetObject(typeof(IAdminBll));
        DataTable treeData = treeBll.GetTreeNodes();
        this.GridView1.DataSource = treeData; // FilterDataTable(treeData, "PARENTID=1893");
        this.GridView1.DataBind();

        this.GridView2.DataSource = FilterDataTable(treeData, "ParentID='2179'");
        this.GridView2.DataBind();

        //this.Response.Write(CreateJsonParameters(FilterDataTable(treeData, "PARENTID=1893")));
    }

    /// <summary>
    /// Create json format parameters
    /// </summary>
    /// <param name="dt">Parameter data table</param>
    /// <returns>Json format string</returns>
    private string CreateJsonParameters(DataTable dt)
    {
        StringBuilder jsonString = new StringBuilder();

        //Exception Handling
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonString.Append("{ ");

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j <= dt.Columns.Count - 1)
                    {
                        jsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                    }
                    else if (j == dt.Columns.Count - 1)
                    {
                        jsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                    }
                }

                /*end Of String*/
                if (i == dt.Rows.Count - 1)
                {
                    jsonString.Append("} ");
                }
                else
                {
                    jsonString.Append("}, ");
                }
            }

            return jsonString.ToString();
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Filter data by condition
    /// </summary>
    /// <param name="dt">data table.</param>
    /// <param name="condition">condition use to filter</param>
    /// <returns>filtered data table</returns>
    private DataTable FilterDataTable(DataTable dt, string condition)
    {
        DataTable newdt = new DataTable();
        newdt = dt.Clone();
        DataRow[] dr = dt.Select(condition);

        for (int i = 0; i < dr.Length; i++)
        {
            newdt.ImportRow((DataRow)dr[i]);
        }

        return newdt;
    }

    #endregion Methods
}