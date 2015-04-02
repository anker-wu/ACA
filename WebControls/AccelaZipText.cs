#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaZipText.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaZipText.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI.WebControls;

using Accela.ACA.Common.Util;

namespace Accela.Web.Controls
{
    /// <summary>
    /// provide a zip box to input zip
    /// </summary>
    public class AccelaZipText : AccelaMaskText
    {
        #region Fields

        /// <summary>
        /// indicates validate zip format or not.
        /// </summary>
        private bool _isIgnoreValidate = false;

        /// <summary>
        /// max length
        /// </summary>
        private int _maxLength;

        /// <summary>
        /// zip mask
        /// </summary>
        private string _zipMask = string.Empty;

        /// <summary>
        /// zip mask from AA
        /// </summary>
        private string _zipMaskFromAA = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether validate zip format or not.
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
        /// Gets Zip Mask
        /// </summary>
        public override string Mask
        {
            get
            {
                return _zipMask;
            }

            set
            {
                _zipMask = value;
            }
        }

        /// <summary>
        /// Gets or sets the zip mask from AA.
        /// </summary>
        /// <value>The zip mask from AA.</value>
        public string ZipMaskFromAA
        {
            get
            {
                return _zipMaskFromAA;
            }

            set
            {
                _zipMaskFromAA = value;
            }
        }

        /// <summary>
        /// Gets No Mask Type
        /// </summary>
        public override AjaxControlToolkit.MaskedEditType MaskType
        {
            get
            {
                return AjaxControlToolkit.MaskedEditType.None;
            }
        }

        /// <summary>
        /// Gets or sets maximum length of the text value of this element.
        /// </summary>
        public int ZipMaxLength
        {
            get
            {
                return _maxLength;
            }

            set
            {
                _maxLength = value;
            }
        }

        /// <summary>
        /// Gets or sets text, override the text property to add a rule when now it is zip control
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                if (value != null)
                {
                    base.Text = value;
                }
                else
                {
                    base.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is use zip.
        /// </summary>
        /// <value><c>true</c> if this instance is use zip; otherwise, <c>false</c>.</value>
        public bool IsUseZip { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether focus is set to the control specified by the System.Web.UI.WebControls.BaseValidator.ControlToValidate property when validation fails.
        /// </summary>
        public override bool SetFocusOnError
        {
            get
            {
                return false;
            }

            set
            {
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clear current control value
        /// </summary>
        public override void ClearValue()
        {
            this.SetZipFromAA("0");
            base.ClearValue();
        }

        /// <summary>
        /// when saving the zip,call this method to get the zip from the control
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <param name="mask">The mask.</param>
        /// <returns>the zip save to DB.</returns>
        public string GetZip(string countryCode, string mask)
        {
            string text = Text.Trim();

            if (!string.IsNullOrEmpty(text))
            {
                string zipFromAA = GetZipFromHiddenField();

                if (!string.IsNullOrEmpty(zipFromAA) && zipFromAA != "0")
                {
                    text = zipFromAA;
                }
                else
                {
                    text = I18nZipUtil.UnFormatZipByMask(mask, text);
                }
               
                text = text.Trim();
            }

            return text;
        }

        /// <summary>
        /// Set zip reference from AA to hidden-field control
        /// </summary>
        /// <param name="zip">zip code from AA</param>
        public void SetZipFromAA(string zip)
        {
            HiddenField hf = (HiddenField)this.FindControl(this.ID + "_ZipFromAA");

            if (hf != null)
            {
                hf.Value = zip;
            }
        }

        /// <summary>
        /// Adds HTML attributes and styles that need to be rendered to the specified
        /// System.Web.UI.HtmlTextWriter instance.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render
        /// HTML content on the client.</param>
        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            if (!AccelaControlRender.IsAdminRender(this))
            {
                writer.AddAttribute("onblur", "ClearSplitChar(this, true);ToUpperCase(this);");

                if (IsUseZip)
                {
                    writer.AddAttribute("isusezip", "true");
                }
                else
                {
                    writer.AddAttribute("isusezip", "false");
                }
            }
        }

        /// <summary>
        /// Validate the illegal character in the text-box field.
        /// </summary>
        /// <returns>RegularExpressionValidator object</returns>
        protected override RegularExpressionValidator ExpressionValidator()
        {
            if (!_isIgnoreValidate)
            {
                //set the mask to validation message, to guide the user input.
                AddionalExpressionValidateMessage = string.IsNullOrEmpty(_zipMaskFromAA) ? string.Empty : " (" + _zipMaskFromAA + ")";
                return base.ExpressionValidator();
            }

            return null;
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.Initial event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            //the code does not using any way?
            this.CustomValidationMessageKey = "ACA_AccelaMaskText_InvalidValueMessage";
            HiddenField hfZipFromAA = new HiddenField();
            hfZipFromAA.ID = this.ID + "_ZipFromAA";
            hfZipFromAA.Value = "0";
            Controls.Add(hfZipFromAA);
            HiddenField hfZipMask = new HiddenField();
            hfZipMask.ID = this.ID + "_zipMask";
            hfZipMask.Value = Mask;
            Controls.Add(hfZipMask);

            if (!this.Validate.ToLowerInvariant().Contains("maxlength"))
            {
                this.Validate += ";maxlength";
            }

            this.ZipMaxLength = 10;

            //ajax maskedit control, custom mask.
            Filtered = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            base.OnInit(e);
        }

        /// <summary>
        /// Required Validator, if not use zip, don't create the require validator.
        /// </summary>
        protected override void RequiredValidator()
        {
            if (IsUseZip)
            {
                base.RequiredValidator();
            }
            else
            {
                Validate = Validate.Replace("required", string.Empty);
            }
        }

        /// <summary>
        /// Get the zip from the hidden field
        /// </summary>
        /// <returns>
        /// "0" means zip code come from user's input;when null we need not care it;
        /// other value means zip code come from AA and it is the zip code value;
        /// </returns>
        private string GetZipFromHiddenField()
        {
            string prefix = this.ClientID.Replace(this.ID, string.Empty).Replace("_", "$");
            string value = Page.Request.Form[prefix + this.ID + "_ZipFromAA"];

            if (value == null)
            {
                value = Page.Request.Form[this.ClientID + "_ZipFromAA"];
            }

            return value;
        }

        #endregion Methods
    }
}