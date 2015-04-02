#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: OwnerModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: OwnerModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using System.Text;

namespace Accela.ACA.WSProxy
{
    /// <summary>
    /// Asset master model extender for APO performance tuning.
    /// this address was format as same as java side manually.
    /// </summary>
    public partial class OwnerModel
    {
        /// <summary>
        /// Gets mail address
        /// </summary>
        public string mailAddress
        {
            get
            {
                StringBuilder mailAddressField = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(mailAddress1))
                {
                    mailAddressField.Append(mailAddress1);
                }

                if (!string.IsNullOrWhiteSpace(mailAddress2))
                {
                    mailAddressField.Append(" ");
                    mailAddressField.Append(mailAddress2);
                }

                if (!string.IsNullOrWhiteSpace(mailAddress3))
                {
                    mailAddressField.Append(" ");
                    mailAddressField.Append(mailAddress3);
                }

                if (!string.IsNullOrWhiteSpace(mailCity))
                {
                    mailAddressField.Append(" ");
                    mailAddressField.Append(mailCity);
                }

                if (!string.IsNullOrWhiteSpace(mailState))
                {
                    mailAddressField.Append(" ");
                    mailAddressField.Append(mailState);
                }

                if (!string.IsNullOrWhiteSpace(mailZip))
                {
                    mailAddressField.Append(" ");
                    mailAddressField.Append(mailZip);
                }

                return mailAddressField.ToString();
            }

        }

        /// <summary>
        /// Convert to reference owner model.
        /// </summary>
        /// <returns>A RefOwnerModel</returns>
        public RefOwnerModel ToRefOwnerModel()
        {
            RefOwnerModel refOwnerModel = new RefOwnerModel();

            refOwnerModel.ownerFullName = ownerFullName;
            refOwnerModel.ownerTitle = ownerTitle;
            refOwnerModel.mailAddress1 = mailAddress1;
            refOwnerModel.mailAddress2 = mailAddress2;
            refOwnerModel.mailAddress3 = mailAddress3;
            refOwnerModel.mailCity = mailCity;
            refOwnerModel.mailZip = mailZip;
            refOwnerModel.mailState = mailState;
            refOwnerModel.mailCountry = mailCountry;
            refOwnerModel.fax = fax;
            refOwnerModel.faxCountryCode = faxCountryCode;
            refOwnerModel.phone = phone;
            refOwnerModel.phoneCountryCode = phoneCountryCode;
            refOwnerModel.email = email;
            refOwnerModel.l1OwnerNumber = ownerNumber;
            refOwnerModel.ownerNumber = ownerNumber;
            refOwnerModel.UID = UID;
            refOwnerModel.templates = templates;
            refOwnerModel.duplicatedAPOKeys = duplicatedAPOKeys;

            refOwnerModel.address1 = address1;
            refOwnerModel.address2 = address2;
            refOwnerModel.address3 = address3;
            refOwnerModel.auditDate = auditDate;
            refOwnerModel.auditID = auditID;
            refOwnerModel.auditStatus = auditStatus;
            refOwnerModel.city = city;
            refOwnerModel.country = country;
            refOwnerModel.primaryOwner = isPrimary;
            refOwnerModel.ownerFirstName = ownerFirstName;
            refOwnerModel.ownerLastName1 = ownerLastName;
            refOwnerModel.ownerMiddleName = ownerMiddleName;
            refOwnerModel.ownerStatus = ownerStatus;
            refOwnerModel.state = state;
            refOwnerModel.taxID = taxID;
            refOwnerModel.zip = zip;

            return refOwnerModel;
        }

    }
}
