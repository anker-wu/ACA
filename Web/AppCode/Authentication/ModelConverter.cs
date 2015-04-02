#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: ModelConvert.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: The util for convert model.
*
* </pre>
*/

#endregion

using System.Collections.Generic;
using Accela.ACA.Common.Util;
using Accela.ACA.SSOInterface.Constant;
using Accela.ACA.SSOInterface.Model;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Authentication
{
    /// <summary>
    /// The model converter class.
    /// </summary>
    public class ModelConverter
    {
        /// <summary>
        /// Converts the automatic public user model4 SSO.
        /// </summary>
        /// <param name="publicUser">The public user.</param>
        /// <returns>user model.</returns>
        public static UserModel ConvertToUserModel4SSO(PublicUserModel4WS publicUser)
        {
            if (publicUser == null)
            {
                return null;
            }

            UserModel userModel = new UserModel();
            userModel.UserId = publicUser.userID;
            userModel.Email = publicUser.email;
            userModel.UserSeqNum = publicUser.userSeqNum;
            userModel.PeopleModel = ConvertToPeopleModel4SSO(publicUser.peopleModel);

            return userModel;
        }

        /// <summary>
        /// Convert the UserModel to PublicUserModel4WS.
        /// </summary>
        /// <param name="userModel">user model</param>
        /// <returns>public user model</returns>
        public static PublicUserModel4WS ConvertToPublicUserModel4WS(UserModel userModel)
        {
            if (userModel == null)
            {
                return null;
            }

            PublicUserModel4WS publicUserModel = new PublicUserModel4WS();
            publicUserModel.SSOType = userModel.SSOType;
            publicUserModel.SSOUserName = userModel.SSOUserName;
            publicUserModel.peopleModel = null; //TODO

            return publicUserModel;
        }

        /// <summary>
        /// Converts the automatic people model to SSO.
        /// </summary>
        /// <param name="peopleModels">The people model array.</param>
        /// <returns>UserContactModel array.</returns>
        public static UserContactModel[] ConvertToPeopleModel4SSO(PeopleModel4WS[] peopleModels)
        {
            List<UserContactModel> userContactModels = new List<UserContactModel>();

            foreach (var peopleModel in peopleModels)
            {
                UserContactModel userContactModel = new UserContactModel();
                userContactModel.AccountOwner = peopleModel.accountOwner;
                userContactModel.BirthCity = peopleModel.birthCity;
                userContactModel.BirthDate = peopleModel.birthDate;
                userContactModel.BirthRegion = peopleModel.birthRegion;
                userContactModel.BirthState = peopleModel.birthState;
                userContactModel.BusinessName = peopleModel.businessName;
                userContactModel.BusinessName2 = peopleModel.businessName2;
                userContactModel.Comment = peopleModel.comment;
                userContactModel.ContactSeqNumber = peopleModel.contactSeqNumber;
                userContactModel.ContactType = peopleModel.contactType;
                userContactModel.ContactTypeFlag = peopleModel.contactTypeFlag;
                userContactModel.Country = peopleModel.country;
                userContactModel.CountryCode = peopleModel.countryCode;
                userContactModel.DeceasedDate = peopleModel.deceasedDate;
                userContactModel.DriverLicenseNbr = peopleModel.driverLicenseNbr;
                userContactModel.DriverLicenseState = peopleModel.driverLicenseState;
                userContactModel.Email = peopleModel.email;
                userContactModel.EndBirthDate = peopleModel.endBirthDate;
                userContactModel.EndDate = peopleModel.endDate;
                userContactModel.EndDeceasedDate = peopleModel.endDeceasedDate;
                userContactModel.Fax = peopleModel.fax;
                userContactModel.FaxCountryCode = peopleModel.faxCountryCode;
                userContactModel.Fein = peopleModel.fein;
                userContactModel.FirstName = peopleModel.firstName;
                userContactModel.Flag = peopleModel.flag;
                userContactModel.FullName = peopleModel.fullName;
                userContactModel.Gender = EnumUtil<SSOConstant.Gender>.Parse(peopleModel.gender);
                userContactModel.HoldCode = peopleModel.holdCode;
                userContactModel.HoldDescription = peopleModel.holdDescription;
                userContactModel.LastName = peopleModel.lastName;
                userContactModel.MaskedSsn = peopleModel.maskedSsn;
                userContactModel.MiddleName = peopleModel.middleName;
                userContactModel.Namesuffix = peopleModel.namesuffix;
                userContactModel.PassportNumber = peopleModel.passportNumber;
                userContactModel.Phone1 = peopleModel.phone1;
                userContactModel.Phone1CountryCode = peopleModel.phone1CountryCode;
                userContactModel.Phone2 = peopleModel.phone2;
                userContactModel.Phone2CountryCode = peopleModel.phone2CountryCode;
                userContactModel.Phone3 = peopleModel.phone3;
                userContactModel.Phone3CountryCode = peopleModel.phone3CountryCode;
                userContactModel.PostOfficeBox = peopleModel.postOfficeBox;
                userContactModel.PreferredChannel = peopleModel.preferredChannel;
                userContactModel.PreferredChannelString = peopleModel.preferredChannelString;
                userContactModel.Race = peopleModel.race;
                userContactModel.Relation = peopleModel.relation;
                userContactModel.Salutation = peopleModel.salutation;
                userContactModel.ServiceProviderCode = peopleModel.serviceProviderCode;
                userContactModel.SocialSecurityNumber = peopleModel.socialSecurityNumber;
                userContactModel.StartDate = peopleModel.startDate;
                userContactModel.StateIDNbr = peopleModel.stateIDNbr;
                userContactModel.Title = peopleModel.title;
                userContactModel.TradeName = peopleModel.tradeName;

                if (peopleModel.compactAddress != null)
                {
                    userContactModel.AddressId = peopleModel.compactAddress.addressId;
                    userContactModel.AddressLine1 = peopleModel.compactAddress.addressLine1;
                    userContactModel.AddressLine2 = peopleModel.compactAddress.addressLine2;
                    userContactModel.AddressLine3 = peopleModel.compactAddress.addressLine3;
                    userContactModel.City = peopleModel.compactAddress.city;
                    userContactModel.CompactAddressCountry = peopleModel.compactAddress.country;
                    userContactModel.CompactAddressCountryCode = peopleModel.compactAddress.countryCode;
                    userContactModel.CountryZip = peopleModel.compactAddress.countryZip;
                    userContactModel.ResState = peopleModel.compactAddress.resState;
                    userContactModel.State = peopleModel.compactAddress.state;
                    userContactModel.StreetName = peopleModel.compactAddress.streetName;
                    userContactModel.Zip = peopleModel.compactAddress.zip;
                }

                userContactModels.Add(userContactModel);
            }

            return userContactModels.ToArray();
        }
    }
}