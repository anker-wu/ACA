#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PeopleBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: PeopleBll.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/17/2007    ken.huang    Initial.
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using System.Linq;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.BLL.People
{
    /// <summary>
    /// This class provide the ability to operation people.
    /// </summary>
    public sealed class PeopleBll : BaseBll, IPeopleBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of PeopleService.
        /// </summary>
        private PeopleWebServiceService PeopleService
        {
            get
            {
                return WSFactory.Instance.GetWebService<PeopleWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// get Contacts by both user-defined attributes and system attributes.
        /// </summary>
        /// <param name="searchPeopleModel4ws">Model of PeopleModel4WS</param>
        /// <param name="peopleType">People Type</param>
        /// <param name="recordStatus">REC_STATUS in database</param>
        /// <param name="queryFormat4ws">Model of QueryFormat4WS</param>
        /// <returns>Array of PeopleModel4WS</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public PeopleModel4WS[] GetPeoplesByAttrs(PeopleModel4WS searchPeopleModel4ws, string peopleType, string recordStatus, QueryFormat4WS queryFormat4ws)
        {
            try
            {
                return PeopleService.getPeoplesByAttrs(searchPeopleModel4ws, peopleType, recordStatus, queryFormat4ws);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get contacts and licenses with cap id.
        /// </summary>
        /// <param name="capIDs">cap id model list</param>
        /// <returns>people model list</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public PeopleModel4WS[] GetPeoplesByCapIDs(CapIDModel4WS[] capIDs)
        {
            try
            {
                return PeopleService.getPeoplesByCapIDs(capIDs);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get contact data, search type by contact.
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="capContactModel4WS">Sequence cap contact model.</param>
        /// <param name="templateColumn">The template column.</param>
        /// <param name="queryFormat">Query format for ACA</param>
        /// <returns>Search Result Model</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public SearchResultModel GetContactListByCapContactModel(string moduleName, CapContactModel4WS capContactModel4WS, string[] templateColumn, QueryFormat queryFormat)
        {
            try
            {
                return PeopleService.getCapContactByCapContactModel(AgencyCode, moduleName, capContactModel4WS, User.PublicUserId, templateColumn, queryFormat);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get the duplicate contact address list.
        /// </summary>
        /// <param name="existContactAddresses">The exist contact address model list.</param>
        /// <param name="targetContactAddress">the target contact address model.</param>
        /// <returns>The duplicate contact address list</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public ContactAddressModel[] GetDuplicateContactAddressList(ContactAddressModel[] existContactAddresses, ContactAddressModel targetContactAddress)
        {
            try
            {
                return PeopleService.getDuplicateContactAddressList(AgencyCode, existContactAddresses, targetContactAddress);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Convert contact model to data row.
        /// </summary>
        /// <param name="capContactModel4WS">CapContactModel4WS model</param>
        /// <returns>a data row with Contact</returns>
        public DataRow ConvertContactModelToDataRow(CapContactModel4WS capContactModel4WS)
        {
            DataRow drContact = null;

            if (capContactModel4WS != null && capContactModel4WS.people != null)
            {
                DataTable dtContact = ConstructContactDataTable();
                drContact = dtContact.NewRow();

                drContact[ColumnConstant.Contact.RowIndex.ToString()] = capContactModel4WS.people.RowIndex;
                drContact[ColumnConstant.Contact.AdditionalAddresses.ToString()] = (capContactModel4WS.people.contactAddressList == null
                                 ? 0 : capContactModel4WS.people.contactAddressList.Length).ToString();

                drContact[ColumnConstant.Contact.ContactType.ToString()] = capContactModel4WS.people.contactType;
                drContact[ColumnConstant.Contact.ContactTypeFlag.ToString()] = capContactModel4WS.people.contactTypeFlag;
                drContact[ColumnConstant.Contact.Salutation.ToString()] = capContactModel4WS.people.salutation;
                drContact[ColumnConstant.Contact.Title.ToString()] = capContactModel4WS.people.title;
                drContact[ColumnConstant.Contact.FullName.ToString()] = GetContactFullName(capContactModel4WS.people);
                drContact[ColumnConstant.Contact.FirstName.ToString()] = capContactModel4WS.people.firstName;
                drContact[ColumnConstant.Contact.MiddleName.ToString()] = capContactModel4WS.people.middleName;
                drContact[ColumnConstant.Contact.LastName.ToString()] = capContactModel4WS.people.lastName;
                drContact[ColumnConstant.Contact.SuffixName.ToString()] = capContactModel4WS.people.namesuffix;
                drContact[ColumnConstant.Contact.BirthDate.ToString()] = I18nDateTimeUtil.ParseFromWebService4DataTable(capContactModel4WS.people.birthDate);
                drContact[ColumnConstant.Contact.Gender.ToString()] = capContactModel4WS.people.gender;
                drContact[ColumnConstant.Contact.Business.ToString()] = capContactModel4WS.people.businessName;
                drContact[ColumnConstant.Contact.POBox.ToString()] = capContactModel4WS.people.postOfficeBox;

                if (capContactModel4WS.people.compactAddress != null)
                {
                    drContact[ColumnConstant.Contact.AddressLine1.ToString()] = capContactModel4WS.people.compactAddress.addressLine1;
                    drContact[ColumnConstant.Contact.AddressLine2.ToString()] = capContactModel4WS.people.compactAddress.addressLine2;
                    drContact[ColumnConstant.Contact.AddressLine3.ToString()] = capContactModel4WS.people.compactAddress.addressLine3;
                    drContact[ColumnConstant.Contact.City.ToString()] = capContactModel4WS.people.compactAddress.city;
                    drContact[ColumnConstant.Contact.State.ToString()] = I18nStringUtil.GetString(capContactModel4WS.people.compactAddress.resState, capContactModel4WS.people.compactAddress.state);
                    drContact[ColumnConstant.Contact.Zip.ToString()] = capContactModel4WS.people.compactAddress.zip;
                    drContact[ColumnConstant.Contact.Country.ToString()] = capContactModel4WS.people.compactAddress.countryCode;
                }

                drContact[ColumnConstant.Contact.TradeName.ToString()] = capContactModel4WS.people.tradeName;
                drContact[ColumnConstant.Contact.Fein.ToString()] = capContactModel4WS.people.fein;
                drContact[ColumnConstant.Contact.SocialSecurityNumber.ToString()] = capContactModel4WS.people.socialSecurityNumber;

                drContact[ColumnConstant.Contact.HomePhone.ToString()] = capContactModel4WS.people.phone1;
                drContact[ColumnConstant.Contact.HomePhoneCode.ToString()] = capContactModel4WS.people.phone1CountryCode;
                drContact[ColumnConstant.Contact.WorkPhone.ToString()] = capContactModel4WS.people.phone3;
                drContact[ColumnConstant.Contact.WorkPhoneCode.ToString()] = capContactModel4WS.people.phone3CountryCode;
                drContact[ColumnConstant.Contact.MobilePhone.ToString()] = capContactModel4WS.people.phone2;
                drContact[ColumnConstant.Contact.MobilePhoneCode.ToString()] = capContactModel4WS.people.phone2CountryCode;
                drContact[ColumnConstant.Contact.Fax.ToString()] = capContactModel4WS.people.fax;
                drContact[ColumnConstant.Contact.FaxCode.ToString()] = capContactModel4WS.people.faxCountryCode;
                drContact[ColumnConstant.Contact.Email.ToString()] = capContactModel4WS.people.email;
                drContact[ColumnConstant.Contact.RefSequence.ToString()] = capContactModel4WS.refContactNumber;
                drContact[ColumnConstant.Contact.ContactSequence.ToString()] = capContactModel4WS.people.contactSeqNumber;
                drContact[ColumnConstant.Contact.BusinessName2.ToString()] = capContactModel4WS.people.businessName2;
                drContact[ColumnConstant.Contact.BirthplaceCity.ToString()] = capContactModel4WS.people.birthCity;
                drContact[ColumnConstant.Contact.BirthplaceState.ToString()] = capContactModel4WS.people.birthState;
                drContact[ColumnConstant.Contact.BirthplaceCountry.ToString()] = capContactModel4WS.people.birthRegion;
                drContact[ColumnConstant.Contact.Race.ToString()] = capContactModel4WS.people.race;
                drContact[ColumnConstant.Contact.DeceasedDate.ToString()] = I18nDateTimeUtil.FormatToDateStringForUI(capContactModel4WS.people.deceasedDate);
                drContact[ColumnConstant.Contact.PassportNumber.ToString()] = capContactModel4WS.people.passportNumber;
                drContact[ColumnConstant.Contact.DriverLicenseNumber.ToString()] = capContactModel4WS.people.driverLicenseNbr;
                drContact[ColumnConstant.Contact.DriverLicenseState.ToString()] = capContactModel4WS.people.driverLicenseState;
                drContact[ColumnConstant.Contact.StateIdNumber.ToString()] = capContactModel4WS.people.stateIDNbr;
                drContact[ColumnConstant.Contact.ContactPermission.ToString()] = capContactModel4WS.accessLevel;
                drContact[ColumnConstant.Contact.CapContactModel.ToString()] = capContactModel4WS;
                drContact[ColumnConstant.Contact.PreferredChannel.ToString()] = capContactModel4WS.people.preferredChannel;
                drContact[ColumnConstant.Contact.Comment.ToString()] = capContactModel4WS.people.comment;
                drContact[ColumnConstant.Contact.ComponentName.ToString()] = capContactModel4WS.componentName;

                drContact[ColumnConstant.TEAMPLATE_ATTRIBUTE] = capContactModel4WS.people.attributes;
            }

            return drContact;
        }

        /// <summary>
        /// build data table for contact list.
        /// </summary>
        /// <param name="capContactModel4WSList">capContactModel array</param>
        /// <returns>dataTable for capContact list</returns>
        public DataTable ConvertContactListToDataTable(CapContactModel4WS[] capContactModel4WSList)
        {
            DataTable dtContact = ConstructContactDataTable();

            if (capContactModel4WSList != null && capContactModel4WSList.Length > 0)
            {
                foreach (CapContactModel4WS capContactModel4WS in capContactModel4WSList)
                {
                    if (capContactModel4WS == null || capContactModel4WS.people == null)
                    {
                        continue;
                    }

                    DataRow dr = ConvertContactModelToDataRow(capContactModel4WS);

                    if (dr != null)
                    {
                        DataRow drContact = dtContact.NewRow();
                        drContact.ItemArray = dr.ItemArray;
                        dtContact.Rows.Add(drContact);
                    }
                }
            }

            return dtContact;
        }

        /// <summary>
        /// Get associate contacts by user id.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="userSeqNum">user sequence number</param>
        /// <returns>Array for PeopleModel4WS</returns>
        public PeopleModel4WS[] GetAssociatedContactsByUserId(string agencyCode, string userSeqNum)
        {
            PeopleModel4WS[] model = null;

            try
            {
                if (!string.IsNullOrEmpty(userSeqNum) && !userSeqNum.Equals("-1"))
                {
                    model = PeopleService.getAssociatedContactsByUserId(agencyCode, Convert.ToInt64(userSeqNum));
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }

            return model;
        }

        /// <summary>
        /// Add reference contact for public user
        /// </summary>
        /// <param name="publicUser">the public user model</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void AddContact4PublicUser(PublicUserModel4WS publicUser)
        {
            try
            {
                PeopleService.addContact4PublicUser(publicUser, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Edit reference contact.
        /// </summary>
        /// <param name="peopleModel">the people model</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void EditRefContact(PeopleModel peopleModel)
        {
            try
            {
                PeopleService.editRefContact(peopleModel, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get contact by standard choice settings.
        /// </summary>
        /// <param name="peopleModel">people object</param>
        /// <returns>People object.</returns>
        public PeopleModel GetPeopleByClosematch(PeopleModel peopleModel)
        {
            //ACA only search active reference contact.
            peopleModel.auditStatus = ACAConstant.VALID_STATUS;

            if (peopleModel.birthDate != null && peopleModel.endBirthDate == null)
            {
                peopleModel.endBirthDate = peopleModel.birthDate;
            }

            if (peopleModel.deceasedDate != null && peopleModel.endDeceasedDate == null)
            {
                peopleModel.endDeceasedDate = peopleModel.deceasedDate;
            }

            PeopleModel people = PeopleService.getPeopleByClosematch(AgencyCode, peopleModel);

            CommonUtil.AdjustTime4PeopleBirthAndDeceaseDate(people);

            return people;
        }

        /// <summary>
        /// Get identity validation result.
        /// </summary>
        /// <param name="peopleModel">the people model.</param>
        /// <returns>The generic identity model.</returns>
        public GenericIdentityFieldModel[] GetIdentityValidationResult(PeopleModel peopleModel)
        {
            GenericIdentityFieldModel[] identityFields = PeopleService.getIdentityValidationResult(AgencyCode, peopleModel);

            return identityFields;
        }

        /// <summary>
        /// Remove reference contact for public user.
        /// </summary>
        /// <param name="userSeqNbr">The user sequence NBR.</param>
        /// <param name="contactSeqNbr">The contact sequence NBR.</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void RemoveContact4publicUser(long userSeqNbr, long contactSeqNbr)
        {
            try
            {
                PeopleService.removeContact4publicUser(AgencyCode, userSeqNbr, contactSeqNbr);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Sets account owner for public user.
        /// </summary>
        /// <param name="userSeqNbr">The user sequence NBR.</param>
        /// <param name="contactSeqNbr">The contact sequence NBR.</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void SetAccountOwner4PublicUser(long userSeqNbr, long contactSeqNbr)
        {
            try
            {
                PeopleService.setAccountOwner4publicUser(AgencyCode, userSeqNbr, contactSeqNbr, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Reference contact model.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="people">people model</param>
        /// <param name="needTemplateAttribute">If need template attribute.</param>
        /// <param name="filterPermission">If need filter contact by permission.</param>
        /// <param name="queryFormat">query format model</param>
        /// <returns>PeopleModel array</returns>
        public PeopleModel[] GetRefContactByPeopleModel(string agencyCode, PeopleModel people, bool needTemplateAttribute, bool filterPermission, QueryFormat queryFormat)
        {
            if (people != null)
            {
                if (people.birthDate != null && people.endBirthDate == null)
                {
                    people.endBirthDate = people.birthDate;
                }

                if (people.deceasedDate != null && people.endDeceasedDate == null)
                {
                    people.endDeceasedDate = people.deceasedDate;
                }
            }

            PeopleModel[] peopleArray = PeopleService.getRefPeoples(agencyCode, people, needTemplateAttribute, filterPermission, queryFormat);

            CommonUtil.AdjustTime4PeopleBirthAndDeceaseDate(peopleArray);

            return peopleArray;
        }

        /// <summary>
        /// Is contact address used in daily as primary
        /// </summary>
        /// <param name="agencyCode">the agency code</param>
        /// <param name="addressID">the address id</param>
        /// <returns>true or false.</returns>
        public bool IsContactAddressUsedInDailyAsPrimary(string agencyCode, long addressID)
        {
           return PeopleService.isPrimaryContactAddress(agencyCode, addressID, true);
        }

        /// <summary>
        /// Get contact user name.
        /// </summary>
        /// <param name="user">the public user model.</param>
        /// <param name="isShowMiddleName">whether to show middle name.</param>
        /// <returns>Contact user name.</returns>
        public string GetContactUserName(PublicUserModel4WS user, bool isShowMiddleName = false)
        {
            string currentUserName = string.Empty;

            if (user == null)
            {
                return currentUserName;
            }

            if (user.peopleModel == null
                || user.peopleModel.Count(p => ContractorPeopleStatus.Approved.Equals(p.contractorPeopleStatus, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(p.contractorPeopleStatus)) == 0)
            {
                return user.userID;
            }

            PeopleModel4WS[] peoples = user.peopleModel.Where(p => ContractorPeopleStatus.Approved.Equals(p.contractorPeopleStatus, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(p.contractorPeopleStatus)).ToArray();

            if (peoples != null && peoples.Length == 1)
            {
                PeopleModel4WS people = peoples[0];

                currentUserName = GetContactUserName(people, isShowMiddleName);
            }

            if (string.IsNullOrEmpty(currentUserName))
            {
                currentUserName = user.userID;
            }

            return currentUserName;
        }

        /// <summary>
        /// Get contact user name.
        /// </summary>
        /// <param name="people">The people object.</param>
        /// <param name="isShowMiddleName">whether to show middle name.</param>
        /// <returns>Contact user name.</returns>
        public string GetContactUserName(PeopleModel4WS people, bool isShowMiddleName = false)
        {
            if (people == null)
            {
                return string.Empty;
            }

            string contactName = string.Empty;

            if (ContactType4License.Organization.ToString().Equals(people.contactTypeFlag, StringComparison.OrdinalIgnoreCase))
            {
                contactName = ScriptFilter.EncodeHtml(people.businessName);
            }
            else if (ContactType4License.Individual.ToString().Equals(people.contactTypeFlag, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(people.contactTypeFlag))
            {
                if (!string.IsNullOrEmpty(people.fullName))
                {
                    contactName = DataUtil.ConcatStringWithSplitChar(new string[] { people.fullName }, ACAConstant.BLANK);
                }
                else if (isShowMiddleName)
                {
                    contactName =
                        DataUtil.ConcatStringWithSplitChar(
                            new[] { people.firstName, people.middleName, people.lastName },
                            ACAConstant.BLANK);
                }
                else
                {
                    contactName =
                        DataUtil.ConcatStringWithSplitChar(
                            new[] { people.firstName, people.lastName },
                            ACAConstant.BLANK);
                }
            }

            return contactName;
        }

        /// <summary>
        /// Search the customer list.
        /// </summary>
        /// <param name="capContact">The CapContactModel.</param>
        /// <param name="enableSoundex">Enable <c>soundex</c> search or not.</param>
        /// <param name="capTypeFilterName">The cap type's filter name.</param>
        /// <param name="moduleName">The module name.</param>
        /// <param name="recordType">The record type model.</param>
        /// <param name="queryFormat">The QueryFormat model.</param>
        /// <returns>Return the customer list.</returns>
        public PeopleModel[] SearchCustomerList(CapContactModel capContact, bool enableSoundex, string capTypeFilterName, string moduleName, CapTypeModel recordType, QueryFormat queryFormat)
        {
            PeopleModel people = capContact.people;

            if (people != null)
            {
                if (people.birthDate != null && people.endBirthDate == null)
                {
                    people.endBirthDate = people.birthDate;
                }

                if (people.deceasedDate != null && people.endDeceasedDate == null)
                {
                    people.endDeceasedDate = people.deceasedDate;
                }
            }

            PeopleModel[] peoples = PeopleService.searchCustomerList(capContact, enableSoundex, capTypeFilterName, moduleName, recordType, queryFormat, User.PublicUserId);

            CommonUtil.AdjustTime4PeopleBirthAndDeceaseDate(peoples);

            return peoples;
        }

        /// <summary>
        /// Creates the original edit customers.
        /// </summary>
        /// <param name="peopleModel">The people model.</param>
        /// <returns>the People object.</returns>
        /// <exception cref="DataValidateException">{ <c>peopleModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public PeopleModel CreateOrEditCustomers(PeopleModel peopleModel)
        {
            if (peopleModel == null)
            {
                throw new DataValidateException(new string[] { "peopleModel" });
            }

            try
            {
                PeopleModel people = PeopleService.createOrEditCustomers(peopleModel, User.PublicUserId);

                CommonUtil.AdjustTime4PeopleBirthAndDeceaseDate(people);

                return people;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get the contact identity fields
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>Return the contact identity fields.</returns>
        public GenericIdentityFieldModel[] GetContactIdentityFields(string agencyCode)
        {
            return PeopleService.getContactIdentityFields(agencyCode);
        }

        /// <summary>
        /// Get the duplicate contact address list.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="contactAddressList">the target contact address model.</param>
        /// <returns>The duplicate contact address list</returns>
        public ContactAddressModel[] GetDuplicateContactAddress(string agencyCode, ContactAddressModel[] contactAddressList)
        {
            if (contactAddressList != null && contactAddressList.Length > 1)
            {
                return PeopleService.getDuplicateContactAddress(agencyCode, contactAddressList); 
            }

            return null;
        }

        /// <summary>
        /// Construct a new DataTable for Contact.
        /// </summary>
        /// <returns>
        /// Construct contact dataTable
        /// </returns>
        private static DataTable ConstructContactDataTable()
        {
            DataTable contactTable = new DataTable();
            contactTable.Columns.Add(ColumnConstant.Contact.RowIndex.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.AdditionalAddresses.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.ContactType.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.ContactTypeFlag.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.Salutation.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.Title.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.FullName.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.FirstName.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.MiddleName.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.LastName.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.SuffixName.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.BirthDate.ToString(), typeof(DateTime));
            contactTable.Columns.Add(ColumnConstant.Contact.Gender.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.Business.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.AddressLine1.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.AddressLine2.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.AddressLine3.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.Fein.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.SocialSecurityNumber.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.TradeName.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.City.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.State.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.Zip.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.POBox.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.Country.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.HomePhone.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.HomePhoneCode.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.WorkPhone.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.WorkPhoneCode.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.MobilePhone.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.MobilePhoneCode.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.Fax.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.FaxCode.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.Email.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.ContactSequence.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.RefSequence.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.BusinessName2.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.BirthplaceCity.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.BirthplaceState.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.BirthplaceCountry.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.Race.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.DeceasedDate.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.PassportNumber.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.DriverLicenseNumber.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.DriverLicenseState.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.StateIdNumber.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.ContactPermission.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.Required.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.CapContactModel.ToString(), typeof(CapContactModel4WS));
            contactTable.Columns.Add(ColumnConstant.TEAMPLATE_ATTRIBUTE, typeof(TemplateAttributeModel[]));
            contactTable.Columns.Add(ColumnConstant.Contact.PreferredChannel.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.Comment.ToString());
            contactTable.Columns.Add(ColumnConstant.Contact.ComponentName.ToString());

            return contactTable;
        }

        /// <summary>
        /// Construct contact full name which consist of first name, middle name and last name
        /// </summary>
        /// <param name="people">the PeopleModel4WS instance</param>
        /// <returns>the contact full name</returns>
        private string GetContactFullName(PeopleModel4WS people)
        {
            if (people == null)
            {
                return string.Empty;
            }

            string contactFullName = people.fullName;

            if (string.IsNullOrEmpty(contactFullName))
            {
                string[] fullName = { people.firstName, people.middleName, people.lastName };
                contactFullName = DataUtil.ConcatStringWithSplitChar(fullName, ACAConstant.BLANK);
            }

            return contactFullName;
        }

        #endregion
    }
}
