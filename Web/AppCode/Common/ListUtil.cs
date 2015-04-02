#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ListUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ListUtil.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// an utility class for Cap relevant logic.
    /// </summary>
    public static class ListUtil
    {
        #region Methods

        /// <summary>
        /// Find Row Current position.
        /// </summary>
        /// <param name="rowIndex">the selected row Index.</param>
        /// <param name="dataSource">the data source</param>
        /// <param name="columnName">the column name</param>
        /// <returns>the row position.</returns>
        public static int FindPos(int rowIndex, DataTable dataSource, string columnName)
        {
            int pos = -1;

            if (dataSource == null)
            {
                return -1;
            }

            DataRowCollection rows = dataSource.Rows;

            if (rows != null && rows.Count > 0)
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    if (Convert.ToInt32(rows[i][columnName]) == rowIndex)
                    {
                        pos = i;
                        break;
                    }
                }
            }

            return pos;
        }

        /// <summary>
        /// Because data table don't update row index when delete a record from list.
        /// </summary>
        /// <param name="dtSource">data table data source</param>
        /// <param name="columnName">index name in list</param>
        public static void UpdateRowIndex(DataTable dtSource, string columnName)
        {
            if (dtSource == null || dtSource.Rows == null || dtSource.Rows.Count == 0)
            {
                return;
            }

            string capContactColumnName = ColumnConstant.Contact.CapContactModel.ToString();

            for (int rowIndex = 0; rowIndex < dtSource.Rows.Count; rowIndex++)
            {
                DataRow dr = dtSource.Rows[rowIndex];

                /* If the data source contains the CapContactModel4WS, it need update 
                 * its child PeopleModel's RowIndex which will be used.
                 */
                if (dtSource.Columns.Contains(capContactColumnName))
                {
                    CapContactModel4WS capContactModel = dr[capContactColumnName] as CapContactModel4WS;

                    if (capContactModel != null && capContactModel.people != null)
                    {
                        capContactModel.people.RowIndex = rowIndex;
                    }
                }

                dr[columnName] = rowIndex;
            }
        }

        #endregion Methods
    }
}
