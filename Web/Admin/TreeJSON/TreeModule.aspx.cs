#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TreeModule.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: TreeModule.aspx.cs 277316 2014-08-14 02:02:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using System.Text;

using Accela.ACA.BLL.Admin;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Security;

/// <summary>
/// Tree module page
/// </summary>
[SuppressCsrfCheck]
public partial class Admin_TreeJSON_TreeModule : System.Web.UI.Page
{
    #region Fields

    /// <summary>
    /// admin caller id
    /// </summary>
    private const string CALLER_ID = "Admin";

    /// <summary>
    /// Create an instance of Admin BLL
    /// </summary>
    private IAdminBll adminBll = (IAdminBll)ObjectFactory.GetObject(typeof(IAdminBll));

    /// <summary>
    /// force login label
    /// </summary>
    private string forceLoginLabel = string.Empty;

    /// <summary>
    /// single service selection only label
    /// </summary>
    private string singleServiceSelectionOnlyLabel = string.Empty;

    /// <summary>
    /// Create a blank;
    /// </summary>
    private string _blank = "     ";

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
            JsonTree = this.GetTree();
        }
    }

    /// <summary>
    /// Add node for force login
    /// </summary>
    /// <param name="forceLogin">force login flag</param>
    /// <param name="singleSelection">single service selection only</param>
    /// <param name="isAddSingleSelectionNode">
    /// Indicating whether need to show the "Single Service Selection Only" node.
    /// </param>
    /// <returns>force login node.</returns>
    private string AddChildrenNodes(string forceLogin, string singleSelection, bool isAddSingleSelectionNode)
    {
        string forceLoginChecked = "false";        

        if (forceLogin.Equals(ACAConstant.COMMON_YES, StringComparison.InvariantCulture))
        {
            forceLoginChecked = "true";
        }       

        StringBuilder sb = new StringBuilder();
        sb.Append("children:");
        sb.Append("  [{");
        sb.Append(_blank + "text:'").Append(forceLoginLabel).Append("',");
        sb.Append(_blank + "hiberarchy:2,\n");
        sb.Append(_blank + "checked:").Append(forceLoginChecked).Append(",");
        sb.Append(_blank + "leaf:true,");
        sb.Append(_blank + "forcelogin:'").Append(forceLogin).Append("'");
        sb.Append("  }");

        if (isAddSingleSelectionNode)
        {
            string singleSelectionChecked = "false";

            if (singleSelection.Equals(ACAConstant.COMMON_YES, StringComparison.InvariantCulture))
            {
                singleSelectionChecked = "true";
            }

            sb.Append(",{");
            sb.Append(_blank + "text:'").Append(singleServiceSelectionOnlyLabel).Append("',");
            sb.Append(_blank + "hiberarchy:2,\n");
            sb.Append(_blank + "checked:").Append(singleSelectionChecked).Append(",");
            sb.Append(_blank + "leaf:true,");
            sb.Append(_blank + "singleserviceonly:'").Append(singleSelection).Append("'");
            sb.Append("  }");
        }       

        sb.Append("]");

        return sb.ToString();
    }

    /// <summary>
    /// Filter data from data table
    /// </summary>
    /// <param name="dt">data table</param>
    /// <param name="cond">condition string</param>
    /// <returns>filtered data table</returns>
    private DataTable FilterDataTable(DataTable dt, string cond)
    {
        DataTable newdt = new DataTable();
        newdt = dt.Clone();
        DataRow[] dr = dt.Select(cond, "ElementName ASC");

        for (int i = 0; i < dr.Length; i++)
        {
            newdt.ImportRow((DataRow)dr[i]);
        }

        return newdt;
    }

    /// <summary>
    /// Filter data from data table
    /// </summary>
    /// <param name="tmpdt">data table</param>
    /// <param name="dt">data table.</param>
    /// <param name="cond">condition use to filter data</param>
    /// <param name="order">string order</param>
    private void FilterRootDataTable(DataTable tmpdt, DataTable dt, string cond, string order)
    {
        DataRow[] dr = dt.Select(cond, order);
        for (int i = 0; i < dr.Length; i++)
        {
            tmpdt.ImportRow((DataRow)dr[i]);
        }
    }

    /// <summary>
    /// Get a GUID id for node if element id is null
    /// </summary>
    /// <param name="elementID">element id</param>
    /// <returns>a new GUID</returns>
    private string GetGuid(string elementID)
    {
        return elementID == string.Empty ? CommonUtil.GetRandomUniqueID() : elementID;
    }

    /// <summary>
    /// Get login status
    /// </summary>
    /// <param name="value">login status flag</param>
    /// <returns>"true" if login status is "yes", otherwise, return "false".</returns>
    private string GetLoginStatus(string value)
    {
        if (value.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
        {
            return "true";
        }
        else
        {
            return "false";
        }
    }

    /// <summary>
    /// Get account management status.
    /// </summary>
    /// <param name="value">flag for status</param>
    /// <returns>false / true</returns>
    private string GetAccountManageStatus(string value)
    {
        return ValidationUtil.IsNo(value) ? "false" : "true";
    }

    /// <summary>
    /// Get rec status
    /// </summary>
    /// <param name="value">string value.</param>
    /// <returns>"true" if value is "a", otherwise, return "false".</returns>
    private string GetRecStatus(string value)
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
    /// Get sub tree string
    /// </summary>
    /// <param name="dtTree">The data table</param>
    /// <param name="cond">The condition use to filter data</param>
    /// <returns>The sub tree</returns>
    private string GetSubTree(DataTable dtTree, string cond)
    {
        DataTable tmpTree2 = null;
        tmpTree2 = FilterDataTable(dtTree, cond);

        if (tmpTree2 == null || tmpTree2.Rows.Count == 0)
        {
            return string.Empty;
        }

        StringBuilder subsb = new StringBuilder();
        subsb.Append(",\n");
        subsb.Append("children: [");

        for (int j = 0; j < tmpTree2.Rows.Count; j++)
        {
            StringBuilder subrowsb = new StringBuilder();

            string labelKey = tmpTree2.Rows[j]["LabelKey"].ToString();

            subrowsb.Append("{\n");
            subrowsb.Append(_blank + "id:'" + GetGuid(tmpTree2.Rows[j]["ElementID"].ToString()) + tmpTree2.Rows[j]["ElementName"].ToString() + tmpTree2.Rows[j]["ParentID"].ToString() + tmpTree2.Rows[j]["PageType"].ToString()
                      + "',\n");
            subrowsb.Append(_blank + "hiberarchy:1,\n");
            subrowsb.Append(_blank + "elementid:'" + tmpTree2.Rows[j]["ElementID"].ToString() + "',\n");
            subrowsb.Append(_blank + "elementname:'" + tmpTree2.Rows[j]["ElementName"].ToString() + "',\n");
            subrowsb.Append(_blank + "parentid:'" + tmpTree2.Rows[j]["ParentID"].ToString() + "',\n");
            subrowsb.Append(_blank + "nodedescribe:'" + tmpTree2.Rows[j]["NodeDescribe"].ToString() + "',\n");
            subrowsb.Append(_blank + "recstatus:'" + tmpTree2.Rows[j]["RecStatus"].ToString() + "',\n");
            subrowsb.Append(_blank + "labelkey:'" + labelKey + "',\n");

            subrowsb.Append(_blank + "pagetype:'" + tmpTree2.Rows[j]["PageType"].ToString() + "',\n");
            subrowsb.Append(_blank + "text:'" + ScriptFilter.FilterJSChar(tmpTree2.Rows[j]["NewElementName"].ToString()) + "',\n");

            if (tmpTree2.Rows[j]["PageType"].ToString() == "REGISTRATION_ENABLED" || tmpTree2.Rows[j]["PageType"].ToString() == "LOGIN_ENABLED")
            {
                subrowsb.Append(_blank + "checked:" + GetLoginStatus(tmpTree2.Rows[j]["NodeDescribe"].ToString()) + ",\n");
            }
            else if (BizDomainConstant.STD_ITEM_ACCOUNT_MANAGEMENT_ENABLED.Equals(tmpTree2.Rows[j]["PageType"].ToString()))
            {
                subrowsb.Append(_blank + "checked:" + GetAccountManageStatus(tmpTree2.Rows[j]["NodeDescribe"].ToString()) + ",\n");
            }
            else
            {
                subrowsb.Append(_blank + "checked:" + GetRecStatus(tmpTree2.Rows[j]["RecStatus"].ToString()) + ",\n");
            }

            //judge whether add the "Force Login" checkbox node as a sub node.
            switch (labelKey.ToLower())
            {
                case "csv_examination_scores_upload":
                case "aca_home_label_inspectionuploadresult":
                    subrowsb.Append(_blank + "leaf:true\n");
                    break;
                default:
                    subrowsb.Append(_blank + "leaf:false,\n");
                    subrowsb.Append(_blank + "forcelogin:'").Append(tmpTree2.Rows[j]["ForceLogin"].ToString()).Append("',");
                    subrowsb.Append(_blank + "singleserviceonly:'").Append(tmpTree2.Rows[j]["SingleServiceOnly"].ToString()).Append("',");

                    string forceLogin = tmpTree2.Rows[j]["ForceLogin"].ToString();
                    string singleSelection = tmpTree2.Rows[j]["SingleServiceOnly"].ToString();
                    bool isAddSingleSelectionNode = false;

                    if (labelKey.ToLower().Equals("aca_sys_feature_apply_a_permit_by_service"))
                    {
                        isAddSingleSelectionNode = true;
                    }

                    string childrenNode = this.AddChildrenNodes(forceLogin, singleSelection, isAddSingleSelectionNode);
                    subrowsb.Append(childrenNode);
                    break;
            }

            subrowsb.Append("}\r\n");

            if (!Convert.ToBoolean(HasChildren(dtTree, tmpTree2.Rows[j]["ElementName"].ToString())))
            {
                string str = this.GetSubTree(dtTree, "ParentID='" + tmpTree2.Rows[j]["ElementName"].ToString() + "'");
                subrowsb.Append(str);
            }

            if (j != tmpTree2.Rows.Count - 1)
            {
                subrowsb.Append(",\n");
            }

            subsb.Append(subrowsb.ToString());
        }

        subsb.Append("]");

        return subsb.ToString();
    }

    /// <summary>
    /// This method is to get page json data.
    /// </summary>
    /// <returns>json format string</returns>
    private string GetTree()
    {
        forceLoginLabel = LabelUtil.GetAdminUITextByKey("admin_feature_setting_label_forcelogin_title");
        singleServiceSelectionOnlyLabel = LabelUtil.GetAdminUITextByKey("aca_modulesetup_label_singleserviceonly");
        DataTable tmpTree1;

        DataTable dtTree = adminBll.GetSubTreeNode(ConfigManager.AgencyCode, CALLER_ID);
        IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
        string isSuperAgency = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_MULTI_SERVICE_SETTINGS, BizDomainConstant.STD_ITEM_IS_SUPER_AGENCY);

        //apply by select services should not been displayed in normal agency.
        if (!ValidationUtil.IsYes(isSuperAgency))
        {
            DataRow[] rows = dtTree.Select("LabelKey='aca_sys_feature_apply_a_permit_by_service'", string.Empty);

            if (rows != null && rows.Length > 0)
            {
                foreach (var row in rows)
                {
                    dtTree.Rows.Remove(row);
                }
            }
        }

        tmpTree1 = dtTree.Clone();
        ModifyDataTable(dtTree);
        FilterRootDataTable(tmpTree1, dtTree, "PageType = 'REGISTRATION_ENABLED'", string.Empty);

        string accountMgr = string.Concat("PageType = ", "'", BizDomainConstant.STD_ITEM_ACCOUNT_MANAGEMENT_ENABLED, "'");
        FilterRootDataTable(tmpTree1, dtTree, accountMgr, string.Empty);
        FilterRootDataTable(tmpTree1, dtTree, "PageType = 'LOGIN_ENABLED'", string.Empty);
        FilterRootDataTable(tmpTree1, dtTree, "PageType = 'GENERAL_INFO_NODE'", string.Empty);
        FilterRootDataTable(tmpTree1, dtTree, "PageType = 'rootNode'", string.Empty);
        FilterRootDataTable(tmpTree1, dtTree, "PageType = 'readOnly'", "ElementName ASC");

        if (tmpTree1 == null || tmpTree1.Rows.Count == 0)
        {
            return string.Empty;
        }

        StringBuilder sb = new StringBuilder();
        sb.Append("[");

        for (int i = 0; i < tmpTree1.Rows.Count; i++)
        {
            // Create a StringBuilder for each row
            StringBuilder rowsb = new StringBuilder();

            string labelKey = tmpTree1.Rows[i]["LabelKey"].ToString();

            rowsb.Append("{");
            rowsb.Append(_blank + "id:'" + tmpTree1.Rows[i]["ElementID"].ToString() + tmpTree1.Rows[i]["ElementName"].ToString() + tmpTree1.Rows[i]["ParentID"].ToString() + tmpTree1.Rows[i]["PageType"].ToString() + "',\n");
            rowsb.Append(_blank + "hiberarchy:0,\n");
            rowsb.Append(_blank + "elementid:'" + tmpTree1.Rows[i]["ElementID"].ToString() + "',\n");
            rowsb.Append(_blank + "elementname:'" + tmpTree1.Rows[i]["ElementName"].ToString() + "',\n");
            rowsb.Append(_blank + "parentid:'" + tmpTree1.Rows[i]["ParentID"].ToString() + "',\n");
            rowsb.Append(_blank + "nodedescribe:'" + tmpTree1.Rows[i]["NodeDescribe"].ToString() + "',\n");
            rowsb.Append(_blank + "recstatus:'" + tmpTree1.Rows[i]["RecStatus"].ToString() + "',\n");
            rowsb.Append(_blank + "labelkey:'" + labelKey + "',\n");
            rowsb.Append(_blank + "pagetype:'" + tmpTree1.Rows[i]["PageType"].ToString() + "',\n");
            rowsb.Append(_blank + "text:'" + ScriptFilter.FilterJSChar(tmpTree1.Rows[i]["NewElementName"].ToString()) + "',\n");

            if (tmpTree1.Rows[i]["PageType"].ToString() == "REGISTRATION_ENABLED" || tmpTree1.Rows[i]["PageType"].ToString() == "LOGIN_ENABLED")
            {
                rowsb.Append(_blank + "checked:" + GetLoginStatus(tmpTree1.Rows[i]["NodeDescribe"].ToString()) + ",\n");
            }
            else if (BizDomainConstant.STD_ITEM_ACCOUNT_MANAGEMENT_ENABLED.Equals(tmpTree1.Rows[i]["PageType"].ToString()))
            {
                rowsb.Append(_blank + "checked:" + GetAccountManageStatus(tmpTree1.Rows[i]["NodeDescribe"].ToString()) + ",\n");
            }
            else if (tmpTree1.Rows[i]["PageType"].ToString() == "readOnly")
            {
                rowsb.Append(_blank + "checked:" + HasSelectChildren(dtTree, tmpTree1.Rows[i]["ElementName"].ToString()) + ",\n");
            }
            else
            {
                rowsb.Append(_blank + "checked:" + GetRecStatus(tmpTree1.Rows[i]["RecStatus"].ToString()) + ",\n");
            }

            rowsb.Append(_blank + "leaf:" + HasChildren(dtTree, tmpTree1.Rows[i]["ElementName"].ToString()) + "\n");

            if (!Convert.ToBoolean(HasChildren(dtTree, tmpTree1.Rows[i]["ElementName"].ToString())))
            {
                string str2 = this.GetSubTree(dtTree, "ParentID='" + tmpTree1.Rows[i]["ElementName"].ToString() + "'");
                rowsb.Append(str2);
            }

            rowsb.Append("}\r\n");

            if (i != tmpTree1.Rows.Count - 1)
            {
                rowsb.Append(",");
            }

            sb.Append(rowsb.ToString());
        }

        sb.Append("]");
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

    /// <summary>
    /// Check if has selected children nodes
    /// </summary>
    /// <param name="dt">data table</param>
    /// <param name="notes">group name</param>
    /// <returns>"true" if has selected node. Otherwise,"false".</returns>
    private string HasSelectChildren(DataTable dt, string notes)
    {
        DataTable tmpNotes = null;
        tmpNotes = FilterDataTable(dt, "ParentID='" + notes + "' and RecStatus ='A' ");
        if (tmpNotes.Rows.Count == 0)
        {
            return "false";
        }
        else
        {
            return "true";
        }
    }

    /// <summary>
    /// modify data from data table
    /// </summary>
    /// <param name="dt">data table</param>
    private void ModifyDataTable(DataTable dt)
    {
        string pageType = string.Empty;
        string elementName = string.Empty;
        string labelName = string.Empty;
        string patentId = string.Empty;
        string defaultModuleName = LabelUtil.GetGlobalTextByKey("aca_sys_default_module_name");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            pageType = dt.Rows[i]["PageType"].ToString();
            elementName = dt.Rows[i]["ElementName"].ToString();
            patentId = dt.Rows[i]["ParentID"].ToString();

            if (pageType == "readOnly")
            {
                labelName = LabelUtil.GetTextByKey(dt.Rows[i]["LabelKey"].ToString(), elementName);

                if (labelName != defaultModuleName)
                {
                    dt.Rows[i]["NewElementName"] = LabelUtil.RemoveHtmlFormat(labelName);
                }
                else
                {
                    dt.Rows[i]["NewElementName"] = DataUtil.AddBlankToString(elementName);
                }
            }
            else if (BizDomainConstant.STD_ITEM_ACCOUNT_MANAGEMENT_ENABLED.Equals(pageType) || pageType == "REGISTRATION_ENABLED" || pageType == "LOGIN_ENABLED")
            {
                labelName = LabelUtil.GetTextByKey(dt.Rows[i]["ElementName"].ToString(), patentId);
                dt.Rows[i]["NewElementName"] = labelName;
            }
            else
            {
                labelName = LabelUtil.GetTextByKey(dt.Rows[i]["LabelKey"].ToString(), patentId);
                dt.Rows[i]["NewElementName"] = labelName;
            }
        }
    }

    #endregion Methods
}