#region Header

/**
 *  Accela Citizen Access
 *  File: ObjectConvertUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *   It provides object convert utility to serve the framework.
 *
 *  Notes:
 * $Id: ObjectConvertUtil.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// It providers the object convert function.
    /// </summary>
    public static class ObjectConvertUtil
    {
        #region Fields

        /// <summary>
        /// Row index for add/edit/delete record.
        /// </summary>
        private const string ROWINDEX = "RowIndex";

        /// <summary>
        /// Model name space for judge current property whether is model.
        /// </summary>
        private const string MODEL_NAMESPACE = "Accela.ACA.WSProxy";

        #endregion Fields

        #region Methods

        /// <summary>
        /// Convert Array To List Collection
        /// </summary>
        /// <typeparam name="T">Object Module</typeparam>
        /// <param name="array">Array Collection</param>
        /// <returns>List Collection</returns>
        public static List<T> ConvertArrayToList<T>(T[] array)
        {
            if (array == null)
            {
                return null;
            }

            List<T> list = new List<T>();
            list.AddRange(array);

            return list;
        }

        /// <summary>
        /// Convert models to data table.
        /// </summary>
        /// <typeparam name="T">The model type.</typeparam>
        /// <param name="models">model list</param>
        /// <param name="isNeedIndex">indicate whether need row index</param>
        /// <returns>data table</returns>
        public static DataTable ConvertModels2DataTable<T>(T[] models, bool isNeedIndex)
        {
            return ConvertModels2DataTable(models, isNeedIndex, true);
        }

        /// <summary>
        /// Convert models to data table.
        /// </summary>
        /// <typeparam name="T">The model type.</typeparam>
        /// <param name="models">model list</param>
        /// <param name="isNeedIndex">indicate whether need row index</param>
        /// <param name="isConvertChildModel">Set if the child model need convert.</param>
        /// <returns>data table</returns>
        public static DataTable ConvertModels2DataTable<T>(T[] models, bool isNeedIndex, bool isConvertChildModel)
        {
            DataTable dt = new DataTable();

            if (models == null || models.Length == 0)
            {
                return dt;
            }

            // Add row index column.
            if (isNeedIndex)
            {
                dt.Columns.Add(new DataColumn(ROWINDEX, typeof(int)));
            }

            DataRow row = null;

            for (int i = 0; i < models.Length; i++)
            {
                row = dt.NewRow();

                // Convert model to data row.
                ConvertModel2DataRow(row, dt, models[i], isConvertChildModel);

                // set value to row index column.
                if (isNeedIndex)
                {
                    row[ROWINDEX] = i;
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        /// <summary>
        /// Convert model to data row.
        /// </summary>
        /// <param name="model">model object</param>
        /// <param name="isNeedIndex">indicate whether need add row index column</param>
        /// <param name="isConvertChildModel">if set to <c>true</c> [is convert child model].</param>
        /// <returns>data row object</returns>
        public static DataRow ConvertModel2DataRow(object model, bool isNeedIndex, bool isConvertChildModel)
        {
            DataTable dt = new DataTable();
            DataRow row = dt.NewRow();

            if (model == null)
            {
                return row;
            }

            // Add row index column.
            if (isNeedIndex)
            {
                dt.Columns.Add(new DataColumn(ROWINDEX, typeof(int)));
            }

            // convert model to data row.
            ConvertModel2DataRow(row, dt, model, isConvertChildModel);

            dt.Rows.Add(row);

            return dt.Rows[0];
        }

        /// <summary>
        /// Update data table with extract child model.
        /// </summary>
        /// <param name="dtSource">The data source.</param>
        /// <param name="childModelName">The child's model name.</param>
        /// <returns>The DataTable that extract the child's model to the DataTable's columns.</returns>
        public static DataTable ExtractChildModel(DataTable dtSource, string childModelName)
        {
            if (dtSource == null || dtSource.Rows.Count == 0)
            {
                return dtSource;
            }

            PropertyInfo[] properties = dtSource.Rows[0][childModelName].GetType().GetProperties();

            foreach (DataRow dtRow in dtSource.Rows)
            {
                foreach (var propertyInfo in properties)
                {
                    string childColumnName = childModelName + "." + propertyInfo.Name;

                    // define the columns for child model
                    if (dtSource.Columns[childColumnName] == null)
                    {
                        Type columnType = propertyInfo.PropertyType;

                        // remove the "Nullable<>" generic type, because the data table column NOT support this type.
                        if (columnType.IsGenericType && columnType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            columnType = columnType.GetGenericArguments()[0];
                        }

                        dtSource.Columns.Add(childColumnName, columnType);
                    }

                    // assign value to child model's row
                    dtRow[childColumnName] = propertyInfo.GetValue(dtRow[childModelName], null);
                }
            }

            return dtSource;
        }

        /// <summary>
        /// Construct empty data table.
        /// </summary>
        /// <param name="dt">data table</param>
        /// <param name="model">model object</param>
        public static void ConstructEmptyDataTable(DataTable dt, object model)
        {
            PropertyInfo[] properties = model.GetType().GetProperties();

            if (dt.Columns[ROWINDEX] == null)
            {
                dt.Columns.Add(new DataColumn(ROWINDEX, typeof(int)));
            }

            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.PropertyType.FullName.IndexOf(MODEL_NAMESPACE) > -1)
                {
                    if (propertyInfo.GetValue(model, null) != null)
                    {
                        // Recursive to find sub model's properties.
                        ConstructEmptyDataTable(dt, propertyInfo.GetValue(model, null));
                    }
                }
                else
                {
                    if (dt.Columns[propertyInfo.Name] == null)
                    {
                        dt.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
                    }
                }
            }
        }

        /// <summary>
        /// Convert object[] to generic array.
        /// </summary>
        /// <typeparam name="T">generic type</typeparam>
        /// <param name="objectArray">object array</param>
        /// <returns>generic array</returns>
        public static T[] ConvertObjectArray2EntityArray<T>(object[] objectArray)
        {
            T[] entityArray = null;

            if (objectArray != null && objectArray.Length > 0)
            {
                entityArray = new T[objectArray.Length];
                for (int i = 0; i < objectArray.Length; i++)
                {
                    entityArray[i] = (T)objectArray[i];
                }
            }

            return entityArray;
        }

        /// <summary>
        /// Convert model to data row.
        /// </summary>
        /// <param name="row">data row object</param>
        /// <param name="dt">data table</param>
        /// <param name="model">model object</param>
        /// <param name="isConvertChildModel">Set if the child model need convert.</param>
        private static void ConvertModel2DataRow(DataRow row, DataTable dt, object model, bool isConvertChildModel)
        {
            PropertyInfo[] properties = model.GetType().GetProperties();

            foreach (PropertyInfo propertyInfo in properties)
            {
                if (isConvertChildModel && propertyInfo.PropertyType.FullName.IndexOf(MODEL_NAMESPACE) > -1)
                {
                    if (propertyInfo.GetValue(model, null) != null)
                    {
                        ConvertModel2DataRow(row, dt, propertyInfo.GetValue(model, null), true);
                    }
                }
                else
                {
                    if (dt.Columns[propertyInfo.Name] == null)
                    {
                        Type columnType = propertyInfo.PropertyType;

                        // remove the "Nullable<>" generic type, because the data table column NOT support this type.
                        if (columnType.IsGenericType && columnType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            columnType = columnType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(propertyInfo.Name, columnType);
                    }

                    row[propertyInfo.Name] = propertyInfo.GetValue(model, null) ?? DBNull.Value;
                }
            }
        }

        #endregion Methods
    }
}
