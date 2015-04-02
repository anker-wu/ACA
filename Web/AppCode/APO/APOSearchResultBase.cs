#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: APOSearchResultBase.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description: Provide the base control for APO search result page inherit.
 *
 *  Notes:
 *      $Id: APOSearchResultBase.cs 170366 2014-06-19 05:34:25Z ACHIEVO\blues.gao $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web
{
    /// <summary>
    /// Provide the base control for APO search result page.
    /// </summary>
    public class APOSearchResultBase : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// contact session parameter
        /// </summary>
        private APOSessionParameterModel _apoSessionParameter;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the contact session parameter
        /// </summary>
        protected APOSessionParameterModel APOSessionParameter
        {
            get
            {
                if (AppSession.IsAdmin)
                {
                    _apoSessionParameter = new APOSessionParameterModel();
                }

                if (_apoSessionParameter == null)
                {
                    _apoSessionParameter = AppSession.GetAPOSessionParameter();
                }

                return _apoSessionParameter;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        protected bool IsEditable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the smart choice validate for address
        /// </summary>
        protected bool IsValidate
        {
            get
            {
                return APOSessionParameter.IsValidate;
            }
        }

        /// <summary>
        /// Gets a value indicating whether address list is from map.
        /// </summary>
        protected bool IsFromMap
        {
            get
            {
                return APOSessionParameter.IsFromMap;
            }
        }

        /// <summary>
        /// Gets a value indicating whether create cap from map or not
        /// </summary>
        protected bool IsCreateCapFromGIS
        {
            get
            {
                return APOSessionParameter.IsCreateCapFromGIS;
            }
        }

        /// <summary>
        /// Gets external Owner for Super Agency
        /// </summary>
        protected RefOwnerModel ExternalOwnerForSuperAgency
        {
            get
            {
                return APOSessionParameter.ExternalOwnerForSuperAgency;
            }
        }

        /// <summary>
        /// Gets External Owner for Super Agency
        /// </summary>
        protected ParcelModel ExternalParcelForSuperAgency
        {
            get
            {
                return APOSessionParameter.ExternalParcelForSuperAgency;
            }
        }

        #endregion Properties
    }
}