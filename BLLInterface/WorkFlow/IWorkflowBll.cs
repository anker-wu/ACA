#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IWorkflowBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IWorkflowBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.BLL.WorkFlow
{
    /// <summary>
    /// There are behaviors defined about work flow.
    /// </summary>
    public interface IWorkflowBll
    {
        #region Methods

        /// <summary>
        /// Create a PageFlow group
        /// </summary>
        /// <param name="pageFlowGroupModel">the PageFlowGroupModel model</param>
        /// <param name="moduleName">The module this group belongs to</param>
        void CreatePageFlowGroup(PageFlowGroupModel pageFlowGroupModel, string moduleName);

        /// <summary>
        /// Edit a PageFlow group
        /// </summary>
        /// <param name="pageFlowGroupModel">the PageFlowGroupModel model</param>
        /// <param name="moduleName">The module this group belongs to</param>
        void EditePageFlowGroup(PageFlowGroupModel pageFlowGroupModel, string moduleName);

        /// <summary>
        /// Gets audited ad hoc task array by cap.
        /// </summary>
        /// <param name="capID">cap ID  number</param>
        /// <param name="qf">query format</param>
        /// <param name="callID">caller ID  number</param>
        /// <returns>Array Of TaskItemModel4WS</returns>
        TaskItemModel4WS[] GetADHocTaskAuditByCapID(CapIDModel4WS capID, QueryFormat4WS qf, string callID);

        /// <summary>
        /// Gets ad hoc task item array by cap.
        /// </summary>
        /// <param name="capID">cap ID  number</param>
        /// <param name="qf">query format</param>
        /// <param name="callID">caller ID  number</param>
        /// <returns>Array Of TaskItemModel4WS</returns>
        TaskItemModel4WS[] GetADHocTaskItemByCapID(CapIDModel4WS capID, QueryFormat4WS qf, string callID);

        /// <summary>
        /// Gets specific information of work flow task.
        /// </summary>
        /// <param name="capID">Cap ID  number</param>
        /// <param name="callID">caller ID  number</param>
        /// <param name="stepNum">Task step Number</param>
        /// <param name="procID">Task process ID</param>
        /// <returns>Array of TaskSpecificInfoModel4WS</returns>
        TaskSpecificInfoModel4WS[] GetAllTaskSpecInfo4Cap(CapIDModel4WS capID, string callID, int stepNum, long procID);

        /// <summary>
        /// Gets department name of workflow task. include action department and assign department.
        /// </summary>
        /// <param name="capID">Cap ID number</param>
        /// <param name="callID">caller ID number</param>
        /// <param name="stepNum">Task step Number</param>
        /// <param name="procID">Task process ID</param>
        /// <param name="isActionDept">is action department</param>
        /// <returns>Department Name</returns>
        string GetDepartmentName(CapIDModel4WS capID, string callID, int stepNum, long procID, bool isActionDept);

        /// <summary>
        /// Get a PageFlow group
        /// </summary>
        /// <param name="groupCode">The page flow code</param>
        /// <param name="moduleName">The module this group belongs to</param>
        /// <returns>a PageFlowGroupModel model</returns>
        PageFlowGroupModel GetPageFlowGroup(string groupCode, string moduleName);

        /// <summary>
        /// Get a string array which contains all smart choice names
        /// </summary>
        /// <param name="groupType">the page flow type</param>
        /// <returns>page flow group name list </returns>
        string[] GetPageFlowGroupNameList(string groupType);

        /// <summary>
        /// Gets process relation model array. 
        /// </summary>
        /// <param name="capID">cap ID  number</param>
        /// <param name="qf">query format</param>
        /// <param name="callID">caller ID  number</param>
        /// <returns>ProcessRelationModel4WS collections</returns>
        ProcessRelationModel4WS[] GetProcessRelationByCapID(CapIDModel4WS capID, QueryFormat4WS qf, string callID);

        /// <summary>
        /// Get all processing items
        /// </summary>
        /// <param name="capID"> Cap ID number</param>
        /// <param name="stepNumber">step number</param>
        /// <param name="parentProcessId">Parent process id</param>
        /// <returns>all processing items for special permit</returns>
        SimpleTaskItemModel4WS[] GetSubProcessesByPK(CapIDModel4WS capID, string stepNumber, string parentProcessId);

        /// <summary>
        /// Get process description list.
        /// </summary>
        /// <param name="capIDs"> Cap ID model list.</param>
        /// <returns>Process description list.</returns>
        string[] GetProcessesDesc(CapIDModel[] capIDs);

        /// <summary>
        /// Gets audited task array by cap.
        /// </summary>
        /// <param name="capID">cap ID number</param>
        /// <param name="qf">query format</param>
        /// <param name="callID">caller ID number</param>
        /// <returns>TaskItemModel4WS collection</returns>
        TaskItemModel4WS[] GetTaskAuditByCapID(CapIDModel4WS capID, QueryFormat4WS qf, string callID);

        /// <summary>
        /// Gets history workflow task detail.
        /// </summary>
        /// <param name="capID">Cap ID number</param>
        /// <param name="taskDes">Task Description</param>
        /// <param name="disposition">Task disposition</param>
        /// <param name="processID">process ID</param>
        /// <param name="auditDate">audit date</param>
        /// <returns>Single TaskItemModel4WS object</returns>
        TaskItemModel4WS GetTaskAuditByPK(CapIDModel4WS capID, string taskDes, string disposition, long processID, string auditDate);

        /// <summary>
        /// Gets history workflow task array.
        /// </summary>
        /// <param name="capID">CapIDModel4WS instance</param>
        /// /// <param name="format">Instance of QueryFormat4WS</param>
        /// <returns>TaskItemModel4WS collection</returns>
        TaskItemModel4WS[] GetTaskHistoryList(CapIDModel4WS capID, QueryFormat4WS format);

        /// <summary>
        /// Gets history workflow task array.
        /// </summary>
        /// <param name="capID">Cap ID number</param>
        /// <returns>TaskItemModel4WS collection</returns>
        TaskItemModel4WS[] GetTaskInspHistoryList(CapIDModel4WS capID);

        /// <summary>
        /// Gets task array by cap.
        /// </summary>
        /// <param name="capID">CAP ID number</param>
        /// <param name="qf">query format</param>
        /// <param name="callID">caller ID number</param>
        /// <returns>TaskItemModel4W collection</returns>
        TaskItemModel4WS[] GetTaskItemByCapID(CapIDModel4WS capID, QueryFormat4WS qf, string callID);

        /// <summary>
        /// Gets task item by process ID.
        /// </summary>
        /// <param name="capID">Cap ID number</param>
        /// <param name="callID">caller ID number</param>
        /// <param name="stepNum">Task step Number</param>
        /// <param name="procID">Task process ID</param>
        /// <returns>Single TaskItemModel4WS object</returns>
        TaskItemModel4WS GetTaskItemByPK(CapIDModel4WS capID, string callID, int stepNum, long procID);

        #endregion Methods
    }
}