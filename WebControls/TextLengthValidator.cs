#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: TextLengthValidator.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: TextLengthValidator.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Web.UI;
using System.Web.UI.WebControls;

namespace Accela.Web.Controls
{
    /// <summary>
    /// TextLengthValidator checks the length of text in a textbox to ensure it is below a 
    /// specified limit.
    /// </summary>
    public class TextLengthValidator : BaseValidator
    {
        #region Fields

        /// <summary>
        /// Whether we should attempt and display the characters entered string
        /// </summary>
        private bool _bDisplayLength = false;

        /// <summary>
        /// Maximum length allowed for the string
        /// </summary>
        private int _iMaxLength;  

        /// <summary>
        /// minimum length allowed for the string
        /// </summary>
        private int _iMinLength;

        /// <summary>
        /// Holds the original error text
        /// </summary>
        private string _sText; 

        /// <summary>
        /// Mask Characters from AccelaTextBox.Mask
        /// </summary>
        private string _sMask;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether we want to display the number of characters entered if over limit.
        /// </summary>
        public bool DisplayCharactersEntered
        {
            get
            {
                return _bDisplayLength;
            }

            set
            {
                _bDisplayLength = value;
            }
        }

        /// <summary>
        /// Gets or sets Mask Characters from AccelaTextBox.Mask
        /// </summary>
        public string Mask
        {
            get
            {
                if (this._sMask == null)
                {
                    return string.Empty;
                }

                return this._sMask;
            }

            set
            {
                this._sMask = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters that can be entered into the textbox.
        /// </summary>
        public int MaxLength
        {
            get
            {
                return _iMaxLength;
            }

            set
            {
                _iMaxLength = value;

                if (value == 0)
                {
                    _iMaxLength = int.MaxValue;
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters that can be entered into the textbox.
        /// </summary>
        public int MinLength
        {
            get
            {
                return _iMinLength;
            }

            set
            {
                _iMinLength = value;
            }
        }

        /// <summary>
        /// Gets or sets the text
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                if (_sText == null)
                {
                    _sText = value;
                }

                base.Text = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds the HTML attributes and styles that need to be rendered for the control
        /// to the specified System.Web.UI.HtmlTextWriter object.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render
        /// HTML content on the client.</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            if (RenderUplevel)
            {
                string clientID = this.ClientID;
                Page page = this.Page;

                AccelaTextBox.AddExpandoAttribute(this, page, clientID, "evaluationfunction", "checkLength", false);
                AccelaTextBox.AddExpandoAttribute(this, page, clientID, "maxLength", MaxLength.ToString(), false);
                AccelaTextBox.AddExpandoAttribute(this, page, clientID, "minLength", MinLength.ToString(), false);
                AccelaTextBox.AddExpandoAttribute(this, page, clientID, "mask", Mask.ToString(), false);
                AccelaTextBox.AddExpandoAttribute(this, page, clientID, "displayEntered", DisplayCharactersEntered.ToString(), false);
                AccelaTextBox.AddExpandoAttribute(this, page, clientID, "errorMessage", this.ErrorMessage, false);
            }
        }

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

            if (sValue.Length > _iMaxLength)
            {
                this.Text = _sText;

                if (DisplayCharactersEntered)
                {
                    this.Text = string.Format(LabelConvertUtil.GetTextByKey("ACA_TextLengthValidator_InvalidMessage", string.Empty), this.Text, sValue.Length.ToString());
                }

                return false;
            }

            return true;
        }

        #endregion Methods
    }
}