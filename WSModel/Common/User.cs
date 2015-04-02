/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: User.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 * 
 *  Description:
 *  User business model, which herits from PublicUserModel4WS.
 * 
 *  Notes:
 *  $Id: User.cs 278108 2014-08-27 09:55:11Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Account
{
    /// <summary>
    /// ACA system role define.When user accecss or login ACA,
    /// The user must belong to one of below roles.
    /// </summary>
    public enum RoleType
    {
        Anonymous = 0,  // All ACA user
        Register = 1,
        LPRegister = 2,
        Admin = 3
    }

    /// <summary>
    /// User business model, which herits from PublicUserModel4WS.
    /// </summary>
    [SerializableAttribute()]
    public class User
    {
        private PublicUserModel4WS _userModel4WS;
        private string _clientIP;
        private RoleType _role = RoleType.Anonymous;

        /// <summary>
        /// Gets the public user id for ACA, the prefix is "PUBLICUSER".
        /// </summary>
        public string PublicUserId
        {
            get
            {
                return "PUBLICUSER" + _userModel4WS.userSeqNum;
            }
        }

        /// <summary>
        /// Indicates the current user whether is anonymous user (hasn't login).
        /// </summary>
        public bool IsAnonymous
        {
            get
            {
                if (String.IsNullOrEmpty(_userModel4WS.userSeqNum) || _userModel4WS.userSeqNum == "0")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets or sets current user role.
        /// </summary>
        public RoleType Role
        {
            get
            {
                return _role;
            }
            set
            {
                _role = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the register user has provider license.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the register user has provider license; otherwise, <c>false</c>.
        /// </value>
        public bool? HasProviderLicense
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the PublicUserModel4WS.
        /// </summary>
        public PublicUserModel4WS UserModel4WS
        {
            get
            {
                return _userModel4WS;
            }
            set
            {
                _userModel4WS = value;
            }
        }

        /// <summary>
        /// Gets user seq number without prefix 'public';
        /// </summary>
        public string UserSeqNum
        {
            get
            {
                return _userModel4WS.userSeqNum;
            }
        }

        /// <summary>
        /// Gets the user Salutation.
        /// </summary>
        public string Salutation
        {
            get
            {
                return PrimaryContact == null ? string.Empty : PrimaryContact.salutation;
            }
        }

        /// <summary>
        /// Gets the user title.
        /// </summary>
        public string UserTitle
        {
            get
            {
                return PrimaryContact == null ? string.Empty : PrimaryContact.title;
            }
        }

        /// <summary>
        /// Gets the user BirthDate.
        /// </summary>
        public string BirthDate
        {
            get
            {
                return PrimaryContact == null ? string.Empty : PrimaryContact.birthDate;
            }
        }

        /// <summary>
        /// Gets the user BirthDate.
        /// </summary>
        public string Gender
        {
            get
            {
                return PrimaryContact == null ? string.Empty : PrimaryContact.gender;
            }
        }

        /// <summary>
        /// Gets the user BirthDate.
        /// </summary>
        public string Country
        {
            get
            {
                return PrimaryContact == null ? string.Empty : PrimaryContact.country;
            }
        }

        /// <summary>
        /// Gets the user address.
        /// </summary>
        public string Address2
        {
            get
            {
                return (PrimaryContact == null || PrimaryContact.compactAddress == null) ? string.Empty : PrimaryContact.compactAddress.addressLine2;
            }
        }

        /// <summary>
        /// Gets the user address.
        /// </summary>
        public string PostOfficeBox
        {
            get
            {
                return PrimaryContact == null ? string.Empty : PrimaryContact.postOfficeBox;
            }
        }
        
        /// <summary>
        /// Gets the user first name.
        /// </summary>
        public string FirstName
        {
            get
            {
                return PrimaryContact == null ? string.Empty : PrimaryContact.firstName;
            }
        }

        /// <summary>
        /// Gets the user middle name.
        /// </summary>
        public string MiddleName
        {
            get
            {
                return PrimaryContact == null ? string.Empty : PrimaryContact.middleName;
            }
        }

        /// <summary>
        /// Gets the user last name.
        /// </summary>
        public string LastName
        {
            get
            {
                return PrimaryContact == null ? string.Empty : PrimaryContact.lastName;
            }
        }

        /// <summary>
        /// Gets the user address.
        /// </summary>
        public string Address
        {
            get
            {
                return (PrimaryContact == null || PrimaryContact.compactAddress == null) ? string.Empty : PrimaryContact.compactAddress.addressLine1;
            }
        }

        /// <summary>
        /// Get the business name.
        /// </summary>
        public string BusinessName
        {
            get
            {
                return PrimaryContact == null ? string.Empty : PrimaryContact.businessName;
            }
        }

        /// <summary>
        /// Get user cell phone.
        /// </summary>
        public string CellPhone
        {
            get
            {
                IList<string> phoneNumberInfo = GetPhoneNumberInfo();

                if (phoneNumberInfo.Count == 2)
                {
                    return phoneNumberInfo[0];                    
                }

                return string.Empty;
            }
        }
    
        /// <summary>
        /// Get user cell phone country code.
        /// </summary>
        public string CellPhoneCountryCode
        {
            get
            {
                IList<string> phoneNumberInfo = GetPhoneNumberInfo();

                if (phoneNumberInfo.Count == 2)
                {
                    return phoneNumberInfo[1];
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the city that user belong to.
        /// </summary>
        public string City
        {
            get
            {
                return (PrimaryContact == null || PrimaryContact.compactAddress == null) ? string.Empty : PrimaryContact.compactAddress.city;
            }
        }

        /// <summary>
        /// Gets the user email.
        /// </summary>
        public string Email
        {
            get
            {
                return _userModel4WS.email;
            }
        }

        /// <summary>
        /// Gets the user fax.
        /// </summary>
        public string Fax
        {
            get
            {
                return PrimaryContact == null ? string.Empty : PrimaryContact.fax;
            }
        }
        
        /// <summary>
        /// Gets the user fax country code.
        /// </summary>
        public string FaxCountryCode
        {
            get
            {
                return PrimaryContact == null ? string.Empty : PrimaryContact.faxCountryCode;
            }
        }

        /// <summary>
        /// Gets the user home phone.
        /// </summary>
        public string HomePhone
        {
            get
            {
                return PrimaryContact == null ? string.Empty : PrimaryContact.phone1;
            }
        }

        /// <summary>
        /// Gets the user home phone country code.
        /// </summary>
        public string HomePhoneCountryCode
        {
            get
            {
                return PrimaryContact == null ? string.Empty : PrimaryContact.phone1CountryCode;
            }
        }

        /// <summary>
        /// Gets the user pager.
        /// </summary>
        public string Pager
        {
            get
            {
                return _userModel4WS.pager;
            }
        }

        /// <summary>
        /// Gets the work phone.
        /// </summary>
        public string WorkPhone
        {
            get
            {
                return PrimaryContact == null ? string.Empty : PrimaryContact.phone3;
            }
        }
        
        /// <summary>
        /// Gets the work phone country code.
        /// </summary>
        public string WorkPhoneCountryCode
        {
            get
            {
                return PrimaryContact == null ? string.Empty : PrimaryContact.phone3CountryCode;
            }
        }

        /// <summary>
        /// Gets the Zip.
        /// </summary>
        public string Zip
        {
            get
            {
                return (PrimaryContact == null || PrimaryContact.compactAddress == null) ? string.Empty : PrimaryContact.compactAddress.zip;
            }
        }

        /// <summary>
        /// Gets the user password.
        /// </summary>
        public string Password
        {
            get
            {
                return _userModel4WS.password;
            }
        }

        /// <summary>
        /// Gets the user password hint.
        /// </summary>
        public string PasswordHint
        {
            get
            {
                return _userModel4WS.passwordHint;
            }
        }

        /// <summary>
        /// Get the Pref Contact channel.
        /// </summary>
        public string PrefContactChannel
        {
            get
            {
                return _userModel4WS.prefContactChannel;
            }
        }

        /// <summary>
        /// Gets the PrePhone.
        /// </summary>
        public string PrefPhone
        {
            get
            {
                return _userModel4WS.prefPhone;
            }
        }

        /// <summary>
        /// Gets the state that user belongs to.
        /// </summary>
        public string State
        {
            get
            {
                return (PrimaryContact == null || PrimaryContact.compactAddress == null) ? string.Empty : PrimaryContact.compactAddress.state;
            }
        }

        /// <summary>
        /// Get the user login name.
        /// </summary>
        public string UserID
        {
            get
            {
                return _userModel4WS.userID;
            }
        }

        /// <summary>
        /// Gets the user full name.
        /// </summary>
        public string FullName
        {
            get
            {
                if (PrimaryContact == null)
                {
                    return string.Empty;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append(PrimaryContact.firstName)
                    .Append(string.IsNullOrEmpty(PrimaryContact.middleName) ? string.Empty : (" " + PrimaryContact.middleName))
                    .Append(string.IsNullOrEmpty(PrimaryContact.lastName) ? string.Empty : (" " + PrimaryContact.lastName));
                return sb.ToString();
            }
        }

        /// <summary>
        /// Client IP address.
        /// </summary>
        public string ClientIP
        {
            get
            {
                return _clientIP;   
            }
            set
            {
                _clientIP = value;
            }
        }

        /// <summary>
        /// The secondary session id is only for register user to prevent manipulating session id.
        /// </summary>
        public string SessionValidationID
        {
            get;
            set;
        }

        /// <summary>
        /// public user associated Licenses not contains Lock condition's.
        /// </summary>
        public LicenseModel4WS[] Licenses
        {
            get
            {
                return _userModel4WS.licenseModel;
            }
        }

        /// <summary>
        /// public user associated Licenses contains Lock condition's.
        /// </summary>
        public Dictionary<string, ContractorLicenseModel4WS[]> AllContractorLicenses
        {
            get;

            set;
        }

        /// <summary>
        /// public user associated owners.
        /// </summary>
        public OwnerModel[] Owners
        {
            get
            {
                return _userModel4WS.ownerModel;
            }
        }

        /// <summary>
        /// public user associated contacts.
        /// </summary>
        public PeopleModel4WS[] ApprovedContacts
        {
            get
            {
                PeopleModel4WS[] approvedContacts = null;

                if (_userModel4WS.peopleModel != null && _userModel4WS.peopleModel.Length > 0)
                {
                    approvedContacts = _userModel4WS.peopleModel.Where(p =>
                        "A".Equals(p.contractorPeopleStatus, StringComparison.OrdinalIgnoreCase) ||
                        string.IsNullOrEmpty(p.contractorPeopleStatus)).ToArray();
                }

                return approvedContacts;
            }
        }

        /// <summary>
        /// Treat the first contact as primary contact.
        /// </summary>
        public PeopleModel4WS PrimaryContact
        {
            get
            {
                PeopleModel4WS primaryContact = null;

                if (ApprovedContacts != null && ApprovedContacts.Length > 0)
                {
                    primaryContact = ApprovedContacts[0];
                }

                return primaryContact;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user is authorized agent or not.
        /// </summary>
        public bool IsAuthorizedAgent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is agent clerk or not.
        /// </summary>
        public bool IsAgentClerk { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is inspector, self certified inspector or contract inspector
        /// </summary>
        public bool IsInspector { get; set; }

        /// <summary>
        /// Gets the Authorized Agent public user ID.
        /// </summary>
        public string AuthAgentPublicUserID
        {
            get
            {
                return _userModel4WS.authAgentID;
            }
        }

        /// <summary>
        /// Gets or gets a value indicating whether Label printer is connected.
        /// Used for authorized agent.
        /// </summary>
        /// <value>
        /// <c>true</c> if this printer connected; otherwise, <c>false</c>.
        /// </value>
        public bool PrinterConnected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [agent client installed].
        /// Used for authorized agent.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [agent clinet intalled]; otherwise, <c>false</c>.
        /// </value>
        public bool AgentClientInstalled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is auth agent need printer.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is auth agent need printer; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthAgentNeedPrinter { get; set; }

        /// <summary>
        /// Gets or sets the user token.
        /// </summary>
        public string UserToken { get; set; }

        /// <summary>
        /// Get phone number and country code
        /// </summary>
        /// <returns>
        /// Phone number and country code
        /// </returns>
        private IList<string> GetPhoneNumberInfo()
        {
            IList<string> result = new List<string>();

            if (!string.IsNullOrEmpty(_userModel4WS.cellPhone))
            {
                result.Add(_userModel4WS.cellPhone);
                result.Add(_userModel4WS.cellPhoneCountryCode);
            }
            else
            {
                if (ApprovedContacts == null || ApprovedContacts.Length == 0)
                {
                    return result;
                }

                // cell phone -> home phone -> work phone
                foreach (PeopleModel4WS conatceModel in ApprovedContacts)
                {                    
                    if (!string.IsNullOrEmpty(conatceModel.phone2))
                    {
                        result.Add(conatceModel.phone2);
                        result.Add(conatceModel.phone2CountryCode);
                        break;
                    }
                }

                if (result.Count == 0)
                {
                    foreach (PeopleModel4WS conatceModel in ApprovedContacts)
                    {
                        if (!string.IsNullOrEmpty(conatceModel.phone1))
                        {
                            result.Add(conatceModel.phone1);
                            result.Add(conatceModel.phone1CountryCode);
                            break;
                        }
                    }
                }

                if (result.Count == 0)
                {
                    foreach (PeopleModel4WS conatceModel in ApprovedContacts)
                    {
                        if (!string.IsNullOrEmpty(conatceModel.phone3))
                        {
                            result.Add(conatceModel.phone3);
                            result.Add(conatceModel.phone3CountryCode);
                            break;
                        }
                    }
                }
            }

            return result;
        }
    }
}
