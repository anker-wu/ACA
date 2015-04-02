#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: GenericTemplateView.ascx.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2013
*
*  Description: A view form for generic template fields.
*
*  Notes:
* $Id: GenericTemplateView.ascx.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Sep 20, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System.Linq;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// A view form for generic template fields.
    /// </summary>
    public partial class GenericTemplateView : BaseUserControl
    {
        #region Methods

        /// <summary>
        /// Display generic template model.
        /// </summary>
        /// <param name="template">Generic template model.</param>
        public void Display(TemplateModel template)
        {
            if (template == null)
            {
                return;
            }

            var templateFields = GenericTemplateUtil.GetAllFields(template);

            if (templateFields != null && templateFields.Any())
            {
                // filter the field which displable in aca is hidden.
                templateFields = templateFields.Where(f =>
                    !ACAConstant.ASISecurity.None.Equals(ASISecurityUtil.GetASISecurity(f.serviceProviderCode, f.groupName, f.subgroupName, f.fieldName, ModuleName), System.StringComparison.OrdinalIgnoreCase)
                    && (f.acaTemplateConfig == null
                        || (f.acaTemplateConfig != null && !ValidationUtil.IsHidden(f.acaTemplateConfig.acaDisplayFlag) && !ValidationUtil.IsNo(f.acaTemplateConfig.acaDisplayFlag))));

                templateFieldsList.DataSource = templateFields;
                templateFieldsList.DataBind();
            }

            if (template.templateTables != null)
            {
                genericTemplateTable.Display(template.templateTables);
                divGenericTemplateTable.Visible = true;
            }
        }

        /// <summary>
        /// Handle ItemDataBound event for Template repeater.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void TemplateFieldsList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var field = e.Item.DataItem as GenericTemplateAttribute;

                if (string.IsNullOrEmpty(field.defaultValue))
                {
                    e.Item.Visible = false;
                    return;
                }

                var lblFieldName = e.Item.FindControl("lblFieldName") as AccelaNameValueLabel;
                string fieldLabel = I18nStringUtil.GetString(field.displayFieldName, field.fieldName);

                if (field.acaTemplateConfig != null)
                {
                    string altLabel = I18nStringUtil.GetString(field.acaTemplateConfig.resFieldLabel, field.acaTemplateConfig.fieldLabel);
                    fieldLabel = I18nStringUtil.GetString(altLabel, fieldLabel);
                }

                lblFieldName.Text = fieldLabel + ":";
                lblFieldName.Value = ScriptFilter.FilterScript(ModelUIFormat.GetTemplateValue4Display(field)) + field.unitType;
            }
        }

        #endregion
    }
}
