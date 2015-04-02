#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LicenseeGeneralInfoLPTemplate.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: LicenseeGeneralInfoLPTemplate.ascx.cs 185614 2010-12-01 06:22:24Z ACHIEVO\xinter.peng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// LicenseeGeneralInfo's LP Template
    /// </summary>
    public partial class LicenseeGeneralInfoLPTemplate : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets the collapse line.
        /// </summary>
        /// <value>The collapse line.</value>
        public string CollapseLine
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [need collapse].
        /// </summary>
        /// <value><c>true</c> if [need collapse]; otherwise, <c>false</c>.</value>
        public bool NeedCollapse
        {
            get
            {
                if (ViewState["NeedCollapse"] == null)
                {
                    return false;
                }

                return Convert.ToBoolean(ViewState["NeedCollapse"]);
            }

            set
            {
                ViewState["NeedCollapse"] = value;
            }
        }

        #endregion

        /// <summary>
        /// display licensee template
        /// </summary>
        /// <param name="templates">template attribute model</param>
        /// <returns>true: if display successfully,otherwise return false;</returns>
        public bool Display(TemplateAttributeModel[] templates)
        {
            return CreateControls(templates);
        }

        /// <summary>
        /// Creates the controls.
        /// </summary>
        /// <param name="templates">The templates.</param>
        /// <returns>Create Controls Result Flag</returns>
        private bool CreateControls(TemplateAttributeModel[] templates)
        {
            bool result = false;
            if (templates != null)
            {
                DateTime dt = new DateTime();
                var filteredTemplates = from t in templates
                                        where t != null
                                              && !string.IsNullOrEmpty(t.vchFlag)
                                              && ACAConstant.TEMPLATE_FIIELD_STATUS_LIC_VERIFICATION_MODEL.Equals(
                                                  t.vchFlag, StringComparison.OrdinalIgnoreCase)
                                        let isRadioType = ACAConstant.CONTROL_RADIO_TYPE.Equals(t.attributeValueDataType, StringComparison.OrdinalIgnoreCase)
                                        let label = string.IsNullOrEmpty(t.attributeLabel) ? t.attributeName : t.attributeLabel
                                        select new
                                                   {
                                                       Name = t.attributeName,
                                                       Label = label,
                                                       Value = isRadioType ? ModelUIFormat.FormatYNLabel(t.attributeValue) : t.attributeValue,
                                                       IsDate = I18nDateTimeUtil.TryParseFromWebService(t.attributeValue, out dt)
                                                   };

                if (filteredTemplates != null)
                {
                    foreach (var template in filteredTemplates)
                    {
                        string label = string.IsNullOrEmpty(template.Label) ? string.Empty : ScriptFilter.FilterScript(template.Label, false);
                        string value = string.IsNullOrEmpty(template.Value) ? string.Empty : template.Value;

                        AccelaNameValueLabel nameValueLabel = new AccelaNameValueLabel();
                        nameValueLabel.Font.Bold = true;
                        nameValueLabel.ID = TemplateUtil.GetTemplateControlID(template.Name, "template_Licensee_Detail_");
                        nameValueLabel.Text = label + ":" + Server.HtmlDecode("&nbsp;");
                        nameValueLabel.EnableEllipsis = NeedCollapse;
                        int collapseLines = 0;

                        if (int.TryParse(CollapseLine, out collapseLines))
                        {
                            nameValueLabel.CollapseLines = collapseLines;
                        }

                        if (template.IsDate)
                        {
                            nameValueLabel.DateType = DateType.ShortDate;
                            nameValueLabel.DateValue = value;
                        }
                        else
                        {
                            nameValueLabel.Value = value;
                        }

                        Controls.Add(nameValueLabel);
                    }

                    result = true;
                }
            }

            return result;
        }
    }
}
