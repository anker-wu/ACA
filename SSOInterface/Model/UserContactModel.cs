#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: PeopleModel.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: The people model.
*
* </pre>
*/

#endregion

using System;
using Accela.ACA.SSOInterface.Constant;

namespace Accela.ACA.SSOInterface.Model
{
    /// <summary>
    /// People model
    /// </summary>
    [Serializable]
    public class UserContactModel
    {
        /// <summary>
        /// Gets or sets the birth city.
        /// </summary>
        /// <value>The birth city.</value>
        public string BirthCity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>The birth date.</value>
        public string BirthDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the birth region.
        /// </summary>
        /// <value>The birth region.</value>
        public string BirthRegion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state of the birth.
        /// </summary>
        /// <value>The state of the birth.</value>
        public string BirthState
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the business.
        /// </summary>
        /// <value>The name of the business.</value>
        public string BusinessName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the business name2.
        /// </summary>
        /// <value>The business name2.</value>
        public string BusinessName2
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the contact sequence number.
        /// </summary>
        /// <value>The contact sequence number.</value>
        public string ContactSeqNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the contact.
        /// </summary>
        /// <value>The type of the contact.</value>
        public string ContactType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the contact type flag.
        /// </summary>
        /// <value>The contact type flag.</value>
        public string ContactTypeFlag
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        public string Country
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        /// <value>The country code.</value>
        public string CountryCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the deceased date.
        /// </summary>
        /// <value>The deceased date.</value>
        public string DeceasedDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the driver license NBR.
        /// </summary>
        /// <value>The driver license NBR.</value>
        public string DriverLicenseNbr
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state of the driver license.
        /// </summary>
        /// <value>The state of the driver license.</value>
        public string DriverLicenseState
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the end birth date.
        /// </summary>
        /// <value>The end birth date.</value>
        public string EndBirthDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        public string EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the end deceased date.
        /// </summary>
        /// <value>The end deceased date.</value>
        public string EndDeceasedDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the fax.
        /// </summary>
        /// <value>The fax.</value>
        public string Fax
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the fax country code.
        /// </summary>
        /// <value>The fax country code.</value>
        public string FaxCountryCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the fein.
        /// </summary>
        /// <value>The fein.</value>
        public string Fein
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the flag.
        /// </summary>
        /// <value>The flag.</value>
        public string Flag
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>The gender.</value>
        public SSOConstant.Gender Gender
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the hold code.
        /// </summary>
        /// <value>The hold code.</value>
        public string HoldCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the hold description.
        /// </summary>
        /// <value>The hold description.</value>
        public string HoldDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the masked SSN.
        /// </summary>
        /// <value>The masked SSN.</value>
        public string MaskedSsn
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the middle.
        /// </summary>
        /// <value>The name of the middle.</value>
        public string MiddleName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name suffix.
        /// </summary>
        /// <value>The name suffix.</value>
        public string Namesuffix
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the passport number.
        /// </summary>
        /// <value>The passport number.</value>
        public string PassportNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the phone1.
        /// </summary>
        /// <value>The phone1.</value>
        public string Phone1
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the phone1 country code.
        /// </summary>
        /// <value>The phone1 country code.</value>
        public string Phone1CountryCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the phone2.
        /// </summary>
        /// <value>The phone2.</value>
        public string Phone2
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the phone2 country code.
        /// </summary>
        /// <value>The phone2 country code.</value>
        public string Phone2CountryCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the phone3.
        /// </summary>
        /// <value>The phone3.</value>
        public string Phone3
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the phone3 country code.
        /// </summary>
        /// <value>The phone3 country code.</value>
        public string Phone3CountryCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the post office box.
        /// </summary>
        /// <value>The post office box.</value>
        public string PostOfficeBox
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the preferred channel.
        /// </summary>
        /// <value>The preferred channel.</value>
        public string PreferredChannel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the preferred channel string.
        /// </summary>
        /// <value>The preferred channel string.</value>
        public string PreferredChannelString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the race.
        /// </summary>
        /// <value>The race.</value>
        public string Race
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the relation.
        /// </summary>
        /// <value>The relation.</value>
        public string Relation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the salutation.
        /// </summary>
        /// <value>The salutation.</value>
        public string Salutation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the service provider code.
        /// </summary>
        /// <value>The service provider code.</value>
        public string ServiceProviderCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the social security number.
        /// </summary>
        /// <value>The social security number.</value>
        public string SocialSecurityNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        public string StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state unique identifier NBR.
        /// </summary>
        /// <value>The state unique identifier NBR.</value>
        public string StateIDNbr
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the trade.
        /// </summary>
        /// <value>The name of the trade.</value>
        public string TradeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the account owner.
        /// </summary>
        /// <value>The account owner.</value>
        public string AccountOwner
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the address unique identifier.
        /// </summary>
        /// <value>The address unique identifier.</value>
        public string AddressId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the address line1.
        /// </summary>
        /// <value>The address line1.</value>
        public string AddressLine1
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the address line2.
        /// </summary>
        /// <value>The address line2.</value>
        public string AddressLine2
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the address line3.
        /// </summary>
        /// <value>The address line3.</value>
        public string AddressLine3
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the compact address country.
        /// </summary>
        /// <value>The compact address country.</value>
        public string CompactAddressCountry
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the compact address country code.
        /// </summary>
        /// <value>The compact address country code.</value>
        public string CompactAddressCountryCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the country zip.
        /// </summary>
        /// <value>The country zip.</value>
        public string CountryZip
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state of the resource.
        /// </summary>
        /// <value>The state of the resource.</value>
        public string ResState
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public string State
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the street.
        /// </summary>
        /// <value>The name of the street.</value>
        public string StreetName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        /// <value>The zip.</value>
        public string Zip
        {
            get;
            set;
        }
    }
}
