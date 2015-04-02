#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapService.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapService.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using System.Linq;
using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.ComponentService.Model;
using Accela.ACA.CustomizeAPI;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.ComponentService.Service
{
    /// <summary>
    /// This class provide the ability to operate cap
    /// </summary>
    public class CapService : BaseService
    {
        /// <summary>
        /// Cap Business Class
        /// </summary>
        private ICapBll capBll = ObjectFactory.GetObject<ICapBll>();

        /// <summary>
        /// Initializes a new instance of the CapService class.
        /// </summary>
        /// <param name="context">User Context</param>
        public CapService(UserContext context) : base(context)
        {        
        }

        /// <summary>
        /// Get My Permit List
        /// </summary>
        /// <param name="moduleName">module Name</param>
        /// <returns>the SimpleCapWrapperModel list</returns>
        public List<SimpleCapWrapperModel> GetMyRecords(string moduleName)
        {
            var capWrapperModelList = new List<SimpleCapWrapperModel>(); 

            // Step 1, Get My Records
            var capModel = new CapModel4WS();
            capModel.moduleName = moduleName;
            SearchResultModel searchResultModel = capBll.GetMyCapList4ACA(AgencyCode, capModel, null, UserSeqNum, null, null);

            // Step 2, Wrapper Simple Cap Model
            if (searchResultModel != null
                && searchResultModel.resultList != null)
            {
                capWrapperModelList.AddRange(searchResultModel.resultList.OfType<SimpleCapModel>().Select(simpleCap => new SimpleCapWrapperModel(simpleCap)));
            }

            return capWrapperModelList;
        }

        /// <summary>
        /// Get Cap by Alt Id
        /// </summary>
        /// <param name="capAltID">this is the Alt Id</param>
        /// <param name="agencyCode"> agency Code</param>
        /// <returns>Wrapper model of the Cap</returns>
        public CapWrapperModel GetCapByAltID(string capAltID, string agencyCode)
        {
            CapModel4WS capModel = new CapModel4WS();
            CapIDModel4WS capIdModel = new CapIDModel4WS();
            CapWrapperModel wrapper = null;

            capIdModel = capBll.GetCapIDByAltID(agencyCode, capAltID);

            if (capIdModel != null)
            {
                CapWithConditionModel4WS capModelWithCondition = capBll.GetCapViewBySingle(capIdModel, AppSession.User.UserSeqNum, ACAConstant.COMMON_N, false);

                if (capModelWithCondition != null && capModelWithCondition.capModel != null)
                {
                    capModel = capModelWithCondition.capModel;   
                    wrapper = new CapWrapperModel(capModel);
                }
            }

            return wrapper;
        }
    }
}
