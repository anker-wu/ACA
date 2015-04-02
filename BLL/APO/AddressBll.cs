#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AddressBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AddressBll.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/18/2007    Sawyer.Liu    Initial.
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.BLL.APO
{
    /// <summary>
    /// This class provide the ability to operation address.
    /// </summary>
    public class AddressBll : BaseBll, IAddressBll
    {
        #region Fields

        /// <summary>
        /// Logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(AddressBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of AddressService.
        /// </summary>
        private AddressWebServiceService AddressService
        {
            get
            {
                return WSFactory.Instance.GetWebService<AddressWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Construct a new DataTable for Address.
        /// </summary>
        /// <returns>A DataTable that contains 2 columns, AddressId and assembled address string.</returns>
        public static DataTable ConstructAddressDataTable()
        {
            DataTable addressTable = new DataTable();

            addressTable.Columns.Add("RowIndex");
            addressTable.Columns.Add("Address");
            addressTable.Columns.Add("AddressModel", typeof(AddressModel));
            addressTable.Columns.Add("AgencyCode");

            return addressTable;
        }

        /// <summary>
        /// Generate Address string from a specified AddressModel model.
        /// </summary>
        /// <param name="addressModel">a AddressModel model.</param>
        /// <returns>an assembled address string</returns>
        public string GenerateAddressString(AddressModel addressModel)
        {
            IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
            return addressBuilderBll.BuildAddressByFormatType(addressModel, null, AddressFormatType.LONG_ADDRESS_NO_FORMAT);
        }

        /// <summary>
        /// Gets Address info by the address info.
        /// </summary>
        /// <param name="capIdModel4ws">the cap id model</param>
        /// <param name="refAddressModel">the ref-address model contain search criteria</param>
        /// <param name="format">the query format</param>
        /// <returns>daily address list</returns>
        /// <exception cref="DataValidateException">{ <c>capIdModel4ws, refAddressModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public DataTable GetDailyAddressesByRefAddressModel(CapIDModel4WS capIdModel4ws, RefAddressModel refAddressModel, QueryFormat format)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AddressBll.GetDailyAddressesByRefAddressModel()");
            }

            if (capIdModel4ws == null || refAddressModel == null)
            {
                throw new DataValidateException(new string[] { "capIdModel4ws", "refAddressModel" });
            }

            try
            {
                // Don't change the refAddressModel, its value will be used later, for example Browser Back action.
                RefAddressModel refAddressModelClone = ObjectCloneUtil.DeepCopy<RefAddressModel>(refAddressModel);

                if (refAddressModel.houseNumberStart != null)
                {
                    refAddressModelClone.houseNumberStartFrom = null;
                    refAddressModelClone.houseNumberStartTo = null;
                }

                if (refAddressModel.houseNumberEnd != null)
                {
                    refAddressModelClone.houseNumberEndFrom = null;
                    refAddressModelClone.houseNumberEndTo = null;
                }

                AddressModel[] address = AddressService.getDailyAddressesByRefAddressModel(capIdModel4ws, refAddressModelClone, User.PublicUserId, format);

                DataTable dtAddress = BuildAddressDataTable(address);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AddressBll.GetDailyAddressesByRefAddressModel()");
                }

                return dtAddress;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Populate the AddressModel list to a DataTable.
        /// </summary>
        /// <param name="addressModelList">An array of AddressModel model.</param>
        /// <returns>A DataTable of addresses.</returns>
        private DataTable BuildAddressDataTable(AddressModel[] addressModelList)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin RefAddressBll.BuildAddressDataTable()");
            }

            // create an empty datatable for address.
            DataTable addresses = ConstructAddressDataTable();

            int i = 0;
            if (addressModelList != null && addressModelList.Length > 0)
            {
                foreach (AddressModel address in addressModelList)
                {
                    //if aca not support super agency, we use old logic with AgencyCode configed by web.congif file
                    string agencyCode = AgencyCode;

                    if (!string.IsNullOrEmpty(address.serviceProviderCode))
                    {
                        agencyCode = address.serviceProviderCode;
                    }
                    else if (Logger.IsDebugEnabled)
                    {
                        Logger.Debug("address.serviceProviderCode is null when search cap by address");
                    }

                    addresses.Rows.Add(new object[] { i, GenerateAddressString(address), address, agencyCode });
                    i = i + 1;
                }
            }

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("End RefAddressBll.BuildAddressDataTable()");
            }

            return addresses;
        }

        #endregion Methods
    }
}
