#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: HomeTab.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: HomeTab.aspx.cs 277316 2014-08-14 02:02:02Z ACHIEVO\james.shi $.
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
using Accela.ACA.Common.Common;
using Accela.ACA.Web.Common;

/// <summary>
/// Page to get tree nodes
/// </summary>
public partial class Admin_TreeJSON_HomeTab : System.Web.UI.Page
{
    #region Fields

    /// <summary>
    /// Gets or sets tree nodes
    /// </summary>
    private string _treeNodes = string.Empty;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets tree data table
    /// </summary>
    public DataTable TreeData
    {
        get
        {
            if (Session["TreeNodes"] == null)
            {
                IAdminBll treeBll = (IAdminBll)ObjectFactory.GetObject(typeof(IAdminBll));
                Session["TreeNodes"] = treeBll.GetTreeNodes();
            }

            return (DataTable)Session["TreeNodes"];
        }
    }

    /// <summary>
    /// Gets or sets tree nodes
    /// </summary>
    public string TreeNodes
    {
        get
        {
            return _treeNodes;
        }

        set
        {
            _treeNodes = value;
        }
    }

    #endregion Properties

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

        string rootId = string.Empty;
        string moduleName = string.Empty;
        string icon = string.Empty;

        if (!this.Page.IsPostBack)
        {
            if (this.Request.QueryString["root"] != null && this.Request.QueryString["root"] != string.Empty)
            {
                rootId = this.Request.QueryString["root"].ToString();
                moduleName = ScriptFilter.AntiXssUrlEncode(Convert.ToString(this.Request.QueryString["moduleName"]));
                icon = this.Request.QueryString["icon"].ToString();
                TreeNodes = GetTreeNodes(rootId, moduleName, icon);
            }
        }
    }

    /// <summary>
    /// Filter data from data table
    /// </summary>
    /// <param name="dt">table table</param>
    /// <param name="condition">condition that use to filter data</param>
    /// <returns>data table</returns>
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

    /// <summary>
    /// Get Tree Nodes
    /// </summary>
    /// <param name="rootId">string root node id.</param>
    /// <param name="moduleName">module name.</param>
    /// <param name="icon">icon use to display</param>
    /// <returns>return string</returns>
    private string GetTreeNodes(string rootId, string moduleName, string icon)
    {
        DataTable parentNodes = null;
        DataTable childNodes = null;
        StringBuilder sb = new StringBuilder();
        parentNodes = FilterDataTable(TreeData, "ParentID='" + rootId + "'");

        if (parentNodes.Rows.Count > 0)
        {
            sb.Append("[");

            for (int i = 0; i < parentNodes.Rows.Count; i++)
            {
                string elementName = LabelUtil.RemoveHtmlFormat(parentNodes.Rows[i]["ElementName"].ToString());
                sb.Append("{");
                sb.Append("     idft:'" + parentNodes.Rows[i]["ElementID"].ToString() + ACAConstant.SPLIT_CHAR + parentNodes.Rows[i]["isUsedDaily"].ToString() + ACAConstant.SPLIT_CHAR + moduleName + ACAConstant.SPLIT_CHAR
                          + elementName.Replace(" ", string.Empty).Replace("&amp;", string.Empty) + "',\n");
                sb.Append("     moduleName:'" + moduleName + "',\n");
                sb.Append("     text:'" + elementName + "',\n");
                sb.Append("     isUsedDaily:'" + parentNodes.Rows[i]["isUsedDaily"].ToString() + "',\n");
                sb.Append("     url:'" + parentNodes.Rows[i]["ActionURL"].ToString() + "?moduleName=" + parentNodes.Rows[i]["RootNodeName"] + "&isAdmin=Y',\n");
                
                if (icon.Equals("1", StringComparison.InvariantCulture))
                {
                    sb.Append("     iconCls:\"x-tree-node-settings\",\n");
                }
                
                if (icon.Equals("2", StringComparison.InvariantCulture))
                {
                    sb.Append("     iconCls:\"x-tree-node-pageflow\",\n");
                }

                sb.Append("     leaf:" + HasChildren(TreeData, parentNodes.Rows[i]["ElementID"].ToString()) + "\n");

                if (FilterDataTable(TreeData, "ParentID='" + parentNodes.Rows[i]["ElementID"].ToString() + "'").Rows.Count > 0)
                {
                    sb.Append(",\n");
                    childNodes = FilterDataTable(TreeData, "ParentID='" + parentNodes.Rows[i]["ElementID"].ToString() + "'");
                    sb.Append("children: [");

                    for (int j = 0; j < childNodes.Rows.Count; j++)
                    {
                        string elementTitle = LabelUtil.RemoveHtmlFormat(childNodes.Rows[j]["ElementName"].ToString());

                        if (!string.IsNullOrEmpty(elementTitle))
                        {
                            elementTitle = elementTitle.Trim();
                        }

                        string url = LabelUtil.RemoveHtmlFormat(childNodes.Rows[j]["ActionURL"].ToString());

                        if (url.EndsWith(".aspx", StringComparison.InvariantCultureIgnoreCase))
                        {
                            url += "?isAdmin=Y";
                        }
                        else
                        {
                            url += "&isAdmin=Y";
                        }

                        sb.Append("{\n");
                        sb.Append("     idft:'" + childNodes.Rows[j]["ElementID"].ToString() + ACAConstant.SPLIT_CHAR + childNodes.Rows[j]["isUsedDaily"].ToString() + ACAConstant.SPLIT_CHAR + moduleName
                                  + ACAConstant.SPLIT_CHAR + elementTitle.Replace(" ", string.Empty).Replace("&amp;", string.Empty) + "',\n");
                        sb.Append("     moduleName:'" + moduleName + "',\n");
                        sb.Append("     text:'" + elementTitle + "',\n");
                        sb.Append("     isUsedDaily:'" + childNodes.Rows[j]["isUsedDaily"].ToString() + "',\n");
                        sb.Append("     url:'" + url + "',\n");
                        sb.Append("     leaf:" + HasChildren(TreeData, childNodes.Rows[j]["ElementID"].ToString()) + "\n");
                        sb.Append("}\n");

                        if (j != childNodes.Rows.Count - 1)
                        {
                            sb.Append(",\n");
                        }
                    }

                    sb.Append("]");
                }

                sb.Append("}\n");

                if (i != parentNodes.Rows.Count - 1)
                {
                    sb.Append(",");
                }
            }

            sb.Append("]");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Check the tree has child or not
    /// </summary>
    /// <param name="dt">data table.</param>
    /// <param name="notes">parent id.</param>
    /// <returns>"true" if has child not,otherwise,false.</returns>
    private string HasChildren(DataTable dt, string notes)
    {
        DataTable tmpNotes = null;
        tmpNotes = FilterDataTable(dt, "ParentID='" + notes + "'");

        if (tmpNotes.Rows.Count == 0)
        {
            return "true";
        }
        else
        {
            return "false";
        }
    }

    #endregion Methods
}
