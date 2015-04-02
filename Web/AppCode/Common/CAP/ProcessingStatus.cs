#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ProcessingStatus.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 * define common function on permit detail page
 *
 *  Notes:
 *      $Id: ProcessingStatus.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;

using Accela.ACA.BLL.WorkFlow;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Class to handler Processing status
    /// </summary>
    public class ProcessingStatus : PermitDetailBaseUserControl
    {
        #region Fields

        /// <summary>
        /// Define a CapModel4WS object
        /// </summary>
        private CapModel4WS _capModel;

        /// <summary>
        /// Define a SimpleTaskItemModel4WS array
        /// </summary>
        private SimpleTaskItemModel4WS[] _processList;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ProcessingStatus class.
        /// </summary>
        /// <param name="capModel">CapModel4WS object</param>
        public ProcessingStatus(CapModel4WS capModel)
        {
            _capModel = capModel;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Create data source
        /// </summary>
        /// <returns>data table.</returns>
        public DataTable CreateDataSource()
        {
            List<SimpleTaskItemModel4WS> processList = new List<SimpleTaskItemModel4WS>();
            DataTable dt = new DataTable();
            _processList = GetWorkProcessings();

            if (_processList != null)
            {
                foreach (SimpleTaskItemModel4WS smws in _processList)
                {
                    if (smws.levelID == "0")
                    {
                        processList.Add(smws);
                    }
                }

                dt = CreateDataTableStructrue();

                ChangeDataFormat(processList, ref dt, 0);
            }

            return dt;
        }

        /// <summary>
        /// Set value for process history list
        /// </summary>
        /// <param name="procssingList">process list data table.</param>
        /// <param name="dtHistoryList">history list data table</param>
        public void SetProcessingHistoryList(DataTable procssingList, ref DataTable dtHistoryList)
        {
            foreach (DataRow drProcess in procssingList.Rows)
            {
                TaskItemModel4WS[] tws = drProcess["HistoryItem"] as TaskItemModel4WS[];

                if (tws != null)
                {
                    foreach (TaskItemModel4WS tm in tws)
                    {
                        DataRow dr = dtHistoryList.NewRow();
                        dr["ParentID"] = drProcess["ID"];
                        dr["TaskName"] = drProcess["TaskName"];
                        dr["IsDisplayEmail"] = tm.asgnEmailDisp;
                        dr["ActionTime"] = tm.dispositionDateString;
                        dr["Action"] = I18nStringUtil.GetString(tm.resDisposition, tm.disposition);
                        dr["ProcessCode"] = drProcess["ProcessCode"];
                        dr["Comment"] = I18nStringUtil.GetString(tm.resDispositionComment, tm.dispositionComment);
                        dr["IsComplete"] = drProcess["IsComplete"];
                        dr["IsActive"] = drProcess["IsActive"];
                        dr["IsRestrictView"] = tm.isRestrictView4ACA;
                        dr["UserRolePrivilege"] = tm.userRolePrivilegeModel;
                        dr["AdditionalInfoItem"] = drProcess["AdditionalInfoItem"];
                        dr["DueDate"] = tm.dueDateString;

                        if (tm.assignedUser == null)
                        {
                            dr["AssignEmail"] = string.Empty;
                            dr["UserName"] = string.Empty;
                        }
                        else
                        {
                            dr["AssignEmail"] = tm.assignedUser.email;
                            dr["UserName"] = GetUserName(tm.assignedUser);
                        }

                        if (tm.sysUser == null)
                        {
                            dr["ActionEmail"] = string.Empty;
                            dr["ActionBy"] = string.Empty;
                        }
                        else
                        {
                            dr["ActionEmail"] = tm.sysUser.email;
                            dr["ActionBy"] = GetUserName(tm.sysUser);
                        }

                        dtHistoryList.Rows.Add(dr);
                    }
                }
            }
        }

        /// <summary>
        /// get the user's initial or the user's name. if the biz domain is true, return the user's initial,
        /// otherwise return the user's initial or the user's name by the option the user selected.
        /// </summary>
        /// <param name="user">SysUserModel4WS object</param>
        /// <returns>string user name</returns>
        private static string GetUserName(SysUserModel4WS user)
        {
            if (user == null)
            {
                return string.Empty;
            }

            // if enable to display user initial name, which means all users will display the initial name.
            if (StandardChoiceUtil.IsDisplayUserInitial())
            {
                return I18nStringUtil.GetString(user.resInitial, user.initial, user.resFullName, user.fullName);
            }
            else
            {
                if (user.isDisplayInitial)
                {
                    return I18nStringUtil.GetString(user.resInitial, user.initial, user.resFullName, user.fullName);
                }
                else
                {
                    return I18nStringUtil.GetString(user.resFullName, user.fullName);
                }
            }
        }

        /// <summary>
        /// Build data to data table
        /// </summary>
        /// <param name="fromDataFormat">from data format </param>
        /// <param name="toDataFormat">to data format</param>
        /// <param name="parentID">the parent id</param>
        private void ChangeDataFormat(List<SimpleTaskItemModel4WS> fromDataFormat, ref DataTable toDataFormat, int parentID)
        {
            foreach (SimpleTaskItemModel4WS smws in fromDataFormat)
            {
                for (int i = 0; i < _processList.Length; i++)
                {
                    DataRow dr = toDataFormat.NewRow();

                    if (smws == _processList[i])
                    {
                        dr["ParentID"] = parentID;
                        dr["TaskName"] = I18nStringUtil.GetString(smws.resProDes, smws.proDes);
                        dr["UserName"] = smws.ga_fname;
                        dr["ActionTime"] = smws.g6StatDd;
                        dr["Action"] = I18nStringUtil.GetString(smws.resSdAppDes, smws.sdAppDes);
                        dr["ProcessCode"] = smws.r1ProcessCode;
                        dr["Comment"] = I18nStringUtil.GetString(smws.resPTaskName, smws.PTaskName);
                        dr["IsComplete"] = smws.sdChk2 == ACAConstant.COMMON_Y ? true : false;
                        dr["IsActive"] = smws.sdChk1 == ACAConstant.COMMON_Y ? true : false;
                        dr["IsRestrictView"] = smws.isRestrictView4ACA;
                        dr["UserRolePrivilege"] = smws.userRolePrivilegeModel;
                        dr["HistoryItem"] = smws.historyTaskItems;
                        dr["AdditionalInfoItem"] = smws.taskSpecItems;
                        dr["ActionStatusFlag"] = smws.refActStatusFlag;

                        toDataFormat.Rows.Add(dr);

                        List<SimpleTaskItemModel4WS> tempSMWS;
                        tempSMWS = GetSpecialLayerData(int.Parse(smws.levelID) + 1, i);

                        if (tempSMWS.Count > 0)
                        {
                            ChangeDataFormat(tempSMWS, ref toDataFormat, int.Parse(dr["ID"].ToString()));
                        }

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Create data table structure
        /// </summary>
        /// <returns>data table.</returns>
        private DataTable CreateDataTableStructrue()
        {
            DataTable table = new DataTable("ProcessTable");
            DataColumn column;

            // Create first column and add to the DataTable.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ID";
            column.AutoIncrement = true;
            column.Caption = "ID";
            column.ReadOnly = true;
            column.Unique = true;
            column.AutoIncrementSeed = 1;
            column.AutoIncrementStep = 1;

            // Add the column to the DataColumnCollection.
            table.Columns.Add(column);

            // Craete 2th column: ParentID.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ParentID";
            column.AutoIncrement = false;
            column.Caption = "ParentID";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 3th column: TaskName.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "TaskName";
            column.AutoIncrement = false;
            column.Caption = "TaskName";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 4th column: Action.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Action";
            column.AutoIncrement = false;
            column.Caption = "Actions";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 5th column: UserName.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "UserName";
            column.AutoIncrement = false;
            column.Caption = "UserName";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 6th column: AssignEmail.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "AssignEmail";
            column.AutoIncrement = false;
            column.Caption = "AssignEmail";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 7th column: IsDisplayEmail.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "IsDisplayEmail";
            column.AutoIncrement = false;
            column.Caption = "IsDisplayEmail";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 8th column: DueDate.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "DueDate";
            column.AutoIncrement = false;
            column.Caption = "DueDate";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 9th column: ActionBy.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ActionBy";
            column.AutoIncrement = false;
            column.Caption = "ActionBy";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 10th column: ActionEmail.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ActionEmail";
            column.AutoIncrement = false;
            column.Caption = "ActionEmail";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 11th column: ActionTime.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ActionTime";
            column.AutoIncrement = false;
            column.Caption = "ActionTime";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 12th column: ProcessCode.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ProcessCode";
            column.AutoIncrement = false;
            column.Caption = "ProcessCode";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 13th column: IsComplete.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Boolean");
            column.ColumnName = "IsComplete";
            column.AutoIncrement = false;
            column.Caption = "IsComplete";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 14th column: IsActive.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Boolean");
            column.ColumnName = "IsActive";
            column.AutoIncrement = false;
            column.Caption = "IsActive";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 15th column: Comment.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Comment";
            column.AutoIncrement = false;
            column.Caption = "Comment";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 16th column: HistoryItem.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Object");
            column.ColumnName = "HistoryItem";
            column.AutoIncrement = false;
            column.Caption = "HistoryItem";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 17th column: IsRestrictView.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "IsRestrictView";
            column.AutoIncrement = false;
            column.Caption = "IsRestrictView";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 18th column: UserRolePrivilege.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Object");
            column.ColumnName = "UserRolePrivilege";
            column.AutoIncrement = false;
            column.Caption = "UserRolePrivilege";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 19th column: AdditionalInfoItem.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Object");
            column.ColumnName = "AdditionalInfoItem";
            column.AutoIncrement = false;
            column.Caption = "AdditionalInfoItem";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Craete 20th column: ActionStatusFlag.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ActionStatusFlag";
            column.AutoIncrement = false;
            column.Caption = "ActionStatusFlag";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            return table;
        }

        /// <summary>
        /// Get work processing array
        /// </summary>
        /// <returns>SimpleTaskItemModel4WS array</returns>
        private SimpleTaskItemModel4WS[] GetWorkProcessings()
        {
            IWorkflowBll workflowBll = ObjectFactory.GetObject<IWorkflowBll>();
            SimpleTaskItemModel4WS[] processList = workflowBll.GetSubProcessesByPK(_capModel.capID, null, null);

            return processList;
        }

        /// <summary>
        /// get special layer data and change data format to IList
        /// </summary>
        /// <param name="levelID">layer count</param>
        /// <param name="postionNumber">position number</param>
        /// <returns>List of SimpleTaskItemModel4WS</returns>
        private List<SimpleTaskItemModel4WS> GetSpecialLayerData(int levelID, int postionNumber)
        {
            List<SimpleTaskItemModel4WS> simps = new List<SimpleTaskItemModel4WS>();

            for (int i = postionNumber + 1; i < _processList.Length; i++)
            {
                if (levelID == int.Parse(_processList[i].levelID))
                {
                    simps.Add(_processList[i]);
                }
                else if (levelID > int.Parse(_processList[i].levelID))
                {
                    break;
                }
            }

            return simps;
        }

        #endregion Methods
    }
}
