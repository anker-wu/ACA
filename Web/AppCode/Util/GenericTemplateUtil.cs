#region Header

/**
 *
 * <pre>
 *  Accela Citizen Access
 *  File: GenericTemplateUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description: Common functions for generic template.
 *  
 * </pre>
 */

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Util
{
    /// <summary>
    /// Common functions for generic template.
    /// </summary>
    public static class GenericTemplateUtil
    {
        /// <summary>
        /// Get all fields from generic template model.
        /// </summary>
        /// <param name="template">template model.</param>
        /// <returns>Collection of GenericTemplateAttribute.</returns>
        public static IEnumerable<GenericTemplateAttribute> GetAllFields(TemplateModel template)
        {
            var templateFields =
                template == null || template.templateForms == null ? null :
                from form in template.templateForms
                where form != null && form.subgroups != null
                from subgroup in form.subgroups
                where subgroup != null && subgroup.fields != null
                from field in subgroup.fields
                where field != null
                select field;

            return templateFields;
        }

        /// <summary>
        /// Merge generic template and generic template table from <see cref="sourceTemplate"/> to <see cref="targetTemplate"/>.
        /// </summary>
        /// <param name="sourceTemplate">Source generic template model.</param>
        /// <param name="targetTemplate">Target generic template model.</param>
        /// <param name="moduleName">Module name used to get the ASI/ASIT security settings.</param>
        public static void MergeGenericTemplate(TemplateModel sourceTemplate, TemplateModel targetTemplate, string moduleName)
        {
            if (sourceTemplate == null || targetTemplate == null)
            {
                return;
            }

            //Merge generic template form.
            MergeGenericTemplateForm(sourceTemplate, targetTemplate, moduleName);

            //Merge generic template tables.
            MergeGenericTemplateTable(sourceTemplate.templateTables, targetTemplate.templateTables, moduleName);
        }

        /// <summary>
        /// Merge generic template form. <see cref="sourceTemplate"/> to <see cref="targetTemplate"/>.
        /// </summary>
        /// <param name="sourceTemplate">Source generic template model.</param>
        /// <param name="targetTemplate">Target generic template model.</param>
        /// <param name="moduleName">Module name used to get the ASI/ASIT security settings.</param>
        public static void MergeGenericTemplateForm(TemplateModel sourceTemplate, TemplateModel targetTemplate, string moduleName)
        {
            if (sourceTemplate == null || targetTemplate == null)
            {
                return;
            }

            //Merge generic template fields.
            var sFields = GetAllFields(sourceTemplate);
            var tFields = GetAllFields(targetTemplate);

            if (tFields != null && sFields != null)
            {
                foreach (var tFld in tFields)
                {
                    foreach (var sFld in sFields)
                    {
                        if (string.Equals(tFld.fieldName, sFld.fieldName, StringComparison.OrdinalIgnoreCase)
                            && tFld.fieldType == sFld.fieldType)
                        {
                            tFld.defaultValue = sFld.defaultValue;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Merge visible generic template tables from <see cref="sourceTemplateTables"/> to <see cref="targetTemplateTables"/>.
        /// </summary>
        /// <param name="sourceTemplateTables">Source generic template tables.</param>
        /// <param name="targetTemplateTables">Target generic template tables.</param>
        /// <param name="moduleName">Module name used to get the ASI/ASIT security settings.</param>
        /// <returns>Return the merged tables in <see cref="targetTemplateTables"/>.</returns>
        public static TemplateSubgroup[] MergeGenericTemplateTable(TemplateGroup[] sourceTemplateTables, TemplateGroup[] targetTemplateTables, string moduleName)
        {
            TemplateSubgroup[] mergedTables = null;

            if (sourceTemplateTables != null && targetTemplateTables != null)
            {
                TemplateSubgroup[] sourceTables = CapUtil.GetGenericTemplateTables(moduleName, sourceTemplateTables, false);
                TemplateSubgroup[] targetTables = CapUtil.GetGenericTemplateTables(moduleName, targetTemplateTables, false);

                mergedTables = MergeGenericTemplateTable(sourceTables, targetTables);
            }

            return mergedTables;
        }

        /// <summary>
        /// Merge visible generic template tables from <see cref="sourceTables"/> to <see cref="targetTables"/>.
        /// </summary>
        /// <param name="sourceTables">Source generic template tables.</param>
        /// <param name="targetTables">Target generic template tables.</param>
        /// <returns>Return the merged tables in <see cref="targetTables"/>.</returns>
        public static TemplateSubgroup[] MergeGenericTemplateTable(TemplateSubgroup[] sourceTables, TemplateSubgroup[] targetTables)
        {
            if (sourceTables == null || targetTables == null)
            {
                return null;
            }

            IList<TemplateSubgroup> mergedTables = null;

            foreach (TemplateSubgroup tTable in targetTables)
            {
                foreach (TemplateSubgroup sTable in sourceTables)
                {
                    if (tTable.fields == null || sTable.fields == null
                        || !tTable.fields.Any() || !sTable.fields.Any()
                        || tTable.fields.Count() != sTable.fields.Count())
                    {
                        continue;
                    }

                    /*
                     * Generic template table merge logic:
                     * 1. Source table and Target table have the same structure(same columns/same column orders/same column name/same column type)
                     * 2. Use source table data rows to fill the target table, if source table is empty, target table will be empty.
                     */
                    bool isSameStructure = true;

                    for (int i = 0; i < tTable.fields.Length; i++)
                    {
                        if (sTable.fields[i].fieldName != tTable.fields[i].fieldName || sTable.fields[i].fieldType != tTable.fields[i].fieldType)
                        {
                            isSameStructure = false;
                            break;
                        }
                    }

                    if (isSameStructure)
                    {
                        if (mergedTables == null)
                        {
                            mergedTables = new List<TemplateSubgroup>();
                        }

                        //Remember merged table.
                        mergedTables.Add(tTable);

                        if (sTable.rows == null || !sTable.rows.Any())
                        {
                            tTable.rows = null;
                            continue;
                        }

                        //Row index of generic template table start with 1.
                        int rowIndex = 1;
                        tTable.rows = new TemplateRow[sTable.rows.Count()];

                        foreach (TemplateRow sRow in sTable.rows)
                        {
                            TemplateRow tRow = new TemplateRow();
                            tRow.rowIndex = rowIndex;
                            tRow.values = new GenericTemplateTableValue[tTable.fields.Count()];
                            tTable.rows[rowIndex - 1] = tRow;
                            int colIdx = 0;

                            foreach (var tColumn in tTable.fields)
                            {
                                //Copy attributes from column to field.
                                GenericTemplateTableValue tField = new GenericTemplateTableValue();
                                tRow.values[colIdx] = tField;
                                tField.serviceProviderCode = tColumn.serviceProviderCode;
                                tField.groupName = tColumn.groupName;
                                tField.subgroupName = tColumn.subgroupName;
                                tField.fieldName = tColumn.fieldName;

                                tField.entityPKModel = tColumn.entityPKModel;
                                tField.entityType = tColumn.entityType;
                                tField.auditModel = tColumn.auditModel;
                                tField.attributeSeq = tColumn.seqNum;
                                tField.columnIndex = colIdx;
                                tField.rowIndex = rowIndex;

                                //Copy field value from source field to target field.
                                GenericTemplateTableValue sField = sRow.values.SingleOrDefault(s => s.fieldName == tColumn.fieldName);
                                tField.value = sField != null ? sField.value : string.Empty;

                                colIdx++;
                            }

                            rowIndex++;
                        }
                    }
                }
            }

            return mergedTables != null ? mergedTables.ToArray() : null;
        }

        /// <summary>
        /// Gets Template Group Code
        /// </summary>
        /// <param name="model">Template Model</param>
        /// <returns>The template group code.</returns>
        public static string GetTemplateGroupCode(TemplateModel model)
        {
            string templateGroupCode = null;

            if (model != null && model.templateForms != null && model.templateForms.Length > 0)
            {
                templateGroupCode = model.templateForms[0].groupName;
            }

            if (string.IsNullOrEmpty(templateGroupCode) && model != null && model.templateTables != null && model.templateTables.Length > 0)
            {
                templateGroupCode = model.templateTables[0].groupName;
            }

            return templateGroupCode;
        }
    }
}