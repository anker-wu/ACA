#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: WorkflowBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: WorkflowBll.cs 277355 2014-08-14 07:28:03Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/10/2007    troy.yang    Initial version.
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.WorkFlow
{
    /// <summary>
    /// This class provide the ability to operation work flow .
    /// </summary>
    public class WorkflowBll : BaseBll, IWorkflowBll
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(WorkflowBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of WorkflowService.
        /// </summary>
        private static WorkflowWebServiceService WorkflowService
        {
            get
            {
                return WSFactory.Instance.GetWebService<WorkflowWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of PageFlowConfigService.
        /// </summary>
        private static PageFlowConfigWebServiceService PageFlowConfigService
        {
            get
            {
                return WSFactory.Instance.GetWebService<PageFlowConfigWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create a PageFlow group
        /// </summary>
        /// <param name="pageFlowGroupModel">the PageFlowGroupModel model</param>
        /// <param name="moduleName">The module this group belongs to</param>
        public void CreatePageFlowGroup(PageFlowGroupModel pageFlowGroupModel, string moduleName)
        {
            try
            {
                pageFlowGroupModel.serviceProviderCode = AgencyCode;
                PageFlowConfigService.createPageFlowGroup(pageFlowGroupModel, moduleName, "ACA Admin"); //User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Edit a PageFlow group
        /// </summary>
        /// <param name="pageFlowGroupModel">the PageFlowGroupModel model</param>
        /// <param name="moduleName">The module this group belongs to</param>
        public void EditePageFlowGroup(PageFlowGroupModel pageFlowGroupModel, string moduleName)
        {
            try
            {
                pageFlowGroupModel.serviceProviderCode = AgencyCode;
                PageFlowConfigService.editePageFlowGroup(pageFlowGroupModel, moduleName, "ACA ADMIN"); //groupType, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets audited ad hoc task array by cap.
        /// </summary>
        /// <param name="capID">cap ID model</param>
        /// <param name="qf">query format</param>
        /// <param name="callID">the public user id.</param>
        /// <returns>Array Of TaskItemModel4WS</returns>
        public TaskItemModel4WS[] GetADHocTaskAuditByCapID(CapIDModel4WS capID, QueryFormat4WS qf, string callID)
        {
            try
            {
                return WorkflowService.getADHocTaskAuditByCapID(capID, qf, callID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets ad hoc task item array by cap.
        /// </summary>
        /// <param name="capID">cap ID model</param>
        /// <param name="qf">query format</param>
        /// <param name="callID">the public user id.</param>
        /// <returns>Array Of TaskItemModel4WS</returns>
        public TaskItemModel4WS[] GetADHocTaskItemByCapID(CapIDModel4WS capID, QueryFormat4WS qf, string callID)
        {
            try
            {
                return WorkflowService.getADHocTaskItemByCapID(capID, qf, callID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets specific information of work flow task.
        /// </summary>
        /// <param name="capID">Cap ID  number</param>
        /// <param name="callID">caller ID  number</param>
        /// <param name="stepNum">Task step Number</param>
        /// <param name="procID">Task process ID</param>
        /// <returns>Array of TaskSpecificInfoModel4WS</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public TaskSpecificInfoModel4WS[] GetAllTaskSpecInfo4Cap(CapIDModel4WS capID, string callID, int stepNum, long procID)
        {
            try
            {
                return WorkflowService.getAllTaskSpecInfo4Cap(capID, callID, stepNum, procID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets department name of workflow task. include action department and assign department.
        /// </summary>
        /// <param name="capID">Cap ID number</param>
        /// <param name="callID">caller ID number</param>
        /// <param name="stepNum">Task step Number</param>
        /// <param name="procID">Task process ID</param>
        /// <param name="isActionDept">is action department</param>
        /// <returns>Department Name</returns>
        public string GetDepartmentName(CapIDModel4WS capID, string callID, int stepNum, long procID, bool isActionDept)
        {
            try
            {
                return WorkflowService.getDepartmentName(capID, callID, stepNum, procID, isActionDept);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get a PageFlow group
        /// </summary>
        /// <param name="moduleName">The module this group belongs to</param>
        /// <param name="groupCode">The page flow type</param>
        /// <returns>a PageFlowGroupModel model</returns>
        public PageFlowGroupModel GetPageFlowGroup(string moduleName, string groupCode)
        {
            try
            {
                return PageFlowConfigService.getPageFlowGroup(AgencyCode, moduleName, groupCode, "ACA ADMIN"); //groupType, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get a string array which contains all smart choice names
        /// </summary>
        /// <param name="groupType">the page flow type</param>
        /// <returns>page flow group name list</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string[] GetPageFlowGroupNameList(string groupType)
        {
            try
            {
                return PageFlowConfigService.getPageFlowGroupNameList(AgencyCode, groupType);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets process relation model array. 
        /// </summary>
        /// <param name="capID">cap ID  number</param>
        /// <param name="qf">query format</param>
        /// <param name="callID">caller ID  number</param>
        /// <returns>ProcessRelationModel4WS collections</returns>
        public ProcessRelationModel4WS[] GetProcessRelationByCapID(CapIDModel4WS capID, QueryFormat4WS qf, string callID)
        {
            try
            {
                return WorkflowService.getProcessRelationByCapID(capID, qf, callID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get all processing items
        /// </summary>
        /// <param name="capID"> Cap ID model</param>
        /// <param name="stepNumber">step number</param>
        /// <param name="parentProcessId">Parent process id</param>
        /// <returns>all processing items for special permit</returns>
        public SimpleTaskItemModel4WS[] GetSubProcessesByPK(CapIDModel4WS capID, string stepNumber, string parentProcessId)
        {
            try
            {
                return WorkflowService.getSubProcessesByPK(capID, stepNumber, parentProcessId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get process description list.
        /// </summary>
        /// <param name="capIDs">Cap ID model list.</param>
        /// <returns>Process description list.</returns>
        public string[] GetProcessesDesc(CapIDModel[] capIDs)
        {
            return WorkflowService.getProcessesDesc(capIDs, AgencyCode);
        }

        /// <summary>
        /// Gets audited task array by cap.
        /// </summary>
        /// <param name="capID">cap ID number</param>
        /// <param name="qf">query format</param>
        /// <param name="callID">caller ID number</param>
        /// <returns>TaskItemModel4WS collection</returns>
        public TaskItemModel4WS[] GetTaskAuditByCapID(CapIDModel4WS capID, QueryFormat4WS qf, string callID)
        {
            try
            {
                return WorkflowService.getTaskAuditByCapID(capID, qf, callID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets history workflow task detail.
        /// </summary>
        /// <param name="capID">Cap ID number</param>
        /// <param name="taskDes">Task Description</param>
        /// <param name="disposition">Task disposition</param>
        /// <param name="processID">process ID</param>
        /// <param name="auditDate">audit date</param>
        /// <returns>Single TaskItemModel4WS object</returns>
        public TaskItemModel4WS GetTaskAuditByPK(CapIDModel4WS capID, string taskDes, string disposition, long processID, string auditDate)
        {
            try
            {
                return WorkflowService.getTaskAuditByPK(capID, taskDes, disposition, processID, auditDate);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets history workflow task array.
        /// </summary>
        /// <param name="capID">CapIDModel4WS instance</param>
        /// /// <param name="format">Instance of QueryFormat4WS</param>
        /// <returns>TaskItemModel4WS collection</returns>
        public TaskItemModel4WS[] GetTaskHistoryList(CapIDModel4WS capID, QueryFormat4WS format)
        {
            try
            {
                return WorkflowService.getTaskHistoryList(capID, format);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets history workflow task array.
        /// </summary>
        /// <param name="capID">Cap ID number</param>
        /// <returns>TaskItemModel4WS collection</returns>
        public TaskItemModel4WS[] GetTaskInspHistoryList(CapIDModel4WS capID)
        {
            try
            {
                return WorkflowService.getTaskInspHistoryList(capID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets task array by cap.
        /// </summary>
        /// <param name="capID">CAP ID number</param>
        /// <param name="qf">query format</param>
        /// <param name="callID">caller ID number</param>
        /// <returns>TaskItemModel4W collection</returns>
        public TaskItemModel4WS[] GetTaskItemByCapID(CapIDModel4WS capID, QueryFormat4WS qf, string callID)
        {
            try
            {
                return WorkflowService.getTaskItemByCapID(capID, qf, callID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets task item by process ID.
        /// </summary>
        /// <param name="capID">Cap ID number</param>
        /// <param name="callID">caller ID number</param>
        /// <param name="stepNum">Task step Number</param>
        /// <param name="procID">Task process ID</param>
        /// <returns>Single TaskItemModel4WS object</returns>
        public TaskItemModel4WS GetTaskItemByPK(CapIDModel4WS capID, string callID, int stepNum, long procID)
        {
            try
            {
                return WorkflowService.getTaskItemByPK(capID, callID, stepNum, procID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}