#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: BreadCrumbParmsInfo.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: BreadCrumbParmsInfo.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;

namespace Accela.Web.Controls
{
    /// <summary>
    /// parameters for breadcrumb
    /// </summary>
    [Serializable]
    public class BreadCrumbParmsInfo
    {
        #region Fields

        /// <summary>
        /// store the cap name.
        /// </summary>
        private string _capName = string.Empty;

        /// <summary>
        /// store the indication that the fee form visible.
        /// </summary>
        private bool _checkFeeFormVisible = false;

        /// <summary>
        /// store the indication that has fee or not.
        /// </summary>
        private string _hasFee = string.Empty;

        /// <summary>
        /// store the indication that having fee estimate or not.
        /// </summary>
        private bool _hasFeeEstimate = false;

        /// <summary>
        /// store the indication that having fee form or not.
        /// </summary>
        private bool _hasFeeForm = false;

        /// <summary>
        /// store the indication that having License list or not.
        /// </summary>
        private bool _hasLicenseList = false;

        /// <summary>
        /// store the indication that is cap fee page or not.
        /// </summary>
        private bool _isCapFeePage = false;

        /// <summary>
        /// store the indication that current page is confirm page from ShoppingCart or not.
        /// </summary>
        private bool _isConfirmPageFromShoppingCart = false;

        /// <summary>
        /// store the indication that is convert to app or not.
        /// </summary>
        private bool _isConvertToApp = false;

        /// <summary>
        /// store the indication that is first load fee form or not.
        /// </summary>
        private bool _isFirstLoadFeeForm = true;

        /// <summary>
        /// store the indication that is is hidden application form or not.
        /// </summary>
        private bool _isHideApplicationForm = false;

        /// <summary>
        /// store the indication that current agency is super or not.
        /// </summary>
        private bool _isSuperAgency = false;

        /// <summary>
        /// store the indication that label key for last Step or not.
        /// </summary>
        private string _labelKeyForLastStep;

        /// <summary>
        /// store the last index.
        /// </summary>
        private int _lastIndex = 1;

        /// <summary>
        /// store the request where come from.
        /// </summary>
        private PageFrom _permitType;

        /// <summary>
        /// store the URLs that breadcrumbs.
        /// </summary>
        private Hashtable _urls = new Hashtable();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the current cap name.
        /// </summary>
        public string CapName
        {
            get
            {
                return _capName;
            }

            set
            {
                _capName = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether visible value if first set lastIndex to 1,set CheckFeeFormVisible to true
        /// </summary>
        public bool CheckFeeFormVisible
        {
            get
            {
                return _checkFeeFormVisible;
            }

            set
            {
                _checkFeeFormVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the indication 'yes' or 'no'.
        /// </summary>
        public string HasFee
        {
            get
            {
                return _hasFee;
            }

            set
            {
                _hasFee = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether has fee estimate or not.
        /// </summary>
        public bool HasFeeEstimate
        {
            get
            {
                return _hasFeeEstimate;
            }

            set
            {
                _hasFeeEstimate = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current page has fee or not.
        /// </summary>
        public bool HasFeeForm
        {
            get
            {
                return _hasFeeForm;
            }

            set
            {
                _hasFeeForm = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether has license list or not.
        /// </summary>
        public bool HasLicenseList
        {
            get
            {
                return _hasLicenseList;
            }

            set
            {
                _hasLicenseList = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether current page is cap fee or not.
        /// </summary>
        public bool IsCapFeePage
        {
            get
            {
                return _isCapFeePage;
            }

            set
            {
                _isCapFeePage = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the request come from shopping cart or not.
        /// </summary>
        public bool IsConfirmPageFromShoppingCart
        {
            get
            {
                return _isConfirmPageFromShoppingCart;
            }

            set
            {
                _isConfirmPageFromShoppingCart = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if  IsConvertToApp=Y ,that means Obtain Fee Estimate
        /// </summary>
        public bool IsConvertToApp
        {
            get
            {
                return _isConvertToApp;
            }

            set
            {
                _isConvertToApp = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is first load fee form or not.
        /// </summary>
        public bool IsFirstLoadFeeForm
        {
            get
            {
                return _isFirstLoadFeeForm;
            }

            set
            {
                _isFirstLoadFeeForm = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if configured only fee estimated,hide application form
        /// </summary>
        public bool IsHideApplicationForm
        {
            get
            {
                return _isHideApplicationForm;
            }

            set
            {
                _isHideApplicationForm = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is super agency or not.
        /// </summary>
        public bool IsSuperAgency
        {
            get
            {
                return _isSuperAgency;
            }

            set
            {
                _isSuperAgency = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the label key by user input
        /// if null or empty,use default label key "per_breadcrumb_PermitIssuance"
        /// </summary>
        public string LabelKeyForLastStep
        {
            get
            {
                return _labelKeyForLastStep;
            }

            set
            {
                _labelKeyForLastStep = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the last index user click.
        /// </summary>
        public int LastIndex
        {
            get
            {
                return _lastIndex;
            }

            set
            {
                _lastIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether expand all steps or not
        /// </summary>
        public bool IsExpendAllStep
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current permit type.
        /// </summary>
        public PageFrom PageFrom
        {
            get
            {
                return _permitType;
            }

            set
            {
                _permitType = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the breadcrumb url.
        /// </summary>
        public Hashtable Urls
        {
            get
            {
                return _urls;
            }

            set
            {
                _urls = value;
            }
        }

        #endregion Properties
    }
}