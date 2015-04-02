#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IServiceProviderBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IServiceProviderBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// Defines methods for Service provider BLL.
    /// </summary>
    public interface IServiceProviderBll
    {
        #region Methods

        /// <summary>
        /// Gets ServiceProvider Model By PK
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>Service provider information.</returns>
        ServiceProviderModel GetServiceProviderByPK(string serviceProviderCode, string callerID);

        /// <summary>
        /// Gets the service provider by source sequence number.
        /// </summary>
        /// <param name="sourceSeqNumer">The source sequence number.</param>
        /// <returns>Service provider information.</returns>
        ServiceProviderModel[] GetServiceProviderBySourceSeqNumber(long sourceSeqNumer);

        /// <summary>
        /// Gets agency code list, 
        /// In super agency, return all sub agencies includes current agency.
        /// In normal agency, return single agency code.
        /// </summary>
        /// <param name="delegateUser">The delegate user model.</param>
        /// <returns>agency code list</returns>
        string[] GetSubAgencies(DelegateUserModel delegateUser);

        /// <summary>
        /// Gets agency code list, 
        /// In super agency, return all sub agencies don't includes current agency.
        /// In normal agency, return single agency code.
        /// </summary>
        /// <param name="callerID">Is for public user</param>
        /// <returns>agency code list</returns>
        string[] GetSubAgencies(string callerID);

        #endregion Methods
    }
}