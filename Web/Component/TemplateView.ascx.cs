#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TemplateView.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: TemplateView.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Provides the ability to dynamical display the custom attributes for APO or people template.
    /// </summary>
    public partial class TemplateView : BaseUserControl
    {
        #region Methods

        /// <summary>
        /// Displays the template or daily attributes to UI.
        /// if there is value in attribute object, the value need to be set as default value.
        /// </summary>
        /// <remarks>
        /// 1.Stores the attributeModels array to ViewState so that 
        ///   the current control can get the attributeModels from ViewState when the page is PostBack
        /// 2.Invoke CreateControls() to create all of web controls dynamically.
        /// </remarks>
        /// <param name="attributeModels">TemplateAttributeModel array to be displayed as a template.</param>
        public void DisplayAttributes(TemplateAttributeModel[] attributeModels)
        {
            // 1.Handle the label name: replace the label with name when label is empty.
            // Repeater will bind attributeLabel
            if (attributeModels != null && attributeModels.Length > 0)
            {
                foreach (TemplateAttributeModel item in attributeModels)
                {
                    if (string.IsNullOrEmpty(item.attributeLabel))
                    {
                        item.attributeLabel = item.attributeName;
                    }
                }
            }

            // 2.Bind data to present the attributes
            rptAttribute.DataSource = attributeModels;
            rptAttribute.DataBind();
        }

        /// <summary>
        /// Display Template value and unit
        /// </summary>
        /// <param name="attributeModel">The attribute model</param>
        /// <returns>The template value and unit.</returns>
        protected string DisplayTemplateValue(TemplateAttributeModel attributeModel)
        {
            string templateValue = ScriptFilter.FilterScript(ModelUIFormat.GetTemplateValue4Display(attributeModel));
            string templateUnit = ScriptFilter.FilterScript(I18nStringUtil.GetString(attributeModel.resAttributeUnitType, attributeModel.attributeUnitType));
            return string.Format("{0}{1}", templateValue, templateUnit);
        }

        /// <summary>
        /// <c>OnInit</c> event method.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Accela.ACA.Web.Common.AppSession.IsAdmin)
            {
                Visible = false;
            }
        }

        /// <summary>
        /// Hide data row when value field is null
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void AttributeRepeater_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TemplateAttributeModel tpItem = (TemplateAttributeModel)e.Item.DataItem;

                if (string.IsNullOrEmpty(tpItem.vchFlag) || ACAConstant.COMMON_N.Equals(tpItem.vchFlag) || string.IsNullOrEmpty(tpItem.attributeValue))
                {
                    e.Item.Visible = false;
                }
            }
        }

        #endregion Methods
    }
}