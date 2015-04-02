#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: GenericTemplateEdit.ascx.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description: An edit form for generic template fields.
*
*  Notes:
* $Id: GenericTemplateEdit.ascx.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Sep 20, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// An edit from for generic template fields.
    /// </summary>
    public partial class GenericTemplateEdit : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// indicates the dynamical control whether has been created.
        /// </summary>
        private bool _isCreated = false;

        /// <summary>
        /// Local instance for generic template table list.
        /// </summary>
        private AppSpecInfoTableList _templateList;

        /// <summary>
        /// A variable indicating whether need to validate all required fields for the generic template table.
        /// Default value is true.
        /// </summary>
        private bool _needValidate = true;

        /// <summary>
        /// the asit UI data key
        /// </summary>
        private string _asitUIDataKey = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether hide template table.
        /// </summary>
        public bool IsHideTemplateTable
        {
            get
            {
                return ViewState["IsHideTemplateTable"] != null && bool.Parse(ViewState["IsHideTemplateTable"].ToString());
            }

            set
            {
                ViewState["IsHideTemplateTable"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is hide generic template.
        /// </summary>
        /// <value><c>true</c> if this instance is hide generic template; otherwise, <c>false</c>.</value>
        public bool IsHideGenericTemplate
        {
            get
            {
                return ViewState["IsHideGenericTemplate"] != null && bool.Parse(ViewState["IsHideGenericTemplate"].ToString());
            }

            set
            {
                ViewState["IsHideGenericTemplate"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the template table is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                if (ViewState["IsReadOnly"] == null)
                {
                    return false;
                }

                return (bool)ViewState["IsReadOnly"];
            }

            set
            {
                if (_templateList != null)
                {
                    _templateList.IsReadOnly = value;
                }

                ViewState["IsReadOnly"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether need to validate all required fields for the generic template table.
        /// </summary>
        public bool NeedValidate
        {
            get
            {
                return _needValidate;
            }

            set
            {
                _needValidate = value;
            }
        }

        /// <summary>
        /// Gets or sets the generic template group code.
        /// </summary>
        public string GenericTemplateGroupCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the UI table group name:sub group name
        /// </summary>
        public List<string> ASITUITables
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the asit edit pop page section info(Format:title/index )
        /// </summary>
        public string SectionInfo
        {
            private get;
            set;
        }

        /// <summary>
        /// Gets or sets the asit UI data key
        /// </summary>
        public string ASITUIDataKey
        {
            private get
            {
                if (string.IsNullOrEmpty(_asitUIDataKey))
                {
                    _asitUIDataKey = ClientID;
                }

                return _asitUIDataKey;
            }

            set
            {
                _asitUIDataKey = value;
            }
        }

        /// <summary>
        /// Gets UI tables.
        /// </summary>
        public ASITUITable[] UITables
        {
            get
            {
                return _templateList != null ? _templateList.UITables : null;
            }
        }

        /// <summary>
        /// Gets web control id list which are readonly by ASI security.
        /// </summary>
        public List<string> ReadOnlyControlIds
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets Model.
        /// </summary>
        private TemplateModel Model
        {
            get
            {
                if (ViewState["Model"] != null)
                {
                    return (TemplateModel)ViewState["Model"];
                }

                return null;
            }

            set
            {
                ViewState["Model"] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clear Controls if need add different template controls
        /// </summary>
        public void ResetControl()
        {
            //need clear viewstate when clear date.
            Model = null;
            _isCreated = false;

            this.Controls.Clear();
        }

        /// <summary>
        /// Create UI control for generic template fields and render to UI.
        /// </summary>
        /// <param name="template">Generic template model.</param>
        public void Display(TemplateModel template)
        {
            Model = template;

            if (template == null)
            {
                return;
            }

            SetExpressionGroup(template);
            CreateControls(false);
        }

        /// <summary>
        /// Gets TemplateModel from UI.
        /// </summary>
        /// <param name="isNeedSetValue">is need set default value or not(when need the generic template fields but not need value,it is set false)</param>
        /// <param name="isForSearchForm">For search form, CheckBox control will got the empty value if the CheckBox is unchecked.</param>
        /// <returns>return TemplateModel</returns>
        public TemplateModel GetTemplateModel(bool isNeedSetValue, bool isForSearchForm = false)
        {
            if (Model == null)
            {
                return null;
            }

            var templateFields = GenericTemplateUtil.GetAllFields(Model);

            if (templateFields != null && templateFields.Count() > 0)
            {
                Display(Model);

                if (isNeedSetValue)
                {
                    foreach (var field in templateFields)
                    {
                        string controlID = ControlBuildHelper.GetGenericTemplateControlID(field);
                        var control = this.FindControl(controlID) as WebControl;

                        if (control != null)
                        {
                            field.defaultValue = ControlBuildHelper.GetControlValue(Request, field.fieldType, control.UniqueID, field.defaultValue, isForSearchForm);
                        }
                    }
                }
            }

            TemplateModel templateModel = ObjectCloneUtil.DeepCopy(Model);
            return templateModel;
        }

        /// <summary>
        /// Load view state.
        /// </summary>
        /// <param name="savedState">the saved state.</param>
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            if (Model != null)
            {
                var templateFields = GenericTemplateUtil.GetAllFields(Model);

                if (templateFields != null || Model.templateTables != null)
                {
                    CreateControls(true);
                }
            }
        }

        /// <summary>
        /// create controls
        /// </summary>
        /// <param name="isLoadViewState">indicating whether is load view state event</param>
        private void CreateControls(bool isLoadViewState)
        {
            if (_isCreated)
            {
                return;
            }
            else
            {
                _isCreated = true;
            }

            this.Controls.Clear();

            if (Model != null)
            {
                var templateFields = GenericTemplateUtil.GetAllFields(Model);

                if (templateFields != null)
                {
                    List<string> controlIds = new List<string>();

                    foreach (var field in templateFields)
                    {
                        WebControl webControl = ControlBuildHelper.GenericTemplate.CreateWebControl(this, field, IsHideGenericTemplate);

                        bool isReadOnlyBySecurity = false;

                        if (webControl != null)
                        {
                            Controls.Add(webControl);
                        }

                        //control is set read only by asi security not be enabled by section enable function at contactEdit line 2010
                        if (!AppSession.IsAdmin && webControl != null)
                        {
                            string asiSecurity = ASISecurityUtil.GetASISecurity(field.serviceProviderCode, field.groupName, field.subgroupName, field.fieldName, ModuleName);
                            isReadOnlyBySecurity = ACAConstant.ASISecurity.Read.Equals(asiSecurity, System.StringComparison.InvariantCultureIgnoreCase);
                        }

                        if (isReadOnlyBySecurity)
                        {
                            controlIds.Add(webControl.ID);
                        }
                    }

                    ReadOnlyControlIds = controlIds;
                }

                // if in admin page and not hide template table,use image way to display template table,
                // if in admin daly and not hide template table,use template table control.
                if (!AppSession.IsAdmin && !IsHideTemplateTable && Model.templateTables != null)
                {
                    string attributeName = string.Format("{1}{0}{2}", ACAConstant.SPLIT_DOUBLE_COLON, ConfigManager.AgencyCode, Model.templateTables.ElementAt(0).groupName);

                    Control control = LoadControl("~/Component/AppSpecInfoTableList.ascx");
                    control.ID = TemplateUtil.GetTemplateControlID(attributeName);
                    _templateList = (AppSpecInfoTableList)control;

                    if (!isLoadViewState && ASITUITables != null && ASITUITables.Count > 0)
                    {
                        _templateList.SectionInfo = SectionInfo;
                    }

                    _templateList.IsTemplateTable = true;
                    _templateList.ASITUIDataKey = ASITUIDataKey;
                    _templateList.IsReadOnly = IsReadOnly;
                    _templateList.NeedValidate = this.NeedValidate;
                    this.Controls.Add(control);
                    _templateList.Display(Model, isLoadViewState, null);
                }
                else if (AppSession.IsAdmin && !IsHideTemplateTable && Model.templateTables != null)
                {
                    string attributeName = string.Format("{1}{0}{2}", ACAConstant.SPLIT_DOUBLE_COLON, ConfigManager.AgencyCode, Model.templateTables.ElementAt(0).groupName);

                    Image control = new Image();
                    control.ID = TemplateUtil.GetTemplateControlID(attributeName);
                    control.ImageUrl = ResolveUrl("~/Admin/images/appspecinfotable.gif");
                    this.Controls.Add(control);
                }
            }
        }

        /// <summary>
        /// Set expression group for expression runtime argumentPKModel
        /// </summary>
        /// <param name="template">the template model</param>
        private void SetExpressionGroup(TemplateModel template)
        {
            if (ASITUITables != null)
            {
                ASITUITables.Clear();
            }

            if (template.templateForms != null && template.templateForms.Length > 0)
            {
                GenericTemplateGroupCode = template.templateForms[0].groupName;
            }

            if (template.templateTables != null)
            {
                ASITUITables = new List<string>();

                foreach (TemplateGroup iGroup in template.templateTables)
                {
                    if (iGroup.subgroups != null)
                    {
                        foreach (TemplateSubgroup subgroup in iGroup.subgroups)
                        {
                            ASITUITables.Add(iGroup.groupName + ACAConstant.SPLIT_DOUBLE_COLON + subgroup.subgroupName);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
