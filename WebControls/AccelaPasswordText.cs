#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaPasswordText.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaPasswordText.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI.WebControls;

using Accela.ACA.Common;

namespace Accela.Web.Controls
{
    /// <summary>
    /// provide a password text box to input password
    /// </summary>
    public class AccelaPasswordText : AccelaTextBox
    {
        #region Fields

        /// <summary>
        /// Enable strength validate or not
        /// </summary>
        private bool _enableStrengthValidate;

        /// <summary>
        /// control ID which is compared to this element
        /// </summary>
        private string _toCompare;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether need password strength validate.
        /// </summary>
        public bool EnableStrengthValidate
        {
            get
            {
                return _enableStrengthValidate;
            }

            set
            {
                _enableStrengthValidate = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control ID which is compared to this element.
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
            base.OnPreRender(e);

            TextMode = TextBoxMode.Password;

            if (EnableStrengthValidate)
            {
                Label label1 = new Label();
                label1.ID = ID + "_HelpLabel";
                label1.CssClass = "form_sublabel";
                label1.Visible = false;
                Controls.Add(label1);

                Label label2 = new Label();
                label2.ID = ID + "_PasswordStrength";
                Controls.Add(label2);
            }

            CompareValidator();
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
                compare.ErrorMessage = LabelConvertUtil.GetTextByKey("ACA_AccelaPasswordText_MatchErrorMessage", GetModuleName()).Replace("'", "\\'");
                compare.EnableClientScript = !ClientScript.Equals("false", StringComparison.InvariantCultureIgnoreCase);
                Controls.Add(compare);

                CreateValidatorCallbackExtender(compare.ID);
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