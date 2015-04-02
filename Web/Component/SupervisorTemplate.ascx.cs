/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: SupervisorTemplate.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 * 
 *  Description:
 *  
 * 
 *  Notes:
 * $Id: SupervisorTemplate.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Supervisor list in SPEAR form and CAP confirm form
    /// </summary>
    public partial class SupervisorTemplate : BaseUserControl
    {
        /// <summary>
        /// control name 
        /// </summary>
        private const string LIST_CONTROL_NAME = "SupervisorList";

        /// <summary>
        /// Gets a value indicating whether the template have always editable field or not.
        /// </summary>
        public bool HasAlwaysEditableControl
        {
            get
            {
                bool _hasAlwaysEditableControl = false;
                int licenseProListCount = dlAttributesList4EMSE.Items.Count;

                for (int i = 0; i < licenseProListCount; i++)
                {
                    TemplateEdit supervisorEdit = dlAttributesList4EMSE.Items[i].FindControl(LIST_CONTROL_NAME) as TemplateEdit;
                    if (supervisorEdit.HasAlwaysEditableControl == true)
                    {
                        _hasAlwaysEditableControl = true;
                        break;
                    }
                }

                return _hasAlwaysEditableControl;
            }
        }

        /// <summary>
        /// Gets or sets license professional model list which have supervisor fields.
        /// </summary>
        private IList<LicenseProfessionalModel> LicenseProModels
        {
            get
            {
                if (ViewState["LicenseProModels"] != null)
                {
                    return (IList<LicenseProfessionalModel>)ViewState["LicenseProModels"];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                ViewState["LicenseProModels"] = value;
            }
        }

        /// <summary>
        /// Load supervisor for each LP.
        /// </summary>
        /// <param name="licenseProModels">Licensed professional models</param>
        public void DisplaySupervisor4EachLP(IList<LicenseProfessionalModel> licenseProModels)
        {
            LicenseProModels = licenseProModels;
            dlAttributesList4EMSE.DataSource = licenseProModels;
            dlAttributesList4EMSE.DataBind();
        }

        /// <summary>
        /// Get licensed professional.
        /// </summary>
        /// <returns>Licensed professional models</returns>
        public LicenseProfessionalModel[] GetLicensees()
        {
            if (LicenseProModels == null)
            {
                return null;
            }

            LicenseProfessionalModel[] licenseProModels4Save = LicenseProModels.ToArray();
            int licenseProListCount = dlAttributesList4EMSE.Items.Count;

            for (int i = 0; i < licenseProListCount; i++)
            {
                TemplateEdit supervisorEdit = dlAttributesList4EMSE.Items[i].FindControl(LIST_CONTROL_NAME) as TemplateEdit;
                licenseProModels4Save[i].templateAttributes = supervisorEdit.GetAttributeModels();
            }

            return licenseProModels4Save;
        }

        /// <summary>
        /// Clear Controls if need add different template controls
        /// </summary>
        public void ResetControl()
        {
            // Need clear viewstate when clear date.
            LicenseProModels = null;
            dlAttributesList4EMSE.DataSource = null;
            dlAttributesList4EMSE.DataBind();
            Controls.Clear();
        }

        /// <summary>
        /// Page load function
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }
        }

        /// <summary>
        /// EMSE dropdown data bound.
        /// </summary>
        /// <param name="sender">Data list event object.</param>
        /// <param name="e">Data list event arguments.</param>
        protected void AttributesList4EMSE_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LicenseProfessionalModel licProModel = (LicenseProfessionalModel)e.Item.DataItem;
                TemplateEdit supervisorEdit = e.Item.FindControl(LIST_CONTROL_NAME) as TemplateEdit;
                supervisorEdit.IsForSupervisor = true;
                supervisorEdit.DisplayAttributes(licProModel.templateAttributes, ACAConstant.CAP_PROFESSIONAL_TEMPLATE_FIELD_PREFIX);
            }
        }
    }
} 