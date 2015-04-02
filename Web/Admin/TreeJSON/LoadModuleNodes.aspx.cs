#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LoadModuleNodes.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: LoadModuleNodes.aspx.cs 277316 2014-08-14 02:02:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Accela.ACA.BLL.Admin;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Web.Common;

/// <summary>
/// Page to load module nodes
/// </summary>
public partial class Admin_TreeJSON_LoadModuleNodes : System.Web.UI.Page
{
    #region Fields

    /// <summary>
    /// Gets or sets module nodes
    /// </summary>
    protected string ModuleNodes 
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets modules
    /// </summary>
    private IList<string> ActiveModules
    {
        get
        {
            if (Session[SessionConstant.SESSION_ACTIVE_MODULES_FOR_ADMIN] == null)
            {
                return null;
            }

            return (IList<string>)Session[SessionConstant.SESSION_ACTIVE_MODULES_FOR_ADMIN];
        }

        set
        {
            Session[SessionConstant.SESSION_ACTIVE_MODULES_FOR_ADMIN] = value;
        }
    }

    #endregion Fields

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

        if (!this.Page.IsPostBack)
        {
            ModuleNodes = GetModuleNodes();
        }
    }

    /// <summary>
    /// Filter data from data table
    /// </summary>
    /// <param name="dt">data table</param>
    /// <param name="condition">condition use to filter data</param>
    /// <returns>data table that has filtered data</returns>
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
    /// Get Module Nodes
    /// </summary>
    /// <returns>return string</returns>
    private string GetModuleNodes()
    {
        IAdminBll treeBll = (IAdminBll)ObjectFactory.GetObject(typeof(IAdminBll));
        DataTable treeData = treeBll.GetTreeNodes();
        Session["TreeNodes"] = treeData;

        DataTable tmpNotes = null;

        StringBuilder sb = new StringBuilder();
        sb.Append("Ext.onReady(function(){\n");
        sb.Append("var myBar = new OutlookBar();\n");

        DataTable dtModules = FilterDataTable(treeData, "ParentID=0");

        HandleActiveModules(dtModules);

        // the all tree nodes
        /*format: 
         *  [{  module: 'General',
                section: [{
                    root: 1002, 
                    nodes: [{ id: 1 }, { id: 2}]
                }]
            },
            {   module: 'Building',
                section: [{
                    root: 1024,
                    nodes: [{ id: 11 }, { id: 12}]
                }]
            }];
         */
        StringBuilder allTreeNodes = new StringBuilder();
        allTreeNodes.Append("[");

        for (int i = 0; i < dtModules.Rows.Count; i++)
        {
            string elementName = LabelUtil.RemoveHtmlFormat(ScriptFilter.FilterJSChar(dtModules.Rows[i]["ElementName"].ToString()));
            string moduleName = ScriptFilter.AntiXssUrlEncode(dtModules.Rows[i]["RootNodeName"].ToString());

            sb.AppendFormat("myBar.addTitle('{0}', '{1}');\n", elementName.Replace("'", "&#39;").Replace("\"", "&quot;"), moduleName);
            allTreeNodes.AppendFormat("{{module: '{0}',section: [", moduleName);
            
            tmpNotes = FilterDataTable(treeData, "ParentID='" + dtModules.Rows[i]["ElementID"].ToString() + "'");

            for (int j = 0; j < tmpNotes.Rows.Count; j++)
            {
                int isSettings = -1;
                int isPageFlowConfiguration = -1;
                string flag = string.Empty;
                isSettings = tmpNotes.Rows[j]["ElementName"].ToString().IndexOf("Settings", StringComparison.InvariantCulture);
                isPageFlowConfiguration = tmpNotes.Rows[j]["ElementName"].ToString().IndexOf("Page Flow Configuration", StringComparison.InvariantCulture);

                if (isSettings >= 0)
                {
                    flag = "1";
                }

                if (isPageFlowConfiguration >= 0)
                {
                    flag = "2";
                }

                string rootId = tmpNotes.Rows[j]["ElementID"].ToString();
                sb.Append("myBar.addItem(" + i + ",'" + ScriptFilter.FilterJSChar(tmpNotes.Rows[j]["ElementName"].ToString()) + "','100px','TreeJSON/HomeTab.aspx?icon=" + flag + "&root=" + rootId
                          + "&moduleName=" + moduleName + "');\n");

                string sectionTreeNodes = GetSectionTreeNodes(treeData, rootId, moduleName, flag);
                allTreeNodes.Append("{");
                allTreeNodes.AppendFormat("root:{0},nodes:{1}", rootId, sectionTreeNodes);
                allTreeNodes.Append("},");
            }

            if (allTreeNodes.ToString().EndsWith(","))
            {
                allTreeNodes.Remove(allTreeNodes.Length - 1, 1);
            }

            allTreeNodes.Append("]},");
        }

        if (allTreeNodes.ToString().EndsWith(","))
        {
            allTreeNodes.Remove(allTreeNodes.Length - 1, 1);
        }

        allTreeNodes.Append("]");

        sb.AppendFormat("     myBar.createTreeNodesObj(\"{0}\");\n", allTreeNodes.Replace("\n", string.Empty));
        sb.Append("     myBar.renderTo('outlookBar');\n");
        sb.Append("});");

        return sb.ToString();
    }

    /// <summary>
    /// Compare the current modules with the former when user refresh the page. 
    /// If they are different(has active/inactive modules), clear the TreeNodes Session. 
    /// </summary>
    /// <param name="dtModules">data table for Modules.</param>
    private void HandleActiveModules(DataTable dtModules)
    {
        if (dtModules == null || dtModules.Rows.Count == 0)
        {
            return;
        }

        // if the modules has changed,clear TreeNodes Session and update Module Session with latest data.
        if (ActiveModules == null || IsActiveModulesChanged(dtModules))
        {
            Session["TreeNodes"] = null;

            ActiveModules = new List<string>();

            // update module session
            for (int i = 0; i < dtModules.Rows.Count; i++)
            {
                ActiveModules.Add(Convert.ToString(dtModules.Rows[i]["ElementName"]));
            }
        }
    }

    /// <summary>
    /// Check if the modules has been changed.
    /// </summary>
    /// <param name="dtModules">data table for Modules.</param>
    /// <returns>true if has changed module;otherwise,false.</returns>
    private bool IsActiveModulesChanged(DataTable dtModules)
    {
        bool hasChangedModule = false;

        if (dtModules != null && ActiveModules != null)
        {
            // has active/inactive module(s).
            if (ActiveModules.Count != dtModules.Rows.Count)
            {
                hasChangedModule = true;
            }
            else
            {
                for (int i = 0; i < dtModules.Rows.Count; i++)
                {
                    string moduleName = Convert.ToString(dtModules.Rows[i]["ElementName"]);

                    // has changed modules.
                    if (ActiveModules[i] != moduleName)
                    {
                        hasChangedModule = true;

                        break;
                    }
                }
            }
        }

        return hasChangedModule;
    }

    /// <summary>
    /// Get section tree nodes.
    /// </summary>
    /// <param name="treeData">The tree data.</param>
    /// <param name="rootId">The root id.</param>
    /// <param name="moduleName">The module name.</param>
    /// <param name="icon">The icon.</param>
    /// <returns>The section tree nodes.</returns>
    private string GetSectionTreeNodes(DataTable treeData, string rootId, string moduleName, string icon)
    {
        DataTable parentNodes = null;
        DataTable childNodes = null;
        StringBuilder sb = new StringBuilder();
        parentNodes = FilterDataTable(treeData, "ParentID='" + rootId + "'");

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
                    sb.Append("     iconCls:'x-tree-node-settings',\n");
                }

                if (icon.Equals("2", StringComparison.InvariantCulture))
                {
                    sb.Append("     iconCls:'x-tree-node-pageflow',\n");
                }

                bool hasChildren = HasChildren(treeData, parentNodes.Rows[i]["ElementID"].ToString());
                sb.AppendFormat("     leaf:{0}\n", hasChildren ? ACAConstant.COMMON_FALSE : ACAConstant.COMMON_TRUE);

                // append the child notes.
                if (hasChildren)
                {
                    AppendChildNotes(sb, treeData, parentNodes.Rows[i]["ElementID"].ToString(), moduleName);
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
    /// Append the child notes.
    /// </summary>
    /// <param name="sb">The String builder object.</param>
    /// <param name="treeData">The admin tree data.</param>
    /// <param name="parentId">The parent id.</param>
    /// <param name="moduleName">The module name.</param>
    private void AppendChildNotes(StringBuilder sb, DataTable treeData, string parentId, string moduleName)
    {
        DataTable childNodes = FilterDataTable(treeData, "ParentID='" + parentId + "'");

        if (childNodes.Rows.Count > 0)
        {
            sb.Append(",\n");
            sb.Append("children: [");

            foreach (DataRow childRow in childNodes.Rows)
            {
                bool hasChildren = HasChildren(treeData, childRow["ElementID"].ToString());
                string elementTitle = LabelUtil.RemoveHtmlFormat(childRow["ElementName"].ToString());

                if (!string.IsNullOrEmpty(elementTitle))
                {
                    elementTitle = elementTitle.Trim();
                }

                string url = LabelUtil.RemoveHtmlFormat(childRow["ActionURL"].ToString());
                url += url.EndsWith(".aspx", StringComparison.InvariantCultureIgnoreCase) ? "?isAdmin=Y" : "&isAdmin=Y";

                sb.Append("{\n");
                sb.AppendFormat("     idft:'{1}{0}{2}{0}{3}{0}{4}',\n", ACAConstant.SPLIT_CHAR, childRow["ElementID"], childRow["isUsedDaily"], moduleName, elementTitle.Replace(" ", string.Empty).Replace("&amp;", string.Empty));
                sb.AppendFormat("     moduleName:'{0}',\n", moduleName);
                sb.AppendFormat("     text:'{0}',\n", elementTitle);
                sb.AppendFormat("     isUsedDaily:'{0}',\n", childRow["isUsedDaily"]);
                sb.AppendFormat("     url:'{0}',\n", url);
                sb.AppendFormat("     leaf:{0}\n", hasChildren ? ACAConstant.COMMON_FALSE : ACAConstant.COMMON_TRUE);

                // append the inner child notes
                if (hasChildren)
                {
                    AppendChildNotes(sb, treeData, childRow["ElementID"].ToString(), moduleName);
                }

                sb.Append("},\n");
            }

            // remove the last string ",\n";
            sb.Remove(sb.Length - 2, 2);
            sb.Append("\n]");
        }
    }

    /// <summary>
    /// Indicates that the node has child node or not.
    /// </summary>
    /// <param name="dt">The data table</param>
    /// <param name="parentId">The parent id.</param>
    /// <returns>Return true or false.</returns>
    private bool HasChildren(DataTable dt, string parentId)
    {
        DataTable tmpNotes = FilterDataTable(dt, "ParentID='" + parentId + "'");

        return tmpNotes.Rows.Count > 0;
    }

    #endregion Methods
}
