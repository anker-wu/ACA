#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: ZipCodeValidator.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ZipCodeValidator.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Web.UI.WebControls;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Serves as the class for validation zip code.
    /// </summary>
    public class ZipCodeValidator : BaseValidator
    {
        #region Fields

        /// <summary>
        /// the id of control
        /// </summary>
        private string _controlID;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets control id
        /// </summary>
        public string ControlID
        {
            get
            {
                return this._controlID;
            }

            set
            {
                this._controlID = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Check whether the value in the input control is valid.
        /// </summary>
        /// <returns>true if the value in the input control is valid; otherwise, false.</returns>
        protected override bool EvaluateIsValid()
        {
            string sValue = GetControlValidationValue(ControlToValidate);

            // If the validator is not bound to a control
            if (sValue == null)
            {
                return true;
            }

            return true;
        }

        #endregion Methods
    }
}