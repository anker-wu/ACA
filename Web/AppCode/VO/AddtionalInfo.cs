#region Header

/**
* <pre>
*
*  Accela Citizen Access
*  File: AddtionalInfo.cs
*
*  Accela, Inc.
*  Copyright (C): 2008-2014
*
*  Description:
*  VO for Additional Information object.
*
*  Notes:
*      $Id: AddtionalInfo.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

#endregion Header

using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.VO
{
    /// <summary>
    /// VO for Additional Information object. This is used by CapDescriptionEdit and CapDescriptionView control.
    /// </summary>
    public class AddtionalInfo
    {
        #region Fields

        /// <summary>
        /// The job value model.
        /// </summary>
        private BValuatnModel4WS _jobValueModel;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        public string ApplicationName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets number of buildings.
        /// </summary>
        public string BuildingNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets construction type.
        /// </summary>
        public string ConstructionType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the detailed description.
        /// </summary>
        public string DetailedDesc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets general description.
        /// </summary>
        public string GeneralDesc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Housing units.
        /// </summary>
        public string HousingUnit
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets job value.This value has been format to UI format. e.g: 100 will be formatted to $100.00
        /// if want to get the original job value,need to call JobValueModel.estimatedValue. 
        /// </summary>
        public string JobValue
        {
            get
            {
                if (!ValidationUtil.IsNumber(JobValueModel.estimatedValue))
                {
                    JobValueModel.estimatedValue = "0";
                }

                return JobValueModel.estimatedValue;
            }

            set
            {
                if (!ValidationUtil.IsNumber(value))
                {
                    JobValueModel.estimatedValue = "0";
                }
                else
                {
                    JobValueModel.estimatedValue = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a <c>BValuatnModel4WS</c> object for job value.
        /// </summary>
        public BValuatnModel4WS JobValueModel
        {
            get
            {
                if (_jobValueModel == null)
                {
                    _jobValueModel = new BValuatnModel4WS();
                    _jobValueModel.estimatedValue = "0"; // initial value to "0" because this is required as number.
                    _jobValueModel.valuationPeriod = "Permit";                    
                    _jobValueModel.feeFactorFlag = "CONT";
                }

                _jobValueModel.auditStatus = "A";
                _jobValueModel.auditID = AppSession.User.PublicUserId;

                return _jobValueModel;
            }

            set
            {
                _jobValueModel = value;
            }
        }

        /// <summary>
        /// Gets or sets the public owner.
        /// </summary>
        public string PublicOwner
        {
            get;
            set;
        }

        #endregion Properties
    }
}