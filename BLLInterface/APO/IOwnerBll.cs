#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File:  IOwnerBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IOwnerBll.cs 277008 2014-08-09 08:37:50Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/20/2008    Steven.lee    Initial version.
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.APO
{
    /// <summary>
    /// Provides owner related method for UI invoking.
    /// </summary>
    public interface IOwnerBll
    {
        #region Methods

        /// <summary>
        /// Gets owner by ownerNumber or ownerUID.
        /// if there is primary owner, get the primary owner to return.
        /// if there is no primary owner, get the first owner as default to return.
        /// </summary>
        /// <param name="owners">Owner List.</param>
        /// <param name="ownerNumber">Owner reference number.</param>
        /// <param name="ownerUID">Owner unique ID for supporting XAPO.</param>
        /// <returns>Primary owner.</returns>
        OwnerModel GetFilledOwner(OwnerModel[] owners, string ownerNumber, string ownerUID);

        /// <summary>
        /// Query address detail information.
        /// </summary>
        /// <remarks> 
        /// 1. This interface supports both internal APO and external APO.
        /// 2. Call getOwnerByPK method of OwnerService for getting OwnerModel model by owner's PK value.
        /// </remarks>
        /// <param name="agencyCode"> agency Code.</param>
        /// <param name="ownerPK"> OwnerModel with PK value.</param>
        /// <returns>OwnerModel model</returns>
        OwnerModel GetOwnerByPK(string agencyCode, OwnerModel ownerPK);

        /// <summary>
        /// Gets highest priority condition and notice conditions related with owner.
        /// This method support both internal APO and XAPO.
        /// </summary>
        /// <remarks>
        /// 1. Encapsulate Owner key value (include ownerNumber or ownerUID) as parameter.
        /// 2. Call OwnerService.getOwnerCondition to return.
        /// </remarks>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="sourceSeqNumber">source sequence Number</param>
        /// <param name="ownerNumber">Owner reference number. It can be null or empty when support XAPO</param>
        /// <param name="ownerUID">Owner unique ID for supporting XAPO. It has value when support XAPO</param>
        /// <returns>An OwnerModel object includes highlight and notice Conditions.</returns>
        OwnerModel GetOwnerCondition(string agencyCode, string sourceSeqNumber, string ownerNumber, string ownerUID);

        /// <summary>
        /// Gets highest priority condition and notice conditions related with owner.
        /// </summary>
        /// <remarks>
        /// 1. call OwnerWebService. getOwnerCondition to return.
        /// </remarks>
        /// <param name="agencyCodes">all selected agency code.</param>
        /// <param name="sourceSeqNumber">Source sequence number</param>
        /// <param name="ownerNumber">owner number</param>
        /// <param name="ownerUID">External owner unique id</param>
        /// <param name="duplicateAPOkeys"> Duplicated APO info for Super Agency special case</param>
        /// <returns>An OwnerModel object includes highlight and notice Conditions.</returns>
        OwnerModel GetOwnerCondition(string[] agencyCodes, string sourceSeqNumber, string ownerNumber, string ownerUID, DuplicatedAPOKeyModel[] duplicateAPOkeys);

        #endregion Methods
    }
}
