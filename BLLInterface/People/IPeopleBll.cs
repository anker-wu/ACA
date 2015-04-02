#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IPeopleBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IPeopleBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using System.Data;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.People
{
    /// <summary>
    /// Defines method signs for People.
    /// </summary>
    public interface IPeopleBll
    {
        #region Methods

        /// <summary>
        /// get Contacts by both user-defined attributes and system attributes.
        /// </summary>
        /// <param name="searchPeopleModel4ws">Model of PeopleModel4WS</param>
        /// <param name="peopleType">People Type</param>
        /// <param name="recordStatus">REC_STATUS in database</param>
        /// <param name="queryFormat4ws">Model of QueryFormat4WS</param>
        /// <returns>Array of PeopleModel4WS</returns>
        PeopleModel4WS[] GetPeoplesByAttrs(PeopleModel4WS searchPeopleModel4ws, string peopleType, string recordStatus, QueryFormat4WS queryFormat4ws);

        /// <summary>
        /// get contacts and licenses with cap id.
        /// </summary>
        /// <param name="capIDs">cap id model list</param>
        /// <returns>people model list</returns>
        PeopleModel4WS[] GetPeoplesByCapIDs(CapIDModel4WS[] capIDs);

        /// <summary>
        /// Get contact data, search type by contact.
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="capContactModel4WS">Sequence cap contact model.</param>
        /// <param name="templateColumn">The template column.</param>
        /// <param name="queryFormat">Query format for ACA</param>
        /// <returns>Search Result Model</returns>
        SearchResultModel GetContactListByCapContactModel(string moduleName, CapContactModel4WS capContactModel4WS, string[] templateColumn, QueryFormat queryFormat);

        /// <summary>
        /// Get the duplicate contact address list.
        /// </summary>
        /// <param name="existContactAddresses">The exist contact address model list.</param>
        /// <param name="targetContactAddress">the target contact address model.</param>
        /// <returns>The duplicate contact address list</returns>
        ContactAddressModel[] GetDuplicateContactAddressList(ContactAddressModel[] existContactAddresses, ContactAddressModel targetContactAddress);

        /// <summary>
        /// build data table for contact list.
        /// </summary>
        /// <param name="capContactModel4WSList">capContactModel array</param>
        /// <returns>dataTable for capContact list</returns>
        DataTable ConvertContactListToDataTable(CapContactModel4WS[] capContactModel4WSList);

        /// <summary>
        /// Convert contact model to data row.
        /// </summary>
        /// <param name="contact">a CapContactModel4WS model</param>
        /// <returns>a data row</returns>
        DataRow ConvertContactModelToDataRow(CapContactModel4WS contact);

        /// <summary>
        /// Get associate contacts by user id.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="userSeqNum">user sequence number</param>
        /// <returns>Array for PeopleModel4WS</returns>
        PeopleModel4WS[] GetAssociatedContactsByUserId(string agencyCode, string userSeqNum);

        /// <summary>
        /// Get Reference contact model.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="people">people model</param>
        /// <param name="needTemplateAttribute">If need template attribute.</param>
        /// <param name="filterPermission">If need filter reference contact by permission.</param>
        /// <param name="queryFormat">query format model</param>
        /// <returns>PeopleModel array</returns>
        PeopleModel[] GetRefContactByPeopleModel(string agencyCode, PeopleModel people, bool needTemplateAttribute, bool filterPermission, QueryFormat queryFormat);

        /// <summary>
        /// Add reference contact for public user
        /// </summary>
        /// <param name="publicUser">the public user model</param>
        void AddContact4PublicUser(PublicUserModel4WS publicUser);

        /// <summary>
        /// Edit reference contact.
        /// </summary>
        /// <param name="peopleModel">the people model</param>
        void EditRefContact(PeopleModel peopleModel);

        /// <summary>
        /// Remove reference contact for public user.
        /// </summary>
        /// <param name="userSeqNbr">The user sequence NBR.</param>
        /// <param name="contactSeqNbr">The contact sequence NBR.</param>
        void RemoveContact4publicUser(long userSeqNbr, long contactSeqNbr);

        /// <summary>
        /// Sets account owner for public user.
        /// </summary>
        /// <param name="userSeqNbr">The user sequence NBR.</param>
        /// <param name="contactSeqNbr">The contact sequence NBR.</param>
        void SetAccountOwner4PublicUser(long userSeqNbr, long contactSeqNbr);

        /// <summary>
        /// Get contact by standard choice settings.
        /// </summary>
        /// <param name="peopleModel">people object</param>
        /// <returns>People object.</returns>
        PeopleModel GetPeopleByClosematch(PeopleModel peopleModel);

        /// <summary>
        /// Get identity validation result.
        /// </summary>
        /// <param name="peopleModel">the people model.</param>
        /// <returns>The generic identity model.</returns>
        GenericIdentityFieldModel[] GetIdentityValidationResult(PeopleModel peopleModel);

        /// <summary>
        /// Get contact user name.
        /// </summary>
        /// <param name="user">the public user model.</param>
        /// <param name="isShowMiddleName">whether to show middle name.</param>
        /// <returns>Contact user name.</returns>
        string GetContactUserName(PublicUserModel4WS user, bool isShowMiddleName = false);

        /// <summary>
        /// Get contact user name.
        /// </summary>
        /// <param name="people">The people object.</param>
        /// <param name="isShowMiddleName">whether to show middle name.</param>
        /// <returns>Contact user name.</returns>
        string GetContactUserName(PeopleModel4WS people, bool isShowMiddleName = false);

        /// <summary>
        /// Is contact address used in daily as primary
        /// </summary>
        /// <param name="agencyCode">the agency code</param>
        /// <param name="addressID">the address ID</param>
        /// <returns><c>true</c> if [is contact address used information daily asynchronous primary] [the specified agency code]; otherwise, <c>false</c>.</returns>
        bool IsContactAddressUsedInDailyAsPrimary(string agencyCode, long addressID);

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
        PeopleModel[] SearchCustomerList(CapContactModel capContact, bool enableSoundex, string capTypeFilterName, string moduleName, CapTypeModel recordType, QueryFormat queryFormat);

        /// <summary>
        /// Get the contact identity fields
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>Return the contact identity fields.</returns>
        GenericIdentityFieldModel[] GetContactIdentityFields(string agencyCode);

        /// <summary>
        /// Creates the original edit customers.
        /// </summary>
        /// <param name="peopleModel">The people model.</param>
        /// <returns>the People object.</returns>
        PeopleModel CreateOrEditCustomers(PeopleModel peopleModel);

        /// <summary>
        /// Get the duplicate contact address list.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="contactAddressList">the target contact address model.</param>
        /// <returns>The duplicate contact address list</returns>
        ContactAddressModel[] GetDuplicateContactAddress(string agencyCode, ContactAddressModel[] contactAddressList);

        #endregion Methods
    }
}
