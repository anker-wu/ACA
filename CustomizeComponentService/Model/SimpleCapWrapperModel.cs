#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SimpleCapWrapperModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: SimpleCapWrapperModel.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.WSProxy;

namespace Accela.ACA.ComponentService.Model
{
    /// <summary>
    /// This class provides one simple cap wrapper model for custom component
    /// </summary>
    [System.SerializableAttribute]
    public class SimpleCapWrapperModel
    {
        /// <summary>
        /// cap field
        /// </summary>
        private SimpleCapModel cap;

        /// <summary>
        /// cap Id
        /// </summary>
        private CapIDWrapperModel capId;

        /// <summary>
        /// cap Type
        /// </summary>
        private CapTypeWrapperModel capType;

        /// <summary>
        /// cap Detail
        /// </summary>
        private CapDetailWrapperModel capDetail;

        /// <summary>
        /// Initializes a new instance of the SimpleCapWrapperModel class
        /// </summary>
        /// <param name="capModel">cap Model</param>
        internal SimpleCapWrapperModel(SimpleCapModel capModel)
        {
            this.cap = capModel;
            capId = new CapIDWrapperModel(capModel.capID);
            capType = new CapTypeWrapperModel(capModel.capType);
            capDetail = new CapDetailWrapperModel(capModel.capDetailModel);
        }

        /// <summary>
        /// Gets Agency Code
        /// </summary>
        public string AgenyCode
        {
            get 
            {
                return capId.AgencyCode;
            }
        }

        /// <summary>
        /// Gets Cap ID
        /// </summary>
        public CapIDWrapperModel CapID
        {
            get
            {
                return capId;
            }
        }

        /// <summary>
        /// Gets Cap Detail
        /// </summary>
        public CapDetailWrapperModel CapDetail
        {
            get
            {
                return capDetail;
            }
        }

        /// <summary>
        /// Gets AltID
        /// </summary>
        public string AltID
        {
            get
            {
                return cap.altID;
            }
        }

        /// <summary>
        /// Gets Module Name
        /// </summary>
        public string Module
        {
            get
            {
                return cap.moduleName;
            }
        }        

        /// <summary>
        /// Gets Cap Type
        /// </summary>
        public CapTypeWrapperModel CapType
        {
            get 
            {
                return capType;
            }
        }                

        /// <summary>
        /// Gets Cap Status
        /// </summary>
        public string CapStatus
        {
            get 
            {
                return cap.capStatus;
            }
        }

        /// <summary>
        /// Gets Cap Class
        /// </summary>
        public string CapClass
        {
            get
            {
                return cap.capClass;
            }
        }        

        /// <summary>
        /// Gets a value indicating whether it Has Unpaid Fees
        /// </summary>
        public bool HasUnpaidFees
        {
            get
            {
                return cap.noPaidFeeFlag;
            }
        }

        /// <summary>
        /// Gets Renewal Status
        /// </summary>
        public string RenewalStatus
        {
            get
            {
                return cap.renewalStatus;
            }
        }

        /// <summary>
        /// Gets a value indicating whether it Has privilege to handle the cap
        /// </summary>
        public bool HasPrivilegeToHandleCap
        {
            get
            {
                return cap.hasPrivilegeToHandleCap;
            }
        }

        /// <summary>
        /// Gets English Trade Name
        /// </summary>
        public string EnglishTradeName
        {
            get
            {
                return cap.englishTradeName;
            }
        }

        /// <summary>
        /// Gets Arabic Trade Name
        /// </summary>
        public string ArabicTradeName
        {
            get
            {
                return cap.arabicTradeName;
            }
        }

        /// <summary>
        /// Gets Related Trade License
        /// </summary>
        public string RelatedTradeLicense
        {
            get
            {
                return cap.relatedTradeLic;
            }
        }

        /// <summary>
        /// Gets LicenseType
        /// </summary>
        public string LicenseType
        {
            get
            {
                return cap.licenseType;
            }
        }

        /// <summary>
        /// Gets a value indicating whether it Indicates if trade name is expired or not
        /// </summary>
        public bool IsTradeNameExpired
        {
            get
            {
                return cap.isTNExpired;
            }
        }        

        /// <summary>
        /// Gets Expiration Date
        /// </summary>
        public DateTime? ExpirationDate
        {
            get
            {
                return cap.expDate;
            }
        }

        /// <summary>
        /// Gets SpecialText
        /// </summary>
        public string SpecialText
        {
            get
            {
                return cap.specialText;
            }
        }
    }
}
