#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: APOUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: APOUtil.cs 130988 2014-07-15  16:26:01Z ACHIEVO\Blues Gao & Canon Wu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using System.Globalization;
using System.Linq;
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web
{
    /// <summary>
    /// APO utility
    /// </summary>
    public static class APOUtil
    {
        #region Properties

        /// <summary>
        /// Field names on parcel search.
        /// </summary>
        private static readonly string[] ParcelSearchFieldNames =
        {
            "parcelNumber",
            "sourceSeqNumber",
            "UID",
            "subdivision",
            "lot",
            "book",
            "page",
            "parcel",
            "parcelArea",
            "landValue",
            "improvedValue",
            "exemptValue",
            "legalDesc",
            "tract",
            "block",
            "primaryParcelFlagWithoutDealing",
            "township",
            "range",
            "section"
        };

        /// <summary>
        /// Field names on address search.
        /// </summary>
        private static readonly string[] RefAddressfieldNames =
        {
            "UID",
            "streetName",
            "streetSuffix",
            "houseNumberStart",
            "houseNumberEnd",
            "streetDirection",
            "unitStart",
            "unitEnd",
            "city",
            "county",
            "state",
            "zip",
            "countryCode",
            "addressDescription",
            "XCoordinator",
            "YCoordinator",
            "distance",
            "secondaryRoadNumber",
            "streetSuffixdirection",
            "sourceNumber",
            "sourceFlag",
            "addressTypeFlag",
            "unitType",
            "streetPrefix",
            "secondaryRoad",
            "inspectionDistrictPrefix",
            "inspectionDistrict",
            "neighborhoodPrefix",
            "neighborhood",
            "eventID",
            "refAddressId",
            "houseFractionStart",
            "houseFractionEnd",
            "auditID",
            "auditDate",
            "fullAddress",
            "primaryFlag",
            "subdivision",
            "lot",
            "addressType",
            "addressLine1",
            "addressLine2",
            "houseNumberAlphaStart",
            "houseNumberAlphaEnd",
            "levelPrefix",
            "levelNumberStart",
            "levelNumberEnd",
            "validateFlag",
            "houseNumberStartFrom",
            "houseNumberStartTo",
            "houseNumberEndFrom",
            "houseNumberEndTo"
        };

        /// <summary>
        /// Field names on owner search.
        /// </summary>
        private static readonly string[] RefOwnerSearchFieldNames =
        {
            "ownerFullName",
            "address1",
            "city",
            "state",
            "zip",
            "mailAddress1",
            "mailAddress2",
            "mailAddress3",
            "mailCity",
            "mailState",
            "mailZip",
            "ownerFirstName",
            "ownerLastName1",
            "country",
            "ownerTitle",
            "fax",
            "faxCountryCode",
            "phone",
            "phoneCountryCode",
            "email",
            "mailCountry"
        };

        #endregion Properties

        #region Methods

        /// <summary>
        /// Judge whether all the values of a parcel are empty or not.
        /// </summary>
        /// <param name="parcelModel">A ParcelModel</param>
        /// <returns>true-all the values of a parcel are empty, otherwise returns false.</returns>
        public static bool IsEmpty(ParcelModel parcelModel)
        {
            if (parcelModel == null)
            {
                return true;
            }

            bool isEmpty = ValidationUtil.IsAllEmpty(parcelModel, ParcelSearchFieldNames);

            if (isEmpty)
            {
                isEmpty = TemplateUtil.IsEmpty(parcelModel.templates);
            }

            return isEmpty;
        }

        /// <summary>
        /// Judge whether all the values of a address are empty or not.
        /// </summary>
        /// <param name="refAddressModel">A RefAddressModel</param>
        /// <returns>true-all the values of a address are empty, otherwise returns false.</returns>
        public static bool IsEmpty(RefAddressModel refAddressModel)
        {
            if (refAddressModel == null)
            {
                return true;
            }

            bool isEmpty = ValidationUtil.IsAllEmpty(refAddressModel, RefAddressfieldNames);

            if (isEmpty)
            {
                isEmpty = TemplateUtil.IsEmpty(refAddressModel.templates);
            }

            return isEmpty;
        }

        /// <summary>
        /// Judge whether all the values of an owner are empty or not.
        /// </summary>
        /// <param name="ownerModel">An OwnerModel</param>
        /// <returns>true-all the values of an owner are empty, otherwise returns false.</returns>
        public static bool IsEmpty(OwnerModel ownerModel)
        {
            if (ownerModel == null)
            {
                return true;
            }

            bool isEmpty = ValidationUtil.IsAllEmpty(ownerModel.ToRefOwnerModel(), RefOwnerSearchFieldNames);

            if (isEmpty)
            {
                isEmpty = TemplateUtil.IsEmpty(ownerModel.templates);
            }

            return isEmpty;
        }

        /// <summary>
        /// Check the selected owner has associated data or not.
        /// </summary>
        /// <param name="parcelInfo">Parcel Info Object Array</param>
        /// <returns>IsAssociatedData flag</returns>
        public static bool IsAssociatedParcelData(object[] parcelInfo)
        {
            if (parcelInfo == null || parcelInfo.Length == 0)
            {
                return false;
            }

            return parcelInfo.OfType<ParcelInfoModel>().Any(model => model.parcelModel != null);
        }

        /// <summary>
        /// Sets the ref address model with specified data row
        /// </summary>
        /// <param name="addressRow">Address Data Row</param>
        /// <param name="parcelNumber">The parcel number.</param>
        /// <returns>Ref address model</returns>
        public static RefAddressModel InitialRefAddressModel(DataRow addressRow, string parcelNumber = null)
        {
            RefAddressModel address = new RefAddressModel();

            if (!Convert.IsDBNull(addressRow["AddressID"]))
            {
                address.refAddressId = Convert.ToInt64(addressRow["AddressID"]);
            }

            if (!Convert.IsDBNull(addressRow["AddressSequenceNumber"]))
            {
                address.sourceNumber = Convert.ToInt32(addressRow["AddressSequenceNumber"]);
            }

            if (!Convert.IsDBNull(addressRow["HouseNumberStart"]))
            {
                address.houseNumberStart = Convert.ToInt32(addressRow["HouseNumberStart"]);
            }

            if (!Convert.IsDBNull(addressRow["houseNumberEnd"]))
            {
                address.houseNumberEnd = Convert.ToInt32(addressRow["houseNumberEnd"]);
            }

            if (!Convert.IsDBNull(addressRow["HouseFractionStart"]))
            {
                address.houseFractionStart = addressRow["HouseFractionStart"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["houseFractionEnd"]))
            {
                address.houseFractionEnd = addressRow["houseFractionEnd"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["StreetDirection"]))
            {
                address.streetDirection = addressRow["StreetDirection"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["StreetName"]))
            {
                address.streetName = addressRow["StreetName"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["StreetPrefix"]))
            {
                address.streetPrefix = addressRow["StreetPrefix"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["StreetSuffix"]))
            {
                address.streetSuffix = addressRow["StreetSuffix"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["UnitType"]))
            {
                address.unitType = addressRow["UnitType"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["UnitStart"]))
            {
                address.unitStart = addressRow["UnitStart"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["UnitEnd"]))
            {
                address.unitEnd = addressRow["UnitEnd"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["City"]))
            {
                address.city = addressRow["City"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["State"]))
            {
                address.state = addressRow["State"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["Country"]))
            {
                address.country = addressRow["Country"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["Zip"]))
            {
                address.zip = addressRow["Zip"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["PrimaryFlag"]))
            {
                address.primaryFlag = addressRow["PrimaryFlag"].ToString();
            }

            address.duplicatedAPOKeys = addressRow["AddressAPOKeys"] as DuplicatedAPOKeyModel[];

            if (!Convert.IsDBNull(addressRow["fullAddress0"]))
            {
                address.fullAddress = addressRow["fullAddress0"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["auditStatus"]))
            {
                address.auditStatus = addressRow["auditStatus"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["addressTypeFlag"]))
            {
                address.addressTypeFlag = addressRow["addressTypeFlag"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["streetSuffixdirection"]))
            {
                address.streetSuffixdirection = addressRow["streetSuffixdirection"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["addressDescription"]))
            {
                address.addressDescription = addressRow["addressDescription"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["distance"]))
            {
                if (!string.IsNullOrEmpty(addressRow["distance"].ToString()))
                {
                    address.distance = Convert.ToDouble(addressRow["distance"], CultureInfo.InvariantCulture.NumberFormat);
                }
            }

            if (!Convert.IsDBNull(addressRow["secondaryRoad"]))
            {
                address.secondaryRoad = addressRow["secondaryRoad"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["secondaryRoadNumber"]))
            {
                if (!string.IsNullOrEmpty(addressRow["secondaryRoadNumber"].ToString()))
                {
                    address.secondaryRoadNumber = Convert.ToInt32(addressRow["secondaryRoadNumber"]);
                }
            }

            if (!Convert.IsDBNull(addressRow["inspectionDistrictPrefix"]))
            {
                address.inspectionDistrictPrefix = addressRow["inspectionDistrictPrefix"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["inspectionDistrict"]))
            {
                address.inspectionDistrict = addressRow["inspectionDistrict"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["neighberhoodPrefix"]))
            {
                address.neighborhoodPrefix = addressRow["neighberhoodPrefix"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["neighborhood"]))
            {
                address.neighborhood = addressRow["neighborhood"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["xcoordinator"]))
            {
                if (!string.IsNullOrEmpty(addressRow["xcoordinator"].ToString()))
                {
                    address.XCoordinator = Convert.ToDouble(addressRow["xcoordinator"], CultureInfo.InvariantCulture.NumberFormat);
                }
            }

            if (!Convert.IsDBNull(addressRow["ycoordinator"]))
            {
                if (!string.IsNullOrEmpty(addressRow["ycoordinator"].ToString()))
                {
                    address.YCoordinator = Convert.ToDouble(addressRow["ycoordinator"], CultureInfo.InvariantCulture.NumberFormat);
                }
            }

            if (!Convert.IsDBNull(addressRow["AddressLine1"]))
            {
                address.addressLine1 = addressRow["AddressLine1"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["AddressLine2"]))
            {
                address.addressLine2 = addressRow["AddressLine2"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["CountryCode"]))
            {
                address.countryCode = addressRow["CountryCode"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["AddressUID"]))
            {
                address.UID = addressRow["AddressUID"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["LevelPrefix"]))
            {
                address.levelPrefix = addressRow["LevelPrefix"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["LevelNumberStart"]))
            {
                address.levelNumberStart = addressRow["LevelNumberStart"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["LevelNumberEnd"]))
            {
                address.levelNumberEnd = addressRow["LevelNumberEnd"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["HouseAlphaStart"]))
            {
                address.houseNumberAlphaStart = addressRow["HouseAlphaStart"].ToString();
            }

            if (!Convert.IsDBNull(addressRow["HouseAlphaEnd"]))
            {
                address.houseNumberAlphaEnd = addressRow["HouseAlphaEnd"].ToString();
            }

            if (!string.IsNullOrEmpty(parcelNumber))
            {
                address.parcelNumber = parcelNumber;
            }
  
            return address;
        }

        /// <summary>
        /// Get owner model from data row.
        /// </summary>
        /// <param name="dr">A DataRow</param>
        /// <returns>An OwnerModel</returns>
        public static OwnerModel GetOwnerModel(DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }

            OwnerModel owner = new OwnerModel();

            owner.ownerNumber = StringUtil.ToLong(Convert.ToString(dr["OwnerNumber"]));
            owner.UID = Convert.ToString(dr["OwnerUID"]);
            owner.sourceSeqNumber = StringUtil.ToLong(Convert.ToString(dr["OwnerSequenceNumber"]));
            owner.ownerFullName = Convert.ToString(dr["OwnerFullName"]);
            owner.mailAddress1 = Convert.ToString(dr["MailAddress1"]);
            owner.mailAddress2 = Convert.ToString(dr["MailAddress2"]);
            owner.mailAddress3 = Convert.ToString(dr["MailAddress3"]);
            owner.mailZip = Convert.ToString(dr["MailZip"]);
            owner.mailState = Convert.ToString(dr["MailState"]);
            owner.mailCountry = Convert.ToString(dr["MailCountry"]);
            owner.ownerTitle = Convert.ToString(dr["OwnerTitle"]);
            owner.mailCity = Convert.ToString(dr["MailCity"]);
            owner.fax = Convert.ToString(dr["Fax"]);
            owner.faxCountryCode = Convert.ToString(dr["FaxIDD"]);
            owner.phone = Convert.ToString(dr["Phone"]);
            owner.phoneCountryCode = Convert.ToString(dr["PhoneIDD"]);
            owner.email = Convert.ToString(dr["Email"]);
            owner.city = Convert.ToString(dr["City"]);
            owner.state = Convert.ToString(dr["State"]);
            owner.zip = Convert.ToString(dr["Zip"]);

            if (dr["OwnerAttributes"] != DBNull.Value && !string.IsNullOrEmpty(dr["OwnerAttributes"].ToString()))
            {
                owner.templates = dr["OwnerAttributes"] as TemplateAttributeModel[];
            }

            if (dr["OwnerAPOKeys"] != DBNull.Value && !string.IsNullOrEmpty(dr["OwnerAPOKeys"].ToString()))
            {
                owner.duplicatedAPOKeys = dr["OwnerAPOKeys"] as DuplicatedAPOKeyModel[];
            }

            return owner;
        }

        /// <summary>
        /// This method receives an array of ParcelInfoModel models and construct a data table which 
        /// contains parcel number,parcel sequence number in parcel model, owner name, owner sequence 
        /// number in OwnerModel model and houseNumberStart, 
        /// streetDirection,streetName,streetSuffix,city,state,zip,unitType,unitNo ,addressId field in
        /// RefAddressModel model. All the three models are members of the ParcelInfoModel model. 
        /// Table columns:
        /// refAddressIdField, houseNumberStartField, cityField, countryField, stateField, streetDirectionField, streetNameField, streetSuffixField, unitStartField, unitTypeField, zipField, sourceNumberField(address)
        /// parcelNumberField, sourceSequenceNumberField(parcel)
        /// ownerFullNameField, ownerNumberField, sourceSequenceNumberField(Owner)
        /// </summary>
        /// <param name="parcelInfo">an array of ParcelInfoModel models.</param>
        /// <param name="addressFormatType">address format type </param>
        /// <returns>a data table.</returns>
        public static DataTable BuildAPODataTable(ParcelInfoModel[] parcelInfo, AddressFormatType addressFormatType = AddressFormatType.SHORT_ADDRESS_NO_FORMAT)
        {
            DataTable table = CreateAPODataTable();

            if (parcelInfo == null || parcelInfo.Length == 0)
            {
                return table;
            }

            foreach (ParcelInfoModel model in parcelInfo)
            {
                if (model == null)
                {
                    continue;
                }

                DataRow dr = table.NewRow();

                BuildAPODataRow(model, dr, addressFormatType);

                table.Rows.Add(dr);
            }

            return table;
        }

        /// <summary>
        /// This method receives an array of ParcelInfoModel models and construct a data table which 
        /// contains parcel number,parcel sequence number in parcel model, owner name, owner sequence 
        /// number in OwnerModel model and houseNumberStart, 
        /// streetDirection,streetName,streetSuffix,city,state,zip,unitType,unitNo ,addressId field in
        /// RefAddressModel model. All the three models are members of the ParcelInfoModel model. 
        /// Table columns:
        /// refAddressIdField, houseNumberStartField, cityField, countryField, stateField, streetDirectionField, streetNameField, streetSuffixField, unitStartField, unitTypeField, zipField, sourceNumberField(address)
        /// parcelNumberField, sourceSequenceNumberField(parcel)
        /// ownerFullNameField, ownerNumberField, sourceSequenceNumberField(Owner)
        /// </summary>
        /// <param name="parcelInfo">an array of ParcelInfoModel models.</param>
        /// <param name="addressFormatType">address format type </param>
        /// <returns>a data table.</returns>
        public static DataTable BuildAPODataTable(object[] parcelInfo, AddressFormatType addressFormatType = AddressFormatType.SHORT_ADDRESS_NO_FORMAT)
        {
            DataTable table = CreateAPODataTable();

            if (parcelInfo == null || parcelInfo.Length == 0)
            {
                return table;
            }

            foreach (object objModel in parcelInfo)
            {
                ParcelInfoModel model = objModel as ParcelInfoModel;

                if (model == null)
                {
                    continue;
                }

                DataRow dr = table.NewRow();

                BuildAPODataRow(model, dr, addressFormatType);

                table.Rows.Add(dr);
            }

            return table;
        }

        /// <summary>
        /// Build Reference Address Data Table.
        /// </summary>
        /// <param name="refAddressInfo">An array of RefAddressModel models.</param>
        /// <param name="addressFormatType">Address Format</param>
        /// <returns>Reference Address Data Table</returns>
        public static DataTable BuildAddressDataTable(RefAddressModel[] refAddressInfo, AddressFormatType addressFormatType = AddressFormatType.SHORT_ADDRESS_NO_FORMAT)
        {
            DataTable dataTable = CreateAddressDataTable();

            if (refAddressInfo == null || refAddressInfo.Length == 0)
            {
                return dataTable;
            }

            for (int rowIndex = 0; rowIndex < refAddressInfo.Length; rowIndex++)
            {
                RefAddressModel addressModel = refAddressInfo[rowIndex];
                DataRow dataRow = dataTable.NewRow();

                dataRow = BuildAddressDataRow(addressModel, dataRow, addressFormatType, rowIndex);
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        /// <summary>
        /// Build Reference Address Data Table.
        /// </summary>
        /// <param name="refAddressInfo">An array of Object models.</param>
        /// <param name="addressFormatType">Address Format</param>
        /// <returns>Reference Address Data Table</returns>
        public static DataTable BuildAddressDataTable(object[] refAddressInfo, AddressFormatType addressFormatType = AddressFormatType.SHORT_ADDRESS_NO_FORMAT)
        {
            var dataTable = CreateAddressDataTable();

            if (refAddressInfo == null || refAddressInfo.Length == 0)
            {
                return dataTable;
            }

            for (int rowIndex = 0; rowIndex < refAddressInfo.Length; rowIndex++)
            {
                object info = refAddressInfo[rowIndex];
                RefAddressModel addressModel = info as RefAddressModel;
                DataRow dataRow = dataTable.NewRow();

                dataRow = BuildAddressDataRow(addressModel, dataRow, addressFormatType, rowIndex);
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        /// <summary>
        /// Build Reference Parcel Data Table.
        /// </summary>
        /// <param name="parcelInfo">Parcel Info Models</param>
        /// <returns>Reference Parcel Tata Table</returns>
        public static DataTable BuildParcelDataTable(object[] parcelInfo)
        {
            var dataTable = CreateParcelDataTable();

            if (parcelInfo == null || parcelInfo.Length == 0)
            {
                return dataTable;
            }

            for (int rowIndex = 0; rowIndex < parcelInfo.Length; rowIndex++)
            {
                object info = parcelInfo[rowIndex];
                ParcelModel parcelModel = info as ParcelModel;
                DataRow dataRow = dataTable.NewRow();

                dataRow = BuildParcelDataRow(dataRow, parcelModel, rowIndex);
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        /// <summary>
        /// This method receives a ParcelModel models and construct a data table 
        /// Table columns:
        /// parcelNumberField, sourceSequenceNumberField, blockField, lotField, subdivisionField
        /// </summary>
        /// <param name="parcels">an array ParcelModel models.</param>
        /// <returns>a data table.</returns>
        public static DataTable BuildParcelDataTable(ParcelModel[] parcels)
        {
            DataTable dataTable = CreateParcelDataTable();

            if (parcels == null || parcels.Length == 0)
            {
                return dataTable;
            }

            for (int rowIndex = 0; rowIndex < parcels.Length; rowIndex++)
            {
                ParcelModel parcel = parcels[rowIndex];
                DataRow dataRow = dataTable.NewRow();

                dataRow = BuildParcelDataRow(dataRow, parcel, rowIndex);
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        /// <summary>
        /// This method receives a OwnerModel models and construct a data table
        /// </summary>
        /// <param name="owners">an array OwnerModel models.</param>
        /// <returns>a data table.</returns>
        public static DataTable BuildOwnerDataTable(object[] owners)
        {
            DataTable table = new DataTable();

            table.Columns.Add("OwnerNumber");
            table.Columns.Add("OwnerUID");
            table.Columns.Add("OwnerSequenceNumber");
            table.Columns.Add("OwnerFullName");
            table.Columns.Add("MailAddress");
            table.Columns.Add("MailAddress1");
            table.Columns.Add("MailAddress2");
            table.Columns.Add("MailAddress3");
            table.Columns.Add("MailZip");
            table.Columns.Add("MailState");
            table.Columns.Add("MailCountry");
            table.Columns.Add("OwnerTitle");
            table.Columns.Add("MailCity");
            table.Columns.Add("Fax");
            table.Columns.Add("FaxIDD");
            table.Columns.Add("Phone");
            table.Columns.Add("PhoneIDD");
            table.Columns.Add("Email");
            table.Columns.Add("City");
            table.Columns.Add("State");
            table.Columns.Add("Zip");
            table.Columns.Add("OwnerAttributes", typeof(TemplateAttributeModel[]));
            table.Columns.Add("OwnerAPOKeys", typeof(DuplicatedAPOKeyModel[]));

            if (owners == null || owners.Length < 1)
            {
                return table;
            }

            foreach (object objectModel in owners)
            {
                DataRow dr = table.NewRow();
                OwnerModel owner = objectModel as OwnerModel;

                // reference owner number
                dr["OwnerNumber"] = owner.ownerNumber;

                // external owner UID
                dr["OwnerUID"] = owner.UID;
                dr["OwnerSequenceNumber"] = owner.sourceSeqNumber;
                dr["OwnerFullName"] = owner.ownerFullName;
                dr["MailAddress"] = owner.mailAddress;
                dr["MailAddress1"] = owner.mailAddress1;
                dr["MailAddress2"] = owner.mailAddress2;
                dr["MailAddress3"] = owner.mailAddress3;
                dr["MailZip"] = owner.mailZip;
                dr["MailState"] = owner.mailState;
                dr["MailCountry"] = owner.mailCountry;
                dr["OwnerTitle"] = owner.ownerTitle;
                dr["MailCity"] = owner.mailCity;
                dr["Fax"] = owner.fax;
                dr["FaxIDD"] = owner.faxCountryCode;
                dr["Phone"] = owner.phone;
                dr["PhoneIDD"] = owner.phoneCountryCode;
                dr["Email"] = owner.email;
                dr["City"] = owner.city;
                dr["State"] = owner.state;
                dr["Zip"] = owner.zip;

                if (owner.templates != null)
                {
                    dr["OwnerAttributes"] = owner.templates;
                }

                if (owner.duplicatedAPOKeys != null)
                {
                    dr["OwnerAPOKeys"] = owner.duplicatedAPOKeys;
                }

                table.Rows.Add(dr);
            }

            return table;
        }

        /// <summary>
        /// Create APO data table.
        /// </summary>
        /// <returns>The APO data table construction.</returns>
        private static DataTable CreateAPODataTable()
        {
            var table = new DataTable();

            table.Columns.Add("AddressID");
            table.Columns.Add("AddressSequenceNumber");
            table.Columns.Add("HouseNumberStart");
            table.Columns.Add("StreetDirection");
            table.Columns.Add("StreetName");
            table.Columns.Add("StreetSuffix");
            table.Columns.Add("UnitType");
            table.Columns.Add("UnitStart");
            table.Columns.Add("Country");
            table.Columns.Add("City");
            table.Columns.Add("State");
            table.Columns.Add("Zip");
            table.Columns.Add("ParcelNumber");
            table.Columns.Add("ParcelSequenceNumber");
            table.Columns.Add("LotOfParcel");
            table.Columns.Add("BlockOfParcel");
            table.Columns.Add("SubdivisionOfParcel");
            table.Columns.Add("BookOfParcel");
            table.Columns.Add("PageOfParcel");
            table.Columns.Add("TractOfParcel");
            table.Columns.Add("ParcelAreaOfParcel");
            table.Columns.Add("LegalDescOfparcel");
            table.Columns.Add("LandValueOfParcel");
            table.Columns.Add("ImprovedValueOfParcel");
            table.Columns.Add("ExceptionValueOfParcel");
            table.Columns.Add("OwnerNumber");
            table.Columns.Add("OwnerSequenceNumber");
            table.Columns.Add("OwnerFullName");
            table.Columns.Add("AddressOfOwner");
            table.Columns.Add("Address1OfOwner");
            table.Columns.Add("Address2OfOwner");
            table.Columns.Add("Address3OfOwner");
            table.Columns.Add("ZipOfOwner");
            table.Columns.Add("StateOfOwner");
            table.Columns.Add("CountryOfOwner");
            table.Columns.Add("FullAddress");
            table.Columns.Add("County");
            table.Columns.Add("streetPrefix");
            table.Columns.Add("houseNumberEnd");
            table.Columns.Add("unitEnd");
            table.Columns.Add("auditStatus");
            table.Columns.Add("houseFractionStart");
            table.Columns.Add("houseFractionEnd");
            table.Columns.Add("addressTypeFlag");
            table.Columns.Add("streetSuffixdirection");
            table.Columns.Add("addressDescription");
            table.Columns.Add("distance");
            table.Columns.Add("secondaryRoad");
            table.Columns.Add("secondaryRoadNumber");
            table.Columns.Add("inspectionDistrictPrefix");
            table.Columns.Add("inspectionDistrict");
            table.Columns.Add("neighberhoodPrefix");
            table.Columns.Add("neighborhood");
            table.Columns.Add("xcoordinator");
            table.Columns.Add("ycoordinator");
            table.Columns.Add("eventID");
            table.Columns.Add("auditID");
            table.Columns.Add("auditDate", typeof(DateTime));
            table.Columns.Add("sourceFlag");
            table.Columns.Add("fullAddress0");
            table.Columns.Add("primaryFlag");
            table.Columns.Add("subdivision");
            table.Columns.Add("addressType");
            table.Columns.Add("lot");
            table.Columns.Add("AddressLine1");
            table.Columns.Add("AddressLine2");
            table.Columns.Add("CountryCode");

            //Support External APO
            table.Columns.Add("AddressUID");
            table.Columns.Add("ParcelUID");
            table.Columns.Add("OwnerUID");
            table.Columns.Add("AddressAttributes", typeof(TemplateAttributeModel[]));
            table.Columns.Add("ParcelAttributes", typeof(TemplateAttributeModel[]));
            table.Columns.Add("OwnerAttributes", typeof(TemplateAttributeModel[]));

            table.Columns.Add("OwnerTitle");
            table.Columns.Add("CityOfOwner");
            table.Columns.Add("Fax");
            table.Columns.Add("FaxIDD");
            table.Columns.Add("Phone");
            table.Columns.Add("PhoneIDD");
            table.Columns.Add("Email");

            table.Columns.Add("RowIndex");
            table.Columns.Add("UnmaskedParcelNumber");
            table.Columns.Add("ParcelStatus");
            table.Columns.Add("ParcelAPOKeys", typeof(DuplicatedAPOKeyModel[]));
            table.Columns.Add("AddressAPOKeys", typeof(DuplicatedAPOKeyModel[]));
            table.Columns.Add("OwnerAPOKeys", typeof(DuplicatedAPOKeyModel[]));

            table.Columns.Add("LevelPrefix");
            table.Columns.Add("LevelNumberStart");
            table.Columns.Add("LevelNumberEnd");
            table.Columns.Add("HouseAlphaStart");
            table.Columns.Add("HouseAlphaEnd");

            return table;
        }

        /// <summary>
        /// Build APO data row.
        /// </summary>
        /// <param name="parcelInfo">A ParcelInfoModel</param>
        /// <param name="dr">A DataRow</param>
        /// <param name="addressFormatType">Address format type</param>
        private static void BuildAPODataRow(ParcelInfoModel parcelInfo, DataRow dr, AddressFormatType addressFormatType)
        {
            if (parcelInfo.RAddressModel != null)
            {
                var addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
                RefAddressModel addressModel = parcelInfo.RAddressModel;

                // reference address number
                dr["AddressID"] = addressModel.refAddressId;

                // external address UID.
                dr["AddressUID"] = addressModel.UID;
                dr["AddressSequenceNumber"] = addressModel.sourceNumber;
                dr["HouseNumberStart"] = addressModel.houseNumberStart;
                dr["StreetDirection"] = I18nStringUtil.GetString(addressModel.resStreetDirection, addressModel.streetDirection);
                dr["StreetName"] = addressModel.streetName;
                dr["StreetSuffix"] = I18nStringUtil.GetString(addressModel.resStreetSuffix, addressModel.streetSuffix);
                dr["UnitType"] = I18nStringUtil.GetString(addressModel.resUnitType, addressModel.unitType);
                dr["UnitStart"] = addressModel.unitStart;
                dr["Country"] = addressModel.country;
                dr["City"] = addressModel.city;
                dr["State"] = I18nStringUtil.GetString(addressModel.resState, addressModel.state);
                dr["Zip"] = addressModel.zip;
                dr["FullAddress"] = addressBuilderBll.BuildAddressByFormatType(addressModel, null, addressFormatType);
                dr["County"] = addressModel.county;

                dr["streetPrefix"] = addressModel.streetPrefix;
                dr["houseNumberEnd"] = addressModel.houseNumberEnd;
                dr["unitEnd"] = addressModel.unitEnd;
                dr["auditStatus"] = addressModel.auditStatus;
                dr["houseFractionStart"] = addressModel.houseFractionStart;
                dr["houseFractionEnd"] = addressModel.houseFractionEnd;
                dr["addressTypeFlag"] = addressModel.addressTypeFlag;
                dr["streetSuffixdirection"] = I18nStringUtil.GetString(addressModel.resStreetSuffixdirection, addressModel.streetSuffixdirection);
                dr["addressDescription"] = addressModel.addressDescription;
                dr["distance"] = addressModel.distance;
                dr["secondaryRoad"] = addressModel.secondaryRoad;
                dr["secondaryRoadNumber"] = addressModel.secondaryRoadNumber;
                dr["inspectionDistrictPrefix"] = addressModel.inspectionDistrictPrefix;
                dr["inspectionDistrict"] = addressModel.inspectionDistrict;
                dr["neighberhoodPrefix"] = addressModel.neighborhoodPrefix;
                dr["neighborhood"] = addressModel.neighborhood;
                dr["xcoordinator"] = addressModel.XCoordinator;
                dr["ycoordinator"] = addressModel.YCoordinator;
                dr["eventID"] = addressModel.eventID;
                dr["auditID"] = addressModel.auditID;
                dr["auditDate"] = addressModel.auditDate == null ? DBNull.Value : (object)addressModel.auditDate;
                dr["sourceFlag"] = addressModel.sourceFlag;
                dr["fullAddress0"] = addressModel.fullAddress;
                dr["primaryFlag"] = addressModel.primaryFlag;

                if (parcelInfo.parcelModel != null)
                {
                    addressModel.subdivision = parcelInfo.parcelModel.subdivision;
                }

                dr["subdivision"] = addressModel.subdivision;

                dr["addressType"] = addressModel.addressType;
                dr["lot"] = addressModel.lot;

                dr["AddressLine1"] = addressModel.addressLine1;
                dr["AddressLine2"] = addressModel.addressLine2;

                dr["LevelPrefix"] = addressModel.levelPrefix;
                dr["LevelNumberStart"] = addressModel.levelNumberStart;
                dr["LevelNumberEnd"] = addressModel.levelNumberEnd;
                dr["HouseAlphaStart"] = addressModel.houseNumberAlphaStart;
                dr["HouseAlphaEnd"] = addressModel.houseNumberAlphaEnd;

                dr["CountryCode"] = I18nStringUtil.GetString(addressModel.resCountryCode, addressModel.countryCode);

                if (addressModel.templates != null)
                {
                    dr["AddressAttributes"] = addressModel.templates;
                }

                if (addressModel.duplicatedAPOKeys != null)
                {
                    dr["AddressAPOKeys"] = addressModel.duplicatedAPOKeys;
                }
            }

            if (parcelInfo.parcelModel != null)
            {
                ParcelModel parcelModel = parcelInfo.parcelModel;

                // reference parcel number
                dr["ParcelNumber"] = parcelModel.parcelNumber;

                // external parcel UID
                dr["ParcelUID"] = parcelModel.UID;
                dr["ParcelSequenceNumber"] = parcelModel.sourceSeqNumber;
                dr["LotOfParcel"] = parcelModel.lot;
                dr["BlockOfParcel"] = parcelModel.block;
                dr["SubdivisionOfParcel"] = I18nStringUtil.GetString(parcelModel.resSubdivision, parcelModel.subdivision);
                dr["BookOfParcel"] = parcelModel.book;
                dr["PageOfParcel"] = parcelModel.page;
                dr["TractOfParcel"] = parcelModel.tract;
                dr["ParcelAreaOfParcel"] = parcelModel.parcelArea;
                dr["LegalDescOfparcel"] = parcelModel.legalDesc;
                dr["LandValueOfParcel"] = parcelModel.landValue;
                dr["ImprovedValueOfParcel"] = parcelModel.improvedValue;
                dr["ExceptionValueOfParcel"] = parcelModel.exemptValue;
                dr["UnmaskedParcelNumber"] = parcelModel.unmaskedParcelNumber;
                dr["ParcelStatus"] = parcelModel.parcelStatus;

                if (parcelModel.templates != null)
                {
                    dr["ParcelAttributes"] = parcelModel.templates;
                }

                if (parcelModel.duplicatedAPOKeys != null)
                {
                    dr["ParcelAPOKeys"] = parcelModel.duplicatedAPOKeys;
                }
            }

            if (parcelInfo.ownerModel != null)
            {
                OwnerModel ownerModel = parcelInfo.ownerModel;

                // reference owner number
                dr["OwnerNumber"] = ownerModel.ownerNumber;

                // external owner UID
                dr["OwnerUID"] = ownerModel.UID;
                dr["OwnerSequenceNumber"] = ownerModel.sourceSeqNumber;
                dr["OwnerFullName"] = ownerModel.ownerFullName;
                dr["AddressOfOwner"] = ownerModel.mailAddress;
                dr["Address1OfOwner"] = ownerModel.mailAddress1;
                dr["Address2OfOwner"] = ownerModel.mailAddress2;
                dr["Address3OfOwner"] = ownerModel.mailAddress3;
                dr["ZipOfOwner"] = ownerModel.mailZip;
                dr["StateOfOwner"] = ownerModel.mailState;
                dr["CountryOfOwner"] = ownerModel.mailCountry;
                dr["OwnerTitle"] = ownerModel.ownerTitle;
                dr["CityOfOwner"] = ownerModel.mailCity;
                dr["Fax"] = ownerModel.fax;
                dr["FaxIDD"] = ownerModel.faxCountryCode;
                dr["Phone"] = ownerModel.phone;
                dr["PhoneIDD"] = ownerModel.phoneCountryCode;
                dr["Email"] = ownerModel.email;

                if (ownerModel.templates != null)
                {
                    dr["OwnerAttributes"] = ownerModel.templates;
                }

                if (ownerModel.duplicatedAPOKeys != null)
                {
                    dr["OwnerAPOKeys"] = ownerModel.duplicatedAPOKeys;
                }
            }

            dr["RowIndex"] = parcelInfo.RowIndex;
        }

        /// <summary>
        /// Create Address Data Table.
        /// </summary>
        /// <returns>Address Data Table</returns>
        private static DataTable CreateAddressDataTable()
        {
            var dt = new DataTable();

            dt.Columns.Add("AddressID");
            dt.Columns.Add("AddressUID");
            dt.Columns.Add("AddressSequenceNumber");
            dt.Columns.Add("HouseNumberStart");
            dt.Columns.Add("StreetDirection");
            dt.Columns.Add("StreetName");
            dt.Columns.Add("StreetSuffix");
            dt.Columns.Add("UnitType");
            dt.Columns.Add("UnitStart");
            dt.Columns.Add("Country");
            dt.Columns.Add("City");
            dt.Columns.Add("County");
            dt.Columns.Add("State");
            dt.Columns.Add("Zip");
            dt.Columns.Add("FullAddress");

            dt.Columns.Add("streetPrefix");
            dt.Columns.Add("houseNumberEnd");
            dt.Columns.Add("unitEnd");
            dt.Columns.Add("houseFractionStart");
            dt.Columns.Add("houseFractionEnd");
            dt.Columns.Add("addressTypeFlag");
            dt.Columns.Add("streetSuffixdirection");
            dt.Columns.Add("addressDescription");
            dt.Columns.Add("distance");
            dt.Columns.Add("secondaryRoad");
            dt.Columns.Add("secondaryRoadNumber");
            dt.Columns.Add("inspectionDistrictPrefix");
            dt.Columns.Add("inspectionDistrict");
            dt.Columns.Add("neighberhoodPrefix");
            dt.Columns.Add("neighborhood");
            dt.Columns.Add("xcoordinator");
            dt.Columns.Add("ycoordinator");
            dt.Columns.Add("addressType");
            dt.Columns.Add("AddressLine1");
            dt.Columns.Add("AddressLine2");

            dt.Columns.Add("LevelPrefix");
            dt.Columns.Add("LevelNumberStart");
            dt.Columns.Add("LevelNumberEnd");
            dt.Columns.Add("HouseAlphaStart");
            dt.Columns.Add("HouseAlphaEnd");

            dt.Columns.Add("eventID");
            dt.Columns.Add("auditID");
            dt.Columns.Add("auditDate", typeof(DateTime));
            dt.Columns.Add("auditStatus");
            dt.Columns.Add("sourceFlag");
            dt.Columns.Add("fullAddress0");
            dt.Columns.Add("primaryFlag");
            dt.Columns.Add("CountryCode");
            dt.Columns.Add("AddressAPOKeys", typeof(DuplicatedAPOKeyModel[]));
            dt.Columns.Add("AddressAttributes", typeof(TemplateAttributeModel[]));
            dt.Columns.Add("RowIndex");

            return dt;
        }

        /// <summary>
        /// Build Address Data Row
        /// </summary>
        /// <param name="addressModel">Ref address model</param>
        /// <param name="dr">Address data table row</param>
        /// <param name="addressFormatType">Address Format Type</param>
        /// <param name="rowIndex">Row Index</param>
        /// <returns>Address Data Row</returns>
        private static DataRow BuildAddressDataRow(RefAddressModel addressModel, DataRow dr, AddressFormatType addressFormatType, int rowIndex)
        {
            var addressBuilderBll = ObjectFactory.GetObject(typeof(IAddressBuilderBll)) as IAddressBuilderBll;

            if (addressModel != null)
            {
                //reference address number
                dr["AddressID"] = addressModel.refAddressId;

                //external address UID.
                dr["AddressUID"] = addressModel.UID;
                dr["AddressSequenceNumber"] = addressModel.sourceNumber;
                dr["HouseNumberStart"] = addressModel.houseNumberStart;
                dr["StreetDirection"] = I18nStringUtil.GetString(addressModel.resStreetDirection, addressModel.streetDirection);
                dr["StreetName"] = addressModel.streetName;
                dr["StreetSuffix"] = I18nStringUtil.GetString(addressModel.resStreetSuffix, addressModel.streetSuffix);
                dr["UnitType"] = I18nStringUtil.GetString(addressModel.resUnitType, addressModel.unitType);
                dr["UnitStart"] = addressModel.unitStart;
                dr["Country"] = addressModel.country;
                dr["City"] = addressModel.city;
                dr["County"] = addressModel.county;
                dr["State"] = I18nStringUtil.GetString(addressModel.resState, addressModel.state);
                dr["Zip"] = addressModel.zip;
                dr["FullAddress"] = addressBuilderBll.BuildAddressByFormatType(addressModel, null, addressFormatType);

                dr["streetPrefix"] = addressModel.streetPrefix;
                dr["houseNumberEnd"] = addressModel.houseNumberEnd;
                dr["unitEnd"] = addressModel.unitEnd;
                dr["houseFractionStart"] = addressModel.houseFractionStart;
                dr["houseFractionEnd"] = addressModel.houseFractionEnd;
                dr["addressTypeFlag"] = addressModel.addressTypeFlag;
                dr["streetSuffixdirection"] = I18nStringUtil.GetString(addressModel.resStreetSuffixdirection, addressModel.streetSuffixdirection);
                dr["addressDescription"] = addressModel.addressDescription;
                dr["distance"] = addressModel.distance;
                dr["secondaryRoad"] = addressModel.secondaryRoad;
                dr["secondaryRoadNumber"] = addressModel.secondaryRoadNumber;
                dr["inspectionDistrictPrefix"] = addressModel.inspectionDistrictPrefix;
                dr["inspectionDistrict"] = addressModel.inspectionDistrict;
                dr["neighberhoodPrefix"] = addressModel.neighborhoodPrefix;
                dr["neighborhood"] = addressModel.neighborhood;
                dr["xcoordinator"] = addressModel.XCoordinator;
                dr["ycoordinator"] = addressModel.YCoordinator;
                dr["addressType"] = addressModel.addressType;
                dr["AddressLine1"] = addressModel.addressLine1;
                dr["AddressLine2"] = addressModel.addressLine2;

                dr["LevelPrefix"] = addressModel.levelPrefix;
                dr["LevelNumberStart"] = addressModel.levelNumberStart;
                dr["LevelNumberEnd"] = addressModel.levelNumberEnd;
                dr["HouseAlphaStart"] = addressModel.houseNumberAlphaStart;
                dr["HouseAlphaEnd"] = addressModel.houseNumberAlphaEnd;

                dr["eventID"] = addressModel.eventID;
                dr["auditID"] = addressModel.auditID;
                dr["auditDate"] = addressModel.auditDate == null ? DBNull.Value : (object)addressModel.auditDate;
                dr["auditStatus"] = addressModel.auditStatus;
                dr["sourceFlag"] = addressModel.sourceFlag;
                dr["fullAddress0"] = addressModel.fullAddress;
                dr["primaryFlag"] = addressModel.primaryFlag;

                dr["CountryCode"] = I18nStringUtil.GetString(addressModel.resCountryCode, addressModel.countryCode);

                if (addressModel.templates != null)
                {
                    dr["AddressAttributes"] = addressModel.templates;
                }

                if (addressModel.duplicatedAPOKeys != null)
                {
                    dr["AddressAPOKeys"] = addressModel.duplicatedAPOKeys;
                }

                dr["RowIndex"] = rowIndex;
            }

            return dr;
        }

        /// <summary>
        /// Create Parcel Data Table.
        /// </summary>
        /// <returns>Parcel data table.</returns>
        private static DataTable CreateParcelDataTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("ParcelNumber");
            dt.Columns.Add("UnmaskedParcelNumber");
            dt.Columns.Add("ParcelUID");
            dt.Columns.Add("ParcelSequenceNumber");
            dt.Columns.Add("Lot");
            dt.Columns.Add("Block");
            dt.Columns.Add("Subdivision");
            dt.Columns.Add("Book");
            dt.Columns.Add("Page");
            dt.Columns.Add("Tract");
            dt.Columns.Add("ParcelArea");
            dt.Columns.Add("legalDescription");
            dt.Columns.Add("LandValue");
            dt.Columns.Add("ImprovedValue");
            dt.Columns.Add("ExceptionValue");
            dt.Columns.Add("ParcelStatus");
            dt.Columns.Add("ParcelAttributes", typeof(TemplateAttributeModel[]));
            dt.Columns.Add("ParcelAPOKeys", typeof(DuplicatedAPOKeyModel[]));
            dt.Columns.Add("RowIndex");

            return dt;
        }

        /// <summary>
        /// Build parcel data row.
        /// </summary>
        /// <param name="dr">Data row</param>
        /// <param name="parcelModel">Parcel model</param>
        /// <param name="rowIndex">Row index</param>
        /// <returns>Parcel data row</returns>
        private static DataRow BuildParcelDataRow(DataRow dr, ParcelModel parcelModel, int rowIndex)
        {
            if (parcelModel != null)
            {
                dr["ParcelNumber"] = parcelModel.parcelNumber;
                dr["UnmaskedParcelNumber"] = parcelModel.unmaskedParcelNumber;
                dr["ParcelUID"] = parcelModel.UID;
                dr["ParcelSequenceNumber"] = parcelModel.sourceSeqNumber;
                dr["Lot"] = parcelModel.lot;
                dr["Block"] = parcelModel.block;
                dr["Subdivision"] = I18nStringUtil.GetString(parcelModel.resSubdivision, parcelModel.subdivision);
                dr["Book"] = parcelModel.book;
                dr["Page"] = parcelModel.page;
                dr["Tract"] = parcelModel.tract;
                dr["ParcelArea"] = parcelModel.parcelArea;
                dr["legalDescription"] = parcelModel.legalDesc;
                dr["LandValue"] = parcelModel.landValue;
                dr["ImprovedValue"] = parcelModel.improvedValue;
                dr["ExceptionValue"] = parcelModel.exemptValue;
                dr["ParcelStatus"] = parcelModel.parcelStatus;

                if (parcelModel.templates != null)
                {
                    dr["ParcelAttributes"] = parcelModel.templates;
                }

                if (parcelModel.duplicatedAPOKeys != null)
                {
                    dr["ParcelAPOKeys"] = parcelModel.duplicatedAPOKeys;
                }

                dr["RowIndex"] = rowIndex;
            }

            return dr;
        }

        #endregion Methods     
    }
}