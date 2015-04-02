#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ParcelBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ParcelBll.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
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
    /// This class provide the ability to operation parcel.
    /// </summary>
    public class ParcelBll : BaseBll, IParcelBll
    {
        #region Fields

        /// <summary>
        /// Logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(ParcelBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of ParcelService.
        /// </summary>
        private ParcelWebServiceService ParcelService
        {
            get
            {
                return WSFactory.Instance.GetWebService<ParcelWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Query parcel detail information
        /// </summary>
        /// <param name="agencyCode">Agency Code.</param>
        /// <param name="parcelPK">ParcelModel with PK value.</param>
        /// <returns>a ParcelModel model</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1. This interface supports both internal APO and external APO.
        /// 2. Call getParcelByPK method of ParcelService for getting ParcelModel model by reference parcel's PK value.</remarks>
        ParcelModel IParcelBll.GetParcelByPK(string agencyCode, ParcelModel parcelPK)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin IParcelBll.GetParcelByPK()");
            }

            try
            {
                ParcelModel parcelModel = ParcelService.getParcelByPK(agencyCode, parcelPK);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End IParcelBll.GetParcelByPK()");
                }

                return parcelModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get TrustAccount Parcel list by Trust account number
        /// </summary>
        /// <param name="accountNbr">trust account number.</param>
        /// <returns>ParcelModel array</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        ParcelInfoModel[] IParcelBll.GetParcelListByTrustAccount(string accountNbr)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin IParcelBll.GetParcelListByTrustAccount()");
            }

            try
            {
                ParcelInfoModel[] parcelInfoModels = ParcelService.getParcelListByTrustAccount(AgencyCode, accountNbr);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End IParcelBll.GetParcelListByTrustAccount()");
                }

                return parcelInfoModels;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets highest priority condition and notice conditions related with parcel.
        /// </summary>
        /// <param name="servProvCodes">agency code</param>
        /// <param name="parcelInfo">ParcelInfoModel include addressId, addressUID, parcelNumber, parcelUID, ownerNumber, ownerUID.</param>
        /// <returns>ParcelModel with highlight and notice Conditions values.</returns>
        /// <exception cref="DataValidateException">{ <c>servProvCodes, parcelInfo, parcelInfo.parcelModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1. call  ParcelWebService.getParcelCondition to return.</remarks>
        ParcelModel IParcelBll.GetParcelCondition(string[] servProvCodes, ParcelInfoModel parcelInfo)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin ParcelBll.GetParcelCondition()");
            }

            if (servProvCodes == null || servProvCodes.Length == 0 || parcelInfo == null || parcelInfo.parcelModel == null)
            {
                throw new DataValidateException(new string[] { "servProvCodes", "parcelInfo", "parcelInfo.parcelModel" });
            }

            try
            {
                ParcelModel parcelModel = ParcelService.getParcelCondition(servProvCodes, parcelInfo, User.PublicUserId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End ParcelBll.GetParcelCondition()");
                }

                return parcelModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Query related parcel to the specified address.
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <param name="refAddressPK">Ref-AddressModel with PK value</param>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>A DataTable</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1. Call getParcelListByRefAddressID method of Parcel WebServices to get an array of ParcelModel model
        /// 2. call BuildParcelDataTable method to fill parcel data into the table</remarks>
        ParcelModel[] IParcelBll.GetParcelListByRefAddressPK(string agencyCode, RefAddressModel refAddressPK, QueryFormat queryFormat)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin APOBll.getParcelListByRefAddressId()");
            }

            try
            {
                // Don't change the refAddressPK, its value will be used later, for example Browser Back action.
                RefAddressModel refAddressModelClone = ObjectCloneUtil.DeepCopy(refAddressPK);

                if (refAddressPK.houseNumberStart != null)
                {
                    refAddressModelClone.houseNumberStartFrom = null;
                    refAddressModelClone.houseNumberStartTo = null;
                }

                if (refAddressPK.houseNumberEnd != null)
                {
                    refAddressModelClone.houseNumberEndFrom = null;
                    refAddressModelClone.houseNumberEndTo = null;
                }

                ParcelModel[] parcelModels = ParcelService.getParcelListByRefAddressPK(agencyCode, refAddressModelClone, User.PublicUserId, queryFormat);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End APOBll.getParcelListByRefAddressId()");
                }

                return parcelModels;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Query related parcel information to the specified owner
        /// </summary>
        /// <param name="agencyCode">The Agency Code.</param>
        /// <param name="ownerPK">The OwnerModel with owner PK value</param>
        /// <param name="callerID">The public user id</param>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>a DataTable of parcel data. This table contains four columns: Parcel Number, Lot, Block, Subdivision</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1. call getParcelListByOwnerNBR method of ParcelWebService
        /// to return a list of ParcelModel model.
        /// 2. call BuildParcelDataTable() method to fill parcel information into a DataTable
        /// 3. return the converted DataTable.</remarks>
        ParcelModel[] IParcelBll.GetParcelListByOwnerPK(string agencyCode, OwnerModel ownerPK, string callerID, QueryFormat queryFormat)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin APOBll.GetParcelListByOwnerPK()");
            }

            try
            {
                ParcelModel[] parcelModels = ParcelService.getParcelListByOwnerPK(agencyCode, ownerPK, callerID, queryFormat);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End APOBll.GetParcelListByOwnerPK()");
                }

                return parcelModels;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets Parcel Genealogy.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="parcelNumber">the parcel Number.</param>
        /// <returns>GenealogyTransactionModel model list.</returns>
        /// <exception cref="DataValidateException">{ <c>agencyCode, parcelNumber</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        GenealogyTransactionModel[] IParcelBll.GetParcelGenealogy(string agencyCode, string parcelNumber)
        {
            if (string.IsNullOrEmpty(agencyCode) || parcelNumber == null)
            {
                throw new DataValidateException(new string[] { "agencyCode", "parcelNumber" });
            }

            try
            {
                return ParcelService.getParcelGenealogy(agencyCode, parcelNumber, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}
