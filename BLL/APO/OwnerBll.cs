#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: OwnerBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: OwnerBll.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.BLL.APO
{
    /// <summary>
    /// Provides owner related method for UI invoking.
    /// </summary>
    public class OwnerBll : BaseBll, IOwnerBll
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(OwnerBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of OwnerService.
        /// </summary>
        private OwnerWebServiceService OwnerService
        {
            get
            {
                return WSFactory.Instance.GetWebService<OwnerWebServiceService>();
            }
        }

        #endregion Properties

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
        OwnerModel IOwnerBll.GetFilledOwner(OwnerModel[] owners, string ownerNumber, string ownerUID)
        {
            if (owners == null || owners.Length == 0)
            {
                return null;
            }

            OwnerModel returnOwner = null;

            // if there is primary owner, get the primary owner to return.
            foreach (OwnerModel owner in owners)
            {
                // if it is external APO.
                if (string.IsNullOrEmpty(ownerNumber))
                {
                    if (owner.UID == ownerUID)
                    {
                        returnOwner = owner;
                        break;
                    }
                }
                else
                {
                    if (Convert.ToString(owner.ownerNumber) == ownerNumber)
                    {
                        returnOwner = owner;
                        break;
                    }
                }
            }

            return returnOwner;
        }

        /// <summary>
        /// Query address detail information.
        /// </summary>
        /// <param name="agencyCode">agency Code.</param>
        /// <param name="ownerPK">OwnerModel with PK value.</param>
        /// <returns>OwnerModel model</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1. This interface supports both internal APO and external APO.
        /// 2. Call getOwnerByPK method of OwnerService for getting OwnerModel model by owner's PK value.</remarks>
        OwnerModel IOwnerBll.GetOwnerByPK(string agencyCode, OwnerModel ownerPK)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin IOwnerBll.GetOwnerByPK()");
            }

            try
            {
                OwnerModel ownerModel = OwnerService.getOwnerByPK(agencyCode, ownerPK);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End IOwnerBll.GetOwnerByPK()");
                }

                return ownerModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets highest priority condition and notice conditions related with owner.
        /// This method support both internal APO and XAPO.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="sourceSeqNumber">source sequence Number</param>
        /// <param name="ownerNumber">Owner reference number. It can be null or empty when support XAPO</param>
        /// <param name="ownerUID">Owner unique ID for supporting XAPO. It has value when support XAPO</param>
        /// <returns>An OwnerModel object includes highlight and notice Conditions.</returns>
        /// <exception cref="DataValidateException">{ <c>agencyCode, sourceSeqNumber, ownerNumber and ownerUID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1. Encapsulate Owner key value (include ownerNumber or ownerUID) as parameter.
        /// 2. Call OwnerService.getOwnerCondition to return.</remarks>
        public OwnerModel GetOwnerCondition(string agencyCode, string sourceSeqNumber, string ownerNumber, string ownerUID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin OwnerlBll.GetOwnerCondition()");
            }

            if (string.IsNullOrEmpty(agencyCode) || string.IsNullOrEmpty(sourceSeqNumber)
                || (string.IsNullOrEmpty(ownerNumber) && string.IsNullOrEmpty(ownerUID)))
            {
                throw new DataValidateException(new string[] { "agencyCode", "sourceSeqNumber", "ownerNumber and ownerUID" });
            }

            try
            {
                OwnerModel ownerPK = new OwnerModel();
                ownerPK.ownerNumber = StringUtil.ToLong(ownerNumber);
                ownerPK.UID = ownerUID;
                ownerPK.sourceSeqNumber = StringUtil.ToLong(sourceSeqNumber);
                string[] agencies = new string[] { agencyCode };

                OwnerModel ownerModel = OwnerService.getOwnerCondition(agencies, ownerPK, User.PublicUserId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End OwnerlBll.GetOwnerCondition()");
                }

                return ownerModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets highest priority condition and notice conditions related with owner.
        /// </summary>
        /// <param name="agencyCodes">all selected agency code.</param>
        /// <param name="sourceSeqNumber">Source sequence number</param>
        /// <param name="ownerNumber">owner number</param>
        /// <param name="ownerUID">External owner unique id</param>
        /// <param name="duplicateAPOkeys">Duplicated APO info for Super Agency special case</param>
        /// <returns>An OwnerModel object includes highlight and notice Conditions.</returns>
        /// <exception cref="DataValidateException">{ <c>agencyCode, ownerNumber and ownerUID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1. call OwnerWebService. getOwnerCondition to return.</remarks>
        public OwnerModel GetOwnerCondition(string[] agencyCodes, string sourceSeqNumber, string ownerNumber, string ownerUID, DuplicatedAPOKeyModel[] duplicateAPOkeys)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin OwnerlBll.GetOwnerCondition()");
            }

            if (string.IsNullOrEmpty(ownerNumber) && string.IsNullOrEmpty(ownerUID))
            {
                throw new DataValidateException(new string[] { "agencyCode", "ownerNumber and ownerUID" });
            }

            try
            {
                OwnerModel ownerPK = new OwnerModel();
                ownerPK.ownerNumber = StringUtil.ToLong(ownerNumber);
                ownerPK.UID = ownerUID;
                ownerPK.sourceSeqNumber = StringUtil.ToLong(sourceSeqNumber);
                ownerPK.duplicatedAPOKeys = duplicateAPOkeys; // It's for special case duplicated APO info in Super Agency.

                OwnerModel ownerModel = OwnerService.getOwnerCondition(agencyCodes, ownerPK, User.UserID);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End OwnerlBll.GetOwnerCondition()");
                }

                return ownerModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}
