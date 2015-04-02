#region Header

/**
 *  Accela Citizen Access
 *  File: TemplateUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *   Provide template related function.
 *
 *  Notes:
 * $Id: TemplateUtil.cs 175327 2010-06-10 09:06:19Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// Template utility class
    /// </summary>
    public static class TemplateUtil
    {
        /// <summary>
        /// Get control IDs for always editable template fields.
        /// </summary>
        /// <param name="attributeModels">Template fields.</param>
        /// <param name="fieldIDPrefix">
        /// Control ID prefix. Please use the same prefix which create the template fields in TemplateEdit.ascx control.
        /// </param>
        /// <returns>String array for the control IDs which are always editable.</returns>
        public static string[] GetAlwaysEditableControlIDs(TemplateAttributeModel[] attributeModels, string fieldIDPrefix)
        {
            List<string> fieldIDList = new List<string>();

            if (attributeModels != null && attributeModels.Length > 0)
            {
                foreach (TemplateAttributeModel field in attributeModels)
                {
                    if (ACAConstant.TEMPLATE_FIIELD_STATUS_ALWAYSEDITABLE.Equals(field.vchFlag, StringComparison.InvariantCultureIgnoreCase))
                    {
                        string controlId = TemplateUtil.GetTemplateControlID(field.attributeName, fieldIDPrefix);
                        fieldIDList.Add(controlId);
                    }
                }
            }

            return fieldIDList.ToArray();
        }

        /// <summary>
        /// Merge PeopleTemplateAttributeModel <see cref="sourceTemplateAttributeModelList"/> to <see cref="targetTemplateAttributeModelList"/>.
        /// </summary>
        /// <param name="sourceTemplateAttributeModelList">Source TemplateAttributeModel.</param>
        /// <param name="targetTemplateAttributeModelList">Target TemplateAttributeModel.</param>
        public static void MergePeopleTemplateAttributes(TemplateAttributeModel[] sourceTemplateAttributeModelList, TemplateAttributeModel[] targetTemplateAttributeModelList)
        {
            if (sourceTemplateAttributeModelList == null || targetTemplateAttributeModelList == null)
            {
                return;
            }

            foreach (var targetField in targetTemplateAttributeModelList)
            {
                foreach (var sourceField in sourceTemplateAttributeModelList)
                {
                    if (string.Equals(targetField.attributeName, sourceField.attributeName)
                        && targetField.attributeValueDataType == sourceField.attributeValueDataType)
                    {
                        targetField.attributeValue = sourceField.attributeValue;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Merge the template attribute for the licensee with the latest template attribute set for the current licensee's licensed professional type.
        /// </summary>
        /// <param name="sourceTemplateAttributes">The reference template attributes associated with the control type</param>
        /// <param name="targetTemplateAttributes">The template attributes associated with the record.</param>
        /// <returns>The merged template attributes</returns>
        public static TemplateAttributeModel[] MergeLicensedProfessionalTemplateAttributes(TemplateAttributeModel[] sourceTemplateAttributes, TemplateAttributeModel[] targetTemplateAttributes)
        {
            if (sourceTemplateAttributes == null || sourceTemplateAttributes.Length == 0)
            {
                return targetTemplateAttributes;
            }

            if (targetTemplateAttributes == null || targetTemplateAttributes.Length == 0)
            {
                return sourceTemplateAttributes;
            }

            List<TemplateAttributeModel> resultList = new List<TemplateAttributeModel>(targetTemplateAttributes);

            foreach (var sourceAttribute in sourceTemplateAttributes.Where(s => s != null))
            {
                var mergedAttribute = sourceAttribute;
                bool existSameAttribute = false;

                for (int i = 0; i < targetTemplateAttributes.Length; i++)
                {
                    var targetAttribute = targetTemplateAttributes[i];

                    if (targetAttribute == null)
                    {
                        continue;
                    }

                    if (sourceAttribute.attributeName.Equals(targetAttribute.attributeName, StringComparison.InvariantCultureIgnoreCase)
                        && sourceAttribute.attributeValueDataType.Equals(targetAttribute.attributeValueDataType, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //If find the same attribute field, merge the template attribute properties and attribute value from sourceAttribute to targetAttribute.
                        var temp = sourceAttribute;
                        temp.attributeValue = !string.IsNullOrEmpty(targetAttribute.attributeValue) ? targetAttribute.attributeValue : temp.attributeValue;
                        resultList[i] = temp;

                        existSameAttribute = true;
                        break;
                    }
                }

                if (!existSameAttribute)
                {
                    resultList.Add(mergedAttribute);
                }
            }

            return resultList.ToArray();
        }

        /// <summary>
        /// convert the attribute name to control id for all template(APO/Contact/LP).
        /// </summary>
        /// <param name="attributeName">attribute name of template.</param>
        /// <returns>control id of template.</returns>
        public static string GetTemplateControlID(string attributeName)
        {
            return GetTemplateControlID(attributeName, string.Empty);
        }

        /// <summary>
        /// convert the attribute name to control id for all template(APO/Contact/LP).
        /// </summary>
        /// <param name="attributeName">attribute name of template.</param>
        /// <param name="prefix">control id prefix for each type of template.</param>
        /// <returns>control id of template.</returns>
        public static string GetTemplateControlID(string attributeName, string prefix)
        {
            return prefix + "Template" + attributeName.GetHashCode().ToString("X2");
        }

        /// <summary>
        /// synchronized the models, remove the model that has merged.
        /// </summary>
        /// <param name="models">The SimpleViewElementModel.</param>
        /// <param name="dictTemplateModel">The template fields that have same label.</param>
        /// <returns>
        /// Return the models that merged the same label template fields.
        /// </returns>
        public static SimpleViewElementModel4WS[] MergeTemplateSimpleViewElement(SimpleViewElementModel4WS[] models, Dictionary<string, IList<SimpleViewElementModel4WS>> dictTemplateModel = null)
        {
            List<SimpleViewElementModel4WS> mergedModels = new List<SimpleViewElementModel4WS>();
            List<string> mergedFieldLabels = new List<string>();

            if (dictTemplateModel == null)
            {
                dictTemplateModel = GetMergedTemplateViewElement(models);
            }

            foreach (SimpleViewElementModel4WS model in models)
            {
                SimpleViewElementModel4WS item = model;
                bool isStandard = !ValidationUtil.IsNo(model.standard);

                // the templates have same label need merge, so left only one model. 
                if (!isStandard && mergedFieldLabels.Contains(model.labelValue))
                {
                    continue;
                }

                // add the template model to result list
                if (!isStandard
                    && !string.Equals(model.elementType, ControlType.Table.ToString(), StringComparison.InvariantCultureIgnoreCase)
                    && dictTemplateModel.Keys.Contains(model.labelValue))
                {
                    item = GetCustomizedTemplateField(dictTemplateModel[model.labelValue]);
                    mergedFieldLabels.Add(model.labelValue);
                }

                mergedModels.Add(item);
            }

            return mergedModels.ToArray();
        }

        /// <summary>
        /// Get the template view element model list that have merged the same display label.
        /// </summary>
        /// <param name="models">The SimpleViewElementModel4WS.</param>
        /// <returns>Return the template view element model list that merged same template label </returns>
        public static Dictionary<string, IList<SimpleViewElementModel4WS>> GetMergedTemplateViewElement(SimpleViewElementModel4WS[] models)
        {
            // key: label, value: list for simple view element model
            Dictionary<string, IList<SimpleViewElementModel4WS>> dict = new Dictionary<string, IList<SimpleViewElementModel4WS>>();

            foreach (SimpleViewElementModel4WS model in models)
            {
                if (!ValidationUtil.IsNo(model.standard)
                    || string.Equals(model.elementType, ControlType.Table.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                if (dict.ContainsKey(model.labelValue))
                {
                    IList<SimpleViewElementModel4WS> templateModels = dict[model.labelValue];
                    templateModels.Add(model);

                    dict[model.labelValue] = templateModels;
                }
                else
                {
                    dict.Add(model.labelValue, new List<SimpleViewElementModel4WS> { model });
                }
            }

            return dict;
        }

        /// <summary>
        /// Fill the I18N value for dropdownlist fields.
        /// </summary>
        /// <param name="primaryLangValues">Primary language attributes.</param>
        /// <param name="i18nValues">I18N attributes.</param>
        public static void FillI18NValueForDropDownList(TemplateAttributeModel[] primaryLangValues, TemplateAttributeModel[] i18nValues)
        {
            if (i18nValues != null && i18nValues.Length != 0 && primaryLangValues != null && primaryLangValues.Length != 0)
            {
                foreach (TemplateAttributeModel primaryLangValue in primaryLangValues)
                {
                    if (primaryLangValue == null || !primaryLangValue.attributeValueDataType.Equals(ControlType.DropdownList.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    TemplateAttributeModel i18nModel =
                            i18nValues.Where(
                                o => o.attributeName == primaryLangValue.attributeName
                                     && o.attributeValueDataType == primaryLangValue.attributeValueDataType).FirstOrDefault();

                    if (i18nModel != null && i18nModel.selectOptions != null && i18nModel.selectOptions.Length != 0)
                    {
                        var resValue =
                            i18nModel.selectOptions.Where(o => o.attributeValue == primaryLangValue.attributeValue).Select(o => o.resAttributeValue).FirstOrDefault();

                        if (!string.IsNullOrEmpty(resValue))
                        {
                            primaryLangValue.attributeValue = resValue;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fills the i18N value for drop down list.
        /// </summary>
        /// <param name="primaryLangValues">The primary lang value.</param>
        /// <param name="i18nValues">The i18n values.</param>
        /// <returns>return the i18n value</returns>
        public static string GetI18NValueForDropDownList(TemplateAttributeModel primaryLangValues, TemplateAttributeModel[] i18nValues)
        {
            TemplateAttributeModel objClone = ObjectCloneUtil.DeepCopy(primaryLangValues);

            FillI18NValueForDropDownList(new TemplateAttributeModel[] { objClone }, i18nValues);

            return objClone == null ? string.Empty : objClone.attributeValue;
        }

        /// <summary>
        /// Create the client empty validation script.
        /// </summary>
        /// <param name="fieldType">The control's type.</param>
        /// <param name="clientID">The control's client id.</param>
        /// <returns>Return the client script for empty validation.</returns>
        public static string CreateEmptyValidationScript(FieldType fieldType, string clientID)
        {
            StringBuilder script = new StringBuilder();
            script.AppendFormat("control = $get('{0}');", clientID);
            script.AppendLine();

            switch (fieldType)
            {
                case FieldType.HTML_RADIOBOX:
                case FieldType.HTML_CHECKBOX:
                    script.AppendLine("if(control && $(control).is(':visible')){");
                    script.AppendLine("    if($(control).is(':checked')) return false;");
                    script.AppendLine("    var radios=control.getElementsByTagName('input');");
                    script.AppendLine("    for(var i=0;i<radios.length;i++){");
                    script.AppendLine("        if(radios[i].type=='radio' || radios[i].type=='checkbox'){");
                    script.AppendLine("            if(radios[i].checked){isNotEmpty=true;}");
                    script.AppendLine("        }");
                    script.AppendLine("    }");
                    script.AppendLine("}");
                    break;
                case FieldType.HTML_TEXTBOX_OF_DATE:
                    script.AppendLine("if(control && $(control).is(':visible'))isNotEmpty = GetValue(control).replace('/','').replace('/','').trim() != '';");
                    break;
                default:
                    script.AppendLine("if(control && $(control).is(':visible'))isNotEmpty = GetValue(control).trim() != '';");
                    break;
            }

            script.AppendLine("if(isNotEmpty)return false;");
            script.AppendLine();

            return script.ToString();
        }

        /// <summary>
        /// Get always editable required template field.
        /// </summary>
        /// <param name="templateAttributes">TemplateAttributeModel collection</param>
        /// <returns>return List for TemplateAttributeModel</returns>
        public static List<TemplateAttributeModel> GetAlwaysEditableRequiredTemplateFields(TemplateAttributeModel[] templateAttributes)
        {
            List<TemplateAttributeModel> fields = new List<TemplateAttributeModel>();

            if (templateAttributes != null)
            {
                foreach (TemplateAttributeModel attribute in templateAttributes)
                {
                    if (attribute != null && ACAConstant.TEMPLATE_FIIELD_STATUS_ALWAYSEDITABLE.Equals(attribute.vchFlag, StringComparison.InvariantCultureIgnoreCase))
                    {
                        fields.Add(attribute);
                    }
                }
            }

            return fields;
        }

        /// <summary>
        /// Judge whether it exists always editable required template field.
        /// </summary>
        /// <param name="templateAttributes">TemplateAttributeModel collection</param>
        /// <returns>Return true if it exists always editable required template field.</returns>
        public static bool IsExistsAlwaysEditableRequiredTemplateField(TemplateAttributeModel[] templateAttributes)
        {
            List<TemplateAttributeModel> fields = new List<TemplateAttributeModel>();

            if (templateAttributes != null)
            {
                foreach (TemplateAttributeModel attribute in templateAttributes)
                {
                    if (attribute != null && ACAConstant.TEMPLATE_FIIELD_STATUS_ALWAYSEDITABLE.Equals(attribute.vchFlag, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Judge whether all the values are empty
        /// </summary>
        /// <param name="attributes">A TemplateAttributeModel array</param>
        /// <returns>true-all fields are empty, otherwise returns false.</returns>
        public static bool IsEmpty(TemplateAttributeModel[] attributes)
        {
            if (attributes == null || attributes.Length == 0)
            {
                return true;
            }

            bool isEmpty = true;

            foreach (TemplateAttributeModel attr in attributes)
            {
                if (attr != null && attr.attributeValue != null && attr.attributeValue.Trim() != string.Empty)
                {
                    isEmpty = false;
                    break;
                }
            }

            return isEmpty;
        }

        /// <summary>
        /// Get the template field that has been customized.
        /// </summary>
        /// <param name="templateModels">The template models.</param>
        /// <returns>Return the merged template's status.</returns>
        private static SimpleViewElementModel4WS GetCustomizedTemplateField(IList<SimpleViewElementModel4WS> templateModels)
        {
            if (templateModels == null || templateModels.Count == 0)
            {
                return null;
            }

            SimpleViewElementModel4WS result = templateModels[0];

            foreach (SimpleViewElementModel4WS model in templateModels)
            {
                if (!model.isOriginalTemplate)
                {
                    result = model;
                    break;
                }
            }

            return result;
        }
    }
}
