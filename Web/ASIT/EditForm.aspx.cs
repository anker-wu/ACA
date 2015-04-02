#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: EditForm.aspx.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description: An edit form for ASI table.
*
*  Notes:
* $Id: EditForm.aspx.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Aug 9, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Component;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Newtonsoft.Json;

namespace Accela.ACA.Web.ASIT
{
    /// <summary>
    /// An edit form for ASI table.
    /// </summary>
    public partial class EditForm : PopupDialogBasePage
    {
        #region Fields

        /// <summary>
        /// the table info.
        /// </summary>
        private string _tableInfo = string.Empty;

        /// <summary>
        /// is for expression.
        /// </summary>
        private bool _isExp = false;

        /// <summary>
        /// is for add
        /// </summary>
        private bool _isAdd = false;

        /// <summary>
        /// the new row QTY.
        /// </summary>
        private int _newRowsQty = 1;

        /// <summary>
        /// indicating the edit form contain the asit section UI.
        /// </summary>
        private bool _isIncludeAsitSectionUI = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the array of the UI data keys.
        /// </summary>
        private string[] UiDataKeys
        {
            get
            {
                if (ViewState["UiDataKeys"] == null)
                {
                    var uiDataKeyAndIndexs = new Dictionary<string, int>();
                    var paramUiDataKeys = JsonConvert.DeserializeObject(Request.QueryString["uikey"], typeof(string[])) as string[];

                    foreach (var paramUiDataKey in paramUiDataKeys)
                    {
                        int uiDataKeyIndex = 0;
                        var curAsitUiTables = UIModelUtil.GetDataFromUIContainer(UIDataType.ASIT, new string[] { paramUiDataKey }) as ASITUITable[];

                        if (curAsitUiTables == null || !curAsitUiTables.Any())
                        {
                            continue;
                        }

                        uiDataKeyIndex = curAsitUiTables[0].SectionIndex;
                        _isIncludeAsitSectionUI = _isIncludeAsitSectionUI || !curAsitUiTables[0].IsTemplateTable;
                        uiDataKeyAndIndexs.Add(paramUiDataKey, uiDataKeyIndex);
                    }

                    ViewState["UiDataKeys"] = uiDataKeyAndIndexs.OrderBy(o => o.Value).Select(s => s.Key).ToArray();
                }

                return ViewState["UiDataKeys"] as string[];
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handle the page load event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetDialogMaxHeight("600");
            DialogUtil.RegisterScriptForDialog(Page);
            SetPageTitleKey("aca_editform_label_title|tip");
            SetPageTitleVisible(false);
            
            if (!IsPostBack)
            {
                //Create Copy of UI Data.
                Dictionary<string, UITable[]> allAsitUIDataList = UIModelUtil.GetDataFromUIContainer(UIDataType.ASIT);
                Dictionary<string, UITable[]> curAsitUIDataList = allAsitUIDataList.Where(v => UiDataKeys.Contains(v.Key)).ToDictionary(v => v.Key, v => v.Value);
                Dictionary<string, UITable[]> asitCopyUIData = ObjectCloneUtil.DeepCopy(curAsitUIDataList);
                UIModelUtil.SetDataToUIContainer(asitCopyUIData, UIDataType.ASITCopy);

                string action = Request.QueryString["action"];

                switch (action.ToLower())
                {
                    case "add":
                        //Add a row by user click.
                        _isAdd = true;
                        _tableInfo = Request.QueryString["param"];
                        int.TryParse(Request.QueryString["qty"], out _newRowsQty);
                        _isExp = ValidationUtil.IsYes(Request.QueryString["isExp"]);
                        break;
                    case "edit":
                        break;
                }
            }

            /*
             * Need bind data form every request
             *  since Insert Row Expression will trigger page post back.
             */
            this.rptAppInfoTableList.DataSource = UiDataKeys;
            this.rptAppInfoTableList.DataBind();
        }

        /// <summary>
        /// Handle ItemDataBound event for table list.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void AppInfoTableList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string curUIDataKey = (string)e.Item.DataItem;
                AppSpecInfoTableEdit asitEdit = e.Item.FindControl("asitEdit") as AppSpecInfoTableEdit;

                if (_isAdd && !_isExp)
                {
                    AddNewRow(_tableInfo, _newRowsQty, curUIDataKey);
                }

                if (!string.IsNullOrEmpty(curUIDataKey) && asitEdit != null)
                {
                    bool isShowSectionTitle = UiDataKeys.Length > 1;
                    asitEdit.Display(curUIDataKey, isShowSectionTitle);
                }
            }
        }

        /// <summary>
        /// Handle the Pre-render event.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!AppSession.IsAdmin)
            {
                //Register the Expression script resources.
                if (!ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
                {
                    ExpressionUtil.RegisterScriptLibToCurrentPage(this);
                }
            }
        }

        /// <summary>
        /// Handle the PreRenderComplete event.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);

            if (!AppSession.IsAdmin)
            {
                bool needRunExpression = false;
                bool isFromDrillDown = ValidationUtil.IsYes(Request.QueryString["isFromDrillDown"]);
                var expressionFieldsList = new Dictionary<string, List<string>>();
                var argumentsModels = new Dictionary<string, ExpressionRuntimeArgumentsModel>();
                
                foreach (RepeaterItem repeaterItem in this.rptAppInfoTableList.Items)
                {
                    AppSpecInfoTableEdit asitEdit = repeaterItem.FindControl("asitEdit") as AppSpecInfoTableEdit;

                    if (asitEdit == null || asitEdit.ExpressionSessionModel == null)
                    {
                        continue;
                    }

                    needRunExpression = true;

                    //Attach onchange event to ASIT/generic template table controls.
                    foreach (KeyValuePair<string, WebControl> isc in asitEdit.AllControls)
                    {
                        if (isc.Value != null)
                        {
                            WebControl ctl = isc.Value;
                            asitEdit.ExpressionInstance.AttachEventToControl(isc.Key, ctl, Page);
                        }
                    }

                    if (!IsPostBack && isFromDrillDown && !Page.ClientScript.IsClientScriptBlockRegistered("onPopulateExpression")
                        && asitEdit.ExpressionSessionModel.ExpressionFactoryType == ExpressionType.ASI_Table)
                    {
                        //Trigger onpopulate expression for those drill-down tables ASIT tables.
                        string callJsFunction = asitEdit.GetOnPopulateRunningExpressionJS();
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "onPopulateExpression", callJsFunction, true);
                    }

                    // ASIT Row Submit
                    if (asitEdit.ExpressionSessionModel.ExpressionFactoryType == ExpressionType.ASI_Table)
                    {
                        string asitScripts = asitEdit.ExpressionInstance.GetClientExpScript4ASIT(ExpressionFactory.ASITROW_SUBMIT_EVENT_NAME);
                        ExpressionUtil.CombineArgumentsAndExpressionFieldsModule(argumentsModels, expressionFieldsList, asitScripts);
                    }
                }

                string asitRowSubmitScript = ExpressionUtil.BuildRunExpressionScripts(argumentsModels, expressionFieldsList, true, null);
                string strSubmitFuction = ExpressionUtil.GetExpressionScriptOnSubmit(asitRowSubmitScript);

                if (!Page.ClientScript.IsStartupScriptRegistered("OnASITRowSubmitExpression"))
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "OnASITRowSubmitExpression", strSubmitFuction, true);
                }

                if (needRunExpression)
                {
                    //Register Validator lib for expression.
                    ScriptManager.RegisterClientScriptResource(Page, typeof(AjaxControlToolkit.ExtenderControlBase), "AjaxControlToolkit.ExtenderBase.BaseScripts.js");
                    ScriptManager.RegisterClientScriptResource(Page, typeof(AjaxControlToolkit.CommonToolkitScripts), "AjaxControlToolkit.Common.Common.js");
                    ScriptManager.RegisterClientScriptResource(Page, typeof(AjaxControlToolkit.PopupExtender), "AjaxControlToolkit.PopupExtender.PopupBehavior.js");
                    ScriptManager.RegisterClientScriptResource(Page, typeof(AjaxControlToolkit.ValidatorCallbackExtender), "Accela.Web.Controls.ValidatorCallback.ValidatorCallbackBehavior.js");
                }
            }
        }

        /// <summary>
        /// Submit the edit form.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void SubmitEditForm(object sender, EventArgs e)
        {
            foreach (RepeaterItem repeaterItem in this.rptAppInfoTableList.Items)
            {
                AppSpecInfoTableEdit asitEdit = repeaterItem.FindControl("asitEdit") as AppSpecInfoTableEdit;

                if (asitEdit == null)
                {
                    continue;
                }

                string uiDataKey = asitEdit.UIDataKey;
                ASITUITable[] asitEditUIData = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITEdit, new string[] { uiDataKey }) as ASITUITable[];
                ASITUITable[] asitCopyUIData = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITCopy, new string[] { uiDataKey }) as ASITUITable[];

                if (asitEditUIData == null || asitCopyUIData == null)
                {
                    continue;
                }

                asitEdit.RefreshValues(asitEditUIData);
                ASITUIModelUtil.SyncASITUIRowData(asitEditUIData, asitCopyUIData);
            }

            ASITUITable[] asitEditUIDatas = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITEdit, null) as ASITUITable[];

            StringBuilder submitScripts = new StringBuilder("ReloadListAndCloseDialog([");
            var tableKeys = from t in asitEditUIDatas
                            select "'" + t.TableKey + "'";
            submitScripts.AppendFormat(string.Join(",", tableKeys));
            submitScripts.Append("]);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SubmitEditScripts", submitScripts.ToString(), true);

            UIModelUtil.SetDataToUIContainer(null, UIDataType.ASITEdit);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add new to ASIT UI table.
        /// </summary>
        /// <param name="tableInfo">String of table info.</param>
        /// <param name="rowQty">Rows quantity need to be added.</param>
        /// <param name="uiDataKey">The UI data key.</param>
        private void AddNewRow(string tableInfo, int rowQty, string uiDataKey)
        {
            string[] tableKeys = JsonConvert.DeserializeObject(tableInfo, typeof(string[])) as string[];
            ASITUITable[] sourTables = UIModelUtil.GetDataFromUIContainer(UIDataType.ASITCopy, new string[] { uiDataKey }) as ASITUITable[];

            if (tableKeys == null || sourTables == null)
            {
                return;
            }

            IList<ASITUITable> newTables = new List<ASITUITable>();

            foreach (string tableKey in tableKeys)
            {
                ASITUITable sTable = sourTables.Where(t => t.TableKey == tableKey).SingleOrDefault();
                ASITUITable dTable = ASITUIModelUtil.CreateNewRow4ASITUITable(sTable, rowQty, uiDataKey);

                if (dTable != null)
                {
                    newTables.Add(dTable);
                }
            }

            if (newTables.Count > 0)
            {
                UIModelUtil.SetDataToUIContainer(newTables.ToArray(), UIDataType.ASITEdit, uiDataKey);
            }
        }

        #endregion
    }
}