#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FeeService.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: FeeService.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.BLL.Finance;
using Accela.ACA.Common;
using Accela.ACA.CustomizeAPI;
using Accela.ACA.WSProxy;

namespace Accela.ACA.ComponentService.Service
{
    /// <summary>
    /// This class provide the ability to operate fee
    /// </summary>
    public class FeeService : BaseService
    {
        /// <summary>
        /// Fee Business Class
        /// </summary>
        private IFeeBll feeBll = ObjectFactory.GetObject<IFeeBll>(); 

        /// <summary>
        /// Initializes a new instance of the FeeService class.
        /// </summary>
        /// <param name="context">User Context</param>
        public FeeService(UserContext context) : base(context)
        {
        }

        /// <summary>
        /// GetTotal Balance Amount
        /// </summary>
        /// <param name="agencyCode">agency Code</param>
        /// <param name="capId1">id1 in CapIDModel4WS</param>
        /// <param name="capId2">id2 in CapIDModel4WS</param>
        /// <param name="capId3">id3 in CapIDModel4WS</param>
        /// <returns>Total Balance Amount</returns>
        public double GetTotalBalanceAmount(string agencyCode, string capId1, string capId2, string capId3)
        {
            CapIDModel4WS capId = new CapIDModel4WS { serviceProviderCode = agencyCode, id1 = capId1, id2 = capId2, id3 = capId3 };

            return feeBll.GetTotalBalanceFee(capId, Context.CallerID);
        }
    }
}
