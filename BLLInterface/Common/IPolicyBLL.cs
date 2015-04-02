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
 * $Id: IPolicyBLL.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;

using Accela.ACA.Common;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// An interface that defines all approach to get policy value.
    /// </summary>
    public interface IPolicyBLL
    {
        #region Methods

        /// <summary>
        /// Get policy list by policy model.
        /// </summary>
        /// <param name="policyName">policy name</param>
        /// <param name="moduleName">module name</param>
        /// <returns>an list of policy object</returns>
        IList<ItemValue> GetPolicyList(string policyName, string moduleName);

        /// <summary>
        /// Get policy list by policy model.
        /// </summary>
        /// <param name="policyName">policy name</param>
        /// <param name="moduleName">module name</param>
        /// <returns>an list of policy object</returns>
        IList<ItemValue> GetPolicyListForPayment(string policyName, string moduleName);

        #endregion Methods
    }
}