#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TreeAppStatus.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: TreeAppStatus.aspx.cs 277316 2014-08-14 02:02:02Z ACHIEVO\james.shi $.
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
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Security;

/// <summary>
/// Get node tree application status
/// </summary>
[SuppressCsrfCheck]
public partial class Admin_TreeJSON_TreeAppStatus : System.Web.UI.Page
{
    #region Fields

    /// <summary>
    /// Create an instance of Admin BBLL
    /// </summary>
    private IAdminBll _adminBll = (IAdminBll)ObjectFactory.GetObject(typeof(IAdminBll));

    /// <summary>
    /// module name
    /// </summary>
    private string moduleName = string.Empty;

    /// <summary>
    /// Create a StringBuilder
    /// </summary>
    private StringBuilder sb = new StringBuilder();

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets or sets json format string to store the tree nodes
    /// </summary>
    protected string JsonTree { get; set; }

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

        if (!IsPostBack)
        {
            if (Request["ModuleName"] != null && Request["ModuleName"].ToString() != string.Empty)
            {
                moduleName = Request["ModuleName"].ToString();
            }

            JsonTree = this.GetTree(moduleName);
        }
    }

    /// <summary>
    /// Filter data from data table
    /// </summary>
    /// <param name="dt">data table</param>
    /// <param name="cond">condition use to filter data</param>
    /// <returns>data table that has filtered data</returns>
    private DataTable FilterDataTable(DataTable dt, string cond)
    {
        DataTable newdt = new DataTable();
        newdt = dt.Clone();
        DataRow[] dr = dt.Select(cond, "ItemName ASC");

        for (int i = 0; i < dr.Length; i++)
        {
            newdt.ImportRow((DataRow)dr[i]);
        }

        return newdt;
    }

    /// <summary>
    /// Get ACA status
    /// </summary>
    /// <param name="value">status value.</param>
    /// <returns>"true" if status value is "a", otherwise, false.</returns>
    private string GetAcaStatus(string value)
    {
        //if (value.ToLower().Equals("a"))
        if (value.Equals("a", StringComparison.InvariantCultureIgnoreCase))
        {
            return "true";
        }
        else
        {
            return "false";
        }
    }

    /// <summary>
    /// Get sub trees
    /// </summary>
    /// <param name="dtTree">data table.</param>
    /// <param name="moduleName">module name</param>
    /// <param name="cond">condition use to filter data</param>
    private void GetSubTree(DataTable dtTree, string moduleName, string cond)
    {
        DataTable tmpTree2 = null;
        tmpTree2 = FilterDataTable(dtTree, cond);

        string disabled = string.Empty;
        string manageInspections = string.Empty;
        bool displayRequestTradeLicense = false;
        bool isDisplayRequestTradeLicenseFilter = StandardChoiceUtil.IsEnableRequestTradeLicenseFilter();

        if (tmpTree2 != null && tmpTree2.Rows.Count > 0)
        {
            sb.Append(",\n");
            sb.Append("children: [");

            for (int j = 0; j < tmpTree2.Rows.Count; j++)
            {
                if (ACAConstant.DISABLED_STATUS.Equals(tmpTree2.Rows[j]["Disabled"].ToString()))
                {
                    disabled = string.Format("{0}<font color=#1F497D DIR=LTR>({1})</font>", ACAConstant.HTML_NBSP, LabelUtil.GetGlobalTextByKey("ACA_Common_Disabled"));
                }
                else
                {
                    disabled = string.Empty;
                }

                string appStatusGroupCode = ScriptFilter.EncodeJson(tmpTree2.Rows[j]["GroupName"].ToString());
                string appStatus = ScriptFilter.EncodeJson(tmpTree2.Rows[j]["ItemName"].ToString());
                manageInspections = string.Format("{0}<a href='javascript:{1}{4}{2}'><font class='ACA_App_Status_Insepction'>{3}</font></a>", ACAConstant.HTML_NBSP, appStatusGroupCode, appStatus, LabelUtil.GetGlobalTextByKey("admin_application_inspectionpermiassion_manage"), ACAConstant.SPLIT_CHAR); // "Manage Inspections"

                sb.Append("{\n");
                sb.Append("     groupname:'" + ScriptFilter.EncodeJson(tmpTree2.Rows[j]["GroupName"].ToString()) + "',\n");
                sb.Append("     text:\"" + ScriptFilter.EncodeJson(tmpTree2.Rows[j]["resItemName"].ToString()) + disabled + manageInspections + "\",\n");
                sb.Append("     defaultText:'" + ScriptFilter.EncodeJson(tmpTree2.Rows[j]["ItemName"].ToString()) + "',\n");
                sb.Append("     checked:" + GetAcaStatus(tmpTree2.Rows[j]["Checked"].ToString()) + ",\n");
                sb.Append("     hiberarchy:1,\n");

                if (tmpTree2.Rows[j]["DisplayRequestTradeLicense"] != null
                    && ValidationUtil.IsYes(tmpTree2.Rows[j]["DisplayRequestTradeLicense"].ToString()))
                {
                    displayRequestTradeLicense = true;
                }

                sb.Append("     displayRequestTradeLicense: '" + (displayRequestTradeLicense ? "Y" : "N") + "',\n");

                if (isDisplayRequestTradeLicenseFilter)
                {
                    sb.Append("     leaf: false,\n");
                    sb.Append(AddDisplayRequestTradeLicenseNode(displayRequestTradeLicense));
                }
                else
                {
                    sb.Append("     leaf: true \n");
                }

                sb.Append("}\r\n");

                if (j != tmpTree2.Rows.Count - 1)
                {
                    sb.Append(",\n");
                }
            }

            sb.Append("]");
        }
    }

    /// <summary>
    /// Display Request a Trade License Link Node
    /// </summary>
    /// <param name="displayRequestTradeLic">whether display request trade license</param>
    /// <returns>The Request a Trade License Link Node.</returns>
    private string AddDisplayRequestTradeLicenseNode(bool displayRequestTradeLic)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("     children: [{\n");
        sb.Append("          text:'" + LabelUtil.GetGlobalTextByKey("per_tradeName_msg_requestTradeLicense") + "',\n");
        sb.Append("          checked: " + (displayRequestTradeLic ? "true" : "false") + ",\n");
        sb.Append("          leaf: true,\n");
        sb.Append("          iconCls:'x-tree-nodes-setting',\n");
        sb.Append("          hiberarchy:2,\n");
        sb.Append("          displayRequestTradeLicense:'" + (displayRequestTradeLic ? "Y" : "N") + "'\n");
        sb.Append("          }]\n");

        return sb.ToString();
    }

    /// <summary>
    /// This method is to get page json data.
    /// </summary>
    /// <param name="moduleName">module name</param>
    /// <returns>json format string</returns>
    private string GetTree(string moduleName)
    {
        DataTable dtTree = _adminBll.GetAppStatusGroup(ConfigManager.AgencyCode, moduleName);
        DataTable tmpTree1 = FilterDataTable(dtTree, "GroupName = '' ");

        if (tmpTree1 != null && tmpTree1.Rows.Count > 0)
        {
            sb.Append("[");
            for (int i = 0; i < tmpTree1.Rows.Count; i++)
            {
                sb.Append("{");
                sb.Append("     groupname:'" + ScriptFilter.EncodeJson(tmpTree1.Rows[i]["GroupName"].ToString()) + "',\n");
                sb.Append("     text:'" + ScriptFilter.EncodeJson(tmpTree1.Rows[i]["resItemName"].ToString()) + "',\n");
                sb.Append("     defaultText:'" + ScriptFilter.EncodeJson(tmpTree1.Rows[i]["ItemName"].ToString()) + "',\n");
                sb.Append("     checked:" + HasSelectChildren(dtTree, tmpTree1.Rows[i]["ItemName"].ToString()) + ",\n");
                sb.Append("     iconCls:\"x-tree-nodes-setting\",\n");
                sb.Append("     leaf:false \n");

                this.GetSubTree(dtTree, moduleName, "GroupName='" + tmpTree1.Rows[i]["ItemName"].ToString() + "' and GroupName <> ''");

                sb.Append("}\r\n");

                if (i != tmpTree1.Rows.Count - 1)
                {
                    sb.Append(",");
                }
            }

            sb.Append("]");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Check if has children nodes
    /// </summary>
    /// <param name="dt">data table</param>
    /// <param name="notes">group name</param>
    /// <returns>"true" if has children node. Otherwise,"false".</returns>
    private string HasChildren(DataTable dt, string notes)
    {
        DataTable tmpNotes = null;
        tmpNotes = FilterDataTable(dt, "GroupName='" + notes + "'");
        if (tmpNotes.Rows.Count == 0)
        {
            return "true";
        }
        else
        {
            return "false";
        }
    }

    /// <summary>
    /// Check if has selected children nodes
    /// </summary>
    /// <param name="dt">data table</param>
    /// <param name="notes">group name</param>
    /// <returns>"true" if has selected node. Otherwise,"false".</returns>
    private string HasSelectChildren(DataTable dt, string notes)
    {
        DataTable tmpNotes = null;
        tmpNotes = FilterDataTable(dt, "GroupName='" + notes + "' and Checked ='A' ");
        if (tmpNotes.Rows.Count == 0)
        {
            return "false";
        }
        else
        {
            return "true";
        }
    }

    #endregion Methods
}
