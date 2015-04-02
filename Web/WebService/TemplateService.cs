#region Header

/*
 * <pre>
 *  Accela Citizen Access
 *  File: TemplateService.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 * Service for JS function called in client end
 *  Notes:
 * $Id: TemplateService.cs 144988 2009-08-27 07:59:40Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Text;
using System.Web.Services;

using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.WebService
{
    /// <summary>
    /// Template Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class TemplateService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateService"/> class.
        /// </summary>
        public TemplateService()
        {
            if (AppSession.User == null)
            {
                throw new ACAException("unauthenticated web service invoking");
            }
        }

        /// <summary>
        /// it is a web service method, gets template EMSE drop down list option from the third web service.
        /// </summary>
        /// <param name="licenseType">License Type</param>
        /// <param name="licenseNbr">License number</param>
        /// <param name="licenseAgency">License agency</param>
        /// <returns>a option string</returns>
        [WebMethod(Description = "GetEMSEDDOption", EnableSession = true)]
        public string GetEMSEDDOption(string licenseType, string licenseNbr, string licenseAgency)
        {
            string optionString = string.Empty;

            if (string.IsNullOrEmpty(licenseType) || string.IsNullOrEmpty(licenseNbr))
            {
                return optionString;
            }

            optionString = BuildDDLOptions(licenseType, licenseNbr, licenseAgency);

            return optionString;
        }

        /// <summary>
        /// Structures the template EMSE drop down list fields' options.
        /// </summary>
        /// <param name="licenseType">license type</param>
        /// <param name="licenseNbr">license number</param>
        /// <param name="licenseAgency">license agency</param>
        /// <returns>an options' string</returns>
        private string BuildDDLOptions(string licenseType, string licenseNbr, string licenseAgency)
        {
            ITemplateBll templateBll = ObjectFactory.GetObject(typeof(ITemplateBll)) as ITemplateBll;
            TemplateAttributeModel[] templatesWithEMSEOptions = templateBll.GetLPAttributes4SupportEMSE(
                string.IsNullOrEmpty(licenseAgency) ? ConfigManager.AgencyCode : licenseAgency,
                licenseType,
                string.Empty,
                licenseNbr,
                AppSession.User.PublicUserId);

            StringBuilder sbOptions = new StringBuilder();

            if (templatesWithEMSEOptions != null)
            {
                foreach (TemplateAttributeModel template in templatesWithEMSEOptions)
                {
                    if (string.IsNullOrEmpty(template.attributeScriptCode) ||
                        template.selectOptions == null || template.selectOptions.Length <= 0)
                    {
                        continue;
                    }

                    sbOptions.Append(StructureFieldOptions(template.attributeName, template.selectOptions));
                }
            }

            string optionString = sbOptions.ToString();

            if (!string.IsNullOrEmpty(optionString.Trim()))
            {
                optionString = "[" + optionString.Substring(0, optionString.Length - 1) + "]";
            }

            return optionString;
        }

        /// <summary>
        /// structure a template field options
        /// </summary>
        /// <param name="attributeName">template field name</param>
        /// <param name="templateFieldOptions">template field options</param>
        /// <returns>an options' string</returns>
        private string StructureFieldOptions(string attributeName, TemplateAttrValueModel[] templateFieldOptions)
        {
            StringBuilder sbOptions = new StringBuilder();

            foreach (TemplateAttrValueModel templateValue in templateFieldOptions)
            {
                if (templateValue == null)
                {
                    continue;
                }

                string resValue = string.IsNullOrEmpty(templateValue.resAttributeValue) ?
                    templateValue.attributeValue : templateValue.resAttributeValue;
                sbOptions.Append(BuildOneDDLOption(attributeName, templateValue.attributeValue, resValue)).Append(",");
            }

            return sbOptions.ToString();
        }

        /// <summary>
        /// Structures one option.
        /// </summary>
        /// <param name="attributeName">template field name.</param>
        /// <param name="optionValue">The option value.</param>
        /// <param name="optionText">The option text.</param>
        /// <returns>return an option string.</returns>
        private string BuildOneDDLOption(string attributeName, string optionValue, string optionText)
        {
            StringBuilder sbOption = new StringBuilder();
            sbOption.Append("{");
            sbOption.Append("'AttributeName'").Append(":").Append("'").Append(attributeName).Append("'");
            sbOption.Append(",");
            sbOption.Append("'ListValue'").Append(":").Append("'").Append(optionValue).Append("'");
            sbOption.Append(",");
            sbOption.Append("'ListText'").Append(":").Append("'").Append(optionText).Append("'");
            sbOption.Append("}");

            return sbOption.ToString();
        }
    }
}