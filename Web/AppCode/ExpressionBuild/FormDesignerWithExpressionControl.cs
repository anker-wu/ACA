#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FormDesignerWithExpressionControl.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: FormDesignerWithExpressionControl.cs 2010-04-14 09:28:50Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Component;
using Accela.ACA.Web.FormDesigner;
using Accela.ACA.WSProxy;
using Newtonsoft.Json;

namespace Accela.ACA.Web.ExpressionBuild
{
    /// <summary>
    /// the expression base control.
    /// </summary>
    public class FormDesignerWithExpressionControl : FormDesignerBaseControl
    {
        #region Fields

        /// <summary>
        /// All expression controls collection.
        /// </summary>
        private Dictionary<string, WebControl> _expressionControls;

        /// <summary>
        /// indicating whether need run expression
        /// </summary>
        private bool _needRunExpression = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FormDesignerWithExpressionControl" /> class
        /// </summary>
        /// <param name="viewId">view id</param>
        public FormDesignerWithExpressionControl(string viewId)
            : base(viewId)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets all expression controls.
        /// </summary>
        public Dictionary<string, WebControl> ExpressionControls
        {
            get
            {
                if (_expressionControls == null)
                {
                    _expressionControls = new Dictionary<string, WebControl>();
                }

                return _expressionControls;
            }

            set
            {
                if (_expressionControls == null)
                {
                    _expressionControls = new Dictionary<string, WebControl>();
                }

                if (value != null)
                {
                    foreach (KeyValuePair<string, WebControl> ctlItem in value)
                    {
                        if (!_expressionControls.ContainsKey(ctlItem.Key))
                        {
                            _expressionControls.Add(ctlItem.Key, ctlItem.Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether needs to run onLoad expression.
        /// </summary>
        public bool NeedRunOnloadExpressionInControl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether need run expression.
        /// </summary>
        public bool NeedRunExpression
        {
            get
            {
                return _needRunExpression;
            }

            set
            {
                _needRunExpression = value;
            }
        }

        /// <summary>
        /// Gets or sets Expression Factory Instance.
        /// </summary>
        protected virtual ExpressionFactory ExpressionInstance
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get run expression function on "onLoad" event is triggered;
        /// </summary>
        /// <returns>The expression</returns>
        public string GetRunExpFunctionOnLoad()
        {
            if (ExpressionInstance == null || !NeedRunExpression || AppSession.IsAdmin)
            {
                return string.Empty;
            }

            return ExpressionInstance.GetRunExpFunctionOnLoad();
        }

        /// <summary>
        /// get run expression function for submit action
        /// </summary>
        /// <returns>complete run expression for JS function name</returns>
        public string GetRunExpFunctionOnSubmit()
        {
            if (ExpressionInstance == null || !NeedRunExpression || AppSession.IsAdmin)
            {
                return string.Empty;
            }

            return ExpressionInstance.GetRunExpFunctionOnSubmit();
        }

        /// <summary>
        /// Clear the expression value of the hidden control store.
        /// </summary>
        /// <param name="needRunExpressionOnLoad">need reset run expression onLoad</param>
        public void ClearExpressionValue(bool needRunExpressionOnLoad)
        {
            NeedRunOnloadExpressionInControl = needRunExpressionOnLoad;

            if (Page.Master == null)
            {
                return;
            }

            UpdatePanel updatePanel4Expression = Page.Master.FindControl("UpdatePanel4Expression") as UpdatePanel;

            if (updatePanel4Expression == null)
            {
                return;
            }

            HtmlInputHidden hdExpression = updatePanel4Expression.FindControl("HDExpressionParam") as HtmlInputHidden;

            if (hdExpression != null && !string.IsNullOrEmpty(hdExpression.Value))
            {
                List<ExpressionResultJsModel> array = JsonConvert.DeserializeObject<List<ExpressionResultJsModel>>(hdExpression.Value);
                array.RemoveAll(w => w.name.IndexOf(this.ClientID, StringComparison.InvariantCultureIgnoreCase) > -1);
                hdExpression.Value = JsonConvert.SerializeObject(array);
                updatePanel4Expression.Update();
            }
        }

        /// <summary>
        /// Is attach expression to control or not.
        /// </summary>
        /// <returns>true:need attach;false:not attach</returns>
        protected virtual bool IsAttachExpressionToControl()
        {
            if (!AppSession.IsAdmin)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// override Initializes event.
        /// </summary>
        /// <param name="e">The event argument.</param>
        protected override void OnInit(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ExpressionUtil.ClearExpressionDataByKey(ClientID);
            }

            base.OnInit(e);
        }

        /// <summary>
        /// override PreRender event.
        /// </summary>
        /// <param name="e">The event argument.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //Don't run expression in administration model.
            if (NeedRunExpression && !AppSession.IsAdmin)
            {
                if (NeedRunOnloadExpressionInControl)
                {
                    string scripts = GetRunExpFunctionOnLoad();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), ClientID + "OnLoadExpression", scripts, true);
                }

                if (IsAttachExpressionToControl())
                {
                    ExpressionInstance.AttachEventToControl();
                }
            }
        }

        /// <summary>
        /// Collect input controls for expression.
        /// </summary>
        /// <param name="viewID">view id for expression.</param>
        /// <param name="moduleName">module name.</param>
        /// <param name="templateEdit">template control</param>
        /// <param name="templatePrefix">template prefix.</param>
        /// <returns>Expression Controls</returns>
        protected virtual Dictionary<string, WebControl> CollectExpressionInputControls(string viewID, string moduleName, TemplateEdit templateEdit, string templatePrefix)
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
            Dictionary<string, WebControl> expressionControls = new Dictionary<string, WebControl>();

            // Standard Field
            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();
            SimpleViewElementModel4WS[] models = gviewBll.GetSimpleViewElementModel(moduleName, viewID);

            foreach (SimpleViewElementModel4WS model in models)
            {
                if (string.IsNullOrEmpty(model.viewElementName))
                {
                    continue;
                }

                Control ctl = FindControl(model.viewElementName);

                if (ctl == null)
                {
                    continue;
                }

                WebControl webCtl;

                if (ctl is UserControl)
                {
                    /* For contact access permission control.
                         * It's a UserControl, so need to find WebControl again in UserControl's control-collection.
                         */
                    webCtl = ctl.FindControl(model.viewElementName) as WebControl;
                }
                else
                {
                    webCtl = ctl as WebControl;
                }

                if (webCtl == null)
                {
                    continue;
                }

                string ctlID = ExpressionUtil.GetFullControlFieldName(capModel, webCtl.ID);

                if (!expressionControls.ContainsKey(ctlID))
                {
                    expressionControls.Add(ctlID, webCtl);
                }
            }

            // Template Field
            if (templateEdit != null && templateEdit.Fields != null)
            {
                foreach (TemplateAttributeModel field in templateEdit.Fields)
                {
                    // contiue when the control type is invalid
                    if (!ControlBuildHelper.IsValidControlType(field.attributeValueDataType) || templateEdit.IsUnavailableField(field))
                    {
                        continue;
                    }

                    // Finds web control by control id
                    WebControl control = templateEdit.FindControl(TemplateUtil.GetTemplateControlID(field.attributeName, templatePrefix)) as WebControl;

                    if (control == null)
                    {
                        continue;
                    }

                    string filedName = templatePrefix + ExpressionUtil.DealSpecialChar(HttpUtility.UrlEncode(field.attributeName));
                    //The generate rule of key is must same with the 'RefExpressionUtil.getRefAPOTemplateFieldName' function in AA.
                    string expCtlKey4Template = ExpressionUtil.GetFullControlFieldName(capModel, filedName);

                    if (!expressionControls.ContainsKey(expCtlKey4Template))
                    {
                        expressionControls.Add(expCtlKey4Template, control);
                    }
                }
            }

            return expressionControls;
        }

        #endregion Methods
    }
}