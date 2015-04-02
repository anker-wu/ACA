#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaEmailText.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaEmailText.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;

namespace Accela.Web.Controls
{
    /// <summary>
    /// provide a email box to input email
    /// </summary>
    public class AccelaEmailText : AccelaTextBox
    {
        #region Fields

        /// <summary>
        /// The email validate
        /// </summary>
        private const string EMAILS_VALIDATE = "Emails";

        /// <summary>
        /// indicates validate email format or not.
        /// </summary>
        private bool _isIgnoreValidate = false;

        /// <summary>
        /// control ID which is compared to this element.
        /// </summary>
        private string _toCompare;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether validate email format or not.
        /// </summary>
        public bool IsIgnoreValidate
        {
            get
            {
                return _isIgnoreValidate;
            }

            set
            {
                _isIgnoreValidate = value;
            }
        }

        /// <summary>
        /// Gets or sets the control ID which is compared to this element.
        /// </summary>
        public string ToCompare
        {
            get
            {
                return _toCompare;
            }

            set
            {
                _toCompare = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            EmailValidator();
            CompareValidator();
            base.OnPreRender(e);
        }

        /// <summary>
        /// Compare Validator
        /// </summary>
        private void CompareValidator()
        {
            if (!string.IsNullOrEmpty(ToCompare))
            {
                CompareValidator compare = new CompareValidator();
                compare.ControlToValidate = ID;
                compare.ID = ID + "_com";
                compare.ControlToCompare = ToCompare;
                compare.Display = ValidatorDisplay.None;

                compare.ErrorMessage = LabelConvertUtil.GetTextByKey("ACA_AccelaEmailText_MatchErrorMessage", GetModuleName()).Replace("'", "\\'");
                compare.EnableClientScript = !ClientScript.Equals("false", StringComparison.InvariantCultureIgnoreCase);
                Controls.Add(compare);

                CreateValidatorCallbackExtender(compare.ID);
            }
        }

        /// <summary>
        /// Email Validator
        /// </summary>
        private void EmailValidator()
        {
            if (!_isIgnoreValidate)
            {
                RegularExpressionValidator regularVad = new RegularExpressionValidator();
                regularVad.ID = ID + "_reg_vad";
                regularVad.ControlToValidate = ID;

                if (this.Validate.IndexOf(EMAILS_VALIDATE) >= 0)
                {
                    regularVad.ValidationExpression = I18nEmailUtil.EmailsValidationExpression;
                }
                else
                {
                    regularVad.ValidationExpression = I18nEmailUtil.EmailValidationExpression;
                }

                regularVad.ErrorMessage = LabelConvertUtil.GetTextByKey("ACA_AccelaEmailText_ErrorMessage", GetModuleName()).Replace("'", "\\'");
                regularVad.Display = ValidatorDisplay.None;
                regularVad.SetFocusOnError = SetFocusOnError;
                Controls.Add(regularVad);

                CreateValidatorCallbackExtender(regularVad.ID);
            }
        }

        /// <summary>
        /// Gets the current runtime page's module name.
        /// if current page doesn't belong to any module, returns string.Empty
        /// </summary>
        /// <returns>module name.</returns>
        private string GetModuleName()
        {
            if (this.Page is IPage)
            {
                return (Page as IPage).GetModuleName();
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Methods
    }
}