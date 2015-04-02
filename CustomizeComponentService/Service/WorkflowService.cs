#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: WorkflowService.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: WorkflowService.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using System.Linq;

using Accela.ACA.BLL.WorkFlow;
using Accela.ACA.Common;
using Accela.ACA.ComponentService.Model;
using Accela.ACA.CustomizeAPI;
using Accela.ACA.WSProxy;

namespace Accela.ACA.ComponentService.Service
{
    /// <summary>
    /// This class provide the ability to operate workflow task
    /// </summary>
    public class WorkflowService : BaseService
    {       
        /// <summary>
        /// Initializes a new instance of the WorkflowService class.
        /// </summary>
        /// <param name="context">User Context</param>
        public WorkflowService(UserContext context) : base(context)
        {
        }

        /// <summary>
        /// Get work processing array
        /// </summary>
        /// <param name="paddingCaps">The padding cap model list.</param>
        /// <returns>SimpleTaskItemModel4WS array</returns>
        public string[] GetWorkflowTasks(List<SimpleCapWrapperModel> paddingCaps)
        {
            var capids = new List<CapIDModel>();

            if (paddingCaps != null && paddingCaps.Count > 0)
            {
                capids.AddRange(paddingCaps.Select(paddingCap => new CapIDModel
                    {
                        serviceProviderCode = paddingCap.AgenyCode, ID1 = paddingCap.CapID.ID1, ID2 = paddingCap.CapID.ID2, ID3 = paddingCap.CapID.ID3
                    }));
            }

            if (capids.Count > 0)
            {
                var workflowBll = ObjectFactory.GetObject<IWorkflowBll>();
                return workflowBll.GetProcessesDesc(capids.ToArray());
            }

            return null;
        }
    }
}
