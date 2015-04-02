#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IBizDomainBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AdminPolicyBLL.cs 277080 2014-08-11 06:28:07Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// This class provide the ability to get policy value.
    /// </summary>
    public class AdminPolicyBLL : BaseBll, IPolicyBLL
    {
        #region Fields

        /// <summary>
        /// Logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(AdminPolicyBLL));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of PolicyService.
        /// </summary>
        private PolicyWebServiceService PolicyService
        {
            get
            {
                return WSFactory.Instance.GetWebService<PolicyWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get policy list by policy model.
        /// </summary>
        /// <param name="policyName">policy name</param>
        /// <param name="moduleName">module name</param>
        /// <returns>an list of XPolicyModel</returns>
        public IList<ItemValue> GetPolicyListForPayment(string policyName, string moduleName)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AdminPolicyBLL.GetPolicyListForPayment()");
            }

            IList<ItemValue> moduleItems = new List<ItemValue>();
            IList<ItemValue> agencyItems = new List<ItemValue>();
            IList<ItemValue> globalItems = new List<ItemValue>();
            IList<ItemValue> items = new List<ItemValue>();

            try
            {
                XPolicyModel[] models = PolicyService.getPolicyList(AgencyCode, policyName, null, ACAConstant.GRANTED_RIGHT, false, false);

                if (models != null)
                {
                    var enumModels = from s in models orderby s.data1 select s;
                    models = enumModels.ToArray();

                    foreach (XPolicyModel model in models)
                    {
                        if (string.Equals(model.levelData, moduleName, StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(moduleName))
                        {
                            ItemValue item = new ItemValue();
                            item.Key = I18nStringUtil.GetString(model.dispData2, model.data2);
                            item.Value = (model.recStatus == ACAConstant.VALID_STATUS ? string.Empty : "-") + model.data1 + "||" + model.data2;
                            moduleItems.Add(item);
                        }
                        else if (string.Equals(model.levelData, AgencyCode, StringComparison.CurrentCultureIgnoreCase))
                        {
                            ItemValue item = new ItemValue();
                            item.Key = I18nStringUtil.GetString(model.dispData2, model.data2);
                            item.Value = (model.recStatus == ACAConstant.VALID_STATUS ? string.Empty : "-") + model.data1 + "||" + model.data2;

                            agencyItems.Add(item);
                        }
                        else if (string.Equals(model.levelData, ACAConstant.STANDARDDATA, StringComparison.CurrentCultureIgnoreCase))
                        {
                            ItemValue item = new ItemValue();
                            item.Key = I18nStringUtil.GetString(model.dispData2, model.data2);
                            item.Value = (model.recStatus == ACAConstant.VALID_STATUS ? string.Empty : "-") + model.data1 + "||" + model.data2;

                            globalItems.Add(item);
                        }
                    }
                }

                if (moduleItems != null && moduleItems.Count > 0)
                {
                    items = moduleItems;
                }
                else if (agencyItems != null && agencyItems.Count > 0)
                {
                    items = agencyItems;
                }
                else
                {
                    items = globalItems;
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End PolicyBLL.GetPolicyListForPayment()");
                }

                return items;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get policy list by policy model.
        /// </summary>
        /// <param name="policyName">Policy name.</param>
        /// <param name="moduleName">The module name.</param>
        /// <returns>an list of XPolicyModel</returns>
        public IList<ItemValue> GetPolicyList(string policyName, string moduleName)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AdminPolicyBLL.GetPoliciesByModel()");
            }

            IList<ItemValue> items = new List<ItemValue>();

            try
            {
                XPolicyModel[] models = PolicyService.getPolicyList(AgencyCode, policyName, moduleName, ACAConstant.GRANTED_RIGHT, true, false);

                if (models != null)
                {
                    foreach (XPolicyModel model in models)
                    {
                        ItemValue item = new ItemValue();
                        item.Key = I18nStringUtil.GetString(model.dispData2, model.data2);
                        item.Value = (model.recStatus == ACAConstant.VALID_STATUS ? string.Empty : "-") + model.data1 + "||" + model.data2;

                        items.Add(item);
                    }
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End PolicyBLL.GetPoliciesByModel()");
                }

                return items;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}